using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArrayVisualizerControls;
using LinqLib.Array;
using LinqLib.Sequence;
using Microsoft.Win32;
using AvProp = ArrayVisualizer.Properties;

namespace ArrayVisualizer
{
  public partial class MainWindow
  {
    #region Fields

    private ArrayControl _arrayCtl;
    private int _dims;
    private double _fillOptionsTabControlWidth;
    private bool _jagged;

    #endregion

    #region Constructors and Destructors

    public MainWindow()
    {
      this.InitializeComponent();

      for (int i = 1; i <= 100; i++)
      {
        this.xDimComboBox.Items.Add(i);
        this.yDimComboBox.Items.Add(i);
        this.zDimComboBox.Items.Add(i);
        this.aDimComboBox.Items.Add(i);

        this.xResizeComboBox.Items.Add(i);
        this.yResizeComboBox.Items.Add(i);
        this.zResizeComboBox.Items.Add(i);
        this.aResizeComboBox.Items.Add(i);
      }

      this.xDimComboBox.SelectedItem = 3;
      this.yDimComboBox.SelectedItem = 4;
      this.zDimComboBox.SelectedItem = 5;
      this.aDimComboBox.SelectedItem = 6;

      this.xResizeComboBox.SelectedItem = 5;
      this.yResizeComboBox.SelectedItem = 5;
      this.zResizeComboBox.SelectedItem = 5;
      this.aResizeComboBox.SelectedItem = 5;

      this._fillOptionsTabControlWidth = this.fillOptionsTabControl.Width;

      this.dimenstionsTab.SelectedIndex = 3;
    }

    #endregion

    #region Methods

    private static string[] GetItems(string list)
    {
      list = list.Replace(" ", string.Empty);
      list = list.Replace('\r', ',');
      list = list.Replace('\n', ',');

      while (list.IndexOf(",,", StringComparison.CurrentCulture) != -1)
        list = list.Replace(",,", ",");

      list = list.Trim(',');

      return list.Split(new[] { ',' });
    }

