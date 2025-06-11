// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.MouseDragElementBehaviorEx
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Interactivity;

internal class MouseDragElementBehaviorEx : Behavior<FrameworkElement>
{
  public static readonly DependencyProperty XProperty = DependencyProperty.Register(nameof (X), typeof (double), typeof (MouseDragElementBehaviorEx), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(MouseDragElementBehaviorEx.OnXChanged)));
  public static readonly DependencyProperty YProperty = DependencyProperty.Register(nameof (Y), typeof (double), typeof (MouseDragElementBehaviorEx), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(MouseDragElementBehaviorEx.OnYChanged)));
  public static readonly DependencyProperty ConstrainToParentBoundsProperty = DependencyProperty.Register(nameof (ConstrainToParentBounds), typeof (bool), typeof (MouseDragElementBehaviorEx), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(MouseDragElementBehaviorEx.OnConstrainToParentBoundsChanged)));
  private Transform _cachedRenderTransform;
  private Point _relativePosition;
  private bool _settingPosition;

  public bool LockY { get; set; }

  public bool LockX { get; set; }

  public double X
  {
    get => (double) this.GetValue(MouseDragElementBehavior.XProperty);
    set => this.SetValue(MouseDragElementBehavior.XProperty, (object) value);
  }

  public double Y
  {
    get => (double) this.GetValue(MouseDragElementBehavior.YProperty);
    set => this.SetValue(MouseDragElementBehavior.YProperty, (object) value);
  }

  public bool ConstrainToParentBounds
  {
    get => (bool) this.GetValue(MouseDragElementBehavior.ConstrainToParentBoundsProperty);
    set
    {
      this.SetValue(MouseDragElementBehavior.ConstrainToParentBoundsProperty, ValueBoxes.BooleanBox(value));
    }
  }

  private Rect ElementBounds
  {
    get
    {
      Rect layoutRect = ArithmeticHelper.GetLayoutRect(this.AssociatedObject);
      return new Rect(new Point(0.0, 0.0), new Size(layoutRect.Width, layoutRect.Height));
    }
  }

  private FrameworkElement ParentElement => this.AssociatedObject.Parent as FrameworkElement;

  private UIElement RootElement
  {
    get
    {
      DependencyObject reference = (DependencyObject) this.AssociatedObject;
      for (DependencyObject dependencyObject = reference; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(reference))
        reference = dependencyObject;
      return reference as UIElement;
    }
  }

  private Transform RenderTransform
  {
    get
    {
      if (this._cachedRenderTransform == null || this._cachedRenderTransform != this.AssociatedObject.RenderTransform)
        this.RenderTransform = MouseDragElementBehaviorEx.CloneTransform(this.AssociatedObject.RenderTransform);
      return this._cachedRenderTransform;
    }
    set
    {
      if (object.Equals((object) this._cachedRenderTransform, (object) value))
        return;
      this._cachedRenderTransform = value;
      this.AssociatedObject.RenderTransform = value;
    }
  }

  public event MouseEventHandler DragBegun;

  public event MouseEventHandler Dragging;

  public event MouseEventHandler DragFinished;

  private static void OnXChanged(object sender, DependencyPropertyChangedEventArgs args)
  {
    MouseDragElementBehaviorEx elementBehaviorEx = (MouseDragElementBehaviorEx) sender;
    elementBehaviorEx.UpdatePosition(new Point((double) args.NewValue, elementBehaviorEx.Y));
  }

  private static void OnYChanged(object sender, DependencyPropertyChangedEventArgs args)
  {
    MouseDragElementBehaviorEx elementBehaviorEx = (MouseDragElementBehaviorEx) sender;
    elementBehaviorEx.UpdatePosition(new Point(elementBehaviorEx.X, (double) args.NewValue));
  }

  private static void OnConstrainToParentBoundsChanged(
    object sender,
    DependencyPropertyChangedEventArgs args)
  {
    MouseDragElementBehaviorEx elementBehaviorEx = (MouseDragElementBehaviorEx) sender;
    elementBehaviorEx.UpdatePosition(new Point(elementBehaviorEx.X, elementBehaviorEx.Y));
  }

  private void UpdatePosition(Point point)
  {
    if (this._settingPosition || this.AssociatedObject == null)
      return;
    Point transformOffset = MouseDragElementBehaviorEx.GetTransformOffset(this.AssociatedObject.TransformToVisual((Visual) this.RootElement));
    this.ApplyTranslation(double.IsNaN(point.X) ? 0.0 : point.X - transformOffset.X, double.IsNaN(point.Y) ? 0.0 : point.Y - transformOffset.Y);
  }

  private void ApplyTranslation(double x, double y)
  {
    if (this.ParentElement == null)
      return;
    Point point = MouseDragElementBehaviorEx.TransformAsVector(this.RootElement.TransformToVisual((Visual) this.ParentElement), x, y);
    x = point.X;
    y = point.Y;
    if (this.ConstrainToParentBounds)
    {
      FrameworkElement parentElement = this.ParentElement;
      Rect rect1 = new Rect(0.0, 0.0, parentElement.ActualWidth, parentElement.ActualHeight);
      Rect rect2 = this.AssociatedObject.TransformToVisual((Visual) parentElement).TransformBounds(this.ElementBounds);
      rect2.X += x;
      rect2.Y += y;
      if (!MouseDragElementBehaviorEx.RectContainsRect(rect1, rect2))
      {
        if (rect2.X < rect1.Left)
        {
          double num = rect2.X - rect1.Left;
          x -= num;
        }
        else if (rect2.Right > rect1.Right)
        {
          double num = rect2.Right - rect1.Right;
          x -= num;
        }
        if (rect2.Y < rect1.Top)
        {
          double num = rect2.Y - rect1.Top;
          y -= num;
        }
        else if (rect2.Bottom > rect1.Bottom)
        {
          double num = rect2.Bottom - rect1.Bottom;
          y -= num;
        }
      }
    }
    this.ApplyTranslationTransform(x, y);
  }

  internal void ApplyTranslationTransform(double x, double y)
  {
    Transform renderTransform = this.RenderTransform;
    if (!(renderTransform is TranslateTransform translateTransform))
    {
      MatrixTransform matrixTransform = renderTransform as MatrixTransform;
      if (renderTransform is TransformGroup transformGroup1)
      {
        if (transformGroup1.Children.Count > 0)
          translateTransform = transformGroup1.Children[transformGroup1.Children.Count - 1] as TranslateTransform;
        if (translateTransform == null)
        {
          translateTransform = new TranslateTransform();
          transformGroup1.Children.Add((Transform) translateTransform);
        }
      }
      else
      {
        if (matrixTransform != null)
        {
          Matrix matrix = matrixTransform.Matrix;
          if (!this.LockX)
            matrix.OffsetX += x;
          if (!this.LockY)
            matrix.OffsetY += y;
          this.RenderTransform = (Transform) new MatrixTransform()
          {
            Matrix = matrix
          };
          return;
        }
        TransformGroup transformGroup = new TransformGroup();
        translateTransform = new TranslateTransform();
        if (renderTransform != null)
          transformGroup.Children.Add(renderTransform);
        transformGroup.Children.Add((Transform) translateTransform);
        this.RenderTransform = (Transform) transformGroup;
      }
    }
    if (!this.LockX)
      translateTransform.X += x;
    if (this.LockY)
      return;
    translateTransform.Y += y;
  }

  internal static Transform CloneTransform(Transform transform)
  {
    switch (transform)
    {
      case null:
        return (Transform) null;
      case ScaleTransform scaleTransform:
        return (Transform) new ScaleTransform()
        {
          CenterX = scaleTransform.CenterX,
          CenterY = scaleTransform.CenterY,
          ScaleX = scaleTransform.ScaleX,
          ScaleY = scaleTransform.ScaleY
        };
      case RotateTransform rotateTransform:
        return (Transform) new RotateTransform()
        {
          Angle = rotateTransform.Angle,
          CenterX = rotateTransform.CenterX,
          CenterY = rotateTransform.CenterY
        };
      case SkewTransform skewTransform:
        return (Transform) new SkewTransform()
        {
          AngleX = skewTransform.AngleX,
          AngleY = skewTransform.AngleY,
          CenterX = skewTransform.CenterX,
          CenterY = skewTransform.CenterY
        };
      case TranslateTransform translateTransform:
        return (Transform) new TranslateTransform()
        {
          X = translateTransform.X,
          Y = translateTransform.Y
        };
      case MatrixTransform matrixTransform:
        return (Transform) new MatrixTransform()
        {
          Matrix = matrixTransform.Matrix
        };
      case TransformGroup transformGroup2:
        TransformGroup transformGroup1 = new TransformGroup();
        foreach (Transform child in transformGroup2.Children)
          transformGroup1.Children.Add(MouseDragElementBehaviorEx.CloneTransform(child));
        return (Transform) transformGroup1;
      default:
        return (Transform) null;
    }
  }

  private void UpdatePosition()
  {
    Point transformOffset = MouseDragElementBehaviorEx.GetTransformOffset(this.AssociatedObject.TransformToVisual((Visual) this.RootElement));
    this.X = transformOffset.X;
    this.Y = transformOffset.Y;
  }

  internal void StartDrag(Point positionInElementCoordinates)
  {
    this._relativePosition = positionInElementCoordinates;
    this.AssociatedObject.CaptureMouse();
    this.AssociatedObject.MouseMove += new MouseEventHandler(this.OnMouseMove);
    this.AssociatedObject.LostMouseCapture += new MouseEventHandler(this.OnLostMouseCapture);
    this.AssociatedObject.AddHandler(UIElement.MouseLeftButtonUpEvent, (Delegate) new MouseButtonEventHandler(this.OnMouseLeftButtonUp), false);
  }

  internal void HandleDrag(Point newPositionInElementCoordinates)
  {
    Point point = MouseDragElementBehaviorEx.TransformAsVector(this.AssociatedObject.TransformToVisual((Visual) this.RootElement), newPositionInElementCoordinates.X - this._relativePosition.X, newPositionInElementCoordinates.Y - this._relativePosition.Y);
    this._settingPosition = true;
    this.ApplyTranslation(point.X, point.Y);
    this.UpdatePosition();
    this._settingPosition = false;
  }

  internal void EndDrag()
  {
    this.AssociatedObject.MouseMove -= new MouseEventHandler(this.OnMouseMove);
    this.AssociatedObject.LostMouseCapture -= new MouseEventHandler(this.OnLostMouseCapture);
    this.AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonUpEvent, (Delegate) new MouseButtonEventHandler(this.OnMouseLeftButtonUp));
  }

  private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.StartDrag(e.GetPosition((IInputElement) this.AssociatedObject));
    MouseEventHandler dragBegun = this.DragBegun;
    if (dragBegun == null)
      return;
    dragBegun((object) this, (MouseEventArgs) e);
  }

  private void OnLostMouseCapture(object sender, MouseEventArgs e)
  {
    this.EndDrag();
    MouseEventHandler dragFinished = this.DragFinished;
    if (dragFinished == null)
      return;
    dragFinished((object) this, e);
  }

  private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.AssociatedObject.ReleaseMouseCapture();
  }

  private void OnMouseMove(object sender, MouseEventArgs e)
  {
    this.HandleDrag(e.GetPosition((IInputElement) this.AssociatedObject));
    MouseEventHandler dragging = this.Dragging;
    if (dragging == null)
      return;
    dragging((object) this, e);
  }

  private static bool RectContainsRect(Rect rect1, Rect rect2)
  {
    return !rect1.IsEmpty && !rect2.IsEmpty && rect1.X <= rect2.X && rect1.Y <= rect2.Y && rect1.X + rect1.Width >= rect2.X + rect2.Width && rect1.Y + rect1.Height >= rect2.Y + rect2.Height;
  }

  private static Point TransformAsVector(GeneralTransform transform, double x, double y)
  {
    Point point1 = transform.Transform(new Point(0.0, 0.0));
    Point point2 = transform.Transform(new Point(x, y));
    return new Point(point2.X - point1.X, point2.Y - point1.Y);
  }

  private static Point GetTransformOffset(GeneralTransform transform)
  {
    return transform.Transform(new Point(0.0, 0.0));
  }

  protected override void OnAttached()
  {
    this.AssociatedObject.AddHandler(UIElement.MouseLeftButtonDownEvent, (Delegate) new MouseButtonEventHandler(this.OnMouseLeftButtonDown), false);
  }

  protected override void OnDetaching()
  {
    this.AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonDownEvent, (Delegate) new MouseButtonEventHandler(this.OnMouseLeftButtonDown));
  }
}
