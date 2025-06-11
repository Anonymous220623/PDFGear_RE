// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.ParseParameters
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class ParseParameters
{
  public readonly FormulaUtil FormulaUtility;
  public readonly IWorksheet Worksheet;
  public readonly Dictionary<string, string> WorksheetNames;
  public readonly bool IsR1C1;
  public readonly int CellRow;
  public readonly int CellColumn;
  public readonly IWorkbook Workbook;
  public readonly OfficeVersion Version;

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
