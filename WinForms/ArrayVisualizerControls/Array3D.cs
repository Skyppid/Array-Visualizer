using System;
using System.Drawing;
using System.Drawing.Text;
using System.Threading;
using WinFormsArrayVisualizerControls.Properties;

namespace WinFormsArrayVisualizerControls
{
#if DEBUG
    public class Array3D : ArrayProxy
#else
  public partial class Array3D : ArrayXD
#endif
    {
        private int xSize;
        private int ySize;
        private int zSize;

        protected override void RenderBlankGrid()
        {
            int zCellHeight = CellSize.Height / 4 * 3;
            int zCellWidth = CellSize.Width / 4 * 3;

            int zSectionHeight = zCellHeight * zSize;
            int zSectionWidth = zCellWidth * zSize;

            int xySectionWidth = CellSize.Width * xSize;
            int xySectionHeight = CellSize.Height * ySize;

            Width = xySectionWidth + zSectionWidth + 1;
            Height = xySectionHeight + zSectionHeight + 1;

            Refresh();

            Image = new Bitmap(Width, Height);

            using (Graphics gr = Graphics.FromImage(Image))
            {
                Pen pen = Pens.Black;

                // Main grid (front)
                for (int y = zSectionHeight; y <= Height; y = y + CellSize.Height)
                    gr.DrawLine(pen, 0, y, xySectionWidth, y);

                for (int x = 0; x <= xySectionWidth; x = x + CellSize.Width)
                    gr.DrawLine(pen, x, zSectionHeight, x, Height);

                // Top section
                for (int x = 0; x <= xySectionWidth; x = x + CellSize.Width)
                    gr.DrawLine(pen, x, zSectionHeight, x + zSectionWidth, 0);

                int tempX = 0;
                for (int y = zSectionHeight - zCellHeight; y >= 0; y = y - zCellHeight)
                {
                    tempX += zCellWidth;
                    gr.DrawLine(pen, tempX, y, tempX + xySectionWidth, y);
                }

                // Right section
                for (int y = zSectionHeight + CellHeight; y <= Height; y = y + CellSize.Height)
                    gr.DrawLine(pen, Width - zSectionWidth, y, Width, y - zSectionHeight);

                int tempY = 0;
                for (int x = xySectionWidth + zSectionWidth; x >= xySectionWidth + zCellWidth; x = x - zCellWidth)
                {
                    gr.DrawLine(pen, x, tempY, x, tempY + xySectionHeight);
                    tempY += zCellHeight;
                }
            }
        }

        protected override void DrawContent()
        {
            int zCellHeight = CellSize.Height / 4 * 3;
            int zCellWidth = CellSize.Width / 4 * 3;

            int zSectionHeight = zCellHeight * zSize;

            int xySectionWidth = CellSize.Width * xSize;

            using (Graphics gr = Graphics.FromImage(Image))
            {
                gr.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                Brush brush = Brushes.Black;

                // Main grid (front)
                double number;
                for (int y = 0; y < ySize; y++)
                {
                    for (int x = 0; x < xSize; x++)
                    {
                        string text = (Data.GetValue(0, y, x) ?? string.Empty).ToString();

                        if (double.TryParse(text, out number))
                            text = number.ToString(Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);

                        SizeF textSize = gr.MeasureString(text, RenderFont);

                        if (textSize.Width + CellPadding > CellSize.Width)
                            textSize.Width = CellSize.Width - CellPadding;

                        if (textSize.Height + CellPadding > CellSize.Height)
                            textSize.Height = CellSize.Height - CellPadding;

                        float drawX = x * CellSize.Width + (CellSize.Width - textSize.Width) / 2;
                        float drawY = y * CellSize.Height + (CellSize.Height - textSize.Height) / 2 + zSectionHeight;

                        var textPos = new PointF(drawX, drawY);
                        gr.DrawString(text, RenderFont, brush, new RectangleF(textPos, textSize));
                    }
                }

                // Top section
                for (int z = 0; z < zSize; z++)
                {
                    for (int x = 0; x < xSize; x++)
                    {
                        string text = (Data.GetValue(z, 0, x) ?? string.Empty).ToString();
                        if (double.TryParse(text, out number))
                            text = number.ToString(Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);

                        SizeF textSize = gr.MeasureString(text, RenderFont);

                        if (textSize.Width + CellPadding > zCellWidth)
                            textSize.Width = zCellWidth - CellPadding;

                        if (textSize.Height + CellPadding > zCellHeight)
                            textSize.Height = zCellHeight - CellPadding;

                        float drawX = z * zCellWidth + zCellWidth + x * CellSize.Width;
                        float drawY = zSectionHeight - (z * zCellHeight + CellSize.Height / 2);

                        var textPos = new PointF(drawX, drawY);

                        gr.DrawString(text, RenderFont, brush, new RectangleF(textPos, textSize));
                    }
                }

                // Right section
                var point00 = new PointF(-15, 0);
                for (int z = 0; z < zSize; z++)
                {
                    for (int y = 0; y < ySize; y++)
                    {
                        string text = (Data.GetValue(z, y, xSize - 1) ?? string.Empty).ToString();
                        if (double.TryParse(text, out number))
                            text = number.ToString(Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);

                        SizeF textSize = gr.MeasureString(text, RenderFont);

                        if (textSize.Width + CellPadding > zCellWidth)
                            textSize.Width = zCellWidth - CellPadding;

                        if (textSize.Height + CellPadding > zCellHeight)
                            textSize.Height = zCellHeight - CellPadding;

                        float drawX = xySectionWidth + z * zCellWidth + zCellWidth / 2;
                        float drawY = zSectionHeight + y * CellSize.Height - zCellHeight * z;

                        var textPos = new PointF(drawX, drawY);

                        gr.ResetTransform();
                        gr.TranslateTransform(textPos.X, textPos.Y);
                        gr.RotateTransform(-30);

                        gr.DrawString(text, RenderFont, brush, new RectangleF(point00, textSize));
                    }
                }
            }
        }

        protected override void SetAxisSize()
        {
            if (Data.Rank != 3)
                throw new ArrayTypeMismatchException(Resources.ArrayNot3DException);

            zSize = Data.GetLength(0);
            ySize = Data.GetLength(1);
            xSize = Data.GetLength(2);
        }
    }
}