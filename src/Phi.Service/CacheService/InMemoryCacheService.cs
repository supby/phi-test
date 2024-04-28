using System.Collections.Concurrent;

namespace Phi.Service;

// NOTE: in-mem is used only for test purposes as it is not scalable
// in real scenarious some external cache server should be used (redis and so on)
public class InMemoryCacheService<K, V> : ICacheService<K, V> where K: notnull
{
    private readonly ConcurrentDictionary<K, V> _inMemCache;

    public InMemoryCacheService()
    {
        _inMemCache = new ConcurrentDictionary<K,V>();
    }

    public V Add(K key, V value)
    {
        return _inMemCache.AddOrUpdate(key, value, (newVal, existingVal) => value);
    }

    public V? GetByKey(K key)
    {
        _inMemCache.TryGetValue(key, out V? ret);

        return ret;
    }
}
