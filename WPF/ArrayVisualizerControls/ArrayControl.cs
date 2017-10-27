using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using LinqLib.Array;

namespace ArrayVisualizerControls
{
  public abstract class ArrayControl : UserControl
  {

    public event EventHandler<CellClickEventArgs> CellClick;

    #region Constants

    public const double MAX_ELEMENTS = 10000;

    public const double SIZE_FACTOR_3_D = .75;

    #endregion

    #region Fields

    private readonly Grid _arrayGrid;
    private ScrollViewer _arrayContainer;
    private Size _cellSize;
    private string _formatter;
    private Func<object, string, string> _captionBuilder;
    private Popup _popup;
    private Transform _sideTransformer;
    private string _tooltipPrefix;
    private Transform _topTransformer;

    #endregion

    #region Constructors and Destructors

    protected ArrayControl()
    {
      _arrayGrid = new Grid();
      Init();
    }

    private void Init()
    {
      AddChild(_arrayGrid);
      _cellSize = new Size(80, 55);
      _formatter = string.Empty;
      _captionBuilder = DefaultCaptionBuilder;
      _tooltipPrefix = string.Empty;
      LeftBracket = '[';
      RightBracket = ']';
    }

    #endregion

    #region Public Properties

    public double CellHeight
    {
      get { return _cellSize.Height; }
      set { _cellSize.Height = value; }
    }

    public Size CellSize
    {
      get { return _cellSize; }
      set { _cellSize = value; }
    }

    public double CellWidth
    {
      get { return _cellSize.Width; }
      set { _cellSize.Width = value; }
    }

    public ArrayControl ChildArray
    {
      get
      {
        if (_arrayContainer != null)
          return (ArrayControl)_arrayContainer.Content;

        return null;
      }
    }

    public Array Data { get; private set; }

    public int ElementsCount
    {
      get { return DimX * DimY * DimZ * DimA; }
    }

    public string Formatter
    {
      get { return _formatter; }
      set { _formatter = value; }
    }

    public Func<object, string, string> CaptionBuilder
    {
      get { return _captionBuilder; }
      set
      {
        _captionBuilder = value ?? DefaultCaptionBuilder;
      }
    }

    public bool Truncated { get; protected set; }

    #endregion

    #region Properties

    protected int DimX { get; set; }
    protected int DimY { get; set; }
    protected int DimZ { get; set; }
    protected int DimA { get; set; }

    public char LeftBracket { get; set; }
    public char RightBracket { get; set; }

    #endregion

    #region Public Methods and Operators


    public void Render()
    {
      try
      {
        if (Data.Length > 500)
          Mouse.OverrideCursor = Cursors.Wait;

        _arrayGrid.Children.Clear();
        RenderBlankGrid();
        DrawContent();
      }
      finally
      {
        Mouse.OverrideCursor = null;
      }
    }

    public void SetControlData(Array data)
    {
      SetControlData(data, string.Empty);
    }

    public void SetControlData(Array data, string popupTooltipPrefix)
    {
      Data = data;
      _tooltipPrefix = popupTooltipPrefix;
      if (data == null)
        _arrayGrid.Children.Clear();
      else
      {
        SetAxisSize();
        Render();
      }
    }

    #endregion

    #region Methods

    internal void SetTransformers()
    {
      _topTransformer = GetTopTransformation();
      _sideTransformer = GetSideTransformation();
    }

