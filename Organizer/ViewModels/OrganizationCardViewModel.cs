using ContractLibrary.DataTransfer.Dto;
using Organizer.Services.Local;
using Organizer.Services.Net;
using Organizer.ViewModels.Core;

namespace Organizer.ViewModels
{
    internal class OrganizationCardViewModel : BaseViewModel
    {
        private IDataServiceProxy _dataProxy;

        private RegionDto _selectedRegion;
        private CityDto _selectedCity;
        private OrganizationDto _selectedOrganization;


        private string _viewTitle;
        private string _buttonName;

        public OrganizationCardViewModel(IMessageService<bool> messageService, IDataServiceProxy dataServiceProxy) 
            : base(messageService)
        {
            _dataProxy = dataServiceProxy;
        }
        public OrganizationCardViewModel(IMessageService<bool> messageService, IDataServiceProxy dataServiceProxy,
            CityDto city) : this(messageService, dataServiceProxy)
        {
            Organization = new OrganizationDto();
            Organization.City_Id = city.Id;
            
            Region = city.Region;
            City = city;

            ViewTitle = "Новая запись";
            ButtonName = "Сохранить";
        }
        public OrganizationCardViewModel(IMessageService<bool> messageService, IDataServiceProxy dataServiceProxy,
            OrganizationDto organization) : this(messageService, dataServiceProxy)
        {
            Region = organization.City.Region;
            City = organization.City;
            Organization = organization;

            ViewTitle = organization.Name;
            ButtonName = "Изменить";
        }
        public OrganizationDto Organization
        {
            get { return _selectedOrganization; }
            set
            {
                _selectedOrganization = value;
                RaisePropertyChanged();
            }
        }
        public RegionDto Region
        {
            get { return _selectedRegion; }
            set
            {
                _selectedRegion = value;
                RaisePropertyChanged();
            }
        }
        public CityDto City
        {
            get { return _selectedCity; }
            set
            {
                _selectedCity = value;
                RaisePropertyChanged();
            }
        }
        public string ViewTitle
        {
            get { return _viewTitle; }
            set
            {
                _viewTitle = value;
                RaisePropertyChanged();
            }
        }
        public string ButtonName
        {
            get { return _buttonName; }
            set
            {
                _buttonName = value;
                RaisePropertyChanged();
            }
        }
    }
}
