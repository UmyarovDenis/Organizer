using ContractLibrary.ContractAttributes;
using ContractLibrary.DataTransfer.Dto;
using ContractLibrary.Exceptions;
using System.ServiceModel;

namespace ContractLibrary.DataTransfer
{
    [ServiceContract]
    [ContractDescription(ContractName = "Сервис SQL запросов")]
    public interface ISqlProviderContract
    {
        [OperationContract]
        BaseDto GetDatabaseData();

        [OperationContract]
        [FaultContract(typeof(SqlFault))]
        Response ExecuteSqlQuery(Request request);

        [OperationContract]
        Response ExecuteSqlCommand(Request request);
    }
}
