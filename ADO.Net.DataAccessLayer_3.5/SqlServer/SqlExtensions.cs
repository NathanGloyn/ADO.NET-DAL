using System.Xml;
using System.Data.SqlClient;

namespace DataAccessLayer.SqlServer
{
    /// <summary>
    /// BH custom extenstions to SqlCommand for ease of use
    /// </summary>
    public static class SqlExtensions
    {
        /// <summary>
        /// Executes the XML reader using our proxy class
        /// </summary>
        /// <param name="cmd">The CMD.</param>
        /// <remarks>Uses our proxy XmlReader so that we can control the underlying SqlConnection</remarks>
        /// <returns></returns>
        public static XmlReader ExecuteSafeXmlReader(this SqlCommand cmd)
        {
            return new SqlXmlReader(cmd);
        }
    }
}
