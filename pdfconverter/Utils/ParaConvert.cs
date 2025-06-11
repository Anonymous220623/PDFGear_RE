// Decompiled with JetBrains decompiler
// Type: pdfconverter.Utils.ParaConvert
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using pdfconverter.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace pdfconverter.Utils;

public static class ParaConvert
{
  public static SizeF GetPdfPagesize(PageSizeItem size)
  {
    switch (size.PDFPageSize)
    {
      case PDFPageSize.MatchSource:
        return new SizeF(0.0f, 0.0f);
      case PDFPageSize.A4_Portrait:
        return PdfPageSize.A4;
      case PDFPageSize.A4_Landscape:
        SizeF a4 = PdfPageSize.A4;
        double height1 = (double) a4.Height;
        a4 = PdfPageSize.A4;
        double width1 = (double) a4.Width;
        return new SizeF((float) height1, (float) width1);
      case PDFPageSize.A3_Portrait:
        return PdfPageSize.A3;
      case PDFPageSize.A3_Landscape:
        SizeF a3 = PdfPageSize.A3;
        double height2 = (double) a3.Height;
        a3 = PdfPageSize.A3;
        double width2 = (double) a3.Width;
        return new SizeF((float) height2, (float) width2);
      default:
        return PdfPageSize.A4;
    }
  }

  public static PdfPageOrientation GetPdfeOrientation(PageSizeItem size)
  {
    switch (size.PDFPageSize)
    {
      case PDFPageSize.A4_Portrait:
      case PDFPageSize.A3_Portrait:
        return PdfPageOrientation.Portrait;
      case PDFPageSize.A4_Landscape:
      case PDFPageSize.A3_Landscape:
        return PdfPageOrientation.Landscape;
      default:
        return PdfPageOrientation.Portrait;
    }
  }

  public static PdfMargins GetMargins(PageMarginItem size)
  {
    PdfMargins margins = new PdfMargins();
    switch (size.PDFPageSize)
    {
      case ContentMargin.NoMargin:
        margins.All = 0.0f;
        return margins;
      case ContentMargin.BigMargin:
        margins.Left = ParaConvert.GetPixValue(3.18);
        margins.Right = ParaConvert.GetPixValue(3.18);
        margins.Top = ParaConvert.GetPixValue(2.54);
        margins.Bottom = ParaConvert.GetPixValue(2.54);
        return margins;
      case ContentMargin.SmallMargin:
        margins.All = ParaConvert.GetPixValue(1.27);
        return margins;
      default:
        return margins;
    }
  }

  private static float GetPixValue(double CM) => (float) (CM * 72.0 / 2.54);
}
