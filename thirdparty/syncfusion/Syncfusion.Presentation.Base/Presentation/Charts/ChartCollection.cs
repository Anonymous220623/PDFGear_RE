// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Charts.ChartCollection
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.Charts;

internal class ChartCollection : IPresentationCharts, IEnumerable<IPresentationChart>, IEnumerable
{
  private readonly BaseSlide _baseSlide;

  internal ChartCollection(BaseSlide baseSlide) => this._baseSlide = baseSlide;

  public IPresentationChart this[int index]
  {
    get
    {
      List<IPresentationChart> list = this.GetList();
      return list.Count == 0 ? (IPresentationChart) null : list[index];
    }
  }

  public int Count => this.GetList().Count;

  public IPresentationChart AddChart(double left, double top, double width, double height)
  {
    return this._baseSlide.Shapes.AddChart(left, top, width, height);
  }

  public IPresentationChart AddChart(
    Stream excelStream,
    int sheetNumber,
    string dataRange,
    RectangleF bounds)
  {
    return this._baseSlide.Shapes.AddChart(excelStream, sheetNumber, dataRange, bounds);
  }

  public IPresentationChart AddChart(
    object[][] data,
    double left,
    double top,
    double width,
    double height)
  {
    return this._baseSlide.Shapes.AddChart(data, left, top, width, height);
  }

  public IPresentationChart AddChart(
    IEnumerable enumerable,
    double left,
    double top,
    double width,
    double height)
  {
    return this._baseSlide.Shapes.AddChart(enumerable, left, top, width, height);
  }

  public int IndexOf(IPresentationChart chart) => this.GetList().IndexOf(chart);

  public void RemoveAt(int index) => this.Remove(this.GetPresentationChartList()[index]);

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetList().GetEnumerator();

  public IEnumerator<IPresentationChart> GetEnumerator()
  {
    return (IEnumerator<IPresentationChart>) this.GetList().GetEnumerator();
  }

  internal void Remove(PresentationChart chart)
  {
    ((Shapes) this._baseSlide.Shapes).Remove((ISlideItem) chart);
  }

  internal List<IPresentationChart> GetList()
  {
    List<IPresentationChart> list = new List<IPresentationChart>();
    foreach (IShape shape in (IEnumerable<ISlideItem>) this._baseSlide.Shapes)
    {
      if (shape.SlideItemType == SlideItemType.Chart)
      {
        PresentationChart presentationChart = shape as PresentationChart;
        list.Add((IPresentationChart) presentationChart);
      }
    }
    return list;
  }

  internal List<PresentationChart> GetPresentationChartList()
  {
    List<PresentationChart> presentationChartList = new List<PresentationChart>();
    foreach (IShape shape in (IEnumerable<ISlideItem>) this._baseSlide.Shapes)
    {
      if (shape.SlideItemType == SlideItemType.Chart)
      {
        PresentationChart presentationChart = shape as PresentationChart;
        presentationChartList.Add(presentationChart);
      }
    }
    return presentationChartList;
  }
}
