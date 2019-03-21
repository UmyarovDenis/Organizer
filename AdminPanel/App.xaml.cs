using System.Windows;
using System.Diagnostics;
using AppController.Core.DIContainer;
using AdminPanel.Infrastructure.Modules;
using AppController;
using AdminPanel.Views;
using AdminPanel.ViewModels;

namespace AdminPanel
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Process currentProcess = Process.GetCurrentProcess();

            if (Process.GetProcessesByName(currentProcess.ProcessName).Length > 1)
            {
                Application.Current.Shutdown();
                return;
            }

            base.OnStartup(e);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IContainerCore container = new DIContainer(new ServiceModule());
            IControllerCore controller = new ControllerCore(new ViewModule(), container);

            container.Bind<IControllerCore>().ToInstance(controller);

            Window window = controller.ViewFactory.GetWindow<MainView>();

            window.Show();
        }
    }
}
