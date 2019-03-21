using AdminPanel.Enums;
using AdminPanel.Infrastructure.Modules;
using AdminPanel.Services.Local;
using AdminPanel.Services.Net;
using AdminPanel.ViewModels.Core;
using AdminPanel.Views.Pages;
using AppController;
using AppController.Infrastructure.Attributes;
using ContractLibrary.Notifyer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows.Input;

namespace AdminPanel.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private IControllerCore _controller;
        private ServiceHost _host;

        private ICommand _windowLoadedCommand;
        private ICommand _startServerCommand;
        private ICommand _stopServerCommand;
        private ICommand _restartServerCommand;

        private ICommand _switchPageCommand;

        private List<object> _pages;
        private object _selectedPage;

        private ServerStatus _serverStatus;

        public MainViewModel(IMessageService<bool> messageService, IControllerCore controller) : base(messageService)
        {
            try
            {
                _pages = new List<object>();
                _controller = controller;

                InitPages();
                SwitchPage(0);
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        ~MainViewModel()
        {
            if(_host != null)
            {
                _host.Close();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        public List<object> Pages
        {
            get
            {
                return _pages;
            }
        }
        public object SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                _selectedPage = value;
                RaisePropertyChanged();
            }
        }
        public ServerStatus ServerStatus
        {
            get { return _serverStatus; }
            set
            {
                _serverStatus = value;
                RaisePropertyChanged();
            }
        }

        #region Commands

        public ICommand WindowLoadedCommand
        {
            get
            {
                return _windowLoadedCommand ??
                    (_windowLoadedCommand = new RelayCommand(arg => WindowOnLoad()));
            }
        }
        public ICommand StartServerCommand
        {
            get
            {
                return _startServerCommand ??
                    (_startServerCommand = new RelayCommand(arg => ServiceStart()));
            }
        }
        public ICommand StopServerCommand
        {
            get
            {
                return _stopServerCommand ??
                    (_stopServerCommand = new RelayCommand(arg => StopService()));
            }
        }
        public ICommand RestartServerCommand
        {
            get
            {
                return _restartServerCommand ??
                    (_restartServerCommand = new RelayCommand(arg => RestartServer()));
            }
        }
        public ICommand SwitchPageCommand
        {
            get
            {
                return _switchPageCommand ??
                    (_switchPageCommand = new RelayCommand(arg => SwitchPage(Convert.ToInt32(arg))));
            }
        }

        #endregion

        #region CommandImplementations

        private void WindowOnLoad()
        {
            try
            {
                Uri _address = new Uri("net.tcp://localhost:7030/INotifyNode");
                NetTcpBinding _netTcpBinding = new NetTcpBinding();
                Type _contractType = typeof(INotifyNode);

                EndpointAddress endpoint = new EndpointAddress(_address);

                _host = new ServiceHost(NotifyNodeService.CreateInstance());
                _host.AddServiceEndpoint(_contractType, _netTcpBinding, _address);
                _host.Open();
            }
            catch (Exception ex)
            {

            }
        }
        private void SwitchPage(int index)
        {
            try
            {
                SelectedPage = _pages.ElementAt(index);
            }
            catch (Exception)
            {

            }
        }

        #endregion


        private void InitPages()
        {
            Pages.Add(_controller.ViewFactory.GetView<MonitorView, MonitorViewModel>(vm => 
            NotifyNodeService.Configuration(n => n.ActionList = vm.OperationList)));

            Pages.Add(_controller.ViewFactory.GetView<ServicesView>());

            Pages.Add(_controller.ViewFactory.GetView<AccountsView, AccountsViewModel>(vm => 
            NotifyNodeService.Configuration(n => n.UserList = vm.Users)));

            Pages.Add(_controller.ViewFactory.GetView<SettingsView>());
        }
        private void ServiceStart()
        {
            try
            {
                ServerStatus = ServerStatus.Active;
                SelectedPage = _pages.ElementAt(0);
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private void StopService()
        {
            try
            {
                if(ServerStatus != ServerStatus.Idle)
                {
                    ServerStatus = ServerStatus.Stoped;
                    SelectedPage = _pages.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private void RestartServer()
        {
            try
            {
                ServerStatus = ServerStatus.Idle;
                SelectedPage = _pages.ElementAt(0);
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
    }
}
