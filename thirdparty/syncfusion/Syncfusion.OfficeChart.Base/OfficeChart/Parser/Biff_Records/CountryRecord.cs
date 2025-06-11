// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.CountryRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.Country)]
[CLSCompliant(false)]
internal class CountryRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 4;
  [BiffRecordPos(0, 2)]
  private ushort m_usDefault = 1;
  [BiffRecordPos(2, 2)]
  private ushort m_usCurrent = 1;

  public ushort DefaultCountry
  {
    get => this.m_usDefault;
    set => this.m_usDefault = value;
  }

  public ushort CurrentCountry
  {
    get => this.m_usCurrent;
    set => this.m_usCurrent = value;
  }

  public override int MinimumRecordSize => 4;

  public override int MaximumRecordSize => 4;

  public CountryRecord()
  {
  }

  public CountryRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public CountryRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usDefault = provider.ReadUInt16(iOffset);
    this.m_usCurrent = provider.ReadUInt16(iOffset + 2);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usDefault);
    provider.WriteUInt16(iOffset + 2, this.m_usCurrent);
    this.m_iLength = 4;
  }

  public override int GetStoreSize(OfficeVersion version) => 4;
}
