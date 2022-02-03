using System.Runtime.Serialization;

namespace ABDDB.Shared.Client.Messages
{
    [DataContract]
    public class ConnectResponse
    {
        [DataMember(Order = 1)]
        public bool Authenticated { get; set; }
    }
}
