// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartDataPointsCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartDataPointsCollection : 
  CommonObject,
  IChartDataPoints,
  IParentApplication,
  IEnumerable
{
  private ChartDataPointImpl m_dataPointDefault;
  internal Dictionary<int, ChartDataPointImpl> m_hashDataPoints = new Dictionary<int, ChartDataPointImpl>();
  private ChartSerieImpl m_series;
  private ChartImpl m_chart;

  public ChartDataPointsCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_dataPointDefault = new ChartDataPointImpl(this.Application, (object) this, (int) ushort.MaxValue);
    if (parent is ChartSerieImpl)
    {
      this.SetParents();
      this.m_chart = (ChartImpl) null;
    }
    else
    {
      this.SetParentChart();
      this.m_series = (ChartSerieImpl) null;
    }
  }

  private void SetParents()
  {
    this.m_series = this.FindParent(typeof (ChartSerieImpl)) as ChartSerieImpl;
    if (this.m_series == null)
      throw new ArgumentNullException("Can't find parent series.");
  }

  private void SetParentChart()
  {
    this.m_chart = this.FindParent(typeof (ChartImpl)) as ChartImpl;
    if (this.m_chart == null)
      throw new ArgumentNullException("Can't find parent chart.");
  }

  public IChartDataPoint this[int index]
  {
    get
    {
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (index == (int) ushort.MaxValue)
        return this.DefaultDataPoint;
      WorksheetImpl parent = this.FindParent(typeof (WorksheetImpl)) as WorksheetImpl;
      if (!this.IsLoading && (parent == null || !parent.IsParsing))
      {
        if (this.m_series != null)
        {
          int pointNumber = this.m_series.PointNumber;
          if (index >= pointNumber)
            throw new ArgumentOutOfRangeException(nameof (index));
        }
        else if (this.m_chart != null && this.m_chart.Series.Count > 0)
        {
          int num = (this.m_chart.Series[0] as ChartSerieImpl).PointNumber;
          for (int index1 = 1; index1 < this.m_chart.Series.Count; ++index1)
          {
            int pointNumber = (this.m_chart.Series[index1] as ChartSerieImpl).PointNumber;
            if (num < pointNumber)
              num = pointNumber;
          }
          if (index >= num)
            throw new ArgumentOutOfRangeException(nameof (index));
        }
      }
      ChartDataPointImpl point;
      if (this.m_hashDataPoints.ContainsKey(index))
      {
        point = this.m_hashDataPoints[index];
      }
      else
      {
        point = new ChartDataPointImpl(this.Application, (object) this, index);
        this.Add(point);
      }
      if (this.m_chart == null && this.m_series != null)
      {
        ChartDataPointImpl defaultDataPoint = (ChartDataPointImpl) this.DefaultDataPoint;
        ChartSerieDataFormatImpl dataFormatOrNull = defaultDataPoint.DataFormatOrNull;
        ChartDataLabelsImpl dataLabels = (ChartDataLabelsImpl) defaultDataPoint.DataLabels;
        if (dataFormatOrNull != null && dataFormatOrNull.IsFormatted)
          point.CloneDataFormat(dataFormatOrNull);
        if (dataLabels != null && dataLabels.ShowTextProperties && (parent == null || parent.Version != ExcelVersion.Excel97to2003))
          point.CloneDataLabels(dataLabels);
      }
      return (IChartDataPoint) point;
    }
  }

  public IChartDataPoint DefaultDataPoint
  {
    get
    {
      if (this.m_chart == null && this.m_series != null && !this.m_series.InnerChart.Loading && !this.m_series.InnerChart.TypeChanging)
        this.m_dataPointDefault.CloneDataFormat(this.m_series.GetCommonSerieFormat().DataFormatOrNull);
      return (IChartDataPoint) this.m_dataPointDefault;
    }
  }

  public bool IsLoading
  {
    get
    {
      return this.m_series != null ? this.m_series.InnerWorkbook.Loading : this.m_chart.InnerWorkbook.Loading;
    }
  }

  public ChartSerieDataFormatImpl DefPointFormatOrNull => this.m_dataPointDefault.DataFormatOrNull;

  [CLSCompliant(false)]
  public void SerializeDataLabels(OffsetArrayList records)
  {
    foreach (ChartDataPointImpl chartDataPointImpl in this.m_hashDataPoints.Values)
      chartDataPointImpl.SerializeDataLabels(records);
    if (this.m_dataPointDefault == null)
      return;
    this.m_dataPointDefault.SerializeDataLabels(records);
  }

  [CLSCompliant(false)]
  public void SerializeDataFormats(OffsetArrayList records)
  {
    foreach (ChartDataPointImpl chartDataPointImpl in this.m_hashDataPoints.Values)
      chartDataPointImpl.SerializeDataFormat(records);
    if (this.m_dataPointDefault == null)
      return;
    this.m_dataPointDefault.SerializeDataFormat(records);
  }

  public object Clone(
    object parent,
    WorkbookImpl book,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<string, string> dicNewSheetNames)
  {
    ChartDataPointsCollection parent1 = (ChartDataPointsCollection) this.MemberwiseClone();
    parent1.SetParent(parent);
    switch (parent)
    {
      case ChartSerieImpl _:
        parent1.SetParents();
        break;
      case ChartImpl _:
        parent1.SetParentChart();
        break;
    }
    int count = this.m_hashDataPoints.Count;
    parent1.m_hashDataPoints = new Dictionary<int, ChartDataPointImpl>(count);
    if (this.m_dataPointDefault != null)
      parent1.m_dataPointDefault = (ChartDataPointImpl) this.m_dataPointDefault.Clone((object) parent1, dicFontIndexes, dicNewSheetNames);
    if (count > 0)
    {
      foreach (ChartDataPointImpl chartDataPointImpl in this.m_hashDataPoints.Values)
      {
        ChartDataPointImpl point = (ChartDataPointImpl) chartDataPointImpl.Clone((object) parent1, dicFontIndexes, dicNewSheetNames);
        parent1.Add(point);
      }
    }
    return (object) parent1;
  }

  public void Add(ChartDataPointImpl point)
  {
    this.m_hashDataPoints[point.Index] = point != null ? point : throw new ArgumentNullException(nameof (point));
  }

  public void Clear()
  {
    this.m_hashDataPoints.Clear();
    this.m_dataPointDefault = new ChartDataPointImpl(this.Application, (object) this, (int) ushort.MaxValue);
    if (this.m_series == null)
      return;
    this.m_dataPointDefault.DataFormat.BarShapeBase = ExcelBaseFormat.Rectangle;
    this.m_dataPointDefault.DataFormat.BarShapeTop = ExcelTopFormat.Straight;
  }

  internal void ClearWithExistingFormats(ExcelChartType destinationType)
  {
    int count = this.m_hashDataPoints.Count;
    Dictionary<int, ChartDataPointImpl> dictionary = (Dictionary<int, ChartDataPointImpl>) null;
    bool interiorSupported = ChartSerieDataFormatImpl.GetIsInteriorSupported(destinationType);
    if (count > 0)
    {
      dictionary = new Dictionary<int, ChartDataPointImpl>(count);
      foreach (int key in this.m_hashDataPoints.Keys)
      {
        ChartDataPointImpl hashDataPoint = this.m_hashDataPoints[key];
        ChartDataPointImpl chartDataPointImpl = new ChartDataPointImpl(this.Application, (object) this, hashDataPoint.Index);
        chartDataPointImpl.CloneDataFormat(hashDataPoint.DataFormatOrNull);
        if (interiorSupported)
          hashDataPoint.DataFormatOrNull.CopyFillBackForeGroundColorObjects(chartDataPointImpl.DataFormatOrNull);
        if (chartDataPointImpl.DataFormatOrNull != null)
          chartDataPointImpl.DataFormatOrNull.SetDefaultValuesForSerieRecords();
        dictionary.Add(key, chartDataPointImpl);
      }
      this.m_hashDataPoints.Clear();
    }
    else
      this.m_hashDataPoints.Clear();
    ChartSerieDataFormatImpl serieDataFormatImpl = (ChartSerieDataFormatImpl) null;
    if (this.m_dataPointDefault.DataFormatOrNull != null)
    {
      serieDataFormatImpl = this.m_dataPointDefault.DataFormatOrNull.Clone((object) this.m_dataPointDefault);
      if (interiorSupported)
        this.m_dataPointDefault.DataFormatOrNull.CopyFillBackForeGroundColorObjects(serieDataFormatImpl);
    }
    this.m_dataPointDefault = new ChartDataPointImpl(this.Application, (object) this, (int) ushort.MaxValue);
    if (serieDataFormatImpl != null)
    {
      serieDataFormatImpl.SetDefaultValuesForSerieRecords();
      this.m_dataPointDefault.CloneDataFormat(serieDataFormatImpl);
      if (interiorSupported)
        serieDataFormatImpl.CopyFillBackForeGroundColorObjects(this.m_dataPointDefault.DataFormatOrNull);
    }
    if (dictionary == null)
      return;
    this.m_hashDataPoints = dictionary;
  }

  public void UpdateSerieIndex()
  {
    int index = this.m_series.Index;
    this.m_dataPointDefault.UpdateSerieIndex();
    foreach (ChartDataPointImpl chartDataPointImpl in this.m_hashDataPoints.Values)
      chartDataPointImpl.UpdateSerieIndex();
  }

  public void ClearDataFormats(ChartSerieDataFormatImpl format)
  {
    this.m_dataPointDefault.ClearDataFormats(format);
    if (this.m_hashDataPoints.Count == 0)
      return;
    foreach (ChartDataPointImpl chartDataPointImpl in this.m_hashDataPoints.Values)
      chartDataPointImpl.ClearDataFormats(format);
  }

  public int DeninedDPCount => this.m_hashDataPoints.Count;

  public IEnumerator GetEnumerator() => (IEnumerator) this.m_hashDataPoints.Values.GetEnumerator();

  internal bool CheckDPDataLabels()
  {
    if (this.m_hashDataPoints.Count == 0)
      return false;
    bool flag = false;
    foreach (int key in this.m_hashDataPoints.Keys)
    {
      if (this.m_hashDataPoints[key].HasDataLabels)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  protected override void OnDispose()
  {
    if (this.m_hashDataPoints != null)
    {
      foreach (CommonObject commonObject in this.m_hashDataPoints.Values)
        commonObject.Dispose();
      this.m_hashDataPoints.Clear();
      this.m_hashDataPoints = (Dictionary<int, ChartDataPointImpl>) null;
    }
    if (this.m_dataPointDefault == null)
      return;
    this.m_dataPointDefault.Dispose();
    this.m_dataPointDefault = (ChartDataPointImpl) null;
  }
}
