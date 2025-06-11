// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ShapeAnnotation
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class ShapeAnnotation : SingleAnnotation
{
  public static readonly DependencyProperty HorizontalTextAlignmentProperty = DependencyProperty.Register(nameof (HorizontalTextAlignment), typeof (HorizontalAlignment), typeof (ShapeAnnotation), new PropertyMetadata((object) HorizontalAlignment.Center, new PropertyChangedCallback(Annotation.OnTextAlignmentChanged)));
  public static readonly DependencyProperty DraggingModeProperty = DependencyProperty.Register(nameof (DraggingMode), typeof (AxisMode), typeof (ShapeAnnotation), new PropertyMetadata((object) AxisMode.All));
  public static readonly DependencyProperty CanDragProperty = DependencyProperty.Register(nameof (CanDrag), typeof (bool), typeof (ShapeAnnotation), new PropertyMetadata((object) false));
  public static readonly DependencyProperty CanResizeProperty = DependencyProperty.Register(nameof (CanResize), typeof (bool), typeof (ShapeAnnotation), new PropertyMetadata((object) false, new PropertyChangedCallback(ShapeAnnotation.OnCanResizeChanged)));
  public static readonly DependencyProperty VerticalTextAlignmentProperty = DependencyProperty.Register(nameof (VerticalTextAlignment), typeof (VerticalAlignment), typeof (ShapeAnnotation), new PropertyMetadata((object) VerticalAlignment.Bottom, new PropertyChangedCallback(ShapeAnnotation.OnAlignmentChanged)));
  public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof (Fill), typeof (Brush), typeof (ShapeAnnotation), new PropertyMetadata((object) null, new PropertyChangedCallback(ShapeAnnotation.OnFillChanged)));
  public static readonly DependencyProperty Y2Property = DependencyProperty.Register(nameof (Y2), typeof (object), typeof (ShapeAnnotation), new PropertyMetadata((object) null, new PropertyChangedCallback(ShapeAnnotation.OnHeightWidthChanged)));
  public static readonly DependencyProperty X2Property = DependencyProperty.Register(nameof (X2), typeof (object), typeof (ShapeAnnotation), new PropertyMetadata((object) null, new PropertyChangedCallback(ShapeAnnotation.OnHeightWidthChanged)));
  public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof (StrokeThickness), typeof (double), typeof (ShapeAnnotation), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(ShapeAnnotation.OnStrokeThicknessChanged)));
  public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof (Stroke), typeof (Brush), typeof (ShapeAnnotation), new PropertyMetadata((object) null, new PropertyChangedCallback(ShapeAnnotation.OnStrokeChanged)));
  public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(nameof (StrokeDashArray), typeof (DoubleCollection), typeof (ShapeAnnotation), new PropertyMetadata((object) null, new PropertyChangedCallback(ShapeAnnotation.OnStrokeDashArrayChanged)));
  public static readonly DependencyProperty StrokeDashCapProperty = DependencyProperty.Register(nameof (StrokeDashCap), typeof (PenLineCap), typeof (ShapeAnnotation), new PropertyMetadata((object) PenLineCap.Flat, new PropertyChangedCallback(ShapeAnnotation.OnStrokeDashCapChanged)));
  public static readonly DependencyProperty StrokeDashOffsetProperty = DependencyProperty.Register(nameof (StrokeDashOffset), typeof (double), typeof (ShapeAnnotation), new PropertyMetadata(Shape.StrokeDashOffsetProperty.GetMetadata(typeof (Shape)).DefaultValue));
  public static readonly DependencyProperty StrokeEndLineCapProperty = DependencyProperty.Register(nameof (StrokeEndLineCap), typeof (PenLineCap), typeof (ShapeAnnotation), new PropertyMetadata((object) PenLineCap.Flat, new PropertyChangedCallback(ShapeAnnotation.OnStrokeEndLineCapChanged)));
  public static readonly DependencyProperty StrokeLineJoinProperty = DependencyProperty.Register(nameof (StrokeLineJoin), typeof (PenLineJoin), typeof (ShapeAnnotation), new PropertyMetadata(Shape.StrokeLineJoinProperty.GetMetadata(typeof (Shape)).DefaultValue));
  public static readonly DependencyProperty StrokeMiterLimitProperty = DependencyProperty.Register(nameof (StrokeMiterLimit), typeof (double), typeof (ShapeAnnotation), new PropertyMetadata(Shape.StrokeMiterLimitProperty.GetMetadata(typeof (Shape)).DefaultValue));
  public static readonly DependencyProperty StrokeStartLineCapProperty = DependencyProperty.Register(nameof (StrokeStartLineCap), typeof (PenLineCap), typeof (ShapeAnnotation), new PropertyMetadata(Shape.StrokeStartLineCapProperty.GetMetadata(typeof (Shape)).DefaultValue));
  internal Thumb nearThumb;
  internal Thumb farThumb;
  internal Shape shape;
  protected double x2;
  protected double y2;

  private static void OnFillChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ShapeAnnotation shapeAnnotation) || shapeAnnotation.shape == null)
      return;
    shapeAnnotation.shape.Fill = (Brush) e.NewValue;
  }

  private static void OnStrokeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ShapeAnnotation shapeAnnotation) || shapeAnnotation.shape == null)
      return;
    shapeAnnotation.shape.Stroke = (Brush) e.NewValue;
  }

  private static void OnStrokeThicknessChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ShapeAnnotation shapeAnnotation) || shapeAnnotation.shape == null)
      return;
    shapeAnnotation.shape.StrokeThickness = (double) e.NewValue;
  }

  private static void OnStrokeDashArrayChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ShapeAnnotation shapeAnnotation) || shapeAnnotation.shape == null)
      return;
    shapeAnnotation.shape.StrokeDashArray = (DoubleCollection) e.NewValue;
  }

  private static void OnStrokeDashCapChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ShapeAnnotation shapeAnnotation) || shapeAnnotation.shape == null)
      return;
    shapeAnnotation.StrokeDashCap = (PenLineCap) e.NewValue;
  }

  private static void OnStrokeEndLineCapChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ShapeAnnotation shapeAnnotation) || shapeAnnotation.shape == null)
      return;
    shapeAnnotation.shape.StrokeEndLineCap = (PenLineCap) e.NewValue;
  }

  public ShapeAnnotation() => this.DefaultStyleKey = (object) typeof (ShapeAnnotation);

  public event EventHandler DragStarted;

  public event EventHandler<AnnotationDragDeltaEventArgs> DragDelta;

  public event EventHandler<AnnotationDragCompletedEventArgs> DragCompleted;

  public HorizontalAlignment HorizontalTextAlignment
  {
    get => (HorizontalAlignment) this.GetValue(ShapeAnnotation.HorizontalTextAlignmentProperty);
    set => this.SetValue(ShapeAnnotation.HorizontalTextAlignmentProperty, (object) value);
  }

  public AxisMode DraggingMode
  {
    get => (AxisMode) this.GetValue(ShapeAnnotation.DraggingModeProperty);
    set => this.SetValue(ShapeAnnotation.DraggingModeProperty, (object) value);
  }

  public bool CanDrag
  {
    get => (bool) this.GetValue(ShapeAnnotation.CanDragProperty);
    set => this.SetValue(ShapeAnnotation.CanDragProperty, (object) value);
  }

  public bool CanResize
  {
    get => (bool) this.GetValue(ShapeAnnotation.CanResizeProperty);
    set => this.SetValue(ShapeAnnotation.CanResizeProperty, (object) value);
  }

  public VerticalAlignment VerticalTextAlignment
  {
    get => (VerticalAlignment) this.GetValue(ShapeAnnotation.VerticalTextAlignmentProperty);
    set => this.SetValue(ShapeAnnotation.VerticalTextAlignmentProperty, (object) value);
  }

  public Brush Fill
  {
    get => (Brush) this.GetValue(ShapeAnnotation.FillProperty);
    set => this.SetValue(ShapeAnnotation.FillProperty, (object) value);
  }

  public object Y2
  {
    get => this.GetValue(ShapeAnnotation.Y2Property);
    set => this.SetValue(ShapeAnnotation.Y2Property, value);
  }

  public object X2
  {
    get => this.GetValue(ShapeAnnotation.X2Property);
    set => this.SetValue(ShapeAnnotation.X2Property, value);
  }

  public double StrokeThickness
  {
    get => (double) this.GetValue(ShapeAnnotation.StrokeThicknessProperty);
    set => this.SetValue(ShapeAnnotation.StrokeThicknessProperty, (object) value);
  }

  public Brush Stroke
  {
    get => (Brush) this.GetValue(ShapeAnnotation.StrokeProperty);
    set => this.SetValue(ShapeAnnotation.StrokeProperty, (object) value);
  }

  public DoubleCollection StrokeDashArray
  {
    get => (DoubleCollection) this.GetValue(ShapeAnnotation.StrokeDashArrayProperty);
    set => this.SetValue(ShapeAnnotation.StrokeDashArrayProperty, (object) value);
  }

  public PenLineCap StrokeDashCap
  {
    get => (PenLineCap) this.GetValue(ShapeAnnotation.StrokeDashCapProperty);
    set => this.SetValue(ShapeAnnotation.StrokeDashCapProperty, (object) value);
  }

  public double StrokeDashOffset
  {
    get => (double) this.GetValue(ShapeAnnotation.StrokeDashOffsetProperty);
    set => this.SetValue(ShapeAnnotation.StrokeDashOffsetProperty, (object) value);
  }

  public PenLineCap StrokeEndLineCap
  {
    get => (PenLineCap) this.GetValue(ShapeAnnotation.StrokeEndLineCapProperty);
    set => this.SetValue(ShapeAnnotation.StrokeEndLineCapProperty, (object) value);
  }

  public PenLineJoin StrokeLineJoin
  {
    get => (PenLineJoin) this.GetValue(ShapeAnnotation.StrokeLineJoinProperty);
    set => this.SetValue(ShapeAnnotation.StrokeLineJoinProperty, (object) value);
  }

  public double StrokeMiterLimit
  {
    get => (double) this.GetValue(ShapeAnnotation.StrokeMiterLimitProperty);
    set => this.SetValue(ShapeAnnotation.StrokeMiterLimitProperty, (object) value);
  }

  public PenLineCap StrokeStartLineCap
  {
    get => (PenLineCap) this.GetValue(ShapeAnnotation.StrokeStartLineCapProperty);
    set => this.SetValue(ShapeAnnotation.StrokeStartLineCapProperty, (object) value);
  }

  internal bool IsDragging { get; set; }

  internal Resizer ResizerControl { get; set; }

  public override void UpdateAnnotation()
  {
    if ((this.shape != null || this.ResizerControl != null) && this.X1 != null && this.Y1 != null && this.X2 != null && this.Y2 != null)
    {
      switch (this.CoordinateUnit)
      {
        case CoordinateUnit.Pixel:
          this.UpdatePixelAnnotation(new Point(Convert.ToDouble(this.X1), Convert.ToDouble(this.Y1)), new Point(Convert.ToDouble(this.X2), Convert.ToDouble(this.Y2)));
          break;
        case CoordinateUnit.Axis:
          base.UpdateAnnotation();
          if (this.XAxis != null && this.YAxis != null)
          {
            this.y2 = Annotation.ConvertData(this.Y2, this.YAxis);
            this.x2 = Annotation.ConvertData(this.X2, this.XAxis);
            if (this.CoordinateUnit == CoordinateUnit.Axis && this.EnableClipping)
            {
              this.x1 = this.GetClippingValues(this.x1, this.XAxis);
              this.y1 = this.GetClippingValues(this.y1, this.YAxis);
              this.x2 = this.GetClippingValues(this.x2, this.XAxis);
              this.y2 = this.GetClippingValues(this.y2, this.YAxis);
            }
            this.UpdateAxisAnnotation(this.XAxis.Orientation == Orientation.Horizontal ? new Point(this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1), this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1)) : new Point(this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1), this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1)), this.XAxis.Orientation == Orientation.Horizontal ? new Point(this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x2), this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y2)) : new Point(this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y2), this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x2)));
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

  internal void HeightWidthChanged()
  {
    if (this.CoordinateUnit == CoordinateUnit.Axis)
    {
      this.y2 = Annotation.ConvertData(this.Y2, this.YAxis);
      this.x2 = Annotation.ConvertData(this.X2, this.XAxis);
    }
    this.UpdateAnnotation();
    if (this.Chart == null || this.CoordinateUnit != CoordinateUnit.Axis || !this.CanUpdateRange(this.X2, this.Y2))
      return;
    this.Chart.ScheduleUpdate();
  }

  internal void UpdateAxisAnnotation(Point point, Point point2)
  {
    Point point1 = new Point();
    double angle = (this as SolidShapeAnnotation).Angle;
    RotateTransform rotateTransform = new RotateTransform()
    {
      Angle = angle
    };
    Point point3 = new Point(0.0, 0.0);
    point.Y = double.IsNaN(point.Y) ? 0.0 : point.Y;
    point.X = double.IsNaN(point.X) ? 0.0 : point.X;
    point2.Y = double.IsNaN(point2.Y) ? 0.0 : point2.Y;
    point2.X = double.IsNaN(point2.X) ? 0.0 : point2.X;
    Rect rect1 = new Rect(point, point2);
    if (this.shape != null)
    {
      this.shape.Height = rect1.Height;
      this.shape.Width = rect1.Width;
    }
    else
    {
      this.ResizerControl.Height = rect1.Height;
      this.ResizerControl.Width = rect1.Width;
    }
    this.AnnotationElement.Height = rect1.Height;
    this.AnnotationElement.Width = rect1.Width;
    Point originalPosition = this.EnsurePoint(point, point2);
    Size desiredSize = new Size(rect1.Width, rect1.Height);
    Point elementPosition = this.GetElementPosition(new Size(rect1.Width, rect1.Height), originalPosition);
    this.TextElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    Point textPosition = this.GetTextPosition(desiredSize, new Point(0.0, 0.0), new Size(this.TextElement.DesiredSize.Width, this.TextElement.DesiredSize.Height));
    Canvas.SetLeft((UIElement) this.AnnotationElement, elementPosition.X);
    Canvas.SetTop((UIElement) this.AnnotationElement, elementPosition.Y);
    Canvas.SetLeft((UIElement) this.TextElement, textPosition.X);
    Canvas.SetTop((UIElement) this.TextElement, textPosition.Y);
    Point point4 = new Point(elementPosition.X + this.AnnotationElement.Width / 2.0, elementPosition.Y + this.AnnotationElement.Height / 2.0);
    Rect rect2 = this.RotateElement(angle, (FrameworkElement) this.AnnotationElement, new Size(this.AnnotationElement.Width, this.AnnotationElement.Height));
    this.AnnotationElement.RenderTransformOrigin = new Point(0.5, 0.5);
    this.AnnotationElement.RenderTransform = (Transform) rotateTransform;
    if (angle > 0.0)
      this.RotatedRect = new Rect(point4.X - rect2.Width / 2.0, point4.Y - rect2.Height / 2.0, rect2.Width, rect2.Height);
    else
      this.RotatedRect = new Rect(point4.X - this.AnnotationElement.Width / 2.0, point4.Y - this.AnnotationElement.Height / 2.0, this.AnnotationElement.Width, this.AnnotationElement.Height);
  }

  internal void UpdatePixelAnnotation(Point elementPoint1, Point elementPoint2)
  {
    Point point1 = new Point();
    double angle = (this as SolidShapeAnnotation).Angle;
    RotateTransform rotateTransform = new RotateTransform()
    {
      Angle = angle
    };
    Point point2 = new Point(0.0, 0.0);
    Rect rect1 = new Rect(elementPoint1, elementPoint2);
    if (this.shape != null)
    {
      this.shape.Height = rect1.Height;
      this.shape.Width = rect1.Width;
    }
    else
    {
      this.ResizerControl.Height = rect1.Height;
      this.ResizerControl.Width = rect1.Width;
    }
    this.AnnotationElement.Height = rect1.Height;
    this.AnnotationElement.Width = rect1.Width;
    Point originalPosition = this.EnsurePoint(elementPoint1, elementPoint2);
    Point point3 = this.shape != null ? this.GetElementPosition((FrameworkElement) this.shape, originalPosition) : this.GetElementPosition((FrameworkElement) this.ResizerControl, originalPosition);
    Size desiredSize = this.shape != null ? new Size(this.shape.Width, this.shape.Height) : new Size(this.ResizerControl.Width, this.ResizerControl.Height);
    this.TextElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    Point textPosition = this.GetTextPosition(desiredSize, new Point(0.0, 0.0), new Size(this.TextElement.DesiredSize.Width, this.TextElement.DesiredSize.Height));
    Canvas.SetLeft((UIElement) this.AnnotationElement, point3.X);
    Canvas.SetTop((UIElement) this.AnnotationElement, point3.Y);
    Canvas.SetLeft((UIElement) this.TextElement, textPosition.X);
    Canvas.SetTop((UIElement) this.TextElement, textPosition.Y);
    Point point4 = new Point(point3.X + this.AnnotationElement.Width / 2.0, point3.Y + this.AnnotationElement.Height / 2.0);
    Rect rect2 = this.RotateElement(angle, (FrameworkElement) this.AnnotationElement, new Size(this.AnnotationElement.Width, this.AnnotationElement.Height));
    this.AnnotationElement.RenderTransformOrigin = new Point(0.5, 0.5);
    this.AnnotationElement.RenderTransform = (Transform) rotateTransform;
    if (angle > 0.0)
      this.RotatedRect = new Rect(point4.X - rect2.Width / 2.0, point4.Y - rect2.Height / 2.0, rect2.Width, rect2.Height);
    else
      this.RotatedRect = new Rect(point4.X - this.AnnotationElement.Width / 2.0, point4.Y - this.AnnotationElement.Height / 2.0, this.AnnotationElement.Width, this.AnnotationElement.Height);
  }

  internal void CheckResizerValues()
  {
    if (this.Chart == null || this.Chart.AnnotationManager == null || this.Chart.AnnotationManager.SelectedAnnotation != this || !this.CanResize || this.shape == null || this.IsDragging)
      return;
    AnnotationResizer annotationResizer = this.Chart.AnnotationManager.AnnotationResizer;
    if (annotationResizer.X1 == null && annotationResizer.Y1 == null && annotationResizer.X2 == null && annotationResizer.Y2 == null)
    {
      annotationResizer.X1 = this.X1;
      annotationResizer.X2 = this.X2;
      annotationResizer.Y1 = this.Y1;
      annotationResizer.Y2 = this.Y2;
      annotationResizer.MapActualValueToPixels();
    }
    annotationResizer.InternalHorizontalAlignment = this.HorizontalAlignment;
    annotationResizer.InternalVerticalAlignment = this.VerticalAlignment;
  }

  internal void ClearAnnotationElements()
  {
    this.AnnotationElement.ClearValue(Canvas.LeftProperty);
    this.AnnotationElement.ClearValue(Canvas.TopProperty);
    this.AnnotationElement.Width = 0.0;
    this.AnnotationElement.Height = 0.0;
    if (this.TextElement.Content != null)
    {
      this.TextElement.ClearValue(Canvas.LeftProperty);
      this.TextElement.ClearValue(Canvas.TopProperty);
    }
    if (this.Chart == null || this.Chart.AnnotationManager == null || this.Chart.AnnotationManager.SelectedAnnotation == null || !this.Chart.AnnotationManager.SelectedAnnotation.Equals((object) this) || !this.CanResize || this.X1 != null || this.Y1 != null || this.X2 != null || this.Y2 != null)
      return;
    AnnotationResizer annotationResizer = this.Chart.AnnotationManager.AnnotationResizer;
    annotationResizer.ClearValue(Annotation.X1Property);
    annotationResizer.ClearValue(ShapeAnnotation.X2Property);
    annotationResizer.ClearValue(Annotation.Y1Property);
    annotationResizer.ClearValue(ShapeAnnotation.Y2Property);
  }

  protected internal virtual void OnDragDelta(AnnotationDragDeltaEventArgs args)
  {
    if (this.DragDelta == null)
      return;
    this.DragDelta((object) this, args);
  }

  protected internal virtual void OnDragCompleted(AnnotationDragCompletedEventArgs args)
  {
    if (this.DragCompleted == null)
      return;
    this.DragCompleted((object) this, args);
  }

  protected internal virtual void OnDragStarted(EventArgs args)
  {
    if (this.DragStarted == null)
      return;
    this.DragStarted((object) this, args);
  }

  protected override void SetBindings()
  {
    base.SetBindings();
    Binding binding1 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Fill", new object[0])
    };
    this.shape.SetBinding(Shape.FillProperty, (BindingBase) binding1);
    Binding binding2 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Opacity", new object[0])
    };
    this.shape.SetBinding(UIElement.OpacityProperty, (BindingBase) binding2);
    Binding binding3 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Stroke", new object[0])
    };
    this.shape.SetBinding(Shape.StrokeProperty, (BindingBase) binding3);
    Binding binding4 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeThickness", new object[0])
    };
    this.shape.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) binding4);
    DoubleCollection strokeDashArray = this.StrokeDashArray;
    DoubleCollection doubleCollection = new DoubleCollection();
    if (strokeDashArray != null && strokeDashArray.Count > 0)
    {
      foreach (double num in strokeDashArray)
        doubleCollection.Add(num);
      this.shape.StrokeDashArray = doubleCollection;
    }
    Binding binding5 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeDashCap", new object[0])
    };
    this.shape.SetBinding(Shape.StrokeDashCapProperty, (BindingBase) binding5);
    Binding binding6 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeDashOffset", new object[0])
    };
    this.shape.SetBinding(Shape.StrokeDashOffsetProperty, (BindingBase) binding6);
    Binding binding7 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeEndLineCap", new object[0])
    };
    this.shape.SetBinding(Shape.StrokeEndLineCapProperty, (BindingBase) binding7);
    Binding binding8 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeLineJoin", new object[0])
    };
    this.shape.SetBinding(Shape.StrokeLineJoinProperty, (BindingBase) binding8);
    Binding binding9 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeMiterLimit", new object[0])
    };
    this.shape.SetBinding(Shape.StrokeMiterLimitProperty, (BindingBase) binding9);
    Binding binding10 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeStartLineCap", new object[0])
    };
    this.shape.SetBinding(Shape.StrokeStartLineCapProperty, (BindingBase) binding10);
    Binding binding11 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Cursor", new object[0])
    };
    this.shape.SetBinding(FrameworkElement.CursorProperty, (BindingBase) binding11);
  }

  protected override DependencyObject CloneAnnotation(Annotation annotation)
  {
    ShapeAnnotation shapeAnnotation = annotation as ShapeAnnotation;
    if (shapeAnnotation is SolidShapeAnnotation solidShapeAnnotation)
      solidShapeAnnotation.Angle = (this as SolidShapeAnnotation).Angle;
    shapeAnnotation.Fill = this.Fill;
    shapeAnnotation.HorizontalTextAlignment = this.HorizontalTextAlignment;
    shapeAnnotation.Stroke = this.Stroke;
    shapeAnnotation.StrokeDashArray = this.StrokeDashArray;
    shapeAnnotation.StrokeDashCap = this.StrokeDashCap;
    shapeAnnotation.StrokeDashOffset = this.StrokeDashOffset;
    shapeAnnotation.StrokeEndLineCap = this.StrokeEndLineCap;
    shapeAnnotation.StrokeLineJoin = this.StrokeLineJoin;
    shapeAnnotation.StrokeMiterLimit = this.StrokeMiterLimit;
    shapeAnnotation.StrokeStartLineCap = this.StrokeStartLineCap;
    shapeAnnotation.StrokeThickness = this.StrokeThickness;
    shapeAnnotation.VerticalTextAlignment = this.VerticalTextAlignment;
    shapeAnnotation.X2 = this.X2;
    shapeAnnotation.Y2 = this.Y2;
    shapeAnnotation.CanDrag = this.CanDrag;
    shapeAnnotation.CanResize = this.CanResize;
    return base.CloneAnnotation((Annotation) shapeAnnotation);
  }

  protected virtual Point GetTextPosition(Size desiredSize, Point originalPosition, Size textSize)
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

  private static void OnCanResizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ShapeAnnotation shapeAnnotation = d as ShapeAnnotation;
    if (e.NewValue == e.OldValue || shapeAnnotation == null || shapeAnnotation.Chart == null)
      return;
    shapeAnnotation.UpdateResizer((bool) e.NewValue);
  }

  private static void OnAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    Annotation.OnTextAlignmentChanged(d, e);
  }

  private static void OnHeightWidthChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    if (args.OldValue != null && args.OldValue.Equals(args.NewValue) || !(sender is ShapeAnnotation shapeAnnotation) || shapeAnnotation.XAxis == null && shapeAnnotation.CoordinateUnit != CoordinateUnit.Pixel || shapeAnnotation.YAxis == null)
      return;
    if (shapeAnnotation is AnnotationResizer && args.OldValue != null)
    {
      (shapeAnnotation.Chart.AnnotationManager.SelectedAnnotation as ShapeAnnotation).X2 = shapeAnnotation.X2;
      (shapeAnnotation.Chart.AnnotationManager.SelectedAnnotation as ShapeAnnotation).Y2 = shapeAnnotation.Y2;
    }
    shapeAnnotation.HeightWidthChanged();
  }

  private void UpdateResizer(bool value)
  {
    if (value && this.Chart.AnnotationManager.SelectedAnnotation != null)
      this.Chart.AnnotationManager.OnAnnotationSelected();
    else if (this.Chart.AnnotationManager.AnnotationResizer != null)
    {
      this.Chart.AnnotationManager.AddOrRemoveAnnotationResizer((Annotation) this.Chart.AnnotationManager.AnnotationResizer, true);
      this.Chart.AnnotationManager.AnnotationResizer = (AnnotationResizer) null;
    }
    else
    {
      if (!(this.Chart.AnnotationManager.SelectedAnnotation is LineAnnotation))
        return;
      this.Chart.AnnotationManager.HideLineResizer();
    }
  }
}
