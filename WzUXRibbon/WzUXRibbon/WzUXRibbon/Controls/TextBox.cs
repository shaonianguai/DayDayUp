using System.Collections;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using WzUXRibbon.AttachedProperties;
using WzUXRibbon.Data;
using WzUXRibbon.Enumerations;
using WzUXRibbon.Helpers;
using WzUXRibbon.Internal.KnownBoxes;

namespace WzUXRibbon.Controls
{

    [TemplatePart(Name = "PART_ContentHost", Type = typeof(UIElement))]
    public class TextBox : System.Windows.Controls.TextBox, IQuickAccessItemProvider, IRibbonControl, IMediumIconProvider, ISimplifiedRibbonControl
    {
        private UIElement contentHost;

        #region Properties (Dependency)

        #region IsSimplified

        /// <summary>
        /// Gets or sets whether or not the ribbon is in Simplified mode
        /// </summary>
        public bool IsSimplified
        {
            get { return (bool)this.GetValue(IsSimplifiedProperty); }
            private set { this.SetValue(IsSimplifiedPropertyKey, BooleanBoxes.Box(value)); }
        }

        private static readonly DependencyPropertyKey IsSimplifiedPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsSimplified), typeof(bool), typeof(TextBox), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>Identifies the <see cref="IsSimplified"/> dependency property.</summary>
        public static readonly DependencyProperty IsSimplifiedProperty = IsSimplifiedPropertyKey.DependencyProperty;

