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
            Assert.AreEqual("key1", (string)cmd.Parameters["@ExcludeKey"].Value);

            Assert.AreEqual(4, result.Rows.Count);
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
        public void Should_execute_command_with_no_parameters_returning_a_XmlReader()
        {
            string results;
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllFromTestTableAsXml"))
            {
                reader.Read();
                results = reader.ReadOuterXml();
            }

            StringAssert.AreEqualIgnoringCase("<Entries><Entry><Key>key1</Key><Value>value1</Value></Entry><Entry><Key>key2</Key><Value>value2</Value></Entry><Entry><Key>key3</Key><Value>value3</Value></Entry><Entry><Key>key4</Key><Value>value4</Value></Entry><Entry><Key>key5</Key><Value>value5</Value></Entry></Entries>", results);
        }

        [Test]
        public void Should_execute_command_with_a_parameter_returning_a_XmlReader()
        {
            string results;
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllButFromTestTableAsXml", DataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1")))
            {
                reader.Read();
                results = reader.ReadOuterXml();
            }

            StringAssert.AreEqualIgnoringCase("<Entries><Entry><Key>key2</Key><Value>value2</Value></Entry><Entry><Key>key3</Key><Value>value3</Value></Entry><Entry><Key>key4</Key><Value>value4</Value></Entry><Entry><Key>key5</Key><Value>value5</Value></Entry></Entries>", results);

        }

        [Test]
        public void Should_execute_command_with_a_parameter_returning_a_XmlReader_and_command_object()
        {
            DbCommand cmd;
            string results;
            using (XmlReader reader = DataAccess.ExecuteXmlReader(out cmd, "SelectAllButFromTestTableAsXml", DataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1")))
            {
                reader.Read();

                Assert.IsNotNull(cmd);
                Assert.AreEqual("key1", (string)cmd.Parameters["@ExcludeKey"].Value);

                results = reader.ReadOuterXml();
            }

            StringAssert.AreEqualIgnoringCase("<Entries><Entry><Key>key2</Key><Value>value2</Value></Entry><Entry><Key>key3</Key><Value>value3</Value></Entry><Entry><Key>key4</Key><Value>value4</Value></Entry><Entry><Key>key5</Key><Value>value5</Value></Entry></Entries>", results);
            Assert.IsNotNull(cmd);
        }

        /// <summary>
        /// A test to ensure that ExecuteXmlReader can be called multiple times with the same parameter
        /// </summary>
        [Test]
        public void Should_execute_command_multiple_times_using_same_parameter_array_sucessfully_returning_a_XmlReader_each_time()
        {
            string results;
            DbParameter[] parameters = new DbParameter[1];
            parameters[0] = DataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1");

            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllButFromTestTableAsXml", parameters))
            {
                reader.Read();
                reader.ReadOuterXml();
            }

            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllButFromTestTableAsXml", parameters))
            {
                reader.Read();
                results = reader.ReadOuterXml();
            }

            StringAssert.AreEqualIgnoringCase("<Entries><Entry><Key>key2</Key><Value>value2</Value></Entry><Entry><Key>key3</Key><Value>value3</Value></Entry><Entry><Key>key4</Key><Value>value4</Value></Entry><Entry><Key>key5</Key><Value>value5</Value></Entry></Entries>", results);
        }

        [Test]
        public void Should_not_clear_initial_parameter_list_when_calling_XmlReader()
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(DataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1"));

            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllButFromTestTableAsXml", parameters.ToArray() ))
            {
                reader.Read();
                reader.ReadOuterXml();
            }

            Assert.AreEqual(1, parameters.Count);
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
