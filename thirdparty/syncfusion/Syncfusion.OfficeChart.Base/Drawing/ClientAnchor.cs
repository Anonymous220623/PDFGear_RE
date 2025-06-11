// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.ClientAnchor
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart;
using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Implementation.Collections;
using System;

#nullable disable
namespace Syncfusion.Drawing;

internal class ClientAnchor
{
  private SizeProperties m_size;
  private WorksheetImpl m_workSheet;

  internal ClientAnchor(WorksheetImpl worksheetImpl)
  {
    this.m_workSheet = worksheetImpl;
    this.m_size = new SizeProperties();
  }

  internal WorksheetImpl Worksheet
  {
    get => this.m_workSheet;
    set => this.m_workSheet = value;
  }

  internal PlacementType Placement
  {
    get => this.m_size.GetPlacementType();
    set
    {
      if (this.m_size.GetPlacementType() == PlacementType.MoveAndSize)
      {
        int topRow = this.m_size.GetTopRow();
        int top = this.m_size.Top;
        int leftColumn = this.m_size.GetLeftColumn();
        int left = this.m_size.Left;
        int bottomRow = this.m_size.GetBottomRow();
        int bottom = this.m_size.Bottom;
        int rightColumn = this.m_size.GetRightColumn();
        int right = this.m_size.Right;
        if (value == PlacementType.FreeFloating)
        {
          int width = this.CalculateWidth(0, 0, leftColumn, left);
          this.m_size.Top = this.CalculateHeight(0, 0, topRow, top);
          this.m_size.Left = width;
        }
        int width1 = this.CalculateWidth(leftColumn, left, rightColumn, right);
        int height = this.CalculateHeight(topRow, top, bottomRow, bottom);
        this.m_size.Right = width1;
        this.m_size.Bottom = height;
        this.m_size.SetPlacementType(value);
      }
      else if (this.m_size.GetPlacementType() == PlacementType.Move)
      {
        int topRow = this.m_size.GetTopRow();
        int top = this.m_size.Top;
        int leftColumn = this.m_size.GetLeftColumn();
        int left = this.m_size.Left;
        switch (value)
        {
          case PlacementType.FreeFloating:
            this.m_size.Left = this.CalculateWidth(0, 0, leftColumn, left);
            this.m_size.Top = this.CalculateHeight(0, 0, topRow, top);
            break;
          case PlacementType.MoveAndSize:
            int right = this.m_size.Right;
            int bottom = this.m_size.Bottom;
            int[] leftColumnOffset = this.GetLeftAndLeftColumnOffset(leftColumn, left, right);
            this.m_size.SetRightColumn(leftColumnOffset[0]);
            this.m_size.Right = leftColumnOffset[1];
            int[] topAndTopRowOffset = this.GetTopAndTopRowOffset(topRow, top, bottom);
            this.m_size.SetBottomRow(topAndTopRowOffset[0]);
            this.m_size.Bottom = topAndTopRowOffset[1];
            break;
        }
        this.m_size.SetPlacementType(value);
      }
      else
      {
        if (this.m_size.GetPlacementType() != PlacementType.FreeFloating)
          return;
        int left1 = this.m_size.Left;
        int[] topAndTopRowOffset1 = this.GetTopAndTopRowOffset(0, 0, this.m_size.Top);
        this.m_size.SetTopRow(topAndTopRowOffset1[0]);
        this.m_size.Top = topAndTopRowOffset1[1];
        int topRow = topAndTopRowOffset1[0];
        int top = topAndTopRowOffset1[1];
        int[] leftColumnOffset1 = this.GetLeftAndLeftColumnOffset(0, 0, left1);
        this.m_size.SetLeftColumn(leftColumnOffset1[0]);
        this.m_size.Left = leftColumnOffset1[1];
        int leftColumn = leftColumnOffset1[0];
        int left2 = leftColumnOffset1[1];
        if (value == PlacementType.MoveAndSize)
        {
          int right = this.m_size.Right;
          int bottom = this.m_size.Bottom;
          int[] topAndTopRowOffset2 = this.GetTopAndTopRowOffset(topRow, top, bottom);
          this.m_size.SetBottomRow(topAndTopRowOffset2[0]);
          this.m_size.Bottom = topAndTopRowOffset2[1];
          int[] leftColumnOffset2 = this.GetLeftAndLeftColumnOffset(leftColumn, left2, right);
          this.m_size.SetRightColumn(leftColumnOffset2[0]);
          this.m_size.Right = leftColumnOffset2[1];
        }
        this.m_size.SetPlacementType(value);
      }
    }
  }

