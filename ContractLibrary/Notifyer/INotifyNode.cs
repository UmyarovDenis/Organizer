using ContractLibrary.DataTransfer.Dto;
using System.ServiceModel;

namespace ContractLibrary.Notifyer
{
    [ServiceContract]
    [ServiceKnownType(typeof(ActionDto))]
    [ServiceKnownType(typeof(UserDto))]
    public interface INotifyNode
    {
        [OperationContract(IsOneWay = true)]
        void Send(ActionDto userAction);

        [OperationContract(IsOneWay = true)]
        void SendUserActivity(UserDto user);
    }
}
