﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using WzUXRibbon.Collections;
using WzUXRibbon.Data;
using WzUXRibbon.Helpers;
using WzUXRibbon.Internal.KnownBoxes;

namespace WzUXRibbon.Controls
{
    [ContentProperty(nameof(Tabs))]
    [DefaultProperty(nameof(Tabs))]
    [TemplatePart(Name = "PART_LayoutRoot", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_RibbonTabControl", Type = typeof(RibbonTabControl))]
    [TemplatePart(Name = "PART_QuickAccessToolBar", Type = typeof(QuickAccessToolBar))]
    public class Ribbon : Control, ILogicalChildSupport
    {
        #region Constants

        /// <summary>
        /// Minimal width of ribbon parent window
        /// </summary>
        public const double MinimalVisibleWidth = 300;

        /// <summary>
        /// Minimal height of ribbon parent window
        /// </summary>
        public const double MinimalVisibleHeight = 250;

        #endregion

        #region ContextMenu

        /// <summary>Identifies the <see cref="IsDefaultContextMenuEnabled"/> dependency property.</summary>
        public static readonly DependencyProperty IsDefaultContextMenuEnabledProperty = DependencyProperty.Register(nameof(IsDefaultContextMenuEnabled), typeof(bool), typeof(Ribbon), new PropertyMetadata(BooleanBoxes.TrueBox));

        /// <summary>
        /// Gets or sets whether the default context menu should be enabled/used.
        /// </summary>
        public bool IsDefaultContextMenuEnabled
        {
            get { return (bool)this.GetValue(IsDefaultContextMenuEnabledProperty); }
            set { this.SetValue(IsDefaultContextMenuEnabledProperty, BooleanBoxes.Box(value)); }
        }

        private static readonly Dictionary<int, System.Windows.Controls.ContextMenu> contextMenus = new Dictionary<int, System.Windows.Controls.ContextMenu>();

        /// <summary>
        /// Context menu for ribbon in current thread
        /// </summary>
        public static System.Windows.Controls.ContextMenu RibbonContextMenu
        {
            get
            {
                if (!contextMenus.ContainsKey(Thread.CurrentThread.ManagedThreadId))
                {
                    InitRibbonContextMenu();
                }

                return contextMenus[Thread.CurrentThread.ManagedThreadId];
            }
        }

        // Context menu owner ribbon
        private static Ribbon contextMenuOwner;

        // Context menu items
        private static readonly Dictionary<int, System.Windows.Controls.MenuItem> addToQuickAccessMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();

        private static System.Windows.Controls.MenuItem AddToQuickAccessMenuItem
        {
            get { return addToQuickAccessMenuItemDictionary[Thread.CurrentThread.ManagedThreadId]; }
        }

        private static readonly Dictionary<int, System.Windows.Controls.MenuItem> addGroupToQuickAccessMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();

        private static System.Windows.Controls.MenuItem AddGroupToQuickAccessMenuItem
        {
            get { return addGroupToQuickAccessMenuItemDictionary[Thread.CurrentThread.ManagedThreadId]; }
        }

        private static readonly Dictionary<int, System.Windows.Controls.MenuItem> addMenuToQuickAccessMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();

        private static System.Windows.Controls.MenuItem AddMenuToQuickAccessMenuItem
        {
            get { return addMenuToQuickAccessMenuItemDictionary[Thread.CurrentThread.ManagedThreadId]; }
        }

        private static readonly Dictionary<int, System.Windows.Controls.MenuItem> addGalleryToQuickAccessMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();

        private static System.Windows.Controls.MenuItem AddGalleryToQuickAccessMenuItem
        {
            get { return addGalleryToQuickAccessMenuItemDictionary[Thread.CurrentThread.ManagedThreadId]; }
        }

        private static readonly Dictionary<int, System.Windows.Controls.MenuItem> removeFromQuickAccessMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();

        private static System.Windows.Controls.MenuItem RemoveFromQuickAccessMenuItem
        {
            get { return removeFromQuickAccessMenuItemDictionary[Thread.CurrentThread.ManagedThreadId]; }
        }

        private static readonly Dictionary<int, System.Windows.Controls.MenuItem> showQuickAccessToolbarBelowTheRibbonMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();

        private static System.Windows.Controls.MenuItem ShowQuickAccessToolbarBelowTheRibbonMenuItem
        {
            get { return showQuickAccessToolbarBelowTheRibbonMenuItemDictionary[Thread.CurrentThread.ManagedThreadId]; }
        }

        private static readonly Dictionary<int, System.Windows.Controls.MenuItem> showQuickAccessToolbarAboveTheRibbonMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();

        private static System.Windows.Controls.MenuItem ShowQuickAccessToolbarAboveTheRibbonMenuItem
        {
            get { return showQuickAccessToolbarAboveTheRibbonMenuItemDictionary[Thread.CurrentThread.ManagedThreadId]; }
        }

        private static readonly Dictionary<int, System.Windows.Controls.MenuItem> minimizeTheRibbonMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();

        private static System.Windows.Controls.MenuItem MinimizeTheRibbonMenuItem
        {
            get { return minimizeTheRibbonMenuItemDictionary[Thread.CurrentThread.ManagedThreadId]; }
        }

        private static readonly Dictionary<int, System.Windows.Controls.MenuItem> useTheClassicRibbonMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();

        private static System.Windows.Controls.MenuItem UseTheClassicRibbonMenuItem
        {
            get { return useTheClassicRibbonMenuItemDictionary[Thread.CurrentThread.ManagedThreadId]; }
        }

        private static readonly Dictionary<int, System.Windows.Controls.MenuItem> useTheSimplifiedRibbonMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();

        private static System.Windows.Controls.MenuItem UseTheSimplifiedRibbonMenuItem
        {
            get { return useTheSimplifiedRibbonMenuItemDictionary[Thread.CurrentThread.ManagedThreadId]; }
        }

        private static readonly Dictionary<int, System.Windows.Controls.MenuItem> customizeQuickAccessToolbarMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();

        private static System.Windows.Controls.MenuItem CustomizeQuickAccessToolbarMenuItem
        {
            get { return customizeQuickAccessToolbarMenuItemDictionary[Thread.CurrentThread.ManagedThreadId]; }
        }

        private static readonly Dictionary<int, System.Windows.Controls.MenuItem> customizeTheRibbonMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();

        private static System.Windows.Controls.MenuItem CustomizeTheRibbonMenuItem
        {
            get { return customizeTheRibbonMenuItemDictionary[Thread.CurrentThread.ManagedThreadId]; }
        }

        private static readonly Dictionary<int, Separator> firstSeparatorDictionary = new Dictionary<int, Separator>();

        private static Separator FirstSeparator
        {
            get { return firstSeparatorDictionary[Thread.CurrentThread.ManagedThreadId]; }
        }

        private static readonly Dictionary<int, Separator> secondSeparatorDictionary = new Dictionary<int, Separator>();

        private static Separator SecondSeparator
        {
            get { return secondSeparatorDictionary[Thread.CurrentThread.ManagedThreadId]; }
        }

        // Initialize ribbon context menu
        private static void InitRibbonContextMenu()
        {
            contextMenus.Add(Thread.CurrentThread.ManagedThreadId, new ContextMenu());
            RibbonContextMenu.Opened += OnContextMenuOpened;
        }

        private static void InitRibbonContextMenuItems()
        {
        }

        private static MenuItem CreateMenuItemForContextMenu(ICommand command)
        {
            return new MenuItem
            {
                Command = command,
                CanAddToQuickAccessToolBar = false,
                ContextMenu = null
            };
        }

        /// <inheritdoc />
        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            contextMenuOwner = this;
            base.OnContextMenuOpening(e);
        }

        /// <inheritdoc />
        protected override void OnContextMenuClosing(ContextMenuEventArgs e)
        {
            contextMenuOwner = null;
            base.OnContextMenuClosing(e);
        }

        private void OnQuickAccessContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            this.OnContextMenuOpening(e);
        }

        private void OnQuickAccessContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            this.OnContextMenuClosing(e);
        }

