// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartFormat
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartFormat
{
  [Obsolete("Please, use IsVaryColor instead of this property. Sorry for inconvenience.")]
  bool IsVeryColor { get; set; }

  bool IsVaryColor { get; set; }

  int Overlap { get; set; }

  int GapWidth { get; set; }

  int FirstSliceAngle { get; set; }

  int DoughnutHoleSize { get; set; }

  int BubbleScale { get; set; }

  ExcelBubbleSize SizeRepresents { get; set; }

  bool ShowNegativeBubbles { get; set; }

  bool HasRadarAxisLabels { get; set; }

  ExcelSplitType SplitType { get; set; }

  int SplitValue { get; set; }

  int PieSecondSize { get; set; }

  IChartDropBar FirstDropBar { get; }

  IChartDropBar SecondDropBar { get; }

  IChartBorder PieSeriesLine { get; }

  ExcelDropLineStyle DropLineStyle { get; set; }

  IChartBorder HighLowLines { get; }

  IChartBorder DropLines { get; }

  bool HasDropLines { get; set; }

  bool HasHighLowLines { get; set; }

  bool HasSeriesLines { get; set; }
}
