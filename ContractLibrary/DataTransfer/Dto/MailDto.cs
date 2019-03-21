using System;
using System.Runtime.Serialization;

namespace ContractLibrary.DataTransfer.Dto
{
    [DataContract(IsReference = true)]
    public class MailDto : BaseDto
    {

        [DataMember]
        public int? Tasks_Id { get; set; }

        [DataMember]
        public string Sender { get; set; }

        [DataMember]
        public string MailTheme { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime? DepartingDate { get; set; }

        [DataMember]
        public TaskDto Task { get; set; }
    }
}
