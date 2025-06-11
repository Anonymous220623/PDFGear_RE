// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.PropertyModifierStructure
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
[StructLayout(LayoutKind.Explicit)]
internal struct PropertyModifierStructure
{
  private const int DEF_BIT_COMPLEX = 0;
  private const int DEF_RECORD_SIZE = 2;
  [FieldOffset(0)]
  private ushort m_usOptions;

  internal bool IsComplex
  {
    get => BaseWordRecord.GetBit((int) this.m_usOptions, 0);
    set => this.m_usOptions = (ushort) BaseWordRecord.SetBit((int) this.m_usOptions, 0, value);
  }

  internal int Length => 2;

  internal void Parse(byte[] arrData, ref int iOffset)
  {
    this.m_usOptions = ByteConverter.ReadUInt16(arrData, ref iOffset);
  }

  internal void Save(byte[] arrData, ref int iOffset)
  {
    ByteConverter.WriteUInt16(arrData, ref iOffset, this.m_usOptions);
  }
}
