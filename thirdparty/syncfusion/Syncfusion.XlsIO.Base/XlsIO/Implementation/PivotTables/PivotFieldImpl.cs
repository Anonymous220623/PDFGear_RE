// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotFieldImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
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

public class PivotFieldImpl : ICloneParent, IPivotField
{
  private PivotViewFieldsRecord m_viewFields = (PivotViewFieldsRecord) BiffRecordFactory.GetRecord(TBIFFRecord.PivotViewFields);
  private PivotViewFieldsExRecord m_viewFieldsEx = (PivotViewFieldsExRecord) BiffRecordFactory.GetRecord(TBIFFRecord.PivotViewFieldsEx);
  private List<PivotViewItemRecord> m_arrItems = new List<PivotViewItemRecord>();
  private PivotCacheFieldImpl m_cacheField;
  private bool m_bDataField;
  public PivotTableImpl m_table;
  private bool m_bCompact = true;
  private bool m_bIsDragOff;
  private bool m_bIsDragToData;
  private bool m_bShowNewItemsInFilter;
  private bool m_bShowNewItemsOnRefresh;
  private bool m_bShowBlankRow;
  private bool m_bShowPageBreak;
  private int m_iItemsPerPage = 10;
  private int m_iItemIndex = -1;
  private bool m_bMeasureField;
  private bool m_bIsMultiSelected;
  private bool m_bShowOutline = true;
  private bool m_bShowDropDown;
  private bool m_bShowPropAsCaption;
  private bool m_bShowItemPropAsCaption;
  private bool m_bShowToolTip;
  private PivotFieldSortType? m_sortType;
  private bool m_bIsAutoFiltersByRank;
  private string m_uniqueName;
  private Dictionary<int, PivotItemOptions> m_fieldItemOptions;
  private Stream m_preservedAutoSort;
  private bool m_bIsAllDrilled;
  private bool m_bIsDataSourceSorted;
  private bool m_bIsDefaultDrill;
  private Stream m_futureDataStorage;
  private string m_FilterValue;
  private PivotFilterCollections m_pivotFilters;
  private PivotFieldItemsCollections m_PivotFieldItems;
  public int m_iItemInvisibleCount;
  private string m_pageFieldName;
  private string m_pageFieldCaption;
  private int m_pageFieldHierarchyIndex;
  internal bool m_bItemOptionSorted;
  private bool m_bCacheFieldUpdated;
  private bool m_bIgnore;
  private bool m_bRepeatLabels;
  private PivotInnerItem m_autoSortItem;
  private PivotArea m_pivotArea;
  private Dictionary<string, int> m_sortedFieldItems = new Dictionary<string, int>();

  internal Stream FutureDataStorageStream
  {
    get => this.m_futureDataStorage;
    set => this.m_futureDataStorage = value;
  }

