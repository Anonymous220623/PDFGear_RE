// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartObjectLinkRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartObjectLink)]
[CLSCompliant(false)]
internal class ChartObjectLinkRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 6;
  [BiffRecordPos(0, 2)]
  private ushort m_usLinkObject;
  [BiffRecordPos(2, 2)]
  private ushort m_usLinkIndex1;
  [BiffRecordPos(4, 2)]
  private ushort m_usLinkIndex2;

  public ExcelObjectTextLink LinkObject
  {
    get => (ExcelObjectTextLink) this.m_usLinkObject;
    set => this.m_usLinkObject = (ushort) value;
  }

  public ushort SeriesNumber
  {
    get => this.m_usLinkIndex1;
    set => this.m_usLinkIndex1 = value;
  }

  public ushort DataPointNumber
  {
    get => this.m_usLinkIndex2;
    set => this.m_usLinkIndex2 = value;
  }

  public override int MinimumRecordSize => 6;

  public override int MaximumRecordSize => 6;

  public ChartObjectLinkRecord()
  {
  }

  public ChartObjectLinkRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartObjectLinkRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usLinkObject = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usLinkIndex1 = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usLinkIndex2 = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usLinkObject);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usLinkIndex1);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usLinkIndex2);
  }

  public override int GetStoreSize(OfficeVersion version) => 6;
}
