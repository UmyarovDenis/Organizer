using ContractLibrary.DataTransfer.Dto;
using Organizer.Enums;
using Organizer.Services.Local;
using Organizer.Services.Net;
using Organizer.ViewModels.Core;
using Organizer.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Organizer.ViewModels
{
    internal sealed class CityViewModel : ContactViewModel
    {
        private CityDto _selectedCity;

        private ICommand _addNewOrganizatiobCommand;
        private ICommand _changeOrganizatiobCommand;
        private ICommand _removeOrganizatiobCommand;
        private ICommand _sendOrganizationMailCommand;

        private ObservableCollection<OrganizationDto> _organizations;

        public CityViewModel(IMessageService<bool> messageService, IDataServiceProxy dataServiceProxy,
            IMailService mailService, IImageManager<ImageType> imageManager) 
            : base(messageService, dataServiceProxy, mailService, imageManager)
        { }

        public void UpdateCityInfo(CityDto city)
        {
            SelectedCity = city;
            Organizations = new ObservableCollection<OrganizationDto>(SelectedCity.Organizations);
            Contacts = null;
        }

        #region BindingProperties

        public CityDto SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                _selectedCity = value;
                SelectedContact = null;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<OrganizationDto> Organizations
        {
            get { return _organizations; }
            set
            {
                _organizations = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand AddNewOrganizationCommand
        {
            get
            {
                return _addNewOrganizatiobCommand ??
                    (_addNewOrganizatiobCommand = new RelayCommand(arg => AddNewOrganization()));
            }
        }
        public ICommand ChangeOrganizationCommand
        {
            get
            {
                return _changeOrganizatiobCommand ??
                    (_changeOrganizatiobCommand = new RelayCommand(arg => ChangeOrganization(arg as OrganizationDto), arg => CanChangeOrRemoveOrganization()));
            }
        }
        public ICommand RemoveOrganizationCommand
        {
            get
            {
                return _removeOrganizatiobCommand ??
                    (_removeOrganizatiobCommand = new RelayCommand(arg => RemoveOrganization(arg as OrganizationDto), arg => CanChangeOrRemoveOrganization()));
            }
        }
        public ICommand SendOrganizationMailCommand
        {
            get
            {
                return _sendOrganizationMailCommand ??
                    (_sendOrganizationMailCommand = new RelayCommand(arg => SendMail(arg.ToString()), arg => CanSendMailOrganization()));
            }
        }

        #endregion

        #region Implemented Organization Commands

        private void AddNewOrganization()
        {
            try
            {
                OrganizationDto newOrganization = _controller.GetConfigs<OrganizationCardView, OrganizationCardViewModel>()
                    .View(v => v.Icon = _imageManager.GetImage(ImageType.OrganizationImage))
                    .AdditionalViewModelParams(SelectedCity)
                    .RunDialogWithResult(vm => vm.Organization);

                if(newOrganization != null)
                {
                    newOrganization.City_Id = SelectedCity.Id;

                    var result = _dataProxy.CreateItems(newOrganization);

                    if (result?.Count > 0)
                    {
                        newOrganization = result.First();
                        newOrganization.City = SelectedCity;
                        SelectedCity.Organizations.Add(newOrganization);
                        Organizations.Add(newOrganization);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private void ChangeOrganization(OrganizationDto organization)
        {
            try
            {
                bool? result = _controller.GetConfigs<OrganizationView, OrganizationViewModel>()
                    .View(v => v.Icon = _imageManager.GetImage(ImageType.OrganizationImage))
                    .AdditionalViewModelParams(SelectedOrganization)
                    .RunDialog();

                if(result == true)
                {
                    _dataProxy.UpdateItems(organization);
                }
            }
            catch(Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private void RemoveOrganization(OrganizationDto organization)
        {
            try
            {
                bool? dlgResult = MessageService.ShowQuestion("Удалить запись?");

                if(dlgResult == true)
                {
                    bool execResult = _dataProxy.RemoveItems(organization);

                    if (execResult)
                    {
                        SelectedCity.Organizations.Remove(organization);
                        Organizations.Remove(organization);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private bool CanChangeOrRemoveOrganization()
        {
            return SelectedOrganization != null;
        }
        private bool CanSendMailOrganization()
        {
            if (SelectedOrganization == null)
                return false;

            return _mailService.IsValidMail(SelectedOrganization.Email);
        }

        #endregion
    }
}
