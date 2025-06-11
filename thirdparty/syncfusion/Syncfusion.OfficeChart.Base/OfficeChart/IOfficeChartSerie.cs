// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IOfficeChartSerie
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IOfficeChartSerie : IParentApplication
{
  ChartColor InvertIfNegativeColor { get; set; }

  IOfficeDataRange Values { get; set; }

  bool InvertIfNegative { get; set; }

  IOfficeDataRange CategoryLabels { get; set; }

  IOfficeDataRange Bubbles { get; set; }

  string Name { get; set; }

  IOfficeDataRange NameRange { get; }

  bool UsePrimaryAxis { get; set; }

  IOfficeChartDataPoints DataPoints { get; }

  IOfficeChartSerieDataFormat SerieFormat { get; }

  OfficeChartType SerieType { get; set; }

  IOfficeChartErrorBars ErrorBarsY { get; }

  bool HasErrorBarsY { get; set; }

  IOfficeChartErrorBars ErrorBarsX { get; }

  bool HasErrorBarsX { get; set; }

  IOfficeChartTrendLines TrendLines { get; }

  bool IsFiltered { get; set; }

  IOfficeChartFrameFormat ParetoLineFormat { get; }

  IOfficeChartErrorBars ErrorBar(bool bIsY);

  IOfficeChartErrorBars ErrorBar(bool bIsY, OfficeErrorBarInclude include);

  IOfficeChartErrorBars ErrorBar(bool bIsY, OfficeErrorBarInclude include, OfficeErrorBarType type);

  IOfficeChartErrorBars ErrorBar(
    bool bIsY,
    OfficeErrorBarInclude include,
    OfficeErrorBarType type,
    double numberValue);

  IOfficeChartErrorBars ErrorBar(
    bool bIsY,
    IOfficeDataRange plusRange,
    IOfficeDataRange minusRange);
}