    protected Label AddLabel(ArrayRenderSection section, string toolTipCoordinates, double x, double y, object data)
    {
      if (data == null)
        throw new ArgumentNullException("data");
      
      Type dataType = data.GetType();
      Label label = new Label();
      switch (section)
      {
        case ArrayRenderSection.Front:
          label.Margin = new Thickness(x, y, 0, 0);
          break;
        case ArrayRenderSection.Top:
          label.Margin = new Thickness(x + 1, y - 1, 0, 0);
          label.RenderTransform = _topTransformer;
          break;
        case ArrayRenderSection.Side:
          label.Margin = new Thickness(x, y, 0, 0);
          label.RenderTransform = _sideTransformer;
          break;
      }

      label.Width = _cellSize.Width;
      label.Height = _cellSize.Height;

      label.HorizontalAlignment = HorizontalAlignment.Left;
      label.VerticalAlignment = VerticalAlignment.Top;
      label.HorizontalContentAlignment = HorizontalAlignment.Center;
      label.VerticalContentAlignment = VerticalAlignment.Center;

      if (dataType.IsArray)
        label.Content = ArrayCaptionBuilder((Array)data);
      else
        label.Content = _captionBuilder(data, _formatter);

      label.ToolTip = string.Format("{0}{1} : {2}", _tooltipPrefix, toolTipCoordinates, label.Content);

      if (!dataType.IsPrimitive && dataType != typeof(string))
      {
        label.Tag = data;
        label.Cursor = Cursors.Hand;
        label.MouseUp += label_MouseUp;
      }

      _arrayGrid.Children.Add(label);

      return label;
    }

    protected void AddLine(double x1, double y1, double x2, double y2)
    {
      var line = new Line
      {
        Stroke = Brushes.Black,
        StrokeThickness = 1,
        X1 = x1,
        X2 = x2,
        Y1 = y1,
        Y2 = y2
      };
      _arrayGrid.Children.Add(line);
    }

    protected abstract void DrawContent();

    protected Transform GetSideTransformation()
    {
      double angle = Math.Atan((CellHeight / CellWidth) * SIZE_FACTOR_3_D) * 180 / Math.PI;
      var skt = new SkewTransform(0, -angle);
      var sct = new ScaleTransform(SIZE_FACTOR_3_D, 1, 0, 0);
      var tg = new TransformGroup();
      tg.Children.Add(skt);
      tg.Children.Add(sct);
      return tg;
    }

    protected Transform GetTopTransformation()
    {
      double angle = Math.Atan((CellWidth / CellHeight) * SIZE_FACTOR_3_D) * 180 / Math.PI;
      var skt = new SkewTransform(-angle, 0);
      var sct = new ScaleTransform(1, SIZE_FACTOR_3_D, 0, 0);
      var tg = new TransformGroup();
      tg.Children.Add(skt);
      tg.Children.Add(sct);
      return tg;
    }

    protected abstract void RenderBlankGrid();

    private static int AdjustDimensionSize(int originalSize, double ratio)
    {
      var size = (int)(originalSize / ratio + .5);
      return Math.Max(1, size);
    }

    private void HideSelfAndChildren()
    {
      if (_popup != null)
      {
        _popup.IsOpen = false;
        ArrayControl child = ChildArray;
        if (child != null)
          child.HideSelfAndChildren();
      }
    }

    private void InitPopup(Color backgroundColor)
    {
      _popup = new Popup
      {
        Placement = PlacementMode.MousePoint,
        StaysOpen = false
      };
      Grid popupGrid = new Grid
      {
        Background = new SolidColorBrush(backgroundColor)
      };

      _popup.Child = popupGrid;
      popupGrid.Children.Add(new Border { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(.25) });

      _arrayContainer = new ScrollViewer
                              {
                                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                              };
      popupGrid.Children.Add(_arrayContainer);

      _popup.MaxWidth = SystemParameters.PrimaryScreenWidth * .85;
      _popup.MaxHeight = SystemParameters.PrimaryScreenHeight * .85;
    }

