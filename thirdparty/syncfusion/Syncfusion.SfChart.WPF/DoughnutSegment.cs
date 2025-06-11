// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.DoughnutSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class DoughnutSegment : ChartSegment
{
  private const double CurveDepth = 1.5;
  public static readonly DependencyProperty TrackBorderWidthProperty = DependencyProperty.Register(nameof (TrackBorderWidth), typeof (double), typeof (DoughnutSegment), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty TrackBorderColorProperty = DependencyProperty.Register(nameof (TrackBorderColor), typeof (Brush), typeof (DoughnutSegment), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ActualStartAngleProperty = DependencyProperty.Register(nameof (ActualStartAngle), typeof (double), typeof (DoughnutSegment), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(DoughnutSegment.OnAngleChanged)));
  public static readonly DependencyProperty ActualEndAngleProperty = DependencyProperty.Register(nameof (ActualEndAngle), typeof (double), typeof (DoughnutSegment), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(DoughnutSegment.OnAngleChanged)));
  public static readonly DependencyProperty IsExplodedProperty = DependencyProperty.Register(nameof (IsExploded), typeof (bool), typeof (DoughnutSegment), new PropertyMetadata((object) false, new PropertyChangedCallback(DoughnutSegment.OnIsExplodedChaned)));
  internal Point startPoint;
  private double startAngle;
  private double endAngle;
  private Path segmentPath;
  private double xData;
  private double yData;
  private double angleOfSlice;
  private DoughnutSeries parentSeries;
  private int doughnutSeriesIndex;
  private int doughnutSeriesCount;
  private int doughnutSegmentsCount;
  private bool isInitializing = true;
  private Geometry pathGeometry;
  private Geometry circularPathGeometry;
  private double trackOpacity;
  private Brush trackColor;

  public DoughnutSegment()
  {
  }

  public DoughnutSegment(double startAngle, double endAngle, DoughnutSeries series)
  {
  }

  public double TrackBorderWidth
  {
    get => (double) this.GetValue(DoughnutSegment.TrackBorderWidthProperty);
    set => this.SetValue(DoughnutSegment.TrackBorderWidthProperty, (object) value);
  }

  public Brush TrackBorderColor
  {
    get => (Brush) this.GetValue(DoughnutSegment.TrackBorderColorProperty);
    set => this.SetValue(DoughnutSegment.TrackBorderColorProperty, (object) value);
  }

  public double TrackOpacity
  {
    get => this.trackOpacity;
    set
    {
      this.trackOpacity = value;
      this.OnPropertyChanged(nameof (TrackOpacity));
    }
  }

  public Brush TrackColor
  {
    get => this.trackColor;
    set
    {
      this.trackColor = value;
      this.OnPropertyChanged(nameof (TrackColor));
    }
  }

  public double ActualStartAngle
  {
    get => (double) this.GetValue(DoughnutSegment.ActualStartAngleProperty);
    set => this.SetValue(DoughnutSegment.ActualStartAngleProperty, (object) value);
  }

  public double ActualEndAngle
  {
    get => (double) this.GetValue(DoughnutSegment.ActualEndAngleProperty);
    set => this.SetValue(DoughnutSegment.ActualEndAngleProperty, (object) value);
  }

  public bool IsExploded
  {
    get => (bool) this.GetValue(DoughnutSegment.IsExplodedProperty);
    set => this.SetValue(DoughnutSegment.IsExplodedProperty, (object) value);
  }

  public double StartAngle
  {
    get => this.startAngle;
    internal set
    {
      this.startAngle = value;
      if (this.Series != null && !this.Series.CanAnimate)
        this.ActualStartAngle = value;
      this.OnPropertyChanged(nameof (StartAngle));
    }
  }

  public double EndAngle
  {
    get => this.endAngle;
    internal set
    {
      this.endAngle = value;
      if (this.Series != null && !this.Series.CanAnimate)
        this.ActualEndAngle = value;
      this.OnPropertyChanged(nameof (EndAngle));
    }
  }

  public double AngleOfSlice
  {
    get => this.angleOfSlice;
    internal set
    {
      this.angleOfSlice = value;
      this.OnPropertyChanged(nameof (AngleOfSlice));
    }
  }

  public double XData
  {
    get => this.xData;
    internal set
    {
      this.xData = value;
      this.OnPropertyChanged(nameof (XData));
    }
  }

  public double YData
  {
    get => this.yData;
    internal set
    {
      this.yData = value;
      this.OnPropertyChanged(nameof (YData));
    }
  }

  internal Path CircularDoughnutPath { get; set; }

  private bool IsMultipleCircleDoughnut
  {
    get => this.doughnutSeriesCount == 1 && this.parentSeries.IsStackedDoughnut;
  }

  internal bool IsEndValueExceed { get; set; }

  internal int DoughnutSegmentIndex { get; set; }

  public override UIElement CreateVisual(Size size)
  {
    this.CircularDoughnutPath = new Path();
    this.CircularDoughnutPath.SetBinding(Shape.FillProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("TrackColor", new object[0])
    });
    this.CircularDoughnutPath.SetBinding(UIElement.OpacityProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("TrackOpacity", new object[0])
    });
    this.CircularDoughnutPath.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("TrackBorderColor", new object[0])
    });
    this.CircularDoughnutPath.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("TrackBorderWidth", new object[0])
    });
    this.segmentPath = new Path();
    this.SetVisualBindings((Shape) this.segmentPath);
    this.segmentPath.Tag = (object) this;
    return (UIElement) this.segmentPath;
  }

  public override UIElement GetRenderedVisual() => (UIElement) this.segmentPath;

  public override void Update(IChartTransformer transformer)
  {
    if (this.Series == null || this.Series.ActualArea == null)
      return;
    if (!this.IsSegmentVisible)
      this.segmentPath.Visibility = Visibility.Collapsed;
    else
      this.segmentPath.Visibility = Visibility.Visible;
    if (this.IsMultipleCircleDoughnut)
      this.DrawMultipleDoughnut(transformer);
    else
      this.DrawSingleDoughnut(transformer);
  }

  public override void OnSizeChanged(Size size)
  {
  }

  internal void UpdateTrackInterior(int segmentIndex)
  {
    if (this.parentSeries.TrackColor != null)
    {
      this.TrackColor = this.parentSeries.TrackColor;
      this.TrackOpacity = 1.0;
    }
    else
    {
      this.TrackColor = ChartExtensionUtils.GetInterior((ChartSeriesBase) this.parentSeries, segmentIndex);
      this.TrackOpacity = 0.2;
    }
  }

  internal void SetData(double arcStartAngle, double arcEndAngle, DoughnutSeries doughnutSeries)
  {
    this.Series = (ChartSeriesBase) doughnutSeries;
    this.StartAngle = arcStartAngle;
    this.EndAngle = arcEndAngle;
    this.parentSeries = doughnutSeries;
    this.doughnutSeriesCount = doughnutSeries.GetDoughnutSeriesCount();
    this.doughnutSeriesIndex = this.GetDoughnutSeriesIndex((ChartSeriesBase) doughnutSeries);
    this.doughnutSegmentsCount = doughnutSeries.DataCount;
    this.isInitializing = false;
  }

  internal bool IsPointInDoughnutSegment(double x, double y)
  {
    Canvas adorningCanvas = this.Series.ActualArea.GetAdorningCanvas();
    Size size = new Size(adorningCanvas.ActualWidth, adorningCanvas.ActualHeight);
    Point point = new Point(size.Width * 0.5, size.Height * 0.5);
    double num1 = Math.Min(point.X, point.Y) * 0.8;
    double num2 = num1 * (this.Series as DoughnutSeries).InternalDoughnutCoefficient;
    double x1 = x - point.X;
    double num3 = y - point.Y;
    double num4 = Math.Sqrt(Math.Pow(x1, 2.0) + Math.Pow(num3, 2.0));
    if (num4 < num1 && num4 > num2)
    {
      double num5 = Math.Atan2(num3, x1);
      double num6 = 6.283;
      double startAngle = this.StartAngle;
      double endAngle = this.EndAngle;
      if (num5 < 0.0)
        num5 = (num5 / (Math.PI / 180.0) + 360.0) * Math.PI / 180.0;
      if (this.StartAngle > 0.0 && endAngle > num6 && num5 < this.StartAngle)
        num5 += num6;
      if (num5 > startAngle && num5 < endAngle)
        return true;
    }
    return false;
  }

  internal int GetDoughnutSeriesIndex(ChartSeriesBase currentSeries)
  {
    ChartVisibleSeriesCollection visibleSeries = this.parentSeries.Area.VisibleSeries;
    Func<ChartSeriesBase, bool> predicate = (Func<ChartSeriesBase, bool>) (series => series is DoughnutSeries);
    int num;
    return (num = visibleSeries.Where<ChartSeriesBase>(predicate).ToList<ChartSeriesBase>().IndexOf(currentSeries)) < 0 ? -1 : num;
  }

  protected override void SetVisualBindings(Shape element)
  {
    element.SetBinding(Shape.FillProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Interior", new object[0])
    });
    element.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Stroke", new object[0])
    });
    element.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeThickness", new object[0])
    });
  }

  private static void OnAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as DoughnutSegment).OnAngleChanged(e);
  }

  private static void OnIsExplodedChaned(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as DoughnutSegment).OnPropertyChanged("IsExploded");
  }

  private static GeometryGroup GetEllipseGeometry(Point center, double radius, double innerRadius)
  {
    return new GeometryGroup()
    {
      FillRule = FillRule.EvenOdd,
      Children = {
        (Geometry) new EllipseGeometry()
        {
          Center = center,
          RadiusX = radius,
          RadiusY = radius
        },
        (Geometry) new EllipseGeometry()
        {
          Center = center,
          RadiusX = innerRadius,
          RadiusY = innerRadius
        }
      }
    };
  }

  private static void SuppressAngleForSmallDifference(
    ref double outerSegmentStartAngle,
    ref double outerSegmentEndAngle,
    ref double innerSegmentStartAngle,
    ref double innerSegmentEndAngle,
    double segmentStartAngle,
    double segmentEndAngle,
    bool isClockWise)
  {
    if (isClockWise)
    {
      double num = segmentStartAngle + (segmentEndAngle - segmentStartAngle) / 2.0;
      if (outerSegmentStartAngle > num)
        outerSegmentStartAngle = num;
      if (innerSegmentStartAngle > num)
        innerSegmentStartAngle = num;
      if (outerSegmentEndAngle < num)
        outerSegmentEndAngle = num;
      if (innerSegmentEndAngle >= num)
        return;
      innerSegmentEndAngle = num;
    }
    else
    {
      double num = segmentEndAngle + (segmentStartAngle - segmentEndAngle) / 2.0;
      if (outerSegmentStartAngle < num)
        outerSegmentStartAngle = num;
      if (innerSegmentStartAngle < num)
        innerSegmentStartAngle = num;
      if (outerSegmentEndAngle > num)
        outerSegmentEndAngle = num;
      if (innerSegmentEndAngle <= num)
        return;
      innerSegmentEndAngle = num;
    }
  }

  private void DrawSingleDoughnut(IChartTransformer transformer)
  {
    double actualStartAngle = this.ActualStartAngle;
    double actualEndAngle = this.ActualEndAngle;
    Point center = this.doughnutSeriesCount <= 1 ? this.parentSeries.Center : new Point(0.5 * transformer.Viewport.Width, 0.5 * transformer.Viewport.Height);
    this.CalculateGapRatioAngle(ref actualEndAngle, ref actualStartAngle);
    double num1 = this.parentSeries.InternalDoughnutSize * Math.Min(transformer.Viewport.Width, transformer.Viewport.Height) / 2.0;
    double num2 = (num1 - num1 * this.Series.ActualArea.InternalDoughnutHoleSize) / (double) this.doughnutSeriesCount;
    double radius = this.parentSeries.Radius = num1 - num2 * (double) (this.doughnutSeriesCount - (this.doughnutSeriesIndex + 1));
    double innerRadius = ChartMath.MaxZero(radius - num2 * this.parentSeries.InternalDoughnutCoefficient);
    if (this.parentSeries.Segments.IndexOf((ChartSegment) this) == 0)
      this.parentSeries.InnerRadius = innerRadius;
    this.pathGeometry = this.GetDoughnutGeometry(center, radius, innerRadius, actualStartAngle, actualEndAngle, false);
    this.segmentPath.Data = this.pathGeometry;
    this.circularPathGeometry = (Geometry) null;
    this.CircularDoughnutPath.Data = (Geometry) null;
  }

  private void DrawMultipleDoughnut(IChartTransformer transformer)
  {
    if (this.IsSegmentVisible)
    {
      double actualStartAngle = this.ActualStartAngle;
      double actualEndAngle = this.ActualEndAngle;
      Point center = this.parentSeries.Center;
      double num1 = this.parentSeries.InternalDoughnutSize * Math.Min(transformer.Viewport.Width, transformer.Viewport.Height) / 2.0;
      double num2 = (num1 - num1 * this.Series.ActualArea.InternalDoughnutHoleSize) / (double) this.doughnutSegmentsCount * this.parentSeries.InternalDoughnutCoefficient;
      double num3 = num1 - num2 * (double) (this.doughnutSegmentsCount - (this.DoughnutSegmentIndex + 1));
      this.parentSeries.Radius = num1 - num2;
      double num4 = num3 - num2;
      double radius = num3 - num2 * this.parentSeries.SegmentSpacing;
      double innerRadius = ChartMath.MaxZero(num4);
      if (this.parentSeries.Segments.IndexOf((ChartSegment) this) == 0)
        this.parentSeries.InnerRadius = innerRadius;
      this.pathGeometry = this.GetDoughnutGeometry(center, radius, innerRadius, actualStartAngle, actualEndAngle, false);
      this.segmentPath.Data = this.pathGeometry;
      double radianConverter1 = this.parentSeries.DegreeToRadianConverter(this.parentSeries.StartAngle);
      double radianConverter2 = this.parentSeries.DegreeToRadianConverter(this.parentSeries.EndAngle);
      double num5 = 2.0 * Math.PI;
      double num6 = radianConverter2 - radianConverter1;
      if (Math.Abs(Math.Round(num6, 2)) > num5)
        num6 %= num5;
      double segmentEndAngle = num6 + radianConverter1;
      this.circularPathGeometry = this.GetDoughnutGeometry(center, radius, innerRadius, radianConverter1, segmentEndAngle, true);
      this.CircularDoughnutPath.Data = this.circularPathGeometry;
    }
    else
    {
      this.pathGeometry = (Geometry) null;
      this.segmentPath.Data = (Geometry) null;
      this.circularPathGeometry = (Geometry) null;
      this.CircularDoughnutPath.Data = (Geometry) null;
    }
  }

  private Geometry GetDoughnutGeometry(
    Point center,
    double radius,
    double innerRadius,
    double segmentStartAngle,
    double segmentEndAngle,
    bool isUnfilledPath)
  {
    return this.parentSeries.CapStyle != DoughnutCapStyle.BothFlat && !isUnfilledPath && !this.IsEndValueExceed || Math.Round(segmentEndAngle - segmentStartAngle, 2) != 6.28 ? (Geometry) this.GetArcGeometry(center, radius, innerRadius, segmentStartAngle, segmentEndAngle, isUnfilledPath) : (Geometry) DoughnutSegment.GetEllipseGeometry(center, radius, innerRadius);
  }

  private PathGeometry GetArcGeometry(
    Point center,
    double radius,
    double innerRadius,
    double segmentStartAngle,
    double segmentEndAngle,
    bool isUnfilledPath)
  {
    if (this.IsExploded)
      center = new Point(center.X + this.parentSeries.ExplodeRadius * Math.Cos(this.AngleOfSlice), center.Y + this.parentSeries.ExplodeRadius * Math.Sin(this.AngleOfSlice));
    bool isClockwise = segmentEndAngle > segmentStartAngle;
    double outerSegmentStartAngle = segmentStartAngle;
    double outerSegmentEndAngle = segmentEndAngle;
    double innerSegmentStartAngle = segmentStartAngle;
    double innerSegmentEndAngle = segmentEndAngle;
    double num = 0.0;
    if (this.parentSeries.CapStyle != DoughnutCapStyle.BothFlat && !isUnfilledPath)
    {
      double segmentRadius = (radius - innerRadius) / 2.0;
      num = radius - segmentRadius;
      this.UpdateSegmentAngleForCurvePosition(ref outerSegmentStartAngle, ref outerSegmentEndAngle, ref innerSegmentStartAngle, ref innerSegmentEndAngle, segmentStartAngle, segmentEndAngle, radius, innerRadius, segmentRadius, isClockwise);
    }
    this.startPoint = new Point(center.X + radius * Math.Cos(outerSegmentStartAngle), center.Y + radius * Math.Sin(outerSegmentStartAngle));
    Point point1 = new Point(center.X + radius * Math.Cos(outerSegmentEndAngle), center.Y + radius * Math.Sin(outerSegmentEndAngle));
    Point point2 = new Point(center.X + innerRadius * Math.Cos(innerSegmentStartAngle), center.Y + innerRadius * Math.Sin(innerSegmentStartAngle));
    Point point3 = new Point(center.X + innerRadius * Math.Cos(innerSegmentEndAngle), center.Y + innerRadius * Math.Sin(innerSegmentEndAngle));
    bool flag1 = outerSegmentEndAngle > outerSegmentStartAngle;
    bool flag2 = innerSegmentEndAngle > innerSegmentStartAngle;
    PathFigure pathFigure = new PathFigure();
    pathFigure.StartPoint = this.startPoint;
    pathFigure.Segments.Add((PathSegment) new ArcSegment()
    {
      Point = point1,
      Size = new Size(radius, radius),
      RotationAngle = ChartMath.ParseAtInvarientCulture(outerSegmentEndAngle - outerSegmentStartAngle),
      IsLargeArc = ((!flag1 ? outerSegmentStartAngle - outerSegmentEndAngle : outerSegmentEndAngle - outerSegmentStartAngle) > Math.PI),
      SweepDirection = (!flag1 ? SweepDirection.Counterclockwise : SweepDirection.Clockwise)
    });
    if ((this.parentSeries.CapStyle == DoughnutCapStyle.EndCurve || this.parentSeries.CapStyle == DoughnutCapStyle.BothCurve) && !isUnfilledPath)
    {
      Point point4 = new Point(center.X + num * Math.Cos(segmentEndAngle), center.Y + num * Math.Sin(segmentEndAngle));
      Point point5 = new Point(center.X + radius * Math.Cos(segmentEndAngle), center.Y + radius * Math.Sin(segmentEndAngle));
      QuadraticBezierSegment quadraticBezierSegment1 = new QuadraticBezierSegment()
      {
        Point1 = point5,
        Point2 = point4
      };
      pathFigure.Segments.Add((PathSegment) quadraticBezierSegment1);
      Point point6 = new Point(center.X + innerRadius * Math.Cos(segmentEndAngle), center.Y + innerRadius * Math.Sin(segmentEndAngle));
      QuadraticBezierSegment quadraticBezierSegment2 = new QuadraticBezierSegment()
      {
        Point1 = point6,
        Point2 = point3
      };
      pathFigure.Segments.Add((PathSegment) quadraticBezierSegment2);
    }
    else
    {
      System.Windows.Media.LineSegment lineSegment = new System.Windows.Media.LineSegment()
      {
        Point = point3
      };
      pathFigure.Segments.Add((PathSegment) lineSegment);
    }
    pathFigure.Segments.Add((PathSegment) new ArcSegment()
    {
      Point = point2,
      Size = new Size(innerRadius, innerRadius),
      RotationAngle = ChartMath.ParseAtInvarientCulture(innerSegmentEndAngle - innerSegmentStartAngle),
      IsLargeArc = ((!flag2 ? innerSegmentStartAngle - innerSegmentEndAngle : innerSegmentEndAngle - innerSegmentStartAngle) > Math.PI),
      SweepDirection = (flag2 ? SweepDirection.Counterclockwise : SweepDirection.Clockwise)
    });
    if ((this.parentSeries.CapStyle == DoughnutCapStyle.StartCurve || this.parentSeries.CapStyle == DoughnutCapStyle.BothCurve) && !isUnfilledPath)
    {
      Point point7 = new Point(center.X + num * Math.Cos(segmentStartAngle), center.Y + num * Math.Sin(segmentStartAngle));
      Point point8 = new Point(center.X + innerRadius * Math.Cos(segmentStartAngle), center.Y + innerRadius * Math.Sin(segmentStartAngle));
      QuadraticBezierSegment quadraticBezierSegment3 = new QuadraticBezierSegment()
      {
        Point1 = point8,
        Point2 = point7
      };
      pathFigure.Segments.Add((PathSegment) quadraticBezierSegment3);
      Point point9 = new Point(center.X + radius * Math.Cos(segmentStartAngle), center.Y + radius * Math.Sin(segmentStartAngle));
      QuadraticBezierSegment quadraticBezierSegment4 = new QuadraticBezierSegment()
      {
        Point1 = point9,
        Point2 = this.startPoint
      };
      pathFigure.Segments.Add((PathSegment) quadraticBezierSegment4);
    }
    pathFigure.IsClosed = true;
    return new PathGeometry()
    {
      Figures = new PathFigureCollection() { pathFigure }
    };
  }

  private void CalculateGapRatioAngle(ref double segmentEndAngle, ref double segmentStartAngle)
  {
    if (this.parentSeries.SegmentSpacing == 0.0)
      return;
    if (segmentEndAngle > segmentStartAngle)
    {
      segmentEndAngle -= this.parentSeries.SegmentGapAngle;
      segmentStartAngle += this.parentSeries.SegmentGapAngle;
      if (segmentEndAngle >= segmentStartAngle)
        return;
      segmentStartAngle = segmentEndAngle = 0.0;
    }
    else
    {
      if (segmentEndAngle >= segmentStartAngle)
        return;
      segmentEndAngle += this.parentSeries.SegmentGapAngle;
      segmentStartAngle -= this.parentSeries.SegmentGapAngle;
      if (segmentEndAngle <= segmentStartAngle)
        return;
      segmentStartAngle = segmentEndAngle = 0.0;
    }
  }

  private void UpdateSegmentAngleForCurvePosition(
    ref double outerSegmentStartAngle,
    ref double outerSegmentEndAngle,
    ref double innerSegmentStartAngle,
    ref double innerSegmentEndAngle,
    double segmentStartAngle,
    double segmentEndAngle,
    double radius,
    double innerRadius,
    double segmentRadius,
    bool isClockwise)
  {
    if (radius == 0.0)
      return;
    if (this.parentSeries.CapStyle != DoughnutCapStyle.EndCurve)
    {
      outerSegmentStartAngle = isClockwise ? segmentStartAngle + segmentRadius * 1.5 / radius : segmentStartAngle - segmentRadius * 1.5 / radius;
      innerSegmentStartAngle = isClockwise ? segmentStartAngle + segmentRadius * 1.5 / innerRadius : segmentStartAngle - segmentRadius * 1.5 / innerRadius;
    }
    if (this.parentSeries.CapStyle != DoughnutCapStyle.StartCurve)
    {
      outerSegmentEndAngle = !isClockwise ? segmentEndAngle + segmentRadius * 1.5 / radius : segmentEndAngle - segmentRadius * 1.5 / radius;
      innerSegmentEndAngle = !isClockwise ? segmentEndAngle + segmentRadius * 1.5 / innerRadius : segmentEndAngle - segmentRadius * 1.5 / innerRadius;
    }
    DoughnutSegment.SuppressAngleForSmallDifference(ref outerSegmentStartAngle, ref outerSegmentEndAngle, ref innerSegmentStartAngle, ref innerSegmentEndAngle, segmentStartAngle, segmentEndAngle, isClockwise);
  }

  private void OnAngleChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.Series == null || this.isInitializing)
      return;
    this.Update(this.Series.CreateTransformer(new Size(), false));
    if (this.Series.adornmentInfo == null || this.StartAngle != this.ActualStartAngle || this.EndAngle != this.ActualEndAngle)
      return;
    this.Series.adornmentInfo.Arrange(this.Series.adornmentInfo.AdornmentInfoSize);
  }

  internal override void Dispose()
  {
    if (this.segmentPath != null)
    {
      this.segmentPath.Tag = (object) null;
      this.segmentPath = (Path) null;
    }
    base.Dispose();
  }
}
