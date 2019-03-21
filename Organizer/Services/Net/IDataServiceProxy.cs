using ContractLibrary.DataTransfer.Dto;
using System.Collections.Generic;

namespace Organizer.Services.Net
{
    internal interface IDataServiceProxy
    {
        List<TDto> LoadData<TDto>() where TDto : BaseDto;
        List<TDto> CreateItems<TDto>(params TDto[] items) where TDto : BaseDto;
        bool UpdateItems<TDto>(params TDto[] items) where TDto : BaseDto;
        bool RemoveItems<TDto>(params TDto[] items) where TDto : BaseDto;
    }
}
