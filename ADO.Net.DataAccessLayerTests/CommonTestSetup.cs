using System.Configuration;
using DataAccessLayer.Core;
using DataAccessLayer.SqlServer;
using NUnit.Framework;
using UnitTest.Database;

namespace ADO.Net.DataAccessLayer.SqlServer.Tests
{
    public class CommonTestSetup
    {
        protected const string ConnectionName = "sqlTest";
        protected static DatabaseSupport DbHelper;
        public ScriptRunner scriptRunner;

        protected string ConnectionString;
        protected SqlParameterFactory ParameterFactory;
        protected DataAccess DataAccess;

        public CommonTestSetup()
        {
            scriptRunner = ScriptRunner.Get(ConnectionName);
        }

        [TestFixtureSetUp]
        public void InitializeTests()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;
            ParameterFactory = new SqlParameterFactory();
        }

        [SetUp]
        public void Setup()
        {
            scriptRunner.Run(@"..\..\TestScripts\CommonCreateScripts\06_insert_test_data.sql");
            DataAccess = new DataAccess(ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString);
            DataAccess.ParameterFactory = ParameterFactory;
        }

        [TearDown]
        public void TearDown()
        {
            SqlCommandType.cacheData = null;
            scriptRunner.Run(@"..\..\TestScripts\CommonCreateScripts\07_reset_data.sql");
        }

    }
}