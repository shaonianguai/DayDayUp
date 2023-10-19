using System;
using System.Windows;

namespace WzUXRibbon.Internal.KnownBoxes
{
    internal static class VisibilityBoxes
    {
        /// <summary>
        /// Gets a boxed value for <see cref="Visibility.Visible"/>.
        /// </summary>
        internal static readonly object Visible = Visibility.Visible;

        /// <summary>
        /// Gets a boxed value for <see cref="Visibility.Hidden"/>.
        /// </summary>
        internal static readonly object Hidden = Visibility.Hidden;

        /// <summary>
        /// Gets a boxed value for <see cref="Visibility.Collapsed"/>.
        /// </summary>
        internal static readonly object Collapsed = Visibility.Collapsed;

        /// <summary>
        /// Gets a boxed value for <paramref name="value"/>.
        /// </summary>
        /// <returns>A boxed <see cref="Visibility"/> value.</returns>
        internal static object Box(Visibility value)
        {
            switch (value)
            {
                case Visibility.Visible:
                    return Visible;

                case Visibility.Hidden:
                    return Hidden;

                case Visibility.Collapsed:
                    return Collapsed;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}