    private void ArrangeFrames()
    {
      int temp = int.Parse((string)((TabItem)this.dimenstionsTab.SelectedItem).Tag);
      this._jagged = temp < 0;
      if (this._jagged)
      {
        this.fillOptionsTabControl.Visibility = Visibility.Hidden;
        this.resizeGrid.Visibility = Visibility.Hidden;
        this.rotateGrid.Visibility = temp == -1 ? Visibility.Hidden : Visibility.Visible;
        this.xDimComboBox.Visibility = Visibility.Hidden;
        this.x1Label.Visibility = Visibility.Hidden;
      }
      else
      {
        this.fillOptionsTabControl.Visibility = Visibility.Visible;
        this.resizeGrid.Visibility = Visibility.Visible;
        this.rotateGrid.Visibility = Visibility.Visible;
        this.xDimComboBox.Visibility = Visibility.Visible;
        this.x1Label.Visibility = Visibility.Visible;
      }

      if (temp != this._dims)
      {
        this._dims = temp;

        this.angelComboBox.Items.Clear();
        this.axisComboBox.Items.Clear();

        this.angelComboBox.Items.Add(90);
        this.angelComboBox.Items.Add(180);
        this.angelComboBox.Items.Add(270);

        this.axisComboBox.Items.Add("X");
        this.axisComboBox.Items.Add("Y");
        this.axisComboBox.Items.Add("Z");

        if (this._dims >= 2)
        {
          this.yDimComboBox.Visibility = Visibility.Visible;
          this.y1Label.Visibility = Visibility.Visible;

          this.yResizeComboBox.Visibility = Visibility.Visible;
          this.y2Label.Visibility = Visibility.Visible;
        }
        else
        {
          // 1
          this.yDimComboBox.Visibility = Visibility.Hidden;
          this.y1Label.Visibility = Visibility.Hidden;

          this.yResizeComboBox.Visibility = Visibility.Hidden;
          this.y2Label.Visibility = Visibility.Hidden;
        }

        if (this._dims >= 3)
        {
          this.zDimComboBox.Visibility = Visibility.Visible;
          this.z1Label.Visibility = Visibility.Visible;

          this.axisComboBox.Visibility = Visibility.Visible;
          this.axisLabel.Visibility = Visibility.Visible;

          this.zResizeComboBox.Visibility = Visibility.Visible;
          this.z2Label.Visibility = Visibility.Visible;
        }
        else
        {
          // 2 or 1
          this.zDimComboBox.Visibility = Visibility.Hidden;
          this.z1Label.Visibility = Visibility.Hidden;

          this.axisComboBox.Visibility = Visibility.Hidden;
          this.axisLabel.Visibility = Visibility.Hidden;

          this.zResizeComboBox.Visibility = Visibility.Hidden;
          this.z2Label.Visibility = Visibility.Hidden;
        }

        if (this._dims >= 4)
        {
          this.angelComboBox.Items.Add(360);
          this.angelComboBox.Items.Add(450);

          this.axisComboBox.Items.Add("A");

          this.aDimComboBox.Visibility = Visibility.Visible;
          this.a1Label.Visibility = Visibility.Visible;

          this.aResizeComboBox.Visibility = Visibility.Visible;
          this.a2Label.Visibility = Visibility.Visible;
        }
        else
        {
          // 3 2 or 1
          this.aDimComboBox.Visibility = Visibility.Hidden;
          this.a1Label.Visibility = Visibility.Hidden;

          this.aResizeComboBox.Visibility = Visibility.Hidden;
          this.a2Label.Visibility = Visibility.Hidden;
        }

        this.angelComboBox.SelectedItem = 90;
        this.axisComboBox.SelectedItem = "Z";

        this.rotateGrid.IsEnabled = false;
        this.resizeGrid.IsEnabled = false;
      }
    }

    private Array Get1DArray(int x)
    {
      if (this.autoFillTab.IsSelected)
        return Enumerator.Generate(double.Parse(this.startValueTextBox.Text, CultureInfo.CurrentCulture), double.Parse(this.stepTextBox.Text, CultureInfo.CurrentCulture), x).Select(V => V).ToArray();
      else if (this.manualTab.IsSelected)
        try
        {
          string[] items = this.GetManualItems();
          return items.Select(X => double.Parse(X, CultureInfo.CurrentCulture)).ToArray();
        }
        catch (Exception ex)
        {
          throw new FormatException(AvProp.Resources.InvalidInputFormat, ex);
        }
      else
        // file
        try
        {
          string[] items = this.GetFileItems();
          return items.Select(X => double.Parse(X, CultureInfo.CurrentCulture)).ToArray();
        }
        catch (FormatException ex)
        {
          throw new FormatException(AvProp.Resources.InvalidFileContent, ex);
        }
        catch
        {
          throw;
        }
    }

    private Array Get2DArray(int x, int y)
    {
      if (this.autoFillTab.IsSelected)
        return Enumerator.Generate(double.Parse(this.startValueTextBox.Text, CultureInfo.CurrentCulture), double.Parse(this.stepTextBox.Text, CultureInfo.CurrentCulture), x * y).Select(V => V).ToArray(y, x);
      else if (this.manualTab.IsSelected)
        try
        {
          string[] items = this.GetManualItems();
          return items.Select(X => double.Parse(X, CultureInfo.CurrentCulture)).ToArray(y, x);
        }
        catch (Exception ex)
        {
          throw new FormatException(AvProp.Resources.InvalidInputFormat, ex);
        }
      else
        // file
        try
        {
          string[] items = this.GetFileItems();
          return items.Select(X => double.Parse(X, CultureInfo.CurrentCulture)).ToArray(y, x);
        }
        catch (FormatException ex)
        {
          throw new FormatException(AvProp.Resources.InvalidFileContent, ex);
        }
        catch
        {
          throw;
        }
    }

