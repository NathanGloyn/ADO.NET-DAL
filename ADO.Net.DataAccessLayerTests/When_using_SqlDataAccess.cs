using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.SqlServer;
using NUnit.Framework;
using System.Xml;
using UnitTest.Database;

namespace ADO.Net.DataAccessLayerTests
{  
    /// <summary>
    ///This is a test class for SqlDataAccessTest and is intended
    ///to contain all SqlDataAccessTest Unit Tests
    ///</summary>
    [TestFixture()]
    public class When_using_SqlDataAccess
    {
        private const string _connectionName = "sqlTest";
        private static DatabaseSupport dbHelper;

        private SqlParameterFactory _parameterFactory;
        private DataAccess _dataAccess;

        [TestFixtureSetUp]
        public void InitializeTests()
        {
            dbHelper = new DatabaseSupport(ConfigurationManager.ConnectionStrings[_connectionName].ConnectionString);

            dbHelper.RunScript(@"..\..\TestScripts\CommonCreateScripts\01_create_tables.sql");
            dbHelper.RunScript(@"..\..\TestScripts\CommonCreateScripts\02_create_stored_procedures.sql");

            _parameterFactory = new SqlParameterFactory();
        }

        [SetUp]
        public void Setup()
        {
            dbHelper.RunScript(@"..\..\TestScripts\CommonCreateScripts\03_insert_test_data.sql");
            _dataAccess = new DataAccess(ConfigurationManager.ConnectionStrings[_connectionName].ConnectionString, _parameterFactory);
        }

        /// <summary>
        ///A test for Begin/RollbackTransaction
        ///</summary>
        [Test()]
        public void Should_rollbackTransaction_successfully()
        {
            _dataAccess.Transactions.BeginTransaction();

            _dataAccess.ExecuteNonQuery("AddToTestTable",
                _dataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "RollbackTest"),
                _dataAccess.ParameterFactory.Create("@TestValue", DbType.AnsiString, "ROLLBACK"));

            string testValue = (string)_dataAccess.ExecuteScalar("SelectFromTestTable", _dataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "RollbackTest"));
            Assert.AreEqual(testValue, "ROLLBACK");

            _dataAccess.Transactions.RollbackTransaction();
            
