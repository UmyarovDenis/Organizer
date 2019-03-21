using ContractLibrary.DataTransfer.Dto;

namespace Organizer.Services.Net
{
    internal interface IAuthServiceProxy
    {
        UserDto Authorizate(string login, string password);
        void Disconnect(UserDto user);
    }
}
