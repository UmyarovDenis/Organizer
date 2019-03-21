using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ContractLibrary.DataTransfer.Dto
{
    [DataContract]
    public class DatabaseDto : BaseDto
    {
        public DatabaseDto()
        {
            Tables = new List<TableDto>();
        }
        [DataMember]
        public string SourceName { get; set; }

        [DataMember]
        public List<TableDto> Tables { get; set; }
    }
}
