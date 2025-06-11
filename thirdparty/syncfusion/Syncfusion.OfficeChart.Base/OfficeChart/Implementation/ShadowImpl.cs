// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.ShadowImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class ShadowImpl : CommonObject, IShadow, ICloneParent
{
  private ShadowData m_chartShadowFormat = new ShadowData();
  private WorkbookImpl m_parentBook;
  private bool m_HasCustomShadowStyle;
  private int m_Transparency = -1;
  private int m_Size;
  private int m_Blur;
  private ChartMarkerFormatRecord m_Shadow;
  private int m_Angle;
  private int m_Distance;
  private ChartColor m_shadowColor;

  public ShadowImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.InitializeColors();
    this.SetParents();
    if (!(this.Parent is ChartWallOrFloorImpl))
      return;
    (this.Parent as ChartWallOrFloorImpl).HasShapeProperties = true;
  }

  private void InitializeColors()
  {
    this.m_shadowColor = new ChartColor(ColorExtension.Empty);
    this.m_shadowColor.AfterChange += new ChartColor.AfterChangeHandler(this.ShadowColorChanged);
  }

  internal void ShadowColorChanged()
  {
    OfficeKnownColors indexed = this.m_shadowColor.GetIndexed((IWorkbook) this.m_parentBook);
    this.ShadowFormat.FillColorIndex = (ushort) indexed;
    this.ShadowFormat.IsNotShowInt = indexed == OfficeKnownColors.Black;
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

  public Office2007ChartPresetsOuter ShadowOuterPresets
  {
    get => this.m_chartShadowFormat.ShadowOuterPresets;
    set
    {
      if (value == this.ShadowOuterPresets)
        return;
      this.m_chartShadowFormat.ShadowOuterPresets = value;
    }
  }

  public Office2007ChartPresetsInner ShadowInnerPresets
  {
    get => this.m_chartShadowFormat.ShadowInnerPresets;
    set
    {
      if (value == this.ShadowInnerPresets)
        return;
      this.m_chartShadowFormat.ShadowInnerPresets = value;
    }
  }

  public bool HasCustomShadowStyle
  {
    get => this.m_HasCustomShadowStyle;
    set => this.m_HasCustomShadowStyle = value;
  }

  public Office2007ChartPresetsPerspective ShadowPerspectivePresets
  {
    get => this.m_chartShadowFormat.ShadowPrespectivePresets;
    set
    {
      if (value == this.ShadowPerspectivePresets)
        return;
      this.m_chartShadowFormat.ShadowPrespectivePresets = value;
    }
  }

  public int Transparency
  {
    get => this.m_Transparency != -1 ? 100 - this.m_Transparency : this.m_Transparency;
    set
    {
      if (value == -1)
        return;
      if (value < 0 || value > 100)
        throw new NotSupportedException("The Value of the transparency should be between(0-100)");
      this.m_Transparency = 100 - value;
    }
  }

  internal ChartColor ColorObject => this.m_shadowColor;

  public int Size
  {
    get => this.m_Size / 1000;
    set
    {
      if (!this.HasCustomShadowStyle)
        return;
      if (value <= 0 || value > 200)
        throw new NotSupportedException("The value of the size should be between(0-200)");
      this.m_Size = value * 1000;
    }
  }

  public int Blur
  {
    get => this.m_Blur / 12700;
    set
    {
      if (!this.HasCustomShadowStyle)
        return;
      if (value < 0 || value > 100)
        throw new NotSupportedException("The Value of the blur should be between(0-100)");
      this.m_Blur = value * 12700;
    }
  }

  public int Angle
  {
    get => this.m_Angle / 60000;
    set
    {
      if (!this.HasCustomShadowStyle)
        return;
      if (value < 0 || value > 359)
        throw new NotSupportedException("The Value of the angle should be between(0-359)");
      this.m_Angle = value * 60000;
    }
  }

  public Color ShadowColor
  {
    get => this.m_shadowColor.GetRGB((IWorkbook) this.m_parentBook);
    set => this.m_shadowColor.SetRGB(value);
  }

  public int Distance
  {
    get => this.m_Distance / 12700;
    set
    {
      if (!this.HasCustomShadowStyle)
        return;
      if (value < 0 || value > 200)
        throw new NotSupportedException("The Value of the distance should be between(0-200)");
      this.m_Distance = value * 12700;
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
    Office2007ChartPresetsOuter iOuter,
    int iTransparency,
    int iSize,
    int iBlur,
    int iAngle,
    int iDistance,
    bool CustomShadowStyle)
  {
    if (!CustomShadowStyle)
      throw new NotSupportedException("It should be set true to implement the custom shadow style");
    if (iOuter == Office2007ChartPresetsOuter.NoShadow)
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
    this.m_Transparency = (100 - iTransparency) * 1000;
    this.m_Size = iSize * 1000;
    this.m_Blur = iBlur * 12700;
    this.m_Angle = iAngle * 60000;
    this.m_Distance = iDistance * 12700;
  }

  public void CustomShadowStyles(
    Office2007ChartPresetsInner iInner,
    int iTransparency,
    int iBlur,
    int iAngle,
    int iDistance,
    bool CustomShadowStyle)
  {
    if (!CustomShadowStyle)
      throw new NotSupportedException("It should be set true to implement the custom shadow style");
    if (iInner == Office2007ChartPresetsInner.NoShadow)
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
    this.m_Transparency = (100 - iTransparency) * 1000;
    this.m_Blur = iBlur * 12700;
    this.m_Angle = iAngle * 60000;
    this.m_Distance = iDistance * 12700;
  }

  public void CustomShadowStyles(
    Office2007ChartPresetsPerspective iPerspective,
    int iTransparency,
    int iSize,
    int iBlur,
    int iAngle,
    int iDistance,
    bool CustomShadowStyle)
  {
    if (!CustomShadowStyle)
      throw new NotSupportedException("It should be set true to implement the custom shadow style");
    if (iPerspective == Office2007ChartPresetsPerspective.NoShadow)
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
    this.m_Transparency = (100 - iTransparency) * 1000;
    this.m_Size = iSize * 1000;
    this.m_Blur = iBlur * 12700;
    this.m_Angle = iAngle * 60000;
    this.m_Distance = iDistance * 12700;
  }
}
