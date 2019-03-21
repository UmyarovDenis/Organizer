using AppController.Core.DIContainer;
using AppController.Core.Modules;
using ContractLibrary;
using ContractLibrary.DataTransfer;
using ContractLibrary.DataTransfer.Dto;
using ContractLibrary.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Organizer.Services.Net
{
    internal sealed class DataServiceProxy : SingleServiceProxy<IDataAccessContract>, IDataServiceProxy
    {
        private IContainerCore _container;

        public DataServiceProxy(IModule module) : base("DataServiceHttp")
        {
            _container = new DIContainer(module);
        }

        public List<TDto> LoadData<TDto>() where TDto : BaseDto
        {
            List<TDto> dataList = null;

            Execute((proxy) =>
            {
                dataList = proxy.GetAll(CreateRequest<TDto>())
                                .Cast<TDto>()
                                .ToList();
            });

            return dataList;
        }
        public List<TDto> CreateItems<TDto>(params TDto[] items) where TDto : BaseDto
        {
            if (items == null)
                throw new ArgumentNullException("Items was null");

            List<TDto> dataList = null;

            Execute((proxy) =>
            {
                dataList = proxy.Create(CreateRequest<TDto>(items.ToList<BaseDto>()))
                                .Cast<TDto>()
                                .ToList();
            });

            return dataList;
        }
        public bool UpdateItems<TDto>(params TDto[] items) where TDto : BaseDto
        {
            if (items == null)
                throw new ArgumentNullException("Items was null");

            Response response = null;

            Execute((proxy) => 
            {
                response = proxy.Update(CreateRequest<TDto>(items.ToList<BaseDto>()));
            });

            return (bool)response.Data;
        }
        public bool RemoveItems<TDto>(params TDto[] items) where TDto : BaseDto
        {
            if (items == null)
                throw new ArgumentNullException("Items was null");

            Response response = null;

            Execute((proxy) =>
            {
                response = proxy.Remove(CreateRequest<TDto>(items.ToList<BaseDto>()));
            });

            return (bool)response.Data;
        }
        public Request CreateRequest<TData>(object data = null)
        {
            return new Request
            {
                DataType = typeof(TData).Name,
                Data = data,
                User = _container.ResolveInstance<UserDto>()
            };
        }
    }
}