    private Array Get3DArray(int x, int y, int z)
    {
      if (this.autoFillTab.IsSelected)
        return Enumerator.Generate(double.Parse(this.startValueTextBox.Text, CultureInfo.CurrentCulture),
                                   double.Parse(this.stepTextBox.Text, CultureInfo.CurrentCulture), x * y * z).Select(V => V).ToArray(z, y, x);
      else if (this.manualTab.IsSelected)
        try
        {
          string[] items = this.GetManualItems();
          return items.Select(X => double.Parse(X, CultureInfo.CurrentCulture)).ToArray(z, y, x);
        }
        catch (Exception ex)
        {
          throw new FormatException(AvProp.Resources.InvalidInputFormat, ex);
        }
      else
        // file
        try
        {
          string[] items = this.GetFileItems();
          return items.Select(X => double.Parse(X, CultureInfo.CurrentCulture)).ToArray(z, y, x);
        }
        catch (FormatException ex)
        {
          throw new FormatException(AvProp.Resources.InvalidFileContent, ex);
        }
        catch
        {
          throw;
        }
    }

    private Array Get4DArray(int x, int y, int z, int a)
    {
      if (this.autoFillTab.IsSelected)
        return Enumerator.Generate(double.Parse(this.startValueTextBox.Text, CultureInfo.CurrentCulture),
                                   double.Parse(this.stepTextBox.Text, CultureInfo.CurrentCulture), x * y * z * a).Select(V => V).ToArray(a, z, y, x);
      else if (this.manualTab.IsSelected)
        try
        {
          string[] items = this.GetManualItems();
          return items.Select(X => double.Parse(X, CultureInfo.CurrentCulture)).ToArray(a, z, y, x);
        }
        catch
        {
          throw new FormatException(AvProp.Resources.InvalidInputFormat);
        }
      else
        // file
        try
        {
          string[] items = this.GetFileItems();
          return items.Select(X => double.Parse(X, CultureInfo.CurrentCulture)).ToArray(a, z, y, x);
        }
        catch (FormatException)
        {
          throw new FormatException(AvProp.Resources.InvalidFileContent);
        }
        catch
        {
          throw;
        }
    }

    private string[] GetFileItems()
    {
      var name = (string)this.fileLabel.Tag;
      string list = string.Join(",", File.ReadAllLines(name));
      return GetItems(list);
    }

    private static Array GetJaggedArray1D()
    {
      var arr = new int[5][][][];
      for (int i = 0; i < 5; i++)
        arr[i] = GetJaggedArray2();

      return arr;
    }

