using System.Configuration;
using NUnit.Framework;
using UnitTest.Database;

namespace ADO.Net.DataAccessLayer.SqlServer.Tests
{
    [SetUpFixture]
    public class AssemblyTestSetup
    {
        protected const string ConnectionName = "sqlTest";

        [SetUp]
        public void Initalize()
        {
            DatabaseSupport DbHelper = new DatabaseSupport(ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString);

            DbHelper.RunScript(@"..\..\TestScripts\CommonCreateScripts\01_create_login_and_user.sql");
            DbHelper.RunScript(@"..\..\TestScripts\CommonCreateScripts\02_create_tables.sql");
            DbHelper.RunScript(@"..\..\TestScripts\CommonCreateScripts\03_create_stored_procedures.sql");
            DbHelper.RunScript(@"..\..\TestScripts\CommonCreateScripts\04_create_views.sql");
            DbHelper.RunScript(@"..\..\TestScripts\CommonCreateScripts\05_create_test_schmea_and_objects.sql");
        }        
    }
}