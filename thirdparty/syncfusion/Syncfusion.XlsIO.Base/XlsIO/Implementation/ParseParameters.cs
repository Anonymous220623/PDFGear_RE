// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ParseParameters
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ParseParameters
{
  public FormulaUtil FormulaUtility;
  public IWorksheet Worksheet;
  public Dictionary<string, string> WorksheetNames;
  public readonly bool IsR1C1;
  public readonly int CellRow;
  public readonly int CellColumn;
  public IWorkbook Workbook;
  public readonly ExcelVersion Version;

  public ParseParameters(
    IWorksheet sheet,
    Dictionary<string, string> worksheetNames,
    bool r1C1,
    int cellRow,
    int cellColumn,
    FormulaUtil formulaUtility,
    IWorkbook book)
  {
    this.Worksheet = sheet;
    this.WorksheetNames = worksheetNames;
    this.IsR1C1 = r1C1;
    this.CellRow = cellRow;
    this.CellColumn = cellColumn;
    this.FormulaUtility = formulaUtility;
    this.Workbook = book;
    this.Version = ((WorkbookImpl) this.Workbook).Version;
  }
}
