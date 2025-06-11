// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.LookupTable4
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class LookupTable4 : LookupTable
{
  private readonly IList<GPOSRecordsCollection> m_recordCollection;

  internal LookupTable4(OtfTable table, int flag, int[] offset)
    : base(table, flag, offset)
  {
    this.m_recordCollection = (IList<GPOSRecordsCollection>) new List<GPOSRecordsCollection>();
    this.ReadSubTables();
  }

  internal override bool ReplaceGlyph(OtfGlyphInfoList glyphList)
  {
    bool flag = false;
    if (glyphList.Index >= glyphList.End)
      flag = false;
    else if (this.OpenTypeFontTable.GDEFTable.IsSkip(glyphList.Glyphs[glyphList.Index].Index, this.Flag))
    {
      ++glyphList.Index;
      flag = false;
    }
    else
    {
      GlyphInfoIndex glyphInfoIndex = (GlyphInfoIndex) null;
      foreach (GPOSRecordsCollection record1 in (IEnumerable<GPOSRecordsCollection>) this.m_recordCollection)
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
              glyphInfoIndex.MovePrevious(this.OpenTypeFontTable, this.Flag);
            }
            while (glyphInfoIndex.GlyphInfo != null && record1.Records.ContainsKey(glyphInfoIndex.GlyphInfo.Index));
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
            flag = true;
            break;
          }
        }
      }
      ++glyphList.Index;
    }
    return flag;
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
    this.m_recordCollection.Add(recordsCollection);
  }
}
