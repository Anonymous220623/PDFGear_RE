// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedLimitWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedLimitWidget : LayoutedFuntionWidget
{
  private LayoutedOMathWidget m_limit;
  private LayoutedOMathWidget m_equation;

  internal LayoutedLimitWidget(IOfficeMathFunctionBase widget)
    : base(widget)
  {
  }

  internal LayoutedLimitWidget(LayoutedLimitWidget srcWidget)
    : base((LayoutedFuntionWidget) srcWidget)
  {
    if (srcWidget.Limit != null)
      this.Limit = new LayoutedOMathWidget(srcWidget.Limit);
    if (srcWidget.Equation == null)
      return;
    this.Equation = new LayoutedOMathWidget(srcWidget.Equation);
  }

  internal LayoutedOMathWidget Limit
  {
    get => this.m_limit;
    set => this.m_limit = value;
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
    this.Limit.ShiftXYPosition(xPosition, yPosition);
  }

  public override void Dispose()
  {
    if (this.Equation != null)
    {
      this.Equation.Dispose();
      this.Equation = (LayoutedOMathWidget) null;
    }
    if (this.Limit != null)
    {
      this.Limit.Dispose();
      this.Limit = (LayoutedOMathWidget) null;
    }
    base.Dispose();
  }
}
