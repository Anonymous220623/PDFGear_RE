// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontLookupList
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontLookupList(SystemFontOpenTypeFontSourceBase fontFile) : SystemFontTableBase(fontFile)
{
  private ushort[] lookupOffsets;
  private SystemFontLookup[] lookups;

  private SystemFontLookup ReadLookup(SystemFontOpenTypeFontReader reader, ushort offset)
  {
    reader.BeginReadingBlock();
    long offset1 = this.Offset + (long) offset;
    reader.Seek(offset1, SeekOrigin.Begin);
    ushort type = reader.ReadUShort();
    if (!SystemFontLookup.IsSupported(type))
      return (SystemFontLookup) null;
    SystemFontLookup systemFontLookup = new SystemFontLookup(this.FontSource, type);
    systemFontLookup.Offset = offset1;
    systemFontLookup.Read(reader);
    reader.EndReadingBlock();
    return systemFontLookup;
  }

  public SystemFontLookup GetLookup(ushort index)
  {
    if (this.lookups[(int) index] == null && this.lookupOffsets != null)
      this.lookups[(int) index] = this.ReadLookup(this.Reader, this.lookupOffsets[(int) index]);
    return this.lookups[(int) index];
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    ushort length = reader.ReadUShort();
    this.lookupOffsets = new ushort[(int) length];
    this.lookups = new SystemFontLookup[(int) length];
    for (int index = 0; index < (int) length; ++index)
      this.lookupOffsets[index] = reader.ReadUShort();
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    writer.WriteUShort((ushort) this.lookupOffsets.Length);
    for (ushort index = 0; (int) index < this.lookupOffsets.Length; ++index)
    {
      SystemFontLookup lookup = this.GetLookup(index);
      if (lookup == null)
        writer.WriteUShort(SystemFontTags.NULL_TYPE);
      else
        lookup.Write(writer);
    }
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    ushort length = reader.ReadUShort();
    this.lookups = new SystemFontLookup[(int) length];
    for (int index = 0; index < (int) length; ++index)
    {
      ushort type = reader.ReadUShort();
      if ((int) type != (int) SystemFontTags.NULL_TYPE)
        new SystemFontLookup(this.FontSource, type).Import(reader);
    }
  }
}
