// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.CacheDataExRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.CacheDataEx)]
public class CacheDataExRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 12;
  [BiffRecordPos(0, 8, TFieldType.Float)]
  private double m_dDate;
  [BiffRecordPos(8, 4)]
  private uint m_uiFormulaCount;

  public CacheDataExRecord()
  {
  }

  public CacheDataExRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public CacheDataExRecord(int iReserve)
    : base(iReserve)
  {
  }

  public double RefreshDate
  {
    get => this.m_dDate;
    set => this.m_dDate = value;
  }

  public uint FormulaCount
  {
    get => this.m_uiFormulaCount;
    set => this.m_uiFormulaCount = value;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_dDate = provider.ReadDouble(iOffset);
    this.m_uiFormulaCount = provider.ReadUInt32(iOffset + 8);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteDouble(iOffset, this.m_dDate);
    provider.WriteUInt32(iOffset + 8, this.m_uiFormulaCount);
    this.m_iLength = 12;
  }

  public override int GetStoreSize(ExcelVersion version) => 12;
}
