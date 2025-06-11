// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedMathFunctionWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedMathFunctionWidget : LayoutedFuntionWidget
{
  private LayoutedOMathWidget m_functionName;
  private LayoutedOMathWidget m_equation;

  internal LayoutedMathFunctionWidget(IOfficeMathFunctionBase widget)
    : base(widget)
  {
  }

  internal LayoutedMathFunctionWidget(LayoutedMathFunctionWidget srcWidget)
    : base((LayoutedFuntionWidget) srcWidget)
  {
    if (srcWidget.FunctionName != null)
      this.FunctionName = new LayoutedOMathWidget(srcWidget.FunctionName);
    if (srcWidget.Equation == null)
      return;
    this.Equation = new LayoutedOMathWidget(srcWidget.Equation);
  }

  internal LayoutedOMathWidget FunctionName
  {
    get => this.m_functionName;
    set => this.m_functionName = value;
  }

  internal LayoutedOMathWidget Equation
  {
    get => this.m_equation;
    set => this.m_equation = value;
  }

  public override void ShiftXYPosition(float xPosition, float yPosition)
  {
    this.Bounds = new RectangleF(this.Bounds.X + xPosition, this.Bounds.Y + yPosition, this.Bounds.Width, this.Bounds.Height);
    this.FunctionName.ShiftXYPosition(xPosition, yPosition);
    this.Equation.ShiftXYPosition(xPosition, yPosition);
  }

  public override void Dispose()
  {
    if (this.FunctionName != null)
    {
      this.FunctionName.Dispose();
      this.FunctionName = (LayoutedOMathWidget) null;
    }
    if (this.Equation != null)
    {
      this.Equation.Dispose();
      this.Equation = (LayoutedOMathWidget) null;
    }
    base.Dispose();
  }
}
