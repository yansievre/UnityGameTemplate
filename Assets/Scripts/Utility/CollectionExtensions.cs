using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;

namespace Utility
{
    public interface IResettable
    {
        void Reset();
    }
    public static class CollectionExtensions
    {
        public static T IntoCollection<T>(this T item, ICollection<T> collection)
        {
            collection.Add(item);

            return item;
        }
    
        public static void AddRange<T>(this List<T> list, params T[] items)
        {
            list.AddRange(items);
        }
        
        
        /// <summary>
        /// Does not force execution
        /// </summary>
        /// <param name="enumeration"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
                yield return item;
            }
        }
        
        /// <summary>
        /// Does not force execution
        /// </summary>
        /// <param name="enumeration"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumeration, Action<T, int> action)
        {
            int num = 0;
            foreach (T item in enumeration)
            {
                action(item, num++);
                yield return item;
            }
        }
        
        public static IEnumerable<T> WhereTrue<T>(this IEnumerable<T> enumeration)
        {
            return enumeration.Where(x => Convert.ToBoolean(x));
        }
        
        public static T FirstOrValue<T>(this IEnumerable<T> enumeration, T value)
        {
            foreach (T item in enumeration)
            {
                return item;
            }

            return value;
        }

        public static (T, T) MinMax<T>(this IEnumerable<T> enumeration) where T : IComparable
        {
            T min = default;
            T max = default;
            bool first = true;
            foreach (T item in enumeration)
            {
                if (first)
                {
                    min = item;
                    max = item;
                    first = false;
                }
                else
                {
                    if (item.CompareTo(min) < 0)
                        min = item;
                    if (item.CompareTo(max) > 0)
                        max = item;
                }
            }

            return (min, max);
        }
        
        public static (TSource, TSource) MinMax<T, TSource>(this IEnumerable<TSource> enumeration, Func<TSource, T> getter) where T : IComparable
        {
            TSource min = default;
            T minVal = default;
            TSource max = default;
            T maxVal = default;
            bool first = true;
            foreach (TSource item in enumeration)
            {
                if (first)
                {
                    min = item;
                    minVal = getter(item);
                    max = item;
                    maxVal = getter(item);
                    first = false;
                }
                else
                {
                    if (getter(item).CompareTo(minVal) < 0)
                    {
                        min = item;
                        minVal = getter(item);
                    }
                    if (getter(item).CompareTo(maxVal) > 0)
                    {
                        max = item;
                        maxVal = getter(item);
                    }
                }
            }

            return (min, max);
        }

        public static IEnumerable<TValue> Enumerate<TValue>(this NativeQueue<TValue> nativeQueue) where TValue : unmanaged
        {
            while (nativeQueue.TryDequeue(out var value))
            {
                yield return value;
            }
        }

        public delegate bool SelectWhereDelegate<TElement, TSelect>(TElement value, out TSelect selected);
        public delegate bool EqualityDelegate<TElement>(TElement value1, TElement value2);
        public delegate uint HashDelegate<TElement>(TElement value);
        /// <summary>
        /// Does not force execution
        /// </summary>
        /// <param name="enumeration"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TSelect> SelectWhere<TElement, TSelect>(this IEnumerable<TElement> enumeration, SelectWhereDelegate<TElement, TSelect> selector)
        {
            foreach (TElement item in enumeration)
            {
                if(selector(item, out var selected))
                    yield return selected;
            }
        }
        
        public static void ResetEach<TResettable>(this IEnumerable<TResettable> enumeration) where TResettable : IResettable
        {
            foreach (var item in enumeration.ToList())
            {
                item.Reset();
            }
        }

        public static IEnumerable<TValue> Distinct<TValue>(this IEnumerable<TValue> enumeration, HashDelegate<TValue> hash)
        {
            var set = new HashSet<uint>();

            foreach (var value in enumeration)
            {
                if (set.Add(hash(value)))
                    yield return value;
            }
        }
        
        
        /// <summary>
        /// Does not force execution
        /// </summary>
        /// <param name="enumeration"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<(T val, int index)> WithIndex<T>(this IEnumerable<T> enumeration)
        {
            int num = 0;
            foreach (T item in enumeration)
            {
                yield return (item, num++);
            }
        }

        /// <summary>
        /// Does not force execution
        /// </summary>
        /// <param name="enumeration"></param>
        /// <param name="offset"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<(T val, int index)> WithIndexOffset<T>(this IEnumerable<T> enumeration, int offset)
        {
            int num = 0;
            foreach (T item in enumeration)
            {
                yield return (item, offset+num++);
            }
        }

        /// <summary>
        /// Forces execution
        /// </summary>
        /// <param name="enumeration"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> enumeration, T value)
        {
            return IndexOf<T>(enumeration, value, EqualityComparer<T>.Default.Equals);
        }

        /// <summary>
        /// Forces execution
        /// </summary>
        /// <param name="enumeration"></param>
        /// <param name="value"></param>
        /// <param name="equality"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> enumeration, T value, EqualityDelegate<T> equality)
        {
            int num = 0;
            foreach (T item in enumeration)
            {
                if(equality(item, value))
                    return num;
                num++;
            }

            return -1;
        }

        /// <summary>
        /// Forces execution
        /// </summary>
        /// <param name="enumeration"></param>
        /// <param name="value"></param>
        /// <param name="hash"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> enumeration, T value, HashDelegate<T> hash)
        {
            var valueHash = hash(value);
            int num = 0;
            foreach (T item in enumeration)
            {
                if(valueHash == hash(item))
                    return num;
                num++;
            }

            return -1;
        }
    }
}