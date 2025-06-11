// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocToPdfConverter.Rendering.MathRenderer
// Assembly: Syncfusion.DocToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 84EFC094-D348-494C-A410-44F5807BB0D3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocToPdfConverter.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.Layouting;
using Syncfusion.Office;
using Syncfusion.Pdf.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.DocToPdfConverter.Rendering;

internal class MathRenderer
{
  private PDFDrawingContext m_drawingContext;

  internal MathRenderer(PDFDrawingContext drawingContext) => this.m_drawingContext = drawingContext;

  internal PDFDrawingContext DrawingContext => this.m_drawingContext;

  internal void Draw(WMath math, LayoutedWidget ltWidget)
  {
    LayoutedMathWidget layoutedMathWidget = ltWidget as LayoutedMathWidget;
    for (int index = 0; index < layoutedMathWidget.ChildWidgets.Count; ++index)
      this.Draw(layoutedMathWidget.ChildWidgets[index]);
  }

  internal void Draw(LayoutedOMathWidget layoutedOMathWidget)
  {
    for (int index1 = 0; index1 < layoutedOMathWidget.ChildWidgets.Count; ++index1)
    {
      ILayoutedFuntionWidget childWidget = layoutedOMathWidget.ChildWidgets[index1];
      switch (childWidget.Widget.Type)
      {
        case MathFunctionType.Accent:
          LayoutedAccentWidget layoutedAccentWidget = childWidget as LayoutedAccentWidget;
          IOfficeMathAccent widget1 = layoutedAccentWidget.Widget as IOfficeMathAccent;
          this.Draw(layoutedAccentWidget.Equation);
          RectangleF bounds1 = layoutedAccentWidget.AccentCharacter.Bounds;
          float scalingFactor1 = layoutedAccentWidget.ScalingFactor;
          if ((double) scalingFactor1 != 1.0)
          {
            float x = bounds1.X / scalingFactor1;
            layoutedAccentWidget.AccentCharacter.Bounds = new RectangleF(x, bounds1.Y, bounds1.Width, bounds1.Height);
          }
          this.Draw(layoutedAccentWidget.AccentCharacter, widget1.ControlProperties as WCharacterFormat, scalingFactor1);
          break;
        case MathFunctionType.Bar:
          LayoutedBarWidget layoutedBarWidget = childWidget as LayoutedBarWidget;
          this.Draw(layoutedBarWidget.Equation);
          this.Draw(layoutedBarWidget.BarLine);
          break;
        case MathFunctionType.BorderBox:
          LayoutedBoderBoxWidget layoutedBoderBoxWidget = childWidget as LayoutedBoderBoxWidget;
          this.Draw(layoutedBoderBoxWidget.Equation);
          using (List<LayoutedLineWidget>.Enumerator enumerator = layoutedBoderBoxWidget.BorderLines.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.Draw(enumerator.Current);
            break;
          }
        case MathFunctionType.Box:
          this.Draw((childWidget as LayoutedBoxWidget).Equation);
          break;
        case MathFunctionType.Delimiter:
          LayoutedDelimiterWidget layoutedDelimiterWidget = childWidget as LayoutedDelimiterWidget;
          IOfficeMathDelimiter widget2 = layoutedDelimiterWidget.Widget as IOfficeMathDelimiter;
          if (layoutedDelimiterWidget.BeginCharacter != null)
            this.DrawDelimiterCharacter(layoutedDelimiterWidget.BeginCharacter, layoutedDelimiterWidget.Bounds.Height, widget2.ControlProperties as WCharacterFormat);
          for (int index2 = 0; index2 < layoutedDelimiterWidget.Equation.Count; ++index2)
          {
            LayoutedOMathWidget layoutedOMathWidget1 = layoutedDelimiterWidget.Equation[index2];
            this.Draw(layoutedOMathWidget1);
            if (layoutedDelimiterWidget.Seperator != null && index2 != layoutedDelimiterWidget.Equation.Count - 1)
            {
              layoutedDelimiterWidget.Seperator.Bounds = new RectangleF(layoutedOMathWidget1.Bounds.Right, layoutedDelimiterWidget.Seperator.Bounds.Y, layoutedDelimiterWidget.Seperator.Bounds.Width, layoutedDelimiterWidget.Seperator.Bounds.Height);
              this.DrawDelimiterCharacter(layoutedDelimiterWidget.Seperator, layoutedDelimiterWidget.Bounds.Height, widget2.ControlProperties as WCharacterFormat);
            }
          }
          if (layoutedDelimiterWidget.EndCharacter != null)
          {
            this.DrawDelimiterCharacter(layoutedDelimiterWidget.EndCharacter, layoutedDelimiterWidget.Bounds.Height, widget2.ControlProperties as WCharacterFormat);
            break;
          }
          break;
        case MathFunctionType.EquationArray:
          LayoutedEquationArrayWidget equationArrayWidget = childWidget as LayoutedEquationArrayWidget;
          for (int index3 = 0; index3 < equationArrayWidget.Equation.Count; ++index3)
          {
            List<LayoutedOMathWidget> layoutedOmathWidgetList = equationArrayWidget.Equation[index3];
            for (int index4 = 0; index4 < layoutedOmathWidgetList.Count; ++index4)
              this.Draw(layoutedOmathWidgetList[index4]);
          }
          break;
        case MathFunctionType.Fraction:
          LayoutedFractionWidget layoutedFractionWidget = childWidget as LayoutedFractionWidget;
          this.Draw(layoutedFractionWidget.Numerator);
          this.Draw(layoutedFractionWidget.FractionLine);
          this.Draw(layoutedFractionWidget.Denominator);
          break;
        case MathFunctionType.Function:
          LayoutedMathFunctionWidget mathFunctionWidget = childWidget as LayoutedMathFunctionWidget;
          this.Draw(mathFunctionWidget.FunctionName);
          this.Draw(mathFunctionWidget.Equation);
          break;
        case MathFunctionType.GroupCharacter:
          LayoutedGroupCharacterWidget groupCharacterWidget = childWidget as LayoutedGroupCharacterWidget;
          IOfficeMathGroupCharacter widget3 = groupCharacterWidget.Widget as IOfficeMathGroupCharacter;
          this.Draw(groupCharacterWidget.Equation);
          RectangleF bounds2 = groupCharacterWidget.GroupCharacter.Bounds;
          float scalingFactor2 = groupCharacterWidget.ScalingFactor;
          if ((double) scalingFactor2 != 1.0)
          {
            float x = bounds2.X / scalingFactor2;
            groupCharacterWidget.GroupCharacter.Bounds = new RectangleF(x, bounds2.Y, bounds2.Width, bounds2.Height);
          }
          this.Draw(groupCharacterWidget.GroupCharacter, widget3.ControlProperties as WCharacterFormat, scalingFactor2);
          break;
        case MathFunctionType.Limit:
          LayoutedLimitWidget layoutedLimitWidget = childWidget as LayoutedLimitWidget;
          this.Draw(layoutedLimitWidget.Equation);
          this.Draw(layoutedLimitWidget.Limit);
          break;
        case MathFunctionType.Matrix:
          LayoutedMatrixWidget layoutedMatrixWidget = childWidget as LayoutedMatrixWidget;
          for (int index5 = 0; index5 < layoutedMatrixWidget.Rows.Count; ++index5)
          {
            List<LayoutedOMathWidget> row = layoutedMatrixWidget.Rows[index5];
            for (int index6 = 0; index6 < row.Count; ++index6)
              this.Draw(row[index6]);
          }
          break;
        case MathFunctionType.NArray:
          LayoutedNArrayWidget layoutedNarrayWidget = childWidget as LayoutedNArrayWidget;
          IOfficeMathNArray widget4 = layoutedNarrayWidget.Widget as IOfficeMathNArray;
          this.Draw(layoutedNarrayWidget.Equation);
          this.Draw(layoutedNarrayWidget.NArrayCharacter, widget4.ControlProperties as WCharacterFormat, 1f);
          if (!widget4.HideUpperLimit)
            this.Draw(layoutedNarrayWidget.Superscript);
          if (!widget4.HideLowerLimit)
          {
            this.Draw(layoutedNarrayWidget.Subscript);
            break;
          }
          break;
        case MathFunctionType.Phantom:
          LayoutedPhantomWidget layoutedPhantomWidget = childWidget as LayoutedPhantomWidget;
          if (layoutedPhantomWidget.Show)
          {
            this.Draw(layoutedPhantomWidget.Equation);
            break;
          }
          break;
        case MathFunctionType.Radical:
          LayoutedRadicalWidget layoutedRadicalWidget = childWidget as LayoutedRadicalWidget;
          this.Draw(layoutedRadicalWidget.Equation);
          if (layoutedRadicalWidget.Degree != null)
            this.Draw(layoutedRadicalWidget.Degree);
          foreach (LayoutedLineWidget radicalLine in layoutedRadicalWidget.RadicalLines)
            this.Draw(radicalLine);
          break;
        case MathFunctionType.LeftSubSuperscript:
        case MathFunctionType.SubSuperscript:
        case MathFunctionType.RightSubSuperscript:
          LayoutedScriptWidget layoutedScriptWidget = childWidget as LayoutedScriptWidget;
          if (childWidget.Widget is IOfficeMathScript widget5)
          {
            if (widget5.ScriptType == MathScriptType.Superscript)
              this.Draw(layoutedScriptWidget.Superscript);
            else
              this.Draw(layoutedScriptWidget.Subscript);
          }
          else
          {
            this.Draw(layoutedScriptWidget.Superscript);
            this.Draw(layoutedScriptWidget.Subscript);
          }
          this.Draw(layoutedScriptWidget.Equation);
          break;
        case MathFunctionType.RunElement:
          this.DrawingContext.Draw((childWidget as LayoutedOfficeRunWidget).LayoutedWidget, true);
          break;
      }
    }
  }

