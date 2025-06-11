// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.ParagraphHeightStructure
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
[StructLayout(LayoutKind.Explicit)]
internal class ParagraphHeightStructure : IDataStructure
{
  private const int DEF_RECORD_SIZE = 12;
  [FieldOffset(0)]
  internal uint Options;
  [FieldOffset(4)]
  internal int Width;
  [FieldOffset(8)]
  internal int Height;

  public int Length => 12;

  public void Parse(byte[] arrData, int iOffset)
  {
    this.Options = ByteConverter.ReadUInt32(arrData, ref iOffset);
    this.Width = ByteConverter.ReadInt32(arrData, ref iOffset);
    this.Height = ByteConverter.ReadInt32(arrData, ref iOffset);
  }

  public int Save(byte[] arrData, int iOffset)
  {
    ByteConverter.WriteUInt32(arrData, ref iOffset, this.Options);
    ByteConverter.WriteInt32(arrData, ref iOffset, this.Width);
    ByteConverter.WriteInt32(arrData, ref iOffset, this.Height);
    return 12;
  }
}
