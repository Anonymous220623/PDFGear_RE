// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChart
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChart : ITabSheet, IParentApplication
{
  ExcelChartType ChartType { get; set; }

  IRange DataRange { get; set; }

  bool IsSeriesInRows { get; set; }

  string ChartTitle { get; set; }

  IChartTextArea ChartTitleArea { get; }

  IChartPageSetup PageSetup { get; }

  double XPos { get; set; }

  double YPos { get; set; }

  double Width { get; set; }

  double Height { get; set; }

  IChartSeries Series { get; }

  IChartCategoryAxis PrimaryCategoryAxis { get; }

  IChartValueAxis PrimaryValueAxis { get; }

  IChartSeriesAxis PrimarySerieAxis { get; }

  IChartCategoryAxis SecondaryCategoryAxis { get; }

  IChartValueAxis SecondaryValueAxis { get; }

  IChartFrameFormat ChartArea { get; }

  IChartFrameFormat PlotArea { get; }

  IChartWallOrFloor Walls { get; }

  IChartWallOrFloor SideWall { get; }

  IChartWallOrFloor BackWall { get; }

  IChartWallOrFloor Floor { get; }

  IChartDataTable DataTable { get; }

  bool HasDataTable { get; set; }

  IChartLegend Legend { get; }

  bool HasTitle { get; set; }

  bool HasLegend { get; set; }

  int Rotation { get; set; }

  int Elevation { get; set; }

  int Perspective { get; set; }

  int HeightPercent { get; set; }

  int DepthPercent { get; set; }

  int GapDepth { get; set; }

  bool RightAngleAxes { get; set; }

  bool AutoScaling { get; set; }

  bool WallsAndGridlines2D { get; set; }

  bool HasPlotArea { get; set; }

  ExcelChartPlotEmpty DisplayBlanksAs { get; set; }

  bool PlotVisibleOnly { get; set; }

  bool SizeWithWindow { get; set; }

  IPivotTable PivotSource { get; set; }

  ExcelChartType PivotChartType { get; set; }

  bool ShowAllFieldButtons { get; set; }

  bool ShowValueFieldButtons { get; set; }

  bool ShowAxisFieldButtons { get; set; }

  bool ShowLegendFieldButtons { get; set; }

  bool ShowReportFilterFieldButtons { get; set; }

  IChartCategories Categories { get; }

  ExcelSeriesNameLevel SeriesNameLevel { get; set; }

  ExcelCategoriesLabelLevel CategoryLabelLevel { get; set; }

  int Style { get; set; }

  void SaveAsImage(Stream imageAsStream);
}
