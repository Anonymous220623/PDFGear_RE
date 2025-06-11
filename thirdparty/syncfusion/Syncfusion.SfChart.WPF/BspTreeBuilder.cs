// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.BspTreeBuilder
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal sealed class BspTreeBuilder
{
  private const double EPSILON = 0.0005;
  internal readonly List<Polygon3D> Polygons = new List<Polygon3D>();

  public Polygon3D this[int index] => this.Polygons[index];

  public int Add(Polygon3D poly)
  {
    this.Polygons.Add(poly);
    return this.Polygons.Count - 1;
  }

  public void Remove(Polygon3D polygon) => this.Polygons.Remove(polygon);

  public void Clear() => this.Polygons.Clear();

  public int Count() => this.Polygons.Count;

  public BspNode Build() => this.Build(this.Polygons);

  public BspNode Build(List<Polygon3D> arlist)
  {
    if (arlist.Count < 1)
      return (BspNode) null;
    BspNode bspNode = new BspNode();
    Polygon3D polygon3D1 = arlist[0];
    bspNode.Plane = polygon3D1;
    List<Polygon3D> arlist1 = new List<Polygon3D>(arlist.Count);
    List<Polygon3D> arlist2 = new List<Polygon3D>(arlist.Count);
    int index = 1;
    for (int count = arlist.Count; index < count; ++index)
    {
      Polygon3D polygon3D2 = arlist[index];
      if (polygon3D2 != polygon3D1)
      {
        switch (BspTreeBuilder.ClassifyPolygon(polygon3D1, polygon3D2))
        {
          case ClassifyPolyResult.OnPlane:
          case ClassifyPolyResult.ToRight:
            arlist2.Add(polygon3D2);
            continue;
          case ClassifyPolyResult.ToLeft:
            arlist1.Add(polygon3D2);
            continue;
          case ClassifyPolyResult.Unknown:
            switch (polygon3D2)
            {
              case Line3D _:
              case UIElement3D _:
                arlist1.Add(polygon3D2);
                continue;
              case PolyLine3D _:
                arlist2.Add(polygon3D2);
                continue;
              default:
                Polygon3D[] backPoly;
                Polygon3D[] frontPoly;
                BspTreeBuilder.SplitPolygon(polygon3D2, polygon3D1, out backPoly, out frontPoly);
                arlist1.AddRange((IEnumerable<Polygon3D>) backPoly);
                arlist2.AddRange((IEnumerable<Polygon3D>) frontPoly);
                continue;
            }
          default:
            continue;
        }
      }
    }
    if (arlist1.Count > 0)
      bspNode.Back = this.Build(arlist1);
    if (arlist2.Count > 0)
      bspNode.Front = this.Build(arlist2);
    return bspNode;
  }

  public int GetNodeCount(BspNode el)
  {
    return el != null ? 1 + this.GetNodeCount(el.Back) + this.GetNodeCount(el.Front) : 0;
  }

  private static void CutOutBackPolygon(
    List<Vector3DIndexClassification> polyPoints,
    Vector3DIndexClassification vwiwc,
    ICollection<Vector3D> points)
  {
    points.Clear();
    Vector3DIndexClassification dindexClassification = vwiwc;
    while (true)
    {
      dindexClassification.AlreadyCuttedBack = true;
      points.Add(dindexClassification.Vector);
      Vector3DIndexClassification polyPoint1 = polyPoints[dindexClassification.CuttingBackPairIndex];
      if (dindexClassification.CuttingBackPoint)
      {
        if (!polyPoint1.AlreadyCuttedBack)
        {
          dindexClassification = polyPoint1;
        }
        else
        {
          Vector3DIndexClassification polyPoint2 = polyPoints[BspTreeBuilder.GetNext(dindexClassification.Index - 1, polyPoints.Count)];
          Vector3DIndexClassification polyPoint3 = polyPoints[BspTreeBuilder.GetNext(dindexClassification.Index + 1, polyPoints.Count)];
          if (polyPoint2.Result == ClassifyPointResult.OnBack && !polyPoint2.AlreadyCuttedBack)
            dindexClassification = polyPoint2;
          else if (polyPoint3.Result == ClassifyPointResult.OnBack && !polyPoint3.AlreadyCuttedBack)
            dindexClassification = polyPoint3;
          else
            break;
        }
      }
      else
      {
        Vector3DIndexClassification polyPoint4 = polyPoints[BspTreeBuilder.GetNext(dindexClassification.Index - 1, polyPoints.Count)];
        Vector3DIndexClassification polyPoint5 = polyPoints[BspTreeBuilder.GetNext(dindexClassification.Index + 1, polyPoints.Count)];
        if (polyPoint4.Result != ClassifyPointResult.OnFront && !polyPoint4.AlreadyCuttedBack)
          dindexClassification = polyPoint4;
        else if (polyPoint5.Result != ClassifyPointResult.OnFront && !polyPoint5.AlreadyCuttedBack)
          dindexClassification = polyPoint5;
        else
          goto label_8;
      }
    }
    return;
label_8:;
  }

  private static void CutOutFrontPolygon(
    List<Vector3DIndexClassification> polyPoints,
    Vector3DIndexClassification vwiwc,
    List<Vector3D> points)
  {
    points.Clear();
    Vector3DIndexClassification dindexClassification = vwiwc;
    while (true)
    {
      dindexClassification.AlreadyCuttedFront = true;
      points.Add(dindexClassification.Vector);
      Vector3DIndexClassification polyPoint1 = polyPoints[dindexClassification.CuttingFrontPairIndex];
      if (dindexClassification.CuttingFrontPoint)
      {
        if (!polyPoint1.AlreadyCuttedFront)
        {
          dindexClassification = polyPoint1;
        }
        else
        {
          Vector3DIndexClassification polyPoint2 = polyPoints[BspTreeBuilder.GetNext(dindexClassification.Index - 1, polyPoints.Count)];
          Vector3DIndexClassification polyPoint3 = polyPoints[BspTreeBuilder.GetNext(dindexClassification.Index + 1, polyPoints.Count)];
          if (polyPoint2.Result == ClassifyPointResult.OnFront && !polyPoint2.AlreadyCuttedFront)
            dindexClassification = polyPoint2;
          else if (polyPoint3.Result == ClassifyPointResult.OnFront && !polyPoint3.AlreadyCuttedFront)
            dindexClassification = polyPoint3;
          else
            break;
        }
      }
      else
      {
        Vector3DIndexClassification polyPoint4 = polyPoints[BspTreeBuilder.GetNext(dindexClassification.Index - 1, polyPoints.Count)];
        Vector3DIndexClassification polyPoint5 = polyPoints[BspTreeBuilder.GetNext(dindexClassification.Index + 1, polyPoints.Count)];
        if (polyPoint4.Result != ClassifyPointResult.OnBack && !polyPoint4.AlreadyCuttedFront)
          dindexClassification = polyPoint4;
        else if (polyPoint5.Result != ClassifyPointResult.OnBack && !polyPoint5.AlreadyCuttedFront)
          dindexClassification = polyPoint5;
        else
          goto label_8;
      }
    }
    return;
label_8:;
  }

  private static int GetNext(int index, int count)
  {
    if (index >= count)
      return index - count;
    return index < 0 ? index + count : index;
  }

  private static ClassifyPolyResult ClassifyPolygon(Polygon3D polygon1, Polygon3D polygon2)
  {
    ClassifyPolyResult classifyPolyResult = ClassifyPolyResult.Unknown;
    Vector3D[] points = polygon2.Points;
    if (points == null)
      return classifyPolyResult;
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    Vector3D normal = polygon1.Normal;
    double d = polygon1.D;
    int index = 0;
    for (int length = points.Length; index < length; ++index)
    {
      double num4 = -d - (points[index] & normal);
      if (num4 > 0.0005)
        ++num1;
      else if (num4 < -0.0005)
        ++num2;
      else
        ++num3;
      if (num1 > 0 && num2 > 0)
        break;
    }
    return num3 != points.Length ? (num2 + num3 != points.Length ? (num1 + num3 != points.Length ? ClassifyPolyResult.Unknown : ClassifyPolyResult.ToLeft) : ClassifyPolyResult.ToRight) : ClassifyPolyResult.OnPlane;
  }

  private static ClassifyPointResult ClassifyPoint(Vector3D point3D, Polygon3D polygon3D)
  {
    ClassifyPointResult classifyPointResult = ClassifyPointResult.OnPlane;
    double num = -polygon3D.D - (point3D & polygon3D.Normal);
    if (num > 0.0005)
      classifyPointResult = ClassifyPointResult.OnBack;
    else if (num < -0.0005)
      classifyPointResult = ClassifyPointResult.OnFront;
    return classifyPointResult;
  }

  private static void SplitPolygon(
    Polygon3D poly,
    Polygon3D part,
    out Polygon3D[] backPoly,
    out Polygon3D[] frontPoly)
  {
    List<Polygon3D> polygon3DList1 = new List<Polygon3D>();
    List<Polygon3D> polygon3DList2 = new List<Polygon3D>();
    if (poly.Points != null)
    {
      List<Vector3DIndexClassification> polyPoints = new List<Vector3DIndexClassification>();
      List<Vector3DIndexClassification> dindexClassificationList1 = new List<Vector3DIndexClassification>();
      List<Vector3DIndexClassification> dindexClassificationList2 = new List<Vector3DIndexClassification>();
      List<Vector3D> points1 = new List<Vector3D>();
      List<Vector3D> points2 = new List<Vector3D>();
      int length = poly.Points.Length;
      for (int index = 0; index < length; ++index)
      {
        Vector3D point1 = poly.Points[index];
        Vector3D point2 = poly.Points[BspTreeBuilder.GetNext(index + 1, length)];
        ClassifyPointResult res = BspTreeBuilder.ClassifyPoint(point1, part);
        ClassifyPointResult classifyPointResult1 = BspTreeBuilder.ClassifyPoint(point2, part);
        Vector3DIndexClassification dindexClassification1 = new Vector3DIndexClassification(point1, polyPoints.Count, res);
        polyPoints.Add(dindexClassification1);
        if (res != classifyPointResult1 && res != ClassifyPointResult.OnPlane && classifyPointResult1 != ClassifyPointResult.OnPlane)
        {
          Vector3D vector3D = point1 - point2;
          double num = (part.Normal * -part.D - point2 & part.Normal) / (part.Normal & vector3D);
          Vector3DIndexClassification dindexClassification2 = new Vector3DIndexClassification(point2 + vector3D * num, polyPoints.Count, ClassifyPointResult.OnPlane);
          polyPoints.Add(dindexClassification2);
          dindexClassificationList1.Add(dindexClassification2);
          dindexClassificationList2.Add(dindexClassification2);
        }
        else if (res == ClassifyPointResult.OnPlane)
        {
          ClassifyPointResult classifyPointResult2 = BspTreeBuilder.ClassifyPoint(poly.Points[BspTreeBuilder.GetNext(index - 1, length)], part);
          if (classifyPointResult2 != classifyPointResult1)
          {
            if (classifyPointResult2 != ClassifyPointResult.OnPlane && classifyPointResult1 != ClassifyPointResult.OnPlane)
            {
              dindexClassificationList1.Add(dindexClassification1);
              dindexClassificationList2.Add(dindexClassification1);
            }
            else if (classifyPointResult2 == ClassifyPointResult.OnPlane)
            {
              switch (classifyPointResult1)
              {
                case ClassifyPointResult.OnFront:
                  dindexClassificationList2.Add(dindexClassification1);
                  continue;
                case ClassifyPointResult.OnBack:
                  dindexClassificationList1.Add(dindexClassification1);
                  continue;
                default:
                  continue;
              }
            }
            else if (classifyPointResult1 == ClassifyPointResult.OnPlane)
            {
              switch (classifyPointResult2)
              {
                case ClassifyPointResult.OnFront:
                  dindexClassificationList2.Add(dindexClassification1);
                  continue;
                case ClassifyPointResult.OnBack:
                  dindexClassificationList1.Add(dindexClassification1);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
      if (dindexClassificationList2.Count != 0 || dindexClassificationList1.Count != 0)
      {
        for (int index = 0; index < dindexClassificationList1.Count - 1; index += 2)
        {
          Vector3DIndexClassification dindexClassification3 = dindexClassificationList1[index];
          Vector3DIndexClassification dindexClassification4 = dindexClassificationList1[index + 1];
          dindexClassification3.CuttingBackPoint = true;
          dindexClassification4.CuttingBackPoint = true;
          dindexClassification3.CuttingBackPairIndex = dindexClassification4.Index;
          dindexClassification4.CuttingBackPairIndex = dindexClassification3.Index;
        }
        for (int index = 0; index < dindexClassificationList2.Count - 1; index += 2)
        {
          Vector3DIndexClassification dindexClassification5 = dindexClassificationList2[index];
          Vector3DIndexClassification dindexClassification6 = dindexClassificationList2[index + 1];
          dindexClassification5.CuttingFrontPoint = true;
          dindexClassification6.CuttingFrontPoint = true;
          dindexClassification5.CuttingFrontPairIndex = dindexClassification6.Index;
          dindexClassification6.CuttingFrontPairIndex = dindexClassification5.Index;
        }
        for (int index = 0; index < dindexClassificationList1.Count - 1; ++index)
        {
          Vector3DIndexClassification vwiwc = dindexClassificationList1[index];
          if (!vwiwc.AlreadyCuttedBack)
          {
            BspTreeBuilder.CutOutBackPolygon(polyPoints, vwiwc, (ICollection<Vector3D>) points1);
            if (points1.Count > 2)
            {
              Vector3D[] array = points1.ToArray();
              Polygon3D polygon3D = new Polygon3D(array, poly);
              polygon3D.CalcNormal(array[0], array[1], array[2]);
              polygon3D.CalcNormal();
              polygon3DList1.Add(polygon3D);
            }
          }
        }
        for (int index = 0; index < dindexClassificationList2.Count - 1; ++index)
        {
          Vector3DIndexClassification vwiwc = dindexClassificationList2[index];
          if (!vwiwc.AlreadyCuttedFront)
          {
            BspTreeBuilder.CutOutFrontPolygon(polyPoints, vwiwc, points2);
            if (points2.Count > 2)
            {
              Vector3D[] array = points2.ToArray();
              Polygon3D polygon3D = new Polygon3D(array, poly);
              polygon3D.CalcNormal(array[0], array[1], array[2]);
              polygon3D.CalcNormal();
              polygon3DList2.Add(polygon3D);
            }
          }
        }
      }
    }
    else
    {
      polygon3DList1.Add(poly);
      polygon3DList2.Add(poly);
    }
    backPoly = polygon3DList1.ToArray();
    frontPoly = polygon3DList2.ToArray();
  }
}
