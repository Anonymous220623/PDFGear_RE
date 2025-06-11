// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.FormulaRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.Formula)]
[CLSCompliant(false)]
public class FormulaRecord : 
  CellPositionBase,
  ICellPositionFormat,
  ICloneable,
  IDoubleValue,
  IFormulaRecord
{
  public const ulong DEF_FIRST_MASK = 18446462598732841215 /*0xFFFF0000000000FF*/;
  public const ulong DEF_BOOL_MASK = 18446462598732840961 /*0xFFFF000000000001*/;
  public const ulong DEF_ERROR_MASK = 18446462598732840962 /*0xFFFF000000000002*/;
  public const ulong DEF_BLANK_MASK = 18446462598732840963 /*0xFFFF000000000003*/;
  public const ulong DEF_STRING_MASK = 18446462598732840960 /*0xFFFF000000000000*/;
  public const ulong DEF_STRING_MASK_VALUE = 18446470295314235392 /*0xFFFF070000000000*/;
  private const int DEF_FIXED_SIZE = 22;
  private const ulong DEF_STRING_VALUE_ULONG = 18446471231618678784;
  private const ulong DEF_BLANK_VALUE_ULONG = 18446465898945477895;
  private const int FormulaValueOffset = 10;
  private const int DataSizeBeforeExpression = 16 /*0x10*/;
  public static readonly long DEF_STRING_VALUE_LONG = -272842090872832;
  public static readonly long DEF_BLANK_VALUE_LONG;
  public static readonly double DEF_STRING_VALUE = BitConverterGeneral.Int64BitsToDouble(FormulaRecord.DEF_STRING_VALUE_LONG);
  [BiffRecordPos(6, 8, TFieldType.Float)]
  private double m_dbValue;
  [BiffRecordPos(14, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(14, 0, TFieldType.Bit)]
  private bool m_bRecalculateAlways;
  [BiffRecordPos(14, 1, TFieldType.Bit)]
  private bool m_bCalculateOnOpen;
  [BiffRecordPos(14, 3, TFieldType.Bit)]
  private bool m_bPartOfSharedFormula;
  [BiffRecordPos(16 /*0x10*/, 4, true)]
  private int m_iReserved;
  [BiffRecordPos(20, 2)]
  private ushort m_usExpressionLen;
  private byte[] m_expression;
  private Ptg[] m_arrParsedExpression;
  private bool m_bFillFromExpression;

  internal byte[] Expression
  {
    get => this.m_expression;
    set => this.m_expression = value;
  }

  public double Value
  {
    get => this.m_dbValue;
    set => this.m_dbValue = value;
  }

  public ushort Options
  {
    get => this.m_usOptions;
    set => this.m_usOptions = value;
  }

  public bool RecalculateAlways
  {
    get => this.m_bRecalculateAlways;
    set => this.m_bRecalculateAlways = value;
  }

  public bool CalculateOnOpen
  {
    get => this.m_bCalculateOnOpen;
    set => this.m_bCalculateOnOpen = value;
  }

  public bool PartOfSharedFormula
  {
    get => this.m_bPartOfSharedFormula;
    set => this.m_bPartOfSharedFormula = value;
  }

  public Ptg[] ParsedExpression
  {
    get => this.m_arrParsedExpression;
    set
    {
      this.m_arrParsedExpression = value;
      if (value != null)
      {
        int formulaLen;
        this.m_expression = FormulaUtil.PtgArrayToByteArray(value, out formulaLen, ExcelVersion.Excel2007);
        this.m_usExpressionLen = (ushort) formulaLen;
      }
      else
      {
        this.m_expression = (byte[]) null;
        this.m_usExpressionLen = (ushort) 0;
      }
    }
  }

  public int Reserved => this.m_iReserved;

  public override int MinimumRecordSize => 24;

  public bool IsFillFromExpression
  {
    get => this.m_bFillFromExpression;
    set => this.m_bFillFromExpression = value;
  }

  public double DoubleValue => this.m_dbValue;

  public bool IsBool
  {
    get
    {
      return (BitConverterGeneral.DoubleToInt64Bits(this.m_dbValue) & -281474976710401L /*0xFFFF0000000000FF*/) == -281474976710655L /*0xFFFF000000000001*/;
    }
  }

  public bool IsError
  {
    get
    {
      return (BitConverterGeneral.DoubleToInt64Bits(this.m_dbValue) & -281474976710401L /*0xFFFF0000000000FF*/) == -281474976710654L /*0xFFFF000000000002*/;
    }
  }

  public bool IsBlank
  {
    get
    {
      return (BitConverterGeneral.DoubleToInt64Bits(this.m_dbValue) & -281474976710401L /*0xFFFF0000000000FF*/) == -281474976710653L /*0xFFFF000000000003*/;
    }
  }

  public bool HasString
  {
    get
    {
      return (BitConverterGeneral.DoubleToInt64Bits(this.m_dbValue) & -281474976710401L /*0xFFFF0000000000FF*/) == -281474976710656L /*0xFFFF000000000000*/;
    }
    set => this.m_dbValue = FormulaRecord.DEF_STRING_VALUE;
  }

  public bool BooleanValue
  {
    get
    {
      return this.IsBool && (BitConverterGeneral.DoubleToInt64Bits(this.m_dbValue) & 16711680L /*0xFF0000*/) > 0L;
    }
    set => this.SetBoolErrorValue(value ? (byte) 1 : (byte) 0, false);
  }

  public byte ErrorValue
  {
    get
    {
      return this.IsError ? (byte) ((BitConverterGeneral.DoubleToInt64Bits(this.m_dbValue) & 16711680L /*0xFF0000*/) >> 16 /*0x10*/) : (byte) 0;
    }
    set => this.SetBoolErrorValue(value, true);
  }

  static FormulaRecord() => FormulaRecord.DEF_BLANK_VALUE_LONG = -278174764073721L;

  protected override void ParseCellData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_dbValue = provider.ReadDouble(iOffset);
    iOffset += 8;
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bPartOfSharedFormula = provider.ReadBit(iOffset, 3);
    this.m_bCalculateOnOpen = provider.ReadBit(iOffset, 1);
    this.m_bRecalculateAlways = provider.ReadBit(iOffset, 0);
    iOffset += 2;
    this.m_iReserved = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_usExpressionLen = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_expression = new byte[(int) this.m_usExpressionLen];
    provider.ReadArray(iOffset, this.m_expression);
    this.ParseFormula(provider, iOffset, version);
  }

  protected override void InfillCellData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.PrepareExpression(version);
    provider.WriteDouble(iOffset, this.m_dbValue);
    iOffset += 8;
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bPartOfSharedFormula, 3);
    provider.WriteBit(iOffset, this.m_bCalculateOnOpen, 1);
    provider.WriteBit(iOffset, this.m_bRecalculateAlways, 0);
    iOffset += 2;
    provider.WriteInt32(iOffset, this.m_iReserved);
    iOffset += 4;
    provider.WriteUInt16(iOffset, this.m_usExpressionLen);
    iOffset += 2;
    if (this.m_usExpressionLen == (ushort) 0)
      return;
    provider.WriteBytes(iOffset, this.m_expression, 0, this.m_expression.Length);
  }

  private void ParseFormula(DataProvider provider, int iOffset, ExcelVersion version)
  {
    int offset = iOffset;
    int finalOffset;
    try
    {
      this.ParsedExpression = FormulaUtil.ParseExpression(provider, offset, (int) this.m_usExpressionLen, out finalOffset, version);
    }
    catch (Exception ex)
    {
      finalOffset = 0;
    }
  }

  private void PrepareExpression(ExcelVersion version)
  {
    if (this.m_arrParsedExpression != null && this.m_arrParsedExpression.Length > 0)
    {
      int formulaLen;
      this.m_expression = FormulaUtil.PtgArrayToByteArray(this.ParsedExpression, out formulaLen, version);
      this.m_usExpressionLen = (ushort) formulaLen;
    }
    else
    {
      this.m_expression = (byte[]) null;
      this.m_usExpressionLen = (ushort) 0;
    }
    this.m_bFillFromExpression = false;
  }

  private void SetBoolErrorValue(byte value, bool bIsError)
  {
    this.m_dbValue = FormulaRecord.GetBoolErrorValue(value, bIsError);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int storeSize = 22 + DVRecord.GetFormulaSize(this.m_arrParsedExpression, version, true);
    if (version != ExcelVersion.Excel97to2003)
      storeSize += 4;
    return storeSize;
  }

  public static double GetBoolErrorValue(byte value, bool bIsError)
  {
    return BitConverterGeneral.Int64BitsToDouble(((1099494850560L /*0xFFFF000000*/ << 8) + (long) value << 16 /*0x10*/) + (bIsError ? 2L : 1L));
  }

  public static void SetStringValue(
    DataProvider dataProvider,
    int iFormulaOffset,
    ExcelVersion version)
  {
    if (version != ExcelVersion.Excel97to2003)
      iFormulaOffset += 4;
    dataProvider.WriteInt64(iFormulaOffset + 10, FormulaRecord.DEF_STRING_VALUE_LONG);
  }

  public static void SetBlankValue(
    DataProvider dataProvider,
    int iFormulaOffset,
    ExcelVersion version)
  {
    if (version != ExcelVersion.Excel97to2003)
      iFormulaOffset += 4;
    dataProvider.WriteInt64(iFormulaOffset + 10, FormulaRecord.DEF_BLANK_VALUE_LONG);
  }

  public static Ptg[] ReadValue(DataProvider provider, int recordStart, ExcelVersion version)
  {
    recordStart += 10;
    if (version != ExcelVersion.Excel97to2003)
      recordStart += 4;
    recordStart += 14;
    int iExpressionLength = (int) provider.ReadUInt16(recordStart);
    recordStart += 2;
    return FormulaUtil.ParseExpression(provider, recordStart, iExpressionLength, out int _, version);
  }

  public static long ReadInt64Value(DataProvider provider, int recordStart, ExcelVersion version)
  {
    recordStart += 10;
    if (version != ExcelVersion.Excel97to2003)
      recordStart += 4;
    return provider.ReadInt64(recordStart);
  }

  public static double ReadDoubleValue(
    DataProvider provider,
    int recordStart,
    ExcelVersion version)
  {
    recordStart += 10;
    if (version != ExcelVersion.Excel97to2003)
      recordStart += 4;
    return provider.ReadDouble(recordStart);
  }

  public static void WriteDoubleValue(
    DataProvider provider,
    int recordStart,
    ExcelVersion version,
    double value)
  {
    recordStart += 10;
    if (version != ExcelVersion.Excel97to2003)
      recordStart += 4;
    provider.WriteDouble(recordStart, value);
  }

  public static void UpdateOptions(DataProvider provider, int iOffset)
  {
    byte num = (byte) ((uint) (byte) ((uint) provider.ReadByte(iOffset + 18) | 3U) & 247U);
    provider.WriteByte(iOffset + 18, num);
  }

  public new object Clone()
  {
    FormulaRecord formulaRecord = (FormulaRecord) base.Clone();
    if (this.m_expression != null)
    {
      int length = this.m_expression.Length;
      this.m_expression = new byte[length];
      for (int index = length - 1; index >= 0; --index)
        this.m_expression[index] = formulaRecord.m_expression[index];
    }
    if (this.m_arrParsedExpression != null)
    {
      int length = this.m_arrParsedExpression.Length;
      this.m_arrParsedExpression = new Ptg[length];
      for (int index = length - 1; index >= 0; --index)
        this.m_arrParsedExpression[index] = (Ptg) formulaRecord.m_arrParsedExpression[index].Clone();
    }
    return (object) formulaRecord;
  }

  public Ptg[] Formula
  {
    get => this.ParsedExpression;
    set => this.ParsedExpression = value;
  }

  public static void ConvertFormulaTokens(Ptg[] tokens, bool bFromExcel07To97)
  {
    if (tokens == null)
      return;
    for (int iGotoTokenIndex = 0; iGotoTokenIndex < tokens.Length; ++iGotoTokenIndex)
    {
      if (tokens[iGotoTokenIndex] is AttrPtg token1 && (token1.HasOptGoto || token1.HasOptimizedIf))
        FormulaRecord.ConvertFormulaGotoToken(tokens, iGotoTokenIndex, bFromExcel07To97);
      if (tokens[iGotoTokenIndex] is AreaPtg token2)
      {
        tokens[iGotoTokenIndex] = (Ptg) token2.ConvertFullRowColumnAreaPtgs(bFromExcel07To97);
        if (bFromExcel07To97 && tokens[iGotoTokenIndex].TokenCode == FormulaToken.tArea3d2)
          tokens[iGotoTokenIndex].TokenCode = FormulaToken.tArea3d1;
      }
    }
  }

  private static void ConvertFormulaGotoToken(
    Ptg[] formulaTokens,
    int iGotoTokenIndex,
    bool bFromExcel07To97)
  {
    ushort num1 = 0;
    AttrPtg formulaToken1 = (AttrPtg) formulaTokens[iGotoTokenIndex];
    ushort attrData = formulaToken1.AttrData;
    int index = iGotoTokenIndex + 1;
    ushort num2 = 0;
    ExcelVersion version1;
    ExcelVersion version2;
    if (bFromExcel07To97)
    {
      version1 = ExcelVersion.Excel2007;
      version2 = ExcelVersion.Excel97to2003;
    }
    else
    {
      version1 = ExcelVersion.Excel97to2003;
      version2 = ExcelVersion.Excel2007;
    }
    do
    {
      Ptg formulaToken2 = formulaTokens[index];
      num2 += (ushort) formulaToken2.GetSize(version2);
      num1 += (ushort) formulaToken2.GetSize(version1);
      ++index;
    }
    while (index < formulaTokens.Length && (int) num1 < (int) attrData);
    if (formulaToken1.HasOptimizedIf)
      formulaToken1.AttrData = num2;
    else
      formulaToken1.AttrData = (ushort) ((uint) num2 - 1U);
  }
}
