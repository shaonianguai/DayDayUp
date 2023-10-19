using System.Collections;

namespace WzUXRibbon.Extensions
{
    internal static class IListExtensions
    {
        private static readonly IList emptyReadOnlyList = ArrayList.ReadOnly(new ArrayList());

        public static IList NullSafe(this IList list)
        {
            return list ?? emptyReadOnlyList;
        }
    }
}
