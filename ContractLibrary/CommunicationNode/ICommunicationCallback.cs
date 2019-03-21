using ContractLibrary.DataTransfer.Dto;
using System.Collections.Generic;
using System.ServiceModel;

namespace ContractLibrary.CommunicationNode
{
    public interface ICommunicationCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendDescriptionList(List<ServiceDescriptionDto> serviceInfo);

        [OperationContract(IsOneWay = true)]
        void SendServiceState(ServiceDescriptionDto service);
    }
}
