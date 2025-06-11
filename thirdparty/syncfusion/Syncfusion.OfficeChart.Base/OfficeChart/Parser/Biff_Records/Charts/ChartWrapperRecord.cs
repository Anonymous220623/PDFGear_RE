// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartWrapperRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartWrapper)]
internal class ChartWrapperRecord : BiffRecordRaw, ICloneable
{
  private const int DEF_RECORD_OFFSET = 4;
  private BiffRecordRaw m_record;

  public ChartWrapperRecord()
  {
  }

  public ChartWrapperRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartWrapperRecord(int iReserve)
    : base(iReserve)
  {
  }

  public BiffRecordRaw Record
  {
    get => this.m_record;
    set => this.m_record = value != null ? value : throw new ArgumentNullException(nameof (value));
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_record = BiffRecordFactory.GetRecord(provider, iOffset + 4, version);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    int storeSize = this.m_record.GetStoreSize(version);
    this.m_iLength = 4 + storeSize + 4;
    provider.WriteUInt16(iOffset + 4, (ushort) this.m_record.TypeCode);
    provider.WriteUInt16(iOffset + 4 + 2, (ushort) storeSize);
    this.m_record.InfillInternalData(provider, iOffset + 4 + 4, version);
    provider.WriteUInt16(iOffset, (ushort) this.TypeCode);
    provider.WriteUInt16(iOffset + 2, (ushort) 0);
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    return 8 + this.m_record.GetStoreSize(version);
  }

  public new object Clone()
  {
    object obj = base.Clone();
    if (this.m_record != null)
      ((ChartWrapperRecord) obj).m_record = (BiffRecordRaw) this.m_record.Clone();
    return obj;
  }
}
