// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartSerie
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartSerie : IParentApplication
{
  bool? InvertIfNegative { get; set; }

  ColorObject InvertIfNegativeColor { get; set; }

  IRange Values { get; set; }

  IRange CategoryLabels { get; set; }

  IRange Bubbles { get; set; }

  string Name { get; set; }

  IRange NameRange { get; }

  bool UsePrimaryAxis { get; set; }

  IChartDataPoints DataPoints { get; }

  IChartSerieDataFormat SerieFormat { get; }

  ExcelChartType SerieType { get; set; }

  object[] EnteredDirectlyValues { get; set; }

  object[] EnteredDirectlyCategoryLabels { get; set; }

  object[] EnteredDirectlyBubbles { get; set; }

  IChartErrorBars ErrorBarsY { get; }

  bool HasErrorBarsY { get; set; }

  IChartErrorBars ErrorBarsX { get; }

  bool HasErrorBarsX { get; set; }

  IChartTrendLines TrendLines { get; }

  bool IsFiltered { get; set; }

  IChartFrameFormat ParetoLineFormat { get; }

  bool HasLeaderLines { get; set; }

  IChartBorder LeaderLines { get; }

  IChartErrorBars ErrorBar(bool bIsY);

  IChartErrorBars ErrorBar(bool bIsY, ExcelErrorBarInclude include);

  IChartErrorBars ErrorBar(bool bIsY, ExcelErrorBarInclude include, ExcelErrorBarType type);

  IChartErrorBars ErrorBar(
    bool bIsY,
    ExcelErrorBarInclude include,
    ExcelErrorBarType type,
    double numberValue);

  IChartErrorBars ErrorBar(bool bIsY, IRange plusRange, IRange minusRange);
}
