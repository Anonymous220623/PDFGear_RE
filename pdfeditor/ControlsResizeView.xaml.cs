// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ResizeView
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls;

public partial class ResizeView : ContentControl
{
  private Grid draggerContainer;
  private Canvas DraggerCanvas;
  private Rectangle MoveDragger;
  private Rectangle LeftTopDragger;
  private Rectangle CenterTopDragger;
  private Rectangle RightTopDragger;
  private Rectangle LeftCenterDragger;
  private Rectangle RightCenterDragger;
  private Rectangle LeftBottomDragger;
  private Rectangle CenterBottomDragger;
  private Rectangle RightBottomDragger;
  private Border DraggerContainerBorder;
  public static readonly DependencyProperty DragModeProperty = DependencyProperty.Register(nameof (DragMode), typeof (ResizeViewOperation), typeof (ResizeView), new PropertyMetadata((object) ResizeViewOperation.All, new PropertyChangedCallback(ResizeView.OnDragModePropertyChanged)));
  public static readonly DependencyProperty CanDragCrossProperty = DependencyProperty.Register(nameof (CanDragCross), typeof (bool), typeof (ResizeView), new PropertyMetadata((object) true));
  public static readonly DependencyProperty DragPlaceholderFillProperty = DependencyProperty.Register(nameof (DragPlaceholderFill), typeof (Brush), typeof (ResizeView), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsDraggerVisibleProperty = DependencyProperty.Register(nameof (IsDraggerVisible), typeof (bool), typeof (ResizeView), new PropertyMetadata((object) true, new PropertyChangedCallback(ResizeView.OnIsDraggerVisiblePropertyChanged)));
  public static readonly DependencyProperty PlaceholderContentProperty = DependencyProperty.Register(nameof (PlaceholderContent), typeof (object), typeof (ResizeView), new PropertyMetadata((object) null, new PropertyChangedCallback(ResizeView.OnPlaceholderContentPropertyChanged)));
  public static readonly DependencyProperty PlaceholderContentTemplateProperty = DependencyProperty.Register(nameof (PlaceholderContentTemplate), typeof (ControlTemplate), typeof (ResizeView), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsCompactModeProperty = DependencyProperty.Register(nameof (IsCompactMode), typeof (bool), typeof (ResizeView), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ResizeView resizeView2) || object.Equals(a.NewValue, a.OldValue))
      return;
    resizeView2.UpdateSizeMode();
  })));
  public static readonly DependencyProperty IsProportionalScaleEnabledProperty = DependencyProperty.Register(nameof (IsProportionalScaleEnabled), typeof (bool), typeof (ResizeView), new PropertyMetadata((object) true, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ResizeView resizeView4) || object.Equals(a.NewValue, a.OldValue))
      return;
    resizeView4.UpdateMouseMoveProperties();
  })));
  private bool dragging;
  private Window curWindow;
  private ResizeView.DragDataContext? dragDataContext;

  static ResizeView()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ResizeView), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ResizeView)));
  }

  public ResizeView()
  {
    this.SizeChanged += new SizeChangedEventHandler(this.ResizeView_SizeChanged);
    this.Unloaded += new RoutedEventHandler(this.ResizeView_Unloaded);
  }

  private Grid DraggerContainer
  {
    get => this.draggerContainer;
    set
    {
      if (this.draggerContainer != null)
        this.draggerContainer.MouseDown -= new MouseButtonEventHandler(this.DraggerContainer_MouseDown);
      this.draggerContainer = value;
      if (this.draggerContainer != null)
        this.draggerContainer.MouseDown += new MouseButtonEventHandler(this.DraggerContainer_MouseDown);
      this.draggerContainer.Width = this.ActualWidth;
      this.draggerContainer.Height = this.ActualHeight;
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.MoveDragger = this.GetTemplateChild<Rectangle>("MoveDragger");
    this.LeftTopDragger = this.GetTemplateChild<Rectangle>("LeftTopDragger");
    this.CenterTopDragger = this.GetTemplateChild<Rectangle>("CenterTopDragger");
    this.RightTopDragger = this.GetTemplateChild<Rectangle>("RightTopDragger");
    this.LeftCenterDragger = this.GetTemplateChild<Rectangle>("LeftCenterDragger");
    this.RightCenterDragger = this.GetTemplateChild<Rectangle>("RightCenterDragger");
    this.LeftBottomDragger = this.GetTemplateChild<Rectangle>("LeftBottomDragger");
    this.CenterBottomDragger = this.GetTemplateChild<Rectangle>("CenterBottomDragger");
    this.RightBottomDragger = this.GetTemplateChild<Rectangle>("RightBottomDragger");
    this.DraggerContainer = this.GetTemplateChild<Grid>("DraggerContainer");
    this.DraggerContainerBorder = this.GetTemplateChild<Border>("DraggerContainerBorder");
    this.DraggerCanvas = this.GetTemplateChild<Canvas>("DraggerCanvas");
    this.UpdateDraggersEnabledState();
    this.UpdateMoveState();
    this.UpdateDraggerVisible();
    this.UpdateSizeMode();
  }

  public ResizeViewOperation DragMode
  {
    get => (ResizeViewOperation) this.GetValue(ResizeView.DragModeProperty);
    set => this.SetValue(ResizeView.DragModeProperty, (object) value);
  }

  private static void OnDragModePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ResizeView resizeView))
      return;
    resizeView.UpdateDraggersEnabledState();
    resizeView.UpdateMoveState();
  }

  public bool CanDragCross
  {
    get => (bool) this.GetValue(ResizeView.CanDragCrossProperty);
    set => this.SetValue(ResizeView.CanDragCrossProperty, (object) value);
  }

  public Brush DragPlaceholderFill
  {
    get => (Brush) this.GetValue(ResizeView.DragPlaceholderFillProperty);
    set => this.SetValue(ResizeView.DragPlaceholderFillProperty, (object) value);
  }

  public bool IsDraggerVisible
  {
    get => (bool) this.GetValue(ResizeView.IsDraggerVisibleProperty);
    set => this.SetValue(ResizeView.IsDraggerVisibleProperty, (object) value);
  }

  private static void OnIsDraggerVisiblePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ResizeView resizeView))
      return;
    resizeView.UpdateDraggerVisible();
  }

  public object PlaceholderContent
  {
    get => this.GetValue(ResizeView.PlaceholderContentProperty);
    set => this.SetValue(ResizeView.PlaceholderContentProperty, value);
  }

  private static void OnPlaceholderContentPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ResizeView resizeView))
      return;
    if (e.OldValue is DependencyObject oldValue)
      resizeView.RemoveLogicalChild((object) oldValue);
    if (!(e.NewValue is DependencyObject newValue))
      return;
    resizeView.AddLogicalChild((object) newValue);
  }

  public ControlTemplate PlaceholderContentTemplate
  {
    get => (ControlTemplate) this.GetValue(ResizeView.PlaceholderContentTemplateProperty);
    set => this.SetValue(ResizeView.PlaceholderContentTemplateProperty, (object) value);
  }

  public bool IsCompactMode
  {
    get => (bool) this.GetValue(ResizeView.IsCompactModeProperty);
    set => this.SetValue(ResizeView.IsCompactModeProperty, (object) value);
  }

  public bool IsProportionalScaleEnabled
  {
    get => (bool) this.GetValue(ResizeView.IsProportionalScaleEnabledProperty);
    set => this.SetValue(ResizeView.IsProportionalScaleEnabledProperty, (object) value);
  }

  private void Dragger_MouseDown(Rectangle dragger, MouseButtonEventArgs e)
  {
    if (!this.ProcessMousePressed(dragger, (MouseEventArgs) e))
      return;
    e.Handled = true;
  }

  private void Dragger_MouseMove(object sender, MouseEventArgs e)
  {
    if (!(e.MouseDevice.Captured is Rectangle captured) || !this.ProcessMouseMove(captured, e))
      return;
    e.Handled = true;
  }

  private void Dragger_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (e.ChangedButton != MouseButton.Left || !(e.MouseDevice.Captured is Rectangle captured) || !this.ProcessMouseRelease(captured, (MouseEventArgs) e))
      return;
    e.Handled = true;
  }

  private void Dragger_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key != Key.LeftShift && e.Key != Key.RightShift)
      return;
    this.UpdateMouseMoveProperties();
  }

  private void Dragger_PreviewKeyUp(object sender, KeyEventArgs e)
  {
    if (e.Key != Key.LeftShift && e.Key != Key.RightShift)
      return;
    this.UpdateMouseMoveProperties();
  }

  private void DraggerContainer_MouseDown(object sender, MouseButtonEventArgs e)
  {
    if (e.ChangedButton != MouseButton.Left || !(e.OriginalSource is Rectangle originalSource))
      return;
    string name = originalSource.Name;
    if ((name != null ? (name.EndsWith("Dragger") ? 1 : 0) : 0) == 0 || originalSource.Parent != sender)
      return;
    this.Dragger_MouseDown(originalSource, e);
  }

  private ResizeViewOperation? GetOperation(string draggerName)
  {
    string str = draggerName;
    if (string.IsNullOrEmpty(str))
      return new ResizeViewOperation?();
    if (str.EndsWith("Dragger"))
      str = str.Substring(0, str.Length - "Dragger".Length);
    if (str != null)
    {
      switch (str.Length)
      {
        case 4:
          if (str == "Move")
            return new ResizeViewOperation?(ResizeViewOperation.Move);
          break;
        case 7:
          if (str == "LeftTop")
            return new ResizeViewOperation?(ResizeViewOperation.LeftTop);
          break;
        case 8:
          if (str == "RightTop")
            return new ResizeViewOperation?(ResizeViewOperation.RightTop);
          break;
        case 9:
          if (str == "CenterTop")
            return new ResizeViewOperation?(ResizeViewOperation.CenterTop);
          break;
        case 10:
          switch (str[4])
          {
            case 'B':
              if (str == "LeftBottom")
                return new ResizeViewOperation?(ResizeViewOperation.LeftBottom);
              break;
            case 'C':
              if (str == "LeftCenter")
                return new ResizeViewOperation?(ResizeViewOperation.LeftCenter);
              break;
          }
          break;
        case 11:
          switch (str[5])
          {
            case 'B':
              if (str == "RightBottom")
                return new ResizeViewOperation?(ResizeViewOperation.RightBottom);
              break;
            case 'C':
              if (str == "RightCenter")
                return new ResizeViewOperation?(ResizeViewOperation.RightCenter);
              break;
          }
          break;
        case 12:
          if (str == "CenterBottom")
            return new ResizeViewOperation?(ResizeViewOperation.CenterBottom);
          break;
      }
    }
    return new ResizeViewOperation?();
  }

  private void UpdateDragDataContext(Rectangle dragger, MouseEventArgs args, bool isDragging)
  {
    if (this.curWindow?.Content == null)
      return;
    Interlocked.MemoryBarrier();
    ResizeView.DragDataContext? dragDataContext = this.dragDataContext;
    if (isDragging && !dragDataContext.HasValue)
      return;
    Point position = args.GetPosition((IInputElement) this.curWindow.Content);
    Size size = new Size(this.ActualWidth, this.ActualHeight);
    ResizeViewOperation? operation = this.GetOperation(dragger?.Name);
    this.dragDataContext = new ResizeView.DragDataContext?(new ResizeView.DragDataContext()
    {
      Operation = operation,
      CanXCross = this.CanDragCross && this.MinWidth == 0.0,
      CanYCross = this.CanDragCross && this.MinHeight == 0.0,
      StartPoint = isDragging ? dragDataContext.Value.StartPoint : position,
      StartSize = isDragging ? dragDataContext.Value.StartSize : size,
      CurrentPoint = position
    });
    Interlocked.MemoryBarrier();
  }

  private bool ProcessMousePressed(Rectangle dragger, MouseEventArgs args)
  {
    if (!this.GetOperation(dragger?.Name).HasValue)
      return false;
    if (Keyboard.FocusedElement is TextBoxBase focusedElement)
    {
      TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
      if (!focusedElement.MoveFocus(request))
        Keyboard.ClearFocus();
    }
    dragger.CaptureMouse();
    if (this.curWindow != null)
    {
      this.curWindow.MouseMove -= new MouseEventHandler(this.Dragger_MouseMove);
      this.curWindow.MouseLeftButtonUp -= new MouseButtonEventHandler(this.Dragger_MouseLeftButtonUp);
      this.curWindow.PreviewKeyDown -= new KeyEventHandler(this.Dragger_PreviewKeyDown);
      this.curWindow.PreviewKeyUp -= new KeyEventHandler(this.Dragger_PreviewKeyUp);
    }
    this.curWindow = Window.GetWindow((DependencyObject) this);
    if (this.curWindow == null)
      return false;
    this.curWindow.MouseMove += new MouseEventHandler(this.Dragger_MouseMove);
    this.curWindow.MouseLeftButtonUp += new MouseButtonEventHandler(this.Dragger_MouseLeftButtonUp);
    this.curWindow.PreviewKeyDown += new KeyEventHandler(this.Dragger_PreviewKeyDown);
    this.curWindow.PreviewKeyUp += new KeyEventHandler(this.Dragger_PreviewKeyUp);
    this.UpdateDragDataContext((Rectangle) null, args, false);
    return true;
  }

  private bool UpdateMouseMoveProperties()
  {
    ResizeView.DragDataContext? dragDataContext = this.dragDataContext;
    if (!dragDataContext.HasValue || !dragDataContext.Value.Operation.HasValue || !this.dragging)
      return false;
    Rect rect = this.ProcessDragOperation(dragDataContext.Value);
    if (!rect.IsEmpty)
    {
      this.DraggerContainer.Width = rect.Width;
      this.DraggerContainer.Height = rect.Height;
      Canvas.SetLeft((UIElement) this.DraggerContainerBorder, rect.Left);
      Canvas.SetTop((UIElement) this.DraggerContainerBorder, rect.Top);
    }
    if ((this.dragging ? 1 : (rect.IsEmpty ? 0 : (rect.X != 0.0 || rect.Y != 0.0 ? 1 : (rect.Size != dragDataContext.Value.StartSize ? 1 : 0)))) != 0)
    {
      ResizeViewResizeDragEventArgs e = new ResizeViewResizeDragEventArgs(dragDataContext.Value.StartSize, rect.IsEmpty ? new Size(this.ActualWidth, this.ActualHeight) : new Size(rect.Width, rect.Height), rect.IsEmpty ? 0.0 : rect.Left, rect.IsEmpty ? 0.0 : rect.Top, dragDataContext.Value.Operation.Value);
      EventHandler<ResizeViewResizeDragEventArgs> resizeDragging = this.ResizeDragging;
      if (resizeDragging != null)
        resizeDragging((object) this, e);
    }
    return true;
  }

  private bool ProcessMouseMove(Rectangle dragger, MouseEventArgs args)
  {
    this.UpdateDragDataContext(dragger, args, true);
    ResizeView.DragDataContext? dragDataContext1 = this.dragDataContext;
    if (!dragDataContext1.HasValue || !dragDataContext1.Value.Operation.HasValue)
      return false;
    if (!this.dragging)
    {
      this.dragging = true;
      ResizeView.DragDataContext dragDataContext2 = dragDataContext1.Value;
      VisualStateManager.GoToState((FrameworkElement) this, dragDataContext2.Operation.GetValueOrDefault() == ResizeViewOperation.Move ? "DragMoving" : "Dragging", true);
      this.Cursor = dragger.Cursor;
      EventHandler<ResizeViewResizeDragStartedEventArgs> resizeDragStarted = this.ResizeDragStarted;
      if (resizeDragStarted != null)
      {
        dragDataContext2 = dragDataContext1.Value;
        resizeDragStarted((object) this, new ResizeViewResizeDragStartedEventArgs(dragDataContext2.Operation.Value));
      }
    }
    return this.UpdateMouseMoveProperties();
  }

  private bool ProcessMouseRelease(Rectangle dragger, MouseEventArgs args)
  {
    if (this.curWindow == null)
      return false;
    this.curWindow.MouseMove -= new MouseEventHandler(this.Dragger_MouseMove);
    this.curWindow.MouseLeftButtonUp -= new MouseButtonEventHandler(this.Dragger_MouseLeftButtonUp);
    this.Cursor = (Cursor) null;
    Mouse.Captured.ReleaseMouseCapture();
    this.dragging = false;
    VisualStateManager.GoToState((FrameworkElement) this, "DragCompleted", true);
    this.UpdateDragDataContext(dragger, args, true);
    ResizeView.DragDataContext? dragDataContext = this.dragDataContext;
    if (!dragDataContext.HasValue || !dragDataContext.Value.Operation.HasValue)
      return false;
    Rect rect = this.ProcessDragOperation(dragDataContext.Value);
    this.DraggerContainer.Width = this.ActualWidth;
    this.DraggerContainer.Height = this.ActualHeight;
    Canvas.SetLeft((UIElement) this.DraggerContainerBorder, 0.0);
    Canvas.SetTop((UIElement) this.DraggerContainerBorder, 0.0);
    if ((this.dragging ? 1 : (rect.IsEmpty ? 0 : (rect.X != 0.0 || rect.Y != 0.0 ? 1 : (rect.Size != dragDataContext.Value.StartSize ? 1 : 0)))) != 0)
    {
      ResizeViewResizeDragEventArgs e = new ResizeViewResizeDragEventArgs(dragDataContext.Value.StartSize, rect.IsEmpty ? new Size(this.ActualWidth, this.ActualHeight) : new Size(rect.Width, rect.Height), rect.IsEmpty ? 0.0 : rect.Left, rect.IsEmpty ? 0.0 : rect.Top, dragDataContext.Value.Operation.Value);
      EventHandler<ResizeViewResizeDragEventArgs> resizeDragCompleted = this.ResizeDragCompleted;
      if (resizeDragCompleted != null)
        resizeDragCompleted((object) this, e);
    }
    this.dragDataContext = new ResizeView.DragDataContext?();
    this.curWindow = (Window) null;
    return true;
  }

  private void ResizeView_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    if (this.DraggerContainer == null)
      return;
    this.DraggerContainer.Width = e.NewSize.Width;
    this.DraggerContainer.Height = e.NewSize.Height;
  }

  private void ResizeView_Unloaded(object sender, RoutedEventArgs e)
  {
    if (this.curWindow != null)
    {
      this.curWindow.MouseMove -= new MouseEventHandler(this.Dragger_MouseMove);
      this.curWindow.MouseLeftButtonUp -= new MouseButtonEventHandler(this.Dragger_MouseLeftButtonUp);
    }
    this.Cursor = (Cursor) null;
    if (Mouse.Captured is Rectangle captured)
    {
      string name = captured.Name;
      if ((name != null ? (name.EndsWith("Dragger") ? 1 : 0) : 0) != 0 && captured.Parent == this.DraggerContainer)
        captured.ReleaseMouseCapture();
    }
    this.dragging = false;
    VisualStateManager.GoToState((FrameworkElement) this, "DragCompleted", true);
  }

  private Rect ProcessDragOperation(ResizeView.DragDataContext context)
  {
    Rect rect = new Rect();
    ResizeViewOperation? operation = context.Operation;
    if (operation.HasValue)
    {
      switch (operation.GetValueOrDefault())
      {
        case ResizeViewOperation.Move:
          rect = this.ProcessDragMove(context);
          break;
        case ResizeViewOperation.LeftTop:
          rect = this.ProcessDragLeftTop(context);
          break;
        case ResizeViewOperation.CenterTop:
          rect = this.ProcessDragCenterTop(context);
          break;
        case ResizeViewOperation.RightTop:
          rect = this.ProcessDragRightTop(context);
          break;
        case ResizeViewOperation.LeftCenter:
          rect = this.ProcessDragLeftCenter(context);
          break;
        case ResizeViewOperation.RightCenter:
          rect = this.ProcessDragRightCenter(context);
          break;
        case ResizeViewOperation.LeftBottom:
          rect = this.ProcessDragLeftBottom(context);
          break;
        case ResizeViewOperation.CenterBottom:
          rect = this.ProcessDragCenterBottom(context);
          break;
        case ResizeViewOperation.RightBottom:
          rect = this.ProcessDragRightBottom(context);
          break;
      }
    }
    return rect;
  }

  internal static bool IsShiftPressedInternal
  {
    get
    {
      return (Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) > KeyStates.None || (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) > KeyStates.None;
    }
  }

  private void ProportionalScaleSize(
    ResizeView.DragDataContext context,
    ref double width,
    ref double height)
  {
    double num1 = 1.0;
    if (context.StartSize.Width != 0.0 && context.StartSize.Height != 0.0)
      num1 = context.StartSize.Width / context.StartSize.Height;
    double num2 = Math.Max(width, this.MinWidth) / num1;
    if (num2 >= this.MinHeight)
    {
      height = num2;
      width = Math.Max(width, this.MinWidth);
    }
    else
    {
      height = this.MinHeight;
      width = this.MinHeight * num1;
    }
  }

  private Rect ProcessDragMove(ResizeView.DragDataContext context)
  {
    return new Rect(context.CurrentPoint.X - context.StartPoint.X, context.CurrentPoint.Y - context.StartPoint.Y, context.StartSize.Width, context.StartSize.Height);
  }

  private Rect ProcessDragLeftTop(ResizeView.DragDataContext context)
  {
    double num1 = context.CurrentPoint.X - context.StartPoint.X;
    double num2 = context.CurrentPoint.Y - context.StartPoint.Y;
    double width = context.StartSize.Width - num1;
    double height = context.StartSize.Height - num2;
    bool flag1 = this.IsProportionalScaleEnabled && ResizeView.IsShiftPressedInternal;
    bool flag2 = context.CanXCross && context.CanYCross;
    if (flag2)
      flag2 = width < 0.0 && height < 0.0 || width < 0.0 & flag1;
    if (flag2)
    {
      ResizeView.DragDataContext context1 = context;
      context1.StartPoint.X += context1.StartSize.Width;
      context1.StartPoint.Y += context1.StartSize.Height;
      context1.CurrentPoint.X -= context1.StartSize.Width;
      context1.CurrentPoint.Y -= context1.StartSize.Height;
      Rect rect = this.ProcessDragRightBottom(context1);
      rect.X += context1.StartSize.Width;
      rect.Y += context1.StartSize.Height;
      return rect;
    }
    if (width < 0.0 && !flag1)
    {
      if (context.CanXCross)
      {
        ResizeView.DragDataContext context2 = context;
        context2.StartPoint.X += context2.StartSize.Width;
        context2.CurrentPoint.X -= context2.StartSize.Width;
        Rect rect = this.ProcessDragRightTop(context2);
        rect.X += context2.StartSize.Width;
        return rect;
      }
      width = this.MinWidth;
    }
    else if (height < 0.0 && !flag1)
    {
      if (context.CanYCross)
      {
        ResizeView.DragDataContext context3 = context;
        context3.StartPoint.Y += context3.StartSize.Height;
        context3.CurrentPoint.Y -= context3.StartSize.Height;
        Rect rect = this.ProcessDragLeftBottom(context3);
        rect.Y += context3.StartSize.Height;
        return rect;
      }
      height = this.MinHeight;
    }
    if (flag1)
    {
      this.ProportionalScaleSize(context, ref width, ref height);
    }
    else
    {
      if (width < this.MinWidth)
        width = this.MinWidth;
      if (height < this.MinHeight)
        height = this.MinHeight;
    }
    return new Rect(context.StartSize.Width - width, context.StartSize.Height - height, width, height);
  }

  private Rect ProcessDragCenterTop(ResizeView.DragDataContext context)
  {
    int x = 0;
    double num = context.CurrentPoint.Y - context.StartPoint.Y;
    double width = context.StartSize.Width;
    double height = context.StartSize.Height - num;
    if (context.CanYCross && height < 0.0)
    {
      ResizeView.DragDataContext context1 = context;
      context1.StartPoint.Y += context1.StartSize.Height;
      context1.CurrentPoint.Y -= context1.StartSize.Height;
      Rect rect = this.ProcessDragCenterBottom(context1);
      rect.Y += context1.StartSize.Height;
      return rect;
    }
    if (height < this.MinHeight)
      height = this.MinHeight;
    double y = context.StartSize.Height - height;
    return new Rect((double) x, y, width, height);
  }

  private Rect ProcessDragRightTop(ResizeView.DragDataContext context)
  {
    double x = 0.0;
    double num = context.CurrentPoint.Y - context.StartPoint.Y;
    double width = context.StartSize.Width + (context.CurrentPoint.X - context.StartPoint.X);
    double height = context.StartSize.Height - num;
    bool flag1 = this.IsProportionalScaleEnabled && ResizeView.IsShiftPressedInternal;
    bool flag2 = context.CanXCross && context.CanYCross;
    if (flag2)
      flag2 = width < 0.0 && height < 0.0 || width < 0.0 & flag1;
    if (flag2)
    {
      ResizeView.DragDataContext context1 = context;
      context1.StartPoint.X -= context1.StartSize.Width;
      context1.StartPoint.Y += context1.StartSize.Height;
      context1.CurrentPoint.X += context1.StartSize.Width;
      context1.CurrentPoint.Y -= context1.StartSize.Height;
      Rect rect = this.ProcessDragLeftBottom(context1);
      rect.X -= context1.StartSize.Width;
      rect.Y += context1.StartSize.Height;
      return rect;
    }
    if (width < 0.0 && !flag1)
    {
      if (context.CanXCross)
      {
        ResizeView.DragDataContext context2 = context;
        context2.StartPoint.X -= context2.StartSize.Width;
        context2.CurrentPoint.X += context2.StartSize.Width;
        Rect rect = this.ProcessDragLeftTop(context2);
        rect.X -= context2.StartSize.Width;
        return rect;
      }
      width = this.MinWidth;
    }
    else if (height < 0.0 && !flag1)
    {
      if (context.CanYCross)
      {
        ResizeView.DragDataContext context3 = context;
        context3.StartPoint.Y += context3.StartSize.Height;
        context3.CurrentPoint.Y -= context3.StartSize.Height;
        Rect rect = this.ProcessDragRightBottom(context3);
        rect.Y += context3.StartSize.Height;
        return rect;
      }
      height = this.MinHeight;
    }
    if (flag1)
    {
      this.ProportionalScaleSize(context, ref width, ref height);
    }
    else
    {
      if (width < this.MinWidth)
        width = this.MinWidth;
      if (height < this.MinHeight)
        height = this.MinHeight;
    }
    double y = context.StartSize.Height - height;
    return new Rect(x, y, width, height);
  }

  private Rect ProcessDragLeftCenter(ResizeView.DragDataContext context)
  {
    double num = context.CurrentPoint.X - context.StartPoint.X;
    int y = 0;
    double width = context.StartSize.Width - num;
    double height = context.StartSize.Height;
    if (context.CanXCross && width < 0.0)
    {
      ResizeView.DragDataContext context1 = context;
      context1.StartPoint.X += context1.StartSize.Width;
      context1.CurrentPoint.X -= context1.StartSize.Width;
      Rect rect = this.ProcessDragRightCenter(context1);
      rect.X += context1.StartSize.Width;
      return rect;
    }
    if (width < this.MinWidth)
      width = this.MinWidth;
    return new Rect(context.StartSize.Width - width, (double) y, width, height);
  }

  private Rect ProcessDragRightCenter(ResizeView.DragDataContext context)
  {
    double x = 0.0;
    double y = 0.0;
    double width = context.StartSize.Width + (context.CurrentPoint.X - context.StartPoint.X);
    double height = context.StartSize.Height;
    if (context.CanXCross && width < 0.0)
    {
      ResizeView.DragDataContext context1 = context;
      context1.StartPoint.X -= context1.StartSize.Width;
      context1.CurrentPoint.X += context1.StartSize.Width;
      Rect rect = this.ProcessDragLeftCenter(context1);
      rect.X -= context1.StartSize.Width;
      return rect;
    }
    if (width < this.MinWidth)
      width = this.MinWidth;
    return new Rect(x, y, width, height);
  }

  private Rect ProcessDragLeftBottom(ResizeView.DragDataContext context)
  {
    double num = context.CurrentPoint.X - context.StartPoint.X;
    double y = 0.0;
    double width = context.StartSize.Width - num;
    double height = context.StartSize.Height + (context.CurrentPoint.Y - context.StartPoint.Y);
    bool flag1 = this.IsProportionalScaleEnabled && ResizeView.IsShiftPressedInternal;
    bool flag2 = context.CanXCross && context.CanYCross;
    if (flag2)
      flag2 = width < 0.0 && height < 0.0 || width < 0.0 & flag1;
    if (flag2)
    {
      ResizeView.DragDataContext context1 = context;
      context1.StartPoint.X += context1.StartSize.Width;
      context1.StartPoint.Y -= context1.StartSize.Height;
      context1.CurrentPoint.X -= context1.StartSize.Width;
      context1.CurrentPoint.Y += context1.StartSize.Height;
      Rect rect = this.ProcessDragRightTop(context1);
      rect.X += context1.StartSize.Width;
      rect.Y -= context1.StartSize.Height;
      return rect;
    }
    if (width < 0.0 && !flag1)
    {
      if (context.CanXCross)
      {
        ResizeView.DragDataContext context2 = context;
        context2.StartPoint.X += context2.StartSize.Width;
        context2.CurrentPoint.X -= context2.StartSize.Width;
        Rect rect = this.ProcessDragRightBottom(context2);
        rect.X += context2.StartSize.Width;
        return rect;
      }
      width = this.MinWidth;
    }
    else if (height < 0.0 && !flag1)
    {
      if (context.CanYCross)
      {
        ResizeView.DragDataContext context3 = context;
        context3.StartPoint.Y -= context3.StartSize.Height;
        context3.CurrentPoint.Y += context3.StartSize.Height;
        Rect rect = this.ProcessDragLeftTop(context3);
        rect.Y -= context3.StartSize.Height;
        return rect;
      }
      height = this.MinHeight;
    }
    if (flag1)
    {
      this.ProportionalScaleSize(context, ref width, ref height);
    }
    else
    {
      if (width < this.MinWidth)
        width = this.MinWidth;
      if (height < this.MinHeight)
        height = this.MinHeight;
    }
    return new Rect(context.StartSize.Width - width, y, width, height);
  }

  private Rect ProcessDragCenterBottom(ResizeView.DragDataContext context)
  {
    double x = 0.0;
    double y = 0.0;
    double width = context.StartSize.Width;
    double height = context.StartSize.Height + (context.CurrentPoint.Y - context.StartPoint.Y);
    if (context.CanYCross && height < 0.0)
    {
      ResizeView.DragDataContext context1 = context;
      context1.StartPoint.Y -= context1.StartSize.Height;
      context1.CurrentPoint.Y += context1.StartSize.Height;
      Rect rect = this.ProcessDragCenterTop(context1);
      rect.Y -= context1.StartSize.Height;
      return rect;
    }
    if (height < this.MinHeight)
      height = this.MinHeight;
    return new Rect(x, y, width, height);
  }

  private Rect ProcessDragRightBottom(ResizeView.DragDataContext context)
  {
    double x = 0.0;
    double y = 0.0;
    double width = context.StartSize.Width + (context.CurrentPoint.X - context.StartPoint.X);
    double height = context.StartSize.Height + (context.CurrentPoint.Y - context.StartPoint.Y);
    bool flag1 = this.IsProportionalScaleEnabled && ResizeView.IsShiftPressedInternal;
    bool flag2 = context.CanXCross && context.CanYCross;
    if (flag2)
      flag2 = width < 0.0 && height < 0.0 || width < 0.0 & flag1;
    if (flag2)
    {
      ResizeView.DragDataContext context1 = context;
      context1.StartPoint.X -= context1.StartSize.Width;
      context1.StartPoint.Y -= context1.StartSize.Height;
      context1.CurrentPoint.X += context1.StartSize.Width;
      context1.CurrentPoint.Y += context1.StartSize.Height;
      Rect rect = this.ProcessDragLeftTop(context1);
      rect.X -= context1.StartSize.Width;
      rect.Y -= context1.StartSize.Height;
      return rect;
    }
    if (width < 0.0 && !flag1)
    {
      if (context.CanXCross)
      {
        ResizeView.DragDataContext context2 = context;
        context2.StartPoint.X -= context2.StartSize.Width;
        context2.CurrentPoint.X += context2.StartSize.Width;
        Rect rect = this.ProcessDragLeftBottom(context2);
        rect.X -= context2.StartSize.Width;
        return rect;
      }
      width = this.MinWidth;
    }
    else if (height < 0.0 && !flag1)
    {
      if (context.CanYCross)
      {
        ResizeView.DragDataContext context3 = context;
        context3.StartPoint.Y -= context3.StartSize.Height;
        context3.CurrentPoint.Y += context3.StartSize.Height;
        Rect rect = this.ProcessDragRightTop(context3);
        rect.Y -= context3.StartSize.Height;
        return rect;
      }
      height = this.MinHeight;
    }
    if (flag1)
    {
      this.ProportionalScaleSize(context, ref width, ref height);
    }
    else
    {
      if (width < this.MinWidth)
        width = this.MinWidth;
      if (height < this.MinHeight)
        height = this.MinHeight;
    }
    return new Rect(x, y, width, height);
  }

  private void UpdateDraggersEnabledState()
  {
    ResizeViewOperation dragMode = this.DragMode;
    UpdateVisibility(this.LeftTopDragger, dragMode);
    UpdateVisibility(this.CenterTopDragger, dragMode);
    UpdateVisibility(this.RightTopDragger, dragMode);
    UpdateVisibility(this.LeftCenterDragger, dragMode);
    UpdateVisibility(this.RightCenterDragger, dragMode);
    UpdateVisibility(this.LeftBottomDragger, dragMode);
    UpdateVisibility(this.CenterBottomDragger, dragMode);
    UpdateVisibility(this.RightBottomDragger, dragMode);

    void UpdateVisibility(Rectangle dragger, ResizeViewOperation source)
    {
      if (dragger == null)
        return;
      ResizeViewOperation? operation = this.GetOperation(dragger.Name);
      if (!operation.HasValue)
        return;
      dragger.Visibility = ToVisibility(source, operation.Value);
    }

    static Visibility ToVisibility(ResizeViewOperation source, ResizeViewOperation flag)
    {
      return !source.HasFlag((Enum) flag) ? Visibility.Collapsed : Visibility.Visible;
    }
  }

  private void UpdateMoveState()
  {
    if ((this.DragMode & ResizeViewOperation.Move) > ResizeViewOperation.None)
      VisualStateManager.GoToState((FrameworkElement) this, "DragMoveEnabled", true);
    else
      VisualStateManager.GoToState((FrameworkElement) this, "DragMoveDisabled", true);
  }

  private void UpdateDraggerVisible()
  {
    VisualStateManager.GoToState((FrameworkElement) this, this.IsDraggerVisible ? "IsDraggerVisible" : "IsDraggerNotVisible", true);
  }

  private void UpdateSizeMode()
  {
    VisualStateManager.GoToState((FrameworkElement) this, this.IsCompactMode ? "CompactSize" : "NormalSize", true);
  }

  private T GetTemplateChild<T>(string name) where T : DependencyObject
  {
    return this.GetTemplateChild(name) as T;
  }

  public event EventHandler<ResizeViewResizeDragStartedEventArgs> ResizeDragStarted;

  public event EventHandler<ResizeViewResizeDragEventArgs> ResizeDragging;

  public event EventHandler<ResizeViewResizeDragEventArgs> ResizeDragCompleted;

  public bool StartDrag(ResizeViewOperation operation, MouseEventArgs args)
  {
    if (args.LeftButton == MouseButtonState.Released)
      return false;
    if ((this.DragMode & operation) == ResizeViewOperation.None)
      throw new ArgumentException(nameof (operation));
    Rectangle dragger;
    switch (operation)
    {
      case ResizeViewOperation.Move:
        dragger = this.MoveDragger;
        break;
      case ResizeViewOperation.LeftTop:
        dragger = this.LeftTopDragger;
        break;
      case ResizeViewOperation.CenterTop:
        dragger = this.CenterTopDragger;
        break;
      case ResizeViewOperation.RightTop:
        dragger = this.RightTopDragger;
        break;
      case ResizeViewOperation.LeftCenter:
        dragger = this.LeftCenterDragger;
        break;
      case ResizeViewOperation.RightCenter:
        dragger = this.RightCenterDragger;
        break;
      case ResizeViewOperation.LeftBottom:
        dragger = this.LeftBottomDragger;
        break;
      case ResizeViewOperation.CenterBottom:
        dragger = this.CenterBottomDragger;
        break;
      case ResizeViewOperation.RightBottom:
        dragger = this.RightBottomDragger;
        break;
      default:
        throw new ArgumentException(nameof (operation));
    }
    return dragger != null && this.ProcessMousePressed(dragger, args);
  }

  public void AddUIElementToCanvas(UIElement element) => this.DraggerCanvas.Children.Add(element);

  public void ClearDrawUIElementOfCanvas()
  {
    if (this.DraggerCanvas.Children.Count <= 1)
      return;
    this.DraggerCanvas.Children.RemoveRange(1, this.DraggerCanvas.Children.Count - 1);
  }

  public Canvas GetDraggerCanvas() => this.DraggerCanvas;

  public void RemoveDrawControl(UIElement element)
  {
    if (element == null || !this.DraggerCanvas.Children.Contains(element))
      return;
    this.DraggerCanvas.Children.Remove(element);
  }

  private struct DragDataContext
  {
    public ResizeViewOperation? Operation;
    public Size StartSize;
    public Point StartPoint;
    public Point CurrentPoint;
    public bool CanXCross;
    public bool CanYCross;
  }
}
