// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.EmptyPointSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class EmptyPointSegment : ScatterSegment
{
  private double ypos;
  private double xpos;
  private double emptyPointSymbolHeight = 20.0;
  private double emptyPointSymbolWidth = 20.0;
  private ContentControl control;

  public EmptyPointSegment(
    double xData,
    double yData,
    ChartSeriesBase series,
    bool isEmptyPointInterior)
  {
    this.Series = series;
    this.ScatterWidth = this.EmptyPointSymbolWidth;
    this.ScatterHeight = this.EmptyPointSymbolHeight;
    this.IsEmptySegmentInterior = isEmptyPointInterior;
    this.XData = xData;
    this.YData = yData;
    this.SetData(new double[2]{ xData, yData });
  }

  public double EmptyPointSymbolHeight
  {
    get => this.emptyPointSymbolHeight;
    set => this.emptyPointSymbolHeight = value;
  }

  public double EmptyPointSymbolWidth
  {
    get => this.emptyPointSymbolWidth;
    set => this.emptyPointSymbolWidth = value;
  }

  public double X
  {
    get => this.xpos;
    set
    {
      this.xpos = value;
      this.OnPropertyChanged(nameof (X));
    }
  }

  public double Y
  {
    get => this.ypos;
    set
    {
      this.ypos = value;
      this.OnPropertyChanged(nameof (Y));
    }
  }

  [Obsolete("Use SetData(ChartPoint point1, ChartPoint point2, ChartPoint point3, ChartPoint point4)")]
  public override void SetData(Point point1, Point point2, Point point3, Point point4)
  {
  }

  public override void SetData(
    ChartPoint point1,
    ChartPoint point2,
    ChartPoint point3,
    ChartPoint point4)
  {
  }

  public override UIElement CreateVisual(Size size)
  {
    if (this.Series.EmptyPointSymbolTemplate == null)
      return (UIElement) (base.CreateVisual(size) as Ellipse);
    this.control = new ContentControl();
    this.control.Content = (object) this;
    this.control.Width = this.EmptyPointSymbolWidth;
    this.control.Height = this.EmptyPointSymbolHeight;
    this.control.ContentTemplate = this.Series.EmptyPointSymbolTemplate;
    return (UIElement) this.control;
  }

  public override UIElement GetRenderedVisual()
  {
    return this.Series.EmptyPointSymbolTemplate != null && this.control != null ? (UIElement) this.control : base.GetRenderedVisual();
  }

  public override void Update(IChartTransformer transformer)
  {
    if (transformer != null)
    {
      Point visible = transformer.TransformToVisible(this.XData, this.YData);
      this.X = visible.X - this.EmptyPointSymbolWidth / 2.0;
      this.Y = visible.Y - this.EmptyPointSymbolHeight / 2.0;
    }
    if (this.Series.EmptyPointSymbolTemplate != null)
      return;
    base.Update(transformer);
  }
}
