using System.Runtime.Serialization;

namespace ContractLibrary.DataTransfer.Dto
{
    [DataContract]
    public class UserDto : BaseDto
    {
        public UserDto()
        {
            Host = new HostDto();
        }

        [DataMember]
        public string FIO { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public HostDto Host { get; set; }

        [DataMember]
        public bool IsConnected { get; set; }
    }
}
