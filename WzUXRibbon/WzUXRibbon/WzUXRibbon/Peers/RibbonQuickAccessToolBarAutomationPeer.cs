﻿using System.Collections.Generic;
using System.Windows.Automation.Peers;
using WzUXRibbon.Controls;

namespace WzUXRibbon.Automation.Peers
{

    public class RibbonQuickAccessToolBarAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonQuickAccessToolBarAutomationPeer(QuickAccessToolBar owner)
            : base(owner)
        {
            this.OwningQuickAccessToolBar = owner;
        }

        private QuickAccessToolBar OwningQuickAccessToolBar { get; }

        /// <inheritdoc />
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.ToolBar;
        }

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return this.Owner.GetType().Name;
        }

        /// <inheritdoc />
        protected override List<AutomationPeer> GetChildrenCore()
        {
            var children = new List<AutomationPeer>();

            foreach (var quickAccessMenuItem in this.OwningQuickAccessToolBar.Items)
            {
                //if (quickAccessMenuItem.IsChecked == false)
                //{
                //    continue;
                //}

                var automationPeer = CreatePeerForElement(quickAccessMenuItem);

                if (automationPeer != null)
                {
                    children.Add(automationPeer);
                }
            }

            var customizeMenuButton = this.OwningQuickAccessToolBar.MenuDownButton;
            if (customizeMenuButton != null)
            {
                var automationPeer = CreatePeerForElement(customizeMenuButton);

                if (automationPeer != null)
                {
                    children.Add(automationPeer);
                }
            }

            return children;
        }
    }
}