    private static int[][][] GetJaggedArray2()
    {
      var arr = new[]
                  {
                    new[] { new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4, 5 }, new[] { 1 }, new[] { 1, 2, 3 }, new[] { 1, 2 } }
                    , new[] { new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4, 5 }, new[] { 1 }, new[] { 1, 2, 3 } }, 
                    new[] { new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4, 5 }, new[] { 1 } }, 
                    new[]
                      {
                        new[] { 1, 2, 3 }, 
                        new[]
                          {
                            1, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3
                          }, 
                        new[] { 1 }, new[] { 1, 2, 3 }, new[] { 1, 2 }
                      }
                  };

      return arr;
    }

    private static Array GetJaggedArray2D()
    {
      var arr = new int[5, 3][][][];
      for (int i = 0; i < 5; i++)
      {
        arr[i, 0] = GetJaggedArray2();
        arr[i, 1] = GetJaggedArray3();
        arr[i, 2] = GetJaggedArray2();
      }

      return arr;
    }

    private static int[][][] GetJaggedArray3()
    {
      var arr = new[]
                  {
                    new[]
                      {
                        new[] { 11, 21, 3 }, new[] { 11, 21, 31, 41, 5 }, new[] { 1 }, new[] { 11, 21, 3 }, 
                        new[] { 11, 2 }
                      }, 
                    new[] { new[] { 11, 21, 3 }, new[] { 11, 21, 31, 41, 5 }, new[] { 1 }, new[] { 11, 21, 3 } }, 
                    new[] { new[] { 11, 21, 3 }, new[] { 11, 21, 31, 41, 5 }, new[] { 1 } }, 
                    new[]
                      {
                        new[] { 11, 21, 3 }, 
                        new[]
                          {
                            11, 21, 31, 41, 51, 21, 31, 41, 51, 21, 31, 4, 5, 2, 31, 41, 51, 21, 31, 41, 51, 21, 31, 41, 
                            51, 21, 31, 41, 51, 21, 3
                          }, 
                        new[] { 1 }, new[] { 11, 21, 3 }, new[] { 11, 2 }
                      }
                  };

      return arr;
    }

    private string[] GetManualItems()
    {
      return GetItems(this.manualItemsTextBox.Text);
    }

    private void PrepairGrid(ArrayControl arrayControl)
    {
      if (this._arrayCtl != null)
        this.mainPanel.Children.Remove(this._arrayCtl);

      this._arrayCtl = arrayControl;
      this._arrayCtl.CellHeight = GetCellSize(this.cellHeightTextBox.Text, 40);
      this._arrayCtl.CellWidth = GetCellSize(this.cellWidthTextBox.Text, 60);
      this._arrayCtl.Formatter = this.formatterTextBox.Text;
      this._arrayCtl.CaptionBuilder = this.CaptionBuilder;
      this._arrayCtl.Margin = new Thickness(12, 12, 0, 0);
      this._arrayCtl.HorizontalAlignment = HorizontalAlignment.Left;
      this._arrayCtl.Width = 285;
      this._arrayCtl.Height = 251;
      this._arrayCtl.VerticalAlignment = VerticalAlignment.Top;

      this._arrayCtl.LeftBracket = '[';
      this._arrayCtl.RightBracket = ']';

      this.mainPanel.Children.Add(this._arrayCtl);
    }

    private void SaveToFile(string fileName)
    {
      double[] values = this._arrayCtl.Data.AsEnumerable<double>().ToArray();
      string list = string.Join(",", values);

      File.WriteAllText(fileName, list);
    }

    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Space)
        e.Handled = true;
    }

    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      var textBox = sender as TextBox;
      if (textBox != null)
      {
        string text = textBox.Text + e.Text;
        double temp;
        bool ok = double.TryParse(text, out temp);
        e.Handled = !ok;
      }
    }

    private void dimenstionsTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.ArrangeFrames();
    }

    private void manualItemsTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
      this._fillOptionsTabControlWidth = this.fillOptionsTabControl.Width;
      this.fillOptionsTabControl.Width = this.Width - 53;
    }

    private void manualItemsTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
      this.fillOptionsTabControl.Width = this._fillOptionsTabControlWidth;
    }

    private void openFileButton_Click(object sender, RoutedEventArgs e)
    {
      var ofd = new OpenFileDialog();
      ofd.Filter = "Text Files|*.txt|Csv Files|*.csv|All Files|*.*";
      ofd.Title = "Select Input File";

      bool? res = ofd.ShowDialog();
      if (res.HasValue && res.Value)
      {
        string name = ofd.FileName;
        this.fileLabel.Tag = name;
        this.fileLabel.Content = Path.GetFileName(name);
        this.fileLabel.ToolTip = name;
      }
    }

    private void renderButton_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        var x = (int)this.xDimComboBox.SelectedItem;
        var y = (int)this.yDimComboBox.SelectedItem;
        var z = (int)this.zDimComboBox.SelectedItem;
        var a = (int)this.aDimComboBox.SelectedItem;

        switch (this._dims)
        {
          case -1: // Jagged 
            this.PrepairGrid(new Array1D());
            this._arrayCtl.SetControlData(GetJaggedArray1D());
            break;
          case 1:
            this.PrepairGrid(new Array1D());
            this._arrayCtl.SetControlData(this.Get1DArray(x));
            break;
          case -2: // Jagged 
            this.PrepairGrid(new Array2D());
            this._arrayCtl.SetControlData(GetJaggedArray2D());
            break;
          case 2:
            this.PrepairGrid(new Array2D());
            this._arrayCtl.SetControlData(this.Get2DArray(x, y));
            break;
          case 3:
            this.PrepairGrid(new Array3D());
            this._arrayCtl.SetControlData(this.Get3DArray(x, y, z));
            break;
          case 4:
            this.PrepairGrid(new Array4D());
            this._arrayCtl.SetControlData(this.Get4DArray(x, y, z, a));
            break;
        }


        this.rotateGrid.IsEnabled = Math.Abs(this._dims) > 1;
        this.resizeGrid.IsEnabled = this._dims > 0;
        this.saveButton.IsEnabled = true;
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, AvProp.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void resizeButton_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        var x = (int)this.xResizeComboBox.SelectedItem;
        var y = (int)this.yResizeComboBox.SelectedItem;
        var z = (int)this.zResizeComboBox.SelectedItem;
        var a = (int)this.aResizeComboBox.SelectedItem;

        switch (this._dims)
        {
          case 1:
            this._arrayCtl.SetControlData(((double[])this._arrayCtl.Data).Resize(x));
            break;
          case 2:
            this._arrayCtl.SetControlData(((double[,])this._arrayCtl.Data).Resize(y, x));
            break;
          case 3:
            this._arrayCtl.SetControlData(((double[, ,])this._arrayCtl.Data).Resize(z, y, x));
            break;
          case 4:
            this._arrayCtl.SetControlData(((double[, , ,])this._arrayCtl.Data).Resize(a, z, y, x));
            break;
          default:
            throw new ArrayTypeMismatchException();
        }
      }
      catch
      {
        MessageBox.Show(this, "Unable to resize this array.", "Resize Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void rotateButton_Click(object sender, RoutedEventArgs e)
    {
      var r = RotateAxis.RotateNone;
      var angle = (int)this.angelComboBox.SelectedItem;

      switch ((string)this.axisComboBox.SelectedItem)
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

      int[] dims = this._arrayCtl.Data.GetDimensions();
      switch (Math.Abs(this._dims))
      {
        case 2:
          this._arrayCtl.SetControlData((this._arrayCtl.Data.AsEnumerable<object>().ToArray(dims[0], dims[1])).Rotate(angle));
          break;
        case 3:
          this._arrayCtl.SetControlData((this._arrayCtl.Data.AsEnumerable<object>().ToArray(dims[0], dims[1], dims[2])).Rotate(r, angle));
          break;
        case 4:
          this._arrayCtl.SetControlData((this._arrayCtl.Data.AsEnumerable<object>().ToArray(dims[0], dims[1], dims[2], dims[3])).Rotate(r, angle));
          break;
      }
    }

    private void saveButton_Click(object sender, RoutedEventArgs e)
    {
      var sfd = new SaveFileDialog();
      sfd.Filter = "Text Files|*.txt|Csv Files|*.csv|All Files|*.*";
      sfd.Title = "Select Input File";

      bool? res = sfd.ShowDialog();
      if (res.HasValue && res.Value)
        this.SaveToFile(sfd.FileName);
    }

    private string CaptionBuilder(object data, string formatter)
    {
      double number;
      string text = (data ?? "").ToString();
      if (double.TryParse(text, out number))
        text = number.ToString(formatter, System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat);
      return text;
    }

    private static int GetCellSize(string text, int defaultValue)
    {
      double value;
      if (double.TryParse(text, out value))
        return defaultValue;
      else
        return 40;
    }

    #endregion
  }
}