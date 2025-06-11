// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontTopIndex
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontTopIndex(SystemFontCFFFontFile file, long offset) : SystemFontIndex(file, offset)
{
  private SystemFontTop[] tops;

  public SystemFontTop this[int index] => this.GetTop(index);

  private SystemFontTop ReadTop(SystemFontCFFFontReader reader, uint offset, int length)
  {
    reader.BeginReadingBlock();
    long offset1 = this.DataOffset + (long) offset;
    reader.Seek(offset1, SeekOrigin.Begin);
    SystemFontTop systemFontTop = new SystemFontTop(this.File, offset1, length);
    systemFontTop.Read(reader);
    reader.EndReadingBlock();
    return systemFontTop;
  }

  private SystemFontTop GetTop(int index)
  {
    if (this.tops[index] == null)
      this.tops[index] = this.ReadTop(this.Reader, this.Offsets[index], this.GetDataLength(index));
    return this.tops[index];
  }

  public override void Read(SystemFontCFFFontReader reader)
  {
    base.Read(reader);
    if (this.Count == (ushort) 0)
      return;
    this.tops = new SystemFontTop[(int) this.Count];
  }
}
