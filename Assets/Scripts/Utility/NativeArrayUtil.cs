using Unity.Collections;

namespace Utility
{
    public static class NativeArrayUtil
    {
        public static NativeArray<T> Resize<T>(this NativeArray<T> array, int newCount, Allocator allocator) where T : unmanaged
        {
            var newArray = new NativeArray<T>(newCount, allocator);
            NativeArray<T>.Copy(array,0,newArray,0, array.Length);
            array.Dispose();
            return newArray;
        }        
        
        public static int GetFirstBiggerOrEqPowerOfTwo(this int biggerOrEqualTo)
        {
            int powerOfTwo = 1;
            while (powerOfTwo < biggerOrEqualTo)
            {
                powerOfTwo *= 2;
            }
            return powerOfTwo;
        }
    }
}