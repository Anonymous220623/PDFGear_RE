// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.PieSegment
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

public class PieSegment : ChartSegment
{
  public static readonly DependencyProperty IsExplodedProperty = DependencyProperty.Register(nameof (IsExploded), typeof (bool), typeof (PieSegment), new PropertyMetadata((object) false, new PropertyChangedCallback(PieSegment.OnIsExplodedChanged)));
  public static readonly DependencyProperty ActualStartAngleProperty = DependencyProperty.Register(nameof (ActualStartAngle), typeof (double), typeof (PieSegment), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(PieSegment.OnAngleChanged)));
  public static readonly DependencyProperty ActualEndAngleProperty = DependencyProperty.Register(nameof (ActualEndAngle), typeof (double), typeof (PieSegment), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(PieSegment.OnAngleChanged)));
  private double startAngle;
  private PieSeries parentSeries;
  private Path segmentPath;
  internal Point startPoint;
  private bool isInitializing = true;
  private PathGeometry segmentGeometry;
  private int pieSeriesCount;
  private int pieIndex;
  private double angleOfSlice;
  private double xData;
  private double yData;
  private double endAngle;

  public PieSegment()
  {
  }

  public PieSegment(double arcStartAngle, double arcEndAngle, PieSeries series, object item)
  {
  }

  internal void SetData(
    double arcStartAngle,
    double arcEndAngle,
    PieSeries pieSeries,
    object item)
  {
    this.Series = (ChartSeriesBase) pieSeries;
    this.StartAngle = arcStartAngle;
    this.EndAngle = arcEndAngle;
    this.parentSeries = pieSeries;
    this.pieSeriesCount = pieSeries.GetPieSeriesCount();
    this.pieIndex = this.GetPieSeriesIndex((ChartSeriesBase) pieSeries);
    this.Item = item;
    this.isInitializing = false;
  }

  public PieSegment(
    double arcStartAngle,
    double arcEndAngle,
    bool isEmptyInterior,
    PieSeries series,
    object item)
    : this(arcStartAngle, arcEndAngle, series, item)
  {
    this.IsEmptySegmentInterior = isEmptyInterior;
  }

  public bool IsExploded
  {
    get => (bool) this.GetValue(PieSegment.IsExplodedProperty);
    set => this.SetValue(PieSegment.IsExplodedProperty, (object) value);
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

  public double ActualStartAngle
  {
    get => (double) this.GetValue(PieSegment.ActualStartAngleProperty);
    set => this.SetValue(PieSegment.ActualStartAngleProperty, (object) value);
  }

  public double ActualEndAngle
  {
    get => (double) this.GetValue(PieSegment.ActualEndAngleProperty);
    set => this.SetValue(PieSegment.ActualEndAngleProperty, (object) value);
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

  public override UIElement CreateVisual(Size size)
  {
    this.segmentPath = new Path();
    this.SetVisualBindings((Shape) this.segmentPath);
    this.segmentPath.Tag = (object) this;
    return (UIElement) this.segmentPath;
  }

  public override UIElement GetRenderedVisual() => (UIElement) this.segmentPath;

  public override void Update(IChartTransformer transformer)
  {
    if (!this.IsSegmentVisible)
      this.segmentPath.Visibility = Visibility.Collapsed;
    else
      this.segmentPath.Visibility = Visibility.Visible;
    this.segmentPath.StrokeLineJoin = PenLineJoin.Round;
    this.segmentPath.Width = transformer.Viewport.Width;
    this.segmentPath.Height = transformer.Viewport.Height;
    this.segmentPath.VerticalAlignment = VerticalAlignment.Center;
    this.segmentPath.HorizontalAlignment = HorizontalAlignment.Center;
    double num1 = Math.Min(transformer.Viewport.Width, transformer.Viewport.Height) / 2.0 / (double) this.pieSeriesCount;
    if (this.pieIndex == 0)
    {
      Point point1 = this.pieSeriesCount != 1 ? ChartLayoutUtils.GetCenter(transformer.Viewport) : (this.Series as CircularSeriesBase).Center;
      double num2 = this.parentSeries.InternalPieCoefficient * num1;
      this.parentSeries.Radius = num2;
      if (Math.Round(this.ActualEndAngle - this.ActualStartAngle, 2) == 6.28)
        this.segmentPath.Data = (Geometry) new EllipseGeometry()
        {
          Center = point1,
          RadiusX = num2,
          RadiusY = num2
        };
      else if (this.ActualEndAngle - this.ActualStartAngle != 0.0)
      {
        if (this.IsExploded)
          point1 = new Point(point1.X + this.parentSeries.ExplodeRadius * Math.Cos(this.AngleOfSlice), point1.Y + this.parentSeries.ExplodeRadius * Math.Sin(this.AngleOfSlice));
        this.startPoint = new Point(point1.X + num2 * Math.Cos(this.ActualStartAngle), point1.Y + num2 * Math.Sin(this.ActualStartAngle));
        Point point2 = new Point(point1.X + num2 * Math.Cos(this.ActualEndAngle), point1.Y + num2 * Math.Sin(this.ActualEndAngle));
        PathFigure pathFigure = new PathFigure();
        pathFigure.StartPoint = point1;
        pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
        {
          Point = this.startPoint
        });
        pathFigure.Segments.Add((PathSegment) new ArcSegment()
        {
          Point = point2,
          Size = new Size(num2, num2),
          RotationAngle = ChartMath.ParseAtInvarientCulture(this.ActualEndAngle + this.ActualStartAngle),
          IsLargeArc = (this.ActualEndAngle - this.ActualStartAngle > Math.PI),
          SweepDirection = (this.StartAngle > this.EndAngle ? SweepDirection.Counterclockwise : SweepDirection.Clockwise)
        });
        pathFigure.IsClosed = true;
        this.segmentGeometry = new PathGeometry();
        this.segmentGeometry.Figures = new PathFigureCollection()
        {
          pathFigure
        };
        this.segmentPath.Data = (Geometry) this.segmentGeometry;
      }
      else
        this.segmentPath.Data = (Geometry) null;
    }
    else
    {
      if (this.pieIndex < 1)
        return;
      double num3 = num1 * (double) (this.pieIndex + 1) - num1 * (1.0 - this.parentSeries.InternalPieCoefficient);
      double num4 = num1 * (double) this.pieIndex;
      this.parentSeries.Radius = num3;
      Point point3 = ChartLayoutUtils.GetCenter(transformer.Viewport);
      if (this.IsExploded)
        point3 = new Point(point3.X + this.parentSeries.ExplodeRadius * Math.Cos(this.AngleOfSlice), point3.Y + this.parentSeries.ExplodeRadius * Math.Sin(this.AngleOfSlice));
      this.startPoint = new Point(point3.X + num3 * Math.Cos(this.ActualStartAngle), point3.Y + num3 * Math.Sin(this.ActualStartAngle));
      Point point4 = new Point(point3.X + num3 * Math.Cos(this.ActualEndAngle), point3.Y + num3 * Math.Sin(this.ActualEndAngle));
      if (Math.Round(this.ActualEndAngle - this.ActualStartAngle, 2) == 6.28)
        this.segmentPath.Data = (Geometry) new GeometryGroup()
        {
          Children = {
            (Geometry) new EllipseGeometry()
            {
              Center = point3,
              RadiusX = num3,
              RadiusY = num3
            },
            (Geometry) new EllipseGeometry()
            {
              Center = point3,
              RadiusX = num4,
              RadiusY = num4
            }
          }
        };
      else if (this.ActualEndAngle - this.ActualStartAngle != 0.0)
      {
        Point point5 = new Point(point3.X + num4 * Math.Cos(this.ActualStartAngle), point3.Y + num4 * Math.Sin(this.ActualStartAngle));
        Point point6 = new Point(point3.X + num4 * Math.Cos(this.ActualEndAngle), point3.Y + num4 * Math.Sin(this.ActualEndAngle));
        PathFigure pathFigure = new PathFigure();
        pathFigure.StartPoint = this.startPoint;
        pathFigure.Segments.Add((PathSegment) new ArcSegment()
        {
          Point = point4,
          Size = new Size(num3, num3),
          RotationAngle = ChartMath.ParseAtInvarientCulture(this.ActualEndAngle - this.ActualStartAngle),
          IsLargeArc = (this.ActualEndAngle - this.ActualStartAngle > Math.PI),
          SweepDirection = (this.StartAngle > this.EndAngle ? SweepDirection.Counterclockwise : SweepDirection.Clockwise)
        });
        pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
        {
          Point = point6
        });
        pathFigure.Segments.Add((PathSegment) new ArcSegment()
        {
          Point = point5,
          Size = new Size(num4, num4),
          RotationAngle = ChartMath.ParseAtInvarientCulture(this.ActualEndAngle - this.ActualStartAngle),
          IsLargeArc = (this.ActualEndAngle - this.ActualStartAngle > Math.PI),
          SweepDirection = (this.StartAngle < this.EndAngle ? SweepDirection.Counterclockwise : SweepDirection.Clockwise)
        });
        pathFigure.IsClosed = true;
        this.segmentGeometry = new PathGeometry();
        this.segmentGeometry.Figures = new PathFigureCollection()
        {
          pathFigure
        };
        this.segmentPath.Data = (Geometry) this.segmentGeometry;
      }
      else
        this.segmentPath.Data = (Geometry) null;
    }
  }

  public override void OnSizeChanged(Size size)
  {
  }

  internal bool IsPointInPieSegment(double x, double y)
  {
    Canvas adorningCanvas = this.Series.ActualArea.GetAdorningCanvas();
    Size size = new Size(adorningCanvas.ActualWidth, adorningCanvas.ActualHeight);
    Point point = new Point(size.Width * 0.5, size.Height * 0.5);
    double num1 = Math.Min(point.X, point.Y) * (this.Series as PieSeries).InternalPieCoefficient;
    double x1 = x - point.X;
    double num2 = y - point.Y;
    if (Math.Sqrt(Math.Pow(x1, 2.0) + Math.Pow(num2, 2.0)) < num1)
    {
      double num3 = Math.Atan2(num2, x1);
      double num4 = 6.283;
      double startAngle = this.StartAngle;
      double endAngle = this.EndAngle;
      if (num3 < 0.0)
        num3 = (num3 / (Math.PI / 180.0) + 360.0) * Math.PI / 180.0;
      if (this.StartAngle > 0.0 && endAngle > num4 && num3 < this.StartAngle)
        num3 += num4;
      if (num3 > startAngle && num3 < endAngle)
        return true;
    }
    return false;
  }

  internal int GetPieSeriesIndex(ChartSeriesBase currentSeries)
  {
    ChartVisibleSeriesCollection visibleSeries = this.parentSeries.Area.VisibleSeries;
    Func<ChartSeriesBase, bool> predicate = (Func<ChartSeriesBase, bool>) (series => series is PieSeries);
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

  private static void OnIsExplodedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as PieSegment).OnPropertyChanged("IsExploded");
  }

  private static void OnAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as PieSegment).OnAngleChanged(e);
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
