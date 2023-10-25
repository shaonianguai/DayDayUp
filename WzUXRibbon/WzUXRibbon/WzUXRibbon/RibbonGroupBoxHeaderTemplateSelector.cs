using System.Windows.Controls;
using System.Windows;
using WzUXRibbon.Controls;

namespace WzUXRibbon
{
    public class RibbonGroupBoxHeaderTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Gets a static instance of <see cref="RibbonGroupBoxHeaderTemplateSelector"/>.
        /// </summary>
        public static readonly RibbonGroupBoxHeaderTemplateSelector Instance = new RibbonGroupBoxHeaderTemplateSelector();

        /// <inheritdoc />
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = (FrameworkElement)container;

            if (RibbonGroupBox.GetIsCollapsedHeaderContentPresenter(element))
            {
                return (DataTemplate)element.FindResource("Ribbon.DataTemplates.RibbonGroupBox.TwoLineHeader");
            }

            return (DataTemplate)element.FindResource("Ribbon.DataTemplates.RibbonGroupBox.OneLineHeader");
        }
    }
}
