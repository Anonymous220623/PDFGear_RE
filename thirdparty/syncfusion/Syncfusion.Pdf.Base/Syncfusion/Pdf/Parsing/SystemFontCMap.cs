// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontCMap
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontCMap(SystemFontOpenTypeFontSourceBase fontFile) : 
  SystemFontTrueTypeTableBase(fontFile)
{
  private const ushort UNICODE_PLATFORM_ID = 0;
  private const ushort WINDOWS_PLATFORM_ID = 3;
  private const ushort DEFAULT_SEMANTIC_ID = 0;
  private const ushort SYMBOL_ENCODING_ID = 0;
  private const ushort UNICODE_ENCODING_ID = 1;
  internal const ushort MISSING_GLYPH_ID = 0;
  private SystemFontEncodingRecord[] encodings;
  private Dictionary<SystemFontEncodingRecord, SystemFontCMapTable> tables;
  private bool isInitialized;
  private SystemFontCMapTable encoding;

  internal override uint Tag => SystemFontTags.CMAP_TABLE;

  private void Initialize()
  {
    if (this.isInitialized)
      return;
    this.encoding = this.GetCMapTable((ushort) 3, (ushort) 1);
    this.isInitialized = true;
  }

  private SystemFontCMapTable GetCMapTable(
    SystemFontOpenTypeFontReader reader,
    SystemFontEncodingRecord record)
  {
    SystemFontCMapTable cmapTable;
    if (!this.tables.TryGetValue(record, out cmapTable))
    {
      reader.BeginReadingBlock();
      reader.Seek(this.Offset + (long) record.Offset, SeekOrigin.Begin);
      cmapTable = SystemFontCMapTable.ReadCMapTable(reader);
      reader.EndReadingBlock();
      this.tables[record] = cmapTable;
    }
    return cmapTable;
  }

  public SystemFontCMapTable GetCMapTable(ushort platformId, ushort encodingId)
  {
    if (this.encodings == null)
      return (SystemFontCMapTable) null;
    SystemFontEncodingRecord record = SystemFontEnumerable.FirstOrDefault<SystemFontEncodingRecord>((IEnumerable<SystemFontEncodingRecord>) this.encodings, (Func<SystemFontEncodingRecord, bool>) (e => (int) e.PlatformId == (int) platformId && (int) e.EncodingId == (int) encodingId));
    return record == null ? (SystemFontCMapTable) null : this.GetCMapTable(this.Reader, record);
  }

  public ushort GetGlyphId(ushort unicode)
  {
    this.Initialize();
    return this.encoding == null ? (ushort) 0 : this.encoding.GetGlyphId(unicode);
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    int num = (int) reader.ReadUShort();
    ushort capacity = reader.ReadUShort();
    this.encodings = new SystemFontEncodingRecord[(int) capacity];
    this.tables = new Dictionary<SystemFontEncodingRecord, SystemFontCMapTable>((int) capacity);
    for (int index = 0; index < (int) capacity; ++index)
    {
      SystemFontEncodingRecord fontEncodingRecord = new SystemFontEncodingRecord();
      fontEncodingRecord.Read(reader);
      this.encodings[index] = fontEncodingRecord;
    }
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    this.Initialize();
    this.encoding.Write(writer);
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    this.encoding = SystemFontCMapTable.ImportCMapTable(reader);
    this.isInitialized = true;
  }
}
