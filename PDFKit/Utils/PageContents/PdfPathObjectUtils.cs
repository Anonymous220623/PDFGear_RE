// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PageContents.PdfPathObjectUtils
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Utils.PageContents;

public static class PdfPathObjectUtils
{
  public static System.Collections.Generic.IReadOnlyList<PdfPathObject> CreateFromVisual(
    Visual visual,
    FS_POINTF leftBottomPoint,
    double dpi = 96.0,
    bool includeInvisiblePath = false)
  {
    if (visual == null)
      return (System.Collections.Generic.IReadOnlyList<PdfPathObject>) Array.Empty<PdfPathObject>();
    System.Collections.Generic.IReadOnlyList<PdfPathObjectUtils.GeometryData> geometries = PdfPathObjectUtils.GetGeometries(visual);
    Rect empty = Rect.Empty;
    foreach (PdfPathObjectUtils.GeometryData geometryData in (IEnumerable<PdfPathObjectUtils.GeometryData>) geometries)
      empty.Union(geometryData.Geometry.Bounds);
    FS_RECTF _viewbox2 = new FS_RECTF((double) leftBottomPoint.X, (double) leftBottomPoint.Y + empty.Height / dpi * 72.0, (double) leftBottomPoint.X + empty.Width / dpi * 72.0, (double) leftBottomPoint.Y);
    List<PdfPathObject> fromVisual = new List<PdfPathObject>();
    foreach (PdfPathObjectUtils.GeometryData geometryData in (IEnumerable<PdfPathObjectUtils.GeometryData>) geometries)
    {
      FS_COLOR? pdfFillColor = geometryData.PdfFillColor;
      FS_COLOR? pdfStrokeColor = geometryData.PdfStrokeColor;
      if (((pdfStrokeColor.HasValue ? 1 : (pdfFillColor.HasValue ? 1 : 0)) | (includeInvisiblePath ? 1 : 0)) != 0)
      {
        FillModes fillMode = FillModes.None;
        if (pdfFillColor.HasValue)
          fillMode |= geometryData.Geometry.FillRule == FillRule.Nonzero ? FillModes.Winding : FillModes.Alternate;
        PdfPathObject pdfPathObject = PdfPathObject.Create(fillMode, pdfStrokeColor.HasValue);
        if (pdfFillColor.HasValue)
          pdfPathObject.FillColor = pdfFillColor.Value;
        if (pdfStrokeColor.HasValue)
        {
          pdfPathObject.LineWidth = (float) (geometryData.Pen.Thickness / dpi * 72.0);
          pdfPathObject.StrokeColor = pdfStrokeColor.Value;
          if (geometryData.Pen.DashStyle?.Dashes != null && geometryData.Pen.DashStyle.Dashes.Count > 0)
          {
            pdfPathObject.SetDashArray(geometryData.Pen.DashStyle.Dashes.Cast<float>().ToArray<float>());
            pdfPathObject.DashPhase = (float) (geometryData.Pen.DashStyle.Offset / dpi * 72.0);
          }
          switch (geometryData.Pen.LineJoin)
          {
            case PenLineJoin.Miter:
              pdfPathObject.LineJoin = LineJoin.Miter;
              break;
            case PenLineJoin.Bevel:
              pdfPathObject.LineJoin = LineJoin.Bevel;
              break;
            case PenLineJoin.Round:
              pdfPathObject.LineJoin = LineJoin.Round;
              break;
          }
          switch (geometryData.Pen.StartLineCap)
          {
            case PenLineCap.Flat:
              pdfPathObject.LineCap = LineCap.Butt;
              break;
            case PenLineCap.Square:
              pdfPathObject.LineCap = LineCap.Square;
              break;
            case PenLineCap.Round:
              pdfPathObject.LineCap = LineCap.Round;
              break;
            case PenLineCap.Triangle:
              pdfPathObject.LineCap = LineCap.Square;
              break;
          }
          pdfPathObject.MiterLimit = (float) (geometryData.Pen.MiterLimit / dpi * 72.0);
        }
        else
          pdfPathObject.LineWidth = 0.0f;
        foreach (PathFigure figure in geometryData.Geometry.Figures)
        {
          FS_POINTF point = ToPoint(figure.StartPoint, empty, _viewbox2, dpi);
          pdfPathObject.Path.Add(new FS_PATHPOINTF(point.X, point.Y, PathPointFlags.MoveTo));
          foreach (PathSegment segment in figure.Segments)
          {
            foreach (FS_POINTF fsPointf in FlattenCore(segment, empty, _viewbox2))
              pdfPathObject.Path.Add(new FS_PATHPOINTF(fsPointf.X, fsPointf.Y, PathPointFlags.LineTo));
          }
          if (pdfPathObject.Path.Count > 0)
          {
            FS_PATHPOINTF fsPathpointf = pdfPathObject.Path[pdfPathObject.Path.Count - 1];
            fsPathpointf.Flags |= PathPointFlags.CloseFigure;
            pdfPathObject.Path[pdfPathObject.Path.Count - 1] = fsPathpointf;
          }
        }
        Pdfium.FPDFPathObj_CalcBoundingBox(pdfPathObject.Handle);
        fromVisual.Add(pdfPathObject);
      }
    }
    return (System.Collections.Generic.IReadOnlyList<PdfPathObject>) fromVisual;

    static FS_POINTF ToPoint(Point point, Rect _viewbox, FS_RECTF _viewbox2, double _dpi)
    {
      return new FS_POINTF((float) ((point.X - _viewbox.Left) / _dpi * 72.0) + _viewbox2.left, (float) ((_viewbox.Bottom - point.Y) / _dpi * 72.0) + _viewbox2.bottom);
    }

    IEnumerable<FS_POINTF> FlattenCore(PathSegment _segment, Rect _viewbox, FS_RECTF _viewbox2)
    {
      PathSegment pathSegment = _segment;
      switch (pathSegment)
      {
        case LineSegment lineSegment:
          yield return ToPoint(lineSegment.Point, _viewbox, _viewbox2, dpi);
          break;
        case PolyLineSegment polyLineSegment:
          foreach (Point point in polyLineSegment.Points)
          {
            Point sp = point;
            yield return ToPoint(sp, _viewbox, _viewbox2, dpi);
            sp = new Point();
          }
          break;
      }
      lineSegment = (LineSegment) null;
      polyLineSegment = (PolyLineSegment) null;
      pathSegment = (PathSegment) null;
    }
  }

