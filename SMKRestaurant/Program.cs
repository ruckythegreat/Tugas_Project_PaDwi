using SMKRestaurant.Helpers;

namespace SMKRestaurant;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Tangkap semua unhandled exception agar aplikasi tidak crash mentah
        GlobalExceptionHandler.Register();

        Application.Run(new Forms.FrmLogin());
    }
}
