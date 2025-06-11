// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.Line3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class Line3D : Polygon3D
{
  private readonly UIElement element;

  public Line3D(UIElement element, Vector3D[] points)
    : base(points)
  {
    this.element = element;
    this.CalcNormal(points[0], points[1], points[2]);
    this.CalcNormal();
  }

  internal override void Draw(Panel panel)
  {
    if (((FrameworkElement) this.element).Parent == null)
      panel.Children.Add(this.element);
    ChartTransform.ChartTransform3D transform = this.Graphics3D.Transform;
    if (transform == null)
      return;
    Point screen1 = transform.ToScreen(this.VectorPoints[0]);
    Point screen2 = transform.ToScreen(this.VectorPoints[2]);
    if (!(this.element is Line element))
      return;
    element.X1 = screen1.X;
    element.X2 = screen2.X;
    element.Y1 = screen1.Y;
    element.Y2 = screen2.Y;
  }
}
