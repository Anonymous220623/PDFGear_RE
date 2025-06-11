// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.CFRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.CF)]
internal class CFRecord : BiffRecordRaw, ICloneable
{
  private const ushort DEF_MINIMUM_RECORD_SIZE = 12;
  private const int DEF_FONT_FIRST_RESERVED_SIZE = 64 /*0x40*/;
  private const int DEF_FONT_SECOND_RESERVED_SIZE = 3;
  private const int DEF_FONT_THIRD_RESERVED_SIZE = 16 /*0x10*/;
  private const uint DEF_FONT_POSTURE_MASK = 2;
  private const uint DEF_FONT_CANCELLATION_MASK = 128 /*0x80*/;
  private const uint DEF_FONT_STYLE_MODIFIED_MASK = 2;
  private const uint DEF_FONT_CANCELLATION_MODIFIED_MASK = 128 /*0x80*/;
  private const ushort DEF_BORDER_LEFT_MASK = 15;
  private const ushort DEF_BORDER_RIGHT_MASK = 240 /*0xF0*/;
  private const ushort DEF_BORDER_TOP_MASK = 3840 /*0x0F00*/;
  private const ushort DEF_BORDER_BOTTOM_MASK = 61440 /*0xF000*/;
  private const uint DEF_BORDER_LEFT_COLOR_MASK = 127 /*0x7F*/;
  private const uint DEF_BORDER_RIGHT_COLOR_MASK = 16256;
  private const uint DEF_BORDER_TOP_COLOR_MASK = 8323072 /*0x7F0000*/;
  private const uint DEF_BORDER_BOTTOM_COLOR_MASK = 1065353216 /*0x3F800000*/;
  private const int DEF_BORDER_LEFT_COLOR_START = 0;
  private const int DEF_BORDER_RIGHT_COLOR_START = 7;
  private const int DEF_BORDER_TOP_COLOR_START = 16 /*0x10*/;
  private const int DEF_BORDER_BOTTOM_COLOR_START = 23;
  private const ushort DEF_PATTERN_MASK = 64512;
  private const ushort DEF_PATTERN_COLOR_MASK = 127 /*0x7F*/;
  private const ushort DEF_PATTERN_BACKCOLOR_MASK = 16256;
  private const int DEF_PATTERN_START = 10;
  private const int DEF_PATTERN_BACKCOLOR_START = 7;
  private const int DEF_FONT_BLOCK_SIZE = 118;
  private const int DEF_BORDER_BLOCK_SIZE = 8;
  private const int DEF_PATTERN_BLOCK_SIZE = 4;
  private const int DEF_NUMBER_FORMAT_BLOCK_SIZE = 2;
  public const uint DefaultColorIndex = 4294967295 /*0xFFFFFFFF*/;
  [BiffRecordPos(0, 1)]
  private byte m_formatingType = 1;
  [BiffRecordPos(1, 1)]
  private byte m_compareOperator = 1;
  [BiffRecordPos(2, 2)]
  private ushort m_usFirstFormulaSize;
  [BiffRecordPos(4, 2)]
  private ushort m_usSecondFormulaSize;
  [BiffRecordPos(6, 4)]
  private uint m_uiOptions;
  [BiffRecordPos(10, 2)]
  private ushort m_usReserved;
  [BiffRecordPos(7, 2, TFieldType.Bit)]
  private bool m_bLeftBorder = true;
  [BiffRecordPos(7, 3, TFieldType.Bit)]
  private bool m_bRightBorder = true;
  [BiffRecordPos(7, 4, TFieldType.Bit)]
  private bool m_bTopBorder = true;
  [BiffRecordPos(7, 5, TFieldType.Bit)]
  private bool m_bBottomBorder = true;
  [BiffRecordPos(8, 0, TFieldType.Bit)]
  private bool m_bPatternStyle = true;
  [BiffRecordPos(8, 1, TFieldType.Bit)]
  private bool m_bPatternColor = true;
  [BiffRecordPos(8, 2, TFieldType.Bit)]
  private bool m_bPatternBackColor = true;
  [BiffRecordPos(8, 3, TFieldType.Bit)]
  private bool m_bNumberFormatModified = true;
  [BiffRecordPos(9, 1, TFieldType.Bit)]
  private bool m_bNumberFormatPresent;
  [BiffRecordPos(9, 2, TFieldType.Bit)]
  private bool m_bFontFormat;
  [BiffRecordPos(9, 4, TFieldType.Bit)]
  private bool m_bBorderFormat;
  [BiffRecordPos(9, 5, TFieldType.Bit)]
  private bool m_bPatternFormat;
  [BiffRecordPos(10, 0, TFieldType.Bit)]
  private bool m_numberFormatIsUserDefined;
  private uint m_uiFontHeight = uint.MaxValue;
  private uint m_uiFontOptions;
  private ushort m_usFontWeight = 400;
  private ushort m_usEscapmentType;
  private byte m_Underline;
  private uint m_uiFontColorIndex = uint.MaxValue;
  private uint m_uiModifiedFlags = 15;
  private uint m_uiEscapmentModified = 1;
  private uint m_uiUnderlineModified = 1;
  private ushort m_usBorderLineStyles;
  private uint m_uiBorderColors;
  private ushort m_usPatternStyle;
  private ushort m_usPatternColors;
  private ushort m_unUsed;
  private ushort m_numFormatIndex;
  private byte[] m_arrFirstFormula = new byte[0];
  private byte[] m_arrSecondFormula = new byte[0];
  private Ptg[] m_arrFirstFormulaParsed;
  private Ptg[] m_arrSecondFormulaParsed;

