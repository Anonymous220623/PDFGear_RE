// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartLegend
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartLegend : ItemsControl, ICloneable
{
  public static readonly DependencyProperty DockPositionProperty = DependencyProperty.Register(nameof (DockPosition), typeof (ChartDock), typeof (ChartLegend), new PropertyMetadata((object) ChartDock.Top, new PropertyChangedCallback(ChartLegend.OnDockPositionChanged)));
  public static readonly DependencyProperty OffsetXProperty = DependencyProperty.Register(nameof (OffsetX), typeof (double), typeof (ChartLegend), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ChartLegend.OnOffsetValueChanged)));
  public static readonly DependencyProperty OffsetYProperty = DependencyProperty.Register(nameof (OffsetY), typeof (double), typeof (ChartLegend), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ChartLegend.OnOffsetValueChanged)));
  public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(nameof (Series), typeof (ChartSeriesBase), typeof (ChartLegend), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartLegend.OnSeriesPropertyChanged)));
  public static readonly DependencyProperty LegendPositionProperty = DependencyProperty.Register(nameof (LegendPosition), typeof (LegendPosition), typeof (ChartLegend), new PropertyMetadata((object) LegendPosition.Outside, new PropertyChangedCallback(ChartLegend.OnLegendPositionChanged)));
  public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (ChartLegend), (PropertyMetadata) null);
  public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (ChartLegend), (PropertyMetadata) null);
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (ChartOrientation), typeof (ChartLegend), new PropertyMetadata((object) ChartOrientation.Default, new PropertyChangedCallback(ChartLegend.OnOrientationChanged)));
  public static DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (CornerRadius), typeof (ChartLegend), new PropertyMetadata((object) new CornerRadius(0.0)));
  public static DependencyProperty CheckBoxVisibilityProperty = DependencyProperty.Register(nameof (CheckBoxVisibility), typeof (Visibility), typeof (ChartLegend), new PropertyMetadata((object) Visibility.Collapsed));
  public static DependencyProperty ToggleSeriesVisibilityProperty = DependencyProperty.Register(nameof (ToggleSeriesVisibility), typeof (bool), typeof (ChartLegend), new PropertyMetadata((object) false));
  public static DependencyProperty IconVisibilityProperty = DependencyProperty.Register(nameof (IconVisibility), typeof (Visibility), typeof (ChartLegend), new PropertyMetadata((object) Visibility.Visible));
  public static DependencyProperty IconWidthProperty = DependencyProperty.Register(nameof (IconWidth), typeof (double), typeof (ChartLegend), new PropertyMetadata((object) 8.0));
  public static DependencyProperty IconHeightProperty = DependencyProperty.Register(nameof (IconHeight), typeof (double), typeof (ChartLegend), new PropertyMetadata((object) 8.0));
  public static DependencyProperty ItemMarginProperty = DependencyProperty.RegisterAttached(nameof (ItemMargin), typeof (Thickness), typeof (ChartLegend), new PropertyMetadata((object) new Thickness().GetThickness(0.0, 0.0, 0.0, 0.0)));
  internal bool isSegmentsUpdated;
  private ChartDock internalDockPosition = ChartDock.Top;
  private bool isLayoutUpdated;

  public ChartLegend()
  {
    this.DefaultStyleKey = (object) typeof (ChartLegend);
    this.Loaded += new RoutedEventHandler(this.ChartLegend_Loaded);
    this.LayoutUpdated += new EventHandler(this.OnChartLegendLayoutUpdated);
  }

  public ChartOrientation Orientation
  {
    get => (ChartOrientation) this.GetValue(ChartLegend.OrientationProperty);
    set => this.SetValue(ChartLegend.OrientationProperty, (object) value);
  }

  public ChartDock DockPosition
  {
    get => (ChartDock) this.GetValue(ChartLegend.DockPositionProperty);
    set => this.SetValue(ChartLegend.DockPositionProperty, (object) value);
  }

  public LegendPosition LegendPosition
  {
    get => (LegendPosition) this.GetValue(ChartLegend.LegendPositionProperty);
    set => this.SetValue(ChartLegend.LegendPositionProperty, (object) value);
  }

  public Thickness ItemMargin
  {
    get => (Thickness) this.GetValue(ChartLegend.ItemMarginProperty);
    set => this.SetValue(ChartLegend.ItemMarginProperty, (object) value);
  }

  public object Header
  {
    get => this.GetValue(ChartLegend.HeaderProperty);
    set => this.SetValue(ChartLegend.HeaderProperty, value);
  }

  public DataTemplate HeaderTemplate
  {
    get => (DataTemplate) this.GetValue(ChartLegend.HeaderTemplateProperty);
    set => this.SetValue(ChartLegend.HeaderTemplateProperty, (object) value);
  }

  public CornerRadius CornerRadius
  {
    get => (CornerRadius) this.GetValue(ChartLegend.CornerRadiusProperty);
    set => this.SetValue(ChartLegend.CornerRadiusProperty, (object) value);
  }

  public Visibility CheckBoxVisibility
  {
    get => (Visibility) this.GetValue(ChartLegend.CheckBoxVisibilityProperty);
    set => this.SetValue(ChartLegend.CheckBoxVisibilityProperty, (object) value);
  }

  public bool ToggleSeriesVisibility
  {
    get => (bool) this.GetValue(ChartLegend.ToggleSeriesVisibilityProperty);
    set => this.SetValue(ChartLegend.ToggleSeriesVisibilityProperty, (object) value);
  }

  public Visibility IconVisibility
  {
    get => (Visibility) this.GetValue(ChartLegend.IconVisibilityProperty);
    set => this.SetValue(ChartLegend.IconVisibilityProperty, (object) value);
  }

  public double IconWidth
  {
    get => (double) this.GetValue(ChartLegend.IconWidthProperty);
    set => this.SetValue(ChartLegend.IconWidthProperty, (object) value);
  }

  public double IconHeight
  {
    get => (double) this.GetValue(ChartLegend.IconHeightProperty);
    set => this.SetValue(ChartLegend.IconHeightProperty, (object) value);
  }

  public double OffsetX
  {
    get => (double) this.GetValue(ChartLegend.OffsetXProperty);
    set => this.SetValue(ChartLegend.OffsetXProperty, (object) value);
  }

  public ChartSeriesBase Series
  {
    get => (ChartSeriesBase) this.GetValue(ChartLegend.SeriesProperty);
    set => this.SetValue(ChartLegend.SeriesProperty, (object) value);
  }

  public double OffsetY
  {
    get => (double) this.GetValue(ChartLegend.OffsetYProperty);
    set => this.SetValue(ChartLegend.OffsetYProperty, (object) value);
  }

  internal ChartAxis XAxis { get; set; }

  internal ChartAxis YAxis { get; set; }

  internal ChartBase ChartArea { get; set; }

  internal int RowColumnIndex { get; set; }

  internal Rect ArrangeRect { get; set; }

  internal ChartDock InternalDockPosition
  {
    get => this.internalDockPosition;
    set => this.internalDockPosition = value;
  }

  public DependencyObject Clone()
  {
    ChartLegend chartLegend = new ChartLegend();
    chartLegend.CheckBoxVisibility = this.CheckBoxVisibility;
    chartLegend.CornerRadius = this.CornerRadius;
    chartLegend.DockPosition = this.DockPosition;
    chartLegend.LegendPosition = this.LegendPosition;
    chartLegend.Header = this.Header;
    chartLegend.HeaderTemplate = this.HeaderTemplate;
    chartLegend.IconHeight = this.IconHeight;
    chartLegend.IconVisibility = this.IconVisibility;
    chartLegend.IconWidth = this.IconWidth;
    chartLegend.ItemMargin = this.ItemMargin;
    chartLegend.OffsetX = this.OffsetX;
    chartLegend.OffsetY = this.OffsetY;
    chartLegend.Orientation = this.Orientation;
    chartLegend.ItemTemplate = this.ItemTemplate;
    chartLegend.ItemsPanel = this.ItemsPanel;
    chartLegend.ToggleSeriesVisibility = this.ToggleSeriesVisibility;
    ChartLegend copy = chartLegend;
    ChartCloning.CloneControl((Control) this, (Control) copy);
    return (DependencyObject) copy;
  }

  internal void Dispose()
  {
    this.XAxis = (ChartAxis) null;
    this.YAxis = (ChartAxis) null;
    this.ChartArea = (ChartBase) null;
  }

  internal void ComputeToggledSegment(ChartSeriesBase series, LegendItem legendItem)
  {
    ChartSegment segment = ChartLegend.GetSegment(series.Segments, legendItem.Item);
    int num = !(series is CircularSeriesBase circularSeriesBase) || double.IsNaN(circularSeriesBase.GroupTo) ? series.ActualData.IndexOf(legendItem.Item) : series.Segments.IndexOf(segment);
    if (segment != null && segment.IsSegmentVisible)
    {
      series.ToggledLegendIndex.Add(num);
      segment.IsSegmentVisible = false;
      legendItem.Opacity = 0.5;
    }
    else
    {
      series.ToggledLegendIndex.Remove(num);
      segment.IsSegmentVisible = true;
      legendItem.Opacity = 1.0;
    }
    this.ChartArea.ScheduleUpdate();
  }

  internal void ChangeOrientation()
  {
    ItemsPresenter visualChild = ChartLayoutUtils.GetVisualChild<ItemsPresenter>((DependencyObject) this);
    if (visualChild == null || VisualTreeHelper.GetChildrenCount((DependencyObject) visualChild) <= 0 || !(VisualTreeHelper.GetChild((DependencyObject) visualChild, 0) is StackPanel child))
      return;
    int num = (int) Enum.Parse(typeof (System.Windows.Controls.Orientation), ((ChartOrientation) (this.Orientation == ChartOrientation.Default ? (this.DockPosition == ChartDock.Left || this.DockPosition == ChartDock.Right ? 2 : 1) : (int) this.Orientation)).ToString(), false);
    child.Orientation = (System.Windows.Controls.Orientation) num;
  }

  internal LegendPosition GetPosition() => this.LegendPosition;

  protected override AutomationPeer OnCreateAutomationPeer()
  {
    return (AutomationPeer) new ChartLegendAutomationPeer(this);
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    if (!(e.OriginalSource is FrameworkElement originalSource) || !(originalSource.DataContext is LegendItem) || !this.ToggleSeriesVisibility || Mouse.OverrideCursor != null)
      return;
    Mouse.OverrideCursor = Cursors.Hand;
  }

  protected override void OnMouseDown(MouseButtonEventArgs e)
  {
    if (!(e.OriginalSource is FrameworkElement originalSource) || !(originalSource.DataContext is LegendItem dataContext))
      return;
    if (this.ToggleSeriesVisibility && !this.ChartArea.HasDataPointBasedLegend())
      dataContext.IsSeriesVisible = !dataContext.IsSeriesVisible;
    dataContext.OnPropertyChanged("HookLegendClick");
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    if (Mouse.OverrideCursor == null)
      return;
    Mouse.OverrideCursor = (Cursor) null;
  }

  private static void OnLegendPositionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    (d as ChartLegend).OnLegendPositionChanged();
  }

  private static void OnDockPositionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartLegend).OnDockPositionChanged((ChartDock) e.OldValue, (ChartDock) e.NewValue);
  }

  private static void OnSeriesPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ChartLegend newLegend) || newLegend.ChartArea == null)
      return;
    newLegend.ChartArea.IsUpdateLegend = true;
    newLegend.ChartArea.UpdateLegend((object) newLegend, true);
  }

  private static void OnOffsetValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ChartLegend).OnOffsetValueChanged(e);
  }

  private static ChartSegment GetSegment(ObservableCollection<ChartSegment> segments, object item)
  {
    ChartSegment segment1 = (ChartSegment) null;
    foreach (ChartSegment segment2 in (Collection<ChartSegment>) segments)
    {
      if (segment2.Item == item)
      {
        segment1 = segment2;
        break;
      }
      if (segment2.Series is CircularSeriesBase && !double.IsNaN(((CircularSeriesBase) segment2.Series).GroupTo) && ((CircularSeriesBase) segment2.Series).GroupedData == segment2.Item)
        segment1 = segment2;
    }
    return segment1;
  }

  private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ChartLegend).ChangeOrientation();
  }

  private void OnLegendPositionChanged()
  {
    this.InternalDockPosition = this.GetPosition() != LegendPosition.Inside ? this.DockPosition : ChartDock.Floating;
    ChartDockPanel.SetDock((UIElement) this, this.InternalDockPosition);
    if (this.Parent == null)
      return;
    this.ChartArea.LayoutLegends();
    this.ChartArea.UpdateLegendArrangeRect();
    (this.Parent as ChartDockPanel).InvalidateMeasure();
  }

  private void OnDockPositionChanged(ChartDock oldValue, ChartDock newValue)
  {
    if (this.GetPosition() == LegendPosition.Outside)
    {
      this.InternalDockPosition = this.DockPosition;
      if (this.ChartArea != null)
      {
        this.ChartArea.LayoutLegends();
        this.ChartArea.UpdateLegendArrangeRect();
      }
    }
    this.ChangeOrientation();
    if (this.GetPosition() == LegendPosition.Inside)
    {
      if (!(this.Parent is ChartDockPanel parent))
        return;
      this.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      this.ChartArea.UpdateLegendArrangeRect();
      parent.InvalidateMeasure();
    }
    else
    {
      if (ChartDockPanel.GetDock((UIElement) this) == this.InternalDockPosition)
        return;
      ChartDockPanel.SetDock((UIElement) this, this.InternalDockPosition);
    }
  }

  private void OnOffsetValueChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(this.Parent is ChartDockPanel))
      return;
    (this.Parent as ChartDockPanel).InvalidateArrange();
  }

  private void OnChartLegendLayoutUpdated(object sender, EventArgs e)
  {
    if (this.isLayoutUpdated)
      return;
    this.ChangeOrientation();
    this.isLayoutUpdated = true;
  }

  private void ChartLegend_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    this.isLayoutUpdated = false;
  }

  private void ChartLegend_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.Visibility == Visibility.Collapsed && this.Orientation == ChartOrientation.Vertical)
      this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.ChartLegend_IsVisibleChanged);
    this.ChangeOrientation();
  }
}
