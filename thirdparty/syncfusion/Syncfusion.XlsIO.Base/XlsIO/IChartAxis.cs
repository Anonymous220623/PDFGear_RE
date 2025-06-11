// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartAxis
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartAxis
{
  string NumberFormat { get; set; }

  ExcelAxisType AxisType { get; }

  string Title { get; set; }

  int TextRotationAngle { get; set; }

  IChartTextArea TitleArea { get; }

  IFont Font { get; }

  IChartGridLine MajorGridLines { get; }

  IChartGridLine MinorGridLines { get; }

  bool HasMinorGridLines { get; set; }

  bool HasMajorGridLines { get; set; }

  ExcelTickMark MinorTickMark { get; set; }

  ExcelTickMark MajorTickMark { get; set; }

  IChartBorder Border { get; }

  bool AutoTickLabelSpacing { get; set; }

  ExcelTickLabelPosition TickLabelPosition { get; set; }

  bool Visible { get; set; }

  ExcelAxisTextDirection Alignment { get; set; }

  [Obsolete("Please use ReversePlotOrder property instead of this one.")]
  bool IsReversed { get; set; }

  bool ReversePlotOrder { get; set; }

  IShadow Shadow { get; }

  IThreeDFormat Chart3DOptions { get; }
}
