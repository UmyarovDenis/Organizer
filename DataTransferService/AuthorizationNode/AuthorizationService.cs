using ContractLibrary.Authorization;
using ContractLibrary.DataTransfer.Dto;
using ContractLibrary.Exceptions;
using DataTransferService.DataAccessLayer.Mapping;
using DataTransferService.DataAccessLayer.Models;
using DataTransferService.DataAccessLayer.UnitOfWork;
using DataTransferService.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DataTransferService.AuthorizationNode
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class AuthorizationService : IAuthorizationContract
    {
        private NotifyProxyService _proxy;
        private Context _context;
        private Mutator<User> _mutator;

        private List<UserDto> _userList;

        public AuthorizationService()
        {
            _context = new Context();
            _mutator = new Mutator<User>();
            _userList = new List<UserDto>();
            _proxy = new NotifyProxyService();
        }
        public List<UserDto> GetUserList()
        {
            try
            {
                List<UserDto> users = _mutator.EntitiesToDtoList<UserDto>(_context.UserRepository.GetAll());

                users.ForEach(u => 
                {
                    UserDto connectedUser = _userList.FirstOrDefault(user => user.Id == u.Id);

                    if (connectedUser != null)
                    {
                        u.IsConnected = connectedUser.IsConnected;
                        u.Host = connectedUser.Host;
                    }
                });

                return users;
            }
            catch (Exception ex)
            {
                throw new FaultException<Exception>(ex, new FaultReason(ex.Message));
            }
        }
        public UserDto Authorizate(AuthorizationRequest authorizationRequest)
        {
            try
            {
                User user = _context.UserRepository
                                    .GetItem(u => u.Login == authorizationRequest.Login
                                             && u.Password == authorizationRequest.Password);

                if (user == null)
                {
                    UserDto unknownUser = new UserDto();
                    unknownUser.FIO = "Неизвестно";
                    unknownUser.Host = authorizationRequest.Host;

                    SendMessageAsync(CreateAction(unknownUser, "Попытка входа", "Пользователь не зарегистрирован"));

                    throw new FaultException<AuthorizationFault>(
                            new AuthorizationFault("Такой пользователь не зарегистрирован"),
                                new FaultReason("Ошибка авторизации.\r\nТакой пользователь не зарегистрирован"));
                }

                UserDto userDto = _mutator.EntityMutator.Map<UserDto>(user);
                userDto.Host = authorizationRequest.Host;
                userDto.IsConnected = true;

                _userList.Add(userDto);

                SendMessageAsync(CreateAction(userDto, "Connect", $"Логин: {user.Login}"));
                SendActivityAsync(userDto);

                return userDto;
            }
            catch (ArgumentNullException ex)
            {
                throw new FaultException<ArgumentNullException>(ex, new FaultReason(ex.Message));
            }
        }

        public void Disconnect(UserDto user)
        {
            try
            {
                if(user != null)
                {
                    _userList.RemoveAt(_userList.FindIndex(u => u.Id == user.Id));
                    user.IsConnected = false;

                    SendMessageAsync(CreateAction(user, "Disconnect", $"Логин: {user.Login}"));

                    user.Host.Erase();

                    SendActivityAsync(user);
                }
            }
            catch (Exception)
            {

            }
        }

        public void RestorePassword(string email)
        {
            throw new NotImplementedException();
        }
        private async void SendMessageAsync(ActionDto action)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    try
                    {
                        _proxy.ProxyService.Send(action);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Write($"Error: {ex.Message} | InnerException {ex?.InnerException.Message}");
                    }
                });
            }
            catch (Exception ex)
            {

            }
        }
        private async void SendActivityAsync(UserDto user)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    try
                    {
                        _proxy.ProxyService.SendUserActivity(user);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Write($"Error: {ex.Message} | InnerException {ex?.InnerException.Message}");
                    }
                });
            }
            catch (Exception)
            {
                
            }
        }
        private ActionDto CreateAction(UserDto user, string action, string details)
        {
            return new ActionDto
            {
                TimeStamp = DateTime.Now.ToString(),
                User = user,
                Action = action,
                Details = details
            };
        }
    }
}
