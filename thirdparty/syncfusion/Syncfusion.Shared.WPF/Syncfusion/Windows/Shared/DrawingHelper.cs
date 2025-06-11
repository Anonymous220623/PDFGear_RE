// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DrawingHelper
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class DrawingHelper : ContentControl
{
  private readonly Hashtable m_drawingsHash = new Hashtable();
  private Visual m_topMostVisual;
  private int m_convertionChainCount;
  public static readonly DependencyProperty DrawingBrushProperty = DependencyProperty.Register(nameof (DrawingBrush), typeof (DrawingBrush), typeof (DrawingHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(DrawingHelper.OnDrawingBrushChanged)));

  private static void OnDrawingBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
  }

  private List<Point> GetPointsFromDrawing(Drawing drawing)
  {
    List<Point> pointsFromDrawing = new List<Point>();
    switch (drawing)
    {
      case DrawingGroup _:
        using (DrawingCollection.Enumerator enumerator = (drawing as DrawingGroup).Children.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Drawing current = enumerator.Current;
            pointsFromDrawing.AddRange((IEnumerable<Point>) this.GetPointsFromDrawing(current));
          }
          break;
        }
      case GeometryDrawing _:
        pointsFromDrawing.AddRange((IEnumerable<Point>) this.GetPointsFromGeometryDrawing(drawing));
        break;
    }
    return pointsFromDrawing;
  }

  private List<Point> GetPointsFromGeometryDrawing(Drawing drawing)
  {
    List<Point> pointsFromGeometry = DrawingHelper.GetPointsFromGeometry(((GeometryDrawing) drawing).Geometry);
    this.m_drawingsHash[(object) drawing] = (object) pointsFromGeometry;
    return pointsFromGeometry;
  }

  private static List<Point> GetPointsFromGeometry(Geometry geometry)
  {
    List<Point> pointsFromGeometry = new List<Point>();
    switch (geometry)
    {
      case CombinedGeometry _:
        CombinedGeometry combinedGeometry = geometry as CombinedGeometry;
        pointsFromGeometry.AddRange((IEnumerable<Point>) DrawingHelper.GetPointsFromGeometry(combinedGeometry.Geometry1));
        pointsFromGeometry.AddRange((IEnumerable<Point>) DrawingHelper.GetPointsFromGeometry(combinedGeometry.Geometry2));
        break;
      case GeometryGroup _:
        using (GeometryCollection.Enumerator enumerator = (geometry as GeometryGroup).Children.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Geometry current = enumerator.Current;
            pointsFromGeometry.AddRange((IEnumerable<Point>) DrawingHelper.GetPointsFromGeometry(current));
          }
          break;
        }
      case LineGeometry _:
        LineGeometry lineGeometry = geometry as LineGeometry;
        pointsFromGeometry.Add(lineGeometry.StartPoint);
        pointsFromGeometry.Add(lineGeometry.EndPoint);
        break;
      case RectangleGeometry _:
        RectangleGeometry rectangleGeometry = geometry as RectangleGeometry;
        pointsFromGeometry.Add(rectangleGeometry.Rect.TopLeft);
        pointsFromGeometry.Add(rectangleGeometry.Rect.TopRight);
        pointsFromGeometry.Add(rectangleGeometry.Rect.BottomRight);
        pointsFromGeometry.Add(rectangleGeometry.Rect.BottomLeft);
        break;
      case null:
        if (geometry is EllipseGeometry)
        {
          EllipseGeometry ellipseGeometry = geometry as EllipseGeometry;
          pointsFromGeometry.Add(ellipseGeometry.Center);
          break;
        }
        break;
      default:
        using (PathFigureCollection.Enumerator enumerator = geometry.GetFlattenedPathGeometry().Figures.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            PathFigure current = enumerator.Current;
            pointsFromGeometry.Add(current.StartPoint);
            foreach (PathSegment segment in current.Segments)
            {
              switch (segment)
              {
                case LineSegment _:
                  LineSegment lineSegment = segment as LineSegment;
                  pointsFromGeometry.Add(lineSegment.Point);
                  continue;
                case ArcSegment _:
                  ArcSegment arcSegment = segment as ArcSegment;
                  pointsFromGeometry.Add(arcSegment.Point);
                  continue;
                case BezierSegment _:
                  BezierSegment bezierSegment = segment as BezierSegment;
                  pointsFromGeometry.AddRange((IEnumerable<Point>) new Point[3]
                  {
                    bezierSegment.Point1,
                    bezierSegment.Point2,
                    bezierSegment.Point3
                  });
                  continue;
                case PolyLineSegment _:
                  PolyLineSegment polyLineSegment = segment as PolyLineSegment;
                  pointsFromGeometry.AddRange((IEnumerable<Point>) polyLineSegment.Points);
                  continue;
                case PolyBezierSegment _:
                  PolyBezierSegment polyBezierSegment = segment as PolyBezierSegment;
                  pointsFromGeometry.AddRange((IEnumerable<Point>) polyBezierSegment.Points);
                  continue;
                case QuadraticBezierSegment _:
                  QuadraticBezierSegment quadraticBezierSegment1 = segment as QuadraticBezierSegment;
                  pointsFromGeometry.AddRange((IEnumerable<Point>) new Point[2]
                  {
                    quadraticBezierSegment1.Point1,
                    quadraticBezierSegment1.Point2
                  });
                  continue;
                case PolyQuadraticBezierSegment _:
                  PolyQuadraticBezierSegment quadraticBezierSegment2 = segment as PolyQuadraticBezierSegment;
                  pointsFromGeometry.AddRange((IEnumerable<Point>) quadraticBezierSegment2.Points);
                  continue;
                default:
                  continue;
              }
            }
          }
          goto case null;
        }
    }
    return pointsFromGeometry;
  }

  private GuidelineSet GetGuidelineSetForDrawing(Drawing drawing)
  {
    this.m_drawingsHash.Clear();
    this.GetPointsFromDrawing(drawing);
    GuidelineSet guidelineSetForDrawing = new GuidelineSet();
    foreach (Drawing key in (IEnumerable) this.m_drawingsHash.Keys)
    {
      List<Point> pointList = this.m_drawingsHash[(object) key] as List<Point>;
      double num = 1.0;
      if (key is GeometryDrawing)
      {
        GeometryDrawing geometryDrawing = key as GeometryDrawing;
        if (geometryDrawing.Pen != null)
          num = geometryDrawing.Pen.Thickness;
      }
      int index = 0;
      for (int count = pointList.Count; index < count; ++index)
      {
        Point point = pointList[index];
        guidelineSetForDrawing.GuidelinesX.Add(point.X - num / 2.0);
        guidelineSetForDrawing.GuidelinesX.Add(point.X + num / 2.0);
        guidelineSetForDrawing.GuidelinesY.Add(point.Y - num / 2.0);
        guidelineSetForDrawing.GuidelinesY.Add(point.Y + num / 2.0);
      }
    }
    return guidelineSetForDrawing;
  }

  internal static Drawing ApplyTransformToDrawing(
    Drawing drawing,
    GeneralTransform generalTransform)
  {
    if (generalTransform is Transform)
    {
      Transform transform = generalTransform as Transform;
      switch (drawing)
      {
        case DrawingGroup _:
          (drawing as DrawingGroup).Transform = transform;
          break;
        case GeometryDrawing _:
          (drawing as GeometryDrawing).Geometry.Transform = transform;
          break;
        case GlyphRunDrawing _:
          (drawing as GlyphRunDrawing).GlyphRun.BuildGeometry().Transform = transform;
          break;
        case ImageDrawing _:
          ImageDrawing imageDrawing = drawing as ImageDrawing;
          imageDrawing.Rect = transform.TransformBounds(imageDrawing.Rect);
          break;
        case VideoDrawing _:
          VideoDrawing videoDrawing = drawing as VideoDrawing;
          videoDrawing.Rect = transform.TransformBounds(videoDrawing.Rect);
          break;
      }
    }
    return drawing;
  }

  private Drawing GetDrawing(object obj)
  {
    Drawing drawing = (Drawing) null;
    switch (obj)
    {
      case Visual _:
        drawing = this.ConvertVisualToDrawing(obj as Visual);
        break;
      case DrawingBrush _:
        drawing = DrawingHelper.ConvertDrawingBrushToDrawing(obj as DrawingBrush);
        break;
    }
    return drawing;
  }

  private static Drawing ConvertDrawingBrushToDrawing(DrawingBrush brush)
  {
    if (brush != null)
    {
      Drawing drawing = brush.Drawing;
      if (drawing != null)
        return drawing;
    }
    return (Drawing) null;
  }

  private Drawing ConvertVisualToDrawing(Visual visual)
  {
    DrawingGroup drawing1 = (DrawingGroup) null;
    if (visual != null)
    {
      if (this.m_topMostVisual == null || this.m_convertionChainCount == 0)
        this.m_topMostVisual = visual;
      ++this.m_convertionChainCount;
      drawing1 = VisualTreeHelper.GetDrawing(visual);
      if (drawing1 != null)
      {
        IEnumerable children = LogicalTreeHelper.GetChildren((DependencyObject) visual);
        if (children != null)
        {
          foreach (object obj in children)
          {
            if (obj is Visual visual2)
            {
              Drawing drawing2 = this.ConvertVisualToDrawing(visual2);
              if (drawing2 != null)
              {
                GeneralTransform visual1 = visual2.TransformToVisual(this.m_topMostVisual);
                Drawing drawing3 = DrawingHelper.ApplyTransformToDrawing(drawing2, visual1);
                drawing1.Children.Add(drawing3);
              }
            }
          }
        }
      }
      --this.m_convertionChainCount;
    }
    return (Drawing) drawing1;
  }

  public DrawingBrush DrawingBrush
  {
    get => (DrawingBrush) this.GetValue(DrawingHelper.DrawingBrushProperty);
    set => this.SetValue(DrawingHelper.DrawingBrushProperty, (object) value);
  }

  protected override void OnRender(DrawingContext drawingContext)
  {
    Drawing drawing = this.GetDrawing(this.Content ?? (object) this.DrawingBrush);
    if (drawing != null)
    {
      GuidelineSet guidelineSetForDrawing = this.GetGuidelineSetForDrawing(drawing);
      drawingContext.PushGuidelineSet(guidelineSetForDrawing);
    }
    base.OnRender(drawingContext);
    if (drawing == null)
      return;
    if (this.Content == null)
      drawingContext.DrawDrawing(drawing);
    drawingContext.Pop();
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    if (this.VisualChildrenCount > 0)
    {
      UIElement visualChild = (UIElement) this.GetVisualChild(0);
      if (visualChild != null)
      {
        visualChild.Measure(availableSize);
        return visualChild.DesiredSize;
      }
    }
    return new Size(0.0, 0.0);
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    IEnumerable children = LogicalTreeHelper.GetChildren((FrameworkElement) this);
    if (children != null)
    {
      foreach (object obj in children)
      {
        if (obj is UIElement reference)
        {
          Vector offset = VisualTreeHelper.GetOffset((Visual) reference);
          reference.Arrange(new Rect(new Point(offset.X, offset.Y), reference.DesiredSize));
        }
      }
    }
    return finalSize;
  }
}
