// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.DrawingHelpers.RadioCheck_GeometryGenerator
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

internal static class RadioCheck_GeometryGenerator
{
  public static readonly Rect Viewbox = new Rect(176.0, 176.0, 672.0, 672.0);
  public static System.Collections.Generic.IReadOnlyList<string> GeometryData = (System.Collections.Generic.IReadOnlyList<string>) new string[1]
  {
    "M512 176C543 176 572.75 180 601.25 188 629.75 196 656.5 207.333 681.5 222 706.5 236.667 729.167 254.167 749.5 274.5 769.833 294.833 787.333 317.5 802 342.5 816.667 367.5 828 394.25 836 422.75 844 451.25 848 481 848 512 848 543 844 572.75 836 601.25 828 629.75 816.667 656.5 802 681.5 787.333 706.5 769.833 729.167 749.5 749.5 729.167 769.833 706.5 787.333 681.5 802 656.5 816.667 629.75 828 601.25 836 572.75 844 543 848 512 848 481 848 451.25 844 422.75 836 394.25 828 367.5 816.667 342.5 802 317.5 787.333 294.833 769.833 274.5 749.5 254.167 729.167 236.667 706.5 222 681.5 207.333 656.5 196 629.75 188 601.25 180 572.75 176 543 176 512 176 481 180 451.25 188 422.75 196 394.25 207.333 367.5 222 342.5 236.667 317.5 254.167 294.833 274.5 274.5 294.833 254.167 317.5 236.667 342.5 222 367.5 207.333 394.25 196 422.75 188 451.25 180 481 176 512 176Z"
  };
  private static Geometry geometry;

