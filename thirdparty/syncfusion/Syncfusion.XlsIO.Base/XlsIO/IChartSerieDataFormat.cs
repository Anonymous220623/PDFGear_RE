// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartSerieDataFormat
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartSerieDataFormat : IChartFillBorder
{
  IChartInterior AreaProperties { get; }

  ExcelBaseFormat BarShapeBase { get; set; }

  ExcelTopFormat BarShapeTop { get; set; }

  Color MarkerBackgroundColor { get; set; }

  Color MarkerForegroundColor { get; set; }

  ExcelChartMarkerType MarkerStyle { get; set; }

  ExcelKnownColors MarkerForegroundColorIndex { get; set; }

  ExcelKnownColors MarkerBackgroundColorIndex { get; set; }

  int MarkerSize { get; set; }

  bool IsAutoMarker { get; set; }

  int Percent { get; set; }

  bool Is3DBubbles { get; set; }

  IChartFormat CommonSerieOptions { get; }

  bool IsMarkerSupported { get; }

  ExcelTreeMapLabelOption TreeMapLabelOption { get; set; }

  bool ShowConnectorLines { get; set; }

  bool ShowMeanLine { get; set; }

  bool ShowMeanMarkers { get; set; }

  bool ShowInnerPoints { get; set; }

  bool ShowOutlierPoints { get; set; }

  ExcelQuartileCalculation QuartileCalculationType { get; set; }
}
