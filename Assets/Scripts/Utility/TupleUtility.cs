using System;

namespace Utility
{
    public static class TupleUtility
    {
        public static (T1, T2) Swap<T1, T2>(this (T2, T1) tuple) => (tuple.Item2, tuple.Item1);

        public static (TValue, TValue) Select<TSource, TValue>(this (TSource, TSource) tuple,
            Func<TSource, TValue> selector) => (selector(tuple.Item1), selector(tuple.Item2));
    }
}