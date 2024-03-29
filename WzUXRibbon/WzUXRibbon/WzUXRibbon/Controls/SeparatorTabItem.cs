﻿using System.Windows.Controls;
using System.Windows;
using WzUXRibbon.Internal.KnownBoxes;
using WzUXRibbon.Internal;

namespace WzUXRibbon.Controls
{
    public class SeparatorTabItem : TabItem
    {
        #region Constructors

        /// <summary>
        /// Static constructor
        /// </summary>
        static SeparatorTabItem()
        {
            var type = typeof(SeparatorTabItem);

            DefaultStyleKeyProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(type));
            IsEnabledProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, null, CoerceIsEnabledAndTabStop));
            IsTabStopProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, null, CoerceIsEnabledAndTabStop));
            IsSelectedProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, OnIsSelectedChanged));
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false)
            {
                return;
            }

            var separatorTabItem = (SeparatorTabItem)d;
            var tabControl = UIHelper.GetParent<TabControl>(separatorTabItem);

            if (tabControl is null
                || tabControl.Items.Count <= 1)
            {
                return;
            }

            tabControl.SelectedIndex = tabControl.SelectedIndex == tabControl.Items.Count - 1
                ? tabControl.SelectedIndex - 1
                : tabControl.SelectedIndex + 1;
        }

        private static object CoerceIsEnabledAndTabStop(DependencyObject d, object basevalue)
        {
            return BooleanBoxes.FalseBox;
        }

        #endregion
    }
}
