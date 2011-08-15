using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using NUnit.Framework;
using System.Xml;

namespace ADO.Net.DataAccessLayerTests
{  
    /// <summary>
    ///This is a test class for SqlDataAccessTest and is intended
    ///to contain all SqlDataAccessTest Unit Tests
    ///</summary>
    [TestFixture()]
    public class When_using_SqlDataAccess:CommonTestSetup
    {


        /// <summary>
        ///A test for ExecuteNonQuery(cmd, sp, params)
        ///</summary>
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

        /// <summary>
        ///A test for ExecuteNonQuery(sp, params)
        ///</summary>
        [Test()]
        public void Should_execute_multiple_nonquery_commands_with_parameters_returning_the_correct_values()
        {
            int rowsAffected = DataAccess.ExecuteNonQuery("DeleteFromTestTable", DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(1, rowsAffected);

            string result = (string)DataAccess.ExecuteScalar("SelectFromTestTable", DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));
            Assert.IsNull(result);
        }

        /// <summary>
        ///A test for ExecuteNonQuery(sp)
        ///</summary>
        [Test()]
        public void Should_execute_multiple_nonquery_commands_with_and_without_parameters_returning_the_correct_values()
        {
            int rowsAffected = DataAccess.ExecuteNonQuery("DeleteOneFromTestTable");

            Assert.AreEqual(1, rowsAffected);

            string result = (string)DataAccess.ExecuteScalar("SelectFromTestTable", DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));
            Assert.IsNull(result);
        }

        /// <summary>
        ///A test for ExecuteDataTable(cmd, sp, params)
        ///</summary>
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

        [Test()]
        public void Should_execute_command_Text_returning_a_populated_DataTable_and_command_object()
        {
            DbCommand cmd;
            DataTable result = DataAccess.ExecuteDataTable(out cmd, "SELECT * FROM TestTable WHERE TestKey !=  'key1'");

            Assert.IsNotNull(cmd);
            Assert.That(cmd.CommandType, Is.EqualTo(CommandType.Text));

            Assert.AreEqual(4, result.Rows.Count);
        }

        [Test()]
        public void Should_execute_TableDirect_returning_a_populated_DataTable_and_command_object()
        {
            DbCommand cmd;
            DataTable result = DataAccess.ExecuteDataTable(out cmd, "TestTable");

            Assert.IsNotNull(cmd);
            Assert.That(cmd.CommandType, Is.EqualTo(CommandType.TableDirect));

            Assert.AreEqual(5, result.Rows.Count);
        }

        /// <summary>
        ///A test for ExecuteDataTable(sp, params)
        ///</summary>
        [Test()]
        public void Should_execute_command_with_parameters_returning_a_populated_DataTable()
        {
            DataTable result = DataAccess.ExecuteDataTable("SelectAllButFromTestTable", DataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(4, result.Rows.Count);
        }

        /// <summary>
        ///A test for ExecuteDataTable(sp)
        ///</summary>
        [Test()]
        public void Should_execute_command_with_no_parameters_returning_a_populated_DataTable()
        {
            DataTable result = DataAccess.ExecuteDataTable("SelectAllFromTestTable");

            Assert.AreEqual(5, result.Rows.Count);
        }

        /// <summary>
        ///A test for ExecuteDataSet(cmd, sp, params)
        ///</summary>
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

        /// <summary>
        ///A test for ExecuteDataSet(sp, params)
        ///</summary>
        [Test()]
        public void Should_execute_command_with_parameters_returning_a_populated_DataSet()
        {
            DataSet result = DataAccess.ExecuteDataSet("SelectAllButFromTestTable", DataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(1, result.Tables.Count);
            Assert.AreEqual(4, result.Tables[0].Rows.Count);
        }

        /// <summary>
        ///A test for ExecuteDataSet(sp)
        ///</summary>
        [Test()]
        public void Should_execute_command_with_no_parameters_returning_a_populated_DataSet()
        {
            DataSet result = DataAccess.ExecuteDataSet("SelectAllFromTestTable");

            Assert.AreEqual(1, result.Tables.Count);
            Assert.AreEqual(5, result.Tables[0].Rows.Count);
        }


        [Test]
        public void Should_use_default_SqlCommand_timeout()
        {
            DbCommand cmd = null;

            DataAccess.ExecuteScalar(out cmd, "SelectFromTestTable", DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(30, cmd.CommandTimeout);
        }

        [Test]
        public void Should_use_command_timeout_set_on_class()
        {
            DataAccess.CommandTimeOut = 10;
            DbCommand cmd = null;

            DataAccess.ExecuteScalar(out cmd, "SelectFromTestTable", DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(10, cmd.CommandTimeout);
        }

    }
}
