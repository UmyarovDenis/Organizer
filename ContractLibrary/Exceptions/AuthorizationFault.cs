using System.Runtime.Serialization;

namespace ContractLibrary.Exceptions
{
    [DataContract]
    public class AuthorizationFault
    {
        [DataMember]
        public string Error { get; set; }
        public AuthorizationFault(string errorMessage)
        {
            Error = errorMessage;
        }
    }
}
