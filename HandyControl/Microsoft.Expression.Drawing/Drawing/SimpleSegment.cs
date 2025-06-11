// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Drawing.SimpleSegment
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Collections.Generic;
using System.Windows;

#nullable disable
namespace HandyControl.Expression.Drawing;

internal class SimpleSegment
{
  private SimpleSegment()
  {
  }

  public Point[] Points { get; private set; }

  public SimpleSegment.SegmentType Type { get; private set; }

  public static SimpleSegment Create(Point point0, Point point1)
  {
    return new SimpleSegment()
    {
      Type = SimpleSegment.SegmentType.Line,
      Points = new Point[2]{ point0, point1 }
    };
  }

  public static SimpleSegment Create(Point point0, Point point1, Point point2)
  {
    Point point3 = GeometryHelper.Lerp(point0, point1, 2.0 / 3.0);
    Point point4 = GeometryHelper.Lerp(point1, point2, 1.0 / 3.0);
    return new SimpleSegment()
    {
      Type = SimpleSegment.SegmentType.CubicBeizer,
      Points = new Point[4]
      {
        point0,
        point3,
        point4,
        point2
      }
    };
  }

  public static SimpleSegment Create(Point point0, Point point1, Point point2, Point point3)
  {
    return new SimpleSegment()
    {
      Type = SimpleSegment.SegmentType.CubicBeizer,
      Points = new Point[4]
      {
        point0,
        point1,
        point2,
        point3
      }
    };
  }

  public void Flatten(
    IList<Point> resultPolyline,
    double tolerance,
    IList<double> resultParameters)
  {
    switch (this.Type)
    {
      case SimpleSegment.SegmentType.Line:
        resultPolyline.Add(this.Points[1]);
        resultParameters?.Add(1.0);
        break;
      case SimpleSegment.SegmentType.CubicBeizer:
        BezierCurveFlattener.FlattenCubic(this.Points, tolerance, (ICollection<Point>) resultPolyline, true, (ICollection<double>) resultParameters);
        break;
    }
  }

  public enum SegmentType
  {
    Line,
    CubicBeizer,
  }
}
