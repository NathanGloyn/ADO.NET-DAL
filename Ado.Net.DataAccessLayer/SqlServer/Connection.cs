using System;
using System.Data;
using System.Data.SqlClient;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.SqlServer
{
    internal class Connection : IConnection
    {

        public string ConnectionString { get; private set; }

        public IDbConnection DatabaseConnection { get; private set; }
        public bool InTransaction{get; set;}

        internal Connection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Determines if the connection is currently in a transaction and
        /// close the connection if not.
        /// </summary>
        public void Close()
        {
            if (! InTransaction)
            {
                DatabaseConnection.Close();
                DatabaseConnection.Dispose();
            }
        }

        public void Open()
        {
            if (DatabaseConnection == null || DatabaseConnection.State != ConnectionState.Open)
            {
                DatabaseConnection = new SqlConnection(ConnectionString);
                DatabaseConnection.Open();
            }
        }        
    }
}