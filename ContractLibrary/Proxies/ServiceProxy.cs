using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace ContractLibrary.Proxies
{
    public abstract class ServiceProxy<TServiceContract, TChannel> : ServiceProxyBase<TServiceContract, TChannel>,
        IProxyExecutor<TServiceContract> where TChannel : ChannelFactory<TServiceContract> where TServiceContract : class
    {
        public ServiceProxy(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        public ServiceProxy(Uri address, Binding binding)
            : base(address, binding)
        {
        }

        public void Execute(Action<TServiceContract> action)
        {
            action.Invoke(ProxyService);
        }
        public Task ExecuteAsync(Action<TServiceContract> action)
        {
            return Task.Run(() =>
            {
                action.Invoke(ProxyService);
            });
        }
        public TResult GetServiceResult<TResult>(Func<TServiceContract, TResult> executor)
        {
            return executor.Invoke(ProxyService);
        }
        public Task<TResult> GetServiceResultAsync<TResult>(Func<TServiceContract, TResult> executor)
        {
            return Task.Run(() => executor.Invoke(ProxyService));
        }
    }
}
