// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.GradientWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Interfaces;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class GradientWrapper : CommonWrapper, IGradient, IOptimizedUpdate
{
  private ShapeFillImpl m_gradient;

  public GradientWrapper()
  {
  }

  public GradientWrapper(ShapeFillImpl gradient)
  {
    this.m_gradient = gradient != null ? gradient : throw new ArgumentNullException(nameof (gradient));
  }

  public ColorObject BackColorObject => this.m_gradient.BackColorObject;

  public Color BackColor
  {
    get => this.m_gradient.BackColor;
    set
    {
      if (!(value != this.BackColor))
        return;
      this.BeginUpdate();
      this.m_gradient.BackColor = value;
      this.EndUpdate();
    }
  }

  public ExcelKnownColors BackColorIndex
  {
    get => this.m_gradient.BackColorIndex;
    set
    {
      if (value == this.BackColorIndex)
        return;
      this.BeginUpdate();
      this.m_gradient.BackColorIndex = value;
      this.EndUpdate();
    }
  }

  public ColorObject ForeColorObject => this.m_gradient.ForeColorObject;

  public Color ForeColor
  {
    get => this.m_gradient.ForeColor;
    set
    {
      if (!(value != this.ForeColor))
        return;
      this.BeginUpdate();
      this.m_gradient.ForeColor = value;
      this.EndUpdate();
    }
  }

  public ExcelKnownColors ForeColorIndex
  {
    get => this.m_gradient.ForeColorIndex;
    set
    {
      if (value == this.ForeColorIndex)
        return;
      this.BeginUpdate();
      this.m_gradient.ForeColorIndex = value;
      this.EndUpdate();
    }
  }

  public ExcelGradientStyle GradientStyle
  {
    get => this.m_gradient.GradientStyle;
    set
    {
      if (value == this.GradientStyle)
        return;
      this.BeginUpdate();
      this.m_gradient.GradientStyle = value;
      this.EndUpdate();
    }
  }

  public ExcelGradientVariants GradientVariant
  {
    get => this.m_gradient.GradientVariant;
    set
    {
      this.ValidateGradientVariant(value);
      if (value == this.GradientVariant)
        return;
      this.BeginUpdate();
      this.m_gradient.GradientVariant = value;
      this.EndUpdate();
    }
  }

  public int CompareTo(IGradient gradient) => this.m_gradient.CompareTo(gradient);

  public void TwoColorGradient()
  {
    this.BeginUpdate();
    this.m_gradient.TwoColorGradient();
    this.EndUpdate();
  }

  public void TwoColorGradient(ExcelGradientStyle style, ExcelGradientVariants variant)
  {
    this.BeginUpdate();
    this.m_gradient.TwoColorGradient(style, variant);
    this.EndUpdate();
  }

  public ShapeFillImpl Wrapped => this.m_gradient;

  public event EventHandler AfterChangeEvent;

  public override void BeginUpdate()
  {
    if (this.BeginCallsCount == 0)
      this.m_gradient = this.m_gradient.Clone(this.m_gradient.Parent);
    base.BeginUpdate();
  }

  public override void EndUpdate()
  {
    base.EndUpdate();
    if (this.BeginCallsCount != 0)
      return;
    ((ExtendedFormatImpl) this.m_gradient.Parent).Workbook.SetChanged();
    if (this.AfterChangeEvent == null)
      return;
    this.AfterChangeEvent((object) this, EventArgs.Empty);
  }

  private void ValidateGradientVariant(ExcelGradientVariants gradientVariant)
  {
    switch (this.GradientStyle)
    {
      case ExcelGradientStyle.Horizontal:
      case ExcelGradientStyle.Vertical:
      case ExcelGradientStyle.Diagonl_Up:
      case ExcelGradientStyle.Diagonl_Down:
        if (gradientVariant != ExcelGradientVariants.ShadingVariants_4)
          break;
        throw new ArgumentException("Shading variant 4 is not valid for current gradient style.");
      case ExcelGradientStyle.From_Center:
        if (gradientVariant != ExcelGradientVariants.ShadingVariants_2 && gradientVariant != ExcelGradientVariants.ShadingVariants_3 && gradientVariant != ExcelGradientVariants.ShadingVariants_4)
          break;
        throw new ArgumentException("Current shading variant is not valid for from center gradient style.");
    }
  }

  internal void Dispose()
  {
    this.AfterChangeEvent = (EventHandler) null;
    this.m_gradient.Clear();
  }
}
