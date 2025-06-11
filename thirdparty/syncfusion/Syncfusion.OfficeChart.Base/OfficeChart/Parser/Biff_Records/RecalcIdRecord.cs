// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.RecalcIdRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.RecalcId)]
[CLSCompliant(false)]
internal class RecalcIdRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 4)]
  private uint m_record = 449;
  [BiffRecordPos(4, 8)]
  private uint m_dwBuild;

  public uint RecordId
  {
    get => this.m_record;
    set => this.m_record = value;
  }

  public uint CalcIdentifier
  {
    get => this.m_dwBuild;
    set => this.m_dwBuild = value;
  }

  public override int MinimumRecordSize => 8;

  public override int MaximumRecordSize => 8;

  public RecalcIdRecord()
  {
  }

  public RecalcIdRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public RecalcIdRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_record = (uint) provider.ReadUInt16(iOffset);
    this.m_dwBuild = provider.ReadUInt32(iOffset + 4);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt32(iOffset, this.m_record);
    provider.WriteUInt32(iOffset + 4, this.m_dwBuild);
  }
}
