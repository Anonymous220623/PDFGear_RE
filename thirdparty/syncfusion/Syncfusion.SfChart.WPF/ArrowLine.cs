// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ArrowLine
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ArrowLine : DependencyObject
{
  public static readonly DependencyProperty X1Property = DependencyProperty.Register(nameof (X1), typeof (double), typeof (ArrowLine), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty Y1Property = DependencyProperty.Register(nameof (Y1), typeof (double), typeof (ArrowLine), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty X2Property = DependencyProperty.Register(nameof (X2), typeof (double), typeof (ArrowLine), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty Y2Property = DependencyProperty.Register(nameof (Y2), typeof (double), typeof (ArrowLine), new PropertyMetadata((object) 0.0));
  protected PathGeometry pathGeometry;
  protected PathFigure pathFigureLine;
  private System.Windows.Media.LineSegment segmentLine;
  private PathFigure pathFigureHead;
  private PolyLineSegment polySegmentHead;
  private double arrowAngle = 45.0;
  private double arrowLength = 12.0;

  public ArrowLine()
  {
    this.pathGeometry = new PathGeometry();
    this.pathFigureLine = new PathFigure();
    this.segmentLine = new System.Windows.Media.LineSegment();
    this.pathFigureLine.Segments.Add((PathSegment) this.segmentLine);
    this.pathFigureHead = new PathFigure();
    this.polySegmentHead = new PolyLineSegment();
    this.pathFigureHead.Segments.Add((PathSegment) this.polySegmentHead);
  }

  public double X1
  {
    get => (double) this.GetValue(ArrowLine.X1Property);
    set => this.SetValue(ArrowLine.X1Property, (object) value);
  }

  public double Y1
  {
    get => (double) this.GetValue(ArrowLine.Y1Property);
    set => this.SetValue(ArrowLine.Y1Property, (object) value);
  }

  public double X2
  {
    get => (double) this.GetValue(ArrowLine.X2Property);
    set => this.SetValue(ArrowLine.X2Property, (object) value);
  }

  public double Y2
  {
    get => (double) this.GetValue(ArrowLine.Y2Property);
    set => this.SetValue(ArrowLine.Y2Property, (object) value);
  }

  public Geometry GetGeometry()
  {
    Point point1 = new Point(this.X1, this.Y1);
    Point point2 = new Point(this.X2, this.Y2);
    this.pathGeometry.Figures.Clear();
    this.pathFigureLine.StartPoint = point1;
    this.segmentLine.Point = point2;
    this.pathGeometry.Figures.Add(this.pathFigureLine);
    this.pathGeometry.Figures.Add(this.CalculateArrow(this.pathFigureHead, point1, point2));
    return (Geometry) this.pathGeometry;
  }

  private static Point MultiplyMatrixVector(Point point, Matrix mat)
  {
    return new Point(mat.M11 * point.X + mat.M12 * point.Y, mat.M21 * point.X + mat.M22 * point.Y);
  }

  private static Matrix MultiplyMatrixes(Matrix mat1, Matrix mat2)
  {
    return new Matrix(mat1.M11 * mat2.M11 + mat1.M12 * mat2.M21, mat1.M11 * mat2.M12 + mat1.M12 * mat2.M22, mat1.M21 * mat2.M11 + mat1.M22 * mat2.M21, mat1.M21 * mat2.M12 + mat1.M22 * mat2.M22, 0.0, 0.0);
  }

  private static Point FindNormalization(Point vectPoint, double len)
  {
    double num = Math.Sqrt(vectPoint.X * vectPoint.X + vectPoint.Y * vectPoint.Y);
    return new Point(vectPoint.X / num * len, vectPoint.Y / num * len);
  }

  private PathFigure CalculateArrow(PathFigure pathfigure, Point point1, Point point2)
  {
    Matrix mat1 = new Matrix();
    mat1.M11 = 1.0;
    mat1.M22 = 1.0;
    Point normalization = ArrowLine.FindNormalization(new Point(point1.X - point2.X, point1.Y - point2.Y), this.arrowLength);
    PolyLineSegment segment = pathfigure.Segments[0] as PolyLineSegment;
    segment.Points.Clear();
    double num1 = 2.0 * Math.PI * (this.arrowAngle / 2.0) / 360.0;
    double m12_1 = Math.Sin(num1);
    double num2 = Math.Cos(num1);
    Matrix mat2 = new Matrix(num2, m12_1, -m12_1, num2, 0.0, 0.0);
    Matrix matrix = ArrowLine.MultiplyMatrixes(mat1, mat2);
    Point point3 = ArrowLine.MultiplyMatrixVector(normalization, matrix);
    pathfigure.StartPoint = new Point(point2.X + point3.X, point2.Y + point3.Y);
    segment.Points.Add(point2);
    double num3 = 2.0 * Math.PI * -this.arrowAngle / 360.0;
    double m12_2 = Math.Sin(num3);
    double num4 = Math.Cos(num3);
    mat2 = new Matrix(num4, m12_2, -m12_2, num4, 0.0, 0.0);
    Matrix mat = ArrowLine.MultiplyMatrixes(matrix, mat2);
    Point point4 = ArrowLine.MultiplyMatrixVector(normalization, mat);
    segment.Points.Add(new Point(point2.X + point4.X, point2.Y + point4.Y));
    pathfigure.IsClosed = true;
    return pathfigure;
  }
}
