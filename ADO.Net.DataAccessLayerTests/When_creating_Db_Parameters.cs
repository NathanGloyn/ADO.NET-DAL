using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using DataAccessLayer.SqlServer;
using NUnit.Framework;

namespace ADO.Net.DataAccessLayerTests
{
    [TestFixture]
    class When_creating_Db_Parameters
    {
        private SqlParameterFactory parameterFactory;
        private const string mockConnectionString = "test";

        [SetUp]
        public void FixtureSetup()
        {
            parameterFactory = new SqlParameterFactory();
        }

        /// <summary>
        ///A test for CreateParameter(name, type, value, size, direction)
        ///</summary>
        [Test()]
        public void Should_create_a_parameter_when_specifying_name_type_value_size_direction()
        {

            DbParameter result = parameterFactory.Create("@TestParam", DbType.AnsiString, mockConnectionString, 10, ParameterDirection.Input) as SqlParameter;
            Assert.AreEqual("@TestParam", result.ParameterName);
            Assert.AreEqual(DbType.AnsiString, result.DbType);
            Assert.AreEqual(mockConnectionString, result.Value);
            Assert.AreEqual(10, result.Size);
            Assert.AreEqual(ParameterDirection.Input, result.Direction);
        }

        /// <summary>
        ///A test for CreateParameter(name, type, value, size, precision)
        ///</summary>
        [Test()]
        public void Should_create_a_parameter_when_specifying_name_type_value_size_precision()
        {
            SqlParameter result = parameterFactory.Create("@TestParam", DbType.Decimal, 10.123, 10, 3) as SqlParameter;
            Assert.AreEqual("@TestParam", result.ParameterName);
            Assert.AreEqual(DbType.Decimal, result.DbType);
            Assert.AreEqual(10.123, result.Value);
            Assert.AreEqual(10, result.Size);
            Assert.AreEqual(3, result.Precision);
        }

        /// <summary>
        ///A test for CreateParameter(name, type, value, size, precision, direction)
        ///</summary>
        [Test()]
        public void Should_create_a_parameter_when_specifying_name_type_value_size_precision_direction()
        {
            SqlParameter result = parameterFactory.Create("@TestParam", DbType.Decimal, 10.123, 10, 3, ParameterDirection.Input) as SqlParameter;
            Assert.AreEqual("@TestParam", result.ParameterName);
            Assert.AreEqual(DbType.Decimal, result.DbType);
            Assert.AreEqual(10.123, result.Value);
            Assert.AreEqual(10, result.Size);
            Assert.AreEqual(3, result.Precision);
            Assert.AreEqual(ParameterDirection.Input, result.Direction);
        }

        /// <summary>
        ///A test for CreateParameter(name, type, direction)
        ///</summary>
        [Test()]
        public void Should_create_a_parameter_when_specifying_name_type_direction()
        {
            SqlParameter result = parameterFactory.Create("@TestParam", DbType.Decimal, ParameterDirection.Input) as SqlParameter;
            Assert.AreEqual("@TestParam", result.ParameterName);
            Assert.AreEqual(DbType.Decimal, result.DbType);
            Assert.AreEqual(ParameterDirection.Input, result.Direction);
        }

        /// <summary>
        ///A test for CreateParameter(name, type, value)
        ///</summary>
        [Test()]
        public void Should_create_a_parameter_when_specifying_name_type_value()
        {
            SqlParameter result = parameterFactory.Create("@TestParam", DbType.Decimal, 10.123) as SqlParameter;
            Assert.AreEqual("@TestParam", result.ParameterName);
            Assert.AreEqual(DbType.Decimal, result.DbType);
            Assert.AreEqual(10.123, result.Value);
        }

        /// <summary>
        ///A test for CreateParameter(name, type, value, size)
        ///</summary>
        [Test()]
        public void Should_create_a_parameter_when_specifying_name_type_value_size()
        {
            SqlParameter result = parameterFactory.Create("@TestParam", DbType.Decimal, 10.123, 10) as SqlParameter;
            Assert.AreEqual("@TestParam", result.ParameterName);
            Assert.AreEqual(DbType.Decimal, result.DbType);
            Assert.AreEqual(10.123, result.Value);
            Assert.AreEqual(10, result.Size);
        }

        /// <summary>
        ///A test for CreateParameter(name, type, value, direction)
        ///</summary>
        [Test()]
        public void Should_create_a_parameter_when_specifying_name_type_value_direction()
        {
            SqlParameter result = parameterFactory.Create("@TestParam", DbType.Decimal, 10.123, ParameterDirection.Input) as SqlParameter;
            Assert.AreEqual("@TestParam", result.ParameterName);
            Assert.AreEqual(DbType.Decimal, result.DbType);
            Assert.AreEqual(10.123, result.Value);
            Assert.AreEqual(ParameterDirection.Input, result.Direction);
        }
    }
}