  internal int Height
  {
    get
    {
      if (this.Placement == PlacementType.Move || this.Placement == PlacementType.FreeFloating)
        return this.m_size.Bottom;
      int topRow = this.m_size.GetTopRow();
      int bottomRow = this.m_size.GetBottomRow();
      int top = this.m_size.Top;
      int bottom = this.m_size.Bottom;
      return this.CalculateHeight(topRow, top, bottomRow, bottom);
    }
    set
    {
      if (this.Placement != PlacementType.Move && this.Placement != PlacementType.FreeFloating)
      {
        int[] topAndTopRowOffset = this.GetTopAndTopRowOffset(this.m_size.GetTopRow(), this.m_size.Top, value);
        this.m_size.SetBottomRow(topAndTopRowOffset[0]);
        this.m_size.Bottom = topAndTopRowOffset[1];
      }
      else
        this.m_size.Bottom = value;
    }
  }

  internal int Width
  {
    get
    {
      if (this.Placement == PlacementType.Move || this.Placement == PlacementType.FreeFloating)
        return this.m_size.Right;
      int leftColumn = this.m_size.GetLeftColumn();
      int rightColumn = this.m_size.GetRightColumn();
      int left = this.m_size.Left;
      int right = this.m_size.Right;
      return this.CalculateWidth(leftColumn, left, rightColumn, right);
    }
    set
    {
      if (this.Placement != PlacementType.Move && this.Placement != PlacementType.FreeFloating)
      {
        int[] leftColumnOffset = this.GetLeftAndLeftColumnOffset(this.m_size.GetLeftColumn(), this.m_size.Left, value);
        this.m_size.SetRightColumn(leftColumnOffset[0]);
        this.m_size.Right = leftColumnOffset[1];
      }
      else
        this.m_size.Right = value;
    }
  }

  internal int Top
  {
    get
    {
      return (int) ((double) (this.m_workSheet.GetInnerRowHeightInPixels(this.TopRow) * this.TopRowOffset) / (double) SizeProperties.FULL_ROW_OFFSET + 0.5);
    }
    set
    {
      int rowHeightInPixels = this.m_workSheet.GetInnerRowHeightInPixels(this.TopRow);
      if (rowHeightInPixels < value)
        throw new ArgumentException("The top value must be less than the upper left row height.");
      this.TopRowOffset = (int) ((double) value * (double) SizeProperties.FULL_ROW_OFFSET / (double) rowHeightInPixels + 0.5);
    }
  }

  internal int Left
  {
    get
    {
      return (int) ((double) (this.m_workSheet.GetViewColumnWidthPixel(this.LeftColumn) * this.LeftColumnOffset) / (double) SizeProperties.FULL_COLUMN_OFFSET + 0.5);
    }
    set
    {
      int columnWidthPixel = this.m_workSheet.GetViewColumnWidthPixel(this.LeftColumn);
      if (columnWidthPixel < value)
        throw new ArgumentException("The left value must be less than the upper left column width.");
      this.LeftColumnOffset = (int) ((double) value * (double) SizeProperties.FULL_COLUMN_OFFSET / (double) columnWidthPixel + 0.5);
    }
  }

  internal int LeftColumn
  {
    get
    {
      return this.Placement != PlacementType.Move && this.Placement != PlacementType.MoveAndSize ? this.GetLeftAndLeftColumnOffset(0, 0, this.m_size.Left)[0] : this.m_size.GetLeftColumn();
    }
    set
    {
      WorksheetImpl.CheckColumnIndex(value);
      PlacementType placement = this.Placement;
      this.Placement = PlacementType.Move;
      this.m_size.SetLeftColumn(value);
      this.Placement = placement;
    }
  }

  internal int TopRow
  {
    get
    {
      return this.Placement != PlacementType.Move && this.Placement != PlacementType.MoveAndSize ? this.GetTopAndTopRowOffset(0, 0, this.m_size.Top)[0] : this.m_size.GetTopRow();
    }
    set
    {
      WorksheetImpl.CheckRowIndex(value);
      PlacementType placement = this.Placement;
      this.Placement = PlacementType.Move;
      this.m_size.SetTopRow(value);
      this.Placement = placement;
    }
  }

