using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WzUXRibbon.Controls;
using WzUXRibbon.Extensions;
using WzUXRibbon.Internal;

namespace WzUXRibbon.Services
{
    public enum DismissPopupMode
    {
        Always,
        MouseNotOver
    }

    public enum DismissPopupReason
    {
        Undefined,
        ApplicationLostFocus,
        ShowingKeyTips
    }

    public class DismissPopupEventArgs : RoutedEventArgs
    {
        public DismissPopupEventArgs()
            : this(DismissPopupMode.Always)
        {
        }

        public DismissPopupEventArgs(DismissPopupMode dismissMode)
            : this(dismissMode, DismissPopupReason.Undefined)
        {
        }

        public DismissPopupEventArgs(DismissPopupMode dismissMode, DismissPopupReason reason)
        {
            this.RoutedEvent = PopupService.DismissPopupEvent;
            this.DismissMode = dismissMode;
            this.DismissReason = reason;
        }

        public DismissPopupMode DismissMode { get; }

        public DismissPopupReason DismissReason { get; set; }

        
        protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
        {
            var handler = (EventHandler<DismissPopupEventArgs>)genericHandler;
            handler(genericTarget, this);
        }
    }

    public static class PopupService
    {
        #region DismissPopup

        public static readonly RoutedEvent DismissPopupEvent = EventManager.RegisterRoutedEvent("DismissPopup", RoutingStrategy.Bubble, typeof(EventHandler<DismissPopupEventArgs>), typeof(PopupService));

        public static void RaiseDismissPopupEventAsync(object sender, DismissPopupMode mode, DismissPopupReason reason = DismissPopupReason.Undefined)
        {
            var element = sender as UIElement;

            if (element is null)
            {
                return;
            }

            element.RunInDispatcherAsync(() => RaiseDismissPopupEvent(sender, mode, reason));
        }

        public static void RaiseDismissPopupEvent(object sender, DismissPopupMode mode, DismissPopupReason reason = DismissPopupReason.Undefined)
        {
            var element = sender as UIElement;

            if (element is null)
            {
                return;
            }

            element.RaiseEvent(new DismissPopupEventArgs(mode, reason));
        }

        #endregion

        public static void Attach(Type classType)
        {
            EventManager.RegisterClassHandler(classType, Mouse.PreviewMouseDownOutsideCapturedElementEvent, new MouseButtonEventHandler(OnClickThroughThunk));
            EventManager.RegisterClassHandler(classType, DismissPopupEvent, new EventHandler<DismissPopupEventArgs>(OnDismissPopup));
            EventManager.RegisterClassHandler(classType, FrameworkElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(OnContextMenuOpening), true);
            EventManager.RegisterClassHandler(classType, FrameworkElement.ContextMenuClosingEvent, new ContextMenuEventHandler(OnContextMenuClosing), true);
            EventManager.RegisterClassHandler(classType, UIElement.LostMouseCaptureEvent, new MouseEventHandler(OnLostMouseCapture));
        }

        private static void OnClickThroughThunk(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left
                || e.ChangedButton == MouseButton.Right)
            {
                if (Mouse.Captured == sender
                    || (sender is IDropDownControl && IsPopupRoot(Mouse.Captured)))
                {
                    if (sender is RibbonTabControl ribbonTabControl
                        && ribbonTabControl.IsMinimized
                        && IsPopupRoot(e.OriginalSource) == false)
                    {
                        if (IsMousePhysicallyOver(ribbonTabControl.SelectedContentPresenter) == false)
                        {
                            RaiseDismissPopupEvent(sender, DismissPopupMode.Always);
                        }
                    }
                    else
                    {
                        RaiseDismissPopupEvent(sender, DismissPopupMode.MouseNotOver);
                    }
                }
            }
        }

