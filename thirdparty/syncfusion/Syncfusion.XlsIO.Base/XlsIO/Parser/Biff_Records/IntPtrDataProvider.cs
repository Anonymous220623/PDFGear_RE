// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.IntPtrDataProvider
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public class IntPtrDataProvider : DataProvider, IDisposable
{
  private IntPtr m_ptrHeapHandle;
  private IntPtr m_ptrData;
  protected long m_lPointer;
  private bool m_bControlPointer;
  private int m_iSize;

  public IntPtrDataProvider(IntPtr heapHandle)
  {
    this.m_ptrHeapHandle = heapHandle;
    this.m_bControlPointer = true;
  }

  ~IntPtrDataProvider()
  {
    if (!(this.m_ptrData != IntPtr.Zero))
      return;
    this.Dispose(false);
  }

  public IntPtr DataPointer
  {
    get => this.m_ptrData;
    set
    {
      this.m_ptrData = value;
      this.m_lPointer = this.m_ptrData.ToInt64();
    }
  }

  public override int Capacity => this.m_iSize;

  public IntPtr HeapHandle => this.m_ptrHeapHandle;

  public override bool IsCleared => this.m_ptrData == IntPtr.Zero;

  public override byte ReadByte(int iOffset) => Marshal.ReadByte(this.m_ptrData, iOffset);

  public override short ReadInt16(int iOffset) => Marshal.ReadInt16(this.m_ptrData, iOffset);

  public override int ReadInt32(int iOffset) => Marshal.ReadInt32(this.m_ptrData, iOffset);

  public override long ReadInt64(int iOffset) => Marshal.ReadInt64(this.m_ptrData, iOffset);

  public override void CopyTo(
    int iSourceOffset,
    byte[] arrDestination,
    int iDestOffset,
    int iLength)
  {
    Marshal.Copy((IntPtr) (this.m_lPointer + (long) iSourceOffset), arrDestination, iDestOffset, iLength);
  }

  public override void Read(BinaryReader reader, int iOffset, int iLength, byte[] arrBuffer)
  {
    int length = arrBuffer.Length;
    long lPointer = this.m_lPointer;
    while (iLength > 0)
    {
      int num = length > iLength ? iLength : length;
      reader.Read(arrBuffer, 0, num);
      IntPtr destination = (IntPtr) (lPointer + (long) iOffset);
      Marshal.Copy(arrBuffer, 0, destination, num);
      iLength -= num;
      iOffset += num;
    }
  }

  public override string ReadString(
    int offset,
    int stringLength,
    Encoding encoding,
    bool isUnicode)
  {
    IntPtr ptr = (IntPtr) (this.m_lPointer + (long) offset);
    string str;
    if (isUnicode)
      str = Marshal.PtrToStringUni(ptr, stringLength / 2);
    else if (encoding == Encoding.Default)
    {
      str = Marshal.PtrToStringAnsi(ptr, stringLength);
    }
    else
    {
      byte[] numArray = new byte[stringLength];
      this.ReadArray(offset, numArray, stringLength);
      str = encoding.GetString(numArray);
    }
    return str;
  }

  public override void WriteByte(int iOffset, byte value)
  {
    Marshal.WriteByte(this.m_ptrData, iOffset, value);
  }

  public override void WriteInt16(int iOffset, short value)
  {
    Marshal.WriteInt16(this.m_ptrData, iOffset, value);
  }

  [CLSCompliant(false)]
  public override void WriteUInt16(int iOffset, ushort value)
  {
    this.WriteInt16(iOffset, (short) value);
  }

  public override void WriteInt32(int iOffset, int value)
  {
    Marshal.WriteInt32(this.m_ptrData, iOffset, value);
  }

  public override void WriteInt64(int iOffset, long value)
  {
    Marshal.WriteInt64(this.m_ptrData, iOffset, value);
  }

  public override void WriteBit(int offset, bool value, int bitPos)
  {
    if (bitPos < 0 || bitPos > 7)
      throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position can be zero or greater than 7.");
    byte num1 = this.ReadByte(offset);
    byte num2 = !value ? (byte) ((uint) num1 & (uint) (byte) ~(1 << bitPos)) : (byte) ((uint) num1 | (uint) (byte) (1 << bitPos));
    this.WriteByte(offset, num2);
  }

  public override void WriteDouble(int iOffset, double value)
  {
    Marshal.Copy(BitConverter.GetBytes(value), 0, (IntPtr) (this.m_lPointer + (long) iOffset), 8);
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
        Marshal.WriteByte(this.m_ptrData, offset, unicode ? (byte) 1 : (byte) 0);
        ++offset;
        int length = bytes.Length;
        IntPtr destination = (IntPtr) (this.m_lPointer + (long) offset);
        Marshal.Copy(bytes, 0, destination, length);
        offset += length;
        break;
    }
  }

  public override void WriteBytes(int offset, byte[] value, int pos, int length)
  {
    if (length == 0)
      return;
    IntPtr destination = (IntPtr) (this.m_lPointer + (long) offset);
    Marshal.Copy(value, pos, destination, length);
  }

  protected override void OnDispose()
  {
    if (!(this.m_ptrData != IntPtr.Zero))
      return;
    if (this.m_bControlPointer)
    {
      if (this.m_ptrHeapHandle != IntPtr.Zero)
        Heap.HeapFree(this.m_ptrHeapHandle, 0, this.m_ptrData);
      else
        Marshal.FreeHGlobal(this.m_ptrData);
      if (this.m_iSize > 0)
        GC.RemoveMemoryPressure((long) this.m_iSize);
    }
    this.m_ptrHeapHandle = IntPtr.Zero;
    this.m_ptrData = IntPtr.Zero;
    this.m_lPointer = 0L;
    this.m_iSize = 0;
  }

  public override int EnsureCapacity(int size) => this.EnsureCapacity(size, 0);

  public override int EnsureCapacity(int size, int forceAdd)
  {
    if (!this.m_bControlPointer || size <= this.m_iSize)
      return this.m_iSize;
    if (this.m_ptrHeapHandle == IntPtr.Zero)
    {
      this.m_ptrData = this.m_iSize > 0 ? Marshal.ReAllocHGlobal(this.m_ptrData, (IntPtr) size) : Marshal.AllocHGlobal(size);
    }
    else
    {
      size += forceAdd;
      this.m_ptrData = this.m_iSize > 0 ? Heap.HeapReAlloc(this.m_ptrHeapHandle, 0, this.m_ptrData, size) : Heap.HeapAlloc(this.m_ptrHeapHandle, 0, size);
    }
    this.m_lPointer = this.m_ptrData.ToInt64();
    if (this.m_lPointer == 0L)
      throw new OutOfMemoryException();
    GC.AddMemoryPressure((long) (size - this.m_iSize));
    this.m_iSize = size;
    return size;
  }

  public override void Clear()
  {
    if (!this.m_bControlPointer || this.m_iSize <= 0)
      return;
    if (this.m_ptrHeapHandle != IntPtr.Zero)
      Heap.HeapFree(this.m_ptrHeapHandle, 0, this.m_ptrData);
    else
      Marshal.FreeHGlobal(this.m_ptrData);
    this.m_ptrData = IntPtr.Zero;
    this.m_lPointer = 0L;
    this.m_iSize = 0;
  }

  public override void CopyTo(
    int iSourceOffset,
    DataProvider destination,
    int iDestOffset,
    int iLength)
  {
    IntPtr ptrSource = (IntPtr) (this.m_lPointer + (long) iSourceOffset);
    Memory.CopyMemory((IntPtr) (((IntPtrDataProvider) destination).m_lPointer + (long) iDestOffset), ptrSource, iLength);
  }

  public override void ZeroMemory()
  {
    if (this.m_iSize <= 0)
      return;
    Memory.RtlZeroMemory(this.m_ptrData, this.m_iSize);
  }

  public override void MoveMemory(int iDestOffset, int iSourceOffset, int iMemorySize)
  {
    if (iMemorySize < 0)
      Debugger.Break();
    Memory.RtlMoveMemory((IntPtr) (this.m_lPointer + (long) iDestOffset), (IntPtr) (this.m_lPointer + (long) iSourceOffset), iMemorySize);
  }

  public override void CopyMemory(int iDestOffset, int iSourceOffset, int iMemorySize)
  {
    Memory.CopyMemory((IntPtr) (this.m_lPointer + (long) iDestOffset), (IntPtr) (this.m_lPointer + (long) iSourceOffset), iMemorySize);
  }

  public override DataProvider CreateProvider()
  {
    return (DataProvider) new IntPtrDataProvider(this.HeapHandle);
  }
}
