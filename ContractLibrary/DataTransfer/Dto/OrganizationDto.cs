using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ContractLibrary.DataTransfer.Dto
{
    [DataContract(IsReference = true)]
    public class OrganizationDto : BaseDto
    {
        public OrganizationDto()
        {
            Contacts = new List<ContactDto>();
            Tasks = new List<TaskDto>();
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int? City_Id { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public CityDto City { get; set; }

        [DataMember]
        public List<ContactDto> Contacts { get; set; }

        [DataMember]
        public List<TaskDto> Tasks { get; set; }
    }
}
