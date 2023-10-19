using System;
using WzUXRibbon.Enumerations;

namespace WzUXRibbon.Internal.KnownBoxes
{
    public static class IconSizeBoxes
    {
        /// <summary>
        /// Gets a boxed value for <see cref="IconSize.Small"/>.
        /// </summary>
        public static readonly object Small = IconSize.Small;

        /// <summary>
        /// Gets a boxed value for <see cref="IconSize.Medium"/>.
        /// </summary>
        public static readonly object Medium = IconSize.Medium;

        /// <summary>
        /// Gets a boxed value for <see cref="IconSize.Large"/>.
        /// </summary>
        public static readonly object Large = IconSize.Large;

        /// <summary>
        /// Gets a boxed value for <see cref="IconSize.Custom"/>.
        /// </summary>
        public static readonly object Custom = IconSize.Custom;

        /// <summary>
        /// Gets a boxed value for <see cref="IconSize"/>.
        /// </summary>
        public static object Box(IconSize iconSize)
        {
            switch (iconSize)
            {
                case IconSize.Small:
                    return Small;
                case IconSize.Medium:
                    return Medium;
                case IconSize.Large:
                    return Large;
                default:
                    throw new ArgumentOutOfRangeException(nameof(iconSize), iconSize, null);
            }
        }
    }
}
