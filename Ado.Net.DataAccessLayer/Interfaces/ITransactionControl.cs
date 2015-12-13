using System.Data;
using System.Data.Common;

namespace DataAccessLayer.Interfaces
{
    public interface ITransactionControl
    {
        IDbTransaction CurrentTransaction { get; }

        /// <summary>
        /// Starts a transaction on the connection provided
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        void RollbackTransaction();
    }
}
