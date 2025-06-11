// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.AreaSegment3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class AreaSegment3D : ChartSegment3D
{
  internal static readonly DependencyProperty YProperty = DependencyProperty.Register(nameof (Y), typeof (double), typeof (AreaSegment3D), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(AreaSegment3D.OnValueChanged)));
  private Polygon3D frontPolygon;
  private Polygon3D bottomPolygon;
  private Polygon3D rightPolygon;
  private Polygon3D leftPolygon;
  private Polygon3D backPolygon;
  private Polygon3D[] topPolygonCollection;
  private SfChart3D area;
  private IList<double> xValues;
  private IList<double> yValues;

  public AreaSegment3D()
  {
  }

  public AreaSegment3D(
    List<double> xValues,
    IList<double> YValues,
    double startDepth,
    double endDepth,
    AreaSeries3D areaSeries3D)
  {
    this.Series = (ChartSeriesBase) areaSeries3D;
    this.area = areaSeries3D.Area;
  }

  public double XData { get; set; }

  public double YData { get; set; }

  internal double Y
  {
    get => (double) this.GetValue(AreaSegment3D.YProperty);
    set => this.SetValue(AreaSegment3D.YProperty, (object) value);
  }

  public override void SetData(
    List<double> xValues,
    IList<double> YValues,
    double startDepth,
    double endDepth)
  {
    this.xValues = (IList<double>) xValues.ToList<double>();
    this.yValues = (IList<double>) YValues.ToList<double>();
    this.startDepth = startDepth;
    this.endDepth = endDepth;
    this.XRange = new DoubleRange(xValues.Min(), xValues.Max());
    this.YRange = new DoubleRange(YValues.Min(), YValues.Max());
  }

  public override UIElement CreateVisual(Size size) => (UIElement) null;

  public override UIElement GetRenderedVisual() => (UIElement) null;

  public override void Update(IChartTransformer transformer)
  {
    if (!(transformer is ChartTransform.ChartCartesianTransformer cartesianTransformer))
      return;
    double x1 = cartesianTransformer.XAxis.IsLogarithmic ? ((LogarithmicAxis3D) cartesianTransformer.XAxis).LogarithmicBase : 1.0;
    double x2 = cartesianTransformer.YAxis.IsLogarithmic ? ((LogarithmicAxis3D) cartesianTransformer.YAxis).LogarithmicBase : 1.0;
    bool isLogarithmic1 = cartesianTransformer.XAxis.IsLogarithmic;
    bool isLogarithmic2 = cartesianTransformer.YAxis.IsLogarithmic;
    double num1 = isLogarithmic1 ? Math.Pow(x1, cartesianTransformer.XAxis.VisibleRange.Start) : cartesianTransformer.XAxis.VisibleRange.Start;
    double num2 = isLogarithmic1 ? Math.Pow(x1, cartesianTransformer.XAxis.VisibleRange.End) : cartesianTransformer.XAxis.VisibleRange.End;
    double num3 = isLogarithmic2 ? Math.Pow(x2, cartesianTransformer.YAxis.VisibleRange.Start) : cartesianTransformer.YAxis.VisibleRange.Start;
    double num4 = isLogarithmic2 ? Math.Pow(x2, cartesianTransformer.YAxis.VisibleRange.End) : cartesianTransformer.YAxis.VisibleRange.End;
    if (this.xValues.Min() < num1)
    {
      while (this.xValues[0] < num1)
      {
        this.xValues.RemoveAt(0);
        this.yValues.RemoveAt(0);
      }
    }
    if (this.xValues.Max() > num2)
    {
      int num5 = this.xValues.IndexOf(num2);
      while (this.xValues[num5 + 1] > num2)
      {
        this.xValues.RemoveAt(num5 + 1);
        this.yValues.RemoveAt(num5 + 1);
        if (num5 >= this.xValues.Count - 1)
          break;
      }
    }
    if (this.yValues.Min() < num3)
    {
      foreach (double num6 in this.yValues.ToList<double>())
      {
        if (num6 < num3)
          this.yValues[this.yValues.IndexOf(num6)] = num3;
      }
    }
    if (this.yValues.Max() > num4)
    {
      foreach (double num7 in this.yValues.ToList<double>())
      {
        if (num7 > num4)
          this.yValues[this.yValues.IndexOf(num7)] = num4;
      }
    }
    double xValue1 = this.xValues[0];
    double y1 = (this.Series as AreaSeries3D).IsAnimated || this.Series.EnableAnimation ? (this.Y >= this.yValues[0] || this.Y <= 0.0 ? this.yValues[0] : this.Y) : this.yValues[0];
    Point point1 = transformer.TransformToVisible(xValue1, y1);
    List<Point> pointList = new List<Point>();
    pointList.Add(point1);
    this.topPolygonCollection = new Polygon3D[this.yValues.Count];
    for (int index = 1; index < this.xValues.Count; ++index)
    {
      double xValue2 = this.xValues[index];
      double num8 = (this.Series as AreaSeries3D).IsAnimated || this.Series.EnableAnimation ? (this.Y >= this.yValues[index] || this.Y <= 0.0 ? this.yValues[index] : this.Y) : this.yValues[index];
      double x3 = xValue2 < num1 ? num1 : (xValue2 > num2 ? num2 : xValue2);
      double y2 = num8 < num3 ? num3 : (num8 > num4 ? num4 : num8);
      Point visible = transformer.TransformToVisible(x3, y2);
      Vector3D[] points = new Vector3D[4]
      {
        new Vector3D(point1.X, point1.Y, this.startDepth),
        new Vector3D(visible.X, visible.Y, this.startDepth),
        new Vector3D(visible.X, visible.Y, this.endDepth),
        new Vector3D(point1.X, point1.Y, this.endDepth)
      };
      this.topPolygonCollection[index] = new Polygon3D(points, (DependencyObject) this, this.Series.Segments.IndexOf((ChartSegment) this), this.Stroke, this.StrokeThickness, this.Interior);
      this.topPolygonCollection[index].CalcNormal(points[0], points[1], points[2]);
      this.topPolygonCollection[index].CalcNormal();
      point1 = visible;
      pointList.Add(visible);
    }
    double xValue3 = this.xValues[0];
    double y3 = (this.Series as AreaSeries3D).IsAnimated || this.Series.EnableAnimation ? (this.Y >= this.yValues[0] || this.Y <= 0.0 ? this.yValues[0] : this.Y) : this.yValues[0];
    Point visible1 = transformer.TransformToVisible(xValue3, y3);
    double xValue4 = this.xValues[this.xValues.Count - 1];
    double y4 = (this.Series as AreaSeries3D).IsAnimated || this.Series.EnableAnimation ? (this.Y < this.yValues[this.yValues.Count - 1] ? this.Y : this.yValues[this.yValues.Count - 1]) : this.yValues[this.xValues.Count - 1];
    Point visible2 = transformer.TransformToVisible(xValue4, y4);
    double y5 = this.Series.ActualYAxis.Origin < num3 ? num3 : this.Series.ActualYAxis.Origin;
    double xValue5 = this.xValues[0];
    Point visible3 = transformer.TransformToVisible(xValue5, y5);
    double xValue6 = this.xValues[this.xValues.Count - 1];
    Point visible4 = transformer.TransformToVisible(xValue6, y5);
    pointList.Add(visible4);
    pointList.Add(visible3);
    Vector3D vector3D1 = new Vector3D(visible1.X, visible1.Y, this.startDepth);
    Vector3D vector3D2 = new Vector3D(visible1.X, visible1.Y, this.endDepth);
    Vector3D vector3D3 = new Vector3D(visible2.X, visible2.Y, this.startDepth);
    Vector3D vector3D4 = new Vector3D(visible2.X, visible2.Y, this.endDepth);
    Vector3D vector3D5 = new Vector3D(visible3.X, visible3.Y, this.startDepth);
    Vector3D vector3D6 = new Vector3D(visible3.X, visible3.Y, this.endDepth);
    Vector3D vector3D7 = new Vector3D(visible4.X, visible4.Y, this.startDepth);
    Vector3D vector3D8 = new Vector3D(visible4.X, visible4.Y, this.endDepth);
    Vector3D[] points1 = new Vector3D[4]
    {
      vector3D1,
      vector3D2,
      vector3D6,
      vector3D5
    };
    Vector3D[] points2 = new Vector3D[4]
    {
      vector3D3,
      vector3D4,
      vector3D8,
      vector3D7
    };
    Vector3D[] points3 = new Vector3D[4]
    {
      vector3D5,
      vector3D6,
      vector3D8,
      vector3D7
    };
    Vector3D[] points4 = new Vector3D[pointList.Count];
    Vector3D[] points5 = new Vector3D[pointList.Count];
    for (int index = 0; index < pointList.Count; ++index)
    {
      Point point2 = pointList[index];
      points4[index] = new Vector3D(point2.X, point2.Y, this.startDepth);
      points5[index] = new Vector3D(point2.X, point2.Y, this.endDepth);
    }
    if (!(this.Series as AreaSeries3D).IsAnimated && this.Series.EnableAnimation)
      return;
    this.leftPolygon = new Polygon3D(points1, (DependencyObject) this, this.Series.Segments.IndexOf((ChartSegment) this), this.Stroke, this.StrokeThickness, this.Interior);
    this.leftPolygon.CalcNormal(points1[0], points1[1], points1[2]);
    this.leftPolygon.CalcNormal();
    this.area.Graphics3D.AddVisual(this.leftPolygon);
    this.rightPolygon = new Polygon3D(points2, (DependencyObject) this, this.Series.Segments.IndexOf((ChartSegment) this), this.Stroke, this.StrokeThickness, this.Interior);
    this.rightPolygon.CalcNormal(points2[0], points2[1], points2[2]);
    this.rightPolygon.CalcNormal();
    this.area.Graphics3D.AddVisual(this.rightPolygon);
    this.bottomPolygon = new Polygon3D(points3, (DependencyObject) this, this.Series.Segments.IndexOf((ChartSegment) this), this.Stroke, this.StrokeThickness, this.Interior);
    this.bottomPolygon.CalcNormal(points3[0], points3[1], points3[2]);
    this.bottomPolygon.CalcNormal();
    this.area.Graphics3D.AddVisual(this.bottomPolygon);
    this.frontPolygon = new Polygon3D(points4, (DependencyObject) this, this.Series.Segments.IndexOf((ChartSegment) this), this.Stroke, this.StrokeThickness, this.Interior);
    this.frontPolygon.CalcNormal(points4[0], points4[1], points4[2]);
    this.frontPolygon.CalcNormal();
    this.area.Graphics3D.AddVisual(this.frontPolygon);
    this.backPolygon = new Polygon3D(points5, (DependencyObject) this, this.Series.Segments.IndexOf((ChartSegment) this), this.Stroke, this.StrokeThickness, this.Interior);
    this.backPolygon.CalcNormal(points5[0], points5[1], points5[2]);
    this.backPolygon.CalcNormal();
    this.area.Graphics3D.AddVisual(this.backPolygon);
    for (int index = 1; index < this.yValues.Count; ++index)
      this.area.Graphics3D.AddVisual(this.topPolygonCollection[index]);
  }

  public override void OnSizeChanged(Size size)
  {
  }

  private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
  {
    if (!(d is AreaSegment3D areaSegment3D))
      return;
    areaSegment3D.ScheduleRender();
  }

  private void ScheduleRender()
  {
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) new Action(this.OnValueChanged));
  }

  private void OnValueChanged()
  {
    if (this.Series == null || !this.Series.GetAnimationIsActive())
      return;
    (this.Series as AreaSeries3D).IsAnimated = true;
    List<Polygon3D> visual = this.area.Graphics3D.GetVisual();
    foreach (Polygon3D polygon3D in visual.Where<Polygon3D>((Func<Polygon3D, bool>) (item => item.Tag == this)).ToList<Polygon3D>())
      visual.Remove(polygon3D);
    this.Update(this.Series.CreateTransformer(new Size(), true));
    if (this.Series.adornmentInfo != null)
    {
      foreach (UIElement3D polygon in this.area.Graphics3D.GetVisual().OfType<UIElement3D>().ToList<UIElement3D>())
      {
        this.area.Graphics3D.Remove((Polygon3D) polygon);
        this.area.Graphics3D.AddVisual((Polygon3D) polygon);
      }
    }
    this.ScheduleView();
  }

  private void ScheduleView()
  {
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) new Action(this.OnViewChanged));
  }

  private void OnViewChanged()
  {
    this.area.Graphics3D.PrepareView();
    this.area.Graphics3D.View((Panel) this.area.RootPanel);
  }
}
