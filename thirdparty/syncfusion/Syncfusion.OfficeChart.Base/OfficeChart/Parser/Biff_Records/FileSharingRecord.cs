// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.FileSharingRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.FileSharing)]
[CLSCompliant(false)]
internal class FileSharingRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 2)]
  private ushort m_usRecommendReadOnly;
  [BiffRecordPos(2, 2)]
  private ushort m_usHashPassword;
  [BiffRecordPos(4, 2, TFieldType.String16Bit)]
  private string m_strCreatorName;

  public ushort RecommendReadOnly
  {
    get => this.m_usRecommendReadOnly;
    set => this.m_usRecommendReadOnly = value;
  }

  public ushort HashPassword
  {
    get => this.m_usHashPassword;
    set => this.m_usHashPassword = value;
  }

  public string CreatorName
  {
    get => this.m_strCreatorName;
    set => this.m_strCreatorName = value;
  }

  public override int MinimumRecordSize => 6;

  public FileSharingRecord()
  {
  }

  public FileSharingRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public FileSharingRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usRecommendReadOnly = provider.ReadUInt16(iOffset);
    this.m_usHashPassword = provider.ReadUInt16(iOffset + 2);
    this.m_strCreatorName = provider.ReadString16Bit(iOffset + 4, out int _);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usRecommendReadOnly);
    provider.WriteUInt16(iOffset + 2, this.m_usHashPassword);
    this.m_iLength = 4;
    this.m_iLength += provider.WriteString16Bit(iOffset + 4, this.m_strCreatorName);
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    int length = this.m_strCreatorName != null ? this.m_strCreatorName.Length : 0;
    return 4 + (length > 0 ? 3 + length * 2 : 2);
  }
}
