// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.WorksheetImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.Exceptions;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlReaders;
using Syncfusion.OfficeChart.Implementation.XmlSerialization;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class WorksheetImpl(IApplication application, object parent) : 
  WorksheetBaseImpl(application, parent),
  ISerializableNamedObject,
  INamedObject,
  IParseable,
  ICloneParent,
  IInternalWorksheet,
  IWorksheet,
  ITabSheet,
  IParentApplication
{
  internal const char DEF_STANDARD_CHAR = '0';
  private const float DEF_AXE_IN_RADIANS = 0.0174532924f;
  private const int DEF_MAX_COLUMN_WIDTH = 255 /*0xFF*/;
  private const double DEF_ZERO_CHAR_WIDTH = 8.0;
  private const int DEF_ARRAY_SIZE = 100;
  private const int DEF_AUTO_FILTER_WIDTH = 16 /*0x10*/;
  private const int DEF_INDENT_WIDTH = 12;
  private const double DEF_OLE_DOUBLE = 2958465.9999999884;
  private const double DEF_MAX_DOUBLE = 2958466.0;
  private const char CarriageReturn = '\r';
  private const char NewLine = '\n';
  private const string MSExcel = "Microsoft Excel";
  private const int DEFAULT_DATE_NUMBER_FORMAT_INDEX = 14;
  internal bool m_hasSheetCalculation;
  private bool m_hasAlternateContent;
  internal int unknown_formula_name = 9;
  private static readonly TBIFFRecord[] s_arrAutofilterRecord = new TBIFFRecord[3]
  {
    TBIFFRecord.AutoFilter,
    TBIFFRecord.AutoFilterInfo,
    TBIFFRecord.FilterMode
  };
  private Dictionary<int, int> m_indexAndLevels;
  private bool m_bParseDataOnDemand;
  private RangeImpl m_rngUsed;
  private CellRecordCollection m_dicRecordsCells;
  private ColumnInfoRecord[] m_arrColumnInfo;
  private bool m_bDisplayPageBreaks;
  private PageSetupImpl m_pageSetup;
  private double m_dStandardColWidth = 8.43;
  private MergeCellsImpl m_mergedCells;
  private List<SelectionRecord> m_arrSelections;
  private PaneRecord m_pane;
  private WorksheetNamesCollection m_names;
  private OfficeSheetType m_sheetType;
  private bool m_bStringsPreserved;
  private List<BiffRecordRaw> m_arrAutoFilter;
  private SortedList<int, NoteRecord> m_arrNotes;
  private SortedList<long, NoteRecord> m_arrNotesByCellIndex;
  private NameImpl.NameIndexChangedEventHandler m_nameIndexChanged;
  private List<BiffRecordRaw> m_arrSortRecords;
  private int m_iPivotStartIndex = -1;
  private int m_iHyperlinksStartIndex = -1;
  private int m_iCondFmtPos = -1;
  private int m_iDValPos = -1;
  private int m_iCustomPropertyStartIndex = -1;
  private List<BiffRecordRaw> m_arrDConRecords;
  private IMigrantRange m_migrantRange;
  private IndexRecord m_index;
  private bool m_bUsedRangeIncludesFormatting = true;
  private bool m_busedRangeIncludesCF;
  private RangeTrueFalse m_stringPreservedRanges = new RangeTrueFalse();
  private ItemSizeHelper m_rowHeightHelper;
  private List<BiffRecordRaw> m_tableRecords;
  private bool m_isRowHeightSet;
  private bool m_isZeroHeight;
  private int m_baseColumnWidth;
  private bool m_isThickBottom;
  private bool m_isThickTop;
  private byte m_outlineLevelColumn;
  private double m_defaultColWidth = 8.43;
  private byte m_outlineLevelRow;
  private ColumnInfoRecord m_rawColRecord;
  private bool m_bOptimizeImport;
  private OfficeSheetView m_view;
  internal List<Stream> preservedStreams;
  internal Dictionary<int, CondFMTRecord> m_dictCondFMT = new Dictionary<int, CondFMTRecord>();
  internal Dictionary<int, CFExRecord> m_dictCFExRecords = new Dictionary<int, CFExRecord>();
  private AutoFitManager m_autoFitManager;
  internal List<IOutlineWrapper> m_outlineWrappers;
  private Dictionary<int, List<Point>> m_columnOutlineLevels;
  private Dictionary<int, List<Point>> m_rowOutlineLevels;
  private Dictionary<string, string> m_inlineStrings;
  private List<BiffRecordRaw> m_preserveExternalConnection;
  private List<Stream> m_preservePivotTables;
  internal Stream m_worksheetSlicer;
  internal string m_formulaString;
  private bool m_bIsExportDataTable;
  private ColumnCollection columnCollection;
  private ImportDTHelper m_importDTHelper;
  private bool m_bIsImporting;
  private ExtendedFormatImpl format;
  private int dateTimeStyleIndex;
  private DataTable m_dataTable;
  internal bool m_parseCondtionalFormats = true;
  internal bool m_parseCF = true;
  private string m_archiveItemName;

  public event MissingFunctionEventHandler MissingFunction;

  public event RangeImpl.CellValueChangedEventHandler CellValueChanged;

  internal bool HasSheetCalculation => this.m_hasSheetCalculation;

  internal bool HasAlternateContent
  {
    get => this.m_hasAlternateContent;
    set => this.m_hasAlternateContent = value;
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

  protected override void InitializeCollections()
  {
    base.InitializeCollections();
    this.m_nameIndexChanged = new NameImpl.NameIndexChangedEventHandler(this.OnNameIndexChanged);
    this.m_dicRecordsCells = new CellRecordCollection(this.Application, (object) this);
    this.m_pageSetup = new PageSetupImpl(this.Application, (object) this);
    if (this.Application.DefaultVersion != OfficeVersion.Excel97to2003)
      this.m_pageSetup.DefaultRowHeight = (int) this.Application.StandardHeight * 20;
    this.m_names = new WorksheetNamesCollection(this.Application, (object) this);
    this.m_arrColumnInfo = new ColumnInfoRecord[this.m_book.MaxColumnCount + 2];
    this.m_bOptimizeImport = this.Application.OptimizeImport;
    this.Index = this.m_book.Worksheets.Count;
    this.m_arrSelections = new List<SelectionRecord>();
    this.StandardWidth = this.Application.StandardWidth;
    this.StandardHeight = this.Application.StandardHeight;
    this.StandardHeightFlag = this.Application.StandardHeightFlag;
    this.AttachEvents();
  }

  protected void ClearAll() => this.ClearAll(OfficeWorksheetCopyFlags.CopyAll);

  protected override void ClearAll(OfficeWorksheetCopyFlags flags)
  {
    this.m_dicRecordsCells.Clear();
    this.m_arrSelections.Clear();
    if ((flags & OfficeWorksheetCopyFlags.CopyNames) != OfficeWorksheetCopyFlags.None)
      this.m_names.Clear();
    base.ClearAll(flags);
  }

  protected void CopyNames(
    WorksheetImpl basedOn,
    Dictionary<string, string> hashNewSheetNames,
    Dictionary<int, int> hashNewNameIndexes,
    Dictionary<int, int> hashExternSheetIndexes)
  {
    if (basedOn == null)
      throw new ArgumentNullException(nameof (basedOn));
    for (int index = this.m_names.Count - 1; index >= 0; --index)
      this.m_names[index].Delete();
    if (hashNewSheetNames == null)
    {
      hashNewSheetNames = new Dictionary<string, string>();
      hashNewSheetNames.Add(this.Name, basedOn.Name);
    }
    this.m_names.FillFrom(basedOn.m_names, (IDictionary) hashNewSheetNames, hashNewNameIndexes, ExcelNamesMergeOptions.MakeLocal, hashExternSheetIndexes);
    WorkbookNamesCollection names1 = basedOn.Workbook.Names as WorkbookNamesCollection;
    WorkbookNamesCollection names2 = this.Workbook.Names as WorkbookNamesCollection;
    int index1 = 0;
    for (int count = names1.Count; index1 < count; ++index1)
    {
      NameImpl inner = (NameImpl) names1.InnerList[index1];
      if (!inner.IsLocal)
      {
        IRange refersToRange = inner.RefersToRange;
        int index2 = inner.Index;
        if (refersToRange == null)
        {
          if (!names2.Contains(inner.Name))
          {
            NameRecord name1 = (NameRecord) inner.Record.Clone();
            WorksheetNamesCollection.UpdateReferenceIndexes(name1, inner.Workbook, (IDictionary) hashNewSheetNames, hashExternSheetIndexes, this.m_book);
            IName name2 = names2.Add(name1);
            hashNewNameIndexes[index2] = (name2 as NameImpl).Index;
          }
        }
        else if (refersToRange.Worksheet == basedOn)
        {
          try
          {
            IName name = names2.AddCopy((IName) inner, (IWorksheet) this, hashExternSheetIndexes, (IDictionary) hashNewSheetNames);
            hashNewNameIndexes[index2] = (name as NameImpl).Index;
          }
          catch (Exception ex)
          {
            if (!names2.Contains(inner.Name))
            {
              NameRecord name3 = (NameRecord) inner.Record.Clone();
              WorksheetNamesCollection.UpdateReferenceIndexes(name3, inner.Workbook, (IDictionary) hashNewSheetNames, hashExternSheetIndexes, this.m_book);
              IName name4 = names2.Add(name3);
              hashNewNameIndexes[index2] = (name4 as NameImpl).Index;
            }
          }
        }
      }
    }
    foreach (int key in basedOn.GetUsedNames().Keys)
    {
      if (!hashNewNameIndexes.ContainsKey(key))
      {
        NameImpl nameToCopy = (NameImpl) names1[key];
        IName name = names2.AddCopy((IName) nameToCopy, (IWorksheet) this, hashExternSheetIndexes, (IDictionary) hashNewSheetNames);
        hashNewNameIndexes[key] = (name as NameImpl).Index;
      }
    }
  }

  private Dictionary<int, object> GetUsedNames()
  {
    ArrayListEx rows = this.m_dicRecordsCells.Table.Rows;
    Dictionary<int, object> result = new Dictionary<int, object>();
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      rows[iFirstRow]?.GetUsedNames(result);
    return result;
  }

  protected void CopyRowHeight(WorksheetImpl sourceSheet, Dictionary<int, int> hashExtFormatIndexes)
  {
    if (sourceSheet == null)
      throw new ArgumentNullException(nameof (sourceSheet));
  }

  protected void CopyConditionalFormats(WorksheetImpl sourceSheet)
  {
    if (sourceSheet == null)
      throw new ArgumentNullException(nameof (sourceSheet));
    sourceSheet.ParseSheetCF();
    this.ParseSheetCF();
  }

  protected void CopyAutoFilters(WorksheetImpl sourceSheet)
  {
    List<BiffRecordRaw> biffRecordRawList = sourceSheet != null ? sourceSheet.m_arrAutoFilter : throw new ArgumentNullException(nameof (sourceSheet));
    if (biffRecordRawList == null)
      return;
    List<BiffRecordRaw> autoFilterRecords = this.AutoFilterRecords;
    int index = 0;
    for (int count = biffRecordRawList.Count; index < count; ++index)
      autoFilterRecords.Add((BiffRecordRaw) CloneUtils.CloneCloneable((ICloneable) biffRecordRawList[index]));
  }

  protected void CopyColumnWidth(
    WorksheetImpl sourceSheet,
    Dictionary<int, int> hashExtFormatIndexes)
  {
    ColumnInfoRecord[] sourceArray = sourceSheet != null ? CloneUtils.CloneArray(sourceSheet.m_arrColumnInfo) : throw new ArgumentNullException(nameof (sourceSheet));
    int length1 = Math.Min(sourceArray.Length, this.m_arrColumnInfo.Length);
    Array.Copy((Array) sourceArray, (Array) this.m_arrColumnInfo, length1);
    this.UpdateIndexes((ICollection) this.m_arrColumnInfo, sourceSheet, hashExtFormatIndexes);
    if (hashExtFormatIndexes == null)
      return;
    int defaultXfIndex = sourceSheet.ParentWorkbook.DefaultXFIndex;
    int iXFIndex;
    if (!hashExtFormatIndexes.TryGetValue(defaultXfIndex, out iXFIndex))
      return;
    List<int> arrIsDefaultColumnWidth = (List<int>) null;
    if (iXFIndex != defaultXfIndex)
      arrIsDefaultColumnWidth = this.CreateColumnsOnUpdate(this.m_arrColumnInfo, iXFIndex);
    double d = double.NaN;
    int num = -1;
    int startIndex = 0;
    int index = 1;
    for (int length2 = this.m_arrColumnInfo.Length; index < length2; ++index)
    {
      ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[index];
      if (columnInfoRecord != null)
      {
        if (this.IsDefaultColumnWidth(arrIsDefaultColumnWidth, ref startIndex, index))
        {
          if (num < 0)
          {
            int columnWidth = (int) columnInfoRecord.ColumnWidth;
            int columnWidthInPixels = sourceSheet.GetColumnWidthInPixels(index);
            this.SetColumnWidthInPixels(index, columnWidthInPixels);
            num = (int) columnInfoRecord.ColumnWidth;
          }
          else
            columnInfoRecord.ColumnWidth = (ushort) num;
        }
        else if (double.IsNaN(d))
        {
          int columnWidth = (int) columnInfoRecord.ColumnWidth;
          int columnWidthInPixels = sourceSheet.GetColumnWidthInPixels(index);
          this.SetColumnWidthInPixels(index, columnWidthInPixels);
          d = (double) columnInfoRecord.ColumnWidth / (double) columnWidth;
        }
        else
          columnInfoRecord.ColumnWidth = (ushort) ((double) columnInfoRecord.ColumnWidth * d);
      }
    }
  }

  private bool IsDefaultColumnWidth(
    List<int> arrIsDefaultColumnWidth,
    ref int startIndex,
    int columnIndex)
  {
    if (arrIsDefaultColumnWidth == null)
      return false;
    int count = arrIsDefaultColumnWidth.Count;
    if (count == 0 || startIndex >= count)
      return false;
    int num;
    for (num = arrIsDefaultColumnWidth[startIndex]; num < columnIndex; num = arrIsDefaultColumnWidth[startIndex])
    {
      ++startIndex;
      if (startIndex >= count)
        return false;
    }
    return num == columnIndex;
  }

  private void UpdateIndexes(
    ICollection collection,
    WorksheetImpl sourceSheet,
    Dictionary<int, int> hashExtFormatIndexes)
  {
    this.UpdateIndexes(collection, sourceSheet, hashExtFormatIndexes, true);
  }

  private void UpdateIndexes(
    ICollection collection,
    WorksheetImpl sourceSheet,
    Dictionary<int, int> hashExtFormatIndexes,
    bool bUpdateDefault)
  {
    if (collection == null)
      throw new ArgumentNullException(nameof (collection));
    if (sourceSheet.Workbook == this.Workbook)
      return;
    WorkbookImpl parentWorkbook = sourceSheet.ParentWorkbook;
    int defaultXfIndex = this.m_book.DefaultXFIndex;
    foreach (IOutline outline in (IEnumerable) collection)
    {
      if (outline != null)
      {
        int extendedFormatIndex = (int) outline.ExtendedFormatIndex;
        if (hashExtFormatIndexes.ContainsKey(extendedFormatIndex))
        {
          int hashExtFormatIndex = hashExtFormatIndexes[extendedFormatIndex];
          if (bUpdateDefault || hashExtFormatIndex == defaultXfIndex)
            outline.ExtendedFormatIndex = (ushort) hashExtFormatIndex;
        }
      }
    }
  }

  private void UpdateOutlineIndexes(ICollection collection, int[] extFormatIndexes)
  {
    if (collection == null)
      throw new ArgumentNullException(nameof (collection));
    foreach (IOutline outline in (IEnumerable) collection)
    {
      if (outline != null)
      {
        int extendedFormatIndex = (int) outline.ExtendedFormatIndex;
        int extFormatIndex = extFormatIndexes[extendedFormatIndex];
        outline.ExtendedFormatIndex = (ushort) extFormatIndex;
      }
    }
  }

  private List<int> CreateColumnsOnUpdate(ColumnInfoRecord[] columns, int iXFIndex)
  {
    if (columns == null)
      throw new ArgumentNullException(nameof (columns));
    List<int> columnsOnUpdate = new List<int>();
    for (int index = 1; index <= this.m_book.MaxColumnCount; ++index)
    {
      if (columns[index] == null)
      {
        ColumnInfoRecord record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
        record.FirstColumn = record.LastColumn = (ushort) (index - 1);
        record.ExtendedFormatIndex = (ushort) iXFIndex;
        columns[index] = record;
        columnsOnUpdate.Add(index);
      }
    }
    return columnsOnUpdate;
  }

  protected void CopyMerges(WorksheetImpl sourceSheet)
  {
    MergeCellsImpl toClone = sourceSheet != null ? sourceSheet.MergeCells : throw new ArgumentNullException(nameof (sourceSheet));
    if (toClone == null)
      return;
    this.m_mergedCells = (MergeCellsImpl) CloneUtils.CloneCloneable((ICloneParent) toClone, (object) this);
  }

  protected void AttachEvents()
  {
    if (!this.m_book.Styles.Contains("Normal") && this.m_book.IsWorkbookOpening)
      return;
    (this.m_book.Styles["Normal"].Font as FontWrapper).AfterChangeEvent += new EventHandler(this.NormalFont_OnAfterChange);
  }

  protected void DetachEvents()
  {
    if (this.m_book == null || this.m_book.Styles == null || !this.m_book.Styles.Contains("Normal"))
      return;
    (this.m_book.Styles["Normal"].Font as FontWrapper).AfterChangeEvent -= new EventHandler(this.NormalFont_OnAfterChange);
  }

  protected override void OnDispose()
  {
    if (this.m_bIsDisposed)
      return;
    this.DetachEvents();
    this.m_arrColumnInfo = (ColumnInfoRecord[]) null;
    this.m_arrDConRecords = (List<BiffRecordRaw>) null;
    this.m_arrNotes = (SortedList<int, NoteRecord>) null;
    this.m_arrNotesByCellIndex = (SortedList<long, NoteRecord>) null;
    this.m_arrRecords = (List<BiffRecordRaw>) null;
    this.m_arrSortRecords = (List<BiffRecordRaw>) null;
    if (this.columnCollection != null)
    {
      this.columnCollection.Clear();
      this.columnCollection = (ColumnCollection) null;
    }
    if (this.m_arrAutoFilter != null)
    {
      this.m_arrAutoFilter.Clear();
      this.m_arrAutoFilter = (List<BiffRecordRaw>) null;
    }
    if (this.m_arrSelections != null)
    {
      this.m_arrSelections.Clear();
      this.m_arrSelections = (List<SelectionRecord>) null;
    }
    if (this.m_autoFitManager != null)
      this.m_autoFitManager.Dispose();
    this.m_bof = (BOFRecord) null;
    if (this.m_dataHolder != null && this.m_book == null)
    {
      this.m_dataHolder.Dispose();
      this.m_dataHolder = (WorksheetDataHolder) null;
    }
    if (this.m_dicRecordsCells != null)
    {
      this.m_dicRecordsCells.Dispose();
      this.m_dicRecordsCells = (CellRecordCollection) null;
    }
    this.m_dictCFExRecords = (Dictionary<int, CFExRecord>) null;
    this.m_dictCondFMT = (Dictionary<int, CondFMTRecord>) null;
    if (this.m_inlineStrings != null)
    {
      this.m_inlineStrings.Clear();
      this.m_inlineStrings = (Dictionary<string, string>) null;
    }
    if (this.m_mergedCells != null)
    {
      this.m_mergedCells.Dispose();
      this.m_mergedCells = (MergeCellsImpl) null;
    }
    this.m_migrantRange = (IMigrantRange) null;
    this.m_nameIndexChanged = (NameImpl.NameIndexChangedEventHandler) null;
    this.m_pane = (PaneRecord) null;
    if (this.m_preservePivotTables != null)
      this.m_preservePivotTables.Clear();
    this.m_rawColRecord = (ColumnInfoRecord) null;
    this.m_rowHeightHelper = (ItemSizeHelper) null;
    if (this.m_tableRecords != null)
      this.m_tableRecords.Clear();
    this.m_worksheetSlicer = (Stream) null;
    if (this.m_book != null)
      this.m_book.EnabledCalcEngine = false;
    if (this.m_dicRecordsCells != null)
    {
      this.m_dicRecordsCells.Dispose();
      this.m_dicRecordsCells = (CellRecordCollection) null;
    }
    if (this.m_pageSetup != null)
      this.m_pageSetup.Dispose();
    this.RowHeightChanged = (ValueChangedEventHandler) null;
    this.ColumnWidthChanged = (ValueChangedEventHandler) null;
    GC.SuppressFinalize((object) this);
  }

  protected void CopyPageSetup(WorksheetImpl sourceSheet)
  {
    this.m_pageSetup = sourceSheet != null ? sourceSheet.m_pageSetup.Clone((object) this) : throw new ArgumentNullException(nameof (sourceSheet));
  }

  protected int ImportExtendedFormat(
    int iXFIndex,
    WorkbookImpl basedOn,
    Dictionary<int, int> hashExtFormatIndexes)
  {
    return this.m_book.InnerExtFormats.Import(basedOn.InnerExtFormats[iXFIndex], hashExtFormatIndexes);
  }

  protected internal override void UpdateStyleIndexes(int[] styleIndexes)
  {
    this.UpdateOutlineIndexes((ICollection) this.m_arrColumnInfo, styleIndexes);
    this.m_dicRecordsCells.UpdateExtendedFormatIndex(styleIndexes);
  }

  internal Dictionary<int, int> IndexAndLevels
  {
    get
    {
      if (this.m_indexAndLevels == null)
        this.m_indexAndLevels = new Dictionary<int, int>();
      return this.m_indexAndLevels;
    }
    set => this.m_indexAndLevels = value;
  }

  internal Stream WorksheetSlicerStream
  {
    get => this.m_worksheetSlicer;
    set => this.m_worksheetSlicer = value;
  }

  internal double DefaultColumnWidth
  {
    set => this.m_defaultColWidth = value;
    get => this.m_defaultColWidth;
  }

  public MergeCellsImpl MergeCells
  {
    get
    {
      this.ParseData();
      if (this.m_mergedCells == null)
        this.m_mergedCells = new MergeCellsImpl(this.Application, (object) this);
      return this.m_mergedCells;
    }
  }

  [CLSCompliant(false)]
  public ColumnInfoRecord[] ColumnInformation
  {
    get
    {
      this.ParseData();
      return this.m_arrColumnInfo;
    }
  }

  public int VerticalSplit
  {
    get
    {
      this.ParseData();
      return this.m_pane == null ? 0 : this.m_pane.VerticalSplit;
    }
    set
    {
      this.ParseData();
      if (this.m_pane == null)
        this.CreateEmptyPane();
      this.m_pane.VerticalSplit = (int) (ushort) value;
    }
  }

  public int HorizontalSplit
  {
    get
    {
      this.ParseData();
      return this.m_pane == null ? 0 : this.m_pane.HorizontalSplit;
    }
    set
    {
      this.ParseData();
      if (this.m_pane == null)
        this.CreateEmptyPane();
      this.m_pane.HorizontalSplit = (int) (ushort) value;
    }
  }

  public int FirstVisibleRow
  {
    get
    {
      this.ParseData();
      return this.m_pane == null ? 0 : this.m_pane.FirstRow;
    }
    set
    {
      this.ParseData();
      if (this.m_pane == null)
        this.CreateEmptyPane();
      this.m_pane.FirstRow = (int) (ushort) value;
    }
  }

  internal int MaxColumnWidth => (int) byte.MaxValue;

  public int FirstVisibleColumn
  {
    get
    {
      this.ParseData();
      return this.m_pane == null ? 0 : this.m_pane.FirstColumn;
    }
    set
    {
      this.ParseData();
      if (this.m_pane == null)
        this.CreateEmptyPane();
      this.m_pane.FirstColumn = (int) (ushort) value;
    }
  }

  public IRange PrintArea => this.UsedRange;

  public int SelectionCount
  {
    get
    {
      this.ParseData();
      int selectionCount = 1;
      if (this.m_pane != null)
      {
        if (this.m_pane.VerticalSplit != 0)
          selectionCount *= 2;
        if (this.m_pane.HorizontalSplit != 0)
          selectionCount *= 2;
      }
      return selectionCount;
    }
  }

  public OfficeSheetView View
  {
    get => this.m_view;
    set => this.m_view = value;
  }

  public int DefaultRowHeight
  {
    get
    {
      this.ParseData();
      return this.m_pageSetup.DefaultRowHeight;
    }
    set
    {
      this.ParseData();
      if (this.m_pageSetup.DefaultRowHeight == value)
        return;
      if (this.StandardHeight != this.m_book.StandardRowHeight)
        this.m_pageSetup.DefaultRowHeightFlag = true;
      int defaultRowHeight = this.m_pageSetup.DefaultRowHeight;
      if (this.m_iFirstRow >= 0 && this.m_iLastRow >= 0)
      {
        for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
        {
          RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iFirstRow, false);
          if (row != null && !row.IsBadFontHeight && (int) row.Height == defaultRowHeight)
          {
            row.Height = (ushort) value;
            row.IsBadFontHeight = true;
          }
        }
      }
      this.m_pageSetup.DefaultRowHeight = value;
    }
  }

  public WorksheetNamesCollection InnerNames => this.m_names;

  public CellRecordCollection CellRecords
  {
    [DebuggerStepThrough] get
    {
      this.ParseData();
      return this.m_dicRecordsCells;
    }
  }

  public override PageSetupBaseImpl PageSetupBase
  {
    get
    {
      this.ParseData();
      return (PageSetupBaseImpl) this.m_pageSetup;
    }
  }

  [CLSCompliant(false)]
  public PaneRecord Pane
  {
    get
    {
      this.ParseData();
      if (this.m_pane == null)
        this.m_pane = (PaneRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Pane);
      return this.m_pane;
    }
  }

  [CLSCompliant(false)]
  public List<SelectionRecord> Selections
  {
    get
    {
      this.ParseData();
      return this.m_arrSelections;
    }
  }

  public bool UseRangesCache
  {
    get
    {
      this.ParseData();
      return this.m_dicRecordsCells.UseCache;
    }
    set
    {
      this.ParseData();
      this.m_dicRecordsCells.UseCache = value;
    }
  }

  private List<BiffRecordRaw> AutoFilterRecords
  {
    get
    {
      this.ParseData();
      if (this.m_arrAutoFilter == null)
        this.m_arrAutoFilter = new List<BiffRecordRaw>();
      return this.m_arrAutoFilter;
    }
  }

  private List<BiffRecordRaw> DConRecords
  {
    get
    {
      this.ParseData();
      if (this.m_arrDConRecords == null)
        this.m_arrDConRecords = new List<BiffRecordRaw>();
      return this.m_arrDConRecords;
    }
  }

  private List<BiffRecordRaw> SortRecords
  {
    get
    {
      this.ParseData();
      if (this.m_arrSortRecords == null)
        this.m_arrSortRecords = new List<BiffRecordRaw>();
      return this.m_arrSortRecords;
    }
  }

  public string QuotedName
  {
    get
    {
      this.ParseData();
      return $"'{this.Name.Replace("'", "''")}'";
    }
  }

  public OfficeVersion Version
  {
    get => this.m_book.Version;
    set
    {
      if (this.m_iLastRow != -1)
        this.m_iLastRow = Math.Min(this.m_iLastRow, this.m_book.MaxRowCount);
      if (this.m_iFirstRow != -1)
        this.m_iFirstRow = Math.Min(this.m_iFirstRow, this.m_book.MaxRowCount);
      if (this.m_iFirstColumn != int.MaxValue)
        this.m_iFirstColumn = Math.Min(this.m_iFirstColumn, this.m_book.MaxColumnCount);
      if (this.m_iLastColumn != int.MaxValue)
        this.m_iLastColumn = Math.Min(this.m_iLastColumn, this.m_book.MaxColumnCount);
      ColumnInfoRecord[] arrColumnInfo = this.m_arrColumnInfo;
      this.m_arrColumnInfo = new ColumnInfoRecord[this.m_book.MaxColumnCount + 2];
      Array.Copy((Array) arrColumnInfo, 0, (Array) this.m_arrColumnInfo, 0, Math.Min(arrColumnInfo.Length, this.m_arrColumnInfo.Length));
      if (this.m_book.IsConverted && arrColumnInfo[arrColumnInfo.Length - 1] != null && this.m_rawColRecord != null)
      {
        for (int length = arrColumnInfo.Length; length < this.m_arrColumnInfo.Length; ++length)
        {
          ColumnInfoRecord columnInfoRecord = this.m_rawColRecord.Clone() as ColumnInfoRecord;
          columnInfoRecord.FirstColumn = (ushort) length;
          columnInfoRecord.LastColumn = (ushort) length;
          this.m_arrColumnInfo[length] = columnInfoRecord;
        }
      }
      this.m_dicRecordsCells.Version = value;
      if (value == OfficeVersion.Excel97to2003 && this.m_mergedCells != null)
        this.m_mergedCells.SetNewDimensions(this.m_book.MaxRowCount, this.m_book.MaxColumnCount);
      FileDataHolder dataHolder = ((WorkbookImpl) this.Workbook).DataHolder;
      this.InnerNames?.ConvertFullRowColumnNames(value);
      if (this.m_pane != null && (this.m_pane.FirstRow > this.m_book.MaxRowCount - 1 || this.m_pane.FirstColumn > this.m_book.MaxColumnCount - 1))
        this.m_pane = (PaneRecord) null;
      this.InnerShapes?.SetVersion(value);
    }
  }

  public RecordExtractor RecordExtractor => this.CellRecords.RecordExtractor;

  internal ItemSizeHelper RowHeightHelper
  {
    get
    {
      if (this.m_rowHeightHelper == null)
        this.m_rowHeightHelper = new ItemSizeHelper(new ItemSizeHelper.SizeGetter(this.GetRowHeightInPixels));
      return this.m_rowHeightHelper;
    }
  }

  internal bool IsVisible
  {
    get => this.m_isRowHeightSet;
    set => this.m_isRowHeightSet = value;
  }

  internal bool IsZeroHeight
  {
    get => this.m_isZeroHeight;
    set => this.m_isZeroHeight = value;
  }

  internal int BaseColumnWidth
  {
    get => this.m_baseColumnWidth;
    set => this.m_baseColumnWidth = value;
  }

  internal bool IsThickBottom
  {
    get => this.m_isThickBottom;
    set => this.m_isThickBottom = value;
  }

  internal bool IsThickTop
  {
    get => this.m_isThickTop;
    set => this.m_isThickTop = value;
  }

  internal byte OutlineLevelColumn
  {
    get => this.m_outlineLevelColumn;
    set => this.m_outlineLevelColumn = value;
  }

  internal byte OutlineLevelRow
  {
    get => this.m_outlineLevelRow;
    set => this.m_outlineLevelRow = value;
  }

  internal bool CustomHeight
  {
    get => this.m_isCustomHeight;
    set => this.m_isCustomHeight = value;
  }

  public int RowsOutlineLevel => (int) this.OutlineLevelRow;

  public int ColumnsOutlineLevel => (int) this.OutlineLevelColumn;

  public List<IOutlineWrapper> OutlineWrappers
  {
    get
    {
      if (this.m_outlineWrappers == null)
        this.OutlineWrappers = new List<IOutlineWrapper>();
      return this.m_outlineWrappers;
    }
    set => this.m_outlineWrappers = value;
  }

  public bool HasMergedCells => this.m_mergedCells != null && this.m_mergedCells.MergeCount > 0;

  protected override OfficeSheetProtection DefaultProtectionOptions
  {
    get => OfficeSheetProtection.LockedCells | OfficeSheetProtection.UnLockedCells;
  }

  protected override OfficeSheetProtection UnprotectedOptions => OfficeSheetProtection.Content;

  internal Dictionary<string, string> InlineStrings
  {
    get
    {
      if (this.m_inlineStrings == null)
        this.m_inlineStrings = new Dictionary<string, string>();
      return this.m_inlineStrings;
    }
  }

  internal List<BiffRecordRaw> PreserveExternalConnection
  {
    get
    {
      if (this.m_preserveExternalConnection == null)
        this.m_preserveExternalConnection = new List<BiffRecordRaw>();
      return this.m_preserveExternalConnection;
    }
  }

  internal List<Stream> PreservePivotTables
  {
    get
    {
      if (this.m_preservePivotTables == null)
        this.m_preservePivotTables = new List<Stream>();
      return this.m_preservePivotTables;
    }
  }

  internal override bool ParseDataOnDemand
  {
    get => this.m_bParseDataOnDemand;
    set => this.m_bParseDataOnDemand = value;
  }

  internal Dictionary<int, List<Point>> ColumnOutlineLevels
  {
    get
    {
      if (this.m_columnOutlineLevels == null)
        this.m_columnOutlineLevels = new Dictionary<int, List<Point>>();
      return this.m_columnOutlineLevels;
    }
    set => this.m_columnOutlineLevels = value;
  }

  internal Dictionary<int, List<Point>> RowOutlineLevels
  {
    get
    {
      if (this.m_rowOutlineLevels == null)
        this.m_rowOutlineLevels = new Dictionary<int, List<Point>>();
      return this.m_rowOutlineLevels;
    }
    set => this.m_rowOutlineLevels = value;
  }

  public IRange this[int row, int column] => this.Range[row, column];

  public IRange this[int row, int column, int lastRow, int lastColumn]
  {
    get => this.Range[row, column, lastRow, lastColumn];
  }

  public IRange this[string name] => this[name, false];

  public IRange this[string name, bool IsR1C1Notation] => this.Range[name, IsR1C1Notation];

  public int ActivePane
  {
    get
    {
      this.ParseData();
      return this.m_pane == null ? int.MinValue : (int) this.m_pane.ActivePane;
    }
    set
    {
      this.ParseData();
      if (this.m_pane == null)
        this.CreateEmptyPane();
      this.m_pane.ActivePane = (ushort) value;
    }
  }

  public IRange[] Cells => this.UsedRange.Cells;

  internal ColumnCollection Columnss
  {
    get
    {
      if (this.columnCollection != null)
        return this.columnCollection;
      int num = this.GetAppImpl().GetFontCalc2() * 8 + this.GetAppImpl().GetFontCalc3();
      this.columnCollection = new ColumnCollection(this, 8.0 + (double) ((num / 8 + 1) * 8 - num) * 1.0 / (double) this.GetAppImpl().GetFontCalc2());
      return this.columnCollection;
    }
  }

  public IRange[] Columns => this.UsedRange.Columns;

  public bool DisplayPageBreaks
  {
    get
    {
      this.ParseData();
      return this.m_bDisplayPageBreaks;
    }
    set
    {
      this.ParseData();
      if (this.m_bDisplayPageBreaks == value)
        return;
      this.SetChanged();
      this.m_bDisplayPageBreaks = value;
    }
  }

  internal AutoFitManager AutoFitManagerImpl
  {
    get
    {
      if (this.m_autoFitManager == null)
        this.m_autoFitManager = new AutoFitManager();
      return this.m_autoFitManager;
    }
    set => this.m_autoFitManager = value;
  }

  public bool IsDisplayZeros
  {
    get
    {
      this.ParseData();
      return this.WindowTwo.IsDisplayZeros;
    }
    set
    {
      this.ParseData();
      this.WindowTwo.IsDisplayZeros = value;
    }
  }

  public bool IsGridLinesVisible
  {
    get
    {
      this.ParseData();
      return this.WindowTwo.IsDisplayGridlines;
    }
    set
    {
      this.ParseData();
      this.WindowTwo.IsDisplayGridlines = value;
    }
  }

  public bool IsRowColumnHeadersVisible
  {
    get
    {
      this.ParseData();
      return this.WindowTwo.IsDisplayRowColHeadings;
    }
    set
    {
      this.ParseData();
      this.WindowTwo.IsDisplayRowColHeadings = value;
    }
  }

  public bool IsStringsPreserved
  {
    get
    {
      this.ParseData();
      return this.m_bStringsPreserved;
    }
    set
    {
      this.ParseData();
      this.m_stringPreservedRanges.Clear();
      this.m_bStringsPreserved = value;
    }
  }

  public IRange[] MergedCells
  {
    get
    {
      this.ParseData();
      int mergeCount = this.m_mergedCells != null ? this.m_mergedCells.MergeCount : 0;
      IRange[] mergedCells = mergeCount > 0 ? new IRange[mergeCount] : (IRange[]) null;
      if (mergedCells != null)
      {
        List<Rectangle> mergedRegions = this.m_mergedCells.MergedRegions;
        for (int index = 0; index < mergeCount; ++index)
        {
          Rectangle rectangle = mergedRegions[index];
          RangeImpl range = this.AppImplementation.CreateRange((object) this, rectangle.X + 1, rectangle.Y + 1, rectangle.Right + 1, rectangle.Bottom + 1);
          mergedCells[index] = (IRange) range;
        }
      }
      return mergedCells;
    }
  }

  public INames Names => (INames) this.m_names;

  public IPageSetup PageSetup
  {
    get
    {
      this.ParseData();
      return (IPageSetup) this.m_pageSetup;
    }
  }

  public IRange PaneFirstVisible
  {
    get
    {
      this.ParseData();
      return (IRange) this.AppImplementation.CreateRange((object) this, this.FirstVisibleColumn + 1, this.FirstVisibleRow + 1);
    }
    set
    {
      this.ParseData();
      this.FirstVisibleRow = value.Row - 1;
      this.FirstVisibleColumn = value.Column - 1;
    }
  }

  public IRange Range
  {
    [DebuggerStepThrough] get => this.UsedRange;
  }

  public IRange[] Rows => this.UsedRange.Rows;

  public bool IsFreezePanes => this.WindowTwo.IsFreezePanes;

  public IRange SplitCell
  {
    get
    {
      this.ParseData();
      return (IRange) this.AppImplementation.CreateRange((object) this, this.VerticalSplit + 1, this.HorizontalSplit + 1);
    }
    set
    {
      this.ParseData();
      this.VerticalSplit = value.Column - 1;
      this.HorizontalSplit = value.Row - 1;
      this.WindowTwo.IsFreezePanes = true;
      this.WindowTwo.IsFreezePanesNoSplit = true;
    }
  }

  public double StandardHeight
  {
    get => (double) this.DefaultRowHeight / 20.0;
    set
    {
      if (value < 0.0)
        throw new ArgumentOutOfRangeException("Standard Row Height");
      this.DefaultRowHeight = (int) (value * 20.0);
    }
  }

  public bool StandardHeightFlag
  {
    get
    {
      this.ParseData();
      return this.m_pageSetup.DefaultRowHeightFlag;
    }
    set
    {
      this.ParseData();
      this.m_pageSetup.DefaultRowHeightFlag = value;
    }
  }

  public double StandardWidth
  {
    get
    {
      this.ParseData();
      return this.m_dStandardColWidth;
    }
    set
    {
      this.ParseData();
      this.m_dStandardColWidth = value;
    }
  }

  public OfficeSheetType Type
  {
    get => this.m_sheetType;
    set
    {
      this.m_sheetType = value;
      if (this.IsSupported || this.m_sheetType != OfficeSheetType.Worksheet)
        return;
      this.IsSupported = true;
    }
  }

  public IRange UsedRange
  {
    get
    {
      this.ParseData();
      if (this.m_busedRangeIncludesCF && this.m_parseCF && !(this.Workbook as WorkbookImpl).IsWorkbookOpening && !(this.Workbook as WorkbookImpl).Saving)
        this.ParseSheetCF();
      if (this.m_iFirstColumn == this.m_iLastColumn && this.m_iFirstColumn == int.MaxValue || this.m_iFirstRow == this.m_iLastRow && this.m_iFirstRow < 0)
      {
        if (this.m_rngUsed != null)
          this.m_rngUsed.Dispose();
        this.m_rngUsed = this.AppImplementation.CreateRange((object) this);
      }
      else
      {
        int iFirstRow = this.m_iFirstRow;
        int iFirstColumn = this.m_iFirstColumn;
        int iLastRow = this.m_iLastRow;
        int iLastColumn = this.m_iLastColumn;
        this.GetRangeCoordinates(ref iFirstRow, ref iFirstColumn, ref iLastRow, ref iLastColumn);
        this.CreateUsedRange(iFirstRow, iFirstColumn, iLastRow, iLastColumn);
      }
      if (this.m_rngUsed.Row == 0 && this.m_rngUsed.Column == 0 && this.ImportDTHelper != null)
      {
        this.ImportDataTable(this.ImportDTHelper.DataTable, true, this.ImportDTHelper.FirstRow, this.ImportDTHelper.FirstColumn);
        this.m_rngUsed = this.UsedRange as RangeImpl;
      }
      return (IRange) this.m_rngUsed;
    }
  }

  public IRange[] UsedCells
  {
    get
    {
      this.ParseData();
      List<IRange> rangeList = new List<IRange>();
      int num = 0;
      foreach (DictionaryEntry dicRecordsCell in this.m_dicRecordsCells)
      {
        if (dicRecordsCell.Value != null)
        {
          ICellPositionFormat cellPositionFormat = dicRecordsCell.Value as ICellPositionFormat;
          rangeList.Add(this.InnerGetCell(cellPositionFormat.Column + 1, cellPositionFormat.Row + 1));
          ++num;
        }
      }
      return rangeList.ToArray();
    }
  }

  public bool IsEmpty
  {
    get
    {
      this.ParseData();
      return this.m_iFirstRow == -1;
    }
  }

  public IMigrantRange MigrantRange
  {
    get
    {
      this.ParseData();
      if (this.m_migrantRange == null)
        this.CreateMigrantRange();
      return this.m_migrantRange;
    }
  }

  public bool UsedRangeIncludesFormatting
  {
    get => this.m_bUsedRangeIncludesFormatting;
    set => this.m_bUsedRangeIncludesFormatting = value;
  }

  internal bool UsedRangeIncludesCF
  {
    get => this.m_busedRangeIncludesCF;
    set => this.m_busedRangeIncludesCF = value;
  }

  public override bool ProtectContents
  {
    get => (this.InnerProtection & OfficeSheetProtection.Content) == OfficeSheetProtection.None;
    internal set
    {
      if (!value)
        this.InnerProtection |= OfficeSheetProtection.Content;
      else
        this.InnerProtection &= ~OfficeSheetProtection.Content;
    }
  }

  internal ImportDTHelper ImportDTHelper
  {
    get => this.m_importDTHelper;
    set => this.m_importDTHelper = value;
  }

  internal bool IsImporting
  {
    get => this.m_bIsImporting;
    set => this.m_bIsImporting = value;
  }

  public event ValueChangedEventHandler ColumnWidthChanged;

  public event ValueChangedEventHandler RowHeightChanged;

  public IInternalWorksheet GetClonedObject(
    Dictionary<string, string> hashNewNames,
    WorkbookImpl book)
  {
    string str1 = this.Name;
    string str2;
    if (hashNewNames != null && hashNewNames.TryGetValue(str1, out str2))
      str1 = str2;
    return book.Worksheets[str1] as IInternalWorksheet;
  }

  public void ParseCFFromExcel2007(FileDataHolder dataHolder)
  {
    if (dataHolder == null)
      return;
    List<DxfImpl> dxfsCollection = dataHolder.ParseDxfsCollection();
    WorksheetDataHolder worksheetDataHolder = (WorksheetDataHolder) null;
    if (dxfsCollection != null)
      worksheetDataHolder = this.DataHolder;
    worksheetDataHolder?.ParseConditionalFormatting(dxfsCollection, this);
  }

  public void ParseSheetCF()
  {
    if (this.Version == OfficeVersion.Excel97to2003)
      return;
    this.m_book.AppImplementation.IsFormulaParsed = false;
    this.ParseCFFromExcel2007(this.m_book.DataHolder);
    this.m_book.AppImplementation.IsFormulaParsed = true;
  }

  public override void UpdateExtendedFormatIndex(Dictionary<int, int> dictFormats)
  {
    this.ParseData();
    base.UpdateExtendedFormatIndex(dictFormats);
    this.m_dicRecordsCells.UpdateExtendedFormatIndex(dictFormats);
    this.UpdateOutlineAfterXFRemove((ICollection) this.m_arrColumnInfo, (IDictionary) dictFormats);
  }

  public void UpdateExtendedFormatIndex(int maxCount)
  {
    this.ParseData();
    if (maxCount <= 0)
      throw new ArgumentOutOfRangeException(nameof (maxCount));
    this.m_dicRecordsCells.UpdateExtendedFormatIndex(maxCount);
    int defaultXfIndex = this.m_book.DefaultXFIndex;
    int index = 0;
    for (int length = this.m_arrColumnInfo.Length; index < length; ++index)
    {
      ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[index];
      if (columnInfoRecord != null && (int) columnInfoRecord.ExtendedFormatIndex >= maxCount)
        columnInfoRecord.ExtendedFormatIndex = (ushort) defaultXfIndex;
    }
  }

  public RangeRichTextString CreateLabelSSTRTFString(long cellIndex)
  {
    this.ParseData();
    IRange range = (IRange) this.m_dicRecordsCells.GetRange(cellIndex);
    return range == null ? new RangeRichTextString(this.Application, (object) this, cellIndex) : (RangeRichTextString) range.RichText;
  }

  public IRange[] Find(IRange range, byte findValue, bool bIsError, bool bIsFindFirst)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    this.ParseData();
    return this.ConvertCellListIntoRange(this.m_dicRecordsCells.Find(range, findValue, bIsError, bIsFindFirst));
  }

  public IRange[] Find(IRange range, double findValue, OfficeFindType flags, bool bIsFindFirst)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    this.ParseData();
    return this.ConvertCellListIntoRange(this.m_dicRecordsCells.Find(range, findValue, flags, bIsFindFirst));
  }

  public IRange[] Find(IRange range, string findValue, OfficeFindType flags, bool bIsFindFirst)
  {
    return this.Find(range, findValue, flags, OfficeFindOptions.None, bIsFindFirst);
  }

  public IRange[] Find(
    IRange range,
    string findValue,
    OfficeFindType flags,
    OfficeFindOptions findOptions,
    bool bIsFindFirst)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    if (findValue == null || findValue.Length == 0)
      return (IRange[]) null;
    this.ParseData();
    return this.ConvertCellListIntoRange(this.m_dicRecordsCells.Find(range, findValue, flags, findOptions, bIsFindFirst));
  }

  public void CopyToClipboard()
  {
    this.ParseData();
    this.m_book.CopyToClipboard(this);
  }

  public void MoveRange(
    IRange destination,
    IRange source,
    OfficeCopyRangeOptions options,
    bool bUpdateRowRecords)
  {
    this.MoveRange(destination, source, options, bUpdateRowRecords, (IOperation) null);
  }

  private void MoveRange(
    IRange destination,
    IRange source,
    OfficeCopyRangeOptions options,
    bool bUpdateRowRecords,
    IOperation beforeMove)
  {
    this.ParseData();
    if (destination == source)
      return;
    WorksheetImpl worksheetImpl = this.CanMove(ref destination, source) ? (WorksheetImpl) destination.Worksheet : throw new InvalidRangeException();
    WorksheetImpl worksheet = (WorksheetImpl) source.Worksheet;
    beforeMove?.Do();
    int iSourceIndex = this.m_book.AddSheetReference((IWorksheet) worksheet);
    int iDestIndex = this.m_book.AddSheetReference((IWorksheet) worksheetImpl);
    this.m_book.AddSheetReference((IWorksheet) worksheet);
    Rectangle rectangle1 = Rectangle.FromLTRB(source.Column - 1, source.Row - 1, source.LastColumn - 1, source.LastRow - 1);
    Rectangle rectangle2 = Rectangle.FromLTRB(destination.Column - 1, destination.Row - 1, destination.LastColumn - 1, destination.LastRow - 1);
    int row1 = destination.Row;
    int row2 = source.Row;
    int column1 = destination.Column;
    int column2 = source.Column;
    bool flag = (options & OfficeCopyRangeOptions.UpdateFormulas) != OfficeCopyRangeOptions.None;
    RangeImpl rangeImpl = (RangeImpl) destination;
    int iMaxRow = 0;
    int iMaxColumn = 0;
    RecordTable source1 = this.CacheAndRemoveFromParent(source, destination, ref iMaxRow, ref iMaxColumn, worksheet.m_dicRecordsCells);
    if ((options & OfficeCopyRangeOptions.UpdateMerges) != OfficeCopyRangeOptions.None)
      WorksheetImpl.CopyRangeMerges(destination, source, true);
    worksheet.PartialClearRange(rectangle1);
    worksheetImpl.CellRecords.ClearRange(rectangle2);
    this.CopyCacheInto(source1, worksheetImpl.m_dicRecordsCells.Table, bUpdateRowRecords);
    source1?.Dispose();
    WorksheetHelper.AccessRow((IInternalWorksheet) worksheetImpl, rangeImpl.FirstRow);
    WorksheetHelper.AccessColumn((IInternalWorksheet) worksheetImpl, rangeImpl.FirstColumn);
    if (iMaxColumn > rangeImpl.FirstColumn)
      WorksheetHelper.AccessColumn((IInternalWorksheet) worksheetImpl, iMaxColumn);
    if (iMaxRow > rangeImpl.FirstRow)
      WorksheetHelper.AccessRow((IInternalWorksheet) worksheetImpl, iMaxRow);
    if (flag)
      this.m_book.UpdateFormula(iSourceIndex, rectangle1, iDestIndex, rectangle2);
    if ((options & OfficeCopyRangeOptions.CopyShapes) == OfficeCopyRangeOptions.None)
      return;
    ++rectangle1.X;
    ++rectangle1.Y;
    ++rectangle2.X;
    ++rectangle2.Y;
    ((ShapesCollection) this.Shapes).CopyMoveShapeOnRangeCopy(worksheetImpl, rectangle1, rectangle2, false);
  }

  public IRange CopyRange(IRange destination, IRange source)
  {
    return this.CopyRange(destination, source, OfficeCopyRangeOptions.UpdateMerges);
  }

  public IRange CopyRange(IRange destination, IRange source, OfficeCopyRangeOptions options)
  {
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    this.ParseData();
    if (source.Worksheet != this)
      return ((WorksheetImpl) source.Worksheet).CopyRange(destination, source);
    RangeImpl rangeImpl = (RangeImpl) source;
    RangeImpl destinationRange = (RangeImpl) destination;
    if (rangeImpl.IsEntireRow)
    {
      if (source.RowHeight < 0.0)
        rangeImpl.SetDifferedRowHeight(rangeImpl, destinationRange);
      else
        destination.RowHeight = source.RowHeight;
    }
    if (rangeImpl.IsEntireColumn)
    {
      if (source.ColumnWidth < 0.0)
        rangeImpl.SetDifferedColumnWidth(rangeImpl, destinationRange);
      else
        destination.ColumnWidth = source.ColumnWidth;
    }
    int row = destination.Row;
    int column1 = destination.Column;
    if (destinationRange.IsSingleCell && !rangeImpl.IsSingleCell)
    {
      int lastRow = row + rangeImpl.LastRow - rangeImpl.Row;
      int lastColumn = column1 + rangeImpl.LastColumn - rangeImpl.Column;
      destination = destinationRange[row, column1, lastRow, lastColumn];
    }
    int num1 = destination.LastRow - row + 1;
    int num2 = destination.LastColumn - column1 + 1;
    int num3 = source.LastRow - source.Row + 1;
    int num4 = source.LastColumn - source.Column + 1;
    int num5 = 1;
    int num6 = 1;
    if (num1 % num3 == 0 && num2 % num4 == 0)
    {
      num5 = num1 / num3;
      num6 = num2 / num4;
    }
    int num7 = 0;
    while (num7 < num5)
    {
      int num8 = 0;
      int column2 = column1;
      while (num8 < num6)
      {
        RangeImpl destination1 = (RangeImpl) destination[row, column2, row + num3 - 1, column2 + num4 - 1];
        if (!destination1.AreFormulaArraysNotSeparated)
          throw new InvalidRangeException();
        this.CopyRangeWithoutCheck(rangeImpl, destination1, options);
        ++num8;
        column2 += num4;
      }
      ++num7;
      row += num3;
    }
    return destination;
  }

  public void CopyRangeWithoutCheck(
    RangeImpl source,
    RangeImpl destination,
    OfficeCopyRangeOptions options)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    if (!destination.AreFormulaArraysNotSeparated)
      throw new InvalidRangeException("Can't copy to destination range.");
    this.ParseData();
    int iRowCount = source.LastRow - source.Row + 1;
    int iColumnCount = source.LastColumn - source.Column + 1;
    Rectangle rectIntersection;
    RecordTable intersection = this.m_dicRecordsCells.CacheIntersection((IRange) destination, (IRange) source, out rectIntersection);
    Dictionary<ArrayRecord, object> formulaArrays = destination.FormulaArrays;
    WorksheetImpl worksheet = (WorksheetImpl) destination.Worksheet;
    if ((options & OfficeCopyRangeOptions.UpdateMerges) != OfficeCopyRangeOptions.None)
      this.CopyRangeMerges((IRange) destination, (IRange) source);
    if (formulaArrays != null && formulaArrays.Count > 0)
      worksheet.RemoveArrayFormulas((ICollection<ArrayRecord>) formulaArrays.Keys, true);
    int column1 = destination.Column;
    int row1 = destination.Row;
    int row2 = source.Row;
    int column2 = source.Column;
    if ((options & OfficeCopyRangeOptions.CopyShapes) != OfficeCopyRangeOptions.None)
    {
      Rectangle rec = new Rectangle(column2, row2, iColumnCount - 1, iRowCount - 1);
      Rectangle recDest = rec with { X = column1, Y = row1 };
      ((ShapesCollection) this.Shapes).CopyMoveShapeOnRangeCopy(worksheet, rec, recDest, true);
    }
    destination.Clear();
    this.CopyRange(row2, column2, iRowCount, iColumnCount, row1, column1, worksheet, intersection, rectIntersection, options);
    intersection?.Dispose();
  }

  [CLSCompliant(false)]
  public void CopyCell(
    ICellPositionFormat cell,
    string strFormulaValue,
    IDictionary dicXFIndexes,
    long lNewIndex,
    WorkbookImpl book,
    Dictionary<int, int> dicFontIndexes,
    OfficeCopyRangeOptions options)
  {
    this.ParseData();
    int row = cell.Row;
    int column = cell.Column;
    this.m_dicRecordsCells.GetRange(lNewIndex)?.UpdateRecord();
  }

  public void CopyRange(
    int iSourceRow,
    int iSourceColumn,
    int iRowCount,
    int iColumnCount,
    int iDestRow,
    int iDestColumn,
    WorksheetImpl destSheet,
    RecordTable intersection,
    Rectangle rectIntersection,
    OfficeCopyRangeOptions options)
  {
    this.ParseData();
    Dictionary<int, int> dicFontIndexes = (Dictionary<int, int>) null;
    Dictionary<int, int> dicXFIndexes = (Dictionary<int, int>) null;
    if ((options & OfficeCopyRangeOptions.CopyStyles) != OfficeCopyRangeOptions.None)
      dicXFIndexes = this.GetUpdatedXFIndexes(iSourceRow, iSourceColumn, iRowCount, iColumnCount, destSheet, out dicFontIndexes);
    CellRecordCollection cellRecords = destSheet.CellRecords;
    int iDeltaColumn = iDestColumn - iSourceColumn;
    int iDeltaRow = iDestRow - iSourceRow;
    Dictionary<long, long> dictionary = new Dictionary<long, long>();
    RecordTable table = destSheet.CellRecords.Table;
    int allocationBlockSize = this.Application.RowStorageAllocationBlockSize;
    for (int index1 = 0; index1 < iRowCount; ++index1)
    {
      int firstRow = iDestRow + index1;
      int iRow = iSourceRow + index1;
      for (int index2 = 0; index2 < iColumnCount; ++index2)
      {
        int firstColumn = iDestColumn + index2;
        int iColumn = iSourceColumn + index2;
        long cellIndex1 = RangeImpl.GetCellIndex(firstColumn, firstRow);
        RecordTable recordTable = this.GetRecordTable(iRow, iColumn, rectIntersection, intersection, this.m_dicRecordsCells.Table);
        RowStorage row = recordTable.Rows[iRow - 1];
        ICellPositionFormat record = row?.GetRecord(iColumn - 1, allocationBlockSize);
        if (record != null)
        {
          string formulaStringValue = row.GetFormulaStringValue(iColumn - 1);
          destSheet.CopyCell(record, formulaStringValue, (IDictionary) dicXFIndexes, cellIndex1, this.m_book, dicFontIndexes, options);
          if (record.TypeCode == TBIFFRecord.Formula)
          {
            ArrayRecord arrayRecord = recordTable.GetArrayRecord(record);
            if (arrayRecord != null)
            {
              long cellIndex2 = RangeImpl.GetCellIndex(arrayRecord.FirstColumn, arrayRecord.FirstRow);
              int num1;
              int num2;
              if (dictionary.ContainsKey(cellIndex2))
              {
                long index3 = dictionary[cellIndex2];
                num1 = RangeImpl.GetRowFromCellIndex(index3);
                num2 = RangeImpl.GetColumnFromCellIndex(index3);
              }
              else
              {
                num1 = firstRow - 1;
                num2 = firstColumn - 1;
                long cellIndex3 = RangeImpl.GetCellIndex(num2, num1);
                dictionary[cellIndex2] = cellIndex3;
                arrayRecord.FirstColumn = Math.Max(arrayRecord.FirstColumn, iSourceColumn - 1) + iDeltaColumn;
                arrayRecord.FirstRow = Math.Max(arrayRecord.FirstRow, iSourceRow - 1) + iDeltaRow;
                arrayRecord.LastColumn = Math.Min(arrayRecord.LastColumn, iSourceColumn + iColumnCount - 2);
                arrayRecord.LastColumn += iDeltaColumn;
                arrayRecord.LastRow = Math.Min(arrayRecord.LastRow, iSourceRow + iRowCount - 2);
                arrayRecord.LastRow += iDeltaRow;
                if ((options & OfficeCopyRangeOptions.UpdateFormulas) != OfficeCopyRangeOptions.None)
                  this.UpdateArrayFormula(arrayRecord, (IWorksheet) destSheet, iDeltaRow, iDeltaColumn);
                table.Rows[firstRow - 1].SetArrayRecord(firstColumn - 1, arrayRecord, this.Application.RowStorageAllocationBlockSize);
              }
              table.Rows[firstRow - 1]?.SetArrayFormulaIndex(firstColumn - 1, num1, num2, this.Application.RowStorageAllocationBlockSize);
            }
          }
        }
        else
          cellRecords.Remove(cellIndex1);
      }
    }
  }

  public void RemoveArrayFormulas(ICollection<ArrayRecord> colRemove, bool bClearRange)
  {
    if (colRemove == null)
      throw new ArgumentNullException(nameof (colRemove));
    this.ParseData();
    foreach (ArrayRecord record in (IEnumerable<ArrayRecord>) colRemove)
      this.RemoveArrayFormula(record, bClearRange);
  }

  public Ptg[] UpdateFormula(Ptg[] arrFormula, int iRowOffset, int iColOffset)
  {
    if (arrFormula == null)
      throw new ArgumentNullException(nameof (arrFormula));
    this.ParseData();
    bool flag = iRowOffset != 0 || iColOffset != 0;
    Ptg[] ptgArray = new Ptg[arrFormula.Length];
    int index = 0;
    for (int length = arrFormula.Length; index < length; ++index)
      ptgArray[index] = !flag ? (Ptg) arrFormula[index].Clone() : arrFormula[index].Offset(iRowOffset, iColOffset, this.m_book);
    return ptgArray;
  }

  public override void UpdateFormula(
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect)
  {
    this.ParseData();
    this.m_dicRecordsCells.UpdateFormula(iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect);
    base.UpdateFormula(iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect);
  }

  public void AutofitRow(int rowIndex)
  {
    int column = this.UsedRange.Column;
    int lastColumn = this.UsedRange.LastColumn;
    this.AutofitRow(rowIndex, column, lastColumn, true);
  }

  public void AutofitColumn(int colIndex)
  {
    (this[this.UsedRange.Row, this.UsedRange.Column, this.UsedRange.LastRow, this.UsedRange.LastColumn] as RangeImpl).AutoFitToColumn(colIndex, colIndex);
  }

  public void AutofitColumn(int colIndex, int firstRow, int lastRow)
  {
    (this[firstRow, colIndex, lastRow, colIndex] as RangeImpl).AutoFitToColumn(colIndex, colIndex);
  }

  public void CopyFrom(
    WorksheetImpl worksheet,
    Dictionary<string, string> hashStyleNames,
    Dictionary<string, string> hashWorksheetNames,
    Dictionary<int, int> dicFontIndexes,
    OfficeWorksheetCopyFlags flags)
  {
    Dictionary<int, int> hashExtFormatIndexes = new Dictionary<int, int>();
    Dictionary<int, int> hashNameIndexes = new Dictionary<int, int>();
    Dictionary<int, int> hashExternSheets = new Dictionary<int, int>();
    this.CopyFrom(worksheet, hashStyleNames, hashWorksheetNames, dicFontIndexes, flags, hashExtFormatIndexes, hashNameIndexes, hashExternSheets);
  }

  public void CopyFrom(
    WorksheetImpl worksheet,
    Dictionary<string, string> hashStyleNames,
    Dictionary<string, string> hashWorksheetNames,
    Dictionary<int, int> dicFontIndexes,
    OfficeWorksheetCopyFlags flags,
    Dictionary<int, int> hashExtFormatIndexes,
    Dictionary<int, int> hashNameIndexes)
  {
    this.CopyFrom(worksheet, hashStyleNames, hashWorksheetNames, dicFontIndexes, flags, hashExtFormatIndexes, hashNameIndexes, new Dictionary<int, int>(0));
  }

  public void CopyFrom(
    WorksheetImpl worksheet,
    Dictionary<string, string> hashStyleNames,
    Dictionary<string, string> hashWorksheetNames,
    Dictionary<int, int> dicFontIndexes,
    OfficeWorksheetCopyFlags flags,
    Dictionary<int, int> hashExtFormatIndexes,
    Dictionary<int, int> hashNameIndexes,
    Dictionary<int, int> hashExternSheets)
  {
    this.ParseData();
    if (worksheet.ParseDataOnDemand || worksheet.ParseOnDemand)
    {
      if (worksheet.m_dataHolder != null && worksheet.ParseDataOnDemand)
        worksheet.m_dataHolder.ParseWorksheetData(worksheet, (Dictionary<int, int>) null, worksheet.ParseDataOnDemand);
      else if (worksheet.m_dataHolder == null && worksheet.ParseOnDemand && !worksheet.IsParsed && worksheet.Parent is WorksheetsCollection)
        worksheet.ParseData((Dictionary<int, int>) null);
      if (this.Parent is WorksheetsCollection)
      {
        foreach (WorksheetImpl sheet in (CollectionBase<IWorksheet>) (this.Parent as WorksheetsCollection))
        {
          if (sheet.m_dataHolder != null && sheet.ParseDataOnDemand)
            sheet.m_dataHolder.ParseWorksheetData(sheet, (Dictionary<int, int>) null, sheet.ParseDataOnDemand);
          else if (sheet.m_dataHolder == null && sheet.ParseOnDemand && !sheet.IsParsed && sheet.Parent is WorksheetsCollection)
            sheet.ParseData((Dictionary<int, int>) null);
        }
      }
    }
    if ((flags & OfficeWorksheetCopyFlags.ClearBefore) != OfficeWorksheetCopyFlags.None)
    {
      this.ClearAll();
      flags &= ~OfficeWorksheetCopyFlags.ClearBefore;
    }
    if ((flags & OfficeWorksheetCopyFlags.CopyColumnHeight) != OfficeWorksheetCopyFlags.None)
    {
      this.CopyColumnWidth(worksheet, hashExtFormatIndexes);
      flags &= ~OfficeWorksheetCopyFlags.CopyColumnHeight;
    }
    if ((flags & OfficeWorksheetCopyFlags.CopyRowHeight) != OfficeWorksheetCopyFlags.None)
    {
      this.CopyRowHeight(worksheet, hashExtFormatIndexes);
      flags &= ~OfficeWorksheetCopyFlags.CopyRowHeight;
    }
    this.CustomHeight = worksheet.CustomHeight;
    if ((flags & OfficeWorksheetCopyFlags.CopyNames) != OfficeWorksheetCopyFlags.None)
    {
      this.CopyNames(worksheet, hashWorksheetNames, hashNameIndexes, hashExternSheets);
      flags &= ~OfficeWorksheetCopyFlags.CopyNames;
    }
    this.CopyFrom((WorksheetBaseImpl) worksheet, hashStyleNames, hashWorksheetNames, dicFontIndexes, flags, hashExtFormatIndexes);
    if ((flags & OfficeWorksheetCopyFlags.CopyCells) != OfficeWorksheetCopyFlags.None)
    {
      this.m_iFirstRow = worksheet.m_iFirstRow;
      this.m_iLastRow = worksheet.m_iLastRow;
      this.m_iFirstColumn = worksheet.m_iFirstColumn;
      this.Zoom = worksheet.Zoom;
      this.m_iLastColumn = worksheet.m_iLastColumn;
      this.m_dicRecordsCells.CopyCells(worksheet.m_dicRecordsCells, hashStyleNames, hashWorksheetNames, hashExtFormatIndexes, hashNameIndexes, dicFontIndexes, hashExternSheets);
    }
    if ((flags & OfficeWorksheetCopyFlags.CopyMerges) != OfficeWorksheetCopyFlags.None)
      this.CopyMerges(worksheet);
    if ((flags & OfficeWorksheetCopyFlags.CopyConditionlFormats) != OfficeWorksheetCopyFlags.None)
      this.CopyConditionalFormats(worksheet);
    if ((flags & OfficeWorksheetCopyFlags.CopyAutoFilters) != OfficeWorksheetCopyFlags.None)
      this.CopyAutoFilters(worksheet);
    if ((flags & OfficeWorksheetCopyFlags.CopyPageSetup) != OfficeWorksheetCopyFlags.None)
      this.CopyPageSetup(worksheet);
    if ((flags & OfficeWorksheetCopyFlags.CopyTables) == OfficeWorksheetCopyFlags.None)
      return;
    this.CopyTables(worksheet, hashWorksheetNames);
  }

  private void CopyTables(WorksheetImpl worksheet, Dictionary<string, string> hashWorksheetNames)
  {
  }

  public bool CanMove(ref IRange destination, IRange source)
  {
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    this.ParseData();
    RangeImpl rangeImpl1 = (RangeImpl) destination;
    RangeImpl rangeImpl2 = (RangeImpl) source;
    int lastRow = rangeImpl1.FirstRow + rangeImpl2.LastRow - rangeImpl2.FirstRow;
    int lastColumn = rangeImpl1.FirstColumn + rangeImpl2.LastColumn - rangeImpl2.FirstColumn;
    RangeImpl rangeImpl3;
    destination = (IRange) (rangeImpl3 = (RangeImpl) rangeImpl1.InnerWorksheet.Range[rangeImpl1.Row, rangeImpl1.Column, lastRow, lastColumn]);
    if (rangeImpl3 == rangeImpl2)
      return true;
    Dictionary<ArrayRecord, object> hashToSkip = new Dictionary<ArrayRecord, object>();
    bool formulasNotSeparated = rangeImpl2.GetAreArrayFormulasNotSeparated(hashToSkip);
    if (formulasNotSeparated)
    {
      if (rangeImpl3.Worksheet != rangeImpl2.Worksheet)
        hashToSkip.Clear();
      formulasNotSeparated = rangeImpl3.GetAreArrayFormulasNotSeparated(hashToSkip);
    }
    return formulasNotSeparated;
  }

  public bool CanInsertRow(int iRowIndex, int iRowCount, OfficeInsertOptions options)
  {
    this.ParseData();
    if (iRowIndex < 1 || iRowIndex > this.m_book.MaxRowCount || iRowCount <= 0)
      return false;
    if (this.m_iLastRow <= iRowIndex)
      return true;
    if (iRowIndex >= this.m_iFirstRow && this.m_iLastColumn <= this.m_book.MaxColumnCount && !((RangeImpl) this.Range[iRowIndex, this.m_iFirstColumn, this.m_iLastRow, this.m_iLastColumn]).AreFormulaArraysNotSeparated)
      return false;
    int num1 = this.m_iLastRow + iRowCount - this.m_book.MaxRowCount;
    if (num1 > 0)
    {
      int num2 = Math.Max(this.m_iLastRow - num1, this.m_iFirstRow);
      for (int iLastRow = this.m_iLastRow; iLastRow >= num2; --iLastRow)
      {
        if (!this.IsRowEmpty(iLastRow))
          return false;
        --this.m_iLastRow;
      }
      if (this.m_iFirstRow > this.m_iLastRow)
      {
        this.m_iLastRow = this.m_iFirstRow = -1;
        this.m_iLastColumn = this.m_iFirstColumn = int.MaxValue;
      }
    }
    return true;
  }

  public bool CanInsertColumn(int iColumnIndex, int iColumnCount, OfficeInsertOptions options)
  {
    this.ParseData();
    if (iColumnIndex < 1 || iColumnIndex > this.m_book.MaxColumnCount)
      return false;
    if (this.m_iLastColumn < iColumnIndex || this.m_iFirstColumn == int.MaxValue)
      return true;
    if (iColumnIndex >= this.m_iFirstColumn && !((RangeImpl) this.Range[this.m_iFirstRow, iColumnIndex, this.m_iLastRow, this.m_iLastColumn]).AreFormulaArraysNotSeparated)
      return false;
    int num1 = this.m_iLastColumn + iColumnCount - this.m_book.MaxColumnCount;
    if (num1 > 0)
    {
      int num2 = Math.Max(this.m_iLastColumn - num1, this.m_iFirstColumn);
      for (int iLastColumn = this.m_iLastColumn; iLastColumn >= num2; --iLastColumn)
      {
        if (!this.IsColumnEmpty(iLastColumn))
          return false;
        --this.m_iLastColumn;
      }
      if (this.m_iFirstColumn > this.m_iLastColumn)
      {
        this.m_iLastRow = this.m_iFirstRow = -1;
        this.m_iLastColumn = this.m_iFirstColumn = int.MaxValue;
      }
    }
    return true;
  }

  public IRange GetRangeByString(string strRangeValue, bool hasFormula)
  {
    if (strRangeValue == null || strRangeValue.Length == 0)
      return (IRange) null;
    Ptg[] ptgArray = (this.m_book.IsWorkbookOpening ? new FormulaUtil(this.Application, (object) this.m_book, NumberFormatInfo.InvariantInfo, this.Application.ArgumentsSeparator, this.Application.RowSeparator) : this.m_book.FormulaUtil).ParseString(strRangeValue);
    Stack<object> objectStack = new Stack<object>();
    List<IRange> collection = new List<IRange>();
    int num = 0;
    int index1 = 0;
    for (int length = ptgArray.Length; index1 < length; ++index1)
    {
      if (ptgArray[index1] is IRangeGetter)
      {
        List<IRange> rangeList = collection ?? new List<IRange>();
        IRange range = ((IRangeGetter) ptgArray[index1]).GetRange(this.Workbook, (IWorksheet) this);
        rangeList.Add(range);
        objectStack.Push((object) rangeList);
        ++num;
        collection = (List<IRange>) null;
      }
      else if (ptgArray[index1].TokenCode == FormulaToken.tCellRangeList && num > 0)
      {
        collection = (List<IRange>) objectStack.Pop();
        if (objectStack.Count > 0)
          ((List<IRange>) objectStack.Peek()).AddRange((IEnumerable<IRange>) collection);
        collection.Clear();
        --num;
      }
    }
    List<IRange> rangeList1 = (List<IRange>) objectStack.Pop();
    int count = rangeList1.Count;
    if (count == 1)
      return rangeList1[0];
    IRanges rangesCollection = rangeList1[0].Worksheet.CreateRangesCollection();
    for (int index2 = 0; index2 < count; ++index2)
      rangesCollection.Add(rangeList1[index2]);
    return (IRange) rangesCollection;
  }

  public void UpdateNamedRangeIndexes(int[] arrNewIndex)
  {
    if (arrNewIndex == null)
      throw new ArgumentNullException(nameof (arrNewIndex));
    this.ParseData();
    this.m_dicRecordsCells.UpdateNameIndexes(this.m_book, arrNewIndex);
    this.InnerShapes?.UpdateNamedRangeIndexes(arrNewIndex);
  }

  public void UpdateNamedRangeIndexes(IDictionary<int, int> dicNewIndex)
  {
    if (dicNewIndex == null)
      throw new ArgumentNullException(nameof (dicNewIndex));
    this.ParseData();
    if (this.m_dicRecordsCells != null)
      this.m_dicRecordsCells.UpdateNameIndexes(this.m_book, dicNewIndex);
    this.InnerShapes?.UpdateNamedRangeIndexes(dicNewIndex);
  }

  public int GetStringIndex(long cellIndex)
  {
    this.ParseData();
    return !(this.m_dicRecordsCells.GetCellRecord(cellIndex) is LabelSSTRecord cellRecord) ? -1 : cellRecord.SSTIndex;
  }

  public TextWithFormat GetTextWithFormat(long cellIndex)
  {
    this.ParseData();
    ICellPositionFormat cellRecord1 = this.m_dicRecordsCells.GetCellRecord(cellIndex);
    if (cellRecord1 is LabelRecord)
    {
      LabelRecord labelRecord = cellRecord1 as LabelRecord;
      this.SetString(labelRecord.Row + 1, labelRecord.Column + 1, labelRecord.Label);
    }
    return !(this.m_dicRecordsCells.GetCellRecord(cellIndex) is LabelSSTRecord cellRecord2) ? (TextWithFormat) null : this.m_book.InnerSST[cellRecord2.SSTIndex];
  }

  public object GetTextObject(long cellIndex)
  {
    this.ParseData();
    return !(this.m_dicRecordsCells.GetCellRecord(cellIndex) is LabelSSTRecord cellRecord) ? (object) null : (object) this.m_book.InnerSST[cellRecord.SSTIndex];
  }

  public ExtendedFormatImpl GetExtendedFormat(long cellIndex)
  {
    this.ParseData();
    ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(cellIndex);
    return cellRecord == null ? (ExtendedFormatImpl) null : this.m_book.InnerExtFormats[(int) cellRecord.ExtendedFormatIndex];
  }

  public void SetLabelSSTIndex(long cellIndex, int iSSTIndex)
  {
    this.ParseData();
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(cellIndex);
    int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(cellIndex);
    ICellPositionFormat cell = this.m_dicRecordsCells.GetCellRecord(rowFromCellIndex, columnFromCellIndex);
    if (iSSTIndex == -1)
    {
      if (cell != null && cell.TypeCode == TBIFFRecord.Blank)
        return;
      this.m_dicRecordsCells.SetCellRecord(rowFromCellIndex, columnFromCellIndex, (ICellPositionFormat) this.GetRecord(TBIFFRecord.Blank, rowFromCellIndex, columnFromCellIndex));
    }
    else
    {
      if (iSSTIndex < 0 || iSSTIndex >= this.m_book.InnerSST.Count)
        throw new ArgumentOutOfRangeException(nameof (iSSTIndex));
      if (cell == null || cell.TypeCode != TBIFFRecord.LabelSST)
        cell = (ICellPositionFormat) this.GetRecord(TBIFFRecord.LabelSST, rowFromCellIndex, columnFromCellIndex);
      ((LabelSSTRecord) cell).SSTIndex = iSSTIndex;
      if (rowFromCellIndex == 0 && columnFromCellIndex == 0)
        return;
      this.m_dicRecordsCells.SetCellRecord(rowFromCellIndex, columnFromCellIndex, cell);
    }
  }

  public void UpdateStringIndexes(List<int> arrNewIndexes)
  {
    if (arrNewIndexes == null)
      throw new ArgumentNullException(nameof (arrNewIndexes));
    this.ParseData();
    this.m_dicRecordsCells.UpdateStringIndexes(arrNewIndexes);
  }

  public void RemoveMergedCells(IRange range)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    this.ParseData();
    int row = range.Row;
    int column = range.Column;
    int lastRow = range.LastRow;
    int lastColumn = range.LastColumn;
    for (int firstRow = row; firstRow <= lastRow; ++firstRow)
    {
      for (int firstColumn = column; firstColumn <= lastColumn; ++firstColumn)
      {
        if (firstRow != row || firstColumn != column)
          this.m_dicRecordsCells.Remove(RangeImpl.GetCellIndex(firstColumn, firstRow));
      }
    }
  }

  public void SetActiveCell(IRange range) => this.SetActiveCell(range, true);

  public void SetActiveCell(IRange range, bool updateApplication)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    this.ParseData();
    if (updateApplication)
      this.AppImplementation.SetActiveCell(range);
    this.CreateAllSelections();
    this.ActivatePane(range);
    SelectionRecord activeSelection = this.GetActiveSelection();
    int num1 = range.Column - 1;
    int num2 = range.Row - 1;
    activeSelection.ColumnActiveCell = (ushort) num1;
    activeSelection.RowActiveCell = (ushort) num2;
    SelectionRecord.TAddr addr = new SelectionRecord.TAddr((ushort) num2, (ushort) num2, (byte) num1, (byte) num1);
    activeSelection.SetSelection(0, addr);
  }

  private void ActivatePane(IRange range)
  {
    if (!this.WindowTwo.IsFreezePanes)
      return;
    IRange splitCell = this.SplitCell;
    this.m_pane.ActivePane = this.TopLeftCell.Row >= splitCell.Row ? (this.TopLeftCell.Column < splitCell.Column ? (ushort) 1 : (ushort) 3) : (this.TopLeftCell.Column < splitCell.Column ? (ushort) 0 : (ushort) 2);
  }

  private SelectionRecord GetActiveSelection()
  {
    SelectionRecord activeSelection = (SelectionRecord) null;
    int activePane = this.m_pane != null ? (int) this.m_pane.ActivePane : 0;
    int index = 0;
    for (int count = this.m_arrSelections.Count; index < count; ++index)
    {
      SelectionRecord arrSelection = this.m_arrSelections[index];
      if ((int) arrSelection.Pane == activePane)
      {
        activeSelection = arrSelection;
        break;
      }
    }
    if (activeSelection == null && this.m_arrSelections.Count == 1)
      activeSelection = this.m_arrSelections[0];
    return activeSelection;
  }

  public IRange GetActiveCell()
  {
    this.ParseData();
    SelectionRecord activeSelection = this.GetActiveSelection();
    int num1 = 0;
    int num2 = 0;
    if (activeSelection != null)
    {
      num1 = (int) activeSelection.RowActiveCell;
      num2 = (int) activeSelection.ColumnActiveCell;
    }
    return this[num1 + 1, num2 + 1];
  }

  [CLSCompliant(false)]
  public bool IsArrayFormula(FormulaRecord formula)
  {
    return formula != null && formula.ParsedExpression != null && formula.ParsedExpression.Length != 0 && formula.ParsedExpression[0].TokenCode == FormulaToken.tExp && this.CellRecords.GetArrayRecord(formula.Row + 1, formula.Column + 1) != null;
  }

  public bool IsArrayFormula(long cellIndex)
  {
    this.ParseData();
    return this.m_dicRecordsCells.GetCellRecord(cellIndex) is FormulaRecord cellRecord && this.IsArrayFormula(cellRecord);
  }

  public double InnerGetRowHeight(int iRow, bool bRaiseEvents)
  {
    if (iRow < 1 || iRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException("Value cannot be less 1 and greater than max row index.");
    RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iRow - 1, false);
    bool flag = false;
    if (row != null)
    {
      if (row.IsHidden)
        return 0.0;
      if (this.m_bIsExportDataTable || row.IsBadFontHeight || this.CustomHeight && this.StandardHeight == (double) row.Height / 20.0 && this.Range[iRow, this.FirstColumn, iRow, this.LastColumn].CellStyle.Rotation <= 0 || row.IsWrapText && !this.Range[iRow, this.FirstColumn, iRow, this.LastColumn].IsMerged)
        return (double) row.Height / 20.0;
      if (this.FirstColumn <= this.m_book.MaxColumnCount && this.LastColumn <= this.m_book.MaxColumnCount)
      {
        if (this.Range[iRow, this.FirstColumn, iRow, this.LastColumn].CellStyle.Rotation > 0)
        {
          flag = true;
          this.AutofitRow(iRow);
        }
        double standardFontSize = this.m_book.StandardFontSize;
        for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
        {
          double size = this.Range[iRow, firstColumn].CellStyle.Font.Size;
          ICellPositionFormat cellRecord = this.CellRecords[iRow, firstColumn];
          if (size > standardFontSize || cellRecord != null && Convert.ToInt32(this.m_book.InnerExtFormats[(int) cellRecord.ExtendedFormatIndex].Record.Rotation) > 0)
          {
            flag = true;
            break;
          }
        }
      }
    }
    return flag ? (double) row.Height / 20.0 : this.StandardHeight;
  }

  public override object Clone(object parent, bool cloneShapes)
  {
    WorksheetImpl worksheetImpl = parent != null ? (WorksheetImpl) base.Clone(parent, cloneShapes) : throw new ArgumentNullException(nameof (parent));
    worksheetImpl.m_rngUsed = (RangeImpl) null;
    worksheetImpl.m_migrantRange = (IMigrantRange) null;
    worksheetImpl.m_pane = (PaneRecord) CloneUtils.CloneCloneable((ICloneable) this.m_pane);
    worksheetImpl.m_arrSortRecords = CloneUtils.CloneCloneable(this.m_arrSortRecords);
    worksheetImpl.m_arrDConRecords = CloneUtils.CloneCloneable(this.m_arrDConRecords);
    worksheetImpl.m_arrAutoFilter = CloneUtils.CloneCloneable(this.m_arrAutoFilter);
    worksheetImpl.m_arrSelections = CloneUtils.CloneCloneable<SelectionRecord>(this.m_arrSelections);
    if (this.m_arrNotes != null)
    {
      worksheetImpl.m_arrNotes = CloneUtils.CloneCloneable<int, NoteRecord>(this.m_arrNotes);
      worksheetImpl.m_arrNotesByCellIndex = CloneUtils.CloneCloneable<long, NoteRecord>(this.m_arrNotesByCellIndex);
    }
    worksheetImpl.m_arrColumnInfo = CloneUtils.CloneArray(this.m_arrColumnInfo);
    worksheetImpl.m_names = new WorksheetNamesCollection(this.Application, (object) this);
    worksheetImpl.m_pageSetup = this.m_pageSetup.Clone((object) worksheetImpl);
    worksheetImpl.m_mergedCells = (MergeCellsImpl) CloneUtils.CloneCloneable((ICloneParent) this.m_mergedCells, (object) worksheetImpl);
    worksheetImpl.m_dicRecordsCells = this.m_dicRecordsCells.Clone((object) worksheetImpl);
    worksheetImpl.m_book.InnerWorksheets.InnerAdd((IWorksheet) worksheetImpl);
    return (object) worksheetImpl;
  }

  public void ReAddAllStrings()
  {
    this.ParseData();
    this.m_dicRecordsCells.ReAddAllStrings();
  }

  public bool? GetStringPreservedValue(ICombinedRange range)
  {
    return this.m_stringPreservedRanges.GetRangeValue(range);
  }

  public void SetStringPreservedValue(ICombinedRange range, bool? value)
  {
    this.m_stringPreservedRanges.SetRange(range, value);
  }

  public override void MarkUsedReferences(bool[] usedItems)
  {
    this.m_dicRecordsCells.MarkUsedReferences(usedItems);
    IOfficeChartShapes charts = this.Charts;
    int index = 0;
    for (int count = charts.Count; index < count; ++index)
      (charts[index] as ChartImpl).MarkUsedReferences(usedItems);
  }

  public override void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    this.m_dicRecordsCells.UpdateReferenceIndexes(arrUpdatedIndexes);
    IOfficeChartShapes charts = this.Charts;
    int index = 0;
    for (int count = charts.Count; index < count; ++index)
      (charts[index] as ChartImpl).UpdateReferenceIndexes(arrUpdatedIndexes);
  }

  protected void CreateEmptyPane()
  {
    this.m_pane = (PaneRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Pane);
  }

  protected void CopyCell(IRange destCell, IRange sourceCell)
  {
    this.CopyCell(destCell, sourceCell, OfficeCopyRangeOptions.None);
  }

  protected void CopyCell(IRange destCell, IRange sourceCell, OfficeCopyRangeOptions options)
  {
    if (destCell == null)
      throw new ArgumentNullException(nameof (destCell));
    if (sourceCell == null)
      throw new ArgumentNullException(nameof (sourceCell));
    RangeImpl dest = (RangeImpl) destCell;
    RangeImpl source = (RangeImpl) sourceCell;
    if (!dest.IsSingleCell || !source.IsSingleCell)
      throw new ArgumentException("Each range argument should contain a single cell");
    dest.ExtendedFormatIndex = source.ExtendedFormatIndex;
    if (source.Record != null && source.Record is FormulaRecord)
    {
      if (sourceCell.HasFormulaArray)
        return;
      FormulaRecord record1 = (FormulaRecord) source.Record;
      FormulaRecord record2 = (FormulaRecord) record1.Clone();
      record2.Row = (int) (ushort) (destCell.Row - 1);
      record2.Column = (int) (ushort) (destCell.Column - 1);
      bool flag = (options & OfficeCopyRangeOptions.UpdateFormulas) != OfficeCopyRangeOptions.None;
      int iRowOffset = flag ? destCell.Row - sourceCell.Row : 0;
      int iColOffset = flag ? destCell.Column - sourceCell.Column : 0;
      record2.ParsedExpression = this.UpdateFormula(record1.ParsedExpression, iRowOffset, iColOffset);
      dest.SetFormula(record2);
    }
    else
      destCell.Value = sourceCell.Value;
    this.CopyComment(source, dest);
  }

  private int GetColumnCount(RangeImpl source, RangeImpl dest)
  {
    int columnCount = 0;
    if (source.Row != dest.Row)
      columnCount = dest.Column - source.Column;
    return columnCount;
  }

  private int GetRowCount(RangeImpl source, RangeImpl dest)
  {
    int rowCount = 0;
    if (source.Row != dest.Row)
      rowCount = dest.Row - source.Row;
    return rowCount;
  }

  private void CopyComment(RangeImpl source, RangeImpl dest)
  {
  }

  private void RemoveLastRow(bool bUpdateFormula) => this.RemoveLastRow(bUpdateFormula, 1);

  private void RemoveLastRow(bool bUpdateFormula, int count)
  {
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count));
    if (count == 0)
      return;
    this.ParseData();
    int num1 = this.m_dicRecordsCells.Table.LastRow + 1;
    int num2 = 0;
    int iRowIndex = num1;
    while (num2 < count)
    {
      if (iRowIndex < 0)
      {
        count = num2;
        break;
      }
      this.m_dicRecordsCells.RemoveRow(iRowIndex);
      ++num2;
      --iRowIndex;
    }
    this.m_iLastRow = this.m_dicRecordsCells.Table.LastRow + 1;
    if (this.m_iLastRow != 0)
      return;
    this.m_iFirstRow = -1;
    this.m_iLastRow = -1;
  }

  private void RemoveLastColumn(bool bUpdateFormula)
  {
    this.ParseData();
    int num1 = this.m_iLastColumn--;
    this.m_dicRecordsCells.RemoveLastColumn(num1);
    if (this.m_iFirstColumn > this.m_iLastColumn)
      this.m_iLastColumn = this.m_iFirstColumn = int.MaxValue;
    if (!bUpdateFormula)
      return;
    Rectangle rectSource = Rectangle.FromLTRB(num1, 0, this.m_book.MaxColumnCount - 1, this.m_book.MaxRowCount - 1);
    Rectangle rectDest = Rectangle.FromLTRB(num1 - 1, 0, this.m_book.MaxColumnCount - 1, this.m_book.MaxRowCount - 1);
    int num2 = this.m_book.AddSheetReference((IWorksheet) this);
    this.m_book.UpdateFormula(num2, rectSource, num2, rectDest);
  }

  private void RemoveLastColumn(bool bUpdateFormula, int count)
  {
    this.ParseData();
    int num1 = this.m_iLastColumn--;
    WorksheetImpl worksheetImpl;
    for (int index = 0; index < count && this.m_iLastColumn >= 0; --worksheetImpl.m_iLastColumn)
    {
      this.m_dicRecordsCells.RemoveLastColumn(this.m_iLastColumn + 1);
      ++index;
      worksheetImpl = this;
    }
    this.m_iLastColumn = this.m_dicRecordsCells.LastColumn + 1;
    if (this.m_iFirstColumn > this.m_iLastColumn)
      this.m_iLastColumn = this.m_iFirstColumn;
    if (!bUpdateFormula)
      return;
    Rectangle rectSource = Rectangle.FromLTRB(num1 + count - 1, 0, this.m_book.MaxColumnCount - 1, this.m_book.MaxRowCount - 1);
    Rectangle rectDest = Rectangle.FromLTRB(num1 - 1, 0, this.m_book.MaxColumnCount - 1, this.m_book.MaxRowCount - 1);
    int num2 = this.m_book.AddSheetReference((IWorksheet) this);
    this.m_book.UpdateFormula(num2, rectSource, num2, rectDest);
  }

  private void PartialClearRange(Rectangle rect)
  {
    this.ParseData();
    if (!this.m_dicRecordsCells.UseCache)
      return;
    int num1 = rect.Top + 1;
    int num2 = rect.Left + 1;
    int num3 = rect.Bottom + 1;
    int num4 = rect.Right + 1;
    for (int iRow = num1; iRow <= num3; ++iRow)
    {
      for (int iColumn = num2; iColumn <= num4; ++iColumn)
        this.m_dicRecordsCells.GetRange(iRow, iColumn)?.PartialClear();
    }
  }

  private RecordTable CacheAndRemoveFromParent(
    IRange source,
    IRange destination,
    ref int iMaxRow,
    ref int iMaxColumn,
    CellRecordCollection tableSource)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    if (tableSource == null)
      throw new ArgumentNullException(nameof (tableSource));
    WorksheetImpl parent = (WorksheetImpl) destination.Parent;
    WorksheetImpl worksheet = (WorksheetImpl) source.Worksheet;
    int iDeltaRow = destination.Row - source.Row;
    int iDeltaColumn = destination.Column - source.Column;
    int lastColumn = source.LastColumn;
    int column = source.Column;
    return tableSource.CacheAndRemove((RangeImpl) source, iDeltaRow, iDeltaColumn, ref iMaxRow, ref iMaxColumn);
  }

  private void CopyCacheInto(RecordTable source, RecordTable destination, bool bUpdateRowRecords)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    if (source.FirstRow < 0)
      return;
    int firstRow = source.FirstRow;
    for (int lastRow = source.LastRow; firstRow <= lastRow; ++firstRow)
    {
      RowStorage row1 = source.Rows[firstRow];
      RowStorage row2 = destination.Rows[firstRow];
      if (row1 == null)
      {
        if (row2 != null && row2.UsedSize == 0 && bUpdateRowRecords)
          destination.RemoveRow(firstRow);
      }
      else
      {
        RowStorage row3 = destination.Rows[firstRow];
        WorksheetHelper.AccessRow((IInternalWorksheet) this, firstRow + 1);
        if (row3 == null)
        {
          row3 = new RowStorage(firstRow, this.AppImplementation.StandardHeightInRowUnits, destination.Workbook.DefaultXFIndex);
          row3.IsFormatted = false;
          destination.SetRow(firstRow, row3);
        }
        if (bUpdateRowRecords)
          row3.CopyRowRecordFrom(row1);
        if (row1.UsedSize > 0)
        {
          WorksheetHelper.AccessColumn((IInternalWorksheet) this, row1.FirstColumn + 1);
          WorksheetHelper.AccessColumn((IInternalWorksheet) this, row1.LastColumn + 1);
          row3.InsertRowData(row1, this.Application.RowStorageAllocationBlockSize);
        }
      }
    }
  }

  private static void ClearRange(IDictionary dictionary, Rectangle rect)
  {
    if (dictionary == null)
      throw new ArgumentNullException(nameof (dictionary));
    int num1 = rect.Top + 1;
    int num2 = rect.Left + 1;
    int num3 = rect.Bottom + 1;
    int num4 = rect.Right + 1;
    for (int firstRow = num1; firstRow <= num3; ++firstRow)
    {
      for (int firstColumn = num2; firstColumn <= num4; ++firstColumn)
      {
        long cellIndex = RangeImpl.GetCellIndex(firstColumn, firstRow);
        dictionary.Remove((object) cellIndex);
      }
    }
  }

  private void UpdateArrayFormula(
    ArrayRecord array,
    IWorksheet destSheet,
    int iDeltaRow,
    int iDeltaColumn)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    WorkbookImpl workbookImpl = destSheet != null ? (WorkbookImpl) destSheet.Workbook : throw new ArgumentNullException(nameof (destSheet));
    array.Formula = workbookImpl.FormulaUtil.UpdateFormula(array.Formula, iDeltaRow, iDeltaColumn);
  }

  private RecordTable GetRecordTable(
    int iRow,
    int iColumn,
    Rectangle rectIntersection,
    RecordTable intersection,
    RecordTable rectSource)
  {
    return !UtilityMethods.Contains(rectIntersection, iColumn, iRow) ? rectSource : intersection;
  }

  private Dictionary<int, int> GetUpdatedXFIndexes(
    int iRow,
    int iColumn,
    int iRowCount,
    int iColCount,
    WorksheetImpl destSheet,
    out Dictionary<int, int> dicFontIndexes)
  {
    if (destSheet == null)
      throw new ArgumentNullException(nameof (destSheet));
    dicFontIndexes = (Dictionary<int, int>) null;
    if (this.m_book == destSheet.Workbook)
      return (Dictionary<int, int>) null;
    this.ParseData();
    dicFontIndexes = new Dictionary<int, int>();
    Dictionary<int, object> hashToAdd = new Dictionary<int, object>();
    IList<ExtendedFormatImpl> arrXFormats = (IList<ExtendedFormatImpl>) new List<ExtendedFormatImpl>();
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    int firstRow = iRow;
    for (int index1 = iRow + iRowCount; firstRow < index1; ++firstRow)
    {
      int firstColumn = iColumn;
      for (int index2 = iColumn + iColCount; firstColumn < index2; ++firstColumn)
      {
        ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(RangeImpl.GetCellIndex(firstColumn, firstRow));
        if (cellRecord != null)
        {
          int extendedFormatIndex = (int) cellRecord.ExtendedFormatIndex;
          innerExtFormats.AddIndex(hashToAdd, arrXFormats, extendedFormatIndex);
        }
      }
    }
    return destSheet.ParentWorkbook.InnerExtFormats.Merge(arrXFormats, out dicFontIndexes);
  }

  private void ClearCell(long cellIndex)
  {
    this.ParseData();
    this.m_dicRecordsCells.Remove(cellIndex);
    this.m_dicRecordsCells.GetRange(cellIndex)?.Clear();
  }

  private void SetArrayFormulaRanges(ArrayRecord array)
  {
    this.ParseData();
    Ptg ptg = FormulaUtil.CreatePtg(FormulaToken.tExp, (object) array.FirstRow, (object) array.FirstColumn);
    int lastRow = array.LastRow;
    int lastColumn = array.LastColumn;
    int firstRow = array.FirstRow;
    int firstColumn = array.FirstColumn;
    for (int index1 = firstRow; index1 <= lastRow; ++index1)
    {
      for (int index2 = firstColumn; index2 <= lastColumn; ++index2)
      {
        long cellIndex = RangeImpl.GetCellIndex(index2 + 1, index1 + 1);
        FormulaRecord record = (FormulaRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Formula);
        record.Row = index1;
        record.Column = index2;
        record.ParsedExpression = new Ptg[1]
        {
          (Ptg) ptg.Clone()
        };
        this.m_dicRecordsCells.SetCellRecord(cellIndex, (ICellPositionFormat) record);
        ((RangeImpl) this.Range[index1 + 1, index2 + 1]).UpdateRecord();
      }
    }
    this.UpdateFirstLast(firstRow + 1, firstColumn + 1);
    this.UpdateFirstLast(lastRow + 1, lastColumn + 1);
  }

  [CLSCompliant(false)]
  protected void RemoveArrayFormula(ArrayRecord record, bool bClearRange)
  {
    this.ParseData();
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    int num1 = record.FirstRow + 1;
    int num2 = record.FirstColumn + 1;
    int num3 = record.LastRow + 1;
    int num4 = record.LastColumn + 1;
    for (int iRow = num1; iRow <= num3; ++iRow)
    {
      for (int iColumn = num2; iColumn <= num4; ++iColumn)
        this.m_dicRecordsCells.SetCellRecord(iRow, iColumn, (ICellPositionFormat) null);
    }
  }

  private ArrayRecord CreateArrayFormula(
    ArrayRecord arraySource,
    IRange destination,
    IRange source,
    int iRow,
    int iColumn,
    bool bUpdateFormula)
  {
    if (arraySource == null)
      throw new ArgumentNullException(nameof (arraySource));
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    this.ParseData();
    Rectangle rectangle = Rectangle.Intersect(Rectangle.FromLTRB(arraySource.FirstRow, arraySource.FirstColumn, arraySource.LastRow, arraySource.LastColumn), Rectangle.FromLTRB(source.Row - 1, source.Column - 1, source.LastRow - 1, source.LastColumn - 1));
    if (rectangle.IsEmpty)
      throw new ArgumentNullException("Intersection is empty");
    rectangle.Offset(destination.Row - source.Row, destination.Column - source.Column);
    if (rectangle.Left < 0)
      throw new ArgumentOutOfRangeException();
    if (rectangle.Top < 0)
      throw new ArgumentOutOfRangeException();
    ArrayRecord record = (ArrayRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Array);
    record.FirstRow = iRow + destination.Row - source.Row - 1;
    record.FirstColumn = iColumn + destination.Column - source.Column - 1;
    record.LastRow = record.FirstRow - rectangle.Left + rectangle.Right;
    record.LastColumn = record.FirstColumn - rectangle.Top + rectangle.Bottom;
    record.IsRecalculateAlways = true;
    record.IsRecalculateOnOpen = true;
    int iRowOffset = bUpdateFormula ? record.FirstRow - arraySource.FirstRow : 0;
    int iColOffset = bUpdateFormula ? record.FirstColumn - arraySource.FirstColumn : 0;
    record.Formula = this.UpdateFormula(arraySource.Formula, iRowOffset, iColOffset);
    return record;
  }

  protected void CheckRangesSizes(IRange destination, IRange source)
  {
    if (destination.LastRow - destination.Row != source.LastRow - source.Row || destination.LastColumn - destination.Column != source.LastColumn - source.Column)
      throw new ArgumentException("Ranges do not fit each other");
  }

  private void CopyRangeMerges(IRange destination, IRange source)
  {
    WorksheetImpl.CopyRangeMerges(destination, source, false);
  }

  private static void CopyRangeMerges(IRange destination, IRange source, bool bDeleteSource)
  {
    RangeImpl source1 = (RangeImpl) source;
    RangeImpl destination1 = (RangeImpl) destination;
    MergeCellsImpl mergeCells1 = source1.InnerWorksheet.MergeCells;
    MergeCellsImpl mergeCells2 = destination1.InnerWorksheet.MergeCells;
    if (mergeCells1 == null)
      return;
    if (mergeCells1 == mergeCells2)
    {
      mergeCells1.CopyMoveMerges((IRange) destination1, (IRange) source1, bDeleteSource);
    }
    else
    {
      int iRowDelta = destination.Row - source.Row;
      int iColDelta = destination.Column - source.Column;
      List<MergeCellsRecord.MergedRegion> mergesToCopyMove = mergeCells1.FindMergesToCopyMove((IRange) source1, bDeleteSource);
      Rectangle range = Rectangle.FromLTRB(destination.Column - 1, destination.Row - 1, destination.LastColumn - 1, destination.LastRow - 1);
      mergeCells2.DeleteMerge(range);
      mergeCells2.AddCache(mergesToCopyMove, iRowDelta, iColDelta);
    }
  }

  [CLSCompliant(false)]
  protected internal NoteRecord GetNoteByObjectIndex(int index)
  {
    if (index < 0)
      throw new ArgumentOutOfRangeException("index < 0");
    this.ParseData();
    NoteRecord noteByObjectIndex = (NoteRecord) null;
    if (this.m_arrNotes != null)
      this.m_arrNotes.TryGetValue(index, out noteByObjectIndex);
    return noteByObjectIndex;
  }

  [CLSCompliant(false)]
  protected internal void AddNote(NoteRecord note)
  {
    this.ParseData();
    int objId = (int) note.ObjId;
    bool flag = this.m_arrNotes == null;
    if (!flag && this.m_arrNotes.ContainsKey(objId))
    {
      NoteRecord arrNote = this.m_arrNotes[objId];
      this.m_arrNotesByCellIndex.Remove(RangeImpl.GetCellIndex((int) arrNote.Column, (int) arrNote.Row));
    }
    else if (flag)
    {
      this.m_arrNotes = new SortedList<int, NoteRecord>();
      this.m_arrNotesByCellIndex = new SortedList<long, NoteRecord>();
    }
    this.m_arrNotes[objId] = note;
    this.m_arrNotesByCellIndex[RangeImpl.GetCellIndex((int) note.Column, (int) note.Row)] = note;
  }

  public void AutofitRow(int rowIndex, int firstColumn, int lastColumn, bool bRaiseEvents)
  {
    this.ParseData();
    RichTextString richText = new RichTextString(this.Application, (object) this, false, true);
    if (firstColumn == 0 || lastColumn == 0 || firstColumn > lastColumn)
      return;
    SizeF sizeF1 = new SizeF(0.0f, 0.0f);
    bool bIsMergedAndWrapped = false;
    for (int index = firstColumn; index <= lastColumn; ++index)
    {
      long cellIndex = RangeImpl.GetCellIndex(index, rowIndex);
      if (this.m_dicRecordsCells.Contains(cellIndex))
      {
        SizeF sizeF2 = this.MeasureCell(cellIndex, true, richText, false, out bIsMergedAndWrapped);
        if ((double) sizeF1.Height < (double) sizeF2.Height)
          sizeF1.Height = sizeF2.Height;
        if (this.Range[rowIndex, index].CellStyle.Rotation > 0 && (double) sizeF1.Height < (double) sizeF2.Width)
          sizeF1.Height = sizeF2.Width;
      }
    }
    if ((double) sizeF1.Height == 0.0)
      sizeF1.Height = (this.m_book.Styles["Normal"].Font as FontWrapper).Wrapped.MeasureString('0'.ToString()).Height;
    double num = ApplicationImpl.ConvertFromPixel((double) sizeF1.Height, MeasureUnits.Point);
    if (num > 409.5)
      num = 409.5;
    (this.UsedRange[rowIndex, firstColumn] as RangeImpl).SetRowHeight(num, bIsMergedAndWrapped);
  }

  internal void InnerSetRowHeight(
    int iRowIndex,
    double value,
    bool bIsBadFontHeight,
    MeasureUnits units,
    bool bRaiseEvents)
  {
    value = this.Application.ConvertUnits(value, units, MeasureUnits.Point);
    RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iRowIndex - 1, true);
    if (value == 0.0)
    {
      row.IsHidden = true;
    }
    else
    {
      ushort num = (ushort) Math.Round(value * 20.0);
      if ((int) row.Height != (int) num)
      {
        row.Height = num;
        row.IsBadFontHeight = bIsBadFontHeight;
        WorksheetHelper.AccessRow((IInternalWorksheet) this, iRowIndex);
        this.SetChanged();
      }
      if (!bRaiseEvents)
        return;
      this.RaiseRowHeightChangedEvent(iRowIndex, value);
    }
  }

  private bool IsRowEmpty(int iRowIndex) => this.IsRowEmpty(iRowIndex, true);

  private bool IsRowEmpty(int iRowIndex, bool bCheckStyle)
  {
    this.ParseData();
    if (iRowIndex < this.m_iFirstRow || iRowIndex > this.m_iLastRow)
      return true;
    int defaultXfIndex = this.m_book.DefaultXFIndex;
    for (int iFirstColumn = this.m_iFirstColumn; iFirstColumn <= this.m_iLastColumn; ++iFirstColumn)
    {
      long cellIndex = RangeImpl.GetCellIndex(iFirstColumn, iRowIndex);
      if (this.m_dicRecordsCells.Contains(cellIndex))
      {
        bool flag = true;
        ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(cellIndex);
        if (bCheckStyle && cellRecord.TypeCode == TBIFFRecord.Blank)
        {
          int extendedFormatIndex = (int) cellRecord.ExtendedFormatIndex;
          if (extendedFormatIndex == defaultXfIndex || extendedFormatIndex == 0)
            flag = false;
        }
        if (flag)
          return false;
      }
    }
    return true;
  }

  private bool IsColumnEmpty(int iColumnIndex) => this.IsColumnEmpty(iColumnIndex, true);

  private bool IsColumnEmpty(int iColumnIndex, bool bIgnoreStyles)
  {
    this.ParseData();
    if (iColumnIndex < 1 || iColumnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (iColumnIndex), "Value cannot be less 1 and greater than max column index.");
    if (iColumnIndex < this.m_iFirstColumn || iColumnIndex > this.m_iLastColumn)
      return true;
    int defaultXfIndex = this.m_book.DefaultXFIndex;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
    {
      long cellIndex = RangeImpl.GetCellIndex(iColumnIndex, iFirstRow);
      if (this.m_dicRecordsCells.Contains(cellIndex))
      {
        bool flag = true;
        ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(cellIndex);
        if (bIgnoreStyles && cellRecord.TypeCode == TBIFFRecord.Blank)
        {
          int extendedFormatIndex = (int) cellRecord.ExtendedFormatIndex;
          if (extendedFormatIndex == defaultXfIndex || extendedFormatIndex == 0)
            flag = false;
        }
        if (flag)
          return false;
      }
    }
    return true;
  }

  private int ParseRange(IRange range, string strRowString, string separator, int i)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    if (strRowString == null)
      throw new ArgumentNullException(nameof (strRowString));
    if (separator == null)
      throw new ArgumentNullException(nameof (separator));
    int length1 = strRowString.Length;
    int length2 = separator.Length;
    bool flag = true;
    int startIndex = i;
    int num1 = i;
    while (flag && num1 < length1)
    {
      if (strRowString[num1] == '"' && num1 + 1 < length1)
      {
        int num2 = strRowString.IndexOf('"', num1 + 1);
        if (num2 != -1)
          num1 = num2 + 1;
        else
          ++num1;
      }
      else if (string.CompareOrdinal(strRowString, num1, separator, 0, length2) == 0)
        flag = false;
      else
        ++num1;
    }
    int length3 = num1 - startIndex;
    string str = strRowString.Substring(startIndex, length3);
    if (length3 > 1 && str[0] == '"' && str[length3 - 1] == '"')
      str = str.Substring(1, length3 - 2);
    string s = str.Replace("\"\"", "\"");
    if (s.IndexOf('\n') >= 0)
      range.WrapText = true;
    if (s != null && s.Length > 1 && s[0] == '=' && (char.IsLetter(s, 1) || s[1] == '%' || s[1] == '('))
      range.Value = s;
    else
      range.Text = s;
    if (this.Application.PreserveCSVDataTypes)
    {
      DateTime result1;
      if (DateTime.TryParse(s, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result1))
        range.DateTime = result1;
      double result2;
      if (double.TryParse(s, out result2))
        range.Number = result2;
    }
    return Math.Min(length1, num1 + length2 - 1);
  }

  internal SizeF MeasureCell(IRange cell, bool bAutoFitRows, bool ignoreRotation)
  {
    return this.MeasureCell(RangeImpl.GetCellIndex(cell.Column, cell.Row), bAutoFitRows, ignoreRotation);
  }

  internal SizeF MeasureCell(long cellIndex, bool bAutoFitRows, bool ignoreRotation)
  {
    RichTextString richText = new RichTextString(this.Application, (object) this, false, true);
    bool bIsMergedAndWrapped = false;
    return this.MeasureCell(cellIndex, bAutoFitRows, richText, ignoreRotation, out bIsMergedAndWrapped);
  }

  private SizeF MeasureCell(
    long cellIndex,
    bool bAutoFitRows,
    RichTextString richText,
    bool ignoreRotation,
    out bool bIsMergedAndWrapped)
  {
    this.ParseData();
    this.m_dicRecordsCells.FillRTFString(cellIndex, bAutoFitRows, richText);
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(cellIndex);
    int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(cellIndex);
    bool flag = false;
    string text = richText.Text;
    if (text == null || text.Length == 0)
    {
      bIsMergedAndWrapped = false;
      return new SizeF(0.0f, 0.0f);
    }
    if (this.m_mergedCells != null)
    {
      MergeCellsRecord.MergedRegion mergedCell = this.m_mergedCells[Rectangle.FromLTRB(columnFromCellIndex - 1, rowFromCellIndex - 1, columnFromCellIndex - 1, rowFromCellIndex - 1)];
      if (mergedCell != null && (bAutoFitRows && (mergedCell.RowFrom <= rowFromCellIndex - 1 || mergedCell.RowTo >= rowFromCellIndex - 1) || !bAutoFitRows && (mergedCell.ColumnFrom <= columnFromCellIndex - 1 || mergedCell.ColumnTo >= columnFromCellIndex - 1)))
        flag = true;
    }
    FontImpl wrapped = (this.m_book.Styles["Normal"].Font as FontWrapper).Wrapped;
    ExtendedFormatImpl extendedFormat = this.GetExtendedFormat(cellIndex);
    int rotation = extendedFormat.Rotation;
    SizeF sizeF = richText.StringSize;
    int horizontalAlignment = (int) extendedFormat.HorizontalAlignment;
    if (bAutoFitRows)
    {
      if (!flag && extendedFormat.WrapText)
      {
        int wrappedCell = this.AutoFitManagerImpl.CalculateWrappedCell(extendedFormat, text, this.GetColumnWidthInPixels(columnFromCellIndex), this.AppImplementation);
        sizeF.Height = (float) wrappedCell;
      }
    }
    else
    {
      sizeF = this.UpdateAutofitByIndent(sizeF, extendedFormat);
      if (!ignoreRotation)
        sizeF.Width = this.UpdateTextWidthOrHeightByRotation(sizeF, rotation, false);
    }
    bIsMergedAndWrapped = flag && extendedFormat.WrapText;
    return sizeF;
  }

  private Size WrapLine(IWorksheet sheet, IRichTextString rtf, int columnIndex)
  {
    RichTextString richTextString = rtf as RichTextString;
    string[] strArray = richTextString.Text.Split('\n');
    int startIndex = 0;
    Size empty = Size.Empty;
    int columnWidthInPixels = sheet.GetColumnWidthInPixels(columnIndex);
    foreach (string line in strArray)
    {
      RichTextString stringPart = richTextString.Clone(richTextString.Parent) as RichTextString;
      string str = line.TrimEnd('\r');
      stringPart.Substring(startIndex, str.Length);
      Size size = this.WrapSingleLine(line, columnWidthInPixels, stringPart);
      empty.Height += size.Height;
      empty.Width = Math.Max(size.Width, empty.Width);
      startIndex += line.Length + 1;
    }
    return empty;
  }

  private Size WrapSingleLine(string line, int availableWidth, RichTextString stringPart)
  {
    Size empty = Size.Empty;
    SizeF sizeF = stringPart.StringSize;
    if ((int) sizeF.Width > availableWidth)
      sizeF = (SizeF) this.FitByWords(stringPart, availableWidth);
    empty.Height += (int) sizeF.Height;
    empty.Width = Math.Max((int) sizeF.Width, empty.Width);
    return empty;
  }

  private Size FitByWords(RichTextString stringPart, int availableWidth)
  {
    RichTextString originalString = (RichTextString) stringPart.Clone(stringPart.Parent);
    int currentIndex = 0;
    int startIndex = 0;
    int length = stringPart.Text.Length;
    Size empty = Size.Empty;
    while (startIndex < length)
    {
      stringPart = this.AddNextWord(originalString, currentIndex, ref currentIndex);
      RichTextString richTextString = (RichTextString) null;
      int num = -1;
      SizeF stringSize;
      for (stringSize = stringPart.StringSize; (double) stringSize.Width < (double) availableWidth && currentIndex < originalString.Text.Length; stringSize = stringPart.StringSize)
      {
        richTextString = stringPart;
        num = currentIndex;
        stringPart = this.AddNextWord(originalString, startIndex, ref currentIndex);
      }
      if ((double) stringSize.Width > (double) availableWidth)
      {
        stringPart = richTextString;
        currentIndex = num;
      }
      SizeF sizeF;
      if (stringPart != null)
      {
        sizeF = stringPart.StringSize;
      }
      else
      {
        currentIndex = startIndex;
        sizeF = this.SplitByChars(originalString, startIndex, ref currentIndex, availableWidth);
      }
      empty.Width = Math.Max((int) sizeF.Width, empty.Width);
      empty.Height += (int) sizeF.Height;
      startIndex = currentIndex;
      if (currentIndex == 0)
        ++startIndex;
    }
    return empty;
  }

  private SizeF SplitByChars(
    RichTextString originalString,
    int startIndex,
    ref int currentIndex,
    int availableWidth)
  {
    int length = originalString.Text.Length;
    Size size1 = Size.Empty;
    Size empty = Size.Empty;
    while (currentIndex < length && size1.Width < availableWidth)
    {
      RichTextString richTextString = (RichTextString) originalString.Clone(originalString.Parent);
      richTextString.Substring(startIndex, currentIndex - startIndex + 1);
      Size size2 = size1;
      size1 = richTextString.StringSize.ToSize();
      if (size1.Width > availableWidth)
      {
        size1 = size2;
        break;
      }
      ++currentIndex;
    }
    return (SizeF) size1;
  }

  private RichTextString AddNextWord(
    RichTextString originalString,
    int startIndex,
    ref int currentIndex)
  {
    RichTextString richTextString = (RichTextString) originalString.Clone(originalString.Parent);
    int num = richTextString.Text.IndexOfAny(new char[2]
    {
      '-',
      ' '
    }, currentIndex);
    if (num < 0)
      num = richTextString.Text.Length - 1;
    richTextString.Substring(startIndex, num - startIndex + 1);
    currentIndex = num + 1;
    return richTextString;
  }

  private SizeF UpdateAutofitByIndent(SizeF curSize, ExtendedFormatImpl format)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (format.HorizontalAlignment != OfficeHAlign.HAlignLeft && format.HorizontalAlignment != OfficeHAlign.HAlignRight && format.Rotation != 0 && format.IndentLevel == 0)
      return curSize;
    curSize.Width += (float) (format.IndentLevel * 12);
    return curSize;
  }

  private float UpdateTextWidthOrHeightByRotation(SizeF size, int rotation, bool bUpdateHeight)
  {
    switch (rotation)
    {
      case 0:
        return !bUpdateHeight ? size.Width : size.Height;
      case 90:
      case 180:
        return !bUpdateHeight ? size.Height : size.Width;
      default:
        if (rotation > 90)
          rotation -= 90;
        if (bUpdateHeight)
          rotation = 90 - rotation;
        float num = (float) Math.Sin(Math.PI / 180.0 * (double) rotation) * size.Height;
        return (float) Math.Cos(Math.PI / 180.0 * (double) rotation) * size.Width + num;
    }
  }

  private FontImpl GetFontByExtendedFormatIndex(ICellPositionFormat cellFormat, out int rotation)
  {
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    if (innerExtFormats.Count <= (int) cellFormat.ExtendedFormatIndex)
      throw new ArgumentException(nameof (cellFormat));
    ExtendedFormatImpl extendedFormatImpl = innerExtFormats[(int) cellFormat.ExtendedFormatIndex];
    rotation = extendedFormatImpl.Rotation;
    return (FontImpl) this.m_book.InnerFonts[extendedFormatImpl.FontIndex];
  }

  protected override void CopyOptions(WorksheetBaseImpl sourceSheet)
  {
    base.CopyOptions(sourceSheet);
    WorksheetImpl worksheetImpl = (WorksheetImpl) sourceSheet;
    this.IsRowColumnHeadersVisible = worksheetImpl.IsRowColumnHeadersVisible;
    this.IsStringsPreserved = worksheetImpl.IsStringsPreserved;
    this.IsGridLinesVisible = worksheetImpl.IsGridLinesVisible;
    this.m_pane = (PaneRecord) CloneUtils.CloneCloneable((ICloneable) worksheetImpl.m_pane);
  }

  protected override void OnRealIndexChanged(int iOldIndex)
  {
    if (this.m_names == null)
      return;
    this.m_names.SetSheetIndex(this.RealIndex);
  }

  private void OnInsertRowColumnComplete(int iRowIndex, int iRowCount, bool bRow)
  {
  }

  private SizeF UpdateAutoFitByAutoFilter(
    SizeF size,
    ExtendedFormatImpl format,
    CellRecordCollection col,
    long cellIndex)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (col == null)
      throw new ArgumentNullException(nameof (col));
    OfficeHAlign horizontalAlignment = format.HorizontalAlignment;
    int rotation = format.Rotation;
    switch (horizontalAlignment)
    {
      case OfficeHAlign.HAlignLeft:
      case OfficeHAlign.HAlignCenter:
        size.Width += horizontalAlignment == OfficeHAlign.HAlignLeft ? 16f : 32f;
        return size;
      case OfficeHAlign.HAlignRight:
      case OfficeHAlign.HAlignFill:
      case OfficeHAlign.HAlignCenterAcrossSelection:
        return size;
      case OfficeHAlign.HAlignJustify:
      case OfficeHAlign.HAlignDistributed:
        if (rotation > 0 && rotation < 90)
          size.Width += 16f;
        return size;
      default:
        return this.UpdateAutoFilterForGeneralAllignment(size, rotation, col, cellIndex);
    }
  }

  private SizeF UpdateAutoFilterForGeneralAllignment(
    SizeF size,
    int iRot,
    CellRecordCollection col,
    long cellIndex)
  {
    if (col == null)
      throw new ArgumentNullException(nameof (col));
    if (this.m_dicRecordsCells.ContainFormulaBoolOrError(cellIndex) || this.m_dicRecordsCells.ContainBoolOrError(cellIndex))
    {
      size.Width += 32f;
      return size;
    }
    if (iRot > 0 && iRot < 90 || iRot >= 180)
    {
      size.Width += 16f;
      return size;
    }
    if (!this.m_dicRecordsCells.ContainFormulaNumber(cellIndex) && !this.m_dicRecordsCells.ContainNumber(cellIndex) && iRot == 0)
      size.Width += 16f;
    return size;
  }

  private void CreateMigrantRange()
  {
    this.m_migrantRange = (IMigrantRange) new MigrantRangeImpl(this.Application, (IWorksheet) this);
  }

  private IStyle GetDefaultOutlineStyle(IDictionary dicOutlines, int iIndex)
  {
    IOutline outline = dicOutlines != null ? (IOutline) dicOutlines[(object) iIndex] : throw new ArgumentNullException(nameof (dicOutlines));
    return (IStyle) new ExtendedFormatWrapper(this.m_book, outline != null ? (int) outline.ExtendedFormatIndex : this.m_book.DefaultXFIndex);
  }

  private int SetDefaultRowColumnStyle(
    int iIndex,
    int iEndIndex,
    IStyle defaultStyle,
    IDictionary dicOutlines,
    WorksheetImpl.OutlineDelegate createOutline,
    bool bIsRow)
  {
    this.ParseData();
    int correctIndex = this.ConvertStyleToCorrectIndex(defaultStyle);
    for (int index = iIndex; index <= iEndIndex; ++index)
      (dicOutlines.Contains((object) index) ? (IOutline) dicOutlines[(object) index] : createOutline(index)).ExtendedFormatIndex = (ushort) correctIndex;
    return correctIndex;
  }

  private int SetDefaultRowColumnStyle(
    int iIndex,
    int iEndIndex,
    IStyle defaultStyle,
    IList outlines,
    WorksheetImpl.OutlineDelegate createOutline,
    bool bIsRow)
  {
    this.ParseData();
    int correctIndex = this.ConvertStyleToCorrectIndex(defaultStyle);
    for (int index = iIndex; index <= iEndIndex; ++index)
    {
      (outlines[index] != null ? (IOutline) outlines[index] : createOutline(index)).ExtendedFormatIndex = (ushort) correctIndex;
      this.SetCellStyle(index, (ushort) correctIndex);
    }
    return correctIndex;
  }

  private int ConvertStyleToCorrectIndex(IStyle style)
  {
    int index = style != null ? ((IXFIndex) style).XFormatIndex : throw new ArgumentNullException("defaultStyle");
    if (index == int.MinValue)
      throw new ArgumentException("defaultStyle");
    return this.m_book.InnerExtFormats[index].CreateChildFormat().Index;
  }

  private IOutline CreateColumnOutline(int iColumnIndex)
  {
    this.ParseData();
    if (iColumnIndex < 1 || iColumnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (iColumnIndex), "Column index is out of range.");
    ColumnInfoRecord record = BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo) as ColumnInfoRecord;
    record.FirstColumn = (ushort) (iColumnIndex - 1);
    record.LastColumn = (ushort) (iColumnIndex - 1);
    record.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
    WorksheetHelper.AccessColumn((IInternalWorksheet) this, iColumnIndex);
    this.m_arrColumnInfo[iColumnIndex] = record;
    return (IOutline) record;
  }

  private void CopyStylesAfterInsert(
    int iIndex,
    int iCount,
    OfficeInsertOptions options,
    bool bRow)
  {
    int indexForStyleCopy = this.GetIndexForStyleCopy(iIndex, iCount, options);
    int num1;
    int num2;
    if (!bRow)
    {
      num1 = this.m_iFirstRow;
      num2 = this.m_iLastRow;
    }
    else if (this.m_iFirstColumn == int.MaxValue)
    {
      num1 = -1;
      num2 = -1;
    }
    else
    {
      num1 = this.m_iFirstColumn;
      num2 = this.m_iLastColumn;
    }
    RowStorage sourceRow = (RowStorage) null;
    ColumnInfoRecord sourceColumn = (ColumnInfoRecord) null;
    if (indexForStyleCopy != -1)
    {
      if (bRow)
        sourceRow = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, indexForStyleCopy - 1, false);
      else
        sourceColumn = this.m_arrColumnInfo[indexForStyleCopy];
      if (num1 > 0)
      {
        for (int index1 = num1; index1 <= num2; ++index1)
        {
          ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(bRow ? RangeImpl.GetCellIndex(index1, indexForStyleCopy) : RangeImpl.GetCellIndex(indexForStyleCopy, index1));
          if (cellRecord != null && (int) cellRecord.ExtendedFormatIndex != this.m_book.DefaultXFIndex && (sourceRow == null || (int) cellRecord.ExtendedFormatIndex != (int) sourceRow.ExtendedFormatIndex))
          {
            int num3 = iIndex;
            for (int index2 = iIndex + iCount; num3 < index2; ++num3)
            {
              long key = bRow ? RangeImpl.GetCellIndex(index1, num3) : RangeImpl.GetCellIndex(num3, index1);
              BlankRecord record = (BlankRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Blank);
              record.Row = (bRow ? num3 : index1) - 1;
              record.Column = (bRow ? index1 : num3) - 1;
              record.ExtendedFormatIndex = cellRecord.ExtendedFormatIndex;
              this.m_dicRecordsCells.SetCellRecord(key, (ICellPositionFormat) record);
            }
          }
        }
      }
    }
    if (!bRow)
      return;
    int iCurIndex = iIndex;
    for (int index = iIndex + iCount; iCurIndex < index; ++iCurIndex)
      this.CopyRowColumnSettings(sourceRow, sourceColumn, bRow, indexForStyleCopy, iCurIndex, options);
  }

  private void CopyRowColumnSettings(
    RowStorage sourceRow,
    ColumnInfoRecord sourceColumn,
    bool bRow,
    int iSourceIndex,
    int iCurIndex,
    OfficeInsertOptions options)
  {
    if (options == OfficeInsertOptions.FormatDefault)
    {
      if (bRow)
      {
        RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iCurIndex - 1, false);
        if (row == null)
          return;
        row.SetDefaultRowOptions();
        row.Height = (ushort) this.AppImplementation.StandardHeightInRowUnits;
        row.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
      }
      else
      {
        ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[iCurIndex];
        if (columnInfoRecord == null)
          return;
        columnInfoRecord.FirstColumn = columnInfoRecord.LastColumn = (ushort) (iCurIndex - 1);
        columnInfoRecord.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
        columnInfoRecord.SetDefaultOptions();
      }
    }
    else
    {
      if (iSourceIndex == -1)
        return;
      if (bRow)
      {
        RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iCurIndex - 1, sourceRow != null);
        if (sourceRow == null && row != null)
        {
          row.Height = (ushort) this.AppImplementation.StandardHeightInRowUnits;
          row.SetDefaultRowOptions();
        }
        else
        {
          if (sourceRow == null)
            return;
          row.CopyRowRecordFrom(sourceRow);
        }
      }
      else
      {
        ColumnInfoRecord columnInfoRecord = (ColumnInfoRecord) CloneUtils.CloneCloneable((ICloneable) sourceColumn);
        this.m_arrColumnInfo[iCurIndex] = columnInfoRecord;
        if (columnInfoRecord == null)
          return;
        columnInfoRecord.FirstColumn = columnInfoRecord.LastColumn = (ushort) (iCurIndex - 1);
      }
    }
  }

  private int GetIndexForStyleCopy(int iIndex, int iCount, OfficeInsertOptions options)
  {
    switch (options)
    {
      case OfficeInsertOptions.FormatAsBefore:
        --iIndex;
        break;
      case OfficeInsertOptions.FormatAsAfter:
        iIndex += iCount;
        break;
      default:
        iIndex = -1;
        break;
    }
    return iIndex;
  }

  private OfficeFormatType GetFormatType(int iRow, int iColumn, bool bUseDefaultStyle)
  {
    int index;
    if (bUseDefaultStyle)
    {
      ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[iColumn];
      if (columnInfoRecord == null)
        return OfficeFormatType.General;
      index = (int) columnInfoRecord.ExtendedFormatIndex;
    }
    else
    {
      ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(iRow, iColumn);
      index = cellRecord != null ? (int) cellRecord.ExtendedFormatIndex : this.m_book.DefaultXFIndex;
    }
    return this.m_book.InnerFormats[this.m_book.InnerExtFormats[index].NumberFormatIndex].GetFormatType(1.0);
  }

  private System.Type GetType(ExcelExportType exportType, bool preserveOLEDate)
  {
    switch (exportType)
    {
      case ExcelExportType.Bool:
        return typeof (bool);
      case ExcelExportType.Number:
        return typeof (double);
      case ExcelExportType.Text:
      case ExcelExportType.Error:
      case ExcelExportType.Formula:
        return typeof (string);
      case ExcelExportType.DateTime:
        return !preserveOLEDate ? typeof (DateTime) : typeof (double);
      default:
        throw new ArgumentOutOfRangeException(nameof (exportType));
    }
  }

  internal string GetValue(ICellPositionFormat cell, bool preserveOLEDate)
  {
    if (cell == null)
      return string.Empty;
    object obj;
    switch (cell.TypeCode)
    {
      case TBIFFRecord.Formula:
        FormulaRecord formula = (FormulaRecord) cell;
        Ptg[] parsedExpression = formula.ParsedExpression;
        obj = !this.HasArrayFormula(parsedExpression) ? (object) this.GetFormula(cell.Row, cell.Column, parsedExpression, false, this.m_book.FormulaUtil, false) : (object) this.GetFormulaArray(formula);
        break;
      case TBIFFRecord.LabelSST:
        object sstContentByIndex = this.m_book.InnerSST.GetSSTContentByIndex(((LabelSSTRecord) cell).SSTIndex);
        obj = sstContentByIndex is TextWithFormat textWithFormat ? (object) textWithFormat.Text : sstContentByIndex;
        break;
      case TBIFFRecord.Blank:
        obj = (object) string.Empty;
        break;
      case TBIFFRecord.Number:
      case TBIFFRecord.RK:
        double doubleValue = ((IDoubleValue) cell).DoubleValue;
        FormatImpl innerFormat = this.m_book.InnerFormats[this.m_book.InnerExtFormats[(int) cell.ExtendedFormatIndex].NumberFormatIndex];
        if (innerFormat.FormatType == OfficeFormatType.DateTime)
        {
          obj = preserveOLEDate ? (object) DateTime.FromOADate(doubleValue).ToOADate() : (innerFormat.IsTimeFormat(doubleValue) ? (object) DateTime.FromOADate(doubleValue).ToLongTimeString() : (innerFormat.IsDateFormat(doubleValue) ? (object) DateTime.FromOADate(doubleValue).ToShortDateString() : (object) DateTime.FromOADate(doubleValue)));
          if (this.Workbook.Date1904)
          {
            double d = 1462.0 + DateTime.Parse(obj.ToString()).ToOADate();
            obj = preserveOLEDate ? (object) DateTime.FromOADate(d).ToOADate() : (innerFormat.IsTimeFormat(d) ? (object) DateTime.FromOADate(d).ToLongTimeString() : (innerFormat.IsDateFormat(d) ? (object) DateTime.FromOADate(d).ToShortDateString() : (object) DateTime.FromOADate(d)));
            break;
          }
          break;
        }
        obj = (object) doubleValue;
        break;
      case TBIFFRecord.Label:
        obj = (object) ((LabelRecord) cell).Label;
        break;
      case TBIFFRecord.BoolErr:
        BoolErrRecord boolErrRecord = (BoolErrRecord) cell;
        int boolOrError = (int) boolErrRecord.BoolOrError;
        obj = boolErrRecord.IsErrorCode ? (object) FormulaUtil.ErrorCodeToName[boolOrError] : (object) (boolErrRecord.BoolOrError != (byte) 0).ToString().ToUpper();
        break;
      case TBIFFRecord.String:
        obj = (object) ((StringRecord) cell).Value;
        break;
      default:
        throw new ArgumentException("Cannot recognize cell type.");
    }
    return obj.ToString();
  }

  private void UpdateOutlineAfterXFRemove(ICollection dictOutline, IDictionary dictFormats)
  {
    foreach (IOutline outline in (IEnumerable) dictOutline)
    {
      if (outline != null)
      {
        int extendedFormatIndex = (int) outline.ExtendedFormatIndex;
        if (dictFormats.Contains((object) extendedFormatIndex))
        {
          int dictFormat = (int) dictFormats[(object) extendedFormatIndex];
          outline.ExtendedFormatIndex = (ushort) dictFormat;
        }
      }
    }
  }

  private IRange[] ConvertCellListIntoRange(List<long> arrIndexes)
  {
    if (arrIndexes == null || arrIndexes.Count == 0)
      return (IRange[]) null;
    int count = arrIndexes.Count;
    IRange[] rangeArray = new IRange[count];
    for (int index = 0; index < count; ++index)
    {
      long arrIndex = arrIndexes[index];
      int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(arrIndex);
      int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(arrIndex);
      rangeArray[index] = this[rowFromCellIndex, columnFromCellIndex];
    }
    return rangeArray;
  }

  private IRange FindValueForNumber(
    BiffRecordRaw record,
    double findValue,
    bool bIsNumber,
    bool bIsFormulaValue)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    double num = double.MinValue;
    ICellPositionFormat cellPositionFormat = (ICellPositionFormat) record;
    if (bIsNumber)
    {
      if (record is NumberRecord)
        num = ((NumberRecord) record).Value;
      if (record is RKRecord)
        num = ((RKRecord) record).RKNumber;
    }
    if (bIsFormulaValue && record is FormulaRecord)
      num = ((FormulaRecord) record).Value;
    return num != findValue ? (IRange) null : this.Range[cellPositionFormat.Row + 1, cellPositionFormat.Column + 1];
  }

  private IRange FindValueForByteOrError(BoolErrRecord boolError, byte findValue, bool bIsError)
  {
    if (boolError == null)
      throw new ArgumentNullException(nameof (boolError));
    return bIsError == boolError.IsErrorCode && (int) boolError.BoolOrError == (int) findValue ? this.Range[boolError.Row + 1, boolError.Column + 1] : (IRange) null;
  }

  protected internal IRange InnerGetCell(int column, int row)
  {
    return this.InnerGetCell(column, row, this.GetXFIndex(row, column));
  }

  protected internal IRange InnerGetCell(int column, int row, int iXFIndex)
  {
    this.ParseData();
    IRange cell = (IRange) this.m_dicRecordsCells.GetRange(row, column);
    if (cell == null)
    {
      if (!(this.m_dicRecordsCells.GetCellRecord(row, column) is BiffRecordRaw cellRecord))
      {
        RangeImpl range = this.AppImplementation.CreateRange((object) this, column, row, column, row);
        if ((int) range.ExtendedFormatIndex != iXFIndex)
          range.ExtendedFormatIndex = (ushort) iXFIndex;
        this.m_dicRecordsCells.SetRange(row, column, range);
        cell = (IRange) range;
      }
      else
        cell = this.ConvertRecordToRange(cellRecord);
    }
    return cell;
  }

  protected internal IStyle InnerGetCellStyle(
    int column,
    int row,
    int iXFIndex,
    RangeImpl rangeImpl)
  {
    return RangeImpl.CreateTempStyleWrapperWithoutRange(rangeImpl, iXFIndex);
  }

  private IRange ConvertRecordToRange(BiffRecordRaw record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    this.ParseData();
    RangeImpl range = this.AppImplementation.CreateRange((object) this, record, false);
    this.m_dicRecordsCells.SetRange(range.CellIndex, range);
    return (IRange) range;
  }

  protected void UpdateFirstLast(int iRowIndex, int iColumnIndex)
  {
    this.ParseData();
    this.m_iFirstColumn = this.m_iFirstColumn > iColumnIndex || this.m_iFirstColumn == int.MaxValue ? (int) (ushort) iColumnIndex : this.m_iFirstColumn;
    this.m_iLastColumn = this.m_iLastColumn < iColumnIndex || this.m_iLastColumn == int.MaxValue ? (int) (ushort) iColumnIndex : this.m_iLastColumn;
    this.m_iFirstRow = this.m_iFirstRow > iRowIndex || this.m_iFirstRow < 0 ? iRowIndex : this.m_iFirstRow;
    this.m_iLastRow = this.m_iLastRow < iRowIndex || this.m_iLastRow < 0 ? iRowIndex : this.m_iLastRow;
  }

  protected internal void InnerSetCell(int column, int row, RangeImpl range)
  {
    if (!range.IsSingleCell)
      throw new ArgumentException("Range must represent single cell");
    this.ParseData();
    this.m_dicRecordsCells.SetRange(row, column, range);
  }

  [CLSCompliant(false)]
  protected internal void InnerSetCell(long cellIndex, BiffRecordRaw record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    this.ParseData();
    ICellPositionFormat cell = (ICellPositionFormat) record;
    WorksheetHelper.AccessColumn((IInternalWorksheet) this, cell.Column + 1);
    WorksheetHelper.AccessRow((IInternalWorksheet) this, cell.Row + 1);
    this.m_dicRecordsCells.SetCellRecord(cellIndex, cell);
  }

  [CLSCompliant(false)]
  protected internal void InnerSetCell(int iColumn, int iRow, BiffRecordRaw record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    this.ParseData();
    this.m_dicRecordsCells.SetCellRecord(iRow, iColumn, record as ICellPositionFormat);
  }

  protected internal void InnerGetDimensions(
    out int left,
    out int top,
    out int right,
    out int bottom)
  {
    this.ParseData();
    left = this.m_iFirstColumn;
    right = this.m_iLastColumn;
    top = this.m_iFirstRow;
    bottom = this.m_iLastRow;
  }

  protected internal void InnerGetColumnDimensions(int column, out int top, out int bottom)
  {
    this.ParseData();
    int num1 = -1;
    int num2 = -1;
    int firstRow = this.FirstRow;
    for (int lastRow = this.LastRow; firstRow <= lastRow; ++firstRow)
    {
      if (this.m_dicRecordsCells.Contains(RangeImpl.GetCellIndex(column, firstRow)))
      {
        if (num1 < firstRow)
          num1 = firstRow;
        if (num2 == -1)
          num2 = firstRow;
      }
    }
    top = num2;
    bottom = num1;
  }

  internal void UpdateLabelSSTIndexes(Dictionary<int, int> dictUpdatedIndexes, IncreaseIndex method)
  {
    this.ParseData();
    this.m_dicRecordsCells.UpdateLabelSSTIndexes(dictUpdatedIndexes, method);
  }

  private void InsertIntoDefaultColumns(
    int iColumnIndex,
    int iColumnCount,
    OfficeInsertOptions insertOptions)
  {
    this.ParseData();
    for (int maxColumnCount = this.m_book.MaxColumnCount; maxColumnCount > iColumnIndex + iColumnCount - 1; --maxColumnCount)
    {
      int index = maxColumnCount - iColumnCount;
      ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[index];
      if (columnInfoRecord != null)
        columnInfoRecord.FirstColumn = columnInfoRecord.LastColumn = (ushort) (maxColumnCount - 1);
      this.m_arrColumnInfo[maxColumnCount] = columnInfoRecord;
      this.m_arrColumnInfo[index] = (ColumnInfoRecord) null;
    }
    ColumnInfoRecord columnInfoRecord1 = (ColumnInfoRecord) null;
    switch (insertOptions)
    {
      case OfficeInsertOptions.FormatAsBefore:
        columnInfoRecord1 = this.m_arrColumnInfo[iColumnIndex - 1];
        break;
      case OfficeInsertOptions.FormatAsAfter:
        columnInfoRecord1 = this.m_arrColumnInfo[iColumnIndex + iColumnCount];
        break;
    }
    if (columnInfoRecord1 == null)
      return;
    int index1 = iColumnIndex;
    for (int index2 = iColumnIndex + iColumnCount; index1 < index2; ++index1)
    {
      columnInfoRecord1 = (ColumnInfoRecord) columnInfoRecord1.Clone();
      columnInfoRecord1.FirstColumn = columnInfoRecord1.LastColumn = (ushort) (index1 - 1);
      this.m_arrColumnInfo[index1] = columnInfoRecord1;
    }
  }

  private void RemoveFromDefaultColumns(
    int iColumnIndex,
    int iColumnCount,
    OfficeInsertOptions insertOptions)
  {
    this.ParseData();
    for (int index = iColumnIndex; index <= this.m_book.MaxColumnCount - iColumnCount; ++index)
    {
      ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[index + iColumnCount];
      if (columnInfoRecord != null)
        columnInfoRecord.FirstColumn = columnInfoRecord.LastColumn = (ushort) (index - 1);
      this.m_arrColumnInfo[index] = columnInfoRecord;
    }
    int maxColumnCount = this.m_book.MaxColumnCount;
    ColumnInfoRecord columnInfoRecord1 = (ColumnInfoRecord) CloneUtils.CloneCloneable((ICloneable) this.m_arrColumnInfo[maxColumnCount - 1]);
    this.m_arrColumnInfo[maxColumnCount] = columnInfoRecord1;
    if (columnInfoRecord1 == null)
      return;
    columnInfoRecord1.FirstColumn = columnInfoRecord1.LastColumn = (ushort) (maxColumnCount - 1);
  }

  private void GetRangeCoordinates(
    ref int firstRow,
    ref int firstColumn,
    ref int lastRow,
    ref int lastColumn)
  {
    if (this.m_bUsedRangeIncludesFormatting)
      return;
    while (firstRow <= lastRow && this.IsRowBlankOnly(firstRow))
      ++firstRow;
    while (lastRow >= firstRow && this.IsRowBlankOnly(lastRow))
      --lastRow;
    while (firstColumn <= lastColumn && this.IsColumnBlankOnly(firstColumn))
      ++firstColumn;
    while (lastColumn >= firstColumn && this.IsColumnBlankOnly(lastColumn))
      --lastColumn;
  }

  private bool IsRowBlankOnly(int rowIndex)
  {
    bool flag = true;
    for (int iFirstColumn = this.m_iFirstColumn; iFirstColumn <= this.m_iLastColumn; ++iFirstColumn)
    {
      if (this.GetCellType(rowIndex, iFirstColumn, false) != WorksheetImpl.TRangeValueType.Blank)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  private bool IsColumnBlankOnly(int columnIndex)
  {
    bool flag = true;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
    {
      if (this.GetCellType(iFirstRow, columnIndex, false) != WorksheetImpl.TRangeValueType.Blank)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  private void CreateUsedRange(int firstRow, int firstColumn, int lastRow, int lastColumn)
  {
    if (this.m_rngUsed != null && this.m_rngUsed.FirstColumn == firstColumn && this.m_rngUsed.FirstRow == firstRow && this.m_rngUsed.LastColumn == lastColumn && this.m_rngUsed.LastRow == lastRow)
    {
      this.m_rngUsed.ResetCells();
    }
    else
    {
      if (this.m_rngUsed != null)
        this.m_rngUsed.Dispose();
      this.m_rngUsed = this.AppImplementation.CreateRange((object) this, firstColumn, firstRow, lastColumn, lastRow);
    }
  }

  internal bool ClearExceptFirstCell(RangeImpl rangeImpl, bool isClearCells)
  {
    bool flag1 = false;
    bool flag2 = false;
    int num1 = rangeImpl.Column - 1;
    int num2 = rangeImpl.LastColumn - 1;
    int index = rangeImpl.Row - 1;
    for (int lastRow = rangeImpl.LastRow; index < lastRow; ++index)
    {
      RowStorage row = this.m_dicRecordsCells.Table.Rows[index];
      if (row != null)
      {
        if (!flag1)
        {
          int firstCell = row.FindFirstCell(num1, num2);
          if (firstCell <= num2)
          {
            flag1 = true;
            if (isClearCells)
              row.Remove(firstCell + 1, num2, this.AppImplementation.RowStorageAllocationBlockSize);
            int column = Math.Max(row.FirstColumn, firstCell + 1);
            if (!flag2 && rangeImpl[rangeImpl.Row, rangeImpl.Column].Value != rangeImpl[index + 1, column].Value)
            {
              rangeImpl[rangeImpl.Row, rangeImpl.Column].Value = rangeImpl[index + 1, column].Value;
              rangeImpl[rangeImpl.Row, rangeImpl.Column].CellStyle = rangeImpl[index + 1, column].CellStyle;
              rangeImpl[index + 1, column].Value = (string) null;
              flag2 = true;
              if (!isClearCells)
                return true;
            }
          }
        }
        else if (isClearCells)
          row.Remove(num1, num2, this.AppImplementation.RowStorageAllocationBlockSize);
      }
    }
    return flag1;
  }

  protected override OfficeSheetProtection PrepareProtectionOptions(OfficeSheetProtection options)
  {
    return options &= ~OfficeSheetProtection.Content;
  }

  protected override void PrepareVariables(OfficeParseOptions options, bool bSkipParsing)
  {
    base.PrepareVariables(options, bSkipParsing);
    if (this.m_arrAutoFilter != null)
      this.m_arrAutoFilter.Clear();
    if (this.m_arrDConRecords != null)
      this.m_arrDConRecords.Clear();
    this.m_iDValPos = -1;
    this.m_iCondFmtPos = -1;
    this.m_iPivotStartIndex = -1;
    this.m_iHyperlinksStartIndex = -1;
  }

  [CLSCompliant(false)]
  protected override void ParseRecord(
    BiffRecordRaw raw,
    bool bIgnoreStyles,
    Dictionary<int, int> hashNewXFormatIndexes)
  {
    if (this.m_book.HasDuplicatedNames && raw.TypeCode == TBIFFRecord.Formula)
      this.UpdateDuplicatedNameIndexes((FormulaRecord) raw);
    if (this.IsSkipParsing)
      return;
    if (UtilityMethods.IndexOf(WorksheetImpl.s_arrAutofilterRecord, raw.TypeCode) >= 0)
      this.AutoFilterRecords.Add(raw);
    if (raw is ICellPositionFormat cellPositionFormat && bIgnoreStyles)
      cellPositionFormat.ExtendedFormatIndex = (ushort) this.GetNewXFormatIndex((int) cellPositionFormat.ExtendedFormatIndex, hashNewXFormatIndexes);
    switch (raw.TypeCode)
    {
      case TBIFFRecord.Note:
        this.AddNote(raw as NoteRecord);
        break;
      case TBIFFRecord.Selection:
        this.m_arrSelections.Add((SelectionRecord) raw);
        break;
      case TBIFFRecord.Pane:
        this.m_pane = (PaneRecord) raw;
        break;
      case TBIFFRecord.DCON:
        this.DConRecords.Add(raw);
        break;
      case TBIFFRecord.DefaultColWidth:
        DefaultColWidthRecord defaultColWidthRecord = (DefaultColWidthRecord) raw;
        if (defaultColWidthRecord.Width == (ushort) 8)
          break;
        this.m_dStandardColWidth = this.m_book.WidthToFileWidth((double) defaultColWidthRecord.Width);
        break;
      case TBIFFRecord.ColumnInfo:
        this.ParseColumnInfo((ColumnInfoRecord) raw, bIgnoreStyles);
        break;
      case TBIFFRecord.Sort:
        this.SortRecords.Add(raw);
        break;
      case TBIFFRecord.PivotString:
      case TBIFFRecord.ExternalSourceInfo:
      case TBIFFRecord.Qsi:
      case TBIFFRecord.QsiSXTag:
      case TBIFFRecord.DBQueryExt:
      case TBIFFRecord.ExtString:
      case TBIFFRecord.TextQuery:
      case TBIFFRecord.Qsir:
      case TBIFFRecord.Qsif:
      case TBIFFRecord.OleDbConn:
      case TBIFFRecord.PivotViewAdditionalInfo:
      case TBIFFRecord.Feature12:
        this.PreserveExternalConnection.Add(raw);
        break;
      case TBIFFRecord.MergeCells:
        this.MergeCells.AddMerge((MergeCellsRecord) raw);
        break;
      case TBIFFRecord.CondFMT:
        if (!this.KeepRecord)
        {
          this.KeepRecord = true;
          this.m_arrRecords.Add(raw);
        }
        if (this.m_iCondFmtPos >= 0)
          break;
        this.m_iCondFmtPos = this.m_arrRecords.Count - 1;
        break;
      case TBIFFRecord.DVal:
        if (!this.KeepRecord)
        {
          this.KeepRecord = true;
          this.m_arrRecords.Add(raw);
        }
        ((DValRecord) raw).IsDataCached = false;
        if (this.m_iDValPos >= 0)
          break;
        this.m_iDValPos = this.m_arrRecords.Count - 1;
        break;
      case TBIFFRecord.HLink:
        if (!this.KeepRecord)
        {
          this.KeepRecord = true;
          this.m_arrRecords.Add(raw);
        }
        if (this.m_iHyperlinksStartIndex >= 0)
          break;
        this.m_iHyperlinksStartIndex = this.m_arrRecords.Count - 1;
        break;
      case TBIFFRecord.Row:
        this.ParseRowRecord((RowRecord) raw, bIgnoreStyles);
        break;
      case TBIFFRecord.Index:
        this.m_index = (IndexRecord) raw;
        break;
      case TBIFFRecord.CustomProperty:
        if (!this.KeepRecord)
        {
          this.KeepRecord = true;
          this.m_arrRecords.Add(raw);
        }
        if (this.m_iCustomPropertyStartIndex >= 0)
          break;
        this.m_iCustomPropertyStartIndex = this.m_arrRecords.Count - 1;
        break;
      case (TBIFFRecord) 2161:
      case (TBIFFRecord) 2162:
      case (TBIFFRecord) 2167:
        if (this.m_tableRecords == null)
          this.m_tableRecords = new List<BiffRecordRaw>();
        this.m_tableRecords.Add(raw);
        break;
      case TBIFFRecord.CondFMT12:
        if (!this.KeepRecord)
        {
          this.KeepRecord = true;
          this.m_arrRecords.Add(raw);
        }
        if (this.m_iCondFmtPos >= 0)
          break;
        this.m_iCondFmtPos = this.m_arrRecords.Count - 1;
        break;
    }
  }

  private void UpdateDuplicatedNameIndexes(FormulaRecord formula)
  {
    Ptg[] ptgArray = formula != null ? formula.ParsedExpression : throw new ArgumentNullException(nameof (formula));
    int index = 0;
    for (int length = ptgArray.Length; index < length; ++index)
    {
      Ptg ptg = ptgArray[index];
      if (FormulaUtil.IndexOf(FormulaUtil.NameXCodes, ptg.TokenCode) != -1)
      {
        NameXPtg nameXptg = (NameXPtg) ptg;
        int refIndex = (int) nameXptg.RefIndex;
        int nameIndex = (int) nameXptg.NameIndex;
        if (!this.m_book.IsLocalReference(refIndex))
        {
          ExternWorkbookImpl externWorkbook = this.m_book.ExternWorkbooks[refIndex];
          nameXptg.NameIndex = (ushort) (externWorkbook.GetNewIndex(nameIndex - 1) + 1);
        }
      }
    }
  }

  private int GetNewXFormatIndex(int iXFIndex, Dictionary<int, int> hashNewXFormatIndexes)
  {
    if (hashNewXFormatIndexes == null)
      throw new ArgumentNullException(nameof (hashNewXFormatIndexes));
    int formatIndex = (int) this.m_book.InnerExtFormatRecords[iXFIndex].FormatIndex;
    return hashNewXFormatIndexes[formatIndex];
  }

  public void Parse(TextReader streamToRead, string separator, int row, int column, bool isValid)
  {
    if (streamToRead == null)
      throw new ArgumentNullException(nameof (streamToRead));
    switch (separator)
    {
      case null:
        throw new ArgumentNullException(nameof (separator));
      case "":
        throw new ArgumentException(nameof (separator));
      default:
        int row1 = row;
        StringBuilder builder = new StringBuilder();
        int column1 = column;
        this.CustomHeight = false;
        while (streamToRead.Peek() >= 0)
        {
          string strRowString = WorksheetImpl.ReadCellValue(streamToRead, separator, builder, isValid);
          bool flag = strRowString.EndsWith("\n");
          if (flag)
            strRowString = strRowString.Remove(strRowString.Length - 1);
          if (strRowString.Length > 0)
            this.ParseRange(this.Range[row1, column1], strRowString, separator, 0);
          if (flag)
          {
            ++row1;
            column1 = column;
          }
          else
            ++column1;
        }
        break;
    }
  }

  private static string ReadCellValue(
    TextReader reader,
    string separator,
    StringBuilder builder,
    bool isValid)
  {
    builder.Length = 0;
    do
    {
      int num = reader.Read();
      if (num >= 0)
      {
        char endChar = (char) num;
        switch (endChar)
        {
          case '\n':
            builder.Append(endChar);
            goto label_6;
          case '\r':
            continue;
          case '"':
            builder.Append(endChar);
            WorksheetImpl.ReadToChar(reader, endChar, builder, separator, isValid);
            continue;
          default:
            builder.Append(endChar);
            continue;
        }
      }
      else
        break;
    }
    while (!WorksheetImpl.EndsWith(builder, separator));
label_6:
    return builder.ToString();
  }

  private static bool EndsWith(StringBuilder builder, string separator)
  {
    if (string.IsNullOrEmpty(separator))
      throw new ArgumentException(nameof (separator));
    int length1 = builder.Length;
    int length2 = separator.Length;
    bool flag = false;
    if (length1 >= length2)
    {
      flag = true;
      int index1 = length1 - 1;
      for (int index2 = length2 - 1; index2 >= 0; --index2)
      {
        if ((int) builder[index1] != (int) separator[index2])
        {
          flag = false;
          break;
        }
        --index1;
      }
    }
    return flag;
  }

  private static void ReadToChar(
    TextReader reader,
    char endChar,
    StringBuilder builder,
    string separator,
    bool isValid)
  {
    if (isValid)
      WorksheetImpl.ReadToChar(reader, endChar, builder);
    else
      WorksheetImpl.RemoveJunkChar(reader, endChar, builder, separator);
  }

  private static void RemoveJunkChar(
    TextReader reader,
    char endChar,
    StringBuilder builder,
    string separator)
  {
    char ch1 = ' ';
    bool flag1 = true;
    bool flag2 = true;
    int num;
    do
    {
      char ch2 = ch1;
      num = reader.Read();
      ch1 = (char) num;
      if ((int) ch1 == (int) endChar)
      {
        flag2 = !flag2 || ch2 == '�';
        char ch3 = (char) reader.Peek();
        if (((int) ch3 == (int) Convert.ToChar(separator) || ch3 == '\r' || ch3 == '\n') && !flag2)
        {
          flag1 = false;
          builder.Append(ch1);
        }
        else if (ch2 != '�')
          builder.Append(ch1);
      }
      else if (num > 0)
        builder.Append(ch1);
    }
    while (flag1 && num > 0);
  }

  private static void ReadToChar(TextReader reader, char endChar, StringBuilder builder)
  {
    int num;
    char ch;
    do
    {
      num = reader.Read();
      ch = (char) num;
      builder.Append(ch);
    }
    while ((int) ch != (int) endChar && num > 0 && ch != '\r');
  }

  private static int CharCount(string value, char ch)
  {
    int num = 0;
    for (int index = value.Length - 1; index >= 0; --index)
    {
      if ((int) value[index] == (int) ch)
        ++num;
    }
    return num;
  }

  protected internal override void ParseData(Dictionary<int, int> dictUpdatedSSTIndexes)
  {
    if ((this.IsParsed || this.IsParsing) && !this.ParseDataOnDemand)
      return;
    this.IsParsing = true;
    if (this.m_dataHolder == null)
    {
      if (!this.IsSkipParsing)
      {
        if (this.ParseOnDemand)
        {
          Stream stream = (Stream) new MemoryStream();
          BinaryWriter binaryWriter = new BinaryWriter(stream);
          foreach (BiffRecordRaw arrRecord in this.m_arrRecords)
          {
            int recordCode = arrRecord.RecordCode;
            int count = 0;
            byte[] data = arrRecord.Data;
            if (data != null)
              count = data.Length;
            binaryWriter.Write((short) recordCode);
            binaryWriter.Write((short) count);
            if (data != null)
              binaryWriter.Write(data, 0, count);
          }
          binaryWriter.Flush();
          if (stream.Length > 0L)
          {
            this.m_arrRecords.Clear();
            this.ParseOnDemand = false;
            this.m_book.IsWorkbookOpening = true;
            stream.Position = 0L;
            BiffReader biffReader = new BiffReader(stream);
            this.m_book.ParseWorksheetsOnDemand();
            this.m_book.IsWorkbookOpening = false;
          }
        }
        int calculationOptions = this.ExtractCalculationOptions();
        this.ReplaceSharedFormula();
        this.ExtractPageSetup(calculationOptions);
        if (this.m_iCondFmtPos >= 0)
          this.ExtractConditionalFormats(this.m_iCondFmtPos);
        if (this.m_iDValPos >= 0)
          this.ExtractDataValidation(this.m_iDValPos);
        if (this.m_iCustomPropertyStartIndex >= 0)
          this.ExtractCustomProperties(this.m_iCustomPropertyStartIndex);
      }
    }
    else
    {
      if (this.AppImplementation.IsFormulaParsed)
        this.AppImplementation.IsFormulaParsed = false;
      this.AttachEvents();
      this.m_dataHolder.ParseWorksheetData(this, dictUpdatedSSTIndexes, this.ParseDataOnDemand);
      this.AppImplementation.IsFormulaParsed = true;
    }
    if (!this.IsParsed)
      this.IsSaved = true;
    this.IsParsed = true;
    this.IsParsing = false;
  }

  private void ReplaceSharedFormula() => this.m_dicRecordsCells.ReplaceSharedFormula();

  internal void ParseColumnInfo(ColumnInfoRecord columnInfo, bool bIgnoreStyles)
  {
    if (columnInfo == null)
      throw new ArgumentNullException(nameof (columnInfo));
    if (bIgnoreStyles)
      columnInfo.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
    columnInfo.ColumnWidth = columnInfo.ColumnWidth;
    int extendedFormatIndex = (int) columnInfo.ExtendedFormatIndex;
    if ((int) columnInfo.FirstColumn != (int) columnInfo.LastColumn)
    {
      if ((int) columnInfo.LastColumn == this.m_book.MaxColumnCount)
        this.m_rawColRecord = columnInfo.Clone() as ColumnInfoRecord;
      for (int firstColumn = (int) columnInfo.FirstColumn; firstColumn <= (int) columnInfo.LastColumn; ++firstColumn)
      {
        int index = firstColumn + 1;
        ColumnInfoRecord columnInfoRecord = (ColumnInfoRecord) columnInfo.Clone();
        columnInfoRecord.FirstColumn = (ushort) firstColumn;
        columnInfoRecord.LastColumn = (ushort) firstColumn;
        this.m_arrColumnInfo[index] = columnInfoRecord;
      }
    }
    else
      this.m_arrColumnInfo[(int) columnInfo.FirstColumn + 1] = columnInfo;
  }

  internal void ParseRowRecord(RowRecord row, bool bIgnoreStyles)
  {
    if (row == null)
      throw new ArgumentNullException(nameof (row));
    if (bIgnoreStyles)
    {
      row.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
      row.IsFormatted = false;
    }
    RowStorage row1 = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, (int) row.RowNumber, true);
    row1.UpdateRowInfo(row, this.AppImplementation.UseFastRecordParsing);
    int num = (int) row.RowNumber + 1;
    if (num < this.FirstRow)
      this.FirstRow = num;
    if (num > this.LastRow)
      this.LastRow = num;
    if (this.FirstColumn == int.MaxValue)
      this.FirstColumn = 0;
    if (this.LastColumn == int.MaxValue)
      this.LastColumn = 1;
    int extendedFormatIndex = (int) row1.ExtendedFormatIndex;
    if (extendedFormatIndex > this.m_book.InnerExtFormats.Count)
    {
      row1.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
    }
    else
    {
      ExtendedFormatImpl innerExtFormat = this.m_book.InnerExtFormats[extendedFormatIndex];
      if (innerExtFormat.HasParent)
        return;
      ExtendedFormatImpl format = (ExtendedFormatImpl) innerExtFormat.Clone();
      format.ParentIndex = extendedFormatIndex;
      format.Record.XFType = ExtendedFormatRecord.TXFType.XF_STYLE;
      ExtendedFormatImpl extendedFormatImpl = this.m_book.InnerExtFormats.Add(format);
      row1.ExtendedFormatIndex = (ushort) extendedFormatImpl.Index;
    }
  }

  protected int ExtractCalculationOptions()
  {
    int num = 0;
    for (int count = this.m_arrRecords.Count; num < count; ++num)
    {
      BiffRecordRaw arrRecord = this.m_arrRecords[num];
      if (Array.IndexOf<TBIFFRecord>(CalculationOptionsImpl.DEF_CORRECT_CODES, arrRecord.TypeCode) != -1)
        return this.m_book.InnerCalculation.Parse((IList) this.m_arrRecords, num);
    }
    return 0;
  }

  protected void ExtractPageSetup(int iStartIndex)
  {
    int num = iStartIndex >= 0 ? iStartIndex : throw new ArgumentOutOfRangeException(nameof (iStartIndex));
    for (int count = this.m_arrRecords.Count; num < count; ++num)
    {
      switch (this.m_arrRecords[num].TypeCode)
      {
        case TBIFFRecord.PrintHeaders:
        case TBIFFRecord.DefaultRowHeight:
          this.m_pageSetup = new PageSetupImpl(this.Application, (object) this, this.m_arrRecords, num);
          return;
        default:
          continue;
      }
    }
  }

  protected void ExtractConditionalFormats(int iCondFmtPos)
  {
    if (iCondFmtPos < 0)
      throw new ArgumentOutOfRangeException(nameof (iCondFmtPos));
    bool flag = true;
    int num = 0;
    CondFMTRecord format1 = (CondFMTRecord) null;
    List<CFRecord> lstConditions = new List<CFRecord>();
    CondFmt12Record format2 = (CondFmt12Record) null;
    List<CF12Record> conditions = new List<CF12Record>();
    List<CFExRecord> CFExRecords = new List<CFExRecord>();
    while (flag)
    {
      BiffRecordRaw arrRecord = this.m_arrRecords[iCondFmtPos];
      switch (arrRecord.TypeCode)
      {
        case TBIFFRecord.CondFMT:
          if (format1 != null)
          {
            this.CreateFormatsCollection(format1, (IList) lstConditions, (IList) CFExRecords, false);
            lstConditions.Clear();
          }
          if (format2 != null)
          {
            this.CreateCF12RecordCollection(format2, (IList) conditions);
            conditions.Clear();
            format2 = (CondFmt12Record) null;
          }
          format1 = (CondFMTRecord) arrRecord;
          ++num;
          if (format1.Index == (ushort) 0)
            format1.Index = (ushort) num;
          if (!this.m_dictCondFMT.ContainsKey((int) format1.Index))
          {
            this.m_dictCondFMT.Add((int) format1.Index, format1);
            break;
          }
          ++num;
          format1.Index = (ushort) num;
          this.m_dictCondFMT.Add((int) format1.Index, format1);
          break;
        case TBIFFRecord.CF:
          lstConditions.Add((CFRecord) arrRecord);
          break;
        case TBIFFRecord.CondFMT12:
          if (format2 != null)
          {
            this.CreateCF12RecordCollection(format2, (IList) conditions);
            conditions.Clear();
          }
          if (format1 != null)
          {
            this.CreateFormatsCollection(format1, (IList) lstConditions, (IList) CFExRecords, false);
            lstConditions.Clear();
            format1 = (CondFMTRecord) null;
          }
          format2 = (CondFmt12Record) arrRecord;
          break;
        case TBIFFRecord.CF12:
          if (this.m_dictCFExRecords.Count > 0)
          {
            CFExRecord dictCfExRecord = this.m_dictCFExRecords[this.m_dictCFExRecords.Count - 1];
            if (dictCfExRecord.IsCF12Extends == (byte) 1)
            {
              dictCfExRecord.CF12RecordIfExtends = (CF12Record) arrRecord;
              break;
            }
            conditions.Add((CF12Record) arrRecord);
            break;
          }
          conditions.Add((CF12Record) arrRecord);
          break;
        case TBIFFRecord.CFEx:
          if (format1 != null)
          {
            this.CreateFormatsCollection(format1, (IList) lstConditions, (IList) CFExRecords, false);
            lstConditions.Clear();
            format1 = (CondFMTRecord) null;
          }
          if (format2 != null)
          {
            this.CreateCF12RecordCollection(format2, (IList) conditions);
            conditions.Clear();
            format2 = (CondFmt12Record) null;
          }
          break;
        default:
          flag = false;
          break;
      }
      ++iCondFmtPos;
    }
    if (format1 != null)
    {
      this.CreateFormatsCollection(format1, (IList) lstConditions, (IList) CFExRecords, false);
      lstConditions.Clear();
    }
    if (format2 != null)
    {
      this.CreateCF12RecordCollection(format2, (IList) conditions);
      conditions.Clear();
    }
    this.m_dictCondFMT.Clear();
    this.m_dictCFExRecords.Clear();
  }

  protected void ExtractDataValidation(int iDValPos)
  {
    if (iDValPos < 0)
      throw new ArgumentOutOfRangeException(nameof (iDValPos));
  }

  protected void ExtractCustomProperties(int iCustomPropertyPos)
  {
    if (iCustomPropertyPos < 0)
      throw new ArgumentOutOfRangeException(nameof (iCustomPropertyPos));
  }

  private void CreateFormatsCollection(
    CondFMTRecord format,
    IList lstConditions,
    IList CFExRecords,
    bool isFutureRecord)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (lstConditions == null && CFExRecords == null)
      throw new ArgumentNullException("Conditions");
  }

  private void CreateCF12RecordCollection(CondFmt12Record format, IList conditions)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (conditions == null)
      throw new ArgumentNullException(nameof (conditions));
  }

  public double InnerGetColumnWidth(int iColumn)
  {
    if (iColumn < 1)
      throw new ArgumentOutOfRangeException("iColumn can't be less then 1");
    this.ParseData();
    ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[iColumn];
    return columnInfoRecord != null ? (columnInfoRecord.IsHidden ? 0.0 : (double) columnInfoRecord.ColumnWidth / 256.0) : this.StandardWidth;
  }

  public int ColumnWidthToPixels(double widthInChars)
  {
    return (int) this.m_book.FileWidthToPixels(this.m_book.WidthToFileWidth(widthInChars));
  }

  public double PixelsToColumnWidth(int pixels) => this.m_book.PixelsToWidth((double) pixels);

  internal int EvaluateRealColumnWidth(int fileWidth)
  {
    return (int) (this.m_book.PixelsToWidth(this.m_book.FileWidthToPixels((double) fileWidth / 256.0)) * 256.0);
  }

  internal int EvaluateFileColumnWidth(int realWidth)
  {
    return (int) (this.m_book.WidthToFileWidth((double) realWidth / 256.0) * 256.0);
  }

  private void OnNameIndexChanged(object sender, NameIndexChangedEventArgs args)
  {
    throw new NotImplementedException();
  }

  internal void AttachNameIndexChangedEvent() => this.AttachNameIndexChangedEvent(0);

  internal void AttachNameIndexChangedEvent(int iStartIndex) => throw new NotImplementedException();

  public void ParseAutoFilters()
  {
  }

  [CLSCompliant(false)]
  protected internal ICellPositionFormat GetRecord(long cellIndex)
  {
    return this.m_dicRecordsCells.GetCellRecord(cellIndex);
  }

  [CLSCompliant(false)]
  protected internal ICellPositionFormat GetRecord(int iRow, int iColumn)
  {
    return this.m_dicRecordsCells.GetCellRecord(iRow, iColumn);
  }

  [CLSCompliant(false)]
  protected override void ParseDimensions(DimensionsRecord dimensions)
  {
    base.ParseDimensions(dimensions);
    this.m_dicRecordsCells.Table.EnsureSize(this.m_iLastRow);
  }

  public void SetPaneCell(IRange range)
  {
    if (range.Row != range.LastRow || range.Column != range.LastColumn)
      throw new ArgumentOutOfRangeException(nameof (range));
    this.SplitCell = range;
    this.PaneFirstVisible = range;
    this.CreateAllSelections();
  }

  private void CreateAllSelections()
  {
    int selectionCount = this.SelectionCount;
    Dictionary<int, object> usedIndexes = new Dictionary<int, object>();
    for (int index = this.m_arrSelections.Count - 1; index >= 0; --index)
      usedIndexes[(int) this.m_arrSelections[index].Pane] = (object) null;
    int currentIndex = 0;
    for (int count = this.m_arrSelections.Count; count < selectionCount; ++count)
    {
      SelectionRecord record = (SelectionRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Selection);
      currentIndex = (int) (record.Pane = (byte) this.GetFreeIndex(currentIndex, usedIndexes));
      this.m_arrSelections.Add(record);
    }
    int count1 = this.m_arrSelections.Count - selectionCount;
    if (count1 > 0)
      this.m_arrSelections.RemoveRange(selectionCount, count1);
    this.ReIndexSelections(usedIndexes);
  }

  private void ReIndexSelections(Dictionary<int, object> usedIndexes)
  {
    int num1 = 0;
    int num2 = 0;
    if (this.m_pane != null)
    {
      num1 = this.m_pane.VerticalSplit;
      num2 = this.m_pane.HorizontalSplit;
    }
    List<int> panes = new List<int>();
    Dictionary<int, object> mustPresent = new Dictionary<int, object>();
    if (num1 != 0 && num2 != 0)
    {
      this.TryAdd(mustPresent, panes, usedIndexes, 0);
      this.TryAdd(mustPresent, panes, usedIndexes, 1);
      this.TryAdd(mustPresent, panes, usedIndexes, 2);
      this.TryAdd(mustPresent, panes, usedIndexes, 3);
    }
    else if (num1 != 0)
    {
      this.TryAdd(mustPresent, panes, usedIndexes, 3);
      this.TryAdd(mustPresent, panes, usedIndexes, 1);
    }
    else if (num2 != 0)
    {
      this.TryAdd(mustPresent, panes, usedIndexes, 3);
      this.TryAdd(mustPresent, panes, usedIndexes, 2);
    }
    else
      this.TryAdd(mustPresent, panes, usedIndexes, 3);
    int index1 = 0;
    int index2 = 0;
    int count1 = this.m_arrSelections.Count;
    for (int count2 = panes.Count; index1 < count1 && index2 < count2; ++index1)
    {
      SelectionRecord arrSelection = this.m_arrSelections[index1];
      int pane = (int) arrSelection.Pane;
      if (!mustPresent.ContainsKey(pane))
      {
        arrSelection.Pane = (byte) panes[index2];
        ++index2;
      }
    }
    if (this.m_pane == null || mustPresent.ContainsKey((int) this.m_pane.ActivePane))
      return;
    this.m_pane.ActivePane = (ushort) 3;
  }

  private void TryAdd(
    Dictionary<int, object> mustPresent,
    List<int> panes,
    Dictionary<int, object> usedIndexes,
    int paneIndex)
  {
    mustPresent.Add(paneIndex, (object) null);
    if (usedIndexes.ContainsKey(paneIndex))
      return;
    panes.Add(paneIndex);
  }

  private int GetFreeIndex(int currentIndex, Dictionary<int, object> usedIndexes)
  {
    while (usedIndexes.ContainsKey(currentIndex))
      ++currentIndex;
    usedIndexes[currentIndex] = (object) null;
    return currentIndex;
  }

  public void Clear()
  {
    base.ClearAll(OfficeWorksheetCopyFlags.CopyAll);
    this.ClearData();
    if (this.m_dicRecordsCells != null)
      this.m_dicRecordsCells.Clear();
    this.m_rngUsed = (RangeImpl) null;
    this.m_iFirstColumn = int.MaxValue;
    this.m_iLastColumn = int.MaxValue;
    this.m_iFirstRow = -1;
    this.m_iLastRow = -1;
    if (this.m_mergedCells == null)
      return;
    this.m_mergedCells.Clear();
  }

  public void ClearData()
  {
    if (this.m_dicRecordsCells != null)
      this.m_dicRecordsCells.ClearData();
    if (this.m_arrColumnInfo == null)
      return;
    this.m_arrColumnInfo = (ColumnInfoRecord[]) null;
  }

  public bool Contains(int iRow, int iColumn)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Contains(RangeImpl.GetCellIndex(iColumn, iRow));
  }

  public IRanges CreateRangesCollection()
  {
    return (IRanges) this.AppImplementation.CreateRangesCollection((object) this);
  }

  public void CreateNamedRanges(string namedRange, string referRange, bool vertical)
  {
    IWorksheet worksheet = (IWorksheet) this;
    IRanges rangesCollection = worksheet.CreateRangesCollection();
    if (!vertical)
    {
      for (int row = worksheet[referRange].Row; row < worksheet[referRange].LastRow + 1; ++row)
        rangesCollection.Add(worksheet[row, worksheet[referRange].Column, row, worksheet[referRange].LastColumn]);
    }
    else
    {
      for (int column = worksheet[referRange].Column; column < worksheet[referRange].LastColumn + 1; ++column)
        rangesCollection.Add(worksheet[worksheet[referRange].Row, column, worksheet[referRange].LastRow, column]);
    }
    int index = 0;
    INames names = worksheet.Names;
    try
    {
      foreach (IRange range in (IEnumerable) worksheet[namedRange])
      {
        names.Add(range.Text).RefersToRange = rangesCollection[index];
        ++index;
      }
    }
    catch (Exception ex)
    {
      throw new InvalidRangeException("NamedRange and data count mismatch");
    }
  }

  public void ShowColumn(int columnIndex, bool isVisible)
  {
    this.ParseData();
    if (columnIndex < 0 || columnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (columnIndex), "Value cannot be less than 0 and greater than 255");
    ColumnInfoRecord record = this.m_arrColumnInfo[columnIndex];
    if (record == null)
    {
      record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
      record.FirstColumn = (ushort) (columnIndex - 1);
      record.LastColumn = (ushort) (columnIndex - 1);
      record.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
      this.m_arrColumnInfo[columnIndex] = record;
    }
    else if (isVisible && record.ColumnWidth == (ushort) 0)
      this.SetColumnWidth(columnIndex, this.StandardWidth);
    record.IsHidden = !isVisible;
    this.UpdateShapes();
  }

  public void HideColumn(int columnIndex) => this.ShowColumn(columnIndex, false);

  public void HideRow(int rowIndex) => this.ShowRow(rowIndex, false);

  public void ShowRow(int rowIndex, bool isVisible)
  {
    if (rowIndex < 1 || rowIndex > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (rowIndex));
    WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, rowIndex - 1, true).IsHidden = !isVisible;
    this.UpdateShapes();
  }

  private void UpdateShapes()
  {
    if (this.Shapes.Count == 0)
      return;
    for (int index = 0; index < this.Shapes.Count; ++index)
    {
      if (!this.Shapes[index].IsSizeWithCell)
        ((ShapeImpl) this.Shapes[index]).UpdateAnchorPoints();
    }
  }

  public void ShowRange(IRange range, bool isVisible)
  {
    bool flag1 = false;
    bool flag2 = false;
    if (range.Row < 1 || range.Row > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException("Row");
    int num1 = range.Row;
    int num2 = range.LastRow;
    if (range.LastRow - range.Row > this.m_book.MaxRowCount - (range.LastRow - range.Row) && range.LastRow == this.m_book.MaxRowCount && !isVisible)
    {
      flag1 = true;
      num1 = 1;
      num2 = range.Row - 1;
      if (num2 < this.UsedRange.LastRow)
      {
        num1 = range.Row;
        num2 = this.UsedRange.LastRow;
        flag2 = true;
      }
      this.IsZeroHeight = true;
      isVisible = true;
    }
    int num3 = num1;
    for (int index = num2; num3 <= index; ++num3)
    {
      WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, num3 - 1, true).IsHidden = flag2 ? isVisible : !isVisible;
      this.ParseData();
    }
    if (range.Column < 0 || range.Column > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException("Column", "Value cannot be less than 0 and greater than 255");
    int column = range.Column;
    for (int lastColumn = range.LastColumn; column <= lastColumn; ++column)
    {
      ColumnInfoRecord record = this.m_arrColumnInfo[column];
      if (record == null)
      {
        record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
        record.FirstColumn = (ushort) (column - 1);
        record.LastColumn = (ushort) (lastColumn - 1);
        record.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
        this.m_arrColumnInfo[column] = record;
      }
      else if (isVisible && record.ColumnWidth == (ushort) 0)
        this.SetColumnWidth(column, this.StandardWidth);
      record.IsHidden = flag1 ? isVisible : !isVisible;
    }
    this.UpdateShapes();
  }

  public void ShowRange(RangesCollection ranges, bool isVisible)
  {
    if (ranges.Count == 0)
      return;
    foreach (IRange range in ranges)
      this.ShowRange(range, isVisible);
  }

  public void ShowRange(IRange[] ranges, bool isVisible)
  {
    if (ranges.Length == 0)
      return;
    RangesCollection ranges1 = new RangesCollection(this.Application, (object) this);
    foreach (IRange range in ranges)
      ranges1.Add(range);
    this.ShowRange(ranges1, isVisible);
  }

  public bool IsColumnVisible(int columnIndex)
  {
    if (columnIndex < 1 || columnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (columnIndex), "Value cannot be less than 0 and greater than 255");
    this.ParseData();
    ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[columnIndex];
    return columnInfoRecord == null || !columnInfoRecord.IsHidden;
  }

  public bool IsRowVisible(int rowIndex)
  {
    if (rowIndex < 1 || rowIndex > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (rowIndex));
    RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, rowIndex - 1, false);
    return row == null || !row.IsHidden;
  }

  public void InsertRow(int iRowIndex)
  {
    this.InsertRow(iRowIndex, 1, OfficeInsertOptions.FormatDefault);
  }

  public void InsertRow(int iRowIndex, int iRowCount)
  {
    this.InsertRow(iRowIndex, iRowCount, OfficeInsertOptions.FormatDefault);
  }

  public void InsertRow(int iRowIndex, int iRowCount, OfficeInsertOptions insertOptions)
  {
    this.ParseData();
    if (iRowIndex < 1 || iRowIndex > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (iRowIndex));
    if (!this.CanInsertRow(iRowIndex, iRowCount, insertOptions) || !this.InnerShapes.CanInsertRowColumn(iRowIndex, iRowCount, true, this.m_book.MaxRowCount))
      throw new ArgumentException("Can't insert row");
    if (this.m_mergedCells != null)
      this.m_mergedCells.InsertRow(iRowIndex, iRowCount);
    this.m_book.InnerNamesColection.InsertRow(iRowIndex, iRowCount, this.Name);
    bool flag = iRowIndex <= this.m_iLastRow;
    if (iRowIndex + iRowCount < this.m_book.MaxRowCount)
    {
      if (!flag)
        this.m_iLastRow = iRowIndex;
      if (this.m_iFirstColumn < this.m_book.MaxColumnCount)
      {
        int row = iRowIndex;
        IRange source = this.Range[row, this.m_iFirstColumn, this.m_iLastRow, this.m_iLastColumn];
        OfficeCopyRangeOptions options = OfficeCopyRangeOptions.UpdateFormulas | OfficeCopyRangeOptions.CopyErrorIndicators | OfficeCopyRangeOptions.CopyConditionalFormats;
        this.MoveRange(this.Range[row + iRowCount, this.m_iFirstColumn, this.m_iLastRow + iRowCount, this.m_iLastColumn], source, options, true);
      }
      else
      {
        this.m_iLastRow += iRowCount;
        this.m_dicRecordsCells.Table.InsertIntoDefaultRows(iRowIndex - 1, iRowCount);
      }
    }
    if (flag)
      this.CopyStylesAfterInsert(iRowIndex, iRowCount, insertOptions, true);
    else if (insertOptions != OfficeInsertOptions.FormatDefault)
      this.CopyStylesAfterInsert(iRowIndex, iRowCount, insertOptions, true);
    this.InnerShapes.InsertRemoveRowColumn(iRowIndex, iRowCount, true, false);
  }

  public void InsertColumn(int iColumnIndex)
  {
    this.InsertColumn(iColumnIndex, 1, OfficeInsertOptions.FormatDefault);
  }

  public void InsertColumn(int iColumnIndex, int iColumnCount)
  {
    this.InsertColumn(iColumnIndex, iColumnCount, OfficeInsertOptions.FormatDefault);
  }

  public void InsertColumn(int iColumnIndex, int iColumnCount, OfficeInsertOptions insertOptions)
  {
    this.ParseData();
    if (iColumnIndex < 1 || iColumnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException("Value cannot be less 1 and greater than max column index.");
    if (!this.CanInsertColumn(iColumnIndex, iColumnCount, insertOptions) || !this.InnerShapes.CanInsertRowColumn(iColumnIndex, iColumnCount, false, this.m_book.MaxColumnCount))
      throw new ArgumentException("Can't insert column");
    if (iColumnCount < 1 || iColumnCount > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (iColumnCount), "Value cannot be less 1 and greater than max column index");
    int column = this.m_iFirstColumn;
    if (this.m_mergedCells != null)
      this.m_mergedCells.InsertColumn(iColumnIndex, iColumnCount);
    this.m_book.InnerNamesColection.InsertColumn(iColumnIndex, iColumnCount, this.Name);
    if (iColumnIndex <= this.m_iLastColumn && this.m_iFirstRow > 0 && this.m_iFirstRow <= this.m_book.MaxRowCount)
    {
      if (iColumnIndex >= column)
        column = iColumnIndex;
      IRange source = this.Range[this.m_iFirstRow, column, this.m_iLastRow, this.m_iLastColumn];
      OfficeCopyRangeOptions options = OfficeCopyRangeOptions.UpdateFormulas | OfficeCopyRangeOptions.CopyErrorIndicators | OfficeCopyRangeOptions.CopyConditionalFormats;
      this.MoveRange(this.Range[this.m_iFirstRow, column + iColumnCount], source, options, false);
      this.InsertIntoDefaultColumns(iColumnIndex, iColumnCount, insertOptions);
    }
    this.CopyStylesAfterInsert(iColumnIndex, iColumnCount, insertOptions, false);
    this.InnerShapes.InsertRemoveRowColumn(iColumnIndex, iColumnCount, false, false);
  }

  public void DeleteRow(int index) => this.DeleteRow(index, 1);

  public void DeleteRow(int index, int count)
  {
    this.ParseData();
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count));
    if (index < 1 || index > this.m_book.MaxRowCount - count + 1)
      throw new ArgumentOutOfRangeException("row index");
    RecordTable table = this.m_dicRecordsCells.Table;
    int num = table.FirstRow + 1;
    int lastRow = table.LastRow + 1;
    int iFirstColumn = this.m_iFirstColumn;
    int column = iFirstColumn > 0 ? iFirstColumn : 1;
    int iLastColumn = this.m_iLastColumn;
    int lastColumn = iLastColumn > 0 ? iLastColumn : 1;
    if (num > 0 && (column != lastColumn || column == 1 && lastColumn == 1))
    {
      RangeImpl rangeImpl = (RangeImpl) this.Range[index, column, index + count - 1, lastColumn];
      int row = index + count;
      Rectangle.FromLTRB(this.FirstColumn - 1, index - 1, this.LastColumn - 1, index + count - 2);
      if (row <= lastRow)
      {
        IRange source = this.Range[row, column, lastRow, lastColumn];
        IRange destination = this.Range[index, column];
        OfficeCopyRangeOptions options = OfficeCopyRangeOptions.UpdateFormulas | OfficeCopyRangeOptions.CopyConditionalFormats;
        IOperation beforeMove = (IOperation) new RowsClearer(this, index, count);
        this.MoveRange(destination, source, options, true, beforeMove);
      }
      if (this.m_mergedCells != null)
        this.m_mergedCells.RemoveRow(index, count);
    }
    this.m_book.InnerNamesColection.RemoveRow(index, this.Name, count);
    this.InnerShapes.InsertRemoveRowColumn(index, count, true, true);
    int count1 = Math.Min(lastRow - index + 1, count);
    if (count1 > 0)
      this.RemoveLastRow(true, count1);
    if (!this.HasSheetCalculation)
      return;
    string str = "!";
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    foreach (IName name in (IEnumerable) this.ParentWorkbook.Names)
    {
      if (name.Scope.Length > 0 && str.IndexOf($"!{name.Scope}!") > -1 && name.Value != null)
        dictionary.Add($"{name.Scope}!{name.Name}".ToUpper(), name.Value.Replace("'", ""));
      else if (name.Name != null && name.Value != null && !dictionary.ContainsKey(name.Name.ToUpper()))
        dictionary.Add(name.Name.ToUpper(), name.Value.Replace("'", ""));
    }
    Hashtable hashtable = new Hashtable();
    if (dictionary == null)
      return;
    foreach (string key in dictionary.Keys)
      hashtable.Add((object) key.ToUpper(CultureInfo.InvariantCulture), (object) dictionary[key]);
  }

  private void CopyRowRecord(int iDestRowIndex, int iSourceRowIndex)
  {
    WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iDestRowIndex, true).CopyRowRecordFrom(WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iSourceRowIndex, true));
  }

  public void DeleteColumn(int index) => this.DeleteColumn(index, 1);

  public void DeleteColumn(int index, int count)
  {
    this.ParseData();
    if (index < 1 || index > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException("column index");
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count));
    if (index + count > this.m_book.MaxColumnCount)
      count = this.m_book.MaxColumnCount - index;
    if (count == 0)
      return;
    int iFirstRow = this.m_iFirstRow;
    int iLastRow = this.m_iLastRow;
    int iFirstColumn = this.m_iFirstColumn;
    int iLastColumn = this.m_iLastColumn;
    if (iFirstRow > 0 && (iFirstColumn != iLastColumn || iFirstColumn == 1 && iLastColumn == 1))
    {
      if (!((RangeImpl) this.Range[iFirstRow, index, iLastRow, index + count - 1]).AreFormulaArraysNotSeparated)
        throw new InvalidRangeException();
      Rectangle.FromLTRB(index - 1, this.FirstRow - 1, index + count - 2, this.LastRow - 1);
      if (index < iLastColumn)
      {
        int column = index + count;
        if (column <= iLastColumn)
        {
          IRange source = this.Range[iFirstRow, column, iLastRow, iLastColumn];
          IRange destination = this.Range[iFirstRow, index];
          OfficeCopyRangeOptions options = OfficeCopyRangeOptions.UpdateFormulas | OfficeCopyRangeOptions.CopyConditionalFormats | OfficeCopyRangeOptions.CopyDataValidations;
          this.MoveRange(destination, source, options, false);
        }
      }
      if (this.m_mergedCells != null)
        this.m_mergedCells.RemoveColumn(index, count);
    }
    this.m_book.InnerNamesColection.RemoveColumn(index, this.Name, count);
    this.RemoveFromDefaultColumns(index, count, OfficeInsertOptions.FormatDefault);
    this.InnerShapes.InsertRemoveRowColumn(index, count, false, true);
    if (this.UsedRange.LastColumn < index)
      return;
    count = Math.Min(count, iLastColumn - index + 1);
    this.RemoveLastColumn(true, count);
  }

  public double GetColumnWidth(int iColumnIndex)
  {
    if (iColumnIndex < 1 || iColumnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException("Value cannot be less 1 and greater than max column index.");
    return this.InnerGetColumnWidth(iColumnIndex);
  }

  public int GetColumnWidthInPixels(int iColumnIndex)
  {
    if (iColumnIndex > this.m_book.MaxColumnCount)
      iColumnIndex = this.m_book.MaxColumnCount;
    if (iColumnIndex < 1 || iColumnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException("Value cannot be less 1 and greater than max column index.");
    return this.ColumnWidthToPixels(this.InnerGetColumnWidth(iColumnIndex));
  }

  public double GetRowHeight(int iRow) => this.InnerGetRowHeight(iRow, true);

  internal double GetInnerRowHeight(int iRow) => this.InnerGetRowHeight(iRow + 1, true);

  public int GetRowHeightInPixels(int iRowIndex)
  {
    return (int) ApplicationImpl.ConvertToPixels(this.GetRowHeight(iRowIndex), MeasureUnits.Point);
  }

  internal int GetInnerRowHeightInPixels(int iRowIndex)
  {
    return (int) ApplicationImpl.ConvertToPixels(this.GetInnerRowHeight(iRowIndex), MeasureUnits.Point);
  }

  private int ImportArray<T>(T[] arrObject, int firstRow, int firstColumn, bool isVertical)
  {
    if (arrObject == null)
      throw new ArgumentNullException(nameof (arrObject));
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentNullException(nameof (firstColumn));
    this.ParseData();
    bool flag = false;
    int index1 = 0;
    int num = !isVertical ? Math.Min(firstColumn + arrObject.Length - 1, this.m_book.MaxColumnCount) - firstColumn + 1 : Math.Min(firstRow + arrObject.Length - 1, this.m_book.MaxRowCount) - firstRow + 1;
    int iXFIndex = this.m_book.DefaultXFIndex;
    if (num > 0)
    {
      IRange cell = this.InnerGetCell(firstColumn, firstRow);
      if ((object) arrObject[index1] == null)
      {
        cell.Value2 = (object) null;
      }
      else
      {
        flag = this.IsStringsPreserved;
        this.IsStringsPreserved = arrObject[index1].GetType() == typeof (string) && !this.CheckIsFormula((object) arrObject[index1]) && this.IsStringsPreserved;
        cell.Value2 = (object) arrObject[index1];
        this.IsStringsPreserved = flag;
      }
      iXFIndex = (int) ((RangeImpl) cell).ExtendedFormatIndex;
    }
    int index2;
    for (index2 = 1; index2 < num; ++index2)
    {
      IRange range = isVertical ? this.InnerGetCell(firstColumn, firstRow + index2, iXFIndex) : this.InnerGetCell(firstColumn + index2, firstRow, iXFIndex);
      if ((object) arrObject[index2] != null)
      {
        flag = this.IsStringsPreserved;
        this.IsStringsPreserved = arrObject[index2].GetType() == typeof (string) && !this.CheckIsFormula((object) arrObject[index2]) && this.IsStringsPreserved;
      }
      range.Value2 = (object) arrObject[index2];
      this.IsStringsPreserved = flag;
    }
    return index2;
  }

  private bool CheckIsFormula(object value) => value.ToString().StartsWith("=");

  internal bool checkIsNumber(string value, CultureInfo cultureInfo)
  {
    bool flag = true;
    if (value.Contains(cultureInfo.NumberFormat.NumberDecimalSeparator))
    {
      if (new Regex($"[{cultureInfo.NumberFormat.NumberDecimalSeparator}]").Matches(value).Count > 1)
        return false;
      if (value.Contains(cultureInfo.NumberFormat.NumberGroupSeparator))
      {
        int length = value.IndexOf(cultureInfo.NumberFormat.NumberDecimalSeparator);
        string str = value.Substring(0, length);
        if (value.Substring(length + 1, value.Length - 1 - length).Contains(cultureInfo.NumberFormat.NumberGroupSeparator))
          return false;
        flag = this.checkGroupSeparatorPosition(str, cultureInfo);
      }
    }
    else
      flag = this.checkGroupSeparatorPosition(value, cultureInfo);
    return flag;
  }

  private bool checkGroupSeparatorPosition(string value, CultureInfo cultureInfo)
  {
    string input = "";
    for (int index = value.Length - 1; index >= 0; --index)
      input += (string) (object) value[index];
    MatchCollection matchCollection = new Regex($"[{cultureInfo.NumberFormat.NumberGroupSeparator}]").Matches(input);
    Regex regex;
    for (int i = 0; i < matchCollection.Count; ++i)
    {
      if ((matchCollection[i].Index - i) % 3 != 0)
      {
        regex = (Regex) null;
        return false;
      }
    }
    regex = (Regex) null;
    return true;
  }

  public int ImportArray(object[] arrObject, int firstRow, int firstColumn, bool isVertical)
  {
    return this.ImportArray<object>(arrObject, firstRow, firstColumn, isVertical);
  }

  public int ImportArray(string[] arrString, int firstRow, int firstColumn, bool isVertical)
  {
    return this.ImportArray<string>(arrString, firstRow, firstColumn, isVertical);
  }

  public int ImportArray(int[] arrInt, int firstRow, int firstColumn, bool isVertical)
  {
    return this.ImportArray<int>(arrInt, firstRow, firstColumn, isVertical);
  }

  public int ImportArray(double[] arrDouble, int firstRow, int firstColumn, bool isVertical)
  {
    return this.ImportArray<double>(arrDouble, firstRow, firstColumn, isVertical);
  }

  public int ImportArray(DateTime[] arrDateTime, int firstRow, int firstColumn, bool isVertical)
  {
    if (arrDateTime == null)
      throw new ArgumentNullException("arrObject");
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentNullException(nameof (firstColumn));
    this.ParseData();
    this.IsStringsPreserved = false;
    int index1 = 0;
    int num = !isVertical ? Math.Min(firstColumn + arrDateTime.Length - 1, this.m_book.MaxColumnCount) - firstColumn + 1 : Math.Min(firstRow + arrDateTime.Length - 1, this.m_book.MaxRowCount) - firstRow + 1;
    int iXFIndex = this.m_book.DefaultXFIndex;
    if (num > 0)
    {
      IRange range = isVertical ? this.InnerGetCell(firstColumn, firstRow) : this.InnerGetCell(firstColumn, firstRow);
      range.DateTime = arrDateTime[index1];
      iXFIndex = (int) ((RangeImpl) range).ExtendedFormatIndex;
    }
    int index2;
    for (index2 = 1; index2 < num; ++index2)
      (isVertical ? this.InnerGetCell(firstColumn, firstRow + index2, iXFIndex) : this.InnerGetCell(firstColumn + index2, firstRow, iXFIndex)).DateTime = arrDateTime[index2];
    return index2;
  }

  public int ImportArray(object[,] arrObject, int firstRow, int firstColumn)
  {
    if (arrObject == null)
      throw new ArgumentNullException(nameof (arrObject));
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentNullException(nameof (firstColumn));
    this.ParseData();
    int num = Math.Min(firstRow + arrObject.GetLength(0) - 1, this.m_book.MaxRowCount) - firstRow + 1;
    int length = Math.Min(firstColumn + arrObject.GetLength(1) - 1, this.m_book.MaxColumnCount) - firstColumn + 1;
    int[] numArray = new int[length];
    if (length <= 0 || num <= 0)
      return 0;
    for (int index = 0; index < length; ++index)
    {
      IRange cell = this.InnerGetCell(index + firstColumn, firstRow);
      if (arrObject[0, index] == null)
      {
        cell.Value2 = (object) null;
      }
      else
      {
        this.IsStringsPreserved = arrObject[0, index].GetType() == typeof (string) && !this.CheckIsFormula(arrObject[0, index]) && this.IsStringsPreserved;
        cell.Value2 = arrObject[0, index];
      }
      RangeImpl rangeImpl = (RangeImpl) cell;
      numArray[index] = (int) rangeImpl.ExtendedFormatIndex;
    }
    int index1;
    for (index1 = 1; index1 < num; ++index1)
    {
      for (int index2 = 0; index2 < length; ++index2)
      {
        IRange cell = this.InnerGetCell(firstColumn + index2, index1 + firstRow, numArray[index2]);
        if (arrObject[index1, index2] == null)
        {
          cell.Value2 = (object) null;
        }
        else
        {
          this.IsStringsPreserved = arrObject[index1, index2].GetType() == typeof (string) && !this.CheckIsFormula(arrObject[index1, index2]) && this.IsStringsPreserved;
          cell.Value2 = arrObject[index1, index2];
        }
      }
    }
    return index1;
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn)
  {
    return this.ImportDataTable(dataTable, isFieldNameShown, firstRow, firstColumn, -1, -1);
  }

  public int ImportDataTable(
    DataTable dataTable,
    int firstRow,
    int firstColumn,
    bool importOnSave)
  {
    return this.ImportDataTable(dataTable, false, firstRow, firstColumn, -1, -1, (DataColumn[]) null, false, importOnSave);
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    bool preserveTypes)
  {
    return this.ImportDataTable(dataTable, isFieldNameShown, firstRow, firstColumn, -1, -1, preserveTypes);
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns)
  {
    return this.ImportDataTable(dataTable, isFieldNameShown, firstRow, firstColumn, maxRows, maxColumns, (DataColumn[]) null, false);
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
    return this.ImportDataTable(dataTable, isFieldNameShown, firstRow, firstColumn, maxRows, maxColumns, (DataColumn[]) null, preserveTypes);
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    DataColumn[] arrDataColumns,
    bool bPreserveTypes)
  {
    return this.ImportDataTable(dataTable, isFieldNameShown, firstRow, firstColumn, maxRows, maxColumns, arrDataColumns, bPreserveTypes, false);
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    DataColumn[] arrDataColumns,
    bool bPreserveTypes,
    bool bImportOnSave)
  {
    if (dataTable == null)
      throw new ArgumentNullException(nameof (dataTable));
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (firstColumn));
    this.ParseData();
    if (bImportOnSave)
    {
      SSTDictionary innerSst = this.ParentWorkbook.InnerSST;
      this.dateTimeStyleIndex = this.ReplaceDataTable(dataTable, innerSst);
      this.m_importDTHelper = new ImportDTHelper(dataTable, firstRow, firstColumn, this.dateTimeStyleIndex);
      maxRows = Math.Min(dataTable.Rows.Count, this.m_book.MaxRowCount - firstRow);
      return maxRows;
    }
    this.m_dicRecordsCells.UpdateRows(dataTable.Rows.Count);
    this.m_book.MaxImportColumns = dataTable.Columns.Count * 18;
    if (arrDataColumns == null || arrDataColumns.Length == 0)
    {
      arrDataColumns = new DataColumn[dataTable.Columns.Count];
      dataTable.Columns.CopyTo(arrDataColumns, 0);
    }
    int count = dataTable.Rows.Count;
    if (maxRows < 0 || maxRows > count)
      maxRows = count;
    int length = arrDataColumns.Length;
    if (maxColumns < 0 || maxColumns > length)
      maxColumns = length;
    maxColumns = Math.Min(maxColumns, this.m_book.MaxColumnCount - firstColumn + 1);
    maxRows = Math.Min(maxRows, this.m_book.MaxRowCount - firstRow);
    if (isFieldNameShown)
    {
      for (int index = 0; index < maxColumns; ++index)
        this.SetText(firstRow, firstColumn + index, arrDataColumns[index].Caption);
      ++firstRow;
    }
    if (!bPreserveTypes)
      this.ImportDataTableWithoutCheck(dataTable, firstRow, firstColumn, maxRows, maxColumns, arrDataColumns, this.m_bOptimizeImport);
    else
      this.ImportDataTableWithoutCheckPreserve(dataTable, firstRow, firstColumn, maxRows, maxColumns, arrDataColumns);
    this.m_book.MaxImportColumns = 0;
    return maxRows;
  }

  internal int ReplaceDataTable(DataTable dataTable, SSTDictionary sst)
  {
    for (int index1 = 0; index1 < dataTable.Columns.Count; ++index1)
    {
      DataColumn column = dataTable.Columns[index1];
      switch (column.DataType.Name)
      {
        case "String":
          for (int index2 = 0; index2 < dataTable.Rows.Count; ++index2)
          {
            DataRow row = dataTable.Rows[index2];
            object obj = row[column];
            int num = sst.AddIncrease((object) obj.ToString());
            row[index1] = (object) num;
          }
          break;
        case "DateTime":
          if (this.format == null)
          {
            this.format = (this.Workbook as WorkbookImpl).CreateExtFormat(true) as ExtendedFormatImpl;
            this.format.NumberFormatIndex = 14;
            this.dateTimeStyleIndex = this.format.ParentIndex + 1;
            break;
          }
          break;
      }
    }
    return this.dateTimeStyleIndex;
  }

  public int ImportDataTable(DataTable dataTable, IName namedRange, bool isFieldNameShown)
  {
    return this.ImportDataTable(dataTable, namedRange, isFieldNameShown, 0, 0);
  }

  public int ImportDataTable(
    DataTable dataTable,
    IName namedRange,
    bool isFieldNameShown,
    int rowOffset,
    int columnOffset)
  {
    return this.ImportDataTable(dataTable, namedRange, isFieldNameShown, rowOffset, columnOffset, -1, -1);
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
    return this.ImportDataTable(dataTable, namedRange, isFieldNameShown, rowOffset, columnOffset, iMaxRow, iMaxCol, false);
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
    if (dataTable == null)
      throw new ArgumentNullException(nameof (dataTable));
    if (namedRange == null)
      throw new ArgumentNullException(nameof (namedRange));
    if (rowOffset < 0)
      throw new ArgumentOutOfRangeException(nameof (rowOffset));
    if (columnOffset < 0)
      throw new ArgumentOutOfRangeException(nameof (columnOffset));
    this.ParseData();
    IRange refersToRange = namedRange.RefersToRange;
    if (!(refersToRange is RangeImpl))
      throw new NotSupportedException("Doesnot support range collection as named range.");
    int count1 = dataTable.Rows.Count;
    if (iMaxRow < 0 || iMaxRow > count1)
      iMaxRow = count1;
    int count2 = dataTable.Columns.Count;
    if (iMaxCol < 0 || iMaxCol > count2)
      iMaxCol = count2;
    int num1 = refersToRange.LastColumn - refersToRange.Column + 1 - columnOffset - iMaxCol;
    int num2 = refersToRange.LastRow - refersToRange.Row + 1 - rowOffset - iMaxRow;
    if (isFieldNameShown)
      --num2;
    if (num2 < 0 || num1 < 0)
      throw new NotSupportedException("Bounds of data table is greatfull than bounds of named range.");
    WorksheetImpl worksheet = (WorksheetImpl) refersToRange.Worksheet;
    DataColumn[] dataColumnArray = new DataColumn[count2];
    dataTable.Columns.CopyTo(dataColumnArray, 0);
    if (isFieldNameShown)
    {
      for (int index = 0; index < iMaxCol; ++index)
        refersToRange[refersToRange.Row + rowOffset, refersToRange.Column + columnOffset + index].Value2 = (object) dataColumnArray[index].Caption;
      ++rowOffset;
    }
    if (bPreserveTypes)
      worksheet.ImportDataTableWithoutCheckPreserve(dataTable, refersToRange.Row + rowOffset, refersToRange.Column + columnOffset, iMaxRow, iMaxCol, dataColumnArray);
    else
      worksheet.ImportDataTableWithoutCheck(dataTable, refersToRange.Row + rowOffset, refersToRange.Column + columnOffset, iMaxRow, iMaxCol, dataColumnArray, this.m_bOptimizeImport);
    return iMaxRow;
  }

  public int ImportDataColumn(
    DataColumn dataColumn,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn)
  {
    if (dataColumn == null)
      throw new ArgumentNullException(nameof (dataColumn));
    return this.ImportDataColumns(new DataColumn[1]
    {
      dataColumn
    }, isFieldNameShown, firstRow, firstColumn);
  }

  public int ImportDataColumns(
    DataColumn[] arrDataColumns,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn)
  {
    if (arrDataColumns == null)
      throw new ArgumentNullException(nameof (arrDataColumns));
    if (arrDataColumns.Length == 0)
      throw new ArgumentException("arrDataColumns can't be empty");
    return this.ImportDataTable(arrDataColumns[0].Table, isFieldNameShown, firstRow, firstColumn, -1, -1, arrDataColumns, false);
  }

  public int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn)
  {
    return this.ImportDataView(dataView, isFieldNameShown, firstRow, firstColumn, false);
  }

  public int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    bool bPreserveTypes)
  {
    if (dataView == null)
      throw new ArgumentNullException(nameof (dataView));
    return this.ImportDataView(dataView, isFieldNameShown, firstRow, firstColumn, dataView.Count, dataView.Table.Columns.Count, bPreserveTypes);
  }

  public int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns)
  {
    return this.ImportDataView(dataView, isFieldNameShown, firstRow, firstColumn, maxRows, maxColumns, false);
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
    if (dataView == null)
      throw new ArgumentNullException(nameof (dataView));
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (firstColumn));
    this.ParseData();
    DataColumnCollection columns = dataView.Table.Columns;
    int count1 = dataView.Count;
    if (maxRows < 0 || maxRows > count1)
      maxRows = count1;
    int count2 = columns.Count;
    if (maxColumns < 0 || maxColumns > count2)
      maxColumns = count2;
    maxColumns = Math.Min(maxColumns, this.m_book.MaxColumnCount - firstColumn + 1);
    if (isFieldNameShown)
    {
      for (int index = 0; index < maxColumns; ++index)
        this.Range[firstRow, firstColumn + index].Value2 = (object) columns[index].Caption;
      ++firstRow;
    }
    maxRows = Math.Min(maxRows, this.m_book.MaxRowCount - firstRow + 1);
    if (!bPreserveTypes)
      this.ImportDataViewWithoutCheck(dataView, firstRow, firstColumn, maxRows, maxColumns);
    else
      this.ImportDataViewWithoutCheckPreserve(dataView, firstRow, firstColumn, maxRows, maxColumns);
    return maxRows;
  }

  public int ImportData(IEnumerable arrObject, int firstRow, int firstColumn, bool includeHeader)
  {
    if (arrObject == null)
      throw new ArgumentNullException(nameof (arrObject));
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentNullException(nameof (firstColumn));
    IEnumerator enumerator = arrObject.GetEnumerator();
    if (enumerator == null)
      return 0;
    bool flag = false;
    List<PropertyInfo> propertyInfo = (List<PropertyInfo>) null;
    int num = 0;
    enumerator.MoveNext();
    object current1 = enumerator.Current;
    if (current1 == null)
      return 0;
    System.Type type = current1.GetType();
    if (type.Namespace == null || type.Namespace != null && !type.Namespace.Contains("System"))
      flag = true;
    if (!flag)
      return 0;
    List<TypeCode> objectMembersInfo = this.GetObjectMembersInfo(current1, out propertyInfo);
    if (includeHeader)
    {
      for (int index = 0; index < propertyInfo.Count; ++index)
        this.SetText(firstRow, firstColumn + index, propertyInfo[index].Name);
      ++firstRow;
    }
    for (int index = 0; index < propertyInfo.Count; ++index)
    {
      if (objectMembersInfo[index] == TypeCode.Object)
      {
        System.Type propertyType = propertyInfo[index].PropertyType;
        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof (Nullable<>))
        {
          switch (propertyType.GetGenericArguments()[0].Name)
          {
            case "Int32":
              objectMembersInfo[index] = TypeCode.Int32;
              continue;
            case "Int64":
              objectMembersInfo[index] = TypeCode.Int64;
              continue;
            case "Int16":
              objectMembersInfo[index] = TypeCode.Int16;
              continue;
            case "Decimal":
              objectMembersInfo[index] = TypeCode.Decimal;
              continue;
            case "Double":
              objectMembersInfo[index] = TypeCode.Double;
              continue;
            case "DateTime":
              objectMembersInfo[index] = TypeCode.DateTime;
              continue;
            case "Boolean":
              objectMembersInfo[index] = TypeCode.Boolean;
              continue;
            default:
              continue;
          }
        }
      }
    }
    IMigrantRange migrantRange = this.MigrantRange;
    do
    {
      object current2 = enumerator.Current;
      if (current2 != null)
      {
        for (int index = 0; index < propertyInfo.Count; ++index)
        {
          PropertyInfo strProperty = propertyInfo[index];
          migrantRange.ResetRowColumn(firstRow + num, firstColumn + index);
          object valueFromProperty1 = this.GetValueFromProperty(current2, strProperty);
          if (valueFromProperty1 != null)
          {
            switch (objectMembersInfo[index])
            {
              case TypeCode.Boolean:
                migrantRange.SetValue((bool) valueFromProperty1);
                continue;
              case TypeCode.Int16:
                migrantRange.SetValue((int) Convert.ToInt16(valueFromProperty1));
                continue;
              case TypeCode.Int32:
                migrantRange.SetValue((int) valueFromProperty1);
                continue;
              case TypeCode.Int64:
              case TypeCode.Decimal:
                migrantRange.SetValue(Convert.ToDouble(valueFromProperty1));
                continue;
              case TypeCode.Double:
                migrantRange.SetValue((double) valueFromProperty1);
                continue;
              case TypeCode.DateTime:
                migrantRange.SetValue((DateTime) valueFromProperty1);
                continue;
              case TypeCode.String:
                string valueFromProperty2 = (string) this.GetValueFromProperty(current2, strProperty);
                if (valueFromProperty2 != null && valueFromProperty2.Length != 0)
                {
                  migrantRange.SetValue(valueFromProperty2);
                  continue;
                }
                continue;
              default:
                migrantRange.SetValue(valueFromProperty1.ToString());
                continue;
            }
          }
        }
        ++num;
      }
    }
    while (enumerator.MoveNext());
    return num;
  }

  private List<TypeCode> GetObjectMembersInfo(object obj, out List<PropertyInfo> propertyInfo)
  {
    System.Type type = obj.GetType();
    List<TypeCode> objectMembersInfo = new List<TypeCode>();
    propertyInfo = new List<PropertyInfo>();
    foreach (PropertyInfo property in type.GetProperties())
    {
      propertyInfo.Add(property);
      objectMembersInfo.Add(System.Type.GetTypeCode(property.PropertyType));
    }
    return objectMembersInfo;
  }

  private object GetValueFromProperty(object value, PropertyInfo strProperty)
  {
    value = !(strProperty == (PropertyInfo) null) ? strProperty.GetValue(value, (object[]) null) : throw new ArgumentOutOfRangeException("Can't find property");
    return value;
  }

  public void RemovePanes()
  {
    this.ParseData();
    this.WindowTwo.IsFreezePanes = false;
    this.WindowTwo.IsFreezePanesNoSplit = false;
    this.m_pane = (PaneRecord) null;
  }

  public IRange IntersectRanges(IRange range1, IRange range2)
  {
    if (range1 == null)
      throw new ArgumentNullException(nameof (range1));
    if (range1 == null)
      throw new ArgumentNullException(nameof (range2));
    if (range1.Parent != range2.Parent)
      return (IRange) null;
    Rectangle rectangle = Rectangle.Intersect(Rectangle.FromLTRB(range1.Column, range1.Row, range1.LastColumn, range1.LastRow), Rectangle.FromLTRB(range2.Column, range2.Row, range2.LastColumn, range2.LastRow));
    return rectangle == Rectangle.Empty ? (IRange) null : range1[rectangle.Top, rectangle.Left, rectangle.Bottom, rectangle.Right];
  }

  public IRange MergeRanges(IRange range1, IRange range2)
  {
    if (range1 == null)
      throw new ArgumentNullException(nameof (range1));
    if (range2 == null)
      throw new ArgumentNullException(nameof (range2));
    if (range1.Parent != range2.Parent)
      return (IRange) null;
    int num1 = range1.LastColumn - range1.Column + 1;
    int num2 = range2.LastColumn - range2.Column + 1;
    int num3 = range1.LastRow - range1.Row + 1;
    int num4 = range2.LastRow - range2.Row + 1;
    if (num1 != num2 && num3 != num4)
      return (IRange) null;
    if (num1 == num2 && range1.Column == range2.Column)
    {
      if (range2.Row < range1.Row)
      {
        IRange range = range1;
        range1 = range2;
        range2 = range;
      }
      if (range2.Row >= range1.Row && range2.Row <= range1.LastRow + 1)
        return range1[range1.Row, range1.Column, Math.Max(range1.LastRow, range2.LastRow), range1.LastColumn];
    }
    if (num3 == num4 && range1.Row == range2.Row)
    {
      if (range2.Column < range1.Column)
      {
        IRange range = range1;
        range1 = range2;
        range2 = range;
      }
      if (range2.Column >= range1.Column && range2.Column <= range1.LastColumn + 1)
        return range1[range1.Row, range1.Column, range1.LastRow, Math.Max(range1.LastColumn, range2.LastColumn)];
    }
    return (IRange) null;
  }

  private IRange[] Find(string value)
  {
    this.ParseData();
    return this.ConvertCellListIntoRange(this.m_dicRecordsCells.Find(this.m_book.InnerSST.GetStringIndexes(value)));
  }

  public void Replace(string oldValue, string newValue)
  {
    IRange[] rangeArray = this.Find(oldValue);
    int length = rangeArray != null ? rangeArray.Length : 0;
    for (int index = 0; index < length; ++index)
    {
      IRange range = rangeArray[index];
      string lower = range.Text.ToLower();
      oldValue = oldValue.ToLower();
      range.Text = lower.Replace(oldValue, newValue);
    }
  }

  public void Replace(string oldValue, DateTime newValue)
  {
    IRange[] rangeArray = this.Find(oldValue);
    int length = rangeArray != null ? rangeArray.Length : 0;
    for (int index = 0; index < length; ++index)
      rangeArray[index].DateTime = newValue;
  }

  public void Replace(string oldValue, double newValue)
  {
    IRange[] rangeArray = this.Find(oldValue);
    int length = rangeArray != null ? rangeArray.Length : 0;
    for (int index = 0; index < length; ++index)
      rangeArray[index].Number = newValue;
  }

  public void Replace(string oldValue, string[] newValues, bool isVertical)
  {
    IRange[] rangeArray = this.Find(oldValue);
    int length = rangeArray != null ? rangeArray.Length : 0;
    for (int index = 0; index < length; ++index)
      ((RangeImpl) rangeArray[index]).Replace(oldValue, newValues, isVertical);
  }

  public void Replace(string oldValue, int[] newValues, bool isVertical)
  {
    IRange[] rangeArray = this.Find(oldValue);
    int length = rangeArray != null ? rangeArray.Length : 0;
    for (int index = 0; index < length; ++index)
      ((RangeImpl) rangeArray[index]).Replace(oldValue, newValues, isVertical);
  }

  public void Replace(string oldValue, double[] newValues, bool isVertical)
  {
    IRange[] rangeArray = this.Find(oldValue);
    int length = rangeArray != null ? rangeArray.Length : 0;
    for (int index = 0; index < length; ++index)
      ((RangeImpl) rangeArray[index]).Replace(oldValue, newValues, isVertical);
  }

  public void Replace(string oldValue, DataTable newValues, bool isFieldNamesShown)
  {
    IRange[] rangeArray = this.Find(oldValue);
    int length = rangeArray != null ? rangeArray.Length : 0;
    for (int index = 0; index < length; ++index)
      ((RangeImpl) rangeArray[index]).Replace(oldValue, newValues, isFieldNamesShown);
  }

  public void Replace(string oldValue, DataColumn column, bool isFieldNamesShown)
  {
    IRange[] rangeArray = this.Find(oldValue);
    int length = rangeArray != null ? rangeArray.Length : 0;
    for (int index = 0; index < length; ++index)
      ((RangeImpl) rangeArray[index]).Replace(oldValue, column, isFieldNamesShown);
  }

  public void Remove()
  {
    this.ParseData();
    this.m_book.InnerWorksheets.InnerRemove(this.Index);
    this.m_names.Clear();
    this.Dispose();
  }

  public void Move(int iNewIndex)
  {
    int realIndex = this.RealIndex;
    int worksheetNotBefore = this.FindWorksheetNotBefore(iNewIndex);
    this.m_book.Objects.Move(realIndex, iNewIndex);
    this.m_book.InnerWorksheets.Move(this.Index, worksheetNotBefore);
  }

  private int FindWorksheetNotBefore(int iNewIndex)
  {
    int index = iNewIndex;
    for (int objectCount = this.m_book.ObjectCount; index < objectCount; ++index)
    {
      if (this.m_book.Objects[index] is IWorksheet worksheet)
        return worksheet.Index;
    }
    return this.m_book.Worksheets.Count - 1;
  }

  public void SetColumnWidth(int iColumn, double value)
  {
    if (iColumn < 1 || iColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException("Column", "Column index cannot be larger then 256 or less then one");
    if (this.InnerGetColumnWidth(iColumn) == value)
      return;
    ColumnInfoRecord record = this.m_arrColumnInfo[iColumn];
    if (record == null)
    {
      record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
      record.FirstColumn = record.LastColumn = (ushort) (iColumn - 1);
      record.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
      record.ColumnWidth = (ushort) (this.Application.StandardWidth * 256.0);
      this.m_arrColumnInfo[iColumn] = record;
    }
    if (value == 0.0)
    {
      record.IsHidden = true;
    }
    else
    {
      if (value > (double) byte.MaxValue)
        value = (double) byte.MaxValue;
      record.ColumnWidth = (ushort) (value * 256.0);
      WorksheetHelper.AccessColumn((IInternalWorksheet) this, iColumn);
      this.RaiseColumnWidthChangedEvent(iColumn, value);
    }
    this.SetChanged();
  }

  public void SetColumnWidthInPixels(int iColumn, int value)
  {
    this.ParseData();
    double columnWidth = this.PixelsToColumnWidth(value);
    this.SetColumnWidth(iColumn, columnWidth);
  }

  public void SetColumnWidthInPixels(int iStartColumnIndex, int iCount, int value)
  {
    this.ParseData();
    double columnWidth = this.PixelsToColumnWidth(value);
    for (int index = 0; index < iCount; ++index)
      this.SetColumnWidth(iStartColumnIndex++, columnWidth);
  }

  public void SetRowHeight(int iRow, double value)
  {
    this.InnerSetRowHeight(iRow, value, true, MeasureUnits.Point, true);
  }

  public void SetRowHeightInPixels(int iRowIndex, double value)
  {
    if (iRowIndex < 1 || iRowIndex > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (iRowIndex), "Value cannot be less 1 and greater than max row index.");
    if (value < 0.0)
      throw new ArgumentOutOfRangeException(nameof (value));
    this.InnerSetRowHeight(iRowIndex, value, true, MeasureUnits.Pixel, true);
  }

  public void SetRowHeightInPixels(int iStartRowIndex, int iCount, double value)
  {
    if (iStartRowIndex < 1 || iStartRowIndex > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException("Row Index", "value cannot be less than 1 and greater than max row index");
    if (iStartRowIndex + iCount > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException("End Row Index", "Value cannot be greater than max row index");
    if (value < 0.0)
      throw new ArgumentOutOfRangeException(nameof (value));
    for (int index = 0; index < iCount; ++index)
      this.InnerSetRowHeight(iStartRowIndex++, value, true, MeasureUnits.Pixel, true);
  }

  public IRange FindFirst(string findValue, OfficeFindType flags)
  {
    return this.FindFirst(findValue, flags, OfficeFindOptions.None);
  }

  public IRange FindStringStartsWith(string findValue, OfficeFindType flags)
  {
    return this.FindStringStartsWith(findValue, flags, false);
  }

  public IRange FindStringStartsWith(string findValue, OfficeFindType flags, bool ignoreCase)
  {
    this.m_book.IsStartsOrEndsWith = new bool?(true);
    OfficeFindOptions findOptions = ignoreCase ? OfficeFindOptions.None : OfficeFindOptions.MatchCase;
    return this.FindFirst(findValue, flags, findOptions);
  }

  public IRange FindStringEndsWith(string findValue, OfficeFindType flags)
  {
    return this.FindStringEndsWith(findValue, flags, false);
  }

  public IRange FindStringEndsWith(string findValue, OfficeFindType flags, bool ignoreCase)
  {
    this.m_book.IsStartsOrEndsWith = new bool?(false);
    OfficeFindOptions findOptions = ignoreCase ? OfficeFindOptions.None : OfficeFindOptions.MatchCase;
    return this.FindFirst(findValue, flags, findOptions);
  }

  public IRange FindFirst(string findValue, OfficeFindType flags, OfficeFindOptions findOptions)
  {
    return this.Find(this.UsedRange, findValue, flags, findOptions, true)?[0];
  }

  public IRange FindFirst(double findValue, OfficeFindType flags)
  {
    return this.Find(this.UsedRange, findValue, flags, true)?[0];
  }

  public IRange FindFirst(bool findValue)
  {
    return this.Find(this.UsedRange, findValue ? (byte) 1 : (byte) 0, false, true)?[0];
  }

  public IRange FindFirst(DateTime findValue)
  {
    return this.Find(this.UsedRange, UtilityMethods.ConvertDateTimeToNumber(findValue), OfficeFindType.Number, true)?[0];
  }

  public IRange FindFirst(TimeSpan findValue)
  {
    return this.Find(this.UsedRange, (double) findValue.Days + (double) (findValue.Hours * 360000 + findValue.Minutes * 6000 + findValue.Seconds * 100 + findValue.Milliseconds) / 8640000.0, OfficeFindType.Number, true)?[0];
  }

  public IRange[] FindAll(string findValue, OfficeFindType flags)
  {
    return this.FindAll(findValue, flags, OfficeFindOptions.None);
  }

  public IRange[] FindAll(string findValue, OfficeFindType flags, OfficeFindOptions findOptions)
  {
    return findValue == null || findValue.Length == 0 ? (IRange[]) null : this.Find(this.UsedRange, findValue, flags, findOptions, false);
  }

  public IRange[] FindAll(double findValue, OfficeFindType flags)
  {
    bool flag1 = (flags & OfficeFindType.FormulaValue) == OfficeFindType.FormulaValue;
    bool flag2 = (flags & OfficeFindType.Number) == OfficeFindType.Number;
    if (!flag1 && !flag2)
      throw new ArgumentException("Parameter flags is not valid.", nameof (flags));
    return this.Find(this.UsedRange, findValue, flags, false);
  }

  public IRange[] FindAll(bool findValue)
  {
    return this.Find(this.UsedRange, findValue ? (byte) 1 : (byte) 0, false, false);
  }

  public IRange[] FindAll(DateTime findValue)
  {
    return this.FindAll(UtilityMethods.ConvertDateTimeToNumber(findValue), OfficeFindType.Number | OfficeFindType.FormulaValue);
  }

  public IRange[] FindAll(TimeSpan findValue)
  {
    return this.FindAll(findValue.TotalDays, OfficeFindType.Number | OfficeFindType.FormulaValue);
  }

  public void SaveAs(string fileName, string separator)
  {
    this.SaveAs(fileName, separator, Encoding.UTF8);
  }

  public void SaveAs(string fileName, string separator, Encoding encoding)
  {
    if (fileName == null)
      throw new ArgumentNullException("Filename");
    if (separator == null || separator == string.Empty)
      throw new ArgumentNullException(nameof (separator));
    string path = fileName.Length != 0 ? Path.GetFullPath(fileName) : throw new ArgumentException("FileName cannot be empty.");
    string directoryName = Path.GetDirectoryName(path);
    if (File.Exists(path) && (File.GetAttributes(path) & FileAttributes.ReadOnly) != (FileAttributes) 0)
      throw new IOException("Cannot save. File is readonly.");
    if (directoryName != null && directoryName.Length > 0 && !Directory.Exists(directoryName))
      Directory.CreateDirectory(directoryName);
    using (FileStream fileStream = new FileStream(path, FileMode.Create))
    {
      this.SaveAs((Stream) fileStream, separator, encoding);
      fileStream.Close();
    }
  }

  public void SaveAs(Stream stream, string separator)
  {
    this.SaveAs(stream, separator, Encoding.UTF8);
  }

  public void SaveAsInternal(Stream stream, string separator, Encoding encoding)
  {
    this.ParseData();
    StreamWriter streamWriter = new StreamWriter(stream, encoding);
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
    {
      if (!this.IsRowEmpty(iFirstRow, false))
      {
        for (int iFirstColumn = this.m_iFirstColumn; iFirstColumn <= this.m_iLastColumn; ++iFirstColumn)
        {
          long cellIndex = RangeImpl.GetCellIndex(iFirstColumn, iFirstRow);
          WorksheetImpl.TRangeValueType cellType = this.m_dicRecordsCells.GetCellType(iFirstRow, iFirstColumn);
          string empty = string.Empty;
          if (cellType != WorksheetImpl.TRangeValueType.Blank)
          {
            string str = this.m_dicRecordsCells.GetValue(cellIndex, iFirstRow, iFirstColumn, this.Range, separator);
            if (str.Contains('\n'.ToString()) || str.Contains(separator))
              str = $"\"{str}\"";
            streamWriter.Write(str);
          }
          if (iFirstColumn != this.m_iLastColumn)
            streamWriter.Write(separator);
        }
      }
      streamWriter.WriteLine();
    }
    streamWriter.Flush();
    stream.Flush();
  }

  public void SaveAs(Stream stream, string separator, Encoding encoding)
  {
    if (stream == null)
      throw new ArgumentException(nameof (stream));
    if (separator == null || separator.Length == 0)
      throw new ArgumentException(nameof (separator));
    this.SaveAsInternal(stream, separator, encoding);
  }

  public void SetDefaultColumnStyle(int iColumnIndex, IStyle defaultStyle)
  {
    this.SetDefaultRowColumnStyle(iColumnIndex, iColumnIndex, defaultStyle, (IList) this.m_arrColumnInfo, new WorksheetImpl.OutlineDelegate(this.CreateColumnOutline), false);
    WorksheetHelper.AccessRow((IInternalWorksheet) this, iColumnIndex);
  }

  public void SetDefaultColumnStyle(
    int iStartColumnIndex,
    int iEndColumnIndex,
    IStyle defaultStyle)
  {
    this.ParseData();
    ushort correctIndex = (ushort) this.ConvertStyleToCorrectIndex(defaultStyle);
    for (int index = iStartColumnIndex; index <= iEndColumnIndex; ++index)
    {
      ((IOutline) this.m_arrColumnInfo[index] ?? this.CreateColumnOutline(index)).ExtendedFormatIndex = correctIndex;
      this.SetCellStyle(index, correctIndex);
    }
    WorksheetHelper.AccessColumn((IInternalWorksheet) this, iStartColumnIndex);
    WorksheetHelper.AccessColumn((IInternalWorksheet) this, iEndColumnIndex);
  }

  public void SetDefaultRowStyle(int iRowIndex, IStyle defaultStyle)
  {
    ushort correctIndex = (ushort) this.ConvertStyleToCorrectIndex(defaultStyle);
    WorksheetHelper.AccessRow((IInternalWorksheet) this, iRowIndex);
    --iRowIndex;
    RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iRowIndex, true);
    if (this.Rows.Length > 0)
    {
      foreach (IRange cell in this.Rows[iRowIndex].Cells)
        row.SetCellStyle(iRowIndex, cell.Column - 1, (int) correctIndex, this.Application.RowStorageAllocationBlockSize);
    }
    row.ExtendedFormatIndex = correctIndex;
  }

  public void SetDefaultRowStyle(int iStartRowIndex, int iEndRowIndex, IStyle defaultStyle)
  {
    ushort correctIndex = (ushort) this.ConvertStyleToCorrectIndex(defaultStyle);
    WorksheetHelper.AccessRow((IInternalWorksheet) this, iStartRowIndex);
    WorksheetHelper.AccessRow((IInternalWorksheet) this, iEndRowIndex);
    --iStartRowIndex;
    --iEndRowIndex;
    for (int index = iStartRowIndex; index <= iEndRowIndex; ++index)
    {
      RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, index, true);
      if (this.Rows.Length > 0)
      {
        foreach (IRange cell in this.Rows[index].Cells)
          row.SetCellStyle(index, cell.Column - 1, (int) correctIndex, this.Application.RowStorageAllocationBlockSize);
      }
      row.ExtendedFormatIndex = correctIndex;
    }
  }

  private void SetCellStyle(int iColIndex, ushort XFindex)
  {
    for (int rowIndex = this.CellRecords.FirstRow - 1; rowIndex <= this.CellRecords.LastRow; ++rowIndex)
    {
      RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, rowIndex, false);
      if (row != null && row.ExtendedFormatIndex != (ushort) 0)
      {
        ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(rowIndex + 1, iColIndex);
        if (cellRecord != null)
        {
          cellRecord.ExtendedFormatIndex = XFindex;
          this.m_dicRecordsCells.AddRecord(cellRecord, false);
        }
        else
        {
          ICellPositionFormat cell = this.m_dicRecordsCells.CreateCell(rowIndex + 1, iColIndex, TBIFFRecord.Blank);
          cell.ExtendedFormatIndex = XFindex;
          this.m_dicRecordsCells.AddRecord(cell, false);
        }
      }
    }
  }

  public IStyle GetDefaultColumnStyle(int iColumnIndex)
  {
    this.ParseData();
    if (iColumnIndex < 1 || iColumnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (iColumnIndex), "Value cannot be less than 1 and greater than m_book.MaxColumnCount.");
    IOutline outline = (IOutline) this.m_arrColumnInfo[iColumnIndex];
    return (IStyle) new ExtendedFormatWrapper(this.m_book, outline != null ? (int) outline.ExtendedFormatIndex : this.m_book.DefaultXFIndex);
  }

  public IStyle GetDefaultRowStyle(int iRowIndex)
  {
    if (iRowIndex < 1 || iRowIndex > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (iRowIndex), "Value cannot be less than 1 and greater than m_book.MaxColumnCount.");
    RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iRowIndex - 1, false);
    return (IStyle) new ExtendedFormatWrapper(this.m_book, row == null || !this.m_book.IsFormatted((int) row.ExtendedFormatIndex) ? this.m_book.DefaultXFIndex : (int) row.ExtendedFormatIndex);
  }

  public void FreeRange(IRange range)
  {
    int row = range.Row;
    for (int lastRow = range.LastRow; row <= lastRow; ++row)
    {
      int column = range.Column;
      for (int lastColumn = range.LastColumn; column <= lastColumn; ++column)
        this.FreeRange(row, column);
    }
  }

  public void FreeRange(int iRow, int iColumn)
  {
    this.ParseData();
    this.CellRecords.FreeRange(iRow, iColumn);
  }

  public IRange TopLeftCell
  {
    get => this[this.TopVisibleRow, this.LeftVisibleColumn];
    set
    {
      if (this.IsFreezePanes)
      {
        if (value.Row <= this.PaneFirstVisible.Row || value.Column <= this.PaneFirstVisible.Column)
          return;
        this.FirstVisibleRow = value.Row - 1;
        this.FirstVisibleColumn = value.Column - 1;
      }
      else
      {
        this.TopVisibleRow = value.Row;
        this.LeftVisibleColumn = value.Column;
      }
    }
  }

  private void ImportDataTableWithoutCheck(
    DataTable dataTable,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    DataColumn[] arrColumns,
    bool isOptimized)
  {
    if (isOptimized)
    {
      for (int index1 = 0; index1 < maxRows; ++index1)
      {
        DataRow row = dataTable.Rows[index1];
        for (int index2 = 0; index2 < maxColumns; ++index2)
        {
          DataColumn arrColumn = arrColumns[index2];
          string name = arrColumn.DataType.Name;
          this.SetString(firstRow + index1, firstColumn + index2, (string) row[arrColumn]);
        }
      }
    }
    else
    {
      for (int index3 = 0; index3 < maxRows; ++index3)
      {
        DataRow row = dataTable.Rows[index3];
        for (int index4 = 0; index4 < maxColumns; ++index4)
        {
          DataColumn arrColumn = arrColumns[index4];
          object obj = row[arrColumn];
          if (obj != DBNull.Value)
          {
            switch (arrColumn.DataType.Name)
            {
              case "String":
                MigrantRangeImpl migrantRangeImpl1 = new MigrantRangeImpl(this.Application, (IWorksheet) this);
                migrantRangeImpl1.ResetRowColumn(firstRow + index3, firstColumn + index4);
                migrantRangeImpl1.Value2 = row[arrColumn];
                continue;
              case "DateTime":
                if (obj is DateTime dateTime && dateTime >= RangeImpl.DEF_MIN_DATETIME)
                {
                  MigrantRangeImpl migrantRangeImpl2 = new MigrantRangeImpl(this.Application, (IWorksheet) this);
                  migrantRangeImpl2.ResetRowColumn(firstRow + index3, firstColumn + index4);
                  migrantRangeImpl2.Value2 = row[arrColumn];
                  continue;
                }
                this.SetString(firstRow + index3, firstColumn + index4, obj.ToString());
                continue;
              case "Double":
                this.SetNumber(firstRow + index3, firstColumn + index4, (double) obj);
                continue;
              case "Int32":
                this.SetNumber(firstRow + index3, firstColumn + index4, (double) (int) obj);
                continue;
              default:
                this.SetValueRowCol(obj, firstRow + index3, firstColumn + index4);
                continue;
            }
          }
        }
      }
    }
  }

  private void ImportDataTableWithoutCheckPreserve(
    DataTable dataTable,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    DataColumn[] arrColumns)
  {
    for (int index1 = 0; index1 < maxRows; ++index1)
    {
      DataRow row = dataTable.Rows[index1];
      for (int index2 = 0; index2 < maxColumns; ++index2)
      {
        DataColumn arrColumn = arrColumns[index2];
        object obj = row[arrColumn];
        if (obj != DBNull.Value)
        {
          switch (arrColumn.DataType.Name)
          {
            case "String":
              this.SetString(firstRow + index1, firstColumn + index2, (string) obj);
              continue;
            case "DateTime":
              if (obj is DateTime dateTime && dateTime >= RangeImpl.DEF_MIN_DATETIME)
              {
                MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this);
                migrantRangeImpl.ResetRowColumn(firstRow + index1, firstColumn + index2);
                migrantRangeImpl.Value2 = row[arrColumn];
                continue;
              }
              this.SetString(firstRow + index1, firstColumn + index2, obj.ToString());
              continue;
            case "Double":
              this.SetNumber(firstRow + index1, firstColumn + index2, (double) obj);
              continue;
            case "Int32":
              this.SetNumber(firstRow + index1, firstColumn + index2, (double) (int) obj);
              continue;
            default:
              this.SetValueRowCol(obj, firstRow + index1, firstColumn + index2);
              continue;
          }
        }
      }
    }
  }

  private void ImportDataViewWithoutCheck(
    DataView dataView,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns)
  {
    for (int recordIndex = 0; recordIndex < maxRows; ++recordIndex)
    {
      DataRowView dataRowView = dataView[recordIndex];
      for (int ndx = 0; ndx < maxColumns; ++ndx)
        this.InnerGetCell(firstColumn + ndx, firstRow + recordIndex).Value2 = dataRowView[ndx];
    }
  }

  private void ImportDataViewWithoutCheckPreserve(
    DataView dataView,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns)
  {
    Dictionary<int, WorksheetImpl.RangeProperty> hashColumnTypes = new Dictionary<int, WorksheetImpl.RangeProperty>(maxColumns);
    for (int recordIndex = 0; recordIndex < maxRows; ++recordIndex)
    {
      DataRowView dataRowView = dataView[recordIndex];
      for (int index = 0; index < maxColumns; ++index)
      {
        object obj = dataRowView[index];
        switch (obj)
        {
          case null:
          case DBNull _:
            continue;
          default:
            WorksheetImpl.RangeProperty valueType = this.GetValueType(obj, index, hashColumnTypes);
            IRange cell = this.InnerGetCell(firstColumn + index, firstRow + recordIndex);
            switch (valueType)
            {
              case WorksheetImpl.RangeProperty.Text:
                cell.Text = (string) obj;
                continue;
              case WorksheetImpl.RangeProperty.DateTime:
                cell.DateTime = (DateTime) obj;
                continue;
              case WorksheetImpl.RangeProperty.TimeSpan:
                cell.TimeSpan = (TimeSpan) obj;
                continue;
              default:
                cell.Value2 = obj;
                continue;
            }
        }
      }
    }
  }

  private WorksheetImpl.RangeProperty GetValueType(
    object value,
    int iColumnIndex,
    Dictionary<int, WorksheetImpl.RangeProperty> hashColumnTypes)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (hashColumnTypes == null)
      throw new ArgumentNullException(nameof (hashColumnTypes));
    if (hashColumnTypes.ContainsKey(iColumnIndex))
      return hashColumnTypes[iColumnIndex];
    WorksheetImpl.RangeProperty valueType = WorksheetImpl.RangeProperty.Value2;
    switch (value)
    {
      case string _:
        valueType = WorksheetImpl.RangeProperty.Text;
        break;
      case DateTime _:
        valueType = WorksheetImpl.RangeProperty.DateTime;
        break;
      case TimeSpan _:
        valueType = WorksheetImpl.RangeProperty.TimeSpan;
        break;
    }
    hashColumnTypes.Add(iColumnIndex, valueType);
    return valueType;
  }

  [CLSCompliant(false)]
  public override void Serialize(OffsetArrayList records)
  {
    if (this.ParseOnDemand)
      records.AddList((IList) this.m_arrRecords);
    else
      this.Serialize(records, false);
  }

  protected override bool ContainsProtection => base.ContainsProtection;

  private void SerializeNotParsedWorksheet(OffsetArrayList records)
  {
    throw new NotImplementedException();
  }

  [CLSCompliant(false)]
  public void SerializeForClipboard(OffsetArrayList records) => this.Serialize(records, true);

  [CLSCompliant(false)]
  protected void SerializeColumnInfo(OffsetArrayList records)
  {
    int num = records != null ? this.SerializeGroupColumnInfo(records) : throw new ArgumentNullException(nameof (records));
    if (num >= (int) byte.MaxValue)
      return;
    ColumnInfoRecord record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
    record.FirstColumn = (ushort) (num + 1);
    record.LastColumn = (ushort) byte.MaxValue;
    record.ColumnWidth = (ushort) this.EvaluateFileColumnWidth((int) (this.StandardWidth * 256.0));
    record.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
    records.Add((IBiffStorage) record);
  }

  [CLSCompliant(false)]
  protected int SerializeGroupColumnInfo(OffsetArrayList records)
  {
    int index1 = 1;
    int index2 = 1;
    ColumnInfoRecord columnInfoRecord1 = (ColumnInfoRecord) null;
    for (; index1 <= 256 /*0x0100*/; index1 = index2 + 1)
    {
      for (; index1 <= 256 /*0x0100*/; ++index1)
      {
        columnInfoRecord1 = this.m_arrColumnInfo[index1];
        if (columnInfoRecord1 != null)
          break;
      }
      if (columnInfoRecord1 != null)
      {
        index2 = index1;
        ColumnInfoRecord columnInfoRecord2;
        do
        {
          ++index2;
          columnInfoRecord2 = this.m_arrColumnInfo[index2];
          if (columnInfoRecord1.CompareTo((object) columnInfoRecord2) != 0)
            columnInfoRecord2 = (ColumnInfoRecord) null;
        }
        while (index2 <= 256 /*0x0100*/ && columnInfoRecord2 != null);
        if (columnInfoRecord2 == null)
        {
          --index2;
          columnInfoRecord2 = this.m_arrColumnInfo[index2];
        }
        ColumnInfoRecord columnInfoRecord3;
        if (index1 == index2)
        {
          columnInfoRecord3 = (ColumnInfoRecord) columnInfoRecord1.Clone();
        }
        else
        {
          columnInfoRecord3 = (ColumnInfoRecord) columnInfoRecord1.Clone();
          columnInfoRecord3.LastColumn = columnInfoRecord2.LastColumn;
        }
        columnInfoRecord3.ColumnWidth = (ushort) this.EvaluateFileColumnWidth((int) columnInfoRecord3.ColumnWidth);
        records.Add((IBiffStorage) columnInfoRecord3);
      }
      else
        break;
    }
    return index2 - 1;
  }

  private bool CompareDVWithoutRanges(DVRecord curDV, DVRecord dvToAdd)
  {
    if (curDV == null)
      return dvToAdd == null;
    return curDV.Condition == dvToAdd.Condition && curDV.DataType == dvToAdd.DataType && curDV.ErrorBoxText == dvToAdd.ErrorBoxText && curDV.ErrorBoxTitle == dvToAdd.ErrorBoxTitle && curDV.ErrorStyle == dvToAdd.ErrorStyle && curDV.IsEmptyCell == dvToAdd.IsEmptyCell && curDV.IsShowErrorBox == dvToAdd.IsShowErrorBox && curDV.IsShowPromptBox == dvToAdd.IsShowPromptBox && curDV.IsStrListExplicit == dvToAdd.IsStrListExplicit && curDV.IsSuppressArrow == dvToAdd.IsSuppressArrow && curDV.PromtBoxText == dvToAdd.PromtBoxText && curDV.PromtBoxTitle == dvToAdd.PromtBoxTitle && Ptg.CompareArrays(curDV.FirstFormulaTokens, dvToAdd.FirstFormulaTokens) && Ptg.CompareArrays(curDV.SecondFormulaTokens, dvToAdd.SecondFormulaTokens);
  }

  private void MergeDVRanges(DVRecord curDv, DVRecord dvToAdd)
  {
    if (curDv == null)
      throw new ArgumentNullException(nameof (curDv));
    if (dvToAdd == null)
      throw new ArgumentNullException(nameof (dvToAdd));
    curDv.AddRange(dvToAdd.AddrList);
  }

  [CLSCompliant(false)]
  protected override void SerializeMsoDrawings(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    base.SerializeMsoDrawings(records);
    if ((this.Application.SkipOnSave & OfficeSkipExtRecords.Drawings) == OfficeSkipExtRecords.Drawings || this.m_arrNotesByCellIndex == null)
      return;
    foreach (NoteRecord noteRecord in (IEnumerable<NoteRecord>) this.m_arrNotesByCellIndex.Values)
      records.Add((IBiffStorage) noteRecord);
  }

  private void Serialize(OffsetArrayList records, bool bClipboard)
  {
    if (this.m_arrNotes != null)
    {
      this.m_arrNotes.Clear();
      this.m_arrNotesByCellIndex.Clear();
    }
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (!this.IsSupported)
      records.AddList((IList) this.m_arrRecords);
    else if (!this.IsParsed)
    {
      this.SerializeNotParsedWorksheet(records);
    }
    else
    {
      this.m_bof.Type = BOFRecord.TType.TYPE_WORKSHEET;
      records.Add((IBiffStorage) this.m_bof);
      IndexRecord indexRecord = (IndexRecord) null;
      int num1 = this.m_iLastRow - this.m_iFirstRow + 1;
      int length = 0;
      if (num1 > 0)
      {
        int num2 = num1 % 32 /*0x20*/;
        length = num1 / 32 /*0x20*/;
        if (num2 != 0)
          ++length;
      }
      if (!bClipboard)
      {
        indexRecord = (IndexRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Index);
        indexRecord.DbCells = new int[length];
        indexRecord.FirstRow = this.m_iLastRow != this.m_iFirstRow || this.m_iFirstRow != -1 ? this.m_iFirstRow - 1 : 0;
        indexRecord.LastRow = this.m_iLastRow != this.m_iFirstRow || this.m_iFirstRow != -1 ? this.m_iLastRow : 0;
        records.Add((IBiffStorage) indexRecord);
      }
      this.m_book.InnerCalculation.Serialize(records);
      records.Add((IBiffStorage) this.m_pageSetup);
      this.SerializeProtection(records, false);
      DefaultColWidthRecord record1 = (DefaultColWidthRecord) BiffRecordFactory.GetRecord(TBIFFRecord.DefaultColWidth);
      record1.Width = (ushort) this.m_dStandardColWidth;
      records.Add((IBiffStorage) record1);
      this.SerializeColumnInfo(records);
      if (this.m_arrSortRecords != null)
        records.AddList((IList) this.m_arrSortRecords);
      DimensionsRecord record2 = (DimensionsRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Dimensions);
      record2.LastRow = this.m_iLastRow != this.m_iFirstRow || this.m_iFirstRow != -1 ? this.m_iLastRow : 0;
      record2.LastColumn = this.m_iLastColumn != this.m_iFirstColumn || this.m_iFirstColumn != int.MaxValue ? (ushort) this.m_iLastColumn : (ushort) 0;
      record2.FirstRow = this.m_iLastRow != this.m_iFirstRow || this.m_iFirstRow != -1 ? this.m_iFirstRow - 1 : 0;
      record2.FirstColumn = this.m_iLastColumn != this.m_iFirstColumn || this.m_iFirstColumn != int.MaxValue ? (ushort) (this.m_iFirstColumn - 1) : (ushort) 0;
      records.Add((IBiffStorage) record2);
      List<DBCellRecord> arrDBCells = new List<DBCellRecord>();
      this.m_dicRecordsCells.Serialize(records, arrDBCells);
      if (!bClipboard)
        indexRecord.DbCellRecords = arrDBCells;
      this.SerializeMsoDrawings(records);
      if (this.m_arrDConRecords != null)
        records.AddList((IList) this.m_arrDConRecords);
      this.SerializeHeaderFooterPictures(records);
      this.SerializeWindowTwo(records);
      this.SerializePageLayoutView(records);
      this.SerializeWindowZoom(records);
      if (this.m_pane != null)
      {
        if (this.VerticalSplit == 0 && this.HorizontalSplit == 0)
          this.m_pane.ActivePane = (ushort) 3;
        else if (this.VerticalSplit == 0)
          this.m_pane.ActivePane = (ushort) 2;
        else if (this.HorizontalSplit == 0)
          this.m_pane.ActivePane = (ushort) 1;
        records.Add((IBiffStorage) this.m_pane);
      }
      this.CreateAllSelections();
      records.AddList((IList) this.m_arrSelections);
      if (this.m_mergedCells != null)
        this.m_mergedCells.Serialize(records);
      records.AddList((IList) this.PreserveExternalConnection);
      records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.UnkMarker));
      this.SerializeMacrosSupport(records);
      this.SerializeSheetLayout(records);
      this.SerializeSheetProtection(records);
      if (this.m_tableRecords != null)
        records.AddRange((ICollection) this.m_tableRecords);
      records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.EOF));
      if (this.m_arrNotes == null)
        return;
      this.m_arrNotes.Clear();
      this.m_arrNotesByCellIndex.Clear();
    }
  }

  protected void RaiseColumnWidthChangedEvent(int iColumn, double dNewValue)
  {
    if (this.ColumnWidthChanged == null)
      return;
    this.ColumnWidthChanged((object) this, new ValueChangedEventArgs((object) iColumn, (object) dNewValue, "ColumnWidth"));
  }

  protected void RaiseRowHeightChangedEvent(int iRow, double dNewValue)
  {
    if (this.RowHeightChanged == null)
      return;
    this.RowHeightChanged((object) this, new ValueChangedEventArgs((object) iRow, (object) dNewValue, "RowHeight"));
  }

  private void NormalFont_OnAfterChange(object sender, EventArgs e)
  {
    if (this.m_iFirstRow <= 0)
      return;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
    {
      RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iFirstRow, false);
      if (row != null && !row.IsBadFontHeight)
        this.AutofitRow(iFirstRow);
    }
  }

  public void SetFormulaValue(int iRow, int iColumn, string value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (value[0] == '#' && FormulaUtil.ErrorNameToCode.ContainsKey(value))
    {
      this.SetFormulaErrorValue(iRow, iColumn, value);
    }
    else
    {
      IRange range = this.Range[iRow, iColumn];
      double result1;
      if (double.TryParse(value, out result1) && (!range.NumberFormat.Contains("@") || range.NumberFormat.Length != 1))
      {
        this.SetFormulaNumberValue(iRow, iColumn, result1);
      }
      else
      {
        bool result2;
        if (bool.TryParse(value, out result2))
          this.SetFormulaBoolValue(iRow, iColumn, result2);
        else
          this.SetFormulaStringValue(iRow, iColumn, value);
      }
    }
  }

  public void SetValue(int iRow, int iColumn, string value)
  {
    switch (value)
    {
      case null:
        throw new ArgumentNullException(nameof (value));
      case "":
        this.SetBlankRecord(iRow, iColumn);
        break;
      default:
        if (value[0] == '=')
        {
          this.SetFormula(iRow, iColumn, value.Substring(1));
          break;
        }
        if (value[0] == '#' && FormulaUtil.ErrorNameToCode.ContainsKey(value) && this.GetFormulaErrorValue(iRow, iColumn) != null)
        {
          this.SetFormulaErrorValue(iRow, iColumn, value);
          break;
        }
        if (this[iRow, iColumn].HasFormula)
        {
          this.SetFormulaValue(iRow, iColumn, value);
          break;
        }
        RangeImpl rangeImpl = this.Range[iRow, iColumn] as RangeImpl;
        bool dateTime = rangeImpl.TryParseDateTime(value, out DateTime _);
        double result;
        if (double.TryParse(value, out result) && !dateTime)
        {
          this.SetNumber(iRow, iColumn, result);
          break;
        }
        if (dateTime)
        {
          rangeImpl.Value = value;
          break;
        }
        this.SetString(iRow, iColumn, value);
        break;
    }
  }

  public void SetNumber(int iRow, int iColumn, double value)
  {
    int iXFIndex = this.RemoveString(iRow, iColumn);
    RKRecord rkRecord = this.TryCreateRkRecord(iRow, iColumn, value, iXFIndex);
    if (rkRecord != null)
      this.InnerSetCell(iColumn, iRow, (BiffRecordRaw) rkRecord);
    else
      this.SetNumberRecord(iRow, iColumn, value, iXFIndex);
  }

  public void SetBoolean(int iRow, int iColumn, bool value)
  {
    int iXFIndex = this.RemoveString(iRow, iColumn);
    BoolErrRecord record = (BoolErrRecord) this.GetRecord(TBIFFRecord.BoolErr, iRow, iColumn, iXFIndex);
    record.IsErrorCode = false;
    record.BoolOrError = value ? (byte) 1 : (byte) 0;
    this.InnerSetCell(iColumn, iRow, (BiffRecordRaw) record);
  }

  public void SetText(int iRow, int iColumn, string value)
  {
    if (value == null || value.Length == 0)
      throw new ArgumentOutOfRangeException("Text value cannot be null or empty");
    this.SetString(iRow, iColumn, value);
  }

  public void SetFormula(int iRow, int iColumn, string value)
  {
    this.SetFormula(iRow, iColumn, value, false);
  }

  public void SetFormula(int iRow, int iColumn, string value, bool bIsR1C1)
  {
    if (value == null || value.Length == 0 || value[0] == '=')
      throw new ArgumentOutOfRangeException("Text value cannot be null or empty. First symbol of formula cannot be '='");
    this.SetFormulaValue(iRow, iColumn, value, bIsR1C1);
  }

  public void SetError(int iRow, int iColumn, string value)
  {
    if (value == null || value.Length == 0 || value[0] != '#')
      throw new ArgumentOutOfRangeException("Text value cannot be null or empty. First symbol must be '#'");
    this.SetError(iRow, iColumn, value, false);
  }

  public void SetBlank(int iRow, int iColumn) => this.SetBlankRecord(iRow, iColumn);

  private void SetBlankRecord(int iRow, int iColumn)
  {
    int iXFIndex = this.RemoveString(iRow, iColumn);
    BiffRecordRaw record = this.GetRecord(TBIFFRecord.Blank, iRow, iColumn, iXFIndex);
    this.InnerSetCell(iColumn, iRow, record);
  }

  private void SetNumberRecord(int iRow, int iColumn, double value, int iXFIndex)
  {
    NumberRecord record = (NumberRecord) this.GetRecord(TBIFFRecord.Number, iRow, iColumn, iXFIndex);
    record.Value = value;
    this.InnerSetCell(iColumn, iRow, (BiffRecordRaw) record);
  }

  private void SetRKRecord(int iRow, int iColumn, double value)
  {
    RKRecord record = (RKRecord) this.GetRecord(TBIFFRecord.RK, iRow, iColumn);
    record.RKNumber = value;
    this.InnerSetCell(iColumn, iRow, (BiffRecordRaw) record);
  }

  private void SetFormulaValue(int iRow, int iColumn, string value, bool bIsR1C1)
  {
    int iXFIndex = this.RemoveString(iRow, iColumn);
    FormulaRecord record = (FormulaRecord) this.GetRecord(TBIFFRecord.Formula, iRow, iColumn, iXFIndex);
    record.ParsedExpression = this.m_book.FormulaUtil.ParseString(value, (IWorksheet) this, (Dictionary<string, string>) null, iRow - 1, iColumn - 1, bIsR1C1);
    this.InnerSetCell(iColumn, iRow, (BiffRecordRaw) record);
  }

  public void SetFormulaNumberValue(int iRow, int iColumn, double value)
  {
    if ((this.GetCellType(iRow, iColumn, false) & WorksheetImpl.TRangeValueType.Formula) != WorksheetImpl.TRangeValueType.Formula)
      throw new ArgumentException("Cannot sets formula value in cell that doesn't contain formula");
    this.SetFormulaValue(iRow, iColumn, value);
  }

  public void SetFormulaErrorValue(int iRow, int iColumn, string value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (!FormulaUtil.ErrorNameToCode.ContainsKey(value))
      throw new ArgumentOutOfRangeException("Value does not valid error string.");
    if ((this.GetCellType(iRow, iColumn, false) & WorksheetImpl.TRangeValueType.Formula) != WorksheetImpl.TRangeValueType.Formula)
      throw new ArgumentException("Cannot sets formula value in cell that doesn't contain formula");
    double boolErrorValue = FormulaRecord.GetBoolErrorValue((byte) FormulaUtil.ErrorNameToCode[value], true);
    this.SetFormulaValue(iRow, iColumn, boolErrorValue);
  }

  public void SetFormulaBoolValue(int iRow, int iColumn, bool value)
  {
    if ((this.GetCellType(iRow, iColumn, false) & WorksheetImpl.TRangeValueType.Formula) != WorksheetImpl.TRangeValueType.Formula)
      throw new ArgumentException("Cannot sets formula value in cell that doesn't contain formula");
    double boolErrorValue = FormulaRecord.GetBoolErrorValue(value ? (byte) 1 : (byte) 0, false);
    this.SetFormulaValue(iRow, iColumn, boolErrorValue);
  }

  public void SetFormulaStringValue(int iRow, int iColumn, string value)
  {
    if ((this.GetCellType(iRow, iColumn, false) & WorksheetImpl.TRangeValueType.Formula) != WorksheetImpl.TRangeValueType.Formula)
      throw new ArgumentException("Cannot sets formula value in cell that doesn't contain formula");
    StringRecord record = (StringRecord) this.RecordExtractor.GetRecord(519);
    record.Value = value;
    double defStringValue = FormulaRecord.DEF_STRING_VALUE;
    this.SetFormulaValue(iRow, iColumn, defStringValue, record);
  }

  public void SetError(int iRow, int iColumn, string value, bool isSetText)
  {
    int num;
    if (!FormulaUtil.ErrorNameToCode.TryGetValue(value, out num))
    {
      if (!isSetText)
        throw new ArgumentOutOfRangeException("Cannot parse error code.");
      this.SetString(iRow, iColumn, value);
    }
    else
    {
      int iXFIndex = this.RemoveString(iRow, iColumn);
      BoolErrRecord record = (BoolErrRecord) this.GetRecord(TBIFFRecord.BoolErr, iRow, iColumn, iXFIndex);
      record.IsErrorCode = true;
      record.BoolOrError = (byte) num;
      this.InnerSetCell(iColumn, iRow, (BiffRecordRaw) record);
    }
  }

  private void SetString(int iRow, int iColumn, string value)
  {
    int iXFIndex = this.RemoveString(iRow, iColumn);
    int num = this.m_book.InnerSST.AddIncrease((object) value);
    LabelSSTRecord record = (LabelSSTRecord) this.GetRecord(TBIFFRecord.LabelSST, iRow, iColumn, iXFIndex);
    record.SSTIndex = num;
    this.InnerSetCell(iColumn, iRow, (BiffRecordRaw) record);
  }

  private int RemoveString(int iRow, int iColumn)
  {
    this.ParseData();
    ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(iRow, iColumn);
    int num = this.m_book.DefaultXFIndex;
    if (cellRecord != null)
    {
      num = (int) cellRecord.ExtendedFormatIndex;
    }
    else
    {
      RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iRow - 1, false);
      if (row != null)
        num = (int) row.ExtendedFormatIndex;
      if (num == 0 || num == this.m_book.DefaultXFIndex)
      {
        ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[iColumn];
        if (columnInfoRecord != null)
          num = (int) columnInfoRecord.ExtendedFormatIndex;
      }
    }
    if (cellRecord is LabelSSTRecord labelSstRecord)
      this.m_book.InnerSST.RemoveDecrease(labelSstRecord.SSTIndex);
    return num;
  }

  internal int GetXFIndex(int iRow, int iColumn)
  {
    this.ParseData();
    int extendedFormatIndex1 = this.m_dicRecordsCells.GetExtendedFormatIndex(iRow, iColumn);
    if (extendedFormatIndex1 < 0)
    {
      RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iRow - 1, false);
      int extendedFormatIndex2 = row == null || !this.m_book.IsFormatted((int) row.ExtendedFormatIndex) ? 0 : (int) row.ExtendedFormatIndex;
      if (extendedFormatIndex2 != 0 && extendedFormatIndex2 != this.m_book.DefaultXFIndex)
      {
        extendedFormatIndex1 = (int) row.ExtendedFormatIndex;
      }
      else
      {
        ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[iColumn];
        if (columnInfoRecord != null)
          extendedFormatIndex1 = (int) columnInfoRecord.ExtendedFormatIndex;
      }
    }
    return extendedFormatIndex1 >= 0 ? extendedFormatIndex1 : this.m_book.DefaultXFIndex;
  }

  internal int GetXFIndex(int iRow)
  {
    this.ParseData();
    int num = this.m_dicRecordsCells.GetExtendedFormatIndexByRow(iRow);
    if (num < 0)
    {
      RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iRow - 1, false);
      int extendedFormatIndex = row == null || !this.m_book.IsFormatted((int) row.ExtendedFormatIndex) ? 0 : (int) row.ExtendedFormatIndex;
      if (extendedFormatIndex != 0 && extendedFormatIndex != this.m_book.DefaultXFIndex)
        num = (int) row.ExtendedFormatIndex;
    }
    return num >= 0 ? num : this.m_book.DefaultXFIndex;
  }

  internal int GetColumnXFIndex(int firstColumn)
  {
    this.ParseData();
    int num = this.m_dicRecordsCells.GetExtendedFormatIndexByColumn(firstColumn);
    if (num < 0)
    {
      ColumnInfoRecord record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
      record.FirstColumn = (ushort) (firstColumn - 1);
      int extendedFormatIndex = record == null || !this.m_book.IsFormatted((int) record.ExtendedFormatIndex) ? 0 : (int) record.ExtendedFormatIndex;
      if (extendedFormatIndex != 0 && extendedFormatIndex != this.m_book.DefaultXFIndex)
        num = (int) record.ExtendedFormatIndex;
    }
    return num >= 0 ? num : this.m_book.DefaultXFIndex;
  }

  [CLSCompliant(false)]
  protected internal RKRecord TryCreateRkRecord(int iRow, int iColumn, double value)
  {
    this.ParseData();
    int rkNumber = RKRecord.ConvertToRKNumber(value);
    if (rkNumber == int.MaxValue)
      return (RKRecord) null;
    RKRecord record = (RKRecord) this.GetRecord(TBIFFRecord.RK, iRow, iColumn);
    record.SetConvertedNumber(rkNumber);
    return record;
  }

  [CLSCompliant(false)]
  protected internal RKRecord TryCreateRkRecord(int iRow, int iColumn, double value, int iXFIndex)
  {
    this.ParseData();
    int rkNumber = RKRecord.ConvertToRKNumber(value);
    if (rkNumber == int.MaxValue)
      return (RKRecord) null;
    RKRecord record = (RKRecord) this.GetRecord(TBIFFRecord.RK, iRow, iColumn, iXFIndex);
    record.SetConvertedNumber(rkNumber);
    return record;
  }

  [CLSCompliant(false)]
  public BiffRecordRaw GetRecord(TBIFFRecord recordCode, int iRow, int iColumn)
  {
    return this.GetRecord(recordCode, iRow, iColumn, this.GetXFIndex(iRow, iColumn));
  }

  private BiffRecordRaw GetRecord(TBIFFRecord recordCode, int iRow, int iColumn, int iXFIndex)
  {
    ICellPositionFormat record = this.RecordExtractor.GetRecord((int) recordCode) as ICellPositionFormat;
    record.Row = iRow - 1;
    record.Column = iColumn - 1;
    record.ExtendedFormatIndex = (ushort) iXFIndex;
    return record as BiffRecordRaw;
  }

  private void SetFormulaValue(int iRow, int iColumn, double value)
  {
    this.SetFormulaValue(iRow, iColumn, value, (StringRecord) null);
  }

  private void SetFormulaValue(int iRow, int iColumn, double value, StringRecord strRecord)
  {
    this.ParseData();
    this.m_dicRecordsCells.Table.SetFormulaValue(iRow, iColumn, value, strRecord);
  }

  public string GetFormula(int row, int column, bool bR1C1)
  {
    return this.GetFormula(row, column, bR1C1, false);
  }

  public string GetFormula(int row, int column, bool bR1C1, bool isForSerialization)
  {
    return this.GetFormula(row, column, bR1C1, this.m_book.FormulaUtil, isForSerialization);
  }

  public string GetFormula(
    int row,
    int column,
    bool bR1C1,
    FormulaUtil formulaUtil,
    bool isForSerialization)
  {
    this.ParseData();
    Ptg[] formulaValue = this.m_dicRecordsCells.Table.GetFormulaValue(row, column);
    --row;
    --column;
    return this.GetFormula(row, column, formulaValue, bR1C1, formulaUtil, isForSerialization);
  }

  private string GetFormula(
    int row,
    int column,
    Ptg[] arrTokens,
    bool bR1C1,
    FormulaUtil formulaUtil,
    bool isForSerialization)
  {
    return arrTokens == null ? (string) null : "=" + formulaUtil.ParsePtgArray(arrTokens, row, column, bR1C1, (NumberFormatInfo) null, false, isForSerialization, (IWorksheet) this);
  }

  private string GetFormulaArray(FormulaRecord formula)
  {
    ArrayRecord arrayRecord = this.CellRecords.GetArrayRecord(formula.Row + 1, formula.Column + 1);
    return arrayRecord == null ? (string) null : this.m_book.FormulaUtil.ParsePtgArray(arrayRecord.Formula, arrayRecord.FirstRow, arrayRecord.FirstColumn, false, (NumberFormatInfo) null, false, false, (IWorksheet) this);
  }

  public string GetStringValue(long cellIndex)
  {
    this.ParseData();
    return this.GetText(RangeImpl.GetRowFromCellIndex(cellIndex), RangeImpl.GetColumnFromCellIndex(cellIndex));
  }

  public string GetText(int row, int column)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Table.GetStringValue(row, column, this.m_book.InnerSST);
  }

  public string GetFormulaStringValue(int row, int column)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Table.GetFormulaStringValue(row, column, this.m_book.InnerSST);
  }

  public double GetNumber(int row, int column)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Table.GetNumberValue(row, column);
  }

  public double GetFormulaNumberValue(int row, int column)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Table.GetFormulaNumberValue(row, column);
  }

  public string GetError(int row, int column)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Table.GetErrorValue(row, column);
  }

  internal string GetErrorValueToString(byte value, int row)
  {
    return this.m_dicRecordsCells.Table.GetErrorValue(value, row);
  }

  public string GetFormulaErrorValue(int row, int column)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Table.GetFormulaErrorValue(row, column);
  }

  public bool GetBoolean(int row, int column)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Table.GetBoolValue(row, column) > 0;
  }

  public bool GetFormulaBoolValue(int row, int column)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Table.GetFormulaBoolValue(row, column) > 0;
  }

  public bool HasArrayFormulaRecord(int row, int column)
  {
    this.ParseData();
    return this.HasArrayFormula(this.m_dicRecordsCells.Table.GetFormulaValue(row, column));
  }

  public bool HasArrayFormula(Ptg[] arrTokens)
  {
    if (arrTokens == null || arrTokens.Length != 1)
      return false;
    Ptg arrToken = arrTokens[0];
    if (arrToken.TokenCode != FormulaToken.tExp)
      return false;
    ControlPtg controlPtg = arrToken as ControlPtg;
    return this.m_dicRecordsCells.Table.HasFormulaArrayRecord(controlPtg.RowIndex, controlPtg.ColumnIndex);
  }

  public WorksheetImpl.TRangeValueType GetCellType(int row, int column, bool bNeedFormulaSubType)
  {
    this.ParseData();
    return this.m_dicRecordsCells != null && this.m_dicRecordsCells.Table != null ? this.m_dicRecordsCells.Table.GetCellType(row, column, bNeedFormulaSubType) : WorksheetImpl.TRangeValueType.Error;
  }

  public bool IsExternalFormula(int row, int column)
  {
    this.ParseData();
    Ptg[] formulaValue = this.m_dicRecordsCells.Table.GetFormulaValue(row, column);
    if (formulaValue != null)
    {
      int index = 0;
      for (int length = formulaValue.Length; index < length; ++index)
      {
        if (formulaValue[index] is ISheetReference sheetReference && this.m_book.IsExternalReference((int) sheetReference.RefIndex))
          return true;
      }
    }
    return false;
  }

  internal void OnCellValueChanged(object oldValue, object newValue, IRange range)
  {
    if (this.CellValueChanged == null)
      return;
    this.CellValueChanged((object) this, new CellValueChangedEventArgs()
    {
      OldValue = oldValue,
      NewValue = newValue,
      Range = range
    });
  }

  public int GetFirstRow() => this.Rows[0].Row;

  public int GetLastRow() => this.Rows[this.Rows.Length - 1].Row;

  public int GetRowCount() => this.Rows.Length;

  public int GetFirstColumn() => this.Columns[0].Column;

  public int GetLastColumn() => this.Columns[this.Columns.Length - 1].Column;

  public int GetColumnCount() => this.Columns.Length;

  internal ApplicationImpl GetAppImpl() => this.AppImplementation;

  internal int GetViewColumnWidthPixel(int column)
  {
    WorksheetImpl.CheckColumnIndex(column);
    double columnWidth = this.GetColumnWidth(column + 1);
    double num = this.View == OfficeSheetView.PageLayout ? 1.05 : 1.0;
    return columnWidth > 1.0 ? (int) ((double) ((int) (columnWidth * (double) this.GetAppImpl().GetFontCalc2() + 0.5) + (int) ((double) (this.GetAppImpl().GetFontCalc2() * this.GetAppImpl().GetFontCalc1()) / 256.0 + 0.5)) * num + 0.5) : (int) ((double) (int) (columnWidth * (double) (this.GetAppImpl().GetFontCalc2() + (int) ((double) (this.GetAppImpl().GetFontCalc2() * this.GetAppImpl().GetFontCalc1()) / 256.0 + 0.5)) + 0.5) * num + 0.5);
  }

  internal double CharacterWidth(double width)
  {
    ApplicationImpl appImpl = this.GetAppImpl();
    int num1 = (int) (width * (double) appImpl.GetFontCalc2() + 0.5);
    int fontCalc2 = appImpl.GetFontCalc2();
    int fontCalc1 = appImpl.GetFontCalc1();
    int fontCalc3 = appImpl.GetFontCalc3();
    if (num1 < fontCalc2 + fontCalc3)
      return 1.0 * (double) num1 / (double) (fontCalc2 + fontCalc3);
    double num2 = (double) (int) ((double) (num1 - (int) ((double) (fontCalc2 * fontCalc1) / 256.0 + 0.5)) * 100.0 / (double) fontCalc2 + 0.5) / 100.0;
    if (num2 > (double) byte.MaxValue)
      num2 = (double) byte.MaxValue;
    return num2;
  }

  internal static int CharacterWidth(double width, ApplicationImpl application)
  {
    return width > 1.0 ? (int) (width * (double) application.GetFontCalc2() + 0.5) + (int) ((double) (application.GetFontCalc2() * application.GetFontCalc1()) / 256.0 + 0.5) : (int) (width * (double) (application.GetFontCalc2() + (int) ((double) (application.GetFontCalc2() * application.GetFontCalc1()) / 256.0 + 0.5)) + 0.5);
  }

  internal static void CheckColumnIndex(int columnIndex)
  {
    if (columnIndex < 0 || columnIndex > 16383 /*0x3FFF*/)
      throw new ArgumentException("Invalid column index.");
  }

  internal static void CheckRowIndex(int rowIndex)
  {
    if (rowIndex < 0 || rowIndex > 1048575 /*0x0FFFFF*/)
      throw new ArgumentException("Invalid row index.");
  }

  internal string ArchiveItemName
  {
    get => this.m_archiveItemName;
    set => this.m_archiveItemName = value;
  }

  private enum RangeProperty
  {
    Value2,
    Text,
    DateTime,
    TimeSpan,
    Double,
    Int,
  }

  [Flags]
  public enum TRangeValueType
  {
    Blank = 0,
    Error = 1,
    Boolean = 2,
    Number = 4,
    Formula = 8,
    String = 16, // 0x00000010
  }

  private delegate IOutline OutlineDelegate(int iIndex);
}
