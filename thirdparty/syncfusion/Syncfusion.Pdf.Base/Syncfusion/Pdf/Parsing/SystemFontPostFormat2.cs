// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontPostFormat2
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontPostFormat2(SystemFontOpenTypeFontSourceBase fontSource) : SystemFontPost(fontSource)
{
  private Dictionary<string, ushort> glyphNames;

  private void CreateGlyphNamesMapping(ushort[] glyphNameIndex, string[] names)
  {
    this.glyphNames = new Dictionary<string, ushort>(glyphNameIndex.Length);
    for (int index1 = 0; index1 < glyphNameIndex.Length; ++index1)
    {
      ushort index2 = glyphNameIndex[index1];
      if ((int) index2 < SystemFontPost.macintoshStandardOrderNames.Length)
      {
        this.glyphNames[SystemFontPost.macintoshStandardOrderNames[(int) index2]] = (ushort) index1;
      }
      else
      {
        ushort index3 = (ushort) ((uint) index2 - (uint) SystemFontPost.macintoshStandardOrderNames.Length);
        this.glyphNames[names[(int) index3]] = (ushort) index1;
      }
    }
  }

  public override ushort GetGlyphId(string name)
  {
    ushort num;
    return this.glyphNames.TryGetValue(name, out num) ? num : (ushort) 0;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    base.Read(reader);
    ushort length1 = reader.ReadUShort();
    ushort[] glyphNameIndex = new ushort[(int) length1];
    int num = SystemFontPost.macintoshStandardOrderGlyphIds.Count - 1;
    for (int index = 0; index < (int) length1; ++index)
    {
      glyphNameIndex[index] = reader.ReadUShort();
      if ((int) glyphNameIndex[index] > num)
        num = (int) glyphNameIndex[index];
    }
    int length2 = num - SystemFontPost.macintoshStandardOrderGlyphIds.Count + 1;
    string[] names = new string[length2];
    for (int index = 0; index < length2; ++index)
      names[index] = reader.ReadString();
    this.CreateGlyphNamesMapping(glyphNameIndex, names);
  }
}
