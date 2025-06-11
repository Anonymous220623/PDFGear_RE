// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.UnknownEndRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
internal class UnknownEndRecord : BiffRecordRaw
{
  private const int DEF_UNKNOWN1 = 449;
  private const int DEF_UNKNOWN2 = 144525;
  private const int DefaultRecordSize = 8;
  [BiffRecordPos(0, 4, true)]
  private int m_iUnknown1;
  [BiffRecordPos(4, 4, true)]
  private int m_iUnknown2;

  public int Unknown1
  {
    get => this.m_iUnknown1;
    set => this.m_iUnknown1 = value;
  }

  public int Unknown2
  {
    get => this.m_iUnknown2;
    set => this.m_iUnknown2 = value;
  }

  public override int MinimumRecordSize => 8;

  public override int MaximumRecordSize => 8;

  public UnknownEndRecord()
  {
  }

  public UnknownEndRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public UnknownEndRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_iUnknown1 = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iUnknown2 = provider.ReadInt32(iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iUnknown1 = 449;
    this.m_iUnknown2 = 144525;
    this.m_iLength = 8;
    provider.WriteInt32(iOffset, this.m_iUnknown1);
    provider.WriteInt32(iOffset + 4, this.m_iUnknown2);
  }
}
