using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using LinqLib.Array;
using LinqLib.Sequence;
using WinFormsArrayVisualizer.Properties;
using WinFormsArrayVisualizerControls;

namespace WinFormsArrayVisualizer
{
    public partial class MainForm : Form
    {
        #region Fields

        private ArrayXD _arrCtl;
        private int _dims;

        #endregion

        #region Constructors and Destructors

        public MainForm()
        {
            InitializeComponent();
            SetControls();
        }

        #endregion

        #region Methods

        private static IEnumerable<string> GetItems(string list)
        {
            list = list.Replace(" ", string.Empty);
            list = list.Replace('\r', ',');
            list = list.Replace('\n', ',');
            while (list.IndexOf(",,", StringComparison.CurrentCulture) != -1)
                list = list.Replace(",,", ",");

            return list.Split(new[] {','});
        }

        private Array Get2DArray(int x, int y)
        {
            if (radioButtonAutoFill.Checked)
                return Enumerator.Generate(numericUpDownStart.Value, numericUpDownInc.Value, x * y)
                    .Select(V => (double) V).ToArray(y, x);

            if (radioButtonManualFill.Checked)
                try
                {
                    IEnumerable<string> items = GetManualItems();
                    return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat))
                        .ToArray(y, x);
                }
                catch (Exception ex)
                {
                    throw new FormatException(Resources.InvalidInputFormat, ex);
                }

