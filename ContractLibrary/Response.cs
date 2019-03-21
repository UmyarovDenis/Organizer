using System;
using System.Data;
using System.Runtime.Serialization;

namespace ContractLibrary
{
    [DataContract]
    [KnownType(typeof(DataTable))]
    [KnownType(typeof(Exception))]
    public class Response
    {
        [DataMember]
        public string DataType { get; set; }

        [DataMember]
        public object Data { get; set; }
    }
}
