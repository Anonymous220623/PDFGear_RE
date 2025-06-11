// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.ViewExtendedInfoRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ViewExtendedInfo)]
public class ViewExtendedInfoRecord : BiffRecordRawWithArray
{
  private const ushort DEF_WRAP_PAGE_MASK = 30;
  private const int DEF_WRAP_PAGE_START_BIT = 1;
  public const int DEF_WRAPPAGE_MAXVALUE = 15;
  private const int FirstStringOffset = 24;
  [BiffRecordPos(0, 2)]
  private ushort m_usFormat;
  [BiffRecordPos(2, 2, true)]
  private short m_sErrorStringLength;
  [BiffRecordPos(4, 2, true)]
  private short m_sNullStringLength;
  [BiffRecordPos(6, 2, true)]
  private short m_sTagLength;
  [BiffRecordPos(8, 2)]
  private ushort m_usSelectNumber;
  [BiffRecordPos(10, 2)]
  private ushort m_usFieldPerRow;
  [BiffRecordPos(12, 2)]
  private ushort m_usFieldPerColumn;
  [BiffRecordPos(14, 2)]
  private ushort m_usOptions1;
  [BiffRecordPos(14, 0, TFieldType.Bit)]
  private bool m_bAcrossPageLay;
  [BiffRecordPos(14, 5, TFieldType.Bit)]
  private bool m_bPreserveFormattingNow;
  [BiffRecordPos(14, 6, TFieldType.Bit)]
  private bool m_bManualUpdate;
  [BiffRecordPos(16 /*0x10*/, 2)]
  private ushort m_usOptions2;
  [BiffRecordPos(16 /*0x10*/, 0, TFieldType.Bit)]
  private bool m_bEnableWizard;
  [BiffRecordPos(16 /*0x10*/, 1, TFieldType.Bit)]
  private bool m_bEnableDrilldown;
  [BiffRecordPos(16 /*0x10*/, 2, TFieldType.Bit)]
  private bool m_bEnableFieldDialog;
  [BiffRecordPos(16 /*0x10*/, 3, TFieldType.Bit)]
  private bool m_bPreserveFormatting;
  [BiffRecordPos(16 /*0x10*/, 4, TFieldType.Bit)]
  private bool m_bMergeLabels;
  [BiffRecordPos(16 /*0x10*/, 5, TFieldType.Bit)]
  private bool m_bDisplayErrorString;
  [BiffRecordPos(16 /*0x10*/, 6, TFieldType.Bit)]
  private bool m_bDisplayNullString;
  [BiffRecordPos(16 /*0x10*/, 7, TFieldType.Bit)]
  private bool m_bSubtotalHiddenPageItems;
  [BiffRecordPos(18, 2, true)]
  private short m_sPageFieldStyleLength;
  [BiffRecordPos(20, 2, true)]
  private short m_sTableStyleLength;
  [BiffRecordPos(22, 2, true)]
  private short m_sVacateStyleLength;
  private string m_strErrorString;
  private string m_strNullString;
  private string m_strTag;
  private string m_strPageFieldStyle;
  private string m_strTableStyle;
  private string m_strVacateStyle;

  public ViewExtendedInfoRecord()
  {
  }

  public ViewExtendedInfoRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ViewExtendedInfoRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort Format
  {
    get => this.m_usFormat;
    set => this.m_usFormat = value;
  }

  public short ErrorStringLength => this.m_sErrorStringLength;

  public short NullStringLength => this.m_sNullStringLength;

  public short TagLength => this.m_sTagLength;

  public ushort SelectNumber
  {
    get => this.m_usSelectNumber;
    set => this.m_usSelectNumber = value;
  }

  public ushort FieldPerRow
  {
    get => this.m_usFieldPerRow;
    set => this.m_usFieldPerRow = value;
  }

  public ushort FieldPerColumn
  {
    get => this.m_usFieldPerColumn;
    set => this.m_usFieldPerColumn = value;
  }

  public ushort Options1 => this.m_usOptions1;

  public bool IsAcrossPageLay
  {
    get => this.m_bAcrossPageLay;
    set => this.m_bAcrossPageLay = value;
  }

  public bool IsPreserveFormattingNow
  {
    get => this.m_bPreserveFormattingNow;
    set => this.m_bPreserveFormattingNow = value;
  }

