// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.BreakDescriptorStructure
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
[StructLayout(LayoutKind.Explicit)]
internal class BreakDescriptorStructure : IDataStructure
{
  private const int DEF_RECORD_SIZE = 6;
  [FieldOffset(0)]
  internal short ipgd;
  [FieldOffset(0)]
  internal short itxbxs;
  [FieldOffset(2)]
  internal short dcpDepend;
  [FieldOffset(4)]
  internal byte iCol;
  [FieldOffset(5)]
  internal byte Options;

  public int Length => 6;

  public void Parse(byte[] arrData, int iOffset)
  {
    this.itxbxs = ByteConverter.ReadInt16(arrData, ref iOffset);
    this.dcpDepend = ByteConverter.ReadInt16(arrData, ref iOffset);
    this.iCol = arrData[iOffset];
    ++iOffset;
    this.Options = arrData[iOffset];
    ++iOffset;
  }

  public int Save(byte[] arr, int iOffset)
  {
    ByteConverter.WriteInt16(arr, ref iOffset, this.itxbxs);
    ByteConverter.WriteInt16(arr, ref iOffset, this.dcpDepend);
    arr[iOffset] = this.iCol;
    ++iOffset;
    arr[iOffset] = this.Options;
    ++iOffset;
    return 6;
  }
}
