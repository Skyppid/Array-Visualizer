using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ArrayVisualizerControls;
using ArrayVisualizerExt.ArrayLoaders;
using ArrayVisualizerExt.TypeParsers;
using EnvDTE;
using EnvDTE80;
using LinqLib.Array;
using Microsoft.VisualStudio.Shell;
using Syncfusion.Linq;
using Syncfusion.Windows.Chart;
using Expression = EnvDTE.Expression;

namespace ArrayVisualizerExt
{
    public partial class ArrayVisualizerToolControl : IDisposable
    {
        #region Fields

        private readonly DTE2 _dte;
        private List<ExpressionInfo> _expressions;
        private DebuggerEvents _debugerEvents;
        private ArrayControl _arrCtl;
        private Chart _chartCtl;

        private Array _data;

        //private Type undelyingExpressionType;
        private int[] _dimensions;

        private IArrayLoader _arrayLoader;
        private ParsersCollection _parsers;
        private Exception _lastLoadException;
        private readonly HashSet<Type> _loadedParsers;

        private bool _arraysPending;
        private bool _toolActive;
        private DisplayMode _displayMode;

        private string _lastSelectedArray;

        #endregion

        #region Constructor

        public ArrayVisualizerToolControl()
        {
            InitializeComponent();

            _dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
            _loadedParsers = new HashSet<Type>();

            LoadSettings();

            SetDebugEvents();
            _toolActive = true;
            _arraysPending = true;
            ShowArrays();
        }

        #endregion

        #region Debugger Events

        private void SetDebugEvents()
        {
            _debugerEvents = _dte.Events.DebuggerEvents;
            _debugerEvents.OnEnterBreakMode += DebuggerEvents_OnEnterBreakMode;
            _debugerEvents.OnEnterDesignMode += debugerEvents_OnEnterDesignMode;
            _debugerEvents.OnEnterRunMode += debugerEvents_OnEnterRunMode;
        }

        private void debugerEvents_OnEnterRunMode(dbgEventReason reason)
        {
            SaveSettings();
            _arraysPending = false;
            ClearVisualizer();
        }

        private void debugerEvents_OnEnterDesignMode(dbgEventReason reason)
        {
            SaveSettings();
            _arraysPending = false;
            ClearVisualizer();
        }

        private void DebuggerEvents_OnEnterBreakMode(dbgEventReason reason, ref dbgExecutionAction executionAction)
        {
            _arraysPending = true;
            ShowArrays();
        }

        public void ToolActivated()
        {
            _toolActive = true;
            ShowArrays();
        }

        public void ToolDeactivated()
        {
            _toolActive = false;
        }

        #endregion

        #region Other Events

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                string text = textBox.Text + e.Text;
                bool ok = double.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out var temp);
                e.Handled = !ok;
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void rotateButton_Click(object sender, RoutedEventArgs e)
        {
            RotateAxis r = RotateAxis.RotateNone;
            int angle = (int) angelComboBox.SelectedItem;

            switch ((string) axisComboBox.SelectedItem)
            {
                case "X":
                    r = RotateAxis.RotateX;
                    break;
                case "Y":
                    r = RotateAxis.RotateY;
                    break;
                case "Z":
                    r = RotateAxis.RotateZ;
                    break;
                case "A":
                    r = RotateAxis.RotateA;
                    break;
            }

            int[] dims = _arrCtl.Data.GetDimensions();
            switch (_arrCtl.Data.Rank)
            {
                case 2:
                    Reset((_arrCtl.Data.AsEnumerable<object>().ToArray(dims[0], dims[1])).Rotate(angle));
                    break;
                case 3:
                    Reset((_arrCtl.Data.AsEnumerable<object>().ToArray(dims[0], dims[1], dims[2])).Rotate(r, angle));
                    break;
                case 4:
                    Reset((_arrCtl.Data.AsEnumerable<object>().ToArray(dims[0], dims[1], dims[2], dims[3])).Rotate(r,
                        angle));
                    break;
            }
        }

        private void arraysListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                ExpressionInfo expressionInfo = (ExpressionInfo) e.AddedItems[0];
                LoadResults result = LoadArray(expressionInfo.Expression, false);
                SetupControls(expressionInfo);

