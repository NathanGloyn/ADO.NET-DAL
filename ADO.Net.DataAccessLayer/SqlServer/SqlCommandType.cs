using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.SqlServer
{
    internal class SqlCommandType
    {
        internal static SortedList<string,CommandType> dbObjects;
        
        public SqlCommandType(string connectionString)
        {
            if (dbObjects == null)
            {
                dbObjects = new SortedList<string, CommandType>();
                PopulateCacheData(connectionString);
            }
        }

        public CommandType Get(string commandText)
        {
            if (commandText.Contains("["))
            {
                if (commandText.LastIndexOf(']') == commandText.Length - 1)
                {
                    return TryGetStoredProcedure(commandText.Substring(commandText.LastIndexOf('[')));
                }
            }

            if (commandText.Split(' ', '\t').Length == 1)
            {
                return TryGetStoredProcedure(commandText);
            }

            return CommandType.Text;
        }

        private CommandType TryGetStoredProcedure(string commandText)
        {
            commandText = commandText.Replace("[", "");
            commandText = commandText.Replace("]", "");
            if (dbObjects.ContainsKey(commandText.ToLowerInvariant()))
            {
                CommandType toReturn;

                dbObjects.TryGetValue(commandText.ToLowerInvariant(), out toReturn);

                return toReturn;
            }

            return CommandType.Text;

        }

        private void PopulateCacheData(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                object lockObject = new object();
                lock (lockObject)
                {
                    RetrieveStoredProcedures(connection);
                }
            }
        }

        private void RetrieveStoredProcedures(SqlConnection connection)
        {
            ExtractSchemaDetail(connection, "Procedures", "specific_name", CommandType.StoredProcedure);
        }

        private void ExtractSchemaDetail(SqlConnection connection, string collectionName, string columnName, CommandType type)
        {
            DataTable dt = connection.GetSchema(collectionName);

            foreach (DataRow row in dt.Rows)
            {
                if (!dbObjects.ContainsKey(row[columnName].ToString().ToLowerInvariant()))
                {
                    dbObjects.Add(row[columnName].ToString().ToLowerInvariant(), type);
                }
            }
        }
    }
}