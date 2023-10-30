using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Peers;
using System.Windows.Automation;
using WzUXRibbon.Controls;

namespace WzUXRibbon.Automation.Peers
{
    public class RibbonTabItemAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonTabItemAutomationPeer(RibbonTabItem owner)
            : base(owner)
        {
            this.OwningTab = owner;
        }

        private RibbonTabItem OwningTab { get; }

        
        public override object GetPattern(PatternInterface patternInterface)
        {
            switch (patternInterface)
            {
                case PatternInterface.Scroll:
                    var container = this.OwningTab.GroupsContainer;
                    if (container != null)
                    {
                        var automationPeer = CreatePeerForElement(container);
                        if (automationPeer != null)
                        {
                            return automationPeer.GetPattern(patternInterface);
                        }
                    }

                    break;
            }

            return base.GetPattern(patternInterface);
        }

        
        protected override List<AutomationPeer> GetChildrenCore()
        {
            var children = GetHeaderChildren() ?? new List<AutomationPeer>();

            if (this.OwningTab.IsSelected == false)
            {
                return children;
            }

            foreach (var @group in this.OwningTab.Groups)
            {
                var peer = CreatePeerForElement(@group);

                if (peer != null)
                {
                    children.Add(peer);
                }
            }

            return children;

            List<AutomationPeer> GetHeaderChildren()
            {
                if (this.OwningTab.Header is string)
                {
                    return null;
                }

                if (this.OwningTab.HeaderContentHost != null)
                {
                    return new FrameworkElementAutomationPeer(this.OwningTab.HeaderContentHost).GetChildren();
                }

                return null;
            }
        }

        
        protected override string GetClassNameCore()
        {
            return this.Owner.GetType().Name;
        }

        
        protected override string GetAccessKeyCore()
        {
            var text = this.OwningTab.KeyTip;
            if (string.IsNullOrEmpty(text))
            {
                text = base.GetAccessKeyCore();
            }

            return text;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal void RaiseTabExpandCollapseAutomationEvent(bool oldValue, bool newValue)
        {
            this.EventsSource?.RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty, oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed, newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal void RaiseTabSelectionEvents()
        {
            var eventsSource = this.EventsSource;
            if (eventsSource != null)
            {
                if (this.OwningTab.IsSelected)
                {
                    eventsSource.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
                }
                else
                {
                    eventsSource.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
                }
            }
        }
    }
}
