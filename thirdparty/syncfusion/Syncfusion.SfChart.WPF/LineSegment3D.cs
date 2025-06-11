// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.LineSegment3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class LineSegment3D : ChartSegment3D
{
  public Polygon3D[] topPolygonCollection;
  public Polygon3D[] bottomPolygonCollection;
  public Polygon3D[] frontPolygonCollection;
  public Polygon3D[] backPolygonCollection;
  public PolygonRecycler polygonRecycler;
  internal static readonly DependencyProperty YProperty = DependencyProperty.Register(nameof (Y), typeof (double), typeof (LineSegment3D), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(LineSegment3D.OnValueChanged)));
  private SfChart3D area;
  private double[] point1;
  private double[] point2;
  private Point intersectingPoint;
  private IList<double> xValues;
  private IList<double> yValues;
  private Brush color;

  public LineSegment3D()
  {
  }

  public LineSegment3D(
    List<double> xValues,
    IList<double> YValues,
    double startDepth,
    double endDepth,
    LineSeries3D lineSeries3D)
  {
    this.Series = (ChartSeriesBase) lineSeries3D;
    this.area = lineSeries3D.Area;
    this.polygonRecycler = new PolygonRecycler();
  }

  public double XData { get; set; }

  public double YData { get; set; }

  internal double Y
  {
    get => (double) this.GetValue(LineSegment3D.YProperty);
    set => this.SetValue(LineSegment3D.YProperty, (object) value);
  }

  public override void OnSizeChanged(Size size)
  {
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
    LineSeries3D series = this.Series as LineSeries3D;
    if (this.area == null || this.xValues.Count == 1 || series.StrokeThickness == 0)
      return;
    double x1 = cartesianTransformer.XAxis.IsLogarithmic ? ((LogarithmicAxis3D) cartesianTransformer.XAxis).LogarithmicBase : 1.0;
    double x2 = cartesianTransformer.YAxis.IsLogarithmic ? ((LogarithmicAxis3D) cartesianTransformer.YAxis).LogarithmicBase : 1.0;
    bool isLogarithmic1 = cartesianTransformer.XAxis.IsLogarithmic;
    bool isLogarithmic2 = cartesianTransformer.YAxis.IsLogarithmic;
    double num1 = isLogarithmic1 ? Math.Pow(x1, cartesianTransformer.XAxis.VisibleRange.Start) : cartesianTransformer.XAxis.VisibleRange.Start;
    double num2 = isLogarithmic1 ? Math.Pow(x1, cartesianTransformer.XAxis.VisibleRange.End) : cartesianTransformer.XAxis.VisibleRange.End;
    double yStart = isLogarithmic2 ? Math.Pow(x2, cartesianTransformer.YAxis.VisibleRange.Start) : cartesianTransformer.YAxis.VisibleRange.Start;
    double yEnd = isLogarithmic2 ? Math.Pow(x2, cartesianTransformer.YAxis.VisibleRange.End) : cartesianTransformer.YAxis.VisibleRange.End;
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
      int num3 = this.xValues.IndexOf(num2);
      while (this.xValues[num3 + 1] > num2)
      {
        this.xValues.RemoveAt(num3 + 1);
        this.yValues.RemoveAt(num3 + 1);
        if (num3 >= this.xValues.Count - 1)
          break;
      }
    }
    IEnumerable<int> source = this.yValues.ToList<double>().Where<double>((Func<double, bool>) (val => val < yStart || val > yEnd)).Select<double, int>((Func<double, int>) (val => this.yValues.IndexOf(val)));
    if (this.yValues.Count - source.Count<int>() < 2)
      return;
    if (source.Count<int>() > 0)
    {
      foreach (int index in source)
        this.yValues[index] = this.yValues[index] >= yStart ? yEnd : yStart;
    }
    double xValue1 = this.xValues[0];
    double y1 = series.IsAnimated || this.Series.EnableAnimation ? (this.Y >= this.yValues[0] || this.Y <= 0.0 ? this.yValues[0] : this.Y) : this.yValues[0];
    Point point = transformer.TransformToVisible(xValue1, y1);
    this.point2 = new double[10];
    this.point1 = new double[10];
    double xValue2 = this.xValues[1];
    double y2 = series.IsAnimated || this.Series.EnableAnimation ? (this.Y >= this.yValues[1] || this.Y <= 0.0 ? this.yValues[1] : this.Y) : this.yValues[1];
    Point visible = transformer.TransformToVisible(xValue2, y2);
    int leftThickness = series.StrokeThickness / 2;
    int rightThickness = series.StrokeThickness % 2 == 0 ? series.StrokeThickness / 2 - 1 : series.StrokeThickness / 2;
    LineSegment3D.GetLinePoints(point.X, point.Y, visible.X, visible.Y, (double) leftThickness, (double) rightThickness, this.point1);
    int num4 = 0;
    this.polygonRecycler.Reset();
    int index1 = 1;
    while (index1 < this.yValues.Count)
    {
      this.point2 = new double[10];
      point = visible;
      this.color = this.Series.SegmentColorPath != null || this.Series.Palette != ChartColorPalette.None ? this.Series.GetInteriorColor(index1 - 1) : this.Interior;
      if (index1 == 1)
      {
        Polygon3D polygon = this.polygonRecycler.DequeuePolygon(new Vector3D[4]
        {
          new Vector3D(this.point1[6], this.point1[7], this.startDepth),
          new Vector3D(this.point1[6], this.point1[7], this.endDepth),
          new Vector3D(this.point1[0], this.point1[1], this.endDepth),
          new Vector3D(this.point1[0], this.point1[1], this.startDepth)
        }, (DependencyObject) this, this.Series.Segments.IndexOf((ChartSegment) this), this.Stroke, this.StrokeThickness, this.color);
        if (series.IsAnimated || !this.Series.EnableAnimation)
          this.area.Graphics3D.AddVisual(polygon);
      }
      ++index1;
      if (index1 < this.xValues.Count)
      {
        double xValue3 = this.xValues[index1];
        double num5 = series.IsAnimated || this.Series.EnableAnimation ? (this.Y >= this.yValues[index1] || this.Y <= 0.0 ? this.yValues[index1] : this.Y) : this.yValues[index1];
        double x3 = xValue3 < num1 ? num1 : (xValue3 > num2 ? num2 : xValue3);
        double y3 = num5 < yStart ? yStart : (num5 > yEnd ? yEnd : num5);
        visible = transformer.TransformToVisible(x3, y3);
        this.UpdatePoints2(point.X, point.Y, visible.X, visible.Y, (double) leftThickness, (double) rightThickness);
      }
      Vector3D[] points1 = new Vector3D[4]
      {
        new Vector3D(this.point1[2], this.point1[3], this.startDepth),
        new Vector3D(this.point1[0], this.point1[1], this.startDepth),
        new Vector3D(this.point1[0], this.point1[1], this.endDepth),
        new Vector3D(this.point1[2], this.point1[3], this.endDepth)
      };
      Vector3D[] points2 = new Vector3D[4]
      {
        new Vector3D(this.point1[6], this.point1[7], this.startDepth),
        new Vector3D(this.point1[4], this.point1[5], this.startDepth),
        new Vector3D(this.point1[4], this.point1[5], this.endDepth),
        new Vector3D(this.point1[6], this.point1[7], this.endDepth)
      };
      Polygon3D polygon1 = new Polygon3D(points1, (DependencyObject) this, this.Series.Segments.IndexOf((ChartSegment) this), this.Stroke, this.StrokeThickness, this.color);
      polygon1.CalcNormal(points1[0], points1[1], points1[2]);
      polygon1.CalcNormal();
      Polygon3D polygon2 = new Polygon3D(points2, (DependencyObject) this, this.Series.Segments.IndexOf((ChartSegment) this), this.Stroke, this.StrokeThickness, this.color);
      polygon2.CalcNormal(points2[0], points2[1], points2[2]);
      polygon2.CalcNormal();
      if (series.IsAnimated || !this.Series.EnableAnimation)
      {
        this.area.Graphics3D.AddVisual(polygon1);
        this.area.Graphics3D.AddVisual(polygon2);
        this.RenderFrontPolygon(this.point1, this.startDepth, this.endDepth, this.color);
      }
      if (this.point2 != null && index1 < this.xValues.Count)
        this.point1 = this.point2;
      ++num4;
    }
    Vector3D[] points = new Vector3D[4]
    {
      new Vector3D(this.point1[4], this.point1[5], this.startDepth),
      new Vector3D(this.point1[4], this.point1[5], this.endDepth),
      new Vector3D(this.point1[2], this.point1[3], this.endDepth),
      new Vector3D(this.point1[2], this.point1[3], this.startDepth)
    };
    Polygon3D polygon3 = new Polygon3D(points, (DependencyObject) this, 0, this.Stroke, 1.0, this.color);
    polygon3.CalcNormal(points[0], points[1], points[2]);
    polygon3.CalcNormal();
    if (!series.IsAnimated && this.Series.EnableAnimation)
      return;
    this.area.Graphics3D.AddVisual(polygon3);
  }

  private static void GetLinePoints(
    double x1,
    double y1,
    double x2,
    double y2,
    double leftThickness,
    double rightThickness,
    double[] points)
  {
    double x = x2 - x1;
    double num1 = Math.Atan2(y2 - y1, x);
    double num2 = Math.Cos(-num1);
    double num3 = Math.Sin(-num1);
    double num4 = x1 * num2 - y1 * num3;
    double num5 = x1 * num3 + y1 * num2;
    double num6 = x2 * num2 - y2 * num3;
    double num7 = x2 * num3 + y2 * num2;
    double num8 = Math.Cos(num1);
    double num9 = Math.Sin(num1);
    double num10 = num4 * num8 - (num5 + leftThickness) * num9;
    double num11 = num4 * num9 + (num5 + leftThickness) * num8;
    double num12 = num6 * num8 - (num7 + leftThickness) * num9;
    double num13 = num6 * num9 + (num7 + leftThickness) * num8;
    double num14 = num4 * num8 - (num5 - rightThickness) * num9;
    double num15 = num4 * num9 + (num5 - rightThickness) * num8;
    double num16 = num6 * num8 - (num7 - rightThickness) * num9;
    double num17 = num6 * num9 + (num7 - rightThickness) * num8;
    points[0] = (double) (int) num10;
    points[1] = (double) (int) num11;
    points[2] = (double) (int) num12;
    points[3] = (double) (int) num13;
    points[4] = (double) (int) num16;
    points[5] = (double) (int) num17;
    points[6] = (double) (int) num14;
    points[7] = (double) (int) num15;
    points[8] = (double) (int) num10;
    points[9] = (double) (int) num11;
  }

  private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
  {
    if (!(d is LineSegment3D lineSegment3D))
      return;
    lineSegment3D.ScheduleRender();
  }

  private void ScheduleRender()
  {
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) new Action(this.OnValueChanged));
  }

  private void OnValueChanged()
  {
    if (this.Series == null || !this.Series.GetAnimationIsActive())
      return;
    (this.Series as LineSeries3D).IsAnimated = true;
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

  private void RenderFrontPolygon(
    double[] point1,
    double startDepth,
    double endDepth,
    Brush color)
  {
    Vector3D[] points1 = new Vector3D[5]
    {
      new Vector3D(point1[0], point1[1], startDepth),
      new Vector3D(point1[2], point1[3], startDepth),
      new Vector3D(point1[4], point1[5], startDepth),
      new Vector3D(point1[6], point1[7], startDepth),
      new Vector3D(point1[8], point1[9], startDepth)
    };
    Vector3D[] points2 = new Vector3D[5]
    {
      new Vector3D(point1[0], point1[1], endDepth),
      new Vector3D(point1[2], point1[3], endDepth),
      new Vector3D(point1[4], point1[5], endDepth),
      new Vector3D(point1[6], point1[7], endDepth),
      new Vector3D(point1[8], point1[9], endDepth)
    };
    Polygon3D polygon1 = new Polygon3D(points1, (DependencyObject) this, 0, this.Stroke, 1.0, color);
    polygon1.CalcNormal(points1[0], points1[1], points1[2]);
    polygon1.CalcNormal();
    this.area.Graphics3D.AddVisual(polygon1);
    Polygon3D polygon2 = new Polygon3D(points2, (DependencyObject) this, 0, this.Stroke, 1.0, color);
    polygon2.CalcNormal(points2[0], points2[1], points2[2]);
    polygon2.CalcNormal();
    this.area.Graphics3D.AddVisual(polygon2);
  }

  private void UpdatePoints2(
    double xStart,
    double yStart,
    double xEnd,
    double yEnd,
    double leftThickness,
    double rightThickness)
  {
    LineSegment3D.GetLinePoints(xStart, yStart, xEnd, yEnd, leftThickness, rightThickness, this.point2);
    if (this.FindIntersectingPoint(this.point1[0], this.point1[1], this.point1[2], this.point1[3], this.point2[0], this.point2[1], this.point2[2], this.point2[3]))
    {
      double num1 = this.intersectingPoint.X - this.point1[2];
      double num2 = this.point2[0] - this.point1[2];
      if (num1 >= 0.0 ? num2 >= num1 || num1 - num2 <= 3.0 : num1 >= -3.0)
      {
        this.point1[2] = this.intersectingPoint.X;
        this.point1[3] = this.intersectingPoint.Y;
        this.point2[0] = this.intersectingPoint.X;
        this.point2[1] = this.intersectingPoint.Y;
        this.point2[8] = this.intersectingPoint.X;
        this.point2[9] = this.intersectingPoint.Y;
      }
    }
    if (!this.FindIntersectingPoint(this.point1[6], this.point1[7], this.point1[4], this.point1[5], this.point2[6], this.point2[7], this.point2[4], this.point2[5]))
      return;
    double num3 = this.intersectingPoint.X - this.point1[4];
    double num4 = this.point2[6] - this.point1[4];
    if (!(num3 >= 0.0 ? num4 >= num3 || num3 - num4 <= 3.0 : num3 >= -3.0))
      return;
    this.point1[4] = this.intersectingPoint.X;
    this.point1[5] = this.intersectingPoint.Y;
    this.point2[6] = this.intersectingPoint.X;
    this.point2[7] = this.intersectingPoint.Y;
  }

  private bool FindIntersectingPoint(
    double x11,
    double y11,
    double x12,
    double y12,
    double x21,
    double y21,
    double x22,
    double y22)
  {
    double num1 = (y22 - y21) * (x12 - x11) - (x22 - x21) * (y12 - y11);
    double num2 = (x22 - x21) * (y11 - y21) - (y22 - y21) * (x11 - x21);
    if (num1 == 0.0 || num1 == 1.0 || num1 == -1.0)
      return false;
    double num3 = num2 / num1;
    this.intersectingPoint = new Point(x11 + num3 * (x12 - x11), y11 + num3 * (y12 - y11));
    return x11 == x12 ? x21 != x22 || x11 == x21 : x21 == x22 || (y11 - y12) / (x11 - x12) != (y21 - y22) / (x21 - x22);
  }
}
