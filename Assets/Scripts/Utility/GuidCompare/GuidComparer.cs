using System.Collections.Generic;

namespace Utility.GuidCompare
{

    public sealed class GuidEqualityComparer<TValue> : IEqualityComparer<TValue> where TValue : IGuidOwner
    {
        public static IEqualityComparer<TValue> GuidComparer { get; } = new GuidEqualityComparer<TValue>();
        private IEqualityComparer<IGuidOwner> _delegateComparer = GuidEqualityComparer.GuidComparer;

        public bool Equals(TValue x, TValue y)
        {
            return _delegateComparer.Equals(x, y);
        }

        public int GetHashCode(TValue obj)
        {
            return _delegateComparer.GetHashCode(obj);
        }
    }

    public sealed class GuidEqualityComparer : IEqualityComparer<IGuidOwner>
    {
        public static IEqualityComparer<IGuidOwner> GuidComparer { get; } = new GuidEqualityComparer();
        
        public bool Equals(IGuidOwner x, IGuidOwner y)
        {
            if (x == null || y == null)
                return false;
            return x.Guid == y.Guid;
        }

        public int GetHashCode(IGuidOwner obj)
        {
            return obj.Guid.GetHashCode();
        }
    }

}