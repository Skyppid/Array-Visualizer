using System;
using System.Windows.Forms;

namespace WinFormsArrayVisualizer
{
    internal static class Program
    {
        #region Methods

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        #endregion
    }
}