using Microsoft.AspNetCore.Http;
using System.Text;

namespace ASC.Tests.Mocks
{
    /// <summary>
    /// Fake ISession implementation for unit testing (Lab 3)
    /// </summary>
    public class FakeSession : ISession
    {
        private readonly Dictionary<string, byte[]> _store = new();

        public bool IsAvailable => true;
        public string Id => Guid.NewGuid().ToString();
        public IEnumerable<string> Keys => _store.Keys;

        public void Clear() => _store.Clear();

        public Task CommitAsync(CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task LoadAsync(CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public void Remove(string key) => _store.Remove(key);

        public void Set(string key, byte[] value)
        {
            if (_store.ContainsKey(key))
                _store[key] = value;
            else
                _store.Add(key, value);
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            if (_store.ContainsKey(key))
            {
                value = _store[key];
                return true;
            }
            value = Array.Empty<byte>();
            return false;
        }
    }
}
