// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.ExternWorksheetImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class ExternWorksheetImpl : 
  CommonObject,
  IInternalWorksheet,
  IWorksheet,
  ITabSheet,
  IParentApplication,
  ICloneParent
{
  private XCTRecord m_xct = (XCTRecord) BiffRecordFactory.GetRecord(TBIFFRecord.XCT);
  private List<BiffRecordRaw> m_arrRecords = new List<BiffRecordRaw>();
  private ExternWorkbookImpl m_book;
  private string m_strName;
  private CellRecordCollection m_dicRecordsCells;
  private int m_iFirstRow = -1;
  private int m_iFirstColumn = int.MaxValue;
  private int m_iLastRow = -1;
  private int m_iLastColumn = int.MaxValue;
  private Dictionary<string, string> m_dicAdditionalAttributes;
  internal int unknown_formula_name = 9;

  public event RangeImpl.CellValueChangedEventHandler CellValueChanged;

  public event MissingFunctionEventHandler MissingFunction;

  public ExternWorksheetImpl(IApplication application, ExternWorkbookImpl parent)
    : base(application, (object) parent)
  {
    this.m_book = parent;
    this.m_dicRecordsCells = new CellRecordCollection(this.Application, (object) this);
    this.m_dicAdditionalAttributes = new Dictionary<string, string>();
    this.m_dicAdditionalAttributes["refreshError"] = "1";
  }

  [CLSCompliant(false)]
  public int Parse(BiffRecordRaw[] arrData, int iOffset)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset > arrData.Length - 1)
      throw new ArgumentOutOfRangeException(nameof (iOffset), "Value cannot be less than 0 and greater than arrData.Length - 1");
    BiffRecordRaw biffRecordRaw = arrData[iOffset];
    if (biffRecordRaw.TypeCode != TBIFFRecord.XCT)
      return iOffset;
    XCTRecord xctRecord = (XCTRecord) biffRecordRaw;
    ++iOffset;
    this.m_arrRecords.Clear();
    this.m_arrRecords.Add((BiffRecordRaw) xctRecord);
    int num = 0;
    int crnCount = (int) xctRecord.CRNCount;
    while (num < crnCount)
    {
      BiffRecordRaw crn = arrData[iOffset];
      crn.CheckTypeCode(TBIFFRecord.CRN);
      this.ParseCRN((CRNRecord) crn);
      this.m_arrRecords.Add(crn);
      ++num;
      ++iOffset;
    }
    return iOffset;
  }

  private void ParseCRN(CRNRecord crn)
  {
    int num1 = (int) crn.Row + 1;
    int num2 = (int) crn.FirstColumn + 1;
    int index = 0;
    while (num2 <= (int) crn.LastColumn + 1)
    {
      switch (crn.Values[index])
      {
        case string strValue:
          this.m_dicRecordsCells.SetNonSSTString(num1, num2, 0, strValue);
          break;
        case double dValue:
          this.m_dicRecordsCells.SetNumberValue(num1, num2, dValue, 0);
          break;
        case bool bValue:
          this.m_dicRecordsCells.SetBooleanValue(num1, num2, bValue, 0);
          break;
        case byte errorCode:
          this.m_dicRecordsCells.SetErrorValue(num1, num2, errorCode, 0);
          break;
      }
      ++num2;
      ++index;
    }
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    records.Add((IBiffStorage) this.m_xct);
    this.SerializeRows(records);
  }

  private void SerializeRows(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_iFirstRow < 0)
      return;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      this.SerializeRow(iFirstRow, records);
  }

  private void SerializeRow(int i, OffsetArrayList records)
  {
    RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, i - 1, false);
    if (row == null)
      return;
    IEnumerator enumerator = row.GetEnumerator(this.m_dicRecordsCells.RecordExtractor);
    CRNRecord record = (CRNRecord) BiffRecordFactory.GetRecord(TBIFFRecord.CRN);
    record.Row = (ushort) (i - 1);
    int num = -1;
    List<object> values = record.Values;
    while (enumerator.MoveNext())
    {
      BiffRecordRaw current = (BiffRecordRaw) enumerator.Current;
      object obj = ((IValueHolder) current).Value;
      int column = (current as ICellPositionFormat).Column;
      if (num < 0)
        record.FirstColumn = (byte) column;
      else if (num + 1 != column)
      {
        records.Add((IBiffStorage) record);
        record = (CRNRecord) BiffRecordFactory.GetRecord(TBIFFRecord.CRN);
        record.Row = (ushort) (i - 1);
        record.FirstColumn = (byte) column;
        values = record.Values;
      }
      values.Add(obj);
      record.LastColumn = (byte) column;
      num = column;
    }
    if (values.Count <= 0)
      return;
    records.Add((IBiffStorage) record);
  }

  public int Index
  {
    get => (int) this.m_xct.SheetTableIndex;
    set => this.m_xct.SheetTableIndex = (ushort) value;
  }

  public ExternWorkbookImpl Workbook => this.m_book;

  public int ReferenceIndex
  {
    get => this.m_book.Workbook.AddSheetReference(this.m_book.Index, this.Index, this.Index);
  }

  public Dictionary<string, string> AdditionalAttributes
  {
    get => this.m_dicAdditionalAttributes;
    set => this.m_dicAdditionalAttributes = value;
  }

  public ExternWorksheetImpl Clone(object parent)
  {
    ExternWorksheetImpl parent1 = (ExternWorksheetImpl) this.MemberwiseClone();
    this.m_xct = (XCTRecord) CloneUtils.CloneCloneable((ICloneable) this.m_xct);
    parent1.SetParent(parent);
    parent1.m_book = (ExternWorkbookImpl) parent1.FindParent(typeof (ExternWorkbookImpl));
    parent1.m_dicRecordsCells = this.m_dicRecordsCells.Clone((object) parent1);
    this.m_arrRecords = CloneUtils.CloneCloneable(this.m_arrRecords);
    return parent1;
  }

  protected override void OnDispose()
  {
    if (this.m_bIsDisposed)
      return;
    if (this.m_dicRecordsCells != null)
    {
      this.m_dicRecordsCells.Dispose();
      this.m_dicRecordsCells = (CellRecordCollection) null;
    }
    base.OnDispose();
  }

  internal void CacheValues(IRange sourceRange)
  {
    int row = sourceRange.Row;
    for (int lastRow = sourceRange.LastRow; row <= lastRow; ++row)
    {
      int column = sourceRange.Column;
      for (int lastColumn = sourceRange.LastColumn; column <= lastColumn; ++column)
      {
        IRange range = sourceRange[row, column];
        if (range.HasBoolean)
          this.m_dicRecordsCells.SetBooleanValue(row, column, range.Boolean, 0);
        else if (range.HasDateTime || range.HasNumber)
          this.m_dicRecordsCells.SetNumberValue(row, column, range.Number, 0);
        else if (range.HasString)
          this.m_dicRecordsCells.SetNonSSTString(row, column, 0, range.Text);
        else if (range.IsError)
          this.m_dicRecordsCells.SetErrorValue(row, column, range.Error);
      }
    }
  }

  public object GetValueRowCol(int row, int col)
  {
    IRange range = this[row, col];
    return range.HasFormula ? (object) range.Formula : (object) range.Value;
  }

  public void SetValueRowCol(object value, int row, int col)
  {
    if (value == null)
      return;
    this.SetValue(row, col, value.ToString());
  }

  public void WireParentObject()
  {
  }

  public IRange[] Cells => throw new Exception("The method or operation is not implemented.");

  public bool DisplayPageBreaks
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public OfficeSheetProtection Protection
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public bool ProtectContents => throw new Exception("The method or operation is not implemented.");

  public OfficeSheetView View
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool ProtectDrawingObjects
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public bool ProtectScenarios
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public bool HasOleObject
  {
    set => throw new NotSupportedException();
    get => throw new NotSupportedException();
  }

  public IRange[] MergedCells => throw new Exception("The method or operation is not implemented.");

  public INames Names => throw new Exception("The method or operation is not implemented.");

  public string CodeName => throw new Exception("The method or operation is not implemented.");

  public IPageSetup PageSetup => throw new Exception("The method or operation is not implemented.");

  public IRange Range => throw new Exception("The method or operation is not implemented.");

  public IRange[] Rows => throw new Exception("The method or operation is not implemented.");

  public IRange[] Columns => throw new Exception("The method or operation is not implemented.");

  public double StandardHeight
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool StandardHeightFlag
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public double StandardWidth
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public OfficeSheetType Type => throw new Exception("The method or operation is not implemented.");

  public IRange UsedRange => throw new Exception("The method or operation is not implemented.");

  public int Zoom
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public int VerticalSplit
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public int HorizontalSplit
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public int FirstVisibleRow
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public int FirstVisibleColumn
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public int ActivePane
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool IsDisplayZeros
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool IsGridLinesVisible
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public OfficeKnownColors GridLineColor
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool IsRowColumnHeadersVisible
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool IsStringsPreserved
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool IsPasswordProtected
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public IRange this[int row, int column] => (IRange) null;

  public IRange this[int row, int column, int lastRow, int lastColumn] => (IRange) null;

  public IRange this[string name]
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public IRange this[string name, bool IsR1C1Notation]
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] UsedCells => throw new Exception("The method or operation is not implemented.");

  public IWorksheetCustomProperties CustomProperties
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public bool UseRangesCache
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool IsFreezePanes => throw new Exception("The method or operation is not implemented.");

  public IRange SplitCell => throw new Exception("The method or operation is not implemented.");

  public int TopVisibleRow
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public int LeftVisibleColumn
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool UsedRangeIncludesFormatting
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public void CopyToClipboard()
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Clear() => throw new Exception("The method or operation is not implemented.");

  public void ClearData() => throw new Exception("The method or operation is not implemented.");

  public bool Contains(int iRow, int iColumn)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRanges CreateRangesCollection()
  {
    return (IRanges) this.AppImplementation.CreateRangesCollection((object) this);
  }

  public void CreateNamedRanges(string namedRange, string referRange, bool vertical)
  {
    throw new NotImplementedException();
  }

  public bool IsColumnVisible(int columnIndex)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void ShowColumn(int columnIndex, bool isVisible)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void HideColumn(int columnIndex) => this.ShowColumn(columnIndex, false);

  public void HideRow(int rowIndex) => this.ShowRow(rowIndex, false);

  public bool IsRowVisible(int rowIndex)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void ShowRow(int rowIndex, bool isVisible)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void ShowRange(IRange range, bool isVisible)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void ShowRange(RangesCollection ranges, bool isVisible)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void ShowRange(IRange[] ranges, bool isVisible)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void InsertRow(int index)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void InsertRow(int iRowIndex, int iRowCount)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void InsertRow(int iRowIndex, int iRowCount, OfficeInsertOptions insertOptions)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void InsertColumn(int index)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void InsertColumn(int iColumnIndex, int iColumnCount)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void InsertColumn(int iColumnIndex, int iColumnCount, OfficeInsertOptions insertOptions)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void DeleteRow(int index)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void DeleteRow(int index, int count)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void DeleteColumn(int index)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void DeleteColumn(int index, int count)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportArray(object[] arrObject, int firstRow, int firstColumn, bool isVertical)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportArray(string[] arrString, int firstRow, int firstColumn, bool isVertical)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportArray(int[] arrInt, int firstRow, int firstColumn, bool isVertical)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportArray(double[] arrDouble, int firstRow, int firstColumn, bool isVertical)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportArray(DateTime[] arrDateTime, int firstRow, int firstColumn, bool isVertical)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportArray(object[,] arrObject, int firstRow, int firstColumn)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportData(IEnumerable arrObject, int firstRow, int firstColumn, bool includeHeader)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataColumn(
    DataColumn dataColumn,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataTable(
    DataTable dataTable,
    int firstRow,
    int firstColumn,
    bool importOnSave)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    bool preserveTypes)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    bool preserveTypes)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataTable(DataTable dataTable, IName namedRange, bool isFieldNameShown)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataTable(
    DataTable dataTable,
    IName namedRange,
    bool isFieldNameShown,
    int rowOffset,
    int columnOffset)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataTable(
    DataTable dataTable,
    IName namedRange,
    bool isFieldNameShown,
    int rowOffset,
    int columnOffset,
    int iMaxRow,
    int iMaxCol)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataTable(
    DataTable dataTable,
    IName namedRange,
    bool isFieldNameShown,
    int rowOffset,
    int columnOffset,
    int iMaxRow,
    int iMaxCol,
    bool bPreserveTypes)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    bool bPreserveTypes)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    bool bPreserveTypes)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void RemovePanes() => throw new Exception("The method or operation is not implemented.");

  public DataTable ExportDataTable(
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    ExcelExportDataTableOptions options)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public DataTable ExportDataTable(IRange dataRange, ExcelExportDataTableOptions options)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Protect(string password)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Protect(string password, OfficeSheetProtection options)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Unprotect(string password)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange IntersectRanges(IRange range1, IRange range2)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange MergeRanges(IRange range1, IRange range2)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void AutofitRow(int rowIndex)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void AutofitColumn(int colIndex)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Replace(string oldValue, string newValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Replace(string oldValue, double newValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Replace(string oldValue, DateTime newValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Replace(string oldValue, string[] newValues, bool isVertical)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Replace(string oldValue, int[] newValues, bool isVertical)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Replace(string oldValue, double[] newValues, bool isVertical)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Replace(string oldValue, DataTable newValues, bool isFieldNamesShown)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Replace(string oldValue, DataColumn newValues, bool isFieldNamesShown)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Remove() => throw new Exception("The method or operation is not implemented.");

  public void Move(int iNewIndex)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ColumnWidthToPixels(double widthInChars)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public double PixelsToColumnWidth(int pixels)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetColumnWidth(int iColumnIndex, double value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetColumnWidthInPixels(int iColumnIndex, int value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetColumnWidthInPixels(int iStartColumnIndex, int iCount, int value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetRowHeight(int iRow, double value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetRowHeightInPixels(int iRowIndex, double value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetRowHeightInPixels(int iStartRowIndex, int iCount, double value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public double GetColumnWidth(int iColumnIndex)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int GetColumnWidthInPixels(int iColumnIndex)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public double GetRowHeight(int iRow)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int GetRowHeightInPixels(int iRowIndex)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindFirst(string findValue, OfficeFindType flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindFirst(string findValue, OfficeFindType flags, OfficeFindOptions findOptions)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindFirst(double findValue, OfficeFindType flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindFirst(bool findValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindFirst(DateTime findValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindFirst(TimeSpan findValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindStringStartsWith(string findValue, OfficeFindType flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindStringStartsWith(string findValue, OfficeFindType flags, bool ignoreCase)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindStringEndsWith(string findValue, OfficeFindType flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindStringEndsWith(string findValue, OfficeFindType flags, bool ignoreCase)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] FindAll(string findValue, OfficeFindType flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] FindAll(string findValue, OfficeFindType flags, OfficeFindOptions findOptions)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] FindAll(double findValue, OfficeFindType flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] FindAll(bool findValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] FindAll(DateTime findValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] FindAll(TimeSpan findValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SaveAs(string fileName, string separator)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SaveAs(string fileName, string separator, Encoding encoding)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SaveAs(Stream stream, string separator)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SaveAs(Stream stream, string separator, Encoding encoding)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetDefaultColumnStyle(int iColumnIndex, IStyle defaultStyle)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetDefaultColumnStyle(
    int iStartColumnIndex,
    int iEndColumnIndex,
    IStyle defaultStyle)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetDefaultRowStyle(int iRowIndex, IStyle defaultStyle)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetDefaultRowStyle(int iStartRowIndex, int iEndRowIndex, IStyle defaultStyle)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IStyle GetDefaultColumnStyle(int iColumnIndex)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IStyle GetDefaultRowStyle(int iRowIndex)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void FreeRange(IRange range)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void FreeRange(int iRow, int iColumn)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetValue(int iRow, int iColumn, string value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetNumber(int iRow, int iColumn, double value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetBoolean(int iRow, int iColumn, bool value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetText(int iRow, int iColumn, string value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetFormula(int iRow, int iColumn, string value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetError(int iRow, int iColumn, string value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetBlank(int iRow, int iColumn)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetFormulaNumberValue(int iRow, int iColumn, double value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetFormulaErrorValue(int iRow, int iColumn, string value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetFormulaBoolValue(int iRow, int iColumn, bool value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void SetFormulaStringValue(int iRow, int iColumn, string value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IMigrantRange MigrantRange
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public string GetText(int row, int column)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public double GetNumber(int row, int column)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public string GetFormula(int row, int column, bool bR1C1)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public string GetError(int row, int column)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public bool GetBoolean(int row, int column)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public bool GetFormulaBoolValue(int row, int column)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public string GetFormulaErrorValue(int row, int column)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public double GetFormulaNumberValue(int row, int column)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public string GetFormulaStringValue(int row, int column)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public Image ConvertToImage(int firstRow, int firstColumn, int lastRow, int lastColumn)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public Image ConvertToImage(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ImageType imageType,
    Stream stream)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public OfficeKnownColors TabColor
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public Color TabColorRGB
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public IOfficeChartShapes Charts
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  IWorkbook ITabSheet.Workbook => (IWorkbook) this.m_book.Workbook;

  public IShapes Shapes => throw new Exception("The method or operation is not implemented.");

  public bool IsRightToLeft
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool IsSelected => throw new Exception("The method or operation is not implemented.");

  public int TabIndex => throw new Exception("The method or operation is not implemented.");

  public string Name
  {
    get => this.m_strName;
    set => this.m_strName = value;
  }

  public OfficeWorksheetVisibility Visibility
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public ITextBoxes TextBoxes => throw new Exception("The method or operation is not implemented.");

  public void Activate() => throw new Exception("The method or operation is not implemented.");

  public void Select() => throw new Exception("The method or operation is not implemented.");

  public void Unselect() => throw new Exception("The method or operation is not implemented.");

  public int DefaultRowHeight => 0;

  public int FirstRow
  {
    get => this.m_iFirstRow;
    set => this.m_iFirstRow = value;
  }

  public int FirstColumn
  {
    get => this.m_iFirstColumn;
    set => this.m_iFirstColumn = value;
  }

  public int LastRow
  {
    get => this.m_iLastRow;
    set => this.m_iLastRow = value;
  }

  public int LastColumn
  {
    get => this.m_iLastColumn;
    set => this.m_iLastColumn = value;
  }

  public CellRecordCollection CellRecords
  {
    [DebuggerStepThrough] get => this.m_dicRecordsCells;
  }

  public WorkbookImpl ParentWorkbook => this.m_book.Workbook;

  public bool IsArrayFormula(long index) => false;

  public OfficeVersion Version => OfficeVersion.Excel2007;

  public IInternalWorksheet GetClonedObject(
    Dictionary<string, string> hashNewNames,
    WorkbookImpl book)
  {
    int index1 = this.m_book.Index;
    int index2 = this.Index;
    return (IInternalWorksheet) book.ExternWorkbooks[index1].Worksheets[index2];
  }

  object ICloneParent.Clone(object parent) => (object) this.Clone(parent);
}
