// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.RectangleAnnotation
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class RectangleAnnotation : SolidShapeAnnotation
{
  internal override UIElement CreateAnnotation()
  {
    if (this.AnnotationElement != null && this.AnnotationElement.Children.Count == 0)
    {
      this.shape = (Shape) new Rectangle();
      this.shape.Tag = (object) this;
      this.SetBindings();
      this.AnnotationElement.Children.Add((UIElement) this.shape);
      this.TextElementCanvas.Children.Add((UIElement) this.TextElement);
      this.AnnotationElement.Children.Add((UIElement) this.TextElementCanvas);
    }
    return (UIElement) this.AnnotationElement;
  }

  protected override DependencyObject CloneAnnotation(Annotation annotation)
  {
    return base.CloneAnnotation((Annotation) new RectangleAnnotation());
  }
}
