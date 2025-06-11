// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.PdfCjkStandardFontMetricsFactory
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal sealed class PdfCjkStandardFontMetricsFactory
{
  private const float c_subSuperScriptFactor = 1.52f;

  private PdfCjkStandardFontMetricsFactory()
  {
  }

  public static PdfFontMetrics GetMetrics(
    PdfCjkFontFamily fontFamily,
    PdfFontStyle fontStyle,
    float size)
  {
    PdfFontMetrics metrics;
    switch (fontFamily)
    {
      case PdfCjkFontFamily.HeiseiKakuGothicW5:
        metrics = PdfCjkStandardFontMetricsFactory.GetHeiseiKakuGothicW5Metrix(fontFamily, fontStyle, size);
        break;
      case PdfCjkFontFamily.HeiseiMinchoW3:
        metrics = PdfCjkStandardFontMetricsFactory.GetHeiseiMinchoW3(fontFamily, fontStyle, size);
        break;
      case PdfCjkFontFamily.HanyangSystemsGothicMedium:
        metrics = PdfCjkStandardFontMetricsFactory.GetHanyangSystemsGothicMediumMetrix(fontFamily, fontStyle, size);
        break;
      case PdfCjkFontFamily.HanyangSystemsShinMyeongJoMedium:
        metrics = PdfCjkStandardFontMetricsFactory.GetHanyangSystemsShinMyeongJoMediumMetrix(fontFamily, fontStyle, size);
        break;
      case PdfCjkFontFamily.MonotypeHeiMedium:
        metrics = PdfCjkStandardFontMetricsFactory.GetMonotypeHeiMedium(fontFamily, fontStyle, size);
        break;
      case PdfCjkFontFamily.MonotypeSungLight:
        metrics = PdfCjkStandardFontMetricsFactory.GetMonotypeSungLightMetrix(fontFamily, fontStyle, size);
        break;
      case PdfCjkFontFamily.SinoTypeSongLight:
        metrics = PdfCjkStandardFontMetricsFactory.GetSinoTypeSongLight(fontFamily, fontStyle, size);
        break;
      default:
        throw new ArgumentException("Unsupported font family", nameof (fontFamily));
    }
    metrics.Name = fontFamily.ToString();
    metrics.SubScriptSizeFactor = 1.52f;
    metrics.SuperscriptSizeFactor = 1.52f;
    return metrics;
  }

  private static PdfFontMetrics GetHanyangSystemsGothicMediumMetrix(
    PdfCjkFontFamily fontFamily,
    PdfFontStyle fontStyle,
    float size)
  {
    PdfFontMetrics gothicMediumMetrix = new PdfFontMetrics();
    CjkWidthTable cjkWidthTable = new CjkWidthTable(1000);
    gothicMediumMetrix.WidthTable = (WidthTable) cjkWidthTable;
    cjkWidthTable.Add((CjkWidth) new CjkSameWidth(1, (int) sbyte.MaxValue, 500));
    cjkWidthTable.Add((CjkWidth) new CjkSameWidth(8094, 8190, 500));
    gothicMediumMetrix.Ascent = 880f;
    gothicMediumMetrix.Descent = -120f;
    gothicMediumMetrix.Size = size;
    gothicMediumMetrix.Height = gothicMediumMetrix.Ascent - gothicMediumMetrix.Descent;
    gothicMediumMetrix.PostScriptName = (fontStyle & PdfFontStyle.Bold) == PdfFontStyle.Regular || (fontStyle & PdfFontStyle.Italic) == PdfFontStyle.Regular ? ((fontStyle & PdfFontStyle.Bold) == PdfFontStyle.Regular ? ((fontStyle & PdfFontStyle.Italic) == PdfFontStyle.Regular ? "HYGoThic-Medium" : "HYGoThic-Medium,Italic") : "HYGoThic-Medium,Bold") : "HYGoThic-Medium,BoldItalic";
    return gothicMediumMetrix;
  }

  private static PdfFontMetrics GetMonotypeHeiMedium(
    PdfCjkFontFamily fontFamily,
    PdfFontStyle fontStyle,
    float size)
  {
    PdfFontMetrics monotypeHeiMedium = new PdfFontMetrics();
    CjkWidthTable cjkWidthTable = new CjkWidthTable(1000);
    monotypeHeiMedium.WidthTable = (WidthTable) cjkWidthTable;
    cjkWidthTable.Add((CjkWidth) new CjkSameWidth(1, 95, 500));
    cjkWidthTable.Add((CjkWidth) new CjkSameWidth(13648, 13742, 500));
    monotypeHeiMedium.Ascent = 880f;
    monotypeHeiMedium.Descent = -120f;
    monotypeHeiMedium.Size = size;
    monotypeHeiMedium.Height = monotypeHeiMedium.Ascent - monotypeHeiMedium.Descent;
    monotypeHeiMedium.PostScriptName = (fontStyle & PdfFontStyle.Bold) == PdfFontStyle.Regular || (fontStyle & PdfFontStyle.Italic) == PdfFontStyle.Regular ? ((fontStyle & PdfFontStyle.Bold) == PdfFontStyle.Regular ? ((fontStyle & PdfFontStyle.Italic) == PdfFontStyle.Regular ? "MHei-Medium" : "MHei-Medium,Italic") : "MHei-Medium,Bold") : "MHei-Medium,BoldItalic";
    return monotypeHeiMedium;
  }

  private static PdfFontMetrics GetMonotypeSungLightMetrix(
    PdfCjkFontFamily fontFamily,
    PdfFontStyle fontStyle,
    float size)
  {
    PdfFontMetrics monotypeSungLightMetrix = new PdfFontMetrics();
    CjkWidthTable cjkWidthTable = new CjkWidthTable(1000);
    monotypeSungLightMetrix.WidthTable = (WidthTable) cjkWidthTable;
    cjkWidthTable.Add((CjkWidth) new CjkSameWidth(1, 95, 500));
    cjkWidthTable.Add((CjkWidth) new CjkSameWidth(13648, 13742, 500));
    monotypeSungLightMetrix.Ascent = 880f;
    monotypeSungLightMetrix.Descent = -120f;
    monotypeSungLightMetrix.Size = size;
    monotypeSungLightMetrix.Height = monotypeSungLightMetrix.Ascent - monotypeSungLightMetrix.Descent;
    monotypeSungLightMetrix.PostScriptName = (fontStyle & PdfFontStyle.Bold) == PdfFontStyle.Regular || (fontStyle & PdfFontStyle.Italic) == PdfFontStyle.Regular ? ((fontStyle & PdfFontStyle.Bold) == PdfFontStyle.Regular ? ((fontStyle & PdfFontStyle.Italic) == PdfFontStyle.Regular ? "MSung-Light" : "MSung-Light,Italic") : "MSung-Light,Bold") : "MSung-Light,BoldItalic";
    return monotypeSungLightMetrix;
  }

  private static PdfFontMetrics GetSinoTypeSongLight(
    PdfCjkFontFamily fontFamily,
    PdfFontStyle fontStyle,
    float size)
  {
    PdfFontMetrics sinoTypeSongLight = new PdfFontMetrics();
    CjkWidthTable cjkWidthTable = new CjkWidthTable(1000);
    sinoTypeSongLight.WidthTable = (WidthTable) cjkWidthTable;
    cjkWidthTable.Add((CjkWidth) new CjkSameWidth(1, 95, 500));
    cjkWidthTable.Add((CjkWidth) new CjkSameWidth(814, 939, 500));
    cjkWidthTable.Add((CjkWidth) new CjkDifferentWidth(7712, new int[1]
    {
      500
    }));
    cjkWidthTable.Add((CjkWidth) new CjkDifferentWidth(7716, new int[1]
    {
      500
    }));
    sinoTypeSongLight.Ascent = 880f;
    sinoTypeSongLight.Descent = -120f;
    sinoTypeSongLight.Size = size;
    sinoTypeSongLight.Height = sinoTypeSongLight.Ascent - sinoTypeSongLight.Descent;
    sinoTypeSongLight.PostScriptName = (fontStyle & PdfFontStyle.Bold) == PdfFontStyle.Regular || (fontStyle & PdfFontStyle.Italic) == PdfFontStyle.Regular ? ((fontStyle & PdfFontStyle.Bold) == PdfFontStyle.Regular ? ((fontStyle & PdfFontStyle.Italic) == PdfFontStyle.Regular ? "STSong-Light" : "STSong-Light,Italic") : "STSong-Light,Bold") : "STSong-Light,BoldItalic";
    return sinoTypeSongLight;
  }

  private static PdfFontMetrics GetHeiseiMinchoW3(
    PdfCjkFontFamily fontFamily,
    PdfFontStyle fontStyle,
    float size)
  {
    PdfFontMetrics heiseiMinchoW3 = new PdfFontMetrics();
    CjkWidthTable cjkWidthTable = new CjkWidthTable(1000);
    heiseiMinchoW3.WidthTable = (WidthTable) cjkWidthTable;
    cjkWidthTable.Add((CjkWidth) new CjkSameWidth(1, 95, 500));
    cjkWidthTable.Add((CjkWidth) new CjkSameWidth(231, 632, 500));
    heiseiMinchoW3.Ascent = 857f;
    heiseiMinchoW3.Descent = -143f;
    heiseiMinchoW3.Size = size;
    heiseiMinchoW3.Height = heiseiMinchoW3.Ascent - heiseiMinchoW3.Descent;
    heiseiMinchoW3.PostScriptName = (fontStyle & PdfFontStyle.Bold) == PdfFontStyle.Regular || (fontStyle & PdfFontStyle.Italic) == PdfFontStyle.Regular ? ((fontStyle & PdfFontStyle.Bold) == PdfFontStyle.Regular ? ((fontStyle & PdfFontStyle.Italic) == PdfFontStyle.Regular ? "HeiseiMin-W3" : "HeiseiMin-W3,Italic") : "HeiseiMin-W3,Bold") : "HeiseiMin-W3,BoldItalic";
    return heiseiMinchoW3;
  }

  private static PdfFontMetrics GetHeiseiKakuGothicW5Metrix(
    PdfCjkFontFamily fontFamily,
    PdfFontStyle fontStyle,
    float size)
  {
    PdfFontMetrics kakuGothicW5Metrix = new PdfFontMetrics();
    CjkWidthTable cjkWidthTable = new CjkWidthTable(1000);
    kakuGothicW5Metrix.WidthTable = (WidthTable) cjkWidthTable;
    cjkWidthTable.Add((CjkWidth) new CjkSameWidth(1, 95, 500));
    cjkWidthTable.Add((CjkWidth) new CjkSameWidth(231, 632, 500));
    kakuGothicW5Metrix.Ascent = 857f;
    kakuGothicW5Metrix.Descent = -125f;
    kakuGothicW5Metrix.Size = size;
    kakuGothicW5Metrix.Height = kakuGothicW5Metrix.Ascent - kakuGothicW5Metrix.Descent;
    kakuGothicW5Metrix.PostScriptName = (fontStyle & PdfFontStyle.Bold) == PdfFontStyle.Regular || (fontStyle & PdfFontStyle.Italic) == PdfFontStyle.Regular ? ((fontStyle & PdfFontStyle.Bold) == PdfFontStyle.Regular ? ((fontStyle & PdfFontStyle.Italic) == PdfFontStyle.Regular ? "HeiseiKakuGo-W5" : "HeiseiKakuGo-W5,Italic") : "HeiseiKakuGo-W5,Bold") : "HeiseiKakuGo-W5,BoldItalic";
    return kakuGothicW5Metrix;
  }

  private static PdfFontMetrics GetHanyangSystemsShinMyeongJoMediumMetrix(
    PdfCjkFontFamily fontFamily,
    PdfFontStyle fontStyle,
    float size)
  {
    PdfFontMetrics myeongJoMediumMetrix = new PdfFontMetrics();
    CjkWidthTable cjkWidthTable = new CjkWidthTable(1000);
    myeongJoMediumMetrix.WidthTable = (WidthTable) cjkWidthTable;
    cjkWidthTable.Add((CjkWidth) new CjkSameWidth(1, 95, 500));
    cjkWidthTable.Add((CjkWidth) new CjkSameWidth(8094, 8190, 500));
    myeongJoMediumMetrix.Ascent = 880f;
    myeongJoMediumMetrix.Descent = -120f;
    myeongJoMediumMetrix.Size = size;
    myeongJoMediumMetrix.Height = myeongJoMediumMetrix.Ascent - myeongJoMediumMetrix.Descent;
    myeongJoMediumMetrix.PostScriptName = (fontStyle & PdfFontStyle.Bold) == PdfFontStyle.Regular || (fontStyle & PdfFontStyle.Italic) == PdfFontStyle.Regular ? ((fontStyle & PdfFontStyle.Bold) == PdfFontStyle.Regular ? ((fontStyle & PdfFontStyle.Italic) == PdfFontStyle.Regular ? "HYSMyeongJo-Medium" : "HYSMyeongJo-Medium,Italic") : "HYSMyeongJo-Medium,Bold") : "HYSMyeongJo-Medium,BoldItalic";
    return myeongJoMediumMetrix;
  }
}
