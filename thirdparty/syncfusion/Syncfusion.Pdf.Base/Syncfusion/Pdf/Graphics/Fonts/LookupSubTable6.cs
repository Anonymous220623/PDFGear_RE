// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.LookupSubTable6
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class LookupSubTable6 : LookupSubTable5
{
  internal LookupSubTable6(OtfTable table, int flag, int[] offsets)
    : base(table, flag, offsets)
  {
  }

  internal override void ReadSubTableFormat1(int offset)
  {
    IDictionary<int, IList<SubsetTable>> substMap = (IDictionary<int, IList<SubsetTable>>) new Dictionary<int, IList<SubsetTable>>();
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
        int[] btGlyphs = this.OpenTypeFontTable.ReadUInt32((int) this.OpenTypeFontTable.Reader.ReadUInt16());
        int[] glyphs = this.OpenTypeFontTable.ReadUInt32((int) this.OpenTypeFontTable.Reader.ReadUInt16() - 1);
        int[] lookupGlyphs = this.OpenTypeFontTable.ReadUInt32((int) this.OpenTypeFontTable.Reader.ReadUInt16());
        LookupSubTableRecord[] records = this.OpenTypeFontTable.ReadSubstLookupRecords((int) this.OpenTypeFontTable.Reader.ReadUInt16());
        subsetTableList.Add((SubsetTable) new SubsetTableFormat(btGlyphs, glyphs, lookupGlyphs, records));
      }
      substMap.Add(intList[index1], subsetTableList);
    }
    this.m_records.Add((BaseTable) new LookupSubTable6Format(this.OpenTypeFontTable, this.Flag, substMap, LookupSubTableFormat.Format1));
  }

  internal override void ReadSubTableFormat2(int offset)
  {
    int num1 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int num2 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int num3 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int num4 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int num5 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int[] numArray1 = this.OpenTypeFontTable.ReadUInt16(num5, offset);
    LookupSubTable6Format subTable = new LookupSubTable6Format(this.OpenTypeFontTable, this.Flag, (ICollection<int>) new List<int>((IEnumerable<int>) this.OpenTypeFontTable.ReadFormat(offset + num1)), this.GetTable(this.OpenTypeFontTable.Reader, offset + num2), this.GetTable(this.OpenTypeFontTable.Reader, offset + num3), this.GetTable(this.OpenTypeFontTable.Reader, offset + num4), LookupSubTableFormat.Format2);
    IList<IList<SubsetTable>> subsetTableListList = (IList<IList<SubsetTable>>) new List<IList<SubsetTable>>(num5);
    for (int index1 = 0; index1 < num5; ++index1)
    {
      IList<SubsetTable> subsetTableList = (IList<SubsetTable>) null;
      if (numArray1[index1] != 0)
      {
        this.OpenTypeFontTable.Reader.Seek((long) numArray1[index1]);
        int num6 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
        int[] numArray2 = this.OpenTypeFontTable.ReadUInt16(num6, numArray1[index1]);
        subsetTableList = (IList<SubsetTable>) new List<SubsetTable>(num6);
        for (int index2 = 0; index2 < num6; ++index2)
        {
          this.OpenTypeFontTable.Reader.Seek((long) numArray2[index2]);
          int[] backtrackClassIds = this.OpenTypeFontTable.ReadUInt32((int) this.OpenTypeFontTable.Reader.ReadUInt16());
          int[] inputClassIds = this.OpenTypeFontTable.ReadUInt32((int) this.OpenTypeFontTable.Reader.ReadUInt16() - 1);
          int[] lookAheadClassIds = this.OpenTypeFontTable.ReadUInt32((int) this.OpenTypeFontTable.Reader.ReadUInt16());
          LookupSubTableRecord[] substLookupRecords = this.OpenTypeFontTable.ReadSubstLookupRecords((int) this.OpenTypeFontTable.Reader.ReadUInt16());
          SubsetTableFormat subsetTableFormat = new SubsetTableFormat(subTable, backtrackClassIds, inputClassIds, lookAheadClassIds, substLookupRecords);
          subsetTableList.Add((SubsetTable) subsetTableFormat);
        }
      }
      subsetTableListList.Add(subsetTableList);
    }
    subTable.SubSetTables = subsetTableListList;
    this.m_records.Add((BaseTable) subTable);
  }

  internal override void ReadSubTableFormat3(int offset)
  {
    int num1 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int[] offsets1 = this.OpenTypeFontTable.ReadUInt16(num1, offset);
    int num2 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int[] offsets2 = this.OpenTypeFontTable.ReadUInt16(num2, offset);
    int num3 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int[] offsets3 = this.OpenTypeFontTable.ReadUInt16(num3, offset);
    LookupSubTableRecord[] records = this.OpenTypeFontTable.ReadSubstLookupRecords((int) this.OpenTypeFontTable.Reader.ReadUInt16());
    IList<ICollection<int>> intsList1 = (IList<ICollection<int>>) new List<ICollection<int>>(num1);
    this.OpenTypeFontTable.ReadFormatGlyphIds(offsets1, intsList1);
    IList<ICollection<int>> intsList2 = (IList<ICollection<int>>) new List<ICollection<int>>(num2);
    this.OpenTypeFontTable.ReadFormatGlyphIds(offsets2, intsList2);
    IList<ICollection<int>> intsList3 = (IList<ICollection<int>>) new List<ICollection<int>>(num3);
    this.OpenTypeFontTable.ReadFormatGlyphIds(offsets3, intsList3);
    this.m_records.Add((BaseTable) new LookupSubTable6Format(this.OpenTypeFontTable, this.Flag, new SubsetTableFormat(intsList1, intsList2, intsList3, records), LookupSubTableFormat.Format3));
  }
}
