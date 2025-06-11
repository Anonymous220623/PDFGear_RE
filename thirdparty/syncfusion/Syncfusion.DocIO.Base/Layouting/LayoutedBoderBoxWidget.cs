// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedBoderBoxWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedBoderBoxWidget : LayoutedFuntionWidget
{
  private List<LayoutedLineWidget> m_borderLines;
  private LayoutedOMathWidget m_equation;

  internal LayoutedBoderBoxWidget(IOfficeMathFunctionBase widget)
    : base(widget)
  {
  }

  internal LayoutedBoderBoxWidget(LayoutedBoderBoxWidget srcWidget)
    : base((LayoutedFuntionWidget) srcWidget)
  {
    if (srcWidget.Equation != null)
      this.Equation = new LayoutedOMathWidget(srcWidget.Equation);
    if (srcWidget.BorderLines == null)
      return;
    this.BorderLines = new List<LayoutedLineWidget>((IEnumerable<LayoutedLineWidget>) srcWidget.BorderLines);
  }

  internal List<LayoutedLineWidget> BorderLines
  {
    get => this.m_borderLines;
    set => this.m_borderLines = value;
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
    foreach (LayoutedLineWidget borderLine in this.BorderLines)
      borderLine.ShiftXYPosition(xPosition, yPosition);
  }

  public override void Dispose()
  {
    if (this.Equation != null)
    {
      this.Equation.Dispose();
      this.Equation = (LayoutedOMathWidget) null;
    }
    if (this.BorderLines != null)
    {
      for (int index = 0; index < this.BorderLines.Count; ++index)
      {
        this.BorderLines[index].Dispose();
        this.BorderLines[index] = (LayoutedLineWidget) null;
      }
      this.BorderLines.Clear();
      this.BorderLines = (List<LayoutedLineWidget>) null;
    }
    base.Dispose();
  }
}
