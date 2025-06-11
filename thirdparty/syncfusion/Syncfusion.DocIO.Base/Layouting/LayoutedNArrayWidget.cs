// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedNArrayWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedNArrayWidget : LayoutedFuntionWidget
{
  private LayoutedOMathWidget m_superscript;
  private LayoutedOMathWidget m_subscript;
  private LayoutedOMathWidget m_equation;
  private LayoutedStringWidget m_narrayCharacter;

  internal LayoutedNArrayWidget(IOfficeMathFunctionBase widget)
    : base(widget)
  {
  }

  internal LayoutedNArrayWidget(LayoutedNArrayWidget srcWidget)
    : base((LayoutedFuntionWidget) srcWidget)
  {
    if (srcWidget.Superscript != null)
      this.Superscript = new LayoutedOMathWidget(srcWidget.Superscript);
    if (srcWidget.Subscript != null)
      this.Subscript = new LayoutedOMathWidget(srcWidget.Subscript);
    this.Equation = new LayoutedOMathWidget(srcWidget.Equation);
    this.NArrayCharacter = new LayoutedStringWidget(srcWidget.NArrayCharacter);
  }

  internal LayoutedOMathWidget Superscript
  {
    get => this.m_superscript;
    set => this.m_superscript = value;
  }

  internal LayoutedOMathWidget Subscript
  {
    get => this.m_subscript;
    set => this.m_subscript = value;
  }

  internal LayoutedOMathWidget Equation
  {
    get => this.m_equation;
    set => this.m_equation = value;
  }

  internal LayoutedStringWidget NArrayCharacter
  {
    get => this.m_narrayCharacter;
    set => this.m_narrayCharacter = value;
  }

  public override void ShiftXYPosition(float xPosition, float yPosition)
  {
    this.Bounds = new RectangleF(this.Bounds.X + xPosition, this.Bounds.Y + yPosition, this.Bounds.Width, this.Bounds.Height);
    IOfficeMathNArray widget = this.Widget as IOfficeMathNArray;
    if (!widget.HideUpperLimit)
      this.Superscript.ShiftXYPosition(xPosition, yPosition);
    if (!widget.HideLowerLimit)
      this.Subscript.ShiftXYPosition(xPosition, yPosition);
    this.Equation.ShiftXYPosition(xPosition, yPosition);
    this.NArrayCharacter.ShiftXYPosition(xPosition, yPosition);
  }

  public override void Dispose()
  {
    if (this.Superscript != null)
    {
      this.Superscript.Dispose();
      this.Superscript = (LayoutedOMathWidget) null;
    }
    if (this.Subscript != null)
    {
      this.Subscript.Dispose();
      this.Subscript = (LayoutedOMathWidget) null;
    }
    if (this.Equation != null)
    {
      this.Equation.Dispose();
      this.Equation = (LayoutedOMathWidget) null;
    }
    if (this.NArrayCharacter != null)
    {
      this.NArrayCharacter.Dispose();
      this.NArrayCharacter = (LayoutedStringWidget) null;
    }
    base.Dispose();
  }
}
