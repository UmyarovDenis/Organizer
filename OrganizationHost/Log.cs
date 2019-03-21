using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace OrganizationHost
{
    internal static class Log
    {
        private const string _directoryName = "ServerLogs";
        private const string _fileName = "Logs.txt";

        public static void Write(string message)
        {
            string execPath = @"E:\Programming\C#\Organizer\AdminPanel\bin\Debug\Core";
            string directoryPath = Path.Combine(execPath, _directoryName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (StreamWriter sw = new StreamWriter(Path.Combine(directoryPath, _fileName), true, Encoding.Default))
            {
                sw.WriteLine(message);
                sw.WriteLine(new string('-', 40));
            }
        }
    }
}
