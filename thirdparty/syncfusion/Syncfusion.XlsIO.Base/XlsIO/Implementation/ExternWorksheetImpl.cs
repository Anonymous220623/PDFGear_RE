// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ExternWorksheetImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Calculate;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.PivotTables;
using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Forms;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ExternWorksheetImpl : 
  CommonObject,
  IInternalWorksheet,
  IWorksheet,
  ITabSheet,
  IParentApplication,
  ICalcData,
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
  private CalcEngine m_calcEngine;

  public event RangeImpl.CellValueChangedEventHandler CellValueChanged;

  public event WorksheetImpl.ExportDataTableEventHandler ExportDataTableEvent;

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
  public void Parse(BiffReader reader, IDecryptor decryptor)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.PeekRecordType() != TBIFFRecord.XCT)
      return;
    this.m_xct = (XCTRecord) reader.GetRecord(decryptor);
    this.m_arrRecords.Clear();
    for (TBIFFRecord tbiffRecord = reader.PeekRecordType(); tbiffRecord == TBIFFRecord.CRN; tbiffRecord = reader.PeekRecordType())
    {
      BiffRecordRaw record = reader.GetRecord(decryptor);
      record.CheckTypeCode(TBIFFRecord.CRN);
      this.ParseCRN((CRNRecord) record);
      this.m_arrRecords.Add(record);
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
    parent1.m_xct = (XCTRecord) CloneUtils.CloneCloneable((ICloneable) this.m_xct);
    parent1.SetParent(parent);
    parent1.m_book = (ExternWorkbookImpl) parent1.FindParent(typeof (ExternWorkbookImpl));
    parent1.m_arrRecords = CloneUtils.CloneCloneable(this.m_arrRecords);
    parent1.m_dicRecordsCells = this.m_dicRecordsCells.Clone((object) parent1);
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
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(sourceRange.Worksheet.Application, sourceRange.Worksheet);
    int row = sourceRange.Row;
    for (int lastRow = sourceRange.LastRow; row <= lastRow; ++row)
    {
      int column = sourceRange.Column;
      for (int lastColumn = sourceRange.LastColumn; column <= lastColumn; ++column)
      {
        migrantRangeImpl.ResetRowColumn(row, column);
        if (migrantRangeImpl.HasBoolean)
          this.m_dicRecordsCells.SetBooleanValue(row, column, migrantRangeImpl.Boolean, 0);
        else if (migrantRangeImpl.HasDateTime || migrantRangeImpl.HasNumber)
          this.m_dicRecordsCells.SetNumberValue(row, column, migrantRangeImpl.Number, 0);
        else if (migrantRangeImpl.HasString)
          this.m_dicRecordsCells.SetNonSSTString(row, column, 0, migrantRangeImpl.Text);
        else if (migrantRangeImpl.IsError)
          this.m_dicRecordsCells.SetErrorValue(row, column, migrantRangeImpl.Error);
      }
    }
  }

  internal void SetCellRecords(CellRecordCollection cellRecords)
  {
    this.m_dicRecordsCells.Dispose();
    this.m_dicRecordsCells = cellRecords;
  }

  public CalcEngine CalcEngine
  {
    get => this.m_calcEngine;
    set => this.m_calcEngine = value;
  }

  public void EnableSheetCalculations()
  {
    if (this.CalcEngine != null)
      return;
    this.CalcEngine = new CalcEngine((ICalcData) this);
    this.CalcEngine.PreserveFormula = true;
    int sheetFamilyId = CalcEngine.CreateSheetFamilyID();
    string str = "!";
    foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) this.ParentWorkbook.Worksheets)
    {
      if (worksheet.CalcEngine == null)
        worksheet.CalcEngine = new CalcEngine((ICalcData) worksheet);
      this.CalcEngine.RegisterGridAsSheet(worksheet.Name, (ICalcData) worksheet, sheetFamilyId);
      worksheet.CalcEngine.UnknownFunction += new UnknownFunctionEventHandler(this.CalcEngine_UnknownFunction);
      str = $"{str}{worksheet.Name}!";
    }
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    foreach (IName name in (IEnumerable) this.ParentWorkbook.Names)
    {
      if (name.Scope.Length > 0 && str.IndexOf($"!{name.Scope}!") > -1)
        dictionary.Add($"{name.Scope}!{name.Name}".ToUpper(), name.Value.Replace("'", ""));
      else
        dictionary.Add(name.Name.ToUpper(), name.Value.Replace("'", ""));
    }
    Hashtable hashtable = new Hashtable();
    if (dictionary != null)
    {
      foreach (string key in dictionary.Keys)
        hashtable.Add((object) key.ToUpper(CultureInfo.InvariantCulture), (object) dictionary[key]);
    }
    foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) this.ParentWorkbook.Worksheets)
      worksheet.CalcEngine.NamedRanges = hashtable;
  }

  public void DisableSheetCalculations()
  {
    if (this.CalcEngine == null || this.ParentWorkbook == null || this.ParentWorkbook.Worksheets == null)
      return;
    foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) this.ParentWorkbook.Worksheets)
    {
      if (worksheet.CalcEngine != null)
      {
        worksheet.CalcEngine.UnknownFunction -= new UnknownFunctionEventHandler(this.CalcEngine_UnknownFunction);
        worksheet.CalcEngine.Dispose();
      }
      worksheet.CalcEngine = (CalcEngine) null;
    }
  }

  private void CalcEngine_UnknownFunction(object sender, UnknownFunctionEventArgs args)
  {
    if (this.MissingFunction == null || this.CalcEngine == null)
      return;
    this.MissingFunction((object) this, new MissingFunctionEventArgs()
    {
      MissingFunctionName = args.MissingFunctionName,
      CellLocation = args.CellLocation
    });
  }

  public object GetValueRowCol(int row, int col)
  {
    return this.GetCellType(row, col, false) == WorksheetImpl.TRangeValueType.Number ? (object) this.GetNumber(row, col) : (object) this.GetText(row, col);
  }

  public void SetValueRowCol(object value, int row, int col)
  {
    if (value == null)
      return;
    this.SetValue(row, col, value.ToString());
  }

  private WorksheetImpl.TRangeValueType GetCellType(int row, int column, bool bNeedFormulaSubType)
  {
    return this.m_dicRecordsCells != null && this.m_dicRecordsCells.Table != null ? this.m_dicRecordsCells.Table.GetCellType(row, column, bNeedFormulaSubType) : WorksheetImpl.TRangeValueType.Error;
  }

  public void WireParentObject()
  {
  }

  public event Syncfusion.Calculate.ValueChangedEventHandler ValueChanged;

  public void OnValueChanged(int row, int col, string value)
  {
    if (this.ValueChanged == null)
      return;
    this.ValueChanged((object) this, new Syncfusion.Calculate.ValueChangedEventArgs(row, col, value));
  }

  public IAutoFilters AutoFilters
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public void SaveAsHtml(string filename)
  {
  }

  public void SaveAsHtml(Stream stream)
  {
  }

  public void SaveAsHtml(string filename, HtmlSaveOptions saveOptions)
  {
  }

  private void SaveAsHtml(Stream stream, HtmlSaveOptions saveOptions)
  {
  }

  public IRange[] Cells => throw new Exception("The method or operation is not implemented.");

  public bool DisplayPageBreaks
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public ISparklineGroups SparklineGroups
  {
    get => throw new NotSupportedException("This method or operation is not implemented");
  }

  public ExcelSheetProtection Protection
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public bool ProtectContents => throw new Exception("The method or operation is not implemented.");

  public SheetView View
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

  public ExcelSheetType Type => throw new Exception("The method or operation is not implemented.");

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

  public IOleObjects OleObjects
  {
    get => throw new NotImplementedException("The method or operation is not implemented.");
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

  public ExcelKnownColors GridLineColor
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool IsRowColumnHeadersVisible
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public IVPageBreaks VPageBreaks
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public IHPageBreaks HPageBreaks
  {
    get => throw new Exception("The method or operation is not implemented.");
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

  public IComments Comments => throw new Exception("The method or operation is not implemented.");

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

  public IHyperLinks HyperLinks
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

  public IPivotTables PivotTables
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public IListObjects ListObjects
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public IDataSort DataSorter => throw new Exception("The method or operation is not implemented.");

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

  public ITemplateMarkersProcessor CreateTemplateMarkersProcessor()
  {
    throw new Exception("The method or operation is not implemented.");
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

  public void InsertRow(int iRowIndex, int iRowCount, ExcelInsertOptions insertOptions)
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

  public void InsertColumn(int iColumnIndex, int iColumnCount, ExcelInsertOptions insertOptions)
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

  public int ImportData(IEnumerable arrObject, ExcelImportDataOptions importDataOptions)
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

  public int ImportDataColumn(
    DataColumn dataColumn,
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
    int firstRow,
    int firstColumn,
    bool importOnSave,
    bool includeHeader)
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

  public int ImportDataReader(
    IDataReader dataReader,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataReader(
    IDataReader dataReader,
    int firstRow,
    int firstColumn,
    bool importOnSave)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataReader(
    IDataReader dataReader,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    bool preserveTypes)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int ImportDataReader(IDataReader dataReader, IName namedRange, bool isFieldNameShown)
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

  public void ImportDataGrid(
    System.Windows.Forms.DataGrid dataGrid,
    int firstRow,
    int firstColumn,
    bool isImportHeader,
    bool isImportStyle)
  {
    throw new NotImplementedException();
  }

  public void ImportDataGrid(
    System.Web.UI.WebControls.DataGrid dataGrid,
    int firstRow,
    int firstColumn,
    bool isImportHeader,
    bool isImportStyle)
  {
    throw new NotImplementedException();
  }

  public void ImportGridView(
    GridView gridView,
    int firstRow,
    int firstColumn,
    bool isImportHeader,
    bool isImportStyle)
  {
    throw new NotImplementedException();
  }

  public void ImportDataGridView(
    DataGridView dataGridView,
    int firstRow,
    int firstColumn,
    bool isImportHeader,
    bool isImportStyle)
  {
    throw new NotImplementedException();
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

  public DataTable PEExportDataTable(
    IRange dataRange,
    ExcelExportDataTableOptions options,
    PivotTableImpl pivotTable)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public List<T> ExportData<T>(int firstRow, int firstColumn, int lastRow, int lastColumn) where T : new()
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public List<T> ExportData<T>(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    Dictionary<string, string> mappingProperties)
    where T : new()
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Protect(string password)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Protect(string password, ExcelSheetProtection options)
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

  public void Replace(string oldValue, string newValue, ExcelFindOptions findOptions)
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

  public IRange FindFirst(string findValue, ExcelFindType flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindFirst(string findValue, ExcelFindType flags, ExcelFindOptions findOptions)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindFirst(double findValue, ExcelFindType flags)
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

  public IRange FindStringStartsWith(string findValue, ExcelFindType flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindStringStartsWith(string findValue, ExcelFindType flags, bool ignoreCase)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindStringEndsWith(string findValue, ExcelFindType flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindStringEndsWith(string findValue, ExcelFindType flags, bool ignoreCase)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] FindAll(string findValue, ExcelFindType flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] FindAll(string findValue, ExcelFindType flags, ExcelFindOptions findOptions)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] FindAll(double findValue, ExcelFindType flags)
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
    return this.m_dicRecordsCells.GetText(RangeImpl.GetCellIndex(column, row));
  }

  public double GetNumber(int row, int column)
  {
    return this.m_dicRecordsCells.Table.GetNumberValue(row, column);
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

  public System.Drawing.Image ConvertToImage(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public System.Drawing.Image ConvertToImage(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ImageType imageType,
    Stream stream)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public System.Drawing.Image ConvertToImage(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    EmfType emfType,
    Stream outputStream)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public System.Drawing.Image ConvertToImage(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ImageType imageType,
    Stream outputStream,
    EmfType emfType)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void AdvancedFilter(
    ExcelFilterAction filterInPlace,
    IRange filterRange,
    IRange criteriaRange,
    IRange copyToRange,
    bool isUnique)
  {
    throw new NotImplementedException();
  }

  public void Calculate() => throw new NotImplementedException();

  public ExcelKnownColors TabColor
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public Color TabColorRGB
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public IChartShapes Charts => throw new Exception("The method or operation is not implemented.");

  public IPictures Pictures => throw new Exception("The method or operation is not implemented.");

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

  public WorksheetVisibility Visibility
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public ITextBoxes TextBoxes => throw new Exception("The method or operation is not implemented.");

  public ICheckBoxes CheckBoxes
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public IOptionButtons OptionButtons
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public IComboBoxes ComboBoxes
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

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

  public ExcelVersion Version => ExcelVersion.Excel2007;

  public IInternalWorksheet GetClonedObject(
    Dictionary<string, string> hashNewNames,
    WorkbookImpl book)
  {
    int index1 = this.m_book.Index;
    int index2 = this.Index;
    return (IInternalWorksheet) book.ExternWorkbooks[index1].Worksheets[index2];
  }

  object ICloneParent.Clone(object parent) => (object) this.Clone(parent);

  void IWorksheet.SaveAsHtml(Stream stream, HtmlSaveOptions saveOptions)
  {
    throw new NotImplementedException();
  }

  public void ImportHtmlTable(string fileName, int row, int column)
  {
    throw new NotImplementedException();
  }

  public void ImportHtmlTable(Stream fileStream, int row, int column)
  {
    throw new NotImplementedException();
  }
}
