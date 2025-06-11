// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartSeriesTextRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartSeriesText)]
[CLSCompliant(false)]
internal class ChartSeriesTextRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 3;
  [BiffRecordPos(0, 2)]
  private ushort m_usTextId;
  [BiffRecordPos(2, 1, TFieldType.String)]
  private string m_strText = string.Empty;

  public ushort TextId
  {
    get => this.m_usTextId;
    set => this.m_usTextId = value;
  }

  public string Text
  {
    get => this.m_strText;
    set => this.m_strText = value;
  }

  public override int MinimumRecordSize => 3;

  public ChartSeriesTextRecord()
  {
  }

  public ChartSeriesTextRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartSeriesTextRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usTextId = provider.ReadUInt16(iOffset);
    this.m_strText = provider.ReadString8Bit(iOffset + 2, out int _);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usTextId);
    int num = iOffset;
    iOffset += 2;
    provider.WriteString8BitUpdateOffset(ref iOffset, this.m_strText);
    this.m_iLength = iOffset - num;
  }

  public override int GetStoreSize(OfficeVersion version) => 4 + this.m_strText.Length * 2;
}
