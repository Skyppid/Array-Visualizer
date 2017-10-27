using System;
using AvProp = ArrayVisualizerControls.Properties;

namespace ArrayVisualizerControls
{
  public partial class Array3D
  {
    #region Methods

    protected override void DrawContent()
    {
      if (Data.Rank != 3)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot3DException);

      double zCellHeight = CellSize.Height * SIZE_FACTOR_3_D;
      double zCellWidth = CellSize.Width * SIZE_FACTOR_3_D;

      double zSectionHeight = zCellHeight * DimZ;
      double xySectionWidth = CellSize.Width * DimX;

      SetTransformers();

      // Main grid (front)
      string toolTipFmt = string.Format("{0}{{0}},{{1}},{{2}}{1}", LeftBracket, RightBracket);
      for (int y = 0; y < DimY; y++)
        for (int x = 0; x < DimX; x++)
        {
          object data = Data.GetValue(0, y, x);
          double labelX = x * CellSize.Width;
          double labelY = y * CellSize.Height + zSectionHeight;

          string toolTipCoordinates = string.Format(toolTipFmt, 0, y, x);

          AddLabel(ArrayRenderSection.Front, toolTipCoordinates, labelX, labelY, data);
        }

      // Top section
      for (int z = 0; z < DimZ; z++)
        for (int x = 0; x < DimX; x++)
        {
          object data = Data.GetValue(z, 0, x);
          double labelX = (z + 1) * zCellWidth + x * CellSize.Width;
          double labelY = zSectionHeight - (z + 1) * zCellHeight;

          string toolTipCoordinates = string.Format(toolTipFmt, z, 0, x);

          AddLabel(ArrayRenderSection.Top, toolTipCoordinates, labelX, labelY, data);
        }

      // Right section
      for (int z = 0; z < DimZ; z++)
        for (int y = 0; y < DimY; y++)
        {
          int x = DimX - 1;
          object data = Data.GetValue(z, y, x);
          double labelX = xySectionWidth + z * zCellWidth;
          double labelY = zSectionHeight + y * CellSize.Height - zCellHeight * z;

          string toolTipCoordinates = string.Format(toolTipFmt, z, y, x);

          AddLabel(ArrayRenderSection.Side, toolTipCoordinates, labelX, labelY, data);
        }
    }

    //Highlight color: #FFFDBF3A
    protected override void RenderBlankGrid()
    {
      if (Data.Rank != 3)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot3DException);


      double zCellHeight = CellSize.Height * SIZE_FACTOR_3_D;
      double zCellWidth = CellSize.Width * SIZE_FACTOR_3_D;

      double zSectionHeight = zCellHeight * DimZ;
      double zSectionWidth = zCellWidth * DimZ;

      double xySectionWidth = CellSize.Width * DimX;
      double xySectionHeight = CellSize.Height * DimY;

      Width = xySectionWidth + zSectionWidth + 1;
      Height = xySectionHeight + zSectionHeight + 1;

      for (double y = zSectionHeight; y <= Height; y = y + CellSize.Height)
        AddLine(0, y, xySectionWidth, y);

      for (double x = 0; x <= xySectionWidth; x = x + CellSize.Width)
        AddLine(x, zSectionHeight, x, Height);

      // Top section
      for (double x = 0; x <= xySectionWidth; x = x + CellSize.Width)
        AddLine(x, zSectionHeight, x + zSectionWidth, 0);

      double tempX = 0;
      for (double y = zSectionHeight - zCellHeight; y >= 0; y = y - zCellHeight)
      {
        tempX += zCellWidth;
        AddLine(tempX, y, tempX + xySectionWidth, y);
      }

      // Right section
      for (double y = zSectionHeight + CellHeight; y <= Height; y = y + CellSize.Height)
        AddLine(Width - zSectionWidth, y, Width, y - zSectionHeight);

      double tempY = 0;
      for (double x = xySectionWidth + zSectionWidth; x >= xySectionWidth + zCellWidth; x = x - zCellWidth)
      {
        AddLine(x, tempY, x, tempY + xySectionHeight);
        tempY += zCellHeight;
      }
    }

    #endregion
  }
}