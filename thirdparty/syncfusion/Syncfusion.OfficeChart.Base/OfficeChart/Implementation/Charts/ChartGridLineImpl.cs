// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartGridLineImpl
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

internal class ChartGridLineImpl : CommonObject, IOfficeChartGridLine, IOfficeChartFillBorder
{
  private const OfficeKnownColors DEF_COLOR_INEDX = OfficeKnownColors.YellowCustom | OfficeKnownColors.BlackCustom;
  private ChartAxisLineFormatRecord m_axisLine;
  private ChartAxisImpl m_parentAxis;
  private ShadowImpl m_shadow;
  protected WorkbookImpl m_parentBook;
  private ChartBorderImpl m_border;
  private ThreeDFormatImpl m_3D;

  public ChartGridLineImpl(
    IApplication application,
    object parent,
    ExcelAxisLineIdentifier axisType)
    : base(application, parent)
  {
    if (axisType != ExcelAxisLineIdentifier.MajorGridLine && axisType != ExcelAxisLineIdentifier.MinorGridLine)
      throw new ArgumentException(nameof (axisType));
    this.m_axisLine = (ChartAxisLineFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAxisLineFormat);
    this.m_border = new ChartBorderImpl(application, (object) this);
    this.m_border.ColorIndex = OfficeKnownColors.YellowCustom | OfficeKnownColors.BlackCustom;
    this.m_border.LineWeight = OfficeChartLineWeight.Hairline;
    this.AxisLineType = axisType;
    this.m_border.AutoFormat = true;
    this.SetParents();
  }

  public ChartGridLineImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : base(application, parent)
  {
    this.Parse(data, ref iPos);
    this.SetParents();
  }

  private void SetParents()
  {
    this.m_parentAxis = (ChartAxisImpl) CommonObject.FindParent(this.Parent, typeof (ChartAxisImpl), true);
    this.m_parentBook = (WorkbookImpl) this.FindParent(typeof (WorkbookImpl));
    if (this.m_parentBook == null)
      throw new ApplicationException("Can't find parent objects");
  }

  [CLSCompliant(false)]
  public virtual void Parse(IList<BiffRecordRaw> data, ref int iPos)
  {
    BiffRecordRaw biffRecordRaw = data != null ? data[iPos] : throw new ArgumentNullException(nameof (data));
    biffRecordRaw.CheckTypeCode(TBIFFRecord.ChartAxisLineFormat);
    this.m_axisLine = (ChartAxisLineFormatRecord) biffRecordRaw;
    ++iPos;
    if (data[iPos].TypeCode != TBIFFRecord.ChartLineFormat)
      return;
    this.m_border = new ChartBorderImpl(this.Application, (object) this, data, ref iPos);
  }

  [CLSCompliant(false)]
  public virtual void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_axisLine == null)
      return;
    records.Add((IBiffStorage) this.m_axisLine.Clone());
    this.m_border.Serialize((IList<IBiffStorage>) records);
  }

  public IOfficeChartBorder Border
  {
    get
    {
      if (this.m_border == null)
        this.m_border = new ChartBorderImpl(this.Application, (object) this);
      this.m_border.HasLineProperties = true;
      return (IOfficeChartBorder) this.m_border;
    }
  }

  public IOfficeChartBorder LineProperties => this.Border;

  public bool HasLineProperties => this.m_border != null;

  public IShadow Shadow
  {
    get
    {
      if (this.m_shadow == null)
        this.m_shadow = new ShadowImpl(this.Application, (object) this);
      return (IShadow) this.m_shadow;
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

  public IThreeDFormat ThreeD
  {
    get
    {
      if (this.m_3D == null)
        this.m_3D = new ThreeDFormatImpl(this.Application, (object) this);
      return (IThreeDFormat) this.m_3D;
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

  public bool HasInterior => false;

  public IOfficeChartInterior Interior => throw new NotSupportedException();

  public IOfficeFill Fill => throw new NotSupportedException();

  public virtual void Delete()
  {
    if (this.m_axisLine.LineIdentifier == ExcelAxisLineIdentifier.MajorGridLine)
      this.m_parentAxis.HasMajorGridLines = false;
    else
      this.m_parentAxis.HasMinorGridLines = false;
  }

  public ExcelAxisLineIdentifier AxisLineType
  {
    get => this.m_axisLine.LineIdentifier;
    set => this.m_axisLine.LineIdentifier = value;
  }

  protected ChartAxisImpl ParentAxis => this.m_parentAxis;

  public virtual object Clone(object parent)
  {
    ChartGridLineImpl parent1 = (ChartGridLineImpl) this.MemberwiseClone();
    parent1.SetParent(parent);
    parent1.SetParents();
    parent1.m_axisLine = (ChartAxisLineFormatRecord) CloneUtils.CloneCloneable((ICloneable) this.m_axisLine);
    if (this.m_border != null)
      parent1.m_border = this.m_border.Clone((object) parent1);
    if (this.m_shadow != null)
      parent1.m_shadow = this.m_shadow.Clone((object) parent1);
    if (this.m_3D != null)
      parent1.m_3D = this.m_3D.Clone((object) parent1);
    return (object) parent1;
  }
}
