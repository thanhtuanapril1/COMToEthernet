using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMToEthernet
{
    public static class Logger
    {
        private static readonly object _fileLock = new object();

        public static void Log(string message)
        {
            try
            {
                // Build folder and filename
                string logDirectory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "COMToEthernet"
                );
                Directory.CreateDirectory(logDirectory);

                string timestamp = DateTime.Now.ToString("yyyyMMdd");
                string logFilePath = Path.Combine(logDirectory, $"log_{timestamp}.txt");

                // Thread-safe append
                lock (_fileLock)
                {
                    using (var writer = new StreamWriter(logFilePath, true))
                    {
                        writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}: {message}");
                    }
                }
            }
            catch
            {
                // Intentionally swallow all exceptions to avoid recursive crashes
            }
        }
    }
}
