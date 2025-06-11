// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IPageSetup
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IPageSetup : IPageSetupBase, IParentApplication
{
  int FitToPagesTall { get; set; }

  int FitToPagesWide { get; set; }

  bool PrintGridlines { get; set; }

  bool PrintHeadings { get; set; }

  string PrintArea { get; set; }

  string PrintTitleColumns { get; set; }

  string PrintTitleRows { get; set; }

  bool IsSummaryRowBelow { get; set; }

  bool IsSummaryColumnRight { get; set; }

  bool IsFitToPage { get; set; }
}
