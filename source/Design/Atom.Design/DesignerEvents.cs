using System.Windows;

namespace Atom.Design
{
    public static class DesignerEvents
    {
        public static readonly RoutedEvent DesignerChangedEvent;

        static DesignerEvents()
        {
            DesignerChangedEvent = EventManager.RegisterRoutedEvent("DesignerChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DesignerEvents));
        }

        public static void SubscribeDesignerChanged(IObjectDesigner designer, RoutedEventHandler handler)
        {
            UIElement element = designer as UIElement;
            if (element != null)
            {
                element.AddHandler(DesignerChangedEvent, handler);
            }
        }

        internal static void RaiseDesignerChanged(UIElement element)
        {
            RoutedEventArgs eventArgs = new RoutedEventArgs(DesignerChangedEvent);
            element.RaiseEvent(eventArgs);
        }
    }
}
