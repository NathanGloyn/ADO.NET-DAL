using System;
using DataAccessLayer.Core;
using DataAccessLayer.Interfaces;
using DataAccessLayer.SqlServer;
using NSubstitute;
using NUnit.Framework;

namespace ADO.Net.DataAccessLayer.SqlServer.Tests
{
    [TestFixture]
    public class When_constructing_DataAccess
    {
        [Test]
        public void Should_throw_ArgumentNotNullException_when_null_connection_string_provided()
        {
            Assert.Throws<ArgumentNullException>(() => new DataAccess(null));
        }

        [Test]
        public void Should_throw_ArgumentNotNullException_when_empty_connection_string_provided()
        {
            Assert.Throws<ArgumentNullException>(() => new DataAccess(string.Empty));
        }


        [Test]
        public void Should_construct_instance_when_all_dependencies_provided()
        {
            DataAccess target = new DataAccess("abc");
            Assert.IsNotNull(target);
        }

        [Test]
        public void Should_create_transaction_control_instance()
        {
            DataAccess target = new DataAccess("abc");
            Assert.IsNotNull(target.Transactions);
        }

        [Test]
        public void Should_create_parameter_factory_control_instance()
        {
            DataAccess target = new DataAccess("abc");
            Assert.IsNotNull(target.ParameterFactory);
        }
    }
}