// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.GradientWrapper
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Shapes;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class GradientWrapper : CommonWrapper, IGradient, IOptimizedUpdate
{
  private ShapeFillImpl m_gradient;

  public GradientWrapper()
  {
  }

  public GradientWrapper(ShapeFillImpl gradient)
  {
    this.m_gradient = gradient != null ? gradient : throw new ArgumentNullException(nameof (gradient));
  }

  public ChartColor BackColorObject => this.m_gradient.BackColorObject;

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

  public OfficeKnownColors BackColorIndex
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

  public ChartColor ForeColorObject => this.m_gradient.ForeColorObject;

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

  public OfficeKnownColors ForeColorIndex
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

  public OfficeGradientStyle GradientStyle
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

  public OfficeGradientVariants GradientVariant
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

  public void TwoColorGradient(OfficeGradientStyle style, OfficeGradientVariants variant)
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

  private void ValidateGradientVariant(OfficeGradientVariants gradientVariant)
  {
    switch (this.GradientStyle)
    {
      case OfficeGradientStyle.Horizontal:
      case OfficeGradientStyle.Vertical:
      case OfficeGradientStyle.DiagonalUp:
      case OfficeGradientStyle.DiagonalDown:
        if (gradientVariant != OfficeGradientVariants.ShadingVariants_4)
          break;
        throw new ArgumentException("Shading variant 4 is not valid for current gradient style.");
      case OfficeGradientStyle.FromCenter:
        if (gradientVariant != OfficeGradientVariants.ShadingVariants_2 && gradientVariant != OfficeGradientVariants.ShadingVariants_3 && gradientVariant != OfficeGradientVariants.ShadingVariants_4)
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
