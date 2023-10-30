using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WzUXRibbon.Collections
{
    public class ItemCollectionWithLogicalTreeSupport<TItem> : ObservableCollection<TItem>
    {
        public ItemCollectionWithLogicalTreeSupport(ILogicalChildSupport parent)
        {
            this.Parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        public bool IsOwningItems { get; private set; } = true;

        public ILogicalChildSupport Parent { get; }

        public void AquireLogicalOwnership()
        {
            if (this.IsOwningItems)
            {
                return;
            }

            this.IsOwningItems = true;

            foreach (var item in this.Items)
            {
                this.AddLogicalChild(item);
            }
        }

        public void ReleaseLogicalOwnership()
        {
            if (this.IsOwningItems == false)
            {
                return;
            }

            foreach (var item in this.Items)
            {
                this.RemoveLogicalChild(item);
            }

            this.IsOwningItems = false;
        }

        public IEnumerable<TItem> GetLogicalChildren()
        {
            if (this.IsOwningItems == false)
            {
                return Enumerable.Empty<TItem>();
            }

            return this.Items;
        }

        protected override void InsertItem(int index, TItem item)
        {
            base.InsertItem(index, item);

            this.AddLogicalChild(item);
        }

        protected override void RemoveItem(int index)
        {
            this.RemoveLogicalChild(this[index]);

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, TItem item)
        {
            var oldItem = this[index];

            if (oldItem != null)
            {
                this.RemoveLogicalChild(oldItem);
            }

            base.SetItem(index, item);

            if (item != null)
            {
                this.AddLogicalChild(item);
            }
        }

        protected override void ClearItems()
        {
            foreach (var item in this.Items)
            {
                this.RemoveLogicalChild(item);
            }

            base.ClearItems();
        }

        private void AddLogicalChild(TItem item)
        {
            if (this.IsOwningItems == false
                || item == null)
            {
                return;
            }

            this.Parent.AddLogicalChild(item);
        }

        private void RemoveLogicalChild(TItem item)
        {
            if (this.IsOwningItems == false
                || item == null)
            {
                return;
            }

            this.Parent.RemoveLogicalChild(item);
        }
    }
}
