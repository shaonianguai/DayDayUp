using System;
using WzUXRibbon.Enumerations;

namespace WzUXRibbon.Internal.KnownBoxes
{
    public static class IconSizeBoxes
    {
        public static readonly object Small = IconSize.Small;
        public static readonly object Medium = IconSize.Medium;
        public static readonly object Large = IconSize.Large;
        public static readonly object Custom = IconSize.Custom;

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
