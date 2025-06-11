// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TemplateMarkers.NewSpaceArgument
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.TemplateMarkers;

[TemplateMarker]
public class NewSpaceArgument : MarkerArgument
{
  private const int DEF_PRIORITY = 1;
  private const string DEF_MARKER_VALUE = "insert";
  private const int DEF_COPY_STYLES_GROUP = 1;
  private bool m_bInsertRow;
  private static readonly Regex s_newSpaceRegex = new Regex("insert(:copystyles,copymerges|:copymerges,copystyles|:copystyles|:copymerges)?", RegexOptions.Compiled);
  private bool m_bCopyStyles;
  private bool m_bCopyMerges;

  protected override MarkerArgument Parse(Match m)
  {
    if (m == null)
      throw new ArgumentNullException(nameof (m));
    this.m_bCopyMerges = false;
    this.m_bCopyStyles = false;
    if (m.Groups[1].Value.Length > 0)
    {
      string str = m.Groups[1].Value;
      if (str.Contains(","))
      {
        this.m_bCopyMerges = true;
        this.m_bCopyStyles = true;
      }
      else
      {
        switch (str)
        {
          case ":copystyles":
            this.m_bCopyStyles = true;
            break;
          case ":copymerges":
            this.m_bCopyMerges = true;
            break;
        }
      }
    }
    return (MarkerArgument) this.Clone();
  }

  public override void ApplyArgument(
    IWorksheet sheet,
    Point pOldPosition,
    ref int iRow,
    ref int iColumn,
    IList<long> arrMarkerCells,
    MarkerOptionsImpl options,
    int count)
  {
    if (count != 0)
      this.m_bInsertRow = false;
    ExcelInsertOptions insertOptions = this.m_bCopyStyles ? ExcelInsertOptions.FormatAsBefore : ExcelInsertOptions.FormatDefault;
    if (options.Direction == MarkerDirection.Horizontal)
    {
      int iColumnIndex = iColumn + 1;
      sheet.InsertColumn(iColumnIndex, 1, insertOptions);
      MarkerArgument.InsertColumn(arrMarkerCells, options.MarkerIndex, iColumnIndex, 1);
    }
    else
    {
      if (!this.m_bInsertRow)
      {
        this.m_bInsertRow = true;
        sheet.InsertRow(iRow + 1, count - 1, insertOptions);
      }
      int iRowIndex = iRow + 1;
      MarkerArgument.InsertRow(arrMarkerCells, options.MarkerIndex, iRowIndex, 1);
    }
  }

  public override int Priority => 1;

  public override bool IsApplyable => true;

  protected override Regex ArgumentChecker => NewSpaceArgument.s_newSpaceRegex;

  internal bool IsMergeEnabled => this.m_bCopyMerges;

  internal bool CopyStyles => this.m_bCopyStyles;

  internal bool IsInsertRow => this.m_bInsertRow;

  internal Point ApplyArgumentWithMerge(
    IWorksheet sheet,
    Point pOldPosition,
    ref int iRow,
    ref int iColumn,
    IList<long> arrMarkerCells,
    MarkerOptionsImpl options,
    int count)
  {
    if (count != 0)
      this.m_bInsertRow = false;
    ExcelInsertOptions insertOptions = this.m_bCopyStyles ? ExcelInsertOptions.FormatAsBefore : ExcelInsertOptions.FormatDefault;
    Point point = new Point(1, 1);
    WorksheetImpl worksheetImpl = sheet as WorksheetImpl;
    if (this.m_bCopyMerges)
      point = NewSpaceArgument.GetMergeCount(iRow, iColumn, sheet);
    if (options.Direction == MarkerDirection.Horizontal)
    {
      int iColumnIndex = iColumn + 1;
      if (point.X > 1 || point.Y > 1 || worksheetImpl.HasMergedCells && this.m_bCopyMerges)
      {
        sheet.InsertColumn(iColumn + point.Y, point.Y, insertOptions);
        sheet[iRow, iColumn + point.Y, iRow + point.X - 1, iColumn + 2 * point.Y - 1].Merge();
        options.IsMergeApplied = true;
      }
      else
      {
        sheet.InsertColumn(iColumnIndex, 1, insertOptions);
        if (options.IsMergeApplied)
          options.IsMergeApplied = false;
      }
      if (point.Y > 1)
        iColumnIndex = iColumn + point.Y;
      MarkerArgument.InsertColumn(arrMarkerCells, options.MarkerIndex, iColumnIndex, point.Y);
    }
    else
    {
      if (!this.m_bInsertRow)
      {
        this.m_bInsertRow = true;
        if (point.X > 1 || point.Y > 1 || worksheetImpl.HasMergedCells && this.m_bCopyMerges)
        {
          sheet.InsertRow(iRow + point.X, point.X * (count - 1), insertOptions);
          options.IsMergeApplied = true;
          for (int index1 = 0; index1 < arrMarkerCells.Count; ++index1)
          {
            int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(arrMarkerCells[index1]);
            int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(arrMarkerCells[index1]);
            if (iRow == rowFromCellIndex)
            {
              Point mergeCount = NewSpaceArgument.GetMergeCount(rowFromCellIndex, columnFromCellIndex, sheet);
              if (mergeCount.X > 1 || mergeCount.Y > 1)
              {
                for (int index2 = 1; index2 < count; ++index2)
                {
                  int x = mergeCount.X;
                  int y = mergeCount.Y;
                  sheet[rowFromCellIndex + x * index2, columnFromCellIndex, rowFromCellIndex + x * index2 + x - 1, columnFromCellIndex + y - 1].Merge();
                }
              }
            }
          }
        }
        else
        {
          sheet.InsertRow(iRow + 1, count - 1, insertOptions);
          if (options.IsMergeApplied)
            options.IsMergeApplied = false;
        }
      }
      int iRowIndex = iRow + 1;
      if (point.X > 1)
        iRowIndex = iRow + point.X;
      MarkerArgument.InsertRow(arrMarkerCells, options.MarkerIndex, iRowIndex, point.X);
    }
    return point;
  }

  internal static Point GetMergeCount(int row, int column, IWorksheet sheet)
  {
    WorksheetImpl worksheetImpl = sheet as WorksheetImpl;
    if (!worksheetImpl.HasMergedCells)
      return new Point(1, 1);
    Rectangle rect = new Rectangle(column - 1, row - 1, 0, 0);
    MergeCellsRecord.MergedRegion mergeCell = worksheetImpl.MergeCells[rect];
    if (mergeCell == null)
      return new Point(1, 1);
    int y = mergeCell.ColumnTo - mergeCell.ColumnFrom + 1;
    return new Point(mergeCell.RowTo - mergeCell.RowFrom + 1, y);
  }
}
