using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Markup;
using System.Windows;

namespace WzUXRibbon.Controls
{
    [ContentProperty(nameof(Children))]
    [SuppressMessage("Microsoft.Naming", "CA1702", Justification = "We mean here 'bar row' instead of 'barrow'")]
    public class RibbonToolBarRow : DependencyObject
    {
        #region Properties

        /// <summary>
        /// Gets rows
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<DependencyObject> Children { get; } = new ObservableCollection<DependencyObject>();

        #endregion

        #region Initialization

        #endregion
    }
}
