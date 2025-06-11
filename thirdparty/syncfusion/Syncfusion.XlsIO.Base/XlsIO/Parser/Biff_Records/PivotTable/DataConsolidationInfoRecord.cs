// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.DataConsolidationInfoRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[Biff(TBIFFRecord.DCON)]
[CLSCompliant(false)]
public class DataConsolidationInfoRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 8;
  [BiffRecordPos(0, 2)]
  private ushort m_usFunctionIndex;
  [BiffRecordPos(2, 2)]
  private ushort m_usLeftCat;
  [BiffRecordPos(4, 2)]
  private ushort m_usTopCat;
  [BiffRecordPos(6, 2)]
  private ushort m_usLinkConsol;

  public DataConsolidationInfoRecord()
  {
  }

  public DataConsolidationInfoRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public DataConsolidationInfoRecord(int iReserve)
    : base(iReserve)
  {
  }

  public DataConsolidationInfoRecord.FunctionTypes FunctionIndex
  {
    get => (DataConsolidationInfoRecord.FunctionTypes) this.m_usFunctionIndex;
    set => this.m_usFunctionIndex = (ushort) value;
  }

  public bool IsLeftCat
  {
    get => this.m_usLeftCat == (ushort) 1;
    set => this.m_usLeftCat = value ? (ushort) 1 : (ushort) 0;
  }

  public bool IsTopCat
  {
    get => this.m_usTopCat == (ushort) 1;
    set => this.m_usTopCat = value ? (ushort) 1 : (ushort) 0;
  }

  public bool IsLinkConsol
  {
    get => this.m_usLinkConsol == (ushort) 1;
    set => this.m_usLinkConsol = value ? (ushort) 1 : (ushort) 0;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usFunctionIndex = provider.ReadUInt16(iOffset);
    this.m_usLeftCat = provider.ReadUInt16(iOffset + 2);
    this.m_usTopCat = provider.ReadUInt16(iOffset + 4);
    this.m_usLinkConsol = provider.ReadUInt16(iOffset + 6);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usFunctionIndex);
    provider.WriteUInt16(iOffset + 2, this.m_usLeftCat);
    provider.WriteUInt16(iOffset + 4, this.m_usTopCat);
    provider.WriteUInt16(iOffset + 6, this.m_usLinkConsol);
    this.m_iLength = 8;
  }

  public override int GetStoreSize(ExcelVersion version) => 8;

  public enum FunctionTypes
  {
    Average,
    CountNums,
    Count,
    Max,
    Min,
    Product,
    StdDev,
    StdDevp,
    Sum,
    Var,
    Varp,
  }
}
