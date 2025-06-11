// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.EllipseAnnotation
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class EllipseAnnotation : SolidShapeAnnotation
{
  public new static readonly DependencyProperty WidthProperty = DependencyProperty.Register(nameof (Width), typeof (double), typeof (EllipseAnnotation), new PropertyMetadata((object) 10.0, new PropertyChangedCallback(EllipseAnnotation.OnSizePropertyChanged)));
  public new static readonly DependencyProperty HeightProperty = DependencyProperty.Register(nameof (Height), typeof (double), typeof (EllipseAnnotation), new PropertyMetadata((object) 10.0, new PropertyChangedCallback(EllipseAnnotation.OnSizePropertyChanged)));

  public new double Width
  {
    get => (double) this.GetValue(EllipseAnnotation.WidthProperty);
    set => this.SetValue(EllipseAnnotation.WidthProperty, (object) value);
  }

  public new double Height
  {
    get => (double) this.GetValue(EllipseAnnotation.HeightProperty);
    set => this.SetValue(EllipseAnnotation.HeightProperty, (object) value);
  }

  public override void UpdateAnnotation()
  {
    if (this.IsRenderSize())
    {
      if ((this.shape != null || this.ResizerControl != null) && this.X1 != null && this.Y1 != null)
      {
        switch (this.CoordinateUnit)
        {
          case CoordinateUnit.Pixel:
            double x = Convert.ToDouble(this.X1);
            double y = Convert.ToDouble(this.Y1);
            this.UpdatePixelAnnotation(new Point(x, y), new Point(x + this.Width, y + this.Height));
            break;
          case CoordinateUnit.Axis:
            this.SetData();
            if (this.XAxis != null && this.YAxis != null)
            {
              Point point = this.XAxis.Orientation == Orientation.Horizontal ? new Point(this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1), this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1)) : new Point(this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1), this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1));
              Point point2 = new Point((double.IsNaN(point.X) ? 0.0 : point.X) + this.Width, (double.IsNaN(point.Y) ? 0.0 : point.Y) + this.Height);
              this.UpdateAxisAnnotation(point, point2);
              break;
            }
            break;
        }
        this.CheckResizerValues();
      }
      else
      {
        if (this.shape == null && this.ResizerControl == null)
          return;
        this.ClearAnnotationElements();
      }
    }
    else
      base.UpdateAnnotation();
  }

  internal override UIElement CreateAnnotation()
  {
    if (this.AnnotationElement != null && this.AnnotationElement.Children.Count == 0)
    {
      this.shape = (Shape) new Ellipse();
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
    return base.CloneAnnotation((Annotation) new EllipseAnnotation());
  }

  private static void OnSizePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is EllipseAnnotation ellipseAnnotation))
      return;
    ellipseAnnotation.UpdateAnnotation();
  }

  private static void UpdateAnnotation(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is EllipseAnnotation ellipseAnnotation))
      return;
    ellipseAnnotation.UpdateAnnotation();
  }

  private bool IsRenderSize()
  {
    if (this.X2 == null || this.X2.GetType() == typeof (double) && double.IsNaN((double) this.X2) || this.Y2 == null)
      return true;
    return this.Y2.GetType() == typeof (double) && double.IsNaN((double) this.Y2);
  }
}
