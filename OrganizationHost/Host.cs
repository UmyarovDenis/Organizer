using System;
using System.ServiceProcess;

namespace OrganizationHost
{
    partial class Host : ServiceBase
    {
        private Server _server;

        public Host()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _server = Server.GetInstance();

                _server.Start();
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
                Log.Write(ex?.InnerException.Message);
            }
        }
        protected override void OnStop()
        {
            try
            {
                if(_server != null)
                {
                    _server.Stop();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }
    }
}
