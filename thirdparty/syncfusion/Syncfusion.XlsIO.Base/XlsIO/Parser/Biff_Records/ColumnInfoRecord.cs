// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ColumnInfoRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ColumnInfo)]
public class ColumnInfoRecord : BiffRecordRaw, IOutline, IComparable
{
  private const ushort OutlevelBitMask = 1792 /*0x0700*/;
  private const int DEF_MAX_SIZE = 12;
  [BiffRecordPos(0, 2)]
  private ushort m_usFirstCol;
  [BiffRecordPos(2, 2)]
  private ushort m_usLastCol;
  [BiffRecordPos(4, 2)]
  private ushort m_usColWidth = 2340;
  [BiffRecordPos(6, 2)]
  private ushort m_usExtFormatIndex = 15;
  [BiffRecordPos(8, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(8, 0, TFieldType.Bit)]
  private bool m_bHidden;
  [BiffRecordPos(8, 2, TFieldType.Bit)]
  private bool m_bBestFit;
  [BiffRecordPos(8, 1, TFieldType.Bit)]
  private bool m_bUserSet;
  [BiffRecordPos(8, 3, TFieldType.Bit)]
  private bool m_bPhonetic;
  [BiffRecordPos(9, 4, TFieldType.Bit)]
  private bool m_bCollapsed;
  [BiffRecordPos(10, 2)]
  private ushort m_usReserved = 4;

  public ushort Reserved => this.m_usReserved;

  public ushort FirstColumn
  {
    get => this.m_usFirstCol;
    set => this.m_usFirstCol = value;
  }

  public ushort LastColumn
  {
    get => this.m_usLastCol;
    set => this.m_usLastCol = value;
  }

  public ushort ColumnWidth
  {
    get => this.m_usColWidth;
    set => this.m_usColWidth = value;
  }

  public ushort ExtendedFormatIndex
  {
    get => this.m_usExtFormatIndex;
    set => this.m_usExtFormatIndex = value;
  }

  public bool IsHidden
  {
    get => this.m_bHidden;
    set => this.m_bHidden = value;
  }

  internal bool IsBestFit
  {
    get => this.m_bBestFit;
    set => this.m_bBestFit = value;
  }

  internal bool IsUserSet
  {
    get => this.m_bUserSet;
    set => this.m_bUserSet = value;
  }

  internal bool IsPhenotic
  {
    get => this.m_bPhonetic;
    set => this.m_bPhonetic = value;
  }

  public ushort OutlineLevel
  {
    get
    {
      return (ushort) ((uint) BiffRecordRaw.GetUInt16BitsByMask(this.m_usOptions, (ushort) 1792 /*0x0700*/) >> 8);
    }
    set
    {
      if (value > (ushort) 7)
        throw new ArgumentOutOfRangeException();
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usOptions, (ushort) 1792 /*0x0700*/, (ushort) ((uint) value << 8));
    }
  }

  public bool IsCollapsed
  {
    get => this.m_bCollapsed;
    set => this.m_bCollapsed = value;
  }

  public override int MinimumRecordSize => 10;

  public override int MaximumRecordSize => 12;

  ushort IOutline.Index
  {
    get => this.FirstColumn;
    set
    {
      this.FirstColumn = value;
      this.LastColumn = value;
    }
  }

  public ColumnInfoRecord()
  {
  }

  public ColumnInfoRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ColumnInfoRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usFirstCol = provider.ReadUInt16(iOffset);
    this.m_usLastCol = provider.ReadUInt16(iOffset + 2);
    this.m_usColWidth = provider.ReadUInt16(iOffset + 4);
    this.m_usExtFormatIndex = provider.ReadUInt16(iOffset + 6);
    this.m_usOptions = provider.ReadUInt16(iOffset + 8);
    this.m_bHidden = provider.ReadBit(iOffset + 8, 0);
    this.m_bUserSet = provider.ReadBit(iOffset + 8, 1);
    this.m_bBestFit = provider.ReadBit(iOffset + 8, 2);
    this.m_bPhonetic = provider.ReadBit(iOffset + 8, 3);
    this.m_bCollapsed = provider.ReadBit(iOffset + 9, 4);
    if (iLength > 12)
      this.m_usReserved = provider.ReadUInt16(iOffset + 10);
    this.m_iLength = this.MinimumRecordSize;
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usFirstCol);
    provider.WriteUInt16(iOffset + 2, this.m_usLastCol);
    provider.WriteUInt16(iOffset + 4, this.m_usColWidth);
    provider.WriteUInt16(iOffset + 6, this.m_usExtFormatIndex);
    provider.WriteUInt16(iOffset + 8, this.m_usOptions);
    provider.WriteBit(iOffset + 8, this.m_bHidden, 0);
    provider.WriteBit(iOffset + 8, this.m_bUserSet, 1);
    provider.WriteBit(iOffset + 8, this.m_bBestFit, 2);
    provider.WriteBit(iOffset + 8, this.m_bPhonetic, 3);
    provider.WriteBit(iOffset + 9, this.m_bCollapsed, 4);
    provider.WriteUInt16(iOffset + 10, this.m_usReserved);
  }

  public override int GetStoreSize(ExcelVersion version) => 12;

  public int CompareTo(object obj)
  {
    int num = -1;
    if (obj is ColumnInfoRecord)
    {
      ColumnInfoRecord columnInfoRecord = (ColumnInfoRecord) obj;
      ushort outlineLevel = this.OutlineLevel;
      if ((num = outlineLevel.CompareTo(columnInfoRecord.OutlineLevel)) == 0 && (num = this.m_usExtFormatIndex.CompareTo(columnInfoRecord.m_usExtFormatIndex)) == 0 && (num = this.m_usColWidth.CompareTo(columnInfoRecord.m_usColWidth)) == 0 && (num = this.m_bHidden.CompareTo(columnInfoRecord.m_bHidden)) == 0 && (num = this.m_bCollapsed.CompareTo(columnInfoRecord.m_bCollapsed)) == 0 && (num = this.m_usReserved.CompareTo(columnInfoRecord.m_usReserved)) == 0)
        return 0;
    }
    return num;
  }

  public void SetDefaultOptions()
  {
    this.m_usColWidth = (ushort) 2340;
    this.m_usOptions = (ushort) 0;
    this.m_bHidden = false;
    this.m_bCollapsed = false;
    this.m_usReserved = (ushort) 4;
  }
}
