// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartAreaFormatRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartAreaFormat)]
[CLSCompliant(false)]
public class ChartAreaFormatRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 16 /*0x10*/;
  [BiffRecordPos(0, 4, true)]
  private int m_iForeground;
  [BiffRecordPos(4, 4, true)]
  private int m_iBackground;
  [BiffRecordPos(8, 2)]
  private ushort m_usPattern;
  [BiffRecordPos(10, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(10, 0, TFieldType.Bit)]
  private bool m_bAutomaticFormat = true;
  [BiffRecordPos(10, 1, TFieldType.Bit)]
  private bool m_bSwapColorsOnNegative;
  [BiffRecordPos(12, 2)]
  private ushort m_usForegroundIndex;
  [BiffRecordPos(14, 2)]
  private ushort m_usBackgroundIndex;

  public int ForegroundColor
  {
    get => this.m_iForeground;
    set
    {
      if (value == this.m_iForeground)
        return;
      this.m_iForeground = value;
    }
  }

  public Color BackgroundColor
  {
    get => ColorExtension.FromArgb(this.m_iBackground);
    set
    {
      int num = value.ToArgb() & 16777215 /*0xFFFFFF*/;
      if (num == this.m_iBackground)
        return;
      this.m_iBackground = num;
    }
  }

  public ExcelPattern Pattern
  {
    get => (ExcelPattern) this.m_usPattern;
    set => this.m_usPattern = (ushort) value;
  }

  public ushort Options => this.m_usOptions;

  public ExcelKnownColors ForegroundColorIndex
  {
    get => (ExcelKnownColors) this.m_usForegroundIndex;
    set
    {
      ushort num = (ushort) value;
      if ((int) num == (int) this.m_usForegroundIndex)
        return;
      this.m_usForegroundIndex = num;
    }
  }

  public ExcelKnownColors BackgroundColorIndex
  {
    get => (ExcelKnownColors) this.m_usBackgroundIndex;
    set
    {
      ushort num = (ushort) value;
      if ((int) num == (int) this.m_usBackgroundIndex)
        return;
      this.m_usBackgroundIndex = num;
    }
  }

  public bool UseAutomaticFormat
  {
    get => this.m_bAutomaticFormat;
    set => this.m_bAutomaticFormat = value;
  }

  public bool SwapColorsOnNegative
  {
    get => this.m_bSwapColorsOnNegative;
    set => this.m_bSwapColorsOnNegative = value;
  }

  public override int MinimumRecordSize => 16 /*0x10*/;

  public override int MaximumRecordSize => 16 /*0x10*/;

  public ChartAreaFormatRecord()
  {
  }

  public ChartAreaFormatRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartAreaFormatRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override int GetStoreSize(ExcelVersion version) => 16 /*0x10*/;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_iForeground = this.ReadColor(provider, ref iOffset);
    this.m_iBackground = this.ReadColor(provider, ref iOffset);
    this.m_usPattern = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bAutomaticFormat = provider.ReadBit(iOffset, 0);
    this.m_bSwapColorsOnNegative = provider.ReadBit(iOffset, 1);
    iOffset += 2;
    this.m_usForegroundIndex = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usBackgroundIndex = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_usOptions &= (ushort) 3;
    this.m_iLength = this.GetStoreSize(version);
    this.WriteColor(provider, ref iOffset, this.m_iForeground);
    this.WriteColor(provider, ref iOffset, this.m_iBackground);
    provider.WriteUInt16(iOffset, this.m_usPattern);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bAutomaticFormat, 0);
    provider.WriteBit(iOffset, this.m_bSwapColorsOnNegative, 1);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usForegroundIndex);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usBackgroundIndex);
  }

  private int ReadColor(DataProvider provider, ref int iOffset)
  {
    byte red = provider.ReadByte(iOffset++);
    byte green = provider.ReadByte(iOffset++);
    byte blue = provider.ReadByte(iOffset++);
    ++iOffset;
    return Color.FromArgb((int) byte.MaxValue, (int) red, (int) green, (int) blue).ToArgb();
  }

  private void WriteColor(DataProvider provider, ref int iOffset, int iColor)
  {
    Color color = ColorExtension.FromArgb(iColor);
    provider.WriteByte(iOffset++, color.R);
    provider.WriteByte(iOffset++, color.G);
    provider.WriteByte(iOffset++, color.B);
    provider.WriteByte(iOffset++, (byte) 0);
  }
}
