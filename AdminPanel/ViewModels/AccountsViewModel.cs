using AdminPanel.Services.Local;
using AdminPanel.Services.Net;
using AdminPanel.ViewModels.Core;
using ContractLibrary.DataTransfer.Dto;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace AdminPanel.ViewModels
{
    internal sealed class AccountsViewModel : BaseViewModel
    {
        private AuthorizationServiceProxy _proxy;

        private ObservableCollection<UserDto> _users;
        private UserDto _selectedUser;

        private int _userCount;
        private int _activeConnections;

        public AccountsViewModel(IMessageService<bool> messageService) : base(messageService)
        {
            try
            {
                _proxy = new AuthorizationServiceProxy();
                _users = new ObservableCollection<UserDto>(_proxy.GetServiceResult(p => p.GetUserList()));

                UserCount = Users.Count;
                ActiveConnections = Users.Count(u => u.IsConnected);
                SelectedUser = Users.FirstOrDefault(u => u.IsConnected);

                _users.CollectionChanged += Users_CollectionChanged;
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }

        private void Users_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Replace)
            {
                ActiveConnections = Users.Count(u => u.IsConnected);
                SelectedUser = Users.FirstOrDefault(u => u.IsConnected);
            }
        }

        public ObservableCollection<UserDto> Users
        {
            get { return _users; }
        }
        public UserDto SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                RaisePropertyChanged();
            }
        }
        public int UserCount
        {
            get { return _userCount; }
            set
            {
                _userCount = value;
                RaisePropertyChanged();
            }
        }
        public int ActiveConnections
        {
            get { return _activeConnections; }
            set
            {
                _activeConnections = value;
                RaisePropertyChanged();
            }
        }
    }
}
