using ContractLibrary.DataTransfer.Dto;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ContractLibrary
{
    [DataContract]
    [KnownType(typeof(List<BaseDto>))]
    [KnownType(typeof(BaseDto))]
    [KnownType(typeof(UserDto))]
    public class Request
    {
        [DataMember]
        public string DataType { get; set; }

        [DataMember]
        public object Data { get; set; }

        [DataMember]
        public UserDto User { get; set; }
    }
}
