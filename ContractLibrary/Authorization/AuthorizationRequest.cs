using ContractLibrary.DataTransfer.Dto;
using System.Runtime.Serialization;

namespace ContractLibrary.Authorization
{
    [DataContract]
    public class AuthorizationRequest
    {
        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public HostDto Host { get; set; }
    }
}
