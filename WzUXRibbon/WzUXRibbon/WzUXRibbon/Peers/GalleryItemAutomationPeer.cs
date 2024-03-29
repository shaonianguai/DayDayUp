﻿using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using WzUXRibbon.Controls;

namespace WzUXRibbon.Automation.Peers
{

    public class GalleryItemAutomationPeer : SelectorItemAutomationPeer, IScrollItemProvider
    {
        /// <inheritdoc cref="SelectorItemAutomationPeer" />
        public GalleryItemAutomationPeer(object owner, SelectorAutomationPeer selectorAutomationPeer)
            : base(owner, selectorAutomationPeer)
        {
        }

        
        protected override string GetClassNameCore()
        {
            return "GalleryItem";
        }

        
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.ListItem;
        }

        
        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.ScrollItem)
            {
                return this;
            }

            return base.GetPattern(patternInterface);
        }

        void IScrollItemProvider.ScrollIntoView()
        {
            switch (this.ItemsControlAutomationPeer.Owner)
            {
                case InRibbonGallery parent:
                    parent.ScrollIntoView(this.Item);
                    break;
                case ListBox parent:
                    parent.ScrollIntoView(this.Item);
                    break;
            }
        }
    }
}
