namespace WzUXRibbon.Internal.KnownBoxes
{
    internal static class BooleanBoxes
    {
        internal static readonly object TrueBox = true;
        internal static readonly object FalseBox = false;

        internal static object Box(bool value)
        {
            return value
                ? TrueBox
                : FalseBox;
        }

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
