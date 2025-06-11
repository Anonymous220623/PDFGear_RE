// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ExtendedFormatCRC
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.ExtendedFormatCRC)]
[CLSCompliant(false)]
internal class ExtendedFormatCRC : BiffRecordRaw
{
  private FutureHeader m_header;
  private ushort m_usXFCount;
  private uint m_uiCRC;
  private WorkbookImpl m_book;

  public ExtendedFormatCRC()
  {
    this.m_header = new FutureHeader();
    this.m_header.Type = (ushort) 2172;
    this.m_usXFCount = (ushort) 16 /*0x10*/;
  }

  public ushort XFCount
  {
    get => this.m_usXFCount;
    set => this.m_usXFCount = value;
  }

  public uint CRCChecksum
  {
    get => this.m_uiCRC;
    set => this.m_uiCRC = value;
  }

  public override void ParseStructure(
    DataProvider arrData,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_header.Type = arrData.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_header.Attributes = (ushort) arrData.ReadByte(iOffset);
    iOffset += 2;
    int num1 = (int) arrData.ReadByte(iOffset);
    iOffset += 8;
    int num2 = (int) arrData.ReadByte(iOffset);
    iOffset += 2;
    this.m_usXFCount = arrData.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_uiCRC = arrData.ReadUInt32(iOffset);
    iOffset += 4;
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    ushort num = 0;
    provider.WriteUInt16(iOffset, this.m_header.Type);
    provider.WriteUInt16(iOffset + 2, this.m_header.Attributes);
    provider.WriteInt64(iOffset + 4, (long) num);
    provider.WriteUInt16(iOffset + 12, num);
    provider.WriteUInt16(iOffset + 14, this.m_usXFCount);
    provider.WriteUInt32(iOffset + 16 /*0x10*/, this.m_uiCRC);
  }

  public override int GetStoreSize(OfficeVersion version) => 20;

  public override int GetHashCode()
  {
    return this.m_header.Type.GetHashCode() ^ this.m_header.Attributes.GetHashCode() ^ this.m_usXFCount.GetHashCode() ^ this.m_uiCRC.GetHashCode();
  }

  public override object Clone() => (object) new ExtendedFormatCRC();
}
