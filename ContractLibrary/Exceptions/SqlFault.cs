using System.Runtime.Serialization;

namespace ContractLibrary.Exceptions
{
    [DataContract]
    public class SqlFault
    {
        [DataMember]
        public string Error { get; set; }

        public SqlFault(string error)
        {

            Error = error;
        }
    }
}
