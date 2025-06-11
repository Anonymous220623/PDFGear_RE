// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartLegendEntriesColl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartLegendEntriesColl : CommonObject, IChartLegendEntries
{
  private Dictionary<int, ChartLegendEntryImpl> m_hashEntries = new Dictionary<int, ChartLegendEntryImpl>();
  private ChartImpl m_parentChart;

  public ChartLegendEntriesColl(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  public int Count
  {
    get
    {
      IChartSeries series = this.m_parentChart.Series;
      string startSerieType = ChartFormatImpl.GetStartSerieType(this.m_parentChart.ChartType);
      int count1;
      if (Array.IndexOf<string>(ChartImpl.DEF_LEGEND_NEED_DATA_POINT, startSerieType) == -1)
      {
        int count2 = series.Count;
        int index = 0;
        for (int count3 = series.Count; index < count3; ++index)
        {
          IChartSerie chartSerie = series[index];
          count2 += chartSerie.TrendLines.Count;
        }
        count1 = count2;
      }
      else
        count1 = ((ChartSerieImpl) series[0]).PointNumber;
      return count1;
    }
  }

  public IChartLegendEntry this[int iIndex]
  {
    get
    {
      if (!this.m_parentChart.Loading && iIndex >= this.Count)
        throw new ArgumentOutOfRangeException(nameof (iIndex));
      if (this.m_hashEntries.ContainsKey(iIndex))
        return (IChartLegendEntry) this.m_hashEntries[iIndex];
      IChartSeries series = this.m_parentChart.Series;
      List<IChartTrendLine> chartTrendLineList = new List<IChartTrendLine>();
      int index = iIndex - series.Count;
      if (iIndex >= series.Count)
      {
        foreach (IChartSerie chartSerie in (IEnumerable<IChartSerie>) series)
        {
          if (chartSerie.TrendLines.Count > 0)
          {
            for (int iIndex1 = 0; iIndex1 < chartSerie.TrendLines.Count; ++iIndex1)
              chartTrendLineList.Add(chartSerie.TrendLines[iIndex1]);
          }
        }
      }
      ChartLegendEntryImpl chartLegendEntryImpl = this.Add(iIndex);
      if (index >= 0 && index < chartTrendLineList.Count)
        (chartTrendLineList[index] as ChartTrendLineImpl).LegendEntry = chartLegendEntryImpl;
      return (IChartLegendEntry) chartLegendEntryImpl;
    }
  }

  internal Dictionary<int, ChartLegendEntryImpl> HashEntries => this.m_hashEntries;

  private void SetParents()
  {
    this.m_parentChart = (ChartImpl) this.FindParent(typeof (ChartImpl));
    if (this.m_parentChart == null)
      throw new ApplicationException("Can't find parent object.");
  }

  public ChartLegendEntryImpl Add(int iIndex)
  {
    if (this.m_hashEntries.ContainsKey(iIndex))
      return this.m_hashEntries[iIndex];
    ChartLegendEntryImpl entry = new ChartLegendEntryImpl(this.Application, (object) this, iIndex);
    return this.Add(iIndex, entry);
  }

  public ChartLegendEntryImpl Add(int iIndex, ChartLegendEntryImpl entry)
  {
    if (!this.m_parentChart.Loading && iIndex >= this.Count)
      throw new ArgumentOutOfRangeException(nameof (iIndex));
    if (entry == null)
      throw new ArgumentNullException(nameof (entry));
    entry.Index = iIndex;
    if (!this.m_parentChart.Loading)
    {
      string startSerieType = ChartFormatImpl.GetStartSerieType(this.m_parentChart.ChartType);
      if (Array.IndexOf<string>(ChartImpl.DEF_LEGEND_NEED_DATA_POINT, startSerieType) != -1)
        entry.LegendEntityIndex = iIndex;
    }
    if (this.m_hashEntries.ContainsKey(iIndex))
      this.m_hashEntries[iIndex] = entry;
    else
      this.m_hashEntries.Add(iIndex, entry);
    return entry;
  }

  public bool Contains(int iIndex) => this.m_hashEntries.ContainsKey(iIndex);

  public bool CanDelete(int iIndex)
  {
    if (this.m_hashEntries.Count != this.Count)
      return true;
    int key = 0;
    for (int count = this.m_hashEntries.Count; key < count; ++key)
    {
      if (key != iIndex)
      {
        ChartLegendEntryImpl chartLegendEntryImpl = (ChartLegendEntryImpl) null;
        if (this.m_hashEntries.TryGetValue(key, out chartLegendEntryImpl) && !chartLegendEntryImpl.IsDeleted)
          return true;
      }
    }
    return false;
  }

  public void Remove(int iIndex)
  {
    int count = this.Count;
    string startSerieType = ChartFormatImpl.GetStartSerieType(this.m_parentChart.ChartType);
    if (Array.IndexOf<string>(ChartImpl.DEF_LEGEND_NEED_DATA_POINT, startSerieType) != -1)
      return;
    if (iIndex < 0 || iIndex >= count)
      throw new ArgumentOutOfRangeException(nameof (iIndex));
    if (this.m_hashEntries.ContainsKey(iIndex))
      this.m_hashEntries.Remove(iIndex);
    for (int key = iIndex + 1; key < count; ++key)
    {
      if (this.m_hashEntries.ContainsKey(key))
      {
        ChartLegendEntryImpl hashEntry = this.m_hashEntries[key];
        hashEntry.Index = key - 1;
        this.m_hashEntries.Add(key - 1, hashEntry);
        this.m_hashEntries.Remove(key);
      }
    }
  }

  public ChartLegendEntriesColl Clone(
    object parent,
    Dictionary<int, int> dicIndexes,
    Dictionary<string, string> dicNewSheetNames)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    ChartLegendEntriesColl parent1 = (ChartLegendEntriesColl) this.MemberwiseClone();
    parent1.SetParent(parent);
    parent1.SetParents();
    int count = this.m_hashEntries.Count;
    parent1.m_hashEntries = new Dictionary<int, ChartLegendEntryImpl>(count);
    if (count == 0)
      return parent1;
    foreach (KeyValuePair<int, ChartLegendEntryImpl> hashEntry in this.m_hashEntries)
    {
      ChartLegendEntryImpl chartLegendEntryImpl = hashEntry.Value.Clone((object) parent1, dicIndexes, dicNewSheetNames);
      parent1.m_hashEntries.Add(hashEntry.Key, chartLegendEntryImpl);
    }
    return parent1;
  }

  public void Clear() => this.m_hashEntries.Clear();

  public void UpdateEntries(int entryIndex, int value)
  {
    int count = this.Count;
    for (int key = this.Count - 1; key >= entryIndex; --key)
    {
      if (this.m_hashEntries.ContainsKey(key))
      {
        ChartLegendEntryImpl hashEntry = this.m_hashEntries[key];
        hashEntry.Index += value;
        this.m_hashEntries.Add(hashEntry.Index, hashEntry);
        this.m_hashEntries.Remove(key);
      }
    }
  }
}
