using System.Data;
using NUnit.Framework;

namespace ADO.Net.DataAccessLayer.SqlServer.Tests
{
    [TestFixture]
    public class When_using_transactions:CommonTestSetup
    {
        [Test()]
        public void Should_rollbackTransaction_successfully()
        {
            DataAccess.Transactions.BeginTransaction();

            DataAccess.ExecuteNonQuery("AddToTestTable",
                                        DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "RollbackTest"),
                                        DataAccess.ParameterFactory.Create("@TestValue", DbType.AnsiString, "ROLLBACK"));

            string testValue = (string)DataAccess.ExecuteScalar("SelectFromTestTable", DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "RollbackTest"));
            Assert.AreEqual(testValue, "ROLLBACK");

            DataAccess.Transactions.RollbackTransaction();

            testValue = (string)DataAccess.ExecuteScalar("SelectFromTestTable", 
                                                          DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "RollbackTest"));
            Assert.IsNull(testValue);
        }

        [Test()]
        public void Should_commit_transaction_successfully()
        {
            DataAccess.Transactions.BeginTransaction();

            DataAccess.ExecuteNonQuery("AddToTestTable",
                                        DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "RollbackTest"),
                                        DataAccess.ParameterFactory.Create("@TestValue", DbType.AnsiString, "ROLLBACK"));

            string result = (string)DataAccess.ExecuteScalar("SelectFromTestTable", 
                                                             DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "RollbackTest"));
            Assert.AreEqual("ROLLBACK", result);

            DataAccess.Transactions.CommitTransaction();

            result = (string)DataAccess.ExecuteScalar("SelectFromTestTable", DataAccess.ParameterFactory.Create("@TestKey", DbType.AnsiString, "RollbackTest"));
            Assert.AreEqual("ROLLBACK", result);
        }
    }
}