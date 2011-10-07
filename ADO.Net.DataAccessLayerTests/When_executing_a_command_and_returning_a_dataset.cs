using System.Data;
using System.Data.Common;
using NUnit.Framework;

namespace ADO.Net.DataAccessLayer.SqlServer.Tests
{
    [TestFixture]
    public class When_executing_a_command_and_returning_a_dataset:CommonTestSetup
    {

        [Test()]
        public void Should_execute_command_with_parameters_returning_a_populated_DataSet_and_command_object()
        {
            DbCommand cmd;
            DataSet result = DataAccess.ExecuteDataSet(out cmd, "SelectAllButFromTestTable", DataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1"));

            Assert.IsNotNull(cmd);
            Assert.AreEqual("key1", (string)cmd.Parameters["@ExcludeKey"].Value);

            Assert.AreEqual(1, result.Tables.Count);
            Assert.AreEqual(4, result.Tables[0].Rows.Count);
        }


        [Test()]
        public void Should_execute_command_with_parameters_returning_a_populated_DataSet()
        {
            DataSet result = DataAccess.ExecuteDataSet("SelectAllButFromTestTable", DataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(1, result.Tables.Count);
            Assert.AreEqual(4, result.Tables[0].Rows.Count);
        }

        [Test()]
        public void Should_execute_command_with_no_parameters_returning_a_populated_DataSet()
        {
            DataSet result = DataAccess.ExecuteDataSet("SelectAllFromTestTable");

            Assert.AreEqual(1, result.Tables.Count);
            Assert.AreEqual(5, result.Tables[0].Rows.Count);
        }        
    }
}