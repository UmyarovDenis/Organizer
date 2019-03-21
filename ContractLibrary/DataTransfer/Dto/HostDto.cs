using System.Runtime.Serialization;

namespace ContractLibrary.DataTransfer.Dto
{
    [DataContract]
    public class HostDto : BaseDto
    {
        [DataMember]
        public string ExternalIPAddress { get; set; }

        [DataMember]
        public string LocalIPAddress { get; set; }

        [DataMember]
        public string MachineName { get; set; }

        public void Erase()
        {
            ExternalIPAddress = string.Empty;
            ExternalIPAddress = "";
            LocalIPAddress = "";
            MachineName = "";
        }
    }
}
