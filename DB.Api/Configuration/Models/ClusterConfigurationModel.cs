using ABDDB.Replication;

namespace ADBDB.Api.Configuration.Models
{
    public struct ClusterConfigurationModel
    {
        public List<Node> Nodes { get; set; }
        public int ReadQuorum { get; set; }
        public int WriteQuorum { get; set; }
    }
}
