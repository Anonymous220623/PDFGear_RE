// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.WorksheetChartsCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class WorksheetChartsCollection : 
  CollectionBaseEx<object>,
  IChartShapes,
  IEnumerable,
  IParentApplication
{
  private WorksheetBaseImpl m_sheet;

  public WorksheetChartsCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  public IChart AddChart()
  {
    IChart chart = (IChart) this.m_sheet.Shapes.AddChart();
    this.InnerList.Add((object) chart);
    return chart;
  }

  protected internal IChartShape InnerAddChart(IChartShape chart)
  {
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    this.InnerList.Add((object) chart);
    return chart;
  }

  protected internal IChartShape AddChart(IChartShape chart)
  {
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    this.InnerAddChart(chart);
    this.m_sheet.InnerShapes.AddShape((ShapeImpl) chart);
    return chart;
  }

  private void SetParents()
  {
    this.m_sheet = this.FindParent(typeof (WorksheetBaseImpl), true) as WorksheetBaseImpl;
    if (this.m_sheet == null)
      throw new ArgumentNullException("Can't find parent worksheet.");
  }

  public IChartShape this[int index]
  {
    get
    {
      if (index < 0 || index >= this.InnerList.Count)
        throw new ArgumentOutOfRangeException("Chart index");
      return this.InnerList[index] as IChartShape;
    }
  }

  public IChartShape Add() => this.m_sheet.Shapes.AddChart();

  public new void RemoveAt(int index)
  {
    IChartShape chartShape = index >= 0 && index < this.Count ? this.InnerList[index] as IChartShape : throw new ArgumentOutOfRangeException(nameof (index));
    this.InnerList.RemoveAt(index);
    chartShape.Remove();
  }
}
