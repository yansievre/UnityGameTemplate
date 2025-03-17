using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility
{
    public class LazyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, ICollection where TValue : new()
    {
        private class Entry
        {
            public Func<TValue> generator;
            public TValue cachedValue;

            public TValue GetValue() => cachedValue == null ? cachedValue = generator() : cachedValue;
        }
        private readonly Func<TKey, TValue> _defaultGenerator;
        private readonly Dictionary<TKey, Entry> _generatorDictionary;

        public LazyDictionary(Func<TKey, TValue> defaultGenerator)
        {
            _generatorDictionary = new Dictionary<TKey, Entry>();
            _defaultGenerator = defaultGenerator;
        }
        
        public LazyDictionary()
        {
            _generatorDictionary = new Dictionary<TKey, Entry>();
            _defaultGenerator = _ => new TValue();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _generatorDictionary.Select(x => new KeyValuePair<TKey,TValue>(x.Key, x.Value.GetValue())).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _generatorDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _generatorDictionary.ContainsKey(item.Key) && _generatorDictionary[item.Key].GetValue().Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (_generatorDictionary.ContainsKey(item.Key))
            {
                var entry = _generatorDictionary[item.Key];

                if (entry.GetValue().Equals(item.Value))
                {
                    _generatorDictionary.Remove(item.Key);

                    return true;
                }

                return false;
            }

            return false;
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count => _generatorDictionary.Count;
        public bool IsReadOnly => throw new NotImplementedException();
        public bool IsSynchronized => ((ICollection) _generatorDictionary).IsSynchronized;

        public object SyncRoot => ((ICollection) _generatorDictionary).SyncRoot;

        public void Add(TKey key, TValue value)
        {
            if (_generatorDictionary.TryGetValue(key, out var entry))
            {
                entry.cachedValue = value;
            }
            else
            {
                _generatorDictionary.Add(key, new Entry{cachedValue = value});
            }
        }
        
        public void Add(TKey key, Func<TValue> overrideGenerator = null)
        {
            if (_generatorDictionary.ContainsKey(key))
            {
                Debug.LogError($"Given key is already present: {key?.ToString()}");

                return;
            }

            var entry = new Entry()
            {
                generator = overrideGenerator ?? (() => _defaultGenerator(key)),
            };
            _generatorDictionary.Add(key, entry);
        }

        public bool ContainsKey(TKey key)
        {
            return true;
        }

        public bool Remove(TKey key)
        {
            return _generatorDictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var result = _generatorDictionary.TryGetValue(key, out var val);
            if (!result)
                Add(key, value = _defaultGenerator(key));
            else
                value = val.GetValue();
            return true;
        }

        public bool TryGetRegisteredValue(TKey key, out TValue value)
        {
            var result = _generatorDictionary.TryGetValue(key, out var val);
            value = default;

            if (!result)
                return false;
            value = val.GetValue();
            return true;
        }
        
        public TValue this[TKey key]
        {
            get => _generatorDictionary.TryGetValue(key, out var entry) ? entry.GetValue() : default;
            set => _generatorDictionary[key].cachedValue = value;
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        public ICollection<TKey> Keys => _generatorDictionary.Keys;

        public ICollection<TValue> Values => _generatorDictionary.Values.Select(x => x.GetValue()).ToList();
    }
}