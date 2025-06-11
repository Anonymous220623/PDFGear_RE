// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.UnmanagedArray
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class UnmanagedArray : IDisposable
{
  private IntPtr m_ptrMemory = IntPtr.Zero;
  private int m_iMemorySize;

  public UnmanagedArray()
  {
  }

  public UnmanagedArray(int memorySize, bool bZeroMemory) => this.Resize(memorySize, bZeroMemory);

  ~UnmanagedArray() => this.Dispose();

  public void Dispose()
  {
    if (!(this.m_ptrMemory != IntPtr.Zero))
      return;
    Marshal.FreeHGlobal(this.m_ptrMemory);
    this.m_ptrMemory = IntPtr.Zero;
    this.m_iMemorySize = 0;
    GC.SuppressFinalize((object) this);
  }

  public int GetInt32(int index)
  {
    int ofs = 4 * index;
    return ofs <= this.m_iMemorySize ? Marshal.ReadInt32(this.m_ptrMemory, ofs) : throw new ArgumentOutOfRangeException(nameof (index));
  }

  public int GetInt16(int index)
  {
    int ofs = 2 * index;
    return ofs <= this.m_iMemorySize ? (int) Marshal.ReadInt16(this.m_ptrMemory, ofs) : throw new ArgumentOutOfRangeException(nameof (index));
  }

  public byte GetByte(int index)
  {
    return index <= this.m_iMemorySize ? Marshal.ReadByte(this.m_ptrMemory, index) : throw new ArgumentOutOfRangeException(nameof (index));
  }

  public void SetInt32(int index, int value)
  {
    int ofs = 4 * index;
    if (ofs + 4 > this.m_iMemorySize)
      throw new ArgumentOutOfRangeException(nameof (index));
    Marshal.WriteInt32(this.m_ptrMemory, ofs, value);
  }

  public void SetInt16(int index, short value)
  {
    int ofs = 2 * index;
    if (ofs + 2 > this.m_iMemorySize)
      throw new ArgumentOutOfRangeException(nameof (index));
    Marshal.WriteInt16(this.m_ptrMemory, ofs, value);
  }

  public void SetByte(int index, byte value)
  {
    if (index > this.m_iMemorySize)
      throw new ArgumentOutOfRangeException(nameof (index));
    Marshal.WriteByte(this.m_ptrMemory, index, value);
  }

  public void Resize(int iDesiredSize, bool bZeroMemory)
  {
    if (iDesiredSize <= 0)
      throw new ArgumentOutOfRangeException(nameof (iDesiredSize));
    this.m_ptrMemory = this.m_ptrMemory == IntPtr.Zero ? (this.m_ptrMemory = Marshal.AllocHGlobal(iDesiredSize)) : Marshal.ReAllocHGlobal(this.m_ptrMemory, (IntPtr) iDesiredSize);
    if (bZeroMemory)
      Memory.RtlZeroMemory((IntPtr) (this.m_ptrMemory.ToInt64() + (long) this.m_iMemorySize), iDesiredSize - this.m_iMemorySize);
    this.m_iMemorySize = iDesiredSize;
  }

  public void CopyFrom(UnmanagedArray source)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    Memory.CopyMemory(this.m_ptrMemory, source.m_ptrMemory, this.m_iMemorySize);
  }

  public void Clear() => Memory.RtlZeroMemory(this.m_ptrMemory, this.m_iMemorySize);
}
