// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.PolygonRecycler
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class PolygonRecycler : DependencyObject
{
  private List<Polygon3D> polygonCache = new List<Polygon3D>();
  private int pointer;
  private Polygon3D polygon;

  public Polygon3D DequeuePolygon(
    Vector3D[] points,
    DependencyObject tag,
    int index,
    Brush stroke,
    double strokeThickness,
    Brush fill)
  {
    if (this.polygonCache.Count > this.pointer)
    {
      this.polygon = this.polygonCache[this.pointer];
      this.polygon.Element = (UIElement) new Path();
      this.polygon.Tag = tag;
      this.polygon.Index = index;
      this.polygon.Stroke = stroke;
      this.polygon.CalcNormal(points[0], points[1], points[2]);
      this.polygon.VectorPoints = points;
      this.polygon.CalcNormal();
      ++this.pointer;
    }
    else if (this.polygonCache.Count <= this.pointer)
    {
      this.polygon = new Polygon3D(points, tag, index, stroke, strokeThickness, fill);
      this.polygon.CalcNormal(points[0], points[1], points[2]);
      this.polygon.CalcNormal();
      this.polygonCache.Add(this.polygon);
      ++this.pointer;
    }
    return this.polygon;
  }

  public void Reset() => this.pointer = 0;
}
