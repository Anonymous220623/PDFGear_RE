// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartCategoryCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartCategoryCollection : 
  CollectionBaseEx<IChartCategory>,
  IChartCategories,
  IParentApplication,
  ICloneParent,
  IList<IChartCategory>,
  ICollection<IChartCategory>,
  IEnumerable<IChartCategory>,
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
    get => this.InnerList[index] as ChartCategory;
    set => throw new NotSupportedException();
  }

  public IChartCategory this[string name]
  {
    get
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        if (this.m_chart.Categories[index].Name == name)
          return this.m_chart.Categories[index];
      }
      return (IChartCategory) null;
    }
  }

  public IChartCategory Add(ChartSerieImpl serieToAdd)
  {
    throw new ArgumentException("This method is not supported");
  }

  public void Remove(string serieName)
  {
  }

  public IChartCategory Add(string name, ExcelChartType serieType) => (IChartCategory) null;

  public IChartCategory Add(ExcelChartType serieType)
  {
    throw new ArgumentException("This method is not supported");
  }

  public IChartCategory Add()
  {
    ChartCategory chartCategory = new ChartCategory(this.Application, (object) this);
    this.Add((IChartCategory) chartCategory);
    return (IChartCategory) chartCategory;
  }

  protected override void OnClear() => base.OnClear();

  public override object Clone(object parent)
  {
    ChartCategoryCollection categoryCollection = new ChartCategoryCollection(this.Application, parent);
    categoryCollection.SetParent(parent);
    for (int index = 0; index < this.Count; ++index)
    {
      ChartCategory chartCategory = (ChartCategory) this[index].Clone();
      categoryCollection.Add((IChartCategory) chartCategory);
    }
    return (object) categoryCollection;
  }

  public System.Collections.Generic.List<BiffRecordRaw> GetEnteredRecords(int siIndex)
  {
    return (System.Collections.Generic.List<BiffRecordRaw>) null;
  }

  public IChartCategory Add(string name)
  {
    new ChartSerieImpl(this.Application, (object) this).Name = name;
    if (this.m_chart.ChartTitleArea.Text == null)
      this.m_chart.ChartTitle = name;
    return (IChartCategory) null;
  }

  private System.Collections.Generic.List<System.Collections.Generic.List<BiffRecordRaw>> GetArrays(
    int siIndex)
  {
    return (System.Collections.Generic.List<System.Collections.Generic.List<BiffRecordRaw>>) null;
  }

  public IChartCategory Add(IRange Categorylabel, IRange Values)
  {
    ChartCategory chartCategory = new ChartCategory(this.Application, (object) this);
    chartCategory.CategoryLabel = Categorylabel;
    chartCategory.Values = Values;
    this.Add((IChartCategory) chartCategory);
    return (IChartCategory) chartCategory;
  }
}
