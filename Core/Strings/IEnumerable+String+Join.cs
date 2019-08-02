using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Strings
{
    /// <summary>
    /// https://stackoverflow.com/questions/217805/using-linq-to-concatenate-strings
    /// </summary>
    public static class IEnumerableStringJoin
    {
        public static string Join(this IEnumerable<string> @this, string separator) => string.Join(separator, @this);
    }
}
