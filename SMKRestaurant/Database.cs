using Microsoft.Data.SqlClient;

namespace SMKRestaurant.Helpers
{
    public static class Database
    {
        // Ubah Server=localhost menjadi Server=RUCKIYE\SQLEXPRESS
        private static string connectionString = @"Server=RUCKIYE\SQLEXPRESS;Database=SMKRestaurant;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}