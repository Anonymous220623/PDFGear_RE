// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotViewDefinitionRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.PivotViewDefinition)]
public class PivotViewDefinitionRecord : BiffRecordRawWithArray
{
  private const int DEF_TABLE_NAME_OFFSET = 44;
  [BiffRecordPos(0, 2)]
  private ushort m_usFirstRow;
  [BiffRecordPos(2, 2)]
  private ushort m_usLastRow;
  [BiffRecordPos(4, 2)]
  private ushort m_usFirstColumn;
  [BiffRecordPos(6, 2)]
  private ushort m_usLastColumn;
  [BiffRecordPos(8, 2)]
  private ushort m_usFirstHeadRow;
  [BiffRecordPos(10, 2)]
  private ushort m_usFirstDataRow;
  [BiffRecordPos(12, 2)]
  private ushort m_usFirstDataColumn;
  [BiffRecordPos(14, 2)]
  private ushort m_usCacheIndex;
  [BiffRecordPos(16 /*0x10*/, 2)]
  private ushort m_usReserved;
  [BiffRecordPos(18, 2)]
  private ushort m_usDataAxis;
  [BiffRecordPos(20, 2)]
  private ushort m_usDataPos;
  [BiffRecordPos(22, 2)]
  private ushort m_usFieldsNumber;
  [BiffRecordPos(24, 2)]
  private ushort m_usRowFieldsNumber;
  [BiffRecordPos(26, 2)]
  private ushort m_usColumnFieldsNumber;
  [BiffRecordPos(28, 2)]
  private ushort m_usPageFieldsNumber;
  [BiffRecordPos(30, 2)]
  private ushort m_usDataFieldsNumber;
  [BiffRecordPos(32 /*0x20*/, 2)]
  private ushort m_usDataRowsNumber;
  [BiffRecordPos(34, 2)]
  private ushort m_usDataColumnsNumber;
  [BiffRecordPos(36, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(36, 0, TFieldType.Bit)]
  private bool m_bRowGrand;
  [BiffRecordPos(36, 1, TFieldType.Bit)]
  private bool m_bColumnGrand;
  [BiffRecordPos(36, 3, TFieldType.Bit)]
  private bool m_bAutoFormat;
  [BiffRecordPos(36, 4, TFieldType.Bit)]
  private bool m_bWHAutoFormat;
  [BiffRecordPos(36, 5, TFieldType.Bit)]
  private bool m_bFontAutoFormat;
  [BiffRecordPos(36, 6, TFieldType.Bit)]
  private bool m_bAlignAutoFormat;
  [BiffRecordPos(36, 7, TFieldType.Bit)]
  private bool m_bBorderAutoFormat;
  [BiffRecordPos(37, 0, TFieldType.Bit)]
  private bool m_bPatternAutoFormat;
  [BiffRecordPos(37, 1, TFieldType.Bit)]
  private bool m_bNumberAutoFormat;
  [BiffRecordPos(38, 2)]
  private ushort m_usAutoFormatIndex;
  [BiffRecordPos(40, 2)]
  private ushort m_usTableNameLength;
  [BiffRecordPos(42, 2)]
  private ushort m_usDataFieldNameLength;
  private string m_strTableName;
  private string m_strDataFieldName;

  public PivotViewDefinitionRecord()
  {
  }

  public PivotViewDefinitionRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotViewDefinitionRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort FirstRow
  {
    get => this.m_usFirstRow;
    set => this.m_usFirstRow = value;
  }

  public ushort LastRow
  {
    get => this.m_usLastRow;
    set => this.m_usLastRow = value;
  }

  public ushort FirstColumn
  {
    get => this.m_usFirstColumn;
    set => this.m_usFirstColumn = value;
  }

  public ushort LastColumn
  {
    get => this.m_usLastColumn;
    set => this.m_usLastColumn = value;
  }

  public ushort FirstHeadRow
  {
    get => this.m_usFirstHeadRow;
    set => this.m_usFirstHeadRow = value;
  }

  public ushort FirstDataRow
  {
    get => this.m_usFirstDataRow;
    set => this.m_usFirstDataRow = value;
  }

  public ushort FirstDataColumn
  {
    get => this.m_usFirstDataColumn;
    set => this.m_usFirstDataColumn = value;
  }

  public ushort CacheIndex
  {
    get => this.m_usCacheIndex;
    set => this.m_usCacheIndex = value;
  }

  public ushort Reserved => this.m_usReserved;

  public ushort DataAxis
  {
    get => this.m_usDataAxis;
    set => this.m_usDataAxis = value;
  }

  public ushort DataPos
  {
    get => this.m_usDataPos;
    set => this.m_usDataPos = value;
  }

  public ushort FieldsNumber
  {
    get => this.m_usFieldsNumber;
    set => this.m_usFieldsNumber = value;
  }

  public ushort RowFieldsNumber
  {
    get => this.m_usRowFieldsNumber;
    set => this.m_usRowFieldsNumber = value;
  }

  public ushort ColumnFieldsNumber
  {
    get => this.m_usColumnFieldsNumber;
    set => this.m_usColumnFieldsNumber = value;
  }

  public ushort PageFieldsNumber
  {
    get => this.m_usPageFieldsNumber;
    set => this.m_usPageFieldsNumber = value;
  }

  public ushort DataFieldsNumber
  {
    get => this.m_usDataFieldsNumber;
    set => this.m_usDataFieldsNumber = value;
  }

  public ushort DataRowsNumber
  {
    get => this.m_usDataRowsNumber;
    set => this.m_usDataRowsNumber = value;
  }

  public ushort DataColumnsNumber
  {
    get => this.m_usDataColumnsNumber;
    set => this.m_usDataColumnsNumber = value;
  }

  public ushort Options => this.m_usOptions;

  public bool IsRowGrand
  {
    get => this.m_bRowGrand;
    set => this.m_bRowGrand = value;
  }

  public bool IsColumnGrand
  {
    get => this.m_bColumnGrand;
    set => this.m_bColumnGrand = value;
  }

  public bool IsAutoFormat
  {
    get => this.m_bAutoFormat;
    set => this.m_bAutoFormat = value;
  }

  public bool IsWHAutoFormat
  {
    get => this.m_bWHAutoFormat;
    set => this.m_bWHAutoFormat = value;
  }

  public bool IsFontAutoFormat
  {
    get => this.m_bFontAutoFormat;
    set => this.m_bFontAutoFormat = value;
  }

  public bool IsAlignAutoFormat
  {
    get => this.m_bAlignAutoFormat;
    set => this.m_bAlignAutoFormat = value;
  }

  public bool IsBorderAutoFormat
  {
    get => this.m_bBorderAutoFormat;
    set => this.m_bBorderAutoFormat = value;
  }

  public bool IsPatternAutoFormat
  {
    get => this.m_bPatternAutoFormat;
    set => this.m_bPatternAutoFormat = value;
  }

  public bool IsNumberAutoFormat
  {
    get => this.m_bNumberAutoFormat;
    set => this.m_bNumberAutoFormat = value;
  }

  public ushort AutoFormatIndex
  {
    get => this.m_usAutoFormatIndex;
    set => this.m_usAutoFormatIndex = value;
  }

  public ushort TableNameLength => this.m_usTableNameLength;

  public ushort DataFieldNameLength => this.m_usDataFieldNameLength;

  public string TableName
  {
    get => this.m_strTableName;
    set
    {
      this.m_strTableName = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_usTableNameLength = (ushort) value.Length;
    }
  }

  public string DataFieldName
  {
    get => this.m_strDataFieldName;
    set
    {
      this.m_strDataFieldName = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_usDataFieldNameLength = (ushort) value.Length;
    }
  }

  public override void ParseStructure()
  {
    this.m_usFirstRow = this.GetUInt16(0);
    this.m_usLastRow = this.GetUInt16(2);
    this.m_usFirstColumn = this.GetUInt16(4);
    this.m_usLastColumn = this.GetUInt16(6);
    this.m_usFirstHeadRow = this.GetUInt16(8);
    this.m_usFirstDataRow = this.GetUInt16(10);
    this.m_usFirstDataColumn = this.GetUInt16(12);
    this.m_usCacheIndex = this.GetUInt16(14);
    this.m_usReserved = this.GetUInt16(16 /*0x10*/);
    this.m_usDataAxis = this.GetUInt16(18);
    this.m_usDataPos = this.GetUInt16(20);
    this.m_usFieldsNumber = this.GetUInt16(22);
    this.m_usRowFieldsNumber = this.GetUInt16(24);
    this.m_usColumnFieldsNumber = this.GetUInt16(26);
    this.m_usPageFieldsNumber = this.GetUInt16(28);
    this.m_usDataFieldsNumber = this.GetUInt16(30);
    this.m_usDataRowsNumber = this.GetUInt16(32 /*0x20*/);
    this.m_usDataColumnsNumber = this.GetUInt16(34);
    this.m_usOptions = this.GetUInt16(36);
    this.m_bRowGrand = this.GetBit(36, 0);
    this.m_bColumnGrand = this.GetBit(36, 1);
    this.m_bAutoFormat = this.GetBit(36, 3);
    this.m_bWHAutoFormat = this.GetBit(36, 4);
    this.m_bFontAutoFormat = this.GetBit(36, 5);
    this.m_bAlignAutoFormat = this.GetBit(36, 6);
    this.m_bBorderAutoFormat = this.GetBit(36, 7);
    this.m_bPatternAutoFormat = this.GetBit(37, 0);
    this.m_bNumberAutoFormat = this.GetBit(37, 1);
    this.m_usAutoFormatIndex = this.GetUInt16(38);
    this.m_usTableNameLength = this.GetUInt16(40);
    this.m_usDataFieldNameLength = this.GetUInt16(42);
    int offset = 44;
    this.m_strTableName = this.GetStringUpdateOffset(ref offset, (int) this.m_usTableNameLength);
    this.m_strDataFieldName = this.GetString(offset, (int) this.m_usDataFieldNameLength);
  }

  public override void InfillInternalData(ExcelVersion version)
  {
    this.m_data = new byte[44];
    this.SetUInt16(0, this.m_usFirstRow);
    this.SetUInt16(2, this.m_usLastRow);
    this.SetUInt16(4, this.m_usFirstColumn);
    this.SetUInt16(6, this.m_usLastColumn);
    this.SetUInt16(8, this.m_usFirstHeadRow);
    this.SetUInt16(10, this.m_usFirstDataRow);
    this.SetUInt16(12, this.m_usFirstDataColumn);
    this.SetUInt16(14, this.m_usCacheIndex);
    this.SetUInt16(16 /*0x10*/, this.m_usReserved);
    this.SetUInt16(18, this.m_usDataAxis);
    this.SetUInt16(20, this.m_usDataPos);
    this.SetUInt16(22, this.m_usFieldsNumber);
    this.SetUInt16(24, this.m_usRowFieldsNumber);
    this.SetUInt16(26, this.m_usColumnFieldsNumber);
    this.SetUInt16(28, this.m_usPageFieldsNumber);
    this.SetUInt16(30, this.m_usDataFieldsNumber);
    this.SetUInt16(32 /*0x20*/, this.m_usDataRowsNumber);
    this.SetUInt16(34, this.m_usDataColumnsNumber);
    this.SetUInt16(36, this.m_usOptions);
    this.SetBit(36, this.m_bRowGrand, 0);
    this.SetBit(36, this.m_bColumnGrand, 1);
    this.SetBit(36, this.m_bAutoFormat, 3);
    this.SetBit(36, this.m_bWHAutoFormat, 4);
    this.SetBit(36, this.m_bFontAutoFormat, 5);
    this.SetBit(36, this.m_bAlignAutoFormat, 6);
    this.SetBit(36, this.m_bBorderAutoFormat, 7);
    this.SetBit(37, this.m_bPatternAutoFormat, 0);
    this.SetBit(37, this.m_bNumberAutoFormat, 1);
    this.SetUInt16(38, this.m_usAutoFormatIndex);
    this.SetUInt16(40, this.m_usTableNameLength);
    this.SetUInt16(42, this.m_usDataFieldNameLength);
    this.m_iLength = 44;
    this.m_iLength += this.SetStringNoLen(44, this.m_strTableName, false, true);
    this.m_iLength += this.SetStringNoLen(this.m_iLength, this.m_strDataFieldName, false, BiffRecordRawWithArray.IsAsciiString(this.m_strDataFieldName));
  }
}
