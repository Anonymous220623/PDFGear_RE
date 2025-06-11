// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.PyramidSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class PyramidSegment : ChartSegment
{
  internal bool isExploded;
  internal double height;
  internal double y;
  private double explodedOffset;
  private Path segmentPath;
  private PathGeometry segmentGeometry;
  private double xData;
  private double yData;

  public PyramidSegment()
  {
  }

  public PyramidSegment(
    double y,
    double height,
    double explodedOffset,
    PyramidSeries series,
    bool isExploded)
  {
    this.Series = (ChartSeriesBase) series;
    this.y = y;
    this.height = height;
    this.isExploded = isExploded;
    this.explodedOffset = explodedOffset;
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

  public override void SetData(params double[] Values)
  {
    this.y = Values[0];
    this.height = Values[1];
    this.explodedOffset = Values[2];
  }

  public override UIElement CreateVisual(Size size)
  {
    this.segmentPath = new Path();
    this.SetVisualBindings((Shape) this.segmentPath);
    this.segmentPath.Tag = (object) this;
    return (UIElement) this.segmentPath;
  }

  public override void Update(IChartTransformer transformer)
  {
    if (!this.IsSegmentVisible)
      this.segmentPath.Visibility = Visibility.Collapsed;
    else
      this.segmentPath.Visibility = Visibility.Visible;
    Rect rect = new Rect(0.0, 0.0, transformer.Viewport.Width, transformer.Viewport.Height);
    PyramidSeries series = this.Series as PyramidSeries;
    if (rect.IsEmpty)
    {
      this.segmentPath.Data = (Geometry) null;
    }
    else
    {
      if (this.isExploded)
        rect.X += this.explodedOffset;
      double y = this.y;
      double num1 = this.y + this.height;
      double num2 = 0.5 * (1.0 - this.y);
      double num3 = 0.5 * (1.0 - num1);
      PathFigure pathFigure = new PathFigure();
      pathFigure.StartPoint = new Point(rect.X + num2 * rect.Width, rect.Y + y * rect.Height);
      pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
      {
        Point = new Point(rect.X + (1.0 - num2) * rect.Width, rect.Y + y * rect.Height)
      });
      pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
      {
        Point = new Point(rect.X + (1.0 - num3) * rect.Width, rect.Y + num1 * rect.Height - series.StrokeThickness / 2.0)
      });
      pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
      {
        Point = new Point(rect.X + num3 * rect.Width, rect.Y + num1 * rect.Height - series.StrokeThickness / 2.0)
      });
      pathFigure.IsClosed = true;
      this.segmentGeometry = new PathGeometry();
      this.segmentGeometry.Figures = new PathFigureCollection()
      {
        pathFigure
      };
      this.segmentPath.Data = (Geometry) this.segmentGeometry;
      this.height = (num1 - y) * rect.Height / 2.0;
    }
  }

  public override UIElement GetRenderedVisual() => (UIElement) this.segmentPath;

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
