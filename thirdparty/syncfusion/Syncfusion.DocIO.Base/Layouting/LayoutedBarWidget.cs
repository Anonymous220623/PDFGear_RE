// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedBarWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedBarWidget : LayoutedFuntionWidget
{
  private LayoutedLineWidget m_barline;
  private LayoutedOMathWidget m_equation;

  internal LayoutedBarWidget(IOfficeMathFunctionBase widget)
    : base(widget)
  {
  }

  internal LayoutedBarWidget(LayoutedBarWidget srcWidget)
    : base((LayoutedFuntionWidget) srcWidget)
  {
    if (srcWidget.Equation != null)
      this.Equation = new LayoutedOMathWidget(srcWidget.Equation);
    if (srcWidget.BarLine == null)
      return;
    this.BarLine = new LayoutedLineWidget(srcWidget.BarLine);
  }

  internal LayoutedLineWidget BarLine
  {
    get => this.m_barline;
    set => this.m_barline = value;
  }

  internal LayoutedOMathWidget Equation
  {
    get => this.m_equation;
    set => this.m_equation = value;
  }

  public override void ShiftXYPosition(float xPosition, float yPosition)
  {
    this.Bounds = new RectangleF(this.Bounds.X + xPosition, this.Bounds.Y + yPosition, this.Bounds.Width, this.Bounds.Height);
    this.Equation.ShiftXYPosition(xPosition, yPosition);
    this.BarLine.ShiftXYPosition(xPosition, yPosition);
  }

  public override void Dispose()
  {
    if (this.Equation != null)
    {
      this.Equation.Dispose();
      this.Equation = (LayoutedOMathWidget) null;
    }
    if (this.BarLine != null)
    {
      this.BarLine.Dispose();
      this.BarLine = (LayoutedLineWidget) null;
    }
    base.Dispose();
  }
}
