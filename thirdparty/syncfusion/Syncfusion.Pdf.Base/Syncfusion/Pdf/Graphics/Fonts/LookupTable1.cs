// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.LookupTable1
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class LookupTable1 : LookupTable
{
  private Dictionary<int, GPOSValueRecord> m_records = new Dictionary<int, GPOSValueRecord>();

  internal LookupTable1(OtfTable table, int flag, int[] offsets)
    : base(table, flag, offsets)
  {
    this.ReadSubTables();
  }

  internal override bool ReplaceGlyph(OtfGlyphInfoList glyphList)
  {
    bool flag = false;
    if (glyphList.Index >= glyphList.End)
      flag = false;
    if (this.OpenTypeFontTable.GDEFTable.IsSkip(glyphList.Glyphs[glyphList.Index].Index, this.Flag))
    {
      ++glyphList.Index;
      flag = false;
    }
    int index = glyphList.Glyphs[glyphList.Index].Index;
    GPOSValueRecord gposValueRecord = (GPOSValueRecord) null;
    this.m_records.TryGetValue(index, out gposValueRecord);
    if (gposValueRecord != null)
    {
      OtfGlyphInfo glyph = new OtfGlyphInfo(glyphList.Glyphs[glyphList.Index]);
      glyphList.Set(glyphList.Index, glyph);
      flag = true;
    }
    ++glyphList.Index;
    return flag;
  }

  internal override void ReadSubTable(int offset)
  {
    this.OpenTypeFontTable.Reader.Seek((long) offset);
    int num1 = (int) this.OpenTypeFontTable.Reader.ReadInt16();
    int num2 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    int foramt = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    if (num1 == 1)
    {
      GPOSValueRecord gposValueRecord = this.ReadGposValueRecord(this.OpenTypeFontTable, foramt);
      foreach (int key in (IEnumerable<int>) this.OpenTypeFontTable.ReadFormat(offset + num2))
        this.m_records.Add(key, gposValueRecord);
    }
    if (num1 != 2)
      return;
    int num3 = (int) this.OpenTypeFontTable.Reader.ReadUInt16();
    IList<GPOSValueRecord> gposValueRecordList = (IList<GPOSValueRecord>) new List<GPOSValueRecord>();
    for (int index = 0; index < num3; ++index)
    {
      GPOSValueRecord gposValueRecord = this.ReadGposValueRecord(this.OpenTypeFontTable, foramt);
      gposValueRecordList.Add(gposValueRecord);
    }
    IList<int> intList = this.OpenTypeFontTable.ReadFormat(offset + num2);
    for (int index = 0; index < intList.Count; ++index)
      this.m_records.Add(intList[index], gposValueRecordList[index]);
  }
}
