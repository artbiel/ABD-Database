using ABDDB.LocalStorage.Exceptions;
using ABDDB.LocalStorage.Models;

namespace ABDDB.LocalStorage
{
    public class LocalStorage : ILocalStorage
    {
        private readonly Dictionary<string, ValueModel> _store = new Dictionary<string, ValueModel>();

        private List<string> _reservedKeys;

        public LocalStorage()
        {
            foreach (var (key, value) in StoreInitializer.GetInitialValues())
            {
                _store.Add(key, value);
            }
            _reservedKeys = StoreInitializer.GetInitialValues().Select(v => v.Item1).ToList();
        }

        public Task<ValueModel> GetAsync(string key)
        {
            var exist = _store.TryGetValue(key, out var val);
            return Task.FromResult(exist ? val : default);
        }

        public async Task PutAsync(string key, ValueModel value)
        {
            if (_reservedKeys.Contains(key))
                throw new ReservedKeyException();
            var (_, currentTimestamp) = await GetAsync(key);
            if (currentTimestamp < value.Timestamp)
            {
                if (_store.ContainsKey(key))
                    _store.Remove(key);
                _store.Add(key, value);
            }
        }
    }
}