// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontOpenTypeFontReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontOpenTypeFontReader(byte[] data) : SystemFontReaderBase(data)
{
  public sbyte ReadChar() => (sbyte) this.Read();

  public ushort ReadUShort()
  {
    byte[] buffer = new byte[2];
    this.ReadBE(buffer, 2);
    return BitConverter.ToUInt16(buffer, 0);
  }

  public short ReadShort()
  {
    byte[] buffer = new byte[2];
    this.ReadBE(buffer, 2);
    return BitConverter.ToInt16(buffer, 0);
  }

  public uint ReadULong()
  {
    byte[] buffer = new byte[4];
    this.ReadBE(buffer, 4);
    return BitConverter.ToUInt32(buffer, 0);
  }

  public int ReadLong()
  {
    byte[] buffer = new byte[4];
    this.ReadBE(buffer, 4);
    return BitConverter.ToInt32(buffer, 0);
  }

  public long ReadLongDateTime()
  {
    byte[] buffer = new byte[8];
    this.ReadBE(buffer, 8);
    return BitConverter.ToInt64(buffer, 0);
  }

  public float ReadFixed()
  {
    return (float) this.ReadShort() + (float) ((int) this.ReadUShort() / 65536 /*0x010000*/);
  }

  public float Read2Dot14() => (float) this.ReadShort() / 16384f;

  public string ReadString()
  {
    byte length = this.Read();
    byte[] bytes = new byte[(int) length];
    for (int index = 0; index < (int) length; ++index)
      bytes[index] = this.Read();
    return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
  }
}
