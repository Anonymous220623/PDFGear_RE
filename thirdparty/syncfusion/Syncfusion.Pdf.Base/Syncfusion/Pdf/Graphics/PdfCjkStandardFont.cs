// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfCjkStandardFont
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics.Fonts;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfCjkStandardFont : PdfFont
{
  private const int c_charOffset = 32 /*0x20*/;
  private PdfCjkFontFamily m_fontFamily;

  public PdfCjkStandardFont(PdfCjkFontFamily fontFamily, float size, PdfFontStyle style)
    : base(size, style)
  {
    this.m_fontFamily = fontFamily;
    this.CheckStyle();
    this.InitializeInternals();
  }

  public PdfCjkStandardFont(PdfCjkFontFamily fontFamily, float size)
    : this(fontFamily, size, PdfFontStyle.Regular)
  {
  }

  public PdfCjkStandardFont(PdfCjkStandardFont prototype, float size)
    : this(prototype.FontFamily, size, prototype.Style)
  {
  }

  public PdfCjkStandardFont(PdfCjkStandardFont prototype, float size, PdfFontStyle style)
    : this(prototype.FontFamily, size, style)
  {
  }

  public PdfCjkFontFamily FontFamily => this.m_fontFamily;

  protected override bool EqualsToFont(PdfFont font)
  {
    bool font1 = false;
    if (font is PdfCjkStandardFont pdfCjkStandardFont)
    {
      bool flag1 = this.FontFamily == pdfCjkStandardFont.FontFamily;
      bool flag2 = (this.Style & ~(PdfFontStyle.Underline | PdfFontStyle.Strikeout)) == (pdfCjkStandardFont.Style & ~(PdfFontStyle.Underline | PdfFontStyle.Strikeout));
      font1 = flag1 && flag2;
    }
    return font1;
  }

  protected internal override float GetCharWidth(char charCode, PdfStringFormat format)
  {
    return this.GetCharWidthInternal(charCode, format) * (1f / 1000f * this.Metrics.GetSize(format));
  }

  protected internal override float GetLineWidth(string line, PdfStringFormat format)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    float num = 0.0f;
    int index = 0;
    for (int length = line.Length; index < length; ++index)
    {
      float charWidthInternal = this.GetCharWidthInternal(line[index], format);
      num += charWidthInternal;
    }
    float size = this.Metrics.GetSize(format);
    float width = num * (1f / 1000f * size);
    return this.ApplyFormatSettings(line, format, width);
  }

  private void InitializeInternals()
  {
    lock (PdfFont.s_syncObject)
    {
      IPdfCache pdfCache = (IPdfCache) null;
      if (PdfDocument.EnableCache)
        pdfCache = PdfDocument.Cache.Search((IPdfCache) this);
      IPdfPrimitive internals;
      if (pdfCache == null)
      {
        this.Metrics = PdfCjkStandardFontMetricsFactory.GetMetrics(this.m_fontFamily, this.Style, this.Size);
        internals = (IPdfPrimitive) this.CreateInternals();
      }
      else
      {
        internals = pdfCache.GetInternals();
        PdfFontMetrics pdfFontMetrics = (PdfFontMetrics) ((PdfFont) pdfCache).Metrics.Clone();
        pdfFontMetrics.Size = this.Size;
        this.Metrics = pdfFontMetrics;
      }
      ((IPdfCache) this).SetInternals(internals);
    }
  }

  private PdfDictionary CreateInternals()
  {
    return new PdfDictionary()
    {
      ["Type"] = (IPdfPrimitive) new PdfName("Font"),
      ["Subtype"] = (IPdfPrimitive) new PdfName("Type0"),
      ["BaseFont"] = (IPdfPrimitive) new PdfName(this.Metrics.PostScriptName),
      ["Encoding"] = (IPdfPrimitive) PdfCjkStandardFont.GetEncoding(this.m_fontFamily),
      ["DescendantFonts"] = (IPdfPrimitive) this.GetDescendantFont()
    };
  }

  private PdfArray GetDescendantFont()
  {
    return new PdfArray()
    {
      (IPdfPrimitive) new PdfCidFont(this.m_fontFamily, this.Style, this.Metrics)
    };
  }

  private static PdfName GetEncoding(PdfCjkFontFamily fontFamily)
  {
    string str;
    switch (fontFamily)
    {
      case PdfCjkFontFamily.HeiseiKakuGothicW5:
      case PdfCjkFontFamily.HeiseiMinchoW3:
        str = "UniJIS-UCS2-H";
        break;
      case PdfCjkFontFamily.HanyangSystemsGothicMedium:
      case PdfCjkFontFamily.HanyangSystemsShinMyeongJoMedium:
        str = "UniKS-UCS2-H";
        break;
      case PdfCjkFontFamily.MonotypeHeiMedium:
      case PdfCjkFontFamily.MonotypeSungLight:
        str = "UniCNS-UCS2-H";
        break;
      case PdfCjkFontFamily.SinoTypeSongLight:
        str = "UniGB-UCS2-H";
        break;
      default:
        throw new ArgumentException("Unsupported font face.", nameof (fontFamily));
    }
    return new PdfName(str);
  }

  private void CheckStyle() => this.SetStyle(this.Style);

  private float GetCharWidthInternal(char charCode, PdfStringFormat format)
  {
    int num = (int) charCode;
    return (float) this.Metrics.WidthTable[num >= 0 ? num : 0];
  }
}
