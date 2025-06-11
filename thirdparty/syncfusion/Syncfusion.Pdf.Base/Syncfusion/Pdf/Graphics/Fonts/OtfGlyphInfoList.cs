// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.OtfGlyphInfoList
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class OtfGlyphInfoList
{
  private List<OtfGlyphInfo> m_glyphs;
  private int m_start;
  private int m_end;
  private int m_index;
  private List<string> m_text = new List<string>();
  private bool m_isThaiShape;

  internal List<string> Text
  {
    get => this.m_text;
    set => this.m_text = value;
  }

  internal List<OtfGlyphInfo> Glyphs
  {
    get
    {
      if (this.m_glyphs == null)
        this.m_glyphs = new List<OtfGlyphInfo>();
      return this.m_glyphs;
    }
    set => this.m_glyphs = value;
  }

  internal int Start
  {
    get => this.m_start;
    set => this.m_start = value;
  }

  internal int End
  {
    get => this.m_end;
    set => this.m_end = value;
  }

  internal int Index
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  internal bool IsThaiShaping
  {
    get => this.m_isThaiShape;
    set => this.m_isThaiShape = value;
  }

  internal OtfGlyphInfoList(List<OtfGlyphInfo> glyphs, int start, int end)
  {
    this.Glyphs = glyphs;
    this.Start = start;
    this.End = end;
  }

  internal OtfGlyphInfoList(OtfGlyphInfo[] glyphs, int start, int end)
  {
    this.Glyphs = new List<OtfGlyphInfo>();
    foreach (OtfGlyphInfo glyph in glyphs)
      this.Glyphs.Add(glyph);
    this.Start = start;
    this.End = end;
  }

  internal OtfGlyphInfoList()
  {
  }

  internal OtfGlyphInfoList(List<OtfGlyphInfo> glyphs)
  {
    this.Glyphs = glyphs;
    this.Start = 0;
    this.End = glyphs.Count;
  }

  internal OtfGlyphInfoList(OtfGlyphInfoList glyphList, int start, int end)
  {
    this.Glyphs = glyphList.Glyphs.GetRange(start, end - start);
    if (glyphList.Text.Count > 0)
      this.Text = glyphList.Text.GetRange(start, end - start);
    this.Start = 0;
    this.End = end - start;
    this.Index = glyphList.Index - start;
  }

  internal bool HasYPlacement()
  {
    bool flag = false;
    if (this.Glyphs != null && this.Glyphs.Count > 0)
    {
      foreach (OtfGlyphInfo glyph in this.Glyphs)
      {
        if (glyph.leadingY != 0)
        {
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  internal void SetGlyphs(List<OtfGlyphInfo> glyphs)
  {
    this.Glyphs = glyphs;
    this.Start = 0;
    this.End = glyphs.Count;
    this.Text = (List<string>) null;
  }

  internal virtual void CombineAlternateGlyphs(OtfTable table, int[] glyphs)
  {
    int glyph1 = glyphs[0];
    this.Glyphs[this.Index] = table.GetGlyph(glyph1);
    if (glyphs.Length > 1)
    {
      IList<OtfGlyphInfo> glyphs1 = (IList<OtfGlyphInfo>) new List<OtfGlyphInfo>(glyphs.Length - 1);
      for (int index = 1; index < glyphs.Length; ++index)
      {
        int glyph2 = glyphs[index];
        OtfGlyphInfo glyph3 = table.GetGlyph(glyph2);
        glyphs1.Add(glyph3);
      }
      this.InsertGlyphs(this.Index + 1, glyphs1);
      this.Index += glyphs.Length - 1;
      this.End += glyphs.Length - 1;
    }
    if (!this.IsThaiShaping || glyphs.Length <= 1)
      return;
    int end = this.Index - 1 + 2;
    int num = end - 2;
    while (num > 0 && this.ThaiToneMark(this.Glyphs[num - 1].CharCode))
      --num;
    if (num + 2 >= end)
      return;
    OtfGlyphInfo glyph4 = this.Glyphs[end - 2];
    this.MoveGlyph(num + 1, num, end - num - 2);
    this.Set(num, glyph4);
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = num + 1; index <= end - 2; ++index)
    {
      if (index <= this.Glyphs.Count - 1)
        stringBuilder.Append(char.ConvertFromUtf32(this.Glyphs[index].CharCode));
    }
    stringBuilder.Append("ำ");
    this.SetText(num, end, stringBuilder.ToString());
  }

  internal virtual void CombineAlternateGlyphs(
    OtfTable table,
    int flag,
    int length,
    int glyphIndex)
  {
    GlyphInfoIndex glyphInfoIndex = new GlyphInfoIndex();
    glyphInfoIndex.GlyphInfoList = this;
    glyphInfoIndex.Index = this.Index;
    StringBuilder stringBuilder = new StringBuilder();
    OtfGlyphInfo glyph1 = this.Glyphs[this.Index];
    if (glyph1.Characters != null)
      stringBuilder.Append(glyph1.Characters);
    else if (glyph1.CharCode > -1)
      stringBuilder.Append(char.ConvertFromUtf32(glyph1.CharCode));
    for (int index = 0; index < length; ++index)
    {
      glyphInfoIndex.MoveNext(table, flag);
      OtfGlyphInfo glyph2 = this.Glyphs[glyphInfoIndex.Index];
      if (glyph2.Characters != null)
        stringBuilder.Append(glyph2.Characters);
      else if (glyph2.CharCode > -1)
        stringBuilder.Append(char.ConvertFromUtf32(glyph2.CharCode));
      this.RemoveGlyph(glyphInfoIndex.Index--);
    }
    char[] destinationArray = new char[stringBuilder.Length];
    Array.Copy((Array) stringBuilder.ToString().ToCharArray(), 0, (Array) destinationArray, 0, stringBuilder.Length);
    OtfGlyphInfo otfGlyphInfo = table.GetGlyph(glyphIndex);
    if (this.IsThaiShaping && otfGlyphInfo.CharCode == 0)
      otfGlyphInfo = new OtfGlyphInfo(-1, glyphIndex, 0.0f);
    otfGlyphInfo.Characters = destinationArray;
    this.Glyphs[this.Index] = otfGlyphInfo;
    this.End -= length;
  }

  private void RemoveGlyph(int index)
  {
    this.Glyphs.RemoveAt(index);
    if (this.Text.Count <= 0)
      return;
    this.Text.RemoveAt(index);
  }

  internal virtual void CombineAlternateGlyphs(OtfTable table, int glyphIndex)
  {
    OtfGlyphInfo glyph = this.Glyphs[this.Index];
    OtfGlyphInfo otfGlyphInfo = table.GetGlyph(glyphIndex);
    if (glyph.Characters != null)
    {
      if (this.IsThaiShaping && otfGlyphInfo.CharCode == 0)
        otfGlyphInfo = new OtfGlyphInfo(-1, glyphIndex, 0.0f);
      otfGlyphInfo.Characters = glyph.Characters;
    }
    else if (otfGlyphInfo.CharCode > -1)
      otfGlyphInfo.Characters = char.ConvertFromUtf32(otfGlyphInfo.CharCode).ToCharArray();
    else if (glyph.CharCode > -1)
      otfGlyphInfo.Characters = char.ConvertFromUtf32(glyph.CharCode).ToCharArray();
    this.Glyphs[this.Index] = otfGlyphInfo;
  }

  internal virtual OtfGlyphInfo Set(int index, OtfGlyphInfo glyph) => this.Glyphs[index] = glyph;

  internal virtual OtfGlyphInfoList SubSet(int start, int end)
  {
    return new OtfGlyphInfoList()
    {
      Start = 0,
      End = end - start,
      Glyphs = this.Glyphs.GetRange(start, end - start),
      Text = this.Text.Count > 0 ? new List<string>((IEnumerable<string>) this.Text.GetRange(start, end - start)) : new List<string>()
    };
  }

  internal virtual void ReplaceContent(OtfGlyphInfoList glyphList)
  {
    this.Glyphs.Clear();
    for (int index = 0; index < glyphList.Glyphs.Count; ++index)
      this.Glyphs.Add(glyphList.Glyphs[index]);
    if (this.Text.Count > 0)
      this.Text.Clear();
    if (glyphList.Text != null)
    {
      if (this.Text == null)
        this.Text = new List<string>();
      for (int index = 0; index < glyphList.Text.Count; ++index)
        this.Text.Add(glyphList.Text[index]);
    }
    this.Start = glyphList.Start;
    this.End = glyphList.End;
  }

  internal virtual void SetText(int start, int end, string text)
  {
    if (this.Text.Count == 0)
    {
      this.Text = new List<string>(this.Glyphs.Count);
      for (int index = 0; index < this.Glyphs.Count; ++index)
        this.Text.Add((string) null);
    }
    for (int index = start; index < end; ++index)
      this.Text[index] = text;
  }

  private void InsertGlyphs(int index, IList<OtfGlyphInfo> glyphs)
  {
    for (int index1 = glyphs.Count - 1; index1 >= 0; --index1)
      this.Glyphs.Insert(index, glyphs[index1]);
    if (this.Text.Count == 0)
      return;
    for (int index2 = 0; index2 < glyphs.Count; ++index2)
      this.Text.Insert(index, (string) null);
  }

  private void MoveGlyph(int end, int start, int count)
  {
    IList<OtfGlyphInfo> otfGlyphInfoList = (IList<OtfGlyphInfo>) new List<OtfGlyphInfo>(count);
    for (int index = 0; index < count; ++index)
      otfGlyphInfoList.Add(this.Glyphs[start + index]);
    for (int index = 0; index < count; ++index)
      this.Glyphs[end + index] = otfGlyphInfoList[index];
  }

  private bool ThaiToneMark(int charcode)
  {
    return this.ThaiGlyphRanges(charcode & -129, 3636, 3639, 3655, 3662, 3633, 3633);
  }

  private bool ThaiGlyphRanges(
    int charcode,
    int lowest1,
    int heighest1,
    int lowest2,
    int heighest2,
    int lowest3,
    int heighest3)
  {
    return this.ThaiGlyphRange(charcode, lowest1, heighest1) || this.ThaiGlyphRange(charcode, lowest2, heighest2) || this.ThaiGlyphRange(charcode, lowest3, heighest3);
  }

  private bool ThaiGlyphRange(int charcode, int low, int high)
  {
    return charcode >= low && charcode <= high;
  }
}
