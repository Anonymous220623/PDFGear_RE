// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.DrawingHelpers.IndeterminateFill_GeometryGenerator
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Utils.DrawingHelpers;

internal static class IndeterminateFill_GeometryGenerator
{
  public static readonly Rect Viewbox = new Rect(0.0, 0.0, 1024.0, 1024.0);
  public static System.Collections.Generic.IReadOnlyList<string> GeometryData = (System.Collections.Generic.IReadOnlyList<string>) new string[1]
  {
    "M1024 0L1024 1024 0 1024 0 0Z"
  };
  private static Geometry geometry;

  public static Geometry Geometry
  {
    get
    {
      if (IndeterminateFill_GeometryGenerator.geometry == null)
      {
        if (IndeterminateFill_GeometryGenerator.GeometryData.Count == 0)
          return (Geometry) null;
        GeometryGroup geometryGroup = new GeometryGroup();
        foreach (string source in (IEnumerable<string>) IndeterminateFill_GeometryGenerator.GeometryData)
          geometryGroup.Children.Add(Geometry.Parse(source));
        geometryGroup.Transform = (Transform) new TranslateTransform(-IndeterminateFill_GeometryGenerator.Viewbox.Left, -IndeterminateFill_GeometryGenerator.Viewbox.Top);
        geometryGroup.Freeze();
        IndeterminateFill_GeometryGenerator.geometry = (Geometry) geometryGroup;
      }
      return IndeterminateFill_GeometryGenerator.geometry;
    }
  }

  public static List<PdfPageObject> CreateGeometryPath(
    FS_RECTF rect,
    FS_COLOR strokeColor,
    FS_COLOR fillColor,
    float strokeWidth)
  {
    List<PdfPageObject> geometryPath = new List<PdfPageObject>();
    Matrix identity = Matrix.Identity;
    identity.Scale((double) rect.Width / IndeterminateFill_GeometryGenerator.Viewbox.Width, (double) rect.Height / IndeterminateFill_GeometryGenerator.Viewbox.Height);
    Point point1 = identity.Transform(new Point(0.0, 0.0));
    Point point2 = identity.Transform(new Point(1024.0, 1024.0));
    PdfPathObject pdfPathObject1 = PdfPathObject.Create(FillModes.None, false);
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(point1.X, point2.Y, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(point2.X, point2.Y, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(point2.X, point1.Y, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(point1.X, point1.Y, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject1.LineWidth = 0.0f;
    geometryPath.Add((PdfPageObject) pdfPathObject1);
    PdfPathObject pdfPathObject2 = PdfPathObject.Create(FillModes.Alternate, strokeColor.A != 0);
    pdfPathObject2.StrokeColor = strokeColor;
    pdfPathObject2.FillColor = fillColor;
    pdfPathObject2.LineWidth = strokeWidth;
    Point point3 = identity.Transform(new Point(1024.0, 1024.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point3.X, point3.Y, PathPointFlags.MoveTo));
    Point point4 = identity.Transform(new Point(1024.0, 0.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point4.X, point4.Y, PathPointFlags.LineTo));
    Point point5 = identity.Transform(new Point(0.0, 0.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point5.X, point5.Y, PathPointFlags.LineTo));
    Point point6 = identity.Transform(new Point(0.0, 1024.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point6.X, point6.Y, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    geometryPath.Add((PdfPageObject) pdfPathObject2);
    return geometryPath;
  }
}
