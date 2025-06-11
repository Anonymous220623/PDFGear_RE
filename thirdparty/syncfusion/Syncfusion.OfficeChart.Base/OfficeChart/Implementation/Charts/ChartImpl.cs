// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.XmlReaders;
using Syncfusion.OfficeChart.Implementation.XmlSerialization;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartImpl : 
  WorksheetBaseImpl,
  IOfficeChart,
  ISerializableNamedObject,
  INamedObject,
  IParseable,
  ICloneParent
{
  internal const string DefaultChartTitle = "Chart Title";
  internal const string DefaultTitleFontName = "Calibri";
  internal const double DefaultTitleFontSize = 18.6;
  public const string DEF_FIRST_SERIE_NAME = "Serie1";
  public const OfficeChartType DEFAULT_CHART_TYPE = OfficeChartType.Column_Clustered;
  public const string PREFIX_3D = "_3D";
  public const string PREFIX_BAR = "_Bar";
  public const string PREFIX_CLUSTERED = "_Clustered";
  public const string PREFIX_CONTOUR = "_Contour";
  public const string PREFIX_EXPLODED = "_Exploded";
  public const string PREFIX_LINE = "_Line";
  public const string PREFIX_MARKERS = "_Markers";
  public const string PREFIX_NOCOLOR = "_NoColor";
  public const string PREFIX_SHOW_PERCENT = "_100";
  public const string PREFIX_SMOOTHEDLINE = "_SmoothedLine";
  public const string PREFIX_STACKED = "_Stacked";
  public const string START_AREA = "Area";
  public const string START_BAR = "Bar";
  public const string START_BUBBLE = "Bubble";
  public const string START_COLUMN = "Column";
  public const string START_CONE = "Cone";
  public const string START_CYLINDER = "Cylinder";
  public const string START_DOUGHNUT = "Doughnut";
  public const string START_LINE = "Line";
  public const string START_PIE = "Pie";
  public const string START_PYRAMID = "Pyramid";
  public const string START_RADAR = "Radar";
  public const string START_SCATTER = "Scatter";
  public const string START_SURFACE = "Surface";
  private const int DEF_PRIMARY_INDEX = 0;
  public const int DEF_SI_VALUE = 1;
  public const int DEF_SI_CATEGORY = 2;
  public const int DEF_SI_BUBBLE = 3;
  private const int DEF_SECONDARY_INDEX = 1;
  private const int MaximumFontCount = 506;
  internal const int DefaultPlotAreaX = 328;
  internal const int DefaultPlotAreaY = 243;
  internal const int DefaultPlotAreaXLength = 3125;
  internal const int DefaultPlotAreaYLength = 3283;
  public static readonly string[] DEF_LEGEND_NEED_DATA_POINT = new string[3]
  {
    "Pie",
    "Doughnut",
    "Surface"
  };
  public static readonly OfficeChartType[] DEF_SUPPORT_SERIES_AXIS = new OfficeChartType[10]
  {
    OfficeChartType.Surface_3D,
    OfficeChartType.Surface_Contour,
    OfficeChartType.Surface_NoColor_Contour,
    OfficeChartType.Surface_NoColor_3D,
    OfficeChartType.Column_3D,
    OfficeChartType.Line_3D,
    OfficeChartType.Area_3D,
    OfficeChartType.Pyramid_Clustered_3D,
    OfficeChartType.Cone_Clustered_3D,
    OfficeChartType.Cylinder_Clustered_3D
  };
  public static readonly OfficeChartType[] DEF_UNSUPPORT_PIVOT_CHART = new OfficeChartType[11]
  {
    OfficeChartType.Scatter_Line,
    OfficeChartType.Scatter_Line_Markers,
    OfficeChartType.Scatter_Markers,
    OfficeChartType.Scatter_SmoothedLine,
    OfficeChartType.Scatter_SmoothedLine_Markers,
    OfficeChartType.Stock_HighLowClose,
    OfficeChartType.Stock_OpenHighLowClose,
    OfficeChartType.Stock_VolumeHighLowClose,
    OfficeChartType.Stock_VolumeOpenHighLowClose,
    OfficeChartType.Bubble,
    OfficeChartType.Bubble_3D
  };
  public static readonly string[] DEF_SUPPORT_DATA_TABLE = new string[8]
  {
    "Column",
    "Bar",
    "Line",
    "Area",
    "Cylinder",
    "Cone",
    "Pyramid",
    "Stock"
  };
  public static readonly string[] DEF_SUPPORT_ERROR_BARS = new string[6]
  {
    "Column",
    "Bar",
    "Line",
    "Area",
    "Scatter",
    "Bubble"
  };
  public static readonly OfficeChartType[] DEF_SUPPORT_TREND_LINES = new OfficeChartType[16 /*0x10*/]
  {
    OfficeChartType.Column_Clustered,
    OfficeChartType.Bar_Clustered,
    OfficeChartType.Line,
    OfficeChartType.Line_Markers,
    OfficeChartType.Scatter_Line,
    OfficeChartType.Scatter_Line_Markers,
    OfficeChartType.Scatter_Markers,
    OfficeChartType.Scatter_SmoothedLine,
    OfficeChartType.Scatter_SmoothedLine_Markers,
    OfficeChartType.Stock_HighLowClose,
    OfficeChartType.Stock_OpenHighLowClose,
    OfficeChartType.Stock_VolumeHighLowClose,
    OfficeChartType.Stock_VolumeOpenHighLowClose,
    OfficeChartType.Area,
    OfficeChartType.Bubble,
    OfficeChartType.Bubble_3D
  };
  public static readonly OfficeChartType[] DEF_WALLS_OR_FLOOR_TYPES = new OfficeChartType[34]
  {
    OfficeChartType.Column_3D,
    OfficeChartType.Column_Clustered_3D,
    OfficeChartType.Column_Stacked_100_3D,
    OfficeChartType.Column_Stacked_3D,
    OfficeChartType.Bar_Clustered_3D,
    OfficeChartType.Bar_Stacked_3D,
    OfficeChartType.Bar_Stacked_100_3D,
    OfficeChartType.Line_3D,
    OfficeChartType.Area_3D,
    OfficeChartType.Area_Stacked_3D,
    OfficeChartType.Area_Stacked_100_3D,
    OfficeChartType.Cylinder_Clustered,
    OfficeChartType.Cylinder_Stacked,
    OfficeChartType.Cylinder_Stacked_100,
    OfficeChartType.Cylinder_Bar_Clustered,
    OfficeChartType.Cylinder_Bar_Stacked,
    OfficeChartType.Cylinder_Bar_Stacked_100,
    OfficeChartType.Cylinder_Clustered_3D,
    OfficeChartType.Cone_Clustered,
    OfficeChartType.Cone_Stacked,
    OfficeChartType.Cone_Stacked_100,
    OfficeChartType.Cone_Bar_Clustered,
    OfficeChartType.Cone_Bar_Stacked,
    OfficeChartType.Cone_Bar_Stacked_100,
    OfficeChartType.Cone_Clustered_3D,
    OfficeChartType.Pyramid_Clustered,
    OfficeChartType.Pyramid_Stacked,
    OfficeChartType.Pyramid_Stacked_100,
    OfficeChartType.Pyramid_Bar_Clustered,
    OfficeChartType.Pyramid_Bar_Stacked,
    OfficeChartType.Pyramid_Bar_Stacked_100,
    OfficeChartType.Pyramid_Clustered_3D,
    OfficeChartType.Surface_3D,
    OfficeChartType.Surface_NoColor_3D
  };
  private static readonly OfficeAxisType[] DEF_SECONDARY_AXES_TYPES = new OfficeAxisType[2]
  {
    OfficeAxisType.Category,
    OfficeAxisType.Value
  };
  public static readonly OfficeChartType[] DEF_NOT_3D = new OfficeChartType[17]
  {
    OfficeChartType.Scatter_Markers,
    OfficeChartType.Scatter_SmoothedLine_Markers,
    OfficeChartType.Scatter_SmoothedLine,
    OfficeChartType.Scatter_Line_Markers,
    OfficeChartType.Scatter_Line,
    OfficeChartType.Doughnut,
    OfficeChartType.Doughnut_Exploded,
    OfficeChartType.Radar,
    OfficeChartType.Radar_Markers,
    OfficeChartType.Radar_Filled,
    OfficeChartType.Bubble,
    OfficeChartType.Bubble_3D,
    OfficeChartType.Stock_HighLowClose,
    OfficeChartType.Stock_OpenHighLowClose,
    OfficeChartType.Stock_VolumeHighLowClose,
    OfficeChartType.Stock_VolumeOpenHighLowClose,
    OfficeChartType.Combination_Chart
  };
  public static readonly OfficeChartType[] DEF_CHANGE_SERIE = new OfficeChartType[31 /*0x1F*/]
  {
    OfficeChartType.Column_Clustered,
    OfficeChartType.Column_Stacked,
    OfficeChartType.Column_Stacked_100,
    OfficeChartType.Bar_Clustered,
    OfficeChartType.Bar_Stacked,
    OfficeChartType.Bar_Stacked_100,
    OfficeChartType.Line,
    OfficeChartType.Line_Stacked,
    OfficeChartType.Line_Stacked_100,
    OfficeChartType.Line_Markers,
    OfficeChartType.Line_Markers_Stacked,
    OfficeChartType.Line_Markers_Stacked_100,
    OfficeChartType.Pie,
    OfficeChartType.PieOfPie,
    OfficeChartType.Pie_Exploded,
    OfficeChartType.Pie_Bar,
    OfficeChartType.Scatter_Markers,
    OfficeChartType.Scatter_SmoothedLine_Markers,
    OfficeChartType.Scatter_SmoothedLine,
    OfficeChartType.Scatter_Line_Markers,
    OfficeChartType.Scatter_Line,
    OfficeChartType.Area,
    OfficeChartType.Area_Stacked,
    OfficeChartType.Area_Stacked_100,
    OfficeChartType.Doughnut,
    OfficeChartType.Doughnut_Exploded,
    OfficeChartType.Radar,
    OfficeChartType.Radar_Markers,
    OfficeChartType.Radar_Filled,
    OfficeChartType.Bubble,
    OfficeChartType.Bubble_3D
  };
  public static readonly OfficeChartType[] DEF_NOT_SUPPORT_GRIDLINES = new OfficeChartType[8]
  {
    OfficeChartType.Doughnut,
    OfficeChartType.Doughnut_Exploded,
    OfficeChartType.Pie_Bar,
    OfficeChartType.Pie_Exploded,
    OfficeChartType.PieOfPie,
    OfficeChartType.Pie,
    OfficeChartType.Pie_3D,
    OfficeChartType.Pie_Exploded_3D
  };
  public static readonly OfficeChartType[] DEF_NEED_SECONDARY_AXIS = new OfficeChartType[11]
  {
    OfficeChartType.Doughnut,
    OfficeChartType.Doughnut_Exploded,
    OfficeChartType.Pie_Bar,
    OfficeChartType.Pie_Exploded,
    OfficeChartType.PieOfPie,
    OfficeChartType.Pie,
    OfficeChartType.Radar,
    OfficeChartType.Radar_Filled,
    OfficeChartType.Bar_Clustered,
    OfficeChartType.Bar_Stacked,
    OfficeChartType.Bar_Stacked_100
  };
  public static readonly OfficeChartType[] DEF_COMBINATION_CHART = new OfficeChartType[5]
  {
    OfficeChartType.Stock_HighLowClose,
    OfficeChartType.Stock_OpenHighLowClose,
    OfficeChartType.Stock_VolumeHighLowClose,
    OfficeChartType.Stock_VolumeOpenHighLowClose,
    OfficeChartType.Combination_Chart
  };
  public static readonly string[] DEF_PRIORITY_START_TYPES = new string[8]
  {
    "Pie",
    "Doughnut",
    "Radar",
    "Area",
    "Column",
    "Bar",
    "Line",
    "Scatter"
  };
  public static readonly OfficeChartType[] DEF_CHANGE_INTIMATE = new OfficeChartType[16 /*0x10*/]
  {
    OfficeChartType.Radar,
    OfficeChartType.Radar_Markers,
    OfficeChartType.Radar_Filled,
    OfficeChartType.Scatter_Markers,
    OfficeChartType.Scatter_SmoothedLine_Markers,
    OfficeChartType.Scatter_SmoothedLine,
    OfficeChartType.Scatter_Line_Markers,
    OfficeChartType.Scatter_Line,
    OfficeChartType.Line,
    OfficeChartType.Line_Stacked,
    OfficeChartType.Line_Stacked_100,
    OfficeChartType.Line_Markers,
    OfficeChartType.Line_Markers_Stacked,
    OfficeChartType.Line_Markers_Stacked_100,
    OfficeChartType.Bubble,
    OfficeChartType.Bubble_3D
  };
  public static readonly OfficeChartType[] DEF_DONT_NEED_PLOT = new OfficeChartType[13]
  {
    OfficeChartType.Doughnut,
    OfficeChartType.Doughnut_Exploded,
    OfficeChartType.Pie_Bar,
    OfficeChartType.Pie_Exploded,
    OfficeChartType.PieOfPie,
    OfficeChartType.Pie,
    OfficeChartType.Pie_3D,
    OfficeChartType.Pie_Exploded_3D,
    OfficeChartType.Radar,
    OfficeChartType.Radar_Markers,
    OfficeChartType.Radar_Filled,
    OfficeChartType.Surface_Contour,
    OfficeChartType.Surface_NoColor_Contour
  };
  public static readonly OfficeChartType[] DEF_NEED_VIEW_3D = new OfficeChartType[4]
  {
    OfficeChartType.Surface_3D,
    OfficeChartType.Surface_Contour,
    OfficeChartType.Surface_NoColor_3D,
    OfficeChartType.Surface_NoColor_Contour
  };
  public static readonly OfficeChartType[] CHARTS_100 = new OfficeChartType[14]
  {
    OfficeChartType.Column_Stacked_100,
    OfficeChartType.Column_Stacked_100_3D,
    OfficeChartType.Bar_Stacked_100,
    OfficeChartType.Bar_Stacked_100_3D,
    OfficeChartType.Line_Stacked_100,
    OfficeChartType.Line_Markers_Stacked_100,
    OfficeChartType.Area_Stacked_100,
    OfficeChartType.Area_Stacked_100_3D,
    OfficeChartType.Cylinder_Stacked_100,
    OfficeChartType.Cylinder_Bar_Stacked_100,
    OfficeChartType.Cone_Stacked_100,
    OfficeChartType.Cone_Bar_Stacked_100,
    OfficeChartType.Pyramid_Stacked_100,
    OfficeChartType.Pyramid_Bar_Stacked_100
  };
  public static readonly OfficeChartType[] STACKEDCHARTS = new OfficeChartType[28]
  {
    OfficeChartType.Column_Stacked,
    OfficeChartType.Column_Stacked_3D,
    OfficeChartType.Bar_Stacked,
    OfficeChartType.Bar_Stacked_3D,
    OfficeChartType.Line_Stacked,
    OfficeChartType.Line_Markers_Stacked,
    OfficeChartType.Area_Stacked,
    OfficeChartType.Area_Stacked_3D,
    OfficeChartType.Cylinder_Stacked,
    OfficeChartType.Cylinder_Bar_Stacked,
    OfficeChartType.Cone_Stacked,
    OfficeChartType.Cone_Bar_Stacked,
    OfficeChartType.Pyramid_Stacked,
    OfficeChartType.Pyramid_Bar_Stacked,
    OfficeChartType.Column_Stacked_100,
    OfficeChartType.Column_Stacked_100_3D,
    OfficeChartType.Bar_Stacked_100,
    OfficeChartType.Bar_Stacked_100_3D,
    OfficeChartType.Line_Stacked_100,
    OfficeChartType.Line_Markers_Stacked_100,
    OfficeChartType.Area_Stacked_100,
    OfficeChartType.Area_Stacked_100_3D,
    OfficeChartType.Cylinder_Stacked_100,
    OfficeChartType.Cylinder_Bar_Stacked_100,
    OfficeChartType.Cone_Stacked_100,
    OfficeChartType.Cone_Bar_Stacked_100,
    OfficeChartType.Pyramid_Stacked_100,
    OfficeChartType.Pyramid_Bar_Stacked_100
  };
  public static readonly OfficeChartType[] CHARTS3D = new OfficeChartType[39]
  {
    OfficeChartType.Column_Clustered_3D,
    OfficeChartType.Column_Stacked_3D,
    OfficeChartType.Column_Stacked_100_3D,
    OfficeChartType.Column_3D,
    OfficeChartType.Bar_Clustered_3D,
    OfficeChartType.Bar_Stacked_3D,
    OfficeChartType.Bar_Stacked_100_3D,
    OfficeChartType.Bubble_3D,
    OfficeChartType.Line_3D,
    OfficeChartType.Pie_3D,
    OfficeChartType.Pie_Exploded_3D,
    OfficeChartType.Area_3D,
    OfficeChartType.Area_Stacked_3D,
    OfficeChartType.Area_Stacked_100_3D,
    OfficeChartType.Surface_3D,
    OfficeChartType.Surface_NoColor_3D,
    OfficeChartType.Surface_Contour,
    OfficeChartType.Surface_NoColor_Contour,
    OfficeChartType.Cylinder_Clustered,
    OfficeChartType.Cylinder_Stacked,
    OfficeChartType.Cylinder_Stacked_100,
    OfficeChartType.Cylinder_Bar_Clustered,
    OfficeChartType.Cylinder_Bar_Stacked,
    OfficeChartType.Cylinder_Bar_Stacked_100,
    OfficeChartType.Cylinder_Clustered_3D,
    OfficeChartType.Cone_Clustered,
    OfficeChartType.Cone_Stacked,
    OfficeChartType.Cone_Stacked_100,
    OfficeChartType.Cone_Bar_Clustered,
    OfficeChartType.Cone_Bar_Stacked,
    OfficeChartType.Cone_Bar_Stacked_100,
    OfficeChartType.Cone_Clustered_3D,
    OfficeChartType.Pyramid_Clustered,
    OfficeChartType.Pyramid_Stacked,
    OfficeChartType.Pyramid_Stacked_100,
    OfficeChartType.Pyramid_Bar_Clustered,
    OfficeChartType.Pyramid_Bar_Stacked,
    OfficeChartType.Pyramid_Bar_Stacked_100,
    OfficeChartType.Pyramid_Clustered_3D
  };
  public static readonly OfficeChartType[] CHARTS_LINE = new OfficeChartType[7]
  {
    OfficeChartType.Line,
    OfficeChartType.Line_3D,
    OfficeChartType.Line_Markers,
    OfficeChartType.Line_Markers_Stacked,
    OfficeChartType.Line_Markers_Stacked_100,
    OfficeChartType.Line_Stacked,
    OfficeChartType.Line_Stacked_100
  };
  public static readonly OfficeChartType[] CHARTS_BUBBLE = new OfficeChartType[2]
  {
    OfficeChartType.Bubble,
    OfficeChartType.Bubble_3D
  };
  public static readonly OfficeChartType[] NO_CATEGORY_AXIS = new OfficeChartType[8]
  {
    OfficeChartType.Doughnut,
    OfficeChartType.Doughnut_Exploded,
    OfficeChartType.Pie,
    OfficeChartType.Pie_3D,
    OfficeChartType.Pie_Bar,
    OfficeChartType.Pie_Exploded,
    OfficeChartType.Pie_Exploded_3D,
    OfficeChartType.PieOfPie
  };
  public static readonly OfficeChartType[] CHARTS_VARYCOLOR = new OfficeChartType[8]
  {
    OfficeChartType.Doughnut,
    OfficeChartType.Doughnut_Exploded,
    OfficeChartType.Pie,
    OfficeChartType.Pie_3D,
    OfficeChartType.Pie_Bar,
    OfficeChartType.Pie_Exploded,
    OfficeChartType.Pie_Exploded_3D,
    OfficeChartType.PieOfPie
  };
  public static readonly OfficeChartType[] CHARTS_EXPLODED = new OfficeChartType[3]
  {
    OfficeChartType.Doughnut_Exploded,
    OfficeChartType.Pie_Exploded,
    OfficeChartType.Pie_Exploded_3D
  };
  private static readonly OfficeChartType[] CHART_SERIES_LINES = new OfficeChartType[2]
  {
    OfficeChartType.PieOfPie,
    OfficeChartType.Pie_Bar
  };
  public static readonly OfficeChartType[] CHARTS_SCATTER = new OfficeChartType[5]
  {
    OfficeChartType.Scatter_Markers,
    OfficeChartType.Scatter_Line_Markers,
    OfficeChartType.Scatter_Line,
    OfficeChartType.Scatter_SmoothedLine_Markers,
    OfficeChartType.Scatter_SmoothedLine
  };
  public static readonly OfficeChartType[] CHARTS_SMOOTHED_LINE = new OfficeChartType[2]
  {
    OfficeChartType.Scatter_SmoothedLine_Markers,
    OfficeChartType.Scatter_SmoothedLine
  };
  public static readonly OfficeChartType[] CHARTS_STOCK = new OfficeChartType[4]
  {
    OfficeChartType.Stock_HighLowClose,
    OfficeChartType.Stock_OpenHighLowClose,
    OfficeChartType.Stock_VolumeHighLowClose,
    OfficeChartType.Stock_VolumeOpenHighLowClose
  };
  public static readonly OfficeChartType[] CHARTS_PERSPECTIVE = new OfficeChartType[10]
  {
    OfficeChartType.Area_3D,
    OfficeChartType.Column_3D,
    OfficeChartType.Cone_Clustered_3D,
    OfficeChartType.Cylinder_Clustered_3D,
    OfficeChartType.Line_3D,
    OfficeChartType.Pyramid_Clustered_3D,
    OfficeChartType.Surface_3D,
    OfficeChartType.Surface_NoColor_3D,
    OfficeChartType.Surface_Contour,
    OfficeChartType.Surface_NoColor_Contour
  };
  public static readonly OfficeChartType[] CHARTS_CLUSTERED = new OfficeChartType[10]
  {
    OfficeChartType.Bar_Clustered,
    OfficeChartType.Bar_Clustered_3D,
    OfficeChartType.Column_Clustered,
    OfficeChartType.Column_Clustered_3D,
    OfficeChartType.Cone_Clustered,
    OfficeChartType.Cone_Bar_Clustered,
    OfficeChartType.Cylinder_Clustered,
    OfficeChartType.Cylinder_Bar_Clustered,
    OfficeChartType.Pyramid_Clustered,
    OfficeChartType.Pyramid_Bar_Clustered
  };
  public static readonly OfficeChartType[] CHARTS_WITH_PLOT_AREA = new OfficeChartType[26]
  {
    OfficeChartType.Column_Clustered,
    OfficeChartType.Scatter_Line_Markers,
    OfficeChartType.Scatter_SmoothedLine,
    OfficeChartType.Scatter_SmoothedLine_Markers,
    OfficeChartType.Line_Markers_Stacked_100,
    OfficeChartType.Stock_VolumeOpenHighLowClose,
    OfficeChartType.Line_Markers_Stacked,
    OfficeChartType.Stock_VolumeHighLowClose,
    OfficeChartType.Column_Stacked_100,
    OfficeChartType.Stock_OpenHighLowClose,
    OfficeChartType.Scatter_Line_Markers,
    OfficeChartType.Area_Stacked_100,
    OfficeChartType.Line_Stacked_100,
    OfficeChartType.Line_Markers,
    OfficeChartType.Stock_HighLowClose,
    OfficeChartType.Column_Stacked,
    OfficeChartType.Bar_Clustered,
    OfficeChartType.Bar_Stacked_100,
    OfficeChartType.Area_Stacked,
    OfficeChartType.Line_Stacked,
    OfficeChartType.Bar_Stacked,
    OfficeChartType.Bubble_3D,
    OfficeChartType.Scatter_Markers,
    OfficeChartType.Bubble,
    OfficeChartType.Area,
    OfficeChartType.Line
  };
  public static readonly OfficeLegendPosition[] LEGEND_VERTICAL = new OfficeLegendPosition[3]
  {
    OfficeLegendPosition.Right,
    OfficeLegendPosition.Corner,
    OfficeLegendPosition.Left
  };
  private static readonly byte[][] DEF_UNKNOWN_SERIE_LABEL = new byte[14][]
  {
    new byte[20]
    {
      (byte) 80 /*0x50*/,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 10,
      (byte) 10,
      (byte) 3,
      (byte) 0,
      (byte) 80 /*0x50*/,
      (byte) 8,
      (byte) 90,
      (byte) 8,
      (byte) 97,
      (byte) 8,
      (byte) 97,
      (byte) 8,
      (byte) 106,
      (byte) 8,
      (byte) 107,
      (byte) 8
    },
    new byte[12]
    {
      (byte) 82,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 13,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[12]
    {
      (byte) 82,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[12]
    {
      (byte) 82,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 5,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[12]
    {
      (byte) 106,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[12]
    {
      (byte) 84,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 18,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[12]
    {
      (byte) 81,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 36,
      (byte) 16 /*0x10*/,
      (byte) 2,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[40]
    {
      (byte) 81,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 37,
      (byte) 16 /*0x10*/,
      (byte) 32 /*0x20*/,
      (byte) 0,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 169,
      (byte) 254,
      byte.MaxValue,
      byte.MaxValue,
      (byte) 187,
      (byte) 254,
      byte.MaxValue,
      byte.MaxValue,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 177,
      (byte) 0,
      (byte) 77,
      (byte) 0,
      (byte) 80 /*0x50*/,
      (byte) 40,
      (byte) 0,
      (byte) 0
    },
    new byte[12]
    {
      (byte) 81,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 51,
      (byte) 16 /*0x10*/,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[28]
    {
      (byte) 81,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 79,
      (byte) 16 /*0x10*/,
      (byte) 20,
      (byte) 0,
      (byte) 2,
      (byte) 0,
      (byte) 2,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[16 /*0x10*/]
    {
      (byte) 81,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 81,
      (byte) 16 /*0x10*/,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 1,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[14]
    {
      (byte) 81,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 39,
      (byte) 16 /*0x10*/,
      (byte) 6,
      (byte) 0,
      (byte) 4,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[12]
    {
      (byte) 81,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 52,
      (byte) 16 /*0x10*/,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[12]
    {
      (byte) 85,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 18,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    }
  };
  public static readonly OfficeChartType[] DEF_SPECIAL_DATA_LABELS = new OfficeChartType[7]
  {
    OfficeChartType.Radar_Filled,
    OfficeChartType.Area,
    OfficeChartType.Area_3D,
    OfficeChartType.Area_Stacked,
    OfficeChartType.Area_Stacked_100,
    OfficeChartType.Area_Stacked_100_3D,
    OfficeChartType.Area_Stacked_3D
  };
  public static readonly OfficeChartType[] DEF_CHART_PERCENTAGE = new OfficeChartType[8]
  {
    OfficeChartType.Pie,
    OfficeChartType.Pie_3D,
    OfficeChartType.Pie_Bar,
    OfficeChartType.Pie_Exploded,
    OfficeChartType.Pie_Exploded_3D,
    OfficeChartType.PieOfPie,
    OfficeChartType.Doughnut,
    OfficeChartType.Doughnut_Exploded
  };
  private int m_linechartCount;
  private bool m_bParseDataOnDemand;
  private int m_iOverlap;
  private int m_gapWidth;
  private bool m_bShowGapWidth;
  private bool m_bIsSecondaryAxis;
  private bool m_bInWorksheet;
  internal OfficeChartType m_chartType;
  private OfficeChartType m_pivotChartType;
  private IRange m_dataRange;
  private bool m_bSeriesInRows;
  private bool m_bHasDataTable;
  private ChartPageSetupImpl m_pageSetup;
  private double m_dXPos;
  private double m_dYPos;
  private double m_dWidth;
  private double m_dHeight;
  private List<ChartFbiRecord> m_arrFonts;
  private ChartSeriesCollection m_series;
  private ChartCategoryCollection m_categories;
  private ChartDataTableImpl m_dataTable;
  private ChartShtpropsRecord m_chartProperties;
  private ChartPlotGrowthRecord m_plotGrowth;
  private ChartPosRecord m_plotAreaBoundingBox;
  private ChartFrameFormatImpl m_chartArea;
  private ChartFrameFormatImpl m_plotAreaFrame;
  private TypedSortedListEx<int, List<BiffRecordRaw>> m_lstDefaultText = new TypedSortedListEx<int, List<BiffRecordRaw>>();
  private ChartTextAreaImpl m_title;
  private ChartParentAxisImpl m_primaryParentAxis;
  private ChartParentAxisImpl m_secondaryParentAxis;
  private ChartLegendImpl m_legend;
  private bool m_bHasLegend;
  private ChartWallOrFloorImpl m_walls;
  private ChartWallOrFloorImpl m_sidewall;
  private ChartWallOrFloorImpl m_floor;
  private ChartPlotAreaImpl m_plotArea;
  private bool m_bTypeChanging;
  private OfficeChartType m_destinationType;
  private List<BiffRecordRaw> m_trendList = new List<BiffRecordRaw>();
  private List<BiffRecordRaw> m_pivotList;
  private WindowZoomRecord m_chartChartZoom;
  private RelationCollection m_relations;
  private int m_iStyle2007;
  private Stream m_pivotFormatsStream;
  private bool m_bZoomToFit;
  private Dictionary<int, List<BiffRecordRaw>> m_dictReparseErrorBars;
  private Stream m_bandFormats;
  private string m_preservedPivotSource;
  private int m_formatId;
  private bool m_showAllFieldButtons = true;
  private bool m_showAxisFieldButtons = true;
  private bool m_showValueFieldButtons = true;
  private bool m_showLegendFieldButtons = true;
  private bool m_showReportFilterFieldButtons = true;
  private Stream m_alternateContent;
  internal bool m_bHasChartTitle;
  internal bool m_bIsPrimarySecondaryCategory;
  internal bool m_bIsPrimarySecondaryValue;
  private Stream m_defaultTextProperty;
  private FontWrapper m_font;
  private bool? m_hasAutoTitle;
  private List<int> m_axisIds;
  private ChartPlotAreaLayoutRecord m_plotAreaLayout;
  private bool[] category;
  private bool[] series;
  private OfficeSeriesNameLevel m_seriesNameLevel;
  private OfficeCategoriesLabelLevel m_categoriesLabelLevel;
  private bool m_showPlotVisible;
  private string m_radarStyle;
  internal bool IsAddCopied;
  private object[] m_categoryLabelValues;
  private Syncfusion.OfficeChart.Implementation.Charts.ChartData _chartData;
  private string m_formula;
  internal MemoryStream m_themeOverrideStream;
  internal List<Color> m_themeColors;
  private bool m_isChartCleared;
  private int m_activeSheetIndex;
  internal MemoryStream m_colorMapOverrideStream;
  internal bool m_isChartStyleSkipped;
  internal bool m_isChartColorStyleSkipped;
  private Dictionary<string, Stream> m_relationPreservedStreamCollections;
  internal Dictionary<int, ChartDataPointsCollection> CommonDataPointsCollection;
  private bool m_hasExternalWorkbook;
  private ushort m_chartExTitlePosition;
  private bool m_chartTitleIncludeInLayout;
  private bool? m_isAutoUpdate;
  private string m_chartExRelationId;
  private IEnumerable<IGrouping<int, IOfficeChartSerie>> m_chartSerieGroupsBeforesorting;
  internal bool IsStock;

  internal ChartImpl()
  {
    IApplication excel = new ExcelEngine().Excel;
    excel.DefaultVersion = OfficeVersion.Excel2010;
    WorksheetImpl worksheet = (excel.Workbooks.Create(1) as WorkbookImpl).Worksheets[0] as WorksheetImpl;
    this.SetParent(excel, worksheet);
  }

  protected override void SetParent(IApplication application, WorksheetImpl workSheet)
  {
    base.SetParent(application, workSheet);
    this.SetParents();
  }

  internal void SetDefaultProperties()
  {
    this.m_bInWorksheet = this.FindParent(typeof (WorksheetBaseImpl), true) is WorksheetBaseImpl;
    if (!this.m_book.IsWorkbookOpening)
    {
      this.m_bHasChartTitle = true;
      this.CreateChartTitle();
    }
    ChartFormatImpl formatToAdd = new ChartFormatImpl(this.Application, (object) this.PrimaryFormats);
    this.PrimaryFormats.Add(formatToAdd, true);
    formatToAdd.ChangeChartType(OfficeChartType.Column_Clustered, false);
    if (!this.m_book.IsWorkbookOpening)
    {
      this.HasLegend = true;
      this.PrimaryValueAxis.HasMajorGridLines = true;
      this.m_chartProperties = (ChartShtpropsRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartShtprops);
      this.m_walls = new ChartWallOrFloorImpl(this.Application, (object) this, true);
      this.m_floor = new ChartWallOrFloorImpl(this.Application, (object) this, false);
      this.m_sidewall = new ChartWallOrFloorImpl(this.Application, (object) this, true);
      this.m_plotArea = new ChartPlotAreaImpl(this.Application, (object) this, this.ChartType);
    }
    else if (this.m_book.Version != OfficeVersion.Excel97to2003)
      this.m_chartProperties = (ChartShtpropsRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartShtprops);
    this.InitializeFrames();
  }

  internal bool HasExternalWorkbook
  {
    get => this.m_hasExternalWorkbook;
    set => this.m_hasExternalWorkbook = value;
  }

  public void Refresh()
  {
    this.DetectChartType();
    foreach (ChartSerieImpl chartSerieImpl in (IEnumerable<IOfficeChartSerie>) this.Series)
    {
      string name = (string) null;
      if (((ChartDataRange) chartSerieImpl.NameRange).Range != null)
        name = ((ChartDataRange) chartSerieImpl.NameRange).Range.AddressGlobal;
      if (name != null)
        chartSerieImpl.NameRangeIRange.Value2 = this.Workbook.ActiveSheet.Range[name].Value2;
      foreach (RangeImpl cell in chartSerieImpl.ValuesIRange.Cells)
      {
        string addressGlobal = cell.AddressGlobal;
        if (addressGlobal != null)
          cell.Value2 = this.Workbook.ActiveSheet.Range[addressGlobal].Value2;
      }
    }
    foreach (ChartCategory category in (IEnumerable<IOfficeChartCategory>) this.Categories)
    {
      if (category.CategoryLabelIRange != null)
      {
        foreach (RangeImpl cell in category.CategoryLabelIRange.Cells)
        {
          string addressGlobal = cell.AddressGlobal;
          if (addressGlobal != null)
            cell.Value2 = this.Workbook.ActiveSheet.Range[addressGlobal].Value2;
        }
      }
    }
    this.CategoryLabelValues = (object[]) null;
    foreach (ChartSerieImpl chartSerieImpl in (IEnumerable<IOfficeChartSerie>) this.Series)
      chartSerieImpl.EnteredDirectlyValues = (object[]) null;
  }

  protected internal ChartImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    this.m_bInWorksheet = this.FindParent(typeof (WorksheetBaseImpl), true) is WorksheetBaseImpl;
    if (!this.m_book.IsWorkbookOpening)
    {
      this.m_bHasChartTitle = true;
      this.CreateChartTitle();
    }
    ChartFormatImpl formatToAdd = new ChartFormatImpl(this.Application, (object) this.PrimaryFormats);
    this.PrimaryFormats.Add(formatToAdd, true);
    formatToAdd.ChangeChartType(OfficeChartType.Column_Clustered, false);
    if (!this.m_book.IsWorkbookOpening)
    {
      this.HasLegend = true;
      this.PrimaryValueAxis.HasMajorGridLines = true;
      this.m_chartProperties = (ChartShtpropsRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartShtprops);
      this.m_walls = new ChartWallOrFloorImpl(application, (object) this, true);
      this.m_floor = new ChartWallOrFloorImpl(application, (object) this, false);
      this.m_sidewall = new ChartWallOrFloorImpl(application, (object) this, true);
      this.m_plotArea = new ChartPlotAreaImpl(application, (object) this, this.ChartType);
    }
    else if (this.m_book.Version != OfficeVersion.Excel97to2003)
      this.m_chartProperties = (ChartShtpropsRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartShtprops);
    this.InitializeFrames();
  }

  [CLSCompliant(false)]
  protected internal ChartImpl(
    IApplication application,
    object parent,
    IList data,
    ref int iPos,
    OfficeParseOptions options)
    : this(application, parent)
  {
    this.m_arrRecords.Clear();
    this.m_parseOptions = options;
    this.GetChartRecords(data, ref iPos, this.m_arrRecords, options);
  }

  private void SetParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("Can't find parent workbook.");
  }

  private void CreateChartTitle()
  {
    this.m_title = new ChartTextAreaImpl(this.Application, (object) this, ExcelObjectTextLink.Chart);
    this.m_title.Text = !this.m_bHasChartTitle ? (string) null : "Chart Title";
    this.m_title.FontName = "Calibri";
    this.m_title.Size = 18.6;
    this.m_title.FrameFormat.Interior.UseAutomaticFormat = true;
    if (this.m_book == null || this.m_book.IsWorkbookOpening)
      return;
    this.m_title.Bold = true;
  }

  public override void Parse()
  {
    this.KeepRecord = true;
    base.Parse();
  }

  private void Parse(IList data, ref int iPos, OfficeParseOptions options)
  {
  }

  protected internal override void ParseData(Dictionary<int, int> dictUpdatedSSTIndexes)
  {
    this.SetDefaultValues();
    if ((this.m_parseOptions & OfficeParseOptions.DoNotParseCharts) != OfficeParseOptions.Default)
      return;
    if (this.m_dataHolder == null)
    {
      this.m_pivotList = new List<BiffRecordRaw>();
      int iPos = 0;
      bool flag = false;
      BiffRecordRaw arrRecord1 = this.m_arrRecords[iPos];
      int num = 0;
      Dictionary<int, int> newSeriesIndex = new Dictionary<int, int>();
      while (iPos < this.m_arrRecords.Count && !flag)
      {
        BiffRecordRaw arrRecord2 = this.m_arrRecords[iPos];
        if (arrRecord2.TypeCode == TBIFFRecord.BOF)
        {
          ++num;
          ++iPos;
        }
        else if (arrRecord2.TypeCode == TBIFFRecord.EOF)
        {
          --num;
          ++iPos;
          if (num == 0)
            flag = true;
        }
        else if (num == 1)
          this.ParseOrdinaryRecord(arrRecord2, ref iPos, newSeriesIndex);
        else
          ++iPos;
      }
      this.PrepareProtection();
      this.ReparseErrorBars(newSeriesIndex);
    }
    else
      this.m_dataHolder.ParseChartsheetData(this);
    this.IsParsed = true;
  }

  private void ReparseErrorBars(Dictionary<int, int> newSeriesIndex)
  {
    if (this.m_dictReparseErrorBars != null && this.m_dictReparseErrorBars.Count > 0)
    {
      foreach (KeyValuePair<int, List<BiffRecordRaw>> dictReparseErrorBar in this.m_dictReparseErrorBars)
      {
        int key = dictReparseErrorBar.Key;
        if (newSeriesIndex.ContainsKey(key))
          key = newSeriesIndex[key];
        List<BiffRecordRaw> data = dictReparseErrorBar.Value;
        ((ChartSerieImpl) this.m_series[key]).ParseErrorBars((IList<BiffRecordRaw>) data);
      }
    }
    this.m_dictReparseErrorBars = (Dictionary<int, List<BiffRecordRaw>>) null;
  }

  private void ParseOrdinaryRecord(
    BiffRecordRaw record,
    ref int iPos,
    Dictionary<int, int> newSeriesIndex)
  {
    switch (record.TypeCode)
    {
      case TBIFFRecord.Protect:
        this.ParseProtect((ProtectRecord) record);
        ++iPos;
        break;
      case TBIFFRecord.Password:
        this.ParsePassword((PasswordRecord) record);
        ++iPos;
        break;
      case TBIFFRecord.Header:
        this.m_pageSetup = new ChartPageSetupImpl(this.Application, (object) this, this.m_arrRecords, ref iPos);
        break;
      case TBIFFRecord.ObjectProtect:
        this.ParseObjectProtect((ObjectProtectRecord) record);
        ++iPos;
        break;
      case TBIFFRecord.WindowZoom:
        this.ParseWindowZoom((WindowZoomRecord) this.m_arrRecords[iPos++]);
        break;
      case TBIFFRecord.ScenProtect:
        this.ParseScenProtect((ScenProtectRecord) record);
        ++iPos;
        break;
      case TBIFFRecord.CodeName:
        this.m_strCodeName = ((CodeNameRecord) this.m_arrRecords[iPos]).CodeName;
        ++iPos;
        break;
      case TBIFFRecord.Dimensions:
        this.ParseDimensions((DimensionsRecord) this.m_arrRecords[iPos++]);
        break;
      case TBIFFRecord.WindowTwo:
        this.ParseWindowTwo((WindowTwoRecord) this.m_arrRecords[iPos++]);
        break;
      case TBIFFRecord.DCON | TBIFFRecord.QuickTip:
      case (TBIFFRecord) 2136:
      case TBIFFRecord.XCT | TBIFFRecord.QuickTip:
        this.m_pivotList.Add(record);
        ++iPos;
        break;
      case TBIFFRecord.ChartChart:
        this.ParseChart((IList<BiffRecordRaw>) this.m_arrRecords, ref iPos, newSeriesIndex);
        break;
      case TBIFFRecord.ChartFbi:
        this.ParseFonts((IList) this.m_arrRecords, ref iPos);
        break;
      case TBIFFRecord.ChartSiIndex:
        this.ParseSiIndex((IList<BiffRecordRaw>) this.m_arrRecords, ref iPos);
        break;
      default:
        ++iPos;
        break;
    }
  }

  private void GetChartRecords(
    IList data,
    ref int iPos,
    List<BiffRecordRaw> records,
    OfficeParseOptions options)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (iPos < 0 || iPos >= data.Count)
      throw new ArgumentOutOfRangeException(nameof (iPos), "Value cannot be less than 0 and greater than data.Count - 1");
    BiffRecordRaw biffRecordRaw1 = (BiffRecordRaw) data[iPos];
    if (biffRecordRaw1.TypeCode != TBIFFRecord.BOF)
      throw new ArgumentOutOfRangeException("BOF record was expected.");
    records.Add(biffRecordRaw1);
    ++iPos;
    BiffRecordRaw biffRecordRaw2 = (BiffRecordRaw) data[iPos];
    bool flag = false;
    if (flag)
    {
      if (!(this.FindParent(typeof (WorkbookImpl)) is WorkbookImpl parent))
        throw new ArgumentNullException("Can't find parent workbook");
      FontRecord record = ((FontImpl) parent.InnerFonts[0]).Record;
    }
    for (; biffRecordRaw2.TypeCode != TBIFFRecord.EOF; biffRecordRaw2 = (BiffRecordRaw) data[iPos])
    {
      if (!flag || flag && biffRecordRaw2.TypeCode != TBIFFRecord.ChartFbi)
        records.Add(biffRecordRaw2);
      if (biffRecordRaw2.TypeCode == TBIFFRecord.MSODrawing && this.m_iMsoStartIndex < 0)
        this.m_iMsoStartIndex = records.Count - 1;
      ++iPos;
    }
    records.Add((BiffRecordRaw) data[iPos]);
    ++iPos;
  }

  private void ParseFonts(IList data, ref int iPos)
  {
    BiffRecordRaw biffRecordRaw = (BiffRecordRaw) data[iPos];
    if (biffRecordRaw.TypeCode != TBIFFRecord.ChartFbi)
      throw new ArgumentOutOfRangeException("ChartFbi record was expected.");
    for (; biffRecordRaw.TypeCode == TBIFFRecord.ChartFbi; biffRecordRaw = (BiffRecordRaw) data[iPos])
    {
      this.m_arrFonts.Add((ChartFbiRecord) biffRecordRaw);
      ++iPos;
    }
  }

  private void ParseChart(
    IList<BiffRecordRaw> data,
    ref int iPos,
    Dictionary<int, int> newSeriesIndex)
  {
    BiffRecordRaw chart = data != null ? data[iPos] : throw new ArgumentNullException(nameof (data));
    chart.CheckTypeCode(TBIFFRecord.ChartChart);
    this.FillDataFromChartRecord((ChartChartRecord) chart);
    ++iPos;
    data[iPos].CheckTypeCode(TBIFFRecord.Begin);
    ++iPos;
    int num = 0;
    int count1 = data.Count;
    this.m_series.TrendIndex = 0;
    BiffRecordRaw biffRecordRaw = data[iPos];
    List<ChartTextAreaImpl> chartTextAreaImplList = (List<ChartTextAreaImpl>) null;
    for (; biffRecordRaw.TypeCode != TBIFFRecord.End || num != 0; biffRecordRaw = data[iPos])
    {
      switch (biffRecordRaw.TypeCode)
      {
        case TBIFFRecord.WindowZoom:
          this.m_chartChartZoom = (WindowZoomRecord) biffRecordRaw;
          ++iPos;
          break;
        case TBIFFRecord.ChartWrapper:
          if (((ChartWrapperRecord) biffRecordRaw).Record.TypeCode == TBIFFRecord.ChartText)
          {
            this.AssignTextArea((ChartTextAreaImpl) new ChartWrappedTextAreaImpl(this.Application, (object) this, data, ref iPos), newSeriesIndex);
            break;
          }
          ++iPos;
          break;
        case TBIFFRecord.PlotAreaLayout:
          this.m_plotAreaLayout = (ChartPlotAreaLayoutRecord) biffRecordRaw;
          ++iPos;
          break;
        case TBIFFRecord.ChartSeries:
          this.ParseSeriesOrErrorBars(data, ref iPos, newSeriesIndex);
          break;
        case TBIFFRecord.ChartDefaultText:
          this.ParseDefaultText(data, ref iPos);
          break;
        case TBIFFRecord.ChartText:
          List<ChartTextAreaImpl> collection = this.AssignTextArea(this.ParseText(data, ref iPos), newSeriesIndex);
          if (chartTextAreaImplList == null)
          {
            chartTextAreaImplList = collection;
            break;
          }
          chartTextAreaImplList.AddRange((IEnumerable<ChartTextAreaImpl>) collection);
          break;
        case TBIFFRecord.ChartFrame:
          this.InnerChartArea.Parse(data, ref iPos);
          break;
        case TBIFFRecord.Begin:
          ++num;
          ++iPos;
          break;
        case TBIFFRecord.End:
          --num;
          ++iPos;
          break;
        case TBIFFRecord.ChartAxisParent:
          this.ParseAxisParent(data, ref iPos);
          break;
        case TBIFFRecord.ChartShtprops:
          this.ParseSheetProperties(data, ref iPos);
          break;
        case TBIFFRecord.ChartAxesUsed:
          this.ParseAxesUsed(data, ref iPos);
          break;
        case TBIFFRecord.ChartDat:
          this.ParseDataTable(data, ref iPos);
          break;
        case TBIFFRecord.ChartPlotGrowth:
          this.ParsePlotGrowth((ChartPlotGrowthRecord) data[iPos++]);
          break;
        default:
          ++iPos;
          break;
      }
      if (iPos == count1)
        break;
    }
    ++iPos;
    if (this.m_chartProperties == null)
      this.m_chartProperties = (ChartShtpropsRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartShtprops);
    this.UpdateChartTitle();
    this.DetectIsInRowOnParsing();
    this.ChangePrimaryAxis(true);
    this.DetectChartType();
    this.ReparseTrendLegends();
    int count2 = chartTextAreaImplList != null ? chartTextAreaImplList.Count : 0;
    if (this.DataRange != null)
    {
      IRange range = (IRange) null;
      this.IsSeriesInRows = this.DetectIsInRow((this.Series[0].Values as ChartDataRange).Range);
      this.GetSerieOrAxisRange((this.DataRange as ChartDataRange).Range, this.IsSeriesInRows, out range);
      this.GetSerieOrAxisRange(range, !this.IsSeriesInRows, out range);
      int count3 = range.Count / (this.Series[0].Values as ChartDataRange).Range.Count;
      for (int index = 0; index < (this.Series[0].Values as ChartDataRange).Range.Count; ++index)
      {
        IRange categoryRange = ChartImpl.GetCategoryRange(range, out range, count3, this.IsSeriesInRows);
        if (this.Categories.Count > 0)
        {
          (this.Categories[index] as ChartCategory).CategoryLabelIRange = (this.Series[0].CategoryLabels as ChartDataRange).Range;
          (this.Categories[index] as ChartCategory).ValuesIRange = categoryRange;
          if (this.Categories[0].CategoryLabel != null)
          {
            (this.Categories[index] as ChartCategory).Name = (this.Categories[0].CategoryLabel as ChartDataRange).Range.Cells[index].Text;
          }
          else
          {
            (this.Categories[index] as ChartCategory).Name = (index + 1).ToString();
            if (this.Legend != null && this.Legend.LegendEntries != null && this.Legend.LegendEntries[index].TextArea != null)
              this.Legend.LegendEntries[index].TextArea.Text = (this.Categories[index] as ChartCategory).Name;
          }
        }
        else if (this.Legend != null && this.Legend.LegendEntries != null && this.Legend.LegendEntries[index].TextArea != null)
          this.Legend.LegendEntries[index].TextArea.Text = (index + 1).ToString();
      }
    }
    else if (this.Series != null && this.Series.Count > 0 && this.Series[0].Values != null)
    {
      for (int index = 0; index < (this.Series[0].Values as ChartDataRange).Range.Count; ++index)
      {
        if (index < this.Categories.Count)
        {
          (this.Categories[index] as ChartCategory).CategoryLabelIRange = (this.Series[0].CategoryLabels as ChartDataRange).Range;
          (this.Categories[index] as ChartCategory).ValuesIRange = (this.Series[0].Values as ChartDataRange).Range;
          if (this.Categories[0].CategoryLabel != null && this.Categories[0].CategoryLabel.GetType() != typeof (ExternalRange) && (this.Categories[0].CategoryLabel as ChartDataRange).Range.Worksheet != null && index < (this.Categories[0].CategoryLabel as ChartDataRange).Range.Cells.Length)
            (this.Categories[index] as ChartCategory).Name = (this.Categories[0].CategoryLabel as ChartDataRange).Range.Cells[index].Text;
        }
      }
    }
    for (int index = 0; index < count2; ++index)
      this.m_series.AssignTrendDataLabel(chartTextAreaImplList[index]);
    if (this.Series.Count <= 0 || this.Series[0].SerieType != OfficeChartType.Bubble)
      return;
    this.CheckIsBubble3D();
  }

  private void FillDataFromChartRecord(ChartChartRecord chart)
  {
    this.XPos = ChartImpl.FixedPointToDouble(chart.X);
    this.YPos = ChartImpl.FixedPointToDouble(chart.Y);
    this.EMUWidth = ChartImpl.FixedPointToDouble(chart.Width);
    this.EMUHeight = ChartImpl.FixedPointToDouble(chart.Height);
  }

  private void ParsePlotGrowth(ChartPlotGrowthRecord plotGrowth)
  {
    this.m_plotGrowth = plotGrowth != null ? plotGrowth : throw new ArgumentNullException(nameof (plotGrowth));
  }

  private void ParseSiIndex(IList<BiffRecordRaw> data, ref int iPos)
  {
    this.m_series.ParseSiIndex(data, ref iPos);
  }

  private void ParseSeriesOrErrorBars(
    IList<BiffRecordRaw> data,
    ref int iPos,
    Dictionary<int, int> newSeriesIndexes)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    List<BiffRecordRaw> biffRecordRawList = new List<BiffRecordRaw>();
    int serieIndex = 0;
    bool bIsErrorBars = false;
    if (this.AddRecords(data, (IList<BiffRecordRaw>) biffRecordRawList, ref iPos, ref serieIndex, ref bIsErrorBars))
    {
      int iPos1 = 0;
      this.m_series.Add(new ChartSerieImpl(this.Application, (object) this.m_series, (IList<BiffRecordRaw>) biffRecordRawList, ref iPos1));
      newSeriesIndexes[serieIndex] = this.m_series.Count - 1;
    }
    else if (bIsErrorBars)
    {
      if (newSeriesIndexes.ContainsKey(serieIndex))
        serieIndex = newSeriesIndexes[serieIndex];
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.m_series[serieIndex];
      if (chartSerieImpl != null)
      {
        chartSerieImpl.ParseErrorBars((IList<BiffRecordRaw>) biffRecordRawList);
      }
      else
      {
        if (this.m_dictReparseErrorBars == null)
          this.m_dictReparseErrorBars = new Dictionary<int, List<BiffRecordRaw>>();
        this.m_dictReparseErrorBars.Add(serieIndex, biffRecordRawList);
      }
    }
    else
    {
      int index = 0;
      for (int count = biffRecordRawList.Count; index < count; ++index)
        this.m_trendList.Add(biffRecordRawList[index]);
    }
  }

  private void ParseSheetProperties(IList<BiffRecordRaw> data, ref int iPos)
  {
    BiffRecordRaw biffRecordRaw = data != null ? data[iPos] : throw new ArgumentNullException(nameof (data));
    biffRecordRaw.CheckTypeCode(TBIFFRecord.ChartShtprops);
    this.m_chartProperties = (ChartShtpropsRecord) biffRecordRaw.Clone();
    ++iPos;
  }

  private void ParseDefaultText(IList<BiffRecordRaw> data, ref int iPos)
  {
    BiffRecordRaw biffRecordRaw = data != null ? data[iPos] : throw new ArgumentNullException(nameof (data));
    biffRecordRaw.CheckTypeCode(TBIFFRecord.ChartDefaultText);
    int textCharacteristics = (int) ((ChartDefaultTextRecord) biffRecordRaw.Clone()).TextCharacteristics;
    ++iPos;
    List<BiffRecordRaw> textData = this.GetTextData(data, ref iPos);
    this.m_lstDefaultText[textCharacteristics] = textData;
  }

  private List<BiffRecordRaw> GetTextData(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (iPos < 0 || iPos > data.Count)
      throw new ArgumentOutOfRangeException(nameof (iPos), "Value cannot be less than 0 and greater than data.Length");
    BiffRecordRaw biffRecordRaw1 = data[iPos];
    biffRecordRaw1.CheckTypeCode(TBIFFRecord.ChartText);
    List<BiffRecordRaw> textData = new List<BiffRecordRaw>();
    textData.Add(biffRecordRaw1);
    ++iPos;
    BiffRecordRaw biffRecordRaw2 = data[iPos];
    biffRecordRaw2.CheckTypeCode(TBIFFRecord.Begin);
    textData.Add(biffRecordRaw2);
    BiffRecordRaw biffRecordRaw3;
    do
    {
      ++iPos;
      biffRecordRaw3 = data[iPos];
      textData.Add(biffRecordRaw3);
    }
    while (biffRecordRaw3.TypeCode != TBIFFRecord.End);
    ++iPos;
    return textData;
  }

  private ChartTextAreaImpl ParseText(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    data[iPos].CheckTypeCode(TBIFFRecord.ChartText);
    return new ChartTextAreaImpl(this.Application, (object) this, data, ref iPos);
  }

  private void ParseAxesUsed(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    data[iPos].CheckTypeCode(TBIFFRecord.ChartAxesUsed);
    this.m_primaryParentAxis.Formats.PrimaryFormats.Clear();
    this.m_primaryParentAxis.Formats.SecondaryFormats.Clear();
    ++iPos;
  }

  private void ParseAxisParent(IList<BiffRecordRaw> data, ref int iPos)
  {
    BiffRecordRaw biffRecordRaw = data != null ? data[iPos] : throw new ArgumentNullException(nameof (data));
    biffRecordRaw.CheckTypeCode(TBIFFRecord.ChartAxisParent);
    switch (((ChartAxisParentRecord) biffRecordRaw.Clone()).AxesIndex)
    {
      case 0:
        this.m_primaryParentAxis.Parse(data, ref iPos);
        break;
      case 1:
        this.m_secondaryParentAxis.Parse(data, ref iPos);
        break;
      default:
        throw new ArgumentOutOfRangeException("Axes index must be 0 or 1.");
    }
  }

  private void ParseDataTable(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    data[iPos].CheckTypeCode(TBIFFRecord.ChartDat);
    this.m_bHasDataTable = true;
    this.m_dataTable = new ChartDataTableImpl(this.Application, (object) this, data, ref iPos);
  }

  private List<ChartTextAreaImpl> AssignTextArea(
    ChartTextAreaImpl textArea,
    Dictionary<int, int> newSeriesIndexes)
  {
    ChartObjectLinkRecord objectLinkRecord = textArea != null ? textArea.ObjectLink : throw new ArgumentNullException(nameof (textArea));
    if (objectLinkRecord == null)
      throw new ArgumentNullException("objectLink");
    List<ChartTextAreaImpl> chartTextAreaImplList = (List<ChartTextAreaImpl>) null;
    switch (objectLinkRecord.LinkObject)
    {
      case ExcelObjectTextLink.Chart:
        this.m_title = textArea;
        break;
      case ExcelObjectTextLink.YAxis:
        this.ValueAxisTitle = textArea.Text;
        break;
      case ExcelObjectTextLink.XAxis:
        this.CategoryAxisTitle = textArea.Text;
        break;
      case ExcelObjectTextLink.DataLabel:
        int num = (int) objectLinkRecord.SeriesNumber;
        int dataPointNumber = (int) objectLinkRecord.DataPointNumber;
        if (newSeriesIndexes.ContainsKey(num))
          num = newSeriesIndexes[num];
        if (num >= this.m_series.Count)
        {
          if (chartTextAreaImplList == null)
            chartTextAreaImplList = new List<ChartTextAreaImpl>();
          chartTextAreaImplList.Add(textArea);
          break;
        }
        ((ChartDataPointImpl) ((ChartSerieImpl) this.m_series[num]).DataPoints[dataPointNumber]).SetDataLabels(textArea);
        break;
      case ExcelObjectTextLink.ZAxis:
        this.SeriesAxisTitle = textArea.Text;
        break;
    }
    return chartTextAreaImplList;
  }

  private void ParsePlotArea(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    data[iPos].CheckTypeCode(TBIFFRecord.ChartPlotArea);
    ++iPos;
    data[iPos].CheckTypeCode(TBIFFRecord.ChartFrame);
    this.InnerPlotArea.Parse(data, ref iPos);
  }

  private void ParseChartFrame(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    data[iPos].CheckTypeCode(TBIFFRecord.ChartFrame);
    ++iPos;
    int num1 = 0;
    if (data[iPos].TypeCode != TBIFFRecord.Begin)
      return;
    int num2 = num1 + 1;
    ++iPos;
    while (num2 != 0)
    {
      switch (data[iPos].TypeCode)
      {
        case TBIFFRecord.Begin:
          ++num2;
          break;
        case TBIFFRecord.End:
          --num2;
          break;
      }
      ++iPos;
    }
  }

  public void DetectChartType()
  {
    if (this.Series.Count == 0)
      return;
    this.m_chartType = this.m_primaryParentAxis.Formats.DetectChartType(this.m_series);
  }

  private void SetDefaultValues()
  {
    this.m_plotAreaFrame = (ChartFrameFormatImpl) null;
    this.m_chartArea = (ChartFrameFormatImpl) null;
  }

  private IOfficeFont ParseFontx(ChartFontxRecord fontx)
  {
    if (fontx == null)
      throw new ArgumentNullException(nameof (fontx));
    return (IOfficeFont) new FontWrapper(this.m_book.InnerFonts[(int) fontx.FontIndex] as FontImpl);
  }

  public void ParseLegend(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentException(nameof (data));
    this.HasLegend = true;
    this.m_legend.Parse(data, ref iPos);
  }

  private bool AddRecords(
    IList<BiffRecordRaw> list,
    IList<BiffRecordRaw> holder,
    ref int iPos,
    ref int serieIndex,
    ref bool bIsErrorBars)
  {
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    bool flag = true;
    bIsErrorBars = false;
    BiffRecordRaw biffRecordRaw1 = list[iPos];
    biffRecordRaw1.CheckTypeCode(TBIFFRecord.ChartSeries);
    ++iPos;
    holder.Add(biffRecordRaw1);
    BiffRecordRaw biffRecordRaw2 = list[iPos];
    biffRecordRaw2.CheckTypeCode(TBIFFRecord.Begin);
    ++iPos;
    holder.Add(biffRecordRaw2);
    int num = 1;
    while (num > 0)
    {
      BiffRecordRaw biffRecordRaw3 = list[iPos];
      holder.Add(biffRecordRaw3);
      switch (biffRecordRaw3.TypeCode)
      {
        case TBIFFRecord.ChartDataFormat:
          ChartDataFormatRecord dataFormatRecord = (ChartDataFormatRecord) biffRecordRaw3;
          serieIndex = (int) dataFormatRecord.SeriesIndex;
          break;
        case TBIFFRecord.Begin:
          ++num;
          break;
        case TBIFFRecord.End:
          --num;
          break;
        case TBIFFRecord.ChartSerParent:
          serieIndex = (int) ((ChartSerParentRecord) biffRecordRaw3).Series - 1;
          break;
        case TBIFFRecord.ChartSerAuxTrend:
          flag = false;
          break;
        case TBIFFRecord.ChartSerAuxErrBar:
          flag = false;
          bIsErrorBars = true;
          break;
      }
      ++iPos;
    }
    return flag;
  }

  private void ReparseTrendLegends()
  {
    ChartLegendEntriesColl legendEntries = this.HasLegend ? (ChartLegendEntriesColl) this.m_legend.LegendEntries : (ChartLegendEntriesColl) null;
    int iPos = 0;
    for (int count = this.m_trendList.Count; iPos < count; ++iPos)
    {
      int seriesIndex = this.FindSeriesIndex(this.m_trendList, iPos);
      ChartTrendLineCollection trendLines = (ChartTrendLineCollection) ((ChartSerieImpl) this.m_series[seriesIndex]).TrendLines;
      ChartLegendEntryImpl entry;
      ChartTrendLineImpl trend = new ChartTrendLineImpl(this.Application, (object) trendLines, (IList<BiffRecordRaw>) this.m_trendList, ref iPos, out entry);
      trendLines.Add(trend);
      if (legendEntries != null && entry != null)
      {
        int legendEntryOffset = this.m_series.GetLegendEntryOffset(seriesIndex);
        legendEntries.UpdateEntries(legendEntryOffset, 1);
        legendEntries.Add(legendEntryOffset, entry);
      }
    }
  }

  private int FindSeriesIndex(List<BiffRecordRaw> m_trendList, int i)
  {
    int seriesIndex = -1;
    for (int count = m_trendList.Count; i < count; ++i)
    {
      BiffRecordRaw mTrend = m_trendList[i];
      if (mTrend.TypeCode == TBIFFRecord.ChartSerParent)
      {
        seriesIndex = (int) ((ChartSerParentRecord) mTrend).Series - 1;
        break;
      }
    }
    return seriesIndex;
  }

  [CLSCompliant(false)]
  public override void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    bool isChartEx = ChartImpl.IsChartExSerieType(this.ChartType);
    int count = this.m_arrRecords.Count;
    this.m_bof.Type = BOFRecord.TType.TYPE_CHART;
    this.m_bof.IsNested = this.FindParent(typeof (WorksheetImpl)) != null;
    if (this.m_arrRecords.Count > 0)
    {
      records.AddList((IList) this.m_arrRecords);
    }
    else
    {
      records.Add((IBiffStorage) this.m_bof);
      this.SerializeHeaderFooterPictures(records);
      this.m_pageSetup.Serialize(records);
      this.SerializeFonts(records);
      if (this.m_bInWorksheet)
        records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.Protect));
      else
        this.SerializeProtection(records, true);
      if (!isChartEx)
        this.SerializeMsoDrawings(records);
      if (this.m_pivotList != null && this.m_pivotList.Count > 0)
        records.AddRange((ICollection) this.m_pivotList);
      records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.ChartUnits));
      this.SerializeChart(records, isChartEx);
      if (!isChartEx)
      {
        DimensionsRecord record = (DimensionsRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Dimensions);
        record.LastColumn = (ushort) (this.m_series.TrendErrorBarIndex + 1);
        IRange range = this.m_series.Count > 0 ? (this.m_series[0].Values as ChartDataRange).Range : (IRange) null;
        record.LastRow = range != null ? (this.m_series[0].Values as ChartDataRange).Range.Count : 0;
        records.Add((IBiffStorage) record);
        this.SerializeChartSiIndexes(records);
      }
      if (!this.m_bInWorksheet)
      {
        this.SerializeWindowTwo(records);
        this.SerializeWindowZoom(records);
        this.SerializeSheetLayout(records);
      }
      this.SerializeMacrosSupport(records);
      records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.EOF));
    }
  }

  private void SerializeFonts(OffsetArrayList records) => records.AddList((IList) this.m_arrFonts);

  private void SerializeChart(OffsetArrayList records, bool isChartEx)
  {
    ChartChartRecord record1 = (ChartChartRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartChart);
    record1.X = ChartImpl.DoubleToFixedPoint(this.XPos);
    record1.Y = ChartImpl.DoubleToFixedPoint(this.YPos);
    record1.Width = ChartImpl.DoubleToFixedPoint(this.EMUWidth);
    record1.Height = ChartImpl.DoubleToFixedPoint(this.EMUHeight);
    if (record1.Width == 0)
    {
      record1.Width = 48027384;
      record1.Height = 29506896;
    }
    records.Add((IBiffStorage) record1);
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
    if (this.m_chartChartZoom != null)
    {
      records.Add((IBiffStorage) this.m_chartChartZoom);
    }
    else
    {
      WindowZoomRecord record2 = (WindowZoomRecord) BiffRecordFactory.GetRecord(TBIFFRecord.WindowZoom);
      record2.NumMagnification = (ushort) 1;
      record2.DenumMagnification = (ushort) 1;
      records.Add((IBiffStorage) record2);
    }
    records.Add((IBiffStorage) this.PlotGrowth.Clone());
    if (this.m_chartArea != null)
      this.m_chartArea.Serialize((IList<IBiffStorage>) records);
    if (!isChartEx)
      this.m_series.Serialize(records);
    this.SerializeSheetProperties(records);
    this.SerializeDefaultText(records);
    this.SerializeAxes(records);
    if (this.m_plotAreaLayout != null)
      records.Add((IBiffStorage) this.m_plotAreaLayout.Clone());
    if (!isChartEx)
    {
      records.AddRange((ICollection<IBiffStorage>) this.m_series.TrendLabels);
      this.SerializeDataTable(records);
      if (this.HasTitleInternal)
        this.m_title.Serialize((IList<IBiffStorage>) records);
      this.SerializeDataLabels(records);
    }
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.End));
  }

  private void SerializeDefaultText(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    int index = 0;
    for (int count = this.m_lstDefaultText.Count; index < count; ++index)
    {
      int key = this.m_lstDefaultText.GetKey(index);
      List<BiffRecordRaw> byIndex = this.m_lstDefaultText.GetByIndex(index);
      ChartDefaultTextRecord record = (ChartDefaultTextRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartDefaultText);
      record.TextCharacteristics = (ChartDefaultTextRecord.TextDefaults) key;
      records.Add((IBiffStorage) record);
      records.AddList((IList) byIndex);
    }
  }

  private void SerializeAxes(OffsetArrayList records)
  {
    ChartAxesUsedRecord record = (ChartAxesUsedRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAxesUsed);
    int count = this.SecondaryFormats.Count;
    record.NumberOfAxes = count > 0 ? (ushort) 2 : (ushort) 1;
    records.Add((IBiffStorage) record);
    this.m_primaryParentAxis.Serialize(records);
    if (count <= 0)
      return;
    this.m_secondaryParentAxis.Serialize(records);
  }

  private void SerializeSheetProperties(OffsetArrayList records)
  {
    records.Add((IBiffStorage) this.m_chartProperties.Clone());
  }

  private void SerializeChartSiIndexes(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    ChartSiIndexRecord record = (ChartSiIndexRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartSiIndex);
    record.NumIndex = (ushort) 2;
    records.Add((IBiffStorage) record);
    this.SerializeChartSiMembers(records, 2);
    this.InsertSeriesLabels(records);
    ChartSiIndexRecord chartSiIndexRecord1 = (ChartSiIndexRecord) record.Clone();
    chartSiIndexRecord1.NumIndex = (ushort) 1;
    records.Add((IBiffStorage) chartSiIndexRecord1);
    this.SerializeChartSiMembers(records, 1);
    ChartSiIndexRecord chartSiIndexRecord2 = (ChartSiIndexRecord) chartSiIndexRecord1.Clone();
    chartSiIndexRecord2.NumIndex = (ushort) 3;
    records.Add((IBiffStorage) chartSiIndexRecord2);
    this.SerializeChartSiMembers(records, 3);
  }

  private void InsertSeriesLabels(OffsetArrayList records)
  {
  }

  private void InsertSeriesValues(OffsetArrayList records)
  {
    NumberRecord numberRecord = (NumberRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Number);
    for (int index = 0; index < this.Series.Count; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.Series[index];
      int num = 0;
      foreach (IRange cell in chartSerieImpl.ValuesIRange.Cells)
      {
        numberRecord = (NumberRecord) numberRecord.Clone();
        numberRecord.Row = (int) (ushort) num;
        numberRecord.Column = (int) (ushort) index;
        numberRecord.Value = cell.Number;
        numberRecord.ExtendedFormatIndex = (ushort) 0;
        records.Add((IBiffStorage) numberRecord);
        ++num;
      }
    }
  }

  private void SerializeDataTable(OffsetArrayList records)
  {
    if (!this.HasDataTable)
      return;
    this.m_dataTable.Serialize(records);
  }

  private void SerializeDataLabels(OffsetArrayList records)
  {
    this.m_series.SerializeDataLabels(records);
  }

  private void SerializeSeriesList(OffsetArrayList records)
  {
    if (!this.IsChartVolume)
      return;
    ChartSeriesListRecord record = (ChartSeriesListRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartSeriesList);
    if (this.ChartType == OfficeChartType.Stock_VolumeHighLowClose)
      record.Series = new ushort[4]
      {
        (ushort) 1,
        (ushort) 2,
        (ushort) 3,
        (ushort) 4
      };
    else if (this.ChartType == OfficeChartType.Stock_VolumeOpenHighLowClose)
      record.Series = new ushort[5]
      {
        (ushort) 1,
        (ushort) 2,
        (ushort) 3,
        (ushort) 4,
        (ushort) 5
      };
    records.Add((IBiffStorage) record);
  }

  private void SerializeChartSiMembers(OffsetArrayList records, int siIndex)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    List<BiffRecordRaw> biffRecordRawList = siIndex <= 3 && siIndex >= 1 ? this.m_series.GetEnteredRecords(siIndex) : throw new ArgumentOutOfRangeException(nameof (siIndex));
    if (biffRecordRawList == null || biffRecordRawList.Count <= 0)
      return;
    records.AddList((IList) biffRecordRawList);
  }

  [CLSCompliant(false)]
  public void SerializeLegend(OffsetArrayList records)
  {
    if (this.m_legend == null)
      return;
    this.m_legend.Serialize(records);
  }

  [CLSCompliant(false)]
  public void SerializeWalls(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_walls == null)
      return;
    this.m_walls.Serialize(records);
  }

  [CLSCompliant(false)]
  public void SerializeFloor(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_floor == null)
      return;
    this.m_floor.Serialize(records);
  }

  [CLSCompliant(false)]
  public void SerializePlotArea(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_plotArea == null)
      return;
    this.m_plotArea.Serialize((IList<IBiffStorage>) records);
  }

  internal int OverLap
  {
    get => this.m_iOverlap;
    set => this.m_iOverlap = value;
  }

  internal int GapWidth
  {
    get => this.m_gapWidth;
    set => this.m_gapWidth = value;
  }

  internal bool ShowGapWidth
  {
    get => this.m_bShowGapWidth;
    set => this.m_bShowGapWidth = value;
  }

  public int Rotation
  {
    get => this.ChartFormat.Rotation;
    set => this.ChartFormat.Rotation = value;
  }

  public int Elevation
  {
    get => this.ChartFormat.Elevation;
    set => this.ChartFormat.Elevation = value;
  }

  public int Perspective
  {
    get => this.ChartFormat.Perspective;
    set => this.ChartFormat.Perspective = value;
  }

  internal OfficeChartType PivotChartType
  {
    get => this.m_pivotChartType;
    set
    {
      if (this.Series.Count != 0)
        this.ChartType = value;
      this.m_pivotChartType = value;
      this.CreateNecessaryAxes(true);
    }
  }

  public string PreservedPivotSource
  {
    get => this.m_preservedPivotSource;
    set => this.m_preservedPivotSource = value;
  }

  public int FormatId
  {
    get => this.m_formatId;
    set => this.m_formatId = value;
  }

  internal bool ShowAllFieldButtons
  {
    get => this.m_showAllFieldButtons;
    set => this.m_showAllFieldButtons = value;
  }

  internal bool ShowValueFieldButtons
  {
    get => this.m_showValueFieldButtons;
    set => this.m_showValueFieldButtons = value;
  }

  internal bool ShowAxisFieldButtons
  {
    get => this.m_showAxisFieldButtons;
    set => this.m_showAxisFieldButtons = value;
  }

  internal bool ShowLegendFieldButtons
  {
    get => this.m_showLegendFieldButtons;
    set => this.m_showLegendFieldButtons = value;
  }

  internal bool ShowReportFilterFieldButtons
  {
    get => this.m_showReportFilterFieldButtons;
    set => this.m_showReportFilterFieldButtons = value;
  }

  public int HeightPercent
  {
    get => this.ChartFormat.HeightPercent;
    set => this.ChartFormat.HeightPercent = value;
  }

  public int DepthPercent
  {
    get => this.ChartFormat.DepthPercent;
    set => this.ChartFormat.DepthPercent = value;
  }

  public int GapDepth
  {
    get => this.ChartFormat.GapDepth;
    set => this.ChartFormat.GapDepth = value;
  }

  public bool RightAngleAxes
  {
    get => this.ChartFormat.RightAngleAxes;
    set => this.ChartFormat.RightAngleAxes = value;
  }

  public bool AutoScaling
  {
    get => this.ChartFormat.AutoScaling;
    set => this.ChartFormat.AutoScaling = value;
  }

  public bool WallsAndGridlines2D
  {
    get => this.ChartFormat.WallsAndGridlines2D;
    set => this.ChartFormat.WallsAndGridlines2D = value;
  }

  public OfficeChartType ChartType
  {
    get
    {
      this.DetectChartType();
      return this.m_chartType;
    }
    set
    {
      if (!this.m_book.IsWorkbookOpening)
        this.m_radarStyle = (string) null;
      this.ChangeChartType(value, false);
    }
  }

  public OfficeSeriesNameLevel SeriesNameLevel
  {
    get => this.m_seriesNameLevel;
    set => this.m_seriesNameLevel = value;
  }

  public OfficeCategoriesLabelLevel CategoryLabelLevel
  {
    get => this.m_categoriesLabelLevel;
    set => this.m_categoriesLabelLevel = value;
  }

  public IOfficeDataRange DataRange
  {
    get
    {
      return (IOfficeDataRange) new ChartDataRange((IOfficeChart) this)
      {
        Range = this.DataIRange
      };
    }
    set
    {
      this.DataIRange = this.Workbook.Worksheets[0][value.FirstRow, value.FirstColumn, value.LastRow, value.LastColumn];
    }
  }

  public IRange DataIRange
  {
    get
    {
      if (this.m_dataRange == null)
        this.m_dataRange = this.DetectDataRange();
      return this.m_dataRange;
    }
    set
    {
      if (this.m_dataRange == value)
        return;
      OfficeChartType chartType = this.ChartType;
      this.m_dataRange = value;
      this.OnDataRangeChanged(chartType);
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.m_series[0];
      this.UpdateSeries(this.m_series);
      this.Categories.Clear();
      this.UpdateCategory(this.m_series, true);
      if (chartSerieImpl.NumRefFormula == null && chartSerieImpl.StrRefFormula == null)
        return;
      chartSerieImpl.NumRefFormula = (string) null;
      chartSerieImpl.StrRefFormula = (string) null;
    }
  }

  private void DetectAndUpdateDataRangeForChartEx(OfficeChartType type)
  {
    MigrantRangeImpl tempRange = new MigrantRangeImpl(this.Application, this.m_dataRange.Worksheet);
    IRange range = (IRange) null;
    IRange outputSerieNameRange = (IRange) null;
    IRange outputAxisRange = (IRange) null;
    bool isSeriesInRows = false;
    bool isAnySpace = false;
    if (this.m_dataRange.Columns.Length == 1 && this.m_dataRange.Rows.Length == 1)
    {
      range = this.m_dataRange;
    }
    else
    {
      int rangeForChartEx1 = this.GetRangeForChartEx(tempRange, this.m_dataRange.LastRow, this.m_dataRange.Row, this.m_dataRange.LastColumn, true);
      int rangeForChartEx2 = this.GetRangeForChartEx(tempRange, this.m_dataRange.LastColumn, this.m_dataRange.Column, this.m_dataRange.LastRow, false);
      isAnySpace = this.m_dataRange[this.m_dataRange.Row, this.m_dataRange.Column].IsBlank;
      if (isAnySpace)
        outputSerieNameRange = this.GetSerieRangeByBlank(this.m_dataRange, out range, true);
      isSeriesInRows = this.GetSeriesRangesForChartEx(rangeForChartEx1, rangeForChartEx2, isAnySpace, range, outputSerieNameRange, out range, out outputSerieNameRange, out outputAxisRange);
    }
    int iIndex = 0;
    if (isAnySpace && outputSerieNameRange != null && outputAxisRange != null)
    {
      iIndex = isSeriesInRows ? 0 : outputAxisRange.LastColumn - outputAxisRange.Column + 1;
      outputAxisRange = isSeriesInRows ? outputAxisRange[outputAxisRange.Row, outputSerieNameRange.LastColumn + 1, outputAxisRange.LastRow, outputAxisRange.LastColumn] : outputAxisRange;
    }
    if (!this.ValidateSerieRangeForChartType(range, type, isSeriesInRows))
      throw new ApplicationException("Can't set data range.");
    this.UpdateSeriesByDataRange(range, outputSerieNameRange, outputAxisRange, ChartFormatImpl.GetStartSerieType(type), iIndex, isSeriesInRows);
    this.PrimaryCategoryAxis.CategoryLabels = (IOfficeDataRange) outputAxisRange;
  }

  private bool GetSeriesRangesForChartEx(
    int reqRowsCount,
    int reqColsCount,
    bool isAnySpace,
    IRange inputSerieValue,
    IRange inputSerieNameRange,
    out IRange outputSerieValue,
    out IRange outputSerieNameRange,
    out IRange outputAxisRange)
  {
    bool rangesForChartEx = false;
    outputSerieNameRange = inputSerieNameRange;
    outputSerieValue = inputSerieValue;
    outputAxisRange = (IRange) null;
    if (isAnySpace || reqColsCount < this.m_dataRange.Columns.Length && reqRowsCount < this.m_dataRange.Rows.Length)
    {
      if (reqRowsCount > reqColsCount)
      {
        if (outputSerieValue != null)
        {
          if (outputSerieNameRange != null && this.m_dataRange.Rows.Length != 1 && this.m_dataRange.Columns.Length != 1)
            outputAxisRange = this.CheckForBlankAndAssignAxis(this.m_dataRange, outputSerieNameRange, out outputSerieNameRange, false);
          outputSerieValue = this.GetSerieRanges(outputSerieValue, outputSerieNameRange, outputAxisRange, this.m_dataRange.Columns.Length, this.m_dataRange.Rows.Length, false);
        }
        else
        {
          outputSerieValue = this.m_dataRange[this.m_dataRange.LastRow - reqRowsCount + 1, this.m_dataRange.LastColumn - reqColsCount + 1, this.m_dataRange.LastRow, this.m_dataRange.LastColumn];
          outputAxisRange = this.m_dataRange[outputSerieValue.Row, this.m_dataRange.Column, this.m_dataRange.LastRow, outputSerieValue.Column - 1];
          outputSerieNameRange = this.m_dataRange[this.m_dataRange.Row, outputSerieValue.Column, outputSerieValue.Row - 1, this.m_dataRange.LastColumn];
        }
      }
      else
      {
        rangesForChartEx = true;
        if (outputSerieValue != null)
        {
          if (outputSerieNameRange != null && this.m_dataRange.Rows.Length != 1 && this.m_dataRange.Columns.Length != 1)
            outputSerieNameRange = this.CheckForBlankAndAssignAxis(this.m_dataRange, outputSerieNameRange, out outputAxisRange, false);
          outputSerieValue = this.GetSerieRanges(outputSerieValue, outputSerieNameRange, outputAxisRange, this.m_dataRange.Columns.Length, this.m_dataRange.Rows.Length, true);
        }
        else
        {
          outputSerieValue = this.m_dataRange[this.m_dataRange.LastRow - reqRowsCount + 1, this.m_dataRange.LastColumn - reqColsCount + 1, this.m_dataRange.LastRow, this.m_dataRange.LastColumn];
          outputSerieNameRange = this.m_dataRange[outputSerieValue.Row, this.m_dataRange.Column, this.m_dataRange.LastRow, outputSerieValue.Column - 1];
          outputAxisRange = this.m_dataRange[this.m_dataRange.Row, outputSerieValue.Column, outputSerieValue.Row - 1, this.m_dataRange.LastColumn];
        }
      }
    }
    else if (this.m_dataRange.Rows.Length == 1)
    {
      outputSerieNameRange = this.m_dataRange[this.m_dataRange.Row, this.m_dataRange.Column, this.m_dataRange.LastRow, this.m_dataRange.LastColumn - reqColsCount];
      outputSerieValue = this.m_dataRange[this.m_dataRange.Row, outputSerieNameRange.LastColumn + 1, this.m_dataRange.LastRow, this.m_dataRange.LastColumn];
      rangesForChartEx = true;
    }
    else if (this.m_dataRange.Columns.Length == 1)
    {
      outputSerieNameRange = this.m_dataRange[this.m_dataRange.Row, this.m_dataRange.Column, this.m_dataRange.LastRow - reqRowsCount, this.m_dataRange.LastColumn];
      outputSerieValue = this.m_dataRange[outputSerieNameRange.LastColumn + 1, this.m_dataRange.Column, this.m_dataRange.LastRow, this.m_dataRange.LastColumn];
    }
    else if (reqColsCount != this.m_dataRange.Columns.Length && reqRowsCount == this.m_dataRange.Rows.Length)
    {
      outputAxisRange = this.m_dataRange[this.m_dataRange.Row, this.m_dataRange.Column, this.m_dataRange.LastRow, this.m_dataRange.LastColumn - reqColsCount];
      outputSerieValue = this.m_dataRange[this.m_dataRange.Row, outputAxisRange.LastColumn + 1, this.m_dataRange.LastRow, this.m_dataRange.LastColumn];
    }
    else if (reqColsCount == this.m_dataRange.Columns.Length && reqRowsCount != this.m_dataRange.Rows.Length)
    {
      outputAxisRange = this.m_dataRange[this.m_dataRange.Row, this.m_dataRange.Column, this.m_dataRange.LastRow - reqRowsCount, this.m_dataRange.LastColumn];
      outputSerieValue = this.m_dataRange[outputAxisRange.LastRow + 1, this.m_dataRange.Column, this.m_dataRange.LastRow, this.m_dataRange.LastColumn];
      rangesForChartEx = true;
    }
    else
      outputSerieValue = this.m_dataRange;
    return rangesForChartEx;
  }

  private int GetRangeForChartEx(
    MigrantRangeImpl tempRange,
    int lastIndex,
    int index,
    int constantValue,
    bool isChangeRow)
  {
    bool flag1 = false;
    int rangeForChartEx = 0;
    int num = 0;
    bool flag2 = false;
    for (int index1 = lastIndex; index1 >= index; --index1)
    {
      if (isChangeRow)
        tempRange.ResetRowColumn(index1, constantValue);
      else
        tempRange.ResetRowColumn(constantValue, index1);
      if (tempRange.HasNumber || tempRange.HasFormula)
      {
        ++rangeForChartEx;
        if (flag1)
        {
          rangeForChartEx += num;
          flag1 = false;
          num = 0;
        }
        flag2 = true;
      }
      else if (tempRange.IsBlank)
      {
        ++num;
        flag1 = true;
      }
      else
      {
        if (flag1 && this.IsTreeMapOrSunBurst)
        {
          flag1 = false;
          num = 0;
        }
        if (index1 == lastIndex)
        {
          ++rangeForChartEx;
          break;
        }
        break;
      }
    }
    if (flag1)
    {
      if (this.IsTreeMapOrSunBurst)
        rangeForChartEx = flag2 ? rangeForChartEx : 1;
      else
        rangeForChartEx += num;
    }
    return rangeForChartEx;
  }

  public void UpdateCategory(ChartSeriesCollection series, bool fromDataRange)
  {
    IRange range1 = (IRange) null;
    if ((series.Count == 0 ? 1 : (series[0].Values == null ? 1 : 0)) != 0)
      return;
    int count1 = (series[0].Values as ChartDataRange).Range.Count;
    ChartImpl parentChart = (series[0] as ChartSerieImpl).ParentChart;
    IRange range2 = (parentChart.Series[0].CategoryLabels as ChartDataRange).Range;
    parentChart.DetectIsInRowOnParsing();
    if (parentChart.DataRange == null)
      return;
    parentChart.IsSeriesInRows = this.DetectIsInRow((parentChart.Series[0].Values as ChartDataRange).Range);
    IRange axisRange = (IRange) null;
    if (this.m_dataRange[this.m_dataRange.Row, this.m_dataRange.Column].IsBlank)
    {
      IRange nameRangeOutput = this.GetSerieRangeByBlank(this.m_dataRange, out range1, false);
      if (nameRangeOutput != null && this.m_dataRange.Rows.Length != 1 && this.m_dataRange.Columns.Length != 1)
        axisRange = this.CheckForBlankAndAssignAxis(this.m_dataRange, nameRangeOutput, out nameRangeOutput, this.m_bSeriesInRows);
      if (nameRangeOutput != null && (this.m_bSeriesInRows ? (this.m_dataRange.Rows.Length != 1 ? 1 : 0) : (this.m_dataRange.Rows.Length == 1 ? 1 : 0)) != 0)
      {
        IRange range3 = nameRangeOutput;
        nameRangeOutput = axisRange;
        axisRange = range3;
      }
      range1 = this.GetSerieRanges(range1, nameRangeOutput, axisRange, this.m_dataRange.Columns.Length, this.m_dataRange.Rows.Length, this.m_bSeriesInRows);
    }
    else
    {
      this.GetSerieOrAxisRange(this.DataIRange, this.IsSeriesInRows, out range1);
      this.GetSerieOrAxisRange(range1, !this.IsSeriesInRows, out range1);
    }
    int count2 = range1.Count / count1;
    for (int index = 0; index < count1; ++index)
    {
      IRange categoryRange = ChartImpl.GetCategoryRange(range1, out range1, count2, parentChart.IsSeriesInRows);
      if (fromDataRange)
        (parentChart.Categories as ChartCategoryCollection).Add();
      if (parentChart.Categories.Count > index)
      {
        (parentChart.Categories[index] as ChartCategory).CategoryLabelIRange = range2;
        (parentChart.Categories[index] as ChartCategory).ValuesIRange = categoryRange;
        if ((parentChart.Categories[0].CategoryLabel as ChartDataRange).Range != null)
          (parentChart.Categories[index] as ChartCategory).Name = (parentChart.Categories[0].CategoryLabel as ChartDataRange).Range.Cells[index].Text;
      }
    }
  }

  public void UpdateSeries(ChartSeriesCollection series)
  {
    for (int index = 0; index < series.Count; ++index)
      series[index].IsFiltered = false;
  }

  public static IRange GetCategoryRange(
    IRange Chartvalues,
    out IRange values,
    int count,
    bool bIsInRow)
  {
    int row = Chartvalues.Row;
    int lastRow = Chartvalues.LastRow;
    int column = Chartvalues.Column;
    int lastColumn = Chartvalues.LastColumn;
    if (Chartvalues.Count == count)
    {
      values = Chartvalues;
      return Chartvalues;
    }
    IRange categoryRange = bIsInRow ? Chartvalues[row, column, lastRow, column] : Chartvalues[row, column, row, lastColumn];
    if (row == lastRow && column == lastColumn)
      values = categoryRange;
    else if (row == lastRow)
    {
      values = bIsInRow ? Chartvalues[row, column, lastRow, lastColumn] : Chartvalues[row, column, lastRow, lastColumn];
    }
    else
    {
      int num = bIsInRow ? (column == lastColumn ? 0 : 1) : (row == lastRow ? 0 : 1);
      values = bIsInRow ? Chartvalues[row, column + num, lastRow, lastColumn] : Chartvalues[row + num, column, lastRow, lastColumn];
    }
    return categoryRange;
  }

  public bool IsSeriesInRows
  {
    get => this.m_bSeriesInRows;
    set
    {
      int count = this.m_series.Count;
      if (this.DataRange == null && count != 0)
        throw new NotSupportedException("This property supported only in chart where can detect data range.");
      if (this.m_bSeriesInRows == value)
        return;
      this.m_bSeriesInRows = value;
      if (count == 0)
        return;
      this.GetFilter();
      this.OnSeriesInRowsChanged();
      this.m_categories.Clear();
      this.Setfilter();
      this.UpdateCategory(this.m_series, false);
    }
  }

  private void GetFilter()
  {
    if (this.IsChartBubble)
    {
      int length = 0;
      foreach (IOfficeChartSerie officeChartSerie in (CollectionBase<IOfficeChartSerie>) this.m_series)
        length += officeChartSerie.Bubbles != null ? 2 : 1;
      this.category = new bool[length];
    }
    else
      this.category = new bool[this.m_series.Count];
    this.series = new bool[this.m_categories.Count == 0 ? (this.m_series[0].CategoryLabels != null ? (this.m_series[0].CategoryLabels as ChartDataRange).Range.Count : 0) : this.m_categories.Count];
    for (int index = 0; index < this.m_series.Count; ++index)
      this.category[index] = this.m_series[index].IsFiltered;
    for (int index = 0; index < this.m_categories.Count; ++index)
      this.series[index] = this.m_categories[index].IsFiltered;
    ChartImpl parentChart = (this.m_series[0] as ChartSerieImpl).ParentChart;
    if (parentChart == null)
      return;
    OfficeSeriesNameLevel seriesNameLevel = parentChart.m_seriesNameLevel;
    OfficeCategoriesLabelLevel categoryLabelLevel = parentChart.CategoryLabelLevel;
    parentChart.CategoryLabelLevel = seriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll ? OfficeCategoriesLabelLevel.CategoriesLabelLevelNone : OfficeCategoriesLabelLevel.CategoriesLabelLevelAll;
    if (categoryLabelLevel == OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
      parentChart.SeriesNameLevel = OfficeSeriesNameLevel.SeriesNameLevelAll;
    else
      parentChart.SeriesNameLevel = OfficeSeriesNameLevel.SeriesNameLevelNone;
  }

  private void Setfilter()
  {
    for (int index = 0; index < this.m_series.Count; ++index)
      this.m_series[index].IsFiltered = this.series[index];
    for (int index = 0; index < this.category.Length; ++index)
    {
      this.m_categories.Add();
      this.m_categories[index].IsFiltered = this.category[index];
    }
  }

  public string ChartTitle
  {
    get
    {
      if (!this.ParentWorkbook.Saving && this.m_title != null && this.m_title.Text == "Chart Title")
        return this.GetChartTitle();
      return this.HasTitle ? this.ChartTitleArea.Text : (string) null;
    }
    set
    {
      this.ChartTitleArea.Text = value;
      if (value != null && !(value == string.Empty))
        return;
      this.HasAutoTitle = new bool?(true);
      this.HasTitle = false;
    }
  }

  internal string GetChartTitle()
  {
    if (this.m_series.Count == 0)
      return "Chart Title";
    string chartTitle = "";
    ChartSerieImpl chartSerieImpl = this.m_series[0] as ChartSerieImpl;
    OfficeChartType chartType = this.ChartType;
    if (this.HasTitleInternal)
      chartTitle = this.m_series.Count != 1 ? (!chartType.ToString().Contains("Pie") || chartSerieImpl.IsDefaultName ? "Chart Title" : chartSerieImpl.Name) : (!chartSerieImpl.IsDefaultName ? chartSerieImpl.Name : "Chart Title");
    return chartTitle;
  }

  public IOfficeChartTextArea ChartTitleArea
  {
    get
    {
      if (this.m_title == null)
        this.CreateChartTitle();
      return (IOfficeChartTextArea) this.m_title;
    }
  }

  public IOfficeFont ChartTitleFont => (IOfficeFont) this.ChartTitleArea;

  public string CategoryAxisTitle
  {
    get => this.PrimaryCategoryAxis.Title;
    set => this.PrimaryCategoryAxis.Title = value;
  }

  internal bool IsPrimarySecondaryCategory
  {
    get => this.m_bIsPrimarySecondaryCategory;
    set => this.m_bIsPrimarySecondaryCategory = value;
  }

  internal bool IsPrimarySecondaryValue
  {
    get => this.m_bIsPrimarySecondaryValue;
    set => this.m_bIsPrimarySecondaryValue = value;
  }

  public string ValueAxisTitle
  {
    get => this.PrimaryValueAxis.Title;
    set => this.PrimaryValueAxis.Title = value;
  }

  public string SecondaryCategoryAxisTitle
  {
    get => this.SecondaryCategoryAxis.Title;
    set => this.SecondaryCategoryAxis.Title = value;
  }

  public string SecondaryValueAxisTitle
  {
    get => this.SecondaryValueAxis.Title;
    set => this.SecondaryValueAxis.Title = value;
  }

  public string SeriesAxisTitle
  {
    get => this.PrimarySerieAxis.Title;
    set => this.PrimarySerieAxis.Title = value;
  }

  public IOfficeChartCategoryAxis PrimaryCategoryAxis
  {
    get => (IOfficeChartCategoryAxis) this.m_primaryParentAxis.CategoryAxis;
  }

  public IOfficeChartValueAxis PrimaryValueAxis
  {
    get => (IOfficeChartValueAxis) this.m_primaryParentAxis.ValueAxis;
  }

  public IOfficeChartSeriesAxis PrimarySerieAxis
  {
    get
    {
      if (!this.IsSeriesAxisAvail && !this.Loading)
        throw new NotSupportedException("Series axis doesnot exist in current chart type.");
      return (IOfficeChartSeriesAxis) this.m_primaryParentAxis.SeriesAxis;
    }
  }

  public IOfficeChartCategoryAxis SecondaryCategoryAxis
  {
    get => (IOfficeChartCategoryAxis) this.m_secondaryParentAxis.CategoryAxis;
  }

  public IOfficeChartValueAxis SecondaryValueAxis
  {
    get => (IOfficeChartValueAxis) this.m_secondaryParentAxis.ValueAxis;
  }

  public IOfficeChartPageSetup PageSetup => (IOfficeChartPageSetup) this.m_pageSetup;

  public double XPos
  {
    get => this.m_dXPos;
    set => this.m_dXPos = value;
  }

  public double YPos
  {
    get => this.m_dYPos;
    set => this.m_dYPos = value;
  }

  public double Width
  {
    get
    {
      return (double) (int) Math.Round(ApplicationImpl.ConvertToPixels(this.EMUWidth, MeasureUnits.EMU));
    }
    set => this.m_dWidth = Math.Round(ApplicationImpl.ConvertFromPixel(value, MeasureUnits.EMU));
  }

  public double Height
  {
    get
    {
      return (double) (int) Math.Round(ApplicationImpl.ConvertToPixels(this.EMUHeight, MeasureUnits.EMU));
    }
    set => this.m_dHeight = Math.Round(ApplicationImpl.ConvertFromPixel(value, MeasureUnits.EMU));
  }

  internal double EMUHeight
  {
    get => this.m_dHeight;
    set => this.m_dHeight = value;
  }

  internal double EMUWidth
  {
    get => this.m_dWidth;
    set => this.m_dWidth = value;
  }

  public IOfficeChartSeries Series => (IOfficeChartSeries) this.m_series;

  public IOfficeChartCategories Categories => (IOfficeChartCategories) this.m_categories;

  public ChartFormatCollection PrimaryFormats => this.m_primaryParentAxis.ChartFormats;

  public ChartFormatCollection SecondaryFormats => this.m_secondaryParentAxis.ChartFormats;

  public IOfficeChartFrameFormat ChartArea
  {
    get
    {
      if (this.m_chartArea == null)
      {
        this.m_chartArea = new ChartFrameFormatImpl(this.Application, (object) this);
        this.m_chartArea.Interior.ForegroundColorIndex = OfficeKnownColors.WhiteCustom;
      }
      return (IOfficeChartFrameFormat) this.m_chartArea;
    }
  }

  public bool HasChartArea
  {
    get => this.m_chartArea != null;
    set
    {
      if (value == this.HasChartArea)
        return;
      this.m_chartArea = value ? new ChartFrameFormatImpl(this.Application, (object) this) : (ChartFrameFormatImpl) null;
    }
  }

  public bool HasPlotArea
  {
    get => this.m_plotArea != null;
    set
    {
      if (value == this.HasPlotArea)
        return;
      this.m_plotArea = value ? new ChartPlotAreaImpl(this.Application, (object) this, this.ChartType) : (ChartPlotAreaImpl) null;
    }
  }

  public IOfficeChartFrameFormat PlotArea
  {
    get => (IOfficeChartFrameFormat) this.m_plotArea;
    set => this.m_plotArea = (ChartPlotAreaImpl) value;
  }

  public ChartParentAxisImpl PrimaryParentAxis => this.m_primaryParentAxis;

  public ChartParentAxisImpl SecondaryParentAxis => this.m_secondaryParentAxis;

  public IOfficeChartWallOrFloor Walls
  {
    get
    {
      if (!this.m_book.IsWorkbookOpening && Array.IndexOf<OfficeChartType>(ChartImpl.DEF_WALLS_OR_FLOOR_TYPES, this.ChartType) == -1)
        throw new ApplicationException("Walls are not supported in this chart type");
      if (this.m_walls == null)
        this.m_walls = new ChartWallOrFloorImpl(this.Application, (object) this, true);
      return (IOfficeChartWallOrFloor) this.m_walls;
    }
    set => this.m_walls = (ChartWallOrFloorImpl) value;
  }

  public IOfficeChartWallOrFloor SideWall
  {
    get
    {
      if (!this.m_book.IsWorkbookOpening && Array.IndexOf<OfficeChartType>(ChartImpl.DEF_WALLS_OR_FLOOR_TYPES, this.ChartType) == -1)
        throw new ApplicationException("Walls are not supported in this chart type");
      if (this.m_sidewall == null)
        this.m_sidewall = new ChartWallOrFloorImpl(this.Application, (object) this, true);
      return (IOfficeChartWallOrFloor) this.m_sidewall;
    }
    set => this.m_sidewall = (ChartWallOrFloorImpl) value;
  }

  public IOfficeChartWallOrFloor BackWall
  {
    get => this.Walls;
    set => this.Walls = value;
  }

  public IOfficeChartWallOrFloor Floor
  {
    get
    {
      if (!this.m_book.IsWorkbookOpening && !this.SupportWallsAndFloor)
        throw new ApplicationException("Floor is not supported by this chart type.");
      if (this.m_floor == null)
        this.m_floor = new ChartWallOrFloorImpl(this.Application, (object) this, false);
      return (IOfficeChartWallOrFloor) this.m_floor;
    }
    set => this.m_floor = (ChartWallOrFloorImpl) value;
  }

  public IOfficeChartDataTable DataTable => (IOfficeChartDataTable) this.m_dataTable;

  public bool HasDataTable
  {
    get => this.m_bHasDataTable;
    set
    {
      if (this.m_bHasDataTable == value)
        return;
      if (value)
        this.CheckSupportDataTable();
      this.m_bHasDataTable = value;
      this.m_dataTable = value ? new ChartDataTableImpl(this.Application, (object) this) : (ChartDataTableImpl) null;
    }
  }

  public IOfficeChartLegend Legend => (IOfficeChartLegend) this.m_legend;

  public bool HasLegend
  {
    get => this.m_bHasLegend;
    set
    {
      if (this.m_bHasLegend == value)
        return;
      this.m_bHasLegend = value;
      this.m_legend = value ? new ChartLegendImpl(this.Application, (object) this) : (ChartLegendImpl) null;
    }
  }

  public bool HasTitle
  {
    get => this.m_bHasChartTitle;
    set
    {
      if (this.m_bHasChartTitle != value)
      {
        this.m_bHasChartTitle = value;
        if (this.m_title != null)
        {
          if (value && this.m_title.Text == null)
            this.m_title.Text = "Chart Title";
          else if (!value)
            this.m_title.Text = (string) null;
        }
        else
          this.CreateChartTitle();
      }
      if (this.ParentWorkbook.IsWorkbookOpening)
        return;
      this.m_hasAutoTitle = new bool?(!value);
    }
  }

  public OfficeChartPlotEmpty DisplayBlanksAs
  {
    get => this.m_chartProperties.PlotBlank;
    set => this.m_chartProperties.PlotBlank = value;
  }

  public bool PlotVisibleOnly
  {
    get => this.m_chartProperties.IsPlotVisOnly;
    set => this.m_chartProperties.IsPlotVisOnly = value;
  }

  public bool ShowPlotVisible
  {
    get => this.m_showPlotVisible;
    set => this.m_showPlotVisible = value;
  }

  public bool SizeWithWindow
  {
    get => this.m_bInWorksheet || !this.m_chartProperties.IsNotSizeWith;
    set
    {
      if (this.m_bInWorksheet)
        return;
      this.m_chartProperties.IsNotSizeWith = !value;
    }
  }

  public bool SupportWallsAndFloor
  {
    get => Array.IndexOf<OfficeChartType>(ChartImpl.DEF_WALLS_OR_FLOOR_TYPES, this.ChartType) >= 0;
  }

  public override bool ProtectDrawingObjects
  {
    get => (this.InnerProtection & OfficeSheetProtection.Objects) != OfficeSheetProtection.None;
  }

  public override bool ProtectScenarios
  {
    get => (this.InnerProtection & OfficeSheetProtection.Scenarios) != OfficeSheetProtection.None;
  }

  public override OfficeSheetProtection Protection
  {
    get => base.Protection & ~OfficeSheetProtection.Scenarios;
  }

  public ChartPlotAreaLayoutRecord PlotAreaLayout
  {
    get
    {
      if (this.m_plotAreaLayout == null)
        this.m_plotAreaLayout = (ChartPlotAreaLayoutRecord) BiffRecordFactory.GetRecord(TBIFFRecord.PlotAreaLayout);
      return this.m_plotAreaLayout;
    }
  }

  public object[] CategoryLabelValues
  {
    get => this.m_categoryLabelValues;
    set => this.m_categoryLabelValues = value;
  }

  internal bool IsChartCleared
  {
    get => this.m_isChartCleared;
    set => this.m_isChartCleared = value;
  }

  public IOfficeChartData ChartData
  {
    get
    {
      return (IOfficeChartData) this._chartData ?? (IOfficeChartData) (this._chartData = new Syncfusion.OfficeChart.Implementation.Charts.ChartData((IOfficeChart) this));
    }
  }

  public string CategoryFormula
  {
    get => this.m_formula;
    set
    {
      this.m_formula = value.Length != 0 && value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  internal ushort ChartExTitlePosition
  {
    get => this.m_chartExTitlePosition;
    set => this.m_chartExTitlePosition = value;
  }

  internal bool ChartTitleIncludeInLayout
  {
    get => this.m_chartTitleIncludeInLayout;
    set => this.m_chartTitleIncludeInLayout = value;
  }

  internal bool? AutoUpdate
  {
    get => this.m_isAutoUpdate;
    set => this.m_isAutoUpdate = value;
  }

  internal string ChartExRelationId
  {
    get => this.m_chartExRelationId;
    set => this.m_chartExRelationId = value;
  }

  internal bool IsTreeMapOrSunBurst
  {
    get => this.ChartType == OfficeChartType.SunBurst || this.ChartType == OfficeChartType.TreeMap;
  }

  internal bool IsHistogramOrPareto
  {
    get => this.ChartType == OfficeChartType.Histogram || this.ChartType == OfficeChartType.Pareto;
  }

  internal int LineChartCount
  {
    get => this.m_linechartCount;
    set => this.m_linechartCount = value;
  }

  public override OfficeKnownColors TabColor
  {
    get => base.TabColor;
    set
    {
      if (this.m_bInWorksheet)
        throw new NotSupportedException();
      base.TabColor = value;
    }
  }

  internal int ActiveSheetIndex
  {
    get => this.m_activeSheetIndex;
    set => this.m_activeSheetIndex = value;
  }

  public bool IsCategoryAxisAvail
  {
    get => Array.IndexOf<OfficeChartType>(ChartImpl.NO_CATEGORY_AXIS, this.ChartType) == -1;
  }

  public bool IsValueAxisAvail
  {
    get => Array.IndexOf<OfficeChartType>(ChartImpl.NO_CATEGORY_AXIS, this.ChartType) == -1;
  }

  public bool IsSeriesAxisAvail
  {
    get
    {
      OfficeChartType chartType = this.ChartType;
      return Array.IndexOf<OfficeChartType>(ChartImpl.DEF_SUPPORT_SERIES_AXIS, chartType) != -1;
    }
  }

  public bool IsStacked => ChartImpl.GetIsStacked(this.ChartType);

  public bool IsChart_100 => ChartImpl.GetIs100(this.ChartType);

  public bool IsChart3D => Array.IndexOf<OfficeChartType>(ChartImpl.CHARTS3D, this.ChartType) != -1;

  public bool IsPivotChart3D
  {
    get => Array.IndexOf<OfficeChartType>(ChartImpl.DEF_NEED_VIEW_3D, this.PivotChartType) != -1;
  }

  public bool IsChartLine
  {
    get => Array.IndexOf<OfficeChartType>(ChartImpl.CHARTS_LINE, this.ChartType) != -1;
  }

  public bool NeedDataFormat
  {
    get
    {
      return (this.IsChart3D || this.IsChartLine || this.IsChartExploded || this.IsChartScatter || this.IsChartStock || this.ChartType == OfficeChartType.Bubble_3D || this.ChartType == OfficeChartType.Radar) && this.ChartType != OfficeChartType.Surface_NoColor_Contour;
    }
  }

  public bool NeedMarkerFormat => this.IsChartPyramid || this.IsChartCone || this.IsChartCylinder;

  public bool IsChartBar => this.ChartType.ToString().IndexOf("Bar") != -1;

  public bool IsChartPyramid => this.ChartType.ToString().IndexOf("Pyramid") != -1;

  public bool IsChartCone => this.ChartType.ToString().IndexOf("Cone") != -1;

  public bool IsChartCylinder => this.ChartType.ToString().IndexOf("Cylinder") != -1;

  public bool IsChartBubble
  {
    get => Array.IndexOf<OfficeChartType>(ChartImpl.CHARTS_BUBBLE, this.ChartType) != -1;
  }

  public bool IsChartDoughnut => this.ChartType.ToString().IndexOf("Doughnut") != -1;

  public bool IsChartVaryColor
  {
    get => Array.IndexOf<OfficeChartType>(ChartImpl.CHARTS_VARYCOLOR, this.ChartType) != -1;
  }

  public bool IsChartExploded
  {
    get => Array.IndexOf<OfficeChartType>(ChartImpl.CHARTS_EXPLODED, this.ChartType) != -1;
  }

  public bool IsSeriesLines => this.CanChartHaveSeriesLines;

  public bool CanChartHaveSeriesLines
  {
    get => Array.IndexOf<OfficeChartType>(ChartImpl.CHART_SERIES_LINES, this.ChartType) != -1;
  }

  public bool IsChartScatter
  {
    get => Array.IndexOf<OfficeChartType>(ChartImpl.CHARTS_SCATTER, this.ChartType) != -1;
  }

  public OfficeChartLinePattern DefaultLinePattern
  {
    get
    {
      return this.ChartType == OfficeChartType.Scatter_Markers || this.IsChartStock ? OfficeChartLinePattern.None : OfficeChartLinePattern.Solid;
    }
  }

  public bool IsChartSmoothedLine
  {
    get => Array.IndexOf<OfficeChartType>(ChartImpl.CHARTS_SMOOTHED_LINE, this.ChartType) != -1;
  }

  public bool IsChartStock
  {
    get => Array.IndexOf<OfficeChartType>(ChartImpl.CHARTS_STOCK, this.ChartType) != -1;
  }

  public bool NeedDropBar
  {
    get
    {
      return this.ChartType == OfficeChartType.Stock_OpenHighLowClose || this.ChartType == OfficeChartType.Stock_VolumeOpenHighLowClose;
    }
  }

  public bool IsChartVolume
  {
    get
    {
      return this.ChartType == OfficeChartType.Stock_VolumeHighLowClose || this.ChartType == OfficeChartType.Stock_VolumeOpenHighLowClose;
    }
  }

  public bool IsPerspective
  {
    get => Array.IndexOf<OfficeChartType>(ChartImpl.CHARTS_PERSPECTIVE, this.ChartType) != -1;
  }

  public bool IsClustered => ChartImpl.GetIsClustered(this.ChartType);

  public bool NoPlotArea
  {
    get
    {
      return this.IsChartRadar || this.IsChartPie || this.IsChartDoughnut || this.ChartType == OfficeChartType.Surface_NoColor_Contour;
    }
  }

  public bool IsChartRadar => this.ChartType.ToString().StartsWith("Radar");

  public bool IsChartPie => ChartImpl.GetIsChartPie(this.ChartType);

  public bool IsChartWalls => false;

  public bool IsChartFloor
  {
    get
    {
      return OfficeChartType.Surface_NoColor_Contour == this.ChartType || OfficeChartType.Surface_Contour == this.ChartType;
    }
  }

  internal List<int> SerializedAxisIds
  {
    get
    {
      if (this.m_axisIds == null)
        this.m_axisIds = new List<int>();
      return this.m_axisIds;
    }
  }

  public bool IsSecondaryCategoryAxisAvail => this.SecondaryCategoryAxis != null;

  public bool IsSecondaryValueAxisAvail => this.SecondaryValueAxis != null;

  public bool IsSecondaryAxes
  {
    get
    {
      return this.IsSecondaryValueAxisAvail || this.IsSecondaryCategoryAxisAvail || this.m_bIsSecondaryAxis;
    }
    set => this.m_bIsSecondaryAxis = value;
  }

  public bool IsSpecialDataLabels
  {
    get => Array.IndexOf<OfficeChartType>(ChartImpl.DEF_SPECIAL_DATA_LABELS, this.ChartType) != -1;
  }

  public bool CanChartPercentageLabel
  {
    get => Array.IndexOf<OfficeChartType>(ChartImpl.DEF_CHART_PERCENTAGE, this.ChartType) != -1;
  }

  public bool CanChartBubbleLabel => this.IsChartBubble;

  public bool IsManuallyFormatted
  {
    get => this.m_chartProperties.IsManSerAlloc;
    set => this.m_chartProperties.IsManSerAlloc = value;
  }

  private ChartPlotGrowthRecord PlotGrowth
  {
    get
    {
      if (this.m_plotGrowth == null)
        this.m_plotGrowth = (ChartPlotGrowthRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartPlotGrowth);
      return this.m_plotGrowth;
    }
  }

  private ChartPosRecord PlotAreaBoundingBox
  {
    get
    {
      if (this.m_plotAreaBoundingBox == null)
      {
        this.m_plotAreaBoundingBox = (ChartPosRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartPos);
        this.m_plotAreaBoundingBox.BottomRight = (ushort) 2;
        this.m_plotAreaBoundingBox.TopLeft = (ushort) 2;
      }
      return this.m_plotAreaBoundingBox;
    }
  }

  public WorkbookImpl InnerWorkbook => this.m_book;

  public ChartFrameFormatImpl InnerChartArea => this.ChartArea as ChartFrameFormatImpl;

  public ChartFrameFormatImpl InnerPlotArea
  {
    get
    {
      if (this.m_plotAreaFrame == null)
        this.m_plotAreaFrame = new ChartFrameFormatImpl(this.Application, (object) this);
      return this.m_plotAreaFrame;
    }
  }

  public string ChartStartType => ChartFormatImpl.GetStartSerieType(this.ChartType);

  public override PageSetupBaseImpl PageSetupBase => (PageSetupBaseImpl) this.m_pageSetup;

  [CLSCompliant(false)]
  public ChartShtpropsRecord ChartProperties => this.m_chartProperties;

  public bool Loading => this.m_book.IsWorkbookOpening;

  internal ChartFormatImpl ChartFormat
  {
    get
    {
      return this.m_series.Count != 0 ? ((ChartSerieImpl) this.m_series[0]).GetCommonSerieFormat() : throw new ApplicationException("cannot get format.");
    }
  }

  public bool TypeChanging
  {
    get => this.m_bTypeChanging;
    set => this.m_bTypeChanging = value;
  }

  public OfficeChartType DestinationType
  {
    get => this.m_destinationType;
    set => this.m_destinationType = value;
  }

  public RelationCollection Relations
  {
    get
    {
      if (this.m_relations == null)
        this.m_relations = new RelationCollection();
      return this.m_relations;
    }
  }

  public int Style
  {
    get => this.m_iStyle2007;
    set
    {
      this.m_iStyle2007 = value;
      if (this.ParentWorkbook.IsWorkbookOpening || ChartImpl.IsChartExSerieType(this.ChartType) || this.m_relationPreservedStreamCollections == null || this.m_relationPreservedStreamCollections.Count <= 0)
        return;
      if (this.m_relationPreservedStreamCollections.ContainsKey("http://schemas.microsoft.com/office/2011/relationships/chartColorStyle"))
      {
        this.m_relationPreservedStreamCollections.Remove("http://schemas.microsoft.com/office/2011/relationships/chartColorStyle");
        this.m_isChartColorStyleSkipped = false;
      }
      if (!this.m_relationPreservedStreamCollections.ContainsKey("http://schemas.microsoft.com/office/2011/relationships/chartStyle"))
        return;
      this.m_relationPreservedStreamCollections.Remove("http://schemas.microsoft.com/office/2011/relationships/chartStyle");
      this.m_isChartStyleSkipped = false;
    }
  }

  public bool HasFloor => this.m_floor != null;

  public bool HasWalls => this.m_walls != null;

  public Stream PivotFormatsStream
  {
    get => this.m_pivotFormatsStream;
    set => this.m_pivotFormatsStream = value;
  }

  public bool ZoomToFit
  {
    get => this.SizeWithWindow;
    set => this.SizeWithWindow = value;
  }

  protected override OfficeSheetProtection DefaultProtectionOptions
  {
    get
    {
      return OfficeSheetProtection.Objects | OfficeSheetProtection.Scenarios | OfficeSheetProtection.Content;
    }
  }

  public bool IsEmbeded => this.m_bInWorksheet;

  public int DefaultTextIndex
  {
    get
    {
      int defaultTextIndex = 0;
      if (this.m_lstDefaultText != null && this.m_lstDefaultText.Count > 0)
      {
        List<BiffRecordRaw> byIndex = this.m_lstDefaultText.GetByIndex(0);
        if (byIndex != null)
        {
          foreach (BiffRecordRaw biffRecordRaw in byIndex)
          {
            if (biffRecordRaw.TypeCode == TBIFFRecord.ChartFontx)
            {
              defaultTextIndex = (int) ((ChartFontxRecord) biffRecordRaw).FontIndex;
              break;
            }
          }
        }
      }
      return defaultTextIndex;
    }
  }

  public Stream PreservedBandFormats
  {
    get => this.m_bandFormats;
    set => this.m_bandFormats = value;
  }

  public bool HasTitleInternal
  {
    get
    {
      bool hasTitleInternal = false;
      if (this.m_title != null && this.HasTitle && (this.m_title.Text != null || this.m_title.FontIndex == 0 || !this.m_title.TextRecord.IsAutoColor || !this.m_title.TextRecord.IsAutoMode))
        hasTitleInternal = true;
      return hasTitleInternal;
    }
  }

  internal Stream AlternateContent
  {
    get => this.m_alternateContent;
    set => this.m_alternateContent = value;
  }

  internal Stream DefaultTextProperty
  {
    get => this.m_defaultTextProperty;
    set => this.m_defaultTextProperty = value;
  }

  public IOfficeFont Font
  {
    get
    {
      if (this.m_font == null)
        this.m_font = new FontWrapper((FontImpl) this.ParentWorkbook.InnerFonts[0]);
      return (IOfficeFont) this.m_font;
    }
  }

  internal bool? HasAutoTitle
  {
    get => this.m_hasAutoTitle;
    set => this.m_hasAutoTitle = value;
  }

  internal override bool ParseDataOnDemand
  {
    get => this.m_bParseDataOnDemand;
    set => this.m_bParseDataOnDemand = value;
  }

  internal string RadarStyle
  {
    get => this.m_radarStyle;
    set => this.m_radarStyle = value;
  }

  internal Dictionary<string, Stream> RelationPreservedStreamCollection
  {
    get
    {
      if (this.m_relationPreservedStreamCollections == null)
        this.m_relationPreservedStreamCollections = new Dictionary<string, Stream>(3);
      return this.m_relationPreservedStreamCollections;
    }
  }

  internal IEnumerable<IGrouping<int, IOfficeChartSerie>> ChartSerieGroupsBeforesorting
  {
    get => this.m_chartSerieGroupsBeforesorting;
    set => this.m_chartSerieGroupsBeforesorting = value;
  }

  public static bool GetIsClustered(OfficeChartType chartType)
  {
    return Array.IndexOf<OfficeChartType>(ChartImpl.CHARTS_CLUSTERED, chartType) >= 0;
  }

  public static bool GetIs100(OfficeChartType chartType)
  {
    return Array.IndexOf<OfficeChartType>(ChartImpl.CHARTS_100, chartType) >= 0;
  }

  public static bool GetIsStacked(OfficeChartType chartType)
  {
    return Array.IndexOf<OfficeChartType>(ChartImpl.STACKEDCHARTS, chartType) >= 0;
  }

  public static bool GetIsChartPie(OfficeChartType chartType)
  {
    return chartType.ToString().StartsWith("Pie");
  }

  public void CreateNecessaryAxes(bool bPrimary)
  {
    if (bPrimary)
    {
      if (this.IsCategoryAxisAvail && this.m_primaryParentAxis.CategoryAxis == null)
        this.m_primaryParentAxis.CategoryAxis = new ChartCategoryAxisImpl(this.Application, (object) this.m_primaryParentAxis, OfficeAxisType.Category);
      if (this.IsValueAxisAvail && this.m_primaryParentAxis.ValueAxis == null)
        this.m_primaryParentAxis.ValueAxis = new ChartValueAxisImpl(this.Application, (object) this.m_primaryParentAxis, OfficeAxisType.Value);
      if (!this.IsSeriesAxisAvail || this.m_primaryParentAxis.SeriesAxis != null)
        return;
      this.m_primaryParentAxis.SeriesAxis = new ChartSeriesAxisImpl(this.Application, (object) this.m_primaryParentAxis, OfficeAxisType.Serie);
    }
    else
    {
      if (this.m_secondaryParentAxis.CategoryAxis != null)
        return;
      this.m_secondaryParentAxis.CategoryAxis = new ChartCategoryAxisImpl(this.Application, (object) this.m_secondaryParentAxis, OfficeAxisType.Category, false);
      this.m_secondaryParentAxis.ValueAxis = new ChartValueAxisImpl(this.Application, (object) this.m_secondaryParentAxis, OfficeAxisType.Value, false);
    }
  }

  protected override void InitializeCollections()
  {
    base.InitializeCollections();
    this.m_arrFonts = new List<ChartFbiRecord>();
    this.m_series = new ChartSeriesCollection(this.Application, (object) this);
    this.m_pageSetup = new ChartPageSetupImpl(this.Application, (object) this);
    this.m_categories = new ChartCategoryCollection(this.Application, (object) this);
    this.m_primaryParentAxis = new ChartParentAxisImpl(this.Application, (object) this);
    this.m_secondaryParentAxis = new ChartParentAxisImpl(this.Application, (object) this, false);
    this.m_primaryParentAxis.CreatePrimaryFormats();
    this.m_secondaryParentAxis.UpdateSecondaryAxis(false);
    this.InitializeDefaultText();
  }

  private void CheckSupportDataTable()
  {
    if (this.ChartType != OfficeChartType.Combination_Chart)
    {
      ChartImpl.CheckDataTablePossibility(ChartFormatImpl.GetStartSerieType(this.ChartType), true);
    }
    else
    {
      int index = 0;
      for (int count = this.Series.Count; index < count; ++index)
        ChartImpl.CheckDataTablePossibility(ChartFormatImpl.GetStartSerieType((this.Series[index] as ChartSerieImpl).SerieType), true);
    }
  }

  public static bool CheckDataTablePossibility(string startType, bool bThrowException)
  {
    bool flag = Array.IndexOf<string>(ChartImpl.DEF_SUPPORT_DATA_TABLE, startType) != -1;
    return flag || !bThrowException ? flag : throw new NotSupportedException("Data table does not suported in this chart type");
  }

  private void InitializeDefaultText()
  {
    List<BiffRecordRaw> biffRecordRawList1 = new List<BiffRecordRaw>();
    ChartTextRecord record1 = (ChartTextRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartText);
    record1.IsAutoText = true;
    record1.IsGenerated = true;
    record1.HorzAlign = ExcelChartHorzAlignment.Center;
    record1.VertAlign = ExcelChartVertAlignment.Center;
    biffRecordRawList1.Add((BiffRecordRaw) record1);
    biffRecordRawList1.Add(BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
    ChartPosRecord record2 = (ChartPosRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartPos);
    record2.TopLeft = (ushort) 2;
    record2.BottomRight = (ushort) 2;
    biffRecordRawList1.Add((BiffRecordRaw) record2);
    ChartFontxRecord record3 = (ChartFontxRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartFontx);
    biffRecordRawList1.Add((BiffRecordRaw) record3);
    ChartAIRecord record4 = (ChartAIRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAI);
    record4.Reference = ChartAIRecord.ReferenceType.EnteredDirectly;
    biffRecordRawList1.Add((BiffRecordRaw) record4);
    biffRecordRawList1.Add(BiffRecordFactory.GetRecord(TBIFFRecord.End));
    this.m_lstDefaultText.Add(2, biffRecordRawList1);
    List<BiffRecordRaw> biffRecordRawList2 = new List<BiffRecordRaw>();
    biffRecordRawList2.Add((BiffRecordRaw) record1.Clone());
    biffRecordRawList2.Add(BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
    biffRecordRawList2.Add((BiffRecordRaw) record2.Clone());
    ChartFontxRecord chartFontxRecord = (ChartFontxRecord) record3.Clone();
    biffRecordRawList2.Add((BiffRecordRaw) chartFontxRecord);
    biffRecordRawList2.Add((BiffRecordRaw) record4.Clone());
    biffRecordRawList2.Add(BiffRecordFactory.GetRecord(TBIFFRecord.End));
    this.m_lstDefaultText.Add(3, biffRecordRawList2);
  }

  internal void OnDataRangeChanged(OfficeChartType type)
  {
    if (this.m_dataRange == null)
    {
      this.m_series.Clear();
    }
    else
    {
      IRange axisRange = (IRange) null;
      IRange nameRangeOutput;
      IRange serieRange1;
      if (this.m_dataRange[this.m_dataRange.Row, this.m_dataRange.Column].IsBlank)
      {
        IRange serieRange2;
        nameRangeOutput = this.GetSerieRangeByBlank(this.m_dataRange, out serieRange2, false);
        if (nameRangeOutput != null && this.m_dataRange.Rows.Length != 1 && this.m_dataRange.Columns.Length != 1)
          axisRange = this.CheckForBlankAndAssignAxis(this.m_dataRange, nameRangeOutput, out nameRangeOutput, this.m_bSeriesInRows);
        if (nameRangeOutput != null && (this.m_bSeriesInRows ? (this.m_dataRange.Rows.Length != 1 ? 1 : 0) : (this.m_dataRange.Rows.Length == 1 ? 1 : 0)) != 0)
        {
          IRange range = nameRangeOutput;
          nameRangeOutput = axisRange;
          axisRange = range;
        }
        serieRange1 = this.GetSerieRanges(serieRange2, nameRangeOutput, axisRange, this.m_dataRange.Columns.Length, this.m_dataRange.Rows.Length, this.m_bSeriesInRows);
      }
      else
      {
        nameRangeOutput = this.GetSerieOrAxisRange(this.m_dataRange, this.m_bSeriesInRows, out serieRange1);
        axisRange = this.GetSerieOrAxisRange(serieRange1, !this.m_bSeriesInRows, out serieRange1);
      }
      if (!this.ValidateSerieRangeForChartType(serieRange1, type, this.m_bSeriesInRows))
        throw new ApplicationException("Cann't set data range.");
      ((ChartCategoryAxisImpl) this.PrimaryCategoryAxis).CategoryLabelsIRange = axisRange;
      int iIndex = 0;
      if (nameRangeOutput != null && axisRange != null)
        iIndex = this.m_bSeriesInRows ? axisRange.LastRow - axisRange.Row + 1 : axisRange.LastColumn - axisRange.Column + 1;
      this.UpdateSeriesByDataRange(serieRange1, nameRangeOutput, axisRange, ChartFormatImpl.GetStartSerieType(type), iIndex, this.m_bSeriesInRows);
    }
  }

  private IRange GetSerieRangeByBlank(IRange range, out IRange serieRange, bool isChartEx)
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = range.LastRow - range.Row + 1;
    int num4 = range.LastColumn - range.Column + 1;
    if (num3 == 1 && num4 == 1)
    {
      serieRange = range;
      return (IRange) null;
    }
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, range.Worksheet);
    for (int index1 = 0; index1 < num3; ++index1)
    {
      int num5 = 0;
      int row = range.Row;
      for (int index2 = 0; index2 < num4; ++index2)
      {
        migrantRangeImpl.ResetRowColumn(range.Row + index1, range.Column + index2);
        if (migrantRangeImpl.IsBlank)
          ++num5;
        else
          break;
      }
      if (index1 == 0)
        num1 = num5;
      else if (isChartEx)
      {
        if (num5 < num1)
          break;
      }
      else if (num5 != num1)
        break;
      ++num2;
    }
    IRange serieRangeByBlank;
    if (num4 == 1 || num3 == 1)
    {
      if (num4 == num1 && num3 == num2)
      {
        num2 = 1;
        num1 = 1;
      }
      serieRangeByBlank = num4 == 1 ? range[range.Row, range.Column, range.Row + num2 - 1, range.Column] : range[range.Row, range.Column, range.Row, range.Column + num1 - 1];
      serieRange = num3 == 1 ? range[serieRangeByBlank.Row, serieRangeByBlank.LastColumn + 1, range.LastRow, range.LastColumn] : range[serieRangeByBlank.LastRow + 1, serieRangeByBlank.LastColumn, range.LastRow, range.LastColumn];
    }
    else
    {
      if (num1 == num4)
        num1 = 1;
      if (num2 == num3)
        num2 = 1;
      serieRangeByBlank = range[range.Row, range.Column + num1, range.Row + num2 - 1, range.LastColumn];
      serieRange = range[serieRangeByBlank.LastRow + 1, range.Column, range.LastRow, range.LastColumn];
    }
    return serieRangeByBlank;
  }

  private IRange CheckForBlankAndAssignAxis(
    IRange dataRange,
    IRange nameRangeInput,
    out IRange nameRangeOutput,
    bool isSeriesInRows)
  {
    int num1 = dataRange.LastColumn - dataRange.Column - (nameRangeInput.LastColumn - nameRangeInput.Column);
    int num2 = dataRange.LastColumn - dataRange.Column + 1;
    if (num1 != num2)
    {
      if (isSeriesInRows)
      {
        nameRangeOutput = nameRangeInput;
        return dataRange[nameRangeOutput.Row, dataRange.Column, dataRange.LastRow, nameRangeOutput.Column - 1];
      }
      nameRangeOutput = dataRange[dataRange.Row, dataRange.Column, nameRangeInput.LastRow, nameRangeInput.LastColumn];
      return dataRange[nameRangeOutput.LastRow + 1, dataRange.Column, dataRange.LastRow, dataRange.Column + num1 - 1];
    }
    nameRangeOutput = nameRangeInput;
    return (IRange) null;
  }

  private IRange GetSerieRanges(
    IRange inputRange,
    IRange serieNameRange,
    IRange axisRange,
    int columnCount,
    int rowCount,
    bool isSeriesInRows)
  {
    inputRange = columnCount != 1 || (isSeriesInRows ? (axisRange != null ? 1 : 0) : (serieNameRange != null ? 1 : 0)) == 0 ? (rowCount != 1 || (isSeriesInRows ? (serieNameRange != null ? 1 : 0) : (axisRange != null ? 1 : 0)) == 0 ? (isSeriesInRows ? inputRange[inputRange.Row, serieNameRange.LastColumn + 1, inputRange.LastRow, inputRange.LastColumn] : inputRange[inputRange.Row, axisRange.LastColumn + 1, inputRange.LastRow, inputRange.LastColumn]) : (isSeriesInRows ? inputRange[inputRange.Row, serieNameRange.LastColumn + 1, inputRange.LastRow, inputRange.LastColumn] : inputRange[inputRange.Row, axisRange.LastColumn + 1, inputRange.LastRow, inputRange.LastColumn])) : (isSeriesInRows ? inputRange[axisRange.LastRow + 1, inputRange.Column, inputRange.LastRow, inputRange.LastColumn] : inputRange[serieNameRange.LastRow + 1, inputRange.Column, inputRange.LastRow, inputRange.LastColumn]);
    return inputRange;
  }

  private void AddDefaultRowSerie()
  {
    int row = this.m_dataRange.Row;
    for (int lastRow = this.m_dataRange.LastRow; row <= lastRow; ++row)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.Series.Add();
      chartSerieImpl.ValuesIRange = this.m_dataRange[row, this.m_dataRange.Column, row, this.m_dataRange.LastColumn];
      chartSerieImpl.ValueRangeChanged += new ValueChangedEventHandler(this.serie_ValueRangeChanged);
    }
  }

  private void AddDefaultColumnSerie()
  {
    int row = this.m_dataRange.Row;
    int lastRow = this.m_dataRange.LastRow;
    int lastColumn = this.m_dataRange.LastColumn;
    for (int column = this.m_dataRange.Column; column <= lastColumn; ++column)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.Series.Add();
      chartSerieImpl.ValuesIRange = this.m_dataRange[row, column, lastRow, column];
      chartSerieImpl.ValueRangeChanged += new ValueChangedEventHandler(this.serie_ValueRangeChanged);
    }
  }

  private void AddBubbleRowSerie()
  {
    int lastRow = this.m_dataRange.LastRow;
    int lastColumn = this.m_dataRange.LastColumn;
    int column = this.m_dataRange.Column;
    for (int row = this.m_dataRange.Row; row <= lastRow; row += 2)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.Series.Add();
      chartSerieImpl.BubblesIRange = this.m_dataRange[row, column, row, lastColumn];
      chartSerieImpl.ValuesIRange = this.m_dataRange[row + 1, column, row + 1, lastColumn];
      chartSerieImpl.ValueRangeChanged += new ValueChangedEventHandler(this.serie_ValueRangeChanged);
    }
  }

  private void AddBubbleColumnSerie()
  {
    int row = this.m_dataRange.Row;
    int lastRow = this.m_dataRange.LastRow;
    int lastColumn = this.m_dataRange.LastColumn;
    for (int column = this.m_dataRange.Column; column <= lastColumn; ++column)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.Series.Add();
      chartSerieImpl.BubblesIRange = this.m_dataRange[row, column, lastRow, column];
      chartSerieImpl.ValuesIRange = this.m_dataRange[row, column + 1, lastRow, column + 1];
      chartSerieImpl.ValueRangeChanged += new ValueChangedEventHandler(this.serie_ValueRangeChanged);
    }
  }

  private void AddScatterRowSerie()
  {
    int row = this.m_dataRange.Row;
    int column = this.m_dataRange.Column;
    int lastRow = this.m_dataRange.LastRow;
    int lastColumn = this.m_dataRange.LastColumn;
    for (int index = row + 1; index <= lastRow; index += 2)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.Series.Add();
      chartSerieImpl.CategoryLabelsIRange = this.m_dataRange[row, column, row, lastColumn];
      chartSerieImpl.ValuesIRange = this.m_dataRange[index, column, index, lastColumn];
      chartSerieImpl.ValueRangeChanged += new ValueChangedEventHandler(this.serie_ValueRangeChanged);
    }
  }

  private void AddScatterColumnSerie()
  {
    int row = this.m_dataRange.Row;
    int column = this.m_dataRange.Column;
    int lastRow = this.m_dataRange.LastRow;
    int lastColumn = this.m_dataRange.LastColumn;
    for (int index = column + 1; index <= lastColumn; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.Series.Add();
      chartSerieImpl.CategoryLabelsIRange = this.m_dataRange[row, column, lastRow, column];
      chartSerieImpl.ValuesIRange = this.m_dataRange[row, index, lastRow, index];
      chartSerieImpl.ValueRangeChanged += new ValueChangedEventHandler(this.serie_ValueRangeChanged);
    }
  }

  private void AddStockHLCRowSerie(int count)
  {
    int row = this.m_dataRange.Row;
    int column = this.m_dataRange.Column;
    int lastRow = this.m_dataRange.LastRow;
    int lastColumn = this.m_dataRange.LastColumn;
    if (lastRow - row != count - 1)
      throw new ArgumentOutOfRangeException($"There should be {count.ToString()} rows for this chart type.");
    for (int index = 0; index < count; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.Series.Add();
      chartSerieImpl.ValuesIRange = this.m_dataRange[row + index, column, lastRow + index, column];
      chartSerieImpl.ValueRangeChanged += new ValueChangedEventHandler(this.serie_ValueRangeChanged);
    }
    this.SetStockSerieFormat((ChartSerieImpl) this.Series[count - 1]);
  }

  private void AddStockHLCColumnSerie(int count)
  {
    int row = this.m_dataRange.Row;
    int column = this.m_dataRange.Column;
    int lastRow = this.m_dataRange.LastRow;
    if (this.m_dataRange.LastColumn - column != count - 1)
      throw new ArgumentOutOfRangeException($"There should be {(object) count} columns for this chart type.");
    for (int index = 0; index < count; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.Series.Add();
      chartSerieImpl.ValuesIRange = this.m_dataRange[row, column + index, lastRow, column + index];
      chartSerieImpl.ValueRangeChanged += new ValueChangedEventHandler(this.serie_ValueRangeChanged);
    }
    this.SetStockSerieFormat((ChartSerieImpl) this.Series[count - 1]);
  }

  private void AddStockVolumeRowSerie(int count)
  {
    int row = this.m_dataRange.Row;
    int column = this.m_dataRange.Column;
    int lastRow = this.m_dataRange.LastRow;
    int lastColumn = this.m_dataRange.LastColumn;
    if (lastRow - row != count - 1)
      throw new ArgumentOutOfRangeException($"There should be {count.ToString()} rows for this chart type.");
    ChartSerieImpl chartSerieImpl1 = (ChartSerieImpl) this.m_series.Add();
    chartSerieImpl1.ValuesIRange = this.m_dataRange[row, column, lastRow, column];
    chartSerieImpl1.Number = count - 1;
    chartSerieImpl1.ValueRangeChanged += new ValueChangedEventHandler(this.serie_ValueRangeChanged);
    for (int index = 1; index < count; ++index)
    {
      ChartSerieImpl chartSerieImpl2 = (ChartSerieImpl) this.m_series.Add();
      chartSerieImpl2.ValuesIRange = this.m_dataRange[row + index, column, lastRow + index, column];
      chartSerieImpl2.Number = index - 1;
      chartSerieImpl2.ChartGroup = 1;
      chartSerieImpl2.ValueRangeChanged += new ValueChangedEventHandler(this.serie_ValueRangeChanged);
    }
    if (count == 4)
      this.SetStockSerieFormat((ChartSerieImpl) this.m_series[3]);
    this.SetVolumeSecondaryAxisFormat();
  }

  private void AddStockVolumeColumnSerie(int count)
  {
    int row = this.m_dataRange.Row;
    int column = this.m_dataRange.Column;
    int lastRow = this.m_dataRange.LastRow;
    if (this.m_dataRange.LastColumn - column != count - 1)
      throw new ArgumentOutOfRangeException($"There should be {(object) count} columns for this chart type.");
    ChartSerieImpl chartSerieImpl1 = (ChartSerieImpl) this.m_series.Add();
    chartSerieImpl1.ValuesIRange = this.m_dataRange[row, column, lastRow, column];
    chartSerieImpl1.Number = count - 1;
    chartSerieImpl1.ValueRangeChanged += new ValueChangedEventHandler(this.serie_ValueRangeChanged);
    for (int index = 1; index < count; ++index)
    {
      ChartSerieImpl chartSerieImpl2 = (ChartSerieImpl) this.m_series.Add();
      chartSerieImpl2.ValuesIRange = this.m_dataRange[row, column + index, lastRow, column + index];
      chartSerieImpl2.Number = index - 1;
      chartSerieImpl2.ChartGroup = 1;
      chartSerieImpl2.ValueRangeChanged += new ValueChangedEventHandler(this.serie_ValueRangeChanged);
    }
    if (count == 4)
      this.SetStockSerieFormat((ChartSerieImpl) this.m_series[3]);
    this.SetVolumeSecondaryAxisFormat();
  }

  private void SetVolumeSecondaryAxisFormat()
  {
    this.SecondaryCategoryAxis.IsMaxCross = true;
    ChartCategoryAxisImpl secondaryCategoryAxis = (ChartCategoryAxisImpl) this.SecondaryCategoryAxis;
    this.m_chartProperties.IsManSerAlloc = true;
  }

  private void SetStockSerieFormat(ChartSerieImpl serie)
  {
    if (serie == null)
      throw new ArgumentNullException(nameof (serie));
    ChartSerieDataFormatImpl dataFormat = (ChartSerieDataFormatImpl) ((ChartDataPointImpl) serie.DataPoints.DefaultDataPoint).DataFormat;
    IOfficeChartBorder lineProperties = dataFormat.LineProperties;
    lineProperties.LinePattern = OfficeChartLinePattern.None;
    lineProperties.LineWeight = OfficeChartLineWeight.Hairline;
    dataFormat.PieFormat.Percent = (ushort) 0;
    ChartMarkerFormatRecord markerFormat = dataFormat.MarkerFormat;
    markerFormat.MarkerType = OfficeChartMarkerType.DowJones;
    markerFormat.LineSize = 60;
  }

  private void OnSeriesInRowsChanged()
  {
    if (this.m_dataRange == null)
      return;
    this.OnDataRangeChanged(this.ChartType);
  }

  private void UpdateSeriesInBubleChart()
  {
    ChartSeriesCollection series = this.m_series;
    this.m_series = new ChartSeriesCollection(this.Application, this.m_series.Parent);
    int index = 0;
    for (int count = series.Count; index < count; ++index)
    {
      ChartSerieImpl chartSerieImpl1 = (ChartSerieImpl) this.m_series.Add();
      ChartSerieImpl chartSerieImpl2 = (ChartSerieImpl) series[index];
      chartSerieImpl1.ValuesIRange = chartSerieImpl2.ValuesIRange;
      IRange bubblesIrange = chartSerieImpl2.BubblesIRange;
      if (bubblesIrange != null)
        this.m_series.Add(new ChartSerieImpl(this.Application, (object) this.m_series)
        {
          ValuesIRange = bubblesIrange
        });
    }
  }

  private void OnChartTypeChanged(OfficeChartType type, bool isSeriesCreation)
  {
    if (type == OfficeChartType.Combination_Chart)
      throw new ArgumentException("Cannot change chart type.");
    this.HasDataTable = false;
    this.m_series.ClearErrorBarsAndTrends();
    if (this.ChartStartType == "Bubble")
      this.UpdateSeriesInBubleChart();
    this.m_primaryParentAxis.Formats.Clear();
    this.m_series.ClearSeriesForChangeChartType(!this.Loading && !ChartImpl.IsChartExSerieType(type) && !ChartImpl.IsChartExSerieType(this.m_chartType) && Array.IndexOf<OfficeChartType>(ChartImpl.DEF_NOT_3D, type) == -1 && Array.IndexOf<OfficeChartType>(ChartImpl.DEF_NOT_SUPPORT_GRIDLINES, type) == -1 && Array.IndexOf<OfficeChartType>(ChartImpl.DEF_NEED_VIEW_3D, type) == -1);
    if (Array.IndexOf<OfficeChartType>(ChartImpl.CHARTS_STOCK, type) != -1)
    {
      this.ChangeChartStockType(type);
    }
    else
    {
      ChartFormatImpl formatToAdd = new ChartFormatImpl(this.Application, (object) this.PrimaryFormats);
      this.PrimaryFormats.Add(formatToAdd, false);
      formatToAdd.ChangeChartType(type, isSeriesCreation);
      this.UpdateChartMembersOnTypeChanging(type, false);
    }
  }

  private void UpdateSurfaceTickRecord()
  {
    ((ChartAxisImpl) this.PrimaryCategoryAxis).UpdateTickRecord(OfficeTickLabelPosition.TickLabelPosition_Low);
    ((ChartAxisImpl) this.PrimaryValueAxis).UpdateTickRecord(OfficeTickLabelPosition.TickLabelPosition_None);
    ((ChartAxisImpl) this.PrimarySerieAxis).UpdateTickRecord(OfficeTickLabelPosition.TickLabelPosition_Low);
  }

  private void UpdateRadarTickRecord()
  {
    ((ChartAxisImpl) this.PrimaryCategoryAxis).UpdateTickRecord(OfficeTickLabelPosition.TickLabelPosition_NextToAxis);
    ((ChartAxisImpl) this.PrimaryValueAxis).UpdateTickRecord(OfficeTickLabelPosition.TickLabelPosition_NextToAxis);
  }

  private void UpdateChartMembersOnTypeChanging(OfficeChartType type, bool isChartExChanges)
  {
    if (this.m_book.IsWorkbookOpening || isChartExChanges)
      return;
    if (!this.m_bHasLegend)
    {
      this.m_bHasLegend = true;
      this.m_legend = new ChartLegendImpl(this.Application, (object) this);
    }
    this.m_primaryParentAxis.ClearGridLines();
    this.SetToDefaultGridlines(type);
    this.m_sidewall = new ChartWallOrFloorImpl(this.Application, (object) this, true);
    this.m_walls = new ChartWallOrFloorImpl(this.Application, (object) this, true);
    this.m_floor = new ChartWallOrFloorImpl(this.Application, (object) this, false);
    if (!isChartExChanges)
      this.m_plotArea = new ChartPlotAreaImpl(this.Application, (object) this, type);
    if (Array.IndexOf<OfficeChartType>(ChartImpl.CHARTS3D, this.ChartType) != -1)
      ((ChartAxisImpl) this.PrimaryCategoryAxis).UpdateTickRecord(OfficeTickLabelPosition.TickLabelPosition_Low);
    if (Array.IndexOf<OfficeChartType>(ChartImpl.DEF_SUPPORT_SERIES_AXIS, type) != -1)
      ((ChartAxisImpl) this.m_primaryParentAxis.SeriesAxis ?? (ChartAxisImpl) (this.m_primaryParentAxis.SeriesAxis = new ChartSeriesAxisImpl(this.Application, (object) this.m_primaryParentAxis, OfficeAxisType.Serie))).UpdateTickRecord(OfficeTickLabelPosition.TickLabelPosition_Low);
    else
      this.m_primaryParentAxis.SeriesAxis = (ChartSeriesAxisImpl) null;
    switch (type)
    {
      case OfficeChartType.Radar:
      case OfficeChartType.Radar_Markers:
      case OfficeChartType.Radar_Filled:
        this.UpdateRadarTickRecord();
        break;
      case OfficeChartType.Surface_Contour:
      case OfficeChartType.Surface_NoColor_Contour:
        this.UpdateSurfaceTickRecord();
        if (type != OfficeChartType.Surface_NoColor_Contour)
          break;
        this.m_sidewall.Interior.Pattern = OfficePattern.None;
        this.m_walls.Interior.Pattern = OfficePattern.None;
        this.m_floor.Interior.Pattern = OfficePattern.None;
        break;
      default:
        ((ChartAxisImpl) this.PrimaryValueAxis).UpdateTickRecord(OfficeTickLabelPosition.TickLabelPosition_Low);
        break;
    }
  }

  internal static bool IsChartExSerieType(OfficeChartType type)
  {
    switch (type)
    {
      case OfficeChartType.Pareto:
      case OfficeChartType.Funnel:
      case OfficeChartType.Histogram:
      case OfficeChartType.WaterFall:
      case OfficeChartType.TreeMap:
      case OfficeChartType.SunBurst:
      case OfficeChartType.BoxAndWhisker:
        return true;
      default:
        return false;
    }
  }

  internal void ChangeToChartExType(
    OfficeChartType oldChartType,
    OfficeChartType type,
    bool isSeriesCreation)
  {
    this.m_series.ClearErrorBarsAndTrends();
    bool flag = (oldChartType == OfficeChartType.Pareto || oldChartType == OfficeChartType.Histogram) && type != OfficeChartType.Histogram && type != OfficeChartType.Pareto;
    if (isSeriesCreation && type != oldChartType && !flag)
      this.m_series.ClearSeriesForChangeChartType();
    this.HasDataTable = false;
    this.PrimaryParentAxis.Formats.Clear();
    ChartFormatImpl formatToAdd = new ChartFormatImpl(this.Application, (object) this.PrimaryFormats);
    this.PrimaryFormats.Add(formatToAdd, false);
    formatToAdd.ChangeChartType(type, isSeriesCreation);
    this.UpdateChartMembersOnTypeChanging(type, true);
    if (!this.Loading)
    {
      switch (type)
      {
        case OfficeChartType.Pareto:
        case OfficeChartType.Histogram:
          formatToAdd.GapWidth = 0;
          break;
        case OfficeChartType.Funnel:
          formatToAdd.GapWidth = 6;
          break;
        case OfficeChartType.BoxAndWhisker:
          formatToAdd.GapWidth = 50;
          break;
      }
    }
    else
      formatToAdd.GapWidth = 100;
    this.m_chartType = type;
    if (ChartImpl.IsChartExSerieType(oldChartType))
    {
      this.PrimaryCategoryAxis.MajorTickMark = OfficeTickMark.TickMark_None;
      this.PrimaryValueAxis.MajorTickMark = OfficeTickMark.TickMark_None;
    }
    if (type == OfficeChartType.Pareto)
    {
      for (int index = 0; index < this.m_series.Count; ++index)
        (this.m_series[index] as ChartSerieImpl).ParetoLineFormat = (IOfficeChartFrameFormat) new ChartFrameFormatImpl(this.Application, (object) this.m_series[index]);
      this.m_secondaryParentAxis.UpdateSecondaryAxis(true);
      this.SecondaryFormats.Add(new ChartFormatImpl(this.Application, (object) this.SecondaryFormats)
      {
        DrawingZOrder = 1
      });
      this.SecondaryFormats.IsParetoFormat = true;
      if (!ChartImpl.IsChartExSerieType(oldChartType))
        this.SecondaryValueAxis.MajorTickMark = OfficeTickMark.TickMark_None;
    }
    else if (oldChartType == OfficeChartType.Pareto)
    {
      for (int index = 0; index < this.m_series.Count; ++index)
        (this.m_series[index] as ChartSerieImpl).ParetoLineFormat = (IOfficeChartFrameFormat) null;
      this.SecondaryFormats.IsParetoFormat = false;
    }
    if (flag)
    {
      for (int index = 0; index < this.m_series.Count; ++index)
        (this.m_series[index].SerieFormat as ChartSerieDataFormatImpl).HistogramAxisFormatProperty = (HistogramAxisFormat) null;
    }
    if (this.Loading)
      return;
    if (!isSeriesCreation && (type == OfficeChartType.TreeMap || type == OfficeChartType.SunBurst))
    {
      for (int index = 0; index < this.m_series.Count; ++index)
      {
        ChartDataPointImpl defaultDataPoint = this.m_series[index].DataPoints.DefaultDataPoint as ChartDataPointImpl;
        if (!defaultDataPoint.HasDataLabels)
        {
          defaultDataPoint.DataLabels.IsCategoryName = true;
          defaultDataPoint.DataLabels.Delimiter = ",";
          defaultDataPoint.DataLabels.Position = OfficeDataLabelPosition.Inside;
        }
      }
    }
    else if (isSeriesCreation && (type == OfficeChartType.Funnel || type == OfficeChartType.WaterFall))
    {
      for (int index = 0; index < this.m_series.Count; ++index)
      {
        ChartDataPointImpl defaultDataPoint = this.m_series[index].DataPoints.DefaultDataPoint as ChartDataPointImpl;
        if (!defaultDataPoint.HasDataLabels)
        {
          defaultDataPoint.DataLabels.IsValue = true;
          defaultDataPoint.DataLabels.Delimiter = ",";
          defaultDataPoint.DataLabels.Position = OfficeDataLabelPosition.Inside;
        }
      }
    }
    if (!this.m_bHasLegend || this.m_legend == null)
      return;
    this.m_legend.IncludeInLayout = false;
    this.m_legend.Position = OfficeLegendPosition.Top;
  }

  private void ChangeChartStockType(OfficeChartType type)
  {
    int count = this.m_series.Count;
    switch (type)
    {
      case OfficeChartType.Stock_HighLowClose:
        if (count != 3)
          throw new ArgumentException("Cannot change serie type.");
        ChartFormatImpl formatToAdd1 = new ChartFormatImpl(this.Application, (object) this.PrimaryFormats);
        this.PrimaryFormats.Add(formatToAdd1);
        formatToAdd1.ChangeChartStockHigh_Low_CloseType();
        break;
      case OfficeChartType.Stock_OpenHighLowClose:
        if (count != 4)
          throw new ArgumentException("Cannot change serie type.");
        ChartFormatImpl formatToAdd2 = new ChartFormatImpl(this.Application, (object) this.PrimaryFormats);
        this.PrimaryFormats.Add(formatToAdd2);
        formatToAdd2.ChangeChartStockOpen_High_Low_CloseType();
        break;
      case OfficeChartType.Stock_VolumeHighLowClose:
        if (count != 4)
          throw new ArgumentException("Cannot change serie type.");
        ChartFormatImpl formatToAdd3 = new ChartFormatImpl(this.Application, (object) this.PrimaryFormats);
        this.PrimaryFormats.Add(formatToAdd3);
        formatToAdd3.ChangeChartStockVolume_High_Low_CloseTypeFirst();
        ChartFormatImpl formatToAdd4 = new ChartFormatImpl(this.Application, (object) this.SecondaryFormats);
        formatToAdd4.DrawingZOrder = 1;
        this.SecondaryFormats.Add(formatToAdd4);
        formatToAdd4.ChangeChartStockVolume_High_Low_CloseTypeSecond();
        break;
      case OfficeChartType.Stock_VolumeOpenHighLowClose:
        if (count != 5)
          throw new ArgumentException("Cannot change serie type.");
        this.IsManuallyFormatted = true;
        this.m_secondaryParentAxis.UpdateSecondaryAxis(true);
        ChartFormatImpl formatToAdd5 = new ChartFormatImpl(this.Application, (object) this.PrimaryFormats);
        this.PrimaryFormats.Add(formatToAdd5);
        formatToAdd5.ChangeChartStockVolume_High_Low_CloseTypeFirst();
        ChartFormatImpl formatToAdd6 = new ChartFormatImpl(this.Application, (object) this.SecondaryFormats);
        formatToAdd6.DrawingZOrder = 1;
        this.SecondaryFormats.Add(formatToAdd6);
        formatToAdd6.ChangeChartStockVolume_Open_High_Low_CloseType();
        this.SecondaryCategoryAxis.IsMaxCross = true;
        break;
      default:
        throw new ArgumentException(nameof (type));
    }
    this.IsStock = true;
  }

  private void UpdateSeriesByDataRange(
    IRange serieValue,
    IRange serieNameRange,
    IRange axisRange,
    string strType,
    int iIndex,
    bool isSeriesInRows)
  {
    bool isClearNameRange = serieNameRange == null;
    if (strType == "Bubble")
    {
      int num1 = 0;
      int num2 = 0;
      int count = this.m_series.Count;
      while (num2 < count)
      {
        IRange range = isSeriesInRows ? serieValue[serieValue.Row + num1, serieValue.Column, serieValue.Row + num1, serieValue.LastColumn] : serieValue[serieValue.Row, serieValue.Column + num1, serieValue.LastRow, serieValue.Column + num1];
        if ((isSeriesInRows ? (serieValue.Rows.Length >= num1 + 2 ? 1 : 0) : (serieValue.Columns.Length >= num1 + 2 ? 1 : 0)) != 0)
          this.Series[num2].Bubbles = isSeriesInRows ? (IOfficeDataRange) serieValue[serieValue.Row + num1 + 1, serieValue.Column, serieValue.Row + num1 + 1, serieValue.LastColumn] : (IOfficeDataRange) serieValue[serieValue.Row, serieValue.Column + num1 + 1, serieValue.LastRow, serieValue.Column + num1 + 1];
        ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.Series[num2];
        chartSerieImpl.SetDefaultName(this.m_series.GetDefSerieName(num2), isClearNameRange);
        int num3 = iIndex;
        chartSerieImpl.ValuesIRange = range;
        if (serieNameRange != null)
        {
          int num4 = num3 + (isSeriesInRows ? serieNameRange.Row : serieNameRange.Column);
          string str = isSeriesInRows ? serieValue[num4 + num1, serieNameRange.Column, num4 + num1, serieNameRange.LastColumn].AddressGlobal : serieValue[serieNameRange.Row, num4 + num1, serieNameRange.LastRow, num4 + num1].AddressGlobal;
          chartSerieImpl.Name = "=" + str;
        }
        ++num2;
        num1 += 2;
      }
    }
    else
    {
      if (axisRange != null && !axisRange.IsBlank && strType == "Pareto")
        this.PrimaryCategoryAxis.IsBinningByCategory = true;
      int num5 = 0;
      for (int count = this.m_series.Count; num5 < count; ++num5)
      {
        IRange range = isSeriesInRows ? serieValue[serieValue.Row + num5, serieValue.Column, serieValue.Row + num5, serieValue.LastColumn] : serieValue[serieValue.Row, serieValue.Column + num5, serieValue.LastRow, serieValue.Column + num5];
        ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.Series[num5];
        chartSerieImpl.BubblesIRange = (IRange) null;
        chartSerieImpl.SetDefaultName(this.m_series.GetDefSerieName(num5), isClearNameRange);
        int num6 = iIndex;
        chartSerieImpl.ValuesIRange = range;
        if (serieNameRange != null)
        {
          int num7 = num6 + (isSeriesInRows ? serieNameRange.Row : serieNameRange.Column);
          string str = isSeriesInRows ? serieValue[num7 + num5, serieNameRange.Column, num7 + num5, serieNameRange.LastColumn].AddressGlobal : serieValue[serieNameRange.Row, num7 + num5, serieNameRange.LastRow, num7 + num5].AddressGlobal;
          chartSerieImpl.Name = "=" + str;
        }
      }
    }
  }

  private Chart3DRecord GetChart3D()
  {
    Chart3DRecord record = (Chart3DRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Chart3D);
    record.IsPerspective = this.IsPerspective;
    record.IsClustered = this.IsClustered;
    switch (this.ChartType)
    {
      case OfficeChartType.Pie_3D:
      case OfficeChartType.Pie_Exploded_3D:
        record.RotationAngle = (ushort) 0;
        record.IsAutoScaled = false;
        record.Is2DWalls = false;
        break;
      case OfficeChartType.Surface_Contour:
      case OfficeChartType.Surface_NoColor_Contour:
        record.RotationAngle = (ushort) 0;
        record.ElevationAngle = (short) 90;
        record.DistanceFromEye = (ushort) 0;
        break;
    }
    return record;
  }

  private void serie_ValueRangeChanged(object sender, ValueChangedEventArgs e)
  {
    this.m_dataRange = (IRange) null;
  }

  private void InitializeFrames()
  {
    this.m_chartArea = new ChartFrameFormatImpl(this.Application, (object) this);
    this.m_chartArea.Interior.ForegroundColorIndex = OfficeKnownColors.WhiteCustom;
    this.m_plotAreaFrame = new ChartFrameFormatImpl(this.Application, (object) this, true, false, true);
  }

  public void RemoveFormat(IOfficeChartFormat formatToRemove)
  {
    if (formatToRemove == null)
      throw new ArgumentNullException(nameof (formatToRemove));
    this.m_primaryParentAxis.Formats.Remove((ChartFormatImpl) formatToRemove);
  }

  public void UpdateChartTitle()
  {
    if (this.m_title == null)
      return;
    ChartTextAreaImpl chartTitleArea = this.ChartTitleArea as ChartTextAreaImpl;
    if (this.ChartTitle != null || !chartTitleArea.TextRecord.IsAutoMode)
      return;
    int count = this.Series.Count;
    if (count <= 0)
      return;
    ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.Series[0];
    if (chartSerieImpl.IsDefaultName)
      return;
    string chartTypeStart = count != 1 ? this.GetChartTypeStart() : (string) null;
    if (count != 1 && !(chartTypeStart == "Pie") && !(chartTypeStart == "Doughnut"))
      return;
    this.ChartTitle = chartSerieImpl.ParseSerieNotDefaultText;
  }

  private string GetChartTypeStart()
  {
    if (this.m_series.Count == 0)
      return (string) null;
    string chartTypeStart = (this.m_series[0] as ChartSerieImpl).DetectSerieTypeStart();
    int index = 1;
    for (int count = this.m_series.Count; index < count; ++index)
    {
      if ((this.m_series[index] as ChartSerieImpl).DetectSerieTypeStart() != chartTypeStart)
      {
        chartTypeStart = (string) null;
        break;
      }
    }
    return chartTypeStart;
  }

  internal IRange DetectDataRange()
  {
    if (this.m_series.Count == 0)
      return (IRange) null;
    ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.Series[0];
    IRange valuesIrange = chartSerieImpl.ValuesIRange;
    IRange serieNameRange = chartSerieImpl.GetSerieNameRange();
    IRange bubblesIrange = chartSerieImpl.BubblesIRange;
    if (valuesIrange == null || valuesIrange.Worksheet == null)
      return (IRange) null;
    IWorksheet worksheet = valuesIrange.Worksheet;
    string name = worksheet.Name;
    IRange seriesValuesRange = this.GetSeriesValuesRange(valuesIrange, bubblesIrange, worksheet, name);
    IRange range = (this.PrimaryCategoryAxis.CategoryLabels as ChartDataRange).Range;
    if (range != null && range.Worksheet != null && name != range.Worksheet.Name)
      return (IRange) null;
    if (seriesValuesRange == null)
      return (IRange) null;
    IRange result;
    if (!this.GetSerieNameValuesRange(serieNameRange, bubblesIrange, worksheet, name, out result))
      return (IRange) null;
    Rectangle rectangeOfRange = RangeImpl.GetRectangeOfRange(seriesValuesRange, true);
    if (result != null && !this.GetDataRangeRec(result, ref rectangeOfRange, !this.m_bSeriesInRows))
      return (IRange) null;
    return range != null && !this.GetDataRangeRec(range, ref rectangeOfRange, this.m_bSeriesInRows) ? (IRange) null : worksheet[rectangeOfRange.Top, rectangeOfRange.Left, rectangeOfRange.Bottom, rectangeOfRange.Right];
  }

  private bool DetectIsInRow(IRange range)
  {
    return range == null || range.LastRow - range.Row <= range.LastColumn - range.Column;
  }

  public IRange GetSerieOrAxisRange(IRange range, bool bIsInRow, out IRange serieRange)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    int num1 = bIsInRow ? range.Row : range.Column;
    int num2 = bIsInRow ? range.LastRow : range.LastColumn;
    int num3 = bIsInRow ? range.Column : range.Row;
    int num4 = bIsInRow ? range.LastColumn : range.LastRow;
    int num5 = -1;
    bool flag = false;
    for (int index = num3; index < num4 && !flag; ++index)
    {
      IRange range1 = bIsInRow ? range[num2, index] : range[index, num2];
      flag = range1.HasNumber || range1.IsBlank || range1.HasFormula;
      if (!flag)
        num5 = index;
    }
    if (num5 == -1)
    {
      serieRange = range;
      return (IRange) null;
    }
    IRange serieOrAxisRange = bIsInRow ? range[num1, num3, num2, num5] : range[num3, num1, num5, num2];
    serieRange = bIsInRow ? range[range.Row, serieOrAxisRange.LastColumn + 1, range.LastRow, range.LastColumn] : range[serieOrAxisRange.LastRow + 1, range.Column, range.LastRow, range.LastColumn];
    return serieOrAxisRange;
  }

  private bool ValidateSerieRangeForChartType(
    IRange serieValue,
    OfficeChartType type,
    bool isSeriesInRows)
  {
    if (serieValue == null)
      throw new ArgumentNullException(nameof (serieValue));
    string startSerieType = ChartFormatImpl.GetStartSerieType(type);
    int num1 = this.m_bSeriesInRows ? serieValue.LastRow - serieValue.Row + 1 : serieValue.LastColumn - serieValue.Column + 1;
    if (num1 < 2 && (startSerieType == "Bubble" || startSerieType == "Surface") || type == OfficeChartType.Stock_HighLowClose && num1 != 3 || type == OfficeChartType.Stock_OpenHighLowClose || type == OfficeChartType.Stock_VolumeHighLowClose && num1 != 4 || type == OfficeChartType.Stock_VolumeOpenHighLowClose && num1 != 5)
      return false;
    if (startSerieType == "Bubble")
      num1 = num1 / 2 + num1 % 2;
    int count = this.m_series.Count;
    bool flag = count > num1;
    int num2 = flag ? num1 : count;
    int num3 = flag ? count : num1;
    for (int index = num2; index < num3; ++index)
    {
      if (flag)
        this.m_series.RemoveAt(num3 - index + num2 - 1);
      else
        this.m_series.Add();
    }
    return true;
  }

  private bool CompareSeriesValues(Rectangle rec, IRange range, int i, string strSheetName)
  {
    if ((strSheetName == null || strSheetName.Length == 0) && !(range.Worksheet is ExternWorksheetImpl))
      throw new ArgumentNullException(nameof (strSheetName));
    return range != null && (range.Worksheet == null || !(range.Worksheet.Name != strSheetName)) && (this.m_bSeriesInRows ? range.Row == rec.Top + i && range.LastRow == rec.Bottom + i && range.Column == rec.Left && range.LastColumn == rec.Right : range.Row == rec.Top && range.LastRow == rec.Bottom && range.Column == rec.Left + i && range.LastColumn == rec.Right + i);
  }

  internal IRange GetSeriesValuesRange(
    IRange lastRange,
    IRange buble,
    IWorksheet sheet,
    string strSheetName)
  {
    if (lastRange == null)
      throw new ArgumentNullException(nameof (lastRange));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    Rectangle rectangeOfRange = RangeImpl.GetRectangeOfRange(lastRange, true);
    bool flag = this.ChartStartType == "Bubble";
    int count = this.Series.Count;
    int num = 0;
    if (flag && buble != null)
    {
      if (!this.CompareSeriesValues(rectangeOfRange, buble, 1, strSheetName))
        return (IRange) null;
      if (count == 1)
        ++num;
    }
    for (int index = 1; index < count; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.Series[index];
      lastRange = chartSerieImpl.ValuesIRange;
      if (!this.CompareSeriesValues(rectangeOfRange, lastRange, !flag || chartSerieImpl.Bubbles == null ? index : index * 2, strSheetName))
        return (IRange) null;
      if (flag)
      {
        buble = chartSerieImpl.BubblesIRange;
        if (count - index > 1 && !this.CompareSeriesValues(rectangeOfRange, lastRange, index * 2 + index, strSheetName))
          return (IRange) null;
        if (count == index && buble != null)
        {
          if (!this.CompareSeriesValues(rectangeOfRange, lastRange, index * 2 + index, strSheetName))
            return (IRange) null;
          ++num;
        }
      }
    }
    return !this.m_bSeriesInRows ? sheet[rectangeOfRange.Top, rectangeOfRange.Left, lastRange.LastRow, lastRange.LastColumn + num] : sheet[rectangeOfRange.Top, rectangeOfRange.Left, lastRange.LastRow + num, lastRange.LastColumn];
  }

  private bool GetSerieNameValuesRange(
    IRange lastRange,
    IRange bubles,
    IWorksheet sheet,
    string strSheetName,
    out IRange result)
  {
    bool flag1 = lastRange == null;
    Rectangle rec = new Rectangle(0, 0, 0, 0);
    bool flag2 = this.ChartStartType == "Bubble";
    result = (IRange) null;
    int count = this.Series.Count;
    int num = 0;
    if (flag2 && bubles != null && count == 1)
      ++num;
    if (!flag1)
      rec = RangeImpl.GetRectangeOfRange(lastRange, true);
    for (int index = 1; index < count; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.Series[index];
      lastRange = chartSerieImpl.GetSerieNameRange();
      if (flag1 != (lastRange == null))
        return false;
      if (flag2 && count - index > 1 && chartSerieImpl.BubblesIRange != null)
        ++num;
      if (lastRange != null)
      {
        if (!this.CompareSeriesValues(rec, lastRange, !flag2 || chartSerieImpl.Bubbles == null ? index : index * 2, strSheetName))
          return false;
      }
      else if (!chartSerieImpl.IsDefaultName)
        return false;
    }
    if (flag1)
      return true;
    result = this.m_bSeriesInRows ? sheet[rec.Top, rec.Left, lastRange.LastRow + num, lastRange.LastColumn] : sheet[rec.Top, rec.Left, lastRange.LastRow, lastRange.LastColumn + num];
    return true;
  }

  private bool GetDataRangeRec(IRange range, ref Rectangle rec, bool inRow)
  {
    if (range == null)
      throw new ArgumentNullException("values");
    if (range != null)
    {
      if (!(inRow ? rec.Right == range.LastColumn && rec.Top == range.LastRow + 1 : rec.Bottom == range.LastRow && rec.Left == range.Column + 1))
        return false;
      if (inRow)
      {
        rec.Y = range.Row;
        ++rec.Height;
      }
      else
      {
        rec.X = range.Column;
        ++rec.Width;
      }
    }
    return true;
  }

  public void DetectIsInRowOnParsing()
  {
    if (this.m_series.Count == 0)
    {
      this.m_bSeriesInRows = false;
    }
    else
    {
      IRange range = (this.m_series[0].Values as ChartDataRange).Range;
      if (range == null)
        this.m_bSeriesInRows = false;
      else
        this.m_bSeriesInRows = this.DetectIsInRow(range);
    }
  }

  internal void ChangeChartType(OfficeChartType newChartType, bool isSeriesCreation)
  {
    if (this.ChartType == newChartType)
      return;
    if (this.IsChartStock)
      this.IsStock = false;
    this.TypeChanging = true;
    this.DestinationType = newChartType;
    this.UpdateChartType(this.ChartType, newChartType, isSeriesCreation);
    this.m_chartType = newChartType;
    this.TypeChanging = false;
  }

  protected override OfficeSheetProtection PrepareProtectionOptions(OfficeSheetProtection options)
  {
    return options |= OfficeSheetProtection.Scenarios;
  }

  private void UpdateChartType(
    OfficeChartType oldChartType,
    OfficeChartType newChartType,
    bool isSeriesCreation)
  {
    if (ChartImpl.IsChartExSerieType(newChartType))
    {
      bool flag = !ChartImpl.IsChartExSerieType(oldChartType);
      this.ChangeToChartExType(oldChartType, newChartType, isSeriesCreation);
      foreach (IOfficeChartSerie officeChartSerie in (IEnumerable<IOfficeChartSerie>) this.Series)
      {
        ChartSerieImpl chartSerieImpl = officeChartSerie as ChartSerieImpl;
        chartSerieImpl.Number = -1;
        if (flag)
        {
          chartSerieImpl.UpdateChartExSerieRangesMembers(true);
          chartSerieImpl.UpdateChartExSerieRangesMembers(false);
          if (chartSerieImpl.SerieFormat.HasInterior)
            (chartSerieImpl.SerieFormat.Interior as ChartInteriorImpl).UseAutomaticFormat = true;
          if (chartSerieImpl.SerieFormat.HasLineProperties)
            chartSerieImpl.SerieFormat.LineProperties.AutoFormat = true;
        }
      }
      this.ClearDependentStreams();
    }
    else
    {
      if (ChartImpl.IsChartExSerieType(oldChartType))
      {
        int num = 0;
        foreach (IOfficeChartSerie officeChartSerie in (IEnumerable<IOfficeChartSerie>) this.Series)
        {
          (officeChartSerie as ChartSerieImpl).Number = num;
          if (officeChartSerie.IsFiltered)
            officeChartSerie.IsFiltered = false;
          ++num;
        }
        this.ClearDependentStreams();
      }
      this.OnChartTypeChanged(newChartType, isSeriesCreation);
    }
  }

  private void ClearDependentStreams()
  {
    this.m_defaultTextProperty = (Stream) null;
    this.m_colorMapOverrideStream = (MemoryStream) null;
    if (this.m_primaryParentAxis.CategoryAxis != null)
      this.m_primaryParentAxis.CategoryAxis.TextStream = (Stream) null;
    if (this.m_primaryParentAxis.ValueAxis != null)
      this.m_primaryParentAxis.ValueAxis.TextStream = (Stream) null;
    if (this.m_secondaryParentAxis.CategoryAxis != null)
      this.m_secondaryParentAxis.CategoryAxis.TextStream = (Stream) null;
    if (this.m_secondaryParentAxis.ValueAxis == null)
      return;
    this.m_secondaryParentAxis.ValueAxis.TextStream = (Stream) null;
  }

  private new object Clone(object parent)
  {
    return (object) this.Clone((Dictionary<string, string>) null, parent, (Dictionary<int, int>) null);
  }

  internal IOfficeChart Clone()
  {
    return this.Clone((object) ((this.Workbook.Clone() as WorkbookImpl).Worksheets[0] as WorksheetImpl)) as IOfficeChart;
  }

  internal ChartImpl Clone(
    Dictionary<string, string> hashNewNames,
    object parent,
    Dictionary<int, int> dicFontIndexes)
  {
    ChartImpl chartImpl = (ChartImpl) base.Clone(parent);
    chartImpl.SetParent(parent);
    chartImpl.FindParents();
    if (this._chartData != null)
      chartImpl._chartData = this._chartData.Clone((IOfficeChart) chartImpl);
    chartImpl.m_arrFonts = CloneUtils.CloneCloneable<ChartFbiRecord>(this.m_arrFonts);
    if (this.m_arrFonts != null)
      chartImpl.UpdateChartFbiIndexes((IDictionary) dicFontIndexes);
    if (this.m_arrRecords != null)
      chartImpl.m_arrRecords = CloneUtils.CloneCloneable(this.m_arrRecords);
    if (this.m_chartProperties != null)
      chartImpl.m_chartProperties = (ChartShtpropsRecord) this.m_chartProperties.Clone();
    if (this.m_plotArea != null)
      chartImpl.m_plotArea = (ChartPlotAreaImpl) this.m_plotArea.Clone((object) chartImpl);
    if (this.m_dataTable != null)
      chartImpl.m_dataTable = this.m_dataTable.Clone((object) chartImpl);
    if (this.m_chartArea != null)
      chartImpl.m_chartArea = this.m_chartArea.Clone((object) chartImpl);
    if (this.m_lstDefaultText != null)
    {
      chartImpl.m_lstDefaultText = this.m_lstDefaultText.CloneAll();
      chartImpl.UpdateChartFontXIndexes((IDictionary) dicFontIndexes);
    }
    if (this.m_pageSetup != null)
      chartImpl.m_pageSetup = this.m_pageSetup.Clone((object) chartImpl);
    this.m_plotAreaBoundingBox = (ChartPosRecord) CloneUtils.CloneCloneable((ICloneable) this.m_plotAreaBoundingBox);
    if (this.m_plotAreaFrame != null)
      chartImpl.m_plotAreaFrame = this.m_plotAreaFrame.Clone((object) chartImpl);
    this.m_plotGrowth = (ChartPlotGrowthRecord) CloneUtils.CloneCloneable((ICloneable) this.m_plotGrowth);
    if (this.m_series != null)
      chartImpl.m_series = this.m_series.Clone((object) chartImpl, hashNewNames, dicFontIndexes);
    if (this.m_title != null)
      chartImpl.m_title = (ChartTextAreaImpl) this.m_title.Clone((object) chartImpl, dicFontIndexes, hashNewNames);
    if (this.m_primaryParentAxis != null)
      chartImpl.m_primaryParentAxis = this.m_primaryParentAxis.Clone((object) chartImpl, dicFontIndexes, hashNewNames);
    if (this.m_secondaryParentAxis != null)
      chartImpl.m_secondaryParentAxis = this.m_secondaryParentAxis.Clone((object) chartImpl, dicFontIndexes, hashNewNames);
    if (this.m_legend != null)
      chartImpl.m_legend = this.m_legend.Clone((object) chartImpl, dicFontIndexes, hashNewNames);
    if (this.m_walls != null)
      chartImpl.m_walls = (ChartWallOrFloorImpl) this.m_walls.Clone((object) chartImpl);
    if (this.m_themeColors != null)
      chartImpl.m_themeColors = CloneUtils.CloneCloneable<Color>(this.m_themeColors);
    if (this.m_themeOverrideStream != null)
    {
      this.m_themeOverrideStream.Position = 0L;
      chartImpl.m_themeOverrideStream = (MemoryStream) CloneUtils.CloneStream((Stream) this.m_themeOverrideStream);
    }
    chartImpl.m_categoriesLabelLevel = this.m_categoriesLabelLevel;
    chartImpl.m_seriesNameLevel = this.m_seriesNameLevel;
    if (this.m_font != null)
      chartImpl.m_font = (FontWrapper) this.m_font.Clone((object) chartImpl);
    if (this.m_defaultTextProperty != null)
    {
      this.m_defaultTextProperty.Position = 0L;
      chartImpl.m_defaultTextProperty = CloneUtils.CloneStream(this.m_defaultTextProperty);
    }
    if (this.m_bandFormats != null)
    {
      this.m_bandFormats.Position = 0L;
      chartImpl.m_bandFormats = CloneUtils.CloneStream(this.m_bandFormats);
    }
    if (this.m_pivotFormatsStream != null)
    {
      this.m_pivotFormatsStream.Position = 0L;
      chartImpl.m_pivotFormatsStream = CloneUtils.CloneStream(this.m_pivotFormatsStream);
    }
    if (this.m_alternateContent != null)
    {
      this.m_alternateContent.Position = 0L;
      chartImpl.m_alternateContent = CloneUtils.CloneStream(this.m_alternateContent);
    }
    if (this.m_relations != null)
      chartImpl.m_relations = this.m_relations.Clone();
    if (this.m_trendList != null)
      chartImpl.m_trendList = CloneUtils.CloneCloneable(this.m_trendList);
    if (this.m_pivotList != null)
      chartImpl.m_pivotList = CloneUtils.CloneCloneable(this.m_pivotList);
    if (this.m_sidewall != null)
      chartImpl.m_sidewall = (ChartWallOrFloorImpl) this.m_sidewall.Clone((object) chartImpl);
    if (this.m_floor != null)
      chartImpl.m_floor = (ChartWallOrFloorImpl) this.m_floor.Clone((object) chartImpl);
    if (this.m_chartChartZoom != null)
      chartImpl.m_chartChartZoom = this.m_chartChartZoom;
    chartImpl.m_chartType = this.m_chartType;
    chartImpl.m_pivotChartType = this.m_pivotChartType;
    if (this.m_dataRange != null)
      chartImpl.m_dataRange = this.m_dataRange;
    chartImpl.m_destinationType = this.m_destinationType;
    if (this.m_dictReparseErrorBars != null)
      chartImpl.m_dictReparseErrorBars = CloneUtils.CloneHash<int, List<BiffRecordRaw>>(this.m_dictReparseErrorBars);
    if (this.m_plotAreaLayout != null)
      chartImpl.m_plotAreaLayout = (ChartPlotAreaLayoutRecord) this.m_plotAreaLayout.Clone();
    if (this.m_categoryLabelValues != null)
      chartImpl.m_categoryLabelValues = CloneUtils.CloneArray(this.m_categoryLabelValues);
    chartImpl.m_isChartStyleSkipped = this.m_isChartStyleSkipped;
    chartImpl.m_isChartColorStyleSkipped = this.m_isChartColorStyleSkipped;
    if (this.m_relationPreservedStreamCollections != null && this.m_relationPreservedStreamCollections.Count > 0)
    {
      chartImpl.m_relationPreservedStreamCollections = new Dictionary<string, Stream>();
      foreach (KeyValuePair<string, Stream> streamCollection in this.m_relationPreservedStreamCollections)
      {
        streamCollection.Value.Position = 0L;
        Stream stream = CloneUtils.CloneStream(streamCollection.Value);
        chartImpl.m_relationPreservedStreamCollections.Add(streamCollection.Key, stream);
      }
    }
    if (this.CommonDataPointsCollection != null && this.CommonDataPointsCollection.Count > 0)
    {
      chartImpl.CommonDataPointsCollection = new Dictionary<int, ChartDataPointsCollection>();
      foreach (int key in this.CommonDataPointsCollection.Keys)
      {
        ChartDataPointsCollection pointsCollection = (ChartDataPointsCollection) this.CommonDataPointsCollection[key].Clone((object) chartImpl, chartImpl.m_book, dicFontIndexes, hashNewNames);
        chartImpl.CommonDataPointsCollection.Add(key, pointsCollection);
      }
    }
    return chartImpl;
  }

  public void ChangePrimaryAxis(bool isParsing)
  {
    if (isParsing && (this.SecondaryFormats.Count == 0 || this.PrimaryFormats.Count > 1 || !this.PrimaryFormats.NeedSecondaryAxis))
      return;
    this.PrimaryParentAxis.Formats.ChangeCollections();
  }

  public void UpdateChartFbiIndexes(IDictionary dicFontIndexes)
  {
    if (dicFontIndexes == null || this.m_arrFonts == null)
      return;
    int index = 0;
    for (int count = this.m_arrFonts.Count; index < count; ++index)
    {
      ChartFbiRecord arrFont = this.m_arrFonts[index];
      int fontIndex = (int) arrFont.FontIndex;
      int dicFontIndex = (int) dicFontIndexes[(object) fontIndex];
      arrFont.FontIndex = (ushort) dicFontIndex;
    }
  }

  public void UpdateChartFontXIndexes(IDictionary dicFontIndexes)
  {
    if (dicFontIndexes == null || this.m_lstDefaultText == null)
      return;
    int index1 = 0;
    for (int count1 = this.m_lstDefaultText.Count; index1 < count1; ++index1)
    {
      List<BiffRecordRaw> byIndex = this.m_lstDefaultText.GetByIndex(index1);
      if (byIndex != null)
      {
        int index2 = 0;
        for (int count2 = byIndex.Count; index2 < count2; ++index2)
        {
          if (byIndex[index2] is ChartFontxRecord chartFontxRecord)
          {
            int fontIndex = (int) chartFontxRecord.FontIndex;
            if (dicFontIndexes.Contains((object) fontIndex))
            {
              int dicFontIndex = (int) dicFontIndexes[(object) fontIndex];
              chartFontxRecord.FontIndex = (ushort) dicFontIndex;
            }
          }
        }
      }
    }
  }

  public bool CheckForSupportGridLine()
  {
    OfficeChartType chartType = this.ChartType;
    ChartSeriesCollection series = this.m_series;
    if (chartType != OfficeChartType.Combination_Chart)
      return Array.IndexOf<OfficeChartType>(ChartImpl.DEF_NOT_SUPPORT_GRIDLINES, chartType) == -1;
    int index = 0;
    for (int count = series.Count; index < count; ++index)
    {
      IOfficeChartSerie officeChartSerie = series[index];
      if (Array.IndexOf<OfficeChartType>(ChartImpl.DEF_NOT_SUPPORT_GRIDLINES, officeChartSerie.SerieType) != -1)
        return false;
    }
    return true;
  }

  internal void CheckIsBubble3D()
  {
    bool flag = this.ChartType == OfficeChartType.Combination_Chart;
    int count1 = this.PrimaryFormats.Count;
    int count2 = flag ? this.SecondaryFormats.Count : 0;
    for (int index = 0; index < this.Series.Count && (count1 > 0 || flag && count2 > 0); ++index)
    {
      int chartGroup = (this.Series[index] as ChartSerieImpl).ChartGroup;
      if (this.PrimaryFormats.ContainsIndex(chartGroup))
      {
        this.PrimaryFormats[chartGroup].SerieDataFormat.Is3DBubbles = this.Series[index].SerieFormat.Is3DBubbles;
        --count1;
      }
      else if (flag && this.SecondaryFormats.ContainsIndex(chartGroup))
      {
        this.SecondaryFormats[chartGroup].SerieDataFormat.Is3DBubbles = this.Series[index].SerieFormat.Is3DBubbles;
        --count2;
        if (count2 == 0)
          flag = false;
      }
    }
  }

  public void SetToDefaultGridlines(OfficeChartType type)
  {
    IOfficeChartAxis seriesAxis = (IOfficeChartAxis) this.m_primaryParentAxis.SeriesAxis;
    if (seriesAxis != null)
    {
      seriesAxis.HasMinorGridLines = false;
      seriesAxis.HasMajorGridLines = false;
    }
    IOfficeChartAxis categoryAxis = (IOfficeChartAxis) this.m_primaryParentAxis.CategoryAxis;
    if (categoryAxis != null)
    {
      categoryAxis.HasMinorGridLines = false;
      categoryAxis.HasMajorGridLines = false;
    }
    IOfficeChartAxis valueAxis = (IOfficeChartAxis) this.m_primaryParentAxis.ValueAxis;
    if (valueAxis != null)
    {
      valueAxis.HasMinorGridLines = false;
      valueAxis.HasMajorGridLines = false;
    }
    if (Array.IndexOf<OfficeChartType>(ChartImpl.DEF_NOT_SUPPORT_GRIDLINES, type) != -1)
      return;
    valueAxis.HasMajorGridLines = true;
  }

  public override void UpdateFormula(
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect)
  {
    this.m_series.UpdateFormula(iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect);
  }

  public override void MarkUsedReferences(bool[] usedItems)
  {
    if (this.m_series != null)
      this.m_series.MarkUsedReferences(usedItems);
    if (this.m_primaryParentAxis != null)
      this.m_primaryParentAxis.MarkUsedReferences(usedItems);
    if (this.m_secondaryParentAxis != null)
      this.m_secondaryParentAxis.MarkUsedReferences(usedItems);
    if (this.m_title == null)
      return;
    this.m_title.MarkUsedReferences(usedItems);
  }

  public override void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    if (this.m_series != null)
      this.m_series.UpdateReferenceIndexes(arrUpdatedIndexes);
    if (this.m_primaryParentAxis != null)
      this.m_primaryParentAxis.UpdateReferenceIndexes(arrUpdatedIndexes);
    if (this.m_secondaryParentAxis != null)
      this.m_secondaryParentAxis.UpdateReferenceIndexes(arrUpdatedIndexes);
    if (this.m_title == null)
      return;
    this.m_title.UpdateReferenceIndexes(arrUpdatedIndexes);
  }

  public void SetChartData(object[][] data) => this.ChartData.SetChartData(data);

  public void SetDataRange(object[][] data, int rowIndex, int columnIndex)
  {
    this.ChartData.SetDataRange(data, rowIndex, columnIndex);
  }

  public void SetDataRange(IEnumerable enumerable, int rowIndex, int columnIndex)
  {
    this.ChartData.SetDataRange(enumerable, rowIndex, columnIndex);
  }

  public void Close()
  {
    if (this.m_legend != null)
    {
      this.m_legend.Clear();
      this.m_legend = (ChartLegendImpl) null;
    }
    if (this._chartData != null)
    {
      this._chartData.Clear();
      this._chartData = (Syncfusion.OfficeChart.Implementation.Charts.ChartData) null;
    }
    if (this.m_plotArea != null)
    {
      this.m_plotArea.Clear();
      this.m_plotArea = (ChartPlotAreaImpl) null;
    }
    if (this.m_chartArea != null)
    {
      this.m_chartArea.Clear();
      this.m_chartArea = (ChartFrameFormatImpl) null;
    }
    if (this.m_categories != null)
    {
      this.m_categories.Clear();
      this.m_categories = (ChartCategoryCollection) null;
    }
    if (this.m_series != null)
    {
      this.m_series.Clear();
      this.m_series = (ChartSeriesCollection) null;
    }
    this.m_relationPreservedStreamCollections = (Dictionary<string, Stream>) null;
    this.m_themeOverrideStream = (MemoryStream) null;
    this.m_defaultTextProperty = (Stream) null;
    this.m_bandFormats = (Stream) null;
    this.m_pivotFormatsStream = (Stream) null;
    this.m_alternateContent = (Stream) null;
    this.ParentWorkbook.Close();
  }

  internal void RemoveSecondaryAxes()
  {
    if (!this.IsSecondaryAxes)
      return;
    if (this.IsCategoryAxisAvail)
      this.m_secondaryParentAxis.RemoveAxis(true);
    if (!this.IsValueAxisAvail)
      return;
    this.m_secondaryParentAxis.RemoveAxis(false);
  }

  public void SaveAsImage(Stream imageAsstream)
  {
    (this.Application.ChartToImageConverter ?? throw new ArgumentException("IApplication.ChartToImageConverter must be instantiated")).SaveAsImage((IOfficeChart) this, imageAsstream);
  }

  public static int DoubleToFixedPoint(double value)
  {
    ushort num1 = (ushort) value;
    double num2 = (value - (double) num1) * 100000.0;
    if (num2 > (double) ushort.MaxValue)
      num2 /= 10.0;
    byte[] bytes1 = BitConverter.GetBytes(num1);
    byte[] bytes2 = BitConverter.GetBytes((ushort) num2);
    byte[] numArray = new byte[4];
    bytes1.CopyTo((Array) numArray, 2);
    bytes2.CopyTo((Array) numArray, 0);
    return BitConverter.ToInt32(numArray, 0);
  }

  public static double FixedPointToDouble(int value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    int uint16_1 = (int) BitConverter.ToUInt16(bytes, 0);
    int uint16_2 = (int) BitConverter.ToUInt16(bytes, 2);
    int y = uint16_1 != 0 ? (int) Math.Log10((double) uint16_1) + 1 : 0;
    return ((double) Math.Abs(uint16_2) + (double) uint16_1 / Math.Pow(10.0, (double) y)) * (double) Math.Sign(uint16_2);
  }

  internal ChartSeriesAxisImpl CreatePrimarySeriesAxis()
  {
    return this.m_primaryParentAxis.SeriesAxis = new ChartSeriesAxisImpl(this.Application, (object) this.m_primaryParentAxis, OfficeAxisType.Serie);
  }

  internal Color GetChartColor(int index, int totalCount, bool isBinary, bool isColorPalette)
  {
    Color empty = Color.Empty;
    Color chartColor1;
    if (isBinary)
    {
      if (isColorPalette)
      {
        int num = 56;
        index %= num;
        if (index < 31 /*0x1F*/)
        {
          chartColor1 = WorkbookImpl.DEF_PALETTE[index + 32 /*0x20*/];
        }
        else
        {
          index %= 31 /*0x1F*/;
          chartColor1 = WorkbookImpl.DEF_PALETTE[index + 8];
        }
      }
      else
      {
        ChartColor chartColor2;
        if (index <= 30)
        {
          chartColor2 = new ChartColor((OfficeKnownColors) (index + 24));
        }
        else
        {
          index -= 30;
          chartColor2 = new ChartColor((OfficeKnownColors) (index % 55 + 7));
        }
        chartColor1 = chartColor2.GetRGB((IWorkbook) this.ParentWorkbook);
      }
    }
    else
    {
      int num1 = 6;
      double num2 = -0.5;
      Color colorByColorIndex = this.GetChartThemeColorByColorIndex(index % num1 + 4);
      double num3 = 1.0 / (double) (int) Math.Ceiling((double) totalCount / (double) num1);
      double dTint = num3 < 0.5 ? num2 + num3 * (double) (index / num1 + 1) : (index / num1 <= 0 ? 0.0 : 0.25);
      chartColor1 = Excel2007Parser.ConvertColorByTint(colorByColorIndex, dTint);
    }
    return chartColor1;
  }

  internal Color GetChartThemeColorByColorIndex(int color_Index)
  {
    Color empty = Color.Empty;
    Color colorByColorIndex;
    if (this.m_themeOverrideStream == null && this.m_themeColors == null)
      colorByColorIndex = this.ParentWorkbook.IsCreated && this.ParentWorkbook.Version == OfficeVersion.Excel2013 || !this.ParentWorkbook.m_isThemeColorsParsed && this.ParentWorkbook.DefaultThemeVersion == "153222" ? this.ParentWorkbook.GetThemeColor2013(color_Index) : this.ParentWorkbook.GetThemeColor(color_Index);
    else if (this.m_themeColors != null)
    {
      colorByColorIndex = this.m_themeColors[color_Index];
    }
    else
    {
      this.m_themeColors = this.ParentWorkbook.DataHolder.Parser.ParseThemeOverideColors(this);
      colorByColorIndex = this.m_themeColors != null ? this.m_themeColors[color_Index] : this.ParentWorkbook.GetThemeColor(color_Index);
    }
    return colorByColorIndex;
  }

  internal static bool TryAndModifyToValidFormula(string value)
  {
    if (value == null || value == "")
      return false;
    bool flag = value.StartsWith("=");
    if (value.Length > (flag ? 1 : 0))
    {
      value = flag ? value.Substring(1, value.Length - 1) : value;
      if (double.TryParse(value, out double _))
        return false;
      if (value.Contains(" "))
      {
        MatchCollection matchCollection1 = Regex.Matches(value, "( \\d+)", RegexOptions.Compiled);
        MatchCollection matchCollection2 = Regex.Matches(value, "('[^']*')", RegexOptions.Compiled);
        if (matchCollection1.Count == 0)
          return true;
        if (matchCollection2.Count == 0)
          return false;
        for (int i1 = 0; i1 < matchCollection1.Count; ++i1)
        {
          for (int i2 = 0; i2 < matchCollection2.Count && (matchCollection2[i2].Index >= matchCollection1[i1].Index || matchCollection1[i1].Index >= matchCollection2[i2].Index + matchCollection2[i2].Length); ++i2)
          {
            if (i2 == matchCollection2.Count - 1)
              return false;
          }
        }
      }
    }
    return true;
  }
}