  internal int TopRowOffset
  {
    get
    {
      if (this.Placement != PlacementType.Move && this.Placement != PlacementType.MoveAndSize)
        return this.GetTopAndTopRowOffset(0, 0, this.m_size.Top)[1];
      return (double) this.m_size.Top <= (double) SizeProperties.FULL_ROW_OFFSET ? this.m_size.Top : (int) SizeProperties.FULL_ROW_OFFSET;
    }
    set
    {
      if (value < 0 || (double) value > (double) SizeProperties.FULL_ROW_OFFSET)
        return;
      PlacementType placement = this.Placement;
      this.Placement = PlacementType.Move;
      this.m_size.Top = value;
      this.Placement = placement;
    }
  }

  internal int LeftColumnOffset
  {
    get
    {
      if (this.Placement != PlacementType.Move && this.Placement != PlacementType.MoveAndSize)
        return this.GetLeftAndLeftColumnOffset(0, 0, this.m_size.Left)[1];
      return (double) this.m_size.Left <= (double) SizeProperties.FULL_COLUMN_OFFSET ? this.m_size.Left : (int) SizeProperties.FULL_COLUMN_OFFSET;
    }
    set
    {
      if (value < 0 || (double) value > (double) SizeProperties.FULL_COLUMN_OFFSET)
        return;
      PlacementType placement = this.Placement;
      this.Placement = PlacementType.Move;
      this.m_size.Left = value;
      this.Placement = placement;
    }
  }

  internal int RightColumnOffset
  {
    get
    {
      if (this.Placement == PlacementType.MoveAndSize)
        return this.m_size.Right;
      int leftColumn = 0;
      int left = 0;
      int right = this.m_size.Right;
      if (this.Placement == PlacementType.Move)
      {
        leftColumn = this.m_size.GetLeftColumn();
        left = this.m_size.Left;
      }
      else
        right += this.m_size.Left;
      return this.GetLeftAndLeftColumnOffset(leftColumn, left, right)[1];
    }
    set
    {
      if (value < 0 && (double) value > (double) SizeProperties.FULL_COLUMN_OFFSET)
        return;
      int rightColumn = this.RightColumn;
      PlacementType placement = this.Placement;
      this.Placement = PlacementType.Move;
      int right = this.m_size.Right;
      int[] leftAndLeftOffset = this.GetLeftAndLeftOffset(rightColumn, value, right);
      this.m_size.SetLeftColumn(leftAndLeftOffset[0]);
      this.m_size.Left = leftAndLeftOffset[1];
      this.Placement = placement;
    }
  }

  internal int BottomRowOffset
  {
    get
    {
      if (this.Placement == PlacementType.MoveAndSize)
        return this.m_size.Bottom;
      int bottom = this.m_size.Bottom;
      int topRow = 0;
      int top = 0;
      if (this.Placement == PlacementType.Move)
      {
        topRow = this.m_size.GetTopRow();
        top = this.m_size.Top;
      }
      else
        bottom += this.m_size.Top;
      return this.GetTopAndTopRowOffset(topRow, top, bottom)[1];
    }
    set
    {
      if (value < 0 || (double) value > (double) SizeProperties.FULL_ROW_OFFSET)
        return;
      int bottomRow = this.BottomRow;
      PlacementType placement = this.Placement;
      this.Placement = PlacementType.Move;
      int bottom = this.m_size.Bottom;
      int[] topAndTopOffset = this.GetTopAndTopOffset(bottomRow, value, bottom);
      this.m_size.SetTopRow(topAndTopOffset[0]);
      this.m_size.Top = topAndTopOffset[1];
      this.Placement = placement;
    }
  }

  internal int RightColumn
  {
    get
    {
      if (this.Placement == PlacementType.MoveAndSize)
        return this.m_size.GetRightColumn();
      int leftColumn = 0;
      int left = 0;
      int right = this.m_size.Right;
      if (this.Placement == PlacementType.Move)
      {
        leftColumn = this.m_size.GetLeftColumn();
        left = this.m_size.Left;
      }
      else
        right += this.m_size.Left;
      return this.GetLeftAndLeftColumnOffset(leftColumn, left, right)[0];
    }
    set
    {
      WorksheetImpl.CheckColumnIndex(value);
      int rightColumnOffset = this.RightColumnOffset;
      PlacementType placement = this.Placement;
      this.Placement = PlacementType.Move;
      int right = this.m_size.Right;
      int[] leftAndLeftOffset = this.GetLeftAndLeftOffset(value, rightColumnOffset, right);
      this.m_size.SetLeftColumn(leftAndLeftOffset[0]);
      this.m_size.Left = leftAndLeftOffset[1];
      this.Placement = placement;
    }
  }

