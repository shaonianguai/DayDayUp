﻿using System.Windows;

namespace WzUXRibbon.Controls
{
    internal class GalleryItemPlaceholder : UIElement
    {
        #region Properties

        /// <summary>
        /// Gets the target of the placeholder
        /// </summary>
        public UIElement Target { get; }

        public Size ArrangedSize { get; private set; }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Target</param>
        public GalleryItemPlaceholder(UIElement target)
        {
            this.Target = target;
        }

        #endregion

        #region Methods

        
        protected override Size MeasureCore(Size availableSize)
        {
            this.Target.Measure(availableSize);
            return this.Target.DesiredSize;
        }

        
        protected override void ArrangeCore(Rect finalRect)
        {
            base.ArrangeCore(finalRect);

            // Remember arranged size to arrange
            // targets in GalleryPanel lately
            this.ArrangedSize = finalRect.Size;
        }

        #endregion

        #region Debug

        /* FOR DEGUG */
        //protected override void OnRender(DrawingContext drawingContext)
        //{
        //    drawingContext.DrawRectangle(null, new Pen(Brushes.Red, 1), new Rect(this.RenderSize));
        //}

        #endregion
    }
}
