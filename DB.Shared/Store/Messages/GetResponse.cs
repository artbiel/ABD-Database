using System.Runtime.Serialization;

namespace ABDDB.Shared.Store
{
    [DataContract]
    public class GetResponse
    {
        [DataMember(Order = 1)]
        public string Value { get; set; }
    }
}