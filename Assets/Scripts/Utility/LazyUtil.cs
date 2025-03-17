using System;
using System.Collections.Generic;

namespace Utility
{
    public static class LazyUtil
    {
        private static Dictionary<int, object> S_GlobalLazyCache = new Dictionary<int, object>();
        
        public static TValue Lazy<TCaller, TValue>(this TCaller caller, Func<TValue> factory)
        {
            var hash = caller.GetHashCode();
                
            if (S_GlobalLazyCache.TryGetValue(caller.GetHashCode(), out var value))
                return (TValue) value;
            var val = factory();
            S_GlobalLazyCache.Add(hash, val);
            return val;
        }
    }
}