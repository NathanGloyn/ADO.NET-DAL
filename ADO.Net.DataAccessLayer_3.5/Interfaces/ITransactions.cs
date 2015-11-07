namespace DataAccessLayer.Interfaces
{
    interface ITransactions
    {
        /// <summary>
        /// Starts a transaction on the current connection
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
