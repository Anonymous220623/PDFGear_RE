// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartCategoryAxis
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartCategoryAxis : IChartValueAxis, IChartAxis
{
  [Obsolete("Please, use TickLabelSpacing instead of it")]
  int LabelFrequency { get; set; }

  int TickLabelSpacing { get; set; }

  new bool AutoTickLabelSpacing { get; set; }

  [Obsolete("Please, use TickMarkSpacing instead of it")]
  int TickMarksFrequency { get; set; }

  int TickMarkSpacing { get; set; }

  bool IsBetween { get; set; }

  IRange CategoryLabels { get; set; }

  object[] EnteredDirectlyCategoryLabels { get; set; }

  ExcelCategoryType CategoryType { get; set; }

  int Offset { get; set; }

  ExcelChartBaseUnit BaseUnit { get; set; }

  bool BaseUnitIsAuto { get; set; }

  ExcelChartBaseUnit MajorUnitScale { get; set; }

  ExcelChartBaseUnit MinorUnitScale { get; set; }

  bool NoMultiLevelLabel { get; set; }

  bool IsBinningByCategory { get; set; }

  bool HasAutomaticBins { get; set; }

  int NumberOfBins { get; set; }

  double BinWidth { get; set; }

  double UnderflowBinValue { get; set; }

  double OverflowBinValue { get; set; }
}