        private static void OnLostMouseCapture(object sender, MouseEventArgs e)
        {
            var control = sender as IDropDownControl;

            if (control is null)
            {
                return;
            }

            if (Mouse.Captured == sender
                || control.IsDropDownOpen == false
                || control.IsContextMenuOpened)
            {
                return;
            }

            var popup = control.DropDownPopup;

            if (popup?.Child is null)
            {
                RaiseDismissPopupEvent(sender, DismissPopupMode.MouseNotOver);
                return;
            }

            if (e.OriginalSource == sender)
            {
                if (popup.PlacementTarget is RibbonTabItem)
                {
                    if (Mouse.Captured is null
                        || IsAncestorOf(popup, Mouse.Captured as DependencyObject) == false)
                    {
                        RaiseDismissPopupEvent(sender, DismissPopupMode.Always);
                    }
                }

                return;
            }

            if (IsAncestorOf(popup, sender as DependencyObject) == false
                && IsAncestorOf(sender as DependencyObject, popup) == false
                && IsAncestorOf(popup, e.OriginalSource as DependencyObject) == false)
            {
                RaiseDismissPopupEvent(sender, DismissPopupMode.MouseNotOver);
                return;
            }

            if (e.OriginalSource != null
                && Mouse.Captured is null
                && (IsPopupRoot(e.OriginalSource) || IsAncestorOf(popup.Child, e.OriginalSource as DependencyObject)))
            {
                Mouse.Capture(sender as IInputElement, CaptureMode.SubTree);
                e.Handled = true;

                if (e.OriginalSource is MenuBase)
                {
                    RaiseDismissPopupEvent(sender, DismissPopupMode.MouseNotOver);
                }
            }
        }

        public static bool IsAncestorOf(DependencyObject parent, DependencyObject element)
        {
            if (parent is null)
            {
                return false;
            }

            while (element != null)
            {
                if (ReferenceEquals(element, parent))
                {
                    return true;
                }

                element = UIHelper.GetVisualOrLogicalParent(element);
            }

            return false;
        }

        private static void OnDismissPopup(object sender, DismissPopupEventArgs e)
        {
            var control = sender as IDropDownControl;

            if (control is null)
            {
                return;
            }

            switch (e.DismissMode)
            {
                case DismissPopupMode.Always:
                    DismisPopupForAlways(control, e);
                    break;

                case DismissPopupMode.MouseNotOver:
                    DismisPopupForMouseNotOver(control, e);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(e.DismissMode), e.DismissMode, "Unknown DismissMode.");
            }
        }

        private static void DismisPopupForAlways(IDropDownControl control, DismissPopupEventArgs e)
        {
            control.IsDropDownOpen = false;
        }

        private static void DismisPopupForMouseNotOver(IDropDownControl control, DismissPopupEventArgs e)
        {
            if (control.IsDropDownOpen == false
                || (control is DropDownButton dropDownButton && dropDownButton.DismissOnClickOutside == false))
            {
                return;
            }

            if (control is RibbonTabControl ribbonTabControl
                && ribbonTabControl.IsMinimized
                && IsAncestorOf(control as DependencyObject, e.OriginalSource as DependencyObject))
            {
                if (Mouse.Captured is ApplicationMenu)
                {
                    control.IsDropDownOpen = false;
                    return;
                }

                Mouse.Capture(control as IInputElement, CaptureMode.SubTree);
                return;
            }

            if (IsMousePhysicallyOver(control.DropDownPopup) == false)
            {
                control.IsDropDownOpen = false;
            }
            else
            {
                if (Mouse.Captured != control)
                {
                    Mouse.Capture(control as IInputElement, CaptureMode.SubTree);
                }

                e.Handled = true;
            }
        }

        public static bool IsMousePhysicallyOver(Popup popup)
        {
            if (popup?.Child is null)
            {
                return false;
            }

            return IsMousePhysicallyOver(popup.Child);
        }

        public static bool IsMousePhysicallyOver(UIElement element)
        {
            if (element is null)
            {
                return false;
            }

            var position = Mouse.GetPosition(element);
            return position.X >= 0.0
                   && position.Y >= 0.0
                   && position.X <= element.RenderSize.Width
                   && position.Y <= element.RenderSize.Height;
        }

        private static void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (sender is IDropDownControl control)
            {
                control.IsContextMenuOpened = true;
            }
        }

        private static void OnContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            if (sender is IDropDownControl control)
            {
                control.IsContextMenuOpened = false;

                if (Mouse.Captured is System.Windows.Controls.ContextMenu == false)
                {
                    RaiseDismissPopupEvent(e.OriginalSource, DismissPopupMode.MouseNotOver);
                }
            }
        }

        private static bool IsPopupRoot(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            var type = obj.GetType();

            return type.FullName == "System.Windows.Controls.Primitives.PopupRoot"
                   || type.Name == "PopupRoot";
        }
    }
}
