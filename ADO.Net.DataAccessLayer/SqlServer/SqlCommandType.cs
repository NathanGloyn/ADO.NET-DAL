using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.SqlServer
{
    internal class SqlCommandType
    {
        internal SortedList<string,CommandType> dbObjects;
        internal static Dictionary<string, SortedList<string, CommandType>> cacheData;


        public SqlCommandType(string connectionString)
        {
            dbObjects = GetCachedObjectForDB(connectionString);

            if(dbObjects == null)
                PopulateCacheData(connectionString);
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
            commandText = commandText.ToLowerInvariant();

            if (dbObjects.ContainsKey(commandText))
            {
                CommandType toReturn;

                dbObjects.TryGetValue(commandText.ToLowerInvariant(), out toReturn);

                return toReturn;
            }

            return CommandType.Text;

        }

        private void PopulateCacheData(string connectionString)
        {
            if(cacheData == null)
                cacheData = new Dictionary<string, SortedList<string, CommandType>>();

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

            dbObjects = new SortedList<string, CommandType>();
            cacheData.Add(connection.ConnectionString, dbObjects);

            foreach (DataRow row in dt.Rows)
            {
                if (!dbObjects.ContainsKey(row[columnName].ToString().ToLowerInvariant()))
                {
                    dbObjects.Add(row[columnName].ToString().ToLowerInvariant(), type);
                }
            }
        }

        private static SortedList<string, CommandType> GetCachedObjectForDB(string connectionString)
        {
            SortedList<string, CommandType> currentDbObjects = null;

            if (cacheData != null && cacheData.ContainsKey(connectionString))
            {
                cacheData.TryGetValue(connectionString, out currentDbObjects);
            }

            return currentDbObjects;
        }
    }
}