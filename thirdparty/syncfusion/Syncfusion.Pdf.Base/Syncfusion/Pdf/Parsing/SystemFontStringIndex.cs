// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontStringIndex
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontStringIndex(SystemFontCFFFontFile file, long offset) : SystemFontIndex(file, offset)
{
  private string[] strings;

  public string this[ushort sid]
  {
    get
    {
      if (SystemFontStandardStrings.IsStandardString(sid))
        return SystemFontStandardStrings.GetStandardString(sid);
      sid -= (ushort) SystemFontStandardStrings.StandardStringsCount;
      return this.GetString((int) sid);
    }
  }

  private string ReadString(SystemFontCFFFontReader reader, uint offset, int length)
  {
    reader.BeginReadingBlock();
    long offset1 = this.DataOffset + (long) offset;
    reader.Seek(offset1, SeekOrigin.Begin);
    string str = reader.ReadString(length);
    reader.EndReadingBlock();
    return str;
  }

  private string GetString(int index)
  {
    if (this.strings[index] == null)
      this.strings[index] = this.ReadString(this.Reader, this.Offsets[index], this.GetDataLength(index));
    return this.strings[index];
  }

  public override void Read(SystemFontCFFFontReader reader)
  {
    base.Read(reader);
    if (this.Count == (ushort) 0)
      return;
    this.strings = new string[(int) this.Count];
  }
}
