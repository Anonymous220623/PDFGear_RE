// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedFractionWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedFractionWidget : LayoutedFuntionWidget
{
  private LayoutedOMathWidget m_numerator;
  private LayoutedOMathWidget m_denominator;
  private LayoutedLineWidget m_fractionLine;

  internal LayoutedFractionWidget(IOfficeMathFunctionBase widget)
    : base(widget)
  {
  }

  internal LayoutedFractionWidget(LayoutedFractionWidget srcWidget)
    : base((LayoutedFuntionWidget) srcWidget)
  {
    this.Numerator = new LayoutedOMathWidget(srcWidget.Numerator);
    this.Denominator = new LayoutedOMathWidget(srcWidget.Denominator);
    this.FractionLine = new LayoutedLineWidget(srcWidget.FractionLine);
  }

  internal LayoutedOMathWidget Numerator
  {
    get => this.m_numerator;
    set => this.m_numerator = value;
  }

  internal LayoutedOMathWidget Denominator
  {
    get => this.m_denominator;
    set => this.m_denominator = value;
  }

  internal LayoutedLineWidget FractionLine
  {
    get => this.m_fractionLine;
    set => this.m_fractionLine = value;
  }

  public override void ShiftXYPosition(float xPosition, float yPosition)
  {
    this.Bounds = new RectangleF(this.Bounds.X + xPosition, this.Bounds.Y + yPosition, this.Bounds.Width, this.Bounds.Height);
    this.Numerator.ShiftXYPosition(xPosition, yPosition);
    this.Denominator.ShiftXYPosition(xPosition, yPosition);
    this.FractionLine.ShiftXYPosition(xPosition, yPosition);
  }

  public override void Dispose()
  {
    if (this.Numerator != null)
    {
      this.Numerator.Dispose();
      this.Numerator = (LayoutedOMathWidget) null;
    }
    if (this.Denominator != null)
    {
      this.Denominator.Dispose();
      this.Denominator = (LayoutedOMathWidget) null;
    }
    if (this.FractionLine != null)
    {
      this.FractionLine.Dispose();
      this.FractionLine = (LayoutedLineWidget) null;
    }
    base.Dispose();
  }
}