  public bool IsManualUpdate
  {
    get => this.m_bManualUpdate;
    set => this.m_bManualUpdate = value;
  }

  public ushort WrapPage
  {
    get => (ushort) ((uint) BiffRecordRaw.GetUInt16BitsByMask(this.m_usOptions1, (ushort) 30) >> 1);
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usOptions1, (ushort) 30, (ushort) ((uint) value << 1));
    }
  }

  public ushort Options2
  {
    get => this.m_usOptions2;
    set => this.m_usOptions2 = value;
  }

  public bool IsEnableWizard
  {
    get => this.m_bEnableWizard;
    set => this.m_bEnableWizard = value;
  }

  public bool IsEnableDrilldown
  {
    get => this.m_bEnableDrilldown;
    set => this.m_bEnableDrilldown = value;
  }

  public bool IsEnableFieldDialog
  {
    get => this.m_bEnableFieldDialog;
    set => this.m_bEnableFieldDialog = value;
  }

  public bool IsPreserveFormatting
  {
    get => this.m_bPreserveFormatting;
    set => this.m_bPreserveFormatting = value;
  }

  public bool IsMergeLabels
  {
    get => this.m_bMergeLabels;
    set => this.m_bMergeLabels = value;
  }

  public bool IsDisplayErrorString
  {
    get => this.m_bDisplayErrorString;
    set => this.m_bDisplayErrorString = value;
  }

  public bool IsDisplayNullString
  {
    get => this.m_bDisplayNullString;
    set => this.m_bDisplayNullString = value;
  }

  public bool IsSubtotalHiddenPageItems
  {
    get => this.m_bSubtotalHiddenPageItems;
    set => this.m_bSubtotalHiddenPageItems = value;
  }

  public short PageFieldStyleLength => this.m_sPageFieldStyleLength;

  public short TableStyleLength => this.m_sTableStyleLength;

  public short VacateStyleLength => this.m_sVacateStyleLength;

  public string ErrorString
  {
    get => this.m_strErrorString;
    set
    {
      this.m_strErrorString = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_sErrorStringLength = (short) value.Length;
    }
  }

  public string NullString
  {
    get => this.m_strNullString;
    set
    {
      this.m_strNullString = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_sNullStringLength = (short) value.Length;
    }
  }

  public string Tag
  {
    get => this.m_strTag;
    set
    {
      this.m_strTag = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_sTagLength = (short) value.Length;
    }
  }

  public string PageFieldStyle
  {
    get => this.m_strPageFieldStyle;
    set
    {
      this.m_strPageFieldStyle = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_sPageFieldStyleLength = (short) value.Length;
    }
  }

  public string TableStyle
  {
    get => this.m_strTableStyle;
    set
    {
      this.m_strTableStyle = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_sTableStyleLength = (short) value.Length;
    }
  }

  public string VacateStyle
  {
    get => this.m_strVacateStyle;
    set
    {
      this.m_strVacateStyle = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_sVacateStyleLength = (short) value.Length;
    }
  }

  public override void ParseStructure()
  {
    this.m_usFormat = this.GetUInt16(0);
    this.m_sErrorStringLength = this.GetInt16(2);
    this.m_sNullStringLength = this.GetInt16(4);
    this.m_sTagLength = this.GetInt16(6);
    this.m_usSelectNumber = this.GetUInt16(8);
    this.m_usFieldPerRow = this.GetUInt16(10);
    this.m_usFieldPerColumn = this.GetUInt16(12);
    this.m_usOptions1 = this.GetUInt16(14);
    this.m_bAcrossPageLay = this.GetBit(14, 0);
    this.m_bPreserveFormattingNow = this.GetBit(14, 5);
    this.m_bManualUpdate = this.GetBit(14, 6);
    this.m_usOptions2 = this.GetUInt16(16 /*0x10*/);
    this.m_bEnableWizard = this.GetBit(16 /*0x10*/, 0);
    this.m_bEnableDrilldown = this.GetBit(16 /*0x10*/, 1);
    this.m_bEnableFieldDialog = this.GetBit(16 /*0x10*/, 2);
    this.m_bPreserveFormatting = this.GetBit(16 /*0x10*/, 3);
    this.m_bMergeLabels = this.GetBit(16 /*0x10*/, 4);
    this.m_bDisplayErrorString = this.GetBit(16 /*0x10*/, 5);
    this.m_bDisplayNullString = this.GetBit(16 /*0x10*/, 6);
    this.m_bSubtotalHiddenPageItems = this.GetBit(16 /*0x10*/, 7);
    this.m_sPageFieldStyleLength = this.GetInt16(18);
    this.m_sTableStyleLength = this.GetInt16(20);
    this.m_sVacateStyleLength = this.GetInt16(22);
    if (this.m_iLength <= 24)
      return;
    int offset = 24;
    if (this.m_sErrorStringLength > (short) 0)
      this.m_strErrorString = this.GetStringUpdateOffset(ref offset, (int) this.m_sErrorStringLength);
    if (this.m_sNullStringLength > (short) 0)
      this.m_strNullString = this.GetStringUpdateOffset(ref offset, (int) this.m_sNullStringLength);
    if (this.m_sTagLength > (short) 0)
      this.m_strTag = this.GetStringUpdateOffset(ref offset, (int) this.m_sTagLength);
    if (this.m_sPageFieldStyleLength > (short) 0)
      this.m_strPageFieldStyle = this.GetStringUpdateOffset(ref offset, (int) this.m_sPageFieldStyleLength);
    if (this.m_sTableStyleLength > (short) 0)
      this.m_strTableStyle = this.GetStringUpdateOffset(ref offset, (int) this.m_sTableStyleLength);
    if (this.m_sVacateStyleLength <= (short) 0)
      return;
    this.m_strVacateStyle = this.GetStringUpdateOffset(ref offset, (int) this.m_sVacateStyleLength);
  }

  public override void InfillInternalData(ExcelVersion version)
  {
    this.SetUInt16(0, this.m_usFormat);
    this.SetInt16(2, this.m_sErrorStringLength);
    this.SetInt16(4, this.m_sNullStringLength);
    this.SetInt16(6, this.m_sTagLength);
    this.SetUInt16(8, this.m_usSelectNumber);
    this.SetUInt16(10, this.m_usFieldPerRow);
    this.SetUInt16(12, this.m_usFieldPerColumn);
    this.SetUInt16(14, this.m_usOptions1);
    this.SetBit(14, this.m_bAcrossPageLay, 0);
    this.SetBit(14, this.m_bPreserveFormattingNow, 5);
    this.SetBit(14, this.m_bManualUpdate, 6);
    this.SetUInt16(16 /*0x10*/, this.m_usOptions2);
    this.SetBit(16 /*0x10*/, this.m_bEnableWizard, 0);
    this.SetBit(16 /*0x10*/, this.m_bEnableDrilldown, 1);
    this.SetBit(16 /*0x10*/, this.m_bEnableFieldDialog, 2);
    this.SetBit(16 /*0x10*/, this.m_bPreserveFormatting, 3);
    this.SetBit(16 /*0x10*/, this.m_bMergeLabels, 4);
    this.SetBit(16 /*0x10*/, this.m_bDisplayErrorString, 5);
    this.SetBit(16 /*0x10*/, this.m_bDisplayNullString, 6);
    this.SetBit(16 /*0x10*/, this.m_bSubtotalHiddenPageItems, 7);
    this.SetInt16(18, this.m_sPageFieldStyleLength);
    this.SetInt16(20, this.m_sTableStyleLength);
    this.SetInt16(22, this.m_sVacateStyleLength);
    this.m_iLength = 24;
    this.AutoGrowData = true;
    if (this.m_sErrorStringLength > (short) 0)
      this.m_iLength += this.SetStringNoLen(this.m_iLength, this.m_strErrorString);
    if (this.m_sNullStringLength > (short) 0)
      this.m_iLength += this.SetStringNoLen(this.m_iLength, this.m_strNullString);
    if (this.m_sTagLength > (short) 0)
      this.m_iLength += this.SetStringNoLen(this.m_iLength, this.m_strTag);
    if (this.m_sPageFieldStyleLength > (short) 0)
      this.m_iLength += this.SetStringNoLen(this.m_iLength, this.m_strPageFieldStyle);
    if (this.m_sTableStyleLength > (short) 0)
      this.m_iLength += this.SetStringNoLen(this.m_iLength, this.m_strTableStyle);
    if (this.m_sVacateStyleLength <= (short) 0)
      return;
    this.m_iLength += this.SetStringNoLen(this.m_iLength, this.m_strVacateStyle);
  }
}
