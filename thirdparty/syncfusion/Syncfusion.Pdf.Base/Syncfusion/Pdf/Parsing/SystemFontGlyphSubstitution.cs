// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontGlyphSubstitution
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontGlyphSubstitution(SystemFontOpenTypeFontSourceBase fontFile) : 
  SystemFontTrueTypeTableBase(fontFile)
{
  private ushort scriptListOffset;
  private ushort featureListOffset;
  private ushort lookupListOffset;
  private SystemFontScriptList scriptList;
  private SystemFontLookupList lookupList;
  private SystemFontFeatureList featureList;

  internal override uint Tag => SystemFontTags.GSUB_TABLE;

  private void ReadTable<T>(T table, ushort offset) where T : SystemFontTableBase
  {
    if (offset == (ushort) 0)
      return;
    long offset1 = this.Offset + (long) offset;
    table.Offset = offset1;
    this.Reader.BeginReadingBlock();
    this.Reader.Seek(offset1, SeekOrigin.Begin);
    table.Read(this.Reader);
    this.Reader.EndReadingBlock();
  }

  private void ReadScriptList()
  {
    this.scriptList = new SystemFontScriptList(this.FontSource);
    this.ReadTable<SystemFontScriptList>(this.scriptList, this.scriptListOffset);
  }

  private void ReadFeatureList()
  {
    this.featureList = new SystemFontFeatureList(this.FontSource);
    this.ReadTable<SystemFontFeatureList>(this.featureList, this.featureListOffset);
  }

  private void ReadLookupList()
  {
    this.lookupList = new SystemFontLookupList(this.FontSource);
    this.ReadTable<SystemFontLookupList>(this.lookupList, this.lookupListOffset);
  }

  public SystemFontScript GetScript(uint tag)
  {
    if (this.scriptList == null)
      this.ReadScriptList();
    return this.scriptList.GetScript(tag);
  }

  public SystemFontFeature GetFeature(ushort index)
  {
    if (this.featureList == null)
      this.ReadFeatureList();
    return this.featureList.GetFeature((int) index);
  }

  public SystemFontLookup GetLookup(ushort index)
  {
    if (this.lookupList == null)
      this.ReadLookupList();
    return this.lookupList.GetLookup(index);
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    double num = (double) reader.ReadFixed();
    this.scriptListOffset = reader.ReadUShort();
    this.featureListOffset = reader.ReadUShort();
    this.lookupListOffset = reader.ReadUShort();
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    this.ReadScriptList();
    this.ReadFeatureList();
    this.ReadLookupList();
    if (this.scriptList == null || this.featureList == null || this.lookupList == null)
    {
      writer.Write((byte) 0);
    }
    else
    {
      writer.Write((byte) 1);
      this.scriptList.Write(writer);
      this.featureList.Write(writer);
      this.lookupList.Write(writer);
    }
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    if (reader.Read() <= (byte) 0)
      return;
    this.scriptList = new SystemFontScriptList(this.FontSource);
    this.featureList = new SystemFontFeatureList(this.FontSource);
    this.lookupList = new SystemFontLookupList(this.FontSource);
    this.scriptList.Import(reader);
    this.featureList.Import(reader);
    this.lookupList.Import(reader);
  }
}
