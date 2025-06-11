// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.IndicGlyphInfoList
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class IndicGlyphInfoList : OtfGlyphInfoList
{
  private int m_glyphInfoStart;
  private int m_glyphInfoEnd;

  internal int GlyphInfoStart
  {
    get => this.m_glyphInfoStart;
    set => this.m_glyphInfoStart = value;
  }

  internal int GlyphInfoEnd
  {
    get => this.m_glyphInfoEnd;
    set => this.m_glyphInfoEnd = value;
  }

  internal IndicGlyphInfo this[int index]
  {
    get
    {
      if (!(this.Glyphs[index] is IndicGlyphInfo))
        this.Glyphs[index] = (OtfGlyphInfo) new IndicGlyphInfo(this.Glyphs[index]);
      return this.Glyphs[index] as IndicGlyphInfo;
    }
  }

  internal IndicGlyphInfoList(OtfGlyphInfoList glyphInfoList, int start, int end, string text)
    : base(glyphInfoList, start, end)
  {
    List<OtfGlyphInfo> glyphs = new List<OtfGlyphInfo>();
    foreach (OtfGlyphInfo glyph in this.Glyphs)
      glyphs.Add(glyph is IndicGlyphInfo ? glyph : (OtfGlyphInfo) new IndicGlyphInfo(glyph));
    this.SetGlyphs(glyphs);
    this.GlyphInfoStart = start;
    this.GlyphInfoEnd = end;
    this.Text = new List<string>(this.Glyphs.Count);
    List<string> stringList = new List<string>();
    stringList.Add(text);
    for (int index = 0; index < this.Glyphs.Count; ++index)
      this.Text.Add(stringList.ToString());
  }

  internal void DoOrder() => this.Order<OtfGlyphInfo>(this.Glyphs, 0, this.Glyphs.Count - 1);

  private void Order<T>(List<T> glyphList, int l, int r)
  {
    if (r <= l)
      return;
    int r1 = (r + l) / 2;
    this.Order<T>(glyphList, l, r1);
    this.Order<T>(glyphList, r1 + 1, r);
    this.Order<T>(glyphList, l, r1 + 1, r, new GlyphComparer() as IComparer<T>);
  }

  private void Order<T>(List<T> glyphList, int l, int m, int r, IComparer<T> comparer)
  {
    int num = m - 1;
    int capacity = r - l + 1;
    List<T> objList = new List<T>(capacity);
    while (l <= num && m <= r)
    {
      if (comparer.Compare(glyphList[l], glyphList[m]) <= 0)
        objList.Add(glyphList[l++]);
      else
        objList.Add(glyphList[m++]);
    }
    while (l <= num)
      objList.Add(glyphList[l++]);
    while (m <= r)
      objList.Add(glyphList[m++]);
    for (int index = capacity - 1; index >= 0; --index)
      glyphList[r--] = objList[index];
  }

  internal virtual string GetText()
  {
    return this.Text.GetEnumerator().MoveNext() ? this.Text.GetEnumerator().Current : (string) null;
  }

  internal override void CombineAlternateGlyphs(
    OtfTable table,
    int flag,
    int length,
    int glyphIndex)
  {
    IndicGlyphInfo indicGlyphInfo = this[this.Index];
    base.CombineAlternateGlyphs(table, flag, length, glyphIndex);
    this.Glyphs[this.Index] = (OtfGlyphInfo) new IndicGlyphInfo(this.Glyphs[this.Index], indicGlyphInfo.Group, indicGlyphInfo.Position, indicGlyphInfo.Mask, true, true);
  }

  internal override void CombineAlternateGlyphs(OtfTable table, int glyphIndex)
  {
    IndicGlyphInfo indicGlyphInfo = this[this.Index];
    base.CombineAlternateGlyphs(table, glyphIndex);
    this.Glyphs[this.Index] = (OtfGlyphInfo) new IndicGlyphInfo(this.Glyphs[this.Index], indicGlyphInfo.Group, indicGlyphInfo.Position, indicGlyphInfo.Mask, true, false);
  }

  internal override void CombineAlternateGlyphs(OtfTable table, int[] glyphIndexes)
  {
    IndicGlyphInfo indicGlyphInfo = this[this.Index];
    base.CombineAlternateGlyphs(table, glyphIndexes);
    for (int index = 0; index < glyphIndexes.Length; ++index)
    {
      if (this.Index + index < this.Glyphs.Count)
        this.Glyphs[this.Index + index] = (OtfGlyphInfo) new IndicGlyphInfo(this.Glyphs[this.Index + index], indicGlyphInfo.Group, indicGlyphInfo.Position, indicGlyphInfo.Mask, true, false);
    }
  }

  internal new OtfGlyphInfoList SubSet(int start, int end)
  {
    OtfGlyphInfoList glyphInfoList = base.SubSet(start, end);
    return (OtfGlyphInfoList) new IndicGlyphInfoList(glyphInfoList, glyphInfoList.Start, glyphInfoList.End, this.GetText())
    {
      GlyphInfoStart = this.GlyphInfoStart,
      GlyphInfoEnd = this.GlyphInfoEnd
    };
  }

  internal virtual void Rearrange(int d, int s, int count)
  {
    List<OtfGlyphInfo> otfGlyphInfoList = new List<OtfGlyphInfo>();
    for (int index = 0; index < count; ++index)
      otfGlyphInfoList.Add(this.Glyphs[s + index]);
    for (int index = 0; index < count; ++index)
      this.Glyphs[d + index] = otfGlyphInfoList[index];
  }

  public override bool Equals(object obj)
  {
    if (this == obj)
      return true;
    if (obj == null || this.GetType() != obj.GetType())
      return false;
    IndicGlyphInfoList indicGlyphInfoList = (IndicGlyphInfoList) obj;
    bool flag;
    if (this.Glyphs.Count == indicGlyphInfoList.Glyphs.Count)
    {
      flag = true;
      for (int index = 0; index < this.Glyphs.Count; ++index)
      {
        if (!this.Glyphs[index].CharCode.Equals(indicGlyphInfoList.Glyphs[index].CharCode))
        {
          flag = false;
          break;
        }
      }
    }
    else
      flag = false;
    return flag;
  }
}
