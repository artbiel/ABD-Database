using ABDDB.LocalStorage.Models;
using ABDDB.Replication.Contracts;
using ABDDB.Replication.Services;
using ABDDB.Replication.Utils;

namespace ABDDB.Replication.Actors
{
    public class Coordinator : ICoordinator
    {
        private readonly IReplicasService _replicasService;
        private readonly IConfigurationService _configuration;

        public Coordinator(IReplicasService replicasService, IConfigurationService configuration)
        {
            _replicasService = replicasService ?? throw new ArgumentNullException(nameof(replicasService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task WriteAsync(string key, string value, CancellationToken token = default)
        {
            var replicas = _replicasService.GetReplicas();
            var readQuorum = _configuration.ClusterConfiguration.ReadQuorum;
            var writeQuorum = _configuration.ClusterConfiguration.WriteQuorum;

            var readTasks = replicas
                .Select(r => r.ReadAsync(new ReadRequest { Key = key }))
                .ToArray();

            var reads = await Combinators.WhenSome(readTasks, readQuorum, token);

            var newTimestamp = reads.Select(r => r.TimestampModel.Timestamp).Max() + 1;
            var salt = Guid.NewGuid();
            var timestampModel = new TimestampModel(newTimestamp, salt);


            var writeTasks = replicas
               .Select(r => r.WriteAsync(new WriteRequest 
                   { Key = key, TimestampModel = timestampModel, Value = value }))
               .ToArray();

            await Combinators.WhenSome(writeTasks, writeQuorum, token);
        }

        public async Task<string> ReadAsync(string key, CancellationToken token = default)
        {
            var replicas = _replicasService.GetReplicas();
            var readQuorum = _configuration.ClusterConfiguration.ReadQuorum;
            var writeQuorum = _configuration.ClusterConfiguration.WriteQuorum;

            var readTasks = replicas
                .Select(ch => ch.ReadAsync(new ReadRequest { Key = key }, default))
                .ToArray();

            var reads = await Combinators.WhenSome(readTasks, readQuorum, token);

            var maxTimestampValue = reads.MaxBy(r => r.TimestampModel.Timestamp);
            var timestampsConflicts = reads
                .Select(r => r.TimestampModel.Timestamp)
                .Any(t => t != maxTimestampValue.TimestampModel.Timestamp);

            if (timestampsConflicts)
            {
                var writeTasks = replicas
                   .Select(ch => ch.WriteAsync(new WriteRequest
                   {
                       Key = key.ToString(),
                       TimestampModel = maxTimestampValue.TimestampModel,
                       Value = maxTimestampValue.Value
                   }))
                   .ToArray();

                await Combinators.WhenSome(writeTasks, writeQuorum, token);
            }

            return maxTimestampValue.Value;
        }
    }
}