        // Occurs when context menu is opening
        private static void OnContextMenuOpened(object sender, RoutedEventArgs e)
        {
            var ribbon = contextMenuOwner;

            if (RibbonContextMenu is null
                || ribbon is null)
            {
                return;
            }

            if (ribbon.IsDefaultContextMenuEnabled
                && RibbonContextMenu.Items.Count == 0)
            {
                InitRibbonContextMenuItems();
            }

            if (ribbon.IsDefaultContextMenuEnabled == false)
            {
                foreach (var item in RibbonContextMenu.Items.OfType<UIElement>())
                {
                    item.Visibility = Visibility.Collapsed;
                }

                RibbonContextMenu.IsOpen = false;
                return;
            }

            //AddToQuickAccessMenuItem.CommandTarget = ribbon;
            //AddGroupToQuickAccessMenuItem.CommandTarget = ribbon;
            //AddMenuToQuickAccessMenuItem.CommandTarget = ribbon;
            //AddGalleryToQuickAccessMenuItem.CommandTarget = ribbon;
            //RemoveFromQuickAccessMenuItem.CommandTarget = ribbon;
            //CustomizeQuickAccessToolbarMenuItem.CommandTarget = ribbon;
            //CustomizeTheRibbonMenuItem.CommandTarget = ribbon;
            //MinimizeTheRibbonMenuItem.CommandTarget = ribbon;
            //UseTheClassicRibbonMenuItem.CommandTarget = ribbon;
            //UseTheSimplifiedRibbonMenuItem.CommandTarget = ribbon;
            //ShowQuickAccessToolbarBelowTheRibbonMenuItem.CommandTarget = ribbon;
            //ShowQuickAccessToolbarAboveTheRibbonMenuItem.CommandTarget = ribbon;

            // Hide items for ribbon controls
            //AddToQuickAccessMenuItem.Visibility = Visibility.Collapsed;
            //AddGroupToQuickAccessMenuItem.Visibility = Visibility.Collapsed;
            //AddMenuToQuickAccessMenuItem.Visibility = Visibility.Collapsed;
            //AddGalleryToQuickAccessMenuItem.Visibility = Visibility.Collapsed;
            //RemoveFromQuickAccessMenuItem.Visibility = Visibility.Collapsed;
            //FirstSeparator.Visibility = Visibility.Collapsed;

            //// Hide customize quick access menu item
            //CustomizeQuickAccessToolbarMenuItem.Visibility = Visibility.Collapsed;
            //SecondSeparator.Visibility = Visibility.Collapsed;

            //// Set use the classic ribbon menu item visibility
            //UseTheClassicRibbonMenuItem.Visibility = (ribbon.CanUseSimplified && ribbon.IsSimplified)
            //    ? Visibility.Visible
            //    : Visibility.Collapsed;

            //// Set use the simplified ribbon menu item visibility
            //UseTheSimplifiedRibbonMenuItem.Visibility = (ribbon.CanUseSimplified && !ribbon.IsSimplified)
            //    ? Visibility.Visible
            //    : Visibility.Collapsed;

            // Set minimize the ribbon menu item state
            //MinimizeTheRibbonMenuItem.IsChecked = ribbon.IsMinimized;

            //// Set minimize the ribbon menu item visibility
            //MinimizeTheRibbonMenuItem.Visibility = ribbon.CanMinimize
            //    ? Visibility.Visible
            //    : Visibility.Collapsed;

            //// Set customize the ribbon menu item visibility
            //CustomizeTheRibbonMenuItem.Visibility = ribbon.CanCustomizeRibbon
            //    ? Visibility.Visible
            //    : Visibility.Collapsed;

            //// Hide quick access position menu items
            //ShowQuickAccessToolbarBelowTheRibbonMenuItem.Visibility = Visibility.Collapsed;
            //ShowQuickAccessToolbarAboveTheRibbonMenuItem.Visibility = Visibility.Collapsed;

            // If quick access toolbar is visible show
            //if (ribbon.IsQuickAccessToolBarVisible)
            //{
            //    // Set quick access position menu items visibility
            //    if (ribbon.CanQuickAccessLocationChanging)
            //    {
            //        if (ribbon.ShowQuickAccessToolBarAboveRibbon)
            //        {
            //            ShowQuickAccessToolbarBelowTheRibbonMenuItem.Visibility = Visibility.Visible;
            //        }
            //        else
            //        {
            //            ShowQuickAccessToolbarAboveTheRibbonMenuItem.Visibility = Visibility.Visible;
            //        }
            //    }

            //    if (ribbon.CanCustomizeQuickAccessToolBar)
            //    {
            //        CustomizeQuickAccessToolbarMenuItem.Visibility = Visibility.Visible;
            //    }

            //    if (ribbon.CanQuickAccessLocationChanging
            //        || ribbon.CanCustomizeQuickAccessToolBar)
            //    {
            //        SecondSeparator.Visibility = Visibility.Visible;
            //    }
            //    else
            //    {
            //        SecondSeparator.Visibility = Visibility.Collapsed;
            //    }

            //    if (ribbon.CanCustomizeQuickAccessToolBarItems)
            //    {
            //        // Gets control that raise menu opened
            //        var control = RibbonContextMenu.PlacementTarget;
            //        AddToQuickAccessCommand.CanExecute(null, control);
            //        RemoveFromQuickAccessCommand.CanExecute(null, control);

            //        //Debug.WriteLine("Menu opened on "+control);
            //        if (control != null)
            //        {
            //            FirstSeparator.Visibility = Visibility.Visible;

            //            // Check for value because remove is only possible in the context menu of items in QA which represent the value for QA-items
            //            if (ribbon.QuickAccessElements.ContainsValue(control))
            //            {
            //                // Control is on quick access
            //                RemoveFromQuickAccessMenuItem.Visibility = Visibility.Visible;
            //            }
            //            else if (control is System.Windows.Controls.MenuItem)
            //            {
            //                // Control is menu item
            //                AddMenuToQuickAccessMenuItem.Visibility = Visibility.Visible;
            //            }
            //            else if (control is Gallery ||
            //                     control is InRibbonGallery)
            //            {
            //                // Control is gallery
            //                AddGalleryToQuickAccessMenuItem.Visibility = Visibility.Visible;
            //            }
            //            else if (control is RibbonGroupBox)
            //            {
            //                // Control is group box
            //                AddGroupToQuickAccessMenuItem.Visibility = Visibility.Visible;
            //            }
            //            else if (control is IQuickAccessItemProvider)
            //            {
            //                // Its other control
            //                AddToQuickAccessMenuItem.Visibility = Visibility.Visible;
            //            }
            //            else
            //            {
            //                FirstSeparator.Visibility = Visibility.Collapsed;
            //            }
            //        }
            //    }
            //}

            // We have to close the context menu if no items are visible
            if (RibbonContextMenu.Items.OfType<System.Windows.Controls.MenuItem>().All(x => x.Visibility == Visibility.Collapsed))
            {
                RibbonContextMenu.IsOpen = false;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when selected tab has been changed (be aware that SelectedTab can be null)
        /// </summary>
        public event SelectionChangedEventHandler SelectedTabChanged;

        /// <summary>
        /// Occurs when customize the ribbon
        /// </summary>
        public event EventHandler CustomizeTheRibbon;

        /// <summary>
        /// Occurs when customize quick access toolbar
        /// </summary>
        public event EventHandler CustomizeQuickAccessToolbar;

        /// <summary>
        /// Occurs when IsMinimized property is changing
        /// </summary>
        public event DependencyPropertyChangedEventHandler IsMinimizedChanged;

        /// <summary>
        /// Occurs when IsCollapsed property is changing
        /// </summary>
        public event DependencyPropertyChangedEventHandler IsCollapsedChanged;

        #endregion

        #region Fields

        private ObservableCollection<Key> keyTipKeys;

        // Collection of contextual tab groups
        private ObservableCollection<RibbonContextualTabGroup> contextualGroups;

        // Collection of tabs
        private ObservableCollection<RibbonTabItem> tabs;
        private CollectionSyncHelper<RibbonTabItem> tabsSync;

        // Collection of toolbar items
        private ObservableCollection<UIElement> toolBarItems;
        private CollectionSyncHelper<UIElement> toolBarItemsSync;

        // Ribbon quick access toolbar

        // Ribbon layout root
        private Panel layoutRoot;

        // Handles F10, Alt and so on
        private readonly WzUXRibbon.Services.KeyTipService keyTipService;

        // Collection of quick-access menu items
        private ObservableCollection<QuickAccessMenuItem> quickAccessItems;
        private CollectionSyncHelper<QuickAccessMenuItem> quickAccessItemsSync;

        // Currently added in QAT items

        private Window ownerWindow;

        #endregion

        #region Properties

        #region Menu

        /// <summary>
        /// Gets or sets file menu control (can be application menu button, backstage button and so on)
        /// </summary>
        public FrameworkElement Menu
        {
            get { return (FrameworkElement)this.GetValue(MenuProperty); }
            set { this.SetValue(MenuProperty, value); }
        }

        /// <summary>Identifies the <see cref="Menu"/> dependency property.</summary>
        public static readonly DependencyProperty MenuProperty =
            DependencyProperty.Register(nameof(Menu), typeof(FrameworkElement), typeof(Ribbon), new FrameworkPropertyMetadata(default(FrameworkElement), FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, OnMenuChanged));

        private static void OnMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AddOrRemoveLogicalChildOnPropertyChanged(d, e);
        }

        #endregion

        #region QuickAccessToolBar

        /// <summary>
        /// Property for defining the QuickAccessToolBar.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public QuickAccessToolBar QuickAccessToolBar
        {
            get { return (QuickAccessToolBar)this.GetValue(QuickAccessToolBarProperty); }
            private set { this.SetValue(QuickAccessToolBarPropertyKey, value); }
        }

        // ReSharper disable once InconsistentNaming
        private static readonly DependencyPropertyKey QuickAccessToolBarPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(QuickAccessToolBar), typeof(QuickAccessToolBar), typeof(Ribbon), new FrameworkPropertyMetadata(default(QuickAccessToolBar), FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, OnQuickAccessToolBarChanged));

