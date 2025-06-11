// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotViewSourceRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[Biff(TBIFFRecord.PivotViewSource)]
[CLSCompliant(false)]
public class PivotViewSourceRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usDataSource;

  public PivotViewSourceRecord()
  {
  }

  public PivotViewSourceRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotViewSourceRecord(int iReserve)
    : base(iReserve)
  {
  }

  public PivotViewSourceRecord.DataSourceTypes DataSource
  {
    get => (PivotViewSourceRecord.DataSourceTypes) this.m_usDataSource;
    set => this.m_usDataSource = (ushort) value;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usDataSource = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usDataSource);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(ExcelVersion version) => 2;

  public enum DataSourceTypes
  {
    MSExcelOrDB = 1,
    External = 2,
    ConsolidationRanges = 4,
    PivotTable = 8,
    ScenarioManager = 16, // 0x00000010
  }
}