  public static System.Collections.Generic.IReadOnlyList<PdfPathObjectUtils.GeometryData> GetGeometries(
    Visual visual)
  {
    List<PdfPathObjectUtils.GeometryData> _list = new List<PdfPathObjectUtils.GeometryData>();
    TransformGroup transform = new TransformGroup();
    GetGeometriesCore(visual, transform, 1.0, _list);
    return (System.Collections.Generic.IReadOnlyList<PdfPathObjectUtils.GeometryData>) _list;

    void GetGeometriesCore(
      Visual _visual,
      TransformGroup _transformGroup,
      double _opacity,
      List<PdfPathObjectUtils.GeometryData> _list)
    {
      if (_visual == null || _opacity == 0.0)
        return;
      double _opacity1 = _opacity;
      int num = 0;
      if (_visual is ContainerVisual containerVisual)
      {
        _opacity1 *= containerVisual.Opacity;
        if (containerVisual.Transform != null)
        {
          ++num;
          _transformGroup.Children.Add(containerVisual.Transform);
        }
        if (containerVisual.Offset.X != 0.0 || containerVisual.Offset.Y != 0.0)
        {
          ++num;
          TranslateTransform translateTransform = new TranslateTransform(containerVisual.Offset.X, containerVisual.Offset.Y);
          translateTransform.Freeze();
          _transformGroup.Children.Add((Transform) translateTransform);
        }
      }
      if (_opacity1 == 0.0)
        return;
      GetDrawingGeometriesCore((Drawing) VisualTreeHelper.GetDrawing(_visual), _transformGroup, _opacity1, _list);
      for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount((DependencyObject) _visual); ++childIndex)
      {
        if (VisualTreeHelper.GetChild((DependencyObject) _visual, childIndex) is Visual child)
          GetGeometriesCore(child, _transformGroup, _opacity1, _list);
      }
      while (num > 0)
      {
        --num;
        _transformGroup.Children.RemoveAt(_transformGroup.Children.Count - 1);
      }
    }

