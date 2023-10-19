using System.Text;
using System.Windows.Input;
using WzUXRibbon.Utils;

namespace WzUXRibbon.Internal
{
    internal static class KeyEventUtility
    {
        public static unsafe string GetStringFromKey(Key key)
        {
            var keyboardState = new byte[256];
            if (NativeMethods.GetKeyboardState(keyboardState) == false)
            {
                return null;
            }

            var virtualKey = KeyInterop.VirtualKeyFromKey(key);

            var scanCode = NativeMethods.MapVirtualKeyEx((uint)virtualKey, NativeMethods.MapVirtualKeyMapTypes.MAPVK_VK_TO_VSC, System.IntPtr.Zero);
            var stringBuilder = new StringBuilder(256);
            var result = NativeMethods.ToUnicodeEx((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Length, 1, System.IntPtr.Zero);
            switch (result)
            {
                case 0:
                case -1:
                    return null;
                case 1:
                    return stringBuilder.ToString();
                default:
                    return null;
            }
        }
    }
}
