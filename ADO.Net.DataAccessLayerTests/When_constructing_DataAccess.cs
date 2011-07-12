using System;
using DataAccessLayer.Interfaces;
using DataAccessLayer.SqlServer;
using NSubstitute;
using NUnit.Framework;

namespace ADO.Net.DataAccessLayerTests
{
    [TestFixture]
    public class When_constructing_DataAccess
    {
        [Test]
        public void Should_throw_ArgumentNotNullException_when_null_connection_string_provided()
        {
            Assert.Throws<ArgumentNullException>(() => new DataAccess(null, Substitute.For<IParameterCreation>()));
        }

        [Test]
        public void Should_throw_ArgumentNotNullException_when_empty_connection_string_provided()
        {
            Assert.Throws<ArgumentNullException>(() => new DataAccess(string.Empty, Substitute.For<IParameterCreation>()));
        }

        [Test]
        public void Should_throw_ArgumentNotNullException_when_no_parameter_factory_provided()
        {
            Assert.Throws<ArgumentNullException>(() => new DataAccess("abc", null));
        }

        [Test]
        public void Should_construct_instance_when_all_dependencies_provided()
        {
            DataAccess target = new DataAccess("abc",Substitute.For<IParameterCreation>());
            Assert.IsNotNull(target);
        }

        [Test]
        public void Should_create_transaction_control_instance()
        {
            DataAccess target = new DataAccess("abc", Substitute.For<IParameterCreation>());
            Assert.IsNotNull(target.Transactions);
        }
    }
}