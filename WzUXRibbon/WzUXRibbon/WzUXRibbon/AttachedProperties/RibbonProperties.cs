using System.Windows;
using System.Windows.Media;
using WzUXRibbon.Controls;
using WzUXRibbon.Data;
using WzUXRibbon.Enumerations;
using WzUXRibbon.Extensibility;
using WzUXRibbon.Internal.KnownBoxes;

namespace WzUXRibbon.AttachedProperties
{
    public class RibbonProperties : DependencyObject
    {
        #region Size Property
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.RegisterAttached("Size", typeof(RibbonControlSize), typeof(RibbonProperties),
                new FrameworkPropertyMetadata(RibbonControlSize.Large,
                    FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsParentArrange |
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure,
                    OnSizeChanged));

        public static void SetSize(DependencyObject element, RibbonControlSize value)
        {
            element.SetValue(SizeProperty, value);
        }

        public static RibbonControlSize GetSize(DependencyObject element)
        {
            return (RibbonControlSize)element.GetValue(SizeProperty);
        }

        private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sink = d as IRibbonSizeChangedSink;

            sink?.OnSizePropertyChanged((RibbonControlSize)e.OldValue, (RibbonControlSize)e.NewValue);
        }

        #endregion

        #region SizeDefinition Property

        public static readonly DependencyProperty SizeDefinitionProperty =
            DependencyProperty.RegisterAttached("SizeDefinition", typeof(RibbonControlSizeDefinition), typeof(RibbonProperties),
                new FrameworkPropertyMetadata(new RibbonControlSizeDefinition(RibbonControlSize.Large, RibbonControlSize.Middle, RibbonControlSize.Small),
                    FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsParentArrange |
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure,
                    OnSizeDefinitionChanged));

        public static void SetSizeDefinition(DependencyObject element, RibbonControlSizeDefinition value)
        {
            element.SetValue(SizeDefinitionProperty, value);
        }

        public static RibbonControlSizeDefinition GetSizeDefinition(DependencyObject element)
        {
            return (RibbonControlSizeDefinition)element.GetValue(SizeDefinitionProperty);
        }

        internal static void OnSizeDefinitionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var groupBox = FindParentRibbonGroupBox(d);
            var element = (UIElement)d;
            var isSimplified = groupBox?.IsSimplified ?? false;

