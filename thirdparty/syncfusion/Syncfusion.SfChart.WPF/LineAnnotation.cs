// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.LineAnnotation
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

public class LineAnnotation : ShapeAnnotation
{
  public static readonly DependencyProperty GrabExtentProperty = DependencyProperty.Register(nameof (GrabExtent), typeof (double), typeof (LineAnnotation), new PropertyMetadata((object) 5.0, new PropertyChangedCallback(LineAnnotation.OnGrabExtentChanged)));
  public static readonly DependencyProperty ShowLineProperty = DependencyProperty.Register(nameof (ShowLine), typeof (bool), typeof (LineAnnotation), new PropertyMetadata((object) true, new PropertyChangedCallback(LineAnnotation.OnShowLinePropertyChanged)));
  public static readonly DependencyProperty LineCapProperty = DependencyProperty.Register(nameof (LineCap), typeof (LineCap), typeof (LineAnnotation), new PropertyMetadata((object) LineCap.None));
  protected ArrowLine arrowLine;
  private double minimumSize;
  private AnnotationManager annotationManager;
  private bool isRotated;
  private RotateTransform rotate;
  private Point[] hitRectPoints;

  public double GrabExtent
  {
    get => (double) this.GetValue(LineAnnotation.GrabExtentProperty);
    set => this.SetValue(LineAnnotation.GrabExtentProperty, (object) value);
  }

  public bool ShowLine
  {
    get => (bool) this.GetValue(LineAnnotation.ShowLineProperty);
    set => this.SetValue(LineAnnotation.ShowLineProperty, (object) value);
  }

  public LineCap LineCap
  {
    get => (LineCap) this.GetValue(LineAnnotation.LineCapProperty);
    set => this.SetValue(LineAnnotation.LineCapProperty, (object) value);
  }

  internal Canvas LineCanvas { get; set; }

  internal Point[] LinePoints { get; set; }

  protected internal double HorizontalChange { get; set; }

  protected internal double VerticalChange { get; set; }

  private bool IsWithCap => this.LineCap == LineCap.Arrow;

  private bool IsAxis => this.CoordinateUnit == CoordinateUnit.Axis;

  private bool IsVertical => !(this is HorizontalLineAnnotation);

  private bool IsHorizontal => !(this is VerticalLineAnnotation);

