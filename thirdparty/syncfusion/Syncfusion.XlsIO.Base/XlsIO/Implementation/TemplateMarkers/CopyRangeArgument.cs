// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TemplateMarkers.CopyRangeArgument
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.TemplateMarkers;

[TemplateMarker]
public class CopyRangeArgument : JumpArgument
{
  private const string DEF_COPYRANGE = "copyrange";
  private const ExcelCopyRangeOptions DEF_DEFAULT_COPY_OPTIONS = ExcelCopyRangeOptions.UpdateFormulas | ExcelCopyRangeOptions.UpdateMerges;
  private Rectangle m_copyRangeRect = Rectangle.Empty;
  private int m_iSecondRow;
  private int m_iSecondColumn;
  private bool m_bSecondRowRelative;
  private bool m_bSecondColumnRelative;

  public override MarkerArgument TryParse(string strArgument)
  {
    if (strArgument == null || strArgument.Length == 0)
      return (MarkerArgument) null;
    string[] strArray = strArgument.Split(':');
    int length = strArray.Length;
    if (strArray[0].ToLower() != "copyrange")
      return (MarkerArgument) null;
    if (length > 1)
    {
      this.m_bCopyStyles = strArray[length - 1].ToLower() == "copystyles";
      if (this.m_bCopyStyles)
        --length;
    }
    int index = 1;
    if (index < length)
    {
      if (!JumpArgument.TryParseCell(strArray[index], out this.m_iRow, out this.m_iColumn, out this.m_bRowRelative, out this.m_bColumnRelative))
        return (MarkerArgument) null;
      ++index;
    }
    if (index < length)
    {
      if (!JumpArgument.TryParseCell(strArray[index], out this.m_iSecondRow, out this.m_iSecondColumn, out this.m_bSecondRowRelative, out this.m_bSecondColumnRelative))
        return (MarkerArgument) null;
    }
    else
    {
      this.m_iSecondRow = this.m_iRow;
      this.m_iSecondColumn = this.m_iColumn;
      this.m_bSecondRowRelative = this.m_bRowRelative;
      this.m_bSecondColumnRelative = this.m_bColumnRelative;
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
    int dx = iRow - pOldPosition.X;
    int dy = iColumn - pOldPosition.Y;
    if (dx == 0 && dy == 1 || dx == 1 && dy == 0)
    {
      if (this.m_copyRangeRect.IsEmpty)
      {
        Point cellLocation = this.GetCellLocation(pOldPosition, options.Workbook);
        cellLocation.Offset(dx, dy);
        this.m_copyRangeRect = new Rectangle(cellLocation.Y, cellLocation.X, 0, 0);
      }
      else
      {
        this.m_copyRangeRect.Width += dy;
        this.m_copyRangeRect.Height += dx;
      }
    }
    else
    {
      Point cellLocation1 = this.GetCellLocation(pOldPosition, options.Workbook);
      Point cellLocation2 = JumpArgument.GetCellLocation(pOldPosition, this.m_iSecondRow, this.m_iSecondColumn, this.m_bSecondRowRelative, this.m_bSecondColumnRelative, options.Workbook);
      Point point = cellLocation1;
      point.Offset(dx, dy);
      ((WorksheetImpl) sheet).CopyRange(sheet.Range[point.X, point.Y], sheet.Range[cellLocation1.X, cellLocation1.Y, cellLocation2.X, cellLocation2.Y], this.m_bCopyStyles ? ExcelCopyRangeOptions.UpdateFormulas | ExcelCopyRangeOptions.UpdateMerges : ExcelCopyRangeOptions.All);
    }
  }

  public override int Priority => base.Priority + 1;

  public override bool IsAllowMultiple => true;

  internal void TryAndApplyCopy(IWorksheet sheet, MarkerOptionsImpl options, Point startPoint)
  {
    if (this.m_copyRangeRect.IsEmpty)
      return;
    Point cellLocation1 = this.GetCellLocation(startPoint, options.Workbook);
    Point cellLocation2 = JumpArgument.GetCellLocation(startPoint, this.m_iSecondRow, this.m_iSecondColumn, this.m_bSecondRowRelative, this.m_bSecondColumnRelative, options.Workbook);
    ((WorksheetImpl) sheet).CopyRange(sheet.Range[this.m_copyRangeRect.Top, this.m_copyRangeRect.Left, this.m_copyRangeRect.Bottom, this.m_copyRangeRect.Right], sheet.Range[cellLocation1.X, cellLocation1.Y, cellLocation2.X, cellLocation2.Y], this.m_bCopyStyles ? ExcelCopyRangeOptions.UpdateFormulas | ExcelCopyRangeOptions.UpdateMerges : ExcelCopyRangeOptions.All);
  }
}
