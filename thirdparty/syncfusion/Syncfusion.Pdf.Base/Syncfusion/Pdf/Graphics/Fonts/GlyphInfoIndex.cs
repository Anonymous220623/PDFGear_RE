// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.GlyphInfoIndex
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class GlyphInfoIndex
{
  private OtfGlyphInfoList m_glyphInfoList;
  private OtfGlyphInfo m_glyphInfo;
  private int m_index;

  internal OtfGlyphInfoList GlyphInfoList
  {
    get => this.m_glyphInfoList;
    set => this.m_glyphInfoList = value;
  }

  internal OtfGlyphInfo GlyphInfo
  {
    get => this.m_glyphInfo;
    set => this.m_glyphInfo = value;
  }

  internal int Index
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  internal virtual void MoveNext(OtfTable table, int flag)
  {
    this.GlyphInfo = (OtfGlyphInfo) null;
    while (++this.Index < this.GlyphInfoList.End)
    {
      OtfGlyphInfo glyph = this.GlyphInfoList.Glyphs[this.Index];
      if (!table.GDEFTable.IsSkip(glyph.Index, flag))
      {
        this.GlyphInfo = glyph;
        break;
      }
    }
  }

  internal virtual void MovePrevious(OtfTable table, int flag)
  {
    this.GlyphInfo = (OtfGlyphInfo) null;
    while (--this.Index >= this.GlyphInfoList.Start)
    {
      OtfGlyphInfo glyph = this.GlyphInfoList.Glyphs[this.Index];
      if (!table.GDEFTable.IsSkip(glyph.Index, flag))
      {
        this.GlyphInfo = glyph;
        break;
      }
    }
  }
}
