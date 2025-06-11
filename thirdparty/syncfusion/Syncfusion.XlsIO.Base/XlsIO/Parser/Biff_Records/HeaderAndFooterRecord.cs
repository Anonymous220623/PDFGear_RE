// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.HeaderAndFooterRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.HeaderFooter)]
public class HeaderAndFooterRecord : BiffRecordRaw
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
  private string m_strHeaderEven = string.Empty;
  private string m_strFooterEven = string.Empty;
  private string m_strHeaderFirst = string.Empty;
  private string m_strFooterFirst = string.Empty;
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

  internal string EvenHeaderString
  {
    get => this.m_strHeaderEven;
    set => this.m_strHeaderEven = value;
  }

  internal string EvenFooterString
  {
    get => this.m_strFooterEven;
    set => this.m_strFooterEven = value;
  }

  internal string FirstHeaderString
  {
    get => this.m_strHeaderFirst;
    set => this.m_strHeaderFirst = value;
  }

  internal string FirstFooterString
  {
    get => this.m_strFooterFirst;
    set => this.m_strFooterFirst = value;
  }

  public override int MinimumRecordSize => 0;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
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
    int iFullLength1 = 0;
    int iFullLength2 = 0;
    int iFullLength3 = 0;
    int iFullLength4 = 0;
    uint num1 = (uint) provider.ReadUInt16(iOffset + 2);
    uint num2 = (uint) provider.ReadUInt16(iOffset + 4);
    uint num3 = (uint) provider.ReadUInt16(iOffset + 6);
    uint num4 = (uint) provider.ReadUInt16(iOffset + 8);
    if (num1 > 0U)
      this.EvenHeaderString = provider.ReadString16Bit(iOffset + 10, out iFullLength1);
    if (num2 > 0U)
      this.EvenFooterString = provider.ReadString16Bit(iOffset + 10 + iFullLength1, out iFullLength2);
    if (num3 > 0U)
      this.FirstHeaderString = provider.ReadString16Bit(iOffset + 10 + iFullLength1 + iFullLength2, out iFullLength3);
    if (num4 <= 0U)
      return;
    this.FirstFooterString = provider.ReadString16Bit(iOffset + 10 + iFullLength1 + iFullLength2 + iFullLength3, out iFullLength4);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteBytes(4, this.m_arrBytes);
    provider.WriteInt32(4, this.recordCode);
    iOffset = 32 /*0x20*/;
    provider.WriteBit(iOffset, this.m_bfHFDiffOddEven, 0);
    provider.WriteBit(iOffset, this.m_bfHFDiffFirst, 1);
    provider.WriteBit(iOffset, this.m_bfHFScaleWithDoc, 2);
    provider.WriteBit(iOffset, this.m_bfHFAlignMargins, 3);
    provider.WriteBit(iOffset, false, 4);
    provider.WriteBit(iOffset, false, 5);
    provider.WriteBit(iOffset, false, 6);
    provider.WriteBit(iOffset, false, 7);
    provider.WriteByte(iOffset + 1, (byte) 0);
    provider.WriteInt16(iOffset + 2, (short) this.EvenHeaderString.Length);
    provider.WriteInt16(iOffset + 4, (short) this.EvenFooterString.Length);
    provider.WriteInt16(iOffset + 6, (short) this.FirstHeaderString.Length);
    provider.WriteInt16(iOffset + 8, (short) this.FirstFooterString.Length);
    iOffset += 10;
    if (this.EvenHeaderString.Length > 0)
      provider.WriteString16BitUpdateOffset(ref iOffset, this.EvenHeaderString);
    if (this.EvenFooterString.Length > 0)
      provider.WriteString16BitUpdateOffset(ref iOffset, this.EvenFooterString);
    if (this.FirstHeaderString.Length > 0)
      provider.WriteString16BitUpdateOffset(ref iOffset, this.FirstHeaderString);
    if (this.FirstFooterString.Length <= 0)
      return;
    provider.WriteString16BitUpdateOffset(ref iOffset, this.FirstFooterString);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    return 38 + (this.EvenHeaderString == null || this.EvenHeaderString.Length == 0 ? 0 : 3 + Encoding.Unicode.GetByteCount(this.EvenHeaderString)) + (this.EvenFooterString == null || this.EvenFooterString.Length == 0 ? 0 : 3 + Encoding.Unicode.GetByteCount(this.EvenFooterString)) + (this.FirstHeaderString == null || this.FirstHeaderString.Length == 0 ? 0 : 3 + Encoding.Unicode.GetByteCount(this.FirstHeaderString)) + (this.FirstFooterString == null || this.FirstFooterString.Length == 0 ? 0 : 3 + Encoding.Unicode.GetByteCount(this.FirstFooterString));
  }
}
