using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Xml.Linq;
using WzUXRibbon.Internal.KnownBoxes;

namespace WzUXRibbon.Controls
{
    /// <inheritdoc />
    [TemplatePart(Name = "PART_ResizeVerticalThumb", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_ResizeBothThumb", Type = typeof(Thumb))]
    public class ResizeableContentControl : ContentControl
    {
        // Thumb to resize in both directions
        private Thumb resizeBothThumb;

        // Thumb to resize vertical
        private Thumb resizeVerticalThumb;

        static ResizeableContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeableContentControl), new FrameworkPropertyMetadata(typeof(ResizeableContentControl)));

            FocusableProperty.OverrideMetadata(typeof(ResizeableContentControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ResizeableContentControl), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ResizeableContentControl), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
        }

        #region ResizeMode

        /// <summary>
        /// Gets or sets context menu resize mode
        /// </summary>
        public ContextMenuResizeMode ResizeMode
        {
            get => (ContextMenuResizeMode)this.GetValue(ResizeModeProperty);
            set => this.SetValue(ResizeModeProperty, value);
        }

        /// <summary>Identifies the <see cref="ResizeMode"/> dependency property.</summary>
        public static readonly DependencyProperty ResizeModeProperty =
            DependencyProperty.Register(nameof(ResizeMode), typeof(ContextMenuResizeMode),
                typeof(ResizeableContentControl), new PropertyMetadata(ContextMenuResizeMode.None));

        #endregion

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.resizeVerticalThumb != null)
            {
                this.resizeVerticalThumb.DragDelta -= this.OnResizeVerticalDelta;
            }

            if (this.resizeBothThumb != null)
            {
                this.resizeBothThumb.DragDelta -= this.OnResizeBothDelta;
            }

            this.resizeVerticalThumb = this.GetTemplateChild("PART_ResizeVerticalThumb") as Thumb;

            this.resizeBothThumb = this.GetTemplateChild("PART_ResizeBothThumb") as Thumb;

            if (this.resizeVerticalThumb != null)
            {
                this.resizeVerticalThumb.DragDelta += this.OnResizeVerticalDelta;
            }

            if (this.resizeBothThumb != null)
            {
                this.resizeBothThumb.DragDelta += this.OnResizeBothDelta;
            }
        }

        private double GetResizeThumbHeight()
        {
            double? height;
            switch (this.ResizeMode)
            {
                case ContextMenuResizeMode.None:
                    height = 0;
                    break;
                case ContextMenuResizeMode.Vertical:
                    height = this.resizeVerticalThumb?.ActualHeight;
                    break;
                case ContextMenuResizeMode.Both:
                    height = this.resizeBothThumb?.ActualHeight;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            return height ?? 0;
        }

        // Handles resize both drag
        private void OnResizeBothDelta(object sender, DragDeltaEventArgs e)
        {
            if (double.IsNaN(this.Width))
            {
                this.Width = this.ActualWidth;
            }

            if (double.IsNaN(this.Height))
            {
                this.Height = this.ActualHeight;
            }

            this.Width = Math.Max(this.MinWidth, Math.Min(this.MaxWidth, this.Width + e.HorizontalChange));
            this.Height = Math.Max(this.MinHeight, Math.Min(this.MaxHeight + this.GetResizeThumbHeight(), this.Height + e.VerticalChange));
        }

        // Handles resize vertical drag
        private void OnResizeVerticalDelta(object sender, DragDeltaEventArgs e)
        {
            if (double.IsNaN(this.Height))
            {
                this.Height = this.ActualHeight;
            }

            this.Height = Math.Max(this.MinHeight, Math.Min(this.MaxHeight + this.GetResizeThumbHeight(), this.Height + e.VerticalChange));
        }

        /// <summary>
        /// Gets whether the mouse is over any of the resize thumbs.
        /// </summary>
        public bool IsMouseOverResizeThumbs => (this.resizeBothThumb?.IsMouseOver ?? false)
                                               || (this.resizeVerticalThumb?.IsMouseOver ?? false);
    }
}