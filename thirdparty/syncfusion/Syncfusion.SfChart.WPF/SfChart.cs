// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SfChart
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
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

[ContentProperty("Series")]
public class SfChart : ChartBase, IDisposable
{
  public static readonly DependencyProperty PrimaryAxisProperty = DependencyProperty.Register(nameof (PrimaryAxis), typeof (ChartAxisBase2D), typeof (SfChart), new PropertyMetadata((object) null, new PropertyChangedCallback(SfChart.OnPrimaryAxisChanged)));
  public static readonly DependencyProperty SecondaryAxisProperty = DependencyProperty.Register(nameof (SecondaryAxis), typeof (RangeAxisBase), typeof (SfChart), new PropertyMetadata((object) null, new PropertyChangedCallback(SfChart.OnSecondaryAxisChanged)));
  public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(nameof (Watermark), typeof (Watermark), typeof (SfChart), new PropertyMetadata((object) null, new PropertyChangedCallback(SfChart.OnWatermarkChanged)));
  public static readonly DependencyProperty AreaBorderBrushProperty = DependencyProperty.Register(nameof (AreaBorderBrush), typeof (Brush), typeof (SfChart), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty AreaBorderThicknessProperty = DependencyProperty.Register(nameof (AreaBorderThickness), typeof (Thickness), typeof (SfChart), new PropertyMetadata((object) new Thickness().GetThickness(0.0, 0.0, 0.0, 0.0)));
  public static readonly DependencyProperty AreaBackgroundProperty = DependencyProperty.Register(nameof (AreaBackground), typeof (Brush), typeof (SfChart), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(nameof (Series), typeof (ChartSeriesCollection), typeof (SfChart), new PropertyMetadata((object) null, new PropertyChangedCallback(SfChart.OnSeriesPropertyCollectionChanged)));
  public static readonly DependencyProperty AnnotationsProperty = DependencyProperty.Register(nameof (Annotations), typeof (AnnotationCollection), typeof (SfChart), new PropertyMetadata((object) null, new PropertyChangedCallback(SfChart.OnAnnotationsChanged)));
  public static readonly DependencyProperty TechnicalIndicatorsProperty = DependencyProperty.Register(nameof (TechnicalIndicators), typeof (ObservableCollection<ChartSeries>), typeof (SfChart), new PropertyMetadata((object) null, new PropertyChangedCallback(SfChart.OnTechnicalIndicatorsPropertyChanged)));
  internal bool isRenderSeriesDispatched;
  internal Panel chartAxisPanel;
  internal int currentBitmapPixel = -1;
  internal Point adorningCanvasPoint;
  internal bool isBitmapPixelsConverted;
  internal bool HoldUpdate;
  internal ZoomingToolBar zoomingToolBar;
  internal ChartZoomPanBehavior chartZoomBehavior;
  internal WriteableBitmap fastRenderSurface;
  private Panel gridLinesPanel;
  private bool clearPixels;
  private Panel seriesPresenter;
  private ChartRootPanel rootPanel;
  private List<double> sumItems = new List<double>();
  private Dictionary<string, double> isIndexedFalseSumValues = new Dictionary<string, double>();
  private ChartSeries previousSeries;
  private IList rowBorderLines;
  private IList columnBorderLines;
  private ChartBehaviorsCollection behaviors;
  private Panel internalCanvas;
  private Image fastRenderDevice = new Image();
  private byte[] fastBuffer;
  private bool disposed;

  public SfChart()
  {
    if (!(bool) DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof (DependencyObject)).DefaultValue)
      LicenseHelper.ValidateLicense();
    this.DefaultStyleKey = (object) typeof (SfChart);
    this.UpdateAction = UpdateAction.Invalidate;
    this.VisibleSeries = new ChartVisibleSeriesCollection();
    this.Axes = new ChartAxisCollection();
    this.DependentSeriesAxes = new List<ChartAxis>();
    this.Printing = new Printing((ChartBase) this);
    this.Axes.CollectionChanged += new NotifyCollectionChangedEventHandler(this.Axes_CollectionChanged);
    this.Behaviors = new ChartBehaviorsCollection(this);
    if (this.ColorModel != null)
      this.ColorModel.Palette = this.Palette;
    this.InitializeLegendItems();
  }

  private void Behaviors_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    switch (e.Action)
    {
      case NotifyCollectionChangedAction.Add:
        this.AddChartBehavior((ChartBehavior) e.NewItems[0]);
        break;
      case NotifyCollectionChangedAction.Remove:
      case NotifyCollectionChangedAction.Replace:
        ChartBehavior oldItem = (ChartBehavior) e.OldItems[0];
        if (e.Action != NotifyCollectionChangedAction.Replace)
          break;
        this.AddChartBehavior((ChartBehavior) e.NewItems[0]);
        break;
    }
  }

