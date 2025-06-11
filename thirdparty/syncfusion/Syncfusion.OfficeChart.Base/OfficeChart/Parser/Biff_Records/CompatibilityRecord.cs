// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.CompatibilityRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.Compatibility)]
[CLSCompliant(false)]
internal class CompatibilityRecord : BiffRecordRaw
{
  private FutureHeader m_header;
  private uint m_bNoCompCheck;

  public CompatibilityRecord()
  {
    this.m_header = new FutureHeader();
    this.m_header.Type = (ushort) 2188;
  }

  public uint NoComptabilityCheck
  {
    get => this.m_bNoCompCheck;
    set => this.m_bNoCompCheck = value;
  }

  public override void ParseStructure(
    DataProvider arrData,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_bNoCompCheck = arrData.ReadUInt32(iOffset + 12);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_header.Type);
    provider.WriteUInt16(iOffset + 2, this.m_header.Attributes);
    provider.WriteInt64(iOffset + 4, 0L);
    provider.WriteUInt32(iOffset + 12, this.m_bNoCompCheck);
  }

  public override int GetStoreSize(OfficeVersion version) => 16 /*0x10*/;
}
