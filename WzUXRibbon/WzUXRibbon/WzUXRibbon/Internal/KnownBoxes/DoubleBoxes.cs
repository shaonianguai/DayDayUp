using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzUXRibbon.Internal.KnownBoxes
{
    internal static class DoubleBoxes
    {
        /// <summary>
        /// Gets a boxed value for <c>0D</c>.
        /// </summary>
        internal static readonly object Zero = 0D;

        /// <summary>
        /// Gets a boxed value for <see cref="double.NaN"/>.
        /// </summary>
        internal static readonly object NaN = double.NaN;

        /// <summary>
        /// Gets a boxed value for <see cref="double.MaxValue"/>.
        /// </summary>
        internal static readonly object MaxValue = double.MaxValue;

        /// <summary>
        /// Gets a boxed value for <c>1D</c>.
        /// </summary>
        internal static readonly object One = 1D;
    }
}
