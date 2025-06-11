// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.RuleFilterRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[Biff(TBIFFRecord.RuleFilter)]
[CLSCompliant(false)]
public class RuleFilterRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 8;
  private const ushort DEF_DIM_BITMASK = 65472;
  private const ushort DEF_DIM_START_BIT = 6;
  private const ushort DEF_SXVD_BITMASK = 1023 /*0x03FF*/;
  [BiffRecordPos(0, 2)]
  private ushort m_usOptions1;
  [BiffRecordPos(0, 0, TFieldType.Bit)]
  private bool m_bRowField;
  [BiffRecordPos(0, 1, TFieldType.Bit)]
  private bool m_bColumnField;
  [BiffRecordPos(0, 2, TFieldType.Bit)]
  private bool m_bPageField;
  [BiffRecordPos(0, 3, TFieldType.Bit)]
  private bool m_bDataField;
  [BiffRecordPos(2, 2)]
  private ushort m_usOptions2;
  [BiffRecordPos(4, 2)]
  private ushort m_usFunction;
  [BiffRecordPos(6, 2)]
  private ushort m_usViewItemCount;

  public RuleFilterRecord()
  {
  }

  public RuleFilterRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public RuleFilterRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort Options1 => this.m_usOptions1;

  public bool IsRowField
  {
    get => this.m_bRowField;
    set => this.m_bRowField = value;
  }

  public bool IsColumnField
  {
    get => this.m_bColumnField;
    set => this.m_bColumnField = value;
  }

  public bool IsPageField
  {
    get => this.m_bPageField;
    set => this.m_bPageField = value;
  }

  public bool IsDataField
  {
    get => this.m_bDataField;
    set => this.m_bDataField = value;
  }

  public ushort Dim
  {
    get
    {
      return (ushort) ((uint) BiffRecordRaw.GetUInt16BitsByMask(this.m_usOptions1, (ushort) 65472) >> 6);
    }
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usOptions1, (ushort) 65472, (ushort) ((uint) value << 6));
    }
  }

  public ushort Options2 => this.m_usOptions2;

  public ushort SXVD
  {
    get => BiffRecordRaw.GetUInt16BitsByMask(this.m_usOptions2, (ushort) 1023 /*0x03FF*/);
    set
    {
      if (value > (ushort) 1023 /*0x03FF*/)
        throw new ArgumentOutOfRangeException(nameof (value), "Value cannot be greater than " + (ushort) 1023 /*0x03FF*/.ToString());
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usOptions2, (ushort) 1023 /*0x03FF*/, value);
    }
  }

  public RuleFilterRecord.FunctionType Function
  {
    get => (RuleFilterRecord.FunctionType) this.m_usFunction;
    set => this.m_usFunction = (ushort) value;
  }

  public ushort ViewItemCount
  {
    get => this.m_usViewItemCount;
    set => this.m_usViewItemCount = value;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usOptions1 = provider.ReadUInt16(iOffset);
    this.m_bRowField = provider.ReadBit(iOffset, 0);
    this.m_bColumnField = provider.ReadBit(iOffset, 1);
    this.m_bPageField = provider.ReadBit(iOffset, 2);
    this.m_bDataField = provider.ReadBit(iOffset, 3);
    this.m_usOptions2 = provider.ReadUInt16(iOffset + 2);
    this.m_usFunction = provider.ReadUInt16(iOffset + 4);
    this.m_usViewItemCount = provider.ReadUInt16(iOffset + 6);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usOptions1);
    provider.WriteBit(iOffset, this.m_bRowField, 0);
    provider.WriteBit(iOffset, this.m_bColumnField, 1);
    provider.WriteBit(iOffset, this.m_bPageField, 2);
    provider.WriteBit(iOffset, this.m_bDataField, 3);
    provider.WriteUInt16(iOffset + 2, this.m_usOptions2);
    provider.WriteUInt16(iOffset + 4, this.m_usFunction);
    provider.WriteUInt16(iOffset + 6, this.m_usViewItemCount);
    this.m_iLength = 8;
  }

  public override int GetStoreSize(ExcelVersion version) => 8;

  public enum FunctionType
  {
    Data = 1,
    Default = 2,
    Sum = 4,
    CountA = 8,
    Count = 16, // 0x00000010
    Average = 32, // 0x00000020
    Max = 64, // 0x00000040
    Min = 128, // 0x00000080
    Product = 256, // 0x00000100
    Stdev = 512, // 0x00000200
    Stdevp = 1024, // 0x00000400
    Var = 2048, // 0x00000800
    Varp = 4096, // 0x00001000
  }
}
