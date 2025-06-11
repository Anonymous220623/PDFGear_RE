// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.SizeProperties
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart;

#nullable disable
namespace Syncfusion.Drawing;

internal class SizeProperties
{
  internal static float FULL_COLUMN_OFFSET = 1024f;
  internal static float FULL_ROW_OFFSET = 256f;
  private int m_left;
  private int m_top;
  private int m_bottom;
  private int m_right;
  private int m_leftColumn;
  private int m_rightColumn;
  private int m_topRow;
  private int m_bottomRow;
  private PlacementType m_placementType;

  internal SizeProperties() => this.m_placementType = PlacementType.MoveAndSize;

  internal void SetBottomRow(int bottomRow) => this.m_bottomRow = bottomRow;

  internal int GetRightColumn() => this.m_rightColumn;

  internal void SetRightColumn(int rightColumn) => this.m_rightColumn = rightColumn;

  internal PlacementType GetPlacementType() => this.m_placementType;

  internal void SetPlacementType(PlacementType placementType)
  {
    if (this.m_placementType == placementType)
      return;
    this.m_placementType = placementType;
  }

  internal int GetTopRow() => this.m_topRow;

  internal void SetTopRow(int topRow) => this.m_topRow = topRow;

  internal int GetLeftColumn() => this.m_leftColumn;

  internal void SetLeftColumn(int leftColumn) => this.m_leftColumn = leftColumn;

  internal int GetBottomRow() => this.m_bottomRow;

  internal int Bottom
  {
    get => this.m_bottom;
    set => this.m_bottom = value;
  }

  internal int Left
  {
    get => this.m_left;
    set => this.m_left = value;
  }

  internal int Right
  {
    get => this.m_right;
    set => this.m_right = value;
  }

  internal int Top
  {
    get => this.m_top;
    set => this.m_top = value;
  }
}
