// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.PdfCidFont
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class PdfCidFont : PdfDictionary
{
  public PdfCidFont(
    PdfCjkFontFamily fontFamily,
    PdfFontStyle fontStyle,
    PdfFontMetrics fontMetrics)
  {
    this["Type"] = (IPdfPrimitive) new PdfName("Font");
    this["Subtype"] = (IPdfPrimitive) new PdfName("CIDFontType2");
    this["BaseFont"] = (IPdfPrimitive) new PdfName(fontMetrics.PostScriptName);
    this["DW"] = (IPdfPrimitive) new PdfNumber((fontMetrics.WidthTable as CjkWidthTable).DefaultWidth);
    this["W"] = (IPdfPrimitive) fontMetrics.WidthTable.ToArray();
    this["FontDescriptor"] = (IPdfPrimitive) PdfCjkFontDescryptorFactory.GetFontDescryptor(fontFamily, fontStyle, fontMetrics);
    this["CIDSystemInfo"] = (IPdfPrimitive) this.GetSystemInfo(fontFamily);
  }

  private PdfDictionary GetSystemInfo(PdfCjkFontFamily fontFamily)
  {
    PdfDictionary systemInfo = new PdfDictionary();
    systemInfo["Registry"] = (IPdfPrimitive) new PdfString("Adobe");
    switch (fontFamily)
    {
      case PdfCjkFontFamily.HeiseiKakuGothicW5:
      case PdfCjkFontFamily.HeiseiMinchoW3:
        systemInfo["Ordering"] = (IPdfPrimitive) new PdfString("Japan1");
        systemInfo["Supplement"] = (IPdfPrimitive) new PdfNumber(2);
        break;
      case PdfCjkFontFamily.HanyangSystemsGothicMedium:
      case PdfCjkFontFamily.HanyangSystemsShinMyeongJoMedium:
        systemInfo["Ordering"] = (IPdfPrimitive) new PdfString("Korea1");
        systemInfo["Supplement"] = (IPdfPrimitive) new PdfNumber(1);
        break;
      case PdfCjkFontFamily.MonotypeHeiMedium:
      case PdfCjkFontFamily.MonotypeSungLight:
        systemInfo["Ordering"] = (IPdfPrimitive) new PdfString("CNS1");
        systemInfo["Supplement"] = (IPdfPrimitive) new PdfNumber(0);
        break;
      case PdfCjkFontFamily.SinoTypeSongLight:
        systemInfo["Ordering"] = (IPdfPrimitive) new PdfString("GB1");
        systemInfo["Supplement"] = (IPdfPrimitive) new PdfNumber(2);
        break;
      default:
        throw new ArgumentException("Unsupported font family: " + fontFamily.ToString(), nameof (fontFamily));
    }
    return systemInfo;
  }
}
