// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.OtfGlyphInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class OtfGlyphInfo
{
  private int m_index;
  private int m_charCode;
  private char[] m_chars;
  private float m_width;
  internal int leadingX;
  internal int leadingY;
  internal bool unsupportedGlyph;

  internal int Index
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  internal int CharCode
  {
    get => this.m_charCode;
    set => this.m_charCode = value;
  }

  internal char[] Characters
  {
    get => this.m_chars;
    set => this.m_chars = value;
  }

  internal float Width
  {
    get => this.m_width;
    set => this.m_width = value;
  }

  internal OtfGlyphInfo(int charCode, int index, float width)
  {
    this.m_charCode = charCode;
    this.m_index = index;
    this.m_width = width;
    if (charCode <= -1)
      return;
    this.Characters = char.ConvertFromUtf32(charCode).ToCharArray();
  }

  internal OtfGlyphInfo(OtfGlyphInfo glyph, int x, int y)
    : this(glyph)
  {
    this.leadingX = x;
    this.leadingY = y;
  }

  internal OtfGlyphInfo(OtfGlyphInfo glyph)
  {
    this.m_index = glyph.Index;
    this.m_chars = glyph.Characters;
    this.m_charCode = glyph.CharCode;
    this.m_width = glyph.Width;
  }
}
