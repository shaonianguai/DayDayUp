using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

namespace WzUXRibbon.Internal
{
    internal static class UIHelper
    {
        public static DependencyObject GetFirstVisualChild(DependencyObject parent)
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            return childrenCount == 0
                ? null
                : VisualTreeHelper.GetChild(parent, 0);
        }

        public static T FindImmediateVisualChild<T>(DependencyObject parent, Predicate<T> predicate)
            where T : DependencyObject
        {
            foreach (var child in GetVisualChildren(parent))
            {
                if (child is T obj
                    && predicate(obj))
                {
                    return obj;
                }
            }

            return null;
        }

        public static TChildItem FindVisualChild<TChildItem>(DependencyObject parent)
            where TChildItem : DependencyObject
        {
            foreach (var child in GetVisualChildren(parent))
            {
                if (child is TChildItem item)
                {
                    return item;
                }

                var childOfChild = FindVisualChild<TChildItem>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }

            return null;
        }

        public static IEnumerable<DependencyObject> GetVisualChildren(DependencyObject parent)
        {
            var visualChildrenCount = VisualTreeHelper.GetChildrenCount(parent);

            for (var i = 0; i < visualChildrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                yield return child;
            }
        }

        public static T GetParent<T>(DependencyObject element, Predicate<T> filter = null)
            where T : DependencyObject
        {
            if (element is null)
            {
                return null;
            }

            {
                var item = GetVisualParent(element);

                while (item != null)
                {
                    if (item is T variable
                        && (filter?.Invoke(variable) ?? true))
                    {
                        return variable;
                    }

                    item = GetVisualParent(item) ?? LogicalTreeHelper.GetParent(item);
                }
            }

            {
                var item = LogicalTreeHelper.GetParent(element);

                while (item != null)
                {
                    if (item is T variable
                        && (filter?.Invoke(variable) ?? true))
                    {
                        return variable;
                    }

                    item = LogicalTreeHelper.GetParent(item);
                }
            }

            return null;
        }

        public static DependencyObject GetVisualOrLogicalParent(DependencyObject element)
        {
            return GetVisualParent(element) ?? LogicalTreeHelper.GetParent(element);
        }

        public static DependencyObject GetVisualParent(DependencyObject element)
        {
            if (element is null)
            {
                return null;
            }

            if (element is ContentElement contentElement)
            {
                var parent = ContentOperations.GetParent(contentElement);

                if (parent != null)
                {
                    return parent;
                }

                var frameworkContentElement = contentElement as FrameworkContentElement;
                return frameworkContentElement?.Parent;
            }

            return VisualTreeHelper.GetParent(element);
        }

        public static AdornerLayer GetAdornerLayer(Visual visual)
        {
            if (visual is null)
            {
                throw new ArgumentNullException(nameof(visual));
            }

            if (visual is AdornerDecorator decorator)
            {
                return decorator.AdornerLayer;
            }

            if (visual is ScrollContentPresenter scrollContentPresenter)
            {
                return scrollContentPresenter.AdornerLayer;
            }

            return AdornerLayer.GetAdornerLayer(visual);
        }

        public static IEnumerable<T> GetAllItemContainers<T>(ItemsControl itemsControl)
            where T : class
        {
            return GetAllItemContainers<T>(itemsControl.ItemContainerGenerator);
        }

        public static IEnumerable<T> GetAllItemContainers<T>(ItemContainerGenerator itemContainerGenerator)
            where T : class
        {
            for (var i = 0; i < itemContainerGenerator.Items.Count; i++)
            {
                if (itemContainerGenerator.ContainerFromIndex(i) is T container)
                {
                    yield return container;
                }
            }
        }
    }
}
