using System;
using System.Collections.Generic;

namespace HoneyComb.Core.Collections
{
    public static class ListExtensions
    {
        /// <summary>
        ///     
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="item"></param>
        /// <remarks>
        ///     Credits to https://stackoverflow.com/questions/12172162/how-to-insert-item-into-list-in-order
        /// </remarks>
        public static int GetIndexForAddSorted<T>(this List<T> @this, T item, IComparer<T> comparer)
        {
            if (@this.Count is 0)
                return @this.Count;
            if (comparer.Compare(@this[@this.Count - 1], item) <= 0)
                return @this.Count;
            if (comparer.Compare(@this[0], item) >= 0)
                return 0;
            int index = @this.BinarySearch(item, comparer);
            if (index < 0)
                index = ~index;
            return index;
        }
    }
}
