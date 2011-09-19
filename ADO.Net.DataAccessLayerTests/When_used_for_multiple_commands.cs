using System.Data;
using System.Data.Common;
using NUnit.Framework;

namespace ADO.Net.DataAccessLayerTests
{  
    [TestFixture()]
    public class When_used_for_multiple_commands:CommonTestSetup
    {

        [Test()]
        public void Should_execute_multiple_nonquery_commands_with_parameters_returning_a_command_and_the_correct_values()
        {
            DbCommand cmd = null;
            int rowsAffected = DataAccess.ExecuteNonQuery(out cmd, "DeleteFromTestTable", DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(1, rowsAffected);           
            Assert.IsNotNull(cmd);
            Assert.AreEqual("key1", (string)cmd.Parameters["@TestKey"].Value);
            
            string result = (string)DataAccess.ExecuteScalar("SelectFromTestTable", DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));
            Assert.IsNull(result);
        }


        [Test()]
        public void Should_execute_multiple_nonquery_commands_with_parameters_returning_the_correct_values()
        {
            int rowsAffected = DataAccess.ExecuteNonQuery("DeleteFromTestTable", DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(1, rowsAffected);

            string result = (string)DataAccess.ExecuteScalar("SelectFromTestTable", DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));
            Assert.IsNull(result);
        }

        [Test()]
        public void Should_execute_multiple_nonquery_commands_with_and_without_parameters_returning_the_correct_values()
        {
            int rowsAffected = DataAccess.ExecuteNonQuery("DeleteOneFromTestTable");

            Assert.AreEqual(1, rowsAffected);

            string result = (string)DataAccess.ExecuteScalar("SelectFromTestTable", DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));
            Assert.IsNull(result);
        }
    }
}
