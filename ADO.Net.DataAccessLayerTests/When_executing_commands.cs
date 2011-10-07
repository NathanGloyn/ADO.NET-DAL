using System.Data;
using System.Data.Common;
using NUnit.Framework;

namespace ADO.Net.DataAccessLayer.SqlServer.Tests
{
    [TestFixture]
    public class When_executing_commands:CommonTestSetup
    {
        [Test]
        public void Should_use_default_SqlCommand_timeout()
        {
            DbCommand cmd = null;

            DataAccess.ExecuteScalar(out cmd, "SelectFromTestTable", DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(30, cmd.CommandTimeout);
        }

        [Test]
        public void Should_use_command_timeout_if_set()
        {
            DataAccess.CommandTimeOut = 10;
            DbCommand cmd = null;

            DataAccess.ExecuteScalar(out cmd, "SelectFromTestTable", DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(10, cmd.CommandTimeout);
        }        
    }
}