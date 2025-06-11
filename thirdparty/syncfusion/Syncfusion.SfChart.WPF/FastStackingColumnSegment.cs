// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastStackingColumnSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class FastStackingColumnSegment : FastColumnBitmapSegment
{
  public FastStackingColumnSegment()
  {
  }

  public FastStackingColumnSegment(
    IList<double> x1Values,
    IList<double> y1Values,
    IList<double> x2Values,
    IList<double> y2Values,
    ChartSeries series)
    : base(x1Values, y1Values, x2Values, y2Values, series)
  {
    this.Series = (ChartSeriesBase) series;
    this.Item = (object) series.ActualData;
  }
}
