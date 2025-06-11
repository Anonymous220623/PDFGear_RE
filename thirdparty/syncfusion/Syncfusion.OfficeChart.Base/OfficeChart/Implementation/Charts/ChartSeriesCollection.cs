// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartSeriesCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartSeriesCollection : 
  CollectionBaseEx<IOfficeChartSerie>,
  IOfficeChartSeries,
  IParentApplication,
  ICloneParent,
  IList<IOfficeChartSerie>,
  ICollection<IOfficeChartSerie>,
  IEnumerable<IOfficeChartSerie>,
  IEnumerable
{
  public const string DEF_START_SERIE_NAME = "Serie";
  private ChartImpl m_chart;
  private IList<IBiffStorage> m_arrTrendError = (IList<IBiffStorage>) new System.Collections.Generic.List<IBiffStorage>();
  private IList<IBiffStorage> m_arrTrendLabels = (IList<IBiffStorage>) new System.Collections.Generic.List<IBiffStorage>();
  private int m_trendErrorBarsIndex;
  private int m_trendsIndex;
  private System.Collections.Generic.List<IOfficeChartSerie> m_additionOrder = new System.Collections.Generic.List<IOfficeChartSerie>();

  public ChartSeriesCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_chart = (ChartImpl) this.FindParent(typeof (ChartImpl));
    if (this.m_chart == null)
      throw new ApplicationException("cannot find parent chart.");
  }

  public new IOfficeChartSerie this[int index]
  {
    get => this.List[index];
    set => throw new NotSupportedException();
  }

  public IOfficeChartSerie this[string name]
  {
    get
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IOfficeChartSerie officeChartSerie = this.List[index];
        if (officeChartSerie.Name == name)
          return officeChartSerie;
      }
      return (IOfficeChartSerie) null;
    }
  }

  public IOfficeChartSerie Add()
  {
    return this.Add(new ChartSerieImpl(this.Application, (object) this)
    {
      IsDefaultName = true
    });
  }

  public IOfficeChartSerie Add(string name)
  {
    ChartSerieImpl serieToAdd = new ChartSerieImpl(this.Application, (object) this);
    serieToAdd.Name = name;
    if (this.m_chart.HasTitle && this.m_chart.ChartTitle == null)
      this.m_chart.ChartTitle = name;
    return this.Add(serieToAdd);
  }

  public IOfficeChartSerie Add(OfficeChartType serieType)
  {
    ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.Add();
    chartSerieImpl.ChangeSeriesType(serieType, true);
    return (IOfficeChartSerie) chartSerieImpl;
  }

  public IOfficeChartSerie Add(string name, OfficeChartType serieType)
  {
    IOfficeChartSerie officeChartSerie = this.Add(name);
    officeChartSerie.SerieType = serieType;
    return officeChartSerie;
  }

  public new void RemoveAt(int index)
  {
    int count = this.List.Count;
    ChartSerieImpl chartSerieImpl = index >= 0 && index < count ? (ChartSerieImpl) this[index] : throw new ArgumentOutOfRangeException(nameof (index));
    if (this.m_chart.HasLegend)
      ((ChartLegendEntriesColl) this.m_chart.Legend.LegendEntries).Remove(index);
    base.RemoveAt(index);
    if (this.GetCountOfSeriesWithSameDrawingOrder(chartSerieImpl.ChartGroup) == 0 && count != 1)
      this.m_chart.RemoveFormat((IOfficeChartFormat) chartSerieImpl.GetCommonSerieFormat());
    this.UpdateSerieIndexAfterRemove(index);
    if (this.HasSecondary())
      return;
    this.m_chart.RemoveSecondaryAxes();
  }

  public void Remove(string serieName)
  {
    if (serieName == null)
      throw new ArgumentException(nameof (serieName));
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.List[index];
      if (chartSerieImpl.Name == serieName)
      {
        this.RemoveAt(chartSerieImpl.Index);
        --index;
        --count;
      }
    }
  }

  public void ParseSiIndex(IList<BiffRecordRaw> data, ref int iPos)
  {
    BiffRecordRaw biffRecordRaw1 = data != null ? data[iPos] : throw new ArgumentNullException(nameof (data));
    int siIndex = biffRecordRaw1.TypeCode == TBIFFRecord.ChartSiIndex ? (int) ((ChartSiIndexRecord) biffRecordRaw1).NumIndex : throw new ArgumentOutOfRangeException("ChartSiIndex record was expected.");
    ++iPos;
    BiffRecordRaw biffRecordRaw2 = data[iPos];
    while (biffRecordRaw2.TypeCode == TBIFFRecord.Number || biffRecordRaw2.TypeCode == TBIFFRecord.Label || biffRecordRaw2.TypeCode == TBIFFRecord.Blank)
    {
      ICellPositionFormat record = (ICellPositionFormat) biffRecordRaw2;
      ++iPos;
      biffRecordRaw2 = data[iPos];
      if (record.Column < this.Count)
        this.AddEnteredRecord(siIndex, record);
    }
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    this.m_arrTrendError.Clear();
    this.m_arrTrendLabels.Clear();
    this.m_trendErrorBarsIndex = this.Count;
    this.m_trendsIndex = this.Count;
    foreach (ChartSerieImpl chartSerieImpl in (IEnumerable<IOfficeChartSerie>) this.List)
      chartSerieImpl.Serialize(records);
    records.AddRange((ICollection<IBiffStorage>) this.m_arrTrendError);
  }

  [CLSCompliant(false)]
  public void SerializeDataLabels(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      ((ChartSerieImpl) this.InnerList[index]).SerializeDataLabels(records);
  }

  public IOfficeChartSerie Add(ChartSerieImpl serieToAdd)
  {
    if (serieToAdd == null)
      throw new ArgumentNullException(nameof (serieToAdd));
    if (serieToAdd.IsDefaultName)
      serieToAdd.SetDefaultName(this.GetDefSerieName(), false);
    this.Add((IOfficeChartSerie) serieToAdd);
    serieToAdd.Index = this.List.Count - 1;
    if (!this.m_chart.ParentWorkbook.IsWorkbookOpening)
    {
      serieToAdd.Number = serieToAdd.Index;
      this.m_additionOrder.Clear();
    }
    else
      this.m_additionOrder.Add((IOfficeChartSerie) serieToAdd);
    return (IOfficeChartSerie) serieToAdd;
  }

  protected override void OnClear() => base.OnClear();

  public ChartSeriesCollection Clone(
    object parent,
    Dictionary<string, string> hashNewNames,
    Dictionary<int, int> dicFontIndexes)
  {
    ChartSeriesCollection parent1 = new ChartSeriesCollection(this.Application, parent);
    int index = 0;
    for (int count = this.InnerList.Count; index < count; ++index)
    {
      ChartSerieImpl serieToAdd = (this.InnerList[index] as ChartSerieImpl).Clone((object) parent1, hashNewNames, dicFontIndexes);
      serieToAdd.IsDefaultName = false;
      parent1.Add(serieToAdd);
    }
    if (this.m_arrTrendError != null)
      parent1.m_arrTrendError = this.m_arrTrendError;
    if (this.m_arrTrendLabels != null)
      parent1.m_arrTrendLabels = this.m_arrTrendLabels;
    return parent1;
  }

  public override object Clone(object parent)
  {
    ChartSeriesCollection parent1 = new ChartSeriesCollection(this.Application, parent);
    System.Collections.Generic.List<IOfficeChartSerie> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      ChartSerieImpl serieToAdd = (innerList[index] as ChartSerieImpl).Clone((object) parent1, (Dictionary<string, string>) null, (Dictionary<int, int>) null);
      parent1.Add(serieToAdd);
    }
    return (object) parent1;
  }

  public int GetCountOfSeriesWithSameDrawingOrder(int order)
  {
    int sameDrawingOrder = 0;
    int index = 0;
    for (int count = this.List.Count; index < count; ++index)
    {
      if (((ChartSerieImpl) this.List[index]).ChartGroup == order)
        ++sameDrawingOrder;
    }
    return sameDrawingOrder;
  }

  public System.Collections.Generic.List<ChartSerieImpl> GetSeriesWithDrawingOrder(int order)
  {
    System.Collections.Generic.List<ChartSerieImpl> withDrawingOrder = new System.Collections.Generic.List<ChartSerieImpl>();
    int index = 0;
    for (int count = this.List.Count; index < count; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.List[index];
      if (chartSerieImpl.ChartGroup == order)
        withDrawingOrder.Add(chartSerieImpl);
    }
    return withDrawingOrder;
  }

  public int GetCountOfSeriesWithSameType(OfficeChartType type, bool usePrimaryAxis)
  {
    int seriesWithSameType = 0;
    int index = 0;
    for (int count = this.List.Count; index < count; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.List[index];
      if (chartSerieImpl.SerieType == type && chartSerieImpl.UsePrimaryAxis == usePrimaryAxis)
        ++seriesWithSameType;
    }
    return seriesWithSameType;
  }

  public int GetCountOfSeriesWithSameStartType(OfficeChartType type)
  {
    string startSerieType = ChartFormatImpl.GetStartSerieType(type);
    int withSameStartType = 0;
    int index = 0;
    for (int count = this.List.Count; index < count; ++index)
    {
      if (((ChartSerieImpl) this.List[index]).StartType == startSerieType)
        ++withSameStartType;
    }
    return withSameStartType;
  }

  internal void ClearSeriesForChangeChartType(bool preserveFormats)
  {
    System.Collections.Generic.List<ChartDataLabelsImpl> chartDataLabelsImplList = (System.Collections.Generic.List<ChartDataLabelsImpl>) null;
    if (!this.m_chart.Loading)
      chartDataLabelsImplList = new System.Collections.Generic.List<ChartDataLabelsImpl>(this.Count);
    for (int index = 0; index < this.Count; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.List[index];
      chartSerieImpl.ChartGroup = 0;
      if (chartDataLabelsImplList != null)
      {
        ChartDataPointImpl defaultDataPoint = chartSerieImpl.DataPoints.DefaultDataPoint as ChartDataPointImpl;
        if (defaultDataPoint.HasDataLabels)
        {
          ChartDataLabelsImpl chartDataLabelsImpl = (defaultDataPoint.DataLabels as ChartDataLabelsImpl).Clone((object) defaultDataPoint, (Dictionary<int, int>) null, (Dictionary<string, string>) null) as ChartDataLabelsImpl;
          chartDataLabelsImplList.Add(chartDataLabelsImpl);
        }
        else
          chartDataLabelsImplList.Add((ChartDataLabelsImpl) null);
      }
      if (preserveFormats)
        ((ChartDataPointsCollection) chartSerieImpl.DataPoints).ClearWithExistingFormats(this.m_chart.DestinationType);
      else
        ((ChartDataPointsCollection) chartSerieImpl.DataPoints).Clear();
    }
    if (chartDataLabelsImplList == null)
      return;
    for (int index = 0; index < this.Count; ++index)
    {
      if (chartDataLabelsImplList[index] != null)
        (this.List[index].DataPoints.DefaultDataPoint as ChartDataPointImpl).DataLabels = (IOfficeChartDataLabels) chartDataLabelsImplList[index];
    }
    chartDataLabelsImplList.Clear();
  }

  public void ClearSeriesForChangeChartType() => this.ClearSeriesForChangeChartType(false);

  public int FindOrderByType(OfficeChartType type)
  {
    Dictionary<string, object> dictionary1 = new Dictionary<string, object>();
    Dictionary<int, object> dictionary2 = new Dictionary<int, object>(5);
    for (int index = 0; index < this.Count; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.List[index];
      int chartGroup = chartSerieImpl.ChartGroup;
      if (!dictionary2.ContainsKey(chartGroup))
      {
        dictionary2.Add(chartGroup, (object) null);
        string startSerieType = ChartFormatImpl.GetStartSerieType(chartSerieImpl.SerieType);
        dictionary1[startSerieType] = (object) null;
      }
    }
    string startSerieType1 = ChartFormatImpl.GetStartSerieType(type);
    int orderByType = 0;
    int index1 = 0;
    for (int length = ChartImpl.DEF_PRIORITY_START_TYPES.Length; index1 < length; ++index1)
    {
      string key = ChartImpl.DEF_PRIORITY_START_TYPES[index1];
      if (startSerieType1 == key)
        return orderByType;
      if (dictionary1.ContainsKey(key))
        ++orderByType;
    }
    throw new ApplicationException("Cannot find order.");
  }

  public void UpdateDataPointForCylConePurChartType(
    OfficeBaseFormat baseFormat,
    OfficeTopFormat topFormat)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      IOfficeChartSerieDataFormat dataFormat = ((ChartSerieImpl) this.List[index]).DataPoints.DefaultDataPoint.DataFormat;
      dataFormat.BarShapeBase = baseFormat;
      dataFormat.BarShapeTop = topFormat;
    }
  }

  private void AddEnteredRecord(int siIndex, ICellPositionFormat record)
  {
    if (siIndex > 3 || siIndex < 1)
      throw new ArgumentOutOfRangeException(nameof (siIndex));
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    ((ChartSerieImpl) this.List[record.Column]).AddEnteredRecord(siIndex, record);
  }

  public System.Collections.Generic.List<BiffRecordRaw> GetEnteredRecords(int siIndex)
  {
    if (siIndex > 3 || siIndex < 1)
      throw new ArgumentOutOfRangeException(nameof (siIndex));
    System.Collections.Generic.List<BiffRecordRaw> enteredRecords = new System.Collections.Generic.List<BiffRecordRaw>();
    System.Collections.Generic.List<System.Collections.Generic.List<BiffRecordRaw>> arrays = this.GetArrays(siIndex);
    if (arrays == null)
      return (System.Collections.Generic.List<BiffRecordRaw>) null;
    int val1 = arrays[0].Count;
    int index1 = 1;
    for (int count = arrays.Count; index1 < count; ++index1)
      val1 = Math.Max(val1, arrays[index1].Count);
    for (int index2 = 0; index2 < val1; ++index2)
    {
      int index3 = 0;
      for (int count = arrays.Count; index3 < count; ++index3)
      {
        System.Collections.Generic.List<BiffRecordRaw> biffRecordRawList = arrays[index3];
        if (biffRecordRawList.Count > index2)
          enteredRecords.Add(biffRecordRawList[index2]);
      }
    }
    return enteredRecords;
  }

  private System.Collections.Generic.List<System.Collections.Generic.List<BiffRecordRaw>> GetArrays(
    int siIndex)
  {
    System.Collections.Generic.List<System.Collections.Generic.List<BiffRecordRaw>> biffRecordRawListList = new System.Collections.Generic.List<System.Collections.Generic.List<BiffRecordRaw>>();
    for (int index = 0; index < this.Count; ++index)
    {
      System.Collections.Generic.List<BiffRecordRaw> array = ((ChartSerieImpl) this.List[index]).GetArray(siIndex);
      if (array != null)
        biffRecordRawListList.Add(array);
    }
    return biffRecordRawListList.Count == 0 ? (System.Collections.Generic.List<System.Collections.Generic.List<BiffRecordRaw>>) null : biffRecordRawListList;
  }

  public void UpdateSerieIndexAfterRemove(int iRemoveIndex)
  {
    if (iRemoveIndex < 0 || iRemoveIndex > this.List.Count)
      throw new ArgumentOutOfRangeException(nameof (iRemoveIndex));
    int index = iRemoveIndex;
    for (int count = this.List.Count; index < count; ++index)
      --((ChartSerieImpl) this.List[index]).Index;
  }

  public OfficeChartType GetTypeByOrder(int iOrder)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.List[index];
      if (chartSerieImpl.ChartGroup == iOrder)
        return chartSerieImpl.SerieType;
    }
    throw new ArgumentOutOfRangeException(nameof (iOrder));
  }

  public void ClearDataFormats(ChartSerieDataFormatImpl format)
  {
    int index = 0;
    for (int count = this.List.Count; index < count; ++index)
      ((ChartDataPointsCollection) ((ChartSerieImpl) this.List[index]).DataPoints).ClearDataFormats(format);
  }

  public string GetDefSerieName()
  {
    return CollectionBaseEx<IOfficeChartSerie>.GenerateDefaultName((ICollection<IOfficeChartSerie>) this.List, "Serie");
  }

  public string GetDefSerieName(int iSerieIndex)
  {
    int count = this.List.Count;
    if (iSerieIndex > count || iSerieIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (iSerieIndex));
    IList<IOfficeChartSerie> namesCollection;
    if (iSerieIndex == this.List.Count)
    {
      namesCollection = this.List;
    }
    else
    {
      namesCollection = (IList<IOfficeChartSerie>) new System.Collections.Generic.List<IOfficeChartSerie>(iSerieIndex);
      for (int index = 0; index < iSerieIndex; ++index)
        namesCollection.Add(this.List[index]);
    }
    return CollectionBaseEx<IOfficeChartSerie>.GenerateDefaultName((ICollection<IOfficeChartSerie>) namesCollection, "Serie");
  }

  public void UpdateFormula(
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect)
  {
    System.Collections.Generic.List<IOfficeChartSerie> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      ((ChartSerieImpl) innerList[index]).UpdateFormula(iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect);
  }

  public int GetLegendEntryOffset(int iSerIndex)
  {
    if (iSerIndex >= this.Count)
      throw new ArgumentOutOfRangeException(nameof (iSerIndex));
    int num = 0;
    for (int index = 0; index < iSerIndex; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.List[index];
      num += chartSerieImpl.TrendLines.Count;
    }
    return num + this.Count;
  }

  public void AssignTrendDataLabel(ChartTextAreaImpl area)
  {
    if (area == null)
      throw new ArgumentNullException(nameof (area));
    int index = 0;
    for (int count1 = this.Count; index < count1; ++index)
    {
      IOfficeChartTrendLines trendLines = this.List[index].TrendLines;
      int iIndex = 0;
      for (int count2 = trendLines.Count; iIndex < count2; ++iIndex)
      {
        ChartTrendLineImpl chartTrendLineImpl = (ChartTrendLineImpl) trendLines[iIndex];
        if (chartTrendLineImpl.Index == (int) area.ObjectLink.SeriesNumber)
        {
          chartTrendLineImpl.SetDataLabel(area);
          return;
        }
      }
    }
  }

  internal void ClearErrorBarsAndTrends()
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      IOfficeChartSerie officeChartSerie = this.List[index];
      IOfficeChartTrendLines trendLines = officeChartSerie.TrendLines;
      officeChartSerie.HasErrorBarsX = false;
      officeChartSerie.HasErrorBarsY = false;
      trendLines.Clear();
    }
  }

  internal void ResortSeries(Dictionary<int, int> dictSeriesAxis, System.Collections.Generic.List<int> markerSeriesList)
  {
    int count = this.Count;
    bool flag = markerSeriesList != null && markerSeriesList.Count > 0;
    if (count <= 1)
      return;
    System.Collections.Generic.List<IOfficeChartSerie> innerList = this.InnerList;
    SortedList<int, ChartSerieImpl> sortedList = new SortedList<int, ChartSerieImpl>();
    System.Collections.Generic.List<int> intList = new System.Collections.Generic.List<int>(count);
    for (int index1 = 0; index1 < count; ++index1)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) innerList[index1];
      int index2 = chartSerieImpl.Index;
      sortedList.Add(index2, chartSerieImpl);
    }
    IList<ChartSerieImpl> values = sortedList.Values;
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    for (int index3 = 0; index3 < count; ++index3)
    {
      ChartSerieImpl chartSerieImpl = values[index3];
      innerList[index3] = (IOfficeChartSerie) chartSerieImpl;
      int index4 = chartSerieImpl.Index;
      dictionary[index3] = index4;
      chartSerieImpl.Index = index3;
    }
    for (int index = 0; index < count; ++index)
    {
      int key = dictionary[index];
      int num;
      if (dictSeriesAxis.TryGetValue(key, out num))
      {
        ChartAxisImpl primaryCategoryAxis = this.m_chart.PrimaryCategoryAxis as ChartAxisImpl;
        ChartAxisImpl primaryValueAxis = this.m_chart.PrimaryValueAxis as ChartAxisImpl;
        if (num != primaryCategoryAxis.AxisId && num != primaryValueAxis.AxisId)
          ((ChartSerieImpl) innerList[index]).UsePrimaryAxis = false;
      }
      if (flag && markerSeriesList.Contains(key))
      {
        ChartMarkerFormatRecord markerFormat = ((ChartSerieDataFormatImpl) ((ChartSerieImpl) innerList[index]).SerieFormat).MarkerFormat;
        ((ChartSerieDataFormatImpl) ((ChartSerieImpl) innerList[index]).GetCommonSerieFormat().SerieDataFormat).MarkerFormat.MarkerType = markerFormat.MarkerType;
      }
    }
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    System.Collections.Generic.List<IOfficeChartSerie> innerList = this.InnerList;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      ((ChartSerieImpl) innerList[index]).MarkUsedReferences(usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    System.Collections.Generic.List<IOfficeChartSerie> innerList = this.InnerList;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      ((ChartSerieImpl) innerList[index]).UpdateReferenceIndexes(arrUpdatedIndexes);
  }

  internal System.Collections.Generic.List<IOfficeChartSerie> AdditionOrder => this.m_additionOrder;

  internal bool HasSecondary()
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (!this[index].UsePrimaryAxis)
        return true;
    }
    return false;
  }

  internal IList<IBiffStorage> TrendErrorList => this.m_arrTrendError;

  internal int TrendErrorBarIndex
  {
    get => this.m_trendErrorBarsIndex;
    set => this.m_trendErrorBarsIndex = value;
  }

  internal IList<IBiffStorage> TrendLabels => this.m_arrTrendLabels;

  internal int TrendIndex
  {
    get => this.m_trendsIndex;
    set => this.m_trendsIndex = value;
  }
}
