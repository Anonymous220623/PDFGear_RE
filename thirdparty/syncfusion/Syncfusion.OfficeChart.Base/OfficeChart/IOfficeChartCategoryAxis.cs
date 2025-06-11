// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IOfficeChartCategoryAxis
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IOfficeChartCategoryAxis : IOfficeChartValueAxis, IOfficeChartAxis
{
  int TickLabelSpacing { get; set; }

  new bool AutoTickLabelSpacing { get; set; }

  int TickMarkSpacing { get; set; }

  bool IsBetween { get; set; }

  IOfficeDataRange CategoryLabels { get; set; }

  OfficeCategoryType CategoryType { get; set; }

  int Offset { get; set; }

  OfficeChartBaseUnit BaseUnit { get; set; }

  bool BaseUnitIsAuto { get; set; }

  OfficeChartBaseUnit MajorUnitScale { get; set; }

  OfficeChartBaseUnit MinorUnitScale { get; set; }

  bool NoMultiLevelLabel { get; set; }

  bool IsBinningByCategory { get; set; }

  bool HasAutomaticBins { get; set; }

  int NumberOfBins { get; set; }

  double BinWidth { get; set; }

  double UnderflowBinValue { get; set; }

  double OverflowBinValue { get; set; }
}
