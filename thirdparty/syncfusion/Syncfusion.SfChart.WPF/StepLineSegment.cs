// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.StepLineSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class StepLineSegment : ChartSegment
{
  private Polyline poly;
  private ChartPoint pointStart;
  private ChartPoint pointEnd;
  private ChartPoint stepMidPoint;
  private DataTemplate customTemplate;
  private double _yData;
  private double _y1Data;
  private double _xData;
  private double _x1Data;
  private ContentControl control;
  private PointCollection points;
  private bool isSegmentUpdated;
  private PointCollection stepPoints;
  private double x1;
  private double x2;
  private double y1;
  private double y2;
  private double x3;
  private double y3;

  public StepLineSegment()
  {
  }

  [Obsolete("Use StepLineSegment(ChartPoint point1, ChartPoint stepPoint, ChartPoint point2, StepLineSeries series)")]
  public StepLineSegment(Point point1, Point stepPoint, Point point2, StepLineSeries series)
  {
    this.customTemplate = series.CustomTemplate;
  }

  public StepLineSegment(
    ChartPoint point1,
    ChartPoint stepPoint,
    ChartPoint point2,
    StepLineSeries series)
  {
    this.customTemplate = series.CustomTemplate;
  }

  public double X1
  {
    get => this.x1;
    set
    {
      this.x1 = value;
      this.OnPropertyChanged(nameof (X1));
    }
  }

  public double X2
  {
    get => this.x2;
    set
    {
      this.x2 = value;
      this.OnPropertyChanged(nameof (X2));
    }
  }

  public double Y1
  {
    get => this.y1;
    set
    {
      this.y1 = value;
      this.OnPropertyChanged(nameof (Y1));
    }
  }

  public double Y2
  {
    get => this.y2;
    set
    {
      this.y2 = value;
      this.OnPropertyChanged(nameof (Y2));
    }
  }

  public double X3
  {
    get => this.x3;
    set
    {
      this.x3 = value;
      this.OnPropertyChanged(nameof (X3));
    }
  }

  public double Y3
  {
    get => this.y3;
    set
    {
      this.y3 = value;
      this.OnPropertyChanged(nameof (Y3));
    }
  }

  public double X1Value { get; set; }

  public double Y1Value { get; set; }

  public double X2Value { get; set; }

  public double Y2Value { get; set; }

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

  public double Y1Data
  {
    get => this._y1Data;
    set
    {
      this._y1Data = value;
      this.OnPropertyChanged(nameof (Y1Data));
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

  public PointCollection Points
  {
    get => this.stepPoints;
    set
    {
      this.stepPoints = value;
      this.OnPropertyChanged(nameof (Points));
    }
  }

  [Obsolete("Use SetData(List<ChartPoint> linePoints)")]
  public override void SetData(List<Point> linePoints)
  {
    this.pointStart = new ChartPoint(linePoints[0].X, linePoints[0].Y);
    this.stepMidPoint = new ChartPoint(linePoints[1].X, linePoints[1].Y);
    this.pointEnd = new ChartPoint(linePoints[2].X, linePoints[2].Y);
    this.X1Value = this.pointStart.X;
    this.X2Value = this.pointEnd.X;
    this.Y1Value = this.pointStart.Y;
    this.Y2Value = this.pointEnd.Y;
    this.XData = this.pointStart.X;
    this.X1Data = this.stepMidPoint.X;
    this.YData = this.pointStart.Y;
    this.Y1Data = this.stepMidPoint.Y;
    this.XRange = new DoubleRange(this.pointStart.X, this.pointEnd.X);
    this.YRange = new DoubleRange(this.pointStart.Y, this.stepMidPoint.Y);
  }

  public override void SetData(List<ChartPoint> linePoints)
  {
    this.pointStart = linePoints[0];
    this.stepMidPoint = linePoints[1];
    this.pointEnd = linePoints[2];
    this.X1Value = this.pointStart.X;
    this.X2Value = this.pointEnd.X;
    this.Y1Value = this.pointStart.Y;
    this.Y2Value = this.pointEnd.Y;
    this.XData = this.pointStart.X;
    this.X1Data = this.stepMidPoint.X;
    this.YData = this.pointStart.Y;
    this.Y1Data = this.stepMidPoint.Y;
    this.XRange = new DoubleRange(this.pointStart.X, this.pointEnd.X);
    this.YRange = new DoubleRange(this.pointStart.Y, this.stepMidPoint.Y);
    if (!(this.Series is StepLineSeries series))
      return;
    this.customTemplate = series.CustomTemplate;
  }

  public override UIElement CreateVisual(Size size)
  {
    if (this.customTemplate == null)
    {
      this.poly = new Polyline();
      this.SetVisualBindings((Shape) this.poly);
      this.poly.Fill = (Brush) new SolidColorBrush(Colors.Transparent);
      this.poly.Tag = (object) this;
      this.poly.StrokeEndLineCap = PenLineCap.Round;
      return (UIElement) this.poly;
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
    return this.customTemplate == null ? (UIElement) this.poly : (UIElement) this.control;
  }

  public override void Update(IChartTransformer transformer)
  {
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    if (this.isSegmentUpdated)
      this.Series.SeriesRootPanel.Clip = (Geometry) null;
    double num1 = Math.Floor(cartesianTransformer.XAxis.VisibleRange.Start);
    double num2 = Math.Ceiling(cartesianTransformer.XAxis.VisibleRange.End);
    double newBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
    bool isLogarithmic = cartesianTransformer.XAxis.IsLogarithmic;
    double num3 = isLogarithmic ? Math.Log(this.X1Value, newBase) : this.X1Value;
    double num4 = isLogarithmic ? Math.Log(this.X2Value, newBase) : this.X2Value;
    if (num3 <= num2 && num4 >= num1)
    {
      if (this.poly != null)
        this.poly.Visibility = Visibility.Visible;
      Point visible1 = transformer.TransformToVisible(this.pointStart.X, this.pointStart.Y);
      Point visible2 = transformer.TransformToVisible(this.pointEnd.X, this.pointEnd.Y);
      Point visible3 = transformer.TransformToVisible(this.stepMidPoint.X, this.stepMidPoint.Y);
      this.points = new PointCollection();
      if (this.X1 != visible1.X || this.X2 != visible2.X || this.Y1 != visible1.Y || this.Y2 != visible2.Y || this.X3 != visible3.X || this.Y3 != visible3.Y)
      {
        this.X1 = visible1.X;
        this.X2 = visible2.X;
        this.Y1 = visible1.Y;
        this.Y2 = visible2.Y;
        this.X3 = visible3.X;
        this.Y3 = visible3.Y;
        this.points.Add(visible1);
        this.points.Add(visible2);
        this.points.Add(visible3);
        if (this.poly != null)
          this.poly.Points = this.points;
        else
          this.Points = this.points;
      }
    }
    else if (this.poly != null)
    {
      this.poly.ClearUIValues();
      this.poly.Visibility = Visibility.Collapsed;
    }
    this.isSegmentUpdated = true;
  }

  public override void OnSizeChanged(Size size)
  {
  }

  internal override void Dispose()
  {
    if (this.poly != null)
    {
      this.poly.Tag = (object) null;
      this.poly = (Polyline) null;
    }
    base.Dispose();
  }
}