  internal int BottomRow
  {
    get
    {
      if (this.Placement == PlacementType.MoveAndSize)
        return this.m_size.GetBottomRow();
      int bottom = this.m_size.Bottom;
      int topRow = 0;
      int top = 0;
      if (this.Placement == PlacementType.Move)
      {
        topRow = this.m_size.GetTopRow();
        top = this.m_size.Top;
      }
      else
        bottom += this.m_size.Top;
      return this.GetTopAndTopRowOffset(topRow, top, bottom)[0];
    }
    set
    {
      WorksheetImpl.CheckRowIndex(value);
      int bottomRowOffset = this.BottomRowOffset;
      PlacementType placement = this.Placement;
      this.Placement = PlacementType.Move;
      int bottom = this.m_size.Bottom;
      int[] topAndTopOffset = this.GetTopAndTopOffset(value, bottomRowOffset, bottom);
      this.m_size.SetTopRow(topAndTopOffset[0]);
      this.m_size.Top = topAndTopOffset[1];
      this.Placement = placement;
    }
  }

  internal int[] GetTopAndTopRowOffset(int topRow, int top, int bottom)
  {
    int[] topAndTopRowOffset = new int[2];
    if (bottom == 0)
    {
      topAndTopRowOffset[0] = topRow;
      topAndTopRowOffset[1] = top;
      return topAndTopRowOffset;
    }
    int num1 = 0;
    WorksheetImpl workSheet = this.m_workSheet;
    RecordTable table = workSheet.CellRecords.Table;
    int count = table.Rows.GetCount();
    if (top != 0)
    {
      num1 = workSheet.GetInnerRowHeightInPixels(topRow);
      int num2 = (int) ((double) num1 - (double) (num1 * top) / (double) SizeProperties.FULL_ROW_OFFSET + 0.5);
      if (bottom <= num2)
      {
        topAndTopRowOffset[0] = topRow;
        topAndTopRowOffset[1] = (int) ((double) bottom * (double) SizeProperties.FULL_ROW_OFFSET / (double) num1 + (double) top + 0.5);
        return topAndTopRowOffset;
      }
      ++topRow;
      bottom -= num2;
    }
    int num3 = (int) (workSheet.StandardHeight * (double) workSheet.GetAppImpl().GetdpiY() / 72.0 + 0.5);
    int arrIndex = 0;
    if (count == 0)
    {
      int num4 = (int) Math.Ceiling((double) bottom / (double) num3);
      topRow += num4 - 1;
      bottom -= num3 * num4;
      num1 = num3;
    }
    else
    {
      table.Rows.GetRowIndex(topRow, out arrIndex);
      if (arrIndex >= count)
      {
        int num5 = (int) Math.Ceiling((double) bottom / (double) num3);
        topRow += num5 - 1;
        bottom -= num3 * num5;
        num1 = num3;
      }
      else
      {
        RowStorage row1 = table.Rows[arrIndex];
        for (; topRow <= 1048575 /*0x0FFFFF*/; ++topRow)
        {
          if (arrIndex == topRow)
          {
            num1 = (int) (workSheet.GetInnerRowHeight(arrIndex) * (double) workSheet.GetAppImpl().GetdpiY() / 72.0 + 0.5);
            bottom -= num1;
            if (bottom > 0)
            {
              ++arrIndex;
              if (arrIndex >= count)
              {
                int num6 = (int) Math.Ceiling((double) bottom / (double) num3);
                topRow += num6;
                bottom -= num3 * num6;
                num1 = num3;
                break;
              }
              RowStorage row2 = table.Rows[arrIndex];
            }
            else
              break;
          }
          else
          {
            num1 = num3;
            bottom -= num1;
            if (bottom <= 0)
              break;
          }
        }
      }
    }
    if (bottom <= 0 && (bottom != 0 || topRow != 1048575 /*0x0FFFFF*/))
    {
      if (bottom == 0)
      {
        topAndTopRowOffset[0] = topRow + 1;
        return topAndTopRowOffset;
      }
      topAndTopRowOffset[0] = topRow;
      topAndTopRowOffset[1] = (int) ((double) (bottom + num1) * (double) SizeProperties.FULL_ROW_OFFSET / (double) num1 + 0.5);
      return topAndTopRowOffset;
    }
    topAndTopRowOffset[0] = 1048575 /*0x0FFFFF*/;
    topAndTopRowOffset[1] = (int) SizeProperties.FULL_ROW_OFFSET;
    return topAndTopRowOffset;
  }

