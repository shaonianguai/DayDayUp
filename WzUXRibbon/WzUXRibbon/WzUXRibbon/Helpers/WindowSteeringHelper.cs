﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using WzUXRibbon.Utils;

namespace WzUXRibbon.Helpers
{
    public static class WindowSteeringHelper
    {
        private static readonly PropertyInfo criticalHandlePropertyInfo = typeof(Window).GetProperty("CriticalHandle", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly object[] emptyObjectArray = Array.Empty<object>();

        /// <summary>
        /// Shows the system menu at the current mouse position.
        /// </summary>
        /// <param name="e">The mouse event args.</param>
        /// <param name="handleDragMove">Defines if window dragging should be handled.</param>
        /// <param name="handleStateChange">Defines if window state changes should be handled.</param>
        public static void HandleMouseLeftButtonDown(MouseButtonEventArgs e, bool handleDragMove, bool handleStateChange)
        {
            if (e.Source is DependencyObject dpo)
            {
                HandleMouseLeftButtonDown(dpo, e, handleDragMove, handleStateChange);
            }
        }

#pragma warning disable 618
        /// <summary>
        /// Shows the system menu at the current mouse position.
        /// </summary>
        /// <param name="dependencyObject">The object which was the source of the mouse event.</param>
        /// <param name="e">The mouse event args.</param>
        /// <param name="handleDragMove">Defines if window dragging should be handled.</param>
        /// <param name="handleStateChange">Defines if window state changes should be handled.</param>
        public static void HandleMouseLeftButtonDown(DependencyObject dependencyObject, MouseButtonEventArgs e, bool handleDragMove, bool handleStateChange)
        {
            var window = Window.GetWindow(dependencyObject);

            if (window is null)
            {
                return;
            }

            if (handleDragMove
                && e.ClickCount == 1)
            {
                e.Handled = true;

                // taken from DragMove internal code
                window.VerifyAccess();

                // for the touch usage
                NativeMethods.ReleaseCapture();

                var criticalHandle = (IntPtr)(criticalHandlePropertyInfo?.GetValue(window, emptyObjectArray) ?? IntPtr.Zero);

                if (criticalHandle != IntPtr.Zero)
                {
                    // these lines are from DragMove
                    // NativeMethods.SendMessage(criticalHandle, WM.SYSCOMMAND, (IntPtr)SC.MOUSEMOVE, IntPtr.Zero);
                    // NativeMethods.SendMessage(criticalHandle, WM.LBUTTONUP, IntPtr.Zero, IntPtr.Zero);

                    var wpfPoint = window.PointToScreen(Mouse.GetPosition(window));
                    var x = (int)wpfPoint.X;
                    var y = (int)wpfPoint.Y;
                    StringBuilder builder = new StringBuilder();
                    builder.Append((x | (y << 16)).ToString());
                    //NativeMethods.SendMessage(criticalHandle, 0x00A1 /*NCLBUTTONDOWN*/, (uint)ControlzEx.Native.HT.CAPTION, builder);
                }
            }
            else if (handleStateChange
                     && e.ClickCount == 2
                     && (window.ResizeMode == ResizeMode.CanResize || window.ResizeMode == ResizeMode.CanResizeWithGrip))
            {
                e.Handled = true;

                if (window.WindowState == WindowState.Normal)
                {
                    //ControlzEx.SystemCommands.MaximizeWindow(window);
                }
                else
                {
                    //ControlzEx.SystemCommands.RestoreWindow(window);
                }
            }
        }
#pragma warning restore 618

        /// <summary>
        /// Shows the system menu at the current mouse position.
        /// </summary>
        /// <param name="dependencyObject">The object which was the source of the mouse event.</param>
        /// <param name="e">The mouse event args.</param>
        public static void ShowSystemMenu(DependencyObject dependencyObject, MouseButtonEventArgs e)
        {
            var window = Window.GetWindow(dependencyObject);

            if (window is null)
            {
                return;
            }

            ShowSystemMenu(window, e);
        }

        /// <summary>
        /// Shows the system menu at the current mouse position.
        /// </summary>
        /// <param name="window">The window for which the system menu should be shown.</param>
        /// <param name="e">The mouse event args.</param>
        public static void ShowSystemMenu(Window window, MouseButtonEventArgs e)
        {
            e.Handled = true;

#pragma warning disable 618
            //ControlzEx.SystemCommands.ShowSystemMenu(window, e);
#pragma warning restore 618
        }

        /// <summary>
        /// Shows the system menu at <paramref name="screenLocation"/>.
        /// </summary>
        /// <param name="window">The window for which the system menu should be shown.</param>
        /// <param name="screenLocation">The location at which the system menu should be shown.</param>
        public static void ShowSystemMenu(Window window, Point screenLocation)
        {
#pragma warning disable 618
            //ControlzEx.SystemCommands.ShowSystemMenuPhysicalCoordinates(window, screenLocation);
#pragma warning restore 618
        }
    }
}
