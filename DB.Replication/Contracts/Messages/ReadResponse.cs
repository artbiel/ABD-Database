using ABDDB.Replication.Contracts.DTOs;
using System.Runtime.Serialization;

namespace ABDDB.Replication.Contracts
{
    [DataContract]
    public record ReadResponse
    {
        [DataMember(Order = 1)]
        public string Value { get; set; }

        [DataMember(Order = 2)]
        public TimestampDTO TimestampModel { get; set; }
    }
}