    void GetDrawingGeometriesCore(
      Drawing _drawing,
      TransformGroup _transformGroup,
      double _opacity,
      List<PdfPathObjectUtils.GeometryData> _list)
    {
      if (_opacity == 0.0)
        return;
      switch (_drawing)
      {
        case GeometryDrawing geometryDrawing:
          PathGeometry pathGeometry1 = ConvertToPathGeometry(geometryDrawing.Geometry, (Transform) transform);
          if (pathGeometry1 == null)
            break;
          _list.Add(new PdfPathObjectUtils.GeometryData(pathGeometry1, geometryDrawing.Brush, geometryDrawing.Pen, _opacity));
          break;
        case GlyphRunDrawing glyphRunDrawing:
          PathGeometry pathGeometry2 = ConvertToPathGeometry(glyphRunDrawing.GlyphRun.BuildGeometry(), (Transform) transform);
          if (pathGeometry2 == null)
            break;
          _list.Add(new PdfPathObjectUtils.GeometryData(pathGeometry2, glyphRunDrawing.ForegroundBrush, (Pen) null, _opacity));
          break;
        case DrawingGroup drawingGroup:
          if (drawingGroup.Children != null && drawingGroup.Children.Count > 0)
          {
            int num = 0;
            double _opacity1 = _opacity * drawingGroup.Opacity;
            if (drawingGroup.Transform != null)
            {
              ++num;
              _transformGroup.Children.Add(drawingGroup.Transform);
            }
            foreach (Drawing child in drawingGroup.Children)
              GetDrawingGeometriesCore(child, _transformGroup, _opacity1, _list);
            while (num > 0)
            {
              --num;
              _transformGroup.Children.RemoveAt(_transformGroup.Children.Count - 1);
            }
          }
          break;
      }
    }

    static PathGeometry ConvertToPathGeometry(Geometry _geometry, Transform _transform)
    {
      if (_geometry == null || _geometry.IsEmpty())
        return (PathGeometry) null;
      PathGeometry pathGeometry = Geometry.Combine(Geometry.Empty, _geometry, GeometryCombineMode.Union, _transform, 0.01, ToleranceType.Absolute);
      pathGeometry?.Freeze();
      if (pathGeometry != null && pathGeometry.MayHaveCurves())
        pathGeometry = pathGeometry.GetFlattenedPathGeometry(0.01, ToleranceType.Absolute);
      return pathGeometry;
    }
  }

  public class GeometryData
  {
    public GeometryData(PathGeometry geometry, Brush brush, Pen pen, double opacity)
    {
      this.Geometry = geometry;
      this.Brush = brush;
      this.Pen = pen;
      this.Opacity = opacity;
    }

    public PathGeometry Geometry { get; }

    public Brush Brush { get; }

    public Pen Pen { get; }

    public double Opacity { get; }

    public Color? FillColor
    {
      get
      {
        return this.Opacity <= 0.0 || !(this.Brush is SolidColorBrush brush) || brush.Opacity <= 0.0 || brush.Color.A <= (byte) 0 ? new Color?() : new Color?(PdfPathObjectUtils.GeometryData.ColorWithOpacity(brush.Color, brush.Opacity * this.Opacity));
      }
    }

    public Color? StrokeColor
    {
      get
      {
        return this.Opacity <= 0.0 || this.Pen == null || this.Pen.Thickness <= 0.0 || !(this.Pen.Brush is SolidColorBrush brush) || brush.Opacity <= 0.0 || brush.Color.A <= (byte) 0 ? new Color?() : new Color?(PdfPathObjectUtils.GeometryData.ColorWithOpacity(brush.Color, brush.Opacity * this.Opacity));
      }
    }

    public FS_COLOR? PdfFillColor
    {
      get
      {
        Color? fillColor = this.FillColor;
        if (!fillColor.HasValue)
          return new FS_COLOR?();
        Color color = fillColor.Value;
        int a = (int) color.A;
        color = fillColor.Value;
        int r = (int) color.R;
        color = fillColor.Value;
        int g = (int) color.G;
        color = fillColor.Value;
        int b = (int) color.B;
        return new FS_COLOR?(new FS_COLOR(a, r, g, b));
      }
    }

    public FS_COLOR? PdfStrokeColor
    {
      get
      {
        Color? strokeColor = this.StrokeColor;
        if (!strokeColor.HasValue)
          return new FS_COLOR?();
        Color color = strokeColor.Value;
        int a = (int) color.A;
        color = strokeColor.Value;
        int r = (int) color.R;
        color = strokeColor.Value;
        int g = (int) color.G;
        color = strokeColor.Value;
        int b = (int) color.B;
        return new FS_COLOR?(new FS_COLOR(a, r, g, b));
      }
    }

    private static Color ColorWithOpacity(Color color, double opacity)
    {
      return Color.FromArgb((byte) Math.Min((double) byte.MaxValue, (double) color.A * opacity), color.R, color.G, color.B);
    }
  }
}
