using System.Text;

namespace SMKRestaurant.Helpers
{
    /// <summary>
    /// Handler exception global agar WinForms tidak crash diam-diam.
    /// Menampilkan pesan ramah ke user dan menulis log ke file.
    /// </summary>
    public static class GlobalExceptionHandler
    {
        private static readonly object LogLock = new();

        public static void Register()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += OnThreadException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Handle(e.Exception, isTerminating: false);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception
                     ?? new Exception(e.ExceptionObject?.ToString() ?? "Unknown error");
            Handle(ex, isTerminating: e.IsTerminating);
        }

        private static void Handle(Exception exception, bool isTerminating)
        {
            try
            {
                WriteLog(exception, isTerminating);
            }
            catch
            {
                // Jangan biarkan logging menggagalkan penanganan error.
            }

            var message = BuildUserMessage(exception, isTerminating);

            try
            {
                MessageBox.Show(
                    message,
                    "SMK Restaurant — Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch
            {
                // UI mungkin sudah tidak tersedia.
            }
        }

        private static string BuildUserMessage(Exception exception, bool isTerminating)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Terjadi kesalahan yang tidak terduga.");
            sb.AppendLine();
            sb.AppendLine(GetFriendlyMessage(exception));

            if (isTerminating)
            {
                sb.AppendLine();
                sb.AppendLine("Aplikasi mungkin akan ditutup.");
            }

            sb.AppendLine();
            sb.AppendLine("Detail teknis telah disimpan ke file log.");
            return sb.ToString();
        }

        private static string GetFriendlyMessage(Exception exception)
        {
            var text = exception.Message ?? string.Empty;

            if (text.Contains("SQL Server", StringComparison.OrdinalIgnoreCase)
                || text.Contains("SqlConnection", StringComparison.OrdinalIgnoreCase)
                || text.Contains("network-related", StringComparison.OrdinalIgnoreCase)
                || exception.GetType().Name.Contains("Sql", StringComparison.OrdinalIgnoreCase))
            {
                return "Gagal terhubung ke database. Pastikan SQL Server (RUCKIYE\\SQLEXPRESS) berjalan.";
            }

            return string.IsNullOrWhiteSpace(text)
                ? "Silakan coba lagi atau hubungi administrator."
                : text;
        }

        private static void WriteLog(Exception exception, bool isTerminating)
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "SMKRestaurant",
                "logs");

            Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, $"error-{DateTime.Now:yyyyMMdd}.log");
            var content =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Terminating={isTerminating}{Environment.NewLine}" +
                $"{exception}{Environment.NewLine}" +
                new string('-', 60) + Environment.NewLine;

            lock (LogLock)
            {
                File.AppendAllText(filePath, content);
            }
        }
    }
}
