// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.DrawingHelpers.Cancel_GeometryGenerator
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

internal static class Cancel_GeometryGenerator
{
  public static readonly Rect Viewbox = new Rect(0.0, 0.0, 1024.0, 1024.0);
  public static System.Collections.Generic.IReadOnlyList<string> GeometryData = (System.Collections.Generic.IReadOnlyList<string>) new string[1]
  {
    "M557.5 512L902.5 857.5 857.5 902.5 512 557.5 166.5 902.5 121.5 857.5 466.5 512 121.5 166.5 166.5 121.5 512 466.5 857.5 121.5 902.5 166.5Z"
  };
  private static Geometry geometry;

  public static Geometry Geometry
  {
    get
    {
      if (Cancel_GeometryGenerator.geometry == null)
      {
        if (Cancel_GeometryGenerator.GeometryData.Count == 0)
          return (Geometry) null;
        GeometryGroup geometryGroup = new GeometryGroup();
        foreach (string source in (IEnumerable<string>) Cancel_GeometryGenerator.GeometryData)
          geometryGroup.Children.Add(Geometry.Parse(source));
        geometryGroup.Transform = (Transform) new TranslateTransform(-Cancel_GeometryGenerator.Viewbox.Left, -Cancel_GeometryGenerator.Viewbox.Top);
        geometryGroup.Freeze();
        Cancel_GeometryGenerator.geometry = (Geometry) geometryGroup;
      }
      return Cancel_GeometryGenerator.geometry;
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
    identity.Scale((double) rect.Width / Cancel_GeometryGenerator.Viewbox.Width, (double) rect.Height / Cancel_GeometryGenerator.Viewbox.Height);
    Point point1 = identity.Transform(new Point(0.0, 0.0));
    Point point2 = identity.Transform(new Point(1024.0, 1024.0));
    PdfPathObject pdfPathObject1 = PdfPathObject.Create(FillModes.None, false);
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(point1.X, point2.Y, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(point2.X, point2.Y, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(point2.X, point1.Y, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(point1.X, point1.Y, PathPointFlags.MoveTo | PathPointFlags.CloseFigure));
    pdfPathObject1.LineWidth = 0.0f;
    geometryPath.Add((PdfPageObject) pdfPathObject1);
    PdfPathObject pdfPathObject2 = PdfPathObject.Create(FillModes.Alternate, strokeColor.A != 0);
    pdfPathObject2.StrokeColor = strokeColor;
    pdfPathObject2.FillColor = fillColor;
    pdfPathObject2.LineWidth = strokeWidth;
    Point point3 = identity.Transform(new Point(557.5, 513.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point3.X, point3.Y, PathPointFlags.MoveTo));
    Point point4 = identity.Transform(new Point(902.5, 167.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point4.X, point4.Y, PathPointFlags.LineTo));
    Point point5 = identity.Transform(new Point(857.5, 122.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point5.X, point5.Y, PathPointFlags.LineTo));
    Point point6 = identity.Transform(new Point(512.0, 467.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point6.X, point6.Y, PathPointFlags.LineTo));
    Point point7 = identity.Transform(new Point(166.5, 122.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point7.X, point7.Y, PathPointFlags.LineTo));
    Point point8 = identity.Transform(new Point(121.5, 167.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point8.X, point8.Y, PathPointFlags.LineTo));
    Point point9 = identity.Transform(new Point(466.5, 513.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point9.X, point9.Y, PathPointFlags.LineTo));
    Point point10 = identity.Transform(new Point(121.5, 858.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point10.X, point10.Y, PathPointFlags.LineTo));
    Point point11 = identity.Transform(new Point(166.5, 903.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point11.X, point11.Y, PathPointFlags.LineTo));
    Point point12 = identity.Transform(new Point(512.0, 558.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point12.X, point12.Y, PathPointFlags.LineTo));
    Point point13 = identity.Transform(new Point(857.5, 903.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point13.X, point13.Y, PathPointFlags.LineTo));
    Point point14 = identity.Transform(new Point(902.5, 858.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point14.X, point14.Y, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    geometryPath.Add((PdfPageObject) pdfPathObject2);
    return geometryPath;
  }
}
