using System;
using System.Collections.Generic;
using System.Linq;

namespace HoneyComb.Core.Linq
{
    public static class IEnumerableForEachImmutable
    {
        /// <summary>
        /// Performs the specified action on each element of the <see cref="IEnumerable{T}"/>.
        /// Use it if: An element in the collection has been modified should not throw Exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="action">The System.Action`1 delegate to perform on each element of the System.Collections.Generic.List`1.</param>
        public static void ForeachImmutable<T>(this IEnumerable<T> @this, Action<T> action) =>
            @this.ToList().ForEach(action);
    }
}