  private double ActualX1
  {
    get
    {
      return this.IsAxis ? this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1) : Convert.ToDouble(this.X1);
    }
    set => this.X1 = (object) value;
  }

  private double ActualX2
  {
    get
    {
      return this.IsAxis ? this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x2) : Convert.ToDouble(this.X2);
    }
    set => this.X2 = (object) value;
  }

  private double ActualY1
  {
    get
    {
      return this.IsAxis ? this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1) : Convert.ToDouble(this.Y1);
    }
    set => this.Y1 = (object) value;
  }

  private double ActualY2
  {
    get
    {
      return this.IsAxis ? this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y2) : Convert.ToDouble(this.Y2);
    }
    set => this.Y2 = (object) value;
  }

  public override void UpdateAnnotation()
  {
    if (this.shape != null && this.X1 != null && this.X2 != null && this.Y1 != null && this.Y2 != null)
    {
      this.ValidateSelection();
      switch (this.CoordinateUnit)
      {
        case CoordinateUnit.Pixel:
          if (!this.ShowLine)
            break;
          this.DrawLine(new Point(Convert.ToDouble(this.X1), Convert.ToDouble(this.Y1)), new Point(Convert.ToDouble(this.X2), Convert.ToDouble(this.Y2)), this.shape);
          break;
        case CoordinateUnit.Axis:
          this.SetAxisFromName();
          if (this.XAxis == null || this.YAxis == null || !this.ShowLine)
            break;
          this.x1 = Annotation.ConvertData(this.X1, this.XAxis);
          this.y1 = Annotation.ConvertData(this.Y1, this.YAxis);
          this.y2 = Annotation.ConvertData(this.Y2, this.YAxis);
          this.x2 = Annotation.ConvertData(this.X2, this.XAxis);
          if (this.CoordinateUnit == CoordinateUnit.Axis && this.EnableClipping)
          {
            this.x1 = this.GetClippingValues(this.x1, this.XAxis);
            this.y1 = this.GetClippingValues(this.y1, this.YAxis);
            this.x2 = this.GetClippingValues(this.x2, this.XAxis);
            this.y2 = this.GetClippingValues(this.y2, this.YAxis);
          }
          this.DrawLine(this.XAxis.Orientation == Orientation.Horizontal ? new Point(this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1), this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1)) : new Point(this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1), this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1)), this.XAxis.Orientation == Orientation.Horizontal ? new Point(this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x2), this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y2)) : new Point(this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y2), this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x2)), this.shape);
          break;
      }
    }
    else
    {
      if (this.shape == null)
        return;
      this.ClearValues();
    }
  }

  internal override UIElement CreateAnnotation()
  {
    if (this.AnnotationElement != null && this.AnnotationElement.Children.Count == 0)
    {
      this.LineCanvas = new Canvas();
      if (this.IsWithCap)
      {
        this.shape = (Shape) new Path();
        this.arrowLine = new ArrowLine();
      }
      else
        this.shape = (Shape) new Line();
      this.shape.Tag = (object) this;
      if (this.ShowLine)
      {
        if (this.CanResize)
          this.AddThumb();
        this.LineCanvas.Children.Add((UIElement) this.TextElement);
        this.LineCanvas.Children.Add((UIElement) this.shape);
      }
      this.SetBindings();
      this.AnnotationElement.Children.Add((UIElement) this.LineCanvas);
    }
    return (UIElement) this.AnnotationElement;
  }

  internal void AddThumb()
  {
    this.nearThumb = new Thumb();
    this.farThumb = new Thumb();
    this.nearThumb.Style = ChartDictionaries.GenericCommonDictionary[(object) "roundthumbstyle"] as Style;
    this.farThumb.Style = ChartDictionaries.GenericCommonDictionary[(object) "roundthumbstyle"] as Style;
    this.nearThumb.Visibility = Visibility.Collapsed;
    this.farThumb.Visibility = Visibility.Collapsed;
    this.nearThumb.Margin = new Thickness().GetThickness(-5.0, -5.0, -5.0, -5.0);
    this.farThumb.Margin = new Thickness().GetThickness(-5.0, -5.0, -5.0, -5.0);
    this.LineCanvas.Children.Add((UIElement) this.nearThumb);
    this.LineCanvas.Children.Add((UIElement) this.farThumb);
    this.nearThumb.DragDelta += new DragDeltaEventHandler(this.OnNearThumbDragDelta);
    this.farThumb.DragDelta += new DragDeltaEventHandler(this.OnFarThumbDragDelta);
    Panel.SetZIndex((UIElement) this.nearThumb, 1);
    Panel.SetZIndex((UIElement) this.farThumb, 1);
    this.nearThumb.DragCompleted += new DragCompletedEventHandler(this.OnDragCompleted);
    this.farThumb.DragCompleted += new DragCompletedEventHandler(this.OnDragCompleted);
  }

  internal void ValidateSelection()
  {
    if (!this.CanResize || this.Chart == null || this.Chart.AnnotationManager == null || this.Chart.AnnotationManager.SelectedAnnotation == null || !this.Chart.AnnotationManager.SelectedAnnotation.Equals((object) this))
      return;
    this.nearThumb.Visibility = Visibility.Visible;
    this.farThumb.Visibility = Visibility.Visible;
  }

  internal void ClearValues()
  {
    if (this.ShowLine)
      this.shape.ClearUIValues();
    if (this.TextElement.Content != null)
    {
      this.TextElement.ClearValue(Canvas.LeftProperty);
      this.TextElement.ClearValue(Canvas.TopProperty);
    }
    if (!this.CanResize)
      return;
    this.nearThumb.Visibility = Visibility.Collapsed;
    this.farThumb.Visibility = Visibility.Collapsed;
  }

  internal virtual void UpdateHitArea()
  {
    if (this.LinePoints == null)
      return;
    this.hitRectPoints = this.GetHitRectPoints(this.LinePoints[0], this.LinePoints[1]);
  }

  internal virtual bool IsPointInsideRectangle(Point point)
  {
    return this.hitRectPoints != null && ChartMath.IsPointInsideRectangle(point, this.hitRectPoints);
  }

  protected virtual void OnDragCompleted(object sender, DragCompletedEventArgs e)
  {
    this.IsResizing = false;
    this.Chart.AnnotationManager.RaiseDragCompleted();
  }

  protected override DependencyObject CloneAnnotation(Annotation annotation)
  {
    return base.CloneAnnotation((Annotation) new LineAnnotation()
    {
      ShowLine = this.ShowLine,
      LineCap = this.LineCap
    });
  }

  protected void DrawLine(Point point, Point point2, Shape shape)
  {
    Point point1 = new Point(0.0, 0.0);
    Point point3 = new Point(0.0, 0.0);
    Path path = (Path) null;
    Line line = (Line) null;
    if (this.IsWithCap)
      path = shape as Path;
    else
      line = shape as Line;
    point.Y = double.IsNaN(point.Y) ? 0.0 : point.Y;
    point.X = double.IsNaN(point.X) ? 0.0 : point.X;
    point2.Y = double.IsNaN(point2.Y) ? 0.0 : point2.Y;
    point2.X = double.IsNaN(point2.X) ? 0.0 : point2.X;
    Rect rect = new Rect(point, point2);
    Point originalPosition = this.EnsurePoint(point, point2);
    Size desiredSize = new Size(rect.Width, rect.Height);
    Point elementPosition = this.GetElementPosition(new Size(rect.Width, rect.Height), originalPosition);
    if (this.IsWithCap)
    {
      this.arrowLine.X1 = point.X;
      this.arrowLine.Y1 = point.Y;
      this.arrowLine.X2 = point2.X;
      this.arrowLine.Y2 = point2.Y;
      path.Data = (Geometry) (this.arrowLine.GetGeometry() as PathGeometry);
    }
    else
    {
      line.X1 = point.X;
      line.Y1 = point.Y;
      line.X2 = point2.X;
      line.Y2 = point2.Y;
    }
    if (this.CanResize && this.farThumb != null && this.nearThumb != null)
    {
      Canvas.SetLeft((UIElement) this.farThumb, point.X);
      Canvas.SetTop((UIElement) this.farThumb, point.Y);
      Canvas.SetLeft((UIElement) this.nearThumb, point2.X);
      Canvas.SetTop((UIElement) this.nearThumb, point2.Y);
    }
    this.AnnotationElement.Width = rect.Width;
    this.AnnotationElement.Height = rect.Height;
    point3 = new Point(elementPosition.X + rect.Width / 2.0, elementPosition.Y + rect.Height / 2.0);
    this.minimumSize = this.GrabExtent * 2.0;
    double height = rect.Height < this.minimumSize ? this.minimumSize : rect.Height;
    double width = rect.Width < this.minimumSize ? this.minimumSize : rect.Width;
    this.RotatedRect = new Rect(point3.X - width / 2.0, point3.Y - height / 2.0, width, height);
    this.LinePoints = new Point[2]{ point, point2 };
    switch (this)
    {
      case HorizontalLineAnnotation _:
      case VerticalLineAnnotation _:
label_10:
        if (this.TextElement.Content != null)
          this.SetTextElementPosition(point, point2, desiredSize, elementPosition, this.TextElement);
        if (this.LineCanvas == null || !this.IsDragging)
          break;
        this.LineCanvas.UpdateLayout();
        break;
      default:
        this.UpdateHitArea();
        goto label_10;
    }
  }

  protected virtual void SetTextElementPosition(
    Point point,
    Point point2,
    Size desiredSize,
    Point positionedPoint,
    ContentControl textElement)
  {
    this.rotate = new RotateTransform();
    textElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    double d = (point2.Y - point.Y) / (point2.X - point.X);
    this.rotate.Angle = double.IsNaN(d) ? 0.0 : Math.Atan(d) * (180.0 / Math.PI);
    textElement.RenderTransformOrigin = new Point(0.5, 0.5);
    textElement.RenderTransform = (Transform) this.rotate;
    Point textPosition = this.GetTextPosition(desiredSize, positionedPoint, new Size(textElement.DesiredSize.Width, textElement.DesiredSize.Height));
    Canvas.SetLeft((UIElement) textElement, textPosition.X);
    Canvas.SetTop((UIElement) textElement, textPosition.Y);
  }

  protected override Point GetTextPosition(Size desiredSize, Point originalPosition, Size textSize)
  {
    Point point = originalPosition;
    return this.XAxis == null || this.XAxis.Orientation != Orientation.Horizontal ? this.CalculateVerticalPosition(desiredSize, point, textSize) : this.CalculateHorizontalPosition(desiredSize, point, textSize);
  }

  private Point CalculateHorizontalPosition(Size desiredSize, Point point, Size textSize)
  {
    HorizontalAlignment horizontalTextAlignment = this.HorizontalTextAlignment;
    VerticalAlignment verticalTextAlignment = this.VerticalTextAlignment;
    switch (horizontalTextAlignment)
    {
      case HorizontalAlignment.Center:
        point.X += desiredSize.Width / 2.0;
        if (Math.Abs(this.rotate.Angle) < 80.0)
        {
          point.X -= textSize.Width / 2.0;
          break;
        }
        break;
      case HorizontalAlignment.Right:
        point.X += desiredSize.Width - textSize.Width;
        break;
    }
    switch (verticalTextAlignment)
    {
      case VerticalAlignment.Top:
        point.Y += desiredSize.Height / 2.0;
        point.Y -= textSize.Height;
        this.TextElement.Margin = new Thickness().GetThickness(0.0, 0.0, 0.0, this.minimumSize);
        break;
      case VerticalAlignment.Center:
        point.Y += desiredSize.Height / 2.0;
        point.Y -= textSize.Height / 2.0;
        break;
      case VerticalAlignment.Bottom:
        point.Y += desiredSize.Height / 2.0;
        point.Y -= textSize.Height / 8.0;
        this.TextElement.Margin = new Thickness().GetThickness(0.0, this.minimumSize, 0.0, 0.0);
        break;
    }
    return point;
  }

  private Point CalculateVerticalPosition(Size desiredSize, Point point, Size textSize)
  {
    HorizontalAlignment horizontalTextAlignment = this.HorizontalTextAlignment;
    VerticalAlignment verticalTextAlignment = this.VerticalTextAlignment;
    switch (horizontalTextAlignment)
    {
      case HorizontalAlignment.Left:
        point.Y += textSize.Width / 2.0;
        break;
      case HorizontalAlignment.Center:
        point.Y += desiredSize.Height / 2.0;
        point.Y -= textSize.Height / 2.0;
        break;
      case HorizontalAlignment.Right:
        point.Y += desiredSize.Height + textSize.Height;
        point.Y -= textSize.Width;
        break;
    }
    switch (verticalTextAlignment)
    {
      case VerticalAlignment.Top:
        point.X -= textSize.Height;
        break;
      case VerticalAlignment.Center:
        point.X -= textSize.Width / 2.0;
        if (Math.Abs(this.rotate.Angle) < 80.0)
        {
          point.X -= textSize.Height / 2.0;
          break;
        }
        break;
      case VerticalAlignment.Bottom:
        point.X -= textSize.Height;
        point.X -= textSize.Width / 2.0;
        break;
    }
    return point;
  }

  protected override void SetBindings()
  {
    base.SetBindings();
    if (this.LineCap != LineCap.Arrow)
      return;
    Binding binding = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Stroke", new object[0])
    };
    this.shape.SetBinding(Shape.FillProperty, (BindingBase) binding);
  }

  private static void OnShowLinePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    LineAnnotation lineAnnotation = d as LineAnnotation;
    if ((bool) e.NewValue)
      lineAnnotation.AddLine();
    else
      lineAnnotation.RemoveLine();
  }

  private static void OnGrabExtentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    LineAnnotation lineAnnotation = d as LineAnnotation;
    if (lineAnnotation.LinePoints == null)
      return;
    lineAnnotation.UpdateHitArea();
  }

  private void AddLine()
  {
    if (this.LineCanvas == null || this.LineCanvas.Children == null || this.LineCanvas.Children.Contains((UIElement) this.TextElement) || this.LineCanvas.Children.Contains((UIElement) this.shape))
      return;
    this.shape.Tag = (object) this;
    if (this.CanResize)
      this.AddThumb();
    this.LineCanvas.Children.Add((UIElement) this.TextElement);
    this.LineCanvas.Children.Add((UIElement) this.shape);
    this.UpdateAnnotation();
  }

  private void RemoveLine()
  {
    if (this.LineCanvas == null || this.LineCanvas.Children == null || !this.LineCanvas.Children.Contains((UIElement) this.TextElement) || !this.LineCanvas.Children.Contains((UIElement) this.shape))
      return;
    if (this.Chart != null && this.CanResize && this.Chart.AnnotationManager.SelectedAnnotation != null)
    {
      this.nearThumb.DragDelta -= new DragDeltaEventHandler(this.OnNearThumbDragDelta);
      this.farThumb.DragDelta -= new DragDeltaEventHandler(this.OnFarThumbDragDelta);
      this.nearThumb.DragCompleted -= new DragCompletedEventHandler(this.OnDragCompleted);
      this.farThumb.DragCompleted -= new DragCompletedEventHandler(this.OnDragCompleted);
      this.Chart.AnnotationManager.SelectedAnnotation = (Annotation) null;
      this.nearThumb = (Thumb) null;
      this.farThumb = (Thumb) null;
    }
    this.LineCanvas.Children.Clear();
  }

  private void OnFarThumbDragDelta(object sender, DragDeltaEventArgs e)
  {
    this.IsResizing = true;
    this.annotationManager = this.Chart.AnnotationManager;
    double x1 = 0.0;
    double y1 = 0.0;
    this.isRotated = this.XAxis != null && this.XAxis.Orientation == Orientation.Vertical && this.IsAxis;
    this.HorizontalChange = this.isRotated ? e.VerticalChange : e.HorizontalChange;
    this.VerticalChange = this.isRotated ? e.HorizontalChange : e.VerticalChange;
    ShapeAnnotation currentAnnotation = this.annotationManager.CurrentAnnotation as ShapeAnnotation;
    AnnotationManager.SetPosition(this.annotationManager.PreviousPoints, currentAnnotation.X1, currentAnnotation.X2, currentAnnotation.Y1, currentAnnotation.Y2);
    if (!this.isRotated)
    {
      if (this.IsHorizontal)
      {
        x1 = this.IsAxis ? SfChart.PointToAnnotationValue(this.XAxis, new Point(this.ActualX1 + this.HorizontalChange, 0.0)) : this.ActualX1 + this.HorizontalChange;
        AnnotationManager.SetPosition(this.annotationManager.CurrentPoints, (object) x1, currentAnnotation.X2, currentAnnotation.Y1, currentAnnotation.Y2);
      }
      if (this.IsVertical)
      {
        y1 = this.IsAxis ? SfChart.PointToAnnotationValue(this.YAxis, new Point(0.0, this.ActualY1 + this.VerticalChange)) : this.ActualY1 + this.VerticalChange;
        AnnotationManager.SetPosition(this.annotationManager.CurrentPoints, currentAnnotation.X1, currentAnnotation.X2, (object) y1, currentAnnotation.Y2);
      }
    }
    else
    {
      if (this.IsVertical)
      {
        x1 = this.IsAxis ? SfChart.PointToAnnotationValue(this.XAxis, new Point(0.0, this.ActualX1 + this.HorizontalChange)) : this.ActualX1 + this.HorizontalChange;
        AnnotationManager.SetPosition(this.annotationManager.CurrentPoints, (object) x1, currentAnnotation.X2, currentAnnotation.Y1, currentAnnotation.Y2);
      }
      if (this.IsHorizontal)
      {
        y1 = this.IsAxis ? SfChart.PointToAnnotationValue(this.YAxis, new Point(this.ActualY1 + this.VerticalChange, 0.0)) : this.ActualY1 + this.VerticalChange;
        AnnotationManager.SetPosition(this.annotationManager.CurrentPoints, currentAnnotation.X1, currentAnnotation.X2, (object) y1, currentAnnotation.Y2);
      }
    }
    this.annotationManager.RaiseDragStarted();
    this.annotationManager.RaiseDragDelta();
    if (this.annotationManager.DragDeltaArgs.Cancel)
      return;
    if (!this.isRotated && this.IsHorizontal || this.isRotated && this.IsVertical)
      this.ActualX1 = x1;
    if ((this.isRotated || !this.IsVertical) && (!this.isRotated || !this.IsHorizontal))
      return;
    this.ActualY1 = y1;
  }

  private void OnNearThumbDragDelta(object sender, DragDeltaEventArgs e)
  {
    this.IsResizing = true;
    this.annotationManager = this.Chart.AnnotationManager;
    double x2 = 0.0;
    double y2 = 0.0;
    this.isRotated = this.XAxis != null && this.XAxis.Orientation == Orientation.Vertical && this.IsAxis;
    this.HorizontalChange = this.isRotated ? e.VerticalChange : e.HorizontalChange;
    this.VerticalChange = this.isRotated ? e.HorizontalChange : e.VerticalChange;
    ShapeAnnotation currentAnnotation = this.annotationManager.CurrentAnnotation as ShapeAnnotation;
    AnnotationManager.SetPosition(this.annotationManager.PreviousPoints, currentAnnotation.X1, currentAnnotation.X2, currentAnnotation.Y1, currentAnnotation.Y2);
    if (!this.isRotated)
    {
      if (this.IsHorizontal)
      {
        x2 = this.IsAxis ? SfChart.PointToAnnotationValue(this.XAxis, new Point(this.ActualX2 + this.HorizontalChange, 0.0)) : this.ActualX2 + this.HorizontalChange;
        AnnotationManager.SetPosition(this.annotationManager.CurrentPoints, currentAnnotation.X1, (object) x2, currentAnnotation.Y1, currentAnnotation.Y2);
      }
      if (this.IsVertical)
      {
        y2 = this.IsAxis ? SfChart.PointToAnnotationValue(this.YAxis, new Point(0.0, this.ActualY2 + this.VerticalChange)) : this.ActualY2 + this.VerticalChange;
        AnnotationManager.SetPosition(this.annotationManager.CurrentPoints, currentAnnotation.X1, currentAnnotation.X2, currentAnnotation.Y1, (object) y2);
      }
    }
    else
    {
      if (this.IsVertical)
      {
        x2 = this.IsAxis ? SfChart.PointToAnnotationValue(this.XAxis, new Point(0.0, this.ActualX2 + this.HorizontalChange)) : this.ActualX2 + this.HorizontalChange;
        AnnotationManager.SetPosition(this.annotationManager.CurrentPoints, currentAnnotation.X1, (object) x2, currentAnnotation.Y1, currentAnnotation.Y2);
      }
      if (this.IsHorizontal)
      {
        y2 = this.IsAxis ? SfChart.PointToAnnotationValue(this.YAxis, new Point(this.ActualY2 + this.VerticalChange, 0.0)) : this.ActualY2 + this.VerticalChange;
        AnnotationManager.SetPosition(this.annotationManager.CurrentPoints, currentAnnotation.X1, currentAnnotation.X2, currentAnnotation.Y1, (object) y2);
      }
    }
    this.annotationManager.RaiseDragStarted();
    this.annotationManager.RaiseDragDelta();
    if (this.annotationManager.DragDeltaArgs.Cancel)
      return;
    if (!this.isRotated && this.IsHorizontal || this.isRotated && this.IsVertical)
      this.ActualX2 = x2;
    if ((this.isRotated || !this.IsVertical) && (!this.isRotated || !this.IsHorizontal))
      return;
    this.ActualY2 = y2;
  }

  private Point[] GetHitRectPoints(Point point1, Point point2)
  {
    return new Point[4]
    {
      ChartMath.CalculatePerpendicularDistantPoint(point1, point2, -this.GrabExtent),
      ChartMath.CalculatePerpendicularDistantPoint(point2, point1, this.GrabExtent),
      ChartMath.CalculatePerpendicularDistantPoint(point2, point1, -this.GrabExtent),
      ChartMath.CalculatePerpendicularDistantPoint(point1, point2, this.GrabExtent)
    };
  }
}
