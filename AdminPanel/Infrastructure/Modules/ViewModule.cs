using AdminPanel.ViewModels;
using AdminPanel.Views;
using AdminPanel.Views.Pages;
using AppController.Core.Dynamic;
using AppController.Core.Modules;

namespace AdminPanel.Infrastructure.Modules
{
    internal class ViewModule : ControllerModule
    {
        public ViewModule() : base(new Repository())
        {

        }
        public override void Load()
        {
            BindView<MainView>().To<MainViewModel>();

            #region PageBindings

            BindView<AccountsView>().To<AccountsViewModel>();
            BindView<MonitorView>().To<MonitorViewModel>();
            BindView<ServicesView>().To<ServicesViewModel>();
            BindView<SettingsView>().To<SettingsViewModel>();

            #endregion
        }
    }
}
