using System.Configuration;
using DataAccessLayer.SqlServer;
using NUnit.Framework;
using UnitTest.Database;

namespace ADO.Net.DataAccessLayerTests
{
    public class CommonTestSetup
    {
        protected const string ConnectionName = "sqlTest";
        protected static DatabaseSupport DbHelper;

        protected SqlParameterFactory ParameterFactory;
        protected DataAccess DataAccess;

        [TestFixtureSetUp]
        public void InitializeTests()
        {
            DbHelper = new DatabaseSupport(ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString);

            DbHelper.RunScript(@"..\..\TestScripts\CommonCreateScripts\01_create_tables.sql");
            DbHelper.RunScript(@"..\..\TestScripts\CommonCreateScripts\02_create_stored_procedures.sql");

            ParameterFactory = new SqlParameterFactory();
        }

        [SetUp]
        public void Setup()
        {
            DbHelper.RunScript(@"..\..\TestScripts\CommonCreateScripts\03_insert_test_data.sql");
            DataAccess = new DataAccess(ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString, ParameterFactory);
        }
    }
}