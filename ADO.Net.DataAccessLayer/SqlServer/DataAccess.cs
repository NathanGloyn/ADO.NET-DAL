using System;
using System.Data.Common;
using System.Data;
using System.Xml;
using DataAccessLayer.Core;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.SqlServer
{
    /// <summary>
    /// Base class used by all concrete classes in DataAccessLayer
    /// </summary>
    public class DataAccess : IDataAccess
    {
        private IConnection connection = null;
        private readonly IParameterCreation _parameterFactory;
        private readonly ITransactionControl _transactionControl;

        /// <summary>
        /// Allows child classes to pass the connection string to be used for the
        /// connection during construction
        /// </summary>
        /// <param name="connectionString"></param>
        public DataAccess(string connectionString, IParameterCreation parameterFactory)
        {
            if (parameterFactory == null) throw new ArgumentNullException("parameterFactory");
            connection = new Connection(connectionString);
            _parameterFactory = parameterFactory;
            _transactionControl = new TransactionControl(connection);
        }

        /// <summary>
        /// Timeout setting to use when executing commands
        /// </summary>
        public int CommandTimeOut { get; set; }

        public IParameterCreation ParameterFactory
        {
            get { return _parameterFactory; }
        }

        public ITransactionControl Transactions
        {
            get { return _transactionControl; }
        }

        private Commands CreateCommands()
        {
            return new Commands(connection, _transactionControl.CurrentTransaction, CommandTimeOut);
        }

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

            Commands commands = CreateCommands();
            return commands.ExecuteNonQuery(out cmd, storedProcedureName, parameters);

        }

        /// <summary>
        /// Executes a command that does not return a query
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        public int ExecuteNonQuery(string storedProcedureName, params DbParameter[] parameters)
        {
            Commands commands = CreateCommands();
            return commands.ExecuteNonQuery(storedProcedureName, parameters);
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
            Commands commands = CreateCommands();
            return commands.ExecuteScalar(storedProcedureName, parameters);
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
                Commands commands = CreateCommands();

                return commands.ExecuteScalar(out cmd, storedProcedureName, parameters);
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
            Commands commands = CreateCommands();
            return commands.ExecuteReader(storedProcedureName, parameters);
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
            try
            {
                Commands commands = CreateCommands();
                return commands.ExecuteDataTable(storedProcedureName, parameters);
            }
            finally
            {
                connection.Close();
            }
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
            try
            {
                Commands commands = CreateCommands();
                return commands.ExecuteDataTable(out cmd, storedProcedureName, parameters);
            }
            finally
            {
                connection.Close();
            }
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
            try
            {
                Commands commands = CreateCommands();
                return commands.ExecuteDataSet(storedProcedureName, parameters);
            }
            finally
            {
                connection.Close();
            }
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
            try
            {
                Commands commands = CreateCommands();
                return commands.ExecuteDataSet(out cmd, storedProcedureName, parameters);
            }
            finally
            {
                connection.Close();
            }
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
            Commands commands = CreateCommands();
            return commands.ExecuteXmlReader(storedProcedureName, parameters);
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
            Commands commands = CreateCommands();
            return commands.ExecuteXmlReader(out cmd, storedProcedureName, parameters);
        }


    }
}
