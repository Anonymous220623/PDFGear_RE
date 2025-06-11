// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontOpenTypeFontSource
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontOpenTypeFontSource : SystemFontOpenTypeFontSourceBase
{
  private Dictionary<ushort, SystemFontGlyphData> glyphsData;
  private SystemFontCMap cmap;
  private SystemFontHorizontalMetrics hmtx;
  private SystemFontKerning kern;
  private SystemFontGlyphSubstitution gsub;
  private Dictionary<uint, SystemFontTableRecord> tables;
  private SystemFontIndexToLocation loca;
  private SystemFontMaxProfile maxp;
  private SystemFontHead head;
  private SystemFontHorizontalHeader hhea;
  private SystemFontSystemFontName name;
  private SystemFontPost post;
  private SystemFontCFFFontSource ccf;

  internal override SystemFontOutlines Outlines
  {
    get
    {
      return !this.OffsetTable.HasOpenTypeOutlines ? SystemFontOutlines.TrueType : SystemFontOutlines.OpenType;
    }
  }

  internal override SystemFontCMap CMap
  {
    get
    {
      if (this.cmap == null)
      {
        this.cmap = new SystemFontCMap((SystemFontOpenTypeFontSourceBase) this);
        this.ReadTableData<SystemFontCMap>(this.cmap);
      }
      return this.cmap;
    }
  }

  internal override SystemFontHorizontalMetrics HMtx
  {
    get
    {
      if (this.hmtx == null)
      {
        this.hmtx = new SystemFontHorizontalMetrics((SystemFontOpenTypeFontSourceBase) this);
        this.ReadTableData<SystemFontHorizontalMetrics>(this.hmtx);
      }
      return this.hmtx;
    }
  }

  internal override SystemFontKerning Kern
  {
    get
    {
      if (this.kern == null)
      {
        this.kern = new SystemFontKerning((SystemFontOpenTypeFontSourceBase) this);
        this.ReadTableData<SystemFontKerning>(this.kern);
      }
      return this.kern;
    }
  }

  internal override SystemFontGlyphSubstitution GSub
  {
    get
    {
      if (this.gsub == null)
      {
        this.gsub = new SystemFontGlyphSubstitution((SystemFontOpenTypeFontSourceBase) this);
        this.ReadTableData<SystemFontGlyphSubstitution>(this.gsub);
      }
      return this.gsub;
    }
  }

  internal override SystemFontHead Head
  {
    get
    {
      if (this.head == null)
      {
        this.head = new SystemFontHead((SystemFontOpenTypeFontSourceBase) this);
        this.ReadTableData<SystemFontHead>(this.head);
      }
      return this.head;
    }
  }

  internal override SystemFontHorizontalHeader HHea
  {
    get
    {
      if (this.hhea == null)
      {
        this.hhea = new SystemFontHorizontalHeader((SystemFontOpenTypeFontSourceBase) this);
        this.ReadTableData<SystemFontHorizontalHeader>(this.hhea);
      }
      return this.hhea;
    }
  }

  internal SystemFontPost Post
  {
    get
    {
      if (this.post == null)
        this.post = this.ReadTableData<SystemFontPost>(SystemFontTags.POST_TABLE, new SystemFontReadTableFormatDelegate<SystemFontPost>(SystemFontPost.ReadPostTable));
      return this.post;
    }
  }

  internal SystemFontSystemFontName Name
  {
    get
    {
      if (this.name == null)
        this.name = this.ReadTableData<SystemFontSystemFontName>(SystemFontTags.NAME_TABLE, new SystemFontReadTableFormatDelegate<SystemFontSystemFontName>(SystemFontSystemFontName.ReadNameTable));
      return this.name;
    }
  }

  internal SystemFontIndexToLocation Loca
  {
    get
    {
      if (this.loca == null)
      {
        this.loca = new SystemFontIndexToLocation(this);
        this.ReadTableData<SystemFontIndexToLocation>(this.loca);
      }
      return this.loca;
    }
  }

  internal SystemFontMaxProfile MaxP
  {
    get
    {
      if (this.maxp == null)
      {
        this.maxp = new SystemFontMaxProfile((SystemFontOpenTypeFontSourceBase) this);
        this.ReadTableData<SystemFontMaxProfile>(this.maxp);
      }
      return this.maxp;
    }
  }

  internal Dictionary<uint, SystemFontTableRecord> Tables => this.tables;

  internal SystemFontOffsetTable OffsetTable { get; private set; }

  internal override ushort GlyphCount => this.MaxP.NumGlyphs;

  internal long Offset { get; set; }

  public override string FontFamily => this.Name.FontFamily;

  public override bool IsBold => this.Head.IsBold;

  public override bool IsItalic => this.Head.IsItalic;

  internal override SystemFontCFFFontSource CFF
  {
    get
    {
      if (!this.OffsetTable.HasOpenTypeOutlines)
        return (SystemFontCFFFontSource) null;
      if (this.ccf == null)
        this.ccf = this.ReadCFFTable();
      return this.ccf;
    }
  }

  public SystemFontOpenTypeFontSource(SystemFontOpenTypeFontReader reader)
    : base(reader)
  {
    this.Offset = 0L;
    this.Initialize();
  }

  private void ReadTableData<T>(T table) where T : SystemFontTrueTypeTableBase
  {
    SystemFontTableRecord systemFontTableRecord;
    if (!this.tables.TryGetValue(table.Tag, out systemFontTableRecord))
      return;
    long offset = this.Offset + (long) systemFontTableRecord.Offset;
    this.Reader.BeginReadingBlock();
    this.Reader.Seek(offset, SeekOrigin.Begin);
    table.Read(this.Reader);
    table.Offset = offset;
    this.Reader.EndReadingBlock();
  }

  private T ReadTableData<T>(
    uint tag,
    SystemFontReadTableFormatDelegate<T> readTableDelegate)
    where T : SystemFontTrueTypeTableBase
  {
    SystemFontTableRecord systemFontTableRecord;
    if (!this.tables.TryGetValue(tag, out systemFontTableRecord))
      return default (T);
    this.Reader.BeginReadingBlock();
    long offset = this.Offset + (long) systemFontTableRecord.Offset;
    this.Reader.Seek(offset, SeekOrigin.Begin);
    T obj = readTableDelegate((SystemFontOpenTypeFontSourceBase) this, this.Reader);
    obj.Offset = offset;
    this.Reader.EndReadingBlock();
    return obj;
  }

  private int GetTableLength(uint tag)
  {
    SystemFontTableRecord table = this.tables[tag];
    int tableLength = (int) ((long) this.Reader.Length - (long) table.Offset);
    foreach (SystemFontTableRecord systemFontTableRecord in this.tables.Values)
    {
      int num = (int) systemFontTableRecord.Offset - (int) table.Offset;
      if (num > 0 && num < tableLength)
        tableLength = num;
    }
    return tableLength;
  }

  private SystemFontCFFFontSource ReadCFFTable()
  {
    int tableLength = this.GetTableLength(SystemFontTags.CFF_TABLE);
    byte[] numArray = new byte[tableLength];
    this.Reader.BeginReadingBlock();
    this.Reader.Seek(this.Offset + (long) this.tables[SystemFontTags.CFF_TABLE].Offset, SeekOrigin.Begin);
    this.Reader.Read(numArray, tableLength);
    this.Reader.EndReadingBlock();
    return new SystemFontCFFFontFile(numArray).FontSource;
  }

  private void ReadTableRecords()
  {
    this.tables = new Dictionary<uint, SystemFontTableRecord>();
    for (int index = 0; index < (int) this.OffsetTable.NumTables; ++index)
    {
      SystemFontTableRecord systemFontTableRecord = new SystemFontTableRecord();
      systemFontTableRecord.Read(this.Reader);
      this.tables[systemFontTableRecord.Tag] = systemFontTableRecord;
    }
  }

  private void Initialize()
  {
    this.OffsetTable = new SystemFontOffsetTable();
    this.OffsetTable.Read(this.Reader);
    this.ReadTableRecords();
    this.glyphsData = new Dictionary<ushort, SystemFontGlyphData>();
  }

  internal override SystemFontGlyphData GetGlyphData(ushort glyphIndex)
  {
    SystemFontGlyphData glyphData;
    if (!this.glyphsData.TryGetValue(glyphIndex, out glyphData))
    {
      long offset1 = this.Loca.GetOffset(glyphIndex);
      SystemFontTableRecord systemFontTableRecord;
      this.tables.TryGetValue(SystemFontTags.GLYF_TABLE, out systemFontTableRecord);
      if (offset1 == -1L || systemFontTableRecord == null || offset1 >= (long) (systemFontTableRecord.Offset + systemFontTableRecord.Length))
        return new SystemFontGlyphData((SystemFontOpenTypeFontSourceBase) this, glyphIndex);
      long offset2 = offset1 + (long) systemFontTableRecord.Offset;
      this.Reader.BeginReadingBlock();
      this.Reader.Seek(offset2, SeekOrigin.Begin);
      glyphData = SystemFontGlyphData.ReadGlyf((SystemFontOpenTypeFontSourceBase) this, glyphIndex);
      this.Reader.EndReadingBlock();
      this.glyphsData[glyphIndex] = glyphData;
    }
    return glyphData;
  }
}
