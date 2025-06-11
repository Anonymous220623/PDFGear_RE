// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.CompatibilityRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.Compatibility)]
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
    ExcelVersion version)
  {
    this.m_bNoCompCheck = arrData.ReadUInt32(iOffset + 12);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_header.Type);
    provider.WriteUInt16(iOffset + 2, this.m_header.Attributes);
    provider.WriteInt64(iOffset + 4, 0L);
    provider.WriteUInt32(iOffset + 12, this.m_bNoCompCheck);
  }

  public override int GetStoreSize(ExcelVersion version) => 16 /*0x10*/;
}
