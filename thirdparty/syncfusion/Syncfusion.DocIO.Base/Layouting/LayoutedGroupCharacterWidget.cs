// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedGroupCharacterWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedGroupCharacterWidget : LayoutedFuntionWidget
{
  private LayoutedOMathWidget m_equation;
  private LayoutedStringWidget m_groupCharacter;
  private float m_scalingFactor = 1f;

  internal LayoutedGroupCharacterWidget(IOfficeMathFunctionBase widget)
    : base(widget)
  {
  }

  internal LayoutedGroupCharacterWidget(LayoutedGroupCharacterWidget srcWidget)
    : base((LayoutedFuntionWidget) srcWidget)
  {
    this.GroupCharacter = new LayoutedStringWidget(srcWidget.GroupCharacter);
    this.Equation = new LayoutedOMathWidget(srcWidget.Equation);
  }

  internal LayoutedStringWidget GroupCharacter
  {
    get => this.m_groupCharacter;
    set => this.m_groupCharacter = value;
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
    this.GroupCharacter.Bounds = new RectangleF(this.GroupCharacter.Bounds.X + xPosition, this.GroupCharacter.Bounds.Y + yPosition, this.GroupCharacter.Bounds.Width, this.GroupCharacter.Bounds.Height);
    this.Equation.ShiftXYPosition(xPosition, yPosition);
  }

  public override void Dispose()
  {
    if (this.Equation != null)
    {
      this.Equation.Dispose();
      this.Equation = (LayoutedOMathWidget) null;
    }
    if (this.GroupCharacter != null)
    {
      this.GroupCharacter.Dispose();
      this.GroupCharacter = (LayoutedStringWidget) null;
    }
    base.Dispose();
  }
}
