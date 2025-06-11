// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedAccentWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedAccentWidget : LayoutedFuntionWidget
{
  private LayoutedOMathWidget m_equation;
  private LayoutedStringWidget m_accentCharacter;
  private float m_scalingFactor = 1f;

  internal LayoutedAccentWidget(IOfficeMathFunctionBase widget)
    : base(widget)
  {
  }

  internal LayoutedAccentWidget(LayoutedAccentWidget srcWidget)
    : base((LayoutedFuntionWidget) srcWidget)
  {
    this.AccentCharacter = new LayoutedStringWidget(srcWidget.AccentCharacter);
    this.Equation = new LayoutedOMathWidget(srcWidget.Equation);
  }

  internal LayoutedStringWidget AccentCharacter
  {
    get => this.m_accentCharacter;
    set => this.m_accentCharacter = value;
  }

  internal LayoutedOMathWidget Equation
  {
    get => this.m_equation;
    set => this.m_equation = value;
  }

  internal float ScalingFactor
  {
    get => this.m_scalingFactor;
    set => this.m_scalingFactor = value;
  }

  public override void ShiftXYPosition(float xPosition, float yPosition)
  {
    this.Bounds = new RectangleF(this.Bounds.X + xPosition, this.Bounds.Y + yPosition, this.Bounds.Width, this.Bounds.Height);
    this.AccentCharacter.Bounds = new RectangleF(this.AccentCharacter.Bounds.X + xPosition, this.AccentCharacter.Bounds.Y + yPosition, this.AccentCharacter.Bounds.Width, this.AccentCharacter.Bounds.Height);
    this.Equation.ShiftXYPosition(xPosition, yPosition);
  }

  public override void Dispose()
  {
    if (this.Equation != null)
    {
      this.Equation.Dispose();
      this.Equation = (LayoutedOMathWidget) null;
    }
    if (this.AccentCharacter != null)
    {
      this.AccentCharacter.Dispose();
      this.AccentCharacter = (LayoutedStringWidget) null;
    }
    base.Dispose();
  }
}
