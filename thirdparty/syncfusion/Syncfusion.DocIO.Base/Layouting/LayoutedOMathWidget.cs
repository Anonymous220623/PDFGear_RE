// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedOMathWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedOMathWidget
{
  private List<ILayoutedFuntionWidget> m_layoutedFucntionWidgetList;
  private RectangleF m_bounds = RectangleF.Empty;
  private IOfficeMath m_widget;
  private LayoutedMathWidget m_owner;

  internal LayoutedOMathWidget(IOfficeMath widget)
  {
    this.m_widget = widget;
    this.m_layoutedFucntionWidgetList = new List<ILayoutedFuntionWidget>();
  }

  internal LayoutedOMathWidget(LayoutedOMathWidget srcWidget)
  {
    this.Bounds = srcWidget.Bounds;
    this.m_widget = srcWidget.Widget;
    this.m_layoutedFucntionWidgetList = new List<ILayoutedFuntionWidget>();
    for (int index = 0; index < srcWidget.ChildWidgets.Count; ++index)
    {
      IOfficeMathFunctionBase widget = srcWidget.ChildWidgets[index].Widget;
      ILayoutedFuntionWidget layoutedFuntionWidget = (ILayoutedFuntionWidget) null;
      switch (widget.Type)
      {
        case MathFunctionType.Accent:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) new LayoutedAccentWidget(srcWidget.ChildWidgets[index] as LayoutedAccentWidget);
          break;
        case MathFunctionType.Bar:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) new LayoutedBarWidget(srcWidget.ChildWidgets[index] as LayoutedBarWidget);
          break;
        case MathFunctionType.BorderBox:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) new LayoutedBoderBoxWidget(srcWidget.ChildWidgets[index] as LayoutedBoderBoxWidget);
          break;
        case MathFunctionType.Box:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) new LayoutedBoxWidget(srcWidget.ChildWidgets[index] as LayoutedBoxWidget);
          break;
        case MathFunctionType.Delimiter:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) new LayoutedDelimiterWidget(srcWidget.ChildWidgets[index] as LayoutedDelimiterWidget);
          break;
        case MathFunctionType.EquationArray:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) new LayoutedEquationArrayWidget(srcWidget.ChildWidgets[index] as LayoutedEquationArrayWidget);
          break;
        case MathFunctionType.Fraction:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) new LayoutedFractionWidget(srcWidget.ChildWidgets[index] as LayoutedFractionWidget);
          break;
        case MathFunctionType.Function:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) new LayoutedMathFunctionWidget(srcWidget.ChildWidgets[index] as LayoutedMathFunctionWidget);
          break;
        case MathFunctionType.GroupCharacter:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) new LayoutedGroupCharacterWidget(srcWidget.ChildWidgets[index] as LayoutedGroupCharacterWidget);
          break;
        case MathFunctionType.Limit:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) new LayoutedLimitWidget(srcWidget.ChildWidgets[index] as LayoutedLimitWidget);
          break;
        case MathFunctionType.Matrix:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) new LayoutedMatrixWidget(srcWidget.ChildWidgets[index] as LayoutedMatrixWidget);
          break;
        case MathFunctionType.NArray:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) new LayoutedNArrayWidget(srcWidget.ChildWidgets[index] as LayoutedNArrayWidget);
          break;
        case MathFunctionType.Phantom:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) new LayoutedPhantomWidget(srcWidget.ChildWidgets[index] as LayoutedPhantomWidget);
          break;
        case MathFunctionType.Radical:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) new LayoutedRadicalWidget(srcWidget.ChildWidgets[index] as LayoutedRadicalWidget);
          break;
        case MathFunctionType.LeftSubSuperscript:
        case MathFunctionType.SubSuperscript:
        case MathFunctionType.RightSubSuperscript:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) new LayoutedScriptWidget(srcWidget.ChildWidgets[index] as LayoutedScriptWidget);
          break;
        case MathFunctionType.RunElement:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) new LayoutedOfficeRunWidget(srcWidget.ChildWidgets[index]);
          break;
      }
      this.ChildWidgets.Add(layoutedFuntionWidget);
    }
  }

  internal List<ILayoutedFuntionWidget> ChildWidgets => this.m_layoutedFucntionWidgetList;

  internal RectangleF Bounds
  {
    get => this.m_bounds;
    set => this.m_bounds = value;
  }

  internal IOfficeMath Widget => this.m_widget;

  internal LayoutedMathWidget Owner
  {
    get => this.m_owner;
    set => this.m_owner = value;
  }

  internal void Dispose()
  {
    if (this.m_layoutedFucntionWidgetList != null)
    {
      for (int index = 0; index < this.m_layoutedFucntionWidgetList.Count; ++index)
      {
        this.m_layoutedFucntionWidgetList[index].Dispose();
        this.m_layoutedFucntionWidgetList[index] = (ILayoutedFuntionWidget) null;
      }
      this.m_layoutedFucntionWidgetList.Clear();
      this.m_layoutedFucntionWidgetList = (List<ILayoutedFuntionWidget>) null;
    }
    this.m_widget = (IOfficeMath) null;
    this.m_owner = (LayoutedMathWidget) null;
  }

  internal void ShiftXYPosition(float xPosition, float yPosition, bool isSkipOwnerContainer)
  {
    if (!isSkipOwnerContainer)
      this.Bounds = new RectangleF(this.Bounds.X + xPosition, this.Bounds.Y + yPosition, this.Bounds.Width, this.Bounds.Height);
    foreach (ILayoutedFuntionWidget childWidget in this.ChildWidgets)
      childWidget.ShiftXYPosition(xPosition, yPosition);
  }

  internal void ShiftXYPosition(float xPosition, float yPosition)
  {
    this.ShiftXYPosition(xPosition, yPosition, false);
  }

  internal float GetVerticalCenterPoint() => this.GetVerticalCenterPoint(out int _);

  internal float GetVerticalCenterPoint(out int maxHeightWidgetIndex)
  {
    float num = 0.0f;
    maxHeightWidgetIndex = 0;
    if (this.ChildWidgets.Count == 0)
      return 0.0f;
    for (int index = 0; index < this.ChildWidgets.Count; ++index)
    {
      ILayoutedFuntionWidget childWidget = this.ChildWidgets[index];
      if ((double) num < (double) childWidget.Bounds.Height)
      {
        num = childWidget.Bounds.Height;
        maxHeightWidgetIndex = index;
      }
    }
    return this.GetVerticalCenterPoint(this.ChildWidgets[maxHeightWidgetIndex]);
  }

  internal float GetVerticalCenterPoint(ILayoutedFuntionWidget layoutedFuntionWidget)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    int index1 = 0;
    RectangleF bounds;
    switch (layoutedFuntionWidget.Widget.Type)
    {
      case MathFunctionType.Accent:
        LayoutedAccentWidget layoutedAccentWidget = layoutedFuntionWidget as LayoutedAccentWidget;
        num1 = layoutedAccentWidget.Bounds.Y + layoutedAccentWidget.Equation.Bounds.Height / 2f;
        break;
      case MathFunctionType.Bar:
      case MathFunctionType.BorderBox:
      case MathFunctionType.Phantom:
      case MathFunctionType.RunElement:
        num1 = layoutedFuntionWidget.Bounds.Y + layoutedFuntionWidget.Bounds.Height / 2f;
        break;
      case MathFunctionType.Box:
        num1 = layoutedFuntionWidget.Bounds.Y + layoutedFuntionWidget.Bounds.Height / 2f;
        break;
      case MathFunctionType.Delimiter:
        float num3 = 0.0f;
        int index2 = 0;
        LayoutedDelimiterWidget layoutedDelimiterWidget = layoutedFuntionWidget as LayoutedDelimiterWidget;
        OfficeMathDelimiter widget1 = layoutedDelimiterWidget.Widget as OfficeMathDelimiter;
        float num4;
        if (widget1.IsGrow && widget1.DelimiterShape == MathDelimiterShapeType.Centered)
        {
          num4 = layoutedDelimiterWidget.Bounds.Height / 2f;
        }
        else
        {
          for (int index3 = 0; index3 < layoutedDelimiterWidget.Equation.Count; ++index3)
          {
            if ((double) num3 < (double) layoutedDelimiterWidget.Equation[index3].Bounds.Height)
            {
              num3 = layoutedDelimiterWidget.Equation[index3].Bounds.Height;
              index2 = index3;
            }
          }
          num4 = layoutedDelimiterWidget.Equation[index2].GetVerticalCenterPoint();
        }
        num1 = num4 + layoutedFuntionWidget.Bounds.Y;
        break;
      case MathFunctionType.EquationArray:
        IOfficeMathEquationArray widget2 = layoutedFuntionWidget.Widget as IOfficeMathEquationArray;
        switch (widget2.VerticalAlignment)
        {
          case MathVerticalAlignment.Center:
            bounds = layoutedFuntionWidget.Bounds;
            num1 = bounds.Y + layoutedFuntionWidget.Bounds.Height / 2f;
            break;
          case MathVerticalAlignment.Top:
          case MathVerticalAlignment.Bottom:
            List<LayoutedOMathWidget> layoutedOmathWidgetList1 = widget2.VerticalAlignment != MathVerticalAlignment.Top ? (layoutedFuntionWidget as LayoutedEquationArrayWidget).Equation[widget2.Equation.Count - 1] : (layoutedFuntionWidget as LayoutedEquationArrayWidget).Equation[0];
            float num5 = float.MaxValue;
            for (int index4 = 0; index4 < layoutedOmathWidgetList1.Count; ++index4)
            {
              if ((double) num2 < (double) layoutedOmathWidgetList1[index4].Bounds.Height)
              {
                num2 = layoutedOmathWidgetList1[index4].Bounds.Height;
                index1 = index4;
              }
              if ((double) layoutedOmathWidgetList1[index4].Bounds.Y < (double) num5)
                num5 = layoutedOmathWidgetList1[index4].Bounds.Y;
            }
            float verticalCenterPoint1 = layoutedOmathWidgetList1[index1].GetVerticalCenterPoint();
            num1 = widget2.VerticalAlignment != MathVerticalAlignment.Top ? verticalCenterPoint1 + num5 : verticalCenterPoint1 + layoutedFuntionWidget.Bounds.Y;
            break;
        }
        break;
      case MathFunctionType.Fraction:
        switch ((layoutedFuntionWidget.Widget as IOfficeMathFraction).FractionType)
        {
          case MathFractionType.NormalFractionBar:
          case MathFractionType.NoFractionBar:
            num1 = (layoutedFuntionWidget as LayoutedFractionWidget).FractionLine.Point1.Y + (layoutedFuntionWidget as LayoutedFractionWidget).FractionLine.Width / 2f;
            break;
          case MathFractionType.SkewedFractionBar:
          case MathFractionType.FractionInline:
            num1 = (layoutedFuntionWidget as LayoutedFractionWidget).FractionLine.Point1.Y + (float) (((double) (layoutedFuntionWidget as LayoutedFractionWidget).FractionLine.Point2.Y - (double) (layoutedFuntionWidget as LayoutedFractionWidget).FractionLine.Point1.Y) / 2.0);
            break;
        }
        break;
      case MathFunctionType.Function:
        LayoutedMathFunctionWidget mathFunctionWidget = layoutedFuntionWidget as LayoutedMathFunctionWidget;
        num1 = mathFunctionWidget.Equation.Bounds.Y + mathFunctionWidget.Equation.Bounds.Height / 2f;
        break;
      case MathFunctionType.GroupCharacter:
        LayoutedGroupCharacterWidget groupCharacterWidget = layoutedFuntionWidget as LayoutedGroupCharacterWidget;
        num1 = groupCharacterWidget.Bounds.Y + groupCharacterWidget.Equation.Bounds.Height / 2f;
        break;
      case MathFunctionType.Limit:
        LayoutedLimitWidget layoutedLimitWidget = layoutedFuntionWidget as LayoutedLimitWidget;
        num1 = layoutedLimitWidget.Equation.Bounds.Y + layoutedLimitWidget.Equation.Bounds.Height / 2f;
        break;
      case MathFunctionType.Matrix:
        IOfficeMathMatrix widget3 = layoutedFuntionWidget.Widget as IOfficeMathMatrix;
        switch (widget3.VerticalAlignment)
        {
          case MathVerticalAlignment.Center:
            num1 = layoutedFuntionWidget.Bounds.Y + layoutedFuntionWidget.Bounds.Height / 2f;
            break;
          case MathVerticalAlignment.Top:
          case MathVerticalAlignment.Bottom:
            List<LayoutedOMathWidget> layoutedOmathWidgetList2 = widget3.VerticalAlignment != MathVerticalAlignment.Top ? (layoutedFuntionWidget as LayoutedMatrixWidget).Rows[(layoutedFuntionWidget as LayoutedMatrixWidget).Rows.Count - 1] : (layoutedFuntionWidget as LayoutedMatrixWidget).Rows[0];
            float num6 = 0.0f;
            float num7 = float.MaxValue;
            int index5 = 0;
            for (int index6 = 0; index6 < layoutedOmathWidgetList2.Count; ++index6)
            {
              if ((double) num6 < (double) layoutedOmathWidgetList2[index6].Bounds.Height)
              {
                num6 = layoutedOmathWidgetList2[index6].Bounds.Height;
                index5 = index6;
              }
              if ((double) layoutedOmathWidgetList2[index6].Bounds.Y < (double) num7)
                num7 = layoutedOmathWidgetList2[index6].Bounds.Y;
            }
            float verticalCenterPoint2 = layoutedOmathWidgetList2[index5].GetVerticalCenterPoint();
            num1 = widget3.VerticalAlignment != MathVerticalAlignment.Top ? verticalCenterPoint2 + num7 : verticalCenterPoint2 + layoutedFuntionWidget.Bounds.Y;
            break;
        }
        break;
      case MathFunctionType.NArray:
        LayoutedNArrayWidget layoutedNarrayWidget = layoutedFuntionWidget as LayoutedNArrayWidget;
        double y1 = (double) layoutedNarrayWidget.Equation.Bounds.Y;
        bounds = layoutedNarrayWidget.Equation.Bounds;
        double num8 = (double) bounds.Height / 2.0;
        num1 = (float) (y1 + num8);
        break;
      case MathFunctionType.Radical:
        LayoutedRadicalWidget layoutedRadicalWidget = layoutedFuntionWidget as LayoutedRadicalWidget;
        num1 = layoutedRadicalWidget.Equation.Bounds.Y + layoutedRadicalWidget.Equation.Bounds.Height / 2f;
        break;
      case MathFunctionType.LeftSubSuperscript:
      case MathFunctionType.SubSuperscript:
      case MathFunctionType.RightSubSuperscript:
        LayoutedScriptWidget layoutedScriptWidget = layoutedFuntionWidget as LayoutedScriptWidget;
        num1 = layoutedScriptWidget.Equation.Bounds.Y + layoutedScriptWidget.Equation.Bounds.Height / 2f;
        break;
    }
    double num9 = (double) num1;
    bounds = layoutedFuntionWidget.Bounds;
    double y2 = (double) bounds.Y;
    return (float) (num9 - y2);
  }
}
