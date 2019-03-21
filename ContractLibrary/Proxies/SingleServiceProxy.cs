using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ContractLibrary.Proxies
{
    public abstract class SingleServiceProxy<TServiceContract> : ServiceProxy<TServiceContract, ChannelFactory<TServiceContract>>
        where TServiceContract : class
    {
        public SingleServiceProxy(string endpointConfigurationName) 
            : base(endpointConfigurationName)
        {
            _channel = CreateChannel(_endpointConfigurationName);
            ProxyService = CreateProxy(_channel);
        }

        public SingleServiceProxy(Uri address, Binding binding) 
            : base(address, binding)
        {
            _channel = CreateChannel(_binding, _endpoint);
            ProxyService = CreateProxy(_channel);
        }
    }
}
