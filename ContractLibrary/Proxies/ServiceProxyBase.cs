using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ContractLibrary.Proxies
{
    public abstract class ServiceProxyBase<TServiceContract, TChannel> : IDisposable where TServiceContract : class
        where TChannel : ChannelFactory<TServiceContract>
    {
        protected TServiceContract _proxyService;
        protected TChannel _channel;

        protected object _sync = new object();

        protected Uri _address;
        protected Binding _binding;
        protected EndpointAddress _endpoint;

        protected string _endpointConfigurationName;

        protected bool _isConfCreated = true;
        private bool _disposed = false;

        public ServiceProxyBase(Uri address, Binding binding)
        {
            _address = address;
            _binding = binding;
            _endpoint = new EndpointAddress(address);

            _isConfCreated = false;
        }
        public ServiceProxyBase(string endpointConfigurationName)
        {
            _endpointConfigurationName = endpointConfigurationName;
        }
        ~ServiceProxyBase()
        {
            Dispose(false);
        }
        public TServiceContract ProxyService
        {
            get { return _proxyService; }
            protected set { _proxyService = value; }
        }
        protected virtual void ServiceProxyBase_Faulted(object sender, EventArgs e)
        {
            try
            {
                lock (_sync)
                {
                    CloseChannel();

                    if (_isConfCreated)
                    {
                        _channel = CreateChannel(_endpointConfigurationName);
                    }
                    else
                    {
                        _channel = CreateChannel(_binding, _endpoint);
                    }

                    ProxyService = CreateProxy(_channel);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected TChannel CreateChannel(params object[] args)
        {
            return Activator.CreateInstance(typeof(TChannel), args) as TChannel;
        }
        protected TServiceContract CreateProxy(TChannel channelFactory)
        {
            TServiceContract proxy = channelFactory.CreateChannel();
            (proxy as IClientChannel).Faulted += ServiceProxyBase_Faulted;

            return proxy;
        }
        protected void CloseChannel()
        {
            if(_channel != null && ProxyService != null)
            {
                (ProxyService as IClientChannel).Faulted -= ServiceProxyBase_Faulted;

                try
                {
                    _channel.Close();
                }
                catch (FaultException)
                {
                    _channel.Abort();
                }
                catch (CommunicationException)
                {
                    _channel.Abort();
                }
                catch (TimeoutException)
                {
                    _channel.Abort();
                }
                catch (Exception)
                {
                    _channel.Abort();
                }
            }
        }

        #region Dispose Implementation

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposed)
        {
            if (!_disposed)
            {
                lock (_sync)
                {
                    if (disposed)
                    {
                        _address = null;
                        _binding = null;
                        _endpoint = null;
                    }

                    CloseChannel();

                    if (_channel != null)
                    {
                        (_channel as IDisposable).Dispose();
                    }

                    _proxyService = null;
                    _channel = null;

                    _disposed = true;
                }
            }
        }

        #endregion
    }
}
