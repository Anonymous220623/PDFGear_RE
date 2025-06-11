// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.RuleDataRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[Biff(TBIFFRecord.RuleData)]
[CLSCompliant(false)]
public class RuleDataRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 8;
  private const ushort DEF_BITMASK_RULETYPE = 240 /*0xF0*/;
  private const ushort DEF_BIT_RULETYPE = 4;
  [BiffRecordPos(0, 1)]
  private byte m_btDim;
  [BiffRecordPos(1, 1)]
  private byte m_btCurrentField;
  [BiffRecordPos(2, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(2, 0, TFieldType.Bit)]
  private bool m_bRowArea;
  [BiffRecordPos(2, 1, TFieldType.Bit)]
  private bool m_bColumnArea;
  [BiffRecordPos(2, 2, TFieldType.Bit)]
  private bool m_bPageArea;
  [BiffRecordPos(2, 3, TFieldType.Bit)]
  private bool m_bDataArea;
  [BiffRecordPos(3, 1, TFieldType.Bit)]
  private bool m_bNoHeader;
  [BiffRecordPos(3, 2, TFieldType.Bit)]
  private bool m_bNoData;
  [BiffRecordPos(3, 3, TFieldType.Bit)]
  private bool m_bGrandRow;
  [BiffRecordPos(3, 4, TFieldType.Bit)]
  private bool m_bGrandColumn;
  [BiffRecordPos(3, 5, TFieldType.Bit)]
  private bool m_bGrandRowSav;
  [BiffRecordPos(3, 6, TFieldType.Bit)]
  private bool m_bCacheBased;
  [BiffRecordPos(3, 7, TFieldType.Bit)]
  private bool m_bGrandColSav;
  [BiffRecordPos(4, 2)]
  private ushort m_usReserved;
  [BiffRecordPos(6, 2)]
  private ushort m_usFiltersCount;
  private int? m_iReserved;

  public RuleDataRecord()
  {
  }

  public RuleDataRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public RuleDataRecord(int iReserve)
    : base(iReserve)
  {
  }

  public byte Dim
  {
    get => this.m_btDim;
    set => this.m_btDim = value;
  }

  public byte CurrentField
  {
    get => this.m_btCurrentField;
    set => this.m_btCurrentField = value;
  }

  public ushort Options => this.m_usOptions;

  public bool IsRowArea
  {
    get => this.m_bRowArea;
    set => this.m_bRowArea = value;
  }

  public bool IsColumnArea
  {
    get => this.m_bColumnArea;
    set => this.m_bColumnArea = value;
  }

  public bool IsPageArea
  {
    get => this.m_bPageArea;
    set => this.m_bPageArea = value;
  }

  public bool IsDataArea
  {
    get => this.m_bDataArea;
    set => this.m_bDataArea = value;
  }

  public ushort RuleType
  {
    get
    {
      return (ushort) ((uint) BiffRecordRaw.GetUInt16BitsByMask(this.m_usOptions, (ushort) 240 /*0xF0*/) >> 4);
    }
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usOptions, (ushort) 240 /*0xF0*/, (ushort) ((uint) value << 4));
    }
  }

  public bool IsNoHeader
  {
    get => this.m_bNoHeader;
    set => this.m_bNoHeader = value;
  }

  public bool IsNoData
  {
    get => this.m_bNoData;
    set => this.m_bNoData = value;
  }

  public bool IsGrandRow
  {
    get => this.m_bGrandRow;
    set => this.m_bGrandRow = value;
  }

  public bool IsGrandColumn
  {
    get => this.m_bGrandColumn;
    set => this.m_bGrandColumn = value;
  }

  public bool IsGrandRowSav
  {
    get => this.m_bGrandRowSav;
    set => this.m_bGrandRowSav = value;
  }

  public bool IsCacheBased
  {
    get => this.m_bCacheBased;
    set => this.m_bCacheBased = value;
  }

  public bool IsGrandColSav
  {
    get => this.m_bGrandColSav;
    set => this.m_bGrandColSav = value;
  }

  public ushort Reserved
  {
    get => this.m_usReserved;
    set => this.m_usReserved = value;
  }

  public ushort FiltersCount
  {
    get => this.m_usFiltersCount;
    set => this.m_usFiltersCount = value;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_btDim = provider.ReadByte(iOffset);
    this.m_btCurrentField = provider.ReadByte(iOffset + 1);
    this.m_usOptions = provider.ReadUInt16(iOffset + 2);
    this.m_bRowArea = provider.ReadBit(iOffset + 2, 0);
    this.m_bColumnArea = provider.ReadBit(iOffset + 2, 1);
    this.m_bPageArea = provider.ReadBit(iOffset + 2, 2);
    this.m_bDataArea = provider.ReadBit(iOffset + 2, 3);
    this.m_bNoHeader = provider.ReadBit(iOffset + 3, 1);
    this.m_bNoData = provider.ReadBit(iOffset + 3, 2);
    this.m_bGrandRow = provider.ReadBit(iOffset + 3, 3);
    this.m_bGrandColumn = provider.ReadBit(iOffset + 3, 4);
    this.m_bGrandRowSav = provider.ReadBit(iOffset + 3, 5);
    this.m_bCacheBased = provider.ReadBit(iOffset + 3, 6);
    this.m_bGrandColSav = provider.ReadBit(iOffset + 3, 7);
    this.m_usReserved = provider.ReadUInt16(iOffset + 4);
    this.m_usFiltersCount = provider.ReadUInt16(iOffset + 6);
    if (iLength <= 8)
      return;
    this.m_iReserved = new int?(provider.ReadInt32(iOffset + 8));
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteByte(iOffset, this.m_btDim);
    provider.WriteByte(iOffset + 1, this.m_btCurrentField);
    provider.WriteUInt16(iOffset + 2, this.m_usOptions);
    provider.WriteBit(iOffset + 2, this.m_bRowArea, 0);
    provider.WriteBit(iOffset + 2, this.m_bColumnArea, 1);
    provider.WriteBit(iOffset + 2, this.m_bPageArea, 2);
    provider.WriteBit(iOffset + 2, this.m_bDataArea, 3);
    provider.WriteBit(iOffset + 3, this.m_bNoHeader, 1);
    provider.WriteBit(iOffset + 3, this.m_bNoData, 2);
    provider.WriteBit(iOffset + 3, this.m_bGrandRow, 3);
    provider.WriteBit(iOffset + 3, this.m_bGrandColumn, 4);
    provider.WriteBit(iOffset + 3, this.m_bGrandRowSav, 5);
    provider.WriteBit(iOffset + 3, this.m_bCacheBased, 6);
    provider.WriteBit(iOffset + 3, this.m_bGrandColSav, 7);
    provider.WriteUInt16(iOffset + 4, this.m_usReserved);
    provider.WriteUInt16(iOffset + 6, this.m_usFiltersCount);
    this.m_iLength = 8;
    if (!this.m_iReserved.HasValue)
      return;
    provider.WriteInt32(iOffset + 8, this.m_iReserved.Value);
    this.m_iLength += 4;
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int storeSize = 8;
    if (this.m_iReserved.HasValue)
      storeSize += 4;
    return storeSize;
  }
}
