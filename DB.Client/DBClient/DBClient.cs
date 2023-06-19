using ABDDB.Client.Services;
using ABDDB.Shared.Store;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc.Client;
using System.Text.Json;

namespace ABDDB.Client.DBClient
{
    public class DBClient : IDBClient
    {
        private readonly IChannelPool _channelPool;
        private readonly ILogger<DBClient> _logger;

        public DBClient(IChannelPool channelPool, ILoggerFactory loggerFactory)
        {
            _channelPool = channelPool ?? throw new ArgumentNullException(nameof(channelPool));
            _logger = loggerFactory?.CreateLogger<DBClient>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public bool IsConnected => _channelPool.GetActiveChannel() is not null;

        public Task<TResult> GetAsync<TResult, TKey>(TKey key) => _channelPool.Execute<TResult>(async (channel) =>
        {
            var serializedKey = JsonSerializer.Serialize(key);
            var store = channel.CreateGrpcService<IStoreService>();
            _logger.LogDebug($"Getting result by key \"{key}\"");
            var result = await store.GetAsync(new GetRequest { Key = serializedKey });
            _logger.LogDebug($"Received result by key \"{key}\": {result.Value}");
            return result?.Value is not null ? JsonSerializer.Deserialize<TResult>(result.Value) : default;
        });

        public Task PutAsync<TResult, TKey>(TKey key, TResult value) => _channelPool.Execute(async (channel) =>
        {
            _logger.LogDebug($"Putting value \"{value}\" by key \"{key}\"...");
            var serializedKey = JsonSerializer.Serialize(key);
            var serializedValue = JsonSerializer.Serialize(value);
            var store = channel.CreateGrpcService<IStoreService>();
            await store.PutAsync(new PutRequest { Key = serializedKey, Value = serializedValue });
            _logger.LogDebug($"Value \"{value}\" was put by key \"{key}\"");
        });

        public Task DeleteAsync<TKey>(TKey key) => _channelPool.Execute(async (channel) =>
        {
            _logger.LogDebug($"Deleting record by key \"{key}\"...");
            var serializedKey = JsonSerializer.Serialize(key);
            var store = channel.CreateGrpcService<IStoreService>();
            await store.DeleteAsync(new DeleteRequest { Key = serializedKey });
            _logger.LogDebug($"Record by key \"{key}\" was deleted");
        });
    }
}