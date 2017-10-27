using System;
using System.Windows;

namespace ArrayVisualizerExt
{
    /// <summary>
    /// Interaction logic for LargeArrayHandler.xaml
    /// </summary>
    public partial class LargeArrayHandler
    {
        public event EventHandler<RoutedEventArgs> LoadArrayRequest;

        public LargeArrayHandler()
        {
            InitializeComponent();
        }

        public LargeArrayHandler(int itemsCount, int autoMax, int absoluteMax)
            : this()
        {
            const string BASEMASSAGE =
                "The selected array contains {0:N0} elements.\r\nFor performance reasons, the Array Visualizer will only display small arrays ({1:N0} elements or less) automatically.\r\n";
            if (itemsCount > absoluteMax)
            {
                ButtonLoadArray.Visibility = Visibility.Hidden;
                MessageBlock.Text =
                    string.Format(
                        BASEMASSAGE +
                        "You can force medium size arrays ({1:N0} to {2:N0} elements) to load manually.\r\n\r\nThis array is too large!",
                        itemsCount, autoMax, absoluteMax);
            }
            else
            {
                ButtonLoadArray.Visibility = Visibility.Visible;
                MessageBlock.Text = string.Format(BASEMASSAGE + "Click \"Load Array\" to load the array anyway.",
                    itemsCount, autoMax);
            }
        }

        private void LoadArray_Click(object sender, RoutedEventArgs e)
        {
            OnLoadArrayRequest();
        }

        protected void OnLoadArrayRequest()
        {
            if (LoadArrayRequest != null)
                LoadArrayRequest(this, new RoutedEventArgs());
        }
    }
}