using System.Collections.Generic;
using System.Linq;

namespace Sapo.DI.Runtime.Common
{
    internal static class EnumerableExtensions
    {
        internal static bool IsEmpty<T>(this IEnumerable<T> enumerable) => !enumerable.Any();
    }
}