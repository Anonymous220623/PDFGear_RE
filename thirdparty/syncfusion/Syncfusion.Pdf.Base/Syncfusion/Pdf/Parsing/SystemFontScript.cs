// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontScript
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontScript : SystemFontTableBase
{
  private ushort defaultLangSysOffset;
  private SystemFontLangSys defaultLangSys;

  public uint ScriptTag { get; private set; }

  public SystemFontLangSys DefaultLangSys
  {
    get
    {
      if (this.defaultLangSys == null && this.defaultLangSysOffset != (ushort) 0)
        this.defaultLangSys = this.ReadLangSys(this.Reader, this.defaultLangSysOffset);
      return this.defaultLangSys;
    }
  }

  public SystemFontScript(SystemFontOpenTypeFontSourceBase fontFile, uint scriptTag)
    : base(fontFile)
  {
    this.ScriptTag = scriptTag;
  }

  private SystemFontLangSys ReadLangSys(SystemFontOpenTypeFontReader reader, ushort offset)
  {
    reader.BeginReadingBlock();
    reader.Seek(this.Offset + (long) offset, SeekOrigin.Begin);
    SystemFontLangSys systemFontLangSys = new SystemFontLangSys(this.FontSource);
    systemFontLangSys.Read(reader);
    reader.EndReadingBlock();
    return systemFontLangSys;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    this.defaultLangSysOffset = reader.ReadUShort();
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    if (this.DefaultLangSys != null)
    {
      writer.WriteULong(this.ScriptTag);
      this.DefaultLangSys.Write(writer);
    }
    else
      writer.WriteULong(SystemFontTags.NULL_TAG);
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    this.defaultLangSys = new SystemFontLangSys(this.FontSource);
    this.defaultLangSys.Import(reader);
  }

  public override string ToString() => SystemFontTags.GetStringFromTag(this.ScriptTag);
}
