// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.EndianBinaryReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf;

internal class EndianBinaryReader : BinaryReader
{
  private bool _bigEndian;

  public EndianBinaryReader(Stream input)
    : base(input)
  {
  }

  public EndianBinaryReader(Stream input, Encoding encoding)
    : base(input, encoding)
  {
  }

  public EndianBinaryReader(Stream input, Encoding encoding, bool bigEndian)
    : base(input, encoding)
  {
    this._bigEndian = bigEndian;
  }

  public EndianBinaryReader(Stream input, bool bigEndian)
    : base(input, bigEndian ? Encoding.BigEndianUnicode : Encoding.UTF8)
  {
    this._bigEndian = bigEndian;
  }

  public override short ReadInt16()
  {
    if (!this._bigEndian)
      return base.ReadInt16();
    byte[] numArray = this.ReadBytes(2);
    Array.Reverse((Array) numArray);
    return BitConverter.ToInt16(numArray, 0);
  }

  public override int ReadInt32()
  {
    if (!this._bigEndian)
      return base.ReadInt32();
    byte[] numArray = this.ReadBytes(4);
    Array.Reverse((Array) numArray);
    return BitConverter.ToInt32(numArray, 0);
  }

  public override long ReadInt64()
  {
    if (!this._bigEndian)
      return base.ReadInt64();
    byte[] numArray = this.ReadBytes(8);
    Array.Reverse((Array) numArray);
    return BitConverter.ToInt64(numArray, 0);
  }

  public override float ReadSingle()
  {
    if (!this._bigEndian)
      return base.ReadSingle();
    byte[] numArray = this.ReadBytes(4);
    Array.Reverse((Array) numArray);
    return BitConverter.ToSingle(numArray, 0);
  }

  public override ushort ReadUInt16()
  {
    if (!this._bigEndian)
      return base.ReadUInt16();
    byte[] numArray = this.ReadBytes(2);
    Array.Reverse((Array) numArray);
    return BitConverter.ToUInt16(numArray, 0);
  }

  public override uint ReadUInt32()
  {
    if (!this._bigEndian)
      return base.ReadUInt32();
    byte[] numArray = this.ReadBytes(4);
    Array.Reverse((Array) numArray);
    return BitConverter.ToUInt32(numArray, 0);
  }

  public override ulong ReadUInt64()
  {
    if (!this._bigEndian)
      return base.ReadUInt64();
    byte[] numArray = this.ReadBytes(8);
    Array.Reverse((Array) numArray);
    return BitConverter.ToUInt64(numArray, 0);
  }
}
