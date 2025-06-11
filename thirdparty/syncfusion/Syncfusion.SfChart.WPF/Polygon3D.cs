// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.Polygon3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class Polygon3D
{
  public const double Epsilon = 1E-05;
  protected internal Vector3D[] VectorPoints;
  protected double d;
  protected Vector3D normal;
  private readonly double strokeThickness;

  public Polygon3D()
  {
  }

  public Polygon3D(Vector3D v1, Vector3D v2, Vector3D v3) => this.CalcNormal(v1, v2, v3);

  public Polygon3D(Vector3D[] points) => this.VectorPoints = points;

  public Polygon3D(Vector3D[] points, int index)
    : this(points)
  {
    this.Index = index;
  }

  public Polygon3D(Vector3D normal, double d)
  {
    this.normal = normal;
    this.d = d;
  }

  public Polygon3D(
    Vector3D[] points,
    DependencyObject tag,
    int index,
    Brush stroke,
    double strokeThickness,
    Brush fill,
    string name)
    : this(points)
  {
    this.Element = (UIElement) new Path();
    this.Index = index;
    this.Tag = tag;
    this.Name = name;
    this.Stroke = stroke;
    this.strokeThickness = strokeThickness;
    this.Fill = fill;
  }

  public Polygon3D(
    Vector3D[] points,
    DependencyObject tag,
    int index,
    Brush stroke,
    double strokeThickness,
    Brush fill)
    : this(points)
  {
    this.Element = (UIElement) new Path();
    this.Index = index;
    this.Tag = tag;
    this.Stroke = stroke;
    this.strokeThickness = strokeThickness;
    this.Fill = fill;
  }

  public Polygon3D(Vector3D[] points, Polygon3D polygon)
    : this(points)
  {
    polygon.Element = (UIElement) null;
    this.Element = (UIElement) new Path();
    this.Index = polygon.Index;
    this.Stroke = polygon.Stroke;
    this.Tag = polygon.Tag;
    this.Graphics3D = polygon.Graphics3D;
    this.Fill = polygon.Fill;
    this.strokeThickness = polygon.strokeThickness;
  }

  public UIElement Element { get; set; }

  public Vector3D Normal => this.normal;

  public double A => this.normal.X;

  public double B => this.normal.Y;

  public double C => this.normal.Z;

  public double D => this.d;

  public virtual Vector3D[] Points => this.VectorPoints;

  internal int Index { get; set; }

  internal DependencyObject Tag { get; set; }

  internal Brush Stroke { get; set; }

  internal string Name { get; set; }

  internal Brush Fill { get; set; }

  internal Graphics3D Graphics3D { get; set; }

  public bool Test() => !this.normal.IsValid;

  public Vector3D GetPoint(double x, double y)
  {
    double vz = -(this.A * x + this.B * y + this.D) / this.C;
    return new Vector3D(x, y, vz);
  }

  public Vector3D GetPoint(Vector3D position, Vector3D ray)
  {
    double num = (this.normal * -this.d - position & this.normal) / (this.normal & ray);
    return position + ray * num;
  }

  public virtual void Transform(Matrix3D matrix)
  {
    if (this.Points != null)
    {
      for (int index = 0; index < this.Points.Length; ++index)
        this.Points[index] = matrix * this.Points[index];
      this.CalcNormal();
    }
    else
    {
      Vector3D vector3D = matrix * (this.normal * -this.d);
      this.normal = matrix & this.normal;
      this.normal.Normalize();
      this.d = -(this.normal & vector3D);
    }
  }

  internal static UIElement3D CreateUIElement(
    Vector3D position,
    UIElement element,
    double xLen,
    double yLen,
    bool isFront,
    UIElementLeftShift leftShiftType,
    UIElementTopShift topShiftType)
  {
    Panel.SetZIndex(element, 0);
    Vector3D[] points = new Vector3D[3];
    double x = position.X;
    double y = position.Y;
    double z = position.Z;
    double width = element.DesiredSize.Width;
    double height = element.DesiredSize.Height;
    if (isFront)
    {
      points[0] = new Vector3D(x, y, position.Z);
      points[1] = new Vector3D(x + width, y + height + yLen, position.Z);
      points[2] = new Vector3D(x + width + xLen, y + height + yLen, position.Z);
    }
    else
    {
      points[0] = new Vector3D(x, y, z);
      points[1] = new Vector3D(x, y + height + yLen, z + width);
      points[2] = new Vector3D(x, y + height + yLen, z + width + xLen);
    }
    return new UIElement3D(element, points)
    {
      LeftShift = leftShiftType,
      TopShift = topShiftType
    };
  }

  internal static PolyLine3D CreatePolyline(List<Vector3D> points, Path element, bool isFront)
  {
    if (points.Count == 2)
    {
      Vector3D point = points[1];
      if (isFront)
        points.Add(new Vector3D(point.X + 0.01, point.Y + 0.01, point.Z));
      else
        points.Add(new Vector3D(point.X, point.Y + 0.01, point.Z + 0.01));
    }
    return new PolyLine3D(element, points);
  }

  internal static Polygon3D[] CreateBox(
    Vector3D vector1,
    Vector3D vector2,
    DependencyObject tag,
    int index,
    Graphics3D graphics3D,
    Brush stroke,
    Brush fill,
    double strokeThickness,
    bool inverse,
    string name)
  {
    Polygon3D[] box = new Polygon3D[6];
    Vector3D[] points1 = new Vector3D[4]
    {
      new Vector3D(vector1.X, vector1.Y, vector1.Z),
      new Vector3D(vector2.X, vector1.Y, vector1.Z),
      new Vector3D(vector2.X, vector2.Y, vector1.Z),
      new Vector3D(vector1.X, vector2.Y, vector1.Z)
    };
    Vector3D[] points2 = new Vector3D[4]
    {
      new Vector3D(vector1.X, vector1.Y, vector2.Z),
      new Vector3D(vector2.X, vector1.Y, vector2.Z),
      new Vector3D(vector2.X, vector2.Y, vector2.Z),
      new Vector3D(vector1.X, vector2.Y, vector2.Z)
    };
    Vector3D[] points3 = new Vector3D[4]
    {
      new Vector3D(vector1.X, vector1.Y, vector2.Z),
      new Vector3D(vector2.X, vector1.Y, vector2.Z),
      new Vector3D(vector2.X, vector1.Y, vector1.Z),
      new Vector3D(vector1.X, vector1.Y, vector1.Z)
    };
    Vector3D[] points4 = new Vector3D[4]
    {
      new Vector3D(vector1.X, vector2.Y, vector2.Z),
      new Vector3D(vector2.X, vector2.Y, vector2.Z),
      new Vector3D(vector2.X, vector2.Y, vector1.Z),
      new Vector3D(vector1.X, vector2.Y, vector1.Z)
    };
    Vector3D[] points5 = new Vector3D[4]
    {
      new Vector3D(vector1.X, vector1.Y, vector1.Z),
      new Vector3D(vector1.X, vector1.Y, vector2.Z),
      new Vector3D(vector1.X, vector2.Y, vector2.Z),
      new Vector3D(vector1.X, vector2.Y, vector1.Z)
    };
    Vector3D[] points6 = new Vector3D[4]
    {
      new Vector3D(vector2.X, vector1.Y, vector1.Z),
      new Vector3D(vector2.X, vector1.Y, vector2.Z),
      new Vector3D(vector2.X, vector2.Y, vector2.Z),
      new Vector3D(vector2.X, vector2.Y, vector1.Z)
    };
    box[0] = new Polygon3D(points1, tag, index, stroke, strokeThickness, fill, "0" + name);
    box[0].CalcNormal(points1[0], points1[1], points1[2]);
    box[0].CalcNormal();
    box[1] = new Polygon3D(points2, tag, index, stroke, strokeThickness, fill, "1" + name);
    box[1].CalcNormal(points2[0], points2[1], points2[2]);
    box[1].CalcNormal();
    box[2] = new Polygon3D(points3, tag, index, stroke, strokeThickness, fill, "2" + name);
    box[2].CalcNormal(points3[0], points3[1], points3[2]);
    box[2].CalcNormal();
    box[3] = new Polygon3D(points4, tag, index, stroke, strokeThickness, fill, "3" + name);
    box[3].CalcNormal(points4[0], points4[1], points4[2]);
    box[3].CalcNormal();
    box[4] = new Polygon3D(points5, tag, index, stroke, strokeThickness, fill, "4" + name);
    box[4].CalcNormal(points5[0], points5[1], points5[2]);
    box[4].CalcNormal();
    box[5] = new Polygon3D(points6, tag, index, stroke, strokeThickness, fill, "5" + name);
    box[5].CalcNormal(points6[0], points6[1], points6[2]);
    box[5].CalcNormal();
    if (inverse)
    {
      graphics3D.AddVisual(box[0]);
      graphics3D.AddVisual(box[1]);
      graphics3D.AddVisual(box[2]);
      graphics3D.AddVisual(box[3]);
      graphics3D.AddVisual(box[4]);
      graphics3D.AddVisual(box[5]);
    }
    else
    {
      graphics3D.AddVisual(box[5]);
      graphics3D.AddVisual(box[4]);
      graphics3D.AddVisual(box[0]);
      graphics3D.AddVisual(box[1]);
      graphics3D.AddVisual(box[2]);
      graphics3D.AddVisual(box[3]);
    }
    return box;
  }

  internal static Polygon3D[] CreateBox(
    Vector3D vector1,
    Vector3D vector2,
    DependencyObject tag,
    int index,
    Graphics3D graphics3D,
    Brush stroke,
    Brush fill,
    double strokeThickness,
    bool inverse)
  {
    Polygon3D[] box = new Polygon3D[6];
    Vector3D[] points1 = new Vector3D[4]
    {
      new Vector3D(vector1.X, vector1.Y, vector1.Z),
      new Vector3D(vector2.X, vector1.Y, vector1.Z),
      new Vector3D(vector2.X, vector2.Y, vector1.Z),
      new Vector3D(vector1.X, vector2.Y, vector1.Z)
    };
    Vector3D[] points2 = new Vector3D[4]
    {
      new Vector3D(vector1.X, vector1.Y, vector2.Z),
      new Vector3D(vector2.X, vector1.Y, vector2.Z),
      new Vector3D(vector2.X, vector2.Y, vector2.Z),
      new Vector3D(vector1.X, vector2.Y, vector2.Z)
    };
    Vector3D[] points3 = new Vector3D[4]
    {
      new Vector3D(vector1.X, vector1.Y, vector2.Z),
      new Vector3D(vector2.X, vector1.Y, vector2.Z),
      new Vector3D(vector2.X, vector1.Y, vector1.Z),
      new Vector3D(vector1.X, vector1.Y, vector1.Z)
    };
    Vector3D[] points4 = new Vector3D[4]
    {
      new Vector3D(vector1.X, vector2.Y, vector2.Z),
      new Vector3D(vector2.X, vector2.Y, vector2.Z),
      new Vector3D(vector2.X, vector2.Y, vector1.Z),
      new Vector3D(vector1.X, vector2.Y, vector1.Z)
    };
    Vector3D[] points5 = new Vector3D[4]
    {
      new Vector3D(vector1.X, vector1.Y, vector1.Z),
      new Vector3D(vector1.X, vector1.Y, vector2.Z),
      new Vector3D(vector1.X, vector2.Y, vector2.Z),
      new Vector3D(vector1.X, vector2.Y, vector1.Z)
    };
    Vector3D[] points6 = new Vector3D[4]
    {
      new Vector3D(vector2.X, vector1.Y, vector1.Z),
      new Vector3D(vector2.X, vector1.Y, vector2.Z),
      new Vector3D(vector2.X, vector2.Y, vector2.Z),
      new Vector3D(vector2.X, vector2.Y, vector1.Z)
    };
    box[0] = new Polygon3D(points1, tag, index, stroke, strokeThickness, fill);
    box[0].CalcNormal(points1[0], points1[1], points1[2]);
    box[0].CalcNormal();
    box[1] = new Polygon3D(points2, tag, index, stroke, strokeThickness, fill);
    box[1].CalcNormal(points2[0], points2[1], points2[2]);
    box[1].CalcNormal();
    box[2] = new Polygon3D(points3, tag, index, stroke, strokeThickness, fill);
    box[2].CalcNormal(points3[0], points3[1], points3[2]);
    box[2].CalcNormal();
    box[3] = new Polygon3D(points4, tag, index, stroke, strokeThickness, fill);
    box[3].CalcNormal(points4[0], points4[1], points4[2]);
    box[3].CalcNormal();
    box[4] = new Polygon3D(points5, tag, index, stroke, strokeThickness, fill);
    box[4].CalcNormal(points5[0], points5[1], points5[2]);
    box[4].CalcNormal();
    box[5] = new Polygon3D(points6, tag, index, stroke, strokeThickness, fill);
    box[5].CalcNormal(points6[0], points6[1], points6[2]);
    box[5].CalcNormal();
    if (inverse)
    {
      graphics3D.AddVisual(box[0]);
      graphics3D.AddVisual(box[1]);
      graphics3D.AddVisual(box[2]);
      graphics3D.AddVisual(box[3]);
      graphics3D.AddVisual(box[4]);
      graphics3D.AddVisual(box[5]);
    }
    else
    {
      graphics3D.AddVisual(box[5]);
      graphics3D.AddVisual(box[4]);
      graphics3D.AddVisual(box[0]);
      graphics3D.AddVisual(box[1]);
      graphics3D.AddVisual(box[2]);
      graphics3D.AddVisual(box[3]);
    }
    return box;
  }

  internal static void UpdateBox(
    Polygon3D[] plan,
    Vector3D vector1,
    Vector3D vector2,
    Brush stroke,
    Visibility visibility)
  {
    if (plan.Length < 6)
      return;
    plan[0].Update(new Vector3D[4]
    {
      new Vector3D(vector1.X, vector1.Y, vector1.Z),
      new Vector3D(vector2.X, vector1.Y, vector1.Z),
      new Vector3D(vector2.X, vector2.Y, vector1.Z),
      new Vector3D(vector1.X, vector2.Y, vector1.Z)
    }, stroke, visibility);
    plan[1].Update(new Vector3D[4]
    {
      new Vector3D(vector1.X, vector1.Y, vector2.Z),
      new Vector3D(vector2.X, vector1.Y, vector2.Z),
      new Vector3D(vector2.X, vector2.Y, vector2.Z),
      new Vector3D(vector1.X, vector2.Y, vector2.Z)
    }, stroke, visibility);
    plan[2].Update(new Vector3D[4]
    {
      new Vector3D(vector1.X, vector1.Y, vector2.Z),
      new Vector3D(vector2.X, vector1.Y, vector2.Z),
      new Vector3D(vector2.X, vector1.Y, vector1.Z),
      new Vector3D(vector1.X, vector1.Y, vector1.Z)
    }, stroke, visibility);
    plan[3].Update(new Vector3D[4]
    {
      new Vector3D(vector1.X, vector2.Y, vector2.Z),
      new Vector3D(vector2.X, vector2.Y, vector2.Z),
      new Vector3D(vector2.X, vector2.Y, vector1.Z),
      new Vector3D(vector1.X, vector2.Y, vector1.Z)
    }, stroke, visibility);
    plan[4].Update(new Vector3D[4]
    {
      new Vector3D(vector1.X, vector1.Y, vector1.Z),
      new Vector3D(vector1.X, vector1.Y, vector2.Z),
      new Vector3D(vector1.X, vector2.Y, vector2.Z),
      new Vector3D(vector1.X, vector2.Y, vector1.Z)
    }, stroke, visibility);
    plan[5].Update(new Vector3D[4]
    {
      new Vector3D(vector2.X, vector1.Y, vector1.Z),
      new Vector3D(vector2.X, vector1.Y, vector2.Z),
      new Vector3D(vector2.X, vector2.Y, vector2.Z),
      new Vector3D(vector2.X, vector2.Y, vector1.Z)
    }, stroke, visibility);
  }

  internal static Line3D CreateLine(
    Line line,
    double x1,
    double y1,
    double x2,
    double y2,
    double z1,
    double z2,
    bool isfront)
  {
    double strokeThickness = line.StrokeThickness;
    Vector3D[] points = new Vector3D[3];
    if (isfront)
    {
      points[0] = new Vector3D(x1, y1, z1);
      points[1] = new Vector3D(x1 + strokeThickness, y2 + strokeThickness, z1);
      points[2] = new Vector3D(x2, y2, z1);
    }
    else
    {
      points[0] = new Vector3D(x1, y1, z1);
      points[1] = new Vector3D(x1, y2 + strokeThickness, z1 + strokeThickness);
      points[2] = new Vector3D(x1, y2, z2);
    }
    return new Line3D((UIElement) line, points);
  }

  internal virtual void Draw(Panel panel)
  {
    if (this.VectorPoints == null || this.VectorPoints.Length <= 0)
      return;
    ChartTransform.ChartTransform3D transform = this.Graphics3D.Transform;
    if (!(this.Element is Path element))
      return;
    element.Tag = (object) this.Tag;
    Color color = this.Fill is SolidColorBrush ? ((SolidColorBrush) this.Fill).Color : (!(this.Fill is LinearGradientBrush) || ((GradientBrush) this.Fill).GradientStops.Count <= 0 ? new SolidColorBrush(Colors.Transparent).Color : ((GradientBrush) this.Fill).GradientStops[0].Color);
    if (this.Tag != null && this.Tag is ChartSegment3D)
      ((ChartSegment3D) this.Tag).Polygons.Add(this);
    if (!panel.Children.Contains((UIElement) element))
      panel.Children.Add((UIElement) element);
    PathFigure pathFigure = new PathFigure();
    PathGeometry pathGeometry = new PathGeometry();
    if (transform != null)
    {
      pathFigure.StartPoint = transform.ToScreen(this.VectorPoints[0]);
      foreach (System.Windows.Media.LineSegment lineSegment in ((IEnumerable<Vector3D>) this.VectorPoints).Select<Vector3D, System.Windows.Media.LineSegment>((Func<Vector3D, System.Windows.Media.LineSegment>) (item => new System.Windows.Media.LineSegment()
      {
        Point = transform.ToScreen(item)
      })))
        pathFigure.Segments.Add((PathSegment) lineSegment);
    }
    pathGeometry.Figures.Add(pathFigure);
    element.Data = (Geometry) pathGeometry;
    int num1 = (int) (2.0 * (Math.Abs(this.normal & new Vector3D(0.0, 0.0, 1.0)) - 1.0));
    int num2 = (int) (2.0 * (Math.Abs(this.normal & new Vector3D(0.0, 1.0, 0.0)) - 1.0));
    int num3 = (int) (2.0 * (Math.Abs(this.normal & new Vector3D(1.0, 0.0, 0.0)) - 1.0));
    if (this.Fill is VisualBrush)
      element.Fill = this.Fill;
    else if (num1 == num3 && this.Fill != null)
      element.Fill = (Brush) Polygon3D.ApplyZLight(color);
    else if ((num2 == num1 || num1 != 0 && num2 < num1) && !(this.Tag is LineSegment3D) && !(this.Tag is AreaSegment3D) && !(this.Tag is PieSegment3D) && this.Fill != null)
      element.Fill = Polygon3D.ApplyXLight(color);
    else if (num1 < 0 && this.Fill != null)
      element.Fill = (Brush) Polygon3D.ApplyZLight(color);
    else
      element.Fill = this.Fill;
    element.StrokeThickness = this.strokeThickness;
    element.Stroke = this.Stroke;
  }

  internal void ReDraw()
  {
    if (this.VectorPoints == null || this.VectorPoints.Length <= 0)
      return;
    ChartTransform.ChartTransform3D transform = this.Graphics3D.Transform;
    if (!(this.Element is Path element))
      return;
    PathFigure pathFigure = new PathFigure();
    PathGeometry pathGeometry = new PathGeometry();
    if (transform != null)
    {
      pathFigure.StartPoint = transform.ToScreen(this.VectorPoints[0]);
      foreach (System.Windows.Media.LineSegment lineSegment in ((IEnumerable<Vector3D>) this.VectorPoints).Select<Vector3D, System.Windows.Media.LineSegment>((Func<Vector3D, System.Windows.Media.LineSegment>) (item => new System.Windows.Media.LineSegment()
      {
        Point = transform.ToScreen(item)
      })))
        pathFigure.Segments.Add((PathSegment) lineSegment);
    }
    pathGeometry.Figures.Add(pathFigure);
    element.Data = (Geometry) pathGeometry;
    int num1 = (int) (2.0 * (Math.Abs(this.normal & new Vector3D(0.0, 0.0, 1.0)) - 1.0));
    int num2 = (int) (2.0 * (Math.Abs(this.normal & new Vector3D(0.0, 1.0, 0.0)) - 1.0));
    int num3 = (int) (2.0 * (Math.Abs(this.normal & new Vector3D(1.0, 0.0, 0.0)) - 1.0));
    Color color = this.Fill is SolidColorBrush ? ((SolidColorBrush) this.Fill).Color : (!(this.Fill is LinearGradientBrush) || ((GradientBrush) this.Fill).GradientStops.Count <= 0 ? new SolidColorBrush(Colors.Transparent).Color : ((GradientBrush) this.Fill).GradientStops[0].Color);
    if (this.Fill is VisualBrush)
      element.Fill = this.Fill;
    else if (num1 == num3 && this.Fill != null)
      element.Fill = (Brush) Polygon3D.ApplyZLight(color);
    else if ((num2 == num1 || num1 != 0 && num2 < num1) && !(this.Tag is LineSegment3D) && !(this.Tag is AreaSegment3D) && !(this.Tag is PieSegment3D) && this.Fill != null)
      element.Fill = Polygon3D.ApplyXLight(color);
    else if (num1 < 0 && this.Fill != null)
      element.Fill = (Brush) Polygon3D.ApplyZLight(color);
    else
      element.Fill = this.Fill;
    element.StrokeThickness = this.strokeThickness;
    element.Stroke = this.Stroke;
  }

  internal void Update(Vector3D[] updatedPoints, Brush interior, Visibility visibility)
  {
    this.VectorPoints = updatedPoints;
    if (!(this.Element is Path element))
      return;
    element.Visibility = visibility;
    if (this.Graphics3D == null)
      return;
    ChartTransform.ChartTransform3D transform = this.Graphics3D.Transform;
    PathFigure pathFigure = new PathFigure();
    PathGeometry pathGeometry = new PathGeometry();
    Color color = this.Fill is SolidColorBrush ? ((SolidColorBrush) this.Fill).Color : (!(this.Fill is LinearGradientBrush) || ((GradientBrush) this.Fill).GradientStops.Count <= 0 ? new SolidColorBrush(Colors.Transparent).Color : ((GradientBrush) this.Fill).GradientStops[0].Color);
    if (transform != null)
    {
      pathFigure.StartPoint = transform.ToScreen(this.VectorPoints[0]);
      foreach (System.Windows.Media.LineSegment lineSegment in ((IEnumerable<Vector3D>) this.VectorPoints).Select<Vector3D, System.Windows.Media.LineSegment>((Func<Vector3D, System.Windows.Media.LineSegment>) (item => new System.Windows.Media.LineSegment()
      {
        Point = transform.ToScreen(item)
      })))
        pathFigure.Segments.Add((PathSegment) lineSegment);
    }
    pathGeometry.Figures.Add(pathFigure);
    int num1 = (int) (2.0 * (Math.Abs(this.normal & new Vector3D(0.0, 0.0, 1.0)) - 1.0));
    int num2 = (int) (2.0 * (Math.Abs(this.normal & new Vector3D(0.0, 1.0, 0.0)) - 1.0));
    int num3 = (int) (2.0 * (Math.Abs(this.normal & new Vector3D(1.0, 0.0, 0.0)) - 1.0));
    if (this.Fill is VisualBrush)
      element.Fill = interior;
    else if (num1 == num3 && interior != null)
      element.Fill = (Brush) Polygon3D.ApplyZLight(color);
    else if ((num2 == num1 || num1 != 0 && num2 < num1) && !(this.Tag is LineSegment3D) && !(this.Tag is AreaSegment3D) && interior != null)
      element.Fill = Polygon3D.ApplyXLight(color);
    else if (num1 < 0 && interior != null)
      element.Fill = (Brush) Polygon3D.ApplyZLight(color);
    else
      element.Fill = interior;
    element.Data = (Geometry) pathGeometry;
  }

  internal void CalcNormal()
  {
    this.CalcNormal(this.Points[0], this.Points[1], this.Points[2]);
    for (int index = 3; index < this.Points.Length && this.Test(); ++index)
      this.CalcNormal(this.Points[index], this.Points[0], this.Points[index / 2]);
  }

  internal virtual Vector3D GetNormal(Matrix3D transform)
  {
    Vector3D normal;
    if (this.VectorPoints != null)
    {
      normal = ChartMath.GetNormal(transform * this.VectorPoints[0], transform * this.VectorPoints[1], transform * this.VectorPoints[2]);
      for (int index = 3; index < this.VectorPoints.Length && !normal.IsValid; ++index)
        normal = ChartMath.GetNormal(transform * this.VectorPoints[index], transform * this.VectorPoints[0], transform * this.VectorPoints[index / 2]);
    }
    else
    {
      normal = transform & this.normal;
      normal.Normalize();
    }
    return normal;
  }

  protected internal void CalcNormal(Vector3D vector1, Vector3D vector2, Vector3D vector3)
  {
    Vector3D vector3D = (vector1 - vector2) * (vector3 - vector2);
    double num = vector3D.GetLength();
    if (num < 1E-05)
      num = 1.0;
    this.normal = new Vector3D(vector3D.X / num, vector3D.Y / num, vector3D.Z / num);
    this.d = -(this.A * vector1.X + this.B * vector1.Y + this.C * vector1.Z);
  }

  private static SolidColorBrush ApplyZLight(Color color)
  {
    return new SolidColorBrush(Color.FromArgb(color.A, (byte) ((double) color.R * 0.9), (byte) ((double) color.G * 0.9), (byte) ((double) color.B * 0.9)));
  }

  private static Brush ApplyXLight(Color color)
  {
    return (Brush) new SolidColorBrush(Color.FromArgb(color.A, (byte) ((double) color.R * 0.7), (byte) ((double) color.G * 0.7), (byte) ((double) color.B * 0.7)));
  }
}
