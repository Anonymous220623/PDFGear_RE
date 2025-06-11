// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartBorderImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartBorderImpl : CommonObject, IOfficeChartBorder, ICloneParent
{
  private const OfficeKnownColors DEF_COLOR_INEDX = OfficeKnownColors.YellowCustom | OfficeKnownColors.BlackCustom;
  private ChartLineFormatRecord m_lineFormat = (ChartLineFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartLineFormat);
  private WorkbookImpl m_parentBook;
  private ChartSerieDataFormatImpl m_serieFormat;
  private ChartColor m_color;
  private double m_solidTransparency;
  private IInternalFill m_fill;
  private Excel2007BorderJoinType m_joinType;
  private string m_lineWeightString;
  private bool m_lineProperties;

  public ChartBorderImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_lineFormat = (ChartLineFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartLineFormat);
    this.Fill = (IInternalFill) new ShapeFillImpl(application, parent);
    this.SetParents();
  }

  [CLSCompliant(false)]
  public ChartBorderImpl(IApplication application, object parent, ChartLineFormatRecord line)
    : base(application, parent)
  {
    this.m_lineFormat = line != null ? line : throw new ArgumentNullException(nameof (line));
    this.SetParents();
  }

  public ChartBorderImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : base(application, parent)
  {
    this.Parse(data, ref iPos);
    this.SetParents();
  }

  public void Parse(IList<BiffRecordRaw> data, ref int iPos)
  {
    BiffRecordRaw biffRecordRaw = data != null ? data[iPos] : throw new ArgumentNullException(nameof (data));
    biffRecordRaw.CheckTypeCode(TBIFFRecord.ChartLineFormat);
    this.m_lineFormat = (ChartLineFormatRecord) biffRecordRaw;
    ++iPos;
  }

  public void Serialize(IList<IBiffStorage> records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_lineFormat == null)
      return;
    this.UpdateColor();
    records.Add((IBiffStorage) this.m_lineFormat.Clone());
  }

  private void SetParents()
  {
    this.m_parentBook = (WorkbookImpl) this.FindParent(typeof (WorkbookImpl));
    this.m_serieFormat = (ChartSerieDataFormatImpl) this.FindParent(typeof (ChartSerieDataFormatImpl));
    if (this.m_parentBook == null)
      throw new ApplicationException("cannot find parent objects.");
    this.m_color = new ChartColor((OfficeKnownColors) this.m_lineFormat.ColorIndex);
    this.m_color.AfterChange += new ChartColor.AfterChangeHandler(this.UpdateColor);
  }

  internal void UpdateColor()
  {
    this.m_lineFormat.ColorIndex = (ushort) this.m_color.GetIndexed((IWorkbook) this.m_parentBook);
  }

  public System.Drawing.Color LineColor
  {
    get => this.m_color.GetRGB((IWorkbook) this.m_parentBook);
    set
    {
      if (this.m_color.ColorType == ColorType.RGB && value.ToArgb() == this.m_color.Value && !this.AutoFormat)
        return;
      this.AutoFormat = false;
      this.m_color.SetRGB(value, (IWorkbook) this.m_parentBook);
      this.m_lineFormat.IsAutoLineColor = false;
      this.HasLineProperties = true;
      if (this.m_serieFormat == null)
        return;
      this.m_serieFormat.ClearOnPropertyChange();
    }
  }

  public OfficeChartLinePattern LinePattern
  {
    get => this.m_lineFormat.LinePattern;
    set
    {
      if (value == this.LinePattern && !this.AutoFormat)
        return;
      this.m_lineFormat.LinePattern = value;
      this.AutoFormat = false;
      if (this.m_serieFormat != null)
        this.m_serieFormat.ClearOnPropertyChange();
      this.HasLineProperties = true;
      if (!(this.Parent is ChartWallOrFloorImpl))
        return;
      (this.Parent as ChartWallOrFloorImpl).HasShapeProperties = true;
    }
  }

  public OfficeChartLineWeight LineWeight
  {
    get => this.m_lineFormat.LineWeight;
    set
    {
      if (value == this.LineWeight && !this.AutoFormat)
        return;
      this.m_lineFormat.LineWeight = value;
      this.AutoFormat = false;
      if (!this.m_parentBook.IsWorkbookOpening)
        this.LineWeightString = (short) value != (short) -1 ? (((int) (short) value + 1) * 12700).ToString() : "3175";
      if (this.m_serieFormat == null)
        return;
      this.m_serieFormat.ClearOnPropertyChange();
    }
  }

  internal IInternalFill Fill
  {
    get => this.m_fill;
    set => this.m_fill = value;
  }

  internal bool HasGradientFill
  {
    get => this.m_fill != null && this.m_fill.FillType == OfficeFillType.Gradient;
  }

  internal bool HasLineProperties
  {
    get => this.m_lineProperties;
    set => this.m_lineProperties = value;
  }

  internal Excel2007BorderJoinType JoinType
  {
    get => this.m_joinType;
    set => this.m_joinType = value;
  }

  public bool AutoFormat
  {
    get => this.m_lineFormat.AutoFormat;
    set
    {
      if (this.AutoFormat == value)
        return;
      this.m_lineFormat.AutoFormat = value;
      if (value)
      {
        this.m_lineFormat.LineWeight = OfficeChartLineWeight.Hairline;
        this.m_lineFormat.LinePattern = OfficeChartLinePattern.Solid;
        this.IsAutoLineColor = true;
      }
      else
        this.TryAndClearAutoColor();
      if (this.m_serieFormat == null || this.m_serieFormat.ParentChart.TypeChanging)
        return;
      this.m_serieFormat.ClearOnPropertyChange();
    }
  }

  internal bool DrawTickLabels
  {
    get => this.m_lineFormat.DrawTickLabels;
    set => this.m_lineFormat.DrawTickLabels = value;
  }

  public bool IsAutoLineColor
  {
    get => this.m_lineFormat.IsAutoLineColor;
    set
    {
      this.m_lineFormat.IsAutoLineColor = value;
      if (value)
        this.m_lineFormat.ColorIndex = (ushort) 77;
      if (this.m_serieFormat == null)
        return;
      this.m_serieFormat.ClearOnPropertyChange();
    }
  }

  public OfficeKnownColors ColorIndex
  {
    get => this.m_color.GetIndexed((IWorkbook) this.m_parentBook);
    set
    {
      if (this.m_color.ColorType == ColorType.Indexed && this.ColorIndex == value && !this.AutoFormat)
        return;
      value = ChartFrameFormatImpl.UpdateLineColor(value);
      this.AutoFormat = false;
      this.m_color.SetIndexed(value);
      this.m_lineFormat.IsAutoLineColor = false;
      if (this.m_serieFormat == null)
        return;
      this.m_serieFormat.ClearOnPropertyChange();
    }
  }

  public ChartColor Color => this.m_color;

  public double Transparency
  {
    get => this.m_solidTransparency;
    set
    {
      if (value < 0.0 || value > 1.0)
        throw new ArgumentOutOfRangeException("Transparency is out of range");
      if (value != this.m_solidTransparency && (this.AutoFormat || this.IsAutoLineColor))
      {
        this.AutoFormat = false;
        this.IsAutoLineColor = false;
      }
      this.m_solidTransparency = value;
    }
  }

  internal string LineWeightString
  {
    get => this.m_lineWeightString;
    set => this.m_lineWeightString = value;
  }

  internal void TryAndClearAutoColor()
  {
    if (this.m_serieFormat == null || this.m_serieFormat.ParentChart.TypeChanging)
      return;
    int num = this.m_serieFormat.UpdateLineColor();
    if (num == -1)
      return;
    this.m_lineFormat.ColorIndex = (ushort) num;
    this.m_lineFormat.IsAutoLineColor = false;
  }

  public ChartBorderImpl Clone(object parent)
  {
    ChartBorderImpl chartBorderImpl = parent != null ? new ChartBorderImpl(this.Application, parent) : throw new ArgumentNullException(nameof (parent));
    chartBorderImpl.m_lineFormat = (ChartLineFormatRecord) CloneUtils.CloneCloneable((ICloneable) this.m_lineFormat);
    chartBorderImpl.m_lineProperties = this.m_lineProperties;
    chartBorderImpl.JoinType = this.JoinType;
    if (this.m_fill != null)
      chartBorderImpl.m_fill = this.m_fill;
    chartBorderImpl.SetParent(parent);
    chartBorderImpl.SetParents();
    chartBorderImpl.m_color = this.m_color.Clone();
    chartBorderImpl.m_solidTransparency = this.m_solidTransparency;
    return chartBorderImpl;
  }

  internal void ClearAutoColor() => this.IsAutoLineColor = false;

  object ICloneParent.Clone(object parent) => (object) this.Clone(parent);
}
