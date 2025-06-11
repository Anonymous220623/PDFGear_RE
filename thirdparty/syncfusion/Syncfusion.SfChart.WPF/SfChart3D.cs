// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SfChart3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using Syncfusion.Licensing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

[ContentProperty("Series")]
public class SfChart3D : ChartBase, IDisposable
{
  public static readonly DependencyProperty DepthAxisProperty = DependencyProperty.Register(nameof (DepthAxis), typeof (ChartAxisBase3D), typeof (SfChart3D), new PropertyMetadata((object) null, new PropertyChangedCallback(SfChart3D.OnDepthAxisChanged)));
  public static readonly DependencyProperty WallSizeProperty = DependencyProperty.Register(nameof (WallSize), typeof (double), typeof (SfChart3D), new PropertyMetadata((object) 10.0));
  public static readonly DependencyProperty EnableRotationProperty = DependencyProperty.Register(nameof (EnableRotation), typeof (bool), typeof (SfChart3D), new PropertyMetadata((object) false));
  public static readonly DependencyProperty EnableSeriesSelectionProperty = DependencyProperty.Register(nameof (EnableSeriesSelection), typeof (bool), typeof (SfChart3D), new PropertyMetadata((object) false, new PropertyChangedCallback(SfChart3D.OnEnableSeriesSelectionChanged)));
  public static readonly DependencyProperty EnableSegmentSelectionProperty = DependencyProperty.Register(nameof (EnableSegmentSelection), typeof (bool), typeof (SfChart3D), new PropertyMetadata((object) true, new PropertyChangedCallback(SfChart3D.OnEnableSegmentSelectionChanged)));
  public static readonly DependencyProperty SelectionStyleProperty = DependencyProperty.Register(nameof (SelectionStyle), typeof (SelectionStyle3D), typeof (SfChart3D), new PropertyMetadata((object) SelectionStyle3D.Single, new PropertyChangedCallback(SfChart3D.OnSelectionStyleChanged)));
  public static readonly DependencyProperty TopWallBrushProperty = DependencyProperty.Register(nameof (TopWallBrush), typeof (Brush), typeof (SfChart3D), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 211, (byte) 211, (byte) 211)), new PropertyChangedCallback(SfChart3D.OnTopWallBrushChanged)));
  public static readonly DependencyProperty BottomWallBrushProperty = DependencyProperty.Register(nameof (BottomWallBrush), typeof (Brush), typeof (SfChart3D), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 211, (byte) 211, (byte) 211)), new PropertyChangedCallback(SfChart3D.OnBottomWallBrushChanged)));
  public static readonly DependencyProperty RightWallBrushProperty = DependencyProperty.Register(nameof (RightWallBrush), typeof (Brush), typeof (SfChart3D), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 211, (byte) 211, (byte) 211)), new PropertyChangedCallback(SfChart3D.OnRightWallBrushChanged)));
  public static readonly DependencyProperty LeftWallBrushProperty = DependencyProperty.Register(nameof (LeftWallBrush), typeof (Brush), typeof (SfChart3D), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 211, (byte) 211, (byte) 211)), new PropertyChangedCallback(SfChart3D.OnLeftWallBrushChanged)));
  public static readonly DependencyProperty BackWallBrushProperty = DependencyProperty.Register(nameof (BackWallBrush), typeof (Brush), typeof (SfChart3D), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 211, (byte) 211, (byte) 211)), new PropertyChangedCallback(SfChart3D.OnBackWallBrushChanged)));
  public static readonly DependencyProperty PerspectiveAngleProperty = DependencyProperty.Register(nameof (PerspectiveAngle), typeof (double), typeof (SfChart3D), new PropertyMetadata((object) 90.0, new PropertyChangedCallback(SfChart3D.OnPerspectiveAngleChanged)));
  public static readonly DependencyProperty PrimaryAxisProperty = DependencyProperty.Register(nameof (PrimaryAxis), typeof (ChartAxisBase3D), typeof (SfChart3D), new PropertyMetadata((object) null, new PropertyChangedCallback(SfChart3D.OnPrimaryAxisChanged)));
  public static readonly DependencyProperty SecondaryAxisProperty = DependencyProperty.Register(nameof (SecondaryAxis), typeof (RangeAxisBase3D), typeof (SfChart3D), new PropertyMetadata((object) null, new PropertyChangedCallback(SfChart3D.OnSecondaryAxisChanged)));
  public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(nameof (Series), typeof (ChartSeries3DCollection), typeof (SfChart3D), new PropertyMetadata((object) null, new PropertyChangedCallback(SfChart3D.OnSeriesPropertyCollectionChanged)));
  public static readonly DependencyProperty TiltProperty = DependencyProperty.Register(nameof (Tilt), typeof (double), typeof (SfChart3D), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(SfChart3D.OnTiltPropertyChanged)));
  public static readonly DependencyProperty DepthProperty = DependencyProperty.Register(nameof (Depth), typeof (double), typeof (SfChart3D), new PropertyMetadata((object) 100.0, new PropertyChangedCallback(SfChart3D.OnDepthPropertyChanged)));
  public static readonly DependencyProperty RotationProperty = DependencyProperty.Register(nameof (Rotation), typeof (double), typeof (SfChart3D), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(SfChart3D.OnRotationPropertyChanged)));
  public static readonly DependencyProperty SelectionCursorProperty = DependencyProperty.Register(nameof (SelectionCursor), typeof (Cursor), typeof (SfChart3D), new PropertyMetadata((PropertyChangedCallback) null));
  private double zminPointsDelta = double.NaN;
  private Polygon3D[] leftSideWallPlans;
  private Polygon3D[] bottomSideWallPlans;
  private Polygon3D[] topSideWallPlans;
  private Polygon3D[] rightSideWallPlans;
  private ChartSeries3D currentSeries;
  private object series;
  private Point previousChartPosition;
  private bool rotationActivated;
  private double previousAutoDepth;
  private Dictionary<int, double> sumByIndex = new Dictionary<int, double>();
  private bool is3DUpdateScheduled;
  private ChartSegment mouseMoveSegment;
  private Panel controlsPresenter;
  private bool disposed;

  public SfChart3D()
  {
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
    this.IsRenderSeriesDispatched = false;
    this.Graphics3D = new Graphics3D();
    this.ActualRotationAngle = 0.0;
    this.ActualTiltAngle = 0.0;
    this.DefaultStyleKey = (object) typeof (SfChart3D);
    this.UpdateAction = UpdateAction.Invalidate;
    this.Series = new ChartSeries3DCollection();
    this.VisibleSeries = new ChartVisibleSeriesCollection();
    this.Axes = new ChartAxisCollection();
    this.Printing = new Printing((ChartBase) this);
    if (this.ColorModel == null)
      return;
    this.ColorModel.Palette = this.Palette;
  }

  public ChartAxisBase3D DepthAxis
  {
    get => (ChartAxisBase3D) this.GetValue(SfChart3D.DepthAxisProperty);
    set => this.SetValue(SfChart3D.DepthAxisProperty, (object) value);
  }

  public double WallSize
  {
    get => (double) this.GetValue(SfChart3D.WallSizeProperty);
    set => this.SetValue(SfChart3D.WallSizeProperty, (object) value);
  }

  public bool EnableRotation
  {
    get => (bool) this.GetValue(SfChart3D.EnableRotationProperty);
    set => this.SetValue(SfChart3D.EnableRotationProperty, (object) value);
  }

  public bool EnableSeriesSelection
  {
    get => (bool) this.GetValue(SfChart3D.EnableSeriesSelectionProperty);
    set => this.SetValue(SfChart3D.EnableSeriesSelectionProperty, (object) value);
  }

  public bool EnableSegmentSelection
  {
    get => (bool) this.GetValue(SfChart3D.EnableSegmentSelectionProperty);
    set => this.SetValue(SfChart3D.EnableSegmentSelectionProperty, (object) value);
  }

  public SelectionStyle3D SelectionStyle
  {
    get => (SelectionStyle3D) this.GetValue(SfChart3D.SelectionStyleProperty);
    set => this.SetValue(SfChart3D.SelectionStyleProperty, (object) value);
  }

  public Cursor SelectionCursor
  {
    get => (Cursor) this.GetValue(SfChart3D.SelectionCursorProperty);
    set => this.SetValue(SfChart3D.SelectionCursorProperty, (object) value);
  }

  public Brush TopWallBrush
  {
    get => (Brush) this.GetValue(SfChart3D.TopWallBrushProperty);
    set => this.SetValue(SfChart3D.TopWallBrushProperty, (object) value);
  }

  public Brush BottomWallBrush
  {
    get => (Brush) this.GetValue(SfChart3D.BottomWallBrushProperty);
    set => this.SetValue(SfChart3D.BottomWallBrushProperty, (object) value);
  }

  public Brush RightWallBrush
  {
    get => (Brush) this.GetValue(SfChart3D.RightWallBrushProperty);
    set => this.SetValue(SfChart3D.RightWallBrushProperty, (object) value);
  }

  public Brush LeftWallBrush
  {
    get => (Brush) this.GetValue(SfChart3D.LeftWallBrushProperty);
    set => this.SetValue(SfChart3D.LeftWallBrushProperty, (object) value);
  }

  public Brush BackWallBrush
  {
    get => (Brush) this.GetValue(SfChart3D.BackWallBrushProperty);
    set => this.SetValue(SfChart3D.BackWallBrushProperty, (object) value);
  }

  public double PerspectiveAngle
  {
    get => (double) this.GetValue(SfChart3D.PerspectiveAngleProperty);
    set => this.SetValue(SfChart3D.PerspectiveAngleProperty, (object) value);
  }

  public ChartAxisBase3D PrimaryAxis
  {
    get => (ChartAxisBase3D) this.GetValue(SfChart3D.PrimaryAxisProperty);
    set => this.SetValue(SfChart3D.PrimaryAxisProperty, (object) value);
  }

  public RangeAxisBase3D SecondaryAxis
  {
    get => (RangeAxisBase3D) this.GetValue(SfChart3D.SecondaryAxisProperty);
    set => this.SetValue(SfChart3D.SecondaryAxisProperty, (object) value);
  }

  public ChartSeries3DCollection Series
  {
    get => (ChartSeries3DCollection) this.GetValue(SfChart3D.SeriesProperty);
    set => this.SetValue(SfChart3D.SeriesProperty, (object) value);
  }

  public double Tilt
  {
    get => (double) this.GetValue(SfChart3D.TiltProperty);
    set => this.SetValue(SfChart3D.TiltProperty, (object) value);
  }

  public double Depth
  {
    get => (double) this.GetValue(SfChart3D.DepthProperty);
    set => this.SetValue(SfChart3D.DepthProperty, (object) value);
  }

  public double Rotation
  {
    get => (double) this.GetValue(SfChart3D.RotationProperty);
    set => this.SetValue(SfChart3D.RotationProperty, (object) value);
  }

  internal bool IsRenderSeriesDispatched { get; set; }

  internal Graphics3D Graphics3D { get; set; }

  internal double ZMinPointsDelta
  {
    get
    {
      this.zminPointsDelta = double.MaxValue;
      foreach (XyzDataSeries3D series in this.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (x => x is XyzDataSeries3D)))
      {
        List<double> actualZvalues = series.ActualZValues as List<double>;
        if (series.IsIndexed && actualZvalues != null && series.IsSideBySide)
          this.GetMinPointsDelta(actualZvalues, ref this.zminPointsDelta, (ChartSeriesBase) series, series.IsIndexedZAxis);
      }
      this.zminPointsDelta = this.zminPointsDelta == double.MaxValue || this.zminPointsDelta < 0.0 ? 1.0 : this.zminPointsDelta;
      return this.zminPointsDelta;
    }
  }

  internal bool IsAutoDepth { get; set; }

  internal Canvas RootPanel { get; set; }

  internal bool IsRotationScheduleUpdate { get; set; }

  internal double ActualRotationAngle { get; set; }

  internal double ActualTiltAngle { get; set; }

  internal double ActualDepth { get; set; }

  public override void SeriesSelectedIndexChanged(int newIndex, int oldIndex)
  {
    if (oldIndex < this.Series.Count && oldIndex >= 0 && this.SelectionStyle == SelectionStyle3D.Single)
    {
      ChartSeriesBase series = (ChartSeriesBase) this.Series[oldIndex];
      if (this.SelectedSeriesCollection.Contains(series))
        this.SelectedSeriesCollection.Remove(series);
      SfChart3D.OnResetSeries(series);
    }
    if (newIndex >= 0 && newIndex < this.Series.Count && this.GetEnableSeriesSelection())
    {
      ChartSeriesBase chartSeriesBase = (ChartSeriesBase) this.Series[newIndex];
      if (!this.SelectedSeriesCollection.Contains(chartSeriesBase))
        this.SelectedSeriesCollection.Add(chartSeriesBase);
      foreach (ChartSegment3D segment in (Collection<ChartSegment>) chartSeriesBase.Segments)
      {
        segment.BindProperties();
        foreach (Polygon3D polygon in segment.Polygons)
        {
          polygon.Fill = segment.Interior;
          polygon.ReDraw();
        }
      }
      ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
      {
        SelectedSegment = (ChartSegment) null,
        SelectedSeries = chartSeriesBase,
        SelectedSeriesCollection = this.SelectedSeriesCollection,
        SelectedIndex = newIndex,
        PreviousSelectedIndex = oldIndex,
        IsDataPointSelection = false,
        IsSelected = true,
        PreviousSelectedSeries = (ChartSeriesBase) null,
        PreviousSelectedSegment = (ChartSegment) null
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
        PreviousSelectedSeries = (ChartSeriesBase) this.Series[oldIndex],
        PreviousSelectedSegment = (ChartSegment) null,
        IsSelected = false
      });
    }
  }

  public override double PointToValue(ChartAxis axis, Point point)
  {
    Polygon3D plane1 = new Polygon3D(new Vector3D(0.0, 0.0, 1.0), 0.0);
    ChartTransform.ChartTransform3D transform = this.Graphics3D.Transform;
    plane1.Transform(transform.View);
    Vector3D plane2 = transform.ToPlane(point, plane1);
    return base.PointToValue(axis, new Point(plane2.X, plane2.Y));
  }

  public override double ValueToPoint(ChartAxis axis, double value)
  {
    double point = base.ValueToPoint(axis, value);
    return axis.Orientation == Orientation.Horizontal ? this.Graphics3D.Transform.ToScreen(new Vector3D(point, 0.0, 0.0)).X : this.Graphics3D.Transform.ToScreen(new Vector3D(0.0, point, 0.0)).Y;
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
    if (this.currentSeries != null)
    {
      this.currentSeries.Area = (SfChart3D) null;
      this.currentSeries = (ChartSeries3D) null;
    }
    if (this.series is ChartSeries)
    {
      (this.series as ChartSeries3D).Area = (SfChart3D) null;
      this.series = (object) null;
    }
    if (this.mouseMoveSegment != null)
    {
      this.mouseMoveSegment.Series = (ChartSeriesBase) null;
      this.mouseMoveSegment = (ChartSegment) null;
    }
    this.DisposeSeries();
    this.DisposeSelectionEvents();
    this.DisposeLegend();
    this.DisposeAxis();
    this.DisposeRowColumnsDefinitions();
    this.DisposePanels();
    this.SizeChanged -= new SizeChangedEventHandler(this.OnSizeChanged);
    this.RootPanelDesiredSize = new Size?();
    this.GridLinesLayout = (ILayoutCalculator) null;
    this.ChartAxisLayoutPanel = (ILayoutCalculator) null;
    this.SelectionBehaviour = (ChartSelectionBehavior) null;
    this.TooltipBehavior = (ChartTooltipBehavior) null;
    this.RootPanel = (Canvas) null;
    if (this.Printing == null)
      return;
    this.Printing.Chart = (ChartBase) null;
    this.Printing = (Printing) null;
  }

  private void DisposeSeries()
  {
    if (this.VisibleSeries != null)
      this.VisibleSeries.Clear();
    if (this.ActualSeries != null)
      this.ActualSeries.Clear();
    if (this.SelectedSeriesCollection != null)
      this.SelectedSeriesCollection.Clear();
    this.CurrentSelectedSeries = (ChartSeriesBase) null;
    this.PreviousSelectedSeries = (ChartSeriesBase) null;
    if (this.Series == null)
      return;
    foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeries3D>) this.Series)
      chartSeriesBase.Dispose();
    this.Series.Clear();
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
    if (this.Legend is ChartLegend)
    {
      (this.Legend as ChartLegend).Dispose();
      this.Legend = (object) null;
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

  private void DisposeAxis()
  {
    if (this.Axes != null)
    {
      foreach (ChartAxis ax in (Collection<ChartAxis>) this.Axes)
        ax.Dispose();
      this.Axes.Clear();
      this.Axes = (ChartAxisCollection) null;
    }
    this.PrimaryAxis = (ChartAxisBase3D) null;
    this.SecondaryAxis = (RangeAxisBase3D) null;
    this.InternalPrimaryAxis = (ChartAxis) null;
    this.InternalSecondaryAxis = (ChartAxis) null;
    this.InternalDepthAxis = (ChartAxis) null;
  }

  private void DisposePanels()
  {
    if (this.ChartAxisLayoutPanel is ChartCartesianAxisLayoutPanel chartAxisLayoutPanel)
    {
      chartAxisLayoutPanel.Area = (ChartBase) null;
      if (chartAxisLayoutPanel.Children != null)
        chartAxisLayoutPanel.Children.Clear();
    }
    if (this.GridLinesLayout is ChartCartesianGridLinesPanel gridLinesLayout)
    {
      gridLinesLayout.Area = (ChartBase) null;
      if (gridLinesLayout.Children != null)
        gridLinesLayout.Children.Clear();
    }
    if (this.ChartDockPanel == null || this.ChartDockPanel.Children.Count <= 0)
      return;
    this.ChartDockPanel.RootElement = (UIElement) null;
    if (this.ChartDockPanel.Children != null)
      this.ChartDockPanel.Children.Clear();
    this.ChartDockPanel = (ChartDockPanel) null;
  }

  internal static void OnResetSeries(ChartSeriesBase series)
  {
    foreach (ChartSegment3D segment in (Collection<ChartSegment>) series.Segments)
    {
      segment.BindProperties();
      foreach (Polygon3D polygon in segment.Polygons)
      {
        polygon.Fill = segment.Interior;
        polygon.ReDraw();
      }
    }
  }

  internal void InitializeDefaultAxes()
  {
    if (this.PrimaryAxis == null || this.PrimaryAxis.IsDefault)
    {
      if (this.Series != null && this.Series.Count == 0)
      {
        if (this.PrimaryAxis == null)
        {
          NumericalAxis3D numericalAxis3D = new NumericalAxis3D();
          numericalAxis3D.IsDefault = true;
          this.PrimaryAxis = (ChartAxisBase3D) numericalAxis3D;
        }
      }
      else
      {
        List<ChartValueType> list = this.Series.Where<ChartSeries3D>((Func<ChartSeries3D, bool>) (series => series.ActualXAxis == null || series.ActualXAxis.IsDefault || !this.Axes.Contains(series.ActualXAxis))).Select<ChartSeries3D, ChartValueType>((Func<ChartSeries3D, ChartValueType>) (series => series.XAxisValueType)).ToList<ChartValueType>();
        if (list.Count > 0)
          this.SetPrimaryAxis(list[0]);
        else
          this.InternalPrimaryAxis = this.Series[0].ActualXAxis;
      }
    }
    if (this.SecondaryAxis == null || this.SecondaryAxis.IsDefault)
    {
      if (this.Series != null && this.Series.Count == 0)
      {
        if (this.SecondaryAxis == null)
        {
          NumericalAxis3D numericalAxis3D = new NumericalAxis3D();
          numericalAxis3D.IsDefault = true;
          this.SecondaryAxis = (RangeAxisBase3D) numericalAxis3D;
        }
      }
      else if (this.Series.Where<ChartSeries3D>((Func<ChartSeries3D, bool>) (series => series.ActualYAxis == null || series.ActualYAxis.IsDefault || !this.Axes.Contains(series.ActualYAxis))).ToList<ChartSeries3D>().Count > 0 && this.SecondaryAxis == null)
      {
        NumericalAxis3D numericalAxis3D = new NumericalAxis3D();
        numericalAxis3D.IsDefault = true;
        this.SecondaryAxis = (RangeAxisBase3D) numericalAxis3D;
      }
      else
        this.InternalSecondaryAxis = this.Series[0].ActualYAxis;
    }
    if (this.DepthAxis == null || this.DepthAxis.IsDefault)
    {
      if (this.Series.Count > 0 && this.Series.Any<ChartSeries3D>((Func<ChartSeries3D, bool>) (x => x is XyzDataSeries3D && !string.IsNullOrEmpty((x as XyzDataSeries3D).ZBindingPath))))
      {
        if (this.Series != null && this.Series.Count == 0)
        {
          if (this.DepthAxis == null)
          {
            NumericalAxis3D numericalAxis3D = new NumericalAxis3D();
            numericalAxis3D.IsDefault = true;
            this.DepthAxis = (ChartAxisBase3D) numericalAxis3D;
          }
        }
        else
        {
          List<ChartValueType> list = this.Series.Where<ChartSeries3D>((Func<ChartSeries3D, bool>) (series => series is XyzDataSeries3D && (series as XyzDataSeries3D).ActualZAxis == null || (series as XyzDataSeries3D).ActualZAxis.IsDefault || !this.Axes.Contains((series as XyzDataSeries3D).ActualZAxis))).Select<ChartSeries3D, ChartValueType>((Func<ChartSeries3D, ChartValueType>) (series => (series as XyzDataSeries3D).ZAxisValueType)).ToList<ChartValueType>();
          if (list.Count > 0)
            this.SetDepthAxis(list[0]);
          else
            this.InternalDepthAxis = (this.Series[0] as XyzDataSeries3D).ActualZAxis;
        }
      }
    }
    else if (this.IsManhattanAxis() && this.DepthAxis != null && !(this.DepthAxis is CategoryAxis3D))
    {
      CategoryAxis3D categoryAxis3D = new CategoryAxis3D();
      categoryAxis3D.IsDefault = true;
      this.DepthAxis = (ChartAxisBase3D) categoryAxis3D;
    }
    this.IsUpdateLegend = true;
  }

  internal bool IsManhattanAxis()
  {
    return this.VisibleSeries.Count > 1 && this.VisibleSeries.All<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series is AreaSeries3D || series is LineSeries3D));
  }

  internal void SetPrimaryAxis(ChartValueType type)
  {
    switch (type)
    {
      case ChartValueType.Double:
        if (this.PrimaryAxis != null && !(this.PrimaryAxis.GetType() != typeof (NumericalAxis3D)))
          break;
        NumericalAxis3D numericalAxis3D = new NumericalAxis3D();
        numericalAxis3D.IsDefault = true;
        this.PrimaryAxis = (ChartAxisBase3D) numericalAxis3D;
        break;
      case ChartValueType.DateTime:
        if (this.PrimaryAxis != null && !(this.PrimaryAxis.GetType() != typeof (DateTimeAxis3D)))
          break;
        DateTimeAxis3D dateTimeAxis3D = new DateTimeAxis3D();
        dateTimeAxis3D.IsDefault = true;
        this.PrimaryAxis = (ChartAxisBase3D) dateTimeAxis3D;
        break;
      case ChartValueType.String:
        if (this.PrimaryAxis != null && !(this.PrimaryAxis.GetType() != typeof (CategoryAxis3D)))
          break;
        CategoryAxis3D categoryAxis3D = new CategoryAxis3D();
        categoryAxis3D.IsDefault = true;
        this.PrimaryAxis = (ChartAxisBase3D) categoryAxis3D;
        break;
      case ChartValueType.TimeSpan:
        if (this.PrimaryAxis != null && !(this.PrimaryAxis.GetType() != typeof (TimeSpanAxis3D)))
          break;
        TimeSpanAxis3D timeSpanAxis3D = new TimeSpanAxis3D();
        timeSpanAxis3D.IsDefault = true;
        this.PrimaryAxis = (ChartAxisBase3D) timeSpanAxis3D;
        break;
    }
  }

  internal void SetDepthAxis(ChartValueType type)
  {
    switch (type)
    {
      case ChartValueType.Double:
        if (this.DepthAxis != null && !(this.DepthAxis.GetType() != typeof (NumericalAxis3D)))
          break;
        NumericalAxis3D numericalAxis3D = new NumericalAxis3D();
        numericalAxis3D.IsDefault = true;
        this.DepthAxis = (ChartAxisBase3D) numericalAxis3D;
        break;
      case ChartValueType.DateTime:
        if (this.DepthAxis != null && !(this.DepthAxis.GetType() != typeof (DateTimeAxis3D)))
          break;
        DateTimeAxis3D dateTimeAxis3D = new DateTimeAxis3D();
        dateTimeAxis3D.IsDefault = true;
        this.DepthAxis = (ChartAxisBase3D) dateTimeAxis3D;
        break;
      case ChartValueType.String:
        if (this.DepthAxis != null && !(this.DepthAxis.GetType() != typeof (CategoryAxis3D)))
          break;
        CategoryAxis3D categoryAxis3D = new CategoryAxis3D();
        categoryAxis3D.IsDefault = true;
        this.DepthAxis = (ChartAxisBase3D) categoryAxis3D;
        break;
      case ChartValueType.TimeSpan:
        if (this.DepthAxis != null && !(this.DepthAxis.GetType() != typeof (TimeSpanAxis3D)))
          break;
        TimeSpanAxis3D timeSpanAxis3D = new TimeSpanAxis3D();
        timeSpanAxis3D.IsDefault = true;
        this.DepthAxis = (ChartAxisBase3D) timeSpanAxis3D;
        break;
    }
  }

  internal override DependencyObject CloneChart()
  {
    SfChart3D copy = new SfChart3D();
    ChartCloning.CloneControl((Control) this, (Control) copy);
    copy.Height = double.IsNaN(this.Height) ? this.ActualHeight : this.Height;
    copy.Width = double.IsNaN(this.Width) ? this.ActualWidth : this.Width;
    copy.Header = this.Header;
    copy.Palette = this.Palette;
    copy.SideBySideSeriesPlacement = this.SideBySideSeriesPlacement;
    copy.EnableRotation = this.EnableRotation;
    copy.EnableSegmentSelection = this.EnableSegmentSelection;
    copy.EnableSeriesSelection = this.EnableSeriesSelection;
    copy.LeftWallBrush = this.LeftWallBrush;
    copy.SelectionStyle = this.SelectionStyle;
    copy.SelectionCursor = this.SelectionCursor;
    if (this.PrimaryAxis != null && this.PrimaryAxis != null)
      copy.PrimaryAxis = (ChartAxisBase3D) this.PrimaryAxis.Clone();
    if (this.SecondaryAxis != null && this.SecondaryAxis != null)
      copy.SecondaryAxis = (RangeAxisBase3D) this.SecondaryAxis.Clone();
    if (this.Legend != null)
      copy.Legend = (object) (ChartLegend) (this.Legend as ICloneable).Clone();
    foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeries3D>) this.Series)
      copy.Series.Add((ChartSeries3D) chartSeriesBase.Clone());
    foreach (ChartRowDefinition rowDefinition in (Collection<ChartRowDefinition>) this.RowDefinitions)
      copy.RowDefinitions.Add((ChartRowDefinition) rowDefinition.Clone());
    foreach (ChartColumnDefinition columnDefinition in (Collection<ChartColumnDefinition>) this.ColumnDefinitions)
      copy.ColumnDefinitions.Add((ChartColumnDefinition) columnDefinition.Clone());
    copy.UpdateArea(true);
    return (DependencyObject) copy;
  }

  internal double GetPercentByIndex(
    List<StackingSeriesBase3D> series,
    int index,
    double value,
    bool isReCalculation)
  {
    if (this.sumByIndex.Keys.Contains<int>(index) && !isReCalculation)
      return value / this.sumByIndex[index] * 100.0;
    double num = series.Sum<StackingSeriesBase3D>((Func<StackingSeriesBase3D, double>) (item => (double) item.YValues.Count == 0.0 || index >= item.YValues.Count ? 0.0 : Math.Abs(double.IsNaN(item.YValues[index]) ? 0.0 : item.YValues[index])));
    this.sumByIndex[index] = num;
    return value / this.sumByIndex[index] * 100.0;
  }

  internal void RenderSeries()
  {
    if (this.RootPanelDesiredSize.HasValue)
    {
      this.Update3DWall();
      Size size = this.RootPanelDesiredSize.Value;
      if (this.VisibleSeries != null)
      {
        foreach (ChartSeries3D chartSeries3D in (Collection<ChartSeriesBase>) this.VisibleSeries)
        {
          chartSeries3D.UpdateOnSeriesBoundChanged(size);
          if (chartSeries3D.AdornmentsInfo != null)
            chartSeries3D.AdornmentsInfo.Arrange(size);
          foreach (ChartSegment3D chartSegment3D in chartSeries3D.Segments.OfType<ChartSegment3D>())
            chartSegment3D.Polygons.Clear();
        }
      }
      this.Graphics3D.PrepareView(this.PerspectiveAngle, this.ActualDepth, this.ActualRotationAngle, this.ActualTiltAngle, size);
      this.Graphics3D.View((Panel) this.RootPanel);
      foreach (ChartSeriesBase chartSeriesBase in this.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (item => item.CanAnimate && item.Segments.Count > 0)))
      {
        chartSeriesBase.Animate();
        chartSeriesBase.CanAnimate = false;
      }
    }
    this.IsRenderSeriesDispatched = false;
    this.StackedValues = (Dictionary<object, StackingValues>) null;
  }

  internal void UpdateRightWall()
  {
    Rect rect = new Rect(-this.ActualDepth, this.SeriesClipRect.Top, this.ActualDepth, this.SeriesClipRect.Height);
    this.rightSideWallPlans = Polygon3D.CreateBox(new Vector3D(rect.Left, rect.Top, this.SeriesClipRect.Right + 1.5), new Vector3D(rect.Right, rect.Bottom, this.SeriesClipRect.Right + this.WallSize), (DependencyObject) this, 0, this.Graphics3D, this.RightWallBrush, this.RightWallBrush, 0.0, false, "RightWallBrush");
    foreach (Polygon3D rightSideWallPlan in this.rightSideWallPlans)
      rightSideWallPlan.Transform(Matrix3D.Turn(-1.5707963705062866));
  }

  internal void UpdateLeftWall()
  {
    Rect rect = new Rect(-this.ActualDepth, this.SeriesClipRect.Top, this.ActualDepth, this.SeriesClipRect.Height);
    double left = this.SeriesClipRect.Left;
    this.leftSideWallPlans = Polygon3D.CreateBox(new Vector3D(rect.Left, rect.Top, left - 0.1), new Vector3D(rect.Right, rect.Bottom, left - this.WallSize), (DependencyObject) this, 0, this.Graphics3D, this.LeftWallBrush, this.LeftWallBrush, 0.0, false, "LeftWallBrush");
    foreach (Polygon3D leftSideWallPlan in this.leftSideWallPlans)
      leftSideWallPlan.Transform(Matrix3D.Turn(-1.5707963705062866));
  }

  internal void UpdateTopWall()
  {
    Rect rect = new Rect(this.SeriesClipRect.Left, (double) -(int) this.ActualDepth, this.SeriesClipRect.Width, (double) (int) this.ActualDepth);
    double vz = this.WallSize >= this.SeriesClipRect.Top ? -(this.WallSize - this.SeriesClipRect.Top) : this.SeriesClipRect.Top - this.WallSize;
    this.topSideWallPlans = Polygon3D.CreateBox(new Vector3D(rect.Right, rect.Top, this.SeriesClipRect.Top - 0.1), new Vector3D(rect.Left, rect.Bottom - 0.1, vz), (DependencyObject) this, 0, this.Graphics3D, this.TopWallBrush, this.TopWallBrush, 0.0, false, "TopWallBrush");
    foreach (Polygon3D topSideWallPlan in this.topSideWallPlans)
      topSideWallPlan.Transform(Matrix3D.Tilt(1.5707963705062866));
  }

  internal void UpdateBottomWall()
  {
    Rect rect = new Rect(this.SeriesClipRect.Left, (double) -(int) this.ActualDepth, this.SeriesClipRect.Width, (double) (int) this.ActualDepth);
    Vector3D vector2 = new Vector3D(rect.Right, rect.Top, this.WallSize + this.SeriesClipRect.Height);
    this.bottomSideWallPlans = Polygon3D.CreateBox(new Vector3D(rect.Left, rect.Bottom - 0.1, this.SeriesClipRect.Top + this.SeriesClipRect.Height + 1.0), vector2, (DependencyObject) this, 0, this.Graphics3D, this.BottomWallBrush, this.BottomWallBrush, 0.0, false, "BottomWallBrush");
    foreach (Polygon3D bottomSideWallPlan in this.bottomSideWallPlans)
      bottomSideWallPlan.Transform(Matrix3D.Tilt(1.5707963705062866));
  }

  internal void UpdateBackWall()
  {
    Rect seriesClipRect = this.SeriesClipRect;
    Polygon3D.CreateBox(new Vector3D(seriesClipRect.Left, seriesClipRect.Top, this.ActualDepth == 0.0 ? 1.5 : this.ActualDepth + this.WallSize), new Vector3D(seriesClipRect.Right, seriesClipRect.Bottom, this.ActualDepth == 0.0 ? 1.5 : this.ActualDepth), (DependencyObject) this, 0, this.Graphics3D, this.BackWallBrush, this.BackWallBrush, 0.0, false, "BackWallBrush");
  }

  internal bool IsChartRotated()
  {
    double num1 = Math.Abs(this.Tilt % 360.0);
    double num2 = Math.Abs(this.Rotation % 360.0);
    return ((num1 <= 90.0 ? 0 : (num1 < 270.0 ? 1 : 0)) ^ (num2 <= 90.0 ? 0 : (num2 < 270.0 ? 1 : 0))) != 0;
  }

  internal override void UpdateAxisLayoutPanels()
  {
    if (this.AreaType == ChartAreaType.CartesianAxes)
    {
      this.ChartAxisLayoutPanel = (ILayoutCalculator) new ChartCartesianAxisLayoutPanel(this.controlsPresenter)
      {
        Area = (ChartBase) this
      };
      this.GridLinesLayout = (ILayoutCalculator) new ChartCartesianGridLinesPanel((Panel) null)
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

  internal override void UpdateArea(bool forceUpdate)
  {
    if (!this.isUpdateDispatched && !forceUpdate)
      return;
    this.sumByIndex.Clear();
    this.Graphics3D.ClearVisual();
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
      if (this.AreaType == ChartAreaType.CartesianAxes)
      {
        foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) this.VisibleSeries)
        {
          ISupportAxes supportAxes = chartSeriesBase as ISupportAxes;
          if (this.InternalPrimaryAxis != null && !this.InternalPrimaryAxis.RegisteredSeries.Contains(supportAxes))
            this.InternalPrimaryAxis.RegisteredSeries.Add(supportAxes);
          if (this.InternalSecondaryAxis != null && !this.InternalSecondaryAxis.RegisteredSeries.Contains(supportAxes))
            this.InternalSecondaryAxis.RegisteredSeries.Add(supportAxes);
          if (this.InternalDepthAxis != null && !this.InternalDepthAxis.RegisteredSeries.Contains(supportAxes))
            this.InternalDepthAxis.RegisteredSeries.Add(supportAxes);
        }
      }
      this.InitializeDefaultAxes();
      if (this.InternalPrimaryAxis is CategoryAxis3D && !(this.InternalPrimaryAxis as CategoryAxis3D).IsIndexed)
        CategoryAxisHelper.GroupData(this.VisibleSeries);
      foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) this.VisibleSeries)
        chartSeriesBase.Invalidate();
      if (this.ShowTooltip && this.Tooltip == null)
        this.Tooltip = new ChartTooltip();
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
        else if (!this.IsRenderSeriesDispatched)
          this.RenderSeries();
      }
    }
    this.UpdateAction = UpdateAction.Invalidate;
    this.isUpdateDispatched = false;
    this.IsRotationScheduleUpdate = false;
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    if (double.IsInfinity(availableSize.Width) || double.IsInfinity(availableSize.Height))
    {
      this.SizeChanged -= new SizeChangedEventHandler(this.OnSizeChanged);
      this.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
      this.AvailableSize = new Size(this.ActualWidth == 0.0 ? 500.0 : this.ActualWidth, this.ActualHeight == 0.0 ? 500.0 : this.ActualHeight);
    }
    else
      this.AvailableSize = availableSize;
    return base.MeasureOverride(this.AvailableSize);
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    this.ChartMouseMove(e.OriginalSource, e.GetPosition((IInputElement) this.AdorningCanvas));
    this.RotateChart(e.GetPosition((IInputElement) this));
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    base.OnMouseLeftButtonDown(e);
    this.ChartMouseDown(e.OriginalSource, e.GetPosition((IInputElement) this), (object) null);
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    this.OnMouseRightButtonUp(e);
    this.ChartMouseUp(e.OriginalSource, e.GetPosition((IInputElement) this), (object) null);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    if (this == null || Mouse.OverrideCursor == null)
      return;
    Mouse.OverrideCursor = (Cursor) null;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.RootPanel = this.GetTemplateChild("PART_3DPanel") as Canvas;
    this.AdorningCanvas = this.GetTemplateChild("PART_adorningCanvas") as Canvas;
    this.ChartDockPanel = this.GetTemplateChild("Part_DockPanel") as ChartDockPanel;
    (this.GetTemplateChild("Part_LayoutRoot") as ChartRootPanel).Area = (ChartBase) this;
    this.controlsPresenter = this.GetTemplateChild("Part_ControlsPanel") as Panel;
    this.UpdateAxisLayoutPanels();
    foreach (UIElement element in this.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (visibleSeries => this.controlsPresenter != null)))
      this.controlsPresenter.Children.Add(element);
    if (this.Legend is ChartLegendCollection legend)
    {
      this.LegendItems = new ObservableCollection<ObservableCollection<LegendItem>>();
      for (int index = 0; index < legend.Count; ++index)
        this.LegendItems.Add(new ObservableCollection<LegendItem>());
    }
    else
    {
      ObservableCollection<ObservableCollection<LegendItem>> observableCollection = new ObservableCollection<ObservableCollection<LegendItem>>();
      observableCollection.Add(new ObservableCollection<LegendItem>());
      this.LegendItems = observableCollection;
    }
    this.UpdateLegend(this.Legend, true);
    this.IsTemplateApplied = true;
  }

  private static bool CheckRotationAngleSameQuadrant(double actualRotation, double previousRotation)
  {
    if (actualRotation >= 0.0 && actualRotation < 45.0 && previousRotation >= 0.0 && previousRotation < 45.0 || actualRotation >= 45.0 && actualRotation < 90.0 && previousRotation >= 45.0 && previousRotation < 90.0 || actualRotation >= 90.0 && actualRotation < 135.0 && previousRotation >= 90.0 && previousRotation < 135.0 || actualRotation >= 135.0 && actualRotation < 180.0 && previousRotation >= 135.0 && previousRotation < 180.0 || actualRotation >= 180.0 && actualRotation < 225.0 && previousRotation >= 180.0 && previousRotation < 225.0 || actualRotation >= 225.0 && actualRotation < 270.0 && previousRotation >= 225.0 && previousRotation < 270.0 || actualRotation >= 270.0 && actualRotation < 315.0 && previousRotation >= 270.0 && previousRotation < 315.0)
      return true;
    return actualRotation >= 315.0 && actualRotation < 360.0 && previousRotation >= 315.0 && previousRotation < 360.0;
  }

  private static bool CheckTiltAngleSameQuadrant(double actualtilt, double previousTilt)
  {
    if (actualtilt >= 0.0 && actualtilt < 45.0 && previousTilt >= 0.0 && previousTilt < 45.0 || actualtilt >= 45.0 && actualtilt < 90.0 && previousTilt >= 45.0 && previousTilt < 90.0 || actualtilt >= 90.0 && actualtilt < 135.0 && previousTilt >= 90.0 && previousTilt < 135.0 || actualtilt >= 135.0 && actualtilt < 180.0 && previousTilt >= 135.0 && previousTilt < 180.0 || actualtilt >= 180.0 && actualtilt < 225.0 && previousTilt >= 180.0 && previousTilt < 225.0 || actualtilt >= 225.0 && actualtilt < 270.0 && previousTilt >= 225.0 && previousTilt < 270.0 || actualtilt >= 270.0 && actualtilt < 315.0 && previousTilt >= 270.0 && previousTilt < 315.0)
      return true;
    return actualtilt >= 315.0 && actualtilt < 360.0 && previousTilt >= 315.0 && previousTilt < 360.0;
  }

  private static void CheckSeriesTransposition(ChartSeries3D series)
  {
    if (series.ActualXAxis == null || series.ActualYAxis == null)
      return;
    series.ActualXAxis.Orientation = series.IsActualTransposed ? Orientation.Vertical : Orientation.Horizontal;
    series.ActualYAxis.Orientation = series.IsActualTransposed ? Orientation.Horizontal : Orientation.Vertical;
  }

  private static bool IsSeriesEventTrigger(object source, ChartSeriesBase series)
  {
    FrameworkElement reference = source as FrameworkElement;
    if (series != null && series.Adornments.Count > 0)
    {
      while (!(reference is ChartAdornmentContainer) && reference != null)
      {
        reference = VisualTreeHelper.GetParent((DependencyObject) reference) as FrameworkElement;
        if (reference is ContentControl contentControl && contentControl.Tag != null)
        {
          if (series.adornmentInfo.LabelPresenters.Count > 0 && series.adornmentInfo.LabelPresenters.Contains<FrameworkElement>(reference))
            return true;
        }
        else if (reference is ChartAdornmentContainer)
          return series.adornmentInfo.adormentContainers.Count > 0 && ((IEnumerable<FrameworkElement>) series.adornmentInfo.adormentContainers).Contains<FrameworkElement>(reference);
      }
    }
    return false;
  }

  private static void OnDepthAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ChartAxisBase3D newValue = e.NewValue as ChartAxisBase3D;
    SfChart3D sfChart3D = d as SfChart3D;
    if (newValue != null)
    {
      newValue.IsZAxis = true;
      newValue.Orientation = Orientation.Horizontal;
      sfChart3D.InternalDepthAxis = (ChartAxis) e.NewValue;
    }
    sfChart3D?.OnAxisChanged(e);
  }

  private static void OnEnableSeriesSelectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (d is SfChart3D sfChart3D && !(bool) e.NewValue)
    {
      foreach (ChartSeries3D series in (Collection<ChartSeriesBase>) sfChart3D.VisibleSeries)
      {
        if (sfChart3D.SelectedSeriesCollection.Contains((ChartSeriesBase) series))
        {
          sfChart3D.SelectedSeriesCollection.Remove((ChartSeriesBase) series);
          SfChart3D.OnResetSeries((ChartSeriesBase) series);
        }
      }
      sfChart3D.SeriesSelectedIndex = -1;
      sfChart3D.SelectedSeriesCollection.Clear();
    }
    else
    {
      if (sfChart3D == null || !(bool) e.NewValue || sfChart3D.SeriesSelectedIndex == -1)
        return;
      sfChart3D.SeriesSelectedIndexChanged(sfChart3D.SeriesSelectedIndex, -1);
    }
  }

  private static void OnEnableSegmentSelectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfChart3D sfChart3D))
      return;
    if (!(bool) e.NewValue)
    {
      foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) sfChart3D.VisibleSeries)
      {
        for (int index = 0; index < chartSeriesBase.ActualData.Count; ++index)
        {
          if (chartSeriesBase.SelectedSegmentsIndexes.Contains(index))
          {
            chartSeriesBase.SelectedSegmentsIndexes.Remove(index);
            chartSeriesBase.OnResetSegment(index);
          }
        }
        if (chartSeriesBase is ChartSeries3D chartSeries3D)
          chartSeries3D.SelectedIndex = -1;
        chartSeriesBase.SelectedSegmentsIndexes.Clear();
      }
    }
    else
    {
      for (int index = 0; index < sfChart3D.VisibleSeries.Count; ++index)
      {
        ChartSeriesBase chartSeriesBase = sfChart3D.VisibleSeries[index];
        if (chartSeriesBase is ChartSeries3D chartSeries3D && chartSeries3D.SelectedIndex != -1)
          chartSeriesBase.SelectedIndexChanged(chartSeries3D.SelectedIndex, -1);
      }
    }
  }

  private static void OnSelectionStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfChart3D sfChart3D) || sfChart3D.Series == null || sfChart3D.Series.Count == 0)
      return;
    foreach (ChartSeries3D series in (Collection<ChartSeries3D>) sfChart3D.Series)
    {
      if (sfChart3D.SelectedSeriesCollection.Contains((ChartSeriesBase) series))
      {
        sfChart3D.SelectedSeriesCollection.Remove((ChartSeriesBase) series);
        SfChart3D.OnResetSeries((ChartSeriesBase) series);
      }
      for (int index = 0; index < series.ActualData.Count; ++index)
      {
        if (series.SelectedSegmentsIndexes.Contains(index))
        {
          series.SelectedSegmentsIndexes.Remove(index);
          series.OnResetSegment(index);
        }
      }
      series.SelectedIndex = -1;
    }
    sfChart3D.SeriesSelectedIndex = -1;
  }

  private static void OnTopWallBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(d is SfChart3D sfChart3D) || args.NewValue == args.OldValue)
      return;
    sfChart3D.OnTopWallBrushChanged();
  }

  private static void OnBottomWallBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(d is SfChart3D sfChart3D) || args.NewValue == args.OldValue)
      return;
    sfChart3D.OnBottomWallBrushChanged();
  }

  private static void OnRightWallBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(d is SfChart3D sfChart3D) || args.NewValue == args.OldValue)
      return;
    sfChart3D.OnRightWallBrushChanged();
  }

  private static void OnLeftWallBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(d is SfChart3D sfChart3D) || args.NewValue == args.OldValue)
      return;
    sfChart3D.OnLeftWallBrushChanged();
  }

  private static void OnBackWallBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(d is SfChart3D sfChart3D) || args.NewValue == args.OldValue)
      return;
    sfChart3D.OnBackWallBrushChanged();
  }

  private static void OnPerspectiveAngleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(d is SfChart3D sfChart3D))
      return;
    sfChart3D.OnPerspectiveAngleChanged();
  }

  private static void OnPrimaryAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ChartAxis newValue = e.NewValue as ChartAxis;
    SfChart3D sfChart3D = d as SfChart3D;
    if (newValue != null)
    {
      newValue.Orientation = Orientation.Horizontal;
      if (sfChart3D != null)
        sfChart3D.InternalPrimaryAxis = (ChartAxis) e.NewValue;
    }
    sfChart3D?.OnAxisChanged(e);
  }

  private static void OnSecondaryAxisChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfChart3D sfChart3D))
      return;
    if (e.NewValue != null)
    {
      sfChart3D.InternalSecondaryAxis = (ChartAxis) e.NewValue;
      ((ChartAxis) e.NewValue).Orientation = Orientation.Vertical;
    }
    sfChart3D.OnAxisChanged(e);
  }

  private static void OnSeriesPropertyCollectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((SfChart3D) d).OnSeriesPropertyCollectionChanged(e);
  }

  private static void OnDepthPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    ((ChartBase) d).ScheduleUpdate();
  }

  private static void OnRotationPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    SfChart3D sfChart3D = (SfChart3D) d;
    sfChart3D.IsRotationScheduleUpdate = true;
    double num1 = (double) e.NewValue % 360.0;
    double actualRotation = sfChart3D.ActualRotationAngle = num1 < 0.0 ? num1 + 360.0 : num1;
    double num2 = (double) e.OldValue % 360.0;
    double previousRotation = num2 < 0.0 ? num2 + 360.0 : num2;
    if (sfChart3D.ChartAxisLayoutPanel == null || SfChart3D.CheckRotationAngleSameQuadrant(actualRotation, previousRotation))
      sfChart3D.Schedule3DUpdate();
    else
      sfChart3D?.ScheduleUpdate();
  }

  private static void OnTiltPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    SfChart3D sfChart3D = (SfChart3D) d;
    double num1 = (double) e.NewValue % 360.0;
    double actualtilt = sfChart3D.ActualTiltAngle = num1 < 0.0 ? num1 + 360.0 : num1;
    double num2 = (double) e.OldValue % 360.0;
    double previousTilt = num2 < 0.0 ? num2 + 360.0 : num2;
    if (sfChart3D.ChartAxisLayoutPanel == null || SfChart3D.CheckTiltAngleSameQuadrant(actualtilt, previousTilt))
      sfChart3D.Schedule3DUpdate();
    else
      sfChart3D?.ScheduleUpdate();
  }

  private void OnBackWallBrushChanged()
  {
    if (this.Graphics3D == null || this.Graphics3D.GetVisualCount() <= 0)
      return;
    List<Polygon3D> visual = this.Graphics3D.GetVisual();
    foreach (Polygon3D polygon3D in visual.Where<Polygon3D>((Func<Polygon3D, bool>) (item => item.Name != null && item.Name.Contains("BackWallBrush"))).ToList<Polygon3D>())
      visual.Remove(polygon3D);
    this.UpdateBackWall();
    this.Graphics3D.PrepareView();
    this.Graphics3D.View((Panel) this.RootPanel);
  }

  private void OnLeftWallBrushChanged()
  {
    if (this.leftSideWallPlans == null || this.Graphics3D == null || this.Graphics3D.GetVisualCount() <= 0)
      return;
    List<Polygon3D> visual = this.Graphics3D.GetVisual();
    foreach (Polygon3D polygon3D in visual.Where<Polygon3D>((Func<Polygon3D, bool>) (item => item.Name != null && item.Name.Contains("LeftWallBrush"))).ToList<Polygon3D>())
      visual.Remove(polygon3D);
    this.UpdateLeftWall();
    this.Graphics3D.PrepareView();
    this.Graphics3D.View((Panel) this.RootPanel);
  }

  private void OnRightWallBrushChanged()
  {
    if (this.rightSideWallPlans == null || this.Graphics3D == null || this.Graphics3D.GetVisualCount() <= 0)
      return;
    List<Polygon3D> visual = this.Graphics3D.GetVisual();
    foreach (Polygon3D polygon3D in visual.Where<Polygon3D>((Func<Polygon3D, bool>) (item => item.Name != null && item.Name.Contains("RightWallBrush"))).ToList<Polygon3D>())
      visual.Remove(polygon3D);
    this.UpdateRightWall();
    this.Graphics3D.PrepareView();
    this.Graphics3D.View((Panel) this.RootPanel);
  }

  private void OnBottomWallBrushChanged()
  {
    if (this.bottomSideWallPlans == null || this.Graphics3D == null || this.Graphics3D.GetVisualCount() <= 0)
      return;
    List<Polygon3D> visual = this.Graphics3D.GetVisual();
    foreach (Polygon3D polygon3D in visual.Where<Polygon3D>((Func<Polygon3D, bool>) (item => item.Name != null && item.Name.Contains("BottomWallBrush"))).ToList<Polygon3D>())
      visual.Remove(polygon3D);
    this.UpdateBottomWall();
    this.Graphics3D.PrepareView();
    this.Graphics3D.View((Panel) this.RootPanel);
  }

  private void OnTopWallBrushChanged()
  {
    if (this.topSideWallPlans == null || this.Graphics3D == null || this.Graphics3D.GetVisualCount() <= 0)
      return;
    List<Polygon3D> visual = this.Graphics3D.GetVisual();
    foreach (Polygon3D polygon3D in visual.Where<Polygon3D>((Func<Polygon3D, bool>) (item => item.Name != null && item.Name.Contains("TopWallBrush"))).ToList<Polygon3D>())
      visual.Remove(polygon3D);
    this.UpdateTopWall();
    this.Graphics3D.PrepareView();
    this.Graphics3D.View((Panel) this.RootPanel);
  }

  private void UnRegisterSeries(ISupportAxes series)
  {
    if (this.InternalPrimaryAxis == null || this.InternalSecondaryAxis == null)
      return;
    if (this.InternalPrimaryAxis.RegisteredSeries.Contains(series))
      this.InternalPrimaryAxis.RegisteredSeries.Remove(series);
    if (this.InternalSecondaryAxis.RegisteredSeries.Contains(series))
      this.InternalSecondaryAxis.RegisteredSeries.Remove(series);
    if (this.InternalSecondaryAxis.RegisteredSeries.Count == 0 && this.InternalPrimaryAxis.RegisteredSeries.Count == 0)
    {
      this.InternalPrimaryAxis.Orientation = Orientation.Horizontal;
      this.InternalSecondaryAxis.Orientation = Orientation.Vertical;
    }
    if (this.InternalDepthAxis == null || !this.InternalDepthAxis.RegisteredSeries.Contains(series))
      return;
    this.InternalDepthAxis.RegisteredSeries.Remove(series);
    if (!this.InternalDepthAxis.IsDefault)
      return;
    this.InternalDepthAxis = (ChartAxis) null;
    this.DepthAxis = (ChartAxisBase3D) null;
  }

  private void UpdateVisibleSeries(IList seriesColl)
  {
    foreach (ChartSeries3D chartSeries3D in (IEnumerable) seriesColl)
    {
      chartSeries3D.UpdateLegendIconTemplate(false);
      chartSeries3D.Area = this;
      SfChart3D.CheckSeriesTransposition(chartSeries3D);
      if (chartSeries3D.ActualXAxis != null && !this.Axes.Contains(chartSeries3D.ActualXAxis))
      {
        chartSeries3D.ActualXAxis.Area = (ChartBase) this;
        this.Axes.Add(chartSeries3D.ActualXAxis);
      }
      if (chartSeries3D.ActualYAxis != null && !this.Axes.Contains(chartSeries3D.ActualYAxis))
      {
        chartSeries3D.ActualYAxis.Area = (ChartBase) this;
        this.Axes.Add(chartSeries3D.ActualYAxis);
      }
      if (chartSeries3D is XyzDataSeries3D xyzDataSeries3D && xyzDataSeries3D.ActualZAxis != null && !this.Axes.Contains(xyzDataSeries3D.ActualZAxis) && xyzDataSeries3D != null && !string.IsNullOrEmpty(xyzDataSeries3D.ZBindingPath))
      {
        xyzDataSeries3D.ActualZAxis.Area = (ChartBase) this;
        this.Axes.Add(xyzDataSeries3D.ActualZAxis);
      }
      if (this.controlsPresenter != null && !this.controlsPresenter.Children.Contains((UIElement) chartSeries3D))
        this.controlsPresenter.Children.Add((UIElement) chartSeries3D);
      if (chartSeries3D.IsSeriesVisible)
      {
        if (this.AreaType == ChartAreaType.None && chartSeries3D is CircularSeriesBase3D)
          this.VisibleSeries.Add((ChartSeriesBase) chartSeries3D);
        else if (this.AreaType == ChartAreaType.CartesianAxes && chartSeries3D is CartesianSeries3D)
          this.VisibleSeries.Add((ChartSeriesBase) chartSeries3D);
      }
      this.ActualSeries.Add((ChartSeriesBase) chartSeries3D);
    }
  }

  private void OnSeriesPropertyCollectionChanged(DependencyPropertyChangedEventArgs e)
  {
    if (e.OldValue != null)
    {
      ((Collection<ChartSeries3D>) e.OldValue).Clear();
      ((ObservableCollection<ChartSeries3D>) e.OldValue).CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnSeriesCollectionChanged);
      if (this.InternalPrimaryAxis != null && this.InternalPrimaryAxis.RegisteredSeries != null)
        this.InternalPrimaryAxis.RegisteredSeries.Clear();
      if (this.InternalSecondaryAxis != null && this.InternalSecondaryAxis.RegisteredSeries != null)
        this.InternalSecondaryAxis.RegisteredSeries.Clear();
      this.VisibleSeries.Clear();
      this.ActualSeries.Clear();
    }
    if (this.Series == null)
      return;
    this.Series.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnSeriesCollectionChanged);
    if (this.Series.Count <= 0)
      return;
    if (this.Series[0] is CircularSeriesBase3D)
      this.AreaType = ChartAreaType.None;
    else
      this.AreaType = ChartAreaType.CartesianAxes;
    this.UpdateVisibleSeries((IList) this.Series);
    this.UpdateLegend(this.Legend, false);
    this.ScheduleUpdate();
  }

  private void OnPerspectiveAngleChanged()
  {
    if (this.RootPanel == null || !this.RootPanelDesiredSize.HasValue)
      return;
    this.Graphics3D.View((Panel) this.RootPanel, this.ActualRotationAngle, this.ActualTiltAngle, this.RootPanelDesiredSize.Value, this.PerspectiveAngle, this.ActualDepth);
  }

  private void OnSeriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    switch (e.Action)
    {
      case NotifyCollectionChangedAction.Add:
        if (e.NewStartingIndex == 0)
        {
          if (this.Series[0] is CircularSeriesBase3D)
            this.AreaType = ChartAreaType.None;
          else
            this.AreaType = ChartAreaType.CartesianAxes;
        }
        this.UpdateVisibleSeries(e.NewItems);
        break;
      case NotifyCollectionChangedAction.Remove:
        ChartSeriesBase oldItem = e.OldItems[0] as ChartSeriesBase;
        if (oldItem is ISupportAxes series1)
          this.UnRegisterSeries(series1);
        if (oldItem is ChartSeries3D chartSeries3D && oldItem.GetAnimationIsActive())
        {
          chartSeries3D.AnimationStoryboard.Stop();
          chartSeries3D.AnimationStoryboard = (Storyboard) null;
        }
        if (this.VisibleSeries.Contains(oldItem))
          this.VisibleSeries.Remove(oldItem);
        if (this.ActualSeries.Contains(oldItem))
          this.ActualSeries.Remove(oldItem);
        this.controlsPresenter.Children.Remove((UIElement) oldItem);
        oldItem.RemoveTooltip();
        if (this.VisibleSeries.Count == 0 && this.Series.Count > 0)
        {
          if (this.Series[0] is CircularSeriesBase3D)
            this.AreaType = ChartAreaType.None;
          else
            this.AreaType = ChartAreaType.CartesianAxes;
          this.UpdateVisibleSeries((IList) this.Series);
        }
        if (this.Legend != null && this.LegendItems != null)
        {
          if (this.Legend is ChartLegendCollection)
          {
            using (IEnumerator<ObservableCollection<LegendItem>> enumerator = this.LegendItems.GetEnumerator())
            {
              while (enumerator.MoveNext())
                enumerator.Current.Clear();
              break;
            }
          }
          if (this.LegendItems.Count > 0)
          {
            this.LegendItems[0].Clear();
            break;
          }
          break;
        }
        break;
      case NotifyCollectionChangedAction.Reset:
        if (this.controlsPresenter != null)
        {
          for (int index = this.controlsPresenter.Children.Count - 1; index >= 0; --index)
          {
            if (this.controlsPresenter.Children[index] is ChartSeries3D)
            {
              ChartSeries3D child = this.controlsPresenter.Children[index] as ChartSeries3D;
              this.controlsPresenter.Children.RemoveAt(index);
              if (child.GetAnimationIsActive())
              {
                child.AnimationStoryboard.Stop();
                child.AnimationStoryboard = (Storyboard) null;
              }
              if (child is ISupportAxes series2)
                this.UnRegisterSeries(series2);
            }
          }
        }
        if (this.Legend != null && this.LegendItems != null)
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
        break;
    }
    Canvas adorningCanvas = this.GetAdorningCanvas();
    if (adorningCanvas != null)
    {
      foreach (ChartTooltip element in adorningCanvas.Children.OfType<ChartTooltip>().ToList<ChartTooltip>())
        adorningCanvas.Children.Remove((UIElement) element);
    }
    this.IsUpdateLegend = true;
    this.ScheduleUpdate();
    this.SBSInfoCalculated = false;
  }

  private void OnSizeChanged(object sender, SizeChangedEventArgs e)
  {
    if (!(e.NewSize != this.AvailableSize))
      return;
    this.InvalidateMeasure();
  }

  private void RotateChart(Point updatedPosition)
  {
    if (!this.rotationActivated)
      return;
    foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) this.VisibleSeries)
      chartSeriesBase.RemoveTooltip();
    double num1 = this.previousChartPosition.Y - updatedPosition.Y;
    double num2 = this.previousChartPosition.X - updatedPosition.X;
    this.Tilt -= num1;
    this.Rotation += num2;
    this.previousChartPosition = updatedPosition;
  }

  private void UpdateFrontWall()
  {
    Rect seriesClipRect = this.SeriesClipRect;
    Polygon3D.CreateBox(new Vector3D(seriesClipRect.Left, seriesClipRect.Top, this.ActualDepth == 0.0 ? -1.5 : -this.WallSize - 1.0), new Vector3D(seriesClipRect.Right, seriesClipRect.Bottom, this.ActualDepth == 0.0 ? -1.5 : -1.0), (DependencyObject) this, 0, this.Graphics3D, this.BackWallBrush, this.BackWallBrush, 0.0, false, "BackWallBrush");
  }

  private void Update3DWall()
  {
    if (this.AreaType != ChartAreaType.CartesianAxes)
      return;
    if (!(this.InternalSecondaryAxis.Orientation == Orientation.Vertical ? this.InternalSecondaryAxis : this.InternalPrimaryAxis).OpposedPosition && this.ActualRotationAngle >= 90.0 && this.ActualRotationAngle < 270.0)
      this.UpdateFrontWall();
    else
      this.UpdateBackWall();
    this.leftSideWallPlans = (Polygon3D[]) null;
    this.bottomSideWallPlans = (Polygon3D[]) null;
    this.topSideWallPlans = (Polygon3D[]) null;
    this.rightSideWallPlans = (Polygon3D[]) null;
    bool flag = false;
    if (this.Axes == null)
      return;
    foreach (ChartAxis ax in (Collection<ChartAxis>) this.Axes)
    {
      if (ax.Orientation == Orientation.Vertical)
      {
        if (ax.OpposedPosition || !ax.OpposedPosition && this.ActualRotationAngle >= 180.0 && this.ActualRotationAngle < 360.0 && this.rightSideWallPlans == null)
          this.UpdateRightWall();
        else if (this.leftSideWallPlans == null)
          this.UpdateLeftWall();
      }
      else
      {
        if (!flag)
        {
          if ((ax.OpposedPosition || ax.Area.Axes.Where<ChartAxis>((Func<ChartAxis, bool>) (x => x is ChartAxisBase3D && x.Orientation == Orientation.Horizontal && x.OpposedPosition)).Any<ChartAxis>()) && this.topSideWallPlans == null)
            this.UpdateTopWall();
          else if (this.bottomSideWallPlans == null)
            this.UpdateBottomWall();
        }
        flag = true;
      }
    }
  }

  private void Update3DView()
  {
    this.is3DUpdateScheduled = false;
    if (!this.RootPanelDesiredSize.HasValue)
      return;
    foreach (ChartSegment3D chartSegment3D in this.Series.SelectMany<ChartSeries3D, ChartSegment3D>((Func<ChartSeries3D, IEnumerable<ChartSegment3D>>) (series => series.Segments.OfType<ChartSegment3D>())))
      chartSegment3D.Polygons.Clear();
    if (this.RootPanel == null)
      return;
    if (this.IsAutoDepth && this.AutoDepthAdjust())
      this.Graphics3D.PrepareView(this.PerspectiveAngle, this.Depth, this.Rotation, this.Tilt, this.RootPanelDesiredSize.Value);
    this.Graphics3D.View((Panel) this.RootPanel, this.Rotation, this.Tilt, this.RootPanelDesiredSize.Value, this.PerspectiveAngle, this.ActualDepth);
  }

  private bool AutoDepthAdjust()
  {
    double vz = 0.0;
    if (this.IsChartRotated())
      vz = this.ActualDepth + 5.0;
    if (this.previousAutoDepth == vz)
      return false;
    foreach (Polygon3D polygon3D in this.Graphics3D.GetVisual())
    {
      if (polygon3D is UIElement3D || polygon3D is PolyLine3D)
      {
        int index = 0;
        Vector3D[] vector3DArray = new Vector3D[polygon3D.VectorPoints.Length];
        foreach (Vector3D vectorPoint in polygon3D.VectorPoints)
        {
          vector3DArray[index] = new Vector3D(vectorPoint.X, vectorPoint.Y, vz);
          ++index;
        }
        polygon3D.VectorPoints = vector3DArray;
      }
    }
    this.previousAutoDepth = vz;
    return true;
  }

  private void ChartMouseMove(object source, Point position)
  {
    ChartSegment tag = source is FrameworkElement frameworkElement ? frameworkElement.Tag as ChartSegment : (ChartSegment) null;
    if (this.rotationActivated)
      this.CaptureMouse();
    if (tag != null)
    {
      if ((this.EnableSegmentSelection && !tag.Series.IsLinear || this.EnableSeriesSelection) && Mouse.OverrideCursor == null)
        Mouse.OverrideCursor = this.SelectionCursor;
      if (this.mouseMoveSegment != null && this.mouseMoveSegment != tag && ((ChartSeries3D) this.mouseMoveSegment.Series).SelectionMode == SelectionMode.MouseMove)
        ((ChartSeries3D) this.mouseMoveSegment.Series).SelectedIndex = -1;
      this.currentSeries = (ChartSeries3D) tag.Series;
      ((ChartSeries3D) tag.Series).OnSeriesMouseMove(source, position);
      this.mouseMoveSegment = tag;
    }
    else if (this.mouseMoveSegment != null && ((ChartSeries3D) this.mouseMoveSegment.Series).SelectionMode == SelectionMode.MouseMove)
    {
      if (!(!this.EnableSeriesSelection ? this.mouseMoveSegment.Series.RaiseSelectionChanging(-1, ((ChartSeries3D) this.mouseMoveSegment.Series).SelectedIndex) : this.mouseMoveSegment.Series.RaiseSelectionChanging(-1, this.SeriesSelectedIndex)))
      {
        ((ChartSeries3D) this.mouseMoveSegment.Series).SelectedIndex = -1;
        this.SeriesSelectedIndex = -1;
        this.mouseMoveSegment = (ChartSegment) null;
      }
    }
    else if (tag == null && this.currentSeries != null && !(frameworkElement.DataContext is LegendItem))
    {
      if (Mouse.OverrideCursor != null)
        Mouse.OverrideCursor = (Cursor) null;
      this.currentSeries.OnSeriesMouseLeave(source, position);
    }
    if (frameworkElement.DataContext is ChartAdornmentContainer)
    {
      ChartAdornmentContainer dataContext = frameworkElement.DataContext as ChartAdornmentContainer;
      (dataContext.Adornment.Series as ChartSeries3D).OnSeriesMouseMove((object) frameworkElement, position);
      this.series = (object) dataContext.Adornment.Series;
    }
    else if (VisualTreeHelper.GetParent((DependencyObject) frameworkElement) is ContentPresenter)
    {
      ContentPresenter parent = VisualTreeHelper.GetParent((DependencyObject) frameworkElement) as ContentPresenter;
      if (!(parent.Content is ChartAdornment3D))
        return;
      ((parent.Content as ChartAdornment3D).Series as ChartSeries3D).OnSeriesMouseMove((object) frameworkElement, position);
      this.series = (object) (parent.Content as ChartAdornment3D).Series;
    }
    else
    {
      if (!(this.series is ChartSeries3D))
        return;
      (this.series as ChartSeries3D).OnSeriesMouseLeave((object) frameworkElement, position);
      this.series = (object) null;
    }
  }

  private void ChartMouseDown(object source, Point position, object pointer)
  {
    this.previousChartPosition = position;
    this.rotationActivated = this.EnableRotation;
    FrameworkElement frameworkElement = source as FrameworkElement;
    frameworkElement.CaptureMouse();
    ChartSegment tag = frameworkElement != null ? frameworkElement.Tag as ChartSegment : (ChartSegment) null;
    if (tag != null)
    {
      tag.Series.OnSeriesMouseDown(source, position);
    }
    else
    {
      if (this.Series == null)
        return;
      foreach (ChartSeries3D series in (Collection<ChartSeries3D>) this.Series)
      {
        if (SfChart3D.IsSeriesEventTrigger(source, (ChartSeriesBase) series))
        {
          series.OnSeriesMouseDown(source, position);
          break;
        }
      }
    }
  }

  private void ChartMouseUp(object source, Point position, object pointer)
  {
    this.rotationActivated = false;
    if (source is FrameworkElement element)
      element.ReleaseMouseCapture();
    this.ExplodeOnMouseClick(element, position);
  }

  private void ExplodeOnMouseClick(FrameworkElement element, Point position)
  {
    ChartSegment tag = element != null ? element.Tag as ChartSegment : (ChartSegment) null;
    if (tag != null)
    {
      tag.Series.OnSeriesMouseUp((object) element, position);
    }
    else
    {
      if (this.Series == null)
        return;
      foreach (ChartSeries3D series in (Collection<ChartSeries3D>) this.Series)
      {
        if (SfChart3D.IsSeriesEventTrigger((object) element, (ChartSeriesBase) series))
        {
          series.OnSeriesMouseUp((object) element, position);
          break;
        }
      }
    }
  }

  private void Schedule3DUpdate()
  {
    if (this.is3DUpdateScheduled)
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) new System.Action(this.Update3DView));
    this.is3DUpdateScheduled = true;
  }

  private void OnAxisChanged(DependencyPropertyChangedEventArgs e)
  {
    ChartAxis newValue = e.NewValue as ChartAxis;
    ChartAxis oldValue = e.OldValue as ChartAxis;
    if (this.Axes != null && oldValue != null && this.Axes.Contains(oldValue))
    {
      this.Axes.Remove(oldValue);
      oldValue.RegisteredSeries.Clear();
    }
    if (this.Axes != null && newValue != null && !this.Axes.Contains(newValue))
    {
      newValue.Area = (ChartBase) this;
      this.Axes.Insert(0, newValue);
    }
    if (this.Series != null && newValue != null)
    {
      foreach (ChartSeries3D series in (Collection<ChartSeries3D>) this.Series)
      {
        if (series is CartesianSeries3D)
        {
          SfChart3D.CheckSeriesTransposition(series);
          newValue.RegisteredSeries.Add((ISupportAxes) series);
        }
      }
    }
    this.ScheduleUpdate();
  }

  private void LayoutAxis(Size availableSize)
  {
    if (this.ChartAxisLayoutPanel != null)
    {
      this.ChartAxisLayoutPanel.UpdateElements();
      this.ChartAxisLayoutPanel.Measure(availableSize);
      this.ChartAxisLayoutPanel.Arrange(availableSize);
    }
    if (this.GridLinesLayout == null)
      return;
    this.GridLinesLayout.UpdateElements();
    this.GridLinesLayout.Measure(availableSize);
    ((ChartCartesianGridLinesPanel) this.GridLinesLayout).Arrange3D(availableSize);
  }

  private void ScheduleRenderSeries()
  {
    if (this.IsRenderSeriesDispatched)
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) new System.Action(this.RenderSeries));
    this.IsRenderSeriesDispatched = true;
  }
}
