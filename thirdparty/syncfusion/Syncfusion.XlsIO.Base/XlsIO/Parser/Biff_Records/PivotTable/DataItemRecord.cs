// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.DataItemRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[Biff(TBIFFRecord.DataItem)]
[CLSCompliant(false)]
public class DataItemRecord : BiffRecordRawWithArray
{
  internal const ushort DEF_NULL_NAME_LENGTH = 65535 /*0xFFFF*/;
  private const int DEF_STRING_OFFSET = 14;
  [BiffRecordPos(0, 2)]
  private ushort m_usField;
  [BiffRecordPos(2, 2)]
  private ushort m_usFunctionIndex;
  [BiffRecordPos(4, 2)]
  private ushort m_usDisplayFormat;
  [BiffRecordPos(6, 2)]
  private ushort m_usViewFieldIndex;
  [BiffRecordPos(8, 2)]
  private ushort m_usViewItemIndex;
  [BiffRecordPos(10, 2)]
  private ushort m_usFormatTableIndex;
  [BiffRecordPos(12, 2)]
  private ushort m_usNameLength = ushort.MaxValue;
  private string m_strName;

  public DataItemRecord()
  {
  }

  public DataItemRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public DataItemRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort Field
  {
    get => this.m_usField;
    set => this.m_usField = value;
  }

  public ushort FunctionIndex
  {
    get => this.m_usFunctionIndex;
    set => this.m_usFunctionIndex = value;
  }

  public ushort DisplayFormat
  {
    get => this.m_usDisplayFormat;
    set => this.m_usDisplayFormat = value;
  }

  public ushort ViewFieldIndex
  {
    get => this.m_usViewFieldIndex;
    set => this.m_usViewFieldIndex = value;
  }

  public ushort ViewItemIndex
  {
    get => this.m_usViewItemIndex;
    set => this.m_usViewItemIndex = value;
  }

  public ushort FormatTableIndex
  {
    get => this.m_usFormatTableIndex;
    set => this.m_usFormatTableIndex = value;
  }

  public ushort NameLength => this.m_usNameLength;

  public string Name
  {
    get => this.m_strName;
    set
    {
      this.m_strName = value;
      if (value == null)
        this.m_usNameLength = ushort.MaxValue;
      else
        this.m_usNameLength = (ushort) value.Length;
    }
  }

  public override void ParseStructure()
  {
    this.m_usField = this.GetUInt16(0);
    this.m_usFunctionIndex = this.GetUInt16(2);
    this.m_usDisplayFormat = this.GetUInt16(4);
    this.m_usViewFieldIndex = this.GetUInt16(6);
    this.m_usViewItemIndex = this.GetUInt16(8);
    this.m_usFormatTableIndex = this.GetUInt16(10);
    this.m_usNameLength = this.GetUInt16(12);
    if (this.m_usNameLength == ushort.MaxValue)
      this.m_strName = (string) null;
    else
      this.m_strName = this.GetString(14, (int) this.m_usNameLength);
  }

  public override void InfillInternalData(ExcelVersion version)
  {
    this.m_iLength = 14;
    this.m_data = new byte[this.m_iLength];
    this.SetUInt16(0, this.m_usField);
    this.SetUInt16(2, this.m_usFunctionIndex);
    this.SetUInt16(4, this.m_usDisplayFormat);
    this.SetUInt16(6, this.m_usViewFieldIndex);
    this.SetUInt16(8, this.m_usViewItemIndex);
    this.SetUInt16(10, this.m_usFormatTableIndex);
    this.SetUInt16(12, this.m_usNameLength);
    if (this.m_strName == null)
      return;
    this.AutoGrowData = true;
    this.m_iLength += this.SetStringNoLen(this.m_iLength, this.m_strName, false, BiffRecordRawWithArray.IsAsciiString(this.m_strName));
  }
}
