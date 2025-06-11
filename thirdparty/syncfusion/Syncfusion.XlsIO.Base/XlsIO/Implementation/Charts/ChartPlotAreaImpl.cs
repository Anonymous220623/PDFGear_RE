// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartPlotAreaImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartPlotAreaImpl : ChartFrameFormatImpl, IChartFrameFormat, IChartFillBorder
{
  private ChartPlotAreaRecord m_plotArea;
  private IChartLayout m_layout;

  public new IChartLayout Layout
  {
    get
    {
      if (this.m_layout == null)
      {
        this.m_layout = (IChartLayout) new ChartLayoutImpl(this.Application, (object) this, this.Parent);
        (this.m_layout.ManualLayout as ChartManualLayoutImpl).PlotAreaLayout = this.PlotAreaLayout;
      }
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
    this.Border.LinePattern = ExcelChartLinePattern.None;
  }

  public ChartPlotAreaImpl(IApplication application, object parent, ExcelChartType type)
    : this(application, parent)
  {
    if (Array.IndexOf<ExcelChartType>(ChartImpl.DEF_WALLS_OR_FLOOR_TYPES, type) == -1 && Array.IndexOf<ExcelChartType>(ChartImpl.DEF_DONT_NEED_PLOT, type) == -1 && this.Workbook.Version == ExcelVersion.Excel97to2003)
      this.Interior.ForegroundColorIndex = ExcelKnownColors.Grey_25_percent;
    else
      this.Interior.ForegroundColorIndex = ExcelKnownColors.WhiteCustom;
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
  }
}
