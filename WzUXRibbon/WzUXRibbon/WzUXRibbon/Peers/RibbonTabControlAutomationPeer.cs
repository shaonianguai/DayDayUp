using System.Collections.Generic;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows;
using WzUXRibbon.Controls;

namespace WzUXRibbon.Automation.Peers
{
    public class RibbonTabControlAutomationPeer : SelectorAutomationPeer, ISelectionProvider
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonTabControlAutomationPeer(RibbonTabControl owner)
            : base(owner)
        {
            this.OwningRibbonTabControl = owner;
        }

        private RibbonTabControl OwningRibbonTabControl { get; }

        /// <inheritdoc />
        protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
        {
            return new RibbonTabItemDataAutomationPeer(item, this);
        }

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return this.Owner.GetType().Name;
        }

        /// <inheritdoc />
        protected override Point GetClickablePointCore()
        {
            return new Point(double.NaN, double.NaN);
        }

        bool ISelectionProvider.IsSelectionRequired => true;

        bool ISelectionProvider.CanSelectMultiple => false;

        /// <inheritdoc />
        public override object GetPattern(PatternInterface patternInterface)
        {
            switch (patternInterface)
            {
                case PatternInterface.Scroll:
                    var ribbonTabsContainerPanel = this.OwningRibbonTabControl.TabsContainer;
                    if (ribbonTabsContainerPanel != null)
                    {
                        var automationPeer = CreatePeerForElement(ribbonTabsContainerPanel);
                        if (automationPeer != null)
                        {
                            return automationPeer.GetPattern(patternInterface);
                        }
                    }

                    if (this.OwningRibbonTabControl.TabsContainer is RibbonTabsContainer ribbonTabsContainer
                        && ribbonTabsContainer.ScrollOwner != null)
                    {
                        var automationPeer = CreatePeerForElement(ribbonTabsContainer.ScrollOwner);
                        if (automationPeer != null)
                        {
                            automationPeer.EventsSource = this;
                            return automationPeer.GetPattern(patternInterface);
                        }
                    }

                    break;
            }

            return base.GetPattern(patternInterface);
        }

        /// <inheritdoc />
        protected override List<AutomationPeer> GetChildrenCore()
        {
            var children = base.GetChildrenCore() ?? new List<AutomationPeer>();

            var toolbarPanel = this.OwningRibbonTabControl.ToolbarPanel;

            if (toolbarPanel != null)
            {
                foreach (UIElement child in toolbarPanel.Children)
                {
                    if (child is null)
                    {
                        continue;
                    }

                    var automationPeer = CreatePeerForElement(child);

                    if (automationPeer != null)
                    {
                        children.Add(automationPeer);
                    }
                }
            }

            var displayOptionsButton = this.OwningRibbonTabControl.DisplayOptionsControl;

            if (displayOptionsButton != null)
            {
                var automationPeer = CreatePeerForElement(displayOptionsButton);

                if (automationPeer != null)
                {
                    children.Add(automationPeer);
                }
            }

            return children;
        }
    }
}
