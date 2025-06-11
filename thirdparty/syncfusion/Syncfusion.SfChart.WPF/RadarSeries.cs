// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.RadarSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class RadarSeries : PolarRadarSeriesBase
{
  public override void CreateSegments()
  {
    this.Segments.Clear();
    this.Segment = (ChartSegment) null;
    if (this.DrawType == ChartSeriesDrawType.Area)
    {
      double num = this.ActualXAxis != null ? this.ActualXAxis.Origin : 0.0;
      List<double> list1 = this.GetXValues().ToList<double>();
      List<double> doubleList = new List<double>();
      List<double> list2 = this.YValues.Select<double, double>((Func<double, double>) (val => val)).ToList<double>();
      if (list1 == null)
        return;
      if (!this.IsClosed)
      {
        list1.Insert(this.DataCount - 1, list1[this.DataCount - 1]);
        list1.Insert(0, list1[0]);
        list2.Insert(0, num);
        list2.Insert(list2.Count, num);
      }
      else
      {
        list1.Insert(0, list1[0]);
        list2.Insert(0, this.YValues[0]);
        list1.Insert(0, list1[this.DataCount]);
        list2.Insert(0, this.YValues[this.DataCount - 1]);
      }
      if (this.Segment == null)
      {
        this.Segment = (ChartSegment) (this.CreateSegment() as AreaSegment);
        if (this.Segment != null)
        {
          this.Segment.Series = (ChartSeriesBase) this;
          this.Segment.SetData((IList<double>) list1, (IList<double>) list2);
          this.Segments.Add(this.Segment);
        }
      }
      else
        this.Segment.SetData((IList<double>) list1, (IList<double>) list2);
      if (this.AdornmentsInfo == null)
        return;
      this.AddAreaAdornments(this.YValues);
    }
    else
    {
      if (this.DrawType != ChartSeriesDrawType.Line)
        return;
      double xIndexValues = 0.0;
      List<double> doubleList = this.ActualXValues as List<double>;
      if (this.IsIndexed || doubleList == null)
        doubleList = doubleList != null ? doubleList.Select<double, double>((Func<double, double>) (val => xIndexValues++)).ToList<double>() : (this.ActualXValues as List<string>).Select<string, double>((Func<string, double>) (val => xIndexValues++)).ToList<double>();
      if (doubleList == null)
        return;
      int index1;
      for (index1 = 0; index1 < this.DataCount; ++index1)
      {
        int index2 = index1 + 1;
        if (index2 < this.DataCount)
        {
          if (index1 < this.Segments.Count)
            this.Segments[index1].SetData(doubleList[index1], this.YValues[index1], doubleList[index2], this.YValues[index2]);
          else
            this.CreateSegment(new double[4]
            {
              doubleList[index1],
              this.YValues[index1],
              doubleList[index2],
              this.YValues[index2]
            });
        }
        if (this.AdornmentsInfo != null)
        {
          if (index1 < this.Adornments.Count)
            this.Adornments[index1].SetData(doubleList[index1], this.YValues[index1], doubleList[index1], this.YValues[index1]);
          else
            this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, doubleList[index1], this.YValues[index1], doubleList[index1], this.YValues[index1]));
          this.Adornments[index1].Item = this.ActualData[index1];
        }
      }
      if (this.IsClosed)
        this.CreateSegment(new double[4]
        {
          doubleList[0],
          this.YValues[0],
          doubleList[index1 - 1],
          this.YValues[index1 - 1]
        });
      if (!this.ShowEmptyPoints)
        return;
      this.UpdateEmptyPointSegments(doubleList, false);
    }
  }

  private void CreateSegment(double[] values)
  {
    if (!(this.CreateSegment() is LineSegment segment))
      return;
    segment.Series = (ChartSeriesBase) this;
    segment.Item = (object) this;
    segment.SetData(values);
    this.Segment = (ChartSegment) segment;
    this.Segments.Add((ChartSegment) segment);
  }

  protected internal override IChartTransformer CreateTransformer(Size size, bool create)
  {
    if (create || this.ChartTransformer == null)
      this.ChartTransformer = ChartTransform.CreatePolar(new Rect(new Point(), size), (ChartSeriesBase) this);
    return this.ChartTransformer;
  }

  protected override ChartSegment CreateSegment()
  {
    return this.DrawType == ChartSeriesDrawType.Area ? (ChartSegment) new AreaSegment() : (ChartSegment) new LineSegment();
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new RadarSeries());
  }
}
