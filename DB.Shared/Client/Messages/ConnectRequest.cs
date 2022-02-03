using System.Runtime.Serialization;

namespace ABDDB.Shared.Client.Messages
{
    [DataContract]
    public class ConnectRequest
    {
        [DataMember(Order = 1)]
        public string UserName { get; set; }

        [DataMember(Order = 2)]
        public string Password { get; set; }
    }
}