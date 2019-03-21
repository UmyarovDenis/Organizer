using System;
using System.Net;
using System.ServiceModel;
using ContractLibrary.Authorization;
using ContractLibrary.DataTransfer.Dto;
using ContractLibrary.Exceptions;
using ContractLibrary.Proxies;

namespace Organizer.Services.Net
{
    internal sealed class AuthServiceProxy : SingleServiceProxy<IAuthorizationContract>, IAuthServiceProxy
    {
        public AuthServiceProxy() : base("AuthServiceHttp")
        { }

        public UserDto Authorizate(string login, string password)
        {
            try
            {
                UserDto user = null;
                
                Execute((proxy) => 
                {
                    user = proxy.Authorizate(CreateRequest(login, password));
                });

                return user;
            }
            catch (FaultException<AuthorizationFault>)
            {
                throw;
            }
            catch (CommunicationException)
            {
                throw;
            }
            catch(ObjectDisposedException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Disconnect(UserDto user)
        {
            if(user != null)
            {
                ExecuteAsync(p => p.Disconnect(user));
            }
        }
        private AuthorizationRequest CreateRequest(string login, string password)
        {
            string macineName = Dns.GetHostName();
            string localIPAddress = Dns.GetHostByName(macineName).AddressList[0].ToString();

            return new AuthorizationRequest
            {
                Login = login,
                Password = password,
                Host = new HostDto
                {
                    MachineName = macineName,
                    LocalIPAddress = localIPAddress
                }
            };
        }
    }
}
