using Organizer.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Organizer.Views;
using Organizer.Settings;
using Organizer.Views.Pages;
using ContractLibrary.DataTransfer.Dto;
using Organizer.Services.FileServises;
using Organizer.Services.Net;
using Organizer.Services.Local;
using System.ServiceModel;
using ContractLibrary.Exceptions;
using AppController;
using AppController.Infrastructure.Attributes;
using Organizer.Infrastructure.Modules;
using System.Windows;

namespace Organizer.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private ISettingService _settingService;
        private IDataServiceProxy _dataServiceProxy;
        private IAuthServiceProxy _authProxy;

        [Injected(typeof(ViewModule))]
        private IControllerCore _controller;

        private string _currentUserName;
        private UserDto _currentUser;

        private OrganizationDto _selectedOrganization;
        private CityDto _selectedCity;

        private ICommand _itemChangesCommand;
        private ICommand _organizationSelectCommand;
        private ICommand _openMailServiceCommand;
        private ICommand _openSqlWindowExecCommand;
        private ICommand _enterSystemCommand;
        private ICommand _closingMainWindowCommand;

        private ObservableCollection<RegionDto> _regions;

        private Dictionary<Type, object> _pages;
        private object _currentPage;

        public MainViewModel(IMessageService<bool> messageService, IDataServiceProxy dataServiceProxy
            , IAuthServiceProxy authServiceProxy, ISettingService settingService) : base(messageService)
        {
            _dataServiceProxy = dataServiceProxy;
            _authProxy = authServiceProxy;
            _settingService = settingService;
        }

        #region Binding Properties

        public string CurrentUserName
        {
            get { return _currentUserName; }
            set
            {
                _currentUserName = value;
                RaisePropertyChanged();
            }
        }
        public object CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<RegionDto> Regions
        {
            get { return _regions; }
            set
            {
                _regions = value;
                RaisePropertyChanged();
            }
        }
        public CityDto SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                _selectedCity = value;
                RaisePropertyChanged();
            }
        }
        public OrganizationDto SelectedOrganization
        {
            get { return _selectedOrganization; }
            set
            {
                _selectedOrganization = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand EnterSystemCommand
        {
            get
            {
                return _enterSystemCommand ??
                    (_enterSystemCommand = new RelayCommand(arg => EnterSystem(), arg => CanEnterSystem()));
            }
        }
        public ICommand ItemChangesCommand
        {
            get
            {
                return _itemChangesCommand ??
                    (_itemChangesCommand = new RelayCommand(arg => ItemChanges(arg)));
            }
        }
        public ICommand OrganizationSelectCommand
        {
            get
            {
                return _organizationSelectCommand ??
                    (_organizationSelectCommand = new RelayCommand(arg => SelectOrganization(arg as OrganizationDto)));
            }
        }
        public ICommand OpenMailServiceCommand
        {
            get
            {
                return _openMailServiceCommand ??
                    (_openMailServiceCommand = new RelayCommand(arg => OpenMailService()));
            }
        }
        public ICommand OpenSqlWindowExecCommand
        {
            get
            {
                return _openSqlWindowExecCommand ??
                    (_openSqlWindowExecCommand = new RelayCommand(arg => OpenSqlWindowExec(arg), arg => CanOpenSqlCommandWindow()));
            }
        }
        public ICommand ClothingMainWindowCommand
        {
            get
            {
                return _closingMainWindowCommand ??
                    (_closingMainWindowCommand = new RelayCommand(arg => ClothingMainWindow()));
            }
        }

        #endregion

        private async void EnterSystem()
        {
            try
            {
                UserDto currentUser = _controller.ResolveView<AuthorizationView, AuthorizationViewModel>()
                           .RunDialogWithResult(vm => vm.User);

                if (currentUser != null)
                {
                    _controller.Container.Bind<UserDto>().ToInstance(currentUser);
                    _currentUser = currentUser;
                    CurrentUserName = currentUser.FIO;

                    await System.Threading.Tasks.Task.Run(() =>
                    {
                        Regions = new ObservableCollection<RegionDto>(_dataServiceProxy.LoadData<RegionDto>());
                    });

                    InitPages();
                }
            }
            catch(FaultException<AuthorizationFault> ex)
            {
                MessageService.ShowError(ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                MessageService.ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException.Message ?? ex.Message;
                MessageService.ShowError(errorMessage);
            }
        }
        private void InitPages()
        {
            _pages = new Dictionary<Type, object>();

            object projectPage = _controller.ViewFactory
                .GetView<ProjectView, ProjectViewModel>(vm => OnUpdateOrganization += vm.UpdateOrganizationInfo);

            object cityPage = _controller.ViewFactory
                .GetView<CityView, CityViewModel>(vm => OnUpdateCity += vm.UpdateCityInfo);

            _pages.Add(typeof(OrganizationDto), projectPage);
            _pages.Add(typeof(CityDto), cityPage);
        }
        private void ItemChanges(object arg)
        {
            Type keyType = arg.GetType();

            if (_pages.ContainsKey(keyType))
            {
                var page = _pages[arg.GetType()];
                if(keyType.IsEquivalentTo(typeof(OrganizationDto)))
                {
                    OnUpdateOrganization?.Invoke(SelectedOrganization);
                }
                else if (keyType.IsEquivalentTo(typeof(CityDto)))
                {
                    OnUpdateCity?.Invoke(SelectedCity);
                }

                CurrentPage = page;
            }
        }
        private void SelectOrganization(OrganizationDto organization)
        {
            try
            {
                if (organization != null)
                {
                    _controller.GetConfigs<OrganizationCardView, OrganizationCardViewModel>()
                        .AdditionalViewModelParams(organization)
                        .RunDialog();
                }
            }
            catch(Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private void OpenMailService()
        {
            try
            {
                MailSettings settings = _controller.ResolveView<MailServiceSettingsView, MailServiceSettingsViewModel>()
                    .RunDialogWithResult(vm => vm.MailSettings);

                if(settings != null)
                {
                    _settingService.SaveSettings(settings, _settingService.MailSettingsFileName);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private void OpenSqlWindowExec(object owner)
        {
            try
            {
                if (owner == null)
                    throw new NullReferenceException();

                _controller.GetConfigs<SqlCommandView, SqlCommandViewModel>()
                    .View(v => v.Owner = owner as Window)
                    .Run();

            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private bool CanOpenSqlCommandWindow()
        {
            return _currentUser != null;
        }
        private bool CanEnterSystem()
        {
            return _currentUser == null;
        }
        private void ClothingMainWindow()
        {
            _authProxy.Disconnect(_currentUser);
        }

        private event Action<OrganizationDto> OnUpdateOrganization;
        private event Action<CityDto> OnUpdateCity;
    }
}
