// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.OverviewContentHolder
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.Windows.Shared;

[System.Windows.Markup.ContentProperty("Content")]
public class OverviewContentHolder : Control, IOverviewPanel, IScrollInfo
{
  private const double LineSize = 16.0;
  private const double WheelSize = 48.0;
  public static readonly DependencyProperty OriginProperty = DependencyProperty.RegisterAttached("Origin", typeof (Point), typeof (OverviewContentHolder), new PropertyMetadata((object) new Point(0.0, 0.0)));
  public static readonly DependencyProperty EnableFitToPageProperty = DependencyProperty.Register(nameof (EnableFitToPage), typeof (bool), typeof (OverviewContentHolder), new PropertyMetadata((object) false, new PropertyChangedCallback(OverviewContentHolder.OnFitToPageEnabled)));
  private static readonly DependencyProperty TopLeftProperty = DependencyProperty.Register(nameof (TopLeft), typeof (Point), typeof (OverviewContentHolder), new PropertyMetadata((object) new Point(0.0, 0.0), new PropertyChangedCallback(OverviewContentHolder.OnTopLeftChanged)));
  public static readonly DependencyProperty PageBackgroundProperty = DependencyProperty.Register(nameof (PageBackground), typeof (Brush), typeof (OverviewContentHolder), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty AllowResizeProperty = DependencyProperty.Register(nameof (AllowResize), typeof (bool), typeof (OverviewContentHolder), new PropertyMetadata((object) false));
  internal Overview _overviewParent;
  public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof (Content), typeof (UIElement), typeof (OverviewContentHolder), new PropertyMetadata((object) null, new PropertyChangedCallback(OverviewContentHolder.OnContentChanged)));
  private Stack<OverviewContentHolder.ZoomOperation> zoomStack = new Stack<OverviewContentHolder.ZoomOperation>();
  public static readonly DependencyProperty IsZoomEnabledProperty = DependencyProperty.Register(nameof (IsZoomEnabled), typeof (bool), typeof (OverviewContentHolder), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(OverviewContentHolder.IsZoomEnabledChanged)));
  private Point? m_PanStartPosition = new Point?();
  private Point m_PreviousMousePosition = new Point(0.0, 0.0);
  private DateTime m_LastLeftClick = DateTime.Now;
  private DateTime m_LastRightClick = DateTime.Now;
  internal OverviewMouseState m_MouseState = OverviewMouseState.None;
  private ScaleTransform _scaleTransform = new ScaleTransform();
  private TranslateTransform _translateTransform = new TranslateTransform();
  private DoubleAnimation _scaleXAnimation = new DoubleAnimation();
  private DoubleAnimation _scaleYAnimation = new DoubleAnimation();
  private DoubleAnimation _translateXAnimation = new DoubleAnimation();
  private DoubleAnimation _translateYAnimation = new DoubleAnimation();
  private Duration _animationDuration = new Duration(new TimeSpan(0, 0, 0, 0, 200));
  private Storyboard _storyboard = new Storyboard();
  public static readonly DependencyProperty AnimationEnabledProperty = DependencyProperty.Register(nameof (AnimationEnabled), typeof (bool), typeof (OverviewContentHolder), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ZoomInGestureProperty = DependencyProperty.Register(nameof (ZoomInGesture), typeof (ZoomGesture), typeof (OverviewContentHolder), new PropertyMetadata((object) (ZoomGesture.MouseWheelUp | ZoomGesture.Ctrl)));
  public static readonly DependencyProperty ZoomOutGestureProperty = DependencyProperty.Register(nameof (ZoomOutGesture), typeof (ZoomGesture), typeof (OverviewContentHolder), new PropertyMetadata((object) (ZoomGesture.MouseWheelDown | ZoomGesture.Ctrl)));
  public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(nameof (Scale), typeof (double), typeof (OverviewContentHolder), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(OverviewContentHolder.OnScaleChanged)));
  public static readonly DependencyProperty IsZoomInEnabledProperty = DependencyProperty.Register(nameof (IsZoomInEnabled), typeof (bool), typeof (OverviewContentHolder), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ZoomInProperty = DependencyProperty.Register(nameof (ZoomIn), typeof (ICommand), typeof (OverviewContentHolder), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsZoomOutEnabledProperty = DependencyProperty.Register(nameof (IsZoomOutEnabled), typeof (bool), typeof (OverviewContentHolder), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ZoomOutProperty = DependencyProperty.Register(nameof (ZoomOut), typeof (ICommand), typeof (OverviewContentHolder), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsZoomToEnabledProperty = DependencyProperty.Register(nameof (IsZoomToEnabled), typeof (bool), typeof (OverviewContentHolder), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ZoomToProperty = DependencyProperty.Register(nameof (ZoomTo), typeof (ICommand), typeof (OverviewContentHolder), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsZoomResetEnabledProperty = DependencyProperty.Register(nameof (IsZoomResetEnabled), typeof (bool), typeof (OverviewContentHolder), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ZoomResetProperty = DependencyProperty.Register(nameof (ZoomReset), typeof (ICommand), typeof (OverviewContentHolder), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ZoomFactorProperty = DependencyProperty.Register(nameof (ZoomFactor), typeof (double), typeof (OverviewContentHolder), new PropertyMetadata((object) 0.2));
  public static readonly DependencyProperty MinimumZoomProperty = DependencyProperty.Register(nameof (MinimumZoom), typeof (double), typeof (OverviewContentHolder), new PropertyMetadata((object) 0.2));
  public static readonly DependencyProperty MaximumZoomProperty = DependencyProperty.Register(nameof (MaximumZoom), typeof (double), typeof (OverviewContentHolder), new PropertyMetadata((object) 10.0));
  public static readonly DependencyProperty ZoomModeProperty = DependencyProperty.Register(nameof (ZoomMode), typeof (ZoomMode), typeof (OverviewContentHolder), new PropertyMetadata((object) ZoomMode.Unit));
  public static readonly DependencyProperty IsPanEnabledProperty = DependencyProperty.Register(nameof (IsPanEnabled), typeof (bool), typeof (OverviewContentHolder), new PropertyMetadata((object) false, new PropertyChangedCallback(OverviewContentHolder.OnIsPanEnabledChanged)));
  private Grid PART_Grid;
  private Rectangle PART_PageBackground;
  private ContentControl diagramView;
  private bool m_CanHorizontallyScroll;
  private bool m_CanVerticallyScroll;
  private double m_HorizontalOffset;
  private double m_VerticalOffset;
  private double m_ViewportWidth;
  private double m_ViewportHeight;
  private double m_ExtentWidth;
  private double m_ExtentHeight;
  private ScrollViewer m_ScrollOwner;
  public static readonly RoutedEvent FitToPageEvent = EventManager.RegisterRoutedEvent("UpdateFitToPage", RoutingStrategy.Bubble, typeof (OverviewContentHolder.OverviewFitPageEventHandler), typeof (OverviewContentHolder));
  public static readonly RoutedEvent ExtraPanningEvent = EventManager.RegisterRoutedEvent("ExtraPanning", RoutingStrategy.Bubble, typeof (OverviewContentHolder.ExtraPanningEventEventHandler), typeof (OverviewContentHolder));

  public static Point GetOrigin(DependencyObject obj)
  {
    return (Point) obj.GetValue(OverviewContentHolder.OriginProperty);
  }

  public static void SetOrigin(DependencyObject obj, Point value)
  {
    obj.SetValue(OverviewContentHolder.OriginProperty, (object) value);
  }

  public bool EnableFitToPage
  {
    get => (bool) this.GetValue(OverviewContentHolder.EnableFitToPageProperty);
    set => this.SetValue(OverviewContentHolder.EnableFitToPageProperty, (object) value);
  }

  private static void OnFitToPageEnabled(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    OverviewContentHolder parameter = (OverviewContentHolder) d;
    parameter.ZoomReset.Execute((object) parameter);
    parameter.FitToPage();
  }

  internal Point TopLeft
  {
    get => (Point) this.GetValue(OverviewContentHolder.TopLeftProperty);
    set => this.SetValue(OverviewContentHolder.TopLeftProperty, (object) value);
  }

  private static void OnTopLeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    OverviewContentHolder overviewContentHolder = d as OverviewContentHolder;
    Point oldValue = (Point) e.OldValue;
    Point newValue = (Point) e.NewValue;
    double num1 = oldValue.X - newValue.X;
    double num2 = oldValue.Y - newValue.Y;
    overviewContentHolder.m_HorizontalOffset -= num1;
    overviewContentHolder.m_VerticalOffset -= num2;
    overviewContentHolder.UpdatePageBackground();
  }

  public Brush PageBackground
  {
    get => (Brush) this.GetValue(OverviewContentHolder.PageBackgroundProperty);
    set => this.SetValue(OverviewContentHolder.PageBackgroundProperty, (object) value);
  }

  public bool AllowResize
  {
    get => (bool) this.GetValue(OverviewContentHolder.AllowResizeProperty);
    set => this.SetValue(OverviewContentHolder.AllowResizeProperty, (object) value);
  }

  public OverviewContentHolder()
  {
    this.DefaultStyleKey = (object) typeof (OverviewContentHolder);
    this.ZoomIn = (ICommand) new DelegateCommand<object>(new Action<object>(this.ExecuteZoomInCommand), new Predicate<object>(this.CanZoomInExecute));
    this.ZoomOut = (ICommand) new DelegateCommand<object>(new Action<object>(this.ExecuteZoomOutCommand), new Predicate<object>(this.CanZoomOutExecute));
    this.ZoomTo = (ICommand) new DelegateCommand<object>(new Action<object>(this.ExecuteZoomToCommand), new Predicate<object>(this.CanZoomToExecute));
    this.ZoomReset = (ICommand) new DelegateCommand<object>(new Action<object>(this.ExecuteZoomResetCommand), new Predicate<object>(this.CanZoomResetExecute));
    this.UseLayoutRounding = false;
    this.Loaded += new RoutedEventHandler(this.OverviewContentHolder_Loaded);
  }

  private void OverviewContentHolder_Loaded(object sender, RoutedEventArgs e)
  {
    if (this._overviewParent == null)
      return;
    this._overviewParent.InvalidateScroll();
  }

  public UIElement Content
  {
    get => (UIElement) this.GetValue(OverviewContentHolder.ContentProperty);
    set => this.SetValue(OverviewContentHolder.ContentProperty, (object) value);
  }

  private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as OverviewContentHolder).OnContentChanged(e.OldValue, e.NewValue);
  }

  protected virtual void OnContentChanged(object oldContent, object newContent)
  {
    if (this.Content == null)
      return;
    Binding binding = (Binding) XamlReader.Parse("<Binding xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:mns=\"clr-namespace:Syncfusion.Windows.Shared;assembly=Syncfusion.Shared.Wpf\" Path=\"(mns:OverviewContentHolder.Origin)\" />");
    binding.Source = (object) this.Content;
    binding.Converter = (IValueConverter) new OverviewContentHolder.Inverter();
    this.SetBinding(OverviewContentHolder.TopLeftProperty, (BindingBase) binding);
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    base.OnMouseWheel(e);
    this.PerformZoom((MouseEventArgs) e);
  }

  public bool IsZoomEnabled
  {
    get => (bool) this.GetValue(OverviewContentHolder.IsZoomEnabledProperty);
    set => this.SetValue(OverviewContentHolder.IsZoomEnabledProperty, (object) value);
  }

  private static void IsZoomEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
  }

  private void PerformZoom(MouseEventArgs e)
  {
    if (e.CanZoomIn(this))
    {
      if (!this.IsZoomInEnabled || !this.IsZoomEnabled)
        return;
      this.ZoomIn.Execute((object) new ZoomParamenter()
      {
        FocusPoint = new Point?(this.GetMousePosition(e, 1.0))
      });
    }
    else
    {
      if (!e.CanZoomOut(this) || !this.IsZoomOutEnabled || !this.IsZoomEnabled)
        return;
      this.ZoomOut.Execute((object) new ZoomParamenter()
      {
        FocusPoint = new Point?(this.GetMousePosition(e, 1.0))
      });
    }
    if (!(e is MouseWheelEventArgs mouseWheelEventArgs))
      return;
    mouseWheelEventArgs.Handled = true;
  }

  private object GetZoomParameter(object param)
  {
    object parameter = new object();
    if (param is IZoomParameter)
    {
      parameter = (object) new ZoomParamenter();
      if (param is IZoomMouseParameter)
      {
        IZoomMouseParameter zoomMouseParameter = param as IZoomMouseParameter;
        ((ZoomParamenter) parameter).FocusPoint = new Point?(this.GetMousePosition(zoomMouseParameter.MouseEventArgs, 1.0));
        ((ZoomParamenter) parameter).ZoomFactor = zoomMouseParameter is IZoomPositionParameter ? ((IZoomPositionParameter) zoomMouseParameter).ZoomFactor : new double?();
        if (zoomMouseParameter is IZoomPositionParameter && ((IZoomPositionParameter) zoomMouseParameter).ZoomTo.HasValue)
        {
          ((ZoomParamenter) parameter).ZoomTo = ((IZoomPositionParameter) zoomMouseParameter).ZoomTo;
          this.ZoomTo.Execute(parameter);
          return (object) null;
        }
      }
      if (param is IZoomPositionParameter)
      {
        IZoomPositionParameter positionParameter = param as IZoomPositionParameter;
        if (positionParameter.FocusPoint.HasValue)
          ((ZoomParamenter) parameter).FocusPoint = new Point?(positionParameter.FocusPoint ?? new Point(0.0, 0.0));
        ZoomParamenter zoomParamenter = (ZoomParamenter) parameter;
        double? zoomFactor = positionParameter.ZoomFactor;
        double? nullable = zoomFactor.HasValue ? new double?(zoomFactor.GetValueOrDefault()) : new double?();
        zoomParamenter.ZoomFactor = nullable;
        if (positionParameter.ZoomTo.HasValue)
        {
          ((ZoomParamenter) parameter).ZoomTo = positionParameter.ZoomTo;
          this.ZoomTo.Execute(parameter);
          return (object) null;
        }
      }
    }
    return parameter;
  }

  private Point GetMousePosition(MouseEventArgs e, double Scale)
  {
    Point mousePosition = e.GetPosition((IInputElement) this.Content);
    mousePosition = new Point((mousePosition.X + this.TopLeft.X) * Scale, (mousePosition.Y + this.TopLeft.Y) * Scale);
    e.Handled = true;
    return mousePosition;
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    base.OnMouseLeftButtonDown(e);
    this.SetPanStartPoint(e);
  }

  private void SetPanStartPoint(MouseButtonEventArgs e)
  {
    if (!this.IsPanEnabled)
      return;
    this.CaptureMouse();
    this.m_PanStartPosition = new Point?(e.GetPosition((IInputElement) this));
    this.m_PreviousMousePosition = this.m_PanStartPosition.Value;
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    this.m_LastLeftClick -= new TimeSpan(1, 0, 0);
    this.m_LastRightClick -= new TimeSpan(1, 0, 0);
    if (this.IsPanEnabled && this.m_PanStartPosition.HasValue)
    {
      this.m_MouseState = OverviewMouseState.Pan;
      Point position = e.GetPosition((IInputElement) this);
      this.SetHorizontalOffset(this.m_HorizontalOffset + this.m_PreviousMousePosition.X - position.X);
      this.SetVerticalOffset(this.m_VerticalOffset + this.m_PreviousMousePosition.Y - position.Y);
      this.VerifyAndInvalidateScrollData();
      this.m_PreviousMousePosition = position;
    }
    else
      this.m_MouseState = OverviewMouseState.None;
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    base.OnMouseLeftButtonUp(e);
    if (this.m_MouseState == OverviewMouseState.Pan)
      this.m_MouseState = OverviewMouseState.None;
    if ((DateTime.Now - this.m_LastLeftClick).TotalMilliseconds < 300.0)
    {
      this.m_MouseState = OverviewMouseState.LeftDoubleClick;
      this.m_LastLeftClick = DateTime.Now - new TimeSpan(0, 0, 0, 2);
      this.PerformZoom((MouseEventArgs) e);
    }
    else
    {
      this.m_MouseState = OverviewMouseState.LeftClick;
      this.m_LastLeftClick = DateTime.Now;
      this.PerformZoom((MouseEventArgs) e);
    }
    this.m_MouseState = OverviewMouseState.None;
    this.ReleaseMouseCapture();
    this.m_PanStartPosition = new Point?();
  }

  protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
  {
    base.OnMouseRightButtonUp(e);
    if (this.m_MouseState == OverviewMouseState.Pan)
      this.m_MouseState = OverviewMouseState.None;
    else if ((DateTime.Now - this.m_LastRightClick).TotalMilliseconds < 300.0)
    {
      this.m_MouseState = OverviewMouseState.RightDoubleClick;
      this.m_LastRightClick = DateTime.Now - new TimeSpan(0, 0, 0, 2);
      this.PerformZoom((MouseEventArgs) e);
    }
    else
    {
      this.m_MouseState = OverviewMouseState.RightClick;
      this.m_LastRightClick = DateTime.Now;
      this.PerformZoom((MouseEventArgs) e);
    }
    this.m_MouseState = OverviewMouseState.None;
  }

  public bool AnimationEnabled
  {
    get => (bool) this.GetValue(OverviewContentHolder.AnimationEnabledProperty);
    set => this.SetValue(OverviewContentHolder.AnimationEnabledProperty, (object) value);
  }

  public ZoomGesture ZoomInGesture
  {
    get => (ZoomGesture) this.GetValue(OverviewContentHolder.ZoomInGestureProperty);
    set => this.SetValue(OverviewContentHolder.ZoomInGestureProperty, (object) value);
  }

  public ZoomGesture ZoomOutGesture
  {
    get => (ZoomGesture) this.GetValue(OverviewContentHolder.ZoomOutGestureProperty);
    set => this.SetValue(OverviewContentHolder.ZoomOutGestureProperty, (object) value);
  }

  public double Scale
  {
    get => (double) this.GetValue(OverviewContentHolder.ScaleProperty);
    set => this.SetValue(OverviewContentHolder.ScaleProperty, (object) value);
  }

  internal double UnitScale
  {
    get => this.ZoomMode == ZoomMode.Unit ? this.Scale : this.Scale / 100.0;
    set
    {
      if (this.ZoomMode == ZoomMode.Unit)
        this.Scale = value;
      else
        this.Scale = value * 100.0;
    }
  }

  private static void OnScaleChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
  {
    OverviewContentHolder overviewContentHolder = dp as OverviewContentHolder;
    if (overviewContentHolder.Scale >= overviewContentHolder.MinimumZoom || !overviewContentHolder.EnableFitToPage)
      overviewContentHolder.Scale = Math.Max(overviewContentHolder.MinimumZoom, Math.Min(overviewContentHolder.MaximumZoom, overviewContentHolder.Scale));
    overviewContentHolder.InvalidateMeasure();
    overviewContentHolder.InvalidateArrange();
  }

  public bool IsZoomInEnabled
  {
    get => (bool) this.GetValue(OverviewContentHolder.IsZoomInEnabledProperty);
    set => this.SetValue(OverviewContentHolder.IsZoomInEnabledProperty, (object) value);
  }

  public ICommand ZoomIn
  {
    get => (ICommand) this.GetValue(OverviewContentHolder.ZoomInProperty);
    set => this.SetValue(OverviewContentHolder.ZoomInProperty, (object) value);
  }

  public bool IsZoomOutEnabled
  {
    get => (bool) this.GetValue(OverviewContentHolder.IsZoomOutEnabledProperty);
    set => this.SetValue(OverviewContentHolder.IsZoomOutEnabledProperty, (object) value);
  }

  public ICommand ZoomOut
  {
    get => (ICommand) this.GetValue(OverviewContentHolder.ZoomOutProperty);
    set => this.SetValue(OverviewContentHolder.ZoomOutProperty, (object) value);
  }

  public bool IsZoomToEnabled
  {
    get => (bool) this.GetValue(OverviewContentHolder.IsZoomToEnabledProperty);
    set => this.SetValue(OverviewContentHolder.IsZoomToEnabledProperty, (object) value);
  }

  public ICommand ZoomTo
  {
    get => (ICommand) this.GetValue(OverviewContentHolder.ZoomToProperty);
    set => this.SetValue(OverviewContentHolder.ZoomToProperty, (object) value);
  }

  public bool IsZoomResetEnabled
  {
    get => (bool) this.GetValue(OverviewContentHolder.IsZoomResetEnabledProperty);
    set => this.SetValue(OverviewContentHolder.IsZoomResetEnabledProperty, (object) value);
  }

  public ICommand ZoomReset
  {
    get => (ICommand) this.GetValue(OverviewContentHolder.ZoomResetProperty);
    set => this.SetValue(OverviewContentHolder.ZoomResetProperty, (object) value);
  }

  public double ZoomFactor
  {
    get => (double) this.GetValue(OverviewContentHolder.ZoomFactorProperty);
    set => this.SetValue(OverviewContentHolder.ZoomFactorProperty, (object) value);
  }

  public double MinimumZoom
  {
    get => (double) this.GetValue(OverviewContentHolder.MinimumZoomProperty);
    set => this.SetValue(OverviewContentHolder.MinimumZoomProperty, (object) value);
  }

  public double MaximumZoom
  {
    get => (double) this.GetValue(OverviewContentHolder.MaximumZoomProperty);
    set => this.SetValue(OverviewContentHolder.MaximumZoomProperty, (object) value);
  }

  public ZoomMode ZoomMode
  {
    get => (ZoomMode) this.GetValue(OverviewContentHolder.ZoomModeProperty);
    set => this.SetValue(OverviewContentHolder.ZoomModeProperty, (object) value);
  }

  public bool IsPanEnabled
  {
    get => (bool) this.GetValue(OverviewContentHolder.IsPanEnabledProperty);
    set => this.SetValue(OverviewContentHolder.IsPanEnabledProperty, (object) value);
  }

  private static void OnIsPanEnabledChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if ((d as OverviewContentHolder).IsPanEnabled)
      return;
    (d as OverviewContentHolder).m_PanStartPosition = new Point?();
  }

  private bool CanZoomInExecute(object parameter) => this.IsZoomInEnabled;

  private bool CanZoomOutExecute(object parameter) => this.IsZoomOutEnabled;

  private bool CanZoomToExecute(object parameter) => this.IsZoomToEnabled;

  private bool CanZoomResetExecute(object parameter) => this.IsZoomResetEnabled;

  private void ExecuteZoomInCommand(object param)
  {
    object zoomParameter = this.GetZoomParameter(param);
    if (zoomParameter == null)
      return;
    this.CheckZoomStack(zoomParameter);
    if (zoomParameter is IZoomPositionParameter)
      this.UnitScale += (zoomParameter as IZoomPositionParameter).ZoomFactor ?? this.ZoomFactor;
    else
      this.UnitScale += this.ZoomFactor;
  }

  private void ExecuteZoomOutCommand(object param)
  {
    object zoomParameter = this.GetZoomParameter(param);
    if (zoomParameter == null)
      return;
    this.CheckZoomStack(zoomParameter);
    if (zoomParameter is IZoomPositionParameter)
      this.UnitScale -= (zoomParameter as IZoomPositionParameter).ZoomFactor ?? this.ZoomFactor;
    else
      this.UnitScale -= this.ZoomFactor;
  }

  private void CheckZoomStack(object parameter)
  {
    Point? nullable = new Point?();
    if (parameter is IZoomPositionParameter)
    {
      nullable = (parameter as IZoomPositionParameter).FocusPoint;
      nullable = new Point?(new Point(nullable.Value.X * this.UnitScale, nullable.Value.Y * this.UnitScale));
    }
    if (this.zoomStack.Count != 0)
      return;
    OverviewContentHolder.ZoomOperation zoomOperation = new OverviewContentHolder.ZoomOperation();
    if (this.zoomStack.Count == 0)
    {
      zoomOperation.mousePos = nullable ?? new Point(this.m_HorizontalOffset + this.m_ViewportWidth / 2.0, this.m_VerticalOffset + this.m_ViewportHeight / 2.0);
      zoomOperation.ratio = new Point(zoomOperation.mousePos.X / this.ExtentWidth, zoomOperation.mousePos.Y / this.ExtentHeight);
    }
    this.zoomStack.Push(zoomOperation);
  }

  private void ExecuteZoomResetCommand(object parameter)
  {
    this.CheckZoomStack(parameter);
    this.UnitScale = 1.0;
  }

  private void ExecuteZoomToCommand(object parameter)
  {
    this.CheckZoomStack(parameter);
    if (!(parameter is IZoomPositionParameter))
      return;
    this.UnitScale = (parameter as IZoomPositionParameter).ZoomTo ?? 1.0;
  }

  private void UpdatePageBackground()
  {
    if (this.PART_PageBackground == null)
      return;
    this.PART_PageBackground.Margin = new Thickness(-this.TopLeft.X, -this.TopLeft.Y, 0.0, 0.0);
  }

  public void FitToPage()
  {
    double num1 = this.ViewportWidth * this.Scale / this.ExtentWidth;
    double num2 = this.ViewportHeight * this.Scale / this.ExtentHeight;
    this.SetValue(OverviewContentHolder.ScaleProperty, (object) (num1 < num2 ? num1 : num2));
    this.SetHorizontalOffset(0.0);
    this.SetVerticalOffset(0.0);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.PART_Grid = this.GetTemplateChild("PART_Grid") as Grid;
    this.PART_PageBackground = this.GetTemplateChild("PART_PageBackground") as Rectangle;
    this.UpdatePageBackground();
    if (this.PART_Grid.Resources != null && this.PART_Grid.Resources.Count > 0)
    {
      this._storyboard = this.PART_Grid.Resources[(object) "ZoomPanStoryboard"] as Storyboard;
      this._scaleXAnimation = this._storyboard.Children[0] as DoubleAnimation;
      this._scaleYAnimation = this._storyboard.Children[1] as DoubleAnimation;
      this._translateXAnimation = this._storyboard.Children[2] as DoubleAnimation;
      this._translateYAnimation = this._storyboard.Children[3] as DoubleAnimation;
    }
    this._translateTransform = this.GetTemplateChild("PART_PanTransform") as TranslateTransform;
    this._scaleTransform = this.GetTemplateChild("PART_ZoomTransform") as ScaleTransform;
    this.diagramView = this.GetDiagramView((DependencyObject) this);
    if (this.diagramView != null)
    {
      this.diagramView.PreviewMouseWheel += new MouseWheelEventHandler(this.diagramView_PreviewMouseWheel);
      this.diagramView.AddHandler(UIElement.MouseLeftButtonDownEvent, (Delegate) new MouseButtonEventHandler(this.diagramView_MouseLeftButtonDown), true);
    }
    Grid partGrid = this.PART_Grid;
  }

  private void diagramView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (this.CheckScrollBar(e.OriginalSource as DependencyObject) != null)
      return;
    this.SetPanStartPoint(e);
  }

  private ScrollBar CheckScrollBar(DependencyObject element)
  {
    DependencyObject parent;
    while (true)
    {
      parent = VisualTreeHelper.GetParent(element);
      if (parent != null)
      {
        if (!(parent is ScrollBar))
          element = parent;
        else
          break;
      }
      else
        goto label_4;
    }
    return (ScrollBar) parent;
label_4:
    return (ScrollBar) null;
  }

  private ContentControl GetDiagramView(DependencyObject element)
  {
    while (element != null && !(element.DependencyObjectType.Name == "DiagramView"))
      element = VisualTreeHelper.GetParent(element);
    return element as ContentControl;
  }

  private void diagramView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
  {
    this.PerformZoom((MouseEventArgs) e);
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    this.SetViewportWidth(availableSize.Width);
    this.SetViewportHeight(availableSize.Height);
    Size size1 = base.MeasureOverride(new Size(double.PositiveInfinity, double.PositiveInfinity));
    Size size2 = new Size(Math.Abs(size1.Width * this.UnitScale), Math.Abs(size1.Height * this.UnitScale));
    this.SetExtentWidth(this.TopLeft.X * this.UnitScale + size2.Width);
    this.SetExtentHeight(this.TopLeft.Y * this.UnitScale + size2.Height);
    this.VerifyScrollData();
    this.ScrollOwner.InvalidateScrollInfo();
    OverviewContentHolder.OverViewFitToPageEventArgs e = new OverviewContentHolder.OverViewFitToPageEventArgs(true);
    e.RoutedEvent = OverviewContentHolder.FitToPageEvent;
    e.Source = (object) this;
    this.RaiseEvent((RoutedEventArgs) e);
    if (e.Cancel && this.EnableFitToPage)
      this.FitToPage();
    double width = this.ExtentWidth > this.ViewportWidth ? this.ExtentWidth : this.ViewportWidth;
    double height = this.ExtentHeight > this.ViewportHeight ? this.ExtentHeight : this.ViewportHeight;
    return DesignerProperties.GetIsInDesignMode((DependencyObject) this) ? size1 : new Size(width, height);
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    this.ApplyAnimation(base.ArrangeOverride(finalSize));
    this.VerifyScrollData();
    this.ScrollOwner.InvalidateScrollInfo();
    return new Size(this.ExtentWidth > this.ViewportWidth ? this.ExtentWidth : this.ViewportWidth, this.ExtentHeight > this.ViewportHeight ? this.ExtentHeight : this.ViewportHeight);
  }

  private void ApplyAnimation(Size actualSize)
  {
    if (this.zoomStack.Count > 0)
    {
      OverviewContentHolder.ZoomOperation zoomOperation = this.zoomStack.Pop();
      this.SetHorizontalOffset(this.m_HorizontalOffset + this.ExtentWidth * zoomOperation.ratio.X - zoomOperation.mousePos.X);
      this.SetVerticalOffset(this.m_VerticalOffset + this.ExtentHeight * zoomOperation.ratio.Y - zoomOperation.mousePos.Y);
    }
    if (double.IsNaN(this.UnitScale))
      this.UnitScale = 30.0;
    Rect rect = new Rect();
    if (this.UnitScale > 0.0)
      rect = new Rect(-(this.HorizontalOffset / this.UnitScale - this.TopLeft.X) * this.UnitScale, -(this.VerticalOffset / this.UnitScale - this.TopLeft.Y) * this.UnitScale, actualSize.Width, actualSize.Height);
    this._scaleXAnimation.From = new double?(this._scaleTransform.ScaleX);
    this._scaleXAnimation.To = new double?(this.UnitScale);
    this._scaleYAnimation.From = new double?(this._scaleTransform.ScaleY);
    this._scaleYAnimation.To = new double?(this.UnitScale);
    if (this.AnimationEnabled)
      this._scaleYAnimation.Duration = this._scaleXAnimation.Duration = this._animationDuration;
    else
      this._scaleYAnimation.Duration = this._scaleXAnimation.Duration = new Duration(new TimeSpan(0L));
    this._translateXAnimation.From = new double?(this._translateTransform.X);
    this._translateXAnimation.To = new double?(rect.Left);
    this._translateYAnimation.From = new double?(this._translateTransform.Y);
    this._translateYAnimation.To = new double?(rect.Top);
    if (this.AnimationEnabled)
      this._translateXAnimation.Duration = this._translateYAnimation.Duration = this._animationDuration;
    else
      this._translateXAnimation.Duration = this._translateYAnimation.Duration = new Duration(new TimeSpan(0L));
    this._storyboard.Begin();
  }

  public bool CanHorizontallyScroll
  {
    get => this.m_CanHorizontallyScroll;
    set => this.m_CanHorizontallyScroll = value;
  }

  public bool CanVerticallyScroll
  {
    get => this.m_CanVerticallyScroll;
    set => this.m_CanVerticallyScroll = value;
  }

  public void LineDown()
  {
    this.SetVerticalOffset(this.VerticalOffset + 16.0);
    this.VerifyAndInvalidateScrollData();
  }

  public void LineLeft()
  {
    this.SetHorizontalOffset(this.HorizontalOffset - 16.0);
    this.VerifyAndInvalidateScrollData();
  }

  public void LineRight()
  {
    this.SetHorizontalOffset(this.HorizontalOffset + 16.0);
    this.VerifyAndInvalidateScrollData();
  }

  public void LineUp()
  {
    this.SetVerticalOffset(this.VerticalOffset - 16.0);
    this.VerifyAndInvalidateScrollData();
  }

  public void MouseWheelDown()
  {
    this.SetVerticalOffset(this.VerticalOffset + 48.0);
    this.VerifyAndInvalidateScrollData();
  }

  public void MouseWheelLeft()
  {
    this.SetHorizontalOffset(this.HorizontalOffset - 48.0);
    this.VerifyAndInvalidateScrollData();
  }

  public void MouseWheelRight()
  {
    this.SetHorizontalOffset(this.HorizontalOffset + 48.0);
    this.VerifyAndInvalidateScrollData();
  }

  public void MouseWheelUp()
  {
    this.SetVerticalOffset(this.VerticalOffset - 48.0);
    this.VerifyAndInvalidateScrollData();
  }

  public void PageDown() => this.SetVerticalOffset(this.VerticalOffset + this.ViewportHeight);

  public void PageLeft() => this.SetHorizontalOffset(this.HorizontalOffset - this.ViewportWidth);

  public void PageRight() => this.SetHorizontalOffset(this.HorizontalOffset + this.ViewportWidth);

  public void PageUp() => this.SetVerticalOffset(this.VerticalOffset - this.ViewportHeight);

  public ScrollViewer ScrollOwner
  {
    get => this.m_ScrollOwner;
    set => this.m_ScrollOwner = value;
  }

  public double ExtentHeight => this.m_ExtentHeight;

  public double ExtentWidth => this.m_ExtentWidth;

  public double HorizontalOffset => this.m_HorizontalOffset;

  public double VerticalOffset => this.m_VerticalOffset;

  public double ViewportHeight => this.m_ViewportHeight;

  public double ViewportWidth => this.m_ViewportWidth;

  public void SetHorizontalOffset(double offset)
  {
    if (this.m_HorizontalOffset == offset)
      return;
    double horizontalOffset = this.m_HorizontalOffset;
    this.m_HorizontalOffset = offset;
    this.VerifyAndInvalidateScrollData();
    this.InvokeScrollChangedEvent(ScrollParamether.HorizontalOffset, horizontalOffset, offset);
  }

  public void SetVerticalOffset(double offset)
  {
    if (this.m_VerticalOffset == offset)
      return;
    double verticalOffset = this.m_VerticalOffset;
    this.m_VerticalOffset = offset;
    this.VerifyAndInvalidateScrollData();
    this.InvokeScrollChangedEvent(ScrollParamether.VerticalOffset, verticalOffset, offset);
  }

  private void SetViewportWidth(double newValue)
  {
    double viewportWidth = this.m_ViewportWidth;
    this.m_ViewportWidth = newValue;
    this.InvokeScrollChangedEvent(ScrollParamether.ViewportWidth, viewportWidth, newValue);
  }

  private void SetViewportHeight(double newValue)
  {
    double viewportHeight = this.m_ViewportHeight;
    this.m_ViewportHeight = newValue;
    this.InvokeScrollChangedEvent(ScrollParamether.ViewportHeight, viewportHeight, newValue);
  }

  private void SetExtentWidth(double newValue)
  {
    double extentWidth = this.m_ExtentWidth;
    this.m_ExtentWidth = newValue;
    this.InvokeScrollChangedEvent(ScrollParamether.ExtentWidth, extentWidth, newValue);
  }

  private void SetExtentHeight(double newValue)
  {
    double extentHeight = this.m_ExtentHeight;
    this.m_ExtentHeight = newValue;
    this.InvokeScrollChangedEvent(ScrollParamether.ExtentHeight, extentHeight, newValue);
  }

  private void VerifyScrollData()
  {
    if (this.IsPanEnabled && this.m_PanStartPosition.HasValue)
    {
      if (this.m_HorizontalOffset < 0.0 && this.m_VerticalOffset < 0.0)
      {
        Point origin = OverviewContentHolder.GetOrigin((DependencyObject) this.Content);
        OverviewContentHolder.SetOrigin((DependencyObject) this.Content, new Point(origin.X + this.m_HorizontalOffset, origin.Y + this.m_VerticalOffset));
      }
      else if (this.m_HorizontalOffset < 0.0)
      {
        Point origin = OverviewContentHolder.GetOrigin((DependencyObject) this.Content);
        OverviewContentHolder.SetOrigin((DependencyObject) this.Content, new Point(origin.X + this.m_HorizontalOffset, origin.Y));
      }
      else if (this.m_VerticalOffset < 0.0)
      {
        Point origin = OverviewContentHolder.GetOrigin((DependencyObject) this.Content);
        OverviewContentHolder.SetOrigin((DependencyObject) this.Content, new Point(origin.X, origin.Y + this.m_VerticalOffset));
      }
      else
      {
        OverviewContentHolder.GetOrigin((DependencyObject) this.Content);
        double height = this.m_VerticalOffset + this.m_ViewportHeight - this.ExtentHeight;
        double width = this.m_HorizontalOffset + this.m_ViewportWidth - this.ExtentWidth;
        if (width > 0.0 || height > 0.0)
        {
          if (width < 0.0)
            width = 0.0;
          if (height < 0.0)
            height = 0.0;
          OverviewContentHolder.ExtraPanningEventEventArgs e = new OverviewContentHolder.ExtraPanningEventEventArgs(true);
          e.RoutedEvent = OverviewContentHolder.ExtraPanningEvent;
          e.Source = (object) this;
          e.ExtraSize = new Size(width, height);
          this.RaiseEvent((RoutedEventArgs) e);
        }
      }
    }
    double val2_1 = Math.Max(0.0, this.m_HorizontalOffset);
    double offset = Math.Min(Math.Max(0.0, this.ExtentWidth - this.ViewportWidth), val2_1);
    if (double.IsNaN(this.m_HorizontalOffset) || double.IsInfinity(this.m_HorizontalOffset))
      offset = 0.0;
    if (offset != this.m_HorizontalOffset)
      this.SetHorizontalOffset(offset);
    double val2_2 = Math.Max(0.0, this.m_VerticalOffset);
    double num = Math.Min(Math.Max(0.0, this.ExtentHeight - this.ViewportHeight), val2_2);
    if (double.IsNaN(num) || double.IsInfinity(num))
      num = 0.0;
    if (num != this.m_VerticalOffset)
      this.SetVerticalOffset(num);
    this.ScrollOwner.InvalidateScrollInfo();
  }

  private void VerifyAndInvalidateScrollData()
  {
    this.VerifyScrollData();
    this.ScrollOwner.InvalidateScrollInfo();
    this.InvalidateArrange();
  }

  public event OverviewContentHolder.OverviewFitPageEventHandler UpdateFitToPage
  {
    add => this.AddHandler(OverviewContentHolder.FitToPageEvent, (Delegate) value);
    remove => this.RemoveHandler(OverviewContentHolder.FitToPageEvent, (Delegate) value);
  }

  public event OverviewContentHolder.ExtraPanningEventEventHandler ExtraPanning
  {
    add => this.AddHandler(OverviewContentHolder.ExtraPanningEvent, (Delegate) value);
    remove => this.RemoveHandler(OverviewContentHolder.ExtraPanningEvent, (Delegate) value);
  }

  private void InvokeScrollChangedEvent(ScrollParamether param, double oldValue, double newValue)
  {
  }

  public Rect MakeVisible(Visual visual, Rect rectangle)
  {
    return new Rect(visual.TransformToAncestor((Visual) this).Transform(new Point(0.0, 0.0)), new Size());
  }

  internal class Inverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value is Point point ? (object) new Point(-point.X, -point.Y) : (object) null;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  internal class ZoomOperation
  {
    internal Point mousePos;
    internal Point ratio;
  }

  public delegate void OverviewFitPageEventHandler(
    object sender,
    OverviewContentHolder.OverViewFitToPageEventArgs evtArgs);

  public class OverViewFitToPageEventArgs : RoutedEventArgs
  {
    private bool cc;

    public bool Cancel
    {
      get => this.cc;
      set
      {
        if (value && this.Source != null && (this.Source as OverviewContentHolder).EnableFitToPage)
          (this.Source as OverviewContentHolder).FitToPage();
        this.cc = value;
      }
    }

    public OverViewFitToPageEventArgs(bool fittopage) => this.Cancel = fittopage;
  }

  public delegate void ExtraPanningEventEventHandler(
    object sender,
    OverviewContentHolder.ExtraPanningEventEventArgs evtArgs);

  public class ExtraPanningEventEventArgs : RoutedEventArgs
  {
    private Size cc;

    public Size ExtraSize
    {
      get => this.cc;
      set => this.cc = value;
    }

    public ExtraPanningEventEventArgs(bool ExtraSize)
    {
    }
  }
}
