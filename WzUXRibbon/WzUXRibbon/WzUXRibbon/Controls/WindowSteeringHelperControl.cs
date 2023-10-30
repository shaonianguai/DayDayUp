using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using WzUXRibbon.Helpers;
using WzUXRibbon.Internal.KnownBoxes;

namespace WzUXRibbon.Controls
{
    public class WindowSteeringHelperControl : Border
    {
        /// <summary>
        /// Static constructor
        /// </summary>
        static WindowSteeringHelperControl()
        {
            BackgroundProperty.OverrideMetadata(typeof(WindowSteeringHelperControl), new FrameworkPropertyMetadata(System.Windows.Media.Brushes.Transparent));
            IsHitTestVisibleProperty.OverrideMetadata(typeof(WindowSteeringHelperControl), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
            HorizontalAlignmentProperty.OverrideMetadata(typeof(WindowSteeringHelperControl), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch));
            VerticalAlignmentProperty.OverrideMetadata(typeof(WindowSteeringHelperControl), new FrameworkPropertyMetadata(VerticalAlignment.Stretch));
        }

        
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (this.IsEnabled)
            {
                WindowSteeringHelper.HandleMouseLeftButtonDown(e, true, true);
            }
        }

        
        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonUp(e);

            if (this.IsEnabled)
            {
                WindowSteeringHelper.ShowSystemMenu(this, e);
            }
        }
    }
}
