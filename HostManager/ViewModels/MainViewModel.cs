using HostManager.Models;
using HostManager.ViewModels.Core;
using Microsoft.Win32;
using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HostManager.ViewModels
{
    internal sealed class MainViewModel : BaseViewModel
    {
        private ServiceController _serviceController;
        private ServiceControllerStatus _serviceStatus;
        private OpenFileDialog _openFileDialog;
        private string _fileName;
        private string _filePath;

        private string _message;

        private bool _isInstalled = true;

        private ICommand _openFileDialogCommand;
        private ICommand _installServiceCommand;
        private ICommand _uninstallServiceCommand;
        private ICommand _startServiceCommand;
        private ICommand _stopServiceCommand;

        public MainViewModel()
        {
            _openFileDialog = new OpenFileDialog();

            var fileInfo = TryGetServicePath();
            FileName = fileInfo?.Name;
            FilePath = fileInfo?.FullName;

            try
            {
                _serviceController = new ServiceController("+++ OrganizerServer +++");
                ServiceStatus = _serviceController.Status;
                Message = new ServiceDescription(_serviceController).ToString();
            }
            catch (InvalidOperationException)
            {
                _isInstalled = false;
                Message = "Служба не установлена";
            }
            catch (Exception)
            { }
        }
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                RaisePropertyChanged();
            }
        }
        public ServiceControllerStatus ServiceStatus
        {
            get { return _serviceStatus; }
            private set
            {
                _serviceStatus = value;
                RaisePropertyChanged();
            }
        }
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged();
            }
        }
        private string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        #region Commands

        public ICommand OpenFileDialogCommand
        {
            get
            {
                return _openFileDialogCommand ??
                    (_openFileDialogCommand = new RelayCommand(arg => OpenFile()));
            }
        }
        public ICommand InstallServiceCommand
        {
            get
            {
                return _installServiceCommand ??
                    (_installServiceCommand = new RelayCommand(arg => InstallService(), arg => CanInstallService()));
            }
        }
        public ICommand UninstallServiceCommand
        {
            get
            {
                return _uninstallServiceCommand ??
                    (_uninstallServiceCommand = new RelayCommand(arg => UninstallService(), arg => CanUninstallService()));
            }
        }
        public ICommand StartServiceCommand
        {
            get
            {
                return _startServiceCommand ??
                    (_startServiceCommand = new RelayCommand(arg => StartService(), arg => CanStartService()));
            }
        }
        public ICommand StopServiceCommand
        {
            get
            {
                return _stopServiceCommand ??
                    (_stopServiceCommand = new RelayCommand(arg => StopService(), arg => CanStopService()));
            }
        }

        #endregion

        private void OpenFile()
        {
            try
            {
                if (_openFileDialog.ShowDialog() == true)
                {
                    FileName = _openFileDialog.SafeFileName;
                    _filePath = _openFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void InstallService()
        {
            try
            {
                await Task.Run(() => 
                {
                    ManagedInstallerClass.InstallHelper(new string[] { _filePath });
                    _serviceController = new ServiceController("+++ OrganizerServer +++");
                    _isInstalled = true;
                    ServiceStatus = _serviceController.Status;

                    Message = string.Format("Служба установлена\n\n{0}", 
                                                new ServiceDescription(_serviceController));
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void UninstallService()
        {
            try
            {
                await Task.Run(() => 
                {
                    ManagedInstallerClass.InstallHelper(new string[] { @"/u", _filePath });
                    _isInstalled = false;
                    _serviceController = null;
                    ServiceStatus = 0;
                    Message = "Удаление службы выполнено успешно";
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void StartService()
        {
            try
            {
                _serviceController.Start();
                ServiceStatus = ServiceControllerStatus.Running;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void StopService()
        {
            try
            {
                _serviceController.Stop();
                ServiceStatus = ServiceControllerStatus.Stopped;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private bool CanInstallService()
        {
            if (_isInstalled)
                return false;

            return !string.IsNullOrEmpty(FilePath);
        }
        private bool CanUninstallService()
        {
            return _isInstalled;
        }
        private bool CanStartService()
        {
            if (_isInstalled == false)
                return false;

            return ServiceStatus != ServiceControllerStatus.Running;
        }
        private bool CanStopService()
        {
            if (_isInstalled == false)
                return false;

            return ServiceStatus != ServiceControllerStatus.Stopped;
        }
        private FileInfo TryGetServicePath()
        {
            try
            {
                string execPath = AppDomain.CurrentDomain.BaseDirectory;
                FileInfo fileInfo = new FileInfo(Path.Combine(execPath, "OrganizationHost.exe"));

                return fileInfo;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
