using System.Runtime.Serialization;

namespace ABDDB.Replication.Contracts
{
    [DataContract]
    public class ReadRequest
    {
        [DataMember(Order = 1)]
        public string Key { get; set; }
    }
}