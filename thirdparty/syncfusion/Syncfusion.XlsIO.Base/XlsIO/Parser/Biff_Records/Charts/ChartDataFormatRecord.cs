// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartDataFormatRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartDataFormat)]
public class ChartDataFormatRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 8;
  [BiffRecordPos(0, 2)]
  private ushort m_usPointNumber;
  [BiffRecordPos(2, 2)]
  private ushort m_usSeriesIndex;
  [BiffRecordPos(4, 2)]
  private ushort m_usSeriesNumber;
  [BiffRecordPos(6, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(6, 0, TFieldType.Bit)]
  private bool m_bUseXL4Color;

  public ushort PointNumber
  {
    get => this.m_usPointNumber;
    set
    {
      if ((int) value == (int) this.m_usPointNumber)
        return;
      this.m_usPointNumber = value;
    }
  }

  public ushort SeriesIndex
  {
    get => this.m_usSeriesIndex;
    set
    {
      if ((int) value == (int) this.m_usSeriesIndex)
        return;
      this.m_usSeriesIndex = value;
    }
  }

  public ushort SeriesNumber
  {
    get => this.m_usSeriesNumber;
    set
    {
      if ((int) value == (int) this.m_usSeriesNumber)
        return;
      this.m_usSeriesNumber = value;
    }
  }

  public ushort Options => this.m_usOptions;

  public bool UserExcel4Colors
  {
    get => this.m_bUseXL4Color;
    set => this.m_bUseXL4Color = value;
  }

  public override int MinimumRecordSize => 8;

  public override int MaximumRecordSize => 8;

  public ChartDataFormatRecord()
  {
  }

  public ChartDataFormatRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartDataFormatRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usPointNumber = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usSeriesIndex = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usSeriesNumber = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bUseXL4Color = provider.ReadBit(iOffset, 0);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_usOptions &= (ushort) 1;
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usPointNumber);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usSeriesIndex);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usSeriesNumber);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bUseXL4Color, 0);
  }

  public override int GetStoreSize(ExcelVersion version) => 8;
}
