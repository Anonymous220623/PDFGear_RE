// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartDataLabelsRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartDataLabels)]
public class ChartDataLabelsRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 12;
  [BiffRecordPos(12, 1)]
  private byte m_Options;
  [BiffRecordPos(12, 0, TFieldType.Bit)]
  private bool m_bSeriesName;
  [BiffRecordPos(12, 1, TFieldType.Bit)]
  private bool m_bCategoryName;
  [BiffRecordPos(12, 2, TFieldType.Bit)]
  private bool m_bValue;
  [BiffRecordPos(12, 3, TFieldType.Bit)]
  private bool m_bPercentage;
  [BiffRecordPos(12, 4, TFieldType.Bit)]
  private bool m_bBubbleSize;
  [BiffRecordPos(14, 2)]
  private ushort m_usDelimLen;
  private string m_strDelimiter;

  public byte Options => this.m_Options;

  public bool IsSeriesName
  {
    get => this.m_bSeriesName;
    set => this.m_bSeriesName = value;
  }

  public bool IsCategoryName
  {
    get => this.m_bCategoryName;
    set => this.m_bCategoryName = value;
  }

  public bool IsValue
  {
    get => this.m_bValue;
    set => this.m_bValue = value;
  }

  public bool IsPercentage
  {
    get => this.m_bPercentage;
    set => this.m_bPercentage = value;
  }

  public bool IsBubbleSize
  {
    get => this.m_bBubbleSize;
    set => this.m_bBubbleSize = value;
  }

  public int DelimiterLength => (int) this.m_usDelimLen;

  public string Delimiter
  {
    get => this.m_strDelimiter;
    set
    {
      this.m_strDelimiter = value;
      this.m_usDelimLen = value == null ? (ushort) 0 : (ushort) value.Length;
    }
  }

  public ChartDataLabelsRecord()
  {
  }

  public ChartDataLabelsRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartDataLabelsRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_Options = provider.ReadByte(iOffset + 12);
    this.m_bSeriesName = provider.ReadBit(iOffset + 12, 0);
    this.m_bCategoryName = provider.ReadBit(iOffset + 12, 1);
    this.m_bValue = provider.ReadBit(iOffset + 12, 2);
    this.m_bPercentage = provider.ReadBit(iOffset + 12, 3);
    this.m_bBubbleSize = provider.ReadBit(iOffset + 12, 4);
    this.m_usDelimLen = provider.ReadUInt16(iOffset + 14);
    if (this.m_usDelimLen <= (ushort) 0)
      return;
    this.m_strDelimiter = provider.ReadString(iOffset + 16 /*0x10*/, (int) this.m_usDelimLen, out int _, false);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    int num = iOffset;
    provider.WriteInt16(iOffset, (short) 2155);
    int iOffset1 = iOffset + 2;
    for (int index = iOffset + 12; iOffset1 < index; ++iOffset1)
      provider.WriteByte(iOffset1, (byte) 0);
    provider.WriteByte(iOffset + 12, this.m_Options);
    provider.WriteBit(iOffset + 12, this.m_bSeriesName, 0);
    provider.WriteBit(iOffset + 12, this.m_bCategoryName, 1);
    provider.WriteBit(iOffset + 12, this.m_bValue, 2);
    provider.WriteBit(iOffset + 12, this.m_bPercentage, 3);
    provider.WriteBit(iOffset + 12, this.m_bBubbleSize, 4);
    provider.WriteByte(iOffset + 13, (byte) 0);
    provider.WriteUInt16(iOffset + 14, this.m_usDelimLen);
    if (this.m_usDelimLen > (ushort) 0)
    {
      iOffset += 16 /*0x10*/;
      provider.WriteStringNoLenUpdateOffset(ref iOffset, this.m_strDelimiter);
    }
    this.m_iLength = iOffset - num;
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int storeSize = 16 /*0x10*/;
    if (this.m_usDelimLen > (ushort) 0)
      storeSize += 1 + Encoding.Unicode.GetByteCount(this.m_strDelimiter);
    return storeSize;
  }

  public static bool operator ==(ChartDataLabelsRecord record1, ChartDataLabelsRecord record2)
  {
    bool flag1 = object.Equals((object) record1, (object) null);
    bool flag2 = object.Equals((object) record2, (object) null);
    if (flag1 && flag2)
      return true;
    return !flag1 && !flag2 && record1.m_bSeriesName == record2.m_bSeriesName && record1.m_bCategoryName == record2.m_bCategoryName && record1.m_bValue == record2.m_bValue && record1.m_bPercentage == record2.m_bPercentage && record1.m_bBubbleSize == record2.m_bBubbleSize;
  }

  public static bool operator !=(ChartDataLabelsRecord record1, ChartDataLabelsRecord record2)
  {
    return !(record1 == record2);
  }
}