  internal int CalculateHeight(int topRow, int top, int bottomRow, int bottom)
  {
    int num1 = 0;
    WorksheetImpl workSheet = this.m_workSheet;
    if ((double) top >= (double) SizeProperties.FULL_ROW_OFFSET)
      top = (int) SizeProperties.FULL_ROW_OFFSET;
    if ((double) bottom >= (double) SizeProperties.FULL_ROW_OFFSET)
      bottom = (int) SizeProperties.FULL_ROW_OFFSET;
    if (bottomRow == topRow)
    {
      int rowHeightInPixels = workSheet.GetInnerRowHeightInPixels(topRow);
      return (int) ((double) ((bottom - top) * rowHeightInPixels) / (double) SizeProperties.FULL_ROW_OFFSET + 0.5);
    }
    if (bottomRow < topRow)
      return 0;
    int rowHeightInPixels1 = workSheet.GetInnerRowHeightInPixels(topRow);
    int num2 = num1 + (rowHeightInPixels1 - (int) ((double) (top * rowHeightInPixels1) / (double) SizeProperties.FULL_ROW_OFFSET + 0.5));
    int num3 = topRow;
    ++topRow;
    int num4 = 0;
    int num5 = 0;
    RecordTable table = workSheet.CellRecords.Table;
    for (int count = table.Rows.GetCount(); num4 < count; ++num4)
    {
      if (table.Rows[num4] != null && num4 >= topRow)
      {
        if (num4 < bottomRow)
        {
          ++num5;
          num2 += (int) (workSheet.GetInnerRowHeight(num4) * (double) workSheet.GetAppImpl().GetdpiY() / 72.0 + 0.5);
        }
        else
          break;
      }
    }
    int num6 = bottomRow - num3 - 1 - num5;
    if (num6 > 0)
      num2 += num6 * (int) (workSheet.StandardHeight * (double) workSheet.GetAppImpl().GetdpiY() / 72.0 + 0.5);
    int rowHeightInPixels2 = workSheet.GetInnerRowHeightInPixels(bottomRow);
    return num2 + (int) ((double) (bottom * rowHeightInPixels2) / (double) SizeProperties.FULL_ROW_OFFSET + 0.5);
  }

  internal int CalculateRowOffset(int minRow, int minOffsetValue, int maxRow, int maxOffsetValue)
  {
    int num1 = 0;
    WorksheetImpl workSheet = this.m_workSheet;
    if (maxRow == minRow)
    {
      int rowHeightInPixels = workSheet.GetInnerRowHeightInPixels(minRow);
      return (int) ((double) ((maxOffsetValue - minOffsetValue) * rowHeightInPixels) / (double) SizeProperties.FULL_ROW_OFFSET + 0.5);
    }
    int rowHeightInPixels1 = workSheet.GetInnerRowHeightInPixels(minRow);
    int num2 = num1 + (rowHeightInPixels1 - (int) ((double) (minOffsetValue * rowHeightInPixels1) / (double) SizeProperties.FULL_ROW_OFFSET + 0.5));
    int num3 = minRow;
    ++minRow;
    int index = 0;
    int num4 = 0;
    RecordTable table = workSheet.CellRecords.Table;
    for (int count = table.Rows.GetCount(); index < count; ++index)
    {
      if (table.Rows[index] != null && index >= minRow)
      {
        if (index < maxRow)
        {
          ++num4;
          num2 += (int) (workSheet.StandardHeight * (double) workSheet.GetAppImpl().GetdpiY() / 1440.0);
        }
        else
          break;
      }
    }
    int num5 = maxRow - num3 - 1 - num4;
    if (num5 > 0)
      num2 += num5 * (int) (workSheet.StandardHeight * (double) workSheet.GetAppImpl().GetdpiY() / 72.0 + 0.5);
    int rowHeightInPixels2 = workSheet.GetInnerRowHeightInPixels(maxRow);
    return num2 + (int) ((double) (maxOffsetValue * rowHeightInPixels2) / (double) SizeProperties.FULL_ROW_OFFSET + 0.5);
  }

