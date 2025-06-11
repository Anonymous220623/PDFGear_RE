// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.DVRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Implementation.Exceptions;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.DV)]
[CLSCompliant(false)]
internal class DVRecord : BiffRecordRawWithArray, ICloneable
{
  public const uint DataTypeBitMask = 15;
  public const uint ErrorStyleBitMask = 112 /*0x70*/;
  public const uint ConditionBitMask = 15728640 /*0xF00000*/;
  public const int ErrorStyleStartBit = 4;
  public const int ConditionStartBit = 20;
  public const string StringEmpty = "\0";
  private const int DEF_FIXED_PART_SIZE = 14;
  [BiffRecordPos(0, 4)]
  private uint m_uiOptions;
  [BiffRecordPos(0, 7, TFieldType.Bit)]
  private bool m_bStrListExplicit;
  [BiffRecordPos(1, 0, TFieldType.Bit)]
  private bool m_bEmptyCell = true;
  [BiffRecordPos(1, 1, TFieldType.Bit)]
  private bool m_bSuppressArrow;
  [BiffRecordPos(2, 2, TFieldType.Bit)]
  private bool m_bShowPromptBox = true;
  [BiffRecordPos(2, 3, TFieldType.Bit)]
  private bool m_bShowErrorBox = true;
  private string m_strPromtBoxTitle = string.Empty;
  private bool m_bPromptBoxShort;
  private string m_strErrorBoxTitle = string.Empty;
  private bool m_bErrorBoxShort;
  private string m_strPromtBoxText = string.Empty;
  private bool m_bPromptBoxTextShort;
  private string m_strErrorBoxText = string.Empty;
  private bool m_bErrorBoxTextShort;
  private ushort m_usAddrListSize;
  private List<TAddr> m_arrAddrList = new List<TAddr>();
  private Ptg[] m_arrFirstFormulaTokens;
  private Ptg[] m_arrSecondFormulaTokens;

  public uint Options => this.m_uiOptions;

  public bool IsStrListExplicit
  {
    get => this.m_bStrListExplicit;
    set => this.m_bStrListExplicit = value;
  }

  public bool IsEmptyCell
  {
    get => this.m_bEmptyCell;
    set => this.m_bEmptyCell = value;
  }

  public bool IsSuppressArrow
  {
    get => this.m_bSuppressArrow;
    set => this.m_bSuppressArrow = value;
  }

  public bool IsShowPromptBox
  {
    get => this.m_bShowPromptBox;
    set => this.m_bShowPromptBox = value;
  }

  public bool IsShowErrorBox
  {
    get => this.m_bShowErrorBox;
    set => this.m_bShowErrorBox = value;
  }

  public ExcelDataType DataType
  {
    get => (ExcelDataType) BiffRecordRaw.GetUInt32BitsByMask(this.m_uiOptions, 15U);
    set => BiffRecordRaw.SetUInt32BitsByMask(ref this.m_uiOptions, 15U, (uint) value);
  }

  public ExcelErrorStyle ErrorStyle
  {
    get
    {
      return (ExcelErrorStyle) (BiffRecordRaw.GetUInt32BitsByMask(this.m_uiOptions, 112U /*0x70*/) >> 4);
    }
    set
    {
      BiffRecordRaw.SetUInt32BitsByMask(ref this.m_uiOptions, 112U /*0x70*/, (uint) value << 4);
    }
  }

  public ExcelDataValidationComparisonOperator Condition
  {
    get
    {
      return (ExcelDataValidationComparisonOperator) (BiffRecordRaw.GetUInt32BitsByMask(this.m_uiOptions, 15728640U /*0xF00000*/) >> 20);
    }
    set
    {
      BiffRecordRaw.SetUInt32BitsByMask(ref this.m_uiOptions, 15728640U /*0xF00000*/, (uint) value << 20);
    }
  }

  public string PromtBoxTitle
  {
    get => this.m_strPromtBoxTitle;
    set => this.m_strPromtBoxTitle = value;
  }

