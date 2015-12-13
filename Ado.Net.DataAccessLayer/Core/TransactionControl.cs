using System.Data;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Core
{
    public class TransactionControl:ITransactionControl
    {
        private readonly IConnection connection;

        public IDbTransaction CurrentTransaction { get; private set; }

        internal TransactionControl(IConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Starts a transaction on the current connection
        /// </summary>
        public void BeginTransaction()
        {
            connection.Open();
            CurrentTransaction = connection.DatabaseConnection.BeginTransaction();
            connection.InTransaction = true;
        }

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        public void CommitTransaction()
        {
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Commit();
                CurrentTransaction = null;
                connection.InTransaction = false;
            }
        }

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        public void RollbackTransaction()
        {
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Rollback();
                CurrentTransaction = null;
                connection.InTransaction = false;
            }
        }
    }
}