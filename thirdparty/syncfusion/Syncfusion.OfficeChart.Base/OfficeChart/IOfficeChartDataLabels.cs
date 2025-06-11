// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IOfficeChartDataLabels
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IOfficeChartDataLabels : 
  IOfficeChartTextArea,
  IOfficeFont,
  IParentApplication,
  IOptimizedUpdate
{
  bool IsSeriesName { get; set; }

  bool IsCategoryName { get; set; }

  bool IsValue { get; set; }

  bool IsPercentage { get; set; }

  string NumberFormat { get; set; }

  bool IsBubbleSize { get; set; }

  string Delimiter { get; set; }

  bool IsLegendKey { get; set; }

  bool ShowLeaderLines { get; set; }

  OfficeDataLabelPosition Position { get; set; }

  bool IsValueFromCells { get; set; }

  IOfficeDataRange ValueFromCellsRange { get; set; }
}
