using ContractLibrary.Authorization;
using ContractLibrary.CommunicationNode;
using ContractLibrary.DataTransfer;
using ContractLibrary.DataTransfer.Dto;
using DataTransferService.AuthorizationNode;
using DataTransferService.DataAccessLayer;
using DataTransferService.SqlData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Reflection;
using ContractLibrary.ContractAttributes;
using System.ServiceModel.Description;

namespace OrganizationHost
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    internal sealed class Server : ICommunicationContract
    {
        private static Server _instance;

        private readonly Dictionary<ServiceDescriptionDto, ServiceHost> _services;
        private readonly Dictionary<string, Type> _serviceImplementations;

        ServiceHost _server;

        Uri _address;
        NetTcpBinding _netTcpBinding;
        ContractDescription _contractDescription;
        EndpointAddress _endpointAddress;

        ServiceEndpoint _endpoint;
        Type _contract;

        private Server()
        {
            try
            {
                _services = new Dictionary<ServiceDescriptionDto, ServiceHost>();
                _serviceImplementations = new Dictionary<string, Type>();

                _address = new Uri($"net.tcp://localhost:8660/{nameof(ICommunicationContract)}");
                _netTcpBinding = new NetTcpBinding();
                _contract = typeof(ICommunicationContract);

                _contractDescription = new ContractDescription(_contract.Name);
                _contractDescription.CallbackContractType = typeof(ICommunicationCallback);

                _endpointAddress = new EndpointAddress(_address);
                _endpoint = new ServiceEndpoint(ContractDescription.GetContract(_contract), _netTcpBinding, _endpointAddress);

                _server = new ServiceHost(this);
                _server.AddServiceEndpoint(_endpoint);
                _server.Open();
            }
            catch (Exception ex)
            {

            }
        }
        ~Server()
        {
            if (_server != null)
            {
                _server.Close();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        public static Server GetInstance()
        {
            if (_instance == null)
                _instance = new Server();

            return _instance;
        }
        private ICommunicationCallback Callback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<ICommunicationCallback>();
            }
        }
        public void Start()
        {
            try
            {
                OpenHost<IDataAccessContract, DataAccessService>();
                OpenHost<ISqlProviderContract, SqlDataService>();
                OpenHost<IAuthorizationContract, AuthorizationService>();

                ServerStart?.Invoke();
            }
            catch (Exception ex)
            {
                
            }
        }
        public void OpenHost<TContract, TImplementation>() where TContract : class where TImplementation : TContract
        {
            ServiceHost host = new ServiceHost(typeof(TImplementation));

            host.Opened += HostStateHandler;
            host.Closed += HostStateHandler;
            host.Faulted += HostStateHandler;

            host.Open();

            _services.Add(CreateDescription(host), host);
            _serviceImplementations.Add(typeof(TContract).Name, typeof(TImplementation));

            HostStart?.Invoke(typeof(TContract).Name, host);
        }
        public void Stop()
        {
            foreach (var keyValuePair in _services)
            {
                keyValuePair.Value.Close();

                keyValuePair.Value.Opened -= HostStateHandler;
                keyValuePair.Value.Closed -= HostStateHandler;
                keyValuePair.Value.Faulted -= HostStateHandler;
            }

            _services.Clear();

            ServerStop?.Invoke();
        }
        private ServiceDescriptionDto CreateDescription(ServiceHost serviceHost)
        {
            Type contractType = serviceHost.Description.Endpoints[0].Contract.ContractType;
            string serviceName = string.Empty;

            if (Attribute.IsDefined(contractType, typeof(ContractDescriptionAttribute)))
            {
                serviceName = contractType.GetCustomAttribute<ContractDescriptionAttribute>().ContractName;
            }

            return new ServiceDescriptionDto
            {
                ServiceName = !string.IsNullOrEmpty(serviceName) ? serviceName : serviceHost.Description.Name,
                Address = serviceHost.Description.Endpoints[0].Address.ToString(),
                Binding = serviceHost.Description.Endpoints[0].Binding.Name,
                ServiceType = serviceHost.Description.Endpoints[0].Contract.Name,
                OpenTimeout = serviceHost.Description.Endpoints[0].Binding.OpenTimeout.ToString(),
                CloseTimeout = serviceHost.Description.Endpoints[0].Binding.CloseTimeout.ToString(),
                SendTimeout = serviceHost.Description.Endpoints[0].Binding.SendTimeout.ToString(),
                ReceiveTimeout = serviceHost.Description.Endpoints[0].Binding.ReceiveTimeout.ToString(),
                StartDate = DateTime.Now,
                State = serviceHost.State.ToString(),
                IsStarted = true
            };
        }

        #region Handlers

        private void HostStateHandler(object sender, EventArgs e)
        {
            ServiceHost host = sender as ServiceHost;

            if (host != null)
            {
                try
                {

                }
                catch (Exception ex)
                {

                }
            }
        }

        #endregion

        #region CommunicationNode

        void ICommunicationContract.GetServicesInfo()
        {
            try
            {
                Callback.SendDescriptionList(_services.Keys.ToList());
            }
            catch (Exception ex)
            {
                
            }
        }
        void ICommunicationContract.StartService(ServiceDescriptionDto service)
        {
            if (service != null)
            {
                try
                {
                    ServiceHost newHost = null;
                    ServiceDescriptionDto serviceDescription = null;
                    ServiceDescriptionDto oldDescription = null;

                    for (int i = 0; i < _services.Values.Count; i++)
                    {
                        if (_services.Values.ElementAt(i).Description.Endpoints[0].Contract.Name == service.ServiceType
                            && _services.Values.ElementAt(i).State == CommunicationState.Closed)
                        {
                            newHost = new ServiceHost(_serviceImplementations[service.ServiceType]);
                            newHost.Open();

                            newHost.Opened += HostStateHandler;
                            newHost.Closed += HostStateHandler;
                            newHost.Faulted += HostStateHandler;

                            serviceDescription = CreateDescription(newHost);
                            oldDescription = _services.Keys.ElementAt(i);

                            break;
                        }
                    }

                    _services.Remove(oldDescription);
                    _services.Add(serviceDescription, newHost);

                    Callback.SendServiceState(serviceDescription);

                    HostStart?.Invoke(serviceDescription.ServiceType, newHost);
                }
                catch (Exception ex)
                {
                    
                }
            }
        }
        void ICommunicationContract.StopService(ServiceDescriptionDto service)
        {
            if (service != null)
            {
                try
                {
                    ServiceHost closedHost = null;
                    foreach (var host in _services.Values)
                    {
                        if (host.Description.Endpoints[0].Contract.Name == service.ServiceType
                            && host.State == CommunicationState.Opened)
                        {
                            closedHost = host;
                            host.Close();

                            service.StopDate = DateTime.Now;
                            service.IsStarted = false;
                            service.State = host.State.ToString();

                            Callback.SendServiceState(service);

                            break;
                        }
                    }

                    closedHost.Opened -= HostStateHandler;
                    closedHost.Closed -= HostStateHandler;
                    closedHost.Faulted -= HostStateHandler;

                    HostStop?.Invoke(closedHost, service.StartDate);
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        #endregion

        public event Action ServerStart;
        public event Action<string, ServiceHost> HostStart;
        public event Action<ServiceHost, DateTime> HostStop;
        public event Action ServerStop;
    }
}
