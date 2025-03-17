using System;
using System.Collections.Generic;

namespace Utility.GuidCompare
{
    public class CustomEqualityComparer<T> : EqualityComparer<T>
    {
        public static EqualityComparer<T> Create(Func<T, int> getHashCode, Func<T, T, bool> equals)
        {
            return new CustomEqualityComparer<T>(getHashCode, equals);
        }
        
        public static EqualityComparer<T> Create(Func<T, int> getHashCode)
        {
            return new CustomEqualityComparer<T>(getHashCode);
        }
        
        private readonly Func<T, int> _getHashCode;
        private readonly Func<T, T, bool> _equals;

        public CustomEqualityComparer(Func<T, int> getHashCode, Func<T, T, bool> equals)
        {
            _getHashCode = getHashCode;
            _equals = equals;
        }
        
        public CustomEqualityComparer(Func<T, int> getHashCode)
        {
            _getHashCode = getHashCode;
            _equals = HashCodeEquals;
        }
        
        public override bool Equals(T x, T y)
        {
            return _equals(x, y);
        }

        public override int GetHashCode(T obj)
        {
            return _getHashCode(obj);
        }

        private bool HashCodeEquals(T x, T y)
        {
            return _getHashCode(x) == _getHashCode(y);
        }
    }
}