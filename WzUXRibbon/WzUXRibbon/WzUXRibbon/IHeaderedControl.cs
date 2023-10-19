﻿using System.Windows.Controls;
using System.Windows;

namespace WzUXRibbon
{
    public interface IHeaderedControl
    {
        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        object Header { get; set; }

        /// <summary>
        ///     HeaderTemplate is the template used to display the header.
        /// </summary>
        DataTemplate HeaderTemplate { get; set; }

        /// <summary>
        ///     HeaderTemplateSelector allows the application writer to provide custom logic
        ///     for choosing the template used to display the header of each item.
        /// </summary>
        /// <remarks>
        ///     This property is ignored if <seealso cref="HeaderTemplate"/> is set.
        /// </remarks>
        DataTemplateSelector HeaderTemplateSelector { get; set; }
    }
}
