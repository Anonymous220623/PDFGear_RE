// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.UIElement3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class UIElement3D : Polygon3D
{
  private readonly FrameworkElement element;

  public UIElement3D(UIElement element, Vector3D[] points)
    : base(points)
  {
    this.element = element as FrameworkElement;
    this.CalcNormal(points[0], points[1], points[2]);
    this.CalcNormal();
  }

  internal UIElementLeftShift LeftShift { get; set; }

  internal UIElementTopShift TopShift { get; set; }

  internal override void Draw(Panel panel)
  {
    if (this.element.Parent == null)
      panel.Children.Add((UIElement) this.element);
    this.element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    ChartTransform.ChartTransform3D transform = this.Graphics3D.Transform;
    if (transform == null)
      return;
    Point screen = transform.ToScreen(this.VectorPoints[0]);
    double x = screen.X;
    double y = screen.Y;
    this.GetShift(ref x, ref y, this.element.DesiredSize.Width, this.element.DesiredSize.Height);
    Canvas.SetLeft((UIElement) this.element, x);
    Canvas.SetTop((UIElement) this.element, y);
  }

  private void GetShift(ref double x, ref double y, double width, double height)
  {
    if (this.LeftShift == UIElementLeftShift.LeftShift)
      x -= width;
    else if (this.LeftShift == UIElementLeftShift.RightShift)
      x += width;
    else if (this.LeftShift == UIElementLeftShift.LeftHalfShift)
      x -= width / 2.0;
    else if (this.LeftShift == UIElementLeftShift.RightHalfShift)
      x += width / 2.0;
    if (this.TopShift == UIElementTopShift.TopShift)
      y -= height;
    else if (this.TopShift == UIElementTopShift.BottomShift)
      y += height;
    else if (this.TopShift == UIElementTopShift.TopHalfShift)
    {
      y -= height * 0.5;
    }
    else
    {
      if (this.TopShift != UIElementTopShift.BottomHalfShift)
        return;
      y += height * 0.5;
    }
  }
}
