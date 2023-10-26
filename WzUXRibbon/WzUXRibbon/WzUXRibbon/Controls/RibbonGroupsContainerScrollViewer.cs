using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media.Animation;
using System;
using WzUXRibbon.Command;

namespace WzUXRibbon.Controls
{
    public static class LeftOrRighnLineCommand
    {
        private static ModelCommand _leftLineScrollCommand;
        private static ModelCommand _rightLineScrollCommand;

        public static ModelCommand LeftLineScrollCommand => _leftLineScrollCommand ?? (_leftLineScrollCommand = new ModelCommand(ExecuteLeftLineScrollCommand, p => true));

        public static ModelCommand RightLineScrollCommand => _rightLineScrollCommand ?? (_rightLineScrollCommand = new ModelCommand(ExecuteRightLineScrollCommand, p => true));

        private static void ExecuteLeftLineScrollCommand(object parameter)
        {
            var scrollView = parameter as RibbonGroupsContainerScrollViewer;
            scrollView.CalcuteAnimateScroll(120);
        }

        private static void ExecuteRightLineScrollCommand(object parameter)
        {
            var scrollView = parameter as RibbonGroupsContainerScrollViewer;
            scrollView.CalcuteAnimateScroll(-120);
        }
    }

    public static class ScrollViewerBehavior
    {
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ScrollViewerBehavior), new UIPropertyMetadata(0.0, OnHorizontalOffsetChanged));
        public static void SetHorizontalOffset(FrameworkElement target, double value) => target.SetValue(HorizontalOffsetProperty, value);
        public static double GetHorizontalOffset(FrameworkElement target) => (double)target.GetValue(HorizontalOffsetProperty);
        private static void OnHorizontalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e) => (target as ScrollViewer)?.ScrollToHorizontalOffset((double)e.NewValue);
    }

    public class RibbonGroupsContainerScrollViewer : ScrollViewer
    {
        private double _lastLocation = 0;

        static RibbonGroupsContainerScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonGroupsContainerScrollViewer), new FrameworkPropertyMetadata(typeof(RibbonGroupsContainerScrollViewer)));
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }

            if (this.ScrollInfo is null)
            {
                return;
            }

            CalcuteAnimateScroll(e.Delta);

            e.Handled = true;
        }

        public void CalcuteAnimateScroll(double changeOffset)
        {
            if (Mouse.Captured is IDropDownControl dropDownControl
                && dropDownControl.IsDropDownOpen == true
                && dropDownControl.DropDownPopup != null
                && dropDownControl as RibbonControl == null)
            {
                return;
            }

            double newOffset = _lastLocation - (changeOffset * 1.5);
            ScrollToHorizontalOffset(_lastLocation);
            if (newOffset < 0)
                newOffset = 0;

            if (newOffset > ScrollableWidth)
                newOffset = ScrollableWidth;

            AnimateScroll(newOffset);
            _lastLocation = newOffset;
        }

        private void AnimateScroll(double ToValue)
        {
            BeginAnimation(ScrollViewerBehavior.HorizontalOffsetProperty, null);
            var Animation = new DoubleAnimation();
            Animation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Animation.From = HorizontalOffset;
            Animation.To = ToValue;
            Animation.Duration = TimeSpan.FromMilliseconds(700);
            BeginAnimation(ScrollViewerBehavior.HorizontalOffsetProperty, Animation);
        }
    }
}