  public string ErrorBoxTitle
  {
    get => this.m_strErrorBoxTitle;
    set => this.m_strErrorBoxTitle = value;
  }

  public string PromtBoxText
  {
    get => this.m_strPromtBoxText;
    set => this.m_strPromtBoxText = value;
  }

  public string ErrorBoxText
  {
    get => this.m_strErrorBoxText;
    set => this.m_strErrorBoxText = value;
  }

  public Ptg[] FirstFormulaTokens
  {
    get => this.m_arrFirstFormulaTokens;
    set => this.m_arrFirstFormulaTokens = value;
  }

  public Ptg[] SecondFormulaTokens
  {
    get => this.m_arrSecondFormulaTokens;
    set => this.m_arrSecondFormulaTokens = value;
  }

  public ushort AddrListSize => this.m_usAddrListSize;

  public TAddr[] AddrList
  {
    get => this.m_arrAddrList.ToArray();
    set
    {
      this.m_arrAddrList.Clear();
      if (value != null)
        this.m_arrAddrList.AddRange((IEnumerable<TAddr>) value);
      this.m_usAddrListSize = (ushort) this.m_arrAddrList.Count;
    }
  }

  public override int MinimumRecordSize => 12;

  public DVRecord()
  {
  }

  public DVRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public DVRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure()
  {
    this.m_uiOptions = this.GetUInt32(0);
    this.m_bStrListExplicit = this.GetBit(0, 7);
    this.m_bEmptyCell = this.GetBit(1, 0);
    this.m_bSuppressArrow = this.GetBit(1, 1);
    this.m_bShowPromptBox = this.GetBit(2, 2);
    this.m_bShowErrorBox = this.GetBit(2, 3);
    int offset1 = 4;
    this.PromtBoxTitle = this.CreateEmptyString(this.GetString16BitUpdateOffset(ref offset1, out this.m_bPromptBoxShort));
    this.ErrorBoxTitle = this.CreateEmptyString(this.GetString16BitUpdateOffset(ref offset1, out this.m_bErrorBoxShort));
    this.PromtBoxText = this.CreateEmptyString(this.GetString16BitUpdateOffset(ref offset1, out this.m_bPromptBoxTextShort));
    this.ErrorBoxText = this.CreateEmptyString(this.GetString16BitUpdateOffset(ref offset1, out this.m_bErrorBoxTextShort));
    ushort uint16_1 = this.GetUInt16(offset1);
    int offset2 = offset1 + 4;
    byte[] bytes1 = this.GetBytes(offset2, (int) uint16_1);
    int offset3 = offset2 + (int) uint16_1;
    ushort uint16_2 = this.GetUInt16(offset3);
    int offset4 = offset3 + 4;
    byte[] bytes2 = this.GetBytes(offset4, (int) uint16_2);
    int offset5 = offset4 + (int) uint16_2;
    ByteArrayDataProvider provider = new ByteArrayDataProvider(bytes1);
    this.m_arrFirstFormulaTokens = FormulaUtil.ParseExpression((DataProvider) provider, (int) uint16_1, OfficeVersion.Excel97to2003);
    provider.SetBuffer(bytes2);
    this.m_arrSecondFormulaTokens = FormulaUtil.ParseExpression((DataProvider) provider, (int) uint16_2, OfficeVersion.Excel97to2003);
    this.m_usAddrListSize = this.GetUInt16(offset5);
    int offset6 = offset5 + 2;
    this.m_arrAddrList.Clear();
    int num = 0;
    while (num < (int) this.m_usAddrListSize)
    {
      this.m_arrAddrList.Add(this.GetAddr(offset6));
      ++num;
      offset6 += 8;
    }
    if (offset6 != this.m_iLength)
      throw new WrongBiffRecordDataException();
  }

