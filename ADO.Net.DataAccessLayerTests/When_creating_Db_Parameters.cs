using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using DataAccessLayer.SqlServer;
using NUnit.Framework;

namespace ADO.Net.DataAccessLayer.SqlServer.Tests
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

        [TestCase(DbType.AnsiString, Result = SqlDbType.VarChar)]
        [TestCase(DbType.AnsiStringFixedLength, Result = SqlDbType.Char)]
        [TestCase(DbType.String, Result = SqlDbType.NVarChar)]
        [TestCase(DbType.StringFixedLength, Result = SqlDbType.NChar)]
        [TestCase(DbType.Xml, Result = SqlDbType.Xml)]
        public SqlDbType Should_create_text_parameter_with_correct_data_type(DbType suppliedType)
        {
            SqlParameter result = parameterFactory.Create("@TestParam", suppliedType, ParameterDirection.Input) as SqlParameter;
            return result.SqlDbType;
        }

        [TestCase(DbType.Byte, Result = SqlDbType.TinyInt)]
        [TestCase(DbType.Currency, Result = SqlDbType.Money)]
        [TestCase(DbType.Decimal, Result = SqlDbType.Decimal)]
        [TestCase(DbType.Double, Result = SqlDbType.Float)]
        [TestCase(DbType.Int16, Result = SqlDbType.SmallInt)]
        [TestCase(DbType.Int32, Result = SqlDbType.Int)]
        [TestCase(DbType.Int64, Result = SqlDbType.BigInt)]
        [TestCase(DbType.Single, Result = SqlDbType.Real)]
        public SqlDbType Should_create_numeric_parameter_with_correct_data_type(DbType suppliedType)
        {
            SqlParameter result = parameterFactory.Create("@TestParam", suppliedType, ParameterDirection.Input) as SqlParameter;
            return result.SqlDbType;
        }

        [TestCase(DbType.Date, Result = SqlDbType.Date)]
        [TestCase(DbType.DateTime, Result = SqlDbType.DateTime)]
        [TestCase(DbType.DateTime2, Result = SqlDbType.DateTime2)]
        [TestCase(DbType.DateTimeOffset, Result = SqlDbType.DateTimeOffset)]
        [TestCase(DbType.Time, Result = SqlDbType.Time)]
        public SqlDbType Should_create_datetime_parameter_with_correct_data_type(DbType suppliedType)
        {
            SqlParameter result = parameterFactory.Create("@TestParam", suppliedType, ParameterDirection.Input) as SqlParameter;
            return result.SqlDbType;
        }

        [TestCase(DbType.Binary, Result = SqlDbType.Binary)]
        [TestCase(DbType.Boolean, Result = SqlDbType.Bit)]
        [TestCase(DbType.Guid, Result = SqlDbType.UniqueIdentifier)]
        [TestCase(DbType.Object,Result  = SqlDbType.Variant)]
        public SqlDbType Should_create_parameter_with_correct_data_type(DbType suppliedType)
        {
            SqlParameter result = parameterFactory.Create("@TestParam", suppliedType, ParameterDirection.Input) as SqlParameter;
            return result.SqlDbType;
        }

        [TestCase(DbType.SByte, ExpectedException = typeof(ArgumentException))]
        [TestCase(DbType.UInt16, ExpectedException = typeof(ArgumentException))]
        [TestCase(DbType.UInt32, ExpectedException = typeof(ArgumentException))]
        [TestCase(DbType.UInt64, ExpectedException = typeof(ArgumentException))]
        [TestCase(DbType.VarNumeric, ExpectedException = typeof(ArgumentException))]
        [TestCase((DbType)33 , ExpectedException = typeof(ArgumentException))]
        public void Should_throw_exception(DbType suppliedType)
        {
            parameterFactory.Create("@TestParam", suppliedType, ParameterDirection.Input);
        }
    }
}
