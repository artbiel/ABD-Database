using ABDDB.Replication.Services;

namespace ADBDB.Api.Configuration.Models
{
    public class ConfigurationModel
    {
        public ClusterConfiguration ClusterConfiguration { get; set; }
        public TransportConfiguration TransportConfiguration { get; set; }
        public SecurityConfiguration SecurityConfiguration { get; set; }
    }
}
