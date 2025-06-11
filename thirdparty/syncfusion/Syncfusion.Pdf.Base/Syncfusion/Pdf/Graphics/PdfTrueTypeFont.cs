// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfTrueTypeFont
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics.Fonts;
using Syncfusion.Pdf.Native;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfTrueTypeFont : PdfFont, IDisposable
{
  private const int c_codePage = 1252;
  internal static readonly Encoding Encoding = Encoding.GetEncoding(1252);
  protected static object s_rtlRenderLock = new object();
  private bool m_embed;
  private bool m_unicode = true;
  internal ITrueTypeFont m_fontInternal;
  private bool m_bUseTrueType;
  private bool m_isContainsFont;
  private PdfFontStyle m_style;
  private string metricsName = string.Empty;
  private bool m_isEmbedFont;
  private TtfReader m_ttfReader;
  private bool m_isXPSFontStream;
  private bool m_isEMFFontStream;
  private bool m_conformanceEnabled;
  private bool m_isSkipFontEmbed;
  private bool is_filePath;

  public PdfTrueTypeFont(Font font)
    : this(font, false)
  {
  }

  public PdfTrueTypeFont(Font font, bool unicode)
    : this(font, font.SizeInPoints, unicode)
  {
  }

  internal PdfTrueTypeFont(Font font, bool unicode, bool useTrueType, bool isEnableEmbedding)
    : this(font, font.SizeInPoints, unicode, useTrueType, isEnableEmbedding)
  {
  }

  public PdfTrueTypeFont(Font font, float size)
    : this(font, size, false)
  {
  }

  public PdfTrueTypeFont(Font font, float size, bool unicode)
    : base(size, (PdfFontStyle) font.Style)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    this.m_unicode = unicode;
    if (PdfDocument.ConformanceLevel != PdfConformanceLevel.None && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_X1A2001)
      this.m_unicode = true;
    this.CreateFontInternal(font);
  }

  internal PdfTrueTypeFont(
    Font font,
    float size,
    bool unicode,
    bool useTrueType,
    bool isEnableEmbedding)
    : base(size, (PdfFontStyle) font.Style)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    this.m_unicode = unicode;
    this.m_embed = isEnableEmbedding;
    this.m_bUseTrueType = useTrueType;
    this.m_isEmbedFont = isEnableEmbedding;
    this.CreateFontInternal(font);
  }

  internal PdfTrueTypeFont(
    Font font,
    bool unicode,
    bool useTrueType,
    bool isEnableEmbedding,
    float size,
    bool conformanceEnabled)
    : base(size, (PdfFontStyle) font.Style)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    this.m_unicode = unicode;
    this.m_conformanceEnabled = conformanceEnabled;
    this.m_bUseTrueType = useTrueType;
    this.m_isEmbedFont = isEnableEmbedding;
    this.CreateFontInternal(font);
  }

  internal PdfTrueTypeFont(
    Font font,
    float size,
    bool unicode,
    bool useTrueType,
    bool isEnableEmbedding,
    bool isRemoveCache)
    : base(size, (PdfFontStyle) font.Style)
  {
    this.m_isEMFFontStream = true;
    this.m_unicode = unicode;
    this.m_bUseTrueType = useTrueType;
    this.m_isEmbedFont = isEnableEmbedding;
    this.CreateFontInternal(font);
  }

  internal PdfTrueTypeFont(
    string fontFile,
    float size,
    PdfFontStyle style,
    bool useTrueType,
    bool isEnableEmbedding,
    bool isRemoveCache)
    : base(size)
  {
    switch (fontFile)
    {
      case null:
        throw new ArgumentNullException(nameof (fontFile));
      case "":
        throw new ArgumentException("fontFile - string can not be empty");
      default:
        this.m_bUseTrueType = useTrueType;
        this.m_isEmbedFont = isEnableEmbedding;
        this.m_unicode = true;
        this.m_isEMFFontStream = true;
        this.CreateFontInternal(fontFile, style);
        break;
    }
  }

  public PdfTrueTypeFont(Font font, FontStyle style, float size, bool unicode, bool embed)
    : base(size, (PdfFontStyle) style)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    font = new Font(font.Name, font.Size, style);
    this.m_unicode = unicode;
    this.m_bUseTrueType = true;
    this.m_embed = embed;
    if (unicode && !embed)
      throw new Exception("Unicode font need to be embedded");
    this.CreateFontInternal(font);
  }

  public PdfTrueTypeFont(string fontFile, float size)
    : this(fontFile, size, PdfFontStyle.Regular)
  {
  }

  internal PdfTrueTypeFont(string fontFile, float size, bool isTrueType)
    : this(fontFile, size, PdfFontStyle.Regular, isTrueType)
  {
  }

  public PdfTrueTypeFont(string fontFile, float size, PdfFontStyle style)
    : base(size)
  {
    switch (fontFile)
    {
      case null:
        throw new ArgumentNullException(nameof (fontFile));
      case "":
        throw new ArgumentException("fontFile - string can not be empty");
      default:
        this.m_unicode = true;
        this.CreateFontInternal(fontFile, style);
        break;
    }
  }

  internal PdfTrueTypeFont(string fontFile, float size, PdfFontStyle style, bool useTrueType)
    : base(size)
  {
    switch (fontFile)
    {
      case null:
        throw new ArgumentNullException(nameof (fontFile));
      case "":
        throw new ArgumentException("fontFile - string can not be empty");
      default:
        this.m_bUseTrueType = useTrueType;
        this.m_unicode = true;
        this.CreateFontInternal(fontFile, style);
        break;
    }
  }

  internal PdfTrueTypeFont(
    string fontFile,
    float size,
    PdfFontStyle style,
    bool useTrueType,
    bool isEnableEmbedding)
    : base(size)
  {
    switch (fontFile)
    {
      case null:
        throw new ArgumentNullException(nameof (fontFile));
      case "":
        throw new ArgumentException("fontFile - string can not be empty");
      default:
        this.m_bUseTrueType = useTrueType;
        this.m_isEmbedFont = isEnableEmbedding;
        this.m_unicode = true;
        this.CreateFontInternal(fontFile, style);
        break;
    }
  }

  internal PdfTrueTypeFont(Stream fontStream, float size, bool isEnableEmbedding)
    : base(size)
  {
    this.m_unicode = true;
    this.m_isXPSFontStream = true;
    this.m_isEmbedFont = isEnableEmbedding;
    this.CreateFontInternal(fontStream, PdfFontStyle.Regular);
  }

  public PdfTrueTypeFont(Stream fontStream, float size)
    : this(fontStream, size, true)
  {
  }

  public PdfTrueTypeFont(Stream fontStream, float size, PdfFontStyle style)
    : base(size)
  {
    if (fontStream == null)
      throw new ArgumentNullException(nameof (fontStream));
    if (!fontStream.CanSeek || !fontStream.CanRead)
      throw new PdfException("Unable to parse the given font stream");
    fontStream.Seek(0L, SeekOrigin.Begin);
    this.CreateFontInternal(fontStream, style);
  }

  public PdfTrueTypeFont(Stream fontStream, bool embed, PdfFontStyle style, float size)
    : base(size)
  {
    if (fontStream == null)
      throw new ArgumentNullException(nameof (fontStream));
    this.m_unicode = embed;
    if (!embed)
      this.m_bUseTrueType = !embed;
    this.m_isSkipFontEmbed = !embed;
    this.m_isEmbedFont = embed;
    this.CreateFontInternal(fontStream, style);
  }

  internal PdfTrueTypeFont(
    Stream fontStream,
    float size,
    string metricsName,
    bool isEnableEmbedding)
    : base(size)
  {
    this.m_unicode = true;
    this.metricsName = metricsName;
    this.m_isXPSFontStream = true;
    this.m_isEmbedFont = isEnableEmbedding;
    this.CreateFontInternal(fontStream, PdfFontStyle.Regular);
  }

  internal PdfTrueTypeFont(
    Stream fontStream,
    float size,
    string metricsName,
    bool isEnableEmbedding,
    PdfFontStyle fontStyle)
    : base(size)
  {
    this.m_unicode = true;
    this.metricsName = metricsName;
    this.m_isXPSFontStream = true;
    this.m_isEmbedFont = isEnableEmbedding;
    this.CreateFontInternal(fontStream, fontStyle);
  }

  internal PdfTrueTypeFont(
    Stream fontStream,
    PdfFontStyle fontStyle,
    float size,
    string metricsName,
    bool useTrueType,
    bool isEnableEmbedding)
    : base(size)
  {
    this.m_unicode = true;
    this.m_bUseTrueType = useTrueType;
    this.metricsName = metricsName;
    this.m_isXPSFontStream = true;
    this.m_isEmbedFont = isEnableEmbedding;
    this.CreateFontInternal(fontStream, fontStyle);
  }

  internal PdfTrueTypeFont(
    Stream fontStream,
    PdfFontStyle fontStyle,
    float size,
    string metricsName,
    bool useTrueType,
    bool isEnableEmbedding,
    bool isConformanceEnabled)
    : base(size)
  {
    this.m_unicode = true;
    this.m_bUseTrueType = useTrueType;
    this.metricsName = metricsName;
    this.m_isXPSFontStream = true;
    this.m_isEmbedFont = isEnableEmbedding;
    this.m_conformanceEnabled = isConformanceEnabled;
    this.CreateFontInternal(fontStream, fontStyle);
  }

  internal PdfTrueTypeFont(
    Stream fontStream,
    float size,
    bool isUnicode,
    string metricsName,
    PdfFontStyle fontStyle)
    : base(size)
  {
    this.m_unicode = isUnicode;
    this.metricsName = metricsName;
    this.m_isXPSFontStream = true;
    if (!isUnicode)
      this.m_bUseTrueType = !isUnicode;
    this.m_isSkipFontEmbed = !isUnicode;
    this.CreateFontInternal(fontStream, fontStyle);
  }

  internal PdfTrueTypeFont(
    Stream fontStream,
    float size,
    bool isEnableEmbedding,
    PdfFontStyle fontStyle)
    : base(size)
  {
    this.m_unicode = true;
    this.m_isEmbedFont = isEnableEmbedding;
    this.CreateFontInternal(fontStream, fontStyle);
  }

  public PdfTrueTypeFont(PdfTrueTypeFont prototype, float size)
    : base(size, prototype.Style)
  {
    this.is_filePath = prototype != null ? prototype.is_filePath : throw new ArgumentNullException(nameof (prototype));
    this.m_unicode = prototype.Unicode;
    this.CreateFontInternal(prototype);
  }

  internal PdfTrueTypeFont(PdfTrueTypeFont prototype, bool isEnableEmbedding, float size)
    : base(size, prototype.Style)
  {
    this.m_unicode = prototype != null ? prototype.Unicode : throw new ArgumentNullException(nameof (prototype));
    this.m_isEmbedFont = isEnableEmbedding;
    this.CreateFontInternal(prototype);
  }

  internal PdfTrueTypeFont(PdfTrueTypeFont prototype, float size, bool isXpsFontstream)
    : base(size, prototype.Style)
  {
    this.m_isXPSFontStream = isXpsFontstream;
    if (prototype == null)
      throw new ArgumentNullException(nameof (prototype));
    this.m_unicode = prototype != null ? prototype.Unicode : throw new ArgumentNullException(nameof (prototype));
    IPdfCache pdfCache = (IPdfCache) prototype;
    IPdfPrimitive internals = (IPdfPrimitive) null;
    if (pdfCache != null)
    {
      internals = pdfCache.GetInternals();
      PdfFontMetrics pdfFontMetrics = (PdfFontMetrics) ((PdfFont) pdfCache).Metrics.Clone();
      pdfFontMetrics.Size = this.Size;
      this.Metrics = pdfFontMetrics;
      this.m_fontInternal = ((PdfTrueTypeFont) pdfCache).InternalFont;
      this.m_ttfReader = ((PdfTrueTypeFont) pdfCache).m_ttfReader;
    }
    ((IPdfCache) this).SetInternals(internals);
  }

  internal PdfTrueTypeFont(
    PdfTrueTypeFont prototype,
    Font font,
    bool isReuse,
    bool isEnableEmbedding)
    : base(font.Size, (PdfFontStyle) font.Style)
  {
    if (prototype == null)
      throw new ArgumentNullException(nameof (prototype));
    this.m_isEmbedFont = isEnableEmbedding;
    this.m_unicode = prototype.Unicode;
    IPdfCache pdfCache = (IPdfCache) prototype;
    IPdfPrimitive internals = (IPdfPrimitive) null;
    if (pdfCache != null)
    {
      internals = pdfCache.GetInternals();
      PdfFontMetrics pdfFontMetrics = (PdfFontMetrics) ((PdfFont) pdfCache).Metrics.Clone();
      pdfFontMetrics.Size = this.Size;
      this.Metrics = pdfFontMetrics;
      this.m_fontInternal = ((PdfTrueTypeFont) pdfCache).InternalFont;
      this.m_ttfReader = ((PdfTrueTypeFont) pdfCache).m_ttfReader;
    }
    ((IPdfCache) this).SetInternals(internals);
  }

  ~PdfTrueTypeFont()
  {
    if (this.m_isXPSFontStream)
      return;
    this.Dispose();
  }

  public bool Unicode => this.m_unicode;

  internal bool Embed => this.m_embed;

  internal ITrueTypeFont InternalFont => this.m_fontInternal;

  internal bool IsContainsFont => this.m_isContainsFont;

  internal Font Font => this.InternalFont.Font;

  internal string FontFile
  {
    get
    {
      string fontFile = (string) null;
      if (this.InternalFont is UnicodeTrueTypeFont internalFont)
        fontFile = internalFont.FontFile;
      return fontFile;
    }
  }

  internal TtfReader TtfReader => this.m_ttfReader;

  public void Dispose()
  {
    if (this.m_fontInternal == null)
      return;
    lock (PdfFont.s_syncObject)
    {
      if (!PdfDocument.EnableCache)
        return;
      PdfDocument.Cache.Remove((IPdfCache) this);
      if (PdfDocument.Cache.GroupCount((IPdfCache) this) == 0)
        this.m_fontInternal.Close();
      this.m_fontInternal = (ITrueTypeFont) null;
    }
  }

  protected internal override float GetCharWidth(char charCode, PdfStringFormat format)
  {
    return (float) this.InternalFont.GetCharWidth(charCode) * (1f / 1000f * this.Metrics.GetSize(format));
  }

  protected internal override float GetLineWidth(string line, PdfStringFormat format)
  {
    float width1 = 0.0f;
    if (format != null && format.TextDirection != PdfTextDirection.None && this.Unicode)
    {
      if (!this.GetUnicodeLineWidth(line, out width1, format))
        width1 = (float) this.InternalFont.GetLineWidth(line);
    }
    else if (format == null || !format.RightToLeft || !this.Unicode)
      width1 = this.GetWidth(this.InternalFont as UnicodeTrueTypeFont, line);
    else if (!this.GetUnicodeLineWidth(line, out width1, format))
      width1 = (float) this.InternalFont.GetLineWidth(line);
    float size = this.Metrics.GetSize(format);
    float width2 = width1 * (1f / 1000f * size);
    return this.ApplyFormatSettings(line, format, width2);
  }

  internal float GetLineWidth(
    string line,
    PdfStringFormat format,
    out OtfGlyphInfoList glyphList,
    ScriptTags[] tags)
  {
    float width1 = 0.0f;
    glyphList = (OtfGlyphInfoList) null;
    if (this.TtfReader.isOTFFont())
    {
      bool flag1 = false;
      foreach (ScriptTags tag in tags)
      {
        if (tag == ScriptTags.Arabic)
        {
          flag1 = true;
          if (format == null)
            format = new PdfStringFormat();
          if (format.TextDirection == PdfTextDirection.None)
          {
            format.TextDirection = PdfTextDirection.RightToLeft;
            break;
          }
          break;
        }
      }
      ScriptLayouter scriptLayouter = new ScriptLayouter();
      List<OtfGlyphInfo> glyphs = new List<OtfGlyphInfo>();
      string str = line;
      if (format != null && (format.RightToLeft || format.TextDirection != PdfTextDirection.None) && !flag1)
        str = new ArabicShapeRenderer().Shape(line.ToCharArray(), 0);
      foreach (char charCode in str)
      {
        TtfGlyphInfo glyph = this.TtfReader.GetGlyph(charCode);
        OtfGlyphInfo otfGlyphInfo = new OtfGlyphInfo(glyph.CharCode, glyph.Index, (float) glyph.Width);
        if (charCode != ' ' && glyph.CharCode == 32 /*0x20*/)
          otfGlyphInfo.unsupportedGlyph = true;
        glyphs.Add(otfGlyphInfo);
      }
      glyphList = new OtfGlyphInfoList(glyphs);
      if (tags.Length > 0)
      {
        bool flag2 = false;
        foreach (ScriptTags tag in tags)
        {
          if (this.TtfReader.supportedScriptTags.Contains(tag))
          {
            bool flag3 = scriptLayouter.DoLayout(this.InternalFont as UnicodeTrueTypeFont, glyphList, tag);
            if (flag3)
              flag2 = flag3;
          }
        }
        if (flag2)
        {
          foreach (OtfGlyphInfo glyph in glyphList.Glyphs)
            width1 += glyph.Width;
          this.m_isContainsFont = true;
        }
        else
        {
          glyphList = (OtfGlyphInfoList) null;
          this.m_isContainsFont = false;
        }
      }
    }
    if ((double) width1 == 0.0)
    {
      if (format != null && format.TextDirection != PdfTextDirection.None)
      {
        if (!this.GetUnicodeLineWidth(line, out width1, format))
          width1 = (float) this.InternalFont.GetLineWidth(line);
      }
      else if (format == null || !format.RightToLeft || !this.Unicode)
        width1 = this.GetWidth(this.InternalFont as UnicodeTrueTypeFont, line);
      else if (!this.GetUnicodeLineWidth(line, out width1, format))
        width1 = (float) this.InternalFont.GetLineWidth(line);
    }
    float size = this.Metrics.GetSize(format);
    float width2 = width1 * (1f / 1000f * size);
    return this.ApplyFormatSettings(line, format, width2);
  }

  protected override bool EqualsToFont(PdfFont font) => this.m_fontInternal.EqualsToFont(font);

  internal void SetSymbols(string text)
  {
    if (this.m_fontInternal is UnicodeTrueTypeFont)
    {
      if (!(this.m_fontInternal is UnicodeTrueTypeFont fontInternal))
        return;
      fontInternal.SetSymbols(text);
    }
    else
    {
      if (!(this.m_fontInternal is TrueTypeFont fontInternal))
        return;
      fontInternal.SetSymbols(text);
    }
  }

  internal void SetSymbols(ushort[] glyphs)
  {
    if (!(this.m_fontInternal is UnicodeTrueTypeFont fontInternal))
      return;
    fontInternal.SetSymbols(glyphs);
  }

  private bool IsAzureEnvironment()
  {
    IntPtr dc = GdiApi.GetDC(IntPtr.Zero);
    uint fontData = GdiApi.GetFontData(dc, 0U, 0U, (byte[]) null, 0U);
    GdiApi.DeleteDC(dc);
    return fontData == 3221225506U /*0xC0000022*/;
  }

  private void CreateFontInternal(Font mfont)
  {
    Font font = (Font) mfont.Clone();
    if (!PdfDocument.EnableCache)
    {
      string key;
      if (this.m_isEmbedFont)
        key = font.Name + (object) font.Style + (object) this.Unicode + (object) this.m_isEmbedFont;
      else
        key = font.Name + (object) font.Style + (object) this.Unicode;
      if (PdfDocument.Cache.FontCollection.ContainsKey(key))
        this.m_fontInternal = (PdfDocument.Cache.FontCollection[key] as PdfTrueTypeFont).m_fontInternal;
    }
    if (this.m_fontInternal == null)
    {
      bool isAzureCompatible = this.IsAzureEnvironment();
      this.m_fontInternal = this.Unicode || !this.m_bUseTrueType || !this.Embed ? (!this.Unicode || !this.m_bUseTrueType || !this.Embed ? (!this.Unicode || !this.m_bUseTrueType ? (!this.Unicode || this.m_bUseTrueType ? (!isAzureCompatible ? (ITrueTypeFont) new TrueTypeFont(font, this.Size) : (ITrueTypeFont) new TrueTypeFont(font, this.Size, false, isAzureCompatible)) : (ITrueTypeFont) new UnicodeTrueTypeFont(font, this.Size, CompositeFontType.Type0, isAzureCompatible)) : (ITrueTypeFont) new UnicodeTrueTypeFont(font, this.Size, CompositeFontType.TrueType, isAzureCompatible)) : (ITrueTypeFont) new UnicodeTrueTypeFont(font, this.Size, CompositeFontType.Type0, isAzureCompatible)) : (ITrueTypeFont) new TrueTypeFont(font, this.Size, true, isAzureCompatible);
    }
    this.InitializeInternals();
  }

  private void CreateFontInternal(string fontFile, PdfFontStyle style)
  {
    switch (fontFile)
    {
      case null:
        throw new ArgumentNullException(nameof (fontFile));
      case "":
        throw new ArgumentException("fontFile - string can not be empty");
      default:
        this.m_fontInternal = this.m_bUseTrueType ? (ITrueTypeFont) new UnicodeTrueTypeFont(fontFile, this.Size, CompositeFontType.TrueType) : (ITrueTypeFont) new UnicodeTrueTypeFont(fontFile, this.Size, CompositeFontType.Type0);
        this.m_style = style;
        this.CalculateStyle(style);
        this.InitializeInternals();
        break;
    }
  }

  private void CreateFontInternal(PdfTrueTypeFont prototype)
  {
    if (prototype == null)
      throw new ArgumentNullException(nameof (prototype));
    if (PdfDocument.ConformanceLevel != PdfConformanceLevel.None && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_X1A2001)
      this.m_unicode = true;
    this.m_fontInternal = !this.Unicode ? (ITrueTypeFont) new TrueTypeFont(prototype.Font, this.Size) : (ITrueTypeFont) new UnicodeTrueTypeFont(prototype.InternalFont as UnicodeTrueTypeFont);
    this.InitializeInternals();
  }

  private void CreateFontInternal(Stream fontStream, PdfFontStyle style)
  {
    if (fontStream == null)
      throw new ArgumentNullException("fontFile");
    if (!fontStream.CanSeek || !fontStream.CanRead)
      throw new PdfException("Unable to parse the given font stream");
    this.m_fontInternal = this.m_bUseTrueType ? (!(this.metricsName != string.Empty) ? (ITrueTypeFont) new UnicodeTrueTypeFont(fontStream, this.Size, CompositeFontType.TrueType) : (ITrueTypeFont) new UnicodeTrueTypeFont(fontStream, this.Size, CompositeFontType.TrueType, this.metricsName)) : (!(this.metricsName != string.Empty) ? (ITrueTypeFont) new UnicodeTrueTypeFont(fontStream, this.Size, CompositeFontType.Type0) : (ITrueTypeFont) new UnicodeTrueTypeFont(fontStream, this.Size, CompositeFontType.Type0, this.metricsName));
    if (this.m_fontInternal != null)
      (this.m_fontInternal as UnicodeTrueTypeFont).SkipFontEmbed = this.m_isSkipFontEmbed;
    this.CalculateStyle(style);
    this.InitializeInternals();
  }

  private void InitializeInternals()
  {
    IPdfCache pdfCache = (IPdfCache) null;
    string key = " ";
    if (this.m_conformanceEnabled)
    {
      if (this.Unicode)
      {
        if (this.InternalFont.Font != null)
          key = this.InternalFont.Font.Name + (object) this.InternalFont.Font.Style + (object) this.Unicode + (object) this.m_isEmbedFont;
        else
          key = ((UnicodeTrueTypeFont) this.InternalFont).m_ttfMetrics.FontFamily + (object) this.m_style + (object) this.Unicode + (object) this.m_isEmbedFont;
      }
      else if (this.Font != null)
        key = this.InternalFont.Font.Name + (object) this.InternalFont.Font.Style + (object) this.Unicode + (object) this.m_isEmbedFont;
    }
    else if (this.Unicode)
    {
      if (this.m_isEmbedFont)
      {
        if (this.InternalFont.Font != null)
          key = this.InternalFont.Font.Name + (object) this.InternalFont.Font.Style + (object) this.Unicode + (object) this.m_isEmbedFont;
        else
          key = ((UnicodeTrueTypeFont) this.InternalFont).m_ttfMetrics.FontFamily + (object) this.m_style + (object) this.Unicode + (object) this.m_isEmbedFont;
      }
      else
        key = this.InternalFont.Font == null ? ((UnicodeTrueTypeFont) this.InternalFont).m_ttfMetrics.FontFamily + (object) this.m_style + (object) this.Unicode : this.InternalFont.Font.Name + (object) this.InternalFont.Font.Style + (object) this.Unicode;
    }
    else if (this.Font != null)
    {
      if (this.m_isEmbedFont)
        key = this.InternalFont.Font.Name + (object) this.InternalFont.Font.Style + (object) this.Unicode + (object) this.m_isEmbedFont;
      else
        key = this.InternalFont.Font.Name + (object) this.InternalFont.Font.Style + (object) this.Unicode;
    }
    if (PdfDocument.EnableCache)
      pdfCache = PdfDocument.Cache.Search((IPdfCache) this);
    else if (!this.m_isXPSFontStream || !this.m_isEMFFontStream)
    {
      lock (PdfDocument.Cache)
      {
        if (PdfDocument.Cache.FontCollection.ContainsKey(key))
          pdfCache = (IPdfCache) PdfDocument.Cache.FontCollection[key];
      }
    }
    IPdfPrimitive internals = (IPdfPrimitive) null;
    if (pdfCache != null)
    {
      internals = pdfCache.GetInternals();
      PdfFontMetrics pdfFontMetrics = (PdfFontMetrics) ((PdfFont) pdfCache).Metrics.Clone();
      pdfFontMetrics.Size = this.Size;
      this.Metrics = pdfFontMetrics;
      this.m_fontInternal = ((PdfTrueTypeFont) pdfCache).InternalFont;
    }
    else
    {
      if (pdfCache == null || this.m_bUseTrueType)
      {
        if (PdfDocument.EnableCache && this.m_bUseTrueType)
          PdfDocument.Cache.Remove(pdfCache);
        if (this.m_fontInternal is UnicodeTrueTypeFont)
        {
          (this.m_fontInternal as UnicodeTrueTypeFont).IsEmbed = this.m_isEmbedFont;
          (this.m_fontInternal as UnicodeTrueTypeFont).conformanceEnabled = this.m_conformanceEnabled;
          (this.m_fontInternal as UnicodeTrueTypeFont).is_filePath = this.is_filePath;
        }
        this.m_fontInternal.CreateInternals();
        internals = this.m_fontInternal.GetInternals();
        this.Metrics = this.m_fontInternal.Metrics;
      }
      if (!PdfDocument.EnableCache && !this.m_isXPSFontStream && !this.m_isEMFFontStream)
      {
        lock (PdfDocument.Cache)
        {
          if (!PdfDocument.Cache.FontCollection.ContainsKey(key))
            PdfDocument.Cache.FontCollection.Add(key, (PdfFont) this);
        }
      }
    }
    this.Metrics.isUnicodeFont = this.Unicode;
    ((IPdfCache) this).SetInternals(internals);
    if (!(this.m_fontInternal is UnicodeTrueTypeFont))
      return;
    this.m_ttfReader = (this.m_fontInternal as UnicodeTrueTypeFont).TtfReader;
  }

  private void CalculateStyle(PdfFontStyle style)
  {
    int macStyle = ((UnicodeTrueTypeFont) this.m_fontInternal).TtfMetrics.MacStyle;
    if ((style & PdfFontStyle.Underline) != PdfFontStyle.Regular)
      macStyle |= 4;
    if ((style & PdfFontStyle.Strikeout) != PdfFontStyle.Regular)
    {
      int num = macStyle | 8;
    }
    this.m_style = style;
    this.SetStyle(style);
  }

  private float GetSymbolSize(char ch, PdfStringFormat format)
  {
    float width = 0.0f;
    if (format == null || !format.RightToLeft || !this.Unicode)
    {
      width = this.GetCharWidth(ch, format);
    }
    else
    {
      float size = this.Metrics.GetSize(format);
      this.GetUnicodeLineWidth(new string(ch, 1), out width, format);
      width *= 1f / 1000f * size;
    }
    return width;
  }

  private bool GetUnicodeLineWidth(string line, out float width, PdfStringFormat format)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    width = 0.0f;
    ushort[] glyphs = (ushort[]) null;
    bool unicodeLineWidth = false;
    lock (PdfTrueTypeFont.s_rtlRenderLock)
    {
      unicodeLineWidth = format == null || format.TextDirection == PdfTextDirection.None ? RtlRenderer.GetGlyphIndices(line, this, false, out glyphs) : (this.Font == null || format.isCustomRendering ? RtlRenderer.GetGlyphIndices(line, this, format.TextDirection == PdfTextDirection.RightToLeft, out glyphs, true) : RtlRenderer.GetGlyphIndices(line, this, format.TextDirection == PdfTextDirection.RightToLeft, out glyphs));
      if (unicodeLineWidth)
      {
        if (glyphs != null)
        {
          TtfReader ttfReader = (this.InternalFont as UnicodeTrueTypeFont).TtfReader;
          if (this.Font != null && format != null && !format.isCustomRendering)
            ttfReader.m_missedGlyphCount = 0;
          int index = 0;
          for (int length = glyphs.Length; index < length; ++index)
          {
            int glyphIndex = (int) glyphs[index];
            TtfGlyphInfo glyph = ttfReader.GetGlyph(glyphIndex);
            if (!glyph.Empty)
              width += (float) glyph.Width;
            if (this.Font != null && format != null && glyphIndex != glyph.Index && format.TextDirection != PdfTextDirection.None && !format.isCustomRendering)
              format.isCustomRendering = true;
          }
          this.m_isContainsFont = !new List<ushort>((IEnumerable<ushort>) glyphs).Contains((ushort) 0) && ttfReader.m_missedGlyphCount == 0;
        }
      }
    }
    return unicodeLineWidth;
  }

  private float GetWidth(UnicodeTrueTypeFont unicodeFont, string line)
  {
    bool flag = false;
    if (unicodeFont != null && unicodeFont.TtfReader != null)
    {
      flag = true;
      unicodeFont.TtfReader.m_missedGlyphCount = 0;
    }
    float lineWidth = (float) this.InternalFont.GetLineWidth(line);
    if (flag && unicodeFont.TtfReader.m_missedGlyphCount == 0)
      this.m_isContainsFont = true;
    else if (unicodeFont != null)
      this.m_isContainsFont = false;
    return lineWidth;
  }
}
