// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartZoomPanBehavior
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartZoomPanBehavior : ChartBehavior
{
  public static readonly DependencyProperty ToolBarItemsProperty = DependencyProperty.Register(nameof (ToolBarItems), typeof (ZoomToolBarItems), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) ZoomToolBarItems.All, new PropertyChangedCallback(ChartZoomPanBehavior.OnToolBarItemsChanged)));
  public static readonly DependencyProperty EnableZoomingToolBarProperty = DependencyProperty.Register(nameof (EnableZoomingToolBar), typeof (bool), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartZoomPanBehavior.OnToolbarPropertyChanged)));
  public static readonly DependencyProperty ToolBarItemHeightProperty = DependencyProperty.Register(nameof (ToolBarItemHeight), typeof (double), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) 24.0));
  public static readonly DependencyProperty ToolBarItemWidthProperty = DependencyProperty.Register(nameof (ToolBarItemWidth), typeof (double), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) 24.0));
  public static readonly DependencyProperty ToolBarItemMarginProperty = DependencyProperty.Register(nameof (ToolBarItemMargin), typeof (Thickness), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) new Thickness().GetThickness(4.0, 4.0, 4.0, 4.0)));
  public static readonly DependencyProperty HorizontalPositionProperty = DependencyProperty.Register(nameof (HorizontalPosition), typeof (HorizontalAlignment), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) HorizontalAlignment.Right, new PropertyChangedCallback(ChartZoomPanBehavior.OnAlignmentPropertyChanged)));
  public static readonly DependencyProperty VerticalPositionProperty = DependencyProperty.Register(nameof (VerticalPosition), typeof (VerticalAlignment), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) VerticalAlignment.Top, new PropertyChangedCallback(ChartZoomPanBehavior.OnAlignmentPropertyChanged)));
  public static readonly DependencyProperty ToolBarOrientationProperty = DependencyProperty.Register(nameof (ToolBarOrientation), typeof (Orientation), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) Orientation.Horizontal, new PropertyChangedCallback(ChartZoomPanBehavior.OnOrientationChanged)));
  public static readonly DependencyProperty ToolBarBackgroundProperty = DependencyProperty.Register(nameof (ToolBarBackground), typeof (SolidColorBrush), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartZoomPanBehavior.OnToolBarBackgroundChanged)));
  public static readonly DependencyProperty ZoomRelativeToCursorProperty = DependencyProperty.Register(nameof (ZoomRelativeToCursor), typeof (bool), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) true));
  public static readonly DependencyProperty EnablePinchZoomingProperty = DependencyProperty.Register(nameof (EnablePinchZooming), typeof (bool), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ZoomModeProperty = DependencyProperty.Register(nameof (ZoomMode), typeof (ZoomMode), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) ZoomMode.XY));
  public static readonly DependencyProperty EnableDirectionalZoomingProperty = DependencyProperty.Register(nameof (EnableDirectionalZooming), typeof (bool), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) false));
  public static readonly DependencyProperty EnablePanningProperty = DependencyProperty.Register(nameof (EnablePanning), typeof (bool), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) true));
  public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof (StrokeThickness), typeof (double), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) 1.0));
  public static readonly DependencyProperty MaximumZoomLevelProperty = DependencyProperty.Register(nameof (MaximumZoomLevel), typeof (double), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) double.NaN));
  public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof (Stroke), typeof (Brush), typeof (ChartZoomPanBehavior), (PropertyMetadata) null);
  public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof (Fill), typeof (Brush), typeof (ChartZoomPanBehavior), (PropertyMetadata) null);
  public static readonly DependencyProperty EnableSelectionZoomingProperty = DependencyProperty.Register(nameof (EnableSelectionZooming), typeof (bool), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartZoomPanBehavior.OnZoomSelectionChanged)));
  public static readonly DependencyProperty ResetOnDoubleTapProperty = DependencyProperty.Register(nameof (ResetOnDoubleTap), typeof (bool), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) true));
  public static readonly DependencyProperty EnableMouseWheelZoomingProperty = DependencyProperty.Register(nameof (EnableMouseWheelZooming), typeof (bool), typeof (ChartZoomPanBehavior), new PropertyMetadata((object) true));
  internal bool IsActive;
  private bool enablePanning = true;
  private bool enableSelectionZooming = true;
  private Rectangle selectionRectangle;
  private ZoomChangingEventArgs zoomChangingEventArgs;
  private ZoomChangedEventArgs zoomChangedEventArgs;
  private PanChangingEventArgs panChangingEventArgs;
  private PanChangedEventArgs panChangedEventArgs;
  private SelectionZoomingStartEventArgs sel_ZoomingStartEventArgs;
  private SelectionZoomingDeltaEventArgs sel_ZoomingDeltaEventArgs;
  private SelectionZoomingEndEventArgs sel_ZoomingEndEventArgs;
  private ResetZoomEventArgs zoomingResetEventArgs;
  private bool isPanningChanged;
  private bool isReset;
  private Point startPoint;
  private bool isZooming;
  private bool isTransposed;
  private Rect zoomRect;
  private double previousScale;
  private Rect areaRect;
  private double angle;
  private Point previousPoint;
  private List<TouchDevice> touchDevices = new List<TouchDevice>();
  private bool isLeftButtonPressed;
  private bool mouseCapture;

  public ChartZoomPanBehavior()
  {
    this.Fill = (Brush) new SolidColorBrush(Color.FromArgb((byte) 100, (byte) 210, (byte) 223, (byte) 242));
    this.Stroke = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 43, (byte) 87, (byte) 154));
    this.zoomChangingEventArgs = new ZoomChangingEventArgs();
    this.zoomChangedEventArgs = new ZoomChangedEventArgs();
    this.panChangedEventArgs = new PanChangedEventArgs();
    this.panChangingEventArgs = new PanChangingEventArgs();
    this.sel_ZoomingStartEventArgs = new SelectionZoomingStartEventArgs();
    this.sel_ZoomingDeltaEventArgs = new SelectionZoomingDeltaEventArgs();
    this.sel_ZoomingEndEventArgs = new SelectionZoomingEndEventArgs();
    this.zoomingResetEventArgs = new ResetZoomEventArgs();
    Rectangle rectangle = new Rectangle();
    rectangle.IsHitTestVisible = false;
    this.selectionRectangle = rectangle;
  }

  public ZoomToolBarItems ToolBarItems
  {
    get => (ZoomToolBarItems) this.GetValue(ChartZoomPanBehavior.ToolBarItemsProperty);
    set => this.SetValue(ChartZoomPanBehavior.ToolBarItemsProperty, (object) value);
  }

  public bool EnableZoomingToolBar
  {
    get => (bool) this.GetValue(ChartZoomPanBehavior.EnableZoomingToolBarProperty);
    set => this.SetValue(ChartZoomPanBehavior.EnableZoomingToolBarProperty, (object) value);
  }

  public double ToolBarItemHeight
  {
    get => (double) this.GetValue(ChartZoomPanBehavior.ToolBarItemHeightProperty);
    set => this.SetValue(ChartZoomPanBehavior.ToolBarItemHeightProperty, (object) value);
  }

  public double ToolBarItemWidth
  {
    get => (double) this.GetValue(ChartZoomPanBehavior.ToolBarItemWidthProperty);
    set => this.SetValue(ChartZoomPanBehavior.ToolBarItemWidthProperty, (object) value);
  }

  public Thickness ToolBarItemMargin
  {
    get => (Thickness) this.GetValue(ChartZoomPanBehavior.ToolBarItemMarginProperty);
    set => this.SetValue(ChartZoomPanBehavior.ToolBarItemMarginProperty, (object) value);
  }

  public HorizontalAlignment HorizontalPosition
  {
    get => (HorizontalAlignment) this.GetValue(ChartZoomPanBehavior.HorizontalPositionProperty);
    set => this.SetValue(ChartZoomPanBehavior.HorizontalPositionProperty, (object) value);
  }

  public VerticalAlignment VerticalPosition
  {
    get => (VerticalAlignment) this.GetValue(ChartZoomPanBehavior.VerticalPositionProperty);
    set => this.SetValue(ChartZoomPanBehavior.VerticalPositionProperty, (object) value);
  }

  public Orientation ToolBarOrientation
  {
    get => (Orientation) this.GetValue(ChartZoomPanBehavior.ToolBarOrientationProperty);
    set => this.SetValue(ChartZoomPanBehavior.ToolBarOrientationProperty, (object) value);
  }

  public SolidColorBrush ToolBarBackground
  {
    get => (SolidColorBrush) this.GetValue(ChartZoomPanBehavior.ToolBarBackgroundProperty);
    set => this.SetValue(ChartZoomPanBehavior.ToolBarBackgroundProperty, (object) value);
  }

  public bool ZoomRelativeToCursor
  {
    get => (bool) this.GetValue(ChartZoomPanBehavior.ZoomRelativeToCursorProperty);
    set => this.SetValue(ChartZoomPanBehavior.ZoomRelativeToCursorProperty, (object) value);
  }

  public bool EnablePinchZooming
  {
    get => (bool) this.GetValue(ChartZoomPanBehavior.EnablePinchZoomingProperty);
    set => this.SetValue(ChartZoomPanBehavior.EnablePinchZoomingProperty, (object) value);
  }

  public ZoomMode ZoomMode
  {
    get => (ZoomMode) this.GetValue(ChartZoomPanBehavior.ZoomModeProperty);
    set => this.SetValue(ChartZoomPanBehavior.ZoomModeProperty, (object) value);
  }

  public bool EnableDirectionalZooming
  {
    get => (bool) this.GetValue(ChartZoomPanBehavior.EnableDirectionalZoomingProperty);
    set => this.SetValue(ChartZoomPanBehavior.EnableDirectionalZoomingProperty, (object) value);
  }

  public bool EnablePanning
  {
    get => (bool) this.GetValue(ChartZoomPanBehavior.EnablePanningProperty);
    set => this.SetValue(ChartZoomPanBehavior.EnablePanningProperty, (object) value);
  }

  public double StrokeThickness
  {
    get => (double) this.GetValue(ChartZoomPanBehavior.StrokeThicknessProperty);
    set => this.SetValue(ChartZoomPanBehavior.StrokeThicknessProperty, (object) value);
  }

  public double MaximumZoomLevel
  {
    get => (double) this.GetValue(ChartZoomPanBehavior.MaximumZoomLevelProperty);
    set => this.SetValue(ChartZoomPanBehavior.MaximumZoomLevelProperty, (object) value);
  }

  public Brush Stroke
  {
    get => (Brush) this.GetValue(ChartZoomPanBehavior.StrokeProperty);
    set => this.SetValue(ChartZoomPanBehavior.StrokeProperty, (object) value);
  }

  public Brush Fill
  {
    get => (Brush) this.GetValue(ChartZoomPanBehavior.FillProperty);
    set => this.SetValue(ChartZoomPanBehavior.FillProperty, (object) value);
  }

  public bool EnableSelectionZooming
  {
    get => (bool) this.GetValue(ChartZoomPanBehavior.EnableSelectionZoomingProperty);
    set => this.SetValue(ChartZoomPanBehavior.EnableSelectionZoomingProperty, (object) value);
  }

  public bool ResetOnDoubleTap
  {
    get => (bool) this.GetValue(ChartZoomPanBehavior.ResetOnDoubleTapProperty);
    set => this.SetValue(ChartZoomPanBehavior.ResetOnDoubleTapProperty, (object) value);
  }

  public bool EnableMouseWheelZooming
  {
    get => (bool) this.GetValue(ChartZoomPanBehavior.EnableMouseWheelZoomingProperty);
    set => this.SetValue(ChartZoomPanBehavior.EnableMouseWheelZoomingProperty, (object) value);
  }

  internal bool InternalEnablePanning
  {
    get => this.enablePanning && this.EnablePanning;
    set => this.enablePanning = value;
  }

  internal ZoomingToolBar ChartZoomingToolBar { get; set; }

  internal bool InternalEnableSelectionZooming
  {
    get => this.enableSelectionZooming && this.EnableSelectionZooming;
    set => this.enableSelectionZooming = value;
  }

  private bool IsMaxZoomLevel => !double.IsNaN(this.MaximumZoomLevel);

  private bool HorizontalMode
  {
    get
    {
      if (this.ZoomMode == ZoomMode.X && !this.isTransposed)
        return true;
      return this.ZoomMode == ZoomMode.Y && this.isTransposed;
    }
  }

  private bool VerticalMode
  {
    get
    {
      if (this.ZoomMode == ZoomMode.Y && !this.isTransposed)
        return true;
      return this.ZoomMode == ZoomMode.X && this.isTransposed;
    }
  }

  public virtual bool Zoom(double cumulativeScale, double origin, ChartAxisBase2D axis)
  {
    if (cumulativeScale >= 1.0 && axis != null)
    {
      double calcZoomPos = 0.0;
      double calcZoomFactor = 0.0;
      double zoomPosition = axis.ZoomPosition;
      double zoomFactor1 = axis.ZoomFactor;
      ChartZoomPanBehavior.CalZoomFactors(cumulativeScale, origin, zoomFactor1, zoomPosition, ref calcZoomPos, ref calcZoomFactor);
      double zoomFactor2 = calcZoomPos + calcZoomFactor > 1.0 ? 1.0 - calcZoomPos : calcZoomFactor;
      this.RaiseZoomChangingEvent(calcZoomPos, zoomFactor2, axis);
      if (!this.zoomChangingEventArgs.Cancel && (axis.ZoomPosition != calcZoomPos || axis.ZoomFactor != calcZoomFactor))
      {
        axis.ZoomPosition = calcZoomPos;
        axis.ZoomFactor = zoomFactor2;
        this.RaiseZoomChangedEvent(zoomFactor1, zoomPosition, axis);
        return true;
      }
    }
    return false;
  }

  public bool Zoom(double cumulativeScale, ChartAxisBase2D axis)
  {
    return this.Zoom(cumulativeScale, 0.5, axis);
  }

  public void Reset()
  {
    foreach (ChartAxisBase2D ax in (Collection<ChartAxis>) this.ChartArea.Axes)
    {
      this.RaiseResetZoomingEvent(ax);
      if (this.zoomingResetEventArgs.Cancel)
        break;
      ax.ZoomPosition = 0.0;
      ax.ZoomFactor = 1.0;
      ax.IsScrolling = false;
      this.RaiseZoomChangedEvent(ax.ZoomFactor, ax.ZoomPosition, ax);
    }
  }

  internal void DisposeZoomEventArguments()
  {
    if (this.zoomChangedEventArgs != null)
    {
      this.zoomChangedEventArgs.Axis = (ChartAxisBase2D) null;
      this.zoomChangedEventArgs = (ZoomChangedEventArgs) null;
    }
    if (this.zoomChangingEventArgs != null)
    {
      this.zoomChangingEventArgs.Axis = (ChartAxisBase2D) null;
      this.zoomChangingEventArgs = (ZoomChangingEventArgs) null;
    }
    if (this.panChangedEventArgs != null)
    {
      this.panChangedEventArgs.Axis = (ChartAxisBase2D) null;
      this.panChangedEventArgs = (PanChangedEventArgs) null;
    }
    if (this.panChangingEventArgs == null)
      return;
    this.panChangingEventArgs.Axis = (ChartAxisBase2D) null;
    this.panChangingEventArgs = (PanChangingEventArgs) null;
  }

  internal void ZoomByRange(ChartAxisBase2D chartAxis, DateTime start, DateTime end)
  {
    this.ZoomByRange(chartAxis, start.ToOADate(), end.ToOADate());
  }

  internal void ZoomByRange(ChartAxisBase2D chartAxis, double start, double end)
  {
    if (!this.CanZoom(chartAxis))
      return;
    if (start > end)
    {
      double num = start;
      start = end;
      end = num;
    }
    if (start >= chartAxis.ActualRange.End || end <= chartAxis.ActualRange.Start)
      return;
    if (start < chartAxis.ActualRange.Start)
      start = chartAxis.ActualRange.Start;
    if (end > chartAxis.ActualRange.End)
      end = chartAxis.ActualRange.End;
    chartAxis.ZoomPosition = (start - chartAxis.ActualRange.Start) / chartAxis.ActualRange.Delta;
    chartAxis.ZoomFactor = (end - start) / chartAxis.ActualRange.Delta;
  }

  internal void ZoomToFactor(ChartAxisBase2D chartAxis, double zoomPosition, double zoomFactor)
  {
    if (!this.CanZoom(chartAxis))
      return;
    chartAxis.ZoomFactor = zoomFactor;
    chartAxis.ZoomPosition = zoomPosition;
  }

  internal void ZoomIn()
  {
    foreach (ChartAxisBase2D ax in (Collection<ChartAxis>) this.ChartArea.Axes)
    {
      if (ax != null && this.CanZoom(ax))
        this.Zoom(this.ValMaxScaleLevel(Math.Max(Math.Max(1.0 / ChartMath.MinMax(ax.ZoomFactor, 0.0, 1.0), 1.0) + 0.25, 1.0)), 0.5, ax);
    }
  }

  internal void ZoomOut()
  {
    foreach (ChartAxisBase2D ax in (Collection<ChartAxis>) this.ChartArea.Axes)
    {
      if (ax != null && this.CanZoom(ax))
        this.Zoom(this.ValMaxScaleLevel(Math.Max(Math.Max(1.0 / ChartMath.MinMax(ax.ZoomFactor, 0.0, 1.0), 1.0) - 0.25, 1.0)), 0.5, ax);
    }
  }

  internal void Zoom(double zoomFactor)
  {
    foreach (ChartAxisBase2D ax in (Collection<ChartAxis>) this.ChartArea.Axes)
    {
      if (this.CanZoom(ax) && ax.ZoomFactor <= 1.0 && ax.ZoomFactor >= 0.1)
      {
        ax.ZoomFactor = zoomFactor;
        ax.ZoomPosition = 0.5;
      }
    }
  }

  internal void Zoom(Rect zoomRect)
  {
    foreach (ChartAxisBase2D ax in (Collection<ChartAxis>) this.ChartArea.Axes)
      this.Zoom(zoomRect, ax);
  }

  protected internal override void DetachElements()
  {
    if (this.ChartZoomingToolBar == null || this.ChartArea == null || this.ChartArea.ToolkitCanvas == null || this.ChartArea.AreaType != ChartAreaType.CartesianAxes || !this.ChartArea.ToolkitCanvas.Children.Contains((UIElement) this.ChartZoomingToolBar))
      return;
    this.ChartArea.ToolkitCanvas.Children.Remove((UIElement) this.ChartZoomingToolBar);
  }

  protected internal override void OnLayoutUpdated()
  {
    if (this.ChartZoomingToolBar == null || this.ChartArea == null || this.ChartZoomingToolBar.Visibility == Visibility.Collapsed)
      return;
    if (this.HorizontalPosition == HorizontalAlignment.Left)
      Canvas.SetLeft((UIElement) this.ChartZoomingToolBar, this.ChartArea.SeriesClipRect.Left + 5.0);
    else if (this.HorizontalPosition == HorizontalAlignment.Center)
      Canvas.SetLeft((UIElement) this.ChartZoomingToolBar, this.ChartArea.SeriesClipRect.Width / 2.0 - this.ChartZoomingToolBar.DesiredSize.Width / 2.0);
    else
      Canvas.SetLeft((UIElement) this.ChartZoomingToolBar, this.ChartArea.SeriesClipRect.Right - this.ChartZoomingToolBar.DesiredSize.Width - 5.0);
    if (this.VerticalPosition == VerticalAlignment.Bottom)
      Canvas.SetTop((UIElement) this.ChartZoomingToolBar, this.ChartArea.SeriesClipRect.Bottom - this.ChartZoomingToolBar.DesiredSize.Height - 5.0);
    else if (this.VerticalPosition == VerticalAlignment.Center)
      Canvas.SetTop((UIElement) this.ChartZoomingToolBar, this.ChartArea.SeriesClipRect.Height / 2.0 - this.ChartZoomingToolBar.DesiredSize.Height / 2.0);
    else
      Canvas.SetTop((UIElement) this.ChartZoomingToolBar, this.ChartArea.SeriesClipRect.Top + 5.0);
  }

  protected internal override void OnMouseWheel(MouseWheelEventArgs e)
  {
    if (this.ChartArea == null || this.ChartArea.AreaType != ChartAreaType.CartesianAxes || !this.EnableMouseWheelZooming)
      return;
    this.RemoveTooltip();
    this.MouseWheelZoom(e.GetPosition((IInputElement) this.AdorningCanvas), e.Delta > 0 ? 1.0 : -1.0);
  }

  protected internal override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.ChartArea == null)
      return;
    this.isPanningChanged = false;
    if (this.ChartArea.AreaType != ChartAreaType.CartesianAxes)
      return;
    if ((e.OriginalSource is FrameworkElement originalSource ? originalSource.DataContext as LegendItem : (LegendItem) null) == null)
    {
      foreach (ChartAxis ax in (Collection<ChartAxis>) this.ChartArea.Axes)
        ax.IsScrolling = true;
    }
    this.isLeftButtonPressed = true;
    this.previousPoint = e.GetPosition((IInputElement) this.AdorningCanvas);
    if (e.ClickCount == 2 && this.ChartArea.SeriesClipRect.Contains(this.previousPoint) && this.ResetOnDoubleTap)
    {
      this.isReset = true;
    }
    else
    {
      if (!this.InternalEnableSelectionZooming)
        return;
      Point position = e.GetPosition((IInputElement) this.AdorningCanvas);
      if (!this.ChartArea.SeriesClipRect.Contains(position))
        return;
      this.zoomRect = Rect.Empty;
      Canvas.SetLeft((UIElement) this.selectionRectangle, position.X);
      Canvas.SetTop((UIElement) this.selectionRectangle, position.Y);
      this.startPoint = position;
      this.sel_ZoomingStartEventArgs.ZoomRect = this.zoomRect;
      this.ChartArea.OnSelectionZoomingStart(this.sel_ZoomingStartEventArgs);
      this.isZooming = true;
    }
  }

  protected internal override void OnMouseMove(MouseEventArgs e)
  {
    if (this.ChartArea == null)
      return;
    this.isTransposed = this.ChartArea.Series.Where<ChartSeries>((Func<ChartSeries, bool>) (series => series.IsActualTransposed)).Any<ChartSeries>();
    if (this.isLeftButtonPressed && !this.isZooming && this.InternalEnablePanning && !this.isReset)
    {
      if (!this.mouseCapture)
        this.mouseCapture = this.AdorningCanvas.CaptureMouse();
      Point position = e.GetPosition((IInputElement) this.AdorningCanvas);
      TranslateTransform translateTransform = ChartMath.Translate(this.previousPoint, position);
      this.RemoveTooltip();
      foreach (ChartAxisBase2D ax in (Collection<ChartAxis>) this.ChartArea.Axes)
      {
        double currentScale = Math.Max(1.0 / ChartMath.MinMax(ax.ZoomFactor, 0.0, 1.0), 1.0);
        if (ax.EnableTouchMode)
        {
          if (ax.Area.SeriesClipRect.Contains(position))
            this.Translate(ax, translateTransform.X, translateTransform.Y, currentScale);
        }
        else
          this.Translate(ax, translateTransform.X, translateTransform.Y, currentScale);
      }
      this.previousPoint = position;
    }
    if (!this.isZooming)
      return;
    if (!this.mouseCapture)
      this.mouseCapture = this.AdorningCanvas.CaptureMouse();
    Point position1 = e.GetPosition((IInputElement) this.AdorningCanvas);
    Point point2;
    if (this.ChartArea.IsMultipleArea)
    {
      bool isPointInsideRect = false;
      foreach (ChartAxis ax in (Collection<ChartAxis>) this.ChartArea.Axes)
      {
        this.areaRect = ChartExtensionUtils.GetAxisArrangeRect(this.startPoint, ax, out isPointInsideRect);
        if (isPointInsideRect)
          break;
      }
      if (!isPointInsideRect)
        this.areaRect = this.ChartArea.SeriesClipRect;
      point2 = ChartBehavior.ValidatePoint(position1, this.areaRect);
      point2.X = ChartMath.MinMax(point2.X, this.areaRect.X, this.areaRect.Right);
      point2.Y = ChartMath.MinMax(point2.Y, this.areaRect.Y, this.areaRect.Bottom);
    }
    else
    {
      this.areaRect = this.ChartArea.SeriesClipRect;
      point2 = ChartBehavior.ValidatePoint(position1, this.areaRect);
      point2.X = ChartMath.MinMax(point2.X, 0.0, this.AdorningCanvas.Width);
      point2.Y = ChartMath.MinMax(point2.Y, 0.0, this.AdorningCanvas.Height);
    }
    this.zoomRect = !this.HorizontalMode ? (!this.VerticalMode ? new Rect(this.startPoint, point2) : new Rect(new Point(this.ChartArea.SeriesClipRect.Left, this.startPoint.Y), new Point(this.ChartArea.SeriesClipRect.Right, point2.Y))) : new Rect(new Point(this.startPoint.X, this.ChartArea.SeriesClipRect.Top), new Point(point2.X, this.ChartArea.SeriesClipRect.Bottom));
    this.RaiseSelectionZoomDeltaEvent();
    if (this.sel_ZoomingDeltaEventArgs.Cancel)
      return;
    this.selectionRectangle.Height = this.zoomRect.Height;
    this.selectionRectangle.Width = this.zoomRect.Width;
    Canvas.SetLeft((UIElement) this.selectionRectangle, this.zoomRect.X);
    Canvas.SetTop((UIElement) this.selectionRectangle, this.zoomRect.Y);
  }

  protected internal override void OnMouseLeave(MouseEventArgs e)
  {
    if (this.ChartArea == null || !this.isZooming)
      return;
    if (!this.isLeftButtonPressed)
      this.AdorningCanvas.ReleaseMouseCapture();
    this.isZooming = false;
    this.selectionRectangle.Width = 0.0;
    this.selectionRectangle.Height = 0.0;
    if (this.zoomRect.Width <= 0.0 || this.zoomRect.Height <= 0.0)
      return;
    foreach (ChartAxisBase2D ax in (Collection<ChartAxis>) this.ChartArea.Axes)
      this.Zoom(this.zoomRect, ax);
    this.sel_ZoomingEndEventArgs.ZoomRect = this.zoomRect;
    this.ChartArea.OnSelectionZoomingEnd(this.sel_ZoomingEndEventArgs);
  }

  protected internal override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    if (this.ChartArea == null)
      return;
    foreach (ChartAxisBase2D ax in (Collection<ChartAxis>) this.ChartArea.Axes)
    {
      ax.IsScrolling = false;
      if (this.isPanningChanged)
        this.RaisePanChangedEvent(ax);
    }
    this.isLeftButtonPressed = false;
    this.AdorningCanvas.ReleaseMouseCapture();
    this.mouseCapture = false;
    if (this.isZooming)
    {
      this.isZooming = false;
      this.selectionRectangle.Width = 0.0;
      this.selectionRectangle.Height = 0.0;
      if (this.zoomRect.Width > 0.0 && this.zoomRect.Height > 0.0 && !this.sel_ZoomingDeltaEventArgs.Cancel)
      {
        foreach (ChartAxisBase2D ax in (Collection<ChartAxis>) this.ChartArea.Axes)
          this.Zoom(this.zoomRect, ax);
        this.sel_ZoomingEndEventArgs.ZoomRect = this.zoomRect;
        this.ChartArea.OnSelectionZoomingEnd(this.sel_ZoomingEndEventArgs);
      }
    }
    if (!this.ResetOnDoubleTap || !this.isReset)
      return;
    this.Reset();
    this.isReset = false;
  }

  internal override void OnTouchDown(TouchEventArgs e)
  {
    if (!this.EnableDirectionalZooming || !this.EnablePinchZooming)
      return;
    this.touchDevices.Add(e.TouchDevice);
    if (this.touchDevices.Count != 2)
      return;
    this.angle = ChartMath.GetAngle(this.touchDevices[0].GetTouchPoint((IInputElement) this.AdorningCanvas).Position, this.touchDevices[1].GetTouchPoint((IInputElement) this.AdorningCanvas).Position);
  }

  internal override void OnTouchMove(TouchEventArgs e)
  {
    if (this.touchDevices.Count != 2)
      return;
    for (int index = 0; index < this.touchDevices.Count; ++index)
    {
      if (this.touchDevices[index].Id == e.TouchDevice.Id)
        this.touchDevices[index] = e.TouchDevice;
    }
    this.angle = ChartMath.GetAngle(this.touchDevices[0].GetTouchPoint((IInputElement) this.AdorningCanvas).Position, this.touchDevices[1].GetTouchPoint((IInputElement) this.AdorningCanvas).Position);
  }

  internal override void OnTouchUp(TouchEventArgs e) => this.touchDevices.Remove(e.TouchDevice);

  protected internal override void OnManipulationStarted(ManipulationStartedEventArgs e)
  {
    this.previousScale = 1.0;
    if (this.ChartArea == null)
      return;
    foreach (ChartAxis ax in (Collection<ChartAxis>) this.ChartArea.Axes)
      ax.IsScrolling = true;
  }

  protected internal override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
  {
    base.OnManipulationCompleted(e);
    if (this.ChartArea == null)
      return;
    foreach (ChartAxisBase2D ax in (Collection<ChartAxis>) this.ChartArea.Axes)
    {
      if (this.isPanningChanged)
        this.RaisePanChangedEvent(ax);
      ax.IsScrolling = false;
    }
    if (this.selectionRectangle.Width <= 0.0 || this.selectionRectangle.Height <= 0.0)
      return;
    this.sel_ZoomingEndEventArgs.ZoomRect = this.zoomRect;
    this.ChartArea.OnSelectionZoomingEnd(this.sel_ZoomingEndEventArgs);
  }

  protected internal override void OnManipulationDelta(ManipulationDeltaEventArgs e)
  {
    if (this.ChartArea == null)
      return;
    bool flag1 = false;
    if (this.ChartArea.HoldUpdate || this.ChartArea.AreaType != ChartAreaType.CartesianAxes)
      return;
    double num = Math.Max(e.CumulativeManipulation.Scale.X, e.CumulativeManipulation.Scale.Y);
    foreach (ChartAxisBase2D ax in (Collection<ChartAxis>) this.ChartArea.Axes)
    {
      double currentScale = Math.Max(1.0 / ChartMath.MinMax(ax.ZoomFactor, 0.0, 1.0), 1.0);
      bool flag2 = e.CumulativeManipulation.Scale.X != 0.0 || e.CumulativeManipulation.Scale.Y != 0.0;
      double d = currentScale * ((num - this.previousScale) / this.previousScale);
      if (this.EnablePinchZooming && flag2 && num != this.previousScale && (ax.Orientation == Orientation.Vertical && this.VerticalMode || ax.Orientation == Orientation.Horizontal && this.HorizontalMode || this.CanDirectionalZoom(ax)))
      {
        if (!double.IsNaN(d) && !double.IsInfinity(d))
        {
          double cumulativeScale = this.ValMaxScaleLevel(Math.Max(currentScale + d, 1.0));
          if (cumulativeScale >= 1.0)
          {
            double origin = ax.Orientation != Orientation.Horizontal ? 1.0 - e.ManipulationOrigin.Y / this.ChartArea.ActualHeight : e.ManipulationOrigin.X / this.ChartArea.ActualWidth;
            flag1 |= this.Zoom(cumulativeScale, origin, ax);
          }
        }
      }
      else if (this.InternalEnablePanning && !this.isZooming && !this.isReset && e.CumulativeManipulation.Scale.X == 1.0 && e.CumulativeManipulation.Scale.Y == 1.0)
      {
        this.Translate(ax, e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y, currentScale);
        flag1 = true;
      }
    }
    if (flag1)
      this.UpdateArea();
    this.previousScale = num;
  }

  private bool CanDirectionalZoom(ChartAxisBase2D axis)
  {
    if (!this.EnableDirectionalZooming || this.ZoomMode != ZoomMode.XY)
      return this.ZoomMode == ZoomMode.XY;
    bool flag1 = this.angle >= 340.0 && this.angle <= 360.0 || this.angle >= 0.0 && this.angle <= 20.0 || this.angle >= 160.0 && this.angle <= 200.0;
    bool flag2 = this.angle >= 70.0 && this.angle <= 110.0 || this.angle >= 250.0 && this.angle <= 290.0;
    bool flag3 = this.angle > 20.0 && this.angle < 70.0 || this.angle > 110.0 && this.angle < 160.0 || this.angle > 200.0 && this.angle < 250.0 || this.angle > 290.0 && this.angle < 340.0;
    return axis.Orientation == Orientation.Horizontal && flag1 || axis.Orientation == Orientation.Vertical && flag2 || flag3;
  }

  protected internal void AddZoomingToolBar()
  {
    if (!this.EnableZoomingToolBar || this.ChartZoomingToolBar == null || this.ChartArea == null || this.ChartArea.ToolkitCanvas == null || this.ChartArea.AreaType != ChartAreaType.CartesianAxes || this.ChartArea.ToolkitCanvas.Children.Contains((UIElement) this.ChartZoomingToolBar))
      return;
    this.ChartArea.AddZoomToolBar(this.ChartZoomingToolBar, this);
  }

  protected internal void RemoveZoomingToolBar()
  {
    if (this.EnableZoomingToolBar || this.ChartZoomingToolBar == null || this.ChartArea == null || this.ChartArea.ToolkitCanvas == null || !this.ChartArea.ToolkitCanvas.Children.Contains((UIElement) this.ChartZoomingToolBar))
      return;
    this.ChartArea.RemoveZoomToolBar(this.ChartZoomingToolBar);
  }

  protected override void AttachElements()
  {
    this.SelectionRectangleBinding();
    if (this.EnableSelectionZooming && this.AdorningCanvas != null && !this.AdorningCanvas.Children.Contains((UIElement) this.selectionRectangle))
      this.AdorningCanvas.Children.Add((UIElement) this.selectionRectangle);
    this.AddZoomingToolBar();
  }

  protected override DependencyObject CloneBehavior(DependencyObject obj)
  {
    return base.CloneBehavior((DependencyObject) new ChartZoomPanBehavior()
    {
      EnableSelectionZooming = this.EnableSelectionZooming,
      EnablePanning = this.EnablePanning,
      EnableZoomingToolBar = this.EnableZoomingToolBar,
      ResetOnDoubleTap = this.ResetOnDoubleTap,
      MaximumZoomLevel = this.MaximumZoomLevel,
      EnableMouseWheelZooming = this.EnableMouseWheelZooming,
      EnablePinchZooming = this.EnablePinchZooming,
      HorizontalPosition = this.HorizontalPosition,
      VerticalPosition = this.VerticalPosition,
      ZoomMode = this.ZoomMode,
      ToolBarOrientation = this.ToolBarOrientation,
      ToolBarBackground = this.ToolBarBackground,
      ToolBarItems = this.ToolBarItems,
      ZoomRelativeToCursor = this.ZoomRelativeToCursor
    });
  }

  private static void OnToolBarItemsChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ChartZoomPanBehavior chartZoomPanBehavior) || chartZoomPanBehavior.ChartZoomingToolBar == null)
      return;
    chartZoomPanBehavior.ChartZoomingToolBar.SetItemSource();
  }

  private static void OnToolbarPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartZoomPanBehavior chartZoomPanBehavior = d as ChartZoomPanBehavior;
    if ((bool) e.NewValue)
    {
      if (chartZoomPanBehavior.ChartZoomingToolBar == null)
        chartZoomPanBehavior.ChartZoomingToolBar = new ZoomingToolBar()
        {
          ZoomBehavior = chartZoomPanBehavior
        };
      chartZoomPanBehavior.AddZoomingToolBar();
    }
    else
      chartZoomPanBehavior.RemoveZoomingToolBar();
  }

  private static void OnAlignmentPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartZoomPanBehavior).OnLayoutUpdated();
  }

  private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ChartZoomPanBehavior chartZoomPanBehavior = d as ChartZoomPanBehavior;
    if (chartZoomPanBehavior.ChartZoomingToolBar == null)
      return;
    chartZoomPanBehavior.ChartZoomingToolBar.ChangedOrientation();
  }

  private static void OnToolBarBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartZoomPanBehavior chartZoomPanBehavior = d as ChartZoomPanBehavior;
    if (chartZoomPanBehavior.ChartZoomingToolBar == null)
      return;
    chartZoomPanBehavior.ChartZoomingToolBar.ChangeBackground();
  }

  private static void OnZoomSelectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartZoomPanBehavior chartZoomPanBehavior = d as ChartZoomPanBehavior;
    if (chartZoomPanBehavior.ChartZoomingToolBar != null)
      chartZoomPanBehavior.ChartZoomingToolBar.SetItemSource();
    if ((bool) e.NewValue)
    {
      if (chartZoomPanBehavior.AdorningCanvas == null || chartZoomPanBehavior.AdorningCanvas.Children.Contains((UIElement) chartZoomPanBehavior.selectionRectangle))
        return;
      chartZoomPanBehavior.AdorningCanvas.Children.Add((UIElement) chartZoomPanBehavior.selectionRectangle);
    }
    else
      chartZoomPanBehavior.DetachElement((UIElement) chartZoomPanBehavior.selectionRectangle);
  }

  private static void CalZoomFactors(
    double cumulativeScale,
    double origin,
    double currentZoomFactor,
    double currentZoomPos,
    ref double calcZoomPos,
    ref double calcZoomFactor)
  {
    if (cumulativeScale == 1.0)
    {
      calcZoomFactor = 1.0;
      calcZoomPos = 0.0;
    }
    else
    {
      calcZoomFactor = ChartMath.MinMax(1.0 / cumulativeScale, 0.0, 1.0);
      calcZoomPos = currentZoomPos + (currentZoomFactor - calcZoomFactor) * origin;
    }
  }

  private void SelectionRectangleBinding()
  {
    if (this.selectionRectangle == null)
      return;
    this.selectionRectangle.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Path = new PropertyPath("StrokeThickness", new object[0]),
      Source = (object) this
    });
    this.selectionRectangle.SetBinding(Shape.FillProperty, (BindingBase) new Binding()
    {
      Path = new PropertyPath("Fill", new object[0]),
      Source = (object) this
    });
    this.selectionRectangle.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Path = new PropertyPath("Stroke", new object[0]),
      Source = (object) this
    });
  }

  private void MouseWheelZoom(Point mousePoint, double direction)
  {
    if (this.ChartArea == null)
      return;
    bool flag1 = false;
    Rect seriesClipRect = this.ChartArea.SeriesClipRect;
    mousePoint = new Point(mousePoint.X - seriesClipRect.Left, mousePoint.Y - seriesClipRect.Top);
    bool flag2 = false;
    foreach (ChartAxisBase2D ax in (Collection<ChartAxis>) this.ChartArea.Axes)
    {
      if (ax.RegisteredSeries.Count > 0 && ax.RegisteredSeries[0] is CartesianSeries && (ax.RegisteredSeries[0] as CartesianSeries).IsActualTransposed)
      {
        if (ax.Orientation == Orientation.Horizontal && (this.ZoomMode == ZoomMode.Y || this.ZoomMode == ZoomMode.XY) || ax.Orientation == Orientation.Vertical && (this.ZoomMode == ZoomMode.X || this.ZoomMode == ZoomMode.XY))
          flag2 = true;
      }
      else if (ax.Orientation == Orientation.Vertical && (this.ZoomMode == ZoomMode.Y || this.ZoomMode == ZoomMode.XY) || ax.Orientation == Orientation.Horizontal && (this.ZoomMode == ZoomMode.X || this.ZoomMode == ZoomMode.XY))
        flag2 = true;
      if (flag2)
      {
        double num1 = 0.5;
        double cumulativeScale = this.ValMaxScaleLevel(Math.Max(Math.Max(1.0 / ChartMath.MinMax(ax.ZoomFactor, 0.0, 1.0), 1.0) + 0.25 * direction, 1.0));
        if (this.ZoomRelativeToCursor)
          num1 = ax.Orientation != Orientation.Horizontal ? 1.0 - mousePoint.Y / seriesClipRect.Height : mousePoint.X / seriesClipRect.Width;
        double num2 = ax.IsInversed ? 1.0 - num1 : num1;
        flag1 |= this.Zoom(cumulativeScale, num2 > 1.0 ? 1.0 : (num2 < 0.0 ? 0.0 : num2), ax);
      }
      flag2 = false;
    }
    if (!flag1)
      return;
    this.UpdateArea();
  }

  private void RemoveTooltip()
  {
    foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeries>) this.ChartArea.Series)
      chartSeriesBase.RemoveTooltip();
  }

  private void RaiseSelectionZoomDeltaEvent()
  {
    this.sel_ZoomingDeltaEventArgs.ZoomRect = this.zoomRect;
    this.sel_ZoomingDeltaEventArgs.Cancel = false;
    this.ChartArea.OnSelectionZoomingDelta(this.sel_ZoomingDeltaEventArgs);
  }

  private void Translate(
    ChartAxisBase2D axis,
    double translateX,
    double translateY,
    double currentScale)
  {
    double zoomPosition = axis.ZoomPosition;
    double newZoomPosition;
    if (axis.Orientation == Orientation.Horizontal)
    {
      double num = translateX / this.AdorningCanvas.ActualWidth / currentScale;
      newZoomPosition = ChartMath.MinMax(axis.IsInversed ? axis.ZoomPosition + num : axis.ZoomPosition - num, 0.0, 1.0 - axis.ZoomFactor);
    }
    else
    {
      double num = translateY / this.AdorningCanvas.ActualHeight / currentScale;
      newZoomPosition = ChartMath.MinMax(axis.IsInversed ? axis.ZoomPosition - num : axis.ZoomPosition + num, 0.0, 1.0 - axis.ZoomFactor);
    }
    if (zoomPosition == newZoomPosition)
      return;
    this.RaisePanChangingEvent(axis, zoomPosition, newZoomPosition);
    if (this.panChangingEventArgs.Cancel)
      return;
    axis.ZoomPosition = newZoomPosition;
    this.isPanningChanged = true;
  }

  private void RaisePanChangingEvent(
    ChartAxisBase2D axis,
    double preZoomPosition,
    double newZoomPosition)
  {
    this.panChangingEventArgs.Axis = axis;
    this.panChangingEventArgs.NewZoomPosition = newZoomPosition;
    this.panChangingEventArgs.OldZoomPosition = preZoomPosition;
    this.panChangingEventArgs.Cancel = false;
    this.ChartArea.OnPanChanging(this.panChangingEventArgs);
  }

  private void RaisePanChangedEvent(ChartAxisBase2D axis)
  {
    this.panChangedEventArgs.Axis = axis;
    this.panChangedEventArgs.NewZoomPosition = axis.ZoomPosition;
    this.ChartArea.OnPanChanged(this.panChangedEventArgs);
  }

  private void RaiseZoomChangedEvent(double zoomFactor, double zoomPosition, ChartAxisBase2D axis)
  {
    DoubleRange range = axis.CalculateRange(axis.ActualRange, axis.ZoomPosition, axis.ZoomFactor);
    this.zoomChangedEventArgs.Axis = axis;
    this.zoomChangedEventArgs.CurrentFactor = axis.ZoomFactor;
    this.zoomChangedEventArgs.CurrentPosition = axis.ZoomPosition;
    switch (axis)
    {
      case DateTimeAxis _:
      case DateTimeCategoryAxis _:
        this.zoomChangedEventArgs.OldRange = (object) new DateTimeRange(axis.VisibleRange.Start.FromOADate(), axis.VisibleRange.End.FromOADate());
        this.zoomChangedEventArgs.NewRange = (object) new DateTimeRange(range.Start.FromOADate(), range.End.FromOADate());
        break;
      default:
        this.zoomChangedEventArgs.OldRange = (object) new DoubleRange(axis.VisibleRange.Start, axis.VisibleRange.End);
        this.zoomChangedEventArgs.NewRange = (object) range;
        break;
    }
    this.zoomChangedEventArgs.PreviousFactor = zoomFactor;
    this.zoomChangedEventArgs.PreviousPosition = zoomPosition;
    this.ChartArea.OnZoomChanged(this.zoomChangedEventArgs);
  }

  private void RaiseZoomChangingEvent(double zoomPosition, double zoomFactor, ChartAxisBase2D axis)
  {
    this.zoomChangingEventArgs.Axis = axis;
    this.zoomChangingEventArgs.CurrentFactor = zoomFactor;
    this.zoomChangingEventArgs.PreviousFactor = axis.ZoomFactor;
    this.zoomChangingEventArgs.PreviousPosition = axis.ZoomPosition;
    this.zoomChangingEventArgs.CurrentPosition = zoomPosition;
    switch (axis)
    {
      case DateTimeAxis _:
      case DateTimeCategoryAxis _:
        this.zoomChangingEventArgs.OldRange = (object) new DateTimeRange(axis.VisibleRange.Start.FromOADate(), axis.VisibleRange.End.FromOADate());
        break;
      default:
        this.zoomChangingEventArgs.OldRange = (object) new DoubleRange(axis.VisibleRange.Start, axis.VisibleRange.End);
        break;
    }
    this.zoomChangingEventArgs.Cancel = false;
    this.ChartArea.OnZoomChanging(this.zoomChangingEventArgs);
  }

  private void Zoom(Rect zoomRect, ChartAxisBase2D axis)
  {
    double zoomFactor = axis.ZoomFactor;
    double zoomPosition = axis.ZoomPosition;
    Rect rect = new Rect();
    bool isPointInsideRect = false;
    if (axis.Orientation == Orientation.Horizontal)
    {
      if (this.ChartArea.IsMultipleArea)
      {
        ChartExtensionUtils.GetAxisArrangeRect(this.startPoint, (ChartAxis) axis, out isPointInsideRect);
        if (!isPointInsideRect)
          return;
        double num = this.areaRect.Width - axis.GetActualPlotOffset();
        double width = num > 0.0 ? num : 0.0;
        rect = new Rect(new Point(this.areaRect.X + axis.GetActualPlotOffsetStart(), this.areaRect.Y), new Size(width, this.areaRect.Height));
      }
      else
      {
        double num = this.ChartArea.SeriesClipRect.Width - axis.GetActualPlotOffset();
        double width = num > 0.0 ? num : 0.0;
        rect = new Rect(new Point(this.ChartArea.SeriesClipRect.X + axis.GetActualPlotOffsetStart(), this.ChartArea.SeriesClipRect.Y), new Size(width, this.ChartArea.SeriesClipRect.Height));
      }
      if (zoomRect.X < rect.X)
      {
        double num = zoomRect.Width - (rect.X - zoomRect.X);
        double width = num > 0.0 ? num : (this.StrokeThickness > 0.0 ? this.StrokeThickness : 1.0);
        zoomRect = new Rect(new Point(rect.X, zoomRect.Y), new Size(width, zoomRect.Height));
      }
      if (zoomRect.Right > rect.Right)
      {
        double num = zoomRect.Width - (zoomRect.Right - rect.Right);
        double width = num > 0.0 ? num : (this.StrokeThickness > 0.0 ? this.StrokeThickness : 1.0);
        zoomRect = new Rect(new Point(zoomRect.X, zoomRect.Y), new Size(width, zoomRect.Height));
      }
      double currentZoomFactor = zoomFactor * (zoomRect.Width / rect.Width);
      if (currentZoomFactor == zoomFactor || !this.ValMaxZoomLevel(currentZoomFactor))
        return;
      axis.ZoomFactor = !this.VerticalMode ? currentZoomFactor : 1.0;
      axis.ZoomPosition = !this.VerticalMode ? zoomPosition + Math.Abs((axis.IsInversed ? rect.Right - zoomRect.Right : zoomRect.X - rect.Left) / rect.Width) * zoomFactor : 0.0;
    }
    else
    {
      if (this.ChartArea.IsMultipleArea)
      {
        ChartExtensionUtils.GetAxisArrangeRect(this.startPoint, (ChartAxis) axis, out isPointInsideRect);
        if (!isPointInsideRect)
          return;
        double num = this.areaRect.Height - axis.GetActualPlotOffset();
        double height = num > 0.0 ? num : 0.0;
        rect = new Rect(new Point(this.areaRect.X, this.areaRect.Y + axis.GetActualPlotOffsetEnd()), new Size(this.areaRect.Width, height));
      }
      else
      {
        double num = this.ChartArea.SeriesClipRect.Height - axis.GetActualPlotOffset();
        double height = num > 0.0 ? num : 0.0;
        rect = new Rect(new Point(this.ChartArea.SeriesClipRect.X, this.ChartArea.SeriesClipRect.Y + axis.GetActualPlotOffsetEnd()), new Size(this.ChartArea.SeriesClipRect.Width, height));
      }
      if (zoomRect.Y < rect.Y)
      {
        double num = zoomRect.Height - (rect.Y - zoomRect.Y);
        double height = num > 0.0 ? num : (this.StrokeThickness > 0.0 ? this.StrokeThickness : 1.0);
        zoomRect = new Rect(new Point(zoomRect.X, rect.Y), new Size(zoomRect.Width, height));
      }
      if (rect.Bottom < zoomRect.Bottom)
      {
        double num = zoomRect.Height - (zoomRect.Bottom - rect.Bottom);
        double height = num > 0.0 ? num : (this.StrokeThickness > 0.0 ? this.StrokeThickness : 1.0);
        zoomRect = new Rect(new Point(zoomRect.X, zoomRect.Y), new Size(zoomRect.Width, height));
      }
      double currentZoomFactor = zoomFactor * zoomRect.Height / rect.Height;
      if (currentZoomFactor == zoomFactor || !this.ValMaxZoomLevel(currentZoomFactor))
        return;
      axis.ZoomFactor = !this.HorizontalMode ? currentZoomFactor : 1.0;
      axis.ZoomPosition = !this.HorizontalMode ? zoomPosition + (1.0 - Math.Abs((axis.IsInversed ? zoomRect.Top - rect.Bottom : zoomRect.Bottom - rect.Top) / rect.Height)) * zoomFactor : 0.0;
    }
  }

  internal bool CanZoom(ChartAxisBase2D axis)
  {
    bool flag = false;
    if (axis.RegisteredSeries.Count > 0 && axis.RegisteredSeries[0] is CartesianSeries && (axis.RegisteredSeries[0] as CartesianSeries).IsActualTransposed)
    {
      if (axis.Orientation == Orientation.Horizontal && (this.ZoomMode == ZoomMode.Y || this.ZoomMode == ZoomMode.XY) || axis.Orientation == Orientation.Vertical && (this.ZoomMode == ZoomMode.X || this.ZoomMode == ZoomMode.XY))
        flag = true;
    }
    else if (axis.Orientation == Orientation.Vertical && (this.ZoomMode == ZoomMode.Y || this.ZoomMode == ZoomMode.XY) || axis.Orientation == Orientation.Horizontal && (this.ZoomMode == ZoomMode.X || this.ZoomMode == ZoomMode.XY))
      flag = true;
    return flag;
  }

  private void RaiseResetZoomingEvent(ChartAxisBase2D axis)
  {
    this.zoomingResetEventArgs.Axis = axis;
    switch (axis)
    {
      case DateTimeAxis _:
      case DateTimeCategoryAxis _:
        this.zoomingResetEventArgs.PreviousZoomRange = (object) new DateTimeRange(axis.VisibleRange.Start.FromOADate(), axis.VisibleRange.End.FromOADate());
        break;
      default:
        this.zoomingResetEventArgs.PreviousZoomRange = (object) new DoubleRange(axis.VisibleRange.Start, axis.VisibleRange.End);
        break;
    }
    this.zoomingResetEventArgs.Cancel = false;
    this.ChartArea.OnResetZoom(this.zoomingResetEventArgs);
  }

  private bool ValMaxZoomLevel(double currentZoomFactor)
  {
    return !this.IsMaxZoomLevel || 1.0 / currentZoomFactor <= this.MaximumZoomLevel;
  }

  private double ValMaxScaleLevel(double cumulativeScale)
  {
    return this.IsMaxZoomLevel && cumulativeScale > this.MaximumZoomLevel ? this.MaximumZoomLevel : cumulativeScale;
  }
}
