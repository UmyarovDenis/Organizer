using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ContractLibrary.DataTransfer.Dto
{
    [DataContract(IsReference = true)]
    public class CityDto : BaseDto
    {
        public CityDto()
        {
            Organizations = new List<OrganizationDto>();
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string GMT { get; set; }

        [DataMember]
        public int? Region_Id { get; set; }

        [DataMember]
        public RegionDto Region { get; set; }

        [DataMember]
        public List<OrganizationDto> Organizations { get; set; }
    }
}
