// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ShadowImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ShadowImpl : CommonObject, IShadow, ICloneParent
{
  private ShadowData m_chartShadowFormat = new ShadowData();
  private WorkbookImpl m_parentBook;
  private bool m_HasCustomShadowStyle;
  private int m_Transparency;
  private int m_Size;
  private int m_Blur;
  private ChartMarkerFormatRecord m_Shadow;
  private int m_Angle;
  private int m_Distance;
  private ColorObject m_shadowColor;
  private Stream m_glowStream;
  private int m_softEdgeRadius = -1;

  public ShadowImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.InitializeColors();
    this.SetParents();
  }

  private void InitializeColors()
  {
    this.m_shadowColor = new ColorObject(ColorExtension.Empty);
    this.m_shadowColor.AfterChange += new ColorObject.AfterChangeHandler(this.ShadowColorChanged);
  }

  internal void ShadowColorChanged()
  {
    ExcelKnownColors indexed = this.m_shadowColor.GetIndexed((IWorkbook) this.m_parentBook);
    this.ShadowFormat.FillColorIndex = (ushort) indexed;
    this.ShadowFormat.IsNotShowInt = indexed == ExcelKnownColors.None;
  }

  [CLSCompliant(false)]
  public ChartMarkerFormatRecord ShadowFormat
  {
    get
    {
      if (this.m_Shadow == null)
      {
        this.m_Shadow = (ChartMarkerFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartMarkerFormat);
        this.m_Shadow.IsAutoColor = true;
      }
      return this.m_Shadow;
    }
  }

  private void SetParents()
  {
    this.m_parentBook = (WorkbookImpl) this.FindParent(typeof (WorkbookImpl));
    if (this.m_parentBook == null)
      throw new ApplicationException("cannot find parent objects.");
  }

  public Excel2007ChartPresetsOuter ShadowOuterPresets
  {
    get => this.m_chartShadowFormat.ShadowOuterPresets;
    set
    {
      if (value != this.ShadowOuterPresets)
        this.m_chartShadowFormat.ShadowOuterPresets = value;
      this.m_chartShadowFormat.ShadowInnerPresets = Excel2007ChartPresetsInner.NoShadow;
      this.m_chartShadowFormat.ShadowPrespectivePresets = Excel2007ChartPresetsPrespective.NoShadow;
      this.SetFormat();
      if (this.m_parentBook.Loading)
        return;
      switch (value)
      {
        case Excel2007ChartPresetsOuter.NoShadow:
          this.m_Blur = 0;
          this.m_Distance = 0;
          this.m_Size = 0;
          this.m_Transparency = 0;
          this.m_Angle = 0;
          break;
        case Excel2007ChartPresetsOuter.OffsetCenter:
          this.m_Blur = 5;
          this.m_Distance = 0;
          this.m_Size = 102;
          this.m_Transparency = 60;
          this.m_Angle = 0;
          break;
        default:
          this.m_Blur = 4;
          this.m_Distance = 3;
          this.m_Size = 100;
          this.m_Transparency = 60;
          if (value == Excel2007ChartPresetsOuter.OffsetDiagonalBottomRight)
          {
            this.m_Angle = 45;
            break;
          }
          if (value == Excel2007ChartPresetsOuter.OffsetBottom)
          {
            this.m_Angle = 90;
            break;
          }
          if (value == Excel2007ChartPresetsOuter.OffsetDiagonalBottomLeft)
          {
            this.m_Angle = 135;
            break;
          }
          if (value == Excel2007ChartPresetsOuter.OffsetRight)
          {
            this.m_Angle = 0;
            break;
          }
          if (value == Excel2007ChartPresetsOuter.OffsetLeft)
          {
            this.m_Angle = 180;
            break;
          }
          if (value == Excel2007ChartPresetsOuter.OffsetDiagonalTopRight)
          {
            this.m_Angle = 315;
            break;
          }
          if (value == Excel2007ChartPresetsOuter.OffsetTop)
          {
            this.m_Angle = 270;
            break;
          }
          if (value == Excel2007ChartPresetsOuter.OffsetDiagonalTopLeft)
          {
            this.m_Angle = 225;
            break;
          }
          break;
      }
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (ShadowOuterPresets));
    }
  }

  public Excel2007ChartPresetsInner ShadowInnerPresets
  {
    get => this.m_chartShadowFormat.ShadowInnerPresets;
    set
    {
      if (value != this.ShadowInnerPresets)
        this.m_chartShadowFormat.ShadowInnerPresets = value;
      this.m_chartShadowFormat.ShadowOuterPresets = Excel2007ChartPresetsOuter.NoShadow;
      this.m_chartShadowFormat.ShadowPrespectivePresets = Excel2007ChartPresetsPrespective.NoShadow;
      this.SetFormat();
      if (this.m_parentBook.Loading)
        return;
      switch (value)
      {
        case Excel2007ChartPresetsInner.NoShadow:
          this.m_Blur = 0;
          this.m_Distance = 0;
          this.m_Size = 0;
          this.m_Transparency = 0;
          this.m_Angle = 0;
          break;
        case Excel2007ChartPresetsInner.InsideCenter:
          this.m_Blur = 9;
          this.m_Distance = 0;
          this.m_Transparency = 0;
          this.m_Angle = 0;
          this.m_Size = 0;
          break;
        default:
          this.m_Blur = 5;
          this.m_Distance = 4;
          this.m_Transparency = 50;
          this.m_Size = 0;
          if (value == Excel2007ChartPresetsInner.InsideDiagonalTopLeft)
          {
            this.m_Angle = 225;
            break;
          }
          if (value == Excel2007ChartPresetsInner.InsideTop)
          {
            this.m_Angle = 270;
            break;
          }
          if (value == Excel2007ChartPresetsInner.InsideDiagonalTopRight)
          {
            this.m_Angle = 315;
            break;
          }
          if (value == Excel2007ChartPresetsInner.InsideLeft)
          {
            this.m_Angle = 180;
            break;
          }
          if (value == Excel2007ChartPresetsInner.InsideRight)
          {
            this.m_Angle = 0;
            break;
          }
          if (value == Excel2007ChartPresetsInner.InsideDiagonalBottomLeft)
          {
            this.m_Angle = 135;
            break;
          }
          if (value == Excel2007ChartPresetsInner.InsideBottom)
          {
            this.m_Angle = 90;
            break;
          }
          if (value == Excel2007ChartPresetsInner.InsideDiagonalBottomRight)
          {
            this.m_Angle = 45;
            break;
          }
          break;
      }
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (ShadowInnerPresets));
    }
  }

  public bool HasCustomShadowStyle
  {
    get => this.m_HasCustomShadowStyle;
    set
    {
      this.m_HasCustomShadowStyle = value;
      this.SetFormat();
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (HasCustomShadowStyle));
    }
  }

  public Excel2007ChartPresetsPrespective ShadowPrespectivePresets
  {
    get => this.m_chartShadowFormat.ShadowPrespectivePresets;
    set
    {
      if (value != this.ShadowPrespectivePresets)
        this.m_chartShadowFormat.ShadowPrespectivePresets = value;
      this.m_chartShadowFormat.ShadowInnerPresets = Excel2007ChartPresetsInner.NoShadow;
      this.m_chartShadowFormat.ShadowOuterPresets = Excel2007ChartPresetsOuter.NoShadow;
      this.SetFormat();
      if (this.m_parentBook.Loading)
        return;
      switch (value)
      {
        case Excel2007ChartPresetsPrespective.NoShadow:
          this.m_Transparency = 0;
          this.m_Size = 0;
          this.m_Blur = 0;
          this.m_Angle = 0;
          this.m_Distance = 0;
          break;
        case Excel2007ChartPresetsPrespective.Below:
          this.m_Transparency = 85;
          this.m_Size = 90;
          this.m_Blur = 12;
          this.m_Angle = 90;
          this.m_Distance = 25;
          break;
        default:
          this.m_Transparency = 80 /*0x50*/;
          this.m_Size = 100;
          this.m_Blur = 6;
          if (value == Excel2007ChartPresetsPrespective.PrespectiveDiagonalUpperLeft)
          {
            this.m_Angle = 225;
            this.m_Distance = 0;
            break;
          }
          if (value == Excel2007ChartPresetsPrespective.PrespectiveDiagonalUpperRight)
          {
            this.m_Angle = 315;
            this.m_Distance = 0;
            break;
          }
          if (value == Excel2007ChartPresetsPrespective.PrespectiveDiagonalLowerLeft)
          {
            this.m_Angle = 135;
            this.m_Distance = 1;
            break;
          }
          if (value == Excel2007ChartPresetsPrespective.PrespectiveDiagonalLowerRight)
          {
            this.m_Angle = 45;
            this.m_Distance = 1;
            break;
          }
          break;
      }
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (ShadowPrespectivePresets));
    }
  }

  public int Transparency
  {
    get => this.m_Transparency;
    set
    {
      this.m_Transparency = value >= 0 && value <= 100 ? value : throw new NotSupportedException("The Value of the transparency should be between(0-100)");
      this.SetFormat();
      if (this.m_parentBook.Loading)
        return;
      this.m_HasCustomShadowStyle = true;
      this.m_chartShadowFormat.ShadowOuterPresets = Excel2007ChartPresetsOuter.OffsetCenter;
      if (this.m_chartShadowFormat.ShadowInnerPresets == Excel2007ChartPresetsInner.NoShadow && this.m_chartShadowFormat.ShadowOuterPresets == Excel2007ChartPresetsOuter.NoShadow && this.m_chartShadowFormat.ShadowPrespectivePresets == Excel2007ChartPresetsPrespective.NoShadow)
      {
        this.m_Size = 100;
        this.m_Blur = 4;
        this.m_Angle = 90;
        this.m_Distance = 4;
      }
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (Transparency));
    }
  }

  public int Size
  {
    get => this.m_Size;
    set
    {
      this.m_Size = value > 0 && value <= 200 ? value : throw new NotSupportedException("The value of the size should be between(0-200)");
      this.SetFormat();
      if (this.m_parentBook.Loading)
        return;
      this.m_HasCustomShadowStyle = true;
      this.m_chartShadowFormat.ShadowOuterPresets = Excel2007ChartPresetsOuter.OffsetCenter;
      if (this.m_chartShadowFormat.ShadowInnerPresets == Excel2007ChartPresetsInner.NoShadow && this.m_chartShadowFormat.ShadowOuterPresets == Excel2007ChartPresetsOuter.NoShadow && this.m_chartShadowFormat.ShadowPrespectivePresets == Excel2007ChartPresetsPrespective.NoShadow)
      {
        this.m_Transparency = 57;
        this.m_Blur = 4;
        this.m_Angle = 90;
        this.m_Distance = 4;
      }
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (Size));
    }
  }

  public int Blur
  {
    get => this.m_Blur;
    set
    {
      this.m_Blur = value >= 0 && value <= 100 ? value : throw new NotSupportedException("The Value of the blur should be between(0-100)");
      this.SetFormat();
      if (this.m_parentBook.Loading)
        return;
      this.m_HasCustomShadowStyle = true;
      this.m_chartShadowFormat.ShadowOuterPresets = Excel2007ChartPresetsOuter.OffsetCenter;
      if (this.m_chartShadowFormat.ShadowInnerPresets == Excel2007ChartPresetsInner.NoShadow && this.m_chartShadowFormat.ShadowOuterPresets == Excel2007ChartPresetsOuter.NoShadow && this.m_chartShadowFormat.ShadowPrespectivePresets == Excel2007ChartPresetsPrespective.NoShadow)
      {
        this.m_Transparency = 57;
        this.m_Size = 100;
        this.m_Angle = 90;
        this.m_Distance = 4;
      }
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (Blur));
    }
  }

  public int Angle
  {
    get => this.m_Angle;
    set
    {
      this.m_Angle = value >= 0 && value <= 359 ? value : throw new NotSupportedException("The Value of the angle should be between(0-359)");
      this.SetFormat();
      if (this.m_parentBook.Loading)
        return;
      this.m_HasCustomShadowStyle = true;
      this.m_chartShadowFormat.ShadowOuterPresets = Excel2007ChartPresetsOuter.OffsetCenter;
      if (this.m_chartShadowFormat.ShadowInnerPresets == Excel2007ChartPresetsInner.NoShadow && this.m_chartShadowFormat.ShadowOuterPresets == Excel2007ChartPresetsOuter.NoShadow && this.m_chartShadowFormat.ShadowPrespectivePresets == Excel2007ChartPresetsPrespective.NoShadow)
      {
        this.m_Transparency = 57;
        this.m_Size = 100;
        this.m_Blur = 4;
        this.m_Distance = 4;
      }
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (Angle));
    }
  }

  public Color ShadowColor
  {
    get => this.m_shadowColor.GetRGB((IWorkbook) this.m_parentBook);
    set
    {
      this.m_shadowColor.SetRGB(value);
      this.SetFormat();
      if (this.m_parentBook.Loading)
        return;
      this.m_HasCustomShadowStyle = true;
      this.m_chartShadowFormat.ShadowOuterPresets = Excel2007ChartPresetsOuter.OffsetCenter;
      if (this.m_chartShadowFormat.ShadowInnerPresets == Excel2007ChartPresetsInner.NoShadow && this.m_chartShadowFormat.ShadowOuterPresets == Excel2007ChartPresetsOuter.NoShadow && this.m_chartShadowFormat.ShadowPrespectivePresets == Excel2007ChartPresetsPrespective.NoShadow)
      {
        this.m_Size = 100;
        this.m_Blur = 4;
        this.m_Angle = 90;
        this.m_Distance = 4;
      }
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (ShadowColor));
    }
  }

  public int Distance
  {
    get => this.m_Distance;
    set
    {
      this.m_Distance = value >= 0 && value <= 200 ? value : throw new NotSupportedException("The Value of the distance should be between(0-200)");
      this.SetFormat();
      if (this.m_parentBook.Loading)
        return;
      this.m_HasCustomShadowStyle = true;
      this.m_chartShadowFormat.ShadowOuterPresets = Excel2007ChartPresetsOuter.OffsetCenter;
      if (this.m_chartShadowFormat.ShadowInnerPresets == Excel2007ChartPresetsInner.NoShadow && this.m_chartShadowFormat.ShadowOuterPresets == Excel2007ChartPresetsOuter.NoShadow && this.m_chartShadowFormat.ShadowPrespectivePresets == Excel2007ChartPresetsPrespective.NoShadow)
      {
        this.m_Transparency = 57;
        this.m_Size = 100;
        this.m_Blur = 4;
        this.m_Angle = 90;
      }
      if (this.m_parentBook.Loading || !(this.Parent is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (Distance));
    }
  }

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
        case "Angle":
          shapeImpl.Shadow.Angle = (int) value;
          break;
        case "Blur":
          shapeImpl.Shadow.Blur = (int) value;
          break;
        case "Distance":
          shapeImpl.Shadow.Distance = (int) value;
          break;
        case "HasCustomShadowStyle":
          shapeImpl.Shadow.HasCustomShadowStyle = (bool) value;
          break;
        case "ShadowColor":
          shapeImpl.Shadow.ShadowColor = (Color) value;
          break;
        case "ShadowInnerPresets":
          shapeImpl.Shadow.ShadowInnerPresets = (Excel2007ChartPresetsInner) value;
          break;
        case "ShadowOuterPresets":
          shapeImpl.Shadow.ShadowOuterPresets = (Excel2007ChartPresetsOuter) value;
          break;
        case "ShadowPrespectivePresets":
          shapeImpl.Shadow.ShadowPrespectivePresets = (Excel2007ChartPresetsPrespective) value;
          break;
        case "Size":
          shapeImpl.Shadow.Size = (int) value;
          break;
        case "Transparency":
          shapeImpl.Shadow.Transparency = (int) value;
          break;
      }
    }
  }

  object ICloneParent.Clone(object parent) => (object) this.Clone(parent);

  public ShadowImpl Clone(object parent)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    ShadowImpl shadowImpl = (ShadowImpl) this.MemberwiseClone();
    shadowImpl.m_chartShadowFormat = (ShadowData) CloneUtils.CloneCloneable((ICloneable) this.m_chartShadowFormat);
    shadowImpl.SetParent(parent);
    shadowImpl.SetParents();
    return shadowImpl;
  }

  public void CustomShadowStyles(
    Excel2007ChartPresetsOuter iOuter,
    int iTransparency,
    int iSize,
    int iBlur,
    int iAngle,
    int iDistance,
    bool CustomShadowStyle)
  {
    if (!CustomShadowStyle)
      throw new NotSupportedException("It should be set true to implement the custom shadow style");
    if (iOuter == Excel2007ChartPresetsOuter.NoShadow)
      throw new NotSupportedException("The method does not accept Noshadow");
    if (iSize <= 0 || iSize > 200)
      throw new NotSupportedException("The value of the size should be between(0-200)");
    if (iTransparency < 0 || iTransparency > 100)
      throw new NotSupportedException("The Value of the transparency should be between(0-100)");
    if (iBlur < 0 || iBlur > 100)
      throw new NotSupportedException("The Value of the blur should be between(0-100)");
    if (iAngle < 0 || iAngle > 359)
      throw new NotSupportedException("The Value of the angle should be between(0-359)");
    if (iDistance < 0 || iDistance > 200)
      throw new NotSupportedException("The Value of the distance should be between(0-200)");
    this.m_HasCustomShadowStyle = CustomShadowStyle;
    this.m_chartShadowFormat.ShadowOuterPresets = iOuter;
    this.m_Transparency = iTransparency;
    this.m_Size = iSize;
    this.m_Blur = iBlur;
    this.m_Angle = iAngle;
    this.m_Distance = iDistance;
  }

  public void CustomShadowStyles(
    Excel2007ChartPresetsInner iInner,
    int iTransparency,
    int iBlur,
    int iAngle,
    int iDistance,
    bool CustomShadowStyle)
  {
    if (!CustomShadowStyle)
      throw new NotSupportedException("It should be set true to implement the custom shadow style");
    if (iInner == Excel2007ChartPresetsInner.NoShadow)
      throw new NotSupportedException("The method does not accept Noshadow");
    if (iTransparency < 0 || iTransparency > 100)
      throw new NotSupportedException("The Value of the transparency should be between(0-100)");
    if (iBlur < 0 || iBlur > 100)
      throw new NotSupportedException("The Value of the blur should be between(0-100)");
    if (iAngle < 0 || iAngle > 359)
      throw new NotSupportedException("The Value of the angle should be between(0-359)");
    if (iDistance < 0 || iDistance > 200)
      throw new NotSupportedException("The Value of the distance should be between(0-200)");
    this.m_HasCustomShadowStyle = CustomShadowStyle;
    this.m_chartShadowFormat.ShadowInnerPresets = iInner;
    this.m_Transparency = iTransparency;
    this.m_Blur = iBlur;
    this.m_Angle = iAngle;
    this.m_Distance = iDistance;
  }

  public void CustomShadowStyles(
    Excel2007ChartPresetsPrespective iPerspective,
    int iTransparency,
    int iSize,
    int iBlur,
    int iAngle,
    int iDistance,
    bool CustomShadowStyle)
  {
    if (!CustomShadowStyle)
      throw new NotSupportedException("It should be set true to implement the custom shadow style");
    if (iPerspective == Excel2007ChartPresetsPrespective.NoShadow)
      throw new NotSupportedException("The method does not accept Noshadow");
    if (iSize <= 0 || iSize > 200)
      throw new NotSupportedException("The value of the size should be between(0-200)");
    if (iTransparency < 0 || iTransparency > 100)
      throw new NotSupportedException("The Value of the transparency should be between(0-100)");
    if (iBlur < 0 || iBlur > 100)
      throw new NotSupportedException("The Value of the blur should be between(0-100)");
    if (iAngle < 0 || iAngle > 359)
      throw new NotSupportedException("The Value of the angle should be between(0-359)");
    if (iDistance < 0 || iDistance > 200)
      throw new NotSupportedException("The Value of the distance should be between(0-200)");
    this.m_HasCustomShadowStyle = CustomShadowStyle;
    this.m_chartShadowFormat.ShadowPrespectivePresets = iPerspective;
    this.m_Transparency = iTransparency;
    this.m_Size = iSize;
    this.m_Blur = iBlur;
    this.m_Angle = iAngle;
    this.m_Distance = iDistance;
  }

  internal Stream GlowStream
  {
    get => this.m_glowStream;
    set => this.m_glowStream = value;
  }

  internal int SoftEdgeRadius
  {
    get => this.m_softEdgeRadius;
    set => this.m_softEdgeRadius = value;
  }
}
