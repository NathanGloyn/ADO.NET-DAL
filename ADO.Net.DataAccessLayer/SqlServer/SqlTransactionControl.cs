using System;
using System.Data.Common;
using System.Data.SqlClient;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.SqlServer
{
    public class SqlTransactionControl:ITransactionControl
    {
        private readonly DbConnection _connection;
        private SqlTransaction currentTransaction = null;

        public DbTransaction CurrentTransaction
        {
            get { return currentTransaction as DbTransaction; }
        }

        public SqlTransactionControl(DbConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Starts a transaction on the current connection
        /// </summary>
        public void BeginTransaction()
        {
            currentTransaction = ((SqlConnection)_connection).BeginTransaction();
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
            }
        }
    }
}