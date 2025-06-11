// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelChartToImageConverter.ExtensionMethods
// Assembly: Syncfusion.ExcelChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 8A92A829-7139-4C93-8632-144655877EB3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelChartToImageConverter.Wpf.dll

using Syncfusion.XlsIO;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.ExcelChartToImageConverter;

internal static class ExtensionMethods
{
  internal static List<IChartSerie> OrderByType(this IChartSeries series)
  {
    List<IChartSerie> chartSerieList = new List<IChartSerie>(series.Count);
    List<IChartSerie> collection1 = new List<IChartSerie>(series.Count);
    List<IChartSerie> collection2 = new List<IChartSerie>(series.Count);
    List<IChartSerie> collection3 = new List<IChartSerie>(series.Count);
    List<IChartSerie> collection4 = new List<IChartSerie>(series.Count);
    List<IChartSerie> collection5 = new List<IChartSerie>(series.Count);
    List<IChartSerie> collection6 = new List<IChartSerie>(series.Count);
    List<IChartSerie> collection7 = new List<IChartSerie>(series.Count);
    foreach (IChartSerie chartSerie in (IEnumerable<IChartSerie>) series)
    {
      if (!chartSerie.IsFiltered)
      {
        switch (chartSerie.SerieType)
        {
          case ExcelChartType.Column_Clustered:
          case ExcelChartType.Column_Stacked:
          case ExcelChartType.Column_Stacked_100:
          case ExcelChartType.Column_Clustered_3D:
          case ExcelChartType.Column_Stacked_3D:
          case ExcelChartType.Column_Stacked_100_3D:
          case ExcelChartType.Column_3D:
          case ExcelChartType.Cylinder_Clustered:
          case ExcelChartType.Cylinder_Clustered_3D:
            collection2.Add(chartSerie);
            continue;
          case ExcelChartType.Bar_Clustered:
          case ExcelChartType.Bar_Stacked:
          case ExcelChartType.Bar_Stacked_100:
          case ExcelChartType.Bar_Clustered_3D:
          case ExcelChartType.Bar_Stacked_3D:
          case ExcelChartType.Bar_Stacked_100_3D:
            collection3.Add(chartSerie);
            continue;
          case ExcelChartType.Line:
          case ExcelChartType.Line_Stacked:
          case ExcelChartType.Line_Stacked_100:
          case ExcelChartType.Line_Markers:
          case ExcelChartType.Line_Markers_Stacked:
          case ExcelChartType.Line_Markers_Stacked_100:
          case ExcelChartType.Line_3D:
            collection6.Add(chartSerie);
            continue;
          case ExcelChartType.Pie:
          case ExcelChartType.Pie_3D:
          case ExcelChartType.PieOfPie:
          case ExcelChartType.Pie_Exploded:
          case ExcelChartType.Pie_Exploded_3D:
          case ExcelChartType.Pie_Bar:
            chartSerieList.Add(chartSerie);
            continue;
          case ExcelChartType.Scatter_Markers:
          case ExcelChartType.Scatter_SmoothedLine_Markers:
          case ExcelChartType.Scatter_SmoothedLine:
          case ExcelChartType.Scatter_Line_Markers:
          case ExcelChartType.Scatter_Line:
            collection5.Add(chartSerie);
            continue;
          case ExcelChartType.Area:
          case ExcelChartType.Area_Stacked:
          case ExcelChartType.Area_Stacked_100:
          case ExcelChartType.Area_3D:
          case ExcelChartType.Area_Stacked_3D:
          case ExcelChartType.Area_Stacked_100_3D:
            collection1.Add(chartSerie);
            continue;
          case ExcelChartType.Doughnut:
          case ExcelChartType.Doughnut_Exploded:
            collection7.Add(chartSerie);
            continue;
          case ExcelChartType.Radar:
          case ExcelChartType.Radar_Markers:
          case ExcelChartType.Radar_Filled:
            collection4.Add(chartSerie);
            continue;
          default:
            continue;
        }
      }
    }
    chartSerieList.AddRange((IEnumerable<IChartSerie>) collection4);
    if (collection3.Count > 0 && collection3.Count != series.Count && collection3[0].SerieType.ToString().Contains("Stacked"))
    {
      chartSerieList.AddRange((IEnumerable<IChartSerie>) collection3);
      chartSerieList.AddRange((IEnumerable<IChartSerie>) collection2);
      chartSerieList.AddRange((IEnumerable<IChartSerie>) collection1);
    }
    else
    {
      chartSerieList.AddRange((IEnumerable<IChartSerie>) collection1);
      chartSerieList.AddRange((IEnumerable<IChartSerie>) collection2);
      chartSerieList.AddRange((IEnumerable<IChartSerie>) collection3);
    }
    chartSerieList.AddRange((IEnumerable<IChartSerie>) collection5);
    chartSerieList.AddRange((IEnumerable<IChartSerie>) collection6);
    chartSerieList.AddRange((IEnumerable<IChartSerie>) collection7);
    return chartSerieList;
  }
}
