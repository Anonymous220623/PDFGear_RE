// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.HistogramDistributionSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class HistogramDistributionSegment : ChartSegment
{
  internal PointCollection distributionPoints;
  private Polyline polyLine;
  private PointCollection Points;

  public HistogramDistributionSegment(PointCollection distributionPoints, HistogramSeries series)
  {
    this.Series = (ChartSeriesBase) series;
    this.distributionPoints = distributionPoints;
  }

  public double XData { get; internal set; }

  public double YData { get; internal set; }

  public override UIElement CreateVisual(Size size)
  {
    this.polyLine = new Polyline();
    this.SetVisualBindings((Shape) this.polyLine);
    this.polyLine.Tag = (object) this;
    return (UIElement) this.polyLine;
  }

  public override UIElement GetRenderedVisual() => (UIElement) this.polyLine;

  public override void Update(IChartTransformer transformer)
  {
    this.Points = new PointCollection();
    foreach (Point distributionPoint in this.distributionPoints)
      this.Points.Add(transformer.TransformToVisible(distributionPoint.X, distributionPoint.Y));
    this.polyLine.Points = this.Points;
  }

  public override void OnSizeChanged(Size size)
  {
  }

  protected override void SetVisualBindings(Shape element)
  {
    element.SetBinding(FrameworkElement.StyleProperty, (BindingBase) new Binding()
    {
      Source = (object) this.Series,
      Path = new PropertyPath("CurveLineStyle", new object[0])
    });
  }
}
