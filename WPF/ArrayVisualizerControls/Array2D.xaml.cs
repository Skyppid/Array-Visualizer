using System;
using AvProp = ArrayVisualizerControls.Properties;

namespace ArrayVisualizerControls
{
  public partial class Array2D
  {
    #region Methods

    protected override void DrawContent()
    {
      if (Data.Rank != 2)
      {
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot2DException);
      }

      string toolTipFmt = string.Format("{0}{{0}},{{1}}{1}", LeftBracket, RightBracket);
      for (int y = 0; y < DimY; y++)
        for (int x = 0; x < DimX; x++)
        {
          string toolTipCoordinates = string.Format(toolTipFmt, y, x);
          object data = Data.GetValue(y, x);
          double labelX = x * CellSize.Width;
          double labelY = y * CellSize.Height;

          AddLabel(ArrayRenderSection.Front, toolTipCoordinates, labelX, labelY, data);
        }
    }

    protected override void RenderBlankGrid()
    {
      if (Data.Rank != 2)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot2DException);

      Width = CellSize.Width * DimX + 1;
      Height = CellSize.Height * DimY + 1;
      InvalidateVisual();

      for (double y = 0; y <= Height; y = y + CellSize.Height)
        AddLine(0, y, Width, y);

      for (double x = 0; x <= Width; x = x + CellSize.Width)
        AddLine(x, 0, x, Height);
    }

    #endregion
  }
}