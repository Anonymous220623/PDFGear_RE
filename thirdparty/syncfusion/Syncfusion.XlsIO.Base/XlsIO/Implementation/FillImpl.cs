// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.FillImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class FillImpl
{
  private ColorObject m_color = new ColorObject(ExcelKnownColors.BlackCustom);
  private ColorObject m_patternColor = new ColorObject(ExcelKnownColors.None);
  private ExcelPattern m_pattern;
  private ExcelGradientStyle m_gradientStyle;
  private ExcelGradientVariants m_gradientVariant;
  private ExcelFillType m_fillType;
  private bool m_IsDxfPatternNone;

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

  public FillImpl(ExcelPattern pattern, Color color, Color patternColor)
  {
    this.m_pattern = pattern;
    if (pattern != ExcelPattern.None)
      this.m_color.SetRGB(color);
    if (pattern != ExcelPattern.Solid)
      this.m_patternColor.SetRGB(patternColor);
    this.m_fillType = pattern == ExcelPattern.Solid ? ExcelFillType.SolidColor : ExcelFillType.Pattern;
  }

  public FillImpl(ExcelPattern pattern, ColorObject color, ColorObject patternColor)
  {
    this.m_pattern = pattern;
    if (pattern != ExcelPattern.None)
      this.m_color = color;
    if (pattern != ExcelPattern.Solid)
      this.m_patternColor = patternColor;
    this.m_fillType = pattern == ExcelPattern.Solid ? ExcelFillType.SolidColor : ExcelFillType.Pattern;
  }

  public ColorObject ColorObject => this.m_color;

  public ColorObject PatternColorObject => this.m_patternColor;

  public ExcelPattern Pattern
  {
    get => this.m_pattern;
    set => this.m_pattern = value;
  }

  public ExcelGradientStyle GradientStyle
  {
    get => this.m_gradientStyle;
    set => this.m_gradientStyle = value;
  }

  public ExcelGradientVariants GradientVariant
  {
    get => this.m_gradientVariant;
    set => this.m_gradientVariant = value;
  }

  public ExcelFillType FillType
  {
    get => this.m_fillType;
    set => this.m_fillType = value;
  }

  internal bool IsDxfPatternNone
  {
    get => this.m_IsDxfPatternNone;
    set => this.m_IsDxfPatternNone = value;
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
    this.m_color = (ColorObject) null;
    this.m_patternColor = (ColorObject) null;
  }
}
