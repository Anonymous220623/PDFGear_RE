// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.LookupSubTable5
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class LookupSubTable5 : LookupTable
{
  protected internal IList<BaseTable> m_records;

  protected internal LookupSubTable5(OtfTable table, int flag, int[] offsets)
    : base(table, flag, offsets)
  {
    this.m_records = (IList<BaseTable>) new List<BaseTable>();
    this.ReadSubTables();
  }

  internal override bool ReplaceGlyph(OtfGlyphInfoList glyphList)
  {
    bool flag = false;
    int start = glyphList.Start;
    int end1 = glyphList.End;
    int index1 = glyphList.Index;
    foreach (BaseTable record in (IEnumerable<BaseTable>) this.m_records)
    {
      SubsetTable table = record.GetTable(glyphList);
      if (table != null)
      {
        int end2 = glyphList.End;
        LookupSubTableRecord[] lookupRecord = table.LookupRecord;
        GlyphInfoIndex glyphInfoIndex = new GlyphInfoIndex();
        glyphInfoIndex.GlyphInfoList = glyphList;
        foreach (LookupSubTableRecord lookupSubTableRecord in lookupRecord)
        {
          glyphInfoIndex.Index = index1;
          for (int index2 = 0; index2 < lookupSubTableRecord.Index; ++index2)
            glyphInfoIndex.MoveNext(this.OpenTypeFontTable, this.Flag);
          glyphList.Index = glyphInfoIndex.Index;
          LookupTable lookupTable = (LookupTable) null;
          if (lookupSubTableRecord.LookupIndex >= 0 || lookupSubTableRecord.LookupIndex < this.OpenTypeFontTable.LookupList.Count)
            lookupTable = this.OpenTypeFontTable.LookupList[lookupSubTableRecord.LookupIndex];
          flag = lookupTable.ReplaceGlyph(glyphList) || flag;
        }
        glyphList.Index = glyphList.End;
        glyphList.Start = start;
        int num = end2 - glyphList.End;
        glyphList.End = end1 - num;
        return flag;
      }
    }
    ++glyphList.Index;
    return flag;
  }

  internal override void ReadSubTable(int offset)
  {
    this.OpenTypeFontTable.Reader.Seek((long) offset);
    int num = (int) this.OpenTypeFontTable.Reader.ReadInt16();
    switch (num)
    {
      case 1:
        this.ReadSubTableFormat1(offset);
        break;
      case 2:
        this.ReadSubTableFormat2(offset);
        break;
      case 3:
        this.ReadSubTableFormat3(offset);
        break;
      default:
        throw new ArgumentException("Bad Format: " + (object) num);
    }
  }

  internal virtual void ReadSubTableFormat1(int offset)
  {
    IDictionary<int, IList<SubsetTable>> records1 = (IDictionary<int, IList<SubsetTable>>) new Dictionary<int, IList<SubsetTable>>();
    int num1 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int size = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int[] numArray1 = this.OpenTypeFontTable.ReadUInt16(size, offset);
    IList<int> intList = this.OpenTypeFontTable.ReadFormat(offset + num1);
    for (int index1 = 0; index1 < size; ++index1)
    {
      this.OpenTypeFontTable.Reader.Seek((long) numArray1[index1]);
      int num2 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
      int[] numArray2 = this.OpenTypeFontTable.ReadUInt16(num2, numArray1[index1]);
      IList<SubsetTable> subsetTableList = (IList<SubsetTable>) new List<SubsetTable>(num2);
      for (int index2 = 0; index2 < num2; ++index2)
      {
        this.OpenTypeFontTable.Reader.Seek((long) numArray2[index2]);
        int num3 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
        int substCount = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
        int[] glyphs = this.OpenTypeFontTable.ReadUInt32(num3 - 1);
        LookupSubTableRecord[] records2 = this.OpenTypeFontTable.ReadSubstLookupRecords(substCount);
        subsetTableList.Add((SubsetTable) new SubsetTableFormat(glyphs, records2));
      }
      records1.Add(intList[index1], subsetTableList);
    }
    this.m_records.Add((BaseTable) new LookupSubTable5Format(this.OpenTypeFontTable, this.Flag, records1, LookupSubTableFormat.Format1));
  }

  internal virtual void ReadSubTableFormat2(int offset)
  {
    int num1 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int num2 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int num3 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int[] numArray1 = this.OpenTypeFontTable.ReadUInt16(num3, offset);
    LookupSubTable5Format subtable5 = new LookupSubTable5Format(this.OpenTypeFontTable, this.Flag, (ICollection<int>) new List<int>((IEnumerable<int>) this.OpenTypeFontTable.ReadFormat(offset + num1)), this.GetTable(this.OpenTypeFontTable.Reader, offset + num2), LookupSubTableFormat.Format2);
    IList<IList<SubsetTable>> subsetTableListList = (IList<IList<SubsetTable>>) new List<IList<SubsetTable>>(num3);
    for (int index1 = 0; index1 < num3; ++index1)
    {
      IList<SubsetTable> subsetTableList = (IList<SubsetTable>) null;
      if (numArray1[index1] != 0)
      {
        this.OpenTypeFontTable.Reader.Seek((long) numArray1[index1]);
        int num4 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
        int[] numArray2 = this.OpenTypeFontTable.ReadUInt16(num4, numArray1[index1]);
        subsetTableList = (IList<SubsetTable>) new List<SubsetTable>(num4);
        for (int index2 = 0; index2 < num4; ++index2)
        {
          this.OpenTypeFontTable.Reader.Seek((long) numArray2[index2]);
          int num5 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
          int substCount = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
          int[] glyphs = this.OpenTypeFontTable.ReadUInt32(num5 - 1);
          LookupSubTableRecord[] records = this.OpenTypeFontTable.ReadSubstLookupRecords(substCount);
          SubsetTable subsetTable = (SubsetTable) new SubsetTableFormat(subtable5, glyphs, records);
          subsetTableList.Add(subsetTable);
        }
      }
      subsetTableListList.Add(subsetTableList);
    }
    subtable5.SubsetTables = subsetTableListList;
    this.m_records.Add((BaseTable) subtable5);
  }

  internal virtual void ReadSubTableFormat3(int offset)
  {
    int num = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int substCount = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int[] offsets = this.OpenTypeFontTable.ReadUInt16(num, offset);
    LookupSubTableRecord[] records = this.OpenTypeFontTable.ReadSubstLookupRecords(substCount);
    IList<ICollection<int>> intsList = (IList<ICollection<int>>) new List<ICollection<int>>(num);
    this.OpenTypeFontTable.ReadFormatGlyphIds(offsets, intsList);
    this.m_records.Add((BaseTable) new LookupSubTable5Format(this.OpenTypeFontTable, this.Flag, new SubsetTableFormat(intsList, records), LookupSubTableFormat.Format3));
  }
}
