using Syncfusion.Windows.Chart;

namespace ArrayVisualizerExt.ChartData
{
  internal class VisualizerDataPoint : IChartDataPoint
  {

    public VisualizerDataPoint(double x, double y)
    {
      X = x;
      Y = y;
      Values = new[] { y };
    }

    public double X { get; set; }
    public double Y { get; set; }
    public double[] Values { get; set; }
    public bool IsEmpty { get; set; }
    public bool EmptyPoint { get; set; }
    public string Label { get; set; }
    public bool Visible { get; set; }
    public object StringItem { get; set; }
    public ChartSegment ParentSegment { get; set; }
    public object Item { get; set; }
    public object Tag { get; set; }

    public object Clone()
    {
      VisualizerDataPoint customPoint = new VisualizerDataPoint(X, Y);
      return customPoint;
    }

    public void Dispose() { }
  }
}



