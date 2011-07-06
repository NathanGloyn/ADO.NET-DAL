using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.SqlServer
{
    internal class Connection
    {
        private readonly string connectionString;
        internal SqlConnection DatabaseConnection { get; set; }

        internal bool InTransaction{get; set;}


        internal Connection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Determines if the connection is currently in a transaction and
        /// close the connection if not.
        /// </summary>
        internal void SafelyCloseConnection()
        {

            if (! InTransaction)
            {
                DatabaseConnection.Close();
                DatabaseConnection.Dispose();
            }
        }

        internal void SafelyOpenConnection()
        {
            if (DatabaseConnection == null || DatabaseConnection.State != ConnectionState.Open)
            {
                DatabaseConnection = new SqlConnection(connectionString);
                DatabaseConnection.Open();
            }
        }        
    }
}