            if (!isSimplified)
            {
                SetAppropriateSize(element, groupBox?.State ?? RibbonGroupBoxState.Large, isSimplified);
            }
        }

        internal static RibbonGroupBox FindParentRibbonGroupBox(DependencyObject element)
        {
            var currentElement = element;
            RibbonGroupBox groupBox;

            while ((groupBox = currentElement as RibbonGroupBox) is null)
            {
                currentElement = VisualTreeHelper.GetParent(currentElement)
                                 ?? LogicalTreeHelper.GetParent(currentElement);

                if (currentElement is null)
                {
                    break;
                }
            }

            return groupBox;
        }

        public static void SetAppropriateSize(DependencyObject element, RibbonGroupBoxState state, bool isSimplified)
        {
            var sizeDefinition = isSimplified ? GetSimplifiedSizeDefinition(element) : GetSizeDefinition(element);
            SetSize(element, sizeDefinition.GetSize(state));
        }

        #endregion

        #region SimplifiedSizeDefinition Property

        public static readonly DependencyProperty SimplifiedSizeDefinitionProperty =
            DependencyProperty.RegisterAttached(nameof(ISimplifiedRibbonControl.SimplifiedSizeDefinition), typeof(RibbonControlSizeDefinition), typeof(RibbonProperties),
                new FrameworkPropertyMetadata(new RibbonControlSizeDefinition(RibbonControlSize.Large, RibbonControlSize.Middle, RibbonControlSize.Small),
                    FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsParentArrange |
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure,
                    OnSimplifiedSizeDefinitionChanged));

        public static void SetSimplifiedSizeDefinition(DependencyObject element, RibbonControlSizeDefinition value)
        {
            element.SetValue(SimplifiedSizeDefinitionProperty, value);
        }

        public static RibbonControlSizeDefinition GetSimplifiedSizeDefinition(DependencyObject element)
        {
            return (RibbonControlSizeDefinition)element.GetValue(SimplifiedSizeDefinitionProperty);
        }

        internal static void OnSimplifiedSizeDefinitionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var groupBox = FindParentRibbonGroupBox(d);
            var element = (UIElement)d;
            var isSimplified = groupBox?.IsSimplified ?? false;

            if (isSimplified)
            {
                SetAppropriateSize(element, groupBox?.State ?? RibbonGroupBoxState.Large, isSimplified);
            }
        }

        public static void SetAppropriateSize(DependencyObject element, RibbonControlSize size)
        {
            SetSize(element, GetSizeDefinition(element).GetSize(size));
        }

        #endregion

        #region MouseOverBackgroundProperty

        public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.RegisterAttached("MouseOverBackground", typeof(Brush), typeof(RibbonProperties), new PropertyMetadata(default(Brush)));

        public static void SetMouseOverBackground(DependencyObject element, Brush value)
        {
            element.SetValue(MouseOverBackgroundProperty, value);
        }

        public static Brush GetMouseOverBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(MouseOverBackgroundProperty);
        }

        #endregion

        #region MouseOverForegroundProperty

        public static readonly DependencyProperty MouseOverForegroundProperty = DependencyProperty.RegisterAttached("MouseOverForeground", typeof(Brush), typeof(RibbonProperties), new PropertyMetadata(default(Brush)));

        public static void SetMouseOverForeground(DependencyObject element, Brush value)
        {
            element.SetValue(MouseOverForegroundProperty, value);
        }

        public static Brush GetMouseOverForeground(DependencyObject element)
        {
            return (Brush)element.GetValue(MouseOverForegroundProperty);
        }

        #endregion

        #region IsSelectedBackgroundProperty

        public static readonly DependencyProperty IsSelectedBackgroundProperty = DependencyProperty.RegisterAttached("IsSelectedBackground", typeof(Brush), typeof(RibbonProperties), new PropertyMetadata(default(Brush)));

        public static void SetIsSelectedBackground(DependencyObject element, Brush value)
        {
            element.SetValue(IsSelectedBackgroundProperty, value);
        }

        public static Brush GetIsSelectedBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(IsSelectedBackgroundProperty);
        }

        #endregion

        #region LastVisibleWidthProperty

        public static readonly DependencyProperty LastVisibleWidthProperty = DependencyProperty.RegisterAttached(
            "LastVisibleWidth", typeof(double), typeof(RibbonProperties), new PropertyMetadata(DoubleBoxes.Zero));

        public static void SetLastVisibleWidth(DependencyObject element, double value)
        {
            element.SetValue(LastVisibleWidthProperty, value);
        }

        public static double GetLastVisibleWidth(DependencyObject element)
        {
            if (element is null)
            {
                return 0;
            }

            return (double)element.GetValue(LastVisibleWidthProperty);
        }

        #endregion LastVisibleWidthProperty

        #region IsElementInQuickAccessToolBarProperty

        public static readonly DependencyProperty IsElementInQuickAccessToolBarProperty = DependencyProperty.RegisterAttached(
            "IsElementInQuickAccessToolBar", typeof(bool), typeof(RibbonProperties), new PropertyMetadata(BooleanBoxes.FalseBox));

        public static void SetIsElementInQuickAccessToolBar(DependencyObject element, bool value)
        {
            element.SetValue(IsElementInQuickAccessToolBarProperty, BooleanBoxes.Box(value));
        }

        public static bool GetIsElementInQuickAccessToolBar(DependencyObject element)
        {
            return (bool)element.GetValue(IsElementInQuickAccessToolBarProperty);
        }

        #endregion IsElementInQuickAccessToolBarProperty

        #region DesiredIconSize

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.RegisterAttached(
            "IconSize", typeof(IconSize), typeof(RibbonProperties), new PropertyMetadata(IconSizeBoxes.Small));

        public static void SetIconSize(DependencyObject element, IconSize value)
        {
            element.SetValue(IconSizeProperty, IconSizeBoxes.Box(value));
        }

        [AttachedPropertyBrowsableForType(typeof(IRibbonControl))]
        [AttachedPropertyBrowsableForType(typeof(IMediumIconProvider))]
        [AttachedPropertyBrowsableForType(typeof(ILargeIconProvider))]
        public static IconSize GetIconSize(DependencyObject element)
        {
            return (IconSize)element.GetValue(IconSizeProperty);
        }

        #endregion
    }
}
