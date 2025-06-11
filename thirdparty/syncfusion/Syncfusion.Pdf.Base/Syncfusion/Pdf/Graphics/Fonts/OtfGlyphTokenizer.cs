// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.OtfGlyphTokenizer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class OtfGlyphTokenizer
{
  internal OtfGlyphInfoList m_glyphInfoList;
  internal int m_position;

  internal OtfGlyphTokenizer(OtfGlyphInfoList glyphInfoList)
  {
    this.m_glyphInfoList = glyphInfoList;
  }

  internal OtfGlyphTokenizer()
  {
  }

  internal OtfGlyphInfo[] ReadWord(out string text)
  {
    StringBuilder stringBuilder = new StringBuilder();
    List<OtfGlyphInfo> otfGlyphInfoList = new List<OtfGlyphInfo>();
    int position;
    for (position = this.m_position; position < this.m_glyphInfoList.Glyphs.Count; ++position)
    {
      OtfGlyphInfo glyph = this.m_glyphInfoList.Glyphs[position];
      if (glyph.Index != 3 && glyph.CharCode != 32 /*0x20*/ || glyph.unsupportedGlyph)
      {
        otfGlyphInfoList.Add(glyph);
        if (glyph.Characters != null)
        {
          foreach (char character in glyph.Characters)
            stringBuilder.Append(character);
        }
      }
      else
      {
        if (otfGlyphInfoList.Count == 0)
        {
          otfGlyphInfoList.Add(glyph);
          if (glyph.Characters != null)
          {
            foreach (char character in glyph.Characters)
              stringBuilder.Append(character);
          }
        }
        if (position == this.m_position)
        {
          ++this.m_position;
          break;
        }
        this.m_position = position;
        break;
      }
    }
    if (position == this.m_glyphInfoList.Glyphs.Count)
      this.m_position = position;
    text = stringBuilder.ToString();
    return otfGlyphInfoList.ToArray();
  }

  internal OtfGlyphInfo[][] SplitGlyphs(List<OtfGlyphInfo> glyphs)
  {
    List<OtfGlyphInfo[]> otfGlyphInfoArrayList = new List<OtfGlyphInfo[]>();
    List<OtfGlyphInfo> otfGlyphInfoList = new List<OtfGlyphInfo>();
    for (int index = 0; index < glyphs.Count; ++index)
    {
      OtfGlyphInfo glyph = glyphs[index];
      if (glyph.Index != 3 && glyph.CharCode != 32 /*0x20*/)
        otfGlyphInfoList.Add(glyph);
      else if (otfGlyphInfoList.Count > 0 && (otfGlyphInfoList.Count != 1 || otfGlyphInfoList[0].Index != 32 /*0x20*/))
      {
        otfGlyphInfoArrayList.Add(otfGlyphInfoList.ToArray());
        otfGlyphInfoList = new List<OtfGlyphInfo>();
      }
    }
    if (otfGlyphInfoList.Count > 0)
      otfGlyphInfoArrayList.Add(otfGlyphInfoList.ToArray());
    return otfGlyphInfoArrayList.ToArray();
  }

  internal float GetLineWidth(OtfGlyphInfo[] glyphs, PdfTrueTypeFont font, PdfStringFormat format)
  {
    float num = 0.0f;
    foreach (OtfGlyphInfo glyph in glyphs)
      num += glyph.Width;
    float size = font.Metrics.GetSize(format);
    float width = num * (1f / 1000f * size);
    return this.ApplyFormatSettings(glyphs, format, width);
  }

  internal float GetLineWidth(
    OtfGlyphInfo[] glyphs,
    PdfTrueTypeFont font,
    PdfStringFormat format,
    string text,
    out float outWordSpace,
    out float outCharSpace)
  {
    float num = 0.0f;
    outWordSpace = 0.0f;
    outCharSpace = 0.0f;
    foreach (OtfGlyphInfo glyph in glyphs)
      num += glyph.Width;
    float size = font.Metrics.GetSize(format);
    float width = num * (1f / 1000f * size);
    return text == null ? this.ApplyFormatSettings(glyphs, format, width) : this.ApplyFormatSettings(glyphs, format, width, text, out outWordSpace, out outCharSpace);
  }

  internal float GetLineWidth(OtfGlyphInfo glyphs, PdfTrueTypeFont font, PdfStringFormat format)
  {
    float width = glyphs.Width * (1f / 1000f * font.Metrics.GetSize(format));
    return this.ApplyFormatSettings(new OtfGlyphInfo[1]
    {
      glyphs
    }, format, width);
  }

  protected float ApplyFormatSettings(
    OtfGlyphInfo[] glyphs,
    PdfStringFormat format,
    float width,
    string text,
    out float outWordSpace,
    out float outCharSpace)
  {
    float num = width;
    outWordSpace = 0.0f;
    outCharSpace = 0.0f;
    if (format != null && (double) width > 0.0)
    {
      if ((double) format.CharacterSpacing != 0.0 && text != null)
        outCharSpace += (float) (text.Length - 1) * format.CharacterSpacing;
      if ((double) format.WordSpacing != 0.0 && text != null)
      {
        char[] spaces = StringTokenizer.Spaces;
        int charsCount = StringTokenizer.GetCharsCount(text, spaces);
        outWordSpace += (float) charsCount * format.WordSpacing;
      }
    }
    return num;
  }

  protected float ApplyFormatSettings(OtfGlyphInfo[] glyphs, PdfStringFormat format, float width)
  {
    float num = width;
    if (format != null && (double) width > 0.0)
    {
      if ((double) format.CharacterSpacing != 0.0)
        num += (float) (glyphs.Length - 1) * format.CharacterSpacing;
      if ((double) format.WordSpacing != 0.0)
      {
        int charsCount = this.GetCharsCount(glyphs);
        num += (float) charsCount * format.WordSpacing;
      }
    }
    return num;
  }

  protected int GetCharsCount(OtfGlyphInfo[] glyphs)
  {
    int charsCount = 0;
    foreach (OtfGlyphInfo glyph in glyphs)
    {
      if (glyph.CharCode == 32 /*0x20*/)
        ++charsCount;
    }
    return charsCount;
  }

  internal OtfGlyphInfoList TrimEndSpaces(OtfGlyphInfoList glyphList)
  {
    List<OtfGlyphInfo> glyphs = glyphList.Glyphs;
    for (int index = glyphList.Glyphs.Count - 1; index >= 0 && (glyphList.Glyphs[index].Index == 3 || glyphList.Glyphs[index].CharCode == 32 /*0x20*/); --index)
      glyphs.RemoveAt(glyphs.Count - 1);
    return new OtfGlyphInfoList(glyphs, 0, glyphs.Count);
  }

  internal OtfGlyphInfoList TrimStartSpaces(OtfGlyphInfoList glyphList)
  {
    List<OtfGlyphInfo> glyphs = glyphList.Glyphs;
    for (int index = 0; index <= glyphList.Glyphs.Count - 1 && (glyphList.Glyphs[index].Index == 3 || glyphList.Glyphs[index].CharCode == 32 /*0x20*/); ++index)
      glyphs.RemoveAt(0);
    return new OtfGlyphInfoList(glyphs, 0, glyphs.Count);
  }
}
