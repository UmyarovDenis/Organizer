using AppController.Core.Dynamic;
using AppController.Core.Modules;
using Organizer.ViewModels;
using Organizer.Views;
using Organizer.Views.Pages;

namespace Organizer.Infrastructure.Modules
{
    public class ViewModule : ControllerModule
    {
        public ViewModule() : base(new Repository())
        { }
        public override void Load()
        {
            #region WindowBindings

            BindView<MainView>().To<MainViewModel>();
            BindView<AuthorizationView>().To<AuthorizationViewModel>();
            BindView<ContactCardView>().To<ContactCardViewModel>();
            BindView<MailServiceSettingsView>().To<MailServiceSettingsViewModel>();
            BindView<OrganizationCardView>().To<OrganizationCardViewModel>();
            BindView<OrganizationView>().To<OrganizationViewModel>();
            BindView<SendMailView>().To<SendMailViewModel>();
            BindView<SqlCommandView>().To<SqlCommandViewModel>();
            BindView<TaskCardView>().To<TaskCardViewModel>();

            #endregion

            #region PageBindings

            BindView<CityView>().To<CityViewModel>();
            BindView<ProjectView>().To<ProjectViewModel>();

            #endregion
        }
    }
}
