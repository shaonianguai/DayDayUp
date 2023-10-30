using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;
using WzUXRibbon.AttachedProperties;
using WzUXRibbon.Data;
using WzUXRibbon.Enumerations;
using WzUXRibbon.Helpers;
using WzUXRibbon.Internal.KnownBoxes;
using WzUXRibbon.Services;

namespace WzUXRibbon.Controls
{

    [ContentProperty(nameof(Header))]
    [DebuggerDisplay("class{GetType().FullName}: Header = {Header}, IsChecked = {IsChecked}, Size = {Size}, IsSimplified = {IsSimplified}")]
    public class ToggleButton : System.Windows.Controls.Primitives.ToggleButton, IToggleButton, IRibbonControl, IQuickAccessItemProvider, ILargeIconProvider, IMediumIconProvider, ISimplifiedRibbonControl
    {
        #region Properties

        #region Size

        
        public RibbonControlSize Size
        {
            get { return (RibbonControlSize)this.GetValue(SizeProperty); }
            set { this.SetValue(SizeProperty, value); }
        }

        /// <summary>Identifies the <see cref="Size"/> dependency property.</summary>
        public static readonly DependencyProperty SizeProperty = RibbonProperties.SizeProperty.AddOwner(typeof(ToggleButton));

        #endregion

        #region SizeDefinition

        
        public RibbonControlSizeDefinition SizeDefinition
        {
            get { return (RibbonControlSizeDefinition)this.GetValue(SizeDefinitionProperty); }
            set { this.SetValue(SizeDefinitionProperty, value); }
        }

        /// <summary>Identifies the <see cref="SizeDefinition"/> dependency property.</summary>
        public static readonly DependencyProperty SizeDefinitionProperty = RibbonProperties.SizeDefinitionProperty.AddOwner(typeof(ToggleButton));

        #endregion

        #region SimplifiedSizeDefinition

        
        public RibbonControlSizeDefinition SimplifiedSizeDefinition
        {
            get { return (RibbonControlSizeDefinition)this.GetValue(SimplifiedSizeDefinitionProperty); }
            set { this.SetValue(SimplifiedSizeDefinitionProperty, value); }
        }

        /// <summary>Identifies the <see cref="SimplifiedSizeDefinition"/> dependency property.</summary>
        public static readonly DependencyProperty SimplifiedSizeDefinitionProperty = RibbonProperties.SimplifiedSizeDefinitionProperty.AddOwner(typeof(ToggleButton));

        #endregion

        #region KeyTip

        
        public string KeyTip
        {
            get { return (string)this.GetValue(KeyTipProperty); }
            set { this.SetValue(KeyTipProperty, value); }
        }

        /// <inheritdoc cref="KeyTip.KeysProperty"/>
        public static readonly DependencyProperty KeyTipProperty = WzUXRibbon.Controls.KeyTip.KeysProperty.AddOwner(typeof(ToggleButton));

        #endregion

        #region GroupName

        
        public string GroupName
        {
            get { return (string)this.GetValue(GroupNameProperty); }
            set { this.SetValue(GroupNameProperty, value); }
        }

        /// <summary>Identifies the <see cref="GroupName"/> dependency property.</summary>
        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register(nameof(GroupName), typeof(string), typeof(ToggleButton), new PropertyMetadata(ToggleButtonHelper.OnGroupNameChanged));

        #endregion

        #region Header

        
        public object Header
        {
            get { return this.GetValue(HeaderProperty); }
            set { this.SetValue(HeaderProperty, value); }
        }

