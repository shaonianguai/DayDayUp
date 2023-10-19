using System;
using System.Windows;
using WzUXRibbon.Controls;
using WzUXRibbon.Internal.KnownBoxes;

namespace WzUXRibbon.Services
{
    public static class ContextMenuService
    {
        /// <summary>
        /// Attach needed parameters to control
        /// </summary>
        public static void Attach(Type type)
        {
            System.Windows.Controls.ContextMenuService.ShowOnDisabledProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
            FrameworkElement.ContextMenuProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(OnContextMenuChanged, CoerceContextMenu));
        }

        private static void OnContextMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(FrameworkElement.ContextMenuProperty);
        }

        /// <summary>
        /// Coerces the ContextMenu for <paramref name="d"/>.
        /// </summary>
        /// <returns></returns>
        public static object CoerceContextMenu(DependencyObject d, object basevalue)
        {
            var control = d as IQuickAccessItemProvider;
            if (basevalue is null
                && (control is null || control.CanAddToQuickAccessToolBar))
            {
                return Ribbon.RibbonContextMenu;
            }

            return basevalue;
        }

        /// <summary>
        /// Coerce control context menu
        /// </summary>
        public static void Coerce(DependencyObject o)
        {
            o.CoerceValue(FrameworkElement.ContextMenuProperty);
        }
    }
}
