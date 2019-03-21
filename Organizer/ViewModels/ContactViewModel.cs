using AppController;
using AppController.Infrastructure.Attributes;
using ContractLibrary.DataTransfer.Dto;
using Organizer.Enums;
using Organizer.Infrastructure.Modules;
using Organizer.Models;
using Organizer.Services.Local;
using Organizer.Services.Net;
using Organizer.ViewModels.Core;
using Organizer.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Organizer.ViewModels
{
    internal class ContactViewModel : BaseViewModel
    {
        [Injected(typeof(ViewModule))]
        protected IControllerCore _controller;

        protected IMailService _mailService;
        protected IDataServiceProxy _dataProxy;
        protected IImageManager<ImageType> _imageManager;

        private OrganizationDto _selectedOrganization;
        private ContactDto _selectedContact;

        private ICommand _addNewContactCommand;
        private ICommand _changeContactCommand;
        private ICommand _removeContactCommand;
        private ICommand _sendContactMailCommand;

        private ObservableCollection<ContactDto> _contacts;

        public ContactViewModel(IMessageService<bool> messageService, IDataServiceProxy dataServiceProxy,
            IMailService mailService, IImageManager<ImageType> imageManager) : base(messageService)
        {
            _dataProxy = dataServiceProxy;
            _mailService = mailService;
            _imageManager = imageManager;
        }
        public OrganizationDto SelectedOrganization
        {
            get { return _selectedOrganization; }
            set
            {
                _selectedOrganization = value;
                Contacts = new ObservableCollection<ContactDto>(value.Contacts);
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<ContactDto> Contacts
        {
            get { return _contacts; }
            set
            {
                _contacts = value;
                RaisePropertyChanged();
            }
        }
        public ContactDto SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                RaisePropertyChanged();
            }
        }

        protected async void SendMail(string email)
        {
            try
            {
                MailMessageDto mailDto = _controller.ResolveView<SendMailView, SendMailViewModel>()
                    .RunDialogWithResult(vm => vm.MailObject);

                if (mailDto != null)
                {
                    await _mailService.SendMail(email, mailDto.Subject, mailDto.Text, mailDto.Attachments.ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }

        #region Commands

        public ICommand AddNewContactCommand
        {
            get
            {
                return _addNewContactCommand ??
                    (_addNewContactCommand = new RelayCommand(arg => AddNewContact(), arg => CanAddNewContact()));
            }
        }
        public ICommand ChangeContactCommand
        {
            get
            {
                return _changeContactCommand ??
                    (_changeContactCommand = new RelayCommand(arg => ChangeContact(arg as ContactDto), arg => CanChangeContact()));
            }
        }
        public ICommand RemoveContactCommand
        {
            get
            {
                return _removeContactCommand ??
                    (_removeContactCommand = new RelayCommand(arg => RemoveContact(arg as ContactDto), arg => CanRemoveContact()));
            }
        }
        public ICommand SendContactMailCommand
        {
            get
            {
                return _sendContactMailCommand ??
                    (_sendContactMailCommand = new RelayCommand(arg => SendMail(arg.ToString()), arg => CanSendContactMail()));
            }
        }

        #endregion

        #region Implemented Contact Commands

        protected virtual void AddNewContact()
        {
            try
            {
                ContactDto newContact = _controller.GetConfigs<ContactCardView, ContactCardViewModel>()
                    .View(v => v.Icon = _imageManager.GetImage(ImageType.ContactImage))
                    .RunDialogWithResult(vm => vm.Contact);

                if (newContact != null)
                {
                    newContact.Organization_Id = SelectedOrganization.Id;

                    var result = _dataProxy.CreateItems(newContact);
                    
                    if(result?.Count > 0)
                    {
                        SelectedOrganization.Contacts.Add(result.First());
                        Contacts.Add(result.First());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        protected virtual void ChangeContact(ContactDto contact)
        {
            try
            {
                bool? result = _controller.GetConfigs<ContactCardView, ContactCardViewModel>()
                    .View(v => v.Icon = _imageManager.GetImage(ImageType.ContactImage))
                    .AdditionalViewModelParams(contact)
                    .RunDialog();

                if (result == true)
                {
                    _dataProxy.UpdateItems(contact);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        protected virtual void RemoveContact(ContactDto contact)
        {
            try
            {
                bool dlgResult = MessageService.ShowQuestion("Удалить запись?");

                if (dlgResult)
                {
                    bool execResult = _dataProxy.RemoveItems(contact);

                    if (execResult)
                    {
                        SelectedOrganization.Contacts.Remove(contact);
                        Contacts.Remove(contact);
                        SelectedContact = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private bool CanAddNewContact()
        {
            return SelectedOrganization != null;
        }
        private bool CanChangeContact()
        {
            return SelectedContact != null;
        }
        private bool CanRemoveContact()
        {
            return _selectedOrganization?.Contacts.Count > 0
                && SelectedContact != null;
        }
        private bool CanSendContactMail()
        {
            if (SelectedContact == null)
                return false;

            return _mailService.IsValidMail(SelectedContact.Email);
        }

        #endregion
    }
}
