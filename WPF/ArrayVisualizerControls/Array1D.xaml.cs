using System;
using AvProp = ArrayVisualizerControls.Properties;

namespace ArrayVisualizerControls
{
  public partial class Array1D
  {
    #region Methods

    protected override void DrawContent()
    {
      if (Data.Rank != 1)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot1DException);

      string toolTipFmt = string.Format("{0}{{0}}{1}", LeftBracket, RightBracket);
      for (int x = 0; x < DimX; x++)
      {
        object data = Data.GetValue(x);
        double labelX = x * CellSize.Width;

        string toolTipCoordinates = string.Format(toolTipFmt, x);

        AddLabel(ArrayRenderSection.Front, toolTipCoordinates, labelX, 1, data);
      }
    }

    protected override void RenderBlankGrid()
    {
      if (Data.Rank != 1)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot1DException);

      Width = CellSize.Width * DimX + 1;
      Height = CellSize.Height + 1;
      InvalidateVisual();

      for (double y = 0; y <= Height; y = y + CellSize.Height)
        AddLine(0, y, Width, y);

      for (double x = 0; x <= Width; x = x + CellSize.Width)
        AddLine(x, 0, x, Height);
    }

    #endregion
  }
}