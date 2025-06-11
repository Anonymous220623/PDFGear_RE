// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.SheetLayoutRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.SheetLayout)]
[CLSCompliant(false)]
internal class SheetLayoutRecord : BiffRecordRaw
{
  public const int DefaultRecordSize = 20;
  [BiffRecordPos(0, 2, true)]
  private short m_id;
  [BiffRecordPos(2, 4, true)]
  private int m_iReserved1;
  [BiffRecordPos(6, 4, true)]
  private int m_iReserved2;
  [BiffRecordPos(10, 2, true)]
  private short m_sReserved3;
  [BiffRecordPos(12, 4, true)]
  private int m_iUnknown = 20;
  [BiffRecordPos(16 /*0x10*/, 4, true)]
  private int m_iColorIndex;

  public SheetLayoutRecord() => this.m_id = (short) this.TypeCode;

  public SheetLayoutRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public SheetLayoutRecord(int iReserve)
    : base(iReserve)
  {
  }

  public short Id
  {
    get => this.m_id;
    set => this.m_id = value;
  }

  public int Reserved1
  {
    get => this.m_iReserved1;
    set => this.m_iReserved1 = value;
  }

  public int Reserved2
  {
    get => this.m_iReserved2;
    set => this.m_iReserved2 = value;
  }

  public short Reserved3
  {
    get => this.m_sReserved3;
    set => this.m_sReserved3 = value;
  }

  public int Unknown
  {
    get => this.m_iUnknown;
    set => this.m_iUnknown = value;
  }

  public int ColorIndex
  {
    get => this.m_iColorIndex;
    set => this.m_iColorIndex = value;
  }

  public override int MinimumRecordSize => 20;

  public override int MaximumRecordSize => 20;

  public override int MaximumMemorySize => 20;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_id = provider.ReadInt16(iOffset);
    this.m_iReserved1 = provider.ReadInt32(iOffset + 2);
    this.m_iReserved2 = provider.ReadInt32(iOffset + 6);
    this.m_sReserved3 = provider.ReadInt16(iOffset + 10);
    this.m_iUnknown = provider.ReadInt32(iOffset + 12);
    this.m_iColorIndex = provider.ReadInt32(iOffset + 16 /*0x10*/);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteInt16(iOffset, this.m_id);
    provider.WriteInt32(iOffset + 2, this.m_iReserved1);
    provider.WriteInt32(iOffset + 6, this.m_iReserved2);
    provider.WriteInt16(iOffset + 10, this.m_sReserved3);
    provider.WriteInt32(iOffset + 12, this.m_iUnknown);
    provider.WriteInt32(iOffset + 16 /*0x10*/, this.m_iColorIndex);
    this.m_iLength = 20;
  }

  public override int GetStoreSize(OfficeVersion version) => 20;
}
