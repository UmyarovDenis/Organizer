using System.ComponentModel;
using System.ServiceProcess;

namespace OrganizationHost
{
    [RunInstaller(true)]
    public partial class HostInstaller : System.Configuration.Install.Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;

        public HostInstaller()
        {
            InitializeComponent();

            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = "+++ OrganizerServer +++";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
