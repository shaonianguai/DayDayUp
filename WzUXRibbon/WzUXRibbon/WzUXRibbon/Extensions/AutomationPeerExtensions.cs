using System.Reflection;
using System.Windows.Automation.Peers;
using System.Windows;

namespace WzUXRibbon.Extensions
{
    internal static class AutomationPeerExtensions
    {
        private static readonly MethodInfo getWrapperPeerMethodInfo = typeof(ItemAutomationPeer).GetMethod("GetWrapperPeer", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo getWrapperMethodInfo = typeof(ItemAutomationPeer).GetMethod("GetWrapper", BindingFlags.Instance | BindingFlags.NonPublic);

        internal static AutomationPeer GetWrapperPeer(this ItemAutomationPeer automationPeer)
        {
            return (AutomationPeer)getWrapperPeerMethodInfo?.Invoke(automationPeer, null);
        }

        internal static UIElement GetWrapper(this ItemAutomationPeer automationPeer)
        {
            return (UIElement)getWrapperMethodInfo?.Invoke(automationPeer, null);
        }
    }
}
