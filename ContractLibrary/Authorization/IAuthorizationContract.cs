using ContractLibrary.ContractAttributes;
using ContractLibrary.DataTransfer.Dto;
using ContractLibrary.Exceptions;
using System.Collections.Generic;
using System.ServiceModel;

namespace ContractLibrary.Authorization
{
    [ServiceContract]
    [ContractDescription(ContractName = "Сервис аутентификации")]
    public interface IAuthorizationContract
    {
        [OperationContract]
        [FaultContract(typeof(AuthorizationFault))]
        UserDto Authorizate(AuthorizationRequest authorizationRequest);

        [OperationContract]
        List<UserDto> GetUserList();

        [OperationContract]
        void RestorePassword(string email);

        [OperationContract(IsOneWay = true)]
        void Disconnect(UserDto user);
    }
}
