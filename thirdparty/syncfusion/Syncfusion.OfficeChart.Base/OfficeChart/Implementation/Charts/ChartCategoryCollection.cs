// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartCategoryCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartCategoryCollection : 
  CollectionBaseEx<IOfficeChartCategory>,
  IOfficeChartCategories,
  IParentApplication,
  ICloneParent,
  IList<IOfficeChartCategory>,
  ICollection<IOfficeChartCategory>,
  IEnumerable<IOfficeChartCategory>,
  IEnumerable
{
  private ChartImpl m_chart;

  public ChartCategoryCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_chart = (ChartImpl) this.FindParent(typeof (ChartImpl));
    if (this.m_chart == null)
      throw new Exception("cannot find parent chart.");
  }

  public ChartCategory this[int index]
  {
    get => (ChartCategory) null;
    set => throw new NotSupportedException();
  }

  public IOfficeChartCategory this[string name]
  {
    get
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        if (this.m_chart.Categories[index].Name == name)
          return this.m_chart.Categories[index];
      }
      return (IOfficeChartCategory) null;
    }
  }

  public IOfficeChartCategory Add(ChartSerieImpl serieToAdd)
  {
    throw new ArgumentException("This method is not supported");
  }

  public void Remove(string serieName)
  {
  }

  public IOfficeChartCategory Add(string name, OfficeChartType serieType)
  {
    return (IOfficeChartCategory) null;
  }

  public IOfficeChartCategory Add(OfficeChartType serieType)
  {
    throw new ArgumentException("This method is not supported");
  }

  public IOfficeChartCategory Add()
  {
    ChartCategory chartCategory = new ChartCategory(this.Application, (object) this);
    this.Add((IOfficeChartCategory) chartCategory);
    return (IOfficeChartCategory) chartCategory;
  }

  protected override void OnClear() => base.OnClear();

  public override object Clone(object parent)
  {
    throw new ArgumentException("This method is not supported");
  }

  public System.Collections.Generic.List<BiffRecordRaw> GetEnteredRecords(int siIndex)
  {
    return (System.Collections.Generic.List<BiffRecordRaw>) null;
  }

  public IOfficeChartCategory Add(string name)
  {
    new ChartSerieImpl(this.Application, (object) this).Name = name;
    if (this.m_chart.HasTitle && this.m_chart.ChartTitle == null)
      this.m_chart.ChartTitle = name;
    return (IOfficeChartCategory) null;
  }

  private System.Collections.Generic.List<System.Collections.Generic.List<BiffRecordRaw>> GetArrays(
    int siIndex)
  {
    return (System.Collections.Generic.List<System.Collections.Generic.List<BiffRecordRaw>>) null;
  }

  public IOfficeChartCategory Add(IRange Categorylabel, IRange Values)
  {
    ChartCategory chartCategory = new ChartCategory(this.Application, (object) this);
    chartCategory.CategoryLabel = (this.m_chart.ChartData as ChartData)[Categorylabel];
    chartCategory.Values = (this.m_chart.ChartData as ChartData)[Values];
    this.Add((IOfficeChartCategory) chartCategory);
    return (IOfficeChartCategory) chartCategory;
  }
}
