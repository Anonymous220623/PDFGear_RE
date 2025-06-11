// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Rendering.PDFShapePath
// Assembly: Syncfusion.PresentationToPdfConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 66FE5253-50B1-47E3-888F-DF2FAFB49C7E
// Assembly location: C:\Program Files\PDFgear\Syncfusion.PresentationToPdfConverter.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Presentation.Rendering;

internal class PDFShapePath
{
  private RectangleF _rectBounds;
  private Dictionary<string, string> _shapeGuide;
  private FormulaValues _formulaValues;

  internal PDFShapePath(RectangleF bounds, Dictionary<string, string> shapeGuide)
  {
    this._rectBounds = bounds;
    this._shapeGuide = shapeGuide;
    this._formulaValues = new FormulaValues(this._rectBounds, this._shapeGuide);
  }

  internal PdfPath GetCurvedConnector2Path()
  {
    PdfPath curvedConnector2Path = new PdfPath();
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

  internal PdfPath GetCurvedConnector3Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedConnector);
    PdfPath curvedConnector3Path = new PdfPath();
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

  internal PdfPath GetCurvedConnector4Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedConnector4);
    PdfPath curvedConnector4Path = new PdfPath();
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

  internal PdfPath GetCurvedConnector5Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedConnector5);
    PdfPath curvedConnector5Path = new PdfPath();
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

  internal PdfPath GetBentConnector2Path()
  {
    PointF[] linePoints = new PointF[3]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    PdfPath bentConnector2Path = new PdfPath();
    bentConnector2Path.AddLines(linePoints);
    return bentConnector2Path;
  }

  internal PdfPath GetBentConnector3Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.ElbowConnector);
    PointF[] linePoints = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    PdfPath bentConnector3Path = new PdfPath();
    bentConnector3Path.AddLines(linePoints);
    return bentConnector3Path;
  }

  internal PdfPath GetBentConnector4Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.BentConnector4);
    PointF[] linePoints = new PointF[5]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    PdfPath bentConnector4Path = new PdfPath();
    bentConnector4Path.AddLines(linePoints);
    return bentConnector4Path;
  }

  internal PdfPath GetBentConnector5Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.BentConnector5);
    PointF[] linePoints = new PointF[6]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    PdfPath bentConnector5Path = new PdfPath();
    bentConnector5Path.AddLines(linePoints);
    return bentConnector5Path;
  }

  internal PdfPath GetRoundedRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RoundedRectangle);
    PdfPath roundedRectanglePath = new PdfPath();
    float num = shapeFormula["x1"] * 2f;
    roundedRectanglePath.AddArc(this._rectBounds.X, this._rectBounds.Y, num, num, 180f, 90f);
    roundedRectanglePath.AddArc(this._rectBounds.Right - num, this._rectBounds.Y, num, num, 270f, 90f);
    roundedRectanglePath.AddArc(this._rectBounds.Right - num, this._rectBounds.Bottom - num, num, num, 0.0f, 90f);
    roundedRectanglePath.AddArc(this._rectBounds.X, this._rectBounds.Bottom - num, num, num, 90f, 90f);
    roundedRectanglePath.CloseFigure();
    return roundedRectanglePath;
  }

  internal PdfPath GetSnipSingleCornerRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.SnipSingleCornerRectangle);
    PdfPath cornerRectanglePath = new PdfPath();
    PointF[] linePoints = new PointF[5]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["dx1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    cornerRectanglePath.AddLines(linePoints);
    cornerRectanglePath.CloseFigure();
    return cornerRectanglePath;
  }

  internal PdfPath GetSnipSameSideCornerRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.SnipSameSideCornerRectangle);
    PdfPath cornerRectanglePath = new PdfPath();
    PointF[] linePoints = new PointF[8]
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
    cornerRectanglePath.AddLines(linePoints);
    cornerRectanglePath.CloseFigure();
    return cornerRectanglePath;
  }

  internal PdfPath GetSnipDiagonalCornerRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.SnipDiagonalCornerRectangle);
    PdfPath cornerRectanglePath = new PdfPath();
    PointF[] linePoints = new PointF[8]
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
    cornerRectanglePath.AddLines(linePoints);
    cornerRectanglePath.CloseFigure();
    return cornerRectanglePath;
  }

  internal PdfPath GetSnipAndRoundSingleCornerRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.SnipAndRoundSingleCornerRectangle);
    PdfPath cornerRectanglePath = new PdfPath();
    PointF[] linePoints = new PointF[6];
    float num = shapeFormula["x1"] * 2f;
    linePoints[0] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y);
    linePoints[1] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y);
    linePoints[2] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["dx2"]);
    linePoints[3] = new PointF(this._rectBounds.Right, this._rectBounds.Bottom);
    linePoints[4] = new PointF(this._rectBounds.X, this._rectBounds.Bottom);
    linePoints[5] = new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["x1"]);
    cornerRectanglePath.AddArc(this._rectBounds.X, this._rectBounds.Y, num, num, 180f, 90f);
    cornerRectanglePath.AddLines(linePoints);
    cornerRectanglePath.CloseFigure();
    return cornerRectanglePath;
  }

  internal PdfPath GetRoundSingleCornerRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RoundSingleCornerRectangle);
    PdfPath cornerRectanglePath = new PdfPath();
    PointF[] linePoints = new PointF[2];
    float num = shapeFormula["dx1"] * 2f;
    linePoints[0] = new PointF(this._rectBounds.X, this._rectBounds.Y);
    linePoints[1] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y);
    cornerRectanglePath.AddLines(linePoints);
    cornerRectanglePath.AddArc(this._rectBounds.Right - num, this._rectBounds.Y, num, num, 270f, 90f);
    linePoints[0] = new PointF(this._rectBounds.Right, this._rectBounds.Bottom);
    linePoints[1] = new PointF(this._rectBounds.X, this._rectBounds.Bottom);
    cornerRectanglePath.AddLines(linePoints);
    cornerRectanglePath.CloseFigure();
    return cornerRectanglePath;
  }

  internal PdfPath GetRoundSameSideCornerRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RoundSameSideCornerRectangle);
    PdfPath cornerRectanglePath = new PdfPath();
    PointF[] linePoints = new PointF[2];
    float num1 = shapeFormula["tx1"] * 2f;
    float num2 = shapeFormula["bx1"] * 2f;
    linePoints[0] = new PointF(this._rectBounds.X + shapeFormula["tx1"], this._rectBounds.Y);
    linePoints[1] = new PointF(this._rectBounds.X + shapeFormula["tx2"], this._rectBounds.Y);
    cornerRectanglePath.AddLines(linePoints);
    cornerRectanglePath.AddArc(this._rectBounds.Right - num1, this._rectBounds.Y, num1, num1, 270f, 90f);
    if ((double) num2 == 0.0)
    {
      linePoints[0] = new PointF(this._rectBounds.Right, this._rectBounds.Bottom);
      linePoints[1] = new PointF(this._rectBounds.X, this._rectBounds.Bottom);
      cornerRectanglePath.AddLines(linePoints);
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

  internal PdfPath GetRoundDiagonalCornerRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RoundDiagonalCornerRectangle);
    PdfPath cornerRectanglePath = new PdfPath();
    PointF[] linePoints = new PointF[2];
    float num1 = shapeFormula["x1"] * 2f;
    float num2 = shapeFormula["a"] * 2f;
    linePoints[0] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y);
    linePoints[1] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y);
    cornerRectanglePath.AddLines(linePoints);
    if ((double) num2 != 0.0)
      cornerRectanglePath.AddArc(this._rectBounds.Right - num2, this._rectBounds.Y, num2, num2, 270f, 90f);
    cornerRectanglePath.AddArc(this._rectBounds.Right - num1, this._rectBounds.Bottom - num1, num1, num1, 0.0f, 90f);
    if ((double) num2 == 0.0)
    {
      linePoints[0] = new PointF(this._rectBounds.Right - shapeFormula["x1"], this._rectBounds.Bottom);
      linePoints[1] = new PointF(this._rectBounds.X, this._rectBounds.Bottom);
      cornerRectanglePath.AddLines(linePoints);
    }
    else
      cornerRectanglePath.AddArc(this._rectBounds.X, this._rectBounds.Bottom - num2, num2, num2, 90f, 90f);
    cornerRectanglePath.AddArc(this._rectBounds.X, this._rectBounds.Y, num1, num1, 180f, 90f);
    cornerRectanglePath.CloseFigure();
    return cornerRectanglePath;
  }

  internal PdfPath GetTrianglePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.IsoscelesTriangle);
    PdfPath trianglePath = new PdfPath();
    PointF[] linePoints = new PointF[3]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    trianglePath.AddLines(linePoints);
    trianglePath.CloseFigure();
    return trianglePath;
  }

  internal PdfPath GetParallelogramPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Parallelogram);
    PdfPath parallelogramPath = new PdfPath();
    PointF[] linePoints = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Bottom)
    };
    parallelogramPath.AddLines(linePoints);
    parallelogramPath.CloseFigure();
    return parallelogramPath;
  }

  internal PdfPath GetTrapezoidPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Trapezoid);
    PdfPath trapezoidPath = new PdfPath();
    PointF[] linePoints = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    trapezoidPath.AddLines(linePoints);
    trapezoidPath.CloseFigure();
    return trapezoidPath;
  }

  internal PdfPath GetRegularPentagonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RegularPentagon);
    PdfPath regularPentagonPath = new PdfPath();
    PointF[] linePoints = new PointF[5]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"])
    };
    regularPentagonPath.AddLines(linePoints);
    regularPentagonPath.CloseFigure();
    return regularPentagonPath;
  }

  internal PdfPath GetHexagonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Hexagon);
    PdfPath hexagonPath = new PdfPath();
    PointF[] linePoints = new PointF[6]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"])
    };
    hexagonPath.AddLines(linePoints);
    hexagonPath.CloseFigure();
    return hexagonPath;
  }

  internal PdfPath GetHeptagonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Heptagon);
    PdfPath heptagonPath = new PdfPath();
    PointF[] linePoints = new PointF[7]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"])
    };
    heptagonPath.AddLines(linePoints);
    heptagonPath.CloseFigure();
    return heptagonPath;
  }

  internal PdfPath GetOctagonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Octagon);
    PdfPath octagonPath = new PdfPath();
    PointF[] linePoints = new PointF[8]
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
    octagonPath.AddLines(linePoints);
    octagonPath.CloseFigure();
    return octagonPath;
  }

  internal PdfPath GetDecagonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Decagon);
    PdfPath decagonPath = new PdfPath();
    PointF[] linePoints = new PointF[10]
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
    decagonPath.AddLines(linePoints);
    decagonPath.CloseFigure();
    return decagonPath;
  }

  internal PdfPath GetDodecagonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Dodecagon);
    PdfPath dodecagonPath = new PdfPath();
    PointF[] linePoints = new PointF[12]
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
    dodecagonPath.AddLines(linePoints);
    dodecagonPath.CloseFigure();
    return dodecagonPath;
  }

  internal PdfPath GetPiePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Pie);
    PdfPath piePath = new PdfPath();
    piePath.AddPie(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, shapeFormula["stAng"] / 60000f, shapeFormula["swAng"] / 60000f);
    return piePath;
  }

  internal PdfPath GetChordPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Chord);
    PdfPath chordPath = new PdfPath();
    chordPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, shapeFormula["stAng"] / 60000f, shapeFormula["swAng"] / 60000f);
    chordPath.CloseFigure();
    return chordPath;
  }

  internal PdfPath GetTearDropPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Teardrop);
    PdfPath tearDropPath = new PdfPath();
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

  internal PdfPath GetSwooshArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.SwooshArrow);
    PdfPath swooshArrowPath = new PdfPath();
    PointF[] points1 = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["xP1"], this._rectBounds.Y + shapeFormula["yP1"]),
      new PointF(this._rectBounds.X + shapeFormula["xB"], this._rectBounds.Y + shapeFormula["yB"]),
      new PointF(this._rectBounds.X + shapeFormula["xB"], this._rectBounds.Y + shapeFormula["yB"])
    };
    swooshArrowPath.AddBeziers(points1);
    PointF[] linePoints = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["xC"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["yD"]),
      new PointF(this._rectBounds.X + shapeFormula["xE"], this._rectBounds.Y + shapeFormula["yE"]),
      new PointF(this._rectBounds.X + shapeFormula["xF"], this._rectBounds.Y + shapeFormula["yF"])
    };
    swooshArrowPath.AddLines(linePoints);
    PointF[] points2 = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["xF"], this._rectBounds.Y + shapeFormula["yF"]),
      new PointF(this._rectBounds.X + shapeFormula["xP2"], this._rectBounds.Y + shapeFormula["yP2"]),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    swooshArrowPath.AddBeziers(points2);
    return swooshArrowPath;
  }

  internal PdfPath GetFramePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Frame);
    PdfPath framePath = new PdfPath();
    PointF[] linePoints = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    framePath.AddLines(linePoints);
    framePath.CloseFigure();
    linePoints[0] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["x1"]);
    linePoints[1] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"]);
    linePoints[2] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y4"]);
    linePoints[3] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["x1"]);
    framePath.AddLines(linePoints);
    framePath.CloseFigure();
    return framePath;
  }

  internal PdfPath GetHalfFramePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.HalfFrame);
    PdfPath halfFramePath = new PdfPath();
    PointF[] linePoints = new PointF[6]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    halfFramePath.AddLines(linePoints);
    halfFramePath.CloseFigure();
    return halfFramePath;
  }

  internal PdfPath GetL_ShapePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Corner);
    PdfPath lShapePath = new PdfPath();
    PointF[] linePoints = new PointF[6]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    lShapePath.AddLines(linePoints);
    lShapePath.CloseFigure();
    return lShapePath;
  }

  internal PdfPath GetDiagonalStripePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.DiagonalStripe);
    PdfPath diagonalStripePath = new PdfPath();
    PointF[] linePoints = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Left, this._rectBounds.Bottom)
    };
    diagonalStripePath.AddLines(linePoints);
    diagonalStripePath.CloseFigure();
    return diagonalStripePath;
  }

  internal PdfPath GetCrossPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Cross);
    PdfPath crossPath = new PdfPath();
    PointF[] linePoints = new PointF[12]
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
    crossPath.AddLines(linePoints);
    crossPath.CloseFigure();
    return crossPath;
  }

  internal PdfPath GetPlaquePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Plaque);
    PdfPath plaquePath = new PdfPath();
    float num = shapeFormula["x1"] * 2f;
    plaquePath.AddArc(this._rectBounds.X - shapeFormula["x1"], this._rectBounds.Y - shapeFormula["x1"], num, num, 90f, -90f);
    plaquePath.AddArc(this._rectBounds.Right - shapeFormula["x1"], this._rectBounds.Y - shapeFormula["x1"], num, num, 180f, -90f);
    plaquePath.AddArc(this._rectBounds.Right - shapeFormula["x1"], this._rectBounds.Bottom - shapeFormula["x1"], num, num, 270f, -90f);
    plaquePath.AddArc(this._rectBounds.X - shapeFormula["x1"], this._rectBounds.Bottom - shapeFormula["x1"], num, num, 0.0f, -90f);
    plaquePath.CloseFigure();
    return plaquePath;
  }

  internal PdfPath GetCanPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Can);
    PdfPath canPath = new PdfPath();
    canPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, shapeFormula["y1"] * 2f, 0.0f, 180f);
    canPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, shapeFormula["y1"] * 2f, 180f, 180f);
    canPath.AddArc(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y3"] - shapeFormula["y1"], this._rectBounds.Width, shapeFormula["y1"] * 2f, 0.0f, 180f);
    canPath.AddLine(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y3"] - shapeFormula["y1"], this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"]);
    return canPath;
  }

  internal PdfPath GetLeftRightRibbonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftRightRibbon);
    PdfPath leftRightRibbonPath = new PdfPath();
    PointF[] linePoints1 = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["ly2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["ly1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["ly1"])
    };
    leftRightRibbonPath.AddLines(linePoints1);
    leftRightRibbonPath.AddArc(linePoints1[3].X, linePoints1[3].Y, this._rectBounds.Width / 32f, shapeFormula["hR"] * 2f, 270f, 180f);
    leftRightRibbonPath.AddArc(linePoints1[3].X, linePoints1[3].Y + shapeFormula["hR"] * 2f, this._rectBounds.Width / 32f, shapeFormula["hR"] * 2f, 270f, -180f);
    PointF[] linePoints2 = new PointF[6]
    {
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ry2"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ry1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["ry3"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ry4"]),
      new PointF((float) ((double) this._rectBounds.X + (double) this._rectBounds.Width / 2.0 + (double) this._rectBounds.Width / 64.0), this._rectBounds.Y + shapeFormula["ry4"])
    };
    leftRightRibbonPath.AddLines(linePoints2);
    leftRightRibbonPath.AddArc(this._rectBounds.X + this._rectBounds.Width / 2f, linePoints2[5].Y - shapeFormula["hR"] * 2f, this._rectBounds.Width / 32f, shapeFormula["hR"] * 2f, 90f, 90f);
    PointF[] linePoints3 = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["ly3"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["ly3"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["ly4"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["ly2"])
    };
    leftRightRibbonPath.AddLines(linePoints3);
    leftRightRibbonPath.CloseFigure();
    PointF[] pointFArray = new PointF[4]
    {
      new PointF(linePoints1[3].X, linePoints1[3].Y + shapeFormula["hR"] * 3f),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["ly3"]),
      new PointF(linePoints1[3].X + this._rectBounds.Width / 32f, linePoints1[3].Y + shapeFormula["hR"]),
      new PointF(linePoints1[3].X + this._rectBounds.Width / 32f, linePoints1[3].Y + shapeFormula["hR"] * 4f)
    };
    leftRightRibbonPath.AddLine(pointFArray[0], pointFArray[1]);
    leftRightRibbonPath.CloseFigure();
    leftRightRibbonPath.AddLine(pointFArray[2], pointFArray[3]);
    leftRightRibbonPath.CloseFigure();
    return leftRightRibbonPath;
  }

  internal PdfPath GetFunnelPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Funnel);
    PdfPath funnelPath = new PdfPath();
    funnelPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height / 2f, shapeFormula["stAng1"] - 2f, shapeFormula["swAng1"] + 4f);
    funnelPath.AddLine(funnelPath.PathPoints[funnelPath.PointCount - 1].X, funnelPath.PathPoints[funnelPath.PointCount - 1].Y, this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]);
    funnelPath.AddArc((float) ((double) this._rectBounds.X + (double) shapeFormula["x3"] - (double) shapeFormula["rw3"] * 2.0), this._rectBounds.Y + shapeFormula["y2"] - shapeFormula["rh3"], shapeFormula["rw3"] * 2f, shapeFormula["rh3"] * 2f, shapeFormula["da"], shapeFormula["swAng3"]);
    funnelPath.CloseFigure();
    funnelPath.AddArc(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + this._rectBounds.Height / 20f, shapeFormula["rw2"] * 2f, shapeFormula["rh2"] * 2f, 180f, 360f);
    return funnelPath;
  }

  internal PdfPath GetGear6Path()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.Gear6);
    return new PdfPath();
  }

  internal PdfPath GetGear9Path()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.Gear9);
    return new PdfPath();
  }

  internal PdfPath GetCubePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Cube);
    PdfPath cubePath = new PdfPath();
    PointF[] linePoints = new PointF[6]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["y1"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    cubePath.AddLines(linePoints);
    cubePath.CloseFigure();
    PointF point2 = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]);
    cubePath.AddLine(linePoints[0], point2);
    cubePath.AddLine(linePoints[2], point2);
    cubePath.AddLine(linePoints[4], point2);
    return cubePath;
  }

  internal PdfPath GetBevelPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Bevel);
    PdfPath bevelPath = new PdfPath();
    PointF[] linePoints1 = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.Left, this._rectBounds.Bottom)
    };
    bevelPath.AddLines(linePoints1);
    bevelPath.CloseFigure();
    PointF[] linePoints2 = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["x1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"])
    };
    bevelPath.AddLines(linePoints2);
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

  internal PdfPath GetDonutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Donut);
    PdfPath donutPath = new PdfPath();
    RectangleF rectangle = new RectangleF(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height);
    donutPath.AddArc(rectangle, 180f, 90f);
    donutPath.AddArc(rectangle, 270f, 90f);
    donutPath.AddArc(rectangle, 0.0f, 90f);
    donutPath.AddArc(rectangle, 90f, 90f);
    donutPath.CloseFigure();
    rectangle = new RectangleF(this._rectBounds.X + shapeFormula["dr"], this._rectBounds.Y + shapeFormula["dr"], shapeFormula["iwd2"] * 2f, shapeFormula["ihd2"] * 2f);
    donutPath.AddArc(rectangle, 180f, -90f);
    donutPath.AddArc(rectangle, 90f, -90f);
    donutPath.AddArc(rectangle, 0.0f, -90f);
    donutPath.AddArc(rectangle, 270f, -90f);
    donutPath.CloseFigure();
    return donutPath;
  }

  internal PdfPath GetNoSymbolPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.NoSymbol);
    PdfPath noSymbolPath = new PdfPath();
    RectangleF rectangle = new RectangleF(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height);
    noSymbolPath.AddArc(rectangle, 180f, 90f);
    noSymbolPath.AddArc(rectangle, 270f, 90f);
    noSymbolPath.AddArc(rectangle, 0.0f, 90f);
    noSymbolPath.AddArc(rectangle, 90f, 90f);
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

  internal PdfPath GetBlockArcPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.BlockArc);
    PdfPath blockArcPath = new PdfPath();
    RectangleF rectangle = new RectangleF(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height);
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
        blockArcPath.AddArc(rectangle, shapeFormula["stAng"] / 60000f, shapeFormula["swAng"] / 60000f);
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

  internal PdfPath GetFoldedCornerPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.FoldedCorner);
    PdfPath foldedCornerPath = new PdfPath();
    PointF[] linePoints = new PointF[8]
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
    foldedCornerPath.AddLines(linePoints);
    return foldedCornerPath;
  }

  internal PdfPath[] GetSmileyFacePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.SmileyFace);
    PdfPath[] smileyFacePath = new PdfPath[2];
    for (int index = 0; index < smileyFacePath.Length; ++index)
      smileyFacePath[index] = new PdfPath();
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

  internal PdfPath GetHeartPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Heart);
    PdfPath heartPath = new PdfPath();
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

  internal PdfPath GetLightningBoltPath()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.LightningBolt);
    PdfPath lightningBoltPath = new PdfPath();
    PointF[] linePoints = new PointF[11]
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
    lightningBoltPath.AddLines(linePoints);
    lightningBoltPath.AddLine(linePoints[0], linePoints[10]);
    lightningBoltPath.CloseAllFigures();
    return lightningBoltPath;
  }

  internal PdfPath GetSunPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Sun);
    PdfPath sunPath = new PdfPath();
    PointF[] linePoints = new PointF[3]
    {
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x15"], this._rectBounds.Y + shapeFormula["y18"]),
      new PointF(this._rectBounds.X + shapeFormula["x15"], this._rectBounds.Y + shapeFormula["y14"])
    };
    sunPath.AddLines(linePoints);
    sunPath.CloseFigure();
    linePoints[0] = new PointF(this._rectBounds.X + shapeFormula["ox1"], this._rectBounds.Y + shapeFormula["oy1"]);
    linePoints[1] = new PointF(this._rectBounds.X + shapeFormula["x16"], this._rectBounds.Y + shapeFormula["y13"]);
    linePoints[2] = new PointF(this._rectBounds.X + shapeFormula["x17"], this._rectBounds.Y + shapeFormula["y12"]);
    sunPath.AddLines(linePoints);
    sunPath.CloseFigure();
    linePoints[0] = new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y);
    linePoints[1] = new PointF(this._rectBounds.X + shapeFormula["x18"], this._rectBounds.Y + shapeFormula["y10"]);
    linePoints[2] = new PointF(this._rectBounds.X + shapeFormula["x14"], this._rectBounds.Y + shapeFormula["y10"]);
    sunPath.AddLines(linePoints);
    sunPath.CloseFigure();
    linePoints[0] = new PointF(this._rectBounds.X + shapeFormula["ox2"], this._rectBounds.Y + shapeFormula["oy1"]);
    linePoints[1] = new PointF(this._rectBounds.X + shapeFormula["x13"], this._rectBounds.Y + shapeFormula["y12"]);
    linePoints[2] = new PointF(this._rectBounds.X + shapeFormula["x12"], this._rectBounds.Y + shapeFormula["y13"]);
    sunPath.AddLines(linePoints);
    sunPath.CloseFigure();
    linePoints[0] = new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f);
    linePoints[1] = new PointF(this._rectBounds.X + shapeFormula["x10"], this._rectBounds.Y + shapeFormula["y14"]);
    linePoints[2] = new PointF(this._rectBounds.X + shapeFormula["x10"], this._rectBounds.Y + shapeFormula["y18"]);
    sunPath.AddLines(linePoints);
    sunPath.CloseFigure();
    linePoints[0] = new PointF(this._rectBounds.X + shapeFormula["ox2"], this._rectBounds.Y + shapeFormula["oy2"]);
    linePoints[1] = new PointF(this._rectBounds.X + shapeFormula["x12"], this._rectBounds.Y + shapeFormula["y17"]);
    linePoints[2] = new PointF(this._rectBounds.X + shapeFormula["x13"], this._rectBounds.Y + shapeFormula["y16"]);
    sunPath.AddLines(linePoints);
    sunPath.CloseFigure();
    linePoints[0] = new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom);
    linePoints[1] = new PointF(this._rectBounds.X + shapeFormula["x14"], this._rectBounds.Y + shapeFormula["y15"]);
    linePoints[2] = new PointF(this._rectBounds.X + shapeFormula["x18"], this._rectBounds.Y + shapeFormula["y15"]);
    sunPath.AddLines(linePoints);
    sunPath.CloseFigure();
    linePoints[0] = new PointF(this._rectBounds.X + shapeFormula["ox1"], this._rectBounds.Y + shapeFormula["oy2"]);
    linePoints[1] = new PointF(this._rectBounds.X + shapeFormula["x17"], this._rectBounds.Y + shapeFormula["y16"]);
    linePoints[2] = new PointF(this._rectBounds.X + shapeFormula["x16"], this._rectBounds.Y + shapeFormula["y17"]);
    sunPath.AddLines(linePoints);
    sunPath.CloseFigure();
    sunPath.AddEllipse(this._rectBounds.X + shapeFormula["x19"], this._rectBounds.Y + this._rectBounds.Height / 2f - shapeFormula["hR"], shapeFormula["wR"] * 2f, shapeFormula["hR"] * 2f);
    sunPath.CloseFigure();
    return sunPath;
  }

  internal PdfPath GetMoonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Moon);
    PdfPath moonPath = new PdfPath();
    moonPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width * 2f, this._rectBounds.Height, 90f, 180f);
    float startAngle = shapeFormula["stAng1"];
    if ((double) startAngle < 180.0)
      startAngle += 180f;
    moonPath.AddArc(this._rectBounds.X + shapeFormula["g0w"], this._rectBounds.Y + this._rectBounds.Height / 2f - shapeFormula["dy1"], shapeFormula["g18w"] * 2f, shapeFormula["dy1"] * 2f, startAngle, shapeFormula["swAng1"] % 360f);
    moonPath.CloseFigure();
    return moonPath;
  }

  internal PdfPath GetCloudPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Cloud);
    PdfPath cloudPath = new PdfPath();
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

  internal PdfPath[] GetArcPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Arc);
    PdfPath[] arcPath = new PdfPath[2];
    for (int index = 0; index < arcPath.Length; ++index)
      arcPath[index] = new PdfPath();
    arcPath[0].AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, shapeFormula["stAng"] / 60000f, shapeFormula["swAng"] / 60000f);
    arcPath[1].AddPie(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, shapeFormula["stAng"] / 60000f, shapeFormula["swAng"] / 60000f);
    return arcPath;
  }

  internal PdfPath GetDoubleBracketPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.DoubleBracket);
    PdfPath doubleBracketPath = new PdfPath();
    doubleBracketPath.AddArc(this._rectBounds.X, this._rectBounds.Bottom - shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, 90f, 90f);
    doubleBracketPath.AddArc(this._rectBounds.X, this._rectBounds.Y, shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, 180f, 90f);
    doubleBracketPath.StartFigure();
    doubleBracketPath.AddArc(this._rectBounds.X + shapeFormula["x2"] - shapeFormula["x1"], this._rectBounds.Y, shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, 270f, 90f);
    doubleBracketPath.AddArc(this._rectBounds.X + shapeFormula["x2"] - shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"] - shapeFormula["x1"], shapeFormula["x1"] * 2f, shapeFormula["x1"] * 2f, 0.0f, 90f);
    return doubleBracketPath;
  }

  internal PdfPath GetDoubleBracePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.DoubleBrace);
    PdfPath doubleBracePath = new PdfPath();
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

  internal PdfPath GetLeftBracketPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftBracket);
    PdfPath leftBracketPath = new PdfPath();
    leftBracketPath.AddArc(this._rectBounds.Right - this._rectBounds.Width, this._rectBounds.Bottom - shapeFormula["y1"] * 2f, this._rectBounds.Width * 2f, shapeFormula["y1"] * 2f, 90f, 90f);
    leftBracketPath.AddArc(this._rectBounds.Right - this._rectBounds.Width, this._rectBounds.Y, this._rectBounds.Width * 2f, shapeFormula["y1"] * 2f, 180f, 90f);
    return leftBracketPath;
  }

  internal PdfPath GetRightBracketPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RightBracket);
    PdfPath rightBracketPath = new PdfPath();
    rightBracketPath.AddArc(this._rectBounds.X - this._rectBounds.Width, this._rectBounds.Y, this._rectBounds.Width * 2f, shapeFormula["y1"] * 2f, 270f, 90f);
    rightBracketPath.AddArc(this._rectBounds.X - this._rectBounds.Width, this._rectBounds.Y + shapeFormula["y2"] - shapeFormula["y1"], this._rectBounds.Width * 2f, shapeFormula["y1"] * 2f, 0.0f, 90f);
    return rightBracketPath;
  }

  internal PdfPath GetLeftBracePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftBrace);
    PdfPath leftBracePath = new PdfPath();
    leftBracePath.AddArc(this._rectBounds.Right - this._rectBounds.Width / 2f, this._rectBounds.Bottom - shapeFormula["y1"] * 2f, this._rectBounds.Width, shapeFormula["y1"] * 2f, 90f, 90f);
    leftBracePath.AddArc(this._rectBounds.X - this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["y4"] - shapeFormula["y1"], this._rectBounds.Width, shapeFormula["y1"] * 2f, 0.0f, -90f);
    leftBracePath.AddArc(this._rectBounds.X - this._rectBounds.Width / 2f, (float) ((double) this._rectBounds.Y + (double) shapeFormula["y4"] - (double) shapeFormula["y1"] * 3.0), this._rectBounds.Width, shapeFormula["y1"] * 2f, 90f, -90f);
    leftBracePath.AddArc(this._rectBounds.Right - this._rectBounds.Width / 2f, this._rectBounds.Y, this._rectBounds.Width, shapeFormula["y1"] * 2f, 180f, 90f);
    return leftBracePath;
  }

  internal PdfPath GetRightBracePath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RightBrace);
    PdfPath rightBracePath = new PdfPath();
    rightBracePath.AddArc(this._rectBounds.X - this._rectBounds.Width / 2f, this._rectBounds.Top, this._rectBounds.Width, shapeFormula["y1"] * 2f, 270f, 90f);
    PointF pointF = new PointF(this._rectBounds.X + this._rectBounds.Width, this._rectBounds.Y + shapeFormula["y2"] - shapeFormula["y1"]);
    rightBracePath.AddArc(pointF.X - this._rectBounds.Width / 2f, pointF.Y, this._rectBounds.Width, shapeFormula["y1"] * 2f, 180f, -90f);
    rightBracePath.AddArc(pointF.X - this._rectBounds.Width / 2f, pointF.Y + shapeFormula["y1"] * 2f, this._rectBounds.Width, shapeFormula["y1"] * 2f, 270f, -90f);
    rightBracePath.AddArc(this._rectBounds.X - this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["y4"] - shapeFormula["y1"], this._rectBounds.Width, shapeFormula["y1"] * 2f, 0.0f, 90f);
    return rightBracePath;
  }

  internal PdfPath GetRightArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RightArrow);
    PdfPath rightArrowPath = new PdfPath();
    PointF[] linePoints = new PointF[7]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"])
    };
    rightArrowPath.AddLines(linePoints);
    rightArrowPath.CloseFigure();
    return rightArrowPath;
  }

  internal PdfPath GetLeftArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftArrow);
    PdfPath leftArrowPath = new PdfPath();
    PointF[] linePoints = new PointF[7]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom)
    };
    leftArrowPath.AddLines(linePoints);
    leftArrowPath.CloseFigure();
    return leftArrowPath;
  }

  internal PdfPath GetUpArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.UpArrow);
    PdfPath upArrowPath = new PdfPath();
    PointF[] linePoints = new PointF[7]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"])
    };
    upArrowPath.AddLines(linePoints);
    upArrowPath.CloseFigure();
    return upArrowPath;
  }

  internal PdfPath GetDownArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.DownArrow);
    PdfPath downArrowPath = new PdfPath();
    PointF[] linePoints = new PointF[7]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Bottom)
    };
    downArrowPath.AddLines(linePoints);
    downArrowPath.CloseFigure();
    return downArrowPath;
  }

  internal PdfPath GetLeftRightArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftRightArrow);
    PdfPath leftRightArrowPath = new PdfPath();
    PointF[] linePoints = new PointF[10]
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
    leftRightArrowPath.AddLines(linePoints);
    leftRightArrowPath.CloseFigure();
    return leftRightArrowPath;
  }

  internal PdfPath GetCurvedRightArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedRightArrow);
    PdfPath curvedRightArrowPath = new PdfPath();
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

  internal PdfPath GetCurvedLeftArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedLeftArrow);
    PdfPath curvedLeftArrowPath = new PdfPath();
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

  internal PdfPath GetCurvedUpArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedUpArrow);
    PdfPath curvedUpArrowPath = new PdfPath();
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

  internal PdfPath GetCurvedDownArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedDownArrow);
    PdfPath curvedDownArrowPath = new PdfPath();
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
    PointF[] linePoints = new PointF[1]
    {
      new PointF(this._rectBounds.X + sizeF.Width, this._rectBounds.Y)
    };
    curvedDownArrowPath.AddLines(linePoints);
    return curvedDownArrowPath;
  }

  internal PdfPath GetUpDownArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.UpDownArrow);
    PdfPath upDownArrowPath = new PdfPath();
    PointF[] linePoints = new PointF[10]
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
    upDownArrowPath.AddLines(linePoints);
    upDownArrowPath.CloseFigure();
    return upDownArrowPath;
  }

  internal PdfPath GetQuadArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.QuadArrow);
    PdfPath quadArrowPath = new PdfPath();
    PointF[] linePoints = new PointF[24]
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
    quadArrowPath.AddLines(linePoints);
    quadArrowPath.CloseFigure();
    return quadArrowPath;
  }

  internal PdfPath GetLeftRightUpArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftRightUpArrow);
    PdfPath rightUpArrowPath = new PdfPath();
    PointF[] linePoints = new PointF[17]
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
    rightUpArrowPath.AddLines(linePoints);
    rightUpArrowPath.CloseFigure();
    return rightUpArrowPath;
  }

  internal PdfPath GetBentArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.BentArrow);
    PdfPath bentArrowPath = new PdfPath();
    PointF[] linePoints = new PointF[6];
    bentArrowPath.AddLine(this._rectBounds.X, this._rectBounds.Bottom, this._rectBounds.X, this._rectBounds.Y + shapeFormula["y5"]);
    bentArrowPath.AddArc(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y5"] - shapeFormula["bd"], shapeFormula["bd"] * 2f, shapeFormula["bd"] * 2f, 180f, 90f);
    linePoints[0] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["dh2"]);
    linePoints[1] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y);
    linePoints[2] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["aw2"]);
    linePoints[3] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y4"]);
    linePoints[4] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y3"]);
    linePoints[5] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]);
    bentArrowPath.AddLines(linePoints);
    if ((double) shapeFormula["bd2"] == 0.0)
      shapeFormula["bd2"] = 1f;
    bentArrowPath.AddArc(linePoints[5].X - shapeFormula["bd2"], linePoints[5].Y, shapeFormula["bd2"] * 2f, shapeFormula["bd2"] * 2f, 270f, -90f);
    bentArrowPath.AddLine(linePoints[5].X - shapeFormula["bd2"], linePoints[5].Y + shapeFormula["bd2"], this._rectBounds.X + shapeFormula["th"], this._rectBounds.Bottom);
    bentArrowPath.CloseFigure();
    return bentArrowPath;
  }

  internal PdfPath GetUTrunArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.UTurnArrow);
    if ((double) shapeFormula["bd2"] == 0.0)
      shapeFormula["bd2"] = 1f;
    PdfPath utrunArrowPath = new PdfPath();
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

  internal PdfPath GetLeftUpArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftUpArrow);
    PdfPath leftUpArrowPath = new PdfPath();
    PointF[] linePoints = new PointF[12]
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
    leftUpArrowPath.AddLines(linePoints);
    leftUpArrowPath.CloseFigure();
    return leftUpArrowPath;
  }

  internal PdfPath GetBentUpArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.BentUpArrow);
    PdfPath bentUpArrowPath = new PdfPath();
    PointF[] linePoints = new PointF[9]
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
    bentUpArrowPath.AddLines(linePoints);
    bentUpArrowPath.CloseFigure();
    return bentUpArrowPath;
  }

  internal PdfPath GetStripedRightArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.StripedRightArrow);
    PdfPath stripedRightArrowPath = new PdfPath();
    PointF[] linePoints1 = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 32f, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 32f, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"])
    };
    stripedRightArrowPath.AddLines(linePoints1);
    stripedRightArrowPath.CloseFigure();
    PointF[] linePoints2 = new PointF[4]
    {
      new PointF(this._rectBounds.X + Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 16f, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 8f, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 8f, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 16f, this._rectBounds.Y + shapeFormula["y2"])
    };
    stripedRightArrowPath.AddLines(linePoints2);
    stripedRightArrowPath.CloseFigure();
    PointF[] linePoints3 = new PointF[7]
    {
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y2"])
    };
    stripedRightArrowPath.AddLines(linePoints3);
    stripedRightArrowPath.CloseFigure();
    return stripedRightArrowPath;
  }

  internal PdfPath GetNotchedRightArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.NotchedRightArrow);
    PdfPath notchedRightArrowPath = new PdfPath();
    PointF[] linePoints = new PointF[8]
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
    notchedRightArrowPath.AddLines(linePoints);
    notchedRightArrowPath.CloseFigure();
    return notchedRightArrowPath;
  }

  internal PdfPath GetPentagonPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Pentagon);
    PdfPath pentagonPath = new PdfPath();
    PointF[] linePoints = new PointF[5]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    pentagonPath.AddLines(linePoints);
    pentagonPath.CloseFigure();
    return pentagonPath;
  }

  internal PdfPath GetChevronPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Chevron);
    PdfPath chevronPath = new PdfPath();
    PointF[] linePoints = new PointF[6]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + this._rectBounds.Height / 2f),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + this._rectBounds.Height / 2f)
    };
    chevronPath.AddLines(linePoints);
    chevronPath.CloseFigure();
    return chevronPath;
  }

  internal PdfPath GetRightArrowCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RightArrowCallout);
    PdfPath arrowCalloutPath = new PdfPath();
    PointF[] linePoints = new PointF[11]
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
    arrowCalloutPath.AddLines(linePoints);
    arrowCalloutPath.CloseFigure();
    return arrowCalloutPath;
  }

  internal PdfPath GetDownArrowCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.DownArrowCallout);
    PdfPath arrowCalloutPath = new PdfPath();
    PointF[] linePoints = new PointF[11]
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
    arrowCalloutPath.AddLines(linePoints);
    arrowCalloutPath.CloseFigure();
    return arrowCalloutPath;
  }

  internal PdfPath GetLeftArrowCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftArrowCallout);
    PdfPath arrowCalloutPath = new PdfPath();
    PointF[] linePoints = new PointF[11]
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
    arrowCalloutPath.AddLines(linePoints);
    arrowCalloutPath.CloseFigure();
    return arrowCalloutPath;
  }

  internal PdfPath GetUpArrowCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.UpArrowCallout);
    PdfPath arrowCalloutPath = new PdfPath();
    PointF[] linePoints = new PointF[11]
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
    arrowCalloutPath.AddLines(linePoints);
    arrowCalloutPath.CloseFigure();
    return arrowCalloutPath;
  }

  internal PdfPath GetLeftRightArrowCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftRightArrowCallout);
    PdfPath arrowCalloutPath = new PdfPath();
    PointF[] linePoints = new PointF[18]
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
    arrowCalloutPath.AddLines(linePoints);
    arrowCalloutPath.CloseFigure();
    return arrowCalloutPath;
  }

  internal PdfPath GetQuadArrowCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.QuadArrowCallout);
    PdfPath arrowCalloutPath = new PdfPath();
    PointF[] linePoints = new PointF[32 /*0x20*/]
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
    arrowCalloutPath.AddLines(linePoints);
    arrowCalloutPath.CloseFigure();
    return arrowCalloutPath;
  }

  internal PdfPath GetCircularArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CircularArrow);
    PdfPath circularArrowPath = new PdfPath();
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

  internal PdfPath GetLeftCircularArrowPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LeftCircularArrow);
    PdfPath circularArrowPath = new PdfPath();
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

  internal PdfPath GetMathPlusPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.MathPlus);
    PdfPath mathPlusPath = new PdfPath();
    PointF[] linePoints = new PointF[12]
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
    mathPlusPath.AddLines(linePoints);
    mathPlusPath.CloseFigure();
    return mathPlusPath;
  }

  internal PdfPath GetMathMinusPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.MathMinus);
    PdfPath mathMinusPath = new PdfPath();
    PointF[] linePoints = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"])
    };
    mathMinusPath.AddLines(linePoints);
    mathMinusPath.CloseFigure();
    return mathMinusPath;
  }

  internal PdfPath GetMathMultiplyPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.MathMultiply);
    PdfPath mathMultiplyPath = new PdfPath();
    PointF[] linePoints = new PointF[12]
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
    mathMultiplyPath.AddLines(linePoints);
    mathMultiplyPath.CloseFigure();
    return mathMultiplyPath;
  }

  internal PdfPath GetMathDivisionPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.MathDivision);
    PdfPath mathDivisionPath = new PdfPath();
    PointF[] linePoints = new PointF[4];
    mathDivisionPath.AddEllipse(this._rectBounds.X + this._rectBounds.Width / 2f - shapeFormula["rad"], this._rectBounds.Y + shapeFormula["y1"], shapeFormula["rad"] * 2f, shapeFormula["rad"] * 2f);
    mathDivisionPath.CloseFigure();
    mathDivisionPath.AddEllipse(this._rectBounds.X + this._rectBounds.Width / 2f - shapeFormula["rad"], (float) ((double) this._rectBounds.Y + (double) shapeFormula["y5"] - (double) shapeFormula["rad"] * 2.0), shapeFormula["rad"] * 2f, shapeFormula["rad"] * 2f);
    mathDivisionPath.CloseFigure();
    linePoints[0] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"]);
    linePoints[1] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y3"]);
    linePoints[2] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y4"]);
    linePoints[3] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"]);
    mathDivisionPath.AddLines(linePoints);
    mathDivisionPath.CloseFigure();
    return mathDivisionPath;
  }

  internal PdfPath GetMathEqualPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.MathEqual);
    PdfPath mathEqualPath = new PdfPath();
    PointF[] linePoints1 = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"])
    };
    mathEqualPath.AddLines(linePoints1);
    mathEqualPath.CloseFigure();
    PointF[] linePoints2 = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"])
    };
    mathEqualPath.AddLines(linePoints2);
    mathEqualPath.CloseFigure();
    return mathEqualPath;
  }

  internal PdfPath GetMathNotEqualPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.MathNotEqual);
    PdfPath mathNotEqualPath = new PdfPath();
    PointF[] linePoints = new PointF[20]
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
    mathNotEqualPath.AddLines(linePoints);
    mathNotEqualPath.CloseFigure();
    return mathNotEqualPath;
  }

  internal PdfPath GetFlowChartAlternateProcessPath()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.FlowChartAlternateProcess);
    PdfPath alternateProcessPath = new PdfPath();
    float num = this._formulaValues.GetPresetOperandValue("ssd6") * 2f;
    alternateProcessPath.AddArc(this._rectBounds.X, this._rectBounds.Y, num, num, 180f, 90f);
    alternateProcessPath.AddArc(this._rectBounds.Right - num, this._rectBounds.Y, num, num, 270f, 90f);
    alternateProcessPath.AddArc(this._rectBounds.Right - num, this._rectBounds.Bottom - num, num, num, 0.0f, 90f);
    alternateProcessPath.AddArc(this._rectBounds.X, this._rectBounds.Bottom - num, num, num, 90f, 90f);
    alternateProcessPath.CloseFigure();
    return alternateProcessPath;
  }

  internal PdfPath GetFlowChartPredefinedProcessPath()
  {
    PdfPath predefinedProcessPath = new PdfPath();
    predefinedProcessPath.AddRectangle(this._rectBounds);
    predefinedProcessPath.AddLine(this._rectBounds.X + this._rectBounds.Width / 8f, this._rectBounds.Y, this._rectBounds.X + this._rectBounds.Width / 8f, this._rectBounds.Bottom);
    predefinedProcessPath.CloseFigure();
    predefinedProcessPath.AddLine(this._rectBounds.Right - this._rectBounds.Width / 8f, this._rectBounds.Y, this._rectBounds.Right - this._rectBounds.Width / 8f, this._rectBounds.Bottom);
    predefinedProcessPath.CloseFigure();
    return predefinedProcessPath;
  }

  internal PdfPath GetFlowChartInternalStoragePath()
  {
    PdfPath internalStoragePath = new PdfPath();
    internalStoragePath.AddRectangle(this._rectBounds);
    internalStoragePath.AddLine(this._rectBounds.X + this._rectBounds.Width / 8f, this._rectBounds.Y, this._rectBounds.X + this._rectBounds.Width / 8f, this._rectBounds.Bottom);
    internalStoragePath.CloseFigure();
    internalStoragePath.AddLine(this._rectBounds.X, this._rectBounds.Y + this._rectBounds.Height / 8f, this._rectBounds.Right, this._rectBounds.Top + this._rectBounds.Height / 8f);
    internalStoragePath.CloseFigure();
    return internalStoragePath;
  }

  internal PdfPath GetFlowChartDocumentPath()
  {
    PdfPath chartDocumentPath = new PdfPath();
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

  internal PdfPath GetFlowChartMultiDocumentPath()
  {
    PdfPath multiDocumentPath = new PdfPath();
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

  internal PdfPath GetFlowChartTerminatorPath()
  {
    PdfPath chartTerminatorPath = new PdfPath();
    SizeF sizeF = new SizeF((float) ((double) this._rectBounds.Width * 3475.0 / 21600.0 * 2.0), (float) ((double) this._rectBounds.Height * 10800.0 / 21600.0 * 2.0));
    chartTerminatorPath.AddLine(this.GetXYPosition(3475f, 0.0f, 21600f), this.GetXYPosition(18125f, 0.0f, 21600f));
    chartTerminatorPath.StartFigure();
    PointF xyPosition1 = this.GetXYPosition(18125f, 0.0f, 21600f);
    RectangleF rectangle = new RectangleF(xyPosition1.X - sizeF.Width / 2f, xyPosition1.Y, sizeF.Width, sizeF.Height);
    chartTerminatorPath.AddArc(rectangle, 270f, 180f);
    chartTerminatorPath.AddLine(new PointF(rectangle.X, rectangle.Y + rectangle.Height), this.GetXYPosition(3475f, 21600f, 21600f));
    PointF xyPosition2 = this.GetXYPosition(3475f, 0.0f, 21600f);
    rectangle = new RectangleF(xyPosition2.X - sizeF.Width / 2f, xyPosition2.Y, sizeF.Width, sizeF.Height);
    chartTerminatorPath.AddArc(rectangle, 90f, 180f);
    chartTerminatorPath.CloseFigure();
    return chartTerminatorPath;
  }

  internal PdfPath GetFlowChartPreparationPath()
  {
    PdfPath chartPreparationPath = new PdfPath();
    chartPreparationPath.AddLine(this.GetXYPosition(0.0f, 5f, 10f), this.GetXYPosition(2f, 0.0f, 10f));
    chartPreparationPath.AddLine(this.GetXYPosition(2f, 0.0f, 10f), this.GetXYPosition(8f, 0.0f, 10f));
    chartPreparationPath.AddLine(this.GetXYPosition(8f, 0.0f, 10f), this.GetXYPosition(10f, 5f, 10f));
    chartPreparationPath.AddLine(this.GetXYPosition(10f, 5f, 10f), this.GetXYPosition(8f, 10f, 10f));
    chartPreparationPath.AddLine(this.GetXYPosition(8f, 10f, 10f), this.GetXYPosition(2f, 10f, 10f));
    chartPreparationPath.CloseFigure();
    return chartPreparationPath;
  }

  internal PdfPath GetFlowChartManualInputPath()
  {
    PdfPath chartManualInputPath = new PdfPath();
    chartManualInputPath.AddLine(this.GetXYPosition(0.0f, 1f, 5f), this.GetXYPosition(5f, 0.0f, 5f));
    chartManualInputPath.AddLine(this.GetXYPosition(5f, 0.0f, 5f), this.GetXYPosition(5f, 5f, 5f));
    chartManualInputPath.AddLine(this.GetXYPosition(5f, 5f, 5f), this.GetXYPosition(0.0f, 5f, 5f));
    chartManualInputPath.CloseFigure();
    return chartManualInputPath;
  }

  internal PdfPath GetFlowChartManualOperationPath()
  {
    PdfPath manualOperationPath = new PdfPath();
    manualOperationPath.AddLine(this.GetXYPosition(0.0f, 0.0f, 5f), this.GetXYPosition(5f, 0.0f, 5f));
    manualOperationPath.AddLine(this.GetXYPosition(5f, 0.0f, 5f), this.GetXYPosition(4f, 5f, 5f));
    manualOperationPath.AddLine(this.GetXYPosition(4f, 5f, 5f), this.GetXYPosition(1f, 5f, 5f));
    manualOperationPath.CloseFigure();
    return manualOperationPath;
  }

  internal PdfPath GetFlowChartConnectorPath()
  {
    PdfPath chartConnectorPath = new PdfPath();
    RectangleF rectangle = new RectangleF(new PointF(this._rectBounds.X, this._rectBounds.Y), new SizeF(this._rectBounds.Width, this._rectBounds.Height));
    chartConnectorPath.AddArc(rectangle, 180f, 90f);
    chartConnectorPath.AddArc(rectangle, 270f, 90f);
    chartConnectorPath.AddArc(rectangle, 0.0f, 90f);
    chartConnectorPath.AddArc(rectangle, 90f, 90f);
    return chartConnectorPath;
  }

  internal PdfPath GetFlowChartOffPageConnectorPath()
  {
    PdfPath pageConnectorPath = new PdfPath();
    pageConnectorPath.AddLine(this.GetXYPosition(0.0f, 0.0f, 10f), this.GetXYPosition(10f, 0.0f, 10f));
    pageConnectorPath.AddLine(this.GetXYPosition(10f, 0.0f, 10f), this.GetXYPosition(10f, 8f, 10f));
    pageConnectorPath.AddLine(this.GetXYPosition(10f, 8f, 10f), this.GetXYPosition(5f, 10f, 10f));
    pageConnectorPath.AddLine(this.GetXYPosition(5f, 10f, 10f), this.GetXYPosition(0.0f, 8f, 10f));
    pageConnectorPath.CloseFigure();
    return pageConnectorPath;
  }

  internal PdfPath GetFlowChartCardPath()
  {
    PdfPath flowChartCardPath = new PdfPath();
    flowChartCardPath.AddLine(this.GetXYPosition(0.0f, 1f, 5f), this.GetXYPosition(1f, 0.0f, 5f));
    flowChartCardPath.AddLine(this.GetXYPosition(1f, 0.0f, 5f), this.GetXYPosition(5f, 0.0f, 5f));
    flowChartCardPath.AddLine(this.GetXYPosition(5f, 0.0f, 5f), this.GetXYPosition(5f, 5f, 5f));
    flowChartCardPath.AddLine(this.GetXYPosition(5f, 5f, 5f), this.GetXYPosition(0.0f, 5f, 5f));
    flowChartCardPath.CloseFigure();
    return flowChartCardPath;
  }

  internal PdfPath GetFlowChartPunchedTapePath()
  {
    PdfPath chartPunchedTapePath = new PdfPath();
    RectangleF rectangle = new RectangleF(this.GetXYPosition(0.0f, 2f, 20f), new SizeF((float) ((double) this._rectBounds.Width * 5.0 / 20.0 * 2.0), (float) ((double) this._rectBounds.Height * 2.0 / 20.0 * 2.0)));
    chartPunchedTapePath.AddArc(rectangle, 180f, -180f);
    PointF location1 = new PointF(rectangle.X + rectangle.Width, rectangle.Y);
    rectangle = new RectangleF(location1, new SizeF((float) ((double) this._rectBounds.Width * 5.0 / 20.0 * 2.0), (float) ((double) this._rectBounds.Height * 2.0 / 20.0 * 2.0)));
    chartPunchedTapePath.AddArc(rectangle, 180f, 180f);
    chartPunchedTapePath.AddLine(new PointF(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), this.GetXYPosition(20f, 18f, 20f));
    ref RectangleF local = ref rectangle;
    PointF xyPosition = this.GetXYPosition(20f, 18f, 20f);
    double x = (double) xyPosition.X - (double) rectangle.Width;
    xyPosition = this.GetXYPosition(20f, 18f, 20f);
    double y = (double) xyPosition.Y;
    PointF location2 = new PointF((float) x, (float) y);
    SizeF size = new SizeF((float) ((double) this._rectBounds.Width * 5.0 / 20.0 * 2.0), (float) ((double) this._rectBounds.Height * 2.0 / 20.0 * 2.0));
    local = new RectangleF(location2, size);
    chartPunchedTapePath.AddArc(rectangle, 0.0f, -180f);
    location1 = new PointF(rectangle.X - rectangle.Width, rectangle.Y);
    rectangle = new RectangleF(location1, new SizeF((float) ((double) this._rectBounds.Width * 5.0 / 20.0 * 2.0), (float) ((double) this._rectBounds.Height * 2.0 / 20.0 * 2.0)));
    chartPunchedTapePath.AddArc(rectangle, 0.0f, 180f);
    chartPunchedTapePath.CloseFigure();
    return chartPunchedTapePath;
  }

  internal PdfPath GetFlowChartSummingJunctionPath()
  {
    PdfPath summingJunctionPath = new PdfPath();
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

  internal PdfPath GetFlowChartOrPath()
  {
    PdfPath flowChartOrPath = new PdfPath();
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

  internal PdfPath GetFlowChartCollatePath()
  {
    PdfPath chartCollatePath = new PdfPath();
    chartCollatePath.AddLine(this.GetXYPosition(0.0f, 0.0f, 2f), this.GetXYPosition(2f, 0.0f, 2f));
    chartCollatePath.AddLine(this.GetXYPosition(2f, 0.0f, 2f), this.GetXYPosition(1f, 1f, 2f));
    chartCollatePath.AddLine(this.GetXYPosition(1f, 1f, 2f), this.GetXYPosition(2f, 2f, 2f));
    chartCollatePath.AddLine(this.GetXYPosition(2f, 2f, 2f), this.GetXYPosition(0.0f, 2f, 2f));
    chartCollatePath.AddLine(this.GetXYPosition(0.0f, 2f, 2f), this.GetXYPosition(1f, 1f, 2f));
    chartCollatePath.CloseFigure();
    return chartCollatePath;
  }

  internal PdfPath GetFlowChartSortPath()
  {
    PdfPath flowChartSortPath = new PdfPath();
    flowChartSortPath.AddLine(this.GetXYPosition(0.0f, 1f, 2f), this.GetXYPosition(2f, 1f, 2f));
    flowChartSortPath.AddLine(this.GetXYPosition(2f, 1f, 2f), this.GetXYPosition(0.0f, 1f, 2f));
    flowChartSortPath.AddLine(this.GetXYPosition(0.0f, 1f, 2f), this.GetXYPosition(1f, 0.0f, 2f));
    flowChartSortPath.AddLine(this.GetXYPosition(1f, 0.0f, 2f), this.GetXYPosition(2f, 1f, 2f));
    flowChartSortPath.AddLine(this.GetXYPosition(2f, 1f, 2f), this.GetXYPosition(1f, 2f, 2f));
    flowChartSortPath.CloseFigure();
    return flowChartSortPath;
  }

  internal PdfPath GetFlowChartExtractPath()
  {
    PdfPath chartExtractPath = new PdfPath();
    chartExtractPath.AddLine(this.GetXYPosition(0.0f, 2f, 2f), this.GetXYPosition(1f, 0.0f, 2f));
    chartExtractPath.AddLine(this.GetXYPosition(1f, 0.0f, 2f), this.GetXYPosition(2f, 2f, 2f));
    chartExtractPath.CloseFigure();
    return chartExtractPath;
  }

  internal PdfPath GetFlowChartMergePath()
  {
    PdfPath flowChartMergePath = new PdfPath();
    flowChartMergePath.AddLine(this.GetXYPosition(0.0f, 0.0f, 2f), this.GetXYPosition(2f, 0.0f, 2f));
    flowChartMergePath.AddLine(this.GetXYPosition(2f, 0.0f, 2f), this.GetXYPosition(1f, 2f, 2f));
    flowChartMergePath.CloseFigure();
    return flowChartMergePath;
  }

  internal PdfPath GetFlowChartOnlineStoragePath()
  {
    PdfPath onlineStoragePath = new PdfPath();
    onlineStoragePath.AddLine(this.GetXYPosition(1f, 0.0f, 6f), this.GetXYPosition(6f, 0.0f, 6f));
    SizeF sizeF = new SizeF((float) ((double) this._rectBounds.Width / 6.0 * 2.0), (float) ((double) this._rectBounds.Height / 2.0 * 2.0));
    PointF xyPosition1 = this.GetXYPosition(6f, 0.0f, 6f);
    RectangleF rectangle = new RectangleF(xyPosition1.X - sizeF.Width / 2f, xyPosition1.Y, sizeF.Width, sizeF.Height);
    onlineStoragePath.AddArc(rectangle, 270f, -180f);
    onlineStoragePath.AddLine(new PointF(xyPosition1.X, xyPosition1.Y + sizeF.Height), this.GetXYPosition(1f, 6f, 6f));
    PointF xyPosition2 = this.GetXYPosition(1f, 0.0f, 6f);
    rectangle = new RectangleF(xyPosition2.X - sizeF.Width / 2f, xyPosition2.Y, sizeF.Width, sizeF.Height);
    onlineStoragePath.AddArc(rectangle, 90f, 180f);
    onlineStoragePath.CloseFigure();
    return onlineStoragePath;
  }

  internal PdfPath GetFlowChartDelayPath()
  {
    PdfPath flowChartDelayPath = new PdfPath();
    flowChartDelayPath.AddLine(new PointF(this._rectBounds.X, this._rectBounds.Y), new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y));
    SizeF sizeF = new SizeF(this._rectBounds.Width, this._rectBounds.Height);
    PointF pointF = new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y);
    RectangleF rectangle = new RectangleF(pointF.X - sizeF.Width / 2f, pointF.Y, sizeF.Width, sizeF.Height);
    flowChartDelayPath.AddArc(rectangle, 270f, 180f);
    flowChartDelayPath.AddLine(new PointF(pointF.X, pointF.Y + sizeF.Height), new PointF(this._rectBounds.X, this._rectBounds.Bottom));
    flowChartDelayPath.CloseFigure();
    return flowChartDelayPath;
  }

  internal PdfPath GetFlowChartSequentialAccessStoragePath()
  {
    PdfPath accessStoragePath = new PdfPath();
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.FlowChartSequentialAccessStorage);
    SizeF sizeF = new SizeF(this._rectBounds.Width, this._rectBounds.Height);
    RectangleF rectangle = new RectangleF(this._rectBounds.X, this._rectBounds.Y, sizeF.Width, sizeF.Height);
    accessStoragePath.AddArc(rectangle, 90f, 90f);
    accessStoragePath.AddArc(rectangle, 180f, 90f);
    accessStoragePath.AddArc(rectangle, 270f, 90f);
    accessStoragePath.AddArc(rectangle, 0.0f, shapeFormula["ang1"]);
    accessStoragePath.AddLine(new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["ib"]), new PointF(this._rectBounds.Right, this._rectBounds.Bottom));
    accessStoragePath.CloseFigure();
    return accessStoragePath;
  }

  internal PdfPath GetFlowChartMagneticDiskPath()
  {
    PdfPath magneticDiskPath = new PdfPath();
    SizeF sizeF = new SizeF(this._rectBounds.Width, this._rectBounds.Height / 3f);
    PointF xyPosition1 = this.GetXYPosition(6f, 1f, 6f);
    RectangleF rectangle = new RectangleF(xyPosition1.X - sizeF.Width, xyPosition1.Y - sizeF.Height / 2f, sizeF.Width, sizeF.Height);
    magneticDiskPath.AddArc(rectangle, 0.0f, 180f);
    magneticDiskPath.StartFigure();
    PointF xyPosition2 = this.GetXYPosition(0.0f, 1f, 6f);
    rectangle = new RectangleF(xyPosition2.X, xyPosition2.Y - sizeF.Height / 2f, sizeF.Width, sizeF.Height);
    magneticDiskPath.AddArc(rectangle, 180f, 180f);
    magneticDiskPath.AddLine(new PointF(rectangle.X + rectangle.Width, rectangle.Y + sizeF.Height), this.GetXYPosition(6f, 5f, 6f));
    PointF xyPosition3 = this.GetXYPosition(6f, 5f, 6f);
    rectangle = new RectangleF(xyPosition3.X - sizeF.Width, xyPosition3.Y - sizeF.Height / 2f, sizeF.Width, sizeF.Height);
    magneticDiskPath.AddArc(rectangle, 0.0f, 180f);
    magneticDiskPath.CloseFigure();
    return magneticDiskPath;
  }

  internal PdfPath GetFlowChartDirectAccessStoragePath()
  {
    PdfPath accessStoragePath = new PdfPath();
    SizeF sizeF = new SizeF(this._rectBounds.Width / 3f, this._rectBounds.Height);
    PointF xyPosition1 = this.GetXYPosition(5f, 6f, 6f);
    RectangleF rectangle = new RectangleF(xyPosition1.X - sizeF.Width / 2f, xyPosition1.Y - sizeF.Height, sizeF.Width, sizeF.Height);
    accessStoragePath.AddArc(rectangle, 90f, 180f);
    accessStoragePath.StartFigure();
    accessStoragePath.AddLine(this.GetXYPosition(1f, 0.0f, 6f), this.GetXYPosition(5f, 0.0f, 6f));
    PointF xyPosition2 = this.GetXYPosition(5f, 0.0f, 6f);
    rectangle = new RectangleF(xyPosition2.X - sizeF.Width / 2f, xyPosition2.Y, sizeF.Width, sizeF.Height);
    accessStoragePath.AddArc(rectangle, 270f, 180f);
    accessStoragePath.AddLine(new PointF(rectangle.X, rectangle.Y + sizeF.Height), this.GetXYPosition(1f, 6f, 6f));
    PointF xyPosition3 = this.GetXYPosition(1f, 6f, 6f);
    rectangle = new RectangleF(xyPosition3.X - sizeF.Width / 2f, xyPosition3.Y - sizeF.Height, sizeF.Width, sizeF.Height);
    accessStoragePath.StartFigure();
    accessStoragePath.AddArc(rectangle, 90f, 180f);
    return accessStoragePath;
  }

  internal PdfPath GetFlowChartDisplayPath()
  {
    PdfPath chartDisplayPath = new PdfPath();
    chartDisplayPath.AddLine(this.GetXYPosition(0.0f, 3f, 6f), this.GetXYPosition(1f, 0.0f, 6f));
    chartDisplayPath.AddLine(this.GetXYPosition(1f, 0.0f, 6f), this.GetXYPosition(5f, 0.0f, 6f));
    PdfPath pdfPath1 = chartDisplayPath;
    PointF xyPosition1 = this.GetXYPosition(5f, 0.0f, 6f);
    double x1 = (double) xyPosition1.X - (double) this._rectBounds.Width / 6.0;
    xyPosition1 = this.GetXYPosition(5f, 0.0f, 6f);
    double y1 = (double) xyPosition1.Y;
    double width = (double) this._rectBounds.Width / 3.0;
    double height = (double) this._rectBounds.Height;
    pdfPath1.AddArc((float) x1, (float) y1, (float) width, (float) height, 270f, 180f);
    PdfPath pdfPath2 = chartDisplayPath;
    PointF xyPosition2 = this.GetXYPosition(5f, 0.0f, 6f);
    double x2 = (double) xyPosition2.X - (double) this._rectBounds.Width / 6.0;
    xyPosition2 = this.GetXYPosition(5f, 0.0f, 6f);
    double y2 = (double) xyPosition2.Y + (double) this._rectBounds.Height;
    PointF point1 = new PointF((float) x2, (float) y2);
    PointF xyPosition3 = this.GetXYPosition(1f, 6f, 6f);
    pdfPath2.AddLine(point1, xyPosition3);
    chartDisplayPath.CloseFigure();
    return chartDisplayPath;
  }

  internal PdfPath GetExplosion1()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.Explosion1);
    PdfPath explosion1 = new PdfPath();
    PointF[] linePoints = new PointF[24]
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
    explosion1.AddLines(linePoints);
    explosion1.CloseFigure();
    return explosion1;
  }

  internal PdfPath GetExplosion2()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.Explosion2);
    PdfPath explosion2 = new PdfPath();
    PointF[] linePoints = new PointF[28]
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
    explosion2.AddLines(linePoints);
    explosion2.CloseFigure();
    return explosion2;
  }

  internal PdfPath GetStar4Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star4Point);
    PdfPath star4Point = new PdfPath();
    PointF[] linePoints = new PointF[8]
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
    star4Point.AddLines(linePoints);
    star4Point.CloseFigure();
    return star4Point;
  }

  internal PdfPath GetStar5Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star5Point);
    PdfPath star5Point = new PdfPath();
    PointF[] linePoints = new PointF[10]
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
    star5Point.AddLines(linePoints);
    star5Point.CloseFigure();
    return star5Point;
  }

  internal PdfPath GetStar6Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star6Point);
    PdfPath star6Point = new PdfPath();
    PointF[] linePoints = new PointF[12]
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
    star6Point.AddLines(linePoints);
    star6Point.CloseFigure();
    return star6Point;
  }

  internal PdfPath GetStar7Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star7Point);
    PdfPath star7Point = new PdfPath();
    PointF[] linePoints = new PointF[14]
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
    star7Point.AddLines(linePoints);
    star7Point.CloseFigure();
    return star7Point;
  }

  internal PdfPath GetStar8Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star8Point);
    PdfPath star8Point = new PdfPath();
    PointF[] linePoints = new PointF[16 /*0x10*/]
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
    star8Point.AddLines(linePoints);
    star8Point.CloseFigure();
    return star8Point;
  }

  internal PdfPath GetStar10Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star10Point);
    PdfPath star10Point = new PdfPath();
    PointF[] linePoints = new PointF[20]
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
    star10Point.AddLines(linePoints);
    star10Point.CloseFigure();
    return star10Point;
  }

  internal PdfPath GetStar12Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star12Point);
    PdfPath star12Point = new PdfPath();
    PointF[] linePoints = new PointF[24]
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
    star12Point.AddLines(linePoints);
    star12Point.CloseFigure();
    return star12Point;
  }

  internal PdfPath GetStar16Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star16Point);
    PdfPath star16Point = new PdfPath();
    PointF[] linePoints = new PointF[32 /*0x20*/]
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
    star16Point.AddLines(linePoints);
    star16Point.CloseFigure();
    return star16Point;
  }

  internal PdfPath GetStar24Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star24Point);
    PdfPath star24Point = new PdfPath();
    PointF[] linePoints = new PointF[48 /*0x30*/]
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
    star24Point.AddLines(linePoints);
    star24Point.CloseFigure();
    return star24Point;
  }

  internal PdfPath GetStar32Point()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Star32Point);
    PdfPath star32Point = new PdfPath();
    PointF[] linePoints = new PointF[64 /*0x40*/]
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
    star32Point.AddLines(linePoints);
    star32Point.CloseFigure();
    return star32Point;
  }

  internal PdfPath GetUpRibbon()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.UpRibbon);
    PdfPath upRibbon = new PdfPath();
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

  internal PdfPath GetDownRibbon()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.DownRibbon);
    PdfPath downRibbon = new PdfPath();
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

  internal PdfPath GetCurvedUpRibbon()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedUpRibbon);
    PdfPath curvedUpRibbon = new PdfPath();
    PointF[] pointFArray1 = new PointF[5]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + this._rectBounds.Width / 8f, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["q1"]),
      new PointF(this._rectBounds.X + shapeFormula["cx4"], this._rectBounds.Y + shapeFormula["cy4"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y5"])
    };
    PointF[] linePoints1 = new PointF[2]
    {
      pointFArray1[0],
      pointFArray1[1]
    };
    curvedUpRibbon.AddLines(linePoints1);
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
    PointF[] linePoints2 = new PointF[1]{ pointFArray4[0] };
    curvedUpRibbon.AddLines(linePoints2);
    curvedUpRibbon.AddBezier(pointFArray4[1], pointFArray4[1], pointFArray4[2], pointFArray4[3]);
    pointFArray3[0] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y3"]);
    pointFArray3[1] = new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["cy3"]);
    pointFArray3[2] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]);
    PointF[] linePoints3 = new PointF[3]
    {
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y7"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"])
    };
    curvedUpRibbon.AddLines(linePoints3);
    curvedUpRibbon.AddBezier(pointFArray3[0], pointFArray3[0], pointFArray3[1], pointFArray3[2]);
    PointF[] pointFArray5 = new PointF[3]
    {
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["cx1"], this._rectBounds.Y + shapeFormula["cy1"]),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    PointF[] linePoints4 = new PointF[3]
    {
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y7"])
    };
    curvedUpRibbon.AddLines(linePoints4);
    curvedUpRibbon.AddBezier(pointFArray5[0], pointFArray5[0], pointFArray5[1], pointFArray5[2]);
    curvedUpRibbon.CloseFigure();
    PointF[] linePoints5 = new PointF[2]
    {
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y5"])
    };
    curvedUpRibbon.AddLines(linePoints5);
    curvedUpRibbon.CloseFigure();
    linePoints5[0] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y5"]);
    linePoints5[1] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y3"]);
    curvedUpRibbon.AddLines(linePoints5);
    curvedUpRibbon.CloseFigure();
    return curvedUpRibbon;
  }

  internal PdfPath GetCurvedDownRibbon()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CurvedDownRibbon);
    PdfPath curvedDownRibbon = new PdfPath();
    PointF[] pointFArray1 = new PointF[3]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["cx1"], this._rectBounds.Y + shapeFormula["cy1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"])
    };
    curvedDownRibbon.AddBezier(pointFArray1[0], pointFArray1[0], pointFArray1[1], pointFArray1[2]);
    PointF[] linePoints1 = new PointF[3]
    {
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y7"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"])
    };
    curvedDownRibbon.AddLines(linePoints1);
    pointFArray1[0] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]);
    pointFArray1[1] = new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["cy3"]);
    pointFArray1[2] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y3"]);
    curvedDownRibbon.AddBezier(pointFArray1[0], pointFArray1[0], pointFArray1[1], pointFArray1[2]);
    PointF[] linePoints2 = new PointF[3]
    {
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y7"])
    };
    curvedDownRibbon.AddLines(linePoints2);
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
    PointF[] linePoints3 = new PointF[1]{ pointFArray2[0] };
    curvedDownRibbon.AddLines(linePoints3);
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
    linePoints3[0] = pointFArray1[0];
    curvedDownRibbon.AddLines(linePoints3);
    curvedDownRibbon.CloseFigure();
    PointF[] linePoints4 = new PointF[2]
    {
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"])
    };
    curvedDownRibbon.AddLines(linePoints4);
    curvedDownRibbon.CloseFigure();
    linePoints4[0] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y3"]);
    linePoints4[1] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y5"]);
    curvedDownRibbon.AddLines(linePoints4);
    curvedDownRibbon.CloseFigure();
    return curvedDownRibbon;
  }

  internal PdfPath GetVerticalScroll()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.VerticalScroll);
    PdfPath verticalScroll = new PdfPath();
    PointF[] pointFArray1 = new PointF[2]
    {
      new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y)
    };
    PointF[] linePoints1 = new PointF[1]{ pointFArray1[0] };
    verticalScroll.AddLines(linePoints1);
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
    PointF[] linePoints2 = new PointF[1]{ pointFArray3[0] };
    verticalScroll.AddLines(linePoints2);
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
    PointF[] linePoints3 = new PointF[1]
    {
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ch2"])
    };
    verticalScroll.AddLines(linePoints3);
    PointF[] linePoints4 = new PointF[2];
    verticalScroll.StartFigure();
    linePoints4[0] = new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["ch"]);
    linePoints4[1] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["ch"]);
    verticalScroll.AddLines(linePoints4);
    verticalScroll.CloseFigure();
    PointF[] pointFArray6 = new PointF[1]
    {
      new PointF(this._rectBounds.X + shapeFormula["ch2"], this._rectBounds.Y + shapeFormula["y3"])
    };
    verticalScroll.StartFigure();
    verticalScroll.AddArc(pointFArray6[0].X - shapeFormula["ch2"] / 2f, pointFArray6[0].Y, shapeFormula["ch4"] * 2f, shapeFormula["ch4"] * 2f, 270f, 180f);
    PointF[] linePoints5 = new PointF[1]
    {
      new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y + shapeFormula["y4"])
    };
    verticalScroll.AddLines(linePoints5);
    PointF[] pointFArray7 = new PointF[1]
    {
      new PointF(this._rectBounds.X + shapeFormula["ch2"], this._rectBounds.Bottom)
    };
    verticalScroll.StartFigure();
    verticalScroll.AddArc(pointFArray7[0].X - shapeFormula["ch2"], pointFArray7[0].Y - shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 90f, -90f);
    PointF[] linePoints6 = new PointF[1]
    {
      new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y + shapeFormula["y3"])
    };
    verticalScroll.AddLines(linePoints6);
    return verticalScroll;
  }

  internal PdfPath[] GetHorizontalScroll()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.HorizontalScroll);
    PdfPath[] horizontalScroll = new PdfPath[7];
    for (int index = 0; index < horizontalScroll.Length; ++index)
      horizontalScroll[index] = new PdfPath();
    PointF[] pointFArray = new PointF[1]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y3"])
    };
    horizontalScroll[0].AddArc(pointFArray[0].X, pointFArray[0].Y - shapeFormula["ch2"], shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 180f, 90f);
    PointF[] linePoints1 = new PointF[2]
    {
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["ch"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["ch2"])
    };
    PointF[] linePoints2 = new PointF[1]{ linePoints1[0] };
    horizontalScroll[0].AddLines(linePoints2);
    horizontalScroll[0].AddArc(linePoints1[1].X, linePoints1[1].Y - shapeFormula["ch2"], shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 180f, 180f);
    pointFArray[0] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y5"]);
    horizontalScroll[0].AddArc(pointFArray[0].X - shapeFormula["ch2"] * 2f, pointFArray[0].Y - shapeFormula["ch2"], shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 0.0f, 90f);
    linePoints1[0] = new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y + shapeFormula["y6"]);
    linePoints1[1] = new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y + shapeFormula["y7"]);
    linePoints2[0] = linePoints1[0];
    horizontalScroll[0].AddLines(linePoints2);
    horizontalScroll[0].AddArc(linePoints1[0].X - shapeFormula["ch2"] * 2f, linePoints1[0].Y, shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 0.0f, 180f);
    horizontalScroll[0].CloseFigure();
    linePoints1[0] = new PointF(this._rectBounds.X + shapeFormula["x3"] + shapeFormula["ch2"], this._rectBounds.Y + shapeFormula["ch"]);
    linePoints1[1] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ch"]);
    PointF[] linePoints3 = new PointF[2]
    {
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["ch"]),
      linePoints1[0]
    };
    horizontalScroll[1].AddLines(linePoints3);
    horizontalScroll[2].AddArc(linePoints1[1].X - shapeFormula["ch2"], linePoints1[1].Y - shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 90f, -90f);
    horizontalScroll[1].CloseFigure();
    linePoints1[0] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ch"] - shapeFormula["ch2"]);
    linePoints1[1] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ch2"]);
    linePoints3[0] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ch"]);
    linePoints3[1] = linePoints1[0];
    horizontalScroll[3].AddLines(linePoints3);
    horizontalScroll[3].AddArc(linePoints1[1].X - shapeFormula["ch2"], linePoints1[1].Y - shapeFormula["ch2"] / 2f, shapeFormula["ch4"] * 2f, shapeFormula["ch4"] * 2f, 0.0f, 180f);
    linePoints1[0] = new PointF(this._rectBounds.X + shapeFormula["ch2"] * 2f, this._rectBounds.Y - shapeFormula["ch2"] + shapeFormula["y4"]);
    linePoints1[1] = new PointF(this._rectBounds.X + shapeFormula["ch2"], this._rectBounds.Y + shapeFormula["y3"]);
    linePoints3[0] = new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y + shapeFormula["y6"]);
    linePoints3[1] = linePoints1[0];
    horizontalScroll[4].AddLines(linePoints3);
    horizontalScroll[5].AddArc(linePoints1[1].X, linePoints1[1].Y - shapeFormula["ch2"] / 2f, shapeFormula["ch4"] * 2f, shapeFormula["ch4"] * 2f, 180f, 180f);
    horizontalScroll[5].AddArc(linePoints1[1].X - shapeFormula["ch2"], linePoints1[1].Y - shapeFormula["ch2"], shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 0.0f, 180f);
    linePoints1[0] = new PointF(this._rectBounds.X + shapeFormula["ch"] - shapeFormula["ch2"], this._rectBounds.Y + shapeFormula["y3"]);
    linePoints1[1] = new PointF(this._rectBounds.X + shapeFormula["ch"] - shapeFormula["ch2"], this._rectBounds.Y + shapeFormula["y3"] + shapeFormula["ch2"]);
    horizontalScroll[6].AddLines(linePoints1);
    return horizontalScroll;
  }

  internal PdfPath GetWave()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.Wave);
    PdfPath wave = new PdfPath();
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

  internal PdfPath GetDoubleWave()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.DoubleWave);
    PdfPath doubleWave = new PdfPath();
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

  internal PdfPath GetRectangularCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RectangularCallout);
    PdfPath rectangularCalloutPath = new PdfPath();
    PointF[] linePoints = new PointF[16 /*0x10*/]
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
    rectangularCalloutPath.AddLines(linePoints);
    rectangularCalloutPath.CloseFigure();
    return rectangularCalloutPath;
  }

  internal PdfPath GetRoundedRectangularCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.RoundedRectangularCallout);
    PdfPath rectangularCalloutPath = new PdfPath();
    PointF[] linePoints1 = new PointF[4];
    rectangularCalloutPath.AddArc(this._rectBounds.X, this._rectBounds.Y, shapeFormula["u1"] * 2f, shapeFormula["u1"] * 2f, 180f, 90f);
    linePoints1[0] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y);
    linePoints1[1] = new PointF(this._rectBounds.X + shapeFormula["xt"], this._rectBounds.Y + shapeFormula["yt"]);
    linePoints1[2] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y);
    linePoints1[3] = new PointF(this._rectBounds.X + shapeFormula["u2"], this._rectBounds.Y);
    rectangularCalloutPath.AddLines(linePoints1);
    rectangularCalloutPath.AddArc(linePoints1[3].X - shapeFormula["u1"], linePoints1[3].Y, shapeFormula["u1"] * 2f, shapeFormula["u1"] * 2f, 270f, 90f);
    PointF[] linePoints2 = new PointF[4]
    {
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["xr"], this._rectBounds.Y + shapeFormula["yr"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["v2"])
    };
    rectangularCalloutPath.AddLines(linePoints2);
    rectangularCalloutPath.AddArc(linePoints2[3].X - shapeFormula["u1"] * 2f, linePoints2[3].Y - shapeFormula["u1"], shapeFormula["u1"] * 2f, shapeFormula["u1"] * 2f, 0.0f, 90f);
    PointF[] linePoints3 = new PointF[4]
    {
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["xb"], this._rectBounds.Y + shapeFormula["yb"]),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X + shapeFormula["u1"], this._rectBounds.Bottom)
    };
    rectangularCalloutPath.AddLines(linePoints3);
    rectangularCalloutPath.AddArc(linePoints3[3].X - shapeFormula["u1"], linePoints3[3].Y - shapeFormula["u1"] * 2f, shapeFormula["u1"] * 2f, shapeFormula["u1"] * 2f, 90f, 90f);
    PointF[] linePoints4 = new PointF[3]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["xl"], this._rectBounds.Y + shapeFormula["yl"]),
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"])
    };
    rectangularCalloutPath.AddLines(linePoints4);
    rectangularCalloutPath.CloseFigure();
    return rectangularCalloutPath;
  }

  internal PdfPath GetOvalCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.OvalCallout);
    PdfPath ovalCalloutPath = new PdfPath();
    PointF[] linePoints = new PointF[1]
    {
      new PointF(this._rectBounds.X + shapeFormula["xPos"], this._rectBounds.Y + shapeFormula["yPos"])
    };
    float startAngle = shapeFormula["stAng1"];
    float sweepAngle = shapeFormula["swAng"];
    if ((double) startAngle < 180.0 && (double) linePoints[0].X < (double) this._rectBounds.X + (double) this._rectBounds.Width / 2.0 || (double) startAngle < 0.0 && (double) linePoints[0].Y > (double) this._rectBounds.Y)
      startAngle += 180f;
    if ((double) sweepAngle < 180.0)
      sweepAngle += 180f;
    ovalCalloutPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, startAngle, sweepAngle);
    ovalCalloutPath.AddLines(linePoints);
    ovalCalloutPath.CloseFigure();
    return ovalCalloutPath;
  }

  internal PdfPath GetCloudCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.CloudCallout);
    PdfPath cloudCalloutPath = new PdfPath();
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

  internal PdfPath GetLineCallout1Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout1);
    PdfPath lineCallout1Path = new PdfPath();
    float num = (double) shapeFormula["x1"] < (double) shapeFormula["x2"] ? ((double) shapeFormula["x1"] < 0.0 ? shapeFormula["x1"] : 0.0f) : ((double) shapeFormula["x2"] < 0.0 ? shapeFormula["x2"] : 0.0f);
    RectangleF rectangleF = this._rectBounds;
    if ((double) num < 0.0)
      rectangleF = new RectangleF(this._rectBounds.X - num, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height);
    PointF[] linePoints = new PointF[4]
    {
      new PointF(rectangleF.X, rectangleF.Y),
      new PointF(rectangleF.Right, rectangleF.Y),
      new PointF(rectangleF.Right, rectangleF.Bottom),
      new PointF(rectangleF.X, rectangleF.Bottom)
    };
    lineCallout1Path.AddLines(linePoints);
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

  internal PdfPath GetLineCallout2Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout2);
    PdfPath lineCallout2Path = new PdfPath();
    PointF[] linePoints = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    lineCallout2Path.AddLines(linePoints);
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

  internal PdfPath GetLineCallout3Path()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout3);
    PdfPath lineCallout3Path = new PdfPath();
    PointF[] linePoints = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    lineCallout3Path.AddLines(linePoints);
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

  internal PdfPath GetLineCallout1AccentBarPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout1AccentBar);
    PdfPath callout1AccentBarPath = new PdfPath();
    PointF[] linePoints = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    callout1AccentBarPath.AddLines(linePoints);
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

  internal PdfPath GetLineCallout2AccentBarPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout2AccentBar);
    PdfPath callout2AccentBarPath = new PdfPath();
    PointF[] linePoints = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    callout2AccentBarPath.AddLines(linePoints);
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

  internal PdfPath GetLineCallout3AccentBarPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout3AccentBar);
    PdfPath callout3AccentBarPath = new PdfPath();
    PointF[] linePoints = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    callout3AccentBarPath.AddLines(linePoints);
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

  internal PdfPath GetLineCallout1NoBorderPath()
  {
    Dictionary<string, float> shapeFormula = this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout1NoBorder);
    PdfPath callout1NoBorderPath = new PdfPath();
    PointF[] linePoints1 = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    callout1NoBorderPath.AddLines(linePoints1);
    callout1NoBorderPath.CloseFigure();
    PointF[] linePoints2 = new PointF[2]
    {
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y2"])
    };
    callout1NoBorderPath.AddLines(linePoints2);
    return callout1NoBorderPath;
  }

  internal PdfPath GetLineCallout2NoBorderPath()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout2NoBorder);
    PdfPath callout2NoBorderPath = new PdfPath();
    PointF[] linePoints = new PointF[2];
    callout2NoBorderPath.AddLines(linePoints);
    callout2NoBorderPath.CloseFigure();
    return callout2NoBorderPath;
  }

  internal PdfPath GetLineCallout3NoBorderPath()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout3NoBorder);
    PdfPath callout3NoBorderPath = new PdfPath();
    PointF[] linePoints = new PointF[2];
    callout3NoBorderPath.AddLines(linePoints);
    callout3NoBorderPath.CloseFigure();
    return callout3NoBorderPath;
  }

  internal PdfPath GetLineCallout1BorderAndAccentBarPath()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout1BorderAndAccentBar);
    PdfPath andAccentBarPath = new PdfPath();
    PointF[] linePoints = new PointF[2];
    andAccentBarPath.AddLines(linePoints);
    andAccentBarPath.CloseFigure();
    return andAccentBarPath;
  }

  internal PdfPath GetLineCallout2BorderAndAccentBarPath()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout2BorderAndAccentBar);
    PdfPath andAccentBarPath = new PdfPath();
    PointF[] linePoints = new PointF[2];
    andAccentBarPath.AddLines(linePoints);
    andAccentBarPath.CloseFigure();
    return andAccentBarPath;
  }

  internal PdfPath GetLineCallout3BorderAndAccentBarPath()
  {
    this._formulaValues.ParseShapeFormula(AutoShapeType.LineCallout3BorderAndAccentBar);
    PdfPath andAccentBarPath = new PdfPath();
    PointF[] linePoints = new PointF[2];
    andAccentBarPath.AddLines(linePoints);
    andAccentBarPath.CloseFigure();
    return andAccentBarPath;
  }

  private float GetDegreeValue(float value) => value / 60000f;

  private PointF GetXYPosition(float xDifference, float yDifference, float positionRatio)
  {
    return new PointF(this._rectBounds.X + this._rectBounds.Width * xDifference / positionRatio, this._rectBounds.Y + this._rectBounds.Height * yDifference / positionRatio);
  }
}
