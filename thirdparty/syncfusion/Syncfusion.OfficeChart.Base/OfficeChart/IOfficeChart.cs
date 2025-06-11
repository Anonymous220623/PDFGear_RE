// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IOfficeChart
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Collections;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IOfficeChart
{
  OfficeChartType ChartType { get; set; }

  IOfficeDataRange DataRange { get; set; }

  bool IsSeriesInRows { get; set; }

  string ChartTitle { get; set; }

  IOfficeChartTextArea ChartTitleArea { get; }

  double XPos { get; set; }

  double YPos { get; set; }

  double Width { get; set; }

  double Height { get; set; }

  IOfficeChartSeries Series { get; }

  IOfficeChartCategoryAxis PrimaryCategoryAxis { get; }

  IOfficeChartValueAxis PrimaryValueAxis { get; }

  IOfficeChartSeriesAxis PrimarySerieAxis { get; }

  IOfficeChartCategoryAxis SecondaryCategoryAxis { get; }

  IOfficeChartValueAxis SecondaryValueAxis { get; }

  IOfficeChartFrameFormat ChartArea { get; }

  IOfficeChartFrameFormat PlotArea { get; }

  IOfficeChartWallOrFloor Walls { get; }

  IOfficeChartWallOrFloor SideWall { get; }

  IOfficeChartWallOrFloor BackWall { get; }

  IOfficeChartWallOrFloor Floor { get; }

  IOfficeChartDataTable DataTable { get; }

  bool HasDataTable { get; set; }

  IOfficeChartLegend Legend { get; }

  bool HasLegend { get; set; }

  bool HasTitle { get; set; }

  int Rotation { get; set; }

  int Elevation { get; set; }

  int Perspective { get; set; }

  void Refresh();

  int HeightPercent { get; set; }

  int DepthPercent { get; set; }

  int GapDepth { get; set; }

  bool RightAngleAxes { get; set; }

  bool AutoScaling { get; set; }

  bool WallsAndGridlines2D { get; set; }

  bool HasPlotArea { get; set; }

  OfficeChartPlotEmpty DisplayBlanksAs { get; set; }

  bool PlotVisibleOnly { get; set; }

  bool SizeWithWindow { get; set; }

  IOfficeChartCategories Categories { get; }

  OfficeSeriesNameLevel SeriesNameLevel { get; set; }

  OfficeCategoriesLabelLevel CategoryLabelLevel { get; set; }

  int Style { get; set; }

  void SaveAsImage(Stream imageAsStream);

  IOfficeChartData ChartData { get; }

  void SetChartData(object[][] data);

  void SetDataRange(object[][] data, int rowIndex, int columnIndex);

  void SetDataRange(IEnumerable enumerable, int rowIndex, int columnIndex);
}
