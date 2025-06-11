// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.WorksheetChartsCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Shapes;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class WorksheetChartsCollection : 
  CollectionBaseEx<object>,
  IOfficeChartShapes,
  IEnumerable,
  IParentApplication
{
  private WorksheetBaseImpl m_sheet;

  public WorksheetChartsCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  public IOfficeChart AddChart()
  {
    IOfficeChart officeChart = (IOfficeChart) this.m_sheet.Shapes.AddChart();
    this.InnerList.Add((object) officeChart);
    return officeChart;
  }

  protected internal IOfficeChartShape InnerAddChart(IOfficeChartShape chart)
  {
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    this.InnerList.Add((object) chart);
    return chart;
  }

  protected internal IOfficeChartShape AddChart(IOfficeChartShape chart)
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

  public IOfficeChartShape this[int index]
  {
    get
    {
      if (index < 0 || index >= this.InnerList.Count)
        throw new ArgumentOutOfRangeException("Chart index");
      return this.InnerList[index] as IOfficeChartShape;
    }
  }

  public IOfficeChartShape Add() => this.m_sheet.Shapes.AddChart();

  public new void RemoveAt(int index)
  {
    IOfficeChartShape officeChartShape = index >= 0 && index < this.Count ? this.InnerList[index] as IOfficeChartShape : throw new ArgumentOutOfRangeException(nameof (index));
    this.InnerList.RemoveAt(index);
    officeChartShape.Remove();
  }
}