  internal int CalculateColumnOffset(
    int minColumn,
    int minOffsetValue,
    int maxColumn,
    int maxOffsetValue)
  {
    int num1 = 0;
    WorksheetImpl workSheet = this.m_workSheet;
    if (maxColumn == minColumn)
    {
      int columnWidthInPixels = workSheet.GetColumnWidthInPixels(minColumn + 1);
      return (int) ((double) ((maxOffsetValue - minOffsetValue) * columnWidthInPixels) / (double) SizeProperties.FULL_COLUMN_OFFSET + 0.5);
    }
    int columnWidthInPixels1 = workSheet.GetColumnWidthInPixels(minColumn + 1);
    int num2 = num1 + (columnWidthInPixels1 - (int) ((double) (minOffsetValue * columnWidthInPixels1) / (double) SizeProperties.FULL_COLUMN_OFFSET + 0.5));
    int num3 = minColumn;
    ++minColumn;
    int num4 = 0;
    int arrIndex;
    for (workSheet.Columnss.GetColumnIndex(minColumn, out arrIndex); arrIndex < workSheet.Columnss.Count; ++arrIndex)
    {
      Column columnByIndex = workSheet.Columnss.GetColumnByIndex(arrIndex);
      if (columnByIndex.Index >= minColumn)
      {
        if (columnByIndex.Index < maxColumn)
        {
          ++num4;
          if (!columnByIndex.IsHidden)
            num2 += WorksheetImpl.CharacterWidth(columnByIndex.Width, workSheet.GetAppImpl());
        }
        else
          break;
      }
    }
    int num5 = num2 + workSheet.Columnss.GetWidth(num3 + num4 + 1, maxColumn - 1, true, true);
    int columnWidthInPixels2 = workSheet.GetColumnWidthInPixels(maxColumn + 1);
    return num5 + (int) ((double) (maxOffsetValue * columnWidthInPixels2) / (double) SizeProperties.FULL_COLUMN_OFFSET + 0.5);
  }

  internal int[] GetLeftAndLeftColumnOffset(int leftColumn, int left, int right)
  {
    int[] leftColumnOffset = new int[2];
    WorksheetImpl workSheet = this.m_workSheet;
    if (left != 0)
    {
      int columnWidthPixel = workSheet.GetViewColumnWidthPixel(leftColumn);
      int num = (int) ((double) columnWidthPixel - (double) (columnWidthPixel * left) / (double) SizeProperties.FULL_COLUMN_OFFSET + 0.5);
      if (right <= num)
      {
        leftColumnOffset[0] = leftColumn;
        leftColumnOffset[1] = (int) ((double) right * (double) SizeProperties.FULL_COLUMN_OFFSET / (double) columnWidthPixel + (double) left + 0.5);
        return leftColumnOffset;
      }
      ++leftColumn;
      right -= num;
    }
    int arrIndex = 0;
    workSheet.Columnss.GetColumnIndex(leftColumn, out arrIndex);
    do
    {
      double width;
      if (arrIndex >= workSheet.Columnss.Count)
      {
        width = workSheet.Columnss.GetWidth(leftColumn, false);
      }
      else
      {
        Column columnByIndex = workSheet.Columnss.GetColumnByIndex(arrIndex);
        if (columnByIndex.Index == leftColumn)
        {
          ++arrIndex;
          width = columnByIndex.IsHidden ? 0.0 : columnByIndex.defaultWidth;
        }
        else
          width = workSheet.Columnss.GetWidth(leftColumn, false);
      }
      int num = WorksheetImpl.CharacterWidth(width, workSheet.GetAppImpl());
      right -= num;
      if (right <= 0)
      {
        if (right <= 0 && (right != 0 || leftColumn != 16383 /*0x3FFF*/))
        {
          if (right == 0)
          {
            leftColumnOffset[0] = leftColumn + 1;
            return leftColumnOffset;
          }
          leftColumnOffset[0] = leftColumn;
          leftColumnOffset[1] = (int) ((double) (right + num) * (double) SizeProperties.FULL_COLUMN_OFFSET / (double) num + 0.5);
          return leftColumnOffset;
        }
        leftColumnOffset[0] = 16383 /*0x3FFF*/;
        leftColumnOffset[1] = (int) SizeProperties.FULL_COLUMN_OFFSET;
        return leftColumnOffset;
      }
      ++leftColumn;
    }
    while (leftColumn <= 16383 /*0x3FFF*/);
    leftColumnOffset[0] = 16383 /*0x3FFF*/;
    leftColumnOffset[1] = (int) SizeProperties.FULL_COLUMN_OFFSET;
    return leftColumnOffset;
  }

