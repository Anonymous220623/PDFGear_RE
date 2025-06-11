// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ImageAnnotation
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ImageAnnotation : SingleAnnotation
{
  public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(nameof (Angle), typeof (double), typeof (ImageAnnotation), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ImageAnnotation.OnUpdatePropertyChanged)));
  public static readonly DependencyProperty Y2Property = DependencyProperty.Register(nameof (Y2), typeof (object), typeof (ImageAnnotation), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageAnnotation.OnHeightWidthChanged)));
  public static readonly DependencyProperty X2Property = DependencyProperty.Register(nameof (X2), typeof (object), typeof (ImageAnnotation), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageAnnotation.OnHeightWidthChanged)));
  public static readonly DependencyProperty HorizontalTextAlignmentProperty = DependencyProperty.Register(nameof (HorizontalTextAlignment), typeof (HorizontalAlignment), typeof (ImageAnnotation), new PropertyMetadata((object) HorizontalAlignment.Center, new PropertyChangedCallback(ImageAnnotation.OnAlignmentChanged)));
  public static readonly DependencyProperty VerticalTextAlignmentProperty = DependencyProperty.Register(nameof (VerticalTextAlignment), typeof (VerticalAlignment), typeof (ImageAnnotation), new PropertyMetadata((object) VerticalAlignment.Bottom, new PropertyChangedCallback(Annotation.OnTextAlignmentChanged)));
  public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(nameof (ImageSource), typeof (ImageSource), typeof (ImageAnnotation), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageAnnotation.OnImageSourceChanged)));
  private Image _image;

  public double Angle
  {
    get => (double) this.GetValue(ImageAnnotation.AngleProperty);
    set => this.SetValue(ImageAnnotation.AngleProperty, (object) value);
  }

  public ImageSource ImageSource
  {
    get => (ImageSource) this.GetValue(ImageAnnotation.ImageSourceProperty);
    set => this.SetValue(ImageAnnotation.ImageSourceProperty, (object) value);
  }

  public object Y2
  {
    get => this.GetValue(ImageAnnotation.Y2Property);
    set => this.SetValue(ImageAnnotation.Y2Property, value);
  }

  public object X2
  {
    get => this.GetValue(ImageAnnotation.X2Property);
    set => this.SetValue(ImageAnnotation.X2Property, value);
  }

  public HorizontalAlignment HorizontalTextAlignment
  {
    get => (HorizontalAlignment) this.GetValue(ImageAnnotation.HorizontalTextAlignmentProperty);
    set => this.SetValue(ImageAnnotation.HorizontalTextAlignmentProperty, (object) value);
  }

  public VerticalAlignment VerticalTextAlignment
  {
    get => (VerticalAlignment) this.GetValue(ImageAnnotation.VerticalTextAlignmentProperty);
    set => this.SetValue(ImageAnnotation.VerticalTextAlignmentProperty, (object) value);
  }

  internal double ImageWidth { get; set; }

  internal double ImageHeight { get; set; }

  public override void UpdateAnnotation()
  {
    if (this._image != null && this.X1 != null && this.Y1 != null)
    {
      Point point1 = new Point();
      RotateTransform rotateTransform = new RotateTransform()
      {
        Angle = this.Angle
      };
      Point point2 = new Point(0.0, 0.0);
      this.TextElement.Visibility = Visibility.Visible;
      switch (this.CoordinateUnit)
      {
        case CoordinateUnit.Pixel:
          Point point3 = new Point(Convert.ToDouble(this.X1), Convert.ToDouble(this.Y1));
          Point point2_1 = new Point(Convert.ToDouble(this.X2), Convert.ToDouble(this.Y2));
          Rect rect1 = new Rect(point3, point2_1);
          this._image.Height = rect1.Height;
          this._image.Width = rect1.Width;
          this.AnnotationElement.Height = rect1.Height;
          this.AnnotationElement.Width = rect1.Width;
          point2 = this.GetElementPosition((FrameworkElement) this._image, point3);
          Size desiredSize1 = new Size(this._image.Width, this._image.Height);
          this.TextElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
          if (this.X2 != null && this.Y2 != null)
          {
            Point textPosition = this.GetTextPosition(desiredSize1, new Point(0.0, 0.0), new Size(this.TextElement.DesiredSize.Width, this.TextElement.DesiredSize.Height));
            Canvas.SetLeft((UIElement) this.AnnotationElement, point2.X);
            Canvas.SetTop((UIElement) this.AnnotationElement, point2.Y);
            Canvas.SetLeft((UIElement) this.TextElement, textPosition.X);
            Canvas.SetTop((UIElement) this.TextElement, textPosition.Y);
          }
          else
          {
            this._image.Height = 0.0;
            this._image.Width = 0.0;
            if (this.ContentTemplate != null)
            {
              this.AnnotationElement.Height = this.TextElement.DesiredSize.Height;
              this.AnnotationElement.Width = this.TextElement.DesiredSize.Width;
              Point positionWithX1Y1 = this.GetTextPositionWithX1Y1(point3, new Size(this.TextElement.DesiredSize.Width, this.TextElement.DesiredSize.Height));
              Canvas.SetLeft((UIElement) this.TextElement, 0.0);
              Canvas.SetTop((UIElement) this.TextElement, 0.0);
              Canvas.SetLeft((UIElement) this.AnnotationElement, positionWithX1Y1.X);
              Canvas.SetTop((UIElement) this.AnnotationElement, positionWithX1Y1.Y);
            }
          }
          Rect rect2 = this.RotateElement(this.Angle, (FrameworkElement) this.AnnotationElement);
          this.AnnotationElement.RenderTransformOrigin = new Point(0.5, 0.5);
          this.AnnotationElement.RenderTransform = (Transform) rotateTransform;
          Point point4 = new Point(point2.X + this.AnnotationElement.Width / 2.0, point2.Y + this.AnnotationElement.Height / 2.0);
          if (this.Angle > 0.0)
          {
            this.RotatedRect = new Rect(point4.X - rect2.Width / 2.0, point4.Y - rect2.Height / 2.0, rect2.Width, rect2.Height);
            break;
          }
          this.RotatedRect = new Rect(point4.X - this.AnnotationElement.Width / 2.0, point4.Y - this.AnnotationElement.Height / 2.0, this.AnnotationElement.Width, this.AnnotationElement.Height);
          break;
        case CoordinateUnit.Axis:
          base.UpdateAnnotation();
          if (this.XAxis == null || this.YAxis == null)
            break;
          this.ImageHeight = Annotation.ConvertData(this.Y2, this.YAxis);
          this.ImageWidth = Annotation.ConvertData(this.X2, this.XAxis);
          if (this.CoordinateUnit == CoordinateUnit.Axis && this.EnableClipping)
          {
            this.x1 = this.GetClippingValues(this.x1, this.XAxis);
            this.y1 = this.GetClippingValues(this.y1, this.YAxis);
            this.ImageWidth = this.GetClippingValues(this.ImageWidth, this.XAxis);
            this.ImageHeight = this.GetClippingValues(this.ImageHeight, this.YAxis);
          }
          Point point5 = this.XAxis.Orientation == Orientation.Horizontal ? new Point(this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1), this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1)) : new Point(this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1), this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1));
          Point point2_2 = this.XAxis.Orientation == Orientation.Horizontal ? new Point(this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.ImageWidth), this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.ImageHeight)) : new Point(this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.ImageHeight), this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.ImageWidth));
          point5.Y = double.IsNaN(point5.Y) ? 0.0 : point5.Y;
          point5.X = double.IsNaN(point5.X) ? 0.0 : point5.X;
          point2_2.Y = double.IsNaN(point2_2.Y) ? 0.0 : point2_2.Y;
          point2_2.X = double.IsNaN(point2_2.X) ? 0.0 : point2_2.X;
          Rect rect3 = new Rect(point5, point2_2);
          this._image.Height = rect3.Height;
          this._image.Width = rect3.Width;
          this.AnnotationElement.Height = rect3.Height;
          this.AnnotationElement.Width = rect3.Width;
          Point originalPosition = this.EnsurePoint(point5, point2_2);
          Size desiredSize2 = new Size(rect3.Width, rect3.Height);
          point2 = this.GetElementPosition(new Size(rect3.Width, rect3.Height), originalPosition);
          this.TextElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
          if (this.X2 != null && this.Y2 != null)
          {
            Point textPosition = this.GetTextPosition(desiredSize2, new Point(0.0, 0.0), new Size(this.TextElement.DesiredSize.Width, this.TextElement.DesiredSize.Height));
            Canvas.SetLeft((UIElement) this.TextElement, textPosition.X);
            Canvas.SetTop((UIElement) this.TextElement, textPosition.Y);
            Canvas.SetLeft((UIElement) this.AnnotationElement, point2.X);
            Canvas.SetTop((UIElement) this.AnnotationElement, point2.Y);
          }
          else
          {
            this._image.Height = 0.0;
            this._image.Width = 0.0;
            if (this.ContentTemplate != null)
            {
              this.AnnotationElement.Height = this.TextElement.DesiredSize.Height;
              this.AnnotationElement.Width = this.TextElement.DesiredSize.Width;
              Point positionWithX1Y1 = this.GetTextPositionWithX1Y1(point5, new Size(this.TextElement.DesiredSize.Width, this.TextElement.DesiredSize.Height));
              Canvas.SetLeft((UIElement) this.TextElement, 0.0);
              Canvas.SetTop((UIElement) this.TextElement, 0.0);
              Canvas.SetLeft((UIElement) this.AnnotationElement, positionWithX1Y1.X);
              Canvas.SetTop((UIElement) this.AnnotationElement, positionWithX1Y1.Y);
            }
          }
          Point point6 = new Point(point2.X + this.AnnotationElement.Width / 2.0, point2.Y + this.AnnotationElement.Height / 2.0);
          Rect rect4 = this.RotateElement(this.Angle, (FrameworkElement) this.AnnotationElement);
          this.AnnotationElement.RenderTransformOrigin = new Point(0.5, 0.5);
          this.AnnotationElement.RenderTransform = (Transform) rotateTransform;
          if (this.Angle > 0.0)
          {
            this.RotatedRect = new Rect(point6.X - rect4.Width / 2.0, point6.Y - rect4.Height / 2.0, rect4.Width, rect4.Height);
            break;
          }
          this.RotatedRect = new Rect(point6.X - this.AnnotationElement.Width / 2.0, point6.Y - this.AnnotationElement.Height / 2.0, this.AnnotationElement.Width, this.AnnotationElement.Height);
          break;
      }
    }
    else
    {
      if (this._image == null)
        return;
      this._image.Height = 0.0;
      this._image.Width = 0.0;
      this.AnnotationElement.ClearValue(Canvas.LeftProperty);
      this.AnnotationElement.ClearValue(Canvas.TopProperty);
      this.TextElement.Visibility = Visibility.Collapsed;
    }
  }

  internal override UIElement CreateAnnotation()
  {
    if (this.AnnotationElement != null && this.AnnotationElement.Children.Count == 0)
    {
      this._image = new Image();
      this.SetBindings();
      this._image.Tag = (object) this;
      this.AnnotationElement.Children.Add((UIElement) this._image);
      this.TextElementCanvas.Children.Add((UIElement) this.TextElement);
      this.AnnotationElement.Children.Add((UIElement) this.TextElementCanvas);
    }
    return (UIElement) this.AnnotationElement;
  }

  internal void HeightWidthChanged()
  {
    if (this.CoordinateUnit == CoordinateUnit.Axis)
    {
      this.ImageHeight = Annotation.ConvertData(this.Y2, this.YAxis);
      this.ImageWidth = Annotation.ConvertData(this.X2, this.XAxis);
    }
    this.UpdateAnnotation();
    if (this.Chart == null || this.CoordinateUnit != CoordinateUnit.Axis || !this.CanUpdateRange(this.X2, this.Y2))
      return;
    this.Chart.ScheduleUpdate();
  }

  protected override void SetBindings()
  {
    base.SetBindings();
    this._image.Source = this.ImageSource;
    this._image.Stretch = Stretch.Fill;
    Binding binding = new Binding()
    {
      Path = new PropertyPath("Cursor", new object[0]),
      Source = (object) this
    };
    this._image.SetBinding(FrameworkElement.CursorProperty, (BindingBase) binding);
  }

  protected override DependencyObject CloneAnnotation(Annotation annotation)
  {
    return base.CloneAnnotation((Annotation) new ImageAnnotation()
    {
      Angle = this.Angle,
      HorizontalTextAlignment = this.HorizontalTextAlignment,
      ImageSource = this.ImageSource,
      VerticalTextAlignment = this.VerticalTextAlignment,
      X2 = this.X2,
      Y2 = this.Y2
    });
  }

  protected Point GetTextPosition(Size desiredSize, Point originalPosition)
  {
    Point textPosition = originalPosition;
    HorizontalAlignment horizontalTextAlignment = this.HorizontalTextAlignment;
    VerticalAlignment verticalTextAlignment = this.VerticalTextAlignment;
    Size size = new Size(this.TextElement.ActualWidth, this.TextElement.ActualHeight);
    switch (horizontalTextAlignment)
    {
      case HorizontalAlignment.Left:
        textPosition.X -= size.Width;
        break;
      case HorizontalAlignment.Center:
        textPosition.X += desiredSize.Width / 2.0;
        textPosition.X -= size.Width / 2.0;
        break;
      case HorizontalAlignment.Right:
        textPosition.X += desiredSize.Width;
        break;
    }
    switch (verticalTextAlignment)
    {
      case VerticalAlignment.Top:
        textPosition.Y -= size.Height;
        break;
      case VerticalAlignment.Center:
        textPosition.Y += desiredSize.Height / 2.0;
        textPosition.Y -= size.Height / 2.0;
        break;
      case VerticalAlignment.Bottom:
        textPosition.Y += desiredSize.Height;
        break;
    }
    return textPosition;
  }

  protected Point GetTextPosition(Size desiredSize, Point originalPosition, Size textSize)
  {
    Point textPosition = originalPosition;
    HorizontalAlignment horizontalTextAlignment = this.HorizontalTextAlignment;
    VerticalAlignment verticalTextAlignment = this.VerticalTextAlignment;
    switch (horizontalTextAlignment)
    {
      case HorizontalAlignment.Left:
        textPosition.X -= textSize.Width;
        break;
      case HorizontalAlignment.Center:
        textPosition.X += desiredSize.Width / 2.0;
        textPosition.X -= textSize.Width / 2.0;
        break;
      case HorizontalAlignment.Right:
        textPosition.X += desiredSize.Width;
        break;
    }
    switch (verticalTextAlignment)
    {
      case VerticalAlignment.Top:
        textPosition.Y -= textSize.Height;
        break;
      case VerticalAlignment.Center:
        textPosition.Y += desiredSize.Height / 2.0;
        textPosition.Y -= textSize.Height / 2.0;
        break;
      case VerticalAlignment.Bottom:
        textPosition.Y += desiredSize.Height;
        break;
    }
    return textPosition;
  }

  private static void OnUpdatePropertyChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(sender is Annotation annotation))
      return;
    annotation.UpdatePropertyChanged(args);
  }

  private static void OnImageSourceChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(sender is ImageAnnotation imageAnnotation) || imageAnnotation.XAxis == null || imageAnnotation.YAxis == null)
      return;
    imageAnnotation.SetBindings();
  }

  private static void OnHeightWidthChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(sender is ImageAnnotation imageAnnotation) || imageAnnotation.XAxis == null || imageAnnotation.YAxis == null)
      return;
    imageAnnotation.HeightWidthChanged();
  }

  private static void OnAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    Annotation.OnTextAlignmentChanged(d, e);
  }

  private Point GetTextPositionWithX1Y1(Point specifiedPoints, Size textSize)
  {
    Point positionWithX1Y1 = specifiedPoints;
    HorizontalAlignment horizontalTextAlignment = this.HorizontalTextAlignment;
    VerticalAlignment verticalTextAlignment = this.VerticalTextAlignment;
    switch (horizontalTextAlignment)
    {
      case HorizontalAlignment.Left:
        positionWithX1Y1.X -= textSize.Width;
        break;
      case HorizontalAlignment.Center:
        positionWithX1Y1.X -= textSize.Width / 2.0;
        break;
    }
    switch (verticalTextAlignment)
    {
      case VerticalAlignment.Top:
        positionWithX1Y1.Y -= textSize.Height;
        break;
      case VerticalAlignment.Center:
        positionWithX1Y1.Y -= textSize.Height / 2.0;
        break;
    }
    return positionWithX1Y1;
  }
}
