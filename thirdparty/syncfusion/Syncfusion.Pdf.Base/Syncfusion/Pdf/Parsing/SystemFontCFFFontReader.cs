// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontCFFFontReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontCFFFontReader(byte[] data) : SystemFontReaderBase(data)
{
  public byte ReadCard8() => this.Read();

  public ushort ReadCard16()
  {
    byte[] buffer = new byte[2];
    this.ReadBE(buffer, 2);
    return BitConverter.ToUInt16(buffer, 0);
  }

  public uint ReadCard24()
  {
    byte[] buffer = new byte[4];
    this.ReadBE(buffer, 3);
    return BitConverter.ToUInt32(buffer, 0);
  }

  public uint ReadCard32()
  {
    byte[] buffer = new byte[4];
    this.ReadBE(buffer, 4);
    return BitConverter.ToUInt32(buffer, 0);
  }

  public uint ReadOffset(byte offsetSize)
  {
    switch (offsetSize)
    {
      case 1:
        return (uint) this.ReadCard8();
      case 2:
        return (uint) this.ReadCard16();
      case 3:
        return this.ReadCard24();
      case 4:
        return this.ReadCard32();
      default:
        throw new NotSupportedException();
    }
  }

  public byte ReadOffSize() => this.ReadCard8();

  public ushort ReadSID() => this.ReadCard16();

  public string ReadString(int length)
  {
    byte[] numArray = new byte[length];
    this.Read(numArray, length);
    return Encoding.UTF8.GetString(numArray, 0, numArray.Length);
  }
}
