// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartAttachedLabelRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartAttachedLabel)]
public class ChartAttachedLabelRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 2;
  [BiffRecordPos(0, 2)]
  private ChartAttachedLabelRecord.OptionFlags m_options;

  public ushort Options => (ushort) this.m_options;

  public bool ShowActiveValue
  {
    get
    {
      return (this.m_options & ChartAttachedLabelRecord.OptionFlags.ActiveValue) != ChartAttachedLabelRecord.OptionFlags.None;
    }
    set
    {
      if (value)
        this.m_options |= ChartAttachedLabelRecord.OptionFlags.ActiveValue;
      else
        this.m_options &= ~ChartAttachedLabelRecord.OptionFlags.ActiveValue;
    }
  }

  public bool ShowPieInPercents
  {
    get
    {
      return (this.m_options & ChartAttachedLabelRecord.OptionFlags.PiePercents) != ChartAttachedLabelRecord.OptionFlags.None;
    }
    set
    {
      if (value)
        this.m_options |= ChartAttachedLabelRecord.OptionFlags.PiePercents;
      else
        this.m_options &= ~ChartAttachedLabelRecord.OptionFlags.PiePercents;
    }
  }

  public bool ShowPieCategoryLabel
  {
    get
    {
      return (this.m_options & ChartAttachedLabelRecord.OptionFlags.PieCategoryLabel) != ChartAttachedLabelRecord.OptionFlags.None;
    }
    set
    {
      if (value)
        this.m_options |= ChartAttachedLabelRecord.OptionFlags.PieCategoryLabel;
      else
        this.m_options &= ~ChartAttachedLabelRecord.OptionFlags.PieCategoryLabel;
    }
  }

  public bool SmoothLine
  {
    get
    {
      return (this.m_options & ChartAttachedLabelRecord.OptionFlags.SmoothLine) != ChartAttachedLabelRecord.OptionFlags.None;
    }
    set
    {
      if (value)
        this.m_options |= ChartAttachedLabelRecord.OptionFlags.SmoothLine;
      else
        this.m_options &= ~ChartAttachedLabelRecord.OptionFlags.SmoothLine;
    }
  }

  public bool ShowCategoryLabel
  {
    get
    {
      return (this.m_options & ChartAttachedLabelRecord.OptionFlags.CategoryLabel) != ChartAttachedLabelRecord.OptionFlags.None;
    }
    set
    {
      if (value)
        this.m_options |= ChartAttachedLabelRecord.OptionFlags.CategoryLabel;
      else
        this.m_options &= ~ChartAttachedLabelRecord.OptionFlags.CategoryLabel;
    }
  }

  public bool ShowBubble
  {
    get
    {
      return (this.m_options & ChartAttachedLabelRecord.OptionFlags.Bubble) != ChartAttachedLabelRecord.OptionFlags.None;
    }
    set
    {
      if (value)
        this.m_options |= ChartAttachedLabelRecord.OptionFlags.Bubble;
      else
        this.m_options &= ~ChartAttachedLabelRecord.OptionFlags.Bubble;
    }
  }

  public ChartAttachedLabelRecord()
  {
  }

  public ChartAttachedLabelRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartAttachedLabelRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_options = (ChartAttachedLabelRecord.OptionFlags) provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, (ushort) this.m_options);
  }

  public override int GetStoreSize(ExcelVersion version) => 2;

  [Flags]
  private enum OptionFlags
  {
    None = 0,
    ActiveValue = 1,
    PiePercents = 2,
    PieCategoryLabel = 4,
    SmoothLine = 8,
    CategoryLabel = 16, // 0x00000010
    Bubble = 32, // 0x00000020
  }
}
