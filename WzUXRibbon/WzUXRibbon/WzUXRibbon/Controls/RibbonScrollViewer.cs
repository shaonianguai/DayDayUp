using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WzUXRibbon.Internal;

namespace WzUXRibbon.Controls
{
    public class RibbonScrollViewer : ScrollViewer
    {
        
        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            if (this.VisualChildrenCount > 0
                && this.GetVisualChild(0) is Visual firstVisualChild)
            {
                return VisualTreeHelper.HitTest(firstVisualChild, hitTestParameters.HitPoint);
            }

            return base.HitTestCore(hitTestParameters);
        }

        
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }

            if (this.ScrollInfo != null)
            {
                var horizontalOffsetBefore = this.ScrollInfo.HorizontalOffset;
                var verticalOffsetBefore = this.ScrollInfo.VerticalOffset;

                if (e.Delta < 0)
                {
                    this.ScrollInfo.MouseWheelDown();
                }
                else
                {
                    this.ScrollInfo.MouseWheelUp();
                }

                e.Handled = DoubleUtil.AreClose(horizontalOffsetBefore, this.ScrollInfo.HorizontalOffset) == false
                            || DoubleUtil.AreClose(verticalOffsetBefore, this.ScrollInfo.VerticalOffset) == false;
            }
        }
    }
}
