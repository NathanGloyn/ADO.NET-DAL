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
                RetrieveTablesAndViews(connection);
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

        private void RetrieveTablesAndViews(SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand("SELECT TABLE_NAME FROM information_schema.tables ORDER BY 2", connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    dbObjects.Add(reader.GetString(0), CommandType.TableDirect);
                }   
            }
        }

        private void RetrieveStoredProcedures(SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand("select Specific_Name from information_schema.routines where routine_type = 'PROCEDURE' and Left(Routine_Name, 3) NOT IN ('sp_', 'xp_', 'ms_')", connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    dbObjects.Add(reader.GetString(0), CommandType.StoredProcedure);
                }
            }
        }

    }
}