// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartSbaserefRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartSbaseref)]
[CLSCompliant(false)]
internal class ChartSbaserefRecord : BiffRecordRaw
{
  public const int DefaultRecordSize = 8;
  [BiffRecordPos(0, 2)]
  private ushort m_usFirstRow;
  [BiffRecordPos(2, 2)]
  private ushort m_usLastRow;
  [BiffRecordPos(4, 2)]
  private ushort m_usFirstColumn;
  [BiffRecordPos(6, 2)]
  private ushort m_usLastColumn;

  public ushort FirstRow
  {
    get => this.m_usFirstRow;
    set => this.m_usFirstRow = value;
  }

  public ushort LastRow
  {
    get => this.m_usLastRow;
    set => this.m_usLastRow = value;
  }

  public ushort FirstColumn
  {
    get => this.m_usFirstColumn;
    set => this.m_usFirstColumn = value;
  }

  public ushort LastColumn
  {
    get => this.m_usLastColumn;
    set => this.m_usLastColumn = value;
  }

  public override int MinimumRecordSize => 8;

  public override int MaximumRecordSize => 8;

  public ChartSbaserefRecord()
  {
  }

  public ChartSbaserefRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartSbaserefRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usFirstRow = provider.ReadUInt16(iOffset);
    this.m_usLastRow = provider.ReadUInt16(iOffset + 2);
    this.m_usFirstColumn = provider.ReadUInt16(iOffset + 4);
    this.m_usLastColumn = provider.ReadUInt16(iOffset + 6);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usFirstRow);
    provider.WriteUInt16(iOffset + 2, this.m_usLastRow);
    provider.WriteUInt16(iOffset + 4, this.m_usFirstColumn);
    provider.WriteUInt16(iOffset + 6, this.m_usLastColumn);
    this.m_iLength = 8;
  }

  public override int GetStoreSize(OfficeVersion version) => 8;
}
