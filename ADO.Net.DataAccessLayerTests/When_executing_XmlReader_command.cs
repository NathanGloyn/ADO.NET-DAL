using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using NUnit.Framework;
using System.Xml;


namespace ADO.Net.DataAccessLayer.SqlServer.Tests
{
    [TestFixture]
    public class When_executing_XmlReader_command : CommonTestSetup
    {

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

            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllButFromTestTableAsXml", parameters.ToArray()))
            {
                reader.Read();
                reader.ReadOuterXml();
            }

            Assert.AreEqual(1, parameters.Count);
        }

    }
}