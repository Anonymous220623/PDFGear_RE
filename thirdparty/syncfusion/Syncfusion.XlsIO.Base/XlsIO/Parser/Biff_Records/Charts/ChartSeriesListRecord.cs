// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartSeriesListRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Exceptions;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartSeriesList)]
[CLSCompliant(false)]
public class ChartSeriesListRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usCount;
  private ushort[] m_arrSeries;

  public ushort SeriesCount => this.m_usCount;

  public ushort[] Series
  {
    get => this.m_arrSeries;
    set
    {
      this.m_arrSeries = value;
      this.m_usCount = value != null ? (ushort) value.Length : (ushort) 0;
    }
  }

  public override int MinimumRecordSize => 2;

  public ChartSeriesListRecord()
  {
  }

  public ChartSeriesListRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartSeriesListRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usCount = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_arrSeries = (int) this.m_usCount * 2 + 2 == this.m_iLength ? new ushort[(int) this.m_usCount] : throw new WrongBiffRecordDataException("ChartListRecord");
    int index = 0;
    while (index < (int) this.m_usCount)
    {
      this.m_arrSeries[index] = provider.ReadUInt16(iOffset);
      ++index;
      iOffset += 2;
    }
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usCount);
    this.m_iLength = 2;
    int index = 0;
    while (index < (int) this.m_usCount)
    {
      provider.WriteUInt16(iOffset + this.m_iLength, this.m_arrSeries[index]);
      ++index;
      this.m_iLength += 2;
    }
  }

  public override int GetStoreSize(ExcelVersion version) => 2 + 2 * (int) this.m_usCount;

  public static bool operator ==(ChartSeriesListRecord record1, ChartSeriesListRecord record2)
  {
    bool flag1 = object.Equals((object) record1, (object) null);
    bool flag2 = object.Equals((object) record2, (object) null);
    if (flag1 && flag2)
      return true;
    if (flag1 || flag2)
      return false;
    bool flag3 = (int) record1.m_usCount == (int) record2.m_usCount;
    int index = 0;
    for (int usCount = (int) record1.m_usCount; index < usCount && flag3; ++index)
      flag3 = (int) record1.m_arrSeries[index] == (int) record2.m_arrSeries[index];
    return flag3;
  }

  public static bool operator !=(ChartSeriesListRecord record1, ChartSeriesListRecord record2)
  {
    return !(record1 == record2);
  }

  public override object Clone()
  {
    ChartSeriesListRecord seriesListRecord = (ChartSeriesListRecord) base.Clone();
    seriesListRecord.m_arrSeries = CloneUtils.CloneUshortArray(this.m_arrSeries);
    return (object) seriesListRecord;
  }
}
