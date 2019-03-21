using ContractLibrary.DataTransfer.Dto;
using ContractLibrary.Exceptions;
using Organizer.Services.Net;
using Organizer.ViewModels.Core;
using System.ServiceModel;
using System.Windows.Input;
using System;
using Organizer.Services.Local;

namespace Organizer.ViewModels
{
    internal sealed class AuthorizationViewModel : BaseViewModel
    {
        private IAuthServiceProxy _authServiceProxy;
        private ICommand _authorizeCommand;

        private string _login;
        private string _password;
        private UserDto _user;

        public AuthorizationViewModel(IAuthServiceProxy authServiceProxy, IMessageService<bool> messageService) 
            : base(messageService)
        {
            _authServiceProxy = authServiceProxy;
        }
        public UserDto User
        {
            get { return _user; }
            set
            {
                _user = value;
            }
        }
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                RaisePropertyChanged();
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged();
            }
        }
        public ICommand AuthorizeCommand
        {
            get
            {
                return _authorizeCommand ??
                    (_authorizeCommand = new RelayCommand(arg => Authorize(), arg => CanAuthorize()));
            }
        }
        private void Authorize()
        {
            try
            {
                User = _authServiceProxy.Authorizate(Login, Password);
            }
            catch (FaultException<AuthorizationFault> ex)
            {
                MessageService.ShowError(ex.Message);
            }
            catch(CommunicationException ex)
            {
                MessageService.ShowError(ex.Message);
            }
            catch(Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private bool CanAuthorize()
        {
            return !string.IsNullOrEmpty(Login)
                && !string.IsNullOrEmpty(Password);
        }
    }
}
