using ABDDB.Replication.Contracts;
using ABDDB.Replication.Transport;

namespace ABDDB.Replication.Services
{
    public class ReplicasService : IReplicasService
    {
        private readonly IChannelPool _channelPool;
        private readonly IEnumerable<IReplica> _replicas;

        public ReplicasService(IChannelPool channelPool)
        {
            _channelPool = channelPool ?? throw new ArgumentNullException(nameof(channelPool));
            _replicas = _channelPool
                .GetChannels()
                .Select(c => c.GetService<IReplica>());
        }

        IEnumerable<IReplica> IReplicasService.GetReplicas() => _replicas;
    }
}