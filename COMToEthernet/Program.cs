using static COMToEthernet.Form1;

namespace COMToEthernet
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Standard Windows Forms initialization
            ApplicationConfiguration.Initialize();

            // ----- Global Exception Handlers -----

            // 1. Catch exceptions on the WinForms UI thread
            Application.ThreadException += (sender, args) =>
            {
                Logger.Log($"[UI Thread Exception] {args.Exception}");
            };

            // 2. Catch exceptions on background threads and tasks
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var ex = args.ExceptionObject as Exception;
                Logger.Log($"[Non-UI Thread Exception] {ex}");
                // If args.IsTerminating is true, the process will exit after this.
            };

            // ---------------------------------------

            // Start the main form
            Application.Run(new Form1());
        }
    }
}