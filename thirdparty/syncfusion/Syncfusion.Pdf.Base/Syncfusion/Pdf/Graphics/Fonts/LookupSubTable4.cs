// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.LookupSubTable4
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class LookupSubTable4 : LookupTable
{
  private IDictionary<int, IList<int[]>> m_records;

  internal LookupSubTable4(OtfTable table, int flag, int[] offsets)
    : base(table, flag, offsets)
  {
    this.m_records = (IDictionary<int, IList<int[]>>) new Dictionary<int, IList<int[]>>();
    this.ReadSubTables();
  }

  internal override bool ReplaceGlyph(OtfGlyphInfoList glyphList)
  {
    bool flag1 = false;
    if (glyphList.Index >= glyphList.End)
    {
      flag1 = false;
    }
    else
    {
      OtfGlyphInfo glyph = glyphList.Glyphs[glyphList.Index];
      bool flag2 = false;
      if (this.m_records.ContainsKey(glyph.Index) && !this.OpenTypeFontTable.GDEFTable.IsSkip(glyph.Index, this.Flag))
      {
        GlyphInfoIndex glyphInfoIndex = new GlyphInfoIndex();
        glyphInfoIndex.GlyphInfoList = glyphList;
        IList<int[]> numArrayList;
        this.m_records.TryGetValue(glyph.Index, out numArrayList);
        foreach (int[] numArray in (IEnumerable<int[]>) numArrayList)
        {
          flag2 = true;
          glyphInfoIndex.Index = glyphList.Index;
          for (int index = 1; index < numArray.Length; ++index)
          {
            glyphInfoIndex.MoveNext(this.OpenTypeFontTable, this.Flag);
            if (glyphInfoIndex.GlyphInfo == null || glyphInfoIndex.GlyphInfo.Index != numArray[index])
            {
              flag2 = false;
              break;
            }
          }
          if (flag2)
          {
            glyphList.CombineAlternateGlyphs(this.OpenTypeFontTable, this.Flag, numArray.Length - 1, numArray[0]);
            break;
          }
        }
      }
      if (flag2)
        flag1 = true;
      ++glyphList.Index;
    }
    return flag1;
  }

  internal override void ReadSubTable(int offset)
  {
    this.OpenTypeFontTable.Reader.Seek((long) offset);
    int num1 = (int) this.OpenTypeFontTable.Reader.ReadInt16();
    int offset1 = (int) this.OpenTypeFontTable.Reader.ReadUInt16() + offset;
    int length1 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int[] numArray1 = new int[length1];
    for (int index = 0; index < length1; ++index)
      numArray1[index] = (int) this.OpenTypeFontTable.Reader.ReadUInt16() + offset;
    IList<int> intList = this.OpenTypeFontTable.ReadFormat(offset1);
    for (int index1 = 0; index1 < length1; ++index1)
    {
      this.OpenTypeFontTable.Reader.Seek((long) numArray1[index1]);
      int capacity = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
      int[] numArray2 = new int[capacity];
      for (int index2 = 0; index2 < capacity; ++index2)
        numArray2[index2] = (int) this.OpenTypeFontTable.Reader.ReadUInt16() + numArray1[index1];
      IList<int[]> numArrayList = (IList<int[]>) new List<int[]>(capacity);
      for (int index3 = 0; index3 < capacity; ++index3)
      {
        this.OpenTypeFontTable.Reader.Seek((long) numArray2[index3]);
        int num2 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
        int length2 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
        int[] numArray3 = new int[length2];
        numArray3[0] = num2;
        for (int index4 = 1; index4 < length2; ++index4)
          numArray3[index4] = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
        numArrayList.Add(numArray3);
      }
      if (this.m_records.ContainsKey(intList[index1]))
        this.m_records[intList[index1]] = numArrayList;
      else
        this.m_records.Add(intList[index1], numArrayList);
    }
  }
}