  public ExcelCFType FormatType
  {
    get => (ExcelCFType) this.m_formatingType;
    set => this.m_formatingType = (byte) value;
  }

  public ExcelComparisonOperator ComparisonOperator
  {
    get => (ExcelComparisonOperator) this.m_compareOperator;
    set => this.m_compareOperator = (byte) value;
  }

  public ushort FirstFormulaSize => this.m_usFirstFormulaSize;

  public ushort SecondFormulaSize => this.m_usSecondFormulaSize;

  public uint Options
  {
    get => this.m_uiOptions;
    internal set => this.m_uiOptions = value;
  }

  public ushort Reserved
  {
    get => this.m_usReserved;
    internal set => this.m_usReserved = value;
  }

  public bool IsLeftBorderModified
  {
    get => !this.m_bLeftBorder;
    set => this.m_bLeftBorder = !value;
  }

  public bool IsRightBorderModified
  {
    get => !this.m_bRightBorder;
    set => this.m_bRightBorder = !value;
  }

  public bool IsTopBorderModified
  {
    get => !this.m_bTopBorder;
    set => this.m_bTopBorder = !value;
  }

  public bool IsBottomBorderModified
  {
    get => !this.m_bBottomBorder;
    set => this.m_bBottomBorder = !value;
  }

  public bool IsPatternStyleModified
  {
    get => !this.m_bPatternStyle;
    set => this.m_bPatternStyle = !value;
  }

  public bool IsPatternColorModified
  {
    get => !this.m_bPatternColor;
    set => this.m_bPatternColor = !value;
  }

  public bool IsPatternBackColorModified
  {
    get => !this.m_bPatternBackColor;
    set => this.m_bPatternBackColor = !value;
  }

  public bool IsNumberFormatModified
  {
    get => !this.m_bNumberFormatModified;
    set => this.m_bNumberFormatModified = !value;
  }

  public bool IsFontFormatPresent
  {
    get => this.m_bFontFormat;
    set => this.m_bFontFormat = value;
  }

  public bool IsBorderFormatPresent
  {
    get => this.m_bBorderFormat;
    set => this.m_bBorderFormat = value;
  }

  public bool IsPatternFormatPresent
  {
    get => this.m_bPatternFormat;
    set => this.m_bPatternFormat = value;
  }

  public bool IsNumberFormatPresent
  {
    get => this.m_bNumberFormatPresent;
    set => this.m_bNumberFormatPresent = value;
  }

  public override int MinimumRecordSize => 12;

  public uint FontHeight
  {
    get => this.m_uiFontHeight;
    set
    {
      if ((int) this.m_uiFontHeight == (int) value)
        return;
      this.m_uiFontHeight = value;
    }
  }

