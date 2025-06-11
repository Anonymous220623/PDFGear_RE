// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.SortRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.Sort)]
internal class SortRecord : BiffRecordRaw
{
  private const ushort TableIndexBitMask = 992;
  private const int TableIndexStartBit = 5;
  private const int DEF_FIXED_PART_SIZE = 5;
  [BiffRecordPos(0, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(0, 0, TFieldType.Bit)]
  private bool m_bSortColumns;
  [BiffRecordPos(0, 1, TFieldType.Bit)]
  private bool m_bFirstDesc;
  [BiffRecordPos(0, 2, TFieldType.Bit)]
  private bool m_bSecondDesc;
  [BiffRecordPos(0, 3, TFieldType.Bit)]
  private bool m_bThirdDesc;
  [BiffRecordPos(0, 4, TFieldType.Bit)]
  private bool m_bCaseSensitive;
  [BiffRecordPos(2, 1)]
  private byte m_FirstKeyLen;
  [BiffRecordPos(3, 1)]
  private byte m_SecondKeyLen;
  [BiffRecordPos(4, 1)]
  private byte m_ThirdKeyLen;
  private string m_strFirstKey = string.Empty;
  private string m_strSecondKey = string.Empty;
  private string m_strThirdKey = string.Empty;

  public bool IsSortColumns
  {
    get => this.m_bSortColumns;
    set => this.m_bSortColumns = value;
  }

  public bool IsFirstDesc
  {
    get => this.m_bFirstDesc;
    set => this.m_bFirstDesc = value;
  }

  public bool IsSecondDesc
  {
    get => this.m_bSecondDesc;
    set => this.m_bSecondDesc = value;
  }

  public bool IsThirdDesc
  {
    get => this.m_bThirdDesc;
    set => this.m_bThirdDesc = value;
  }

  public bool IsCaseSensitive
  {
    get => this.m_bCaseSensitive;
    set => this.m_bCaseSensitive = value;
  }

  public ushort TableIndex
  {
    get => (ushort) ((uint) BiffRecordRaw.GetUInt16BitsByMask(this.m_usOptions, (ushort) 992) >> 5);
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usOptions, (ushort) 992, (ushort) ((uint) value << 5));
    }
  }

  public byte FirstKeyLen => this.m_FirstKeyLen;

  public byte SecondKeyLen => this.m_SecondKeyLen;

  public byte ThirdKeyLen => this.m_ThirdKeyLen;

  public string FirstKey
  {
    get => this.m_strFirstKey;
    set
    {
      this.m_strFirstKey = value;
      this.m_FirstKeyLen = value != null ? (byte) value.Length : (byte) 0;
    }
  }

  public string SecondKey
  {
    get => this.m_strSecondKey;
    set
    {
      this.m_strSecondKey = value;
      this.m_SecondKeyLen = value != null ? (byte) value.Length : (byte) 0;
    }
  }

  public string ThirdKey
  {
    get => this.m_strThirdKey;
    set
    {
      this.m_strThirdKey = value;
      this.m_ThirdKeyLen = value != null ? (byte) value.Length : (byte) 0;
    }
  }

  public override int MinimumRecordSize => 5;

  public SortRecord()
  {
  }

  public SortRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public SortRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bSortColumns = provider.ReadBit(iOffset, 0);
    this.m_bFirstDesc = provider.ReadBit(iOffset, 1);
    this.m_bSecondDesc = provider.ReadBit(iOffset, 2);
    this.m_bThirdDesc = provider.ReadBit(iOffset, 3);
    this.m_bCaseSensitive = provider.ReadBit(iOffset, 4);
    this.m_FirstKeyLen = provider.ReadByte(iOffset + 2);
    this.m_SecondKeyLen = provider.ReadByte(iOffset + 3);
    this.m_ThirdKeyLen = provider.ReadByte(iOffset + 4);
    iOffset += 5;
    this.m_strFirstKey = provider.ReadStringUpdateOffset(ref iOffset, (int) this.m_FirstKeyLen);
    this.m_strSecondKey = provider.ReadStringUpdateOffset(ref iOffset, (int) this.m_SecondKeyLen);
    this.m_strThirdKey = provider.ReadStringUpdateOffset(ref iOffset, (int) this.m_ThirdKeyLen);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    int num = iOffset;
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bSortColumns, 0);
    provider.WriteBit(iOffset, this.m_bFirstDesc, 1);
    provider.WriteBit(iOffset, this.m_bSecondDesc, 2);
    provider.WriteBit(iOffset, this.m_bThirdDesc, 3);
    provider.WriteBit(iOffset, this.m_bCaseSensitive, 4);
    provider.WriteByte(iOffset + 2, this.m_FirstKeyLen);
    provider.WriteByte(iOffset + 3, this.m_SecondKeyLen);
    provider.WriteByte(iOffset + 4, this.m_ThirdKeyLen);
    iOffset += 5;
    provider.WriteStringNoLenUpdateOffset(ref iOffset, this.m_strFirstKey);
    provider.WriteStringNoLenUpdateOffset(ref iOffset, this.m_strSecondKey);
    provider.WriteStringNoLenUpdateOffset(ref iOffset, this.m_strThirdKey);
    provider.WriteByte(iOffset, (byte) 0);
    ++iOffset;
    this.m_iLength = iOffset - num;
  }

  private int GetStringSize(string strValue)
  {
    return strValue == null || strValue.Length == 0 ? 0 : Encoding.Unicode.GetByteCount(strValue) + 1;
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    return 5 + this.GetStringSize(this.m_strFirstKey) + this.GetStringSize(this.m_strSecondKey) + this.GetStringSize(this.m_strThirdKey) + 1;
  }
}
