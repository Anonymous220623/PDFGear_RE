// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.RowRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.Row)]
[CLSCompliant(false)]
public class RowRecord : BiffRecordRaw, IOutline
{
  public const ushort DEF_OUTLINE_LEVEL_MASK = 7;
  public const double DEF_MAX_HEIGHT = 409.5;
  internal const int DEF_RECORD_SIZE = 16 /*0x10*/;
  [BiffRecordPos(0, 2)]
  private ushort m_usRowNumber;
  [BiffRecordPos(2, 2)]
  private ushort m_usFirstCol;
  [BiffRecordPos(4, 2)]
  private ushort m_usLastCol;
  [BiffRecordPos(6, 2)]
  private ushort m_usHeigth;
  [BiffRecordPos(8, 4, true)]
  private int m_iReserved;
  [BiffRecordPos(12, 4, true)]
  private RowRecord.OptionFlags m_optionFlags = RowRecord.OptionFlags.ShowOutlineGroups;
  private WorksheetImpl m_sheet;

  public int Options
  {
    get => (int) this.m_optionFlags;
    set => this.m_optionFlags = (RowRecord.OptionFlags) value;
  }

  public ushort RowNumber
  {
    get => this.m_usRowNumber;
    set => this.m_usRowNumber = value;
  }

  public ushort FirstColumn
  {
    get => this.m_usFirstCol;
    set => this.m_usFirstCol = value;
  }

  public ushort LastColumn
  {
    get => this.m_usLastCol;
    set => this.m_usLastCol = value;
  }

  public ushort Height
  {
    get => this.m_usHeigth;
    set => this.m_usHeigth = value;
  }

  public ushort ExtendedFormatIndex
  {
    get
    {
      return (ushort) ((uint) (this.m_optionFlags & (RowRecord.OptionFlags) 268369920 /*0x0FFF0000*/) >> 16 /*0x10*/);
    }
    set
    {
      int num = (int) (this.m_optionFlags & (RowRecord.OptionFlags) -268369921 | (RowRecord.OptionFlags) ((int) value << 16 /*0x10*/ & 268369920 /*0x0FFF0000*/));
      if (value != (ushort) 15)
        this.IsFormatted = true;
      this.m_optionFlags = (RowRecord.OptionFlags) num;
    }
  }

  public ushort OutlineLevel
  {
    get => (ushort) (this.m_optionFlags & (RowRecord.OptionFlags) 7);
    set
    {
      if (value > (ushort) 7)
        throw new ArgumentOutOfRangeException();
      this.m_optionFlags = this.m_optionFlags & (RowRecord.OptionFlags) -8 | (RowRecord.OptionFlags) ((int) value & 7);
    }
  }

  public bool IsCollapsed
  {
    get => (this.m_optionFlags & RowRecord.OptionFlags.Colapsed) != (RowRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_optionFlags |= RowRecord.OptionFlags.Colapsed;
      else
        this.m_optionFlags &= ~RowRecord.OptionFlags.Colapsed;
    }
  }

  public bool IsHidden
  {
    get => (this.m_optionFlags & RowRecord.OptionFlags.ZeroHeight) != (RowRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_optionFlags |= RowRecord.OptionFlags.ZeroHeight;
      else
        this.m_optionFlags &= ~RowRecord.OptionFlags.ZeroHeight;
    }
  }

  public bool IsBadFontHeight
  {
    get => (this.m_optionFlags & RowRecord.OptionFlags.BadFontHeight) != (RowRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_optionFlags |= RowRecord.OptionFlags.BadFontHeight;
      else
        this.m_optionFlags &= ~RowRecord.OptionFlags.BadFontHeight;
    }
  }

  public bool IsFormatted
  {
    get => (this.m_optionFlags & RowRecord.OptionFlags.Formatted) != (RowRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_optionFlags |= RowRecord.OptionFlags.Formatted;
      else
        this.m_optionFlags &= ~RowRecord.OptionFlags.Formatted;
    }
  }

  public bool IsSpaceAboveRow
  {
    get => (this.m_optionFlags & RowRecord.OptionFlags.SpaceAbove) != (RowRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_optionFlags |= RowRecord.OptionFlags.SpaceAbove;
      else
        this.m_optionFlags &= ~RowRecord.OptionFlags.SpaceAbove;
    }
  }

  public bool IsSpaceBelowRow
  {
    get => (this.m_optionFlags & RowRecord.OptionFlags.SpaceBelow) != (RowRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_optionFlags |= RowRecord.OptionFlags.SpaceBelow;
      else
        this.m_optionFlags &= ~RowRecord.OptionFlags.SpaceBelow;
    }
  }

  public bool IsGroupShown
  {
    get
    {
      return (this.m_optionFlags & RowRecord.OptionFlags.ShowOutlineGroups) != (RowRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_optionFlags |= RowRecord.OptionFlags.ShowOutlineGroups;
      else
        this.m_optionFlags &= ~RowRecord.OptionFlags.ShowOutlineGroups;
    }
  }

  public int Reserved => this.m_iReserved;

  public override int MinimumRecordSize => 16 /*0x10*/;

  public override int MaximumRecordSize => 16 /*0x10*/;

  public override int MaximumMemorySize => 16 /*0x10*/;

  ushort IOutline.Index
  {
    get => this.RowNumber;
    set => this.RowNumber = value;
  }

  internal WorksheetImpl Worksheet
  {
    get => this.m_sheet;
    set => this.m_sheet = value;
  }

  public RowRecord()
  {
  }

  public RowRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public RowRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usRowNumber = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usFirstCol = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usLastCol = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usHeigth = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_iReserved = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_optionFlags = (RowRecord.OptionFlags) provider.ReadInt32(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.IsFormatted = this.ExtendedFormatIndex != (ushort) 15;
    provider.WriteUInt16(iOffset, this.m_usRowNumber);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usFirstCol);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usLastCol);
    iOffset += 2;
    if (this.Worksheet != null)
    {
      ushort num = this.IsBadFontHeight ? this.m_usHeigth : (ushort) (this.Worksheet.PageSetup as PageSetupImpl).DefaultRowHeight;
      provider.WriteUInt16(iOffset, num);
    }
    else
      provider.WriteUInt16(iOffset, this.m_usHeigth);
    iOffset += 2;
    provider.WriteInt32(iOffset, this.m_iReserved);
    iOffset += 4;
    provider.WriteInt32(iOffset, (int) this.m_optionFlags);
  }

  internal enum OptionFlags
  {
    Colapsed = 16, // 0x00000010
    ZeroHeight = 32, // 0x00000020
    BadFontHeight = 64, // 0x00000040
    Formatted = 128, // 0x00000080
    ShowOutlineGroups = 256, // 0x00000100
    SpaceAbove = 268435456, // 0x10000000
    SpaceBelow = 536870912, // 0x20000000
  }
}