            // file
            try
            {
                IEnumerable<string> items = GetFileItems();
                return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat))
                    .ToArray(y, x);
            }
            catch (FormatException ex)
            {
                throw new FormatException(Resources.InvalidFileContent, ex);
            }
        }

        private Array Get3DArray(int x, int y, int z)
        {
            if (radioButtonAutoFill.Checked)
                return Enumerator.Generate(numericUpDownStart.Value, numericUpDownInc.Value, x * y * z)
                    .Select(V => (double) V).ToArray(z, y, x);
            if (radioButtonManualFill.Checked)
                try
                {
                    IEnumerable<string> items = GetManualItems();
                    return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat))
                        .ToArray(z, y, x);
                }
                catch (Exception ex)
                {
                    throw new FormatException(Resources.InvalidInputFormat, ex);
                }

            // file
            try
            {
                IEnumerable<string> items = GetFileItems();
                return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat))
                    .ToArray(z, y, x);
            }
            catch (FormatException ex)
            {
                throw new FormatException(Resources.InvalidFileContent, ex);
            }
        }

        private Array Get4DArray(int x, int y, int z, int a)
        {
            if (radioButtonAutoFill.Checked)
                return Enumerator.Generate(numericUpDownStart.Value, numericUpDownInc.Value, x * y * z * a)
                    .Select(V => (double) V).ToArray(a, z, y, x);

            if (radioButtonManualFill.Checked)
                try
                {
                    IEnumerable<string> items = GetManualItems();
                    return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat))
                        .ToArray(a, z, y, x);
                }
                catch
                {
                    throw new FormatException(Resources.InvalidInputFormat);
                }

            // file
            try
            {
                IEnumerable<string> items = GetFileItems();
                return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat))
                    .ToArray(a, z, y, x);
            }
            catch (FormatException)
            {
                throw new FormatException(Resources.InvalidFileContent);
            }
        }

        private Array GetData(int dimensions)
        {
            var x = (int) numericUpDownX.Value;
            var y = (int) numericUpDownY.Value;
            var z = (int) numericUpDownZ.Value;
            var a = (int) numericUpDownA.Value;

            switch (dimensions)
            {
                case 2:
                    return Get2DArray(x, y);
                case 3:
                    return Get3DArray(x, y, z);
                case 4:
                    return Get4DArray(x, y, z, a);
                default:
                    throw new ArrayTypeMismatchException(string.Format(
                        Thread.CurrentThread.CurrentUICulture.NumberFormat, Resources.ArrayNotValidDimsException,
                        dimensions));
            }
        }

        private IEnumerable<string> GetFileItems()
        {
            var name = (string) lblFile.Tag;
            string list = string.Join(",", File.ReadAllLines(name));
            return GetItems(list);
        }

        private IEnumerable<string> GetManualItems()
        {
            return GetItems(textBoxData.Text);
        }

        private void SaveToFile(string fileName)
        {
            double[] values = _arrCtl.Data.AsEnumerable<double>().ToArray();
            string list = string.Join(",", values);

            File.WriteAllText(fileName, list);
        }

        private void SetControls()
        {
            int temp = dimensionSelector.SelectedIndex + 2;
            if (temp != _dims)
            {
                _dims = temp;

                domainUpDownAngle.Items.Clear();
                domainUpDownAxis.Items.Clear();

                domainUpDownAngle.Items.Add("90");
                domainUpDownAngle.Items.Add("180");
                domainUpDownAngle.Items.Add("270");

                domainUpDownAxis.Items.Add("X");
                domainUpDownAxis.Items.Add("Y");
                domainUpDownAxis.Items.Add("Z");

                if (_dims >= 3)
                {
                    numericUpDownZ.Visible = true;
                    label1Z.Visible = true;

                    domainUpDownAxis.Visible = true;
                    labelAxis.Visible = true;

                    numericUpDownZ1.Visible = true;
                    label2Z.Visible = true;
                }
                else
                {
                    numericUpDownZ.Visible = false;
                    label1Z.Visible = false;

                    domainUpDownAxis.Visible = false;
                    labelAxis.Visible = false;

                    numericUpDownZ1.Visible = false;
                    label2Z.Visible = false;
                }

                if (_dims >= 4)
                {
                    domainUpDownAngle.Items.Add("360");
                    domainUpDownAngle.Items.Add("450");

                    domainUpDownAxis.Items.Add("A");

                    numericUpDownA.Visible = true;
                    label1A.Visible = true;

                    numericUpDownA1.Visible = true;
                    label2A.Visible = true;
                }
                else
                {
                    numericUpDownA.Visible = false;
                    label1A.Visible = false;

                    numericUpDownA1.Visible = false;
                    label2A.Visible = false;
                }

                rotatePanel.Enabled = false;
                resizePanel.Enabled = false;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                SaveToFile(saveFileDialog.FileName);
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                radioButtonFileFill.Checked = true;
                string name = openFileDialog.FileName;
                lblFile.Tag = name;
                lblFile.Text = Path.GetFileName(name);
                toolTip.SetToolTip(lblFile, name);
            }
        }

        private void dimensionSelector_Selected(object sender, TabControlEventArgs e)
        {
            SetControls();
        }

        private void numericUpDownInc_Enter(object sender, EventArgs e)
        {
            radioButtonAutoFill.Checked = true;
        }

        private void numericUpDownStart_Enter(object sender, EventArgs e)
        {
            radioButtonAutoFill.Checked = true;
        }

        private void renderButton_Click(object sender, EventArgs e)
        {
            try
            {
                mainPanel.Controls.Clear();

                switch (_dims)
                {
                    case 2:
                        _arrCtl = new Array2D();
                        break;
                    case 3:
                        _arrCtl = new Array3D();
                        break;
                    case 4:
                        _arrCtl = new Array4D();
                        break;
                    default:
                        throw new ArrayTypeMismatchException(string.Format(
                            Thread.CurrentThread.CurrentUICulture.NumberFormat, Resources.ArrayNotValidDimsException,
                            _dims));
                }

                mainPanel.Controls.Add(_arrCtl);

                _arrCtl.CellWidth = (int) numericUpDownCellWidth.Value;
                _arrCtl.CellHeight = (int) numericUpDownCellHeight.Value;
                _arrCtl.Formatter = "0.##";

                _arrCtl.Data = GetData(_dims);

                rotatePanel.Enabled = true;
                resizePanel.Enabled = true;
                buttonSave.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void resizeButton_Click(object sender, EventArgs e)
        {
            var x = (int) numericUpDownX1.Value;
            var y = (int) numericUpDownY1.Value;
            var z = (int) numericUpDownZ1.Value;
            var a = (int) numericUpDownA1.Value;

            switch (_dims)
            {
                case 2:
                    _arrCtl.Data = ((double[,]) _arrCtl.Data).Resize(y, x);
                    break;
                case 3:
                    _arrCtl.Data = ((double[,,]) _arrCtl.Data).Resize(z, y, x);
                    break;
                case 4:
                    _arrCtl.Data = ((double[,,,]) _arrCtl.Data).Resize(a, z, y, x);
                    break;
                default:
                    throw new ArrayTypeMismatchException();
            }
        }

        private void rotateButton_Click(object sender, EventArgs e)
        {
            var r = RotateAxis.RotateNone;
            int angle = int.Parse(domainUpDownAngle.Text, Thread.CurrentThread.CurrentUICulture.NumberFormat);

            switch (domainUpDownAxis.Text)
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

            switch (_dims)
            {
                case 2:
                    _arrCtl.Data = ((double[,]) _arrCtl.Data).Rotate(angle);
                    break;
                case 3:
                    _arrCtl.Data = ((double[,,]) _arrCtl.Data).Rotate(r, angle);
                    break;
                case 4:
                    _arrCtl.Data = ((double[,,,]) _arrCtl.Data).Rotate(r, angle);
                    break;
            }
        }

        private void textBoxData_Enter(object sender, EventArgs e)
        {
            radioButtonManualFill.Checked = true;
            panelTextInput.Width = mainPanel.Width + panelTextInput.Width;
            panelTextInput.Height = 218;
        }

        private void textBoxData_Leave(object sender, EventArgs e)
        {
            panelTextInput.Width = 150;
            panelTextInput.Height = 55;
        }

        #endregion
    }
}