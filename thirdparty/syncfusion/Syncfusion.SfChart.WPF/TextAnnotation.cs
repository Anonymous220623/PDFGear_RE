// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.TextAnnotation
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class TextAnnotation : SingleAnnotation
{
  public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(nameof (Angle), typeof (double), typeof (SingleAnnotation), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(TextAnnotation.OnUpdatePropertyChanged)));

  public double Angle
  {
    get => (double) this.GetValue(TextAnnotation.AngleProperty);
    set => this.SetValue(TextAnnotation.AngleProperty, (object) value);
  }

  public override void UpdateAnnotation()
  {
    if (this.AnnotationElement != null && this.TextElement != null && this.X1 != null && this.Y1 != null)
    {
      this.TextElement.Visibility = Visibility.Visible;
      RotateTransform rotateTransform = new RotateTransform()
      {
        Angle = this.Angle
      };
      switch (this.CoordinateUnit)
      {
        case CoordinateUnit.Pixel:
          this.TextElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
          Size desiredSize1 = this.TextElement.DesiredSize;
          this.AnnotationElement.Height = desiredSize1.Height;
          this.AnnotationElement.Width = desiredSize1.Width;
          Point elementPosition1 = this.GetElementPosition(desiredSize1, new Point(Convert.ToDouble(this.X1), Convert.ToDouble(this.Y1)));
          Point point1 = new Point(elementPosition1.X + desiredSize1.Width / 2.0, elementPosition1.Y + desiredSize1.Height / 2.0);
          Canvas.SetLeft((UIElement) this.AnnotationElement, elementPosition1.X);
          Canvas.SetTop((UIElement) this.AnnotationElement, elementPosition1.Y);
          this.AnnotationElement.RenderTransformOrigin = new Point(0.5, 0.5);
          this.AnnotationElement.RenderTransform = (Transform) rotateTransform;
          Rect rect1 = this.RotateElement(this.Angle, (FrameworkElement) this.TextElement, desiredSize1);
          if (this.Angle > 0.0)
          {
            this.RotatedRect = new Rect(point1.X - rect1.Width / 2.0, point1.Y - rect1.Height / 2.0, rect1.Width, rect1.Height);
            break;
          }
          this.RotatedRect = new Rect(point1.X - desiredSize1.Width / 2.0, point1.Y - desiredSize1.Height / 2.0, desiredSize1.Width, desiredSize1.Height);
          break;
        case CoordinateUnit.Axis:
          base.UpdateAnnotation();
          if (this.XAxis == null || this.YAxis == null)
            break;
          if (this.CoordinateUnit == CoordinateUnit.Axis && this.EnableClipping)
          {
            this.x1 = this.GetClippingValues(this.x1, this.XAxis);
            this.y1 = this.GetClippingValues(this.y1, this.YAxis);
          }
          Point originalPosition = this.XAxis.Orientation == Orientation.Horizontal ? new Point(this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1), this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1)) : new Point(this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1), this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1));
          originalPosition.Y = double.IsNaN(originalPosition.Y) ? 0.0 : originalPosition.Y;
          originalPosition.X = double.IsNaN(originalPosition.X) ? 0.0 : originalPosition.X;
          this.TextElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
          Size desiredSize2 = this.TextElement.DesiredSize;
          Point elementPosition2 = this.GetElementPosition(desiredSize2, originalPosition);
          Point point2 = new Point(elementPosition2.X + desiredSize2.Width / 2.0, elementPosition2.Y + desiredSize2.Height / 2.0);
          Rect rect2 = this.RotateElement(this.Angle, (FrameworkElement) this.TextElement, desiredSize2);
          this.AnnotationElement.Height = desiredSize2.Height;
          this.AnnotationElement.Width = desiredSize2.Width;
          Canvas.SetLeft((UIElement) this.AnnotationElement, elementPosition2.X);
          Canvas.SetTop((UIElement) this.AnnotationElement, elementPosition2.Y);
          this.AnnotationElement.RenderTransformOrigin = new Point(0.5, 0.5);
          this.AnnotationElement.RenderTransform = (Transform) rotateTransform;
          if (this.Angle > 0.0)
          {
            this.RotatedRect = new Rect(point2.X - rect2.Width / 2.0, point2.Y - rect2.Height / 2.0, rect2.Width, rect2.Height);
            break;
          }
          this.RotatedRect = new Rect(point2.X - desiredSize2.Width / 2.0, point2.Y - desiredSize2.Height / 2.0, desiredSize2.Width, desiredSize2.Height);
          break;
      }
    }
    else
    {
      if (this.TextElement == null)
        return;
      this.TextElement.Visibility = Visibility.Collapsed;
    }
  }

  public override UIElement GetRenderedAnnotation() => (UIElement) this.AnnotationElement;

  internal override UIElement CreateAnnotation()
  {
    if (this.AnnotationElement != null && this.AnnotationElement.Children.Count == 0)
    {
      this.SetBindings();
      this.TextElement.Tag = (object) this;
      this.AnnotationElement.Children.Add((UIElement) this.TextElementCanvas);
      this.TextElementCanvas.Children.Add((UIElement) this.TextElement);
    }
    return (UIElement) this.AnnotationElement;
  }

  protected override DependencyObject CloneAnnotation(Annotation annotation)
  {
    return base.CloneAnnotation((Annotation) new TextAnnotation()
    {
      Angle = this.Angle
    });
  }

  private static void OnUpdatePropertyChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(sender is Annotation annotation))
      return;
    annotation.UpdatePropertyChanged(args);
  }
}
