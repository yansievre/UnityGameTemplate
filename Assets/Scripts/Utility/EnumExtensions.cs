using System;
using System.Runtime.CompilerServices;

namespace Utility
{
    public static class EnumExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckFlag<TEnum>(this TEnum value, TEnum flag) where TEnum : unmanaged, Enum
        {
            return (Convert.ToInt32(value) & Convert.ToInt32(flag)) != 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckFlag(this Int32 value, Int32 flag)
        {
            return (value & flag) != flag;
        }
       
    }
}