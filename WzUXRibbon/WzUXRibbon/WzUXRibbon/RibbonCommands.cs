using System.Windows.Input;

namespace WzUXRibbon
{
    public static class RibbonCommands
    {
        /// <summary>
        /// Gets the value that represents the Open Backstage command
        /// </summary>
        public static readonly RoutedCommand OpenBackstage = new RoutedUICommand("Open backstage", nameof(OpenBackstage), typeof(RibbonCommands));
    }
}
