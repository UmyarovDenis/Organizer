using DataTransferService.DataAccessLayer.Mapping;
using DataTransferService.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using ContractLibrary.DataTransfer.Dto;

namespace DataTransferService.DataAccessLayer.UnitOfWork
{
    internal class DataProcessor<TEntity, TDto> : IDataProcessor where TEntity : class where TDto : BaseDto
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly Mutator<TEntity> _mutator = new Mutator<TEntity>();

        public DataProcessor(IGenericRepository<TEntity> repository)
        {
            _repository = repository;
        }
        public List<BaseDto> Create(List<BaseDto> items)
        {
            try
            {
                IList<TEntity> data = _repository.Create(FromDtosToEntityCollection(items));

                return new List<BaseDto>(_mutator.EntitiesToDtoList<TDto>(data));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<BaseDto> GetAll()
        {
            try
            {
                IList<TEntity> data = _repository.GetAll();

                return new List<BaseDto>(_mutator.EntitiesToDtoList<TDto>(data));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void Update(List<BaseDto> items)
        {
            try
            {
                _repository.Update(FromDtosToEntityCollection(items));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void Delete(List<BaseDto> items)
        {
            try
            {
                _repository.Remove(FromDtosToEntityCollection(items));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private List<TEntity> FromDtosToEntityCollection(IEnumerable<BaseDto> items)
        {
            List<TEntity> entities = new List<TEntity>();

            foreach (var item in items)
            {
                entities.Add(_mutator.DtoMutator.Map<TEntity>(item));
            }

            return entities;
        }
    }
}