            testValue = (string)_dataAccess.ExecuteScalar("SelectFromTestTable", _dataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "RollbackTest"));
            Assert.IsNull(testValue);
        }

        /// <summary>
        ///A test for CommitTransaction
        ///</summary>
        [Test()]
        public void Should_commit_transaction_successfully()
        {
            _dataAccess.Transactions.BeginTransaction();

            _dataAccess.ExecuteNonQuery("AddToTestTable",
                _dataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "RollbackTest"),
                _dataAccess.ParameterFactory.Create("@TestValue", DbType.AnsiString, "ROLLBACK"));

            string result = (string)_dataAccess.ExecuteScalar("SelectFromTestTable", _dataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "RollbackTest"));
            Assert.AreEqual("ROLLBACK", result);

            _dataAccess.Transactions.CommitTransaction();

            result = (string)_dataAccess.ExecuteScalar("SelectFromTestTable", _dataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "RollbackTest"));
            Assert.AreEqual("ROLLBACK", result);
        }

        /// <summary>
        ///A test for ExecuteScalar(sp)
        ///</summary>
        [Test()]
        public void Should_execute_a_scalar_command_with_no_parameters_returning_the_correct_value()
        {
            string result = (string)_dataAccess.ExecuteScalar("SelectOneFromTestTable");
            Assert.AreEqual("value1", result);
        }

        /// <summary>
        ///A test for ExecuteScalar(sp, params)
        ///</summary>
        [Test()]
        public void Should_execute_a_scalar_command_with_a_parameter_returning_the_correct_value()
        {
            string result = (string)_dataAccess.ExecuteScalar("SelectFromTestTable", _dataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));
            Assert.AreEqual("value1", result);
        }

        /// <summary>
        ///A test for ExecuteScalar(cmd, sp, params)
        ///</summary>
        [Test()]
        public void Should_execute_a_scalar_command_with_a_parameter_returning_the_correct_value_and_the_command_object()
        {
            DbCommand cmd = null;

            string result = (string)_dataAccess.ExecuteScalar(out cmd, "SelectFromTestTable", _dataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));

            Assert.IsNotNull(cmd);
            Assert.AreEqual("key1", (string)cmd.Parameters["@TestKey"].Value);
            Assert.AreEqual("value1", result);
        }

        /// <summary>
        ///A test for ExecuteReader(sp)
        ///</summary>
        [Test()]
        public void Should_execute_a_command_with_no_parameters_returning_a_IDataReader()
        {
            IDataReader reader = _dataAccess.ExecuteReader("SelectAllFromTestTable");

            Assert.IsNotNull(reader);

            int count = 0;
            while (reader.Read())
                count++;

            Assert.AreEqual(5, count);
        }

        /// <summary>
        ///A test for ExecuteReader(sp, params)
        ///</summary>
        [Test()]
        public void Should_execute_a_command_with_a_parameter_returning_a_IDataReader()
        {
            IDataReader reader = _dataAccess.ExecuteReader("SelectAllButFromTestTable", _dataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1"));

            Assert.IsNotNull(reader);

            int count = 0;
            while (reader.Read())
                count++;

            Assert.AreEqual(4, count);
        }

        /// <summary>
        ///A test for ExecuteReader(sp, params)
        ///</summary>
        [Test()]
        public void Should_execute_muliple_data_readers_when_all_reading_at_the_same_time()
        {

            List<DataAccess> sqlDataAccessList = new List<DataAccess>(1000);
            
            string connectionString = ConfigurationManager.ConnectionStrings[_connectionName].ConnectionString;

            for (int i = 0; i < 1000; i++)
            {
                sqlDataAccessList.Add(new DataAccess(connectionString,_parameterFactory));
            }

            try
            {
                foreach (DataAccess item in sqlDataAccessList)
                {
                    using (DbDataReader reader = item.ExecuteReader("SelectAllButFromTestTable", item.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1")))
                    {
                        while (reader.Read()) { }
                    }
                }
            }
            catch (System.Exception ex)
            {
                //Clear all pools to ensure other tests will run sucessfully
                SqlConnection.ClearAllPools();
                throw ex;
            } 
            
            // We have no assert calls since there is nothing we can explicity check to see if the connection
            // has been used correctly, if something goes wrong then an error will be thrown and the test will fail
        }

        /// <summary>
        ///A test for ExecuteNonQuery(cmd, sp, params)
        ///</summary>
        [Test()]
        public void Should_execute_multiple_nonquery_commands_with_parameters_returning_a_command_and_the_correct_values()
        {
            DbCommand cmd = null;
            int rowsAffected = _dataAccess.ExecuteNonQuery(out cmd, "DeleteFromTestTable", _dataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(1, rowsAffected);           
            Assert.IsNotNull(cmd);
            Assert.AreEqual("key1", (string)cmd.Parameters["@TestKey"].Value);
            
            string result = (string)_dataAccess.ExecuteScalar("SelectFromTestTable", _dataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));
            Assert.IsNull(result);
        }

        /// <summary>
        ///A test for ExecuteNonQuery(sp, params)
        ///</summary>
        [Test()]
        public void Should_execute_multiple_nonquery_commands_with_parameters_returning_the_correct_values()
        {
            int rowsAffected = _dataAccess.ExecuteNonQuery("DeleteFromTestTable", _dataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(1, rowsAffected);

            string result = (string)_dataAccess.ExecuteScalar("SelectFromTestTable", _dataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));
            Assert.IsNull(result);
        }

        /// <summary>
        ///A test for ExecuteNonQuery(sp)
        ///</summary>
        [Test()]
        public void Should_execute_multiple_nonquery_commands_with_and_without_parameters_returning_the_correct_values()
        {
            int rowsAffected = _dataAccess.ExecuteNonQuery("DeleteOneFromTestTable");

            Assert.AreEqual(1, rowsAffected);

            string result = (string)_dataAccess.ExecuteScalar("SelectFromTestTable", _dataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));
            Assert.IsNull(result);
        }

        /// <summary>
        ///A test for ExecuteDataTable(cmd, sp, params)
        ///</summary>
        [Test()]
        public void Should_execute_command_with_parameters_returning_a_populated_DataTable_and_command_object()
        {
            DbCommand cmd;
            DataTable result = _dataAccess.ExecuteDataTable(out cmd, "SelectAllButFromTestTable", _dataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1"));

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
            DataTable result = _dataAccess.ExecuteDataTable("SelectAllButFromTestTable", _dataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(4, result.Rows.Count);
        }

        /// <summary>
        ///A test for ExecuteDataTable(sp)
        ///</summary>
        [Test()]
        public void Should_execute_command_with_no_parameters_returning_a_populated_DataTable()
        {
            DataTable result = _dataAccess.ExecuteDataTable("SelectAllFromTestTable");

            Assert.AreEqual(5, result.Rows.Count);
        }

        /// <summary>
        ///A test for ExecuteDataSet(cmd, sp, params)
        ///</summary>
        [Test()]
        public void Should_execute_command_with_parameters_returning_a_populated_DataSet_and_command_object()
        {
            DbCommand cmd;
            DataSet result = _dataAccess.ExecuteDataSet(out cmd, "SelectAllButFromTestTable", _dataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1"));

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
            DataSet result = _dataAccess.ExecuteDataSet("SelectAllButFromTestTable", _dataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(1, result.Tables.Count);
            Assert.AreEqual(4, result.Tables[0].Rows.Count);
        }

        /// <summary>
        ///A test for ExecuteDataSet(sp)
        ///</summary>
        [Test()]
        public void Should_execute_command_with_no_parameters_returning_a_populated_DataSet()
        {
            DataSet result = _dataAccess.ExecuteDataSet("SelectAllFromTestTable");

            Assert.AreEqual(1, result.Tables.Count);
            Assert.AreEqual(5, result.Tables[0].Rows.Count);
        }

        [Test]
        public void Should_execute_command_with_no_parameters_returning_a_XmlReader()
        {
            string results;
            using (XmlReader reader = _dataAccess.ExecuteXmlReader("SelectAllFromTestTableAsXml"))
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
            using (XmlReader reader = _dataAccess.ExecuteXmlReader("SelectAllButFromTestTableAsXml", _dataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1")))
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
            using (XmlReader reader = _dataAccess.ExecuteXmlReader(out cmd, "SelectAllButFromTestTableAsXml", _dataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1")))
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
            parameters[0] = _dataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1");

            using (XmlReader reader = _dataAccess.ExecuteXmlReader("SelectAllButFromTestTableAsXml", parameters))
            {
                reader.Read();
                reader.ReadOuterXml();
            }

            using (XmlReader reader = _dataAccess.ExecuteXmlReader("SelectAllButFromTestTableAsXml", parameters))
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
            parameters.Add(_dataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1"));

            using (XmlReader reader = _dataAccess.ExecuteXmlReader("SelectAllButFromTestTableAsXml", parameters.ToArray() ))
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

            _dataAccess.ExecuteScalar(out cmd, "SelectFromTestTable", _dataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(30, cmd.CommandTimeout);
        }

        [Test]
        public void Should_use_command_timeout_set_on_class()
        {
            _dataAccess.CommandTimeOut = 10;
            DbCommand cmd = null;

            _dataAccess.ExecuteScalar(out cmd, "SelectFromTestTable", _dataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "key1"));

            Assert.AreEqual(10, cmd.CommandTimeout);
        }

        
        public void s()
        {
            IDataAccess da = DataAccessFactory.Create("");  
            da.Transactions.BeginTransaction();
            da.ExecuteNonQuery("abc");
            da.Transactions.CommitTransaction();

        }
    }
}
