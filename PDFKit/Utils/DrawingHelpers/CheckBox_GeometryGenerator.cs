// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.DrawingHelpers.CheckBox_GeometryGenerator
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

internal static class CheckBox_GeometryGenerator
{
  public static readonly Rect Viewbox = new Rect(0.0, 0.0, 1024.0, 1024.0);
  public static System.Collections.Generic.IReadOnlyList<string> GeometryData = (System.Collections.Generic.IReadOnlyList<string>) new string[1]
  {
    "M1024 0L1024 1024 0 1024 0 0ZM960 64L64 64 64 960 960 960Z"
  };
  private static Geometry geometry;

  public static Geometry Geometry
  {
    get
    {
      if (CheckBox_GeometryGenerator.geometry == null)
      {
        if (CheckBox_GeometryGenerator.GeometryData.Count == 0)
          return (Geometry) null;
        GeometryGroup geometryGroup = new GeometryGroup();
        foreach (string source in (IEnumerable<string>) CheckBox_GeometryGenerator.GeometryData)
          geometryGroup.Children.Add(Geometry.Parse(source));
        geometryGroup.Transform = (Transform) new TranslateTransform(-CheckBox_GeometryGenerator.Viewbox.Left, -CheckBox_GeometryGenerator.Viewbox.Top);
        geometryGroup.Freeze();
        CheckBox_GeometryGenerator.geometry = (Geometry) geometryGroup;
      }
      return CheckBox_GeometryGenerator.geometry;
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
    identity.Scale((double) rect.Width / CheckBox_GeometryGenerator.Viewbox.Width, (double) rect.Height / CheckBox_GeometryGenerator.Viewbox.Height);
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
    Point point3 = identity.Transform(new Point(1024.0, 1024.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point3.X, point3.Y, PathPointFlags.MoveTo));
    Point point4 = identity.Transform(new Point(1024.0, 0.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point4.X, point4.Y, PathPointFlags.LineTo));
    Point point5 = identity.Transform(new Point(0.0, 0.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point5.X, point5.Y, PathPointFlags.LineTo));
    Point point6 = identity.Transform(new Point(0.0, 1024.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point6.X, point6.Y, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    Point point7 = identity.Transform(new Point(960.0, 960.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point7.X, point7.Y, PathPointFlags.MoveTo));
    Point point8 = identity.Transform(new Point(64.0, 960.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point8.X, point8.Y, PathPointFlags.LineTo));
    Point point9 = identity.Transform(new Point(64.0, 64.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point9.X, point9.Y, PathPointFlags.LineTo));
    Point point10 = identity.Transform(new Point(960.0, 64.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point10.X, point10.Y, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    geometryPath.Add((PdfPageObject) pdfPathObject2);
    return geometryPath;
  }
}
