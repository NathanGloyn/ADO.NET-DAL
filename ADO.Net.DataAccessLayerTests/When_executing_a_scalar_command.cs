using System.Data;
using System.Data.Common;
using NUnit.Framework;

namespace ADO.Net.DataAccessLayer.SqlServer.Tests
{
    public class When_executing_a_scalar_command:CommonTestSetup
    {
        [Test()]
        public void Should_execute_a_scalar_command_with_no_parameters_returning_the_correct_value()
        {
            string result = (string)DataAccess.ExecuteScalar("SelectOneFromTestTable");
            Assert.AreEqual("value1", result);
        }

        [Test()]
        public void Should_execute_a_scalar_command_with_a_parameter_returning_the_correct_value()
        {
            string result = (string)DataAccess.ExecuteScalar("SelectFromTestTable", DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));
            Assert.AreEqual("value1", result);
        }

        [Test()]
        public void Should_execute_a_scalar_command_with_a_parameter_returning_the_correct_value_and_the_command_object()
        {
            DbCommand cmd = null;

            string result = (string)DataAccess.ExecuteScalar(out cmd, "SelectFromTestTable", DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));

            Assert.IsNotNull(cmd);
            Assert.AreEqual("key1", (string)cmd.Parameters["@TestKey"].Value);
            Assert.AreEqual("value1", result);
        }        
    }
}