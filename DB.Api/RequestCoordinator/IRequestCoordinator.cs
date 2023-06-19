namespace ABDDB.Api.RequestCoordinator
{
    public interface IRequestCoordinator
    {
        Task<string> GetAsync(string key, CancellationToken token = default);

        Task PutAsync(string key, string value, CancellationToken token = default);
        
        Task DeleteAsync(string key, CancellationToken token = default);
    }
}
