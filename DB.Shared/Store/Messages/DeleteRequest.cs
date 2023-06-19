using System.Runtime.Serialization;

namespace ABDDB.Shared.Store
{
    [DataContract]
    public class DeleteRequest
    {
        [DataMember(Order = 1)]
        public string Key { get; set; }
    }
}