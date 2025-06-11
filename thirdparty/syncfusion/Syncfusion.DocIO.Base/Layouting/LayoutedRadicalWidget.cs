// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedRadicalWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedRadicalWidget : LayoutedFuntionWidget
{
  private LayoutedOMathWidget m_degree;
  private LayoutedOMathWidget m_equation;
  private LayoutedLineWidget[] m_radicalLines;

  internal LayoutedRadicalWidget(IOfficeMathFunctionBase widget)
    : base(widget)
  {
  }

  internal LayoutedRadicalWidget(LayoutedRadicalWidget srcWidget)
    : base((LayoutedFuntionWidget) srcWidget)
  {
    if (srcWidget.Degree != null)
      this.Degree = new LayoutedOMathWidget(srcWidget.Degree);
    this.Equation = new LayoutedOMathWidget(srcWidget.Equation);
    LayoutedLineWidget[] layoutedLineWidgetArray = new LayoutedLineWidget[srcWidget.RadicalLines.Length];
    for (int index = 0; index < srcWidget.RadicalLines.Length; ++index)
      layoutedLineWidgetArray[index] = new LayoutedLineWidget(srcWidget.RadicalLines[index]);
    this.RadicalLines = layoutedLineWidgetArray;
  }

  internal LayoutedOMathWidget Degree
  {
    get => this.m_degree;
    set => this.m_degree = value;
  }

  internal LayoutedOMathWidget Equation
  {
    get => this.m_equation;
    set => this.m_equation = value;
  }

  internal LayoutedLineWidget[] RadicalLines
  {
    get => this.m_radicalLines;
    set => this.m_radicalLines = value;
  }

  public override void ShiftXYPosition(float xPosition, float yPosition)
  {
    this.Bounds = new RectangleF(this.Bounds.X + xPosition, this.Bounds.Y + yPosition, this.Bounds.Width, this.Bounds.Height);
    this.Equation.ShiftXYPosition(xPosition, yPosition);
    if (this.Degree != null)
      this.Degree.ShiftXYPosition(xPosition, yPosition);
    foreach (LayoutedLineWidget radicalLine in this.RadicalLines)
      radicalLine.ShiftXYPosition(xPosition, yPosition);
  }

  public override void Dispose()
  {
    if (this.Equation != null)
    {
      this.Equation.Dispose();
      this.Equation = (LayoutedOMathWidget) null;
    }
    if (this.Degree != null)
    {
      this.Degree.Dispose();
      this.Degree = (LayoutedOMathWidget) null;
    }
    if (this.RadicalLines != null)
    {
      for (int index = 0; index < this.RadicalLines.Length; ++index)
      {
        this.RadicalLines[index].Dispose();
        this.RadicalLines[index] = (LayoutedLineWidget) null;
      }
      this.RadicalLines = (LayoutedLineWidget[]) null;
    }
    base.Dispose();
  }
}
