using System.Runtime.InteropServices;
using System;
using System.Text;

namespace WzUXRibbon.Utils
{
    static partial class NativeMethods
    {
        public const uint WM_ACTIVATE = 0x0006;
        public const uint WM_SIZE = 0x0005;
        public const uint WM_DESTROY = 0x0002;
        public const uint WM_QUIT = 0x0012;
        public const uint WM_NCACTIVATE = 0x0086;
        public const uint WM_LBUTTONDOWN = 0x0201;
        public const uint WM_NCLBUTTONDOWN = 0x00A1;
        public const uint WM_MBUTTONDBLCLK = 0x0209;
        public const uint WM_NCXBUTTONDBLCLK = 0x00AD;

        public enum MapVirtualKeyMapTypes : uint
        {
            MAPVK_VK_TO_VSC = 0x00,
            MAPVK_VSC_TO_VK = 0x01,
            MAPVK_VK_TO_CHAR = 0x02,
            MAPVK_VSC_TO_VK_EX = 0x03,
            MAPVK_VK_TO_VSC_EX = 0x04
        }

        public struct MONITOR_FROM_FLAGS
        {
            public const uint MONITOR_MONITOR_DEFAULTTONULL = 0x00000000;
            public const uint MONITOR_MONITOR_DEFAULTTOPRIMARY = 0x00000001;
            public const uint MONITOR_DEFAULTTONEAREST = 0x00000002;
        }

        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public struct MONITORINFO
        {
            public uint cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr MonitorFromRect([In] ref RECT lprc, uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, [In, Out] ref MONITORINFO lpmi);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, StringBuilder lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKeyEx(uint uCode, MapVirtualKeyMapTypes uMapType, IntPtr dwhkl);

        [DllImport("user32.dll")]
        public static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);
    }
}
