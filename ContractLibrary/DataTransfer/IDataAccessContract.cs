using System.ServiceModel;
using System.Collections.Generic;
using ContractLibrary.DataTransfer.Dto;
using ContractLibrary.ContractAttributes;

namespace ContractLibrary.DataTransfer
{
    [ServiceContract]
    [ContractDescription(ContractName = "Сервис передачи данных")]
    public interface IDataAccessContract
    {
        [OperationContract]
        List<BaseDto> GetAll(Request request);

        [OperationContract]
        BaseDto GetItem(Request request);

        [OperationContract]
        List<BaseDto> Create(Request request);

        [OperationContract]
        Response Update(Request request);

        [OperationContract]
        Response Remove(Request request);
    }
}
