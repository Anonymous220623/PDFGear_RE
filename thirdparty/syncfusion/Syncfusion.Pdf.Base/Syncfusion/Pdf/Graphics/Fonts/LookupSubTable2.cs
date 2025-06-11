// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.LookupSubTable2
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class LookupSubTable2 : LookupTable
{
  private IDictionary<int, int[]> m_records;

  internal LookupSubTable2(OtfTable table, int flag, int[] offsets)
    : base(table, flag, offsets)
  {
    this.m_records = (IDictionary<int, int[]>) new Dictionary<int, int[]>();
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
        int[] glyphs;
        this.m_records.TryGetValue(glyph.Index, out glyphs);
        if (glyphs != null && glyphs.Length > 0)
        {
          glyphList.CombineAlternateGlyphs(this.OpenTypeFontTable, glyphs);
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
    int num1 = (int) this.OpenTypeFontTable.Reader.ReadInt16();
    if (num1 != 1)
      throw new Exception("Bad Format: " + (object) num1);
    int num2 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int size1 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int[] numArray = this.OpenTypeFontTable.ReadUInt16(size1, offset);
    IList<int> intList = this.OpenTypeFontTable.ReadFormat(offset + num2);
    for (int index = 0; index < size1; ++index)
    {
      this.OpenTypeFontTable.Reader.Seek((long) numArray[index]);
      int size2 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
      this.m_records.Add(intList[index], this.OpenTypeFontTable.ReadUInt32(size2));
    }
  }

  internal override bool IsSubstitute(int index) => this.m_records.ContainsKey(index);
}
