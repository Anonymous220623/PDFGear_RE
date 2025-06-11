// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.LookupSubTable3
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class LookupSubTable3 : LookupTable
{
  private IDictionary<int, int[]> m_records;

  internal LookupSubTable3(OtfTable table, int flag, int[] offsets)
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
        int[] numArray;
        this.m_records.TryGetValue(glyph.Index, out numArray);
        if (numArray != null)
        {
          glyphList.CombineAlternateGlyphs(this.OpenTypeFontTable, numArray[0]);
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
    int num2 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int size1 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int[][] numArray1 = new int[size1][];
    int[] numArray2 = this.OpenTypeFontTable.ReadUInt16(size1, offset);
    for (int index = 0; index < size1; ++index)
    {
      this.OpenTypeFontTable.Reader.Seek((long) numArray2[index]);
      int size2 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
      numArray1[index] = this.OpenTypeFontTable.ReadUInt32(size2);
    }
    IList<int> intList = this.OpenTypeFontTable.ReadFormat(offset + num2);
    for (int index = 0; index < size1; ++index)
      this.m_records.Add(intList[index], numArray1[index]);
  }

  internal override bool IsSubstitute(int index) => this.m_records.ContainsKey(index);
}
