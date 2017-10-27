using System;
using System.Windows.Forms;

namespace CsTester
{
  internal static class Program
  {
    #region Methods

    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Form1());
    }

    #endregion
  }
}