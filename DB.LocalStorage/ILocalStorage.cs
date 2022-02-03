using ABDDB.LocalStorage.Models;

namespace ABDDB.LocalStorage;

public interface ILocalStorage
{
    Task<ValueModel> GetAsync(string key);
    Task PutAsync(string key, ValueModel value);
}
