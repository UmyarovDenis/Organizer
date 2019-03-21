using AppController.Core.Dynamic;
using AppController.Core.Modules;
using Organizer.Enums;
using Organizer.Services.FileServises;
using Organizer.Services.Local;
using Organizer.Services.Net;

namespace Organizer.Infrastructure.Modules
{
    internal class ServiceModule : ContainerModule
    {
        public ServiceModule() : base(new Repository())
        { }

        public override void Load()
        {
            Bind<IDataServiceProxy>().To<DataServiceProxy>().WithArguments(this);
            Bind<ISqlServiceProxy>().To<SqlServiceProxy>();
            Bind<IAuthServiceProxy>().To<AuthServiceProxy>();

            Bind<IMessageService<bool>>().To<MessageService>();
            Bind<IMailService>().To<MailService>();
            Bind<ISettingService>().To<SettingService>();
            Bind<IFileDialogService<FileObject>>().To<MultiselectFileService>();
            Bind<IFileDialogService<string>>().To<FileDialogService>();
            Bind<IImageManager<ImageType>>().To<ImageManager>();
        }
    }
}
