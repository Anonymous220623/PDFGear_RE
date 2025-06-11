// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ExtendedFormatRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ExtendedFormat)]
internal class ExtendedFormatRecord : BiffRecordRaw
{
  private const ushort DEF_INDENT_MASK = 15;
  private const ushort DEF_READ_ORDER_MASK = 192 /*0xC0*/;
  private const ushort DEF_READ_ORDER_START_BIT = 6;
  private const ushort DEF_PARENT_INDEX_MASK = 65520;
  private const ushort DEF_ROTATION_MASK = 65280;
  private const uint DEF_TOP_BORDER_PALLETE_MASK = 127 /*0x7F*/;
  private const uint DEF_BOTTOM_BORDER_PALLETE_MASK = 16256;
  private const uint DEF_DIAGONAL_MASK = 2080768;
  private const uint DEF_DIAGONAL_LINE_MASK = 31457280 /*0x01E00000*/;
  private const uint DEF_FILL_PATTERN_MASK = 4227858432 /*0xFC000000*/;
  private const ushort DEF_BORDER_LEFT_MASK = 15;
  private const ushort DEF_BORDER_RIGTH_MASK = 240 /*0xF0*/;
  private const ushort DEF_BORDER_TOP_MASK = 3840 /*0x0F00*/;
  private const ushort DEF_BORDER_BOTTOM_MASK = 61440 /*0xF000*/;
  private const ushort DEF_HOR_ALIGNMENT_MASK = 7;
  private const ushort DEF_VER_ALIGNMENT_MASK = 112 /*0x70*/;
  private const ushort DEF_BACKGROUND_MASK = 127 /*0x7F*/;
  private const ushort DEF_FOREGROUND_MASK = 16256;
  private const ushort DEF_LEFT_BORDER_PALLETE_MASK = 127 /*0x7F*/;
  private const ushort DEF_RIGHT_BORDER_PALLETE_MASK = 16256;
  private const int DEF_RIGHT_BORDER_START_MASK = 7;
  private const int DEF_RECORD_SIZE = 20;
  private const int DEF_FILL_FOREGROUND_MASK = 16256;
  public const int DEF_DEFAULT_COLOR_INDEX = 65;
  public const int DEF_DEFAULT_PATTERN_COLOR_INDEX = 64 /*0x40*/;
  private const int DEF_XF_MAX_INDEX = 4095 /*0x0FFF*/;
  private const int HALIGN_JUSTIFY = 5;
  private const int VALIGN_JUSTIFY = 3;
  [BiffRecordPos(0, 2)]
  private ushort m_usFontIndex;
  [BiffRecordPos(2, 2)]
  private ushort m_usFormatIndex;
  [BiffRecordPos(4, 2)]
  private ushort m_usCellOptions;
  [BiffRecordPos(4, 0, TFieldType.Bit)]
  private bool m_bLocked = true;
  [BiffRecordPos(4, 1, TFieldType.Bit)]
  private bool m_bHidden;
  [BiffRecordPos(4, 2, TFieldType.Bit)]
  private bool m_xfType;
  [BiffRecordPos(4, 3, TFieldType.Bit)]
  private bool m_b123Prefix;
  [BiffRecordPos(6, 2)]
  private ushort m_usAlignmentOptions = 32 /*0x20*/;
  [BiffRecordPos(6, 3, TFieldType.Bit)]
  private bool m_bWrapText;
  [BiffRecordPos(6, 7, TFieldType.Bit)]
  private bool m_bJustifyLast;
  [BiffRecordPos(8, 2)]
  private ushort m_usIndentOptions;
  [BiffRecordPos(8, 4, TFieldType.Bit)]
  private bool m_bShrinkToFit;
  [BiffRecordPos(8, 5, TFieldType.Bit)]
  private bool m_bMergeCells;
  [BiffRecordPos(9, 2, TFieldType.Bit)]
  private bool m_bIndentNotParentFormat;
  [BiffRecordPos(9, 3, TFieldType.Bit)]
  private bool m_bIndentNotParentFont;
  [BiffRecordPos(9, 4, TFieldType.Bit)]
  private bool m_bIndentNotParentAlignment;
  [BiffRecordPos(9, 5, TFieldType.Bit)]
  private bool m_bIndentNotParentBorder;
  [BiffRecordPos(9, 6, TFieldType.Bit)]
  private bool m_bIndentNotParentPattern;
  [BiffRecordPos(9, 7, TFieldType.Bit)]
  private bool m_bIndentNotParentCellOptions;
  private byte m_btIndent;
  [BiffRecordPos(10, 2)]
  private ushort m_usBorderOptions;
  [BiffRecordPos(12, 2)]
  private ushort m_usPaletteOptions;
  [BiffRecordPos(13, 6, TFieldType.Bit)]
  private bool m_bDiagnalFromTopLeft;
  [BiffRecordPos(13, 7, TFieldType.Bit)]
  private bool m_bDiagnalFromBottomLeft;
  [BiffRecordPos(14, 4)]
  private uint m_uiAddPaletteOptions;
  [BiffRecordPos(18, 2)]
  private ushort m_usFillPaletteOptions = 8257;
  private bool m_bHashValid;
  private int m_iHash;
  private ushort m_usParentXFIndex;
  private ushort m_usFillPattern;
  private WorkbookImpl m_book;
  private ushort m_fillIndex;
  private ushort m_borderIndex;

