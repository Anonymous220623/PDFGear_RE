// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.RangeAreaSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class RangeAreaSegment : ChartSegment
{
  private List<ChartPoint> AreaPoints;
  private Path segPath;
  private Brush hiValueInterior;
  private Brush loValueInterior;
  private double _high;
  private double _low;

  internal bool IsHighLow { get; set; }

  public RangeAreaSegment()
  {
  }

  [Obsolete("Use RangeAreaSegment(List<ChartPoint> AreaPoints, bool isHighLow, RangeAreaSeries series)")]
  public RangeAreaSegment(List<Point> AreaPoints, bool isHighLow, RangeAreaSeries series)
  {
    this.IsHighLow = isHighLow;
  }

  public RangeAreaSegment(List<ChartPoint> AreaPoints, bool isHighLow, RangeAreaSeries series)
  {
    this.IsHighLow = isHighLow;
  }

  public Brush ActualInterior => !this.IsHighLow ? this.LowValueInterior : this.HighValueInterior;

  public Brush HighValueInterior
  {
    get => this.hiValueInterior != null ? this.hiValueInterior : this.Interior;
    set
    {
      if (this.hiValueInterior == value)
        return;
      this.hiValueInterior = value;
      this.OnPropertyChanged("ActualInterior");
    }
  }

  public Brush LowValueInterior
  {
    get => this.loValueInterior != null ? this.loValueInterior : this.Interior;
    set
    {
      if (this.loValueInterior == value)
        return;
      this.loValueInterior = value;
      this.OnPropertyChanged("ActualInterior");
    }
  }

  public double High
  {
    get => this._high;
    set
    {
      this._high = value;
      this.OnPropertyChanged(nameof (High));
    }
  }

  public double Low
  {
    get => this._low;
    set
    {
      this._low = value;
      this.OnPropertyChanged(nameof (Low));
    }
  }

  [Obsolete("Use SetData(List<ChartPoint> AreaPoints)")]
  public override void SetData(List<Point> AreaPoints)
  {
    List<ChartPoint> chartPointList = new List<ChartPoint>();
    foreach (Point areaPoint in AreaPoints)
      chartPointList.Add(new ChartPoint(areaPoint.X, areaPoint.Y));
    this.AreaPoints = chartPointList;
    double end1 = AreaPoints.Max<Point>((Func<Point, double>) (x => x.X));
    double end2 = AreaPoints.Max<Point>((Func<Point, double>) (y => y.Y));
    double start1 = AreaPoints.Min<Point>((Func<Point, double>) (x => x.X));
    double d = AreaPoints.Min<Point>((Func<Point, double>) (item => item.Y));
    double start2;
    if (double.IsNaN(d))
    {
      IEnumerable<Point> source = AreaPoints.Where<Point>((Func<Point, bool>) (item => !double.IsNaN(item.Y)));
      start2 = !source.Any<Point>() ? 0.0 : source.Min<Point>((Func<Point, double>) (item => item.Y));
    }
    else
      start2 = d;
    this.XRange = new DoubleRange(start1, end1);
    this.YRange = new DoubleRange(start2, end2);
  }

  public override void SetData(List<ChartPoint> AreaPoints)
  {
    this.AreaPoints = AreaPoints;
    double end1 = AreaPoints.Max<ChartPoint>((Func<ChartPoint, double>) (x => x.X));
    double end2 = AreaPoints.Max<ChartPoint>((Func<ChartPoint, double>) (y => y.Y));
    double start1 = AreaPoints.Min<ChartPoint>((Func<ChartPoint, double>) (x => x.X));
    double d = AreaPoints.Min<ChartPoint>((Func<ChartPoint, double>) (item => item.Y));
    double start2;
    if (double.IsNaN(d))
    {
      IEnumerable<ChartPoint> source = AreaPoints.Where<ChartPoint>((Func<ChartPoint, bool>) (item => !double.IsNaN(item.Y)));
      start2 = !source.Any<ChartPoint>() ? 0.0 : source.Min<ChartPoint>((Func<ChartPoint, double>) (item => item.Y));
    }
    else
      start2 = d;
    this.XRange = new DoubleRange(start1, end1);
    this.YRange = new DoubleRange(start2, end2);
  }

  public override UIElement CreateVisual(Size size)
  {
    this.segPath = new Path();
    this.segPath.Tag = (object) this;
    this.SetVisualBindings((Shape) this.segPath);
    return (UIElement) this.segPath;
  }

  public override UIElement GetRenderedVisual() => (UIElement) this.segPath;

  public override void Update(IChartTransformer transformer)
  {
    PathFigure pathFigure = new PathFigure();
    int num1 = 0;
    int num2 = this.AreaPoints.Count - 1;
    if (this.AreaPoints.Count > 0)
    {
      pathFigure.StartPoint = transformer.TransformToVisible(this.AreaPoints[0].X, this.AreaPoints[0].Y);
      for (int index = num1; index < this.AreaPoints.Count; index += 2)
        pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
        {
          Point = transformer.TransformToVisible(this.AreaPoints[index].X, this.AreaPoints[index].Y)
        });
      for (int index = num2; index >= 1; index -= 2)
        pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
        {
          Point = transformer.TransformToVisible(this.AreaPoints[index].X, this.AreaPoints[index].Y)
        });
      pathFigure.IsClosed = true;
      this.Series.SeriesRootPanel.Clip = (Geometry) null;
    }
    PathGeometry pathGeometry = new PathGeometry();
    pathGeometry.Figures.Add(pathFigure);
    this.segPath.StrokeLineJoin = PenLineJoin.Round;
    this.segPath.Data = (Geometry) pathGeometry;
  }

  public override void OnSizeChanged(Size size)
  {
  }

  protected override void SetVisualBindings(Shape element)
  {
    element.SetBinding(Shape.FillProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("ActualInterior", new object[0])
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

  protected override void OnPropertyChanged(string name)
  {
    if (name == "Interior")
      name = "ActualInterior";
    base.OnPropertyChanged(name);
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
