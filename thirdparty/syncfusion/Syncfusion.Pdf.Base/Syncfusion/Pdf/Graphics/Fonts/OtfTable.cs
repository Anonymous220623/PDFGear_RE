// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.OtfTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal abstract class OtfTable
{
  private BigEndianReader m_reader;
  private int m_offset;
  private GDEFTable m_gdefTable;
  private IList<LookupTable> m_lookupList;
  private ScriptRecordReader m_otScript;
  private FeatureRecordReader m_otFeature;
  internal TtfReader m_ttfReader;

  internal abstract LookupTable ReadLookupTable(
    int lookupType,
    int lookupFlag,
    int[] subTableLocations);

  internal FeatureRecordReader OTFeature
  {
    get => this.m_otFeature;
    set => this.m_otFeature = value;
  }

  internal IList<LookupTable> LookupList
  {
    get => this.m_lookupList;
    set => this.m_lookupList = value;
  }

  internal BigEndianReader Reader
  {
    get => this.m_reader;
    set => this.m_reader = value;
  }

  internal int Offset
  {
    get => this.m_offset;
    set => this.m_offset = value;
  }

  internal GDEFTable GDEFTable
  {
    get => this.m_gdefTable;
    set => this.m_gdefTable = value;
  }

  protected internal OtfTable(
    BigEndianReader reader,
    int offset,
    GDEFTable gdef,
    TtfReader ttfReader)
  {
    this.m_reader = reader;
    this.m_offset = offset;
    this.m_gdefTable = gdef;
    this.m_ttfReader = ttfReader;
  }

  internal virtual OtfGlyphInfo GetGlyph(int index)
  {
    TtfGlyphInfo ttfGlyphInfo = this.m_ttfReader.ReadGlyph(index, true);
    return new OtfGlyphInfo(ttfGlyphInfo.CharCode, ttfGlyphInfo.Index, (float) ttfGlyphInfo.Width);
  }

  internal virtual IList<LookupTable> GetLookups(FeatureRecord[] features)
  {
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    foreach (FeatureRecord feature in features)
    {
      foreach (int index in feature.Indexes)
        dictionary[index] = 1;
    }
    IList<LookupTable> lookups = (IList<LookupTable>) new List<LookupTable>();
    foreach (KeyValuePair<int, int> keyValuePair in dictionary)
      lookups.Add(this.m_lookupList[keyValuePair.Key]);
    return lookups;
  }

  internal virtual Syncfusion.Pdf.Graphics.Fonts.LanguageRecord LanguageRecord(string tag)
  {
    Syncfusion.Pdf.Graphics.Fonts.LanguageRecord languageRecord = (Syncfusion.Pdf.Graphics.Fonts.LanguageRecord) null;
    if (tag != null)
    {
      foreach (ScriptRecord record in (IEnumerable<ScriptRecord>) this.m_otScript.Records)
      {
        if (tag.Equals(record.ScriptTag))
        {
          languageRecord = record.Language;
          break;
        }
      }
    }
    return languageRecord;
  }

  internal int[] ReadUInt16(int size, int location)
  {
    int[] numArray = new int[size];
    for (int index = 0; index < size; ++index)
    {
      int num = (int) this.Reader.ReadUInt16();
      numArray[index] = num == 0 ? num : num + location;
    }
    return numArray;
  }

  internal int[] ReadUInt32(int size) => this.ReadUInt16(size, 0);

  internal virtual void ReadFormatGlyphIds(int[] offsets, IList<ICollection<int>> formatGlyphs)
  {
    foreach (int offset in offsets)
      formatGlyphs.Add((ICollection<int>) new List<int>((IEnumerable<int>) this.ReadFormat(offset)));
  }

  internal IList<int> ReadFormat(int offset)
  {
    this.Reader.Seek((long) offset);
    int num1 = (int) this.Reader.ReadInt16();
    IList<int> glyphIds;
    switch (num1)
    {
      case 1:
        int capacity = (int) this.Reader.ReadInt16();
        glyphIds = (IList<int>) new List<int>(capacity);
        for (int index = 0; index < capacity; ++index)
        {
          int num2 = (int) this.Reader.ReadInt16();
          glyphIds.Add(num2);
        }
        break;
      case 2:
        int num3 = (int) this.Reader.ReadInt16();
        glyphIds = (IList<int>) new List<int>();
        for (int index = 0; index < num3; ++index)
          this.ReadRecord(this.Reader, glyphIds);
        break;
      default:
        throw new Exception($"Invalid format: {num1}");
    }
    return glyphIds;
  }

  private void ReadRecord(BigEndianReader reader, IList<int> glyphIds)
  {
    int num1 = (int) reader.ReadInt16();
    int num2 = (int) reader.ReadInt16();
    int num3 = (int) reader.ReadInt16();
    for (int index = num1; index <= num2; ++index)
      glyphIds.Add(index);
  }

  internal virtual LookupSubTableRecord[] ReadSubstLookupRecords(int substCount)
  {
    LookupSubTableRecord[] lookupSubTableRecordArray = new LookupSubTableRecord[substCount];
    for (int index = 0; index < substCount; ++index)
      lookupSubTableRecordArray[index] = new LookupSubTableRecord()
      {
        Index = (int) this.Reader.ReadUInt16(),
        LookupIndex = (int) this.Reader.ReadUInt16()
      };
    return lookupSubTableRecordArray;
  }

  internal virtual FeatureTag[] ReadFeatureTag(int offset)
  {
    int length = (int) this.Reader.ReadUInt16();
    FeatureTag[] featureTagArray = new FeatureTag[length];
    for (int index = 0; index < length; ++index)
      featureTagArray[index] = new FeatureTag()
      {
        TagName = this.Reader.ReadUtf8String(4),
        Offset = (int) this.Reader.ReadUInt16() + offset
      };
    return featureTagArray;
  }

  internal void Initialize()
  {
    try
    {
      this.Reader.Seek((long) this.Offset);
      this.Reader.ReadInt32();
      int num1 = (int) this.Reader.ReadUInt16();
      int num2 = (int) this.Reader.ReadUInt16();
      int num3 = (int) this.Reader.ReadUInt16();
      this.m_otScript = new ScriptRecordReader(this, this.Offset + num1);
      this.m_otFeature = new FeatureRecordReader(this, this.Offset + num2);
      this.ReadLookupTables(this.Offset + num3);
    }
    catch (IOException ex)
    {
      throw new Exception("Can't read the font file.", (Exception) ex);
    }
  }

  private void ReadLookupTables(int offset)
  {
    this.m_lookupList = (IList<LookupTable>) new List<LookupTable>();
    this.Reader.Seek((long) offset);
    foreach (int num in this.ReadUInt16((int) this.Reader.ReadUInt16(), offset))
    {
      this.Reader.Seek((long) num);
      this.m_lookupList.Add(this.ReadLookupTable((int) this.Reader.ReadUInt16(), (int) this.Reader.ReadUInt16(), this.ReadUInt16((int) this.Reader.ReadUInt16(), num)));
    }
  }
}