  public int CellOptions
  {
    get
    {
      return ((((0 | (this.m_bLocked ? 1 : 0)) << 1 | (this.m_bHidden ? 1 : 0)) << 1 | (this.m_bMergeCells ? 1 : 0)) << 1 | (this.m_bShrinkToFit ? 1 : 0)) << 1;
    }
  }

  public int BorderOptions
  {
    get
    {
      return ((0 | (this.m_bDiagnalFromTopLeft ? 1 : 0)) << 1 | (this.m_bDiagnalFromBottomLeft ? 1 : 0)) << 15 | (int) this.m_usBorderOptions;
    }
  }

  public int AlignmentOptions => (int) this.m_usAlignmentOptions;

  public ushort FontIndex
  {
    get => this.m_usFontIndex;
    set
    {
      if (value == (ushort) 4)
        throw new ArgumentException("FontIndex must be less than or higher than 4 for ExtendedFormatRecords.");
      this.m_bHashValid = false;
      this.m_usFontIndex = value;
    }
  }

  internal ushort FillIndex
  {
    get => this.m_fillIndex;
    set => this.m_fillIndex = value;
  }

  internal ushort BorderIndex
  {
    get => this.m_borderIndex;
    set => this.m_borderIndex = value;
  }

  public ushort FormatIndex
  {
    get => this.m_usFormatIndex;
    set
    {
      this.m_bHashValid = false;
      this.m_usFormatIndex = value;
    }
  }

  public bool IsLocked
  {
    get => this.m_bLocked;
    set
    {
      this.m_bHashValid = false;
      this.m_bLocked = value;
    }
  }

  public bool IsHidden
  {
    get => this.m_bHidden;
    set
    {
      this.m_bHashValid = false;
      this.m_bHidden = value;
    }
  }

  public ExtendedFormatRecord.TXFType XFType
  {
    get
    {
      return !this.m_xfType ? ExtendedFormatRecord.TXFType.XF_STYLE : ExtendedFormatRecord.TXFType.XF_CELL;
    }
    set
    {
      this.m_bHashValid = false;
      this.m_xfType = value == ExtendedFormatRecord.TXFType.XF_CELL;
    }
  }

  public bool _123Prefix
  {
    get => this.m_b123Prefix;
    set
    {
      this.m_bHashValid = false;
      this.m_b123Prefix = value;
    }
  }

  public ushort ParentIndex
  {
    get => this.m_usParentXFIndex;
    set => this.m_usParentXFIndex = value;
  }

  public bool WrapText
  {
    get => this.m_bWrapText;
    set
    {
      if (this.m_bWrapText == value)
        return;
      this.m_bHashValid = false;
      this.m_bWrapText = value;
      this.SetBitInVar(ref this.m_usAlignmentOptions, this.m_bWrapText, 3);
    }
  }

  public bool JustifyLast
  {
    get => this.m_bJustifyLast;
    set
    {
      if (this.m_bJustifyLast == value)
        return;
      this.m_bHashValid = false;
      this.m_bJustifyLast = value;
      this.SetBitInVar(ref this.m_usAlignmentOptions, this.m_bJustifyLast, 7);
    }
  }

  public byte Indent
  {
    get => this.m_btIndent;
    set
    {
      this.m_bHashValid = false;
      this.m_btIndent = value;
    }
  }

  public bool ShrinkToFit
  {
    get => this.m_bShrinkToFit;
    set
    {
      this.m_bHashValid = false;
      this.m_bShrinkToFit = value;
    }
  }

