using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using DataAccessLayer.Core;
using DataAccessLayer.SqlServer;
using NUnit.Framework;

namespace ADO.Net.DataAccessLayer.SqlServer.Tests
{
    [TestFixture]
    public class When_executing_an_reader_command:CommonTestSetup
    {
        [Test()]
        public void Should_execute_a_command_with_no_parameters_returning_a_IDataReader()
        {
            IDataReader reader = DataAccess.ExecuteReader("SelectAllFromTestTable");

            Assert.IsNotNull(reader);

            int count = 0;
            while (reader.Read())
                count++;

            Assert.AreEqual(5, count);
        }

        [Test()]
        public void Should_execute_a_stored_procedure_with_spaces_in_its_name_with_no_parameters_returning_a_IDataReader()
        {
            IDataReader reader = DataAccess.ExecuteReader("[Sproc with spaces in name]");

            Assert.IsNotNull(reader);

            int count = 0;
            while (reader.Read())
                count++;

            Assert.AreEqual(5, count);
        }

        [Test()]
        public void Should_execute_a_command_Text_with_no_parameters_returning_a_IDataReader()
        {
            IDataReader reader = DataAccess.ExecuteReader("SELECT * FROM TestTable");

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
            IDataReader reader = DataAccess.ExecuteReader("SelectAllButFromTestTable", DataAccess.ParameterFactory.Create("@ExcludeKey", DbType.AnsiString, "key1"));

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

            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;

            for (int i = 0; i < 1000; i++)
            {
                sqlDataAccessList.Add(new DataAccess(connectionString));
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
            catch
            {
                //Clear all pools to ensure other tests will run sucessfully
                SqlConnection.ClearAllPools();
                Assert.Fail("Error has occured whilst attempting to execute multiple data readers");
            }

            // We have no assert calls since there is nothing we can explicity check to see if the connection
            // has been used correctly, if something goes wrong then an error will be thrown and the test will fail
        }        
    }
}