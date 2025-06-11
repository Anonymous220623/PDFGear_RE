// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.CatalogedReaderBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal abstract class CatalogedReaderBase
{
  private bool m_isBigEndian;

  internal CatalogedReaderBase(bool isBigEndian) => this.m_isBigEndian = isBigEndian;

  internal bool IsBigEndian
  {
    get => this.m_isBigEndian;
    set => this.m_isBigEndian = value;
  }

  internal abstract CatalogedReaderBase WithByteOrder(bool isBigEndian);

  internal abstract CatalogedReaderBase WithShiftedBaseOffset(int shift);

  internal abstract int ToUnshiftedOffset(int offset);

  internal abstract byte ReadByte(int index);

  internal abstract byte[] GetBytes(int index, int count);

  internal abstract bool IsValidIndex(int index, int length);

  internal abstract long Length { get; }

  internal sbyte ReadSignedByte(int index) => (sbyte) this.ReadByte(index);

  internal ushort ReadUInt16(int index)
  {
    return this.m_isBigEndian ? (ushort) ((uint) this.ReadByte(index) << 8 | (uint) this.ReadByte(index + 1)) : (ushort) ((uint) this.ReadByte(index + 1) << 8 | (uint) this.ReadByte(index));
  }

  internal short ReadInt16(int index)
  {
    return this.m_isBigEndian ? (short) ((int) this.ReadByte(index) << 8 | (int) this.ReadByte(index + 1)) : (short) ((int) this.ReadByte(index + 1) << 8 | (int) this.ReadByte(index));
  }

  internal int ReadInt24(int index)
  {
    return this.m_isBigEndian ? (int) this.ReadByte(index) << 16 /*0x10*/ | (int) this.ReadByte(index + 1) << 8 | (int) this.ReadByte(index + 2) : (int) this.ReadByte(index + 2) << 16 /*0x10*/ | (int) this.ReadByte(index + 1) << 8 | (int) this.ReadByte(index);
  }

  internal uint ReadUInt32(int index)
  {
    return this.m_isBigEndian ? (uint) ((int) this.ReadByte(index) << 24 | (int) this.ReadByte(index + 1) << 16 /*0x10*/ | (int) this.ReadByte(index + 2) << 8) | (uint) this.ReadByte(index + 3) : (uint) ((int) this.ReadByte(index + 3) << 24 | (int) this.ReadByte(index + 2) << 16 /*0x10*/ | (int) this.ReadByte(index + 1) << 8) | (uint) this.ReadByte(index);
  }

  internal int ReadInt32(int index)
  {
    return this.m_isBigEndian ? (int) this.ReadByte(index) << 24 | (int) this.ReadByte(index + 1) << 16 /*0x10*/ | (int) this.ReadByte(index + 2) << 8 | (int) this.ReadByte(index + 3) : (int) this.ReadByte(index + 3) << 24 | (int) this.ReadByte(index + 2) << 16 /*0x10*/ | (int) this.ReadByte(index + 1) << 8 | (int) this.ReadByte(index);
  }

  internal long ReadInt64(int index)
  {
    return this.m_isBigEndian ? (long) this.ReadByte(index) << 56 | (long) this.ReadByte(index + 1) << 48 /*0x30*/ | (long) this.ReadByte(index + 2) << 40 | (long) this.ReadByte(index + 3) << 32 /*0x20*/ | (long) this.ReadByte(index + 4) << 24 | (long) this.ReadByte(index + 5) << 16 /*0x10*/ | (long) this.ReadByte(index + 6) << 8 | (long) this.ReadByte(index + 7) : (long) this.ReadByte(index + 7) << 56 | (long) this.ReadByte(index + 6) << 48 /*0x30*/ | (long) this.ReadByte(index + 5) << 40 | (long) this.ReadByte(index + 4) << 32 /*0x20*/ | (long) this.ReadByte(index + 3) << 24 | (long) this.ReadByte(index + 2) << 16 /*0x10*/ | (long) this.ReadByte(index + 1) << 8 | (long) this.ReadByte(index);
  }

  internal float ReadFloat32(int index)
  {
    return BitConverter.ToSingle(BitConverter.GetBytes(this.ReadInt32(index)), 0);
  }

  internal double ReadDouble64(int index) => BitConverter.Int64BitsToDouble(this.ReadInt64(index));

  internal string ReadString(int index, int length, Encoding encoding)
  {
    byte[] bytes = this.GetBytes(index, length);
    return encoding.GetString(bytes, 0, bytes.Length);
  }

  internal string ReadNullTerminatedString(int index, int length, Encoding encoding)
  {
    byte[] bytes = this.ReadNullTerminatedBytes(index, length);
    return (encoding == null ? Encoding.UTF8 : encoding).GetString(bytes, 0, bytes.Length);
  }

  internal string ReadNullTerminatedStringValue(int index, int length, Encoding encoding)
  {
    byte[] bytes = this.ReadNullTerminatedBytes(index, length);
    return encoding.GetString(bytes, 0, bytes.Length);
  }

  internal byte[] ReadNullTerminatedBytes(int index, int maxLength)
  {
    byte[] bytes = this.GetBytes(index, maxLength);
    int length = 0;
    while (length < bytes.Length && bytes[length] != (byte) 0)
      ++length;
    if (length == maxLength)
      return bytes;
    byte[] destinationArray = new byte[length];
    if (length > 0)
      Array.Copy((Array) bytes, 0, (Array) destinationArray, 0, length);
    return destinationArray;
  }
}
