// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.MemoryConverter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

internal class MemoryConverter
{
  private const int DEF_MEMORY_BLOCK_SIZE = 8228;
  private const int DEF_MIN_BLOCK_SIZE = 1024 /*0x0400*/;
  private const int DEF_MAX_BLOCK_SIZE = 2147483647 /*0x7FFFFFFF*/;
  private const string DEF_OUT_OF_MEMORY_MSG = "Application was unable to allocate memory block";
  private IntPtr m_memoryBlock;
  private int m_iMemoryBlockSize;
  [ThreadStatic]
  private static MemoryConverter m_instance;

  internal MemoryConverter()
    : this(8228)
  {
  }

  internal MemoryConverter(int iMemoryBlockSize)
  {
    if (iMemoryBlockSize < 1024 /*0x0400*/)
      iMemoryBlockSize = 1024 /*0x0400*/;
    this.m_memoryBlock = iMemoryBlockSize <= int.MaxValue ? Marshal.AllocCoTaskMem(iMemoryBlockSize) : throw new ArgumentOutOfRangeException("iMemoryBlock", "Value can not be greater " + int.MaxValue.ToString());
    this.m_iMemoryBlockSize = iMemoryBlockSize;
    if (this.m_memoryBlock.ToInt64() == 0L)
      throw new OutOfMemoryException("Application was unable to allocate memory block");
  }

  internal void EnsureMemoryBlockSize(int iDesiredSize)
  {
    if (iDesiredSize <= this.m_iMemoryBlockSize)
      return;
    this.m_memoryBlock = iDesiredSize <= int.MaxValue ? Marshal.ReAllocCoTaskMem(this.m_memoryBlock, iDesiredSize) : throw new ArgumentOutOfRangeException(nameof (iDesiredSize), (object) iDesiredSize, "Value can not be greater than " + (object) int.MaxValue);
    this.m_iMemoryBlockSize = iDesiredSize;
    if (this.m_memoryBlock.ToInt64() == 0L)
      throw new OutOfMemoryException("Application was unable to allocate memory block");
  }

  internal void CopyFrom(byte[] arrData)
  {
    int num = arrData != null ? arrData.Length : throw new ArgumentNullException(nameof (arrData));
    if (num == 0)
      return;
    this.EnsureMemoryBlockSize(num);
    Marshal.Copy(arrData, 0, this.m_memoryBlock, num);
  }

  internal void CopyFrom(byte[] arrData, int iStartIndex)
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

  internal void CopyFrom(byte[] arrData, int iStartIndex, int iCount)
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

  internal void CopyTo(object destination)
  {
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    Marshal.PtrToStructure(this.m_memoryBlock, destination);
  }

  internal void Copy(byte[] arrData, object destination)
  {
    this.CopyFrom(arrData);
    this.CopyTo(destination);
  }

  internal void Copy(byte[] arrData, int iStartIndex, object destination)
  {
    this.CopyFrom(arrData, iStartIndex);
    this.CopyTo(destination);
  }

  internal void Copy(byte[] arrData, int iStartIndex, int iCount, object destination)
  {
    this.CopyFrom(arrData, iStartIndex, iCount);
    this.CopyTo(destination);
  }

  internal void Copy(object source, byte[] arrDestination, int iStartIndex, int iLength)
  {
    if (arrDestination == null)
      throw new ArgumentNullException(nameof (arrDestination));
    if (iStartIndex < 0 || iStartIndex >= arrDestination.Length)
      throw new ArgumentOutOfRangeException(nameof (iStartIndex));
    if (iStartIndex + iLength > arrDestination.Length)
      throw new ArgumentOutOfRangeException(nameof (iLength));
    if (source == null)
      throw new ArgumentNullException("UnderlyingStructure");
    this.EnsureMemoryBlockSize(iLength);
    Marshal.StructureToPtr(source, this.m_memoryBlock, false);
    Marshal.Copy(this.m_memoryBlock, arrDestination, iStartIndex, iLength);
    Marshal.DestroyStructure(this.m_memoryBlock, source.GetType());
  }

  internal static void Close()
  {
    if (MemoryConverter.m_instance == null)
      return;
    MemoryConverter.m_instance = (MemoryConverter) null;
  }

  internal static MemoryConverter Instance
  {
    get
    {
      if (MemoryConverter.m_instance == null)
        MemoryConverter.m_instance = new MemoryConverter();
      return MemoryConverter.m_instance;
    }
  }
}
