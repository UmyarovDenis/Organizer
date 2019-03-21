using ContractLibrary.DataTransfer.Dto;
using Organizer.Services.Local;
using Organizer.ViewModels.Core;

namespace Organizer.ViewModels
{
    internal class ContactCardViewModel : BaseViewModel
    {
        private ContactDto _contact;
        private string _title;

        public ContactCardViewModel(IMessageService<bool> messageService) : base(messageService)
        {
            _contact = new ContactDto();
            _title = "Новый контакт";
        }
        public ContactCardViewModel(IMessageService<bool> messageService, ContactDto contact)
            : base(messageService)
        {
            _contact = contact;
            _title = contact.Name;
        }
        public ContactDto Contact
        {
            get { return _contact; }
        }
        public string Title
        {
            get { return _title; }
        }
    }
}
