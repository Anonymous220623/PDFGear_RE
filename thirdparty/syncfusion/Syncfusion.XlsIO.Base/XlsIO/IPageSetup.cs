// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IPageSetup
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IPageSetup : IPageSetupBase, IParentApplication
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
