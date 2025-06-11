// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartChartLineRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartChartLine)]
internal class ChartChartLineRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usDropLines;
  private bool m_hasDropLine;
  private bool m_hasHighLowLine;
  private bool m_hasSeriesLine;

  public ExcelDropLineStyle LineStyle
  {
    get => (ExcelDropLineStyle) this.m_usDropLines;
    set
    {
      this.m_usDropLines = (ushort) value;
      if (this.m_usDropLines == (ushort) 0)
        this.m_hasDropLine = true;
      else if (this.m_usDropLines == (ushort) 1)
      {
        this.m_hasHighLowLine = true;
      }
      else
      {
        if (this.m_usDropLines != (ushort) 2)
          return;
        this.m_hasSeriesLine = true;
      }
    }
  }

  public bool HasDropLine
  {
    get => this.m_hasDropLine;
    set
    {
      this.m_hasDropLine = value;
      if (!value)
        return;
      this.m_usDropLines = (ushort) 0;
    }
  }

  public bool HasHighLowLine
  {
    get => this.m_hasHighLowLine;
    set
    {
      this.m_hasHighLowLine = value;
      if (!value)
        return;
      this.m_usDropLines = (ushort) 1;
    }
  }

  public bool HasSeriesLine
  {
    get => this.m_hasSeriesLine;
    set
    {
      this.m_hasSeriesLine = value;
      if (!value)
        return;
      this.m_usDropLines = (ushort) 2;
    }
  }

  public ChartChartLineRecord()
  {
  }

  public ChartChartLineRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartChartLineRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usDropLines = provider.ReadUInt16(iOffset);
    if (this.m_usDropLines == (ushort) 0)
      this.HasDropLine = true;
    else if (this.m_usDropLines == (ushort) 1)
    {
      this.HasHighLowLine = true;
    }
    else
    {
      if (this.m_usDropLines != (ushort) 2)
        return;
      this.HasSeriesLine = true;
    }
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usDropLines);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(OfficeVersion version) => 2;

  public static bool operator ==(ChartChartLineRecord record1, ChartChartLineRecord record2)
  {
    bool flag1 = object.Equals((object) record1, (object) null);
    bool flag2 = object.Equals((object) record2, (object) null);
    if (flag1 && flag2)
      return true;
    return !flag1 && !flag2 && (int) record1.m_usDropLines == (int) record2.m_usDropLines;
  }

  public static bool operator !=(ChartChartLineRecord record1, ChartChartLineRecord record2)
  {
    return !(record1 == record2);
  }
}
