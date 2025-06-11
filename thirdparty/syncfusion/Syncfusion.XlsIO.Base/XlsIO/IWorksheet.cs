// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IWorksheet
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Calculate;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Forms;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IWorksheet : ITabSheet, IParentApplication, ICalcData
{
  CalcEngine CalcEngine { get; set; }

  void EnableSheetCalculations();

  void DisableSheetCalculations();

  event MissingFunctionEventHandler MissingFunction;

  IAutoFilters AutoFilters { get; }

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

  ExcelSheetType Type { get; }

  IRange UsedRange { get; }

  new int Zoom { get; set; }

  int VerticalSplit { get; set; }

  int HorizontalSplit { get; set; }

  int FirstVisibleRow { get; set; }

  int FirstVisibleColumn { get; set; }

  int ActivePane { get; set; }

  bool IsDisplayZeros { get; set; }

  bool IsGridLinesVisible { get; set; }

  ExcelKnownColors GridLineColor { get; set; }

  bool IsRowColumnHeadersVisible { get; set; }

  IVPageBreaks VPageBreaks { get; }

  IHPageBreaks HPageBreaks { get; }

  bool IsStringsPreserved { get; set; }

  IComments Comments { get; }

  IRange this[int row, int column] { get; }

  IRange this[int row, int column, int lastRow, int lastColumn] { get; }

  IRange this[string name] { get; }

  IRange this[string name, bool IsR1C1Notation] { get; }

  IHyperLinks HyperLinks { get; }

  IRange[] UsedCells { get; }

  IWorksheetCustomProperties CustomProperties { get; }

  bool UseRangesCache { get; set; }

  bool IsFreezePanes { get; }

  IRange SplitCell { get; }

  int TopVisibleRow { get; set; }

  int LeftVisibleColumn { get; set; }

  bool UsedRangeIncludesFormatting { get; set; }

  IPivotTables PivotTables { get; }

  IListObjects ListObjects { get; }

  SheetView View { get; set; }

  IOleObjects OleObjects { get; }

  bool HasOleObject { get; }

  ISparklineGroups SparklineGroups { get; }

  IDataSort DataSorter { get; }

  void CopyToClipboard();

  void Clear();

  void ClearData();

  bool Contains(int iRow, int iColumn);

  IRanges CreateRangesCollection();

  void CreateNamedRanges(string namedRange, string referRange, bool vertical);

  ITemplateMarkersProcessor CreateTemplateMarkersProcessor();

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

  void InsertRow(int iRowIndex, int iRowCount, ExcelInsertOptions insertOptions);

  void InsertColumn(int index);

  void InsertColumn(int iColumnIndex, int iColumnCount);

  void InsertColumn(int iColumnIndex, int iColumnCount, ExcelInsertOptions insertOptions);

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

  int ImportData(IEnumerable arrObject, ExcelImportDataOptions importOptions);

  int ImportDataColumn(
    DataColumn dataColumn,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn);

  int ImportDataColumn(
    DataColumn dataColumn,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    bool preserveTypes);

  void ImportDataGrid(
    System.Windows.Forms.DataGrid dataGrid,
    int firstRow,
    int firstColumn,
    bool isImportHeader,
    bool isImportStyle);

  void ImportDataGrid(
    System.Web.UI.WebControls.DataGrid dataGrid,
    int firstRow,
    int firstColumn,
    bool isImportHeader,
    bool isImportStyle);

  void ImportGridView(
    GridView gridView,
    int firstRow,
    int firstColumn,
    bool isImportHeader,
    bool isImportStyle);

  void ImportDataGridView(
    DataGridView dataGridView,
    int firstRow,
    int firstColumn,
    bool isImportHeader,
    bool isImportStyle);

  int ImportDataTable(DataTable dataTable, bool isFieldNameShown, int firstRow, int firstColumn);

  int ImportDataTable(DataTable dataTable, int firstRow, int firstColumn, bool importOnSave);

  int ImportDataTable(
    DataTable dataTable,
    int firstRow,
    int firstColumn,
    bool importOnSave,
    bool includeHeader);

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

  int ImportDataReader(
    IDataReader dataReader,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn);

  int ImportDataReader(IDataReader dataReader, int firstRow, int firstColumn, bool importOnSave);

  int ImportDataReader(
    IDataReader dataReader,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    bool preserveTypes);

  int ImportDataReader(IDataReader dataReader, IName namedRange, bool isFieldNameShown);

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

  DataTable ExportDataTable(
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    ExcelExportDataTableOptions options);

  DataTable ExportDataTable(IRange dataRange, ExcelExportDataTableOptions options);

  List<T> ExportData<T>(int firstRow, int firstColumn, int lastRow, int lastColumn) where T : new();

  List<T> ExportData<T>(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    Dictionary<string, string> mappingProperties)
    where T : new();

  IRange IntersectRanges(IRange range1, IRange range2);

  IRange MergeRanges(IRange range1, IRange range2);

  void AutofitRow(int rowIndex);

  void AutofitColumn(int colIndex);

  void Replace(string oldValue, string newValue);

  void Replace(string oldValue, string newValue, ExcelFindOptions findOptions);

  void Replace(string oldValue, double newValue);

  void Replace(string oldValue, DateTime newValue);

  void Replace(string oldValue, string[] newValues, bool isVertical);

  void Replace(string oldValue, int[] newValues, bool isVertical);

  void Replace(string oldValue, double[] newValues, bool isVertical);

  void Replace(string oldValue, DataTable newValues, bool isFieldNamesShown);

  void Replace(string oldValue, DataColumn newValues, bool isFieldNamesShown);

  void Remove();

  void Move(int iNewIndex);

  int ColumnWidthToPixels(double Width);

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

  IRange FindFirst(string findValue, ExcelFindType flags);

  IRange FindFirst(string findValue, ExcelFindType flags, ExcelFindOptions findOptions);

  IRange FindStringStartsWith(string findValue, ExcelFindType flags);

  IRange FindStringStartsWith(string findValue, ExcelFindType flags, bool ignoreCase);

  IRange FindStringEndsWith(string findValue, ExcelFindType flags);

  IRange FindStringEndsWith(string findValue, ExcelFindType flags, bool ignoreCase);

  IRange FindFirst(double findValue, ExcelFindType flags);

  IRange FindFirst(bool findValue);

  IRange FindFirst(DateTime findValue);

  IRange FindFirst(TimeSpan findValue);

  IRange[] FindAll(string findValue, ExcelFindType flags);

  IRange[] FindAll(string findValue, ExcelFindType flags, ExcelFindOptions findOptions);

  IRange[] FindAll(double findValue, ExcelFindType flags);

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

  System.Drawing.Image ConvertToImage(int firstRow, int firstColumn, int lastRow, int lastColumn);

  System.Drawing.Image ConvertToImage(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ImageType imageType,
    Stream stream);

  void SaveAsHtml(string filename);

  void SaveAsHtml(Stream stream);

  void SaveAsHtml(string filename, HtmlSaveOptions saveOptions);

  void SaveAsHtml(Stream stream, HtmlSaveOptions saveOptions);

  System.Drawing.Image ConvertToImage(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    EmfType emfType,
    Stream outputStream);

  System.Drawing.Image ConvertToImage(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ImageType imageType,
    Stream outputStream,
    EmfType emfType);

  void Calculate();

  event WorksheetImpl.ExportDataTableEventHandler ExportDataTableEvent;

  event RangeImpl.CellValueChangedEventHandler CellValueChanged;

  void AdvancedFilter(
    ExcelFilterAction filterInPlace,
    IRange filterRange,
    IRange criteriaRange,
    IRange copyToRange,
    bool isUnique);

  void ImportHtmlTable(string fileName, int row, int column);

  void ImportHtmlTable(Stream fileStream, int row, int column);
}
