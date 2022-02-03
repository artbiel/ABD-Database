
namespace ABDDB.Replication.Contracts
{
    public interface ICoordinator
    {
        Task<string> ReadAsync(string key, CancellationToken token = default);
        Task WriteAsync(string key, string value, CancellationToken token = default);
    }
}