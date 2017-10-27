using System;
using AvProp = ArrayVisualizerControls.Properties;

namespace ArrayVisualizerControls
{
  public partial class Array4D
  {
    #region Constants

    private const int SPACE_4_D = 15;

    #endregion

    #region Methods

    protected override void DrawContent()
    {
      if (Data.Rank != 4)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot4DException);


      double zCellHeight = CellSize.Height * SIZE_FACTOR_3_D;
      double zCellWidth = CellSize.Width * SIZE_FACTOR_3_D;

      double zSectionHeight = zCellHeight * DimZ;
      double zSectionWidth = zCellWidth * DimZ;

      double xySectionWidth = CellSize.Width * DimX;

      SetTransformers();

      string toolTipFmt = string.Format("{0}{{0}},{{1}},{{2}},{{3}}{1}", LeftBracket, RightBracket);
      for (int a = 0; a < DimA; a++)
      {
        double aOffset = a * (xySectionWidth + SPACE_4_D + zSectionWidth);

        // Main grid (front)
        for (int y = 0; y < DimY; y++)
          for (int x = 0; x < DimX; x++)
          {
            object data = Data.GetValue(a, 0, y, x);
            double labelX = aOffset + x * CellSize.Width;
            double labelY = y * CellSize.Height + zSectionHeight;

            string toolTipCoordinates = string.Format(toolTipFmt, a, 0, y, x);

            AddLabel(ArrayRenderSection.Front, toolTipCoordinates, labelX, labelY, data);
          }

        // Top section                    
        for (int z = 0; z < DimZ; z++)
          for (int x = 0; x < DimX; x++)
          {
            object data = Data.GetValue(a, z, 0, x);
            double labelX = aOffset + (z + 1) * zCellWidth + x * CellSize.Width;
            double labelY = zSectionHeight - (z + 1) * zCellHeight;

            string toolTipCoordinates = string.Format(toolTipFmt, a, z, 0, x);

            AddLabel(ArrayRenderSection.Top, toolTipCoordinates, labelX, labelY, data);
          }

        // Right section
        for (int z = 0; z < DimZ; z++)
          for (int y = 0; y < DimY; y++)
          {
            int x = DimX - 1;
            object data = Data.GetValue(a, z, y, x);
            double labelX = aOffset + xySectionWidth + z * zCellWidth;
            double labelY = zSectionHeight + y * CellSize.Height - zCellHeight * z;

            string toolTipCoordinates = string.Format(toolTipFmt, a, z, y, x);

            AddLabel(ArrayRenderSection.Side, toolTipCoordinates, labelX, labelY, data);
          }
      }
    }

    protected override void RenderBlankGrid()
    {
      if (Data.Rank != 4)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot4DException);

      double zCellHeight = CellSize.Height * SIZE_FACTOR_3_D;
      double zCellWidth = CellSize.Width * SIZE_FACTOR_3_D;

      double zSectionHeight = zCellHeight * DimZ;
      double zSectionWidth = zCellWidth * DimZ;

      double xySectionWidth = CellSize.Width * DimX;
      double xySectionHeight = CellSize.Height * DimY;

      Width = xySectionWidth + zSectionWidth + 1;
      Height = xySectionHeight + zSectionHeight + 1;

      double sectionWidth = xySectionWidth + zSectionWidth + 1;
      Width = sectionWidth * DimA + SPACE_4_D * (DimA - 1);
      Height = xySectionHeight + zSectionHeight + 1;

      for (int a = 0; a < DimA; a++)
      {
        double aOffset = a * (sectionWidth + SPACE_4_D);

        // Front
        for (double y = zSectionHeight; y <= Height; y = y + CellSize.Height)
          AddLine(aOffset, y, aOffset + xySectionWidth, y);

        for (double x = 0; x <= xySectionWidth; x = x + CellSize.Width)
          AddLine(aOffset + x, zSectionHeight, aOffset + x, Height);

        // Top section
        for (double x = 0; x <= xySectionWidth; x = x + CellSize.Width)
          AddLine(aOffset + x, zSectionHeight, aOffset + x + zSectionWidth, 0);

        double tempX = 0;
        for (double y = zSectionHeight - zCellHeight; y >= 0; y = y - zCellHeight)
        {
          tempX += zCellWidth;
          AddLine(aOffset + tempX, y, aOffset + tempX + xySectionWidth, y);
        }

        // Right section
        for (double y = zSectionHeight + CellHeight; y <= Height; y = y + CellSize.Height)
          AddLine(aOffset + sectionWidth - zSectionWidth, y, aOffset + sectionWidth, y - zSectionHeight);

        double tempY = 0;
        for (double x = xySectionWidth + zSectionWidth; x >= xySectionWidth + zCellWidth; x = x - zCellWidth)
        {
          AddLine(aOffset + x, tempY, aOffset + x, tempY + xySectionHeight);
          tempY += zCellHeight;
        }
      }
    }

    #endregion
  }
}