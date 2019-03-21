using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.Configuration;
using ContractLibrary.DataTransfer.Dto;
using DataTransferService.DataAccessLayer.Models;
using DataTransferService.DataAccessLayer.Repositories.Interfaces;

namespace DataTransferService.DataAccessLayer.Mapping
{
    internal class Mutator<TEntity> where TEntity : class
    {
        private IMapper _entityMutator;
        private IMapper _dtoMutator;

        public Mutator()
        {
            EntityMapping();
            DtoMapping();
        }
        public IMapper EntityMutator
        {
            get { return _entityMutator; }
        }
        public IMapper DtoMutator
        {
            get { return _dtoMutator; }
        }
        public List<TDto> EntitiesToDtoList<TDto>(IEnumerable<TEntity> entities) where TDto : class
        {
            List<TDto> list = new List<TDto>();

            foreach (TEntity entity in entities)
            {
                list.Add(EntityMutator.Map<TDto>(entity));
            }

            return list;
        }
        private void EntityMapping()
        {
            var dtoMutatotConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Region, RegionDto>();
                cfg.CreateMap<City, CityDto>();
                cfg.CreateMap<Organization, OrganizationDto>();
                cfg.CreateMap<Task, TaskDto>();
                cfg.CreateMap<Contact, ContactDto>();
                cfg.CreateMap<Mail, MailDto>();
                cfg.CreateMap<User, UserDto>();
            });

            _entityMutator = dtoMutatotConfig.CreateMapper();
        }
        private void DtoMapping()
        {
            var entityMutatotConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegionDto, Region>();
                cfg.CreateMap<CityDto, City>();
                cfg.CreateMap<OrganizationDto, Organization>();
                cfg.CreateMap<TaskDto, Task>();
                cfg.CreateMap<ContactDto, Contact>();
                cfg.CreateMap<MailDto, Mail>();
                cfg.CreateMap<UserDto, User>();
            });

            _dtoMutator = entityMutatotConfig.CreateMapper();
        }
    }
}
