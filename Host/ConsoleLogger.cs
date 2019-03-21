using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    static class ConsoleLogger
    {
        private static DateTime _startDate;
        private static DateTime _stopDate;

        public static void OnServerStarted()
        {
            Console.WriteLine("{0}{1}{2}", "+", new string('-', 60), "+");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"~~~~~~ SERVER RUNNING ~~~~~~");
            Console.ResetColor();

            _startDate = DateTime.Now;

            Console.WriteLine();
            Console.WriteLine($"Sart date: {_startDate}");
        }
        public static void OnServiceStarted(string typeContract, ServiceHost host)
        {
            Console.WriteLine();
            Console.WriteLine("{0}{1}{2}", "+", new string('-', 60), "+");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"Service: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{typeContract} is started");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"Address: {host.Description.Endpoints[0].Address}");
            Console.WriteLine($"Binding: {host.Description.Endpoints[0].Binding}");
            Console.WriteLine($"Contract: {host.Description.Endpoints[0].Contract.Name}");
            Console.WriteLine($"OpenTimeout - {host.Description.Endpoints[0].Binding.OpenTimeout} | CloseTimeout - {host.Description.Endpoints[0].Binding.CloseTimeout}");
            Console.WriteLine($"SendTimeout - {host.Description.Endpoints[0].Binding.SendTimeout} | ReceiveTimeout - {host.Description.Endpoints[0].Binding.ReceiveTimeout}");
            Console.ResetColor();
            Console.WriteLine();
        }
        public static void OnServiceStoped(ServiceHost host, DateTime stopDate)
        {
            Console.WriteLine();
            Console.WriteLine("{0}{1}{2}", "+", new string('-', 60), "+");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"Service: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{host.Description.Endpoints[0].Contract.Name} is stoped");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"Address: {host.Description.Endpoints[0].Address}");
            Console.WriteLine($"Binding: {host.Description.Endpoints[0].Binding}");
            Console.WriteLine($"Contract: {host.Description.Endpoints[0].Contract.Name}");
            Console.WriteLine($"Stop date: {stopDate}");
            Console.ResetColor();
            Console.WriteLine();
        }
        public static void OnServerStop()
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("~~~~~~ SERVER STOPPED ~~~~~~");
            Console.WriteLine();
            Console.ResetColor();

            _stopDate = DateTime.Now;

            Console.WriteLine($"Sart date:     {_startDate}");
            Console.WriteLine($"Stop date:     {_stopDate}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Working hours: {_stopDate.ToLocalTime() - _startDate.ToLocalTime()}");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
