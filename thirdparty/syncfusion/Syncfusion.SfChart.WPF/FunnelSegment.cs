// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FunnelSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class FunnelSegment : ChartSegment
{
  internal double height;
  internal double top;
  private double bottom;
  private double explodedOffset;
  private double minimumWidth;
  private double topRadius;
  private double bottomRadius;
  private Path segmentPath;
  private PathGeometry segmentGeometry;

  public FunnelSegment()
  {
  }

  public FunnelSegment(double y, double height, FunnelSeries funnelSeries, bool isExploded)
  {
    this.Series = (ChartSeriesBase) funnelSeries;
    this.top = y;
    this.bottom = y + height;
    this.topRadius = y / 2.0;
    this.bottomRadius = (y + height) / 2.0;
    this.minimumWidth = funnelSeries.MinWidth;
    this.explodedOffset = funnelSeries.ExplodeOffset;
    this.IsExploded = isExploded;
  }

  public FunnelSegment(
    double y,
    double height,
    double widthTop,
    double widthBottom,
    FunnelSeries funnelSeries,
    bool isExploded)
  {
    this.Series = (ChartSeriesBase) funnelSeries;
    this.top = y;
    this.bottom = y + height;
    this.topRadius = (1.0 - widthTop) / 2.0;
    this.bottomRadius = (1.0 - widthBottom) / 2.0;
    this.minimumWidth = funnelSeries.MinWidth;
    this.explodedOffset = funnelSeries.ExplodeOffset;
    this.IsExploded = isExploded;
  }

  public bool IsExploded { get; set; }

  public double XData { get; set; }

  public double YData { get; set; }

  public override void SetData(params double[] Values)
  {
    this.top = Values[0];
    this.bottom = Values[1];
    this.topRadius = Values[2];
    this.bottomRadius = Values[3];
    this.minimumWidth = Values[4];
    this.explodedOffset = Values[5];
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
    Rect rect = new Rect(0.0, 0.0, transformer.Viewport.Width, transformer.Viewport.Height);
    if (rect.IsEmpty)
    {
      this.segmentPath.Data = (Geometry) null;
    }
    else
    {
      if (this.IsExploded)
        rect.X = this.explodedOffset;
      PathFigure pathFigure = new PathFigure();
      if (!(this.Series is FunnelSeries series))
        return;
      double val2 = 0.5 * (1.0 - this.minimumWidth / rect.Width);
      bool flag = this.topRadius >= val2 ^ this.bottomRadius > val2 && series.FunnelMode == ChartFunnelMode.ValueIsHeight;
      double num = val2 * (this.bottom - this.top) / (this.bottomRadius - this.topRadius);
      this.topRadius = Math.Min(this.topRadius, val2);
      this.bottomRadius = Math.Min(this.bottomRadius, val2);
      pathFigure.StartPoint = new Point(rect.X + this.topRadius * rect.Width, rect.Y + this.top * rect.Height);
      pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
      {
        Point = new Point(rect.X + (1.0 - this.topRadius) * rect.Width, rect.Y + this.top * rect.Height)
      });
      if (flag)
        pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
        {
          Point = new Point(rect.X + (1.0 - val2) * rect.Width, rect.Y + num * rect.Height)
        });
      pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
      {
        Point = new Point(rect.X + (1.0 - this.bottomRadius) * rect.Width, rect.Y + this.bottom * rect.Height - series.StrokeThickness / 2.0)
      });
      pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
      {
        Point = new Point(rect.X + this.bottomRadius * rect.Width, rect.Y + this.bottom * rect.Height - series.StrokeThickness / 2.0)
      });
      if (flag)
        pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
        {
          Point = new Point(rect.X + val2 * rect.Width, rect.Y + num * rect.Height)
        });
      pathFigure.IsClosed = true;
      this.segmentGeometry = new PathGeometry();
      this.segmentGeometry.Figures = new PathFigureCollection()
      {
        pathFigure
      };
      this.segmentPath.Data = (Geometry) this.segmentGeometry;
      this.height = (this.bottom - this.top) * rect.Height / 2.0;
    }
  }

  public override void OnSizeChanged(Size size)
  {
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
