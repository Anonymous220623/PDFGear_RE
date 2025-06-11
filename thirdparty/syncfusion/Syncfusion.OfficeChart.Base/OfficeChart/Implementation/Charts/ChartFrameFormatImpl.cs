// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartFrameFormatImpl
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

internal class ChartFrameFormatImpl : 
  CommonObject,
  IOfficeChartFrameFormat,
  IOfficeChartFillBorder,
  IFillColor
{
  private ChartFrameRecord m_chartFrame = (ChartFrameRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartFrame);
  private ChartBorderImpl m_border;
  private ThreeDFormatImpl m_3D;
  private ChartInteriorImpl m_interior;
  private ShadowImpl m_shadow;
  private ChartFillImpl m_fill;
  protected ChartImpl m_chart;
  private IOfficeChartLayout m_layout;
  protected ChartPlotAreaLayoutRecord m_plotAreaLayout;

  public ChartFrameFormatImpl(IApplication application, object parent)
    : this(application, parent, false, false, true)
  {
  }

  public ChartFrameFormatImpl(IApplication application, object parent, bool bSetDefaults)
    : this(application, parent, false, false, bSetDefaults)
  {
  }

  public ChartFrameFormatImpl(
    IApplication application,
    object parent,
    bool bAutoSize,
    bool bIsInteriorGrey,
    bool bSetDefaults)
    : base(application, parent)
  {
    this.SetParents();
    if (this.Workbook.IsWorkbookOpening || !bSetDefaults)
      return;
    this.SetDefaultValues(bAutoSize, bIsInteriorGrey);
  }

  public ChartFrameFormatImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : base(application, parent)
  {
    this.SetParents();
    this.Parse(data, ref iPos);
  }

  private void SetParents()
  {
    this.m_chart = this.FindParent(typeof (ChartImpl)) as ChartImpl;
    if (this.m_chart == null)
      throw new ArgumentNullException("Can't find parent chart");
  }

  [CLSCompliant(false)]
  public void Parse(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (iPos < 0 || iPos > data.Count)
      throw new ArgumentOutOfRangeException(nameof (iPos), "Value cannot be less than 0 and greater than data.Count");
    BiffRecordRaw biffRecordRaw = this.UnwrapRecord(data[iPos]);
    biffRecordRaw.CheckTypeCode(TBIFFRecord.ChartFrame);
    this.m_chartFrame = (ChartFrameRecord) biffRecordRaw;
    ++iPos;
    BiffRecordRaw record = data[iPos];
    int num = 0;
    if (!this.CheckBegin(record))
      return;
    int iBeginCounter = num + 1;
    do
    {
      ++iPos;
      this.ParseRecord(data[iPos], ref iBeginCounter);
    }
    while (iBeginCounter != 0);
    ++iPos;
  }

  [CLSCompliant(false)]
  protected virtual bool CheckBegin(BiffRecordRaw record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    return record.TypeCode == TBIFFRecord.Begin;
  }

  [CLSCompliant(false)]
  protected virtual void ParseRecord(BiffRecordRaw record, ref int iBeginCounter)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    switch (record.TypeCode)
    {
      case TBIFFRecord.PlotAreaLayout:
        if (this.m_plotAreaLayout == null)
          this.m_plotAreaLayout = (ChartPlotAreaLayoutRecord) BiffRecordFactory.GetRecord(TBIFFRecord.PlotAreaLayout);
        this.m_plotAreaLayout = (ChartPlotAreaLayoutRecord) record;
        break;
      case TBIFFRecord.ChartLineFormat:
        this.m_border = new ChartBorderImpl(this.Application, (object) this, (ChartLineFormatRecord) record);
        break;
      case TBIFFRecord.ChartAreaFormat:
        this.m_interior = new ChartInteriorImpl(this.Application, (object) this, (ChartAreaFormatRecord) record);
        break;
      case TBIFFRecord.Begin:
        ++iBeginCounter;
        break;
      case TBIFFRecord.End:
        --iBeginCounter;
        break;
      case TBIFFRecord.ChartGelFrame:
        this.m_fill = new ChartFillImpl(this.Application, (object) this, (ChartGelFrameRecord) record);
        break;
    }
  }

  [CLSCompliant(false)]
  public void Serialize(IList<IBiffStorage> records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    this.SerializeRecord(records, (BiffRecordRaw) this.m_chartFrame);
    this.SerializeRecord(records, BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
    if (this.m_border != null)
      this.m_border.Serialize(records);
    if (this.m_interior != null)
      this.m_interior.Serialize(records);
    if (this.m_fill != null)
      this.m_fill.Serialize(records);
    if (this.m_plotAreaLayout != null)
      this.SerializeRecord(records, (BiffRecordRaw) this.m_plotAreaLayout);
    this.SerializeRecord(records, BiffRecordFactory.GetRecord(TBIFFRecord.End));
  }

  [CLSCompliant(false)]
  protected virtual void SerializeRecord(IList<IBiffStorage> list, BiffRecordRaw record)
  {
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    list.Add((IBiffStorage) record.Clone());
  }

  [CLSCompliant(false)]
  protected virtual BiffRecordRaw UnwrapRecord(BiffRecordRaw record) => record;

  public void SetDefaultValues(bool bAutoSize, bool bIsInteriorGray)
  {
    this.m_chartFrame = (ChartFrameRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartFrame);
    this.m_chartFrame.AutoSize = bAutoSize;
    this.m_border = new ChartBorderImpl(this.Application, (object) this);
    this.m_border.ColorIndex = OfficeKnownColors.Grey_50_percent;
    this.m_border.AutoFormat = !this.m_chart.IsChart3D;
    this.m_interior = new ChartInteriorImpl(this.Application, (object) this);
    this.m_interior.InitForFrameFormat(bAutoSize, this.m_chart.IsChart3D, bIsInteriorGray);
  }

  [CLSCompliant(false)]
  public ChartFrameRecord FrameRecord => this.m_chartFrame;

  public WorkbookImpl Workbook => this.m_chart.InnerWorkbook;

  public IOfficeChartLayout Layout
  {
    get => this.m_layout;
    set => this.m_layout = value;
  }

  public bool HasInterior => this.m_interior != null;

  public bool HasLineProperties
  {
    get => this.m_border != null;
    internal set
    {
      if (value)
      {
        IOfficeChartBorder border = this.Border;
      }
      else
        this.m_border = (ChartBorderImpl) null;
    }
  }

  public IOfficeChartBorder Border
  {
    get
    {
      if (this.m_border == null)
        this.m_border = new ChartBorderImpl(this.Application, (object) this);
      return (IOfficeChartBorder) this.m_border;
    }
  }

  public IOfficeChartInterior Interior
  {
    get
    {
      if (this.m_interior == null)
        this.m_interior = new ChartInteriorImpl(this.Application, (object) this);
      return (IOfficeChartInterior) this.m_interior;
    }
  }

  public IThreeDFormat ThreeD
  {
    get
    {
      if (this.m_3D == null)
        this.m_3D = new ThreeDFormatImpl(this.Application, (object) this);
      return (IThreeDFormat) this.m_3D;
    }
  }

  public IOfficeFill Fill
  {
    get
    {
      if (this.m_fill == null)
        this.m_fill = new ChartFillImpl(this.Application, (object) this);
      this.IsAutomaticFormat = false;
      return (IOfficeFill) this.m_fill;
    }
  }

  public bool HasShadowProperties
  {
    get => this.m_shadow != null;
    internal set
    {
      if (value)
      {
        IShadow shadow = this.Shadow;
      }
      else
        this.m_shadow = (ShadowImpl) null;
    }
  }

  public bool Has3dProperties
  {
    get => this.m_3D != null;
    internal set
    {
      if (value)
      {
        IThreeDFormat threeD = this.ThreeD;
      }
      else
        this.m_3D = (ThreeDFormatImpl) null;
    }
  }

  public IShadow Shadow
  {
    get
    {
      if (this.m_shadow == null)
        this.m_shadow = new ShadowImpl(this.Application, (object) this);
      return (IShadow) this.m_shadow;
    }
  }

  internal OfficeRectangleStyle RectangleStyle
  {
    get => this.m_chartFrame.Rectangle;
    set => this.m_chartFrame.Rectangle = value;
  }

  public bool IsAutoSize
  {
    get => this.m_chartFrame.AutoSize;
    set => this.m_chartFrame.AutoSize = value;
  }

  public bool IsAutoPosition
  {
    get => this.m_chartFrame.AutoPosition;
    set => this.m_chartFrame.AutoPosition = value;
  }

  public bool IsBorderCornersRound
  {
    get => this.Interior.SwapColorsOnNegative;
    set => this.Interior.SwapColorsOnNegative = value;
  }

  public IOfficeChartBorder LineProperties => this.Border;

  public static OfficeKnownColors UpdateLineColor(OfficeKnownColors color)
  {
    int num = (int) color;
    return num < 8 ? (OfficeKnownColors) (num + 8) : color;
  }

  public void Clear() => this.SetDefaultValues(false, false);

  public ChartColor ForeGroundColorObject
  {
    get => (this.Interior as ChartInteriorImpl).ForegroundColorObject;
  }

  public ChartColor BackGroundColorObject
  {
    get => (this.Interior as ChartInteriorImpl).BackgroundColorObject;
  }

  public OfficePattern Pattern
  {
    get => this.Interior.Pattern;
    set => this.Interior.Pattern = value;
  }

  public bool IsAutomaticFormat
  {
    get => this.Interior.UseAutomaticFormat;
    set => this.Interior.UseAutomaticFormat = value;
  }

  public bool Visible
  {
    get => this.Interior.Pattern != OfficePattern.None;
    set
    {
      if (value)
      {
        if (this.Interior.Pattern != OfficePattern.None)
          return;
        this.Interior.Pattern = OfficePattern.Solid;
      }
      else
        this.Interior.Pattern = OfficePattern.None;
    }
  }

  public ChartFrameFormatImpl Clone(object parent)
  {
    ChartFrameFormatImpl parent1 = (ChartFrameFormatImpl) this.MemberwiseClone();
    parent1.SetParent(parent);
    parent1.SetParents();
    parent1.m_bIsDisposed = this.m_bIsDisposed;
    if (this.m_chartFrame != null)
      parent1.m_chartFrame = (ChartFrameRecord) this.m_chartFrame.Clone();
    if (this.m_border != null)
      parent1.m_border = this.m_border.Clone((object) parent1);
    if (this.m_interior != null)
    {
      int pattern = (int) this.m_interior.Pattern;
      parent1.Pattern = this.m_interior.Pattern;
      parent1.m_interior = this.m_interior.Clone((object) parent1);
    }
    if (this.m_fill != null)
      parent1.m_fill = (ChartFillImpl) this.m_fill.Clone((object) parent1);
    if (!this.m_chart.TypeChanging && !this.m_chart.IsParsed && this.m_fill != null)
    {
      if (parent1.m_fill.ForeColorObject != this.m_fill.ForeColorObject)
        parent1.m_fill.ForeColorObject.CopyFrom(this.m_fill.ForeColorObject, false);
      if (parent1.m_fill.BackColorObject != this.m_fill.BackColorObject)
        parent1.m_fill.BackColorObject.CopyFrom(this.m_fill.BackColorObject, false);
    }
    if (this.m_3D != null)
      parent1.m_3D = this.m_3D.Clone((object) parent1);
    if (this.m_shadow != null)
      parent1.m_shadow = this.m_shadow.Clone((object) parent1);
    if (this.m_layout != null)
      parent1.m_layout = this.m_layout;
    if (this.m_plotAreaLayout != null)
      parent1.m_plotAreaLayout = (ChartPlotAreaLayoutRecord) this.m_plotAreaLayout.Clone();
    return parent1;
  }
}
