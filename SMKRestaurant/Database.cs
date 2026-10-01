using Microsoft.Data.SqlClient;

namespace SMKRestaurant.Helpers
{
    public static class Database
    {
        private static string connectionString =
            @"Server=NAPTUNE\SQLEXPRESS;
              Database=SMKRestaurant;
              Trusted_Connection=True;
              TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}