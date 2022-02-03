
namespace ABDDB.Client.DBClient
{
    public interface IDBClient
    {
        bool IsConnected { get; }

        Task<TResult> GetAsync<TResult, TKey>(TKey key);
        Task PutAsync<TResult, TKey>(TKey key, TResult value);
    }
}