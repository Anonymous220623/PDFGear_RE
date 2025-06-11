// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Rendering.ShapePath
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Presentation.Rendering;

internal class ShapePath
{
  private RectangleF _rectBounds;
  private Dictionary<string, string> _shapeGuide;
  private FormulaValues _formulaValues;

  internal ShapePath(RectangleF bounds, Dictionary<string, string> shapeGuide)
  {
    this._rectBounds = bounds;
    this._shapeGuide = shapeGuide;
    this._formulaValues = new FormulaValues(this._rectBounds, this._shapeGuide);
  }

  internal GraphicsPath GetCurvedConnector2Path()
  {
    GraphicsPath curvedConnector2Path = new GraphicsPath();
    PointF[] points = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    curvedConnector2Path.AddBeziers(points);
    return curvedConnector2Path;
  }

  internal GraphicsPath GetCurvedConnector3Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedConnector);
    GraphicsPath curvedConnector3Path = new GraphicsPath();
    PointF[] points = new PointF[7]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + this._rectBounds.Height / 4f),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    curvedConnector3Path.AddBeziers(points);
    return curvedConnector3Path;
  }

  internal GraphicsPath GetCurvedConnector4Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedConnector4);
    GraphicsPath curvedConnector4Path = new GraphicsPath();
    PointF[] points = new PointF[10]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    curvedConnector4Path.AddBeziers(points);
    return curvedConnector4Path;
  }

  internal GraphicsPath GetCurvedConnector5Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedConnector5);
    GraphicsPath curvedConnector5Path = new GraphicsPath();
    PointF[] points = new PointF[13]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y6"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y7"]),
      new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    curvedConnector5Path.AddBeziers(points);
    return curvedConnector5Path;
  }

  internal GraphicsPath GetBentConnector2Path()
  {
    PointF[] points = new PointF[3]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    GraphicsPath bentConnector2Path = new GraphicsPath();
    bentConnector2Path.AddLines(points);
    return bentConnector2Path;
  }

  internal GraphicsPath GetBentConnector3Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.ElbowConnector);
    PointF[] points = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    GraphicsPath bentConnector3Path = new GraphicsPath();
    bentConnector3Path.AddLines(points);
    return bentConnector3Path;
  }

  internal GraphicsPath GetBentConnector4Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.BentConnector4);
    PointF[] points = new PointF[5]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    GraphicsPath bentConnector4Path = new GraphicsPath();
    bentConnector4Path.AddLines(points);
    return bentConnector4Path;
  }

  internal GraphicsPath GetBentConnector5Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.BentConnector5);
    PointF[] points = new PointF[6]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    GraphicsPath bentConnector5Path = new GraphicsPath();
    bentConnector5Path.AddLines(points);
    return bentConnector5Path;
  }

  internal GraphicsPath GetRoundedRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RoundedRectangle);
    GraphicsPath roundedRectanglePath = new GraphicsPath();
    float num = shapeFormula["x1"] * 2f;
    roundedRectanglePath.AddArc(this._rectBounds.X, this._rectBounds.Y, num, num, 180f, 90f);
    roundedRectanglePath.AddArc(this._rectBounds.Right - num, this._rectBounds.Y, num, num, 270f, 90f);
    roundedRectanglePath.AddArc(this._rectBounds.Right - num, this._rectBounds.Bottom - num, num, num, 0.0f, 90f);
    roundedRectanglePath.AddArc(this._rectBounds.X, this._rectBounds.Bottom - num, num, num, 90f, 90f);
    roundedRectanglePath.CloseFigure();
    return roundedRectanglePath;
  }

  internal GraphicsPath GetSnipSingleCornerRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.SnipSingleCornerRectangle);
    GraphicsPath cornerRectanglePath = new GraphicsPath();
    PointF[] points = new PointF[5]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["dx1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    cornerRectanglePath.AddLines(points);
    cornerRectanglePath.CloseFigure();
    return cornerRectanglePath;
  }

  internal GraphicsPath GetSnipSameSideCornerRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.SnipSameSideCornerRectangle);
    GraphicsPath cornerRectanglePath = new GraphicsPath();
    PointF[] points = new PointF[8]
    {
      new PointF(this._rectBounds.X + shapeFormula["tx1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["tx2"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["tx1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["by1"]),
      new PointF(this._rectBounds.X + shapeFormula["bx2"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["bx1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["by1"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["tx1"])
    };
    cornerRectanglePath.AddLines(points);
    cornerRectanglePath.CloseFigure();
    return cornerRectanglePath;
  }

  internal GraphicsPath GetSnipDiagonalCornerRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.SnipDiagonalCornerRectangle);
    GraphicsPath cornerRectanglePath = new GraphicsPath();
    PointF[] points = new PointF[8]
    {
      new PointF(this._rectBounds.X + shapeFormula["lx1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["rx2"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["rx1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["ly1"]),
      new PointF(this._rectBounds.X + shapeFormula["lx2"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["rx1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["ry1"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["lx1"])
    };
    cornerRectanglePath.AddLines(points);
    cornerRectanglePath.CloseFigure();
    return cornerRectanglePath;
  }

  internal GraphicsPath GetSnipAndRoundSingleCornerRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.SnipAndRoundSingleCornerRectangle);
    GraphicsPath cornerRectanglePath = new GraphicsPath();
    PointF[] points = new PointF[6];
    float num = shapeFormula["x1"] * 2f;
    points[0] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y);
    points[1] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y);
    points[2] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["dx2"]);
    points[3] = new PointF(this._rectBounds.Right, this._rectBounds.Bottom);
    points[4] = new PointF(this._rectBounds.X, this._rectBounds.Bottom);
    points[5] = new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["x1"]);
    cornerRectanglePath.AddArc(this._rectBounds.X, this._rectBounds.Y, num, num, 180f, 90f);
    cornerRectanglePath.AddLines(points);
    cornerRectanglePath.CloseFigure();
    return cornerRectanglePath;
  }

  internal GraphicsPath GetRoundSingleCornerRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RoundSingleCornerRectangle);
    GraphicsPath cornerRectanglePath = new GraphicsPath();
    PointF[] points = new PointF[2];
    float num = shapeFormula["dx1"] * 2f;
    points[0] = new PointF(this._rectBounds.X, this._rectBounds.Y);
    points[1] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y);
    cornerRectanglePath.AddLines(points);
    cornerRectanglePath.AddArc(this._rectBounds.Right - num, this._rectBounds.Y, num, num, 270f, 90f);
    points[0] = new PointF(this._rectBounds.Right, this._rectBounds.Bottom);
    points[1] = new PointF(this._rectBounds.X, this._rectBounds.Bottom);
    cornerRectanglePath.AddLines(points);
    cornerRectanglePath.CloseFigure();
    return cornerRectanglePath;
  }

  internal GraphicsPath GetRoundSameSideCornerRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RoundSameSideCornerRectangle);
    GraphicsPath cornerRectanglePath = new GraphicsPath();
    PointF[] points = new PointF[2];
    float num1 = shapeFormula["tx1"] * 2f;
    float num2 = shapeFormula["bx1"] * 2f;
    points[0] = new PointF(this._rectBounds.X + shapeFormula["tx1"], this._rectBounds.Y);
    points[1] = new PointF(this._rectBounds.X + shapeFormula["tx2"], this._rectBounds.Y);
    cornerRectanglePath.AddLines(points);
    cornerRectanglePath.AddArc(this._rectBounds.Right - num1, this._rectBounds.Y, num1, num1, 270f, 90f);
    if ((double) num2 == 0.0)
    {
      points[0] = new PointF(this._rectBounds.Right, this._rectBounds.Bottom);
      points[1] = new PointF(this._rectBounds.X, this._rectBounds.Bottom);
      cornerRectanglePath.AddLines(points);
    }
    else
    {
      cornerRectanglePath.AddArc(this._rectBounds.Right - num2, this._rectBounds.Bottom - num2, num2, num2, 0.0f, 90f);
      cornerRectanglePath.AddArc(this._rectBounds.X, this._rectBounds.Bottom - num2, num2, num2, 90f, 90f);
    }
    cornerRectanglePath.AddArc(this._rectBounds.X, this._rectBounds.Y, num1, num1, 180f, 90f);
    cornerRectanglePath.CloseFigure();
    return cornerRectanglePath;
  }

  internal GraphicsPath GetRoundDiagonalCornerRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RoundDiagonalCornerRectangle);
    GraphicsPath cornerRectanglePath = new GraphicsPath();
    PointF[] points = new PointF[2];
    float num1 = shapeFormula["x1"] * 2f;
    float num2 = shapeFormula["a"] * 2f;
    points[0] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y);
    points[1] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y);
    cornerRectanglePath.AddLines(points);
    if ((double) num2 != 0.0)
      cornerRectanglePath.AddArc(this._rectBounds.Right - num2, this._rectBounds.Y, num2, num2, 270f, 90f);
    cornerRectanglePath.AddArc(this._rectBounds.Right - num1, this._rectBounds.Bottom - num1, num1, num1, 0.0f, 90f);
    if ((double) num2 == 0.0)
    {
      points[0] = new PointF(this._rectBounds.Right - shapeFormula["x1"], this._rectBounds.Bottom);
      points[1] = new PointF(this._rectBounds.X, this._rectBounds.Bottom);
      cornerRectanglePath.AddLines(points);
    }
    else
      cornerRectanglePath.AddArc(this._rectBounds.X, this._rectBounds.Bottom - num2, num2, num2, 90f, 90f);
    cornerRectanglePath.AddArc(this._rectBounds.X, this._rectBounds.Y, num1, num1, 180f, 90f);
    cornerRectanglePath.CloseFigure();
    return cornerRectanglePath;
  }

  internal GraphicsPath GetTrianglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.IsoscelesTriangle);
    GraphicsPath trianglePath = new GraphicsPath();
    PointF[] points = new PointF[3]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    trianglePath.AddLines(points);
    trianglePath.CloseFigure();
    return trianglePath;
  }

  internal GraphicsPath GetParallelogramPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Parallelogram);
    GraphicsPath parallelogramPath = new GraphicsPath();
    PointF[] points = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Bottom)
    };
    parallelogramPath.AddLines(points);
    parallelogramPath.CloseFigure();
    return parallelogramPath;
  }

  internal GraphicsPath GetTrapezoidPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Trapezoid);
    GraphicsPath trapezoidPath = new GraphicsPath();
    PointF[] points = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    trapezoidPath.AddLines(points);
    trapezoidPath.CloseFigure();
    return trapezoidPath;
  }

  internal GraphicsPath GetRegularPentagonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RegularPentagon);
    GraphicsPath regularPentagonPath = new GraphicsPath();
    PointF[] points = new PointF[5]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"])
    };
    regularPentagonPath.AddLines(points);
    regularPentagonPath.CloseFigure();
    return regularPentagonPath;
  }

  internal GraphicsPath GetHexagonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Hexagon);
    GraphicsPath hexagonPath = new GraphicsPath();
    PointF[] points = new PointF[6]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"])
    };
    hexagonPath.AddLines(points);
    hexagonPath.CloseFigure();
    return hexagonPath;
  }

  internal GraphicsPath GetHeptagonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Heptagon);
    GraphicsPath heptagonPath = new GraphicsPath();
    PointF[] points = new PointF[7]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"])
    };
    heptagonPath.AddLines(points);
    heptagonPath.CloseFigure();
    return heptagonPath;
  }

  internal GraphicsPath GetOctagonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Octagon);
    GraphicsPath octagonPath = new GraphicsPath();
    PointF[] points = new PointF[8]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"])
    };
    octagonPath.AddLines(points);
    octagonPath.CloseFigure();
    return octagonPath;
  }

  internal GraphicsPath GetDecagonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Decagon);
    GraphicsPath decagonPath = new GraphicsPath();
    PointF[] points = new PointF[10]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"])
    };
    decagonPath.AddLines(points);
    decagonPath.CloseFigure();
    return decagonPath;
  }

  internal GraphicsPath GetDodecagonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Dodecagon);
    GraphicsPath dodecagonPath = new GraphicsPath();
    PointF[] points = new PointF[12]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y3"])
    };
    dodecagonPath.AddLines(points);
    dodecagonPath.CloseFigure();
    return dodecagonPath;
  }

  internal GraphicsPath GetPiePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Pie);
    GraphicsPath piePath = new GraphicsPath();
    piePath.AddPie(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, shapeFormula["stAng"] / 60000f, shapeFormula["swAng"] / 60000f);
    return piePath;
  }

  internal GraphicsPath GetChordPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Chord);
    GraphicsPath chordPath = new GraphicsPath();
    chordPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, shapeFormula["stAng"] / 60000f, shapeFormula["swAng"] / 60000f);
    chordPath.CloseFigure();
    return chordPath;
  }

  internal GraphicsPath GetTearDropPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Teardrop);
    GraphicsPath tearDropPath = new GraphicsPath();
    tearDropPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, 180f, 90f);
    PointF[] points = new PointF[7]
    {
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f)
    };
    tearDropPath.AddBeziers(points);
    tearDropPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, 0.0f, 90f);
    tearDropPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, 90f, 90f);
    tearDropPath.CloseFigure();
    return tearDropPath;
  }

  internal GraphicsPath GetSwooshArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.SwooshArrow);
    GraphicsPath swooshArrowPath = new GraphicsPath();
    PointF[] points1 = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["xP1"], this._rectBounds.Y + shapeFormula["yP1"]),
      new PointF(this._rectBounds.X + shapeFormula["xB"], this._rectBounds.Y + shapeFormula["yB"]),
      new PointF(this._rectBounds.X + shapeFormula["xB"], this._rectBounds.Y + shapeFormula["yB"])
    };
    swooshArrowPath.AddBeziers(points1);
    PointF[] points2 = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["xC"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["yD"]),
      new PointF(this._rectBounds.X + shapeFormula["xE"], this._rectBounds.Y + shapeFormula["yE"]),
      new PointF(this._rectBounds.X + shapeFormula["xF"], this._rectBounds.Y + shapeFormula["yF"])
    };
    swooshArrowPath.AddLines(points2);
    PointF[] points3 = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["xF"], this._rectBounds.Y + shapeFormula["yF"]),
      new PointF(this._rectBounds.X + shapeFormula["xP2"], this._rectBounds.Y + shapeFormula["yP2"]),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    swooshArrowPath.AddBeziers(points3);
    return swooshArrowPath;
  }

  internal GraphicsPath GetFramePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Frame);
    GraphicsPath framePath = new GraphicsPath();
    PointF[] points = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    framePath.AddLines(points);
    framePath.CloseFigure();
    points[0] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["x1"]);
    points[1] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"]);
    points[2] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y4"]);
    points[3] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["x1"]);
    framePath.AddLines(points);
    framePath.CloseFigure();
    return framePath;
  }

  internal GraphicsPath GetHalfFramePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.HalfFrame);
    GraphicsPath halfFramePath = new GraphicsPath();
    PointF[] points = new PointF[6]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    halfFramePath.AddLines(points);
    halfFramePath.CloseFigure();
    return halfFramePath;
  }

  internal GraphicsPath GetL_ShapePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Corner);
    GraphicsPath lShapePath = new GraphicsPath();
    PointF[] points = new PointF[6]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    lShapePath.AddLines(points);
    lShapePath.CloseFigure();
    return lShapePath;
  }

  internal GraphicsPath GetDiagonalStripePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.DiagonalStripe);
    GraphicsPath diagonalStripePath = new GraphicsPath();
    PointF[] points = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Left, this._rectBounds.Bottom)
    };
    diagonalStripePath.AddLines(points);
    diagonalStripePath.CloseFigure();
    return diagonalStripePath;
  }

  internal GraphicsPath GetCrossPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Cross);
    GraphicsPath crossPath = new GraphicsPath();
    PointF[] points = new PointF[12]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"])
    };
    crossPath.AddLines(points);
    crossPath.CloseFigure();
    return crossPath;
  }

  internal GraphicsPath GetPlaquePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Plaque);
    GraphicsPath plaquePath = new GraphicsPath();
    float num = shapeFormula["x1"] * 2f;
    plaquePath.AddArc(this._rectBounds.X - shapeFormula["x1"], this._rectBounds.Y - shapeFormula["x1"], num, num, 90f, -90f);
    plaquePath.AddArc(this._rectBounds.Right - shapeFormula["x1"], this._rectBounds.Y - shapeFormula["x1"], num, num, 180f, -90f);
    plaquePath.AddArc(this._rectBounds.Right - shapeFormula["x1"], this._rectBounds.Bottom - shapeFormula["x1"], num, num, 270f, -90f);
    plaquePath.AddArc(this._rectBounds.X - shapeFormula["x1"], this._rectBounds.Bottom - shapeFormula["x1"], num, num, 0.0f, -90f);
    plaquePath.CloseFigure();
    return plaquePath;
  }

  internal GraphicsPath GetCanPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Can);
    GraphicsPath canPath = new GraphicsPath();
    canPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, shapeFormula["y1"] * 2f, 0.0f, 180f);
    canPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, shapeFormula["y1"] * 2f, 180f, 180f);
    canPath.AddArc(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y3"] - shapeFormula["y1"], this._rectBounds.Width, shapeFormula["y1"] * 2f, 0.0f, 180f);
    canPath.AddLine(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y3"] - shapeFormula["y1"], this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"]);
    return canPath;
  }

  internal GraphicsPath GetLeftRightRibbonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftRightRibbon);
    GraphicsPath leftRightRibbonPath = new GraphicsPath();
    PointF[] points1 = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["ly2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["ly1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["ly1"])
    };
    leftRightRibbonPath.AddLines(points1);
    leftRightRibbonPath.AddArc(points1[3].X, points1[3].Y, this._rectBounds.Width / 32f, shapeFormula["hR"] * 2f, 270f, 180f);
    leftRightRibbonPath.AddArc(points1[3].X, points1[3].Y + shapeFormula["hR"] * 2f, this._rectBounds.Width / 32f, shapeFormula["hR"] * 2f, 270f, -180f);
    PointF[] points2 = new PointF[6]
    {
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ry2"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ry1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["ry3"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ry4"]),
      new PointF((float) ((double) this._rectBounds.X + (double) this._rectBounds.Width / 2.0 + (double) this._rectBounds.Width / 64.0), this._rectBounds.Y + shapeFormula["ry4"])
    };
    leftRightRibbonPath.AddLines(points2);
    leftRightRibbonPath.AddArc(this._rectBounds.X + this._rectBounds.Width / 2f, points2[5].Y - shapeFormula["hR"] * 2f, this._rectBounds.Width / 32f, shapeFormula["hR"] * 2f, 90f, 90f);
    PointF[] points3 = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["ly3"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["ly3"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["ly4"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["ly2"])
    };
    leftRightRibbonPath.AddLines(points3);
    leftRightRibbonPath.CloseFigure();
    PointF[] pointFArray = new PointF[4]
    {
      new PointF(points1[3].X, points1[3].Y + shapeFormula["hR"] * 3f),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["ly3"]),
      new PointF(points1[3].X + this._rectBounds.Width / 32f, points1[3].Y + shapeFormula["hR"]),
      new PointF(points1[3].X + this._rectBounds.Width / 32f, points1[3].Y + shapeFormula["hR"] * 4f)
    };
    leftRightRibbonPath.AddLine(pointFArray[0], pointFArray[1]);
    leftRightRibbonPath.CloseFigure();
    leftRightRibbonPath.AddLine(pointFArray[2], pointFArray[3]);
    leftRightRibbonPath.CloseFigure();
    return leftRightRibbonPath;
  }

  internal GraphicsPath GetFunnelPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Funnel);
    GraphicsPath funnelPath = new GraphicsPath();
    funnelPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height / 2f, shapeFormula["stAng1"] - 2f, shapeFormula["swAng1"] + 4f);
    funnelPath.AddLine(funnelPath.PathPoints[funnelPath.PointCount - 1].X, funnelPath.PathPoints[funnelPath.PointCount - 1].Y, this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]);
    funnelPath.AddArc((float) ((double) this._rectBounds.X + (double) shapeFormula["x3"] - (double) shapeFormula["rw3"] * 2.0), this._rectBounds.Y + shapeFormula["y2"] - shapeFormula["rh3"], shapeFormula["rw3"] * 2f, shapeFormula["rh3"] * 2f, shapeFormula["da"], shapeFormula["swAng3"]);
    funnelPath.CloseFigure();
    funnelPath.AddArc(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + this._rectBounds.Height / 20f, shapeFormula["rw2"] * 2f, shapeFormula["rh2"] * 2f, 180f, 360f);
    return funnelPath;
  }

  internal GraphicsPath GetGear6Path()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.Gear6);
    return new GraphicsPath();
  }

  internal GraphicsPath GetGear9Path()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.Gear9);
    return new GraphicsPath();
  }

  internal GraphicsPath GetCubePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Cube);
    GraphicsPath cubePath = new GraphicsPath();
    PointF[] points = new PointF[6]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["y1"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    cubePath.AddLines(points);
    cubePath.CloseFigure();
    PointF pt2 = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]);
    cubePath.AddLine(points[0], pt2);
    cubePath.AddLine(points[2], pt2);
    cubePath.AddLine(points[4], pt2);
    return cubePath;
  }

  internal GraphicsPath GetBevelPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Bevel);
    GraphicsPath bevelPath = new GraphicsPath();
    PointF[] points1 = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.Left, this._rectBounds.Bottom)
    };
    bevelPath.AddLines(points1);
    bevelPath.CloseFigure();
    PointF[] points2 = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"])
    };
    bevelPath.AddLines(points2);
    bevelPath.CloseFigure();
    bevelPath.AddLine(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["x1"]);
    bevelPath.CloseFigure();
    bevelPath.AddLine(this._rectBounds.X, this._rectBounds.Bottom, this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]);
    bevelPath.CloseFigure();
    bevelPath.AddLine(this._rectBounds.Right, this._rectBounds.Y, this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["x1"]);
    bevelPath.CloseFigure();
    bevelPath.AddLine(this._rectBounds.Right, this._rectBounds.Bottom, this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]);
    bevelPath.CloseFigure();
    return bevelPath;
  }

  internal GraphicsPath GetDonutPath(double lineWidth)
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Donut);
    GraphicsPath donutPath = new GraphicsPath();
    RectangleF rect = new RectangleF(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height);
    donutPath.AddArc(rect, 180f, 90f);
    donutPath.AddArc(rect, 270f, 90f);
    donutPath.AddArc(rect, 0.0f, 90f);
    donutPath.AddArc(rect, 90f, 90f);
    donutPath.CloseFigure();
    rect = new RectangleF(this._rectBounds.X + shapeFormula["dr"], this._rectBounds.Y + shapeFormula["dr"], shapeFormula["iwd2"] * 2f, shapeFormula["ihd2"] * 2f);
    if ((double) rect.Height <= 0.0)
      rect.Height = (float) lineWidth;
    donutPath.AddArc(rect, 180f, -90f);
    donutPath.AddArc(rect, 90f, -90f);
    donutPath.AddArc(rect, 0.0f, -90f);
    donutPath.AddArc(rect, 270f, -90f);
    donutPath.CloseFigure();
    return donutPath;
  }

  internal GraphicsPath GetNoSymbolPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.NoSymbol);
    GraphicsPath noSymbolPath = new GraphicsPath();
    RectangleF rect = new RectangleF(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height);
    noSymbolPath.AddArc(rect, 180f, 90f);
    noSymbolPath.AddArc(rect, 270f, 90f);
    noSymbolPath.AddArc(rect, 0.0f, 90f);
    noSymbolPath.AddArc(rect, 90f, 90f);
    if ((double) shapeFormula["iwd2"] == 0.0)
      shapeFormula["iwd2"] = 1f;
    if ((double) shapeFormula["ihd2"] == 0.0)
      shapeFormula["ihd2"] = 1f;
    noSymbolPath.CloseFigure();
    noSymbolPath.AddArc(this._rectBounds.X + shapeFormula["dr"], this._rectBounds.Y + shapeFormula["dr"], shapeFormula["iwd2"] * 2f, shapeFormula["ihd2"] * 2f, shapeFormula["stAng1"], shapeFormula["swAng"]);
    noSymbolPath.CloseFigure();
    noSymbolPath.AddArc(this._rectBounds.X + shapeFormula["dr"], this._rectBounds.Y + shapeFormula["dr"], shapeFormula["iwd2"] * 2f, shapeFormula["ihd2"] * 2f, shapeFormula["stAng2"], shapeFormula["swAng"]);
    noSymbolPath.CloseFigure();
    return noSymbolPath;
  }

  internal GraphicsPath GetBlockArcPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.BlockArc);
    GraphicsPath blockArcPath = new GraphicsPath();
    RectangleF rect = new RectangleF(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height);
    if ((double) shapeFormula["iwd2"] == 0.0)
      shapeFormula["iwd2"] = 1f;
    if ((double) shapeFormula["ihd2"] == 0.0)
      shapeFormula["ihd2"] = 1f;
    if ((double) (shapeFormula["ihd2"] * 2f) > 0.0)
    {
      float num1 = this._rectBounds.Bottom - (shapeFormula["y1"] + shapeFormula["ihd2"]);
      float num2 = this._rectBounds.Bottom - shapeFormula["y6"];
      if ((double) num1 > (double) this._rectBounds.Y || (double) num2 > 150.0)
      {
        float x = this._rectBounds.X + shapeFormula["dr"];
        float y = this._rectBounds.Y + shapeFormula["dr"];
        float width = shapeFormula["iwd2"] * 2f;
        float height = shapeFormula["ihd2"] * 2f;
        float startAngle = shapeFormula["istAng"] / 60000f;
        float sweepAngle = shapeFormula["iswAng"] / 60000f;
        blockArcPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, shapeFormula["stAng"] / 60000f, shapeFormula["swAng"] / 60000f);
        blockArcPath.AddArc(x, y, width, height, startAngle, sweepAngle);
      }
      else
      {
        float x = this._rectBounds.X + shapeFormula["dr"];
        float y = this._rectBounds.Bottom - shapeFormula["y6"];
        float width = shapeFormula["iwd2"] * 2f;
        float height = shapeFormula["ihd2"] * 2f;
        float startAngle = shapeFormula["istAng"] / 60000f;
        float sweepAngle = shapeFormula["iswAng"] / 60000f;
        blockArcPath.AddArc(rect, shapeFormula["stAng"] / 60000f, shapeFormula["swAng"] / 60000f);
        blockArcPath.AddArc(x, y, width, height, startAngle, sweepAngle);
      }
    }
    else
    {
      float x = this._rectBounds.X + shapeFormula["dr"];
      float y = this._rectBounds.Bottom - shapeFormula["dr"];
      float width = shapeFormula["iwd2"] * 3f;
      float height = shapeFormula["ht1"];
      float startAngle = shapeFormula["istAng"] / 60000f;
      float sweepAngle = shapeFormula["iswAng"] / 60000f;
      blockArcPath.AddArc(this._rectBounds.Right - shapeFormula["x5"], this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, shapeFormula["stAng"] / 60000f, shapeFormula["swAng"] / 60000f);
      blockArcPath.AddArc(x, y, width, height, startAngle, sweepAngle);
    }
    blockArcPath.CloseFigure();
    return blockArcPath;
  }

  internal GraphicsPath GetFoldedCornerPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.FoldedCorner);
    GraphicsPath foldedCornerPath = new GraphicsPath();
    PointF[] points = new PointF[8]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Top + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Top + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.Left, this._rectBounds.Bottom),
      new PointF(this._rectBounds.Left, this._rectBounds.Top),
      new PointF(this._rectBounds.Right, this._rectBounds.Top),
      new PointF(this._rectBounds.Right, this._rectBounds.Top + shapeFormula["y2"])
    };
    foldedCornerPath.AddLines(points);
    return foldedCornerPath;
  }

  internal GraphicsPath[] GetSmileyFacePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.SmileyFace);
    GraphicsPath[] smileyFacePath = new GraphicsPath[2];
    for (int index = 0; index < smileyFacePath.Length; ++index)
      smileyFacePath[index] = new GraphicsPath();
    PointF[] points = new PointF[4];
    points[0] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]);
    points[1] = points[0];
    points[2] = new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["y5"]);
    points[3] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y2"]);
    smileyFacePath[1].AddBeziers(points);
    smileyFacePath[0].AddEllipse(this._rectBounds);
    smileyFacePath[0].AddEllipse(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"] - shapeFormula["hR"], shapeFormula["wR"] * 2f, shapeFormula["hR"] * 2f);
    smileyFacePath[0].AddEllipse(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"] - shapeFormula["hR"], shapeFormula["wR"] * 2f, shapeFormula["hR"] * 2f);
    return smileyFacePath;
  }

  internal GraphicsPath GetHeartPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Heart);
    GraphicsPath heartPath = new GraphicsPath();
    PointF[] points = new PointF[7]
    {
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + this._rectBounds.Height / 4f),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + this._rectBounds.Height / 4f),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + this._rectBounds.Height / 4f),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + this._rectBounds.Height / 4f)
    };
    heartPath.AddBeziers(points);
    heartPath.CloseFigure();
    return heartPath;
  }

  internal GraphicsPath GetLightningBoltPath()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.LightningBolt);
    GraphicsPath lightningBoltPath = new GraphicsPath();
    PointF[] points = new PointF[11]
    {
      this.GetXYPosition(8472f, 0.0f, 21600f),
      this.GetXYPosition(12860f, 6080f, 21600f),
      this.GetXYPosition(11050f, 6797f, 21600f),
      this.GetXYPosition(16577f, 12007f, 21600f),
      this.GetXYPosition(14767f, 12877f, 21600f),
      this.GetXYPosition(21600f, 21600f, 21600f),
      this.GetXYPosition(10012f, 14915f, 21600f),
      this.GetXYPosition(12222f, 13987f, 21600f),
      this.GetXYPosition(5022f, 9705f, 21600f),
      this.GetXYPosition(7602f, 8382f, 21600f),
      this.GetXYPosition(0.0f, 3890f, 21600f)
    };
    lightningBoltPath.AddLines(points);
    lightningBoltPath.CloseAllFigures();
    return lightningBoltPath;
  }

  internal GraphicsPath GetSunPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Sun);
    GraphicsPath sunPath = new GraphicsPath();
    PointF[] points = new PointF[3]
    {
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x15"], this._rectBounds.Y + shapeFormula["y18"]),
      new PointF(this._rectBounds.X + shapeFormula["x15"], this._rectBounds.Y + shapeFormula["y14"])
    };
    sunPath.AddLines(points);
    sunPath.CloseFigure();
    points[0] = new PointF(this._rectBounds.X + shapeFormula["ox1"], this._rectBounds.Y + shapeFormula["oy1"]);
    points[1] = new PointF(this._rectBounds.X + shapeFormula["x16"], this._rectBounds.Y + shapeFormula["y13"]);
    points[2] = new PointF(this._rectBounds.X + shapeFormula["x17"], this._rectBounds.Y + shapeFormula["y12"]);
    sunPath.AddLines(points);
    sunPath.CloseFigure();
    points[0] = new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y);
    points[1] = new PointF(this._rectBounds.X + shapeFormula["x18"], this._rectBounds.Y + shapeFormula["y10"]);
    points[2] = new PointF(this._rectBounds.X + shapeFormula["x14"], this._rectBounds.Y + shapeFormula["y10"]);
    sunPath.AddLines(points);
    sunPath.CloseFigure();
    points[0] = new PointF(this._rectBounds.X + shapeFormula["ox2"], this._rectBounds.Y + shapeFormula["oy1"]);
    points[1] = new PointF(this._rectBounds.X + shapeFormula["x13"], this._rectBounds.Y + shapeFormula["y12"]);
    points[2] = new PointF(this._rectBounds.X + shapeFormula["x12"], this._rectBounds.Y + shapeFormula["y13"]);
    sunPath.AddLines(points);
    sunPath.CloseFigure();
    points[0] = new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f);
    points[1] = new PointF(this._rectBounds.X + shapeFormula["x10"], this._rectBounds.Y + shapeFormula["y14"]);
    points[2] = new PointF(this._rectBounds.X + shapeFormula["x10"], this._rectBounds.Y + shapeFormula["y18"]);
    sunPath.AddLines(points);
    sunPath.CloseFigure();
    points[0] = new PointF(this._rectBounds.X + shapeFormula["ox2"], this._rectBounds.Y + shapeFormula["oy2"]);
    points[1] = new PointF(this._rectBounds.X + shapeFormula["x12"], this._rectBounds.Y + shapeFormula["y17"]);
    points[2] = new PointF(this._rectBounds.X + shapeFormula["x13"], this._rectBounds.Y + shapeFormula["y16"]);
    sunPath.AddLines(points);
    sunPath.CloseFigure();
    points[0] = new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom);
    points[1] = new PointF(this._rectBounds.X + shapeFormula["x14"], this._rectBounds.Y + shapeFormula["y15"]);
    points[2] = new PointF(this._rectBounds.X + shapeFormula["x18"], this._rectBounds.Y + shapeFormula["y15"]);
    sunPath.AddLines(points);
    sunPath.CloseFigure();
    points[0] = new PointF(this._rectBounds.X + shapeFormula["ox1"], this._rectBounds.Y + shapeFormula["oy2"]);
    points[1] = new PointF(this._rectBounds.X + shapeFormula["x17"], this._rectBounds.Y + shapeFormula["y16"]);
    points[2] = new PointF(this._rectBounds.X + shapeFormula["x16"], this._rectBounds.Y + shapeFormula["y17"]);
    sunPath.AddLines(points);
    sunPath.CloseFigure();
    sunPath.AddEllipse(this._rectBounds.X + shapeFormula["x19"], this._rectBounds.Y + this._rectBounds.Height / 2f - shapeFormula["hR"], shapeFormula["wR"] * 2f, shapeFormula["hR"] * 2f);
    sunPath.CloseFigure();
    return sunPath;
  }

  internal GraphicsPath GetMoonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Moon);
    GraphicsPath moonPath = new GraphicsPath();
    moonPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width * 2f, this._rectBounds.Height, 90f, 180f);
    float startAngle = shapeFormula["stAng1"];
    if ((double) startAngle < 180.0)
      startAngle += 180f;
    moonPath.AddArc(this._rectBounds.X + shapeFormula["g0w"], this._rectBounds.Y + this._rectBounds.Height / 2f - shapeFormula["dy1"], shapeFormula["g18w"] * 2f, shapeFormula["dy1"] * 2f, startAngle, shapeFormula["swAng1"] % 360f);
    moonPath.CloseFigure();
    return moonPath;
  }

  internal GraphicsPath GetCloudPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Cloud);
    GraphicsPath cloudPath = new GraphicsPath();
    PointF xyPosition = this.GetXYPosition(3900f, 14370f, 43200f);
    xyPosition.X += shapeFormula["g27"];
    xyPosition.Y -= shapeFormula["g30"];
    SizeF sizeF1 = new SizeF((float) ((double) this._rectBounds.Width * 6753.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 9190.0 / 43200.0 * 2.0));
    SizeF sizeF2 = new SizeF((float) ((double) this._rectBounds.Width * 5333.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 7267.0 / 43200.0 * 2.0));
    SizeF sizeF3 = new SizeF((float) ((double) this._rectBounds.Width * 4365.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 5945.0 / 43200.0 * 2.0));
    SizeF sizeF4 = new SizeF((float) ((double) this._rectBounds.Width * 4857.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 6595.0 / 43200.0 * 2.0));
    SizeF sizeF5 = new SizeF((float) ((double) this._rectBounds.Width * 5333.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 7273.0 / 43200.0 * 2.0));
    SizeF sizeF6 = new SizeF((float) ((double) this._rectBounds.Width * 6775.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 9220.0 / 43200.0 * 2.0));
    SizeF sizeF7 = new SizeF((float) ((double) this._rectBounds.Width * 5785.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 7867.0 / 43200.0 * 2.0));
    SizeF sizeF8 = new SizeF((float) ((double) this._rectBounds.Width * 6752.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 9215.0 / 43200.0 * 2.0));
    SizeF sizeF9 = new SizeF((float) ((double) this._rectBounds.Width * 7720.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 10543.0 / 43200.0 * 2.0));
    SizeF sizeF10 = new SizeF((float) ((double) this._rectBounds.Width * 4360.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 5918.0 / 43200.0 * 2.0));
    SizeF sizeF11 = new SizeF((float) ((double) this._rectBounds.Width * 4345.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 5945.0 / 43200.0 * 2.0));
    cloudPath.AddArc(this._rectBounds.X + (float) (4076.0 * (double) this._rectBounds.Width / 43200.0), this._rectBounds.Y + (float) (3912.0 * (double) this._rectBounds.Height / 43200.0), sizeF1.Width, sizeF1.Height, -190f, 123f);
    cloudPath.AddArc(this._rectBounds.X + (float) (13469.0 * (double) this._rectBounds.Width / 43200.0), this._rectBounds.Y + (float) (1304.0 * (double) this._rectBounds.Height / 43200.0), sizeF2.Width, sizeF2.Height, -144f, 89f);
    cloudPath.AddArc((float) ((double) this._rectBounds.X + (double) this._rectBounds.Width / 2.0 + 531.0 * (double) this._rectBounds.Width / 43200.0), this._rectBounds.Y + 1f, sizeF3.Width, sizeF3.Height, -145f, 99f);
    cloudPath.AddArc((float) ((double) xyPosition.X + (double) this._rectBounds.Width / 2.0 + 3013.0 * (double) this._rectBounds.Width / 43200.0), this._rectBounds.Y + 1f, sizeF4.Width, sizeF4.Height, -130f, 117f);
    cloudPath.AddArc((float) ((double) this._rectBounds.Right - (double) sizeF5.Width - 708.0 * (double) this._rectBounds.Width / 43200.0), (float) ((double) this._rectBounds.Y + (double) sizeF4.Height / 2.0 - 1127.0 * (double) this._rectBounds.Height / 43200.0), sizeF5.Width, sizeF5.Height, -78f, 109f);
    cloudPath.AddArc((float) ((double) this._rectBounds.Right - (double) sizeF6.Width + 354.0 * (double) this._rectBounds.Width / 43200.0), (float) ((double) this._rectBounds.Y + (double) this._rectBounds.Height / 2.0 - 9129.0 * (double) this._rectBounds.Height / 43200.0), sizeF6.Width, sizeF6.Height, -46f, 130f);
    cloudPath.AddArc((float) ((double) this._rectBounds.X + (double) this._rectBounds.Width / 2.0 + 4608.0 * (double) this._rectBounds.Width / 43200.0), (float) ((double) this._rectBounds.Y + (double) this._rectBounds.Height / 2.0 + 869.0 * (double) this._rectBounds.Height / 43200.0), sizeF7.Width, sizeF7.Height, 0.0f, 114f);
    cloudPath.AddArc((float) ((double) this._rectBounds.X + (double) this._rectBounds.Width / 2.0 - (double) sizeF8.Width / 2.0 + 886.0 * (double) this._rectBounds.Width / 43200.0), this._rectBounds.Bottom - sizeF8.Height, sizeF8.Width, sizeF8.Height, 22f, 115f);
    cloudPath.AddArc(this._rectBounds.X + (float) (4962.0 * (double) this._rectBounds.Width / 43200.0), (float) ((double) this._rectBounds.Bottom - (double) sizeF9.Height - 2173.0 * (double) this._rectBounds.Height / 43200.0), sizeF9.Width, sizeF9.Height, 66f, 75f);
    cloudPath.AddArc(this._rectBounds.X + (float) (1063.0 * (double) this._rectBounds.Width / 43200.0), (float) ((double) this._rectBounds.Y + (double) this._rectBounds.Height / 2.0 + 2608.0 * (double) this._rectBounds.Height / 43200.0), sizeF10.Width, sizeF10.Height, -274f, 146f);
    cloudPath.AddArc(this._rectBounds.X + 1f, (float) ((double) this._rectBounds.Y + (double) this._rectBounds.Height / 2.0 - (double) sizeF11.Height / 2.0 - 1304.0 * (double) this._rectBounds.Height / 43200.0), sizeF11.Width, sizeF11.Height, -246f, 152f);
    cloudPath.CloseFigure();
    cloudPath.AddArc((float) ((double) this._rectBounds.X + (double) this._rectBounds.Width / 2.0 + 2658.0 * (double) this._rectBounds.Width / 43200.0), this._rectBounds.Y + this._rectBounds.Height / 2f, (float) ((double) this._rectBounds.Width * 6753.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 9190.0 / 43200.0 * 2.0), -58f, 59f);
    return cloudPath;
  }

  internal GraphicsPath[] GetArcPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Arc);
    GraphicsPath[] arcPath = new GraphicsPath[2];
    for (int index = 0; index < arcPath.Length; ++index)
      arcPath[index] = new GraphicsPath();
    arcPath[0].AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, shapeFormula["stAng"] / 60000f, shapeFormula["swAng"] / 60000f);
    arcPath[1].AddPie(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, shapeFormula["stAng"] / 60000f, shapeFormula["swAng"] / 60000f);
    return arcPath;
  }

  internal GraphicsPath GetDoubleBracketPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.DoubleBracket);
    GraphicsPath doubleBracketPath = new GraphicsPath();
    doubleBracketPath.AddArc(this._rectBounds.X, this._rectBounds.Bottom - shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, 90f, 90f);
    doubleBracketPath.AddArc(this._rectBounds.X, this._rectBounds.Y, shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, 180f, 90f);
    doubleBracketPath.StartFigure();
    doubleBracketPath.AddArc(this._rectBounds.X + shapeFormula["x2"] - shapeFormula["x1"], this._rectBounds.Y, shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, 270f, 90f);
    doubleBracketPath.AddArc(this._rectBounds.X + shapeFormula["x2"] - shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"] - shapeFormula["x1"], shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, 0.0f, 90f);
    return doubleBracketPath;
  }

  internal GraphicsPath GetDoubleBracePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.DoubleBrace);
    GraphicsPath doubleBracePath = new GraphicsPath();
    doubleBracePath.AddArc(this._rectBounds.X - shapeFormula["x1"] + shapeFormula["x2"], this._rectBounds.Bottom - shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, 90f, 90f);
    doubleBracePath.AddArc(this._rectBounds.X - shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"] - shapeFormula["x1"], shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, 0.0f, -90f);
    doubleBracePath.AddArc(this._rectBounds.X - shapeFormula["x1"], (float) ((double) this._rectBounds.Y + (double) shapeFormula["y3"] - (double) shapeFormula["x1"] * 3.0), shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, 90f, -90f);
    doubleBracePath.AddArc(this._rectBounds.X - shapeFormula["x1"] + shapeFormula["x2"], this._rectBounds.Y, shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, 180f, 90f);
    doubleBracePath.StartFigure();
    doubleBracePath.AddArc(this._rectBounds.X + shapeFormula["x3"] - shapeFormula["x1"], this._rectBounds.Top, shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, 270f, 90f);
    doubleBracePath.AddArc(this._rectBounds.Right - shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"] - shapeFormula["x1"], shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, 180f, -90f);
    doubleBracePath.AddArc(this._rectBounds.Right - shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"] + shapeFormula["x1"], shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, 270f, -90f);
    doubleBracePath.AddArc(this._rectBounds.X + shapeFormula["x3"] - shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"], shapeFormula["x1"] * 2f, shapeFormula["x1"], 0.0f, 90f);
    return doubleBracePath;
  }

  internal GraphicsPath GetLeftBracketPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftBracket);
    GraphicsPath leftBracketPath = new GraphicsPath();
    leftBracketPath.AddArc(this._rectBounds.Right - this._rectBounds.Width, this._rectBounds.Bottom - shapeFormula["y1"] * 2f, this._rectBounds.Width * 2f, shapeFormula["y1"] * 2f, 90f, 90f);
    leftBracketPath.AddArc(this._rectBounds.Right - this._rectBounds.Width, this._rectBounds.Y, this._rectBounds.Width * 2f, shapeFormula["y1"] * 2f, 180f, 90f);
    return leftBracketPath;
  }

  internal GraphicsPath GetRightBracketPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RightBracket);
    GraphicsPath rightBracketPath = new GraphicsPath();
    rightBracketPath.AddArc(this._rectBounds.X - this._rectBounds.Width, this._rectBounds.Y, this._rectBounds.Width * 2f, shapeFormula["y1"] * 2f, 270f, 90f);
    rightBracketPath.AddArc(this._rectBounds.X - this._rectBounds.Width, this._rectBounds.Y + shapeFormula["y2"] - shapeFormula["y1"], this._rectBounds.Width * 2f, shapeFormula["y1"] * 2f, 0.0f, 90f);
    return rightBracketPath;
  }

  internal GraphicsPath GetLeftBracePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftBrace);
    GraphicsPath leftBracePath = new GraphicsPath();
    leftBracePath.AddArc(this._rectBounds.Right - this._rectBounds.Width / 2f, this._rectBounds.Bottom - shapeFormula["y1"] * 2f, this._rectBounds.Width, shapeFormula["y1"] * 2f, 90f, 90f);
    leftBracePath.AddArc(this._rectBounds.X - this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["y4"] - shapeFormula["y1"], this._rectBounds.Width, shapeFormula["y1"] * 2f, 0.0f, -90f);
    leftBracePath.AddArc(this._rectBounds.X - this._rectBounds.Width / 2f, (float) ((double) this._rectBounds.Y + (double) shapeFormula["y4"] - (double) shapeFormula["y1"] * 3.0), this._rectBounds.Width, shapeFormula["y1"] * 2f, 90f, -90f);
    leftBracePath.AddArc(this._rectBounds.Right - this._rectBounds.Width / 2f, this._rectBounds.Y, this._rectBounds.Width, shapeFormula["y1"] * 2f, 180f, 90f);
    return leftBracePath;
  }

  internal GraphicsPath GetRightBracePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RightBrace);
    GraphicsPath rightBracePath = new GraphicsPath();
    rightBracePath.AddArc(this._rectBounds.X - this._rectBounds.Width / 2f, this._rectBounds.Top, this._rectBounds.Width, shapeFormula["y1"] * 2f, 270f, 90f);
    PointF pointF = new PointF(this._rectBounds.X + this._rectBounds.Width, this._rectBounds.Y + shapeFormula["y2"] - shapeFormula["y1"]);
    rightBracePath.AddArc(pointF.X - this._rectBounds.Width / 2f, pointF.Y, this._rectBounds.Width, shapeFormula["y1"] * 2f, 180f, -90f);
    rightBracePath.AddArc(pointF.X - this._rectBounds.Width / 2f, pointF.Y + shapeFormula["y1"] * 2f, this._rectBounds.Width, shapeFormula["y1"] * 2f, 270f, -90f);
    rightBracePath.AddArc(this._rectBounds.X - this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["y4"] - shapeFormula["y1"], this._rectBounds.Width, shapeFormula["y1"] * 2f, 0.0f, 90f);
    return rightBracePath;
  }

  internal GraphicsPath GetRightArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RightArrow);
    GraphicsPath rightArrowPath = new GraphicsPath();
    PointF[] points = new PointF[7]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"])
    };
    rightArrowPath.AddLines(points);
    rightArrowPath.CloseFigure();
    return rightArrowPath;
  }

  internal GraphicsPath GetLeftArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftArrow);
    GraphicsPath leftArrowPath = new GraphicsPath();
    PointF[] points = new PointF[7]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom)
    };
    leftArrowPath.AddLines(points);
    leftArrowPath.CloseFigure();
    return leftArrowPath;
  }

  internal GraphicsPath GetUpArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.UpArrow);
    GraphicsPath upArrowPath = new GraphicsPath();
    PointF[] points = new PointF[7]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"])
    };
    upArrowPath.AddLines(points);
    upArrowPath.CloseFigure();
    return upArrowPath;
  }

  internal GraphicsPath GetDownArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.DownArrow);
    GraphicsPath downArrowPath = new GraphicsPath();
    PointF[] points = new PointF[7]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom)
    };
    downArrowPath.AddLines(points);
    downArrowPath.CloseFigure();
    return downArrowPath;
  }

  internal GraphicsPath GetLeftRightArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftRightArrow);
    GraphicsPath leftRightArrowPath = new GraphicsPath();
    PointF[] points = new PointF[10]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom)
    };
    leftRightArrowPath.AddLines(points);
    leftRightArrowPath.CloseFigure();
    return leftRightArrowPath;
  }

  internal GraphicsPath GetCurvedRightArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedRightArrow);
    GraphicsPath curvedRightArrowPath = new GraphicsPath();
    PointF[] pointFArray = new PointF[7];
    pointFArray[0] = new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["hR"]);
    curvedRightArrowPath.AddArc(pointFArray[0].X, pointFArray[0].Y - shapeFormula["hR"], this._rectBounds.Width * 2f, shapeFormula["hR"] * 2f, 180f, shapeFormula["mswAng"]);
    pointFArray[1] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"]);
    pointFArray[2] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y6"]);
    curvedRightArrowPath.AddLine(pointFArray[1], pointFArray[2]);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y8"]);
    curvedRightArrowPath.AddLine(pointFArray[2], pointFArray[3]);
    pointFArray[4] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y7"]);
    curvedRightArrowPath.AddArc(pointFArray[4].X - shapeFormula["x1"], pointFArray[4].Y - shapeFormula["hR"] * 2f, this._rectBounds.Width * 2f, shapeFormula["hR"] * 2f, shapeFormula["stAng"], shapeFormula["swAng"]);
    curvedRightArrowPath.CloseFigure();
    pointFArray[5] = new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["hR"]);
    curvedRightArrowPath.AddArc(pointFArray[5].X, pointFArray[5].Y - shapeFormula["hR"], this._rectBounds.Width * 2f, shapeFormula["hR"] * 2f, 180f, 90f);
    pointFArray[6] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["th"]);
    curvedRightArrowPath.AddLine(pointFArray[5].X + this._rectBounds.Width, pointFArray[5].Y - shapeFormula["hR"], pointFArray[6].X, pointFArray[6].Y);
    curvedRightArrowPath.AddArc(pointFArray[6].X - this._rectBounds.Width, pointFArray[6].Y, this._rectBounds.Width * 2f, shapeFormula["hR"] * 2f, 270f, shapeFormula["swAng2"]);
    return curvedRightArrowPath;
  }

  internal GraphicsPath GetCurvedLeftArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedLeftArrow);
    GraphicsPath curvedLeftArrowPath = new GraphicsPath();
    PointF[] pointFArray = new PointF[7];
    pointFArray[0] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y3"]);
    curvedLeftArrowPath.AddArc(this._rectBounds.Right - this._rectBounds.Width * 2f, pointFArray[0].Y - shapeFormula["hR"], this._rectBounds.Width * 2f, shapeFormula["hR"] * 2f, 0.0f, -90f);
    pointFArray[1] = new PointF(this._rectBounds.X, this._rectBounds.Y);
    curvedLeftArrowPath.AddArc(pointFArray[1].X - this._rectBounds.Width, pointFArray[1].Y, this._rectBounds.Width * 2f, shapeFormula["hR"] * 2f, 270f, 90f);
    pointFArray[2] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y3"]);
    curvedLeftArrowPath.AddArc(pointFArray[2].X - this._rectBounds.Width * 2f, pointFArray[2].Y - shapeFormula["hR"], this._rectBounds.Width * 2f, shapeFormula["hR"] * 2f, 0.0f, shapeFormula["swAng"]);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y8"]);
    pointFArray[4] = new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y6"]);
    curvedLeftArrowPath.AddLine(pointFArray[3], pointFArray[4]);
    pointFArray[5] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"]);
    curvedLeftArrowPath.AddLine(pointFArray[4], pointFArray[5]);
    pointFArray[6] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y5"]);
    curvedLeftArrowPath.AddLine(pointFArray[5], pointFArray[6]);
    curvedLeftArrowPath.AddArc(this._rectBounds.X - this._rectBounds.Width, pointFArray[1].Y, this._rectBounds.Width * 2f, shapeFormula["hR"] * 2f, shapeFormula["swAng"], shapeFormula["swAng2"]);
    return curvedLeftArrowPath;
  }

  internal GraphicsPath GetCurvedUpArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedUpArrow);
    GraphicsPath curvedUpArrowPath = new GraphicsPath();
    PointF[] pointFArray = new PointF[7];
    SizeF sizeF = new SizeF(shapeFormula["wR"], this._rectBounds.Height);
    pointFArray[0] = new PointF(this._rectBounds.X + shapeFormula["ix"], this._rectBounds.Y + shapeFormula["iy"]);
    pointFArray[1] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]);
    pointFArray[2] = new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y1"]);
    pointFArray[4] = new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Y + shapeFormula["y1"]);
    pointFArray[5] = new PointF(this._rectBounds.X + shapeFormula["wR"], this._rectBounds.Bottom);
    pointFArray[6] = new PointF(this._rectBounds.X + shapeFormula["th"], this._rectBounds.Y);
    curvedUpArrowPath.AddArc(pointFArray[5].X - sizeF.Width, pointFArray[0].Y - sizeF.Height - shapeFormula["iy"], sizeF.Width * 2f, sizeF.Height * 2f, shapeFormula["stAng2"], shapeFormula["swAng2"]);
    curvedUpArrowPath.AddLine(pointFArray[1], pointFArray[2]);
    curvedUpArrowPath.AddLine(pointFArray[2], pointFArray[3]);
    curvedUpArrowPath.AddLine(pointFArray[3], pointFArray[4]);
    curvedUpArrowPath.AddArc(pointFArray[6].X, this._rectBounds.Y - sizeF.Height, sizeF.Width * 2f, sizeF.Height * 2f, shapeFormula["stAng3"], shapeFormula["swAng"]);
    curvedUpArrowPath.AddArc(pointFArray[5].X - sizeF.Width, pointFArray[5].Y - sizeF.Height * 2f, sizeF.Width * 2f, sizeF.Height * 2f, 90f, 90f);
    curvedUpArrowPath.AddArc(pointFArray[6].X, pointFArray[6].Y - sizeF.Height, sizeF.Width * 2f, sizeF.Height * 2f, 180f, -90f);
    return curvedUpArrowPath;
  }

  internal GraphicsPath GetCurvedDownArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedDownArrow);
    GraphicsPath curvedDownArrowPath = new GraphicsPath();
    PointF[] pointFArray = new PointF[7];
    SizeF sizeF = new SizeF(shapeFormula["wR"], this._rectBounds.Height);
    pointFArray[0] = new PointF(this._rectBounds.X + shapeFormula["ix"], this._rectBounds.Y + shapeFormula["iy"]);
    pointFArray[1] = new PointF(this._rectBounds.X, this._rectBounds.Bottom);
    pointFArray[2] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y1"]);
    pointFArray[4] = new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Bottom);
    pointFArray[5] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]);
    pointFArray[6] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y1"]);
    curvedDownArrowPath.AddArc(pointFArray[2].X - sizeF.Width, this._rectBounds.Y, sizeF.Width * 2f, sizeF.Height * 2f, shapeFormula["stAng2"], shapeFormula["swAng2"]);
    curvedDownArrowPath.AddArc(this._rectBounds.X, this._rectBounds.Y, sizeF.Width * 2f, sizeF.Height * 2f, 180f, 90f);
    curvedDownArrowPath.AddArc(this._rectBounds.X, this._rectBounds.Y, sizeF.Width * 2f, sizeF.Height * 2f, 270f, shapeFormula["swAng"]);
    curvedDownArrowPath.AddLine(pointFArray[6], pointFArray[5]);
    curvedDownArrowPath.AddLine(pointFArray[5], pointFArray[4]);
    curvedDownArrowPath.AddLine(pointFArray[4], pointFArray[3]);
    curvedDownArrowPath.AddLine(pointFArray[3], new PointF(pointFArray[3].X - shapeFormula["x5"] + shapeFormula["x4"], pointFArray[3].Y));
    curvedDownArrowPath.AddArc(pointFArray[2].X - sizeF.Width, this._rectBounds.Y, sizeF.Width * 2f, sizeF.Height * 2f, shapeFormula["stAng"], shapeFormula["mswAng"]);
    PointF[] points = new PointF[1]
    {
      new PointF(this._rectBounds.X + sizeF.Width, this._rectBounds.Y)
    };
    curvedDownArrowPath.AddLines(points);
    return curvedDownArrowPath;
  }

  internal GraphicsPath GetUpDownArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.UpDownArrow);
    GraphicsPath upDownArrowPath = new GraphicsPath();
    PointF[] points = new PointF[10]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"])
    };
    upDownArrowPath.AddLines(points);
    upDownArrowPath.CloseFigure();
    return upDownArrowPath;
  }

  internal GraphicsPath GetQuadArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.QuadArrow);
    GraphicsPath quadArrowPath = new GraphicsPath();
    PointF[] points = new PointF[24]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y6"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y6"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y6"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y6"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y5"])
    };
    quadArrowPath.AddLines(points);
    quadArrowPath.CloseFigure();
    return quadArrowPath;
  }

  internal GraphicsPath GetLeftRightUpArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftRightUpArrow);
    GraphicsPath rightUpArrowPath = new GraphicsPath();
    PointF[] points = new PointF[17]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom)
    };
    rightUpArrowPath.AddLines(points);
    rightUpArrowPath.CloseFigure();
    return rightUpArrowPath;
  }

  internal GraphicsPath GetBentArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.BentArrow);
    GraphicsPath bentArrowPath = new GraphicsPath();
    PointF[] points = new PointF[6];
    bentArrowPath.AddLine(this._rectBounds.X, this._rectBounds.Bottom, this._rectBounds.X, this._rectBounds.Y + shapeFormula["y5"]);
    bentArrowPath.AddArc(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y5"] - shapeFormula["bd"], shapeFormula["bd"] * 2f, shapeFormula["bd"] * 2f, 180f, 90f);
    points[0] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["dh2"]);
    points[1] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y);
    points[2] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["aw2"]);
    points[3] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y4"]);
    points[4] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y3"]);
    points[5] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]);
    bentArrowPath.AddLines(points);
    if ((double) shapeFormula["bd2"] == 0.0)
      shapeFormula["bd2"] = 1f;
    bentArrowPath.AddArc(points[5].X - shapeFormula["bd2"], points[5].Y, shapeFormula["bd2"] * 2f, shapeFormula["bd2"] * 2f, 270f, -90f);
    bentArrowPath.AddLine(points[5].X - shapeFormula["bd2"], points[5].Y + shapeFormula["bd2"], this._rectBounds.X + shapeFormula["th"], this._rectBounds.Bottom);
    bentArrowPath.CloseFigure();
    return bentArrowPath;
  }

  internal GraphicsPath GetUTrunArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.UTurnArrow);
    if ((double) shapeFormula["bd2"] == 0.0)
      shapeFormula["bd2"] = 1f;
    GraphicsPath utrunArrowPath = new GraphicsPath();
    PointF[] pointFArray = new PointF[11];
    pointFArray[0] = new PointF(this._rectBounds.X, this._rectBounds.Bottom);
    pointFArray[1] = new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["bd"]);
    utrunArrowPath.AddLine(pointFArray[0], pointFArray[1]);
    utrunArrowPath.AddArc(pointFArray[1].X, pointFArray[1].Y - shapeFormula["bd"], shapeFormula["bd"] * 2f, shapeFormula["bd"] * 2f, 180f, 90f);
    pointFArray[2] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y);
    utrunArrowPath.AddLine(pointFArray[1].X + shapeFormula["bd"], pointFArray[1].Y - shapeFormula["bd"], pointFArray[2].X, pointFArray[2].Y);
    utrunArrowPath.AddArc(pointFArray[2].X - shapeFormula["bd"], pointFArray[2].Y, shapeFormula["bd"] * 2f, shapeFormula["bd"] * 2f, 270f, 90f);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["x9"], this._rectBounds.Y + shapeFormula["y4"]);
    utrunArrowPath.AddLine(pointFArray[2].X + shapeFormula["bd"], pointFArray[2].Y + shapeFormula["bd"], pointFArray[3].X, pointFArray[3].Y);
    pointFArray[4] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y4"]);
    utrunArrowPath.AddLine(pointFArray[3], pointFArray[4]);
    pointFArray[5] = new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y5"]);
    utrunArrowPath.AddLine(pointFArray[4], pointFArray[5]);
    pointFArray[6] = new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y4"]);
    utrunArrowPath.AddLine(pointFArray[5], pointFArray[6]);
    pointFArray[7] = new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Y + shapeFormula["y4"]);
    utrunArrowPath.AddLine(pointFArray[6], pointFArray[7]);
    pointFArray[8] = new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Y + shapeFormula["x3"]);
    utrunArrowPath.AddLine(pointFArray[7], pointFArray[8]);
    utrunArrowPath.AddArc(pointFArray[8].X - shapeFormula["bd2"] * 2f, pointFArray[8].Y - shapeFormula["bd2"], shapeFormula["bd2"] * 2f, shapeFormula["bd2"] * 2f, 0.0f, -90f);
    pointFArray[9] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["th"]);
    utrunArrowPath.AddLine(pointFArray[8].X - shapeFormula["bd2"], pointFArray[8].Y - shapeFormula["bd2"], pointFArray[9].X, pointFArray[9].Y);
    utrunArrowPath.AddArc(pointFArray[9].X - shapeFormula["bd2"], pointFArray[9].Y, shapeFormula["bd2"] * 2f, shapeFormula["bd2"] * 2f, 270f, -90f);
    pointFArray[10] = new PointF(this._rectBounds.X + shapeFormula["th"], this._rectBounds.Bottom);
    utrunArrowPath.AddLine(pointFArray[9].X - shapeFormula["bd2"], pointFArray[9].Y + shapeFormula["bd2"], pointFArray[10].X, pointFArray[10].Y);
    utrunArrowPath.CloseFigure();
    return utrunArrowPath;
  }

  internal GraphicsPath GetLeftUpArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftUpArrow);
    GraphicsPath leftUpArrowPath = new GraphicsPath();
    PointF[] points = new PointF[12]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom)
    };
    leftUpArrowPath.AddLines(points);
    leftUpArrowPath.CloseFigure();
    return leftUpArrowPath;
  }

  internal GraphicsPath GetBentUpArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.BentUpArrow);
    GraphicsPath bentUpArrowPath = new GraphicsPath();
    PointF[] points = new PointF[9]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    bentUpArrowPath.AddLines(points);
    bentUpArrowPath.CloseFigure();
    return bentUpArrowPath;
  }

  internal GraphicsPath GetStripedRightArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.StripedRightArrow);
    GraphicsPath stripedRightArrowPath = new GraphicsPath();
    PointF[] points1 = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 32f, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 32f, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"])
    };
    stripedRightArrowPath.AddLines(points1);
    stripedRightArrowPath.CloseFigure();
    PointF[] points2 = new PointF[4]
    {
      new PointF(this._rectBounds.X + Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 16f, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 8f, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 8f, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 16f, this._rectBounds.Y + shapeFormula["y2"])
    };
    stripedRightArrowPath.AddLines(points2);
    stripedRightArrowPath.CloseFigure();
    PointF[] points3 = new PointF[7]
    {
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y2"])
    };
    stripedRightArrowPath.AddLines(points3);
    stripedRightArrowPath.CloseFigure();
    return stripedRightArrowPath;
  }

  internal GraphicsPath GetNotchedRightArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.NotchedRightArrow);
    GraphicsPath notchedRightArrowPath = new GraphicsPath();
    PointF[] points = new PointF[8]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + this._rectBounds.Height / 2f)
    };
    notchedRightArrowPath.AddLines(points);
    notchedRightArrowPath.CloseFigure();
    return notchedRightArrowPath;
  }

  internal GraphicsPath GetPentagonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Pentagon);
    GraphicsPath pentagonPath = new GraphicsPath();
    PointF[] points = new PointF[5]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    pentagonPath.AddLines(points);
    pentagonPath.CloseFigure();
    return pentagonPath;
  }

  internal GraphicsPath GetChevronPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Chevron);
    GraphicsPath chevronPath = new GraphicsPath();
    PointF[] points = new PointF[6]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + this._rectBounds.Height / 2f)
    };
    chevronPath.AddLines(points);
    chevronPath.CloseFigure();
    return chevronPath;
  }

  internal GraphicsPath GetRightArrowCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RightArrowCallout);
    GraphicsPath arrowCalloutPath = new GraphicsPath();
    PointF[] points = new PointF[11]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    arrowCalloutPath.AddLines(points);
    arrowCalloutPath.CloseFigure();
    return arrowCalloutPath;
  }

  internal GraphicsPath GetDownArrowCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.DownArrowCallout);
    GraphicsPath arrowCalloutPath = new GraphicsPath();
    PointF[] points = new PointF[11]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"])
    };
    arrowCalloutPath.AddLines(points);
    arrowCalloutPath.CloseFigure();
    return arrowCalloutPath;
  }

  internal GraphicsPath GetLeftArrowCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftArrowCallout);
    GraphicsPath arrowCalloutPath = new GraphicsPath();
    PointF[] points = new PointF[11]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"])
    };
    arrowCalloutPath.AddLines(points);
    arrowCalloutPath.CloseFigure();
    return arrowCalloutPath;
  }

  internal GraphicsPath GetUpArrowCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.UpArrowCallout);
    GraphicsPath arrowCalloutPath = new GraphicsPath();
    PointF[] points = new PointF[11]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    arrowCalloutPath.AddLines(points);
    arrowCalloutPath.CloseFigure();
    return arrowCalloutPath;
  }

  internal GraphicsPath GetLeftRightArrowCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftRightArrowCallout);
    GraphicsPath arrowCalloutPath = new GraphicsPath();
    PointF[] points = new PointF[18]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"])
    };
    arrowCalloutPath.AddLines(points);
    arrowCalloutPath.CloseFigure();
    return arrowCalloutPath;
  }

  internal GraphicsPath GetQuadArrowCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.QuadArrowCallout);
    GraphicsPath arrowCalloutPath = new GraphicsPath();
    PointF[] points = new PointF[32 /*0x20*/]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["ah"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["ah"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ah"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["ah"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["ah"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["ah"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y6"]),
      new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Y + shapeFormula["y7"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y7"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y8"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y8"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y8"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y8"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y7"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y7"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["ah"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["ah"], this._rectBounds.Y + shapeFormula["y6"])
    };
    arrowCalloutPath.AddLines(points);
    arrowCalloutPath.CloseFigure();
    return arrowCalloutPath;
  }

  internal GraphicsPath GetCircularArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CircularArrow);
    GraphicsPath circularArrowPath = new GraphicsPath();
    PointF[] pointFArray = new PointF[6];
    SizeF sizeF1 = new SizeF(shapeFormula["rw1"], shapeFormula["rh1"]);
    SizeF sizeF2 = new SizeF(shapeFormula["rw2"], shapeFormula["rh2"]);
    pointFArray[0] = new PointF(this._rectBounds.X + shapeFormula["xE"], this._rectBounds.Y + shapeFormula["yE"]);
    if ((double) shapeFormula["stAng"] > 350.0 && (double) shapeFormula["swAng"] > 340.0)
    {
      circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th2"], sizeF1.Width * 2f, sizeF1.Height * 2f, shapeFormula["stAng"], shapeFormula["swAng"]);
      pointFArray[1] = new PointF(this._rectBounds.X + shapeFormula["xGp"], this._rectBounds.Y + shapeFormula["yGp"]);
      pointFArray[1] = new PointF(this._rectBounds.X + shapeFormula["xGp"], this._rectBounds.Y + shapeFormula["yGp"]);
      pointFArray[2] = new PointF(this._rectBounds.X + shapeFormula["xA"], this._rectBounds.Y + shapeFormula["yA"]);
      pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["xBp"], this._rectBounds.Y + shapeFormula["yBp"]);
      pointFArray[4] = new PointF(this._rectBounds.X + shapeFormula["xC"], this._rectBounds.Y + shapeFormula["yC"]);
      circularArrowPath.AddLine(new PointF(pointFArray[1].X - (pointFArray[4].X - pointFArray[3].X), pointFArray[1].Y), pointFArray[1]);
      circularArrowPath.AddLine(pointFArray[1], pointFArray[2]);
      circularArrowPath.AddLine(pointFArray[2], pointFArray[3]);
      circularArrowPath.AddLine(pointFArray[3], pointFArray[4]);
      circularArrowPath.AddArc(this._rectBounds.X + (sizeF1.Width - sizeF2.Width) + shapeFormula["th2"], this._rectBounds.Y + (sizeF1.Height - sizeF2.Height) + shapeFormula["th2"], sizeF2.Width * 2f, sizeF2.Height * 2f, shapeFormula["istAng"], shapeFormula["iswAng"]);
      circularArrowPath.CloseFigure();
      return circularArrowPath;
    }
    if ((double) shapeFormula["swAng"] < 180.0)
    {
      if ((double) shapeFormula["stAng"] > 220.0 && (double) shapeFormula["swAng"] > 80.0 && (double) shapeFormula["enAng"] > 200.0 && (double) shapeFormula["enAng"] < 350.0)
      {
        if ((double) shapeFormula["swAng"] < 120.0)
        {
          circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th2"], sizeF1.Width * 2f, sizeF1.Height * 2f, shapeFormula["stAng"], shapeFormula["enAng"] + shapeFormula["maxAng"]);
          pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
          pointFArray[2] = new PointF(pointFArray[1].X + (shapeFormula["q16"] + shapeFormula["p1"]), pointFArray[1].Y + shapeFormula["q16"] / 2f);
          pointFArray[3] = new PointF(pointFArray[2].X + (shapeFormula["q9"] - shapeFormula["u11"] / 4f), pointFArray[2].Y + (shapeFormula["v18"] - shapeFormula["v16"]));
          pointFArray[4] = new PointF(pointFArray[2].X + shapeFormula["q9"], pointFArray[1].Y + shapeFormula["v18"]);
          pointFArray[5] = new PointF(pointFArray[4].X - (shapeFormula["v16"] - shapeFormula["p1"]), pointFArray[4].Y + shapeFormula["q16"] / 2f);
          circularArrowPath.AddLine(pointFArray[1], pointFArray[2]);
          circularArrowPath.AddLine(pointFArray[2], pointFArray[3]);
          circularArrowPath.AddLine(pointFArray[3], pointFArray[4]);
          circularArrowPath.AddLine(pointFArray[4], pointFArray[5]);
          circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th"] + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th"] + shapeFormula["th2"], sizeF2.Width * 2f, sizeF2.Height * 2f, (float) -((double) shapeFormula["enAng"] - (double) shapeFormula["istAng"] * 2.0), (float) -((double) shapeFormula["enAng"] + (double) shapeFormula["maxAng"]));
          circularArrowPath.CloseFigure();
          return circularArrowPath;
        }
        circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th2"], sizeF1.Width * 2f, sizeF1.Height * 2f, shapeFormula["stAng"], 180f + shapeFormula["swAng"]);
        if ((double) shapeFormula["istAng"] < 75.0)
        {
          pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
          pointFArray[2] = new PointF(pointFArray[1].X + (shapeFormula["q16"] + shapeFormula["p1"]), pointFArray[1].Y - (shapeFormula["p1"] + shapeFormula["u9"] / 3f));
          pointFArray[3] = new PointF(pointFArray[2].X + (shapeFormula["q9"] + shapeFormula["v16"] / 4f), pointFArray[2].Y + (shapeFormula["p4"] + shapeFormula["u9"]));
          pointFArray[4] = new PointF(pointFArray[3].X - shapeFormula["v16"] / 4f, pointFArray[3].Y + shapeFormula["v18"] + shapeFormula["u12"]);
          pointFArray[5] = new PointF(pointFArray[4].X - (shapeFormula["v16"] - shapeFormula["p1"]), pointFArray[4].Y - shapeFormula["q9"] / 2f);
        }
        else
        {
          pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
          pointFArray[2] = new PointF(pointFArray[1].X + (shapeFormula["q9"] + shapeFormula["q16"]), pointFArray[1].Y + shapeFormula["p5"]);
          pointFArray[3] = new PointF(pointFArray[2].X - (shapeFormula["p5"] - shapeFormula["u9"] - shapeFormula["q9"]), pointFArray[2].Y - (shapeFormula["u13"] + shapeFormula["q17"] - shapeFormula["p1"]));
          pointFArray[4] = new PointF(pointFArray[1].X - shapeFormula["q16"], pointFArray[3].Y - (shapeFormula["u13"] - shapeFormula["u12"] + shapeFormula["p1"]));
          pointFArray[5] = new PointF(pointFArray[4].X + (shapeFormula["q9"] + shapeFormula["q16"]), pointFArray[4].Y + shapeFormula["p5"]);
        }
        circularArrowPath.AddLine(pointFArray[1], pointFArray[2]);
        circularArrowPath.AddLine(pointFArray[2], pointFArray[3]);
        circularArrowPath.AddLine(pointFArray[3], pointFArray[4]);
        circularArrowPath.AddLine(pointFArray[4], pointFArray[5]);
        circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th"] + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th"] + shapeFormula["th2"], sizeF2.Width * 2f, sizeF2.Height * 2f, 180f + shapeFormula["istAng"], shapeFormula["iswAng"] - 180f);
        circularArrowPath.CloseFigure();
        return circularArrowPath;
      }
      circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th2"], sizeF1.Width * 2f, sizeF1.Height * 2f, shapeFormula["stAng"], shapeFormula["swAng"]);
      if ((double) shapeFormula["stAng"] > 269.0 && (double) shapeFormula["enAng"] < 50.0)
      {
        if ((double) shapeFormula["enAng"] > 10.0 && (double) shapeFormula["enAng"] < 30.0)
        {
          pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
          pointFArray[2] = new PointF(pointFArray[1].X + shapeFormula["q16"], pointFArray[1].Y + shapeFormula["q18"]);
          pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["xC"], this._rectBounds.Y + shapeFormula["yGp"]);
          pointFArray[4] = new PointF(pointFArray[3].X - shapeFormula["q20"], pointFArray[3].Y - shapeFormula["v20"] * 2f);
          pointFArray[5] = new PointF(pointFArray[4].X + shapeFormula["q16"], pointFArray[4].Y + shapeFormula["q18"]);
        }
        else if ((double) shapeFormula["enAng"] > 30.0)
        {
          pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
          pointFArray[2] = new PointF(pointFArray[1].X + shapeFormula["q16"], pointFArray[1].Y + shapeFormula["q18"]);
          pointFArray[3] = new PointF(pointFArray[2].X - shapeFormula["q9"] - shapeFormula["u11"], pointFArray[2].Y - shapeFormula["p2"] / 2f - shapeFormula["u8"]);
          pointFArray[4] = new PointF(pointFArray[2].X - shapeFormula["q9"], pointFArray[3].Y - shapeFormula["q9"] + shapeFormula["u12"]);
          pointFArray[5] = new PointF(pointFArray[4].X + shapeFormula["q16"], pointFArray[4].Y + shapeFormula["q18"]);
        }
        else
        {
          pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
          pointFArray[2] = new PointF(pointFArray[1].X + shapeFormula["q16"], pointFArray[1].Y + shapeFormula["q18"]);
          pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["xC"] - shapeFormula["v18"], this._rectBounds.Y + shapeFormula["yA"] + shapeFormula["v18"]);
          pointFArray[4] = new PointF(pointFArray[2].X - shapeFormula["dI"], pointFArray[3].Y - shapeFormula["th"]);
          pointFArray[5] = new PointF(pointFArray[4].X + shapeFormula["q16"], pointFArray[4].Y + shapeFormula["q18"]);
        }
      }
      else if ((double) shapeFormula["stAng"] % 3.0 == 0.0 && (double) shapeFormula["enAng"] < 85.0)
      {
        pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
        pointFArray[2] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, pointFArray[1].Y - shapeFormula["p5"]);
        pointFArray[3] = new PointF(pointFArray[2].X - shapeFormula["dxH"], pointFArray[2].Y - shapeFormula["thh"]);
        pointFArray[4] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, pointFArray[2].Y - shapeFormula["dI"]);
        pointFArray[5] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, pointFArray[4].Y - shapeFormula["p5"]);
      }
      else if (((double) shapeFormula["stAng"] % 3.0 == 0.0 || (double) shapeFormula["stAng"] > 320.0 && (double) shapeFormula["stAng"] < 340.0) && (double) shapeFormula["enAng"] > 85.0)
      {
        pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
        pointFArray[2] = new PointF(pointFArray[1].X - shapeFormula["p5"], circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
        pointFArray[3] = new PointF(pointFArray[2].X - shapeFormula["thh"], pointFArray[2].Y - shapeFormula["dyH"]);
        pointFArray[4] = new PointF(pointFArray[2].X - shapeFormula["dI"], circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
        pointFArray[5] = new PointF(pointFArray[4].X - shapeFormula["p5"], circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
      }
      else if ((double) shapeFormula["stAng"] < 30.0 && (double) shapeFormula["stAng"] % 3.0 != 0.0)
      {
        if ((double) shapeFormula["stAng"] < 15.0 && (double) shapeFormula["swAng"] < 30.0)
        {
          if ((double) shapeFormula["enAng"] < 40.0)
          {
            pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
            pointFArray[2] = new PointF(pointFArray[1].X + shapeFormula["q16"], pointFArray[1].Y + shapeFormula["q18"]);
            pointFArray[3] = new PointF(pointFArray[2].X - (shapeFormula["q20"] + shapeFormula["p1"]), pointFArray[2].Y - (shapeFormula["q20"] - shapeFormula["q16"]));
            pointFArray[4] = new PointF(pointFArray[2].X - shapeFormula["q9"], pointFArray[3].Y - (float) ((double) shapeFormula["p1"] + (double) shapeFormula["p2"] + (double) shapeFormula["u8"] / 2.0));
            pointFArray[5] = new PointF(pointFArray[4].X + shapeFormula["q16"], pointFArray[4].Y + shapeFormula["q18"]);
          }
          else
          {
            pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
            pointFArray[2] = new PointF(pointFArray[1].X + shapeFormula["q17"], pointFArray[1].Y + shapeFormula["q19"]);
            pointFArray[3] = new PointF(pointFArray[1].X - shapeFormula["u0"] * 3f, pointFArray[2].Y + (shapeFormula["p1"] + shapeFormula["u0"] + shapeFormula["u10"]));
            pointFArray[4] = new PointF(pointFArray[2].X + shapeFormula["q9"], pointFArray[3].Y + (shapeFormula["q19"] * 3f - shapeFormula["u0"]));
            pointFArray[5] = new PointF(pointFArray[4].X + shapeFormula["q17"], pointFArray[4].Y + shapeFormula["q19"]);
          }
        }
        else
        {
          pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
          pointFArray[2] = new PointF(pointFArray[1].X + shapeFormula["q16"], pointFArray[1].Y + shapeFormula["q18"]);
          pointFArray[3] = new PointF(pointFArray[2].X + shapeFormula["u13"] / 2f, pointFArray[1].Y - shapeFormula["u7"] / 2f);
          pointFArray[4] = new PointF(pointFArray[2].X - shapeFormula["p3"], pointFArray[2].Y - shapeFormula["dyO"]);
          pointFArray[5] = new PointF(pointFArray[4].X + shapeFormula["q16"], pointFArray[4].Y + shapeFormula["q18"]);
        }
      }
      else if ((double) shapeFormula["swAng"] < 80.0 && (double) shapeFormula["swAng"] > 60.0)
      {
        pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
        pointFArray[2] = new PointF(pointFArray[1].X + shapeFormula["q17"], pointFArray[1].Y + shapeFormula["q19"]);
        pointFArray[3] = new PointF(pointFArray[2].X - shapeFormula["q17"] + shapeFormula["q19"], (float) ((double) pointFArray[2].Y + (double) shapeFormula["p4"] - (double) shapeFormula["p5"] - (double) shapeFormula["u7"] * 2.0));
        pointFArray[4] = new PointF(pointFArray[2].X + shapeFormula["v9"], pointFArray[3].Y + shapeFormula["q19"] * 2f);
        pointFArray[5] = new PointF(pointFArray[4].X + shapeFormula["q17"], pointFArray[4].Y + shapeFormula["q19"]);
      }
      else if ((double) shapeFormula["swAng"] < 165.0 && (double) shapeFormula["swAng"] > 130.0)
      {
        pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
        pointFArray[2] = new PointF(pointFArray[1].X + shapeFormula["q16"], pointFArray[1].Y + shapeFormula["q18"]);
        pointFArray[3] = new PointF(pointFArray[2].X - (shapeFormula["q9"] - shapeFormula["u11"]), pointFArray[1].Y + shapeFormula["u8"]);
        pointFArray[4] = new PointF(pointFArray[3].X + shapeFormula["u9"] * 2f, pointFArray[2].Y - shapeFormula["dyO"]);
        pointFArray[5] = new PointF(pointFArray[4].X + shapeFormula["q16"], pointFArray[4].Y + shapeFormula["q18"]);
      }
      else if ((double) shapeFormula["swAng"] < 45.0 && (double) shapeFormula["stAng"] > 180.0)
      {
        if ((double) shapeFormula["aAng"] < 5.0 && (double) shapeFormula["istAng"] > 340.0)
        {
          pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
          pointFArray[2] = new PointF(pointFArray[1].X + shapeFormula["q17"], pointFArray[1].Y + shapeFormula["q19"]);
          pointFArray[3] = new PointF(pointFArray[1].X - shapeFormula["u0"] * 3f, pointFArray[2].Y + (shapeFormula["p1"] + shapeFormula["u0"] + shapeFormula["u10"]));
          pointFArray[4] = new PointF(pointFArray[2].X + shapeFormula["q9"], pointFArray[3].Y + (shapeFormula["q19"] * 3f - shapeFormula["u0"]));
          pointFArray[5] = new PointF(pointFArray[4].X + shapeFormula["q17"], pointFArray[4].Y + shapeFormula["q19"]);
        }
        else
        {
          pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
          pointFArray[2] = new PointF(pointFArray[1].X + shapeFormula["q17"], pointFArray[1].Y + shapeFormula["q19"]);
          pointFArray[3] = new PointF(pointFArray[2].X - (float) ((double) shapeFormula["q9"] + (double) shapeFormula["u11"] - (double) shapeFormula["u12"] / 2.0), pointFArray[2].Y + (float) ((double) shapeFormula["q21"] * 2.0 - (double) shapeFormula["u0"] / 2.0));
          pointFArray[4] = new PointF(pointFArray[2].X + shapeFormula["q9"], pointFArray[3].Y + (shapeFormula["q21"] + shapeFormula["p1"] - shapeFormula["u12"]));
          pointFArray[5] = new PointF(pointFArray[4].X + shapeFormula["q17"], pointFArray[4].Y + shapeFormula["q19"]);
        }
      }
      else if ((double) shapeFormula["stAng"] < 180.0 && (double) shapeFormula["swAng"] < 45.0)
      {
        pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
        pointFArray[2] = new PointF(pointFArray[1].X + shapeFormula["q16"], pointFArray[1].Y + shapeFormula["q18"]);
        pointFArray[3] = new PointF(pointFArray[2].X - (shapeFormula["q9"] + (float) ((double) shapeFormula["p1"] - (double) shapeFormula["u11"] - (double) shapeFormula["u12"] / 2.0)), pointFArray[2].Y - (shapeFormula["p3"] + shapeFormula["u11"] - shapeFormula["u9"] + shapeFormula["u0"]));
        pointFArray[4] = new PointF(pointFArray[2].X - shapeFormula["q9"], pointFArray[3].Y - (shapeFormula["q20"] + shapeFormula["q9"] - shapeFormula["u0"]));
        pointFArray[5] = new PointF(pointFArray[4].X - shapeFormula["v16"], pointFArray[4].Y + shapeFormula["q18"]);
      }
      else
      {
        pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
        pointFArray[2] = new PointF(pointFArray[1].X + shapeFormula["q17"], pointFArray[1].Y + shapeFormula["q19"]);
        pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["xF"], pointFArray[1].Y + shapeFormula["th"] + shapeFormula["u8"]);
        pointFArray[4] = new PointF(pointFArray[2].X + shapeFormula["q9"], pointFArray[3].Y - shapeFormula["u12"]);
        pointFArray[5] = new PointF(pointFArray[4].X + shapeFormula["q17"], pointFArray[4].Y + shapeFormula["q19"]);
      }
      circularArrowPath.AddLine(pointFArray[1], pointFArray[2]);
      circularArrowPath.AddLine(pointFArray[2], pointFArray[3]);
      circularArrowPath.AddLine(pointFArray[3], pointFArray[4]);
      circularArrowPath.AddLine(pointFArray[4], pointFArray[5]);
      circularArrowPath.AddArc(this._rectBounds.X + (sizeF1.Width - sizeF2.Width) + shapeFormula["th2"], this._rectBounds.Y + (sizeF1.Height - sizeF2.Height) + shapeFormula["th2"], sizeF2.Width * 2f, sizeF2.Height * 2f, shapeFormula["istAng"], shapeFormula["iswAng"]);
      circularArrowPath.CloseFigure();
      return circularArrowPath;
    }
    if ((double) shapeFormula["istAng"] < 180.0)
    {
      if ((double) shapeFormula["swAng"] > 220.0 && (double) shapeFormula["istAng"] < 86.0 && (double) shapeFormula["istAng"] > 84.0)
      {
        circularArrowPath.AddArc(this._rectBounds.X - shapeFormula["p5"], this._rectBounds.Y - shapeFormula["p5"], sizeF1.Width * 2f, sizeF1.Height * 2f, shapeFormula["stAng"], shapeFormula["enAng"] - shapeFormula["stAng"]);
        pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
        pointFArray[2] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, pointFArray[1].Y + shapeFormula["p5"]);
        pointFArray[3] = new PointF(pointFArray[2].X + shapeFormula["dxH"], pointFArray[2].Y + shapeFormula["thh"]);
        pointFArray[4] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, pointFArray[2].Y + shapeFormula["dI"]);
        pointFArray[5] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, pointFArray[4].Y + shapeFormula["p5"]);
        circularArrowPath.AddLine(pointFArray[1], pointFArray[2]);
        circularArrowPath.AddLine(pointFArray[2], pointFArray[3]);
        circularArrowPath.AddLine(pointFArray[3], pointFArray[4]);
        circularArrowPath.AddLine(pointFArray[4], pointFArray[5]);
        circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th"] - shapeFormula["p5"], this._rectBounds.Y + shapeFormula["th"] - shapeFormula["p5"], sizeF2.Width * 2f, sizeF2.Height * 2f, (float) -((double) shapeFormula["istAng"] + (double) shapeFormula["aAng"] * 2.0), (float) -((double) shapeFormula["enAng"] - (double) shapeFormula["stAng"]));
        circularArrowPath.CloseFigure();
        return circularArrowPath;
      }
      if ((double) shapeFormula["enAng"] < 220.0 && (double) shapeFormula["istAng"] < 40.0)
      {
        circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th2"], sizeF1.Width * 2f, sizeF1.Height * 2f, shapeFormula["stAng"], shapeFormula["enAng"] - shapeFormula["stAng"]);
        if ((double) shapeFormula["istAng"] < 15.0)
        {
          pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
          pointFArray[2] = new PointF(pointFArray[1].X + (shapeFormula["q16"] + shapeFormula["p1"]), pointFArray[1].Y - (float) ((double) shapeFormula["u12"] / 2.0 - (double) shapeFormula["u7"] / 6.0));
          pointFArray[3] = new PointF(pointFArray[1].X + (shapeFormula["p3"] - shapeFormula["p2"]), pointFArray[2].Y - (float) ((double) shapeFormula["p3"] - (double) shapeFormula["u10"] + (double) shapeFormula["u11"] / 2.0));
          pointFArray[4] = new PointF(pointFArray[2].X + shapeFormula["v9"], pointFArray[1].Y + (shapeFormula["p4"] + shapeFormula["u11"]));
          pointFArray[5] = new PointF(pointFArray[4].X + (shapeFormula["q16"] + shapeFormula["p1"]), pointFArray[4].Y - (float) ((double) shapeFormula["u12"] / 2.0 - (double) shapeFormula["u7"] / 6.0));
        }
        else
        {
          pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
          if ((double) shapeFormula["stAng"] % 3.0 == 0.0 || (double) shapeFormula["stAng"] > 175.0 && (double) shapeFormula["stAng"] < 190.0)
          {
            pointFArray[2] = new PointF(pointFArray[1].X + (shapeFormula["q16"] + shapeFormula["p1"]), pointFArray[1].Y + (shapeFormula["q18"] + shapeFormula["p2"]));
            pointFArray[3] = new PointF(pointFArray[2].X + (shapeFormula["q9"] + shapeFormula["u10"] - shapeFormula["u8"]), pointFArray[2].Y + (float) ((double) shapeFormula["u0"] - (double) shapeFormula["u8"] * 2.0 + (double) shapeFormula["u7"] / 6.0));
            pointFArray[4] = new PointF(pointFArray[2].X + shapeFormula["q9"], pointFArray[3].Y + (float) ((double) shapeFormula["q9"] + (double) shapeFormula["u10"] - (double) shapeFormula["u8"] * 3.0));
            pointFArray[5] = new PointF(pointFArray[4].X + (shapeFormula["q16"] + shapeFormula["p1"]), pointFArray[4].Y + (shapeFormula["q18"] + shapeFormula["p2"]));
          }
          else if ((double) shapeFormula["stAng"] > 190.0 && (double) shapeFormula["stAng"] < 270.0)
          {
            pointFArray[2] = new PointF(pointFArray[1].X + (shapeFormula["q16"] + shapeFormula["p1"]), pointFArray[1].Y + (shapeFormula["q18"] + shapeFormula["p2"]));
            pointFArray[3] = new PointF(pointFArray[2].X + (shapeFormula["q9"] + shapeFormula["u12"] * 2f - shapeFormula["u8"] - shapeFormula["u9"]), pointFArray[2].Y - (float) (((double) shapeFormula["u0"] - (double) shapeFormula["u8"] * 2.0 + (double) shapeFormula["u7"] / 6.0) * 2.0));
            pointFArray[4] = new PointF(pointFArray[2].X + shapeFormula["q9"], pointFArray[3].Y + (float) ((double) shapeFormula["q9"] + (double) shapeFormula["u12"] * 2.0 - (double) shapeFormula["u9"] / 2.0));
            pointFArray[5] = new PointF(pointFArray[4].X + (shapeFormula["q16"] + shapeFormula["p1"]), pointFArray[4].Y + (shapeFormula["q18"] + shapeFormula["p2"]));
          }
          else
          {
            pointFArray[2] = new PointF(pointFArray[1].X - shapeFormula["p4"], pointFArray[1].Y - shapeFormula["p1"] / 2f);
            pointFArray[3] = new PointF((float) ((double) pointFArray[2].X + (double) shapeFormula["v16"] + (double) shapeFormula["u9"] * 2.0), pointFArray[2].Y + shapeFormula["u9"] * 2f);
            pointFArray[4] = new PointF((float) ((double) pointFArray[2].X + (double) shapeFormula["q20"] + (double) shapeFormula["u9"] * 4.0), (float) ((double) pointFArray[3].Y + (double) shapeFormula["v16"] + (double) shapeFormula["u9"] * 2.0));
            pointFArray[5] = new PointF(pointFArray[4].X - shapeFormula["p4"], pointFArray[4].Y - shapeFormula["p1"] / 2f);
          }
        }
        circularArrowPath.AddLine(pointFArray[1], pointFArray[2]);
        circularArrowPath.AddLine(pointFArray[2], pointFArray[3]);
        circularArrowPath.AddLine(pointFArray[3], pointFArray[4]);
        circularArrowPath.AddLine(pointFArray[4], pointFArray[5]);
        circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th"] + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th"] + shapeFormula["th2"], sizeF2.Width * 2f, sizeF2.Height * 2f, shapeFormula["enAng"], (float) -((double) shapeFormula["enAng"] - (double) shapeFormula["stAng"]));
        circularArrowPath.CloseFigure();
        return circularArrowPath;
      }
      if ((double) shapeFormula["stAng"] % 45.0 == 0.0)
      {
        circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th2"], sizeF1.Width * 2f, sizeF1.Height * 2f, shapeFormula["stAng"], shapeFormula["swAng"]);
        pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
        pointFArray[2] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, pointFArray[1].Y - shapeFormula["p5"]);
        pointFArray[3] = new PointF(pointFArray[2].X - shapeFormula["dxH"], pointFArray[2].Y - shapeFormula["thh"]);
        pointFArray[4] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, pointFArray[2].Y - shapeFormula["dI"]);
        pointFArray[5] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, pointFArray[4].Y - shapeFormula["p5"]);
        circularArrowPath.AddLine(pointFArray[1], pointFArray[2]);
        circularArrowPath.AddLine(pointFArray[2], pointFArray[3]);
        circularArrowPath.AddLine(pointFArray[3], pointFArray[4]);
        circularArrowPath.AddLine(pointFArray[4], pointFArray[5]);
        circularArrowPath.AddArc(this._rectBounds.X + (sizeF1.Width - sizeF2.Width) + shapeFormula["th2"], this._rectBounds.Y + (sizeF1.Height - sizeF2.Height) + shapeFormula["th2"], sizeF2.Width * 2f, sizeF2.Height * 2f, shapeFormula["istAng"], shapeFormula["iswAng"]);
        circularArrowPath.CloseFigure();
        return circularArrowPath;
      }
      circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th2"], sizeF1.Width * 2f, sizeF1.Height * 2f, shapeFormula["stAng"], shapeFormula["enAng"] - shapeFormula["stAng"]);
      pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
      pointFArray[2] = new PointF(pointFArray[1].X + (shapeFormula["q16"] + shapeFormula["p1"]), pointFArray[1].Y + (shapeFormula["q18"] + shapeFormula["p2"]));
      pointFArray[3] = new PointF(pointFArray[2].X + shapeFormula["q9"] + shapeFormula["u12"], pointFArray[2].Y + shapeFormula["thh"] / 4f);
      pointFArray[4] = new PointF(pointFArray[3].X - shapeFormula["u12"], pointFArray[3].Y + shapeFormula["q9"] + shapeFormula["u0"]);
      pointFArray[5] = new PointF(pointFArray[4].X + (shapeFormula["q16"] + shapeFormula["p1"]), pointFArray[4].Y + (shapeFormula["q18"] + shapeFormula["p2"]));
      circularArrowPath.AddLine(pointFArray[1], pointFArray[2]);
      circularArrowPath.AddLine(pointFArray[2], pointFArray[3]);
      circularArrowPath.AddLine(pointFArray[3], pointFArray[4]);
      circularArrowPath.AddLine(pointFArray[4], pointFArray[5]);
      circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th"] + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th"] + shapeFormula["th2"], sizeF2.Width * 2f, sizeF2.Height * 2f, shapeFormula["enAng"], (float) -((double) shapeFormula["enAng"] - (double) shapeFormula["stAng"]));
      circularArrowPath.CloseFigure();
      return circularArrowPath;
    }
    if ((double) shapeFormula["stAng"] < 90.0)
    {
      if ((double) shapeFormula["stAng"] > 30.0 && (double) shapeFormula["stAng"] < 31.0)
      {
        circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th2"], sizeF1.Width * 2f, sizeF1.Height * 2f, shapeFormula["stAng"], shapeFormula["enAng"] - shapeFormula["stAng"]);
        pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
        pointFArray[2] = new PointF(pointFArray[1].X + (shapeFormula["q17"] + shapeFormula["p1"]), pointFArray[1].Y + (shapeFormula["q19"] + shapeFormula["p2"]));
        pointFArray[3] = new PointF(pointFArray[1].X - shapeFormula["u11"] / 2f, pointFArray[2].Y + (shapeFormula["q17"] + shapeFormula["u12"]));
        pointFArray[4] = new PointF(pointFArray[2].X - shapeFormula["q9"], pointFArray[3].Y + (shapeFormula["u12"] - shapeFormula["u0"]));
        pointFArray[5] = new PointF(pointFArray[4].X + (shapeFormula["q17"] + shapeFormula["p1"]), pointFArray[4].Y + (shapeFormula["q19"] + shapeFormula["p2"]));
        circularArrowPath.AddLine(pointFArray[1], pointFArray[2]);
        circularArrowPath.AddLine(pointFArray[2], pointFArray[3]);
        circularArrowPath.AddLine(pointFArray[3], pointFArray[4]);
        circularArrowPath.AddLine(pointFArray[4], pointFArray[5]);
        circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th"] + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th"] + shapeFormula["th2"], sizeF2.Width * 2f, sizeF2.Height * 2f, shapeFormula["swAng"] - shapeFormula["ptAng"], (float) -((double) shapeFormula["enAng"] - (double) shapeFormula["stAng"]));
        circularArrowPath.CloseFigure();
        return circularArrowPath;
      }
      circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th2"], sizeF1.Width * 2f, sizeF1.Height * 2f, shapeFormula["stAng"], shapeFormula["enAng"] - shapeFormula["stAng"]);
      if ((double) shapeFormula["enAng"] > 120.0)
      {
        pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
        pointFArray[2] = new PointF(pointFArray[1].X + (shapeFormula["q9"] - shapeFormula["q17"]), pointFArray[1].Y - (shapeFormula["v19"] - shapeFormula["p2"]));
        pointFArray[3] = new PointF(pointFArray[2].X - shapeFormula["u0"], pointFArray[2].Y + shapeFormula["v9"] - shapeFormula["u8"]);
        pointFArray[4] = new PointF(pointFArray[3].X - shapeFormula["v19"] + shapeFormula["u8"], (float) ((double) pointFArray[3].Y - (double) shapeFormula["p4"] + (double) shapeFormula["u7"] / 2.0));
        pointFArray[5] = new PointF(pointFArray[4].X + (shapeFormula["q9"] - shapeFormula["q17"]), pointFArray[4].Y - (shapeFormula["v19"] - shapeFormula["p2"]));
      }
      else if ((double) shapeFormula["stAng"] > 73.0 && (double) shapeFormula["stAng"] < 75.0)
      {
        pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
        pointFArray[2] = new PointF(pointFArray[1].X + (shapeFormula["q17"] + shapeFormula["p1"]), pointFArray[1].Y + (shapeFormula["q19"] + shapeFormula["p2"]));
        pointFArray[3] = new PointF(pointFArray[2].X - (shapeFormula["u11"] * 2f - shapeFormula["u0"]), pointFArray[2].Y - (shapeFormula["q19"] - shapeFormula["p1"]));
        pointFArray[4] = new PointF(pointFArray[2].X - shapeFormula["q9"], pointFArray[3].Y + (shapeFormula["q9"] - shapeFormula["p1"]));
        pointFArray[5] = new PointF(pointFArray[4].X + (shapeFormula["q17"] + shapeFormula["p1"]), pointFArray[4].Y + (shapeFormula["q19"] + shapeFormula["p2"]));
      }
      else
      {
        pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
        pointFArray[2] = new PointF(pointFArray[1].X - shapeFormula["thh"] / 4f, (float) ((double) pointFArray[1].Y + (double) shapeFormula["th2"] + (double) shapeFormula["u9"] * 2.0));
        pointFArray[3] = new PointF(pointFArray[2].X - shapeFormula["th"] / 4f, pointFArray[2].Y - shapeFormula["p3"] - shapeFormula["p1"]);
        pointFArray[4] = new PointF(pointFArray[3].X + shapeFormula["v17"] + shapeFormula["p1"], pointFArray[3].Y - shapeFormula["v17"]);
        pointFArray[5] = new PointF(pointFArray[4].X - shapeFormula["thh"] / 4f, (float) ((double) pointFArray[4].Y + (double) shapeFormula["th2"] + (double) shapeFormula["u9"] * 2.0));
      }
      circularArrowPath.AddLine(pointFArray[1], pointFArray[2]);
      circularArrowPath.AddLine(pointFArray[2], pointFArray[3]);
      circularArrowPath.AddLine(pointFArray[3], pointFArray[4]);
      circularArrowPath.AddLine(pointFArray[4], pointFArray[5]);
      circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th"] + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th"] + shapeFormula["th2"], sizeF2.Width * 2f, sizeF2.Height * 2f, shapeFormula["swAng"] - shapeFormula["ptAng"], (float) -((double) shapeFormula["enAng"] - (double) shapeFormula["stAng"]));
      circularArrowPath.CloseFigure();
      return circularArrowPath;
    }
    circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th2"], sizeF1.Width * 2f, sizeF1.Height * 2f, shapeFormula["stAng"], shapeFormula["enAng"] - shapeFormula["stAng"]);
    if ((double) shapeFormula["stAng"] > 90.0 && (double) shapeFormula["stAng"] < 115.0)
    {
      pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
      if ((double) shapeFormula["swAng"] > 210.0)
      {
        pointFArray[2] = new PointF(pointFArray[1].X + shapeFormula["p2"] - shapeFormula["u0"], pointFArray[1].Y - shapeFormula["p5"] / 2f);
        pointFArray[3] = new PointF(pointFArray[1].X + shapeFormula["u8"] / 2f, (float) ((double) pointFArray[2].Y - (double) shapeFormula["thh"] - (double) shapeFormula["u0"] * 2.0));
        pointFArray[4] = new PointF(pointFArray[3].X + shapeFormula["v17"], pointFArray[3].Y + shapeFormula["u0"] * 2f);
        pointFArray[5] = new PointF(pointFArray[4].X + shapeFormula["p2"] - shapeFormula["u0"], pointFArray[4].Y - shapeFormula["p5"] / 2f);
      }
      else
      {
        pointFArray[2] = new PointF(pointFArray[1].X + (shapeFormula["q17"] + shapeFormula["p1"] - shapeFormula["u8"]), pointFArray[1].Y + (float) ((double) shapeFormula["q19"] + (double) shapeFormula["p2"] + (double) shapeFormula["u9"] / 2.0));
        pointFArray[3] = new PointF(pointFArray[2].X + (float) ((double) shapeFormula["q21"] + (double) shapeFormula["q17"] + (double) shapeFormula["u0"] + (double) shapeFormula["u8"] / 10.0), pointFArray[2].Y + (float) ((double) shapeFormula["q17"] - (double) shapeFormula["u0"] - (double) shapeFormula["u11"] / 3.0));
        pointFArray[4] = new PointF(pointFArray[2].X - (float) ((double) shapeFormula["q9"] - (double) shapeFormula["u0"] - (double) shapeFormula["u7"] / 2.0), pointFArray[3].Y - (shapeFormula["u12"] - shapeFormula["u9"]));
        pointFArray[5] = new PointF(pointFArray[4].X - (shapeFormula["v17"] - shapeFormula["p1"] + shapeFormula["u8"]), pointFArray[4].Y - (float) ((double) shapeFormula["v19"] - (double) shapeFormula["p2"] - (double) shapeFormula["u9"] / 2.0));
      }
    }
    else if ((double) shapeFormula["stAng"] > 115.0)
    {
      pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
      pointFArray[2] = new PointF(pointFArray[1].X + shapeFormula["p5"], pointFArray[1].Y + shapeFormula["u9"] * 3f);
      pointFArray[3] = new PointF(pointFArray[1].X + shapeFormula["u10"] * 2f, pointFArray[2].Y - shapeFormula["th"] - shapeFormula["u8"]);
      pointFArray[4] = new PointF(pointFArray[2].X + shapeFormula["dI"], pointFArray[3].Y + shapeFormula["th2"]);
      pointFArray[5] = new PointF(pointFArray[4].X + shapeFormula["p5"], pointFArray[4].Y + shapeFormula["u9"] * 3f);
    }
    else
    {
      pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
      pointFArray[2] = new PointF(pointFArray[1].X + shapeFormula["p5"], circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
      pointFArray[3] = new PointF(pointFArray[2].X + shapeFormula["thh"], pointFArray[2].Y + shapeFormula["dyH"]);
      pointFArray[4] = new PointF(pointFArray[2].X + shapeFormula["dI"], circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
      pointFArray[5] = new PointF(pointFArray[4].X + shapeFormula["p5"], circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
    }
    circularArrowPath.AddLine(pointFArray[1], pointFArray[2]);
    circularArrowPath.AddLine(pointFArray[2], pointFArray[3]);
    circularArrowPath.AddLine(pointFArray[3], pointFArray[4]);
    circularArrowPath.AddLine(pointFArray[4], pointFArray[5]);
    circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th"] + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th"] + shapeFormula["th2"], sizeF2.Width * 2f, sizeF2.Height * 2f, shapeFormula["enAng"], (float) -((double) shapeFormula["enAng"] - (double) shapeFormula["stAng"]));
    circularArrowPath.CloseFigure();
    return circularArrowPath;
  }

  internal GraphicsPath GetLeftCircularArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftCircularArrow);
    GraphicsPath circularArrowPath = new GraphicsPath();
    PointF[] pointFArray = new PointF[6];
    SizeF sizeF1 = new SizeF(shapeFormula["rw2"], shapeFormula["rh2"]);
    SizeF sizeF2 = new SizeF(shapeFormula["rw1"], shapeFormula["rh1"]);
    if ((double) shapeFormula["istAng"] / 60000.0 == 315.0)
    {
      circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th2"], sizeF2.Width * 2f, sizeF2.Height * 2f, shapeFormula["istAng"] / 60000f, (float) ((double) shapeFormula["iswAng"] / 60000.0 + 105.0));
      pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
      pointFArray[2] = new PointF(pointFArray[1].X + (float) (((double) shapeFormula["q17"] + (double) shapeFormula["p5"]) / 2.0), pointFArray[1].Y - (float) ((double) shapeFormula["p2"] - (double) shapeFormula["q19"] - (double) shapeFormula["u12"] / 4.0));
      pointFArray[3] = new PointF(pointFArray[2].X - (float) ((double) shapeFormula["p2"] - (double) shapeFormula["v19"] - (double) shapeFormula["u13"] / 3.0), pointFArray[2].Y - (shapeFormula["p3"] - shapeFormula["v12"] / 60000f) - shapeFormula["p5"]);
      pointFArray[4] = new PointF((float) ((double) pointFArray[1].X + ((double) shapeFormula["u12"] + (double) shapeFormula["u13"]) + (double) shapeFormula["u13"] * 2.0), pointFArray[3].Y + (shapeFormula["q9"] - shapeFormula["q19"] * 2f) - shapeFormula["v19"]);
      pointFArray[5] = new PointF((float) ((double) shapeFormula["u13"] * 2.0 + (double) pointFArray[4].X + ((double) shapeFormula["q17"] + (double) shapeFormula["p5"]) / 2.0), (float) ((double) pointFArray[4].Y - ((double) shapeFormula["p2"] - (double) shapeFormula["q19"] - (double) shapeFormula["u12"] / 4.0) - 4.0));
      circularArrowPath.AddLine(pointFArray[1], pointFArray[2]);
      circularArrowPath.AddLine(pointFArray[2], pointFArray[3]);
      circularArrowPath.AddLine(pointFArray[3], pointFArray[4]);
      circularArrowPath.AddLine(pointFArray[4], pointFArray[5]);
      circularArrowPath.AddArc(this._rectBounds.X + (sizeF2.Width - sizeF1.Width) + shapeFormula["th2"], this._rectBounds.Y + (sizeF2.Width - sizeF1.Width) + shapeFormula["th2"], sizeF1.Width * 2f, sizeF1.Height * 2f, (float) ((double) shapeFormula["stAng0"] / 60000.0 + 105.0), (float) ((double) shapeFormula["swAng0"] / 60000.0 - 105.0));
      circularArrowPath.CloseFigure();
    }
    else if ((double) shapeFormula["istAng"] / 60000.0 > 360.0 && (double) shapeFormula["swAng0"] / 60000.0 > 225.0)
    {
      circularArrowPath.AddArc(this._rectBounds.X, this._rectBounds.Y, sizeF2.Width * 2f, sizeF2.Height * 2f, shapeFormula["istAng"] / 60000f, (float) ((double) shapeFormula["iswAng"] / 60000.0 + 165.0));
      pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
      pointFArray[2] = new PointF(pointFArray[1].X - (shapeFormula["q16"] - shapeFormula["u13"]), pointFArray[1].Y - (shapeFormula["q18"] + shapeFormula["u9"]));
      pointFArray[3] = new PointF(pointFArray[2].X + (float) ((double) shapeFormula["p4"] - (double) shapeFormula["p5"] - (double) shapeFormula["u11"] / 2.0), pointFArray[2].Y + (shapeFormula["q19"] - shapeFormula["u7"] / 4f));
      pointFArray[4] = new PointF(pointFArray[3].X + (shapeFormula["q19"] + shapeFormula["u9"] * 2f - shapeFormula["u11"]), pointFArray[3].Y - (shapeFormula["q20"] - shapeFormula["u11"]));
      pointFArray[5] = new PointF(pointFArray[4].X - (shapeFormula["q16"] - shapeFormula["u13"]), pointFArray[4].Y - (shapeFormula["q18"] + shapeFormula["u9"]));
      circularArrowPath.AddLine(pointFArray[1], pointFArray[2]);
      circularArrowPath.AddLine(pointFArray[2], pointFArray[3]);
      circularArrowPath.AddLine(pointFArray[3], pointFArray[4]);
      circularArrowPath.AddLine(pointFArray[4], pointFArray[5]);
      circularArrowPath.AddArc(this._rectBounds.X + (sizeF2.Width - sizeF1.Width), this._rectBounds.Y + (sizeF2.Width - sizeF1.Width), sizeF1.Width * 2f, sizeF1.Height * 2f, (float) ((double) shapeFormula["stAng0"] / 60000.0 + 165.0), (float) ((double) shapeFormula["swAng0"] / 60000.0 - 165.0));
      circularArrowPath.CloseFigure();
    }
    else
    {
      circularArrowPath.AddArc(this._rectBounds.X + shapeFormula["th2"], this._rectBounds.Y + shapeFormula["th2"], sizeF2.Width * 2f, sizeF2.Height * 2f, shapeFormula["istAng"] / 60000f, (float) ((double) shapeFormula["iswAng"] / 60000.0 + 30.0));
      pointFArray[1] = new PointF(circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].X, circularArrowPath.PathPoints[circularArrowPath.PathPoints.Length - 1].Y);
      pointFArray[2] = new PointF((float) ((double) shapeFormula["q17"] * 2.0 + (double) pointFArray[1].X - ((double) shapeFormula["q17"] + (double) shapeFormula["p2"] / 2.0)), pointFArray[1].Y + (float) ((double) shapeFormula["p1"] - (double) shapeFormula["q21"] + (double) shapeFormula["q19"] / 8.0));
      pointFArray[3] = new PointF((float) (-((double) shapeFormula["q17"] * 3.0 - (double) shapeFormula["p2"]) + (double) pointFArray[2].X - ((double) shapeFormula["q9"] - (double) shapeFormula["q19"] * 4.0)), (float) ((double) shapeFormula["u12"] + (double) pointFArray[2].Y - ((double) shapeFormula["q21"] - (double) shapeFormula["p2"] - (double) shapeFormula["q19"] / 8.0)));
      pointFArray[4] = new PointF((float) (-((double) shapeFormula["p1"] + (double) shapeFormula["q21"]) + (double) pointFArray[3].X - ((double) shapeFormula["q19"] + (double) shapeFormula["p2"] / 6.0)), (float) ((double) shapeFormula["q17"] * 3.0 - (double) shapeFormula["p2"] + (double) pointFArray[3].Y - (double) shapeFormula["q21"] * 3.0));
      pointFArray[5] = new PointF(pointFArray[4].X - (shapeFormula["q17"] + shapeFormula["p2"] / 2f), pointFArray[4].Y + (float) ((double) shapeFormula["p1"] - (double) shapeFormula["q21"] + (double) shapeFormula["q19"] / 8.0));
      circularArrowPath.AddLine(pointFArray[1], pointFArray[2]);
      circularArrowPath.AddLine(pointFArray[2], pointFArray[3]);
      circularArrowPath.AddLine(pointFArray[3], pointFArray[4]);
      circularArrowPath.AddLine(pointFArray[4], pointFArray[5]);
      circularArrowPath.AddArc(this._rectBounds.X + (sizeF2.Width - sizeF1.Width) + shapeFormula["th2"], this._rectBounds.Y + (sizeF2.Width - sizeF1.Width) + shapeFormula["th2"], sizeF1.Width * 2f, sizeF1.Height * 2f, (float) ((double) shapeFormula["stAng0"] / 60000.0 + 30.0), (float) ((double) shapeFormula["swAng0"] / 60000.0 - 30.0));
      circularArrowPath.CloseFigure();
    }
    return circularArrowPath;
  }

  internal GraphicsPath GetMathPlusPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.MathPlus);
    GraphicsPath mathPlusPath = new GraphicsPath();
    PointF[] points = new PointF[12]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"])
    };
    mathPlusPath.AddLines(points);
    mathPlusPath.CloseFigure();
    return mathPlusPath;
  }

  internal GraphicsPath GetMathMinusPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.MathMinus);
    GraphicsPath mathMinusPath = new GraphicsPath();
    PointF[] points = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"])
    };
    mathMinusPath.AddLines(points);
    mathMinusPath.CloseFigure();
    return mathMinusPath;
  }

  internal GraphicsPath GetMathMultiplyPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.MathMultiply);
    GraphicsPath mathMultiplyPath = new GraphicsPath();
    PointF[] points = new PointF[12]
    {
      new PointF(this._rectBounds.X + shapeFormula["xA"], this._rectBounds.Y + shapeFormula["yA"]),
      new PointF(this._rectBounds.X + shapeFormula["xB"], this._rectBounds.Y + shapeFormula["yB"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["yC"]),
      new PointF(this._rectBounds.X + shapeFormula["xD"], this._rectBounds.Y + shapeFormula["yB"]),
      new PointF(this._rectBounds.X + shapeFormula["xE"], this._rectBounds.Y + shapeFormula["yA"]),
      new PointF(this._rectBounds.X + shapeFormula["xF"], this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["xE"], this._rectBounds.Y + shapeFormula["yG"]),
      new PointF(this._rectBounds.X + shapeFormula["xD"], this._rectBounds.Y + shapeFormula["yH"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["yI"]),
      new PointF(this._rectBounds.X + shapeFormula["xB"], this._rectBounds.Y + shapeFormula["yH"]),
      new PointF(this._rectBounds.X + shapeFormula["xA"], this._rectBounds.Y + shapeFormula["yG"]),
      new PointF(this._rectBounds.X + shapeFormula["xL"], this._rectBounds.Y + this._rectBounds.Height / 2f)
    };
    mathMultiplyPath.AddLines(points);
    mathMultiplyPath.CloseFigure();
    return mathMultiplyPath;
  }

  internal GraphicsPath GetMathDivisionPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.MathDivision);
    GraphicsPath mathDivisionPath = new GraphicsPath();
    PointF[] points = new PointF[4];
    mathDivisionPath.AddEllipse(this._rectBounds.X + this._rectBounds.Width / 2f - shapeFormula["rad"], this._rectBounds.Y + shapeFormula["y1"], shapeFormula["rad"] * 2f, shapeFormula["rad"] * 2f);
    mathDivisionPath.CloseFigure();
    mathDivisionPath.AddEllipse(this._rectBounds.X + this._rectBounds.Width / 2f - shapeFormula["rad"], (float) ((double) this._rectBounds.Y + (double) shapeFormula["y5"] - (double) shapeFormula["rad"] * 2.0), shapeFormula["rad"] * 2f, shapeFormula["rad"] * 2f);
    mathDivisionPath.CloseFigure();
    points[0] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"]);
    points[1] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]);
    points[2] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y4"]);
    points[3] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"]);
    mathDivisionPath.AddLines(points);
    mathDivisionPath.CloseFigure();
    return mathDivisionPath;
  }

  internal GraphicsPath GetMathEqualPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.MathEqual);
    GraphicsPath mathEqualPath = new GraphicsPath();
    PointF[] points1 = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"])
    };
    mathEqualPath.AddLines(points1);
    mathEqualPath.CloseFigure();
    PointF[] points2 = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"])
    };
    mathEqualPath.AddLines(points2);
    mathEqualPath.CloseFigure();
    return mathEqualPath;
  }

  internal GraphicsPath GetMathNotEqualPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.MathNotEqual);
    GraphicsPath mathNotEqualPath = new GraphicsPath();
    PointF[] points = new PointF[20]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["lx"], this._rectBounds.Y + shapeFormula["ly"]),
      new PointF(this._rectBounds.X + shapeFormula["rx"], this._rectBounds.Y + shapeFormula["ry"]),
      new PointF(this._rectBounds.X + shapeFormula["rx6"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["rx5"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["rx4"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["rx3"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["drx"], this._rectBounds.Y + shapeFormula["dry"]),
      new PointF(this._rectBounds.X + shapeFormula["dlx"], this._rectBounds.Y + shapeFormula["dly"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"])
    };
    mathNotEqualPath.AddLines(points);
    mathNotEqualPath.CloseFigure();
    return mathNotEqualPath;
  }

  internal GraphicsPath GetFlowChartAlternateProcessPath()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.FlowChartAlternateProcess);
    GraphicsPath alternateProcessPath = new GraphicsPath();
    float num = this._formulaValues.GetPresetOperandValue("ssd6") * 2f;
    alternateProcessPath.AddArc(this._rectBounds.X, this._rectBounds.Y, num, num, 180f, 90f);
    alternateProcessPath.AddArc(this._rectBounds.Right - num, this._rectBounds.Y, num, num, 270f, 90f);
    alternateProcessPath.AddArc(this._rectBounds.Right - num, this._rectBounds.Bottom - num, num, num, 0.0f, 90f);
    alternateProcessPath.AddArc(this._rectBounds.X, this._rectBounds.Bottom - num, num, num, 90f, 90f);
    alternateProcessPath.CloseFigure();
    return alternateProcessPath;
  }

  internal GraphicsPath GetFlowChartPredefinedProcessPath()
  {
    GraphicsPath predefinedProcessPath = new GraphicsPath();
    predefinedProcessPath.AddRectangle(this._rectBounds);
    predefinedProcessPath.AddLine(this._rectBounds.X + this._rectBounds.Width / 8f, this._rectBounds.Y, this._rectBounds.X + this._rectBounds.Width / 8f, this._rectBounds.Bottom);
    predefinedProcessPath.CloseFigure();
    predefinedProcessPath.AddLine(this._rectBounds.Right - this._rectBounds.Width / 8f, this._rectBounds.Y, this._rectBounds.Right - this._rectBounds.Width / 8f, this._rectBounds.Bottom);
    predefinedProcessPath.CloseFigure();
    return predefinedProcessPath;
  }

  internal GraphicsPath GetFlowChartInternalStoragePath()
  {
    GraphicsPath internalStoragePath = new GraphicsPath();
    internalStoragePath.AddRectangle(this._rectBounds);
    internalStoragePath.AddLine(this._rectBounds.X + this._rectBounds.Width / 8f, this._rectBounds.Y, this._rectBounds.X + this._rectBounds.Width / 8f, this._rectBounds.Bottom);
    internalStoragePath.CloseFigure();
    internalStoragePath.AddLine(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 8f, this._rectBounds.Right, this._rectBounds.Top + this._rectBounds.Height / 8f);
    internalStoragePath.CloseFigure();
    return internalStoragePath;
  }

  internal GraphicsPath GetFlowChartDocumentPath()
  {
    GraphicsPath chartDocumentPath = new GraphicsPath();
    chartDocumentPath.AddLine(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Right, this._rectBounds.Y);
    chartDocumentPath.AddLine(this._rectBounds.Right, this._rectBounds.Y, this._rectBounds.Right, this._rectBounds.Y + (float) ((double) this._rectBounds.Height * 17322.0 / 21600.0));
    PointF xyPosition1 = this.GetXYPosition(21600f, 17322f, 21600f);
    PointF xyPosition2 = this.GetXYPosition(10800f, 17322f, 21600f);
    PointF xyPosition3 = this.GetXYPosition(10800f, 23922f, 21600f);
    PointF xyPosition4 = this.GetXYPosition(0.0f, 20172f, 21600f);
    chartDocumentPath.AddBezier(xyPosition1, xyPosition2, xyPosition3, xyPosition4);
    chartDocumentPath.CloseFigure();
    return chartDocumentPath;
  }

  internal GraphicsPath GetFlowChartMultiDocumentPath()
  {
    GraphicsPath multiDocumentPath = new GraphicsPath();
    multiDocumentPath.AddLine(this.GetXYPosition(0.0f, 3675f, 21600f), this.GetXYPosition(18595f, 3675f, 21600f));
    multiDocumentPath.AddLine(this.GetXYPosition(18595f, 3675f, 21600f), this.GetXYPosition(18595f, 18022f, 21600f));
    multiDocumentPath.AddBezier(this.GetXYPosition(18595f, 18022f, 21600f), this.GetXYPosition(9298f, 18022f, 21600f), this.GetXYPosition(9298f, 23542f, 21600f), this.GetXYPosition(0.0f, 20782f, 21600f));
    multiDocumentPath.CloseFigure();
    multiDocumentPath.AddLine(this.GetXYPosition(1532f, 3675f, 21600f), this.GetXYPosition(1532f, 1815f, 21600f));
    multiDocumentPath.AddLine(this.GetXYPosition(1532f, 1815f, 21600f), this.GetXYPosition(20000f, 1815f, 21600f));
    multiDocumentPath.AddLine(this.GetXYPosition(20000f, 1815f, 21600f), this.GetXYPosition(20000f, 16252f, 21600f));
    multiDocumentPath.AddBezier(this.GetXYPosition(20000f, 16252f, 21600f), this.GetXYPosition(19298f, 16252f, 21600f), this.GetXYPosition(18595f, 16352f, 21600f), this.GetXYPosition(18595f, 16352f, 21600f));
    multiDocumentPath.StartFigure();
    multiDocumentPath.AddLine(this.GetXYPosition(2972f, 1815f, 21600f), this.GetXYPosition(2972f, 0.0f, 21600f));
    multiDocumentPath.AddLine(this.GetXYPosition(2972f, 0.0f, 21600f), this.GetXYPosition(21600f, 0.0f, 21600f));
    multiDocumentPath.AddLine(this.GetXYPosition(21600f, 0.0f, 21600f), this.GetXYPosition(21600f, 14392f, 21600f));
    multiDocumentPath.AddBezier(this.GetXYPosition(21600f, 14392f, 21600f), this.GetXYPosition(20800f, 14392f, 21600f), this.GetXYPosition(20000f, 14467f, 21600f), this.GetXYPosition(20000f, 14467f, 21600f));
    return multiDocumentPath;
  }

  internal GraphicsPath GetFlowChartTerminatorPath()
  {
    GraphicsPath chartTerminatorPath = new GraphicsPath();
    SizeF sizeF = new SizeF((float) ((double) this._rectBounds.Width * 3475.0 / 21600.0 * 2.0), (float) ((double) this._rectBounds.Height * 10800.0 / 21600.0 * 2.0));
    chartTerminatorPath.AddLine(this.GetXYPosition(3475f, 0.0f, 21600f), this.GetXYPosition(18125f, 0.0f, 21600f));
    chartTerminatorPath.StartFigure();
    PointF xyPosition1 = this.GetXYPosition(18125f, 0.0f, 21600f);
    RectangleF rect = new RectangleF(xyPosition1.X - sizeF.Width / 2f, xyPosition1.Y, sizeF.Width, sizeF.Height);
    chartTerminatorPath.AddArc(rect, 270f, 180f);
    chartTerminatorPath.AddLine(new PointF(rect.X, rect.Y + rect.Height), this.GetXYPosition(3475f, 21600f, 21600f));
    PointF xyPosition2 = this.GetXYPosition(3475f, 0.0f, 21600f);
    rect = new RectangleF(xyPosition2.X - sizeF.Width / 2f, xyPosition2.Y, sizeF.Width, sizeF.Height);
    chartTerminatorPath.AddArc(rect, 90f, 180f);
    chartTerminatorPath.CloseFigure();
    return chartTerminatorPath;
  }

  internal GraphicsPath GetFlowChartPreparationPath()
  {
    GraphicsPath chartPreparationPath = new GraphicsPath();
    chartPreparationPath.AddLine(this.GetXYPosition(0.0f, 5f, 10f), this.GetXYPosition(2f, 0.0f, 10f));
    chartPreparationPath.AddLine(this.GetXYPosition(2f, 0.0f, 10f), this.GetXYPosition(8f, 0.0f, 10f));
    chartPreparationPath.AddLine(this.GetXYPosition(8f, 0.0f, 10f), this.GetXYPosition(10f, 5f, 10f));
    chartPreparationPath.AddLine(this.GetXYPosition(10f, 5f, 10f), this.GetXYPosition(8f, 10f, 10f));
    chartPreparationPath.AddLine(this.GetXYPosition(8f, 10f, 10f), this.GetXYPosition(2f, 10f, 10f));
    chartPreparationPath.CloseFigure();
    return chartPreparationPath;
  }

  internal GraphicsPath GetFlowChartManualInputPath()
  {
    GraphicsPath chartManualInputPath = new GraphicsPath();
    chartManualInputPath.AddLine(this.GetXYPosition(0.0f, 1f, 5f), this.GetXYPosition(5f, 0.0f, 5f));
    chartManualInputPath.AddLine(this.GetXYPosition(5f, 0.0f, 5f), this.GetXYPosition(5f, 5f, 5f));
    chartManualInputPath.AddLine(this.GetXYPosition(5f, 5f, 5f), this.GetXYPosition(0.0f, 5f, 5f));
    chartManualInputPath.CloseFigure();
    return chartManualInputPath;
  }

  internal GraphicsPath GetFlowChartManualOperationPath()
  {
    GraphicsPath manualOperationPath = new GraphicsPath();
    manualOperationPath.AddLine(this.GetXYPosition(0.0f, 0.0f, 5f), this.GetXYPosition(5f, 0.0f, 5f));
    manualOperationPath.AddLine(this.GetXYPosition(5f, 0.0f, 5f), this.GetXYPosition(4f, 5f, 5f));
    manualOperationPath.AddLine(this.GetXYPosition(4f, 5f, 5f), this.GetXYPosition(1f, 5f, 5f));
    manualOperationPath.CloseFigure();
    return manualOperationPath;
  }

  internal GraphicsPath GetFlowChartConnectorPath()
  {
    GraphicsPath chartConnectorPath = new GraphicsPath();
    RectangleF rect = new RectangleF(new PointF(this._rectBounds.X, this._rectBounds.Y), new SizeF(this._rectBounds.Width, this._rectBounds.Height));
    chartConnectorPath.AddArc(rect, 180f, 90f);
    chartConnectorPath.AddArc(rect, 270f, 90f);
    chartConnectorPath.AddArc(rect, 0.0f, 90f);
    chartConnectorPath.AddArc(rect, 90f, 90f);
    return chartConnectorPath;
  }

  internal GraphicsPath GetFlowChartOffPageConnectorPath()
  {
    GraphicsPath pageConnectorPath = new GraphicsPath();
    pageConnectorPath.AddLine(this.GetXYPosition(0.0f, 0.0f, 10f), this.GetXYPosition(10f, 0.0f, 10f));
    pageConnectorPath.AddLine(this.GetXYPosition(10f, 0.0f, 10f), this.GetXYPosition(10f, 8f, 10f));
    pageConnectorPath.AddLine(this.GetXYPosition(10f, 8f, 10f), this.GetXYPosition(5f, 10f, 10f));
    pageConnectorPath.AddLine(this.GetXYPosition(5f, 10f, 10f), this.GetXYPosition(0.0f, 8f, 10f));
    pageConnectorPath.CloseFigure();
    return pageConnectorPath;
  }

  internal GraphicsPath GetFlowChartCardPath()
  {
    GraphicsPath flowChartCardPath = new GraphicsPath();
    flowChartCardPath.AddLine(this.GetXYPosition(0.0f, 1f, 5f), this.GetXYPosition(1f, 0.0f, 5f));
    flowChartCardPath.AddLine(this.GetXYPosition(1f, 0.0f, 5f), this.GetXYPosition(5f, 0.0f, 5f));
    flowChartCardPath.AddLine(this.GetXYPosition(5f, 0.0f, 5f), this.GetXYPosition(5f, 5f, 5f));
    flowChartCardPath.AddLine(this.GetXYPosition(5f, 5f, 5f), this.GetXYPosition(0.0f, 5f, 5f));
    flowChartCardPath.CloseFigure();
    return flowChartCardPath;
  }

  internal GraphicsPath GetFlowChartPunchedTapePath()
  {
    GraphicsPath chartPunchedTapePath = new GraphicsPath();
    RectangleF rect = new RectangleF(this.GetXYPosition(0.0f, 2f, 20f), new SizeF((float) ((double) this._rectBounds.Width * 5.0 / 20.0 * 2.0), (float) ((double) this._rectBounds.Height * 2.0 / 20.0 * 2.0)));
    chartPunchedTapePath.AddArc(rect, 180f, -180f);
    PointF location = new PointF(rect.X + rect.Width, rect.Y);
    rect = new RectangleF(location, new SizeF((float) ((double) this._rectBounds.Width * 5.0 / 20.0 * 2.0), (float) ((double) this._rectBounds.Height * 2.0 / 20.0 * 2.0)));
    chartPunchedTapePath.AddArc(rect, 180f, 180f);
    chartPunchedTapePath.AddLine(new PointF(rect.X + rect.Width, rect.Y + rect.Height), this.GetXYPosition(20f, 18f, 20f));
    rect = new RectangleF(new PointF(this.GetXYPosition(20f, 18f, 20f).X - rect.Width, this.GetXYPosition(20f, 18f, 20f).Y), new SizeF((float) ((double) this._rectBounds.Width * 5.0 / 20.0 * 2.0), (float) ((double) this._rectBounds.Height * 2.0 / 20.0 * 2.0)));
    chartPunchedTapePath.AddArc(rect, 0.0f, -180f);
    location = new PointF(rect.X - rect.Width, rect.Y);
    rect = new RectangleF(location, new SizeF((float) ((double) this._rectBounds.Width * 5.0 / 20.0 * 2.0), (float) ((double) this._rectBounds.Height * 2.0 / 20.0 * 2.0)));
    chartPunchedTapePath.AddArc(rect, 0.0f, 180f);
    chartPunchedTapePath.CloseFigure();
    return chartPunchedTapePath;
  }

  internal GraphicsPath GetFlowChartSummingJunctionPath()
  {
    GraphicsPath summingJunctionPath = new GraphicsPath();
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.FlowChartSummingJunction);
    summingJunctionPath.AddLine(new PointF(this._rectBounds.X + shapeFormula["il"], this._rectBounds.Y + shapeFormula["it"]), new PointF(this._rectBounds.X + shapeFormula["ir"], this._rectBounds.Y + shapeFormula["ib"]));
    summingJunctionPath.StartFigure();
    summingJunctionPath.AddLine(new PointF(this._rectBounds.X + shapeFormula["ir"], this._rectBounds.Y + shapeFormula["it"]), new PointF(this._rectBounds.X + shapeFormula["il"], this._rectBounds.Y + shapeFormula["ib"]));
    summingJunctionPath.StartFigure();
    summingJunctionPath.AddArc(this._rectBounds, 180f, 90f);
    summingJunctionPath.AddArc(this._rectBounds, 270f, 90f);
    summingJunctionPath.AddArc(this._rectBounds, 0.0f, 90f);
    summingJunctionPath.AddArc(this._rectBounds, 90f, 90f);
    summingJunctionPath.CloseFigure();
    return summingJunctionPath;
  }

  internal GraphicsPath GetFlowChartOrPath()
  {
    GraphicsPath flowChartOrPath = new GraphicsPath();
    this._formulaValues.ParseShapeFormula(AutoShapeType.FlowChartOr);
    flowChartOrPath.AddLine(new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y), new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom));
    flowChartOrPath.StartFigure();
    flowChartOrPath.AddLine(new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f), new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f));
    flowChartOrPath.StartFigure();
    flowChartOrPath.AddArc(this._rectBounds, 180f, 90f);
    flowChartOrPath.AddArc(this._rectBounds, 270f, 90f);
    flowChartOrPath.AddArc(this._rectBounds, 0.0f, 90f);
    flowChartOrPath.AddArc(this._rectBounds, 90f, 90f);
    flowChartOrPath.CloseFigure();
    return flowChartOrPath;
  }

  internal GraphicsPath GetFlowChartCollatePath()
  {
    GraphicsPath chartCollatePath = new GraphicsPath();
    chartCollatePath.AddLine(this.GetXYPosition(0.0f, 0.0f, 2f), this.GetXYPosition(2f, 0.0f, 2f));
    chartCollatePath.AddLine(this.GetXYPosition(2f, 0.0f, 2f), this.GetXYPosition(1f, 1f, 2f));
    chartCollatePath.AddLine(this.GetXYPosition(1f, 1f, 2f), this.GetXYPosition(2f, 2f, 2f));
    chartCollatePath.AddLine(this.GetXYPosition(2f, 2f, 2f), this.GetXYPosition(0.0f, 2f, 2f));
    chartCollatePath.AddLine(this.GetXYPosition(0.0f, 2f, 2f), this.GetXYPosition(1f, 1f, 2f));
    chartCollatePath.CloseFigure();
    return chartCollatePath;
  }

  internal GraphicsPath GetFlowChartSortPath()
  {
    GraphicsPath flowChartSortPath = new GraphicsPath();
    flowChartSortPath.AddLine(this.GetXYPosition(0.0f, 1f, 2f), this.GetXYPosition(2f, 1f, 2f));
    flowChartSortPath.AddLine(this.GetXYPosition(2f, 1f, 2f), this.GetXYPosition(0.0f, 1f, 2f));
    flowChartSortPath.AddLine(this.GetXYPosition(0.0f, 1f, 2f), this.GetXYPosition(1f, 0.0f, 2f));
    flowChartSortPath.AddLine(this.GetXYPosition(1f, 0.0f, 2f), this.GetXYPosition(2f, 1f, 2f));
    flowChartSortPath.AddLine(this.GetXYPosition(2f, 1f, 2f), this.GetXYPosition(1f, 2f, 2f));
    flowChartSortPath.CloseFigure();
    return flowChartSortPath;
  }

  internal GraphicsPath GetFlowChartExtractPath()
  {
    GraphicsPath chartExtractPath = new GraphicsPath();
    chartExtractPath.AddLine(this.GetXYPosition(0.0f, 2f, 2f), this.GetXYPosition(1f, 0.0f, 2f));
    chartExtractPath.AddLine(this.GetXYPosition(1f, 0.0f, 2f), this.GetXYPosition(2f, 2f, 2f));
    chartExtractPath.CloseFigure();
    return chartExtractPath;
  }

  internal GraphicsPath GetFlowChartMergePath()
  {
    GraphicsPath flowChartMergePath = new GraphicsPath();
    flowChartMergePath.AddLine(this.GetXYPosition(0.0f, 0.0f, 2f), this.GetXYPosition(2f, 0.0f, 2f));
    flowChartMergePath.AddLine(this.GetXYPosition(2f, 0.0f, 2f), this.GetXYPosition(1f, 2f, 2f));
    flowChartMergePath.CloseFigure();
    return flowChartMergePath;
  }

  internal GraphicsPath GetFlowChartOnlineStoragePath()
  {
    GraphicsPath onlineStoragePath = new GraphicsPath();
    onlineStoragePath.AddLine(this.GetXYPosition(1f, 0.0f, 6f), this.GetXYPosition(6f, 0.0f, 6f));
    SizeF sizeF = new SizeF((float) ((double) this._rectBounds.Width / 6.0 * 2.0), (float) ((double) this._rectBounds.Height / 2.0 * 2.0));
    PointF xyPosition1 = this.GetXYPosition(6f, 0.0f, 6f);
    RectangleF rect = new RectangleF(xyPosition1.X - sizeF.Width / 2f, xyPosition1.Y, sizeF.Width, sizeF.Height);
    onlineStoragePath.AddArc(rect, 270f, -180f);
    onlineStoragePath.AddLine(new PointF(xyPosition1.X, xyPosition1.Y + sizeF.Height), this.GetXYPosition(1f, 6f, 6f));
    PointF xyPosition2 = this.GetXYPosition(1f, 0.0f, 6f);
    rect = new RectangleF(xyPosition2.X - sizeF.Width / 2f, xyPosition2.Y, sizeF.Width, sizeF.Height);
    onlineStoragePath.AddArc(rect, 90f, 180f);
    onlineStoragePath.CloseFigure();
    return onlineStoragePath;
  }

  internal GraphicsPath GetFlowChartDelayPath()
  {
    GraphicsPath flowChartDelayPath = new GraphicsPath();
    flowChartDelayPath.AddLine(new PointF(this._rectBounds.X, this._rectBounds.Y), new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y));
    SizeF sizeF = new SizeF(this._rectBounds.Width, this._rectBounds.Height);
    PointF pointF = new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y);
    RectangleF rect = new RectangleF(pointF.X - sizeF.Width / 2f, pointF.Y, sizeF.Width, sizeF.Height);
    flowChartDelayPath.AddArc(rect, 270f, 180f);
    flowChartDelayPath.AddLine(new PointF(pointF.X, pointF.Y + sizeF.Height), new PointF(this._rectBounds.X, this._rectBounds.Bottom));
    flowChartDelayPath.CloseFigure();
    return flowChartDelayPath;
  }

  internal GraphicsPath GetFlowChartSequentialAccessStoragePath()
  {
    GraphicsPath accessStoragePath = new GraphicsPath();
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.FlowChartSequentialAccessStorage);
    SizeF sizeF = new SizeF(this._rectBounds.Width, this._rectBounds.Height);
    RectangleF rect = new RectangleF(this._rectBounds.X, this._rectBounds.Y, sizeF.Width, sizeF.Height);
    accessStoragePath.AddArc(rect, 90f, 90f);
    accessStoragePath.AddArc(rect, 180f, 90f);
    accessStoragePath.AddArc(rect, 270f, 90f);
    accessStoragePath.AddArc(rect, 0.0f, shapeFormula["ang1"]);
    accessStoragePath.AddLine(new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["ib"]), new PointF(this._rectBounds.Right, this._rectBounds.Bottom));
    accessStoragePath.CloseFigure();
    return accessStoragePath;
  }

  internal GraphicsPath GetFlowChartMagneticDiskPath()
  {
    GraphicsPath magneticDiskPath = new GraphicsPath();
    SizeF sizeF = new SizeF(this._rectBounds.Width, this._rectBounds.Height / 3f);
    PointF xyPosition1 = this.GetXYPosition(6f, 1f, 6f);
    RectangleF rect = new RectangleF(xyPosition1.X - sizeF.Width, xyPosition1.Y - sizeF.Height / 2f, sizeF.Width, sizeF.Height);
    magneticDiskPath.AddArc(rect, 0.0f, 180f);
    magneticDiskPath.StartFigure();
    PointF xyPosition2 = this.GetXYPosition(0.0f, 1f, 6f);
    rect = new RectangleF(xyPosition2.X, xyPosition2.Y - sizeF.Height / 2f, sizeF.Width, sizeF.Height);
    magneticDiskPath.AddArc(rect, 180f, 180f);
    magneticDiskPath.AddLine(new PointF(rect.X + rect.Width, rect.Y + sizeF.Height), this.GetXYPosition(6f, 5f, 6f));
    PointF xyPosition3 = this.GetXYPosition(6f, 5f, 6f);
    rect = new RectangleF(xyPosition3.X - sizeF.Width, xyPosition3.Y - sizeF.Height / 2f, sizeF.Width, sizeF.Height);
    magneticDiskPath.AddArc(rect, 0.0f, 180f);
    magneticDiskPath.CloseFigure();
    return magneticDiskPath;
  }

  internal GraphicsPath GetFlowChartDirectAccessStoragePath()
  {
    GraphicsPath accessStoragePath = new GraphicsPath();
    SizeF sizeF = new SizeF(this._rectBounds.Width / 3f, this._rectBounds.Height);
    PointF xyPosition1 = this.GetXYPosition(5f, 6f, 6f);
    RectangleF rect = new RectangleF(xyPosition1.X - sizeF.Width / 2f, xyPosition1.Y - sizeF.Height, sizeF.Width, sizeF.Height);
    accessStoragePath.AddArc(rect, 90f, 180f);
    accessStoragePath.StartFigure();
    accessStoragePath.AddLine(this.GetXYPosition(1f, 0.0f, 6f), this.GetXYPosition(5f, 0.0f, 6f));
    PointF xyPosition2 = this.GetXYPosition(5f, 0.0f, 6f);
    rect = new RectangleF(xyPosition2.X - sizeF.Width / 2f, xyPosition2.Y, sizeF.Width, sizeF.Height);
    accessStoragePath.AddArc(rect, 270f, 180f);
    accessStoragePath.AddLine(new PointF(rect.X, rect.Y + sizeF.Height), this.GetXYPosition(1f, 6f, 6f));
    PointF xyPosition3 = this.GetXYPosition(1f, 6f, 6f);
    rect = new RectangleF(xyPosition3.X - sizeF.Width / 2f, xyPosition3.Y - sizeF.Height, sizeF.Width, sizeF.Height);
    accessStoragePath.StartFigure();
    accessStoragePath.AddArc(rect, 90f, 180f);
    return accessStoragePath;
  }

  internal GraphicsPath GetFlowChartDisplayPath()
  {
    GraphicsPath chartDisplayPath = new GraphicsPath();
    chartDisplayPath.AddLine(this.GetXYPosition(0.0f, 3f, 6f), this.GetXYPosition(1f, 0.0f, 6f));
    chartDisplayPath.AddLine(this.GetXYPosition(1f, 0.0f, 6f), this.GetXYPosition(5f, 0.0f, 6f));
    chartDisplayPath.AddArc(this.GetXYPosition(5f, 0.0f, 6f).X - this._rectBounds.Width / 6f, this.GetXYPosition(5f, 0.0f, 6f).Y, this._rectBounds.Width / 3f, this._rectBounds.Height, 270f, 180f);
    chartDisplayPath.AddLine(new PointF(this.GetXYPosition(5f, 0.0f, 6f).X - this._rectBounds.Width / 6f, this.GetXYPosition(5f, 0.0f, 6f).Y + this._rectBounds.Height), this.GetXYPosition(1f, 6f, 6f));
    chartDisplayPath.CloseFigure();
    return chartDisplayPath;
  }

  internal GraphicsPath GetExplosion1()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.Explosion1);
    GraphicsPath explosion1 = new GraphicsPath();
    PointF[] points = new PointF[24]
    {
      this.GetXYPosition(10800f, 5800f, 21600f),
      this.GetXYPosition(14522f, 0.0f, 21600f),
      this.GetXYPosition(14155f, 5325f, 21600f),
      this.GetXYPosition(18380f, 4457f, 21600f),
      this.GetXYPosition(16702f, 7315f, 21600f),
      this.GetXYPosition(21097f, 8137f, 21600f),
      this.GetXYPosition(17607f, 10475f, 21600f),
      this.GetXYPosition(21600f, 13290f, 21600f),
      this.GetXYPosition(16837f, 12942f, 21600f),
      this.GetXYPosition(18145f, 18095f, 21600f),
      this.GetXYPosition(14020f, 14457f, 21600f),
      this.GetXYPosition(13247f, 19737f, 21600f),
      this.GetXYPosition(10532f, 14935f, 21600f),
      this.GetXYPosition(8485f, 21600f, 21600f),
      this.GetXYPosition(7715f, 15627f, 21600f),
      this.GetXYPosition(4762f, 17617f, 21600f),
      this.GetXYPosition(5667f, 13937f, 21600f),
      this.GetXYPosition(135f, 14587f, 21600f),
      this.GetXYPosition(3722f, 11775f, 21600f),
      this.GetXYPosition(0.0f, 8615f, 21600f),
      this.GetXYPosition(4627f, 7617f, 21600f),
      this.GetXYPosition(370f, 2295f, 21600f),
      this.GetXYPosition(7312f, 6320f, 21600f),
      this.GetXYPosition(8352f, 2295f, 21600f)
    };
    explosion1.AddLines(points);
    explosion1.CloseFigure();
    return explosion1;
  }

  internal GraphicsPath GetExplosion2()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.Explosion2);
    GraphicsPath explosion2 = new GraphicsPath();
    PointF[] points = new PointF[28]
    {
      this.GetXYPosition(11462f, 4342f, 21600f),
      this.GetXYPosition(14790f, 0.0f, 21600f),
      this.GetXYPosition(14525f, 5777f, 21600f),
      this.GetXYPosition(18007f, 3172f, 21600f),
      this.GetXYPosition(16380f, 6532f, 21600f),
      this.GetXYPosition(21600f, 6645f, 21600f),
      this.GetXYPosition(16985f, 9402f, 21600f),
      this.GetXYPosition(18270f, 11290f, 21600f),
      this.GetXYPosition(16380f, 12310f, 21600f),
      this.GetXYPosition(18877f, 15632f, 21600f),
      this.GetXYPosition(14640f, 14350f, 21600f),
      this.GetXYPosition(14942f, 17370f, 21600f),
      this.GetXYPosition(12180f, 15935f, 21600f),
      this.GetXYPosition(11612f, 18842f, 21600f),
      this.GetXYPosition(9872f, 17370f, 21600f),
      this.GetXYPosition(8700f, 19712f, 21600f),
      this.GetXYPosition(7527f, 18125f, 21600f),
      this.GetXYPosition(4917f, 21600f, 21600f),
      this.GetXYPosition(4805f, 18240f, 21600f),
      this.GetXYPosition(1285f, 17825f, 21600f),
      this.GetXYPosition(3330f, 15370f, 21600f),
      this.GetXYPosition(0.0f, 12877f, 21600f),
      this.GetXYPosition(3935f, 11592f, 21600f),
      this.GetXYPosition(1172f, 8270f, 21600f),
      this.GetXYPosition(5372f, 7817f, 21600f),
      this.GetXYPosition(4502f, 3625f, 21600f),
      this.GetXYPosition(8550f, 6382f, 21600f),
      this.GetXYPosition(9722f, 1887f, 21600f)
    };
    explosion2.AddLines(points);
    explosion2.CloseFigure();
    return explosion2;
  }

  internal GraphicsPath GetStar4Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star4Point);
    GraphicsPath star4Point = new GraphicsPath();
    PointF[] points = new PointF[8]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["sx1"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["sx1"], this._rectBounds.Y + shapeFormula["sy2"])
    };
    star4Point.AddLines(points);
    star4Point.CloseFigure();
    return star4Point;
  }

  internal GraphicsPath GetStar5Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star5Point);
    GraphicsPath star5Point = new GraphicsPath();
    PointF[] points = new PointF[10]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["sx3"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["sx4"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["sy3"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["sx1"], this._rectBounds.Y + shapeFormula["sy2"])
    };
    star5Point.AddLines(points);
    star5Point.CloseFigure();
    return star5Point;
  }

  internal GraphicsPath GetStar6Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star6Point);
    GraphicsPath star6Point = new GraphicsPath();
    PointF[] points = new PointF[12]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + this._rectBounds.Height / 4f),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["sx3"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + this._rectBounds.Height / 4f),
      new PointF(this._rectBounds.X + shapeFormula["sx4"], this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["sx3"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["sx1"], this._rectBounds.Y + this._rectBounds.Height / 2f)
    };
    star6Point.AddLines(points);
    star6Point.CloseFigure();
    return star6Point;
  }

  internal GraphicsPath GetStar7Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star7Point);
    GraphicsPath star7Point = new GraphicsPath();
    PointF[] points = new PointF[14]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["sx1"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["sx3"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["sx4"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["sx6"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["sx5"], this._rectBounds.Y + shapeFormula["sy3"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["sy4"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy3"])
    };
    star7Point.AddLines(points);
    star7Point.CloseFigure();
    return star7Point;
  }

  internal GraphicsPath GetStar8Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star8Point);
    GraphicsPath star8Point = new GraphicsPath();
    PointF[] points = new PointF[16 /*0x10*/]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["sx1"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["sx3"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["sx4"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["sx4"], this._rectBounds.Y + shapeFormula["sy3"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["sx3"], this._rectBounds.Y + shapeFormula["sy4"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy4"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["sx1"], this._rectBounds.Y + shapeFormula["sy3"])
    };
    star8Point.AddLines(points);
    star8Point.CloseFigure();
    return star8Point;
  }

  internal GraphicsPath GetStar10Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star10Point);
    GraphicsPath star10Point = new GraphicsPath();
    PointF[] points = new PointF[20]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["sx3"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["sx4"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["sx5"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["sx6"], this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["sx5"], this._rectBounds.Y + shapeFormula["sy3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["sx4"], this._rectBounds.Y + shapeFormula["sy4"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["sx3"], this._rectBounds.Y + shapeFormula["sy4"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy3"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["sx1"], this._rectBounds.Y + this._rectBounds.Height / 2f)
    };
    star10Point.AddLines(points);
    star10Point.CloseFigure();
    return star10Point;
  }

  internal GraphicsPath GetStar12Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star12Point);
    GraphicsPath star12Point = new GraphicsPath();
    PointF[] points = new PointF[24]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["sx1"], this._rectBounds.Y + shapeFormula["sy3"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + this._rectBounds.Height / 4f),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 4f, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["sx3"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["sx4"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["sx5"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + this._rectBounds.Height / 4f),
      new PointF(this._rectBounds.X + shapeFormula["sx6"], this._rectBounds.Y + shapeFormula["sy3"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["sx6"], this._rectBounds.Y + shapeFormula["sy4"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["sx5"], this._rectBounds.Y + shapeFormula["sy5"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["sx4"], this._rectBounds.Y + shapeFormula["sy6"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["sx3"], this._rectBounds.Y + shapeFormula["sy6"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 4f, this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy5"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["sx1"], this._rectBounds.Y + shapeFormula["sy4"])
    };
    star12Point.AddLines(points);
    star12Point.CloseFigure();
    return star12Point;
  }

  internal GraphicsPath GetStar16Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star16Point);
    GraphicsPath star16Point = new GraphicsPath();
    PointF[] points = new PointF[32 /*0x20*/]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["sx1"], this._rectBounds.Y + shapeFormula["sy4"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy3"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["sx3"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["sx4"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["sx5"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["sx6"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["sx7"], this._rectBounds.Y + shapeFormula["sy3"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["sx8"], this._rectBounds.Y + shapeFormula["sy4"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["sx8"], this._rectBounds.Y + shapeFormula["sy5"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["sx7"], this._rectBounds.Y + shapeFormula["sy6"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["sx6"], this._rectBounds.Y + shapeFormula["sy7"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y6"]),
      new PointF(this._rectBounds.X + shapeFormula["sx5"], this._rectBounds.Y + shapeFormula["sy8"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["sx4"], this._rectBounds.Y + shapeFormula["sy8"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y6"]),
      new PointF(this._rectBounds.X + shapeFormula["sx3"], this._rectBounds.Y + shapeFormula["sy7"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy6"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["sx1"], this._rectBounds.Y + shapeFormula["sy5"])
    };
    star16Point.AddLines(points);
    star16Point.CloseFigure();
    return star16Point;
  }

  internal GraphicsPath GetStar24Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star24Point);
    GraphicsPath star24Point = new GraphicsPath();
    PointF[] points = new PointF[48 /*0x30*/]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["sx1"], this._rectBounds.Y + shapeFormula["sy6"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy5"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["sx3"], this._rectBounds.Y + shapeFormula["sy4"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["sx4"], this._rectBounds.Y + shapeFormula["sy3"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["sx5"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["sx6"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["sx7"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["sx8"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["sx9"], this._rectBounds.Y + shapeFormula["sy3"]),
      new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["sx10"], this._rectBounds.Y + shapeFormula["sy4"]),
      new PointF(this._rectBounds.X + shapeFormula["x9"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["sx11"], this._rectBounds.Y + shapeFormula["sy5"]),
      new PointF(this._rectBounds.X + shapeFormula["x10"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["sx12"], this._rectBounds.Y + shapeFormula["sy6"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["sx12"], this._rectBounds.Y + shapeFormula["sy7"]),
      new PointF(this._rectBounds.X + shapeFormula["x10"], this._rectBounds.Y + shapeFormula["y6"]),
      new PointF(this._rectBounds.X + shapeFormula["sx11"], this._rectBounds.Y + shapeFormula["sy8"]),
      new PointF(this._rectBounds.X + shapeFormula["x9"], this._rectBounds.Y + shapeFormula["y7"]),
      new PointF(this._rectBounds.X + shapeFormula["sx10"], this._rectBounds.Y + shapeFormula["sy9"]),
      new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y8"]),
      new PointF(this._rectBounds.X + shapeFormula["sx9"], this._rectBounds.Y + shapeFormula["sy10"]),
      new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Y + shapeFormula["y9"]),
      new PointF(this._rectBounds.X + shapeFormula["sx8"], this._rectBounds.Y + shapeFormula["sy11"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y10"]),
      new PointF(this._rectBounds.X + shapeFormula["sx7"], this._rectBounds.Y + shapeFormula["sy12"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["sx6"], this._rectBounds.Y + shapeFormula["sy12"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y10"]),
      new PointF(this._rectBounds.X + shapeFormula["sx5"], this._rectBounds.Y + shapeFormula["sy11"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y9"]),
      new PointF(this._rectBounds.X + shapeFormula["sx4"], this._rectBounds.Y + shapeFormula["sy10"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y8"]),
      new PointF(this._rectBounds.X + shapeFormula["sx3"], this._rectBounds.Y + shapeFormula["sy9"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y7"]),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy8"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y6"]),
      new PointF(this._rectBounds.X + shapeFormula["sx1"], this._rectBounds.Y + shapeFormula["sy7"])
    };
    star24Point.AddLines(points);
    star24Point.CloseFigure();
    return star24Point;
  }

  internal GraphicsPath GetStar32Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star32Point);
    GraphicsPath star32Point = new GraphicsPath();
    PointF[] points = new PointF[64 /*0x40*/]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["sx1"], this._rectBounds.Y + shapeFormula["sy8"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y7"]),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy7"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y6"]),
      new PointF(this._rectBounds.X + shapeFormula["sx3"], this._rectBounds.Y + shapeFormula["sy6"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["sx4"], this._rectBounds.Y + shapeFormula["sy5"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["sx5"], this._rectBounds.Y + shapeFormula["sy4"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["sx6"], this._rectBounds.Y + shapeFormula["sy3"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["sx7"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["sx8"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["sx9"], this._rectBounds.Y + shapeFormula["sy1"]),
      new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["sx10"], this._rectBounds.Y + shapeFormula["sy2"]),
      new PointF(this._rectBounds.X + shapeFormula["x9"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["sx11"], this._rectBounds.Y + shapeFormula["sy3"]),
      new PointF(this._rectBounds.X + shapeFormula["x10"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["sx12"], this._rectBounds.Y + shapeFormula["sy4"]),
      new PointF(this._rectBounds.X + shapeFormula["x11"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["sx13"], this._rectBounds.Y + shapeFormula["sy5"]),
      new PointF(this._rectBounds.X + shapeFormula["x12"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["sx14"], this._rectBounds.Y + shapeFormula["sy6"]),
      new PointF(this._rectBounds.X + shapeFormula["x13"], this._rectBounds.Y + shapeFormula["y6"]),
      new PointF(this._rectBounds.X + shapeFormula["sx15"], this._rectBounds.Y + shapeFormula["sy7"]),
      new PointF(this._rectBounds.X + shapeFormula["x14"], this._rectBounds.Y + shapeFormula["y7"]),
      new PointF(this._rectBounds.X + shapeFormula["sx16"], this._rectBounds.Y + shapeFormula["sy8"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["sx16"], this._rectBounds.Y + shapeFormula["sy9"]),
      new PointF(this._rectBounds.X + shapeFormula["x14"], this._rectBounds.Y + shapeFormula["y8"]),
      new PointF(this._rectBounds.X + shapeFormula["sx15"], this._rectBounds.Y + shapeFormula["sy10"]),
      new PointF(this._rectBounds.X + shapeFormula["x13"], this._rectBounds.Y + shapeFormula["y9"]),
      new PointF(this._rectBounds.X + shapeFormula["sx14"], this._rectBounds.Y + shapeFormula["sy11"]),
      new PointF(this._rectBounds.X + shapeFormula["x12"], this._rectBounds.Y + shapeFormula["y10"]),
      new PointF(this._rectBounds.X + shapeFormula["sx13"], this._rectBounds.Y + shapeFormula["sy12"]),
      new PointF(this._rectBounds.X + shapeFormula["x11"], this._rectBounds.Y + shapeFormula["y11"]),
      new PointF(this._rectBounds.X + shapeFormula["sx12"], this._rectBounds.Y + shapeFormula["sy13"]),
      new PointF(this._rectBounds.X + shapeFormula["x10"], this._rectBounds.Y + shapeFormula["y12"]),
      new PointF(this._rectBounds.X + shapeFormula["sx11"], this._rectBounds.Y + shapeFormula["sy14"]),
      new PointF(this._rectBounds.X + shapeFormula["x9"], this._rectBounds.Y + shapeFormula["y13"]),
      new PointF(this._rectBounds.X + shapeFormula["sx10"], this._rectBounds.Y + shapeFormula["sy15"]),
      new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y14"]),
      new PointF(this._rectBounds.X + shapeFormula["sx9"], this._rectBounds.Y + shapeFormula["sy16"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["sx8"], this._rectBounds.Y + shapeFormula["sy16"]),
      new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Y + shapeFormula["y14"]),
      new PointF(this._rectBounds.X + shapeFormula["sx7"], this._rectBounds.Y + shapeFormula["sy15"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y13"]),
      new PointF(this._rectBounds.X + shapeFormula["sx6"], this._rectBounds.Y + shapeFormula["sy14"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y12"]),
      new PointF(this._rectBounds.X + shapeFormula["sx5"], this._rectBounds.Y + shapeFormula["sy13"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y11"]),
      new PointF(this._rectBounds.X + shapeFormula["sx4"], this._rectBounds.Y + shapeFormula["sy12"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y10"]),
      new PointF(this._rectBounds.X + shapeFormula["sx3"], this._rectBounds.Y + shapeFormula["sy11"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y9"]),
      new PointF(this._rectBounds.X + shapeFormula["sx2"], this._rectBounds.Y + shapeFormula["sy10"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y8"]),
      new PointF(this._rectBounds.X + shapeFormula["sx1"], this._rectBounds.Y + shapeFormula["sy9"])
    };
    star32Point.AddLines(points);
    star32Point.CloseFigure();
    return star32Point;
  }

  internal GraphicsPath GetUpRibbon()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.UpRibbon);
    GraphicsPath upRibbon = new GraphicsPath();
    PointF[] pointFArray = new PointF[25];
    pointFArray[0] = new PointF(this._rectBounds.X, this._rectBounds.Bottom);
    pointFArray[1] = new PointF(this._rectBounds.X + this._rectBounds.Width / 8f, this._rectBounds.Y + shapeFormula["y3"]);
    upRibbon.AddLine(pointFArray[0], pointFArray[1]);
    pointFArray[2] = new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y4"]);
    upRibbon.AddLine(pointFArray[1], pointFArray[2]);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y4"]);
    upRibbon.AddLine(pointFArray[2], pointFArray[3]);
    pointFArray[4] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["hR"]);
    upRibbon.AddLine(pointFArray[3], pointFArray[4]);
    upRibbon.AddArc(pointFArray[4].X, pointFArray[4].Y - shapeFormula["hR"], (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 180f, 90f);
    pointFArray[5] = new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y);
    upRibbon.AddLine(new PointF(pointFArray[4].X + this._rectBounds.Width / 32f, pointFArray[4].Y - shapeFormula["hR"]), new PointF(pointFArray[5].X - this._rectBounds.Width / 32f, pointFArray[5].Y));
    upRibbon.AddArc(pointFArray[5].X - this._rectBounds.Width / 16f, pointFArray[5].Y, (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 270f, 90f);
    pointFArray[6] = new PointF(this._rectBounds.X + shapeFormula["x9"], this._rectBounds.Y + shapeFormula["y4"]);
    upRibbon.AddLine(new PointF(pointFArray[5].X, pointFArray[5].Y + shapeFormula["hR"]), pointFArray[6]);
    pointFArray[7] = new PointF(this._rectBounds.X + shapeFormula["x9"], this._rectBounds.Y + shapeFormula["y4"]);
    upRibbon.AddLine(pointFArray[6], pointFArray[7]);
    pointFArray[8] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y4"]);
    upRibbon.AddLine(pointFArray[7], pointFArray[8]);
    pointFArray[9] = new PointF(this._rectBounds.X + shapeFormula["x10"], this._rectBounds.Y + shapeFormula["y3"]);
    upRibbon.AddLine(pointFArray[8], pointFArray[9]);
    pointFArray[10] = new PointF(this._rectBounds.Right, this._rectBounds.Bottom);
    upRibbon.AddLine(pointFArray[9], pointFArray[10]);
    pointFArray[11] = new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Bottom);
    upRibbon.AddLine(pointFArray[10], new PointF(pointFArray[11].X + this._rectBounds.Width / 32f, pointFArray[11].Y));
    upRibbon.AddArc(pointFArray[11].X, pointFArray[11].Y - shapeFormula["hR"] * 2f, (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 90f, 180f);
    pointFArray[12] = new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y1"]);
    upRibbon.AddLine(new PointF(pointFArray[11].X + this._rectBounds.Width / 32f, pointFArray[11].Y - shapeFormula["hR"] * 2f), new PointF(pointFArray[12].X - this._rectBounds.Width / 32f, pointFArray[12].Y));
    upRibbon.AddArc(pointFArray[12].X - (float) ((double) this._rectBounds.Width / 32.0 * 2.0), pointFArray[12].Y - shapeFormula["hR"] * 2f, (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 90f, -180f);
    pointFArray[13] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]);
    pointFArray[17] = new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y6"]);
    pointFArray[18] = new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y2"]);
    upRibbon.AddLine(new PointF(pointFArray[12].X - (float) ((double) this._rectBounds.Width / 32.0 * 2.0), pointFArray[12].Y - shapeFormula["hR"] * 2f), pointFArray[18]);
    upRibbon.AddLine(pointFArray[18], pointFArray[17]);
    upRibbon.AddArc(pointFArray[11].X, pointFArray[11].Y - shapeFormula["hR"] * 2f, (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 180f, 90f);
    upRibbon.AddLine(new PointF(pointFArray[11].X + this._rectBounds.Width / 32f, pointFArray[11].Y - shapeFormula["hR"] * 2f), new PointF(pointFArray[12].X - this._rectBounds.Width / 32f, pointFArray[12].Y));
    upRibbon.AddArc(pointFArray[12].X - (float) ((double) this._rectBounds.Width / 32.0 * 2.0), pointFArray[12].Y - shapeFormula["hR"] * 2f, (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 90f, -180f);
    upRibbon.AddLine(new PointF(pointFArray[12].X - (float) ((double) this._rectBounds.Width / 32.0 * 2.0), pointFArray[12].Y - shapeFormula["hR"] * 2f), new PointF(pointFArray[13].X + this._rectBounds.Width / 32f, pointFArray[13].Y));
    upRibbon.AddArc(pointFArray[13].X, pointFArray[13].Y, (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 270f, -180f);
    pointFArray[14] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]);
    upRibbon.AddLine(new PointF(pointFArray[13].X + this._rectBounds.Width / 32f, pointFArray[13].Y + shapeFormula["hR"] * 2f), new PointF(pointFArray[14].X - this._rectBounds.Width / 32f, pointFArray[14].Y));
    upRibbon.AddArc(pointFArray[14].X - (float) ((double) this._rectBounds.Width / 32.0 * 2.0), pointFArray[14].Y, (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 270f, 90f);
    pointFArray[15] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y2"]);
    pointFArray[16 /*0x10*/] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y6"]);
    upRibbon.AddLine(pointFArray[15], pointFArray[16 /*0x10*/]);
    upRibbon.AddLine(pointFArray[15], new PointF(pointFArray[13].X + this._rectBounds.Width / 32f, pointFArray[13].Y));
    upRibbon.AddArc(pointFArray[13].X, pointFArray[13].Y, (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 270f, -180f);
    upRibbon.AddLine(new PointF(pointFArray[13].X + this._rectBounds.Width / 32f, pointFArray[13].Y + shapeFormula["hR"] * 2f), new PointF(pointFArray[14].X - this._rectBounds.Width / 32f, pointFArray[14].Y));
    upRibbon.AddArc(pointFArray[14].X - (float) ((double) this._rectBounds.Width / 32.0 * 2.0), pointFArray[14].Y, (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 270f, 180f);
    upRibbon.CloseFigure();
    upRibbon.StartFigure();
    pointFArray[19] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y7"]);
    pointFArray[20] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y4"]);
    upRibbon.AddLine(pointFArray[19], pointFArray[20]);
    upRibbon.StartFigure();
    pointFArray[21] = new PointF(this._rectBounds.X + shapeFormula["x9"], this._rectBounds.Y + shapeFormula["y4"]);
    pointFArray[22] = new PointF(this._rectBounds.X + shapeFormula["x9"], this._rectBounds.Y + shapeFormula["y7"]);
    upRibbon.AddLine(pointFArray[21], pointFArray[22]);
    return upRibbon;
  }

  internal GraphicsPath GetDownRibbon()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.DownRibbon);
    GraphicsPath downRibbon = new GraphicsPath();
    PointF[] pointFArray = new PointF[23];
    pointFArray[0] = new PointF(this._rectBounds.X, this._rectBounds.Y);
    pointFArray[1] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y);
    downRibbon.AddLine(pointFArray[0], new PointF(pointFArray[1].X - this._rectBounds.Width / 32f, pointFArray[1].Y));
    downRibbon.AddArc(pointFArray[1].X - (float) ((double) this._rectBounds.Width / 32.0 * 2.0), pointFArray[1].Y, (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 270f, 180f);
    pointFArray[2] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]);
    downRibbon.AddLine(new PointF(pointFArray[1].X - this._rectBounds.Width / 32f, pointFArray[1].Y + shapeFormula["hR"] * 2f), new PointF(pointFArray[2].X + this._rectBounds.Width / 32f, pointFArray[2].Y));
    downRibbon.AddArc(pointFArray[2].X, pointFArray[2].Y, (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 270f, -180f);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y2"]);
    downRibbon.AddLine(new PointF(pointFArray[2].X + this._rectBounds.Width / 32f, pointFArray[2].Y + shapeFormula["hR"] * 2f), new PointF(pointFArray[3].X - this._rectBounds.Width / 32f, pointFArray[3].Y));
    downRibbon.AddArc(pointFArray[3].X - (float) ((double) this._rectBounds.Width / 32.0 * 2.0), pointFArray[3].Y - shapeFormula["hR"] * 2f, (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 90f, -180f);
    pointFArray[4] = new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Y + shapeFormula["y1"]);
    downRibbon.AddLine(new PointF(pointFArray[3].X - this._rectBounds.Width / 32f, pointFArray[3].Y - shapeFormula["hR"] * 2f), new PointF(pointFArray[4].X + this._rectBounds.Width / 32f, pointFArray[4].Y));
    downRibbon.AddArc(pointFArray[4].X, pointFArray[4].Y - shapeFormula["hR"] * 2f, (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 90f, 180f);
    pointFArray[5] = new PointF(this._rectBounds.Right, this._rectBounds.Y);
    downRibbon.AddLine(new PointF(pointFArray[4].X + this._rectBounds.Width / 32f, pointFArray[4].Y - shapeFormula["hR"] * 2f), pointFArray[5]);
    pointFArray[6] = new PointF(this._rectBounds.X + shapeFormula["x10"], this._rectBounds.Y + shapeFormula["y3"]);
    downRibbon.AddLine(pointFArray[5], pointFArray[6]);
    pointFArray[7] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y4"]);
    downRibbon.AddLine(pointFArray[6], pointFArray[7]);
    pointFArray[8] = new PointF(this._rectBounds.X + shapeFormula["x9"], this._rectBounds.Y + shapeFormula["y4"]);
    downRibbon.AddLine(pointFArray[7], pointFArray[8]);
    pointFArray[9] = new PointF(this._rectBounds.X + shapeFormula["x9"], this._rectBounds.Y + shapeFormula["y5"]);
    downRibbon.AddLine(pointFArray[8], pointFArray[9]);
    downRibbon.AddArc(pointFArray[9].X - (float) ((double) this._rectBounds.Width / 32.0 * 2.0), pointFArray[9].Y - shapeFormula["hR"], (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 0.0f, 90f);
    pointFArray[10] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Bottom);
    downRibbon.AddLine(new PointF(pointFArray[9].X - this._rectBounds.Width / 32f, pointFArray[9].Y + shapeFormula["hR"]), new PointF(pointFArray[10].X + this._rectBounds.Width / 32f, pointFArray[10].Y));
    downRibbon.AddArc(pointFArray[10].X, pointFArray[10].Y - shapeFormula["hR"] * 2f, (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 90f, 90f);
    pointFArray[11] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y4"]);
    downRibbon.AddLine(new PointF(pointFArray[10].X, pointFArray[10].Y - shapeFormula["hR"]), pointFArray[11]);
    pointFArray[12] = new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y4"]);
    downRibbon.AddLine(pointFArray[11], pointFArray[12]);
    pointFArray[13] = new PointF(this._rectBounds.X + this._rectBounds.Width / 8f, this._rectBounds.Y + shapeFormula["y3"]);
    downRibbon.AddLine(pointFArray[12], pointFArray[13]);
    downRibbon.CloseFigure();
    pointFArray[14] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["hR"]);
    pointFArray[15] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y2"]);
    downRibbon.AddLine(pointFArray[14], pointFArray[15]);
    downRibbon.StartFigure();
    pointFArray[16 /*0x10*/] = new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y2"]);
    pointFArray[17] = new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["hR"]);
    downRibbon.AddLine(pointFArray[16 /*0x10*/], pointFArray[17]);
    downRibbon.StartFigure();
    pointFArray[18] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y4"]);
    pointFArray[19] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y6"]);
    downRibbon.AddLine(pointFArray[18], pointFArray[19]);
    downRibbon.StartFigure();
    pointFArray[21] = new PointF(this._rectBounds.X + shapeFormula["x9"], this._rectBounds.Y + shapeFormula["y6"]);
    pointFArray[22] = new PointF(this._rectBounds.X + shapeFormula["x9"], this._rectBounds.Y + shapeFormula["y4"]);
    downRibbon.AddLine(pointFArray[21], pointFArray[22]);
    return downRibbon;
  }

  internal GraphicsPath GetCurvedUpRibbon()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedUpRibbon);
    GraphicsPath curvedUpRibbon = new GraphicsPath();
    PointF[] pointFArray1 = new PointF[5]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 8f, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["q1"]),
      new PointF(this._rectBounds.X + shapeFormula["cx4"], this._rectBounds.Y + shapeFormula["cy4"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y5"])
    };
    PointF[] points1 = new PointF[2]
    {
      pointFArray1[0],
      pointFArray1[1]
    };
    curvedUpRibbon.AddLines(points1);
    curvedUpRibbon.AddBezier(pointFArray1[2], pointFArray1[2], pointFArray1[3], pointFArray1[4]);
    PointF[] pointFArray2 = new PointF[3]
    {
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y6"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["cy6"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y6"])
    };
    curvedUpRibbon.AddBezier(pointFArray2[0], pointFArray2[0], pointFArray2[1], pointFArray2[2]);
    PointF[] pointFArray3 = new PointF[3]
    {
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["cx5"], this._rectBounds.Y + shapeFormula["cy4"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["q1"])
    };
    curvedUpRibbon.AddBezier(pointFArray3[0], pointFArray3[0], pointFArray3[1], pointFArray3[2]);
    PointF[] pointFArray4 = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["cx2"], this._rectBounds.Y + shapeFormula["cy1"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"])
    };
    PointF[] points2 = new PointF[1]{ pointFArray4[0] };
    curvedUpRibbon.AddLines(points2);
    curvedUpRibbon.AddBezier(pointFArray4[1], pointFArray4[1], pointFArray4[2], pointFArray4[3]);
    pointFArray3[0] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y3"]);
    pointFArray3[1] = new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["cy3"]);
    pointFArray3[2] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]);
    PointF[] points3 = new PointF[3]
    {
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y7"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"])
    };
    curvedUpRibbon.AddLines(points3);
    curvedUpRibbon.AddBezier(pointFArray3[0], pointFArray3[0], pointFArray3[1], pointFArray3[2]);
    PointF[] pointFArray5 = new PointF[3]
    {
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["cx1"], this._rectBounds.Y + shapeFormula["cy1"]),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    PointF[] points4 = new PointF[3]
    {
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y7"])
    };
    curvedUpRibbon.AddLines(points4);
    curvedUpRibbon.AddBezier(pointFArray5[0], pointFArray5[0], pointFArray5[1], pointFArray5[2]);
    curvedUpRibbon.CloseFigure();
    PointF[] points5 = new PointF[2]
    {
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y5"])
    };
    curvedUpRibbon.AddLines(points5);
    curvedUpRibbon.CloseFigure();
    points5[0] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y5"]);
    points5[1] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y3"]);
    curvedUpRibbon.AddLines(points5);
    curvedUpRibbon.CloseFigure();
    return curvedUpRibbon;
  }

  internal GraphicsPath GetCurvedDownRibbon()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedDownRibbon);
    GraphicsPath curvedDownRibbon = new GraphicsPath();
    PointF[] pointFArray1 = new PointF[3]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["cx1"], this._rectBounds.Y + shapeFormula["cy1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"])
    };
    curvedDownRibbon.AddBezier(pointFArray1[0], pointFArray1[0], pointFArray1[1], pointFArray1[2]);
    PointF[] points1 = new PointF[3]
    {
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y7"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"])
    };
    curvedDownRibbon.AddLines(points1);
    pointFArray1[0] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]);
    pointFArray1[1] = new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["cy3"]);
    pointFArray1[2] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y3"]);
    curvedDownRibbon.AddBezier(pointFArray1[0], pointFArray1[0], pointFArray1[1], pointFArray1[2]);
    PointF[] points2 = new PointF[3]
    {
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y7"])
    };
    curvedDownRibbon.AddLines(points2);
    pointFArray1[0] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]);
    pointFArray1[1] = new PointF(this._rectBounds.X + shapeFormula["cx2"], this._rectBounds.Y + shapeFormula["cy1"]);
    pointFArray1[2] = new PointF(this._rectBounds.Right, this._rectBounds.Y);
    curvedDownRibbon.AddBezier(pointFArray1[0], pointFArray1[0], pointFArray1[1], pointFArray1[2]);
    PointF[] pointFArray2 = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["rh"]),
      new PointF(this._rectBounds.X + shapeFormula["cx5"], this._rectBounds.Y + shapeFormula["cy4"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y5"])
    };
    PointF[] points3 = new PointF[1]{ pointFArray2[0] };
    curvedDownRibbon.AddLines(points3);
    curvedDownRibbon.AddBezier(pointFArray2[1], pointFArray2[1], pointFArray2[2], pointFArray2[3]);
    pointFArray1[0] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y6"]);
    pointFArray1[1] = new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["cy6"]);
    pointFArray1[2] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y6"]);
    curvedDownRibbon.AddBezier(pointFArray1[0], pointFArray1[0], pointFArray1[1], pointFArray1[2]);
    pointFArray1[0] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y5"]);
    pointFArray1[1] = new PointF(this._rectBounds.X + shapeFormula["cx4"], this._rectBounds.Y + shapeFormula["cy4"]);
    pointFArray1[2] = new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["rh"]);
    curvedDownRibbon.AddBezier(pointFArray1[0], pointFArray1[0], pointFArray1[1], pointFArray1[2]);
    pointFArray1[0] = new PointF(this._rectBounds.X + this._rectBounds.Width / 8f, this._rectBounds.Y + shapeFormula["y2"]);
    points3[0] = pointFArray1[0];
    curvedDownRibbon.AddLines(points3);
    curvedDownRibbon.CloseFigure();
    PointF[] points4 = new PointF[2]
    {
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"])
    };
    curvedDownRibbon.AddLines(points4);
    curvedDownRibbon.CloseFigure();
    points4[0] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y3"]);
    points4[1] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y5"]);
    curvedDownRibbon.AddLines(points4);
    curvedDownRibbon.CloseFigure();
    return curvedDownRibbon;
  }

  internal GraphicsPath GetVerticalScroll()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.VerticalScroll);
    GraphicsPath verticalScroll = new GraphicsPath();
    PointF[] pointFArray1 = new PointF[2]
    {
      new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y)
    };
    PointF[] points1 = new PointF[1]{ pointFArray1[0] };
    verticalScroll.AddLines(points1);
    verticalScroll.AddArc(pointFArray1[1].X, pointFArray1[1].Y, shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 180f, 90f);
    PointF[] pointFArray2 = new PointF[1]
    {
      new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Y)
    };
    verticalScroll.AddArc(pointFArray2[0].X - shapeFormula["ch2"], pointFArray2[0].Y, shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 270f, 180f);
    PointF[] pointFArray3 = new PointF[2]
    {
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["ch"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y4"])
    };
    PointF[] points2 = new PointF[1]{ pointFArray3[0] };
    verticalScroll.AddLines(points2);
    verticalScroll.AddArc(pointFArray3[1].X - shapeFormula["ch2"] * 2f, pointFArray3[1].Y - shapeFormula["ch2"], shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 0.0f, 90f);
    PointF[] pointFArray4 = new PointF[1]
    {
      new PointF(this._rectBounds.X + shapeFormula["ch2"], this._rectBounds.Bottom)
    };
    verticalScroll.AddArc(pointFArray4[0].X - shapeFormula["ch2"], pointFArray4[0].Y - shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 90f, 180f);
    verticalScroll.CloseFigure();
    PointF[] pointFArray5 = new PointF[1]
    {
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y)
    };
    verticalScroll.StartFigure();
    verticalScroll.AddArc(pointFArray5[0].X - shapeFormula["ch2"], pointFArray5[0].Y, shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 270f, 180f);
    verticalScroll.StartFigure();
    verticalScroll.AddArc(pointFArray5[0].X - shapeFormula["ch2"] / 2f, pointFArray5[0].Y + shapeFormula["ch2"], shapeFormula["ch4"] * 2f, shapeFormula["ch4"] * 2f, 90f, 180f);
    PointF[] points3 = new PointF[1]
    {
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ch2"])
    };
    verticalScroll.AddLines(points3);
    PointF[] points4 = new PointF[2];
    verticalScroll.StartFigure();
    points4[0] = new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["ch"]);
    points4[1] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["ch"]);
    verticalScroll.AddLines(points4);
    verticalScroll.CloseFigure();
    PointF[] pointFArray6 = new PointF[1]
    {
      new PointF(this._rectBounds.X + shapeFormula["ch2"], this._rectBounds.Y + shapeFormula["y3"])
    };
    verticalScroll.StartFigure();
    verticalScroll.AddArc(pointFArray6[0].X - shapeFormula["ch2"] / 2f, pointFArray6[0].Y, shapeFormula["ch4"] * 2f, shapeFormula["ch4"] * 2f, 270f, 180f);
    PointF[] points5 = new PointF[1]
    {
      new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y + shapeFormula["y4"])
    };
    verticalScroll.AddLines(points5);
    PointF[] pointFArray7 = new PointF[1]
    {
      new PointF(this._rectBounds.X + shapeFormula["ch2"], this._rectBounds.Bottom)
    };
    verticalScroll.StartFigure();
    verticalScroll.AddArc(pointFArray7[0].X - shapeFormula["ch2"], pointFArray7[0].Y - shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 90f, -90f);
    PointF[] points6 = new PointF[1]
    {
      new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y + shapeFormula["y3"])
    };
    verticalScroll.AddLines(points6);
    return verticalScroll;
  }

  internal GraphicsPath[] GetHorizontalScroll()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.HorizontalScroll);
    GraphicsPath[] horizontalScroll = new GraphicsPath[7];
    for (int index = 0; index < horizontalScroll.Length; ++index)
      horizontalScroll[index] = new GraphicsPath();
    PointF[] pointFArray = new PointF[1]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y3"])
    };
    horizontalScroll[0].AddArc(pointFArray[0].X, pointFArray[0].Y - shapeFormula["ch2"], shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 180f, 90f);
    PointF[] points1 = new PointF[2]
    {
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["ch"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["ch2"])
    };
    PointF[] points2 = new PointF[1]{ points1[0] };
    horizontalScroll[0].AddLines(points2);
    horizontalScroll[0].AddArc(points1[1].X, points1[1].Y - shapeFormula["ch2"], shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 180f, 180f);
    pointFArray[0] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y5"]);
    horizontalScroll[0].AddArc(pointFArray[0].X - shapeFormula["ch2"] * 2f, pointFArray[0].Y - shapeFormula["ch2"], shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 0.0f, 90f);
    points1[0] = new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y + shapeFormula["y6"]);
    points1[1] = new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y + shapeFormula["y7"]);
    points2[0] = points1[0];
    horizontalScroll[0].AddLines(points2);
    horizontalScroll[0].AddArc(points1[0].X - shapeFormula["ch2"] * 2f, points1[0].Y, shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 0.0f, 180f);
    horizontalScroll[0].CloseFigure();
    points1[0] = new PointF(this._rectBounds.X + shapeFormula["x3"] + shapeFormula["ch2"], this._rectBounds.Y + shapeFormula["ch"]);
    points1[1] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ch"]);
    PointF[] points3 = new PointF[2]
    {
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["ch"]),
      points1[0]
    };
    horizontalScroll[1].AddLines(points3);
    horizontalScroll[2].AddArc(points1[1].X - shapeFormula["ch2"], points1[1].Y - shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 90f, -90f);
    horizontalScroll[1].CloseFigure();
    points1[0] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ch"] - shapeFormula["ch2"]);
    points1[1] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ch2"]);
    points3[0] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ch"]);
    points3[1] = points1[0];
    horizontalScroll[3].AddLines(points3);
    horizontalScroll[3].AddArc(points1[1].X - shapeFormula["ch2"], points1[1].Y - shapeFormula["ch2"] / 2f, shapeFormula["ch4"] * 2f, shapeFormula["ch4"] * 2f, 0.0f, 180f);
    points1[0] = new PointF(this._rectBounds.X + shapeFormula["ch2"] * 2f, this._rectBounds.Y - shapeFormula["ch2"] + shapeFormula["y4"]);
    points1[1] = new PointF(this._rectBounds.X + shapeFormula["ch2"], this._rectBounds.Y + shapeFormula["y3"]);
    points3[0] = new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y + shapeFormula["y6"]);
    points3[1] = points1[0];
    horizontalScroll[4].AddLines(points3);
    horizontalScroll[5].AddArc(points1[1].X, points1[1].Y - shapeFormula["ch2"] / 2f, shapeFormula["ch4"] * 2f, shapeFormula["ch4"] * 2f, 180f, 180f);
    horizontalScroll[5].AddArc(points1[1].X - shapeFormula["ch2"], points1[1].Y - shapeFormula["ch2"], shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 0.0f, 180f);
    points1[0] = new PointF(this._rectBounds.X + shapeFormula["ch"] - shapeFormula["ch2"], this._rectBounds.Y + shapeFormula["y3"]);
    points1[1] = new PointF(this._rectBounds.X + shapeFormula["ch"] - shapeFormula["ch2"], this._rectBounds.Y + shapeFormula["y3"] + shapeFormula["ch2"]);
    horizontalScroll[6].AddLines(points1);
    return horizontalScroll;
  }

  internal GraphicsPath GetWave()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Wave);
    GraphicsPath wave = new GraphicsPath();
    PointF[] pointFArray = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y1"])
    };
    wave.AddBezier(pointFArray[0], pointFArray[1], pointFArray[2], pointFArray[3]);
    pointFArray[0] = new PointF(this._rectBounds.X + shapeFormula["x10"], this._rectBounds.Y + shapeFormula["y4"]);
    pointFArray[1] = new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y6"]);
    pointFArray[2] = new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Y + shapeFormula["y5"]);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y4"]);
    wave.AddBezier(pointFArray[0], pointFArray[1], pointFArray[2], pointFArray[3]);
    wave.CloseFigure();
    return wave;
  }

  internal GraphicsPath GetDoubleWave()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.DoubleWave);
    GraphicsPath doubleWave = new GraphicsPath();
    PointF[] pointFArray1 = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y1"])
    };
    doubleWave.AddBezier(pointFArray1[0], pointFArray1[1], pointFArray1[2], pointFArray1[3]);
    PointF[] pointFArray2 = new PointF[4];
    pointFArray2[1] = new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y2"]);
    pointFArray2[2] = new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Y + shapeFormula["y3"]);
    pointFArray2[3] = new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y1"]);
    doubleWave.AddBezier(pointFArray1[3], pointFArray2[1], pointFArray2[2], pointFArray2[3]);
    pointFArray1[0] = new PointF(this._rectBounds.X + shapeFormula["x15"], this._rectBounds.Y + shapeFormula["y4"]);
    pointFArray1[1] = new PointF(this._rectBounds.X + shapeFormula["x14"], this._rectBounds.Y + shapeFormula["y6"]);
    pointFArray1[2] = new PointF(this._rectBounds.X + shapeFormula["x13"], this._rectBounds.Y + shapeFormula["y5"]);
    pointFArray1[3] = new PointF(this._rectBounds.X + shapeFormula["x12"], this._rectBounds.Y + shapeFormula["y4"]);
    doubleWave.AddBezier(pointFArray1[0], pointFArray1[1], pointFArray1[2], pointFArray1[3]);
    pointFArray2[1] = new PointF(this._rectBounds.X + shapeFormula["x11"], this._rectBounds.Y + shapeFormula["y6"]);
    pointFArray2[2] = new PointF(this._rectBounds.X + shapeFormula["x10"], this._rectBounds.Y + shapeFormula["y5"]);
    pointFArray2[3] = new PointF(this._rectBounds.X + shapeFormula["x9"], this._rectBounds.Y + shapeFormula["y4"]);
    doubleWave.AddBezier(pointFArray1[3], pointFArray2[1], pointFArray2[2], pointFArray2[3]);
    doubleWave.CloseFigure();
    return doubleWave;
  }

  internal GraphicsPath GetRectangularCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RectangularCallout);
    GraphicsPath rectangularCalloutPath = new GraphicsPath();
    PointF[] points = new PointF[16 /*0x10*/]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["xt"], this._rectBounds.Y + shapeFormula["yt"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["xr"], this._rectBounds.Y + shapeFormula["yr"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["xb"], this._rectBounds.Y + shapeFormula["yb"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["xl"], this._rectBounds.Y + shapeFormula["yl"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"])
    };
    rectangularCalloutPath.AddLines(points);
    rectangularCalloutPath.CloseFigure();
    return rectangularCalloutPath;
  }

  internal GraphicsPath GetRoundedRectangularCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RoundedRectangularCallout);
    GraphicsPath rectangularCalloutPath = new GraphicsPath();
    PointF[] points1 = new PointF[4];
    rectangularCalloutPath.AddArc(this._rectBounds.X, this._rectBounds.Y, shapeFormula["u1"] * 2f, shapeFormula["u1"] * 2f, 180f, 90f);
    points1[0] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y);
    points1[1] = new PointF(this._rectBounds.X + shapeFormula["xt"], this._rectBounds.Y + shapeFormula["yt"]);
    points1[2] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y);
    points1[3] = new PointF(this._rectBounds.X + shapeFormula["u2"], this._rectBounds.Y);
    rectangularCalloutPath.AddLines(points1);
    rectangularCalloutPath.AddArc(points1[3].X - shapeFormula["u1"], points1[3].Y, shapeFormula["u1"] * 2f, shapeFormula["u1"] * 2f, 270f, 90f);
    PointF[] points2 = new PointF[4]
    {
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["xr"], this._rectBounds.Y + shapeFormula["yr"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["v2"])
    };
    rectangularCalloutPath.AddLines(points2);
    rectangularCalloutPath.AddArc(points2[3].X - shapeFormula["u1"] * 2f, points2[3].Y - shapeFormula["u1"], shapeFormula["u1"] * 2f, shapeFormula["u1"] * 2f, 0.0f, 90f);
    PointF[] points3 = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["xb"], this._rectBounds.Y + shapeFormula["yb"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["u1"], this._rectBounds.Bottom)
    };
    rectangularCalloutPath.AddLines(points3);
    rectangularCalloutPath.AddArc(points3[3].X - shapeFormula["u1"], points3[3].Y - shapeFormula["u1"] * 2f, shapeFormula["u1"] * 2f, shapeFormula["u1"] * 2f, 90f, 90f);
    PointF[] points4 = new PointF[3]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["xl"], this._rectBounds.Y + shapeFormula["yl"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"])
    };
    rectangularCalloutPath.AddLines(points4);
    rectangularCalloutPath.CloseFigure();
    return rectangularCalloutPath;
  }

  internal GraphicsPath GetOvalCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.OvalCallout);
    GraphicsPath ovalCalloutPath = new GraphicsPath();
    PointF[] points = new PointF[1]
    {
      new PointF(this._rectBounds.X + shapeFormula["xPos"], this._rectBounds.Y + shapeFormula["yPos"])
    };
    float startAngle = shapeFormula["stAng1"];
    float sweepAngle = shapeFormula["swAng"];
    if ((double) startAngle < 180.0 && (double) points[0].X < (double) this._rectBounds.X + (double) this._rectBounds.Width / 2.0 || (double) startAngle < 0.0 && (double) points[0].Y > (double) this._rectBounds.Y)
      startAngle += 180f;
    if ((double) sweepAngle < 180.0)
      sweepAngle += 180f;
    ovalCalloutPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, startAngle, sweepAngle);
    ovalCalloutPath.AddLines(points);
    ovalCalloutPath.CloseFigure();
    return ovalCalloutPath;
  }

  internal GraphicsPath GetCloudCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CloudCallout);
    GraphicsPath cloudCalloutPath = new GraphicsPath();
    PointF xyPosition = this.GetXYPosition(3900f, 14370f, 43200f);
    xyPosition.X += shapeFormula["g27"];
    xyPosition.Y -= shapeFormula["g30"];
    SizeF sizeF1 = new SizeF((float) ((double) this._rectBounds.Width * 6753.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 9190.0 / 43200.0 * 2.0));
    SizeF sizeF2 = new SizeF((float) ((double) this._rectBounds.Width * 5333.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 7267.0 / 43200.0 * 2.0));
    SizeF sizeF3 = new SizeF((float) ((double) this._rectBounds.Width * 4365.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 5945.0 / 43200.0 * 2.0));
    SizeF sizeF4 = new SizeF((float) ((double) this._rectBounds.Width * 4857.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 6595.0 / 43200.0 * 2.0));
    SizeF sizeF5 = new SizeF((float) ((double) this._rectBounds.Width * 5333.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 7273.0 / 43200.0 * 2.0));
    SizeF sizeF6 = new SizeF((float) ((double) this._rectBounds.Width * 6775.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 9220.0 / 43200.0 * 2.0));
    SizeF sizeF7 = new SizeF((float) ((double) this._rectBounds.Width * 5785.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 7867.0 / 43200.0 * 2.0));
    SizeF sizeF8 = new SizeF((float) ((double) this._rectBounds.Width * 6752.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 9215.0 / 43200.0 * 2.0));
    SizeF sizeF9 = new SizeF((float) ((double) this._rectBounds.Width * 7720.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 10543.0 / 43200.0 * 2.0));
    SizeF sizeF10 = new SizeF((float) ((double) this._rectBounds.Width * 4360.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 5918.0 / 43200.0 * 2.0));
    SizeF sizeF11 = new SizeF((float) ((double) this._rectBounds.Width * 4345.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 5945.0 / 43200.0 * 2.0));
    cloudCalloutPath.AddArc(this._rectBounds.X + (float) (4076.0 * (double) this._rectBounds.Width / 43200.0), this._rectBounds.Y + (float) (3912.0 * (double) this._rectBounds.Height / 43200.0), sizeF1.Width, sizeF1.Height, -190f, 123f);
    cloudCalloutPath.AddArc(this._rectBounds.X + (float) (13469.0 * (double) this._rectBounds.Width / 43200.0), this._rectBounds.Y + (float) (1304.0 * (double) this._rectBounds.Height / 43200.0), sizeF2.Width, sizeF2.Height, -144f, 89f);
    cloudCalloutPath.AddArc((float) ((double) this._rectBounds.X + (double) this._rectBounds.Width / 2.0 + 531.0 * (double) this._rectBounds.Width / 43200.0), this._rectBounds.Y + 1f, sizeF3.Width, sizeF3.Height, -145f, 99f);
    cloudCalloutPath.AddArc((float) ((double) xyPosition.X + (double) this._rectBounds.Width / 2.0 + 3013.0 * (double) this._rectBounds.Width / 43200.0), this._rectBounds.Y + 1f, sizeF4.Width, sizeF4.Height, -130f, 117f);
    cloudCalloutPath.AddArc((float) ((double) this._rectBounds.Right - (double) sizeF5.Width - 708.0 * (double) this._rectBounds.Width / 43200.0), (float) ((double) this._rectBounds.Y + (double) sizeF4.Height / 2.0 - 1127.0 * (double) this._rectBounds.Height / 43200.0), sizeF5.Width, sizeF5.Height, -78f, 109f);
    cloudCalloutPath.AddArc((float) ((double) this._rectBounds.Right - (double) sizeF6.Width + 354.0 * (double) this._rectBounds.Width / 43200.0), (float) ((double) this._rectBounds.Y + (double) this._rectBounds.Height / 2.0 - 9129.0 * (double) this._rectBounds.Height / 43200.0), sizeF6.Width, sizeF6.Height, -46f, 130f);
    cloudCalloutPath.AddArc((float) ((double) this._rectBounds.X + (double) this._rectBounds.Width / 2.0 + 4608.0 * (double) this._rectBounds.Width / 43200.0), (float) ((double) this._rectBounds.Y + (double) this._rectBounds.Height / 2.0 + 869.0 * (double) this._rectBounds.Height / 43200.0), sizeF7.Width, sizeF7.Height, 0.0f, 114f);
    cloudCalloutPath.AddArc((float) ((double) this._rectBounds.X + (double) this._rectBounds.Width / 2.0 - (double) sizeF8.Width / 2.0 + 886.0 * (double) this._rectBounds.Width / 43200.0), this._rectBounds.Bottom - sizeF8.Height, sizeF8.Width, sizeF8.Height, 22f, 115f);
    cloudCalloutPath.AddArc(this._rectBounds.X + (float) (4962.0 * (double) this._rectBounds.Width / 43200.0), (float) ((double) this._rectBounds.Bottom - (double) sizeF9.Height - 2173.0 * (double) this._rectBounds.Height / 43200.0), sizeF9.Width, sizeF9.Height, 66f, 75f);
    cloudCalloutPath.AddArc(this._rectBounds.X + (float) (1063.0 * (double) this._rectBounds.Width / 43200.0), (float) ((double) this._rectBounds.Y + (double) this._rectBounds.Height / 2.0 + 2608.0 * (double) this._rectBounds.Height / 43200.0), sizeF10.Width, sizeF10.Height, -274f, 146f);
    cloudCalloutPath.AddArc(this._rectBounds.X + 1f, (float) ((double) this._rectBounds.Y + (double) this._rectBounds.Height / 2.0 - (double) sizeF11.Height / 2.0 - 1304.0 * (double) this._rectBounds.Height / 43200.0), sizeF11.Width, sizeF11.Height, -246f, 152f);
    cloudCalloutPath.CloseFigure();
    cloudCalloutPath.AddArc((float) ((double) this._rectBounds.X + (double) this._rectBounds.Width / 2.0 + 2658.0 * (double) this._rectBounds.Width / 43200.0), this._rectBounds.Y + this._rectBounds.Height / 2f, (float) ((double) this._rectBounds.Width * 6753.0 / 43200.0 * 2.0), (float) ((double) this._rectBounds.Height * 9190.0 / 43200.0 * 2.0), -58f, 59f);
    return cloudCalloutPath;
  }

  internal GraphicsPath GetLineCallout1Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout1);
    GraphicsPath lineCallout1Path = new GraphicsPath();
    float num = (double) shapeFormula["x1"] < (double) shapeFormula["x2"] ? ((double) shapeFormula["x1"] < 0.0 ? shapeFormula["x1"] : 0.0f) : ((double) shapeFormula["x2"] < 0.0 ? shapeFormula["x2"] : 0.0f);
    RectangleF rectangleF = this._rectBounds;
    if ((double) num < 0.0)
      rectangleF = new RectangleF(this._rectBounds.X - num, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height);
    PointF[] points = new PointF[4]
    {
      new PointF(rectangleF.X, rectangleF.Y),
      new PointF(rectangleF.Right, rectangleF.Y),
      new PointF(rectangleF.Right, rectangleF.Bottom),
      new PointF(rectangleF.X, rectangleF.Bottom)
    };
    lineCallout1Path.AddLines(points);
    lineCallout1Path.CloseFigure();
    PointF[] pointFArray = new PointF[2]
    {
      new PointF(rectangleF.X + shapeFormula["x1"], rectangleF.Y + shapeFormula["y1"]),
      new PointF(rectangleF.X + shapeFormula["x2"], rectangleF.Y + shapeFormula["y2"])
    };
    lineCallout1Path.AddLine(pointFArray[0], pointFArray[1]);
    lineCallout1Path.CloseFigure();
    return lineCallout1Path;
  }

  internal GraphicsPath GetLineCallout2Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout2);
    GraphicsPath lineCallout2Path = new GraphicsPath();
    PointF[] points = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    lineCallout2Path.AddLines(points);
    lineCallout2Path.CloseFigure();
    lineCallout2Path.StartFigure();
    PointF[] pointFArray = new PointF[3]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"])
    };
    lineCallout2Path.AddLine(pointFArray[0], pointFArray[1]);
    lineCallout2Path.StartFigure();
    lineCallout2Path.AddLine(pointFArray[1], pointFArray[2]);
    return lineCallout2Path;
  }

  internal GraphicsPath GetLineCallout3Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout3);
    GraphicsPath lineCallout3Path = new GraphicsPath();
    PointF[] points = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    lineCallout3Path.AddLines(points);
    lineCallout3Path.CloseFigure();
    PointF[] pointFArray = new PointF[4];
    pointFArray[0] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]);
    pointFArray[1] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]);
    lineCallout3Path.StartFigure();
    lineCallout3Path.AddLine(pointFArray[0], pointFArray[1]);
    pointFArray[2] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]);
    lineCallout3Path.StartFigure();
    lineCallout3Path.AddLine(pointFArray[1], pointFArray[2]);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y4"]);
    lineCallout3Path.StartFigure();
    lineCallout3Path.AddLine(pointFArray[2], pointFArray[3]);
    return lineCallout3Path;
  }

  internal GraphicsPath GetLineCallout1AccentBarPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout1AccentBar);
    GraphicsPath callout1AccentBarPath = new GraphicsPath();
    PointF[] points = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    callout1AccentBarPath.AddLines(points);
    callout1AccentBarPath.CloseFigure();
    PointF[] pointFArray = new PointF[4];
    pointFArray[0] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y);
    pointFArray[1] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom);
    callout1AccentBarPath.AddLine(pointFArray[0], pointFArray[1]);
    callout1AccentBarPath.StartFigure();
    pointFArray[2] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]);
    callout1AccentBarPath.AddLine(pointFArray[2], pointFArray[3]);
    return callout1AccentBarPath;
  }

  internal GraphicsPath GetLineCallout2AccentBarPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout2AccentBar);
    GraphicsPath callout2AccentBarPath = new GraphicsPath();
    PointF[] points = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    callout2AccentBarPath.AddLines(points);
    callout2AccentBarPath.CloseFigure();
    PointF[] pointFArray = new PointF[5];
    pointFArray[0] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y);
    pointFArray[1] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom);
    callout2AccentBarPath.AddLine(pointFArray[0], pointFArray[1]);
    callout2AccentBarPath.StartFigure();
    pointFArray[2] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]);
    callout2AccentBarPath.AddLine(pointFArray[2], pointFArray[3]);
    callout2AccentBarPath.StartFigure();
    pointFArray[4] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]);
    callout2AccentBarPath.AddLine(pointFArray[3], pointFArray[4]);
    callout2AccentBarPath.CloseFigure();
    return callout2AccentBarPath;
  }

  internal GraphicsPath GetLineCallout3AccentBarPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout3AccentBar);
    GraphicsPath callout3AccentBarPath = new GraphicsPath();
    PointF[] points = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    callout3AccentBarPath.AddLines(points);
    callout3AccentBarPath.CloseFigure();
    PointF[] pointFArray = new PointF[6];
    pointFArray[0] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y);
    pointFArray[1] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom);
    callout3AccentBarPath.AddLine(pointFArray[0], pointFArray[1]);
    callout3AccentBarPath.StartFigure();
    pointFArray[2] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]);
    callout3AccentBarPath.AddLine(pointFArray[2], pointFArray[3]);
    callout3AccentBarPath.StartFigure();
    pointFArray[4] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]);
    callout3AccentBarPath.AddLine(pointFArray[3], pointFArray[4]);
    callout3AccentBarPath.StartFigure();
    pointFArray[5] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y4"]);
    callout3AccentBarPath.AddLine(pointFArray[4], pointFArray[5]);
    callout3AccentBarPath.CloseFigure();
    return callout3AccentBarPath;
  }

  internal GraphicsPath GetLineCallout1NoBorderPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout1NoBorder);
    GraphicsPath callout1NoBorderPath = new GraphicsPath();
    PointF[] points1 = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    callout1NoBorderPath.AddLines(points1);
    callout1NoBorderPath.CloseFigure();
    PointF[] points2 = new PointF[2]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"])
    };
    callout1NoBorderPath.AddLines(points2);
    return callout1NoBorderPath;
  }

  internal GraphicsPath GetLineCallout2NoBorderPath()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout2NoBorder);
    GraphicsPath callout2NoBorderPath = new GraphicsPath();
    PointF[] points = new PointF[2];
    callout2NoBorderPath.AddLines(points);
    callout2NoBorderPath.CloseFigure();
    return callout2NoBorderPath;
  }

  internal GraphicsPath GetLineCallout3NoBorderPath()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout3NoBorder);
    GraphicsPath callout3NoBorderPath = new GraphicsPath();
    PointF[] points = new PointF[2];
    callout3NoBorderPath.AddLines(points);
    callout3NoBorderPath.CloseFigure();
    return callout3NoBorderPath;
  }

  internal GraphicsPath GetLineCallout1BorderAndAccentBarPath()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout1BorderAndAccentBar);
    GraphicsPath andAccentBarPath = new GraphicsPath();
    PointF[] points = new PointF[2];
    andAccentBarPath.AddLines(points);
    andAccentBarPath.CloseFigure();
    return andAccentBarPath;
  }

  internal GraphicsPath GetLineCallout2BorderAndAccentBarPath()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout2BorderAndAccentBar);
    GraphicsPath andAccentBarPath = new GraphicsPath();
    PointF[] points = new PointF[2];
    andAccentBarPath.AddLines(points);
    andAccentBarPath.CloseFigure();
    return andAccentBarPath;
  }

  internal GraphicsPath GetLineCallout3BorderAndAccentBarPath()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout3BorderAndAccentBar);
    GraphicsPath andAccentBarPath = new GraphicsPath();
    PointF[] points = new PointF[2];
    andAccentBarPath.AddLines(points);
    andAccentBarPath.CloseFigure();
    return andAccentBarPath;
  }

  private float GetDegreeValue(float value) => value / 60000f;

  private PointF GetXYPosition(float xDifference, float yDifference, float positionRatio)
  {
    return new PointF(this._rectBounds.X + this._rectBounds.Width * xDifference / positionRatio, this._rectBounds.Y + this._rectBounds.Height * yDifference / positionRatio);
  }
}
