using System.Collections.Generic;
using System.Collections.ObjectModel;
using Syncfusion.Windows.Chart;

namespace ArrayVisualizerExt.ChartData
{
    internal class VisualizerPointsCollection : ObservableCollection<VisualizerDataPoint>, IChartData
    {
        public VisualizerPointsCollection()
        {
        }

        public VisualizerPointsCollection(IEnumerable<double> values)
        {
            int i = 0;
            foreach (double value in values)
            {
                i++;
                Add(new VisualizerDataPoint(i, value));
            }
        }

        public new IChartDataPoint this[int index] => base[index];

        public ChartValueType XValueType { get; set; }

        public ChartValueType ChartXValueType { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}