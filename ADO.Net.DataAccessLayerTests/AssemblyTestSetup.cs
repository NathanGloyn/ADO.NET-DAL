using NUnit.Framework;

namespace ADO.Net.DataAccessLayer.SqlServer.Tests
{
    [SetUpFixture]
    public class AssemblyTestSetup
    {
        protected const string ConnectionName = "sqlTest";

        [SetUp]
        public void Initalize()
        {
            ScriptRunner scriptRunner = ScriptRunner.Get(ConnectionName);

            scriptRunner.Run(@"..\..\TestScripts\CommonCreateScripts\01_create_login_and_user.sql");
            scriptRunner.Run(@"..\..\TestScripts\CommonCreateScripts\02_create_tables.sql");
            scriptRunner.Run(@"..\..\TestScripts\CommonCreateScripts\03_create_stored_procedures.sql");
            scriptRunner.Run(@"..\..\TestScripts\CommonCreateScripts\04_create_views.sql");
            scriptRunner.Run(@"..\..\TestScripts\CommonCreateScripts\05_create_test_schmea_and_objects.sql");
        }        
    }
}