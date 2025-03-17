using System;
using System.Collections.Generic;

namespace Utility
{
    public static class EditorCache
    {
#if UNITY_EDITOR
        private static Dictionary<string, object> _cache = new Dictionary<string, object>();

        public static T Get<T>(string key)
        {
            return _cache.TryGetValue(key, out var value) ? (T)(Convert.ChangeType(value, typeof(T))) : Activator.CreateInstance<T>();
        }

        public static void Save(string key, object obj) => _cache[key] = obj;
        
#endif
    }
}