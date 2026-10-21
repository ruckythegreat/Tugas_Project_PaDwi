using Microsoft.Data.SqlClient;

namespace SMKRestaurant.Shared.Data
{
    /// <summary>
    /// Factory terpusat untuk membuat SqlConnection.
    /// Mencegah hardcode connection string di controller/form.
    /// Caller wajib Open() dan Dispose (gunakan using).
    /// </summary>
    public static class DbConnectionFactory
    {
        public static SqlConnection Create()
        {
            return new SqlConnection(DatabaseConfig.ConnectionString);
        }

        public static SqlConnection Create(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string tidak boleh kosong.", nameof(connectionString));

            return new SqlConnection(connectionString);
        }
    }
}
