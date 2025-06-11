// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotNamePairRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.PivotNamePair)]
public class PivotNamePairRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 8;
  [BiffRecordPos(0, 2)]
  private ushort m_usField;
  [BiffRecordPos(2, 2)]
  private ushort m_usCache;
  [BiffRecordPos(4, 2)]
  private ushort m_usReserved;
  [BiffRecordPos(6, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(6, 0, TFieldType.Bit)]
  private bool m_bCalculatedItem;
  [BiffRecordPos(6, 3, TFieldType.Bit)]
  private bool m_bPhysical;
  [BiffRecordPos(6, 4, TFieldType.Bit)]
  private bool m_bRelative;

  public PivotNamePairRecord()
  {
  }

  public PivotNamePairRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotNamePairRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort Field
  {
    get => this.m_usField;
    set => this.m_usField = value;
  }

  public ushort Cache
  {
    get => this.m_usCache;
    set => this.m_usCache = value;
  }

  public ushort Reserved => this.m_usReserved;

  public ushort Options => this.m_usOptions;

  public bool IsCalculatedItem
  {
    get => this.m_bCalculatedItem;
    set => this.m_bCalculatedItem = value;
  }

  public bool IsPhysical
  {
    get => this.m_bPhysical;
    set => this.m_bPhysical = value;
  }

  public bool IsRelative
  {
    get => this.m_bRelative;
    set => this.m_bRelative = value;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usField = provider.ReadUInt16(iOffset);
    this.m_usCache = provider.ReadUInt16(iOffset + 2);
    this.m_usReserved = provider.ReadUInt16(iOffset + 4);
    this.m_usOptions = provider.ReadUInt16(iOffset + 6);
    this.m_bCalculatedItem = provider.ReadBit(iOffset + 6, 0);
    this.m_bPhysical = provider.ReadBit(iOffset + 6, 3);
    this.m_bRelative = provider.ReadBit(iOffset + 6, 4);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usField);
    provider.WriteUInt16(iOffset + 2, this.m_usCache);
    provider.WriteUInt16(iOffset + 4, this.m_usReserved);
    provider.WriteUInt16(iOffset + 6, this.m_usOptions);
    provider.WriteBit(iOffset + 6, this.m_bCalculatedItem, 0);
    provider.WriteBit(iOffset + 6, this.m_bPhysical, 3);
    provider.WriteBit(iOffset + 6, this.m_bRelative, 4);
    this.m_iLength = 8;
  }

  public override int GetStoreSize(ExcelVersion version) => 8;
}
