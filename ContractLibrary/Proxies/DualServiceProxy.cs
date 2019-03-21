using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ContractLibrary.Proxies
{
    public abstract class DualServiceProxy<TServiceContract> : ServiceProxy<TServiceContract, DuplexChannelFactory<TServiceContract>> 
        where TServiceContract : class
    {
        private InstanceContext _instanceContext;

        public DualServiceProxy(string endpointConfigurationName, object context) : base(endpointConfigurationName)
        {
            _instanceContext = new InstanceContext(context);

            _channel = CreateChannel(_instanceContext, _endpointConfigurationName);
            ProxyService = CreateProxy(_channel);
        }

        public DualServiceProxy(Uri address, Binding binding, object context) : base(address, binding)
        {
            _instanceContext = new InstanceContext(context);

            _channel = CreateChannel(_instanceContext, _binding, _endpoint);
            ProxyService = CreateProxy(_channel);
        }
        protected override void ServiceProxyBase_Faulted(object sender, EventArgs e)
        {
            try
            {
                lock (_sync)
                {
                    CloseChannel();

                    if (_isConfCreated)
                    {
                        _channel = CreateChannel(_instanceContext, _endpointConfigurationName);
                    }
                    else
                    {
                        _channel = CreateChannel(_instanceContext, _binding, _address.ToString());
                    }

                    ProxyService = CreateProxy(_channel);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
