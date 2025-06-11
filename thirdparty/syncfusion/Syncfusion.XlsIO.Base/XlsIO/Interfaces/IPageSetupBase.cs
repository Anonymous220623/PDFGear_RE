// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Interfaces.IPageSetupBase
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Interfaces;

public interface IPageSetupBase : IParentApplication
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

  ExcelOrder Order { get; set; }

  ExcelPageOrientation Orientation { get; set; }

  ExcelPaperSize PaperSize { get; set; }

  ExcelPrintLocation PrintComments { get; set; }

  ExcelPrintErrors PrintErrors { get; set; }

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

  IPage EvenPage { get; }

  IPage FirstPage { get; }

  Bitmap BackgoundImage { get; set; }
}
