// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.PieceDescriptorStructure
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
[StructLayout(LayoutKind.Explicit)]
internal class PieceDescriptorStructure : IDataStructure
{
  private const int DEF_PRM_OFFSET = 6;
  private const int DEF_RECORD_SIZE = 8;
  [FieldOffset(0)]
  private ushort m_usOptions;
  [FieldOffset(2)]
  private uint m_fc;
  [FieldOffset(6)]
  private PropertyModifierStructure m_prm = new PropertyModifierStructure();

  internal ushort Options
  {
    get => this.m_usOptions;
    set => this.m_usOptions = value;
  }

  internal uint FileOffset
  {
    get => this.m_fc;
    set => this.m_fc = value;
  }

  internal PropertyModifierStructure PropertyModifier
  {
    get => this.m_prm;
    set => this.m_prm = value;
  }

  public int Length => 6 + this.m_prm.Length;

  public void Parse(byte[] arrData, int iOffset)
  {
    this.m_usOptions = ByteConverter.ReadUInt16(arrData, ref iOffset);
    this.m_fc = ByteConverter.ReadUInt32(arrData, ref iOffset);
    this.m_prm.Parse(arrData, ref iOffset);
  }

  public int Save(byte[] arrData, int iOffset)
  {
    ByteConverter.WriteUInt16(arrData, ref iOffset, this.m_usOptions);
    ByteConverter.WriteUInt32(arrData, ref iOffset, this.m_fc);
    this.m_prm.Save(arrData, ref iOffset);
    return 8;
  }
}
