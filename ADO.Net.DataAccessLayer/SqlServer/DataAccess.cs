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
        private IConnection connection;
        private readonly IParameterCreation parameterFactory;
        private readonly ITransactionControl transactionControl;
        
        /// <summary>
        /// Allows child classes to pass the connection string to be used for the
        /// connection during construction
        /// </summary>
        /// <param name="connectionString"></param>
        public DataAccess(string connectionString, IParameterCreation parameterFactory)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
            if (parameterFactory == null) throw new ArgumentNullException("parameterFactory");

            connection = new Connection(connectionString);
            this.parameterFactory = parameterFactory;
            transactionControl = new TransactionControl(connection);
        }

        /// <summary>
        /// Timeout setting to use when executing commands
        /// </summary>
        public int CommandTimeOut { get; set; }

        public IParameterCreation ParameterFactory
        {
            get { return parameterFactory; }
        }

        public ITransactionControl Transactions
        {
            get { return transactionControl; }
        }

        /// <summary>
        /// Executes a command that does not return a query
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>/// 
        /// <param name="commandText">command text to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>DbCommand containing the command executed</returns>
        public int ExecuteNonQuery(out DbCommand cmd, string commandText, params DbParameter[] parameters)
        {
            Commands commands = CreateCommand();
            return commands.ExecuteNonQuery(out cmd, commandText, parameters);
        }

        /// <summary>
        /// Executes a command that does not return a query
        /// </summary>
        /// <param name="commandText">command text to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        public int ExecuteNonQuery(string commandText, params DbParameter[] parameters)
        {
            return RunCommand(c => c.ExecuteNonQuery(commandText, parameters));
        }

        /// <summary>
        /// Executes a command that returns a single value
        /// </summary>
        /// <param name="commandText">command text to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>Object holding result of execution of database</returns>
        public object ExecuteScalar(string commandText, params DbParameter[] parameters)
        {
            return RunCommand(c => c.ExecuteScalar(commandText,parameters));
        }

        /// <summary>
        /// Executes a command that returns a single value
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="commandText">command text to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>Object holding result of execution of database</returns>
        public object ExecuteScalar(out DbCommand cmd, string commandText, params DbParameter[] parameters)
        {
            Commands commands = CreateCommand();
            return commands.ExecuteScalar(out cmd, commandText, parameters);
        }

        /// <summary>
        /// Executes a command and returns a data reader
        /// </summary>
        /// <param name="commandText">command text to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>SqlDataReader allowing access to results from command</returns>
        public DbDataReader ExecuteReader(string commandText, params DbParameter[] parameters)
        {
            Commands commands = CreateCommand();
            return commands.ExecuteReader(commandText, parameters);
        }


        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="commandText">command text to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing command</returns>
        public DataTable ExecuteDataTable(string commandText, params DbParameter[] parameters)
        {
            return RunCommand(c => c.ExecuteDataTable(commandText, parameters));
        }

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="commandText">command text to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing command</returns>
        public DataTable ExecuteDataTable(out DbCommand cmd, string commandText, params DbParameter[] parameters)
        {
            Commands commands = CreateCommand();
            return commands.ExecuteDataTable(out cmd, commandText, parameters);
        }

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="commandText">command text to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing command</returns>
        public DataSet ExecuteDataSet(string commandText, params DbParameter[] parameters)
        {
            return RunCommand(c => c.ExecuteDataSet(commandText, parameters));
        }

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="commandText">command text to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing command</returns>
        public DataSet ExecuteDataSet(out DbCommand cmd, string commandText, params DbParameter[] parameters)
        {
            Commands commands = CreateCommand();
            return commands.ExecuteDataSet(out cmd, commandText, parameters);
        }


        /// <summary>
        /// Executes a command and returns an XML reader.
        /// </summary>
        /// <param name="commandText">command text to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>An instance of XmlReader pointing to the stream of xml returned</returns>
        public XmlReader ExecuteXmlReader(string commandText, params DbParameter[] parameters)
        {
            return RunCommand(c => c.ExecuteXmlReader(commandText, parameters));
        }

        /// <summary>
        /// Executes a command and returns an XML reader.
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="commandText">command text to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>An instance of XmlReader pointing to the stream of xml returned</returns>
        public XmlReader ExecuteXmlReader(out DbCommand cmd, string commandText, params DbParameter[] parameters)
        {
            Commands commands = CreateCommand();
            return commands.ExecuteXmlReader(out cmd, commandText, parameters);
        }

        private Commands CreateCommand()
        {
            return new Commands(connection, transactionControl.CurrentTransaction, CommandTimeOut);
        }

        private T RunCommand<T>(Func<Commands, T> toRun)
        {
            return toRun(CreateCommand());
        }
    }
}