  public int Position
  {
    get
    {
      int position = 0;
      if (!this.m_table.Workbook.Loading)
      {
        switch (this.Axis)
        {
          case PivotAxisTypes.None:
          case PivotAxisTypes.Data:
            if (!this.IsDataField)
              throw new Exception("Specified field does not belongs to any fields type");
            PivotDataFields dataFields = this.m_table.DataFields;
            for (int i = 0; i < dataFields.Count; ++i)
            {
              if (dataFields[i].Field.CacheField.Index == this.CacheField.Index)
                return position;
              ++position;
            }
            break;
          case PivotAxisTypes.Row:
            int index1 = this.CacheField.Index;
            using (List<int>.Enumerator enumerator = this.m_table.RowFieldsOrder.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                if (enumerator.Current == index1)
                  return position;
                ++position;
              }
              break;
            }
          case PivotAxisTypes.Column:
            int index2 = this.CacheField.Index;
            using (List<int>.Enumerator enumerator = this.m_table.ColFieldsOrder.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                if (enumerator.Current == index2)
                  return position;
                ++position;
              }
              break;
            }
          case PivotAxisTypes.Page:
            List<IPivotField> pivotPageFields = this.m_table.PivotPageFields;
            for (int index3 = 0; index3 < pivotPageFields.Count; ++index3)
            {
              if ((pivotPageFields[index3] as PivotFieldImpl).CacheField.Index == this.CacheField.Index)
                return position;
              ++position;
            }
            break;
          default:
            throw new Exception("Specified field does not belongs to any fields type");
        }
      }
      return position + 1;
    }
    set
    {
      if (this.m_table.Workbook.Loading)
        return;
      this.m_table.SetChanged(true);
      switch (this.Axis)
      {
        case PivotAxisTypes.None:
        case PivotAxisTypes.Data:
          if (!this.IsDataField)
            throw new Exception("Specified field does not belongs to any fields type");
          this.MovePivotDataFields(value);
          break;
        case PivotAxisTypes.Row:
          this.MovePivotRowsFields(value);
          break;
        case PivotAxisTypes.Column:
          this.MovePivotColumnFields(value);
          break;
        case PivotAxisTypes.Page:
          this.MovePivotPageFields(value);
          break;
        default:
          throw new Exception("Specified field does not belongs to any fields type");
      }
    }
  }

  public PivotAxisTypes Axis
  {
    get => this.m_viewFields.Axis;
    set
    {
      if (this.m_viewFields.Axis == value)
        return;
      if (!this.m_table.Workbook.Loading)
      {
        this.m_table.SetChanged(true);
        if (this.m_viewFields.Axis == PivotAxisTypes.Page)
          this.m_table.MoveLocation(-1);
        if (value == PivotAxisTypes.Page)
          this.m_table.MoveLocation(1);
        if (this.m_viewFields.Axis == PivotAxisTypes.Column)
          this.m_table.ColumnItemsStream = (Stream) null;
        else if (this.m_viewFields.Axis == PivotAxisTypes.Row)
          this.m_table.RowItemsStream = (Stream) null;
        switch (value)
        {
          case PivotAxisTypes.None:
          case PivotAxisTypes.Page:
            if (this.m_table.RowFieldsOrder.Contains(this.m_table.Fields.IndexOf(this)))
            {
              this.m_table.RowFieldsOrder.Remove(this.m_table.Fields.IndexOf(this));
              this.m_table.PivotRowFields.Remove((IPivotField) this);
            }
            if (this.m_table.ColFieldsOrder.Contains(this.m_table.Fields.IndexOf(this)))
            {
              this.m_table.ColFieldsOrder.Remove(this.m_table.Fields.IndexOf(this));
              break;
            }
            break;
          case PivotAxisTypes.Row:
            this.m_table.PivotRowFields.Add((IPivotField) this);
            this.m_table.RowFieldsOrder.Add(this.m_table.Fields.IndexOf(this));
            if (this.m_table.ColFieldsOrder.Contains(this.m_table.Fields.IndexOf(this)))
            {
              this.m_table.ColFieldsOrder.Remove(this.m_table.Fields.IndexOf(this));
              break;
            }
            break;
          case PivotAxisTypes.Column:
            this.m_table.ColFieldsOrder.Add(this.m_table.Fields.IndexOf(this));
            if (this.m_table.RowFieldsOrder.Contains(this.m_table.Fields.IndexOf(this)))
            {
              this.m_table.RowFieldsOrder.Remove(this.m_table.Fields.IndexOf(this));
              this.m_table.PivotRowFields.Remove((IPivotField) this);
              break;
            }
            break;
        }
      }
      this.m_table.RemovePivotField(this.m_viewFields.Axis, this);
      this.m_viewFields.Axis = value;
      this.m_table.AddPivotField(value, this, false);
    }
  }

  public string FilterValue
  {
    get => this.m_FilterValue;
    set => this.m_FilterValue = value;
  }

  internal bool ItemOptionSorted
  {
    get => this.m_bItemOptionSorted;
    set => this.m_bItemOptionSorted = value;
  }

  internal bool CacheFieldUpdated
  {
    get => this.m_bCacheFieldUpdated;
    set => this.m_bCacheFieldUpdated = value;
  }

  public string Name
  {
    get => this.m_viewFields.Name;
    set
    {
      this.m_viewFields.Name = value;
      if (!this.IsDataField)
        return;
      foreach (PivotDataField dataField in (CollectionBase<PivotDataField>) this.m_table.DataFields)
      {
        if (dataField.Field.CacheField.Index == this.CacheField.Index)
          dataField.Name = value;
      }
    }
  }

  public PivotCacheFieldImpl CacheField => this.m_cacheField;

  public bool IsDataField
  {
    get => this.m_bDataField;
    set => this.m_bDataField = value;
  }

  public int NumberFormatIndex
  {
    get => (int) this.m_viewFieldsEx.NumberFormat;
    set
    {
      this.m_viewFieldsEx.NumberFormat = value >= 0 ? (ushort) value : throw new ArgumentOutOfRangeException(nameof (value));
    }
  }

  public string NumberFormat
  {
    get => this.m_table.Workbook.InnerFormats[this.NumberFormatIndex]?.FormatString;
    set => this.NumberFormatIndex = this.m_table.Workbook.InnerFormats.FindOrCreateFormat(value);
  }

  public PivotSubtotalTypes Subtotals
  {
    get => this.m_viewFields.SubtotalType;
    set => this.m_viewFields.SubtotalType = value;
  }

  internal string SubTotalName
  {
    get => this.m_viewFieldsEx.SubTotalName;
    set => this.m_viewFieldsEx.SubTotalName = value;
  }

  public bool IsAutoShow
  {
    get => this.m_viewFieldsEx.IsAutoShow;
    set => this.m_viewFieldsEx.IsAutoShow = value;
  }

  public bool CanDragToRow
  {
    get => this.m_viewFieldsEx.IsDragToRow;
    set => this.m_viewFieldsEx.IsDragToRow = value;
  }

  public bool CanDragToColumn
  {
    get => this.m_viewFieldsEx.IsDragToColumn;
    set => this.m_viewFieldsEx.IsDragToColumn = value;
  }

  public bool CanDragToPage
  {
    get => this.m_viewFieldsEx.IsDragToPage;
    set => this.m_viewFieldsEx.IsDragToPage = value;
  }

  public bool IsDragToHide
  {
    get => this.m_viewFieldsEx.IsDragToHide;
    set => this.m_viewFieldsEx.IsDragToHide = value;
  }

  public bool CanDragOff
  {
    get => this.m_bIsDragOff;
    set => this.m_bIsDragOff = value;
  }

  public bool IncludeNewItemsInFilter
  {
    get => this.m_bShowNewItemsInFilter;
    set => this.m_bShowNewItemsInFilter = value;
  }

  public bool ShowNewItemsInFilter
  {
    get => this.m_bShowNewItemsInFilter;
    set => this.m_bShowNewItemsInFilter = value;
  }

  public bool ShowNewItemsOnRefresh
  {
    get => this.m_bShowNewItemsOnRefresh;
    set => this.m_bShowNewItemsOnRefresh = value;
  }

  public bool ShowBlankRow
  {
    get => this.m_bShowBlankRow;
    set => this.m_bShowBlankRow = value;
  }

  public bool ShowPageBreak
  {
    get => this.m_bShowPageBreak;
    set => this.m_bShowPageBreak = value;
  }

  public int ItemsPerPage
  {
    get => this.m_iItemsPerPage;
    set => this.m_iItemsPerPage = value;
  }

  internal int ItemIndex
  {
    get => this.m_iItemIndex;
    set => this.m_iItemIndex = value;
  }

  public bool IsMeasureField
  {
    get => this.m_bMeasureField;
    set => this.m_bMeasureField = value;
  }

  public bool IsMultiSelected
  {
    get => this.m_bIsMultiSelected;
    set => this.m_bIsMultiSelected = value;
  }

  public bool IsShowAllItems
  {
    get => this.m_viewFieldsEx.IsShowAllItems;
    set => this.m_viewFieldsEx.IsShowAllItems = value;
  }

  public bool ShowOutline
  {
    get => this.m_bShowOutline;
    set => this.m_bShowOutline = value;
  }

  public bool ShowDropDown
  {
    get => this.m_bShowDropDown;
    set => this.m_bShowDropDown = value;
  }

  public bool ShowPropAsCaption
  {
    get => this.m_bShowPropAsCaption;
    set => this.m_bShowPropAsCaption = value;
  }

  public bool ShowItemPropAsCaption
  {
    get => this.m_bShowItemPropAsCaption;
    set => this.m_bShowItemPropAsCaption = value;
  }

  public bool ShowToolTip
  {
    get => this.m_bShowToolTip;
    set => this.m_bShowToolTip = value;
  }

  public PivotFieldSortType? SortType
  {
    get => this.m_sortType;
    set => this.m_sortType = value;
  }

  public bool IsAutoFiltersByRank
  {
    get => this.m_bIsAutoFiltersByRank;
    set => this.m_bIsAutoFiltersByRank = value;
  }

  public string Caption
  {
    get => this.m_uniqueName;
    set => this.m_uniqueName = value;
  }

  internal Dictionary<int, PivotItemOptions> ItemOptions
  {
    get
    {
      if (this.m_fieldItemOptions == null)
        this.m_fieldItemOptions = new Dictionary<int, PivotItemOptions>();
      return this.m_fieldItemOptions;
    }
  }

  public bool Compact
  {
    get => this.m_bCompact;
    set => this.m_bCompact = value;
  }

  public bool CanDragToData
  {
    get => this.m_bIsDragToData;
    set => this.m_bIsDragToData = value;
  }

  public string Formula
  {
    get => this.m_cacheField.Formula;
    set => this.m_cacheField.Formula = value;
  }

  public bool IsFormulaField => this.m_cacheField.IsFormulaField;

  public Stream PreservedAutoSort
  {
    get => this.m_preservedAutoSort;
    set => this.m_preservedAutoSort = value;
  }

  internal bool IsAllDrilled
  {
    get => this.m_bIsAllDrilled;
    set => this.m_bIsAllDrilled = value;
  }

  internal bool IsDataSourceSorted
  {
    get => this.m_bIsDataSourceSorted;
    set => this.m_bIsDataSourceSorted = value;
  }

  internal bool IsDefaultDrill
  {
    get => this.m_bIsDefaultDrill;
    set => this.m_bIsDefaultDrill = value;
  }

  public IPivotFilters PivotFilters
  {
    get
    {
      if (this.m_pivotFilters == null)
        this.m_pivotFilters = new PivotFilterCollections((IPivotField) this);
      return (IPivotFilters) this.m_pivotFilters;
    }
  }

  public IPivotFieldItems Items
  {
    get
    {
      if (this.m_PivotFieldItems == null)
        this.m_PivotFieldItems = new PivotFieldItemsCollections();
      return (IPivotFieldItems) this.m_PivotFieldItems;
    }
  }

  internal string PageFieldName
  {
    get => this.m_pageFieldName;
    set => this.m_pageFieldName = value;
  }

  internal string PageFieldCaption
  {
    get => this.m_pageFieldCaption;
    set => this.m_pageFieldCaption = value;
  }

  internal int PageFieldHierarchyIndex
  {
    get => this.m_pageFieldHierarchyIndex;
    set => this.m_pageFieldHierarchyIndex = value;
  }

  internal PivotTableImpl PivotTable => this.m_table;

  internal bool Ignore
  {
    get => this.m_bIgnore;
    set => this.m_bIgnore = value;
  }

  public bool RepeatLabels
  {
    get => this.m_bRepeatLabels;
    set => this.m_bRepeatLabels = value;
  }

  internal PivotInnerItem AutoSortItem
  {
    get => this.m_autoSortItem;
    set => this.m_autoSortItem = value;
  }

  internal PivotArea PivotArea
  {
    get => this.m_pivotArea;
    set => this.m_pivotArea = value;
  }

  internal Dictionary<string, int> SortedFieldItems
  {
    get => this.m_sortedFieldItems;
    set => this.m_sortedFieldItems = value;
  }

  internal PivotFieldImpl(PivotTableImpl table)
  {
    this.m_table = table != null ? table : throw new ArgumentNullException(nameof (table));
    this.Subtotals = PivotSubtotalTypes.Default;
    this.IsDragToHide = true;
    this.CanDragOff = true;
    this.CanDragToColumn = true;
    this.CanDragToData = true;
    this.CanDragToPage = true;
    this.CanDragToRow = true;
  }

  public PivotFieldImpl(PivotCacheFieldImpl cacheField, PivotTableImpl table)
    : this(table)
  {
    this.m_viewFields.Name = cacheField.Name;
    this.m_viewFields.NumberItems = (ushort) cacheField.ItemCount;
    this.m_cacheField = cacheField;
    PivotFieldItemsCollections itemsCollections = new PivotFieldItemsCollections();
    List<PivotTableSerializator.ComparisonPair> comparisonPairList = PivotTableSerializator.SortFieldValues(this.CacheField, (List<string>) null);
    if (comparisonPairList.Count <= 0)
      return;
    int index1 = 0;
    for (int index2 = 0; index2 < comparisonPairList.Count; ++index2)
    {
      string str = comparisonPairList[index1].Value == null ? (string) null : comparisonPairList[index1].Value.ToString();
      (this.Items as PivotFieldItemsCollections).Add((object) this, str, str);
      ++index1;
    }
  }

  public int Parse(IList data, int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (iPos < 0 || iPos > data.Count - 1)
      throw new ArgumentOutOfRangeException(nameof (iPos), "Value cannot be less than 0 and greater than data.Count - 1");
    BiffRecordRaw biffRecordRaw1 = (BiffRecordRaw) data[iPos];
    biffRecordRaw1.CheckTypeCode(TBIFFRecord.PivotViewFields);
    this.m_viewFields = (PivotViewFieldsRecord) biffRecordRaw1;
    ++iPos;
    int num = 0;
    for (int numberItems = (int) this.m_viewFields.NumberItems; num < numberItems; ++num)
    {
      BiffRecordRaw biffRecordRaw2 = (BiffRecordRaw) data[iPos];
      biffRecordRaw2.CheckTypeCode(TBIFFRecord.PivotViewItem);
      this.m_arrItems.Add((PivotViewItemRecord) biffRecordRaw2);
      ++iPos;
    }
    BiffRecordRaw biffRecordRaw3 = (BiffRecordRaw) data[iPos];
    if (biffRecordRaw3.TypeCode == TBIFFRecord.PivotViewFieldsEx)
    {
      this.m_viewFieldsEx = (PivotViewFieldsExRecord) biffRecordRaw3;
      ++iPos;
    }
    return iPos;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    records.Add((IBiffStorage) this.m_viewFields);
    records.AddList((IList) this.m_arrItems);
    records.Add((IBiffStorage) this.m_viewFieldsEx);
  }

  public void AddItemOption(int index, PivotItemOptions item) => this.ItemOptions.Add(index, item);

  public void AddItemOption(int index) => this.ItemOptions.Add(index, (PivotItemOptions) null);

  internal int GetPosition(PivotFieldItem item)
  {
    return this.m_PivotFieldItems == null ? 0 : this.m_PivotFieldItems.GetPosition(item);
  }

  internal void SetPosition(PivotFieldItem item, int index)
  {
    if (this.m_PivotFieldItems == null)
      return;
    this.m_PivotFieldItems.SetPosition(item, index);
  }

  private void MovePivotRowsFields(int newIndex)
  {
    if (newIndex >= this.m_table.PivotRowFields.Count)
      throw new ArgumentOutOfRangeException("Exceeds the max Row fields count");
    int count = this.m_table.PivotRowFields.Count;
    int index = this.CacheField.Index;
    this.m_table.RowFieldsOrder.Remove(index);
    List<IPivotField> pivotRowFields = this.m_table.PivotRowFields;
    pivotRowFields.Remove((IPivotField) this);
    if (pivotRowFields.Count > newIndex)
      pivotRowFields.Insert(newIndex, (IPivotField) this);
    else
      pivotRowFields.Add((IPivotField) this);
    if (count > newIndex)
      this.m_table.RowFieldsOrder.Insert(newIndex, index);
    else
      this.m_table.RowFieldsOrder.Add(index);
  }

  private void MovePivotColumnFields(int newIndex)
  {
    if (newIndex > this.m_table.ColumnFields.Count)
      throw new ArgumentOutOfRangeException("Exceeds the max Column fields count");
    int index = this.CacheField.Index;
    List<int> colFieldsOrder = this.m_table.ColFieldsOrder;
    colFieldsOrder.Remove(index);
    if (colFieldsOrder.Count > newIndex)
      colFieldsOrder.Insert(newIndex, index);
    else
      colFieldsOrder.Add(index);
  }

  private void MovePivotDataFields(int newIndex)
  {
    if (newIndex > this.m_table.DataFields.Count)
      throw new ArgumentOutOfRangeException("Exceeds the max Data fields count");
    int index = newIndex;
    int i1 = -1;
    for (int i2 = 0; i2 < this.m_table.Fields.Count; ++i2)
    {
      if (this.m_table.Fields[i2].IsDataField)
        ++i1;
      if (this.m_table.Fields[i2].CacheField.Index == this.CacheField.Index)
      {
        PivotDataFields dataFields = this.m_table.DataFields;
        PivotDataField pivotDataField = dataFields[i1];
        dataFields.Remove(pivotDataField);
        if (dataFields.Count > index)
          dataFields.Insert(index, pivotDataField);
        else
          dataFields.Add(pivotDataField);
      }
    }
  }

  private void MovePivotPageFields(int newIndex)
  {
    if (newIndex > this.m_table.PivotPageFields.Count)
      throw new ArgumentOutOfRangeException("Exceeds the max Page fields count");
    int index = this.CacheField.Index;
    this.m_table.PivotPageFields.Remove((IPivotField) this);
    if (this.m_table.PivotPageFields.Count > newIndex)
      this.m_table.PivotPageFields.Insert(newIndex, (IPivotField) this);
    else
      this.m_table.PivotPageFields.Add((IPivotField) this);
  }

  public void Sort(string[] orderByArray)
  {
    if ((this.CacheField.DataType & PivotDataType.String) == (PivotDataType) 0)
      return;
    List<string> orderBy = (List<string>) null;
    if (orderByArray != null)
      orderBy = new List<string>((IEnumerable<string>) orderByArray);
    List<PivotTableSerializator.ComparisonPair> comparisonPairList = PivotTableSerializator.SortFieldValues(this, orderBy, true);
    if (comparisonPairList.Count <= 0)
      return;
    this.m_PivotFieldItems.Clear();
    int index1 = 0;
    for (int index2 = 0; index2 < comparisonPairList.Count; ++index2)
    {
      string text = comparisonPairList[index1].Value == null || comparisonPairList[index1].Value == (object) string.Empty ? (string) null : comparisonPairList[index1].Value.ToString();
      (this.Items as PivotFieldItemsCollections).Add((object) this, comparisonPairList[index1].Index, text);
      ++index1;
    }
    this.SortType = new PivotFieldSortType?(PivotFieldSortType.Manual);
  }

  public void AutoSort(PivotFieldSortType sortType, int lineNumber)
  {
    this.SortType = new PivotFieldSortType?(sortType);
    if (this.Axis == PivotAxisTypes.Column && this.m_table.RowFields.Count > 0)
      this.AutoSortItem = this.m_table.SortByFields(this.m_table.RowFieldsInnerItems, lineNumber, false);
    else if (this.Axis == PivotAxisTypes.Row && (this.m_table.ColumnFields.Count > 0 || this.m_table.DataFields.Count > 0))
      this.AutoSortItem = this.m_table.SortByFields(this.m_table.ColumnFieldsInnerItems, lineNumber, true);
    if (this.AutoSortItem != null)
    {
      this.m_pivotArea = new PivotArea(this.CacheField);
      if (this.AutoSortItem.FieldIndex == int.MaxValue)
      {
        this.m_pivotArea.References.Add(new PivotAreaReference()
        {
          FieldIndex = int.MaxValue,
          Indexes = {
            this.AutoSortItem.ValueIndex
          }
        });
      }
      else
      {
        this.m_table.AddReferences(this.AutoSortItem, this.m_pivotArea);
        this.m_pivotArea.IsAutoSort = true;
      }
    }
    this.m_table.Cache.IsRefreshOnLoad = true;
  }

  internal void PreSort()
  {
    if (this.m_PivotFieldItems == null)
      return;
    this.m_PivotFieldItems.Clear();
    if ((this.CacheField.DataType == PivotDataType.Date || this.CacheField.DataType == (PivotDataType.Blank | PivotDataType.Date)) && this.CacheField.FieldGroup != null && this.CacheField.FieldGroup.PivotRangeGroupNames != null && this.CacheField.FieldGroup.PivotRangeGroupNames.Count > 0)
    {
      foreach (KeyValuePair<int, PivotItemOptions> itemOption in this.ItemOptions)
      {
        if (itemOption.Key >= 0)
        {
          string ItemValue = (string) null;
          if (itemOption.Key < this.CacheField.FieldGroup.PivotRangeGroupNames.Count && this.CacheField.FieldGroup.PivotRangeGroupNames[itemOption.Key] != null)
            ItemValue = this.CacheField.FieldGroup.PivotRangeGroupNames[itemOption.Key].ToString();
          this.m_PivotFieldItems.Add((object) this, ItemValue, itemOption.Value);
        }
      }
    }
    else
    {
      foreach (KeyValuePair<int, PivotItemOptions> itemOption in this.ItemOptions)
      {
        if (itemOption.Key >= 0)
        {
          string ItemValue = (string) null;
          if (itemOption.Key < this.CacheField.Items.Count && this.CacheField.Items[itemOption.Key] != null)
            ItemValue = this.CacheField.Items[itemOption.Key] is double ? ((double) this.CacheField.Items[itemOption.Key]).ToString("r") : this.CacheField.Items[itemOption.Key].ToString();
          this.m_PivotFieldItems.Add((object) this, ItemValue, itemOption.Value);
        }
      }
    }
    this.ItemOptionSorted = true;
  }

  internal void LoadPivotItems(PivotCacheFieldImpl cacheField)
  {
    if (cacheField.ItemCount <= 0)
      return;
    if ((cacheField.DataType & PivotDataType.Blank) != (PivotDataType) 0)
      cacheField.DataType &= ~PivotDataType.Blank;
    this.m_PivotFieldItems = new PivotFieldItemsCollections();
    for (int index = 0; index < cacheField.ItemCount; ++index)
    {
      string str;
      if (cacheField.Items[index] != null)
      {
        str = cacheField.Items[index].ToString();
      }
      else
      {
        str = (string) null;
        cacheField.DataType |= PivotDataType.Blank;
      }
      this.m_PivotFieldItems.Add((object) this, str, str);
    }
  }

  public object Clone(object parent)
  {
    PivotFieldImpl parent1 = (PivotFieldImpl) this.MemberwiseClone();
    parent1.m_table = (PivotTableImpl) CommonObject.FindParent(parent, typeof (PivotTableImpl));
    PivotCacheImpl cache = parent1.m_table.Cache;
    parent1.m_viewFields = (PivotViewFieldsRecord) CloneUtils.CloneCloneable((ICloneable) this.m_viewFields);
    parent1.m_viewFieldsEx = (PivotViewFieldsExRecord) CloneUtils.CloneCloneable((ICloneable) this.m_viewFieldsEx);
    parent1.m_arrItems = CloneUtils.CloneCloneable<PivotViewItemRecord>(this.m_arrItems);
    if (this.m_cacheField != null)
    {
      int index = this.m_cacheField.Index;
      parent1.m_cacheField = cache.CacheFields[index];
    }
    if (this.m_autoSortItem != null)
      parent1.m_autoSortItem = this.m_autoSortItem.Clone((object) parent1) as PivotInnerItem;
    if (this.m_pivotArea != null)
      parent1.m_pivotArea = this.m_pivotArea.Clone((object) parent1) as PivotArea;
    if (this.m_sortedFieldItems != null)
    {
      parent1.m_sortedFieldItems = new Dictionary<string, int>();
      foreach (KeyValuePair<string, int> sortedFieldItem in this.m_sortedFieldItems)
        parent1.m_sortedFieldItems.Add(sortedFieldItem.Key, sortedFieldItem.Value);
    }
    return (object) parent1;
  }
}
