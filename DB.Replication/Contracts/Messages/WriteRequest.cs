using ABDDB.Replication.Contracts.DTOs;
using System.Runtime.Serialization;

namespace ABDDB.Replication.Contracts
{
    [DataContract]
    public class WriteRequest
    {
        [DataMember(Order = 1)]
        public string Key { get; set; }

        [DataMember(Order = 2)]
        public string Value { get; set; }

        [DataMember(Order = 3)]
        public TimestampDTO TimestampModel { get; set; }
    }
}