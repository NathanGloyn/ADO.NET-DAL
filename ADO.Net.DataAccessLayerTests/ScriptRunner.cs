using System.Configuration;
using UnitTest.Database;

namespace ADO.Net.DataAccessLayer.SqlServer.Tests
{
    public class ScriptRunner
    {
        protected const string ConnectionName = "sqlTest";

        private DatabaseSupport DbHelper;


        public static ScriptRunner Get(string connectionStringName)
        {
            return new ScriptRunner(connectionStringName);
        }

        public ScriptRunner(string connectionStringName)
        {
           DbHelper = new DatabaseSupport(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
        }

        public void Run(string script)
        {
            DbHelper.RunScript(script);
        }

        public void CreateDb(string name)
        {
            DbHelper.CreateDB(name);
        }

        public void DropDb(string name)
        {
            DbHelper.DropDB(name);
        }
    }
}