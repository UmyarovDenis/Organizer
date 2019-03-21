using ContractLibrary;
using ContractLibrary.DataTransfer;
using ContractLibrary.DataTransfer.Dto;
using ContractLibrary.Exceptions;
using DataTransferService.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace DataTransferService.SqlData
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class SqlDataService : ISqlProviderContract
    {
        public BaseDto GetDatabaseData()
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                DatabaseDto databaseDto = new DatabaseDto();
                databaseDto.SourceName = "PolitermUsers";

                sb.Append("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES ");
                sb.Append("WHERE TABLE_NAME NOT LIKE '__MigrationHistory' ");
                sb.Append("ORDER BY(TABLE_NAME)");

                List<string> tableNames;

                using (PolitermUsers db = new PolitermUsers())
                {
                    tableNames = db.Database.SqlQuery<string>(sb.ToString()).ToList();
                    sb.Clear();

                    foreach (var tableName in tableNames)
                    {
                        sb.AppendLine("SELECT COLUMN_NAME ");
                        sb.AppendLine("FROM INFORMATION_SCHEMA.COLUMNS ");
                        sb.AppendFormat("WHERE TABLE_NAME = '{0}'", tableName);

                        databaseDto.Tables.Add(new TableDto
                        {
                            TableName = tableName,
                            ColumnNames = db.Database.SqlQuery<string>(sb.ToString()).ToList()
                        });

                        sb.Clear();
                    }
                }

                return databaseDto;
            }
            catch (Exception ex)
            {
                throw new FaultException<Exception>(ex, ex.Message);
            }
        }
        public Response ExecuteSqlQuery(Request request)
        {
            DataTable dt = new DataTable("TableResponse");
            string connectionString = ConfigurationManager.ConnectionStrings["PolitermUsers"].ConnectionString;

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand(request.Data.ToString(), sqlConnection);
                    
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }

                return new Response
                {
                    DataType = typeof(DataTable).Name,
                    Data = dt
                };
            }
            catch(SqlException ex)
            {
                throw new FaultException<SqlFault>(new SqlFault(ex.Message), new FaultReason(ex.Message));
            }
            catch (Exception ex)
            {
                throw new FaultException<Exception>(ex, ex.Message);
            }
        }
        public Response ExecuteSqlCommand(Request request)
        {
            throw new System.NotImplementedException();
        }
    }
}
