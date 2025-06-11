// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.ExtensionMethods
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using Syncfusion.OfficeChart;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal static class ExtensionMethods
{
  internal static List<IOfficeChartSerie> OrderByType(this IOfficeChartSeries series)
  {
    List<IOfficeChartSerie> officeChartSerieList = new List<IOfficeChartSerie>(series.Count);
    List<IOfficeChartSerie> collection1 = new List<IOfficeChartSerie>(series.Count);
    List<IOfficeChartSerie> collection2 = new List<IOfficeChartSerie>(series.Count);
    List<IOfficeChartSerie> collection3 = new List<IOfficeChartSerie>(series.Count);
    List<IOfficeChartSerie> collection4 = new List<IOfficeChartSerie>(series.Count);
    List<IOfficeChartSerie> collection5 = new List<IOfficeChartSerie>(series.Count);
    List<IOfficeChartSerie> collection6 = new List<IOfficeChartSerie>(series.Count);
    List<IOfficeChartSerie> collection7 = new List<IOfficeChartSerie>(series.Count);
    foreach (IOfficeChartSerie officeChartSerie in (IEnumerable<IOfficeChartSerie>) series)
    {
      if (!officeChartSerie.IsFiltered)
      {
        switch (officeChartSerie.SerieType)
        {
          case OfficeChartType.Column_Clustered:
          case OfficeChartType.Column_Stacked:
          case OfficeChartType.Column_Stacked_100:
          case OfficeChartType.Column_Clustered_3D:
          case OfficeChartType.Column_Stacked_3D:
          case OfficeChartType.Column_Stacked_100_3D:
          case OfficeChartType.Column_3D:
          case OfficeChartType.Cylinder_Clustered:
          case OfficeChartType.Cylinder_Clustered_3D:
            collection2.Add(officeChartSerie);
            continue;
          case OfficeChartType.Bar_Clustered:
          case OfficeChartType.Bar_Stacked:
          case OfficeChartType.Bar_Stacked_100:
          case OfficeChartType.Bar_Clustered_3D:
          case OfficeChartType.Bar_Stacked_3D:
          case OfficeChartType.Bar_Stacked_100_3D:
            collection3.Add(officeChartSerie);
            continue;
          case OfficeChartType.Line:
          case OfficeChartType.Line_Stacked:
          case OfficeChartType.Line_Stacked_100:
          case OfficeChartType.Line_Markers:
          case OfficeChartType.Line_Markers_Stacked:
          case OfficeChartType.Line_Markers_Stacked_100:
          case OfficeChartType.Line_3D:
            collection6.Add(officeChartSerie);
            continue;
          case OfficeChartType.Pie:
          case OfficeChartType.Pie_3D:
          case OfficeChartType.PieOfPie:
          case OfficeChartType.Pie_Exploded:
          case OfficeChartType.Pie_Exploded_3D:
          case OfficeChartType.Pie_Bar:
            officeChartSerieList.Add(officeChartSerie);
            continue;
          case OfficeChartType.Scatter_Markers:
          case OfficeChartType.Scatter_SmoothedLine_Markers:
          case OfficeChartType.Scatter_SmoothedLine:
          case OfficeChartType.Scatter_Line_Markers:
          case OfficeChartType.Scatter_Line:
            collection5.Add(officeChartSerie);
            continue;
          case OfficeChartType.Area:
          case OfficeChartType.Area_Stacked:
          case OfficeChartType.Area_Stacked_100:
          case OfficeChartType.Area_3D:
          case OfficeChartType.Area_Stacked_3D:
          case OfficeChartType.Area_Stacked_100_3D:
            collection1.Add(officeChartSerie);
            continue;
          case OfficeChartType.Doughnut:
          case OfficeChartType.Doughnut_Exploded:
            collection7.Add(officeChartSerie);
            continue;
          case OfficeChartType.Radar:
          case OfficeChartType.Radar_Markers:
          case OfficeChartType.Radar_Filled:
            collection4.Add(officeChartSerie);
            continue;
          default:
            continue;
        }
      }
    }
    officeChartSerieList.AddRange((IEnumerable<IOfficeChartSerie>) collection4);
    if (collection3.Count > 0 && collection3.Count != series.Count && collection3[0].SerieType.ToString().Contains("Stacked"))
    {
      officeChartSerieList.AddRange((IEnumerable<IOfficeChartSerie>) collection3);
      officeChartSerieList.AddRange((IEnumerable<IOfficeChartSerie>) collection2);
      officeChartSerieList.AddRange((IEnumerable<IOfficeChartSerie>) collection1);
    }
    else
    {
      officeChartSerieList.AddRange((IEnumerable<IOfficeChartSerie>) collection1);
      officeChartSerieList.AddRange((IEnumerable<IOfficeChartSerie>) collection2);
      officeChartSerieList.AddRange((IEnumerable<IOfficeChartSerie>) collection3);
    }
    officeChartSerieList.AddRange((IEnumerable<IOfficeChartSerie>) collection5);
    officeChartSerieList.AddRange((IEnumerable<IOfficeChartSerie>) collection6);
    officeChartSerieList.AddRange((IEnumerable<IOfficeChartSerie>) collection7);
    return officeChartSerieList;
  }
}
