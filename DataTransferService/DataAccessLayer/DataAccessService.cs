using ContractLibrary;
using ContractLibrary.DataTransfer;
using ContractLibrary.DataTransfer.Dto;
using ContractLibrary.Notifyer;
using DataTransferService.DataAccessLayer.UnitOfWork;
using DataTransferService.Proxies;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace DataTransferService.DataAccessLayer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DataAccessService : IDataAccessContract
    {
        private Context _context;
        private NotifyProxyService _proxy;

        public DataAccessService()
        {
            _context = new Context();
            _proxy = new NotifyProxyService();
        }
        public List<BaseDto> GetAll(Request request)
        {
            if (request == null)
                throw new FaultException<ArgumentNullException>(new ArgumentNullException("Request is null"));

            if (string.IsNullOrEmpty(request.DataType) || !request.DataType.Contains("Dto"))
                throw new FaultException<ArgumentException>(new ArgumentException("Unknown DTO type"));

            try
            {
                List<BaseDto> data = _context.GetAll(request.DataType);

                if (request.User != null)
                {
                    SendMessageAsync(CreateAction(request.User, "Операция: -- чтение", $"Таблица: {request.DataType}"));
                }

                return data;
            }
            catch (Exception ex)
            {
                Logger.Log.Write(ex.Message);
                throw new FaultException<Exception>(ex);
            }
        }
        public BaseDto GetItem(Request request)
        {
            throw new NotImplementedException();
        }
        public List<BaseDto> Create(Request request)
        {
            if (request == null)
                throw new FaultException<ArgumentNullException>(new ArgumentNullException("Request is null"));

            if (string.IsNullOrEmpty(request.DataType) || !request.DataType.Contains("Dto"))
                throw new FaultException<ArgumentException>(new ArgumentException("Unknown DTO type"));

            if (request.Data == null)
                throw new FaultException<ArgumentNullException>(new ArgumentNullException("Data not defined"));

            try
            {
                List<BaseDto> data =  _context.Create(request.DataType, request.Data as List<BaseDto>);

                if (request.User != null)
                {
                    SendMessageAsync(CreateAction(request.User, "Операция: -- создание", $"Таблица: {request.DataType}"));
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new FaultException<Exception>(ex);
            }
        }
        public Response Update(Request request)
        {
            if (request == null)
                throw new FaultException<ArgumentNullException>(new ArgumentNullException("Request is null"));

            if (string.IsNullOrEmpty(request.DataType) || !request.DataType.Contains("Dto"))
                throw new FaultException<ArgumentException>(new ArgumentException("Unknown DTO type"));

            if (request.Data == null)
                throw new FaultException<ArgumentNullException>(new ArgumentNullException("Data not defined"));

            try
            {
                _context.Update(request.DataType, request.Data as List<BaseDto>);

                if (request.User != null)
                {
                    SendMessageAsync(CreateAction(request.User, "Операция: -- изменени", $"Таблица: {request.DataType}"));
                }

                return new Response
                {
                    DataType = typeof(bool).Name,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                throw new FaultException<Exception>(ex, ex.Message);
            }
        }
        public Response Remove(Request request)
        {
            if (request == null)
                throw new FaultException<ArgumentNullException>(new ArgumentNullException("Request is null"));

            if (string.IsNullOrEmpty(request.DataType) || !request.DataType.Contains("Dto"))
                throw new FaultException<ArgumentException>(new ArgumentException("Unknown DTO type"));

            if (request.Data == null)
                throw new FaultException<ArgumentNullException>(new ArgumentNullException("Data not defined"));

            try
            {
                _context.Remove(request.DataType, request.Data as List<BaseDto>);

                if (request.User != null)
                {
                    SendMessageAsync(CreateAction(request.User, "Операция: -- удаление", $"Таблица: {request.DataType}"));
                }

                return new Response
                {
                    DataType = typeof(bool).Name,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                throw new FaultException<Exception>(ex, new FaultReason(ex.Message));
            }
        }
        private async void SendMessageAsync(ActionDto action)
        {
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        _proxy.ProxyService.Send(action);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Write($"Error: {ex.Message} | InnerException {ex?.InnerException.Message}");
                    }
                });
            }
            catch (Exception ex)
            {

            }
        }
        private ActionDto CreateAction(UserDto user, string action, string details)
        {
            return new ActionDto
            {
                TimeStamp = DateTime.Now.ToString(),
                User = user,
                Action = action,
                Details = details
            };
        }
    }
}