  public bool FontPosture
  {
    get => ((int) this.m_uiFontOptions & 2) != 0;
    set
    {
      if (value == this.FontPosture)
        return;
      this.m_uiFontOptions &= 4294967293U;
      if (!value)
        return;
      this.m_uiFontOptions += 2U;
    }
  }

  public bool FontCancellation
  {
    get => ((int) this.m_uiFontOptions & 128 /*0x80*/) != 0;
    set
    {
      if (value == this.FontCancellation)
        return;
      this.m_uiFontOptions &= 4294967167U;
      if (!value)
        return;
      this.m_uiFontOptions += 128U /*0x80*/;
    }
  }

  public ushort FontWeight
  {
    get => this.m_usFontWeight;
    set
    {
      if ((int) this.m_usFontWeight == (int) value)
        return;
      this.m_usFontWeight = value;
    }
  }

  public OfficeFontVerticalAlignment FontEscapment
  {
    get => (OfficeFontVerticalAlignment) this.m_usEscapmentType;
    set
    {
      if ((int) this.m_usEscapmentType == (int) (ushort) value)
        return;
      this.m_usEscapmentType = (ushort) value;
    }
  }

  public OfficeUnderline FontUnderline
  {
    get => (OfficeUnderline) this.m_Underline;
    set
    {
      if ((int) this.m_Underline == (int) (byte) value)
        return;
      this.m_Underline = (byte) value;
    }
  }

  public uint FontColorIndex
  {
    get => this.m_uiFontColorIndex;
    set
    {
      if ((int) this.m_uiFontColorIndex == (int) value)
        return;
      this.m_uiFontColorIndex = value;
    }
  }

  public bool IsFontStyleModified
  {
    get => ((int) this.m_uiModifiedFlags & 2) == 0;
    set
    {
      if (value == this.IsFontStyleModified)
        return;
      this.m_uiModifiedFlags &= 4294967293U;
      if (value)
        return;
      this.m_uiModifiedFlags += 2U;
    }
  }

  public bool IsFontCancellationModified
  {
    get => ((int) this.m_uiModifiedFlags & 128 /*0x80*/) == 0;
    set
    {
      if (value == this.IsFontCancellationModified)
        return;
      this.m_uiModifiedFlags &= 4294967167U;
      if (value)
        return;
      this.m_uiModifiedFlags += 128U /*0x80*/;
    }
  }

  public bool IsFontEscapmentModified
  {
    get => this.m_uiEscapmentModified == 0U;
    set => this.m_uiEscapmentModified = value ? 0U : 1U;
  }

  public bool IsFontUnderlineModified
  {
    get => this.m_uiUnderlineModified == 0U;
    set => this.m_uiUnderlineModified = value ? 0U : 1U;
  }

  public bool IsNumberFormatUserDefined
  {
    get => this.m_numberFormatIsUserDefined;
    set => this.m_numberFormatIsUserDefined = value;
  }

  public ushort NumberFormatIndex
  {
    get => this.m_numFormatIndex;
    set => this.m_numFormatIndex = value;
  }

