using Microsoft.Data.SqlClient;
using SMKRestaurant.Shared.Data;

namespace SMKRestaurant.Helpers
{
    /// <summary>
    /// Facade WinForms ke factory terpusat (SMKRestaurant.Shared).
    /// Jangan hardcode connection string di sini.
    /// </summary>
    public static class Database
    {
        public static SqlConnection GetConnection()
        {
            return DbConnectionFactory.Create();
        }
    }
}
