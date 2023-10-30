using System.Collections;
using System.Diagnostics;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows;
using WzUXRibbon.AttachedProperties;
using WzUXRibbon.Data;
using WzUXRibbon.Enumerations;
using WzUXRibbon.Internal.KnownBoxes;
using WzUXRibbon.Helpers;

namespace WzUXRibbon.Controls
{
    [ContentProperty(nameof(Header))]
    [DebuggerDisplay("{GetType().FullName}: Header = {Header}, Size = {Size}, IsSimplified = {IsSimplified}")]
    public class Button : System.Windows.Controls.Button, IRibbonControl, ILargeIconProvider, IMediumIconProvider, ISimplifiedRibbonControl
    {
        #region Properties

        #region Size
        public RibbonControlSize Size
        {
            get { return (RibbonControlSize)this.GetValue(SizeProperty); }
            set { this.SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty = RibbonProperties.SizeProperty.AddOwner(typeof(Button));

        #endregion

        #region SizeDefinition

        public RibbonControlSizeDefinition SizeDefinition
        {
            get { return (RibbonControlSizeDefinition)this.GetValue(SizeDefinitionProperty); }
            set { this.SetValue(SizeDefinitionProperty, value); }
        }

        public static readonly DependencyProperty SizeDefinitionProperty = RibbonProperties.SizeDefinitionProperty.AddOwner(typeof(Button));

        #endregion

        #region SimplifiedSizeDefinition

        
        public RibbonControlSizeDefinition SimplifiedSizeDefinition
        {
            get { return (RibbonControlSizeDefinition)this.GetValue(SimplifiedSizeDefinitionProperty); }
            set { this.SetValue(SimplifiedSizeDefinitionProperty, value); }
        }

        public static readonly DependencyProperty SimplifiedSizeDefinitionProperty = RibbonProperties.SimplifiedSizeDefinitionProperty.AddOwner(typeof(Button));

        #endregion

        #region KeyTip

        public string KeyTip
        {
            get { return (string)this.GetValue(KeyTipProperty); }
            set { this.SetValue(KeyTipProperty, value); }
        }

        public static readonly DependencyProperty KeyTipProperty = WzUXRibbon.Controls.KeyTip.KeysProperty.AddOwner(typeof(Button));

        #endregion

        #region Header

        public object Header
        {
            get { return this.GetValue(HeaderProperty); }
            set { this.SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = WzUXRibbon.Controls.RibbonControl.HeaderProperty.AddOwner(typeof(Button), new PropertyMetadata(LogicalChildSupportHelper.OnLogicalChildPropertyChanged));

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)this.GetValue(HeaderTemplateProperty); }
            set { this.SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty = RibbonControl.HeaderTemplateProperty.AddOwner(typeof(Button), new PropertyMetadata());

        public DataTemplateSelector HeaderTemplateSelector
        {
            get { return (DataTemplateSelector)this.GetValue(HeaderTemplateSelectorProperty); }
            set { this.SetValue(HeaderTemplateSelectorProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateSelectorProperty = RibbonControl.HeaderTemplateSelectorProperty.AddOwner(typeof(Button), new PropertyMetadata());

        #endregion

        #region Icon

        public object Icon
        {
            get { return this.GetValue(IconProperty); }
            set { this.SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = RibbonControl.IconProperty.AddOwner(typeof(Button), new PropertyMetadata(LogicalChildSupportHelper.OnLogicalChildPropertyChanged));

        #endregion

        #region LargeIcon

        public object LargeIcon
        {
            get { return this.GetValue(LargeIconProperty); }
            set { this.SetValue(LargeIconProperty, value); }
        }

        public static readonly DependencyProperty LargeIconProperty = LargeIconProviderProperties.LargeIconProperty.AddOwner(typeof(Button), new PropertyMetadata(LogicalChildSupportHelper.OnLogicalChildPropertyChanged));

        #endregion

        #region MediumIcon

        public object MediumIcon
        {
            get { return this.GetValue(MediumIconProperty); }
            set { this.SetValue(MediumIconProperty, value); }
        }

        public static readonly DependencyProperty MediumIconProperty = MediumIconProviderProperties.MediumIconProperty.AddOwner(typeof(Button), new PropertyMetadata(LogicalChildSupportHelper.OnLogicalChildPropertyChanged));

        #endregion

        #region IsDefinitive

        public bool IsDefinitive
        {
            get { return (bool)this.GetValue(IsDefinitiveProperty); }
            set { this.SetValue(IsDefinitiveProperty, BooleanBoxes.Box(value)); }
        }

        public static readonly DependencyProperty IsDefinitiveProperty =
            DependencyProperty.Register(nameof(IsDefinitive), typeof(bool), typeof(Button), new PropertyMetadata(BooleanBoxes.TrueBox));

        #endregion

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
            DependencyProperty.RegisterReadOnly(nameof(IsSimplified), typeof(bool), typeof(Button), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>Identifies the <see cref="IsSimplified"/> dependency property.</summary>
        public static readonly DependencyProperty IsSimplifiedProperty = IsSimplifiedPropertyKey.DependencyProperty;

        #endregion

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Static constructor
        /// </summary>
        static Button()
        {
            var type = typeof(Button);
            DefaultStyleKeyProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(type));
            WzUXRibbon.Services.ContextMenuService.Attach(type);
            WzUXRibbon.Services.ToolTipService.Attach(type);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Button()
        {
            WzUXRibbon.Services.ContextMenuService.Coerce(this);
        }

        #endregion

        #region Overrides

        
        protected override void OnClick()
        {
            // Close popup on click
            if (this.IsDefinitive)
            {
                WzUXRibbon.Services.PopupService.RaiseDismissPopupEvent(this, WzUXRibbon.Services.DismissPopupMode.Always);
            }

            base.OnClick();
        }

        #endregion

        #region Quick Access Item Creating

        public virtual FrameworkElement CreateQuickAccessItem()
        {
            var button = new Button();
            button.Click += (sender, e) => this.RaiseEvent(e);
            RibbonControl.BindQuickAccessItem(this, button);
            return button;
        }

        public bool CanAddToQuickAccessToolBar
        {
            get { return (bool)this.GetValue(CanAddToQuickAccessToolBarProperty); }
            set { this.SetValue(CanAddToQuickAccessToolBarProperty, BooleanBoxes.Box(value)); }
        }

        public static readonly DependencyProperty CanAddToQuickAccessToolBarProperty = RibbonControl.CanAddToQuickAccessToolBarProperty.AddOwner(typeof(Button), new PropertyMetadata(BooleanBoxes.TrueBox, RibbonControl.OnCanAddToQuickAccessToolBarChanged));

        #endregion

        #region Implementation of IKeyTipedControl

        
        public KeyTipPressedResult OnKeyTipPressed()
        {
            this.OnClick();

            return KeyTipPressedResult.Empty;
        }

        
        public void OnKeyTipBack()
        {
        }

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

                if (this.LargeIcon != null)
                {
                    yield return this.LargeIcon;
                }

                if (this.Header != null)
                {
                    yield return this.Header;
                }
            }
        }

        protected override AutomationPeer OnCreateAutomationPeer() => new WzUXRibbon.Automation.Peers.RibbonButtonAutomationPeer(this);
    }
}
