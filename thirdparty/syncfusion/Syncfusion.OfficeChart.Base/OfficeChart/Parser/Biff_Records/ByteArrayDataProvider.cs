// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ByteArrayDataProvider
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal class ByteArrayDataProvider : DataProvider
{
  private byte[] m_arrData;

  public ByteArrayDataProvider()
    : this(new byte[128 /*0x80*/])
  {
  }

  public ByteArrayDataProvider(byte[] arrData)
  {
    this.m_arrData = arrData != null ? arrData : throw new ArgumentNullException(nameof (arrData));
  }

  public byte[] InternalBuffer => this.m_arrData;

  public override int Capacity => this.m_arrData.Length;

  public override bool IsCleared => this.m_arrData == null;

  public override byte ReadByte(int iOffset) => this.m_arrData[iOffset];

  public override short ReadInt16(int iOffset) => BitConverter.ToInt16(this.m_arrData, iOffset);

  public override int ReadInt32(int iOffset) => BitConverter.ToInt32(this.m_arrData, iOffset);

  public override long ReadInt64(int iOffset) => BitConverter.ToInt64(this.m_arrData, iOffset);

  public override void CopyTo(
    int iSourceOffset,
    byte[] arrDestination,
    int iDestOffset,
    int iLength)
  {
    Buffer.BlockCopy((Array) this.m_arrData, iSourceOffset, (Array) arrDestination, iDestOffset, iLength);
  }

  public override void CopyTo(
    int iSourceOffset,
    DataProvider destination,
    int iDestOffset,
    int iLength)
  {
    if (iLength <= 0)
      return;
    destination.WriteBytes(iDestOffset, this.m_arrData, iSourceOffset, iLength);
  }

  public override void Read(BinaryReader reader, int iOffset, int iLength, byte[] arrBuffer)
  {
    if (iOffset + iLength > this.m_arrData.Length)
      throw new ArgumentOutOfRangeException();
    reader.Read(this.m_arrData, iOffset, iLength);
  }

  public override string ReadString(
    int offset,
    int stringLength,
    Encoding encoding,
    bool isUnicode)
  {
    if (encoding == null)
      encoding = isUnicode ? Encoding.Unicode : Encoding.ASCII;
    return encoding.GetString(this.m_arrData, offset, stringLength);
  }

  public override int EnsureCapacity(int size) => this.EnsureCapacity(size, 0);

  public override int EnsureCapacity(int size, int forceAdd)
  {
    int length = this.m_arrData != null ? this.m_arrData.Length : 0;
    if (length < size)
    {
      byte[] dst = new byte[size];
      if (length > 0)
        Buffer.BlockCopy((Array) this.m_arrData, 0, (Array) dst, 0, length);
      this.m_arrData = dst;
    }
    return length;
  }

  public override void ZeroMemory()
  {
    int index = 0;
    for (int length = this.m_arrData.Length; index < length; ++index)
      this.m_arrData[index] = (byte) 0;
  }

  public override void WriteByte(int iOffset, byte value) => this.m_arrData[iOffset] = value;

  public override void WriteInt16(int iOffset, short value)
  {
    BitConverter.GetBytes(value).CopyTo((Array) this.m_arrData, iOffset);
  }

  [CLSCompliant(false)]
  public override void WriteUInt16(int iOffset, ushort value)
  {
    BitConverter.GetBytes(value).CopyTo((Array) this.m_arrData, iOffset);
  }

  public override void WriteInt32(int iOffset, int value)
  {
    BitConverter.GetBytes(value).CopyTo((Array) this.m_arrData, iOffset);
  }

  public override void WriteInt64(int iOffset, long value)
  {
    BitConverter.GetBytes(value).CopyTo((Array) this.m_arrData, iOffset);
  }

  public override void WriteBit(int offset, bool value, int bitPos)
  {
    if (bitPos < 0 || bitPos > 7)
      throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position can be zero or greater than 7.");
    if (value)
      this.m_arrData[offset] |= (byte) (1 << bitPos);
    else
      this.m_arrData[offset] &= (byte) ~(1 << bitPos);
  }

  public override void WriteDouble(int iOffset, double value)
  {
    BitConverter.GetBytes(value).CopyTo((Array) this.m_arrData, iOffset);
  }

  public override void WriteStringNoLenUpdateOffset(ref int offset, string value, bool unicode)
  {
    switch (value)
    {
      case null:
        break;
      case "":
        break;
      default:
        byte[] bytes = (unicode ? Encoding.Unicode : Encoding.ASCII).GetBytes(value);
        this.m_arrData[offset] = unicode ? (byte) 1 : (byte) 0;
        ++offset;
        int length = bytes.Length;
        Buffer.BlockCopy((Array) bytes, 0, (Array) this.m_arrData, offset, length);
        offset += length;
        break;
    }
  }

  public override void WriteBytes(int offset, byte[] value, int pos, int length)
  {
    if (length == 0)
      return;
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (pos < 0)
      throw new ArgumentOutOfRangeException(nameof (pos), "Position cannot be zeroless.");
    if (length < 0)
      throw new ArgumentOutOfRangeException(nameof (length), "Length of data to copy must be greater then zero.");
    if (pos + length > value.Length)
      throw new ArgumentOutOfRangeException(nameof (value), "Position or length has wrong value.");
    Buffer.BlockCopy((Array) value, pos, (Array) this.m_arrData, offset, length);
  }

  public override void WriteInto(BinaryWriter writer, int iOffset, int iSize, byte[] arrBuffer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.Write(this.m_arrData, iOffset, iSize);
  }

  internal void SetBuffer(byte[] arrNewBuffer)
  {
    this.m_arrData = arrNewBuffer != null ? arrNewBuffer : throw new ArgumentNullException(nameof (arrNewBuffer));
  }

  public override void Clear() => this.m_arrData = (byte[]) null;

  public override void MoveMemory(int iDestOffset, int iSourceOffset, int iMemorySize)
  {
    Buffer.BlockCopy((Array) this.m_arrData, iSourceOffset, (Array) this.m_arrData, iDestOffset, iMemorySize);
  }

  public override void CopyMemory(int iDestOffset, int iSourceOffset, int iMemorySize)
  {
    Buffer.BlockCopy((Array) this.m_arrData, iSourceOffset, (Array) this.m_arrData, iDestOffset, iMemorySize);
  }

  public override DataProvider CreateProvider() => (DataProvider) new ByteArrayDataProvider();
}
