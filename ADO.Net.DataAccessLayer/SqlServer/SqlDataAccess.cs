using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.SqlServer
{
    /// <summary>
    /// Base class used by all concrete classes in DataAccessLayer
    /// </summary>
    public class SqlDataAccess : IDataAccess,ITransactions,IParameterCreation
    {
        private SqlConnection databaseConnection = null;
        private DbTransaction currentTransaction = null;
        private string connectionString;
        private readonly IParameterCreation _parameterFactory;

        /// <summary>
        /// Allows child classes to pass the connection string to be used for the
        /// connection during construction
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlDataAccess(string connectionString, IParameterCreation parameterFactory)
        {
            if (parameterFactory == null) throw new ArgumentNullException("parameterFactory");
            this.connectionString = connectionString;
            _parameterFactory = parameterFactory;
        }

        /// <summary>
        /// Timeout setting to use when executing commands
        /// </summary>
        public int CommandTimeOut { get; set; }



        /// <summary>
        /// Determines if the connection is currently in a transaction and
        /// close the connection if not.
        /// </summary>
        private void SafelyCloseConnection()
        {
            if (currentTransaction == null)
            {
                databaseConnection.Close();
                databaseConnection.Dispose();
            }
        }

        private void SafelyOpenConnection()
        {
            if (databaseConnection == null || databaseConnection.State != ConnectionState.Open)
            {
                databaseConnection = new SqlConnection(connectionString);
                databaseConnection.Open();
            }
        }

        #region Execute Methods

        /// <summary>
        /// Executes a command that does not return a query
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        public int ExecuteNonQuery(string storedProcedureName)
        {
            return ExecuteNonQuery(storedProcedureName, null);
        }

        /// <summary>
        /// Executes a command that does not return a query
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>/// 
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>DbCommand containing the command executed</returns>
        public int ExecuteNonQuery(out DbCommand cmd, string storedProcedureName, params DbParameter[] parameters)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmdExecute = BuildCommand(storedProcedureName, parameters);

                try
                {
                    connection.Open();
                    rowsAffected = cmdExecute.ExecuteNonQuery();
                }
                finally
                {
                    SafelyCloseConnection();
                }

                cmd = cmdExecute; 
            }

            return rowsAffected;
        }

        /// <summary>
        /// Executes a command that does not return a query
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        public int ExecuteNonQuery(string storedProcedureName, params DbParameter[] parameters)
        {
            DbCommand cmd;
            int rowsAffected = ExecuteNonQuery(out cmd, storedProcedureName, parameters);
            cmd.Parameters.Clear();
            cmd.Dispose();

            return rowsAffected;
        }

        /// <summary>
        /// Executes a command that returns a single value
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <returns>Object holding result of execution of database</returns>
        public object ExecuteScalar(string storedProcedureName)
        {
            return ExecuteScalar(storedProcedureName, null);
        }

        /// <summary>
        /// Executes a command that returns a single value
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>Object holding result of execution of database</returns>
        public object ExecuteScalar(string storedProcedureName, params DbParameter[] parameters)
        {
            DbCommand cmd;
            object result = ExecuteScalar(out cmd, storedProcedureName, parameters);
            cmd.Parameters.Clear();
            cmd.Dispose();

            return result;
        }

        /// <summary>
        /// Executes a command that returns a single value
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>Object holding result of execution of database</returns>
        public object ExecuteScalar(out DbCommand cmd, string storedProcedureName, params DbParameter[] parameters)
        {
            SqlCommands commands = CreateCommands();

            object data = commands.ExecuteScalar(out cmd, storedProcedureName, parameters);

            SafelyCloseConnection();

            return data;
        }

        /// <summary>
        /// Executes a command and returns a data reader
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <returns>SqlDataReader allowing access to results from command</returns>
        public DbDataReader ExecuteReader(string storedProcedureName)
        {
            return ExecuteReader(storedProcedureName, null);
        }

        /// <summary>
        /// Executes a command and returns a data reader
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>SqlDataReader allowing access to results from command</returns>
        public DbDataReader ExecuteReader(string storedProcedureName, params DbParameter[] parameters)
        {
            SqlDataReader reader = null;
            SafelyOpenConnection();

            using (SqlCommand cmdReader = new SqlCommand(storedProcedureName, databaseConnection))
            {
                cmdReader.CommandType = CommandType.StoredProcedure;
                cmdReader.Transaction = (SqlTransaction)currentTransaction;

                if (parameters != null && parameters.Length > 0)
                    cmdReader.Parameters.AddRange(parameters);

                reader = cmdReader.ExecuteReader(CommandBehavior.CloseConnection);
            }

            return reader;
        }

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        public DataTable ExecuteDataTable(string storedProcedureName)
        {
            return ExecuteDataTable(storedProcedureName, null);
        }

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        public DataTable ExecuteDataTable(string storedProcedureName, params DbParameter[] parameters)
        {
            DbCommand cmd;
            DataTable results = ExecuteDataTable(out cmd, storedProcedureName, parameters);
            cmd.Parameters.Clear();
            cmd.Dispose();

            return results;
        }

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        public DataTable ExecuteDataTable(out DbCommand cmd, string storedProcedureName, params DbParameter[] parameters)
        {
            SafelyOpenConnection();
            SqlCommand cmdDataTable = BuildCommand(storedProcedureName, parameters);

            DataTable result = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter(cmdDataTable))
            {
                try
                {
                    da.Fill(result);
                }
                finally
                {
                    SafelyCloseConnection();
                }
            }

            cmd = cmdDataTable;
            return result;
        }

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        public DataSet ExecuteDataSet(string storedProcedureName)
        {
            return ExecuteDataSet(storedProcedureName, null);
        }

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        public DataSet ExecuteDataSet(string storedProcedureName, params DbParameter[] parameters)
        {
            DbCommand cmd;
            DataSet results = ExecuteDataSet(out cmd, storedProcedureName, parameters);
            cmd.Parameters.Clear();
            cmd.Dispose();

            return results;
        }

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        public DataSet ExecuteDataSet(out DbCommand cmd, string storedProcedureName, params DbParameter[] parameters)
        {
            SafelyOpenConnection();
            SqlCommand cmdDataSet = BuildCommand(storedProcedureName, parameters);

            DataSet result = new DataSet();

            using (SqlDataAdapter adapter = new SqlDataAdapter(cmdDataSet))
            {
                try
                {
                    adapter.Fill(result);
                }
                finally
                {
                    SafelyCloseConnection();
                }
            }

            cmd = cmdDataSet;
            return result;
        }

        /// <summary>
        /// Executes a command and returns an XML reader.
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <returns>An instance of XmlReader pointing to the stream of xml returned</returns>
        public XmlReader ExecuteXmlReader(string storedProcedureName)
        {
           return ExecuteXmlReader(storedProcedureName, null);
        }

        /// <summary>
        /// Executes a command and returns an XML reader.
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>An instance of XmlReader pointing to the stream of xml returned</returns>
        public XmlReader ExecuteXmlReader(string storedProcedureName, params DbParameter[] parameters)
        {
            DbCommand cmd;
            XmlReader result = ExecuteXmlReader(out cmd, storedProcedureName, parameters);
            cmd.Parameters.Clear();
            cmd.Dispose();

            return result;
        }

        /// <summary>
        /// Executes a command and returns an XML reader.
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>An instance of XmlReader pointing to the stream of xml returned</returns>
        public XmlReader ExecuteXmlReader(out DbCommand cmd, string storedProcedureName, params DbParameter[] parameters)
        {
            SafelyOpenConnection();

            SqlCommand cmdXmlReader = BuildCommand(storedProcedureName, parameters);

            XmlReader outputReader = cmdXmlReader.ExecuteSafeXmlReader();
            cmd = cmdXmlReader;
            return outputReader;
        }

        #endregion

        /// <summary>
        /// Starts a transaction on the current connection
        /// </summary>
        public void BeginTransaction()
        {
            SafelyOpenConnection();
            currentTransaction = databaseConnection.BeginTransaction();
        }

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        public void CommitTransaction()
        {
            if (currentTransaction != null)
            {
                currentTransaction.Commit();
                currentTransaction = null;
                SafelyCloseConnection();
            }
        }

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        public void RollbackTransaction()
        {
            if (currentTransaction != null)
            {
                currentTransaction.Rollback();
                currentTransaction = null;
                SafelyCloseConnection();
            }
        }

        /// <summary>
        /// Builds a SqlCommand to execute
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">Param array of DbParameter objects to use with command</param>
        /// <returns>SqlCommand object ready for use</returns>
        private SqlCommand BuildCommand(string storedProcedureName, params DbParameter[] parameters)
        {
            SqlCommand newCommand = new SqlCommand(storedProcedureName, databaseConnection) 
            {   
                Transaction = (SqlTransaction)currentTransaction,
                CommandType = CommandType.StoredProcedure 
            };

            if (CommandTimeOut > 0)
            {
                newCommand.CommandTimeout = CommandTimeOut;
            }

            if (parameters != null)
                newCommand.Parameters.AddRange(parameters);

            return newCommand;
        }

        private SqlCommands CreateCommands()
        {
            SafelyOpenConnection();
            return new SqlCommands(databaseConnection, currentTransaction, CommandTimeOut);
        }

        public DbParameter Create(string paramName, DbType paramType, object value, ParameterDirection direction)
        {
            return _parameterFactory.Create(paramName, paramType, value, direction);
        }

        public DbParameter Create(string paramName, DbType paramType, ParameterDirection direction)
        {
            return _parameterFactory.Create(paramName, paramType, direction);
        }

        public DbParameter Create(string paramName, DbType paramType, object value)
        {
            return _parameterFactory.Create(paramName, paramType, value);
        }

        public DbParameter Create(string paramName, DbType paramType, object value, int size)
        {
            return _parameterFactory.Create(paramName, paramType, value, size);
        }

        public DbParameter Create(string paramName, DbType paramType, object value, int size, byte precision, ParameterDirection direction)
        {
            return _parameterFactory.Create(paramName, paramType, value, size, direction);
        }

        public DbParameter Create(string paramName, DbType paramType, object value, int size, byte precision)
        {
            return  _parameterFactory.Create(paramName, paramType, value, size, precision);
        }

        public DbParameter Create(string paramName, DbType paramType, object value, int size, ParameterDirection direction)
        {
            return _parameterFactory.Create(paramName, paramType, value,size, direction);
        }
    }
}
