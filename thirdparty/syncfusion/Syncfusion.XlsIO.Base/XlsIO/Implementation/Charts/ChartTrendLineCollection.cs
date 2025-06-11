// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartTrendLineCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartTrendLineCollection : CollectionBaseEx<IChartTrendLine>, IChartTrendLines
{
  private ChartSerieImpl m_parentSerie;

  public ChartTrendLineCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_parentSerie = (ChartSerieImpl) this.FindParent(typeof (ChartSerieImpl));
    if (this.m_parentSerie == null)
      throw new ApplicationException("Cannot find parent objects.");
  }

  public new IChartTrendLine this[int iIndex]
  {
    get
    {
      if (iIndex >= this.List.Count || iIndex < 0)
        throw new ArgumentOutOfRangeException("Index is out of bounds of collection.");
      this.CheckSeriesType();
      IChartTrendLine chartTrendLine = this.List[iIndex];
      if (!this.IsParsed)
        this.CheckNegativeValues(chartTrendLine.Type);
      return chartTrendLine;
    }
  }

  public IChartTrendLine Add() => this.Add(ExcelTrendLineType.Linear);

  public IChartTrendLine Add(ExcelTrendLineType type)
  {
    this.CheckSeriesType();
    this.CheckNegativeValues(type);
    ChartTrendLineImpl chartTrendLineImpl = new ChartTrendLineImpl(this.Application, (object) this);
    chartTrendLineImpl.Type = type;
    this.Add((IChartTrendLine) chartTrendLineImpl);
    return (IChartTrendLine) chartTrendLineImpl;
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

  private void CheckNegativeValues(ExcelTrendLineType type)
  {
    if (type != ExcelTrendLineType.Power && type != ExcelTrendLineType.Exponential || this.m_parentSerie.Values == null || this.m_parentSerie.Values is ExternalRange)
      return;
    IRange[] cells = this.m_parentSerie.Values.Cells;
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
    this.Add((IChartTrendLine) trend);
  }

  public void CheckSeriesType()
  {
    if (!this.IsParsed && Array.IndexOf<ExcelChartType>(ChartImpl.DEF_SUPPORT_TREND_LINES, this.m_parentSerie.SerieType) == -1)
      throw new ArgumentNullException("Current serie type doesnot support trend lines.");
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    System.Collections.Generic.List<IChartTrendLine> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      ((ChartTrendLineImpl) innerList[index]).MarkUsedReferences(usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    System.Collections.Generic.List<IChartTrendLine> innerList = this.InnerList;
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
