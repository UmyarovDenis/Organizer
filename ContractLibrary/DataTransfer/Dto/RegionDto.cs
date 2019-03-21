using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ContractLibrary.DataTransfer.Dto
{
    [DataContract(IsReference = true)]
    public class RegionDto : BaseDto
    {
        public RegionDto()
        {
            Cities = new List<CityDto>();
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<CityDto> Cities { get; set; }
    }
}
