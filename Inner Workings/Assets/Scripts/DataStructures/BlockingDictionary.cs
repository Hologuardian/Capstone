using System.Collections.Generic;
using UnityEngine;

public class BlockingDictionary<K, V>
{
    private object syncLock = new object();
    private Dictionary<K, V> dict = new Dictionary<K, V>();

    public V this[K i]
    {
        get
        {
            lock(syncLock)
            {
                return dict[i];
            }
        }
        set
        {
            lock (syncLock)
            {
                dict[i] = value;
            }
        }
    }

    public int Count
    {
        get
        {
            return dict.Count;
        }
    }

    public Dictionary<K,V>.KeyCollection Keys
    {
        get
        {
            lock (syncLock)
            {
                return dict.Keys;
            }
        }
    }

    public bool ContainsKey(K key)
    {
        lock (syncLock)
        {
            return dict.ContainsKey(key);
        }
    }

    public void Add(K key, V value)
    {
        lock (syncLock)
        {
            dict.Add(key, value);
        }
    }

    public bool Remove(K key)
    {
        lock (syncLock)
        {
            return dict.Remove(key);
        }
    }

    public void Clear()
    {
        lock (syncLock)
        {
            dict.Clear();
        }
    }
}
