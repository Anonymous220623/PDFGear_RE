// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.DataProvider
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Drawing;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal abstract class DataProvider : IDisposable
{
  ~DataProvider() => this.Dispose(false);

  public bool ReadBit(int iOffset, int iBit)
  {
    if (iBit < 0 || iBit > 7)
      throw new ArgumentOutOfRangeException(nameof (iBit), "Bit Position cannot be less than 0 or greater than 7.");
    return ((int) this.ReadByte(iOffset) & 1 << iBit) == 1 << iBit;
  }

  public abstract byte ReadByte(int iOffset);

  public bool ReadBoolean(int iOffset) => this.ReadByte(iOffset) != (byte) 0;

  public abstract short ReadInt16(int iOffset);

  [CLSCompliant(false)]
  public ushort ReadUInt16(int iOffset) => (ushort) this.ReadInt16(iOffset);

  public abstract int ReadInt32(int iOffset);

  [CLSCompliant(false)]
  public uint ReadUInt32(int iOffset) => (uint) this.ReadInt32(iOffset);

  public abstract long ReadInt64(int iOffset);

  public virtual double ReadDouble(int iOffset)
  {
    return BitConverterGeneral.Int64BitsToDouble(this.ReadInt64(iOffset));
  }

  public abstract void CopyTo(
    int iSourceOffset,
    byte[] arrDestination,
    int iDestOffset,
    int iLength);

  public virtual void CopyTo(
    int iSourceOffset,
    DataProvider destination,
    int iDestOffset,
    int iLength)
  {
    throw new NotImplementedException();
  }

  public abstract void Read(BinaryReader reader, int iOffset, int iLength, byte[] arrBuffer);

  public virtual string ReadString16Bit(int iOffset, out int iFullLength)
  {
    ushort num = this.ReadUInt16(iOffset);
    iOffset += 2;
    bool isUnicode = this.ReadBoolean(iOffset);
    ++iOffset;
    iFullLength = isUnicode ? 3 + (int) num * 2 : 3 + (int) num;
    int stringLength = isUnicode ? (int) num * 2 : (int) num;
    Encoding encoding = isUnicode ? Encoding.Unicode : BiffRecordRaw.LatinEncoding;
    return this.ReadString(iOffset, stringLength, encoding, isUnicode);
  }

  public virtual string ReadString16BitUpdateOffset(ref int iOffset)
  {
    int iFullLength;
    string str = this.ReadString16Bit(iOffset, out iFullLength);
    iOffset += iFullLength;
    return str;
  }

  public virtual string ReadString8Bit(int iOffset, out int iFullLength)
  {
    ushort num = (ushort) this.ReadByte(iOffset);
    ++iOffset;
    bool flag = this.ReadBoolean(iOffset);
    ++iOffset;
    int length = flag ? (int) num * 2 : (int) num;
    iFullLength = 2 + length;
    byte[] numArray = new byte[length];
    this.CopyTo(iOffset, numArray, 0, length);
    return (flag ? Encoding.Unicode : BiffRecordRaw.LatinEncoding).GetString(numArray, 0, length);
  }

  public int ReadArray(int iOffset, byte[] arrDest)
  {
    int iLength = arrDest != null ? arrDest.Length : throw new ArgumentNullException(nameof (arrDest));
    this.CopyTo(iOffset, arrDest, 0, iLength);
    return iOffset + iLength;
  }

  public int ReadArray(int iOffset, byte[] arrDest, int size)
  {
    if (arrDest == null)
      throw new ArgumentNullException(nameof (arrDest));
    this.CopyTo(iOffset, arrDest, 0, size);
    return iOffset + size;
  }

  public string ReadString(int offset, int iStrLen, out int iBytesInString, bool isByteCounted)
  {
    byte num1 = this.ReadByte(offset);
    int num2 = (num1 == (byte) 0 || isByteCounted ? iStrLen : 2 * iStrLen) + (offset + 1);
    bool flag = num1 != (byte) 0;
    iBytesInString = !flag || isByteCounted ? iStrLen : iStrLen * 2;
    byte[] numArray = new byte[iBytesInString];
    this.ReadArray(offset + 1, numArray);
    return (flag ? Encoding.Unicode : BiffRecordRaw.LatinEncoding).GetString(numArray, 0, numArray.Length);
  }

  public string ReadStringUpdateOffset(ref int offset, int iStrLen)
  {
    if (iStrLen <= 0)
      return string.Empty;
    int iBytesInString;
    string str = this.ReadString(offset, iStrLen, out iBytesInString, false);
    offset += iBytesInString + 1;
    return str;
  }

  public abstract string ReadString(
    int offset,
    int stringLength,
    Encoding encoding,
    bool isUnicode);

  [CLSCompliant(false)]
  public TAddr ReadAddr(int offset)
  {
    return new TAddr()
    {
      FirstRow = (int) this.ReadUInt16(offset),
      LastRow = (int) this.ReadUInt16(offset + 2),
      FirstCol = (int) this.ReadUInt16(offset + 4),
      LastCol = (int) this.ReadUInt16(offset + 6)
    };
  }

  public Rectangle ReadAddrAsRectangle(int offset)
  {
    int top = (int) this.ReadUInt16(offset);
    int bottom = (int) this.ReadUInt16(offset + 2);
    int left = (int) this.ReadUInt16(offset + 4);
    int right = (int) this.ReadUInt16(offset + 6);
    return Rectangle.FromLTRB(left, top, right, bottom);
  }

  public virtual void WriteInto(BinaryWriter writer, int iOffset, int iSize, byte[] arrBuffer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    int val2 = iSize;
    int length = arrBuffer.Length;
    int num;
    for (; val2 > 0; val2 -= num)
    {
      num = Math.Min(length, val2);
      this.CopyTo(iOffset, arrBuffer, 0, num);
      writer.Write(arrBuffer, 0, num);
      iOffset += num;
    }
  }

  public abstract void WriteByte(int iOffset, byte value);

  public abstract void WriteInt16(int iOffset, short value);

  [CLSCompliant(false)]
  public virtual void WriteUInt16(int iOffset, ushort value) => throw new NotImplementedException();

  public abstract void WriteInt32(int iOffset, int value);

  public abstract void WriteInt64(int iOffset, long value);

  [CLSCompliant(false)]
  public void WriteUInt32(int iOffset, uint value) => this.WriteInt32(iOffset, (int) value);

  public abstract void WriteBit(int offset, bool value, int bitPos);

  public abstract void WriteDouble(int iOffset, double value);

  public void WriteString8BitUpdateOffset(ref int offset, string value)
  {
    this.WriteByte(offset, (byte) value.Length);
    ++offset;
    this.WriteStringNoLenUpdateOffset(ref offset, value);
  }

  public void WriteString16BitUpdateOffset(ref int offset, string value)
  {
    this.WriteString16BitUpdateOffset(ref offset, value, true);
  }

  public void WriteString16BitUpdateOffset(ref int offset, string value, bool isUnicode)
  {
    int length = value != null ? value.Length : 0;
    this.WriteUInt16(offset, (ushort) length);
    offset += 2;
    this.WriteStringNoLenUpdateOffset(ref offset, value, isUnicode);
  }

  public int WriteString16Bit(int offset, string value)
  {
    return this.WriteString16Bit(offset, value, true);
  }

  public int WriteString16Bit(int offset, string value, bool isUnicode)
  {
    int num = offset;
    this.WriteString16BitUpdateOffset(ref offset, value, isUnicode);
    return offset - num;
  }

  public virtual void WriteStringNoLenUpdateOffset(ref int offset, string value)
  {
    this.WriteStringNoLenUpdateOffset(ref offset, value, true);
  }

  public abstract void WriteStringNoLenUpdateOffset(ref int offset, string value, bool bUnicode);

  public void WriteBytes(int offset, byte[] data) => this.WriteBytes(offset, data, 0, data.Length);

  public abstract void WriteBytes(int offset, byte[] value, int pos, int length);

  [CLSCompliant(false)]
  protected internal void WriteAddr(int offset, TAddr addr)
  {
    this.WriteUInt16(offset, (ushort) addr.FirstRow);
    this.WriteUInt16(offset + 2, (ushort) addr.LastRow);
    this.WriteUInt16(offset + 4, (ushort) addr.FirstCol);
    this.WriteUInt16(offset + 6, (ushort) addr.LastCol);
  }

  protected internal void WriteAddr(int offset, Rectangle addr)
  {
    this.WriteUInt16(offset, (ushort) addr.Top);
    this.WriteUInt16(offset + 2, (ushort) addr.Bottom);
    this.WriteUInt16(offset + 4, (ushort) addr.Left);
    this.WriteUInt16(offset + 6, (ushort) addr.Right);
  }

  public abstract int Capacity { get; }

  public abstract bool IsCleared { get; }

  public abstract void MoveMemory(int iDestOffset, int iSourceOffset, int iMemorySize);

  public abstract void CopyMemory(int iDestOffset, int iSourceOffset, int iMemorySize);

  public abstract int EnsureCapacity(int size);

  public abstract int EnsureCapacity(int size, int forceAdd);

  public abstract void ZeroMemory();

  public abstract void Clear();

  public abstract DataProvider CreateProvider();

  public void Dispose(bool isDisposing)
  {
    if (isDisposing)
      this.OnDispose();
    GC.SuppressFinalize((object) this);
  }

  public void Dispose()
  {
    this.Clear();
    this.Dispose(true);
  }

  protected virtual void OnDispose()
  {
  }
}
