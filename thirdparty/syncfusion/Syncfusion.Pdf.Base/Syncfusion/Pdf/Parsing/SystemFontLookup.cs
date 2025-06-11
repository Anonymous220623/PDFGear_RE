// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontLookup
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontLookup : SystemFontTableBase
{
  private ushort[] subTableOffsets;
  private SystemFontSubTable[] subTables;

  public ushort Type { get; private set; }

  public ushort Flags { get; private set; }

  public IEnumerable<SystemFontSubTable> SubTables
  {
    get
    {
      if (this.subTables == null)
      {
        this.subTables = new SystemFontSubTable[this.subTableOffsets.Length];
        for (int index = 0; index < this.subTableOffsets.Length; ++index)
          this.subTables[index] = this.ReadSubtable(this.Reader, this.subTableOffsets[index]);
      }
      return (IEnumerable<SystemFontSubTable>) this.subTables;
    }
  }

  internal static bool IsSupported(ushort type)
  {
    switch (type)
    {
      case 1:
      case 2:
      case 4:
        return true;
      default:
        return false;
    }
  }

  public SystemFontLookup(SystemFontOpenTypeFontSourceBase fontFile, ushort type)
    : base(fontFile)
  {
    this.Type = type;
  }

  private SystemFontSubTable ReadSubtable(SystemFontOpenTypeFontReader reader, ushort offset)
  {
    reader.BeginReadingBlock();
    long offset1 = this.Offset + (long) offset;
    reader.Seek(offset1, SeekOrigin.Begin);
    SystemFontSubTable systemFontSubTable = SystemFontSubTable.ReadSubTable(this.FontSource, reader, this.Type);
    systemFontSubTable.Offset = offset1;
    reader.EndReadingBlock();
    return systemFontSubTable;
  }

  public SystemFontGlyphsSequence Apply(SystemFontGlyphsSequence glyphIDs)
  {
    SystemFontGlyphsSequence glyphIndices = glyphIDs;
    foreach (SystemFontSubTable subTable in this.SubTables)
      glyphIndices = subTable.Apply(glyphIndices);
    return glyphIndices;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    this.Flags = reader.ReadUShort();
    ushort length = reader.ReadUShort();
    this.subTableOffsets = new ushort[(int) length];
    for (int index = 0; index < (int) length; ++index)
      this.subTableOffsets[index] = reader.ReadUShort();
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    writer.WriteUShort(this.Type);
    writer.WriteUShort(this.Flags);
    writer.WriteUShort((ushort) this.subTableOffsets.Length);
    foreach (SystemFontTableBase subTable in this.SubTables)
      subTable.Write(writer);
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    this.Flags = reader.ReadUShort();
    ushort length = reader.ReadUShort();
    this.subTables = new SystemFontSubTable[(int) length];
    for (int index = 0; index < (int) length; ++index)
    {
      SystemFontSubTable systemFontSubTable = SystemFontSubTable.ImportSubTable(this.FontSource, reader, this.Type);
      this.subTables[index] = systemFontSubTable;
    }
  }
}
