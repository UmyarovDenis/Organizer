using AdminPanel.Extensions;
using AdminPanel.Services.Local;
using ContractLibrary.DataTransfer.Dto;
using ContractLibrary.Notifyer;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel;

namespace AdminPanel.Services.Net
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    internal class NotifyNodeService : INotifyNode
    {
        private static NotifyServiceConfiguration _configuration = new NotifyServiceConfiguration();
        private static NotifyNodeService _instance;

        private readonly ObservableCollection<ActionDto> _actionList;
        private readonly ObservableCollection<UserDto> _userList;

        private readonly IMessageService<bool> _messageService;

        private const int _maxItemsLimit = 200;

        private object _sync = new object();

        private NotifyNodeService(ObservableCollection<ActionDto> actionList, ObservableCollection<UserDto> userList)
        {
            _actionList = actionList;
            _userList = userList;

            _messageService = new MessageService();
        }
        public static void Configuration(Action<NotifyServiceConfiguration> action)
        {
            action.Invoke(_configuration);
        }
        public static NotifyNodeService CreateInstance()
        {
            if (_instance == null)
            {
                if (_configuration.ActionList == null || _configuration.UserList == null)
                {
                    throw new ArgumentNullException();
                }

                _instance = new NotifyNodeService(_configuration.ActionList, _configuration.UserList);

                _configuration.ActionList = null;
                _configuration.UserList = null;
            }

            return _instance;
        }
        public void Send(ActionDto userAction)
        {
            try
            {
                lock (_sync)
                {
                    if (userAction != null)
                    {
                        if (_actionList.Count == _maxItemsLimit)
                        {
                            _actionList.RemoveRangeByIndexes(0, _actionList.Count / 2);
                        }

                        _actionList.Add(userAction);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                _messageService.ShowError(ex.Message);
            }
            catch (IndexOutOfRangeException ex)
            {
                _messageService.ShowError(ex.Message);
            }
        }
        public void SendUserActivity(UserDto user)
        {
            try
            {
                lock (_sync)
                {
                    if (user != null)
                    {
                        UserDto userDto = _userList.FirstOrDefault(u => u.Id == user.Id);
                        
                        if(userDto != null)
                        {
                            _userList[_userList.IndexOf(userDto)] = user;
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _messageService.ShowError(ex.Message);
            }
        }
    }
}