  public bool MergeCells
  {
    get => this.m_bMergeCells;
    set
    {
      this.m_bHashValid = false;
      this.m_bMergeCells = value;
    }
  }

  public ushort ReadingOrder
  {
    get
    {
      return (ushort) ((uint) BiffRecordRaw.GetUInt16BitsByMask(this.m_usIndentOptions, (ushort) 192 /*0xC0*/) >> 6);
    }
    set
    {
      if (value > (ushort) 3)
        throw new ArgumentOutOfRangeException("Reading Order");
      this.m_bHashValid = false;
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usIndentOptions, (ushort) 192 /*0xC0*/, (ushort) ((uint) value << 6));
    }
  }

  public ushort Rotation
  {
    get
    {
      return (ushort) ((uint) BiffRecordRaw.GetUInt16BitsByMask(this.m_usAlignmentOptions, (ushort) 65280) >> 8);
    }
    set
    {
      if (value > (ushort) byte.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (Rotation));
      this.m_bHashValid = false;
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usAlignmentOptions, (ushort) 65280, (ushort) ((uint) value << 8));
    }
  }

  public bool IsNotParentFormat
  {
    get => this.m_bIndentNotParentFormat;
    set
    {
      this.m_bHashValid = false;
      this.m_bIndentNotParentFormat = value;
    }
  }

  public bool IsNotParentFont
  {
    get => this.m_bIndentNotParentFont;
    set
    {
      this.m_bHashValid = false;
      this.m_bIndentNotParentFont = value;
    }
  }

  public bool IsNotParentAlignment
  {
    get => this.m_bIndentNotParentAlignment;
    set
    {
      this.m_bHashValid = false;
      this.m_bIndentNotParentAlignment = value;
    }
  }

  public bool IsNotParentBorder
  {
    get => this.m_bIndentNotParentBorder;
    set
    {
      this.m_bHashValid = false;
      this.m_bIndentNotParentBorder = value;
    }
  }

  public bool IsNotParentPattern
  {
    get => this.m_bIndentNotParentPattern;
    set
    {
      this.m_bHashValid = false;
      this.m_bIndentNotParentPattern = value;
    }
  }

  public bool IsNotParentCellOptions
  {
    get => this.m_bIndentNotParentCellOptions;
    set
    {
      this.m_bHashValid = false;
      this.m_bIndentNotParentCellOptions = value;
    }
  }

  public ushort TopBorderPaletteIndex
  {
    get => (ushort) (this.m_uiAddPaletteOptions & (uint) sbyte.MaxValue);
    set
    {
      if (value > (ushort) sbyte.MaxValue)
        throw new ArgumentOutOfRangeException();
      this.m_bHashValid = false;
      this.m_uiAddPaletteOptions &= 4294967168U;
      this.m_uiAddPaletteOptions += (uint) value;
    }
  }

  public ushort BottomBorderPaletteIndex
  {
    get => (ushort) ((this.m_uiAddPaletteOptions & 16256U) >> 7);
    set
    {
      if (value > (ushort) sbyte.MaxValue)
        throw new ArgumentOutOfRangeException();
      this.m_bHashValid = false;
      this.m_uiAddPaletteOptions &= 4294951039U;
      this.m_uiAddPaletteOptions += (uint) value << 7;
    }
  }

  public ushort LeftBorderPaletteIndex
  {
    get => (ushort) ((uint) this.m_usPaletteOptions & (uint) sbyte.MaxValue);
    set
    {
      if (value > (ushort) sbyte.MaxValue)
        throw new ArgumentOutOfRangeException();
      this.m_bHashValid = false;
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usPaletteOptions, (ushort) sbyte.MaxValue, value);
    }
  }

  public ushort RightBorderPaletteIndex
  {
    get
    {
      return (ushort) ((uint) BiffRecordRaw.GetUInt16BitsByMask(this.m_usPaletteOptions, (ushort) 16256) >> 7);
    }
    set
    {
      if (value > (ushort) sbyte.MaxValue)
        throw new ArgumentOutOfRangeException();
      this.m_bHashValid = false;
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usPaletteOptions, (ushort) 16256, (ushort) ((uint) value << 7));
    }
  }

  public ushort DiagonalLineColor
  {
    get => (ushort) ((this.m_uiAddPaletteOptions & 2080768U) >> 14);
    set
    {
      if (value > (ushort) sbyte.MaxValue)
        throw new ArgumentOutOfRangeException();
      this.m_bHashValid = false;
      this.m_uiAddPaletteOptions &= 4292886527U;
      this.m_uiAddPaletteOptions |= (uint) value << 14;
    }
  }

  public ushort DiagonalLineStyle
  {
    get => (ushort) ((this.m_uiAddPaletteOptions & 31457280U /*0x01E00000*/) >> 21);
    set
    {
      if (value > (ushort) 15)
        throw new ArgumentOutOfRangeException();
      this.m_bHashValid = false;
      this.m_uiAddPaletteOptions &= 4263510015U;
      this.m_uiAddPaletteOptions |= (uint) value << 21;
    }
  }

  public bool DiagonalFromTopLeft
  {
    get => this.m_bDiagnalFromTopLeft;
    set
    {
      if (this.m_bDiagnalFromTopLeft == value)
        return;
      this.m_bHashValid = false;
      this.m_bDiagnalFromTopLeft = value;
      this.SetBitInVar(ref this.m_usPaletteOptions, this.m_bDiagnalFromTopLeft, 14);
    }
  }

  public bool DiagonalFromBottomLeft
  {
    get => this.m_bDiagnalFromBottomLeft;
    set
    {
      if (this.m_bDiagnalFromBottomLeft == value)
        return;
      this.m_bHashValid = false;
      this.m_bDiagnalFromBottomLeft = value;
      this.SetBitInVar(ref this.m_usPaletteOptions, this.m_bDiagnalFromBottomLeft, 15);
    }
  }

  public ushort AdtlFillPattern
  {
    get => this.m_usFillPattern;
    set => this.m_usFillPattern = value;
  }

  public OfficeLineStyle BorderLeft
  {
    get => (OfficeLineStyle) BiffRecordRaw.GetUInt16BitsByMask(this.m_usBorderOptions, (ushort) 15);
    set
    {
      this.m_bHashValid = false;
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usBorderOptions, (ushort) 15, (ushort) value);
      if (value != OfficeLineStyle.None && this.LeftBorderPaletteIndex == (ushort) 0)
      {
        this.LeftBorderPaletteIndex = (ushort) 64 /*0x40*/;
      }
      else
      {
        if (value != OfficeLineStyle.None)
          return;
        this.LeftBorderPaletteIndex = (ushort) 0;
      }
    }
  }

  public OfficeLineStyle BorderRight
  {
    get
    {
      return (OfficeLineStyle) ((int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usBorderOptions, (ushort) 240 /*0xF0*/) >> 4);
    }
    set
    {
      this.m_bHashValid = false;
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usBorderOptions, (ushort) 240 /*0xF0*/, (ushort) ((uint) (ushort) value << 4));
      if (value != OfficeLineStyle.None && this.RightBorderPaletteIndex == (ushort) 0)
      {
        this.RightBorderPaletteIndex = (ushort) 64 /*0x40*/;
      }
      else
      {
        if (value != OfficeLineStyle.None)
          return;
        this.RightBorderPaletteIndex = (ushort) 0;
      }
    }
  }

  public OfficeLineStyle BorderTop
  {
    get
    {
      return (OfficeLineStyle) ((int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usBorderOptions, (ushort) 3840 /*0x0F00*/) >> 8);
    }
    set
    {
      this.m_bHashValid = false;
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usBorderOptions, (ushort) 3840 /*0x0F00*/, (ushort) ((uint) (ushort) value << 8));
      if (value != OfficeLineStyle.None && this.TopBorderPaletteIndex == (ushort) 0)
      {
        this.TopBorderPaletteIndex = (ushort) 64 /*0x40*/;
      }
      else
      {
        if (value != OfficeLineStyle.None)
          return;
        this.TopBorderPaletteIndex = (ushort) 0;
      }
    }
  }

  public OfficeLineStyle BorderBottom
  {
    get
    {
      return (OfficeLineStyle) ((int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usBorderOptions, (ushort) 61440 /*0xF000*/) >> 12);
    }
    set
    {
      this.m_bHashValid = false;
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usBorderOptions, (ushort) 61440 /*0xF000*/, (ushort) ((uint) (ushort) value << 12));
      if (value != OfficeLineStyle.None && this.BottomBorderPaletteIndex == (ushort) 0)
      {
        this.BottomBorderPaletteIndex = (ushort) 64 /*0x40*/;
      }
      else
      {
        if (value != OfficeLineStyle.None)
          return;
        this.BottomBorderPaletteIndex = (ushort) 0;
      }
    }
  }

  public OfficeHAlign HAlignmentType
  {
    get => (OfficeHAlign) BiffRecordRaw.GetUInt16BitsByMask(this.m_usAlignmentOptions, (ushort) 7);
    set
    {
      this.m_bHashValid = false;
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usAlignmentOptions, (ushort) 7, (ushort) value);
    }
  }

  public OfficeVAlign VAlignmentType
  {
    get
    {
      return (OfficeVAlign) ((int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usAlignmentOptions, (ushort) 112 /*0x70*/) >> 4);
    }
    set
    {
      this.m_bHashValid = false;
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usAlignmentOptions, (ushort) 112 /*0x70*/, (ushort) ((uint) (ushort) value << 4));
    }
  }

  public ushort FillBackground
  {
    get => BiffRecordRaw.GetUInt16BitsByMask(this.m_usFillPaletteOptions, (ushort) sbyte.MaxValue);
    set
    {
      if (value > (ushort) sbyte.MaxValue)
        throw new ArgumentOutOfRangeException();
      this.m_bHashValid = false;
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usFillPaletteOptions, (ushort) sbyte.MaxValue, value);
    }
  }

  public ushort FillForeground
  {
    get => (ushort) (((int) this.m_usFillPaletteOptions & 16256) >> 7);
    set
    {
      if (value > (ushort) sbyte.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (FillForeground), "Argument is too large");
      this.m_bHashValid = false;
      this.m_usFillPaletteOptions &= (ushort) 49279;
      this.m_usFillPaletteOptions |= (ushort) ((uint) value << 7);
    }
  }

  public override int MinimumRecordSize => 20;

  public override int MaximumRecordSize => 20;

  public ExtendedFormatRecord()
  {
  }

  public ExtendedFormatRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ExtendedFormatRecord(int iReserve)
    : base(iReserve)
  {
    this.m_iCode = 224 /*0xE0*/;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usFontIndex = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usFormatIndex = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usCellOptions = provider.ReadUInt16(iOffset);
    this.m_xfType = provider.ReadBit(iOffset, 2);
    this.m_b123Prefix = provider.ReadBit(iOffset, 3);
    this.m_bLocked = provider.ReadBit(iOffset, 0);
    this.m_bHidden = provider.ReadBit(iOffset, 1);
    iOffset += 2;
    this.m_usAlignmentOptions = provider.ReadUInt16(iOffset);
    this.m_bJustifyLast = provider.ReadBit(iOffset, 7);
    this.m_bWrapText = provider.ReadBit(iOffset, 3);
    iOffset += 2;
    int num = (int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usAlignmentOptions, (ushort) 112 /*0x70*/) >> 4;
    if (BiffRecordRaw.GetUInt16BitsByMask(this.m_usAlignmentOptions, (ushort) 7) == (ushort) 5 || num == 3)
      this.m_bWrapText = true;
    this.m_usIndentOptions = provider.ReadUInt16(iOffset);
    this.m_bMergeCells = provider.ReadBit(iOffset, 5);
    this.m_bShrinkToFit = provider.ReadBit(iOffset, 4);
    this.m_btIndent = (byte) BiffRecordRaw.GetUInt16BitsByMask(this.m_usIndentOptions, (ushort) 15);
    this.m_usIndentOptions &= (ushort) 65520;
    ++iOffset;
    this.m_bIndentNotParentBorder = provider.ReadBit(iOffset, 5);
    this.m_bIndentNotParentPattern = provider.ReadBit(iOffset, 6);
    this.m_bIndentNotParentCellOptions = provider.ReadBit(iOffset, 7);
    this.m_bIndentNotParentFormat = provider.ReadBit(iOffset, 2);
    this.m_bIndentNotParentFont = provider.ReadBit(iOffset, 3);
    this.m_bIndentNotParentAlignment = provider.ReadBit(iOffset, 4);
    ++iOffset;
    this.m_usBorderOptions = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usPaletteOptions = provider.ReadUInt16(iOffset);
    ++iOffset;
    this.m_bDiagnalFromBottomLeft = provider.ReadBit(iOffset, 7);
    this.m_bDiagnalFromTopLeft = provider.ReadBit(iOffset, 6);
    ++iOffset;
    this.m_uiAddPaletteOptions = provider.ReadUInt32(iOffset);
    this.m_usFillPattern = (ushort) ((this.m_uiAddPaletteOptions & 4227858432U /*0xFC000000*/) >> 26);
    iOffset += 4;
    this.m_usFillPaletteOptions = provider.ReadUInt16(iOffset);
    if (this.m_usFillPattern == (ushort) 0 && this.FillForeground == (ushort) 0)
      this.m_usFillPaletteOptions = (ushort) 8328;
    this.m_iLength = 20;
    this.SwapColors();
    this.m_usParentXFIndex = (ushort) ((uint) BiffRecordRaw.GetUInt16BitsByMask(this.m_usCellOptions, (ushort) 65520) >> 4);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    if (this.m_usParentXFIndex > (ushort) 4095 /*0x0FFF*/)
      this.m_usParentXFIndex = (ushort) 4095 /*0x0FFF*/;
    this.m_bHashValid = false;
    BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usCellOptions, (ushort) 65520, (ushort) ((uint) this.m_usParentXFIndex << 4));
    this.SwapColors();
    provider.WriteUInt16(iOffset, this.m_usFontIndex);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usFormatIndex);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usCellOptions);
    provider.WriteBit(iOffset, this.m_xfType, 2);
    provider.WriteBit(iOffset, this.m_b123Prefix, 3);
    provider.WriteBit(iOffset, this.m_bLocked, 0);
    provider.WriteBit(iOffset, this.m_bHidden, 1);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usAlignmentOptions);
    iOffset += 2;
    BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usIndentOptions, (ushort) 15, (ushort) this.m_btIndent);
    provider.WriteUInt16(iOffset, this.m_usIndentOptions);
    provider.WriteBit(iOffset, this.m_bMergeCells, 5);
    provider.WriteBit(iOffset, this.m_bShrinkToFit, 4);
    ++iOffset;
    provider.WriteBit(iOffset, this.m_bIndentNotParentBorder, 5);
    provider.WriteBit(iOffset, this.m_bIndentNotParentPattern, 6);
    provider.WriteBit(iOffset, this.m_bIndentNotParentCellOptions, 7);
    provider.WriteBit(iOffset, this.m_bIndentNotParentFormat, 2);
    provider.WriteBit(iOffset, this.m_bIndentNotParentFont, 3);
    provider.WriteBit(iOffset, this.m_bIndentNotParentAlignment, 4);
    ++iOffset;
    provider.WriteUInt16(iOffset, this.m_usBorderOptions);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usPaletteOptions);
    ++iOffset;
    provider.WriteBit(iOffset, this.m_bDiagnalFromBottomLeft, 7);
    provider.WriteBit(iOffset, this.m_bDiagnalFromTopLeft, 6);
    ++iOffset;
    this.m_usFillPattern = this.m_usFillPattern == (ushort) 4000 ? (ushort) 1 : this.m_usFillPattern;
    if (this.m_usFillPattern > (ushort) 63 /*0x3F*/)
      throw new ArgumentOutOfRangeException();
    this.m_bHashValid = false;
    this.m_uiAddPaletteOptions &= 67108863U /*0x03FFFFFF*/;
    this.m_uiAddPaletteOptions += (uint) this.m_usFillPattern << 26;
    provider.WriteInt32(iOffset, (int) this.m_uiAddPaletteOptions);
    iOffset += 4;
    provider.WriteUInt16(iOffset, this.m_usFillPaletteOptions);
    this.m_iLength = 20;
    this.SwapColors();
  }

  public int CompareTo(ExtendedFormatRecord twin)
  {
    if (twin == null)
      throw new ArgumentNullException(nameof (twin));
    int num1 = (this.m_bLocked ? 1 : 0) - (twin.m_bLocked ? 1 : 0);
    if (num1 != 0)
      return num1;
    int num2 = (this.m_bHidden ? 1 : 0) - (twin.m_bHidden ? 1 : 0);
    if (num2 != 0)
      return num2;
    int num3 = this.m_xfType.CompareTo(twin.m_xfType);
    if (num3 != 0)
      return num3;
    int num4 = (int) this.m_usAlignmentOptions - (int) twin.m_usAlignmentOptions;
    if (num4 != 0)
      return num4;
    int num5 = (int) this.Indent - (int) twin.Indent;
    if (num5 != 0)
      return num5;
    int num6 = (int) this.ReadingOrder - (int) twin.ReadingOrder;
    if (num6 != 0)
      return num6;
    int num7 = (this.m_bShrinkToFit ? 1 : 0) - (twin.m_bShrinkToFit ? 1 : 0);
    if (num7 != 0)
      return num7;
    int num8 = (this.m_bMergeCells ? 1 : 0) - (twin.m_bMergeCells ? 1 : 0);
    if (num8 != 0)
      return num8;
    int num9 = (this.m_bIndentNotParentFormat ? 1 : 0) - (twin.m_bIndentNotParentFormat ? 1 : 0);
    if (num9 != 0)
      return num9;
    int num10 = (this.m_bIndentNotParentFont ? 1 : 0) - (twin.m_bIndentNotParentFont ? 1 : 0);
    if (num10 != 0)
      return num10;
    int num11 = (this.m_bIndentNotParentAlignment ? 1 : 0) - (twin.m_bIndentNotParentAlignment ? 1 : 0);
    if (num11 != 0)
      return num11;
    int num12 = (this.m_bIndentNotParentBorder ? 1 : 0) - (twin.m_bIndentNotParentBorder ? 1 : 0);
    if (num12 != 0)
      return num12;
    int num13 = (this.m_bIndentNotParentPattern ? 1 : 0) - (twin.m_bIndentNotParentPattern ? 1 : 0);
    if (num13 != 0)
      return num13;
    int num14 = (this.m_bIndentNotParentCellOptions ? 1 : 0) - (twin.m_bIndentNotParentCellOptions ? 1 : 0);
    if (num14 != 0)
      return num14;
    int num15 = (int) this.m_usBorderOptions - (int) twin.m_usBorderOptions;
    if (num15 != 0)
      return num15;
    int num16 = (int) this.m_usPaletteOptions - (int) twin.m_usPaletteOptions;
    if (num16 != 0)
      return num16;
    long num17 = (long) this.m_uiAddPaletteOptions - (long) twin.m_uiAddPaletteOptions;
    if (num17 != 0L)
      return num17 <= 0L ? -1 : 1;
    int num18 = (int) this.m_usFillPaletteOptions - (int) twin.m_usFillPaletteOptions;
    if (num18 != 0)
      return num18;
    int num19 = (int) this.m_usFillPattern - (int) twin.m_usFillPattern;
    if (num19 != 0)
      return num19;
    int num20 = (this.m_b123Prefix ? 1 : 0) - (twin.m_b123Prefix ? 1 : 0);
    if (num20 != 0)
      return num20;
    int num21 = (int) this.m_usFormatIndex - (int) twin.m_usFormatIndex;
    if (num21 != 0)
      return num21;
    int num22 = (int) this.m_usFontIndex - (int) twin.m_usFontIndex;
    return num22 != 0 ? num22 : (int) this.m_usParentXFIndex - (int) twin.m_usParentXFIndex;
  }

  public override int GetHashCode()
  {
    if (!this.m_bHashValid)
    {
      this.m_iHash = this.m_usFontIndex.GetHashCode() ^ this.m_usFormatIndex.GetHashCode() ^ ((int) this.m_usCellOptions & 65520).GetHashCode() ^ this.m_bLocked.GetHashCode() ^ this.m_bHidden.GetHashCode() ^ this.m_xfType.GetHashCode() ^ this.m_b123Prefix.GetHashCode() ^ this.m_usAlignmentOptions.GetHashCode() ^ this.m_bWrapText.GetHashCode() ^ this.m_bJustifyLast.GetHashCode() ^ this.m_usIndentOptions.GetHashCode() ^ this.m_bShrinkToFit.GetHashCode() ^ this.m_bMergeCells.GetHashCode() ^ this.m_bIndentNotParentFormat.GetHashCode() ^ this.m_bIndentNotParentFont.GetHashCode() ^ this.m_bIndentNotParentAlignment.GetHashCode() ^ this.m_bIndentNotParentBorder.GetHashCode() ^ this.m_bIndentNotParentPattern.GetHashCode() ^ this.m_bIndentNotParentCellOptions.GetHashCode() ^ this.m_usBorderOptions.GetHashCode() ^ this.m_usPaletteOptions.GetHashCode() ^ this.m_bDiagnalFromTopLeft.GetHashCode() ^ this.m_bDiagnalFromBottomLeft.GetHashCode() ^ this.m_uiAddPaletteOptions.GetHashCode() ^ this.m_usFillPattern.GetHashCode() ^ this.m_usFillPaletteOptions.GetHashCode();
      this.m_bHashValid = true;
    }
    return this.m_iHash;
  }

  private void SwapColors()
  {
    if (this.AdtlFillPattern == (ushort) 1)
      return;
    ushort fillBackground = this.FillBackground;
    this.FillBackground = this.FillForeground;
    this.FillForeground = fillBackground;
  }

  public void CopyBorders(ExtendedFormatRecord source)
  {
    this.m_usBorderOptions = source != null ? source.m_usBorderOptions : throw new ArgumentNullException(nameof (source));
    this.BorderBottom = source.BorderBottom;
    this.BorderLeft = source.BorderLeft;
    this.BorderRight = source.BorderRight;
    this.BorderTop = source.BorderTop;
    this.BottomBorderPaletteIndex = source.BottomBorderPaletteIndex;
    this.LeftBorderPaletteIndex = source.LeftBorderPaletteIndex;
    this.RightBorderPaletteIndex = source.RightBorderPaletteIndex;
    this.TopBorderPaletteIndex = source.TopBorderPaletteIndex;
    this.DiagonalFromBottomLeft = source.DiagonalFromBottomLeft;
    this.DiagonalFromTopLeft = source.DiagonalFromTopLeft;
    this.DiagonalLineColor = source.DiagonalLineColor;
    this.DiagonalLineStyle = source.DiagonalLineStyle;
  }

  public void CopyAlignment(ExtendedFormatRecord source)
  {
    this.m_usAlignmentOptions = source != null ? source.m_usAlignmentOptions : throw new ArgumentNullException(nameof (source));
    this.MergeCells = source.MergeCells;
    this.Rotation = source.Rotation;
    this.ShrinkToFit = source.ShrinkToFit;
    this.Indent = source.Indent;
  }

  public void CopyPatterns(ExtendedFormatRecord source)
  {
    this.AdtlFillPattern = source != null ? source.AdtlFillPattern : throw new ArgumentNullException(nameof (source));
    this.FillBackground = source.FillBackground;
    this.FillForeground = source.FillForeground;
  }

  public void CopyProtection(ExtendedFormatRecord source)
  {
    this.IsLocked = source != null ? source.IsLocked : throw new ArgumentNullException("m_extFormat");
    this.IsHidden = source.IsHidden;
  }

  public void CopyTo(ExtendedFormatRecord twin)
  {
    twin.m_b123Prefix = this.m_b123Prefix;
    twin.m_bDiagnalFromBottomLeft = this.m_bDiagnalFromBottomLeft;
    twin.m_bDiagnalFromTopLeft = this.m_bDiagnalFromTopLeft;
    twin.m_bHashValid = this.m_bHashValid;
    twin.m_bHidden = this.m_bHidden;
    twin.m_bIndentNotParentAlignment = this.m_bIndentNotParentAlignment;
    twin.m_bIndentNotParentBorder = this.m_bIndentNotParentBorder;
    twin.m_bIndentNotParentCellOptions = this.m_bIndentNotParentCellOptions;
    twin.m_bIndentNotParentFont = this.m_bIndentNotParentFont;
    twin.m_bIndentNotParentFormat = this.m_bIndentNotParentFormat;
    twin.m_bIndentNotParentPattern = this.m_bIndentNotParentPattern;
    twin.m_bJustifyLast = this.m_bJustifyLast;
    twin.m_bLocked = this.m_bLocked;
    twin.m_bMergeCells = this.m_bMergeCells;
    twin.m_bShrinkToFit = this.m_bShrinkToFit;
    twin.m_bWrapText = this.m_bWrapText;
    twin.m_iCode = this.m_iCode;
    twin.m_iHash = this.m_iHash;
    twin.m_iLength = this.m_iLength;
    twin.m_uiAddPaletteOptions = this.m_uiAddPaletteOptions;
    twin.m_usAlignmentOptions = this.m_usAlignmentOptions;
    twin.m_usBorderOptions = this.m_usBorderOptions;
    twin.m_usCellOptions = this.m_usCellOptions;
    twin.m_usFillPaletteOptions = this.m_usFillPaletteOptions;
    twin.m_usFontIndex = this.m_usFontIndex;
    twin.m_usFormatIndex = this.m_usFormatIndex;
    twin.m_usIndentOptions = this.m_usIndentOptions;
    twin.m_usPaletteOptions = this.m_usPaletteOptions;
    twin.m_xfType = this.m_xfType;
    twin.m_usParentXFIndex = this.m_usParentXFIndex;
    twin.m_usFillPattern = this.m_usFillPattern;
  }

  internal void SetWorkbook(WorkbookImpl book) => this.m_book = book;

  public override void CopyTo(BiffRecordRaw raw)
  {
    if (raw == null)
      throw new ArgumentNullException(nameof (raw));
    if (!(raw is ExtendedFormatRecord twin))
      throw new ArgumentException(nameof (raw));
    this.CopyTo(twin);
  }

  public enum TXFType
  {
    XF_CELL,
    XF_STYLE,
  }
}
