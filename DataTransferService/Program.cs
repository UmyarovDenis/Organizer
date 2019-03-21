using ContractLibrary.DataTransfer.Dto;
using DataTransferService.DataAccessLayer.UnitOfWork;
using DataTransferService.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ContractLibrary;

namespace DataTransferService
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlData.SqlDataService sqlDataService = new SqlData.SqlDataService();

            Request req = new Request
            {
                DataType = nameof(String),
                Data = "select * from Cities join Regions On Region_Id = Regions.Id where Region.Name Like 'Арх%'"
            };

            sqlDataService.ExecuteSqlQuery(req);
        }
    }
}
