using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.SqlServer
{
    internal class SqlCommandTypeDecider
    {
        internal static SortedList<string,CommandType> dbObjects;
        
        public SqlCommandTypeDecider(string connectionString)
        {
            if (dbObjects == null)
            {
                dbObjects = new SortedList<string, CommandType>();
                PopulateCacheData(connectionString);
            }
        }

        public CommandType GetCommandType(string commandText)
        {

                if (commandText.Split(' ', '\t').Length == 1)
                {
                    if (dbObjects.ContainsKey(commandText))
                    {
                        CommandType toReturn;

                        dbObjects.TryGetValue(commandText, out toReturn);

                        return toReturn;
                    }
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
                    RetrieveTablesAndViews(connection);
                    RetrieveStoredProcedures(connection);
                }
            }
        }

        private void RetrieveTablesAndViews(SqlConnection connection)
        {
            ExtractSchemaDetail(connection, "Tables", "table_name", CommandType.TableDirect);
        }

        private void RetrieveStoredProcedures(SqlConnection connection)
        {
            ExtractSchemaDetail(connection, "Procedures", "specific_name", CommandType.StoredProcedure);
        }

        private void ExtractSchemaDetail(SqlConnection connection, string collectionName, string columnName, CommandType type)
        {
            foreach (DataRow row in connection.GetSchema(collectionName).Rows)
            {
                dbObjects.Add(row[columnName].ToString(), type);
            }
        }
    }
}