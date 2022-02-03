using ABDDB.Replication.Services;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;

namespace ABDDB.Replication.Transport
{
    public class ChannelPool : IChannelPool
    {
        private readonly List<IChannel> _channels = new();

        public ChannelPool(IConfigurationService configService, IServiceProvider serviceProvider)
        {
            if (configService is null)
                throw new ArgumentNullException(nameof(configService));
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));

            var settings = GetChannelOptions(configService);
            foreach (var node in configService.ClusterConfiguration.OtherNodes)
            {
                _channels.Add(new RPCChannel(node, GrpcChannel.ForAddress(node.Uri, settings)));
            }
            _channels.Add(new LocalChannel(configService.ClusterConfiguration.CurrentNode, serviceProvider));
        }

        public IChannel? this[Node node] => GetChannel(node);

        public IChannel? GetChannel(Node node) => _channels.FirstOrDefault(c => c.Node == node);

        public IEnumerable<IChannel> GetChannels() => _channels;

        public void Dispose()
        {
            foreach (var channel in _channels)
                channel.Dispose();
        }

        private GrpcChannelOptions GetChannelOptions(IConfigurationService configService)
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(configService.SecurityConfiguration.Certificate);
            return new GrpcChannelOptions
            {
                HttpHandler = handler,
                ServiceConfig = new()
                {
                    MethodConfigs =
                    {
                        new MethodConfig
                        {
                            Names = { MethodName.Default },
                            RetryPolicy = new RetryPolicy
                            {
                                MaxAttempts = configService.TransportConfiguration.MaxRetryAttempts,
                                InitialBackoff = configService.TransportConfiguration.InitialBackoff,
                                MaxBackoff = configService.TransportConfiguration.MaxBackoff,
                                BackoffMultiplier = configService.TransportConfiguration.BackoffMultiplier,
                                RetryableStatusCodes = { StatusCode.Unavailable }
                            }
                        }
                    }
                },
                MaxRetryAttempts = configService.TransportConfiguration.MaxRetryAttempts
            };
        }
    }
}