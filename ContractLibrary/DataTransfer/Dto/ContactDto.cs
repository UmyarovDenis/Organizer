using System.Runtime.Serialization;

namespace ContractLibrary.DataTransfer.Dto
{
    [DataContract(IsReference = true)]
    public class ContactDto : BaseDto
    {

        [DataMember]
        public int? Organization_Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Email { get; set; }
        
        [DataMember]
        public string Position { get; set; }

        [DataMember]
        public OrganizationDto Organization { get; set; }
    }
}
