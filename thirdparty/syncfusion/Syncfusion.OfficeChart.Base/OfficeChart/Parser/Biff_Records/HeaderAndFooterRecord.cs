// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.HeaderAndFooterRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.HeaderFooter)]
internal class HeaderAndFooterRecord : BiffRecordRaw
{
  private const int Record2003Length = 22;
  private const int Record2010Length = 38;
  [BiffRecordPos(28, 0, TFieldType.Bit)]
  private bool m_bfHFDiffOddEven;
  [BiffRecordPos(28, 1, TFieldType.Bit)]
  private bool m_bfHFDiffFirst;
  [BiffRecordPos(28, 2, TFieldType.Bit)]
  private bool m_bfHFScaleWithDoc;
  [BiffRecordPos(28, 3, TFieldType.Bit)]
  private bool m_bfHFAlignMargins;
  private byte[] m_arrBytes = new byte[38];
  private int recordCode = 2204;

  public HeaderAndFooterRecord() => this.m_bfHFScaleWithDoc = true;

  public HeaderAndFooterRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public HeaderAndFooterRecord(int iReserve)
    : base(iReserve)
  {
  }

  public bool AlignHFWithPageMargins
  {
    get => this.m_bfHFAlignMargins;
    set => this.m_bfHFAlignMargins = value;
  }

  public bool DifferentOddAndEvenPagesHF
  {
    get => this.m_bfHFDiffOddEven;
    set => this.m_bfHFDiffOddEven = value;
  }

  public bool HFScaleWithDoc
  {
    get => this.m_bfHFScaleWithDoc;
    set => this.m_bfHFScaleWithDoc = value;
  }

  public bool DifferentFirstPageHF
  {
    get => this.m_bfHFDiffFirst;
    set => this.m_bfHFDiffFirst = value;
  }

  public override int MinimumRecordSize => 0;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    if (this.m_iLength <= 0)
      return;
    if (this.m_iLength > 22)
      iOffset += 28;
    else
      iOffset += 12;
    this.m_bfHFDiffOddEven = provider.ReadBit(iOffset, 0);
    this.m_bfHFDiffFirst = provider.ReadBit(iOffset, 1);
    this.m_bfHFScaleWithDoc = provider.ReadBit(iOffset, 2);
    this.m_bfHFAlignMargins = provider.ReadBit(iOffset, 3);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteBytes(4, this.m_arrBytes);
    provider.WriteInt32(4, this.recordCode);
    iOffset = 28;
    provider.WriteBit(iOffset, this.m_bfHFDiffOddEven, 0);
    provider.WriteBit(iOffset, this.m_bfHFDiffFirst, 1);
    provider.WriteBit(iOffset, this.m_bfHFScaleWithDoc, 2);
    provider.WriteBit(iOffset, this.m_bfHFAlignMargins, 3);
  }

  public override int GetStoreSize(OfficeVersion version) => 42;
}
