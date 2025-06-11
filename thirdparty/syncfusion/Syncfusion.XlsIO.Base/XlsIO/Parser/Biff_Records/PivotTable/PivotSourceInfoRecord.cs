// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotSourceInfoRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[Biff(TBIFFRecord.PivotSourceInfo)]
[CLSCompliant(false)]
public class PivotSourceInfoRecord : BiffRecordRaw
{
  private const ushort DEF_BITMASK_PAGECOUNT = 32767 /*0x7FFF*/;
  private const int DefaultRecordSize = 4;
  [BiffRecordPos(0, 2)]
  private ushort m_usRefCount;
  [BiffRecordPos(2, 2)]
  private ushort m_usPageItemCount;
  [BiffRecordPos(4, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(5, 7, TFieldType.Bit)]
  private bool m_bAutoPage;

  public PivotSourceInfoRecord()
  {
  }

  public PivotSourceInfoRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotSourceInfoRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort RefCount
  {
    get => this.m_usRefCount;
    set => this.m_usRefCount = value;
  }

  public ushort PageItemCount
  {
    get => this.m_usPageItemCount;
    set => this.m_usPageItemCount = value;
  }

  public ushort Options => this.m_usOptions;

  public bool IsAutoPage
  {
    get => this.m_bAutoPage;
    set => this.m_bAutoPage = value;
  }

  public ushort PageCount
  {
    get => BiffRecordRaw.GetUInt16BitsByMask(this.m_usOptions, (ushort) short.MaxValue);
    set => BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usOptions, (ushort) short.MaxValue, value);
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usRefCount = provider.ReadUInt16(iOffset);
    this.m_usPageItemCount = provider.ReadUInt16(iOffset + 2);
    this.m_usOptions = provider.ReadUInt16(iOffset + 4);
    this.m_bAutoPage = provider.ReadBit(iOffset + 5, 7);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usRefCount);
    provider.WriteUInt16(iOffset + 2, this.m_usPageItemCount);
    provider.WriteUInt16(iOffset + 4, this.m_usOptions);
    provider.WriteBit(iOffset + 5, this.m_bAutoPage, 7);
    this.m_iLength = 4;
  }

  public override int GetStoreSize(ExcelVersion version) => 4;
}
