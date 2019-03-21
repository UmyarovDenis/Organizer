using AdminPanel.Services.Local;
using AppController.Core.Dynamic;
using AppController.Core.Modules;

namespace AdminPanel.Infrastructure.Modules
{
    internal class ServiceModule : ContainerModule
    {
        public ServiceModule() : base(new Repository())
        {

        }
        public override void Load()
        {
            Bind<IMessageService<bool>>().To<MessageService>();
        }
    }
}
