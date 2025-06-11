// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.SQLDataTypeIdRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[Biff(TBIFFRecord.SQLDataTypeId)]
[CLSCompliant(false)]
public class SQLDataTypeIdRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usDataType;

  public SQLDataTypeIdRecord()
  {
  }

  public SQLDataTypeIdRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public SQLDataTypeIdRecord(int iReserve)
    : base(iReserve)
  {
  }

  public SQLDataTypeIdRecord.SQLDataType DataType
  {
    get => (SQLDataTypeIdRecord.SQLDataType) this.m_usDataType;
    set => this.m_usDataType = (ushort) value;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usDataType = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usDataType);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(ExcelVersion version) => 2;

  public enum SQLDataType
  {
    SQL_UNKNOWN_TYPE = 0,
    SQL_CHAR = 1,
    SQL_NUMERIC = 2,
    SQL_DECIMAL = 3,
    SQL_INTEGER = 4,
    SQL_SMALLINT = 5,
    SQL_FLOAT = 6,
    SQL_REAL = 7,
    SQL_DOUBLE = 8,
    SQL_DATETIME = 9,
    SQL_VARCHAR = 12, // 0x0000000C
  }
}
