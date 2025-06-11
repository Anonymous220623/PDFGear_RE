// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ThreeDFormatImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Drawing;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ThreeDFormatImpl : CommonObject, IThreeDFormat, ICloneParent
{
  private ShadowData m_chartShadowFormat;
  private WorkbookImpl m_parentBook;
  private int m_bevelTopHeight = -1;
  private int m_bevelTopWidth = -1;
  private int m_bevelBottomHeight = -1;
  private int m_bevelBottomWidth = -1;
  private byte m_flagOptions;

  public ThreeDFormatImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    this.m_flagOptions = (byte) 0;
    this.m_chartShadowFormat = new ShadowData(this);
  }

  private void SetParents()
  {
    this.m_parentBook = (WorkbookImpl) this.FindParent(typeof (WorkbookImpl));
    if (this.m_parentBook == null)
      throw new ApplicationException("cannot find parent objects.");
  }

  public Excel2007ChartBevelProperties BevelTop
  {
    get => this.m_chartShadowFormat.BevelTop;
    set
    {
      if (value != this.BevelTop)
        this.m_chartShadowFormat.BevelTop = value;
      this.SetFormat();
      this.m_flagOptions |= (byte) 1;
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (BevelTop));
    }
  }

  public Excel2007ChartBevelProperties BevelBottom
  {
    get => this.m_chartShadowFormat.BevelBottom;
    set
    {
      if (value != this.BevelBottom)
        this.m_chartShadowFormat.BevelBottom = value;
      this.SetFormat();
      this.m_flagOptions |= (byte) 2;
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (BevelBottom));
    }
  }

  public Excel2007ChartMaterialProperties Material
  {
    get => this.m_chartShadowFormat.Material;
    set
    {
      if (value != this.Material)
        this.m_chartShadowFormat.Material = value;
      this.SetFormat();
      this.m_flagOptions |= (byte) 4;
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (Material));
    }
  }

  public Excel2007ChartLightingProperties Lighting
  {
    get => this.m_chartShadowFormat.Lighting;
    set
    {
      if (value != this.Lighting)
        this.m_chartShadowFormat.Lighting = value;
      this.SetFormat();
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (Lighting));
    }
  }

  public int BevelTopHeight
  {
    get => (int) Helper.EmuToPoint(this.m_bevelTopHeight);
    set
    {
      this.m_bevelTopHeight = value > 0 ? (value < 1584 ? Helper.PointToEmu((double) value) : 1584) : 0;
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (BevelTopHeight));
    }
  }

  public int BevelBottomHeight
  {
    get => (int) Helper.EmuToPoint(this.m_bevelBottomHeight);
    set
    {
      this.m_bevelBottomHeight = value > 0 ? (value < 1584 ? Helper.PointToEmu((double) value) : 1584) : 0;
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (BevelBottomHeight));
    }
  }

  public int BevelTopWidth
  {
    get => (int) Helper.EmuToPoint(this.m_bevelTopWidth);
    set
    {
      this.m_bevelTopWidth = value > 0 ? (value < 1584 ? Helper.PointToEmu((double) value) : 1584) : 0;
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (BevelTopWidth));
    }
  }

  public int BevelBottomWidth
  {
    get => (int) Helper.EmuToPoint(this.m_bevelBottomWidth);
    set
    {
      this.m_bevelBottomWidth = value > 0 ? (value < 1584 ? Helper.PointToEmu((double) value) : 1584) : 0;
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (BevelBottomWidth));
    }
  }

  internal bool IsDefault
  {
    get
    {
      return (this.IsBevelBottomSet ? 0 : (this.BevelBottom == Excel2007ChartBevelProperties.NoAngle ? 1 : 0)) != 0 && (this.IsBevelTopSet ? 0 : (this.BevelTop == Excel2007ChartBevelProperties.NoAngle ? 1 : 0)) != 0 && this.Material == Excel2007ChartMaterialProperties.NoEffect && this.m_bevelBottomHeight == -1 && this.m_bevelTopHeight == -1 && this.m_bevelTopWidth == -1 && this.m_bevelBottomWidth == -1;
    }
  }

  internal Excel2007ChartMaterialProperties GetMaterial() => this.m_chartShadowFormat.GetMaterial();

  private void SetFormat()
  {
    if (!(this.Parent is ChartSerieDataFormatImpl))
      return;
    ((ChartSerieDataFormatImpl) this.Parent).IsFormatted = true;
  }

  internal void SetInnerShapes(object value, string property)
  {
    foreach (IShape shape in (this.Parent as GroupShapeImpl).Items)
    {
      ShapeImpl shapeImpl = shape as ShapeImpl;
      switch (property)
      {
        case "BevelBottom":
          shapeImpl.ThreeD.BevelBottom = (Excel2007ChartBevelProperties) value;
          break;
        case "BevelBottomHeight":
          shapeImpl.ThreeD.BevelBottomHeight = (int) value;
          break;
        case "BevelBottomWidth":
          shapeImpl.ThreeD.BevelBottomWidth = (int) value;
          break;
        case "BevelTop":
          shapeImpl.ThreeD.BevelTop = (Excel2007ChartBevelProperties) value;
          break;
        case "BevelTopHeight":
          shapeImpl.ThreeD.BevelTopHeight = (int) value;
          break;
        case "BevelTopWidth":
          shapeImpl.ThreeD.BevelTopWidth = (int) value;
          break;
        case "Lighting":
          shapeImpl.ThreeD.Lighting = (Excel2007ChartLightingProperties) value;
          break;
        case "Material":
          shapeImpl.ThreeD.Material = (Excel2007ChartMaterialProperties) value;
          break;
      }
    }
  }

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

  internal bool IsBevelTopWidthSet => this.m_bevelTopWidth != -1;

  internal bool IsBevelTopHeightSet => this.m_bevelTopHeight != -1;

  internal bool IsBevelBottomWidthSet => this.m_bevelBottomWidth != -1;

  internal bool IsBevelBottomHeightSet => this.m_bevelBottomHeight != -1;

  internal bool IsBevelBottomSet => ((int) this.m_flagOptions & 2) != 0;

  internal bool IsBevelTopSet => ((int) this.m_flagOptions & 1) != 0;

  internal bool IsMaterialSet => ((int) this.m_flagOptions & 4) != 0;
}
