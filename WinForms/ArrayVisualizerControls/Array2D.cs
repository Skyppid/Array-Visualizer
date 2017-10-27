using System;
using System.Drawing;
using System.Drawing.Text;
using System.Threading;
using WinFormsArrayVisualizerControls.Properties;

namespace WinFormsArrayVisualizerControls
{
#if DEBUG
    public class Array2D : ArrayProxy
#else
  public partial class Array2D : ArrayXD
#endif
    {
        private int ySize;

        private int xSize;

        protected override void RenderBlankGrid()
        {
            Width = CellSize.Width * xSize + 1;
            Height = CellSize.Height * ySize + 1;
            Refresh();

            Image = new Bitmap(Width, Height);

            using (Graphics gr = Graphics.FromImage(Image))
            {
                Pen pen = Pens.Black;
                for (int y = 0; y <= Height; y = y + CellSize.Height)
                    gr.DrawLine(pen, 0, y, Width, y);

                for (int x = 0; x <= Width; x = x + CellSize.Width)
                    gr.DrawLine(pen, x, 0, x, Height);
            }
        }

        protected override void DrawContent()
        {
            using (Graphics gr = Graphics.FromImage(Image))
            {
                gr.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                Brush brush = Brushes.Black;
                double number;
                for (int y = 0; y < ySize; y++)

                for (int x = 0; x < xSize; x++)
                {
                    string text = (Data.GetValue(y, x) ?? string.Empty).ToString();
                    if (double.TryParse(text, out number))
                        text = number.ToString(Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);

                    SizeF textSize = gr.MeasureString(text, RenderFont);

                    if (textSize.Width + CellPadding > CellSize.Width)
                        textSize.Width = CellSize.Width - CellPadding;

                    if (textSize.Height + CellPadding > CellSize.Height)
                        textSize.Height = CellSize.Height - CellPadding;

                    float drawX = x * CellSize.Width + (CellSize.Width - textSize.Width) / 2;
                    float drawY = y * CellSize.Height + (CellSize.Height - textSize.Height) / 2;

                    var textPos = new PointF(drawX, drawY);

                    gr.DrawString(text, RenderFont, brush, new RectangleF(textPos, textSize));
                }
            }
        }

        protected override void SetAxisSize()
        {
            if (Data.Rank != 2)
                throw new ArrayTypeMismatchException(Resources.ArrayNot2DException);

            ySize = Data.GetLength(0);
            xSize = Data.GetLength(1);
        }
    }
}