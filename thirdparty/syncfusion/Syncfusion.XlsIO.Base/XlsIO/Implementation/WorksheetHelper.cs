// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.WorksheetHelper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class WorksheetHelper
{
  public static bool HasFormulaRecord(IInternalWorksheet sheet, int row, int column)
  {
    return sheet.CellRecords.Table.HasFormulaRecord(row, column);
  }

  public static RowStorage GetOrCreateRow(IInternalWorksheet sheet, int rowIndex, bool bCreate)
  {
    CellRecordCollection cellRecords = sheet.CellRecords;
    if (cellRecords == null && !bCreate)
      return (RowStorage) null;
    ExcelVersion version = sheet.Workbook.Version;
    int heightInRowUnits = (sheet.Application as ApplicationImpl).StandardHeightInRowUnits;
    return cellRecords.Table.GetOrCreateRow(rowIndex, heightInRowUnits, bCreate, version);
  }

  [CLSCompliant(false)]
  public static IOutline GetRowOutline(IInternalWorksheet sheet, int iRowIndex)
  {
    return (IOutline) WorksheetHelper.GetOrCreateRow(sheet, iRowIndex - 1, false);
  }

  public static void AccessColumn(IInternalWorksheet sheet, int iColumnIndex)
  {
    int firstColumn = sheet.FirstColumn;
    int lastColumn = sheet.LastColumn;
    if (firstColumn > iColumnIndex || firstColumn == int.MaxValue)
      sheet.FirstColumn = (int) (ushort) iColumnIndex;
    if (lastColumn >= iColumnIndex && lastColumn != int.MaxValue)
      return;
    sheet.LastColumn = (int) (ushort) iColumnIndex;
  }

  public static void AccessRow(IInternalWorksheet sheet, int iRowIndex)
  {
    int firstRow = sheet.FirstRow;
    int lastRow = sheet.LastRow;
    if (firstRow > iRowIndex || firstRow < 0)
      sheet.FirstRow = iRowIndex;
    if (lastRow >= iRowIndex && lastRow >= 0)
      return;
    sheet.LastRow = iRowIndex;
  }
}
