// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.FillImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class FillImpl
{
  private ChartColor m_color = new ChartColor(OfficeKnownColors.BlackCustom);
  private ChartColor m_patternColor = new ChartColor(OfficeKnownColors.White | OfficeKnownColors.BlackCustom);
  private OfficePattern m_pattern;
  private OfficeGradientStyle m_gradientStyle;
  private OfficeGradientVariants m_gradientVariant;
  private OfficeFillType m_fillType;

  public FillImpl()
  {
  }

  public FillImpl(ExtendedFormatImpl format)
  {
    IGradient gradient = format != null ? format.Gradient : throw new ArgumentNullException(nameof (format));
    if (gradient != null)
    {
      this.m_gradientStyle = gradient.GradientStyle;
      this.m_gradientVariant = gradient.GradientVariant;
      this.m_color = gradient.BackColorObject;
      this.m_patternColor = gradient.ForeColorObject;
    }
    else
    {
      this.m_color = format.ColorObject;
      this.m_patternColor = format.PatternColorObject;
    }
    this.m_pattern = format.FillPattern;
  }

  public FillImpl(OfficePattern pattern, Color color, Color patternColor)
  {
    this.m_pattern = pattern;
    if (pattern != OfficePattern.None)
      this.m_color.SetRGB(color);
    if (pattern != OfficePattern.Solid)
      this.m_patternColor.SetRGB(patternColor);
    this.m_fillType = pattern == OfficePattern.Solid ? OfficeFillType.SolidColor : OfficeFillType.Pattern;
  }

  public FillImpl(OfficePattern pattern, ChartColor color, ChartColor patternColor)
  {
    this.m_pattern = pattern;
    if (pattern != OfficePattern.None)
      this.m_color = color;
    if (pattern != OfficePattern.Solid)
      this.m_patternColor = patternColor;
    this.m_fillType = pattern == OfficePattern.Solid ? OfficeFillType.SolidColor : OfficeFillType.Pattern;
  }

  public ChartColor ColorObject => this.m_color;

  public ChartColor PatternColorObject => this.m_patternColor;

  public OfficePattern Pattern
  {
    get => this.m_pattern;
    set => this.m_pattern = value;
  }

  public OfficeGradientStyle GradientStyle
  {
    get => this.m_gradientStyle;
    set => this.m_gradientStyle = value;
  }

  public OfficeGradientVariants GradientVariant
  {
    get => this.m_gradientVariant;
    set => this.m_gradientVariant = value;
  }

  public OfficeFillType FillType
  {
    get => this.m_fillType;
    set => this.m_fillType = value;
  }

  public override bool Equals(object obj)
  {
    return obj is FillImpl fillImpl && this.ColorObject == fillImpl.ColorObject && this.PatternColorObject == fillImpl.PatternColorObject && this.Pattern == fillImpl.Pattern && this.GradientStyle == fillImpl.GradientStyle && this.GradientVariant == fillImpl.GradientVariant && this.FillType == fillImpl.FillType;
  }

  public override int GetHashCode()
  {
    return this.ColorObject.GetHashCode() ^ this.PatternColorObject.GetHashCode() ^ this.Pattern.GetHashCode() ^ this.GradientStyle.GetHashCode() ^ this.GradientVariant.GetHashCode() ^ this.FillType.GetHashCode();
  }

  public FillImpl Clone() => (FillImpl) this.MemberwiseClone();

  internal void Dispose()
  {
    this.m_color.Dispose();
    this.m_patternColor.Dispose();
    this.m_color = (ChartColor) null;
    this.m_patternColor = (ChartColor) null;
  }
}
