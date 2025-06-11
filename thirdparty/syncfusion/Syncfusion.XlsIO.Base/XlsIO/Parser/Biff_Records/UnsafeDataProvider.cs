// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.UnsafeDataProvider
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public class UnsafeDataProvider(IntPtr heapHandle) : IntPtrDataProvider(heapHandle)
{
  public override DataProvider CreateProvider()
  {
    return (DataProvider) new UnsafeDataProvider(this.HeapHandle);
  }

  public override unsafe byte ReadByte(int iOffset)
  {
    return *(byte*) ((ulong) this.m_lPointer + (ulong) iOffset);
  }

  public override unsafe short ReadInt16(int iOffset)
  {
    return *(short*) ((ulong) this.m_lPointer + (ulong) iOffset);
  }

  public override unsafe int ReadInt32(int iOffset)
  {
    return *(int*) ((ulong) this.m_lPointer + (ulong) iOffset);
  }

  public override unsafe long ReadInt64(int iOffset)
  {
    return *(long*) ((ulong) this.m_lPointer + (ulong) iOffset);
  }

  public override unsafe double ReadDouble(int iOffset)
  {
    return *(double*) ((ulong) this.m_lPointer + (ulong) iOffset);
  }

  public override unsafe void WriteByte(int iOffset, byte value)
  {
    *(byte*) ((ulong) this.m_lPointer + (ulong) iOffset) = value;
  }

  public override unsafe void WriteInt16(int iOffset, short value)
  {
    *(short*) ((ulong) this.m_lPointer + (ulong) iOffset) = value;
  }

  public override unsafe void WriteInt32(int iOffset, int value)
  {
    *(int*) ((ulong) this.m_lPointer + (ulong) iOffset) = value;
  }

  public override unsafe void WriteInt64(int iOffset, long value)
  {
    *(long*) ((ulong) this.m_lPointer + (ulong) iOffset) = value;
  }

  public override unsafe void WriteDouble(int iOffset, double value)
  {
    *(double*) ((ulong) this.m_lPointer + (ulong) iOffset) = value;
  }
}
