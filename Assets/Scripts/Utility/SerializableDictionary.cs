using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private List<TKey> _keyData = new();
	
        [SerializeField, HideInInspector]
        private List<TValue> _valueData = new();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Clear();
            for (int i = 0; i < _keyData.Count && i < _valueData.Count; i++)
            {
                this[_keyData[i]] = _valueData[i];
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _keyData.Clear();
            _valueData.Clear();

            foreach (var item in this)
            {
                _keyData.Add(item.Key);
                _valueData.Add(item.Value);
            }
        }
    }
    
    public static class SerializableDictionaryExtensions
    {
        public static SerializableDictionary<TKey, TValue> Create<TKey, TValue>()
        {
            return new SerializableDictionary<TKey, TValue>();
        }
        
        public static SerializableDictionary<TKey, TValue> ToSerializableDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var serializableDictionary = new SerializableDictionary<TKey, TValue>();
            foreach (var (key, value) in dictionary)
            {
                serializableDictionary[key] = value;
            }

            return serializableDictionary;
        }
        
        //to serializable dictionary from enumerable
        public static SerializableDictionary<TKey, TValue> ToSerializableDictionary<TKey, TValue>(this IEnumerable<TValue> enumerable, Func<TValue, TKey> keySelector)
        {
            var serializableDictionary = new SerializableDictionary<TKey, TValue>();
            foreach (var value in enumerable)
            {
                serializableDictionary[keySelector(value)] = value;
            }

            return serializableDictionary;
        }
        
        //to serializable dictionary from enumerable
        public static SerializableDictionary<TKey, TValue> ToSerializableDictionary<TKey, TValue, TObject>(this IEnumerable<TObject> enumerable, Func<TObject, TKey> keySelector, Func<TObject, TValue> valueSelector)
        {
            var serializableDictionary = new SerializableDictionary<TKey, TValue>();
            foreach (var value in enumerable)
            {
                serializableDictionary[keySelector(value)] = valueSelector(value);
            }

            return serializableDictionary;
        }
    
    }
}