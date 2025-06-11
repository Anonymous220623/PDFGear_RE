// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.LookupSubTable1
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class LookupSubTable1 : LookupTable
{
  private Dictionary<int, int> m_records;

  internal LookupSubTable1(OtfTable table, int flag, int[] offsets)
    : base(table, flag, offsets)
  {
    this.m_records = new Dictionary<int, int>();
    this.ReadSubTables();
  }

  internal override bool ReplaceGlyph(OtfGlyphInfoList glyphList)
  {
    bool flag = false;
    if (glyphList.Index >= glyphList.End)
    {
      flag = false;
    }
    else
    {
      OtfGlyphInfo glyph = glyphList.Glyphs[glyphList.Index];
      if (!this.OpenTypeFontTable.GDEFTable.IsSkip(glyph.Index, this.Flag))
      {
        int glyphIndex;
        this.m_records.TryGetValue(glyph.Index, out glyphIndex);
        if (glyphIndex != 0)
        {
          glyphList.CombineAlternateGlyphs(this.OpenTypeFontTable, glyphIndex);
          flag = true;
        }
      }
      ++glyphList.Index;
    }
    return flag;
  }

  internal override void ReadSubTable(int offset)
  {
    this.OpenTypeFontTable.Reader.Seek((long) offset);
    switch (this.OpenTypeFontTable.Reader.ReadInt16())
    {
      case 1:
        int num1 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
        int num2 = (int) this.OpenTypeFontTable.Reader.ReadInt16();
        using (IEnumerator<int> enumerator = this.OpenTypeFontTable.ReadFormat(offset + num1).GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            int current = enumerator.Current;
            int num3 = current + num2;
            this.m_records.Add(current, num3);
          }
          break;
        }
      case 2:
        int num4 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
        int length = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
        int[] numArray = new int[length];
        for (int index = 0; index < length; ++index)
          numArray[index] = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
        IList<int> intList = this.OpenTypeFontTable.ReadFormat(offset + num4);
        for (int index = 0; index < length; ++index)
          this.m_records.Add(intList[index], numArray[index]);
        break;
      default:
        throw new Exception("Bad format");
    }
  }

  internal override bool IsSubstitute(int index) => this.m_records.ContainsKey(index);
}
