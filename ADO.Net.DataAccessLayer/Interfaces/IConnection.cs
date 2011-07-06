using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DataAccessLayer.Interfaces
{
    internal interface IConnection
    {
        IDbConnection DatabaseConnection { get;}
        bool InTransaction { get; set; }

        /// <summary>
        /// Determines if the connection is currently in a transaction and
        /// close the connection if not.
        /// </summary>
        void Close();

        void Open();
    }
}