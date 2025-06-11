// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotNameRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.PivotName)]
public class PivotNameRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 8;
  [BiffRecordPos(0, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(0, 1, TFieldType.Bit)]
  private bool m_bErrorName;
  [BiffRecordPos(2, 2)]
  private ushort m_usAggregateField;
  [BiffRecordPos(4, 2)]
  private ushort m_usAggregateFunction;
  [BiffRecordPos(6, 2)]
  private ushort m_usPairCount;

  public PivotNameRecord()
  {
  }

  public PivotNameRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotNameRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort Options => this.m_usOptions;

  public bool IsErrorName
  {
    get => this.m_bErrorName;
    set => this.m_bErrorName = value;
  }

  public ushort AggregateField
  {
    get => this.m_usAggregateField;
    set => this.m_usAggregateField = value;
  }

  public ushort AggregateFunction
  {
    get => this.m_usAggregateFunction;
    set => this.m_usAggregateFunction = value;
  }

  public ushort PairCount
  {
    get => this.m_usPairCount;
    set => this.m_usPairCount = value;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bErrorName = provider.ReadBit(iOffset, 1);
    this.m_usAggregateField = provider.ReadUInt16(iOffset + 2);
    this.m_usAggregateFunction = provider.ReadUInt16(iOffset + 4);
    this.m_usPairCount = provider.ReadUInt16(iOffset + 6);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bErrorName, 1);
    provider.WriteUInt16(iOffset + 2, this.m_usAggregateField);
    provider.WriteUInt16(iOffset + 4, this.m_usAggregateFunction);
    provider.WriteUInt16(iOffset + 6, this.m_usPairCount);
    this.m_iLength = 8;
  }

  public override int GetStoreSize(ExcelVersion version) => 8;
}
