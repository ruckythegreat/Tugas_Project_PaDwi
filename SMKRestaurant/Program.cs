namespace SMKRestaurant;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        // DIPERBAIKI: Mengubah startup form ke FrmLogin
        Application.Run(new Forms.FrmLogin());
    }    
}