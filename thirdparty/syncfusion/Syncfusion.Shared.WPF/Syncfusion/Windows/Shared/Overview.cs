// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Overview
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class Overview : Control, IOverviewPanel
{
  internal bool IsResizing;
  internal bool IsResized;
  public static readonly DependencyProperty AllowResizeProperty = DependencyProperty.Register(nameof (AllowResize), typeof (bool), typeof (Overview), new PropertyMetadata((object) true));
  public static readonly DependencyProperty IsPanEnabledProperty = DependencyProperty.Register(nameof (IsPanEnabled), typeof (bool), typeof (Overview), new PropertyMetadata((object) false));
  public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(nameof (Scale), typeof (double), typeof (Overview), new PropertyMetadata((object) 1.0));
  public static readonly DependencyProperty IsZoomInEnabledProperty = DependencyProperty.Register(nameof (IsZoomInEnabled), typeof (bool), typeof (Overview), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ZoomInProperty = DependencyProperty.Register(nameof (ZoomIn), typeof (ICommand), typeof (Overview), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsZoomOutEnabledProperty = DependencyProperty.Register(nameof (IsZoomOutEnabled), typeof (bool), typeof (Overview), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ZoomOutProperty = DependencyProperty.Register(nameof (ZoomOut), typeof (ICommand), typeof (Overview), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsZoomToEnabledProperty = DependencyProperty.Register(nameof (IsZoomToEnabled), typeof (bool), typeof (Overview), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ZoomToProperty = DependencyProperty.Register(nameof (ZoomTo), typeof (ICommand), typeof (Overview), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsZoomResetEnabledProperty = DependencyProperty.Register(nameof (IsZoomResetEnabled), typeof (bool), typeof (Overview), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ZoomResetProperty = DependencyProperty.Register(nameof (ZoomReset), typeof (ICommand), typeof (Overview), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ZoomFactorProperty = DependencyProperty.Register(nameof (ZoomFactor), typeof (double), typeof (Overview), new PropertyMetadata((object) 0.2));
  public static readonly DependencyProperty MinimumZoomProperty = DependencyProperty.Register(nameof (MinimumZoom), typeof (double), typeof (Overview), new PropertyMetadata((object) 0.2));
  public static readonly DependencyProperty MaximumZoomProperty = DependencyProperty.Register(nameof (MaximumZoom), typeof (double), typeof (Overview), new PropertyMetadata((object) 10.0));
  public static readonly DependencyProperty ZoomModeProperty = DependencyProperty.Register(nameof (ZoomMode), typeof (ZoomMode), typeof (Overview), new PropertyMetadata((object) ZoomMode.Unit));
  public static readonly DependencyProperty ContentBackgroundProperty = DependencyProperty.Register(nameof (ContentBackground), typeof (Brush), typeof (Overview), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ViewPortBrushProperty = DependencyProperty.Register(nameof (ViewPortBrush), typeof (Brush), typeof (Overview), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty OverviewSourceAncestorProperty = DependencyProperty.Register(nameof (OverviewSourceAncestor), typeof (DependencyObject), typeof (Overview), new PropertyMetadata((object) null, new PropertyChangedCallback(Overview.OnOverviewSourceAncestorChanged)));
  internal static readonly DependencyProperty ScrollSourceProperty = DependencyProperty.Register(nameof (ScrollSource), typeof (OverviewContentHolder), typeof (Overview), new PropertyMetadata((object) null, new PropertyChangedCallback(Overview.OnScrollSourceChanged)));
  internal static readonly DependencyProperty ScrollContentTargetProperty = DependencyProperty.Register(nameof (ScrollContentTarget), typeof (DependencyObject), typeof (Overview), new PropertyMetadata((object) null, new PropertyChangedCallback(Overview.OnScrollContentTargetChanged)));
  internal static readonly DependencyProperty VpOffsetXProperty = DependencyProperty.Register(nameof (VpOffsetX), typeof (double), typeof (Overview), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(Overview.OnVpOffsetChanged)));
  internal static readonly DependencyProperty VpOffsetYProperty = DependencyProperty.Register(nameof (VpOffsetY), typeof (double), typeof (Overview), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(Overview.OnVpOffsetChanged)));
  internal static readonly DependencyProperty VpWidthProperty = DependencyProperty.Register(nameof (VpWidth), typeof (double), typeof (Overview), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(Overview.OnVpOffsetChanged)));
  internal static readonly DependencyProperty VpHeightProperty = DependencyProperty.Register(nameof (VpHeight), typeof (double), typeof (Overview), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(Overview.OnVpOffsetChanged)));
  internal static readonly DependencyProperty WindowWidthProperty = DependencyProperty.Register(nameof (WindowWidth), typeof (double), typeof (Overview), new PropertyMetadata((object) 0.0));
  internal static readonly DependencyProperty WindowHeightProperty = DependencyProperty.Register(nameof (WindowHeight), typeof (double), typeof (Overview), new PropertyMetadata((object) 0.0));
  internal static readonly DependencyProperty TransProperty = DependencyProperty.Register(nameof (Trans), typeof (Transform), typeof (Overview), new PropertyMetadata((PropertyChangedCallback) null));
  private EventHandler ScrollViewer_LayoutUpdated;
  private Point m_DragDelta = new Point(0.0, 0.0);

  public bool AllowResize
  {
    get => (bool) this.GetValue(Overview.AllowResizeProperty);
    set => this.SetValue(Overview.AllowResizeProperty, (object) value);
  }

  public bool IsPanEnabled
  {
    get => (bool) this.GetValue(Overview.IsPanEnabledProperty);
    set => this.SetValue(Overview.IsPanEnabledProperty, (object) value);
  }

  public Overview()
  {
    this.DefaultStyleKey = (object) typeof (Overview);
    this.InternalBinding("ScrollSource.ZoomIn", Overview.ZoomInProperty);
    this.InternalBinding("ScrollSource.ZoomOut", Overview.ZoomOutProperty);
    this.InternalBinding("ScrollSource.ZoomTo", Overview.ZoomToProperty);
    this.InternalBinding("ScrollSource.ZoomReset", Overview.ZoomResetProperty);
    this.InternalBinding("ScrollSource.ZoomFactor", Overview.ZoomFactorProperty);
    this.InternalBinding("ScrollSource.MinimumZoom", Overview.MinimumZoomProperty);
    this.InternalBinding("ScrollSource.MaximumZoom", Overview.MaximumZoomProperty);
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  private void InternalBinding(string sourceProp, DependencyProperty dpProp)
  {
    this.SetBinding(dpProp, (BindingBase) new Binding(sourceProp)
    {
      Source = (object) this
    });
  }

  public double Scale
  {
    get => (double) this.GetValue(Overview.ScaleProperty);
    set => this.SetValue(Overview.ScaleProperty, (object) value);
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

  public bool IsZoomInEnabled
  {
    get => (bool) this.GetValue(Overview.IsZoomInEnabledProperty);
    set => this.SetValue(Overview.IsZoomInEnabledProperty, (object) value);
  }

  public ICommand ZoomIn
  {
    get => (ICommand) this.GetValue(Overview.ZoomInProperty);
    set => this.SetValue(Overview.ZoomInProperty, (object) value);
  }

  public bool IsZoomOutEnabled
  {
    get => (bool) this.GetValue(Overview.IsZoomOutEnabledProperty);
    set => this.SetValue(Overview.IsZoomOutEnabledProperty, (object) value);
  }

  public ICommand ZoomOut
  {
    get => (ICommand) this.GetValue(Overview.ZoomOutProperty);
    set => this.SetValue(Overview.ZoomOutProperty, (object) value);
  }

  public bool IsZoomToEnabled
  {
    get => (bool) this.GetValue(Overview.IsZoomToEnabledProperty);
    set => this.SetValue(Overview.IsZoomToEnabledProperty, (object) value);
  }

  public ICommand ZoomTo
  {
    get => (ICommand) this.GetValue(Overview.ZoomToProperty);
    set => this.SetValue(Overview.ZoomToProperty, (object) value);
  }

  public bool IsZoomResetEnabled
  {
    get => (bool) this.GetValue(Overview.IsZoomResetEnabledProperty);
    set => this.SetValue(Overview.IsZoomResetEnabledProperty, (object) value);
  }

  public ICommand ZoomReset
  {
    get => (ICommand) this.GetValue(Overview.ZoomResetProperty);
    set => this.SetValue(Overview.ZoomResetProperty, (object) value);
  }

  public double ZoomFactor
  {
    get => (double) this.GetValue(Overview.ZoomFactorProperty);
    set => this.SetValue(Overview.ZoomFactorProperty, (object) value);
  }

  public double MinimumZoom
  {
    get => (double) this.GetValue(Overview.MinimumZoomProperty);
    set => this.SetValue(Overview.MinimumZoomProperty, (object) value);
  }

  public double MaximumZoom
  {
    get => (double) this.GetValue(Overview.MaximumZoomProperty);
    set => this.SetValue(Overview.MaximumZoomProperty, (object) value);
  }

  public ZoomMode ZoomMode
  {
    get => (ZoomMode) this.GetValue(Overview.ZoomModeProperty);
    set => this.SetValue(Overview.ZoomModeProperty, (object) value);
  }

  public Brush ContentBackground
  {
    get => (Brush) this.GetValue(Overview.ContentBackgroundProperty);
    set => this.SetValue(Overview.ContentBackgroundProperty, (object) value);
  }

  public Brush ViewPortBrush
  {
    get => (Brush) this.GetValue(Overview.ViewPortBrushProperty);
    set => this.SetValue(Overview.ViewPortBrushProperty, (object) value);
  }

  public DependencyObject OverviewSourceAncestor
  {
    get => (DependencyObject) this.GetValue(Overview.OverviewSourceAncestorProperty);
    set => this.SetValue(Overview.OverviewSourceAncestorProperty, (object) value);
  }

  private static void OnOverviewSourceAncestorChanged(
    DependencyObject dp,
    DependencyPropertyChangedEventArgs evtArgs)
  {
    if (evtArgs.NewValue == null)
      return;
    (dp as Overview).UpdateSource();
  }

  private static T FindChildScrollViewer<T>(DependencyObject depObj) where T : UIElement
  {
    if (depObj is T)
      return depObj as T;
    int childrenCount = VisualTreeHelper.GetChildrenCount(depObj);
    while (childrenCount > 0)
    {
      DependencyObject child = VisualTreeHelper.GetChild(depObj, childrenCount - 1);
      --childrenCount;
      if (child is T)
        return child as T;
      if (child == null)
        return default (T);
      DependencyObject childScrollViewer = (DependencyObject) Overview.FindChildScrollViewer<T>(child);
      if (childScrollViewer is T)
        return childScrollViewer as T;
    }
    return default (T);
  }

  internal OverviewContentHolder ScrollSource
  {
    get => (OverviewContentHolder) this.GetValue(Overview.ScrollSourceProperty);
    set => this.SetValue(Overview.ScrollSourceProperty, (object) value);
  }

  internal DependencyObject ScrollContentTarget
  {
    get => (DependencyObject) this.GetValue(Overview.ScrollContentTargetProperty);
    set => this.SetValue(Overview.ScrollContentTargetProperty, (object) value);
  }

  private static void OnScrollContentTargetChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.OldValue != null && e.OldValue is FrameworkElement)
      (e.OldValue as FrameworkElement).Unloaded -= new RoutedEventHandler((d as Overview).Overview_Unloaded);
    if (e.NewValue == null || !(e.NewValue is FrameworkElement))
      return;
    (e.NewValue as FrameworkElement).Unloaded += new RoutedEventHandler((d as Overview).Overview_Unloaded);
  }

  private void Overview_Unloaded(object sender, RoutedEventArgs e)
  {
    (sender as FrameworkElement).Unloaded -= new RoutedEventHandler(this.Overview_Unloaded);
    this.UpdateScrollContentTarget();
  }

  private static void OnScrollSourceChanged(
    DependencyObject dp,
    DependencyPropertyChangedEventArgs evtArgs)
  {
    Overview overview = dp as Overview;
    overview.UpdateScrollContentTarget();
    if (evtArgs.NewValue == null)
      return;
    OverviewContentHolder newValue = evtArgs.NewValue as OverviewContentHolder;
    newValue._overviewParent = overview;
    newValue.SetBinding(OverviewContentHolder.ScaleProperty, (BindingBase) new Binding()
    {
      Source = (object) overview,
      Mode = BindingMode.TwoWay,
      Path = new PropertyPath("Scale", new object[0])
    });
    newValue.ScrollOwner.ScrollChanged += new ScrollChangedEventHandler(overview.holder_ScrollChanged);
  }

  private void holder_ScrollChanged(object sender, ScrollChangedEventArgs e) => this.UpdateVp();

  internal double VpOffsetX
  {
    get => (double) this.GetValue(Overview.VpOffsetXProperty);
    set => this.SetValue(Overview.VpOffsetXProperty, (object) value);
  }

  private static void OnVpOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    Overview overview = d as Overview;
    overview.WindowWidth = overview.VpOffsetX >= 0.0 ? overview.VpWidth : overview.VpWidth + overview.VpOffsetX;
    if (overview.VpOffsetY < 0.0)
      overview.WindowHeight = overview.VpHeight + overview.VpOffsetY;
    else
      overview.WindowHeight = overview.VpHeight;
  }

  internal double VpOffsetY
  {
    get => (double) this.GetValue(Overview.VpOffsetYProperty);
    set => this.SetValue(Overview.VpOffsetYProperty, (object) value);
  }

  internal double VpWidth
  {
    get => (double) this.GetValue(Overview.VpWidthProperty);
    set
    {
      value = Math.Max(5.0, value);
      this.SetValue(Overview.VpWidthProperty, (object) value);
    }
  }

  internal double VpHeight
  {
    get => (double) this.GetValue(Overview.VpHeightProperty);
    set
    {
      value = Math.Max(5.0, value);
      this.SetValue(Overview.VpHeightProperty, (object) value);
    }
  }

  internal double WindowWidth
  {
    get => (double) this.GetValue(Overview.WindowWidthProperty);
    set => this.SetValue(Overview.WindowWidthProperty, (object) value);
  }

  internal double WindowHeight
  {
    get => (double) this.GetValue(Overview.WindowHeightProperty);
    set => this.SetValue(Overview.WindowHeightProperty, (object) value);
  }

  internal Transform Trans
  {
    get => (Transform) this.GetValue(Overview.TransProperty);
    set => this.SetValue(Overview.TransProperty, (object) value);
  }

  internal Panel Img { get; set; }

  private void Img_SizeChanged(object sender, SizeChangedEventArgs e) => this.UpdateVp();

  private void UpdateVp()
  {
    if (this.IsResizing || this.ScrollSource == null)
      return;
    this.ScrollSource.LayoutUpdated -= new EventHandler(this.Overview_LayoutUpdated_UpdateVP);
    this.ScrollSource.LayoutUpdated += new EventHandler(this.Overview_LayoutUpdated_UpdateVP);
    this.ScrollSource.InvalidateArrange();
  }

  internal void InvalidateScroll()
  {
    if (this.ScrollSource == null)
      return;
    double num1 = this.ScrollSource.HorizontalOffset / this.ScrollSource.ExtentWidth;
    double num2 = this.ScrollSource.ViewportWidth / this.ScrollSource.ExtentWidth;
    double num3 = (this.ScrollSource.ExtentWidth - this.ScrollSource.HorizontalOffset - this.ScrollSource.ViewportWidth) / this.ScrollSource.ExtentWidth;
    double num4 = this.ScrollSource.VerticalOffset / this.ScrollSource.ExtentHeight;
    double num5 = this.ScrollSource.ViewportHeight / this.ScrollSource.ExtentHeight;
    double num6 = (this.ScrollSource.ExtentHeight - this.ScrollSource.VerticalOffset - this.ScrollSource.ViewportHeight) / this.ScrollSource.ExtentHeight;
    if (this.Img == null || this.Img == null)
      return;
    Size uniformImageSize = this.GetUniformImageSize();
    this.VpOffsetX = num1 * uniformImageSize.Width;
    this.VpOffsetY = num4 * uniformImageSize.Height;
    this.VpWidth = num2 * uniformImageSize.Width;
    this.VpHeight = num5 * uniformImageSize.Height;
    this.Trans = (Transform) new TranslateTransform()
    {
      X = this.VpOffsetX,
      Y = this.VpOffsetY
    };
  }

  internal void UpdateSourceScroll()
  {
    if (this.ScrollSource == null)
      return;
    Size uniformImageSize = this.GetUniformImageSize();
    double width = uniformImageSize.Width;
    double height = uniformImageSize.Height;
    double vpHeight = this.VpHeight;
    double vpWidth = this.VpWidth;
    double vpOffsetX = this.VpOffsetX;
    double vpOffsetY = this.VpOffsetY;
    double num1 = this.ScrollSource.ViewportWidth * width / vpWidth;
    double num2 = num1 / this.ScrollSource.ExtentWidth;
    double offset1 = num1 * vpOffsetX / width;
    double offset2 = this.ScrollSource.ViewportHeight * height / vpHeight * vpOffsetY / height;
    if (0.99 < num2 && num2 < 1.01)
    {
      this.ScrollSource.SetHorizontalOffset(offset1);
      this.ScrollSource.SetVerticalOffset(offset2);
    }
    else
      this.UnitScale *= num2;
  }

  internal void UpdateScrollViewer()
  {
    this.LayoutUpdated -= new EventHandler(this.Overview_LayoutUpdated_UpdateSV);
    this.LayoutUpdated += new EventHandler(this.Overview_LayoutUpdated_UpdateSV);
    this.InvalidateArrange();
  }

  private void Overview_LayoutUpdated_UpdateVP(object sender, EventArgs e)
  {
    this.ScrollSource.LayoutUpdated -= new EventHandler(this.Overview_LayoutUpdated_UpdateVP);
    if (this.ScrollSource == null)
      return;
    double num1 = this.ScrollSource.HorizontalOffset / this.ScrollSource.ExtentWidth;
    double num2 = this.ScrollSource.ViewportWidth / this.ScrollSource.ExtentWidth;
    double num3 = (this.ScrollSource.ExtentWidth - this.ScrollSource.HorizontalOffset - this.ScrollSource.ViewportWidth) / this.ScrollSource.ExtentWidth;
    double num4 = this.ScrollSource.VerticalOffset / this.ScrollSource.ExtentHeight;
    double num5 = this.ScrollSource.ViewportHeight / this.ScrollSource.ExtentHeight;
    double num6 = (this.ScrollSource.ExtentHeight - this.ScrollSource.VerticalOffset - this.ScrollSource.ViewportHeight) / this.ScrollSource.ExtentHeight;
    if (this.Img == null)
      return;
    Size uniformImageSize = this.GetUniformImageSize();
    this.VpOffsetX = num1 * uniformImageSize.Width;
    this.VpOffsetY = num4 * uniformImageSize.Height;
    this.VpWidth = num2 * uniformImageSize.Width;
    this.VpHeight = num5 * uniformImageSize.Height;
    this.Trans = (Transform) new TranslateTransform()
    {
      X = this.VpOffsetX,
      Y = this.VpOffsetY
    };
  }

  private void Overview_LayoutUpdated_UpdateSV(object sender, EventArgs e)
  {
    this.LayoutUpdated -= new EventHandler(this.Overview_LayoutUpdated_UpdateSV);
    if (this.Img == null || this.Img == null || this.ScrollSource == null)
      return;
    Size uniformImageSize = this.GetUniformImageSize();
    double width = uniformImageSize.Width;
    double height = uniformImageSize.Height;
    double vpHeight = this.VpHeight;
    double vpWidth = this.VpWidth;
    double vpOffsetX = this.VpOffsetX;
    double vpOffsetY = this.VpOffsetY;
    double num1 = this.ScrollSource.ViewportWidth * width / vpWidth;
    double num2 = num1 / this.ScrollSource.ExtentWidth;
    double SVh = num1 * vpOffsetX / width;
    double SVv = this.ScrollSource.ViewportHeight * height / vpHeight * vpOffsetY / height;
    if (0.99 < num2 && num2 < 1.01)
    {
      this.ScrollSource.SetHorizontalOffset(SVh);
      this.ScrollSource.SetVerticalOffset(SVv);
    }
    else
    {
      this.UnitScale *= num2;
      if (this.ScrollViewer_LayoutUpdated != null)
        this.ScrollSource.LayoutUpdated -= this.ScrollViewer_LayoutUpdated;
      this.ScrollViewer_LayoutUpdated = (EventHandler) ((s, evt) =>
      {
        this.ScrollSource.LayoutUpdated -= this.ScrollViewer_LayoutUpdated;
        this.ScrollSource.SetHorizontalOffset(SVh);
        this.ScrollSource.SetVerticalOffset(SVv);
      });
      this.ScrollSource.LayoutUpdated += this.ScrollViewer_LayoutUpdated;
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.Img = this.GetTemplateChild("PART_CustomPanel") as Panel;
    if (this.Img != null)
      this.Img.SizeChanged += new SizeChangedEventHandler(this.Img_SizeChanged);
    Thumb templateChild = this.GetTemplateChild("PART_DragResizer") as Thumb;
    templateChild.DragStarted += new DragStartedEventHandler(this.drag_DragStarted);
    templateChild.DragDelta += new DragDeltaEventHandler(this.drag_DragDelta);
    templateChild.DragCompleted += new DragCompletedEventHandler(this.drag_DragCompleted);
    this.UpdateSource();
    this.UpdateScrollContentTarget();
  }

  private void UpdateScrollContentTarget()
  {
    if (this.ScrollSource == null || VisualTreeHelper.GetChildrenCount((DependencyObject) this.ScrollSource) <= 0)
      return;
    DependencyObject reference = VisualTreeHelper.GetChild((DependencyObject) this.ScrollSource, 0);
    if (reference is ContentPresenter)
      reference = (reference as ContentPresenter).Content as DependencyObject;
    if (reference.ToString().Contains("FourQuadrantPanel"))
    {
      reference = VisualTreeHelper.GetChild(reference, 2);
      if (reference is ContentPresenter)
        reference = (reference as ContentPresenter).Content as DependencyObject;
    }
    VisualTreeHelper.GetChild(reference, 0);
    this.ScrollContentTarget = (DependencyObject) this.ScrollSource.Content;
  }

  internal void UpdateSource()
  {
    if (this.OverviewSourceAncestor == null || this.ScrollSource != null)
      return;
    this.ScrollSource = Overview.FindScrollViewer<OverviewContentHolder>(this.OverviewSourceAncestor);
    if (this.ScrollSource != null || !(this.OverviewSourceAncestor is FrameworkElement))
      return;
    (this.OverviewSourceAncestor as FrameworkElement).Loaded += new RoutedEventHandler(this.ScrollSource_Loaded);
  }

  private void ScrollSource_Loaded(object sender, RoutedEventArgs e)
  {
    (this.OverviewSourceAncestor as FrameworkElement).Loaded -= new RoutedEventHandler(this.ScrollSource_Loaded);
    this.UpdateSource();
  }

  private void drag_DragStarted(object sender, DragStartedEventArgs e)
  {
    this.m_DragDelta.X = this.VpOffsetX;
    this.m_DragDelta.Y = this.VpOffsetY;
    this.IsResizing = true;
  }

  private void drag_DragDelta(object sender, DragDeltaEventArgs e)
  {
    this.m_DragDelta.X += e.HorizontalChange;
    this.m_DragDelta.Y += e.VerticalChange;
    this.VpOffsetX = this.m_DragDelta.X;
    this.VpOffsetY = this.m_DragDelta.Y;
    this.Trans = (Transform) new TranslateTransform()
    {
      X = this.VpOffsetX,
      Y = this.VpOffsetY
    };
    this.UpdateScrollViewer();
  }

  private void drag_DragCompleted(object sender, DragCompletedEventArgs e)
  {
    this.IsResizing = false;
  }

  private static T FindScrollViewer<T>(DependencyObject obj) where T : UIElement
  {
    return Overview.FindChildScrollViewer<T>(obj);
  }

  private Size GetUniformImageSize() => new Size(this.Img.ActualWidth, this.Img.ActualHeight);
}
