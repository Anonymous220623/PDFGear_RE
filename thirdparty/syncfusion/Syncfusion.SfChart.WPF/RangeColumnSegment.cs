// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.RangeColumnSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class RangeColumnSegment : ColumnSegment
{
  public RangeColumnSegment()
  {
  }

  public RangeColumnSegment(
    double x1,
    double y1,
    double x2,
    double y2,
    RangeColumnSeries series,
    object item)
    : base(x1, y1, x2, y2)
  {
    this.Series = (ChartSeriesBase) series;
    this.customTemplate = series.CustomTemplate;
    this.Item = item;
  }

  public double High { get; set; }

  public double Low { get; set; }

  public override void Update(IChartTransformer transformer)
  {
    if (!this.Series.IsMultipleYPathRequired)
    {
      DoubleRange visibleRange = this.Series.ActualYAxis.VisibleRange;
      double num1 = (visibleRange.End - Math.Abs(visibleRange.Start)) / 2.0;
      double num2 = this.Top / 2.0;
      this.Top = num1 + num2;
      this.Bottom = num1 - num2;
      int index = this.Series.Segments.IndexOf((ChartSegment) this);
      this.YData = (this.Series as RangeSeriesBase).High == null ? (this.Series as RangeSeriesBase).LowValues[index] : (this.Series as RangeSeriesBase).HighValues[index];
    }
    else
    {
      this.High = this.Top;
      this.Low = this.Bottom;
    }
    base.Update(transformer);
  }
}
