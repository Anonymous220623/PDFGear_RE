// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TemplateMarkers.JumpArgument
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.TemplateMarkers;

[TemplateMarker]
public class JumpArgument : MarkerArgument
{
  protected const string DEF_ROW_INDEX_GROUP = "RowIndex";
  protected const string DEF_COLUMN_INDEX_GROUP = "ColumnIndex";
  protected const string DEF_COPY_STYLES_GROUP = "CopyStyles";
  private const string DEF_JUMP = "jump";
  protected const string DEF_COPY_STYLES = "copystyles";
  protected const int DEF_ROW_RELATIVE_GROUP = 1;
  protected const int DEF_COLUMN_RELATIVE_GROUP = 2;
  protected const int DEF_PRIORITY = 2;
  protected const string DEF_R1C1_CELL_REGEX = "R(\\[)?(?<RowIndex>[\\-]?[\\0-9]{0,5})(?(1)\\])C(\\[)?(?<ColumnIndex>[\\-]?[0-9]{0,3})(?(2)\\])";
  private static readonly Regex s_cellRegex = new Regex("R(\\[)?(?<RowIndex>[\\-]?[\\0-9]{0,5})(?(1)\\])C(\\[)?(?<ColumnIndex>[\\-]?[0-9]{0,3})(?(2)\\])", RegexOptions.Compiled);
  protected int m_iRow;
  protected int m_iColumn;
  protected bool m_bRowRelative;
  protected bool m_bColumnRelative;
  protected bool m_bCopyStyles;

  public JumpArgument()
  {
  }

  public JumpArgument(int iRow, int iColumn, bool bRowRelative, bool bColumnRelative)
  {
    this.m_iRow = iRow;
    this.m_iColumn = iColumn;
    this.m_bRowRelative = bRowRelative;
    this.m_bColumnRelative = bColumnRelative;
  }

  public override MarkerArgument TryParse(string strArgument)
  {
    if (strArgument == null || strArgument.Length == 0)
      return (MarkerArgument) null;
    string[] strArray = strArgument.Split(':');
    int length = strArray.Length;
    if (strArray[0].ToLower() != "jump")
      return (MarkerArgument) null;
    if (length > 1)
    {
      this.m_bCopyStyles = strArray[length - 1].ToLower() == "copystyles";
      if (this.m_bCopyStyles)
        --length;
    }
    int index = length - 1;
    return index > 0 && !JumpArgument.TryParseCell(strArray[index], out this.m_iRow, out this.m_iColumn, out this.m_bRowRelative, out this.m_bColumnRelative) ? (MarkerArgument) null : (MarkerArgument) this.Clone();
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
    int row = iRow;
    int column = iColumn;
    Point cellLocation = this.GetCellLocation(pOldPosition, options.Workbook);
    iRow = cellLocation.X;
    iColumn = cellLocation.Y;
    if (!this.m_bCopyStyles || iColumn == 0 || iRow == 0)
      return;
    WorksheetImpl worksheetImpl = sheet != null ? (WorksheetImpl) sheet : throw new ArgumentNullException(nameof (sheet));
    worksheetImpl.Range[row, column].CopyTo(worksheetImpl[iRow, iColumn], ExcelCopyRangeOptions.CopyStyles);
  }

  protected Point GetCellLocation(Point pointStart, IWorkbook book)
  {
    return JumpArgument.GetCellLocation(pointStart, this.m_iRow, this.m_iColumn, this.m_bRowRelative, this.m_bColumnRelative, book);
  }

  private static bool TryParseInt(string strToParse, out int iResult)
  {
    if (strToParse.Length == 0)
    {
      iResult = 0;
      return true;
    }
    iResult = 0;
    double result;
    if (!double.TryParse(strToParse, NumberStyles.Integer, (IFormatProvider) null, out result) || Math.Abs(result) > (double) int.MaxValue)
      return false;
    iResult = (int) result;
    return true;
  }

  protected static bool TryParseCell(
    string strToParse,
    out int iRow,
    out int iColumn,
    out bool bRowRelative,
    out bool bColumnRelative)
  {
    iRow = iColumn = 0;
    bRowRelative = bColumnRelative = false;
    if (strToParse == null || strToParse.Length == 0)
      return false;
    Match match = JumpArgument.s_cellRegex.Match(strToParse);
    bool cell = match.Success && match.Length == strToParse.Length;
    if (cell)
    {
      bRowRelative = match.Groups[1].Value.Length > 0;
      bColumnRelative = match.Groups[2].Value.Length > 0;
      if (!JumpArgument.TryParseInt(match.Groups["RowIndex"].Value, out iRow))
        return false;
      if (iRow == 0)
        bRowRelative = true;
      if (!JumpArgument.TryParseInt(match.Groups["ColumnIndex"].Value, out iColumn))
        return false;
      if (iColumn == 0)
        bColumnRelative = true;
    }
    return cell;
  }

  protected static Point GetCellLocation(
    Point pointStart,
    int iRow,
    int iColumn,
    bool bRowRelative,
    bool bColumnRelative,
    IWorkbook book)
  {
    int y = bColumnRelative ? pointStart.Y + iColumn : iColumn;
    int x = bRowRelative ? pointStart.X + iRow : iRow;
    if (y < 1 || y > book.MaxColumnCount)
      y = 0;
    if (x < 1 || x > book.MaxRowCount)
      x = 0;
    return new Point(x, y);
  }

  public override int Priority => 2;

  public override bool IsApplyable => true;

  internal void UpdateRowColumn(Point mergedPoint)
  {
    if (this.m_iRow > 1 || this.m_iColumn > 1)
      return;
    if (this.m_iRow == 1)
      this.m_iRow = mergedPoint.X;
    else
      this.m_iColumn = mergedPoint.Y;
  }
}
