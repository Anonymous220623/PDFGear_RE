// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SplineSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SplineSegment : ChartSegment
{
  private ChartPoint Point1;
  private ChartPoint Point2;
  private ChartPoint Point3;
  private ChartPoint Point4;
  private Path segPath;
  private ContentControl control;
  private DataTemplate customTemplate;
  private PathGeometry data;
  private double _yData;
  private double _y1Data;
  private double _xData;
  private double _x1Data;
  private Point p1;
  private Point q1;
  private Point q2;
  private Point p2;
  private bool isSegmentUpdated;

  public SplineSegment()
  {
  }

  [Obsolete("Use SplineSegment(ChartPoint point1, ChartPoint point2, ChartPoint point3, ChartPoint point4, SplineSeries series)")]
  public SplineSegment(
    Point point1,
    Point point2,
    Point point3,
    Point point4,
    SplineSeries series)
  {
    this.Series = (ChartSeriesBase) series;
    this.customTemplate = series.CustomTemplate;
  }

  public SplineSegment(
    ChartPoint point1,
    ChartPoint point2,
    ChartPoint point3,
    ChartPoint point4,
    SplineSeries series)
  {
    this.Series = (ChartSeriesBase) series;
    this.customTemplate = series.CustomTemplate;
  }

  [Obsolete("Use SplineSegment(ChartPoint point1, ChartPoint point2, ChartPoint point3, ChartPoint point4, ChartSeriesBase series)")]
  public SplineSegment(
    Point point1,
    Point point2,
    Point point3,
    Point point4,
    ChartSeriesBase series)
  {
  }

  public SplineSegment(
    ChartPoint point1,
    ChartPoint point2,
    ChartPoint point3,
    ChartPoint point4,
    ChartSeriesBase series)
  {
  }

  public double X1 { get; set; }

  public double X2 { get; set; }

  public double Y1 { get; set; }

  public double Y2 { get; set; }

  public Point P1
  {
    get => this.p1;
    set
    {
      this.p1 = value;
      this.OnPropertyChanged(nameof (P1));
    }
  }

  public Point Q1
  {
    get => this.q1;
    set
    {
      this.q1 = value;
      this.OnPropertyChanged(nameof (Q1));
    }
  }

  public Point Q2
  {
    get => this.q2;
    set
    {
      this.q2 = value;
      this.OnPropertyChanged(nameof (Q2));
    }
  }

  public Point P2
  {
    get => this.p2;
    set
    {
      this.p2 = value;
      this.OnPropertyChanged(nameof (P2));
    }
  }

  public double X1Data
  {
    get => this._x1Data;
    set
    {
      this._x1Data = value;
      this.OnPropertyChanged(nameof (X1Data));
    }
  }

  public double XData
  {
    get => this._xData;
    set
    {
      this._xData = value;
      this.OnPropertyChanged(nameof (XData));
    }
  }

  public double YData
  {
    get => this._yData;
    set
    {
      this._yData = value;
      this.OnPropertyChanged(nameof (YData));
    }
  }

  public double Y1Data
  {
    get => this._y1Data;
    set
    {
      this._y1Data = value;
      this.OnPropertyChanged(nameof (Y1Data));
    }
  }

  public PathGeometry Data
  {
    get => this.data;
    set
    {
      this.data = value;
      this.OnPropertyChanged(nameof (Data));
    }
  }

  [Obsolete("Use SetData(ChartPoint point1, ChartPoint point2, ChartPoint point3, ChartPoint point4)")]
  public override void SetData(Point point1, Point point2, Point point3, Point point4)
  {
    this.XData = point1.X;
    this.X1Data = point4.X;
    this.YData = point1.Y;
    this.Y1Data = point4.Y;
    this.Point1 = new ChartPoint(point1.X, point1.Y);
    this.Point2 = new ChartPoint(point2.X, point2.Y);
    this.Point3 = new ChartPoint(point3.X, point2.Y);
    this.Point4 = new ChartPoint(point4.X, point4.Y);
    this.XRange = new DoubleRange(point1.X, point4.X);
    this.YRange = this.GetRange(point1.Y, point2.Y, point3.Y, point4.Y);
  }

  public override void SetData(
    ChartPoint point1,
    ChartPoint point2,
    ChartPoint point3,
    ChartPoint point4)
  {
    this.XData = point1.X;
    this.X1Data = point4.X;
    this.YData = point1.Y;
    this.Y1Data = point4.Y;
    this.Point1 = point1;
    this.Point2 = point2;
    this.Point3 = point3;
    this.Point4 = point4;
    this.XRange = new DoubleRange(point1.X, point4.X);
    this.YRange = this.GetRange(point1.Y, point2.Y, point3.Y, point4.Y);
    if (!(this.Series is SplineSeries series))
      return;
    this.customTemplate = series.CustomTemplate;
  }

  public override UIElement CreateVisual(Size size)
  {
    if (this.customTemplate == null)
    {
      this.segPath = new Path();
      this.segPath.Tag = (object) this;
      this.SetVisualBindings((Shape) this.segPath);
      return (UIElement) this.segPath;
    }
    ContentControl contentControl = new ContentControl();
    contentControl.Content = (object) this;
    contentControl.Tag = (object) this;
    contentControl.ContentTemplate = this.customTemplate;
    this.control = contentControl;
    return (UIElement) this.control;
  }

  public override UIElement GetRenderedVisual()
  {
    return this.customTemplate == null ? (UIElement) this.segPath : (UIElement) this.control;
  }

  public override void Update(IChartTransformer transformer)
  {
    if (transformer == null)
      return;
    if (this.isSegmentUpdated)
      this.Series.SeriesRootPanel.Clip = (Geometry) null;
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    double newBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
    bool isLogarithmic = cartesianTransformer.XAxis.IsLogarithmic;
    double num1 = Math.Floor(cartesianTransformer.XAxis.VisibleRange.Start);
    double num2 = Math.Ceiling(cartesianTransformer.XAxis.VisibleRange.End);
    double num3 = isLogarithmic ? Math.Log(this.Point1.X, newBase) : this.Point1.X;
    double num4 = isLogarithmic ? Math.Log(this.Point4.X, newBase) : this.Point4.X;
    if (num3 <= num2 && num4 >= num1)
    {
      PathFigure pathFigure = new PathFigure();
      BezierSegment bezierSegment = new BezierSegment();
      PathGeometry pathGeometry = new PathGeometry();
      this.P1 = pathFigure.StartPoint = transformer.TransformToVisible(this.Point1.X, this.Point1.Y);
      this.Q1 = bezierSegment.Point1 = transformer.TransformToVisible(this.Point2.X, this.Point2.Y);
      this.Q2 = bezierSegment.Point2 = transformer.TransformToVisible(this.Point3.X, this.Point3.Y);
      this.P2 = bezierSegment.Point3 = transformer.TransformToVisible(this.Point4.X, this.Point4.Y);
      pathFigure.Segments.Add((PathSegment) bezierSegment);
      pathGeometry.Figures = new PathFigureCollection()
      {
        pathFigure
      };
      Path segPath = this.segPath;
      if (segPath != null)
        segPath.Data = (Geometry) pathGeometry;
      else
        this.Data = pathGeometry;
    }
    else if (this.segPath != null)
      this.segPath.Data = (Geometry) null;
    else if (this.Data != null)
      this.Data = (PathGeometry) null;
    this.isSegmentUpdated = true;
  }

  public override void OnSizeChanged(Size size)
  {
  }

  protected internal DoubleRange GetRange(double y1, double y2, double y3, double y4)
  {
    DoubleRange range = DoubleRange.Union(y1, y2, y3, y4);
    double c = 3.0 * (y2 - y1);
    double num1 = 3.0 * (y3 - y3) - c;
    double num2 = y4 - y1 - num1 - c;
    double root1;
    double root2;
    if (ChartMath.SolveQuadraticEquation(3.0 * num2, 2.0 * num1, c, out root1, out root2))
    {
      if (root1 >= 0.0 && root1 <= 1.0)
      {
        double num3 = num2 * root1 * root1 * root1 + num1 * root1 * root1 + c * root1 + y1;
        range = DoubleRange.Union(range, num3);
      }
      if (root2 >= 0.0 && root2 <= 1.0)
      {
        double num4 = num2 * root2 * root2 * root2 + num1 * root2 * root2 + c * root2 + y1;
        range = DoubleRange.Union(range, num4);
      }
    }
    return range;
  }

  protected override void SetVisualBindings(Shape element)
  {
    element.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Interior", new object[0])
    });
    element.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeThickness", new object[0])
    });
    if (this is TrendlineSegment)
    {
      DoubleCollection strokeDashArray = this.StrokeDashArray;
      if (strokeDashArray == null)
        return;
      DoubleCollection doubleCollection = new DoubleCollection();
      foreach (double num in strokeDashArray)
        doubleCollection.Add(num);
      element.StrokeDashArray = doubleCollection;
    }
    else
      BindingOperations.SetBinding((DependencyObject) this, ChartSegment.StrokeDashArrayProperty, (BindingBase) new Binding()
      {
        Source = (object) this.Series,
        Path = new PropertyPath("StrokeDashArray", new object[0])
      });
  }

  internal override void Dispose()
  {
    if (this.segPath != null)
    {
      this.segPath.Tag = (object) null;
      this.segPath = (Path) null;
    }
    base.Dispose();
  }
}
