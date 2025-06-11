// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartDropBarImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartDropBarImpl : CommonObject, IChartDropBar, IChartFillBorder, IFillColor
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
          this.m_interior.UseAutomaticFormat = false;
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
    get => this.m_3D != null && !this.m_3D.IsDefault;
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

  public IChartInterior Interior
  {
    get
    {
      if (this.m_interior == null)
        this.m_interior = new ChartInteriorImpl(this.Application, (object) this);
      return (IChartInterior) this.m_interior;
    }
  }

  public IChartBorder LineProperties
  {
    get
    {
      if (this.m_lineFormat == null)
        this.m_lineFormat = new ChartBorderImpl(this.Application, (object) this);
      return (IChartBorder) this.m_lineFormat;
    }
  }

  public IFill Fill
  {
    get
    {
      if (this.m_fill == null)
        this.m_fill = new ChartFillImpl(this.Application, (object) this);
      return (IFill) this.m_fill;
    }
  }

  public ColorObject ForeGroundColorObject
  {
    get => (this.Interior as ChartInteriorImpl).ForegroundColorObject;
  }

  public ColorObject BackGroundColorObject
  {
    get => (this.Interior as ChartInteriorImpl).BackgroundColorObject;
  }

  public ExcelPattern Pattern
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
    get => this.Interior.Pattern != ExcelPattern.None;
    set
    {
      if (value)
      {
        if (this.Interior.Pattern != ExcelPattern.None)
          return;
        this.Interior.Pattern = ExcelPattern.Solid;
      }
      else
        this.Interior.Pattern = ExcelPattern.None;
    }
  }

  public ChartDropBarImpl Clone(object parent)
  {
    ChartDropBarImpl chartDropBarImpl = (ChartDropBarImpl) this.MemberwiseClone();
    chartDropBarImpl.SetParent(parent);
    chartDropBarImpl.SetParents();
    chartDropBarImpl.m_dropBar = (ChartDropBarRecord) CloneUtils.CloneCloneable((ICloneable) this.m_dropBar);
    chartDropBarImpl.m_lineFormat = (ChartBorderImpl) CloneUtils.CloneCloneable((ICloneParent) this.m_lineFormat, (object) this);
    chartDropBarImpl.m_interior = (ChartInteriorImpl) CloneUtils.CloneCloneable((ICloneParent) this.m_interior, (object) this);
    return chartDropBarImpl;
  }
}
