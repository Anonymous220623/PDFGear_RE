// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.SheetCenterRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.HCenter)]
[CLSCompliant(false)]
[Biff(TBIFFRecord.VCenter)]
internal class SheetCenterRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 2)]
  private ushort m_usCenter;

  public ushort IsCenter
  {
    get => this.m_usCenter;
    set => this.m_usCenter = value;
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 2;

  public SheetCenterRecord()
  {
  }

  public SheetCenterRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public SheetCenterRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usCenter = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usCenter);
  }
}
