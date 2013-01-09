using System;
using System.Configuration;
using System.Data;
using System.Linq;
using DataAccessLayer.SqlServer;
using NUnit.Framework;

namespace ADO.Net.DataAccessLayer.SqlServer.Tests
{
    [TestFixture]
    public class When_determinng_type_of_command:CommonTestSetup
    {
        private string connectionStringMinPermissions;
        private string connectionStringTestSchemaOwnerPermissions;

        public When_determinng_type_of_command()
        {
            connectionStringMinPermissions = ConfigurationManager.ConnectionStrings["MinPermission"].ConnectionString;
            connectionStringTestSchemaOwnerPermissions = ConfigurationManager.ConnectionStrings["TestSchemaOwnerPermission"].ConnectionString;
        }


        [Test]
        public void Should_return_type_as_Text_if_table_not_found()
        {
            SqlCommandType decider = new SqlCommandType(connectionStringMinPermissions);

            Assert.That(decider.Get("MissingTable"), Is.EqualTo(CommandType.Text));            
        }


        [Test]
        public void Should_return_type_as_StoredProcedure_for_existng_procedure()
        {
            SqlCommandType decider = new SqlCommandType(connectionStringMinPermissions);

            Assert.That(decider.Get("AddToTestTable"), Is.EqualTo(CommandType.StoredProcedure));
        }

        [Test]
        public void Should_return_type_as_Text_for_sql_string_with_space_between_keywords()
        {
            SqlCommandType decider = new SqlCommandType(ConnectionString);

            Assert.That(decider.Get("SELECT * FROM TestTable"), Is.EqualTo(CommandType.Text));            
        }

        [Test]
        public void Should_return_type_as_Text_for_sql_string_with_tab_between_keywords()
        {
            SqlCommandType decider = new SqlCommandType(ConnectionString);

            Assert.That(decider.Get("SELECT\t*\tFROM\tTestTable"), Is.EqualTo(CommandType.Text));
        }

        [Test]
        public void Should_return_type_as_StoredProcedure_for_existng_procedure_with_space_in_name()
        {
            SqlCommandType decider = new SqlCommandType(connectionStringMinPermissions);

            Assert.That(decider.Get("[Sproc with spaces in name]"), Is.EqualTo(CommandType.StoredProcedure));
        }

        [Test]
        public void Should_not_return_StoredProcedure_when_procedure_is_in_another_schema_user_has_no_access_to()
        {
            SqlCommandType decider = new SqlCommandType(connectionStringMinPermissions);

            Assert.That(decider.Get("[SelectAllFromTestSchemaTable]"), Is.EqualTo(CommandType.Text));            
        }

        [Test]
        public void Should_return_StoredProcedure_when_procedure_is_in_another_schema_user_access_to()
        {
            SqlCommandType decider = new SqlCommandType(connectionStringTestSchemaOwnerPermissions);

            Assert.That(decider.Get("[SelectAllFromTestSchemaTable]"), Is.EqualTo(CommandType.StoredProcedure));
        }

        [Test]
        public void Should_return_type_as_text_when_commandText_includes_table_with_square_brackets()
        {
            SqlCommandType decider = new SqlCommandType(connectionStringTestSchemaOwnerPermissions);

            Assert.That(decider.Get("SELECT * FROM [TestTable]"), Is.EqualTo(CommandType.Text));   
        }

        [Test]
        public void Should_return_type_as_StoredProcedure_when_name_is_wrong_case()
        {
            SqlCommandType decider = new SqlCommandType(connectionStringMinPermissions);
            Assert.That(decider.Get("addtotesttable"), Is.EqualTo(CommandType.StoredProcedure));
        }

        [Test]
        public void Should_store_schema_data_for_connection_using_connection_string_as_a_key()
        {
            if(SqlCommandType.cacheData != null)
                SqlCommandType.cacheData.Clear();

            SqlCommandType decider = new SqlCommandType(connectionStringMinPermissions);
            decider.Get("[SelectAllFromTestSchemaTable]");

            Assert.That(SqlCommandType.cacheData.Keys.First(), Is.EqualTo(connectionStringMinPermissions));
        }
    }
}