        #endregion

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Static constructor
        /// </summary>
        static TextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(typeof(TextBox)));
        }

        #endregion

        #region Overrides

        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.contentHost = this.Template.FindName("PART_ContentHost", this) as UIElement;
        }

        
        // Handling context menu manually to fix #653
        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            this.InvalidateProperty(ContextMenuProperty);

            if (this.contentHost?.IsMouseOver == true
                || this.contentHost?.IsKeyboardFocusWithin == true)
            {
                base.OnContextMenuOpening(e);
            }
            else
            {
                var coerced = WzUXRibbon.Services.ContextMenuService.CoerceContextMenu(this, this.ContextMenu);
                if (coerced != null)
                {
                    this.SetCurrentValue(ContextMenuProperty, coerced);
                }

                base.OnContextMenuOpening(e);
            }
        }

        
        protected override void OnContextMenuClosing(ContextMenuEventArgs e)
        {
            this.InvalidateProperty(ContextMenuProperty);

            base.OnContextMenuClosing(e);
        }

        
        protected override void OnKeyUp(KeyEventArgs e)
        {
            // Avoid Click invocation (from RibbonControl)
            if (e.Key == Key.Enter
                || e.Key == Key.Space)
            {
                return;
            }

            base.OnKeyUp(e);
        }

        #endregion

        #region Quick Access Item Creating

        
        public virtual FrameworkElement CreateQuickAccessItem()
        {
            var textBoxForQAT = new TextBox();

            this.BindQuickAccessItem(textBoxForQAT);

            return textBoxForQAT;
        }

        
        public bool CanAddToQuickAccessToolBar
        {
            get { return (bool)this.GetValue(CanAddToQuickAccessToolBarProperty); }
            set { this.SetValue(CanAddToQuickAccessToolBarProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="CanAddToQuickAccessToolBar"/> dependency property.</summary>
        public static readonly DependencyProperty CanAddToQuickAccessToolBarProperty = RibbonControl.CanAddToQuickAccessToolBarProperty.AddOwner(typeof(TextBox), new PropertyMetadata(BooleanBoxes.TrueBox, RibbonControl.OnCanAddToQuickAccessToolBarChanged));

        /// <summary>
        /// This method must be overridden to bind properties to use in quick access creating
        /// </summary>
        /// <param name="element">Toolbar item</param>
        protected virtual void BindQuickAccessItem(FrameworkElement element)
        {
            RibbonControl.BindQuickAccessItem(this, element);

            RibbonControl.BindQuickAccessItem(this, element);

            RibbonControl.Bind(this, element, nameof(this.Text), TextProperty, BindingMode.TwoWay, UpdateSourceTrigger.PropertyChanged);
            RibbonControl.Bind(this, element, nameof(this.IsReadOnly), IsReadOnlyProperty, BindingMode.OneWay);
            RibbonControl.Bind(this, element, nameof(this.CharacterCasing), CharacterCasingProperty, BindingMode.TwoWay);
            RibbonControl.Bind(this, element, nameof(this.MaxLength), MaxLengthProperty, BindingMode.TwoWay);
            RibbonControl.Bind(this, element, nameof(this.TextAlignment), TextAlignmentProperty, BindingMode.TwoWay);
            RibbonControl.Bind(this, element, nameof(this.TextDecorations), TextDecorationsProperty, BindingMode.TwoWay);
            RibbonControl.Bind(this, element, nameof(this.IsUndoEnabled), IsUndoEnabledProperty, BindingMode.TwoWay);
            RibbonControl.Bind(this, element, nameof(this.UndoLimit), UndoLimitProperty, BindingMode.TwoWay);
            RibbonControl.Bind(this, element, nameof(this.AutoWordSelection), AutoWordSelectionProperty, BindingMode.TwoWay);
            RibbonControl.Bind(this, element, nameof(this.SelectionBrush), SelectionBrushProperty, BindingMode.TwoWay);
            RibbonControl.Bind(this, element, nameof(this.SelectionOpacity), SelectionOpacityProperty, BindingMode.TwoWay);
            RibbonControl.Bind(this, element, nameof(this.CaretBrush), CaretBrushProperty, BindingMode.TwoWay);
        }

        #endregion

        #region Implementation of Ribbon interfaces

        
        public KeyTipPressedResult OnKeyTipPressed()
        {
            this.SelectAll();
            this.Focus();

            return new KeyTipPressedResult(true, false);
        }

        
        public void OnKeyTipBack()
        {
        }

        #region Size

        
        public RibbonControlSize Size
        {
            get { return (RibbonControlSize)this.GetValue(SizeProperty); }
            set { this.SetValue(SizeProperty, value); }
        }

        /// <summary>Identifies the <see cref="Size"/> dependency property.</summary>
        public static readonly DependencyProperty SizeProperty = RibbonProperties.SizeProperty.AddOwner(typeof(TextBox));

        #endregion

        #region SizeDefinition

        
        public RibbonControlSizeDefinition SizeDefinition
        {
            get { return (RibbonControlSizeDefinition)this.GetValue(SizeDefinitionProperty); }
            set { this.SetValue(SizeDefinitionProperty, value); }
        }

        /// <summary>Identifies the <see cref="SizeDefinition"/> dependency property.</summary>
        public static readonly DependencyProperty SizeDefinitionProperty = RibbonProperties.SizeDefinitionProperty.AddOwner(typeof(TextBox));

        #endregion

        #region SimplifiedSizeDefinition

        
        public RibbonControlSizeDefinition SimplifiedSizeDefinition
        {
            get { return (RibbonControlSizeDefinition)this.GetValue(SimplifiedSizeDefinitionProperty); }
            set { this.SetValue(SimplifiedSizeDefinitionProperty, value); }
        }

        /// <summary>Identifies the <see cref="SimplifiedSizeDefinition"/> dependency property.</summary>
        public static readonly DependencyProperty SimplifiedSizeDefinitionProperty = RibbonProperties.SimplifiedSizeDefinitionProperty.AddOwner(typeof(TextBox));

        #endregion

        #region KeyTip

        
        public string KeyTip
        {
            get { return (string)this.GetValue(KeyTipProperty); }
            set { this.SetValue(KeyTipProperty, value); }
        }

        /// <inheritdoc cref="KeyTip.KeysProperty"/>
        public static readonly DependencyProperty KeyTipProperty = WzUXRibbon.Controls.KeyTip.KeysProperty.AddOwner(typeof(TextBox));

        #endregion

        #region Header

        
        public object Header
        {
            get { return this.GetValue(HeaderProperty); }
            set { this.SetValue(HeaderProperty, value); }
        }

        /// <summary>Identifies the <see cref="Header"/> dependency property.</summary>
        public static readonly DependencyProperty HeaderProperty = RibbonControl.HeaderProperty.AddOwner(typeof(TextBox), new PropertyMetadata(LogicalChildSupportHelper.OnLogicalChildPropertyChanged));

        
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)this.GetValue(HeaderTemplateProperty); }
            set { this.SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>Identifies the <see cref="HeaderTemplate"/> dependency property.</summary>
        public static readonly DependencyProperty HeaderTemplateProperty = RibbonControl.HeaderTemplateProperty.AddOwner(typeof(TextBox), new PropertyMetadata());

        
        public DataTemplateSelector HeaderTemplateSelector
        {
            get { return (DataTemplateSelector)this.GetValue(HeaderTemplateSelectorProperty); }
            set { this.SetValue(HeaderTemplateSelectorProperty, value); }
        }

        /// <summary>Identifies the <see cref="HeaderTemplateSelector"/> dependency property.</summary>
        public static readonly DependencyProperty HeaderTemplateSelectorProperty = RibbonControl.HeaderTemplateSelectorProperty.AddOwner(typeof(TextBox), new PropertyMetadata());

        #endregion

        #region Icon

        
        public object Icon
        {
            get { return this.GetValue(IconProperty); }
            set { this.SetValue(IconProperty, value); }
        }

        /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
        public static readonly DependencyProperty IconProperty = RibbonControl.IconProperty.AddOwner(typeof(TextBox), new PropertyMetadata(LogicalChildSupportHelper.OnLogicalChildPropertyChanged));

        #endregion

        #region MediumIcon

        
        public object MediumIcon
        {
            get { return this.GetValue(MediumIconProperty); }
            set { this.SetValue(MediumIconProperty, value); }
        }

        /// <summary>Identifies the <see cref="MediumIcon"/> dependency property.</summary>
        public static readonly DependencyProperty MediumIconProperty = MediumIconProviderProperties.MediumIconProperty.AddOwner(typeof(TextBox), new PropertyMetadata(LogicalChildSupportHelper.OnLogicalChildPropertyChanged));

        #endregion

        #endregion

        
        void ISimplifiedStateControl.UpdateSimplifiedState(bool isSimplified)
        {
            this.IsSimplified = isSimplified;
        }

        
        void ILogicalChildSupport.AddLogicalChild(object child)
        {
            this.AddLogicalChild(child);
        }

        
        void ILogicalChildSupport.RemoveLogicalChild(object child)
        {
            this.RemoveLogicalChild(child);
        }

        
        protected override IEnumerator LogicalChildren
        {
            get
            {
                var baseEnumerator = base.LogicalChildren;
                while (baseEnumerator?.MoveNext() == true)
                {
                    yield return baseEnumerator.Current;
                }

                if (this.Icon != null)
                {
                    yield return this.Icon;
                }

                if (this.MediumIcon != null)
                {
                    yield return this.MediumIcon;
                }

                if (this.Header != null)
                {
                    yield return this.Header;
                }
            }
        }

        
        protected override AutomationPeer OnCreateAutomationPeer() => new WzUXRibbon.Automation.Peers.RibbonTextBoxAutomationPeer(this);
    }
}
