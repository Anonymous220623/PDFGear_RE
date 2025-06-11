// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotFieldRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.PivotField)]
public class PivotFieldRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 14;
  [BiffRecordPos(0, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(0, 0, TFieldType.Bit)]
  private bool m_bInIndexList;
  [BiffRecordPos(0, 1, TFieldType.Bit)]
  private bool m_bNotInList;
  [BiffRecordPos(0, 5, TFieldType.Bit)]
  private bool m_bDouble;
  [BiffRecordPos(0, 6, TFieldType.Bit)]
  private bool m_bDoubleInt;
  [BiffRecordPos(0, 7, TFieldType.Bit)]
  private bool m_bString;
  [BiffRecordPos(1, 0, TFieldType.Bit)]
  private bool m_bUnknown;
  [BiffRecordPos(1, 1, TFieldType.Bit)]
  private bool m_bLongIndex;
  [BiffRecordPos(1, 2, TFieldType.Bit)]
  private bool m_bUnknown2;
  [BiffRecordPos(1, 3, TFieldType.Bit)]
  private bool m_bDate;
  [BiffRecordPos(2, 4)]
  private uint m_usReserved1;
  [BiffRecordPos(6, 2)]
  private ushort m_usItemCount1;
  [BiffRecordPos(8, 4)]
  private uint m_usReserved2;
  [BiffRecordPos(12, 2)]
  private ushort m_usItemCount2;
  [BiffRecordPos(14, TFieldType.String16Bit)]
  private string m_strFieldName;
  private bool m_bFieldName16Bit;

  public PivotFieldRecord()
  {
  }

  public PivotFieldRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotFieldRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort Options => this.m_usOptions;

  public bool IsInIndexList
  {
    get => this.m_bInIndexList;
    set => this.m_bInIndexList = value;
  }

  public bool IsNotInList
  {
    get => this.m_bNotInList;
    set => this.m_bNotInList = value;
  }

  public bool IsDouble
  {
    get => this.m_bDouble;
    set => this.m_bDouble = value;
  }

  public bool IsDoubleInt
  {
    get => this.m_bDoubleInt;
    set => this.m_bDoubleInt = value;
  }

  public bool IsString
  {
    get => this.m_bString;
    set => this.m_bString = value;
  }

  public bool IsUnknown
  {
    get => this.m_bUnknown;
    set => this.m_bUnknown = value;
  }

  public bool IsLongIndex
  {
    get => this.m_bLongIndex;
    set => this.m_bLongIndex = value;
  }

  public bool IsUnknown2
  {
    get => this.m_bUnknown2;
    set => this.m_bUnknown2 = value;
  }

  public bool IsDate
  {
    get => this.m_bDate;
    set => this.m_bDate = value;
  }

  public uint Reserved1
  {
    get => this.m_usReserved1;
    set => this.m_usReserved1 = value;
  }

  public ushort ItemCount1
  {
    get => this.m_usItemCount1;
    set => this.m_usItemCount1 = value;
  }

  public uint Reserved2
  {
    get => this.m_usReserved2;
    set => this.m_usReserved2 = value;
  }

  public ushort ItemCount2
  {
    get => this.m_usItemCount2;
    set => this.m_usItemCount2 = value;
  }

  public string Name
  {
    get => this.m_strFieldName;
    set
    {
      this.m_strFieldName = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_bFieldName16Bit = !BiffRecordRawWithArray.IsAsciiString(value);
    }
  }

  public override int MinimumRecordSize => 14;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bInIndexList = provider.ReadBit(iOffset, 0);
    this.m_bNotInList = provider.ReadBit(iOffset, 1);
    this.m_bDouble = provider.ReadBit(iOffset, 5);
    this.m_bDoubleInt = provider.ReadBit(iOffset, 6);
    this.m_bString = provider.ReadBit(iOffset, 7);
    this.m_bUnknown = provider.ReadBit(iOffset + 1, 0);
    this.m_bLongIndex = provider.ReadBit(iOffset + 1, 1);
    this.m_bUnknown2 = provider.ReadBit(iOffset + 1, 2);
    this.m_bDate = provider.ReadBit(iOffset + 1, 3);
    this.m_usReserved1 = provider.ReadUInt32(iOffset + 2);
    this.m_usItemCount1 = provider.ReadUInt16(iOffset + 6);
    this.m_usReserved2 = provider.ReadUInt32(iOffset + 8);
    this.m_usItemCount2 = provider.ReadUInt16(iOffset + 12);
    int iFullLength;
    this.m_strFieldName = provider.ReadString16Bit(iOffset + 14, out iFullLength);
    this.m_bFieldName16Bit = this.m_strFieldName.Length * 2 + 3 >= iFullLength;
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bInIndexList, 0);
    provider.WriteBit(iOffset, this.m_bNotInList, 1);
    provider.WriteBit(iOffset, this.m_bDouble, 5);
    provider.WriteBit(iOffset, this.m_bDoubleInt, 6);
    provider.WriteBit(iOffset, this.m_bString, 7);
    provider.WriteBit(iOffset + 1, this.m_bUnknown, 0);
    provider.WriteBit(iOffset + 1, this.m_bLongIndex, 1);
    provider.WriteBit(iOffset + 1, this.m_bUnknown2, 2);
    provider.WriteBit(iOffset + 1, this.m_bDate, 3);
    provider.WriteUInt32(iOffset + 2, this.m_usReserved1);
    provider.WriteUInt16(iOffset + 6, this.m_usItemCount1);
    provider.WriteUInt32(iOffset + 8, this.m_usReserved2);
    provider.WriteUInt16(iOffset + 12, this.m_usItemCount2);
    provider.WriteString16Bit(iOffset + 14, this.m_strFieldName, this.m_bFieldName16Bit);
    this.m_iLength = this.GetStoreSize(version);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int length = this.m_strFieldName.Length;
    int storeSize = 17 + length;
    if (this.m_bFieldName16Bit)
      storeSize += length;
    return storeSize;
  }
}
