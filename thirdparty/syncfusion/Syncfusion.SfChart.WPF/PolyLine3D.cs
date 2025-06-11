// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.PolyLine3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class PolyLine3D : Polygon3D
{
  private readonly Path element;

  public PolyLine3D(Path element, List<Vector3D> vectors)
    : base(vectors.ToArray())
  {
    this.element = element;
    this.CalcNormal(vectors.ToArray()[0], vectors.ToArray()[1], vectors.ToArray()[2]);
    this.CalcNormal();
  }

  internal override void Draw(Panel panel)
  {
    if (this.element.Parent == null)
      panel.Children.Add((UIElement) this.element);
    ChartTransform.ChartTransform3D transform = this.Graphics3D.Transform;
    if (transform == null)
      return;
    PathFigure pathFigure = new PathFigure();
    this.element.Data = (Geometry) new PathGeometry()
    {
      Figures = {
        pathFigure
      }
    };
    pathFigure.StartPoint = transform.ToScreen(this.VectorPoints[0]);
    PolyLineSegment polyLineSegment = new PolyLineSegment();
    polyLineSegment.Points = new PointCollection();
    foreach (Vector3D vectorPoint in this.VectorPoints)
      polyLineSegment.Points.Add(transform.ToScreen(vectorPoint));
    pathFigure.Segments.Add((PathSegment) polyLineSegment);
  }
}
