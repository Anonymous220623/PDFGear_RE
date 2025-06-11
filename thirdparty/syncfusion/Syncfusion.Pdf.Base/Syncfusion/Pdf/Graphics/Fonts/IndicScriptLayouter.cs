// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.IndicScriptLayouter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class IndicScriptLayouter
{
  private Dictionary<int, int[]> m_defaultGlyphs = new Dictionary<int, int[]>();

  internal IndicScriptLayouter()
  {
    this.m_defaultGlyphs.Add(6078, new int[2]{ 6081, 6078 });
    this.m_defaultGlyphs.Add(6079, new int[2]{ 6081, 6079 });
    this.m_defaultGlyphs.Add(6080, new int[2]{ 6081, 6080 });
    this.m_defaultGlyphs.Add(6084, new int[2]{ 6081, 6084 });
    this.m_defaultGlyphs.Add(6085, new int[2]{ 6081, 6085 });
  }

  internal void ReplaceDefaultGlyphs(UnicodeTrueTypeFont font, OtfGlyphInfoList glyphList)
  {
    int index = glyphList.Index;
    for (int start = glyphList.Start; start < glyphList.End; ++start)
    {
      if (glyphList.Glyphs[start].CharCode > -1 && this.m_defaultGlyphs.ContainsKey(glyphList.Glyphs[start].CharCode))
      {
        this.Replace(glyphList, start, font, this.m_defaultGlyphs[glyphList.Glyphs[start].CharCode]);
        ++start;
      }
    }
    glyphList.Index = index;
  }

  internal void SetPosition(
    UnicodeTrueTypeFont font,
    IndicGlyphInfoList indicGlyphList,
    IndicScript iScript,
    IList<LookupTable> belowBaseForm,
    IList<LookupTable> postBaseForm,
    IList<LookupTable> preBaseForm)
  {
    if (iScript.Position != 2)
      return;
    OtfGlyphInfo glyph = this.GetGlyph(font, iScript);
    if (glyph == null)
      return;
    for (int index = 0; index < indicGlyphList.Glyphs.Count; ++index)
    {
      if (indicGlyphList[index].Position == 4)
      {
        OtfGlyphInfo indicGlyph = (OtfGlyphInfo) indicGlyphList[index];
        indicGlyphList[index].Position = this.GetPosition(indicGlyph, glyph, iScript, belowBaseForm, postBaseForm, preBaseForm);
      }
    }
  }

  internal void Reorder(
    IndicGlyphInfoList glyphInfoList,
    IndicScript indicScript,
    IList<LookupTable> rephForm,
    IList<LookupTable> prebaseForm,
    bool oldScript,
    ScriptTags scriptTag)
  {
    int index1;
    int index2 = index1 = 0;
    int index3;
    int num1 = index3 = glyphInfoList.Glyphs.Count;
    bool flag1 = false;
    if (indicScript.RephPosition != 1 && glyphInfoList.Glyphs.Count >= 3 && (indicScript.RephMode == 0 && !this.IsCombined(glyphInfoList[index2 + 2].Group) || indicScript.RephMode == 1 && glyphInfoList[index2 + 2].Group == 6))
    {
      List<OtfGlyphInfo> glyphInfo = new List<OtfGlyphInfo>();
      glyphInfo.Add((OtfGlyphInfo) glyphInfoList[0]);
      glyphInfo.Add((OtfGlyphInfo) glyphInfoList[1]);
      glyphInfo.Add(indicScript.RephMode == 1 ? (OtfGlyphInfo) glyphInfoList[2] : (OtfGlyphInfo) null);
      if (this.Replace(rephForm, (IList<OtfGlyphInfo>) glyphInfo, 2) || indicScript.RephMode == 1 && this.Replace(rephForm, (IList<OtfGlyphInfo>) glyphInfo, 3))
      {
        index1 += 2;
        while (index1 < num1 && this.IsCombined(glyphInfoList[index1].Group))
          ++index1;
        index3 = index2;
        flag1 = true;
      }
    }
    else if (indicScript.RephMode == 3 && glyphInfoList[index2].Group == 15)
    {
      ++index1;
      while (index1 < num1 && this.IsCombined(glyphInfoList[index1].Group))
        ++index1;
      index3 = index2;
      flag1 = true;
    }
    if (indicScript.Position == 0)
    {
      index3 = index2;
      for (int index4 = index3 + 1; index4 < num1; ++index4)
      {
        if (this.IsConsonant(glyphInfoList[index4].Group))
          glyphInfoList[index4].Position = 8;
      }
    }
    else if (indicScript.Position == 1)
    {
      if (!flag1)
        index3 = index1;
      for (int index5 = index1; index5 < num1; ++index5)
      {
        if (this.IsConsonant(glyphInfoList[index5].Group))
        {
          if (index1 >= index5 || glyphInfoList[index5 - 1].Group != 6)
            index3 = index5;
          else
            break;
        }
      }
      for (int index6 = index3 + 1; index6 < num1; ++index6)
      {
        if (this.IsConsonant(glyphInfoList[index6].Group))
          glyphInfoList[index6].Position = 8;
      }
    }
    else if (indicScript.Position == 2)
    {
      int index7 = num1;
      bool flag2 = false;
      bool flag3 = false;
      do
      {
        --index7;
        if (this.IsConsonant(glyphInfoList[index7].Group))
        {
          if (glyphInfoList[index7].Position == 8 || !(glyphInfoList[index7].Position != 11 | flag2))
          {
            if (glyphInfoList[index7].Position == 8)
              flag2 = true;
            index3 = index7;
          }
          else
            goto label_42;
        }
        else if (index2 < index7 && glyphInfoList[index7].Group == 6 && glyphInfoList[index7 - 1].Group == 4)
        {
          flag3 = true;
          goto label_42;
        }
      }
      while (index7 > index1);
      flag3 = true;
label_42:
      if (!flag3)
        index3 = index7;
    }
    if (flag1 && index3 == index2 && index1 - index3 <= 2)
      flag1 = false;
    for (int index8 = index2; index8 < index3; ++index8)
      glyphInfoList[index8].Position = Math.Min(3, glyphInfoList[index8].Position);
    if (index3 < num1)
      glyphInfoList[index3].Position = 4;
    for (int index9 = index3 + 1; index9 < num1; ++index9)
    {
      if (glyphInfoList[index9].Group == 7)
      {
        for (int index10 = index9 + 1; index10 < num1; ++index10)
        {
          if (this.IsConsonant(glyphInfoList[index10].Group))
          {
            glyphInfoList[index10].Position = 13;
            break;
          }
        }
        break;
      }
    }
    if (flag1)
      glyphInfoList[index2].Position = 1;
    if (oldScript)
    {
      bool flag4 = scriptTag != ScriptTags.Malayalam;
      for (int index11 = index3 + 1; index11 < num1; ++index11)
      {
        if (glyphInfoList[index11].Group == 4)
        {
          int index12 = num1 - 1;
          while (index12 > index11 && !this.IsConsonant(glyphInfoList[index12].Group) && (!flag4 || glyphInfoList[index12].Group != 4))
            --index12;
          if (glyphInfoList[index12].Group != 4 && index12 > index11)
          {
            OtfGlyphInfo glyphInfo = (OtfGlyphInfo) glyphInfoList[index11];
            glyphInfoList.Rearrange(index11, index11 + 1, index12 - index11);
            glyphInfoList.Set(index12, glyphInfo);
            break;
          }
          break;
        }
      }
    }
    int num2 = 0;
    for (int index13 = index2; index13 < num1; ++index13)
    {
      if (((long) (1 << glyphInfoList[index13].Group) & 2147508344L) != 0L)
      {
        glyphInfoList[index13].Position = num2;
        if (glyphInfoList[index13].Group == 4 && glyphInfoList[index13].Position == 2)
        {
          for (int index14 = index13; index14 > index2; --index14)
          {
            if (glyphInfoList[index14 - 1].Position != 2)
            {
              glyphInfoList[index13].Position = glyphInfoList[index14 - 1].Position;
              break;
            }
          }
        }
      }
      else if (glyphInfoList[index13].Position != 14)
        num2 = glyphInfoList[index13].Position;
    }
    int num3 = index3;
    for (int index15 = index3 + 1; index15 < num1; ++index15)
    {
      if (this.IsConsonant(glyphInfoList[index15].Group))
      {
        for (int index16 = num3 + 1; index16 < index15; ++index16)
        {
          if (glyphInfoList[index16].Position < 14)
            glyphInfoList[index16].Position = glyphInfoList[index15].Position;
        }
        num3 = index15;
      }
      else if (glyphInfoList[index15].Group == 7)
        num3 = index15;
    }
    glyphInfoList.DoOrder();
    int index17 = num1;
    for (int index18 = index2; index18 < num1; ++index18)
    {
      if (glyphInfoList[index18].Position == 4)
      {
        index17 = index18;
        break;
      }
    }
    int[] indicMask = this.GetIndicMask();
    for (int index19 = index2; index19 < num1 && glyphInfoList[index19].Position == 1; ++index19)
      glyphInfoList[index19].Mask |= indicMask[2];
    int num4 = indicMask[7];
    if (!oldScript && indicScript.BlwfMode == 0)
      num4 |= indicMask[5];
    for (int index20 = index2; index20 < index17; ++index20)
      glyphInfoList[index20].Mask |= num4;
    int num5 = 0;
    if (index17 < num1)
      glyphInfoList[index17].Mask |= num5;
    int num6 = indicMask[5] | indicMask[6] | indicMask[8];
    for (int index21 = index17 + 1; index21 < num1; ++index21)
      glyphInfoList[index21].Mask |= num6;
    if (oldScript && scriptTag == ScriptTags.Devanagari)
    {
      for (int index22 = index2; index22 + 1 < index17; ++index22)
      {
        if (glyphInfoList[index22].Group == 16 /*0x10*/ && glyphInfoList[index22 + 1].Group == 4 && (index22 + 2 == index17 || glyphInfoList[index22 + 2].Group != 6))
        {
          glyphInfoList[index22].Mask |= indicMask[5];
          glyphInfoList[index22 + 1].Mask |= indicMask[5];
        }
      }
    }
    if (index17 + indicScript.Length >= num1)
      return;
    for (int index23 = index17 + 1; index23 + indicScript.Length - 1 < num1; ++index23)
    {
      List<OtfGlyphInfo> glyphInfo = new List<OtfGlyphInfo>();
      for (int index24 = 0; index24 < indicScript.Length; ++index24)
        glyphInfo.Add((OtfGlyphInfo) glyphInfoList[index23 + index24]);
      if (this.Replace(prebaseForm, (IList<OtfGlyphInfo>) glyphInfo, indicScript.Length))
      {
        for (int index25 = 0; index25 < indicScript.Length; ++index25)
          glyphInfoList[index23++].Mask |= indicMask[4];
        break;
      }
    }
  }

  internal bool IsCombined(int group) => this.IsPresent(group, 96L /*0x60*/);

  internal bool IsConsonant(int group) => this.IsPresent(group, 2147563526L);

  internal bool IsHalant(int group) => this.IsPresent(group, 16400L);

  internal bool IsPresent(int group, long flag) => (1L << group & flag) != 0L;

  internal void Reorder(
    IndicGlyphInfoList glyphInfoList,
    IndicScript iScript,
    ScriptTags scriptTag)
  {
    int num1 = 0;
    int count = glyphInfoList.Glyphs.Count;
    bool flag1 = true;
    int index1;
    for (index1 = num1; index1 < count; ++index1)
    {
      if (glyphInfoList[index1].Position >= 4)
      {
        if (num1 < index1 && glyphInfoList[index1].Position > 4)
        {
          --index1;
          break;
        }
        break;
      }
    }
    if (index1 == count && num1 < index1 && this.IsPresent(glyphInfoList[index1 - 1].Group, 64L /*0x40*/))
      --index1;
    if (index1 < count)
    {
      while (num1 < index1 && this.IsPresent(glyphInfoList[index1].Group, 16408L))
        --index1;
    }
    if (num1 + 1 < count && num1 < index1)
    {
      int index2 = index1 == count ? index1 - 2 : index1 - 1;
      if (scriptTag != ScriptTags.Malayalam && scriptTag != ScriptTags.Tamil)
      {
        while (index2 > num1 && !this.IsPresent(glyphInfoList[index2].Group, 16528L))
          --index2;
        if (this.IsHalant(glyphInfoList[index2].Group) && glyphInfoList[index2].Position != 2)
        {
          if (index2 + 1 < count && this.IsCombined(glyphInfoList[index2 + 1].Group))
            ++index2;
        }
        else
          index2 = num1;
      }
      if (num1 < index2 && glyphInfoList[index2].Position != 2)
      {
        for (int index3 = index2; index3 > num1; --index3)
        {
          if (glyphInfoList[index3 - 1].Position == 2)
          {
            int num2 = index3 - 1;
            if (num2 < index1 && index1 <= index2)
              --index1;
            OtfGlyphInfo glyphInfo = (OtfGlyphInfo) glyphInfoList[num2];
            glyphInfoList.Rearrange(num2, num2 + 1, index2 - num2);
            glyphInfoList.Set(index2, glyphInfo);
            --index2;
          }
        }
      }
    }
    if (num1 + 1 < count && glyphInfoList[num1].Position == 1 && glyphInfoList[num1].Group != 15)
    {
      int index4 = num1 + 1;
      int rephPosition = iScript.RephPosition;
      bool flag2 = false;
      bool flag3 = false;
      if (rephPosition == 12)
        flag2 = true;
      if (!flag2)
      {
        index4 = num1 + 1;
        while (index4 < index1 && !this.IsHalant(glyphInfoList[index4].Group))
          ++index4;
        if (index4 < index1 && this.IsHalant(glyphInfoList[index4].Group))
        {
          if (index4 + 1 < index1 && this.IsCombined(glyphInfoList[index4 + 1].Group))
            ++index4;
          flag3 = true;
        }
        if (!flag3 && rephPosition == 5)
        {
          index4 = index1;
          while (index4 + 1 < count && glyphInfoList[index4 + 1].Position <= 5)
            ++index4;
          if (index4 < count)
            flag3 = true;
        }
        if (!flag3 && rephPosition == 9)
        {
          index4 = index1;
          while (index4 + 1 < count && (1L << glyphInfoList[index4 + 1].Position & 22528L) == 0L)
            ++index4;
          if (index4 < count)
            flag3 = true;
        }
      }
      if (!flag3)
      {
        index4 = num1 + 1;
        while (index4 < index1 && !this.IsHalant(glyphInfoList[index4].Group))
          ++index4;
        if (index4 < index1 && this.IsHalant(glyphInfoList[index4].Group))
        {
          if (index4 + 1 < index1 && this.IsCombined(glyphInfoList[index4 + 1].Group))
            ++index4;
          flag3 = true;
        }
      }
      if (!flag3)
      {
        index4 = count - 1;
        while (index4 > num1 && glyphInfoList[index4].Position == 14)
          --index4;
      }
      IndicGlyphInfo glyphInfo = glyphInfoList[num1];
      glyphInfoList.Rearrange(num1, num1 + 1, index4 - num1);
      glyphInfoList.Set(index4, (OtfGlyphInfo) glyphInfo);
      if (num1 < index1 && index1 <= index4)
        --index1;
    }
    if (!flag1 || index1 + 1 >= count)
      return;
    int index5 = index1 + 1;
    int[] indicMask = this.GetIndicMask();
    for (; index5 < count; ++index5)
    {
      if ((glyphInfoList[index5].Mask & indicMask[4]) != 0)
      {
        if (!glyphInfoList[index5].Substitute || !(iScript.Length == 1 ^ glyphInfoList[index5].Ligate))
          break;
        int num3 = index1;
        if (scriptTag != ScriptTags.Malayalam && scriptTag != ScriptTags.Tamil)
        {
          while (num3 > num1 && !this.IsPresent(glyphInfoList[num3 - 1].Group, 16528L))
            --num3;
          if (num3 > num1 && glyphInfoList[num3 - 1].Group == 7)
          {
            for (int index6 = index1 + 1; index6 < index5; ++index6)
            {
              if (glyphInfoList[index6].Group == 7)
              {
                --num3;
                break;
              }
            }
          }
        }
        if (num3 > num1 && this.IsHalant(glyphInfoList[num3 - 1].Group) && num3 < count && this.IsCombined(glyphInfoList[num3].Group))
          ++num3;
        int index7 = index5;
        OtfGlyphInfo glyphInfo = (OtfGlyphInfo) glyphInfoList[index7];
        glyphInfoList.Rearrange(num3 + 1, num3, index7 - num3);
        glyphInfoList.Set(num3, glyphInfo);
        if (num3 > index1 || index1 >= index7)
          break;
        int num4 = index1 + 1;
        break;
      }
    }
  }

  private int[] GetIndicMask()
  {
    int[] indicMask = new int[21];
    for (int index = 0; index < indicMask.Length; ++index)
      indicMask[index] = 1 << index;
    return indicMask;
  }

  private bool Replace(
    IList<LookupTable> featureTables,
    IList<OtfGlyphInfo> glyphInfo,
    int glyphLength)
  {
    bool flag = false;
    if (featureTables != null)
    {
      OtfGlyphInfoList glyphInfoList = new OtfGlyphInfoList((glyphInfo as List<OtfGlyphInfo>).GetRange(0, glyphLength), 0, glyphLength);
      foreach (LookupTable featureTable in (IEnumerable<LookupTable>) featureTables)
      {
        if (featureTable.ReplaceGlyphs(glyphInfoList))
          flag = true;
      }
    }
    return flag;
  }

  private OtfGlyphInfo GetGlyph(UnicodeTrueTypeFont font, IndicScript indicScript)
  {
    TtfGlyphInfo glyph = font.TtfReader.GetGlyph((char) indicScript.InitialChar);
    return new OtfGlyphInfo(glyph.CharCode, glyph.Index, (float) glyph.Width);
  }

  private int GetPosition(
    OtfGlyphInfo glyphInfo,
    OtfGlyphInfo advancedGlyphInfo,
    IndicScript iScript,
    IList<LookupTable> belowBaseForm,
    IList<LookupTable> postBaseForm,
    IList<LookupTable> preBaseForm)
  {
    List<OtfGlyphInfo> glyphInfo1 = new List<OtfGlyphInfo>();
    glyphInfo1.Add(advancedGlyphInfo);
    glyphInfo1.Add(glyphInfo);
    glyphInfo1.Add(advancedGlyphInfo);
    if (this.Replace(belowBaseForm, (IList<OtfGlyphInfo>) glyphInfo1, 2) || this.Replace(belowBaseForm, (IList<OtfGlyphInfo>) glyphInfo1.GetRange(1, glyphInfo1.Count - 1), 2))
      return 8;
    return this.Replace(postBaseForm, (IList<OtfGlyphInfo>) glyphInfo1, 2) || this.Replace(postBaseForm, (IList<OtfGlyphInfo>) glyphInfo1.GetRange(1, glyphInfo1.Count - 1), 2) || iScript.Length == 2 && (this.Replace(preBaseForm, (IList<OtfGlyphInfo>) glyphInfo1, 2) || this.Replace(preBaseForm, (IList<OtfGlyphInfo>) glyphInfo1.GetRange(1, glyphInfo1.Count - 1), 2)) || iScript.Length == 1 && this.Replace(preBaseForm, (IList<OtfGlyphInfo>) glyphInfo1.GetRange(1, glyphInfo1.Count - 1), 1) ? 11 : 4;
  }

  private void Replace(
    OtfGlyphInfoList glyphList,
    int index,
    UnicodeTrueTypeFont font,
    int[] charCodes)
  {
    OtfTable gsub = (OtfTable) font?.TtfReader.GSUB;
    if (gsub != null)
      return;
    int[] glyphs = new int[charCodes.Length];
    for (int index1 = 0; index1 < charCodes.Length; ++index1)
    {
      TtfGlyphInfo glyph = font.TtfReader.GetGlyph(charCodes[index1]);
      OtfGlyphInfo otfGlyphInfo = new OtfGlyphInfo(glyph.CharCode, glyph.Index, (float) glyph.Width);
      if (otfGlyphInfo == null || otfGlyphInfo.Index <= 0)
        return;
      glyphs[index1] = otfGlyphInfo.Index;
    }
    glyphList.Index = index;
    glyphList.CombineAlternateGlyphs(gsub, glyphs);
  }
}
