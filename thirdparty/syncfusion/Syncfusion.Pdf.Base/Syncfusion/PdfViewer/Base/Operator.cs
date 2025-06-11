// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Operator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal abstract class Operator
{
  internal static Point CalculatePoint(CharacterBuilder interpreter, int dx, int dy)
  {
    interpreter.CurrentPoint = new Point(interpreter.CurrentPoint.X + (double) dx, interpreter.CurrentPoint.Y + (double) dy);
    return new Point(interpreter.CurrentPoint.X, interpreter.CurrentPoint.Y);
  }

  internal static void HLineTo(CharacterBuilder interpreter, int dx)
  {
    Operator.LineTo(interpreter, dx, 0);
  }

  internal static void VLineTo(CharacterBuilder interpreter, int dy)
  {
    Operator.LineTo(interpreter, 0, dy);
  }

  internal static void LineTo(CharacterBuilder interpreter, int dx, int dy)
  {
    interpreter.CurrentPathFigure.Segments.Add((PathSegment) new LineSegment()
    {
      Point = Operator.CalculatePoint(interpreter, dx, dy)
    });
  }

  internal static void CurveTo(
    CharacterBuilder interpreter,
    int dxa,
    int dya,
    int dxb,
    int dyb,
    int dxc,
    int dyc)
  {
    interpreter.CurrentPathFigure.Segments.Add((PathSegment) new BezierSegment()
    {
      Point1 = Operator.CalculatePoint(interpreter, dxa, dya),
      Point2 = Operator.CalculatePoint(interpreter, dxb, dyb),
      Point3 = Operator.CalculatePoint(interpreter, dxc, dyc)
    });
  }

  internal static void MoveTo(CharacterBuilder interpreter, int dx, int dy)
  {
    interpreter.CurrentPathFigure = new PathFigure();
    interpreter.CurrentPathFigure.IsClosed = true;
    interpreter.CurrentPathFigure.IsFilled = true;
    interpreter.CurrentPathFigure.StartPoint = Operator.CalculatePoint(interpreter, dx, dy);
    interpreter.GlyphOutlines.Add(interpreter.CurrentPathFigure);
  }

  internal static void ReadWidth(CharacterBuilder interpreter, int operands)
  {
    if (interpreter.Width.HasValue)
      return;
    if (interpreter.Operands.Count == operands + 1)
      interpreter.Width = new int?(interpreter.Operands.GetFirstAsInt());
    else
      interpreter.Width = new int?(0);
  }

  public abstract void Execute(CharacterBuilder buildChar);
}
