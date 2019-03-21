using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ContractLibrary.DataTransfer.Dto
{
    [DataContract]
    public class TableDto : BaseDto
    {
        public TableDto()
        {
            ColumnNames = new List<string>();
        }
        [DataMember]
        public string TableName { get; set; }

        [DataMember]
        public List<string> ColumnNames { get; set; }
    }
}
