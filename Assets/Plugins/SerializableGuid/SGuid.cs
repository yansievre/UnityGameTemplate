using System;
using UnityEngine;

namespace Plugins.SerializableGuid
{
    [Serializable]
    public struct SGuid :  IEquatable<SGuid>
    {
        [SerializeField]
        private int _a;
        [SerializeField]
        private short _b;
        [SerializeField]
        private short _c;
        [SerializeField]
        private byte _d;
        [SerializeField]
        private byte _e;
        [SerializeField]
        private byte _f;
        [SerializeField]
        private byte _g;
        [SerializeField]
        private byte _h;
        [SerializeField]
        private byte _i;
        [SerializeField]
        private byte _j;
        [SerializeField]
        private byte _k;

        //Cast to Guid
        public static implicit operator Guid(SGuid sGuid)
        {
            byte[] byteArray = new byte[16];
            byteArray[0] = (byte) sGuid._a;
            byteArray[1] = (byte) (sGuid._a >> 8);
            byteArray[2] = (byte) (sGuid._a >> 16);
            byteArray[3] = (byte) (sGuid._a >> 24);
            byteArray[4] = (byte) sGuid._b;
            byteArray[5] = (byte) (sGuid._b >> 8);
            byteArray[6] = (byte) sGuid._c;
            byteArray[7] = (byte) (sGuid._c >> 8);
            byteArray[8] = sGuid._d;
            byteArray[9] = sGuid._e;
            byteArray[10] = sGuid._f;
            byteArray[11] = sGuid._g;
            byteArray[12] = sGuid._h;
            byteArray[13] = sGuid._i;
            byteArray[14] = sGuid._j;
            byteArray[15] = sGuid._k;
            return new Guid(byteArray);
        }
        //Cast from Guid
        public static implicit operator SGuid(Guid guid)
        {
            byte[] byteArray = guid.ToByteArray();
            return new SGuid
            {
                _a = byteArray[0] | byteArray[1] << 8 | byteArray[2] << 16 | byteArray[3] << 24,
                _b = (short) (byteArray[4] | byteArray[5] << 8),
                _c = (short) (byteArray[6] | byteArray[7] << 8),
                _d = byteArray[8],
                _e = byteArray[9],
                _f = byteArray[10],
                _g = byteArray[11],
                _h = byteArray[12],
                _i = byteArray[13],
                _j = byteArray[14],
                _k = byteArray[15]
            };
        }


        
        //equality operators
        public static bool operator ==(SGuid a, SGuid b)
        {
            return a._a == b._a && a._b == b._b && a._c == b._c && a._d == b._d && a._e == b._e && a._f == b._f && a._g == b._g && a._h == b._h && a._i == b._i && a._j == b._j && a._k == b._k;
        }

        public static bool operator !=(SGuid a, SGuid b)
        {
            return !(a == b);
        }
        
        public override bool Equals(object obj)
        {
            return (obj is SGuid other && this == other) || (obj is Guid otherGuid && this == otherGuid);
        }

        public bool Equals(SGuid other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return _a ^ (_b << 16 | (ushort) _c) ^ (_f << 24 | _k);
        }

        public void Regenerate()
        {
            this = Guid.NewGuid();
        }
        
        //equality operators
        public static bool operator ==(SGuid a, Guid b)
        {
            return a == (SGuid) b;
        }
        //equality operators
        public static bool operator ==(Guid a, SGuid b)
        {
            return (SGuid) a ==  b;
        }

        public static bool operator !=(Guid a, SGuid b)
        {
            return !(a == b);
        }

        public static bool operator !=(SGuid a, Guid b)
        {
            return !(a == b);
        }

    }
}