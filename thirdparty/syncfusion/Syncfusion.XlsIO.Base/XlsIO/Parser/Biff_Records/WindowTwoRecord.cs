// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.WindowTwoRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.WindowTwo)]
[CLSCompliant(false)]
public class WindowTwoRecord : BiffRecordRaw
{
  private const int DEF_MAX_RECORD_SIZE = 18;
  internal const int DEF_MAX_CHART_SHEET_SIZE = 10;
  [BiffRecordPos(0, 2)]
  private WindowTwoRecord.OptionFlags m_options = WindowTwoRecord.OptionFlags.DisplayGridlines | WindowTwoRecord.OptionFlags.DisplayRowColHeadings | WindowTwoRecord.OptionFlags.DisplayZeros | WindowTwoRecord.OptionFlags.DefaultHeader | WindowTwoRecord.OptionFlags.DisplayGuts;
  [BiffRecordPos(2, 2)]
  private ushort m_usTopRow;
  [BiffRecordPos(4, 2)]
  private ushort m_usLeftCol;
  [BiffRecordPos(6, 4, true)]
  private int m_iHeaderColor = 64 /*0x40*/;
  private ushort m_usPageBreakZoom;
  private ushort m_usNormalZoom;
  private int m_iReserved;
  private int m_iOriginalLength;

  public ushort TopRow
  {
    get => this.m_usTopRow;
    set => this.m_usTopRow = value;
  }

  public ushort LeftColumn
  {
    get => this.m_usLeftCol;
    set => this.m_usLeftCol = value;
  }

  public int HeaderColor
  {
    get => this.m_iHeaderColor;
    set => this.m_iHeaderColor = value;
  }