  private void Axes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.NewItems != null && e.NewItems.Count > 0 && e.NewItems[0] is ChartAxisBase2D newItem && newItem.Area == null)
      newItem.Area = (ChartBase) this;
    if (e.OldItems != null && e.OldItems.Count > 0 && e.OldItems[0] is ChartAxisBase2D oldItem)
      oldItem.Area = (ChartBase) null;
    this.ScheduleUpdate();
  }

  public event EventHandler<ZoomChangedEventArgs> ZoomChanged;

  public event EventHandler<ZoomChangingEventArgs> ZoomChanging;

  public event EventHandler<SelectionZoomingStartEventArgs> SelectionZoomingStart;

  public event EventHandler<SelectionZoomingDeltaEventArgs> SelectionZoomingDelta;

  public event EventHandler<SelectionZoomingEndEventArgs> SelectionZoomingEnd;

  public event EventHandler<PanChangedEventArgs> PanChanged;

  public event EventHandler<PanChangingEventArgs> PanChanging;

  public event EventHandler<ResetZoomEventArgs> ResetZooming;

  public ChartAxisBase2D PrimaryAxis
  {
    get => (ChartAxisBase2D) this.GetValue(SfChart.PrimaryAxisProperty);
    set => this.SetValue(SfChart.PrimaryAxisProperty, (object) value);
  }

  public RangeAxisBase SecondaryAxis
  {
    get => (RangeAxisBase) this.GetValue(SfChart.SecondaryAxisProperty);
    set => this.SetValue(SfChart.SecondaryAxisProperty, (object) value);
  }

  public Watermark Watermark
  {
    get => (Watermark) this.GetValue(SfChart.WatermarkProperty);
    set => this.SetValue(SfChart.WatermarkProperty, (object) value);
  }

  public Brush AreaBorderBrush
  {
    get => (Brush) this.GetValue(SfChart.AreaBorderBrushProperty);
    set => this.SetValue(SfChart.AreaBorderBrushProperty, (object) value);
  }

  public Thickness AreaBorderThickness
  {
    get => (Thickness) this.GetValue(SfChart.AreaBorderThicknessProperty);
    set => this.SetValue(SfChart.AreaBorderThicknessProperty, (object) value);
  }

  public Brush AreaBackground
  {
    get => (Brush) this.GetValue(SfChart.AreaBackgroundProperty);
    set => this.SetValue(SfChart.AreaBackgroundProperty, (object) value);
  }

  public ChartBehaviorsCollection Behaviors
  {
    get => this.behaviors;
    set
    {
      this.OnBehaviorPropertyChanged(this.behaviors, value);
      this.behaviors = value;
    }
  }

  public ChartSeriesCollection Series
  {
    get => (ChartSeriesCollection) this.GetValue(SfChart.SeriesProperty);
    set => this.SetValue(SfChart.SeriesProperty, (object) value);
  }

  public ObservableCollection<ChartSeries> TechnicalIndicators
  {
    get => (ObservableCollection<ChartSeries>) this.GetValue(SfChart.TechnicalIndicatorsProperty);
    set => this.SetValue(SfChart.TechnicalIndicatorsProperty, (object) value);
  }

  public AnnotationCollection Annotations
  {
    get => (AnnotationCollection) this.GetValue(SfChart.AnnotationsProperty);
    set => this.SetValue(SfChart.AnnotationsProperty, (object) value);
  }

  internal bool IsMultipleArea => this.RowDefinitions.Count > 1 || this.ColumnDefinitions.Count > 1;

  internal Panel InternalCanvas
  {
    get => this.internalCanvas;
    set => this.internalCanvas = value;
  }

  internal Panel GridLinesPanel => this.gridLinesPanel;

  internal bool CanRenderToBuffer { get; set; }

  internal Canvas ChartAnnotationCanvas { get; set; }

  internal Canvas SeriesAnnotationCanvas { get; set; }

  internal AnnotationManager AnnotationManager { get; set; }

  internal Canvas BottomAdorningCanvas { get; set; }

  private IList RowBorderLines
  {
    get
    {
      if (this.rowBorderLines == null)
        this.rowBorderLines = (IList) new List<Line>();
      return this.rowBorderLines;
    }
    set => this.rowBorderLines = value;
  }

  private IList ColumnBorderLines
  {
    get
    {
      if (this.columnBorderLines == null)
        this.columnBorderLines = (IList) new List<Line>();
      return this.columnBorderLines;
    }
    set => this.columnBorderLines = value;
  }

  private bool HasBitmapSeries
  {
    get
    {
      if (this.VisibleSeries.Any<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (ser => ser.IsBitmapSeries)))
        return true;
      return this.TechnicalIndicators != null && this.TechnicalIndicators.Any<ChartSeries>((Func<ChartSeries, bool>) (indicator => indicator is MACDTechnicalIndicator && indicator.Visibility == Visibility.Visible && (indicator as MACDTechnicalIndicator).Type != MACDType.Line));
    }
  }

  public void Dispose()
  {
    if (this.disposed)
      return;
    this.disposed = true;
    this.Dispose(true);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    this.DisposeAnnotation();
    if (this.previousSeries != null)
    {
      this.previousSeries.Dispose();
      this.previousSeries = (ChartSeries) null;
    }
    this.DisposeSeriesAndIndicators();
    this.DisposeBehaviors();
    this.DisposeZoomEvents();
    this.DisposeSelectionEvents();
    this.DisposeLegend();
    this.DisposeAxis();
    if (this.ColorModel != null)
      this.ColorModel.Dispose();
    this.SizeChanged -= new SizeChangedEventHandler(this.OnSfChartSizeChanged);
    this.SizeChanged -= new SizeChangedEventHandler(this.OnSizeChanged);
    this.DisposeRowColumnsDefinitions();
    this.DisposePanels();
    this.Watermark = (Watermark) null;
    this.RootPanelDesiredSize = new Size?();
    this.GridLinesLayout = (ILayoutCalculator) null;
    this.ChartAxisLayoutPanel = (ILayoutCalculator) null;
    this.SelectionBehaviour = (ChartSelectionBehavior) null;
    this.TooltipBehavior = (ChartTooltipBehavior) null;
    if (this.Printing == null)
      return;
    this.Printing.Chart = (ChartBase) null;
    this.Printing = (Printing) null;
  }

  private void DisposePanels()
  {
    if (this.rootPanel != null)
    {
      this.rootPanel.Area = (ChartBase) null;
      this.rootPanel = (ChartRootPanel) null;
    }
    if (this.ChartAxisLayoutPanel is ChartCartesianAxisLayoutPanel chartAxisLayoutPanel2)
    {
      chartAxisLayoutPanel2.Area = (ChartBase) null;
      chartAxisLayoutPanel2.Children.Clear();
    }
    else if (this.ChartAxisLayoutPanel is ChartPolarAxisLayoutPanel chartAxisLayoutPanel1)
    {
      chartAxisLayoutPanel1.Area = (SfChart) null;
      chartAxisLayoutPanel1.PolarAxis = (ChartAxisBase2D) null;
      chartAxisLayoutPanel1.CartesianAxis = (ChartAxisBase2D) null;
      chartAxisLayoutPanel1.Children.Clear();
    }
    if (this.GridLinesLayout is ChartCartesianGridLinesPanel gridLinesLayout2)
    {
      gridLinesLayout2.Area = (ChartBase) null;
      gridLinesLayout2.Children.Clear();
    }
    else if (this.GridLinesLayout is ChartPolarGridLinesPanel gridLinesLayout1)
      gridLinesLayout1.Dispose();
    if (this.ChartDockPanel == null || this.ChartDockPanel.Children.Count <= 0)
      return;
    this.ChartDockPanel.RootElement = (UIElement) null;
    this.ChartDockPanel.Children.Clear();
    this.ChartDockPanel = (ChartDockPanel) null;
  }

  private void DisposeAxis()
  {
    if (this.Axes != null)
    {
      foreach (ChartAxis ax in (Collection<ChartAxis>) this.Axes)
        ax.Dispose();
      this.Axes.Clear();
      this.Axes.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.Axes_CollectionChanged);
      this.Axes = (ChartAxisCollection) null;
    }
    this.PrimaryAxis = (ChartAxisBase2D) null;
    this.SecondaryAxis = (RangeAxisBase) null;
    this.InternalPrimaryAxis = (ChartAxis) null;
    this.InternalSecondaryAxis = (ChartAxis) null;
    this.InternalDepthAxis = (ChartAxis) null;
  }

  private void DisposeSeriesAndIndicators()
  {
    if (this.VisibleSeries != null)
      this.VisibleSeries.Clear();
    if (this.ActualSeries != null)
      this.ActualSeries.Clear();
    if (this.SelectedSeriesCollection != null)
      this.SelectedSeriesCollection.Clear();
    this.CurrentSelectedSeries = (ChartSeriesBase) null;
    this.PreviousSelectedSeries = (ChartSeriesBase) null;
    ChartSeriesCollection series = this.Series;
    ObservableCollection<ChartSeries> technicalIndicators = this.TechnicalIndicators;
    if (this.Series != null)
      this.Series.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnSeriesCollectionChanged);
    if (this.TechnicalIndicators != null)
      this.TechnicalIndicators.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnTechnicalIndicatorsCollectionChanged);
    this.Series = (ChartSeriesCollection) null;
    this.TechnicalIndicators = (ObservableCollection<ChartSeries>) null;
    if (series != null)
    {
      foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeries>) series)
        chartSeriesBase.Dispose();
    }
    if (technicalIndicators == null)
      return;
    foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeries>) technicalIndicators)
      chartSeriesBase.Dispose();
  }

  private void DisposeBehaviors()
  {
    if (this.behaviors != null)
    {
      foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.behaviors)
      {
        if (behavior is ChartZoomPanBehavior chartZoomPanBehavior)
          chartZoomPanBehavior.DisposeZoomEventArguments();
        behavior.Dispose();
      }
      this.behaviors.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.Behaviors_CollectionChanged);
      this.behaviors.Area = (SfChart) null;
    }
    if (this.zoomingToolBar != null)
    {
      this.zoomingToolBar.Dispose();
      this.zoomingToolBar = (ZoomingToolBar) null;
    }
    this.chartZoomBehavior = (ChartZoomPanBehavior) null;
    this.SelectionBehaviour = (ChartSelectionBehavior) null;
    this.TooltipBehavior = (ChartTooltipBehavior) null;
  }

  private void DisposeLegend()
  {
    if (this.Legend == null)
      return;
    if (this.LegendItems != null)
    {
      foreach (ObservableCollection<LegendItem> legendItem1 in (Collection<ObservableCollection<LegendItem>>) this.LegendItems)
      {
        foreach (LegendItem legendItem2 in (Collection<LegendItem>) legendItem1)
          legendItem2.Dispose();
        legendItem1.Clear();
      }
      this.LegendItems.Clear();
    }
    if (this.Legend is ChartLegend legend1)
    {
      legend1.Dispose();
    }
    else
    {
      if (!(this.Legend is ChartLegendCollection legend))
        return;
      foreach (ChartLegend chartLegend in (Collection<ChartLegend>) legend)
        chartLegend.Dispose();
      legend.CollectionChanged -= new NotifyCollectionChangedEventHandler(((ChartBase) this).LegendCollectionChanged);
      legend.Clear();
    }
  }

  private void DisposeZoomEvents()
  {
    if (this.ZoomChanged != null)
    {
      foreach (Delegate invocation in this.ZoomChanged.GetInvocationList())
        this.ZoomChanged -= invocation as EventHandler<ZoomChangedEventArgs>;
      this.ZoomChanged = (EventHandler<ZoomChangedEventArgs>) null;
    }
    if (this.ZoomChanging != null)
    {
      foreach (Delegate invocation in this.ZoomChanging.GetInvocationList())
        this.ZoomChanging -= invocation as EventHandler<ZoomChangingEventArgs>;
      this.ZoomChanging = (EventHandler<ZoomChangingEventArgs>) null;
    }
    if (this.SelectionZoomingStart != null)
    {
      foreach (Delegate invocation in this.SelectionZoomingStart.GetInvocationList())
        this.SelectionZoomingStart -= invocation as EventHandler<SelectionZoomingStartEventArgs>;
      this.SelectionZoomingStart = (EventHandler<SelectionZoomingStartEventArgs>) null;
    }
    if (this.SelectionZoomingDelta != null)
    {
      foreach (Delegate invocation in this.SelectionZoomingDelta.GetInvocationList())
        this.SelectionZoomingDelta -= invocation as EventHandler<SelectionZoomingDeltaEventArgs>;
      this.SelectionZoomingDelta = (EventHandler<SelectionZoomingDeltaEventArgs>) null;
    }
    if (this.SelectionZoomingEnd != null)
    {
      foreach (Delegate invocation in this.SelectionZoomingEnd.GetInvocationList())
        this.SelectionZoomingEnd -= invocation as EventHandler<SelectionZoomingEndEventArgs>;
      this.SelectionZoomingEnd = (EventHandler<SelectionZoomingEndEventArgs>) null;
    }
    if (this.PanChanged != null)
    {
      foreach (Delegate invocation in this.PanChanged.GetInvocationList())
        this.PanChanged -= invocation as EventHandler<PanChangedEventArgs>;
      this.PanChanged = (EventHandler<PanChangedEventArgs>) null;
    }
    if (this.PanChanging != null)
    {
      foreach (Delegate invocation in this.PanChanging.GetInvocationList())
        this.PanChanging -= invocation as EventHandler<PanChangingEventArgs>;
      this.PanChanging = (EventHandler<PanChangingEventArgs>) null;
    }
    if (this.ResetZooming == null)
      return;
    foreach (Delegate invocation in this.ResetZooming.GetInvocationList())
      this.ResetZooming -= invocation as EventHandler<ResetZoomEventArgs>;
    this.ResetZooming = (EventHandler<ResetZoomEventArgs>) null;
  }

  private void DisposeAnnotation()
  {
    if (this.Annotations != null)
    {
      foreach (Annotation annotation in (Collection<Annotation>) this.Annotations)
        annotation.Dispose();
      this.Annotations.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnAnnotationsCollectionChanged);
      this.Annotations.Clear();
      this.Annotations = (AnnotationCollection) null;
    }
    if (this.AnnotationManager == null)
      return;
    this.AnnotationManager.Dispose();
    this.AnnotationManager = (AnnotationManager) null;
  }

  public override void SeriesSelectedIndexChanged(int newIndex, int oldIndex)
  {
    if (oldIndex < this.Series.Count && oldIndex >= 0 && this.SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
    {
      ChartSeries series = this.Series[oldIndex];
      if (this.SelectedSeriesCollection.Contains((ChartSeriesBase) series))
        this.SelectedSeriesCollection.Remove((ChartSeriesBase) series);
      this.OnResetSeries(series);
    }
    if (newIndex >= 0 && this.GetEnableSeriesSelection() && newIndex < this.VisibleSeries.Count)
    {
      ChartSeries series = this.Series[newIndex];
      if (!this.SelectedSeriesCollection.Contains((ChartSeriesBase) series))
        this.SelectedSeriesCollection.Add((ChartSeriesBase) series);
      if (series.adornmentInfo is ChartAdornmentInfo && series.adornmentInfo.HighlightOnSelection)
      {
        List<int> list = series.Adornments.Select<ChartAdornment, int>((Func<ChartAdornment, int>) (adorment => series.Adornments.IndexOf(adorment))).ToList<int>();
        series.AdornmentPresenter.UpdateAdornmentSelection(list, false);
      }
      foreach (ChartSegment segment in (Collection<ChartSegment>) series.Segments)
      {
        segment.BindProperties();
        segment.IsSelectedSegment = true;
      }
      if (series.IsBitmapSeries)
        this.UpdateBitmapSeries(series, false);
      ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
      {
        SelectedSegment = (ChartSegment) null,
        SelectedSeries = (ChartSeriesBase) series,
        SelectedSeriesCollection = this.SelectedSeriesCollection,
        SelectedIndex = newIndex,
        PreviousSelectedIndex = oldIndex,
        IsDataPointSelection = false,
        IsSelected = true,
        PreviousSelectedSegment = (ChartSegment) null,
        PreviousSelectedSeries = (ChartSeriesBase) null
      };
      if (oldIndex != -1)
        eventArgs.PreviousSelectedSeries = (ChartSeriesBase) this.Series[oldIndex];
      this.OnSelectionChanged(eventArgs);
    }
    else
    {
      if (newIndex != -1)
        return;
      this.OnSelectionChanged(new ChartSelectionChangedEventArgs()
      {
        SelectedSegment = (ChartSegment) null,
        SelectedSeries = (ChartSeriesBase) null,
        SelectedSeriesCollection = this.SelectedSeriesCollection,
        SelectedIndex = newIndex,
        PreviousSelectedIndex = oldIndex,
        IsDataPointSelection = false,
        IsSelected = false,
        PreviousSelectedSegment = (ChartSegment) null,
        PreviousSelectedSeries = (ChartSeriesBase) this.Series[oldIndex]
      });
    }
  }

  public override double ValueToPoint(ChartAxis axis, double value)
  {
    if (axis == null)
      return double.NaN;
    return axis.Orientation == Orientation.Horizontal ? axis.RenderedRect.Left - axis.Area.SeriesClipRect.Left + axis.ValueToCoefficientCalc(value) * axis.RenderedRect.Width : axis.RenderedRect.Top - axis.Area.SeriesClipRect.Top + (1.0 - axis.ValueToCoefficientCalc(value)) * axis.RenderedRect.Height;
  }

  public double ValueToPointRelativeToAnnotation(ChartAxis axis, double value)
  {
    if (axis == null)
      return double.NaN;
    return axis.Orientation == Orientation.Horizontal ? axis.RenderedRect.Left + axis.ValueToCoefficientCalc(value) * axis.RenderedRect.Width : axis.RenderedRect.Top + (1.0 - axis.ValueToCoefficientCalc(value)) * axis.RenderedRect.Height;
  }

  internal static int ConvertColor(Color color)
  {
    int num = (int) color.A + 1;
    return (int) color.A << 24 | (int) (byte) ((int) color.R * num >> 8) << 16 /*0x10*/ | (int) (byte) ((int) color.G * num >> 8) << 8 | (int) (byte) ((int) color.B * num >> 8);
  }

  internal static double PointToAnnotationValue(ChartAxis axis, Point point)
  {
    if (axis == null)
      return double.NaN;
    return axis.Orientation == Orientation.Horizontal ? axis.CoefficientToValueCalc((point.X - axis.RenderedRect.Left) / axis.RenderedRect.Width) : axis.CoefficientToValueCalc(1.0 - (point.Y - axis.RenderedRect.Top) / axis.RenderedRect.Height);
  }

  internal override void OnRowColumnCollectionChanged(NotifyCollectionChangedEventArgs e)
  {
    if (this.gridLinesPanel == null || this.gridLinesPanel.Children.Count <= 0)
      return;
    if (e.Action == NotifyCollectionChangedAction.Reset)
      this.RemoveBorderLines();
    else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
    {
      if (e.OldItems[0] is ChartRowDefinition)
      {
        this.gridLinesPanel.Children.Remove((UIElement) (this.RowBorderLines[e.OldStartingIndex] as Line));
        this.RowBorderLines.RemoveAt(e.OldStartingIndex);
      }
      else if (e.OldItems[0] is ChartColumnDefinition)
      {
        this.gridLinesPanel.Children.Remove((UIElement) (this.ColumnBorderLines[e.OldStartingIndex] as Line));
        this.ColumnBorderLines.RemoveAt(e.OldStartingIndex);
      }
    }
    base.OnRowColumnCollectionChanged(e);
  }

  internal override DependencyObject CloneChart()
  {
    SfChart copy = new SfChart();
    ChartCloning.CloneControl((Control) this, (Control) copy);
    copy.Height = double.IsNaN(this.Height) ? this.ActualHeight : this.Height;
    copy.Width = double.IsNaN(this.Width) ? this.ActualWidth : this.Width;
    copy.Header = this.Header;
    copy.Palette = this.Palette;
    copy.AxisThickness = this.AxisThickness;
    copy.AreaBorderBrush = this.AreaBorderBrush;
    copy.AreaBackground = this.AreaBackground;
    copy.AreaBorderThickness = this.AreaBorderThickness;
    copy.SideBySideSeriesPlacement = this.SideBySideSeriesPlacement;
    if (this.PrimaryAxis != null && this.PrimaryAxis != null)
      copy.PrimaryAxis = (ChartAxisBase2D) this.PrimaryAxis.Clone();
    if (this.SecondaryAxis != null && this.SecondaryAxis != null)
      copy.SecondaryAxis = (RangeAxisBase) this.SecondaryAxis.Clone();
    if (this.Legend != null)
    {
      if (this.Legend is ChartLegendCollection legend)
      {
        ChartLegendCollection legendCollection = new ChartLegendCollection();
        foreach (ICloneable cloneable in (Collection<ChartLegend>) legend)
        {
          ChartLegend chartLegend = (ChartLegend) cloneable.Clone();
          legendCollection.Add(chartLegend);
        }
        copy.Legend = (object) legendCollection;
      }
      else
        copy.Legend = (object) (ChartLegend) (this.Legend as ICloneable).Clone();
    }
    foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeries>) this.Series)
      copy.Series.Add((ChartSeries) chartSeriesBase.Clone());
    foreach (ChartRowDefinition rowDefinition in (Collection<ChartRowDefinition>) this.RowDefinitions)
      copy.RowDefinitions.Add((ChartRowDefinition) rowDefinition.Clone());
    foreach (ChartColumnDefinition columnDefinition in (Collection<ChartColumnDefinition>) this.ColumnDefinitions)
      copy.ColumnDefinitions.Add((ChartColumnDefinition) columnDefinition.Clone());
    foreach (Annotation annotation in (Collection<Annotation>) this.Annotations)
      copy.Annotations.Add((Annotation) annotation.Clone());
    foreach (FinancialTechnicalIndicator technicalIndicator in (Collection<ChartSeries>) this.TechnicalIndicators)
      copy.TechnicalIndicators.Add((ChartSeries) technicalIndicator.Clone());
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      copy.Behaviors.Add((ChartBehavior) behavior.Clone());
    copy.UpdateArea(true);
    return (DependencyObject) copy;
  }

  internal override void UpdateArea(bool forceUpdate)
  {
    if (!this.isUpdateDispatched && !forceUpdate)
      return;
    if (this.AreaType == ChartAreaType.CartesianAxes)
    {
      if (this.ColumnDefinitions.Count == 0)
        this.ColumnDefinitions.Add(new ChartColumnDefinition());
      if (this.RowDefinitions.Count == 0)
        this.RowDefinitions.Add(new ChartRowDefinition());
    }
    if (this.VisibleSeries == null)
      return;
    if ((this.UpdateAction & UpdateAction.Create) == UpdateAction.Create)
    {
      foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) this.VisibleSeries)
      {
        if (!chartSeriesBase.IsPointGenerated)
          chartSeriesBase.GeneratePoints();
        if (chartSeriesBase.ShowTooltip)
          this.ShowTooltip = true;
      }
      this.InitializeDefaultAxes();
      if (this.AreaType == ChartAreaType.CartesianAxes)
      {
        foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) this.VisibleSeries)
        {
          ISupportAxes2D supportAxes2D = chartSeriesBase as ISupportAxes2D;
          if (supportAxes2D.XAxis == null && this.InternalPrimaryAxis != null && !this.InternalPrimaryAxis.RegisteredSeries.Contains((ISupportAxes) supportAxes2D))
            this.InternalPrimaryAxis.RegisteredSeries.Add((ISupportAxes) supportAxes2D);
          if (supportAxes2D.YAxis == null && this.InternalSecondaryAxis != null && !this.InternalSecondaryAxis.RegisteredSeries.Contains((ISupportAxes) supportAxes2D))
            this.InternalSecondaryAxis.RegisteredSeries.Add((ISupportAxes) supportAxes2D);
        }
      }
      if (this.Series != null && this.Series.Count > 0)
      {
        if (this.InternalPrimaryAxis == null || !this.Axes.Contains(this.InternalPrimaryAxis))
          this.InternalPrimaryAxis = this.Series[0].ActualXAxis;
        if (this.InternalSecondaryAxis == null || !this.Axes.Contains(this.InternalSecondaryAxis))
          this.InternalSecondaryAxis = this.Series[0].ActualYAxis;
      }
      if (this.InternalPrimaryAxis is CategoryAxis && !(this.InternalPrimaryAxis as CategoryAxis).IsIndexed)
        CategoryAxisHelper.GroupData(this.VisibleSeries);
      foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) this.VisibleSeries)
        chartSeriesBase.Invalidate();
      if (this.ShowTooltip && this.Tooltip == null)
        this.Tooltip = new ChartTooltip();
      if (this.TechnicalIndicators != null && this.AreaType == ChartAreaType.CartesianAxes)
      {
        foreach (FinancialTechnicalIndicator technicalIndicator in (Collection<ChartSeries>) this.TechnicalIndicators)
        {
          if (!technicalIndicator.IsPointGenerated)
          {
            if (technicalIndicator.ItemsSource == null && this.VisibleSeries.Count > 0 && this.Series != null && this.Series.Count > 0)
            {
              ChartSeriesBase series = (ChartSeriesBase) (this.Series[technicalIndicator.Name] ?? this.Series[0]);
              technicalIndicator.SetSeriesItemSource(series);
            }
            else
              technicalIndicator.GeneratePoints();
          }
          technicalIndicator.Invalidate();
        }
      }
    }
    if (this.IsUpdateLegend && this.ChartDockPanel != null)
    {
      this.UpdateLegend(this.Legend, false);
      this.IsUpdateLegend = false;
    }
    if ((this.UpdateAction & UpdateAction.UpdateRange) == UpdateAction.UpdateRange)
    {
      foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) this.VisibleSeries)
        chartSeriesBase.UpdateRange();
      if (this.TechnicalIndicators != null)
      {
        foreach (ChartSeriesBase technicalIndicator in (Collection<ChartSeries>) this.TechnicalIndicators)
          technicalIndicator.UpdateRange();
      }
    }
    if (this.RootPanelDesiredSize.HasValue)
    {
      if ((this.UpdateAction & UpdateAction.Layout) == UpdateAction.Layout)
        this.LayoutAxis(this.RootPanelDesiredSize.Value);
      this.UpdateLegendArrangeRect();
      if ((this.UpdateAction & UpdateAction.Render) == UpdateAction.Render)
      {
        if (!this.IsChartLoaded)
        {
          this.ScheduleRenderSeries();
          this.IsChartLoaded = true;
          if (this.SeriesSelectedIndex >= 0 && this.VisibleSeries.Count > 0 && this.GetEnableSeriesSelection())
            this.RaiseSeriesSelectionChangedEvent();
        }
        else if (!this.isRenderSeriesDispatched)
          this.RenderSeries();
      }
    }
    this.UpdateAction = UpdateAction.Invalidate;
    this.isUpdateDispatched = false;
    if (this.Behaviors == null)
      return;
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnLayoutUpdated();
  }

  internal override void UpdateAxisLayoutPanels()
  {
    if (this.internalCanvas != null)
      this.internalCanvas.Clip = (Geometry) null;
    this.AxisThickness = new Thickness().GetThickness(0.0, 0.0, 0.0, 0.0);
    if (this.ChartAxisLayoutPanel != null)
      this.ChartAxisLayoutPanel.DetachElements();
    if (this.GridLinesLayout != null)
      this.GridLinesLayout.DetachElements();
    if (this.chartAxisPanel == null)
      return;
    if (this.AreaType == ChartAreaType.PolarAxes)
    {
      this.ChartAxisLayoutPanel = (ILayoutCalculator) new ChartPolarAxisLayoutPanel(this.chartAxisPanel)
      {
        Area = this
      };
      this.ChartAxisLayoutPanel.UpdateElements();
      this.GridLinesLayout = (ILayoutCalculator) new ChartPolarGridLinesPanel(this.gridLinesPanel)
      {
        Area = this
      };
    }
    else if (this.AreaType == ChartAreaType.CartesianAxes)
    {
      this.ChartAxisLayoutPanel = (ILayoutCalculator) new ChartCartesianAxisLayoutPanel(this.chartAxisPanel)
      {
        Area = (ChartBase) this
      };
      this.ChartAxisLayoutPanel.UpdateElements();
      this.GridLinesLayout = (ILayoutCalculator) new ChartCartesianGridLinesPanel(this.gridLinesPanel)
      {
        Area = (ChartBase) this
      };
    }
    else
    {
      this.ChartAxisLayoutPanel = (ILayoutCalculator) null;
      this.GridLinesLayout = (ILayoutCalculator) null;
    }
  }

  internal override double ValueToLogPoint(ChartAxis axis, double value)
  {
    if (axis == null)
      return double.NaN;
    value = axis is LogarithmicAxis logarithmicAxis ? Math.Log(value, logarithmicAxis.LogarithmicBase) : value;
    return this.ValueToPoint(axis, value);
  }

  internal void AnnotationsChanged(DependencyPropertyChangedEventArgs args)
  {
    AnnotationCollection newValue = args.NewValue as AnnotationCollection;
    if (args.OldValue is AnnotationCollection oldValue)
      oldValue.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnAnnotationsCollectionChanged);
    if (newValue != null)
      this.Annotations.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnAnnotationsCollectionChanged);
    if (newValue == null || newValue.Count <= 0)
      return;
    if (this.AnnotationManager != null)
    {
      this.AnnotationManager.Annotations = newValue;
    }
    else
    {
      if (!this.IsTemplateApplied)
        return;
      this.AnnotationManager = new AnnotationManager()
      {
        Chart = this,
        Annotations = newValue
      };
      this.UpdateAnnotationClips();
    }
  }

  internal void OnAnnotationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (this.AnnotationManager != null || !this.IsTemplateApplied)
      return;
    this.AnnotationManager = new AnnotationManager()
    {
      Chart = this,
      Annotations = this.Annotations
    };
    this.UpdateAnnotationClips();
    (sender as AnnotationCollection).CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnAnnotationsCollectionChanged);
  }

  internal void ChangeToolBarState()
  {
    if (this.zoomingToolBar.ItemsSource == null)
      return;
    foreach (ZoomingToolBarItem zoomingToolBarItem in (this.zoomingToolBar.ItemsSource as List<ZoomingToolBarItem>).Where<ZoomingToolBarItem>((Func<ZoomingToolBarItem, bool>) (item => item is ZoomOut || item is ZoomReset)).ToList<ZoomingToolBarItem>())
    {
      zoomingToolBarItem.IsEnabled = true;
      zoomingToolBarItem.IconBackground = zoomingToolBarItem.EnableColor;
    }
  }

  internal void ResetToolBarState()
  {
    foreach (ZoomingToolBarItem zoomingToolBarItem in (IEnumerable) this.zoomingToolBar.Items)
    {
      switch (zoomingToolBarItem)
      {
        case ZoomOut _:
        case ZoomReset _:
          zoomingToolBarItem.IconBackground = zoomingToolBarItem.DisableColor;
          zoomingToolBarItem.IsEnabled = false;
          continue;
        case ZoomPan _:
          zoomingToolBarItem.IconBackground = zoomingToolBarItem.DisableColor;
          continue;
        case SelectionZoom _:
          zoomingToolBarItem.IconBackground = zoomingToolBarItem.EnableColor;
          this.chartZoomBehavior.InternalEnableSelectionZooming = true;
          continue;
        default:
          continue;
      }
    }
  }

  internal void OnResetSeries(ChartSeries series)
  {
    foreach (ChartSegment segment in (Collection<ChartSegment>) series.Segments)
    {
      segment.BindProperties();
      segment.IsSelectedSegment = false;
    }
    if (series.IsBitmapSeries)
      this.UpdateBitmapSeries(series, true);
    if (series.adornmentInfo is ChartAdornmentInfo)
      series.AdornmentPresenter.ResetAdornmentSelection(new int?(), true);
    foreach (int selectedSegmentsIndex in (Collection<int>) series.SelectedSegmentsIndexes)
    {
      if (series is ISegmentSelectable segmentSelectable && selectedSegmentsIndex > -1 && this.GetEnableSegmentSelection())
      {
        if (series.adornmentInfo is ChartAdornmentInfo && series.adornmentInfo.HighlightOnSelection)
          series.UpdateAdornmentSelection(selectedSegmentsIndex);
        if (series.IsBitmapSeries)
        {
          series.dataPoint = series.GetDataPoint(selectedSegmentsIndex);
          if (series.dataPoint != null && segmentSelectable.SegmentSelectionBrush != null)
          {
            if (series.Segments.Count > 0)
              series.GeneratePixels();
            series.OnBitmapSelection(series.selectedSegmentPixels, segmentSelectable.SegmentSelectionBrush, true);
          }
        }
      }
    }
  }

  internal void CreateFastRenderSurface()
  {
    if (this.seriesPresenter == null || !this.seriesPresenter.Children.Contains((UIElement) this.fastRenderDevice) || this.SeriesClipRect.IsEmpty || this.SeriesClipRect.Width < 1.0 || this.SeriesClipRect.Height < 1.0 || this.fastRenderDevice == null)
      return;
    this.fastRenderDevice.MouseMove -= new MouseEventHandler(this.fastRenderDevice_MouseMove);
    this.fastRenderDevice.MouseLeave -= new MouseEventHandler(this.fastRenderDevice_MouseLeave);
    this.fastRenderSurface = new WriteableBitmap((int) this.SeriesClipRect.Width, (int) this.SeriesClipRect.Height, 96.0, 96.0, PixelFormats.Pbgra32, (BitmapPalette) null);
    this.fastRenderDevice.Height = this.SeriesClipRect.Height;
    this.fastRenderDevice.Width = this.SeriesClipRect.Width;
    this.fastRenderDevice.Source = (ImageSource) this.fastRenderSurface;
    this.fastRenderDevice.MouseMove += new MouseEventHandler(this.fastRenderDevice_MouseMove);
    this.fastRenderDevice.MouseLeave += new MouseEventHandler(this.fastRenderDevice_MouseLeave);
  }

  internal void AddOrRemoveBitmap()
  {
    if (this.seriesPresenter != null && this.seriesPresenter.Children.Contains((UIElement) this.fastRenderDevice) && !this.HasBitmapSeries)
    {
      this.seriesPresenter.Children.Remove((UIElement) this.fastRenderDevice);
      this.fastRenderSurface = (WriteableBitmap) null;
      this.fastBuffer = (byte[]) null;
    }
    else
    {
      if (this.seriesPresenter == null || this.seriesPresenter.Children.Contains((UIElement) this.fastRenderDevice) || !this.HasBitmapSeries)
        return;
      this.seriesPresenter.Children.Insert(0, (UIElement) this.fastRenderDevice);
      if (this.fastRenderSurface != null)
        return;
      this.CreateFastRenderSurface();
    }
  }

  internal unsafe void UpdateBitmapSeries(ChartSeries bitmapSeries, bool isReset)
  {
    int seriesIndex = this.Series.IndexOf(bitmapSeries);
    if (!this.isBitmapPixelsConverted)
      this.ConvertBitmapPixels();
    foreach (ChartSeries chartSeries in this.Series.Where<ChartSeries>((Func<ChartSeries, bool>) (series => this.Series.IndexOf(series) > seriesIndex)).ToList<ChartSeries>())
      bitmapSeries.upperSeriesPixels.UnionWith((IEnumerable<int>) chartSeries.Pixels);
    this.fastRenderSurface.Lock();
    int* pixels = this.fastRenderSurface.GetPixels();
    Brush seriesSelectionBrush = this.GetSeriesSelectionBrush((ChartSeriesBase) bitmapSeries);
    if (isReset)
    {
      switch (bitmapSeries)
      {
        case FastLineBitmapSeries _:
        case FastStepLineBitmapSeries _:
        case FastRangeAreaBitmapSeries _:
          Size size = this.AreaType != ChartAreaType.None ? new Size(this.SeriesClipRect.Width, this.SeriesClipRect.Height) : this.RootPanelDesiredSize.Value;
          List<ChartSeriesBase> list = this.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series.IsBitmapSeries)).ToList<ChartSeriesBase>();
          this.clearPixels = true;
          this.ClearBuffer();
          using (List<ChartSeriesBase>.Enumerator enumerator = list.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              ChartSeriesBase current = enumerator.Current;
              Rect rect = ChartLayoutUtils.Subtractthickness(new Rect(new Point(), size), current.Margin);
              IChartTransformer transformer = current.CreateTransformer(new Size(rect.Width, rect.Height), true);
              current.Pixels.Clear();
              current.bitmapPixels.Clear();
              current.bitmapRects.Clear();
              this.isBitmapPixelsConverted = false;
              foreach (ChartSegment segment in (Collection<ChartSegment>) current.Segments)
                segment.Update(transformer);
            }
            break;
          }
        default:
          for (int index = 0; index < bitmapSeries.DataCount; ++index)
          {
            bitmapSeries.dataPoint = bitmapSeries.GetDataPoint(index);
            if (bitmapSeries.dataPoint != null)
            {
              if (bitmapSeries.Segments.Count > 0)
                bitmapSeries.GeneratePixels();
              int num;
              switch (bitmapSeries)
              {
                case FastHiLoOpenCloseBitmapSeries _:
                  num = SfChart.ConvertColor((bitmapSeries.Segments[0] as FastHiLoOpenCloseSegment).GetSegmentBrush(index));
                  break;
                case FastCandleBitmapSeries _:
                  num = SfChart.ConvertColor((bitmapSeries.Segments[0] as FastCandleBitmapSegment).GetSegmentBrush(index));
                  break;
                default:
                  num = SfChart.ConvertColor(((SolidColorBrush) bitmapSeries.GetInteriorColor(index)).Color);
                  break;
              }
              foreach (int selectedSegmentPixel in bitmapSeries.selectedSegmentPixels)
              {
                if (!bitmapSeries.upperSeriesPixels.Contains(selectedSegmentPixel))
                  pixels[selectedSegmentPixel] = num;
              }
              bitmapSeries.selectedSegmentPixels.Clear();
              bitmapSeries.dataPoint = (ChartDataPointInfo) null;
            }
          }
          break;
      }
    }
    else if (seriesSelectionBrush != null)
    {
      int num = SfChart.ConvertColor((seriesSelectionBrush as SolidColorBrush).Color);
      foreach (int pixel in bitmapSeries.Pixels)
      {
        if (!bitmapSeries.upperSeriesPixels.Contains(pixel))
          pixels[pixel] = num;
      }
    }
    this.fastRenderSurface.AddDirtyRect(new Int32Rect(0, 0, this.fastRenderSurface.PixelWidth, this.fastRenderSurface.PixelHeight));
    this.fastRenderSurface.Unlock();
    bitmapSeries.upperSeriesPixels.Clear();
  }

  internal void ClearStripLines()
  {
    if (this.GridLinesLayout == null || !(this.GridLinesLayout is ChartCartesianGridLinesPanel))
      return;
    (this.GridLinesLayout as ChartCartesianGridLinesPanel).ClearStripLines();
  }

  internal void UpdateStripLines()
  {
    if (this.GridLinesLayout != null && this.GridLinesLayout is ChartCartesianGridLinesPanel)
    {
      (this.GridLinesLayout as ChartCartesianGridLinesPanel).UpdateStripLines();
      this.ScheduleUpdate();
    }
    else
    {
      if (this.GridLinesLayout == null || !(this.GridLinesLayout is ChartPolarGridLinesPanel))
        return;
      (this.GridLinesLayout as ChartPolarGridLinesPanel).UpdateStripLines();
      this.ScheduleUpdate();
    }
  }

  internal void InitializeDefaultAxes()
  {
    if (this.PrimaryAxis != null && !this.Axes.Contains((ChartAxis) this.PrimaryAxis))
      this.ClearValue(SfChart.PrimaryAxisProperty);
    if (this.SecondaryAxis != null && !this.Axes.Contains((ChartAxis) this.SecondaryAxis))
      this.ClearValue(SfChart.SecondaryAxisProperty);
    if (this.PrimaryAxis == null || this.PrimaryAxis.IsDefault)
    {
      if ((this.Series == null || this.Series.Count == 0) && (this.TechnicalIndicators == null || this.TechnicalIndicators.Count == 0))
      {
        if (this.PrimaryAxis == null)
        {
          NumericalAxis numericalAxis = new NumericalAxis();
          numericalAxis.IsDefault = true;
          this.PrimaryAxis = (ChartAxisBase2D) numericalAxis;
        }
      }
      else if (this.Series != null)
      {
        if (this.Series.Where<ChartSeries>((Func<ChartSeries, bool>) (series =>
        {
          switch (series)
          {
            case HistogramSeries _:
            case CartesianSeries _ when (series as CartesianSeries).XAxis == null:
              return true;
            case PolarRadarSeriesBase _:
              return (series as PolarRadarSeriesBase).XAxis == null;
            default:
              return false;
          }
        })).ToList<ChartSeries>().Count != 0)
        {
          List<ChartValueType> list = this.Series.Where<ChartSeries>((Func<ChartSeries, bool>) (series =>
          {
            switch (series)
            {
              case HistogramSeries _:
              case CartesianSeries _ when series.ActualXAxis == null || !this.Axes.Contains(series.ActualXAxis) || series.ActualXAxis.IsDefault:
                return true;
              case PolarRadarSeriesBase _:
                return series.ActualXAxis == null || !this.Axes.Contains(series.ActualXAxis) || series.ActualXAxis.IsDefault;
              default:
                return false;
            }
          })).Select<ChartSeries, ChartValueType>((Func<ChartSeries, ChartValueType>) (series => series.XAxisValueType)).ToList<ChartValueType>();
          if (list.Count > 0)
          {
            this.SetPrimaryAxis(list[0]);
          }
          else
          {
            this.PrimaryAxis = this.PrimaryAxis != null ? this.Series[0].ActualXAxis as ChartAxisBase2D : this.PrimaryAxis;
            if (this.Annotations != null)
            {
              foreach (Annotation annotation in (Collection<Annotation>) this.Annotations)
                annotation.SetAxisFromName();
            }
          }
        }
        else if (this.TechnicalIndicators != null)
        {
          if (this.TechnicalIndicators.Where<ChartSeries>((Func<ChartSeries, bool>) (series => series is FinancialTechnicalIndicator && (series as FinancialTechnicalIndicator).YAxis == null)).ToList<ChartSeries>().Count != 0)
          {
            List<ChartValueType> list = this.TechnicalIndicators.Where<ChartSeries>((Func<ChartSeries, bool>) (series =>
            {
              if (!(series is FinancialTechnicalIndicator))
                return false;
              return series.ActualXAxis == null || !this.Axes.Contains(series.ActualXAxis) || series.ActualXAxis.IsDefault;
            })).Select<ChartSeries, ChartValueType>((Func<ChartSeries, ChartValueType>) (series => series.XAxisValueType)).ToList<ChartValueType>();
            if (list.Count > 0)
              this.SetPrimaryAxis(list[0]);
          }
          else
            this.ClearValue(SfChart.PrimaryAxisProperty);
        }
        else
          this.ClearValue(SfChart.PrimaryAxisProperty);
      }
    }
    if (this.SecondaryAxis != null && !this.SecondaryAxis.IsDefault)
      return;
    if ((this.Series == null || this.Series.Count == 0) && (this.TechnicalIndicators == null || this.TechnicalIndicators.Count == 0))
    {
      if (this.SecondaryAxis != null)
        return;
      NumericalAxis numericalAxis = new NumericalAxis();
      numericalAxis.IsDefault = true;
      this.SecondaryAxis = (RangeAxisBase) numericalAxis;
    }
    else
    {
      if (this.Series == null)
        return;
      if (this.Series.Where<ChartSeries>((Func<ChartSeries, bool>) (series =>
      {
        switch (series)
        {
          case HistogramSeries _:
          case CartesianSeries _ when (series as CartesianSeries).YAxis == null:
            return true;
          case PolarRadarSeriesBase _:
            return (series as PolarRadarSeriesBase).YAxis == null;
          default:
            return false;
        }
      })).ToList<ChartSeries>().Count != 0)
      {
        if (this.Series.Where<ChartSeries>((Func<ChartSeries, bool>) (series =>
        {
          switch (series)
          {
            case HistogramSeries _:
            case CartesianSeries _ when series.ActualYAxis == null || !this.Axes.Contains(series.ActualYAxis) || series.ActualYAxis.IsDefault:
              return true;
            case PolarRadarSeriesBase _:
              return series.ActualYAxis == null || !this.Axes.Contains(series.ActualYAxis) || series.ActualYAxis.IsDefault;
            default:
              return false;
          }
        })).ToList<ChartSeries>().Count > 0 && this.SecondaryAxis == null)
        {
          if (this.Axes.Contains(this.InternalSecondaryAxis))
            this.Axes.Remove(this.InternalSecondaryAxis);
          NumericalAxis numericalAxis = new NumericalAxis();
          numericalAxis.IsDefault = true;
          this.SecondaryAxis = (RangeAxisBase) numericalAxis;
        }
        else
        {
          this.SecondaryAxis = this.SecondaryAxis != null ? this.SecondaryAxis : this.Series[0].ActualYAxis as RangeAxisBase;
          if (this.Annotations == null)
            return;
          foreach (Annotation annotation in (Collection<Annotation>) this.Annotations)
            annotation.SetAxisFromName();
        }
      }
      else if (this.TechnicalIndicators != null)
      {
        if (this.TechnicalIndicators.Where<ChartSeries>((Func<ChartSeries, bool>) (series => series is FinancialTechnicalIndicator && (series as FinancialTechnicalIndicator).XAxis == null)).ToList<ChartSeries>().Count != 0)
        {
          if (this.TechnicalIndicators.Where<ChartSeries>((Func<ChartSeries, bool>) (series =>
          {
            if (!(series is FinancialTechnicalIndicator))
              return false;
            return series.ActualYAxis == null || !this.Axes.Contains(series.ActualYAxis) || series.ActualYAxis.IsDefault;
          })).ToList<ChartSeries>().Count > 0 && this.SecondaryAxis == null)
          {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.IsDefault = true;
            this.SecondaryAxis = (RangeAxisBase) numericalAxis;
          }
          else
            this.SecondaryAxis = this.SecondaryAxis != null ? this.SecondaryAxis : this.TechnicalIndicators[0].ActualYAxis as RangeAxisBase;
        }
        else
          this.ClearValue(SfChart.SecondaryAxisProperty);
      }
      else
        this.ClearValue(SfChart.SecondaryAxisProperty);
    }
  }

  internal void ConvertBitmapPixels()
  {
    foreach (ChartSeries chartSeries in (Collection<ChartSeries>) this.Series)
    {
      if (chartSeries.bitmapPixels.Count > 0)
        chartSeries.Pixels = new HashSet<int>((IEnumerable<int>) chartSeries.bitmapPixels);
      chartSeries.bitmapPixels.Clear();
    }
    this.isBitmapPixelsConverted = true;
  }

  internal void SetPrimaryAxis(ChartValueType type)
  {
    if (this.PrimaryAxis == null && this.Axes.Contains(this.InternalPrimaryAxis))
      this.Axes.Remove(this.InternalPrimaryAxis);
    switch (type)
    {
      case ChartValueType.Double:
        if (this.PrimaryAxis != null && !(this.PrimaryAxis.GetType() != typeof (NumericalAxis)))
          break;
        NumericalAxis numericalAxis = new NumericalAxis();
        numericalAxis.IsDefault = true;
        this.PrimaryAxis = (ChartAxisBase2D) numericalAxis;
        break;
      case ChartValueType.DateTime:
        if (this.PrimaryAxis != null && !(this.PrimaryAxis.GetType() != typeof (DateTimeAxis)))
          break;
        DateTimeAxis dateTimeAxis = new DateTimeAxis();
        dateTimeAxis.IsDefault = true;
        this.PrimaryAxis = (ChartAxisBase2D) dateTimeAxis;
        break;
      case ChartValueType.String:
        if (this.PrimaryAxis != null && !(this.PrimaryAxis.GetType() != typeof (CategoryAxis)))
          break;
        CategoryAxis categoryAxis = new CategoryAxis();
        categoryAxis.IsDefault = true;
        this.PrimaryAxis = (ChartAxisBase2D) categoryAxis;
        break;
      case ChartValueType.TimeSpan:
        if (this.PrimaryAxis != null && !(this.PrimaryAxis.GetType() != typeof (TimeSpanAxis)))
          break;
        TimeSpanAxis timeSpanAxis = new TimeSpanAxis();
        timeSpanAxis.IsDefault = true;
        this.PrimaryAxis = (ChartAxisBase2D) timeSpanAxis;
        break;
    }
  }

  internal void RenderSeries()
  {
    if (this.RootPanelDesiredSize.HasValue)
    {
      this.clearPixels = true;
      byte[] fastBuffer = this.fastBuffer;
      Size size = this.AreaType != ChartAreaType.None ? new Size(this.SeriesClipRect.Width, this.SeriesClipRect.Height) : this.RootPanelDesiredSize.Value;
      if (this.VisibleSeries != null)
      {
        this.isBitmapPixelsConverted = false;
        foreach (ChartSeriesBase chartSeriesBase in this.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (item => item.Visibility == Visibility.Visible)))
          chartSeriesBase.UpdateOnSeriesBoundChanged(size);
      }
      if (this.TechnicalIndicators != null)
      {
        foreach (ChartSeriesBase technicalIndicator in (Collection<ChartSeries>) this.TechnicalIndicators)
          technicalIndicator.UpdateOnSeriesBoundChanged(size);
      }
      if (!this.CanRenderToBuffer)
        this.fastBuffer = fastBuffer;
      this.RenderToBuffer();
    }
    this.isRenderSeriesDispatched = false;
    this.StackedValues = (Dictionary<object, StackingValues>) null;
  }

  internal double AreaValueToPoint(ChartAxis axis, double value)
  {
    if (axis == null)
      return double.NaN;
    return axis.Orientation == Orientation.Horizontal ? axis.ValueToPoint(value) + this.GetOffetValue(axis) : axis.ValueToPoint(value) + this.GetOffetValue(axis);
  }

  internal byte[] GetFastBuffer() => this.fastBuffer;

  internal WriteableBitmap GetFastRenderSurface() => this.fastRenderSurface;

  internal void CreateBuffer(Size size)
  {
    this.CanRenderToBuffer = false;
    this.fastBuffer = new byte[(int) size.Width * (int) size.Height * 4];
  }

  internal void ClearBuffer()
  {
    if (!this.clearPixels)
      return;
    if (this.fastRenderSurface != null)
    {
      this.fastRenderSurface.Lock();
      this.fastRenderSurface.Clear();
      this.fastRenderSurface.Unlock();
    }
    this.clearPixels = false;
  }

  internal void RenderToBuffer() => this.CanRenderToBuffer = false;

  internal void ScheduleRenderSeries()
  {
    if (this.isRenderSeriesDispatched)
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) new Action(this.RenderSeries));
    this.isRenderSeriesDispatched = true;
  }

  internal double GetPercentage(
    IList<ISupportAxes> seriesColl,
    string xItem,
    double item,
    int index,
    bool reCalculation)
  {
    double num = 0.0;
    StackingSeriesBase stackingSeriesBase1 = seriesColl[0] as StackingSeriesBase;
    if (reCalculation)
    {
      if (index == 0)
        this.sumItems.Clear();
      this.isIndexedFalseSumValues.Clear();
      if (stackingSeriesBase1 != null && stackingSeriesBase1.ActualXAxis is CategoryAxis && !(stackingSeriesBase1.ActualXAxis as CategoryAxis).IsIndexed)
      {
        foreach (ISupportAxes supportAxes in (IEnumerable<ISupportAxes>) seriesColl)
        {
          StackingSeriesBase stackingSeriesBase2 = supportAxes as StackingSeriesBase;
          if (stackingSeriesBase2.IsSeriesVisible)
          {
            IList<string> xvalues = (IList<string>) (stackingSeriesBase2.XValues as List<string>);
            int index1 = 0;
            foreach (string str in (IEnumerable<string>) xvalues)
            {
              string X = str;
              if (this.isIndexedFalseSumValues.Any<KeyValuePair<string, double>>((Func<KeyValuePair<string, double>, bool>) (s => s.Key == X)))
                this.isIndexedFalseSumValues[X] += stackingSeriesBase2.YValues[index1];
              else
                this.isIndexedFalseSumValues.Add(X, stackingSeriesBase2.YValues[index1]);
              ++index1;
            }
          }
        }
      }
      else
      {
        foreach (ISupportAxes supportAxes in (IEnumerable<ISupportAxes>) seriesColl)
        {
          StackingSeriesBase stackingSeriesBase3 = supportAxes as StackingSeriesBase;
          if (stackingSeriesBase3.IsSeriesVisible && stackingSeriesBase3 != null && stackingSeriesBase3.YValues.Count != 0)
          {
            double d = index < stackingSeriesBase3.YValues.Count ? stackingSeriesBase3.YValues[index] : 0.0;
            if (double.IsNaN(d))
              d = 0.0;
            num += Math.Abs(d);
          }
        }
      }
      this.sumItems.Add(num);
    }
    if (stackingSeriesBase1 != null && stackingSeriesBase1.ActualXAxis is CategoryAxis && !(stackingSeriesBase1.ActualXAxis as CategoryAxis).IsIndexed)
    {
      double indexedFalseSumValue = this.isIndexedFalseSumValues[xItem];
      item = item / indexedFalseSumValue * 100.0;
    }
    else if (this.sumItems.Count != 0)
      item = item / this.sumItems[index] * 100.0;
    return item;
  }

  protected internal override void OnSeriesBoundsChanged(ChartSeriesBoundsEventArgs args)
  {
    this.CreateFastRenderSurface();
    if (this.InternalCanvas != null)
      this.InternalCanvas.Clip = (Geometry) new RectangleGeometry()
      {
        Rect = new Rect(0.0, 0.0, this.SeriesClipRect.Width + 0.5, this.SeriesClipRect.Height + 0.5)
      };
    base.OnSeriesBoundsChanged(args);
  }

  protected internal virtual void OnZoomChanged(ZoomChangedEventArgs args)
  {
    if (this.ZoomChanged == null || args == null)
      return;
    this.ZoomChanged((object) this, args);
  }

  protected internal virtual void OnZoomChanging(ZoomChangingEventArgs args)
  {
    if (this.ZoomChanging == null || args == null)
      return;
    this.ZoomChanging((object) this, args);
  }

  protected internal virtual void OnSelectionZoomingStart(SelectionZoomingStartEventArgs args)
  {
    if (this.SelectionZoomingStart == null || args == null)
      return;
    this.SelectionZoomingStart((object) this, args);
  }

  protected internal virtual void OnSelectionZoomingEnd(SelectionZoomingEndEventArgs args)
  {
    if (this.SelectionZoomingEnd == null || args == null)
      return;
    this.SelectionZoomingEnd((object) this, args);
  }

  protected internal virtual void OnSelectionZoomingDelta(SelectionZoomingDeltaEventArgs args)
  {
    if (this.SelectionZoomingDelta == null || args == null)
      return;
    this.SelectionZoomingDelta((object) this, args);
  }

  protected internal virtual void OnPanChanged(PanChangedEventArgs args)
  {
    if (this.PanChanged == null || args == null)
      return;
    this.PanChanged((object) this, args);
  }

  protected internal virtual void OnPanChanging(PanChangingEventArgs args)
  {
    if (this.PanChanging == null || args == null)
      return;
    this.PanChanging((object) this, args);
  }

  protected internal virtual void OnResetZoom(ResetZoomEventArgs args)
  {
    if (this.ResetZooming == null || args == null)
      return;
    this.ResetZooming((object) this, args);
  }

  protected internal void AddZoomToolBar(
    ZoomingToolBar chartZoomingToolBar,
    ChartZoomPanBehavior zoomBehavior)
  {
    this.zoomingToolBar = chartZoomingToolBar;
    this.chartZoomBehavior = zoomBehavior;
    this.ToolkitCanvas.Children.Add((UIElement) chartZoomingToolBar);
  }

  protected internal void RemoveZoomToolBar(ZoomingToolBar chartZoomingToolBar)
  {
    this.ToolkitCanvas.Children.Remove((UIElement) chartZoomingToolBar);
    this.zoomingToolBar = (ZoomingToolBar) null;
    this.chartZoomBehavior = (ChartZoomPanBehavior) null;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.SizeChanged += new SizeChangedEventHandler(this.OnSfChartSizeChanged);
    if (this.seriesPresenter != null && this.seriesPresenter.Children.Contains((UIElement) this.fastRenderDevice))
      this.seriesPresenter.Children.Remove((UIElement) this.fastRenderDevice);
    this.seriesPresenter = this.GetTemplateChild("seriesPresenter") as Panel;
    this.chartAxisPanel = this.GetTemplateChild("PART_chartAxisPanel") as Panel;
    this.gridLinesPanel = this.GetTemplateChild("gridLines") as Panel;
    this.InternalCanvas = this.GetTemplateChild("InternalCanvas") as Panel;
    this.AdorningCanvas = this.GetTemplateChild("adorningCanvas") as Canvas;
    this.ChartDockPanel = this.GetTemplateChild("Part_DockPanel") as ChartDockPanel;
    this.rootPanel = this.GetTemplateChild("LayoutRoot") as ChartRootPanel;
    this.rootPanel.Area = (ChartBase) this;
    this.ChartAnnotationCanvas = this.GetTemplateChild("Part_ChartAnnotationCanvas") as Canvas;
    this.SeriesAnnotationCanvas = this.GetTemplateChild("Part_SeriesAnnotationCanvas") as Canvas;
    if (this.Annotations != null && this.Annotations.Count > 0)
      this.AnnotationManager = new AnnotationManager()
      {
        Chart = this,
        Annotations = this.Annotations
      };
    this.BottomAdorningCanvas = this.GetTemplateChild("bottomAdorningCanvas") as Canvas;
    this.ToolkitCanvas = this.GetTemplateChild("Part_ToolkitCanvas") as Canvas;
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
    {
      behavior.AdorningCanvas = this.AdorningCanvas;
      behavior.BottomAdorningCanvas = this.BottomAdorningCanvas;
      behavior.InternalAttachElements();
    }
    if (this.Series != null)
    {
      foreach (ChartSeries chartSeries in (Collection<ChartSeries>) this.Series)
      {
        chartSeries.Area = this;
        if (chartSeries.ShowTooltip)
          this.ShowTooltip = true;
      }
      if (this.ShowTooltip)
      {
        ChartSeriesBase.AddTooltipBehavior(this);
        this.Tooltip = new ChartTooltip();
      }
    }
    if (this.Behaviors != null)
    {
      foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      {
        if (behavior is ChartTooltipBehavior)
          (behavior as ChartTooltipBehavior).ChartArea = this;
      }
    }
    if (this.TechnicalIndicators != null)
    {
      foreach (ChartSeries technicalIndicator in (Collection<ChartSeries>) this.TechnicalIndicators)
        technicalIndicator.Area = this;
    }
    foreach (ChartAxis ax in (Collection<ChartAxis>) this.Axes)
      ax.Area = (ChartBase) this;
    this.UpdateAxisLayoutPanels();
    if (this.seriesPresenter != null)
    {
      this.AddOrRemoveBitmap();
      if (this.Series != null)
      {
        foreach (UIElement element in (Collection<ChartSeries>) this.Series)
          this.seriesPresenter.Children.Add(element);
      }
      if (this.TechnicalIndicators != null)
      {
        foreach (FinancialTechnicalIndicator technicalIndicator in (Collection<ChartSeries>) this.TechnicalIndicators)
        {
          if (!this.seriesPresenter.Children.Contains((UIElement) technicalIndicator))
            this.seriesPresenter.Children.Add((UIElement) technicalIndicator);
        }
      }
    }
    this.UpdateLegend(this.Legend, true);
    if (this.Watermark != null)
      this.AddOrRemoveWatermark(this.Watermark, (Watermark) null);
    this.IsTemplateApplied = true;
  }

  protected override AutomationPeer OnCreateAutomationPeer()
  {
    return (AutomationPeer) new SfChartAutomationPeer(this);
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    bool flag = false;
    double num1 = availableSize.Width;
    double num2 = availableSize.Height;
    if (double.IsInfinity(num1))
    {
      num1 = this.ActualWidth == 0.0 ? 500.0 : this.ActualWidth;
      flag = true;
    }
    if (double.IsInfinity(num2))
    {
      num2 = this.ActualHeight == 0.0 ? 300.0 : this.ActualHeight;
      flag = true;
    }
    if (flag)
    {
      this.SizeChanged -= new SizeChangedEventHandler(this.OnSizeChanged);
      this.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
      this.AvailableSize = new Size(num1, num2);
    }
    else
      this.AvailableSize = availableSize;
    return base.MeasureOverride(this.AvailableSize);
  }

  protected override void OnLostFocus(RoutedEventArgs e)
  {
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnLostFocus(e);
    base.OnLostFocus(e);
  }

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnGotFocus(e);
    base.OnGotFocus(e);
  }

  protected override void OnMouseEnter(MouseEventArgs e)
  {
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnMouseEnter(e);
    base.OnMouseEnter(e);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnMouseLeave(e);
    base.OnMouseLeave(e);
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnMouseWheel(e);
    base.OnMouseWheel(e);
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnMouseMove(e);
    base.OnMouseMove(e);
  }

  protected override void OnTouchDown(TouchEventArgs e)
  {
    if (this.Behaviors != null)
    {
      foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
        behavior.OnTouchDown(e);
    }
    base.OnTouchDown(e);
  }

  protected override void OnTouchMove(TouchEventArgs e)
  {
    if (this.Behaviors != null)
    {
      foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
        behavior.OnTouchMove(e);
    }
    base.OnTouchMove(e);
  }

  protected override void OnTouchUp(TouchEventArgs e)
  {
    if (this.Behaviors != null)
    {
      foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
        behavior.OnTouchUp(e);
    }
    base.OnTouchUp(e);
  }

  protected override void OnKeyUp(KeyEventArgs e)
  {
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnKeyUp(e);
    base.OnKeyUp(e);
  }

  protected override void OnKeyDown(KeyEventArgs e)
  {
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnKeyDown(e);
    base.OnKeyDown(e);
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.Axes != null)
    {
      foreach (ChartAxisBase2D ax in (Collection<ChartAxis>) this.Axes)
      {
        ax.isManipulated = ax.EnableTouchMode;
        if (this.AreaType == ChartAreaType.PolarAxes)
        {
          if (ax.Orientation == Orientation.Horizontal && !(e.OriginalSource is ChartCartesianAxisPanel))
            ax.SetLabelDownArguments(e.OriginalSource);
        }
        else if (!(e.OriginalSource is ChartCartesianAxisPanel))
        {
          Point position = e.GetPosition((IInputElement) this.chartAxisPanel);
          if (ax.axisLabelsPanel is ChartCartesianAxisLabelsPanel axisLabelsPanel && axisLabelsPanel.Bounds.Contains(position))
            ax.SetLabelDownArguments(e.OriginalSource);
        }
      }
    }
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnMouseLeftButtonDown(e);
    base.OnMouseLeftButtonDown(e);
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    if (this.Axes != null)
    {
      foreach (ChartAxisBase2D ax in (Collection<ChartAxis>) this.Axes)
        ax.isManipulated = !ax.EnableTouchMode;
    }
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnMouseLeftButtonUp(e);
    base.OnMouseLeftButtonUp(e);
  }

  protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
  {
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnManipulationStarted(e);
    base.OnManipulationStarted(e);
  }

  protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
  {
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnManipulationCompleted(e);
    base.OnManipulationCompleted(e);
  }

  protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
  {
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnManipulationDelta(e);
    base.OnManipulationDelta(e);
  }

  protected override void OnDrop(DragEventArgs e)
  {
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnDrop(e);
    base.OnDrop(e);
  }

  protected override void OnDragOver(DragEventArgs e)
  {
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnDragOver(e);
    base.OnDragOver(e);
  }

  protected override void OnDragLeave(DragEventArgs e)
  {
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnDragLeave(e);
    base.OnDragLeave(e);
  }

  protected override void OnDragEnter(DragEventArgs e)
  {
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnDragEnter(e);
    base.OnDragEnter(e);
  }

  private static void OnPrimaryAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ChartAxis newValue = e.NewValue as ChartAxis;
    ChartAxis oldValue = e.OldValue as ChartAxis;
    SfChart sfChart = d as SfChart;
    if (newValue != null)
    {
      newValue.Area = (ChartBase) sfChart;
      newValue.Orientation = Orientation.Horizontal;
      sfChart.InternalPrimaryAxis = (ChartAxis) e.NewValue;
      newValue.VisibleRangeChanged += new EventHandler<VisibleRangeChangedEventArgs>(newValue.OnVisibleRangeChanged);
    }
    if (oldValue != null)
    {
      if (sfChart != null && sfChart.Axes != null && sfChart.Axes.Contains(oldValue))
      {
        sfChart.Axes.RemoveItem(oldValue, sfChart.DependentSeriesAxes.Contains(oldValue));
        sfChart.DependentSeriesAxes.Remove(oldValue);
      }
      oldValue.VisibleRangeChanged -= new EventHandler<VisibleRangeChangedEventArgs>(oldValue.OnVisibleRangeChanged);
      oldValue.RegisteredSeries.Clear();
      oldValue.Dispose();
    }
    if (sfChart.Series != null && newValue != null)
    {
      foreach (ChartSeries series in (Collection<ChartSeries>) sfChart.Series)
      {
        CartesianSeries cartesianSeries = series as CartesianSeries;
        PolarRadarSeriesBase polarRadarSeriesBase = series as PolarRadarSeriesBase;
        if (cartesianSeries != null && cartesianSeries.XAxis == null || polarRadarSeriesBase != null && polarRadarSeriesBase.XAxis == null)
        {
          SfChart.CheckSeriesTransposition(series);
          newValue.RegisteredSeries.Add((ISupportAxes) series);
        }
      }
    }
    if (sfChart.TechnicalIndicators != null && newValue != null)
    {
      foreach (ChartSeries technicalIndicator1 in (Collection<ChartSeries>) sfChart.TechnicalIndicators)
      {
        if (technicalIndicator1 is FinancialTechnicalIndicator technicalIndicator2 && technicalIndicator2.XAxis == null)
        {
          SfChart.CheckSeriesTransposition(technicalIndicator1);
          newValue.RegisteredSeries.Add((ISupportAxes) technicalIndicator1);
        }
      }
    }
    sfChart.OnAxisChanged(e);
  }

  private static void OnSecondaryAxisChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartAxis newValue = e.NewValue as ChartAxis;
    ChartAxis oldValue = e.OldValue as ChartAxis;
    SfChart sfChart = d as SfChart;
    if (newValue != null)
    {
      newValue.Area = (ChartBase) sfChart;
      newValue.Orientation = Orientation.Vertical;
      sfChart.InternalSecondaryAxis = (ChartAxis) e.NewValue;
      newValue.IsSecondaryAxis = true;
    }
    if (oldValue != null)
    {
      if (oldValue is NumericalAxis numericalAxis && numericalAxis.AxisScaleBreaks != null && numericalAxis.AxisScaleBreaks.Count >= 0)
        numericalAxis.ClearBreakElements();
      if (sfChart != null && sfChart.Axes != null && sfChart.Axes.Contains(oldValue))
      {
        sfChart.Axes.RemoveItem(oldValue, sfChart.DependentSeriesAxes.Contains(oldValue));
        sfChart.DependentSeriesAxes.Remove(oldValue);
      }
      oldValue.RegisteredSeries.Clear();
      oldValue.Dispose();
    }
    if (sfChart.Series != null && newValue != null)
    {
      foreach (ChartSeries series in (Collection<ChartSeries>) sfChart.Series)
      {
        if (series is CartesianSeries cartesianSeries && cartesianSeries.YAxis == null)
        {
          SfChart.CheckSeriesTransposition(series);
          newValue.RegisteredSeries.Add((ISupportAxes) series);
        }
      }
    }
    if (sfChart.TechnicalIndicators != null && newValue != null)
    {
      foreach (ChartSeries technicalIndicator1 in (Collection<ChartSeries>) sfChart.TechnicalIndicators)
      {
        if (technicalIndicator1 is FinancialTechnicalIndicator technicalIndicator2 && technicalIndicator2.YAxis == null)
        {
          SfChart.CheckSeriesTransposition(technicalIndicator1);
          newValue.RegisteredSeries.Add((ISupportAxes) technicalIndicator1);
        }
      }
    }
    sfChart.OnAxisChanged(e);
  }

  private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as SfChart).OnWaterMarkChanged(e);
  }

  private void OnBehaviorPropertyChanged(
    ChartBehaviorsCollection oldValue,
    ChartBehaviorsCollection newValue)
  {
    if (oldValue != null)
      oldValue.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.Behaviors_CollectionChanged);
    if (newValue == null)
      return;
    newValue.CollectionChanged += new NotifyCollectionChangedEventHandler(this.Behaviors_CollectionChanged);
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) newValue)
      this.AddChartBehavior(behavior);
  }

  private void AddChartBehavior(ChartBehavior behavior)
  {
    if (!(behavior is ChartTooltipBehavior))
      return;
    behavior.ChartArea = this;
    this.RemoveDefaultTooltipBehavior((ChartTooltipBehavior) behavior);
  }

  private void RemoveDefaultTooltipBehavior(ChartTooltipBehavior tooltipBehavior)
  {
    int index = -1;
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
    {
      if (behavior is ChartTooltipBehavior && behavior != tooltipBehavior)
      {
        index = this.Behaviors.IndexOf(behavior);
        break;
      }
    }
    if (index < 0 || index >= this.Behaviors.Count - 1)
      return;
    this.Behaviors.RemoveAt(index);
    this.TooltipBehavior = (ChartTooltipBehavior) null;
  }

  private static void OnSeriesPropertyCollectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as SfChart).OnSeriesPropertyCollectionChanged(e);
  }

  private static void OnTechnicalIndicatorsPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as SfChart).OnTechnicalIndicatorsPropertyChanged(e);
  }

  private static void OnAnnotationsChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    (sender as SfChart).AnnotationsChanged(args);
  }

  private static void CheckSeriesTransposition(ChartSeries series)
  {
    if (series.ActualXAxis == null || series.ActualYAxis == null)
      return;
    series.ActualXAxis.Orientation = series.IsActualTransposed ? Orientation.Vertical : Orientation.Horizontal;
    series.ActualYAxis.Orientation = series.IsActualTransposed ? Orientation.Horizontal : Orientation.Vertical;
  }

  private void RemoveBorderLines()
  {
    if (this.ColumnDefinitions.Count == 0)
    {
      foreach (UIElement columnBorderLine in (IEnumerable) this.ColumnBorderLines)
        this.gridLinesPanel.Children.Remove(columnBorderLine);
      this.ColumnBorderLines.Clear();
    }
    if (this.RowDefinitions.Count != 0)
      return;
    foreach (UIElement rowBorderLine in (IEnumerable) this.RowBorderLines)
      this.gridLinesPanel.Children.Remove(rowBorderLine);
    this.RowBorderLines.Clear();
  }

  private void OnWaterMarkChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.ChartDockPanel == null)
      return;
    this.AddOrRemoveWatermark(e.NewValue as Watermark, e.OldValue as Watermark);
  }

  private void OnSeriesPropertyCollectionChanged(DependencyPropertyChangedEventArgs e)
  {
    if (e.OldValue != null)
    {
      this.RemoveVisualChild();
      this.AddOrRemoveBitmap();
      ChartSeriesCollection oldValue = e.OldValue as ChartSeriesCollection;
      oldValue.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnSeriesCollectionChanged);
      foreach (ChartSeries chartSeries in (Collection<ChartSeries>) oldValue)
      {
        if (chartSeries is CartesianSeries cartesianSeries)
        {
          cartesianSeries.XAxis = (ChartAxisBase2D) null;
          cartesianSeries.YAxis = (RangeAxisBase) null;
        }
      }
    }
    if (this.Series != null)
    {
      this.Series.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnSeriesCollectionChanged);
      if (this.Series.Count > 0)
      {
        if (this.Series[0] is PolarSeries || this.Series[0] is RadarSeries)
          this.AreaType = ChartAreaType.PolarAxes;
        else if (this.Series[0] is AccumulationSeriesBase)
          this.AreaType = ChartAreaType.None;
        else
          this.AreaType = ChartAreaType.CartesianAxes;
        foreach (ChartSeries chartSeries in (Collection<ChartSeries>) this.Series)
        {
          chartSeries.UpdateLegendIconTemplate(false);
          chartSeries.Area = this;
          ISupportAxes supportAxes = chartSeries as ISupportAxes;
          if (chartSeries.ActualXAxis != null)
          {
            chartSeries.ActualXAxis.Area = (ChartBase) this;
            if (supportAxes != null && !chartSeries.ActualXAxis.RegisteredSeries.Contains(supportAxes))
              chartSeries.ActualXAxis.RegisteredSeries.Add(supportAxes);
            if (!this.Axes.Contains(chartSeries.ActualXAxis))
            {
              this.Axes.Add(chartSeries.ActualXAxis);
              this.DependentSeriesAxes.Add(chartSeries.ActualXAxis);
            }
          }
          if (chartSeries.ActualYAxis != null)
          {
            chartSeries.ActualYAxis.Area = (ChartBase) this;
            if (supportAxes != null && !chartSeries.ActualYAxis.RegisteredSeries.Contains(supportAxes))
              chartSeries.ActualYAxis.RegisteredSeries.Add(supportAxes);
            if (!this.Axes.Contains(chartSeries.ActualYAxis))
            {
              this.Axes.Add(chartSeries.ActualYAxis);
              this.DependentSeriesAxes.Add(chartSeries.ActualYAxis);
            }
          }
          SfChart.CheckSeriesTransposition(chartSeries);
          if (this.seriesPresenter != null && !this.seriesPresenter.Children.Contains((UIElement) chartSeries))
            this.seriesPresenter.Children.Add((UIElement) chartSeries);
          if (chartSeries.IsSeriesVisible)
          {
            if (this.AreaType == ChartAreaType.PolarAxes && (chartSeries is PolarSeries || chartSeries is RadarSeries))
              this.VisibleSeries.Add((ChartSeriesBase) chartSeries);
            else if (this.AreaType == ChartAreaType.None && chartSeries is AccumulationSeriesBase)
              this.VisibleSeries.Add((ChartSeriesBase) chartSeries);
            else if (this.AreaType == ChartAreaType.CartesianAxes)
            {
              switch (chartSeries)
              {
                case CartesianSeries _:
                case HistogramSeries _:
                  this.VisibleSeries.Add((ChartSeriesBase) chartSeries);
                  break;
              }
            }
          }
          this.ActualSeries.Add((ChartSeriesBase) chartSeries);
        }
      }
      this.UpdateLegend(this.Legend, false);
      this.AddOrRemoveBitmap();
    }
    else
      this.UpdateLegend(this.Legend, true);
    this.ScheduleUpdate();
  }

  private void RemoveVisualChild()
  {
    if (this.seriesPresenter != null)
    {
      for (int index = this.seriesPresenter.Children.Count - 1; index >= 0; --index)
      {
        if (this.seriesPresenter.Children[index] is AdornmentSeries || this.seriesPresenter.Children[index] is HistogramSeries)
        {
          if (this.seriesPresenter.Children[index] is ISupportAxes child && child is CartesianSeries cartesianSeries)
          {
            if (cartesianSeries.ActualXAxis != null)
              cartesianSeries.ActualXAxis.RegisteredSeries.Clear();
            if (cartesianSeries.ActualYAxis != null)
              cartesianSeries.ActualYAxis.RegisteredSeries.Clear();
            if (this.InternalPrimaryAxis == cartesianSeries.XAxis)
              this.InternalPrimaryAxis = (ChartAxis) null;
            if (this.InternalSecondaryAxis == cartesianSeries.YAxis)
              this.InternalSecondaryAxis = (ChartAxis) null;
          }
          this.seriesPresenter.Children.RemoveAt(index);
        }
      }
    }
    this.VisibleSeries.Clear();
    this.ActualSeries.Clear();
  }

  private void OnTechnicalIndicatorsPropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    if (e.OldValue != null)
    {
      if (this.seriesPresenter != null)
      {
        this.AddOrRemoveBitmap();
        for (int index = this.seriesPresenter.Children.Count - 1; index >= 0; --index)
        {
          if (this.seriesPresenter.Children[index] is FinancialTechnicalIndicator)
          {
            ISupportAxes2D child = this.seriesPresenter.Children[index] as ISupportAxes2D;
            child.XAxis = (ChartAxisBase2D) null;
            child.YAxis = (RangeAxisBase) null;
            this.seriesPresenter.Children.RemoveAt(index);
          }
        }
      }
      (e.OldValue as ObservableCollection<ChartSeries>).CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnTechnicalIndicatorsCollectionChanged);
    }
    if (this.TechnicalIndicators != null)
    {
      this.TechnicalIndicators.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnTechnicalIndicatorsCollectionChanged);
      if (this.TechnicalIndicators.Count > 0)
      {
        foreach (ChartSeries technicalIndicator1 in (Collection<ChartSeries>) this.TechnicalIndicators)
        {
          technicalIndicator1.Area = this;
          FinancialTechnicalIndicator technicalIndicator2 = technicalIndicator1 as FinancialTechnicalIndicator;
          if (technicalIndicator2.XAxis != null && !this.Axes.Contains((ChartAxis) technicalIndicator2.XAxis))
          {
            technicalIndicator2.XAxis.Area = (ChartBase) this;
            this.Axes.Add((ChartAxis) technicalIndicator2.XAxis);
            if (!technicalIndicator2.XAxis.RegisteredSeries.Contains((ISupportAxes) technicalIndicator2))
              technicalIndicator2.XAxis.RegisteredSeries.Add((ISupportAxes) technicalIndicator2);
          }
          if (technicalIndicator2.YAxis != null && !this.Axes.Contains((ChartAxis) technicalIndicator2.YAxis))
          {
            technicalIndicator2.YAxis.Area = (ChartBase) this;
            this.Axes.Add((ChartAxis) technicalIndicator2.YAxis);
            if (!technicalIndicator2.YAxis.RegisteredSeries.Contains((ISupportAxes) technicalIndicator2))
              technicalIndicator2.YAxis.RegisteredSeries.Add((ISupportAxes) technicalIndicator2);
          }
          else
          {
            switch (technicalIndicator2)
            {
              case SimpleAverageIndicator _:
              case TriangularAverageIndicator _:
              case BollingerBandIndicator _:
              case ExponentialAverageIndicator _:
                break;
              default:
                FinancialTechnicalIndicator technicalIndicator3 = technicalIndicator2;
                NumericalAxis numericalAxis1 = new NumericalAxis();
                numericalAxis1.OpposedPosition = true;
                numericalAxis1.RangePadding = NumericalPadding.Round;
                NumericalAxis numericalAxis2 = numericalAxis1;
                technicalIndicator3.YAxis = (RangeAxisBase) numericalAxis2;
                break;
            }
          }
          if (this.seriesPresenter != null && !this.seriesPresenter.Children.Contains((UIElement) technicalIndicator1))
            this.seriesPresenter.Children.Add((UIElement) technicalIndicator1);
        }
      }
      this.AddOrRemoveBitmap();
    }
    this.ScheduleUpdate();
  }

  private void UpdateAnnotationClips()
  {
    foreach (Annotation annotation in (Collection<Annotation>) this.Annotations)
      annotation.SetAxisFromName();
    this.ChartAnnotationCanvas.ClipToBounds = true;
    this.SeriesAnnotationCanvas.Clip = (Geometry) new RectangleGeometry()
    {
      Rect = this.SeriesClipRect
    };
  }

  private void OnAxisChanged(DependencyPropertyChangedEventArgs e)
  {
    ChartAxis newValue = e.NewValue as ChartAxis;
    if (this.Axes != null && newValue != null && !this.Axes.Contains(newValue))
    {
      newValue.Area = (ChartBase) this;
      this.Axes.Insert(0, newValue);
      this.DependentSeriesAxes.Add(newValue);
    }
    if (this.AnnotationManager != null)
      this.AnnotationManager.Annotations = this.Annotations;
    this.ScheduleUpdate();
  }

  private void fastRenderDevice_MouseLeave(object sender, MouseEventArgs e)
  {
    for (int index = this.VisibleSeries.Count - 1; index >= 0; --index)
    {
      ChartSeries chartSeries = this.VisibleSeries[index] as ChartSeries;
      if (chartSeries.ShowTooltip && (chartSeries.ActualTooltipPosition == TooltipPosition.Pointer || !chartSeries.Timer.IsEnabled))
        chartSeries.RemoveTooltip();
    }
  }

  private void OnTechnicalIndicatorsCollectionChanged(
    object sender,
    NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == NotifyCollectionChangedAction.Reset)
    {
      if (this.seriesPresenter != null)
      {
        for (int index = this.seriesPresenter.Children.Count - 1; index >= 0; --index)
        {
          if (this.seriesPresenter.Children[index] is FinancialTechnicalIndicator)
          {
            this.UnRegisterSeries(this.seriesPresenter.Children[index] as ISupportAxes);
            this.seriesPresenter.Children.RemoveAt(index);
          }
        }
      }
    }
    else if (e.Action == NotifyCollectionChangedAction.Remove)
    {
      if (!(e.OldItems[0] is FinancialTechnicalIndicator oldItem))
        return;
      if (oldItem.ActualYAxis.RegisteredSeries != null && oldItem.ActualYAxis.RegisteredSeries.Contains((ISupportAxes) oldItem))
      {
        oldItem.YAxis = (RangeAxisBase) null;
        oldItem.XAxis = (ChartAxisBase2D) null;
      }
      if (this.seriesPresenter.Children.Contains((UIElement) oldItem))
        this.seriesPresenter.Children.Remove((UIElement) oldItem);
    }
    else if (e.Action == NotifyCollectionChangedAction.Add)
    {
      foreach (FinancialTechnicalIndicator newItem in (IEnumerable) e.NewItems)
      {
        newItem.UpdateLegendIconTemplate(false);
        newItem.Area = this;
        if (newItem.XAxis != null && !this.Axes.Contains((ChartAxis) newItem.XAxis))
        {
          newItem.XAxis.Area = (ChartBase) this;
          this.Axes.Add((ChartAxis) newItem.XAxis);
          if (!newItem.XAxis.RegisteredSeries.Contains((ISupportAxes) newItem))
            newItem.XAxis.RegisteredSeries.Add((ISupportAxes) newItem);
        }
        if (newItem.YAxis != null && !this.Axes.Contains((ChartAxis) newItem.YAxis))
        {
          newItem.YAxis.Area = (ChartBase) this;
          this.Axes.Add((ChartAxis) newItem.YAxis);
          if (!newItem.YAxis.RegisteredSeries.Contains((ISupportAxes) newItem))
            newItem.YAxis.RegisteredSeries.Add((ISupportAxes) newItem);
        }
        else
        {
          switch (newItem)
          {
            case SimpleAverageIndicator _:
            case TriangularAverageIndicator _:
            case BollingerBandIndicator _:
            case ExponentialAverageIndicator _:
              break;
            default:
              FinancialTechnicalIndicator technicalIndicator = newItem;
              NumericalAxis numericalAxis1 = new NumericalAxis();
              numericalAxis1.OpposedPosition = true;
              numericalAxis1.RangePadding = NumericalPadding.Round;
              NumericalAxis numericalAxis2 = numericalAxis1;
              technicalIndicator.YAxis = (RangeAxisBase) numericalAxis2;
              break;
          }
        }
        if (this.seriesPresenter != null && !this.seriesPresenter.Children.Contains((UIElement) newItem))
          this.seriesPresenter.Children.Add((UIElement) newItem);
      }
    }
    this.AddOrRemoveBitmap();
    this.ScheduleUpdate();
  }

  private void OnSeriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == NotifyCollectionChangedAction.Reset)
    {
      if (this.seriesPresenter != null)
      {
        for (int index = this.seriesPresenter.Children.Count - 1; index >= 0; --index)
        {
          if (this.seriesPresenter.Children[index] is AdornmentSeries || this.seriesPresenter.Children[index] is HistogramSeries)
          {
            if (this.seriesPresenter.Children[index] is ISupportAxes)
              this.UnRegisterSeries(this.seriesPresenter.Children[index] as ISupportAxes);
            UIElement child = this.seriesPresenter.Children[index];
            if (child is DoughnutSeries doughnutSeries)
              doughnutSeries.RemoveCenterView(doughnutSeries.CenterView);
            this.seriesPresenter.Children.Remove(child);
          }
        }
      }
      else
      {
        foreach (ChartAxis ax in (Collection<ChartAxis>) this.Axes)
          ax.RegisteredSeries.Clear();
      }
      if (this.LegendItems != null)
      {
        if (this.Legend is ChartLegendCollection)
        {
          foreach (Collection<LegendItem> legendItem in (Collection<ObservableCollection<LegendItem>>) this.LegendItems)
            legendItem.Clear();
        }
        else if (this.LegendItems.Count > 0)
          this.LegendItems[0].Clear();
      }
      this.ActualSeries.Clear();
      this.VisibleSeries.Clear();
      this.SelectedSeriesCollection.Clear();
      this.PreviousSelectedSeries = (ChartSeriesBase) null;
    }
    else if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
    {
      ChartSeriesBase series = e.OldItems[0] as ChartSeriesBase;
      if (this.VisibleSeries.Contains(series))
        this.VisibleSeries.Remove(series);
      if (this.ActualSeries.Contains(series))
        this.ActualSeries.Remove(series);
      if (this.SelectedSeriesCollection.Contains(series))
        this.SelectedSeriesCollection.Remove(series);
      this.UnRegisterSeries(series as ISupportAxes);
      if (series is AccumulationSeriesBase || series is CircularSeriesBase)
      {
        if (this.Legend != null && this.LegendItems != null)
        {
          List<ObservableCollection<LegendItem>> list = this.LegendItems.Where<ObservableCollection<LegendItem>>((Func<ObservableCollection<LegendItem>, bool>) (item => item.Where<LegendItem>((Func<LegendItem, bool>) (it => it.Series == series)).Count<LegendItem>() > 0)).ToList<ObservableCollection<LegendItem>>();
          if (list.Count > 0)
            list[0].Clear();
        }
      }
      else if (this.Legend != null && this.LegendItems != null)
      {
        List<ObservableCollection<LegendItem>> list1 = this.LegendItems.Where<ObservableCollection<LegendItem>>((Func<ObservableCollection<LegendItem>, bool>) (item => item.Where<LegendItem>((Func<LegendItem, bool>) (it => it.Series == series)).Count<LegendItem>() > 0)).ToList<ObservableCollection<LegendItem>>();
        if (list1.Count > 0)
        {
          int index1 = list1[0].IndexOf(list1[0].Where<LegendItem>((Func<LegendItem, bool>) (item => item.Series == series)).FirstOrDefault<LegendItem>());
          int index2 = this.LegendItems.IndexOf(list1[0]);
          if (list1[0].Count<LegendItem>() > 0 && this.LegendItems[index2].Contains(list1[0][index1]))
          {
            this.LegendItems[index2].Remove(list1[0][index1]);
            if (series is CartesianSeries)
            {
              foreach (Trendline trendline in (Collection<Trendline>) (series as CartesianSeries).Trendlines)
              {
                Trendline item = trendline;
                List<LegendItem> list2 = this.LegendItems[index2].Where<LegendItem>((Func<LegendItem, bool>) (it => it.Trendline == item)).ToList<LegendItem>();
                if (list2.Count<LegendItem>() > 0 && this.LegendItems[index2].Contains(list2[0]))
                  this.LegendItems[index2].Remove(list2[0]);
              }
            }
          }
        }
      }
      if (this.seriesPresenter != null)
        this.seriesPresenter.Children.Remove((UIElement) series);
      series.RemoveTooltip();
      if (series is DoughnutSeries doughnutSeries)
        doughnutSeries.RemoveCenterView(doughnutSeries.CenterView);
      if (e.Action == NotifyCollectionChangedAction.Replace)
      {
        if (this.Series.Count > 0)
        {
          if (this.Series[0] is PolarRadarSeriesBase)
            this.AreaType = ChartAreaType.PolarAxes;
          else if (this.Series[0] is AccumulationSeriesBase)
            this.AreaType = ChartAreaType.None;
          else
            this.AreaType = ChartAreaType.CartesianAxes;
          this.UpdateVisibleSeries((IList) this.Series, e.NewStartingIndex);
        }
      }
      else if (this.VisibleSeries.Count == 0 && this.Series.Count > 0)
      {
        if (this.Series[0] is PolarRadarSeriesBase)
          this.AreaType = ChartAreaType.PolarAxes;
        else if (this.Series[0] is AccumulationSeriesBase)
          this.AreaType = ChartAreaType.None;
        else
          this.AreaType = ChartAreaType.CartesianAxes;
      }
      else if (this.VisibleSeries.Count == 0 && this.Series.Count == 0)
        this.AreaType = ChartAreaType.CartesianAxes;
      if (this.Palette != ChartColorPalette.None)
      {
        foreach (ChartSegment chartSegment in this.VisibleSeries.SelectMany<ChartSeriesBase, ChartSegment>((Func<ChartSeriesBase, IEnumerable<ChartSegment>>) (ser => (IEnumerable<ChartSegment>) ser.Segments)))
          chartSegment.BindProperties();
      }
    }
    else if (e.Action == NotifyCollectionChangedAction.Add)
    {
      if (e.NewStartingIndex == 0)
      {
        if (this.Series[0] is PolarRadarSeriesBase)
          this.AreaType = ChartAreaType.PolarAxes;
        else if (this.Series[0] is AccumulationSeriesBase)
          this.AreaType = ChartAreaType.None;
        else
          this.AreaType = ChartAreaType.CartesianAxes;
      }
      if (e.OldItems == null && this.GetEnableSeriesSelection() && this.SeriesSelectedIndex < this.Series.Count && this.SeriesSelectedIndex != -1)
        this.SelectedSeriesCollection.Add((ChartSeriesBase) this.Series[this.SeriesSelectedIndex]);
      this.UpdateVisibleSeries(e.NewItems, e.NewStartingIndex);
    }
    Canvas adorningCanvas = this.GetAdorningCanvas();
    if (adorningCanvas != null && adorningCanvas.Children.Contains((UIElement) this.Tooltip))
      adorningCanvas.Children.Remove((UIElement) this.Tooltip);
    this.IsUpdateLegend = true;
    this.AddOrRemoveBitmap();
    this.ScheduleUpdate();
    this.SBSInfoCalculated = false;
  }

  private void UnRegisterSeries(ISupportAxes series)
  {
    if (series != null)
    {
      if (series is CartesianSeries)
      {
        CartesianSeries cartesianSeries = series as CartesianSeries;
        if (cartesianSeries.YAxis == null && this.InternalSecondaryAxis != null)
        {
          this.InternalSecondaryAxis.RegisteredSeries.Remove(series);
        }
        else
        {
          if (this.InternalSecondaryAxis == cartesianSeries.YAxis)
            this.InternalSecondaryAxis = (ChartAxis) null;
          cartesianSeries.ClearValue(CartesianSeries.YAxisProperty);
        }
        if (cartesianSeries.XAxis == null)
        {
          this.InternalPrimaryAxis.RegisteredSeries.Remove(series);
        }
        else
        {
          if (this.InternalPrimaryAxis == cartesianSeries.XAxis)
            this.InternalPrimaryAxis = (ChartAxis) null;
          cartesianSeries.ClearValue(CartesianSeries.XAxisProperty);
        }
      }
      else if (series is FinancialTechnicalIndicator)
      {
        FinancialTechnicalIndicator technicalIndicator = series as FinancialTechnicalIndicator;
        if (technicalIndicator.YAxis == null)
        {
          this.InternalSecondaryAxis.RegisteredSeries.Remove(series);
        }
        else
        {
          if (technicalIndicator.YAxis.Equals((object) this.SecondaryAxis))
            this.ClearValue(SfChart.SecondaryAxisProperty);
          technicalIndicator.ClearValue(FinancialTechnicalIndicator.YAxisProperty);
        }
        if (technicalIndicator.XAxis == null)
        {
          this.InternalPrimaryAxis.RegisteredSeries.Remove(series);
        }
        else
        {
          if (technicalIndicator.XAxis == this.PrimaryAxis)
            this.ClearValue(SfChart.PrimaryAxisProperty);
          technicalIndicator.ClearValue(FinancialTechnicalIndicator.XAxisProperty);
        }
      }
    }
    if (this.InternalPrimaryAxis == null || this.InternalSecondaryAxis == null || this.InternalPrimaryAxis.RegisteredSeries.Count != 0 || this.InternalSecondaryAxis.RegisteredSeries.Count != 0 || series == null || !(series as ChartSeries).IsActualTransposed)
      return;
    this.InternalPrimaryAxis.Orientation = Orientation.Horizontal;
    this.InternalSecondaryAxis.Orientation = Orientation.Vertical;
  }

  private void UpdateVisibleSeries(IList seriesColl, int seriesIndex)
  {
    foreach (ChartSeries chartSeries in (IEnumerable) seriesColl)
    {
      if (chartSeries != null)
      {
        chartSeries.UpdateLegendIconTemplate(false);
        chartSeries.Area = this;
        SfChart.CheckSeriesTransposition(chartSeries);
        ISupportAxes supportAxes = chartSeries as ISupportAxes;
        if (chartSeries.ActualXAxis != null)
        {
          chartSeries.ActualXAxis.Area = (ChartBase) this;
          if (supportAxes != null && !chartSeries.ActualXAxis.RegisteredSeries.Contains(supportAxes))
            chartSeries.ActualXAxis.RegisteredSeries.Add(supportAxes);
          if (!this.Axes.Contains(chartSeries.ActualXAxis))
          {
            this.Axes.Add(chartSeries.ActualXAxis);
            this.DependentSeriesAxes.Add(chartSeries.ActualXAxis);
          }
        }
        if (chartSeries.ActualYAxis != null)
        {
          chartSeries.ActualYAxis.Area = (ChartBase) this;
          if (supportAxes != null && !chartSeries.ActualYAxis.RegisteredSeries.Contains(supportAxes))
            chartSeries.ActualYAxis.RegisteredSeries.Add(supportAxes);
          if (!this.Axes.Contains(chartSeries.ActualYAxis))
          {
            this.Axes.Add(chartSeries.ActualYAxis);
            this.DependentSeriesAxes.Add(chartSeries.ActualYAxis);
          }
        }
        if (this.seriesPresenter != null && !this.seriesPresenter.Children.Contains((UIElement) chartSeries))
          this.seriesPresenter.Children.Insert(seriesIndex, (UIElement) chartSeries);
        if (chartSeries.IsSeriesVisible)
        {
          int count = this.VisibleSeries.Count;
          int index = seriesIndex > count ? count : seriesIndex;
          if (this.AreaType == ChartAreaType.PolarAxes && chartSeries is PolarRadarSeriesBase && !this.VisibleSeries.Contains((ChartSeriesBase) chartSeries))
            this.VisibleSeries.Insert(index, (ChartSeriesBase) chartSeries);
          else if (this.AreaType == ChartAreaType.None && chartSeries is AccumulationSeriesBase && !this.VisibleSeries.Contains((ChartSeriesBase) chartSeries))
            this.VisibleSeries.Insert(index, (ChartSeriesBase) chartSeries);
          else if (this.AreaType == ChartAreaType.CartesianAxes && (chartSeries is CartesianSeries || chartSeries is HistogramSeries) && !this.VisibleSeries.Contains((ChartSeriesBase) chartSeries))
            this.VisibleSeries.Insert(index, (ChartSeriesBase) chartSeries);
        }
        if (!this.ActualSeries.Contains((ChartSeriesBase) chartSeries))
          this.ActualSeries.Insert(seriesIndex, (ChartSeriesBase) chartSeries);
      }
    }
  }

  private void AddOrRemoveWatermark(Watermark newWatermark, Watermark oldWatermark)
  {
    if (this.ChartDockPanel.Children.Contains((UIElement) oldWatermark))
      this.ChartDockPanel.Children.Remove((UIElement) oldWatermark);
    if (newWatermark == null || this.rootPanel.Children.Contains((UIElement) newWatermark))
      return;
    this.Watermark.SetValue(ChartDockPanel.DockProperty, (object) ChartDock.Floating);
    this.ChartDockPanel.Children.Insert(0, (UIElement) newWatermark);
  }

  private void UpdateBitmapToolTip()
  {
    for (int index = this.VisibleSeries.Count - 1; index >= 0; --index)
    {
      ChartSeries chartSeries = this.VisibleSeries[index] as ChartSeries;
      if (chartSeries.ShowTooltip && chartSeries.IsHitTestSeries())
      {
        ChartDataPointInfo dataPoint = chartSeries.GetDataPoint(this.adorningCanvasPoint);
        if (dataPoint != null)
        {
          chartSeries.mousePos = this.adorningCanvasPoint;
          if (this.Tooltip != null && this.Tooltip.PreviousSeries != null && chartSeries.ActualTooltipPosition == TooltipPosition.Auto && !chartSeries.Equals((object) this.Tooltip.PreviousSeries))
          {
            chartSeries.RemoveTooltip();
            chartSeries.Timer.Stop();
          }
          chartSeries.UpdateSeriesTooltip((object) dataPoint);
          this.previousSeries = chartSeries;
          break;
        }
        break;
      }
      if (this.previousSeries != null && (chartSeries.ActualTooltipPosition == TooltipPosition.Pointer || !chartSeries.Timer.IsEnabled))
      {
        chartSeries.Timer.Stop();
        this.previousSeries.RemoveTooltip();
      }
    }
    this.currentBitmapPixel = -1;
  }

  private void OnSfChartSizeChanged(object sender, SizeChangedEventArgs e)
  {
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) this.Behaviors)
      behavior.OnSizeChanged(e);
  }

  private double GetOffetValue(ChartAxis axis)
  {
    return this.Legend != null && this.Legend is ChartLegend && ((this.Legend as ChartLegend).DockPosition == ChartDock.Left || (this.Legend as ChartLegend).DockPosition == ChartDock.Top) ? (axis.Orientation == Orientation.Horizontal ? this.ChartDockPanel.DesiredSize.Width - this.rootPanel.DesiredSize.Width + this.Margin.Left : this.ChartDockPanel.DesiredSize.Height - this.rootPanel.DesiredSize.Height + this.Margin.Top) : (axis.Orientation == Orientation.Horizontal ? this.Margin.Left : this.Margin.Top);
  }

  private void OnSizeChanged(object sender, SizeChangedEventArgs e)
  {
    if (!(e.NewSize != this.AvailableSize))
      return;
    this.InvalidateMeasure();
  }

  private void LayoutAxis(Size availableSize)
  {
    if (this.ChartAxisLayoutPanel != null)
    {
      this.ChartAxisLayoutPanel.UpdateElements();
      this.ChartAxisLayoutPanel.Measure(availableSize);
      this.ChartAxisLayoutPanel.Arrange(availableSize);
    }
    foreach (ChartColumnDefinition columnDefinition in (Collection<ChartColumnDefinition>) this.ColumnDefinitions)
    {
      if (this.gridLinesPanel != null && columnDefinition != null && columnDefinition.BorderLine != null && !this.gridLinesPanel.Children.Contains((UIElement) columnDefinition.BorderLine))
      {
        this.ColumnBorderLines.Add((object) columnDefinition.BorderLine);
        this.gridLinesPanel.Children.Add((UIElement) columnDefinition.BorderLine);
      }
    }
    foreach (ChartRowDefinition rowDefinition in (Collection<ChartRowDefinition>) this.RowDefinitions)
    {
      if (this.gridLinesPanel != null && rowDefinition != null && rowDefinition.BorderLine != null && !this.gridLinesPanel.Children.Contains((UIElement) rowDefinition.BorderLine))
      {
        this.RowBorderLines.Add((object) rowDefinition.BorderLine);
        this.gridLinesPanel.Children.Add((UIElement) rowDefinition.BorderLine);
      }
    }
    if (this.GridLinesLayout != null)
    {
      this.GridLinesLayout.UpdateElements();
      this.GridLinesLayout.Measure(availableSize);
      this.GridLinesLayout.Arrange(availableSize);
      this.gridLinesPanel.Measure(availableSize);
    }
    if (this.AdorningCanvas == null)
      return;
    foreach (ChartAxis ax in (Collection<ChartAxis>) this.Axes)
    {
      if (ax is NumericalAxis numericalAxis && numericalAxis.AxisScaleBreaks.Count > 0 && numericalAxis.IsSecondaryAxis)
        numericalAxis.DrawScaleBreakLines();
    }
  }

  private void fastRenderDevice_MouseMove(object sender, MouseEventArgs e)
  {
    Point position = e.GetPosition((IInputElement) this.fastRenderDevice);
    this.adorningCanvasPoint = e.GetPosition((IInputElement) this.GetAdorningCanvas());
    this.currentBitmapPixel = this.fastRenderSurface.PixelWidth * (int) position.Y + (int) position.X;
    this.UpdateBitmapToolTip();
  }
}
