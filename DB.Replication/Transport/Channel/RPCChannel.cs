using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;

namespace ABDDB.Replication.Transport
{
    public class RPCChannel : IChannel
    {
        private readonly GrpcChannel _channel;
        private readonly Node _node;

        public RPCChannel(Node node, GrpcChannel channel)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(node));
            _node = node ?? throw new ArgumentNullException(nameof(channel));
        }

        public T GetService<T>() where T : class => _channel.CreateGrpcService<T>();

        public void Dispose() => _channel.Dispose();

        public Node Node => _node;
    }
}