// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontOperator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.PdfViewer.Base;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class SystemFontOperator
{
  internal static Point CalculatePoint(SystemFontBuildChar interpreter, int dx, int dy)
  {
    interpreter.CurrentPoint = new Point(interpreter.CurrentPoint.X + (double) dx, interpreter.CurrentPoint.Y + (double) dy);
    return new Point(interpreter.CurrentPoint.X, interpreter.CurrentPoint.Y);
  }

  internal static void HLineTo(SystemFontBuildChar interpreter, int dx)
  {
    SystemFontOperator.LineTo(interpreter, dx, 0);
  }

  internal static void VLineTo(SystemFontBuildChar interpreter, int dy)
  {
    SystemFontOperator.LineTo(interpreter, 0, dy);
  }

  internal static void LineTo(SystemFontBuildChar interpreter, int dx, int dy)
  {
    interpreter.CurrentPathFigure.Segments.Add((SystemFontPathSegment) new SystemFontLineSegment()
    {
      Point = SystemFontOperator.CalculatePoint(interpreter, dx, dy)
    });
  }

  internal static void CurveTo(
    SystemFontBuildChar interpreter,
    int dxa,
    int dya,
    int dxb,
    int dyb,
    int dxc,
    int dyc)
  {
    interpreter.CurrentPathFigure.Segments.Add((SystemFontPathSegment) new SystemFontBezierSegment()
    {
      Point1 = SystemFontOperator.CalculatePoint(interpreter, dxa, dya),
      Point2 = SystemFontOperator.CalculatePoint(interpreter, dxb, dyb),
      Point3 = SystemFontOperator.CalculatePoint(interpreter, dxc, dyc)
    });
  }

  internal static void MoveTo(SystemFontBuildChar interpreter, int dx, int dy)
  {
    interpreter.CurrentPathFigure = new SystemFontPathFigure();
    interpreter.CurrentPathFigure.IsClosed = true;
    interpreter.CurrentPathFigure.IsFilled = true;
    interpreter.CurrentPathFigure.StartPoint = SystemFontOperator.CalculatePoint(interpreter, dx, dy);
    interpreter.GlyphOutlines.Add(interpreter.CurrentPathFigure);
  }

  internal static void ReadWidth(SystemFontBuildChar interpreter, int operands)
  {
    if (interpreter.Width.HasValue)
      return;
    if (interpreter.Operands.Count == operands + 1)
      interpreter.Width = new int?(interpreter.Operands.GetFirstAsInt());
    else
      interpreter.Width = new int?(0);
  }

  public abstract void Execute(SystemFontBuildChar buildChar);
}
