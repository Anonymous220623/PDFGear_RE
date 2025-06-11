// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IOfficeChartAxis
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IOfficeChartAxis
{
  string NumberFormat { get; set; }

  OfficeAxisType AxisType { get; }

  string Title { get; set; }

  int TextRotationAngle { get; set; }

  IOfficeChartTextArea TitleArea { get; }

  IOfficeFont Font { get; }

  IOfficeChartGridLine MajorGridLines { get; }

  IOfficeChartGridLine MinorGridLines { get; }

  bool HasMinorGridLines { get; set; }

  bool HasMajorGridLines { get; set; }

  OfficeTickMark MinorTickMark { get; set; }

  OfficeTickMark MajorTickMark { get; set; }

  IOfficeChartBorder Border { get; }

  bool AutoTickLabelSpacing { get; set; }

  OfficeTickLabelPosition TickLabelPosition { get; set; }

  bool Visible { get; set; }

  bool ReversePlotOrder { get; set; }

  IShadow Shadow { get; }

  IThreeDFormat Chart3DOptions { get; }
}
