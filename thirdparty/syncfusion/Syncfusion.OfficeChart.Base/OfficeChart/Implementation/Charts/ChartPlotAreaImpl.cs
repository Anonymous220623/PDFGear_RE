// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartPlotAreaImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartPlotAreaImpl : 
  ChartFrameFormatImpl,
  IOfficeChartFrameFormat,
  IOfficeChartFillBorder
{
  private ChartPlotAreaRecord m_plotArea;
  private IOfficeChartLayout m_layout;

  public new IOfficeChartLayout Layout
  {
    get
    {
      if (this.m_layout == null)
        this.m_layout = (IOfficeChartLayout) new ChartLayoutImpl(this.Application, (object) this, this.Parent);
      return this.m_layout;
    }
    set => this.m_layout = value;
  }

  public ChartPlotAreaLayoutRecord PlotAreaLayout
  {
    get
    {
      if (this.m_plotAreaLayout == null)
        this.m_plotAreaLayout = (ChartPlotAreaLayoutRecord) BiffRecordFactory.GetRecord(TBIFFRecord.PlotAreaLayout);
      return this.m_plotAreaLayout;
    }
  }

  public ChartPlotAreaImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_plotArea = (ChartPlotAreaRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartPlotArea);
    this.Border.LinePattern = OfficeChartLinePattern.None;
  }

  public ChartPlotAreaImpl(IApplication application, object parent, OfficeChartType type)
    : this(application, parent)
  {
    if (Array.IndexOf<OfficeChartType>(ChartImpl.DEF_WALLS_OR_FLOOR_TYPES, type) == -1 && Array.IndexOf<OfficeChartType>(ChartImpl.DEF_DONT_NEED_PLOT, type) == -1 && this.Workbook.Version == OfficeVersion.Excel97to2003)
      this.Interior.ForegroundColorIndex = OfficeKnownColors.Grey_25_percent;
    else
      this.Interior.ForegroundColorIndex = OfficeKnownColors.WhiteCustom;
  }

  public ChartPlotAreaImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : base(application, parent, false)
  {
    this.Parse(data, ref iPos);
  }

  public new void Parse(IList<BiffRecordRaw> data, ref int iPos)
  {
    BiffRecordRaw biffRecordRaw = data != null ? data[iPos] : throw new ArgumentNullException(nameof (data));
    biffRecordRaw.CheckTypeCode(TBIFFRecord.ChartPlotArea);
    this.m_plotArea = (ChartPlotAreaRecord) biffRecordRaw;
    ++iPos;
    if (data[iPos].TypeCode == TBIFFRecord.ChartFrame)
      base.Parse(data, ref iPos);
    --iPos;
  }

  [CLSCompliant(false)]
  public new void Serialize(IList<IBiffStorage> records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_plotArea != null)
      records.Add((IBiffStorage) this.m_plotArea.Clone());
    base.Serialize(records);
    if (this.m_plotAreaLayout == null)
      return;
    this.SerializeRecord(records, (BiffRecordRaw) this.m_plotAreaLayout);
  }
}
