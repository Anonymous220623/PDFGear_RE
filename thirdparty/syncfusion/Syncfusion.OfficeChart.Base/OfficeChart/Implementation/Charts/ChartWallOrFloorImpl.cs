// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartWallOrFloorImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartWallOrFloorImpl : 
  ChartGridLineImpl,
  IOfficeChartWallOrFloor,
  IOfficeChartGridLine,
  IOfficeChartFillBorder,
  IFillColor
{
  public const int DEF_CATEGORY_LINE_COLOR = 8421504 /*0x808080*/;
  public const OfficeKnownColors DEF_CATEGORY_COLOR_INDEX = OfficeKnownColors.Grey_50_percent;
  private const int DEF_VALUE_LINE_COLOR = 0;
  public const int DEF_CATEGORY_FOREGROUND_COLOR = 12632256 /*0xC0C0C0*/;
  public const OfficeKnownColors DEF_CATEGORY_BACKGROUND_COLOR_INDEX = OfficeKnownColors.Turquoise | OfficeKnownColors.BlackCustom;
  public const OfficeKnownColors DEF_VALUE_BACKGROUND_COLOR_INDEX = OfficeKnownColors.YellowCustom | OfficeKnownColors.BlackCustom;
  private const OfficeKnownColors DEF_VALUE_FOREGROUND_COLOR_INDEX = OfficeKnownColors.Pink | OfficeKnownColors.BlackCustom;
  private bool m_bWalls;
  private ChartInteriorImpl m_interior;
  private ChartImpl m_parentChart;
  private ShadowImpl m_shadow;
  private ThreeDFormatImpl m_3D;
  private ChartFillImpl m_fill;
  private bool m_shapeProperties;
  private uint m_thickness;
  private OfficeChartPictureType m_PictureUnit = OfficeChartPictureType.stretch;

  public ChartWallOrFloorImpl(IApplication application, object parent, bool bWalls)
    : base(application, parent, ExcelAxisLineIdentifier.MajorGridLine)
  {
    this.AxisLineType = ExcelAxisLineIdentifier.WallsOrFloor;
    this.m_interior = new ChartInteriorImpl(application, (object) this);
    this.m_interior.InitForFrameFormat(false, true, this.m_parentBook.Version == OfficeVersion.Excel97to2003, !bWalls);
    this.m_bWalls = bWalls;
    this.m_parentChart = (ChartImpl) this.FindParent(typeof (ChartImpl));
    this.m_fill = new ChartFillImpl(application, (object) this);
    if (this.m_parentChart == null)
      throw new ApplicationException("Can't find parent objects");
    this.SetToDefault();
  }

  public ChartWallOrFloorImpl(
    IApplication application,
    object parent,
    bool bWalls,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : base(application, parent, data, ref iPos)
  {
    this.AxisLineType = ExcelAxisLineIdentifier.WallsOrFloor;
    this.m_bWalls = bWalls;
    this.m_parentChart = (ChartImpl) this.FindParent(typeof (ChartImpl));
    if (this.m_fill == null)
      this.m_fill = new ChartFillImpl(application, (object) this);
    if (this.m_parentChart == null)
      throw new ApplicationException("Can't find parent objects");
  }

  [CLSCompliant(false)]
  public override void Parse(IList<BiffRecordRaw> data, ref int iPos)
  {
    this.m_interior = (ChartInteriorImpl) null;
    base.Parse(data, ref iPos);
    if (this.AxisLineType != ExcelAxisLineIdentifier.WallsOrFloor)
      throw new ParseException("Bad axis line type");
    if (data[iPos].TypeCode == TBIFFRecord.ChartAreaFormat)
      this.m_interior = new ChartInteriorImpl(this.Application, (object) this, data, ref iPos);
    BiffRecordRaw gel = data[iPos];
    if (gel.TypeCode == TBIFFRecord.ChartGelFrame)
    {
      this.m_fill = new ChartFillImpl(this.Application, (object) this, (ChartGelFrameRecord) gel);
      ++iPos;
    }
    int num = 1;
    while (num > 0)
    {
      switch (data[iPos].TypeCode)
      {
        case TBIFFRecord.Begin:
          ++num;
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
  public override void Serialize(OffsetArrayList records)
  {
    base.Serialize(records);
    if (this.m_interior != null)
      this.m_interior.Serialize((IList<IBiffStorage>) records);
    this.m_fill.Serialize((IList<IBiffStorage>) records);
  }

  public new IOfficeChartInterior Interior
  {
    get
    {
      if (this.m_interior == null)
        this.m_interior = new ChartInteriorImpl(this.Application, (object) this);
      return (IOfficeChartInterior) this.m_interior;
    }
  }

  public new IShadow Shadow
  {
    get
    {
      if (this.m_shadow == null)
        this.m_shadow = new ShadowImpl(this.Application, (object) this);
      return (IShadow) this.m_shadow;
    }
  }

  public new bool HasShadowProperties
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

  internal bool HasShapeProperties
  {
    get => this.m_shapeProperties;
    set => this.m_shapeProperties = value;
  }

  public uint Thickness
  {
    get => this.m_thickness;
    set => this.m_thickness = value;
  }

  public OfficeChartPictureType PictureUnit
  {
    get => this.m_PictureUnit;
    set
    {
      if (value == OfficeChartPictureType.stack)
        this.m_PictureUnit = value;
      else
        this.m_PictureUnit = OfficeChartPictureType.stretch;
    }
  }

  public new IThreeDFormat ThreeD
  {
    get
    {
      if (this.m_3D == null)
        this.m_3D = new ThreeDFormatImpl(this.Application, (object) this);
      return (IThreeDFormat) this.m_3D;
    }
  }

  public new bool Has3dProperties
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

  public new IOfficeFill Fill
  {
    get
    {
      this.IsAutomaticFormat = false;
      return (IOfficeFill) this.m_fill;
    }
  }

  public new bool HasInterior => this.m_interior != null;

  private bool IsWall => this.m_bWalls;

  public override void Delete()
  {
    if (this.m_bWalls)
    {
      this.m_parentChart.Walls = (IOfficeChartWallOrFloor) new ChartWallOrFloorImpl(this.Application, (object) this.m_parentChart, true);
      this.m_parentChart.SideWall = (IOfficeChartWallOrFloor) new ChartWallOrFloorImpl(this.Application, (object) this.m_parentChart, true);
    }
    else
      this.m_parentChart.Floor = (IOfficeChartWallOrFloor) new ChartWallOrFloorImpl(this.Application, (object) this.m_parentChart, false);
  }

  public void SetToDefault()
  {
    if (this.m_bWalls)
    {
      this.SetToDefaultCategoryLine();
      this.SetToDefaultCategoryArea();
    }
    else
    {
      this.SetToDefaultValueLine();
      this.SetToDefaultValueArea();
    }
  }

  private void SetToDefaultCategoryLine()
  {
    this.Border.LineWeight = OfficeChartLineWeight.Narrow;
    this.Border.ColorIndex = OfficeKnownColors.Grey_50_percent;
  }

  private void SetToDefaultValueLine()
  {
    if (this.m_parentBook.Version == OfficeVersion.Excel97to2003)
      this.Border.ColorIndex = OfficeKnownColors.YellowCustom | OfficeKnownColors.BlackCustom;
    else
      this.Border.ColorIndex = OfficeKnownColors.Grey_25_percent;
  }

  private void SetToDefaultCategoryArea()
  {
    if (this.m_parentBook.Version == OfficeVersion.Excel97to2003)
    {
      this.m_interior.Pattern = OfficePattern.Solid;
      this.m_interior.ForegroundColorObject.SetIndexed(OfficeKnownColors.Grey_25_percent);
      this.m_interior.BackgroundColorObject.SetIndexed(OfficeKnownColors.Turquoise | OfficeKnownColors.BlackCustom);
    }
    else
      this.m_interior.Pattern = OfficePattern.None;
  }

  private void SetToDefaultValueArea() => this.Interior.UseAutomaticFormat = true;

  public override object Clone(object parent)
  {
    ChartWallOrFloorImpl parent1 = (ChartWallOrFloorImpl) base.Clone(parent);
    if (this.m_interior != null)
      parent1.m_interior = this.m_interior.Clone((object) parent1);
    if (this.m_3D != null)
      parent1.m_3D = this.m_3D.Clone((object) parent1);
    if (this.m_shadow != null)
      parent1.m_shadow = this.m_shadow.Clone((object) parent1);
    if (this.m_fill != null)
      parent1.m_fill = (ChartFillImpl) this.m_fill.Clone((object) parent1);
    parent1.m_PictureUnit = this.m_PictureUnit;
    return (object) parent1;
  }

  public ChartColor ForeGroundColorObject => this.m_interior.ForegroundColorObject;

  public ChartColor BackGroundColorObject => this.m_interior.BackgroundColorObject;

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
}
