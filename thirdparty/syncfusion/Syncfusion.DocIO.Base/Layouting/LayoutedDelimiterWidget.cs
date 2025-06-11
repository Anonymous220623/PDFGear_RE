// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedDelimiterWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedDelimiterWidget : LayoutedFuntionWidget
{
  private LayoutedStringWidget m_beginChar;
  private LayoutedStringWidget m_endChar;
  private LayoutedStringWidget m_seperator;
  private List<LayoutedOMathWidget> m_equation;

  internal LayoutedDelimiterWidget(IOfficeMathFunctionBase widget)
    : base(widget)
  {
  }

  internal LayoutedDelimiterWidget(LayoutedDelimiterWidget srcWidget)
    : base((LayoutedFuntionWidget) srcWidget)
  {
    if (srcWidget.BeginCharacter != null)
      this.BeginCharacter = new LayoutedStringWidget(srcWidget.BeginCharacter);
    if (srcWidget.EndCharacter != null)
      this.EndCharacter = new LayoutedStringWidget(srcWidget.EndCharacter);
    if (srcWidget.Seperator != null)
      this.Seperator = new LayoutedStringWidget(srcWidget.Seperator);
    List<LayoutedOMathWidget> layoutedOmathWidgetList = new List<LayoutedOMathWidget>();
    foreach (LayoutedOMathWidget srcWidget1 in srcWidget.Equation)
      layoutedOmathWidgetList.Add(new LayoutedOMathWidget(srcWidget1));
    this.Equation = layoutedOmathWidgetList;
  }

  internal LayoutedStringWidget BeginCharacter
  {
    get => this.m_beginChar;
    set => this.m_beginChar = value;
  }

  internal LayoutedStringWidget EndCharacter
  {
    get => this.m_endChar;
    set => this.m_endChar = value;
  }

  internal LayoutedStringWidget Seperator
  {
    get => this.m_seperator;
    set => this.m_seperator = value;
  }

  internal List<LayoutedOMathWidget> Equation
  {
    get => this.m_equation;
    set => this.m_equation = value;
  }

  public override void ShiftXYPosition(float xPosition, float yPosition)
  {
    this.Bounds = new RectangleF(this.Bounds.X + xPosition, this.Bounds.Y + yPosition, this.Bounds.Width, this.Bounds.Height);
    if (this.BeginCharacter != null)
      this.BeginCharacter.ShiftXYPosition(xPosition, yPosition);
    if (this.EndCharacter != null)
      this.EndCharacter.ShiftXYPosition(xPosition, yPosition);
    if (this.Seperator != null)
      this.Seperator.ShiftXYPosition(xPosition, yPosition);
    foreach (LayoutedOMathWidget layoutedOmathWidget in this.Equation)
      layoutedOmathWidget.ShiftXYPosition(xPosition, yPosition);
  }

  public override void Dispose()
  {
    if (this.BeginCharacter != null)
    {
      this.BeginCharacter.Dispose();
      this.BeginCharacter = (LayoutedStringWidget) null;
    }
    if (this.EndCharacter != null)
    {
      this.EndCharacter.Dispose();
      this.EndCharacter = (LayoutedStringWidget) null;
    }
    if (this.Seperator != null)
    {
      this.Seperator.Dispose();
      this.Seperator = (LayoutedStringWidget) null;
    }
    if (this.Equation != null)
    {
      for (int index = 0; index < this.Equation.Count; ++index)
      {
        this.Equation[index].Dispose();
        this.Equation[index] = (LayoutedOMathWidget) null;
      }
      this.Equation.Clear();
      this.Equation = (List<LayoutedOMathWidget>) null;
    }
    base.Dispose();
  }
}