  public override void InfillInternalData(OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    this.m_data = new byte[this.m_iLength];
    this.SetUInt32(0, this.m_uiOptions);
    this.SetBit(0, this.m_bStrListExplicit, 7);
    this.SetBit(1, this.m_bEmptyCell, 0);
    this.SetBit(1, this.m_bSuppressArrow, 1);
    this.SetBit(2, this.m_bShowPromptBox, 2);
    this.SetBit(2, this.m_bShowErrorBox, 3);
    int offset1 = 4;
    this.SetString16BitUpdateOffset(ref offset1, this.CreateNotEmptyString(this.m_strPromtBoxTitle), this.m_bPromptBoxShort);
    this.SetString16BitUpdateOffset(ref offset1, this.CreateNotEmptyString(this.m_strErrorBoxTitle), this.m_bErrorBoxShort);
    this.SetString16BitUpdateOffset(ref offset1, this.CreateNotEmptyString(this.m_strPromtBoxText), this.m_bPromptBoxTextShort);
    this.SetString16BitUpdateOffset(ref offset1, this.CreateNotEmptyString(this.m_strErrorBoxText), this.m_bErrorBoxTextShort);
    byte[] byteArray1 = FormulaUtil.PtgArrayToByteArray(this.m_arrFirstFormulaTokens, version);
    ushort length1 = byteArray1 != null ? (ushort) byteArray1.Length : (ushort) 0;
    this.SetUInt16(offset1, length1);
    int offset2 = offset1 + 2;
    this.SetUInt16(offset2, (ushort) 0);
    int offset3 = offset2 + 2;
    if (length1 > (ushort) 0)
    {
      this.SetBytes(offset3, byteArray1, 0, (int) length1);
      offset3 += (int) length1;
    }
    byte[] byteArray2 = FormulaUtil.PtgArrayToByteArray(this.m_arrSecondFormulaTokens, version);
    ushort length2 = byteArray2 != null ? (ushort) byteArray2.Length : (ushort) 0;
    this.SetUInt16(offset3, length2);
    int offset4 = offset3 + 2;
    this.SetUInt16(offset4, (ushort) 0);
    int offset5 = offset4 + 2;
    if (length2 > (ushort) 0)
    {
      this.SetBytes(offset5, byteArray2, 0, (int) length2);
      offset5 += (int) length2;
    }
    this.SetUInt16(offset5, this.m_usAddrListSize);
    int offset6 = offset5 + 2;
    int index = 0;
    while (index < (int) this.m_usAddrListSize)
    {
      this.SetAddr(offset6, this.m_arrAddrList[index]);
      ++index;
      offset6 += 8;
    }
  }

  public void Add(TAddr addrToAdd)
  {
    this.m_arrAddrList.Add(addrToAdd);
    ++this.m_usAddrListSize;
  }

  public void AddRange(TAddr[] addrToAdd)
  {
    this.m_arrAddrList.AddRange((IEnumerable<TAddr>) addrToAdd);
    this.m_usAddrListSize = (ushort) this.m_arrAddrList.Count;
  }

  public void AddRange(ICollection<TAddr> addrToAdd)
  {
    this.m_arrAddrList.AddRange((IEnumerable<TAddr>) addrToAdd);
    this.m_usAddrListSize = (ushort) this.m_arrAddrList.Count;
  }

  public void ClearAddressList() => this.m_arrAddrList.Clear();

  public static int GetFormulaSize(
    Ptg[] arrTokens,
    OfficeVersion version,
    bool addAdditionalDataSize)
  {
    if (arrTokens == null)
      return 0;
    int length = arrTokens.Length;
    if (length == 0)
      return 0;
    int formulaSize = 0;
    for (int index = 0; index < length; ++index)
    {
      Ptg arrToken = arrTokens[index];
      formulaSize += arrToken.GetSize(version);
      if (addAdditionalDataSize && arrToken is IAdditionalData additionalData)
        formulaSize += additionalData.AdditionalDataSize;
    }
    return formulaSize;
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    return 14 + this.Get16BitStringSize(this.CreateNotEmptyString(this.m_strPromtBoxTitle), this.m_bPromptBoxShort) + this.Get16BitStringSize(this.CreateNotEmptyString(this.m_strErrorBoxTitle), this.m_bErrorBoxShort) + this.Get16BitStringSize(this.CreateNotEmptyString(this.m_strPromtBoxText), this.m_bPromptBoxTextShort) + this.Get16BitStringSize(this.CreateNotEmptyString(this.m_strErrorBoxText), this.m_bErrorBoxTextShort) + DVRecord.GetFormulaSize(this.m_arrFirstFormulaTokens, version, true) + DVRecord.GetFormulaSize(this.m_arrSecondFormulaTokens, version, true) + (int) this.m_usAddrListSize * 8;
  }

