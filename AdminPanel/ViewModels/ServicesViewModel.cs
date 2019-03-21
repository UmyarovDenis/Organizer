using AdminPanel.Services.Net;
using AdminPanel.ViewModels.Core;
using ContractLibrary.CommunicationNode;
using ContractLibrary.DataTransfer.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Threading;
using System.Threading.Tasks;
using AdminPanel.Extensions;
using System.Windows.Controls;
using AdminPanel.Services.Local;

namespace AdminPanel.ViewModels
{
    internal sealed class ServicesViewModel : BaseViewModel, ICommunicationCallback
    {
        private CommunicationServiceProxy _proxy;
        private ObservableCollection<ServiceDescriptionDto> _services;

        private Dictionary<ServiceDescriptionDto, CancellationTokenSource> _tokens;

        private ICommand _switchServiceStateCommand;

        public ServicesViewModel(IMessageService<bool> messageService) : base(messageService)
        {
            try
            {
                _tokens = new Dictionary<ServiceDescriptionDto, CancellationTokenSource>();
                _proxy = new CommunicationServiceProxy(this);
                _proxy.ExecuteAsync(p => p.GetServicesInfo());
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        public ObservableCollection<ServiceDescriptionDto> Services
        {
            get { return _services; }
            set
            {
                _services = value;
                RaisePropertyChanged();
            }
        }

        #region Commands

        public ICommand SwitchServiceStateCommand
        {
            get
            {
                return _switchServiceStateCommand ??
                    (_switchServiceStateCommand = new RelayCommand(arg => SwitchServiceState(arg)));
            }
        }

        #endregion

        #region CommandImplementations

        
        private void SwitchServiceState(object checkBox)
        {
            try
            {
                CheckBox control = checkBox as CheckBox;

                if (control != null)
                {
                    if (control.IsChecked == true)
                    {
                        StartService(control.Tag as ServiceDescriptionDto);
                    }
                    else if (control.IsChecked == false)
                    {
                        StopService(control.Tag as ServiceDescriptionDto);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }

        #endregion

        public void SendDescriptionList(List<ServiceDescriptionDto> serviceInfo)
        {
            try
            {
                Services = new ObservableCollection<ServiceDescriptionDto>(serviceInfo);
                SetTimers(Services);
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        public void SendServiceState(ServiceDescriptionDto service)
        {
            try
            {
                var srv = Services.FirstOrDefault(s => s.ServiceType == service.ServiceType);

                if (srv != null)
                {
                    if(service.IsStarted == false && _tokens.ContainsKey(srv))
                    {
                        _tokens[srv].Cancel();
                        _tokens.Remove(srv);
                    }
                    else if (service.IsStarted == true)
                    {
                        StartNewTimer(service);
                    }

                    Services.ReplaceItem(oldItem: srv, newItem: service);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private void SetTimers(IEnumerable<ServiceDescriptionDto> services)
        {
            foreach(ServiceDescriptionDto service in services)
            {
                StartNewTimer(service);
            }
        }
        private void StartNewTimer(ServiceDescriptionDto service)
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            _tokens.Add(service, cancellationToken);

            service.WorkingHours = DateTime.Now.Subtract(service.StartDate);

            Task.Factory.StartNew(() =>
            {
                while (cancellationToken.Token.IsCancellationRequested == false)
                {
                    service.WorkingHours += new TimeSpan(0, 0, 1);
                    Task.Delay(1000).Wait();
                }
            }, cancellationToken.Token);
        }
        private void StartService(ServiceDescriptionDto service)
        {
            if (service != null)
            {
                try
                {
                    _proxy.ProxyService.StartService(service);
                }
                catch (Exception ex)
                {
                    MessageService.ShowError(ex.Message);
                }
            }
        }
        private void StopService(ServiceDescriptionDto service)
        {
            if (service != null)
            {
                try
                {
                    _proxy.ProxyService.StopService(service);
                }
                catch (Exception ex)
                {
                    MessageService.ShowError(ex.Message);
                }
            }
        }
    }
}