  internal int CalculateWidth(int leftColumn, int left, int rightColumn, int right)
  {
    int num1 = 0;
    WorksheetImpl workSheet = this.m_workSheet;
    if (rightColumn == leftColumn)
    {
      int columnWidthPixel = workSheet.GetViewColumnWidthPixel(leftColumn);
      return (int) ((double) ((right - left) * columnWidthPixel) / (double) SizeProperties.FULL_COLUMN_OFFSET + 0.5);
    }
    if (rightColumn < leftColumn)
      return 0;
    int columnWidthPixel1 = workSheet.GetViewColumnWidthPixel(leftColumn);
    int num2 = num1 + (columnWidthPixel1 - (int) ((double) (left * columnWidthPixel1) / (double) SizeProperties.FULL_COLUMN_OFFSET + 0.5));
    int num3 = leftColumn;
    ++leftColumn;
    int num4 = 0;
    int arrIndex;
    for (workSheet.Columnss.GetColumnIndex(leftColumn, out arrIndex); arrIndex < workSheet.Columnss.Count; ++arrIndex)
    {
      Column columnByIndex = workSheet.Columnss.GetColumnByIndex(arrIndex);
      if (columnByIndex.Index >= leftColumn)
      {
        if (columnByIndex.Index < rightColumn)
        {
          ++num4;
          if (!columnByIndex.IsHidden)
            num2 += WorksheetImpl.CharacterWidth(columnByIndex.Width, workSheet.GetAppImpl());
        }
        else
          break;
      }
    }
    int num5 = num2 + workSheet.Columnss.GetWidth(num3 + num4 + 1, rightColumn - 1, false, true);
    int columnWidthPixel2 = workSheet.GetViewColumnWidthPixel(rightColumn);
    return num5 + (int) ((double) (right * columnWidthPixel2) / (double) SizeProperties.FULL_COLUMN_OFFSET + 0.5);
  }

  internal int[] GetTopAndTopOffset(int bottomRow, int bottomRowOffset, int bottom)
  {
    int[] topAndTopOffset = new int[2];
    int num1 = 0;
    WorksheetImpl workSheet = this.m_workSheet;
    if (bottomRowOffset != 0)
    {
      num1 = workSheet.GetInnerRowHeightInPixels(bottomRow);
      int num2 = (int) ((double) (num1 * bottomRowOffset) / (double) SizeProperties.FULL_ROW_OFFSET + 0.5);
      if (bottom <= num2)
      {
        topAndTopOffset[0] = bottomRow;
        topAndTopOffset[1] = (int) ((double) (num2 - bottom) * (double) SizeProperties.FULL_ROW_OFFSET / (double) num1 + (double) bottomRowOffset + 0.5);
        return topAndTopOffset;
      }
      bottom -= num2;
    }
    for (--bottomRow; bottomRow >= 0; --bottomRow)
    {
      num1 = workSheet.GetInnerRowHeightInPixels(bottomRow);
      bottom -= num1;
      if (bottom <= 0)
        break;
    }
    if (bottom <= 0 && (bottom != 0 || bottomRow > 0))
    {
      if (bottom == 0)
      {
        topAndTopOffset[0] = bottomRow;
        return topAndTopOffset;
      }
      topAndTopOffset[0] = bottomRow;
      topAndTopOffset[1] = (int) ((double) -bottom * (double) SizeProperties.FULL_ROW_OFFSET / (double) num1 + 0.5);
    }
    return topAndTopOffset;
  }

  internal int[] GetLeftAndLeftOffset(int rightColumn, int rightColumnOffset, int right)
  {
    int[] leftAndLeftOffset = new int[2];
    if (right == 0)
    {
      leftAndLeftOffset[0] = rightColumn;
      leftAndLeftOffset[1] = rightColumnOffset;
      return leftAndLeftOffset;
    }
    int num1 = 0;
    WorksheetImpl workSheet = this.m_workSheet;
    if (rightColumnOffset != 0)
    {
      num1 = workSheet.GetViewColumnWidthPixel(rightColumn);
      int num2 = (int) ((double) (num1 * rightColumnOffset) / (double) SizeProperties.FULL_COLUMN_OFFSET + 0.5);
      if (right <= num2)
      {
        leftAndLeftOffset[0] = rightColumn;
        leftAndLeftOffset[1] = (int) ((double) (num2 - right) * (double) SizeProperties.FULL_COLUMN_OFFSET / (double) num1 + (double) rightColumnOffset + 0.5);
        return leftAndLeftOffset;
      }
      right -= num2;
    }
    for (--rightColumn; rightColumn >= 0; --rightColumn)
    {
      num1 = workSheet.GetViewColumnWidthPixel(rightColumn);
      right -= num1;
      if (right <= 0)
        break;
    }
    if (right <= 0 && (right != 0 || rightColumn > 0))
    {
      leftAndLeftOffset[0] = rightColumn;
      if (right != 0)
      {
        leftAndLeftOffset[0] = rightColumn;
        leftAndLeftOffset[1] = (int) ((double) -right * (double) SizeProperties.FULL_COLUMN_OFFSET / (double) num1 + 0.5);
      }
    }
    return leftAndLeftOffset;
  }