  public static Geometry Geometry
  {
    get
    {
      if (RadioCheck_GeometryGenerator.geometry == null)
      {
        if (RadioCheck_GeometryGenerator.GeometryData.Count == 0)
          return (Geometry) null;
        GeometryGroup geometryGroup = new GeometryGroup();
        foreach (string source in (IEnumerable<string>) RadioCheck_GeometryGenerator.GeometryData)
          geometryGroup.Children.Add(Geometry.Parse(source));
        geometryGroup.Transform = (Transform) new TranslateTransform(-RadioCheck_GeometryGenerator.Viewbox.Left, -RadioCheck_GeometryGenerator.Viewbox.Top);
        geometryGroup.Freeze();
        RadioCheck_GeometryGenerator.geometry = (Geometry) geometryGroup;
      }
      return RadioCheck_GeometryGenerator.geometry;
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
    identity.Scale((double) rect.Width / RadioCheck_GeometryGenerator.Viewbox.Width, (double) rect.Height / RadioCheck_GeometryGenerator.Viewbox.Height);
    Point point1 = identity.Transform(new Point(176.0, 176.0));
    Point point2 = identity.Transform(new Point(848.0, 848.0));
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
    Point point3 = identity.Transform(new Point(512.0, 848.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point3.X, point3.Y, PathPointFlags.MoveTo));
    Point point4 = identity.Transform(new Point(543.0, 848.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point4.X, point4.Y, PathPointFlags.BezierTo));
    Point point5 = identity.Transform(new Point(572.75, 844.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point5.X, point5.Y, PathPointFlags.BezierTo));
    Point point6 = identity.Transform(new Point(601.25, 836.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point6.X, point6.Y, PathPointFlags.BezierTo));
    Point point7 = identity.Transform(new Point(629.75, 828.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point7.X, point7.Y, PathPointFlags.BezierTo));
    Point point8 = identity.Transform(new Point(656.5, 816.667));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point8.X, point8.Y, PathPointFlags.BezierTo));
    Point point9 = identity.Transform(new Point(681.5, 802.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point9.X, point9.Y, PathPointFlags.BezierTo));
    Point point10 = identity.Transform(new Point(706.5, 787.333));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point10.X, point10.Y, PathPointFlags.BezierTo));
    Point point11 = identity.Transform(new Point(729.167, 769.833));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point11.X, point11.Y, PathPointFlags.BezierTo));
    Point point12 = identity.Transform(new Point(749.5, 749.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point12.X, point12.Y, PathPointFlags.BezierTo));
    Point point13 = identity.Transform(new Point(769.833, 729.167));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point13.X, point13.Y, PathPointFlags.BezierTo));
    Point point14 = identity.Transform(new Point(787.333, 706.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point14.X, point14.Y, PathPointFlags.BezierTo));
    Point point15 = identity.Transform(new Point(802.0, 681.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point15.X, point15.Y, PathPointFlags.BezierTo));
    Point point16 = identity.Transform(new Point(816.667, 656.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point16.X, point16.Y, PathPointFlags.BezierTo));
    Point point17 = identity.Transform(new Point(828.0, 629.75));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point17.X, point17.Y, PathPointFlags.BezierTo));
    Point point18 = identity.Transform(new Point(836.0, 601.25));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point18.X, point18.Y, PathPointFlags.BezierTo));
    Point point19 = identity.Transform(new Point(844.0, 572.75));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point19.X, point19.Y, PathPointFlags.BezierTo));
    Point point20 = identity.Transform(new Point(848.0, 543.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point20.X, point20.Y, PathPointFlags.BezierTo));
    Point point21 = identity.Transform(new Point(848.0, 512.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point21.X, point21.Y, PathPointFlags.BezierTo));
    Point point22 = identity.Transform(new Point(848.0, 481.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point22.X, point22.Y, PathPointFlags.BezierTo));
    Point point23 = identity.Transform(new Point(844.0, 451.25));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point23.X, point23.Y, PathPointFlags.BezierTo));
    Point point24 = identity.Transform(new Point(836.0, 422.75));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point24.X, point24.Y, PathPointFlags.BezierTo));
    Point point25 = identity.Transform(new Point(828.0, 394.25));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point25.X, point25.Y, PathPointFlags.BezierTo));
    Point point26 = identity.Transform(new Point(816.667, 367.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point26.X, point26.Y, PathPointFlags.BezierTo));
    Point point27 = identity.Transform(new Point(802.0, 342.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point27.X, point27.Y, PathPointFlags.BezierTo));
    Point point28 = identity.Transform(new Point(787.333, 317.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point28.X, point28.Y, PathPointFlags.BezierTo));
    Point point29 = identity.Transform(new Point(769.833, 294.833));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point29.X, point29.Y, PathPointFlags.BezierTo));
    Point point30 = identity.Transform(new Point(749.5, 274.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point30.X, point30.Y, PathPointFlags.BezierTo));
    Point point31 = identity.Transform(new Point(729.167, 254.167));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point31.X, point31.Y, PathPointFlags.BezierTo));
    Point point32 = identity.Transform(new Point(706.5, 236.667));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point32.X, point32.Y, PathPointFlags.BezierTo));
    Point point33 = identity.Transform(new Point(681.5, 222.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point33.X, point33.Y, PathPointFlags.BezierTo));
    Point point34 = identity.Transform(new Point(656.5, 207.333));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point34.X, point34.Y, PathPointFlags.BezierTo));
    Point point35 = identity.Transform(new Point(629.75, 196.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point35.X, point35.Y, PathPointFlags.BezierTo));
    Point point36 = identity.Transform(new Point(601.25, 188.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point36.X, point36.Y, PathPointFlags.BezierTo));
    Point point37 = identity.Transform(new Point(572.75, 180.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point37.X, point37.Y, PathPointFlags.BezierTo));
    Point point38 = identity.Transform(new Point(543.0, 176.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point38.X, point38.Y, PathPointFlags.BezierTo));
    Point point39 = identity.Transform(new Point(512.0, 176.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point39.X, point39.Y, PathPointFlags.BezierTo));
    Point point40 = identity.Transform(new Point(481.0, 176.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point40.X, point40.Y, PathPointFlags.BezierTo));
    Point point41 = identity.Transform(new Point(451.25, 180.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point41.X, point41.Y, PathPointFlags.BezierTo));
    Point point42 = identity.Transform(new Point(422.75, 188.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point42.X, point42.Y, PathPointFlags.BezierTo));
    Point point43 = identity.Transform(new Point(394.25, 196.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point43.X, point43.Y, PathPointFlags.BezierTo));
    Point point44 = identity.Transform(new Point(367.5, 207.333));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point44.X, point44.Y, PathPointFlags.BezierTo));
    Point point45 = identity.Transform(new Point(342.5, 222.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point45.X, point45.Y, PathPointFlags.BezierTo));
    Point point46 = identity.Transform(new Point(317.5, 236.667));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point46.X, point46.Y, PathPointFlags.BezierTo));
    Point point47 = identity.Transform(new Point(294.833, 254.167));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point47.X, point47.Y, PathPointFlags.BezierTo));
    Point point48 = identity.Transform(new Point(274.5, 274.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point48.X, point48.Y, PathPointFlags.BezierTo));
    Point point49 = identity.Transform(new Point(254.167, 294.833));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point49.X, point49.Y, PathPointFlags.BezierTo));
    Point point50 = identity.Transform(new Point(236.667, 317.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point50.X, point50.Y, PathPointFlags.BezierTo));
    Point point51 = identity.Transform(new Point(222.0, 342.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point51.X, point51.Y, PathPointFlags.BezierTo));
    Point point52 = identity.Transform(new Point(207.333, 367.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point52.X, point52.Y, PathPointFlags.BezierTo));
    Point point53 = identity.Transform(new Point(196.0, 394.25));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point53.X, point53.Y, PathPointFlags.BezierTo));
    Point point54 = identity.Transform(new Point(188.0, 422.75));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point54.X, point54.Y, PathPointFlags.BezierTo));
    Point point55 = identity.Transform(new Point(180.0, 451.25));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point55.X, point55.Y, PathPointFlags.BezierTo));
    Point point56 = identity.Transform(new Point(176.0, 481.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point56.X, point56.Y, PathPointFlags.BezierTo));
    Point point57 = identity.Transform(new Point(176.0, 512.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point57.X, point57.Y, PathPointFlags.BezierTo));
    Point point58 = identity.Transform(new Point(176.0, 543.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point58.X, point58.Y, PathPointFlags.BezierTo));
    Point point59 = identity.Transform(new Point(180.0, 572.75));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point59.X, point59.Y, PathPointFlags.BezierTo));
    Point point60 = identity.Transform(new Point(188.0, 601.25));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point60.X, point60.Y, PathPointFlags.BezierTo));
    Point point61 = identity.Transform(new Point(196.0, 629.75));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point61.X, point61.Y, PathPointFlags.BezierTo));
    Point point62 = identity.Transform(new Point(207.333, 656.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point62.X, point62.Y, PathPointFlags.BezierTo));
    Point point63 = identity.Transform(new Point(222.0, 681.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point63.X, point63.Y, PathPointFlags.BezierTo));
    Point point64 = identity.Transform(new Point(236.667, 706.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point64.X, point64.Y, PathPointFlags.BezierTo));
    Point point65 = identity.Transform(new Point(254.167, 729.167));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point65.X, point65.Y, PathPointFlags.BezierTo));
    Point point66 = identity.Transform(new Point(274.5, 749.5));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point66.X, point66.Y, PathPointFlags.BezierTo));
    Point point67 = identity.Transform(new Point(294.833, 769.833));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point67.X, point67.Y, PathPointFlags.BezierTo));
    Point point68 = identity.Transform(new Point(317.5, 787.333));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point68.X, point68.Y, PathPointFlags.BezierTo));
    Point point69 = identity.Transform(new Point(342.5, 802.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point69.X, point69.Y, PathPointFlags.BezierTo));
    Point point70 = identity.Transform(new Point(367.5, 816.667));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point70.X, point70.Y, PathPointFlags.BezierTo));
    Point point71 = identity.Transform(new Point(394.25, 828.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point71.X, point71.Y, PathPointFlags.BezierTo));
    Point point72 = identity.Transform(new Point(422.75, 836.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point72.X, point72.Y, PathPointFlags.BezierTo));
    Point point73 = identity.Transform(new Point(451.25, 844.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point73.X, point73.Y, PathPointFlags.BezierTo));
    Point point74 = identity.Transform(new Point(481.0, 848.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point74.X, point74.Y, PathPointFlags.BezierTo));
    Point point75 = identity.Transform(new Point(512.0, 848.0));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(point75.X, point75.Y, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    geometryPath.Add((PdfPageObject) pdfPathObject2);
    return geometryPath;
  }
}
