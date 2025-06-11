// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.MathLayoutContext
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.Office;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class MathLayoutContext : LayoutContext
{
  private WordDocument m_document;
  private SizeF m_containerSize;
  private List<char> stretchableCharacters;
  private Stack<MathFunctionType> m_mathLayoutingStack;
  private float operatorXPosition;
  private float lineMaxHeight;
  private float mathXPosition;

  internal MathLayoutContext(ILeafWidget widget, ILCOperator lcOperator, bool isForceFitLayout)
    : base((IWidget) widget, lcOperator, isForceFitLayout)
  {
    this.m_document = (widget as Entity).Document;
    this.m_mathLayoutingStack = new Stack<MathFunctionType>();
  }

  internal WMath MathWidget => this.Widget as WMath;

  internal WordDocument Document => this.m_document;

  internal SizeF ContainerSize
  {
    get => this.m_containerSize;
    set => this.m_containerSize = value;
  }

  private Stack<MathFunctionType> MathLayoutingStack => this.m_mathLayoutingStack;

  public override LayoutedWidget Layout(RectangleF rect)
  {
    bool layoutArea = this.CreateLayoutArea(ref rect);
    LayoutedMathWidget mathLayoutedWidget = this.CreateMathLayoutedWidget(rect.Location);
    this.ContainerSize = rect.Size;
    this.LayoutOfficeMathCollection(rect, mathLayoutedWidget, this.MathWidget.MathParagraph.Maths);
    this.m_ltState = LayoutState.Fitted;
    return !layoutArea && this.Document.Settings.MathProperties != null ? (LayoutedWidget) mathLayoutedWidget : (LayoutedWidget) this.DoMathAlignment(mathLayoutedWidget, rect);
  }

  private bool CreateLayoutArea(ref RectangleF rect)
  {
    if (this.Document.Settings.MathProperties == null || !this.Document.Settings.MathProperties.DisplayMathDefaults || this.MathWidget.IsInline)
      return false;
    rect.X += (float) this.Document.Settings.MathProperties.LeftMargin;
    rect.Width -= (float) this.Document.Settings.MathProperties.RightMargin;
    return true;
  }

  private LayoutedMathWidget DoMathAlignment(LayoutedMathWidget ltMathWidget, RectangleF clientArea)
  {
    MathJustification mathJustification = MathJustification.CenterGroup;
    if (this.Document.Settings.MathProperties != null)
      mathJustification = this.Document.Settings.MathProperties.DefaultJustification;
    if ((this.MathWidget.MathParagraph as OfficeMathParagraph).HasValue(77))
      mathJustification = this.MathWidget.MathParagraph.Justification;
    float xPosition = clientArea.Right - ltMathWidget.Bounds.Right;
    switch (mathJustification)
    {
      case MathJustification.CenterGroup:
      case MathJustification.Center:
        ltMathWidget.ShiftXYPosition(xPosition / 2f, 0.0f);
        break;
      case MathJustification.Right:
        ltMathWidget.ShiftXYPosition(xPosition, 0.0f);
        break;
    }
    return ltMathWidget;
  }

  private void LayoutOfficeMathFunctions(
    RectangleF clientArea,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathBaseCollection officeMathFunctions)
  {
    RectangleF rectangleF = new RectangleF(clientArea.Location, clientArea.Size);
    for (int index = 0; index < officeMathFunctions.Count; ++index)
    {
      ILayoutedFuntionWidget layoutedFuntionWidget = (ILayoutedFuntionWidget) null;
      IOfficeMathFunctionBase officeMathFunction = officeMathFunctions[index];
      this.MathLayoutingStack.Push(officeMathFunction.Type);
      switch (officeMathFunction.Type)
      {
        case MathFunctionType.Accent:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) this.LayoutAccentWidget(rectangleF, officeMathLayoutedWidget, officeMathFunction);
          break;
        case MathFunctionType.Bar:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) this.LayoutBarWidget(rectangleF, officeMathLayoutedWidget, officeMathFunction);
          break;
        case MathFunctionType.BorderBox:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) this.LayoutBoderBoxWidget(rectangleF, officeMathLayoutedWidget, officeMathFunction);
          break;
        case MathFunctionType.Box:
          layoutedFuntionWidget = this.LayoutBoxWidget(rectangleF, officeMathLayoutedWidget, officeMathFunction);
          break;
        case MathFunctionType.Delimiter:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) this.LayoutDelimiterSwitch(rectangleF, officeMathLayoutedWidget, officeMathFunction);
          break;
        case MathFunctionType.EquationArray:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) this.LayoutEquationArraySwitch(rectangleF, officeMathLayoutedWidget, officeMathFunction);
          break;
        case MathFunctionType.Fraction:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) this.LayoutFractionSwitch(rectangleF, officeMathLayoutedWidget, officeMathFunction);
          break;
        case MathFunctionType.Function:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) this.LayoutMathFunctionWidget(rectangleF, officeMathLayoutedWidget, officeMathFunction);
          break;
        case MathFunctionType.GroupCharacter:
          layoutedFuntionWidget = this.LayoutGroupCharacterWidget(rectangleF, officeMathLayoutedWidget, officeMathFunction);
          break;
        case MathFunctionType.Limit:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) this.LayoutLimitWidget(rectangleF, officeMathLayoutedWidget, officeMathFunction);
          break;
        case MathFunctionType.Matrix:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) this.LayoutMatrixWidget(rectangleF, officeMathLayoutedWidget, officeMathFunction);
          break;
        case MathFunctionType.NArray:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) this.LayoutNArrayWidget(rectangleF, officeMathLayoutedWidget, officeMathFunction);
          break;
        case MathFunctionType.Phantom:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) this.LayoutPhantomSwitch(rectangleF, officeMathLayoutedWidget, officeMathFunction);
          break;
        case MathFunctionType.Radical:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) this.LayoutRadicalSwitch(rectangleF, officeMathLayoutedWidget, officeMathFunction);
          break;
        case MathFunctionType.LeftSubSuperscript:
        case MathFunctionType.SubSuperscript:
        case MathFunctionType.RightSubSuperscript:
          layoutedFuntionWidget = (ILayoutedFuntionWidget) this.LayoutScriptWidget(rectangleF, officeMathLayoutedWidget, officeMathFunction);
          break;
        case MathFunctionType.RunElement:
          layoutedFuntionWidget = this.LayoutRunElement(rectangleF, officeMathLayoutedWidget, officeMathFunction, index);
          break;
      }
      if (index + 1 != officeMathFunctions.Count && (officeMathFunctions[index].Type != MathFunctionType.RunElement || officeMathFunctions[index + 1].Type != MathFunctionType.RunElement))
      {
        WCharacterFormat characterProperty = this.GetControlCharacterProperty(officeMathFunction);
        if (characterProperty != null)
        {
          float width = this.DrawingContext.MeasureString(" ", characterProperty.GetFontToRender(FontScriptType.English), (StringFormat) null).Width;
          layoutedFuntionWidget.Bounds = new RectangleF(layoutedFuntionWidget.Bounds.X, layoutedFuntionWidget.Bounds.Y, layoutedFuntionWidget.Bounds.Width + width, layoutedFuntionWidget.Bounds.Height);
        }
      }
      if (layoutedFuntionWidget != null)
      {
        rectangleF.X += layoutedFuntionWidget.Bounds.Width;
        rectangleF.Width -= layoutedFuntionWidget.Bounds.Width;
        layoutedFuntionWidget.Owner = officeMathLayoutedWidget;
        officeMathLayoutedWidget.Bounds = this.UpdateBounds(officeMathLayoutedWidget.Bounds, layoutedFuntionWidget.Bounds);
        officeMathLayoutedWidget.ChildWidgets.Add(layoutedFuntionWidget);
        if ((double) officeMathLayoutedWidget.Bounds.Height > (double) this.lineMaxHeight)
          this.lineMaxHeight = officeMathLayoutedWidget.Bounds.Height;
        if (layoutedFuntionWidget.Widget is IOfficeMathBox && (double) layoutedFuntionWidget.Bounds.Y > (double) rectangleF.Y)
        {
          rectangleF.X = layoutedFuntionWidget.Bounds.X + layoutedFuntionWidget.Bounds.Width;
          rectangleF.Y = layoutedFuntionWidget.Bounds.Y;
        }
      }
      int num = (int) this.MathLayoutingStack.Pop();
    }
    this.AlignOfficeMathWidgetVertically(officeMathLayoutedWidget);
  }

  private ILayoutedFuntionWidget LayoutGroupCharacterWidget(
    RectangleF clientActiveArea,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathFunctionBase mathFunction)
  {
    LayoutedGroupCharacterWidget groupCharacterWidget = new LayoutedGroupCharacterWidget(mathFunction);
    IOfficeMathGroupCharacter mathGroupCharacter = mathFunction as IOfficeMathGroupCharacter;
    WCharacterFormat controlProperties = mathGroupCharacter.ControlProperties as WCharacterFormat;
    float shiftY = 0.0f;
    groupCharacterWidget.GroupCharacter = this.GetStringltWidget(controlProperties, mathGroupCharacter.GroupCharacter, clientActiveArea);
    if (mathGroupCharacter.HasCharacterTop)
    {
      if (mathGroupCharacter.HasAlignTop)
      {
        float fontSizeRatio = 0.714285731f;
        this.ReduceFontSizeOfOfficeMath(mathGroupCharacter.Equation, fontSizeRatio);
        groupCharacterWidget.Equation = this.LayoutOfficeMath(clientActiveArea, mathGroupCharacter.Equation);
        groupCharacterWidget.Equation.ShiftXYPosition(0.0f, this.DrawingContext.GetAscent(controlProperties.Font));
      }
      else
      {
        groupCharacterWidget.Equation = this.LayoutOfficeMath(clientActiveArea, mathGroupCharacter.Equation);
        string equationText = this.GetEquationText(mathGroupCharacter.Equation);
        string baseChar = "a";
        if ((double) groupCharacterWidget.GroupCharacter.Bounds.Height >= (double) groupCharacterWidget.Equation.Bounds.Height)
          this.FindTextDifference(equationText, baseChar, controlProperties, groupCharacterWidget.Equation.Bounds, ref shiftY);
        else
          shiftY = groupCharacterWidget.Equation.Bounds.Height / 8f;
      }
    }
    else if (!mathGroupCharacter.HasAlignTop)
    {
      float fontSizeRatio = 0.714285731f;
      this.ReduceFontSizeOfOfficeMath(mathGroupCharacter.Equation, fontSizeRatio);
      groupCharacterWidget.Equation = this.LayoutOfficeMath(clientActiveArea, mathGroupCharacter.Equation);
      groupCharacterWidget.Equation.ShiftXYPosition(0.0f, (float) -((double) this.DrawingContext.GetAscent(controlProperties.Font) / 2.0));
    }
    else
    {
      groupCharacterWidget.Equation = this.LayoutOfficeMath(clientActiveArea, mathGroupCharacter.Equation);
      if ((double) groupCharacterWidget.Equation.Bounds.Height > (double) groupCharacterWidget.GroupCharacter.Bounds.Height)
        shiftY = (float) -((double) groupCharacterWidget.Equation.Bounds.Height / 1.5);
    }
    if (groupCharacterWidget.Equation == null)
      groupCharacterWidget.Equation = this.LayoutOfficeMath(clientActiveArea, mathGroupCharacter.Equation);
    if ((double) groupCharacterWidget.Equation.Bounds.Width > (double) groupCharacterWidget.GroupCharacter.Bounds.Width)
      groupCharacterWidget.ScalingFactor = groupCharacterWidget.Equation.Bounds.Width / groupCharacterWidget.GroupCharacter.Bounds.Width;
    RectangleF bounds = groupCharacterWidget.GroupCharacter.Bounds;
    groupCharacterWidget.GroupCharacter.Bounds = new RectangleF(bounds.X, bounds.Y - shiftY, bounds.Width, bounds.Height);
    groupCharacterWidget.Bounds = this.UpdateBounds(groupCharacterWidget.GroupCharacter.Bounds, groupCharacterWidget.Equation.Bounds);
    return (ILayoutedFuntionWidget) groupCharacterWidget;
  }

  private void FindTextDifference(
    string eqText,
    string baseChar,
    WCharacterFormat characterFormat,
    RectangleF bounds,
    ref float shiftY)
  {
    if (eqText != string.Empty)
    {
      SizeF size = this.DrawingContext.MeasureString(baseChar, characterFormat.Font, this.DrawingContext.StringFormt);
      float textTopPosition = (float) this.DrawingContext.GetTextTopPosition(eqText, characterFormat.Font, bounds.Size);
      if ((double) this.DrawingContext.GetTextTopPosition(baseChar, characterFormat.Font, size) <= (double) textTopPosition || (double) textTopPosition == -1.0)
        return;
      shiftY = bounds.Height / 6f;
    }
    else
      shiftY = bounds.Height / 6f;
  }

  private LayoutedLimitWidget LayoutLimitWidget(
    RectangleF clientActiveArea,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathFunctionBase mathFunction)
  {
    LayoutedLimitWidget layoutedLimitWidget = new LayoutedLimitWidget(mathFunction);
    IOfficeMathLimit officeMathLimit = mathFunction as IOfficeMathLimit;
    layoutedLimitWidget.Equation = this.LayoutOfficeMath(clientActiveArea, officeMathLimit.Equation);
    float fontSizeRatio = 0.714285731f;
    this.ReduceFontSizeOfOfficeMath(officeMathLimit.Limit, fontSizeRatio);
    layoutedLimitWidget.Limit = this.LayoutOfficeMath(clientActiveArea, officeMathLimit.Limit);
    RectangleF rectangleF = new RectangleF();
    if (officeMathLimit.LimitType == MathLimitType.UpperLimit)
    {
      Font fontToRender = (officeMathLimit.ControlProperties as WCharacterFormat).GetFontToRender(FontScriptType.English);
      layoutedLimitWidget.Equation.ShiftXYPosition(0.0f, layoutedLimitWidget.Limit.Bounds.Height - this.DrawingContext.GetDescent(fontToRender));
      rectangleF.Y = layoutedLimitWidget.Limit.Bounds.Y;
      rectangleF.Height = layoutedLimitWidget.Equation.Bounds.Bottom - clientActiveArea.Y;
    }
    else if (officeMathLimit.LimitType == MathLimitType.LowerLimit)
    {
      Font font = (officeMathLimit.ControlProperties as WCharacterFormat).Font;
      layoutedLimitWidget.Limit.ShiftXYPosition(0.0f, layoutedLimitWidget.Equation.Bounds.Height - this.DrawingContext.GetDescent(font));
      rectangleF.Y = layoutedLimitWidget.Equation.Bounds.Y;
      rectangleF.Height = layoutedLimitWidget.Limit.Bounds.Bottom - clientActiveArea.Y;
    }
    if ((double) layoutedLimitWidget.Equation.Bounds.Width > (double) layoutedLimitWidget.Limit.Bounds.Width)
    {
      float xPosition = (float) (((double) layoutedLimitWidget.Equation.Bounds.Width - (double) layoutedLimitWidget.Limit.Bounds.Width) / 2.0);
      layoutedLimitWidget.Limit.ShiftXYPosition(xPosition, 0.0f);
      rectangleF.X = layoutedLimitWidget.Equation.Bounds.X;
      rectangleF.Width = layoutedLimitWidget.Equation.Bounds.Width;
    }
    else if ((double) layoutedLimitWidget.Limit.Bounds.Width > (double) layoutedLimitWidget.Equation.Bounds.Width)
    {
      float xPosition = (float) (((double) layoutedLimitWidget.Limit.Bounds.Width - (double) layoutedLimitWidget.Equation.Bounds.Width) / 2.0);
      layoutedLimitWidget.Equation.ShiftXYPosition(xPosition, 0.0f);
      rectangleF.X = layoutedLimitWidget.Limit.Bounds.X;
      rectangleF.Width = layoutedLimitWidget.Limit.Bounds.Width;
    }
    else
    {
      rectangleF.X = layoutedLimitWidget.Equation.Bounds.X;
      rectangleF.Width = layoutedLimitWidget.Equation.Bounds.Width;
    }
    layoutedLimitWidget.Bounds = rectangleF;
    return layoutedLimitWidget;
  }

  private ILayoutedFuntionWidget LayoutBoxWidget(
    RectangleF clientActiveArea,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathFunctionBase mathFunction)
  {
    LayoutedBoxWidget layoutedBoxWidget = new LayoutedBoxWidget(mathFunction);
    IOfficeMathBox officeMathBox = mathFunction as IOfficeMathBox;
    layoutedBoxWidget.Equation = this.LayoutOfficeMath(clientActiveArea, officeMathBox.Equation);
    if (officeMathBox.OperatorEmulator && officeMathBox.Break != null)
    {
      float xPosition = this.mathXPosition - clientActiveArea.X;
      if (officeMathBox.Break.AlignAt == 1)
        xPosition = this.operatorXPosition - clientActiveArea.X;
      layoutedBoxWidget.Equation.ShiftXYPosition(xPosition, this.lineMaxHeight);
    }
    if ((double) this.operatorXPosition == 0.0)
      this.operatorXPosition = layoutedBoxWidget.Equation.Bounds.X;
    layoutedBoxWidget.Bounds = layoutedBoxWidget.Equation.Bounds;
    return (ILayoutedFuntionWidget) layoutedBoxWidget;
  }

  private LayoutedScriptWidget LayoutScriptWidget(
    RectangleF currentBounds,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathFunctionBase mathFunction)
  {
    LayoutedScriptWidget scriptWidget = new LayoutedScriptWidget(mathFunction);
    IOfficeMathScript script = mathFunction as IOfficeMathScript;
    IOfficeMathLeftScript leftScript = mathFunction as IOfficeMathLeftScript;
    IOfficeMathRightScript rightScript = mathFunction as IOfficeMathRightScript;
    if (this.IsNested())
    {
      float fontSizeRatio = 0.588235259f;
      if (script != null)
        this.ReduceFontSizeOfOfficeMath(script.Script, fontSizeRatio);
      else if (leftScript != null)
      {
        this.ReduceFontSizeOfOfficeMath(leftScript.Superscript, fontSizeRatio);
        this.ReduceFontSizeOfOfficeMath(leftScript.Subscript, fontSizeRatio);
      }
      else if (rightScript != null)
      {
        this.ReduceFontSizeOfOfficeMath(rightScript.Superscript, fontSizeRatio);
        this.ReduceFontSizeOfOfficeMath(rightScript.Subscript, fontSizeRatio);
      }
    }
    float x = currentBounds.X;
    float y = currentBounds.Y;
    switch (mathFunction.Type)
    {
      case MathFunctionType.LeftSubSuperscript:
        Font font1 = (leftScript.ControlProperties as WCharacterFormat).Font;
        if (!this.IsNested())
        {
          float fontSizeRatio = 0.714285731f;
          this.ReduceFontSizeOfOfficeMath(leftScript.Superscript, fontSizeRatio);
          this.ReduceFontSizeOfOfficeMath(leftScript.Subscript, fontSizeRatio);
        }
        scriptWidget.Superscript = this.LayoutOfficeMath(currentBounds, leftScript.Superscript);
        scriptWidget.Subscript = this.LayoutOfficeMath(currentBounds, leftScript.Subscript);
        scriptWidget.Equation = this.LayoutOfficeMath(currentBounds, leftScript.Equation);
        float width1;
        if ((double) scriptWidget.Superscript.Bounds.Width > (double) scriptWidget.Subscript.Bounds.Width)
        {
          width1 = scriptWidget.Superscript.Bounds.Width;
          float xPosition = width1 - scriptWidget.Subscript.Bounds.Width;
          scriptWidget.Subscript.ShiftXYPosition(xPosition, 0.0f);
        }
        else if ((double) scriptWidget.Superscript.Bounds.Width < (double) scriptWidget.Subscript.Bounds.Width)
        {
          width1 = scriptWidget.Subscript.Bounds.Width;
          float xPosition = width1 - scriptWidget.Superscript.Bounds.Width;
          scriptWidget.Superscript.ShiftXYPosition(xPosition, 0.0f);
        }
        else
          width1 = scriptWidget.Superscript.Bounds.Width;
        scriptWidget.Equation.ShiftXYPosition(width1, 0.0f);
        float yPosition1 = scriptWidget.Superscript.Bounds.Height / 2f - this.DrawingContext.GetDescent(font1);
        scriptWidget.Equation.ShiftXYPosition(0.0f, yPosition1);
        float yPosition2 = (float) ((double) scriptWidget.Equation.Bounds.Height - (double) this.DrawingContext.GetDescent(font1) - (double) scriptWidget.Subscript.Bounds.Height / 2.0);
        scriptWidget.Subscript.ShiftXYPosition(0.0f, yPosition2);
        break;
      case MathFunctionType.SubSuperscript:
        if (!this.IsNested())
        {
          float fontSizeRatio = 0.714285731f;
          this.ReduceFontSizeOfOfficeMath(script.Script, fontSizeRatio);
        }
        Font font2 = (script.ControlProperties as WCharacterFormat).Font;
        scriptWidget.Equation = this.LayoutOfficeMath(currentBounds, script.Equation);
        if (script.ScriptType == MathScriptType.Superscript)
        {
          scriptWidget.Superscript = this.LayoutOfficeMath(currentBounds, script.Script);
          float yPosition3 = scriptWidget.Superscript.Bounds.Height / 2f - this.DrawingContext.GetDescent(font2);
          float width2 = scriptWidget.Equation.Bounds.Width;
          scriptWidget.Equation.ShiftXYPosition(0.0f, yPosition3);
          scriptWidget.Superscript.ShiftXYPosition(width2, 0.0f);
          break;
        }
        scriptWidget.Subscript = this.LayoutOfficeMath(currentBounds, script.Script);
        float width3 = scriptWidget.Equation.Bounds.Width;
        float yPosition4 = (float) ((double) scriptWidget.Equation.Bounds.Height - (double) this.DrawingContext.GetDescent(font2) - (double) scriptWidget.Subscript.Bounds.Height / 2.0);
        scriptWidget.Subscript.ShiftXYPosition(width3, yPosition4);
        break;
      case MathFunctionType.RightSubSuperscript:
        Font font3 = (rightScript.ControlProperties as WCharacterFormat).Font;
        if (!this.IsNested())
        {
          float fontSizeRatio = 0.714285731f;
          this.ReduceFontSizeOfOfficeMath(rightScript.Superscript, fontSizeRatio);
          this.ReduceFontSizeOfOfficeMath(rightScript.Subscript, fontSizeRatio);
        }
        scriptWidget.Equation = this.LayoutOfficeMath(currentBounds, rightScript.Equation);
        scriptWidget.Superscript = this.LayoutOfficeMath(currentBounds, rightScript.Superscript);
        float yPosition5 = scriptWidget.Superscript.Bounds.Height / 2f - this.DrawingContext.GetDescent(font3);
        float width4 = scriptWidget.Equation.Bounds.Width;
        scriptWidget.Equation.ShiftXYPosition(0.0f, yPosition5);
        scriptWidget.Superscript.ShiftXYPosition(width4, 0.0f);
        scriptWidget.Subscript = this.LayoutOfficeMath(currentBounds, rightScript.Subscript);
        float width5 = scriptWidget.Equation.Bounds.Width;
        float yPosition6 = (float) ((double) scriptWidget.Equation.Bounds.Height - (double) this.DrawingContext.GetDescent(font3) - (double) scriptWidget.Subscript.Bounds.Height / 2.0);
        scriptWidget.Subscript.ShiftXYPosition(width5, yPosition6);
        break;
    }
    scriptWidget.Bounds = this.GetUpdatedBounds(script, leftScript, rightScript, scriptWidget);
    return scriptWidget;
  }

  private LayoutedFuntionWidget LayoutNArrayWidget(
    RectangleF clientActiveArea,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathFunctionBase mathFunction)
  {
    LayoutedNArrayWidget nArrayWidget = new LayoutedNArrayWidget(mathFunction);
    IOfficeMathNArray nArray = mathFunction as IOfficeMathNArray;
    nArrayWidget.Equation = this.LayoutOfficeMath(clientActiveArea, nArray.Equation);
    nArrayWidget.NArrayCharacter = this.GetStringltWidget(nArray.ControlProperties as WCharacterFormat, nArray.NArrayCharacter, clientActiveArea);
    RectangleF bounds = nArrayWidget.NArrayCharacter.Bounds;
    float size = nArrayWidget.NArrayCharacter.Font.Size;
    float right = bounds.Right;
    if (!nArray.HideUpperLimit)
    {
      if (!this.IsNested())
      {
        float fontSizeRatio = 0.714285731f;
        this.ReduceFontSizeOfOfficeMath(nArray.Superscript, fontSizeRatio);
      }
      else
      {
        float fontSizeRatio = 0.588235259f;
        this.ReduceFontSizeOfOfficeMath(nArray.Superscript, fontSizeRatio);
      }
      nArrayWidget.Superscript = this.LayoutOfficeMath(clientActiveArea, nArray.Superscript);
      if (nArray.SubSuperscriptLimit)
      {
        bounds.Y += nArrayWidget.Superscript.Bounds.Height / 4f;
        nArrayWidget.Equation.ShiftXYPosition(0.0f, bounds.Y - nArrayWidget.Equation.Bounds.Y);
        nArrayWidget.Superscript.ShiftXYPosition(bounds.Width, 0.0f);
      }
      else
      {
        bounds.Y = nArrayWidget.Superscript.Bounds.Bottom;
        nArrayWidget.Equation.ShiftXYPosition(0.0f, bounds.Y - nArrayWidget.Equation.Bounds.Y);
        nArrayWidget.Superscript.ShiftXYPosition(bounds.Width / 2.5f, 0.0f);
      }
      if ((double) nArrayWidget.Superscript.Bounds.Width > (double) bounds.Width && !this.IsNested())
        bounds.X += nArrayWidget.Superscript.Bounds.Width / 2f;
      if ((double) nArrayWidget.Superscript.Bounds.Right > (double) bounds.Right)
        right = nArrayWidget.Superscript.Bounds.Right;
    }
    if (!nArray.HideLowerLimit)
    {
      if (!this.IsNested())
      {
        float fontSizeRatio = 0.714285731f;
        this.ReduceFontSizeOfOfficeMath(nArray.Subscript, fontSizeRatio);
      }
      else
      {
        float fontSizeRatio = 0.588235259f;
        this.ReduceFontSizeOfOfficeMath(nArray.Subscript, fontSizeRatio);
      }
      nArrayWidget.Subscript = this.LayoutOfficeMath(clientActiveArea, nArray.Subscript);
      float num1 = bounds.Height - size;
      float num2 = clientActiveArea.X;
      float y = clientActiveArea.Y;
      if (nArray.SubSuperscriptLimit)
      {
        Font font = nArrayWidget.NArrayCharacter.Font;
        float width = nArrayWidget.NArrayCharacter.Bounds.Width;
        float yPosition = bounds.Bottom - clientActiveArea.Y - num1 - this.DrawingContext.GetDescent(font);
        nArrayWidget.Subscript.ShiftXYPosition(width, yPosition);
      }
      else
      {
        num2 = nArrayWidget.NArrayCharacter.Bounds.Width / 2.5f;
        float yPosition = bounds.Bottom - clientActiveArea.Y - num1;
        float xPosition1;
        if ((double) bounds.Right >= (double) nArrayWidget.Subscript.Bounds.Right)
        {
          xPosition1 = (bounds.Right - nArrayWidget.Subscript.Bounds.Right) / 2f;
        }
        else
        {
          float xPosition2 = (nArrayWidget.Subscript.Bounds.Width - bounds.Width) / 2f;
          xPosition1 = 0.0f;
          bounds.X += xPosition2;
          nArrayWidget.Superscript.ShiftXYPosition(xPosition2, 0.0f);
        }
        nArrayWidget.Subscript.ShiftXYPosition(xPosition1, yPosition);
      }
      if ((double) nArrayWidget.Subscript.Bounds.Right > (double) bounds.Right && (nArray.HideUpperLimit || (double) nArrayWidget.Subscript.Bounds.Right > (double) nArrayWidget.Superscript.Bounds.Right))
        right = nArrayWidget.Subscript.Bounds.Right;
    }
    float xPosition = right - clientActiveArea.X;
    float yPosition1 = (float) (((double) bounds.Height - (double) nArrayWidget.Equation.Bounds.Height) / 2.0);
    if (!nArray.HideUpperLimit)
    {
      if ((double) nArrayWidget.Superscript.Bounds.Y > (double) nArrayWidget.Equation.Bounds.Y + (double) yPosition1)
      {
        float yPosition2 = nArrayWidget.Superscript.Bounds.Y - (nArrayWidget.Equation.Bounds.Y + yPosition1);
        nArrayWidget.Superscript.ShiftXYPosition(0.0f, yPosition2);
        if (nArrayWidget.Subscript != null)
          nArrayWidget.Subscript.ShiftXYPosition(0.0f, yPosition2);
        nArrayWidget.Equation.ShiftXYPosition(xPosition, yPosition1 + yPosition2);
        bounds.Y += yPosition2;
      }
      else
        nArrayWidget.Equation.ShiftXYPosition(xPosition, yPosition1);
    }
    else if ((double) nArrayWidget.NArrayCharacter.Bounds.Y > (double) nArrayWidget.Equation.Bounds.Y + (double) yPosition1)
    {
      float yPosition3 = nArrayWidget.NArrayCharacter.Bounds.Y - (nArrayWidget.Equation.Bounds.Y + yPosition1);
      nArrayWidget.Equation.ShiftXYPosition(xPosition, yPosition1 + yPosition3);
      if (nArrayWidget.Subscript != null)
        nArrayWidget.Subscript.ShiftXYPosition(0.0f, yPosition3);
      bounds.Y += yPosition3;
    }
    else
      nArrayWidget.Equation.ShiftXYPosition(xPosition, yPosition1);
    nArrayWidget.NArrayCharacter.Bounds = bounds;
    nArrayWidget.Bounds = this.GetUpdatedBoundsForNArray(nArrayWidget, nArray, bounds);
    return (LayoutedFuntionWidget) nArrayWidget;
  }

  private LayoutedAccentWidget LayoutAccentWidget(
    RectangleF clientActiveArea,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathFunctionBase mathFunction)
  {
    LayoutedAccentWidget layoutedAccentWidget = new LayoutedAccentWidget(mathFunction);
    IOfficeMathAccent officeMathAccent = mathFunction as IOfficeMathAccent;
    WCharacterFormat controlProperties = officeMathAccent.ControlProperties as WCharacterFormat;
    List<int> intList = new List<int>();
    intList.Add(769);
    intList.Add(780);
    intList.Add(776);
    intList.Add(775);
    intList.Add(770);
    intList.Add(768 /*0x0300*/);
    intList.Add(774);
    intList.Add(771);
    string baseChar = "a";
    layoutedAccentWidget.Equation = this.LayoutOfficeMath(clientActiveArea, officeMathAccent.Equation);
    layoutedAccentWidget.AccentCharacter = this.GetStringltWidget(controlProperties, officeMathAccent.AccentCharacter, clientActiveArea);
    if ((double) layoutedAccentWidget.Equation.Bounds.Width > (double) layoutedAccentWidget.AccentCharacter.Bounds.Width && !intList.Contains((int) officeMathAccent.AccentCharacter[0]))
      layoutedAccentWidget.ScalingFactor = layoutedAccentWidget.Equation.Bounds.Width / layoutedAccentWidget.AccentCharacter.Bounds.Width;
    float num = 0.0f;
    float shiftY = 0.0f;
    string equationText = this.GetEquationText(officeMathAccent.Equation);
    if ((double) layoutedAccentWidget.AccentCharacter.Bounds.Height >= (double) layoutedAccentWidget.Equation.Bounds.Height)
      this.FindTextDifference(equationText, baseChar, controlProperties, layoutedAccentWidget.Equation.Bounds, ref shiftY);
    else
      shiftY = layoutedAccentWidget.Equation.Bounds.Height / 10f;
    if ((double) layoutedAccentWidget.Equation.Bounds.Width > (double) layoutedAccentWidget.AccentCharacter.Bounds.Width && (double) layoutedAccentWidget.ScalingFactor == 1.0)
      num = (float) ((double) layoutedAccentWidget.Equation.Bounds.Width / 2.0 + (double) layoutedAccentWidget.AccentCharacter.Bounds.Width / 2.0);
    else if (officeMathAccent.AccentCharacter[0] == '̅' || officeMathAccent.AccentCharacter[0] == '̿')
      num = layoutedAccentWidget.Equation.Bounds.Width;
    layoutedAccentWidget.AccentCharacter.Bounds = new RectangleF(layoutedAccentWidget.AccentCharacter.Bounds.X + num, layoutedAccentWidget.AccentCharacter.Bounds.Y - shiftY, layoutedAccentWidget.AccentCharacter.Bounds.Width, layoutedAccentWidget.AccentCharacter.Bounds.Height);
    layoutedAccentWidget.Bounds = this.UpdateBounds(layoutedAccentWidget.AccentCharacter.Bounds, layoutedAccentWidget.Equation.Bounds);
    return layoutedAccentWidget;
  }

  private string GetEquationText(IOfficeMath equation)
  {
    string equationText = "";
    for (int index = 0; index < equation.Functions.Count; ++index)
    {
      IOfficeMathFunctionBase function = equation.Functions[index];
      if (function.Type == MathFunctionType.RunElement)
      {
        IOfficeMathRunElement officeMathRunElement = function as IOfficeMathRunElement;
        if (officeMathRunElement.Item is WTextRange)
          equationText = (officeMathRunElement.Item as WTextRange).Text;
      }
    }
    return equationText;
  }

  private LayoutedStringWidget GetStringltWidget(
    WCharacterFormat characterFormat,
    string character,
    RectangleF clientActiveArea)
  {
    LayoutedStringWidget stringltWidget = new LayoutedStringWidget();
    stringltWidget.Text = character;
    float fontSize = characterFormat.FontSize;
    if (this.MathLayoutingStack.Peek() == MathFunctionType.NArray)
      fontSize = characterFormat.FontSize * 2f;
    Font font = this.Document.FontSettings.GetFont(characterFormat.SymExFontName != null ? characterFormat.SymExFontName : characterFormat.FontName, fontSize, characterFormat.Font.Style);
    stringltWidget.Font = font.Clone() as Font;
    SizeF sizeF = this.DrawingContext.MeasureString(stringltWidget.Text, stringltWidget.Font, this.DrawingContext.StringFormt);
    if ((double) sizeF.Width == 0.0)
      sizeF.Width = this.DrawingContext.MeasureString('⃗'.ToString(), stringltWidget.Font, this.DrawingContext.StringFormt).Width;
    stringltWidget.Bounds = new RectangleF(clientActiveArea.X, clientActiveArea.Y, sizeF.Width, sizeF.Height);
    return stringltWidget;
  }

  private RectangleF GetUpdatedBoundsForNArray(
    LayoutedNArrayWidget nArrayWidget,
    IOfficeMathNArray nArray,
    RectangleF narrayBounds)
  {
    RectangleF updatedBoundsForNarray = new RectangleF();
    updatedBoundsForNarray.X = nArrayWidget.NArrayCharacter.Bounds.X;
    updatedBoundsForNarray.Y = (double) nArrayWidget.Equation.Bounds.Y >= (double) narrayBounds.Y ? narrayBounds.Y : nArrayWidget.Equation.Bounds.Y;
    if (!nArray.HideUpperLimit && (double) nArrayWidget.Equation.Bounds.Y > (double) nArrayWidget.Superscript.Bounds.Y)
      updatedBoundsForNarray.Y = nArrayWidget.Superscript.Bounds.Y;
    updatedBoundsForNarray.Width = nArrayWidget.Equation.Bounds.Right - updatedBoundsForNarray.X;
    updatedBoundsForNarray.Height = (double) nArrayWidget.Equation.Bounds.Bottom <= (double) narrayBounds.Bottom ? narrayBounds.Bottom - updatedBoundsForNarray.Y : nArrayWidget.Equation.Bounds.Bottom - updatedBoundsForNarray.Y;
    if (!nArray.HideLowerLimit && (double) nArrayWidget.Equation.Bounds.Bottom < (double) nArrayWidget.Subscript.Bounds.Bottom)
      updatedBoundsForNarray.Height = nArrayWidget.Subscript.Bounds.Bottom - updatedBoundsForNarray.Y;
    return updatedBoundsForNarray;
  }

  private RectangleF GetUpdatedBounds(
    IOfficeMathScript script,
    IOfficeMathLeftScript leftScript,
    IOfficeMathRightScript rightScript,
    LayoutedScriptWidget scriptWidget)
  {
    RectangleF updatedBounds = new RectangleF();
    if (script != null)
    {
      updatedBounds.X = scriptWidget.Equation.Bounds.X;
      if (script.ScriptType == MathScriptType.Superscript)
      {
        updatedBounds.Y = scriptWidget.Superscript.Bounds.Y;
        updatedBounds.Width = scriptWidget.Equation.Bounds.Width + scriptWidget.Superscript.Bounds.Width;
        updatedBounds.Height = scriptWidget.Equation.Bounds.Bottom - scriptWidget.Superscript.Bounds.Y;
      }
      else
      {
        updatedBounds.Y = scriptWidget.Equation.Bounds.Y;
        updatedBounds.Width = scriptWidget.Equation.Bounds.Width + scriptWidget.Subscript.Bounds.Width;
        updatedBounds.Height = scriptWidget.Subscript.Bounds.Bottom - scriptWidget.Equation.Bounds.Y;
      }
    }
    else if (leftScript != null)
    {
      updatedBounds.X = scriptWidget.Superscript.Bounds.X;
      updatedBounds.Y = scriptWidget.Superscript.Bounds.Y;
      updatedBounds.Width = scriptWidget.Equation.Bounds.Width + scriptWidget.Superscript.Bounds.Width;
      updatedBounds.Height = scriptWidget.Subscript.Bounds.Bottom - scriptWidget.Superscript.Bounds.Y;
    }
    else if (rightScript != null)
    {
      updatedBounds.X = scriptWidget.Equation.Bounds.X;
      updatedBounds.Y = scriptWidget.Superscript.Bounds.Y;
      updatedBounds.Width = scriptWidget.Equation.Bounds.Width + scriptWidget.Superscript.Bounds.Width;
      updatedBounds.Height = scriptWidget.Subscript.Bounds.Bottom - scriptWidget.Superscript.Bounds.Y;
    }
    return updatedBounds;
  }

  private WCharacterFormat GetControlCharacterProperty(IOfficeMathFunctionBase mathFunction)
  {
    WCharacterFormat characterProperty = (WCharacterFormat) null;
    switch (mathFunction.Type)
    {
      case MathFunctionType.Accent:
        characterProperty = (mathFunction as IOfficeMathAccent).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Bar:
        characterProperty = (mathFunction as IOfficeMathBar).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.BorderBox:
        characterProperty = (mathFunction as IOfficeMathBorderBox).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Box:
        characterProperty = (mathFunction as IOfficeMathBox).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Delimiter:
        characterProperty = (mathFunction as IOfficeMathDelimiter).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.EquationArray:
        characterProperty = (mathFunction as IOfficeMathEquationArray).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Fraction:
        characterProperty = (mathFunction as IOfficeMathFraction).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Function:
        characterProperty = (mathFunction as IOfficeMathFunction).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.GroupCharacter:
        characterProperty = (mathFunction as IOfficeMathGroupCharacter).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Limit:
        characterProperty = (mathFunction as IOfficeMathLimit).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Matrix:
        characterProperty = (mathFunction as IOfficeMathMatrix).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.NArray:
        characterProperty = (mathFunction as IOfficeMathNArray).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Phantom:
        characterProperty = (mathFunction as IOfficeMathPhantom).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.Radical:
        characterProperty = (mathFunction as IOfficeMathRadical).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.LeftSubSuperscript:
        characterProperty = (mathFunction as IOfficeMathLeftScript).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.SubSuperscript:
        characterProperty = (mathFunction as IOfficeMathScript).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.RightSubSuperscript:
        characterProperty = (mathFunction as IOfficeMathRightScript).ControlProperties as WCharacterFormat;
        break;
      case MathFunctionType.RunElement:
        if ((mathFunction as IOfficeMathRunElement).Item is WTextRange)
        {
          characterProperty = ((mathFunction as IOfficeMathRunElement).Item as WTextRange).CharacterFormat;
          break;
        }
        break;
    }
    return characterProperty;
  }

  private RectangleF UpdateBounds(RectangleF currentBounds, RectangleF modifiedBounds)
  {
    RectangleF rectangleF = currentBounds;
    if ((double) modifiedBounds.X < (double) rectangleF.X)
      rectangleF.X = modifiedBounds.X;
    if ((double) modifiedBounds.Y < (double) rectangleF.Y)
      rectangleF.Y = modifiedBounds.Y;
    rectangleF.Width += modifiedBounds.Width;
    if ((double) modifiedBounds.Height > (double) rectangleF.Height)
      rectangleF.Height = modifiedBounds.Height;
    return rectangleF;
  }

  private void GetNextCharacters(
    WTextRange textRange,
    ref char firstSpace,
    ref char secondSpace,
    ref char finalChar)
  {
    if (textRange.Text.Length > 0)
    {
      if (firstSpace == char.MaxValue)
        firstSpace = textRange.Text[0];
      else if (secondSpace == char.MaxValue)
        secondSpace = textRange.Text[0];
      else if (finalChar == char.MaxValue)
        finalChar = textRange.Text[0];
    }
    if (textRange.Text.Length > 1)
    {
      if (secondSpace == char.MaxValue)
        secondSpace = textRange.Text[1];
      else if (finalChar == char.MaxValue)
        finalChar = textRange.Text[1];
    }
    if (textRange.Text.Length <= 2 || finalChar != char.MaxValue)
      return;
    finalChar = textRange.Text[2];
  }

  internal void UpdateTextByMathStyles(WTextRange textRange, int currentIndex)
  {
    string text = textRange.Text;
    MathStyleType mathStyleType = textRange.OwnerMathRunElement.MathFormat.Style;
    if (textRange.OwnerMathRunElement.MathFormat.HasNormalText)
      mathStyleType = MathStyleType.Regular;
    string str = "";
    for (int index1 = 0; index1 < text.Length; ++index1)
    {
      char ch = text[index1];
      int utf32_1 = 0;
      int int32 = Convert.ToInt32(ch);
      int num1 = 52;
      char maxValue1 = char.MaxValue;
      char maxValue2 = char.MaxValue;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = this.IsOperatorSymbol((int) ch);
      if (mathStyleType != MathStyleType.Regular && ch == ',')
      {
        char maxValue3 = char.MaxValue;
        char maxValue4 = char.MaxValue;
        char maxValue5 = char.MaxValue;
        if (index1 + 1 < text.Length)
          maxValue3 = text[index1 + 1];
        if (index1 + 2 < text.Length)
          maxValue4 = text[index1 + 2];
        if (index1 + 3 < text.Length)
          maxValue5 = text[index1 + 3];
        for (int index2 = 1; index2 < 3; ++index2)
        {
          if ((maxValue5 == char.MaxValue || maxValue4 == char.MaxValue || maxValue3 == char.MaxValue) && textRange.OwnerMathRunElement.OwnerMathEntity is OfficeMath && currentIndex + index2 < (textRange.OwnerMathRunElement.OwnerMathEntity as OfficeMath).Functions.Count && (textRange.OwnerMathRunElement.OwnerMathEntity as OfficeMath).Functions[currentIndex + index2] is OfficeMathRunElement)
          {
            OfficeMathRunElement function = (textRange.OwnerMathRunElement.OwnerMathEntity as OfficeMath).Functions[currentIndex + index2] as OfficeMathRunElement;
            if (function.Item is WTextRange)
              this.GetNextCharacters(function.Item as WTextRange, ref maxValue3, ref maxValue4, ref maxValue5);
          }
        }
        if ((maxValue5 == char.MaxValue || maxValue4 == char.MaxValue || maxValue3 == char.MaxValue) && textRange.OwnerMathRunElement.OwnerMathEntity != null && textRange.OwnerMathRunElement.OwnerMathEntity.OwnerMathEntity is OfficeMathParagraph)
        {
          OfficeMathParagraph ownerMathEntity = textRange.OwnerMathRunElement.OwnerMathEntity.OwnerMathEntity as OfficeMathParagraph;
          int num2 = 0;
          for (int index3 = 0; index3 < ownerMathEntity.Maths.Count; ++index3)
          {
            if (ownerMathEntity.Maths[index3] == textRange.OwnerMathRunElement.OwnerMathEntity)
            {
              num2 = index3;
              break;
            }
          }
          for (int index4 = 1; index4 < ownerMathEntity.Maths.Count && (maxValue5 == char.MaxValue || maxValue4 == char.MaxValue || maxValue3 == char.MaxValue); ++index4)
          {
            if (num2 + 1 < ownerMathEntity.Maths.Count && ownerMathEntity.Maths[num2 + index4] is OfficeMath)
            {
              OfficeMath math = ownerMathEntity.Maths[num2 + index4] as OfficeMath;
              for (int index5 = 0; index5 < math.Functions.Count; ++index5)
              {
                if (math.Functions[index5] is OfficeMathRunElement)
                {
                  OfficeMathRunElement function = math.Functions[index5] as OfficeMathRunElement;
                  if (function.Item is WTextRange)
                    this.GetNextCharacters(function.Item as WTextRange, ref maxValue3, ref maxValue4, ref maxValue5);
                }
              }
            }
          }
        }
        if (maxValue3 == ' ' && maxValue4 == ' ' && maxValue5 != char.MaxValue)
        {
          str = str + char.ConvertFromUtf32((int) ch) + char.ConvertFromUtf32(8195) + char.ConvertFromUtf32(8195);
          continue;
        }
      }
      if (mathStyleType != MathStyleType.Regular && flag3)
      {
        if (index1 > 0)
          maxValue2 = text[index1 - 1];
        else if (currentIndex > 0 && textRange.OwnerMathRunElement.OwnerMathEntity is OfficeMath && (textRange.OwnerMathRunElement.OwnerMathEntity as OfficeMath).Functions[currentIndex - 1] is OfficeMathRunElement)
        {
          OfficeMathRunElement function = (textRange.OwnerMathRunElement.OwnerMathEntity as OfficeMath).Functions[currentIndex - 1] as OfficeMathRunElement;
          if (function.Item is WTextRange)
          {
            WTextRange wtextRange = function.Item as WTextRange;
            if (wtextRange.Text.Length > 0)
              maxValue2 = wtextRange.Text[wtextRange.Text.Length - 1];
          }
        }
        if (index1 + 1 < text.Length)
          maxValue1 = text[index1 + 1];
        else if (currentIndex + 1 < (textRange.OwnerMathRunElement.OwnerMathEntity as OfficeMath).Functions.Count && textRange.OwnerMathRunElement.OwnerMathEntity is OfficeMath && (textRange.OwnerMathRunElement.OwnerMathEntity as OfficeMath).Functions[currentIndex + 1] is OfficeMathRunElement)
        {
          OfficeMathRunElement function = (textRange.OwnerMathRunElement.OwnerMathEntity as OfficeMath).Functions[currentIndex + 1] as OfficeMathRunElement;
          if (function.Item is WTextRange)
          {
            WTextRange wtextRange = function.Item as WTextRange;
            if (wtextRange.Text.Length > 0)
              maxValue1 = wtextRange.Text[0];
          }
        }
        if (maxValue2 != char.MaxValue)
          flag2 = this.IsOperatorSymbol((int) maxValue2);
        if (maxValue1 != char.MaxValue)
          flag1 = this.IsOperatorSymbol((int) maxValue1);
        if (!flag2 && maxValue2 != char.MaxValue)
          str += " ";
        str = ch != '-' ? str + char.ConvertFromUtf32((int) ch) : str + char.ConvertFromUtf32(8722);
        if ((flag1 ? (ch < '∫' ? 0 : (ch <= '∴' ? 1 : 0)) : 1) != 0 && maxValue1 != char.MaxValue)
          str += " ";
      }
      else
      {
        MathFontType font = textRange.OwnerMathRunElement.MathFormat.Font;
        if (font == MathFontType.Fraktur)
          num1 = 104;
        if (this.IsGreakCharacter((int) ch))
        {
          if (char.IsUpper(ch))
          {
            utf32_1 = int32 - 913;
            switch (font)
            {
              case MathFontType.Roman:
                utf32_1 += 120488;
                break;
              case MathFontType.SansSerif:
                utf32_1 += 120662;
                break;
            }
          }
          else if (char.IsLower(ch))
          {
            utf32_1 = int32 - 945;
            switch (font)
            {
              case MathFontType.Roman:
                utf32_1 += 120514;
                break;
              case MathFontType.SansSerif:
                utf32_1 += 120688;
                break;
            }
          }
          num1 = 58;
        }
        else
        {
          if (char.IsNumber(ch))
          {
            int utf32_2 = int32 - 48 /*0x30*/;
            switch (font)
            {
              case MathFontType.DoubleStruck:
                utf32_2 += 120792;
                break;
              case MathFontType.Monospace:
                utf32_2 += 120822;
                break;
              case MathFontType.Roman:
                if (mathStyleType == MathStyleType.Bold)
                {
                  utf32_2 += 120782;
                  break;
                }
                str += (string) (object) ch;
                continue;
              case MathFontType.SansSerif:
                if (mathStyleType == MathStyleType.Bold)
                {
                  utf32_2 += 120812;
                  break;
                }
                utf32_2 += 120802;
                break;
            }
            str += char.ConvertFromUtf32(utf32_2);
            continue;
          }
          if (char.IsUpper(ch))
          {
            utf32_1 = int32 - 65;
            switch (font)
            {
              case MathFontType.DoubleStruck:
                utf32_1 += 120120;
                break;
              case MathFontType.Fraktur:
                utf32_1 += 120068;
                break;
              case MathFontType.Monospace:
                utf32_1 += 120432;
                break;
              case MathFontType.Roman:
                utf32_1 += 119808;
                break;
              case MathFontType.SansSerif:
                utf32_1 += 120224;
                break;
              case MathFontType.Script:
                utf32_1 += 119964;
                break;
            }
          }
          else if (char.IsLower(ch))
          {
            utf32_1 = int32 - 97;
            switch (font)
            {
              case MathFontType.DoubleStruck:
                utf32_1 += 120146;
                break;
              case MathFontType.Fraktur:
                utf32_1 += 120094;
                break;
              case MathFontType.Monospace:
                utf32_1 += 120458;
                break;
              case MathFontType.Roman:
                utf32_1 += 119834;
                break;
              case MathFontType.SansSerif:
                utf32_1 += 120250;
                break;
              case MathFontType.Script:
                utf32_1 += 119990;
                break;
            }
          }
          else if (char.IsPunctuation(ch) || char.IsSymbol(ch) || char.IsWhiteSpace(ch))
          {
            str += (string) (object) ch;
            continue;
          }
        }
        if (utf32_1 == 0)
        {
          str += (string) (object) ch;
        }
        else
        {
          switch (font)
          {
            case MathFontType.DoubleStruck:
            case MathFontType.Monospace:
              if (font == MathFontType.DoubleStruck)
              {
                switch (ch)
                {
                  case 'C':
                    utf32_1 = 8450;
                    break;
                  case 'H':
                    utf32_1 = 8461;
                    break;
                  case 'N':
                    utf32_1 = 8469;
                    break;
                  case 'P':
                    utf32_1 = 8473;
                    break;
                  case 'Q':
                    utf32_1 = 8474;
                    break;
                  case 'R':
                    utf32_1 = 8477;
                    break;
                  case 'Z':
                    utf32_1 = 8484;
                    break;
                }
              }
              str += char.ConvertFromUtf32(utf32_1);
              continue;
            case MathFontType.Fraktur:
            case MathFontType.Script:
              if (mathStyleType == MathStyleType.Bold)
              {
                str += char.ConvertFromUtf32(utf32_1 + num1);
                continue;
              }
              if (font == MathFontType.Script)
              {
                switch (ch)
                {
                  case 'B':
                    utf32_1 = 8492;
                    break;
                  case 'E':
                    utf32_1 = 8496;
                    break;
                  case 'F':
                    utf32_1 = 8497;
                    break;
                  case 'H':
                    utf32_1 = 8459;
                    break;
                  case 'I':
                    utf32_1 = 8464;
                    break;
                  case 'L':
                    utf32_1 = 8466;
                    break;
                  case 'M':
                    utf32_1 = 8499;
                    break;
                  case 'R':
                    utf32_1 = 8475;
                    break;
                  case 'e':
                    utf32_1 = 8495;
                    break;
                  case 'g':
                    utf32_1 = 8458;
                    break;
                  case 'o':
                    utf32_1 = 8500;
                    break;
                }
              }
              else
              {
                switch (ch)
                {
                  case 'H':
                    utf32_1 = 8460;
                    break;
                  case 'I':
                    utf32_1 = 8465;
                    break;
                  case 'R':
                    utf32_1 = 8476;
                    break;
                  case 'Z':
                    utf32_1 = 8488;
                    break;
                  case 'c':
                    utf32_1 = 8493;
                    break;
                }
              }
              str += char.ConvertFromUtf32(utf32_1);
              continue;
            case MathFontType.Roman:
              switch (mathStyleType)
              {
                case MathStyleType.Italic:
                  str = ch != 'h' ? str + char.ConvertFromUtf32(utf32_1 + num1) : str + char.ConvertFromUtf32(8462);
                  continue;
                case MathStyleType.BoldItalic:
                  str += char.ConvertFromUtf32(utf32_1 + num1 * 2);
                  continue;
                case MathStyleType.Bold:
                  str += char.ConvertFromUtf32(utf32_1);
                  continue;
                default:
                  str += (string) (object) ch;
                  continue;
              }
            default:
              switch (mathStyleType)
              {
                case MathStyleType.Italic:
                  str = ch != 'h' ? str + char.ConvertFromUtf32(utf32_1 + num1 * 2) : str + char.ConvertFromUtf32(8462);
                  continue;
                case MathStyleType.BoldItalic:
                  str += char.ConvertFromUtf32(utf32_1 + num1 * 3);
                  continue;
                case MathStyleType.Bold:
                  str += char.ConvertFromUtf32(utf32_1 + num1);
                  continue;
                case MathStyleType.Regular:
                  str += char.ConvertFromUtf32(utf32_1);
                  continue;
                default:
                  continue;
              }
          }
        }
      }
    }
    textRange.Text = str;
  }

  internal bool IsOperatorSymbol(int charValue)
  {
    return charValue >= 8704 && charValue <= 8959 || charValue >= 42 && charValue <= 47 || charValue == 247 || charValue == 215 || charValue == 177 || charValue == 183 || charValue == 8741 || charValue == 61 || charValue == 8729;
  }

  internal bool IsGreakCharacter(int charValue) => charValue >= 913 && charValue <= 989;

  private ILayoutedFuntionWidget LayoutRunElement(
    RectangleF currentBounds,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathFunctionBase mathFunction,
    int currentIndex)
  {
    IOfficeMathRunElement widget = mathFunction as IOfficeMathRunElement;
    if (widget.Item is ILeafWidget)
    {
      LayoutedWidget layoutedWidget;
      if (widget.Item is WTextRange)
      {
        WTextRange textRange = widget.Item as WTextRange;
        if (textRange.OrignalText == string.Empty)
        {
          textRange.OrignalText = textRange.Text;
          this.UpdateTextByMathStyles(textRange, currentIndex);
        }
        layoutedWidget = new LayoutedWidget((IWidget) textRange);
        SizeF size = layoutedWidget.Widget.LayoutInfo.Size;
        layoutedWidget.Bounds = new RectangleF(currentBounds.X, currentBounds.Y, size.Width, size.Height);
      }
      else
        layoutedWidget = LayoutContext.Create((IWidget) (widget.Item as ILeafWidget), this.m_lcOperator, this.IsForceFitLayout).Layout(currentBounds);
      if (layoutedWidget != null)
      {
        LayoutedOfficeRunWidget layoutedOfficeRunWidget = new LayoutedOfficeRunWidget((IOfficeMathFunctionBase) widget);
        layoutedOfficeRunWidget.LayoutedWidget = layoutedWidget;
        layoutedWidget.Owner = (LayoutedWidget) layoutedOfficeRunWidget;
        layoutedOfficeRunWidget.Bounds = layoutedWidget.Bounds;
        return (ILayoutedFuntionWidget) layoutedOfficeRunWidget;
      }
    }
    return (ILayoutedFuntionWidget) null;
  }

  private bool IsNested() => this.MathLayoutingStack.Count > 1;

  private bool HasNestedFunction(MathFunctionType functionType)
  {
    int num = 0;
    foreach (MathFunctionType mathFunctionType in this.MathLayoutingStack.ToArray())
    {
      if (mathFunctionType == functionType)
        ++num;
      if (num > 1)
        return true;
    }
    return num > 1;
  }

  private LayoutedRadicalWidget LayoutRadicalSwitch(
    RectangleF currentBounds,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathFunctionBase mathFunction)
  {
    LayoutedRadicalWidget layoutedRadicalWidget = new LayoutedRadicalWidget(mathFunction);
    IOfficeMathRadical radical = mathFunction as IOfficeMathRadical;
    layoutedRadicalWidget.Equation = this.LayoutOfficeMath(currentBounds, radical.Equation);
    float fontSizeRatio = 0.0f;
    if (!radical.HideDegree)
    {
      fontSizeRatio = 0.603f;
      if (radical.Degree.ArgumentSize == 1)
        fontSizeRatio = 0.803f;
      else if (radical.Degree.ArgumentSize == 2)
        fontSizeRatio = 1f;
      this.ReduceFontSizeOfOfficeMath(radical.Degree, fontSizeRatio);
      layoutedRadicalWidget.Degree = this.LayoutOfficeMath(currentBounds, radical.Degree);
    }
    float radicalSymbolWidth;
    layoutedRadicalWidget.RadicalLines = this.GenerateRadicalLines(radical, layoutedRadicalWidget.Equation.Bounds, out radicalSymbolWidth);
    Font fontToRender = (radical.ControlProperties as WCharacterFormat).GetFontToRender(FontScriptType.English);
    float width1 = this.DrawingContext.MeasureString("√", fontToRender, (StringFormat) null).Width;
    float num1 = radicalSymbolWidth + (float) ((double) width1 / 100.0 * 10.0);
    float descent = this.DrawingContext.GetDescent(fontToRender);
    float xPosition = num1;
    float yPosition1 = descent;
    if (!radical.HideDegree)
    {
      float width2 = layoutedRadicalWidget.Degree.Bounds.Width;
      float height = layoutedRadicalWidget.Degree.Bounds.Height;
      WCharacterFormat controlProperties = radical.ControlProperties as WCharacterFormat;
      Font font = this.Document.FontSettings.GetFont(controlProperties.GetFontNameToRender(FontScriptType.English), controlProperties.GetFontSizeToRender() * fontSizeRatio, FontStyle.Regular);
      float shiftX = width2 - (layoutedRadicalWidget.RadicalLines[1].Point2.X - (currentBounds.X + (float) ((double) width1 / 100.0 * 10.0)));
      if ((double) shiftX < 0.0)
        shiftX = 0.0f;
      float num2 = height - this.DrawingContext.GetDescent(font);
      float num3 = layoutedRadicalWidget.RadicalLines[1].Point1.Y - layoutedRadicalWidget.RadicalLines[0].Point2.Y;
      float shiftY;
      if ((double) num2 > (double) num3)
      {
        shiftY = num2 - num3;
      }
      else
      {
        float yPosition2 = (float) ((double) num3 / 100.0 * 50.0) - num2;
        if ((double) yPosition2 <= 0.0)
        {
          shiftY = -yPosition2;
        }
        else
        {
          shiftY = 0.0f;
          layoutedRadicalWidget.Degree.ShiftXYPosition(0.0f, yPosition2);
        }
      }
      this.ShiftLayoutedLineWidgetXYPosition(layoutedRadicalWidget.RadicalLines, shiftX, shiftY);
      xPosition += shiftX;
      yPosition1 += shiftY;
    }
    layoutedRadicalWidget.Equation.ShiftXYPosition(xPosition, yPosition1);
    float width3 = layoutedRadicalWidget.Equation.Bounds.Right - currentBounds.X;
    float height1 = layoutedRadicalWidget.Equation.Bounds.Bottom - currentBounds.Y;
    layoutedRadicalWidget.Bounds = new RectangleF(currentBounds.X, currentBounds.Y, width3, height1);
    return layoutedRadicalWidget;
  }

  private void ShiftLayoutedLineWidgetXYPosition(
    LayoutedLineWidget[] layoutedLines,
    float shiftX,
    float shiftY)
  {
    foreach (LayoutedLineWidget layoutedLine in layoutedLines)
      layoutedLine.ShiftXYPosition(shiftX, shiftY);
  }

  private LayoutedLineWidget[] GenerateRadicalLines(
    IOfficeMathRadical radical,
    RectangleF equationBounds,
    out float radicalSymbolWidth)
  {
    Font fontToRender = (radical.ControlProperties as WCharacterFormat).GetFontToRender(FontScriptType.English);
    float lineThickness = (float) (((double) this.DrawingContext.GetAscent(fontToRender) - (double) this.DrawingContext.GetDescent(fontToRender)) / 10.0);
    Color textColor = this.DrawingContext.GetTextColor(radical.ControlProperties as WCharacterFormat);
    float height = 0.0f;
    List<PointF> radicalUpwardLine = this.GenerateRadicalUpwardLine(equationBounds, lineThickness, fontToRender);
    List<PointF> radicalDownwardLine = this.GenerateRadicalDownwardLine(equationBounds, lineThickness, radicalUpwardLine, ref height, fontToRender);
    List<PointF> topHorizontalLine = MathLayoutContext.GenerateRadicalTopHorizontalLine(equationBounds, lineThickness, radicalUpwardLine);
    List<PointF> radicalHook = this.GenerateRadicalHook(equationBounds, lineThickness, radicalDownwardLine, ref height, fontToRender);
    List<List<PointF>> pointFListList = new List<List<PointF>>();
    pointFListList.Add(radicalUpwardLine);
    pointFListList.Add(radicalDownwardLine);
    pointFListList.Add(topHorizontalLine);
    pointFListList.Add(radicalHook);
    float num = topHorizontalLine[0].X - radicalHook[0].X;
    foreach (List<PointF> pointFList in pointFListList)
    {
      for (int index = 0; index < pointFList.Count; ++index)
      {
        PointF pointF = pointFList[index];
        pointF.X += num;
        pointFList[index] = pointF;
      }
    }
    radicalSymbolWidth = num;
    LayoutedLineWidget[] radicalLines = new LayoutedLineWidget[4];
    for (int index = 0; index < pointFListList.Count; ++index)
    {
      List<PointF> pointFList = pointFListList[index];
      radicalLines[index] = new LayoutedLineWidget()
      {
        Point1 = pointFList[0],
        Point2 = pointFList[1],
        Width = lineThickness,
        Color = textColor
      };
    }
    return radicalLines;
  }

  private List<PointF> GenerateRadicalHook(
    RectangleF argumentBounds,
    float lineThickness,
    List<PointF> downwardLinePoints,
    ref float height,
    Font controlFont)
  {
    float ascent = this.DrawingContext.GetAscent(controlFont);
    height -= ascent * 0.425f;
    float widthFromAngle = this.GetWidthFromAngle(height, this.DegreeIntoRadians(32.2825f), this.DegreeIntoRadians(57.7174f));
    return new List<PointF>()
    {
      new PointF(downwardLinePoints[0].X + lineThickness / 4f - widthFromAngle, argumentBounds.Bottom - ascent * 0.425f),
      new PointF(downwardLinePoints[0].X, downwardLinePoints[0].Y)
    };
  }

  private static List<PointF> GenerateRadicalTopHorizontalLine(
    RectangleF argumentBounds,
    float lineThickness,
    List<PointF> upwardLinePoints)
  {
    float y = argumentBounds.Top + lineThickness / 4f;
    PointF pointF1 = new PointF(upwardLinePoints[1].X, y);
    PointF pointF2 = new PointF(pointF1.X + argumentBounds.Width, pointF1.Y);
    return new List<PointF>() { pointF1, pointF2 };
  }

  private List<PointF> GenerateRadicalDownwardLine(
    RectangleF argumentBounds,
    float lineThickness,
    List<PointF> upwardLinePoints,
    ref float height,
    Font controlFont)
  {
    height = this.DrawingContext.GetAscent(controlFont) * 0.558f;
    float widthFromAngle = this.GetWidthFromAngle(height, this.DegreeIntoRadians(64.3483f), this.DegreeIntoRadians(25.6516f));
    float num1 = upwardLinePoints[0].X - widthFromAngle;
    float num2 = argumentBounds.Bottom - height;
    return new List<PointF>()
    {
      new PointF(num1 - lineThickness / 4f, num2 + lineThickness / 4f),
      new PointF(upwardLinePoints[0].X, upwardLinePoints[0].Y)
    };
  }

  private List<PointF> GenerateRadicalUpwardLine(
    RectangleF argumentBounds,
    float lineThickness,
    Font controlFont)
  {
    float widthFromAngle = this.GetWidthFromAngle(this.DrawingContext.MeasureString("√", controlFont, (StringFormat) null).Height, this.DegreeIntoRadians(80.3856f), this.DegreeIntoRadians(9.6143f));
    return new List<PointF>()
    {
      new PointF((float) ((double) argumentBounds.Left - (double) widthFromAngle - (double) lineThickness / 2.0), argumentBounds.Bottom + lineThickness / 2f),
      new PointF(argumentBounds.Left - lineThickness / 2f, argumentBounds.Top + lineThickness / 4f)
    };
  }

  private float GetWidthFromAngle(float height, double angle1, double angle2)
  {
    return (float) ((double) height / Math.Sin(angle1) * Math.Sin(angle2));
  }

  private double DegreeIntoRadians(float angle) => Math.PI * (double) angle / 180.0;

  private LayoutedPhantomWidget LayoutPhantomSwitch(
    RectangleF currentBounds,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathFunctionBase mathFunction)
  {
    LayoutedPhantomWidget layoutedPhantomWidget = new LayoutedPhantomWidget(mathFunction);
    IOfficeMathPhantom officeMathPhantom = mathFunction as IOfficeMathPhantom;
    layoutedPhantomWidget.Equation = this.LayoutOfficeMath(currentBounds, officeMathPhantom.Equation);
    layoutedPhantomWidget.Show = officeMathPhantom.Show;
    float width = 0.0f;
    if (!officeMathPhantom.ZeroWidth)
      width = layoutedPhantomWidget.Equation.Bounds.Width;
    layoutedPhantomWidget.Bounds = new RectangleF(currentBounds.X, currentBounds.Y, width, layoutedPhantomWidget.Equation.Bounds.Height);
    return layoutedPhantomWidget;
  }

  private LayoutedFractionWidget LayoutFractionSwitch(
    RectangleF currentBounds,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathFunctionBase mathFunction)
  {
    LayoutedFractionWidget layoutedFractionWidget1 = new LayoutedFractionWidget(mathFunction);
    IOfficeMathFraction officeMathFraction = mathFunction as IOfficeMathFraction;
    bool flag = this.HasNestedFunction(MathFunctionType.Fraction);
    if (flag && this.Document.Settings.MathProperties.SmallFraction || this.MathWidget.IsInline || flag && officeMathFraction.FractionType == MathFractionType.NoFractionBar)
    {
      float fontSizeRatio = 0.725f;
      this.ReduceFontSizeOfOfficeMath(officeMathFraction.Numerator, fontSizeRatio);
      this.ReduceFontSizeOfOfficeMath(officeMathFraction.Denominator, fontSizeRatio);
    }
    layoutedFractionWidget1.Numerator = this.LayoutOfficeMath(currentBounds, officeMathFraction.Numerator);
    layoutedFractionWidget1.Denominator = this.LayoutOfficeMath(currentBounds, officeMathFraction.Denominator);
    Font fontToRender = (officeMathFraction.ControlProperties as WCharacterFormat).GetFontToRender(FontScriptType.English);
    float ascent = this.DrawingContext.GetAscent(fontToRender);
    float descent = this.DrawingContext.GetDescent(fontToRender);
    float x = currentBounds.X;
    float y1 = currentBounds.Y + layoutedFractionWidget1.Numerator.Bounds.Height;
    if (officeMathFraction.FractionType != MathFractionType.NormalFractionBar && officeMathFraction.FractionType != MathFractionType.NoFractionBar)
      x += layoutedFractionWidget1.Numerator.Bounds.Width;
    switch (officeMathFraction.FractionType)
    {
      case MathFractionType.NormalFractionBar:
      case MathFractionType.NoFractionBar:
        float num1 = (float) (((double) ascent - (double) descent) / 10.0);
        if (!this.IsNested() && !this.MathWidget.IsInline)
          y1 += descent / 2f;
        float width1 = layoutedFractionWidget1.Numerator.Bounds.Width;
        if ((double) width1 < (double) layoutedFractionWidget1.Denominator.Bounds.Width)
          width1 = layoutedFractionWidget1.Denominator.Bounds.Width;
        layoutedFractionWidget1.FractionLine = new LayoutedLineWidget();
        layoutedFractionWidget1.FractionLine.Width = num1;
        layoutedFractionWidget1.FractionLine.Color = this.DrawingContext.GetTextColor(officeMathFraction.ControlProperties as WCharacterFormat);
        layoutedFractionWidget1.FractionLine.Point1 = new PointF(x, y1);
        layoutedFractionWidget1.FractionLine.Point2 = new PointF(x + width1, y1);
        if (officeMathFraction.FractionType == MathFractionType.NoFractionBar)
          layoutedFractionWidget1.FractionLine.Skip = true;
        float num2 = this.IsNested() ? y1 + num1 : y1 + (descent / 2f + num1);
        layoutedFractionWidget1.Denominator.ShiftXYPosition(0.0f, num2 - currentBounds.Y);
        if ((double) layoutedFractionWidget1.Numerator.Bounds.Width < (double) layoutedFractionWidget1.Denominator.Bounds.Width)
        {
          float xPosition = (float) (((double) width1 - (double) layoutedFractionWidget1.Numerator.Bounds.Width) / 2.0);
          layoutedFractionWidget1.Numerator.ShiftXYPosition(xPosition, 0.0f);
        }
        else
        {
          float xPosition = (float) (((double) width1 - (double) layoutedFractionWidget1.Denominator.Bounds.Width) / 2.0);
          layoutedFractionWidget1.Denominator.ShiftXYPosition(xPosition, 0.0f);
        }
        float width2 = width1;
        float height1 = layoutedFractionWidget1.Denominator.Bounds.Bottom - layoutedFractionWidget1.Numerator.Bounds.Y;
        layoutedFractionWidget1.Bounds = new RectangleF(layoutedFractionWidget1.Numerator.Bounds.X, layoutedFractionWidget1.Numerator.Bounds.Y, width2, height1);
        break;
      case MathFractionType.SkewedFractionBar:
        float num3 = (float) ((double) layoutedFractionWidget1.Numerator.Bounds.Height / 2.0 + (double) layoutedFractionWidget1.Denominator.Bounds.Height / 2.0);
        float num4 = this.Transform(new PointF(x, y1 + num3 / 2f), num3 / 2f, 290f).X - x;
        float num5 = (float) (((double) ascent - (double) descent) / 10.0);
        layoutedFractionWidget1.FractionLine = new LayoutedLineWidget();
        layoutedFractionWidget1.FractionLine.Width = num5;
        layoutedFractionWidget1.FractionLine.Color = this.DrawingContext.GetTextColor(officeMathFraction.ControlProperties as WCharacterFormat);
        layoutedFractionWidget1.FractionLine.Point1 = new PointF(x + num4, y1 - num3 / 2f);
        layoutedFractionWidget1.FractionLine.Point2 = new PointF(x - num4, y1 + num3 / 2f);
        float num6 = x + num4;
        layoutedFractionWidget1.Denominator.ShiftXYPosition(num6 - currentBounds.X, y1 - currentBounds.Y);
        float width3 = layoutedFractionWidget1.Denominator.Bounds.Right - layoutedFractionWidget1.Numerator.Bounds.X;
        float height2 = layoutedFractionWidget1.Denominator.Bounds.Bottom - layoutedFractionWidget1.Numerator.Bounds.Y;
        layoutedFractionWidget1.Bounds = new RectangleF(layoutedFractionWidget1.Numerator.Bounds.X, layoutedFractionWidget1.Numerator.Bounds.Y, width3, height2);
        break;
      case MathFractionType.FractionInline:
        float num7 = (float) ((double) layoutedFractionWidget1.Numerator.Bounds.Height / 2.0 + (double) layoutedFractionWidget1.Denominator.Bounds.Height / 2.0);
        float num8 = this.Transform(new PointF(0.0f, num7 / 2f), num7 / 2f, 290f).X - 0.0f;
        float num9 = (float) (((double) ascent - (double) descent) / 10.0);
        layoutedFractionWidget1.FractionLine = new LayoutedLineWidget();
        layoutedFractionWidget1.FractionLine.Width = num9;
        layoutedFractionWidget1.FractionLine.Color = this.DrawingContext.GetTextColor(officeMathFraction.ControlProperties as WCharacterFormat);
        layoutedFractionWidget1.FractionLine.Point1 = new PointF(x + num8 * 2f, layoutedFractionWidget1.Numerator.Bounds.Y);
        layoutedFractionWidget1.FractionLine.Point2 = new PointF(x, layoutedFractionWidget1.Numerator.Bounds.Y + num7);
        float num10;
        if ((double) layoutedFractionWidget1.Numerator.Bounds.Height < (double) layoutedFractionWidget1.Denominator.Bounds.Height)
        {
          float yPosition = (float) (((double) layoutedFractionWidget1.Denominator.Bounds.Height - (double) layoutedFractionWidget1.Numerator.Bounds.Height) / 2.0);
          layoutedFractionWidget1.Numerator.ShiftXYPosition(0.0f, yPosition);
          num10 = (float) (((double) layoutedFractionWidget1.Denominator.Bounds.Height - (double) num7) / 2.0);
        }
        else
        {
          float yPosition = (float) (((double) layoutedFractionWidget1.Numerator.Bounds.Height - (double) layoutedFractionWidget1.Denominator.Bounds.Height) / 2.0);
          layoutedFractionWidget1.Denominator.ShiftXYPosition(0.0f, yPosition);
          num10 = (float) (((double) layoutedFractionWidget1.Numerator.Bounds.Height - (double) num7) / 2.0);
        }
        layoutedFractionWidget1.FractionLine.Point1 = new PointF(layoutedFractionWidget1.FractionLine.Point1.X, layoutedFractionWidget1.FractionLine.Point1.Y + num10);
        layoutedFractionWidget1.FractionLine.Point2 = new PointF(layoutedFractionWidget1.FractionLine.Point2.X, layoutedFractionWidget1.FractionLine.Point2.Y + num10);
        float num11 = x + num8 * 2f;
        layoutedFractionWidget1.Denominator.ShiftXYPosition(num11 - layoutedFractionWidget1.Denominator.Bounds.X, 0.0f);
        float width4 = layoutedFractionWidget1.Denominator.Bounds.Right - layoutedFractionWidget1.Numerator.Bounds.X;
        float height3 = (double) layoutedFractionWidget1.Numerator.Bounds.Height >= (double) layoutedFractionWidget1.Denominator.Bounds.Height ? layoutedFractionWidget1.Numerator.Bounds.Height : layoutedFractionWidget1.Denominator.Bounds.Height;
        float y2 = layoutedFractionWidget1.Numerator.Bounds.Y;
        RectangleF bounds = layoutedFractionWidget1.Numerator.Bounds;
        double y3 = (double) bounds.Y;
        bounds = layoutedFractionWidget1.Denominator.Bounds;
        double y4 = (double) bounds.Y;
        if (y3 > y4)
        {
          bounds = layoutedFractionWidget1.Denominator.Bounds;
          y2 = bounds.Y;
        }
        LayoutedFractionWidget layoutedFractionWidget2 = layoutedFractionWidget1;
        bounds = layoutedFractionWidget1.Numerator.Bounds;
        RectangleF rectangleF = new RectangleF(bounds.X, y2, width4, height3);
        layoutedFractionWidget2.Bounds = rectangleF;
        break;
    }
    return layoutedFractionWidget1;
  }

  private LayoutedDelimiterWidget LayoutDelimiterSwitch(
    RectangleF currentBounds,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathFunctionBase mathFunction)
  {
    LayoutedDelimiterWidget layoutedDelimiterWidget = new LayoutedDelimiterWidget(mathFunction);
    IOfficeMathDelimiter officeMathDelimiter = mathFunction as IOfficeMathDelimiter;
    List<LayoutedOMathWidget> layoutedOmathWidgetList = new List<LayoutedOMathWidget>();
    RectangleF clientActiveArea = currentBounds;
    WCharacterFormat controlProperties = officeMathDelimiter.ControlProperties as WCharacterFormat;
    Font font = this.Document.FontSettings.GetFont(controlProperties.GetFontNameToRender(FontScriptType.English), controlProperties.GetFontSizeToRender(), FontStyle.Regular);
    if (!string.IsNullOrEmpty(officeMathDelimiter.BeginCharacter))
    {
      LayoutedStringWidget layoutedStringWidget = new LayoutedStringWidget();
      layoutedStringWidget.Text = officeMathDelimiter.BeginCharacter[0].ToString();
      layoutedStringWidget.Font = font;
      SizeF sizeF = this.DrawingContext.MeasureString(layoutedStringWidget.Text, layoutedStringWidget.Font, this.DrawingContext.StringFormt);
      layoutedStringWidget.Bounds = new RectangleF(currentBounds.X, currentBounds.Y, sizeF.Width, sizeF.Height);
      clientActiveArea.X += sizeF.Width;
      clientActiveArea.Width -= sizeF.Width;
      layoutedDelimiterWidget.BeginCharacter = layoutedStringWidget;
    }
    if (!string.IsNullOrEmpty(officeMathDelimiter.Seperator))
    {
      LayoutedStringWidget layoutedStringWidget = new LayoutedStringWidget();
      layoutedStringWidget.Text = officeMathDelimiter.Seperator[0].ToString();
      layoutedStringWidget.Font = font;
      SizeF sizeF = this.DrawingContext.MeasureString(layoutedStringWidget.Text, layoutedStringWidget.Font, this.DrawingContext.StringFormt);
      layoutedStringWidget.Bounds = new RectangleF(currentBounds.X, currentBounds.Y, sizeF.Width, sizeF.Height);
      layoutedDelimiterWidget.Seperator = layoutedStringWidget;
    }
    float num1 = 0.0f;
    float y = currentBounds.Y;
    int index1 = 0;
    for (int index2 = 0; index2 < officeMathDelimiter.Equation.Count; ++index2)
    {
      LayoutedOMathWidget layoutedOmathWidget = this.LayoutOfficeMath(clientActiveArea, officeMathDelimiter.Equation[index2]);
      clientActiveArea.X += layoutedOmathWidget.Bounds.Width;
      clientActiveArea.Width -= layoutedOmathWidget.Bounds.Width;
      if (layoutedDelimiterWidget.Seperator != null && index2 != officeMathDelimiter.Equation.Count - 1)
      {
        clientActiveArea.X += layoutedDelimiterWidget.Seperator.Bounds.Width;
        clientActiveArea.Width -= layoutedDelimiterWidget.Seperator.Bounds.Width;
      }
      if ((double) num1 < (double) layoutedOmathWidget.Bounds.Height)
      {
        num1 = layoutedOmathWidget.Bounds.Height;
        index1 = index2;
      }
      layoutedOmathWidgetList.Add(layoutedOmathWidget);
    }
    float verticalCenterPoint1 = this.GetVerticalCenterPoint(layoutedOmathWidgetList[index1]);
    float yPosition1 = 0.0f;
    if (officeMathDelimiter.IsGrow && officeMathDelimiter.DelimiterShape == MathDelimiterShapeType.Centered)
    {
      yPosition1 = layoutedOmathWidgetList[index1].Bounds.Height - verticalCenterPoint1 * 2f;
      if ((double) yPosition1 > 0.0)
        verticalCenterPoint1 += yPosition1;
    }
    for (int index3 = 0; index3 < layoutedOmathWidgetList.Count; ++index3)
    {
      if (index3 != index1)
      {
        LayoutedOMathWidget officeMathWidget = layoutedOmathWidgetList[index3];
        float verticalCenterPoint2 = this.GetVerticalCenterPoint(officeMathWidget);
        float yPosition2 = verticalCenterPoint1 - verticalCenterPoint2;
        officeMathWidget.ShiftXYPosition(0.0f, yPosition2);
        if ((double) y > (double) officeMathWidget.Bounds.Y)
          y = officeMathWidget.Bounds.Y;
      }
      else if ((double) yPosition1 > 0.0)
        layoutedOmathWidgetList[index3].ShiftXYPosition(0.0f, yPosition1);
    }
    if ((double) y < (double) currentBounds.Y)
    {
      float yPosition3 = currentBounds.Y - y;
      for (int index4 = 0; index4 < layoutedOmathWidgetList.Count; ++index4)
        layoutedOmathWidgetList[index4].ShiftXYPosition(0.0f, yPosition3, true);
    }
    float num2 = 0.0f;
    for (int index5 = 0; index5 < layoutedOmathWidgetList.Count; ++index5)
    {
      LayoutedOMathWidget layoutedOmathWidget = layoutedOmathWidgetList[index5];
      if ((double) num2 < (double) layoutedOmathWidget.Bounds.Bottom)
        num2 = layoutedOmathWidget.Bounds.Bottom;
    }
    if (!string.IsNullOrEmpty(officeMathDelimiter.EndCharacter))
    {
      LayoutedStringWidget layoutedStringWidget = new LayoutedStringWidget();
      layoutedStringWidget.Text = officeMathDelimiter.EndCharacter[0].ToString();
      layoutedStringWidget.Font = font;
      SizeF sizeF = this.DrawingContext.MeasureString(layoutedStringWidget.Text, layoutedStringWidget.Font, this.DrawingContext.StringFormt);
      layoutedStringWidget.Bounds = new RectangleF(clientActiveArea.X, clientActiveArea.Y, sizeF.Width, sizeF.Height);
      clientActiveArea.X += sizeF.Width;
      layoutedDelimiterWidget.EndCharacter = layoutedStringWidget;
    }
    if (layoutedDelimiterWidget.BeginCharacter != null)
    {
      if (!officeMathDelimiter.IsGrow || officeMathDelimiter.IsGrow && !this.IsStretchableCharacter(officeMathDelimiter.BeginCharacter[0]))
      {
        float yPosition4 = verticalCenterPoint1 - layoutedDelimiterWidget.BeginCharacter.Bounds.Height / 2f;
        layoutedDelimiterWidget.BeginCharacter.ShiftXYPosition(0.0f, yPosition4);
      }
      else
        layoutedDelimiterWidget.BeginCharacter.IsStretchable = true;
    }
    if (layoutedDelimiterWidget.Seperator != null)
    {
      if (!officeMathDelimiter.IsGrow || officeMathDelimiter.IsGrow && !this.IsStretchableCharacter(officeMathDelimiter.Seperator[0]))
      {
        float yPosition5 = verticalCenterPoint1 - layoutedDelimiterWidget.Seperator.Bounds.Height / 2f;
        layoutedDelimiterWidget.Seperator.ShiftXYPosition(0.0f, yPosition5);
      }
      else
        layoutedDelimiterWidget.Seperator.IsStretchable = true;
    }
    if (layoutedDelimiterWidget.EndCharacter != null)
    {
      if (!officeMathDelimiter.IsGrow || officeMathDelimiter.IsGrow && !this.IsStretchableCharacter(officeMathDelimiter.EndCharacter[0]))
      {
        float yPosition6 = verticalCenterPoint1 - layoutedDelimiterWidget.EndCharacter.Bounds.Height / 2f;
        layoutedDelimiterWidget.EndCharacter.ShiftXYPosition(0.0f, yPosition6);
      }
      else
        layoutedDelimiterWidget.EndCharacter.IsStretchable = true;
    }
    if ((double) yPosition1 < 0.0)
      num2 += -yPosition1;
    float width = clientActiveArea.X - currentBounds.X;
    layoutedDelimiterWidget.Equation = layoutedOmathWidgetList;
    layoutedDelimiterWidget.Bounds = new RectangleF(currentBounds.X, currentBounds.Y, width, num2 - currentBounds.Y);
    return layoutedDelimiterWidget;
  }

  private bool IsStretchableCharacter(char character)
  {
    if (this.stretchableCharacters == null)
    {
      this.stretchableCharacters = new List<char>();
      this.stretchableCharacters.Add('(');
      this.stretchableCharacters.Add(')');
      this.stretchableCharacters.Add('[');
      this.stretchableCharacters.Add(']');
      this.stretchableCharacters.Add('{');
      this.stretchableCharacters.Add('}');
      this.stretchableCharacters.Add('⌊');
      this.stretchableCharacters.Add('⌋');
      this.stretchableCharacters.Add('⌈');
      this.stretchableCharacters.Add('⌉');
      this.stretchableCharacters.Add('|');
      this.stretchableCharacters.Add('‖');
      this.stretchableCharacters.Add('⟦');
      this.stretchableCharacters.Add('⟧');
      this.stretchableCharacters.Add('⟨');
      this.stretchableCharacters.Add('⟩');
    }
    return this.stretchableCharacters.Contains(character);
  }

  private LayoutedMathFunctionWidget LayoutMathFunctionWidget(
    RectangleF currentBounds,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathFunctionBase mathFunction)
  {
    LayoutedMathFunctionWidget mathFunctionWidget = new LayoutedMathFunctionWidget(mathFunction);
    IOfficeMathFunction officeMathFunction = mathFunction as IOfficeMathFunction;
    mathFunctionWidget.FunctionName = this.LayoutOfficeMath(currentBounds, officeMathFunction.FunctionName);
    mathFunctionWidget.Equation = this.LayoutOfficeMath(currentBounds, officeMathFunction.Equation);
    SizeF sizeF = this.DrawingContext.MeasureString(" ", (officeMathFunction.ControlProperties as WCharacterFormat).GetFontToRender(FontScriptType.English), (StringFormat) null);
    float xPosition = mathFunctionWidget.FunctionName.Bounds.Width + sizeF.Width;
    float yPosition = 0.0f;
    if ((double) mathFunctionWidget.FunctionName.Bounds.Height < (double) mathFunctionWidget.Equation.Bounds.Height)
    {
      float verticalCenterPoint1 = this.GetVerticalCenterPoint(mathFunctionWidget.Equation);
      float verticalCenterPoint2 = this.GetVerticalCenterPoint(mathFunctionWidget.FunctionName);
      mathFunctionWidget.FunctionName.ShiftXYPosition(0.0f, verticalCenterPoint1 - verticalCenterPoint2);
    }
    else
      yPosition = this.GetVerticalCenterPoint(mathFunctionWidget.FunctionName) - this.GetVerticalCenterPoint(mathFunctionWidget.Equation);
    mathFunctionWidget.Equation.ShiftXYPosition(xPosition, yPosition);
    float width = mathFunctionWidget.Equation.Bounds.Right - mathFunctionWidget.FunctionName.Bounds.X;
    float height = mathFunctionWidget.FunctionName.Bounds.Height;
    if ((double) height < (double) mathFunctionWidget.Equation.Bounds.Height)
      height = mathFunctionWidget.Equation.Bounds.Height;
    mathFunctionWidget.Bounds = new RectangleF(currentBounds.X, currentBounds.Y, width, height);
    return mathFunctionWidget;
  }

  private LayoutedBoderBoxWidget LayoutBoderBoxWidget(
    RectangleF currentBounds,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathFunctionBase mathFunction)
  {
    LayoutedBoderBoxWidget borderBoxWidget = new LayoutedBoderBoxWidget(mathFunction);
    IOfficeMathBorderBox borderBox = mathFunction as IOfficeMathBorderBox;
    borderBoxWidget.Equation = this.LayoutOfficeMath(currentBounds, borderBox.Equation);
    WCharacterFormat controlProperties = borderBox.ControlProperties as WCharacterFormat;
    Font fontToRender = controlProperties.GetFontToRender(FontScriptType.English);
    RectangleF bounds = borderBoxWidget.Equation.Bounds;
    float borderWidth = (float) (((double) this.DrawingContext.GetAscent(fontToRender) - (double) this.DrawingContext.GetDescent(fontToRender)) / 10.0);
    borderBoxWidget.BorderLines = this.GenerateBorderBox(borderBox, borderBoxWidget, ref bounds, fontToRender, controlProperties, borderWidth);
    borderBoxWidget.Bounds = new RectangleF(bounds.X - borderWidth / 2f, bounds.Y - borderWidth / 2f, bounds.Width + borderWidth, bounds.Height + borderWidth);
    return borderBoxWidget;
  }

  private LayoutedBarWidget LayoutBarWidget(
    RectangleF currentBounds,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathFunctionBase mathFunction)
  {
    LayoutedBarWidget layoutedBarWidget = new LayoutedBarWidget(mathFunction);
    IOfficeMathBar bar = mathFunction as IOfficeMathBar;
    layoutedBarWidget.Equation = this.LayoutOfficeMath(currentBounds, bar.Equation);
    WCharacterFormat controlProperties = bar.ControlProperties as WCharacterFormat;
    Font fontToRender = controlProperties.GetFontToRender(FontScriptType.English);
    RectangleF bounds = layoutedBarWidget.Equation.Bounds;
    float barWidth = (float) (((double) this.DrawingContext.GetAscent(fontToRender) - (double) this.DrawingContext.GetDescent(fontToRender)) / 10.0);
    layoutedBarWidget.BarLine = this.GenerateBarline(bar, layoutedBarWidget, ref bounds, fontToRender, controlProperties, barWidth);
    layoutedBarWidget.Bounds = bounds;
    return layoutedBarWidget;
  }

  private LayoutedMatrixWidget LayoutMatrixWidget(
    RectangleF currentBounds,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathFunctionBase mathFunction)
  {
    LayoutedMatrixWidget layoutedMatrixWidget = new LayoutedMatrixWidget(mathFunction);
    IOfficeMathMatrix matrix = mathFunction as IOfficeMathMatrix;
    layoutedMatrixWidget.Rows = new List<List<LayoutedOMathWidget>>();
    Font fontToRender = (matrix.ControlProperties as WCharacterFormat).GetFontToRender(FontScriptType.English);
    float rowSpacing = this.GetRowSpacing(matrix.RowSpacing, fontToRender, matrix.RowSpacingRule);
    float columnSpacing = this.GetColumnSpacing(matrix.ColumnSpacing, fontToRender, matrix.ColumnSpacingRule);
    float x = currentBounds.X;
    float y = currentBounds.Y;
    float previousLowerHeight = 0.0f;
    int num1 = 0;
    List<LayoutedOMathWidget> layoutedCellCollection = (List<LayoutedOMathWidget>) null;
    LayoutedOMathWidget maxCellWidget = (LayoutedOMathWidget) null;
    for (int index1 = 0; index1 < matrix.Rows.Count; ++index1)
    {
      IOfficeMaths arguments = matrix.Rows[index1].Arguments;
      layoutedCellCollection = new List<LayoutedOMathWidget>();
      float maxRowHeight = 0.0f;
      for (int index2 = 0; index2 < matrix.Columns.Count; ++index2)
      {
        MathHorizontalAlignment horizontalAlignment = matrix.Columns[index2].HorizontalAlignment;
        LayoutedOMathWidget layoutedOmathWidget = this.LayoutOfficeMath(currentBounds, arguments[index2]);
        float num2 = columnSpacing + matrix.ColumnWidth;
        switch (horizontalAlignment)
        {
          case MathHorizontalAlignment.Center:
            layoutedOmathWidget.ShiftXYPosition((float) ((double) matrix.ColumnWidth / 2.0 - (double) layoutedOmathWidget.Bounds.Width / 2.0), 0.0f);
            break;
          case MathHorizontalAlignment.Right:
            layoutedOmathWidget.ShiftXYPosition(matrix.ColumnWidth - layoutedOmathWidget.Bounds.Width, 0.0f);
            break;
        }
        currentBounds.X += num2;
        layoutedCellCollection.Add(layoutedOmathWidget);
        if ((double) maxRowHeight < (double) layoutedOmathWidget.Bounds.Height)
        {
          maxRowHeight = layoutedOmathWidget.Bounds.Height;
          num1 = index2;
        }
      }
      this.AlignCellsVertically(layoutedCellCollection, num1, rowSpacing, ref currentBounds, matrix, maxRowHeight, ref previousLowerHeight);
      layoutedMatrixWidget.Rows.Add(layoutedCellCollection);
      currentBounds.X = x;
    }
    RectangleF empty = RectangleF.Empty;
    for (int index3 = 0; index3 < matrix.Columns.Count; ++index3)
    {
      float num3 = 0.0f;
      int num4 = 0;
      MathHorizontalAlignment horizontalAlignment = matrix.Columns[index3].HorizontalAlignment;
      for (int index4 = 0; index4 < matrix.Rows.Count; ++index4)
      {
        LayoutedOMathWidget layoutedOmathWidget = layoutedMatrixWidget.Rows[index4][index3];
        if ((double) num3 < (double) layoutedOmathWidget.Bounds.Width)
        {
          num3 = layoutedOmathWidget.Bounds.Width;
          num4 = index4;
        }
      }
      maxCellWidget = layoutedMatrixWidget.Rows[num4][index3];
      this.AlignCellsHorizontally(ref empty, x, index3, maxCellWidget, layoutedMatrixWidget, horizontalAlignment, columnSpacing, num4, matrix);
    }
    float width = maxCellWidget.Bounds.Right - x;
    float height = layoutedCellCollection[num1].Bounds.Bottom - y;
    layoutedMatrixWidget.Bounds = new RectangleF(x, y, width, height);
    return layoutedMatrixWidget;
  }

  internal void AlignCellsVertically(
    List<LayoutedOMathWidget> layoutedCellCollection,
    int maxHeightCellIndex,
    float rowSpacing,
    ref RectangleF currentBounds,
    IOfficeMathMatrix matrix,
    float maxRowHeight,
    ref float previousLowerHeight)
  {
    float num1 = layoutedCellCollection[maxHeightCellIndex].Bounds.Y + this.GetVerticalCenterPoint(layoutedCellCollection[maxHeightCellIndex]);
    float num2 = num1 - layoutedCellCollection[maxHeightCellIndex].Bounds.Y;
    float num3 = layoutedCellCollection[maxHeightCellIndex].Bounds.Bottom - num1;
    if ((double) num2 > (double) rowSpacing || (double) num3 > (double) rowSpacing)
    {
      for (int index = 0; index < layoutedCellCollection.Count; ++index)
      {
        if (index != maxHeightCellIndex)
          layoutedCellCollection[index].ShiftXYPosition(0.0f, num2 - layoutedCellCollection[index].Bounds.Height / 2f);
      }
      currentBounds.Y = layoutedCellCollection[maxHeightCellIndex].Bounds.Bottom;
    }
    else if (matrix.RowSpacingRule == SpacingRule.Single)
      currentBounds.Y += maxRowHeight;
    else
      currentBounds.Y += rowSpacing;
    previousLowerHeight = num3;
  }

  internal void AlignCellsHorizontally(
    ref RectangleF previousMaxCellBounds,
    float xPosition,
    int columnIndex,
    LayoutedOMathWidget maxCellWidget,
    LayoutedMatrixWidget layoutedMatrixWidget,
    MathHorizontalAlignment columnAlignment,
    float columnSpacing,
    int maxCellWidthIndex,
    IOfficeMathMatrix matrix)
  {
    float xPosition1 = 0.0f;
    if (previousMaxCellBounds != RectangleF.Empty || (double) maxCellWidget.Bounds.X < (double) xPosition)
    {
      if ((double) maxCellWidget.Bounds.X < (double) xPosition && previousMaxCellBounds == RectangleF.Empty)
        xPosition1 = xPosition - maxCellWidget.Bounds.X;
      else if ((double) maxCellWidget.Bounds.X < (double) previousMaxCellBounds.Right)
        xPosition1 = previousMaxCellBounds.Right - maxCellWidget.Bounds.X + columnSpacing;
      maxCellWidget.ShiftXYPosition(xPosition1, 0.0f);
      float num = maxCellWidget.Bounds.Width / 2f + maxCellWidget.Bounds.X;
      for (int index = 0; index < matrix.Rows.Count; ++index)
      {
        if (index != maxCellWidthIndex)
        {
          LayoutedOMathWidget layoutedOmathWidget = layoutedMatrixWidget.Rows[index][columnIndex];
          switch (columnAlignment)
          {
            case MathHorizontalAlignment.Center:
              xPosition1 = num - (layoutedOmathWidget.Bounds.Width / 2f + layoutedOmathWidget.Bounds.X);
              break;
            case MathHorizontalAlignment.Left:
              xPosition1 = maxCellWidget.Bounds.X - layoutedOmathWidget.Bounds.X;
              break;
            case MathHorizontalAlignment.Right:
              xPosition1 = maxCellWidget.Bounds.Right - layoutedOmathWidget.Bounds.Right;
              break;
          }
          layoutedMatrixWidget.Rows[index][columnIndex].ShiftXYPosition(xPosition1, 0.0f);
        }
      }
    }
    previousMaxCellBounds = layoutedMatrixWidget.Rows[maxCellWidthIndex][columnIndex].Bounds;
  }

  internal LayoutedLineWidget GenerateBarline(
    IOfficeMathBar bar,
    LayoutedBarWidget layoutedBarWidget,
    ref RectangleF innerBounds,
    Font controlFont,
    WCharacterFormat characterFormat,
    float barWidth)
  {
    LayoutedLineWidget barline = new LayoutedLineWidget();
    SizeF sizeF = this.DrawingContext.MeasureString(" ", controlFont, (StringFormat) null);
    Color textColor = this.DrawingContext.GetTextColor(characterFormat);
    RectangleF rectangleF = innerBounds;
    if (bar.BarTop)
    {
      barline.Point1 = new PointF(rectangleF.Left, rectangleF.Top + sizeF.Width / 2f);
      barline.Point2 = new PointF(rectangleF.Right, rectangleF.Top + sizeF.Width / 2f);
    }
    else
    {
      barline.Point1 = new PointF(rectangleF.Left, rectangleF.Bottom);
      barline.Point2 = new PointF(rectangleF.Right, rectangleF.Bottom);
    }
    layoutedBarWidget.Equation.ShiftXYPosition(0.0f, sizeF.Width);
    innerBounds.Height += sizeF.Width;
    barline.Color = textColor;
    barline.Width = barWidth;
    return barline;
  }

  internal List<LayoutedLineWidget> GenerateBorderBox(
    IOfficeMathBorderBox borderBox,
    LayoutedBoderBoxWidget borderBoxWidget,
    ref RectangleF innerBounds,
    Font controlFont,
    WCharacterFormat characterFormat,
    float borderWidth)
  {
    List<LayoutedLineWidget> borderBox1 = new List<LayoutedLineWidget>();
    SizeF sizeF = this.DrawingContext.MeasureString(" ", controlFont, (StringFormat) null);
    RectangleF rectangleF = innerBounds;
    Color textColor = this.DrawingContext.GetTextColor(characterFormat);
    if (!borderBox.HideRight)
    {
      rectangleF.Width += sizeF.Width * 2f;
      innerBounds.Width += sizeF.Width * 2f;
    }
    if (!borderBox.HideBottom && !this.IsNested())
    {
      rectangleF.Height += sizeF.Width * 2f;
      innerBounds.Height += sizeF.Width * 2f;
    }
    if (!borderBox.HideLeft)
    {
      PointF pointF1 = new PointF(rectangleF.Left, rectangleF.Top - borderWidth / 2f);
      PointF pointF2 = new PointF(rectangleF.Left, rectangleF.Bottom);
      borderBox1.Add(new LayoutedLineWidget()
      {
        Point1 = pointF1,
        Point2 = pointF2,
        Color = textColor,
        Width = borderWidth
      });
      borderBoxWidget.Equation.ShiftXYPosition(sizeF.Width, 0.0f);
      innerBounds.X += sizeF.Width;
    }
    if (!borderBox.HideTop)
    {
      PointF pointF3 = new PointF(rectangleF.Left, rectangleF.Top);
      PointF pointF4 = new PointF(rectangleF.Right, rectangleF.Top);
      borderBox1.Add(new LayoutedLineWidget()
      {
        Point1 = pointF3,
        Point2 = pointF4,
        Color = textColor,
        Width = borderWidth
      });
      if (!this.IsNested())
      {
        borderBoxWidget.Equation.ShiftXYPosition(0.0f, sizeF.Width);
        innerBounds.Y += sizeF.Width;
      }
      else
      {
        borderBoxWidget.Equation.ShiftXYPosition(0.0f, sizeF.Width * 0.5f);
        innerBounds.Y += sizeF.Width * 0.5f;
        if (this.IsNested())
          innerBounds.Height += sizeF.Width;
      }
    }
    if (!borderBox.HideRight)
    {
      PointF pointF5 = new PointF(rectangleF.Right, rectangleF.Top - borderWidth / 2f);
      PointF pointF6 = new PointF(rectangleF.Right, rectangleF.Bottom);
      borderBox1.Add(new LayoutedLineWidget()
      {
        Point1 = pointF5,
        Point2 = pointF6,
        Color = textColor,
        Width = borderWidth
      });
    }
    if (!borderBox.HideBottom)
    {
      PointF pointF7 = new PointF(rectangleF.Left, rectangleF.Bottom - borderWidth / 2f);
      PointF pointF8 = new PointF(rectangleF.Right, rectangleF.Bottom - borderWidth / 2f);
      borderBox1.Add(new LayoutedLineWidget()
      {
        Point1 = pointF7,
        Point2 = pointF8,
        Color = textColor,
        Width = borderWidth
      });
    }
    if (borderBox.StrikeHorizontal)
    {
      PointF pointF9 = new PointF(rectangleF.Left, rectangleF.Top + rectangleF.Height / 2f);
      PointF pointF10 = new PointF(rectangleF.Right, rectangleF.Top + rectangleF.Height / 2f);
      borderBox1.Add(new LayoutedLineWidget()
      {
        Point1 = pointF9,
        Point2 = pointF10,
        Color = textColor,
        Width = borderWidth
      });
    }
    if (borderBox.StrikeVertical)
    {
      PointF pointF11 = new PointF(rectangleF.Left + rectangleF.Width / 2f, rectangleF.Top);
      PointF pointF12 = new PointF(rectangleF.Left + rectangleF.Width / 2f, rectangleF.Bottom);
      borderBox1.Add(new LayoutedLineWidget()
      {
        Point1 = pointF11,
        Point2 = pointF12,
        Color = textColor,
        Width = borderWidth
      });
    }
    if (borderBox.StrikeDiagonalDown)
    {
      PointF pointF13 = new PointF(rectangleF.Left, rectangleF.Top);
      PointF pointF14 = new PointF(rectangleF.Right, rectangleF.Bottom);
      borderBox1.Add(new LayoutedLineWidget()
      {
        Point1 = pointF13,
        Point2 = pointF14,
        Color = textColor,
        Width = borderWidth
      });
    }
    if (borderBox.StrikeDiagonalUp)
    {
      PointF pointF15 = new PointF(rectangleF.Right, rectangleF.Top);
      PointF pointF16 = new PointF(rectangleF.Left, rectangleF.Bottom);
      borderBox1.Add(new LayoutedLineWidget()
      {
        Point1 = pointF15,
        Point2 = pointF16,
        Color = textColor,
        Width = borderWidth
      });
    }
    return borderBox1;
  }

  private LayoutedEquationArrayWidget LayoutEquationArraySwitch(
    RectangleF currentBounds,
    LayoutedOMathWidget officeMathLayoutedWidget,
    IOfficeMathFunctionBase mathFunction)
  {
    LayoutedEquationArrayWidget equationArrayWidget = new LayoutedEquationArrayWidget(mathFunction);
    IOfficeMathEquationArray equationArray = mathFunction as IOfficeMathEquationArray;
    List<List<IOfficeMath>> splittedEquationArray = new List<List<IOfficeMath>>();
    for (int index = 0; index < equationArray.Equation.Count; ++index)
    {
      IOfficeMath officeMath = equationArray.Equation[index];
      splittedEquationArray.Add(new List<IOfficeMath>()
      {
        officeMath
      });
    }
    int count1 = splittedEquationArray.Count;
    if (equationArray.ExpandEquationContainer)
      this.SplitEquationArray(splittedEquationArray, equationArray);
    int count2 = splittedEquationArray[0].Count;
    bool flag = true;
    float num1 = 0.0f;
    this.HasNestedFunction(MathFunctionType.Fraction);
    WCharacterFormat controlProperties = equationArray.ControlProperties as WCharacterFormat;
    if ((double) controlProperties.ReducedFontSize == 1.0)
    {
      flag = false;
      controlProperties.ReducedFontSize = 0.0f;
    }
    List<List<LayoutedOMathWidget>> layoutedOmathWidgetListList = new List<List<LayoutedOMathWidget>>();
    for (int index = 0; index < equationArray.Equation.Count; ++index)
    {
      List<LayoutedOMathWidget> layoutedOmathWidgetList = new List<LayoutedOMathWidget>();
      layoutedOmathWidgetListList.Add(layoutedOmathWidgetList);
    }
    Font fontToRender = controlProperties.GetFontToRender(FontScriptType.English);
    if (flag)
      num1 = this.GetRowSpacing(equationArray.RowSpacing, fontToRender, equationArray.RowSpacingRule);
    float height1 = this.DrawingContext.MeasureString(" ", fontToRender, (StringFormat) null).Height;
    float height2 = 0.0f;
    float width1 = 0.0f;
    float num2 = 0.0f;
    float y = currentBounds.Y;
    float height3 = currentBounds.Height;
    for (int index1 = 0; index1 < count1; ++index1)
    {
      float x = currentBounds.X;
      float width2 = currentBounds.Width;
      float num3 = 0.0f;
      int index2 = 0;
      float num4 = 0.0f;
      int count3 = splittedEquationArray[index1].Count;
      for (int index3 = 0; index3 < count3; ++index3)
      {
        LayoutedOMathWidget layoutedOmathWidget = this.LayoutOfficeMath(new RectangleF(x, y, width2, height3), splittedEquationArray[index1][index3]);
        layoutedOmathWidgetListList[index1].Add(layoutedOmathWidget);
        if ((double) num3 < (double) layoutedOmathWidget.Bounds.Height)
        {
          num3 += layoutedOmathWidget.Bounds.Height;
          index2 = index3;
        }
        width2 -= layoutedOmathWidget.Bounds.Width;
      }
      float num5 = num3;
      float num6 = num1;
      float verticalCenterPoint1 = this.GetVerticalCenterPoint(layoutedOmathWidgetListList[index1][index2]);
      if (index1 != 1)
        num6 -= height1 / 2f;
      if (index1 > 0)
      {
        if ((double) y > (double) num2)
          num6 -= y - num2;
        num4 = num6 - verticalCenterPoint1;
        if ((double) num4 > 0.0)
          num5 += num4;
        num2 = y + num1;
      }
      else
        num2 = y + verticalCenterPoint1;
      for (int index4 = 0; index4 < count3; ++index4)
      {
        LayoutedOMathWidget officeMathWidget = layoutedOmathWidgetListList[index1][index4];
        float verticalCenterPoint2 = this.GetVerticalCenterPoint(officeMathWidget);
        float yPosition = verticalCenterPoint1 - verticalCenterPoint2;
        if ((double) num4 > 0.0)
          yPosition += num4;
        officeMathWidget.ShiftXYPosition(0.0f, yPosition);
      }
      height2 += num5;
      y += num5;
      height3 -= num5;
    }
    float num7 = 0.0f;
    if (equationArray.ExpandEquationContainer)
      num7 = !equationArray.ExpandEquationContent || count2 == 1 ? this.ContainerSize.Width / (float) (count2 + 1) : this.ContainerSize.Width / (float) (count2 - 1);
    for (int index5 = 0; index5 < count2; ++index5)
    {
      float num8 = 0.0f;
      float num9 = 0.0f;
      for (int index6 = 0; index6 < count1; ++index6)
      {
        if ((double) num8 < (double) layoutedOmathWidgetListList[index6][index5].Bounds.Width)
          num8 = layoutedOmathWidgetListList[index6][index5].Bounds.Width;
      }
      if (equationArray.ExpandEquationContainer && (index5 != 0 || !equationArray.ExpandEquationContent || count2 == 1))
        num9 = !equationArray.ExpandEquationContent ? num7 - num8 / 2f : num7 - num8;
      for (int index7 = 0; index7 < count1; ++index7)
      {
        LayoutedOMathWidget layoutedOmathWidget = layoutedOmathWidgetListList[index7][index5];
        float xPosition = (float) (((double) num8 - (double) layoutedOmathWidget.Bounds.Width) / 2.0) + (num9 + width1);
        layoutedOmathWidget.ShiftXYPosition(xPosition, 0.0f);
      }
      width1 += num8 + num9;
    }
    equationArrayWidget.Equation = layoutedOmathWidgetListList;
    equationArrayWidget.Bounds = new RectangleF(currentBounds.X, currentBounds.Y, width1, height2);
    return equationArrayWidget;
  }

  private void SplitEquationArray(
    List<List<IOfficeMath>> splittedEquationArray,
    IOfficeMathEquationArray equationArray)
  {
    int count1 = splittedEquationArray.Count;
    string[] separator = new string[1]{ "&&" };
    for (int index1 = 0; index1 < count1; ++index1)
    {
      List<IOfficeMath> splittedEquation = splittedEquationArray[index1];
      int count2 = splittedEquation.Count;
      for (int index2 = 0; index2 < count2; ++index2)
      {
        IOfficeMath officeMath1 = splittedEquation[index2];
        int count3 = officeMath1.Functions.Count;
        for (int index3 = 0; index3 < count3; ++index3)
        {
          IOfficeMathFunctionBase function = officeMath1.Functions[index3];
          if (function is IOfficeMathRunElement && (function as IOfficeMathRunElement).Item is WTextRange && ((function as IOfficeMathRunElement).Item as WTextRange).Text.Contains(separator[0]))
          {
            WTextRange wtextRange1 = (function as IOfficeMathRunElement).Item as WTextRange;
            string[] strArray = wtextRange1.Text.Split(separator, StringSplitOptions.None);
            float length = (float) strArray.Length;
            for (int index4 = 0; (double) index4 < (double) length; ++index4)
            {
              if (index4 == 0)
              {
                wtextRange1.Text = strArray[index4];
              }
              else
              {
                if (index4 == strArray.Length - 1)
                {
                  IOfficeMath officeMath2 = (IOfficeMath) new OfficeMath((IOfficeMathEntity) equationArray);
                  OfficeMathRunElement officeMathRunElement = officeMath2.Functions.Add(MathFunctionType.RunElement) as OfficeMathRunElement;
                  WTextRange wtextRange2 = wtextRange1.Clone() as WTextRange;
                  wtextRange2.Text = strArray[index4];
                  officeMathRunElement.Item = (IOfficeRun) wtextRange2;
                  if (index3 + 1 < count3)
                  {
                    (officeMath1.Functions as OfficeMathBaseCollection).CloneItemsTo(officeMath2.Functions as OfficeMathBaseCollection, index3 + 1, count3 - 1);
                    int index5 = index3 + 1;
                    while (index3 + 1 < officeMath1.Functions.Count)
                      (officeMath1.Functions as OfficeMathBaseCollection).Remove((IOfficeMathEntity) officeMath1.Functions[index5]);
                  }
                  splittedEquation.Add(officeMath2);
                  ++count2;
                  break;
                }
                IOfficeMath officeMath3 = (IOfficeMath) new OfficeMath((IOfficeMathEntity) equationArray);
                OfficeMathRunElement officeMathRunElement1 = officeMath3.Functions.Add(MathFunctionType.RunElement) as OfficeMathRunElement;
                WTextRange wtextRange3 = wtextRange1.Clone() as WTextRange;
                wtextRange3.Text = strArray[index4];
                officeMathRunElement1.Item = (IOfficeRun) wtextRange3;
                splittedEquation.Insert(index2 + 1, officeMath3);
                ++index2;
                ++count2;
              }
            }
            break;
          }
        }
      }
      if (index1 != 0 && count2 > splittedEquationArray[index1 - 1].Count)
      {
        int num = count2 - splittedEquationArray[index1 - 1].Count;
        for (int index6 = 0; index6 < index1; ++index6)
        {
          for (int index7 = 0; index7 < num; ++index7)
          {
            IOfficeMath officeMath = (IOfficeMath) new OfficeMath((IOfficeMathEntity) equationArray);
            (officeMath.Functions.Add(MathFunctionType.RunElement) as OfficeMathRunElement).Item = (IOfficeRun) new WTextRange((IWordDocument) this.Document);
            splittedEquationArray[index6].Add(officeMath);
          }
        }
      }
      else if (index1 != 0 && count2 < splittedEquationArray[index1 - 1].Count)
      {
        int num = splittedEquationArray[index1 - 1].Count - count2;
        for (int index8 = 0; index8 < num; ++index8)
        {
          IOfficeMath officeMath = (IOfficeMath) new OfficeMath((IOfficeMathEntity) equationArray);
          (officeMath.Functions.Add(MathFunctionType.RunElement) as OfficeMathRunElement).Item = (IOfficeRun) new WTextRange((IWordDocument) this.Document);
          splittedEquationArray[index1].Add(officeMath);
        }
      }
    }
  }

  private float GetRowSpacing(float inputRowSpacing, Font font, SpacingRule spacingRule)
  {
    SizeF sizeF = this.DrawingContext.MeasureString(" ", font, (StringFormat) null);
    switch (spacingRule)
    {
      case SpacingRule.OneAndHalf:
      case SpacingRule.Double:
      case SpacingRule.Multiple:
        if (spacingRule == SpacingRule.Double)
          inputRowSpacing = 2f;
        else if (spacingRule == SpacingRule.OneAndHalf)
          inputRowSpacing = 1.5f;
        return sizeF.Height * inputRowSpacing;
      case SpacingRule.Exactly:
        return inputRowSpacing;
      default:
        return 0.0f;
    }
  }

  private float GetColumnSpacing(float columnSpacing, Font font, SpacingRule spacingRule)
  {
    float columnSpacing1 = 0.0f;
    switch (spacingRule)
    {
      case SpacingRule.Single:
      case SpacingRule.OneAndHalf:
      case SpacingRule.Double:
      case SpacingRule.Multiple:
        if (spacingRule == SpacingRule.Single)
          columnSpacing = 1f;
        else if (spacingRule == SpacingRule.OneAndHalf)
          columnSpacing = 1.5f;
        else if (spacingRule == SpacingRule.Double)
          columnSpacing = 2f;
        else if (spacingRule == SpacingRule.Multiple)
          columnSpacing /= 12f;
        columnSpacing1 = font.SizeInPoints * columnSpacing;
        break;
      case SpacingRule.Exactly:
        columnSpacing1 = columnSpacing;
        break;
    }
    return columnSpacing1;
  }

  private void ReduceFontSizeOfOfficeMath(IOfficeMath officeMath, float fontSizeRatio)
  {
    for (int index1 = 0; index1 < officeMath.Functions.Count; ++index1)
    {
      IOfficeMathFunctionBase function = officeMath.Functions[index1];
      switch (function.Type)
      {
        case MathFunctionType.Accent:
          IOfficeMathAccent officeMathAccent = function as IOfficeMathAccent;
          this.ReduceFontSizeOfOfficeMath(officeMathAccent.Equation, fontSizeRatio);
          WCharacterFormat controlProperties1 = officeMathAccent.ControlProperties as WCharacterFormat;
          controlProperties1.ReducedFontSize = controlProperties1.FontSize * fontSizeRatio;
          break;
        case MathFunctionType.Bar:
          this.ReduceFontSizeOfOfficeMath((function as IOfficeMathBar).Equation, fontSizeRatio);
          break;
        case MathFunctionType.BorderBox:
          this.ReduceFontSizeOfOfficeMath((function as IOfficeMathBorderBox).Equation, fontSizeRatio);
          break;
        case MathFunctionType.Box:
          this.ReduceFontSizeOfOfficeMath((function as IOfficeMathBox).Equation, fontSizeRatio);
          break;
        case MathFunctionType.Delimiter:
          IOfficeMathDelimiter officeMathDelimiter = function as IOfficeMathDelimiter;
          for (int index2 = 0; index2 < officeMathDelimiter.Equation.Count; ++index2)
            this.ReduceFontSizeOfOfficeMath(officeMathDelimiter.Equation[index2], fontSizeRatio);
          break;
        case MathFunctionType.EquationArray:
          IOfficeMathEquationArray mathEquationArray = function as IOfficeMathEquationArray;
          (mathEquationArray.ControlProperties as WCharacterFormat).ReducedFontSize = 1f;
          for (int index3 = 0; index3 < mathEquationArray.Equation.Count; ++index3)
            this.ReduceFontSizeOfOfficeMath(mathEquationArray.Equation[index3], fontSizeRatio);
          break;
        case MathFunctionType.Fraction:
          IOfficeMathFraction officeMathFraction = function as IOfficeMathFraction;
          this.ReduceFontSizeOfOfficeMath(officeMathFraction.Numerator, fontSizeRatio);
          this.ReduceFontSizeOfOfficeMath(officeMathFraction.Denominator, fontSizeRatio);
          break;
        case MathFunctionType.Function:
          IOfficeMathFunction officeMathFunction = function as IOfficeMathFunction;
          this.ReduceFontSizeOfOfficeMath(officeMathFunction.FunctionName, fontSizeRatio);
          this.ReduceFontSizeOfOfficeMath(officeMathFunction.Equation, fontSizeRatio);
          break;
        case MathFunctionType.GroupCharacter:
          IOfficeMathGroupCharacter mathGroupCharacter = function as IOfficeMathGroupCharacter;
          this.ReduceFontSizeOfOfficeMath(mathGroupCharacter.Equation, fontSizeRatio);
          WCharacterFormat controlProperties2 = mathGroupCharacter.ControlProperties as WCharacterFormat;
          controlProperties2.ReducedFontSize = controlProperties2.FontSize * fontSizeRatio;
          break;
        case MathFunctionType.Limit:
          this.ReduceFontSizeOfOfficeMath((function as IOfficeMathLimit).Equation, fontSizeRatio);
          break;
        case MathFunctionType.Matrix:
          IOfficeMathMatrix officeMathMatrix = function as IOfficeMathMatrix;
          for (int index4 = 0; index4 < officeMathMatrix.Rows.Count; ++index4)
          {
            for (int index5 = 0; index5 < officeMathMatrix.Columns.Count; ++index5)
              this.ReduceFontSizeOfOfficeMath(officeMathMatrix.Rows[index4].Arguments[index5], fontSizeRatio);
          }
          break;
        case MathFunctionType.NArray:
          IOfficeMathNArray officeMathNarray = function as IOfficeMathNArray;
          this.ReduceFontSizeOfOfficeMath(officeMathNarray.Equation, fontSizeRatio);
          WCharacterFormat controlProperties3 = officeMathNarray.ControlProperties as WCharacterFormat;
          controlProperties3.ReducedFontSize = controlProperties3.FontSize * fontSizeRatio;
          break;
        case MathFunctionType.Phantom:
          this.ReduceFontSizeOfOfficeMath((function as IOfficeMathPhantom).Equation, fontSizeRatio);
          break;
        case MathFunctionType.Radical:
          IOfficeMathRadical officeMathRadical = function as IOfficeMathRadical;
          this.ReduceFontSizeOfOfficeMath(officeMathRadical.Equation, fontSizeRatio);
          if (!officeMathRadical.HideDegree)
          {
            this.ReduceFontSizeOfOfficeMath(officeMathRadical.Degree, fontSizeRatio);
            break;
          }
          break;
        case MathFunctionType.LeftSubSuperscript:
          this.ReduceFontSizeOfOfficeMath((function as IOfficeMathLeftScript).Equation, fontSizeRatio);
          break;
        case MathFunctionType.SubSuperscript:
          this.ReduceFontSizeOfOfficeMath((function as IOfficeMathScript).Equation, fontSizeRatio);
          break;
        case MathFunctionType.RightSubSuperscript:
          this.ReduceFontSizeOfOfficeMath((function as IOfficeMathRightScript).Equation, fontSizeRatio);
          break;
        case MathFunctionType.RunElement:
          IOfficeMathRunElement officeMathRunElement = function as IOfficeMathRunElement;
          if (officeMathRunElement.Item is WTextRange)
          {
            WTextRange wtextRange = officeMathRunElement.Item as WTextRange;
            wtextRange.CharacterFormat.ReducedFontSize = wtextRange.CharacterFormat.GetFontSizeToRender() * fontSizeRatio;
            break;
          }
          break;
      }
    }
  }

  private void AlignMathWidgetVertically(LayoutedMathWidget layoutedMathWidget)
  {
    List<LayoutedOMathWidget> childWidgets = layoutedMathWidget.ChildWidgets;
    if (childWidgets.Count <= 1)
      return;
    float num = 0.0f;
    int index1 = 0;
    for (int index2 = 0; index2 < childWidgets.Count; ++index2)
    {
      LayoutedOMathWidget layoutedOmathWidget = childWidgets[index2];
      if ((double) num < (double) layoutedOmathWidget.Bounds.Height)
      {
        num = layoutedOmathWidget.Bounds.Height;
        index1 = index2;
      }
    }
    float verticalCenterPoint1 = this.GetVerticalCenterPoint(childWidgets[index1]);
    float y = layoutedMathWidget.Bounds.Y;
    for (int index3 = 0; index3 < childWidgets.Count; ++index3)
    {
      if (index3 != index1)
      {
        LayoutedOMathWidget officeMathWidget = childWidgets[index3];
        float verticalCenterPoint2 = this.GetVerticalCenterPoint(officeMathWidget);
        float yPosition = verticalCenterPoint1 - verticalCenterPoint2;
        officeMathWidget.ShiftXYPosition(0.0f, yPosition);
        if ((double) y > (double) officeMathWidget.Bounds.Y)
          y = officeMathWidget.Bounds.Y;
      }
    }
    if ((double) y < (double) layoutedMathWidget.Bounds.Y)
    {
      float yPosition = layoutedMathWidget.Bounds.Y - y;
      layoutedMathWidget.ShiftXYPosition(0.0f, yPosition, true);
    }
    float maxBottom = this.GetMaxBottom(layoutedMathWidget);
    layoutedMathWidget.Bounds = new RectangleF(layoutedMathWidget.Bounds.X, layoutedMathWidget.Bounds.Y, layoutedMathWidget.Bounds.Width, maxBottom - layoutedMathWidget.Bounds.Y);
  }

  private void AlignOfficeMathWidgetVertically(LayoutedOMathWidget officeMathWidget)
  {
    if (officeMathWidget.ChildWidgets.Count <= 1)
      return;
    float y = officeMathWidget.Bounds.Y;
    int maxHeightWidgetIndex;
    float verticalCenterPoint1 = officeMathWidget.GetVerticalCenterPoint(out maxHeightWidgetIndex);
    for (int index = 0; index < officeMathWidget.ChildWidgets.Count; ++index)
    {
      if (index != maxHeightWidgetIndex)
      {
        ILayoutedFuntionWidget childWidget = officeMathWidget.ChildWidgets[index];
        float verticalCenterPoint2 = officeMathWidget.GetVerticalCenterPoint(childWidget);
        float yPosition = verticalCenterPoint1 - verticalCenterPoint2;
        childWidget.ShiftXYPosition(0.0f, yPosition);
        if ((double) y > (double) childWidget.Bounds.Y)
          y = childWidget.Bounds.Y;
      }
    }
    if ((double) y < (double) officeMathWidget.Bounds.Y)
    {
      float yPosition = officeMathWidget.Bounds.Y - y;
      officeMathWidget.ShiftXYPosition(0.0f, yPosition, true);
    }
    float maxBottom = this.GetMaxBottom(officeMathWidget);
    officeMathWidget.Bounds = new RectangleF(officeMathWidget.Bounds.X, officeMathWidget.Bounds.Y, officeMathWidget.Bounds.Width, maxBottom - officeMathWidget.Bounds.Y);
  }

  private float GetMaxBottom(LayoutedOMathWidget officeMathWidget)
  {
    float maxBottom = 0.0f;
    for (int index = 0; index < officeMathWidget.ChildWidgets.Count; ++index)
    {
      ILayoutedFuntionWidget childWidget = officeMathWidget.ChildWidgets[index];
      if ((double) maxBottom < (double) childWidget.Bounds.Bottom)
        maxBottom = childWidget.Bounds.Bottom;
    }
    return maxBottom;
  }

  private float GetMaxBottom(LayoutedMathWidget mathWidget)
  {
    float maxBottom = 0.0f;
    for (int index = 0; index < mathWidget.ChildWidgets.Count; ++index)
    {
      LayoutedOMathWidget childWidget = mathWidget.ChildWidgets[index];
      if ((double) maxBottom < (double) childWidget.Bounds.Bottom)
        maxBottom = childWidget.Bounds.Bottom;
    }
    return maxBottom;
  }

  private float GetVerticalCenterPoint(LayoutedOMathWidget officeMathWidget)
  {
    return officeMathWidget.GetVerticalCenterPoint(out int _);
  }

  private void LayoutOfficeMathCollection(
    RectangleF clientArea,
    LayoutedMathWidget layoutedMathWidget,
    IOfficeMaths officeMathCollection)
  {
    RectangleF clientActiveArea = new RectangleF(clientArea.Location, clientArea.Size);
    this.mathXPosition = clientArea.X;
    for (int index = 0; index < officeMathCollection.Count; ++index)
    {
      IOfficeMath officeMath1 = officeMathCollection[index];
      LayoutedOMathWidget layoutedOmathWidget = this.LayoutOfficeMath(clientActiveArea, officeMath1);
      if (index + 1 != officeMathCollection.Count)
      {
        IOfficeMath officeMath2 = officeMathCollection[index + 1];
        if (officeMath2.Functions.Count > 0 && (officeMath1.Functions[officeMath1.Functions.Count - 1].Type != MathFunctionType.RunElement || officeMath2.Functions[0].Type != MathFunctionType.RunElement))
        {
          WCharacterFormat characterProperty = this.GetControlCharacterProperty(officeMath1.Functions[officeMath1.Functions.Count - 1]);
          if (characterProperty != null)
          {
            float width = this.DrawingContext.MeasureString(" ", characterProperty.GetFontToRender(FontScriptType.English), (StringFormat) null).Width;
            layoutedOmathWidget.Bounds = new RectangleF(layoutedOmathWidget.Bounds.X, layoutedOmathWidget.Bounds.Y, layoutedOmathWidget.Bounds.Width + width, layoutedOmathWidget.Bounds.Height);
          }
        }
      }
      clientActiveArea.X += layoutedOmathWidget.Bounds.Width;
      clientActiveArea.Width -= layoutedOmathWidget.Bounds.Width;
      layoutedOmathWidget.Owner = layoutedMathWidget;
      layoutedMathWidget.Bounds = this.UpdateBounds(layoutedMathWidget.Bounds, layoutedOmathWidget.Bounds);
      layoutedMathWidget.ChildWidgets.Add(layoutedOmathWidget);
    }
    this.AlignMathWidgetVertically(layoutedMathWidget);
  }

  private LayoutedOMathWidget LayoutOfficeMath(RectangleF clientActiveArea, IOfficeMath officeMath)
  {
    LayoutedOMathWidget omathLayoutedWidget = this.CreateOMathLayoutedWidget(clientActiveArea.Location, officeMath);
    this.LayoutOfficeMathFunctions(clientActiveArea, omathLayoutedWidget, officeMath.Functions);
    return omathLayoutedWidget;
  }

  private LayoutedOMathWidget CreateOMathLayoutedWidget(PointF location, IOfficeMath officeMath)
  {
    LayoutedOMathWidget omathLayoutedWidget = new LayoutedOMathWidget(officeMath);
    omathLayoutedWidget.Bounds = omathLayoutedWidget.Bounds with
    {
      Location = location
    };
    return omathLayoutedWidget;
  }

  private LayoutedMathWidget CreateMathLayoutedWidget(PointF location)
  {
    LayoutedMathWidget mathLayoutedWidget = new LayoutedMathWidget(this.Widget);
    RectangleF bounds = mathLayoutedWidget.Bounds with
    {
      Location = location
    };
    mathLayoutedWidget.Bounds = bounds;
    return mathLayoutedWidget;
  }

  private PointF Transform(PointF inputPoint, float length, float angle)
  {
    return new PointF(inputPoint.X + length * (float) Math.Cos((double) angle * Math.PI / 180.0), inputPoint.Y + length * (float) Math.Sin((double) angle * Math.PI / 180.0));
  }
}
