// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontNameIndex
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontNameIndex(SystemFontCFFFontFile file, long offset) : SystemFontIndex(file, offset)
{
  private string[] names;

  public string this[ushort sid] => this.GetString((int) sid);

  private string ReadString(SystemFontCFFFontReader reader, uint offset, int length)
  {
    reader.BeginReadingBlock();
    reader.Seek(this.DataOffset + (long) offset, SeekOrigin.Begin);
    string str = reader.ReadString(length);
    reader.EndReadingBlock();
    return str;
  }

  private string GetString(int index)
  {
    if (this.names[index] == null)
      this.names[index] = this.ReadString(this.Reader, this.Offsets[index], this.GetDataLength(index));
    return this.names[index];
  }

  public override void Read(SystemFontCFFFontReader reader)
  {
    base.Read(reader);
    if (this.Count == (ushort) 0)
      return;
    this.names = new string[(int) this.Count];
  }
}
