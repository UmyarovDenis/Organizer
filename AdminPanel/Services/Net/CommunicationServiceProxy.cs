using ContractLibrary.CommunicationNode;
using ContractLibrary.Proxies;
using System;
using System.ServiceModel;

namespace AdminPanel.Services.Net
{
    internal class CommunicationServiceProxy : DualServiceProxy<ICommunicationContract>
    {
        public CommunicationServiceProxy(string endpointConfigurationName, object context)
            : base(endpointConfigurationName, context)
        {
        }
        public CommunicationServiceProxy(object context) 
            : base(new Uri($"net.tcp://localhost:8660/{nameof(ICommunicationContract)}"), 
                  new NetTcpBinding(), context)
        {
        }
    }
}
