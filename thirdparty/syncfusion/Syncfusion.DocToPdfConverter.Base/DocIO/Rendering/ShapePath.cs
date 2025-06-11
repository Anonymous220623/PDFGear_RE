// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.Rendering.ShapePath
// Assembly: Syncfusion.DocToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 84EFC094-D348-494C-A410-44F5807BB0D3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocToPdfConverter.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;

#nullable disable
namespace Syncfusion.DocIO.Rendering;

internal class ShapePath
{
  private RectangleF _rectBounds;
  private Dictionary<string, string> _shapeGuide;

  internal ShapePath(RectangleF bounds, Dictionary<string, string> shapeGuide)
  {
    this._rectBounds = bounds;
    this._shapeGuide = shapeGuide;
  }

  internal PdfPath GetCurvedConnectorPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.CurvedConnector);
    PdfPath curvedConnectorPath = new PdfPath();
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
    curvedConnectorPath.AddBeziers(points);
    return curvedConnectorPath;
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

  internal PdfPath GetCurvedConnector4Path()
  {
    PdfPath curvedConnector4Path = new PdfPath();
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.CurvedConnector4);
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
    PdfPath curvedConnector5Path = new PdfPath();
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.CurvedConnector5);
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

  internal PdfPath GetBentConnectorPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.ElbowConnector);
    PointF[] pointFArray = new PointF[4]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    PdfPath bentConnectorPath = new PdfPath();
    bentConnectorPath.StartFigure();
    bentConnectorPath.AddLine(pointFArray[0], pointFArray[1]);
    bentConnectorPath.CloseFigure();
    bentConnectorPath.StartFigure();
    bentConnectorPath.AddLine(pointFArray[1], pointFArray[2]);
    bentConnectorPath.CloseFigure();
    bentConnectorPath.StartFigure();
    bentConnectorPath.AddLine(pointFArray[2], pointFArray[3]);
    bentConnectorPath.CloseFigure();
    return bentConnectorPath;
  }

  internal PdfPath GetBendConnector2Path()
  {
    PdfPath bendConnector2Path = new PdfPath();
    PointF[] linePoints = new PointF[3]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    bendConnector2Path.AddLines(linePoints);
    return bendConnector2Path;
  }

  internal PdfPath GetBentConnector4Path()
  {
    PdfPath bentConnector4Path = new PdfPath();
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.BentConnector4);
    PointF[] linePoints = new PointF[5]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    bentConnector4Path.AddLines(linePoints);
    return bentConnector4Path;
  }

  internal PdfPath GetBentConnector5Path()
  {
    PdfPath bentConnector5Path = new PdfPath();
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.BentConnector5);
    PointF[] linePoints = new PointF[6]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y2"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.Right, this._rectBounds.Bottom)
    };
    bentConnector5Path.AddLines(linePoints);
    return bentConnector5Path;
  }

  internal PdfPath GetRoundedRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.RoundedRectangle);
    PdfPath roundedRectanglePath = new PdfPath();
    float num = shapeFormula["x1"] * 2f;
    if ((double) num > 0.0)
    {
      roundedRectanglePath.AddArc(this._rectBounds.X, this._rectBounds.Y, num, num, 180f, 90f);
      roundedRectanglePath.AddArc(this._rectBounds.Right - num, this._rectBounds.Y, num, num, 270f, 90f);
      roundedRectanglePath.AddArc(this._rectBounds.Right - num, this._rectBounds.Bottom - num, num, num, 0.0f, 90f);
      roundedRectanglePath.AddArc(this._rectBounds.X, this._rectBounds.Bottom - num, num, num, 90f, 90f);
      roundedRectanglePath.CloseFigure();
    }
    return roundedRectanglePath;
  }

  internal PdfPath GetSnipSingleCornerRectanglePath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.SnipSingleCornerRectangle);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.SnipSameSideCornerRectangle);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.SnipDiagonalCornerRectangle);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.SnipAndRoundSingleCornerRectangle);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.RoundSingleCornerRectangle);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.RoundSameSideCornerRectangle);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.RoundDiagonalCornerRectangle);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.IsoscelesTriangle);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Parallelogram);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Trapezoid);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.RegularPentagon);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Hexagon);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Heptagon);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Octagon);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Decagon);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Dodecagon);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Pie);
    PdfPath piePath = new PdfPath();
    piePath.AddPie(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, shapeFormula["stAng"], shapeFormula["swAng"]);
    return piePath;
  }

  internal PdfPath GetChordPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Chord);
    PdfPath chordPath = new PdfPath();
    chordPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, shapeFormula["stAng"], shapeFormula["swAng"]);
    chordPath.CloseFigure();
    return chordPath;
  }

  internal PdfPath GetTearDropPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Teardrop);
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

  internal PdfPath GetFramePath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Frame);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.HalfFrame);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.L_Shape);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.DiagonalStripe);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Cross);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Plaque);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Can);
    PdfPath canPath = new PdfPath();
    canPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, shapeFormula["y1"] * 2f, 0.0f, 180f);
    canPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, shapeFormula["y1"] * 2f, 180f, 180f);
    canPath.AddArc(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y3"] - shapeFormula["y1"], this._rectBounds.Width, shapeFormula["y1"] * 2f, 0.0f, 180f);
    canPath.AddLine(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y3"] - shapeFormula["y1"], this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"]);
    return canPath;
  }

  internal PdfPath GetCubePath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Cube);
    PdfPath cubePath = new PdfPath();
    PointF[] linePoints1 = new PointF[6]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["y1"], this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y4"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Bottom),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    cubePath.AddLines(linePoints1);
    cubePath.CloseFigure();
    PointF[] linePoints2 = new PointF[5]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.Right, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Bottom)
    };
    cubePath.AddLines(linePoints2);
    return cubePath;
  }

  internal PdfPath GetBevelPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Bevel);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Donut);
    PdfPath donutPath = new PdfPath();
    RectangleF rectangle = new RectangleF(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height);
    if ((double) rectangle.Width > 0.0 && (double) rectangle.Height > 0.0)
    {
      donutPath.AddArc(rectangle, 180f, 90f);
      donutPath.AddArc(rectangle, 270f, 90f);
      donutPath.AddArc(rectangle, 0.0f, 90f);
      donutPath.AddArc(rectangle, 90f, 90f);
    }
    donutPath.CloseFigure();
    rectangle = new RectangleF(this._rectBounds.X + shapeFormula["dr"], this._rectBounds.Y + shapeFormula["dr"], shapeFormula["iwd2"] * 2f, shapeFormula["ihd2"] * 2f);
    if ((double) rectangle.Width > 0.0 && (double) rectangle.Height > 0.0)
    {
      donutPath.AddArc(rectangle, 180f, -90f);
      donutPath.AddArc(rectangle, 90f, -90f);
      donutPath.AddArc(rectangle, 0.0f, -90f);
      donutPath.AddArc(rectangle, 270f, -90f);
    }
    donutPath.CloseFigure();
    return donutPath;
  }

  internal PdfPath GetNoSymbolPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.NoSymbol);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.BlockArc);
    PdfPath blockArcPath = new PdfPath();
    RectangleF rectangle = new RectangleF(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height);
    if ((double) shapeFormula["iwd2"] == 0.0)
      shapeFormula["iwd2"] = 1f;
    if ((double) shapeFormula["ihd2"] == 0.0)
      shapeFormula["ihd2"] = 1f;
    blockArcPath.AddArc(rectangle, shapeFormula["stAng"] / 60000f, shapeFormula["swAng"] / 60000f);
    blockArcPath.AddArc(this._rectBounds.Right - shapeFormula["x2"], this._rectBounds.Bottom - shapeFormula["y2"], shapeFormula["iwd2"] * 2f, shapeFormula["ihd2"] * 2f, shapeFormula["istAng"] / 60000f, shapeFormula["iswAng"] / 60000f);
    blockArcPath.CloseFigure();
    return blockArcPath;
  }

  internal PdfPath GetFoldedCornerPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.FoldedCorner);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.SmileyFace);
    PdfPath[] smileyFacePath = new PdfPath[3];
    for (int index = 0; index < smileyFacePath.Length; ++index)
      smileyFacePath[index] = new PdfPath();
    PointF[] points = new PointF[4];
    points[0] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y2"]);
    points[1] = points[0];
    points[2] = new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["y5"]);
    points[3] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y2"]);
    smileyFacePath[1].AddBeziers(points);
    smileyFacePath[0].AddEllipse(this._rectBounds);
    smileyFacePath[2].AddEllipse(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y1"] - shapeFormula["hR"], shapeFormula["wR"] * 2f, shapeFormula["hR"] * 2f);
    smileyFacePath[2].AddEllipse(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"] - shapeFormula["hR"], shapeFormula["wR"] * 2f, shapeFormula["hR"] * 2f);
    return smileyFacePath;
  }

  internal PdfPath GetHeartPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Heart);
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
    lightningBoltPath.CloseAllFigures();
    return lightningBoltPath;
  }

  internal PdfPath GetSunPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Sun);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Moon);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Cloud);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Arc);
    PdfPath[] arcPath = new PdfPath[2];
    for (int index = 0; index < arcPath.Length; ++index)
      arcPath[index] = new PdfPath();
    arcPath[0].AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, shapeFormula["stAng"] / 60000f, shapeFormula["swAng"] / 60000f);
    arcPath[1].AddPie(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, shapeFormula["stAng"] / 60000f, shapeFormula["swAng"] / 60000f);
    return arcPath;
  }

  internal PdfPath GetDoubleBracketPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.DoubleBracket);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.DoubleBrace);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.LeftBracket);
    PdfPath leftBracketPath = new PdfPath();
    leftBracketPath.AddArc(this._rectBounds.Right - this._rectBounds.Width, this._rectBounds.Bottom - shapeFormula["y1"] * 2f, this._rectBounds.Width * 2f, shapeFormula["y1"] * 2f, 90f, 90f);
    leftBracketPath.AddArc(this._rectBounds.Right - this._rectBounds.Width, this._rectBounds.Y, this._rectBounds.Width * 2f, shapeFormula["y1"] * 2f, 180f, 90f);
    return leftBracketPath;
  }

  internal PdfPath GetRightBracketPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.RightBracket);
    PdfPath rightBracketPath = new PdfPath();
    rightBracketPath.AddArc(this._rectBounds.X - this._rectBounds.Width, this._rectBounds.Y, this._rectBounds.Width * 2f, shapeFormula["y1"] * 2f, 270f, 90f);
    rightBracketPath.AddArc(this._rectBounds.X - this._rectBounds.Width, this._rectBounds.Y + shapeFormula["y2"] - shapeFormula["y1"], this._rectBounds.Width * 2f, shapeFormula["y1"] * 2f, 0.0f, 90f);
    return rightBracketPath;
  }

  internal PdfPath GetLeftBracePath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.LeftBrace);
    PdfPath leftBracePath = new PdfPath();
    leftBracePath.AddArc(this._rectBounds.Right - this._rectBounds.Width / 2f, this._rectBounds.Bottom - shapeFormula["y1"] * 2f, this._rectBounds.Width, shapeFormula["y1"] * 2f, 90f, 90f);
    leftBracePath.AddArc(this._rectBounds.X - this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["y4"] - shapeFormula["y1"], this._rectBounds.Width, shapeFormula["y1"] * 2f, 0.0f, -90f);
    leftBracePath.AddArc(this._rectBounds.X - this._rectBounds.Width / 2f, (float) ((double) this._rectBounds.Y + (double) shapeFormula["y4"] - (double) shapeFormula["y1"] * 3.0), this._rectBounds.Width, shapeFormula["y1"] * 2f, 90f, -90f);
    leftBracePath.AddArc(this._rectBounds.Right - this._rectBounds.Width / 2f, this._rectBounds.Y, this._rectBounds.Width, shapeFormula["y1"] * 2f, 180f, 90f);
    return leftBracePath;
  }

  internal PdfPath GetRightBracePath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.RightBrace);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.RightArrow);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.LeftArrow);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.UpArrow);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.DownArrow);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.LeftRightArrow);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.CurvedRightArrow);
    GraphicsPath graphicsPath = this.GetGraphicsPath();
    PointF[] pointFArray = new PointF[7];
    pointFArray[0] = new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["hR"]);
    graphicsPath.AddArc(pointFArray[0].X, pointFArray[0].Y - shapeFormula["hR"], this._rectBounds.Width * 2f, shapeFormula["hR"] * 2f, 180f, shapeFormula["mswAng"]);
    pointFArray[1] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"]);
    pointFArray[2] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y6"]);
    graphicsPath.AddLine(pointFArray[1], pointFArray[2]);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y8"]);
    graphicsPath.AddLine(pointFArray[2], pointFArray[3]);
    pointFArray[4] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y7"]);
    graphicsPath.AddArc(pointFArray[4].X - shapeFormula["x1"], pointFArray[4].Y - shapeFormula["hR"] * 2f, this._rectBounds.Width * 2f, shapeFormula["hR"] * 2f, shapeFormula["stAng"], shapeFormula["swAng"]);
    graphicsPath.CloseFigure();
    pointFArray[5] = new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["hR"]);
    graphicsPath.AddArc(pointFArray[5].X, pointFArray[5].Y - shapeFormula["hR"], this._rectBounds.Width * 2f, shapeFormula["hR"] * 2f, 180f, 90f);
    pointFArray[6] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["th"]);
    graphicsPath.AddLine(pointFArray[5].X + this._rectBounds.Width, pointFArray[5].Y - shapeFormula["hR"], pointFArray[6].X, pointFArray[6].Y);
    graphicsPath.AddArc(pointFArray[6].X - this._rectBounds.Width, pointFArray[6].Y, this._rectBounds.Width * 2f, shapeFormula["hR"] * 2f, 270f, shapeFormula["swAng2"]);
    return new PdfPath(graphicsPath.PathPoints, graphicsPath.PathTypes);
  }

  internal PdfPath GetCurvedLeftArrowPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.CurvedLeftArrow);
    GraphicsPath graphicsPath = this.GetGraphicsPath();
    PointF[] pointFArray = new PointF[7];
    pointFArray[0] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y3"]);
    graphicsPath.AddArc(this._rectBounds.Right - this._rectBounds.Width * 2f, pointFArray[0].Y - shapeFormula["hR"], this._rectBounds.Width * 2f, shapeFormula["hR"] * 2f, 0.0f, -90f);
    pointFArray[1] = new PointF(this._rectBounds.X, this._rectBounds.Y);
    graphicsPath.AddArc(pointFArray[1].X - this._rectBounds.Width, pointFArray[1].Y, this._rectBounds.Width * 2f, shapeFormula["hR"] * 2f, 270f, 90f);
    pointFArray[2] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y3"]);
    graphicsPath.AddArc(pointFArray[2].X - this._rectBounds.Width * 2f, pointFArray[2].Y - shapeFormula["hR"], this._rectBounds.Width * 2f, shapeFormula["hR"] * 2f, 0.0f, shapeFormula["swAng"]);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y8"]);
    pointFArray[4] = new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y6"]);
    graphicsPath.AddLine(pointFArray[3], pointFArray[4]);
    pointFArray[5] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y4"]);
    graphicsPath.AddLine(pointFArray[4], pointFArray[5]);
    pointFArray[6] = new PointF(this._rectBounds.X + shapeFormula["x1"], this._rectBounds.Y + shapeFormula["y5"]);
    graphicsPath.AddLine(pointFArray[5], pointFArray[6]);
    graphicsPath.AddArc(this._rectBounds.X - this._rectBounds.Width, pointFArray[1].Y, this._rectBounds.Width * 2f, shapeFormula["hR"] * 2f, shapeFormula["swAng"], shapeFormula["swAng2"]);
    return new PdfPath(graphicsPath.PathPoints, graphicsPath.PathTypes);
  }

  internal PdfPath GetCurvedUpArrowPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.CurvedUpArrow);
    GraphicsPath graphicsPath = this.GetGraphicsPath();
    PointF[] pointFArray = new PointF[7];
    SizeF sizeF = new SizeF(shapeFormula["wR"], this._rectBounds.Height);
    pointFArray[0] = new PointF(this._rectBounds.X + shapeFormula["ix"], this._rectBounds.Y + shapeFormula["iy"]);
    pointFArray[1] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]);
    pointFArray[2] = new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y1"]);
    pointFArray[4] = new PointF(this._rectBounds.X + shapeFormula["x7"], this._rectBounds.Y + shapeFormula["y1"]);
    pointFArray[5] = new PointF(this._rectBounds.X + shapeFormula["wR"], this._rectBounds.Bottom);
    pointFArray[6] = new PointF(this._rectBounds.X + shapeFormula["th"], this._rectBounds.Y);
    graphicsPath.AddArc(pointFArray[5].X - sizeF.Width, pointFArray[0].Y - sizeF.Height - shapeFormula["iy"], sizeF.Width * 2f, sizeF.Height * 2f, shapeFormula["stAng2"], shapeFormula["swAng2"]);
    graphicsPath.AddLine(pointFArray[1], pointFArray[2]);
    graphicsPath.AddLine(pointFArray[2], pointFArray[3]);
    graphicsPath.AddLine(pointFArray[3], pointFArray[4]);
    graphicsPath.AddArc(pointFArray[6].X, this._rectBounds.Y - sizeF.Height, sizeF.Width * 2f, sizeF.Height * 2f, shapeFormula["stAng3"], shapeFormula["swAng"]);
    graphicsPath.AddArc(pointFArray[5].X - sizeF.Width, pointFArray[5].Y - sizeF.Height * 2f, sizeF.Width * 2f, sizeF.Height * 2f, 90f, 90f);
    graphicsPath.AddArc(pointFArray[6].X, pointFArray[6].Y - sizeF.Height, sizeF.Width * 2f, sizeF.Height * 2f, 180f, -90f);
    return new PdfPath(graphicsPath.PathPoints, graphicsPath.PathTypes);
  }

  internal PdfPath GetCurvedDownArrowPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.CurvedDownArrow);
    GraphicsPath graphicsPath = this.GetGraphicsPath();
    PointF[] pointFArray = new PointF[7];
    SizeF sizeF = new SizeF(shapeFormula["wR"], this._rectBounds.Height);
    pointFArray[0] = new PointF(this._rectBounds.X + shapeFormula["ix"], this._rectBounds.Y + shapeFormula["iy"]);
    pointFArray[1] = new PointF(this._rectBounds.X, this._rectBounds.Bottom);
    pointFArray[2] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["x8"], this._rectBounds.Y + shapeFormula["y1"]);
    pointFArray[4] = new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Bottom);
    pointFArray[5] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]);
    pointFArray[6] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y1"]);
    graphicsPath.AddArc(pointFArray[2].X - sizeF.Width, this._rectBounds.Y, sizeF.Width * 2f, sizeF.Height * 2f, shapeFormula["stAng2"], shapeFormula["swAng2"]);
    graphicsPath.AddArc(this._rectBounds.X, this._rectBounds.Y, sizeF.Width * 2f, sizeF.Height * 2f, 180f, 90f);
    graphicsPath.AddArc(this._rectBounds.X, this._rectBounds.Y, sizeF.Width * 2f, sizeF.Height * 2f, 270f, shapeFormula["swAng"]);
    graphicsPath.AddLine(pointFArray[6], pointFArray[5]);
    graphicsPath.AddLine(pointFArray[5], pointFArray[4]);
    graphicsPath.AddLine(pointFArray[4], pointFArray[3]);
    graphicsPath.AddLine(pointFArray[3], new PointF(pointFArray[3].X - shapeFormula["x5"] + shapeFormula["x4"], pointFArray[3].Y));
    graphicsPath.AddArc(pointFArray[2].X - sizeF.Width, this._rectBounds.Y, sizeF.Width * 2f, sizeF.Height * 2f, shapeFormula["stAng"], shapeFormula["mswAng"]);
    PointF[] points = new PointF[1]
    {
      new PointF(this._rectBounds.X + sizeF.Width, this._rectBounds.Y)
    };
    graphicsPath.AddLines(points);
    return new PdfPath(graphicsPath.PathPoints, graphicsPath.PathTypes);
  }

  internal PdfPath GetUpDownArrowPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.UpDownArrow);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.QuadArrow);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.LeftRightUpArrow);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.BentArrow);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.UTurnArrow);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.LeftUpArrow);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.BentUpArrow);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.StripedRightArrow);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.NotchedRightArrow);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Pentagon);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Chevron);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.RightArrowCallout);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.DownArrowCallout);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.LeftArrowCallout);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.UpArrowCallout);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.LeftRightArrowCallout);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.QuadArrowCallout);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.CircularArrow);
    PdfPath circularArrowPath = new PdfPath();
    PointF[] pointFArray = new PointF[5];
    SizeF sizeF1 = new SizeF(shapeFormula["rw1"], shapeFormula["rh1"]);
    pointFArray[0] = new PointF(this._rectBounds.X + shapeFormula["xE"], this._rectBounds.Y + shapeFormula["yE"]);
    circularArrowPath.AddArc(pointFArray[0].X - sizeF1.Width * 2f, pointFArray[0].Y - sizeF1.Height, sizeF1.Width * 2f, sizeF1.Height * 2f, shapeFormula["stAng"], shapeFormula["swAng"]);
    pointFArray[1] = new PointF(this._rectBounds.X + shapeFormula["xGp"], this._rectBounds.Y + shapeFormula["yGp"]);
    pointFArray[2] = new PointF(this._rectBounds.X + shapeFormula["xA"], this._rectBounds.Y + shapeFormula["yA"]);
    pointFArray[3] = new PointF(this._rectBounds.X + shapeFormula["xBp"], this._rectBounds.Y + shapeFormula["yBp"]);
    pointFArray[4] = new PointF(this._rectBounds.X + shapeFormula["xC"], this._rectBounds.Y + shapeFormula["yC"]);
    circularArrowPath.AddLine(new PointF(pointFArray[1].X - (pointFArray[4].X - pointFArray[3].X), pointFArray[1].Y), pointFArray[1]);
    circularArrowPath.AddLine(pointFArray[1], pointFArray[2]);
    circularArrowPath.AddLine(pointFArray[2], pointFArray[3]);
    circularArrowPath.AddLine(pointFArray[3], pointFArray[4]);
    SizeF sizeF2 = new SizeF(shapeFormula["rw2"], shapeFormula["rh2"]);
    circularArrowPath.AddArc(pointFArray[0].X - sizeF1.Width - sizeF2.Width, pointFArray[0].Y - sizeF2.Height, sizeF2.Width * 2f, sizeF2.Height * 2f, shapeFormula["istAng"], shapeFormula["iswAng"]);
    circularArrowPath.CloseFigure();
    return circularArrowPath;
  }

  internal PdfPath GetMathPlusPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.MathPlus);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.MathMinus);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.MathMultiply);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.MathDivision);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.MathEqual);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.MathNotEqual);
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
    PdfPath alternateProcessPath = new PdfPath();
    float num = this.GetPresetOperandValue("ssd6") * 2f;
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
    PointF location = new PointF(rectangle.X + rectangle.Width, rectangle.Y);
    rectangle = new RectangleF(location, new SizeF((float) ((double) this._rectBounds.Width * 5.0 / 20.0 * 2.0), (float) ((double) this._rectBounds.Height * 2.0 / 20.0 * 2.0)));
    chartPunchedTapePath.AddArc(rectangle, 180f, 180f);
    chartPunchedTapePath.AddLine(new PointF(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), this.GetXYPosition(20f, 18f, 20f));
    rectangle = new RectangleF(new PointF(this.GetXYPosition(20f, 18f, 20f).X - rectangle.Width, this.GetXYPosition(20f, 18f, 20f).Y), new SizeF((float) ((double) this._rectBounds.Width * 5.0 / 20.0 * 2.0), (float) ((double) this._rectBounds.Height * 2.0 / 20.0 * 2.0)));
    chartPunchedTapePath.AddArc(rectangle, 0.0f, -180f);
    location = new PointF(rectangle.X - rectangle.Width, rectangle.Y);
    rectangle = new RectangleF(location, new SizeF((float) ((double) this._rectBounds.Width * 5.0 / 20.0 * 2.0), (float) ((double) this._rectBounds.Height * 2.0 / 20.0 * 2.0)));
    chartPunchedTapePath.AddArc(rectangle, 0.0f, 180f);
    chartPunchedTapePath.CloseFigure();
    return chartPunchedTapePath;
  }

  internal PdfPath GetFlowChartSummingJunctionPath()
  {
    PdfPath summingJunctionPath = new PdfPath();
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.FlowChartSummingJunction);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.FlowChartSequentialAccessStorage);
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
    chartDisplayPath.AddArc(this.GetXYPosition(5f, 0.0f, 6f).X - this._rectBounds.Width / 6f, this.GetXYPosition(5f, 0.0f, 6f).Y, this._rectBounds.Width / 3f, this._rectBounds.Height, 270f, 180f);
    chartDisplayPath.AddLine(new PointF(this.GetXYPosition(5f, 0.0f, 6f).X - this._rectBounds.Width / 6f, this.GetXYPosition(5f, 0.0f, 6f).Y + this._rectBounds.Height), this.GetXYPosition(1f, 6f, 6f));
    chartDisplayPath.CloseFigure();
    return chartDisplayPath;
  }

  internal PdfPath GetExplosion1()
  {
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Star4Point);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Star5Point);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Star6Point);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Star7Point);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Star8Point);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Star10Point);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Star12Point);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Star16Point);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Star24Point);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Star32Point);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.UpRibbon);
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
    upRibbon.AddLine(new PointF(pointFArray[12].X - (float) ((double) this._rectBounds.Width / 32.0 * 2.0), pointFArray[12].Y - shapeFormula["hR"] * 2f), new PointF(pointFArray[13].X + this._rectBounds.Width / 32f, pointFArray[13].Y));
    upRibbon.AddArc(pointFArray[13].X, pointFArray[13].Y, (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 270f, -180f);
    pointFArray[14] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]);
    upRibbon.AddLine(new PointF(pointFArray[13].X + this._rectBounds.Width / 32f, pointFArray[13].Y + shapeFormula["hR"] * 2f), new PointF(pointFArray[14].X - this._rectBounds.Width / 32f, pointFArray[14].Y));
    upRibbon.AddArc(pointFArray[14].X - (float) ((double) this._rectBounds.Width / 32.0 * 2.0), pointFArray[14].Y, (float) ((double) this._rectBounds.Width / 32.0 * 2.0), shapeFormula["hR"] * 2f, 270f, 180f);
    upRibbon.CloseFigure();
    upRibbon.StartFigure();
    pointFArray[15] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y2"]);
    pointFArray[16 /*0x10*/] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y6"]);
    upRibbon.AddLine(pointFArray[15], pointFArray[16 /*0x10*/]);
    upRibbon.StartFigure();
    pointFArray[17] = new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y6"]);
    pointFArray[18] = new PointF(this._rectBounds.X + shapeFormula["x6"], this._rectBounds.Y + shapeFormula["y2"]);
    upRibbon.AddLine(pointFArray[17], pointFArray[18]);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.DownRibbon);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.CurvedUpRibbon);
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
    curvedUpRibbon.AddBezier(pointFArray3[0], pointFArray3[0], pointFArray3[1], pointFArray3[2]);
    PointF[] pointFArray5 = new PointF[3]
    {
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]),
      new PointF(this._rectBounds.X + shapeFormula["cx1"], this._rectBounds.Y + shapeFormula["cy1"]),
      new PointF(this._rectBounds.X, this._rectBounds.Bottom)
    };
    curvedUpRibbon.AddBezier(pointFArray5[0], pointFArray5[0], pointFArray5[1], pointFArray5[2]);
    curvedUpRibbon.CloseFigure();
    PointF[] linePoints3 = new PointF[2]
    {
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y5"])
    };
    curvedUpRibbon.AddLines(linePoints3);
    curvedUpRibbon.CloseFigure();
    linePoints3[0] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y5"]);
    linePoints3[1] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y3"]);
    curvedUpRibbon.AddLines(linePoints3);
    curvedUpRibbon.CloseFigure();
    linePoints3[0] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y7"]);
    linePoints3[1] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]);
    curvedUpRibbon.AddLines(linePoints3);
    curvedUpRibbon.CloseFigure();
    linePoints3[0] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]);
    linePoints3[1] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y7"]);
    curvedUpRibbon.AddLines(linePoints3);
    curvedUpRibbon.CloseFigure();
    return curvedUpRibbon;
  }

  internal PdfPath GetCurvedDownRibbon()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.CurvedDownRibbon);
    PdfPath curvedDownRibbon = new PdfPath();
    PointF[] pointFArray1 = new PointF[3]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y),
      new PointF(this._rectBounds.X + shapeFormula["cx1"], this._rectBounds.Y + shapeFormula["cy1"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"])
    };
    curvedDownRibbon.AddBezier(pointFArray1[0], pointFArray1[0], pointFArray1[1], pointFArray1[2]);
    pointFArray1[0] = new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"]);
    pointFArray1[1] = new PointF(this._rectBounds.X + this._rectBounds.Width / 2f, this._rectBounds.Y + shapeFormula["cy3"]);
    pointFArray1[2] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y3"]);
    curvedDownRibbon.AddBezier(pointFArray1[0], pointFArray1[0], pointFArray1[1], pointFArray1[2]);
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
    PointF[] linePoints1 = new PointF[1]{ pointFArray2[0] };
    curvedDownRibbon.AddLines(linePoints1);
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
    linePoints1[0] = pointFArray1[0];
    curvedDownRibbon.AddLines(linePoints1);
    curvedDownRibbon.CloseFigure();
    PointF[] linePoints2 = new PointF[2]
    {
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y5"]),
      new PointF(this._rectBounds.X + shapeFormula["x2"], this._rectBounds.Y + shapeFormula["y3"])
    };
    curvedDownRibbon.AddLines(linePoints2);
    curvedDownRibbon.CloseFigure();
    linePoints2[0] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y3"]);
    linePoints2[1] = new PointF(this._rectBounds.X + shapeFormula["x5"], this._rectBounds.Y + shapeFormula["y5"]);
    curvedDownRibbon.AddLines(linePoints2);
    curvedDownRibbon.CloseFigure();
    linePoints2[0] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y1"]);
    linePoints2[1] = new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["y7"]);
    curvedDownRibbon.AddLines(linePoints2);
    curvedDownRibbon.CloseFigure();
    linePoints2[0] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y7"]);
    linePoints2[1] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["y1"]);
    curvedDownRibbon.AddLines(linePoints2);
    curvedDownRibbon.CloseFigure();
    return curvedDownRibbon;
  }

  internal PdfPath GetVerticalScroll()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.VerticalScroll);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.HorizontalScroll);
    PdfPath[] horizontalScroll = new PdfPath[7]
    {
      new PdfPath(),
      new PdfPath(),
      new PdfPath(),
      new PdfPath(),
      new PdfPath(),
      new PdfPath(),
      new PdfPath()
    };
    PointF[] pointFArray = new PointF[1]
    {
      new PointF(this._rectBounds.X, this._rectBounds.Y + shapeFormula["y3"])
    };
    if ((double) shapeFormula["ch2"] > 0.0)
      horizontalScroll[0].AddArc(pointFArray[0].X, pointFArray[0].Y - shapeFormula["ch2"], shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 180f, 90f);
    PointF[] linePoints1 = new PointF[2]
    {
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["ch"]),
      new PointF(this._rectBounds.X + shapeFormula["x3"], this._rectBounds.Y + shapeFormula["ch2"])
    };
    PointF[] linePoints2 = new PointF[1]{ linePoints1[0] };
    horizontalScroll[0].AddLines(linePoints2);
    if ((double) shapeFormula["ch2"] > 0.0)
      horizontalScroll[0].AddArc(linePoints1[1].X, linePoints1[1].Y - shapeFormula["ch2"], shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 180f, 180f);
    pointFArray[0] = new PointF(this._rectBounds.Right, this._rectBounds.Y + shapeFormula["y5"]);
    if ((double) shapeFormula["ch2"] > 0.0)
      horizontalScroll[0].AddArc(pointFArray[0].X - shapeFormula["ch2"] * 2f, pointFArray[0].Y - shapeFormula["ch2"], shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 0.0f, 90f);
    linePoints1[0] = new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y + shapeFormula["y6"]);
    linePoints1[1] = new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y + shapeFormula["y7"]);
    linePoints2[0] = linePoints1[0];
    horizontalScroll[0].AddLines(linePoints2);
    if ((double) shapeFormula["ch2"] > 0.0)
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
    if ((double) shapeFormula["ch2"] > 0.0)
      horizontalScroll[2].AddArc(linePoints1[1].X - shapeFormula["ch2"], linePoints1[1].Y - shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 90f, -90f);
    horizontalScroll[1].CloseFigure();
    linePoints1[0] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ch"] - shapeFormula["ch2"]);
    linePoints1[1] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ch2"]);
    linePoints3[0] = new PointF(this._rectBounds.X + shapeFormula["x4"], this._rectBounds.Y + shapeFormula["ch"]);
    linePoints3[1] = linePoints1[0];
    horizontalScroll[3].AddLines(linePoints3);
    if ((double) shapeFormula["ch4"] > 0.0)
      horizontalScroll[3].AddArc(linePoints1[1].X - shapeFormula["ch2"], linePoints1[1].Y - shapeFormula["ch2"] / 2f, shapeFormula["ch4"] * 2f, shapeFormula["ch4"] * 2f, 0.0f, 180f);
    linePoints1[0] = new PointF(this._rectBounds.X + shapeFormula["ch2"] * 2f, this._rectBounds.Y - shapeFormula["ch2"] + shapeFormula["y4"]);
    linePoints1[1] = new PointF(this._rectBounds.X + shapeFormula["ch2"], this._rectBounds.Y + shapeFormula["y3"]);
    linePoints3[0] = new PointF(this._rectBounds.X + shapeFormula["ch"], this._rectBounds.Y + shapeFormula["y6"]);
    linePoints3[1] = linePoints1[0];
    horizontalScroll[4].AddLines(linePoints3);
    if ((double) shapeFormula["ch4"] > 0.0)
      horizontalScroll[5].AddArc(linePoints1[1].X, linePoints1[1].Y - shapeFormula["ch2"] / 2f, shapeFormula["ch4"] * 2f, shapeFormula["ch4"] * 2f, 180f, 180f);
    if ((double) shapeFormula["ch2"] > 0.0)
      horizontalScroll[5].AddArc(linePoints1[1].X - shapeFormula["ch2"], linePoints1[1].Y - shapeFormula["ch2"], shapeFormula["ch2"] * 2f, shapeFormula["ch2"] * 2f, 0.0f, 180f);
    linePoints1[0] = new PointF(this._rectBounds.X + shapeFormula["ch"] - shapeFormula["ch2"], this._rectBounds.Y + shapeFormula["y3"]);
    linePoints1[1] = new PointF(this._rectBounds.X + shapeFormula["ch"] - shapeFormula["ch2"], this._rectBounds.Y + shapeFormula["y3"] + shapeFormula["ch2"]);
    horizontalScroll[6].AddLines(linePoints1);
    return horizontalScroll;
  }

  internal PdfPath GetWave()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.Wave);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.DoubleWave);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.RectangularCallout);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.RoundedRectangularCallout);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.OvalCallout);
    GraphicsPath graphicsPath = this.GetGraphicsPath();
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
    graphicsPath.AddArc(this._rectBounds.X, this._rectBounds.Y, this._rectBounds.Width, this._rectBounds.Height, startAngle, sweepAngle);
    graphicsPath.AddLines(points);
    graphicsPath.CloseFigure();
    return new PdfPath(graphicsPath.PathPoints, graphicsPath.PathTypes);
  }

  internal PdfPath GetCloudCalloutPath()
  {
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.CloudCallout);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.LineCallout1);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.LineCallout2);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.LineCallout3);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.LineCallout1AccentBar);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.LineCallout2AccentBar);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.LineCallout3AccentBar);
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
    Dictionary<string, float> shapeFormula = this.ParseShapeFormula(AutoShapeType.LineCallout1NoBorder);
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
    PdfPath callout2NoBorderPath = new PdfPath();
    PointF[] linePoints = new PointF[2];
    callout2NoBorderPath.AddLines(linePoints);
    callout2NoBorderPath.CloseFigure();
    return callout2NoBorderPath;
  }

  internal PdfPath GetLineCallout3NoBorderPath()
  {
    PdfPath callout3NoBorderPath = new PdfPath();
    PointF[] linePoints = new PointF[2];
    callout3NoBorderPath.AddLines(linePoints);
    callout3NoBorderPath.CloseFigure();
    return callout3NoBorderPath;
  }

  internal PdfPath GetLineCallout1BorderAndAccentBarPath()
  {
    PdfPath andAccentBarPath = new PdfPath();
    PointF[] linePoints = new PointF[2];
    andAccentBarPath.AddLines(linePoints);
    andAccentBarPath.CloseFigure();
    return andAccentBarPath;
  }

  internal PdfPath GetLineCallout2BorderAndAccentBarPath()
  {
    PdfPath andAccentBarPath = new PdfPath();
    PointF[] linePoints = new PointF[2];
    andAccentBarPath.AddLines(linePoints);
    andAccentBarPath.CloseFigure();
    return andAccentBarPath;
  }

  internal PdfPath GetLineCallout3BorderAndAccentBarPath()
  {
    PdfPath andAccentBarPath = new PdfPath();
    PointF[] linePoints = new PointF[2];
    andAccentBarPath.AddLines(linePoints);
    andAccentBarPath.CloseFigure();
    return andAccentBarPath;
  }

  internal PdfPath GetVMLCustomShapePath(List<Path2D> path2DPoints)
  {
    PdfPath vmlCustomShapePath = new PdfPath();
    PointF pointF = (PointF) Point.Empty;
    for (int index1 = 0; index1 < path2DPoints.Count; ++index1)
    {
      Path2D path2Dpoint = path2DPoints[index1];
      switch (path2Dpoint.PathCommandType)
      {
        case "l":
        case "r":
          if (path2Dpoint.PathPoints.Count > 0)
          {
            PointF[] linePoints = new PointF[path2Dpoint.PathPoints.Count + 1];
            linePoints[0] = pointF;
            for (int index2 = 0; index2 < path2Dpoint.PathPoints.Count; ++index2)
              linePoints[index2 + 1] = path2Dpoint.PathPoints[index2];
            vmlCustomShapePath.AddLines(linePoints);
            pointF = linePoints[linePoints.Length - 1];
            break;
          }
          break;
        case "m":
        case "t":
          if (path2Dpoint.PathPoints.Count > 0)
          {
            vmlCustomShapePath.CloseFigure();
            pointF = path2Dpoint.PathPoints[path2Dpoint.PathPoints.Count - 1];
            break;
          }
          break;
        case "x":
          vmlCustomShapePath.CloseFigure();
          pointF = (PointF) Point.Empty;
          break;
        case "e":
          vmlCustomShapePath.CloseFigure();
          pointF = (PointF) Point.Empty;
          break;
      }
    }
    return vmlCustomShapePath;
  }

  internal PdfPath GetCustomGeomentryPath(RectangleF bounds, PdfPath path, Shape shape)
  {
    Dictionary<string, string> guideList = shape.GetGuideList();
    Dictionary<string, string> avList = shape.GetAvList();
    Dictionary<string, string> combinedValues = new Dictionary<string, string>();
    Dictionary<string, float> calculatedValues = new Dictionary<string, float>();
    foreach (KeyValuePair<string, string> keyValuePair in guideList)
      combinedValues.Add(keyValuePair.Key, keyValuePair.Value);
    foreach (KeyValuePair<string, string> keyValuePair in avList)
    {
      if (!guideList.ContainsKey(keyValuePair.Key))
        combinedValues.Add(keyValuePair.Key, keyValuePair.Value);
    }
    foreach (Path2D path1 in shape.Path2DList)
    {
      List<double> pathElements = new List<double>();
      foreach (string pathElement in path1.PathElements)
        this.ConvertPathElement(pathElement, pathElements, combinedValues, path1, calculatedValues);
      double width = path1.Width;
      double height = path1.Height;
      this.GetGeomentryPath(path, pathElements, width, height, bounds);
      pathElements.Clear();
    }
    combinedValues.Clear();
    calculatedValues.Clear();
    return path;
  }

  private void GetGeomentryPath(
    PdfPath path,
    List<double> pathElements,
    double pathWidth,
    double pathHeight,
    RectangleF bounds)
  {
    PointF point1 = (PointF) Point.Empty;
    double num = 0.0;
    for (int index = 0; index < pathElements.Count; index = index + ((int) num + 1) + 1)
    {
      switch ((ushort) pathElements[index])
      {
        case 1:
          path.CloseFigure();
          point1 = (PointF) Point.Empty;
          num = 0.0;
          break;
        case 2:
          path.CloseFigure();
          num = pathElements[index + 1] * 2.0;
          point1 = new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds));
          break;
        case 3:
          num = pathElements[index + 1] * 2.0;
          PointF point2 = new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds));
          path.AddLine(point1, point2);
          point1 = point2;
          break;
        case 4:
          num = pathElements[index + 1] * 2.0;
          RectangleF rectangle = new RectangleF();
          rectangle.X = bounds.X;
          rectangle.Y = bounds.Y;
          rectangle.Width = (float) (pathElements[index + 2] / 12700.0) * 2f;
          rectangle.Height = (float) (pathElements[index + 3] / 12700.0) * 2f;
          float startAngle = (float) pathElements[index + 4] / 60000f;
          float sweepAngle = (float) pathElements[index + 5] / 60000f;
          path.AddArc(rectangle, startAngle, sweepAngle);
          point1 = path.PathPoints[path.PathPoints.Length - 1];
          break;
        case 5:
          num = pathElements[index + 1] * 2.0;
          PointF[] points1 = new PointF[3]
          {
            point1,
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds)),
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 4], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 5], bounds))
          };
          path.AddBeziers(points1);
          point1 = points1[2];
          break;
        case 6:
          num = pathElements[index + 1] * 2.0;
          PointF[] points2 = new PointF[4]
          {
            point1,
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds)),
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 4], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 5], bounds)),
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 6], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 7], bounds))
          };
          path.AddBeziers(points2);
          point1 = points2[3];
          break;
      }
    }
  }

  private float GetGeomentryPathXValue(double pathWidth, double x, RectangleF bounds)
  {
    if (pathWidth == 0.0)
      return bounds.X + (float) (x / 12700.0);
    double num = x * 100.0 / pathWidth;
    return (float) ((double) bounds.Width * num / 100.0) + bounds.X;
  }

  private float GetGeomentryPathYValue(double pathHeight, double y, RectangleF bounds)
  {
    if (pathHeight == 0.0)
      return bounds.Y + (float) (y / 12700.0);
    double num = y * 100.0 / pathHeight;
    return (float) ((double) bounds.Height * num / 100.0) + bounds.Y;
  }

  private void ConvertPathElement(
    string pathElement,
    List<double> pathElements,
    Dictionary<string, string> combinedValues,
    Path2D path,
    Dictionary<string, float> calculatedValues)
  {
    int result = 0;
    if (int.TryParse(pathElement, out result))
      pathElements.Add(double.Parse(pathElement, (IFormatProvider) CultureInfo.InvariantCulture));
    else if (combinedValues != null && combinedValues.Count > 0 && calculatedValues.Count == 0)
    {
      ShapePath shapePath = new ShapePath(new RectangleF(0.0f, 0.0f, (float) path.Width, (float) path.Height), new Dictionary<string, string>());
      Dictionary<string, float> formulaValues = shapePath.GetFormulaValues(AutoShapeType.Unknown, combinedValues, false);
      foreach (KeyValuePair<string, float> keyValuePair in formulaValues)
        calculatedValues.Add(keyValuePair.Key, keyValuePair.Value);
      shapePath.Close();
      formulaValues.Clear();
      if (!calculatedValues.ContainsKey(pathElement))
        return;
      pathElements.Add((double) calculatedValues[pathElement]);
    }
    else
    {
      if (!calculatedValues.ContainsKey(pathElement))
        return;
      pathElements.Add((double) calculatedValues[pathElement]);
    }
  }

  private GraphicsPath GetGraphicsPath() => new GraphicsPath();

  private float GetDegreeValue(float value) => value / 60000f;

  private PointF GetXYPosition(float xDifference, float yDifference, float positionRatio)
  {
    return new PointF(this._rectBounds.X + this._rectBounds.Width * xDifference / positionRatio, this._rectBounds.Y + this._rectBounds.Height * yDifference / positionRatio);
  }

  private Dictionary<string, float> GetPathAdjustValue(AutoShapeType shapeType)
  {
    Dictionary<string, float> formulaValues = this.GetFormulaValues(AutoShapeType.Unknown, this._shapeGuide, true);
    List<string> stringList = new List<string>((IEnumerable<string>) formulaValues.Keys);
    if (shapeType == AutoShapeType.CircularArrow)
    {
      foreach (string key in stringList)
      {
        if (key != "adj1" && key != "adj5")
          formulaValues[key] /= 60000f;
      }
    }
    return formulaValues;
  }

  public Dictionary<string, float> ParseShapeFormula(AutoShapeType shapeType)
  {
    return this.GetFormulaValues(shapeType, this.GetShapeFormula(shapeType), false);
  }

  private Dictionary<string, float> GetFormulaValues(
    AutoShapeType shapeType,
    Dictionary<string, string> formulaColl,
    bool isAdjValue)
  {
    if (formulaColl.Count == 0)
      return (Dictionary<string, float>) null;
    Dictionary<string, float> formulaValues = new Dictionary<string, float>();
    foreach (KeyValuePair<string, string> keyValuePair in formulaColl)
    {
      string[] splitFormula = keyValuePair.Value.Split(new char[1]
      {
        ' '
      }, StringSplitOptions.RemoveEmptyEntries);
      if (splitFormula.Length > 1)
      {
        float[] operandValues = this.GetOperandValues(shapeType, ref formulaValues, splitFormula, isAdjValue);
        float resultValue = this.GetResultValue(splitFormula[0], operandValues);
        formulaValues.Add(keyValuePair.Key, resultValue);
      }
    }
    return formulaValues;
  }

  private float[] GetOperandValues(
    AutoShapeType shapeType,
    ref Dictionary<string, float> formulaValues,
    string[] splitFormula,
    bool isAdjValue)
  {
    string[] array = new string[34]
    {
      "3cd4",
      "3cd8",
      "5cd8",
      "7cd8",
      "b",
      "cd2",
      "cd4",
      "cd8",
      "h",
      "hc",
      "hd2",
      "hd3",
      "hd4",
      "hd5",
      "hd6",
      "hd8",
      "l",
      "ls",
      "r",
      "ss",
      "ssd2",
      "ssd4",
      "ssd6",
      "ssd8",
      "t",
      "vc",
      "w",
      "wd2",
      "wd3",
      "wd4",
      "wd5",
      "wd6",
      "wd8",
      "wd10"
    };
    Dictionary<string, float> dictionary = this._shapeGuide.Count <= 0 || isAdjValue ? this.GetDefaultPathAdjValues(shapeType) : this.GetPathAdjustValue(shapeType);
    float[] operandValues = new float[splitFormula.Length - 1];
    int index1 = 0;
    for (int index2 = 1; index2 < splitFormula.Length; ++index2)
    {
      if (!float.TryParse(splitFormula[index2], out operandValues[index1]))
      {
        if (Array.IndexOf<string>(array, splitFormula[index2]) > -1)
          operandValues[index1] = this.GetPresetOperandValue(splitFormula[index2]);
        else if (!isAdjValue && dictionary.ContainsKey(splitFormula[index2]))
          operandValues[index1] = dictionary[splitFormula[index2]];
        else if (formulaValues.ContainsKey(splitFormula[index2]))
          operandValues[index1] = formulaValues[splitFormula[index2]];
      }
      ++index1;
    }
    return operandValues;
  }

  private float GetPresetOperandValue(string operand)
  {
    switch (operand)
    {
      case "3cd4":
        return 270f;
      case "3cd8":
        return 135f;
      case "5cd8":
        return 225f;
      case "7cd8":
        return 315f;
      case "b":
        return this._rectBounds.Height;
      case "cd2":
        return 180f;
      case "cd4":
        return 90f;
      case "cd8":
        return 45f;
      case "h":
        return this._rectBounds.Height;
      case "hc":
        return this._rectBounds.Width / 2f;
      case "hd2":
        return this._rectBounds.Height / 2f;
      case "hd3":
        return this._rectBounds.Height / 3f;
      case "hd4":
        return this._rectBounds.Height / 4f;
      case "hd5":
        return this._rectBounds.Height / 5f;
      case "hd6":
        return this._rectBounds.Height / 6f;
      case "hd8":
        return this._rectBounds.Height / 8f;
      case "l":
        return 0.0f;
      case "ls":
        return Math.Max(this._rectBounds.Width, this._rectBounds.Height);
      case "r":
        return this._rectBounds.Width;
      case "ss":
        return Math.Min(this._rectBounds.Width, this._rectBounds.Height);
      case "ssd2":
        return Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 2f;
      case "ssd4":
        return Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 4f;
      case "ssd6":
        return Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 6f;
      case "ssd8":
        return Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 8f;
      case "t":
        return 0.0f;
      case "vc":
        return this._rectBounds.Height / 2f;
      case "w":
        return this._rectBounds.Width;
      case "wd2":
        return this._rectBounds.Width / 2f;
      case "wd3":
        return this._rectBounds.Width / 3f;
      case "wd4":
        return this._rectBounds.Width / 4f;
      case "wd5":
        return this._rectBounds.Width / 5f;
      case "wd6":
        return this._rectBounds.Width / 6f;
      case "wd8":
        return this._rectBounds.Width / 8f;
      case "wd10":
        return this._rectBounds.Width / 10f;
      default:
        return 0.0f;
    }
  }

  private float GetResultValue(string formula, float[] operandValues)
  {
    char[] array = new char[4]{ '*', '/', '+', '-' };
    float resultValue = operandValues[0];
    if (formula.Length > 1 && Array.IndexOf<char>(array, formula[0]) > -1)
    {
      int index1 = 0;
      for (int index2 = 0; index2 < formula.Length; ++index2)
      {
        ++index1;
        switch (formula[index2])
        {
          case '*':
            if ((double) operandValues[index1] != 0.0)
            {
              resultValue *= operandValues[index1];
              break;
            }
            break;
          case '+':
            resultValue += operandValues[index1];
            break;
          case '-':
            resultValue -= operandValues[index1];
            break;
          case '/':
            if ((double) operandValues[index1] != 0.0)
            {
              resultValue /= operandValues[index1];
              break;
            }
            break;
        }
      }
    }
    else
    {
      switch (formula)
      {
        case "?:":
          resultValue = (double) operandValues[0] > 0.0 ? operandValues[1] : operandValues[2];
          break;
        case "abs":
          resultValue = Math.Abs(operandValues[0]);
          break;
        case "at2":
          resultValue = (float) Math.Atan((double) operandValues[1] / (double) operandValues[0]);
          break;
        case "cat2":
          float d = (float) Math.Atan((double) operandValues[2] / (double) operandValues[1]);
          resultValue = operandValues[0] * (float) Math.Cos((double) d);
          break;
        case "cos":
          double num1 = Math.Cos((double) operandValues[1] * Math.PI / 180.0);
          resultValue = operandValues[0] * (float) num1;
          break;
        case "max":
          resultValue = Math.Max(operandValues[0], operandValues[1]);
          break;
        case "min":
          resultValue = Math.Min(operandValues[0], operandValues[1]);
          break;
        case "mod":
          resultValue = (float) Math.Sqrt(Math.Pow((double) operandValues[0], 2.0) + Math.Pow((double) operandValues[1], 2.0) + Math.Pow((double) operandValues[2], 2.0));
          break;
        case "pin":
          resultValue = (double) operandValues[1] >= (double) operandValues[0] ? ((double) operandValues[1] <= (double) operandValues[2] ? operandValues[1] : operandValues[2]) : operandValues[0];
          break;
        case "sat2":
          float a = (float) Math.Atan((double) operandValues[2] / (double) operandValues[1]);
          resultValue = operandValues[0] * (float) Math.Sin((double) a);
          break;
        case "sin":
          double num2 = Math.Sin((double) operandValues[1] * Math.PI / 180.0);
          resultValue = operandValues[0] * (float) num2;
          break;
        case "sqrt":
          resultValue = (float) Math.Sqrt((double) operandValues[0]);
          break;
        case "tan":
          double num3 = Math.Tan((double) operandValues[1] * Math.PI / 180.0);
          resultValue = operandValues[0] * (float) num3;
          break;
        case "val":
          resultValue = operandValues[0];
          break;
      }
    }
    return resultValue;
  }

  private Dictionary<string, string> GetShapeFormula(AutoShapeType shapeType)
  {
    Dictionary<string, string> shapeFormula = new Dictionary<string, string>();
    switch (shapeType)
    {
      case AutoShapeType.Parallelogram:
        shapeFormula.Add("maxAdj", "*/ 100000 w ss");
        shapeFormula.Add("a", "pin 0 adj maxAdj");
        shapeFormula.Add("x1", "*/ ss a 200000");
        shapeFormula.Add("x2", "*/ ss a 100000");
        shapeFormula.Add("x6", "+- r 0 x1");
        shapeFormula.Add("x5", "+- r 0 x2");
        shapeFormula.Add("x3", "*/ x5 1 2");
        shapeFormula.Add("x4", "+- r 0 x3");
        shapeFormula.Add("il1", "*/ wd2 a maxAdj");
        shapeFormula.Add("q1", "*/ 5 a maxAdj");
        shapeFormula.Add("q2", "+/ 1 q1 12");
        shapeFormula.Add("il", "*/ q2 w 1");
        shapeFormula.Add("it", "*/ q2 h 1");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 it");
        shapeFormula.Add("q3", "*/ h hc x2");
        shapeFormula.Add("y1", "pin 0 q3 h");
        shapeFormula.Add("y2", "+- b 0 y1");
        break;
      case AutoShapeType.Trapezoid:
        shapeFormula.Add("maxAdj", "*/ 50000 w ss");
        shapeFormula.Add("a", "pin 0 adj maxAdj");
        shapeFormula.Add("x1", "*/ ss a 200000");
        shapeFormula.Add("x2", "*/ ss a 100000");
        shapeFormula.Add("x3", "+- r 0 x2");
        shapeFormula.Add("x4", "+- r 0 x1");
        shapeFormula.Add("il", "*/ wd3 a maxAdj");
        shapeFormula.Add("it", "*/ hd3 a maxAdj");
        shapeFormula.Add("ir", "+- r 0 il");
        break;
      case AutoShapeType.Diamond:
        shapeFormula.Add("ir", "*/ w 3 4");
        shapeFormula.Add("ib", "*/ h 3 4");
        break;
      case AutoShapeType.RoundedRectangle:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "+- r 0 x1");
        shapeFormula.Add("y2", "+- b 0 x1");
        shapeFormula.Add("il", "*/ x1 29289 100000");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 il");
        break;
      case AutoShapeType.Octagon:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "+- r 0 x1");
        shapeFormula.Add("y2", "+- b 0 x1");
        shapeFormula.Add("il", "*/ x1 1 2");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 il");
        break;
      case AutoShapeType.IsoscelesTriangle:
        shapeFormula.Add("a", "pin 0 adj 100000");
        shapeFormula.Add("x1", "*/ w a 200000");
        shapeFormula.Add("x2", "*/ w a 100000");
        shapeFormula.Add("x3", "+- x1 wd2 0");
        break;
      case AutoShapeType.RightTriangle:
        shapeFormula.Add("it", "*/ h 7 12");
        shapeFormula.Add("ir", "*/ w 7 12");
        shapeFormula.Add("ib", "*/ h 11 12");
        break;
      case AutoShapeType.Oval:
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.Hexagon:
        shapeFormula.Add("maxAdj", "*/ 50000 w ss");
        shapeFormula.Add("a", "pin 0 adj maxAdj");
        shapeFormula.Add("shd2", "*/ hd2 vf 100000");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "+- r 0 x1");
        shapeFormula.Add("dy1", "sin shd2 60");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc dy1 0");
        shapeFormula.Add("q1", "*/ maxAdj -1 2");
        shapeFormula.Add("q2", "+- a q1 0");
        shapeFormula.Add("q3", "?: q2 4 2");
        shapeFormula.Add("q4", "?: q2 3 2");
        shapeFormula.Add("q5", "?: q2 q1 0");
        shapeFormula.Add("q6", "+/ a q5 q1");
        shapeFormula.Add("q7", "*/ q6 q4 -1");
        shapeFormula.Add("q8", "+- q3 q7 0");
        shapeFormula.Add("il", "*/ w q8 24");
        shapeFormula.Add("it", "*/ h q8 24");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 it");
        break;
      case AutoShapeType.Cross:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "+- r 0 x1");
        shapeFormula.Add("y2", "+- b 0 x1");
        shapeFormula.Add("d", "+- w 0 h");
        shapeFormula.Add("il", "?: d l x1");
        shapeFormula.Add("ir", "?: d r x2");
        shapeFormula.Add("it", "?: d x1 t");
        shapeFormula.Add("ib", "?: d y2 b");
        break;
      case AutoShapeType.RegularPentagon:
        shapeFormula.Add("swd2", "*/ wd2 hf 100000");
        shapeFormula.Add("shd2", "*/ hd2 vf 100000");
        shapeFormula.Add("svc", "*/ vc  vf 100000");
        shapeFormula.Add("dx1", "cos swd2 18");
        shapeFormula.Add("dx2", "cos swd2 306");
        shapeFormula.Add("dy1", "sin shd2 18");
        shapeFormula.Add("dy2", "sin shd2 306");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- svc 0 dy1");
        shapeFormula.Add("y2", "+- svc 0 dy2");
        shapeFormula.Add("it", "*/ y1 dx2 dx1");
        break;
      case AutoShapeType.Can:
        shapeFormula.Add("maxAdj", "*/ 50000 h ss");
        shapeFormula.Add("a", "pin 0 adj maxAdj");
        shapeFormula.Add("y1", "*/ ss a 200000");
        shapeFormula.Add("y2", "+- y1 y1 0");
        shapeFormula.Add("y3", "+- b 0 y1");
        break;
      case AutoShapeType.Cube:
        shapeFormula.Add("a", "pin 0 adj 100000");
        shapeFormula.Add("y1", "*/ ss a 100000");
        shapeFormula.Add("y4", "+- b 0 y1");
        shapeFormula.Add("y2", "*/ y4 1 2");
        shapeFormula.Add("y3", "+/ y1 b 2");
        shapeFormula.Add("x4", "+- r 0 y1");
        shapeFormula.Add("x2", "*/ x4 1 2");
        shapeFormula.Add("x3", "+/ y1 r 2");
        break;
      case AutoShapeType.Bevel:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "+- r 0 x1");
        shapeFormula.Add("y2", "+- b 0 x1");
        break;
      case AutoShapeType.FoldedCorner:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dy2", "*/ ss a 100000");
        shapeFormula.Add("dy1", "*/ dy2 1 5");
        shapeFormula.Add("x1", "+- r 0 dy2");
        shapeFormula.Add("x2", "+- x1 dy1 0");
        shapeFormula.Add("y2", "+- b 0 dy2");
        shapeFormula.Add("y1", "+- y2 dy1 0");
        break;
      case AutoShapeType.SmileyFace:
        shapeFormula.Add("a", "pin -4653 adj 4653");
        shapeFormula.Add("x1", "*/ w 4969 21699");
        shapeFormula.Add("x2", "*/ w 6215 21600");
        shapeFormula.Add("x3", "*/ w 13135 21600");
        shapeFormula.Add("x4", "*/ w 16640 21600");
        shapeFormula.Add("y1", "*/ h 7570 21600");
        shapeFormula.Add("y3", "*/ h 16515 21600");
        shapeFormula.Add("dy2", "*/ h a 100000");
        shapeFormula.Add("y2", "+- y3 0 dy2");
        shapeFormula.Add("y4", "+- y3 dy2 0");
        shapeFormula.Add("dy3", "*/ h a 50000");
        shapeFormula.Add("y5", "+- y4 dy3 0");
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        shapeFormula.Add("wR", "*/ w 1125 21600");
        shapeFormula.Add("hR", "*/ h 1125 21600");
        break;
      case AutoShapeType.Donut:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dr", "*/ ss a 100000");
        shapeFormula.Add("iwd2", "+- wd2 0 dr");
        shapeFormula.Add("ihd2", "+- hd2 0 dr");
        shapeFormula.Add("idx", "cos wd2 2700000");
        shapeFormula.Add("idy", "sin hd2 2700000");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.NoSymbol:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dr", "*/ ss a 100000");
        shapeFormula.Add("iwd2", "+- wd2 0 dr");
        shapeFormula.Add("ihd2", "+- hd2 0 dr");
        shapeFormula.Add("ang3", "at2 w h");
        shapeFormula.Add("ang", "*/ ang3 180 " + Math.PI.ToString());
        shapeFormula.Add("ct", "cos ihd2 ang");
        shapeFormula.Add("st", "sin iwd2 ang");
        shapeFormula.Add("m", "mod ct st 0");
        shapeFormula.Add("n", "*/ iwd2 ihd2 m");
        shapeFormula.Add("drd2", "*/ dr 1 2");
        shapeFormula.Add("dang3", "at2 n drd2");
        shapeFormula.Add("dang", "*/ dang3 180 " + Math.PI.ToString());
        shapeFormula.Add("2dang", "*/ dang 2 1");
        shapeFormula.Add("swAng", "+- -180 2dang 0");
        shapeFormula.Add("t4", "at2 w h");
        shapeFormula.Add("t3", "*/ t4 180 " + Math.PI.ToString());
        shapeFormula.Add("stAng1", "+- t3 0 dang");
        shapeFormula.Add("stAng2", "+- stAng1 0 cd2");
        shapeFormula.Add("ct1", "cos ihd2 stAng1");
        shapeFormula.Add("st1", "sin iwd2 stAng1");
        shapeFormula.Add("m1", "mod ct1 st1 0");
        shapeFormula.Add("n1", "*/ iwd2 ihd2 m1");
        shapeFormula.Add("dx1", "cos n1 stAng1");
        shapeFormula.Add("dy1", "sin n1 stAng1");
        shapeFormula.Add("x1", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc dy1 0");
        shapeFormula.Add("x2", "+- hc 0 dx1");
        shapeFormula.Add("y2", "+- vc 0 dy1");
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.BlockArc:
        shapeFormula.Add("stAng", "pin 0 adj1 21599999");
        shapeFormula.Add("istAng", "pin 0 adj2 21599999");
        shapeFormula.Add("a3", "pin 0 adj3 50000");
        shapeFormula.Add("sw11", "+- istAng 0 stAng");
        shapeFormula.Add("sw12", "+- sw11 21600000 0");
        shapeFormula.Add("swAng", "?: sw11 sw11 sw12");
        shapeFormula.Add("iswAng", "+- 0 0 swAng");
        shapeFormula.Add("wt1", "sin wd2 stAng");
        shapeFormula.Add("ht1", "cos hd2 stAng");
        shapeFormula.Add("wt3", "sin wd2 istAng");
        shapeFormula.Add("ht3", "cos hd2 istAng");
        shapeFormula.Add("dx1", "cat2 wd2 ht1 wt1");
        shapeFormula.Add("dy1", "sat2 hd2 ht1 wt1");
        shapeFormula.Add("dx3", "cat2 wd2 ht3 wt3");
        shapeFormula.Add("dy3", "sat2 hd2 ht3 wt3");
        shapeFormula.Add("x1", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc dy1 0");
        shapeFormula.Add("x3", "+- hc dx3 0");
        shapeFormula.Add("y3", "+- vc dy3 0");
        shapeFormula.Add("dr", "*/ ss a3 100000");
        shapeFormula.Add("iwd2", "+- wd2 0 dr");
        shapeFormula.Add("ihd2", "+- hd2 0 dr");
        shapeFormula.Add("wt2", "sin iwd2 istAng");
        shapeFormula.Add("ht2", "cos ihd2 istAng");
        shapeFormula.Add("wt4", "sin iwd2 stAng");
        shapeFormula.Add("ht4", "cos ihd2 stAng");
        shapeFormula.Add("dx2", "cat2 iwd2 ht2 wt2");
        shapeFormula.Add("dy2", "sat2 ihd2 ht2 wt2");
        shapeFormula.Add("dx4", "cat2 iwd2 ht4 wt4");
        shapeFormula.Add("dy4", "sat2 ihd2 ht4 wt4");
        shapeFormula.Add("x2", "+- hc dx2 0");
        shapeFormula.Add("y2", "+- vc dy2 0");
        shapeFormula.Add("x4", "+- hc dx4 0");
        shapeFormula.Add("y4", "+- vc dy4 0");
        shapeFormula.Add("sw0", "+- 21600000 0 stAng");
        shapeFormula.Add("da1", "+- swAng 0 sw0");
        shapeFormula.Add("g1", "max x1 x2");
        shapeFormula.Add("g2", "max x3 x4");
        shapeFormula.Add("g3", "max g1 g2");
        shapeFormula.Add("ir", "?: da1 r g3");
        shapeFormula.Add("sw1", "+- cd4 0 stAng");
        shapeFormula.Add("sw2", "+- 27000000 0 stAng");
        shapeFormula.Add("sw3", "?: sw1 sw1 sw2");
        shapeFormula.Add("da2", "+- swAng 0 sw3");
        shapeFormula.Add("g5", "max y1 y2");
        shapeFormula.Add("g6", "max y3 y4");
        shapeFormula.Add("g7", "max g5 g6");
        shapeFormula.Add("ib", "?: da2 b g7");
        shapeFormula.Add("sw4", "+- cd2 0 stAng");
        shapeFormula.Add("sw5", "+- 32400000 0 stAng");
        shapeFormula.Add("sw6", "?: sw4 sw4 sw5");
        shapeFormula.Add("da3", "+- swAng 0 sw6");
        shapeFormula.Add("g9", "min x1 x2");
        shapeFormula.Add("g10", "min x3 x4");
        shapeFormula.Add("g11", "min g9 g10");
        shapeFormula.Add("il", "?: da3 l g11");
        shapeFormula.Add("sw7", "+- 3cd4 0 stAng");
        shapeFormula.Add("sw8", "+- 37800000 0 stAng");
        shapeFormula.Add("sw9", "?: sw7 sw7 sw8");
        shapeFormula.Add("da4", "+- swAng 0 sw9");
        shapeFormula.Add("g13", "min y1 y2");
        shapeFormula.Add("g14", "min y3 y4");
        shapeFormula.Add("g15", "min g13 g14");
        shapeFormula.Add("it", "?: da4 t g15");
        shapeFormula.Add("x5", "+/ x1 x4 2");
        shapeFormula.Add("y5", "+/ y1 y4 2");
        shapeFormula.Add("x6", "+/ x3 x2 2");
        shapeFormula.Add("y6", "+/ y3 y2 2");
        shapeFormula.Add("cang1", "+- stAng 0 cd4");
        shapeFormula.Add("cang2", "+- istAng cd4 0");
        shapeFormula.Add("cang3", "+/ cang1 cang2 2");
        break;
      case AutoShapeType.Heart:
        shapeFormula.Add("dx1", "*/ w 49 48");
        shapeFormula.Add("dx2", "*/ w 10 48");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- t 0 hd3");
        shapeFormula.Add("il", "*/ w 1 6");
        shapeFormula.Add("ir", "*/ w 5 6");
        shapeFormula.Add("ib", "*/ h 2 3");
        break;
      case AutoShapeType.LightningBolt:
        shapeFormula.Add("x1", "*/ w 5022 21600");
        shapeFormula.Add("x3", "*/ w 8472 21600");
        shapeFormula.Add("x4", "*/ w 8757 21600");
        shapeFormula.Add("x5", "*/ w 10012 21600");
        shapeFormula.Add("x8", "*/ w 12860 21600");
        shapeFormula.Add("x9", "*/ w 13917 21600");
        shapeFormula.Add("x11", "*/ w 16577 21600");
        shapeFormula.Add("y1", "*/ h 3890 21600");
        shapeFormula.Add("y2", "*/ h 6080 21600");
        shapeFormula.Add("y4", "*/ h 7437 21600");
        shapeFormula.Add("y6", "*/ h 9705 21600");
        shapeFormula.Add("y7", "*/ h 12007 21600");
        shapeFormula.Add("y10", "*/ h 14277 21600");
        shapeFormula.Add("y11", "*/ h 14915 21600");
        break;
      case AutoShapeType.Sun:
        shapeFormula.Add("a", "pin 12500 adj 46875");
        shapeFormula.Add("g0", "+- 50000 0 a");
        shapeFormula.Add("g1", "*/ g0 30274 32768");
        shapeFormula.Add("g2", "*/ g0 12540 32768");
        shapeFormula.Add("g3", "+- g1 50000 0");
        shapeFormula.Add("g4", "+- g2 50000 0");
        shapeFormula.Add("g5", "+- 50000 0 g1");
        shapeFormula.Add("g6", "+- 50000 0 g2");
        shapeFormula.Add("g7", "*/ g0 23170 32768");
        shapeFormula.Add("g8", "+- 50000 g7 0");
        shapeFormula.Add("g9", "+- 50000 0 g7");
        shapeFormula.Add("g10", "*/ g5 3 4");
        shapeFormula.Add("g11", "*/ g6 3 4");
        shapeFormula.Add("g12", "+- g10 3662 0");
        shapeFormula.Add("g13", "+- g11 3662 0");
        shapeFormula.Add("g14", "+- g11 12500 0");
        shapeFormula.Add("g15", "+- 100000 0 g10");
        shapeFormula.Add("g16", "+- 100000 0 g12");
        shapeFormula.Add("g17", "+- 100000 0 g13");
        shapeFormula.Add("g18", "+- 100000 0 g14");
        shapeFormula.Add("ox1", "*/ w 18436 21600");
        shapeFormula.Add("oy1", "*/ h 3163 21600");
        shapeFormula.Add("ox2", "*/ w 3163 21600");
        shapeFormula.Add("oy2", "*/ h 18436 21600");
        shapeFormula.Add("x8", "*/ w g8 100000");
        shapeFormula.Add("x9", "*/ w g9 100000");
        shapeFormula.Add("x10", "*/ w g10 100000");
        shapeFormula.Add("x12", "*/ w g12 100000");
        shapeFormula.Add("x13", "*/ w g13 100000");
        shapeFormula.Add("x14", "*/ w g14 100000");
        shapeFormula.Add("x15", "*/ w g15 100000");
        shapeFormula.Add("x16", "*/ w g16 100000");
        shapeFormula.Add("x17", "*/ w g17 100000");
        shapeFormula.Add("x18", "*/ w g18 100000");
        shapeFormula.Add("x19", "*/ w a 100000");
        shapeFormula.Add("wR", "*/ w g0 100000");
        shapeFormula.Add("hR", "*/ h g0 100000");
        shapeFormula.Add("y8", "*/ h g8 100000");
        shapeFormula.Add("y9", "*/ h g9 100000");
        shapeFormula.Add("y10", "*/ h g10 100000");
        shapeFormula.Add("y12", "*/ h g12 100000");
        shapeFormula.Add("y13", "*/ h g13 100000");
        shapeFormula.Add("y14", "*/ h g14 100000");
        shapeFormula.Add("y15", "*/ h g15 100000");
        shapeFormula.Add("y16", "*/ h g16 100000");
        shapeFormula.Add("y17", "*/ h g17 100000");
        shapeFormula.Add("y18", "*/ h g18 100000");
        break;
      case AutoShapeType.Moon:
        shapeFormula.Add("a", "pin 0 adj 87500");
        shapeFormula.Add("g0", "*/ ss a 100000");
        shapeFormula.Add("g0w", "*/ g0 w ss");
        shapeFormula.Add("g1", "+- ss 0 g0");
        shapeFormula.Add("g2", "*/ g0 g0 g1");
        shapeFormula.Add("g3", "*/ ss ss g1");
        shapeFormula.Add("g4", "*/ g3 2 1");
        shapeFormula.Add("g5", "+- g4 0 g2");
        shapeFormula.Add("g6", "+- g5 0 g0");
        shapeFormula.Add("g6w", "*/ g6 w ss");
        shapeFormula.Add("g7", "*/ g5 1 2");
        shapeFormula.Add("g8", "+- g7 0 g0");
        shapeFormula.Add("dy1", "*/ g8 hd2 ss");
        shapeFormula.Add("g10h", "+- vc 0 dy1");
        shapeFormula.Add("g11h", "+- vc dy1 0");
        shapeFormula.Add("g12", "*/ g0 9598 32768");
        shapeFormula.Add("g12w", "*/ g12 w ss");
        shapeFormula.Add("g13", "+- ss 0 g12");
        shapeFormula.Add("q1", "*/ ss ss 1");
        shapeFormula.Add("q2", "*/ g13 g13 1");
        shapeFormula.Add("q3", "+- q1 0 q2");
        shapeFormula.Add("q4", "sqrt q3");
        shapeFormula.Add("dy4", "*/ q4 hd2 ss");
        shapeFormula.Add("g15h", "+- vc 0 dy4");
        shapeFormula.Add("g16h", "+- vc dy4 0");
        shapeFormula.Add("g17w", "+- g6w 0 g0w");
        shapeFormula.Add("g18w", "*/ g17w 1 2");
        shapeFormula.Add("dx2p", "+- g0w g18w w");
        shapeFormula.Add("dx2", "*/ dx2p -1 1");
        shapeFormula.Add("dy2", "*/ hd2 -1 1");
        shapeFormula.Add("stAng", "at2 dx2 dy2");
        shapeFormula.Add("stAng1", "*/ stAng 180 " + Math.PI.ToString());
        shapeFormula.Add("enAngp", "at2 dx2 hd2");
        shapeFormula.Add("enAngp1", "*/ enAngp 180 " + Math.PI.ToString());
        shapeFormula.Add("enAng1", "+- enAngp1 0 360");
        shapeFormula.Add("swAng1", "+- enAng1 0 stAng1");
        break;
      case AutoShapeType.Arc:
        shapeFormula.Add("stAng", "pin 0 adj1 21599999");
        shapeFormula.Add("enAng", "pin 0 adj2 21599999");
        shapeFormula.Add("sw11", "+- enAng 0 stAng");
        shapeFormula.Add("sw12", "+- sw11 21600000 0");
        shapeFormula.Add("swAng", "?: sw11 sw11 sw12");
        shapeFormula.Add("wt1", "sin wd2 stAng");
        shapeFormula.Add("ht1", "cos hd2 stAng");
        shapeFormula.Add("dx1", "cat2 wd2 ht1 wt1");
        shapeFormula.Add("dy1", "sat2 hd2 ht1 wt1");
        shapeFormula.Add("wt2", "sin wd2 enAng");
        shapeFormula.Add("ht2", "cos hd2 enAng");
        shapeFormula.Add("dx2", "cat2 wd2 ht2 wt2");
        shapeFormula.Add("dy2", "sat2 hd2 ht2 wt2");
        shapeFormula.Add("x1", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc dy1 0");
        shapeFormula.Add("x2", "+- hc dx2 0");
        shapeFormula.Add("y2", "+- vc dy2 0");
        shapeFormula.Add("sw0", "+- 21600000 0 stAng");
        shapeFormula.Add("da1", "+- swAng 0 sw0");
        shapeFormula.Add("g1", "max x1 x2");
        shapeFormula.Add("ir", "?: da1 r g1");
        shapeFormula.Add("sw1", "+- cd4 0 stAng");
        shapeFormula.Add("sw2", "+- 27000000 0 stAng");
        shapeFormula.Add("sw3", "?: sw1 sw1 sw2");
        shapeFormula.Add("da2", "+- swAng 0 sw3");
        shapeFormula.Add("g5", "max y1 y2");
        shapeFormula.Add("ib", "?: da2 b g5");
        shapeFormula.Add("sw4", "+- cd2 0 stAng");
        shapeFormula.Add("sw5", "+- 32400000 0 stAng");
        shapeFormula.Add("sw6", "?: sw4 sw4 sw5");
        shapeFormula.Add("da3", "+- swAng 0 sw6");
        shapeFormula.Add("g9", "min x1 x2");
        shapeFormula.Add("il", "?: da3 l g9");
        shapeFormula.Add("sw7", "+- 3cd4 0 stAng");
        shapeFormula.Add("sw8", "+- 37800000 0 stAng");
        shapeFormula.Add("sw9", "?: sw7 sw7 sw8");
        shapeFormula.Add("da4", "+- swAng 0 sw9");
        shapeFormula.Add("g13", "min y1 y2");
        shapeFormula.Add("it", "?: da4 t g13");
        shapeFormula.Add("cang1", "+- stAng 0 cd4");
        shapeFormula.Add("cang2", "+- enAng cd4 0");
        shapeFormula.Add("cang3", "+/ cang1 cang2 2");
        break;
      case AutoShapeType.DoubleBracket:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "+- r 0 x1");
        shapeFormula.Add("y2", "+- b 0 x1");
        shapeFormula.Add("il", "*/ x1 29289 100000");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 il");
        break;
      case AutoShapeType.DoubleBrace:
        shapeFormula.Add("a", "pin 0 adj 25000");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "*/ ss a 50000");
        shapeFormula.Add("x3", "+- r 0 x2");
        shapeFormula.Add("x4", "+- r 0 x1");
        shapeFormula.Add("y2", "+- vc 0 x1");
        shapeFormula.Add("y3", "+- vc x1 0");
        shapeFormula.Add("y4", "+- b 0 x1");
        shapeFormula.Add("it", "*/ x1 29289 100000");
        shapeFormula.Add("il", "+- x1 it 0");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 it");
        break;
      case AutoShapeType.Plaque:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "+- r 0 x1");
        shapeFormula.Add("y2", "+- b 0 x1");
        shapeFormula.Add("il", "*/ x1 70711 100000");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 il");
        break;
      case AutoShapeType.LeftBracket:
        shapeFormula.Add("maxAdj", "*/ 50000 h ss");
        shapeFormula.Add("a", "pin 0 adj maxAdj");
        shapeFormula.Add("y1", "*/ ss a 100000");
        shapeFormula.Add("y2", "+- b 0 y1");
        shapeFormula.Add("dx1", "cos w 2700000");
        shapeFormula.Add("dy1", "sin y1 2700000");
        shapeFormula.Add("il", "+- r 0 dx1");
        shapeFormula.Add("it", "+- y1 0 dy1");
        shapeFormula.Add("ib", "+- b dy1 y1");
        break;
      case AutoShapeType.RightBracket:
        shapeFormula.Add("maxAdj", "*/ 50000 h ss");
        shapeFormula.Add("a", "pin 0 adj maxAdj");
        shapeFormula.Add("y1", "*/ ss a 100000");
        shapeFormula.Add("y2", "+- b 0 y1");
        shapeFormula.Add("dx1", "cos w 2700000");
        shapeFormula.Add("dy1", "sin y1 2700000");
        shapeFormula.Add("ir", "+- l dx1 0");
        shapeFormula.Add("it", "+- y1 0 dy1");
        shapeFormula.Add("ib", "+- b dy1 y1");
        break;
      case AutoShapeType.LeftBrace:
        shapeFormula.Add("a2", "pin 0 adj2 100000");
        shapeFormula.Add("q1", "+- 100000 0 a2");
        shapeFormula.Add("q2", "min q1 a2");
        shapeFormula.Add("q3", "*/ q2 1 2");
        shapeFormula.Add("maxAdj1", "*/ q3 h ss");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("y1", "*/ ss a1 100000");
        shapeFormula.Add("y3", "*/ h a2 100000");
        shapeFormula.Add("y4", "+- y3 y1 0");
        shapeFormula.Add("dx1", "cos wd2 2700000");
        shapeFormula.Add("dy1", "sin y1 2700000");
        shapeFormula.Add("il", "+- r 0 dx1");
        shapeFormula.Add("it", "+- y1 0 dy1");
        shapeFormula.Add("ib", "+- b dy1 y1");
        break;
      case AutoShapeType.RightBrace:
        shapeFormula.Add("a2", "pin 0 adj2 100000");
        shapeFormula.Add("q1", "+- 100000 0 a2");
        shapeFormula.Add("q2", "min q1 a2");
        shapeFormula.Add("q3", "*/ q2 1 2");
        shapeFormula.Add("maxAdj1", "*/ q3 h ss");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("y1", "*/ ss a1 100000");
        shapeFormula.Add("y3", "*/ h a2 100000");
        shapeFormula.Add("y2", "+- y3 0 y1");
        shapeFormula.Add("y4", "+- b 0 y1");
        shapeFormula.Add("dx1", "cos wd2 2700000");
        shapeFormula.Add("dy1", "sin y1 2700000");
        shapeFormula.Add("ir", "+- l dx1 0");
        shapeFormula.Add("it", "+- y1 0 dy1");
        shapeFormula.Add("ib", "+- b dy1 y1");
        break;
      case AutoShapeType.RightArrow:
        shapeFormula.Add("maxAdj2", "*/ 100000 w ss");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("dx1", "*/ ss a2 100000");
        shapeFormula.Add("x1", "+- r 0 dx1");
        shapeFormula.Add("dy1", "*/ h a1 200000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc dy1 0");
        shapeFormula.Add("dx2", "*/ y1 dx1 hd2");
        shapeFormula.Add("x2", "+- x1 dx2 0");
        break;
      case AutoShapeType.LeftArrow:
        shapeFormula.Add("maxAdj2", "*/ 100000 w ss");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("dx2", "*/ ss a2 100000");
        shapeFormula.Add("x2", "+- l dx2 0");
        shapeFormula.Add("dy1", "*/ h a1 200000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc dy1 0");
        shapeFormula.Add("dx1", "*/ y1 dx2 hd2");
        shapeFormula.Add("x1", "+- x2  0 dx1");
        break;
      case AutoShapeType.UpArrow:
        shapeFormula.Add("maxAdj2", "*/ 100000 h ss");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("dy2", "*/ ss a2 100000");
        shapeFormula.Add("y2", "+- t dy2 0");
        shapeFormula.Add("dx1", "*/ w a1 200000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc dx1 0");
        shapeFormula.Add("dy1", "*/ x1 dy2 wd2");
        shapeFormula.Add("y1", "+- y2  0 dy1");
        break;
      case AutoShapeType.DownArrow:
        shapeFormula.Add("maxAdj2", "*/ 100000 h ss");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("dy1", "*/ ss a2 100000");
        shapeFormula.Add("y1", "+- b 0 dy1");
        shapeFormula.Add("dx1", "*/ w a1 200000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc dx1 0");
        shapeFormula.Add("dy2", "*/ x1 dy1 wd2");
        shapeFormula.Add("y2", "+- y1 dy2 0");
        break;
      case AutoShapeType.LeftRightArrow:
        shapeFormula.Add("maxAdj2", "*/ 50000 w ss");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("x2", "*/ ss a2 100000");
        shapeFormula.Add("x3", "+- r 0 x2");
        shapeFormula.Add("dy", "*/ h a1 200000");
        shapeFormula.Add("y1", "+- vc 0 dy");
        shapeFormula.Add("y2", "+- vc dy 0");
        shapeFormula.Add("dx1", "*/ y1 x2 hd2");
        shapeFormula.Add("x1", "+- x2 0 dx1");
        shapeFormula.Add("x4", "+- x3 dx1 0");
        break;
      case AutoShapeType.UpDownArrow:
        shapeFormula.Add("maxAdj2", "*/ 50000 h ss");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("y2", "*/ ss a2 100000");
        shapeFormula.Add("y3", "+- b 0 y2");
        shapeFormula.Add("dx1", "*/ w a1 200000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc dx1 0");
        shapeFormula.Add("dy1", "*/ x1 y2 wd2");
        shapeFormula.Add("y1", "+- y2 0 dy1");
        shapeFormula.Add("y4", "+- y3 dy1 0");
        break;
      case AutoShapeType.QuadArrow:
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("q1", "+- 100000 0 maxAdj1");
        shapeFormula.Add("maxAdj3", "*/ q1 1 2");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("x1", "*/ ss a3 100000");
        shapeFormula.Add("dx2", "*/ ss a2 100000");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x5", "+- hc dx2 0");
        shapeFormula.Add("dx3", "*/ ss a1 200000");
        shapeFormula.Add("x3", "+- hc 0 dx3");
        shapeFormula.Add("x4", "+- hc dx3 0");
        shapeFormula.Add("x6", "+- r 0 x1");
        shapeFormula.Add("y2", "+- vc 0 dx2");
        shapeFormula.Add("y5", "+- vc dx2 0");
        shapeFormula.Add("y3", "+- vc 0 dx3");
        shapeFormula.Add("y4", "+- vc dx3 0");
        shapeFormula.Add("y6", "+- b 0 x1");
        shapeFormula.Add("il", "*/ dx3 x1 dx2");
        shapeFormula.Add("ir", "+- r 0 il");
        break;
      case AutoShapeType.LeftRightUpArrow:
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("q1", "+- 100000 0 maxAdj1");
        shapeFormula.Add("maxAdj3", "*/ q1 1 2");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("x1", "*/ ss a3 100000");
        shapeFormula.Add("dx2", "*/ ss a2 100000");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x5", "+- hc dx2 0");
        shapeFormula.Add("dx3", "*/ ss a1 200000");
        shapeFormula.Add("x3", "+- hc 0 dx3");
        shapeFormula.Add("x4", "+- hc dx3 0");
        shapeFormula.Add("x6", "+- r 0 x1");
        shapeFormula.Add("dy2", "*/ ss a2 50000");
        shapeFormula.Add("y2", "+- b 0 dy2");
        shapeFormula.Add("y4", "+- b 0 dx2");
        shapeFormula.Add("y3", "+- y4 0 dx3");
        shapeFormula.Add("y5", "+- y4 dx3 0");
        shapeFormula.Add("il", "*/ dx3 x1 dx2");
        shapeFormula.Add("ir", "+- r 0 il");
        break;
      case AutoShapeType.BentArrow:
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("a3", "pin 0 adj3 50000");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("aw2", "*/ ss a2 100000");
        shapeFormula.Add("th2", "*/ th 1 2");
        shapeFormula.Add("dh2", "+- aw2 0 th2");
        shapeFormula.Add("ah", "*/ ss a3 100000");
        shapeFormula.Add("bw", "+- r 0 ah");
        shapeFormula.Add("bh", "+- b 0 dh2");
        shapeFormula.Add("bs", "min bw bh");
        shapeFormula.Add("maxAdj4", "*/ 100000 bs ss");
        shapeFormula.Add("a4", "pin 0 adj4 maxAdj4");
        shapeFormula.Add("bd", "*/ ss a4 100000");
        shapeFormula.Add("bd3", "+- bd 0 th");
        shapeFormula.Add("bd2", "max bd3 0");
        shapeFormula.Add("x3", "+- th bd2 0");
        shapeFormula.Add("x4", "+- r 0 ah");
        shapeFormula.Add("y3", "+- dh2 th 0");
        shapeFormula.Add("y4", "+- y3 dh2 0");
        shapeFormula.Add("y5", "+- dh2 bd 0");
        shapeFormula.Add("y6", "+- y3 bd2 0");
        break;
      case AutoShapeType.UTurnArrow:
        shapeFormula.Add("a2", "pin 0 adj2 25000");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("q2", "*/ a1 ss h");
        shapeFormula.Add("q3", "+- 100000 0 q2");
        shapeFormula.Add("maxAdj3", "*/ q3 h ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("q1", "+- a3 a1 0");
        shapeFormula.Add("minAdj5", "*/ q1 ss h");
        shapeFormula.Add("a5", "pin minAdj5 adj5 100000");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("aw2", "*/ ss a2 100000");
        shapeFormula.Add("th2", "*/ th 1 2");
        shapeFormula.Add("dh2", "+- aw2 0 th2");
        shapeFormula.Add("y5", "*/ h a5 100000");
        shapeFormula.Add("ah", "*/ ss a3 100000");
        shapeFormula.Add("y4", "+- y5 0 ah");
        shapeFormula.Add("x9", "+- r 0 dh2");
        shapeFormula.Add("bw", "*/ x9 1 2");
        shapeFormula.Add("bs", "min bw y4");
        shapeFormula.Add("maxAdj4", "*/ bs 100000 ss");
        shapeFormula.Add("a4", "pin 0 adj4 maxAdj4");
        shapeFormula.Add("bd", "*/ ss a4 100000");
        shapeFormula.Add("bd3", "+- bd 0 th");
        shapeFormula.Add("bd2", "max bd3 0");
        shapeFormula.Add("x3", "+- th bd2 0");
        shapeFormula.Add("x8", "+- r 0 aw2");
        shapeFormula.Add("x6", "+- x8 0 aw2");
        shapeFormula.Add("x7", "+- x6 dh2 0");
        shapeFormula.Add("x4", "+- x9 0 bd");
        shapeFormula.Add("x5", "+- x7 0 bd2");
        shapeFormula.Add("cx", "+/ th x7 2");
        break;
      case AutoShapeType.LeftUpArrow:
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("maxAdj3", "+- 100000 0 maxAdj1");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("x1", "*/ ss a3 100000");
        shapeFormula.Add("dx2", "*/ ss a2 50000");
        shapeFormula.Add("x2", "+- r 0 dx2");
        shapeFormula.Add("y2", "+- b 0 dx2");
        shapeFormula.Add("dx4", "*/ ss a2 100000");
        shapeFormula.Add("x4", "+- r 0 dx4");
        shapeFormula.Add("y4", "+- b 0 dx4");
        shapeFormula.Add("dx3", "*/ ss a1 200000");
        shapeFormula.Add("x3", "+- x4 0 dx3");
        shapeFormula.Add("x5", "+- x4 dx3 0");
        shapeFormula.Add("y3", "+- y4 0 dx3");
        shapeFormula.Add("y5", "+- y4 dx3 0");
        shapeFormula.Add("il", "*/ dx3 x1 dx4");
        shapeFormula.Add("cx1", "+/ x1 x5 2");
        shapeFormula.Add("cy1", "+/ x1 y5 2");
        break;
      case AutoShapeType.BentUpArrow:
        shapeFormula.Add("a1", "pin 0 adj1 50000");
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("a3", "pin 0 adj3 50000");
        shapeFormula.Add("y1", "*/ ss a3 100000");
        shapeFormula.Add("dx1", "*/ ss a2 50000");
        shapeFormula.Add("x1", "+- r 0 dx1");
        shapeFormula.Add("dx3", "*/ ss a2 100000");
        shapeFormula.Add("x3", "+- r 0 dx3");
        shapeFormula.Add("dx2", "*/ ss a1 200000");
        shapeFormula.Add("x2", "+- x3 0 dx2");
        shapeFormula.Add("x4", "+- x3 dx2 0");
        shapeFormula.Add("dy2", "*/ ss a1 100000");
        shapeFormula.Add("y2", "+- b 0 dy2");
        shapeFormula.Add("x0", "*/ x4 1 2");
        shapeFormula.Add("y3", "+/ y2 b 2");
        shapeFormula.Add("y15", "+/ y1 b 2");
        break;
      case AutoShapeType.CurvedRightArrow:
        shapeFormula.Add("maxAdj2", "*/ 50000 h ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("a1", "pin 0 adj1 a2");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("aw", "*/ ss a2 100000");
        shapeFormula.Add("q1", "+/ th aw 4");
        shapeFormula.Add("hR", "+- hd2 0 q1");
        shapeFormula.Add("q7", "*/ hR 2 1");
        shapeFormula.Add("q8", "*/ q7 q7 1");
        shapeFormula.Add("q9", "*/ th th 1");
        shapeFormula.Add("q10", "+- q8 0 q9");
        shapeFormula.Add("q11", "sqrt q10");
        shapeFormula.Add("idx", "*/ q11 w q7");
        shapeFormula.Add("maxAdj3", "*/ 100000 idx ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("ah", "*/ ss a3 100000");
        shapeFormula.Add("y3", "+- hR th 0");
        shapeFormula.Add("q2", "*/ w w 1");
        shapeFormula.Add("q3", "*/ ah ah 1");
        shapeFormula.Add("q4", "+- q2 0 q3");
        shapeFormula.Add("q5", "sqrt q4");
        shapeFormula.Add("dy", "*/ q5 hR w");
        shapeFormula.Add("y5", "+- hR dy 0");
        shapeFormula.Add("y7", "+- y3 dy 0");
        shapeFormula.Add("q6", "+- aw 0 th");
        shapeFormula.Add("dh", "*/ q6 1 2");
        shapeFormula.Add("y4", "+- y5 0 dh");
        shapeFormula.Add("y8", "+- y7 dh 0");
        shapeFormula.Add("aw2", "*/ aw 1 2");
        shapeFormula.Add("y6", "+- b 0 aw2");
        shapeFormula.Add("x1", "+- r 0 ah");
        shapeFormula.Add("swAng0", "at2 ah dy");
        shapeFormula.Add("swAng", "*/ swAng0 180 " + Math.PI.ToString());
        shapeFormula.Add("stAng", "+- cd2 0 swAng");
        shapeFormula.Add("mswAng", "+- 0 0 swAng");
        shapeFormula.Add("ix", "+- r 0 idx");
        shapeFormula.Add("iy", "+/ hR y3 2");
        shapeFormula.Add("q12", "*/ th 1 2");
        shapeFormula.Add("dang0", "at2 idx q12");
        shapeFormula.Add("dang2", "*/ dang0 180 " + Math.PI.ToString());
        shapeFormula.Add("swAng2", "+- dang2 0 cd4");
        shapeFormula.Add("swAng3", "+- cd4 dang2 0");
        shapeFormula.Add("stAng3", "+- cd2 0 dang2");
        break;
      case AutoShapeType.CurvedLeftArrow:
        shapeFormula.Add("maxAdj2", "*/ 50000 h ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("a1", "pin 0 adj1 a2");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("aw", "*/ ss a2 100000");
        shapeFormula.Add("q1", "+/ th aw 4");
        shapeFormula.Add("hR", "+- hd2 0 q1");
        shapeFormula.Add("q7", "*/ hR 2 1");
        shapeFormula.Add("q8", "*/ q7 q7 1");
        shapeFormula.Add("q9", "*/ th th 1");
        shapeFormula.Add("q10", "+- q8 0 q9");
        shapeFormula.Add("q11", "sqrt q10");
        shapeFormula.Add("idx", "*/ q11 w q7");
        shapeFormula.Add("maxAdj3", "*/ 100000 idx ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("ah", "*/ ss a3 100000");
        shapeFormula.Add("y3", "+- hR th 0");
        shapeFormula.Add("q2", "*/ w w 1");
        shapeFormula.Add("q3", "*/ ah ah 1");
        shapeFormula.Add("q4", "+- q2 0 q3");
        shapeFormula.Add("q5", "sqrt q4");
        shapeFormula.Add("dy", "*/ q5 hR w");
        shapeFormula.Add("y5", "+- hR dy 0");
        shapeFormula.Add("y7", "+- y3 dy 0");
        shapeFormula.Add("q6", "+- aw 0 th");
        shapeFormula.Add("dh", "*/ q6 1 2");
        shapeFormula.Add("y4", "+- y5 0 dh");
        shapeFormula.Add("y8", "+- y7 dh 0");
        shapeFormula.Add("aw2", "*/ aw 1 2");
        shapeFormula.Add("y6", "+- b 0 aw2");
        shapeFormula.Add("x1", "+- l ah 0");
        shapeFormula.Add("swAng1", "at2 ah dy");
        shapeFormula.Add("swAng", "*/ swAng1 180 " + Math.PI.ToString());
        shapeFormula.Add("mswAng", "+- 0 0 swAng");
        shapeFormula.Add("ix", "+- l idx 0");
        shapeFormula.Add("iy", "+/ hR y3 2");
        shapeFormula.Add("q12", "*/ th 1 2");
        shapeFormula.Add("dang3", "at2 idx q12");
        shapeFormula.Add("dang2", "*/ dang3 180 " + Math.PI.ToString());
        shapeFormula.Add("swAng2", "+- dang2 0 swAng");
        shapeFormula.Add("swAng3", "+- swAng dang2 0");
        shapeFormula.Add("stAng3", "+- 0 0 dang2");
        break;
      case AutoShapeType.CurvedUpArrow:
        shapeFormula.Add("maxAdj2", "*/ 50000 w ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("aw", "*/ ss a2 100000");
        shapeFormula.Add("q1", "+/ th aw 4");
        shapeFormula.Add("wR", "+- wd2 0 q1");
        shapeFormula.Add("q7", "*/ wR 2 1");
        shapeFormula.Add("q8", "*/ q7 q7 1");
        shapeFormula.Add("q9", "*/ th th 1");
        shapeFormula.Add("q10", "+- q8 0 q9");
        shapeFormula.Add("q11", "sqrt q10");
        shapeFormula.Add("idy", "*/ q11 h q7");
        shapeFormula.Add("maxAdj3", "*/ 100000 idy ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("ah", "*/ ss adj3 100000");
        shapeFormula.Add("x3", "+- wR th 0");
        shapeFormula.Add("q2", "*/ h h 1");
        shapeFormula.Add("q3", "*/ ah ah 1");
        shapeFormula.Add("q4", "+- q2 0 q3");
        shapeFormula.Add("q5", "sqrt q4");
        shapeFormula.Add("dx", "*/ q5 wR h");
        shapeFormula.Add("x5", "+- wR dx 0");
        shapeFormula.Add("x7", "+- x3 dx 0");
        shapeFormula.Add("q6", "+- aw 0 th");
        shapeFormula.Add("dh", "*/ q6 1 2");
        shapeFormula.Add("x4", "+- x5 0 dh");
        shapeFormula.Add("x8", "+- x7 dh 0");
        shapeFormula.Add("aw2", "*/ aw 1 2");
        shapeFormula.Add("x6", "+- r 0 aw2");
        shapeFormula.Add("y1", "+- t ah 0");
        shapeFormula.Add("swAng0", "at2 ah dx");
        shapeFormula.Add("swAng", "*/ swAng0 180 " + Math.PI.ToString());
        shapeFormula.Add("mswAng", "+- 0 0 swAng");
        shapeFormula.Add("iy", "+- t idy 0");
        shapeFormula.Add("ix", "+/ wR x3 2");
        shapeFormula.Add("q12", "*/ th 1 2");
        shapeFormula.Add("dang0", "at2 idy q12");
        shapeFormula.Add("dang2", "*/ dang0 180 " + Math.PI.ToString());
        shapeFormula.Add("swAng2", "+- dang2 0 swAng");
        shapeFormula.Add("mswAng2", "+- 0 0 swAng2");
        shapeFormula.Add("stAng3", "+- cd4 0 swAng");
        shapeFormula.Add("swAng3", "+- swAng dang2 0");
        shapeFormula.Add("stAng2", "+- cd4 0 dang2");
        break;
      case AutoShapeType.CurvedDownArrow:
        shapeFormula.Add("maxAdj2", "*/ 50000 w ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("aw", "*/ ss a2 100000");
        shapeFormula.Add("q1", "+/ th aw 4");
        shapeFormula.Add("wR", "+- wd2 0 q1");
        shapeFormula.Add("q7", "*/ wR 2 1");
        shapeFormula.Add("q8", "*/ q7 q7 1");
        shapeFormula.Add("q9", "*/ th th 1");
        shapeFormula.Add("q10", "+- q8 0 q9");
        shapeFormula.Add("q11", "sqrt q10");
        shapeFormula.Add("idy", "*/ q11 h q7");
        shapeFormula.Add("maxAdj3", "*/ 100000 idy ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("ah", "*/ ss adj3 100000");
        shapeFormula.Add("x3", "+- wR th 0");
        shapeFormula.Add("q2", "*/ h h 1");
        shapeFormula.Add("q3", "*/ ah ah 1");
        shapeFormula.Add("q4", "+- q2 0 q3");
        shapeFormula.Add("q5", "sqrt q4");
        shapeFormula.Add("dx", "*/ q5 wR h");
        shapeFormula.Add("x5", "+- wR dx 0");
        shapeFormula.Add("x7", "+- x3 dx 0");
        shapeFormula.Add("q6", "+- aw 0 th");
        shapeFormula.Add("dh", "*/ q6 1 2");
        shapeFormula.Add("x4", "+- x5 0 dh");
        shapeFormula.Add("x8", "+- x7 dh 0");
        shapeFormula.Add("aw2", "*/ aw 1 2");
        shapeFormula.Add("x6", "+- r 0 aw2");
        shapeFormula.Add("y1", "+- b 0 ah");
        shapeFormula.Add("swAng0", "at2 ah dx");
        shapeFormula.Add("swAng", "*/ swAng0 180 " + Math.PI.ToString());
        shapeFormula.Add("mswAng", "+- 0 0 swAng");
        shapeFormula.Add("iy", "+- b 0 idy");
        shapeFormula.Add("ix", "+/ wR x3 2");
        shapeFormula.Add("q12", "*/ th 1 2");
        shapeFormula.Add("dang0", "at2 idy q12");
        shapeFormula.Add("dang2", "*/ dang0 180 " + Math.PI.ToString());
        shapeFormula.Add("stAng", "+- 3cd4 swAng 0");
        shapeFormula.Add("stAng2", "+- 3cd4 0 dang2");
        shapeFormula.Add("swAng2", "+- dang2 0 cd4");
        shapeFormula.Add("swAng3", "+- cd4 dang2 0");
        break;
      case AutoShapeType.StripedRightArrow:
        shapeFormula.Add("maxAdj2", "*/ 84375 w ss");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("x4", "*/ ss 5 32");
        shapeFormula.Add("dx5", "*/ ss a2 100000");
        shapeFormula.Add("x5", "+- r 0 dx5");
        shapeFormula.Add("dy1", "*/ h a1 200000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc dy1 0");
        shapeFormula.Add("dx6", "*/ dy1 dx5 hd2");
        shapeFormula.Add("x6", "+- r 0 dx6");
        break;
      case AutoShapeType.NotchedRightArrow:
        shapeFormula.Add("maxAdj2", "*/ 100000 w ss");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("dx2", "*/ ss a2 100000");
        shapeFormula.Add("x2", "+- r 0 dx2");
        shapeFormula.Add("dy1", "*/ h a1 200000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc dy1 0");
        shapeFormula.Add("x1", "*/ dy1 dx2 hd2");
        shapeFormula.Add("x3", "+- r 0 x1");
        break;
      case AutoShapeType.Pentagon:
        shapeFormula.Add("maxAdj", "*/ 100000 w ss");
        shapeFormula.Add("a", "pin 0 adj maxAdj");
        shapeFormula.Add("dx1", "*/ ss a 100000");
        shapeFormula.Add("x1", "+- r 0 dx1");
        shapeFormula.Add("ir", "+/ x1 r 2");
        shapeFormula.Add("x2", "*/ x1 1 2");
        break;
      case AutoShapeType.Chevron:
        shapeFormula.Add("maxAdj", "*/ 100000 w ss");
        shapeFormula.Add("a", "pin 0 adj maxAdj");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "+- r 0 x1");
        shapeFormula.Add("x3", "*/ x2 1 2");
        shapeFormula.Add("dx", "+- x2 0 x1");
        shapeFormula.Add("il", "?: dx x1 l");
        shapeFormula.Add("ir", "?: dx x2 r");
        break;
      case AutoShapeType.RightArrowCallout:
        shapeFormula.Add("maxAdj2", "*/ 50000 h ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("maxAdj3", "*/ 100000 w ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("q2", "*/ a3 ss w");
        shapeFormula.Add("maxAdj4", "+- 100000 0 q2");
        shapeFormula.Add("a4", "pin 0 adj4 maxAdj4");
        shapeFormula.Add("dy1", "*/ ss a2 100000");
        shapeFormula.Add("dy2", "*/ ss a1 200000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc dy2 0");
        shapeFormula.Add("y4", "+- vc dy1 0");
        shapeFormula.Add("dx3", "*/ ss a3 100000");
        shapeFormula.Add("x3", "+- r 0 dx3");
        shapeFormula.Add("x2", "*/ w a4 100000");
        shapeFormula.Add("x1", "*/ x2 1 2");
        break;
      case AutoShapeType.LeftArrowCallout:
        shapeFormula.Add("maxAdj2", "*/ 50000 h ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("maxAdj3", "*/ 100000 w ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("q2", "*/ a3 ss w");
        shapeFormula.Add("maxAdj4", "+- 100000 0 q2");
        shapeFormula.Add("a4", "pin 0 adj4 maxAdj4");
        shapeFormula.Add("dy1", "*/ ss a2 100000");
        shapeFormula.Add("dy2", "*/ ss a1 200000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc dy2 0");
        shapeFormula.Add("y4", "+- vc dy1 0");
        shapeFormula.Add("x1", "*/ ss a3 100000");
        shapeFormula.Add("dx2", "*/ w a4 100000");
        shapeFormula.Add("x2", "+- r 0 dx2");
        shapeFormula.Add("x3", "+/ x2 r 2");
        break;
      case AutoShapeType.UpArrowCallout:
        shapeFormula.Add("maxAdj2", "*/ 50000 w ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("maxAdj3", "*/ 100000 h ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("q2", "*/ a3 ss h");
        shapeFormula.Add("maxAdj4", "+- 100000 0 q2");
        shapeFormula.Add("a4", "pin 0 adj4 maxAdj4");
        shapeFormula.Add("dx1", "*/ ss a2 100000");
        shapeFormula.Add("dx2", "*/ ss a1 200000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("y1", "*/ ss a3 100000");
        shapeFormula.Add("dy2", "*/ h a4 100000");
        shapeFormula.Add("y2", "+- b 0 dy2");
        shapeFormula.Add("y3", "+/ y2 b 2");
        break;
      case AutoShapeType.DownArrowCallout:
        shapeFormula.Add("maxAdj2", "*/ 50000 w ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("maxAdj3", "*/ 100000 h ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("q2", "*/ a3 ss h");
        shapeFormula.Add("maxAdj4", "+- 100000 0 q2");
        shapeFormula.Add("a4", "pin 0 adj4 maxAdj4");
        shapeFormula.Add("dx1", "*/ ss a2 100000");
        shapeFormula.Add("dx2", "*/ ss a1 200000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("dy3", "*/ ss a3 100000");
        shapeFormula.Add("y3", "+- b 0 dy3");
        shapeFormula.Add("y2", "*/ h a4 100000");
        shapeFormula.Add("y1", "*/ y2 1 2");
        break;
      case AutoShapeType.LeftRightArrowCallout:
        shapeFormula.Add("maxAdj2", "*/ 50000 h ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("maxAdj3", "*/ 50000 w ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("q2", "*/ a3 ss wd2");
        shapeFormula.Add("maxAdj4", "+- 100000 0 q2");
        shapeFormula.Add("a4", "pin 0 adj4 maxAdj4");
        shapeFormula.Add("dy1", "*/ ss a2 100000");
        shapeFormula.Add("dy2", "*/ ss a1 200000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc dy2 0");
        shapeFormula.Add("y4", "+- vc dy1 0");
        shapeFormula.Add("x1", "*/ ss a3 100000");
        shapeFormula.Add("x4", "+- r 0 x1");
        shapeFormula.Add("dx2", "*/ w a4 200000");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        break;
      case AutoShapeType.UpDownArrowCallout:
        shapeFormula.Add("maxAdj2", "*/ 50000 w ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("maxAdj3", "*/ 50000 h ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("q2", "*/ a3 ss hd2");
        shapeFormula.Add("maxAdj4", "+- 100000 0 q2");
        shapeFormula.Add("a4", "pin 0 adj4 maxAdj4");
        shapeFormula.Add("dx1", "*/ ss a2 100000");
        shapeFormula.Add("dx2", "*/ ss a1 200000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("y1", "*/ ss a3 100000");
        shapeFormula.Add("y4", "+- b 0 y1");
        shapeFormula.Add("dy2", "*/ h a4 200000");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc dy2 0");
        break;
      case AutoShapeType.QuadArrowCallout:
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("maxAdj3", "+- 50000 0 a2");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("q2", "*/ a3 2 1");
        shapeFormula.Add("maxAdj4", "+- 100000 0 q2");
        shapeFormula.Add("a4", "pin a1 adj4 maxAdj4");
        shapeFormula.Add("dx2", "*/ ss a2 100000");
        shapeFormula.Add("dx3", "*/ ss a1 200000");
        shapeFormula.Add("ah", "*/ ss a3 100000");
        shapeFormula.Add("dx1", "*/ w a4 200000");
        shapeFormula.Add("dy1", "*/ h a4 200000");
        shapeFormula.Add("x8", "+- r 0 ah");
        shapeFormula.Add("x2", "+- hc 0 dx1");
        shapeFormula.Add("x7", "+- hc dx1 0");
        shapeFormula.Add("x3", "+- hc 0 dx2");
        shapeFormula.Add("x6", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc 0 dx3");
        shapeFormula.Add("x5", "+- hc dx3 0");
        shapeFormula.Add("y8", "+- b 0 ah");
        shapeFormula.Add("y2", "+- vc 0 dy1");
        shapeFormula.Add("y7", "+- vc dy1 0");
        shapeFormula.Add("y3", "+- vc 0 dx2");
        shapeFormula.Add("y6", "+- vc dx2 0");
        shapeFormula.Add("y4", "+- vc 0 dx3");
        shapeFormula.Add("y5", "+- vc dx3 0");
        break;
      case AutoShapeType.CircularArrow:
        shapeFormula.Add("a5", "pin 0 adj5 25000");
        shapeFormula.Add("maxAdj1", "*/ a5 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("enAng", "pin 1 adj3 360");
        shapeFormula.Add("stAng", "pin 0 adj4 360");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("thh", "*/ ss a5 100000");
        shapeFormula.Add("th2", "*/ th 1 2");
        shapeFormula.Add("rw1", "+- wd2 th2 thh");
        shapeFormula.Add("rh1", "+- hd2 th2 thh");
        shapeFormula.Add("rw2", "+- rw1 0 th");
        shapeFormula.Add("rh2", "+- rh1 0 th");
        shapeFormula.Add("rw3", "+- rw2 th2 0");
        shapeFormula.Add("rh3", "+- rh2 th2 0");
        shapeFormula.Add("wtH", "sin rw3 enAng");
        shapeFormula.Add("htH", "cos rh3 enAng");
        shapeFormula.Add("dxH", "cat2 rw3 htH wtH");
        shapeFormula.Add("dyH", "sat2 rh3 htH wtH");
        shapeFormula.Add("xH", "+- hc dxH 0");
        shapeFormula.Add("yH", "+- vc dyH 0");
        shapeFormula.Add("rI", "min rw2 rh2");
        shapeFormula.Add("u1", "*/ dxH dxH 1");
        shapeFormula.Add("u2", "*/ dyH dyH 1");
        shapeFormula.Add("u3", "*/ rI rI 1");
        shapeFormula.Add("u4", "+- u1 0 u3");
        shapeFormula.Add("u5", "+- u2 0 u3");
        shapeFormula.Add("u6", "*/ u4 u5 u1");
        shapeFormula.Add("u7", "*/ u6 1 u2");
        shapeFormula.Add("u8", "+- 1 0 u7");
        shapeFormula.Add("u9", "sqrt u8");
        shapeFormula.Add("u10", "*/ u4 1 dxH");
        shapeFormula.Add("u11", "*/ u10 1 dyH");
        shapeFormula.Add("u12", "+/ 1 u9 u11");
        shapeFormula.Add("u0", "at2 1 u12");
        shapeFormula.Add("u13", "*/ u0 180 " + Math.PI.ToString());
        shapeFormula.Add("u14", "+- u13 360 0");
        shapeFormula.Add("u15", "?: u13 u13 u14");
        shapeFormula.Add("u16", "+- u15 0 enAng");
        shapeFormula.Add("u17", "+- u16 360 0");
        shapeFormula.Add("u18", "?: u16 u16 u17");
        shapeFormula.Add("u19", "+- u18 0 cd2");
        shapeFormula.Add("u20", "+- u18 0 360");
        shapeFormula.Add("u21", "?: u19 u20 u18");
        shapeFormula.Add("maxAng", "abs u21");
        shapeFormula.Add("aAng", "pin 0 adj2 maxAng");
        shapeFormula.Add("ptAng", "+- enAng aAng 0");
        shapeFormula.Add("wtA", "sin rw3 ptAng");
        shapeFormula.Add("htA", "cos rh3 ptAng");
        shapeFormula.Add("dxA", "cat2 rw3 htA wtA");
        shapeFormula.Add("dyA", "sat2 rh3 htA wtA");
        shapeFormula.Add("xA", "+- hc dxA 0");
        shapeFormula.Add("yA", "+- vc dyA 0");
        shapeFormula.Add("wtE", "sin rw1 stAng");
        shapeFormula.Add("htE", "cos rh1 stAng");
        shapeFormula.Add("dxE", "cat2 rw1 htE wtE");
        shapeFormula.Add("dyE", "sat2 rh1 htE wtE");
        shapeFormula.Add("xE", "+- hc dxE 0");
        shapeFormula.Add("yE", "+- vc dyE 0");
        shapeFormula.Add("dxG", "cos thh ptAng");
        shapeFormula.Add("dyG", "sin thh ptAng");
        shapeFormula.Add("xG", "+- xH dxG 0");
        shapeFormula.Add("yG", "+- yH dyG 0");
        shapeFormula.Add("dxB", "cos thh ptAng");
        shapeFormula.Add("dyB", "sin thh ptAng");
        shapeFormula.Add("xB", "+- xH 0 dxB 0");
        shapeFormula.Add("yB", "+- yH 0 dyB 0");
        shapeFormula.Add("sx1", "+- xB 0 hc");
        shapeFormula.Add("sy1", "+- yB 0 vc");
        shapeFormula.Add("sx2", "+- xG 0 hc");
        shapeFormula.Add("sy2", "+- yG 0 vc");
        shapeFormula.Add("rO", "min rw1 rh1");
        shapeFormula.Add("x1O", "*/ sx1 rO rw1");
        shapeFormula.Add("y1O", "*/ sy1 rO rh1");
        shapeFormula.Add("x2O", "*/ sx2 rO rw1");
        shapeFormula.Add("y2O", "*/ sy2 rO rh1");
        shapeFormula.Add("dxO", "+- x2O 0 x1O");
        shapeFormula.Add("dyO", "+- y2O 0 y1O");
        shapeFormula.Add("dO", "mod dxO dyO 0");
        shapeFormula.Add("q1", "*/ x1O y2O 1");
        shapeFormula.Add("q2", "*/ x2O y1O 1");
        shapeFormula.Add("DO", "+- q1 0 q2");
        shapeFormula.Add("q3", "*/ rO rO 1");
        shapeFormula.Add("q4", "*/ dO dO 1");
        shapeFormula.Add("q5", "*/ q3 q4 1");
        shapeFormula.Add("q6", "*/ DO DO 1");
        shapeFormula.Add("q7", "+- q5 0 q6");
        shapeFormula.Add("q8", "max q7 0");
        shapeFormula.Add("sdelO", "sqrt q8");
        shapeFormula.Add("ndyO", "*/ dyO -1 1");
        shapeFormula.Add("sdyO", "?: ndyO -1 1");
        shapeFormula.Add("q9", "*/ sdyO dxO 1");
        shapeFormula.Add("q10", "*/ q9 sdelO 1");
        shapeFormula.Add("q11", "*/ DO dyO 1");
        shapeFormula.Add("dxF1", "+/ q11 q10 q4");
        shapeFormula.Add("q12", "+- q11 0 q10");
        shapeFormula.Add("dxF2", "*/ q12 1 q4");
        shapeFormula.Add("adyO", "abs dyO");
        shapeFormula.Add("q13", "*/ adyO sdelO 1");
        shapeFormula.Add("q14", "*/ DO dxO -1");
        shapeFormula.Add("dyF1", "+/ q14 q13 q4");
        shapeFormula.Add("q15", "+- q14 0 q13");
        shapeFormula.Add("dyF2", "*/ q15 1 q4");
        shapeFormula.Add("q16", "+- x2O 0 dxF1");
        shapeFormula.Add("q17", "+- x2O 0 dxF2");
        shapeFormula.Add("q18", "+- y2O 0 dyF1");
        shapeFormula.Add("q19", "+- y2O 0 dyF2");
        shapeFormula.Add("q20", "mod q16 q18 0");
        shapeFormula.Add("q21", "mod q17 q19 0");
        shapeFormula.Add("q22", "+- q21 0 q20");
        shapeFormula.Add("dxF", "?: q22 dxF1 dxF2");
        shapeFormula.Add("dyF", "?: q22 dyF1 dyF2");
        shapeFormula.Add("sdxF", "*/ dxF rw1 rO");
        shapeFormula.Add("sdyF", "*/ dyF rh1 rO");
        shapeFormula.Add("xF", "+- hc sdxF 0");
        shapeFormula.Add("yF", "+- vc sdyF 0");
        shapeFormula.Add("x1I", "*/ sx1 rI rw2");
        shapeFormula.Add("y1I", "*/ sy1 rI rh2");
        shapeFormula.Add("x2I", "*/ sx2 rI rw2");
        shapeFormula.Add("y2I", "*/ sy2 rI rh2");
        shapeFormula.Add("dxI1", "+- x2I 0 x1I");
        shapeFormula.Add("dyI1", "+- y2I 0 y1I");
        shapeFormula.Add("dI", "mod dxI1 dyI1 0");
        shapeFormula.Add("v1", "*/ x1I y2I 1");
        shapeFormula.Add("v2", "*/ x2I y1I 1");
        shapeFormula.Add("DI", "+- v1 0 v2");
        shapeFormula.Add("v3", "*/ rI rI 1");
        shapeFormula.Add("v4", "*/ dI dI 1");
        shapeFormula.Add("v5", "*/ v3 v4 1");
        shapeFormula.Add("v6", "*/ DI DI 1");
        shapeFormula.Add("v7", "+- v5 0 v6");
        shapeFormula.Add("v8", "max v7 0");
        shapeFormula.Add("sdelI", "sqrt v8");
        shapeFormula.Add("v9", "*/ sdyO dxI1 1");
        shapeFormula.Add("v10", "*/ v9 sdelI 1");
        shapeFormula.Add("v11", "*/ DI dyI1 1");
        shapeFormula.Add("dxC1", "+/ v11 v10 v4");
        shapeFormula.Add("v12", "+- v11 0 v10");
        shapeFormula.Add("dxC2", "*/ v12 1 v4");
        shapeFormula.Add("adyI", "abs dyI1");
        shapeFormula.Add("v13", "*/ adyI sdelI 1");
        shapeFormula.Add("v14", "*/ DI dxI1 -1");
        shapeFormula.Add("dyC1", "+/ v14 v13 v4");
        shapeFormula.Add("v15", "+- v14 0 v13");
        shapeFormula.Add("dyC2", "*/ v15 1 v4");
        shapeFormula.Add("v16", "+- x1I 0 dxC1");
        shapeFormula.Add("v17", "+- x1I 0 dxC2");
        shapeFormula.Add("v18", "+- y1I 0 dyC1");
        shapeFormula.Add("v19", "+- y1I 0 dyC2");
        shapeFormula.Add("v20", "mod v16 v18 0");
        shapeFormula.Add("v21", "mod v17 v19 0");
        shapeFormula.Add("v22", "+- v21 0 v20");
        shapeFormula.Add("dxC", "?: v22 dxC1 dxC2");
        shapeFormula.Add("dyC", "?: v22 dyC1 dyC2");
        shapeFormula.Add("sdxC", "*/ dxC rw2 rI");
        shapeFormula.Add("sdyC", "*/ dyC rh2 rI");
        shapeFormula.Add("xC", "+- hc sdxC 0");
        shapeFormula.Add("yC", "+- vc sdyC 0");
        shapeFormula.Add("ist00", "at2 sdxC sdyC");
        shapeFormula.Add("ist0", "*/ ist00 180 " + Math.PI.ToString());
        shapeFormula.Add("ist1", "+- ist0 360 0");
        shapeFormula.Add("istAng", "?: ist0 ist0 ist1");
        shapeFormula.Add("isw1", "+- stAng 0 istAng");
        shapeFormula.Add("isw2", "+- isw1 0 360");
        shapeFormula.Add("iswAng", "?: isw1 isw2 isw1");
        shapeFormula.Add("p1", "+- xF 0 xC");
        shapeFormula.Add("p2", "+- yF 0 yC");
        shapeFormula.Add("p3", "mod p1 p2 0");
        shapeFormula.Add("p4", "*/ p3 1 2");
        shapeFormula.Add("p5", "+- p4 0 thh");
        shapeFormula.Add("xGp", "?: p5 xF xG");
        shapeFormula.Add("yGp", "?: p5 yF yG");
        shapeFormula.Add("xBp", "?: p5 xC xB");
        shapeFormula.Add("yBp", "?: p5 yC yB");
        shapeFormula.Add("en00", "at2 sdxF sdyF");
        shapeFormula.Add("en0", "*/ en00 180 " + Math.PI.ToString());
        shapeFormula.Add("en1", "+- en0 360 0");
        shapeFormula.Add("en2", "?: en0 en0 en1");
        shapeFormula.Add("sw0", "+- en2 0 stAng");
        shapeFormula.Add("sw1", "+- sw0 360 0");
        shapeFormula.Add("swAng", "?: sw0 sw0 sw1");
        shapeFormula.Add("wtI", "sin rw3 stAng");
        shapeFormula.Add("htI", "cos rh3 stAng");
        shapeFormula.Add("dxI", "cat2 rw3 htI wtI");
        shapeFormula.Add("dyI", "sat2 rh3 htI wtI");
        shapeFormula.Add("xI", "+- hc dxI 0");
        shapeFormula.Add("yI", "+- vc dyI 0");
        shapeFormula.Add("aI", "+- stAng 0 cd4");
        shapeFormula.Add("aA", "+- ptAng cd4 0");
        shapeFormula.Add("aB", "+- ptAng cd2 0");
        shapeFormula.Add("idx", "cos rw1 45");
        shapeFormula.Add("idy", "sin rh1 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.FlowChartAlternateProcess:
        shapeFormula.Add("x2", "+- r 0 ssd6");
        shapeFormula.Add("y2", "+- b 0 ssd6");
        shapeFormula.Add("il", "*/ ssd6 29289 100000");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 il");
        break;
      case AutoShapeType.FlowChartDecision:
        shapeFormula.Add("ir", "*/ w 3 4");
        shapeFormula.Add("ib", "*/ h 3 4");
        break;
      case AutoShapeType.FlowChartData:
        shapeFormula.Add("x3", "*/ w 2 5");
        shapeFormula.Add("x4", "*/ w 3 5");
        shapeFormula.Add("x5", "*/ w 4 5");
        shapeFormula.Add("x6", "*/ w 9 10");
        break;
      case AutoShapeType.FlowChartPredefinedProcess:
        shapeFormula.Add("x2", "*/ w 7 8");
        break;
      case AutoShapeType.FlowChartDocument:
        shapeFormula.Add("y1", "*/ h 17322 21600");
        shapeFormula.Add("y2", "*/ h 20172 21600");
        break;
      case AutoShapeType.FlowChartMultiDocument:
        shapeFormula.Add("y2", "*/ h 3675 21600");
        shapeFormula.Add("y8", "*/ h 20782 21600");
        shapeFormula.Add("x3", "*/ w 9298 21600");
        shapeFormula.Add("x4", "*/ w 12286 21600");
        shapeFormula.Add("x5", "*/ w 18595 21600");
        break;
      case AutoShapeType.FlowChartTerminator:
        shapeFormula.Add("il", "*/ w 1018 21600");
        shapeFormula.Add("ir", "*/ w 20582 21600");
        shapeFormula.Add("it", "*/ h 3163 21600");
        shapeFormula.Add("ib", "*/ h 18437 21600");
        break;
      case AutoShapeType.FlowChartPreparation:
        shapeFormula.Add("x2", "*/ w 4 5");
        break;
      case AutoShapeType.FlowChartManualOperation:
        shapeFormula.Add("x3", "*/ w 4 5");
        shapeFormula.Add("x4", "*/ w 9 10");
        break;
      case AutoShapeType.FlowChartConnector:
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.FlowChartOffPageConnector:
        shapeFormula.Add("y1", "*/ h 4 5");
        break;
      case AutoShapeType.FlowChartPunchedTape:
        shapeFormula.Add("y2", "*/ h 9 10");
        shapeFormula.Add("ib", "*/ h 4 5");
        break;
      case AutoShapeType.FlowChartSummingJunction:
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.FlowChartOr:
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.FlowChartCollate:
        shapeFormula.Add("ir", "*/ w 3 4");
        shapeFormula.Add("ib", "*/ h 3 4");
        break;
      case AutoShapeType.FlowChartSort:
        shapeFormula.Add("ir", "*/ w 3 4");
        shapeFormula.Add("ib", "*/ h 3 4");
        break;
      case AutoShapeType.FlowChartExtract:
        shapeFormula.Add("x2", "*/ w 3 4");
        break;
      case AutoShapeType.FlowChartMerge:
        shapeFormula.Add("x2", "*/ w 3 4");
        break;
      case AutoShapeType.FlowChartStoredData:
        shapeFormula.Add("x2", "*/ w 5 6");
        break;
      case AutoShapeType.FlowChartDelay:
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.FlowChartSequentialAccessStorage:
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        shapeFormula.Add("ang", "at2 w h");
        shapeFormula.Add("ang1", "*/ ang 180 " + Math.PI.ToString());
        break;
      case AutoShapeType.FlowChartMagneticDisk:
        shapeFormula.Add("y3", "*/ h 5 6");
        break;
      case AutoShapeType.FlowChartDirectAccessStorage:
        shapeFormula.Add("x2", "*/ w 2 3");
        break;
      case AutoShapeType.FlowChartDisplay:
        shapeFormula.Add("x2", "*/ w 5 6");
        break;
      case AutoShapeType.Explosion1:
        shapeFormula.Add("x5", "*/ w 4627 21600");
        shapeFormula.Add("x12", "*/ w 8485 21600");
        shapeFormula.Add("x21", "*/ w 16702 21600");
        shapeFormula.Add("x24", "*/ w 14522 21600");
        shapeFormula.Add("y3", "*/ h 6320 21600");
        shapeFormula.Add("y6", "*/ h 8615 21600");
        shapeFormula.Add("y9", "*/ h 13937 21600");
        shapeFormula.Add("y18", "*/ h 13290 21600");
        break;
      case AutoShapeType.Explosion2:
        shapeFormula.Add("x2", "*/ w 9722 21600");
        shapeFormula.Add("x5", "*/ w 5372 21600");
        shapeFormula.Add("x16", "*/ w 11612 21600");
        shapeFormula.Add("x19", "*/ w 14640 21600");
        shapeFormula.Add("y2", "*/ h 1887 21600");
        shapeFormula.Add("y3", "*/ h 6382 21600");
        shapeFormula.Add("y8", "*/ h 12877 21600");
        shapeFormula.Add("y14", "*/ h 19712 21600");
        shapeFormula.Add("y16", "*/ h 18842 21600");
        shapeFormula.Add("y17", "*/ h 15935 21600");
        shapeFormula.Add("y24", "*/ h 6645 21600");
        break;
      case AutoShapeType.Star4Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("iwd2", "*/ wd2 a 50000");
        shapeFormula.Add("ihd2", "*/ hd2 a 50000");
        shapeFormula.Add("sdx", "cos iwd2 45");
        shapeFormula.Add("sdy", "sin ihd2 45");
        shapeFormula.Add("sx1", "+- hc 0 sdx");
        shapeFormula.Add("sx2", "+- hc sdx 0");
        shapeFormula.Add("sy1", "+- vc 0 sdy");
        shapeFormula.Add("sy2", "+- vc sdy 0");
        shapeFormula.Add("yAdj", "+- vc 0 ihd2");
        break;
      case AutoShapeType.Star5Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("swd2", "*/ wd2 hf 100000");
        shapeFormula.Add("shd2", "*/ hd2 vf 100000");
        shapeFormula.Add("svc", "*/ vc  vf 100000");
        shapeFormula.Add("dx1", "cos swd2 18");
        shapeFormula.Add("dx2", "cos swd2 306");
        shapeFormula.Add("dy1", "sin shd2 18");
        shapeFormula.Add("dy2", "sin shd2 306");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- svc 0 dy1");
        shapeFormula.Add("y2", "+- svc 0 dy2");
        shapeFormula.Add("iwd2", "*/ swd2 a 50000");
        shapeFormula.Add("ihd2", "*/ shd2 a 50000");
        shapeFormula.Add("sdx1", "cos iwd2 342");
        shapeFormula.Add("sdx2", "cos iwd2 54");
        shapeFormula.Add("sdy1", "sin ihd2 54");
        shapeFormula.Add("sdy2", "sin ihd2 342");
        shapeFormula.Add("sx1", "+- hc 0 sdx1");
        shapeFormula.Add("sx2", "+- hc 0 sdx2");
        shapeFormula.Add("sx3", "+- hc sdx2 0");
        shapeFormula.Add("sx4", "+- hc sdx1 0");
        shapeFormula.Add("sy1", "+- svc 0 sdy1");
        shapeFormula.Add("sy2", "+- svc 0 sdy2");
        shapeFormula.Add("sy3", "+- svc ihd2 0");
        shapeFormula.Add("yAdj", "+- svc 0 ihd2");
        break;
      case AutoShapeType.Star8Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dx1", "cos wd2 45");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc dx1 0");
        shapeFormula.Add("dy1", "sin hd2 45");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc dy1 0");
        shapeFormula.Add("iwd2", "*/ wd2 a 50000");
        shapeFormula.Add("ihd2", "*/ hd2 a 50000");
        shapeFormula.Add("sdx1", "*/ iwd2 92388 100000");
        shapeFormula.Add("sdx2", "*/ iwd2 38268 100000");
        shapeFormula.Add("sdy1", "*/ ihd2 92388 100000");
        shapeFormula.Add("sdy2", "*/ ihd2 38268 100000");
        shapeFormula.Add("sx1", "+- hc 0 sdx1");
        shapeFormula.Add("sx2", "+- hc 0 sdx2");
        shapeFormula.Add("sx3", "+- hc sdx2 0");
        shapeFormula.Add("sx4", "+- hc sdx1 0");
        shapeFormula.Add("sy1", "+- vc 0 sdy1");
        shapeFormula.Add("sy2", "+- vc 0 sdy2");
        shapeFormula.Add("sy3", "+- vc sdy2 0");
        shapeFormula.Add("sy4", "+- vc sdy1 0");
        shapeFormula.Add("yAdj", "+- vc 0 ihd2");
        break;
      case AutoShapeType.Star16Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dx1", "*/ wd2 92388 100000");
        shapeFormula.Add("dx2", "*/ wd2 70711 100000");
        shapeFormula.Add("dx3", "*/ wd2 38268 100000");
        shapeFormula.Add("dy1", "*/ hd2 92388 100000");
        shapeFormula.Add("dy2", "*/ hd2 70711 100000");
        shapeFormula.Add("dy3", "*/ hd2 38268 100000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc 0 dx3");
        shapeFormula.Add("x4", "+- hc dx3 0");
        shapeFormula.Add("x5", "+- hc dx2 0");
        shapeFormula.Add("x6", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc 0 dy3");
        shapeFormula.Add("y4", "+- vc dy3 0");
        shapeFormula.Add("y5", "+- vc dy2 0");
        shapeFormula.Add("y6", "+- vc dy1 0");
        shapeFormula.Add("iwd2", "*/ wd2 a 50000");
        shapeFormula.Add("ihd2", "*/ hd2 a 50000");
        shapeFormula.Add("sdx1", "*/ iwd2 98079 100000");
        shapeFormula.Add("sdx2", "*/ iwd2 83147 100000");
        shapeFormula.Add("sdx3", "*/ iwd2 55557 100000");
        shapeFormula.Add("sdx4", "*/ iwd2 19509 100000");
        shapeFormula.Add("sdy1", "*/ ihd2 98079 100000");
        shapeFormula.Add("sdy2", "*/ ihd2 83147 100000");
        shapeFormula.Add("sdy3", "*/ ihd2 55557 100000");
        shapeFormula.Add("sdy4", "*/ ihd2 19509 100000");
        shapeFormula.Add("sx1", "+- hc 0 sdx1");
        shapeFormula.Add("sx2", "+- hc 0 sdx2");
        shapeFormula.Add("sx3", "+- hc 0 sdx3");
        shapeFormula.Add("sx4", "+- hc 0 sdx4");
        shapeFormula.Add("sx5", "+- hc sdx4 0");
        shapeFormula.Add("sx6", "+- hc sdx3 0");
        shapeFormula.Add("sx7", "+- hc sdx2 0");
        shapeFormula.Add("sx8", "+- hc sdx1 0");
        shapeFormula.Add("sy1", "+- vc 0 sdy1");
        shapeFormula.Add("sy2", "+- vc 0 sdy2");
        shapeFormula.Add("sy3", "+- vc 0 sdy3");
        shapeFormula.Add("sy4", "+- vc 0 sdy4");
        shapeFormula.Add("sy5", "+- vc sdy4 0");
        shapeFormula.Add("sy6", "+- vc sdy3 0");
        shapeFormula.Add("sy7", "+- vc sdy2 0");
        shapeFormula.Add("sy8", "+- vc sdy1 0");
        shapeFormula.Add("idx", "cos iwd2 45");
        shapeFormula.Add("idy", "sin ihd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("ib", "+- vc idy 0");
        shapeFormula.Add("yAdj", "+- vc 0 ihd2");
        break;
      case AutoShapeType.Star24Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dx1", "cos wd2 15");
        shapeFormula.Add("dx2", "cos wd2 30");
        shapeFormula.Add("dx3", "cos wd2 45");
        shapeFormula.Add("dx4", "val wd4");
        shapeFormula.Add("dx5", "cos wd2 75");
        shapeFormula.Add("dy1", "sin hd2 75");
        shapeFormula.Add("dy2", "sin hd2 60");
        shapeFormula.Add("dy3", "sin hd2 45");
        shapeFormula.Add("dy4", "val hd4");
        shapeFormula.Add("dy5", "sin hd2 15");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc 0 dx3");
        shapeFormula.Add("x4", "+- hc 0 dx4");
        shapeFormula.Add("x5", "+- hc 0 dx5");
        shapeFormula.Add("x6", "+- hc dx5 0");
        shapeFormula.Add("x7", "+- hc dx4 0");
        shapeFormula.Add("x8", "+- hc dx3 0");
        shapeFormula.Add("x9", "+- hc dx2 0");
        shapeFormula.Add("x10", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc 0 dy3");
        shapeFormula.Add("y4", "+- vc 0 dy4");
        shapeFormula.Add("y5", "+- vc 0 dy5");
        shapeFormula.Add("y6", "+- vc dy5 0");
        shapeFormula.Add("y7", "+- vc dy4 0");
        shapeFormula.Add("y8", "+- vc dy3 0");
        shapeFormula.Add("y9", "+- vc dy2 0");
        shapeFormula.Add("y10", "+- vc dy1 0");
        shapeFormula.Add("iwd2", "*/ wd2 a 50000");
        shapeFormula.Add("ihd2", "*/ hd2 a 50000");
        shapeFormula.Add("sdx1", "*/ iwd2 99144 100000");
        shapeFormula.Add("sdx2", "*/ iwd2 92388 100000");
        shapeFormula.Add("sdx3", "*/ iwd2 79335 100000");
        shapeFormula.Add("sdx4", "*/ iwd2 60876 100000");
        shapeFormula.Add("sdx5", "*/ iwd2 38268 100000");
        shapeFormula.Add("sdx6", "*/ iwd2 13053 100000");
        shapeFormula.Add("sdy1", "*/ ihd2 99144 100000");
        shapeFormula.Add("sdy2", "*/ ihd2 92388 100000");
        shapeFormula.Add("sdy3", "*/ ihd2 79335 100000");
        shapeFormula.Add("sdy4", "*/ ihd2 60876 100000");
        shapeFormula.Add("sdy5", "*/ ihd2 38268 100000");
        shapeFormula.Add("sdy6", "*/ ihd2 13053 100000");
        shapeFormula.Add("sx1", "+- hc 0 sdx1");
        shapeFormula.Add("sx2", "+- hc 0 sdx2");
        shapeFormula.Add("sx3", "+- hc 0 sdx3");
        shapeFormula.Add("sx4", "+- hc 0 sdx4");
        shapeFormula.Add("sx5", "+- hc 0 sdx5");
        shapeFormula.Add("sx6", "+- hc 0 sdx6");
        shapeFormula.Add("sx7", "+- hc sdx6 0");
        shapeFormula.Add("sx8", "+- hc sdx5 0");
        shapeFormula.Add("sx9", "+- hc sdx4 0");
        shapeFormula.Add("sx10", "+- hc sdx3 0");
        shapeFormula.Add("sx11", "+- hc sdx2 0");
        shapeFormula.Add("sx12", "+- hc sdx1 0");
        shapeFormula.Add("sy1", "+- vc 0 sdy1");
        shapeFormula.Add("sy2", "+- vc 0 sdy2");
        shapeFormula.Add("sy3", "+- vc 0 sdy3");
        shapeFormula.Add("sy4", "+- vc 0 sdy4");
        shapeFormula.Add("sy5", "+- vc 0 sdy5");
        shapeFormula.Add("sy6", "+- vc 0 sdy6");
        shapeFormula.Add("sy7", "+- vc sdy6 0");
        shapeFormula.Add("sy8", "+- vc sdy5 0");
        shapeFormula.Add("sy9", "+- vc sdy4 0");
        shapeFormula.Add("sy10", "+- vc sdy3 0");
        shapeFormula.Add("sy11", "+- vc sdy2 0");
        shapeFormula.Add("sy12", "+- vc sdy1 0");
        shapeFormula.Add("idx", "cos iwd2 45");
        shapeFormula.Add("idy", "sin ihd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("ib", "+- vc idy 0");
        shapeFormula.Add("yAdj", "+- vc 0 ihd2");
        break;
      case AutoShapeType.Star32Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dx1", "*/ wd2 98079 100000");
        shapeFormula.Add("dx2", "*/ wd2 92388 100000");
        shapeFormula.Add("dx3", "*/ wd2 83147 100000");
        shapeFormula.Add("dx4", "cos wd2 45");
        shapeFormula.Add("dx5", "*/ wd2 55557 100000");
        shapeFormula.Add("dx6", "*/ wd2 38268 100000");
        shapeFormula.Add("dx7", "*/ wd2 19509 100000");
        shapeFormula.Add("dy1", "*/ hd2 98079 100000");
        shapeFormula.Add("dy2", "*/ hd2 92388 100000");
        shapeFormula.Add("dy3", "*/ hd2 83147 100000");
        shapeFormula.Add("dy4", "sin hd2 45");
        shapeFormula.Add("dy5", "*/ hd2 55557 100000");
        shapeFormula.Add("dy6", "*/ hd2 38268 100000");
        shapeFormula.Add("dy7", "*/ hd2 19509 100000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc 0 dx3");
        shapeFormula.Add("x4", "+- hc 0 dx4");
        shapeFormula.Add("x5", "+- hc 0 dx5");
        shapeFormula.Add("x6", "+- hc 0 dx6");
        shapeFormula.Add("x7", "+- hc 0 dx7");
        shapeFormula.Add("x8", "+- hc dx7 0");
        shapeFormula.Add("x9", "+- hc dx6 0");
        shapeFormula.Add("x10", "+- hc dx5 0");
        shapeFormula.Add("x11", "+- hc dx4 0");
        shapeFormula.Add("x12", "+- hc dx3 0");
        shapeFormula.Add("x13", "+- hc dx2 0");
        shapeFormula.Add("x14", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc 0 dy3");
        shapeFormula.Add("y4", "+- vc 0 dy4");
        shapeFormula.Add("y5", "+- vc 0 dy5");
        shapeFormula.Add("y6", "+- vc 0 dy6");
        shapeFormula.Add("y7", "+- vc 0 dy7");
        shapeFormula.Add("y8", "+- vc dy7 0");
        shapeFormula.Add("y9", "+- vc dy6 0");
        shapeFormula.Add("y10", "+- vc dy5 0");
        shapeFormula.Add("y11", "+- vc dy4 0");
        shapeFormula.Add("y12", "+- vc dy3 0");
        shapeFormula.Add("y13", "+- vc dy2 0");
        shapeFormula.Add("y14", "+- vc dy1 0");
        shapeFormula.Add("iwd2", "*/ wd2 a 50000");
        shapeFormula.Add("ihd2", "*/ hd2 a 50000");
        shapeFormula.Add("sdx1", "*/ iwd2 99518 100000");
        shapeFormula.Add("sdx2", "*/ iwd2 95694 100000");
        shapeFormula.Add("sdx3", "*/ iwd2 88192 100000");
        shapeFormula.Add("sdx4", "*/ iwd2 77301 100000");
        shapeFormula.Add("sdx5", "*/ iwd2 63439 100000");
        shapeFormula.Add("sdx6", "*/ iwd2 47140 100000");
        shapeFormula.Add("sdx7", "*/ iwd2 29028 100000");
        shapeFormula.Add("sdx8", "*/ iwd2 9802 100000");
        shapeFormula.Add("sdy1", "*/ ihd2 99518 100000");
        shapeFormula.Add("sdy2", "*/ ihd2 95694 100000");
        shapeFormula.Add("sdy3", "*/ ihd2 88192 100000");
        shapeFormula.Add("sdy4", "*/ ihd2 77301 100000");
        shapeFormula.Add("sdy5", "*/ ihd2 63439 100000");
        shapeFormula.Add("sdy6", "*/ ihd2 47140 100000");
        shapeFormula.Add("sdy7", "*/ ihd2 29028 100000");
        shapeFormula.Add("sdy8", "*/ ihd2 9802 100000");
        shapeFormula.Add("sx1", "+- hc 0 sdx1");
        shapeFormula.Add("sx2", "+- hc 0 sdx2");
        shapeFormula.Add("sx3", "+- hc 0 sdx3");
        shapeFormula.Add("sx4", "+- hc 0 sdx4");
        shapeFormula.Add("sx5", "+- hc 0 sdx5");
        shapeFormula.Add("sx6", "+- hc 0 sdx6");
        shapeFormula.Add("sx7", "+- hc 0 sdx7");
        shapeFormula.Add("sx8", "+- hc 0 sdx8");
        shapeFormula.Add("sx9", "+- hc sdx8 0");
        shapeFormula.Add("sx10", "+- hc sdx7 0");
        shapeFormula.Add("sx11", "+- hc sdx6 0");
        shapeFormula.Add("sx12", "+- hc sdx5 0");
        shapeFormula.Add("sx13", "+- hc sdx4 0");
        shapeFormula.Add("sx14", "+- hc sdx3 0");
        shapeFormula.Add("sx15", "+- hc sdx2 0");
        shapeFormula.Add("sx16", "+- hc sdx1 0");
        shapeFormula.Add("sy1", "+- vc 0 sdy1");
        shapeFormula.Add("sy2", "+- vc 0 sdy2");
        shapeFormula.Add("sy3", "+- vc 0 sdy3");
        shapeFormula.Add("sy4", "+- vc 0 sdy4");
        shapeFormula.Add("sy5", "+- vc 0 sdy5");
        shapeFormula.Add("sy6", "+- vc 0 sdy6");
        shapeFormula.Add("sy7", "+- vc 0 sdy7");
        shapeFormula.Add("sy8", "+- vc 0 sdy8");
        shapeFormula.Add("sy9", "+- vc sdy8 0");
        shapeFormula.Add("sy10", "+- vc sdy7 0");
        shapeFormula.Add("sy11", "+- vc sdy6 0");
        shapeFormula.Add("sy12", "+- vc sdy5 0");
        shapeFormula.Add("sy13", "+- vc sdy4 0");
        shapeFormula.Add("sy14", "+- vc sdy3 0");
        shapeFormula.Add("sy15", "+- vc sdy2 0");
        shapeFormula.Add("sy16", "+- vc sdy1 0");
        shapeFormula.Add("idx", "cos iwd2 45");
        shapeFormula.Add("idy", "sin ihd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("ib", "+- vc idy 0");
        shapeFormula.Add("yAdj", "+- vc 0 ihd2");
        break;
      case AutoShapeType.UpRibbon:
        shapeFormula.Add("a1", "pin 0 adj1 33333");
        shapeFormula.Add("a2", "pin 25000 adj2 75000");
        shapeFormula.Add("x10", "+- r 0 wd8");
        shapeFormula.Add("dx2", "*/ w a2 200000");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x9", "+- hc dx2 0");
        shapeFormula.Add("x3", "+- x2 wd32 0");
        shapeFormula.Add("x8", "+- x9 0 wd32");
        shapeFormula.Add("x5", "+- x2 wd8 0");
        shapeFormula.Add("x6", "+- x9 0 wd8");
        shapeFormula.Add("x4", "+- x5 0 wd32");
        shapeFormula.Add("x7", "+- x6 wd32 0");
        shapeFormula.Add("dy1", "*/ h a1 200000");
        shapeFormula.Add("y1", "+- b 0 dy1");
        shapeFormula.Add("dy2", "*/ h a1 100000");
        shapeFormula.Add("y2", "+- b 0 dy2");
        shapeFormula.Add("y4", "+- t dy2 0");
        shapeFormula.Add("y3", "+/ y4 b 2");
        shapeFormula.Add("hR", "*/ h a1 400000");
        shapeFormula.Add("y6", "+- b 0 hR");
        shapeFormula.Add("y7", "+- y1 0 hR");
        break;
      case AutoShapeType.DownRibbon:
        shapeFormula.Add("a1", "pin 0 adj1 33333");
        shapeFormula.Add("a2", "pin 25000 adj2 75000");
        shapeFormula.Add("x10", "+- r 0 wd8");
        shapeFormula.Add("dx2", "*/ w a2 200000");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x9", "+- hc dx2 0");
        shapeFormula.Add("x3", "+- x2 wd32 0");
        shapeFormula.Add("x8", "+- x9 0 wd32");
        shapeFormula.Add("x5", "+- x2 wd8 0");
        shapeFormula.Add("x6", "+- x9 0 wd8");
        shapeFormula.Add("x4", "+- x5 0 wd32");
        shapeFormula.Add("x7", "+- x6 wd32 0");
        shapeFormula.Add("y1", "*/ h a1 200000");
        shapeFormula.Add("y2", "*/ h a1 100000");
        shapeFormula.Add("y4", "+- b 0 y2");
        shapeFormula.Add("y3", "*/ y4 1 2");
        shapeFormula.Add("hR", "*/ h a1 400000");
        shapeFormula.Add("y5", "+- b 0 hR");
        shapeFormula.Add("y6", "+- y2 0 hR");
        break;
      case AutoShapeType.CurvedUpRibbon:
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 25000 adj2 75000");
        shapeFormula.Add("q10", "+- 100000 0 a1");
        shapeFormula.Add("q11", "*/ q10 1 2");
        shapeFormula.Add("q12", "+- a1 0 q11");
        shapeFormula.Add("minAdj3", "max 0 q12");
        shapeFormula.Add("a3", "pin minAdj3 adj3 a1");
        shapeFormula.Add("dx2", "*/ w a2 200000");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- x2 wd8 0");
        shapeFormula.Add("x4", "+- r 0 x3");
        shapeFormula.Add("x5", "+- r 0 x2");
        shapeFormula.Add("x6", "+- r 0 wd8");
        shapeFormula.Add("dy1", "*/ h a3 100000");
        shapeFormula.Add("f1", "*/ 4 dy1 w");
        shapeFormula.Add("q111", "*/ x3 x3 w");
        shapeFormula.Add("q2", "+- x3 0 q111");
        shapeFormula.Add("u1", "*/ f1 q2 1");
        shapeFormula.Add("y1", "+- b 0 u1");
        shapeFormula.Add("cx1", "*/ x3 1 2");
        shapeFormula.Add("cu1", "*/ f1 cx1 1");
        shapeFormula.Add("cy1", "+- b 0 cu1");
        shapeFormula.Add("cx2", "+- r 0 cx1");
        shapeFormula.Add("q1", "*/ h a1 100000");
        shapeFormula.Add("dy3", "+- q1 0 dy1");
        shapeFormula.Add("q3", "*/ x2 x2 w");
        shapeFormula.Add("q4", "+- x2 0 q3");
        shapeFormula.Add("q5", "*/ f1 q4 1");
        shapeFormula.Add("u3", "+- q5 dy3 0");
        shapeFormula.Add("y3", "+- b 0 u3");
        shapeFormula.Add("q6", "+- dy1 dy3 u3");
        shapeFormula.Add("q7", "+- q6 dy1 0");
        shapeFormula.Add("cu3", "+- q7 dy3 0");
        shapeFormula.Add("cy3", "+- b 0 cu3");
        shapeFormula.Add("rh", "+- b 0 q1");
        shapeFormula.Add("q8", "*/ dy1 14 16");
        shapeFormula.Add("u2", "+/ q8 rh 2");
        shapeFormula.Add("y2", "+- b 0 u2");
        shapeFormula.Add("u5", "+- q5 rh 0");
        shapeFormula.Add("y5", "+- b 0 u5");
        shapeFormula.Add("u6", "+- u3 rh 0");
        shapeFormula.Add("y6", "+- b 0 u6");
        shapeFormula.Add("cx4", "*/ x2 1 2");
        shapeFormula.Add("q9", "*/ f1 cx4 1");
        shapeFormula.Add("cu4", "+- q9 rh 0");
        shapeFormula.Add("cy4", "+- b 0 cu4");
        shapeFormula.Add("cx5", "+- r 0 cx4");
        shapeFormula.Add("cu6", "+- cu3 rh 0");
        shapeFormula.Add("cy6", "+- b 0 cu6");
        shapeFormula.Add("u7", "+- u1 dy3 0");
        shapeFormula.Add("y7", "+- b 0 u7");
        shapeFormula.Add("cu7", "+- q1 q1 u7");
        shapeFormula.Add("cy7", "+- b 0 cu7");
        break;
      case AutoShapeType.CurvedDownRibbon:
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 25000 adj2 75000");
        shapeFormula.Add("q10", "+- 100000 0 a1");
        shapeFormula.Add("q11", "*/ q10 1 2");
        shapeFormula.Add("q12", "+- a1 0 q11");
        shapeFormula.Add("minAdj3", "max 0 q12");
        shapeFormula.Add("a3", "pin minAdj3 adj3 a1");
        shapeFormula.Add("dx2", "*/ w a2 200000");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- x2 wd8 0");
        shapeFormula.Add("x4", "+- r 0 x3");
        shapeFormula.Add("x5", "+- r 0 x2");
        shapeFormula.Add("x6", "+- r 0 wd8");
        shapeFormula.Add("dy1", "*/ h a3 100000");
        shapeFormula.Add("f1", "*/ 4 dy1 w");
        shapeFormula.Add("q111", "*/ x3 x3 w");
        shapeFormula.Add("q2", "+- x3 0 q111");
        shapeFormula.Add("y1", "*/ f1 q2 1");
        shapeFormula.Add("cx1", "*/ x3 1 2");
        shapeFormula.Add("cy1", "*/ f1 cx1 1");
        shapeFormula.Add("cx2", "+- r 0 cx1");
        shapeFormula.Add("q1", "*/ h a1 100000");
        shapeFormula.Add("dy3", "+- q1 0 dy1");
        shapeFormula.Add("q3", "*/ x2 x2 w");
        shapeFormula.Add("q4", "+- x2 0 q3");
        shapeFormula.Add("q5", "*/ f1 q4 1");
        shapeFormula.Add("y3", "+- q5 dy3 0");
        shapeFormula.Add("q6", "+- dy1 dy3 y3");
        shapeFormula.Add("q7", "+- q6 dy1 0");
        shapeFormula.Add("cy3", "+- q7 dy3 0");
        shapeFormula.Add("rh", "+- b 0 q1");
        shapeFormula.Add("q8", "*/ dy1 14 16");
        shapeFormula.Add("y2", "+/ q8 rh 2");
        shapeFormula.Add("y5", "+- q5 rh 0");
        shapeFormula.Add("y6", "+- y3 rh 0");
        shapeFormula.Add("cx4", "*/ x2 1 2");
        shapeFormula.Add("q9", "*/ f1 cx4 1");
        shapeFormula.Add("cy4", "+- q9 rh 0");
        shapeFormula.Add("cx5", "+- r 0 cx4");
        shapeFormula.Add("cy6", "+- cy3 rh 0");
        shapeFormula.Add("y7", "+- y1 dy3 0");
        shapeFormula.Add("cy7", "+- q1 q1 y7");
        shapeFormula.Add("y8", "+- b 0 dy1");
        break;
      case AutoShapeType.VerticalScroll:
        shapeFormula.Add("a", "pin 0 adj 25000");
        shapeFormula.Add("ch", "*/ ss a 100000");
        shapeFormula.Add("ch2", "*/ ch 1 2");
        shapeFormula.Add("ch4", "*/ ch 1 4");
        shapeFormula.Add("x3", "+- ch ch2 0");
        shapeFormula.Add("x4", "+- ch ch 0");
        shapeFormula.Add("x6", "+- r 0 ch");
        shapeFormula.Add("x7", "+- r 0 ch2");
        shapeFormula.Add("x5", "+- x6 0 ch2");
        shapeFormula.Add("y3", "+- b 0 ch");
        shapeFormula.Add("y4", "+- b 0 ch2");
        break;
      case AutoShapeType.HorizontalScroll:
        shapeFormula.Add("a", "pin 0 adj 25000");
        shapeFormula.Add("ch", "*/ ss a 100000");
        shapeFormula.Add("ch2", "*/ ch 1 2");
        shapeFormula.Add("ch4", "*/ ch 1 4");
        shapeFormula.Add("y3", "+- ch ch2 0");
        shapeFormula.Add("y4", "+- ch ch 0");
        shapeFormula.Add("y6", "+- b 0 ch");
        shapeFormula.Add("y7", "+- b 0 ch2");
        shapeFormula.Add("y5", "+- y6 0 ch2");
        shapeFormula.Add("x3", "+- r 0 ch");
        shapeFormula.Add("x4", "+- r 0 ch2");
        break;
      case AutoShapeType.Wave:
        shapeFormula.Add("a1", "pin 0 adj1 20000");
        shapeFormula.Add("a2", "pin -10000 adj2 10000");
        shapeFormula.Add("y1", "*/ h a1 100000");
        shapeFormula.Add("dy2", "*/ y1 10 3");
        shapeFormula.Add("y2", "+- y1 0 dy2");
        shapeFormula.Add("y3", "+- y1 dy2 0");
        shapeFormula.Add("y4", "+- b 0 y1");
        shapeFormula.Add("y5", "+- y4 0 dy2");
        shapeFormula.Add("y6", "+- y4 dy2 0");
        shapeFormula.Add("dx1", "*/ w a2 100000");
        shapeFormula.Add("of2", "*/ w a2 50000");
        shapeFormula.Add("x1", "abs dx1");
        shapeFormula.Add("dx2", "?: of2 0 of2");
        shapeFormula.Add("x2", "+- l 0 dx2");
        shapeFormula.Add("dx5", "?: of2 of2 0");
        shapeFormula.Add("x5", "+- r 0 dx5");
        shapeFormula.Add("dx3", "+/ dx2 x5 3");
        shapeFormula.Add("x3", "+- x2 dx3 0");
        shapeFormula.Add("x4", "+/ x3 x5 2");
        shapeFormula.Add("x6", "+- l dx5 0");
        shapeFormula.Add("x10", "+- r dx2 0");
        shapeFormula.Add("x7", "+- x6 dx3 0");
        shapeFormula.Add("x8", "+/ x7 x10 2");
        shapeFormula.Add("x9", "+- r 0 x1");
        shapeFormula.Add("xAdj", "+- hc dx1 0");
        shapeFormula.Add("xAdj2", "+- hc 0 dx1");
        shapeFormula.Add("il", "max x2 x6");
        shapeFormula.Add("ir", "min x5 x10");
        shapeFormula.Add("it", "*/ h a1 50000");
        shapeFormula.Add("ib", "+- b 0 it");
        break;
      case AutoShapeType.DoubleWave:
        shapeFormula.Add("a1", "pin 0 adj1 12500");
        shapeFormula.Add("a2", "pin -10000 adj2 10000");
        shapeFormula.Add("y1", "*/ h a1 100000");
        shapeFormula.Add("dy2", "*/ y1 10 3");
        shapeFormula.Add("y2", "+- y1 0 dy2");
        shapeFormula.Add("y3", "+- y1 dy2 0");
        shapeFormula.Add("y4", "+- b 0 y1");
        shapeFormula.Add("y5", "+- y4 0 dy2");
        shapeFormula.Add("y6", "+- y4 dy2 0");
        shapeFormula.Add("dx1", "*/ w a2 100000");
        shapeFormula.Add("of2", "*/ w a2 50000");
        shapeFormula.Add("x1", "abs dx1");
        shapeFormula.Add("dx2", "?: of2 0 of2");
        shapeFormula.Add("x2", "+- l 0 dx2");
        shapeFormula.Add("dx8", "?: of2 of2 0");
        shapeFormula.Add("x8", "+- r 0 dx8");
        shapeFormula.Add("dx3", "+/ dx2 x8 6");
        shapeFormula.Add("x3", "+- x2 dx3 0");
        shapeFormula.Add("dx4", "+/ dx2 x8 3");
        shapeFormula.Add("x4", "+- x2 dx4 0");
        shapeFormula.Add("x5", "+/ x2 x8 2");
        shapeFormula.Add("x6", "+- x5 dx3 0");
        shapeFormula.Add("x7", "+/ x6 x8 2");
        shapeFormula.Add("x9", "+- l dx8 0");
        shapeFormula.Add("x15", "+- r dx2 0");
        shapeFormula.Add("x10", "+- x9 dx3 0");
        shapeFormula.Add("x11", "+- x9 dx4 0");
        shapeFormula.Add("x12", "+/ x9 x15 2");
        shapeFormula.Add("x13", "+- x12 dx3 0");
        shapeFormula.Add("x14", "+/ x13 x15 2");
        shapeFormula.Add("x16", "+- r 0 x1");
        shapeFormula.Add("xAdj", "+- hc dx1 0");
        shapeFormula.Add("il", "max x2 x9");
        shapeFormula.Add("ir", "min x8 x15");
        shapeFormula.Add("it", "*/ h a1 50000");
        shapeFormula.Add("ib", "+- b 0 it");
        break;
      case AutoShapeType.RectangularCallout:
        shapeFormula.Add("dxPos", "*/ w adj1 100000");
        shapeFormula.Add("dyPos", "*/ h adj2 100000");
        shapeFormula.Add("xPos", "+- hc dxPos 0");
        shapeFormula.Add("yPos", "+- vc dyPos 0");
        shapeFormula.Add("dx", "+- xPos 0 hc");
        shapeFormula.Add("dy", "+- yPos 0 vc");
        shapeFormula.Add("dq", "*/ dxPos h w");
        shapeFormula.Add("ady", "abs dyPos");
        shapeFormula.Add("adq", "abs dq");
        shapeFormula.Add("dz", "+- ady 0 adq");
        shapeFormula.Add("xg1", "?: dxPos 7 2");
        shapeFormula.Add("xg2", "?: dxPos 10 5");
        shapeFormula.Add("x1", "*/ w xg1 12");
        shapeFormula.Add("x2", "*/ w xg2 12");
        shapeFormula.Add("yg1", "?: dyPos 7 2");
        shapeFormula.Add("yg2", "?: dyPos 10 5");
        shapeFormula.Add("y1", "*/ h yg1 12");
        shapeFormula.Add("y2", "*/ h yg2 12");
        shapeFormula.Add("t1", "?: dxPos l xPos");
        shapeFormula.Add("xl", "?: dz l t1");
        shapeFormula.Add("t2", "?: dyPos x1 xPos");
        shapeFormula.Add("xt", "?: dz t2 x1");
        shapeFormula.Add("t3", "?: dxPos xPos r");
        shapeFormula.Add("xr", "?: dz r t3");
        shapeFormula.Add("t4", "?: dyPos xPos x1");
        shapeFormula.Add("xb", "?: dz t4 x1");
        shapeFormula.Add("t5", "?: dxPos y1 yPos");
        shapeFormula.Add("yl", "?: dz y1 t5");
        shapeFormula.Add("t6", "?: dyPos t yPos");
        shapeFormula.Add("yt", "?: dz t6 t");
        shapeFormula.Add("t7", "?: dxPos yPos y1");
        shapeFormula.Add("yr", "?: dz y1 t7");
        shapeFormula.Add("t8", "?: dyPos yPos b");
        shapeFormula.Add("yb", "?: dz t8 b");
        break;
      case AutoShapeType.RoundedRectangularCallout:
        shapeFormula.Add("dxPos", "*/ w adj1 100000");
        shapeFormula.Add("dyPos", "*/ h adj2 100000");
        shapeFormula.Add("xPos", "+- hc dxPos 0");
        shapeFormula.Add("yPos", "+- vc dyPos 0");
        shapeFormula.Add("dq", "*/ dxPos h w");
        shapeFormula.Add("ady", "abs dyPos");
        shapeFormula.Add("adq", "abs dq");
        shapeFormula.Add("dz", "+- ady 0 adq");
        shapeFormula.Add("xg1", "?: dxPos 7 2");
        shapeFormula.Add("xg2", "?: dxPos 10 5");
        shapeFormula.Add("x1", "*/ w xg1 12");
        shapeFormula.Add("x2", "*/ w xg2 12");
        shapeFormula.Add("yg1", "?: dyPos 7 2");
        shapeFormula.Add("yg2", "?: dyPos 10 5");
        shapeFormula.Add("y1", "*/ h yg1 12");
        shapeFormula.Add("y2", "*/ h yg2 12");
        shapeFormula.Add("t1", "?: dxPos l xPos");
        shapeFormula.Add("xl", "?: dz l t1");
        shapeFormula.Add("t2", "?: dyPos x1 xPos");
        shapeFormula.Add("xt", "?: dz t2 x1");
        shapeFormula.Add("t3", "?: dxPos xPos r");
        shapeFormula.Add("xr", "?: dz r t3");
        shapeFormula.Add("t4", "?: dyPos xPos x1");
        shapeFormula.Add("xb", "?: dz t4 x1");
        shapeFormula.Add("t5", "?: dxPos y1 yPos");
        shapeFormula.Add("yl", "?: dz y1 t5");
        shapeFormula.Add("t6", "?: dyPos t yPos");
        shapeFormula.Add("yt", "?: dz t6 t");
        shapeFormula.Add("t7", "?: dxPos yPos y1");
        shapeFormula.Add("yr", "?: dz y1 t7");
        shapeFormula.Add("t8", "?: dyPos yPos b");
        shapeFormula.Add("yb", "?: dz t8 b");
        shapeFormula.Add("u1", "*/ ss adj3 100000");
        shapeFormula.Add("u2", "+- r 0 u1");
        shapeFormula.Add("v2", "+- b 0 u1");
        shapeFormula.Add("il", "*/ u1 29289 100000");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 il");
        break;
      case AutoShapeType.OvalCallout:
        shapeFormula.Add("dxPos", "*/ w adj1 100000");
        shapeFormula.Add("dyPos", "*/ h adj2 100000");
        shapeFormula.Add("xPos", "+- hc dxPos 0");
        shapeFormula.Add("yPos", "+- vc dyPos 0");
        shapeFormula.Add("sdx", "*/ dxPos h 1");
        shapeFormula.Add("sdy", "*/ dyPos w 1");
        shapeFormula.Add("pang1", "at2 sdx sdy");
        shapeFormula.Add("pang", "*/ pang1 180 " + Math.PI.ToString());
        shapeFormula.Add("stAng", "+- pang 11 0");
        shapeFormula.Add("enAng", "+- pang 0 11");
        shapeFormula.Add("dx1", "cos wd2 stAng");
        shapeFormula.Add("dy1", "sin hd2 stAng");
        shapeFormula.Add("x1", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc dy1 0");
        shapeFormula.Add("dx2", "cos wd2 enAng");
        shapeFormula.Add("dy2", "sin hd2 enAng");
        shapeFormula.Add("x2", "+- hc dx2 0");
        shapeFormula.Add("y2", "+- vc dy2 0");
        shapeFormula.Add("stAng2", "at2 dx1 dy1");
        shapeFormula.Add("stAng1", "*/ stAng2 180 " + Math.PI.ToString());
        shapeFormula.Add("enAng2", "at2 dx2 dy2");
        shapeFormula.Add("enAng1", "*/ enAng2 180 " + Math.PI.ToString());
        shapeFormula.Add("swAng1", "+- enAng1 0 stAng1");
        shapeFormula.Add("swAng2", "+- swAng1 360 0");
        shapeFormula.Add("swAng", "?: swAng1 swAng1 swAng2");
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.CloudCallout:
        shapeFormula.Add("dxPos", "*/ w adj1 100000");
        shapeFormula.Add("dyPos", "*/ h adj2 100000");
        shapeFormula.Add("xPos", "+- hc dxPos 0");
        shapeFormula.Add("yPos", "+- vc dyPos 0");
        shapeFormula.Add("ht", "cat2 hd2 dxPos dyPos");
        shapeFormula.Add("wt", "sat2 wd2 dxPos dyPos");
        shapeFormula.Add("g2", "cat2 wd2 ht wt");
        shapeFormula.Add("g3", "sat2 hd2 ht wt");
        shapeFormula.Add("g4", "+- hc g2 0");
        shapeFormula.Add("g5", "+- vc g3 0");
        shapeFormula.Add("g6", "+- g4 0 xPos");
        shapeFormula.Add("g7", "+- g5 0 yPos");
        shapeFormula.Add("g8", "mod g6 g7 0");
        shapeFormula.Add("g9", "*/ ss 6600 21600");
        shapeFormula.Add("g10", "+- g8 0 g9");
        shapeFormula.Add("g11", "*/ g10 1 3");
        shapeFormula.Add("g12", "*/ ss 1800 21600");
        shapeFormula.Add("g13", "+- g11 g12 0");
        shapeFormula.Add("g14", "*/ g13 g6 g8");
        shapeFormula.Add("g15", "*/ g13 g7 g8");
        shapeFormula.Add("g16", "+- g14 xPos 0");
        shapeFormula.Add("g17", "+- g15 yPos 0");
        shapeFormula.Add("g18", "*/ ss 4800 21600");
        shapeFormula.Add("g19", "*/ g11 2 1");
        shapeFormula.Add("g20", "+- g18 g19 0");
        shapeFormula.Add("g21", "*/ g20 g6 g8");
        shapeFormula.Add("g22", "*/ g20 g7 g8");
        shapeFormula.Add("g23", "+- g21 xPos 0");
        shapeFormula.Add("g24", "+- g22 yPos 0");
        shapeFormula.Add("g25", "*/ ss 1200 21600");
        shapeFormula.Add("g26", "*/ ss 600 21600");
        shapeFormula.Add("x23", "+- xPos g26 0");
        shapeFormula.Add("x24", "+- g16 g25 0");
        shapeFormula.Add("x25", "+- g23 g12 0");
        shapeFormula.Add("il", "*/ w 2977 21600");
        shapeFormula.Add("it", "*/ h 3262 21600");
        shapeFormula.Add("ir", "*/ w 17087 21600");
        shapeFormula.Add("ib", "*/ h 17337 21600");
        shapeFormula.Add("g27", "*/ w 67 21600");
        shapeFormula.Add("g28", "*/ h 21577 21600");
        shapeFormula.Add("g29", "*/ w 21582 21600");
        shapeFormula.Add("g30", "*/ h 1235 21600");
        shapeFormula.Add("pang", "at2 dxPos dyPos");
        break;
      case AutoShapeType.LineCallout1:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        break;
      case AutoShapeType.LineCallout2:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        shapeFormula.Add("y3", "*/ h adj5 100000");
        shapeFormula.Add("x3", "*/ w adj6 100000");
        break;
      case AutoShapeType.LineCallout3:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        shapeFormula.Add("y3", "*/ h adj5 100000");
        shapeFormula.Add("x3", "*/ w adj6 100000");
        shapeFormula.Add("y4", "*/ h adj7 100000");
        shapeFormula.Add("x4", "*/ w adj8 100000");
        break;
      case AutoShapeType.LineCallout1NoBorder:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        break;
      case AutoShapeType.LineCallout1AccentBar:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        break;
      case AutoShapeType.LineCallout2AccentBar:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        shapeFormula.Add("y3", "*/ h adj5 100000");
        shapeFormula.Add("x3", "*/ w adj6 100000");
        break;
      case AutoShapeType.LineCallout3AccentBar:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        shapeFormula.Add("y3", "*/ h adj5 100000");
        shapeFormula.Add("x3", "*/ w adj6 100000");
        shapeFormula.Add("y4", "*/ h adj7 100000");
        shapeFormula.Add("x4", "*/ w adj8 100000");
        break;
      case AutoShapeType.LineCallout2NoBorder:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        shapeFormula.Add("y3", "*/ h adj5 100000");
        shapeFormula.Add("x3", "*/ w adj6 100000");
        break;
      case AutoShapeType.LineCallout3NoBorder:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        shapeFormula.Add("y3", "*/ h adj5 100000");
        shapeFormula.Add("x3", "*/ w adj6 100000");
        shapeFormula.Add("y4", "*/ h adj7 100000");
        shapeFormula.Add("x4", "*/ w adj8 100000");
        break;
      case AutoShapeType.LineCallout1BorderAndAccentBar:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        break;
      case AutoShapeType.LineCallout2BorderAndAccentBar:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        shapeFormula.Add("y3", "*/ h adj5 100000");
        shapeFormula.Add("x3", "*/ w adj6 100000");
        break;
      case AutoShapeType.LineCallout3BorderAndAccentBar:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        shapeFormula.Add("y3", "*/ h adj5 100000");
        shapeFormula.Add("x3", "*/ w adj6 100000");
        shapeFormula.Add("y4", "*/ h adj7 100000");
        shapeFormula.Add("x4", "*/ w adj8 100000");
        break;
      case AutoShapeType.DiagonalStripe:
        shapeFormula.Add("a", "pin 0 adj 100000");
        shapeFormula.Add("x2", "*/ w a 100000");
        shapeFormula.Add("x1", "*/ x2 1 2");
        shapeFormula.Add("x3", "+/ x2 r 2");
        shapeFormula.Add("y2", "*/ h a 100000");
        shapeFormula.Add("y1", "*/ y2 1 2");
        shapeFormula.Add("y3", "+/ y2 b 2");
        break;
      case AutoShapeType.Pie:
        shapeFormula.Add("stAng", "pin 0 adj1 360");
        shapeFormula.Add("enAng", "pin 0 adj2 360");
        shapeFormula.Add("sw1", "+- enAng 0 stAng");
        shapeFormula.Add("sw2", "+- sw1 360 0");
        shapeFormula.Add("swAng", "?: sw1 sw1 sw2");
        shapeFormula.Add("wt1", "sin wd2 stAng");
        shapeFormula.Add("ht1", "cos hd2 stAng");
        shapeFormula.Add("dx1", "cat2 wd2 ht1 wt1");
        shapeFormula.Add("dy1", "sat2 hd2 ht1 wt1");
        shapeFormula.Add("x1", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc dy1 0");
        shapeFormula.Add("wt2", "sin wd2 enAng");
        shapeFormula.Add("ht2", "cos hd2 enAng");
        shapeFormula.Add("dx2", "cat2 wd2 ht2 wt2");
        shapeFormula.Add("dy2", "sat2 hd2 ht2 wt2");
        shapeFormula.Add("x2", "+- hc dx2 0");
        shapeFormula.Add("y2", "+- vc dy2 0");
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.Decagon:
        shapeFormula.Add("shd2", "*/ hd2 vf 100000");
        shapeFormula.Add("dx1", "cos wd2 36");
        shapeFormula.Add("dx2", "cos wd2 72");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("dy1", "sin shd2 72");
        shapeFormula.Add("dy2", "sin shd2 36");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc dy2 0");
        shapeFormula.Add("y4", "+- vc dy1 0");
        break;
      case AutoShapeType.Heptagon:
        shapeFormula.Add("swd2", "*/ wd2 hf 100000");
        shapeFormula.Add("shd2", "*/ hd2 vf 100000");
        shapeFormula.Add("svc", "*/ vc  vf 100000");
        shapeFormula.Add("dx1", "*/ swd2 97493 100000");
        shapeFormula.Add("dx2", "*/ swd2 78183 100000");
        shapeFormula.Add("dx3", "*/ swd2 43388 100000");
        shapeFormula.Add("dy1", "*/ shd2 62349 100000");
        shapeFormula.Add("dy2", "*/ shd2 22252 100000");
        shapeFormula.Add("dy3", "*/ shd2 90097 100000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc 0 dx3");
        shapeFormula.Add("x4", "+- hc dx3 0");
        shapeFormula.Add("x5", "+- hc dx2 0");
        shapeFormula.Add("x6", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- svc 0 dy1");
        shapeFormula.Add("y2", "+- svc dy2 0");
        shapeFormula.Add("y3", "+- svc dy3 0");
        shapeFormula.Add("ib", "+- b 0 y1");
        break;
      case AutoShapeType.Dodecagon:
        shapeFormula.Add("x1", "*/ w 2894 21600");
        shapeFormula.Add("x2", "*/ w 7906 21600");
        shapeFormula.Add("x3", "*/ w 13694 21600");
        shapeFormula.Add("x4", "*/ w 18706 21600");
        shapeFormula.Add("y1", "*/ h 2894 21600");
        shapeFormula.Add("y2", "*/ h 7906 21600");
        shapeFormula.Add("y3", "*/ h 13694 21600");
        shapeFormula.Add("y4", "*/ h 18706 21600");
        break;
      case AutoShapeType.Star6Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("swd2", "*/ wd2 hf 100000");
        shapeFormula.Add("dx1", "cos swd2 30");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc dx1 0");
        shapeFormula.Add("y2", "+- vc hd4 0");
        shapeFormula.Add("iwd2", "*/ swd2 a 50000");
        shapeFormula.Add("ihd2", "*/ hd2 a 50000");
        shapeFormula.Add("sdx2", "*/ iwd2 1 2");
        shapeFormula.Add("sx1", "+- hc 0 iwd2");
        shapeFormula.Add("sx2", "+- hc 0 sdx2");
        shapeFormula.Add("sx3", "+- hc sdx2 0");
        shapeFormula.Add("sx4", "+- hc iwd2 0");
        shapeFormula.Add("sdy1", "sin ihd2 60");
        shapeFormula.Add("sy1", "+- vc 0 sdy1");
        shapeFormula.Add("sy2", "+- vc sdy1 0");
        shapeFormula.Add("yAdj", "+- vc 0 ihd2");
        break;
      case AutoShapeType.Star7Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("swd2", "*/ wd2 hf 100000");
        shapeFormula.Add("shd2", "*/ hd2 vf 100000");
        shapeFormula.Add("svc", "*/ vc  vf 100000");
        shapeFormula.Add("dx1", "*/ swd2 97493 100000");
        shapeFormula.Add("dx2", "*/ swd2 78183 100000");
        shapeFormula.Add("dx3", "*/ swd2 43388 100000");
        shapeFormula.Add("dy1", "*/ shd2 62349 100000");
        shapeFormula.Add("dy2", "*/ shd2 22252 100000");
        shapeFormula.Add("dy3", "*/ shd2 90097 100000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc 0 dx3");
        shapeFormula.Add("x4", "+- hc dx3 0");
        shapeFormula.Add("x5", "+- hc dx2 0");
        shapeFormula.Add("x6", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- svc 0 dy1");
        shapeFormula.Add("y2", "+- svc dy2 0");
        shapeFormula.Add("y3", "+- svc dy3 0");
        shapeFormula.Add("iwd2", "*/ swd2 a 50000");
        shapeFormula.Add("ihd2", "*/ shd2 a 50000");
        shapeFormula.Add("sdx1", "*/ iwd2 97493 100000");
        shapeFormula.Add("sdx2", "*/ iwd2 78183 100000");
        shapeFormula.Add("sdx3", "*/ iwd2 43388 100000");
        shapeFormula.Add("sx1", "+- hc 0 sdx1");
        shapeFormula.Add("sx2", "+- hc 0 sdx2");
        shapeFormula.Add("sx3", "+- hc 0 sdx3");
        shapeFormula.Add("sx4", "+- hc sdx3 0");
        shapeFormula.Add("sx5", "+- hc sdx2 0");
        shapeFormula.Add("sx6", "+- hc sdx1 0");
        shapeFormula.Add("sdy1", "*/ ihd2 90097 100000");
        shapeFormula.Add("sdy2", "*/ ihd2 22252 100000");
        shapeFormula.Add("sdy3", "*/ ihd2 62349 100000");
        shapeFormula.Add("sy1", "+- svc 0 sdy1");
        shapeFormula.Add("sy2", "+- svc 0 sdy2");
        shapeFormula.Add("sy3", "+- svc sdy3 0");
        shapeFormula.Add("sy4", "+- svc ihd2 0");
        shapeFormula.Add("yAdj", "+- svc 0 ihd2");
        break;
      case AutoShapeType.Star10Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("swd2", "*/ wd2 hf 100000");
        shapeFormula.Add("dx1", "*/ swd2 95106 100000");
        shapeFormula.Add("dx2", "*/ swd2 58779 100000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("dy1", "*/ hd2 80902 100000");
        shapeFormula.Add("dy2", "*/ hd2 30902 100000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc dy2 0");
        shapeFormula.Add("y4", "+- vc dy1 0");
        shapeFormula.Add("iwd2", "*/ swd2 a 50000");
        shapeFormula.Add("ihd2", "*/ hd2 a 50000");
        shapeFormula.Add("sdx1", "*/ iwd2 80902 100000");
        shapeFormula.Add("sdx2", "*/ iwd2 30902 100000");
        shapeFormula.Add("sdy1", "*/ ihd2 95106 100000");
        shapeFormula.Add("sdy2", "*/ ihd2 58779 100000");
        shapeFormula.Add("sx1", "+- hc 0 iwd2");
        shapeFormula.Add("sx2", "+- hc 0 sdx1");
        shapeFormula.Add("sx3", "+- hc 0 sdx2");
        shapeFormula.Add("sx4", "+- hc sdx2 0");
        shapeFormula.Add("sx5", "+- hc sdx1 0");
        shapeFormula.Add("sx6", "+- hc iwd2 0");
        shapeFormula.Add("sy1", "+- vc 0 sdy1");
        shapeFormula.Add("sy2", "+- vc 0 sdy2");
        shapeFormula.Add("sy3", "+- vc sdy2 0");
        shapeFormula.Add("sy4", "+- vc sdy1 0");
        shapeFormula.Add("yAdj", "+- vc 0 ihd2");
        break;
      case AutoShapeType.Star12Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dx1", "cos wd2 30");
        shapeFormula.Add("dy1", "sin hd2 60");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x3", "*/ w 3 4");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y3", "*/ h 3 4");
        shapeFormula.Add("y4", "+- vc dy1 0");
        shapeFormula.Add("iwd2", "*/ wd2 a 50000");
        shapeFormula.Add("ihd2", "*/ hd2 a 50000");
        shapeFormula.Add("sdx1", "cos iwd2 15");
        shapeFormula.Add("sdx2", "cos iwd2 45");
        shapeFormula.Add("sdx3", "cos iwd2 75");
        shapeFormula.Add("sdy1", "sin ihd2 75");
        shapeFormula.Add("sdy2", "sin ihd2 45");
        shapeFormula.Add("sdy3", "sin ihd2 15");
        shapeFormula.Add("sx1", "+- hc 0 sdx1");
        shapeFormula.Add("sx2", "+- hc 0 sdx2");
        shapeFormula.Add("sx3", "+- hc 0 sdx3");
        shapeFormula.Add("sx4", "+- hc sdx3 0");
        shapeFormula.Add("sx5", "+- hc sdx2 0");
        shapeFormula.Add("sx6", "+- hc sdx1 0");
        shapeFormula.Add("sy1", "+- vc 0 sdy1");
        shapeFormula.Add("sy2", "+- vc 0 sdy2");
        shapeFormula.Add("sy3", "+- vc 0 sdy3");
        shapeFormula.Add("sy4", "+- vc sdy3 0");
        shapeFormula.Add("sy5", "+- vc sdy2 0");
        shapeFormula.Add("sy6", "+- vc sdy1 0");
        shapeFormula.Add("yAdj", "+- vc 0 ihd2");
        break;
      case AutoShapeType.RoundSingleCornerRectangle:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dx1", "*/ ss a 100000");
        shapeFormula.Add("x1", "+- r 0 dx1");
        shapeFormula.Add("idx", "*/ dx1 29289 100000");
        shapeFormula.Add("ir", "+- r 0 idx");
        break;
      case AutoShapeType.RoundSameSideCornerRectangle:
        shapeFormula.Add("a1", "pin 0 adj1 50000");
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("tx1", "*/ ss a1 100000");
        shapeFormula.Add("tx2", "+- r 0 tx1");
        shapeFormula.Add("bx1", "*/ ss a2 100000");
        shapeFormula.Add("bx2", "+- r 0 bx1");
        shapeFormula.Add("by1", "+- b 0 bx1");
        shapeFormula.Add("d", "+- tx1 0 bx1");
        shapeFormula.Add("tdx", "*/ tx1 29289 100000");
        shapeFormula.Add("bdx", "*/ bx1 29289 100000");
        shapeFormula.Add("il", "?: d tdx bdx");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 bdx");
        break;
      case AutoShapeType.RoundDiagonalCornerRectangle:
        shapeFormula.Add("a1", "pin 0 adj1 50000");
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("x1", "*/ ss a1 100000");
        shapeFormula.Add("y1", "+- b 0 x1");
        shapeFormula.Add("a", "*/ ss a2 100000");
        shapeFormula.Add("x2", "+- r 0 a");
        shapeFormula.Add("y2", "+- b 0 a");
        shapeFormula.Add("dx1", "*/ x1 29289 100000");
        shapeFormula.Add("dx2", "*/ a 29289 100000");
        shapeFormula.Add("d", "+- dx1 0 dx2");
        shapeFormula.Add("dx", "?: d dx1 dx2");
        shapeFormula.Add("ir", "+- r 0 dx");
        shapeFormula.Add("ib", "+- b 0 dx");
        break;
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
        shapeFormula.Add("a1", "pin 0 adj1 50000");
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("x1", "*/ ss a1 100000");
        shapeFormula.Add("dx2", "*/ ss a2 100000");
        shapeFormula.Add("x2", "+- r 0 dx2");
        shapeFormula.Add("il", "*/ x1 29289 100000");
        shapeFormula.Add("ir", "+/ x2 r 2");
        break;
      case AutoShapeType.SnipSingleCornerRectangle:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dx1", "*/ ss a 100000");
        shapeFormula.Add("x1", "+- r 0 dx1");
        shapeFormula.Add("it", "*/ dx1 1 2");
        shapeFormula.Add("ir", "+/ x1 r 2");
        break;
      case AutoShapeType.SnipSameSideCornerRectangle:
        shapeFormula.Add("a1", "pin 0 adj1 50000");
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("tx1", "*/ ss a1 100000");
        shapeFormula.Add("tx2", "+- r 0 tx1");
        shapeFormula.Add("bx1", "*/ ss a2 100000");
        shapeFormula.Add("bx2", "+- r 0 bx1");
        shapeFormula.Add("by1", "+- b 0 bx1");
        shapeFormula.Add("d", "+- tx1 0 bx1");
        shapeFormula.Add("dx", "?: d tx1 bx1");
        shapeFormula.Add("il", "*/ dx 1 2");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("it", "*/ tx1 1 2");
        shapeFormula.Add("ib", "+/ by1 b 2");
        break;
      case AutoShapeType.SnipDiagonalCornerRectangle:
        shapeFormula.Add("a1", "pin 0 adj1 50000");
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("lx1", "*/ ss a1 100000");
        shapeFormula.Add("lx2", "+- r 0 lx1");
        shapeFormula.Add("ly1", "+- b 0 lx1");
        shapeFormula.Add("rx1", "*/ ss a2 100000");
        shapeFormula.Add("rx2", "+- r 0 rx1");
        shapeFormula.Add("ry1", "+- b 0 rx1");
        shapeFormula.Add("d", "+- lx1 0 rx1");
        shapeFormula.Add("dx", "?: d lx1 rx1");
        shapeFormula.Add("il", "*/ dx 1 2");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 il");
        break;
      case AutoShapeType.Frame:
        shapeFormula.Add("a1", "pin 0 adj1 50000");
        shapeFormula.Add("x1", "*/ ss a1 100000");
        shapeFormula.Add("x4", "+- r 0 x1");
        shapeFormula.Add("y4", "+- b 0 x1");
        break;
      case AutoShapeType.HalfFrame:
        shapeFormula.Add("maxAdj2", "*/ 100000 w ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("x1", "*/ ss a2 100000");
        shapeFormula.Add("g1", "*/ h x1 w");
        shapeFormula.Add("g2", "+- h 0 g1");
        shapeFormula.Add("maxAdj1", "*/ 100000 g2 ss");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("y1", "*/ ss a1 100000");
        shapeFormula.Add("dx2", "*/ y1 w h");
        shapeFormula.Add("x2", "+- r 0 dx2");
        shapeFormula.Add("dy2", "*/ x1 h w");
        shapeFormula.Add("y2", "+- b 0 dy2");
        shapeFormula.Add("cx1", "*/ x1 1 2");
        shapeFormula.Add("cy1", "+/ y2 b 2");
        shapeFormula.Add("cx2", "+/ x2 r 2");
        shapeFormula.Add("cy2", "*/ y1 1 2");
        break;
      case AutoShapeType.Teardrop:
        shapeFormula.Add("a", "pin 0 adj 200000");
        shapeFormula.Add("r2", "sqrt 2");
        shapeFormula.Add("tw", "*/ wd2 r2 1");
        shapeFormula.Add("th", "*/ hd2 r2 1");
        shapeFormula.Add("sw", "*/ tw a 100000");
        shapeFormula.Add("sh", "*/ th a 100000");
        shapeFormula.Add("dx1", "cos sw 45");
        shapeFormula.Add("dy1", "sin sh 45");
        shapeFormula.Add("x1", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("x2", "+/ hc x1 2");
        shapeFormula.Add("y2", "+/ vc y1 2");
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.Chord:
        shapeFormula.Add("stAng", "pin 0 adj1 360");
        shapeFormula.Add("enAng", "pin 0 adj2 360");
        shapeFormula.Add("sw1", "+- enAng 0 stAng");
        shapeFormula.Add("sw2", "+- sw1 360 0");
        shapeFormula.Add("swAng", "?: sw1 sw1 sw2");
        shapeFormula.Add("wt1", "sin wd2 stAng");
        shapeFormula.Add("ht1", "cos hd2 stAng");
        shapeFormula.Add("dx1", "cat2 wd2 ht1 wt1");
        shapeFormula.Add("dy1", "sat2 hd2 ht1 wt1");
        shapeFormula.Add("wt2", "sin wd2 enAng");
        shapeFormula.Add("ht2", "cos hd2 enAng");
        shapeFormula.Add("dx2", "cat2 wd2 ht2 wt2");
        shapeFormula.Add("dy2", "sat2 hd2 ht2 wt2");
        shapeFormula.Add("x1", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc dy1 0");
        shapeFormula.Add("x2", "+- hc dx2 0");
        shapeFormula.Add("y2", "+- vc dy2 0");
        shapeFormula.Add("x3", "+/ x1 x2 2");
        shapeFormula.Add("y3", "+/ y1 y2 2");
        shapeFormula.Add("midAng0", "*/ swAng 1 2");
        shapeFormula.Add("midAng", "+- stAng midAng0 cd2");
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.L_Shape:
        shapeFormula.Add("maxAdj1", "*/ 100000 h ss");
        shapeFormula.Add("maxAdj2", "*/ 100000 w ss");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("x1", "*/ ss a2 100000");
        shapeFormula.Add("dy1", "*/ ss a1 100000");
        shapeFormula.Add("y1", "+- b 0 dy1");
        shapeFormula.Add("cx1", "*/ x1 1 2");
        shapeFormula.Add("cy1", "+/ y1 b 2");
        shapeFormula.Add("d", "+- w 0 h");
        shapeFormula.Add("it", "?: d y1 t");
        shapeFormula.Add("ir", "?: d r x1");
        break;
      case AutoShapeType.MathPlus:
        shapeFormula.Add("a1", "pin 0 adj1 73490");
        shapeFormula.Add("dx1", "*/ w 73490 200000");
        shapeFormula.Add("dy1", "*/ h 73490 200000");
        shapeFormula.Add("dx2", "*/ ss a1 200000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dx2");
        shapeFormula.Add("y3", "+- vc dx2 0");
        shapeFormula.Add("y4", "+- vc dy1 0");
        break;
      case AutoShapeType.MathMinus:
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("dy1", "*/ h a1 200000");
        shapeFormula.Add("dx1", "*/ w 73490 200000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc dy1 0");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc dx1 0");
        break;
      case AutoShapeType.MathMultiply:
        shapeFormula.Add("a1", "pin 0 adj1 51965");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("a0", "at2 w h");
        shapeFormula.Add("a", "*/ a0 180 " + Math.PI.ToString());
        shapeFormula.Add("sa", "sin 1 a");
        shapeFormula.Add("ca", "cos 1 a");
        shapeFormula.Add("ta", "tan 1 a");
        shapeFormula.Add("dl", "mod w h 0");
        shapeFormula.Add("rw", "*/ dl 51965 100000");
        shapeFormula.Add("lM", "+- dl 0 rw");
        shapeFormula.Add("xM", "*/ ca lM 2");
        shapeFormula.Add("yM", "*/ sa lM 2");
        shapeFormula.Add("dxAM", "*/ sa th 2");
        shapeFormula.Add("dyAM", "*/ ca th 2");
        shapeFormula.Add("xA", "+- xM 0 dxAM");
        shapeFormula.Add("yA", "+- yM dyAM 0");
        shapeFormula.Add("xB", "+- xM dxAM 0");
        shapeFormula.Add("yB", "+- yM 0 dyAM");
        shapeFormula.Add("xBC", "+- hc 0 xB");
        shapeFormula.Add("yBC", "*/ xBC ta 1");
        shapeFormula.Add("yC", "+- yBC yB 0");
        shapeFormula.Add("xD", "+- r 0 xB");
        shapeFormula.Add("xE", "+- r 0 xA");
        shapeFormula.Add("yFE", "+- vc 0 yA");
        shapeFormula.Add("xFE", "*/ yFE 1 ta");
        shapeFormula.Add("xF", "+- xE 0 xFE");
        shapeFormula.Add("xL", "+- xA xFE 0");
        shapeFormula.Add("yG", "+- b 0 yA");
        shapeFormula.Add("yH", "+- b 0 yB");
        shapeFormula.Add("yI", "+- b 0 yC");
        shapeFormula.Add("xC2", "+- r 0 xM");
        shapeFormula.Add("yC3", "+- b 0 yM");
        break;
      case AutoShapeType.MathDivision:
        shapeFormula.Add("a1", "pin 1000 adj1 36745");
        shapeFormula.Add("ma1", "+- 0 0 a1");
        shapeFormula.Add("ma3h", "+/ 73490 ma1 4");
        shapeFormula.Add("ma3w", "*/ 36745 w h");
        shapeFormula.Add("maxAdj3", "min ma3h ma3w");
        shapeFormula.Add("a3", "pin 1000 adj3 maxAdj3");
        shapeFormula.Add("m4a3", "*/ -4 a3 1");
        shapeFormula.Add("maxAdj2", "+- 73490 m4a3 a1");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("dy1", "*/ h a1 200000");
        shapeFormula.Add("yg", "*/ h a2 100000");
        shapeFormula.Add("rad", "*/ h a3 100000");
        shapeFormula.Add("dx1", "*/ w 73490 200000");
        shapeFormula.Add("y3", "+- vc 0 dy1");
        shapeFormula.Add("y4", "+- vc dy1 0");
        shapeFormula.Add("a", "+- yg rad 0");
        shapeFormula.Add("y2", "+- y3 0 a");
        shapeFormula.Add("y1", "+- y2 0 rad");
        shapeFormula.Add("y5", "+- b 0 y1");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x3", "+- hc dx1 0");
        shapeFormula.Add("x2", "+- hc 0 rad");
        break;
      case AutoShapeType.MathEqual:
        shapeFormula.Add("a1", "pin 0 adj1 36745");
        shapeFormula.Add("2a1", "*/ a1 2 1");
        shapeFormula.Add("mAdj2", "+- 100000 0 2a1");
        shapeFormula.Add("a2", "pin 0 adj2 mAdj2");
        shapeFormula.Add("dy1", "*/ h a1 100000");
        shapeFormula.Add("dy2", "*/ h a2 200000");
        shapeFormula.Add("dx1", "*/ w 73490 200000");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc dy2 0");
        shapeFormula.Add("y1", "+- y2 0 dy1");
        shapeFormula.Add("y4", "+- y3 dy1 0");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc dx1 0");
        shapeFormula.Add("yC1", "+/ y1 y2 2");
        shapeFormula.Add("yC2", "+/ y3 y4 2");
        break;
      case AutoShapeType.MathNotEqual:
        shapeFormula.Add("a1", "pin 0 adj1 50000");
        shapeFormula.Add("crAng", "pin 4200000 adj2 6600000");
        shapeFormula.Add("2a1", "*/ a1 2 1");
        shapeFormula.Add("maxAdj3", "+- 100000 0 2a1");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("dy1", "*/ h a1 100000");
        shapeFormula.Add("dy2", "*/ h a3 200000");
        shapeFormula.Add("dx1", "*/ w 73490 200000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x8", "+- hc dx1 0");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc dy2 0");
        shapeFormula.Add("y1", "+- y2 0 dy1");
        shapeFormula.Add("y4", "+- y3 dy1 0");
        shapeFormula.Add("cadj2", "+- crAng 0 cd4");
        shapeFormula.Add("xadj2", "tan hd2 cadj2");
        shapeFormula.Add("len", "mod xadj2 hd2 0");
        shapeFormula.Add("bhw", "*/ len dy1 hd2");
        shapeFormula.Add("bhw2", "*/ bhw 1 2");
        shapeFormula.Add("x7", "+- hc xadj2 bhw2");
        shapeFormula.Add("dx67", "*/ xadj2 y1 hd2");
        shapeFormula.Add("x6", "+- x7 0 dx67");
        shapeFormula.Add("dx57", "*/ xadj2 y2 hd2");
        shapeFormula.Add("x5", "+- x7 0 dx57");
        shapeFormula.Add("dx47", "*/ xadj2 y3 hd2");
        shapeFormula.Add("x4", "+- x7 0 dx47");
        shapeFormula.Add("dx37", "*/ xadj2 y4 hd2");
        shapeFormula.Add("x3", "+- x7 0 dx37");
        shapeFormula.Add("dx27", "*/ xadj2 2 1");
        shapeFormula.Add("x2", "+- x7 0 dx27");
        shapeFormula.Add("rx7", "+- x7 bhw 0");
        shapeFormula.Add("rx6", "+- x6 bhw 0");
        shapeFormula.Add("rx5", "+- x5 bhw 0");
        shapeFormula.Add("rx4", "+- x4 bhw 0");
        shapeFormula.Add("rx3", "+- x3 bhw 0");
        shapeFormula.Add("rx2", "+- x2 bhw 0");
        shapeFormula.Add("dx7", "*/ dy1 hd2 len");
        shapeFormula.Add("rxt", "+- x7 dx7 0");
        shapeFormula.Add("lxt", "+- rx7 0 dx7");
        shapeFormula.Add("rx", "?: cadj2 rxt rx7");
        shapeFormula.Add("lx", "?: cadj2 x7 lxt");
        shapeFormula.Add("dy3", "*/ dy1 xadj2 len");
        shapeFormula.Add("dy4", "+- 0 0 dy3");
        shapeFormula.Add("ry", "?: cadj2 dy3 t");
        shapeFormula.Add("ly", "?: cadj2 t dy4");
        shapeFormula.Add("dlx", "+- w 0 rx");
        shapeFormula.Add("drx", "+- w 0 lx");
        shapeFormula.Add("dly", "+- h 0 ry");
        shapeFormula.Add("dry", "+- h 0 ly");
        shapeFormula.Add("xC1", "+/ rx lx 2");
        shapeFormula.Add("xC2", "+/ drx dlx 2");
        shapeFormula.Add("yC1", "+/ ry ly 2");
        shapeFormula.Add("yC2", "+/ y1 y2 2");
        shapeFormula.Add("yC3", "+/ y3 y4 2");
        shapeFormula.Add("yC4", "+/ dry dly 2");
        break;
      case AutoShapeType.Cloud:
        shapeFormula.Add("il", "*/ w 2977 21600");
        shapeFormula.Add("it", "*/ h 3262 21600");
        shapeFormula.Add("ir", "*/ w 17087 21600");
        shapeFormula.Add("ib", "*/ h 17337 21600");
        shapeFormula.Add("g27", "*/ w 67 21600");
        shapeFormula.Add("g28", "*/ h 21577 21600");
        shapeFormula.Add("g29", "*/ w 21582 21600");
        shapeFormula.Add("g30", "*/ h 1235 21600");
        break;
      case AutoShapeType.ElbowConnector:
        shapeFormula.Add("x1", "*/ w adj1 100000");
        break;
      case AutoShapeType.CurvedConnector:
        shapeFormula.Add("x2", "*/ w adj1 100000");
        shapeFormula.Add("x1", "+/ l x2 2");
        shapeFormula.Add("x3", "+/ r x2 2");
        shapeFormula.Add("y3", "*/ h 3 4");
        break;
      case AutoShapeType.BentConnector4:
        shapeFormula.Add("x1", "*/ w adj1 100000");
        shapeFormula.Add("x2", "+/ x1 r 2");
        shapeFormula.Add("y2", "*/ h adj2 100000");
        shapeFormula.Add("y1", "+/ t y2 2");
        break;
      case AutoShapeType.BentConnector5:
        shapeFormula.Add("x1", "*/ w adj1 100000");
        shapeFormula.Add("x3", "*/ w adj3 100000");
        shapeFormula.Add("x2", "+/ x1 x3 2");
        shapeFormula.Add("y2", "*/ h adj2 100000");
        shapeFormula.Add("y1", "+/ t y2 2");
        shapeFormula.Add("y3", "+/ b y2 2");
        break;
      case AutoShapeType.CurvedConnector4:
        shapeFormula.Add("x2", "*/ w adj1 100000");
        shapeFormula.Add("x1", "+/ l x2 2");
        shapeFormula.Add("x3", "+/ r x2 2");
        shapeFormula.Add("x4", "+/ x2 x3 2");
        shapeFormula.Add("x5", "+/ x3 r 2");
        shapeFormula.Add("y4", "*/ h adj2 100000");
        shapeFormula.Add("y1", "+/ t y4 2");
        shapeFormula.Add("y2", "+/ t y1 2");
        shapeFormula.Add("y3", "+/ y1 y4 2");
        shapeFormula.Add("y5", "+/ b y4 2");
        break;
      case AutoShapeType.CurvedConnector5:
        shapeFormula.Add("x3", "*/ w adj1 100000");
        shapeFormula.Add("x6", "*/ w adj3 100000");
        shapeFormula.Add("x1", "+/ x3 x6 2");
        shapeFormula.Add("x2", "+/ l x3 2");
        shapeFormula.Add("x4", "+/ x3 x1 2");
        shapeFormula.Add("x5", "+/ x6 x1 2");
        shapeFormula.Add("x7", "+/ x6 r 2");
        shapeFormula.Add("y4", "*/ h adj2 100000");
        shapeFormula.Add("y1", "+/ t y4 2");
        shapeFormula.Add("y2", "+/ t y1 2");
        shapeFormula.Add("y3", "+/ y1 y4 2");
        shapeFormula.Add("y5", "+/ b y4 2");
        shapeFormula.Add("y6", "+/ y5 y4 2");
        shapeFormula.Add("y7", "+/ y5 b 2");
        break;
    }
    return shapeFormula;
  }

  private Dictionary<string, float> GetDefaultPathAdjValues(AutoShapeType shapeType)
  {
    Dictionary<string, float> defaultPathAdjValues = new Dictionary<string, float>();
    switch (shapeType)
    {
      case AutoShapeType.Parallelogram:
      case AutoShapeType.Trapezoid:
        defaultPathAdjValues.Add("adj", 25000f);
        break;
      case AutoShapeType.RoundedRectangle:
      case AutoShapeType.FoldedCorner:
      case AutoShapeType.DoubleBracket:
      case AutoShapeType.Plaque:
      case AutoShapeType.RoundSingleCornerRectangle:
      case AutoShapeType.SnipSingleCornerRectangle:
        defaultPathAdjValues.Add("adj", 16667f);
        break;
      case AutoShapeType.Octagon:
        defaultPathAdjValues.Add("adj", 29289f);
        break;
      case AutoShapeType.IsoscelesTriangle:
      case AutoShapeType.Moon:
      case AutoShapeType.Pentagon:
      case AutoShapeType.Chevron:
      case AutoShapeType.DiagonalStripe:
        defaultPathAdjValues.Add("adj", 50000f);
        break;
      case AutoShapeType.Hexagon:
        defaultPathAdjValues.Add("adj", 25000f);
        defaultPathAdjValues.Add("vf", 115470f);
        break;
      case AutoShapeType.Cross:
      case AutoShapeType.Can:
      case AutoShapeType.Cube:
      case AutoShapeType.Donut:
      case AutoShapeType.Sun:
        defaultPathAdjValues.Add("adj", 25000f);
        break;
      case AutoShapeType.RegularPentagon:
        defaultPathAdjValues.Add("hf", 105146f);
        defaultPathAdjValues.Add("vf", 110557f);
        break;
      case AutoShapeType.Bevel:
      case AutoShapeType.Star4Point:
      case AutoShapeType.VerticalScroll:
      case AutoShapeType.HorizontalScroll:
        defaultPathAdjValues.Add("adj", 12500f);
        break;
      case AutoShapeType.SmileyFace:
        defaultPathAdjValues.Add("adj", 4653f);
        break;
      case AutoShapeType.NoSymbol:
        defaultPathAdjValues.Add("adj", 18750f);
        break;
      case AutoShapeType.BlockArc:
        defaultPathAdjValues.Add("adj1", 1.08E+07f);
        defaultPathAdjValues.Add("adj2", 0.0f);
        defaultPathAdjValues.Add("adj3", 25000f);
        break;
      case AutoShapeType.Arc:
        defaultPathAdjValues.Add("adj1", 1.62E+07f);
        defaultPathAdjValues.Add("adj2", 0.0f);
        break;
      case AutoShapeType.DoubleBrace:
      case AutoShapeType.LeftBracket:
      case AutoShapeType.RightBracket:
        defaultPathAdjValues.Add("adj", 8333f);
        break;
      case AutoShapeType.LeftBrace:
      case AutoShapeType.RightBrace:
        defaultPathAdjValues.Add("adj1", 8333f);
        defaultPathAdjValues.Add("adj2", 50000f);
        break;
      case AutoShapeType.RightArrow:
      case AutoShapeType.LeftArrow:
      case AutoShapeType.UpArrow:
      case AutoShapeType.DownArrow:
      case AutoShapeType.LeftRightArrow:
      case AutoShapeType.UpDownArrow:
      case AutoShapeType.StripedRightArrow:
      case AutoShapeType.NotchedRightArrow:
      case AutoShapeType.L_Shape:
        defaultPathAdjValues.Add("adj1", 50000f);
        defaultPathAdjValues.Add("adj2", 50000f);
        break;
      case AutoShapeType.QuadArrow:
      case AutoShapeType.LeftRightUpArrow:
        defaultPathAdjValues.Add("adj1", 22500f);
        defaultPathAdjValues.Add("adj2", 22500f);
        defaultPathAdjValues.Add("adj3", 22500f);
        break;
      case AutoShapeType.BentArrow:
        defaultPathAdjValues.Add("adj1", 25000f);
        defaultPathAdjValues.Add("adj2", 25000f);
        defaultPathAdjValues.Add("adj3", 25000f);
        defaultPathAdjValues.Add("adj4", 43750f);
        break;
      case AutoShapeType.UTurnArrow:
        defaultPathAdjValues.Add("adj1", 25000f);
        defaultPathAdjValues.Add("adj2", 25000f);
        defaultPathAdjValues.Add("adj3", 25000f);
        defaultPathAdjValues.Add("adj4", 43750f);
        defaultPathAdjValues.Add("adj5", 75000f);
        break;
      case AutoShapeType.LeftUpArrow:
      case AutoShapeType.BentUpArrow:
        defaultPathAdjValues.Add("adj1", 25000f);
        defaultPathAdjValues.Add("adj2", 25000f);
        defaultPathAdjValues.Add("adj3", 25000f);
        break;
      case AutoShapeType.CurvedRightArrow:
      case AutoShapeType.CurvedLeftArrow:
      case AutoShapeType.CurvedUpArrow:
      case AutoShapeType.CurvedDownArrow:
        defaultPathAdjValues.Add("adj1", 25000f);
        defaultPathAdjValues.Add("adj2", 50000f);
        defaultPathAdjValues.Add("adj3", 25000f);
        break;
      case AutoShapeType.RightArrowCallout:
      case AutoShapeType.LeftArrowCallout:
      case AutoShapeType.UpArrowCallout:
      case AutoShapeType.DownArrowCallout:
        defaultPathAdjValues.Add("adj1", 25000f);
        defaultPathAdjValues.Add("adj2", 25000f);
        defaultPathAdjValues.Add("adj3", 25000f);
        defaultPathAdjValues.Add("adj4", 64977f);
        break;
      case AutoShapeType.LeftRightArrowCallout:
      case AutoShapeType.UpDownArrowCallout:
        defaultPathAdjValues.Add("adj1", 25000f);
        defaultPathAdjValues.Add("adj2", 25000f);
        defaultPathAdjValues.Add("adj3", 25000f);
        defaultPathAdjValues.Add("adj4", 48123f);
        break;
      case AutoShapeType.QuadArrowCallout:
        defaultPathAdjValues.Add("adj1", 18515f);
        defaultPathAdjValues.Add("adj2", 18515f);
        defaultPathAdjValues.Add("adj3", 18515f);
        defaultPathAdjValues.Add("adj4", 48123f);
        break;
      case AutoShapeType.CircularArrow:
        defaultPathAdjValues.Add("adj1", 12500f);
        defaultPathAdjValues.Add("adj2", 19f);
        defaultPathAdjValues.Add("adj3", 341f);
        defaultPathAdjValues.Add("adj4", 180f);
        defaultPathAdjValues.Add("adj5", 12500f);
        break;
      case AutoShapeType.Star5Point:
        defaultPathAdjValues.Add("adj", 19098f);
        defaultPathAdjValues.Add("hf", 105146f);
        defaultPathAdjValues.Add("vf", 110557f);
        break;
      case AutoShapeType.Star8Point:
      case AutoShapeType.Star16Point:
      case AutoShapeType.Star24Point:
      case AutoShapeType.Star32Point:
      case AutoShapeType.Star12Point:
        defaultPathAdjValues.Add("adj", 37500f);
        break;
      case AutoShapeType.UpRibbon:
      case AutoShapeType.DownRibbon:
        defaultPathAdjValues.Add("adj1", 16667f);
        defaultPathAdjValues.Add("adj2", 50000f);
        break;
      case AutoShapeType.CurvedUpRibbon:
      case AutoShapeType.CurvedDownRibbon:
        defaultPathAdjValues.Add("adj1", 25000f);
        defaultPathAdjValues.Add("adj2", 50000f);
        defaultPathAdjValues.Add("adj3", 12500f);
        break;
      case AutoShapeType.Wave:
        defaultPathAdjValues.Add("adj1", 12500f);
        defaultPathAdjValues.Add("adj2", 0.0f);
        break;
      case AutoShapeType.DoubleWave:
        defaultPathAdjValues.Add("adj1", 6250f);
        defaultPathAdjValues.Add("adj2", 0.0f);
        break;
      case AutoShapeType.RectangularCallout:
      case AutoShapeType.OvalCallout:
      case AutoShapeType.CloudCallout:
        defaultPathAdjValues.Add("adj1", -20833f);
        defaultPathAdjValues.Add("adj2", 62500f);
        break;
      case AutoShapeType.RoundedRectangularCallout:
        defaultPathAdjValues.Add("adj1", -20833f);
        defaultPathAdjValues.Add("adj2", 62500f);
        defaultPathAdjValues.Add("adj3", 16667f);
        break;
      case AutoShapeType.LineCallout1:
      case AutoShapeType.LineCallout1NoBorder:
      case AutoShapeType.LineCallout1AccentBar:
      case AutoShapeType.LineCallout1BorderAndAccentBar:
        defaultPathAdjValues.Add("adj1", 18750f);
        defaultPathAdjValues.Add("adj2", -8333f);
        defaultPathAdjValues.Add("adj3", 112500f);
        defaultPathAdjValues.Add("adj4", -38333f);
        break;
      case AutoShapeType.LineCallout2:
      case AutoShapeType.LineCallout2AccentBar:
      case AutoShapeType.LineCallout2NoBorder:
      case AutoShapeType.LineCallout2BorderAndAccentBar:
        defaultPathAdjValues.Add("adj1", 18750f);
        defaultPathAdjValues.Add("adj2", -8333f);
        defaultPathAdjValues.Add("adj3", 18750f);
        defaultPathAdjValues.Add("adj4", -16667f);
        defaultPathAdjValues.Add("adj5", 112500f);
        defaultPathAdjValues.Add("adj6", -46667f);
        break;
      case AutoShapeType.LineCallout3:
      case AutoShapeType.LineCallout3AccentBar:
      case AutoShapeType.LineCallout3NoBorder:
      case AutoShapeType.LineCallout3BorderAndAccentBar:
        defaultPathAdjValues.Add("adj1", 18750f);
        defaultPathAdjValues.Add("adj2", -8333f);
        defaultPathAdjValues.Add("adj3", 18750f);
        defaultPathAdjValues.Add("adj4", -16667f);
        defaultPathAdjValues.Add("adj5", 100000f);
        defaultPathAdjValues.Add("adj6", -16667f);
        defaultPathAdjValues.Add("adj7", 112963f);
        defaultPathAdjValues.Add("adj8", -8333f);
        break;
      case AutoShapeType.Pie:
        defaultPathAdjValues.Add("adj1", 0.0f);
        defaultPathAdjValues.Add("adj2", 270f);
        break;
      case AutoShapeType.Decagon:
        defaultPathAdjValues.Add("vf", 105146f);
        break;
      case AutoShapeType.Heptagon:
        defaultPathAdjValues.Add("hf", 102572f);
        defaultPathAdjValues.Add("vf", 105210f);
        break;
      case AutoShapeType.Star6Point:
        defaultPathAdjValues.Add("adj", 28868f);
        defaultPathAdjValues.Add("hf", 115470f);
        break;
      case AutoShapeType.Star7Point:
        defaultPathAdjValues.Add("adj", 34601f);
        defaultPathAdjValues.Add("hf", 102572f);
        defaultPathAdjValues.Add("vf", 105210f);
        break;
      case AutoShapeType.Star10Point:
        defaultPathAdjValues.Add("adj", 42533f);
        defaultPathAdjValues.Add("hf", 105146f);
        break;
      case AutoShapeType.RoundSameSideCornerRectangle:
      case AutoShapeType.RoundDiagonalCornerRectangle:
      case AutoShapeType.SnipSameSideCornerRectangle:
        defaultPathAdjValues.Add("adj1", 16667f);
        defaultPathAdjValues.Add("adj2", 0.0f);
        break;
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
        defaultPathAdjValues.Add("adj1", 16667f);
        defaultPathAdjValues.Add("adj2", 16667f);
        break;
      case AutoShapeType.SnipDiagonalCornerRectangle:
        defaultPathAdjValues.Add("adj2", 16667f);
        defaultPathAdjValues.Add("adj1", 0.0f);
        break;
      case AutoShapeType.Frame:
        defaultPathAdjValues.Add("adj1", 12500f);
        break;
      case AutoShapeType.HalfFrame:
        defaultPathAdjValues.Add("adj1", 33333f);
        defaultPathAdjValues.Add("adj2", 33333f);
        break;
      case AutoShapeType.Teardrop:
        defaultPathAdjValues.Add("adj", 100000f);
        break;
      case AutoShapeType.Chord:
        defaultPathAdjValues.Add("adj1", 45f);
        defaultPathAdjValues.Add("adj2", 270f);
        break;
      case AutoShapeType.MathPlus:
      case AutoShapeType.MathMinus:
      case AutoShapeType.MathMultiply:
        defaultPathAdjValues.Add("adj1", 23520f);
        break;
      case AutoShapeType.MathDivision:
        defaultPathAdjValues.Add("adj1", 23520f);
        defaultPathAdjValues.Add("adj2", 5880f);
        defaultPathAdjValues.Add("adj3", 11760f);
        break;
      case AutoShapeType.MathEqual:
        defaultPathAdjValues.Add("adj1", 23520f);
        defaultPathAdjValues.Add("adj2", 11760f);
        break;
      case AutoShapeType.MathNotEqual:
        defaultPathAdjValues.Add("adj1", 23520f);
        defaultPathAdjValues.Add("adj2", 6600000f);
        defaultPathAdjValues.Add("adj3", 11760f);
        break;
      case AutoShapeType.ElbowConnector:
      case AutoShapeType.CurvedConnector:
        defaultPathAdjValues.Add("adj1", 50000f);
        break;
      case AutoShapeType.BentConnector4:
      case AutoShapeType.CurvedConnector4:
        defaultPathAdjValues.Add("adj1", 50000f);
        defaultPathAdjValues.Add("adj2", 50000f);
        break;
      case AutoShapeType.BentConnector5:
      case AutoShapeType.CurvedConnector5:
        defaultPathAdjValues.Add("adj1", 50000f);
        defaultPathAdjValues.Add("adj2", 50000f);
        defaultPathAdjValues.Add("adj3", 50000f);
        break;
    }
    return defaultPathAdjValues;
  }

  internal void Close()
  {
    if (this._shapeGuide == null)
      return;
    this._shapeGuide.Clear();
    this._shapeGuide = (Dictionary<string, string>) null;
  }
}