    private void SetAxisSize() //!!!!!
    {
      int ranks = Data.Rank;

      DimX = DimY = DimZ = DimA = 1;

      DimX = Data.GetLength(ranks - 1);
      if (ranks > 1)
      {
        DimY = Data.GetLength(ranks - 2);
        if (ranks > 2)
        {
          DimZ = Data.GetLength(ranks - 3);
          if (ranks > 3)
            DimA = Data.GetLength(ranks - 4);
        }
      }

      Truncated = Data.Length > MAX_ELEMENTS;

      if (Truncated)
      {
        double r = Math.Pow(Data.Length / MAX_ELEMENTS, 1.0 / ranks);
        DimA = AdjustDimensionSize(DimA, r);
        DimZ = AdjustDimensionSize(DimZ, r);
        DimY = AdjustDimensionSize(DimY, r);
        DimX = AdjustDimensionSize(DimX, r);
      }
    }

    public void ShowArrayPopup(UIElement target, Array data, string popupTooltipPrefix, Color backgroundColor)
    {
      if (data == null)
        throw new ArgumentNullException("data"); 
      
      if (_popup == null)
        InitPopup(backgroundColor);

      HideSelfAndChildren();

      ArrayControl arrCtl = GetArrayControl(data.Rank);

      if (arrCtl != null)
      {
        arrCtl.LeftBracket = LeftBracket;
        arrCtl.RightBracket = RightBracket;

        arrCtl.CaptionBuilder = _captionBuilder;
        arrCtl.CellClick = CellClick;
        arrCtl.Formatter = _formatter;
        arrCtl.CellHeight = CellHeight;
        arrCtl.CellWidth = CellWidth;
        arrCtl.SetControlData(data, popupTooltipPrefix);

        arrCtl.Padding = new Thickness(8);
        arrCtl.Width += 16;
        arrCtl.Height += 16;

        _arrayContainer.Content = arrCtl;
        if (_popup != null)
        {
          _popup.PlacementTarget = target;
          _popup.IsOpen = true;
        }
      }
    }

    public static ArrayControl GetArrayControl(int rank)
    {
      switch (rank)
      {
        case 1:
          return new Array1D();          
        case 2:
          return new Array2D();        
        case 3:
          return new Array3D();          
        case 4:
          return new Array4D();       
        default:
          return null;
      }
    }

    private void label_MouseUp(object sender, MouseButtonEventArgs e)
    {
      OnCellClick(sender, e);
    }

    private void OnCellClick(object sender, RoutedEventArgs e)
    {
      FrameworkElement fe = (FrameworkElement)sender;
      string toolTip = "";

      if (fe.ToolTip != null)
      {
        toolTip = (string)fe.ToolTip;
        int pos = toolTip.IndexOf(":", StringComparison.Ordinal);
        if (pos > 0)
          toolTip = toolTip.Substring(0, pos - 1);
      }

      if (CellClick == null)
      {
        FrameworkElement element = fe;
        if (fe.Tag.GetType().IsArray)
        {
          Array data = (Array)fe.Tag;
          object depObj = null;
          while (depObj == null)
          {
            element = (FrameworkElement)element.Parent;
            depObj = element.GetValue(BackgroundProperty);
          }
          Color color = ((SolidColorBrush)depObj).Color;
          ShowArrayPopup((UIElement)sender, data, toolTip, color);
        }
      }
      else
        CellClick(this, new CellClickEventArgs(fe.Tag, toolTip, e.RoutedEvent, sender));
    }

    #endregion

    private string DefaultCaptionBuilder(object data, string numberFormatter)
    {
      double number;
      string text = (data ?? "").ToString();
      if (double.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out number))
        text = number.ToString(numberFormatter, CultureInfo.CurrentCulture);
      return text;
    }

    private string ArrayCaptionBuilder(Array data)
    {
      int[] dims = data.GetDimensions();
      string dimsText = string.Join(", ", dims);
      string text = string.Format("{{{0}}}", data.GetType().Name);
      int pos1 = text.IndexOf(RightBracket);
      int pos2 = text.IndexOf(LeftBracket);
      text = text.Substring(0, pos1) + LeftBracket + dimsText + RightBracket + text.Substring(pos2 + 1);
      return text;
    }
  }
}