        /// <summary>Identifies the <see cref="Header"/> dependency property.</summary>
        public static readonly DependencyProperty HeaderProperty = RibbonControl.HeaderProperty.AddOwner(typeof(ToggleButton), new PropertyMetadata(LogicalChildSupportHelper.OnLogicalChildPropertyChanged));

        
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)this.GetValue(HeaderTemplateProperty); }
            set { this.SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>Identifies the <see cref="HeaderTemplate"/> dependency property.</summary>
        public static readonly DependencyProperty HeaderTemplateProperty = RibbonControl.HeaderTemplateProperty.AddOwner(typeof(ToggleButton), new PropertyMetadata());

        
        public DataTemplateSelector HeaderTemplateSelector
        {
            get { return (DataTemplateSelector)this.GetValue(HeaderTemplateSelectorProperty); }
            set { this.SetValue(HeaderTemplateSelectorProperty, value); }
        }

        /// <summary>Identifies the <see cref="HeaderTemplateSelector"/> dependency property.</summary>
        public static readonly DependencyProperty HeaderTemplateSelectorProperty = RibbonControl.HeaderTemplateSelectorProperty.AddOwner(typeof(ToggleButton), new PropertyMetadata());

        #endregion

        #region Icon

        
        public object Icon
        {
            get { return this.GetValue(IconProperty); }
            set { this.SetValue(IconProperty, value); }
        }

        /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
        public static readonly DependencyProperty IconProperty = RibbonControl.IconProperty.AddOwner(typeof(ToggleButton), new PropertyMetadata(LogicalChildSupportHelper.OnLogicalChildPropertyChanged));

        #endregion

        #region LargeIcon

        
        public object LargeIcon
        {
            get { return this.GetValue(LargeIconProperty); }
            set { this.SetValue(LargeIconProperty, value); }
        }

        /// <summary>Identifies the <see cref="LargeIcon"/> dependency property.</summary>
        public static readonly DependencyProperty LargeIconProperty = LargeIconProviderProperties.LargeIconProperty.AddOwner(typeof(ToggleButton), new PropertyMetadata(LogicalChildSupportHelper.OnLogicalChildPropertyChanged));

        #endregion

        #region MediumIcon

        
        public object MediumIcon
        {
            get { return this.GetValue(MediumIconProperty); }
            set { this.SetValue(MediumIconProperty, value); }
        }

        /// <summary>Identifies the <see cref="MediumIcon"/> dependency property.</summary>
        public static readonly DependencyProperty MediumIconProperty = MediumIconProviderProperties.MediumIconProperty.AddOwner(typeof(ToggleButton), new PropertyMetadata(LogicalChildSupportHelper.OnLogicalChildPropertyChanged));

        #endregion

        #region IsDefinitive

        /// <summary>
        /// Gets or sets whether ribbon control click must close backstage
        /// </summary>
        public bool IsDefinitive
        {
            get { return (bool)this.GetValue(IsDefinitiveProperty); }
            set { this.SetValue(IsDefinitiveProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="IsDefinitive"/> dependency property.</summary>
        public static readonly DependencyProperty IsDefinitiveProperty =
            DependencyProperty.Register(nameof(IsDefinitive), typeof(bool), typeof(ToggleButton), new PropertyMetadata(BooleanBoxes.TrueBox));

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
            DependencyProperty.RegisterReadOnly(nameof(IsSimplified), typeof(bool), typeof(ToggleButton), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>Identifies the <see cref="IsSimplified"/> dependency property.</summary>
        public static readonly DependencyProperty IsSimplifiedProperty = IsSimplifiedPropertyKey.DependencyProperty;

        #endregion

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="ToggleButton"/> class.
        /// </summary>
        static ToggleButton()
        {
            var type = typeof(ToggleButton);
            DefaultStyleKeyProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(type));
            WzUXRibbon.Services.ContextMenuService.Attach(type);
            WzUXRibbon.Services.ToolTipService.Attach(type);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleButton"/> class.
        /// </summary>
        public ToggleButton()
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
                PopupService.RaiseDismissPopupEvent(this, DismissPopupMode.Always);
            }

            // fix for #481
            // We can't overwrite OnToggle because it's "internal protected"...
            if (string.IsNullOrEmpty(this.GroupName) == false)
            {
                // Only forward click if button is not checked to prevent wrong bound values
                if (this.IsChecked == false)
                {
                    base.OnClick();
                }
            }
            else
            {
                base.OnClick();
            }
        }

        
        protected override void OnChecked(RoutedEventArgs e)
        {
            ToggleButtonHelper.UpdateButtonGroup(this);
            base.OnChecked(e);
        }

        #endregion

        /// <summary>
        /// Used to call OnClick (which is protected)
        /// </summary>
        public void InvokeClick()
        {
            this.OnClick();
        }

        #region Quick Access Item Creating

        
        public virtual FrameworkElement CreateQuickAccessItem()
        {
            var button = new ToggleButton();

            RibbonControl.Bind(this, button, nameof(this.IsChecked), IsCheckedProperty, BindingMode.TwoWay);
            button.Click += (sender, e) => this.RaiseEvent(e);
            RibbonControl.BindQuickAccessItem(this, button);

            return button;
        }

        
        public bool CanAddToQuickAccessToolBar
        {
            get { return (bool)this.GetValue(CanAddToQuickAccessToolBarProperty); }
            set { this.SetValue(CanAddToQuickAccessToolBarProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="CanAddToQuickAccessToolBar"/> dependency property.</summary>
        public static readonly DependencyProperty CanAddToQuickAccessToolBarProperty = RibbonControl.CanAddToQuickAccessToolBarProperty.AddOwner(typeof(ToggleButton), new PropertyMetadata(BooleanBoxes.TrueBox, RibbonControl.OnCanAddToQuickAccessToolBarChanged));

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

        
        protected override AutomationPeer OnCreateAutomationPeer() => new WzUXRibbon.Automation.Peers.RibbonToggleButtonAutomationPeer(this);
    }
}
