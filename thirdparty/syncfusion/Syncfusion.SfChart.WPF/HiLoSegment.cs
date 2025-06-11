// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.HiLoSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class HiLoSegment : ChartSegment
{
  private double lowValue;
  private double highValue;
  private double xVal;
  private Line segLine;

  public HiLoSegment()
  {
  }

  public HiLoSegment(
    double xVal,
    double hghValue,
    double lwValue,
    HiLoSeries series,
    object item)
  {
    this.Series = (ChartSeriesBase) series;
    this.Item = item;
    this.SetData(xVal, hghValue, lwValue);
  }

  public double High { get; set; }

  public double Low { get; set; }

  public object XValue { get; set; }

  public override void SetData(params double[] Values)
  {
    this.highValue = Values[1];
    this.lowValue = Values[2];
    this.xVal = Values[0];
    this.XRange = new DoubleRange(Values[0], Values[0]);
    if (!double.IsNaN(Values[1]) || !double.IsNaN(Values[2]))
      this.YRange = DoubleRange.Union(Values[1], Values[2]);
    else
      this.YRange = DoubleRange.Empty;
  }

  public override UIElement CreateVisual(Size size)
  {
    this.segLine = new Line();
    this.segLine.Tag = (object) this;
    this.SetVisualBindings((Shape) this.segLine);
    return (UIElement) this.segLine;
  }

  public override UIElement GetRenderedVisual() => (UIElement) this.segLine;

  public override void Update(IChartTransformer transformer)
  {
    if (transformer == null)
      return;
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    double num1 = Math.Floor(cartesianTransformer.XAxis.VisibleRange.Start);
    double num2 = Math.Ceiling(cartesianTransformer.XAxis.VisibleRange.End);
    double newBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
    double num3 = cartesianTransformer.XAxis.IsLogarithmic ? Math.Log(this.xVal, newBase) : this.xVal;
    if (num3 >= num1 && num3 <= num2 && (!double.IsNaN(this.highValue) && !double.IsNaN(this.lowValue) || this.Series.ShowEmptyPoints))
    {
      Point visible1 = transformer.TransformToVisible(this.xVal, this.highValue);
      Point visible2 = transformer.TransformToVisible(this.xVal, this.lowValue);
      this.segLine.X1 = visible1.X;
      this.segLine.Y1 = visible1.Y;
      this.segLine.X2 = visible2.X;
      this.segLine.Y2 = visible2.Y;
    }
    else
      this.segLine.ClearUIValues();
  }

  public override void OnSizeChanged(Size size)
  {
  }

  internal override void Dispose()
  {
    if (this.segLine != null)
    {
      this.segLine.Tag = (object) null;
      this.segLine = (Line) null;
    }
    base.Dispose();
  }
}
