using ABDDB.Api.Configuration.Models;
using ABDDB.Replication;
using ABDDB.Replication.Services;
using ADBDB.Api.Configuration.Models;
using System.Security.Cryptography.X509Certificates;

namespace ABDDB.Api.Utils
{
    public static class ConfigUtils
    {
        public static ConfigurationModel GetConfiguration(IConfiguration configuration, string[] args) =>
            new ConfigurationModel
            {
                ClusterConfiguration = GetClusterConfiguration(configuration, args),
                TransportConfiguration = GetTransportConfiguration(configuration),
                SecurityConfiguration = GetSecurityConfiguration(configuration)
            };

        private static ClusterConfiguration GetClusterConfiguration(IConfiguration configuration, string[] args)
        {
            var nodes = GetNodes(configuration);
            var currentNode = GetCurrentNode(nodes, args);
            var (readQuorum, writeQuorum) = GetQuorums(configuration);
            return new ClusterConfiguration(nodes, currentNode, readQuorum, writeQuorum);
        }

        private static TransportConfiguration GetTransportConfiguration(IConfiguration configuration)
        {
            var configModel = configuration.GetSection("TransportConfig").Get<TransportConfigurationModel>();
            var transportConfig = new TransportConfiguration(configModel.MaxRetryAttempts,
                configModel.InitialBackoff, configModel.MaxBackoff, configModel.BackoffMultiplier);
            return transportConfig;
        }

        private static SecurityConfiguration GetSecurityConfiguration(IConfiguration configuration)
        {
            var configModel = configuration.GetSection("SecurityConfig").Get<SecurityConfigurationModel>();
            var certificate = GetCertificate(configModel);
            var secConfig = new SecurityConfiguration(certificate, configModel.AllowedThumbprints);
            return secConfig;
        }

        private static Node GetCurrentNode(IList<Node> nodes, string[] args)
        {
            var id = Convert.ToInt32(args[0]);
            return nodes.FirstOrDefault(n => n.Id == id)
                ?? throw new ArgumentOutOfRangeException(nameof(args));
        }

        private static IList<Node> GetNodes(IConfiguration configuration)
        {
            var nodes = new List<Node>();
            configuration.GetSection("ClusterConfig:Nodes").Bind(nodes);
            return nodes;
        }

        private static (int, int) GetQuorums(IConfiguration configuration)
        {
            var readQuorum = configuration.GetSection("ClusterConfig:ReadQuorum").Get<int>();
            var writeQuorum = configuration.GetSection("ClusterConfig:WriteQuorum").Get<int>();
            return (readQuorum, writeQuorum);
        }

        private static X509Certificate2 GetCertificate(SecurityConfigurationModel configModel)
        {
            var certificatePath = configModel.Certificate.Path;
            if (!certificatePath.Contains('/'))
                certificatePath = Path.Combine(Directory.GetCurrentDirectory(), certificatePath);
            return new X509Certificate2(certificatePath, configModel.Certificate.Password);
        }
    }
}
