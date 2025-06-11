// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartAxis
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

[TemplateVisualState(Name = "VerticalTouchModeStyle", GroupName = "StyleMode")]
[TemplateVisualState(Name = "CommonStyle", GroupName = "StyleMode")]
[TemplateVisualState(Name = "TouchModeStyle", GroupName = "StyleMode")]
public abstract class ChartAxis : Control, ICloneable
{
  protected const int CRoundDecimals = 11;
  public static readonly DependencyProperty MaximumLabelsProperty = DependencyProperty.Register(nameof (MaximumLabels), typeof (int), typeof (ChartAxis), new PropertyMetadata((object) 3, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty HeaderPositionProperty = DependencyProperty.Register(nameof (HeaderPosition), typeof (AxisHeaderPosition), typeof (ChartAxis), new PropertyMetadata((object) AxisHeaderPosition.Near, new PropertyChangedCallback(ChartAxis.OnHeaderPositionChanged)));
  public static readonly DependencyProperty PositionPathProperty = DependencyProperty.Register(nameof (PositionPath), typeof (string), typeof (ChartAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty ContentPathProperty = DependencyProperty.Register(nameof (ContentPath), typeof (string), typeof (ChartAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty LabelFormatProperty = DependencyProperty.Register(nameof (LabelFormat), typeof (string), typeof (ChartAxis), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty PlotOffsetProperty = DependencyProperty.Register(nameof (PlotOffset), typeof (double), typeof (ChartAxis), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty PlotOffsetStartProperty = DependencyProperty.Register(nameof (PlotOffsetStart), typeof (double), typeof (ChartAxis), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty PlotOffsetEndProperty = DependencyProperty.Register(nameof (PlotOffsetEnd), typeof (double), typeof (ChartAxis), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty AxisLineOffsetProperty = DependencyProperty.Register(nameof (AxisLineOffset), typeof (double), typeof (ChartAxis), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty LabelsIntersectActionProperty = DependencyProperty.Register(nameof (LabelsIntersectAction), typeof (AxisLabelsIntersectAction), typeof (ChartAxis), new PropertyMetadata((object) AxisLabelsIntersectAction.Hide, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty LabelsPositionProperty = DependencyProperty.Register(nameof (LabelsPosition), typeof (AxisElementPosition), typeof (ChartAxis), new PropertyMetadata((object) AxisElementPosition.Outside, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty LabelsSourceProperty = DependencyProperty.Register(nameof (LabelsSource), typeof (object), typeof (ChartAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty PostfixLabelTemplateProperty = DependencyProperty.Register(nameof (PostfixLabelTemplate), typeof (DataTemplate), typeof (ChartAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxis.OnLabelTemplateChanged)));
  public static readonly DependencyProperty PrefixLabelTemplateProperty = DependencyProperty.Register(nameof (PrefixLabelTemplate), typeof (DataTemplate), typeof (ChartAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxis.OnLabelTemplateChanged)));
  public static readonly DependencyProperty LabelExtentProperty = DependencyProperty.Register(nameof (LabelExtent), typeof (double), typeof (ChartAxis), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty LabelRotationAngleProperty = DependencyProperty.Register(nameof (LabelRotationAngle), typeof (double), typeof (ChartAxis), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ChartAxis.OnLabelRotationChanged)));
  public static readonly DependencyProperty AxisLineStyleProperty = DependencyProperty.Register(nameof (AxisLineStyle), typeof (Style), typeof (ChartAxis), (PropertyMetadata) null);
  public static readonly DependencyProperty OpposedPositionProperty = DependencyProperty.Register(nameof (OpposedPosition), typeof (bool), typeof (ChartAxis), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartAxis.OnOpposedPositionChanged)));
  public static readonly DependencyProperty DesiredIntervalsCountProperty = DependencyProperty.Register(nameof (DesiredIntervalsCount), typeof (int?), typeof (ChartAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxis.OnDesiredIntervalsCountPropertyChanged)));
  public static readonly DependencyProperty ThumbLabelVisibilityProperty = DependencyProperty.Register(nameof (ThumbLabelVisibility), typeof (Visibility), typeof (ChartAxis), new PropertyMetadata((object) Visibility.Collapsed));
  public static readonly DependencyProperty ThumbLabelTemplateProperty = DependencyProperty.Register(nameof (ThumbLabelTemplate), typeof (DataTemplate), typeof (ChartAxis), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (ChartAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty HeaderStyleProperty = DependencyProperty.Register(nameof (HeaderStyle), typeof (LabelStyle), typeof (ChartAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (ChartAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty TickLineSizeProperty = DependencyProperty.Register(nameof (TickLineSize), typeof (double), typeof (ChartAxis), new PropertyMetadata((object) 8.0, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty IsInversedProperty = DependencyProperty.Register(nameof (IsInversed), typeof (bool), typeof (ChartAxis), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartAxis.OnIsInversedChanged)));
  public static readonly DependencyProperty OriginProperty = DependencyProperty.Register(nameof (Origin), typeof (double), typeof (ChartAxis), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty ShowOriginProperty = DependencyProperty.Register(nameof (ShowOrigin), typeof (bool), typeof (ChartAxis), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty ShowAxisNextToOriginProperty = DependencyProperty.Register(nameof (ShowAxisNextToOrigin), typeof (bool), typeof (ChartAxis), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartAxis.OnShowAxisNextToOriginChanged)));
  public static readonly DependencyProperty OriginLineStyleProperty = DependencyProperty.Register(nameof (OriginLineStyle), typeof (Style), typeof (ChartAxis), (PropertyMetadata) null);
  public static readonly DependencyProperty TickLinesPositionProperty = DependencyProperty.Register(nameof (TickLinesPosition), typeof (AxisElementPosition), typeof (ChartAxis), new PropertyMetadata((object) AxisElementPosition.Outside, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty EdgeLabelsDrawingModeProperty = DependencyProperty.Register(nameof (EdgeLabelsDrawingMode), typeof (EdgeLabelsDrawingMode), typeof (ChartAxis), new PropertyMetadata((object) EdgeLabelsDrawingMode.Center, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty EdgeLabelsVisibilityModeProperty = DependencyProperty.Register(nameof (EdgeLabelsVisibilityMode), typeof (EdgeLabelsVisibilityMode), typeof (ChartAxis), new PropertyMetadata((object) EdgeLabelsVisibilityMode.Default, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty MajorGridLineStyleProperty = DependencyProperty.Register(nameof (MajorGridLineStyle), typeof (Style), typeof (ChartAxis), (PropertyMetadata) null);
  public static readonly DependencyProperty MinorGridLineStyleProperty = DependencyProperty.Register(nameof (MinorGridLineStyle), typeof (Style), typeof (ChartAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty MajorTickLineStyleProperty = DependencyProperty.Register(nameof (MajorTickLineStyle), typeof (Style), typeof (ChartAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty MinorTickLineStyleProperty = DependencyProperty.Register(nameof (MinorTickLineStyle), typeof (Style), typeof (ChartAxis), (PropertyMetadata) null);
  public static readonly DependencyProperty ShowTrackBallInfoProperty = DependencyProperty.Register(nameof (ShowTrackBallInfo), typeof (bool), typeof (ChartAxis), new PropertyMetadata((object) false));
  public static readonly DependencyProperty TrackBallLabelTemplateProperty = DependencyProperty.Register(nameof (TrackBallLabelTemplate), typeof (DataTemplate), typeof (ChartAxis), (PropertyMetadata) null);
  public static readonly DependencyProperty CrosshairLabelTemplateProperty = DependencyProperty.Register(nameof (CrosshairLabelTemplate), typeof (DataTemplate), typeof (ChartAxis), (PropertyMetadata) null);
  public static readonly DependencyProperty ShowGridLinesProperty = DependencyProperty.Register(nameof (ShowGridLines), typeof (bool), typeof (ChartAxis), new PropertyMetadata((object) true, new PropertyChangedCallback(ChartAxis.OnShowGridLinePropertyChanged)));
  public static readonly DependencyProperty LabelStyleProperty = DependencyProperty.Register(nameof (LabelStyle), typeof (LabelStyle), typeof (ChartAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty LabelTemplateProperty = DependencyProperty.Register(nameof (LabelTemplate), typeof (DataTemplate), typeof (ChartAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxis.OnLabelTemplateChanged)));
  public static readonly DependencyProperty EnableAutoIntervalOnZoomingProperty = DependencyProperty.Register(nameof (EnableAutoIntervalOnZooming), typeof (bool), typeof (ChartAxis), new PropertyMetadata((object) true));
  internal static readonly DependencyProperty AxisVisibilityProperty = DependencyProperty.Register(nameof (AxisVisibility), typeof (Visibility), typeof (ChartAxis), new PropertyMetadata((object) Visibility.Visible, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  internal static readonly DependencyProperty ActualTrackBallLabelTemplateProperty = DependencyProperty.Register(nameof (ActualTrackBallLabelTemplate), typeof (DataTemplate), typeof (ChartAxis), (PropertyMetadata) null);
  internal static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (ChartAxis), new PropertyMetadata((object) Orientation.Horizontal, new PropertyChangedCallback(ChartAxis.OnAxisOrientationChanged)));
  internal static readonly DependencyProperty ValueTypeProperty = DependencyProperty.Register(nameof (ValueType), typeof (ChartValueType), typeof (ChartAxis), new PropertyMetadata((object) ChartValueType.Double, new PropertyChangedCallback(ChartAxis.OnValuetypeChanged)));
  internal static readonly DependencyProperty AxisLabelAlignmentProperty = DependencyProperty.Register(nameof (AxisLabelAlignment), typeof (LabelAlignment), typeof (ChartAxis), new PropertyMetadata((object) LabelAlignment.Center, new PropertyChangedCallback(ChartAxis.OnPropertyChanged)));
  public static readonly DependencyProperty RangeStylesProperty = DependencyProperty.Register(nameof (RangeStyles), typeof (ChartAxisRangeStyleCollection), typeof (ChartAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAxis.RangeStylesPropertyChanged)));
  internal static readonly int[] c_intervalDivs = new int[4]
  {
    10,
    5,
    2,
    1
  };
  internal ObservableCollection<ChartAxisLabel> m_VisibleLabels;
  internal bool smallTicksRequired;
  internal bool IsDefaultRange;
  internal bool IsSecondaryAxis;
  internal bool DisableScrollbar;
  internal bool axisElementsUpdateRequired;
  internal ILayoutCalculator AxisLayoutPanel;
  internal ILayoutCalculator axisLabelsPanel;
  internal MultiLevelLabelsPanel axisMultiLevelLabelsPanel;
  internal ILayoutCalculator axisElementsPanel;
  internal UIElementsRecycler<Line> GridLinesRecycler;
  internal UIElementsRecycler<Line> MinorGridLinesRecycler;
  internal ContentControl headerContent;
  internal List<double> m_smalltickPoints = new List<double>();
  internal bool isManipulated;
  internal double previousLabelCoefficientValue = -1.0;
  protected double MaxPixelsCount = 100.0;
  private Rect renderedRect;
  private ChartAxisLabelCollection m_customLabels;
  private ObservableCollection<ISupportAxes> registeredSeries;
  private List<ChartAxis> associatedAxes;
  private double actualPlotOffset;
  private double actualPlotOffsetStart;
  private double actualPlotOffsetEnd;
  private DoubleRange m_actualRange = DoubleRange.Empty;
  private DoubleRange m_actualVisibleRange = DoubleRange.Empty;
  private Rect arrangeRect;
  private bool isChecked;

  internal bool IsActualInversed { get; set; }

  public ChartAxis()
  {
    this.ValueToCoefficientCalc = new ChartAxis.ValueToCoefficientHandler(this.ValueToCoefficient);
    this.CoefficientToValueCalc = new ChartAxis.ValueToCoefficientHandler(this.CoefficientToValue);
    this.DefaultStyleKey = (object) typeof (ChartAxis);
    this.m_VisibleLabels = new ObservableCollection<ChartAxisLabel>();
    this.m_VisibleLabels.CollectionChanged += new NotifyCollectionChangedEventHandler(this.m_VisibleLabels_CollectionChanged);
    BindingOperations.SetBinding((DependencyObject) this, ChartAxis.AxisVisibilityProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Visibility", new object[0])
    });
    this.AxisLabelCoefficientRoundOffValue = 2;
    this.IsActualInversed = false;
  }

  public event EventHandler<ChartAxisBoundsEventArgs> AxisBoundsChanged;

  public event EventHandler<ActualRangeChangedEventArgs> ActualRangeChanged;

  public event EventHandler<LabelCreatedEventArgs> LabelCreated;

  internal event EventHandler<VisibleRangeChangedEventArgs> VisibleRangeChanged;

  public bool IsLogarithmic { get; internal set; }

  public double ActualPlotOffset
  {
    get => this.actualPlotOffset;
    internal set => this.actualPlotOffset = value;
  }

  public int MaximumLabels
  {
    get => (int) this.GetValue(ChartAxis.MaximumLabelsProperty);
    set => this.SetValue(ChartAxis.MaximumLabelsProperty, (object) value);
  }

  public DoubleRange VisibleRange
  {
    get => this.m_actualVisibleRange;
    protected internal set
    {
      DoubleRange actualVisibleRange = this.m_actualVisibleRange;
      this.m_actualVisibleRange = value;
      this.OnAxisVisibleRangeChanged(new VisibleRangeChangedEventArgs()
      {
        OldRange = actualVisibleRange,
        NewRange = value
      });
    }
  }

  public AxisHeaderPosition HeaderPosition
  {
    get => (AxisHeaderPosition) this.GetValue(ChartAxis.HeaderPositionProperty);
    set => this.SetValue(ChartAxis.HeaderPositionProperty, (object) value);
  }

  public Rect ArrangeRect
  {
    get => this.arrangeRect;
    internal set
    {
      this.arrangeRect = value;
      if (this.Orientation.Equals((object) Orientation.Horizontal))
      {
        ChartAxisBase3D chartAxisBase3D = this as ChartAxisBase3D;
        double width = Math.Max(0.0, this.arrangeRect.Width - this.GetActualPlotOffset());
        if (chartAxisBase3D != null && chartAxisBase3D.IsZAxis)
          this.RenderedRect = new Rect(this.GetActualPlotOffsetStart(), this.arrangeRect.Top, width, this.arrangeRect.Height);
        else
          this.RenderedRect = new Rect(this.arrangeRect.Left + this.GetActualPlotOffsetStart(), this.arrangeRect.Top, width, this.arrangeRect.Height);
      }
      else
      {
        double height = Math.Max(0.0, this.arrangeRect.Height - this.GetActualPlotOffset());
        this.RenderedRect = new Rect(this.arrangeRect.Left, this.arrangeRect.Top + this.GetActualPlotOffsetEnd(), this.arrangeRect.Width, height);
      }
    }
  }

  public string PositionPath
  {
    get => (string) this.GetValue(ChartAxis.PositionPathProperty);
    set => this.SetValue(ChartAxis.PositionPathProperty, (object) value);
  }

  public string ContentPath
  {
    get => (string) this.GetValue(ChartAxis.ContentPathProperty);
    set => this.SetValue(ChartAxis.ContentPathProperty, (object) value);
  }

  public string LabelFormat
  {
    get => (string) this.GetValue(ChartAxis.LabelFormatProperty);
    set => this.SetValue(ChartAxis.LabelFormatProperty, (object) value);
  }

  public double PlotOffset
  {
    get => (double) this.GetValue(ChartAxis.PlotOffsetProperty);
    set => this.SetValue(ChartAxis.PlotOffsetProperty, (object) value);
  }

  public double PlotOffsetStart
  {
    get => (double) this.GetValue(ChartAxis.PlotOffsetStartProperty);
    set => this.SetValue(ChartAxis.PlotOffsetStartProperty, (object) value);
  }

  public double PlotOffsetEnd
  {
    get => (double) this.GetValue(ChartAxis.PlotOffsetEndProperty);
    set => this.SetValue(ChartAxis.PlotOffsetEndProperty, (object) value);
  }

  public double AxisLineOffset
  {
    get => (double) this.GetValue(ChartAxis.AxisLineOffsetProperty);
    set => this.SetValue(ChartAxis.AxisLineOffsetProperty, (object) value);
  }

  public AxisElementPosition LabelsPosition
  {
    get => (AxisElementPosition) this.GetValue(ChartAxis.LabelsPositionProperty);
    set => this.SetValue(ChartAxis.LabelsPositionProperty, (object) value);
  }

  public object LabelsSource
  {
    get => this.GetValue(ChartAxis.LabelsSourceProperty);
    set => this.SetValue(ChartAxis.LabelsSourceProperty, value);
  }

  public DataTemplate PostfixLabelTemplate
  {
    get => (DataTemplate) this.GetValue(ChartAxis.PostfixLabelTemplateProperty);
    set => this.SetValue(ChartAxis.PostfixLabelTemplateProperty, (object) value);
  }

  public DataTemplate PrefixLabelTemplate
  {
    get => (DataTemplate) this.GetValue(ChartAxis.PrefixLabelTemplateProperty);
    set => this.SetValue(ChartAxis.PrefixLabelTemplateProperty, (object) value);
  }

  public AxisLabelsIntersectAction LabelsIntersectAction
  {
    get => (AxisLabelsIntersectAction) this.GetValue(ChartAxis.LabelsIntersectActionProperty);
    set => this.SetValue(ChartAxis.LabelsIntersectActionProperty, (object) value);
  }

  public double LabelExtent
  {
    get => (double) this.GetValue(ChartAxis.LabelExtentProperty);
    set => this.SetValue(ChartAxis.LabelExtentProperty, (object) value);
  }

  public double LabelRotationAngle
  {
    get => (double) this.GetValue(ChartAxis.LabelRotationAngleProperty);
    set => this.SetValue(ChartAxis.LabelRotationAngleProperty, (object) value);
  }

  public Style AxisLineStyle
  {
    get => (Style) this.GetValue(ChartAxis.AxisLineStyleProperty);
    set => this.SetValue(ChartAxis.AxisLineStyleProperty, (object) value);
  }

  public bool OpposedPosition
  {
    get => (bool) this.GetValue(ChartAxis.OpposedPositionProperty);
    set => this.SetValue(ChartAxis.OpposedPositionProperty, (object) value);
  }

  public int? DesiredIntervalsCount
  {
    get => (int?) this.GetValue(ChartAxis.DesiredIntervalsCountProperty);
    set => this.SetValue(ChartAxis.DesiredIntervalsCountProperty, (object) value);
  }

  public Visibility ThumbLabelVisibility
  {
    get => (Visibility) this.GetValue(ChartAxis.ThumbLabelVisibilityProperty);
    set => this.SetValue(ChartAxis.ThumbLabelVisibilityProperty, (object) value);
  }

  public DataTemplate ThumbLabelTemplate
  {
    get => (DataTemplate) this.GetValue(ChartAxis.ThumbLabelTemplateProperty);
    set => this.SetValue(ChartAxis.ThumbLabelTemplateProperty, (object) value);
  }

  public object Header
  {
    get => this.GetValue(ChartAxis.HeaderProperty);
    set => this.SetValue(ChartAxis.HeaderProperty, value);
  }

  public LabelStyle HeaderStyle
  {
    get => (LabelStyle) this.GetValue(ChartAxis.HeaderStyleProperty);
    set => this.SetValue(ChartAxis.HeaderStyleProperty, (object) value);
  }

  public DataTemplate HeaderTemplate
  {
    get => (DataTemplate) this.GetValue(ChartAxis.HeaderTemplateProperty);
    set => this.SetValue(ChartAxis.HeaderTemplateProperty, (object) value);
  }

  public double TickLineSize
  {
    get => (double) this.GetValue(ChartAxis.TickLineSizeProperty);
    set => this.SetValue(ChartAxis.TickLineSizeProperty, (object) value);
  }

  public ObservableCollection<ChartAxisLabel> VisibleLabels => this.m_VisibleLabels;

  public bool IsInversed
  {
    get => (bool) this.GetValue(ChartAxis.IsInversedProperty);
    set => this.SetValue(ChartAxis.IsInversedProperty, (object) value);
  }

  public double Origin
  {
    get => (double) this.GetValue(ChartAxis.OriginProperty);
    set => this.SetValue(ChartAxis.OriginProperty, (object) value);
  }

  public bool ShowOrigin
  {
    get => (bool) this.GetValue(ChartAxis.ShowOriginProperty);
    set => this.SetValue(ChartAxis.ShowOriginProperty, (object) value);
  }

  public bool ShowAxisNextToOrigin
  {
    get => (bool) this.GetValue(ChartAxis.ShowAxisNextToOriginProperty);
    set => this.SetValue(ChartAxis.ShowAxisNextToOriginProperty, (object) value);
  }

  public Style OriginLineStyle
  {
    get => (Style) this.GetValue(ChartAxis.OriginLineStyleProperty);
    set => this.SetValue(ChartAxis.OriginLineStyleProperty, (object) value);
  }

  public AxisElementPosition TickLinesPosition
  {
    get => (AxisElementPosition) this.GetValue(ChartAxis.TickLinesPositionProperty);
    set => this.SetValue(ChartAxis.TickLinesPositionProperty, (object) value);
  }

  public EdgeLabelsDrawingMode EdgeLabelsDrawingMode
  {
    get => (EdgeLabelsDrawingMode) this.GetValue(ChartAxis.EdgeLabelsDrawingModeProperty);
    set => this.SetValue(ChartAxis.EdgeLabelsDrawingModeProperty, (object) value);
  }

  public EdgeLabelsVisibilityMode EdgeLabelsVisibilityMode
  {
    get => (EdgeLabelsVisibilityMode) this.GetValue(ChartAxis.EdgeLabelsVisibilityModeProperty);
    set => this.SetValue(ChartAxis.EdgeLabelsVisibilityModeProperty, (object) value);
  }

  public Style MajorGridLineStyle
  {
    get => (Style) this.GetValue(ChartAxis.MajorGridLineStyleProperty);
    set => this.SetValue(ChartAxis.MajorGridLineStyleProperty, (object) value);
  }

  public Style MinorGridLineStyle
  {
    get => (Style) this.GetValue(ChartAxis.MinorGridLineStyleProperty);
    set => this.SetValue(ChartAxis.MinorGridLineStyleProperty, (object) value);
  }

  public Style MajorTickLineStyle
  {
    get => (Style) this.GetValue(ChartAxis.MajorTickLineStyleProperty);
    set => this.SetValue(ChartAxis.MajorTickLineStyleProperty, (object) value);
  }

  public Style MinorTickLineStyle
  {
    get => (Style) this.GetValue(ChartAxis.MinorTickLineStyleProperty);
    set => this.SetValue(ChartAxis.MinorTickLineStyleProperty, (object) value);
  }

  public bool ShowTrackBallInfo
  {
    get => (bool) this.GetValue(ChartAxis.ShowTrackBallInfoProperty);
    set => this.SetValue(ChartAxis.ShowTrackBallInfoProperty, (object) value);
  }

  public DataTemplate TrackBallLabelTemplate
  {
    get => (DataTemplate) this.GetValue(ChartAxis.TrackBallLabelTemplateProperty);
    set => this.SetValue(ChartAxis.TrackBallLabelTemplateProperty, (object) value);
  }

  public DataTemplate CrosshairLabelTemplate
  {
    get => (DataTemplate) this.GetValue(ChartAxis.CrosshairLabelTemplateProperty);
    set => this.SetValue(ChartAxis.CrosshairLabelTemplateProperty, (object) value);
  }

  public bool ShowGridLines
  {
    get => (bool) this.GetValue(ChartAxis.ShowGridLinesProperty);
    set => this.SetValue(ChartAxis.ShowGridLinesProperty, (object) value);
  }

  public bool EnableAutoIntervalOnZooming
  {
    get => (bool) this.GetValue(ChartAxis.EnableAutoIntervalOnZoomingProperty);
    set => this.SetValue(ChartAxis.EnableAutoIntervalOnZoomingProperty, (object) value);
  }

  public LabelStyle LabelStyle
  {
    get => (LabelStyle) this.GetValue(ChartAxis.LabelStyleProperty);
    set => this.SetValue(ChartAxis.LabelStyleProperty, (object) value);
  }

  public DataTemplate LabelTemplate
  {
    get => (DataTemplate) this.GetValue(ChartAxis.LabelTemplateProperty);
    set => this.SetValue(ChartAxis.LabelTemplateProperty, (object) value);
  }

  public ChartAxisLabelCollection CustomLabels
  {
    get
    {
      if (this.m_customLabels == null)
      {
        this.m_customLabels = new ChartAxisLabelCollection();
        this.m_customLabels.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnCustomLabelsCollectionChanged);
      }
      return this.m_customLabels;
    }
  }

  internal Size ComputedDesiredSize { get; set; }

  internal double VisibleInterval { get; set; }

  internal double ActualInterval { get; set; }

  internal int AxisLabelCoefficientRoundOffValue { get; set; }

  internal bool IsRangeCalculating { get; set; }

  internal double ActualPlotOffsetStart
  {
    get => this.actualPlotOffsetStart;
    set => this.actualPlotOffsetStart = value;
  }

  internal double ActualPlotOffsetEnd
  {
    get => this.actualPlotOffsetEnd;
    set => this.actualPlotOffsetEnd = value;
  }

  internal DoubleRange ActualRange
  {
    get => this.m_actualRange;
    set => this.m_actualRange = value;
  }

  internal Rect RenderedRect
  {
    get => this.renderedRect;
    set
    {
      Rect renderedRect = this.renderedRect;
      this.renderedRect = value;
      this.OnAxisBoundsChanged(new ChartAxisBoundsEventArgs()
      {
        NewBounds = value,
        OldBounds = renderedRect
      });
    }
  }

  internal Orientation Orientation
  {
    get => (Orientation) this.GetValue(ChartAxis.OrientationProperty);
    set => this.SetValue(ChartAxis.OrientationProperty, (object) value);
  }

  public ChartAxisRangeStyleCollection RangeStyles
  {
    get => (ChartAxisRangeStyleCollection) this.GetValue(ChartAxis.RangeStylesProperty);
    set => this.SetValue(ChartAxis.RangeStylesProperty, (object) value);
  }

  internal ChartValueType ValueType
  {
    get => (ChartValueType) this.GetValue(ChartAxis.ValueTypeProperty);
    set => this.SetValue(ChartAxis.ValueTypeProperty, (object) value);
  }

  internal DataTemplate ActualTrackBallLabelTemplate
  {
    get => (DataTemplate) this.GetValue(ChartAxis.ActualTrackBallLabelTemplateProperty);
    set => this.SetValue(ChartAxis.ActualTrackBallLabelTemplateProperty, (object) value);
  }

  internal Visibility AxisVisibility
  {
    get => (Visibility) this.GetValue(ChartAxis.AxisVisibilityProperty);
    set => this.SetValue(ChartAxis.AxisVisibilityProperty, (object) value);
  }

  internal LabelAlignment AxisLabelAlignment
  {
    get => (LabelAlignment) this.GetValue(ChartAxis.AxisLabelAlignmentProperty);
    set => this.SetValue(ChartAxis.AxisLabelAlignmentProperty, (object) value);
  }

  internal ObservableCollection<ISupportAxes> RegisteredSeries
  {
    get
    {
      if (this.registeredSeries == null)
      {
        this.registeredSeries = new ObservableCollection<ISupportAxes>();
        this.associatedAxes = new List<ChartAxis>();
        this.registeredSeries.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnRegisteredSeriesCollectionChanged);
      }
      return this.registeredSeries;
    }
  }

  internal List<ChartAxis> AssociatedAxes
  {
    get => this.associatedAxes;
    set => this.associatedAxes = value;
  }

  internal bool IsLabelRotateRequired { get; set; }

  internal bool IsDefault { get; set; }

  internal bool IsScrolling { get; set; }

  internal double InsidePadding { get; set; }

  internal ChartBase Area { get; set; }

  internal List<double> SmallTickPoints => this.m_smalltickPoints;

  internal Size AvailableSize { get; set; }

  internal DoubleRange StriplineXRange { get; set; }

  internal DoubleRange StriplineYRange { get; set; }

  internal ChartAxis.ValueToCoefficientHandler ValueToCoefficientCalc { get; set; }

  internal ChartAxis.ValueToCoefficientHandler CoefficientToValueCalc { get; set; }

  public virtual double CoefficientToValue(double value)
  {
    value = this.IsInversed ? 1.0 - value : value;
    return this.VisibleRange.Start + this.VisibleRange.Delta * value;
  }

  public virtual double PolarCoefficientToValue(double value)
  {
    value = this.IsInversed ? value : 1.0 - value;
    if (this.VisibleLabels.Count > 0)
      value /= 1.0 - 1.0 / (double) this.VisibleLabels.Count;
    else
      value /= 1.0 - 1.0 / (this.VisibleRange.Delta + 1.0);
    return this.VisibleRange.Start + this.VisibleRange.Delta * value;
  }

  public virtual double CoefficientToActualValue(double value)
  {
    value = this.IsInversed ? 1.0 - value : value;
    return this.ActualRange.Start + this.ActualRange.Delta * value;
  }

  public virtual double ValueToCoefficient(double value)
  {
    double start = this.VisibleRange.Start;
    double delta = this.VisibleRange.Delta;
    if (delta == 0.0)
      return -1.0;
    double num = (value - start) / delta;
    return !this.IsActualInversed ? num : 1.0 - num;
  }

  public virtual double ValueToPolarCoefficient(double value)
  {
    double start = this.VisibleRange.Start;
    double delta = this.VisibleRange.Delta;
    double num1 = (value - start) / delta;
    double num2 = this.VisibleLabels.Count <= 0 ? num1 * (1.0 - 1.0 / (delta + 1.0)) : num1 * (1.0 - 1.0 / (double) this.VisibleLabels.Count);
    return !this.IsActualInversed ? 1.0 - num2 : num2;
  }

  public virtual double ValueToCoefficient(double value, bool isInversed)
  {
    double start = this.VisibleRange.Start;
    double delta = this.VisibleRange.Delta;
    double num = (value - start) / delta;
    return !isInversed ? num : 1.0 - num;
  }

  public virtual object GetLabelContent(double position)
  {
    return this.CustomLabels.Count > 0 || this.GetLabelSource() != null ? this.GetCustomLabelContent(position) ?? this.GetActualLabelContent(position) : this.GetActualLabelContent(position);
  }

  public Rect GetRenderedRect() => this.RenderedRect;

  public Rect GetArrangeRect() => this.ArrangeRect;

  public DependencyObject Clone() => this.CloneAxis((DependencyObject) null);

  internal static IEnumerable GetSourceList(object source)
  {
    IEnumerable sourceList = (IEnumerable) null;
    switch (source)
    {
      case null:
        return sourceList;
      case CollectionViewSource _:
        CollectionViewSource collectionViewSource = source as CollectionViewSource;
        if (collectionViewSource.View != null)
        {
          sourceList = ChartAxis.GetSourceList((object) collectionViewSource.View.Groups);
          goto case null;
        }
        goto case null;
      case ICollectionView _:
        sourceList = ChartAxis.GetSourceList((object) ((ICollectionView) source).Groups);
        goto case null;
      default:
        sourceList = source as IEnumerable;
        goto case null;
    }
  }

  internal virtual void ComputeDesiredSize(Size size)
  {
  }

  internal virtual void CreateLineRecycler()
  {
  }

  internal virtual object GetActualLabelContent(double position)
  {
    return (object) Math.Round(position, 11).ToString(this.LabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
  }

  internal virtual void Dispose()
  {
    this.DisposeCustomLabels();
    this.DisposeEvents();
    this.DisposeRegisteredSeries();
    this.DisposeVisibleLabels();
    this.AssociatedAxes.Clear();
    this.LabelStyle = (LabelStyle) null;
    this.Area = (ChartBase) null;
  }

  internal double PixelToCoefficientValue(double value)
  {
    double num = this.Orientation == Orientation.Horizontal ? this.renderedRect.Width : this.renderedRect.Height;
    return value * (this.VisibleRange.End - this.VisibleRange.Start) / num;
  }

  internal void OnLabelCreated(LabelCreatedEventArgs args)
  {
    if (this.LabelCreated == null || args == null)
      return;
    this.LabelCreated((object) this, args);
  }

  internal object GetCustomLabelContent(double position)
  {
    string customLabelContent = (string) null;
    foreach (ChartAxisLabel visibleLabel in (Collection<ChartAxisLabel>) this.VisibleLabels)
    {
      if (visibleLabel.Position == position)
        customLabelContent = visibleLabel.GetContent().ToString();
    }
    return (object) customLabelContent;
  }

  internal double GetActualPlotOffsetStart()
  {
    return this.ActualPlotOffset != 0.0 ? this.ActualPlotOffset : this.ActualPlotOffsetStart;
  }

  internal double GetActualPlotOffsetEnd()
  {
    return this.ActualPlotOffset != 0.0 ? this.ActualPlotOffset : this.ActualPlotOffsetEnd;
  }

  internal double GetActualPlotOffset()
  {
    return this.ActualPlotOffset != 0.0 ? this.ActualPlotOffset * 2.0 : this.ActualPlotOffsetStart + this.ActualPlotOffsetEnd;
  }

  internal void CalculateRangeAndInterval(Size availableSize)
  {
    bool flag = this is ChartAxisBase2D chartAxisBase2D && !(this is TimeSpanAxis) && !double.IsNaN(chartAxisBase2D.AutoScrollingDelta);
    if (!this.IsScrolling)
    {
      DoubleRange range = this.CalculateActualRange();
      if (range.IsEmpty)
      {
        range = new DoubleRange(0.0, 1.0);
        this.IsDefaultRange = true;
      }
      else
        this.IsDefaultRange = false;
      if (range.Start == range.End)
        range = new DoubleRange(range.Start, range.End + 1.0);
      this.ActualInterval = this.CalculateActualInterval(range, availableSize);
      this.ActualRange = this.ApplyRangePadding(range, this.ActualInterval);
      if (flag && chartAxisBase2D.AutoScrollingDelta >= 0.0 && chartAxisBase2D.CanAutoScroll)
      {
        chartAxisBase2D.UpdateAutoScrollingDelta(chartAxisBase2D.AutoScrollingDelta);
        chartAxisBase2D.CanAutoScroll = false;
      }
    }
    else if (flag)
      this.CalculateActualRange();
    if (this.ActualRangeChanged != null)
      this.ApplyCustomVisibleRange(availableSize);
    else
      this.CalculateVisibleRange(availableSize);
  }

  internal void Invalidate()
  {
    if (this.RegisteredSeries.Count <= 0 || !(this.RegisteredSeries[0] is PolarRadarSeriesBase))
      return;
    this.CalculateRangeAndInterval(this.AvailableSize);
    this.UpdateLabels();
  }

  internal void UpdateLabels()
  {
    if (this.MaximumLabels <= 0 || this.VisibleRange.Delta <= 0.0)
      return;
    this.previousLabelCoefficientValue = -1.0;
    this.VisibleLabels.Clear();
    this.m_smalltickPoints.Clear();
    if (this.CustomLabels.Count > 0)
      this.PopulateVisibleLabelForCustomLabels();
    else if (this.GetLabelSource() != null)
    {
      this.PopulateVisibleLabelsForLabelSource();
    }
    else
    {
      this.GenerateVisibleLabels();
      if (this.IsActualInversed)
      {
        if (this.m_VisibleLabels != null)
          this.m_VisibleLabels.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.m_VisibleLabels_CollectionChanged);
        this.m_VisibleLabels = new ObservableCollection<ChartAxisLabel>(this.VisibleLabels.Reverse<ChartAxisLabel>());
        this.m_VisibleLabels.CollectionChanged += new NotifyCollectionChangedEventHandler(this.m_VisibleLabels_CollectionChanged);
      }
    }
    if (this.axisLabelsPanel != null)
    {
      if (this.axisElementsPanel != null)
        this.axisElementsPanel.UpdateElements();
      this.axisLabelsPanel.UpdateElements();
      if (this.axisMultiLevelLabelsPanel == null)
        return;
      this.axisMultiLevelLabelsPanel.UpdateElements();
    }
    else
      this.axisElementsUpdateRequired = true;
  }

  internal double ValueToPoint(double value)
  {
    if (this.Area == null)
      return double.NaN;
    return this.Orientation == Orientation.Horizontal ? this.RenderedRect.Left + this.ValueToCoefficientCalc(value) * this.RenderedRect.Width : this.RenderedRect.Top + (1.0 - this.ValueToCoefficientCalc(value)) * this.RenderedRect.Height;
  }

  internal double PointToValue(Point point)
  {
    if (this.Area == null)
      return double.NaN;
    return this.Orientation == Orientation.Horizontal ? this.CoefficientToValueCalc((point.X - this.RenderedRect.Left) / this.RenderedRect.Width) : this.CoefficientToValueCalc(1.0 - (point.Y - this.RenderedRect.Top) / this.RenderedRect.Height);
  }

  internal void OnVisibleRangeChanged(object sender, VisibleRangeChangedEventArgs e)
  {
    if (this.registeredSeries == null)
      return;
    foreach (ISupportAxes supportAxes in (Collection<ISupportAxes>) this.registeredSeries)
    {
      if (supportAxes is CartesianSeries cartesianSeries)
        cartesianSeries.OnVisibleRangeChanged(sender, e);
    }
  }

  internal bool GetTrackballInfo() => this.ShowTrackBallInfo;

  internal DataTemplate GetTrackBallTemplate() => this.TrackBallLabelTemplate;

  internal AxisElementPosition GetLabelPosition() => this.LabelsPosition;

  internal object GetLabelSource() => this.LabelsSource;

  internal AxisLabelsIntersectAction GetLabelIntersection() => this.LabelsIntersectAction;

  protected internal virtual void OnAxisBoundsChanged(ChartAxisBoundsEventArgs args)
  {
    if (this.AxisBoundsChanged == null || args == null)
      return;
    this.AxisBoundsChanged((object) this, args);
  }

  protected internal virtual void OnAxisVisibleRangeChanged(VisibleRangeChangedEventArgs args)
  {
    if (this.VisibleRangeChanged == null || args == null)
      return;
    this.VisibleRangeChanged((object) this, args);
  }

  protected internal virtual void AddSmallTicksPoint(double position)
  {
  }

  protected internal virtual void AddSmallTicksPoint(double position, double interval)
  {
  }

  protected internal virtual double CalculateActualInterval(DoubleRange range, Size availableSize)
  {
    return 1.0;
  }

  protected internal virtual double CalculateNiceInterval(
    DoubleRange actualRange,
    Size availableSize)
  {
    double delta = actualRange.Delta;
    double desiredIntervalsCount = this.GetActualDesiredIntervalsCount(availableSize);
    double d = delta / desiredIntervalsCount;
    if (this.DesiredIntervalsCount.HasValue)
      return d;
    double num1 = Math.Pow(10.0, Math.Floor(Math.Log10(d)));
    foreach (int cIntervalDiv in ChartAxis.c_intervalDivs)
    {
      double num2 = num1 * (double) cIntervalDiv;
      if (desiredIntervalsCount >= delta / num2)
        d = num2;
      else
        break;
    }
    if (d <= 10.0 && actualRange.Start < 0.0 && this.RegisteredSeries.Count > 0 && this.RegisteredSeries.All<ISupportAxes>((Func<ISupportAxes, bool>) (series => series is ChartSeriesBase && (series as ChartSeriesBase).IsStacked100)))
      d = 20.0;
    return d;
  }

  protected internal virtual void CalculateVisibleRange(Size availableSize)
  {
    this.VisibleRange = this.ActualRange;
    this.VisibleInterval = this.ActualInterval;
  }

  protected internal double GetActualDesiredIntervalsCount(Size availableSize)
  {
    double num1 = this.Orientation == Orientation.Horizontal ? availableSize.Width : availableSize.Height;
    double desiredIntervalsCount = this.DesiredIntervalsCount.HasValue ? (double) this.DesiredIntervalsCount.Value : 0.0;
    if (!this.DesiredIntervalsCount.HasValue)
    {
      double num2 = (this.Orientation == Orientation.Horizontal ? 0.54 : 1.0) * (double) this.MaximumLabels;
      desiredIntervalsCount = Math.Max(num1 * num2 / this.MaxPixelsCount, 1.0);
    }
    return desiredIntervalsCount;
  }

  protected virtual void OnPropertyChanged()
  {
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  protected virtual void GenerateVisibleLabels()
  {
    double visibleInterval = this.VisibleInterval;
    for (double num = this.VisibleRange.Start - this.VisibleRange.Start % this.ActualInterval; num <= this.VisibleRange.End; num += visibleInterval)
    {
      if (this.VisibleRange.Inside(num))
        this.VisibleLabels.Add(new ChartAxisLabel(num, this.GetActualLabelContent(num), num));
    }
  }

  internal void SetRangeForAxisStyle()
  {
    if (this.RangeStyles == null || this.RangeStyles.Count <= 0)
      return;
    foreach (ChartAxisRangeStyle rangeStyle in (Collection<ChartAxisRangeStyle>) this.RangeStyles)
    {
      if (rangeStyle.Start == null && rangeStyle.End == null)
      {
        rangeStyle.Range = DoubleRange.Empty;
      }
      else
      {
        double start = rangeStyle.Start == null ? this.VisibleRange.Start : ChartDataUtils.ObjectToDouble(rangeStyle.Start);
        double end = rangeStyle.End == null ? this.VisibleRange.End : ChartDataUtils.ObjectToDouble(rangeStyle.End);
        rangeStyle.Range = new DoubleRange(start, end);
      }
    }
  }

  protected virtual DoubleRange CalculateActualRange()
  {
    if (this.Area == null)
      return DoubleRange.Empty;
    List<ChartSeriesBase> second = new List<ChartSeriesBase>();
    if (this.Area is SfChart && (this.Area as SfChart).TechnicalIndicators != null)
    {
      foreach (ChartSeries technicalIndicator in (Collection<ChartSeries>) (this.Area as SfChart).TechnicalIndicators)
        second.Add((ChartSeriesBase) technicalIndicator);
    }
    DoubleRange actualRange = (this.Area is SfChart ? this.Area.VisibleSeries.Union<ChartSeriesBase>((IEnumerable<ChartSeriesBase>) second).OfType<ISupportAxes>() : this.Area.VisibleSeries.OfType<ISupportAxes>()).Select<ISupportAxes, DoubleRange>((Func<ISupportAxes, DoubleRange>) (series =>
    {
      if (series.ActualXAxis == this)
        return series.XRange;
      if (series.ActualYAxis == this)
        return series.YRange;
      return series is XyzDataSeries3D xyzDataSeries3D2 && xyzDataSeries3D2.ActualZAxis == this ? xyzDataSeries3D2.ZRange : DoubleRange.Empty;
    })).Sum();
    if (this is ChartAxisBase2D chartAxisBase2D && chartAxisBase2D.IncludeStripLineRange)
    {
      if (this.Orientation == Orientation.Horizontal)
        return this.StriplineXRange + actualRange;
      if (this.Orientation == Orientation.Vertical)
        return this.StriplineYRange + actualRange;
    }
    return actualRange;
  }

  protected virtual DoubleRange ApplyRangePadding(DoubleRange range, double interval)
  {
    return this.RegisteredSeries.Count > 0 && this.RegisteredSeries[0] is PolarRadarSeriesBase ? new DoubleRange(Math.Floor(range.Start / interval) * interval, Math.Ceiling(range.End / interval) * interval) : range;
  }

  protected virtual DependencyObject CloneAxis(DependencyObject obj)
  {
    ChartAxis copy = obj as ChartAxis;
    ChartCloning.CloneControl((Control) this, (Control) copy);
    ChartBase.SetRow((UIElement) copy, ChartBase.GetRow((UIElement) this));
    ChartBase.SetColumn((UIElement) copy, ChartBase.GetRow((UIElement) this));
    copy.ContentPath = this.ContentPath;
    copy.Header = this.Header;
    copy.HeaderTemplate = this.HeaderTemplate;
    copy.HeaderStyle = this.HeaderStyle;
    copy.HeaderPosition = this.HeaderPosition;
    copy.IsInversed = this.IsInversed;
    copy.LabelExtent = this.LabelExtent;
    copy.LabelFormat = this.LabelFormat;
    copy.LabelRotationAngle = this.LabelRotationAngle;
    copy.LabelsIntersectAction = this.LabelsIntersectAction;
    copy.LabelsPosition = this.LabelsPosition;
    copy.LabelsSource = this.LabelsSource;
    copy.LabelTemplate = this.LabelTemplate;
    copy.LabelStyle = this.LabelStyle;
    copy.MajorGridLineStyle = this.MajorGridLineStyle;
    copy.MajorTickLineStyle = this.MajorTickLineStyle;
    copy.MinorGridLineStyle = this.MinorGridLineStyle;
    copy.MinorTickLineStyle = this.MinorTickLineStyle;
    copy.OriginLineStyle = this.OriginLineStyle;
    copy.MaximumLabels = this.MaximumLabels;
    copy.OpposedPosition = this.OpposedPosition;
    copy.PositionPath = this.PositionPath;
    copy.PostfixLabelTemplate = this.PostfixLabelTemplate;
    copy.PrefixLabelTemplate = this.PrefixLabelTemplate;
    copy.ShowAxisNextToOrigin = this.ShowAxisNextToOrigin;
    copy.ShowGridLines = this.ShowGridLines;
    copy.ShowOrigin = this.ShowOrigin;
    copy.TickLineSize = this.TickLineSize;
    copy.EnableAutoIntervalOnZooming = this.EnableAutoIntervalOnZooming;
    copy.ShowTrackBallInfo = this.ShowTrackBallInfo;
    copy.TrackBallLabelTemplate = this.TrackBallLabelTemplate;
    copy.ActualTrackBallLabelTemplate = this.ActualTrackBallLabelTemplate;
    copy.CrosshairLabelTemplate = this.CrosshairLabelTemplate;
    copy.EdgeLabelsDrawingMode = this.EdgeLabelsDrawingMode;
    copy.EdgeLabelsVisibilityMode = this.EdgeLabelsVisibilityMode;
    copy.TickLinesPosition = this.TickLinesPosition;
    copy.Origin = this.Origin;
    copy.DesiredIntervalsCount = this.DesiredIntervalsCount;
    copy.Orientation = this.Orientation;
    copy.AxisLineStyle = this.AxisLineStyle;
    copy.AxisLineOffset = this.AxisLineOffset;
    copy.HeaderPosition = this.HeaderPosition;
    copy.ThumbLabelTemplate = this.ThumbLabelTemplate;
    copy.ThumbLabelVisibility = this.ThumbLabelVisibility;
    return (DependencyObject) copy;
  }

  protected virtual void OnRegisteredSeriesCollectionChanged(
    object sender,
    NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == NotifyCollectionChangedAction.Add)
    {
      if (!(e.NewItems[0] is ISupportAxes newItem))
        return;
      ChartAxis actualYaxis = newItem.ActualYAxis;
      ChartAxis actualXaxis = newItem.ActualXAxis;
      if (actualXaxis != null && actualYaxis != null)
      {
        if (actualXaxis.associatedAxes != null && !actualXaxis.associatedAxes.Contains(actualYaxis))
        {
          this.CheckAxes(actualXaxis);
          actualXaxis.associatedAxes.Add(actualYaxis);
        }
        if (actualYaxis.associatedAxes != null && !actualYaxis.associatedAxes.Contains(actualXaxis))
        {
          this.CheckAxes(actualYaxis);
          actualYaxis.associatedAxes.Add(actualXaxis);
        }
        if (this is ChartAxisBase3D chartAxisBase3D && chartAxisBase3D.IsZAxis)
        {
          ChartAxis chartAxis = this;
          if (chartAxis.associatedAxes != null && !chartAxis.associatedAxes.Contains(actualYaxis))
          {
            this.CheckAxes(chartAxis);
            if (actualXaxis.Orientation == Orientation.Vertical)
              chartAxis.associatedAxes.Add(actualXaxis);
            else
              chartAxis.associatedAxes.Add(actualYaxis);
          }
        }
      }
      this.ScheduleCheck();
    }
    else if (e.Action == NotifyCollectionChangedAction.Remove)
    {
      if (!(e.OldItems[0] is ISupportAxes oldItem))
        return;
      ChartAxis actualYaxis = oldItem.ActualYAxis;
      ChartAxis actualXaxis = oldItem.ActualXAxis;
      if (this.Area == null || actualXaxis == null || actualYaxis == null || actualXaxis.associatedAxes == null || actualYaxis.associatedAxes == null)
        return;
      if (this.Area is SfChart3D || oldItem is FinancialTechnicalIndicator || actualXaxis.Equals((object) this.Area.InternalPrimaryAxis) || actualYaxis.Equals((object) this.Area.InternalSecondaryAxis))
      {
        if (!this.Area.Axes.Contains(actualYaxis))
          actualXaxis.associatedAxes.Remove(actualYaxis);
        if (this.Area.Axes.Contains(actualXaxis))
          return;
        actualYaxis.associatedAxes.Remove(actualXaxis);
      }
      else
      {
        actualXaxis.associatedAxes.Remove(actualYaxis);
        actualYaxis.associatedAxes.Remove(actualXaxis);
      }
    }
    else
    {
      if (e.Action != NotifyCollectionChangedAction.Reset)
        return;
      if (this.associatedAxes != null)
        this.associatedAxes.Clear();
      if (this.Area == null)
        return;
      if (this.Area is SfChart)
      {
        ChartSeriesCollection series = (this.Area as SfChart).Series;
        ObservableCollection<ChartSeries> technicalIndicators = (this.Area as SfChart).TechnicalIndicators;
        if (series == null || series.Count == 0)
        {
          if (this.Area.InternalPrimaryAxis != null && this.Area.InternalPrimaryAxis.AssociatedAxes != null)
            this.Area.InternalPrimaryAxis.AssociatedAxes.Clear();
          if (this.Area.InternalSecondaryAxis != null && this.Area.InternalSecondaryAxis.AssociatedAxes != null)
            this.Area.InternalSecondaryAxis.AssociatedAxes.Clear();
        }
        else
        {
          foreach (ChartSeries chartSeries in (Collection<ChartSeries>) series)
            this.RemoveAssociatedAxis(chartSeries.ActualXAxis, chartSeries.ActualYAxis);
        }
        if (technicalIndicators == null)
          return;
        foreach (ChartSeries chartSeries in (Collection<ChartSeries>) technicalIndicators)
          this.RemoveAssociatedAxis(chartSeries.ActualXAxis, chartSeries.ActualYAxis);
      }
      else
      {
        ChartSeries3DCollection series = (this.Area as SfChart3D).Series;
        if (series == null)
          return;
        foreach (ChartSeries3D chartSeries3D in (Collection<ChartSeries3D>) series)
          this.RemoveAssociatedAxis(chartSeries3D.ActualXAxis, chartSeries3D.ActualYAxis);
      }
    }
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    if (this.Area != null && this.Area.VisibleSeries.Count > 0 && this.Area.VisibleSeries[0] is AccumulationSeriesBase)
      return new Size(0.0, 0.0);
    base.MeasureOverride(availableSize);
    return new Size(this.ArrangeRect.Width, this.ArrangeRect.Height);
  }

  protected override AutomationPeer OnCreateAutomationPeer()
  {
    return (AutomationPeer) new ChartAxisAutomationPeer(this);
  }

  private static void OnHeaderPositionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ChartAxis chartAxis) || chartAxis.Area == null)
      return;
    chartAxis.Area.ScheduleUpdate();
  }

  private static void OnAxisOrientationChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartAxis chartAxis = d as ChartAxis;
    chartAxis.ScheduleCheck();
    chartAxis.OnPropertyChanged();
  }

  private static void RangeStylesPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAxis).RangeStylesChanged(e);
  }

  internal void RangeStylesChanged(DependencyPropertyChangedEventArgs args)
  {
    ChartAxisRangeStyleCollection oldValue = args.OldValue as ChartAxisRangeStyleCollection;
    ChartAxisRangeStyleCollection newValue = args.NewValue as ChartAxisRangeStyleCollection;
    if (oldValue != null)
      oldValue.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.RangeStyles_CollectionChanged);
    if (newValue != null)
      newValue.CollectionChanged += new NotifyCollectionChangedEventHandler(this.RangeStyles_CollectionChanged);
    this.OnPropertyChanged();
  }

  private void RangeStyles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    this.OnPropertyChanged();
  }

  private static void OnOpposedPositionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (d is ChartAxisBase2D chartAxisBase2D)
      chartAxisBase2D.ChangeStyle(chartAxisBase2D.EnableTouchMode);
    (d as ChartAxis).OnPropertyChanged();
  }

  private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAxis).OnPropertyChanged();
  }

  private static void OnIsInversedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAxis).OnIsInversedChanged(e);
  }

  private static void OnShowGridLinePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAxis).OnShowGridLines((bool) e.NewValue);
  }

  private static void OnShowAxisNextToOriginChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ChartAxis chartAxis) || chartAxis.Area == null)
      return;
    chartAxis.Area.ScheduleUpdate();
  }

  private static void OnLabelTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartAxis chartAxis = d as ChartAxis;
    if (chartAxis.Area == null)
      return;
    chartAxis.Area.ScheduleUpdate();
  }

  private static void OnLabelRotationChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartAxis chartAxis = d as ChartAxis;
    chartAxis.IsLabelRotateRequired = true;
    if (chartAxis.Area == null)
      return;
    chartAxis.Area.ScheduleUpdate();
  }

  private static void OnValuetypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ChartAxis chartAxis = d as ChartAxis;
    if (chartAxis.ValueType != ChartValueType.Logarithmic)
      return;
    chartAxis.IsLogarithmic = true;
  }

  private static void OnDesiredIntervalsCountPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartAxis chartAxis = d as ChartAxis;
    if (chartAxis.Area == null)
      return;
    chartAxis.Area.ScheduleUpdate();
  }

  private void DisposeVisibleLabels()
  {
    if (this.m_VisibleLabels == null)
      return;
    foreach (ChartAxisLabel visibleLabel in (Collection<ChartAxisLabel>) this.m_VisibleLabels)
    {
      visibleLabel.ChartAxis = (ChartAxis) null;
      visibleLabel.LabelContent = (object) null;
      visibleLabel.LabelStyle = (object) null;
      visibleLabel.PrefixLabelTemplate = (DataTemplate) null;
      visibleLabel.PostfixLabelTemplate = (DataTemplate) null;
    }
    this.m_VisibleLabels.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.m_VisibleLabels_CollectionChanged);
    this.m_VisibleLabels.Clear();
    this.m_VisibleLabels = (ObservableCollection<ChartAxisLabel>) null;
  }

  private void DisposeEvents()
  {
    if (this.VisibleRangeChanged != null)
    {
      foreach (Delegate invocation in this.VisibleRangeChanged.GetInvocationList())
        this.VisibleRangeChanged -= invocation as EventHandler<VisibleRangeChangedEventArgs>;
      this.VisibleRangeChanged = (EventHandler<VisibleRangeChangedEventArgs>) null;
    }
    if (this.LabelCreated != null)
    {
      foreach (Delegate invocation in this.LabelCreated.GetInvocationList())
        this.LabelCreated -= invocation as EventHandler<LabelCreatedEventArgs>;
      this.LabelCreated = (EventHandler<LabelCreatedEventArgs>) null;
    }
    if (this.AxisBoundsChanged != null)
    {
      foreach (Delegate invocation in this.AxisBoundsChanged.GetInvocationList())
        this.AxisBoundsChanged -= invocation as EventHandler<ChartAxisBoundsEventArgs>;
      this.AxisBoundsChanged = (EventHandler<ChartAxisBoundsEventArgs>) null;
    }
    if (this.ActualRangeChanged == null)
      return;
    foreach (Delegate invocation in this.ActualRangeChanged.GetInvocationList())
      this.ActualRangeChanged -= invocation as EventHandler<ActualRangeChangedEventArgs>;
    this.ActualRangeChanged = (EventHandler<ActualRangeChangedEventArgs>) null;
  }

  private void DisposeCustomLabels()
  {
    if (this.m_customLabels == null)
      return;
    this.m_customLabels.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnCustomLabelsCollectionChanged);
    this.m_customLabels.Clear();
  }

