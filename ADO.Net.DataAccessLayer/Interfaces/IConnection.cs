using System.Data;

namespace DataAccessLayer.Interfaces
{
    internal interface IConnection
    {
        IDbConnection DatabaseConnection { get;}

        bool InTransaction { get; set; }

        void Close();

        void Open();
    }
}