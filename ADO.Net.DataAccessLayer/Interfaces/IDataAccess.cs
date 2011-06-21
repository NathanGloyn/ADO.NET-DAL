using System.Data;
using System.Data.Common;
using System.Xml;

namespace DataAccessLayer.Interfaces
{
    /// <summary>
    /// Standard interface for data access using stored procedures
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// Default time out to use for execution of a command
        /// </summary>
        int CommandTimeOut { get; set; }

        IParameterCreation ParameterFactory { get; }
        ITransactionControl Transactions { get; }


        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        DataSet ExecuteDataSet(string storedProcedureName, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        DataSet ExecuteDataSet(string storedProcedureName);

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        DataSet ExecuteDataSet(out DbCommand cmd, string storedProcedureName, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        DataTable ExecuteDataTable(string storedProcedureName, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        DataTable ExecuteDataTable(out DbCommand cmd, string storedProcedureName, params DbParameter[] parameters);
        
        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        DataTable ExecuteDataTable(string storedProcedureName);

        /// <summary>
        /// Executes a command that does not return a query
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>Number of rows that have been effected by the stored procedure execution</returns>
        int ExecuteNonQuery(string storedProcedureName, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command that does not return a query
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <param name="cmd">Output parameter that will return the command object used in the execution of the procedure</param>
        /// <returns>Integer holding the number of rows effected by the stored procedure execution</returns>
        int ExecuteNonQuery(out DbCommand cmd, string storedProcedureName, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command that does not return a query
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <returns>Number of rows that have been effected by the stored procedure execution</returns>
        int ExecuteNonQuery(string storedProcedureName);

        /// <summary>
        /// Executes a command and returns a data reader
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>SqlDataReader allowing access to results from command</returns>
        DbDataReader ExecuteReader(string storedProcedureName, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command and returns a data reader
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <returns>SqlDataReader allowing access to results from command</returns>
        DbDataReader ExecuteReader(string storedProcedureName);

        /// <summary>
        /// Executes a command that returns a single value
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>Object holding result of execution of database</returns>
        object ExecuteScalar(string storedProcedureName, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command that returns a single value
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <returns>Object holding result of execution of database</returns>
        object ExecuteScalar(string storedProcedureName);

        /// <summary>
        /// Executes a command that returns a single value
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>Object holding result of execution of database</returns>
        object ExecuteScalar(out DbCommand cmd, string storedProcedureName, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command and returns an XML reader.
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <returns>An instance of XmlReader pointing to the stream of xml returned</returns>
        XmlReader ExecuteXmlReader(string storedProcedureName);

        /// <summary>
        /// Executes a command and returns an XML reader.
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>An instance of XmlReader pointing to the stream of xml returned</returns>
        XmlReader ExecuteXmlReader(string storedProcedureName, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command and returns an XML reader.
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="storedProcedureName">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>An instance of XmlReader pointing to the stream of xml returned</returns>
        XmlReader ExecuteXmlReader(out DbCommand cmd, string storedProcedureName, params DbParameter[] parameters);
    }
}
