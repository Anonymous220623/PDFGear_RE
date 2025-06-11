// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontSubrsIndex
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontSubrsIndex : SystemFontIndex
{
  private readonly int charstringType;
  private byte[][] subrs;
  private ushort bias;

  public byte[] this[int index] => this.GetSubr(index + (int) this.bias);

  public SystemFontSubrsIndex(SystemFontCFFFontFile fontFile, int charstringType, long offset)
    : base(fontFile, offset)
  {
    this.charstringType = charstringType;
  }

  private byte[] ReadSubr(SystemFontCFFFontReader reader, uint offset, int length)
  {
    reader.BeginReadingBlock();
    long offset1 = this.DataOffset + (long) offset;
    reader.Seek(offset1, SeekOrigin.Begin);
    byte[] buffer = new byte[length];
    reader.Read(buffer, length);
    reader.EndReadingBlock();
    return buffer;
  }

  private byte[] GetSubr(int index)
  {
    if (this.subrs[index] == null)
      this.subrs[index] = this.ReadSubr(this.Reader, this.Offsets[index], this.GetDataLength(index));
    return this.subrs[index];
  }

  public override void Read(SystemFontCFFFontReader reader)
  {
    base.Read(reader);
    if (this.Count == (ushort) 0)
      return;
    this.subrs = new byte[(int) this.Count][];
    ushort count = this.Count;
    if (this.charstringType == 1)
      this.bias = (ushort) 0;
    else if (count < (ushort) 1240)
      this.bias = (ushort) 107;
    else if (count < (ushort) 33900)
      this.bias = (ushort) 1131;
    else
      this.bias = (ushort) 32768 /*0x8000*/;
  }
}
