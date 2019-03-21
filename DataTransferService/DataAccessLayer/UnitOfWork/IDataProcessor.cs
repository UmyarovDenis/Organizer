using ContractLibrary.DataTransfer.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferService.DataAccessLayer.UnitOfWork
{
    internal interface IDataProcessor
    {
        List<BaseDto> GetAll();
        List<BaseDto> Create(List<BaseDto> items);
        void Update(List<BaseDto> items);
        void Delete(List<BaseDto> items);
    }
}
