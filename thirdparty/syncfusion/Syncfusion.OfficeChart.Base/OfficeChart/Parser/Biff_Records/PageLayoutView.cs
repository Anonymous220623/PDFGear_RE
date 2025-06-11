// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.PageLayoutView
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.PageLayoutView)]
[CLSCompliant(false)]
internal class PageLayoutView : BiffRecordRaw
{
  private const int DEF_FIXED_SIZE = 16 /*0x10*/;
  private ushort m_futureRecord = 2187;
  private ushort m_iScale;
  [BiffRecordPos(14, 0, TFieldType.Bit)]
  private bool m_bPageLayoutView;
  [BiffRecordPos(14, 1, TFieldType.Bit)]
  private bool m_bRulerVisible;
  [BiffRecordPos(14, 2, TFieldType.Bit)]
  private bool m_bWhiteSpaceHidden;

  internal ushort Scaling => this.m_iScale;

  internal bool LayoutView
  {
    get => this.m_bPageLayoutView;
    set => this.m_bPageLayoutView = value;
  }

  internal bool WhiteSpaceHidden
  {
    get => this.m_bWhiteSpaceHidden;
    set => this.m_bWhiteSpaceHidden = value;
  }

  internal bool RulerVisible
  {
    get => this.m_bRulerVisible;
    set => this.m_bRulerVisible = value;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    iOffset += 12;
    this.m_iScale = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_bPageLayoutView = provider.ReadBit(iOffset, 0);
    this.m_bRulerVisible = provider.ReadBit(iOffset, 1);
    this.m_bWhiteSpaceHidden = provider.ReadBit(iOffset, 2);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_futureRecord);
    provider.WriteUInt16(iOffset + 2, (ushort) 0);
    provider.WriteInt64(iOffset + 4, 0L);
    iOffset += 12;
    provider.WriteUInt16(iOffset, this.m_iScale);
    iOffset += 2;
    provider.WriteBit(iOffset, this.m_bPageLayoutView, 0);
    provider.WriteBit(iOffset, this.m_bRulerVisible, 1);
    provider.WriteBit(iOffset, this.m_bWhiteSpaceHidden, 2);
  }

  public override int GetStoreSize(OfficeVersion version) => 16 /*0x10*/;
}
