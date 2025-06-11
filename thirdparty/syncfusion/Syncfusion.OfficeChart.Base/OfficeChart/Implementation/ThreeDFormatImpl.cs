// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.ThreeDFormatImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.Drawing;
using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class ThreeDFormatImpl : CommonObject, IThreeDFormat, ICloneParent
{
  private ShadowData m_chartShadowFormat = new ShadowData();
  private WorkbookImpl m_parentBook;
  private int m_bevelTopHeight = -1;
  private int m_bevelTopWidth = -1;
  private int m_bevelBottomHeight = -1;
  private int m_bevelBottomWidth = -1;
  private string m_prestShape;

  public ThreeDFormatImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    if (!(this.Parent is ChartWallOrFloorImpl))
      return;
    (this.Parent as ChartWallOrFloorImpl).HasShapeProperties = true;
  }

  private void SetParents()
  {
    this.m_parentBook = (WorkbookImpl) this.FindParent(typeof (WorkbookImpl));
    if (this.m_parentBook == null)
      throw new ApplicationException("cannot find parent objects.");
  }

  public Office2007ChartBevelProperties BevelTop
  {
    get => this.m_chartShadowFormat.BevelTop;
    set
    {
      if (value == this.BevelTop)
        return;
      this.m_chartShadowFormat.BevelTop = value;
    }
  }

  public Office2007ChartBevelProperties BevelBottom
  {
    get => this.m_chartShadowFormat.BevelBottom;
    set
    {
      if (value == this.BevelBottom)
        return;
      this.m_chartShadowFormat.BevelBottom = value;
    }
  }

  public Office2007ChartMaterialProperties Material
  {
    get => this.m_chartShadowFormat.Material;
    set
    {
      if (value == this.Material)
        return;
      this.m_chartShadowFormat.Material = value;
    }
  }

  public Office2007ChartLightingProperties Lighting
  {
    get => this.m_chartShadowFormat.Lighting;
    set
    {
      if (value == this.Lighting)
        return;
      this.m_chartShadowFormat.Lighting = value;
    }
  }

  public int BevelTopHeight
  {
    get => (int) Helper.EmuToPoint(this.m_bevelTopHeight);
    set
    {
      this.m_bevelTopHeight = value >= 0 && value <= 1584 ? Helper.PointToEmu((double) value) : throw new ArgumentException($"Invalid BevalTopHeight {value.ToString()}The value ranges from 0 to 1584");
    }
  }

  public int BevelBottomHeight
  {
    get => (int) Helper.EmuToPoint(this.m_bevelBottomHeight);
    set
    {
      this.m_bevelBottomHeight = value >= 0 && value <= 1584 ? Helper.PointToEmu((double) value) : throw new ArgumentException($"Invalid BevelBottomHeight {value.ToString()}The value ranges from 0 to 1584");
    }
  }

  public int BevelTopWidth
  {
    get => (int) Helper.EmuToPoint(this.m_bevelTopWidth);
    set
    {
      this.m_bevelTopWidth = value >= 0 && value <= 1584 ? Helper.PointToEmu((double) value) : throw new ArgumentException($"Invalid BevelTopWidth {value.ToString()}The value ranges from 0 to 1584");
    }
  }

  public int BevelBottomWidth
  {
    get => (int) Helper.EmuToPoint(this.m_bevelBottomWidth);
    set
    {
      this.m_bevelBottomWidth = value >= 0 && value <= 1584 ? Helper.PointToEmu((double) value) : throw new ArgumentException($"Invalid BevelBottomWidth {value.ToString()}The value ranges from 0 to 1584");
    }
  }

  public string PresetShape
  {
    get => this.m_prestShape;
    set => this.m_prestShape = value;
  }

  internal bool IsBevelTopWidthSet => this.m_bevelTopWidth != -1;

  internal bool IsBevelTopHeightSet => this.m_bevelTopHeight != -1;

  internal bool IsBevelBottomWidthSet => this.m_bevelBottomWidth != -1;

  internal bool IsBevelBottomHeightSet => this.m_bevelBottomHeight != -1;

  object ICloneParent.Clone(object parent) => (object) this.Clone(parent);

  public ThreeDFormatImpl Clone(object parent)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    ThreeDFormatImpl threeDformatImpl = (ThreeDFormatImpl) this.MemberwiseClone();
    threeDformatImpl.m_chartShadowFormat = (ShadowData) CloneUtils.CloneCloneable((ICloneable) this.m_chartShadowFormat);
    threeDformatImpl.SetParent(parent);
    threeDformatImpl.SetParents();
    threeDformatImpl.m_chartShadowFormat.ChartObject = threeDformatImpl.FindParent(typeof (ChartImpl)) as ChartImpl;
    return threeDformatImpl;
  }
}
