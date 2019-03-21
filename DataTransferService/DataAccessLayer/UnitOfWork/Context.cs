using DataTransferService.DataAccessLayer.Models;
using DataTransferService.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using DataTransferService.DataAccessLayer.Repositories.Interfaces;
using ContractLibrary.DataTransfer.Dto;

namespace DataTransferService.DataAccessLayer.UnitOfWork
{
    internal class Context
    {
        private readonly IRegionRepository _regionRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IOrganizationRepository _orgRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IMailRepository _mailRepository;
        private readonly IUserRepository _userRepository;

        private readonly Dictionary<string, Tuple<Type, IRepository>> _dataProcessors;

        public Context()
        {
            _regionRepository = new RegionRepository();
            _cityRepository = new CityRepository();
            _orgRepository = new OrganizationRepository();
            _taskRepository = new TaskRepository();
            _contactRepository = new ContactRepository();
            _mailRepository = new MailRepository();
            _userRepository = new UserRpository();

            _dataProcessors = new Dictionary<string, Tuple<Type, IRepository>>()
            {
                { nameof(RegionDto), new Tuple<Type, IRepository>(typeof(DataProcessor<Region, RegionDto>), _regionRepository) },
                { nameof(CityDto), new Tuple<Type, IRepository>(typeof(DataProcessor<City, CityDto>), _cityRepository) },
                { nameof(OrganizationDto), new Tuple<Type, IRepository>(typeof(DataProcessor<Organization, OrganizationDto>), _orgRepository) },
                { nameof(TaskDto), new Tuple<Type, IRepository>(typeof(DataProcessor<Task, TaskDto>), _taskRepository) },
                { nameof(ContactDto), new Tuple<Type, IRepository>(typeof(DataProcessor<Contact, ContactDto>), _contactRepository) },
                { nameof(MailDto), new Tuple<Type, IRepository>(typeof(DataProcessor<Mail, MailDto>), _mailRepository) },
                { nameof(UserDto), new Tuple<Type, IRepository>(typeof(DataProcessor<User, UserDto>), _userRepository) },
            };
        }

        #region Repositories

        public IRegionRepository RegionRepository
        {
            get { return _regionRepository; }
        }
        public ICityRepository CityRepository
        {
            get { return _cityRepository; }
        }
        public IOrganizationRepository OrganizationRepository
        {
            get { return _orgRepository; }
        }
        public ITaskRepository TaskRepository
        {
            get { return _taskRepository; }
        }
        public IContactRepository ContactRepository
        {
            get { return _contactRepository; }
        }
        public IMailRepository MailRepository
        {
            get { return _mailRepository; }
        }
        public IUserRepository UserRepository
        {
            get { return _userRepository; }
        }

        #endregion

        public List<BaseDto> GetAll(string dtoTypeName)
        {
            try
            {
                return GetProcessor(dtoTypeName)?.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<BaseDto> Create(string dtoTypeName, List<BaseDto> baseDtos)
        {
            try
            {
                return GetProcessor(dtoTypeName)?.Create(baseDtos);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(string dtoTypeName, List<BaseDto> baseDtos)
        {
            try
            {
                GetProcessor(dtoTypeName)?.Update(baseDtos);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Remove(string dtoTypeName, List<BaseDto> baseDtos)
        {
            try
            {
                GetProcessor(dtoTypeName)?.Delete(baseDtos);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private IDataProcessor GetProcessor(string dtoTypeName)
        {
            var tuple = _dataProcessors[dtoTypeName];
            return Activator.CreateInstance(tuple.Item1, tuple.Item2) as IDataProcessor;
        }
    }
}
