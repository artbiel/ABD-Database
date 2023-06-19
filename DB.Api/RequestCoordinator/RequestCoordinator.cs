using ABDDB.Replication.Contracts;

namespace ABDDB.Api.RequestCoordinator
{
    public class RequestCoordinator : IRequestCoordinator
    {
        private readonly ICoordinator _coordinator;

        public RequestCoordinator(ICoordinator coordinator)
        {
            _coordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
        }

        public Task<string> GetAsync(string key, CancellationToken token = default)
            => _coordinator.ReadAsync(key);

        public Task PutAsync(string key, string value, CancellationToken token = default)
            => _coordinator.WriteAsync(key, value);

        public Task DeleteAsync(string key, CancellationToken token = default)
            => _coordinator.WriteAsync(key, null);
    }
}
