using System.Data;
using System.Data.Common;
using System.Xml;

namespace DataAccessLayer.Interfaces
{
    public interface ISqlCommands
    {
        /// <summary>
        /// Executes a command that does not return a query
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        int ExecuteNonQuery(string commandText);

        /// <summary>
        /// Executes a command that does not return a query
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        int ExecuteNonQuery(string commandText, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command that does not return a query
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>/// 
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>DbCommand containing the command executed</returns>
        int ExecuteNonQuery(out DbCommand cmd, string commandText, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command that returns a single value
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <returns>Object holding result of execution of database</returns>
        object ExecuteScalar(string commandText);

        /// <summary>
        /// Executes a command that returns a single value
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>Object holding result of execution of database</returns>
        object ExecuteScalar(string commandText, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command that returns a single value
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>Object holding result of execution of database</returns>
        object ExecuteScalar(out DbCommand cmd, string commandText, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command and returns a data reader
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <returns>SqlDataReader allowing access to results from command</returns>
        DbDataReader ExecuteReader(string commandText);

        /// <summary>
        /// Executes a command and returns a data reader
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>SqlDataReader allowing access to results from command</returns>
        DbDataReader ExecuteReader(string commandText, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        DataTable ExecuteDataTable(string commandText);

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        DataTable ExecuteDataTable(string commandText, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        DataTable ExecuteDataTable(out DbCommand cmd, string commandText, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        DataSet ExecuteDataSet(string commandText);

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        DataSet ExecuteDataSet(string commandText, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command and returns a DataTable
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>DataTable populated with data from executing stored procedure</returns>
        DataSet ExecuteDataSet(out DbCommand cmd, string commandText, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command and returns an XML reader.
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <returns>An instance of XmlReader pointing to the stream of xml returned</returns>
        XmlReader ExecuteXmlReader(string commandText);

        /// <summary>
        /// Executes a command and returns an XML reader.
        /// </summary>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">SqlParameter colleciton to use in executing</param>
        /// <returns>An instance of XmlReader pointing to the stream of xml returned</returns>
        XmlReader ExecuteXmlReader(string commandText, params DbParameter[] parameters);

        /// <summary>
        /// Executes a command and returns an XML reader.
        /// </summary>
        /// <param name="cmd">Output parameter that holds reference to the command object just executed</param>
        /// <param name="commandText">Name of stored procedure to execute</param>
        /// <param name="parameters">DbParameter colleciton to use in executing</param>
        /// <returns>An instance of XmlReader pointing to the stream of xml returned</returns>
        XmlReader ExecuteXmlReader(out DbCommand cmd, string commandText, params DbParameter[] parameters);
    }
}