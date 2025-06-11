// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.ChartSeriesFactory
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using Syncfusion.UI.Xaml.Charts;
using System;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal class ChartSeriesFactory
{
  internal static ChartSeries CreateSeries(Type type)
  {
    switch (type.Name)
    {
      case "BarSeries":
        BarSeries series1 = new BarSeries();
        series1.XBindingPath = "X";
        series1.YBindingPath = "Value";
        return (ChartSeries) series1;
      case "FastBarBitmapSeries":
        FastBarBitmapSeries series2 = new FastBarBitmapSeries();
        series2.XBindingPath = "X";
        series2.YBindingPath = "Value";
        return (ChartSeries) series2;
      case "LineSeries":
        LineSeries series3 = new LineSeries();
        series3.XBindingPath = "X";
        series3.YBindingPath = "Value";
        return (ChartSeries) series3;
      case "FastLineSeries":
        FastLineSeries series4 = new FastLineSeries();
        series4.XBindingPath = "X";
        series4.YBindingPath = "Value";
        return (ChartSeries) series4;
      case "ColumnSeries":
        ColumnSeries series5 = new ColumnSeries();
        series5.XBindingPath = "X";
        series5.YBindingPath = "Value";
        return (ChartSeries) series5;
      case "FastColumnBitmapSeries":
        FastColumnBitmapSeries series6 = new FastColumnBitmapSeries();
        series6.XBindingPath = "X";
        series6.YBindingPath = "Value";
        return (ChartSeries) series6;
      case "StackingColumnSeries":
        StackingColumnSeries series7 = new StackingColumnSeries();
        series7.XBindingPath = "X";
        series7.YBindingPath = "Value";
        return (ChartSeries) series7;
      case "FastStackingColumnBitmapSeries":
        FastStackingColumnBitmapSeries series8 = new FastStackingColumnBitmapSeries();
        series8.XBindingPath = "X";
        series8.YBindingPath = "Value";
        return (ChartSeries) series8;
      case "ScatterSeries":
        ScatterSeries series9 = new ScatterSeries();
        series9.XBindingPath = "X";
        series9.YBindingPath = "Value";
        return (ChartSeries) series9;
      case "FastScatterBitmapSeries":
        FastScatterBitmapSeries series10 = new FastScatterBitmapSeries();
        series10.XBindingPath = "X";
        series10.YBindingPath = "Value";
        return (ChartSeries) series10;
      default:
        return (ChartSeries) null;
    }
  }
}
