// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartDataTableImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartDataTableImpl : CommonObject, IOfficeChartDataTable
{
  private ChartDatRecord m_chartDat;
  private List<BiffRecordRaw> m_arrRecords = new List<BiffRecordRaw>();
  private ChartTextAreaImpl m_text;
  internal bool HasShapeProperties;
  internal MemoryStream shapeStream;

  public ChartDataTableImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_chartDat = (ChartDatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartDat);
    this.m_chartDat.HasBorders = true;
    this.m_chartDat.HasHorizontalBorders = true;
    this.m_chartDat.HasVerticalBorders = true;
  }

  [CLSCompliant(false)]
  public ChartDataTableImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : base(application, parent)
  {
    this.Parse(data, ref iPos);
  }

  private void Parse(IList<BiffRecordRaw> data, ref int iPos)
  {
    BiffRecordRaw biffRecordRaw1 = data != null ? data[iPos] : throw new ArgumentNullException(nameof (data));
    biffRecordRaw1.CheckTypeCode(TBIFFRecord.ChartDat);
    this.m_chartDat = (ChartDatRecord) biffRecordRaw1;
    ++iPos;
    BiffRecordRaw biffRecordRaw2 = data[iPos];
    biffRecordRaw2.CheckTypeCode(TBIFFRecord.Begin);
    this.m_arrRecords.Add(biffRecordRaw2);
    ++iPos;
    int num = 1;
    while (num != 0)
    {
      BiffRecordRaw biffRecordRaw3 = data[iPos];
      if (biffRecordRaw3.TypeCode == TBIFFRecord.End)
        --num;
      else if (biffRecordRaw3.TypeCode == TBIFFRecord.Begin)
        ++num;
      this.m_arrRecords.Add(biffRecordRaw3);
      ++iPos;
    }
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    records.Add((IBiffStorage) this.m_chartDat);
    if (this.m_arrRecords.Count == 0)
    {
      this.m_arrRecords.Add(BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
      ChartLegendRecord record1 = (ChartLegendRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartLegend);
      record1.IsVerticalLegend = true;
      record1.ContainsDataTable = true;
      record1.Position = OfficeLegendPosition.NotDocked;
      record1.Spacing = ExcelLegendSpacing.Medium;
      this.m_arrRecords.Add((BiffRecordRaw) record1);
      this.m_arrRecords.Add(BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
      ChartPosRecord record2 = (ChartPosRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartPos);
      record2.TopLeft = (ushort) 3;
      this.m_arrRecords.Add((BiffRecordRaw) record2);
      ChartTextRecord record3 = (ChartTextRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartText);
      record3.Options2 = (ushort) 10816;
      this.m_arrRecords.Add((BiffRecordRaw) record3);
      this.m_arrRecords.Add(BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
      ChartPosRecord record4 = (ChartPosRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartPos);
      record4.TopLeft = (ushort) 2;
      record4.BottomRight = (ushort) 2;
      this.m_arrRecords.Add((BiffRecordRaw) record4);
      this.m_arrRecords.Add(BiffRecordFactory.GetRecord(TBIFFRecord.ChartFontx));
      this.m_arrRecords.Add(BiffRecordFactory.GetRecord(TBIFFRecord.ChartAI));
      this.m_arrRecords.Add(BiffRecordFactory.GetRecord(TBIFFRecord.End));
      this.m_arrRecords.Add(BiffRecordFactory.GetRecord(TBIFFRecord.End));
      this.m_arrRecords.Add(BiffRecordFactory.GetRecord(TBIFFRecord.End));
    }
    records.AddList((IList) this.m_arrRecords);
  }

  public ChartDataTableImpl Clone(object parent)
  {
    ChartDataTableImpl parent1 = new ChartDataTableImpl(this.Application, parent);
    parent1.m_bIsDisposed = this.m_bIsDisposed;
    if (this.m_arrRecords != null)
    {
      List<BiffRecordRaw> biffRecordRawList = new List<BiffRecordRaw>();
      int index = 0;
      for (int count = this.m_arrRecords.Count; index < count; ++index)
      {
        BiffRecordRaw biffRecordRaw = (BiffRecordRaw) this.m_arrRecords[index].Clone();
        biffRecordRawList.Add(biffRecordRaw);
      }
      parent1.m_arrRecords = biffRecordRawList;
    }
    if (this.m_chartDat != null)
      parent1.m_chartDat = (ChartDatRecord) this.m_chartDat.Clone();
    if (this.m_text != null)
      parent1.m_text = (ChartTextAreaImpl) this.m_text.Clone((object) parent1);
    if (this.shapeStream != null)
    {
      this.shapeStream.Position = 0L;
      parent1.shapeStream = (MemoryStream) CloneUtils.CloneStream((Stream) this.shapeStream);
    }
    return parent1;
  }

  public bool HasHorzBorder
  {
    get => this.m_chartDat.HasHorizontalBorders;
    set => this.m_chartDat.HasHorizontalBorders = value;
  }

  public bool HasVertBorder
  {
    get => this.m_chartDat.HasVerticalBorders;
    set => this.m_chartDat.HasVerticalBorders = value;
  }

  public bool HasBorders
  {
    get => this.m_chartDat.HasBorders;
    set => this.m_chartDat.HasBorders = value;
  }

  public bool ShowSeriesKeys
  {
    get => this.m_chartDat.ShowSeriesKeys;
    set => this.m_chartDat.ShowSeriesKeys = value;
  }

  public IOfficeChartTextArea TextArea
  {
    get
    {
      if (this.m_text == null)
      {
        this.m_text = new ChartTextAreaImpl(this.Application, (object) this);
        this.m_text.FontName = "Calibri";
      }
      else
        ChartParserCommon.CheckDefaultSettings(this.m_text);
      return (IOfficeChartTextArea) this.m_text;
    }
  }
}
