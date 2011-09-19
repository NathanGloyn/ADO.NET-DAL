using System.Data;
using System.Data.Common;
using NUnit.Framework;

namespace ADO.Net.DataAccessLayerTests
{
    [TestFixture]
    public class When_executing_a_command_returning_a_datatable : CommonTestSetup
    {

        [Test()]
        public void Should_execute_command_with_no_parameters_returning_a_populated_DataTable()
        {
            DataTable result = DataAccess.ExecuteDataTable("SelectAllFromTestTable");

            Assert.AreEqual(5, result.Rows.Count);
        }

        [Test()]
        public void Should_execute_command_with_parameters_returning_a_populated_DataTable()
        {
            DataTable result = DataAccess.ExecuteDataTable("SelectAllButFromTestTable", DataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(4, result.Rows.Count);
        }

        [Test()]
        public void Should_execute_command_Text_returning_a_populated_DataTable_and_command_with_correct_command_Type()
        {
            DbCommand cmd;
            DataTable result = DataAccess.ExecuteDataTable(out cmd, "SELECT * FROM TestTable WHERE TestKey !=  'key1'");

            Assert.IsNotNull(cmd);
            Assert.That(cmd.CommandType, Is.EqualTo(CommandType.Text));

            Assert.AreEqual(4, result.Rows.Count);
        }


        [Test()]
        public void Should_execute_command_with_parameters_returning_a_populated_DataTable_and_command_object()
        {
            DbCommand cmd;
            DataTable result = DataAccess.ExecuteDataTable(out cmd, "SelectAllButFromTestTable", DataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1"));

            Assert.IsNotNull(cmd);
            Assert.That(cmd.CommandType, Is.EqualTo(CommandType.StoredProcedure));
            Assert.AreEqual("key1", (string)cmd.Parameters["@ExcludeKey"].Value);

            Assert.AreEqual(4, result.Rows.Count);
        }
    }
}