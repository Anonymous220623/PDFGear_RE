// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartAxisBase2D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class ChartAxisBase2D : ChartAxis
{
  public static readonly DependencyProperty AutoScrollingModeProperty = DependencyProperty.Register(nameof (AutoScrollingMode), typeof (ChartAutoScrollingMode), typeof (ChartAxisBase2D), new PropertyMetadata((object) ChartAutoScrollingMode.End, new PropertyChangedCallback(ChartAxisBase2D.OnAutoScrollingPropertyChanged)));
  public static readonly DependencyProperty AutoScrollingDeltaProperty = DependencyProperty.Register(nameof (AutoScrollingDelta), typeof (double), typeof (ChartAxisBase2D), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(ChartAxisBase2D.OnAutoScrollingPropertyChanged)));
  public static readonly DependencyProperty ZoomPositionProperty = DependencyProperty.Register(nameof (ZoomPosition), typeof (double), typeof (ChartAxisBase2D), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ChartAxisBase2D.OnZoomPositionChanged)));
  public static readonly DependencyProperty ZoomFactorProperty = DependencyProperty.Register(nameof (ZoomFactor), typeof (double), typeof (ChartAxisBase2D), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(ChartAxisBase2D.OnZoomFactorChanged)));
  public static readonly DependencyProperty PolarAngleProperty = DependencyProperty.Register(nameof (PolarAngle), typeof (ChartPolarAngle), typeof (ChartAxisBase2D), new PropertyMetadata((object) ChartPolarAngle.Rotate270, new PropertyChangedCallback(ChartAxisBase2D.OnAxisPropertyChanged)));
  public static readonly DependencyProperty LabelBorderBrushProperty = DependencyProperty.Register(nameof (LabelBorderBrush), typeof (Brush), typeof (ChartAxisBase2D), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ShowLabelBorderProperty = DependencyProperty.Register(nameof (ShowLabelBorder), typeof (bool), typeof (ChartAxisBase2D), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartAxisBase2D.OnAxisPropertyChanged)));
  public static readonly DependencyProperty LabelBorderWidthProperty = DependencyProperty.Register(nameof (LabelBorderWidth), typeof (double), typeof (ChartAxisBase2D), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(ChartAxisBase2D.OnLabelBorderWidthPropertyChanged)));
  public static readonly DependencyProperty IncludeStripLineRangeProperty = DependencyProperty.Register(nameof (IncludeStripLineRange), typeof (bool), typeof (ChartAxisBase2D), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartAxisBase2D.OnIncludeStripLineRangeChanged)));
  public static readonly DependencyProperty StripLinesProperty = DependencyProperty.Register(nameof (StripLines), typeof (ChartStripLines), typeof (ChartAxisBase2D), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxisBase2D.OnStripLinesChanged)));
  public static readonly DependencyProperty MultiLevelLabelsProperty = DependencyProperty.Register(nameof (MultiLevelLabels), typeof (ChartMultiLevelLabels), typeof (ChartAxisBase2D), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxisBase2D.OnMultiLevelLabelsChanged)));
  public static readonly DependencyProperty MultiLevelLabelsBorderTypeProperty = DependencyProperty.Register(nameof (MultiLevelLabelsBorderType), typeof (BorderType), typeof (ChartAxisBase2D), new PropertyMetadata((object) BorderType.Rectangle, new PropertyChangedCallback(ChartAxisBase2D.OnAxisPropertyChanged)));
  public static readonly DependencyProperty EnableScrollBarResizingProperty = DependencyProperty.Register(nameof (EnableScrollBarResizing), typeof (bool), typeof (ChartAxis), new PropertyMetadata((object) true, new PropertyChangedCallback(ChartAxisBase2D.OnScrollBarResizableChanged)));
  public static readonly DependencyProperty EnableScrollBarProperty = DependencyProperty.Register(nameof (EnableScrollBar), typeof (bool), typeof (ChartAxis), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartAxisBase2D.OnEnableScrollBarValueChanged)));
  public static readonly DependencyProperty DeferredScrollingProperty = DependencyProperty.Register(nameof (DeferredScrolling), typeof (bool), typeof (ChartAxis), new PropertyMetadata((object) false));
  public static readonly DependencyProperty EnableTouchModeProperty = DependencyProperty.Register(nameof (EnableTouchMode), typeof (bool), typeof (ChartAxis), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartAxisBase2D.OnEnableTouchModeChanged)));
  internal static readonly DependencyProperty AxisStyleProperty = DependencyProperty.Register(nameof (AxisStyle), typeof (Style), typeof (ChartAxisBase2D), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxisBase2D.OnAxisPropertyChanged)));
  internal ChartCartesianAxisPanel axisPanel;
  internal SfChartResizableBar sfChartResizableBar;
  private bool isUpdateStripDispatched;
  private Panel labelsPanel;
  private Panel multiLevelLabelsPanel;
  private Panel elementsPanel;
  private double rangeEnd = 1.0;

  public ChartAxisBase2D()
  {
    this.DefaultStyleKey = (object) typeof (ChartAxisBase2D);
    this.SetBinding();
    this.StripLines = new ChartStripLines();
    this.MultiLevelLabels = new ChartMultiLevelLabels();
  }

  public event EventHandler<AxisLabelClickedEventArgs> LabelClicked;

  public ChartAutoScrollingMode AutoScrollingMode
  {
    get => (ChartAutoScrollingMode) this.GetValue(ChartAxisBase2D.AutoScrollingModeProperty);
    set => this.SetValue(ChartAxisBase2D.AutoScrollingModeProperty, (object) value);
  }

  public double AutoScrollingDelta
  {
    get => (double) this.GetValue(ChartAxisBase2D.AutoScrollingDeltaProperty);
    set => this.SetValue(ChartAxisBase2D.AutoScrollingDeltaProperty, (object) value);
  }

  public double ZoomPosition
  {
    get => (double) this.GetValue(ChartAxisBase2D.ZoomPositionProperty);
    set => this.SetValue(ChartAxisBase2D.ZoomPositionProperty, (object) value);
  }

  public double ZoomFactor
  {
    get => (double) this.GetValue(ChartAxisBase2D.ZoomFactorProperty);
    set => this.SetValue(ChartAxisBase2D.ZoomFactorProperty, (object) value);
  }

  public ChartPolarAngle PolarAngle
  {
    get => (ChartPolarAngle) this.GetValue(ChartAxisBase2D.PolarAngleProperty);
    set => this.SetValue(ChartAxisBase2D.PolarAngleProperty, (object) value);
  }

  public Brush LabelBorderBrush
  {
    get => (Brush) this.GetValue(ChartAxisBase2D.LabelBorderBrushProperty);
    set => this.SetValue(ChartAxisBase2D.LabelBorderBrushProperty, (object) value);
  }

  public bool ShowLabelBorder
  {
    get => (bool) this.GetValue(ChartAxisBase2D.ShowLabelBorderProperty);
    set => this.SetValue(ChartAxisBase2D.ShowLabelBorderProperty, (object) value);
  }

  public ChartMultiLevelLabels MultiLevelLabels
  {
    get => (ChartMultiLevelLabels) this.GetValue(ChartAxisBase2D.MultiLevelLabelsProperty);
    set => this.SetValue(ChartAxisBase2D.MultiLevelLabelsProperty, (object) value);
  }

  public BorderType MultiLevelLabelsBorderType
  {
    get => (BorderType) this.GetValue(ChartAxisBase2D.MultiLevelLabelsBorderTypeProperty);
    set => this.SetValue(ChartAxisBase2D.MultiLevelLabelsBorderTypeProperty, (object) value);
  }

  public bool IncludeStripLineRange
  {
    get => (bool) this.GetValue(ChartAxisBase2D.IncludeStripLineRangeProperty);
    set => this.SetValue(ChartAxisBase2D.IncludeStripLineRangeProperty, (object) value);
  }

  public ChartStripLines StripLines
  {
    get => (ChartStripLines) this.GetValue(ChartAxisBase2D.StripLinesProperty);
    set => this.SetValue(ChartAxisBase2D.StripLinesProperty, (object) value);
  }

  public bool EnableScrollBarResizing
  {
    get => (bool) this.GetValue(ChartAxisBase2D.EnableScrollBarResizingProperty);
    set => this.SetValue(ChartAxisBase2D.EnableScrollBarResizingProperty, (object) value);
  }

  public bool EnableScrollBar
  {
    get => (bool) this.GetValue(ChartAxisBase2D.EnableScrollBarProperty);
    set => this.SetValue(ChartAxisBase2D.EnableScrollBarProperty, (object) value);
  }

  public bool DeferredScrolling
  {
    get => (bool) this.GetValue(ChartAxisBase2D.DeferredScrollingProperty);
    set => this.SetValue(ChartAxisBase2D.DeferredScrollingProperty, (object) value);
  }

  public double LabelBorderWidth
  {
    get => (double) this.GetValue(ChartAxisBase2D.LabelBorderWidthProperty);
    set => this.SetValue(ChartAxisBase2D.LabelBorderWidthProperty, (object) value);
  }

  public bool EnableTouchMode
  {
    get => (bool) this.GetValue(ChartAxisBase2D.EnableTouchModeProperty);
    set => this.SetValue(ChartAxisBase2D.EnableTouchModeProperty, (object) value);
  }

  internal bool CanAutoScroll { get; set; }

  internal bool IsZoomed => this.ZoomFactor != 1.0;

  internal Style AxisStyle
  {
    get => (Style) this.GetValue(ChartAxisBase2D.AxisStyleProperty);
    set => this.SetValue(ChartAxisBase2D.AxisStyleProperty, (object) value);
  }

  internal double ActualZoomFactor => this.ZoomFactor != 0.0 ? this.ZoomFactor : 0.01;

  public override double ValueToPolarCoefficient(double value)
  {
    double start = this.VisibleRange.Start;
    double delta = this.VisibleRange.Delta;
    double num = (value - start) / delta;
    return this.ValueBasedOnAngle(this.VisibleLabels.Count <= 0 ? num * (1.0 - 1.0 / (delta + 1.0)) : num * (1.0 - 1.0 / (double) this.VisibleLabels.Count));
  }

  public override double PolarCoefficientToValue(double value)
  {
    value = this.ValueBasedOnAngle(value);
    if (this.VisibleLabels.Count > 0)
      value /= 1.0 - 1.0 / (double) this.VisibleLabels.Count;
    else
      value /= 1.0 - 1.0 / (this.VisibleRange.Delta + 1.0);
    return this.VisibleRange.Start + this.VisibleRange.Delta * value;
  }

  internal virtual void UpdateAutoScrollingDelta(double scrollingDelta)
  {
    if (this.AutoScrollingMode == ChartAutoScrollingMode.Start)
    {
      this.VisibleRange = new DoubleRange(this.ActualRange.Start, this.ActualRange.Start + scrollingDelta);
      this.ZoomFactor = this.VisibleRange.Delta / this.ActualRange.Delta;
      this.ZoomPosition = 0.0;
    }
    else
    {
      this.VisibleRange = new DoubleRange(this.ActualRange.End - scrollingDelta, this.ActualRange.End);
      this.ZoomFactor = this.VisibleRange.Delta / this.ActualRange.Delta;
      this.ZoomPosition = 1.0 - this.ZoomFactor;
    }
  }

  internal override void Dispose()
  {
    if (this.LabelClicked != null)
    {
      foreach (Delegate invocation in this.LabelClicked.GetInvocationList())
        this.LabelClicked -= invocation as EventHandler<AxisLabelClickedEventArgs>;
      this.LabelClicked = (EventHandler<AxisLabelClickedEventArgs>) null;
    }
    if (this.AssociatedAxes != null)
      this.AssociatedAxes.Clear();
    this.LabelsSource = (object) null;
    this.Area = (ChartBase) null;
    this.DisposePanels();
    this.PrefixLabelTemplate = (DataTemplate) null;
    this.PostfixLabelTemplate = (DataTemplate) null;
    this.LabelTemplate = (DataTemplate) null;
    this.DisposeEvents();
    base.Dispose();
  }

  internal override void CreateLineRecycler()
  {
    if (this.Area == null || (this.Area as SfChart).GridLinesPanel == null)
      return;
    this.GridLinesRecycler = new UIElementsRecycler<Line>((this.Area as SfChart).GridLinesPanel);
    this.MinorGridLinesRecycler = new UIElementsRecycler<Line>((this.Area as SfChart).GridLinesPanel);
  }

  internal override void ComputeDesiredSize(Size size)
  {
    this.ClearValue(FrameworkElement.HeightProperty);
    this.ClearValue(FrameworkElement.WidthProperty);
    if (this.sfChartResizableBar != null)
    {
      this.sfChartResizableBar.ClearValue(FrameworkElement.WidthProperty);
      this.sfChartResizableBar.ClearValue(FrameworkElement.HeightProperty);
    }
    this.AvailableSize = size;
    this.CalculateRangeAndInterval(size);
    if (this.Visibility != Visibility.Collapsed || this.Area.AreaType == ChartAreaType.PolarAxes)
    {
      this.ApplyTemplate();
      if (this.axisPanel == null)
        return;
      this.UpdatePanels();
      this.UpdateLabels();
      this.ComputedDesiredSize = this.axisPanel.ComputeSize(size);
    }
    else
    {
      this.ActualPlotOffset = this.PlotOffset;
      this.ActualPlotOffsetStart = this.PlotOffsetStart;
      this.ActualPlotOffsetEnd = this.PlotOffsetEnd;
      this.InsidePadding = 0.0;
      this.UpdateLabels();
      this.ComputedDesiredSize = this.Orientation == Orientation.Horizontal ? new Size(size.Width, 0.0) : new Size(0.0, size.Height);
    }
  }

  internal void ChangeStyle(bool modeValue)
  {
    if (this.sfChartResizableBar == null)
      return;
    if (modeValue)
    {
      if (this.OpposedPosition)
        VisualStateManager.GoToState((FrameworkElement) this, "OpposedTouchModeStyle", true);
      else
        VisualStateManager.GoToState((FrameworkElement) this, "TouchModeStyle", true);
    }
    else
      VisualStateManager.GoToState((FrameworkElement) this, "CommonStyle", true);
    this.sfChartResizableBar.UpdateResizable(this.EnableScrollBarResizing);
  }

  internal DoubleRange CalculateRange(
    DoubleRange actualRange,
    double zoomPosition,
    double zoomFactor)
  {
    DoubleRange doubleRange = actualRange;
    DoubleRange range = new DoubleRange();
    double start = this.ActualRange.Start + this.ZoomPosition * actualRange.Delta;
    double num = start + this.ActualZoomFactor * actualRange.Delta;
    if (start < doubleRange.Start)
    {
      num += doubleRange.Start - start;
      start = doubleRange.Start;
    }
    if (num > doubleRange.End || this.EnableScrollBar && this.sfChartResizableBar != null && Math.Round(num) == doubleRange.End && this.sfChartResizableBar.RangeEnd == this.rangeEnd && this.RegisteredSeries.Count > 0 && !(this.RegisteredSeries[0] as ChartSeries).IsActualTransposed)
    {
      start -= num - doubleRange.End;
      num = doubleRange.End;
    }
    this.rangeEnd = this.sfChartResizableBar != null ? this.sfChartResizableBar.RangeEnd : 1.0;
    range = new DoubleRange(start, num);
    return range;
  }

  internal double ValueBasedOnAngle(double result)
  {
    if (this.PolarAngle == ChartPolarAngle.Rotate270)
      result = this.IsInversed ? result : 1.0 - result;
    else if (this.PolarAngle == ChartPolarAngle.Rotate0)
      result = this.IsInversed ? 0.75 + result : 0.75 - result;
    else if (this.PolarAngle == ChartPolarAngle.Rotate90)
      result = this.IsInversed ? 0.5 + result : 0.5 - result;
    else if (this.PolarAngle == ChartPolarAngle.Rotate180)
      result = this.IsInversed ? 0.25 + result : 0.25 - result;
    result = result < 0.0 ? result + 1.0 : result;
    result = result > 1.0 ? result - 1.0 : result;
    return result;
  }

  internal void SetLabelDownArguments(object source)
  {
    if (this.LabelClicked == null || !(source is FrameworkElement element))
      return;
    FrameworkElement parent = ChartAxisBase2D.GetParent(element);
    if (parent == null)
      return;
    AxisLabelClickedEventArgs e = new AxisLabelClickedEventArgs();
    e.AxisLabel = parent.Tag as ChartAxisLabel;
    if (e.AxisLabel == null)
      return;
    this.LabelClicked((object) this, e);
  }

  protected internal override void CalculateVisibleRange(Size avalableSize)
  {
    this.VisibleRange = this.ActualRange;
    this.VisibleInterval = this.ActualInterval;
    if (this.ZoomFactor >= 1.0 && this.ZoomPosition <= 0.0)
      return;
    this.VisibleRange = this.CalculateRange(this.ActualRange, this.ZoomPosition, this.ZoomFactor);
  }

  protected internal override void OnAxisBoundsChanged(ChartAxisBoundsEventArgs args)
  {
    base.OnAxisBoundsChanged(args);
    if (this.sfChartResizableBar != null)
    {
      if (this.Orientation == Orientation.Horizontal)
      {
        this.sfChartResizableBar.Width = this.ArrangeRect.Width;
        if (this.Area.VisibleSeries.Count != 0)
          this.Width = this.ArrangeRect.Width;
      }
      else
      {
        this.sfChartResizableBar.Height = this.ArrangeRect.Height;
        if (this.Area.VisibleSeries.Count != 0)
          this.Height = this.ArrangeRect.Height;
      }
    }
    if (this.axisPanel == null)
      return;
    this.axisPanel.ArrangeElements(new Size(this.ArrangeRect.Width, this.ArrangeRect.Height));
  }

  protected override DependencyObject CloneAxis(DependencyObject obj)
  {
    ChartAxisBase2D chartAxisBase2D = obj as ChartAxisBase2D;
    chartAxisBase2D.ZoomFactor = this.ZoomFactor;
    chartAxisBase2D.ZoomPosition = this.ZoomPosition;
    chartAxisBase2D.IncludeStripLineRange = this.IncludeStripLineRange;
    chartAxisBase2D.DeferredScrolling = this.DeferredScrolling;
    chartAxisBase2D.EnableScrollBar = this.EnableScrollBar;
    chartAxisBase2D.EnableScrollBarResizing = this.EnableScrollBarResizing;
    chartAxisBase2D.EnableTouchMode = this.EnableTouchMode;
    chartAxisBase2D.LabelBorderBrush = this.LabelBorderBrush;
    chartAxisBase2D.LabelBorderWidth = this.LabelBorderWidth;
    chartAxisBase2D.MultiLevelLabelsBorderType = this.MultiLevelLabelsBorderType;
    chartAxisBase2D.ShowLabelBorder = this.ShowLabelBorder;
    foreach (ChartStripLine stripLine in (Collection<ChartStripLine>) this.StripLines)
      chartAxisBase2D.StripLines.Add((ChartStripLine) stripLine.Clone());
    foreach (ChartMultiLevelLabel multiLevelLabel in (Collection<ChartMultiLevelLabel>) this.MultiLevelLabels)
      chartAxisBase2D.MultiLevelLabels.Add((ChartMultiLevelLabel) multiLevelLabel.Clone());
    return base.CloneAxis(obj);
  }

  public override void OnApplyTemplate()
  {
    this.ClearItems();
    base.OnApplyTemplate();
    this.sfChartResizableBar = this.GetTemplateChild("sfchartResizableBar") as SfChartResizableBar;
    if (this.sfChartResizableBar != null)
    {
      this.sfChartResizableBar.Visibility = this.EnableScrollBar ? Visibility.Visible : Visibility.Collapsed;
      this.sfChartResizableBar.Axis = this;
      BindingOperations.SetBinding((DependencyObject) this.sfChartResizableBar, ResizableScrollBar.OrientationProperty, (BindingBase) new Binding()
      {
        Source = (object) this,
        Path = new PropertyPath("Orientation", new object[0])
      });
      if (this.EnableScrollBar)
        this.ChangeStyle(this.EnableTouchMode);
    }
    this.labelsPanel = this.GetTemplateChild("axisLabelsPanel") as Panel;
    this.elementsPanel = this.GetTemplateChild("axisElementPanel") as Panel;
    this.multiLevelLabelsPanel = this.GetTemplateChild("axisMultiLevelLabelsPanels") as Panel;
    this.axisPanel = this.GetTemplateChild("axisPanel") as ChartCartesianAxisPanel;
    if (this.axisPanel != null)
      this.axisPanel.Axis = this;
    if (this.axisPanel != null)
    {
      foreach (ChartStripLine stripLine in (Collection<ChartStripLine>) this.StripLines)
      {
        if (!this.axisPanel.Children.Contains((UIElement) stripLine))
          this.axisPanel.Children.Add((UIElement) stripLine);
      }
      if (this.axisPanel.Axis is NumericalAxis axis && axis.IsSecondaryAxis)
      {
        foreach (ChartAxisScaleBreak axisScaleBreak in (Collection<ChartAxisScaleBreak>) axis.AxisScaleBreaks)
        {
          if (!this.axisPanel.Children.Contains((UIElement) axisScaleBreak))
            this.axisPanel.Children.Add((UIElement) axisScaleBreak);
        }
      }
    }
    this.UpdatePanels();
    this.headerContent = this.GetTemplateChild("headerContent") as ContentControl;
    if (this.HeaderTemplate == null && this.headerContent != null && this.HeaderStyle != null)
    {
      if (this.HeaderStyle.Foreground != null)
      {
        Binding binding = new Binding()
        {
          Source = (object) this.HeaderStyle,
          Path = new PropertyPath("Foreground", new object[0])
        };
        this.headerContent.SetBinding(Control.ForegroundProperty, (BindingBase) binding);
      }
      if (this.HeaderStyle.FontSize != 0.0)
      {
        Binding binding = new Binding()
        {
          Source = (object) this.HeaderStyle,
          Path = new PropertyPath("FontSize", new object[0])
        };
        this.headerContent.SetBinding(Control.FontSizeProperty, (BindingBase) binding);
      }
      if (this.HeaderStyle.FontFamily != null)
      {
        Binding binding = new Binding()
        {
          Source = (object) this.HeaderStyle,
          Path = new PropertyPath("FontFamily", new object[0])
        };
        this.headerContent.SetBinding(Control.FontFamilyProperty, (BindingBase) binding);
      }
    }
    if (this.axisElementsUpdateRequired)
    {
      if (this.axisElementsPanel != null)
        this.axisElementsPanel.UpdateElements();
      if (this.axisLabelsPanel != null)
        this.axisLabelsPanel.UpdateElements();
      if (this.axisMultiLevelLabelsPanel != null && this.MultiLevelLabels.Count > 0)
        this.axisMultiLevelLabelsPanel.UpdateElements();
      this.axisElementsUpdateRequired = false;
    }
    if (this.Area == null || (this.Area as SfChart).GridLinesPanel == null)
      return;
    this.GridLinesRecycler = new UIElementsRecycler<Line>((this.Area as SfChart).GridLinesPanel);
    this.MinorGridLinesRecycler = new UIElementsRecycler<Line>((this.Area as SfChart).GridLinesPanel);
  }

  private static void OnAutoScrollingPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ChartAxisBase2D chartAxisBase2D))
      return;
    chartAxisBase2D.CanAutoScroll = true;
    chartAxisBase2D.OnPropertyChanged();
  }

  private static void OnAxisPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ChartAxisBase2D chartAxisBase2D))
      return;
    chartAxisBase2D.OnPropertyChanged();
  }

  private static void OnLabelBorderWidthPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartAxisBase2D chartAxisBase2D = d as ChartAxisBase2D;
    if (e.OldValue != null && Convert.ToDouble(e.OldValue) > 0.0 && Convert.ToDouble(e.NewValue) == 0.0)
      chartAxisBase2D.OnLabelBorderWidthChanged();
    chartAxisBase2D.OnPropertyChanged();
  }

  private static void OnZoomFactorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ChartAxisBase2D chartAxisBase2D = d as ChartAxisBase2D;
    if (d is NumericalAxis numericalAxis && numericalAxis.BreakExistence())
      numericalAxis.ClearBreakElements();
    chartAxisBase2D.OnZoomDataChanged(e);
    if (chartAxisBase2D.Area == null || (chartAxisBase2D.Area as SfChart).zoomingToolBar == null)
      return;
    chartAxisBase2D.EnableZoomingToolBarState();
  }

  private static void OnZoomPositionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartAxisBase2D chartAxisBase2D = d as ChartAxisBase2D;
    if (d is NumericalAxis numericalAxis && numericalAxis.BreakExistence())
      numericalAxis.ClearBreakElements();
    chartAxisBase2D.OnZoomDataChanged(e);
    if (chartAxisBase2D.Area == null || (chartAxisBase2D.Area as SfChart).zoomingToolBar == null)
      return;
    chartAxisBase2D.EnableZoomingToolBarState();
  }

  private static void OnIncludeStripLineRangeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ChartAxis chartAxis) || chartAxis.Area == null)
      return;
    (chartAxis.Area as SfChart).UpdateStripLines();
  }

  private static void OnStripLinesChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    (d as ChartAxisBase2D).OnStripLinesChanged(args.NewValue as ChartStripLines, args.OldValue as ChartStripLines);
  }

  private static void OnMultiLevelLabelsChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAxisBase2D).OnMultiLevelLabelsCollectionChanged(e.OldValue as ChartMultiLevelLabels, e.NewValue as ChartMultiLevelLabels);
  }

  private static void OnScrollBarResizableChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    (d as ChartAxisBase2D).OnScrollBarResizableChanged();
  }

  private static void OnEnableScrollBarValueChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartAxisBase2D chartAxisBase2D = d as ChartAxisBase2D;
    if (chartAxisBase2D.Area == null || chartAxisBase2D.sfChartResizableBar == null)
      return;
    chartAxisBase2D.sfChartResizableBar.Visibility = (bool) e.NewValue ? Visibility.Visible : Visibility.Collapsed;
    chartAxisBase2D.ChangeStyle(chartAxisBase2D.EnableTouchMode);
    chartAxisBase2D.Area.ScheduleUpdate();
  }

  private static void OnEnableTouchModeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartAxisBase2D chartAxisBase2D = d as ChartAxisBase2D;
    chartAxisBase2D.ChangeStyle((bool) e.NewValue);
    if (!chartAxisBase2D.EnableScrollBar || chartAxisBase2D.Area == null)
      return;
    chartAxisBase2D.Area.ScheduleUpdate();
  }

  private static FrameworkElement GetParent(FrameworkElement element)
  {
    if (element.Tag is ChartAxisLabel)
      return element;
    FrameworkElement parent = VisualTreeHelper.GetParent((DependencyObject) element) as FrameworkElement;
    while (parent != null && !(parent.Tag is ChartAxisLabel))
      parent = VisualTreeHelper.GetParent((DependencyObject) parent) as FrameworkElement;
    return parent ?? (FrameworkElement) null;
  }

  private void DisposeEvents()
  {
    if (this.MultiLevelLabels != null)
    {
      this.MultiLevelLabels.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.MultiLevelLabls_CollectionChanged);
      this.MultiLevelLabels.Clear();
      this.MultiLevelLabels = (ChartMultiLevelLabels) null;
    }
    if (this.StripLines != null)
    {
      this.StripLines.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnStripLinesCollectionChanged);
      this.StripLines.Clear();
    }
    if (this.RegisteredSeries != null)
    {
      this.RegisteredSeries.CollectionChanged -= new NotifyCollectionChangedEventHandler(((ChartAxis) this).OnRegisteredSeriesCollectionChanged);
      this.RegisteredSeries.Clear();
    }
    if (this.ValueToCoefficientCalc != null)
    {
      foreach (Delegate invocation in this.ValueToCoefficientCalc.GetInvocationList())
      {
        ChartAxisBase2D chartAxisBase2D = this;
        chartAxisBase2D.ValueToCoefficientCalc = chartAxisBase2D.ValueToCoefficientCalc - (invocation as ChartAxis.ValueToCoefficientHandler);
      }
      this.ValueToCoefficientCalc = (ChartAxis.ValueToCoefficientHandler) null;
    }
    if (this.CoefficientToValueCalc == null)
      return;
    foreach (Delegate invocation in this.CoefficientToValueCalc.GetInvocationList())
    {
      ChartAxisBase2D chartAxisBase2D = this;
      chartAxisBase2D.CoefficientToValueCalc = chartAxisBase2D.CoefficientToValueCalc - (invocation as ChartAxis.ValueToCoefficientHandler);
    }
    this.CoefficientToValueCalc = (ChartAxis.ValueToCoefficientHandler) null;
  }

  internal void DisposePanels()
  {
    if (this.axisPanel != null)
    {
      if (this.axisPanel.LayoutCalc != null)
        this.axisPanel.LayoutCalc.Clear();
      this.axisPanel.Axis = (ChartAxisBase2D) null;
      this.axisPanel.Children.Clear();
      this.axisPanel = (ChartCartesianAxisPanel) null;
    }
    if (this.labelsPanel != null)
    {
      this.labelsPanel.Children.Clear();
      this.labelsPanel = (Panel) null;
    }
    if (this.multiLevelLabelsPanel != null)
    {
      this.multiLevelLabelsPanel.Children.Clear();
      this.multiLevelLabelsPanel = (Panel) null;
    }
    if (this.elementsPanel != null)
    {
      this.elementsPanel.Children.Clear();
      this.elementsPanel = (Panel) null;
    }
    if (this.axisLabelsPanel != null)
    {
      if (this.axisLabelsPanel is ChartCircularAxisPanel axisLabelsPanel1)
        axisLabelsPanel1.Axis = (ChartAxis) null;
      if (this.axisLabelsPanel is ChartCartesianAxisLabelsPanel axisLabelsPanel2)
      {
        if (axisLabelsPanel2.children != null)
          axisLabelsPanel2.children.Clear();
        axisLabelsPanel2.Dispose();
      }
      this.axisLabelsPanel = (ILayoutCalculator) null;
    }
    if (this.axisElementsPanel is ChartCartesianAxisElementsPanel axisElementsPanel)
    {
      if (this.axisElementsPanel.Children != null)
        this.axisElementsPanel.Children.Clear();
      axisElementsPanel.Dispose();
      this.axisElementsPanel = (ILayoutCalculator) null;
    }
    this.axisMultiLevelLabelsPanel?.Dispose();
    if (this.GridLinesRecycler != null)
    {
      foreach (Shape element in this.GridLinesRecycler)
        element.ClearUIValues();
      this.GridLinesRecycler.Clear();
      this.GridLinesRecycler = (UIElementsRecycler<Line>) null;
    }
    if (this.MinorGridLinesRecycler == null)
      return;
    foreach (Shape element in this.MinorGridLinesRecycler)
      element.ClearUIValues();
    this.MinorGridLinesRecycler.Clear();
    this.MinorGridLinesRecycler = (UIElementsRecycler<Line>) null;
  }

  private void OnLabelBorderWidthChanged()
  {
    if (this.axisLabelsPanel == null)
      return;
    this.axisLabelsPanel.UpdateElements();
  }

  private void EnableZoomingToolBarState()
  {
    if (this.ZoomPosition > 0.0 || this.ZoomFactor < 1.0)
      (this.Area as SfChart).ChangeToolBarState();
    else
      (this.Area as SfChart).ResetToolBarState();
  }

  private void OnZoomDataChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  private void OnStripLinesChanged(ChartStripLines newValue, ChartStripLines oldValue)
  {
    if (newValue != null)
    {
      newValue.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnStripLinesCollectionChanged);
      foreach (ChartStripLine element in (Collection<ChartStripLine>) newValue)
      {
        element.PropertyChanged += new PropertyChangedEventHandler(this.OnStripLinesPropertyChanged);
        if (element.Parent is Panel)
          (element.Parent as Panel).Children.Remove((UIElement) element);
        if (this.axisPanel != null && !this.axisPanel.Children.Contains((UIElement) element))
          this.axisPanel.Children.Add((UIElement) element);
      }
    }
    if (oldValue != null)
    {
      oldValue.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnStripLinesCollectionChanged);
      foreach (ChartStripLine element in (Collection<ChartStripLine>) oldValue)
      {
        element.PropertyChanged -= new PropertyChangedEventHandler(this.OnStripLinesPropertyChanged);
        if (this.axisPanel != null && this.axisPanel.Children.Contains((UIElement) element))
          this.axisPanel.Children.Remove((UIElement) element);
      }
    }
    this.ClearStripLines();
    this.UpdateStripsLines();
  }

  private void OnMultiLevelLabelsCollectionChanged(
    ChartMultiLevelLabels oldValues,
    ChartMultiLevelLabels newValues)
  {
    if (oldValues != null)
    {
      oldValues.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.MultiLevelLabls_CollectionChanged);
      if (oldValues.Count > 0 && this.axisMultiLevelLabelsPanel != null)
        this.axisMultiLevelLabelsPanel.DetachElements();
    }
    if (newValues != null)
      newValues.CollectionChanged += new NotifyCollectionChangedEventHandler(this.MultiLevelLabls_CollectionChanged);
    if (this.Area == null)
      return;
    this.OnPropertyChanged();
  }

  private void MultiLevelLabls_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == NotifyCollectionChangedAction.Add)
    {
      if (e.NewItems != null)
      {
        foreach (ChartMultiLevelLabel newItem in (IEnumerable) e.NewItems)
        {
          if (newItem != null)
            newItem.PropertyChanged += new PropertyChangedEventHandler(this.Item_PropertyChanged);
        }
      }
    }
    else if (e.Action == NotifyCollectionChangedAction.Remove)
    {
      if (e.OldItems != null)
      {
        foreach (ChartMultiLevelLabel oldItem in (IEnumerable) e.OldItems)
        {
          if (oldItem != null)
            oldItem.PropertyChanged -= new PropertyChangedEventHandler(this.Item_PropertyChanged);
        }
      }
    }
    else if (e.Action == NotifyCollectionChangedAction.Replace)
    {
      if (e.OldItems != null)
      {
        foreach (ChartMultiLevelLabel oldItem in (IEnumerable) e.OldItems)
        {
          if (oldItem != null)
            oldItem.PropertyChanged -= new PropertyChangedEventHandler(this.Item_PropertyChanged);
        }
      }
      if (e.NewItems != null)
      {
        foreach (ChartMultiLevelLabel newItem in (IEnumerable) e.NewItems)
        {
          if (newItem != null)
            newItem.PropertyChanged += new PropertyChangedEventHandler(this.Item_PropertyChanged);
        }
      }
    }
    else if (e.Action == NotifyCollectionChangedAction.Reset)
    {
      if (this.axisMultiLevelLabelsPanel != null)
        this.axisMultiLevelLabelsPanel.DetachElements();
      this.axisMultiLevelLabelsPanel = new MultiLevelLabelsPanel(this.multiLevelLabelsPanel)
      {
        Axis = this
      };
    }
    if (this.Area == null)
      return;
    this.OnPropertyChanged();
  }

  private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (this.Area == null)
      return;
    this.OnPropertyChanged();
  }

  private void OnScrollBarResizableChanged()
  {
    if (this.sfChartResizableBar == null)
      return;
    this.sfChartResizableBar.UpdateResizable(this.EnableScrollBarResizing);
  }

  private void SetBinding()
  {
    Binding binding1 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Visibility", new object[0])
    };
    BindingOperations.SetBinding((DependencyObject) this, ChartAxis.AxisVisibilityProperty, (BindingBase) binding1);
    Binding binding2 = new Binding()
    {
      Path = new PropertyPath("Style", new object[0]),
      Source = (object) this
    };
    BindingOperations.SetBinding((DependencyObject) this, ChartAxisBase2D.AxisStyleProperty, (BindingBase) binding2);
  }

  private void OnStripLinesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == NotifyCollectionChangedAction.Add)
    {
      foreach (ChartStripLine newItem in (IEnumerable) e.NewItems)
      {
        newItem.PropertyChanged += new PropertyChangedEventHandler(this.OnStripLinesPropertyChanged);
        if (this.axisPanel != null && !this.axisPanel.Children.Contains((UIElement) newItem))
          this.axisPanel.Children.Add((UIElement) newItem);
      }
      if (this.Area == null)
        return;
      this.UpdateStripsLines();
    }
    else if (e.Action == NotifyCollectionChangedAction.Remove)
    {
      foreach (ChartStripLine oldItem in (IEnumerable) e.OldItems)
      {
        oldItem.PropertyChanged -= new PropertyChangedEventHandler(this.OnStripLinesPropertyChanged);
        if (this.axisPanel != null && this.axisPanel.Children.Contains((UIElement) oldItem))
          this.axisPanel.Children.Remove((UIElement) oldItem);
      }
      this.ClearStripLines();
      this.UpdateStripsLines();
    }
    else if (e.Action == NotifyCollectionChangedAction.Reset)
    {
      if (this.axisPanel == null)
        return;
      UIElement[] uiElementArray = new UIElement[this.axisPanel.Children.Count];
      this.axisPanel.Children.CopyTo(uiElementArray, 0);
      foreach (ChartStripLine element in ((IEnumerable<UIElement>) uiElementArray).Where<UIElement>((Func<UIElement, bool>) (it => it is ChartStripLine)))
      {
        if (this.axisPanel.Children.Contains((UIElement) element))
        {
          element.PropertyChanged -= new PropertyChangedEventHandler(this.OnStripLinesPropertyChanged);
          this.axisPanel.Children.Remove((UIElement) element);
        }
      }
      this.ClearStripLines();
      this.UpdateStripsLines();
    }
    else
    {
      if (e.Action != NotifyCollectionChangedAction.Replace)
        return;
      if (e.OldItems[0] is ChartStripLine oldItem && this.axisPanel != null && this.axisPanel.Children.Contains((UIElement) oldItem))
      {
        oldItem.PropertyChanged -= new PropertyChangedEventHandler(this.OnStripLinesPropertyChanged);
        this.axisPanel.Children.Remove((UIElement) oldItem);
      }
      if (e.NewItems[0] is ChartStripLine newItem && this.axisPanel != null && !this.axisPanel.Children.Contains((UIElement) newItem))
      {
        newItem.PropertyChanged += new PropertyChangedEventHandler(this.OnStripLinesPropertyChanged);
        this.axisPanel.Children.Add((UIElement) newItem);
      }
      this.ClearStripLines();
      this.UpdateStripsLines();
    }
  }

  private void ClearStripLines()
  {
    if (this.Area == null)
      return;
    (this.Area as SfChart).ClearStripLines();
  }

  private void UpdateStripsLines()
  {
    if (this.Area != null)
      (this.Area as SfChart).UpdateStripLines();
    this.isUpdateStripDispatched = false;
  }

  private void ClearItems()
  {
    if (this.sfChartResizableBar != null)
      this.sfChartResizableBar = (SfChartResizableBar) null;
    if (this.GridLinesRecycler != null)
    {
      this.GridLinesRecycler.Clear();
      this.GridLinesRecycler = (UIElementsRecycler<Line>) null;
    }
    if (this.MinorGridLinesRecycler != null)
    {
      this.MinorGridLinesRecycler.Clear();
      this.MinorGridLinesRecycler = (UIElementsRecycler<Line>) null;
    }
    if (this.labelsPanel != null)
    {
      this.labelsPanel.Children.Clear();
      this.labelsPanel = (Panel) null;
    }
    if (this.elementsPanel != null)
    {
      this.elementsPanel.Children.Clear();
      this.elementsPanel = (Panel) null;
    }
    if (this.axisPanel != null)
    {
      this.axisPanel.Children.Clear();
      this.axisPanel = (ChartCartesianAxisPanel) null;
    }
    if (this.axisMultiLevelLabelsPanel != null)
      this.axisMultiLevelLabelsPanel.DetachElements();
    if (this.axisLabelsPanel != null)
    {
      this.axisLabelsPanel.Children.Clear();
      this.axisLabelsPanel = (ILayoutCalculator) null;
    }
    if (this.axisElementsPanel != null)
    {
      this.axisElementsPanel.Children.Clear();
      this.axisElementsPanel = (ILayoutCalculator) null;
    }
    if (this.headerContent == null)
      return;
    this.headerContent = (ContentControl) null;
  }

  private void OnStripLinesPropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (this.isUpdateStripDispatched)
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) new Action(this.UpdateStripsLines));
    this.isUpdateStripDispatched = true;
  }

  private void UpdatePanels()
  {
    if (this.axisPanel == null)
      return;
    if (this.AxisLayoutPanel is ChartPolarAxisLayoutPanel && !(this.axisLabelsPanel is ChartCircularAxisPanel))
    {
      if (this.axisLabelsPanel != null)
        this.axisLabelsPanel.DetachElements();
      if (this.axisElementsPanel != null)
        this.axisElementsPanel.DetachElements();
      this.axisPanel.LayoutCalc.Clear();
      this.axisLabelsPanel = (ILayoutCalculator) new ChartCircularAxisPanel(this.labelsPanel)
      {
        Axis = (ChartAxis) this
      };
      this.axisPanel.LayoutCalc.Add(this.axisLabelsPanel);
    }
    else if (!(this.AxisLayoutPanel is ChartPolarAxisLayoutPanel) && !(this.axisLabelsPanel is ChartCartesianAxisLabelsPanel))
    {
      if (this.axisLabelsPanel != null)
        this.axisLabelsPanel.DetachElements();
      if (this.axisElementsPanel != null)
        this.axisElementsPanel.DetachElements();
      this.axisPanel.LayoutCalc.Clear();
      if (this.axisMultiLevelLabelsPanel != null)
        this.axisMultiLevelLabelsPanel.DetachElements();
      if (this.labelsPanel != null)
      {
        this.axisLabelsPanel = (ILayoutCalculator) new ChartCartesianAxisLabelsPanel(this.labelsPanel)
        {
          Axis = (ChartAxis) this
        };
        this.axisPanel.LayoutCalc.Add(this.axisLabelsPanel);
      }
      if (this.elementsPanel != null)
      {
        this.axisElementsPanel = (ILayoutCalculator) new ChartCartesianAxisElementsPanel(this.elementsPanel)
        {
          Axis = (ChartAxis) this
        };
        this.axisPanel.LayoutCalc.Add(this.axisElementsPanel);
      }
      if (this.MultiLevelLabels == null || this.MultiLevelLabels.Count <= 0 || this.multiLevelLabelsPanel == null)
        return;
      this.axisMultiLevelLabelsPanel = new MultiLevelLabelsPanel(this.multiLevelLabelsPanel)
      {
        Axis = this
      };
    }
    else
    {
      if (this.MultiLevelLabels == null || this.MultiLevelLabels.Count <= 0 || this.multiLevelLabelsPanel == null || !(this.axisLabelsPanel is ChartCartesianAxisLabelsPanel) || (this.axisMultiLevelLabelsPanel == null || this.axisMultiLevelLabelsPanel.Panel != null) && this.axisMultiLevelLabelsPanel != null)
        return;
      this.axisMultiLevelLabelsPanel = new MultiLevelLabelsPanel(this.multiLevelLabelsPanel)
      {
        Axis = this
      };
    }
  }
}
