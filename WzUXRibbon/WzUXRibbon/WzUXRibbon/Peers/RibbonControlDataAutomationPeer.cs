﻿using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows;

namespace WzUXRibbon.Automation.Peers
{

    public class RibbonControlDataAutomationPeer : ItemAutomationPeer
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonControlDataAutomationPeer(object item, ItemsControlAutomationPeer itemsControlPeer)
            : base(item, itemsControlPeer)
        {
        }

        
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.ListItem;
        }

        
        protected override string GetClassNameCore()
        {
            var wrapperPeer = this.GetWrapperPeer();
            return wrapperPeer?.GetClassName() ?? string.Empty;
        }

        
        public override object GetPattern(PatternInterface patternInterface)
        {
            // Doesnt implement any patterns of its own, so just forward to the wrapper peer. 
            var wrapperPeer = this.GetWrapperPeer();

            return wrapperPeer?.GetPattern(patternInterface) ?? base.GetPattern(patternInterface);
        }

        private UIElement GetWrapper()
        {
            var itemsControlAutomationPeer = this.ItemsControlAutomationPeer;

            var owner = (ItemsControl)itemsControlAutomationPeer?.Owner;
            return owner?.ItemContainerGenerator.ContainerFromItem(this.Item) as UIElement;
        }

        private AutomationPeer GetWrapperPeer()
        {
            var wrapper = this.GetWrapper();
            if (wrapper is null)
            {
                return null;
            }

            var wrapperPeer = UIElementAutomationPeer.CreatePeerForElement(wrapper);
            if (wrapperPeer != null)
            {
                return wrapperPeer;
            }

            if (wrapper is FrameworkElement element)
            {
                return new FrameworkElementAutomationPeer(element);
            }

            return new UIElementAutomationPeer(wrapper);
        }
    }
}
