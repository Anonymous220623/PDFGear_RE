// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartDataLabels
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartDataLabels : IChartTextArea, IFont, IParentApplication, IOptimizedUpdate
{
  bool IsSeriesName { get; set; }

  bool IsCategoryName { get; set; }

  bool IsValue { get; set; }

  bool IsPercentage { get; set; }

  bool IsBubbleSize { get; set; }

  string Delimiter { get; set; }

  bool IsLegendKey { get; set; }

  bool ShowLeaderLines { get; set; }

  ExcelDataLabelPosition Position { get; set; }

  new bool IsFormula { get; set; }

  bool IsValueFromCells { get; set; }

  IRange ValueFromCellsRange { get; set; }
}
