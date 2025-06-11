// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontOpenTypeFontSourceBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class SystemFontOpenTypeFontSourceBase : SystemFontFontSource
{
  private readonly SystemFontOpenTypeGlyphScaler scaler;
  private readonly SystemFontOpenTypeFontReader reader;

  internal abstract SystemFontOutlines Outlines { get; }

  internal SystemFontOpenTypeFontReader Reader => this.reader;

  internal abstract SystemFontCFFFontSource CFF { get; }

  internal abstract SystemFontCMap CMap { get; }

  internal abstract SystemFontHead Head { get; }

  internal abstract SystemFontHorizontalHeader HHea { get; }

  internal abstract SystemFontHorizontalMetrics HMtx { get; }

  internal abstract SystemFontKerning Kern { get; }

  internal abstract SystemFontGlyphSubstitution GSub { get; }

  internal abstract ushort GlyphCount { get; }

  internal ushort NumberOfHorizontalMetrics => this.HHea.NumberOfHMetrics;

  internal SystemFontOpenTypeGlyphScaler Scaler => this.scaler;

  public override short Ascender => this.HHea.Ascender;

  public override short Descender => this.HHea.Descender;

  public SystemFontOpenTypeFontSourceBase(SystemFontOpenTypeFontReader reader)
  {
    this.reader = reader;
    this.scaler = new SystemFontOpenTypeGlyphScaler(this);
  }

  public SystemFontGlyph GetGlyphMetrics(ushort glyphId, ushort previousGlyphId, double fontSize)
  {
    SystemFontGlyph glyphMetrics = this.GetGlyphMetrics(glyphId, fontSize);
    SystemFontKerningInfo kerning = this.Kern.GetKerning(previousGlyphId, glyphId);
    glyphMetrics.HorizontalKerning = (Syncfusion.PdfViewer.Base.Point) this.scaler.FUnitsPointToPixels((System.Drawing.Point) kerning.HorizontalKerning, fontSize);
    glyphMetrics.VerticalKerning = (Syncfusion.PdfViewer.Base.Point) this.scaler.FUnitsPointToPixels((System.Drawing.Point) kerning.VerticalKerning, fontSize);
    return glyphMetrics;
  }

  public SystemFontGlyph GetGlyphMetrics(ushort glyphId, double fontSize)
  {
    SystemFontGlyph glyph = new SystemFontGlyph();
    glyph.GlyphId = glyphId;
    this.scaler.ScaleGlyphMetrics(glyph, fontSize);
    return glyph;
  }

  public override void GetAdvancedWidth(SystemFontGlyph glyph)
  {
    this.scaler.ScaleGlyphMetrics(glyph, 1.0);
  }

  internal SystemFontScript GetScript(uint tag) => this.GSub.GetScript(tag);

  internal SystemFontFeature GetFeature(ushort index) => this.GSub.GetFeature(index);

  internal SystemFontLookup GetLookup(ushort index) => this.GSub.GetLookup(index);

  internal abstract SystemFontGlyphData GetGlyphData(ushort glyphID);

  public ushort GetGlyphId(ushort ch) => this.CMap.GetGlyphId(ch);

  public double GetLineHeight(double fontSize)
  {
    return this.Scaler.FUnitsToPixels((double) ((int) Math.Abs(this.HHea.Ascender) + (int) Math.Abs(this.HHea.Descender) + (int) this.HHea.LineGap), fontSize);
  }

  public double GetBaselineOffset(double fontSize)
  {
    return this.Scaler.FUnitsToPixels((double) ((int) Math.Abs(this.HHea.Ascender) + (int) Math.Abs(this.HHea.LineGap)), fontSize);
  }

  public override void GetGlyphOutlines(SystemFontGlyph glyph, double fontSize)
  {
    this.scaler.GetScaleGlyphOutlines(glyph, fontSize);
  }

  internal void Write(SystemFontFontWriter writer)
  {
    writer.WriteString(this.FontFamily);
    ushort us = 0;
    if (this.IsBold)
      us |= (ushort) 1;
    if (this.IsItalic)
      us |= (ushort) 2;
    writer.WriteUShort(us);
    writer.WriteUShort(this.GlyphCount);
    this.Head.Write(writer);
    this.CMap.Write(writer);
    this.HHea.Write(writer);
    this.HMtx.Write(writer);
    this.Kern.Write(writer);
    this.GSub.Write(writer);
  }
}
