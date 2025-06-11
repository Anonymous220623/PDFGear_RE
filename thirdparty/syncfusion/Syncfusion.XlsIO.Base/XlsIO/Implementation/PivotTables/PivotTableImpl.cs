// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotTableImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.PivotAnalysis;
using Syncfusion.XlsIO.Implementation.XmlSerialization.PivotTables;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class PivotTableImpl : CommonObject, ICloneParent, IPivotTable
{
  public const string DefaultDataFieldStart = "Sum of ";
  public const TBIFFRecord DEF_FIRSTRECORD_CODE = TBIFFRecord.PivotViewDefinition;
  public const byte Excel2007Version = 3;
  private const byte Excel2010Version = 4;
  private const byte Excel2013Version = 5;
  private const byte Excel2016Version = 6;
  private static readonly TBIFFRecord[] DEF_UNKNOWN_PIVOTRECORDS = new TBIFFRecord[10]
  {
    TBIFFRecord.QsiSXTag,
    TBIFFRecord.Delta | TBIFFRecord.QuickTip,
    TBIFFRecord.PivotViewAdditionalInfo,
    TBIFFRecord.RowColumnFieldId | TBIFFRecord.Backup,
    TBIFFRecord.MergeCells | TBIFFRecord.Delta,
    TBIFFRecord.PivotFormat,
    TBIFFRecord.RuleData,
    TBIFFRecord.RuleFilter,
    TBIFFRecord.SelectionInfo,
    TBIFFRecord.DBQueryExt
  };
  private PivotViewDefinitionRecord m_viewDefinition = (PivotViewDefinitionRecord) BiffRecordFactory.GetRecord(TBIFFRecord.PivotViewDefinition);
  private List<PivotFieldImpl> m_arrFields = new List<PivotFieldImpl>();
  private RowColumnFiledIdRecord[] m_arrRowColumnFiledId = new RowColumnFiledIdRecord[2];
  private List<LineItemArrayRecord> m_arrLineItems = new List<LineItemArrayRecord>();
  private ViewExtendedInfoRecord m_viewExInfo = (ViewExtendedInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ViewExtendedInfo);
  private PageItemRecord m_pageItem;
  private List<DataItemRecord> m_arrDataItems = new List<DataItemRecord>();
  private List<BiffRecordRaw> m_arrUnknown = new List<BiffRecordRaw>();
  private WorkbookImpl m_book;
  private PivotTableFields m_arrPivotFields;
  internal IRange m_location;
  private IRange m_endLocation;
  private PivotDataFields m_dataFields;
  private bool m_bItemPrintTitles = true;
  private PivotBuiltInStyles? m_builtInStyle;
  private string customStyleName;
  private WorksheetImpl m_worksheet;
  private List<PivotFieldImpl> m_lstRowFields = new List<PivotFieldImpl>();
  private List<PivotFieldImpl> m_lstColumnFields = new List<PivotFieldImpl>();
  private List<PivotFieldImpl> m_lstPageFields = new List<PivotFieldImpl>();
  private PivotTableOptions m_options;
  private int m_iFirstDataCol;
  private int m_iFirstDataRow;
  private int m_iFirstHeaderRow;
  private int m_iColumnsPerPage;
  private int m_iRowsPerPage;
  private bool m_bShowColHeaderStyle;
  private bool m_bShowColStripes;
  private bool m_bShowLastCol;
  private bool m_bShowRowHeaderStyle;
  private bool m_bShowRowStripes;
  private Stream m_colItemsStream;
  private Stream m_rowItemsStream;
  private bool m_bShowDataFieldInRow;
  private Dictionary<string, Stream> m_preservedElements;
  private PivotCalculatedFields m_calculatedFields;
  private bool m_bIsChanged;
  private List<IPivotField> m_rowFields;
  private List<IPivotField> m_pageFields;
  private List<int> m_colFieldsOrder;
  private List<int> m_rowFieldsOrder;
  private PivotEngine m_PivotEngine;
  private PivotTableFilters m_filters;
  private PivotTableLayout m_PivotTableLayout;
  private PivotFormats m_pivotFormats;
  private Stream m_pivotFormatsStream;
  private List<PivotInnerItem> m_rowFieldsInnerItems;
  private List<PivotInnerItem> m_colFieldsInnerItems;

  public PivotTableImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetWorkbook();
    this.RowGrand = true;
    this.ColumnGrand = true;
    this.m_options = new PivotTableOptions(this, this.m_viewExInfo, this.m_viewDefinition);
    this.m_bShowRowHeaderStyle = true;
    this.m_bShowRowStripes = false;
    this.m_bShowColStripes = false;
    this.m_bShowColHeaderStyle = true;
    this.FirstDataRow = 2;
    this.FirstDataCol = 1;
    this.FirstHeaderRow = 1;
    this.RowsPerPage = 1;
    this.ColumnsPerPage = 1;
    this.ShowDataFieldInRow = false;
    this.m_filters = new PivotTableFilters();
    this.m_pivotFormats = new PivotFormats(this);
  }

  public PivotTableImpl(IApplication application, object parent, int cacheIndex, IRange location)
    : this(application, parent)
  {
    this.CacheIndex = cacheIndex;
    this.m_arrPivotFields = new PivotTableFields(this);
    this.m_dataFields = new PivotDataFields(application, (object) this);
    this.m_location = location;
  }

  private void SetWorkbook()
  {
    this.m_worksheet = this.FindParent(typeof (WorksheetImpl)) as WorksheetImpl;
    this.m_book = this.m_worksheet != null ? this.m_worksheet.ParentWorkbook : throw new ArgumentNullException("Cannot find parent worksheet.");
  }

  internal PivotTableLayout PivotLayout
  {
    get => this.m_PivotTableLayout;
    set => this.m_PivotTableLayout = value;
  }

  internal PivotTableFilters Filters
  {
    get => this.m_filters;
    set => this.m_filters = value;
  }

  public PivotEngine PivotEngineValues
  {
    get => this.m_PivotEngine;
    set => this.m_PivotEngine = value;
  }

  public int CacheIndex
  {
    get => (int) this.m_viewDefinition.CacheIndex;
    set
    {
      this.m_viewDefinition.CacheIndex = value >= 0 && value <= (int) ushort.MaxValue ? (ushort) value : throw new ArgumentOutOfRangeException(nameof (CacheIndex));
    }
  }

  public bool DisplayErrorString
  {
    get => this.m_viewExInfo.IsDisplayErrorString;
    set => this.m_viewExInfo.IsDisplayErrorString = value;
  }

  public bool DisplayNullString
  {
    get => this.m_viewExInfo.IsDisplayNullString;
    set => this.m_viewExInfo.IsDisplayNullString = value;
  }

  public bool ColumnGrand
  {
    get => this.m_viewDefinition.IsColumnGrand;
    set => this.m_viewDefinition.IsColumnGrand = value;
  }

  public bool EnableDrilldown
  {
    get => this.m_viewExInfo.IsEnableDrilldown;
    set => this.m_viewExInfo.IsEnableDrilldown = value;
  }

  public bool EnableFieldDialog
  {
    get => this.m_viewExInfo.IsEnableFieldDialog;
    set => this.m_viewExInfo.IsEnableFieldDialog = value;
  }

  public bool EnableWizard
  {
    get => this.m_viewExInfo.IsEnableWizard;
    set => this.m_viewExInfo.IsEnableWizard = value;
  }

  public string ErrorString
  {
    get => this.m_viewExInfo.ErrorString;
    set => this.m_viewExInfo.ErrorString = value;
  }

  public bool ManualUpdate
  {
    get => this.m_viewExInfo.IsManualUpdate;
    set => this.m_viewExInfo.IsManualUpdate = value;
  }

  public bool MergeLabels
  {
    get => this.m_viewExInfo.IsMergeLabels;
    set => this.m_viewExInfo.IsMergeLabels = value;
  }

  public string Name
  {
    get => this.m_viewDefinition.TableName;
    set => this.m_viewDefinition.TableName = value;
  }

  public string NullString
  {
    get => this.m_viewExInfo.NullString;
    set => this.m_viewExInfo.NullString = value;
  }

  public ExcelPagesOrder PageFieldOrder
  {
    get
    {
      return !this.m_viewExInfo.IsAcrossPageLay ? ExcelPagesOrder.DownThenOver : ExcelPagesOrder.OverThenDown;
    }
    set => this.m_viewExInfo.IsAcrossPageLay = ExcelPagesOrder.OverThenDown == value;
  }

  public string PageFieldStyle
  {
    get => this.m_viewExInfo.PageFieldStyle;
    set => this.m_viewExInfo.PageFieldStyle = value;
  }

  public int PageFieldWrapCount
  {
    get => this.m_options.PageFieldWrapCount;
    set => this.m_options.PageFieldWrapCount = (int) (ushort) value;
  }

  public bool RowGrand
  {
    get => this.m_viewDefinition.IsRowGrand;
    set => this.m_viewDefinition.IsRowGrand = value;
  }

  public PivotCacheImpl Cache => this.m_book.PivotCaches[this.CacheIndex];

  public IRange Location
  {
    get => this.m_location;
    set
    {
      if (!this.Workbook.Loading)
        this.SetChanged(true);
      this.m_location = value;
    }
  }

  public IRange EndLocation
  {
    get => this.m_endLocation;
    set => this.m_endLocation = value;
  }

  internal PivotTableFields InternalFields
  {
    get
    {
      if (this.m_arrPivotFields == null)
        this.m_arrPivotFields = new PivotTableFields(this);
      return this.m_arrPivotFields;
    }
  }

  public PivotTableFields Fields => this.m_arrPivotFields;

  IPivotFields IPivotTable.Fields => (IPivotFields) this.m_arrPivotFields;

  public PivotDataFields DataFields
  {
    get
    {
      if (this.m_dataFields == null)
        this.m_dataFields = new PivotDataFields(this.Application, (object) this);
      return this.m_dataFields;
    }
  }

  IPivotDataFields IPivotTable.DataFields => (IPivotDataFields) this.m_dataFields;

  public WorkbookImpl Workbook => this.m_book;

  public WorksheetImpl Worksheet => this.m_worksheet;

  public bool ShowDrillIndicators
  {
    get => this.m_options.ShowDrillIndicators;
    set => this.m_options.ShowDrillIndicators = value;
  }

  public bool DisplayFieldCaptions
  {
    get => this.m_options.DisplayFieldCaptions;
    set => this.m_options.DisplayFieldCaptions = value;
  }

  public bool RepeatItemsOnEachPrintedPage
  {
    get => this.m_bItemPrintTitles;
    set => this.m_bItemPrintTitles = value;
  }

  public PivotBuiltInStyles? BuiltInStyle
  {
    get => this.m_builtInStyle;
    set => this.m_builtInStyle = value;
  }

  public string CustomStyleName
  {
    get => this.customStyleName;
    set => this.customStyleName = value;
  }

  public bool ShowRowGrand
  {
    get => this.ColumnGrand;
    set => this.ColumnGrand = value;
  }

  public bool ShowColumnGrand
  {
    get => this.RowGrand;
    set => this.RowGrand = value;
  }

  public IPivotTableOptions Options => (IPivotTableOptions) this.m_options;

  public int FirstDataCol
  {
    get => this.m_iFirstDataCol;
    set => this.m_iFirstDataCol = value;
  }

  public int FirstDataRow
  {
    get => this.m_iFirstDataRow;
    set => this.m_iFirstDataRow = value;
  }

  public int FirstHeaderRow
  {
    get => this.m_iFirstHeaderRow;
    set => this.m_iFirstHeaderRow = value;
  }

  public int ColumnsPerPage
  {
    get => this.m_iColumnsPerPage;
    set => this.m_iColumnsPerPage = value;
  }

  public int RowsPerPage
  {
    get => this.m_iRowsPerPage;
    set => this.m_iRowsPerPage = value;
  }

  public bool ShowColHeaderStyle
  {
    get => this.m_bShowColHeaderStyle;
    set => this.m_bShowColHeaderStyle = value;
  }

  public bool ShowColStripes
  {
    get => this.m_bShowColStripes;
    set => this.m_bShowColStripes = value;
  }

  public bool ShowLastCol
  {
    get => this.m_bShowLastCol;
    set => this.m_bShowLastCol = value;
  }

  public bool ShowRowHeaderStyle
  {
    get => this.m_bShowRowHeaderStyle;
    set => this.m_bShowRowHeaderStyle = value;
  }

  public bool ShowRowStripes
  {
    get => this.m_bShowRowStripes;
    set => this.m_bShowRowStripes = value;
  }

  internal Stream ColumnItemsStream
  {
    get => this.m_colItemsStream;
    set => this.m_colItemsStream = value;
  }

  internal Stream RowItemsStream
  {
    get => this.m_rowItemsStream;
    set => this.m_rowItemsStream = value;
  }

  public bool ShowDataFieldInRow
  {
    get => this.m_bShowDataFieldInRow;
    set
    {
      if (this.m_bShowDataFieldInRow == value)
        return;
      if (!this.m_book.Loading)
        this.SetChanged(true);
      this.m_bShowDataFieldInRow = value;
    }
  }

  public new IApplication Application => this.m_book.Application;

  internal Dictionary<string, Stream> PreservedElements
  {
    get
    {
      if (this.m_preservedElements == null)
        this.m_preservedElements = new Dictionary<string, Stream>();
      return this.m_preservedElements;
    }
  }

  public IPivotCalculatedFields CalculatedFields
  {
    get => (IPivotCalculatedFields) this.GetCalculatedFields();
  }

  public IPivotFields PageFields => (IPivotFields) this.GetPivotFields(PivotAxisTypes.Page);

  internal List<IPivotField> PivotRowFields
  {
    get
    {
      if (this.m_rowFields == null)
        this.m_rowFields = new List<IPivotField>();
      return this.m_rowFields;
    }
  }

  internal List<IPivotField> PivotPageFields
  {
    get
    {
      if (this.m_pageFields == null)
        this.m_pageFields = new List<IPivotField>();
      return this.m_pageFields;
    }
  }

  public IPivotFields RowFields => (IPivotFields) this.GetPivotFields(PivotAxisTypes.Row);

  public IPivotFields ColumnFields => (IPivotFields) this.GetPivotFields(PivotAxisTypes.Column);

  public bool IsChanged
  {
    get => this.m_bIsChanged;
    set => this.m_bIsChanged = value;
  }

  internal List<int> ColFieldsOrder
  {
    get
    {
      if (this.m_colFieldsOrder == null)
        this.m_colFieldsOrder = new List<int>();
      return this.m_colFieldsOrder;
    }
  }

  internal List<int> RowFieldsOrder
  {
    get
    {
      if (this.m_rowFieldsOrder == null)
        this.m_rowFieldsOrder = new List<int>();
      return this.m_rowFieldsOrder;
    }
  }

  internal PivotFormats PivotFormats => this.m_pivotFormats;

  internal Stream PivotFormatsStream
  {
    get => this.m_pivotFormatsStream;
    set => this.m_pivotFormatsStream = value;
  }

  internal List<PivotInnerItem> RowFieldsInnerItems
  {
    get
    {
      if (this.m_rowFieldsInnerItems == null)
      {
        List<PivotFieldImpl> fields = new List<PivotFieldImpl>();
        foreach (int i in this.RowFieldsOrder)
        {
          if (i >= 0 && i < this.Fields.Count)
            fields.Add(this.Fields[i]);
        }
        if (fields.Count > 0)
          this.m_rowFieldsInnerItems = this.GetPivotInnerItems(fields);
      }
      return this.m_rowFieldsInnerItems;
    }
  }

  internal List<PivotInnerItem> ColumnFieldsInnerItems
  {
    get
    {
      if (this.m_colFieldsInnerItems == null)
      {
        List<PivotFieldImpl> fields = new List<PivotFieldImpl>();
        foreach (int i in this.ColFieldsOrder)
        {
          if (i >= 0 && i < this.Fields.Count)
            fields.Add(this.Fields[i]);
          else if (i == -2)
          {
            PivotFieldImpl Parent = new PivotFieldImpl(this);
            foreach (PivotDataField dataField in (CollectionBase<PivotDataField>) this.DataFields)
              (Parent.Items as PivotFieldItemsCollections).Add((object) Parent, dataField.Name, dataField.Name);
            fields.Add(Parent);
          }
        }
        if (fields.Count > 0)
          this.m_colFieldsInnerItems = this.GetPivotInnerItems(fields);
      }
      return this.m_colFieldsInnerItems;
    }
  }

  public void Layout()
  {
    PivotEngineSerialization engineSerialization = new PivotEngineSerialization();
    PivotTableImpl pivotTable = this;
    pivotTable.Location.Clear();
    if (this.Cache.SourceRange != null)
    {
      for (int i = 0; i < pivotTable.Cache.CacheFields.Count; ++i)
      {
        PivotCacheFieldImpl cacheField = pivotTable.Cache.CacheFields[i];
        if (string.IsNullOrEmpty(cacheField.Name) && cacheField.ItemRange != null)
          cacheField.Name = this.Cache.SourceRange.Worksheet[cacheField.ItemRange.Row - 1, cacheField.ItemRange.Column].Value;
        if (pivotTable.Fields[i].Name != cacheField.Name)
          pivotTable.Fields[i].Name = cacheField.Name;
        cacheField.ItemRange = this.Cache.SourceRange.Worksheet[this.Cache.SourceRange.Row + 1, this.Cache.SourceRange.Column + i, this.Cache.SourceRange.LastRow, this.Cache.SourceRange.Column + i];
        cacheField.Items = (IList<object>) null;
        cacheField.IsParsed = new bool?(true);
        cacheField.Fill(this.Cache.SourceRange.Worksheet, this.Cache.SourceRange.Row, this.Cache.SourceRange.LastRow, this.Cache.SourceRange.Column + i);
        PivotFieldImpl field = pivotTable.Fields[i];
        List<PivotTableSerializator.ComparisonPair> comparisonPairList = PivotTableSerializator.SortFieldValues(cacheField, (List<string>) null);
        if (comparisonPairList.Count > 0)
        {
          int index1 = 0;
          (field.Items as PivotFieldItemsCollections).Clear();
          for (int index2 = 0; index2 < comparisonPairList.Count; ++index2)
          {
            string str = comparisonPairList[index1].Value == null ? (string) null : comparisonPairList[index1].Value.ToString();
            (field.Items as PivotFieldItemsCollections).Add((object) this, str, str);
            ++index1;
          }
        }
      }
    }
    pivotTable.Cache.IsRefreshOnLoad = false;
    PivotEngine pivotEngine = engineSerialization.PopulatePivotEngine((IWorksheet) this.Worksheet, pivotTable);
    pivotTable.PivotEngineValues = pivotEngine;
  }

  public IPivotCellFormat GetCellFormat(string range)
  {
    IRange pivotRange = this.Location.IntersectWith(this.m_worksheet[range]);
    return pivotRange != null ? this.GetPivotCellFormat(pivotRange) : (IPivotCellFormat) null;
  }

  public int Parse(IList data, int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (iPos < 0 || iPos > data.Count - 1)
      throw new ArgumentOutOfRangeException(nameof (iPos), "Value cannot be less than 0 and greater than data.Length - 1");
    this.ClearCollections();
    BiffRecordRaw biffRecordRaw1 = (BiffRecordRaw) data[iPos];
    biffRecordRaw1.CheckTypeCode(TBIFFRecord.PivotViewDefinition);
    this.m_viewDefinition = (PivotViewDefinitionRecord) biffRecordRaw1;
    ++iPos;
    BiffRecordRaw biffRecordRaw2 = (BiffRecordRaw) data[iPos];
    int num1 = 0;
    for (int fieldsNumber = (int) this.m_viewDefinition.FieldsNumber; num1 < fieldsNumber; ++num1)
    {
      PivotFieldImpl pivotFieldImpl = new PivotFieldImpl(this);
      iPos = pivotFieldImpl.Parse(data, iPos);
      this.m_arrFields.Add(pivotFieldImpl);
    }
    BiffRecordRaw biffRecordRaw3 = (BiffRecordRaw) data[iPos];
    int index = 0;
    for (; biffRecordRaw3.TypeCode == TBIFFRecord.RowColumnFieldId; biffRecordRaw3 = (BiffRecordRaw) data[iPos])
    {
      this.m_arrRowColumnFiledId[index] = (RowColumnFiledIdRecord) biffRecordRaw3;
      ++index;
      ++iPos;
    }
    int[] numArray = new int[2]
    {
      (int) this.m_viewDefinition.RowFieldsNumber,
      (int) this.m_viewDefinition.ColumnFieldsNumber
    };
    int num2 = 0;
    if (biffRecordRaw3.TypeCode == TBIFFRecord.PageItem)
    {
      this.m_pageItem = (PageItemRecord) biffRecordRaw3;
      ++iPos;
      biffRecordRaw3 = (BiffRecordRaw) data[iPos];
    }
    for (; biffRecordRaw3.TypeCode == TBIFFRecord.DataItem; biffRecordRaw3 = (BiffRecordRaw) data[iPos])
    {
      this.m_arrDataItems.Add((DataItemRecord) biffRecordRaw3);
      ++iPos;
    }
    for (; biffRecordRaw3.TypeCode == TBIFFRecord.LineItemArray; biffRecordRaw3 = (BiffRecordRaw) data[iPos])
    {
      LineItemArrayRecord lineItemArrayRecord = (LineItemArrayRecord) biffRecordRaw3;
      lineItemArrayRecord.ParseStructure(numArray[num2++]);
      this.m_arrLineItems.Add(lineItemArrayRecord);
      ++iPos;
    }
    biffRecordRaw3.CheckTypeCode(TBIFFRecord.ViewExtendedInfo);
    this.m_viewExInfo = (ViewExtendedInfoRecord) biffRecordRaw3;
    ++iPos;
    for (BiffRecordRaw biffRecordRaw4 = (BiffRecordRaw) data[iPos]; Array.IndexOf<TBIFFRecord>(PivotTableImpl.DEF_UNKNOWN_PIVOTRECORDS, biffRecordRaw4.TypeCode) != -1; biffRecordRaw4 = (BiffRecordRaw) data[iPos])
    {
      this.m_arrUnknown.Add(biffRecordRaw4);
      ++iPos;
    }
    return iPos;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    records.Add((IBiffStorage) this.m_viewDefinition);
    int index1 = 0;
    for (int count = this.m_arrFields.Count; index1 < count; ++index1)
      this.m_arrFields[index1].Serialize(records);
    int index2 = 0;
    for (int length = this.m_arrRowColumnFiledId.Length; index2 < length; ++index2)
    {
      RowColumnFiledIdRecord columnFiledIdRecord = this.m_arrRowColumnFiledId[index2];
      if (columnFiledIdRecord != null)
        records.Add((IBiffStorage) columnFiledIdRecord);
      else
        break;
    }
    if (this.m_pageItem != null)
      records.Add((IBiffStorage) this.m_pageItem);
    records.AddList((IList) this.m_arrDataItems);
    records.AddList((IList) this.m_arrLineItems);
    records.Add((IBiffStorage) this.m_viewExInfo);
    records.AddList((IList) this.m_arrUnknown);
  }

  private void ClearCollections()
  {
    this.m_arrFields.Clear();
    this.m_arrLineItems.Clear();
    this.m_arrDataItems.Clear();
    this.m_arrUnknown.Clear();
  }

  internal void RemovePivotField(PivotAxisTypes pivotAxisTypes, PivotFieldImpl field)
  {
    List<PivotFieldImpl> fields = this.GetFields(pivotAxisTypes);
    switch (pivotAxisTypes)
    {
      case PivotAxisTypes.Row:
        this.PivotRowFields.Remove((IPivotField) field);
        break;
      case PivotAxisTypes.Page:
        this.PivotPageFields.Remove((IPivotField) field);
        break;
    }
    fields?.Remove(field);
  }

  internal void AddPivotField(PivotAxisTypes pivotAxisTypes, PivotFieldImpl field, bool isData)
  {
    List<PivotFieldImpl> fields = this.GetFields(pivotAxisTypes);
    if (fields != null)
      fields.Add(field);
    else if (this.m_arrPivotFields.Count < this.Cache.CacheFields.Count)
    {
      this.m_arrPivotFields.Add(field);
      field.Axis = pivotAxisTypes;
    }
    if (!isData)
      return;
    string name = "Sum of  " + field.Name;
    this.DataFields.Add((IPivotField) field, name, PivotSubtotalTypes.Sum);
  }

  internal List<PivotFieldImpl> GetFields(PivotAxisTypes pivotAxisTypes)
  {
    List<PivotFieldImpl> fields = (List<PivotFieldImpl>) null;
    switch (pivotAxisTypes)
    {
      case PivotAxisTypes.Row:
        fields = this.m_lstRowFields;
        break;
      case PivotAxisTypes.Column:
        fields = this.m_lstColumnFields;
        break;
      case PivotAxisTypes.Page:
        fields = this.m_lstPageFields;
        break;
    }
    return fields;
  }

  internal PivotTableFields GetPivotFields(PivotAxisTypes pivotAxisTypes)
  {
    PivotTableFields pivotFields = new PivotTableFields(this.Workbook.Application, (object) this);
    if (this.m_arrPivotFields != null)
    {
      foreach (PivotFieldImpl arrPivotField in (CollectionBase<PivotFieldImpl>) this.m_arrPivotFields)
      {
        if (arrPivotField.Axis == pivotAxisTypes)
          pivotFields.Add(arrPivotField);
      }
    }
    return pivotFields;
  }

  internal byte GetPivotVersion()
  {
    if (this.m_book.Version == ExcelVersion.Excel2007)
      return 3;
    if (this.m_book.Version == ExcelVersion.Excel2010)
      return 4;
    if (this.m_book.Version == ExcelVersion.Excel2013)
      return 5;
    return this.m_book.Version == ExcelVersion.Excel2016 || this.m_book.Version == ExcelVersion.Xlsx ? (byte) 6 : (byte) 2;
  }

  private IPivotCellFormat GetPivotCellFormat(IRange pivotRange)
  {
    if ((pivotRange as RangeImpl).IsSingleCell)
      return this.GetPivotFormat(pivotRange).PivotCellFormat;
    List<PivotCellFormat> pivotCellFormats = new List<PivotCellFormat>();
    List<PivotFormat> pivotFormats = new List<PivotFormat>();
    IRange[] cells = pivotRange.Cells;
    int index = 0;
    for (int length = cells.Length; index < length; ++index)
    {
      PivotFormat pivotFormat = this.GetPivotFormat(cells[index]);
      pivotCellFormats.Add(pivotFormat.PivotCellFormat as PivotCellFormat);
      pivotFormats.Add(pivotFormat);
    }
    return (IPivotCellFormat) new PivotCellFormatWrapper(pivotFormats, pivotCellFormats);
  }

  private PivotFormat GetPivotFormat(IRange pivotCell)
  {
    PivotFormat pivotFormat = new PivotFormat(this);
    PivotArea pivotArea = pivotFormat.PivotArea;
    this.m_pivotFormatsStream = (Stream) null;
    int column = pivotCell.Column;
    int row = pivotCell.Row;
    int count1 = this.ColumnFields.Count;
    if (this.ColumnFields.Count > 0 && this.DataFields.Count > 1)
      ++count1;
    int count2 = this.RowFields.Count;
    int num1;
    int num2;
    if (this.m_options.RowLayout == PivotTableRowLayout.Compact)
    {
      num1 = pivotCell.Column - this.Location.Column;
      num2 = pivotCell.Row - (this.Location.Row + count1);
    }
    else
    {
      num1 = pivotCell.Column - count2;
      num2 = pivotCell.Row - (this.Location.Row + count1);
    }
    if (!this.UpdatePivotArea(pivotArea, num1, num2, this.m_options.RowLayout))
    {
      if (num2 == 0 || count2 > 0 && num1 <= 0)
        pivotArea.IsLableOnly = true;
      List<PivotInnerItem> pivotInnerItemList = new List<PivotInnerItem>();
      if (num1 > 0 || count2 == 0)
      {
        PivotInnerItem pivotInnerItem;
        if (count2 == 0 && count1 == 0)
        {
          pivotInnerItem = this.ColumnFieldsInnerItems[num1];
        }
        else
        {
          PivotInnerItem innerItem = this.SortByFields(this.ColumnFieldsInnerItems, num1, true);
          pivotInnerItem = this.UpdateInnerItem(pivotArea, num1, num2, innerItem, true, this.m_options.RowLayout);
        }
        if (pivotInnerItem != null)
        {
          if (pivotInnerItem.FieldIndex == (int) short.MaxValue)
          {
            if (this.ColumnGrand && count1 > 0)
            {
              pivotArea.Axis = PivotAxisTypes.Row;
              pivotArea.HasColumnGrand = true;
              pivotArea.CollapsedLevelsAreSubtotals = true;
            }
            else if (count1 == 0)
              pivotArea.Axis = PivotAxisTypes.Data;
          }
          else
            pivotInnerItemList.Add(pivotInnerItem);
        }
      }
      if (num2 > 0)
      {
        PivotInnerItem innerItem = this.SortByFields(this.RowFieldsInnerItems, num2, this.m_options.RowLayout == PivotTableRowLayout.Tabular);
        if (this.m_options.RowLayout != PivotTableRowLayout.Compact)
          innerItem = this.UpdateInnerItem(pivotArea, num1, num2, innerItem, false, this.m_options.RowLayout);
        if (innerItem != null)
        {
          if (innerItem.FieldIndex == (int) short.MaxValue)
          {
            if (this.RowGrand && count2 > 0)
            {
              pivotArea.Axis = PivotAxisTypes.Column;
              pivotArea.HasRowGrand = true;
              pivotArea.CollapsedLevelsAreSubtotals = true;
            }
          }
          else
            pivotInnerItemList.Add(innerItem);
        }
      }
      if (pivotInnerItemList.Count > 0)
      {
        foreach (PivotInnerItem pivotInnerItem in pivotInnerItemList)
          this.AddReferences(pivotInnerItem, pivotArea);
      }
    }
    int index = this.PivotFormats.IndexOf(pivotFormat);
    if (index != -1)
      return this.PivotFormats[index];
    IPivotCellFormat pivotCellFormat = (IPivotCellFormat) new PivotCellFormat(pivotFormat);
    pivotFormat.PivotCellFormat = pivotCellFormat;
    this.PivotFormats.Add(pivotFormat);
    return pivotFormat;
  }

  private bool UpdatePivotArea(
    PivotArea pivotArea,
    int column,
    int row,
    PivotTableRowLayout layout)
  {
    bool flag = false;
    int count1 = this.ColumnFields.Count;
    if (this.ColumnFields.Count > 0 && this.DataFields.Count > 1)
      ++count1;
    int count2 = this.RowFields.Count;
    if (this.m_options.RowLayout == PivotTableRowLayout.Compact)
    {
      if (row < 0)
      {
        if (column == 0)
        {
          pivotArea.AreaType = PivotAreaType.Orgin;
          pivotArea.Range = this.m_worksheet[count1 + row + 1, 1];
          pivotArea.Offset = pivotArea.Range.AddressLocal;
          pivotArea.IsLableOnly = true;
          flag = true;
        }
        else if (row == -count1 && column <= count1)
        {
          pivotArea.AreaType = PivotAreaType.FieldButton;
          pivotArea.Axis = PivotAxisTypes.Column;
          pivotArea.IsLableOnly = true;
          pivotArea.FieldPosition = column - 1;
          flag = true;
        }
        else if (row == -count1)
        {
          pivotArea.AreaType = PivotAreaType.TopRight;
          pivotArea.IsLableOnly = true;
          pivotArea.Range = this.m_worksheet[1, column - count1];
          pivotArea.Offset = pivotArea.Range.AddressLocal;
          pivotArea.FieldPosition = 0;
          flag = true;
        }
      }
      else if (row == 0 && column == 0 && count2 > 0)
      {
        pivotArea.AreaType = PivotAreaType.FieldButton;
        pivotArea.Axis = PivotAxisTypes.Row;
        pivotArea.IsLableOnly = true;
        pivotArea.FieldPosition = 0;
        flag = true;
      }
    }
    else if (row <= 0)
    {
      if (row < 0 && column <= 0)
      {
        pivotArea.AreaType = PivotAreaType.Orgin;
        pivotArea.Range = this.m_worksheet[count1 + row + 1, count2 + column];
        pivotArea.Offset = pivotArea.Range.AddressLocal;
        pivotArea.IsLableOnly = true;
        flag = true;
      }
      else if (row == -count1 && column <= count1)
      {
        pivotArea.AreaType = PivotAreaType.FieldButton;
        pivotArea.Axis = PivotAxisTypes.Column;
        pivotArea.IsLableOnly = true;
        pivotArea.FieldPosition = column - 1;
        flag = true;
      }
      else if (column <= 0 && row == 0 && count2 > 0)
      {
        pivotArea.AreaType = PivotAreaType.FieldButton;
        pivotArea.Axis = PivotAxisTypes.Row;
        pivotArea.IsLableOnly = true;
        pivotArea.FieldPosition = count2 - 1 + column;
        flag = true;
      }
      else if (row == -count1)
      {
        pivotArea.AreaType = PivotAreaType.TopRight;
        pivotArea.IsLableOnly = true;
        pivotArea.Range = this.m_worksheet[1, column - count1];
        pivotArea.Offset = pivotArea.Range.AddressLocal;
        pivotArea.FieldPosition = 0;
        flag = true;
      }
    }
    pivotArea.IsOutline = !flag;
    return flag;
  }

  private PivotInnerItem UpdateInnerItem(
    PivotArea pivotArea,
    int column,
    int row,
    PivotInnerItem innerItem,
    bool isColumn,
    PivotTableRowLayout layout)
  {
    if (!isColumn)
    {
      int num = column;
      column = row;
      row = num;
    }
    PivotInnerItem tempInnerItem = innerItem;
    int num1 = isColumn ? this.ColumnFields.Count : this.RowFields.Count;
    if (isColumn && this.ColumnFields.Count > 0 && this.DataFields.Count > 1)
      ++num1;
    if (num1 > 1 && row <= 0)
    {
      int num2 = num1 - 1 + row;
      int parentCount = PivotTableImpl.GetParentCount(tempInnerItem);
      PivotInnerItem pivotInnerItem = innerItem;
      for (int index = 0; index < parentCount - num2; ++index)
      {
        if (pivotInnerItem.Parent is PivotInnerItem)
        {
          pivotInnerItem.IsSubtotal = false;
          pivotInnerItem = pivotInnerItem.Parent as PivotInnerItem;
        }
      }
      pivotArea.IsLableOnly = true;
      int index1 = 0;
      this.GetCurrentIndex(isColumn ? this.ColumnFieldsInnerItems : this.RowFieldsInnerItems, pivotInnerItem, ref index1);
      int num3;
      int column1 = num3 = column - index1;
      int row1 = 1;
      if (pivotInnerItem.IsSubtotal || pivotInnerItem.Name == "Grand Total")
      {
        column1 = 256 /*0x0100*/;
        if (parentCount - num2 != 0)
          row1 = num2 - parentCount + 1;
      }
      if (!isColumn)
      {
        int num4 = column1;
        column1 = row1;
        row1 = num4;
      }
      if (!isColumn && layout == PivotTableRowLayout.Outline && pivotInnerItem.Name != "Grand Total")
      {
        if (parentCount - num2 == 0)
        {
          column1 = 256 /*0x0100*/;
          row1 = 1;
          pivotArea.IsOutline = false;
        }
        else if (parentCount - num2 > 0)
        {
          column1 = 256 /*0x0100*/;
          row1 -= PivotTableImpl.GetParentCount(pivotInnerItem);
          pivotArea.IsOutline = false;
        }
        else if (parentCount - num2 < 0)
        {
          column1 = num2 - parentCount + 1;
          row1 = 256 /*0x0100*/;
        }
      }
      pivotArea.Range = this.m_worksheet[row1, column1];
      innerItem = pivotInnerItem;
    }
    return innerItem;
  }

  private static int GetParentCount(PivotInnerItem tempInnerItem)
  {
    int parentCount = 0;
    while (tempInnerItem != null)
    {
      tempInnerItem = tempInnerItem.Parent as PivotInnerItem;
      if (tempInnerItem != null)
        ++parentCount;
    }
    return parentCount;
  }

  private int GetCurrentIndex(
    List<PivotInnerItem> innerItems,
    PivotInnerItem innerItem,
    ref int index)
  {
    int currentIndex = -1;
    for (int index1 = 0; index1 < innerItems.Count; ++index1)
    {
      if (innerItems[index1] == innerItem)
        return index;
      if (innerItems[index1].Items != null)
        currentIndex = this.GetCurrentIndex(innerItems[index1].Items, innerItem, ref index);
      if (currentIndex == index)
        return index;
      ++index;
    }
    return currentIndex;
  }

  internal PivotInnerItem SortByFields(
    List<PivotInnerItem> pivotInnerItems,
    int lineNumber,
    bool isColumnSort)
  {
    int index = 0;
    PivotInnerItem pivotInnerItem = this.GetPivotInnerItem(pivotInnerItems, ref index, lineNumber, (PivotInnerItem) null, isColumnSort);
    if (pivotInnerItem == null && lineNumber == index + 1)
    {
      pivotInnerItem = new PivotInnerItem("Grand Total", (object) null);
      pivotInnerItem.FieldIndex = (int) short.MaxValue;
    }
    return pivotInnerItem;
  }

  private List<PivotInnerItem> GetPivotInnerItems(List<PivotFieldImpl> fields)
  {
    for (int index1 = 0; index1 < fields.Count; ++index1)
    {
      PivotFieldImpl field = fields[index1];
      List<PivotTableSerializator.ComparisonPair> comparisonPairList = PivotTableSerializator.SortFieldValues(field);
      for (int index2 = 0; index2 < comparisonPairList.Count; ++index2)
      {
        if (comparisonPairList[index2].Value != null && !field.SortedFieldItems.ContainsKey(comparisonPairList[index2].Value.ToString()))
          field.SortedFieldItems.Add(comparisonPairList[index2].Value.ToString(), index2);
      }
    }
    PivotFieldImpl field1 = fields[0];
    List<PivotInnerItem> pivotInnerItems = new List<PivotInnerItem>();
    for (int index = 0; index < field1.Items.Count; ++index)
    {
      if (!string.IsNullOrEmpty((field1.Items[index] as PivotFieldItem).Name))
      {
        PivotInnerItem pivotInnerItem = new PivotInnerItem((field1.Items[index] as PivotFieldItem).Name, (object) field1);
        pivotInnerItem.FieldIndex = field1.CacheField == null ? int.MaxValue : field1.CacheField.Index;
        pivotInnerItems.Add(pivotInnerItem);
        pivotInnerItem.ValueIndex = field1.SortedFieldItems[pivotInnerItem.Name];
      }
    }
    if (field1.Axis != PivotAxisTypes.None)
    {
      List<PivotTableSerializator.ComparisonPair> comparisonPairList = PivotTableSerializator.SortPivotInnerItems(pivotInnerItems, field1.PivotTable);
      List<PivotInnerItem> pivotInnerItemList = new List<PivotInnerItem>();
      for (int index3 = 0; index3 < comparisonPairList.Count; ++index3)
      {
        int index4 = PivotInnerItem.GetIndex(pivotInnerItems, (string) comparisonPairList[index3].Value);
        pivotInnerItemList.Add(pivotInnerItems[index4]);
      }
      pivotInnerItems = pivotInnerItemList;
    }
    if (fields.Count > 1)
      this.AddInnerItems(pivotInnerItems, fields, 1);
    return pivotInnerItems;
  }

  private PivotInnerItem GetPivotInnerItem(
    List<PivotInnerItem> innerItems,
    ref int index,
    int lineNumber,
    PivotInnerItem pivotInnerItem,
    bool isColumnSort)
  {
    if (innerItems != null)
    {
      for (int index1 = 0; index1 < innerItems.Count; ++index1)
      {
        if (!isColumnSort || innerItems[index1].Items == null)
          ++index;
        if (index == lineNumber)
        {
          pivotInnerItem = innerItems[index1];
          break;
        }
        pivotInnerItem = this.GetPivotInnerItem(innerItems[index1].Items, ref index, lineNumber, pivotInnerItem, isColumnSort);
        if (pivotInnerItem != null)
          return pivotInnerItem;
        if (isColumnSort && innerItems[index1].Items != null)
        {
          ++index;
          if (index == lineNumber)
          {
            pivotInnerItem = innerItems[index1];
            pivotInnerItem.IsSubtotal = true;
            break;
          }
        }
      }
    }
    return pivotInnerItem;
  }

  private void AddInnerItems(List<PivotInnerItem> items, List<PivotFieldImpl> fields, int index)
  {
    for (int index1 = 0; index1 < items.Count; ++index1)
    {
      PivotInnerItem pivotItem = items[index1];
      this.AddPivotItems(fields[index], pivotItem, fields[index - 1].CacheField != null ? fields[index - 1].CacheField.Index : int.MaxValue);
      if (pivotItem.Items != null && pivotItem.Items.Count > 0 && index < fields.Count - 1)
        this.AddInnerItems(pivotItem.Items, fields, index + 1);
    }
  }

  private void AddPivotItems(PivotFieldImpl field, PivotInnerItem pivotItem, int parentFieldIndex)
  {
    List<PivotInnerItem> pivotInnerItemList1 = new List<PivotInnerItem>();
    if (field.IsShowAllItems || parentFieldIndex == int.MaxValue)
    {
      int index = 0;
      for (int count = field.Items.Count; index < count; ++index)
      {
        string text = field.Items[index].Text;
        if (PivotInnerItem.GetIndex(pivotInnerItemList1, text) == -1)
        {
          PivotInnerItem pivotInnerItem = new PivotInnerItem(text, (object) pivotItem);
          pivotInnerItem.FieldIndex = field.CacheField.Index;
          pivotInnerItemList1.Add(pivotInnerItem);
          pivotInnerItem.ValueIndex = field.SortedFieldItems[text];
        }
      }
    }
    else
    {
      IRange sourceRange = field.m_table.Cache.SourceRange;
      IWorksheet worksheet = sourceRange.Worksheet;
      MigrantRangeImpl range = new MigrantRangeImpl(worksheet.Application, worksheet);
      for (int index = sourceRange.Row + 1; index <= sourceRange.LastRow; ++index)
      {
        range.ResetRowColumn(index, sourceRange.Column + parentFieldIndex);
        if (this.CheckItem(sourceRange, range, pivotItem, index))
        {
          range.ResetRowColumn(index, sourceRange.Column + field.CacheField.Index);
          string str = (worksheet as WorksheetImpl).GetTextFromCellType(range.Row, range.Column, out WorksheetImpl.TRangeValueType _)?.ToString();
          if (!string.IsNullOrEmpty(str) && PivotInnerItem.GetIndex(pivotInnerItemList1, str) == -1)
          {
            PivotInnerItem pivotInnerItem = new PivotInnerItem(str, (object) pivotItem);
            pivotInnerItem.FieldIndex = field.CacheField.Index;
            pivotInnerItemList1.Add(pivotInnerItem);
            pivotInnerItem.ValueIndex = field.SortedFieldItems[str];
          }
        }
      }
    }
    if (field.Axis != PivotAxisTypes.None)
    {
      List<PivotTableSerializator.ComparisonPair> comparisonPairList = PivotTableSerializator.SortPivotInnerItems(pivotInnerItemList1, field.PivotTable);
      List<PivotInnerItem> pivotInnerItemList2 = new List<PivotInnerItem>();
      for (int index1 = 0; index1 < comparisonPairList.Count; ++index1)
      {
        int index2 = PivotInnerItem.GetIndex(pivotInnerItemList1, (string) comparisonPairList[index1].Value);
        pivotInnerItemList2.Add(pivotInnerItemList1[index2]);
      }
      pivotItem.Items = pivotInnerItemList2;
    }
    else
      pivotItem.Items = pivotInnerItemList1;
  }

  private bool CheckItem(
    IRange sourceRange,
    MigrantRangeImpl range,
    PivotInnerItem pivotItem,
    int row)
  {
    IWorksheet worksheet = sourceRange.Worksheet;
    WorksheetImpl.TRangeValueType type;
    object textFromCellType1 = (worksheet as WorksheetImpl).GetTextFromCellType(range.Row, range.Column, out type);
    if (type == WorksheetImpl.TRangeValueType.Blank || textFromCellType1 == null || !(textFromCellType1.ToString() == pivotItem.Name))
      return false;
    for (pivotItem = pivotItem.Parent as PivotInnerItem; pivotItem != null; pivotItem = pivotItem.Parent as PivotInnerItem)
    {
      range.ResetRowColumn(row, sourceRange.Column + pivotItem.FieldIndex);
      object textFromCellType2 = (worksheet as WorksheetImpl).GetTextFromCellType(range.Row, range.Column, out type);
      if (textFromCellType2 == null || pivotItem.Name != textFromCellType2.ToString())
        return false;
    }
    return true;
  }

  internal void AddReferences(PivotInnerItem pivotInnerItem, PivotArea pivotArea)
  {
    for (PivotInnerItem pivotInnerItem1 = pivotInnerItem; pivotInnerItem1 != null; pivotInnerItem1 = pivotInnerItem1.Parent as PivotInnerItem)
    {
      PivotAreaReference reference = new PivotAreaReference();
      reference.FieldIndex = pivotInnerItem1.FieldIndex;
      reference.Indexes.Add(pivotInnerItem1.ValueIndex);
      if (pivotInnerItem1.IsSubtotal)
      {
        reference.IsDefaultSubTotal = true;
        pivotInnerItem1.IsSubtotal = false;
      }
      pivotArea.References.Add(reference);
      if (pivotInnerItem1.Parent is PivotFieldImpl)
        break;
    }
  }

  internal Dictionary<long, ExtendedFormatImpl> ApplyPivotFormats(PivotTableLayout layout)
  {
    Dictionary<long, ExtendedFormatImpl> pivotFormats = new Dictionary<long, ExtendedFormatImpl>();
    foreach (PivotFormat format in this.PivotFormats.Formats)
    {
      PivotArea pivotArea = format.PivotArea;
      int firstRow = 0;
      int firstColumn = 0;
      int count1 = this.ColumnFields.Count;
      if (this.ColumnFields.Count > 0 && this.DataFields.Count > 1)
        ++count1;
      int count2 = this.RowFields.Count;
      if (pivotArea.AreaType == PivotAreaType.Orgin)
      {
        if (pivotArea.Range != null)
        {
          firstRow += pivotArea.Range.Row;
          firstColumn += pivotArea.Range.Column;
        }
        else
        {
          firstColumn = 1;
          firstRow = 1;
        }
      }
      else if (pivotArea.AreaType == PivotAreaType.FieldButton)
      {
        if (pivotArea.Axis == PivotAxisTypes.Column)
        {
          firstRow = 1;
          firstColumn = this.m_options.RowLayout != PivotTableRowLayout.Compact ? count2 + 1 + pivotArea.FieldPosition : 2 + pivotArea.FieldPosition;
        }
        else if (pivotArea.Axis == PivotAxisTypes.Row)
        {
          firstColumn = 1 + pivotArea.FieldPosition;
          firstRow = 1 + count1;
        }
      }
      else if (pivotArea.AreaType == PivotAreaType.TopRight)
      {
        firstRow = 1;
        int num = count1 + 1;
        if (this.m_options.RowLayout != PivotTableRowLayout.Compact)
          num += count2 - 1;
        firstColumn = pivotArea.Range == null ? num + 1 : num + pivotArea.Range.Column;
      }
      else if (pivotArea.References.Count > 0)
      {
        List<PivotAreaReference> references1 = new List<PivotAreaReference>();
        List<PivotAreaReference> references2 = new List<PivotAreaReference>();
        foreach (int num in this.RowFieldsOrder)
        {
          for (int index = 0; index < pivotArea.References.AreaReferences.Count; ++index)
          {
            if (num == pivotArea.References.AreaReferences[index].FieldIndex)
              references1.Add(pivotArea.References.AreaReferences[index]);
          }
        }
        foreach (int num in this.ColFieldsOrder)
        {
          for (int index = 0; index < pivotArea.References.AreaReferences.Count; ++index)
          {
            if (num == pivotArea.References.AreaReferences[index].FieldIndex || pivotArea.References.AreaReferences[index].FieldIndex == int.MaxValue)
              references2.Add(pivotArea.References.AreaReferences[index]);
          }
        }
        bool isSubTotal1 = false;
        int num1;
        if (references1.Count > 0)
        {
          num1 = this.GetLineNumber(references1, this.RowFieldsInnerItems, pivotArea.Range, false, this.m_options.RowLayout, out isSubTotal1);
          if (num1 == -1)
            continue;
        }
        else
          num1 = -1;
        bool isSubTotal2 = false;
        firstColumn = count1 > 0 || references2.Count > 0 ? this.GetLineNumber(references2, this.ColumnFieldsInnerItems, pivotArea.Range, true, this.m_options.RowLayout, out isSubTotal2) : -1;
        if (num1 != -1 || firstColumn != -1)
        {
          if (num1 == -1)
          {
            firstRow = references2.Count + 1;
            if (firstColumn != -1 && isSubTotal2 && pivotArea.Range != null)
              firstRow += pivotArea.Range.Row - 1;
          }
          else
            firstRow = num1 + (count1 + 1);
          if (firstColumn != -1)
          {
            if (this.m_options.RowLayout == PivotTableRowLayout.Compact)
              ++firstColumn;
            else
              firstColumn += count2;
          }
          else if (firstColumn == -1)
          {
            firstColumn = this.m_options.RowLayout != PivotTableRowLayout.Compact ? references1.Count : 1;
            if (firstRow != -1 && isSubTotal1 && pivotArea.Range != null)
              firstColumn += pivotArea.Range.Column - 1;
          }
        }
        else
          continue;
      }
      if (pivotArea.Axis == PivotAxisTypes.Data)
      {
        if (pivotArea.References.Count == 0)
          firstRow = 1;
        firstColumn = this.m_options.RowLayout != PivotTableRowLayout.Compact ? count2 + 1 : 2;
      }
      if (pivotArea.HasColumnGrand && pivotArea.CollapsedLevelsAreSubtotals)
      {
        int num = this.GetTotalInnerCount(this.ColumnFieldsInnerItems) + 1;
        firstColumn = this.m_options.RowLayout != PivotTableRowLayout.Compact ? num + count2 : num + 1;
        if (pivotArea.IsLableOnly)
          firstRow = 2;
        if (pivotArea.Range != null)
          firstRow = pivotArea.Range.Row + 1;
      }
      if (pivotArea.HasRowGrand && pivotArea.CollapsedLevelsAreSubtotals)
      {
        firstRow = this.GetTotalInnerCount(this.RowFieldsInnerItems) + count1 + 2;
        if (pivotArea.IsLableOnly)
          firstColumn = 1;
        if (pivotArea.Range != null)
          firstColumn = pivotArea.Range.Column;
      }
      if (firstRow > 0 && firstColumn > 0)
      {
        if (firstRow - 1 <= layout.maxRowCount && firstColumn - 1 < layout[firstRow - 1].Count)
        {
          long cellIndex = RangeImpl.GetCellIndex(firstColumn, firstRow);
          if (!pivotFormats.ContainsKey(cellIndex))
            pivotFormats.Add(cellIndex, this.GetExtendedFormat(layout[firstRow - 1, firstColumn - 1].XF, firstRow - 1, firstColumn - 1, format.PivotCellFormat as IInternalPivotCellFormat));
          else
            pivotFormats[cellIndex] = this.GetExtendedFormat(pivotFormats[cellIndex], RangeImpl.GetRowFromCellIndex(cellIndex), RangeImpl.GetColumnFromCellIndex(cellIndex), format.PivotCellFormat as IInternalPivotCellFormat);
        }
      }
      else if (pivotArea.AreaType == PivotAreaType.All)
        this.UpdatePivotCells(layout, pivotFormats, format, 0, 0, layout.maxRowCount);
      else if (pivotArea.AreaType == PivotAreaType.Data || pivotArea.References.Count == 0 && !pivotArea.IsLableOnly && !pivotArea.IsDataOnly && !pivotArea.IsOutline)
      {
        int colIndex = 1;
        if (this.m_options.RowLayout != PivotTableRowLayout.Compact)
          colIndex = count2;
        this.UpdatePivotCells(layout, pivotFormats, format, count1 + 1, colIndex, layout.maxRowCount);
      }
    }
    return pivotFormats;
  }

  private void UpdatePivotCells(
    PivotTableLayout layout,
    Dictionary<long, ExtendedFormatImpl> pivotFormats,
    PivotFormat format,
    int rowIndex,
    int colIndex,
    int rowCount)
  {
    for (int index1 = rowIndex; index1 <= rowCount; ++index1)
    {
      int num = colIndex;
      for (int index2 = layout[rowIndex].Count - 1; num <= index2; ++num)
      {
        if (index1 <= layout.maxRowCount && num < layout[index1].Count)
        {
          long cellIndex = RangeImpl.GetCellIndex(num + 1, index1 + 1);
          if (!pivotFormats.ContainsKey(cellIndex))
            pivotFormats.Add(cellIndex, this.GetExtendedFormat(layout[index1, num].XF, index1, num, format.PivotCellFormat as IInternalPivotCellFormat));
          else
            pivotFormats[cellIndex] = this.GetExtendedFormat(pivotFormats[cellIndex], RangeImpl.GetRowFromCellIndex(cellIndex), RangeImpl.GetColumnFromCellIndex(cellIndex), format.PivotCellFormat as IInternalPivotCellFormat);
        }
      }
    }
  }

  internal ExtendedFormatImpl GetExtendedFormat(
    ExtendedFormatImpl EX,
    int row,
    int column,
    IInternalPivotCellFormat pivotCellFormat)
  {
    ExtendedFormatImpl extendedFormat;
    FontImpl font;
    if (EX == null)
    {
      extendedFormat = new ExtendedFormatImpl(this.m_worksheet.Application, (object) this.m_worksheet.Workbook);
      font = (FontImpl) this.m_book.CreateFont((IFont) null, false);
    }
    else
    {
      extendedFormat = EX.Clone() as ExtendedFormatImpl;
      font = (FontImpl) this.m_book.CreateFont(EX.Font, false);
    }
    if (pivotCellFormat.IsBackgroundColorPresent || pivotCellFormat.IsFontFormatPresent || pivotCellFormat.IsBorderFormatPresent || pivotCellFormat.IsPatternFormatPresent || pivotCellFormat.IncludeAlignment || pivotCellFormat.IsNumberFormatPresent || pivotCellFormat.IncludeProtection)
    {
      if (pivotCellFormat.IncludeAlignment)
      {
        this.Worksheet[row + this.Location.Row, column + this.Location.Column].CellStyle.HorizontalAlignment = pivotCellFormat.HorizontalAlignment;
        extendedFormat.HorizontalAlignment = pivotCellFormat.HorizontalAlignment;
        this.Worksheet[row + this.Location.Row, column + this.Location.Column].CellStyle.VerticalAlignment = pivotCellFormat.VerticalAlignment;
        extendedFormat.VerticalAlignment = pivotCellFormat.VerticalAlignment;
        extendedFormat.Rotation = pivotCellFormat.Rotation;
        extendedFormat.WrapText = pivotCellFormat.WrapText;
        extendedFormat.ShrinkToFit = pivotCellFormat.ShrinkToFit;
        extendedFormat.ReadingOrder = pivotCellFormat.ReadingOrder;
        this.Worksheet[row + this.Location.Row, column + this.Location.Column].CellStyle.IndentLevel = pivotCellFormat.IndentLevel;
        extendedFormat.IndentLevel = pivotCellFormat.IndentLevel;
      }
      if (pivotCellFormat.IsFontFormatPresent)
      {
        if (!font.Bold)
          font.Bold = pivotCellFormat.Bold;
        if (!font.Italic)
          font.Italic = pivotCellFormat.Italic;
        font.Underline = pivotCellFormat.Underline;
        if (!font.Strikethrough)
          font.Strikethrough = pivotCellFormat.StrikeThrough;
        font.Size = pivotCellFormat.FontSize;
        font.FontName = pivotCellFormat.FontName;
        if (pivotCellFormat.IsFontColorPresent)
          font.RGBColor = pivotCellFormat.FontColorRGB;
      }
      if (pivotCellFormat.IsBorderFormatPresent)
      {
        if (pivotCellFormat.IsDiagonalBorderModified && pivotCellFormat.DiagonalBorderStyle != ExcelLineStyle.None)
        {
          extendedFormat.Borders[ExcelBordersIndex.DiagonalUp].ColorRGB = pivotCellFormat.DiagonalBorderColorRGB;
          extendedFormat.Borders[ExcelBordersIndex.DiagonalUp].LineStyle = pivotCellFormat.DiagonalBorderStyle;
          extendedFormat.Borders[ExcelBordersIndex.DiagonalDown].ColorRGB = pivotCellFormat.DiagonalBorderColorRGB;
          extendedFormat.Borders[ExcelBordersIndex.DiagonalDown].LineStyle = pivotCellFormat.DiagonalBorderStyle;
        }
        if (pivotCellFormat.IsLeftBorderModified && pivotCellFormat.LeftBorderStyle != ExcelLineStyle.None)
        {
          extendedFormat.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = pivotCellFormat.LeftBorderColorRGB;
          extendedFormat.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = pivotCellFormat.LeftBorderStyle;
        }
        if (pivotCellFormat.IsRightBorderModified && pivotCellFormat.RightBorderStyle != ExcelLineStyle.None)
        {
          extendedFormat.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = pivotCellFormat.RightBorderColorRGB;
          extendedFormat.Borders[ExcelBordersIndex.EdgeRight].LineStyle = pivotCellFormat.RightBorderStyle;
        }
        if (pivotCellFormat.IsTopBorderModified && pivotCellFormat.TopBorderStyle != ExcelLineStyle.None)
        {
          extendedFormat.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = pivotCellFormat.TopBorderColorRGB;
          extendedFormat.Borders[ExcelBordersIndex.EdgeTop].LineStyle = pivotCellFormat.TopBorderStyle;
        }
        if (pivotCellFormat.IsBottomBorderModified && pivotCellFormat.BottomBorderStyle != ExcelLineStyle.None)
        {
          extendedFormat.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = pivotCellFormat.BottomBorderColorRGB;
          extendedFormat.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = pivotCellFormat.BottomBorderStyle;
        }
      }
      if (pivotCellFormat.IsPatternFormatPresent && pivotCellFormat.PatternStyle != ExcelPattern.None && pivotCellFormat.PatternStyle != ExcelPattern.Solid)
      {
        extendedFormat.FillPattern = pivotCellFormat.PatternStyle;
        extendedFormat.PatternColor = pivotCellFormat.PatternColorRGB;
      }
      if (pivotCellFormat.IsBackgroundColorPresent)
        extendedFormat.Color = pivotCellFormat.BackColorRGB;
      if (pivotCellFormat.IsNumberFormatPresent)
        extendedFormat.NumberFormatIndex = (int) pivotCellFormat.NumberFormatIndex;
      if (pivotCellFormat.IncludeProtection)
      {
        extendedFormat.FormulaHidden = pivotCellFormat.FormulaHidden;
        extendedFormat.Locked = pivotCellFormat.Locked;
      }
    }
    FontImpl fontImpl = (FontImpl) this.m_book.InnerFonts.Add((IFont) font);
    extendedFormat.FontIndex = fontImpl.Font.Index;
    extendedFormat.IsPivotFormat = true;
    return extendedFormat;
  }

  private int GetLineNumber(
    List<PivotAreaReference> references,
    List<PivotInnerItem> innerItems,
    IRange offsetRange,
    bool isColumn,
    PivotTableRowLayout rowLayout,
    out bool isSubTotal)
  {
    isSubTotal = false;
    int num = 0;
    bool flag = false;
    for (int index1 = 0; index1 < references.Count; ++index1)
    {
      if (innerItems != null)
      {
        flag = false;
        PivotAreaReference reference = references[index1];
        if (reference.Indexes.Count > 0)
        {
          string[] array;
          if (reference.FieldIndex == int.MaxValue)
          {
            array = new string[this.DataFields.Count];
            for (int i = 0; i < this.DataFields.Count; ++i)
              array[i] = this.DataFields[i].Name;
          }
          else
          {
            array = new string[this.Fields[reference.FieldIndex].SortedFieldItems.Keys.Count];
            this.Fields[reference.FieldIndex].SortedFieldItems.Keys.CopyTo(array, 0);
          }
          if (reference.FirstIndex < array.Length)
          {
            string str = array[reference.FirstIndex];
            ++num;
            for (int index2 = 0; index2 < innerItems.Count; ++index2)
            {
              PivotInnerItem innerItem = innerItems[index2];
              if (str == innerItem.Name)
              {
                flag = true;
                innerItems = innerItem.Items;
                if ((isColumn || rowLayout != PivotTableRowLayout.Compact) && innerItems != null)
                {
                  if (reference.IsDefaultSubTotal)
                  {
                    isSubTotal = true;
                    num += this.GetTotalInnerCount(innerItems);
                    break;
                  }
                  if (index1 < references.Count - 1 && (isColumn || rowLayout != PivotTableRowLayout.Outline))
                  {
                    --num;
                    break;
                  }
                  if (index1 == references.Count - 1 && offsetRange != null)
                  {
                    if (isColumn)
                    {
                      if (offsetRange.Column == 256 /*0x0100*/ && offsetRange.Column == 256 /*0x0100*/)
                      {
                        num += this.GetTotalInnerCount(innerItems) - 1;
                        break;
                      }
                      num += offsetRange.Column - 1;
                      break;
                    }
                    if (offsetRange.Row == 256 /*0x0100*/ && offsetRange.Column == 256 /*0x0100*/)
                    {
                      num += this.GetTotalInnerCount(innerItems) - 1;
                      break;
                    }
                    if (offsetRange.Row == 256 /*0x0100*/)
                    {
                      isSubTotal = true;
                      break;
                    }
                    num += offsetRange.Row - 1;
                    break;
                  }
                  break;
                }
                break;
              }
              if (innerItem.Items != null)
                num += this.GetTotalInnerCount(innerItem.Items) + 1;
              else
                ++num;
            }
          }
        }
      }
    }
    return flag ? num : -1;
  }

  private int GetTotalInnerCount(List<PivotInnerItem> innerItems)
  {
    int totalInnerCount = 0;
    for (int index = 0; index < innerItems.Count; ++index)
    {
      ++totalInnerCount;
      if (innerItems[index].Items != null)
        totalInnerCount += this.GetTotalInnerCount(innerItems[index].Items);
    }
    return totalInnerCount;
  }

  public object Clone(object parent)
  {
    return this.Clone(parent, this.CacheIndex, (Dictionary<string, string>) null);
  }

  public object Clone(object parent, int cacheIndex, Dictionary<string, string> hashWorksheetNames)
  {
    PivotTableImpl pivotTableImpl = (PivotTableImpl) this.MemberwiseClone();
    pivotTableImpl.SetParent(parent);
    pivotTableImpl.SetWorkbook();
    pivotTableImpl.m_viewDefinition = (PivotViewDefinitionRecord) CloneUtils.CloneCloneable((ICloneable) this.m_viewDefinition);
    pivotTableImpl.m_viewExInfo = (ViewExtendedInfoRecord) CloneUtils.CloneCloneable((ICloneable) this.m_viewExInfo);
    pivotTableImpl.m_pageItem = (PageItemRecord) CloneUtils.CloneCloneable((ICloneable) this.m_pageItem);
    pivotTableImpl.m_arrFields = CloneUtils.CloneCloneable<PivotFieldImpl>((IList<PivotFieldImpl>) this.m_arrFields, (object) pivotTableImpl);
    pivotTableImpl.m_arrLineItems = CloneUtils.CloneCloneable<LineItemArrayRecord>(this.m_arrLineItems);
    pivotTableImpl.m_arrDataItems = CloneUtils.CloneCloneable<DataItemRecord>(this.m_arrDataItems);
    pivotTableImpl.m_arrUnknown = CloneUtils.CloneCloneable(this.m_arrUnknown);
    pivotTableImpl.m_arrRowColumnFiledId = new RowColumnFiledIdRecord[2];
    pivotTableImpl.m_arrRowColumnFiledId[0] = (RowColumnFiledIdRecord) CloneUtils.CloneCloneable((ICloneable) this.m_arrRowColumnFiledId[0]);
    pivotTableImpl.m_arrRowColumnFiledId[1] = (RowColumnFiledIdRecord) CloneUtils.CloneCloneable((ICloneable) this.m_arrRowColumnFiledId[1]);
    pivotTableImpl.CacheIndex = cacheIndex;
    pivotTableImpl.m_arrPivotFields = (PivotTableFields) CloneUtils.CloneCloneable((ICloneParent) this.m_arrPivotFields, (object) pivotTableImpl);
    pivotTableImpl.m_dataFields = (PivotDataFields) CloneUtils.CloneCloneable((ICloneParent) this.m_dataFields, (object) pivotTableImpl);
    PivotTableImpl parent1 = this.ClonePivotFields(pivotTableImpl);
    if (parent1.Workbook.DataHolder == null && parent1.PreservedElements.ContainsKey("formats"))
      parent1.PreservedElements.Remove("formats");
    object parent2 = parent1.FindParent(typeof (WorksheetImpl));
    if (this.m_location != null)
      parent1.m_location = ((ICombinedRange) this.m_location).Clone(parent2, hashWorksheetNames, parent1.m_book);
    parent1.m_pivotFormats = (PivotFormats) this.m_pivotFormats.Clone(parent1);
    return (object) parent1;
  }

  internal PivotTableImpl ClonePivotFields(PivotTableImpl result)
  {
    result.m_lstPageFields = new List<PivotFieldImpl>();
    for (int index = 0; index < this.m_lstPageFields.Count; ++index)
    {
      int i = this.m_arrPivotFields.IndexOf(this.m_lstPageFields[index]);
      result.m_lstPageFields.Add(result.m_arrPivotFields[i]);
    }
    result.m_lstRowFields = new List<PivotFieldImpl>();
    for (int index = 0; index < this.m_lstRowFields.Count; ++index)
    {
      int i = this.m_arrPivotFields.IndexOf(this.m_lstRowFields[index]);
      result.m_lstRowFields.Add(result.m_arrPivotFields[i]);
    }
    result.m_lstColumnFields = new List<PivotFieldImpl>();
    for (int index = 0; index < this.m_lstColumnFields.Count; ++index)
    {
      int i = this.m_arrPivotFields.IndexOf(this.m_lstColumnFields[index]);
      result.m_lstColumnFields.Add(result.m_arrPivotFields[i]);
    }
    if (this.m_pageFields != null)
    {
      result.m_pageFields = new List<IPivotField>();
      for (int index = 0; index < this.m_pageFields.Count; ++index)
      {
        int i = this.m_arrPivotFields.IndexOf((PivotFieldImpl) this.m_pageFields[index]);
        result.m_pageFields.Add((IPivotField) result.m_arrPivotFields[i]);
      }
    }
    return result;
  }

  internal PivotTableImpl Clone(
    PivotTableCollection tables,
    Dictionary<string, string> hashWorksheetNames)
  {
    WorkbookImpl parentWorkbook = tables.ParentWorksheet.ParentWorkbook;
    int cacheIndex = this.CacheIndex;
    if (parentWorkbook != this.Workbook)
    {
      if (parentWorkbook.Version != this.Workbook.Version)
        throw new InvalidOperationException("Cannot copy pivot tables between workbooks with different versions");
      PivotCacheImpl pivotCach = this.Workbook.PivotCaches[this.CacheIndex];
      PivotCacheCollection pivotCaches = ((IWorkbook) parentWorkbook).PivotCaches as PivotCacheCollection;
      PivotCacheImpl cache = (PivotCacheImpl) pivotCach.Clone((object) pivotCaches, hashWorksheetNames);
      pivotCaches.Add(cache);
      cacheIndex = cache.Index;
    }
    return (PivotTableImpl) this.Clone((object) tables, cacheIndex, hashWorksheetNames);
  }

  public void AutoFitPivotTable(PivotTableImpl m_pivotTable)
  {
    List<IPivotField> pivotRowFields = m_pivotTable.PivotRowFields;
    int column = m_pivotTable.Location.Column;
    foreach (IPivotField pivotRowField in m_pivotTable.PivotRowFields)
    {
      int row = m_pivotTable.Location.Row;
      string str1 = "drp";
      PivotFieldImpl pivotFieldImpl = pivotRowField as PivotFieldImpl;
      string str2 = pivotFieldImpl.CacheField.Name + str1;
      int length = str2.Length;
      PivotCacheFieldImpl cacheField = pivotFieldImpl.CacheField;
      for (int index = 0; index < cacheField.Items.Count; ++index)
      {
        if (cacheField.Items[index] != null)
        {
          string str3 = cacheField.Items[index].ToString();
          if (pivotFieldImpl.Subtotals != PivotSubtotalTypes.None)
            str3 += "Totals";
          if (str3.Length > length)
          {
            length = str3.Length;
            str2 = str3;
          }
        }
      }
      string text = m_pivotTable.Worksheet.Range[row, column].Text;
      m_pivotTable.Worksheet.Range[row, column].Text = str2;
      if (str2 != null || str2 != string.Empty)
        m_pivotTable.Worksheet.AutofitColumn(column);
      m_pivotTable.Worksheet.Range[row, column].Text = text;
      ++column;
    }
  }

  internal void MoveLocation(int delta)
  {
    int row = this.m_location.Row;
    int lastRow = this.m_location.LastRow;
    int column = this.m_location.Column;
    int lastColumn = this.m_location.LastColumn;
    int pageFieldsCount = this.GetPageFieldsCount();
    int num = pageFieldsCount + delta;
    if (pageFieldsCount != 0)
      ++pageFieldsCount;
    if (num != 0)
      ++num;
    this.m_location = this.m_location.Worksheet[row - pageFieldsCount + num, column, lastRow - pageFieldsCount + num, lastColumn];
  }

  private int GetPageFieldsCount()
  {
    int pageFieldsCount = 0;
    int i = 0;
    for (int count = this.Fields.Count; i < count; ++i)
    {
      if (this.Fields[i].Axis == PivotAxisTypes.Page)
        ++pageFieldsCount;
    }
    return pageFieldsCount;
  }

  internal void SetChanged(bool isClearPivot)
  {
    this.m_bIsChanged = true;
    this.Cache.IsRefreshOnLoad = this.PivotEngineValues == null;
    if (!isClearPivot)
      return;
    this.ClearPivotRange();
    this.ColumnItemsStream = (Stream) null;
    this.RowItemsStream = (Stream) null;
    this.PreservedElements.Clear();
  }

  internal void ClearPivotRange()
  {
    if (this.m_location == null)
      return;
    IRange location = this.m_location;
    this.m_location = this.m_worksheet[location.Row, location.Column];
    int row = location.Row - (1 + this.PageFields.Count);
    if (row == 0)
      row = 1;
    this.m_worksheet[row, location.Column, location.LastRow, location.LastColumn + 1].Clear(true);
  }

  internal PivotCalculatedFields GetCalculatedFields()
  {
    PivotCalculatedFields calculatedFields = new PivotCalculatedFields(this);
    for (int i = 0; i < this.Fields.Count; ++i)
    {
      if (this.Fields[i].IsFormulaField)
        calculatedFields.Add(this.Fields[i]);
    }
    return calculatedFields;
  }

  public void ClearTable()
  {
    this.ClearPivotRange();
    foreach (PivotFieldImpl field in (CollectionBase<PivotFieldImpl>) this.Fields)
    {
      field.Axis = PivotAxisTypes.None;
      field.IsDataField = false;
    }
  }
}
