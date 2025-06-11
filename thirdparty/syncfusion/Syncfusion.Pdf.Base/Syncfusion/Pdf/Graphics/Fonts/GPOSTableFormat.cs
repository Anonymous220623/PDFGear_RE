// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.GPOSTableFormat
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class GPOSTableFormat : LookupTable
{
  private Dictionary<int, Dictionary<int, GPOSTableFormatRecord>> m_records = new Dictionary<int, Dictionary<int, GPOSTableFormatRecord>>();
  private CDEFTable m_cdefTable1;
  private CDEFTable m_cdefTable2;
  private IList<int> m_coverageTable;
  private IDictionary<int, GPOSTableFormatRecord[]> m_gposTableFormatRecord = (IDictionary<int, GPOSTableFormatRecord[]>) new Dictionary<int, GPOSTableFormatRecord[]>();
  private GPOSTableType m_format;

  internal GPOSTableFormat(OtfTable table, int flag, int offset, GPOSTableType format)
    : base(table, flag, (int[]) null)
  {
    this.m_format = format;
    this.Initialize(offset);
  }

  internal override bool ReplaceGlyph(OtfGlyphInfoList glyphList)
  {
    return this.m_format == GPOSTableType.Format1 ? this.ReplaceFormat1Glyphs(glyphList) : this.ReplaceFormat2Glyphs(glyphList);
  }

  private bool ReplaceFormat1Glyphs(OtfGlyphInfoList glyphList)
  {
    bool flag = false;
    if (glyphList.Index >= glyphList.End || glyphList.Index < glyphList.Start)
    {
      flag = false;
    }
    else
    {
      OtfGlyphInfo glyph = glyphList.Glyphs[glyphList.Index];
      Dictionary<int, GPOSTableFormatRecord> dictionary;
      this.m_records.TryGetValue(glyph.Index, out dictionary);
      if (dictionary != null)
      {
        GlyphInfoIndex glyphInfoIndex = new GlyphInfoIndex();
        glyphInfoIndex.GlyphInfoList = glyphList;
        glyphInfoIndex.Index = glyphList.Index;
        glyphInfoIndex.MoveNext(this.OpenTypeFontTable, this.Flag);
        if (glyphInfoIndex.GlyphInfo != null)
        {
          GPOSTableFormatRecord tableFormatRecord;
          dictionary.TryGetValue(glyphInfoIndex.GlyphInfo.Index, out tableFormatRecord);
          if (tableFormatRecord != null)
          {
            OtfGlyphInfo glyphInfo = glyphInfoIndex.GlyphInfo;
            glyphList.Set(glyphList.Index, new OtfGlyphInfo(glyph, 0, 0));
            glyphList.Set(glyphInfoIndex.Index, new OtfGlyphInfo(glyphInfo, 0, 0));
            glyphList.Index = glyphInfoIndex.Index;
            flag = true;
          }
        }
      }
    }
    return flag;
  }

  private bool ReplaceFormat2Glyphs(OtfGlyphInfoList line)
  {
    if (line.Index >= line.End || line.Index < line.Start)
      return false;
    OtfGlyphInfo glyph = line.Glyphs[line.Index];
    if (!this.m_coverageTable.Contains(glyph.Index))
      return false;
    GPOSTableFormatRecord[] tableFormatRecordArray;
    this.m_gposTableFormatRecord.TryGetValue(this.m_cdefTable1.GetValue(glyph.Index), out tableFormatRecordArray);
    if (tableFormatRecordArray == null)
      return false;
    GlyphInfoIndex glyphInfoIndex = new GlyphInfoIndex();
    glyphInfoIndex.GlyphInfoList = line;
    glyphInfoIndex.Index = line.Index;
    glyphInfoIndex.MoveNext(this.OpenTypeFontTable, this.Flag);
    if (glyphInfoIndex.GlyphInfo == null)
      return false;
    OtfGlyphInfo glyphInfo = glyphInfoIndex.GlyphInfo;
    int index = this.m_cdefTable2.GetValue(glyphInfo.Index);
    if (index >= tableFormatRecordArray.Length)
      return false;
    GPOSTableFormatRecord tableFormatRecord = tableFormatRecordArray[index];
    line.Set(line.Index, new OtfGlyphInfo(glyph, 0, 0));
    line.Set(glyphInfoIndex.Index, new OtfGlyphInfo(glyphInfo, 0, 0));
    line.Index = glyphInfoIndex.Index;
    return true;
  }

  internal override void ReadSubTable(int offset)
  {
  }

  private void Initialize(int offset)
  {
    switch (this.m_format)
    {
      case GPOSTableType.Format1:
        this.ReadFormat1(offset);
        break;
      case GPOSTableType.Format2:
        this.ReadFormat2(offset);
        break;
    }
  }

  private void ReadFormat1(int offset)
  {
    int offset1 = (int) this.OpenTypeFontTable.Reader.ReadUInt16() + offset;
    int foramt1 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int foramt2 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int size = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int[] numArray = this.OpenTypeFontTable.ReadUInt16(size, offset);
    IList<int> intList = this.OpenTypeFontTable.ReadFormat(offset1);
    for (int index1 = 0; index1 < size; ++index1)
    {
      this.OpenTypeFontTable.Reader.Seek((long) numArray[index1]);
      Dictionary<int, GPOSTableFormatRecord> dictionary = new Dictionary<int, GPOSTableFormatRecord>();
      this.m_records.Add(intList[index1], dictionary);
      int num = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
      for (int index2 = 0; index2 < num; ++index2)
      {
        int key = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
        dictionary[key] = new GPOSTableFormatRecord()
        {
          Record1 = this.ReadGposValueRecord(this.OpenTypeFontTable, foramt1),
          Record2 = this.ReadGposValueRecord(this.OpenTypeFontTable, foramt2)
        };
      }
    }
  }

  private void ReadFormat2(int offset)
  {
    int offset1 = (int) this.OpenTypeFontTable.Reader.ReadUInt16() + offset;
    int foramt1 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int foramt2 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int offset2 = (int) this.OpenTypeFontTable.Reader.ReadUInt16() + offset;
    int offset3 = (int) this.OpenTypeFontTable.Reader.ReadUInt16() + offset;
    int num = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int length = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    for (int key = 0; key < num; ++key)
    {
      GPOSTableFormatRecord[] tableFormatRecordArray = new GPOSTableFormatRecord[length];
      this.m_gposTableFormatRecord.Add(key, tableFormatRecordArray);
      for (int index = 0; index < length; ++index)
        tableFormatRecordArray[index] = new GPOSTableFormatRecord()
        {
          Record1 = this.ReadGposValueRecord(this.OpenTypeFontTable, foramt1),
          Record2 = this.ReadGposValueRecord(this.OpenTypeFontTable, foramt2)
        };
    }
    this.m_coverageTable = (IList<int>) new List<int>((IEnumerable<int>) this.OpenTypeFontTable.ReadFormat(offset1));
    this.m_cdefTable1 = this.GetTable(this.OpenTypeFontTable.Reader, offset2);
    this.m_cdefTable2 = this.GetTable(this.OpenTypeFontTable.Reader, offset3);
  }
}