  internal void SetAnchor(int left, int top, int right, int bottom)
  {
    this.m_size.SetPlacementType(PlacementType.FreeFloating);
    this.m_size.Top = top;
    this.m_size.Left = left;
    this.m_size.Right = right;
    this.m_size.Bottom = bottom;
  }

  internal void SetAnchor(int topRow, int top, int leftColumn, int left, int height, int width)
  {
    PlacementType placement = this.Placement;
    int rowHeightInPixels = this.m_workSheet.GetInnerRowHeightInPixels(topRow);
    top = (int) ((double) top * (double) SizeProperties.FULL_ROW_OFFSET / (double) rowHeightInPixels + 0.5);
    int columnWidthPixel = this.m_workSheet.GetViewColumnWidthPixel(leftColumn);
    left = (int) ((double) left * (double) SizeProperties.FULL_COLUMN_OFFSET / (double) columnWidthPixel + 0.5);
    this.m_size.Right = width;
    this.m_size.Bottom = height;
    this.m_size.SetLeftColumn(leftColumn);
    this.m_size.Left = left;
    this.m_size.SetTopRow(topRow);
    this.m_size.Top = top;
    if (placement == PlacementType.Move)
      return;
    this.m_size.SetPlacementType(PlacementType.Move);
    this.Placement = placement;
  }

  internal void SetAnchor(
    int topRow,
    int topRowOffset,
    int leftColumn,
    int leftColumnOffset,
    int bottomRow,
    int bottomRowOffset,
    int rightColumn,
    int rightColumnOffset)
  {
    PlacementType placement = this.Placement;
    int rowHeightInPixels1 = this.m_workSheet.GetInnerRowHeightInPixels(topRow);
    topRowOffset = rowHeightInPixels1 > topRowOffset ? (int) ((double) topRowOffset * (double) SizeProperties.FULL_ROW_OFFSET / (double) rowHeightInPixels1 + 0.5) : (int) SizeProperties.FULL_ROW_OFFSET;
    int columnWidthPixel1 = this.m_workSheet.GetViewColumnWidthPixel(leftColumn);
    leftColumnOffset = columnWidthPixel1 > leftColumnOffset ? (int) ((double) leftColumnOffset * (double) SizeProperties.FULL_COLUMN_OFFSET / (double) columnWidthPixel1 + 0.5) : (int) SizeProperties.FULL_COLUMN_OFFSET;
    this.m_size.SetLeftColumn(leftColumn);
    this.m_size.Left = leftColumnOffset;
    this.m_size.SetTopRow(topRow);
    this.m_size.Top = topRowOffset;
    int rowHeightInPixels2 = this.m_workSheet.GetInnerRowHeightInPixels(bottomRow);
    bottomRowOffset = rowHeightInPixels2 > bottomRowOffset ? (int) ((double) bottomRowOffset * (double) SizeProperties.FULL_ROW_OFFSET / (double) rowHeightInPixels2 + 0.5) : (int) SizeProperties.FULL_ROW_OFFSET;
    if (rightColumn < 16383 /*0x3FFF*/)
    {
      int columnWidthPixel2 = this.m_workSheet.GetViewColumnWidthPixel(rightColumn);
      rightColumnOffset = columnWidthPixel2 > rightColumnOffset ? (int) ((double) rightColumnOffset * (double) SizeProperties.FULL_COLUMN_OFFSET / (double) columnWidthPixel2 + 0.5) : (int) SizeProperties.FULL_COLUMN_OFFSET;
    }
    else
    {
      rightColumn = 16383 /*0x3FFF*/;
      rightColumnOffset = (int) SizeProperties.FULL_COLUMN_OFFSET;
    }
    this.m_size.SetRightColumn(rightColumn);
    this.m_size.Right = rightColumnOffset;
    this.m_size.SetBottomRow(bottomRow);
    this.m_size.Bottom = bottomRowOffset;
    if (placement == PlacementType.MoveAndSize)
      return;
    this.m_size.SetPlacementType(PlacementType.MoveAndSize);
    this.Placement = placement;
  }
}
