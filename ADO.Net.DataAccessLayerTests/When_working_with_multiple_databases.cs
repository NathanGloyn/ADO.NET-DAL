using System.Configuration;
using System.Data;
using DataAccessLayer.Core;
using DataAccessLayer.SqlServer;
using NUnit.Framework;

namespace ADO.Net.DataAccessLayer.SqlServer.Tests
{
    [TestFixture]
    public class When_working_with_multiple_databases
    {
        protected const string PrimaryConnectionName = "sqlTest";
        protected const string SecondaryConnectionName = "AdditionalDB";
        public ScriptRunner scriptRunner;
        private string connectionStringMinPermissions;
        private string connectionStringAdditionalDb;
        private SqlCommandType commandTypeDecider;

        protected string ConnectionString;
        protected SqlParameterFactory ParameterFactory;


        [TestFixtureSetUp]
        public void SetupFixtureWideObjects()
        {
            connectionStringMinPermissions = ConfigurationManager.ConnectionStrings["MinPermission"].ConnectionString;
            connectionStringAdditionalDb = ConfigurationManager.ConnectionStrings[SecondaryConnectionName].ConnectionString;

            scriptRunner = ScriptRunner.Get(PrimaryConnectionName);
            scriptRunner.CreateDb(SecondaryConnectionName);
            scriptRunner.Run(@"..\..\TestScripts\ExtraDB\01_Create_schema.sql");

            ParameterFactory = new SqlParameterFactory();
        }

        [TestFixtureTearDown]
        public void RemoveAdditionalDb()
        {
            scriptRunner.DropDb("AdditionalDB");
        }


        [SetUp]
        public void Setup()
        {
            scriptRunner.Run(@"..\..\TestScripts\CommonCreateScripts\06_insert_test_data.sql");
        }

        [TearDown]
        public void TearDown()
        {
            SqlCommandType.cacheData = null;
            scriptRunner.Run(@"..\..\TestScripts\CommonCreateScripts\07_reset_data.sql");
        }

        [Test]
        public void Should_determine_the_correct_command_type_for_1st_db()
        {
            PopulateSchemaDetails();
            Assert.That(commandTypeDecider.Get("AddToTestTable"), Is.EqualTo(CommandType.StoredProcedure));
        }


        [Test]
        public void Should_return_type_as_StoredProcedure_when_it_is_in_2nd_database_to_be_accessed()
        {
            PopulateSchemaDetails();
            Assert.That(commandTypeDecider.Get("[SelectAllFromTestTableAdditional]"), Is.EqualTo(CommandType.StoredProcedure));
        }

        [Test()]
        public void Should_execute_command_against_1st_db_with_a_parameter_returning_a_populated_DataTable()
        {
            var dataAccess = CreateDataAccess(PrimaryConnectionName);
            DataTable result = dataAccess.ExecuteDataTable("SelectFromTestTable",
                                                            ParameterFactory.Create("TestKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(1, result.Rows.Count);
        }

        [Test()]
        public void Should_execute_command_against_2nd_db_with_a_parameter_returning_an_empty_DataTable()
        {
            var dataAccess = CreateDataAccess(SecondaryConnectionName);
            DataTable result = dataAccess.ExecuteDataTable("SelectFromTestTable", 
                                                            ParameterFactory.Create("TestKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(0, result.Rows.Count);
        }


        private DataAccess CreateDataAccess(string connectionName)
        {
            var dataAccess = new DataAccess(ConfigurationManager.ConnectionStrings[connectionName].ConnectionString);
            dataAccess.ParameterFactory = ParameterFactory;

            return dataAccess;
        }

        private void PopulateSchemaDetails()
        {
            commandTypeDecider = new SqlCommandType(connectionStringMinPermissions);
            commandTypeDecider = new SqlCommandType(connectionStringAdditionalDb);
        }
    }
}