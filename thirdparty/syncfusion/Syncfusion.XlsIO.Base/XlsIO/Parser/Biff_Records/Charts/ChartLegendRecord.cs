// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartLegendRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartLegend)]
public class ChartLegendRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 20;
  [BiffRecordPos(0, 4, true)]
  private int m_iTopLeftX;
  [BiffRecordPos(4, 4, true)]
  private int m_iTopLeftY;
  [BiffRecordPos(8, 4, true)]
  private int m_iWidth;
  [BiffRecordPos(12, 4, true)]
  private int m_iHeight;
  [BiffRecordPos(16 /*0x10*/, 1)]
  private byte m_wType = 3;
  [BiffRecordPos(17, 1)]
  private byte m_wSpacing = 1;
  [BiffRecordPos(18, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(18, 0, TFieldType.Bit)]
  private bool m_bAutoPosition = true;
  [BiffRecordPos(18, 1, TFieldType.Bit)]
  private bool m_bAutoSeries = true;
  [BiffRecordPos(18, 2, TFieldType.Bit)]
  private bool m_bAutoPosX = true;
  [BiffRecordPos(18, 3, TFieldType.Bit)]
  private bool m_bAutoPosY = true;
  [BiffRecordPos(18, 4, TFieldType.Bit)]
  private bool m_bIsVerticalLegend = true;
  [BiffRecordPos(18, 5, TFieldType.Bit)]
  private bool m_bContainsDataTable;

  public int X
  {
    get => this.m_iTopLeftX;
    set
    {
      if (value == this.m_iTopLeftX)
        return;
      this.m_iTopLeftX = value;
    }
  }

  public int Y
  {
    get => this.m_iTopLeftY;
    set
    {
      if (value == this.m_iTopLeftY)
        return;
      this.m_iTopLeftY = value;
    }
  }

  public int Width
  {
    get => this.m_iWidth;
    set
    {
      if (value == this.m_iWidth)
        return;
      this.m_iWidth = value;
    }
  }

  public int Height
  {
    get => this.m_iHeight;
    set
    {
      if (value == this.m_iHeight)
        return;
      this.m_iHeight = value;
    }
  }

  public ExcelLegendPosition Position
  {
    get => (ExcelLegendPosition) this.m_wType;
    set => this.m_wType = (byte) value;
  }

  public ExcelLegendSpacing Spacing
  {
    get => (ExcelLegendSpacing) this.m_wSpacing;
    set => this.m_wSpacing = (byte) value;
  }

  public bool AutoPosition
  {
    get => this.m_bAutoPosition;
    set => this.m_bAutoPosition = value;
  }

  public bool AutoSeries
  {
    get => this.m_bAutoSeries;
    set => this.m_bAutoSeries = value;
  }

  public bool AutoPositionX
  {
    get => this.m_bAutoPosX;
    set => this.m_bAutoPosX = value;
  }

  public bool AutoPositionY
  {
    get => this.m_bAutoPosY;
    set => this.m_bAutoPosY = value;
  }

  public bool IsVerticalLegend
  {
    get => this.m_bIsVerticalLegend;
    set => this.m_bIsVerticalLegend = value;
  }

  public bool ContainsDataTable
  {
    get => this.m_bContainsDataTable;
    set => this.m_bContainsDataTable = value;
  }

  public ChartLegendRecord()
  {
  }

  public ChartLegendRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartLegendRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_iTopLeftX = provider.ReadInt32(iOffset);
    this.m_iTopLeftY = provider.ReadInt32(iOffset + 4);
    this.m_iWidth = provider.ReadInt32(iOffset + 8);
    this.m_iHeight = provider.ReadInt32(iOffset + 12);
    this.m_wType = provider.ReadByte(iOffset + 16 /*0x10*/);
    this.m_wSpacing = provider.ReadByte(iOffset + 17);
    this.m_usOptions = provider.ReadUInt16(iOffset + 18);
    this.m_bAutoPosition = provider.ReadBit(iOffset + 18, 0);
    this.m_bAutoSeries = provider.ReadBit(iOffset + 18, 1);
    this.m_bAutoPosX = provider.ReadBit(iOffset + 18, 2);
    this.m_bAutoPosY = provider.ReadBit(iOffset + 18, 3);
    this.m_bIsVerticalLegend = provider.ReadBit(iOffset + 18, 4);
    this.m_bContainsDataTable = provider.ReadBit(iOffset + 18, 5);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_usOptions &= (ushort) 63 /*0x3F*/;
    provider.WriteInt32(iOffset, this.m_iTopLeftX);
    provider.WriteInt32(iOffset + 4, this.m_iTopLeftY);
    provider.WriteInt32(iOffset + 8, this.m_iWidth);
    provider.WriteInt32(iOffset + 12, this.m_iHeight);
    provider.WriteByte(iOffset + 16 /*0x10*/, this.m_wType);
    provider.WriteByte(iOffset + 17, this.m_wSpacing);
    provider.WriteUInt16(iOffset + 18, this.m_usOptions);
    provider.WriteBit(iOffset + 18, this.m_bAutoPosition, 0);
    provider.WriteBit(iOffset + 18, this.m_bAutoSeries, 1);
    provider.WriteBit(iOffset + 18, this.m_bAutoPosX, 2);
    provider.WriteBit(iOffset + 18, this.m_bAutoPosY, 3);
    provider.WriteBit(iOffset + 18, this.m_bIsVerticalLegend, 4);
    provider.WriteBit(iOffset + 18, this.m_bContainsDataTable, 5);
    this.m_iLength = 20;
  }

  public override int GetStoreSize(ExcelVersion version) => 20;
}
