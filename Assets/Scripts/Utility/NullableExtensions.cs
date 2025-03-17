#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public static class NullableExtensions
    {
        public static TResult SelectOr<TInput, TResult>(this TInput? input, Func<TInput, TResult> selector, TResult defaultValue) where TInput : struct
        {
            return input.HasValue ? selector(input.Value) : defaultValue;
        }
        
        public static IEnumerable<TInput> ToEnumerable<TInput>(this TInput? input) where TInput : struct
        {
            return input.SelectOr(x => new[] {x}, Enumerable.Empty<TInput>());
        }
    }
}