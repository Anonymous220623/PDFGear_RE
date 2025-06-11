// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotViewItemRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.PivotViewItem)]
public class PivotViewItemRecord : BiffRecordRawWithArray
{
  private const int DEF_NAME_OFFSET = 8;
  [BiffRecordPos(0, 2)]
  private ushort m_usItemType;
  [BiffRecordPos(2, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(2, 0, TFieldType.Bit)]
  private bool m_bHidden;
  [BiffRecordPos(2, 1, TFieldType.Bit)]
  private bool m_bHideDetail;
  [BiffRecordPos(2, 2, TFieldType.Bit)]
  private bool m_bFormula;
  [BiffRecordPos(2, 3, TFieldType.Bit)]
  private bool m_bMissing;
  [BiffRecordPos(4, 2)]
  private ushort m_usCache;
  [BiffRecordPos(6, 2)]
  private ushort m_usNameLength;
  private string m_strName;

  public PivotViewItemRecord()
  {
  }

  public PivotViewItemRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotViewItemRecord(int iReserve)
    : base(iReserve)
  {
  }

  public PivotViewItemRecord.ItemTypes ItemType
  {
    get => (PivotViewItemRecord.ItemTypes) this.m_usItemType;
    set => this.m_usItemType = (ushort) value;
  }

  public ushort Options => this.m_usOptions;

  public bool IsHidden
  {
    get => this.m_bHidden;
    set => this.m_bHidden = value;
  }

  public bool IsHideDetail
  {
    get => this.m_bHideDetail;
    set => this.m_bHideDetail = value;
  }

  public bool IsFormula
  {
    get => this.m_bFormula;
    set => this.m_bFormula = value;
  }

  public bool IsMissing
  {
    get => this.m_bMissing;
    set => this.m_bMissing = value;
  }

  public ushort Cache
  {
    get => this.m_usCache;
    set => this.m_usCache = value;
  }

  public ushort NameLength => this.m_usNameLength;

  public string Name
  {
    get => this.m_strName;
    set
    {
      if (value == null)
        this.m_usNameLength = ushort.MaxValue;
      else
        this.m_usNameLength = value.Length <= (int) ushort.MaxValue ? (ushort) value.Length : throw new ArgumentOutOfRangeException("value.Length", "Value cannot be greater DataItemRecord.DEF_NULL_NAME_LENGTH");
      this.m_strName = value;
    }
  }

  public override void ParseStructure()
  {
    this.m_usItemType = this.GetUInt16(0);
    this.m_usOptions = this.GetUInt16(2);
    this.m_bHidden = this.GetBit(2, 0);
    this.m_bHideDetail = this.GetBit(2, 1);
    this.m_bFormula = this.GetBit(2, 2);
    this.m_bMissing = this.GetBit(2, 3);
    this.m_usCache = this.GetUInt16(4);
    this.m_usNameLength = this.GetUInt16(6);
    this.m_strName = (string) null;
    if (this.m_usNameLength == ushort.MaxValue)
      return;
    this.m_strName = this.GetString(8, (int) this.m_usNameLength);
  }

  public override void InfillInternalData(ExcelVersion version)
  {
    this.m_iLength = 8;
    this.m_data = new byte[this.m_iLength];
    this.SetUInt16(0, this.m_usItemType);
    this.SetUInt16(2, this.m_usOptions);
    this.SetBit(2, this.m_bHidden, 0);
    this.SetBit(2, this.m_bHideDetail, 1);
    this.SetBit(2, this.m_bFormula, 2);
    this.SetBit(2, this.m_bMissing, 3);
    this.SetUInt16(4, this.m_usCache);
    this.SetUInt16(6, this.m_usNameLength);
    if (this.m_strName == null)
      return;
    this.m_iLength += this.SetStringNoLen(8, this.m_strName);
  }

  public enum ItemTypes
  {
    Data = 0,
    Default = 1,
    Sum = 2,
    Counta = 3,
    Count = 4,
    Average = 5,
    Max = 6,
    Min = 7,
    Product = 8,
    Stdev = 9,
    Stdevp = 10, // 0x0000000A
    Var = 11, // 0x0000000B
    Varp = 12, // 0x0000000C
    GrandTotal = 13, // 0x0000000D
    Page = 254, // 0x000000FE
    Null = 255, // 0x000000FF
  }
}
