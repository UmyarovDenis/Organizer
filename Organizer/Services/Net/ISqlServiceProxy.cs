using ContractLibrary.DataTransfer.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Services.Net
{
    internal interface ISqlServiceProxy
    {
        DatabaseDto GetDatabaseData();
        DataTable ExecuteSqlQuery(string sqlExpression);
        void ExecuteSqlCommand(string sqlExpression);
    }
}
