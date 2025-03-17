using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace Utility
{
    [BurstCompile]
    public static class CMath
    {
        public const float K_Epsilon = 0.001f;

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 OnGroundPlane(this float3 vector)
        {
            return new float2(vector.x, vector.z);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 OnGroundPlane(this Vector3 vector)
        {
            return new float2(vector.x, vector.z);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 FromGroundPlane(this float2 vector)
        {
            return new float3(vector.x, 0, vector.y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 FromGroundPlane(this Vector2 vector)
        {
            return new float3(vector.x, 0, vector.y);
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AccelerateTowards(this float current, float target, float accelRate, float time)
        {
            if (math.abs(current - target) < K_Epsilon)
                return target;
            var direction = math.sign(target - current);
            var newVal = current + direction * accelRate * time;
            var min = math.min(current, target);
            var max = math.max(current, target);
            return math.clamp(newVal, min, max);
        }
    
        public static Vector2 AccelerateTowards(this Vector2 current, Vector2 target, float accelRate, float time)
        {
            return ((Vector3)current).AccelerateTowards(target, accelRate, time);
        }

        public static void ShiftArray<T>(ref T[] array, int startIndex)
        {
            startIndex %= array.Length;

            if (startIndex == 0)
                return;
            var cache = ArrayPool<T>.Shared.Rent(array.Length);
            Array.Copy(array, cache, array.Length);
            Array.Copy(cache, startIndex, array, 0, array.Length - startIndex);
            Array.Copy(cache, 0, array, array.Length - startIndex, startIndex);
        }

        public static IEnumerable<(T first, T second)> Pairwise<T>(this IEnumerable<T> enumerable, bool loop = false)
        {
            T first = default;
            T previous;
            T current = default;
            int i = 0;

            foreach (var trg in enumerable)
            {
                previous = current;
                current = trg;

                if (i == 0)
                    first = current;
                
                
                if (previous != null && i != 0)
                    yield return (previous, current);
                i++;
            }

            if (loop)
                yield return (current, first);
        }
        public static IEnumerable<(T first, K second)> SideBySide<T, K>(this IEnumerable<T> enumerable, IEnumerable<K> other)
        {
            var enumerator1 = enumerable.GetEnumerator();
            var enumerator2 = other.GetEnumerator();

            while (true)
            {
                if (!enumerator1.MoveNext())
                    break;
                if (!enumerator2.MoveNext())
                    break;

                yield return (enumerator1.Current, enumerator2.Current);
            }
            enumerator1.Dispose();
            enumerator2.Dispose();
        }
        public static IEnumerable<T> EndWithFirstElement<T>(this IEnumerable<T> enumerable)
        {
            var enumerator1 = enumerable.GetEnumerator();
            bool hasFirst = false;
            T first = default;

            while (enumerator1.MoveNext())
            {
                if (!hasFirst)
                {
                    first = enumerator1.Current;
                    hasFirst = true;
                }
                yield return enumerator1.Current;
            }
            if(hasFirst)
                yield return first;
            
            enumerator1.Dispose();
        }
        public static IEnumerable<Vector2> GetCorners(this Rect rect)
        {
            yield return rect.min;
            yield return rect.max - Vector2.up*rect.height;
            yield return rect.max;
            yield return rect.min + Vector2.up*rect.height;
        }
        

        
        public static float2 Slerp(this float2 current, float2 target, float time)
        {
            var magnitudeLerp = math.sqrt(math.lerp(math.lengthsq(current), math.lengthsq(target), time));
            var angleLerp = Vector2.SignedAngle(current, target)*time;
            return math.mul(quaternion.Euler(0, 0, angleLerp), new float3(math.normalize(current), 0)).xy * magnitudeLerp;
        }
 
        public static Vector2 Slerp(this Vector2 current, Vector2 target, float time)
        {
            var magnitudeLerp = Mathf.Lerp(current.magnitude, target.magnitude, time);
            var angleLerp = Vector2.SignedAngle(current, target)*time;
            return Quaternion.Euler(0, 0, angleLerp) * current.normalized * magnitudeLerp;
        }

        public static Vector2 TangentToNormal(this Vector2 tangent)
        {
            return -NormalToTangent(tangent);
        }
        
        public static Vector2 NormalToTangent(this Vector2 normal)
        { 
            return new Vector2(-normal.y, normal.x);
        }
    
        public static Vector3 AccelerateTowards(this Vector3 current, Vector3 target, float accelRate, float time)
        {
            var diff = target - current;
            var direction = diff.normalized;
            var delta = direction * accelRate * time;
            if (delta.magnitude > diff.magnitude)
                return target;
            var newVal = current + delta;
            return newVal;
        }

        public static bool InRange(this Vector2 range, float value, bool minInclusive = true, bool maxInclusive = true)
        {
            return (minInclusive ? value >= range.x : value > range.x) 
                   && (maxInclusive ? value <= range.y : value < range.y);
        }
        public static bool InRange(this Vector2Int range, int value, bool minInclusive = true, bool maxInclusive = true)
        {
            return InRange(range, (float) value, minInclusive, maxInclusive);
        }

        public static bool InRangeInclusive(this float value, float min, float max)
        {
            return value >= min && value <= max;
        }
        
        
        public static int ToMilliseconds(this float seconds) => (int) (seconds * 1000);
    }
}