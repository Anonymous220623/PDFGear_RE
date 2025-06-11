// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartDropBarImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartDropBarImpl : 
  CommonObject,
  IOfficeChartDropBar,
  IOfficeChartFillBorder,
  IFillColor
{
  private ChartDropBarRecord m_dropBar;
  private ChartBorderImpl m_lineFormat;
  private ChartInteriorImpl m_interior;
  private WorkbookImpl m_parentBook;
  private ChartFillImpl m_fill;
  private ThreeDFormatImpl m_3D;
  private ShadowImpl m_shadow;

  public ChartDropBarImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    this.m_dropBar = (ChartDropBarRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartDropBar);
  }

  private void SetParents()
  {
    this.m_parentBook = (WorkbookImpl) this.FindParent(typeof (WorkbookImpl));
    if (this.m_parentBook == null)
      throw new ArgumentNullException("Cannot find parent object.");
  }

  [CLSCompliant(false)]
  public void Parse(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    data[iPos].CheckTypeCode(TBIFFRecord.ChartDropBar);
    this.m_dropBar = (ChartDropBarRecord) data[iPos];
    data[iPos + 1].CheckTypeCode(TBIFFRecord.Begin);
    iPos += 2;
    int num = 1;
    while (num > 0)
    {
      BiffRecordRaw biffRecordRaw = data[iPos];
      switch (biffRecordRaw.TypeCode)
      {
        case TBIFFRecord.ChartLineFormat:
          this.m_lineFormat = new ChartBorderImpl(this.Application, (object) this, (ChartLineFormatRecord) biffRecordRaw);
          break;
        case TBIFFRecord.ChartAreaFormat:
          this.m_interior = new ChartInteriorImpl(this.Application, (object) this, (ChartAreaFormatRecord) biffRecordRaw);
          break;
        case TBIFFRecord.Begin:
          ++num;
          iPos = BiffRecordRaw.SkipBeginEndBlock(data, iPos);
          break;
        case TBIFFRecord.End:
          --num;
          break;
      }
      ++iPos;
    }
    --iPos;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentException(nameof (records));
    if (this.m_dropBar == null)
      throw new ApplicationException("Exception occured inside of ChartDropBar object.");
    records.Add((IBiffStorage) this.m_dropBar.Clone());
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
    if (this.m_lineFormat != null)
      this.m_lineFormat.Serialize((IList<IBiffStorage>) records);
    if (this.m_interior != null && !this.m_interior.UseAutomaticFormat)
      this.m_interior.Serialize((IList<IBiffStorage>) records);
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.End));
  }

  public bool HasInterior => this.m_interior != null;

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

  public bool HasLineProperties => this.m_lineFormat != null;

  public int Gap
  {
    get => (int) this.m_dropBar.Gap;
    set => this.m_dropBar.Gap = (ushort) value;
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

  public IOfficeChartBorder LineProperties
  {
    get
    {
      if (this.m_lineFormat == null)
        this.m_lineFormat = new ChartBorderImpl(this.Application, (object) this);
      return (IOfficeChartBorder) this.m_lineFormat;
    }
  }

  public IOfficeFill Fill
  {
    get
    {
      if (this.m_fill == null)
        this.m_fill = new ChartFillImpl(this.Application, (object) this);
      return (IOfficeFill) this.m_fill;
    }
  }

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

  public ChartDropBarImpl Clone(object parent)
  {
    ChartDropBarImpl parent1 = (ChartDropBarImpl) this.MemberwiseClone();
    parent1.SetParent(parent);
    parent1.SetParents();
    parent1.m_dropBar = (ChartDropBarRecord) CloneUtils.CloneCloneable((ICloneable) this.m_dropBar);
    parent1.m_lineFormat = (ChartBorderImpl) CloneUtils.CloneCloneable((ICloneParent) this.m_lineFormat, (object) this);
    parent1.m_interior = (ChartInteriorImpl) CloneUtils.CloneCloneable((ICloneParent) this.m_interior, (object) this);
    if (this.m_interior != null)
    {
      int pattern = (int) this.m_interior.Pattern;
      parent1.Pattern = this.m_interior.Pattern;
    }
    if (this.m_fill != null)
      parent1.m_fill = (ChartFillImpl) this.m_fill.Clone((object) this);
    if (this.m_3D != null)
      parent1.m_3D = this.m_3D.Clone((object) parent1);
    if (this.m_shadow != null)
      parent1.m_shadow = this.m_shadow.Clone((object) parent1);
    return parent1;
  }
}
