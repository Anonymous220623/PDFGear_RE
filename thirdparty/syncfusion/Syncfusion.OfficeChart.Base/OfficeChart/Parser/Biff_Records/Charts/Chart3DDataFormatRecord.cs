// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.Chart3DDataFormatRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.Chart3DDataFormat)]
[CLSCompliant(false)]
internal class Chart3DDataFormatRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 2;
  [BiffRecordPos(0, 1)]
  private byte m_DataFormatBase;
  [BiffRecordPos(1, 1)]
  private byte m_DataFormatTop;

  public OfficeBaseFormat DataFormatBase
  {
    get => (OfficeBaseFormat) this.m_DataFormatBase;
    set => this.m_DataFormatBase = (byte) value;
  }

  public OfficeTopFormat DataFormatTop
  {
    get => (OfficeTopFormat) this.m_DataFormatTop;
    set => this.m_DataFormatTop = (byte) value;
  }

  public Chart3DDataFormatRecord()
  {
  }

  public Chart3DDataFormatRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public Chart3DDataFormatRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_DataFormatBase = provider.ReadByte(iOffset);
    this.m_DataFormatTop = provider.ReadByte(iOffset + 1);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteByte(iOffset, this.m_DataFormatBase);
    provider.WriteByte(iOffset + 1, this.m_DataFormatTop);
  }

  public override int GetStoreSize(OfficeVersion version) => 2;
}
