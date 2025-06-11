// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IPivotTable
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.PivotAnalysis;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IPivotTable
{
  string Name { get; set; }

  IPivotFields Fields { get; }

  IPivotDataFields DataFields { get; }

  bool RowGrand { get; set; }

  bool ColumnGrand { get; set; }

  bool ShowDrillIndicators { get; set; }

  bool DisplayFieldCaptions { get; set; }

  bool RepeatItemsOnEachPrintedPage { get; set; }

  PivotBuiltInStyles? BuiltInStyle { get; set; }

  bool ShowRowGrand { get; set; }

  bool ShowColumnGrand { get; set; }

  int CacheIndex { get; }

  IRange Location { get; set; }

  IPivotTableOptions Options { get; }

  int RowsPerPage { get; }

  int ColumnsPerPage { get; }

  IPivotCalculatedFields CalculatedFields { get; }

  IPivotFields PageFields { get; }

  IPivotFields RowFields { get; }

  IPivotFields ColumnFields { get; }

  bool ShowDataFieldInRow { get; set; }

  void ClearTable();

  IPivotCellFormat GetCellFormat(string range);

  void Layout();

  PivotEngine PivotEngineValues { get; set; }
}
