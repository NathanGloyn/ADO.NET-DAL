using System.Configuration;
using System.Data;
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
            SqlCommandTypeDecider decider = new SqlCommandTypeDecider(connectionStringMinPermissions);

            Assert.That(decider.GetCommandType("MissingTable"), Is.EqualTo(CommandType.Text));            
        }


        [Test]
        public void Should_return_type_as_StoredProcedure_for_existng_procedure()
        {
            SqlCommandTypeDecider decider = new SqlCommandTypeDecider(connectionStringMinPermissions);

            Assert.That(decider.GetCommandType("AddToTestTable"), Is.EqualTo(CommandType.StoredProcedure));
        }

        [Test]
        public void Should_return_type_as_Text_for_sql_string_with_space_between_keywords()
        {
            SqlCommandTypeDecider decider = new SqlCommandTypeDecider(ConnectionString);

            Assert.That(decider.GetCommandType("SELECT * FROM TestTable"), Is.EqualTo(CommandType.Text));            
        }

        [Test]
        public void Should_return_type_as_Text_for_sql_string_with_tab_between_keywords()
        {
            SqlCommandTypeDecider decider = new SqlCommandTypeDecider(ConnectionString);

            Assert.That(decider.GetCommandType("SELECT\t*\tFROM\tTestTable"), Is.EqualTo(CommandType.Text));
        }

        [Test]
        public void Should_return_type_as_StoredProcedure_for_existng_procedure_with_space_in_name()
        {
            SqlCommandTypeDecider decider = new SqlCommandTypeDecider(connectionStringMinPermissions);

            Assert.That(decider.GetCommandType("[Sproc with spaces in name]"), Is.EqualTo(CommandType.StoredProcedure));
        }

        [Test]
        public void Should_not_return_StoredProcedure_when_procedure_is_in_another_schema_user_has_no_access_to()
        {
            SqlCommandTypeDecider decider = new SqlCommandTypeDecider(connectionStringMinPermissions);

            Assert.That(decider.GetCommandType("[SelectAllFromTestSchemaTable]"), Is.EqualTo(CommandType.Text));            
        }

        [Test]
        public void Should_return_StoredProcedure_when_procedure_is_in_another_schema_user_access_to()
        {
            SqlCommandTypeDecider decider = new SqlCommandTypeDecider(connectionStringTestSchemaOwnerPermissions);

            Assert.That(decider.GetCommandType("[SelectAllFromTestSchemaTable]"), Is.EqualTo(CommandType.StoredProcedure));
        }

    }
}