// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartTrendLineCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartTrendLineCollection : 
  CollectionBaseEx<IOfficeChartTrendLine>,
  IOfficeChartTrendLines
{
  private ChartSerieImpl m_parentSerie;

  public ChartTrendLineCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_parentSerie = (ChartSerieImpl) this.FindParent(typeof (ChartSerieImpl));
    if (this.m_parentSerie == null)
      throw new ApplicationException("Cannot find parent objects.");
  }

  public new IOfficeChartTrendLine this[int iIndex]
  {
    get
    {
      if (iIndex >= this.List.Count || iIndex < 0)
        throw new ArgumentOutOfRangeException("Index is out of bounds of collection.");
      this.CheckSeriesType();
      IOfficeChartTrendLine officeChartTrendLine = this.List[iIndex];
      if (!this.IsParsed)
        this.CheckNegativeValues(officeChartTrendLine.Type);
      return officeChartTrendLine;
    }
  }

  public IOfficeChartTrendLine Add() => this.Add(OfficeTrendLineType.Linear);

  public IOfficeChartTrendLine Add(OfficeTrendLineType type)
  {
    this.CheckSeriesType();
    this.CheckNegativeValues(type);
    ChartTrendLineImpl chartTrendLineImpl = new ChartTrendLineImpl(this.Application, (object) this);
    chartTrendLineImpl.Type = type;
    this.Add((IOfficeChartTrendLine) chartTrendLineImpl);
    return (IOfficeChartTrendLine) chartTrendLineImpl;
  }

  public new void RemoveAt(int index)
  {
    if (index < 0 || index >= this.Count)
      throw new ArgumentOutOfRangeException(nameof (index));
    this.CheckSeriesType();
    base.RemoveAt(index);
  }

  [CLSCompliant(false)]
  public void Serialize(IList<IBiffStorage> records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      ((ChartTrendLineImpl) this.List[index]).Serialize(records);
  }

  private void CheckNegativeValues(OfficeTrendLineType type)
  {
    if (type != OfficeTrendLineType.Power && type != OfficeTrendLineType.Exponential || this.m_parentSerie.ValuesIRange is ExternalRange)
      return;
    IRange[] cells = this.m_parentSerie.ValuesIRange.Cells;
    int index = 0;
    for (int length = cells.Length; index < length; ++index)
    {
      IRange range = cells[index];
      if (range.HasNumber && range.Number <= 0.0)
        throw new NotSupportedException("Cannot perform current operation becouse one ofseries values is less or equal zero.");
    }
  }

  public void Add(ChartTrendLineImpl trend)
  {
    if (trend == null)
      throw new ArgumentNullException(nameof (trend));
    this.Add((IOfficeChartTrendLine) trend);
  }

  public void CheckSeriesType()
  {
    if (!this.IsParsed && Array.IndexOf<OfficeChartType>(ChartImpl.DEF_SUPPORT_TREND_LINES, this.m_parentSerie.SerieType) == -1)
      throw new ArgumentNullException("Current serie type doesnot support trend lines.");
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    System.Collections.Generic.List<IOfficeChartTrendLine> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      ((ChartTrendLineImpl) innerList[index]).MarkUsedReferences(usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    System.Collections.Generic.List<IOfficeChartTrendLine> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      ((ChartTrendLineImpl) innerList[index]).UpdateReferenceIndexes(arrUpdatedIndexes);
  }

  private bool IsParsed => this.m_parentSerie.ParentChart.IsParsed;

  public ChartTrendLineCollection Clone(
    object parent,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<string, string> dicNewSheetNames)
  {
    ChartTrendLineCollection parent1 = parent != null ? new ChartTrendLineCollection(this.Application, parent) : throw new ArgumentNullException(nameof (parent));
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      ChartTrendLineImpl chartTrendLineImpl = (ChartTrendLineImpl) this.List[index];
      parent1.Add(chartTrendLineImpl.Clone((object) parent1, dicFontIndexes, dicNewSheetNames));
    }
    return parent1;
  }
}
