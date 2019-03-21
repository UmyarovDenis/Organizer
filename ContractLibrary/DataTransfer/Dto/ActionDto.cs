using System.Runtime.Serialization;

namespace ContractLibrary.DataTransfer.Dto
{
    [DataContract]
    public class ActionDto
    {
        [DataMember]
        public string TimeStamp { get; set; }

        [DataMember]
        public UserDto User { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public string Details { get; set; }
    }
}
