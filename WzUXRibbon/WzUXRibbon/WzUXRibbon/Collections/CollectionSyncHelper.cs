using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace WzUXRibbon.Collections
{
    public class CollectionSyncHelper<TItem>
    {
        public CollectionSyncHelper(ObservableCollection<TItem> source, IList target)
        {
            this.Source = source ?? throw new ArgumentNullException(nameof(source));
            this.Target = target ?? throw new ArgumentNullException(nameof(target));

            this.SyncTarget();

            this.Source.CollectionChanged += this.SourceOnCollectionChanged;
        }

        public ObservableCollection<TItem> Source { get; }

        public IList Target { get; }

        private void SyncTarget()
        {
            this.Target.Clear();

            foreach (var item in this.Source)
            {
                this.Target.Add(item);
            }
        }

        private void SourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (var i = 0; i < e.NewItems?.Count; i++)
                    {
                        this.Target.Insert(e.NewStartingIndex + i, e.NewItems[i]);
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (var item in e.OldItems)
                        {
                            this.Target.Remove(item);
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (e.OldItems != null)
                    {
                        foreach (var item in e.OldItems)
                        {
                            this.Target.Remove(item);
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (var item in e.NewItems)
                        {
                            this.Target.Add(item);
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    this.SyncTarget();

                    break;
            }
        }
    }
}
