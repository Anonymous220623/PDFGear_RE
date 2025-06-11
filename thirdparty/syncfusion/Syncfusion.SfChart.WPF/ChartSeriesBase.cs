// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartSeriesBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class ChartSeriesBase : Control, ICloneable
{
  private const int seriesTipHeight = 6;
  public static readonly DependencyProperty SpacingProperty = DependencyProperty.RegisterAttached("Spacing", typeof (double), typeof (ChartSeriesBase), new PropertyMetadata((object) 0.2, new PropertyChangedCallback(ChartSeriesBase.OnSegmentSpacingChanged)));
  public static readonly DependencyProperty TooltipTemplateProperty = DependencyProperty.Register(nameof (TooltipTemplate), typeof (DataTemplate), typeof (ChartSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartSeriesBase.OnTooltipTemplateChanged)));
  public static readonly DependencyProperty ShowTooltipProperty = DependencyProperty.Register(nameof (ShowTooltip), typeof (bool), typeof (ChartSeriesBase), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartSeriesBase.OnShowTooltipChanged)));
  public static readonly DependencyProperty ListenPropertyChangeProperty = DependencyProperty.Register(nameof (ListenPropertyChange), typeof (bool), typeof (ChartSeriesBase), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartSeriesBase.OnListenPropertyChangeChanged)));
  public static readonly DependencyProperty IsSeriesVisibleProperty = DependencyProperty.Register(nameof (IsSeriesVisible), typeof (bool), typeof (ChartSeriesBase), new PropertyMetadata((object) true, new PropertyChangedCallback(ChartSeriesBase.OnIsSeriesVisibleChanged)));
  public static readonly DependencyProperty XBindingPathProperty = DependencyProperty.Register(nameof (XBindingPath), typeof (string), typeof (ChartSeriesBase), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ChartSeriesBase.OnBindingPathXChanged)));
  public static readonly DependencyProperty SortByProperty = DependencyProperty.Register(nameof (SortBy), typeof (SortingAxis), typeof (ChartSeriesBase), new PropertyMetadata((object) SortingAxis.X, new PropertyChangedCallback(ChartSeriesBase.OnSortDataOrderChanged)));
  public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.Register(nameof (SortDirection), typeof (Direction), typeof (ChartSeriesBase), new PropertyMetadata((object) Direction.Ascending, new PropertyChangedCallback(ChartSeriesBase.OnSortDataOrderChanged)));
  public static readonly DependencyProperty IsSortDataProperty = DependencyProperty.Register(nameof (IsSortData), typeof (bool), typeof (ChartSeriesBase), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartSeriesBase.OnSortDataOrderChanged)));
  public static readonly DependencyProperty PaletteProperty = DependencyProperty.Register(nameof (Palette), typeof (ChartColorPalette), typeof (ChartSeriesBase), new PropertyMetadata((object) ChartColorPalette.None, new PropertyChangedCallback(ChartSeriesBase.OnPaletteChanged)));
  public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof (ItemsSource), typeof (object), typeof (ChartSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartSeriesBase.OnDataSourceChanged)));
  public static readonly DependencyProperty TrackBallLabelTemplateProperty = DependencyProperty.Register(nameof (TrackBallLabelTemplate), typeof (DataTemplate), typeof (ChartSeriesBase), (PropertyMetadata) null);
  public static readonly DependencyProperty InteriorProperty = DependencyProperty.Register(nameof (Interior), typeof (Brush), typeof (ChartSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartSeriesBase.OnAppearanceChanged)));
  public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof (Label), typeof (string), typeof (ChartSeriesBase), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ChartSeriesBase.OnLabelPropertyChanged)));
  public static readonly DependencyProperty LegendIconProperty = DependencyProperty.Register(nameof (LegendIcon), typeof (ChartLegendIcon), typeof (ChartSeriesBase), new PropertyMetadata((object) ChartLegendIcon.SeriesType, new PropertyChangedCallback(ChartSeriesBase.OnLegendIconChanged)));
  public static readonly DependencyProperty LegendIconTemplateProperty = DependencyProperty.Register(nameof (LegendIconTemplate), typeof (DataTemplate), typeof (ChartSeriesBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty VisibilityOnLegendProperty = DependencyProperty.Register(nameof (VisibilityOnLegend), typeof (Visibility), typeof (ChartSeriesBase), new PropertyMetadata((object) Visibility.Visible, new PropertyChangedCallback(ChartSeriesBase.OnVisibilityOnLegendChanged)));
  public static readonly DependencyProperty SeriesSelectionBrushProperty = DependencyProperty.Register(nameof (SeriesSelectionBrush), typeof (Brush), typeof (ChartSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartSeriesBase.OnSeriesSelectionBrushChanged)));
  public static readonly DependencyProperty ColorModelProperty = DependencyProperty.Register(nameof (ColorModel), typeof (ChartColorModel), typeof (ChartSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartSeriesBase.OnColorModelChanged)));
  public static readonly DependencyProperty SegmentColorPathProperty = DependencyProperty.Register(nameof (SegmentColorPath), typeof (string), typeof (ChartSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartSeriesBase.OnSegmentColorPathChanged)));
  public static readonly DependencyProperty EnableAnimationProperty = DependencyProperty.Register(nameof (EnableAnimation), typeof (bool), typeof (ChartSeriesBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.Register(nameof (AnimationDuration), typeof (TimeSpan), typeof (ChartSeriesBase), new PropertyMetadata((object) TimeSpan.FromSeconds(0.8)));
  public static readonly DependencyProperty EmptyPointValueProperty = DependencyProperty.Register(nameof (EmptyPointValue), typeof (EmptyPointValue), typeof (ChartSeriesBase), new PropertyMetadata((object) EmptyPointValue.Zero, new PropertyChangedCallback(ChartSeriesBase.OnEmptyPointValueChanged)));
  public static readonly DependencyProperty EmptyPointStyleProperty = DependencyProperty.Register(nameof (EmptyPointStyle), typeof (EmptyPointStyle), typeof (ChartSeriesBase), new PropertyMetadata((object) EmptyPointStyle.Interior, new PropertyChangedCallback(ChartSeriesBase.OnEmptyPointStyleChanged)));
  public static readonly DependencyProperty EmptyPointSymbolTemplateProperty = DependencyProperty.Register(nameof (EmptyPointSymbolTemplate), typeof (DataTemplate), typeof (ChartSeriesBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ShowEmptyPointsProperty = DependencyProperty.Register(nameof (ShowEmptyPoints), typeof (bool), typeof (ChartSeriesBase), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartSeriesBase.OnShowEmptyPointsChanged)));
  public static readonly DependencyProperty EmptyPointInteriorProperty = DependencyProperty.Register(nameof (EmptyPointInterior), typeof (Brush), typeof (ChartSeriesBase), new PropertyMetadata((object) new SolidColorBrush(Colors.Green), new PropertyChangedCallback(ChartSeriesBase.OnEmptyPointInteriorChanged)));
  internal static readonly DependencyProperty ActualTrackballLabelTemplateProperty = DependencyProperty.Register(nameof (ActualTrackballLabelTemplate), typeof (DataTemplate), typeof (ChartSeriesBase), (PropertyMetadata) null);
  internal readonly Stopwatch _stopwatch = new Stopwatch();
  internal List<int>[] EmptyPointIndexes;
  internal int SeriesYCount;
  internal bool totalCalculated;
  internal List<Rect> bitmapRects = new List<Rect>();
  internal bool IsStacked100;
  internal List<int> bitmapPixels = new List<int>();
  internal ChartDataPointInfo dataPoint;
  internal ChartSelectionChangingEventArgs selectionChangingEventArgs = new ChartSelectionChangingEventArgs();
  internal ChartSelectionChangedEventArgs selectionChangedEventArgs = new ChartSelectionChangedEventArgs();
  internal ChartAdornmentInfoBase adornmentInfo;
  internal List<double> GroupedXValuesIndexes = new List<double>();
  internal List<string> GroupedXValues = new List<string>();
  internal Dictionary<double, List<int>> DistinctValuesIndexes = new Dictionary<double, List<int>>();
  internal List<object> GroupedActualData = new List<object>();
  internal int PreviousSelectedIndex = -1;
  internal List<int> ToggledLegendIndex;
  internal string[] XComplexPaths;
  internal string[][] YComplexPaths;
  internal List<ChartSegment> selectedSegments = new List<ChartSegment>();
  internal ChartSeriesPanel SeriesPanel;
  internal bool IsPointGenerated;
  internal bool IsComplexYProperty;
  internal bool triggerSelectionChangedEventOnLoad;
  internal int UpdateStartedIndex = -1;
  internal bool isPointValidated;
  internal bool isLinearData = true;
  internal double XData;
  internal bool canAnimate;
  internal DoubleAnimation scaleXAnimation;
  internal DoubleAnimation scaleYAnimation;
  internal Storyboard storyBoard;
  internal Point mousePos;
  protected internal ObservableCollection<ChartSegment> Segments;
  protected internal DispatcherTimer Timer;
  protected internal DispatcherTimer InitialDelayTimer;
  protected IChartTransformer ChartTransformer;
  protected string[] YPaths;
  private double grandTotal;
  private ChartValueType xValueType;
  private ObservableCollection<int> _selectedSegmentsIndexes;
  private ObservableCollection<ChartAdornment> m_adornments = new ObservableCollection<ChartAdornment>();
  private ObservableCollection<ChartAdornment> m_visibleAdornments = new ObservableCollection<ChartAdornment>();
  private bool isActualTransposed;
  private DataTemplate toolTipTemplate;
  private int dataCount;
  private bool isNotificationSuspended;
  private bool isPropertyNotificationSuspended;
  private bool isUpdateStarted;
  private DataTemplate defaultTooltipTemplate;
  private double YData;
  private bool isRepeatPoint;
  private bool isSegmentColorChanged;
  internal bool HastoolTip;

  public ChartSeriesBase()
  {
    this.DefaultStyleKey = (object) typeof (ChartSeriesBase);
    this.Segments = new ObservableCollection<ChartSegment>();
    this.SelectedSegmentsIndexes = new ObservableCollection<int>();
    this.ToggledLegendIndex = new List<int>();
    if (this.ColorModel != null)
      this.ColorModel.Palette = this.Palette;
    this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
    this.ActualData = new List<object>();
    this.CanAnimate = true;
    this.Timer = new DispatcherTimer();
    this.Timer.Tick += new EventHandler(this.Timer_Tick);
    this.InitialDelayTimer = new DispatcherTimer();
    this.Pixels = new HashSet<int>();
    this.InitialDelayTimer.Tick += new EventHandler(this.InitialDelayTimer_Tick);
  }

  public event PropertyChangedEventHandler PropertyChanged;

  public EmptyPointValue EmptyPointValue
  {
    get => (EmptyPointValue) this.GetValue(ChartSeriesBase.EmptyPointValueProperty);
    set => this.SetValue(ChartSeriesBase.EmptyPointValueProperty, (object) value);
  }

  public EmptyPointStyle EmptyPointStyle
  {
    get => (EmptyPointStyle) this.GetValue(ChartSeriesBase.EmptyPointStyleProperty);
    set => this.SetValue(ChartSeriesBase.EmptyPointStyleProperty, (object) value);
  }

  public DataTemplate EmptyPointSymbolTemplate
  {
    get => (DataTemplate) this.GetValue(ChartSeriesBase.EmptyPointSymbolTemplateProperty);
    set => this.SetValue(ChartSeriesBase.EmptyPointSymbolTemplateProperty, (object) value);
  }

  public bool ShowEmptyPoints
  {
    get => (bool) this.GetValue(ChartSeriesBase.ShowEmptyPointsProperty);
    set => this.SetValue(ChartSeriesBase.ShowEmptyPointsProperty, (object) value);
  }

  public Brush EmptyPointInterior
  {
    get => (Brush) this.GetValue(ChartSeriesBase.EmptyPointInteriorProperty);
    set => this.SetValue(ChartSeriesBase.EmptyPointInteriorProperty, (object) value);
  }

  public int DataCount
  {
    get => this.dataCount;
    internal set => this.dataCount = value;
  }

  public bool IsSortData
  {
    get => (bool) this.GetValue(ChartSeriesBase.IsSortDataProperty);
    set => this.SetValue(ChartSeriesBase.IsSortDataProperty, (object) value);
  }

  public Direction SortDirection
  {
    get => (Direction) this.GetValue(ChartSeriesBase.SortDirectionProperty);
    set => this.SetValue(ChartSeriesBase.SortDirectionProperty, (object) value);
  }

  public SortingAxis SortBy
  {
    get => (SortingAxis) this.GetValue(ChartSeriesBase.SortByProperty);
    set => this.SetValue(ChartSeriesBase.SortByProperty, (object) value);
  }

  public DataTemplate TooltipTemplate
  {
    get => (DataTemplate) this.GetValue(ChartSeriesBase.TooltipTemplateProperty);
    set => this.SetValue(ChartSeriesBase.TooltipTemplateProperty, (object) value);
  }

  public bool ShowTooltip
  {
    get => (bool) this.GetValue(ChartSeriesBase.ShowTooltipProperty);
    set => this.SetValue(ChartSeriesBase.ShowTooltipProperty, (object) value);
  }

  public bool ListenPropertyChange
  {
    get => (bool) this.GetValue(ChartSeriesBase.ListenPropertyChangeProperty);
    set => this.SetValue(ChartSeriesBase.ListenPropertyChangeProperty, (object) value);
  }

  public ObservableCollection<ChartAdornment> Adornments => this.m_adornments;

  internal ObservableCollection<ChartAdornment> VisibleAdornments
  {
    get => this.m_visibleAdornments;
    set
    {
      if (this.m_visibleAdornments == value)
        return;
      this.m_visibleAdornments = value;
    }
  }

  public bool IsSeriesVisible
  {
    get => (bool) this.GetValue(ChartSeriesBase.IsSeriesVisibleProperty);
    set => this.SetValue(ChartSeriesBase.IsSeriesVisibleProperty, (object) value);
  }

  public ChartColorPalette Palette
  {
    get => (ChartColorPalette) this.GetValue(ChartSeriesBase.PaletteProperty);
    set => this.SetValue(ChartSeriesBase.PaletteProperty, (object) value);
  }

  public object ItemsSource
  {
    get => this.GetValue(ChartSeriesBase.ItemsSourceProperty);
    set => this.SetValue(ChartSeriesBase.ItemsSourceProperty, value);
  }

  public DataTemplate TrackBallLabelTemplate
  {
    get => (DataTemplate) this.GetValue(ChartSeriesBase.TrackBallLabelTemplateProperty);
    set => this.SetValue(ChartSeriesBase.TrackBallLabelTemplateProperty, (object) value);
  }

  public Brush Interior
  {
    get => (Brush) this.GetValue(ChartSeriesBase.InteriorProperty);
    set => this.SetValue(ChartSeriesBase.InteriorProperty, (object) value);
  }

  public string Label
  {
    get => (string) this.GetValue(ChartSeriesBase.LabelProperty);
    set => this.SetValue(ChartSeriesBase.LabelProperty, (object) value);
  }

  public ChartLegendIcon LegendIcon
  {
    get => (ChartLegendIcon) this.GetValue(ChartSeriesBase.LegendIconProperty);
    set => this.SetValue(ChartSeriesBase.LegendIconProperty, (object) value);
  }

  public DataTemplate LegendIconTemplate
  {
    get => (DataTemplate) this.GetValue(ChartSeriesBase.LegendIconTemplateProperty);
    set => this.SetValue(ChartSeriesBase.LegendIconTemplateProperty, (object) value);
  }

  public Visibility VisibilityOnLegend
  {
    get => (Visibility) this.GetValue(ChartSeriesBase.VisibilityOnLegendProperty);
    set => this.SetValue(ChartSeriesBase.VisibilityOnLegendProperty, (object) value);
  }

  public Brush SeriesSelectionBrush
  {
    get => (Brush) this.GetValue(ChartSeriesBase.SeriesSelectionBrushProperty);
    set => this.SetValue(ChartSeriesBase.SeriesSelectionBrushProperty, (object) value);
  }

  public ChartColorModel ColorModel
  {
    get => (ChartColorModel) this.GetValue(ChartSeriesBase.ColorModelProperty);
    set => this.SetValue(ChartSeriesBase.ColorModelProperty, (object) value);
  }

  public string XBindingPath
  {
    get => (string) this.GetValue(ChartSeriesBase.XBindingPathProperty);
    set => this.SetValue(ChartSeriesBase.XBindingPathProperty, (object) value);
  }

  public string SegmentColorPath
  {
    get => (string) this.GetValue(ChartSeriesBase.SegmentColorPathProperty);
    set => this.SetValue(ChartSeriesBase.SegmentColorPathProperty, (object) value);
  }

  public bool EnableAnimation
  {
    get => (bool) this.GetValue(ChartSeriesBase.EnableAnimationProperty);
    set => this.SetValue(ChartSeriesBase.EnableAnimationProperty, (object) value);
  }

  public TimeSpan AnimationDuration
  {
    get => (TimeSpan) this.GetValue(ChartSeriesBase.AnimationDurationProperty);
    set => this.SetValue(ChartSeriesBase.AnimationDurationProperty, (object) value);
  }

  internal ChartBase ActualArea { get; set; }

  internal bool IsSingleAccumulationSeries
  {
    get
    {
      if (this.ActualArea == null || this.ActualArea.ActualSeries.Count != 1)
        return false;
      return this.ActualArea.ActualSeries[0] is AccumulationSeriesBase || this.ActualArea.ActualSeries[0] is CircularSeriesBase3D;
    }
  }

  internal Panel SeriesRootPanel { get; set; }

  internal DoubleRange SideBySideInfoRangePad { get; set; }

  internal HashSet<int> Pixels { get; set; }

  internal IList<double>[] GroupedSeriesYValues { get; set; }

  internal bool IsActualTransposed
  {
    get => this.isActualTransposed;
    set
    {
      this.isActualTransposed = value;
      this.OnActualTransposeChanged();
    }
  }

  internal bool CanAnimate
  {
    get => this.canAnimate && this.EnableAnimation || this.GetAnimationIsActive();
    set => this.canAnimate = value;
  }

  internal ChartAdornmentPresenter AdornmentPresenter { get; set; }

  internal IEnumerable XValues { get; set; }

  internal IList<double>[] SeriesYValues { get; set; }

  internal IList<double>[] ActualSeriesYValues { get; set; }

  internal List<object> ActualData { get; set; }

  internal virtual bool IsMultipleYPathRequired => false;

  internal DataTemplate ActualTrackballLabelTemplate
  {
    get => (DataTemplate) this.GetValue(ChartSeriesBase.ActualTrackballLabelTemplateProperty);
    set => this.SetValue(ChartSeriesBase.ActualTrackballLabelTemplateProperty, (object) value);
  }

  internal ChartValueType XAxisValueType
  {
    get => this.xValueType;
    set => this.xValueType = value;
  }

  internal int NearestSegmentIndex { get; set; }

  internal object ToolTipTag { get; set; }

  internal DataTemplate OutlierTooltipTemplate { get; set; }

  internal bool IsItemsSourceChanged { get; set; }

  protected internal virtual bool IsSideBySide => false;

  protected internal virtual bool IsLinear => false;

  protected internal virtual bool IsAreaTypeSeries => false;

  protected internal virtual bool IsBitmapSeries => false;

  protected internal bool IsColorPathSeries
  {
    get
    {
      return !this.IsAreaTypeSeries && (!(this is PolarRadarSeriesBase) || (this as PolarRadarSeriesBase).DrawType != ChartSeriesDrawType.Area) && !(this is FinancialSeriesBase) && !(this is FastLineSeries);
    }
  }

  protected internal bool IsIndexed
  {
    get
    {
      return this.ActualXAxis is CategoryAxis || this.ActualXAxis is CategoryAxis3D || this.ActualXAxis is DateTimeCategoryAxis;
    }
  }

  protected internal IList<Brush> ColorValues { get; set; }

  protected internal IEnumerable ActualXValues { get; set; }

  protected internal ObservableCollection<int> SelectedSegmentsIndexes
  {
    get => this._selectedSegmentsIndexes;
    set
    {
      if (this.SelectedSegmentsIndexes != null)
        this.SelectedSegmentsIndexes.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.SelectedSegmentsIndexes_CollectionChanged);
      this._selectedSegmentsIndexes = value;
      if (this._selectedSegmentsIndexes == null)
        return;
      this._selectedSegmentsIndexes.CollectionChanged += new NotifyCollectionChangedEventHandler(this.SelectedSegmentsIndexes_CollectionChanged);
    }
  }

  protected internal virtual List<ChartSegment> SelectedSegments
  {
    get
    {
      return this.SelectedSegmentsIndexes.Count > 0 ? this.SelectedSegmentsIndexes.Where<int>((System.Func<int, bool>) (index => index < this.Segments.Count)).Select<int, ChartSegment>((System.Func<int, ChartSegment>) (index => this.Segments[index])).ToList<ChartSegment>() : (List<ChartSegment>) null;
    }
  }

  protected internal virtual ChartSegment SelectedSegment
  {
    get
    {
      if (!(this is ISegmentSelectable segmentSelectable))
        return (ChartSegment) null;
      int selectedIndex = segmentSelectable.SelectedIndex;
      return selectedIndex < this.Segments.Count && selectedIndex > 0 ? this.Segments[selectedIndex] : (ChartSegment) null;
    }
  }

  protected internal ChartAxis ActualXAxis
  {
    get
    {
      if (this.ActualArea == null || !(this is ISupportAxes))
        return (ChartAxis) null;
      return this.ActualArea is SfChart ? (ChartAxis) (this as ISupportAxes2D).XAxis ?? this.ActualArea.InternalPrimaryAxis : (ChartAxis) (this as CartesianSeries3D).XAxis ?? this.ActualArea.InternalPrimaryAxis;
    }
  }

  protected internal ChartAxis ActualYAxis
  {
    get
    {
      if (this.ActualArea == null || !(this is ISupportAxes))
        return (ChartAxis) null;
      return this.ActualArea is SfChart ? (ChartAxis) (this as ISupportAxes2D).YAxis ?? this.ActualArea.InternalSecondaryAxis : (ChartAxis) (this as CartesianSeries3D).YAxis ?? this.ActualArea.InternalSecondaryAxis;
    }
  }

  protected virtual bool IsStacked => false;

  public static double GetSpacing(DependencyObject obj)
  {
    return (double) obj.GetValue(ChartSeriesBase.SpacingProperty);
  }

  public static void SetSpacing(DependencyObject obj, double value)
  {
    obj.SetValue(ChartSeriesBase.SpacingProperty, (object) value);
  }

  public virtual void FindNearestChartPoint(
    Point point,
    out double x,
    out double y,
    out double stackedYValue)
  {
    x = double.NaN;
    y = double.NaN;
    stackedYValue = double.NaN;
    if (this.IsIndexed || !(this.ActualXValues is IList<double>))
    {
      if (this.ActualArea == null)
        return;
      double start = this.ActualXAxis.VisibleRange.Start;
      double end = this.ActualXAxis.VisibleRange.End;
      double num1 = Math.Round(new ChartPoint(this.ActualArea.PointToValue(this.ActualXAxis, point), this.ActualArea.PointToValue(this.ActualYAxis, point)).X);
      if (this.IsSideBySide && this.IsBitmapSeries)
      {
        DoubleRange sideBySideInfo = this.GetSideBySideInfo(this);
        start += sideBySideInfo.Start;
        end += sideBySideInfo.End;
      }
      if (((IEnumerable<IList<double>>) this.ActualSeriesYValues).Count<IList<double>>() <= 0)
        return;
      if (!(this is StackingColumn100Series) && this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed && !(this is WaterfallSeries) && !(this is ErrorBarSeries) || this.ActualXAxis is CategoryAxis3D && !(this.ActualXAxis as CategoryAxis3D).IsIndexed)
      {
        int count;
        switch (this)
        {
          case RangeSeriesBase _:
            count = (this as RangeSeriesBase).GroupedXValues.Count;
            break;
          case FinancialSeriesBase _:
            count = (this as FinancialSeriesBase).GroupedXValues.Count;
            break;
          default:
            count = this.GroupedXValues.Count;
            break;
        }
        int num2 = count;
        List<int> distinctValuesIndex = this.DistinctValuesIndexes[num1];
        int index = (int) num1;
        if (distinctValuesIndex.Count > 0)
        {
          index = distinctValuesIndex[0];
        }
        else
        {
          string groupedXvalue = this.GroupedXValues[(int) num1];
          foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) this.ActualArea.VisibleSeries)
          {
            index = (chartSeriesBase.ActualXValues as List<string>).IndexOf(groupedXvalue);
            if (index > -1)
              break;
          }
        }
        if (num1 > end || num1 < start || num1 >= (double) num2 || num1 < 0.0)
          return;
        ref double local = ref y;
        double num3;
        switch (this)
        {
          case RangeSeriesBase _:
            num3 = (this as RangeSeriesBase).GroupedSeriesYValues[0][index];
            break;
          case FinancialSeriesBase _:
            num3 = (this as FinancialSeriesBase).GroupedSeriesYValues[0][index];
            break;
          default:
            num3 = this.GroupedSeriesYValues[0][index];
            break;
        }
        local = num3;
        x = num1;
        this.NearestSegmentIndex = (int) x;
      }
      else
      {
        int count = this.ActualSeriesYValues[0].Count;
        if (num1 > end || num1 < start || num1 >= (double) count || num1 < 0.0)
          return;
        y = this.ActualSeriesYValues[0][(int) num1];
        x = num1;
        this.NearestSegmentIndex = (int) x;
      }
    }
    else
    {
      ChartPoint chartPoint1 = new ChartPoint();
      IList<double> actualXvalues = this.ActualXValues as IList<double>;
      IList<double> actualSeriesYvalue = this.ActualSeriesYValues[0];
      chartPoint1.X = this.ActualXAxis.VisibleRange.Start;
      if (this.IsSideBySide)
      {
        DoubleRange sideBySideInfo = this.GetSideBySideInfo(this);
        chartPoint1.X = this.ActualXAxis.VisibleRange.Start + sideBySideInfo.Start;
      }
      chartPoint1.Y = this.ActualYAxis.VisibleRange.Start;
      ChartPoint chartPoint2 = new ChartPoint(this.ActualArea.PointToValue(this.ActualXAxis, point), this.ActualArea.PointToValue(this.ActualYAxis, point));
      LogarithmicAxis actualXaxis = this.ActualXAxis as LogarithmicAxis;
      int index = 0;
      double num = actualXaxis != null ? Math.Log(chartPoint2.X, actualXaxis.LogarithmicBase) : chartPoint2.X;
      foreach (double x1 in (IEnumerable<double>) actualXvalues)
      {
        double y1 = actualSeriesYvalue[index];
        if (num > this.ActualXAxis.VisibleRange.Start && num < this.ActualXAxis.VisibleRange.End)
        {
          if (Math.Abs(chartPoint2.X - x1) < Math.Abs(chartPoint2.X - chartPoint1.X))
          {
            chartPoint1 = new ChartPoint(x1, y1);
            x = x1;
            y = y1;
            this.NearestSegmentIndex = index;
          }
          else if (Math.Abs(chartPoint2.X - x1) == Math.Abs(chartPoint2.X - chartPoint1.X) && Math.Abs(chartPoint2.Y - y1) < Math.Abs(chartPoint2.Y - chartPoint1.Y))
          {
            chartPoint1 = new ChartPoint(x1, y1);
            x = x1;
            y = y1;
            this.NearestSegmentIndex = index;
          }
        }
        ++index;
      }
    }
  }

  public virtual void CreateEmptyPointSegments(
    IList<double> YValues,
    out List<List<double>> yValList,
    out List<List<double>> xValList)
  {
    IList<double> doubleList1 = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? (!(this.ActualXValues is IList<double>) || this.IsIndexed ? (IList<double>) this.GetXValues() : this.ActualXValues as IList<double>) : (IList<double>) this.GroupedXValuesIndexes;
    List<double> doubleList2 = new List<double>();
    List<double> doubleList3 = new List<double>();
    yValList = new List<List<double>>();
    xValList = new List<List<double>>();
    for (int index = 0; index < YValues.Count; ++index)
    {
      if (double.IsNaN(YValues[index]))
      {
        if (doubleList3.Count > 0)
          yValList.Add(doubleList3);
        doubleList3 = new List<double>();
        if (doubleList2.Count > 0)
          xValList.Add(doubleList2);
        doubleList2 = new List<double>();
        xValList.Add(new List<double>()
        {
          doubleList1[index]
        });
        yValList.Add(new List<double>() { YValues[index] });
      }
      else
      {
        doubleList3.Add(YValues[index]);
        doubleList2.Add(((List<double>) doubleList1)[index]);
      }
    }
    if (doubleList3.Count > 0)
      yValList.Add(doubleList3);
    if (doubleList2.Count <= 0)
      return;
    xValList.Add(doubleList2);
  }

  public virtual void UpdateSegments(int index, NotifyCollectionChangedAction action)
  {
    this.UpdateArea();
  }

  public Size GetAvailableSize()
  {
    return this.ActualArea == null ? new Size() : this.ActualArea.AvailableSize;
  }

  public void SuspendNotification() => this.isNotificationSuspended = true;

  public void ResumeNotification()
  {
    if (this.isNotificationSuspended && this.isPropertyNotificationSuspended)
    {
      this.isPropertyNotificationSuspended = false;
      IEnumerable itemsSource = this.ItemsSource as IEnumerable;
      int index = -1;
      foreach (object obj in itemsSource)
      {
        ++index;
        this.SetIndividualPoint(index, obj, true);
      }
      if (this.IsSortData)
        this.SortActualPoints();
      this.UpdateArea();
    }
    else if (this.isNotificationSuspended)
    {
      if (!this.isUpdateStarted || this.UpdateStartedIndex < 0)
      {
        this.UpdateArea();
        this.isNotificationSuspended = false;
        return;
      }
      if (this.YPaths != null && this.ActualSeriesYValues != null && this.ItemsSource != null)
      {
        this.GeneratePoints(this.YPaths, this.ActualSeriesYValues);
        this.UpdateArea();
      }
      this.isUpdateStarted = false;
      this.UpdateStartedIndex = -1;
    }
    this.isNotificationSuspended = false;
  }

  public void Invalidate() => this.CalculateSegments();

  public DoubleRange GetSideBySideInfo(ChartSeriesBase currentseries)
  {
    if (this.ActualArea == null || this.ActualArea.InternalPrimaryAxis == null || this.ActualArea.InternalSecondaryAxis == null)
      return DoubleRange.Empty;
    if (!this.ActualArea.SBSInfoCalculated || !this.ActualArea.SeriesPosition.ContainsKey((object) currentseries))
      this.CalculateSideBySidePositions(true);
    double num1 = 1.0 - ChartSeriesBase.GetSpacing((DependencyObject) this);
    double minWidth = 0.0;
    double minPointsDelta = this.ActualArea.MinPointsDelta;
    if (!double.IsNaN(minPointsDelta))
      minWidth = minPointsDelta;
    if (!this.ActualArea.SideBySideSeriesPlacement)
    {
      this.CalculateSideBySideInfoPadding(minWidth, 1, 1, true);
      return new DoubleRange(-num1 * minWidth / 2.0, num1 * minWidth / 2.0);
    }
    int num2 = currentseries.IsActualTransposed ? this.ActualArea.GetActualRow((UIElement) currentseries.ActualXAxis) : this.ActualArea.GetActualRow((UIElement) currentseries.ActualYAxis);
    int num3 = currentseries.IsActualTransposed ? this.ActualArea.GetActualColumn((UIElement) currentseries.ActualYAxis) : this.ActualArea.GetActualColumn((UIElement) currentseries.ActualXAxis);
    int index1 = currentseries.ActualYAxis == null ? 0 : num2;
    int index2 = currentseries.ActualXAxis == null ? 0 : num3;
    if (index1 >= this.ActualArea.SbsSeriesCount.GetLength(0) || index2 >= this.ActualArea.SbsSeriesCount.GetLength(1))
      return DoubleRange.Empty;
    int all = this.ActualArea.SbsSeriesCount[index1, index2];
    if (!this.ActualArea.SeriesPosition.ContainsKey((object) currentseries))
      return DoubleRange.Empty;
    int pos = this.ActualArea.SeriesPosition[(object) currentseries];
    if (all == 0)
    {
      all = 1;
      pos = 1;
    }
    double num4 = minWidth * num1 / (double) all;
    double start = num4 * (double) (pos - 1) - minWidth * num1 / 2.0;
    double end = start + num4;
    this.CalculateSideBySideInfoPadding(minWidth, all, pos, true);
    return new DoubleRange(start, end);
  }

  public abstract void CreateSegments();

  public DependencyObject Clone() => this.CloneSeries((DependencyObject) null);

  internal static List<double> Clone(IList<double> Values)
  {
    List<double> doubleList = new List<double>();
    doubleList.AddRange((IEnumerable<double>) Values);
    return doubleList;
  }

  internal static ChartValueType GetDataType(
    IPropertyAccessor propertyAccessor,
    IEnumerable itemsSource)
  {
    if (itemsSource == null)
      return ChartValueType.Double;
    IEnumerator enumerator = itemsSource.GetEnumerator();
    object xval = (object) null;
    if (enumerator.MoveNext())
    {
      do
      {
        xval = propertyAccessor.GetValue(enumerator.Current);
      }
      while (enumerator.MoveNext() && xval == null);
    }
    return ChartSeriesBase.GetDataType(xval);
  }

  internal static void SetPropertyValue(object obj, string[] paths, object data)
  {
    object instance = obj;
    IPropertyAccessor propertyAccessor = (IPropertyAccessor) null;
    for (int index = 0; index < paths.Length; ++index)
    {
      PropertyInfo propertyInfo = ChartDataUtils.GetPropertyInfo(instance, paths[index]);
      if (propertyInfo != (PropertyInfo) null)
        propertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo);
      if (propertyAccessor == null)
        break;
      if (index == paths.Length - 1)
      {
        switch (propertyInfo.PropertyType.Name)
        {
          case "Int16":
            propertyAccessor.SetValue(instance, (object) Convert.ToInt16(data));
            break;
          case "Int32":
            propertyAccessor.SetValue(instance, (object) Convert.ToInt32(data));
            break;
          case "Single":
            propertyAccessor.SetValue(instance, (object) Convert.ToSingle(data));
            break;
          case "Decimal":
            propertyAccessor.SetValue(instance, (object) Convert.ToDecimal(data));
            break;
          case "String":
            propertyAccessor.SetValue(instance, (object) data.ToString());
            break;
          default:
            propertyAccessor.SetValue(instance, data);
            break;
        }
      }
      instance = propertyAccessor.GetValue(instance);
    }
  }

  internal static ChartValueType GetDataType(object xval)
  {
    switch (xval)
    {
      case string _:
      case string[] _:
        return ChartValueType.String;
      case DateTime _:
      case DateTime[] _:
        return ChartValueType.DateTime;
      case TimeSpan _:
      case TimeSpan[] _:
        return ChartValueType.TimeSpan;
      default:
        return ChartValueType.Double;
    }
  }

  internal virtual void GenerateDataTablePoints(string[] yPaths, IList<double>[] yLists)
  {
    IEnumerator enumerator = (this.ItemsSource as DataTable).Rows.GetEnumerator();
    if (enumerator.MoveNext())
    {
      for (int index = 0; index < this.UpdateStartedIndex; ++index)
        enumerator.MoveNext();
      this.XAxisValueType = ChartSeriesBase.GetDataType((enumerator.Current as DataRow).GetField(this.XBindingPath));
      if (this.XAxisValueType == ChartValueType.DateTime || this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic || this.XAxisValueType == ChartValueType.TimeSpan)
      {
        if (!(this.XValues is List<double>))
          this.ActualXValues = this.XValues = (IEnumerable) new List<double>();
      }
      else if (!(this.XValues is List<string>))
        this.ActualXValues = this.XValues = (IEnumerable) new List<string>();
      if (this.IsMultipleYPathRequired)
      {
        if (string.IsNullOrEmpty(yPaths[0]))
          return;
        if (this.XAxisValueType == ChartValueType.String)
        {
          IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
          do
          {
            object field1 = (enumerator.Current as DataRow).GetField(this.XBindingPath);
            xvalues.Add((string) field1);
            for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
            {
              object field2 = (enumerator.Current as DataRow).GetField(yPaths[index]);
              yLists[index].Add(Convert.ToDouble(field2 ?? (object) double.NaN));
            }
            this.ActualData.Add(enumerator.Current);
          }
          while (enumerator.MoveNext());
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          do
          {
            this.XData = Convert.ToDouble((enumerator.Current as DataRow).GetField(this.XBindingPath) ?? (object) double.NaN);
            if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
              this.isLinearData = false;
            xvalues.Add(this.XData);
            for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
            {
              object field = (enumerator.Current as DataRow).GetField(yPaths[index]);
              yLists[index].Add(Convert.ToDouble(field ?? (object) double.NaN));
            }
            this.ActualData.Add(enumerator.Current);
          }
          while (enumerator.MoveNext());
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.DateTime)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          do
          {
            this.XData = ((DateTime) (enumerator.Current as DataRow).GetField(this.XBindingPath)).ToOADate();
            if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
              this.isLinearData = false;
            xvalues.Add(this.XData);
            for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
            {
              object field = (enumerator.Current as DataRow).GetField(yPaths[index]);
              yLists[index].Add(Convert.ToDouble(field ?? (object) double.NaN));
            }
            this.ActualData.Add(enumerator.Current);
          }
          while (enumerator.MoveNext());
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.TimeSpan)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          do
          {
            this.XData = ((TimeSpan) (enumerator.Current as DataRow).GetField(this.XBindingPath)).TotalMilliseconds;
            if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
              this.isLinearData = false;
            xvalues.Add(this.XData);
            for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
            {
              object field = (enumerator.Current as DataRow).GetField(yPaths[index]);
              yLists[index].Add(Convert.ToDouble(field ?? (object) double.NaN));
            }
            this.ActualData.Add(enumerator.Current);
          }
          while (enumerator.MoveNext());
          this.dataCount = xvalues.Count;
        }
      }
      else
      {
        if (string.IsNullOrEmpty(yPaths[0]))
          return;
        IList<double> yList = yLists[0];
        if (this.XAxisValueType == ChartValueType.String)
        {
          IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
          do
          {
            object field3 = (enumerator.Current as DataRow).GetField(this.XBindingPath);
            object field4 = (enumerator.Current as DataRow).GetField(yPaths[0]);
            xvalues.Add((string) field3);
            yList.Add(Convert.ToDouble(field4 ?? (object) double.NaN));
            this.ActualData.Add(enumerator.Current);
          }
          while (enumerator.MoveNext());
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          do
          {
            object field5 = (enumerator.Current as DataRow).GetField(this.XBindingPath);
            object field6 = (enumerator.Current as DataRow).GetField(yPaths[0]);
            this.XData = Convert.ToDouble(field5 ?? (object) double.NaN);
            if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
              this.isLinearData = false;
            xvalues.Add(this.XData);
            yList.Add(Convert.ToDouble(field6 ?? (object) double.NaN));
            this.ActualData.Add(enumerator.Current);
          }
          while (enumerator.MoveNext());
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.DateTime)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          do
          {
            object field7 = (enumerator.Current as DataRow).GetField(this.XBindingPath);
            object field8 = (enumerator.Current as DataRow).GetField(yPaths[0]);
            this.XData = ((DateTime) field7).ToOADate();
            if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
              this.isLinearData = false;
            xvalues.Add(this.XData);
            yList.Add(Convert.ToDouble(field8 ?? (object) double.NaN));
            this.ActualData.Add(enumerator.Current);
          }
          while (enumerator.MoveNext());
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.TimeSpan)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          do
          {
            object field9 = (enumerator.Current as DataRow).GetField(this.XBindingPath);
            object field10 = (enumerator.Current as DataRow).GetField(yPaths[0]);
            this.XData = ((TimeSpan) field9).TotalMilliseconds;
            if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
              this.isLinearData = false;
            xvalues.Add(((TimeSpan) field9).TotalMilliseconds);
            yList.Add(Convert.ToDouble(field10 ?? (object) double.NaN));
            this.ActualData.Add(enumerator.Current);
          }
          while (enumerator.MoveNext());
          this.dataCount = xvalues.Count;
        }
      }
    }
    this.IsPointGenerated = true;
  }

  internal virtual void DataTableRowChanged(object sender, DataRowChangeEventArgs e)
  {
    int index1 = (this.ItemsSource as DataTable).Rows.IndexOf(e.Row);
    switch (e.Action)
    {
      case DataRowAction.Delete:
        if (this.ItemsSource != null)
        {
          if (this.XValues is IList<double>)
          {
            (this.XValues as IList<double>).RemoveAt(index1);
            --this.dataCount;
          }
          else if (this.XValues is IList<string>)
          {
            (this.XValues as IList<string>).RemoveAt(index1);
            --this.dataCount;
          }
          for (int index2 = 0; index2 < ((IEnumerable<IList<double>>) this.SeriesYValues).Count<IList<double>>(); ++index2)
            this.SeriesYValues[index2].RemoveAt(index1);
          if (this is XyzDataSeries3D xyzDataSeries3D)
          {
            if (xyzDataSeries3D.ZValues is IList<double>)
              (xyzDataSeries3D.ZValues as IList<double>).RemoveAt(index1);
            else if (xyzDataSeries3D.ZValues is IList<string>)
              (xyzDataSeries3D.ZValues as IList<string>).RemoveAt(index1);
          }
          if (this.IsSortData)
            this.SortActualPoints();
          this.ActualData.RemoveAt(index1);
          break;
        }
        break;
      case DataRowAction.Change:
        this.SetIndividualDataTablePoint(index1, (object) e.Row, true);
        break;
      case DataRowAction.Add:
        this.SetIndividualDataTablePoint(index1, (object) e.Row, false);
        break;
    }
    if (this is AccumulationSeriesBase || this.ActualArea.HasDataPointBasedLegend())
      this.ActualArea.IsUpdateLegend = true;
    this.totalCalculated = false;
    this.UpdateArea();
  }

  internal virtual void GenerateCustomTypeDescriptorPropertyPoints(
    string[] yPaths,
    IList<double>[] yLists,
    IEnumerator enumerator)
  {
    PropertyDescriptorCollection properties1 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
    PropertyDescriptor propertyDescriptor = properties1.Find(this.XBindingPath, false);
    if (propertyDescriptor == null)
      return;
    for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
    {
      if (properties1.Find(yPaths[index], false) == null)
        return;
    }
    object obj1 = propertyDescriptor.GetValue((object) this.XBindingPath);
    if (double.TryParse(obj1.ToString(), out double _))
    {
      this.XAxisValueType = ChartValueType.Double;
    }
    else
    {
      switch (obj1)
      {
        case DateTime _:
          this.XAxisValueType = ChartValueType.DateTime;
          break;
        case TimeSpan _:
          this.XAxisValueType = ChartValueType.TimeSpan;
          break;
        default:
          this.XAxisValueType = ChartValueType.String;
          break;
      }
    }
    if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
    {
      if (!(this.ActualXValues is List<double>))
        this.ActualXValues = this.XValues = (IEnumerable) new List<double>();
      IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
      do
      {
        PropertyDescriptorCollection properties2 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
        this.XData = Convert.ToDouble(properties2.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath) ?? (object) double.NaN);
        if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
          this.isLinearData = false;
        xvalues.Add(this.XData);
        for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
        {
          object obj2 = properties2.Find(yPaths[index], false).GetValue((object) yPaths[index]);
          yLists[index].Add(Convert.ToDouble(obj2 ?? (object) double.NaN));
        }
        this.ActualData.Add(enumerator.Current);
      }
      while (enumerator.MoveNext());
      this.dataCount = xvalues.Count;
    }
    else if (this.XAxisValueType == ChartValueType.DateTime)
    {
      if (!(this.ActualXValues is List<double>))
        this.ActualXValues = this.XValues = (IEnumerable) new List<double>();
      IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
      do
      {
        PropertyDescriptorCollection properties3 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
        this.XData = ((DateTime) properties3.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath)).ToOADate();
        if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
          this.isLinearData = false;
        xvalues.Add(this.XData);
        for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
        {
          object obj3 = properties3.Find(yPaths[index], false).GetValue((object) yPaths[index]);
          yLists[index].Add(Convert.ToDouble(obj3 ?? (object) double.NaN));
        }
        this.ActualData.Add(enumerator.Current);
      }
      while (enumerator.MoveNext());
      this.dataCount = xvalues.Count;
    }
    else if (this.XAxisValueType == ChartValueType.TimeSpan)
    {
      if (!(this.ActualXValues is List<double>))
        this.ActualXValues = this.XValues = (IEnumerable) new List<double>();
      IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
      do
      {
        PropertyDescriptorCollection properties4 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
        this.XData = ((TimeSpan) properties4.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath)).TotalMilliseconds;
        if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
          this.isLinearData = false;
        xvalues.Add(this.XData);
        for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
        {
          object obj4 = properties4.Find(yPaths[index], false).GetValue((object) yPaths[index]);
          yLists[index].Add(Convert.ToDouble(obj4 ?? (object) double.NaN));
        }
        this.ActualData.Add(enumerator.Current);
      }
      while (enumerator.MoveNext());
      this.dataCount = xvalues.Count;
    }
    else
    {
      if (this.XAxisValueType != ChartValueType.String)
        return;
      if (!(this.ActualXValues is List<string>))
        this.ActualXValues = this.XValues = (IEnumerable) new List<string>();
      IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
      do
      {
        PropertyDescriptorCollection properties5 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
        object obj5 = properties5.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath);
        xvalues.Add((string) obj5);
        for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
        {
          object obj6 = properties5.Find(yPaths[index], false).GetValue((object) yPaths[index]);
          yLists[index].Add(Convert.ToDouble(obj6 ?? (object) double.NaN));
        }
        this.ActualData.Add(enumerator.Current);
      }
      while (enumerator.MoveNext());
      this.dataCount = xvalues.Count;
    }
  }

  internal virtual void SelectedSegmentsIndexes_CollectionChanged(
    object sender,
    NotifyCollectionChangedEventArgs e)
  {
    switch (e.Action)
    {
      case NotifyCollectionChangedAction.Add:
        if (e.NewItems == null || this.ActualArea.SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
          break;
        int previousSelectedIndex = this.PreviousSelectedIndex;
        int newItem = (int) e.NewItems[0];
        if (newItem < 0 || !this.ActualArea.GetEnableSegmentSelection())
          break;
        if (this.adornmentInfo is ChartAdornmentInfo && this.adornmentInfo.HighlightOnSelection)
          this.UpdateAdornmentSelection(newItem);
        ISegmentSelectable segmentSelectable = this as ISegmentSelectable;
        if (newItem < this.Segments.Count && segmentSelectable != null && segmentSelectable.SegmentSelectionBrush != null)
        {
          this.Segments[newItem].BindProperties();
          this.Segments[newItem].IsSelectedSegment = true;
        }
        if (newItem < this.Segments.Count)
        {
          ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = this.Segments[newItem],
            SelectedSegments = this.ActualArea.SelectedSegments,
            SelectedSeries = this,
            SelectedIndex = newItem,
            PreviousSelectedIndex = previousSelectedIndex,
            PreviousSelectedSegment = (ChartSegment) null,
            NewPointInfo = this.Segments[newItem].Item,
            IsSelected = true
          };
          eventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
          if (previousSelectedIndex != -1 && previousSelectedIndex < this.Segments.Count)
          {
            eventArgs.PreviousSelectedSegment = this.Segments[previousSelectedIndex];
            eventArgs.OldPointInfo = this.Segments[previousSelectedIndex].Item;
          }
          (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
          this.PreviousSelectedIndex = newItem;
          break;
        }
        if (this.Segments.Count != 0)
          break;
        this.triggerSelectionChangedEventOnLoad = true;
        break;
      case NotifyCollectionChangedAction.Remove:
        if (e.OldItems == null || this.ActualArea.SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
          break;
        int oldItem = (int) e.OldItems[0];
        ChartSelectionChangedEventArgs eventArgs1 = new ChartSelectionChangedEventArgs()
        {
          SelectedSegment = (ChartSegment) null,
          SelectedSegments = this.ActualArea.SelectedSegments,
          SelectedSeries = (ChartSeriesBase) null,
          SelectedIndex = oldItem,
          PreviousSelectedIndex = this.PreviousSelectedIndex,
          PreviousSelectedSegment = (ChartSegment) null,
          PreviousSelectedSeries = this,
          IsSelected = false
        };
        if (this.PreviousSelectedIndex != -1 && this.PreviousSelectedIndex < this.Segments.Count)
        {
          eventArgs1.PreviousSelectedSegment = this.Segments[this.PreviousSelectedIndex];
          eventArgs1.OldPointInfo = this.Segments[this.PreviousSelectedIndex].Item;
        }
        (this.ActualArea as SfChart).OnSelectionChanged(eventArgs1);
        this.OnResetSegment(oldItem);
        this.PreviousSelectedIndex = oldItem;
        break;
    }
  }

  internal virtual void InitialDelayTimer_Tick(object sender, object e)
  {
    Canvas adorningCanvas = this.ActualArea.GetAdorningCanvas();
    ChartTooltip tooltip = this.ActualArea.Tooltip;
    tooltip.PolygonPath = " ";
    int actualShowDuration = ChartTooltip.GetActualShowDuration(this.ActualArea.TooltipBehavior, ChartTooltip.GetShowDuration((DependencyObject) this));
    if (!adorningCanvas.Children.Contains((UIElement) tooltip))
    {
      this.HastoolTip = true;
      adorningCanvas.Children.Add((UIElement) tooltip);
    }
    this.AddTooltip();
    if (ChartTooltip.GetActualEnableAnimation(this.ActualArea.TooltipBehavior, ChartTooltip.GetEnableAnimation((UIElement) this)))
    {
      if (this.ActualArea is SfChart)
        this.SetDoubleAnimation(tooltip);
      else
        ChartSeriesBase.FadeInAnimation(ref tooltip);
    }
    Canvas.SetLeft((UIElement) tooltip, tooltip.LeftOffset);
    Canvas.SetTop((UIElement) tooltip, tooltip.TopOffset);
    this.InitialDelayTimer.Stop();
    this.Timer.Interval = new TimeSpan(0, 0, 0, 0, actualShowDuration);
    this.Timer.Start();
  }

  internal virtual DataTemplate GetDefaultTooltipTemplate()
  {
    if (this.defaultTooltipTemplate == null)
      this.defaultTooltipTemplate = this.ActualArea is SfChart ? ChartDictionaries.GenericCommonDictionary[(object) "DefaultTooltipTemplate"] as DataTemplate : ChartDictionaries.GenericCommonDictionary[(object) "Default3DTooltipTemplate"] as DataTemplate;
    return this.defaultTooltipTemplate;
  }

  internal virtual void AddTooltip()
  {
    ChartTooltip tooltip = this.ActualArea.Tooltip;
    tooltip.PreviousSeries = this;
    if (this.ToolTipTag == null)
      return;
    object toolTipTag = this.ToolTipTag;
    tooltip.UpdateLayout();
    Size size = new Size(tooltip.ActualWidth, tooltip.ActualHeight);
    Point point1 = new Point();
    ChartTooltipBehavior tooltipBehavior = this.ActualArea.TooltipBehavior;
    this.ActualTooltipPosition = tooltipBehavior == null ? TooltipPosition.Auto : tooltipBehavior.Position;
    Point position;
    if (this.ActualTooltipPosition == TooltipPosition.Auto && this.ActualArea is SfChart)
    {
      Point point2 = this.GetDataPointPosition(tooltip);
      switch (this)
      {
        case CircularSeriesBase _:
        case TriangularSeriesBase _:
        case PolarRadarSeriesBase _:
label_9:
          position = this.Position(point2, ref tooltip);
          break;
        default:
          Rect seriesClipRect = this.ActualArea.SeriesClipRect;
          if (!(this is FinancialSeriesBase))
          {
            if (this.IsActualTransposed)
            {
              switch (this)
              {
                case BarSeries _:
                case StackingBarSeries _:
                case ColumnSeries _:
                case StackingColumnSeries _:
                case RangeSeriesBase _:
                  goto label_7;
              }
            }
            point2 = this.SetTooltipMarkerPosition(point2, tooltip);
          }
label_7:
          if (!seriesClipRect.Contains(point2))
          {
            point2 = this.mousePos;
            this.ActualTooltipPosition = TooltipPosition.Pointer;
            goto label_9;
          }
          goto label_9;
      }
    }
    else
      position = this.Position(this.mousePos, ref tooltip);
    HorizontalPosition horizontal = HorizontalPosition.Center;
    VerticalPosition vertical = VerticalPosition.Bottom;
    this.AlignTooltip(size, position, out horizontal, out vertical);
    tooltip.Margin = ChartTooltip.GetActualTooltipMargin(tooltipBehavior, ChartTooltip.GetTooltipMargin((UIElement) this));
    if (tooltipBehavior != null)
    {
      tooltip.BackgroundStyle = tooltipBehavior.Style != null ? tooltipBehavior.Style : this.ActualArea.ChartResourceDictionary[(object) "tooltipPathStyle"] as Style;
      tooltip.LabelStyle = tooltipBehavior.LabelStyle != null ? tooltipBehavior.LabelStyle : this.ActualArea.ChartResourceDictionary[(object) "tooltipLabelStyle"] as Style;
    }
    if (!(this.ActualArea is SfChart))
      return;
    tooltip.PolygonPath = ChartDataUtils.GenerateTooltipPolygon(size, horizontal, vertical);
  }

  private Point SetTooltipMarkerPosition(Point tooltipPosition, ChartTooltip chartTooltip)
  {
    if (this.adornmentInfo != null && this.adornmentInfo.ShowMarker && (this.adornmentInfo.AdornmentsPosition == AdornmentsPosition.Top || this is RangeSeriesBase && this.adornmentInfo.AdornmentsPosition == AdornmentsPosition.TopAndBottom))
    {
      if (this.adornmentInfo.Symbol == ChartSymbol.Custom && this.adornmentInfo.SymbolTemplate != null)
      {
        FrameworkElement frameworkElement = (FrameworkElement) (this.adornmentInfo.SymbolTemplate.LoadContent() as Shape);
        if (frameworkElement != null)
        {
          frameworkElement.UpdateLayout();
          double actualHeight = frameworkElement.ActualHeight;
          double actualWidth = frameworkElement.ActualWidth;
          tooltipPosition.Y -= frameworkElement.ActualHeight / 2.0;
          if (tooltipPosition.Y - chartTooltip.ActualHeight < this.ActualArea.SeriesClipRect.Top)
            tooltipPosition.Y += frameworkElement.ActualHeight;
        }
      }
      else if (this.adornmentInfo.Symbol != ChartSymbol.Custom)
      {
        tooltipPosition.Y -= this.adornmentInfo.SymbolHeight / 2.0;
        if (tooltipPosition.Y - chartTooltip.ActualHeight < this.ActualArea.SeriesClipRect.Top)
          tooltipPosition.Y += this.adornmentInfo.SymbolHeight;
      }
    }
    return tooltipPosition;
  }

  internal virtual Point GetDataPointPosition(ChartTooltip tooltip)
  {
    ChartSegment dataContext = tooltip.DataContext as ChartSegment;
    Point dataPointPosition = new Point();
    if (dataContext != null)
    {
      Point visible = this.ChartTransformer.TransformToVisible(dataContext.XRange.End, dataContext.YRange.End);
      dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left - tooltip.DesiredSize.Width / 2.0;
      dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top - tooltip.DesiredSize.Height;
    }
    return dataPointPosition;
  }

  internal virtual VerticalPosition GetVerticalPosition(VerticalPosition verticalPosition)
  {
    return verticalPosition;
  }

  internal Point AlignTooltip(
    Size size,
    Point position,
    out HorizontalPosition horizontal,
    out VerticalPosition vertical)
  {
    ChartTooltip tooltip = this.ActualArea.Tooltip;
    double num1 = size.Width + position.X;
    double num2 = size.Height + position.Y;
    Point point = new Point();
    Rect seriesClipRect = this.ActualArea.SeriesClipRect;
    horizontal = HorizontalPosition.Center;
    vertical = VerticalPosition.Bottom;
    if (this.ActualArea is SfChart3D)
    {
      tooltip.LeftOffset = position.X;
      tooltip.TopOffset = position.Y;
      switch (ChartTooltip.GetHorizontalAlignment((UIElement) this))
      {
        case HorizontalAlignment.Left:
          tooltip.LeftOffset += 7.5;
          break;
        case HorizontalAlignment.Right:
          tooltip.LeftOffset -= 7.5;
          break;
      }
      switch (ChartTooltip.GetVerticalAlignment((UIElement) this))
      {
        case VerticalAlignment.Top:
          tooltip.TopOffset += 5.0;
          break;
        case VerticalAlignment.Bottom:
          tooltip.TopOffset -= 5.0;
          break;
      }
    }
    if (this.ActualArea.AreaType != ChartAreaType.None)
    {
      if (this.ActualArea is SfChart)
      {
        if (position.Y < seriesClipRect.Top)
        {
          point.Y = position.Y + size.Height;
          point.Y += 4.0;
          vertical = VerticalPosition.Top;
        }
        else if (num2 > seriesClipRect.Bottom)
        {
          point.Y = position.Y + (seriesClipRect.Bottom - num2);
          point.Y -= 4.0;
          vertical = VerticalPosition.Bottom;
        }
        else
        {
          point.Y = position.Y;
          point.Y -= 4.0;
          vertical = VerticalPosition.Bottom;
        }
        if (position.X < seriesClipRect.Left)
        {
          point.X = position.X + size.Width / 2.0;
          point.X += 4.0;
          horizontal = HorizontalPosition.Left;
        }
        else if (num1 > seriesClipRect.Right)
        {
          point.X = position.X - size.Width / 2.0;
          point.X -= 4.0;
          horizontal = HorizontalPosition.Right;
        }
        else
        {
          point.X = position.X;
          horizontal = HorizontalPosition.Center;
        }
        vertical = this.GetVerticalPosition(vertical);
        tooltip.LeftOffset = horizontal == HorizontalPosition.Left ? point.X - 2.0 : (horizontal == HorizontalPosition.Right ? point.X + 2.0 : point.X);
        tooltip.TopOffset = vertical == VerticalPosition.Bottom ? point.Y - 2.0 : (vertical == VerticalPosition.Top ? point.Y + 2.0 : point.Y);
      }
      this.AdjustTooltipAtEdge(tooltip);
    }
    else
    {
      point = position;
      if (this.ActualArea is SfChart)
      {
        tooltip.LeftOffset = point.X;
        tooltip.TopOffset = point.Y;
      }
      tooltip.LeftOffset = tooltip.LeftOffset >= 0.0 ? (tooltip.LeftOffset + tooltip.DesiredSize.Width <= this.ActualArea.RootPanelDesiredSize.Value.Width ? tooltip.LeftOffset : this.ActualArea.RootPanelDesiredSize.Value.Width - tooltip.DesiredSize.Width) : 0.0;
      tooltip.TopOffset = tooltip.TopOffset >= 0.0 ? (tooltip.TopOffset + tooltip.DesiredSize.Height <= this.ActualArea.RootPanelDesiredSize.Value.Height ? tooltip.TopOffset : this.ActualArea.RootPanelDesiredSize.Value.Height - tooltip.DesiredSize.Height) : 0.0;
    }
    return new Point(point.X, point.Y);
  }

  private void AdjustTooltipAtEdge(ChartTooltip chartTooltip)
  {
    chartTooltip.LeftOffset = chartTooltip.LeftOffset >= this.ActualArea.SeriesClipRect.Left ? (chartTooltip.LeftOffset + chartTooltip.DesiredSize.Width <= this.ActualArea.SeriesClipRect.Right ? chartTooltip.LeftOffset : this.ActualArea.SeriesClipRect.Right - chartTooltip.DesiredSize.Width) : this.ActualArea.SeriesClipRect.Left;
    if (chartTooltip.TopOffset < this.ActualArea.SeriesClipRect.Top)
      chartTooltip.TopOffset = this.ActualArea.SeriesClipRect.Top;
    else if (chartTooltip.TopOffset + chartTooltip.DesiredSize.Height > this.ActualArea.SeriesClipRect.Bottom)
      chartTooltip.TopOffset = this.ActualArea.SeriesClipRect.Bottom - chartTooltip.DesiredSize.Height;
    else
      chartTooltip.TopOffset = chartTooltip.TopOffset;
  }

  internal void GenerateTooltipPolygon(ContentControl control)
  {
    PointCollection pointCollection = new PointCollection();
    ChartSegment content = control.Content as ChartSegment;
    if (content.PolygonPoints != null && content.PolygonPoints.Count > 1)
    {
      content.PolygonPoints.Clear();
      content.PolygonPoints = (PointCollection) null;
    }
    control.UpdateLayout();
    double width = control.DesiredSize.Width;
    double height = control.DesiredSize.Height;
    ChartAlignment horizontalAlignment = ChartAlignment.Far;
    ChartAlignment verticalAlignment = ChartAlignment.Center;
    double x = width / 2.0;
    double y = height;
    switch (ChartTooltip.GetHorizontalAlignment((UIElement) this))
    {
      case HorizontalAlignment.Left:
        x = width;
        horizontalAlignment = ChartAlignment.Near;
        break;
      case HorizontalAlignment.Center:
        x = width / 2.0;
        horizontalAlignment = ChartAlignment.Center;
        break;
      case HorizontalAlignment.Right:
        x = 0.0;
        horizontalAlignment = ChartAlignment.Far;
        break;
    }
    switch (ChartTooltip.GetVerticalAlignment((UIElement) this))
    {
      case VerticalAlignment.Top:
        verticalAlignment = ChartAlignment.Near;
        y = height + 6.0;
        break;
      case VerticalAlignment.Center:
        verticalAlignment = ChartAlignment.Center;
        y = height / 2.0;
        break;
      case VerticalAlignment.Bottom:
        verticalAlignment = ChartTooltip.GetHorizontalAlignment((UIElement) this) == HorizontalAlignment.Center ? ChartAlignment.Far : ChartAlignment.Near;
        y = 0.0;
        break;
    }
    content.PolygonPoints = ChartDataUtils.GetTooltipPolygonPoints(new Rect(x, y, width, height), 6.0, false, horizontalAlignment, verticalAlignment);
  }

  private HorizontalPosition GetHorizontalPosition(HorizontalAlignment horizontalAlignment)
  {
    switch (horizontalAlignment)
    {
      case HorizontalAlignment.Left:
        return HorizontalPosition.Left;
      case HorizontalAlignment.Center:
        return HorizontalPosition.Center;
      case HorizontalAlignment.Right:
        return HorizontalPosition.Right;
      default:
        return HorizontalPosition.Center;
    }
  }

  private VerticalPosition GetVerticalPosition(VerticalAlignment verticalAlignment)
  {
    switch (verticalAlignment)
    {
      case VerticalAlignment.Top:
        return VerticalPosition.Top;
      case VerticalAlignment.Center:
        return VerticalPosition.Center;
      case VerticalAlignment.Bottom:
        return VerticalPosition.Bottom;
      default:
        return VerticalPosition.Top;
    }
  }

  internal virtual void RemoveTooltip()
  {
    if (this.ActualArea == null)
      return;
    Canvas adorningCanvas = this.ActualArea.GetAdorningCanvas();
    if (adorningCanvas == null)
      return;
    int index = 0;
    while (index < adorningCanvas.Children.Count)
    {
      if (adorningCanvas.Children[index] is ChartTooltip)
      {
        if (this.storyBoard != null)
        {
          ChartTooltip child = adorningCanvas.Children[index] as ChartTooltip;
          Canvas.SetLeft((UIElement) child, child.LeftOffset);
          Canvas.SetTop((UIElement) child, child.TopOffset);
          this.storyBoard.Stop();
          this.storyBoard = (Storyboard) null;
        }
        this.HastoolTip = false;
        adorningCanvas.Children.Remove(adorningCanvas.Children[index]);
      }
      else
        ++index;
    }
  }

  internal virtual void UpdateTooltip(object source)
  {
    if (!this.ShowTooltip || !(source is FrameworkElement element))
      return;
    object tooltipTag = this.GetTooltipTag(element);
    if (this.ToolTipTag != null && !this.ToolTipTag.Equals(tooltipTag))
    {
      this.RemoveTooltip();
      this.Timer.Stop();
      this.ActualArea.Tooltip = new ChartTooltip();
    }
    this.UpdateSeriesTooltip(tooltipTag);
  }

  internal virtual object GetTooltipTag(FrameworkElement element)
  {
    object tooltipTag = (object) null;
    if (element.Tag is ChartSegment)
      tooltipTag = element.Tag;
    else if (element.DataContext is ChartSegment && !(element.DataContext is ChartAdornment))
      tooltipTag = element.DataContext;
    else if (element.DataContext is ChartAdornmentContainer)
      tooltipTag = this.GetSegment((element.DataContext as ChartAdornmentContainer).Adornment.Item);
    else if (VisualTreeHelper.GetParent((DependencyObject) element) is ContentPresenter parent && parent.Content is ChartAdornment)
    {
      tooltipTag = this.GetSegment((parent.Content as ChartAdornment).Item);
    }
    else
    {
      int adornmentIndex = ChartExtensionUtils.GetAdornmentIndex((object) element);
      if (adornmentIndex != -1 && adornmentIndex < this.Adornments.Count && adornmentIndex < this.Segments.Count)
        tooltipTag = this.GetSegment(this.Adornments[adornmentIndex].Item);
    }
    return tooltipTag;
  }

  internal virtual void OnTransposeChanged(bool val) => this.IsActualTransposed = val;

  internal virtual ChartDataPointInfo GetDataPoint(int index) => this.dataPoint;

  internal virtual void OnResetSegment(int index)
  {
    if (index >= this.Segments.Count || index < 0)
      return;
    this.Segments[index].BindProperties();
    this.Segments[index].IsSelectedSegment = false;
    if (!(this.adornmentInfo is ChartAdornmentInfo))
      return;
    this.AdornmentPresenter.ResetAdornmentSelection(new int?(index), false);
  }

  internal virtual void RaiseSelectionChangedEvent()
  {
    this.selectionChangedEventArgs.SelectedSegment = this.SelectedSegment;
    this.selectionChangedEventArgs.SelectedSegments = this.ActualArea.SelectedSegments;
    this.SetSelectionChangedEventArgs();
    this.ActualArea.OnSelectionChanged(this.selectionChangedEventArgs);
    this.PreviousSelectedIndex = this.selectionChangedEventArgs.SelectedIndex;
    this.triggerSelectionChangedEventOnLoad = false;
  }

  internal virtual void SetSelectionChangedEventArgs()
  {
    switch (this)
    {
      case ISegmentSelectable segmentSelectable when this.Segments.Count != 0:
        if (this.adornmentInfo is ChartAdornmentInfo && this.adornmentInfo.HighlightOnSelection)
          this.UpdateAdornmentSelection(segmentSelectable.SelectedIndex);
        this.selectionChangedEventArgs.SelectedIndex = segmentSelectable.SelectedIndex;
        if (this.IsAreaTypeSeries || this.IsBitmapSeries || this is FastLineSeries)
        {
          this.selectionChangedEventArgs.SelectedSegment = this.Segments[0];
          this.selectionChangedEventArgs.NewPointInfo = (object) this.GetDataPoint(this.selectionChangedEventArgs.SelectedIndex);
          break;
        }
        this.selectionChangedEventArgs.NewPointInfo = this.ActualData[this.selectionChangedEventArgs.SelectedIndex];
        break;
      case ChartSeries3D _:
        this.selectionChangedEventArgs.SelectedIndex = (this as ChartSeries3D).SelectedIndex;
        this.selectionChangedEventArgs.NewPointInfo = this.Segments[this.selectionChangedEventArgs.SelectedIndex].Item;
        break;
    }
    this.selectionChangedEventArgs.SelectedSeries = this;
    this.selectionChangedEventArgs.PreviousSelectedIndex = -1;
    this.selectionChangedEventArgs.IsSelected = true;
    this.selectionChangedEventArgs.PreviousSelectedSegment = (ChartSegment) null;
    this.selectionChangedEventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
  }

  internal virtual bool GetAnimationIsActive() => false;

  internal virtual void FindNearestFinancialChartPoint(
    FinancialTechnicalIndicator technicalIndicator,
    Point point,
    out double x,
    out List<double> y1,
    out List<double> y2)
  {
    x = double.NaN;
    y1 = new List<double>();
    y2 = new List<double>();
    if (this.IsIndexed || !(this.ActualXValues is IList<double>))
    {
      if (this.ActualArea == null)
        return;
      double start = this.ActualXAxis.VisibleRange.Start;
      double end = this.ActualXAxis.VisibleRange.End;
      int index1 = (int) Math.Round(new ChartPoint(this.ActualArea.PointToValue(this.ActualXAxis, point), this.ActualArea.PointToValue(this.ActualYAxis, point)).X);
      if (((IEnumerable<IList<double>>) this.ActualSeriesYValues).Count<IList<double>>() <= 0)
        return;
      int count = this.ActualSeriesYValues[0].Count;
      if ((double) index1 > end || (double) index1 < start || index1 >= count || index1 < 0)
        return;
      if (technicalIndicator != null)
      {
        foreach (ChartSegment segment in (Collection<ChartSegment>) technicalIndicator.Segments)
        {
          if (segment is FastColumnBitmapSegment columnBitmapSegment)
          {
            if (index1 < columnBitmapSegment.y1ChartVals.Count)
              y2.Add(columnBitmapSegment.y1ChartVals[index1]);
          }
          else
          {
            TechnicalIndicatorSegment indicatorSegment = segment as TechnicalIndicatorSegment;
            if (index1 < indicatorSegment.yChartVals.Count)
            {
              if ((indicatorSegment.Length == 0 ? ((double) index1 < indicatorSegment.xChartVals[index1] ? 1 : 0) : (index1 < indicatorSegment.Length - 1 ? 1 : 0)) != 0)
              {
                y2.Add(double.NaN);
              }
              else
              {
                double yChartVal = indicatorSegment.yChartVals[index1];
                y2.Add(yChartVal);
              }
            }
          }
        }
      }
      for (int index2 = 0; index2 < ((IEnumerable<IList<double>>) this.ActualSeriesYValues).Count<IList<double>>(); ++index2)
      {
        double num = this.ActualSeriesYValues[index2][index1];
        y1.Add(num);
        if (this is RangeColumnSeries && !this.IsMultipleYPathRequired)
          break;
      }
      x = (double) index1;
    }
    else
    {
      IList<double> actualXvalues = this.ActualXValues as IList<double>;
      ChartPoint chartPoint1 = new ChartPoint();
      chartPoint1.X = this.ActualXAxis.VisibleRange.Start;
      if (this.IsSideBySide)
      {
        DoubleRange sideBySideInfo = this.GetSideBySideInfo(this);
        chartPoint1.X = this.ActualXAxis.VisibleRange.Start + sideBySideInfo.Start;
      }
      chartPoint1.Y = this.ActualYAxis.VisibleRange.Start;
      ChartPoint chartPoint2 = new ChartPoint(this.ActualArea.PointToValue(this.ActualXAxis, point), this.ActualArea.PointToValue(this.ActualYAxis, point));
      for (int index = 0; index < this.DataCount; ++index)
      {
        double x1 = actualXvalues[index];
        double y = this.ActualSeriesYValues[0][index];
        if (this.ActualXAxis is LogarithmicAxis)
        {
          LogarithmicAxis actualXaxis = this.ActualXAxis as LogarithmicAxis;
          if (Math.Abs(chartPoint2.X - x1) <= Math.Abs(chartPoint2.X - chartPoint1.X) && Math.Log(chartPoint2.X, actualXaxis.LogarithmicBase) > this.ActualXAxis.VisibleRange.Start && Math.Log(chartPoint2.X, actualXaxis.LogarithmicBase) < this.ActualXAxis.VisibleRange.End)
          {
            chartPoint1 = new ChartPoint(x1, y);
            x = actualXvalues[index];
            this.CalculateYValues(technicalIndicator, index, out y1, out y2, x);
          }
        }
        else if (Math.Abs(chartPoint2.X - x1) <= Math.Abs(chartPoint2.X - chartPoint1.X) && chartPoint2.X > this.ActualXAxis.VisibleRange.Start && chartPoint2.X < this.ActualXAxis.VisibleRange.End)
        {
          chartPoint1 = new ChartPoint(x1, y);
          x = actualXvalues[index];
          this.CalculateYValues(technicalIndicator, index, out y1, out y2, x);
        }
      }
    }
  }

  internal virtual void GeneratePropertyPoints(string[] yPaths, IList<double>[] yLists)
  {
    IEnumerator enumerator = (this.ItemsSource as IEnumerable).GetEnumerator();
    if (enumerator == null || this.ActualData == null || yPaths == null || yPaths.Length < 1 || yLists == null || yLists.Length < 1)
      return;
    if (enumerator.MoveNext())
    {
      if (enumerator.Current is ICustomTypeDescriptor)
      {
        this.GenerateCustomTypeDescriptorPropertyPoints(yPaths, yLists, enumerator);
      }
      else
      {
        for (int index = 0; index < this.UpdateStartedIndex; ++index)
          enumerator.MoveNext();
        PropertyInfo propertyInfo1 = ChartDataUtils.GetPropertyInfo(enumerator.Current, this.XBindingPath);
        IPropertyAccessor propertyAccessor1 = (IPropertyAccessor) null;
        if (propertyInfo1 != (PropertyInfo) null)
          propertyAccessor1 = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo1);
        if (propertyAccessor1 == null)
          return;
        System.Func<object, object> getMethod1 = propertyAccessor1.GetMethod;
        if (getMethod1(enumerator.Current) != null && getMethod1(enumerator.Current).GetType().IsArray)
          return;
        this.XAxisValueType = ChartSeriesBase.GetDataType(propertyAccessor1, this.ItemsSource as IEnumerable);
        if (this.XAxisValueType == ChartValueType.DateTime || this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic || this.XAxisValueType == ChartValueType.TimeSpan)
        {
          if (!(this.ActualXValues is List<double>))
            this.ActualXValues = this.XValues = (IEnumerable) new List<double>();
        }
        else if (!(this.ActualXValues is List<string>))
          this.ActualXValues = this.XValues = (IEnumerable) new List<string>();
        if (this.IsMultipleYPathRequired)
        {
          List<IPropertyAccessor> propertyAccessorList = new List<IPropertyAccessor>();
          if (string.IsNullOrEmpty(yPaths[0]))
            return;
          for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
          {
            PropertyInfo propertyInfo2 = ChartDataUtils.GetPropertyInfo(enumerator.Current, yPaths[index]);
            if (propertyInfo2 == (PropertyInfo) null)
              return;
            IPropertyAccessor propertyAccessor2 = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo2);
            if (propertyAccessor2 == null || propertyAccessor2.GetValue(enumerator.Current) != null && propertyAccessor2.GetValue(enumerator.Current).GetType().IsArray)
              return;
            propertyAccessorList.Add(propertyAccessor2);
          }
          if (this.XAxisValueType == ChartValueType.String)
          {
            IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
            do
            {
              object obj1 = getMethod1(enumerator.Current);
              xvalues.Add((string) obj1);
              for (int index = 0; index < propertyAccessorList.Count; ++index)
              {
                object obj2 = propertyAccessorList[index].GetValue(enumerator.Current);
                yLists[index].Add(Convert.ToDouble(obj2 ?? (object) double.NaN));
              }
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.dataCount = xvalues.Count;
          }
          else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
          {
            IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
            do
            {
              this.XData = Convert.ToDouble(getMethod1(enumerator.Current) ?? (object) double.NaN);
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              for (int index = 0; index < propertyAccessorList.Count; ++index)
              {
                object obj = propertyAccessorList[index].GetValue(enumerator.Current);
                yLists[index].Add(Convert.ToDouble(obj ?? (object) double.NaN));
              }
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.dataCount = xvalues.Count;
          }
          else if (this.XAxisValueType == ChartValueType.DateTime)
          {
            IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
            do
            {
              this.XData = ((DateTime) getMethod1(enumerator.Current)).ToOADate();
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              for (int index = 0; index < propertyAccessorList.Count; ++index)
              {
                object obj = propertyAccessorList[index].GetValue(enumerator.Current);
                yLists[index].Add(Convert.ToDouble(obj ?? (object) double.NaN));
              }
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.dataCount = xvalues.Count;
          }
          else if (this.XAxisValueType == ChartValueType.TimeSpan)
          {
            IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
            do
            {
              this.XData = ((TimeSpan) getMethod1(enumerator.Current)).TotalMilliseconds;
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              for (int index = 0; index < propertyAccessorList.Count; ++index)
              {
                object obj = propertyAccessorList[index].GetValue(enumerator.Current);
                yLists[index].Add(Convert.ToDouble(obj ?? (object) double.NaN));
              }
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.dataCount = xvalues.Count;
          }
        }
        else
        {
          string path = string.Empty;
          bool flag = this is RangeColumnSeries;
          for (int index = 0; index < yPaths.Length; ++index)
          {
            if (string.IsNullOrEmpty(yPaths[index]) && !flag)
              return;
            if (flag && !this.IsMultipleYPathRequired)
            {
              if (!string.IsNullOrEmpty(yPaths[index]))
              {
                path = yPaths[index];
                break;
              }
            }
            else
              path = yPaths[0];
          }
          PropertyInfo propertyInfo3 = ChartDataUtils.GetPropertyInfo(enumerator.Current, path);
          IPropertyAccessor propertyAccessor3 = (IPropertyAccessor) null;
          if (propertyInfo3 != (PropertyInfo) null)
            propertyAccessor3 = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo3);
          if (propertyAccessor3 == null)
            return;
          IList<double> yList = yLists[0];
          if (propertyAccessor3 == null)
            return;
          System.Func<object, object> getMethod2 = propertyAccessor3.GetMethod;
          if (getMethod2(enumerator.Current) != null && getMethod2(enumerator.Current).GetType().IsArray)
            return;
          if (this.XAxisValueType == ChartValueType.String)
          {
            IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
            do
            {
              object obj3 = getMethod1(enumerator.Current);
              object obj4 = getMethod2(enumerator.Current);
              xvalues.Add(obj3 != null ? (string) obj3 : string.Empty);
              yList.Add(Convert.ToDouble(obj4 ?? (object) double.NaN));
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.dataCount = xvalues.Count;
          }
          else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
          {
            IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
            do
            {
              object obj5 = getMethod1(enumerator.Current);
              object obj6 = getMethod2(enumerator.Current);
              this.XData = Convert.ToDouble(obj5 ?? (object) double.NaN);
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              yList.Add(Convert.ToDouble(obj6 ?? (object) double.NaN));
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.dataCount = xvalues.Count;
          }
          else if (this.XAxisValueType == ChartValueType.DateTime)
          {
            IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
            do
            {
              object obj7 = getMethod1(enumerator.Current);
              object obj8 = getMethod2(enumerator.Current);
              this.XData = ((DateTime) obj7).ToOADate();
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              yList.Add(Convert.ToDouble(obj8 ?? (object) double.NaN));
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.dataCount = xvalues.Count;
          }
          else if (this.XAxisValueType == ChartValueType.TimeSpan)
          {
            IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
            do
            {
              object obj9 = getMethod1(enumerator.Current);
              object obj10 = getMethod2(enumerator.Current);
              this.XData = ((TimeSpan) obj9).TotalMilliseconds;
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              yList.Add(Convert.ToDouble(obj10 ?? (object) double.NaN));
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.dataCount = xvalues.Count;
          }
        }
        this.HookPropertyChangedEvent(this.ListenPropertyChange);
      }
    }
    this.IsPointGenerated = true;
  }

  internal virtual void GenerateComplexPropertyPoints(
    string[] yPaths,
    IList<double>[] yLists,
    ChartSeriesBase.GetReflectedProperty getPropertyValue)
  {
    IEnumerator enumerator = (this.ItemsSource as IEnumerable).GetEnumerator();
    if (enumerator.MoveNext())
    {
      for (int index = 0; index < this.UpdateStartedIndex; ++index)
        enumerator.MoveNext();
      this.XAxisValueType = this.GetDataType(this.ItemsSource as IEnumerable, this.XComplexPaths);
      if (this.XAxisValueType == ChartValueType.DateTime || this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic || this.XAxisValueType == ChartValueType.TimeSpan)
      {
        if (!(this.XValues is List<double>))
          this.ActualXValues = this.XValues = (IEnumerable) new List<double>();
      }
      else if (!(this.XValues is List<string>))
        this.ActualXValues = this.XValues = (IEnumerable) new List<string>();
      if (this.IsMultipleYPathRequired)
      {
        if (string.IsNullOrEmpty(yPaths[0]))
          return;
        if (getPropertyValue(enumerator.Current, this.XComplexPaths) == null)
          return;
        for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
        {
          if (getPropertyValue(enumerator.Current, this.YComplexPaths[index]) == null)
            return;
        }
        if (this.XAxisValueType == ChartValueType.String)
        {
          IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
          do
          {
            object obj1 = getPropertyValue(enumerator.Current, this.XComplexPaths);
            xvalues.Add((string) obj1);
            for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
            {
              object obj2 = getPropertyValue(enumerator.Current, this.YComplexPaths[index]);
              yLists[index].Add(Convert.ToDouble(obj2 ?? (object) double.NaN));
            }
            this.ActualData.Add(enumerator.Current);
          }
          while (enumerator.MoveNext());
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          do
          {
            this.XData = Convert.ToDouble(getPropertyValue(enumerator.Current, this.XComplexPaths) ?? (object) double.NaN);
            if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
              this.isLinearData = false;
            xvalues.Add(this.XData);
            for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
            {
              object obj = getPropertyValue(enumerator.Current, this.YComplexPaths[index]);
              yLists[index].Add(Convert.ToDouble(obj ?? (object) double.NaN));
            }
            this.ActualData.Add(enumerator.Current);
          }
          while (enumerator.MoveNext());
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.DateTime)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          do
          {
            this.XData = ((DateTime) getPropertyValue(enumerator.Current, this.XComplexPaths)).ToOADate();
            if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
              this.isLinearData = false;
            xvalues.Add(this.XData);
            for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
            {
              object obj = getPropertyValue(enumerator.Current, this.YComplexPaths[index]);
              yLists[index].Add(Convert.ToDouble(obj ?? (object) double.NaN));
            }
            this.ActualData.Add(enumerator.Current);
          }
          while (enumerator.MoveNext());
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.TimeSpan)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          do
          {
            this.XData = ((TimeSpan) getPropertyValue(enumerator.Current, this.XComplexPaths)).TotalMilliseconds;
            if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
              this.isLinearData = false;
            xvalues.Add(this.XData);
            for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
            {
              object obj = getPropertyValue(enumerator.Current, this.YComplexPaths[index]);
              yLists[index].Add(Convert.ToDouble(obj ?? (object) double.NaN));
            }
            this.ActualData.Add(enumerator.Current);
          }
          while (enumerator.MoveNext());
          this.dataCount = xvalues.Count;
        }
      }
      else
      {
        string[] ycomplexPath = this.YComplexPaths[0];
        if (string.IsNullOrEmpty(yPaths[0]))
          return;
        IList<double> yList = yLists[0];
        if (this.XAxisValueType == ChartValueType.String)
        {
          IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
          do
          {
            object obj3 = getPropertyValue(enumerator.Current, this.XComplexPaths);
            object obj4 = getPropertyValue(enumerator.Current, ycomplexPath);
            if (obj3 == null)
              return;
            xvalues.Add((string) obj3);
            yList.Add(Convert.ToDouble(obj4 ?? (object) double.NaN));
            this.ActualData.Add(enumerator.Current);
          }
          while (enumerator.MoveNext());
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          do
          {
            object obj5 = getPropertyValue(enumerator.Current, this.XComplexPaths);
            object obj6 = getPropertyValue(enumerator.Current, ycomplexPath);
            if (obj5 == null)
              return;
            this.XData = Convert.ToDouble(obj5);
            if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
              this.isLinearData = false;
            xvalues.Add(this.XData);
            yList.Add(Convert.ToDouble(obj6 ?? (object) double.NaN));
            this.ActualData.Add(enumerator.Current);
          }
          while (enumerator.MoveNext());
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.DateTime)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          do
          {
            object obj7 = getPropertyValue(enumerator.Current, this.XComplexPaths);
            object obj8 = getPropertyValue(enumerator.Current, ycomplexPath);
            if (obj7 == null)
              return;
            this.XData = ((DateTime) obj7).ToOADate();
            if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
              this.isLinearData = false;
            xvalues.Add(this.XData);
            yList.Add(Convert.ToDouble(obj8 ?? (object) double.NaN));
            this.ActualData.Add(enumerator.Current);
          }
          while (enumerator.MoveNext());
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.TimeSpan)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          do
          {
            object obj9 = getPropertyValue(enumerator.Current, this.XComplexPaths);
            object obj10 = getPropertyValue(enumerator.Current, ycomplexPath);
            if (obj9 == null)
              return;
            this.XData = ((TimeSpan) obj9).TotalMilliseconds;
            if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
              this.isLinearData = false;
            xvalues.Add(this.XData);
            yList.Add(Convert.ToDouble(obj10 ?? (object) double.NaN));
            this.ActualData.Add(enumerator.Current);
          }
          while (enumerator.MoveNext());
          this.dataCount = xvalues.Count;
        }
      }
      this.HookPropertyChangedEvent(this.ListenPropertyChange);
    }
    this.IsPointGenerated = true;
  }

  internal virtual void OnDataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    switch (e.Action)
    {
      case NotifyCollectionChangedAction.Add:
        if (this.ItemsSource != null)
        {
          if (!this.isNotificationSuspended)
          {
            this.SetIndividualPoint(e.NewStartingIndex, e.NewItems[0], false);
            if (this.SegmentColorPath != null && this.IsColorPathSeries && this.ColorValues.Count > 0 && e.NewStartingIndex == 0 && this.ActualArea.Legend != null)
              this.ActualArea.IsUpdateLegend = true;
            if (this.IsSortData)
              this.SortActualPoints();
            if (this.ToggledLegendIndex.Count > 0 && this.IsSingleAccumulationSeries)
            {
              List<int> intList = new List<int>();
              foreach (int num in this.ToggledLegendIndex)
              {
                if (e.NewStartingIndex <= num)
                  intList.Add(num + 1);
                else
                  intList.Add(num);
              }
              this.ToggledLegendIndex = intList;
            }
            this.UpdateSegments(e.NewStartingIndex, e.Action);
            break;
          }
          if (!this.isUpdateStarted)
          {
            this.UpdateStartedIndex = e.NewStartingIndex;
            this.isUpdateStarted = true;
            break;
          }
          break;
        }
        break;
      case NotifyCollectionChangedAction.Remove:
        if (this.ItemsSource != null && e.OldStartingIndex < this.DataCount)
        {
          if (this.XValues is IList<double>)
          {
            (this.XValues as IList<double>).RemoveAt(e.OldStartingIndex);
            --this.dataCount;
          }
          else if (this.XValues is IList<string>)
          {
            (this.XValues as IList<string>).RemoveAt(e.OldStartingIndex);
            --this.dataCount;
          }
          if (this.SegmentColorPath != null && this.IsColorPathSeries)
          {
            if (this.ColorValues != null && this.ColorValues.Count > 0)
              this.ColorValues.RemoveAt(e.OldStartingIndex);
            if (e.OldStartingIndex == 0 && this.ActualArea.Legend != null)
              this.ActualArea.IsUpdateLegend = true;
          }
          for (int index = 0; index < ((IEnumerable<IList<double>>) this.SeriesYValues).Count<IList<double>>(); ++index)
            this.SeriesYValues[index].RemoveAt(e.OldStartingIndex);
          if (this is XyzDataSeries3D xyzDataSeries3D && xyzDataSeries3D.ZValues != null && xyzDataSeries3D.ZValues.GetEnumerator().MoveNext())
          {
            if (xyzDataSeries3D.ZValues is IList<double>)
              (xyzDataSeries3D.ZValues as IList<double>).RemoveAt(e.OldStartingIndex);
            else if (xyzDataSeries3D.ZValues is IList<string>)
              (xyzDataSeries3D.ZValues as IList<string>).RemoveAt(e.OldStartingIndex);
          }
          if (this.IsSortData)
            this.SortActualPoints();
          this.ActualData.RemoveAt(e.OldStartingIndex);
          if (this.ToggledLegendIndex.Count > 0 && this.IsSingleAccumulationSeries)
          {
            List<int> intList = new List<int>();
            foreach (int num in this.ToggledLegendIndex)
            {
              if (e.OldStartingIndex < num)
                intList.Add(num - 1);
              else if (e.OldStartingIndex != num)
                intList.Add(num);
            }
            this.ToggledLegendIndex = intList;
          }
          if (!this.isNotificationSuspended)
          {
            this.UpdateSegments(e.OldStartingIndex, e.Action);
            this.UnHookPropertyChangedEvent(this.ListenPropertyChange, e.OldItems[0]);
            break;
          }
          this.UpdateStartedIndex = this.UpdateStartedIndex != 0 ? this.UpdateStartedIndex - e.OldItems.Count : this.UpdateStartedIndex;
          break;
        }
        break;
      case NotifyCollectionChangedAction.Replace:
        if (e.NewStartingIndex > -1 && e.NewStartingIndex < this.DataCount && e.NewItems[0] != null && !this.isNotificationSuspended)
        {
          this.SetIndividualPoint(e.NewStartingIndex, e.NewItems[0], true);
          if (this.IsSortData)
            this.SortActualPoints();
          this.UpdateSegments(e.OldStartingIndex, e.Action);
          break;
        }
        break;
      default:
        this.Refresh();
        if (this.ActualArea is SfChart actualArea)
        {
          bool flag = actualArea.Series.Any<ChartSeries>((System.Func<ChartSeries, bool>) (chartseries => !chartseries.IsBitmapSeries));
          if (this.IsBitmapSeries && flag && actualArea.fastRenderSurface != null)
          {
            actualArea.UpdateBitmapSeries(this as ChartSeries, false);
            break;
          }
          break;
        }
        break;
    }
    this.isPointValidated = false;
    if (this.ShowEmptyPoints)
      this.RevalidateEmptyPointsCollection(e.Action, e.NewStartingIndex, e.OldStartingIndex);
    if (this is AccumulationSeriesBase || this.ActualArea.HasDataPointBasedLegend())
      this.ActualArea.IsUpdateLegend = true;
    this.totalCalculated = false;
    if (this.ActualXAxis is ChartAxisBase2D actualXaxis)
      actualXaxis.CanAutoScroll = true;
    if (!(this.ActualYAxis is ChartAxisBase2D actualYaxis))
      return;
    actualYaxis.CanAutoScroll = true;
  }

  internal virtual void CalculateSegments()
  {
    this.ApplyTemplate();
    if (this.ActualYAxis is LogarithmicAxis && this.ActualSeriesYValues != null)
    {
      foreach (IList<double> actualSeriesYvalue in this.ActualSeriesYValues)
        this.ValidateLogEmptyValues(actualSeriesYvalue);
    }
    if (this.ActualXAxis is LogarithmicAxis)
      this.ValidateLogEmptyValues((IList<double>) (this.ActualXValues as List<double>));
    if (this.dataCount > 0)
    {
      if (!this.isPointValidated)
      {
        if (this.ShowEmptyPoints)
        {
          this.ValidateYValues();
          this.isPointValidated = true;
        }
        else if (this.EmptyPointIndexes != null)
          this.ReValidateYValues(this.EmptyPointIndexes);
      }
      this.isPointValidated = false;
      this.CreateSegments();
      if (this.ActualArea.HasDataPointBasedLegend())
        this.ActualArea.IsUpdateLegend = true;
      int num1;
      switch (this)
      {
        case ISegmentSelectable _:
          num1 = (this as ISegmentSelectable).SelectedIndex;
          break;
        case ChartSeries3D _:
          num1 = (this as ChartSeries3D).SelectedIndex;
          break;
        default:
          num1 = -1;
          break;
      }
      int num2 = num1;
      if (!this.triggerSelectionChangedEventOnLoad || num2 < 0 || this.DataCount <= num2)
        return;
      this.RaiseSelectionChangedEvent();
    }
    else
    {
      if (this.Segments == null || this.dataCount != 0 || this.Segments.Count <= 0)
        return;
      this.ClearUnUsedSegments(this.dataCount);
    }
  }

  private void ValidateLogEmptyValues(IList<double> yValue)
  {
    if (!yValue.Any<double>((System.Func<double, bool>) (x => x <= 0.0)))
      return;
    for (int index = 0; index < yValue.Count; ++index)
    {
      if (yValue[index] <= 0.0)
        yValue[index] = double.NaN;
    }
  }

  internal virtual void UpdateEmptyPointSegments(List<double> xValues, bool isSidebySideSeries)
  {
    int index1 = 0;
    DoubleRange doubleRange = isSidebySideSeries ? this.GetSideBySideInfo(this) : new DoubleRange();
    if (this.EmptyPointIndexes == null || ((IEnumerable<List<int>>) this.EmptyPointIndexes).Count<List<int>>() <= 0)
      return;
    foreach (IList<double> actualSeriesYvalue in this.ActualSeriesYValues)
    {
      switch (this.EmptyPointStyle)
      {
        case EmptyPointStyle.Interior:
          if (((IEnumerable<List<int>>) this.EmptyPointIndexes).Count<List<int>>() > index1)
          {
            foreach (int num in this.EmptyPointIndexes[index1])
            {
              int index2 = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? (this is FunnelSeries ? this.DataCount - 1 - num : num) : xValues.Count - 1;
              switch (this)
              {
                case LineSeries _:
                case SplineSeries _:
                case StepLineSeries _:
                case StackingLineSeries _:
label_8:
                  if (num != 0)
                  {
                    this.Segments[index2 - 1].IsEmptySegmentInterior = true;
                    break;
                  }
                  break;
                case PolarRadarSeriesBase _:
                  if ((this as PolarRadarSeriesBase).DrawType != ChartSeriesDrawType.Line)
                    break;
                  goto label_8;
              }
              if (this.Segments.Count > index2)
                this.Segments[this.Segments.Count == index2 ? index2 - 1 : index2].IsEmptySegmentInterior = true;
              if (this.Adornments != null && index2 >= 0 && this.Adornments.Count > index2)
                this.Adornments[index2].IsEmptySegmentInterior = true;
            }
          }
          ++index1;
          break;
        case EmptyPointStyle.Symbol:
          if (((IEnumerable<List<int>>) this.EmptyPointIndexes).Count<List<int>>() > index1)
          {
            foreach (int index3 in this.EmptyPointIndexes[index1])
            {
              double xData = xValues[index3] + (doubleRange.Start + doubleRange.End) / 2.0;
              int index4 = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? (this is FunnelSeries ? this.DataCount - 1 - index3 : index3) : xValues.Count - 1;
              switch (this)
              {
                case LineSeries _:
                case SplineSeries _:
                case StepLineSeries _:
                case StackingLineSeries _:
                case PolarRadarSeriesBase _ when (this as PolarRadarSeriesBase).DrawType == ChartSeriesDrawType.Line && index3 != 0:
                  this.Segments[index4 - 1].Interior = (Brush) new SolidColorBrush(Color.FromArgb((byte) 0, byte.MaxValue, byte.MaxValue, byte.MaxValue));
                  ObservableCollection<ChartSegment> segments = this.Segments;
                  int index5 = this.Segments.Count == index4 ? index4 - 1 : index4;
                  EmptyPointSegment emptyPointSegment1 = new EmptyPointSegment(xData, actualSeriesYvalue[index4], this, false);
                  emptyPointSegment1.Item = this.ActualData[index4];
                  EmptyPointSegment emptyPointSegment2 = emptyPointSegment1;
                  segments[index5] = (ChartSegment) emptyPointSegment2;
                  goto label_24;
                case StackingAreaSeries _:
label_24:
                  if (this.Adornments != null && index4 >= 0 && this.Adornments.Count > index4)
                  {
                    this.Adornments[index4].IsEmptySegmentInterior = false;
                    continue;
                  }
                  continue;
                default:
                  this.Segments[index4] = (ChartSegment) new EmptyPointSegment(xData, actualSeriesYvalue[index4], this, false);
                  this.Segments[index4].Item = this.ActualData[index4];
                  goto label_24;
              }
            }
          }
          ++index1;
          break;
        case EmptyPointStyle.SymbolAndInterior:
          if (((IEnumerable<List<int>>) this.EmptyPointIndexes).Count<List<int>>() > index1)
          {
            foreach (int num in this.EmptyPointIndexes[index1])
            {
              int index6 = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? (this is FunnelSeries ? this.DataCount - 1 - num : num) : xValues.Count - 1;
              double xData = xValues[index6] + (doubleRange.Start + doubleRange.End) / 2.0;
              switch (this)
              {
                case LineSeries _:
                case SplineSeries _:
                case StepLineSeries _:
                case StackingLineSeries _:
                case PolarRadarSeriesBase _ when (this as PolarRadarSeriesBase).DrawType == ChartSeriesDrawType.Line && num != 0:
                  this.Segments[index6 - 1].Interior = (Brush) new SolidColorBrush(Color.FromArgb((byte) 0, byte.MaxValue, byte.MaxValue, byte.MaxValue));
                  ObservableCollection<ChartSegment> segments = this.Segments;
                  int index7 = this.Segments.Count == index6 ? index6 - 1 : index6;
                  EmptyPointSegment emptyPointSegment3 = new EmptyPointSegment(xData, actualSeriesYvalue[index6], this, true);
                  emptyPointSegment3.Item = this.ActualData[index6];
                  EmptyPointSegment emptyPointSegment4 = emptyPointSegment3;
                  segments[index7] = (ChartSegment) emptyPointSegment4;
                  goto label_35;
                case StackingAreaSeries _:
label_35:
                  if (this.Adornments != null && index6 >= 0 && this.Adornments.Count > index6)
                  {
                    this.Adornments[index6].IsEmptySegmentInterior = true;
                    continue;
                  }
                  continue;
                default:
                  this.Segments[this.Segments.Count == index6 ? index6 - 1 : index6].IsEmptySegmentInterior = true;
                  this.Segments[index6] = (ChartSegment) new EmptyPointSegment(xData, actualSeriesYvalue[index6], this, true);
                  this.Segments[index6].Item = this.ActualData[index6];
                  goto label_35;
              }
            }
          }
          ++index1;
          break;
      }
    }
  }

  internal virtual void UpdateOnSeriesBoundChanged(Size size)
  {
    if (this.Segments == null)
      return;
    if (this.SeriesPanel == null && this.Segments.Count > 0)
      this.SeriesPanel = this.Segments[0].Series.GetTemplateChild("seriesPanel") as ChartSeriesPanel;
    if (this.SeriesPanel == null)
      return;
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
      segment.OnSizeChanged(size);
    this.SeriesPanel.Update(size);
  }

  internal virtual void UpdateRange()
  {
  }

  internal virtual void ValidateYValues()
  {
  }

  internal virtual void ReValidateYValues(List<int>[] emptyPointIndexs)
  {
  }

  internal virtual bool RaiseSelectionChanging(int newIndex, int oldIndex)
  {
    this.selectionChangingEventArgs.SelectedSegments = this.ActualArea.SelectedSegments;
    if (!this.IsBitmapSeries && !this.IsAreaTypeSeries)
    {
      switch (this)
      {
        case FastLineSeries _:
          break;
        case ISegmentSelectable _:
          this.selectionChangingEventArgs.SelectedSegment = newIndex < 0 || newIndex >= this.Segments.Count ? (ChartSegment) null : this.Segments[newIndex];
          goto label_4;
        default:
          goto label_4;
      }
    }
    this.selectionChangingEventArgs.SelectedSegment = newIndex != -1 ? (ChartSegment) this.GetDataPoint(newIndex) : (ChartSegment) null;
label_4:
    this.SetSelectionChangingEventArgs(newIndex, oldIndex);
    this.ActualArea.OnSelectionChanging(this.selectionChangingEventArgs);
    return this.selectionChangingEventArgs.Cancel;
  }

  internal virtual ChartDataPointInfo GetDataPoint(Point mousePos)
  {
    double x = double.NaN;
    double y = double.NaN;
    double stackedYValue = double.NaN;
    this.dataPoint = new ChartDataPointInfo();
    int index = -1;
    if (this.ActualArea.SeriesClipRect.Contains(mousePos))
    {
      Point point = new Point(mousePos.X - this.ActualArea.SeriesClipRect.Left, mousePos.Y - this.ActualArea.SeriesClipRect.Top);
      if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
      {
        double start = this.ActualXAxis.VisibleRange.Start;
        double end = this.ActualXAxis.VisibleRange.End;
        point = new Point(this.ActualArea.PointToValue(this.ActualXAxis, point), this.ActualArea.PointToValue(this.ActualYAxis, point));
        double num = Math.Round(point.X);
        if (num <= end && num >= start && num >= 0.0)
        {
          x = num;
          List<double> doubleList = new List<double>();
          if (this.DistinctValuesIndexes.Count > 0 && this.DistinctValuesIndexes.ContainsKey(x))
          {
            List<double> list = this.DistinctValuesIndexes[x].Select<int, double>((System.Func<int, double>) (value => this.ActualSeriesYValues[0][value])).ToList<double>();
            if (this is FastStackingColumnBitmapSeries)
            {
              list.Reverse();
              y = list[0];
            }
            else
              y = list[0];
          }
        }
        this.dataPoint.XData = x;
        if (!double.IsNaN(x))
          index = this.DistinctValuesIndexes[x][0];
        this.dataPoint.YData = y;
        this.dataPoint.Index = index;
        this.dataPoint.Series = this;
      }
      else
      {
        this.FindNearestChartPoint(point, out x, out y, out stackedYValue);
        this.dataPoint.XData = x;
        index = this.GetXValues().IndexOf(x);
        this.dataPoint.YData = y;
        this.dataPoint.Index = index;
        this.dataPoint.Series = this;
      }
      if (index > -1 && this.ActualData.Count > index)
      {
        ChartDataPointInfo dataPoint = this.dataPoint;
        object obj;
        switch (this)
        {
          case ColumnSeries _:
          case FastColumnBitmapSeries _:
          case StackingColumnSeries _:
          case BarSeries _:
          case FastScatterBitmapSeries _:
          case StackingBarSeries _:
          case FastStackingColumnBitmapSeries _:
            if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
            {
              obj = this.GroupedActualData[index];
              break;
            }
            goto default;
          default:
            obj = this.dataPoint.Item = this.ActualData[index];
            break;
        }
        dataPoint.Item = obj;
      }
    }
    return this.dataPoint;
  }

  internal virtual void ResetAdornmentAnimationState()
  {
    if (this.adornmentInfo == null || !this.adornmentInfo.ShowLabel || this.adornmentInfo.LabelPresenters == null)
      return;
    foreach (FrameworkElement labelPresenter in this.adornmentInfo.LabelPresenters)
    {
      labelPresenter.ClearValue(UIElement.RenderTransformProperty);
      labelPresenter.ClearValue(UIElement.OpacityProperty);
    }
  }

  internal virtual void Animate()
  {
  }

  internal virtual void Dispose()
  {
    if (this.adornmentInfo != null)
    {
      this.adornmentInfo.Dispose();
      this.adornmentInfo = (ChartAdornmentInfoBase) null;
    }
    if (this.m_adornments != null)
    {
      foreach (ChartAdornment adornment in (Collection<ChartAdornment>) this.m_adornments)
      {
        adornment.Series = (ChartSeriesBase) null;
        adornment.Dispose();
      }
      this.m_adornments.Clear();
    }
    if (this.m_visibleAdornments != null)
    {
      foreach (ChartAdornment visibleAdornment in (Collection<ChartAdornment>) this.m_visibleAdornments)
      {
        visibleAdornment.Series = (ChartSeriesBase) null;
        visibleAdornment.Dispose();
      }
      this.m_visibleAdornments.Clear();
    }
    if (this.AdornmentPresenter != null)
    {
      if (this.AdornmentPresenter.VisibleSeries != null)
        this.AdornmentPresenter.VisibleSeries.Clear();
      this.AdornmentPresenter.Series = (ChartSeriesBase) null;
      this.AdornmentPresenter.Children.Clear();
      this.AdornmentPresenter = (ChartAdornmentPresenter) null;
    }
    if (this.PropertyChanged != null)
    {
      foreach (Delegate invocation in this.PropertyChanged.GetInvocationList())
        this.PropertyChanged -= invocation as PropertyChangedEventHandler;
      this.PropertyChanged = (PropertyChangedEventHandler) null;
    }
    if (this.SeriesPanel != null)
    {
      this.SeriesPanel.Dispose();
      this.SeriesPanel = (ChartSeriesPanel) null;
    }
    this.ItemsSource = (object) null;
    if (this.Segments != null)
    {
      foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
        segment.Dispose();
      this.Segments.Clear();
      this.Segments = (ObservableCollection<ChartSegment>) null;
    }
    if (this.ColorModel != null)
      this.ColorModel.Dispose();
    if (this.SelectedSegmentsIndexes != null)
      this.SelectedSegmentsIndexes.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.SelectedSegmentsIndexes_CollectionChanged);
    this.ActualArea = (ChartBase) null;
    this.ChartTransformer = (IChartTransformer) null;
    this.dataPoint = (ChartDataPointInfo) null;
    this.SeriesRootPanel = (Panel) null;
    this.ToolTipTag = (object) null;
  }

  internal string Serialize()
  {
    SerializationBindingHelper.Register<BindingExpression, BindingConvertor>();
    StringBuilder result = new StringBuilder();
    ChartBase.GetSerializedString(out result, (object) this);
    return result.ToString();
  }

  internal object GetSegment(object item)
  {
    return (object) this.Segments.Where<ChartSegment>((System.Func<ChartSegment, bool>) (segment => segment.Item == item)).FirstOrDefault<ChartSegment>();
  }

  internal TooltipPosition ActualTooltipPosition { get; set; }

  internal void UpdateSeriesTooltip(object customTag)
  {
    if (customTag == null)
      return;
    this.ToolTipTag = customTag;
    ChartTooltip chartTooltip1 = this.ActualArea.Tooltip;
    chartTooltip1.PolygonPath = " ";
    ChartTooltipBehavior tooltipBehavior = this.ActualArea.TooltipBehavior;
    this.SetTooltipDuration();
    Canvas adorningCanvas = this.ActualArea.GetAdorningCanvas();
    if (chartTooltip1 == null || adorningCanvas == null)
      return;
    if (!this.IsTooltipAvailable(adorningCanvas))
    {
      chartTooltip1.DataContext = (object) (customTag as ChartSegment);
      if (chartTooltip1.DataContext == null)
        return;
      if (ChartTooltip.GetActualInitialShowDelay(tooltipBehavior, ChartTooltip.GetInitialShowDelay((DependencyObject) this)) == 0)
      {
        this.HastoolTip = true;
        adorningCanvas.Children.Add((UIElement) chartTooltip1);
      }
      if (this.TooltipTemplate != null)
      {
        if (this is BoxAndWhiskerSeries && customTag is ScatterSegment)
        {
          if (this.OutlierTooltipTemplate == null)
            chartTooltip1.ContentTemplate = ChartDictionaries.GenericCommonDictionary[(object) "DefaultTooltipTemplate"] as DataTemplate;
          else
            chartTooltip1.ContentTemplate = this.OutlierTooltipTemplate;
        }
        else
          chartTooltip1.ContentTemplate = this.TooltipTemplate;
      }
      else
        chartTooltip1.ContentTemplate = this.GetTooltipTemplate();
      if (chartTooltip1.ContentTemplate == null)
      {
        if (this.toolTipTemplate == null)
          this.toolTipTemplate = this.ActualArea is SfChart ? ChartDictionaries.GenericCommonDictionary[(object) "DefaultTooltipTemplate"] as DataTemplate : ChartDictionaries.GenericCommonDictionary[(object) "Default3DTooltipTemplate"] as DataTemplate;
        chartTooltip1.ContentTemplate = this.toolTipTemplate;
      }
      this.AddTooltip();
      if (ChartTooltip.GetActualEnableAnimation(tooltipBehavior, ChartTooltip.GetEnableAnimation((UIElement) this)) && this.ActualArea is SfChart)
        this.SetDoubleAnimation(chartTooltip1);
      Canvas.SetLeft((UIElement) chartTooltip1, chartTooltip1.LeftOffset);
      Canvas.SetTop((UIElement) chartTooltip1, chartTooltip1.TopOffset);
      this._stopwatch.Start();
    }
    else
    {
      foreach (object child in adorningCanvas.Children)
      {
        if (child is ChartTooltip chartTooltip2)
          chartTooltip1 = chartTooltip2;
      }
      chartTooltip1.DataContext = customTag;
      if (chartTooltip1.DataContext == null)
      {
        this.RemoveTooltip();
      }
      else
      {
        this.AddTooltip();
        Canvas.SetLeft((UIElement) chartTooltip1, chartTooltip1.LeftOffset);
        Canvas.SetTop((UIElement) chartTooltip1, chartTooltip1.TopOffset);
      }
    }
  }

  internal List<double> GetYValues()
  {
    List<double> yvalues = new List<double>();
    AccumulationSeriesBase accumulationSeriesBase = this as AccumulationSeriesBase;
    for (int index = 0; index < this.DataCount; ++index)
    {
      if (!this.ToggledLegendIndex.Contains(index))
      {
        if (accumulationSeriesBase != null)
          yvalues.Add(accumulationSeriesBase.YValues[index]);
        else
          yvalues.Add((this as CircularSeriesBase3D).YValues[index]);
      }
      else
        yvalues.Add(double.NaN);
    }
    return yvalues;
  }

  internal void ValidateDataPoints(params IList<double>[] yValues)
  {
    if (this.EmptyPointIndexes == null || ((IEnumerable<List<int>>) this.EmptyPointIndexes).Count<List<int>>() == 0)
      this.EmptyPointIndexes = new List<int>[yValues.Length];
    int index1 = 0;
    foreach (IList<double> yValue in yValues)
    {
      if (this.EmptyPointIndexes[index1] == null || this.EmptyPointIndexes[index1].Count == 0)
        this.EmptyPointIndexes[index1] = new List<int>();
      if (yValue.Count != 0)
      {
        switch (this.EmptyPointValue)
        {
          case EmptyPointValue.Zero:
            for (int index2 = 0; index2 < yValue.Count; ++index2)
            {
              if (double.IsNaN(yValue[index2]))
              {
                yValue[index2] = 0.0;
                if (!this.EmptyPointIndexes[index1].Contains(index2))
                  this.EmptyPointIndexes[index1].Add(index2);
              }
            }
            break;
          case EmptyPointValue.Average:
            int index3 = 0;
            if (yValue.Count == 1 && double.IsNaN(yValue[index3]))
            {
              yValue[index3] = 0.0;
              this.EmptyPointIndexes[index1].Add(0);
              break;
            }
            if (double.IsNaN(yValue[index3]))
            {
              yValue[index3] = (0.0 + (double.IsNaN(yValue[index3 + 1]) ? 0.0 : yValue[index3 + 1])) / 2.0;
              if (!this.EmptyPointIndexes[index1].Contains(index3))
                this.EmptyPointIndexes[index1].Add(0);
            }
            for (; index3 < yValue.Count - 1; ++index3)
            {
              if (double.IsNaN(yValue[index3]))
              {
                yValue[index3] = (yValue[index3 - 1] + (double.IsNaN(yValue[index3 + 1]) ? 0.0 : yValue[index3 + 1])) / 2.0;
                if (!this.EmptyPointIndexes[index1].Contains(index3))
                  this.EmptyPointIndexes[index1].Add(index3);
              }
            }
            if (double.IsNaN(yValue[index3]))
            {
              yValue[index3] = yValue[index3 - 1] / 2.0;
              if (!this.EmptyPointIndexes[index1].Contains(index3))
              {
                this.EmptyPointIndexes[index1].Add(index3);
                break;
              }
              break;
            }
            break;
        }
      }
      yValues[index1] = yValue;
      ++index1;
    }
  }

  internal void UpdateLegendIconTemplate(bool iconChanged)
  {
    try
    {
      string key = this.LegendIcon.ToString();
      if (this.LegendIcon == ChartLegendIcon.SeriesType)
        key = !(this is FastScatterBitmapSeries scatterBitmapSeries) ? (!(this is WaterfallSeries | this is FastBarBitmapSeries) ? (!(this is FastStepLineBitmapSeries) ? this.GetType().Name.Replace("Series", "").Replace("3D", "") : "StepLine") : "Column") : (scatterBitmapSeries.ShapeType == ChartSymbol.Ellipse ? "Circle" : scatterBitmapSeries.ShapeType.ToString());
      if (this.GetIconTemplate() != null && !iconChanged || !ChartDictionaries.GenericLegendDictionary.Contains((object) key))
        return;
      this.LegendIconTemplate = ChartDictionaries.GenericLegendDictionary[(object) key] as DataTemplate;
    }
    catch
    {
    }
  }

  internal void SetSelectionChangingEventArgs(int newIndex, int oldIndex)
  {
    this.selectionChangingEventArgs.SelectedIndex = newIndex;
    this.selectionChangingEventArgs.SelectedSeries = this;
    this.selectionChangingEventArgs.PreviousSelectedIndex = oldIndex;
    this.selectionChangingEventArgs.Cancel = false;
    if (this.ActualArea.SelectedSeriesCollection.Count > 0)
    {
      if (this.ActualArea.SelectedSeriesCollection.Contains(this.ActualArea.CurrentSelectedSeries))
        this.selectionChangingEventArgs.IsSelected = true;
      else
        this.selectionChangingEventArgs.IsSelected = false;
    }
    else if (this.SelectedSegmentsIndexes.Count > 0)
    {
      if (this.SelectedSegmentsIndexes.Contains(newIndex))
        this.selectionChangingEventArgs.IsSelected = true;
      else
        this.selectionChangingEventArgs.IsSelected = false;
    }
    else
      this.selectionChangingEventArgs.IsSelected = false;
  }

  internal void RemoveSegments()
  {
    int index = 0;
    while (index < this.Segments.Count)
      this.Segments.RemoveAt(index);
  }

  internal Brush GetFinancialSeriesInterior(int segmentIndex)
  {
    if (this.Interior != null || segmentIndex < 0)
      return this.Interior;
    ChartSegment chartSegment = this.IsBitmapSeries ? this.Segments[0] : this.Segments[segmentIndex];
    switch (chartSegment)
    {
      case CandleSegment _:
        return (chartSegment as CandleSegment).ActualStroke;
      case HiLoOpenCloseSegment _:
        return (chartSegment as HiLoOpenCloseSegment).ActualInterior;
      case FastHiLoOpenCloseSegment _:
        return (Brush) new SolidColorBrush((chartSegment as FastHiLoOpenCloseSegment).GetSegmentBrush(segmentIndex));
      default:
        return (Brush) new SolidColorBrush((chartSegment as FastCandleBitmapSegment).GetSegmentBrush(segmentIndex));
    }
  }

  internal Brush GetInteriorColor(int segmentIndex)
  {
    int seriesIndex = this.ActualArea.GetSeriesIndex(this);
    if (this.Interior != null)
      return this.Interior;
    if (this.SegmentColorPath != null)
    {
      if (this.ColorValues.Count > 0 && this.ColorValues[segmentIndex] != null)
        return this.ColorValues[segmentIndex];
      return this.Palette != ChartColorPalette.None ? this.ColorModel.GetBrush(segmentIndex) : this.ActualArea.ColorModel.GetBrush(seriesIndex);
    }
    if (this.Palette != ChartColorPalette.None)
      return this.ColorModel.GetBrush(segmentIndex);
    if (this.ActualArea.Palette != ChartColorPalette.None)
    {
      if (seriesIndex >= 0)
        return this.ActualArea.ColorModel.GetBrush(seriesIndex);
      if (this.ActualArea is SfChart)
        return this.ActualArea.ColorModel.GetBrush((this.ActualArea as SfChart).TechnicalIndicators.IndexOf(this as ChartSeries));
    }
    return (Brush) new SolidColorBrush(Colors.Transparent);
  }

  internal void CalculateSideBySideInfoPadding(double minWidth, int all, int pos, bool isXAxis)
  {
    XyzDataSeries3D xyzDataSeries3D = this as XyzDataSeries3D;
    ChartAxis chartAxis = isXAxis ? this.ActualXAxis : xyzDataSeries3D.ActualZAxis;
    int num1;
    switch (chartAxis)
    {
      case NumericalAxis _ when (chartAxis as NumericalAxis).RangePadding == NumericalPadding.None:
      case DateTimeAxis _ when (chartAxis as DateTimeAxis).RangePadding == DateTimeRangePadding.None:
      case NumericalAxis3D _ when (chartAxis as NumericalAxis3D).RangePadding == NumericalPadding.None:
        num1 = 1;
        break;
      case DateTimeAxis3D _:
        num1 = (chartAxis as DateTimeAxis3D).RangePadding == DateTimeRangePadding.None ? 1 : 0;
        break;
      default:
        num1 = 0;
        break;
    }
    double num2 = num1 != 0 ? 1.0 - ChartSeriesBase.GetSpacing((DependencyObject) this) : ChartSeriesBase.GetSpacing((DependencyObject) this);
    double num3 = minWidth * num2 / (double) all;
    double start = num3 * (double) (pos - 1) - minWidth * num2 / 2.0;
    double end = start + num3;
    if (isXAxis)
      this.SideBySideInfoRangePad = new DoubleRange(start, end);
    else
      xyzDataSeries3D.ZSideBySideInfoRangePad = new DoubleRange(start, end);
  }

  internal void CalculateSideBySidePositions(bool isXAxis)
  {
    this.ActualArea.SeriesPosition.Clear();
    int length1 = this.ActualArea.RowDefinitions.Count;
    int length2 = this.ActualArea.ColumnDefinitions.Count;
    this.ActualArea.SbsSeriesCount = new int[length1, length2];
    if (length1 == 0)
    {
      this.ActualArea.SbsSeriesCount = new int[1, length2];
      length1 = 1;
    }
    if (length2 == 0)
    {
      this.ActualArea.SbsSeriesCount = new int[length1, 1];
      length2 = 1;
    }
    List<ChartSeriesBase> second = new List<ChartSeriesBase>();
    if (this.ActualArea is SfChart && (this.ActualArea as SfChart).TechnicalIndicators != null)
    {
      foreach (ChartSeries technicalIndicator in (Collection<ChartSeries>) (this.ActualArea as SfChart).TechnicalIndicators)
        second.Add((ChartSeriesBase) technicalIndicator);
    }
    IEnumerable<ChartSeriesBase> first = this.ActualXAxis.RegisteredSeries.Select<ISupportAxes, ChartSeriesBase>((System.Func<ISupportAxes, ChartSeriesBase>) (series => (ChartSeriesBase) series));
    for (int i = 0; i < length1; ++i)
    {
      for (int j = 0; j < length2; ++j)
      {
        int num1 = 0;
        List<ChartSeriesBase> list = (this is ChartSeries ? first.Union<ChartSeriesBase>((IEnumerable<ChartSeriesBase>) second) : first).Select(series => new
        {
          series = series,
          rowPos = series.IsActualTransposed ? (isXAxis ? this.ActualArea.GetActualRow((UIElement) series.ActualXAxis) : this.ActualArea.GetActualRow((UIElement) (series as XyzDataSeries3D).ActualZAxis)) : this.ActualArea.GetActualRow((UIElement) series.ActualYAxis)
        }).Select(_param1 => new
        {
          \u003C\u003Eh__TransparentIdentifier13 = _param1,
          columnPos = _param1.series.IsActualTransposed ? this.ActualArea.GetActualColumn((UIElement) _param1.series.ActualYAxis) : (isXAxis ? this.ActualArea.GetActualColumn((UIElement) _param1.series.ActualXAxis) : this.ActualArea.GetActualColumn((UIElement) (_param1.series as XyzDataSeries3D).ActualZAxis))
        }).Where(_param1 => _param1.columnPos == j && _param1.\u003C\u003Eh__TransparentIdentifier13.rowPos == i).Select(_param0 => _param0.\u003C\u003Eh__TransparentIdentifier13.series).ToList<ChartSeriesBase>();
        int num2 = 0;
        List<ChartSeriesBase> source = new List<ChartSeriesBase>();
        foreach (ChartSeriesBase chartSeriesBase in list)
        {
          ChartSeriesBase item = chartSeriesBase;
          bool flag = false;
          if (item.IsSideBySide && item.IsSeriesVisible)
          {
            if (item.IsStacked)
            {
              if (source.Count == 0)
              {
                ++num1;
                this.ActualArea.SeriesPosition.Add((object) item, num1);
                source.Add(item);
              }
              else
              {
                foreach (ChartSeriesBase key in source.Where<ChartSeriesBase>((System.Func<ChartSeriesBase, bool>) (stackedColumn =>
                {
                  if (stackedColumn.ActualYAxis != item.ActualYAxis)
                    return false;
                  return !(stackedColumn is StackingSeriesBase) ? (stackedColumn as StackingSeriesBase3D).GroupingLabel == (item as StackingSeriesBase3D).GroupingLabel : (stackedColumn as StackingSeriesBase).GroupingLabel == (item as StackingSeriesBase).GroupingLabel;
                })))
                {
                  flag = true;
                  num2 = this.ActualArea.SeriesPosition[(object) key];
                }
                source.Add(item);
                if (flag)
                  this.ActualArea.SeriesPosition.Add((object) item, num2);
                else
                  this.ActualArea.SeriesPosition.Add((object) item, ++num1);
              }
            }
            else
            {
              ++num1;
              this.ActualArea.SeriesPosition.Add((object) item, num1);
            }
          }
        }
        this.ActualArea.SbsSeriesCount[i, j] = num1;
      }
    }
    this.ActualArea.SBSInfoCalculated = true;
  }

  internal object GetArrayPropertyValue(object parentObj, string[] paths)
  {
    for (int index = 0; index < paths.Length; ++index)
    {
      string path = paths[index];
      parentObj = this.GetComplexArrayPropertyValue(parentObj, path);
    }
    return parentObj;
  }

  internal object GetComplexArrayPropertyValue(object parentObj, string path)
  {
    if (parentObj == null)
      return (object) null;
    if (path.Contains<char>('['))
    {
      int int32 = Convert.ToInt32(path.Substring(path.IndexOf('[') + 1, path.IndexOf(']') - path.IndexOf('[') - 1));
      string actualPath = path.Replace(path.Substring(path.IndexOf('[')), string.Empty);
      parentObj = ChartSeriesBase.ReflectedObject(parentObj, actualPath);
      if (parentObj == null)
        return (object) null;
      if (!(parentObj is IList list) || list.Count <= int32)
        return (object) null;
      parentObj = list[int32];
    }
    else
    {
      parentObj = ChartSeriesBase.ReflectedObject(parentObj, path);
      if (parentObj == null)
        return (object) null;
      if (parentObj.GetType().IsArray)
        return (object) null;
    }
    return parentObj;
  }

  internal object GetPropertyValue(object obj, string[] paths)
  {
    object parentObj = obj;
    for (int index = 0; index < paths.Length; ++index)
      parentObj = ChartSeriesBase.ReflectedObject(parentObj, paths[index]);
    return parentObj != null && parentObj.GetType().IsArray ? (object) null : parentObj;
  }

  internal void HookPropertyChangedEvent(bool value)
  {
    if (this.ItemsSource == null || this.ItemsSource is DataTable)
      return;
    IEnumerator enumerator = (this.ItemsSource as IEnumerable).GetEnumerator();
    if (!enumerator.MoveNext() || !(enumerator.Current is INotifyPropertyChanged))
      return;
    if (value)
    {
      do
      {
        if (this.IsComplexYProperty || this.XBindingPath.Contains<char>('.'))
        {
          this.HookComplexProperty(enumerator.Current, this.XComplexPaths);
          for (int index = 0; index < ((IEnumerable<string[]>) this.YComplexPaths).Count<string[]>(); ++index)
            this.HookComplexProperty(enumerator.Current, this.YComplexPaths[index]);
        }
        (enumerator.Current as INotifyPropertyChanged).PropertyChanged -= new PropertyChangedEventHandler(this.OnItemPropertyChanged);
        (enumerator.Current as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(this.OnItemPropertyChanged);
      }
      while (enumerator.MoveNext());
    }
    else
    {
      do
      {
        (enumerator.Current as INotifyPropertyChanged).PropertyChanged -= new PropertyChangedEventHandler(this.OnItemPropertyChanged);
      }
      while (enumerator.MoveNext());
    }
  }

  internal ChartValueType GetDataType(IEnumerable itemSource, string[] paths)
  {
    IEnumerator enumerator = itemSource.GetEnumerator();
    object xval = (object) null;
    if (enumerator.MoveNext())
      xval = this.GetArrayPropertyValue(enumerator.Current, paths);
    return ChartSeriesBase.GetDataType(xval);
  }

  internal DataTemplate GetTooltipTemplate()
  {
    return this.TooltipTemplate != null ? this.TooltipTemplate : this.GetDefaultTooltipTemplate();
  }

  internal void HookPropertyChangedEvent(bool needPropertyChange, object obj)
  {
    if (!needPropertyChange || !(obj is INotifyPropertyChanged notifyPropertyChanged))
      return;
    notifyPropertyChanged.PropertyChanged -= new PropertyChangedEventHandler(this.OnItemPropertyChanged);
    notifyPropertyChanged.PropertyChanged += new PropertyChangedEventHandler(this.OnItemPropertyChanged);
  }

  internal void CalculateHittestRect(
    Point mousePos,
    out int startIndex,
    out int endIndex,
    out Rect rect)
  {
    Point point = new Point();
    Point point1 = new Point();
    Point point2 = new Point();
    IList<double> doubleList = this.ActualXValues is IList<double> ? this.ActualXValues as IList<double> : (IList<double>) this.GetXValues();
    point.X = mousePos.X - this.ActualArea.SeriesClipRect.Left;
    point.Y = mousePos.Y - this.ActualArea.SeriesClipRect.Top;
    double num1 = Math.Floor(this.ActualArea.SeriesClipRect.Width * 0.025);
    double num2 = Math.Floor(this.ActualArea.SeriesClipRect.Height * 0.025);
    double y = 0.0;
    double stackedYValue = double.NaN;
    point.X -= num1;
    point.Y -= num2;
    double x1;
    this.FindNearestChartPoint(point, out x1, out y, out stackedYValue);
    point1.X = this.ActualArea.PointToValue(this.ActualXAxis, point);
    point1.Y = this.ActualArea.PointToValue(this.ActualYAxis, point);
    point.X += 2.0 * num1;
    point.Y += 2.0 * num2;
    double x2;
    this.FindNearestChartPoint(point, out x2, out y, out stackedYValue);
    point2.X = this.ActualArea.PointToValue(this.ActualXAxis, point);
    point2.Y = this.ActualArea.PointToValue(this.ActualYAxis, point);
    rect = new Rect(point1, point2);
    this.dataPoint = (ChartDataPointInfo) null;
    if (this.IsIndexed || !(this.ActualXValues is IList<double>))
    {
      double num3 = double.IsNaN(x1) ? 0.0 : x1;
      double num4 = double.IsNaN(x2) ? (double) (doubleList.Count - 1) : x2;
      startIndex = Convert.ToInt32(num3);
      endIndex = Convert.ToInt32(num4);
    }
    else
    {
      double num5 = double.IsNaN(x1) ? doubleList[0] : x1;
      double num6 = double.IsNaN(x2) ? doubleList[doubleList.Count - 1] : x2;
      startIndex = doubleList.IndexOf(num5);
      endIndex = doubleList.IndexOf(num6);
    }
    if (startIndex == -1)
      startIndex = 0;
    if (endIndex != -1)
      return;
    endIndex = 0;
  }

  internal void UpdateEmptyPoints(int index)
  {
    if (this.EmptyPointIndexes != null && ((IEnumerable<List<int>>) this.EmptyPointIndexes).Count<List<int>>() > 0 && this.ActualArea is SfChart && (this.ActualArea as SfChart).Series != null && (this.ActualArea as SfChart).Series.Count > 0)
    {
      foreach (int num in this.EmptyPointIndexes[0])
      {
        if (num == index)
        {
          this.EmptyPointIndexes[0].Remove(num);
          this.Segments[index].IsEmptySegmentInterior = false;
          break;
        }
      }
    }
    else
    {
      if (this.EmptyPointIndexes == null || ((IEnumerable<List<int>>) this.EmptyPointIndexes).Count<List<int>>() <= 0 || !(this.ActualArea is SfChart3D) || (this.ActualArea as SfChart3D).Series == null || (this.ActualArea as SfChart3D).Series.Count <= 0)
        return;
      foreach (int num in this.EmptyPointIndexes[0])
      {
        if (num == index)
        {
          this.EmptyPointIndexes[0].Remove(num);
          this.Segments[index].IsEmptySegmentInterior = false;
          break;
        }
      }
    }
  }

  internal IEnumerable Clone(IEnumerable XValues)
  {
    IEnumerable enumerable = (IEnumerable) null;
    switch (XValues)
    {
      case List<double> _:
        enumerable = (IEnumerable) new List<double>();
        List<double> doubleList = enumerable as List<double>;
        List<double> collection1 = XValues as List<double>;
        doubleList.AddRange((IEnumerable<double>) collection1);
        this.dataCount = doubleList.Count;
        break;
      case List<string> _:
        enumerable = (IEnumerable) new List<string>();
        List<string> stringList = enumerable as List<string>;
        List<string> collection2 = XValues as List<string>;
        stringList.AddRange((IEnumerable<string>) collection2);
        this.dataCount = stringList.Count;
        break;
    }
    return enumerable;
  }

  internal double GetGrandTotal(IList<double> yValues)
  {
    if (!this.totalCalculated)
    {
      this.grandTotal = yValues.Where<double>((System.Func<double, bool>) (val => !double.IsNaN(val))).Sum();
      this.totalCalculated = true;
    }
    return this.grandTotal;
  }

  internal DataTemplate GetTrackballTemplate() => this.TrackBallLabelTemplate;

  internal object GetActualXValue(int index)
  {
    switch (this.XAxisValueType)
    {
      case ChartValueType.DateTime:
        return (object) ((IList<double>) this.ActualXValues)[index].FromOADate();
      case ChartValueType.String:
        return (object) ((IList<string>) this.ActualXValues)[index];
      case ChartValueType.TimeSpan:
        return (object) TimeSpan.FromMilliseconds(((IList<double>) this.ActualXValues)[index]);
      default:
        return (object) ((IList<double>) this.ActualXValues)[index];
    }
  }

  internal void UpdateAdornmentSelection(int index)
  {
    if (index < 0 || index > this.ActualData.Count - 1)
      return;
    List<int> intList = new List<int>();
    List<int> list;
    if (this is CircularSeriesBase circularSeriesBase && !double.IsNaN(circularSeriesBase.GroupTo))
    {
      list = this.Adornments.Where<ChartAdornment>((System.Func<ChartAdornment, bool>) (adorment => this.Segments[index].Item == adorment.Item)).Select<ChartAdornment, int>((System.Func<ChartAdornment, int>) (adorment => this.Adornments.IndexOf(adorment))).ToList<int>();
    }
    else
    {
      if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed && this.IsSideBySide)
      {
        switch (this)
        {
          case RangeSeriesBase _:
          case FinancialSeriesBase _:
          case WaterfallSeries _:
            break;
          default:
            list = this.Adornments.Where<ChartAdornment>((System.Func<ChartAdornment, bool>) (adorment => this.GroupedActualData[index] == adorment.Item)).Select<ChartAdornment, int>((System.Func<ChartAdornment, int>) (adorment => this.Adornments.IndexOf(adorment))).ToList<int>();
            goto label_7;
        }
      }
      list = this.Adornments.Where<ChartAdornment>((System.Func<ChartAdornment, bool>) (adorment => this.ActualData[index] == adorment.Item)).Select<ChartAdornment, int>((System.Func<ChartAdornment, int>) (adorment => this.Adornments.IndexOf(adorment))).ToList<int>();
    }
label_7:
    if (this.AdornmentPresenter == null)
      return;
    this.AdornmentPresenter.UpdateAdornmentSelection(list, true);
  }

  internal void OnPropertyChanged(string propertyName)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }

  internal DataTemplate GetIconTemplate() => this.LegendIconTemplate;

  protected internal virtual void OnSeriesMouseUp(object source, Point position)
  {
  }

  protected internal virtual void OnSeriesMouseDown(object source, Point position)
  {
  }

  protected internal virtual void SelectedIndexChanged(int newIndex, int oldIndex)
  {
    if (this.ActualArea != null && !this.ActualArea.GetEnableSeriesSelection() && this.ActualArea.SelectionBehaviour != null)
    {
      if (this.ActualArea.SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
      {
        if (this.SelectedSegmentsIndexes.Contains(oldIndex))
          this.SelectedSegmentsIndexes.Remove(oldIndex);
        this.OnResetSegment(oldIndex);
      }
      if (this.IsItemsSourceChanged)
        return;
      if (newIndex >= 0 && this.ActualArea.GetEnableSegmentSelection())
      {
        if (!this.SelectedSegmentsIndexes.Contains(newIndex))
          this.SelectedSegmentsIndexes.Add(newIndex);
        if (this.adornmentInfo is ChartAdornmentInfo && this.adornmentInfo.HighlightOnSelection)
          this.UpdateAdornmentSelection(newIndex);
        ISegmentSelectable segmentSelectable = this as ISegmentSelectable;
        if (newIndex < this.Segments.Count && segmentSelectable != null && segmentSelectable.SegmentSelectionBrush != null)
        {
          this.Segments[newIndex].BindProperties();
          this.Segments[newIndex].IsSelectedSegment = true;
        }
        if (newIndex < this.Segments.Count)
        {
          ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = this.Segments[newIndex],
            SelectedSegments = this.ActualArea.SelectedSegments,
            SelectedSeries = this,
            SelectedIndex = newIndex,
            PreviousSelectedIndex = oldIndex,
            PreviousSelectedSegment = (ChartSegment) null,
            NewPointInfo = this.Segments[newIndex].Item,
            IsSelected = true
          };
          eventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
          if (oldIndex >= 0 && oldIndex < this.Segments.Count)
          {
            eventArgs.PreviousSelectedSegment = this.Segments[oldIndex];
            eventArgs.OldPointInfo = this.Segments[oldIndex].Item;
          }
          (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
          this.PreviousSelectedIndex = newIndex;
        }
        else
        {
          if (this.Segments.Count != 0)
            return;
          this.triggerSelectionChangedEventOnLoad = true;
        }
      }
      else
      {
        if (newIndex != -1)
          return;
        ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
        {
          SelectedSegment = (ChartSegment) null,
          SelectedSegments = this.ActualArea.SelectedSegments,
          SelectedSeries = (ChartSeriesBase) null,
          SelectedIndex = newIndex,
          PreviousSelectedIndex = oldIndex,
          PreviousSelectedSegment = (ChartSegment) null,
          PreviousSelectedSeries = this,
          IsSelected = false
        };
        if (oldIndex != -1 && oldIndex < this.Segments.Count)
        {
          eventArgs.PreviousSelectedSegment = this.Segments[oldIndex];
          eventArgs.OldPointInfo = this.Segments[oldIndex].Item;
        }
        (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
        this.PreviousSelectedIndex = newIndex;
      }
    }
    else
    {
      if (newIndex < 0 || this.Segments.Count != 0)
        return;
      this.triggerSelectionChangedEventOnLoad = true;
    }
  }

  protected internal virtual IChartTransformer CreateTransformer(Size size, bool create)
  {
    if (create || this.ChartTransformer == null)
      this.ChartTransformer = ChartTransform.CreateCartesian(size, this);
    return this.ChartTransformer;
  }

  protected internal abstract void GeneratePoints();

  protected internal List<double> GetXValues()
  {
    double xIndexValues = 0.0;
    List<double> source = this.ActualXValues as List<double>;
    if (this.IsIndexed || source == null)
      source = source != null ? source.Select<double, double>((System.Func<double, double>) (val => xIndexValues++)).ToList<double>() : (this.ActualXValues as List<string>).Select<string, double>((System.Func<string, double>) (val => xIndexValues++)).ToList<double>();
    return source;
  }

  protected virtual void SetTooltipDuration()
  {
    int initialShowDelay = ChartTooltip.GetActualInitialShowDelay(this.ActualArea.TooltipBehavior, ChartTooltip.GetInitialShowDelay((DependencyObject) this));
    int actualShowDuration = ChartTooltip.GetActualShowDuration(this.ActualArea.TooltipBehavior, ChartTooltip.GetShowDuration((DependencyObject) this));
    if (initialShowDelay > 0)
    {
      this.Timer.Stop();
      if (this.InitialDelayTimer.IsEnabled)
        return;
      this.InitialDelayTimer.Interval = new TimeSpan(0, 0, 0, 0, initialShowDelay);
      this.InitialDelayTimer.Start();
    }
    else
    {
      this.Timer.Interval = new TimeSpan(0, 0, 0, 0, actualShowDuration);
      this.Timer.Start();
    }
  }

  protected virtual bool IsTooltipAvailable(Canvas canvas)
  {
    foreach (object child in canvas.Children)
    {
      if (child is ChartTooltip)
        return true;
    }
    return false;
  }

  protected virtual void SetDoubleAnimation(ChartTooltip chartTooltip)
  {
    if (VisualTreeHelper.GetChildrenCount((DependencyObject) chartTooltip) == 0 || !(VisualTreeHelper.GetChild((DependencyObject) chartTooltip, 0) is Grid child))
      return;
    this.storyBoard = new Storyboard();
    DoubleAnimation doubleAnimation1 = new DoubleAnimation();
    doubleAnimation1.From = new double?(0.5);
    doubleAnimation1.To = new double?(1.0);
    doubleAnimation1.Duration = new Duration().GetDuration(new TimeSpan(0, 0, 0, 0, 200));
    DoubleAnimation doubleAnimation2 = doubleAnimation1;
    SineEase sineEase1 = new SineEase();
    sineEase1.EasingMode = EasingMode.EaseOut;
    SineEase sineEase2 = sineEase1;
    doubleAnimation2.EasingFunction = (IEasingFunction) sineEase2;
    this.scaleXAnimation = doubleAnimation1;
    Storyboard.SetTargetName((DependencyObject) this.scaleXAnimation, "scaleTransform");
    Storyboard.SetTargetProperty((DependencyObject) this.scaleXAnimation, new PropertyPath((object) ScaleTransform.ScaleXProperty));
    DoubleAnimation doubleAnimation3 = new DoubleAnimation();
    doubleAnimation3.From = new double?(0.5);
    doubleAnimation3.To = new double?(1.0);
    doubleAnimation3.Duration = new Duration().GetDuration(new TimeSpan(0, 0, 0, 0, 200));
    DoubleAnimation doubleAnimation4 = doubleAnimation3;
    SineEase sineEase3 = new SineEase();
    sineEase3.EasingMode = EasingMode.EaseOut;
    SineEase sineEase4 = sineEase3;
    doubleAnimation4.EasingFunction = (IEasingFunction) sineEase4;
    this.scaleYAnimation = doubleAnimation3;
    Storyboard.SetTargetName((DependencyObject) this.scaleYAnimation, "scaleTransform");
    Storyboard.SetTargetProperty((DependencyObject) this.scaleYAnimation, new PropertyPath((object) ScaleTransform.ScaleYProperty));
    this.storyBoard.Children.Add((Timeline) this.scaleXAnimation);
    this.storyBoard.Children.Add((Timeline) this.scaleYAnimation);
    this.storyBoard.Begin((FrameworkElement) child);
  }

  protected virtual void SetIndividualPoint(int index, object obj, bool replace)
  {
    if (this.SeriesYValues != null && this.YPaths != null && this.ItemsSource != null)
    {
      object arrayPropertyValue1 = this.GetArrayPropertyValue(obj, this.XComplexPaths);
      if (arrayPropertyValue1 != null)
        this.XAxisValueType = ChartSeriesBase.GetDataType(arrayPropertyValue1);
      if (this.IsMultipleYPathRequired)
      {
        if (this.SegmentColorPath != null && this.IsColorPathSeries)
          this.SetIndividualColorValue(index, obj, replace);
        if (this.XAxisValueType == ChartValueType.String)
        {
          if (!(this.XValues is List<string>))
            this.XValues = this.ActualXValues = (IEnumerable) new List<string>();
          IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
          string arrayPropertyValue2 = this.GetArrayPropertyValue(obj, this.XComplexPaths) as string;
          if (replace && xvalues.Count > index)
          {
            if (xvalues[index] == arrayPropertyValue2)
              this.isRepeatPoint = true;
            else
              xvalues[index] = arrayPropertyValue2;
          }
          else
            xvalues.Insert(index, arrayPropertyValue2);
          for (int index1 = 0; index1 < ((IEnumerable<string>) this.YPaths).Count<string>(); ++index1)
          {
            this.YData = Convert.ToDouble(this.GetArrayPropertyValue(obj, this.YComplexPaths[index1]) ?? (object) double.NaN);
            if (replace && this.SeriesYValues[index1].Count > index)
            {
              if (this.SeriesYValues[index1][index] == this.YData && this.isRepeatPoint)
              {
                this.isRepeatPoint = true;
              }
              else
              {
                this.SeriesYValues[index1][index] = this.YData;
                this.isRepeatPoint = false;
              }
            }
            else
              this.SeriesYValues[index1].Insert(index, this.YData);
          }
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
        {
          if (!(this.XValues is List<double>))
            this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          this.XData = Convert.ToDouble(this.GetArrayPropertyValue(obj, this.XComplexPaths) ?? (object) double.NaN);
          if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1] || index == 0 && xvalues.Count > 0 && this.XData > xvalues[0])
            this.isLinearData = false;
          if (replace && xvalues.Count > index)
          {
            if (xvalues[index] == this.XData)
              this.isRepeatPoint = true;
            else
              xvalues[index] = this.XData;
          }
          else
            xvalues.Insert(index, this.XData);
          for (int index2 = 0; index2 < ((IEnumerable<string>) this.YPaths).Count<string>(); ++index2)
          {
            this.YData = Convert.ToDouble(this.GetArrayPropertyValue(obj, this.YComplexPaths[index2]) ?? (object) double.NaN);
            if (replace && this.SeriesYValues[index2].Count > index)
            {
              if (this.SeriesYValues[index2][index] == this.YData && this.isRepeatPoint)
              {
                this.isRepeatPoint = true;
              }
              else
              {
                this.SeriesYValues[index2][index] = this.YData;
                this.isRepeatPoint = false;
              }
            }
            else
              this.SeriesYValues[index2].Insert(index, this.YData);
          }
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.DateTime)
        {
          if (!(this.XValues is List<double>))
            this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          this.XData = Convert.ToDateTime(this.GetArrayPropertyValue(obj, this.XComplexPaths)).ToOADate();
          if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1])
            this.isLinearData = false;
          if (replace && xvalues.Count > index)
          {
            if (xvalues[index] == this.XData)
              this.isRepeatPoint = true;
            else
              xvalues[index] = this.XData;
          }
          else
            xvalues.Insert(index, this.XData);
          for (int index3 = 0; index3 < ((IEnumerable<string>) this.YPaths).Count<string>(); ++index3)
          {
            this.YData = Convert.ToDouble(this.GetArrayPropertyValue(obj, this.YComplexPaths[index3]) ?? (object) double.NaN);
            if (replace && this.SeriesYValues[index3].Count > index)
            {
              if (this.SeriesYValues[index3][index] == this.YData && this.isRepeatPoint)
              {
                this.isRepeatPoint = true;
              }
              else
              {
                this.SeriesYValues[index3][index] = this.YData;
                this.isRepeatPoint = false;
              }
            }
            else
              this.SeriesYValues[index3].Insert(index, this.YData);
          }
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.TimeSpan)
        {
          if (!(this.XValues is List<double>))
            this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          this.XData = ((TimeSpan) this.GetArrayPropertyValue(obj, this.XComplexPaths)).TotalMilliseconds;
          if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1])
            this.isLinearData = false;
          if (replace && xvalues.Count > index)
          {
            if (xvalues[index] == this.XData)
              this.isRepeatPoint = true;
            else
              xvalues[index] = this.XData;
          }
          else
            xvalues.Insert(index, this.XData);
          for (int index4 = 0; index4 < ((IEnumerable<string>) this.YPaths).Count<string>(); ++index4)
          {
            this.YData = Convert.ToDouble(this.GetArrayPropertyValue(obj, this.YComplexPaths[index4]) ?? (object) double.NaN);
            if (replace && this.SeriesYValues[index4].Count > index)
            {
              if (this.SeriesYValues[index4][index] == this.YData && this.isRepeatPoint)
              {
                this.isRepeatPoint = true;
              }
              else
              {
                this.SeriesYValues[index4][index] = this.YData;
                this.isRepeatPoint = false;
              }
            }
            else
              this.SeriesYValues[index4].Insert(index, this.YData);
          }
          this.dataCount = xvalues.Count;
        }
      }
      else
      {
        string[] ycomplexPath = this.YComplexPaths[0];
        IList<double> seriesYvalue = this.SeriesYValues[0];
        if (this.SegmentColorPath != null && this.IsColorPathSeries)
          this.SetIndividualColorValue(index, obj, replace);
        if (this.XAxisValueType == ChartValueType.String)
        {
          if (!(this.XValues is List<string>))
            this.XValues = this.ActualXValues = (IEnumerable) new List<string>();
          IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
          object arrayPropertyValue3 = this.GetArrayPropertyValue(obj, this.XComplexPaths);
          object arrayPropertyValue4 = this.GetArrayPropertyValue(obj, ycomplexPath);
          this.YData = arrayPropertyValue4 != null ? Convert.ToDouble(arrayPropertyValue4) : double.NaN;
          if (replace && xvalues.Count > index)
          {
            if (xvalues[index] == Convert.ToString(arrayPropertyValue3))
              this.isRepeatPoint = true;
            else
              xvalues[index] = Convert.ToString(arrayPropertyValue3);
          }
          else
            xvalues.Insert(index, Convert.ToString(arrayPropertyValue3));
          if (replace && seriesYvalue.Count > index)
          {
            if (seriesYvalue[index] == this.YData && this.isRepeatPoint)
            {
              this.isRepeatPoint = true;
            }
            else
            {
              seriesYvalue[index] = this.YData;
              this.isRepeatPoint = false;
            }
          }
          else
            seriesYvalue.Insert(index, this.YData);
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
        {
          if (!(this.XValues is List<double>))
            this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          object arrayPropertyValue5 = this.GetArrayPropertyValue(obj, this.XComplexPaths);
          object arrayPropertyValue6 = this.GetArrayPropertyValue(obj, ycomplexPath);
          this.XData = arrayPropertyValue5 != null ? Convert.ToDouble(arrayPropertyValue5) : double.NaN;
          this.YData = arrayPropertyValue6 != null ? Convert.ToDouble(arrayPropertyValue6) : double.NaN;
          if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1] || index == 0 && xvalues.Count > 0 && this.XData > xvalues[0])
            this.isLinearData = false;
          if (replace && xvalues.Count > index)
          {
            if (xvalues[index] == this.XData)
              this.isRepeatPoint = true;
            else
              xvalues[index] = this.XData;
          }
          else
            xvalues.Insert(index, this.XData);
          if (replace && seriesYvalue.Count > index)
          {
            if (seriesYvalue[index] == this.YData && this.isRepeatPoint)
            {
              this.isRepeatPoint = true;
            }
            else
            {
              seriesYvalue[index] = this.YData;
              this.isRepeatPoint = false;
            }
          }
          else
            seriesYvalue.Insert(index, this.YData);
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.DateTime)
        {
          if (!(this.XValues is List<double>))
            this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          object arrayPropertyValue7 = this.GetArrayPropertyValue(obj, this.XComplexPaths);
          object arrayPropertyValue8 = this.GetArrayPropertyValue(obj, ycomplexPath);
          this.XData = Convert.ToDateTime(arrayPropertyValue7).ToOADate();
          this.YData = arrayPropertyValue8 != null ? Convert.ToDouble(arrayPropertyValue8) : double.NaN;
          if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1])
            this.isLinearData = false;
          if (replace && xvalues.Count > index)
          {
            if (xvalues[index] == this.XData)
              this.isRepeatPoint = true;
            else
              xvalues[index] = this.XData;
          }
          else
            xvalues.Insert(index, this.XData);
          if (replace && seriesYvalue.Count > index)
          {
            if (seriesYvalue[index] == this.YData && this.isRepeatPoint)
            {
              this.isRepeatPoint = true;
            }
            else
            {
              seriesYvalue[index] = this.YData;
              this.isRepeatPoint = false;
            }
          }
          else
            seriesYvalue.Insert(index, this.YData);
          this.dataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.TimeSpan)
        {
          if (!(this.XValues is List<double>))
            this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          object arrayPropertyValue9 = this.GetArrayPropertyValue(obj, this.XComplexPaths);
          object arrayPropertyValue10 = this.GetArrayPropertyValue(obj, ycomplexPath);
          this.XData = ((TimeSpan) arrayPropertyValue9).TotalMilliseconds;
          this.YData = arrayPropertyValue10 != null ? Convert.ToDouble(arrayPropertyValue10) : double.NaN;
          if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1])
            this.isLinearData = false;
          if (arrayPropertyValue9 != null && replace && xvalues.Count > index)
          {
            if (xvalues[index] == this.XData)
              this.isRepeatPoint = true;
            else
              xvalues[index] = this.XData;
          }
          else if (arrayPropertyValue9 != null)
            xvalues.Insert(index, this.XData);
          if (replace && seriesYvalue.Count > index)
          {
            if (seriesYvalue[index] == this.YData && this.isRepeatPoint)
            {
              this.isRepeatPoint = true;
            }
            else
            {
              seriesYvalue[index] = this.YData;
              this.isRepeatPoint = false;
            }
          }
          else
            seriesYvalue.Insert(index, this.YData);
          this.dataCount = xvalues.Count;
        }
      }
      if (replace && this.ActualData.Count > index)
        this.ActualData[index] = obj;
      else if (this.ActualData.Count == index)
        this.ActualData.Add(obj);
      else
        this.ActualData.Insert(index, obj);
      this.totalCalculated = false;
    }
    this.UpdateEmptyPoints(index);
    this.HookPropertyChangedEvent(this.ListenPropertyChange, obj);
  }

  protected virtual void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.IsPointGenerated = false;
    this.canAnimate = true;
    if (this.ActualData != null)
      this.ActualData.Clear();
    if (this.ActualXValues != null)
    {
      if (this.ActualXValues is IList<double>)
      {
        (this.XValues as IList<double>).Clear();
        (this.ActualXValues as IList<double>).Clear();
      }
      else if (this.ActualXValues is IList<string>)
      {
        (this.XValues as IList<string>).Clear();
        (this.ActualXValues as IList<string>).Clear();
      }
    }
    if (this.IsSortData)
      this.SeriesYValues = (IList<double>[]) null;
    this.totalCalculated = false;
    this.Segments.Clear();
    this.dataCount = 0;
    this.UpdateArea();
  }

  protected virtual void SetIndividualDataTablePoint(int index, object obj, bool replace)
  {
    if (this.SeriesYValues == null || this.YPaths == null || this.ItemsSource == null)
      return;
    DataRow dataRow = obj as DataRow;
    if (this.IsMultipleYPathRequired)
    {
      if (this.XAxisValueType == ChartValueType.String)
      {
        if (!(this.XValues is List<string>))
          this.XValues = this.ActualXValues = (IEnumerable) new List<string>();
        IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
        string field1 = dataRow.GetField(this.XBindingPath) as string;
        if (replace && xvalues.Count > index)
          xvalues[index] = field1;
        else if (xvalues.Count == index)
          xvalues.Add(field1);
        else
          xvalues.Insert(index, field1);
        for (int index1 = 0; index1 < ((IEnumerable<string>) this.YPaths).Count<string>(); ++index1)
        {
          object field2 = dataRow.GetField(this.YComplexPaths[index1][0]);
          if (!(this.SeriesYValues[index1] is List<double>))
            this.SeriesYValues[index1] = (IList<double>) new List<double>();
          if (replace && this.SeriesYValues[index1].Count > index)
            this.SeriesYValues[index1][index] = Convert.ToDouble(field2 ?? (object) double.NaN);
          else if (this.SeriesYValues[index1].Count == index)
            this.SeriesYValues[index1].Add(Convert.ToDouble(field2 ?? (object) double.NaN));
          else
            this.SeriesYValues[index1].Insert(index, Convert.ToDouble(field2 ?? (object) double.NaN));
        }
        this.dataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
      {
        if (!(this.XValues is List<double>))
          this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        this.XData = Convert.ToDouble(dataRow.GetField(this.XBindingPath) ?? (object) double.NaN);
        if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1])
          this.isLinearData = false;
        if (replace && xvalues.Count > index)
          xvalues[index] = this.XData;
        else if (xvalues.Count == index)
          xvalues.Add(this.XData);
        else
          xvalues.Insert(index, this.XData);
        for (int index2 = 0; index2 < ((IEnumerable<string>) this.YPaths).Count<string>(); ++index2)
        {
          object field = dataRow.GetField(this.YComplexPaths[index2][0]);
          if (!(this.SeriesYValues[index2] is List<double>))
            this.SeriesYValues[index2] = (IList<double>) new List<double>();
          if (replace && this.SeriesYValues[index2].Count > index)
            this.SeriesYValues[index2][index] = Convert.ToDouble(field ?? (object) double.NaN);
          else if (this.SeriesYValues[index2].Count == index)
            this.SeriesYValues[index2].Add(Convert.ToDouble(field ?? (object) double.NaN));
          else
            this.SeriesYValues[index2].Insert(index, Convert.ToDouble(field ?? (object) double.NaN));
        }
        this.dataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.DateTime)
      {
        if (!(this.XValues is List<double>))
          this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        this.XData = Convert.ToDateTime(dataRow.GetField(this.XBindingPath)).ToOADate();
        if (!(this.SeriesYValues[0] is List<double>))
          this.SeriesYValues[0] = (IList<double>) new List<double>();
        if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1])
          this.isLinearData = false;
        if (replace && xvalues.Count > index)
          xvalues[index] = this.XData;
        else if (xvalues.Count == index)
          xvalues.Add(this.XData);
        else
          xvalues.Insert(index, this.XData);
        for (int index3 = 0; index3 < ((IEnumerable<string>) this.YPaths).Count<string>(); ++index3)
        {
          object field = dataRow.GetField(this.YComplexPaths[index3][0]);
          if (index == 0)
            this.SeriesYValues[index3] = (IList<double>) new List<double>();
          if (replace && this.SeriesYValues[index3].Count > index)
            this.SeriesYValues[index3][index] = Convert.ToDouble(field ?? (object) double.NaN);
          else if (this.SeriesYValues[index3].Count == index)
            this.SeriesYValues[index3].Add(Convert.ToDouble(field ?? (object) double.NaN));
          else
            this.SeriesYValues[index3].Insert(index, Convert.ToDouble(field ?? (object) double.NaN));
        }
        this.dataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.TimeSpan)
      {
        if (!(this.XValues is List<double>))
          this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        this.XData = ((TimeSpan) dataRow.GetField(this.XBindingPath)).TotalMilliseconds;
        if (!(this.SeriesYValues[0] is List<double>))
          this.SeriesYValues[0] = (IList<double>) new List<double>();
        if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1])
          this.isLinearData = false;
        if (replace && xvalues.Count > index)
          xvalues[index] = this.XData;
        else if (xvalues.Count == index)
          xvalues.Add(this.XData);
        else
          xvalues.Insert(index, this.XData);
        for (int index4 = 0; index4 < ((IEnumerable<string>) this.YPaths).Count<string>(); ++index4)
        {
          object field = dataRow.GetField(this.YComplexPaths[index4][0]);
          if (!(this.SeriesYValues[index4] is List<double>))
            this.SeriesYValues[index4] = (IList<double>) new List<double>();
          if (replace && this.SeriesYValues[index4].Count > index)
            this.SeriesYValues[index4][index] = Convert.ToDouble(field ?? (object) double.NaN);
          else if (this.SeriesYValues[index4].Count == index)
            this.SeriesYValues[index4].Add(Convert.ToDouble(field ?? (object) double.NaN));
          else
            this.SeriesYValues[index4].Insert(index, Convert.ToDouble(field ?? (object) double.NaN));
        }
        this.dataCount = xvalues.Count;
      }
    }
    else
    {
      string[] ycomplexPath = this.YComplexPaths[0];
      IList<double> seriesYvalue = this.SeriesYValues[0];
      if (this.XAxisValueType == ChartValueType.String)
      {
        if (!(this.XValues is List<string>))
          this.XValues = this.ActualXValues = (IEnumerable) new List<string>();
        IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
        object field3 = dataRow.GetField(this.XBindingPath);
        object field4 = dataRow.GetField(ycomplexPath[0]);
        if (replace && xvalues.Count > index)
          xvalues[index] = Convert.ToString(field3);
        else if (xvalues.Count == index)
          xvalues.Add(Convert.ToString(field3));
        else
          xvalues.Insert(index, Convert.ToString(field3));
        if (replace && seriesYvalue.Count > index)
          seriesYvalue[index] = Convert.ToDouble(field4 ?? (object) double.NaN);
        else if (seriesYvalue.Count == index)
          seriesYvalue.Add(Convert.ToDouble(field4 ?? (object) double.NaN));
        else
          seriesYvalue.Insert(index, Convert.ToDouble(field4 ?? (object) double.NaN));
        this.dataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
      {
        if (!(this.XValues is List<double>))
          this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        object field5 = dataRow.GetField(this.XBindingPath);
        object field6 = dataRow.GetField(ycomplexPath[0]);
        this.XData = Convert.ToDouble(field5 ?? (object) double.NaN);
        if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1])
          this.isLinearData = false;
        if (replace && xvalues.Count > index)
          xvalues[index] = this.XData;
        else if (xvalues.Count == index)
          xvalues.Add(this.XData);
        else
          xvalues.Insert(index, this.XData);
        if (replace && seriesYvalue.Count > index)
          seriesYvalue[index] = Convert.ToDouble(field6 ?? (object) double.NaN);
        else if (seriesYvalue.Count == index)
          seriesYvalue.Add(Convert.ToDouble(field6 ?? (object) double.NaN));
        else
          seriesYvalue.Insert(index, Convert.ToDouble(field6 ?? (object) double.NaN));
        this.dataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.DateTime)
      {
        if (!(this.XValues is List<double>))
          this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        object field7 = dataRow.GetField(this.XBindingPath);
        object field8 = dataRow.GetField(ycomplexPath[0]);
        this.XData = Convert.ToDateTime(field7).ToOADate();
        if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1])
          this.isLinearData = false;
        if (replace && xvalues.Count > index)
          xvalues[index] = this.XData;
        else if (xvalues.Count == index)
          xvalues.Add(this.XData);
        else
          xvalues.Insert(index, this.XData);
        if (replace && seriesYvalue.Count > index)
          seriesYvalue[index] = Convert.ToDouble(field8 ?? (object) double.NaN);
        else if (seriesYvalue.Count == index)
          seriesYvalue.Add(Convert.ToDouble(field8 ?? (object) double.NaN));
        else
          seriesYvalue.Insert(index, Convert.ToDouble(field8 ?? (object) double.NaN));
        this.dataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.TimeSpan)
      {
        if (!(this.XValues is List<double>))
          this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        object field9 = dataRow.GetField(this.XBindingPath);
        object field10 = dataRow.GetField(ycomplexPath[0]);
        this.XData = ((TimeSpan) field9).TotalMilliseconds;
        if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1])
          this.isLinearData = false;
        if (field9 != null && replace && xvalues.Count > index)
          xvalues[index] = this.XData;
        else if (xvalues.Count == index)
          xvalues.Add(this.XData);
        else if (field9 != null)
          xvalues.Insert(index, this.XData);
        if (field10 != null && replace && seriesYvalue.Count > index)
          seriesYvalue[index] = Convert.ToDouble(field10 ?? (object) double.NaN);
        else if (seriesYvalue.Count == index)
          seriesYvalue.Add(Convert.ToDouble(field10 ?? (object) double.NaN));
        else
          seriesYvalue.Insert(index, Convert.ToDouble(field10 ?? (object) double.NaN));
        this.dataCount = xvalues.Count;
      }
    }
    if (replace && this.ActualData.Count > index)
      this.ActualData[index] = obj;
    else if (this.ActualData.Count == index)
      this.ActualData.Add(obj);
    else
      this.ActualData.Insert(index, obj);
  }

  protected virtual void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
  }

  protected virtual void ClearUnUsedSegments(int startIndex)
  {
    List<ChartSegment> chartSegmentList = new List<ChartSegment>();
    foreach (ChartSegment chartSegment in this.Segments.Where<ChartSegment>((System.Func<ChartSegment, bool>) (item => item is EmptyPointSegment)))
      chartSegmentList.Add(chartSegment);
    foreach (ChartSegment chartSegment in chartSegmentList)
      this.Segments.Remove(chartSegment);
    if (this.Segments.Count <= startIndex)
      return;
    int count = this.Segments.Count;
    for (int index = startIndex; index < count; ++index)
      this.Segments.RemoveAt(startIndex);
  }

  protected virtual DependencyObject CloneSeries(DependencyObject obj)
  {
    ChartSeriesBase copy = obj as ChartSeriesBase;
    ChartCloning.CloneControl((Control) this, (Control) copy);
    if (this is CartesianSeries cartesianSeries)
      (copy as CartesianSeries).IsTransposed = cartesianSeries.IsTransposed;
    copy.Palette = this.Palette;
    copy.TrackBallLabelTemplate = this.TrackBallLabelTemplate;
    copy.ActualTrackballLabelTemplate = this.ActualTrackballLabelTemplate;
    copy.VisibilityOnLegend = this.VisibilityOnLegend;
    copy.ColorModel = this.ColorModel;
    copy.ItemsSource = this.ItemsSource;
    copy.XBindingPath = this.XBindingPath;
    copy.ShowEmptyPoints = this.ShowEmptyPoints;
    copy.LegendIconTemplate = this.LegendIconTemplate;
    copy.LegendIcon = this.LegendIcon;
    copy.Label = this.Label;
    copy.IsSortData = this.IsSortData;
    copy.SortBy = this.SortBy;
    copy.SortDirection = this.SortDirection;
    copy.Interior = this.Interior;
    copy.EmptyPointValue = this.EmptyPointValue;
    copy.EmptyPointSymbolTemplate = this.EmptyPointSymbolTemplate;
    copy.EmptyPointStyle = this.EmptyPointStyle;
    copy.EmptyPointInterior = this.EmptyPointInterior;
    copy.EnableAnimation = this.EnableAnimation;
    copy.ShowTooltip = this.ShowTooltip;
    copy.TooltipTemplate = this.TooltipTemplate;
    copy.SeriesSelectionBrush = this.SeriesSelectionBrush;
    copy.VisibilityOnLegend = this.VisibilityOnLegend;
    return (DependencyObject) copy;
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    this.OnSeriesMouseUp(e.OriginalSource, e.GetPosition((IInputElement) this));
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    this.OnSeriesMouseDown(e.OriginalSource, e.GetPosition((IInputElement) this));
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    Canvas adorningCanvas = this.ActualArea.GetAdorningCanvas();
    this.mousePos = e.GetPosition((IInputElement) adorningCanvas);
    this.RemovePreviousSeriesTooltip();
    if (this.GetAnimationIsActive() || this is ErrorBarSeries)
      return;
    this.UpdateTooltip(e.OriginalSource);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    this.MousePointerExit(this.ActualArea == null || this.ActualArea.TooltipBehavior == null ? (ChartTooltipBehavior) null : this.ActualArea.TooltipBehavior);
  }

  private void MousePointerExit(ChartTooltipBehavior tooltipBehavior)
  {
    if (this.ActualTooltipPosition == TooltipPosition.Pointer || !this.Timer.IsEnabled)
    {
      this.RemoveTooltip();
      this.Timer.Stop();
    }
    if (ChartTooltip.GetActualInitialShowDelay(tooltipBehavior, ChartTooltip.GetInitialShowDelay((DependencyObject) this)) <= 0)
      return;
    this.InitialDelayTimer.Stop();
  }

  internal void RemovePreviousSeriesTooltip()
  {
    if (this.ActualArea.Tooltip == null || this.ActualArea.Tooltip.PreviousSeries == null || this.ActualTooltipPosition != TooltipPosition.Auto || this.Equals((object) this.ActualArea.Tooltip.PreviousSeries))
      return;
    this.RemoveTooltip();
    this.Timer.Stop();
  }

  public override void OnApplyTemplate() => base.OnApplyTemplate();

  protected Point Position(Point mousePos, ref ChartTooltip tooltip)
  {
    ChartTooltipBehavior tooltipBehavior = this.ActualArea.TooltipBehavior;
    double horizontalOffset = ChartTooltip.GetActualHorizontalOffset(tooltipBehavior, ChartTooltip.GetHorizontalOffset((DependencyObject) this));
    double actualVerticalOffset = ChartTooltip.GetActualVerticalOffset(tooltipBehavior, ChartTooltip.GetVerticalOffset((DependencyObject) this));
    Point point = mousePos;
    if (tooltip.DesiredSize.Height == 0.0 || tooltip.DesiredSize.Width == 0.0)
      tooltip.UpdateLayout();
    HorizontalAlignment horizontalAlignment = ChartTooltip.GetActualHorizontalAlignment(tooltipBehavior, ChartTooltip.GetHorizontalAlignment((UIElement) this));
    VerticalAlignment verticalAlignment = ChartTooltip.GetActualVerticalAlignment(tooltipBehavior, ChartTooltip.GetVerticalAlignment((UIElement) this));
    switch (horizontalAlignment)
    {
      case HorizontalAlignment.Left:
        point.X = mousePos.X - tooltip.DesiredSize.Width - horizontalOffset;
        break;
      case HorizontalAlignment.Center:
        point.X = mousePos.X - tooltip.DesiredSize.Width / 2.0 + horizontalOffset;
        break;
      case HorizontalAlignment.Right:
        point.X = mousePos.X + horizontalOffset;
        break;
    }
    switch (verticalAlignment)
    {
      case VerticalAlignment.Top:
        point.Y = mousePos.Y - tooltip.DesiredSize.Height - actualVerticalOffset;
        break;
      case VerticalAlignment.Center:
        point.Y = mousePos.Y - tooltip.DesiredSize.Height / 2.0 - actualVerticalOffset;
        break;
      case VerticalAlignment.Bottom:
        point.Y = mousePos.Y + actualVerticalOffset;
        break;
    }
    return point;
  }

  protected void UpdateArea()
  {
    if (this.ActualArea == null)
      return;
    this.ActualArea.ScheduleUpdate();
  }

  protected ChartSeriesBase GetPreviousSeries(ChartSeriesBase series)
  {
    int index = this.ActualArea.VisibleSeries.IndexOf(series) - 1;
    return index == -1 ? (ChartSeriesBase) null : this.ActualArea.VisibleSeries[index];
  }

  protected void GeneratePoints(string[] yPaths, params IList<double>[] yValueLists)
  {
    this.IsComplexYProperty = false;
    bool flag = false;
    this.YComplexPaths = new string[((IEnumerable<string>) yPaths).Count<string>()][];
    for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
    {
      if (string.IsNullOrEmpty(yPaths[index]))
      {
        if (!(this is RangeColumnSeries) || this.IsMultipleYPathRequired)
          return;
        break;
      }
      this.YComplexPaths[index] = yPaths[index].Split('.');
      if (yPaths[index].Contains<char>('.'))
        this.IsComplexYProperty = true;
      if (yPaths[index].Contains<char>('['))
        flag = true;
    }
    IList<double>[] yLists;
    if (this.IsSortData)
    {
      if (this.SeriesYValues == null)
        this.CreateYValueCollection(yPaths.Length);
      yLists = this.SeriesYValues;
      this.ActualSeriesYValues = yValueLists;
    }
    else
    {
      IList<double>[] doubleListArray1;
      yLists = doubleListArray1 = yValueLists;
      IList<double>[] doubleListArray2 = doubleListArray1;
      this.ActualSeriesYValues = doubleListArray1;
      this.SeriesYValues = doubleListArray2;
    }
    this.YPaths = yPaths;
    this.SeriesYCount = yPaths.Length;
    if (this.ItemsSource != null && !string.IsNullOrEmpty(this.XBindingPath))
    {
      if (this.ItemsSource is DataTable)
        this.GenerateDataTablePoints(yPaths, yLists);
      else if (this.ItemsSource is IEnumerable)
      {
        if (this.XBindingPath.Contains<char>('[') || flag)
          this.GenerateComplexPropertyPoints(yPaths, yLists, new ChartSeriesBase.GetReflectedProperty(this.GetArrayPropertyValue));
        else if (this.XBindingPath.Contains<char>('.') || this.IsComplexYProperty)
          this.GenerateComplexPropertyPoints(yPaths, yLists, new ChartSeriesBase.GetReflectedProperty(this.GetPropertyValue));
        else
          this.GeneratePropertyPoints(yPaths, yLists);
      }
    }
    this.ColorValues = (IList<Brush>) new List<Brush>();
    if (this.ItemsSource != null && this.SegmentColorPath != null && this.IsColorPathSeries)
    {
      this.GenerateSegmentColor();
      if (this.ActualArea != null)
        this.ActualArea.IsUpdateLegend = true;
    }
    if (this.ShowEmptyPoints)
    {
      this.ValidateDataPoints(this.SeriesYValues);
      this.isPointValidated = true;
    }
    if (!this.IsSortData)
      return;
    this.SortActualPoints();
  }

  protected void OnDataSourceChanged(DependencyPropertyChangedEventArgs args)
  {
    this.canAnimate = true;
    if (this is ISegmentSelectable segmentSelectable && args.OldValue != null)
    {
      this.IsItemsSourceChanged = true;
      segmentSelectable.SelectedIndex = -1;
      this.IsItemsSourceChanged = false;
    }
    if (this.EmptyPointIndexes != null)
    {
      foreach (List<int> emptyPointIndex in this.EmptyPointIndexes)
        emptyPointIndex.Clear();
    }
    if (this.ActualData != null)
      this.ActualData.Clear();
    if (this.ToggledLegendIndex != null)
      this.ToggledLegendIndex.Clear();
    if (this.ActualXValues != null)
    {
      if (this.ActualXValues is IList<double>)
      {
        (this.XValues as IList<double>).Clear();
        (this.ActualXValues as IList<double>).Clear();
      }
      else if (this.ActualXValues is IList<string>)
      {
        (this.XValues as IList<string>).Clear();
        (this.ActualXValues as IList<string>).Clear();
      }
    }
    if (this.IsSortData)
      this.SeriesYValues = (IList<double>[]) null;
    if (args.OldValue is INotifyCollectionChanged)
      (args.OldValue as INotifyCollectionChanged).CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnDataCollectionChanged);
    this.UnHookPropertyChanged(args.OldValue);
    if (args.NewValue is INotifyCollectionChanged)
      (args.NewValue as INotifyCollectionChanged).CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnDataCollectionChanged);
    else if (args.NewValue == null && this.ActualArea is SfChart actualArea && this.IsBitmapSeries && actualArea.fastRenderSurface != null)
      actualArea.UpdateBitmapSeries(this as ChartSeries, false);
    if (args.OldValue is IRaiseItemChangedEvents)
      (args.NewValue as IBindingList).ListChanged -= (ListChangedEventHandler) ((sender, listArgs) => this.OnBindingListChanged(listArgs.ListChangedType, listArgs.NewIndex));
    if (args.NewValue is IRaiseItemChangedEvents)
      (args.NewValue as IBindingList).ListChanged += (ListChangedEventHandler) ((sender, listArgs) => this.OnBindingListChanged(listArgs.ListChangedType, listArgs.NewIndex));
    this.totalCalculated = false;
    if (this.Segments != null)
      this.Segments.Clear();
    this.ClearAdornments();
    this.dataCount = 0;
    DataTable newValue = args.NewValue as DataTable;
    if (args.OldValue is DataTable oldValue)
    {
      oldValue.RowChanged -= new DataRowChangeEventHandler(this.DataTableRowChanged);
      oldValue.RowDeleting -= new DataRowChangeEventHandler(this.DataTableRowChanged);
      oldValue.TableCleared -= new DataTableClearEventHandler(this.DataTableCleared);
      oldValue.TableClearing -= new DataTableClearEventHandler(this.DataTableCleared);
    }
    if (newValue != null)
    {
      newValue.RowChanged += new DataRowChangeEventHandler(this.DataTableRowChanged);
      newValue.RowDeleting += new DataRowChangeEventHandler(this.DataTableRowChanged);
      newValue.TableCleared += new DataTableClearEventHandler(this.DataTableCleared);
      newValue.TableClearing += new DataTableClearEventHandler(this.DataTableCleared);
      this.OnDataSourceChanged(oldValue == null ? args.OldValue as IEnumerable : (IEnumerable) oldValue.Rows, (IEnumerable) newValue.Rows);
    }
    else
      this.OnDataSourceChanged(args.OldValue as IEnumerable, args.NewValue as IEnumerable);
  }

  private void DataTableCleared(object sender, DataTableClearEventArgs e) => this.Refresh();

  private void ClearAdornments()
  {
    if (!(this is AdornmentSeries adornmentSeries))
      return;
    adornmentSeries.Adornments.Clear();
    adornmentSeries.VisibleAdornments.Clear();
    if (this.adornmentInfo == null)
      return;
    if (this.adornmentInfo.adormentContainers != null)
      this.adornmentInfo.adormentContainers.Clear();
    if (this.adornmentInfo.ConnectorLines != null)
      this.adornmentInfo.ConnectorLines.Clear();
    if (this.adornmentInfo.LabelPresenters == null)
      return;
    this.adornmentInfo.LabelPresenters.Clear();
  }

  private static void FadeInAnimation(ref ChartTooltip chartTooltip)
  {
    Storyboard storyboard = new Storyboard();
    DoubleAnimation doubleAnimation = new DoubleAnimation();
    doubleAnimation.From = new double?(0.0);
    doubleAnimation.To = new double?(1.0);
    doubleAnimation.Duration = new Duration().GetDuration(new TimeSpan(0, 0, 0, 1));
    DoubleAnimation element = doubleAnimation;
    Storyboard.SetTarget((DependencyObject) element, (DependencyObject) chartTooltip);
    Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath("Opacity", new object[0]));
    storyboard.Children.Add((Timeline) element);
    storyboard.Begin();
  }

  private static void OnTooltipTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    ChartSeriesBase chartSeriesBase = (ChartSeriesBase) d;
    if (chartSeriesBase == null || chartSeriesBase.ActualArea == null || chartSeriesBase.ActualArea.Tooltip == null)
      return;
    chartSeriesBase.ActualArea.Tooltip.ContentTemplate = args.NewValue as DataTemplate;
  }

  private static void OnListenPropertyChangeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    (d as ChartSeriesBase).HookPropertyChangedEvent((bool) args.NewValue);
  }

  private static void OnShowTooltipChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    ChartSeriesBase chartSeriesBase = (ChartSeriesBase) d;
    if (chartSeriesBase != null && chartSeriesBase.ActualArea != null && (bool) args.NewValue)
      chartSeriesBase.ActualArea.Tooltip = new ChartTooltip();
    else if (chartSeriesBase != null && chartSeriesBase.ActualArea != null && chartSeriesBase.ActualArea.Tooltip != null)
    {
      Canvas canvas = !(chartSeriesBase.ActualArea is SfChart) ? (chartSeriesBase.ActualArea as SfChart3D).GetAdorningCanvas() : (chartSeriesBase.ActualArea as SfChart).GetAdorningCanvas();
      if (canvas != null && canvas.Children.Contains((UIElement) chartSeriesBase.ActualArea.Tooltip))
        canvas.Children.Remove((UIElement) chartSeriesBase.ActualArea.Tooltip);
    }
    if (chartSeriesBase == null || chartSeriesBase.ActualArea == null || !(chartSeriesBase.ActualArea is SfChart) || !(bool) args.NewValue)
      return;
    ChartSeriesBase.AddTooltipBehavior((SfChart) chartSeriesBase.ActualArea);
  }

  internal static void AddTooltipBehavior(SfChart chart)
  {
    if (chart == null || chart.Behaviors == null)
      return;
    bool flag = false;
    foreach (ChartBehavior behavior in (Collection<ChartBehavior>) chart.Behaviors)
    {
      if (behavior is ChartTooltipBehavior)
      {
        flag = true;
        break;
      }
    }
    if (flag)
      return;
    ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
    chartTooltipBehavior.ChartArea = chart;
    chart.Behaviors.Add((ChartBehavior) chartTooltipBehavior);
  }

  private static void OnSegmentSpacingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartSeriesBase chartSeriesBase = d as ChartSeriesBase;
    if (chartSeriesBase.ActualArea == null)
      return;
    chartSeriesBase.ActualArea.ScheduleUpdate();
  }

  private static void OnLabelPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is XyDataSeries3D xyDataSeries3D))
      return;
    xyDataSeries3D.OnLabelPropertyChanged();
  }

  private static void OnVisibilityOnLegendChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartSeriesBase chartSeriesBase = (ChartSeriesBase) d;
    if (e.NewValue == null || (Visibility) e.NewValue != Visibility.Visible || chartSeriesBase == null || chartSeriesBase.ActualArea == null)
      return;
    chartSeriesBase.ActualArea.UpdateLegend(chartSeriesBase.ActualArea.Legend, false);
  }

  private static void OnSeriesSelectionBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    ChartSeriesBase chartSeriesBase = d as ChartSeriesBase;
    chartSeriesBase.Segments.Clear();
    if (chartSeriesBase.ActualArea == null)
      return;
    chartSeriesBase.ActualArea.ScheduleUpdate();
  }

  private static void OnSegmentColorPathChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartSeriesBase chartSeriesBase = d as ChartSeriesBase;
    if (!chartSeriesBase.IsColorPathSeries)
      return;
    if (chartSeriesBase.ActualArea != null)
      chartSeriesBase.ActualArea.IsUpdateLegend = true;
    if (chartSeriesBase.ColorValues == null)
    {
      chartSeriesBase.OnBindingPathChanged(e);
    }
    else
    {
      chartSeriesBase.ColorValues.Clear();
      if (chartSeriesBase.SegmentColorPath != null)
        chartSeriesBase.GenerateSegmentColor();
      chartSeriesBase.Segments.Clear();
      chartSeriesBase.UpdateArea();
    }
  }

  private static void OnIsSeriesVisibleChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as ChartSeriesBase).IsSeriesVisibleChanged(args);
  }

  private static void OnColorModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ChartSeriesBase).OnColorModelChanged();
  }

  private static void OnLegendIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ChartSeriesBase).UpdateLegendIconTemplate(true);
  }

  private static void OnSortDataOrderChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartSeriesBase chartSeriesBase = d as ChartSeriesBase;
    if (!chartSeriesBase.IsPointGenerated)
      return;
    if (e.Property == ChartSeriesBase.IsSortDataProperty)
    {
      if (chartSeriesBase.IsSortData)
      {
        chartSeriesBase.CreateYValueCollection(chartSeriesBase.YPaths.Length);
        for (int index1 = 0; index1 < chartSeriesBase.YPaths.Length; ++index1)
        {
          for (int index2 = 0; index2 < chartSeriesBase.ActualSeriesYValues[index1].Count; ++index2)
            chartSeriesBase.SeriesYValues[index1].Add(chartSeriesBase.ActualSeriesYValues[index1][index2]);
        }
        chartSeriesBase.SortActualPoints();
      }
      else
      {
        for (int index3 = 0; index3 < chartSeriesBase.YPaths.Length; ++index3)
        {
          chartSeriesBase.ActualSeriesYValues[index3].Clear();
          for (int index4 = 0; index4 < chartSeriesBase.SeriesYValues[index3].Count; ++index4)
            chartSeriesBase.ActualSeriesYValues[index3].Add(chartSeriesBase.SeriesYValues[index3][index4]);
        }
        chartSeriesBase.ActualXValues = chartSeriesBase.XValues;
      }
    }
    else if (chartSeriesBase.IsSortData)
      chartSeriesBase.SortChartPoints();
    chartSeriesBase.UpdateArea();
  }

  private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ChartSeriesBase).OnPaletteChanged(e);
  }

  private static void OnEmptyPointStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartSeriesBase).RevalidateEmptyPointsStyle();
  }

  private static void OnEmptyPointValueChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartSeriesBase).RevalidateEmptyPointsValue();
  }

  private static void OnShowEmptyPointsChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartSeriesBase chartSeriesBase = d as ChartSeriesBase;
    chartSeriesBase.UpdateArea();
    if ((bool) e.NewValue && chartSeriesBase.SeriesYValues != null && !string.IsNullOrEmpty(chartSeriesBase.XBindingPath.ToString()) && chartSeriesBase.YPaths != null && chartSeriesBase.ItemsSource != null)
      chartSeriesBase.ValidateDataPoints(chartSeriesBase.SeriesYValues);
    if (!(d is AccumulationSeriesBase) || chartSeriesBase.ActualArea == null)
      return;
    chartSeriesBase.ActualArea.IsUpdateLegend = true;
  }

  private static void OnEmptyPointInteriorChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartSeriesBase).RevalidateEmptyPointsValue();
  }

  private static object ReflectedObject(object parentObj, string actualPath)
  {
    IPropertyAccessor propertyAccessor = (IPropertyAccessor) null;
    PropertyInfo propertyInfo = ChartDataUtils.GetPropertyInfo(parentObj, actualPath);
    if (propertyInfo != (PropertyInfo) null)
      propertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo);
    return propertyAccessor?.GetValue(parentObj);
  }

  private static void OnBindingPathXChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ChartSeriesBase chartSeriesBase = obj as ChartSeriesBase;
    if (args.NewValue != null)
      chartSeriesBase.XComplexPaths = args.NewValue.ToString().Split('.');
    chartSeriesBase.OnBindingPathChanged(args);
  }

  private static void OnAppearanceChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ChartSeriesBase chartSeriesBase = obj as ChartSeriesBase;
    chartSeriesBase.OnAppearanceChanged(chartSeriesBase);
  }

  private static void OnDataSourceChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as ChartSeriesBase).OnDataSourceChanged(args);
  }

  private void Refresh()
  {
    if (this.ActualData != null)
      this.ActualData.Clear();
    if (this.ActualXValues is IList<double>)
    {
      (this.XValues as IList<double>).Clear();
      (this.ActualXValues as IList<double>).Clear();
    }
    else if (this.ActualXValues is IList<string>)
    {
      (this.XValues as IList<string>).Clear();
      (this.ActualXValues as IList<string>).Clear();
    }
    if (this.ActualSeriesYValues != null && ((IEnumerable<IList<double>>) this.ActualSeriesYValues).Count<IList<double>>() > 0)
    {
      foreach (IList<double> actualSeriesYvalue in this.ActualSeriesYValues)
        actualSeriesYvalue?.Clear();
      foreach (IList<double> seriesYvalue in this.SeriesYValues)
        seriesYvalue?.Clear();
    }
    if (this.EmptyPointIndexes != null && ((IEnumerable<List<int>>) this.EmptyPointIndexes).Count<List<int>>() > 0)
    {
      foreach (IList<int> emptyPointIndex in this.EmptyPointIndexes)
        emptyPointIndex?.Clear();
    }
    if (this is ISegmentSelectable)
    {
      if (this.ActualArea.GetEnableSegmentSelection())
        this.SelectedSegmentsIndexes.Clear();
      else
        this.ActualArea.SelectedSeriesCollection.Clear();
    }
    this.dataCount = 0;
    if (this.XBindingPath == null || this.YPaths == null || ((IEnumerable<string>) this.YPaths).Count<string>() <= 0)
      return;
    this.GeneratePoints();
    this.Segments.Clear();
    this.ClearAdornments();
    this.UpdateArea();
  }

  private void Timer_Tick(object sender, object e)
  {
    this.RemoveTooltip();
    this.Timer.Stop();
  }

  private void OnActualTransposeChanged()
  {
    switch (this)
    {
      case CartesianSeries _:
      case CartesianSeries3D _:
        if (this.ActualXAxis == null || this.ActualYAxis == null)
          break;
        this.ActualXAxis.Orientation = this.IsActualTransposed ? Orientation.Vertical : Orientation.Horizontal;
        this.ActualYAxis.Orientation = this.IsActualTransposed ? Orientation.Horizontal : Orientation.Vertical;
        break;
    }
  }

  private void IsSeriesVisibleChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.ActualArea == null)
      return;
    this.OnPropertyChanged("IsSeriesVisible");
    bool flag = this is PolarRadarSeriesBase;
    if ((bool) args.NewValue)
    {
      if (this.ActualArea.ActualSeries.Contains(this) && !this.ActualArea.VisibleSeries.Contains(this) && !flag)
      {
        int seriesIndex = this.ActualArea.GetSeriesIndex(this);
        int count = this.ActualArea.VisibleSeries.Count;
        this.ActualArea.VisibleSeries.Insert(seriesIndex > count ? count : seriesIndex, this);
      }
      this.Visibility = Visibility.Visible;
    }
    else
    {
      if (this.ActualArea.VisibleSeries.Contains(this) && !flag)
        this.ActualArea.VisibleSeries.Remove(this);
      this.Visibility = Visibility.Collapsed;
      this.RemoveTooltip();
      this.Timer.Stop();
    }
    if (this.ActualArea.Legend != null && this.IsSingleAccumulationSeries)
    {
      foreach (object obj in (IEnumerable) (this.ActualArea.Legend as ChartLegend).Items)
        (obj as LegendItem).IsSeriesVisible = this.Visibility == Visibility.Visible;
    }
    this.ActualArea.SBSInfoCalculated = false;
    if (this.ActualArea is SfChart)
      (this.ActualArea as SfChart).AddOrRemoveBitmap();
    this.UpdateArea();
  }

  private void CalculateYValues(
    FinancialTechnicalIndicator technicalIndicator,
    int i,
    out List<double> y1,
    out List<double> y2,
    double x)
  {
    y1 = new List<double>();
    y2 = new List<double>();
    for (int index = 0; index < ((IEnumerable<IList<double>>) this.ActualSeriesYValues).Count<IList<double>>(); ++index)
    {
      double num = this.ActualSeriesYValues[index][i];
      y1.Add(num);
      if (this is RangeColumnSeries && !this.IsMultipleYPathRequired)
        break;
    }
    if (technicalIndicator == null)
      return;
    foreach (ChartSegment segment in (Collection<ChartSegment>) technicalIndicator.Segments)
    {
      if (segment is FastColumnBitmapSegment columnBitmapSegment)
      {
        if (i < columnBitmapSegment.y1ChartVals.Count)
          y2.Add(columnBitmapSegment.y1ChartVals[i]);
      }
      else
      {
        TechnicalIndicatorSegment indicatorSegment = segment as TechnicalIndicatorSegment;
        if (i < indicatorSegment.yChartVals.Count)
        {
          if ((indicatorSegment.Length == 0 ? (x < indicatorSegment.xChartVals[i] ? 1 : 0) : (i < indicatorSegment.Length - 1 ? 1 : 0)) != 0)
          {
            y2.Add(double.NaN);
          }
          else
          {
            double yChartVal = indicatorSegment.yChartVals[i];
            y2.Add(yChartVal);
          }
        }
      }
    }
  }

  private void OnColorModelChanged()
  {
    if (this.ColorModel != null)
    {
      this.ColorModel.Palette = this.Palette;
      this.ColorModel.Series = this;
    }
    if (this.ActualArea == null || this.Palette != ChartColorPalette.Custom)
      return;
    this.Segments.Clear();
    this.ActualArea.IsUpdateLegend = true;
    this.UpdateArea();
  }

  private void OnPaletteChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.ColorModel != null)
      this.ColorModel.Palette = this.Palette;
    if (this.SegmentColorPath != null && this.IsColorPathSeries && this.ColorValues != null)
    {
      this.ColorValues.Clear();
      this.GenerateSegmentColor();
    }
    if (this.ActualArea == null)
      return;
    this.Segments.Clear();
    this.ActualArea.IsUpdateLegend = true;
    this.UpdateArea();
  }

  private void RevalidateEmptyPointsStyle()
  {
    if (this.Segments.Count <= 0)
      return;
    this.UpdateArea();
  }

  private void CreateYValueCollection(int count)
  {
    this.SeriesYValues = new IList<double>[count];
    for (int index = 0; index < count; ++index)
      this.SeriesYValues[index] = (IList<double>) new List<double>();
  }

  private void GenerateSegmentColor()
  {
    IEnumerator enumerator = (this.ItemsSource as IEnumerable).GetEnumerator();
    if (!enumerator.MoveNext())
      return;
    IPropertyAccessor propertyAccessor = (IPropertyAccessor) null;
    PropertyInfo propertyInfo = ChartDataUtils.GetPropertyInfo(enumerator.Current, this.SegmentColorPath);
    if (propertyInfo != (PropertyInfo) null)
      propertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo);
    if (propertyAccessor == null)
      return;
    System.Func<object, object> getMethod = propertyAccessor.GetMethod;
    do
    {
      object obj = getMethod(enumerator.Current);
      this.ColorValues.Add(obj != null ? (Brush) obj : (Brush) null);
    }
    while (enumerator.MoveNext());
  }

  private void SortActualData()
  {
    if (this.ItemsSource == null)
      return;
    IEnumerable actualData = (IEnumerable) this.ActualData;
    List<object> list1 = this.ActualData.ToList<object>();
    XyDataSeries xyDataSeries = this as XyDataSeries;
    IList list2 = !(this.XValues is IList<string>) ? (IList) (this.ActualXValues as List<double>) : (IList) (this.ActualXValues as List<string>);
    IList actualSeriesYvalue = (IList) (this.ActualSeriesYValues[0] as List<double>);
    foreach (object parentObj in actualData)
    {
      object obj1 = ChartSeriesBase.ReflectedObject(parentObj, this.XBindingPath);
      object obj2 = xyDataSeries != null ? ChartSeriesBase.ReflectedObject(parentObj, xyDataSeries.YBindingPath) : (object) null;
      for (int index = 0; index < list2.Count; ++index)
      {
        if (obj1 != null && list2[index].Equals(obj1) && (xyDataSeries == null || obj2 != null && actualSeriesYvalue[index].Equals(obj2)))
        {
          list1.RemoveAt(index);
          list1.Insert(index, parentObj);
          break;
        }
      }
    }
    this.ActualData = list1;
  }

  private void HookComplexProperty(object parentObj, string[] paths)
  {
    for (int index = 0; index < paths.Length; ++index)
    {
      parentObj = this.GetComplexArrayPropertyValue(parentObj, paths[index]);
      if (parentObj is INotifyPropertyChanged notifyPropertyChanged)
      {
        notifyPropertyChanged.PropertyChanged -= new PropertyChangedEventHandler(this.OnItemPropertyChanged);
        notifyPropertyChanged.PropertyChanged += new PropertyChangedEventHandler(this.OnItemPropertyChanged);
      }
    }
  }

  private void SortActualPoints()
  {
    if (this.XValues is IList<double>)
    {
      this.ActualXValues = (IEnumerable) new List<double>();
      List<double> actualXvalues = this.ActualXValues as List<double>;
      List<double> xvalues = this.XValues as List<double>;
      for (int index = 0; index < this.DataCount; ++index)
        actualXvalues.Add(xvalues[index]);
    }
    else
    {
      this.ActualXValues = (IEnumerable) new List<string>();
      List<string> actualXvalues = this.ActualXValues as List<string>;
      List<string> xvalues = this.XValues as List<string>;
      for (int index = 0; index < this.DataCount; ++index)
        actualXvalues.Add(xvalues[index]);
    }
    if (this.YPaths != null)
    {
      for (int index1 = 0; index1 < this.YPaths.Length; ++index1)
      {
        this.ActualSeriesYValues[index1].Clear();
        for (int index2 = 0; index2 < this.SeriesYValues[index1].Count; ++index2)
          this.ActualSeriesYValues[index1].Add(this.SeriesYValues[index1][index2]);
      }
    }
    this.SortChartPoints();
  }

  private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (this.isNotificationSuspended)
    {
      this.isPropertyNotificationSuspended = true;
    }
    else
    {
      XyzDataSeries3D xyzDataSeries3D = this as XyzDataSeries3D;
      if (this.IsComplexYProperty || this.XBindingPath.Contains<char>('.') || xyzDataSeries3D != null && xyzDataSeries3D.ZBindingPath.Contains<char>('.'))
      {
        this.ComplexPropertyChanged(sender, e);
      }
      else
      {
        if (!(this.XBindingPath == e.PropertyName) && (this.YPaths == null || !((IEnumerable<string>) this.YPaths).Contains<string>(e.PropertyName)) && !(this.SegmentColorPath == e.PropertyName) && (!(this is XyzDataSeries3D) || !((this as XyzDataSeries3D).ZBindingPath == e.PropertyName)) && (!(this is WaterfallSeries) || !((this as WaterfallSeries).SummaryBindingPath == e.PropertyName)))
          return;
        int index = -1;
        foreach (object obj in this.ItemsSource as IEnumerable)
        {
          ++index;
          if (obj == sender)
            break;
        }
        if (index == -1)
          return;
        this.SetIndividualPoint(index, sender, true);
        if (this.IsSortData)
          this.SortActualPoints();
        if (this is AccumulationSeriesBase && this.ActualArea != null)
          this.ActualArea.IsUpdateLegend = true;
        if (!this.isRepeatPoint || this.isSegmentColorChanged)
          this.UpdateArea();
        this.isSegmentColorChanged = false;
      }
    }
  }

  private void ComplexPropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    int index1 = -1;
    bool flag1 = false;
    bool flag2 = false;
    object parentObj = (object) null;
    string[] strArray = this.XComplexPaths;
    for (int index2 = 0; index2 < this.YPaths.Length; ++index2)
    {
      if (this.YPaths[index2].Contains(e.PropertyName))
      {
        flag1 = true;
        if (flag1)
        {
          strArray = this.YComplexPaths[index2];
          break;
        }
        break;
      }
    }
    if (this is XyzDataSeries3D xyzDataSeries3D && xyzDataSeries3D.ZBindingPath.Contains(e.PropertyName))
    {
      flag2 = true;
      strArray = xyzDataSeries3D.ZComplexPaths;
    }
    if (!this.XBindingPath.Contains(e.PropertyName) && !flag1 && !flag2)
      return;
    foreach (object obj in this.ItemsSource as IEnumerable)
    {
      parentObj = obj;
      for (int index3 = 0; index3 < strArray.Length - 1; ++index3)
        parentObj = ChartSeriesBase.ReflectedObject(parentObj, strArray[index3]);
      ++index1;
      if (parentObj == sender)
      {
        parentObj = obj;
        break;
      }
    }
    if (index1 == -1)
      return;
    this.SetIndividualPoint(index1, parentObj, true);
    if (this.IsSortData)
      this.SortActualPoints();
    this.UpdateArea();
  }

  private void SortChartPoints()
  {
    if (this.xValueType != ChartValueType.String)
      this.GetTuple<double>(this.ActualXValues as List<double>);
    else
      this.GetTuple<string>(this.ActualXValues as List<string>);
    this.SortActualData();
  }

  private void GetTuple<T>(List<T> xValues) where T : IComparable<T>
  {
    switch (this.SeriesYCount)
    {
      case 1:
        List<Tuple<T, double>> list1 = new List<Tuple<T, double>>();
        List<double> actualSeriesYvalue1 = this.ActualSeriesYValues[0] as List<double>;
        for (int index = 0; index < this.DataCount; ++index)
          list1.Add(Tuple.Create<T, double>(xValues[index], actualSeriesYvalue1[index]));
        if (list1.Count <= 0)
          break;
        this.Sort<T, double>(list1);
        break;
      case 2:
        List<Tuple<T, double, double>> list2 = new List<Tuple<T, double, double>>();
        List<double> actualSeriesYvalue2 = this.ActualSeriesYValues[0] as List<double>;
        List<double> actualSeriesYvalue3 = this.ActualSeriesYValues[1] as List<double>;
        for (int index = 0; index < this.DataCount; ++index)
          list2.Add(Tuple.Create<T, double, double>(xValues[index], actualSeriesYvalue2[index], actualSeriesYvalue3[index]));
        if (list2.Count <= 0)
          break;
        this.Sort<T, double, double>(list2);
        break;
      case 4:
        List<Tuple<T, double, double, double, double>> list3 = new List<Tuple<T, double, double, double, double>>();
        List<double> actualSeriesYvalue4 = this.ActualSeriesYValues[0] as List<double>;
        List<double> actualSeriesYvalue5 = this.ActualSeriesYValues[1] as List<double>;
        List<double> actualSeriesYvalue6 = this.ActualSeriesYValues[2] as List<double>;
        List<double> actualSeriesYvalue7 = this.ActualSeriesYValues[3] as List<double>;
        for (int index = 0; index < this.DataCount; ++index)
          list3.Add(Tuple.Create<T, double, double, double, double>(xValues[index], actualSeriesYvalue4[index], actualSeriesYvalue5[index], actualSeriesYvalue6[index], actualSeriesYvalue7[index]));
        if (list3.Count <= 0)
          break;
        this.Sort<T, double, double, double, double>(list3);
        break;
      case 5:
        List<Tuple<T, double, double, double, double, double>> list4 = new List<Tuple<T, double, double, double, double, double>>();
        List<double> actualSeriesYvalue8 = this.ActualSeriesYValues[0] as List<double>;
        List<double> actualSeriesYvalue9 = this.ActualSeriesYValues[1] as List<double>;
        List<double> actualSeriesYvalue10 = this.ActualSeriesYValues[2] as List<double>;
        List<double> actualSeriesYvalue11 = this.ActualSeriesYValues[3] as List<double>;
        List<double> actualSeriesYvalue12 = this.ActualSeriesYValues[4] as List<double>;
        for (int index = 0; index < this.DataCount; ++index)
          list4.Add(Tuple.Create<T, double, double, double, double, double>(xValues[index], actualSeriesYvalue8[index], actualSeriesYvalue9[index], actualSeriesYvalue10[index], actualSeriesYvalue11[index], actualSeriesYvalue12[index]));
        if (list4.Count <= 0)
          break;
        this.Sort<T, double, double, double, double, double>(list4);
        break;
    }
  }

  private void Sort<T, T1>(List<Tuple<T, T1>> list)
    where T : IComparable<T>
    where T1 : IComparable<T1>
  {
    switch (this.SortBy)
    {
      case SortingAxis.X:
        list.Sort((Comparison<Tuple<T, T1>>) ((t1, t2) => t1.Item1.CompareTo(t2.Item1)));
        this.ActualSort<T, T1>(list);
        break;
      case SortingAxis.Y:
        list.Sort((Comparison<Tuple<T, T1>>) ((t1, t2) => t1.Item2.CompareTo(t2.Item2)));
        this.ActualSort<T, T1>(list);
        break;
    }
  }

  private void ActualSort<T, T1>(List<Tuple<T, T1>> list)
  {
    this.ActualXValues = (IEnumerable) list.Select<Tuple<T, T1>, T>((System.Func<Tuple<T, T1>, T>) (x => x.Item1)).ToList<T>();
    if (this.SortDirection == Direction.Descending)
      (this.ActualXValues as List<T>).Reverse();
    if (this.ActualSeriesYValues == null)
      return;
    List<T1> actualSeriesYvalue = this.ActualSeriesYValues[0] as List<T1>;
    int index = 0;
    foreach (Tuple<T, T1> tuple in list)
    {
      actualSeriesYvalue[index] = tuple.Item2;
      ++index;
    }
    if (this.SortDirection != Direction.Descending)
      return;
    actualSeriesYvalue.Reverse();
  }

  private void Sort<T, T1, T2>(List<Tuple<T, T1, T2>> list)
    where T : IComparable<T>
    where T1 : IComparable<T1>
    where T2 : IComparable<T2>
  {
    switch (this.SortBy)
    {
      case SortingAxis.X:
        list.Sort((Comparison<Tuple<T, T1, T2>>) ((t1, t2) => t1.Item1.CompareTo(t2.Item1)));
        this.ActualSort<T, T1, T2>(list);
        break;
      case SortingAxis.Y:
        list.Sort((Comparison<Tuple<T, T1, T2>>) ((t1, t2) => t1.Item2.CompareTo(t2.Item2)));
        this.ActualSort<T, T1, T2>(list);
        break;
    }
  }

  private void ActualSort<T, T1, T2>(List<Tuple<T, T1, T2>> list)
  {
    this.ActualXValues = (IEnumerable) list.Select<Tuple<T, T1, T2>, T>((System.Func<Tuple<T, T1, T2>, T>) (x => x.Item1)).ToList<T>();
    if (this.SortDirection == Direction.Descending)
      (this.ActualXValues as List<T>).Reverse();
    if (this.ActualSeriesYValues == null)
      return;
    List<T1> actualSeriesYvalue1 = this.ActualSeriesYValues[0] as List<T1>;
    List<T2> actualSeriesYvalue2 = this.ActualSeriesYValues[1] as List<T2>;
    int index = 0;
    foreach (Tuple<T, T1, T2> tuple in list)
    {
      actualSeriesYvalue1[index] = tuple.Item2;
      actualSeriesYvalue2[index] = tuple.Item3;
      ++index;
    }
    if (this.SortDirection != Direction.Descending)
      return;
    actualSeriesYvalue1.Reverse();
    actualSeriesYvalue2.Reverse();
  }

  private void Sort<T, T1, T2, T3, T4>(List<Tuple<T, T1, T2, T3, T4>> list)
    where T : IComparable<T>
    where T1 : IComparable<T1>
    where T2 : IComparable<T2>
    where T3 : IComparable<T3>
    where T4 : IComparable<T4>
  {
    switch (this.SortBy)
    {
      case SortingAxis.X:
        list.Sort((Comparison<Tuple<T, T1, T2, T3, T4>>) ((t1, t2) => t1.Item1.CompareTo(t2.Item1)));
        this.ActualSort<T, T1, T2, T3, T4>(list);
        break;
      case SortingAxis.Y:
        list.Sort((Comparison<Tuple<T, T1, T2, T3, T4>>) ((t1, t2) => t1.Item2.CompareTo(t2.Item2)));
        this.ActualSort<T, T1, T2, T3, T4>(list);
        break;
    }
  }

  private void ActualSort<T, T1, T2, T3, T4>(List<Tuple<T, T1, T2, T3, T4>> list)
  {
    this.ActualXValues = (IEnumerable) list.Select<Tuple<T, T1, T2, T3, T4>, T>((System.Func<Tuple<T, T1, T2, T3, T4>, T>) (x => x.Item1)).ToList<T>();
    if (this.SortDirection == Direction.Descending)
      (this.ActualXValues as List<T>).Reverse();
    if (this.ActualSeriesYValues == null)
      return;
    List<T1> actualSeriesYvalue1 = this.ActualSeriesYValues[0] as List<T1>;
    List<T2> actualSeriesYvalue2 = this.ActualSeriesYValues[1] as List<T2>;
    int index = 0;
    foreach (Tuple<T, T1, T2, T3, T4> tuple in list)
    {
      (this.ActualSeriesYValues[0] as List<T1>)[index] = tuple.Item2;
      (this.ActualSeriesYValues[1] as List<T2>)[index] = tuple.Item3;
      (this.ActualSeriesYValues[2] as List<T3>)[index] = tuple.Item4;
      (this.ActualSeriesYValues[3] as List<T4>)[index] = tuple.Item5;
      ++index;
    }
    if (this.SortDirection != Direction.Descending)
      return;
    actualSeriesYvalue1.Reverse();
    actualSeriesYvalue2.Reverse();
  }

  private void Sort<T, T1, T2, T3, T4, T5>(List<Tuple<T, T1, T2, T3, T4, T5>> list)
    where T : IComparable<T>
    where T1 : IComparable<T1>
    where T2 : IComparable<T2>
  {
    switch (this.SortBy)
    {
      case SortingAxis.X:
        list.Sort((Comparison<Tuple<T, T1, T2, T3, T4, T5>>) ((t1, t2) => t1.Item1.CompareTo(t2.Item1)));
        this.ActualSort<T, T1, T2, T3, T4, T5>(list);
        break;
      case SortingAxis.Y:
        list.Sort((Comparison<Tuple<T, T1, T2, T3, T4, T5>>) ((t1, t2) => t1.Item2.CompareTo(t2.Item2)));
        this.ActualSort<T, T1, T2, T3, T4, T5>(list);
        break;
    }
  }

  private void ActualSort<T, T1, T2, T3, T4, T5>(List<Tuple<T, T1, T2, T3, T4, T5>> list)
  {
    this.ActualXValues = (IEnumerable) list.Select<Tuple<T, T1, T2, T3, T4, T5>, T>((System.Func<Tuple<T, T1, T2, T3, T4, T5>, T>) (x => x.Item1)).ToList<T>();
    if (this.SortDirection == Direction.Descending)
      (this.ActualXValues as List<T>).Reverse();
    if (this.ActualSeriesYValues == null)
      return;
    List<T1> actualSeriesYvalue1 = this.ActualSeriesYValues[0] as List<T1>;
    List<T2> actualSeriesYvalue2 = this.ActualSeriesYValues[1] as List<T2>;
    List<T3> actualSeriesYvalue3 = this.ActualSeriesYValues[1] as List<T3>;
    List<T4> actualSeriesYvalue4 = this.ActualSeriesYValues[1] as List<T4>;
    List<T5> actualSeriesYvalue5 = this.ActualSeriesYValues[1] as List<T5>;
    int index = 0;
    foreach (Tuple<T, T1, T2, T3, T4, T5> tuple in list)
    {
      actualSeriesYvalue1[index] = tuple.Item2;
      actualSeriesYvalue2[index] = tuple.Item3;
      actualSeriesYvalue3[index] = tuple.Item4;
      actualSeriesYvalue4[index] = tuple.Item5;
      actualSeriesYvalue5[index] = tuple.Item6;
      ++index;
    }
    if (this.SortDirection != Direction.Descending)
      return;
    actualSeriesYvalue1.Reverse();
    actualSeriesYvalue2.Reverse();
    actualSeriesYvalue3.Reverse();
    actualSeriesYvalue4.Reverse();
    actualSeriesYvalue5.Reverse();
  }

  private void OnAppearanceChanged(ChartSeriesBase obj)
  {
    if (this.IsBitmapSeries || this is ChartSeries3D || this.SegmentColorPath != null && this.IsColorPathSeries)
    {
      obj.UpdateArea();
    }
    else
    {
      if (obj.adornmentInfo == null)
        return;
      obj.adornmentInfo.UpdateLabels();
      obj.adornmentInfo.UpdateConnectingLines();
      if (!obj.adornmentInfo.UseSeriesPalette || !obj.adornmentInfo.IsAdornmentLabelCreatedEventHooked)
        return;
      foreach (ChartAdornment adornment in (Collection<ChartAdornment>) obj.Adornments)
      {
        if (adornment.CustomAdornmentLabel != null)
        {
          adornment.CustomAdornmentLabel.SymbolInterior = obj.Interior;
          adornment.CustomAdornmentLabel.SymbolStroke = obj.Interior;
        }
      }
    }
  }

  private void OnBindingListChanged(ListChangedType listChangedType, int index)
  {
    NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Reset;
    switch (listChangedType)
    {
      case ListChangedType.ItemAdded:
        if (this.ItemsSource != null)
        {
          if (!this.isNotificationSuspended)
          {
            this.SetIndividualPoint(index, (this.ItemsSource as IBindingList)[index], false);
            if (this.IsSortData)
              this.SortActualPoints();
            this.UpdateSegments(index, NotifyCollectionChangedAction.Add);
          }
          else if (!this.isUpdateStarted)
          {
            this.UpdateStartedIndex = index;
            this.isUpdateStarted = true;
          }
          action = NotifyCollectionChangedAction.Add;
          break;
        }
        break;
      case ListChangedType.ItemDeleted:
        if (this.ItemsSource != null)
        {
          if (this.XValues is IList<double>)
          {
            (this.XValues as IList<double>).RemoveAt(index);
            --this.dataCount;
          }
          else if (this.XValues is IList<string>)
          {
            (this.XValues as IList<string>).RemoveAt(index);
            --this.dataCount;
          }
          for (int index1 = 0; index1 < ((IEnumerable<IList<double>>) this.SeriesYValues).Count<IList<double>>(); ++index1)
            this.SeriesYValues[index1].RemoveAt(index);
          if (this.IsSortData)
            this.SortActualPoints();
          this.ActualData.RemoveAt(index);
          this.UpdateSegments(index, NotifyCollectionChangedAction.Remove);
          action = NotifyCollectionChangedAction.Remove;
          break;
        }
        break;
      case ListChangedType.ItemChanged:
        if (index > -1 && index < this.DataCount && (this.ItemsSource as IBindingList)[index] != null)
        {
          this.SetIndividualPoint(index, (this.ItemsSource as IBindingList)[index], true);
          if (this.IsSortData)
            this.SortActualPoints();
          this.UpdateSegments(index, NotifyCollectionChangedAction.Replace);
          action = NotifyCollectionChangedAction.Replace;
          break;
        }
        break;
      default:
        this.Refresh();
        break;
    }
    if (this.ShowEmptyPoints)
      this.RevalidateEmptyPointsCollection(action, index, index);
    if (this is AccumulationSeriesBase)
      this.ActualArea.IsUpdateLegend = true;
    this.totalCalculated = false;
  }

  private void SetIndividualColorValue(int newStartingIndex, object obj, bool isReplace)
  {
    Brush brush = ChartSeriesBase.ReflectedObject(obj, this.SegmentColorPath) as Brush;
    if (isReplace)
    {
      if (this.ColorValues[newStartingIndex] == brush)
      {
        this.isSegmentColorChanged = false;
      }
      else
      {
        this.ColorValues[newStartingIndex] = brush;
        this.isSegmentColorChanged = true;
      }
    }
    else
      this.ColorValues.Insert(newStartingIndex, brush ?? (Brush) null);
  }

  private void UnHookPropertyChanged(object dataSource)
  {
    if (dataSource == null || !(dataSource is IEnumerable enumerable))
      return;
    IEnumerator enumerator = enumerable.GetEnumerator();
    if (!enumerator.MoveNext())
      return;
    do
    {
      if (enumerator.Current is INotifyPropertyChanged current)
        current.PropertyChanged -= new PropertyChangedEventHandler(this.OnItemPropertyChanged);
    }
    while (enumerator.MoveNext());
  }

  private void UnHookPropertyChangedEvent(bool needPropertyChange, object obj)
  {
    if (!(obj is INotifyPropertyChanged notifyPropertyChanged) || !needPropertyChange)
      return;
    notifyPropertyChanged.PropertyChanged -= new PropertyChangedEventHandler(this.OnItemPropertyChanged);
  }

  private void RevalidateEmptyPointsCollection(
    NotifyCollectionChangedAction action,
    int newIndex,
    int oldIndex)
  {
    if (this.EmptyPointIndexes != null)
    {
      foreach (int index in this.EmptyPointIndexes[0])
      {
        if (this.Segments.Count > index)
          this.Segments[index].IsEmptySegmentInterior = false;
      }
      switch (action)
      {
        case NotifyCollectionChangedAction.Add:
          if (double.IsNaN(this.SeriesYValues[0][newIndex]))
          {
            this.EmptyPointIndexes[0].Add(newIndex);
            break;
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          List<int> intList1 = new List<int>();
          foreach (List<int> emptyPointIndex in this.EmptyPointIndexes)
          {
            if (emptyPointIndex.Contains(oldIndex))
              emptyPointIndex.Remove(oldIndex);
            foreach (int num in emptyPointIndex.Where<int>((System.Func<int, bool>) (item => item > oldIndex)))
              intList1.Add(emptyPointIndex.IndexOf(num));
            foreach (int index1 in intList1)
            {
              List<int> intList2;
              int index2;
              emptyPointIndex[index1] = (intList2 = emptyPointIndex)[index2 = index1] = intList2[index2] - 1;
            }
            intList1.Clear();
          }
          break;
        case NotifyCollectionChangedAction.Replace:
          if (double.IsNaN(this.SeriesYValues[0][newIndex]))
          {
            if (!this.EmptyPointIndexes[0].Contains(newIndex))
            {
              this.EmptyPointIndexes[0].Add(newIndex);
              break;
            }
            break;
          }
          if (this.EmptyPointIndexes[0].Contains(newIndex))
          {
            this.EmptyPointIndexes[0].Remove(newIndex);
            break;
          }
          break;
      }
    }
    this.RevalidateEmptyPointsValue();
  }

  private void RevalidateEmptyPointsValue()
  {
    if (this.Segments != null && this.Segments.Count > 0)
    {
      IList<double>[] seriesYvalues = this.SeriesYValues;
      int index = 0;
      if (this.EmptyPointIndexes != null)
      {
        foreach (List<int> emptyPointIndex in this.EmptyPointIndexes)
        {
          switch (this.EmptyPointValue)
          {
            case EmptyPointValue.Zero:
              using (List<int>.Enumerator enumerator = emptyPointIndex.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  int current = enumerator.Current;
                  seriesYvalues[index][current] = 0.0;
                }
                break;
              }
            case EmptyPointValue.Average:
              IList<double> doubleList = seriesYvalues[index];
              using (IEnumerator<int> enumerator = emptyPointIndex.OrderBy<int, int>((System.Func<int, int>) (val => val)).GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  int current = enumerator.Current;
                  if (current == 0 && doubleList.Count > 1)
                  {
                    double num = double.IsNaN(doubleList[1]) ? 0.0 : doubleList[1];
                    doubleList[0] = num / 2.0;
                  }
                  else if (current == doubleList.Count - 1 && doubleList.Count > 1)
                  {
                    double num = double.IsNaN(doubleList[current - 1]) ? 0.0 : doubleList[current - 1];
                    doubleList[current] = num / 2.0;
                  }
                  else if (current == 0 && doubleList.Count == 1)
                  {
                    double num = double.IsNaN(doubleList[0]) ? 0.0 : doubleList[0];
                    doubleList[current] = num / 2.0;
                  }
                  else
                  {
                    double num1 = double.IsNaN(doubleList[current - 1]) ? 0.0 : doubleList[current - 1];
                    double num2 = double.IsNaN(doubleList[current + 1]) ? 0.0 : doubleList[current + 1];
                    doubleList[current] = (num1 + num2) / 2.0;
                  }
                }
                break;
              }
          }
          ++index;
        }
      }
    }
    this.UpdateArea();
  }

  internal delegate object GetReflectedProperty(object obj, string[] paths);
}