  public bool IsDisplayFormulas
  {
    get
    {
      return (this.m_options & WindowTwoRecord.OptionFlags.DisplayFormulas) != (WindowTwoRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_options |= WindowTwoRecord.OptionFlags.DisplayFormulas;
      else
        this.m_options &= ~WindowTwoRecord.OptionFlags.DisplayFormulas;
    }
  }

  public bool IsDisplayGridlines
  {
    get
    {
      return (this.m_options & WindowTwoRecord.OptionFlags.DisplayGridlines) != (WindowTwoRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_options |= WindowTwoRecord.OptionFlags.DisplayGridlines;
      else
        this.m_options &= ~WindowTwoRecord.OptionFlags.DisplayGridlines;
    }
  }

  public bool IsDisplayRowColHeadings
  {
    get
    {
      return (this.m_options & WindowTwoRecord.OptionFlags.DisplayRowColHeadings) != (WindowTwoRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_options |= WindowTwoRecord.OptionFlags.DisplayRowColHeadings;
      else
        this.m_options &= ~WindowTwoRecord.OptionFlags.DisplayRowColHeadings;
    }
  }

  public bool IsFreezePanes
  {
    get
    {
      return (this.m_options & WindowTwoRecord.OptionFlags.FreezePanes) != (WindowTwoRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_options |= WindowTwoRecord.OptionFlags.FreezePanes;
      else
        this.m_options &= ~WindowTwoRecord.OptionFlags.FreezePanes;
    }
  }

  public bool IsDisplayZeros
  {
    get
    {
      return (this.m_options & WindowTwoRecord.OptionFlags.DisplayZeros) != (WindowTwoRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_options |= WindowTwoRecord.OptionFlags.DisplayZeros;
      else
        this.m_options &= ~WindowTwoRecord.OptionFlags.DisplayZeros;
    }
  }

  public bool IsDefaultHeader
  {
    get
    {
      return (this.m_options & WindowTwoRecord.OptionFlags.DefaultHeader) != (WindowTwoRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_options |= WindowTwoRecord.OptionFlags.DefaultHeader;
      else
        this.m_options &= ~WindowTwoRecord.OptionFlags.DefaultHeader;
    }
  }

  public bool IsArabic
  {
    get => (this.m_options & WindowTwoRecord.OptionFlags.Arabic) != (WindowTwoRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_options |= WindowTwoRecord.OptionFlags.Arabic;
      else
        this.m_options &= ~WindowTwoRecord.OptionFlags.Arabic;
    }
  }

  public bool IsDisplayGuts
  {
    get
    {
      return (this.m_options & WindowTwoRecord.OptionFlags.DisplayGuts) != (WindowTwoRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_options |= WindowTwoRecord.OptionFlags.DisplayGuts;
      else
        this.m_options &= ~WindowTwoRecord.OptionFlags.DisplayGuts;
    }
  }

  public bool IsFreezePanesNoSplit
  {
    get
    {
      return (this.m_options & WindowTwoRecord.OptionFlags.FreezePanesNoSplit) != (WindowTwoRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_options |= WindowTwoRecord.OptionFlags.FreezePanesNoSplit;
      else
        this.m_options &= ~WindowTwoRecord.OptionFlags.FreezePanesNoSplit;
    }
  }

  public bool IsSelected
  {
    get
    {
      return (this.m_options & WindowTwoRecord.OptionFlags.Selected) != (WindowTwoRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_options |= WindowTwoRecord.OptionFlags.Selected;
      else
        this.m_options &= ~WindowTwoRecord.OptionFlags.Selected;
    }
  }

  public bool IsPaged
  {
    get => (this.m_options & WindowTwoRecord.OptionFlags.Paged) != (WindowTwoRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_options |= WindowTwoRecord.OptionFlags.Paged;
      else
        this.m_options &= ~WindowTwoRecord.OptionFlags.Paged;
    }
  }

  public bool IsSavedInPageBreakPreview
  {
    get
    {
      return (this.m_options & WindowTwoRecord.OptionFlags.SavedInPageBreakPreview) != (WindowTwoRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_options |= WindowTwoRecord.OptionFlags.SavedInPageBreakPreview;
      else
        this.m_options &= ~WindowTwoRecord.OptionFlags.SavedInPageBreakPreview;
    }
  }

  public ushort Options => (ushort) this.m_options;

  public override int MinimumRecordSize => 10;

  public override int MaximumRecordSize => 18;

  internal int OriginalLength
  {
    get => this.m_iOriginalLength;
    set => this.m_iOriginalLength = value;
  }

  public WindowTwoRecord()
  {
  }

  public WindowTwoRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public WindowTwoRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_options = (WindowTwoRecord.OptionFlags) provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usTopRow = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usLeftCol = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_iHeaderColor = provider.ReadInt32(iOffset);
    iOffset += 4;
    if (this.m_iLength > 10)
    {
      this.m_usPageBreakZoom = provider.ReadUInt16(iOffset);
      iOffset += 2;
      this.m_usNormalZoom = provider.ReadUInt16(iOffset);
      iOffset += 2;
    }
    if (this.m_iLength > 14)
      this.m_iReserved = provider.ReadInt32(iOffset);
    this.m_iOriginalLength = this.m_iLength;
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, (ushort) this.m_options);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usTopRow);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usLeftCol);
    iOffset += 2;
    provider.WriteInt32(iOffset, this.m_iHeaderColor);
    iOffset += 4;
    provider.WriteUInt16(iOffset, this.m_usPageBreakZoom);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usNormalZoom);
    iOffset += 2;
    provider.WriteInt32(iOffset, this.m_iReserved);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    return this.m_iOriginalLength <= 0 ? 18 : this.m_iOriginalLength;
  }

  [Flags]
  private enum OptionFlags : ushort
  {
    DisplayFormulas = 1,
    DisplayGridlines = 2,
    DisplayRowColHeadings = 4,
    FreezePanes = 8,
    DisplayZeros = 16, // 0x0010
    DefaultHeader = 32, // 0x0020
    Arabic = 64, // 0x0040
    DisplayGuts = 128, // 0x0080
    FreezePanesNoSplit = 256, // 0x0100
    Selected = 512, // 0x0200
    Paged = 1024, // 0x0400
    SavedInPageBreakPreview = 2048, // 0x0800
  }
}
