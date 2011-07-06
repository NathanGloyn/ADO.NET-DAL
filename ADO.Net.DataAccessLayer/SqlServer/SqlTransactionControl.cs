using System.Data.Common;
using System.Data.SqlClient;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.SqlServer
{
    public class SqlTransactionControl:ITransactionControl
    {
        private readonly Connection connection;
        private SqlTransaction currentTransaction = null;

        public DbTransaction CurrentTransaction
        {
            get { return currentTransaction as DbTransaction; }
        }

        internal SqlTransactionControl(Connection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Starts a transaction on the current connection
        /// </summary>
        public void BeginTransaction()
        {
            connection.SafelyOpenConnection();
            currentTransaction = connection.DatabaseConnection.BeginTransaction();
            connection.InTransaction = true;
        }

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        public void CommitTransaction()
        {
            if (currentTransaction != null)
            {
                currentTransaction.Commit();
                currentTransaction = null;
                connection.InTransaction = false;
            }
        }

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        public void RollbackTransaction()
        {
            if (currentTransaction != null)
            {
                currentTransaction.Rollback();
                currentTransaction = null;
                connection.InTransaction = false;
            }
        }
    }
}