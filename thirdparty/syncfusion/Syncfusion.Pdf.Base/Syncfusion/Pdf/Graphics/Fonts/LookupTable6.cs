// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.LookupTable6
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class LookupTable6 : LookupTable
{
  private readonly IList<GPOSRecordsCollection> m_records;

  internal LookupTable6(OtfTable table, int flag, int[] offsets)
    : base(table, flag, offsets)
  {
    this.m_records = (IList<GPOSRecordsCollection>) new List<GPOSRecordsCollection>();
    this.ReadSubTables();
  }

  internal override bool ReplaceGlyph(OtfGlyphInfoList glyphList)
  {
    bool flag1 = false;
    if (glyphList.Index >= glyphList.End)
      flag1 = false;
    else if (this.OpenTypeFontTable.GDEFTable.IsSkip(glyphList.Glyphs[glyphList.Index].Index, this.Flag))
    {
      ++glyphList.Index;
      flag1 = false;
    }
    else
    {
      GlyphInfoIndex glyphInfoIndex = (GlyphInfoIndex) null;
      foreach (GPOSRecordsCollection record1 in (IEnumerable<GPOSRecordsCollection>) this.m_records)
      {
        GPOSRecord gposRecord;
        record1.Records.TryGetValue(glyphList.Glyphs[glyphList.Index].Index, out gposRecord);
        if (gposRecord != null)
        {
          if (glyphInfoIndex == null)
          {
            glyphInfoIndex = new GlyphInfoIndex();
            glyphInfoIndex.Index = glyphList.Index;
            glyphInfoIndex.GlyphInfoList = glyphList;
            do
            {
              int index1 = glyphInfoIndex.Index;
              bool flag2 = false;
              glyphInfoIndex.MovePrevious(this.OpenTypeFontTable, this.Flag);
              if (glyphInfoIndex.Index != -1)
              {
                for (int index2 = glyphInfoIndex.Index; index2 < index1; ++index2)
                {
                  int num;
                  this.OpenTypeFontTable.GDEFTable.GlyphCdefTable.Records.TryGetValue(glyphList.Glyphs[index2].Index, out num);
                  if (num == 1)
                  {
                    flag2 = true;
                    break;
                  }
                }
              }
              if (flag2)
              {
                glyphInfoIndex.GlyphInfo = (OtfGlyphInfo) null;
                break;
              }
            }
            while (glyphInfoIndex.GlyphInfo != null && !record1.Collection.ContainsKey(glyphInfoIndex.GlyphInfo.Index));
            if (glyphInfoIndex.GlyphInfo == null)
              break;
          }
          GPOSValueRecord[] gposValueRecordArray;
          record1.Collection.TryGetValue(glyphInfoIndex.GlyphInfo.Index, out gposValueRecordArray);
          if (gposValueRecordArray != null)
          {
            int index = gposRecord.Index;
            GPOSValueRecord gposValueRecord = gposValueRecordArray[index];
            GPOSValueRecord record2 = gposRecord.Record;
            glyphList.Set(glyphList.Index, new OtfGlyphInfo(glyphList.Glyphs[glyphList.Index], -record2.XPlacement + gposValueRecord.XPlacement, -record2.YPlacement + gposValueRecord.YPlacement));
            flag1 = true;
            break;
          }
        }
      }
      ++glyphList.Index;
    }
    return flag1;
  }

  internal override void ReadSubTable(int offset)
  {
    this.OpenTypeFontTable.Reader.Seek((long) offset);
    int num = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int offset1 = (int) this.OpenTypeFontTable.Reader.ReadUInt16() + offset;
    int offset2 = (int) this.OpenTypeFontTable.Reader.ReadUInt16() + offset;
    int classCount = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int location1 = (int) this.OpenTypeFontTable.Reader.ReadUInt16() + offset;
    int location2 = (int) this.OpenTypeFontTable.Reader.ReadUInt16() + offset;
    IList<int> intList1 = this.OpenTypeFontTable.ReadFormat(offset1);
    IList<int> intList2 = this.OpenTypeFontTable.ReadFormat(offset2);
    IList<GPOSRecord> gposRecordList = this.ReadMark(this.OpenTypeFontTable, location1);
    GPOSRecordsCollection recordsCollection = new GPOSRecordsCollection();
    for (int index = 0; index < intList1.Count; ++index)
      recordsCollection.Records.Add(intList1[index], gposRecordList[index]);
    IList<GPOSValueRecord[]> gposValueRecordArrayList = this.ReadBaseArray(this.OpenTypeFontTable, classCount, location2);
    for (int index = 0; index < intList2.Count; ++index)
      recordsCollection.Collection.Add(intList2[index], gposValueRecordArrayList[index]);
    this.m_records.Add(recordsCollection);
  }
}