  public OfficeLineStyle LeftBorderStyle
  {
    get
    {
      return (OfficeLineStyle) BiffRecordRaw.GetUInt16BitsByMask(this.m_usBorderLineStyles, (ushort) 15);
    }
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usBorderLineStyles, (ushort) 15, (ushort) value);
    }
  }

  public OfficeLineStyle RightBorderStyle
  {
    get
    {
      return (OfficeLineStyle) ((int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usBorderLineStyles, (ushort) 240 /*0xF0*/) >> 4);
    }
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usBorderLineStyles, (ushort) 240 /*0xF0*/, (ushort) ((uint) value << 4));
    }
  }

  public OfficeLineStyle TopBorderStyle
  {
    get
    {
      return (OfficeLineStyle) ((int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usBorderLineStyles, (ushort) 3840 /*0x0F00*/) >> 8);
    }
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usBorderLineStyles, (ushort) 3840 /*0x0F00*/, (ushort) ((uint) value << 8));
    }
  }

  public OfficeLineStyle BottomBorderStyle
  {
    get
    {
      return (OfficeLineStyle) ((int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usBorderLineStyles, (ushort) 61440 /*0xF000*/) >> 12);
    }
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usBorderLineStyles, (ushort) 61440 /*0xF000*/, (ushort) ((uint) value << 12));
    }
  }

  public uint LeftBorderColorIndex
  {
    get => BiffRecordRaw.GetUInt32BitsByMask(this.m_uiBorderColors, (uint) sbyte.MaxValue);
    set
    {
      BiffRecordRaw.SetUInt32BitsByMask(ref this.m_uiBorderColors, (uint) sbyte.MaxValue, value);
    }
  }

  public uint RightBorderColorIndex
  {
    get => BiffRecordRaw.GetUInt32BitsByMask(this.m_uiBorderColors, 16256U) >> 7;
    set => BiffRecordRaw.SetUInt32BitsByMask(ref this.m_uiBorderColors, 16256U, value << 7);
  }

  public uint TopBorderColorIndex
  {
    get
    {
      return BiffRecordRaw.GetUInt32BitsByMask(this.m_uiBorderColors, 8323072U /*0x7F0000*/) >> 16 /*0x10*/;
    }
    set
    {
      BiffRecordRaw.SetUInt32BitsByMask(ref this.m_uiBorderColors, 8323072U /*0x7F0000*/, value << 16 /*0x10*/);
    }
  }

  public uint BottomBorderColorIndex
  {
    get
    {
      return BiffRecordRaw.GetUInt32BitsByMask(this.m_uiBorderColors, 1065353216U /*0x3F800000*/) >> 23;
    }
    set
    {
      BiffRecordRaw.SetUInt32BitsByMask(ref this.m_uiBorderColors, 1065353216U /*0x3F800000*/, value << 23);
    }
  }

  public OfficePattern PatternStyle
  {
    get
    {
      return (OfficePattern) ((int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usPatternStyle, (ushort) 64512) >> 10);
    }
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usPatternStyle, (ushort) 64512, (ushort) ((uint) value << 10));
    }
  }

  public ushort PatternColorIndex
  {
    get => BiffRecordRaw.GetUInt16BitsByMask(this.m_usPatternColors, (ushort) sbyte.MaxValue);
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usPatternColors, (ushort) sbyte.MaxValue, value);
    }
  }

  public ushort PatternBackColor
  {
    get
    {
      return (ushort) ((uint) BiffRecordRaw.GetUInt16BitsByMask(this.m_usPatternColors, (ushort) 16256) >> 7);
    }
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usPatternColors, (ushort) 16256, (ushort) ((uint) value << 7));
    }
  }

  public Ptg[] FirstFormulaPtgs
  {
    get => this.m_arrFirstFormulaParsed;
    set
    {
      this.m_arrFirstFormula = FormulaUtil.PtgArrayToByteArray(value, OfficeVersion.Excel2007);
      this.m_arrFirstFormulaParsed = value;
      this.m_usFirstFormulaSize = (ushort) this.m_arrFirstFormula.Length;
    }
  }

  public Ptg[] SecondFormulaPtgs
  {
    get => this.m_arrSecondFormulaParsed;
    set
    {
      this.m_arrSecondFormula = FormulaUtil.PtgArrayToByteArray(value, OfficeVersion.Excel2007);
      this.m_arrSecondFormulaParsed = value;
      this.m_usSecondFormulaSize = (ushort) this.m_arrSecondFormula.Length;
    }
  }

  public byte[] FirstFormulaBytes => this.m_arrFirstFormula;

  public byte[] SecondFormulaBytes => this.m_arrSecondFormula;

  public CFRecord() => this.m_uiOptions |= 3720191U;

  public CFRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public CFRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_formatingType = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_compareOperator = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_usFirstFormulaSize = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usSecondFormulaSize = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_uiOptions = provider.ReadUInt32(iOffset);
    ++iOffset;
    this.m_bLeftBorder = provider.ReadBit(iOffset, 2);
    this.m_bRightBorder = provider.ReadBit(iOffset, 3);
    this.m_bTopBorder = provider.ReadBit(iOffset, 4);
    this.m_bBottomBorder = provider.ReadBit(iOffset, 5);
    ++iOffset;
    this.m_bPatternStyle = provider.ReadBit(iOffset, 0);
    this.m_bPatternColor = provider.ReadBit(iOffset, 1);
    this.m_bPatternBackColor = provider.ReadBit(iOffset, 2);
    this.m_bNumberFormatModified = provider.ReadBit(iOffset, 3);
    ++iOffset;
    this.m_bNumberFormatPresent = provider.ReadBit(iOffset, 1);
    this.m_bFontFormat = provider.ReadBit(iOffset, 2);
    this.m_bBorderFormat = provider.ReadBit(iOffset, 4);
    this.m_bPatternFormat = provider.ReadBit(iOffset, 5);
    ++iOffset;
    this.m_numberFormatIsUserDefined = provider.ReadBit(iOffset, 0);
    ++iOffset;
    this.m_usReserved = provider.ReadUInt16(iOffset);
    ++iOffset;
    if (!this.m_numberFormatIsUserDefined)
      this.ParseNumberFormatBlock(provider, ref iOffset);
    this.ParseFontBlock(provider, ref iOffset);
    this.ParseBorderBlock(provider, ref iOffset);
    this.ParsePatternBlock(provider, ref iOffset);
    this.m_arrFirstFormula = new byte[(int) this.m_usFirstFormulaSize];
    provider.ReadArray(iOffset, this.m_arrFirstFormula);
    iOffset += (int) this.m_usFirstFormulaSize;
    this.m_arrSecondFormula = new byte[(int) this.m_usSecondFormulaSize];
    provider.ReadArray(iOffset, this.m_arrSecondFormula);
    this.m_arrFirstFormulaParsed = FormulaUtil.ParseExpression((DataProvider) new ByteArrayDataProvider(this.m_arrFirstFormula), (int) this.m_usFirstFormulaSize, version);
    this.m_arrSecondFormulaParsed = FormulaUtil.ParseExpression((DataProvider) new ByteArrayDataProvider(this.m_arrSecondFormula), (int) this.m_usSecondFormulaSize, version);
    if (version == OfficeVersion.Excel2007)
      return;
    if (this.m_usFirstFormulaSize > (ushort) 0)
    {
      this.m_arrFirstFormula = FormulaUtil.PtgArrayToByteArray(this.m_arrFirstFormulaParsed, OfficeVersion.Excel2007);
      this.m_usFirstFormulaSize = (ushort) this.m_arrFirstFormula.Length;
    }
    if (this.m_usSecondFormulaSize <= (ushort) 0)
      return;
    this.m_arrSecondFormula = FormulaUtil.PtgArrayToByteArray(this.m_arrSecondFormulaParsed, OfficeVersion.Excel2007);
    this.m_usSecondFormulaSize = (ushort) this.m_arrSecondFormula.Length;
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    if (this.m_arrFirstFormulaParsed != null && this.m_arrFirstFormulaParsed.Length > 0)
    {
      this.m_arrFirstFormula = FormulaUtil.PtgArrayToByteArray(this.m_arrFirstFormulaParsed, version);
      this.m_usFirstFormulaSize = (ushort) this.m_arrFirstFormula.Length;
    }
    else
    {
      this.m_arrFirstFormula = (byte[]) null;
      this.m_usFirstFormulaSize = (ushort) 0;
    }
    if (this.m_arrSecondFormulaParsed != null && this.m_arrSecondFormulaParsed.Length > 0)
    {
      this.m_arrSecondFormula = FormulaUtil.PtgArrayToByteArray(this.m_arrSecondFormulaParsed, version);
      this.m_usSecondFormulaSize = (ushort) this.m_arrSecondFormula.Length;
    }
    else
    {
      this.m_arrSecondFormula = (byte[]) null;
      this.m_usSecondFormulaSize = (ushort) 0;
    }
    byte num = this.m_formatingType == (byte) 1 ? (byte) 1 : (byte) 2;
    provider.WriteByte(iOffset, num);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_compareOperator);
    ++iOffset;
    provider.WriteUInt16(iOffset, this.m_usFirstFormulaSize);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usSecondFormulaSize);
    iOffset += 2;
    provider.WriteUInt32(iOffset, this.m_uiOptions);
    ++iOffset;
    provider.WriteBit(iOffset, this.m_bLeftBorder, 2);
    provider.WriteBit(iOffset, this.m_bRightBorder, 3);
    provider.WriteBit(iOffset, this.m_bTopBorder, 4);
    provider.WriteBit(iOffset, this.m_bBottomBorder, 5);
    ++iOffset;
    provider.WriteBit(iOffset, this.m_bPatternStyle, 0);
    provider.WriteBit(iOffset, this.m_bPatternColor, 1);
    provider.WriteBit(iOffset, this.m_bPatternBackColor, 2);
    provider.WriteBit(iOffset, this.m_bNumberFormatModified, 3);
    ++iOffset;
    provider.WriteBit(iOffset, this.m_bNumberFormatPresent, 1);
    provider.WriteBit(iOffset, this.m_bFontFormat, 2);
    provider.WriteBit(iOffset, this.m_bBorderFormat, 4);
    provider.WriteBit(iOffset, this.m_bPatternFormat, 5);
    ++iOffset;
    provider.WriteBit(iOffset, this.m_numberFormatIsUserDefined, 0);
    ++iOffset;
    provider.WriteUInt16(iOffset, this.m_usReserved);
    ++iOffset;
    if (!this.m_numberFormatIsUserDefined)
      this.SerializeNumberFormatBlock(provider, ref iOffset);
    this.SerializeFontBlock(provider, ref iOffset);
    this.SerializeBorderBlock(provider, ref iOffset);
    this.SerializePatternBlock(provider, ref iOffset);
    provider.WriteBytes(iOffset, this.m_arrFirstFormula, 0, (int) this.m_usFirstFormulaSize);
    iOffset += (int) this.m_usFirstFormulaSize;
    provider.WriteBytes(iOffset, this.m_arrSecondFormula, 0, (int) this.m_usSecondFormulaSize);
    iOffset += (int) this.m_usSecondFormulaSize;
  }

  public int ParseFontBlock(DataProvider provider, ref int iOffset)
  {
    if (!this.IsFontFormatPresent)
      return iOffset;
    iOffset += 64 /*0x40*/;
    this.m_uiFontHeight = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiFontOptions = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_usFontWeight = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usEscapmentType = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_Underline = provider.ReadByte(iOffset);
    ++iOffset;
    iOffset += 3;
    this.m_uiFontColorIndex = provider.ReadUInt32(iOffset);
    iOffset += 4;
    iOffset += 4;
    this.m_uiModifiedFlags = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiEscapmentModified = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiUnderlineModified = provider.ReadUInt32(iOffset);
    iOffset += 4;
    iOffset += 16 /*0x10*/;
    iOffset += 2;
    return iOffset;
  }

  public int ParseBorderBlock(DataProvider provider, ref int iOffset)
  {
    if (!this.IsBorderFormatPresent)
      return iOffset;
    this.m_usBorderLineStyles = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_uiBorderColors = provider.ReadUInt32(iOffset);
    iOffset += 4;
    iOffset += 2;
    return iOffset;
  }

  public int ParsePatternBlock(DataProvider provider, ref int iOffset)
  {
    if (!this.IsPatternFormatPresent)
      return iOffset;
    this.m_usPatternStyle = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usPatternColors = provider.ReadUInt16(iOffset);
    iOffset += 2;
    return iOffset;
  }

  public int ParseNumberFormatBlock(DataProvider provider, ref int iOffset)
  {
    if (!this.IsNumberFormatPresent)
      return iOffset;
    this.m_unUsed = (ushort) provider.ReadByte(iOffset);
    ++iOffset;
    this.m_numFormatIndex = (ushort) provider.ReadByte(iOffset);
    ++iOffset;
    return iOffset;
  }

  public int SerializeFontBlock(DataProvider provider, ref int iOffset)
  {
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    if (!this.IsFontFormatPresent)
      return iOffset;
    int num1 = 0;
    while (num1 < 64 /*0x40*/)
    {
      provider.WriteByte(iOffset, (byte) 0);
      ++num1;
      ++iOffset;
    }
    provider.WriteUInt32(iOffset, this.m_uiFontHeight);
    iOffset += 4;
    provider.WriteUInt32(iOffset, this.m_uiFontOptions);
    iOffset += 4;
    provider.WriteUInt16(iOffset, this.m_usFontWeight);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usEscapmentType);
    iOffset += 2;
    provider.WriteByte(iOffset, this.m_Underline);
    ++iOffset;
    int num2 = 0;
    while (num2 < 3)
    {
      provider.WriteByte(iOffset, (byte) 0);
      ++num2;
      ++iOffset;
    }
    provider.WriteUInt32(iOffset, this.m_uiFontColorIndex);
    iOffset += 4;
    provider.WriteUInt32(iOffset, 0U);
    iOffset += 4;
    provider.WriteUInt32(iOffset, this.m_uiModifiedFlags);
    iOffset += 4;
    provider.WriteUInt32(iOffset, this.m_uiEscapmentModified);
    iOffset += 4;
    provider.WriteUInt32(iOffset, this.m_uiUnderlineModified);
    iOffset += 4;
    int num3 = 0;
    while (num3 < 16 /*0x10*/)
    {
      provider.WriteByte(iOffset, (byte) 0);
      ++num3;
      ++iOffset;
    }
    provider.WriteUInt16(iOffset, (ushort) 1);
    iOffset += 2;
    return iOffset;
  }

  public int SerializeBorderBlock(DataProvider provider, ref int iOffset)
  {
    if (!this.IsBorderFormatPresent)
      return iOffset;
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    provider.WriteUInt16(iOffset, this.m_usBorderLineStyles);
    iOffset += 2;
    provider.WriteUInt32(iOffset, this.m_uiBorderColors);
    iOffset += 4;
    provider.WriteUInt16(iOffset, (ushort) 0);
    iOffset += 2;
    return iOffset;
  }

  public int SerializePatternBlock(DataProvider provider, ref int iOffset)
  {
    if (!this.IsPatternFormatPresent)
      return iOffset;
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    provider.WriteUInt16(iOffset, this.m_usPatternStyle);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usPatternColors);
    iOffset += 2;
    return iOffset;
  }

  public int SerializeNumberFormatBlock(DataProvider provider, ref int iOffset)
  {
    if (!this.IsNumberFormatPresent)
      return iOffset;
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    provider.WriteUInt16(iOffset, this.m_unUsed);
    ++iOffset;
    provider.WriteUInt16(iOffset, this.m_numFormatIndex);
    ++iOffset;
    return iOffset;
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    int num = 12;
    if (this.IsFontFormatPresent)
      num += 118;
    if (this.IsBorderFormatPresent)
      num += 8;
    if (this.IsPatternFormatPresent)
      num += 4;
    if (this.IsNumberFormatPresent)
      num += 2;
    return num + DVRecord.GetFormulaSize(this.m_arrFirstFormulaParsed, version, true) + DVRecord.GetFormulaSize(this.m_arrSecondFormulaParsed, version, true);
  }

  public override object Clone()
  {
    CFRecord cfRecord = (CFRecord) base.Clone();
    cfRecord.m_arrFirstFormula = CloneUtils.CloneByteArray(this.m_arrFirstFormula);
    cfRecord.m_arrSecondFormula = CloneUtils.CloneByteArray(this.m_arrSecondFormula);
    cfRecord.m_arrFirstFormulaParsed = CloneUtils.ClonePtgArray(this.m_arrFirstFormulaParsed);
    cfRecord.m_arrSecondFormulaParsed = CloneUtils.ClonePtgArray(this.m_arrSecondFormulaParsed);
    return (object) cfRecord;
  }

  public override int GetHashCode()
  {
    return this.m_formatingType.GetHashCode() ^ this.m_compareOperator.GetHashCode() ^ this.m_usFirstFormulaSize.GetHashCode() ^ this.m_usSecondFormulaSize.GetHashCode() ^ this.m_uiOptions.GetHashCode() ^ this.m_usReserved.GetHashCode() ^ this.m_bLeftBorder.GetHashCode() ^ this.m_bRightBorder.GetHashCode() ^ this.m_bTopBorder.GetHashCode() ^ this.m_bBottomBorder.GetHashCode() ^ this.m_bPatternStyle.GetHashCode() ^ this.m_bPatternColor.GetHashCode() ^ this.m_bPatternBackColor.GetHashCode() ^ this.m_bNumberFormatModified.GetHashCode() ^ this.m_bNumberFormatPresent.GetHashCode() ^ this.m_bFontFormat.GetHashCode() ^ this.m_bBorderFormat.GetHashCode() ^ this.m_bPatternFormat.GetHashCode() ^ this.m_uiFontHeight.GetHashCode() ^ this.m_uiFontOptions.GetHashCode() ^ this.m_usFontWeight.GetHashCode() ^ this.m_usEscapmentType.GetHashCode() ^ this.m_Underline.GetHashCode() ^ this.m_uiFontColorIndex.GetHashCode() ^ this.m_uiModifiedFlags.GetHashCode() ^ this.m_uiEscapmentModified.GetHashCode() ^ this.m_uiUnderlineModified.GetHashCode() ^ this.m_usBorderLineStyles.GetHashCode() ^ this.m_uiBorderColors.GetHashCode() ^ this.m_usPatternStyle.GetHashCode() ^ this.m_usPatternColors.GetHashCode();
  }

  public override bool Equals(object obj)
  {
    if (!(obj is CFRecord cfRecord))
      return false;
    bool flag = (int) this.m_formatingType == (int) cfRecord.m_formatingType && (int) this.m_compareOperator == (int) cfRecord.m_compareOperator && (int) this.m_usFirstFormulaSize == (int) cfRecord.m_usFirstFormulaSize && (int) this.m_usSecondFormulaSize == (int) cfRecord.m_usSecondFormulaSize && (int) this.m_uiOptions == (int) cfRecord.m_uiOptions && this.m_bLeftBorder == cfRecord.m_bLeftBorder && this.m_bRightBorder == cfRecord.m_bRightBorder && this.m_bTopBorder == cfRecord.m_bTopBorder && this.m_bBottomBorder == cfRecord.m_bBottomBorder && this.m_bPatternStyle == cfRecord.m_bPatternStyle && this.m_bPatternColor == cfRecord.m_bPatternColor && this.m_bPatternBackColor == cfRecord.m_bPatternBackColor && this.m_bNumberFormatModified == cfRecord.m_bNumberFormatModified && this.m_bNumberFormatPresent == cfRecord.m_bNumberFormatPresent && this.m_bFontFormat == cfRecord.m_bFontFormat && this.m_bBorderFormat == cfRecord.m_bBorderFormat && this.m_bPatternFormat == cfRecord.m_bPatternFormat && (int) this.m_uiFontHeight == (int) cfRecord.m_uiFontHeight && (int) this.m_uiFontOptions == (int) cfRecord.m_uiFontOptions && (int) this.m_usFontWeight == (int) cfRecord.m_usFontWeight && (int) this.m_usEscapmentType == (int) cfRecord.m_usEscapmentType && (int) this.m_Underline == (int) cfRecord.m_Underline && (int) this.m_uiFontColorIndex == (int) cfRecord.m_uiFontColorIndex && (int) this.m_uiModifiedFlags == (int) cfRecord.m_uiModifiedFlags && (int) this.m_uiEscapmentModified == (int) cfRecord.m_uiEscapmentModified && (int) this.m_uiUnderlineModified == (int) cfRecord.m_uiUnderlineModified && (int) this.m_usBorderLineStyles == (int) cfRecord.m_usBorderLineStyles && (int) this.m_uiBorderColors == (int) cfRecord.m_uiBorderColors && (int) this.m_usPatternStyle == (int) cfRecord.m_usPatternStyle && (int) this.m_usPatternColors == (int) cfRecord.m_usPatternColors;
    if (flag)
      flag = BiffRecordRaw.CompareArrays(this.m_arrFirstFormula, cfRecord.m_arrFirstFormula) && BiffRecordRaw.CompareArrays(this.m_arrSecondFormula, cfRecord.m_arrSecondFormula);
    return flag;
  }
}
