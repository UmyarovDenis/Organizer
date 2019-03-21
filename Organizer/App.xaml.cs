using AppController;
using AppController.Core.DIContainer;
using Organizer.Infrastructure.Modules;
using Organizer.Views;
using System.Windows;

namespace Organizer
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IContainerCore containerCore = new DIContainer(new ServiceModule());
            IControllerCore controllerCore = new ControllerCore(new ViewModule(), containerCore);

            Window mainView = controllerCore.ViewFactory.GetWindow<MainView>();
            mainView.Show();
        }
    }
}
