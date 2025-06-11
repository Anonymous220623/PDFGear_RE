// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartBoppCustomRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartBoppCustom)]
internal class ChartBoppCustomRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 2)]
  private ushort m_usQuantity;
  private byte[] m_bits;

  public ushort Counter => this.m_usQuantity;

  public byte[] BitFields
  {
    get => this.m_bits;
    set
    {
      this.m_bits = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_usQuantity = (ushort) value.Length;
    }
  }

  public ChartBoppCustomRecord()
  {
  }

  public ChartBoppCustomRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartBoppCustomRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usQuantity = provider.ReadUInt16(iOffset);
    this.m_bits = new byte[(int) this.m_usQuantity];
    provider.ReadArray(iOffset + 2, this.m_bits);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usQuantity);
    provider.WriteBytes(iOffset + 2, this.m_bits, 0, this.m_bits.Length);
    this.m_iLength = this.m_bits.Length + 2;
  }

  public override int GetStoreSize(OfficeVersion version) => 2 + (int) this.m_usQuantity;

  public override object Clone()
  {
    ChartBoppCustomRecord boppCustomRecord = (ChartBoppCustomRecord) base.Clone();
    boppCustomRecord.m_bits = CloneUtils.CloneByteArray(this.m_bits);
    return (object) boppCustomRecord;
  }
}
