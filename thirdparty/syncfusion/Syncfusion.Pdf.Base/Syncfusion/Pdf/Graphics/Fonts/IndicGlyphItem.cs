// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.IndicGlyphItem
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class IndicGlyphItem
{
  private IndicGlyphInfoList m_glyphList;
  private int m_position;

  internal IndicGlyphInfoList GlyphList
  {
    get => this.m_glyphList;
    set => this.m_glyphList = value;
  }

  internal int Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  internal virtual int Length => this.GlyphList.End - this.GlyphList.Start;

  internal IndicGlyphItem(IndicGlyphInfoList glyphInfoList, int position)
  {
    this.GlyphList = glyphInfoList;
    this.Position = position;
  }

  public override bool Equals(object obj)
  {
    if (this == obj)
      return true;
    if (obj == null || this.GetType() != obj.GetType())
      return false;
    IndicGlyphItem indicGlyphItem = (IndicGlyphItem) obj;
    return this.Length == indicGlyphItem.Length && this.GlyphList.Equals((object) indicGlyphItem.GlyphList);
  }
}
