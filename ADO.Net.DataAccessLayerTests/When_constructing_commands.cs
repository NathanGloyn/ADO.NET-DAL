using System;
using System.Data;
using DataAccessLayer.SqlServer;
using NSubstitute;
using NUnit.Framework;

namespace ADO.Net.DataAccessLayer.SqlServer.Tests
{
    [TestFixture]
    public class When_constructing_commands
    {
        [Test]
        public void Should_throw_ArgumentNullException_if_no_connection_provided()
        {
            Assert.Throws<ArgumentNullException>(() => new Commands(null, Substitute.For<IDbTransaction>(), 10));
        }

    }
}