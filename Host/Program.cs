using System;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "SERVER";
            Server server = Server.GetInstance();
            server.ServerStart += ConsoleLogger.OnServerStarted;
            server.HostStart += ConsoleLogger.OnServiceStarted;
            server.HostStop += ConsoleLogger.OnServiceStoped;
            server.ServerStop += ConsoleLogger.OnServerStop;

            bool isRestarted = false;
            
            do
            {
                Console.Clear();
                Console.ResetColor();

                server.Start();

                Console.ReadKey();

                server.Stop();
                server.Restart(ref isRestarted);

            } while (isRestarted);
        }
    }
}
