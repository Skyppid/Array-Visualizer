using System;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.Windows.Chart;

namespace ArrayVisualizerExt
{
    public class ChartSeriesData : IChartData
    {
        private readonly double[] _arr;

        #region IChartData Members

        public ChartSeriesData(IEnumerable<double> data)
        {
            _arr = data.ToArray();
        }

        public ChartValueType ChartXValueType
        {
            get { return ChartValueType.Double; }
            set { }
        }

        public int Count
        {
            get { return _arr.Length; }
        }

        public ChartValueType XValueType
        {
            get { return ChartValueType.Double; }
        }

        public IChartDataPoint this[int index]
        {
            get { return new ChartDataPoint {X = index, Y = _arr[index]}; }
        }

        #endregion

        #region INotifyCollectionChanged Members

        public event System.Collections.Specialized.NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }

    internal class ChartDataPoint : IChartDataPoint
    {
        #region IChartDataPoint Members

        public bool EmptyPoint { get; set; }

        public bool IsEmpty
        {
            get { throw new NotImplementedException(); }
        }

        public object Item { get; set; }
        public string Label { get; set; }
        public ChartSegment ParentSegment { get; set; }
        public object StringItem { get; set; }
        public object Tag { get; set; }
        public double[] Values { get; set; }
        public bool Visible { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new ChartDataPoint();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}