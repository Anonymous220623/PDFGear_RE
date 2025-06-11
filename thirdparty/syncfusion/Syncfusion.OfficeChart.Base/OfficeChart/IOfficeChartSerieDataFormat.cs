// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IOfficeChartSerieDataFormat
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IOfficeChartSerieDataFormat : IOfficeChartFillBorder
{
  IOfficeChartInterior AreaProperties { get; }

  OfficeBaseFormat BarShapeBase { get; set; }

  OfficeTopFormat BarShapeTop { get; set; }

  Color MarkerBackgroundColor { get; set; }

  Color MarkerForegroundColor { get; set; }

  OfficeChartMarkerType MarkerStyle { get; set; }

  OfficeKnownColors MarkerForegroundColorIndex { get; set; }

  OfficeKnownColors MarkerBackgroundColorIndex { get; set; }

  int MarkerSize { get; set; }

  bool IsAutoMarker { get; set; }

  int Percent { get; set; }

  bool Is3DBubbles { get; set; }

  IOfficeChartFormat CommonSerieOptions { get; }

  bool IsMarkerSupported { get; }

  TreeMapLabelOption TreeMapLabelOption { get; set; }

  bool ShowConnectorLines { get; set; }

  bool ShowMeanLine { get; set; }

  bool ShowMeanMarkers { get; set; }

  bool ShowInnerPoints { get; set; }

  bool ShowOutlierPoints { get; set; }

  QuartileCalculation QuartileCalculationType { get; set; }
}
