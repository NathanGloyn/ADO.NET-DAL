using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Xml;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.SqlServer
{
    public class SqlCommands : ISqlCommands
    {
        private readonly SqlConnection _currentConnection;
        private readonly SqlTransaction _currentTransaction;
        private readonly int _commandTimeOut;


        public SqlCommands(SqlConnection currentConnection, SqlTransaction currentTransaction, int commandTimeOut)
        {
            if (currentConnection == null) throw new ArgumentNullException("currentConnection");
            if (currentTransaction == null) throw new ArgumentNullException("currentTransaction");
            _currentConnection = currentConnection;
            _currentTransaction = currentTransaction;
            _commandTimeOut = commandTimeOut;
        }

        /// <summary>
        /// Executes a command that does not return a query
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        public int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, null);
        }

        /// <summary>
        /// Executes a command that does not return a query
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        public int ExecuteNonQuery(string commandText, params DbParameter[] parameters)
        {
            DbCommand cmd =null;
            int rowsAffected = 0;
            try
            {
                rowsAffected = ExecuteNonQuery(out cmd, commandText, parameters);
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Dispose();
            }

            return rowsAffected;
        }

        /// <summary>
        /// Executes a command that does not return a query
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>/// 
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>DbCommand containing the command executed</returns>
        public int ExecuteNonQuery(out DbCommand cmd, string commandText, params DbParameter[] parameters)
        {
            int rowsAffected = 0;

            SqlCommand cmdExecute = BuildCommand(commandText, parameters);

            rowsAffected = cmdExecute.ExecuteNonQuery();

            cmd = cmdExecute;

            return rowsAffected;
        }

        /// <summary>
        /// Executes a command that returns a single value
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <returns>Object holding result of execution of database</returns>
        public object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, null);
        }

        /// <summary>
        /// Executes a command that returns a single value
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>Object holding result of execution of database</returns>
        public object ExecuteScalar(string commandText, params DbParameter[] parameters)
        {
            DbCommand cmd;
            object result = ExecuteScalar(out cmd, commandText, parameters);
            cmd.Parameters.Clear();
            cmd.Dispose();

            return result;
        }

        /// <summary>
        /// Executes a command that returns a single value
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>Object holding result of execution of database</returns>
        public object ExecuteScalar(out DbCommand cmd, string commandText, params DbParameter[] parameters)
        {
            // Find the command to execute
            object data = null;

            SqlCommand cmdScalar = BuildCommand(commandText, parameters);

            data = cmdScalar.ExecuteScalar();
            
            cmd = cmdScalar;
            return data;
        }

        /// <summary>
        /// Executes a command and returns a data reader
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <returns>SqlDataReader allowing access to results from command</returns>
        public DbDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(commandText, null);
        }

        /// <summary>
        /// Executes a command and returns a data reader
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>SqlDataReader allowing access to results from command</returns>
        public DbDataReader ExecuteReader(string commandText, params DbParameter[] parameters)
        {
            SqlDataReader reader = null;

            using (SqlCommand cmdReader = new SqlCommand(commandText, _currentConnection))
            {
                cmdReader.CommandType = CommandType.StoredProcedure;
                cmdReader.Transaction = _currentTransaction;

                if (parameters != null && parameters.Length > 0)
                    cmdReader.Parameters.AddRange(parameters);

                reader = cmdReader.ExecuteReader(CommandBehavior.CloseConnection);
            }

            return reader;
        }

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        public DataTable ExecuteDataTable(string commandText)
        {
            return ExecuteDataTable(commandText, null);
        }

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        public DataTable ExecuteDataTable(string commandText, params DbParameter[] parameters)
        {
            DbCommand cmd =null;
            DataTable results;
            try
            {
                results = ExecuteDataTable(out cmd, commandText, parameters);
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Dispose();
            }

            return results;
        }

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        public DataTable ExecuteDataTable(out DbCommand cmd, string commandText, params DbParameter[] parameters)
        {
            SqlCommand cmdDataTable = BuildCommand(commandText, parameters);

            DataTable result = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter(cmdDataTable))
            {
                da.Fill(result);
            }

            cmd = cmdDataTable;
            return result;
        }

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        public DataSet ExecuteDataSet(string commandText)
        {
            return ExecuteDataSet(commandText, null);
        }

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        public DataSet ExecuteDataSet(string commandText, params DbParameter[] parameters)
        {
            DbCommand cmd;
            DataSet results = ExecuteDataSet(out cmd, commandText, parameters);
            cmd.Parameters.Clear();
            cmd.Dispose();

            return results;
        }

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        public DataSet ExecuteDataSet(out DbCommand cmd, string commandText, params DbParameter[] parameters)
        {
            SqlCommand cmdDataSet = BuildCommand(commandText, parameters);

            DataSet result = new DataSet();

            using (SqlDataAdapter adapter = new SqlDataAdapter(cmdDataSet))
            {
                adapter.Fill(result);
            }

            cmd = cmdDataSet;
            return result;
        }

        /// <summary>
        /// Executes a command and returns an XML reader.
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <returns>An instance of XmlReader pointing to the stream of xml returned</returns>
        public XmlReader ExecuteXmlReader(string commandText)
        {
            return ExecuteXmlReader(commandText, null);
        }

        /// <summary>
        /// Executes a command and returns an XML reader.
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>An instance of XmlReader pointing to the stream of xml returned</returns>
        public XmlReader ExecuteXmlReader(string commandText, params DbParameter[] parameters)
        {
            DbCommand cmd;
            XmlReader result = ExecuteXmlReader(out cmd, commandText, parameters);
            cmd.Parameters.Clear();
            cmd.Dispose();

            return result;
        }

        /// <summary>
        /// Executes a command and returns an XML reader.
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>An instance of XmlReader pointing to the stream of xml returned</returns>
        public XmlReader ExecuteXmlReader(out DbCommand cmd, string commandText, params DbParameter[] parameters)
        {
            SqlCommand cmdXmlReader = BuildCommand(commandText, parameters);

            XmlReader outputReader = cmdXmlReader.ExecuteSafeXmlReader();
            cmd = cmdXmlReader;
            return outputReader;
        }

        /// <summary>
        /// Builds a SqlCommand to execute
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">Param array of DbParameter objects to use with command</param>
        /// <returns>SqlCommand object ready for use</returns>
        private SqlCommand BuildCommand(string storedProcedureName, params DbParameter[] parameters)
        {
            SqlCommand newCommand = new SqlCommand(storedProcedureName, _currentConnection)
            {
                Transaction = _currentTransaction,
                CommandType = CommandType.StoredProcedure
            };

            if (_commandTimeOut > 0)
            {
                newCommand.CommandTimeout = _commandTimeOut;
            }

            if (parameters != null)
                newCommand.Parameters.AddRange(parameters);

            return newCommand;
        }

    }
}