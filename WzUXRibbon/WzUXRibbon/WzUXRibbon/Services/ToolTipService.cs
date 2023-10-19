﻿using System;
using System.Windows;
using WzUXRibbon.Internal.KnownBoxes;

namespace WzUXRibbon.Services
{
    public static class ToolTipService
    {
        /// <summary>
        /// Attach ooltip properties to control.
        /// </summary>
        /// <param name="type">Control type.</param>
        public static void Attach(Type type)
        {
            System.Windows.Controls.ToolTipService.ShowOnDisabledProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
            System.Windows.Controls.ToolTipService.InitialShowDelayProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(900));
            System.Windows.Controls.ToolTipService.BetweenShowDelayProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(IntBoxes.Zero));
            System.Windows.Controls.ToolTipService.ShowDurationProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(20000));
        }
    }
}
