// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotViewFieldsRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[Biff(TBIFFRecord.PivotViewFields)]
[CLSCompliant(false)]
public class PivotViewFieldsRecord : BiffRecordRawWithArray
{
  private const ushort DEF_NULL_LENGTH = 65535 /*0xFFFF*/;
  private const int DEF_STRING_OFFSET = 10;
  [BiffRecordPos(0, 2)]
  private ushort m_usAxis;
  [BiffRecordPos(2, 2)]
  private ushort m_usSubtotalCount;
  [BiffRecordPos(4, 2)]
  private ushort m_usSubtotalType;
  [BiffRecordPos(6, 2)]
  private ushort m_usNumberItems;
  [BiffRecordPos(8, 2)]
  private ushort m_usNameLength;
  private string m_strName;

  public PivotViewFieldsRecord()
  {
  }

  public PivotViewFieldsRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotViewFieldsRecord(int iReserve)
    : base(iReserve)
  {
  }

  public PivotAxisTypes Axis
  {
    get => (PivotAxisTypes) this.m_usAxis;
    set => this.m_usAxis = (ushort) value;
  }

  public ushort SubtotalCount
  {
    get => this.m_usSubtotalCount;
    set => this.m_usSubtotalCount = value;
  }

  public PivotSubtotalTypes SubtotalType
  {
    get => (PivotSubtotalTypes) this.m_usSubtotalType;
    set => this.m_usSubtotalType = (ushort) value;
  }

  public ushort NumberItems
  {
    get => this.m_usNumberItems;
    set => this.m_usNumberItems = value;
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
        this.m_usNameLength = value.Length <= (int) ushort.MaxValue ? (ushort) value.Length : throw new ArgumentOutOfRangeException("value.Length", "Value cannot be greater DEF_NULL_LENGTH");
    }
  }

  public override void ParseStructure()
  {
    this.m_usAxis = this.GetUInt16(0);
    this.m_usSubtotalCount = this.GetUInt16(2);
    this.m_usSubtotalType = this.GetUInt16(4);
    this.m_usNumberItems = this.GetUInt16(6);
    this.m_usNameLength = this.GetUInt16(8);
    this.m_strName = (string) null;
    if (this.m_usNameLength == ushort.MaxValue)
      return;
    this.m_strName = this.GetString(10, (int) this.m_usNameLength, out int _, false);
  }

  public override void InfillInternalData(ExcelVersion version)
  {
    this.m_iLength = 10;
    this.m_data = new byte[this.m_iLength];
    this.SetUInt16(0, this.m_usAxis);
    this.SetUInt16(2, this.m_usSubtotalCount);
    this.SetUInt16(4, this.m_usSubtotalType);
    this.SetUInt16(6, this.m_usNumberItems);
    this.SetUInt16(8, this.m_usNameLength);
    if (this.m_strName == null)
      return;
    this.m_iLength += this.SetStringNoLenDetectEncoding(10, this.m_strName);
  }
}
