using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;

namespace ADO.Net.DataAccessLayerTests
{
    [TestFixture]
    public class When_determinng_type_of_command:CommonTestSetup
    {

        [Test] 
        public void Should_return_type_as_table_direct()
        {
            SqlCommandTypeDecider decider = new SqlCommandTypeDecider(ConnectionString);

            Assert.That(decider.GetCommandType("TestTable"), Is.EqualTo(CommandType.TableDirect));
        }

        [Test]
        public void Should_return_type_as_Text_if_table_not_found()
        {
            SqlCommandTypeDecider decider = new SqlCommandTypeDecider(ConnectionString);

            Assert.That(decider.GetCommandType("MissingTable"), Is.EqualTo(CommandType.Text));            
        }



    }

    public class SqlCommandTypeDecider
    {
        private SortedList<string,CommandType> dbObjects;
        

        public SqlCommandTypeDecider(string connectionString)
        {
            dbObjects = new SortedList<string, CommandType>();
            PopulateCacheData(connectionString);
        }

        private void PopulateCacheData(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                RetrieveTables(connection);
                RetrieveViews(connection);
                RetrieveStoredProcedures(connection);
            } 
            
        }

        public CommandType GetCommandType(string commandText)
        {
            if (dbObjects.ContainsKey(commandText))
            {
                CommandType toReturn;

                dbObjects.TryGetValue(commandText, out toReturn);

                return toReturn;
            }

            return CommandType.Text;
        }

        private void RetrieveTables(SqlConnection connection)
        {
            ExtractSchemaDetail(connection, "Tables", "table_name");
        }

        private void RetrieveViews(SqlConnection connection)
        {
            ExtractSchemaDetail(connection, "Views", "table_name");
        }

        private void RetrieveStoredProcedures(SqlConnection connection)
        {
            ExtractSchemaDetail(connection, "Procedures", "specific_name");
        }

        private void ExtractSchemaDetail(SqlConnection connection, string collectionName, string columnName)
        {
            var tables = connection.GetSchema(collectionName);
            foreach (DataRow row in tables.Rows)
            {
                dbObjects.Add(row[columnName].ToString(), CommandType.TableDirect);
            }
        }
    }
}