using System;
using System.Data;
using ContractLibrary;
using ContractLibrary.DataTransfer;
using ContractLibrary.DataTransfer.Dto;
using ContractLibrary.Proxies;

namespace Organizer.Services.Net
{
    internal sealed class SqlServiceProxy : SingleServiceProxy<ISqlProviderContract>, ISqlServiceProxy
    {
        public SqlServiceProxy() : base("SqlServiceHttp")
        { }
        public DatabaseDto GetDatabaseData()
        {
            DatabaseDto databaseDto = null;

            Execute((proxy) => 
            {
                databaseDto = proxy.GetDatabaseData() as DatabaseDto;
            });

            return databaseDto;
        }
        public DataTable ExecuteSqlQuery(string sqlExpression)
        {
            DataTable dataTable = null;

            Execute((proxy) =>
            {
                Response response = proxy.ExecuteSqlQuery(CreateRequest<string>(sqlExpression));

                dataTable = response.Data as DataTable;
            });

            return dataTable;
        }
        public void ExecuteSqlCommand(string sqlExpression)
        {
            throw new NotImplementedException();
        }
        public Request CreateRequest<TData>(object data = null)
        {
            return new Request
            {
                DataType = typeof(TData).Name,
                Data = data
            };
        }
    }
}
