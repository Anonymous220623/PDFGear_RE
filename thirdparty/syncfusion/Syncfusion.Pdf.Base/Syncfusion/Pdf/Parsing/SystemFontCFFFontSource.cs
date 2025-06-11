// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontCFFFontSource
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontCFFFontSource : SystemFontFontSource
{
  private readonly SystemFontCFFFontFile file;
  private readonly SystemFontTop top;

  internal SystemFontCFFFontFile File => this.file;

  internal SystemFontCFFFontReader Reader => this.file.Reader;

  public override string FontFamily => this.top.FamilyName;

  public override bool IsBold => throw new NotImplementedException();

  public override bool IsItalic => throw new NotImplementedException();

  public override short Ascender => throw new NotImplementedException();

  public override short Descender => throw new NotImplementedException();

  public bool UsesCIDFontOperators => this.top.UsesCIDFontOperators;

  public SystemFontCFFFontSource(SystemFontCFFFontFile file, SystemFontTop top)
  {
    this.file = file;
    this.top = top;
  }

  public ushort GetGlyphId(string name) => this.top.GetGlyphId(name);

  public ushort GetGlyphId(ushort cid) => this.top.GetGlyphId(cid);

  public string GetGlyphName(ushort cid) => this.top.GetGlyphName(cid);

  public override void GetAdvancedWidth(SystemFontGlyph glyph)
  {
    glyph.AdvancedWidth = (double) this.top.GetAdvancedWidth(glyph.GlyphId) / 1000.0;
  }

  public override void GetGlyphOutlines(SystemFontGlyph glyph, double fontSize)
  {
    this.top.GetGlyphOutlines(glyph, fontSize);
  }
}