        private static void OnQuickAccessToolBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AddOrRemoveLogicalChildOnPropertyChanged(d, e);
        }

        /// <summary>Identifies the <see cref="QuickAccessToolBar"/> dependency property.</summary>
        public static readonly DependencyProperty QuickAccessToolBarProperty = QuickAccessToolBarPropertyKey.DependencyProperty;

        #endregion

        #region TabControl

        /// <summary>
        /// Property for defining the TabControl.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RibbonTabControl TabControl
        {
            get { return (RibbonTabControl)this.GetValue(TabControlProperty); }
            private set { this.SetValue(TabControlPropertyKey, value); }
        }

        // ReSharper disable once InconsistentNaming
        private static readonly DependencyPropertyKey TabControlPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(TabControl), typeof(RibbonTabControl), typeof(Ribbon),
                new FrameworkPropertyMetadata(default(RibbonTabControl), FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, LogicalChildSupportHelper.OnLogicalChildPropertyChanged));

        /// <summary>Identifies the <see cref="TabControl"/> dependency property.</summary>
        public static readonly DependencyProperty TabControlProperty = TabControlPropertyKey.DependencyProperty;

        #endregion

        #region IsSimplified

        /// <summary>
        /// Gets or sets whether or not the ribbon is in Simplified mode
        /// </summary>
        public bool IsSimplified
        {
            get { return (bool)this.GetValue(IsSimplifiedProperty); }
            set { this.SetValue(IsSimplifiedProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="IsSimplified"/> dependency property.</summary>
        public static readonly DependencyProperty IsSimplifiedProperty = DependencyProperty.Register(nameof(IsSimplified), typeof(bool), typeof(Ribbon),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIsSimplifiedChanged));

        private static void OnIsSimplifiedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Ribbon ribbon)
            {
                var isSimplified = ribbon.IsSimplified;
                foreach (var item in ribbon.Tabs.OfType<ISimplifiedStateControl>())
                {
                    item.UpdateSimplifiedState(isSimplified);
                }
            }
        }
        #endregion

        /// <summary>
        /// Gets or sets selected tab item
        /// </summary>
        public RibbonTabItem SelectedTabItem
        {
            get { return (RibbonTabItem)this.GetValue(SelectedTabItemProperty); }
            set { this.SetValue(SelectedTabItemProperty, value); }
        }

        /// <summary>Identifies the <see cref="SelectedTabItem"/> dependency property.</summary>
        public static readonly DependencyProperty SelectedTabItemProperty =
            DependencyProperty.Register(nameof(SelectedTabItem), typeof(RibbonTabItem), typeof(Ribbon), new PropertyMetadata(OnSelectedTabItemChanged));

        private static void OnSelectedTabItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ribbon = (Ribbon)d;
            if (ribbon.TabControl != null)
            {
                ribbon.TabControl.SelectedItem = e.NewValue;
            }

            if (e.NewValue is RibbonTabItem selectedItem
                && ribbon.Tabs.Contains(selectedItem))
            {
                ribbon.SelectedTabIndex = ribbon.Tabs.IndexOf(selectedItem);
            }
            else
            {
                ribbon.SelectedTabIndex = -1;
            }
        }

        /// <summary>
        /// Gets or sets selected tab index
        /// </summary>
        public int SelectedTabIndex
        {
            get { return (int)this.GetValue(SelectedTabIndexProperty); }
            set { this.SetValue(SelectedTabIndexProperty, value); }
        }

        /// <summary>Identifies the <see cref="SelectedTabIndex"/> dependency property.</summary>
        public static readonly DependencyProperty SelectedTabIndexProperty =
            DependencyProperty.Register(nameof(SelectedTabIndex), typeof(int), typeof(Ribbon), new PropertyMetadata(-1, OnSelectedTabIndexChanged));

        private static void OnSelectedTabIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ribbon = (Ribbon)d;
            var selectedIndex = (int)e.NewValue;

            if (ribbon.TabControl != null)
            {
                ribbon.TabControl.SelectedIndex = selectedIndex;
            }

            if (selectedIndex >= 0
                && selectedIndex < ribbon.Tabs.Count)
            {
                ribbon.SelectedTabItem = ribbon.Tabs[selectedIndex];
            }
            else
            {
                ribbon.SelectedTabItem = null;
            }
        }

        private static void AddOrRemoveLogicalChildOnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ribbon = (Ribbon)d;
            if (e.OldValue != null)
            {
                ribbon.RemoveLogicalChild(e.OldValue);
            }

            if (e.NewValue != null)
            {
                ribbon.AddLogicalChild(e.NewValue);
            }
        }

        /// <summary>
        /// Gets the first visible TabItem
        /// </summary>
        public RibbonTabItem FirstVisibleItem => this.GetFirstVisibleItem();

        /// <summary>
        /// Gets the last visible TabItem
        /// </summary>
        public RibbonTabItem LastVisibleItem => this.GetLastVisibleItem();

        /// <summary>
        /// Gets currently active quick access elements.
        /// </summary>
        protected Dictionary<UIElement, UIElement> QuickAccessElements { get; } = new Dictionary<UIElement, UIElement>();

        /// <summary>
        /// Gets a copy of currently active quick access elements.
        /// </summary>
        public IDictionary<UIElement, UIElement> GetQuickAccessElements() => this.QuickAccessElements.ToDictionary(x => x.Key, y => y.Value);

        #region TitelBar

        /// <summary>
        /// Gets ribbon titlebar
        /// </summary>
        public RibbonTitleBar TitleBar
        {
            get { return (RibbonTitleBar)this.GetValue(TitleBarProperty); }
            set { this.SetValue(TitleBarProperty, value); }
        }

        /// <summary>Identifies the <see cref="TitleBar"/> dependency property.</summary>
        public static readonly DependencyProperty TitleBarProperty = DependencyProperty.Register(nameof(TitleBar), typeof(RibbonTitleBar), typeof(Ribbon),
            new PropertyMetadata(OnTitleBarChanged));

        private static void OnTitleBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ribbon = (Ribbon)d;

            if (e.OldValue is RibbonTitleBar oldValue)
            {
                oldValue.ItemsSource = null;

                ribbon.RemoveQuickAccessToolBarFromTitleBar(oldValue);
            }

            if (e.NewValue is RibbonTitleBar newValue)
            {
                newValue.ItemsSource = ribbon.ContextualGroups;

                if (ribbon.ShowQuickAccessToolBarAboveRibbon)
                {
                    ribbon.MoveQuickAccessToolBarToTitleBar(newValue);
                }
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets whether quick access toolbar shows above ribbon
        /// </summary>
        public bool ShowQuickAccessToolBarAboveRibbon
        {
            get { return (bool)this.GetValue(ShowQuickAccessToolBarAboveRibbonProperty); }
            set { this.SetValue(ShowQuickAccessToolBarAboveRibbonProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="ShowQuickAccessToolBarAboveRibbon"/> dependency property.</summary>
        public static readonly DependencyProperty ShowQuickAccessToolBarAboveRibbonProperty =
            DependencyProperty.Register(nameof(ShowQuickAccessToolBarAboveRibbon), typeof(bool), typeof(Ribbon),
                new PropertyMetadata(BooleanBoxes.TrueBox, OnShowQuickAccessToolBarAboveRibbonChanged));

        /// <summary>
        /// Handles ShowQuickAccessToolBarAboveRibbon property changed
        /// </summary>
        /// <param name="d">Object</param>
        /// <param name="e">The event data</param>
        private static void OnShowQuickAccessToolBarAboveRibbonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ribbon = (Ribbon)d;

            if (ribbon.TitleBar != null)
            {
                if ((bool)e.NewValue)
                {
                    ribbon.MoveQuickAccessToolBarToTitleBar(ribbon.TitleBar);
                }
                else
                {
                    ribbon.RemoveQuickAccessToolBarFromTitleBar(ribbon.TitleBar);
                }

                ribbon.TitleBar.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Gets or sets the height which is used to render the window title.
        /// </summary>
        public double QuickAccessToolBarHeight
        {
            get { return (double)this.GetValue(QuickAccessToolBarHeightProperty); }
            set { this.SetValue(QuickAccessToolBarHeightProperty, value); }
        }

        /// <summary>Identifies the <see cref="QuickAccessToolBarHeight"/> dependency property.</summary>
        public static readonly DependencyProperty QuickAccessToolBarHeightProperty =
            DependencyProperty.Register(nameof(QuickAccessToolBarHeight), typeof(double), typeof(Ribbon), new PropertyMetadata(23D));

        /// <summary>
        /// Gets collection of contextual tab groups
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<RibbonContextualTabGroup> ContextualGroups
        {
            get
            {
                if (this.contextualGroups == null)
                {
                    this.contextualGroups = new ObservableCollection<RibbonContextualTabGroup>();
                    return this.contextualGroups;
                }
                else
                {
                    return this.contextualGroups;
                }
            }
        }

        #region Tabs

        /// <summary>
        /// gets collection of ribbon tabs
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<RibbonTabItem> Tabs
        {
            get
            {
                if (this.tabs is null)
                {
                    this.tabs = new ObservableCollection<RibbonTabItem>();
                    this.tabs.CollectionChanged += this.OnTabItemsCollectionChanged;
                }

                return this.tabs;
            }
        }

        /// <summary>
        /// Handles collection of ribbon tab items changes
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">The event data</param>
        private void OnTabItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:
                    {
                        var isSimplified = this.IsSimplified;
                        foreach (var item in e.NewItems.OfType<ISimplifiedStateControl>())
                        {
                            item.UpdateSimplifiedState(isSimplified);
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    {
                        var isSimplified = this.IsSimplified;
                        foreach (var item in this.Tabs.OfType<ISimplifiedStateControl>())
                        {
                            item.UpdateSimplifiedState(isSimplified);
                        }
                    }

                    break;
            }
        }
        #endregion

        /// <summary>
        /// Gets collection of toolbar items
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<UIElement> ToolBarItems
        {
            get
            {
                if (this.toolBarItems != null)
                {
                    return this.toolBarItems;
                }
                else
                {
                    this.toolBarItems = new ObservableCollection<UIElement>();
                    return this.toolBarItems;
                }
            }
        }

        /// <summary>
        /// Gets collection of quick access menu items
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<QuickAccessMenuItem> QuickAccessItems
        {
            get
            {
                if (this.quickAccessItems is null)
                {
                    this.quickAccessItems = new ObservableCollection<QuickAccessMenuItem>();
                    this.quickAccessItems.CollectionChanged += this.OnQuickAccessItemsCollectionChanged;
                }

                return this.quickAccessItems;
            }
        }

        /// <summary>
        /// Handles collection of quick access menu items changes
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">The event data</param>
        private void OnQuickAccessItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems.OfType<QuickAccessMenuItem>())
                    {
                        item.Ribbon = this;
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems.OfType<QuickAccessMenuItem>())
                    {
                        item.Ribbon = null;
                    }

                    break;

                case NotifyCollectionChangedAction.Replace:
                    foreach (var item in e.OldItems.OfType<QuickAccessMenuItem>())
                    {
                        item.Ribbon = null;
                    }

                    foreach (var item in e.NewItems.OfType<QuickAccessMenuItem>())
                    {
                        item.Ribbon = this;
                    }

                    break;
            }
        }

        /// <summary>
        /// Gets or sets whether Customize Quick Access Toolbar menu item is shown
        /// </summary>
        public bool CanCustomizeQuickAccessToolBar
        {
            get { return (bool)this.GetValue(CanCustomizeQuickAccessToolBarProperty); }
            set { this.SetValue(CanCustomizeQuickAccessToolBarProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="CanCustomizeQuickAccessToolBar"/> dependency property.</summary>
        public static readonly DependencyProperty CanCustomizeQuickAccessToolBarProperty =
            DependencyProperty.Register(nameof(CanCustomizeQuickAccessToolBar), typeof(bool),
                typeof(Ribbon), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// Gets or sets whether items can be added or removed from the quick access toolbar by users.
        /// </summary>
        public bool CanCustomizeQuickAccessToolBarItems
        {
            get { return (bool)this.GetValue(CanCustomizeQuickAccessToolBarItemsProperty); }
            set { this.SetValue(CanCustomizeQuickAccessToolBarItemsProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="CanCustomizeQuickAccessToolBarItems"/> dependency property.</summary>
        public static readonly DependencyProperty CanCustomizeQuickAccessToolBarItemsProperty =
            DependencyProperty.Register(nameof(CanCustomizeQuickAccessToolBarItems), typeof(bool), typeof(Ribbon), new PropertyMetadata(BooleanBoxes.TrueBox));

        /// <summary>
        /// Gets or sets whether the QAT Menu-DropDown is visible or not.
        /// </summary>
        public bool IsQuickAccessToolBarMenuDropDownVisible
        {
            get { return (bool)this.GetValue(IsQuickAccessToolBarMenuDropDownVisibleProperty); }
            set { this.SetValue(IsQuickAccessToolBarMenuDropDownVisibleProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="IsQuickAccessToolBarMenuDropDownVisible"/> dependency property.</summary>
        public static readonly DependencyProperty IsQuickAccessToolBarMenuDropDownVisibleProperty =
            DependencyProperty.Register(nameof(IsQuickAccessToolBarMenuDropDownVisible), typeof(bool), typeof(Ribbon), new PropertyMetadata(BooleanBoxes.TrueBox));

        /// <summary>
        /// Gets or sets whether Customize Ribbon menu item is shown
        /// </summary>
        public bool CanCustomizeRibbon
        {
            get { return (bool)this.GetValue(CanCustomizeRibbonProperty); }
            set { this.SetValue(CanCustomizeRibbonProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="CanCustomizeRibbon"/> dependency property.</summary>
        public static readonly DependencyProperty CanCustomizeRibbonProperty =
            DependencyProperty.Register(nameof(CanCustomizeRibbon), typeof(bool),
                typeof(Ribbon), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// Gets or sets whether ribbon can be minimized
        /// </summary>
        public bool CanMinimize
        {
            get { return (bool)this.GetValue(CanMinimizeProperty); }
            set { this.SetValue(CanMinimizeProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// Gets or sets whether ribbon is minimized
        /// </summary>
        public bool IsMinimized
        {
            get { return (bool)this.GetValue(IsMinimizedProperty); }
            set { this.SetValue(IsMinimizedProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="IsMinimized"/> dependency property.</summary>
        public static readonly DependencyProperty IsMinimizedProperty =
            DependencyProperty.Register(nameof(IsMinimized), typeof(bool),
                typeof(Ribbon), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsMinimizedChanged));

        /// <summary>Identifies the <see cref="CanMinimize"/> dependency property.</summary>
        public static readonly DependencyProperty CanMinimizeProperty =
            DependencyProperty.Register(nameof(CanMinimize), typeof(bool), typeof(Ribbon), new PropertyMetadata(BooleanBoxes.TrueBox));

        private static void OnIsMinimizedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ribbon = (Ribbon)d;

            var oldValue = (bool)e.OldValue;
            var newValue = (bool)e.NewValue;

            ribbon.IsMinimizedChanged?.Invoke(ribbon, e);

            // Invert values of arguments for RaiseExpandCollapseAutomationEvent because IsMinimized means the negative for expand/collapsed
            (UIElementAutomationPeer.FromElement(ribbon) as WzUXRibbon.Automation.Peers.RibbonAutomationPeer)?.RaiseExpandCollapseAutomationEvent(!oldValue, !newValue);
        }

        /// <summary>
        /// Gets or sets whether ribbon can be switched
        /// </summary>
        public bool CanUseSimplified
        {
            get { return (bool)this.GetValue(CanUseSimplifiedProperty); }
            set { this.SetValue(CanUseSimplifiedProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="CanUseSimplified"/> dependency property.</summary>
        public static readonly DependencyProperty CanUseSimplifiedProperty =
            DependencyProperty.Register(nameof(CanUseSimplified), typeof(bool), typeof(Ribbon), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>Identifies the <see cref="IsDisplayOptionsButtonVisible"/> dependency property.</summary>
        public static readonly DependencyProperty IsDisplayOptionsButtonVisibleProperty =
            DependencyProperty.Register(nameof(IsDisplayOptionsButtonVisible), typeof(bool), typeof(Ribbon), new PropertyMetadata(BooleanBoxes.TrueBox));

        /// <summary>
        /// Defines whether display options button is visible or not.
        /// </summary>
        public bool IsDisplayOptionsButtonVisible
        {
            get { return (bool)this.GetValue(IsDisplayOptionsButtonVisibleProperty); }
            set { this.SetValue(IsDisplayOptionsButtonVisibleProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// Gets or sets the height of the gap between the ribbon and the regular window content
        /// </summary>
        public double ContentGapHeight
        {
            get { return (double)this.GetValue(ContentGapHeightProperty); }
            set { this.SetValue(ContentGapHeightProperty, value); }
        }

        /// <summary>Identifies the <see cref="ContentGapHeight"/> dependency property.</summary>
        public static readonly DependencyProperty ContentGapHeightProperty =
            DependencyProperty.Register(nameof(ContentGapHeight), typeof(double), typeof(Ribbon), new PropertyMetadata(RibbonTabControl.DefaultContentGapHeight));

        /// <summary>
        /// Gets or sets the height of the ribbon content area
        /// </summary>
        public double ContentHeight
        {
            get { return (double)this.GetValue(ContentHeightProperty); }
            set { this.SetValue(ContentHeightProperty, value); }
        }

        /// <summary>Identifies the <see cref="ContentHeight"/> dependency property.</summary>
        public static readonly DependencyProperty ContentHeightProperty =
            DependencyProperty.Register(nameof(ContentHeight), typeof(double), typeof(Ribbon), new PropertyMetadata(RibbonTabControl.DefaultContentHeight));

        // todo check if IsCollapsed and IsAutomaticCollapseEnabled should be reduced to one shared property for RibbonWindow and Ribbon

        /// <summary>
        /// Gets whether ribbon is collapsed
        /// </summary>
        public bool IsCollapsed
        {
            get { return (bool)this.GetValue(IsCollapsedProperty); }
            set { this.SetValue(IsCollapsedProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="IsCollapsed"/> dependency property.</summary>
        public static readonly DependencyProperty IsCollapsedProperty =
            DependencyProperty.Register(nameof(IsCollapsed), typeof(bool),
                typeof(Ribbon), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsCollapsedChanged));

        private static void OnIsCollapsedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ribbon = (Ribbon)d;
            ribbon.IsCollapsedChanged?.Invoke(ribbon, e);
        }

        /// <summary>
        /// Defines if the Ribbon should automatically set <see cref="IsCollapsed"/> when the width or height of the owner window drop under <see cref="MinimalVisibleWidth"/> or <see cref="MinimalVisibleHeight"/>
        /// </summary>
        public bool IsAutomaticCollapseEnabled
        {
            get { return (bool)this.GetValue(IsAutomaticCollapseEnabledProperty); }
            set { this.SetValue(IsAutomaticCollapseEnabledProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="IsAutomaticCollapseEnabled"/> dependency property.</summary>
        public static readonly DependencyProperty IsAutomaticCollapseEnabledProperty =
            DependencyProperty.Register(nameof(IsAutomaticCollapseEnabled), typeof(bool), typeof(Ribbon), new PropertyMetadata(BooleanBoxes.TrueBox, OnIsAutomaticCollapseEnabledChanged));

        /// <summary>
        /// Gets or sets whether QAT is visible
        /// </summary>
        public bool IsQuickAccessToolBarVisible
        {
            get { return (bool)this.GetValue(IsQuickAccessToolBarVisibleProperty); }
            set { this.SetValue(IsQuickAccessToolBarVisibleProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="IsQuickAccessToolBarVisible"/> dependency property.</summary>
        public static readonly DependencyProperty IsQuickAccessToolBarVisibleProperty =
            DependencyProperty.Register(nameof(IsQuickAccessToolBarVisible), typeof(bool), typeof(Ribbon), new PropertyMetadata(BooleanBoxes.TrueBox));

        /// <summary>
        /// Gets or sets whether user can change location of QAT
        /// </summary>
        public bool CanQuickAccessLocationChanging
        {
            get { return (bool)this.GetValue(CanQuickAccessLocationChangingProperty); }
            set { this.SetValue(CanQuickAccessLocationChangingProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="CanQuickAccessLocationChanging"/> dependency property.</summary>
        public static readonly DependencyProperty CanQuickAccessLocationChangingProperty =
            DependencyProperty.Register(nameof(CanQuickAccessLocationChanging), typeof(bool), typeof(Ribbon), new PropertyMetadata(BooleanBoxes.TrueBox));

        /// <summary>Identifies the <see cref="AreTabHeadersVisible"/> dependency property.</summary>
        public static readonly DependencyProperty AreTabHeadersVisibleProperty = DependencyProperty.Register(nameof(AreTabHeadersVisible), typeof(bool), typeof(Ribbon), new PropertyMetadata(BooleanBoxes.TrueBox));

        /// <summary>
        /// Defines whether tab headers are visible or not.
        /// </summary>
        public bool AreTabHeadersVisible
        {
            get { return (bool)this.GetValue(AreTabHeadersVisibleProperty); }
            set { this.SetValue(AreTabHeadersVisibleProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="IsToolBarVisible"/> dependency property.</summary>
        public static readonly DependencyProperty IsToolBarVisibleProperty = DependencyProperty.Register(nameof(IsToolBarVisible), typeof(bool), typeof(Ribbon), new PropertyMetadata(BooleanBoxes.TrueBox));

        /// <summary>
        /// Defines whether tab headers are visible or not.
        /// </summary>
        public bool IsToolBarVisible
        {
            get { return (bool)this.GetValue(IsToolBarVisibleProperty); }
            set { this.SetValue(IsToolBarVisibleProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="IsMouseWheelScrollingEnabled"/> dependency property.</summary>
        public static readonly DependencyProperty IsMouseWheelScrollingEnabledProperty = DependencyProperty.Register(nameof(IsMouseWheelScrollingEnabled), typeof(bool), typeof(Ribbon), new PropertyMetadata(BooleanBoxes.TrueBox));

        /// <summary>
        /// Defines whether scrolling by mouse wheel on the tab container area to cycle tabs is enabled or not.
        /// </summary>
        public bool IsMouseWheelScrollingEnabled
        {
            get { return (bool)this.GetValue(IsMouseWheelScrollingEnabledProperty); }
            set { this.SetValue(IsMouseWheelScrollingEnabledProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="IsMouseWheelScrollingEnabledEverywhere"/> dependency property.</summary>
        public static readonly DependencyProperty IsMouseWheelScrollingEnabledEverywhereProperty = DependencyProperty.Register(nameof(IsMouseWheelScrollingEnabledEverywhere), typeof(bool), typeof(Ribbon), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// Defines whether scrolling by mouse wheel always cycles selected tab, also outside the tab container area.
        /// </summary>
        public bool IsMouseWheelScrollingEnabledEverywhere
        {
            get { return (bool)this.GetValue(IsMouseWheelScrollingEnabledEverywhereProperty); }
            set { this.SetValue(IsMouseWheelScrollingEnabledEverywhereProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// Checks if any keytips are visible.
        /// </summary>
        public bool AreAnyKeyTipsVisible => this.keyTipService?.AreAnyKeyTipsVisible == true;

        /// <summary>Identifies the <see cref="IsKeyTipHandlingEnabled"/> dependency property.</summary>
        public static readonly DependencyProperty IsKeyTipHandlingEnabledProperty = DependencyProperty.Register(nameof(IsKeyTipHandlingEnabled), typeof(bool), typeof(Ribbon), new PropertyMetadata(BooleanBoxes.TrueBox, OnIsKeyTipHandlingEnabledChanged));

        /// <summary>
        /// Defines whether handling of key tips is enabled or not.
        /// </summary>
        public bool IsKeyTipHandlingEnabled
        {
            get { return (bool)this.GetValue(IsKeyTipHandlingEnabledProperty); }
            set { this.SetValue(IsKeyTipHandlingEnabledProperty, BooleanBoxes.Box(value)); }
        }

        private static void OnIsKeyTipHandlingEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ribbon = (Ribbon)d;

            if ((bool)e.NewValue)
            {
                ribbon.keyTipService?.Attach();
            }
            else
            {
                ribbon.keyTipService?.Detach();
            }
        }

        /// <summary>
        /// Defines the keys that are used to activate the key tips.
        /// </summary>
        public ObservableCollection<Key> KeyTipKeys
        {
            get
            {
                if (this.keyTipKeys is null)
                {
                    this.keyTipKeys = new ObservableCollection<Key>();
                    this.keyTipKeys.CollectionChanged += this.HandleKeyTipKeys_CollectionChanged;
                }

                return this.keyTipKeys;
            }
        }

        private void HandleKeyTipKeys_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.keyTipService.KeyTipKeys.Clear();

            foreach (var keyTipKey in this.KeyTipKeys)
            {
                this.keyTipService.KeyTipKeys.Add(keyTipKey);
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets add to quick access toolbar command
        /// </summary>
        public static readonly RoutedCommand AddToQuickAccessCommand = new RoutedCommand(nameof(AddToQuickAccessCommand), typeof(Ribbon));

        /// <summary>
        /// Gets remove from quick access command
        /// </summary>
        public static readonly RoutedCommand RemoveFromQuickAccessCommand = new RoutedCommand(nameof(RemoveFromQuickAccessCommand), typeof(Ribbon));

        /// <summary>
        /// Gets show quick access above command
        /// </summary>
        public static readonly RoutedCommand ShowQuickAccessAboveCommand = new RoutedCommand(nameof(ShowQuickAccessAboveCommand), typeof(Ribbon));

        /// <summary>
        /// Gets show quick access below command
        /// </summary>
        public static readonly RoutedCommand ShowQuickAccessBelowCommand = new RoutedCommand(nameof(ShowQuickAccessBelowCommand), typeof(Ribbon));

        /// <summary>
        /// Gets toggle ribbon minimize command
        /// </summary>
        public static readonly RoutedCommand ToggleMinimizeTheRibbonCommand = new RoutedCommand(nameof(ToggleMinimizeTheRibbonCommand), typeof(Ribbon));

        /// <summary>
        /// Gets Switch to classic ribbon command
        /// </summary>
        public static readonly RoutedCommand SwitchToTheClassicRibbonCommand = new RoutedCommand(nameof(SwitchToTheClassicRibbonCommand), typeof(Ribbon));

        /// <summary>
        /// Gets Switch to simplified ribbon command
        /// </summary>
        public static readonly RoutedCommand SwitchToTheSimplifiedRibbonCommand = new RoutedCommand(nameof(SwitchToTheSimplifiedRibbonCommand), typeof(Ribbon));

        /// <summary>
        /// Gets customize quick access toolbar command
        /// </summary>
        public static readonly RoutedCommand CustomizeQuickAccessToolbarCommand = new RoutedCommand(nameof(CustomizeQuickAccessToolbarCommand), typeof(Ribbon));

        /// <summary>
        /// Gets customize the ribbon command
        /// </summary>
        public static readonly RoutedCommand CustomizeTheRibbonCommand = new RoutedCommand(nameof(CustomizeTheRibbonCommand), typeof(Ribbon));

        // Occurs when customize toggle minimize command can execute handles
        private static void OnToggleMinimizeTheRibbonCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is Ribbon ribbon)
            {
                e.CanExecute = ribbon.CanMinimize;
            }
        }

        // Occurs when toggle minimize command executed
        private static void OnToggleMinimizeTheRibbonCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Ribbon ribbon)
            {
                ribbon.IsMinimized = !ribbon.IsMinimized;
            }
        }

        // Occurs when customize switch ribbon command can execute handles
        private static void OnSwitchTheRibbonCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is Ribbon ribbon)
            {
                e.CanExecute = ribbon.CanUseSimplified;
            }
        }

        // Occurs when switch ribbon command executed
        private static void OnSwitchToTheClassicRibbonCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Ribbon ribbon)
            {
                ribbon.IsSimplified = false;
            }
        }

        // Occurs when switch ribbon command executed
        private static void OnSwitchToTheSimplifiedRibbonCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Ribbon ribbon)
            {
                ribbon.IsSimplified = true;
            }
        }

        // Occurs when show quick access below command executed
        private static void OnShowQuickAccessBelowCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var ribbon = sender as Ribbon;

            if (ribbon is null)
            {
                return;
            }

            ribbon.ShowQuickAccessToolBarAboveRibbon = false;
        }

        // Occurs when show quick access above command executed
        private static void OnShowQuickAccessAboveCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var ribbon = sender as Ribbon;

            if (ribbon is null)
            {
                return;
            }

            ribbon.ShowQuickAccessToolBarAboveRibbon = true;
        }

        // Occurs when remove from quick access command executed
        private static void OnRemoveFromQuickAccessCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var ribbon = sender as Ribbon;

            if (ribbon?.QuickAccessToolBar != null)
            {
                var element = ribbon.QuickAccessElements.First(x => ReferenceEquals(x.Value, e.Parameter)).Key;
                ribbon.RemoveFromQuickAccessToolBar(element);
            }
        }

        // Occurs when add to quick access command executed
        private static void OnAddToQuickAccessCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var ribbon = sender as Ribbon;

            if (ribbon?.QuickAccessToolBar != null)
            {
                ribbon.AddToQuickAccessToolBar(e.Parameter as UIElement);
            }
        }

        // Occurs when customize quick access command executed
        private static void OnCustomizeQuickAccessToolbarCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var ribbon = sender as Ribbon;

            ribbon?.CustomizeQuickAccessToolbar?.Invoke(sender, EventArgs.Empty);
        }

        // Occurs when customize the ribbon command executed
        private static void OnCustomizeTheRibbonCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var ribbon = sender as Ribbon;

            ribbon?.CustomizeTheRibbon?.Invoke(sender, EventArgs.Empty);
        }

        // Occurs when customize quick access command can execute handles
        private static void OnCustomizeQuickAccessToolbarCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var ribbon = sender as Ribbon;

            if (ribbon is null)
            {
                return;
            }

            e.CanExecute = ribbon.CanCustomizeQuickAccessToolBar;
        }

        // Occurs when customize the ribbon command can execute handles
        private static void OnCustomizeTheRibbonCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var ribbon = sender as Ribbon;

            if (ribbon is null)
            {
                return;
            }

            e.CanExecute = ribbon.CanCustomizeRibbon;
        }

        // Occurs when remove from quick access command can execute handles
        private static void OnRemoveFromQuickAccessCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is Ribbon ribbon
                && ribbon.IsQuickAccessToolBarVisible
                && e.Parameter is UIElement element)
            {
                e.CanExecute = ribbon.QuickAccessElements.ContainsValue(element);
            }
            else
            {
                e.CanExecute = false;
            }
        }

        // Occurs when add to quick access command can execute handles
        private static void OnAddToQuickAccessCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is Ribbon ribbon
                && ribbon.IsQuickAccessToolBarVisible
                && QuickAccessItemsProvider.IsSupported(e.Parameter as UIElement)
                && ribbon.IsInQuickAccessToolBar(e.Parameter as UIElement) == false)
            {
                if (e.Parameter is Gallery gallery)
                {
                    e.CanExecute = ribbon.IsInQuickAccessToolBar(FindParentRibbonControl(gallery) as UIElement) == false;
                }
                else
                {
                    e.CanExecute = ribbon.IsInQuickAccessToolBar(e.Parameter as UIElement) == false;
                }
            }
            else
            {
                e.CanExecute = false;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="Ribbon"/> class.
        /// </summary>
        static Ribbon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Ribbon), new FrameworkPropertyMetadata(typeof(Ribbon)));

            // Subscribe to menu commands
            CommandManager.RegisterClassCommandBinding(typeof(Ribbon), new CommandBinding(AddToQuickAccessCommand, OnAddToQuickAccessCommandExecuted, OnAddToQuickAccessCommandCanExecute));
            CommandManager.RegisterClassCommandBinding(typeof(Ribbon), new CommandBinding(RemoveFromQuickAccessCommand, OnRemoveFromQuickAccessCommandExecuted, OnRemoveFromQuickAccessCommandCanExecute));
            CommandManager.RegisterClassCommandBinding(typeof(Ribbon), new CommandBinding(ShowQuickAccessAboveCommand, OnShowQuickAccessAboveCommandExecuted));
            CommandManager.RegisterClassCommandBinding(typeof(Ribbon), new CommandBinding(ShowQuickAccessBelowCommand, OnShowQuickAccessBelowCommandExecuted));
            CommandManager.RegisterClassCommandBinding(typeof(Ribbon), new CommandBinding(ToggleMinimizeTheRibbonCommand, OnToggleMinimizeTheRibbonCommandExecuted, OnToggleMinimizeTheRibbonCommandCanExecute));
            CommandManager.RegisterClassCommandBinding(typeof(Ribbon), new CommandBinding(SwitchToTheClassicRibbonCommand, OnSwitchToTheClassicRibbonCommandExecuted, OnSwitchTheRibbonCommandCanExecute));
            CommandManager.RegisterClassCommandBinding(typeof(Ribbon), new CommandBinding(SwitchToTheSimplifiedRibbonCommand, OnSwitchToTheSimplifiedRibbonCommandExecuted, OnSwitchTheRibbonCommandCanExecute));
            CommandManager.RegisterClassCommandBinding(typeof(Ribbon), new CommandBinding(CustomizeTheRibbonCommand, OnCustomizeTheRibbonCommandExecuted, OnCustomizeTheRibbonCommandCanExecute));
            CommandManager.RegisterClassCommandBinding(typeof(Ribbon), new CommandBinding(CustomizeQuickAccessToolbarCommand, OnCustomizeQuickAccessToolbarCommandExecuted, OnCustomizeQuickAccessToolbarCommandCanExecute));
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Ribbon()
        {
            IsMinimized = true;
            this.VerticalAlignment = VerticalAlignment.Top;
            KeyboardNavigation.SetDirectionalNavigation(this, KeyboardNavigationMode.Contained);

            this.keyTipService = new WzUXRibbon.Services.KeyTipService(this);

            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;
        }

        #endregion

        #region Overrides

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.MaintainIsCollapsed();
        }

        private static void OnIsAutomaticCollapseEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Ribbon)d).MaintainIsCollapsed();
        }

        private void MaintainIsCollapsed()
        {
            if (this.IsAutomaticCollapseEnabled == false
                || this.ownerWindow is null)
            {
                this.ClearValue(IsCollapsedProperty);
                return;
            }

            if (this.ownerWindow.ActualWidth < MinimalVisibleWidth
                || this.ownerWindow.ActualHeight < MinimalVisibleHeight)
            {
                this.SetCurrentValue(IsCollapsedProperty, BooleanBoxes.TrueBox);
            }
            else
            {
                this.SetCurrentValue(IsCollapsedProperty, BooleanBoxes.FalseBox);
            }
        }

        /// <inheritdoc />
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            var ribbonTabItem = (RibbonTabItem)this.TabControl?.SelectedItem;
            ribbonTabItem?.Focus();
        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            this.layoutRoot = this.GetTemplateChild("PART_LayoutRoot") as Panel;

            var selectedTab = this.SelectedTabItem;
            if (this.TabControl != null)
            {
                this.TabControl.SelectionChanged -= this.OnTabControlSelectionChanged;
                selectedTab = this.TabControl.SelectedItem as RibbonTabItem;

                this.tabsSync?.Target.Clear();

                this.toolBarItemsSync?.Target.Clear();
            }

            this.TabControl = this.GetTemplateChild("PART_RibbonTabControl") as RibbonTabControl;

            if (this.TabControl != null)
            {
                this.TabControl.SelectionChanged += this.OnTabControlSelectionChanged;

                this.tabsSync = new CollectionSyncHelper<RibbonTabItem>(this.Tabs, this.TabControl.Items);

                this.TabControl.SelectedItem = selectedTab;

                this.toolBarItemsSync = new CollectionSyncHelper<UIElement>(this.ToolBarItems, this.TabControl.ToolBarItems);
            }

            //if (this.QuickAccessToolBar != null)
            //{
            //    this.ClearQuickAccessToolBar();

            //    this.quickAccessItemsSync?.Target.Clear();
            //}

            //this.QuickAccessToolBar = this.GetTemplateChild("PART_QuickAccessToolBar") as QuickAccessToolBar;

            //if (this.QuickAccessToolBar != null)
            //{
            //    this.quickAccessItemsSync = new CollectionSyncHelper<QuickAccessMenuItem>(this.QuickAccessItems, this.QuickAccessToolBar.QuickAccessItems);

            //    {
            //        var binding = new Binding(nameof(this.CanQuickAccessLocationChanging))
            //        {
            //            Source = this,
            //            Mode = BindingMode.OneWay
            //        };
            //        this.QuickAccessToolBar.SetBinding(QuickAccessToolBar.CanQuickAccessLocationChangingProperty, binding);
            //    }
            //}

            //if (this.ShowQuickAccessToolBarAboveRibbon)
            //{
            //    this.MoveQuickAccessToolBarToTitleBar(this.TitleBar);
            //}
        }

        /// <inheritdoc />
        protected override AutomationPeer OnCreateAutomationPeer() => new WzUXRibbon.Automation.Peers.RibbonAutomationPeer(this);

        private void MoveQuickAccessToolBarToTitleBar(RibbonTitleBar titleBar)
        {
            if (titleBar != null)
            {
                titleBar.QuickAccessToolBar = this.QuickAccessToolBar;
            }

            if (this.QuickAccessToolBar != null)
            {
                // Prevent double add for handler if this method is called multiple times
                this.QuickAccessToolBar.ContextMenuOpening -= this.OnQuickAccessContextMenuOpening;
                this.QuickAccessToolBar.ContextMenuClosing -= this.OnQuickAccessContextMenuClosing;

                this.QuickAccessToolBar.ContextMenuOpening += this.OnQuickAccessContextMenuOpening;
                this.QuickAccessToolBar.ContextMenuClosing += this.OnQuickAccessContextMenuClosing;
            }
        }

        private void RemoveQuickAccessToolBarFromTitleBar(RibbonTitleBar titleBar)
        {
            if (titleBar != null)
            {
                titleBar.QuickAccessToolBar = null;
            }

            if (this.QuickAccessToolBar != null)
            {
                this.QuickAccessToolBar.ContextMenuOpening -= this.OnQuickAccessContextMenuOpening;
                this.QuickAccessToolBar.ContextMenuClosing -= this.OnQuickAccessContextMenuClosing;
            }
        }

        private void OnOwnerWindowClosed(object sender, EventArgs e)
        {
            this.DetachFromWindow();
        }

        private void AttachToWindow()
        {
            this.DetachFromWindow();

            this.ownerWindow = Window.GetWindow(this);

            if (this.ownerWindow != null)
            {
                this.ownerWindow.Closed += this.OnOwnerWindowClosed;
                this.ownerWindow.SizeChanged += this.OnSizeChanged;
                this.ownerWindow.KeyDown += this.OnKeyDown;
            }
        }

        private void DetachFromWindow()
        {
            if (this.ownerWindow != null)
            {
                this.ownerWindow.Closed -= this.OnOwnerWindowClosed;
                this.ownerWindow.SizeChanged -= this.OnSizeChanged;
                this.ownerWindow.KeyDown -= this.OnKeyDown;
            }

            this.ownerWindow = null;
        }

        #endregion

        #region Quick Access Items Managment

        /// <summary>
        /// Determines whether the given element is in quick access toolbar
        /// </summary>
        /// <param name="element">Element</param>
        /// <returns>True if element in quick access toolbar</returns>
        public bool IsInQuickAccessToolBar(UIElement element)
        {
            if (element is null)
            {
                return false;
            }

            return this.QuickAccessElements.ContainsKey(element);
        }

        /// <summary>
        /// Adds the given element to quick access toolbar
        /// </summary>
        /// <param name="element">Element</param>
        public void AddToQuickAccessToolBar(UIElement element)
        {
            if (element is null)
            {
                return;
            }

            if (element is Gallery)
            {
                element = FindParentRibbonControl(element) as UIElement;
            }

            // Do not add menu items without icon.
            if (element is System.Windows.Controls.MenuItem menuItem && menuItem.Icon is null)
            {
                element = FindParentRibbonControl(element) as UIElement;
            }

            if (element is null)
            {
                return;
            }

            if (QuickAccessItemsProvider.IsSupported(element) == false)
            {
                return;
            }

            if (this.IsInQuickAccessToolBar(element) == false)
            {
                Debug.WriteLine($"Adding \"{element}\" to QuickAccessToolBar.");

                var control = QuickAccessItemsProvider.GetQuickAccessItem(element);

                if (control != null)
                {
                    this.QuickAccessElements.Add(element, control);
                    this.QuickAccessToolBar?.Items.Add(control);
                }
            }
        }

        private static IRibbonControl FindParentRibbonControl(DependencyObject element)
        {
            var parent = LogicalTreeHelper.GetParent(element);

            while (parent != null)
            {
                if (parent is IRibbonControl control)
                {
                    return control;
                }

                parent = LogicalTreeHelper.GetParent(parent);
            }

            return null;
        }

        /// <summary>
        /// Removes the given elements from quick access toolbar
        /// </summary>
        /// <param name="element">Element</param>
        public void RemoveFromQuickAccessToolBar(UIElement element)
        {
            if (element is null)
            {
                return;
            }

            Debug.WriteLine("Removing \"{0}\" from QuickAccessToolBar.", element);

            if (this.IsInQuickAccessToolBar(element))
            {
                var quickAccessItem = this.QuickAccessElements[element];
                this.QuickAccessElements.Remove(element);
                this.QuickAccessToolBar?.Items.Remove(quickAccessItem);
            }
        }

        /// <summary>
        /// Clears quick access toolbar
        /// </summary>
        public void ClearQuickAccessToolBar()
        {
            this.QuickAccessElements.Clear();
            this.QuickAccessToolBar?.Items.Clear();
        }

        #endregion

        #region Event Handling

        // Handles tab control selection changed
        private void OnTabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReferenceEquals(e.OriginalSource, this.TabControl) == false)
            {
                return;
            }

            this.SelectedTabItem = this.TabControl?.SelectedItem as RibbonTabItem;
            this.SelectedTabIndex = this.TabControl?.SelectedIndex ?? -1;

            this.SelectedTabChanged?.Invoke(this, e);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.keyTipService.Attach();

            this.AttachToWindow();

            this.TitleBar?.ScheduleForceMeasureAndArrange();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                switch (e.Key)
                {
                    case Key.F1:
                        {
                            if (this.TabControl?.HasItems == true)
                            {
                                if (this.CanMinimize)
                                {
                                    this.IsMinimized = !this.IsMinimized;
                                }
                            }

                            break;
                        }

                    case Key.F2:
                        {
                            if (this.TabControl?.HasItems == true)
                            {
                                if (this.CanUseSimplified)
                                {
                                    this.IsSimplified = !this.IsSimplified;
                                }
                            }

                            break;
                        }
                }
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.keyTipService.Detach();

            if (this.ownerWindow != null)
            {
                this.ownerWindow.SizeChanged -= this.OnSizeChanged;
                this.ownerWindow.KeyDown -= this.OnKeyDown;
            }
        }

        #endregion

        #region Private methods

        private RibbonTabItem GetFirstVisibleItem()
        {
            return this.Tabs.FirstOrDefault(item => item.Visibility == Visibility.Visible);
        }

        private RibbonTabItem GetLastVisibleItem()
        {
            return this.Tabs.LastOrDefault(item => item.Visibility == Visibility.Visible);
        }

        #endregion

        #region State Management

        public void LoadInitialState()
        {
            this.TabControl?.SelectFirstTab();
        }

        #endregion

        #region AutomaticStateManagement Property

        /// <summary>
        /// Gets or sets whether Quick Access ToolBar can
        /// save and load its state automatically
        /// </summary>
        public bool AutomaticStateManagement
        {
            get { return (bool)this.GetValue(AutomaticStateManagementProperty); }
            set { this.SetValue(AutomaticStateManagementProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>Identifies the <see cref="AutomaticStateManagement"/> dependency property.</summary>
        public static readonly DependencyProperty AutomaticStateManagementProperty =
            DependencyProperty.Register(nameof(AutomaticStateManagement), typeof(bool), typeof(Ribbon),
                new PropertyMetadata(BooleanBoxes.TrueBox, OnAutomaticStateManagementChanged, CoerceAutomaticStateManagement));

        private static object CoerceAutomaticStateManagement(DependencyObject d, object basevalue)
        {
            return basevalue;
        }

        private static void OnAutomaticStateManagementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ribbon = (Ribbon)d;
            if ((bool)e.NewValue)
            {
                ribbon.LoadInitialState();
            }
        }

        #endregion

        /// <inheritdoc />
        void ILogicalChildSupport.AddLogicalChild(object child)
        {
            this.AddLogicalChild(child);
        }

        /// <inheritdoc />
        void ILogicalChildSupport.RemoveLogicalChild(object child)
        {
            this.RemoveLogicalChild(child);
        }

        /// <inheritdoc />
        protected override IEnumerator LogicalChildren
        {
            get
            {
                var baseEnumerator = base.LogicalChildren;
                while (baseEnumerator?.MoveNext() == true)
                {
                    yield return baseEnumerator.Current;
                }

                if (this.Menu != null)
                {
                    yield return this.Menu;
                }

                //if (this.StartScreen != null)
                //{
                //    yield return this.StartScreen;
                //}

                if (this.QuickAccessToolBar != null)
                {
                    yield return this.QuickAccessToolBar;
                }

                if (this.TabControl?.ToolbarPanel != null)
                {
                    yield return this.TabControl.ToolbarPanel;
                }

                if (this.layoutRoot != null)
                {
                    yield return this.layoutRoot;
                }
            }
        }
    }
}