  private void DrawDelimiterCharacter(
    LayoutedStringWidget delimiterCharacterWidget,
    float stretchableHeight,
    WCharacterFormat format)
  {
    if (delimiterCharacterWidget.IsStretchable)
    {
      RectangleF bounds = delimiterCharacterWidget.Bounds;
      RectangleF exactStringBounds = this.DrawingContext.GetExactStringBounds(delimiterCharacterWidget.Text, delimiterCharacterWidget.Font);
      float sy = stretchableHeight / exactStringBounds.Height;
      if ((double) sy > 0.0)
      {
        this.DrawingContext.SetScaleTransform(1f, sy);
        float num = exactStringBounds.Y * sy;
        float y = (bounds.Y - num) / sy;
        delimiterCharacterWidget.Bounds = new RectangleF(bounds.X, y, bounds.Width, bounds.Height);
      }
    }
    this.Draw(delimiterCharacterWidget, format, 1f);
    if (!delimiterCharacterWidget.IsStretchable)
      return;
    this.DrawingContext.ResetTransform();
  }

  internal void Draw(LayoutedLineWidget lineWidget)
  {
    if (lineWidget.Skip)
      return;
    this.DrawingContext.PDFGraphics.DrawLine(new PdfPen(new PdfColor(lineWidget.Color), lineWidget.Width), lineWidget.Point1, lineWidget.Point2);
  }

  private void Draw(
    LayoutedStringWidget stringWidget,
    WCharacterFormat characterFormat,
    float scalingFactor)
  {
    if ((double) scalingFactor != 1.0)
    {
      Matrix matrix = new Matrix();
      matrix.Scale(scalingFactor, 1f);
      this.DrawingContext.PDFGraphics.Transform = matrix;
    }
    PdfStringFormat format = this.DrawingContext.PDFGraphics.ConvertFormat(this.DrawingContext.StringFormt);
    PdfBrush brush = (PdfBrush) new PdfSolidBrush((PdfColor) this.DrawingContext.GetTextColor(characterFormat));
    PdfFont pdfFont = this.DrawingContext.CreatePdfFont(stringWidget.Font, this.DrawingContext.EmbedFonts || this.DrawingContext.EmbedCompleteFonts, this.DrawingContext.EmbedCompleteFonts, stringWidget.Text);
    this.DrawingContext.PDFGraphics.DrawString(stringWidget.Text, pdfFont, brush, stringWidget.Bounds, format, true);
    if ((double) scalingFactor == 1.0)
      return;
    this.DrawingContext.ResetTransform();
  }

  internal void Dispose() => this.m_drawingContext = (PDFDrawingContext) null;
}
