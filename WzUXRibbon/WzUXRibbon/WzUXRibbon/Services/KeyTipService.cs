using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Windows;
using WzUXRibbon.Controls;
using WzUXRibbon.Data;
using WzUXRibbon.Internal;
using WzUXRibbon.Adorners;
using WzUXRibbon.Utils;

namespace WzUXRibbon.Services
{

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class KeyTipService
    {
        #region Fields

        private ScopeGuard windowPreviewKeyDownScopeGuard;
        private readonly Ribbon ribbon;
        private readonly DispatcherTimer timer;
        private KeyTipAdorner activeAdornerChain;
        private FocusWrapper backUpFocusedControl;
        private Window window;
        private bool attached;
        private HwndSource attachedHwndSource;
        private string currentUserInput;

        public bool AreAnyKeyTipsVisible
        {
            get
            {
                if (this.activeAdornerChain != null)
                {
                    return this.activeAdornerChain.AreAnyKeyTipsVisible;
                }

                return false;
            }
        }

        private static readonly Key[] modifierKeys =
        {
        Key.LeftShift,
        Key.RightShift,
        Key.LeftCtrl,
        Key.RightCtrl,
        Key.LeftAlt,
        Key.RightAlt,
        };

        public static IList<Key> DefaultKeyTipKeys =>
            new List<Key>
            {
            Key.LeftAlt,
            Key.RightAlt,
            Key.F10
            };

        public IList<Key> KeyTipKeys { get; } = DefaultKeyTipKeys;

        #endregion

        #region Initialization

        public KeyTipService(Ribbon ribbon)
        {
            this.ribbon = ribbon ?? throw new ArgumentNullException(nameof(ribbon));

            this.timer = new DispatcherTimer(TimeSpan.FromSeconds(0.7), DispatcherPriority.SystemIdle, this.OnDelayedShow, Dispatcher.CurrentDispatcher);
            this.timer.Stop();
        }

        #endregion

        public void Attach()
        {
            if (this.attached)
            {
                return;
            }

            this.attached = true;

            if (DesignerProperties.GetIsInDesignMode(this.ribbon))
            {
                return;
            }

            this.window = Window.GetWindow(this.ribbon);
            if (this.window is null)
            {
                return;
            }

            this.window.PreviewKeyDown += this.OnWindowPreviewKeyDown;
            this.window.KeyUp += this.OnWindowKeyUp;

            this.attachedHwndSource = (HwndSource)PresentationSource.FromVisual(this.window);
            this.attachedHwndSource?.AddHook(this.WindowProc);
        }

        public void Detach()
        {
            if (this.attached == false)
            {
                return;
            }

            this.attached = false;

            this.timer.Stop();

            if (this.window != null)
            {
                this.window.PreviewKeyDown -= this.OnWindowPreviewKeyDown;
                this.window.KeyUp -= this.OnWindowKeyUp;

                this.window = null;
            }

            this.attachedHwndSource?.RemoveHook(this.WindowProc);
        }

        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var message = (uint)msg;

            if (message == NativeMethods.WM_NCACTIVATE // mouse clicks in non client area
                || (message is NativeMethods.WM_ACTIVATE && wParam == IntPtr.Zero) // the window is deactivated
                || (message >= NativeMethods.WM_NCLBUTTONDOWN && message <= NativeMethods.WM_NCXBUTTONDBLCLK) // mouse click (non client area)
                || (message >= NativeMethods.WM_LBUTTONDOWN && message <= NativeMethods.WM_MBUTTONDBLCLK)) // mouse click
            {
                if (this.activeAdornerChain?.IsAdornerChainAlive == true)
                {
                    this.Terminate();
                }
            }

            if (ShouldDismissAllPopups(message, wParam))
            {
                PopupService.RaiseDismissPopupEvent(this.ribbon, DismissPopupMode.Always, DismissPopupReason.ApplicationLostFocus);
                PopupService.RaiseDismissPopupEvent(Mouse.Captured, DismissPopupMode.Always, DismissPopupReason.ApplicationLostFocus);
                PopupService.RaiseDismissPopupEvent(Keyboard.FocusedElement, DismissPopupMode.Always, DismissPopupReason.ApplicationLostFocus);
            }

            return IntPtr.Zero;

            bool ShouldDismissAllPopups(uint messageParam, IntPtr intPtrParam)
            {
                if ((messageParam == NativeMethods.WM_ACTIVATE && intPtrParam == IntPtr.Zero)
                    || messageParam == NativeMethods.WM_SIZE
                    || messageParam == NativeMethods.WM_DESTROY
                    || messageParam == NativeMethods.WM_QUIT)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void OnWindowPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (this.ribbon.IsKeyTipHandlingEnabled == false)
            {
                return;
            }

            if (this.windowPreviewKeyDownScopeGuard?.IsActive == true)
            {
                System.Media.SystemSounds.Beep.Play();
                return;
            }

            using (var scopeGuard = new ScopeGuard().Start())
            {
                this.windowPreviewKeyDownScopeGuard = scopeGuard;

                if (e.IsRepeat
                    || e.Handled)
                {
                    return;
                }

                if (this.ribbon.IsCollapsed
                    || this.ribbon.IsEnabled == false
                    || this.window is null
                    || this.window.IsActive == false)
                {
                    return;
                }

                if (e.KeyboardDevice.Modifiers == ModifierKeys.Alt
                    && e.SystemKey >= Key.NumPad0
                    && e.SystemKey <= Key.NumPad9)
                {
                    this.Terminate();
                    return;
                }

                if (this.IsShowOrHideKey(e))
                {
                    if (this.activeAdornerChain is null
                        || this.activeAdornerChain.IsAdornerChainAlive == false
                        || this.activeAdornerChain.AreAnyKeyTipsVisible == false)
                    {
                        this.ShowDelayed();
                    }
                    else
                    {
                        this.Terminate();
                    }
                }
                else if (e.Key == Key.Escape
                         && this.activeAdornerChain != null)
                {
                    this.activeAdornerChain.ActiveKeyTipAdorner.Back();
                    this.ClearUserInput();
                    e.Handled = true;
                }
                else if ((e.Key != Key.System && this.activeAdornerChain is null)
                         || e.SystemKey == Key.Escape
                         || (e.KeyboardDevice.Modifiers != ModifierKeys.Alt && this.activeAdornerChain is null))
                {
                    return;
                }
                else
                {
                    var actualKey = e.Key == Key.System ? e.SystemKey : e.Key;
                    var key = KeyEventUtility.GetStringFromKey(actualKey);
                    var isKeyRealInput = string.IsNullOrEmpty(key) == false
                                         && key != "\t";

                    if (isKeyRealInput == false)
                    {
                        this.backUpFocusedControl = null;
                        this.Terminate();

                        return;
                    }

                    var shownImmediately = false;

                    if (this.activeAdornerChain is null
                        || this.activeAdornerChain.IsAdornerChainAlive == false
                        || this.activeAdornerChain.AreAnyKeyTipsVisible == false)
                    {
                        this.ShowImmediatly();
                        shownImmediately = true;
                    }

                    if (this.activeAdornerChain is null)
                    {
                        return;
                    }

                    var previousInput = this.currentUserInput;
                    this.currentUserInput += key;

                    if (this.activeAdornerChain.ActiveKeyTipAdorner.ContainsKeyTipStartingWith(this.currentUserInput) == false)
                    {
                        if (shownImmediately)
                        {
                            this.Terminate();
                            return;
                        }

                        if (this.activeAdornerChain.AdornedElement is Ribbon)
                        {
                            this.Terminate();
                            return;
                        }

                        this.currentUserInput = previousInput;
                        System.Media.SystemSounds.Beep.Play();
                        e.Handled = true;
                        return;
                    }

                    if (this.activeAdornerChain.ActiveKeyTipAdorner.Forward(this.currentUserInput, true))
                    {
                        this.ClearUserInput();
                        e.Handled = true;
                        return;
                    }

                    this.activeAdornerChain.ActiveKeyTipAdorner.FilterKeyTips(this.currentUserInput);
                    e.Handled = true;
                }
            }
        }

        private void OnWindowKeyUp(object sender, KeyEventArgs e)
        {
            if (this.ribbon.IsKeyTipHandlingEnabled == false)
            {
                return;
            }

            if (this.ribbon.IsCollapsed
                || this.ribbon.IsEnabled == false
                || this.window is null
                || this.window.IsActive == false)
            {
                this.Terminate();
                return;
            }

            if (this.IsShowOrHideKey(e))
            {
                this.ClearUserInput();

                if (this.timer.IsEnabled)
                {
                    this.ShowImmediatly();
                }

                if (this.activeAdornerChain != null)
                {
                    e.Handled = true;
                }
            }
            else
            {
                this.timer.Stop();
            }
        }

        private bool IsShowOrHideKey(KeyEventArgs e)
        {
            var realKey = e.Key == Key.System
                ? e.SystemKey
                : e.Key;

            if (realKey == Key.F10
                && (Keyboard.IsKeyDown(Key.LeftShift)
                    || Keyboard.IsKeyDown(Key.RightShift)))
            {
                return false;
            }

            var isShowOrHideKey = this.KeyTipKeys.Any(x => x == realKey);

            if (isShowOrHideKey == false)
            {
                return false;
            }

            var blacklistedModifierKeys = modifierKeys.Except(this.KeyTipKeys);
            var blacklistedKeyPressed = blacklistedModifierKeys.Any(Keyboard.IsKeyDown);
            return blacklistedKeyPressed == false;
        }

        private void ClearUserInput()
        {
            this.currentUserInput = string.Empty;
        }

        private void ClosePopups()
        {
            PopupService.RaiseDismissPopupEvent(Keyboard.FocusedElement, DismissPopupMode.Always, DismissPopupReason.ShowingKeyTips);
        }

        private void RestoreFocus()
        {
            this.backUpFocusedControl?.Focus();
            this.backUpFocusedControl = null;
        }

        private void OnAdornerChainTerminated(object sender, KeyTipPressedResult e)
        {
            if (this.activeAdornerChain != null)
            {
                this.activeAdornerChain.Terminated -= this.OnAdornerChainTerminated;
            }

            this.activeAdornerChain = null;
            this.ClearUserInput();

            if (e.PressedElementOpenedPopup == false)
            {
                this.ClosePopups();
            }

            if (e.PressedElementAquiredFocus == false)
            {
                this.RestoreFocus();
            }
        }

        private void OnDelayedShow(object sender, EventArgs e)
        {
            if (this.activeAdornerChain is null)
            {
                this.Show();
            }

            this.timer.Stop();
        }

        private void ShowImmediatly()
        {
            this.Show();
        }

        private void ShowDelayed()
        {
            this.Terminate();

            this.timer.Start();
        }

        private void Terminate()
        {
            this.activeAdornerChain?.Terminate(KeyTipPressedResult.Empty);
        }

        private void Show()
        {
            this.timer.Stop();

            if (this.window is null
                || this.window.IsActive == false)
            {
                this.RestoreFocus();
                return;
            }

            var keyTipsTarget = this.GetApplicationMenu() ?? this.ribbon;

            if (keyTipsTarget is null)
            {
                return;
            }

            this.ClosePopups();

            this.backUpFocusedControl = null;

            if (UIHelper.GetParent<Ribbon>(Keyboard.FocusedElement as DependencyObject) is null)
            {
                this.backUpFocusedControl = FocusWrapper.GetWrapperForCurrentFocus();
            }

            if (keyTipsTarget is Ribbon && this.ribbon.TabControl != null)
            {
                int selectedIndex = Math.Max(this.ribbon.TabControl.SelectedIndex, 0);
                (this.ribbon.TabControl.ItemContainerGenerator.ContainerFromIndex(selectedIndex) as UIElement)?.Focus();
            }

            this.ClearUserInput();

            if (this.activeAdornerChain != null)
            {
                this.activeAdornerChain.Terminated -= this.OnAdornerChainTerminated;
            }

            this.activeAdornerChain = new KeyTipAdorner(this.ribbon, this.ribbon, null);
            this.activeAdornerChain.Terminated += this.OnAdornerChainTerminated;
            this.activeAdornerChain.Attach();

            if (keyTipsTarget as Ribbon == null)
            {
                this.activeAdornerChain.Forward(string.Empty, keyTipsTarget, false);
            }
        }

        private FrameworkElement GetApplicationMenu()
        {
            if (this.ribbon.Menu is null)
            {
                return null;
            }

            var control = this.ribbon.Menu as ApplicationMenu ?? UIHelper.FindImmediateVisualChild<ApplicationMenu>(this.ribbon.Menu, IsVisible);

            if (control is null)
            {
                return null;
            }

            return control.IsDropDownOpen
                ? control
                : null;
        }

        private static bool IsVisible(FrameworkElement obj)
        {
            return obj.Visibility == Visibility.Visible;
        }
    }
}