  private void DisposeRegisteredSeries()
  {
    if (this.registeredSeries == null)
      return;
    this.registeredSeries.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnRegisteredSeriesCollectionChanged);
    this.registeredSeries.Clear();
  }

  private void OnIsInversedChanged(DependencyPropertyChangedEventArgs e)
  {
    this.IsActualInversed = this.IsInversed;
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  private void OnShowGridLines(bool value)
  {
    if (this.Area == null || this.Area.GridLinesLayout == null)
      return;
    if (this is ChartAxisBase3D)
    {
      if (!value && this.GridLinesRecycler != null)
      {
        for (int index = 0; index < this.GridLinesRecycler.Count; ++index)
          this.GridLinesRecycler[index].Visibility = Visibility.Collapsed;
      }
      else
      {
        if (!value || this.VisibleLabels.Count <= 0)
          return;
        for (int index = 0; index < this.GridLinesRecycler.Count; ++index)
          this.GridLinesRecycler[index].Visibility = Visibility.Visible;
      }
    }
    else if (!value && this.GridLinesRecycler != null)
    {
      this.GridLinesRecycler.Clear();
    }
    else
    {
      if (!value || this.VisibleLabels.Count <= 0 || double.IsInfinity(this.Area.AvailableSize.Height) || double.IsInfinity(this.Area.AvailableSize.Width))
        return;
      this.Area.GridLinesLayout.UpdateElements();
      this.Area.GridLinesLayout.Measure(this.Area.AvailableSize);
      this.Area.GridLinesLayout.Arrange(this.Area.AvailableSize);
    }
  }

  private void ApplyCustomVisibleRange(Size availableSize)
  {
    ActualRangeChangedEventArgs e = new ActualRangeChangedEventArgs(this)
    {
      IsScrolling = this.IsScrolling,
      ActualMinimum = (object) this.ActualRange.Start,
      ActualMaximum = (object) this.ActualRange.End,
      ActualInterval = this.ActualInterval
    };
    this.ActualRangeChanged((object) this, e);
    DoubleRange actualRange = e.GetActualRange();
    if (actualRange != this.ActualRange)
    {
      this.ActualRange = actualRange;
      this.ActualInterval = this.CalculateActualInterval(actualRange, availableSize);
    }
    DoubleRange visibleRange = e.GetVisibleRange();
    if (visibleRange.IsEmpty)
    {
      this.CalculateVisibleRange(availableSize);
    }
    else
    {
      this.VisibleRange = visibleRange;
      this.VisibleInterval = this.EnableAutoIntervalOnZooming ? this.CalculateNiceInterval(this.VisibleRange, availableSize) : this.ActualInterval;
      if (!(this is ChartAxisBase2D chartAxisBase2D))
        return;
      chartAxisBase2D.ZoomPosition = (this.VisibleRange.Start - this.ActualRange.Start) / this.ActualRange.Delta;
      chartAxisBase2D.ZoomFactor = (this.VisibleRange.End - this.VisibleRange.Start) / this.ActualRange.Delta;
    }
  }

  private void RaiseLabelCreated(ChartAxisLabel axisLabel)
  {
    this.OnLabelCreated(new LabelCreatedEventArgs()
    {
      AxisLabel = axisLabel
    });
  }

  private void PopulateVisibleLabelsForLabelSource()
  {
    foreach (object source in ChartAxis.GetSourceList(this.LabelsSource))
    {
      double positionalPathValue = (double) (int) ChartDataUtils.GetPositionalPathValue(source, this.PositionPath);
      if (this.VisibleRange.Inside(positionalPathValue))
      {
        if (string.IsNullOrEmpty(this.ContentPath))
        {
          switch (this)
          {
            case DateTimeAxis _:
            case DateTimeCategoryAxis _:
              this.VisibleLabels.Add((ChartAxisLabel) new DateTimeAxisLabel(positionalPathValue, source, positionalPathValue));
              continue;
            default:
              this.VisibleLabels.Add(new ChartAxisLabel(positionalPathValue, source, positionalPathValue));
              continue;
          }
        }
        else
        {
          switch (this)
          {
            case DateTimeAxis _:
            case DateTimeCategoryAxis _:
              this.VisibleLabels.Add((ChartAxisLabel) new DateTimeAxisLabel(positionalPathValue, ChartDataUtils.GetObjectByPath(source, this.ContentPath), positionalPathValue));
              continue;
            default:
              this.VisibleLabels.Add(new ChartAxisLabel(positionalPathValue, ChartDataUtils.GetObjectByPath(source, this.ContentPath), positionalPathValue));
              continue;
          }
        }
      }
    }
  }

  private void PopulateVisibleLabelForCustomLabels()
  {
    this.GenerateVisibleLabels();
    Dictionary<double, ChartAxisLabel> dictionary = this.VisibleLabels.ToDictionary<ChartAxisLabel, double>((Func<ChartAxisLabel, double>) (label => label.Position));
    this.VisibleLabels.Clear();
    foreach (ChartAxisLabel chartAxisLabel in this.m_customLabels.Where<ChartAxisLabel>((Func<ChartAxisLabel, bool>) (label => this.VisibleRange.Inside(label.Position))))
      dictionary[chartAxisLabel.Position] = new ChartAxisLabel()
      {
        Position = chartAxisLabel.Position,
        LabelContent = chartAxisLabel.LabelContent
      };
    foreach (ChartAxisLabel chartAxisLabel in dictionary.Values)
      this.VisibleLabels.Add(chartAxisLabel);
  }

  private void m_VisibleLabels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.Action != NotifyCollectionChangedAction.Add)
      return;
    ChartAxisLabel newItem = e.NewItems[0] as ChartAxisLabel;
    double num = this.ValueToCoefficientCalc(newItem.Position);
    if (object.Equals((object) Math.Round(this.previousLabelCoefficientValue, this.AxisLabelCoefficientRoundOffValue), (object) Math.Round(num, this.AxisLabelCoefficientRoundOffValue)))
    {
      this.VisibleLabels.Remove(newItem);
    }
    else
    {
      this.previousLabelCoefficientValue = num;
      newItem.ChartAxis = this;
      if (this.LabelCreated == null)
        return;
      this.RaiseLabelCreated(newItem);
    }
  }

  private void RemoveAssociatedAxis(ChartAxis xAxis, ChartAxis yAxis)
  {
    if (xAxis == null || yAxis == null)
      return;
    if (yAxis.associatedAxes != null && yAxis.AssociatedAxes.Contains(this))
      yAxis.AssociatedAxes.Remove(this);
    if (xAxis.associatedAxes == null || !xAxis.AssociatedAxes.Contains(this))
      return;
    xAxis.AssociatedAxes.Remove(this);
  }

  private void CheckAxes(ChartAxis chartAxis)
  {
    if (this.Area == null)
      return;
    foreach (ChartAxis chartAxis1 in chartAxis.associatedAxes.ToList<ChartAxis>())
    {
      if (!this.Area.Axes.Contains(chartAxis1))
        chartAxis.AssociatedAxes.Remove(chartAxis1);
    }
  }

  private void ScheduleCheck()
  {
    if (this.isChecked)
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) new Action(this.CheckRegisterSeries));
    this.isChecked = true;
  }

  private void CheckRegisterSeries()
  {
    foreach (ChartSeriesBase chartSeriesBase in (Collection<ISupportAxes>) this.RegisteredSeries)
    {
      if ((this.RegisteredSeries[0] as ChartSeriesBase).IsActualTransposed != chartSeriesBase.IsActualTransposed)
        throw new InvalidOperationException($"{(object) this.RegisteredSeries[0].GetType()} {ChartLocalizationResourceAccessor.Instance.GetString("AxisIncompatibleExceptionMessage")} {(object) chartSeriesBase.GetType()}");
    }
    this.isChecked = false;
  }

  private void OnCustomLabelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    this.UpdateLabels();
  }

  internal delegate double ValueToCoefficientHandler(double value);
}
