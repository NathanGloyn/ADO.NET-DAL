using DataAccessLayer.Interfaces;
using DataAccessLayer.SqlServer;

namespace DataAccessLayer
{
    /// <summary>
    /// Class encapsulating construction of IDataAccess Types
    /// </summary>
    public static class DataAccessFactory
    {
        /// <summary>
        /// Creates an IDataAccess implementation based on the provider specified in the connection string
        /// </summary>
        /// <param name="config">The connection string for the provider</param>
        /// <returns>IDataAccess object</returns>
        /// <remarks>Currently only handling the creation of DataAccess classes, the factory exists to provide an extension point 
        /// where additional data access can be created with out having to refactor a lot of code</remarks>
        public static IDataAccess Create(string config)
        {
            return new DataAccess(config,new SqlParameterFactory());
        }
    }
}
