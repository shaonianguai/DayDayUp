using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzUXRibbon.Internal.KnownBoxes
{
    internal static class BooleanBoxes
    {
        /// <summary>
        /// Gets a boxed value for <c>true</c>.
        /// </summary>
        internal static readonly object TrueBox = true;

        /// <summary>
        /// Gets a boxed value for <c>true</c>.
        /// </summary>
        internal static readonly object FalseBox = false;

        /// <summary>
        /// Gets a boxed value for <paramref name="value"/>.
        /// </summary>
        /// <returns>A boxed <see cref="bool"/> value.</returns>
        internal static object Box(bool value)
        {
            return value
                ? TrueBox
                : FalseBox;
        }

        /// <summary>
        /// Gets a boxed value for <paramref name="value"/>.
        /// </summary>
        /// <returns>A boxed nullable <see cref="bool"/> value.</returns>
        internal static object Box(bool? value)
        {
            if (value.HasValue)
            {
                return value.Value
                    ? TrueBox
                    : FalseBox;
            }

            return null;
        }
    }
}
