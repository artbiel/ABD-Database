using System.Runtime.Serialization;

namespace ABDDB.Shared.Store
{
    [DataContract]
    public class PutRequest
    {
        [DataMember(Order = 1)]
        public string Key { get; set; }

        [DataMember(Order = 2)]
        public string Value { get; set; }
    }
}