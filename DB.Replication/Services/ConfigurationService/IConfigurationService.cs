namespace ABDDB.Replication.Services
{
    public interface IConfigurationService
    {
        ClusterConfiguration ClusterConfiguration { get; }
        TransportConfiguration TransportConfiguration { get; }
        SecurityConfiguration SecurityConfiguration { get; }
    }
}
