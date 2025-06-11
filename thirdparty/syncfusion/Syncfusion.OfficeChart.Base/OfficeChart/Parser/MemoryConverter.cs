// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.MemoryConverter
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Runtime.InteropServices;
using System.Security;

#nullable disable
namespace Syncfusion.OfficeChart.Parser;

internal class MemoryConverter
{
  private const int DEF_MEMORY_BLOCK_SIZE = 8228;
  private const int DEF_MIN_BLOCK_SIZE = 1024 /*0x0400*/;
  private const int DEF_MAX_BLOCK_SIZE = 2147483647 /*0x7FFFFFFF*/;
  private const string DEF_OUT_OF_MEMORY_MSG = "Application was unable to allocate memory block";
  private IntPtr m_memoryBlock;
  private int m_iMemoryBlockSize;

  [SecurityCritical]
  public MemoryConverter()
    : this(8228)
  {
  }

  [SecurityCritical]
  public MemoryConverter(int iMemoryBlockSize)
  {
    if (iMemoryBlockSize < 1024 /*0x0400*/)
      iMemoryBlockSize = 1024 /*0x0400*/;
    this.m_memoryBlock = iMemoryBlockSize <= int.MaxValue ? Marshal.AllocCoTaskMem(iMemoryBlockSize) : throw new ArgumentOutOfRangeException("iMemoryBlock", "Value cannot be greater " + int.MaxValue.ToString());
    this.m_iMemoryBlockSize = iMemoryBlockSize;
    if (this.m_memoryBlock.ToInt64() == 0L)
      throw new OutOfMemoryException("Application was unable to allocate memory block");
  }

  [SecurityCritical]
  public void EnsureMemoryBlockSize(int iDesiredSize)
  {
    if (iDesiredSize <= this.m_iMemoryBlockSize)
      return;
    this.m_memoryBlock = iDesiredSize <= int.MaxValue ? Marshal.ReAllocCoTaskMem(this.m_memoryBlock, iDesiredSize) : throw new ArgumentOutOfRangeException(nameof (iDesiredSize), "Value cannot be greater than " + (object) int.MaxValue);
    this.m_iMemoryBlockSize = iDesiredSize;
    if (this.m_memoryBlock.ToInt64() == 0L)
      throw new OutOfMemoryException("Application was unable to allocate memory block");
  }

  [SecurityCritical]
  public void CopyFrom(byte[] arrData)
  {
    int num = arrData != null ? arrData.Length : throw new ArgumentNullException(nameof (arrData));
    if (num == 0)
      return;
    this.EnsureMemoryBlockSize(num);
    Marshal.Copy(arrData, 0, this.m_memoryBlock, num);
  }

  [SecurityCritical]
  public void CopyFrom(byte[] arrData, int iStartIndex)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iStartIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (iStartIndex));
    int num = arrData.Length - iStartIndex;
    if (num == 0)
      return;
    this.EnsureMemoryBlockSize(num);
    Marshal.Copy(arrData, iStartIndex, this.m_memoryBlock, num);
  }

  [SecurityCritical]
  public void CopyFrom(byte[] arrData, int iStartIndex, int iCount)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iStartIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (iStartIndex));
    if (iCount <= 0)
      return;
    if (arrData.Length - iStartIndex < iCount)
      throw new ArgumentOutOfRangeException("iCount is too large");
    this.EnsureMemoryBlockSize(iCount);
    Marshal.Copy(arrData, iStartIndex, this.m_memoryBlock, iCount);
  }

  [SecurityCritical]
  public object CopyTo(Type destType)
  {
    return !(destType == (Type) null) ? Marshal.PtrToStructure(this.m_memoryBlock, destType) : throw new ArgumentNullException("destination");
  }

  [SecurityCritical]
  public void CopyTo(object destination)
  {
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    Marshal.PtrToStructure(this.m_memoryBlock, destination);
  }

  [SecurityCritical]
  public void Copy(byte[] arrData, object destination)
  {
    this.CopyFrom(arrData);
    this.CopyTo(destination);
  }

  [SecurityCritical]
  public void Copy(byte[] arrData, int iStartIndex, object destination)
  {
    this.CopyFrom(arrData, iStartIndex);
    this.CopyTo(destination);
  }

  [SecurityCritical]
  public void Copy(byte[] arrData, int iStartIndex, int iCount, object destination)
  {
    this.CopyFrom(arrData, iStartIndex, iCount);
    this.CopyTo(destination);
  }

  [SecurityCritical]
  public void CopyObject(object source, byte[] arrDestination, int iCount)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (arrDestination == null)
      throw new ArgumentNullException(nameof (arrDestination));
    int iDesiredSize = iCount;
    if (arrDestination.Length < iDesiredSize)
      throw new ArgumentException(nameof (arrDestination), "Array too short");
    this.EnsureMemoryBlockSize(iDesiredSize);
    Marshal.StructureToPtr(source, this.m_memoryBlock, false);
    Marshal.Copy(this.m_memoryBlock, arrDestination, 0, iCount);
    Marshal.DestroyStructure(this.m_memoryBlock, source.GetType());
  }
}
