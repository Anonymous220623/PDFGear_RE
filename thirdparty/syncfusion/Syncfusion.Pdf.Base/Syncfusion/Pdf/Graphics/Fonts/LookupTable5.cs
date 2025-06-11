// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.LookupTable5
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class LookupTable5 : LookupTable
{
  private readonly IList<GPOSRecordsCollection> m_records;

  internal LookupTable5(OtfTable table, int flag, int[] offsets)
    : base(table, flag, offsets)
  {
    this.m_records = (IList<GPOSRecordsCollection>) new List<GPOSRecordsCollection>();
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
              glyphInfoIndex.MovePrevious(this.OpenTypeFontTable, this.Flag);
            }
            while (glyphInfoIndex.GlyphInfo != null && record1.Records.ContainsKey(glyphInfoIndex.GlyphInfo.Index));
            if (glyphInfoIndex.GlyphInfo == null)
              break;
          }
          IList<GPOSValueRecord[]> gposValueRecordArrayList;
          record1.Ligatures.TryGetValue(glyphInfoIndex.GlyphInfo.Index, out gposValueRecordArrayList);
          if (gposValueRecordArrayList != null)
          {
            int index1 = gposRecord.Index;
            for (int index2 = gposValueRecordArrayList.Count - 1; index2 >= 0; --index2)
            {
              if (gposValueRecordArrayList[index2][index1] != null)
              {
                GPOSValueRecord gposValueRecord = gposValueRecordArrayList[index2][index1];
                GPOSValueRecord record2 = gposRecord.Record;
                glyphList.Glyphs[glyphList.Index] = new OtfGlyphInfo(glyphList.Glyphs[glyphList.Index], gposValueRecord.XPlacement - record2.XPlacement, gposValueRecord.YPlacement - record2.YPlacement);
                if (glyphList.Text != null && glyphList.Text.Count > glyphList.Index)
                  glyphList.Text.Insert(glyphList.Index, (string) null);
                flag = true;
                break;
              }
            }
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
    IList<IList<GPOSValueRecord[]>> gposValueRecordArrayListList = this.ReadLigatureArray(this.OpenTypeFontTable, classCount, location2);
    for (int index = 0; index < intList2.Count; ++index)
      recordsCollection.Ligatures.Add(intList2[index], gposValueRecordArrayListList[index]);
    this.m_records.Add(recordsCollection);
  }
}
