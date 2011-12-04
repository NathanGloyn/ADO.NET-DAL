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

        protected string ConnectionString;
        protected SqlParameterFactory ParameterFactory;
        protected DataAccess DataAccess;

        public CommonTestSetup()
        {
           DbHelper = new DatabaseSupport(ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString);
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
            DbHelper.RunScript(@"..\..\TestScripts\CommonCreateScripts\06_insert_test_data.sql");
            DataAccess = new DataAccess(ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString, ParameterFactory);
        }

        [TearDown]
        public void TearDown()
        {
            SqlCommandType.dbObjects = null;
            DbHelper.RunScript(@"..\..\TestScripts\CommonCreateScripts\07_reset_data.sql");
        }

    }
}