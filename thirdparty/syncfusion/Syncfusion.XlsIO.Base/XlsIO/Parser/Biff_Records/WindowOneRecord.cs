// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.WindowOneRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.WindowOne)]
public class WindowOneRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 18;
  [BiffRecordPos(0, 2)]
  private ushort m_usHHold = 240 /*0xF0*/;
  [BiffRecordPos(2, 2)]
  private ushort m_usVHold = 90;
  [BiffRecordPos(4, 2)]
  private ushort m_usWidth = 11340;
  [BiffRecordPos(6, 2)]
  private ushort m_usHeight = 6795;
  [BiffRecordPos(8, 2)]
  private WindowOneRecord.OptionFlags m_options = WindowOneRecord.OptionFlags.HScroll | WindowOneRecord.OptionFlags.VScroll | WindowOneRecord.OptionFlags.Tabs;
  [BiffRecordPos(10, 2)]
  private ushort m_usSelectedTab;
  [BiffRecordPos(12, 2)]
  private ushort m_usDisplayedTab;
  [BiffRecordPos(14, 2)]
  private ushort m_usNumSelTabs = 1;
  [BiffRecordPos(16 /*0x10*/, 2)]
  private ushort m_usTabWidthRatio = 600;

  public ushort HHold
  {
    get => this.m_usHHold;
    set => this.m_usHHold = value;
  }

  public ushort VHold
  {
    get => this.m_usVHold;
    set => this.m_usVHold = value;
  }

  public ushort Width
  {
    get => this.m_usWidth;
    set => this.m_usWidth = value;
  }

  public ushort Height
  {
    get => this.m_usHeight;
    set => this.m_usHeight = value;
  }

  public ushort SelectedTab
  {
    get => this.m_usSelectedTab;
    set => this.m_usSelectedTab = value;
  }

  public ushort DisplayedTab
  {
    get => this.m_usDisplayedTab;
    set => this.m_usDisplayedTab = value;
  }

  public ushort NumSelectedTabs
  {
    get => this.m_usNumSelTabs;
    set => this.m_usNumSelTabs = value;
  }

  public ushort TabWidthRatio
  {
    get => this.m_usTabWidthRatio;
    set => this.m_usTabWidthRatio = value;
  }

  public bool IsHidden
  {
    get => (this.m_options & WindowOneRecord.OptionFlags.Hidden) != (WindowOneRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_options |= WindowOneRecord.OptionFlags.Hidden;
      else
        this.m_options &= ~WindowOneRecord.OptionFlags.Hidden;
    }
  }

  public bool IsIconic
  {
    get => (this.m_options & WindowOneRecord.OptionFlags.Iconic) != (WindowOneRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_options |= WindowOneRecord.OptionFlags.Iconic;
      else
        this.m_options &= ~WindowOneRecord.OptionFlags.Iconic;
    }
  }

  public bool IsHScroll
  {
    get
    {
      return (this.m_options & WindowOneRecord.OptionFlags.HScroll) != (WindowOneRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_options |= WindowOneRecord.OptionFlags.HScroll;
      else
        this.m_options &= ~WindowOneRecord.OptionFlags.HScroll;
    }
  }

  public bool IsVScroll
  {
    get
    {
      return (this.m_options & WindowOneRecord.OptionFlags.VScroll) != (WindowOneRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_options |= WindowOneRecord.OptionFlags.VScroll;
      else
        this.m_options &= ~WindowOneRecord.OptionFlags.VScroll;
    }
  }

  public bool IsTabs
  {
    get => (this.m_options & WindowOneRecord.OptionFlags.Tabs) != (WindowOneRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_options |= WindowOneRecord.OptionFlags.Tabs;
      else
        this.m_options &= ~WindowOneRecord.OptionFlags.Tabs;
    }
  }

  public bool Reserved
  {
    get
    {
      return (this.m_options & WindowOneRecord.OptionFlags.Reserved) != (WindowOneRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_options |= WindowOneRecord.OptionFlags.Reserved;
      else
        this.m_options &= ~WindowOneRecord.OptionFlags.Reserved;
    }
  }

  public ushort Options => (ushort) this.m_options;

  public override int MinimumRecordSize => 18;

  public override int MaximumRecordSize => 18;

  public WindowOneRecord()
  {
  }

  public WindowOneRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public WindowOneRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usHHold = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usVHold = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usWidth = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usHeight = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_options = (WindowOneRecord.OptionFlags) provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usSelectedTab = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usDisplayedTab = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usNumSelTabs = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usTabWidthRatio = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = 18;
    provider.WriteUInt16(iOffset, this.m_usHHold);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usVHold);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usWidth);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usHeight);
    iOffset += 2;
    provider.WriteUInt16(iOffset, (ushort) this.m_options);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usSelectedTab);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usDisplayedTab);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usNumSelTabs);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usTabWidthRatio);
  }

  private enum OptionFlags
  {
    Hidden = 1,
    Iconic = 2,
    Reserved = 4,
    HScroll = 8,
    VScroll = 16, // 0x00000010
    Tabs = 32, // 0x00000020
  }
}
