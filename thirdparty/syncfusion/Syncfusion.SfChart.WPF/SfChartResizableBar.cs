// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SfChartResizableBar
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SfChartResizableBar : ResizableScrollBar
{
  private const double Interval = 0.5;
  private const double TransformSize = 20.0;
  private const double RectCoordinate = 15.0;
  internal static readonly DependencyProperty ZoomPositionProperty = DependencyProperty.Register(nameof (ZoomPosition), typeof (double), typeof (SfChartResizableBar), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(SfChartResizableBar.OnZoomPositionChanged)));
  internal static readonly DependencyProperty ThumbLabelTemplateProperty = DependencyProperty.Register(nameof (ThumbLabelTemplate), typeof (DataTemplate), typeof (SfChartResizableBar), new PropertyMetadata((PropertyChangedCallback) null));
  internal static readonly DependencyProperty ThumbLabelVisibilityProperty = DependencyProperty.Register("EnableThumbLabel", typeof (Visibility), typeof (SfChartResizableBar), new PropertyMetadata((object) Visibility.Collapsed));
  internal static readonly DependencyProperty ZoomFactorProperty = DependencyProperty.Register(nameof (ZoomFactor), typeof (double), typeof (SfChartResizableBar), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(SfChartResizableBar.OnZoomFactorChanged)));
  private double previousZoomFactor;
  private DispatcherTimer timer;
  private ChartAxisBase2D axis;
  private bool onButtonPressed;
  private bool isValueChanged;
  private DependencyObject parent;
  private ContentControl nearHandContentControl;
  private ContentControl farHandContentControl;
  private object handler;
  private ZoomChangingEventArgs zoomChangingEventArgs;
  private ZoomChangedEventArgs zoomChangedEventArgs;
  private PanChangingEventArgs panChangingEventArgs;
  private PanChangedEventArgs panChangedEventArgs;

  public SfChartResizableBar()
  {
    this.zoomChangingEventArgs = new ZoomChangingEventArgs();
    this.zoomChangedEventArgs = new ZoomChangedEventArgs();
    this.panChangingEventArgs = new PanChangingEventArgs();
    this.panChangedEventArgs = new PanChangedEventArgs();
  }

  internal ChartAxisBase2D Axis
  {
    get => this.axis;
    set
    {
      if (this.axis != null)
        this.DetachTouchModeEvents();
      this.axis = value;
      this.AttachTouchModeEvents();
      this.BindProperties();
    }
  }

  internal double ZoomPosition
  {
    get => (double) this.GetValue(SfChartResizableBar.ZoomPositionProperty);
    set => this.SetValue(SfChartResizableBar.ZoomPositionProperty, (object) value);
  }

  internal DataTemplate ThumbLabelTemplate
  {
    get => (DataTemplate) this.GetValue(SfChartResizableBar.ThumbLabelTemplateProperty);
    set => this.SetValue(SfChartResizableBar.ThumbLabelTemplateProperty, (object) value);
  }

  internal Visibility ThumbLabelVisibility
  {
    get => (Visibility) this.GetValue(SfChartResizableBar.ThumbLabelVisibilityProperty);
    set => this.SetValue(SfChartResizableBar.ThumbLabelVisibilityProperty, (object) value);
  }

  internal double ZoomFactor
  {
    get => (double) this.GetValue(SfChartResizableBar.ZoomFactorProperty);
    set => this.SetValue(SfChartResizableBar.ZoomFactorProperty, (object) value);
  }

  internal void Dispose()
  {
    this.DetachTouchModeEvents();
    this.DiposeEvents();
    this.axis = (ChartAxisBase2D) null;
  }

  private void DiposeEvents()
  {
    if (this.MiddleThumb != null)
      this.MiddleThumb.DragCompleted -= new DragCompletedEventHandler(this.DragCompleted);
    if (this.NearHand != null)
      this.NearHand.DragCompleted -= new DragCompletedEventHandler(this.DragCompleted);
    if (this.FarHand != null)
      this.FarHand.DragCompleted -= new DragCompletedEventHandler(this.DragCompleted);
    if (this.timer == null)
      return;
    this.timer.Tick -= new EventHandler(this.OnTimeout);
  }

  internal void UpdateResizable(bool isVisible)
  {
    if (this.NearHand == null || this.FarHand == null)
      return;
    if (this.EnableTouchMode)
    {
      if (isVisible)
      {
        this.NearHand.Visibility = Visibility.Visible;
        this.FarHand.Visibility = Visibility.Visible;
      }
      else
      {
        this.NearHand.Visibility = Visibility.Collapsed;
        this.FarHand.Visibility = Visibility.Collapsed;
      }
    }
    else
    {
      this.NearHand.Visibility = Visibility.Visible;
      this.FarHand.Visibility = Visibility.Visible;
      if (isVisible)
      {
        if (this.axis.Orientation == Orientation.Horizontal)
        {
          this.NearHand.Cursor = Cursors.ScrollWE;
          this.FarHand.Cursor = Cursors.ScrollWE;
        }
        else
        {
          this.NearHand.Cursor = Cursors.ScrollNS;
          this.FarHand.Cursor = Cursors.ScrollNS;
        }
      }
      else
      {
        this.NearHand.Cursor = Cursors.Hand;
        this.FarHand.Cursor = Cursors.Hand;
      }
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (this.Axis != null)
      this.UpdateResizable(this.Axis.EnableScrollBarResizing);
    if (this.EnableTouchMode)
    {
      if (this.Orientation == Orientation.Horizontal)
      {
        this.nearHandContentControl = this.GetTemplateChild("HorizontalNearHandContent") as ContentControl;
        this.farHandContentControl = this.GetTemplateChild("HorizontalFarHandContent") as ContentControl;
      }
      else
      {
        this.nearHandContentControl = this.GetTemplateChild("VerticalNearHandContent") as ContentControl;
        this.farHandContentControl = this.GetTemplateChild("VerticalFarHandContent") as ContentControl;
      }
    }
    if (this.MiddleThumb == null || this.NearHand == null || this.FarHand == null)
      return;
    this.MiddleThumb.DragCompleted += new DragCompletedEventHandler(this.DragCompleted);
    this.NearHand.DragCompleted += new DragCompletedEventHandler(this.DragCompleted);
    this.FarHand.DragCompleted += new DragCompletedEventHandler(this.DragCompleted);
  }

  protected override void OnValueChanged()
  {
    double zoomPosition = this.axis.ZoomPosition;
    double zoomFactor = this.axis.ZoomFactor;
    double rangeStart = this.RangeStart;
    double num = this.RangeEnd - this.RangeStart >= this.SmallChange ? this.RangeEnd - this.RangeStart : this.ZoomFactor;
    if (this.isNearDragged || this.isFarDragged)
      this.RaiseZoomChangingEvent(rangeStart, num);
    if (this.canDrag)
      this.RaisePanChangingEvent(zoomPosition, rangeStart);
    if (!this.Axis.DeferredScrolling)
    {
      if (!this.zoomChangingEventArgs.Cancel && (this.isNearDragged || this.isFarDragged))
      {
        this.SetZoomingChanges(rangeStart, num);
        this.RaiseZoomChangedEvent(zoomPosition, zoomFactor);
      }
      if (!this.panChangingEventArgs.Cancel && this.canDrag)
      {
        this.SetZoomingChanges(rangeStart, this.ZoomFactor);
        this.RaisePanChangedevent(rangeStart);
      }
    }
    this.ResetTimer();
    this.isValueChanged = true;
    this.previousZoomFactor = 0.0;
    base.OnValueChanged();
  }

  protected override void OnFarHandDragged(object sender, DragDeltaEventArgs e)
  {
    if (this.Axis.EnableScrollBarResizing)
    {
      this.axis.IsScrolling = true;
      base.OnFarHandDragged(sender, e);
      if (this.farHandContentControl != null)
        this.farHandContentControl.Visibility = this.isValueChanged ? Visibility.Visible : Visibility.Collapsed;
      this.onButtonPressed = true;
      if (this.EnableTouchMode)
        this.Translate(this.farHandContentControl, this.RangeEnd);
      this.isValueChanged = false;
    }
    else
      this.OnThumbDragged(sender, e);
  }

  protected override void OnNearHandDragged(object sender, DragDeltaEventArgs e)
  {
    if (this.Axis.EnableScrollBarResizing)
    {
      this.axis.IsScrolling = true;
      base.OnNearHandDragged(sender, e);
      if (this.nearHandContentControl != null)
        this.nearHandContentControl.Visibility = this.isValueChanged ? Visibility.Visible : Visibility.Collapsed;
      this.onButtonPressed = true;
      if (this.EnableTouchMode)
        this.Translate(this.nearHandContentControl, this.RangeStart);
      this.isValueChanged = false;
    }
    else
      this.OnThumbDragged(sender, e);
  }

  protected override void OnThumbDragged(object sender, DragDeltaEventArgs e)
  {
    this.axis.IsScrolling = true;
    base.OnThumbDragged(sender, e);
    this.onButtonPressed = true;
  }

  private static void OnZoomFactorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as SfChartResizableBar).ChangeZoomFactor((double) e.NewValue);
  }

  private static void OnZoomPositionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as SfChartResizableBar).ChangeZoomPosition((double) e.NewValue);
  }

  private void DragCompleted(object sender, DragCompletedEventArgs e)
  {
    if (this.axis == null)
      return;
    this.axis.IsScrolling = false;
  }

  private void AttachTouchModeEvents()
  {
    this.Loaded += new RoutedEventHandler(this.OnAxisLoaded);
    this.Unloaded += new RoutedEventHandler(this.OnAxisUnloaded);
  }

  private void DetachTouchModeEvents()
  {
    if (this.Axis == null)
      return;
    this.Axis.MouseEnter -= new MouseEventHandler(this.OnAxisMouseEnter);
    this.Axis.MouseLeave -= new MouseEventHandler(this.OnAxisMouseLeave);
    this.Loaded -= new RoutedEventHandler(this.OnAxisLoaded);
    this.Unloaded -= new RoutedEventHandler(this.OnAxisUnloaded);
  }

  private void OnAxisUnloaded(object sender, RoutedEventArgs e)
  {
    if (this.Axis != null)
    {
      this.Axis.MouseEnter -= new MouseEventHandler(this.OnAxisMouseEnter);
      this.Axis.MouseLeave -= new MouseEventHandler(this.OnAxisMouseLeave);
    }
    if (!(this.parent is Window))
      return;
    (this.parent as Window).RemoveHandler(UIElement.MouseDownEvent, (Delegate) this.handler);
  }

  private void OnAxisLoaded(object sender, RoutedEventArgs e)
  {
    if (this.Axis != null)
    {
      this.Axis.MouseEnter += new MouseEventHandler(this.OnAxisMouseEnter);
      this.Axis.MouseLeave += new MouseEventHandler(this.OnAxisMouseLeave);
    }
    if (this.Axis.Area == null)
      return;
    this.parent = VisualTreeHelper.GetParent((DependencyObject) this.Axis.Area);
    if (this.parent == null)
      return;
    while (VisualTreeHelper.GetParent(this.parent) != null)
      this.parent = VisualTreeHelper.GetParent(this.parent);
    this.handler = (object) new MouseButtonEventHandler(this.OnMouseDown);
    if (!(this.parent is Window))
      return;
    (this.parent as Window).AddHandler(UIElement.MouseDownEvent, (Delegate) this.handler, true);
  }

  private void OnMouseDown(object sender, MouseButtonEventArgs e)
  {
    if (this.Axis == null || this.Axis.Area == null)
      return;
    if (this.CheckRegion(e.GetPosition((IInputElement) this.Axis.Area)))
    {
      VisualStateManager.GoToState((FrameworkElement) this, "OnView", true);
      this.onButtonPressed = true;
    }
    else
    {
      VisualStateManager.GoToState((FrameworkElement) this, "OnLostFocus", true);
      this.onButtonPressed = false;
    }
  }

  private void OnAxisMouseLeave(object sender, MouseEventArgs e)
  {
    if (this.onButtonPressed)
      return;
    VisualStateManager.GoToState((FrameworkElement) this, "OnExit", true);
  }

  private void OnAxisMouseEnter(object sender, MouseEventArgs e)
  {
    if (this.Axis.isManipulated || this.onButtonPressed)
      return;
    VisualStateManager.GoToState((FrameworkElement) this, "OnFocus", true);
  }

  private bool CheckRegion(Point position)
  {
    if (this.Axis.EnableScrollBar && this.Axis.Visibility != Visibility.Collapsed)
    {
      Rect rect = new Rect();
      Rect arrangeRect = this.Axis.ArrangeRect;
      if (this.Orientation == Orientation.Horizontal)
      {
        arrangeRect.X -= 15.0;
        arrangeRect.Y = this.Axis.OpposedPosition ? arrangeRect.Y + 15.0 : arrangeRect.Y - 15.0;
      }
      else
      {
        arrangeRect.X = this.Axis.OpposedPosition ? arrangeRect.X - 15.0 : arrangeRect.X + 15.0;
        arrangeRect.Y -= 15.0;
      }
      if (this.axis.EnableTouchMode && this.FarHand != null && this.NearHand != null && this.MiddleThumb != null && (arrangeRect.Contains(position) || this.FarHand.IsDragging || this.NearHand.IsDragging || this.MiddleThumb.IsDragging))
        return true;
    }
    return false;
  }

  private void SetZoomingChanges(double newZoomPosition, double newZoomFactor)
  {
    this.ZoomPosition = newZoomPosition;
    this.ZoomFactor = newZoomFactor;
  }

  private void RaiseZoomChangingEvent(double newPosition, double newFactor)
  {
    if (this.axis.Area == null)
      return;
    this.zoomChangingEventArgs.Axis = this.axis;
    this.zoomChangingEventArgs.CurrentFactor = newFactor;
    this.zoomChangingEventArgs.CurrentPosition = newPosition;
    this.zoomChangingEventArgs.PreviousFactor = this.ZoomFactor;
    this.zoomChangingEventArgs.PreviousPosition = this.ZoomPosition;
    if (this.axis is DateTimeAxis || this.axis is DateTimeCategoryAxis)
      this.zoomChangingEventArgs.OldRange = (object) new DateTimeRange(this.axis.VisibleRange.Start.FromOADate(), this.axis.VisibleRange.End.FromOADate());
    else
      this.zoomChangingEventArgs.OldRange = (object) new DoubleRange(this.axis.VisibleRange.Start, this.axis.VisibleRange.End);
    this.zoomChangingEventArgs.Cancel = false;
    (this.axis.Area as SfChart).OnZoomChanging(this.zoomChangingEventArgs);
  }

  private void RaiseZoomChangedEvent(double prevPosition, double prevFactor)
  {
    if (this.axis.Area == null)
      return;
    DoubleRange range = this.axis.CalculateRange(this.axis.ActualRange, this.ZoomPosition, this.ZoomFactor);
    this.zoomChangedEventArgs.Axis = this.axis;
    this.zoomChangedEventArgs.CurrentFactor = this.ZoomFactor;
    this.zoomChangedEventArgs.CurrentPosition = this.ZoomPosition;
    this.zoomChangedEventArgs.PreviousFactor = prevFactor;
    this.zoomChangedEventArgs.PreviousPosition = prevPosition;
    if (this.axis is DateTimeAxis || this.axis is DateTimeCategoryAxis)
    {
      this.zoomChangedEventArgs.NewRange = (object) new DateTimeRange(range.Start.FromOADate(), range.End.FromOADate());
      this.zoomChangedEventArgs.OldRange = (object) new DateTimeRange(this.axis.VisibleRange.Start.FromOADate(), this.axis.VisibleRange.End.FromOADate());
    }
    else
    {
      this.zoomChangedEventArgs.NewRange = (object) range;
      this.zoomChangedEventArgs.OldRange = (object) new DoubleRange(this.axis.VisibleRange.Start, this.axis.VisibleRange.End);
    }
    (this.axis.Area as SfChart).OnZoomChanged(this.zoomChangedEventArgs);
  }

  private void RaisePanChangedevent(double zoomPosition)
  {
    if (this.axis.Area == null)
      return;
    this.panChangedEventArgs.Axis = this.axis;
    this.panChangedEventArgs.NewZoomPosition = zoomPosition;
    (this.axis.Area as SfChart).OnPanChanged(this.panChangedEventArgs);
  }

  private void ChangeZoomPosition(double value)
  {
    this.IsValueChangedTrigger = false;
    this.RangeStart = value;
    if (this.previousZoomFactor == this.ZoomFactor)
      this.RangeEnd = this.RangeStart + this.ZoomFactor;
    this.previousZoomFactor = this.ZoomFactor;
  }

  private void RaisePanChangingEvent(double prevPosition, double newPosition)
  {
    if (this.axis.Area == null)
      return;
    this.panChangingEventArgs.Axis = this.axis;
    this.panChangingEventArgs.OldZoomPosition = prevPosition;
    this.panChangingEventArgs.NewZoomPosition = newPosition;
    this.panChangingEventArgs.Cancel = false;
    (this.axis.Area as SfChart).OnPanChanging(this.panChangingEventArgs);
  }

  private void ChangeZoomFactor(double value)
  {
    this.IsValueChangedTrigger = false;
    this.RangeEnd = this.RangeStart + value;
    this.RangeStart = this.ZoomPosition;
    this.previousZoomFactor = this.ZoomFactor;
  }

  private void BindProperties()
  {
    this.SetBinding(SfChartResizableBar.ZoomPositionProperty, (BindingBase) new Binding()
    {
      Source = (object) this.Axis,
      Path = new PropertyPath("ZoomPosition", new object[0]),
      Mode = BindingMode.TwoWay
    });
    this.SetBinding(SfChartResizableBar.ZoomFactorProperty, (BindingBase) new Binding()
    {
      Source = (object) this.Axis,
      Path = new PropertyPath("ZoomFactor", new object[0]),
      Mode = BindingMode.TwoWay
    });
    this.SetBinding(ResizableScrollBar.EnableTouchModeProperty, (BindingBase) new Binding()
    {
      Source = (object) this.Axis,
      Path = new PropertyPath("EnableTouchMode", new object[0]),
      Mode = BindingMode.TwoWay
    });
    this.SetBinding(SfChartResizableBar.ThumbLabelTemplateProperty, (BindingBase) new Binding()
    {
      Source = (object) this.Axis,
      Path = new PropertyPath("ThumbLabelTemplate", new object[0])
    });
    this.SetBinding(SfChartResizableBar.ThumbLabelVisibilityProperty, (BindingBase) new Binding()
    {
      Source = (object) this.Axis,
      Path = new PropertyPath("ThumbLabelVisibility", new object[0])
    });
  }

  private void Translate(ContentControl contentControl, double rangeValue)
  {
    if (this.ThumbLabelVisibility == Visibility.Visible)
    {
      contentControl.ContentTemplate = this.ThumbLabelTemplate;
      contentControl.Content = this.Axis is NumericalAxis ? (object) Convert.ToDecimal(this.Axis.GetLabelContent(this.Axis.CoefficientToActualValue(rangeValue))).ToString("0.##") : (object) Convert.ToString(this.Axis.GetLabelContent(this.Axis.CoefficientToActualValue(rangeValue)));
      TranslateTransform translateTransform = new TranslateTransform();
      if (this.Orientation == Orientation.Horizontal)
      {
        translateTransform.X = -contentControl.ActualWidth / 2.0;
        translateTransform.Y = this.Axis.OpposedPosition ? 20.0 : -20.0;
      }
      else
      {
        translateTransform.X = this.Axis.OpposedPosition ? -40.0 : 20.0;
        translateTransform.Y = -contentControl.ActualHeight / 2.0;
      }
      contentControl.RenderTransform = (Transform) translateTransform;
    }
    else
      contentControl.Visibility = Visibility.Collapsed;
  }

  private void ResetTimer()
  {
    if (this.timer != null)
    {
      this.timer.Stop();
      this.timer.Interval = TimeSpan.FromSeconds(0.5);
      this.timer.Start();
    }
    else
    {
      this.timer = new DispatcherTimer();
      this.timer.Tick += new EventHandler(this.OnTimeout);
    }
  }

  private void OnTimeout(object sender, object e)
  {
    if (this.Axis.DeferredScrolling)
    {
      this.ZoomPosition = this.RangeStart;
      this.ZoomFactor = this.RangeEnd - this.RangeStart >= this.SmallChange ? this.RangeEnd - this.RangeStart : (this.ZoomFactor == this.Maximum ? this.SmallChange : this.ZoomFactor);
    }
    if (this.nearHandContentControl != null && this.farHandContentControl != null)
    {
      this.nearHandContentControl.Visibility = Visibility.Collapsed;
      this.farHandContentControl.Visibility = Visibility.Collapsed;
    }
    if (this.timer != null)
      this.timer.Stop();
    this.timer.Tick -= new EventHandler(this.OnTimeout);
    this.timer = (DispatcherTimer) null;
  }
}
