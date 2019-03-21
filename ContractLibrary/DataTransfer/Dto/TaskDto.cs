using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ContractLibrary.DataTransfer.Dto
{
    [DataContract(IsReference = true)]
    public class TaskDto : BaseDto
    {
        public TaskDto()
        {
            Mails = new List<MailDto>();
        }

        [DataMember]
        public int? Organization_Id { get; set; }

        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal? ProjectCost { get; set; }

        [DataMember]
        public DateTime? StartDate { get; set; }

        [DataMember]
        public DateTime? Deadline { get; set; }

        [DataMember]
        public int? Status { get; set; }

        [DataMember]
        public string ImplementationPeriod { get; set; }

        [DataMember]
        public List<MailDto> Mails { get; set; }

        [DataMember]
        public OrganizationDto Organization { get; set; }
    }
}
