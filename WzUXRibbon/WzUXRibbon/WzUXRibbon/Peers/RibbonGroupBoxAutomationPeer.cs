using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Automation;
using WzUXRibbon.Controls;
using WzUXRibbon.Enumerations;

namespace WzUXRibbon.Automation.Peers
{

    public class RibbonGroupBoxAutomationPeer : FrameworkElementAutomationPeer, IExpandCollapseProvider, IScrollItemProvider
    {
        private RibbonGroupHeaderAutomationPeer headerPeer;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonGroupBoxAutomationPeer(RibbonGroupBox owner)
            : base(owner)
        {
            this.OwningGroup = owner;
        }

        private RibbonGroupBox OwningGroup { get; }

        private RibbonGroupHeaderAutomationPeer HeaderPeer
        {
            get
            {
                if (this.headerPeer is null
                    || !this.headerPeer.Owner.IsDescendantOf(this.OwningGroup))
                {
                    if (this.OwningGroup.State == RibbonGroupBoxState.Collapsed)
                    {
                        if (this.OwningGroup.CollapsedHeaderContentControl != null)
                        {
                            this.headerPeer = new RibbonGroupHeaderAutomationPeer(this.OwningGroup.CollapsedHeaderContentControl);
                        }
                    }
                    else if (this.OwningGroup.Header != null
                             && this.OwningGroup.HeaderContentControl != null)
                    {
                        this.headerPeer = new RibbonGroupHeaderAutomationPeer(this.OwningGroup.HeaderContentControl);
                    }
                }

                return this.headerPeer;
            }
        }

        
        protected override List<AutomationPeer> GetChildrenCore()
        {
            var list = base.GetChildrenCore();

            if (this.HeaderPeer != null)
            {
                if (list is null)
                {
                    list = new List<AutomationPeer>(1);
                }

                list.Add(this.HeaderPeer);
            }

            return list;
        }

        
        protected override string GetClassNameCore()
        {
            return this.Owner.GetType().Name;
        }

        
        protected override string GetNameCore()
        {
            var name = base.GetNameCore();

            if (string.IsNullOrEmpty(name))
            {
                name = (this.Owner as IHeaderedControl)?.Header as string;
            }

            return name ?? string.Empty;
        }

        
        public override object GetPattern(PatternInterface patternInterface)
        {
            switch (patternInterface)
            {
                case PatternInterface.ExpandCollapse:
                    return this.IsCollapseOrExpandValid ? this : base.GetPattern(patternInterface);

                case PatternInterface.Scroll:
                    return base.GetPattern(patternInterface);

                default:
                    return base.GetPattern(patternInterface);
            }
        }

        
        protected override void SetFocusCore()
        {
        }

        
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return this.IsCollapseOrExpandValid
                ? AutomationControlType.Button
                : AutomationControlType.Group;
        }

        #region IExpandCollapseProvider Members

        
        void IExpandCollapseProvider.Expand()
        {
            if (this.IsCollapseOrExpandValid == false)
            {
                return;
            }

            this.OwningGroup.IsDropDownOpen = true;
        }

        
        void IExpandCollapseProvider.Collapse()
        {
            if (this.IsCollapseOrExpandValid == false)
            {
                return;
            }

            this.OwningGroup.IsDropDownOpen = false;
        }

        
        ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState => this.IsCollapseOrExpandValid
            ? ExpandCollapseState.Collapsed
            : ExpandCollapseState.Expanded;

        private bool IsCollapseOrExpandValid => this.OwningGroup.State is RibbonGroupBoxState.Collapsed || this.OwningGroup.State is RibbonGroupBoxState.QuickAccess;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue)
        {
            this.RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty,
                oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed,
                newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
        }

        #endregion

        #region IScrollItemProvider Members

        
        void IScrollItemProvider.ScrollIntoView()
        {
            this.OwningGroup.BringIntoView();
        }

        #endregion
    }
}