                switch (result)
                {
                    case LoadResults.LargeArray:
                        LargeArrayHandler largeArrayHandler =
                            new LargeArrayHandler(expressionInfo.Expression.DataMembers.Count, 2000, 40000);
                        largeArrayHandler.LoadArrayRequest += largeArrayHandler_LoadArrayRequest;
                        mainPanel.Children.Add(largeArrayHandler);
                        break;
                    case LoadResults.NotSupported:
                        break;
                    case LoadResults.Exception:
                        Label errorLabel = new Label
                        {
                            Content =
                                $"Error rendering array '{expressionInfo.Name}'\r\n\r\n'{_lastLoadException.Message}'"
                        };
                        mainPanel.Children.Clear();
                        mainPanel.Children.Add(errorLabel);
                        break;
                }
            }
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            Reset(_data);
        }

        private void supportlabel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://bit.ly/155n1RK");
        }

        private void gridLinesType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetGridLines();
        }

        private void arrCtl_CellClick(object sender, CellClickEventArgs e)
        {
            Array values = new DefaultParser(_arrayLoader).GetValues((Expression) e.Data);
            Color color = ((SolidColorBrush) mainPanel.Background).Color;
            ((ArrayControl) sender).ShowArrayPopup((UIElement) e.Source, values, e.ToolTipPrefix, color);
        }

        private void largeArrayHandler_LoadArrayRequest(object sender, RoutedEventArgs e)
        {
            if (arraysListBox.SelectedItems.Count == 1)
            {
                ExpressionInfo expressionInfo = (ExpressionInfo) arraysListBox.SelectedItem;
                LoadArray(expressionInfo.Expression, true);
                SetupControls(expressionInfo);
            }
        }

        private void VisualizerTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Equals(e.OriginalSource, VisualizerTab))
            {
                ExpressionInfo expressionInfo = (ExpressionInfo) arraysListBox.SelectedItem;
                SetupControls(expressionInfo);
            }
        }

        private void chartType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetChartType();
        }

        private void lineType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetChartLineStroke();
        }

        private void syncFusionLabel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://bit.ly/Wq50dg");
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            if (_arrCtl != null)
                Reset(_arrCtl.Data);
        }

        private void Parser_CheckedChanged(object sender, RoutedEventArgs e)
        {
            CheckBox chkControl = (CheckBox) sender;

            Type parserType = Type.GetType((string) chkControl.Tag);

            if (parserType != null)
                if (chkControl.IsChecked.HasValue && chkControl.IsChecked.Value)
                    _loadedParsers.Add(parserType);
                else
                    _loadedParsers.Remove(parserType);

            _arraysPending = true;
            ShowArrays();
        }

        private void GridSplitter_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            double width = MainGrid.ColumnDefinitions[0].Width.Value;
            if (width > 212.01 || width < 11.99)
                MainGrid.ColumnDefinitions[0].Width = new GridLength(212, GridUnitType.Pixel);
            else
                MainGrid.ColumnDefinitions[0].Width = new GridLength(360, GridUnitType.Pixel);
        }

        #endregion

        #region Methods

        private void ClearVisualizer()
        {
            if (arraysListBox.SelectedValue != null)
                _lastSelectedArray = ((ExpressionInfo) arraysListBox.SelectedValue).FullName;
            arraysListBox.ItemsSource = null;
            arraysListBox.Items.Clear();
            mainPanel.Children.Clear();
            rotateGrid.IsEnabled = false;
        }

        private void LoadScopeArrays()
        {
            ClearVisualizer();
            _expressions = new List<ExpressionInfo>();

            if (_dte.Debugger.CurrentMode == dbgDebugMode.dbgBreakMode)
            {
                foreach (Expression expression in _dte.Debugger.CurrentStackFrame.Locals)
                    _expressions.AddRange(_arrayLoader.GetArrays(string.Empty, expression, _parsers, 0));

                arraysListBox.ItemsSource = _expressions.OrderBy(a => a.SectionCode).ThenBy(a => a.Name);
                arraysListBox.DisplayMemberPath = "FullName";
            }

            if (!string.IsNullOrEmpty(_lastSelectedArray))
            {
                foreach (ExpressionInfo item in arraysListBox.Items)
                {
                    if (item.FullName == _lastSelectedArray)
                        arraysListBox.SelectedItem = item;
                }
            }
        }

        private LoadResults LoadArray(Expression expression, bool ignoreArraySize)
        {
            _lastLoadException = null;
            _data = null;
            try
            {
                if (expression.Value != "null")
                {
                    object[] values = null;

                    foreach (ITypeParser parser in _parsers.Where(p => p.IsExpressionTypeSupported(expression)))
                    {
                        _dimensions = parser.GetDimensions(expression);
                        int count = parser.GetMembersCount(expression);
                        if (ignoreArraySize || count <= 2000)
                            values = parser.GetValues(expression);
                        else
                            return LoadResults.LargeArray;

                        break;
                    }

                    switch (_dimensions.Length)
                    {
                        case 1:
                            _data = values.ToArray(_dimensions[0]);
                            break;
                        case 2:
                            _data = values.ToArray(_dimensions[0], _dimensions[1]);
                            break;
                        case 3:
                            _data = values.ToArray(_dimensions[0], _dimensions[1], _dimensions[2]);
                            break;
                        case 4:
                            _data = values.ToArray(_dimensions[0], _dimensions[1], _dimensions[2], _dimensions[3]);
                            break;
                        default:
                            return LoadResults.NotSupported;
                    }
                }
            }
            catch (Exception ex)
            {
                _lastLoadException = ex;
                return LoadResults.Exception;
            }
            return LoadResults.Success;
        }

        private void SetupArrayControl(ExpressionInfo expressionInfo)
        {
            if (_arrCtl != null && _arrCtl.Tag == expressionInfo)
                return;

            SetRotationOptions(_dimensions.Length);

            ArrayControl newArrCtl = ArrayControl.GetArrayControl(_dimensions.Length);
            if (newArrCtl == null)
                return;
            else
                _arrCtl = newArrCtl;

            _arrCtl.Formatter = formatterTextBox.Text;
            _arrCtl.CaptionBuilder = GetCaptionBuilder(_arrCtl.Formatter);

            _arrCtl.CellHeight = GetCellSize(cellHeightTextBox.Text, 40);
            _arrCtl.CellWidth = GetCellSize(cellWidthTextBox.Text, 60);

            _arrCtl.LeftBracket = _arrayLoader.LeftBracket;
            _arrCtl.RightBracket = _arrayLoader.RightBracket;

            _arrCtl.CellClick += arrCtl_CellClick;

            _arrCtl.SetControlData(_data);

            _arrCtl.Padding = new Thickness(8);
            _arrCtl.Width += 16;
            _arrCtl.Height += 16;

            _arrCtl.Tag = expressionInfo;
        }

        private static Func<object, string, string> GetCaptionBuilder(string formatter)
        {
            if (!string.IsNullOrEmpty(formatter))
                switch (formatter[0])
                {
                    case 'D':
                    case 'd':
                    case 'X':
                    case 'x':
                        return IntegralCaptionBuilder;
                }

            return DefaultCaptionBuilder;
        }

        private static int GetCellSize(string text, int defaultValue)
        {
            if (double.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out var value))
                return (int) value;

            return defaultValue;
        }

        private void SetupChartControl(ExpressionInfo expressionInfo)
        {
            IEnumerable<double> chartData = null;

            int dimenstionsCount = _dimensions.Length;

            try
            {
                if ((dimenstionsCount == 1 || dimenstionsCount == 2) && _data != null)
                {
                    chartData = ConvertToDoubles(_data);
                    chartTab.IsEnabled = chartData.Any();
                }
                else
                    chartTab.IsEnabled = false;
            }
            finally
            {
                if (!chartTab.IsEnabled)
                {
                    if (VisualizerTab.SelectedIndex == 1)
                        dataTab.IsSelected = true;
                }
                else
                {
                    if (_chartCtl == null || _chartCtl.Tag != expressionInfo)
                    {
                        if (_chartCtl != null)
                            _chartCtl.Dispose();

                        _chartCtl = new Chart();
                        ChartArea area = new ChartArea();

                        if (dimenstionsCount == 1)
                            area.Series.Add(GetSeries(GetSelectedChartType(), chartData));
                        else //2 dims
                        {
                            double[] chartDataFlat = chartData.ToArray();
                            for (int i = 0; i < _dimensions[0]; i++)
                                area.Series.Add(GetSeries(GetSelectedChartType(),
                                    chartDataFlat.Skip(i * _dimensions[1]).Take(_dimensions[1])));
                        }
                        _chartCtl.Areas.Add(area);

                        SetChartLineStroke();
                        SetChartStackModeOptions();
                        SetGridLines();
                        _chartCtl.Tag = expressionInfo;
                    }
                }
            }
        }

        private ChartTypes GetSelectedChartType()
        {
            switch ((string) ((ComboBoxItem) chartType.SelectedItem).Content)
            {
                case "Line":
                    return ChartTypes.Line;
                case "Bar":
                    return ChartTypes.Bar;
                case "Stack Bar":
                    return ChartTypes.StackingBar;
                case "Stack Bar 100":
                    return ChartTypes.StackingBar100;
                case "Column":
                    return ChartTypes.Column;
                case "Stack Column":
                    return ChartTypes.StackingColumn;
                case "Stack Column 100":
                    return ChartTypes.StackingColumn100;
                case "Area":
                    return ChartTypes.Area;
                case "Stack Area":
                    return ChartTypes.StackingArea;
                case "Stack Area 100":
                    return ChartTypes.StackingArea100;
                case "Spline":
                    return ChartTypes.Spline;
                case "Spline Area":
                    return ChartTypes.SplineArea;
                default:
                    throw new NotImplementedException();
            }
        }

        private static RangeCalculationMode GetSelectedChartCalculationMode(ChartTypes selectedChartType)
        {
            switch (selectedChartType)
            {
                case ChartTypes.Line:
                case ChartTypes.Area:
                case ChartTypes.Spline:
                case ChartTypes.SplineArea:
                case ChartTypes.StackingArea:
                case ChartTypes.StackingArea100:
                    return RangeCalculationMode.ConsistentAcrossChartTypes;
                case ChartTypes.Bar:
                case ChartTypes.Column:
                case ChartTypes.StackingBar:
                case ChartTypes.StackingBar100:
                case ChartTypes.StackingColumn:
                case ChartTypes.StackingColumn100:
                    return RangeCalculationMode.AdjustAcrossChartTypes;
                default:
                    throw new NotImplementedException();
            }
        }

        private static IEnumerable<double> ConvertToDoubles(IEnumerable array)
        {
            foreach (object item in array)
            {
                if (double.TryParse(item.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out var value))
                    yield return value;
                else
                    yield break;
            }
        }

        private static ChartSeries GetSeries(ChartTypes seriesChartType, IEnumerable<double> chartData)
        {
            ChartSeries chartSeries = new ChartSeries(seriesChartType);
            chartSeries.Data = new ChartData.VisualizerPointsCollection(chartData);
            return chartSeries;
        }

        private void SetRotationOptions(int dimensions)
        {
            angelComboBox.Items.Clear();
            angelComboBox.Items.Add(90);
            angelComboBox.Items.Add(180);
            angelComboBox.Items.Add(270);

            axisComboBox.Items.Clear();
            axisComboBox.Items.Add("X");
            axisComboBox.Items.Add("Y");
            axisComboBox.Items.Add("Z");

            switch (dimensions)
            {
                case 1:
                    axisComboBox.Visibility = Visibility.Hidden;
                    angelComboBox.Visibility = Visibility.Hidden;
                    break;
                case 2:
                    axisComboBox.Visibility = Visibility.Hidden;
                    angelComboBox.Visibility = Visibility.Visible;
                    break;
                case 3:
                    axisComboBox.Visibility = Visibility.Visible;
                    angelComboBox.Visibility = Visibility.Visible;
                    break;
                case 4:
                    angelComboBox.Items.Add(360);
                    angelComboBox.Items.Add(450);
                    axisComboBox.Items.Add("A");
                    axisComboBox.Visibility = Visibility.Visible;
                    angelComboBox.Visibility = Visibility.Visible;
                    break;
                default:
                    return;
            }

            angelComboBox.SelectedItem = 90;
            axisComboBox.SelectedItem = "Z";

            rotateGrid.IsEnabled = dimensions != 1;
        }

        private void ShowArrays()
        {
            if (_dte.Mode == vsIDEMode.vsIDEModeDebug && _dte.Debugger.CurrentStackFrame != null)
            {
                if (_arraysPending && _toolActive)
                {
                    _arraysPending = false;

                    string language = _dte.Debugger.CurrentStackFrame.Language;
                    switch (language)
                    {
                        case "C#":
                            _arrayLoader = new CsArrayLoader();
                            break;
                        case "F#":
                            _arrayLoader = new FsArrayLoader();
                            break;
                        case "Basic":
                            _arrayLoader = new VbArrayLoader();
                            break;
                        //case "C++":
                        //  arrayLoader = new CppArrayLoader();
                        //  break;
                        default:
                            _arrayLoader = GetLanguageLoader();
                            if (_arrayLoader != null)
                                break;

                            ClearVisualizer();
                            Label msg = new Label
                            {
                                Content = $"Sorry, currently {language} is not supported."
                            };
                            mainPanel.Children.Add(msg);
                            return;
                    }
                    _parsers = new ParsersCollection(_arrayLoader, _loadedParsers);
                    LoadScopeArrays();
                }
            }
        }

        private static IArrayLoader GetLanguageLoader()
        {
            return null; // Todo, try to load from dlls in bin folder
        }

        private static string DefaultCaptionBuilder(object captionData, string formatter)
        {
            Expression exp = captionData as Expression;
            string text;
            if (exp == null)
                text = (captionData ?? "").ToString();
            else
                text = (exp.Value ?? "");

            if (double.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out var number))
                text = number.ToString(formatter, CultureInfo.CurrentCulture);
            return text;
        }

        private static string IntegralCaptionBuilder(object captionData, string formatter)
        {
            Expression exp = captionData as Expression;
            string text;
            if (exp == null)
                text = (captionData ?? "").ToString();
            else
                text = (exp.Value ?? "");

            if (long.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out var number))
                text = number.ToString(formatter, CultureInfo.CurrentCulture);
            return text;
        }

        private void SetupControls(ExpressionInfo expressionInfo)
        {
            if (_data == null || arraysListBox.SelectedIndex == -1)
                return;

            if (VisualizerTab.SelectedIndex == 0)
                _displayMode = DisplayMode.Array;
            else if (VisualizerTab.SelectedIndex == 1)
                _displayMode = DisplayMode.Chart;

            switch (_displayMode)
            {
                case DisplayMode.Array:
                    SetupArrayControl(expressionInfo);
                    SetupChartControl(expressionInfo);
                    ShowElement(_arrCtl);
                    break;
                case DisplayMode.Chart:
                    SetupChartControl(expressionInfo);
                    if (chartTab.IsEnabled)
                        ShowElement(_chartCtl);
                    else
                    {
                        _displayMode = DisplayMode.Array;
                        SetupControls(expressionInfo);
                    }
                    break;
            }
        }

        private void ShowElement(Control control)
        {
            if (mainPanel != null)
                mainPanel.Children.Clear();
            if (mainPanel != null && (control != null && !mainPanel.Children.Contains(control)))
                mainPanel.Children.Add(control);
        }

        private void SetChartType()
        {
            if (_chartCtl != null && _chartCtl.Areas.Any())
            {
                ChartTypes selectedChartType = GetSelectedChartType();
                RangeCalculationMode calculationMode = GetSelectedChartCalculationMode(selectedChartType);

                foreach (ChartArea area in _chartCtl.Areas)
                {
                    area.PrimaryAxis.RangeCalculationMode = calculationMode;
                    foreach (var item in area.Series)
                        item.Type = selectedChartType;
                }
            }
        }

        private void SetGridLines()
        {
            bool seconday = gridLinesType.SelectedIndex == 1 || gridLinesType.SelectedIndex == 3;
            bool primary = gridLinesType.SelectedIndex == 2 || gridLinesType.SelectedIndex == 3;
            if (_chartCtl != null && _chartCtl.Areas.Any())
                foreach (ChartArea area in _chartCtl.Areas)
                {
                    ChartArea.SetShowGridLines(area.PrimaryAxis, primary);
                    ChartArea.SetShowGridLines(area.SecondaryAxis, seconday);
                }
        }

        private void SetChartLineStroke()
        {
            float thickness = 1;
            switch (lineThickness.SelectedIndex)
            {
                case 0:
                    thickness = 1;
                    break;
                case 1:
                    thickness = 1.5f;
                    break;
                case 2:
                    thickness = 2f;
                    break;
                case 3:
                    thickness = 3f;
                    break;
            }
            if (_chartCtl != null && _chartCtl.Areas.Any())
            {
                foreach (ChartArea area in _chartCtl.Areas)
                foreach (var item in area.Series)
                    item.StrokeThickness = thickness;
            }
        }

        private void Reset(Array defaultData)
        {
            if (_arrCtl != null)
            {
                _arrCtl.Formatter = formatterTextBox.Text;
                _arrCtl.CaptionBuilder = GetCaptionBuilder(_arrCtl.Formatter);
                _arrCtl.CellHeight = double.Parse(cellHeightTextBox.Text, CultureInfo.CurrentCulture);
                _arrCtl.CellWidth = double.Parse(cellWidthTextBox.Text, CultureInfo.CurrentCulture);
                _arrCtl.SetControlData(defaultData);
                _arrCtl.Padding = new Thickness(8);
                _arrCtl.Width += 16;
                _arrCtl.Height += 16;
            }
        }

        private void SetChartStackModeOptions()
        {
            if (_dimensions == null)
                return;

            bool chart3D = _dimensions.Length == 2;
            ((ComboBoxItem) chartType.Items[2]).IsEnabled = chart3D;
            ((ComboBoxItem) chartType.Items[3]).IsEnabled = chart3D;
            ((ComboBoxItem) chartType.Items[5]).IsEnabled = chart3D;
            ((ComboBoxItem) chartType.Items[6]).IsEnabled = chart3D;
            ((ComboBoxItem) chartType.Items[8]).IsEnabled = chart3D;
            ((ComboBoxItem) chartType.Items[9]).IsEnabled = chart3D;

            if (!((ComboBoxItem) chartType.SelectedItem).IsEnabled)
                chartType.SelectedItem = chartType.Items[0];
        }

        private void SaveSettings()
        {
            Properties.Settings ds = Properties.Settings.Default;

            ds.CellWidth = cellWidthTextBox.Text;
            ds.CellHeight = cellHeightTextBox.Text;
            ds.CellFormatter = formatterTextBox.Text;

            ds.LoadSharpDX = SharpDXParserCheckBox.IsChecked.GetValueOrDefault();
            ds.LoadFSharpMatrix = FSharpParserCheckBox.IsChecked.GetValueOrDefault();

            ds.SplitterPosition = MainGrid.ColumnDefinitions[0].Width.Value;

            ds.Save();
        }

        private void LoadSettings()
        {
            Properties.Settings ds = Properties.Settings.Default;
            cellWidthTextBox.Text = ds.CellWidth;
            cellHeightTextBox.Text = ds.CellHeight;
            formatterTextBox.Text = ds.CellFormatter;

            SharpDXParserCheckBox.IsChecked = ds.LoadSharpDX;
            FSharpParserCheckBox.IsChecked = ds.LoadFSharpMatrix;

            MainGrid.ColumnDefinitions[0].Width = new GridLength(ds.SplitterPosition, GridUnitType.Pixel);
        }

        #endregion

        private enum DisplayMode
        {
            Array,
            Chart
        }

        private enum LoadResults
        {
            Success,
            LargeArray,
            NotSupported,
            Exception
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ArrayVisualizerToolControl()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _chartCtl != null)
            {
                _chartCtl.Dispose();
                _chartCtl = null;
            }
        }

        #endregion
    }
}