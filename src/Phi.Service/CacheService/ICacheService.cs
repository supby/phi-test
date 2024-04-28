namespace Phi.Service;

public interface ICacheService<K,V> where K: notnull
{
    V? GetByKey(K key);
    V Add(K key, V value);
}
