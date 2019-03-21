using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ContractLibrary.DataTransfer.Dto
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(RegionDto))]
    [KnownType(typeof(CityDto))]
    [KnownType(typeof(OrganizationDto))]
    [KnownType(typeof(ContactDto))]
    [KnownType(typeof(TaskDto))]
    [KnownType(typeof(MailDto))]
    [KnownType(typeof(DatabaseDto))]
    [KnownType(typeof(TableDto))]
    [KnownType(typeof(ServiceDescriptionDto))]
    [KnownType(typeof(UserDto))]
    [KnownType(typeof(HostDto))]
    public class BaseDto
    {
        [DataMember]
        public int Id { get; set; }
    }
}
