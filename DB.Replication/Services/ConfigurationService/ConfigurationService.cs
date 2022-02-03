namespace ABDDB.Replication.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public ConfigurationService(ClusterConfiguration clusterConfiguration,
            TransportConfiguration transportConfiguration,
            SecurityConfiguration securityConfiguration)
        {
            if (clusterConfiguration is null)
                throw new ArgumentNullException(nameof(clusterConfiguration));
            if (transportConfiguration is null)
                throw new ArgumentNullException(nameof(transportConfiguration));
            if (securityConfiguration is null)
                throw new ArgumentNullException(nameof(securityConfiguration));

            ClusterConfiguration = clusterConfiguration;
            TransportConfiguration = transportConfiguration;
            SecurityConfiguration = securityConfiguration;
        }

        public ClusterConfiguration ClusterConfiguration { get; }
        public TransportConfiguration TransportConfiguration { get; }
        public SecurityConfiguration SecurityConfiguration { get; }
    }
}
