// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.StackingColumnSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class StackingColumnSegment : ColumnSegment
{
  public StackingColumnSegment()
  {
  }

  public StackingColumnSegment(
    double x1,
    double y1,
    double x2,
    double y2,
    StackingColumnSeries series)
    : base(x1, y1, x2, y2)
  {
    this.Series = (ChartSeriesBase) series;
    this.customTemplate = series.CustomTemplate;
  }

  public override void Update(IChartTransformer transformer) => base.Update(transformer);
}
