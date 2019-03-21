using ContractLibrary.DataTransfer.Dto;
using System.Collections.Generic;
using System.ServiceModel;

namespace ContractLibrary.CommunicationNode
{
    [ServiceContract(CallbackContract = typeof(ICommunicationCallback), SessionMode = SessionMode.Required)]
    [ServiceKnownType(typeof(ServiceDescriptionDto))]
    [ServiceKnownType(typeof(List<ServiceDescriptionDto>))]
    public interface ICommunicationContract
    {
        [OperationContract(IsOneWay = true)]
        void GetServicesInfo();

        [OperationContract(IsOneWay = true)]
        void StartService(ServiceDescriptionDto service);

        [OperationContract(IsOneWay = true)]
        void StopService(ServiceDescriptionDto service);
    }
}
