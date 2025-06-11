// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedEquationArrayWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedEquationArrayWidget : LayoutedFuntionWidget
{
  private List<List<LayoutedOMathWidget>> m_equation;

  internal LayoutedEquationArrayWidget(IOfficeMathFunctionBase widget)
    : base(widget)
  {
  }

  internal LayoutedEquationArrayWidget(LayoutedEquationArrayWidget srcWidget)
    : base((LayoutedFuntionWidget) srcWidget)
  {
    List<List<LayoutedOMathWidget>> layoutedOmathWidgetListList = new List<List<LayoutedOMathWidget>>();
    for (int index1 = 0; index1 < srcWidget.Equation.Count; ++index1)
    {
      List<LayoutedOMathWidget> layoutedOmathWidgetList = new List<LayoutedOMathWidget>();
      for (int index2 = 0; index2 < srcWidget.Equation[index1].Count; ++index2)
        layoutedOmathWidgetList.Add(new LayoutedOMathWidget(srcWidget.Equation[index1][index2]));
    }
    this.Equation = layoutedOmathWidgetListList;
  }

  internal List<List<LayoutedOMathWidget>> Equation
  {
    get => this.m_equation;
    set => this.m_equation = value;
  }

  public override void ShiftXYPosition(float xPosition, float yPosition)
  {
    this.Bounds = new RectangleF(this.Bounds.X + xPosition, this.Bounds.Y + yPosition, this.Bounds.Width, this.Bounds.Height);
    for (int index1 = 0; index1 < this.Equation.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.Equation[index1].Count; ++index2)
        this.Equation[index1][index2].ShiftXYPosition(xPosition, yPosition);
    }
  }

  public override void Dispose()
  {
    if (this.Equation != null)
    {
      for (int index1 = 0; index1 < this.Equation.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.Equation[index1].Count; ++index2)
        {
          this.Equation[index1][index2].Dispose();
          this.Equation[index1][index2] = (LayoutedOMathWidget) null;
        }
        this.Equation[index1].Clear();
      }
      this.Equation.Clear();
      this.Equation = (List<List<LayoutedOMathWidget>>) null;
    }
    base.Dispose();
  }
}
