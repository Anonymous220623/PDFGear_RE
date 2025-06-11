// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IWorksheet
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Implementation.Collections;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IWorksheet : ITabSheet, IParentApplication
{
  event MissingFunctionEventHandler MissingFunction;

  IRange[] Cells { get; }

  bool DisplayPageBreaks { get; set; }

  int Index { get; }

  IRange[] MergedCells { get; }

  INames Names { get; }

  IPageSetup PageSetup { get; }

  IRange Range { get; }

  IRange[] Rows { get; }

  IRange[] Columns { get; }

  double StandardHeight { get; set; }

  bool StandardHeightFlag { get; set; }

  double StandardWidth { get; set; }

  OfficeSheetType Type { get; }

  IRange UsedRange { get; }

  int Zoom { get; set; }

  int VerticalSplit { get; set; }

  int HorizontalSplit { get; set; }

  int FirstVisibleRow { get; set; }

  int FirstVisibleColumn { get; set; }

  int ActivePane { get; set; }

  bool IsDisplayZeros { get; set; }

  bool IsGridLinesVisible { get; set; }

  OfficeKnownColors GridLineColor { get; set; }

  bool IsRowColumnHeadersVisible { get; set; }

  bool IsStringsPreserved { get; set; }

  IRange this[int row, int column] { get; }

  IRange this[int row, int column, int lastRow, int lastColumn] { get; }

  IRange this[string name] { get; }

  IRange this[string name, bool IsR1C1Notation] { get; }

  IRange[] UsedCells { get; }

  bool UseRangesCache { get; set; }

  bool IsFreezePanes { get; }

  IRange SplitCell { get; }

  int TopVisibleRow { get; set; }

  int LeftVisibleColumn { get; set; }

  bool UsedRangeIncludesFormatting { get; set; }

  OfficeSheetView View { get; set; }

  void Clear();

  void ClearData();

  bool Contains(int iRow, int iColumn);

  IRanges CreateRangesCollection();

  void CreateNamedRanges(string namedRange, string referRange, bool vertical);

  bool IsColumnVisible(int columnIndex);

  void ShowColumn(int columnIndex, bool isVisible);

  void HideColumn(int columnIndex);

  void HideRow(int rowIndex);

  bool IsRowVisible(int rowIndex);

  void ShowRow(int rowIndex, bool isVisible);

  void ShowRange(IRange range, bool isVisible);

  void ShowRange(RangesCollection ranges, bool isVisible);

  void ShowRange(IRange[] ranges, bool isVisible);

  void InsertRow(int index);

  void InsertRow(int iRowIndex, int iRowCount);

  void InsertRow(int iRowIndex, int iRowCount, OfficeInsertOptions insertOptions);

  void InsertColumn(int index);

  void InsertColumn(int iColumnIndex, int iColumnCount);

  void InsertColumn(int iColumnIndex, int iColumnCount, OfficeInsertOptions insertOptions);

  void DeleteRow(int index);

  void DeleteRow(int index, int count);

  void DeleteColumn(int index);

  void DeleteColumn(int index, int count);

  int ImportArray(object[] arrObject, int firstRow, int firstColumn, bool isVertical);

  int ImportArray(string[] arrString, int firstRow, int firstColumn, bool isVertical);

  int ImportArray(int[] arrInt, int firstRow, int firstColumn, bool isVertical);

  int ImportArray(double[] arrDouble, int firstRow, int firstColumn, bool isVertical);

  int ImportArray(DateTime[] arrDateTime, int firstRow, int firstColumn, bool isVertical);

  int ImportArray(object[,] arrObject, int firstRow, int firstColumn);

  int ImportData(IEnumerable arrObject, int firstRow, int firstColumn, bool includeHeader);

  int ImportDataColumn(
    DataColumn dataColumn,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn);

  int ImportDataTable(DataTable dataTable, bool isFieldNameShown, int firstRow, int firstColumn);

  int ImportDataTable(DataTable dataTable, int firstRow, int firstColumn, bool importOnSave);

  int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    bool preserveTypes);

  int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns);

  int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    bool preserveTypes);

  int ImportDataTable(DataTable dataTable, IName namedRange, bool isFieldNameShown);

  int ImportDataTable(
    DataTable dataTable,
    IName namedRange,
    bool isFieldNameShown,
    int rowOffset,
    int columnOffset);

  int ImportDataTable(
    DataTable dataTable,
    IName namedRange,
    bool isFieldNameShown,
    int rowOffset,
    int columnOffset,
    int iMaxRow,
    int iMaxCol);

  int ImportDataTable(
    DataTable dataTable,
    IName namedRange,
    bool isFieldNameShown,
    int rowOffset,
    int columnOffset,
    int iMaxRow,
    int iMaxCol,
    bool bPreserveTypes);

  int ImportDataView(DataView dataView, bool isFieldNameShown, int firstRow, int firstColumn);

  int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    bool bPreserveTypes);

  int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns);

  int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    bool bPreserveTypes);

  void RemovePanes();

  IRange IntersectRanges(IRange range1, IRange range2);

  IRange MergeRanges(IRange range1, IRange range2);

  void AutofitRow(int rowIndex);

  void AutofitColumn(int colIndex);

  void Replace(string oldValue, string newValue);

  void Replace(string oldValue, double newValue);

  void Replace(string oldValue, DateTime newValue);

  void Replace(string oldValue, string[] newValues, bool isVertical);

  void Replace(string oldValue, int[] newValues, bool isVertical);

  void Replace(string oldValue, double[] newValues, bool isVertical);

  void Replace(string oldValue, DataTable newValues, bool isFieldNamesShown);

  void Replace(string oldValue, DataColumn newValues, bool isFieldNamesShown);

  void Remove();

  void Move(int iNewIndex);

  int ColumnWidthToPixels(double widthInChars);

  double PixelsToColumnWidth(int pixels);

  void SetColumnWidth(int iColumnIndex, double value);

  void SetColumnWidthInPixels(int iColumnIndex, int value);

  void SetColumnWidthInPixels(int iStartColumnIndex, int iCount, int value);

  void SetRowHeight(int iRow, double value);

  void SetRowHeightInPixels(int iRowIndex, double value);

  void SetRowHeightInPixels(int iStartRowIndex, int iCount, double value);

  double GetColumnWidth(int iColumnIndex);

  int GetColumnWidthInPixels(int iColumnIndex);

  double GetRowHeight(int iRow);

  int GetRowHeightInPixels(int iRowIndex);

  IRange FindFirst(string findValue, OfficeFindType flags);

  IRange FindFirst(string findValue, OfficeFindType flags, OfficeFindOptions findOptions);

  IRange FindStringStartsWith(string findValue, OfficeFindType flags);

  IRange FindStringStartsWith(string findValue, OfficeFindType flags, bool ignoreCase);

  IRange FindStringEndsWith(string findValue, OfficeFindType flags);

  IRange FindStringEndsWith(string findValue, OfficeFindType flags, bool ignoreCase);

  IRange FindFirst(double findValue, OfficeFindType flags);

  IRange FindFirst(bool findValue);

  IRange FindFirst(DateTime findValue);

  IRange FindFirst(TimeSpan findValue);

  IRange[] FindAll(string findValue, OfficeFindType flags);

  IRange[] FindAll(string findValue, OfficeFindType flags, OfficeFindOptions findOptions);

  IRange[] FindAll(double findValue, OfficeFindType flags);

  IRange[] FindAll(bool findValue);

  IRange[] FindAll(DateTime findValue);

  IRange[] FindAll(TimeSpan findValue);

  void SaveAs(string fileName, string separator);

  void SaveAs(string fileName, string separator, Encoding encoding);

  void SaveAs(Stream stream, string separator);

  void SaveAs(Stream stream, string separator, Encoding encoding);

  void SetDefaultColumnStyle(int iColumnIndex, IStyle defaultStyle);

  void SetDefaultColumnStyle(int iStartColumnIndex, int iEndColumnIndex, IStyle defaultStyle);

  void SetDefaultRowStyle(int iRowIndex, IStyle defaultStyle);

  void SetDefaultRowStyle(int iStartRowIndex, int iEndRowIndex, IStyle defaultStyle);

  IStyle GetDefaultColumnStyle(int iColumnIndex);

  IStyle GetDefaultRowStyle(int iRowIndex);

  void FreeRange(IRange range);

  void FreeRange(int iRow, int iColumn);

  void SetValue(int iRow, int iColumn, string value);

  void SetNumber(int iRow, int iColumn, double value);

  void SetBoolean(int iRow, int iColumn, bool value);

  void SetText(int iRow, int iColumn, string value);

  void SetFormula(int iRow, int iColumn, string value);

  void SetError(int iRow, int iColumn, string value);

  void SetBlank(int iRow, int iColumn);

  void SetFormulaNumberValue(int iRow, int iColumn, double value);

  void SetFormulaErrorValue(int iRow, int iColumn, string value);

  void SetFormulaBoolValue(int iRow, int iColumn, bool value);

  void SetFormulaStringValue(int iRow, int iColumn, string value);

  IMigrantRange MigrantRange { get; }

  string GetText(int row, int column);

  double GetNumber(int row, int column);

  string GetFormula(int row, int column, bool bR1C1);

  string GetError(int row, int column);

  bool GetBoolean(int row, int column);

  bool GetFormulaBoolValue(int row, int column);

  string GetFormulaErrorValue(int row, int column);

  double GetFormulaNumberValue(int row, int column);

  string GetFormulaStringValue(int row, int column);

  event RangeImpl.CellValueChangedEventHandler CellValueChanged;
}
