// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.LookupTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal abstract class LookupTable
{
  private int m_flag;
  private int[] m_offsets;
  private OtfTable m_opentypeFontTable;

  internal int Flag
  {
    get => this.m_flag;
    set => this.m_flag = value;
  }

  internal int[] Offsets
  {
    get => this.m_offsets;
    set => this.m_offsets = value;
  }

  internal OtfTable OpenTypeFontTable
  {
    get => this.m_opentypeFontTable;
    set => this.m_opentypeFontTable = value;
  }

  internal LookupTable(OtfTable otFontTable, int flag, int[] offsets)
  {
    this.Flag = flag;
    this.Offsets = offsets;
    this.OpenTypeFontTable = otFontTable;
  }

  internal GPOSValueRecord ReadGposValueRecord(OtfTable table, int foramt)
  {
    GPOSValueRecord gposValueRecord = new GPOSValueRecord();
    if ((foramt & 1) != 0)
      gposValueRecord.XPlacement = (int) table.Reader.ReadInt16() * 1000 / table.m_ttfReader.unitsPerEM;
    if ((foramt & 2) != 0)
      gposValueRecord.YPlacement = (int) table.Reader.ReadInt16() * 1000 / table.m_ttfReader.unitsPerEM;
    if ((foramt & 4) != 0)
      gposValueRecord.XAdvance = (int) table.Reader.ReadInt16() * 1000 / table.m_ttfReader.unitsPerEM;
    if ((foramt & 8) != 0)
      gposValueRecord.YAdvance = (int) table.Reader.ReadInt16() * 1000 / table.m_ttfReader.unitsPerEM;
    if ((foramt & 16 /*0x10*/) != 0)
      gposValueRecord.XPlaDevice = (int) table.Reader.ReadInt16() * 1000 / table.m_ttfReader.unitsPerEM;
    if ((foramt & 32 /*0x20*/) != 0)
      gposValueRecord.YPlaDevice = (int) table.Reader.ReadInt16() * 1000 / table.m_ttfReader.unitsPerEM;
    if ((foramt & 64 /*0x40*/) != 0)
      gposValueRecord.XAdvDevice = (int) table.Reader.ReadInt16() * 1000 / table.m_ttfReader.unitsPerEM;
    if ((foramt & 128 /*0x80*/) != 0)
      gposValueRecord.YAdvDevice = (int) table.Reader.ReadInt16() * 1000 / table.m_ttfReader.unitsPerEM;
    return gposValueRecord;
  }

  internal IList<GPOSRecord> ReadMark(OtfTable table, int location)
  {
    table.Reader.Seek((long) location);
    int length = (int) table.Reader.ReadUInt16();
    int[] numArray1 = new int[length];
    int[] numArray2 = new int[length];
    for (int index = 0; index < length; ++index)
    {
      numArray1[index] = (int) table.Reader.ReadUInt16();
      int num = (int) table.Reader.ReadInt16();
      numArray2[index] = location + num;
    }
    IList<GPOSRecord> gposRecordList = (IList<GPOSRecord>) new List<GPOSRecord>();
    for (int index = 0; index < length; ++index)
      gposRecordList.Add(new GPOSRecord()
      {
        Index = numArray1[index],
        Record = this.ReadGposValueRecords(table, numArray2[index])
      });
    return gposRecordList;
  }

  internal GPOSValueRecord ReadGposValueRecords(OtfTable table, int location)
  {
    if (location == 0)
      return (GPOSValueRecord) null;
    table.Reader.Seek((long) location);
    int num = (int) table.Reader.ReadUInt16();
    return new GPOSValueRecord()
    {
      XPlacement = (int) table.Reader.ReadInt16() * 1000 / table.m_ttfReader.unitsPerEM,
      YPlacement = (int) table.Reader.ReadInt16() * 1000 / table.m_ttfReader.unitsPerEM
    };
  }

  internal IList<GPOSValueRecord[]> ReadBaseArray(OtfTable table, int classCount, int location)
  {
    IList<GPOSValueRecord[]> gposValueRecordArrayList = (IList<GPOSValueRecord[]>) new List<GPOSValueRecord[]>();
    table.Reader.Seek((long) location);
    int num = (int) table.Reader.ReadUInt16();
    int[] locations = table.ReadUInt16(num * classCount, location);
    int left = 0;
    for (int index = 0; index < num; ++index)
    {
      gposValueRecordArrayList.Add(this.ReadAnchorArray(table, locations, left, left + classCount));
      left += classCount;
    }
    return gposValueRecordArrayList;
  }

  internal GPOSValueRecord[] ReadAnchorArray(
    OtfTable tableReader,
    int[] locations,
    int left,
    int right)
  {
    GPOSValueRecord[] gposValueRecordArray = new GPOSValueRecord[right - left];
    for (int index = left; index < right; ++index)
      gposValueRecordArray[index - left] = this.ReadGposValueRecords(tableReader, locations[index]);
    return gposValueRecordArray;
  }

  internal IList<IList<GPOSValueRecord[]>> ReadLigatureArray(
    OtfTable tableReader,
    int classCount,
    int location)
  {
    IList<IList<GPOSValueRecord[]>> gposValueRecordArrayListList = (IList<IList<GPOSValueRecord[]>>) new List<IList<GPOSValueRecord[]>>();
    tableReader.Reader.Seek((long) location);
    int size = (int) tableReader.Reader.ReadUInt16();
    int[] numArray = tableReader.ReadUInt16(size, location);
    for (int index1 = 0; index1 < size; ++index1)
    {
      int num1 = numArray[index1];
      IList<GPOSValueRecord[]> gposValueRecordArrayList = (IList<GPOSValueRecord[]>) new List<GPOSValueRecord[]>();
      tableReader.Reader.Seek((long) num1);
      int num2 = (int) tableReader.Reader.ReadUInt16();
      int[] locations = tableReader.ReadUInt16(classCount * num2, num1);
      int left = 0;
      for (int index2 = 0; index2 < num2; ++index2)
      {
        gposValueRecordArrayList.Add(this.ReadAnchorArray(tableReader, locations, left, left + classCount));
        left += classCount;
      }
      gposValueRecordArrayListList.Add(gposValueRecordArrayList);
    }
    return gposValueRecordArrayListList;
  }

  internal CDEFTable GetTable(BigEndianReader reader, int offset)
  {
    CDEFTable table = new CDEFTable();
    reader.Seek((long) offset);
    switch (reader.ReadUInt16())
    {
      case 1:
        int num1 = (int) reader.ReadUInt16();
        int num2 = (int) reader.ReadInt16();
        int num3 = num1 + num2;
        for (int key = num1; key < num3; ++key)
        {
          int num4 = (int) reader.ReadInt16();
          table.Records.Add(key, num4);
        }
        break;
      case 2:
        int num5 = (int) reader.ReadUInt16();
        for (int index = 0; index < num5; ++index)
        {
          int num6 = (int) reader.ReadUInt16();
          int num7 = (int) reader.ReadUInt16();
          int num8 = (int) reader.ReadUInt16();
          for (int key = num6; key <= num7; ++key)
          {
            if (table.Records.ContainsKey(key))
              table.Records[key] = num8;
            else
              table.Records.Add(key, num8);
          }
        }
        break;
      default:
        table = (CDEFTable) null;
        break;
    }
    return table;
  }

  internal abstract bool ReplaceGlyph(OtfGlyphInfoList glyphList);

  internal abstract void ReadSubTable(int offset);

  internal virtual bool ReplaceGlyphs(OtfGlyphInfoList glyphInfoList)
  {
    bool flag = false;
    glyphInfoList.Index = glyphInfoList.Start;
    while (glyphInfoList.Index < glyphInfoList.End && glyphInfoList.Index >= glyphInfoList.Start)
      flag = this.ReplaceGlyph(glyphInfoList) || flag;
    return flag;
  }

  internal virtual bool IsSubstitute(int index) => false;

  internal virtual void ReadSubTables()
  {
    foreach (int offset in this.Offsets)
      this.ReadSubTable(offset);
  }
}
