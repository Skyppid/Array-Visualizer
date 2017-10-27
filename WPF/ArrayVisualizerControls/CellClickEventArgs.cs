using System.Windows;

namespace ArrayVisualizerControls
{
    public class CellClickEventArgs : RoutedEventArgs
    {
        public CellClickEventArgs()
        {
        }

        public CellClickEventArgs(RoutedEvent routedEvent)
            : base(routedEvent)
        {
        }

        public CellClickEventArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
        }

        public CellClickEventArgs(object data, string toolTipPrefix, RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
            Data = data;
            ToolTipPrefix = toolTipPrefix;
        }

        public object Data { get; set; }
        public string ToolTipPrefix { get; set; }
    }
}