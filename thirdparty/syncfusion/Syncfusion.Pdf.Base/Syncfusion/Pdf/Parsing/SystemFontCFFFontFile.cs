// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontCFFFontFile
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontCFFFontFile
{
  private readonly SystemFontCFFFontReader reader;
  private SystemFontCFFFontSource fontSource;

  public SystemFontCFFFontSource FontSource => this.fontSource;

  public SystemFontHeader Header { get; private set; }

  public SystemFontNameIndex Name { get; private set; }

  public SystemFontTopIndex TopIndex { get; private set; }

  public SystemFontStringIndex StringIndex { get; private set; }

  public SystemFontSubrsIndex GlobalSubrs { get; private set; }

  public SystemFontCFFFontReader Reader => this.reader;

  public SystemFontCFFFontFile(byte[] data)
  {
    this.reader = new SystemFontCFFFontReader(data);
    this.Initialize();
  }

  public string ReadString(ushort sid) => this.StringIndex[sid];

  internal void ReadTable(SystemFontCFFTable table)
  {
    this.Reader.BeginReadingBlock();
    this.Reader.Seek(table.Offset, SeekOrigin.Begin);
    table.Read(this.Reader);
    this.Reader.EndReadingBlock();
  }

  private void Initialize()
  {
    this.Header = new SystemFontHeader(this);
    this.ReadTable((SystemFontCFFTable) this.Header);
    this.Name = new SystemFontNameIndex(this, (long) this.Header.HeaderSize);
    this.ReadTable((SystemFontCFFTable) this.Name);
    this.TopIndex = new SystemFontTopIndex(this, this.Name.SkipOffset);
    this.ReadTable((SystemFontCFFTable) this.TopIndex);
    this.StringIndex = new SystemFontStringIndex(this, this.TopIndex.SkipOffset);
    this.ReadTable((SystemFontCFFTable) this.StringIndex);
    SystemFontTop top = this.TopIndex[0];
    this.GlobalSubrs = new SystemFontSubrsIndex(this, top.CharstringType, this.StringIndex.SkipOffset);
    this.ReadTable((SystemFontCFFTable) this.GlobalSubrs);
    this.fontSource = new SystemFontCFFFontSource(this, top);
  }
}
