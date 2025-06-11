// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.SectionDescriptorStructure
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
[StructLayout(LayoutKind.Explicit)]
internal class SectionDescriptorStructure : IDataStructure
{
  private const int DEF_RECORD_SIZE = 12;
  [FieldOffset(0)]
  private short m_sInternal;
  [FieldOffset(2)]
  private uint m_fcSepx;
  [FieldOffset(6)]
  private short m_sInternal2;
  [FieldOffset(8)]
  private int m_fcMpr;

  internal short Internal1
  {
    get => this.m_sInternal;
    set => this.m_sInternal = value;
  }

  internal short Internal2
  {
    get => this.m_sInternal2;
    set => this.m_sInternal2 = value;
  }

  internal uint SepxPosition
  {
    get => this.m_fcSepx;
    set => this.m_fcSepx = value;
  }

  internal int MacPrintOffset
  {
    get => this.m_fcMpr;
    set => this.m_fcMpr = value;
  }

  public int Length => 12;

  public void Parse(byte[] arrData, int iOffset)
  {
    this.m_sInternal = ByteConverter.ReadInt16(arrData, ref iOffset);
    this.m_fcSepx = ByteConverter.ReadUInt32(arrData, ref iOffset);
    this.m_sInternal2 = ByteConverter.ReadInt16(arrData, ref iOffset);
    this.m_fcMpr = ByteConverter.ReadInt32(arrData, ref iOffset);
  }

  public int Save(byte[] arrData, int iOffset)
  {
    ByteConverter.WriteInt16(arrData, ref iOffset, this.m_sInternal);
    ByteConverter.WriteUInt32(arrData, ref iOffset, this.m_fcSepx);
    ByteConverter.WriteInt16(arrData, ref iOffset, this.m_sInternal2);
    ByteConverter.WriteInt32(arrData, ref iOffset, this.m_fcMpr);
    return 12;
  }
}