  private string CreateNotEmptyString(string strToModify)
  {
    if (strToModify == null || strToModify.Length == 0)
      strToModify = "\0";
    return strToModify;
  }

  private string CreateEmptyString(string strToModify)
  {
    if (strToModify == "\0")
      strToModify = string.Empty;
    return strToModify;
  }

  public new object Clone()
  {
    DVRecord dvRecord = (DVRecord) base.Clone();
    dvRecord.m_arrFirstFormulaTokens = CloneUtils.ClonePtgArray(this.m_arrFirstFormulaTokens);
    dvRecord.m_arrSecondFormulaTokens = CloneUtils.ClonePtgArray(this.m_arrSecondFormulaTokens);
    int count = this.m_arrAddrList.Count;
    dvRecord.m_arrAddrList = new List<TAddr>(count);
    for (int index = 0; index < count; ++index)
      dvRecord.m_arrAddrList.Add(this.m_arrAddrList[index]);
    return (object) dvRecord;
  }

  public override bool Equals(object obj)
  {
    return obj is DVRecord dvRecord && dvRecord.IsStrListExplicit == this.IsStrListExplicit && dvRecord.IsEmptyCell == this.IsEmptyCell && dvRecord.IsSuppressArrow == this.IsSuppressArrow && dvRecord.IsShowPromptBox == this.IsShowPromptBox && dvRecord.IsShowErrorBox == this.IsShowErrorBox && dvRecord.DataType == this.DataType && dvRecord.ErrorStyle == this.ErrorStyle && dvRecord.Condition == this.Condition && dvRecord.PromtBoxTitle == this.PromtBoxTitle && dvRecord.ErrorBoxTitle == this.ErrorBoxTitle && dvRecord.PromtBoxText == this.PromtBoxText && dvRecord.ErrorBoxText == this.ErrorBoxText && Ptg.CompareArrays(dvRecord.FirstFormulaTokens, this.FirstFormulaTokens) && Ptg.CompareArrays(dvRecord.SecondFormulaTokens, this.SecondFormulaTokens);
  }

  public override int GetHashCode()
  {
    int length1 = this.m_arrFirstFormulaTokens == null ? 0 : this.m_arrFirstFormulaTokens.Length;
    int length2 = this.m_arrSecondFormulaTokens == null ? 0 : this.m_arrSecondFormulaTokens.Length;
    int num = 0;
    for (int index = 0; index < this.AddrList.Length; ++index)
      num += this.AddrList.GetValue(index).GetHashCode();
    return this.IsStrListExplicit.GetHashCode() ^ this.IsEmptyCell.GetHashCode() ^ this.IsSuppressArrow.GetHashCode() ^ this.IsShowPromptBox.GetHashCode() ^ this.IsShowErrorBox.GetHashCode() ^ this.DataType.GetHashCode() ^ this.ErrorStyle.GetHashCode() ^ this.Condition.GetHashCode() ^ this.PromtBoxTitle.GetHashCode() ^ this.ErrorBoxTitle.GetHashCode() ^ this.PromtBoxText.GetHashCode() ^ this.ErrorBoxText.GetHashCode() ^ length1.GetHashCode() ^ length2.GetHashCode() ^ this.AddrListSize.GetHashCode() ^ num.GetHashCode();
  }
}
