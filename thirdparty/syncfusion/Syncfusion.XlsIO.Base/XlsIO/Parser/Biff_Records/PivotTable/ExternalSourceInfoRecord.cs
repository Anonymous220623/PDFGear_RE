// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.ExternalSourceInfoRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ExternalSourceInfo)]
public class ExternalSourceInfoRecord : BiffRecordRaw
{
  private const ushort DEF_DATASOURCETYPE_BITMASK = 7;
  private const int DefaultRecordSize = 12;
  [BiffRecordPos(0, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(0, 3, TFieldType.Bit)]
  private bool m_bOdbcConnection;
  [BiffRecordPos(0, 4, TFieldType.Bit)]
  private bool m_bSql;
  [BiffRecordPos(0, 5, TFieldType.Bit)]
  private bool m_bSqlSav;
  [BiffRecordPos(0, 6, TFieldType.Bit)]
  private bool m_bWeb;
  [BiffRecordPos(0, 7, TFieldType.Bit)]
  private bool m_bSavePassword;
  [BiffRecordPos(1, 0, TFieldType.Bit)]
  private bool m_bTablesOnlyHtml;
  [BiffRecordPos(2, 2)]
  private ushort m_usParamsCount;
  [BiffRecordPos(4, 2)]
  private ushort m_usQueryCount;
  [BiffRecordPos(6, 2)]
  private ushort m_usWebPostCount;
  [BiffRecordPos(8, 2)]
  private ushort m_usSQLSavCount;
  [BiffRecordPos(10, 2)]
  private ushort m_usOdbcConnectionCount;

  public ExternalSourceInfoRecord()
  {
  }

  public ExternalSourceInfoRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ExternalSourceInfoRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort Options => this.m_usOptions;

  public ushort DataSourceType
  {
    get => BiffRecordRaw.GetUInt16BitsByMask(this.m_usOptions, (ushort) 7);
    set
    {
      if (value < (ushort) 1 || value > (ushort) 4)
        throw new ArgumentOutOfRangeException(nameof (value), "Value cannot be less 1 and greater than 4");
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usOptions, (ushort) 7, value);
    }
  }

  public bool IsOdbcConnection
  {
    get => this.m_bOdbcConnection;
    set => this.m_bOdbcConnection = value;
  }

  public bool IsSql
  {
    get => this.m_bSql;
    set => this.m_bSql = value;
  }

  public bool IsSqlSav
  {
    get => this.m_bSqlSav;
    set => this.m_bSqlSav = value;
  }

  public bool IsWeb
  {
    get => this.m_bWeb;
    set => this.m_bWeb = value;
  }

  public bool IsSavePassword
  {
    get => this.m_bSavePassword;
    set => this.m_bSavePassword = value;
  }

  public bool IsTablesOnlyHtml
  {
    get => this.m_bTablesOnlyHtml;
    set => this.m_bTablesOnlyHtml = value;
  }

  public ushort ParamsCount
  {
    get => this.m_usParamsCount;
    set => this.m_usParamsCount = value;
  }

  public ushort QueryCount
  {
    get => this.m_usQueryCount;
    set => this.m_usQueryCount = value;
  }

  public ushort WebPostCount
  {
    get => this.m_usWebPostCount;
    set => this.m_usWebPostCount = value;
  }

  public ushort SQLSavCount
  {
    get => this.m_usSQLSavCount;
    set => this.m_usSQLSavCount = value;
  }

  public ushort OdbcConnectionCount
  {
    get => this.m_usOdbcConnectionCount;
    set => this.m_usOdbcConnectionCount = value;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bOdbcConnection = provider.ReadBit(iOffset, 3);
    this.m_bSql = provider.ReadBit(iOffset, 4);
    this.m_bSqlSav = provider.ReadBit(iOffset, 5);
    this.m_bWeb = provider.ReadBit(iOffset, 6);
    this.m_bSavePassword = provider.ReadBit(iOffset, 7);
    this.m_bTablesOnlyHtml = provider.ReadBit(iOffset + 1, 0);
    this.m_usParamsCount = provider.ReadUInt16(iOffset + 2);
    this.m_usQueryCount = provider.ReadUInt16(iOffset + 4);
    this.m_usWebPostCount = provider.ReadUInt16(iOffset + 6);
    this.m_usSQLSavCount = provider.ReadUInt16(iOffset + 8);
    this.m_usOdbcConnectionCount = provider.ReadUInt16(iOffset + 10);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bOdbcConnection, 3);
    provider.WriteBit(iOffset, this.m_bSql, 4);
    provider.WriteBit(iOffset, this.m_bSqlSav, 5);
    provider.WriteBit(iOffset, this.m_bWeb, 6);
    provider.WriteBit(iOffset, this.m_bSavePassword, 7);
    provider.WriteBit(iOffset + 1, this.m_bTablesOnlyHtml, 0);
    provider.WriteUInt16(iOffset + 2, this.m_usParamsCount);
    provider.WriteUInt16(iOffset + 4, this.m_usQueryCount);
    provider.WriteUInt16(iOffset + 6, this.m_usWebPostCount);
    provider.WriteUInt16(iOffset + 8, this.m_usSQLSavCount);
    provider.WriteUInt16(iOffset + 10, this.m_usOdbcConnectionCount);
    this.m_iLength = 12;
  }

  public override int GetStoreSize(ExcelVersion version) => 12;
}
