// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.PdfCjkFontDescryptorFactory
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal sealed class PdfCjkFontDescryptorFactory
{
  internal static PdfDictionary GetFontDescryptor(
    PdfCjkFontFamily fontFamily,
    PdfFontStyle fontStyle,
    PdfFontMetrics fontMetrics)
  {
    PdfDictionary fontDescryptor = new PdfDictionary();
    switch (fontFamily)
    {
      case PdfCjkFontFamily.HeiseiKakuGothicW5:
        PdfCjkFontDescryptorFactory.FillHeiseiKakuGothicW5(fontDescryptor, fontStyle, fontFamily, fontMetrics);
        break;
      case PdfCjkFontFamily.HeiseiMinchoW3:
        PdfCjkFontDescryptorFactory.FillHeiseiMinchoW3(fontDescryptor, fontFamily, fontMetrics);
        break;
      case PdfCjkFontFamily.HanyangSystemsGothicMedium:
        PdfCjkFontDescryptorFactory.FillHanyangSystemsGothicMedium(fontDescryptor, fontFamily, fontMetrics);
        break;
      case PdfCjkFontFamily.HanyangSystemsShinMyeongJoMedium:
        PdfCjkFontDescryptorFactory.FillHanyangSystemsShinMyeongJoMedium(fontDescryptor, fontFamily, fontMetrics);
        break;
      case PdfCjkFontFamily.MonotypeHeiMedium:
        PdfCjkFontDescryptorFactory.FillMonotypeHeiMedium(fontDescryptor, fontFamily, fontMetrics);
        break;
      case PdfCjkFontFamily.MonotypeSungLight:
        PdfCjkFontDescryptorFactory.FillMonotypeSungLight(fontDescryptor, fontFamily, fontMetrics);
        break;
      case PdfCjkFontFamily.SinoTypeSongLight:
        PdfCjkFontDescryptorFactory.FillSinoTypeSongLight(fontDescryptor, fontFamily, fontMetrics);
        break;
    }
    return fontDescryptor;
  }

  private static void FillMonotypeSungLight(
    PdfDictionary fontDescryptor,
    PdfCjkFontFamily fontFamily,
    PdfFontMetrics fontMetrics)
  {
    Rectangle fontBBox = new Rectangle(-160, -249, 1175, 1137);
    PdfCjkFontDescryptorFactory.FillFontBBox(fontDescryptor, fontBBox);
    PdfCjkFontDescryptorFactory.FillKnownInfo(fontDescryptor, fontFamily, fontMetrics);
    PdfNumber pdfNumber1 = new PdfNumber(93);
    fontDescryptor["StemV"] = (IPdfPrimitive) pdfNumber1;
    fontDescryptor["StemH"] = (IPdfPrimitive) pdfNumber1;
    PdfNumber pdfNumber2 = new PdfNumber(1000);
    fontDescryptor["AvgWidth"] = (IPdfPrimitive) pdfNumber2;
    fontDescryptor["MaxWidth"] = (IPdfPrimitive) pdfNumber2;
    fontDescryptor["CapHeight"] = (IPdfPrimitive) new PdfNumber(880);
    fontDescryptor["XHeight"] = (IPdfPrimitive) new PdfNumber(616);
    fontDescryptor["Leading"] = (IPdfPrimitive) new PdfNumber(250);
  }

  private static void FillHeiseiKakuGothicW5(
    PdfDictionary fontDescryptor,
    PdfFontStyle fontStyle,
    PdfCjkFontFamily fontFamily,
    PdfFontMetrics fontMetrics)
  {
    Rectangle fontBBox1 = new Rectangle(-92, -250, 1102, 1172);
    Rectangle fontBBox2 = new Rectangle(-92, -250, 1102, 1932);
    if ((fontStyle & (PdfFontStyle.Bold | PdfFontStyle.Italic)) != PdfFontStyle.Italic)
      PdfCjkFontDescryptorFactory.FillFontBBox(fontDescryptor, fontBBox1);
    else
      PdfCjkFontDescryptorFactory.FillFontBBox(fontDescryptor, fontBBox2);
    PdfCjkFontDescryptorFactory.FillKnownInfo(fontDescryptor, fontFamily, fontMetrics);
    PdfNumber pdfNumber1 = new PdfNumber(93);
    fontDescryptor["StemV"] = (IPdfPrimitive) pdfNumber1;
    fontDescryptor["StemH"] = (IPdfPrimitive) pdfNumber1;
    PdfNumber pdfNumber2 = new PdfNumber(1000);
    fontDescryptor["AvgWidth"] = (IPdfPrimitive) new PdfNumber(689);
    fontDescryptor["MaxWidth"] = (IPdfPrimitive) pdfNumber2;
    fontDescryptor["CapHeight"] = (IPdfPrimitive) new PdfNumber(718);
    fontDescryptor["XHeight"] = (IPdfPrimitive) new PdfNumber(500);
    fontDescryptor["Leading"] = (IPdfPrimitive) new PdfNumber(250);
  }

  private static void FillHanyangSystemsShinMyeongJoMedium(
    PdfDictionary fontDescryptor,
    PdfCjkFontFamily fontFamily,
    PdfFontMetrics fontMetrics)
  {
    Rectangle fontBBox = new Rectangle(0, -148, 1001, 1028);
    PdfCjkFontDescryptorFactory.FillFontBBox(fontDescryptor, fontBBox);
    PdfCjkFontDescryptorFactory.FillKnownInfo(fontDescryptor, fontFamily, fontMetrics);
    PdfNumber pdfNumber1 = new PdfNumber(93);
    fontDescryptor["StemV"] = (IPdfPrimitive) pdfNumber1;
    fontDescryptor["StemH"] = (IPdfPrimitive) pdfNumber1;
    PdfNumber pdfNumber2 = new PdfNumber(1000);
    fontDescryptor["AvgWidth"] = (IPdfPrimitive) pdfNumber2;
    fontDescryptor["MaxWidth"] = (IPdfPrimitive) pdfNumber2;
    fontDescryptor["CapHeight"] = (IPdfPrimitive) new PdfNumber(880);
    fontDescryptor["XHeight"] = (IPdfPrimitive) new PdfNumber(616);
    fontDescryptor["Leading"] = (IPdfPrimitive) new PdfNumber(250);
  }

  private static void FillHeiseiMinchoW3(
    PdfDictionary fontDescryptor,
    PdfCjkFontFamily fontFamily,
    PdfFontMetrics fontMetrics)
  {
    Rectangle fontBBox = new Rectangle(-123, -257, 1124, 1167);
    PdfCjkFontDescryptorFactory.FillFontBBox(fontDescryptor, fontBBox);
    PdfCjkFontDescryptorFactory.FillKnownInfo(fontDescryptor, fontFamily, fontMetrics);
    PdfNumber pdfNumber1 = new PdfNumber(93);
    fontDescryptor["StemV"] = (IPdfPrimitive) pdfNumber1;
    fontDescryptor["StemH"] = (IPdfPrimitive) pdfNumber1;
    PdfNumber pdfNumber2 = new PdfNumber(1000);
    fontDescryptor["AvgWidth"] = (IPdfPrimitive) new PdfNumber(702);
    fontDescryptor["MaxWidth"] = (IPdfPrimitive) pdfNumber2;
    fontDescryptor["CapHeight"] = (IPdfPrimitive) new PdfNumber(718);
    fontDescryptor["XHeight"] = (IPdfPrimitive) new PdfNumber(500);
    fontDescryptor["Leading"] = (IPdfPrimitive) new PdfNumber(250);
  }

  private static void FillSinoTypeSongLight(
    PdfDictionary fontDescryptor,
    PdfCjkFontFamily fontFamily,
    PdfFontMetrics fontMetrics)
  {
    Rectangle fontBBox = new Rectangle(-25, -254, 1025, 1134);
    PdfCjkFontDescryptorFactory.FillFontBBox(fontDescryptor, fontBBox);
    PdfCjkFontDescryptorFactory.FillKnownInfo(fontDescryptor, fontFamily, fontMetrics);
    PdfNumber pdfNumber1 = new PdfNumber(93);
    fontDescryptor["StemV"] = (IPdfPrimitive) pdfNumber1;
    fontDescryptor["StemH"] = (IPdfPrimitive) pdfNumber1;
    PdfNumber pdfNumber2 = new PdfNumber(1000);
    fontDescryptor["AvgWidth"] = (IPdfPrimitive) pdfNumber2;
    fontDescryptor["MaxWidth"] = (IPdfPrimitive) pdfNumber2;
    fontDescryptor["CapHeight"] = (IPdfPrimitive) new PdfNumber(880);
    fontDescryptor["XHeight"] = (IPdfPrimitive) new PdfNumber(616);
    fontDescryptor["Leading"] = (IPdfPrimitive) new PdfNumber(250);
  }

  private static void FillMonotypeHeiMedium(
    PdfDictionary fontDescryptor,
    PdfCjkFontFamily fontFamily,
    PdfFontMetrics fontMetrics)
  {
    Rectangle fontBBox = new Rectangle(-45, -250, 1060, 1137);
    PdfCjkFontDescryptorFactory.FillFontBBox(fontDescryptor, fontBBox);
    PdfCjkFontDescryptorFactory.FillKnownInfo(fontDescryptor, fontFamily, fontMetrics);
    PdfNumber pdfNumber1 = new PdfNumber(93);
    fontDescryptor["StemV"] = (IPdfPrimitive) pdfNumber1;
    fontDescryptor["StemH"] = (IPdfPrimitive) pdfNumber1;
    PdfNumber pdfNumber2 = new PdfNumber(1000);
    fontDescryptor["AvgWidth"] = (IPdfPrimitive) pdfNumber2;
    fontDescryptor["MaxWidth"] = (IPdfPrimitive) pdfNumber2;
    fontDescryptor["CapHeight"] = (IPdfPrimitive) new PdfNumber(880);
    fontDescryptor["XHeight"] = (IPdfPrimitive) new PdfNumber(616);
    fontDescryptor["Leading"] = (IPdfPrimitive) new PdfNumber(250);
  }

  private static void FillHanyangSystemsGothicMedium(
    PdfDictionary fontDescryptor,
    PdfCjkFontFamily fontFamily,
    PdfFontMetrics fontMetrics)
  {
    Rectangle fontBBox = new Rectangle(-6, -145, 1009, 1025);
    PdfCjkFontDescryptorFactory.FillFontBBox(fontDescryptor, fontBBox);
    PdfCjkFontDescryptorFactory.FillKnownInfo(fontDescryptor, fontFamily, fontMetrics);
    fontDescryptor["Flags"] = (IPdfPrimitive) new PdfNumber(4);
    PdfNumber pdfNumber1 = new PdfNumber(93);
    fontDescryptor["StemV"] = (IPdfPrimitive) pdfNumber1;
    fontDescryptor["StemH"] = (IPdfPrimitive) pdfNumber1;
    PdfNumber pdfNumber2 = new PdfNumber(1000);
    fontDescryptor["AvgWidth"] = (IPdfPrimitive) pdfNumber2;
    fontDescryptor["MaxWidth"] = (IPdfPrimitive) pdfNumber2;
    fontDescryptor["CapHeight"] = (IPdfPrimitive) new PdfNumber(880);
    fontDescryptor["XHeight"] = (IPdfPrimitive) new PdfNumber(616);
    fontDescryptor["Leading"] = (IPdfPrimitive) new PdfNumber(250);
  }

  private static void FillKnownInfo(
    PdfDictionary fontDescryptor,
    PdfCjkFontFamily fontFamily,
    PdfFontMetrics fontMetrics)
  {
    fontDescryptor["FontName"] = (IPdfPrimitive) new PdfName(fontMetrics.PostScriptName);
    fontDescryptor["Type"] = (IPdfPrimitive) new PdfName("FontDescriptor");
    fontDescryptor["ItalicAngle"] = (IPdfPrimitive) new PdfNumber(0);
    fontDescryptor["MissingWidth"] = (IPdfPrimitive) new PdfNumber((fontMetrics.WidthTable as CjkWidthTable).DefaultWidth);
    fontDescryptor["Ascent"] = (IPdfPrimitive) new PdfNumber(fontMetrics.Ascent);
    fontDescryptor["Descent"] = (IPdfPrimitive) new PdfNumber(fontMetrics.Descent);
    PdfCjkFontDescryptorFactory.FillFlags(fontDescryptor, fontFamily);
  }

  private static void FillFlags(PdfDictionary fontDescryptor, PdfCjkFontFamily fontFamily)
  {
    switch (fontFamily)
    {
      case PdfCjkFontFamily.HeiseiKakuGothicW5:
      case PdfCjkFontFamily.HanyangSystemsGothicMedium:
      case PdfCjkFontFamily.MonotypeHeiMedium:
        fontDescryptor["Flags"] = (IPdfPrimitive) new PdfNumber(4);
        break;
      case PdfCjkFontFamily.HeiseiMinchoW3:
      case PdfCjkFontFamily.HanyangSystemsShinMyeongJoMedium:
      case PdfCjkFontFamily.MonotypeSungLight:
      case PdfCjkFontFamily.SinoTypeSongLight:
        fontDescryptor["Flags"] = (IPdfPrimitive) new PdfNumber(6);
        break;
      default:
        throw new ArgumentException("Unsupported font family: " + (object) fontFamily, nameof (fontFamily));
    }
  }

  private static void FillFontBBox(PdfDictionary fontDescryptor, Rectangle fontBBox)
  {
    fontDescryptor["FontBBox"] = (IPdfPrimitive) PdfArray.FromRectangle(fontBBox);
  }
}
