namespace SMKRestaurant.Shared.Data
{
    /// <summary>
    /// Single Source of Truth untuk konfigurasi koneksi SQL Server.
    /// Digunakan oleh WinForms (SMKRestaurant) dan API (SMKRestaurantAPI).
    /// Ubah hanya file ini bila server/database berubah.
    /// </summary>
    public static class DatabaseConfig
    {
        public const string Server = @"RUCKIYE\SQLEXPRESS";
        public const string DatabaseName = "SMKRestaurant";
        public const bool IntegratedSecurity = true;
        public const bool Encrypt = true;
        public const bool TrustServerCertificate = true;

        /// <summary>
        /// Connection string resmi yang dipakai seluruh sistem.
        /// </summary>
        public static string ConnectionString { get; } =
            $"Server={Server};" +
            $"Database={DatabaseName};" +
            $"Integrated Security={IntegratedSecurity};" +
            $"Encrypt={Encrypt};" +
            $"TrustServerCertificate={TrustServerCertificate};";
    }
}
