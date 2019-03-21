using ContractLibrary.DataTransfer.Dto;
using Organizer.Enums;
using Organizer.Services.Local;
using Organizer.Services.Net;
using Organizer.ViewModels.Core;
using System.Windows.Input;

namespace Organizer.ViewModels
{
    internal sealed class OrganizationViewModel : ContactViewModel
    {
        private ICommand _sendOrganizationMailCommand;

        public OrganizationViewModel(IMessageService<bool> messageService, IDataServiceProxy dataServiceProxy, 
            IMailService mailService, IImageManager<ImageType> imageManager,
            OrganizationDto organization) : base(messageService, dataServiceProxy, mailService, imageManager)
        {
            SelectedOrganization = organization;
        }
        public ICommand SendOrganizationMailCommand
        {
            get
            {
                return _sendOrganizationMailCommand ??
                    (_sendOrganizationMailCommand = new RelayCommand(arg => SendMail(SelectedOrganization.Email), arg => CanSendMail()));
            }
        }
        private bool CanSendMail()
        {
            return _mailService.IsValidMail(SelectedOrganization.Email);
        }
    }
}
