// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IPageSetupBase
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IPageSetupBase : IParentApplication
{
  bool AutoFirstPageNumber { get; set; }

  bool BlackAndWhite { get; set; }

  double BottomMargin { get; set; }

  string CenterFooter { get; set; }

  Image CenterFooterImage { get; set; }

  Image CenterHeaderImage { get; set; }

  string CenterHeader { get; set; }

  bool CenterHorizontally { get; set; }

  bool CenterVertically { get; set; }

  int Copies { get; set; }

  bool Draft { get; set; }

  short FirstPageNumber { get; set; }

  double FooterMargin { get; set; }

  double HeaderMargin { get; set; }

  string LeftFooter { get; set; }

  Image LeftFooterImage { get; set; }

  Image LeftHeaderImage { get; set; }

  string LeftHeader { get; set; }

  double LeftMargin { get; set; }

  OfficeOrder Order { get; set; }

  OfficePageOrientation Orientation { get; set; }

  OfficePaperSize PaperSize { get; set; }

  OfficePrintLocation PrintComments { get; set; }

  OfficePrintErrors PrintErrors { get; set; }

  bool PrintNotes { get; set; }

  int PrintQuality { get; set; }

  string RightFooter { get; set; }

  Image RightFooterImage { get; set; }

  Image RightHeaderImage { get; set; }

  string RightHeader { get; set; }

  double RightMargin { get; set; }

  double TopMargin { get; set; }

  int Zoom { get; set; }

  bool AlignHFWithPageMargins { get; set; }

  bool DifferentFirstPageHF { get; set; }

  bool DifferentOddAndEvenPagesHF { get; set; }

  bool HFScaleWithDoc { get; set; }

  Bitmap BackgoundImage { get; set; }
}
