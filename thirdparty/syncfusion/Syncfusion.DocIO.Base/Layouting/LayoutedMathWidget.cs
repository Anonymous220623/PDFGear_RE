// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedMathWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.Office;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedMathWidget : LayoutedWidget
{
  private List<LayoutedOMathWidget> m_layoutedOMathWidgetList;

  internal LayoutedMathWidget(IWidget widget)
    : base(widget)
  {
    this.m_layoutedOMathWidgetList = new List<LayoutedOMathWidget>();
  }

  internal LayoutedMathWidget(LayoutedWidget srcWidget)
    : base(srcWidget)
  {
    this.m_layoutedOMathWidgetList = new List<LayoutedOMathWidget>();
    LayoutedMathWidget layoutedMathWidget = srcWidget as LayoutedMathWidget;
    for (int index = 0; index < layoutedMathWidget.ChildWidgets.Count; ++index)
      this.ChildWidgets.Add(new LayoutedOMathWidget(layoutedMathWidget.ChildWidgets[index]));
  }

  internal List<LayoutedOMathWidget> ChildWidgets => this.m_layoutedOMathWidgetList;

  internal void Dispose()
  {
    if (this.m_layoutedOMathWidgetList == null)
      return;
    for (int index = 0; index < this.m_layoutedOMathWidgetList.Count; ++index)
    {
      this.m_layoutedOMathWidgetList[index].Dispose();
      this.m_layoutedOMathWidgetList[index] = (LayoutedOMathWidget) null;
    }
    this.m_layoutedOMathWidgetList.Clear();
    this.m_layoutedOMathWidgetList = (List<LayoutedOMathWidget>) null;
  }

  internal void ShiftXYPosition(float xPosition, float yPosition, bool isSkipOwnerContainer)
  {
    if (!isSkipOwnerContainer)
      this.Bounds = new RectangleF(this.Bounds.X + xPosition, this.Bounds.Y + yPosition, this.Bounds.Width, this.Bounds.Height);
    foreach (LayoutedOMathWidget childWidget in this.ChildWidgets)
      childWidget.ShiftXYPosition(xPosition, yPosition, isSkipOwnerContainer);
  }

  internal void ShiftXYPosition(float xPosition, float yPosition)
  {
    this.ShiftXYPosition(xPosition, yPosition, false);
  }

  internal Font GetFont()
  {
    int index1 = 0;
    float num = 0.0f;
    for (int index2 = 0; index2 < this.ChildWidgets.Count; ++index2)
    {
      if ((double) num < (double) this.ChildWidgets[index2].Bounds.Height)
      {
        num = this.ChildWidgets[index2].Bounds.Height;
        index1 = index2;
      }
    }
    IOfficeMathFunctionBase widget = this.ChildWidgets[index1].ChildWidgets[0].Widget;
    WCharacterFormat wcharacterFormat = (WCharacterFormat) null;
    switch (widget.Type)
    {
      case MathFunctionType.Accent:
        wcharacterFormat = (widget as IOfficeMathAccent).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Bar:
        wcharacterFormat = (widget as IOfficeMathBar).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.BorderBox:
        wcharacterFormat = (widget as IOfficeMathBorderBox).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Box:
        wcharacterFormat = (widget as IOfficeMathBox).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Delimiter:
        wcharacterFormat = (widget as IOfficeMathDelimiter).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.EquationArray:
        wcharacterFormat = (widget as IOfficeMathEquationArray).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Fraction:
        wcharacterFormat = (widget as IOfficeMathFraction).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Function:
        wcharacterFormat = (widget as IOfficeMathFunction).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.GroupCharacter:
        wcharacterFormat = (widget as IOfficeMathGroupCharacter).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Limit:
        wcharacterFormat = (widget as IOfficeMathLimit).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Matrix:
        wcharacterFormat = (widget as IOfficeMathMatrix).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.NArray:
        wcharacterFormat = (widget as IOfficeMathNArray).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Phantom:
        wcharacterFormat = (widget as IOfficeMathPhantom).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Radical:
        wcharacterFormat = (widget as IOfficeMathRadical).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.LeftSubSuperscript:
        wcharacterFormat = (widget as IOfficeMathLeftScript).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.SubSuperscript:
        wcharacterFormat = (widget as IOfficeMathScript).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.RightSubSuperscript:
        wcharacterFormat = (widget as IOfficeMathRightScript).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.RunElement:
        if ((widget as IOfficeMathRunElement).Item is WTextRange)
        {
          wcharacterFormat = ((widget as IOfficeMathRunElement).Item as WTextRange).CharacterFormat;
          break;
        }
        break;
    }
    return wcharacterFormat.Font;
  }
}
