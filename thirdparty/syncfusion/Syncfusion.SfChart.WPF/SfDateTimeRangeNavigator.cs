// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SfDateTimeRangeNavigator
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SfDateTimeRangeNavigator : SfRangeNavigator
{
  public static readonly DependencyProperty LeftThumbStyleProperty = DependencyProperty.Register(nameof (LeftThumbStyle), typeof (ThumbStyle), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnThumbLineStyleChanged)));
  public static readonly DependencyProperty RightThumbStyleProperty = DependencyProperty.Register(nameof (RightThumbStyle), typeof (ThumbStyle), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnThumbLineStyleChanged)));
  public static readonly DependencyProperty HigherBarTickLineStyleProperty = DependencyProperty.Register(nameof (HigherBarTickLineStyle), typeof (Style), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnTickLineStyleChanged)));
  public static readonly DependencyProperty HigherBarGridLineStyleProperty = DependencyProperty.Register(nameof (HigherBarGridLineStyle), typeof (Style), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnGridLineStyleChanged)));
  public static readonly DependencyProperty LowerBarTickLineStyleProperty = DependencyProperty.Register(nameof (LowerBarTickLineStyle), typeof (Style), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnTickLineStyleChanged)));
  public static readonly DependencyProperty LowerBarGridLineStyleProperty = DependencyProperty.Register(nameof (LowerBarGridLineStyle), typeof (Style), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnGridLineStyleChanged)));
  public static readonly DependencyProperty EnableDeferredUpdateProperty = DependencyProperty.Register(nameof (EnableDeferredUpdate), typeof (bool), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) false, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnEnableDeferredUpdatePropertyChanged)));
  public static readonly DependencyProperty DeferredUpdateDelayProperty = DependencyProperty.Register(nameof (DeferredUpdateDelay), typeof (double), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) 0.5, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnDeferredUpdateDurationPropertyChanged)));
  public static readonly DependencyProperty IntervalsProperty = DependencyProperty.Register(nameof (Intervals), typeof (Intervals), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnIntervalChanged)));
  public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (object), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) DateTime.MinValue, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnMinimumMaximumChanged)));
  public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (object), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) DateTime.MinValue, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnMinimumMaximumChanged)));
  public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof (ItemsSource), typeof (object), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnItemSourceChanged)));
  public static readonly DependencyProperty ShowToolTipProperty = DependencyProperty.Register(nameof (ShowToolTip), typeof (bool), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) false, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnShowToolTipChanged)));
  public static readonly DependencyProperty LeftToolTipTemplateProperty = DependencyProperty.Register(nameof (LeftToolTipTemplate), typeof (DataTemplate), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnLeftToolTipTemplateChanged)));
  public static readonly DependencyProperty RightToolTipTemplateProperty = DependencyProperty.Register(nameof (RightToolTipTemplate), typeof (DataTemplate), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnRightToolTipTemplateChanged)));
  public static readonly DependencyProperty ToolTipLabelFormatProperty = DependencyProperty.Register(nameof (ToolTipLabelFormat), typeof (string), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) "yyyy/MMM/dd", new PropertyChangedCallback(SfDateTimeRangeNavigator.OnToolTipFormatChanged)));
  public static readonly DependencyProperty XBindingPathProperty = DependencyProperty.Register(nameof (XBindingPath), typeof (string), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnXBindingPathChanged)));
  public static readonly DependencyProperty LowerLevelBarStyleProperty = DependencyProperty.Register(nameof (LowerLevelBarStyle), typeof (LabelBarStyle), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnLabelStyleChanged)));
  public static readonly DependencyProperty HigherLevelBarStyleProperty = DependencyProperty.Register(nameof (HigherLevelBarStyle), typeof (LabelBarStyle), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnLabelStyleChanged)));
  public static readonly DependencyProperty HigherLabelStyleProperty = DependencyProperty.Register(nameof (HigherLabelStyle), typeof (Style), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnLabelStyleChanged)));
  public static readonly DependencyProperty LowerLabelStyleProperty = DependencyProperty.Register(nameof (LowerLabelStyle), typeof (Style), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnLabelStyleChanged)));
  public static readonly DependencyProperty ShowGridLinesProperty = DependencyProperty.Register(nameof (ShowGridLines), typeof (bool), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) true, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnShowGridlinesChanged)));
  public static readonly DependencyProperty RangePaddingProperty = DependencyProperty.Register(nameof (RangePadding), typeof (NavigatorRangePadding), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) NavigatorRangePadding.Round));
  public static readonly DependencyProperty LowerBarVisibilityProperty = DependencyProperty.Register(nameof (LowerBarVisibility), typeof (Visibility), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) Visibility.Visible, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnLowerBarVisibilityChanged)));
  public static readonly DependencyProperty HigherBarVisibilityProperty = DependencyProperty.Register(nameof (HigherBarVisibility), typeof (Visibility), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((object) Visibility.Visible, new PropertyChangedCallback(SfDateTimeRangeNavigator.OnHigherBarVisibilityChanged)));
  internal static readonly DependencyProperty SelectedDataProperty = DependencyProperty.Register(nameof (SelectedData), typeof (object), typeof (SfDateTimeRangeNavigator), new PropertyMetadata((PropertyChangedCallback) null));
  private bool isScrolling;
  private bool isRightSet;
  private bool isUpdate;
  private Panel hover;
  private Panel tool;
  private UIElementsRecycler<TextBlock> upperLabelRecycler;
  private DispatcherTimer timer;
  private UIElementsRecycler<TextBlock> lowerLabelRecycler;
  private UIElementsRecycler<Line> upperTickLineRecycler;
  private UIElementsRecycler<Line> lowerGridLineRecycler;
  private UIElementsRecycler<Line> lowerTickLineRecycler;
  private UIElementsRecycler<Line> upperGridLineRecycler;
  private DataTemplate leftTemplate;
  private DataTemplate rightTemplate;
  private Panel navigatorPanel;
  private Border uppperBorder;
  private Border lowerBorder;
  private Panel upperLabelBar;
  private Panel lowerLabelBar;
  private Panel lowerLineBar;
  private Panel upperLineBar;
  private Rect[] labelElementBounds;
  private ObservableCollection<double> lowerLabelBounds;
  private ObservableCollection<double> upperLabelBounds;
  private ObservableCollection<ChartAxisLabel> upperBarLabels;
  private ObservableCollection<ChartAxisLabel> lowerBarLabels;
  private ObservableCollection<string> navigatorIntervals;
  private DateTime maximumDateTimeValue = DateTime.MinValue;
  private DateTime minimumDateTimeValue = DateTime.MinValue;
  private double totalNoofDays;
  private Line leftThumbLine;
  private Line rightThumbLine;
  private ContentPresenter leftThumbSymbol;
  private ContentPresenter rightThumbSymbol;
  private bool isMinMaxSet;
  private ObservableCollection<double> daysvalue = new ObservableCollection<double>();
  private double txtblockwidth;
  private List<ObservableCollection<string>> formatters;
  private bool isLeftButtonPressed;
  private bool isDragged;
  private bool isUpdateDispatched;
  private Thumb leftThumb;
  private Thumb rightThumb;
  private bool isFormatterEmpty;
  private Panel innerGridlines;
  private string dockPosition = "Lower";

  internal object FormsRangeNavigator { get; set; }

  public SfDateTimeRangeNavigator()
  {
    this.DefaultStyleKey = (object) typeof (SfDateTimeRangeNavigator);
    this.upperBarLabels = new ObservableCollection<ChartAxisLabel>();
    this.lowerBarLabels = new ObservableCollection<ChartAxisLabel>();
    this.Loaded += new RoutedEventHandler(this.OnSfDateTimeRangeNavigatorLoaded);
    this.Intervals = new Intervals();
    this.Intervals.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnIntervalsCollectionChanged);
  }

  public event EventHandler<LowerBarLabelsCreatedEventArgs> LowerBarLabelsCreated;

  public event EventHandler<HigherBarLabelsCreatedEventArgs> HigherBarLabelsCreated;

  public ThumbStyle LeftThumbStyle
  {
    get => (ThumbStyle) this.GetValue(SfDateTimeRangeNavigator.LeftThumbStyleProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.LeftThumbStyleProperty, (object) value);
  }

  public ThumbStyle RightThumbStyle
  {
    get => (ThumbStyle) this.GetValue(SfDateTimeRangeNavigator.RightThumbStyleProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.RightThumbStyleProperty, (object) value);
  }

  public Style HigherBarTickLineStyle
  {
    get => (Style) this.GetValue(SfDateTimeRangeNavigator.HigherBarTickLineStyleProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.HigherBarTickLineStyleProperty, (object) value);
  }

  public Style HigherBarGridLineStyle
  {
    get => (Style) this.GetValue(SfDateTimeRangeNavigator.HigherBarGridLineStyleProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.HigherBarGridLineStyleProperty, (object) value);
  }

  public Style LowerBarTickLineStyle
  {
    get => (Style) this.GetValue(SfDateTimeRangeNavigator.LowerBarTickLineStyleProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.LowerBarTickLineStyleProperty, (object) value);
  }

  public Style LowerBarGridLineStyle
  {
    get => (Style) this.GetValue(SfDateTimeRangeNavigator.LowerBarGridLineStyleProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.LowerBarGridLineStyleProperty, (object) value);
  }

  public bool EnableDeferredUpdate
  {
    get => (bool) this.GetValue(SfDateTimeRangeNavigator.EnableDeferredUpdateProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.EnableDeferredUpdateProperty, (object) value);
  }

  public double DeferredUpdateDelay
  {
    get => (double) this.GetValue(SfDateTimeRangeNavigator.DeferredUpdateDelayProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.DeferredUpdateDelayProperty, (object) value);
  }

  public Intervals Intervals
  {
    get => (Intervals) this.GetValue(SfDateTimeRangeNavigator.IntervalsProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.IntervalsProperty, (object) value);
  }

  public object Minimum
  {
    get => this.GetValue(SfDateTimeRangeNavigator.MinimumProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.MinimumProperty, value);
  }

  public object Maximum
  {
    get => this.GetValue(SfDateTimeRangeNavigator.MaximumProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.MaximumProperty, value);
  }

  public object ItemsSource
  {
    get => this.GetValue(SfDateTimeRangeNavigator.ItemsSourceProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.ItemsSourceProperty, value);
  }

  public object SelectedData
  {
    get => this.GetValue(SfDateTimeRangeNavigator.SelectedDataProperty);
    private set => this.SetValue(SfDateTimeRangeNavigator.SelectedDataProperty, value);
  }

  public bool ShowToolTip
  {
    get => (bool) this.GetValue(SfDateTimeRangeNavigator.ShowToolTipProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.ShowToolTipProperty, (object) value);
  }

  public Visibility LowerBarVisibility
  {
    get => (Visibility) this.GetValue(SfDateTimeRangeNavigator.LowerBarVisibilityProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.LowerBarVisibilityProperty, (object) value);
  }

  public Visibility HigherBarVisibility
  {
    get => (Visibility) this.GetValue(SfDateTimeRangeNavigator.HigherBarVisibilityProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.HigherBarVisibilityProperty, (object) value);
  }

  public DataTemplate LeftToolTipTemplate
  {
    get => (DataTemplate) this.GetValue(SfDateTimeRangeNavigator.LeftToolTipTemplateProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.LeftToolTipTemplateProperty, (object) value);
  }

  public DataTemplate RightToolTipTemplate
  {
    get => (DataTemplate) this.GetValue(SfDateTimeRangeNavigator.RightToolTipTemplateProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.RightToolTipTemplateProperty, (object) value);
  }

  public string ToolTipLabelFormat
  {
    get => (string) this.GetValue(SfDateTimeRangeNavigator.ToolTipLabelFormatProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.ToolTipLabelFormatProperty, (object) value);
  }

  public string XBindingPath
  {
    get => (string) this.GetValue(SfDateTimeRangeNavigator.XBindingPathProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.XBindingPathProperty, (object) value);
  }

  public LabelBarStyle LowerLevelBarStyle
  {
    get => (LabelBarStyle) this.GetValue(SfDateTimeRangeNavigator.LowerLevelBarStyleProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.LowerLevelBarStyleProperty, (object) value);
  }

  public LabelBarStyle HigherLevelBarStyle
  {
    get => (LabelBarStyle) this.GetValue(SfDateTimeRangeNavigator.HigherLevelBarStyleProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.HigherLevelBarStyleProperty, (object) value);
  }

  public Style HigherLabelStyle
  {
    get => (Style) this.GetValue(SfDateTimeRangeNavigator.HigherLabelStyleProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.HigherLabelStyleProperty, (object) value);
  }

  public Style LowerLabelStyle
  {
    get => (Style) this.GetValue(SfDateTimeRangeNavigator.LowerLabelStyleProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.LowerLabelStyleProperty, (object) value);
  }

  public bool ShowGridLines
  {
    get => (bool) this.GetValue(SfDateTimeRangeNavigator.ShowGridLinesProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.ShowGridLinesProperty, (object) value);
  }

  public NavigatorRangePadding RangePadding
  {
    get => (NavigatorRangePadding) this.GetValue(SfDateTimeRangeNavigator.RangePaddingProperty);
    set => this.SetValue(SfDateTimeRangeNavigator.RangePaddingProperty, (object) value);
  }

  internal void Scheduleupdate()
  {
    if (this.isUpdateDispatched)
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) new Action(this.Update));
    this.isUpdateDispatched = true;
  }

  internal void Update()
  {
    if (this.minimumDateTimeValue == DateTime.MinValue || this.maximumDateTimeValue == DateTime.MinValue)
      return;
    this.navigatorIntervals = (ObservableCollection<string>) null;
    this.formatters = (List<ObservableCollection<string>>) null;
    this.navigatorIntervals = new ObservableCollection<string>();
    this.formatters = new List<ObservableCollection<string>>();
    string str = "Upper";
    if (this.isMinMaxSet)
    {
      if (this.Navigator != null)
        this.Navigator.IsValueChangedTrigger = true;
      bool flag = false;
      if (this.minimumDateTimeValue != this.maximumDateTimeValue)
      {
        if (this.Intervals != null && this.Intervals.Count > 0 && this.upperLabelBar != null && this.lowerLabelBar != null)
        {
          foreach (Interval interval in (Collection<Interval>) this.Intervals)
          {
            this.navigatorIntervals.Add(interval.IntervalType.ToString());
            this.formatters.Add(interval.LabelFormatters);
          }
          if (this.navigatorIntervals.Contains("Year"))
          {
            this.SetYearInterval(0, str, this.formatters[this.navigatorIntervals.IndexOf("Year")]);
            str = "Lower";
          }
          if (this.navigatorIntervals.Contains("Quarter"))
          {
            this.SetQuarterInterval(0, str, this.formatters[this.navigatorIntervals.IndexOf("Quarter")]);
            if (str == "Lower")
              flag = true;
            else
              str = "Lower";
          }
          if (this.navigatorIntervals.Contains("Month") && !flag)
          {
            this.SetMonthInterval(0, str, this.formatters[this.navigatorIntervals.IndexOf("Month")]);
            if (str == "Lower")
              flag = true;
            else
              str = "Lower";
          }
          if (this.navigatorIntervals.Contains("Week") && !flag)
          {
            this.SetWeekInterval(0, str, this.formatters[this.navigatorIntervals.IndexOf("Week")]);
            if (str == "Lower")
              flag = true;
            else
              str = "Lower";
          }
          if (this.navigatorIntervals.Contains("Day") && !flag)
          {
            this.SetDayInterval(0, str, this.formatters[this.navigatorIntervals.IndexOf("Day")]);
            if (str == "Lower")
              flag = true;
            else
              str = "Lower";
          }
          if (this.navigatorIntervals.Contains("Hour") && !flag)
          {
            this.SetHourInterval(0, str, this.formatters[this.navigatorIntervals.IndexOf("Hour")]);
            if (str == "Lower")
              flag = true;
          }
          if (this.upperLabelRecycler.Count == 0 && this.upperLabelBar != null)
            this.SetYearInterval(0, "Upper", (ObservableCollection<string>) null);
          if (this.lowerLabelBar != null && !flag)
          {
            if (this.navigatorIntervals.Contains("Year"))
              this.SetYearInterval(0, this.dockPosition, this.formatters[this.navigatorIntervals.IndexOf("Year")]);
            if (this.navigatorIntervals.Contains("Quarter"))
              this.SetQuarterInterval(0, this.dockPosition, this.formatters[this.navigatorIntervals.IndexOf("Quarter")]);
            if (this.navigatorIntervals.Contains("Month"))
              this.SetMonthInterval(0, this.dockPosition, this.formatters[this.navigatorIntervals.IndexOf("Month")]);
            if (this.navigatorIntervals.Contains("Week"))
              this.SetWeekInterval(0, this.dockPosition, this.formatters[this.navigatorIntervals.IndexOf("Week")]);
            if (this.navigatorIntervals.Contains("Day"))
              this.SetDayInterval(0, this.dockPosition, this.formatters[this.navigatorIntervals.IndexOf("Day")]);
            if (this.navigatorIntervals.Contains("Hour"))
              this.SetHourInterval(0, this.dockPosition, this.formatters[this.navigatorIntervals.IndexOf("Hour")]);
          }
        }
        else
        {
          if (this.upperLabelBar != null)
            this.SetYearInterval(0, "Upper", (ObservableCollection<string>) null);
          if (this.lowerLabelBar != null)
            this.SetQuarterInterval(0, this.dockPosition, (ObservableCollection<string>) null);
        }
      }
    }
    this.UpdateTooltip();
    this.isUpdateDispatched = false;
  }

  internal void SetThumbStyle()
  {
    if (this.LeftThumbStyle != null)
    {
      if (this.leftThumbLine != null && this.LeftThumbStyle.LineStyle != null)
        this.leftThumbLine.Style = this.LeftThumbStyle.LineStyle;
      if (this.leftThumbSymbol != null && this.LeftThumbStyle.SymbolTemplate != null)
        this.leftThumbSymbol.ContentTemplate = this.LeftThumbStyle.SymbolTemplate;
    }
    if (this.RightThumbStyle != null)
    {
      if (this.rightThumbLine != null && this.RightThumbStyle.LineStyle != null)
        this.rightThumbLine.Style = this.RightThumbStyle.LineStyle;
      if (this.rightThumbSymbol != null && this.RightThumbStyle.SymbolTemplate != null)
        this.rightThumbSymbol.ContentTemplate = this.RightThumbStyle.SymbolTemplate;
    }
    if (this.leftThumb != null)
    {
      this.leftThumbLine.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      this.leftThumb.Width = this.leftThumbLine.DesiredSize.Width;
    }
    if (this.rightThumb != null)
    {
      this.rightThumbLine.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      this.rightThumb.Width = this.rightThumbLine.DesiredSize.Width;
    }
    if (this.leftThumbSymbol != null && this.LeftThumbStyle != null)
    {
      if (this.LeftThumbStyle.SymbolTemplate != null)
      {
        this.leftThumbSymbol.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        this.leftThumbSymbol.Margin = new Thickness(-this.leftThumbSymbol.DesiredSize.Width - 15.0, 0.0, -this.leftThumbSymbol.DesiredSize.Width - 15.0, 0.0);
      }
      else
        this.leftThumbSymbol.ClearValue(FrameworkElement.MarginProperty);
    }
    if (this.rightThumbSymbol == null || this.RightThumbStyle == null)
      return;
    if (this.RightThumbStyle.SymbolTemplate != null)
    {
      this.rightThumbSymbol.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      this.rightThumbSymbol.Margin = new Thickness(-this.rightThumbSymbol.DesiredSize.Width - 15.0, 0.0, -this.rightThumbSymbol.DesiredSize.Width - 15.0, 0.0);
    }
    else
      this.rightThumbSymbol.ClearValue(FrameworkElement.MarginProperty);
  }

  internal void GeneratePoints()
  {
    bool flag = false;
    if (this.ItemsSource != null && !string.IsNullOrEmpty(this.XBindingPath))
    {
      if (this.ItemsSource is DataTable)
        this.GenerateDataTablePoints();
      else
        this.GeneratePropertyPoints();
      flag = true;
    }
    if (this.XValues != null && this.XValues is IList<DateTime> && (this.XValues as IList<DateTime>).Count != 0 && flag)
    {
      if (this.RangePadding == NavigatorRangePadding.Round)
      {
        this.maximumDateTimeValue = (this.XValues as IList<DateTime>).Max<DateTime>().AddHours(new TimeSpan(0, 23, 59, 59).TotalHours - (double) (this.XValues as IList<DateTime>).Max<DateTime>().Hour);
        this.minimumDateTimeValue = (this.XValues as IList<DateTime>).Min<DateTime>().AddHours(new TimeSpan(0, 0, 0, 1).TotalHours - (double) (this.XValues as IList<DateTime>).Min<DateTime>().Hour);
      }
      else
      {
        this.maximumDateTimeValue = (this.XValues as IList<DateTime>).Max<DateTime>();
        this.minimumDateTimeValue = (this.XValues as IList<DateTime>).Min<DateTime>();
      }
    }
    else if (Convert.ToString(this.Minimum) != "0" && Convert.ToString(this.Maximum) != "1")
    {
      this.maximumDateTimeValue = Convert.ToDateTime(this.Maximum, (IFormatProvider) CultureInfo.InvariantCulture);
      this.minimumDateTimeValue = Convert.ToDateTime(this.Minimum, (IFormatProvider) CultureInfo.InvariantCulture);
    }
    if (this.maximumDateTimeValue != DateTime.MinValue && this.minimumDateTimeValue != DateTime.MinValue)
    {
      this.isMinMaxSet = true;
      this.totalNoofDays = this.maximumDateTimeValue.ToOADate() - this.minimumDateTimeValue.ToOADate() == 0.0 ? 1.0 : this.maximumDateTimeValue.ToOADate() - this.minimumDateTimeValue.ToOADate();
      if (this.upperLabelRecycler != null && this.upperLabelRecycler.Count > 0 || this.lowerLabelRecycler != null && this.lowerLabelRecycler.Count > 0)
        this.CalculateSelectedData();
    }
    else
    {
      this.isMinMaxSet = false;
      this.totalNoofDays = 0.0;
    }
    if ((this.Navigator == null || this.upperLabelBar == null || this.Navigator.TrackSize == 0.0) && (this.lowerLabelBar == null || this.Navigator.TrackSize == 0.0))
      return;
    this.Update();
  }

  internal override void CalculateSelectedData()
  {
    this.Selected = new ObservableCollection<object>();
    if (this.Navigator != null)
    {
      this.Navigator.IsValueChangedTrigger = false;
      if (this.ItemsSource != null && this.XValues != null)
      {
        IEnumerator enumerator1 = (this.ItemsSource is DataTable ? (IEnumerable) (this.ItemsSource as DataTable).Rows : this.ItemsSource as IEnumerable).GetEnumerator();
        IEnumerator enumerator2 = this.XValues.GetEnumerator();
        while (enumerator1.MoveNext() && enumerator2.MoveNext())
        {
          if ((DateTime) enumerator2.Current >= Convert.ToDateTime(this.ViewRangeStart) && (DateTime) enumerator2.Current <= Convert.ToDateTime(this.ViewRangeEnd))
            this.Selected.Add(enumerator1.Current);
        }
        this.SelectedData = (object) this.Selected;
      }
      this.Navigator.IsValueChangedTrigger = false;
      this.ZoomFactor = this.Navigator.RangeEnd - this.Navigator.RangeStart;
      this.Navigator.IsValueChangedTrigger = false;
      this.ZoomPosition = this.Navigator.RangeStart;
      this.isUpdate = false;
    }
    this.SetSelectedDataStyle();
  }

  internal void SetLabelPosition()
  {
    if (this.navigatorPanel == null)
      return;
    if (this.HigherLevelBarStyle != null && this.HigherLevelBarStyle.Position == BarPosition.Inside)
    {
      this.uppperBorder.VerticalAlignment = VerticalAlignment.Top;
      Grid.SetRow((UIElement) this.uppperBorder, 1);
      this.uppperBorder.Background = (Brush) new SolidColorBrush(Colors.Transparent);
    }
    else
    {
      Grid.SetRow((UIElement) this.uppperBorder, 0);
      this.uppperBorder.Background = this.HigherLevelBarStyle.Background;
    }
    if (this.LowerLevelBarStyle != null && this.LowerLevelBarStyle.Position == BarPosition.Inside)
    {
      this.lowerBorder.VerticalAlignment = VerticalAlignment.Bottom;
      Grid.SetRow((UIElement) this.lowerBorder, 1);
      this.lowerBorder.Background = (Brush) new SolidColorBrush(Colors.Transparent);
    }
    else
    {
      this.lowerBorder.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetRow((UIElement) this.lowerBorder, 2);
      this.lowerBorder.Background = this.LowerLevelBarStyle.Background;
    }
  }

  internal override void OnViewRangeStartChanged()
  {
    if (!(this.ViewRangeStart is double) && this.Navigator != null && this.Navigator.TrackSize != 0.0 && this.totalNoofDays != 0.0)
    {
      double num = 1.0 * ((Convert.ToDateTime(this.ViewRangeStart) - this.minimumDateTimeValue).TotalDays * (this.Navigator.TrackSize / this.totalNoofDays) / this.Navigator.TrackSize);
      this.Navigator.IsValueChangedTrigger = false;
      this.Navigator.RangeStart = num < 0.0 ? 0.0 : num;
    }
    this.CalculateSelectedData();
    this.UpdateTooltip();
  }

  internal override void OnViewRangeEndChanged()
  {
    if (!(this.ViewRangeEnd is double) && this.Navigator != null && this.Navigator.TrackSize != 0.0 && this.totalNoofDays != 0.0)
    {
      double num = 1.0 * ((Convert.ToDateTime(this.ViewRangeEnd) - this.minimumDateTimeValue).TotalDays * (this.Navigator.TrackSize / this.totalNoofDays) / this.Navigator.TrackSize);
      this.Navigator.IsValueChangedTrigger = false;
      this.Navigator.RangeEnd = num > 1.0 ? 1.0 : num;
    }
    this.CalculateSelectedData();
    this.UpdateTooltip();
  }

  internal override void OnZoomFactorChanged(double newValue, double oldValue)
  {
    this.zoomFactor = newValue;
    if (this.Navigator == null)
    {
      base.OnZoomFactorChanged(newValue, oldValue);
    }
    else
    {
      if (this.Navigator == null)
        return;
      this.isUpdate = true;
      this.Navigator.IsValueChangedTrigger = false;
      if (this.Navigator.RangeEnd != this.Navigator.Maximum && this.Navigator.isFarDragged || newValue != oldValue)
        this.Navigator.RangeEnd = this.ZoomPosition + newValue;
      this.isUpdate = false;
      this.UpdateTooltip();
    }
  }

  internal override void OnZoomPositionChanged(double newValue)
  {
    this.zoomPosition = newValue;
    if (this.Navigator != null)
    {
      this.Navigator.IsValueChangedTrigger = false;
      this.Navigator.RangeEnd = this.ZoomFactor + newValue;
      this.Navigator.IsValueChangedTrigger = false;
      this.Navigator.RangeStart = newValue;
      this.UpdateTooltip();
      this.ChangeViewRange();
    }
    else
    {
      if (this.Navigator != null)
        return;
      base.OnZoomPositionChanged(newValue);
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.navigatorPanel = (Panel) (this.GetTemplateChild("PART_RangeNavigatorPanel") as Grid);
    this.uppperBorder = this.GetTemplateChild("Part_UpperBorder") as Border;
    this.lowerBorder = this.GetTemplateChild("Part_Border") as Border;
    this.upperLabelBar = this.GetTemplateChild("PART_UPPERBAR") as Panel;
    this.upperLineBar = this.GetTemplateChild("PART_UPPERLINE") as Panel;
    this.lowerLabelBar = this.GetTemplateChild("PART_LOWERBAR") as Panel;
    this.lowerLineBar = this.GetTemplateChild("PART_LOWERLINE") as Panel;
    this.hover = this.GetTemplateChild("Part_Hover") as Panel;
    this.tool = this.GetTemplateChild("Part_Tooltip") as Panel;
    this.innerGridlines = this.GetTemplateChild("Part_Content_line") as Panel;
    this.UpdateHigherBarVisibility();
    this.UpdateLowerBarVisibility();
    this.leftTemplate = ChartDictionaries.GenericCommonDictionary[(object) "leftTooltipTemplate"] as DataTemplate;
    this.rightTemplate = ChartDictionaries.GenericCommonDictionary[(object) "rightTooltipTemplate"] as DataTemplate;
    if (this.upperLabelBar != null)
      this.upperLabelRecycler = new UIElementsRecycler<TextBlock>(this.upperLabelBar);
    if (this.lowerLabelBar != null)
      this.lowerLabelRecycler = new UIElementsRecycler<TextBlock>(this.lowerLabelBar);
    if (this.innerGridlines != null)
    {
      this.lowerGridLineRecycler = new UIElementsRecycler<Line>(this.innerGridlines);
      this.upperGridLineRecycler = new UIElementsRecycler<Line>(this.innerGridlines);
    }
    if (this.upperLineBar != null)
      this.upperTickLineRecycler = new UIElementsRecycler<Line>(this.upperLineBar);
    if (this.lowerLineBar != null)
      this.lowerTickLineRecycler = new UIElementsRecycler<Line>(this.lowerLineBar);
    if (this.upperLabelBar != null)
    {
      this.upperLabelBar.MouseMove += new MouseEventHandler(this.UpperLabelBar_MouseMove);
      this.upperLabelBar.MouseLeave += new MouseEventHandler(this.UpperLabelBar_MouseLeave);
      this.upperLabelBar.MouseLeftButtonDown += new MouseButtonEventHandler(this.UpperLabelBar_MouseLeftButtonDown);
    }
    if (this.lowerLabelBar != null)
    {
      this.lowerLabelBar.MouseMove += new MouseEventHandler(this.LowerLabelBar_MouseMove);
      this.lowerLabelBar.MouseLeave += new MouseEventHandler(this.LowerLabelBar_MouseLeave);
      this.lowerLabelBar.MouseLeftButtonDown += new MouseButtonEventHandler(this.LowerLabelBar_MouseLeftButtonDown);
    }
    this.AddHandler(UIElement.MouseLeftButtonDownEvent, (Delegate) new MouseButtonEventHandler(this.SfDateTimeRangeNavigator_MouseLeftButtonDown), true);
    this.AddHandler(UIElement.MouseLeftButtonUpEvent, (Delegate) new MouseButtonEventHandler(this.SfDateTimeRangeNavigator_MouseLeftButtonUp), true);
    if (this.uppperBorder != null)
      Panel.SetZIndex((UIElement) this.uppperBorder, 1);
    if (this.HigherLevelBarStyle != null)
      this.HigherLevelBarStyle.DateTimeRangeNavigator = this;
    if (this.LowerLevelBarStyle != null)
      this.LowerLevelBarStyle.DateTimeRangeNavigator = this;
    this.SetLabelPosition();
    this.GeneratePoints();
    this.UpdateTooltipVisibility();
  }

  protected void OnDataSourceChanged(DependencyPropertyChangedEventArgs args)
  {
    this.SetDefaultRange();
    this.CalculateRange();
    this.Refresh();
    this.UpdateTooltip();
    if (args.OldValue is INotifyCollectionChanged)
      (args.OldValue as INotifyCollectionChanged).CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnSfRangeNavigatorCollectionChanged);
    if (!(args.NewValue is INotifyCollectionChanged))
      return;
    (args.NewValue as INotifyCollectionChanged).CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnSfRangeNavigatorCollectionChanged);
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    if (!this.EnableDeferredUpdate || !this.isLeftButtonPressed)
      return;
    this.isDragged = true;
  }

  protected override void OnScrollbarValueChanged(object sender, EventArgs e)
  {
    this.isScrolling = true;
    base.OnScrollbarValueChanged(sender, e);
    if (this.Navigator.Content != null)
    {
      double left = this.XRange + this.Navigator.ResizableThumbSize;
      double num = this.XRange == 0.0 ? this.Navigator.DesiredSize.Width : this.Navigator.TrackSize;
      if (!this.Scrollbar.isFarDragged)
      {
        if (this.tool != null)
        {
          this.tool.Width = num * this.Scrollbar.Scale;
          this.tool.Margin = new Thickness(left, 0.0, 0.0, 0.0);
        }
        this.upperLabelBar.Width = num * this.Scrollbar.Scale;
        this.upperLineBar.Width = num * this.Scrollbar.Scale;
        this.lowerLabelBar.Width = num * this.Scrollbar.Scale;
        this.lowerLineBar.Width = num * this.Scrollbar.Scale;
      }
      else if (!this.Scrollbar.isNearDragged && this.XRange != 0.0)
      {
        if (this.tool != null)
        {
          this.tool.Width = this.Navigator.TrackSize * this.Scrollbar.Scale;
          this.tool.Margin = new Thickness(left, 0.0, 0.0, 0.0);
        }
        this.upperLabelBar.Width = this.Navigator.TrackSize * this.Scrollbar.Scale;
        this.upperLineBar.Width = this.Navigator.TrackSize * this.Scrollbar.Scale;
        this.lowerLabelBar.Width = this.Navigator.TrackSize * this.Scrollbar.Scale;
        this.lowerLineBar.Width = this.Navigator.TrackSize * this.Scrollbar.Scale;
      }
      this.Clip = (Geometry) new RectangleGeometry()
      {
        Rect = new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight)
      };
      this.SetThumbStyle();
    }
    this.UpdateTooltip();
  }

  protected override void OnTimeLineValueChanged(object sender, EventArgs e)
  {
    this.UpdateTooltip();
    if (!this.EnableDeferredUpdate)
      this.OnInternalValueChanged();
    else
      this.ResetTimer();
  }

  protected override void OnTimeLineSizeChanged(object sender, SizeChangedEventArgs e)
  {
    if (this.Navigator == null)
      return;
    this.Update();
  }

  protected override void OnValueChanged()
  {
    this.ChangeViewRange();
    base.OnValueChanged();
  }

  private static void OnLeftToolTipTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfDateTimeRangeNavigator timeRangeNavigator) || timeRangeNavigator.tool == null || !timeRangeNavigator.ShowToolTip || timeRangeNavigator.tool.Children.Count <= 0)
      return;
    (timeRangeNavigator.tool.Children[0] as ContentControl).ContentTemplate = (DataTemplate) e.NewValue;
    timeRangeNavigator.UpdateTooltip();
  }

  private static void OnThumbLineStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfDateTimeRangeNavigator navigator))
      return;
    navigator.ThumbStyleChanged(navigator, e);
  }

  private static void OnGridLineStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfDateTimeRangeNavigator timeRangeNavigator) || timeRangeNavigator.lowerGridLineRecycler == null && timeRangeNavigator.upperGridLineRecycler == null)
      return;
    timeRangeNavigator.Scheduleupdate();
  }

  private static void OnTickLineStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfDateTimeRangeNavigator timeRangeNavigator) || timeRangeNavigator.upperTickLineRecycler == null && timeRangeNavigator.lowerTickLineRecycler == null)
      return;
    timeRangeNavigator.Scheduleupdate();
  }

  private static void OnEnableDeferredUpdatePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfDateTimeRangeNavigator timeRangeNavigator))
      return;
    timeRangeNavigator.Scheduleupdate();
  }

  private static void OnDeferredUpdateDurationPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfDateTimeRangeNavigator timeRangeNavigator))
      return;
    timeRangeNavigator.Scheduleupdate();
  }

  private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfDateTimeRangeNavigator timeRangeNavigator))
      return;
    if (e.OldValue != null)
      (e.OldValue as Intervals).CollectionChanged -= new NotifyCollectionChangedEventHandler(timeRangeNavigator.OnIntervalsCollectionChanged);
    (e.NewValue as Intervals).CollectionChanged += new NotifyCollectionChangedEventHandler(timeRangeNavigator.OnIntervalsCollectionChanged);
    if ((timeRangeNavigator.upperLabelBar == null || timeRangeNavigator.Navigator.TrackSize == 0.0) && (timeRangeNavigator.lowerLabelBar == null || timeRangeNavigator.Navigator.TrackSize == 0.0))
      return;
    timeRangeNavigator.Scheduleupdate();
  }

  private static void OnMinimumMaximumChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(d is SfDateTimeRangeNavigator timeRangeNavigator))
      return;
    timeRangeNavigator.OnMinimumMaximumChanged();
  }

  private static void OnItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    SfDateTimeRangeNavigator timeRangeNavigator = d as SfDateTimeRangeNavigator;
    if (e.NewValue == null)
    {
      timeRangeNavigator.ClearLabels();
      timeRangeNavigator.SetDefaultRange();
    }
    else
      timeRangeNavigator.OnDataSourceChanged(e);
  }

  private static void OnShowToolTipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as SfDateTimeRangeNavigator).UpdateTooltipVisibility();
  }

  private static void OnRightToolTipTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfDateTimeRangeNavigator timeRangeNavigator) || timeRangeNavigator.tool == null || !timeRangeNavigator.ShowToolTip || timeRangeNavigator.tool.Children.Count <= 0)
      return;
    (timeRangeNavigator.tool.Children[1] as ContentControl).ContentTemplate = (DataTemplate) e.NewValue;
    timeRangeNavigator.UpdateTooltip();
  }

  private static void OnToolTipFormatChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as SfDateTimeRangeNavigator).UpdateTooltip();
  }

  private static void OnXBindingPathChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    SfDateTimeRangeNavigator timeRangeNavigator = d as SfDateTimeRangeNavigator;
    if (timeRangeNavigator.ItemsSource == null)
      return;
    timeRangeNavigator.GeneratePoints();
  }

  private static void OnLabelStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as SfDateTimeRangeNavigator).LabelStyleChanged();
  }

  private static void OnShowGridlinesChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as SfDateTimeRangeNavigator).Scheduleupdate();
  }

  private static void OnLowerBarVisibilityChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as SfDateTimeRangeNavigator).UpdateLowerBarVisibility();
  }

  private static void OnHigherBarVisibilityChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as SfDateTimeRangeNavigator).UpdateHigherBarVisibility();
  }

  private static int GetWeekNumber(DateTime datePassed)
  {
    return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(datePassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
  }

  private static void SetMinuteInterval(int minuteinterval, string dockposition)
  {
  }

  private static void SetLabelStyle(Panel labelBarPanel, int index, LabelBarStyle labelBarStyle)
  {
    if (labelBarPanel.Children.Count <= index)
      return;
    if (labelBarStyle.SelectedLabelStyle == null)
    {
      labelBarPanel.Children[index].ClearValue(FrameworkElement.StyleProperty);
      (labelBarPanel.Children[index] as TextBlock).Foreground = (Brush) labelBarStyle.SelectedLabelBrush;
    }
    else
    {
      labelBarPanel.Children[index].ClearValue(TextBlock.ForegroundProperty);
      labelBarPanel.Children[index].ClearValue(FrameworkElement.StyleProperty);
      (labelBarPanel.Children[index] as TextBlock).Style = labelBarStyle.SelectedLabelStyle;
    }
  }

  private static void SetSecondInterval()
  {
  }

  private void ThumbStyleChanged(
    SfDateTimeRangeNavigator navigator,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue != null)
      (e.NewValue as ThumbStyle).Navigator = this;
    navigator.SetThumbStyle();
  }

  private void OnMinimumMaximumChanged()
  {
    if (!(Convert.ToDateTime(this.Minimum, (IFormatProvider) CultureInfo.InvariantCulture) != DateTime.MinValue) || !(Convert.ToDateTime(this.Maximum, (IFormatProvider) CultureInfo.InvariantCulture) != DateTime.MinValue))
      return;
    this.Refresh();
    this.CalculateRange();
    this.UpdateTooltip();
    if (this.ViewRangeStart != null)
      this.OnViewRangeStartChanged();
    if (this.ViewRangeEnd == null)
      return;
    this.OnViewRangeEndChanged();
  }

  private void SetDefaultRange()
  {
    if (this.Navigator == null)
      return;
    DateTime dateTime1 = Convert.ToDateTime(this.ViewRangeStart, (IFormatProvider) CultureInfo.InvariantCulture);
    DateTime dateTime2 = Convert.ToDateTime(this.ViewRangeEnd, (IFormatProvider) CultureInfo.InvariantCulture);
    if (!(dateTime1 >= this.minimumDateTimeValue) || !(dateTime1 <= this.maximumDateTimeValue))
    {
      this.ViewRangeStart = (object) null;
      this.Navigator.RangeStart = 0.0;
    }
    if (dateTime2 >= this.minimumDateTimeValue && dateTime2 <= this.maximumDateTimeValue)
      return;
    this.ViewRangeEnd = (object) null;
    this.Navigator.RangeEnd = 1.0;
  }

  private void OnSfRangeNavigatorCollectionChanged(
    object sender,
    NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == NotifyCollectionChangedAction.Reset && this.XValues is IList<DateTime>)
    {
      (this.XValues as IList<DateTime>).Clear();
      this.ClearLabels();
    }
    this.Refresh();
    if (this.ViewRangeEnd == null || this.ViewRangeStart == null)
      return;
    this.OnViewRangeStartChanged();
    this.OnViewRangeEndChanged();
  }

  private void Refresh()
  {
    this.GeneratePoints();
    this.UpdateLayout();
  }

  private void LabelStyleChanged()
  {
    if (this.HigherLevelBarStyle == null && this.LowerLevelBarStyle == null && this.HigherLabelStyle == null && this.LowerLabelStyle == null || this.upperLabelBar == null && this.lowerLabelBar == null)
      return;
    this.SetLabelPosition();
    this.SetSelectedDataStyle();
    this.Scheduleupdate();
  }

  private void UpdateHigherBarVisibility()
  {
    if (this.upperLabelBar == null)
      return;
    this.upperLabelBar.Visibility = this.HigherBarVisibility;
  }

  private void UpdateLowerBarVisibility()
  {
    if (this.lowerLabelBar == null)
      return;
    this.lowerLabelBar.Visibility = this.LowerBarVisibility;
  }

  private void OnIntervalsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.OldItems != null)
    {
      foreach (Interval oldItem in (IEnumerable) e.OldItems)
      {
        if (oldItem.LabelFormatters != null)
          oldItem.LabelFormatters.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.LabelFormatters_CollectionChanged);
      }
    }
    if (e.NewItems != null)
    {
      foreach (Interval newItem in (IEnumerable) e.NewItems)
      {
        if (newItem.LabelFormatters != null)
          newItem.LabelFormatters.CollectionChanged += new NotifyCollectionChangedEventHandler(this.LabelFormatters_CollectionChanged);
      }
    }
    if ((this.Navigator == null || this.upperLabelBar == null || this.Navigator.TrackSize == 0.0) && (this.lowerLabelBar == null || this.Navigator.TrackSize == 0.0))
      return;
    if (e.Action == NotifyCollectionChangedAction.Reset && (this.upperLabelRecycler.Count > 0 || this.lowerLabelRecycler.Count > 0))
      this.ClearNavigatorLabels();
    this.Scheduleupdate();
  }

  private void LabelFormatters_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    this.Scheduleupdate();
  }

  private void OnSfDateTimeRangeNavigatorSizeChanged(object sender, SizeChangedEventArgs e)
  {
    if (this.Scrollbar != null && this.Navigator != null)
    {
      this.Navigator.ClearValue(FrameworkElement.WidthProperty);
      this.Navigator.Width = this.ActualWidth * this.Scrollbar.Scale;
      this.XRange = -(this.ActualWidth * this.Scrollbar.Scale * this.Scrollbar.RangeStart);
      if (this.Navigator.Content != null)
      {
        double left = this.XRange >= -this.Navigator.ResizableThumbSize ? this.XRange : this.XRange + this.Navigator.ResizableThumbSize;
        this.upperLabelBar.Measure(e.NewSize);
        this.upperLabelBar.UpdateLayout();
        if (!this.Scrollbar.isFarDragged && this.XRange != 0.0)
        {
          this.upperLabelBar.Width = this.Navigator.TrackSize * this.Scrollbar.Scale;
          this.upperLineBar.Width = this.Navigator.TrackSize * this.Scrollbar.Scale;
          this.lowerLabelBar.Width = this.Navigator.TrackSize * this.Scrollbar.Scale;
          this.lowerLineBar.Width = this.Navigator.TrackSize * this.Scrollbar.Scale;
          this.upperLineBar.Margin = new Thickness(left, 0.0, 0.0, 0.0);
          this.upperLabelBar.Margin = new Thickness(left, 0.0, 0.0, 0.0);
          this.lowerLineBar.Margin = new Thickness(left, 0.0, 0.0, 0.0);
          this.lowerLabelBar.Margin = new Thickness(left, 0.0, 0.0, 0.0);
        }
        else if (!this.Scrollbar.isNearDragged && this.XRange != 0.0)
        {
          this.upperLabelBar.Width = this.Navigator.TrackSize * this.Scrollbar.Scale;
          this.upperLineBar.Width = this.Navigator.TrackSize * this.Scrollbar.Scale;
          this.lowerLabelBar.Width = this.Navigator.TrackSize * this.Scrollbar.Scale;
          this.lowerLineBar.Width = this.Navigator.TrackSize * this.Scrollbar.Scale;
          this.upperLineBar.Margin = new Thickness(left, 0.0, 0.0, 0.0);
          this.upperLabelBar.Margin = new Thickness(left, 0.0, 0.0, 0.0);
          this.lowerLineBar.Margin = new Thickness(left, 0.0, 0.0, 0.0);
          this.lowerLabelBar.Margin = new Thickness(left, 0.0, 0.0, 0.0);
        }
        if (this.tool != null && this.Navigator != null)
        {
          this.tool.Width = this.Navigator.TrackSize * this.Scrollbar.Scale;
          this.tool.Margin = new Thickness(left, 0.0, 0.0, 0.0);
        }
        this.innerGridlines.UpdateLayout();
        this.lowerLabelBar.UpdateLayout();
        this.lowerLineBar.UpdateLayout();
        this.upperLineBar.UpdateLayout();
      }
    }
    this.Clip = (Geometry) new RectangleGeometry()
    {
      Rect = new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight)
    };
    this.SetThumbStyle();
  }

  private void OnSfDateTimeRangeNavigatorLoaded(object sender, RoutedEventArgs e)
  {
    if (this.Navigator != null)
    {
      if (this.upperLabelBar != null)
      {
        this.upperLabelBar.Margin = new Thickness(this.Navigator.ResizableThumbSize, 0.0, 0.0, 0.0);
        this.upperLineBar.Margin = new Thickness(this.Navigator.ResizableThumbSize, 0.0, 0.0, 0.0);
      }
      if (this.lowerLabelBar != null)
      {
        this.lowerLabelBar.Margin = new Thickness(this.Navigator.ResizableThumbSize, 0.0, 0.0, 0.0);
        this.lowerLineBar.Margin = new Thickness(this.Navigator.ResizableThumbSize, 0.0, 0.0, 0.0);
      }
      if (this.hover != null)
        this.hover.Margin = new Thickness(this.Navigator.ResizableThumbSize, 0.0, 0.0, 0.0);
      if (this.innerGridlines != null)
        this.innerGridlines.Margin = new Thickness(this.Navigator.ResizableThumbSize, 0.0, 0.0, 0.0);
      this.GetVisualChildren();
      this.SetThumbStyle();
    }
    this.CalculateRange();
    if (this.Navigator != null)
      this.Update();
    this.SizeChanged += new SizeChangedEventHandler(this.OnSfDateTimeRangeNavigatorSizeChanged);
  }

  private void CalculateRange()
  {
    if (this.ViewRangeStart == null || this.ViewRangeEnd == null || this.totalNoofDays == 0.0 || this.ViewRangeStart is double || this.ViewRangeEnd is double || this.Navigator == null || !(this.minimumDateTimeValue != DateTime.MinValue))
      return;
    double num1 = this.Navigator.TrackSize / this.totalNoofDays;
    double num2 = (Convert.ToDateTime(this.ViewRangeStart) - this.minimumDateTimeValue).TotalDays * num1;
    double num3 = (Convert.ToDateTime(this.ViewRangeEnd) - this.minimumDateTimeValue).TotalDays * num1;
    if (Convert.ToDateTime(this.ViewRangeStart) != this.minimumDateTimeValue)
    {
      this.Navigator.IsValueChangedTrigger = false;
      double d = 1.0 * (num2 / this.Navigator.TrackSize);
      if (!double.IsNaN(d))
        this.Navigator.RangeStart = d;
    }
    if (!(Convert.ToDateTime(this.ViewRangeEnd) != this.maximumDateTimeValue))
      return;
    this.Navigator.IsValueChangedTrigger = false;
    double d1 = 1.0 * (num3 / this.Navigator.TrackSize);
    if (double.IsNaN(d1))
      return;
    this.Navigator.RangeEnd = d1;
  }

  private void UpdateTooltipVisibility()
  {
    if (this.tool == null)
      return;
    if (this.ShowToolTip)
    {
      this.tool.Children[0].Visibility = Visibility.Visible;
      this.tool.Children[1].Visibility = Visibility.Visible;
      this.UpdateTooltip();
    }
    else
    {
      this.tool.Children[0].Visibility = Visibility.Collapsed;
      this.tool.Children[1].Visibility = Visibility.Collapsed;
    }
  }

  private void SfDateTimeRangeNavigator_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!this.EnableDeferredUpdate)
      return;
    this.isLeftButtonPressed = true;
  }

  private void SfDateTimeRangeNavigator_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (!this.EnableDeferredUpdate)
      return;
    if (this.isLeftButtonPressed && this.isDragged)
    {
      this.OnInternalValueChanged();
      if (this.timer != null)
      {
        this.timer.Stop();
        this.timer.Tick -= new EventHandler(this.OnTimeout);
      }
      this.timer = (DispatcherTimer) null;
    }
    this.isLeftButtonPressed = false;
    this.isDragged = false;
  }

  private void GetVisualChildren()
  {
    if (this.Navigator == null || VisualTreeHelper.GetChildrenCount((DependencyObject) this.Navigator) <= 0 || !(VisualTreeHelper.GetChild((DependencyObject) this.Navigator, 0) is Grid child1) || !(VisualTreeHelper.GetChild((DependencyObject) child1, 0) is Grid child2))
      return;
    this.leftThumb = VisualTreeHelper.GetChild((DependencyObject) child2, 6) as Thumb;
    this.rightThumb = VisualTreeHelper.GetChild((DependencyObject) child2, 7) as Thumb;
    if (this.leftThumb != null && VisualTreeHelper.GetChild((DependencyObject) this.leftThumb, 0) is Grid child3)
    {
      this.leftThumbLine = VisualTreeHelper.GetChild((DependencyObject) child3, 0) as Line;
      this.leftThumbSymbol = VisualTreeHelper.GetChild((DependencyObject) child3, 1) as ContentPresenter;
    }
    if (this.rightThumb == null || !(VisualTreeHelper.GetChild((DependencyObject) this.rightThumb, 0) is Grid child4))
      return;
    this.rightThumbLine = VisualTreeHelper.GetChild((DependencyObject) child4, 0) as Line;
    this.rightThumbSymbol = VisualTreeHelper.GetChild((DependencyObject) child4, 1) as ContentPresenter;
  }

  private void AddDaysValues(DateTime currentDate)
  {
    if (currentDate <= this.maximumDateTimeValue)
      this.daysvalue.Add((currentDate - this.minimumDateTimeValue).TotalDays);
    else
      this.daysvalue.Add((this.maximumDateTimeValue - this.minimumDateTimeValue).TotalDays);
  }

  private void GeneratePropertyPoints()
  {
    this.DataEnd = 0.0;
    IEnumerator enumerator = (this.ItemsSource as IEnumerable).GetEnumerator();
    if (!enumerator.MoveNext())
      return;
    PropertyInfo propertyInfo = ChartDataUtils.GetPropertyInfo(enumerator.Current, this.XBindingPath);
    if (!(propertyInfo != (PropertyInfo) null))
      return;
    System.Func<object, object> getMethod = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo).GetMethod;
    this.XValues = (IEnumerable) new List<DateTime>();
    IList<DateTime> xvalues = (IList<DateTime>) (this.XValues as List<DateTime>);
    do
    {
      object obj = getMethod(enumerator.Current);
      xvalues.Add((DateTime) obj);
      ++this.DataEnd;
    }
    while (enumerator.MoveNext());
  }

  private void UpdateTooltip()
  {
    if (this.tool != null && this.ShowToolTip && this.isMinMaxSet && this.Navigator != null && this.Navigator.TrackSize != 0.0)
    {
      this.tool.Visibility = Visibility.Visible;
      double num1 = this.Navigator.TrackSize / this.totalNoofDays;
      ContentControl child1 = this.tool.Children[0] as ContentControl;
      ContentControl child2 = this.tool.Children[1] as ContentControl;
      double num2 = this.Navigator.RangeEnd * this.Navigator.TrackSize / num1;
      double num3 = this.Navigator.RangeStart * this.Navigator.TrackSize / num1;
      child1.ContentTemplate = this.LeftToolTipTemplate == null ? this.leftTemplate : this.LeftToolTipTemplate;
      child2.ContentTemplate = this.RightToolTipTemplate == null ? this.rightTemplate : this.RightToolTipTemplate;
      child1.Content = (object) this.minimumDateTimeValue.AddDays(num3).ToString(this.ToolTipLabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
      child2.Content = (object) this.minimumDateTimeValue.AddDays(num2).ToString(this.ToolTipLabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
      child1.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      child2.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      if (!this.isScrolling)
      {
        child1.UpdateLayout();
        child2.UpdateLayout();
      }
      double width1 = child1.DesiredSize.Width;
      double width2 = child2.DesiredSize.Width;
      Canvas.SetLeft(this.tool.Children[0], this.Navigator.RangeStart * this.Navigator.TrackSize - width1);
      Canvas.SetLeft(this.tool.Children[1], this.Navigator.RangeEnd * this.Navigator.TrackSize);
      this.isRightSet = false;
      child1.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
      child2.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
      if (Canvas.GetLeft((UIElement) child1) <= 0.0)
      {
        child1.ContentTemplate = this.LeftToolTipTemplate == null ? this.rightTemplate : this.LeftToolTipTemplate;
        Canvas.SetLeft(this.tool.Children[0], this.Navigator.RangeStart * this.Navigator.ActualWidth);
        this.isRightSet = true;
      }
      if (Canvas.GetLeft((UIElement) child2) + width2 >= this.Navigator.ActualWidth)
      {
        child2.ContentTemplate = this.RightToolTipTemplate == null ? this.leftTemplate : this.RightToolTipTemplate;
        Canvas.SetLeft(this.tool.Children[1], this.Navigator.RangeEnd * this.Navigator.ActualWidth - width2);
      }
      if (this.Scrollbar != null)
      {
        if (Canvas.GetLeft((UIElement) child1) / this.Navigator.TrackSize <= this.Scrollbar.RangeStart)
        {
          child1.ContentTemplate = this.LeftToolTipTemplate == null ? this.rightTemplate : this.LeftToolTipTemplate;
          if (this.Navigator.RangeStart <= this.Scrollbar.RangeStart)
            Canvas.SetLeft(this.tool.Children[0], this.Scrollbar.RangeStart * this.Navigator.TrackSize);
          else
            Canvas.SetLeft(this.tool.Children[0], Canvas.GetLeft((UIElement) child1) / this.Navigator.TrackSize * this.Navigator.TrackSize + width1);
          if (this.Navigator.RangeEnd <= this.Scrollbar.RangeStart)
            Canvas.SetLeft(this.tool.Children[1], this.Scrollbar.RangeStart * this.Navigator.TrackSize);
          this.isRightSet = true;
        }
        if ((Canvas.GetLeft((UIElement) child2) + width2) / this.Navigator.ActualWidth >= this.Scrollbar.RangeEnd)
        {
          child2.ContentTemplate = this.RightToolTipTemplate == null ? this.leftTemplate : this.RightToolTipTemplate;
          if (Math.Round(this.Navigator.RangeEnd) >= this.Scrollbar.RangeEnd)
            Canvas.SetLeft(this.tool.Children[1], this.Scrollbar.RangeEnd * this.Navigator.TrackSize + this.Navigator.ResizableThumbSize - width2);
          else
            Canvas.SetLeft(this.tool.Children[1], Canvas.GetLeft((UIElement) child2) / this.Navigator.ActualWidth * this.Navigator.ActualWidth - width2);
          if (this.Navigator.RangeStart >= this.Scrollbar.RangeEnd)
            Canvas.SetLeft(this.tool.Children[0], this.Scrollbar.RangeEnd * this.Navigator.TrackSize - width1);
        }
      }
      if (Canvas.GetLeft((UIElement) child1) + width1 >= Canvas.GetLeft((UIElement) child2))
      {
        if (!this.isRightSet)
          child1.Margin = new Thickness(0.0, 30.0, 0.0, 0.0);
        else
          child2.Margin = new Thickness(0.0, 30.0, 0.0, 0.0);
      }
      else if (!this.isRightSet)
        child1.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
      else
        child2.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
    }
    if (!this.isMinMaxSet && this.tool != null)
      this.tool.Visibility = Visibility.Collapsed;
    this.isScrolling = false;
  }

  private void ChangeViewRange()
  {
    if (this.Navigator == null || this.Navigator.TrackSize <= 0.0 || this.totalNoofDays == 0.0)
      return;
    double num = this.Navigator.TrackSize / this.totalNoofDays;
    this.ViewRangeStart = (object) this.minimumDateTimeValue.AddDays(this.Navigator.RangeStart * this.Navigator.TrackSize / num);
    this.ViewRangeEnd = (object) this.minimumDateTimeValue.AddDays(this.Navigator.RangeEnd * this.Navigator.TrackSize / num);
  }

  private void SetSelectedDataStyle()
  {
    int index1 = -1;
    if (this.upperLabelBar != null && this.upperLabelBounds != null)
    {
      for (int index2 = 0; index2 < this.upperLabelBounds.Count; ++index2)
      {
        if (this.Navigator.RangeStart * this.Navigator.TrackSize < this.upperLabelBounds[index2] && this.Navigator.RangeEnd * this.Navigator.TrackSize >= this.upperLabelBounds[index2])
        {
          SfDateTimeRangeNavigator.SetLabelStyle(this.upperLabelBar, index2, this.HigherLevelBarStyle);
          index1 = index2;
        }
        else if (index2 == 0 && this.Navigator.RangeStart * this.Navigator.TrackSize >= 0.0 && this.Navigator.RangeStart * this.Navigator.TrackSize <= this.upperLabelBounds[index2] && this.Navigator.RangeEnd * this.Navigator.TrackSize >= 0.0 && this.Navigator.RangeEnd * this.Navigator.TrackSize <= this.upperLabelBounds[index2])
          SfDateTimeRangeNavigator.SetLabelStyle(this.upperLabelBar, index2, this.HigherLevelBarStyle);
        else if (index2 != 0 && this.Navigator.RangeStart * this.Navigator.TrackSize >= this.upperLabelBounds[index2 - 1] && this.Navigator.RangeStart * this.Navigator.TrackSize <= this.upperLabelBounds[index2] && this.Navigator.RangeEnd * this.Navigator.TrackSize >= this.upperLabelBounds[index2 - 1] && this.Navigator.RangeEnd * this.Navigator.TrackSize <= this.upperLabelBounds[index2])
        {
          SfDateTimeRangeNavigator.SetLabelStyle(this.upperLabelBar, index2, this.HigherLevelBarStyle);
        }
        else
        {
          this.upperLabelBar.Children[index2].ClearValue(TextBlock.ForegroundProperty);
          (this.upperLabelBar.Children[index2] as TextBlock).Style = this.HigherLabelStyle;
        }
        if (index1 != -1 && this.Navigator.RangeEnd * this.Navigator.TrackSize > this.upperLabelBounds[index1] && index1 + 1 != this.upperLabelBar.Children.Count)
          SfDateTimeRangeNavigator.SetLabelStyle(this.upperLabelBar, index1 + 1, this.HigherLevelBarStyle);
      }
    }
    int index3 = -1;
    if (this.lowerLabelBar == null || this.lowerLabelBounds == null || this.lowerLabelBar.Children.Count != this.lowerLabelBounds.Count)
      return;
    for (int index4 = 0; index4 < this.lowerLabelBounds.Count; ++index4)
    {
      if (this.Navigator.RangeStart * this.Navigator.TrackSize < this.lowerLabelBounds[index4] && this.Navigator.RangeEnd * this.Navigator.TrackSize >= this.lowerLabelBounds[index4])
      {
        SfDateTimeRangeNavigator.SetLabelStyle(this.lowerLabelBar, index4, this.LowerLevelBarStyle);
        index3 = index4;
      }
      else if (index4 == 0 && this.Navigator.RangeStart * this.Navigator.TrackSize >= 0.0 && this.Navigator.RangeStart * this.Navigator.TrackSize <= this.lowerLabelBounds[index4] && this.Navigator.RangeEnd * this.Navigator.TrackSize >= 0.0 && this.Navigator.RangeEnd * this.Navigator.TrackSize <= this.lowerLabelBounds[index4])
        SfDateTimeRangeNavigator.SetLabelStyle(this.lowerLabelBar, index4, this.LowerLevelBarStyle);
      else if (index4 != 0 && this.Navigator.RangeStart * this.Navigator.TrackSize >= this.lowerLabelBounds[index4 - 1] && this.Navigator.RangeStart * this.Navigator.TrackSize <= this.lowerLabelBounds[index4] && this.Navigator.RangeEnd * this.Navigator.TrackSize >= this.lowerLabelBounds[index4 - 1] && this.Navigator.RangeEnd * this.Navigator.TrackSize <= this.lowerLabelBounds[index4])
      {
        SfDateTimeRangeNavigator.SetLabelStyle(this.lowerLabelBar, index4, this.LowerLevelBarStyle);
      }
      else
      {
        this.lowerLabelBar.Children[index4].ClearValue(TextBlock.ForegroundProperty);
        (this.lowerLabelBar.Children[index4] as TextBlock).Style = this.LowerLabelStyle;
      }
      if (index3 != -1 && this.Navigator.RangeEnd * this.Navigator.TrackSize > this.lowerLabelBounds[index3] && index3 + 1 != this.lowerLabelBar.Children.Count)
        SfDateTimeRangeNavigator.SetLabelStyle(this.lowerLabelBar, index3 + 1, this.LowerLevelBarStyle);
    }
  }

  private void LowerLabelBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (this.EnableDeferredUpdate)
    {
      this.timer = new DispatcherTimer();
      this.timer.Tick += new EventHandler(this.OnTimeout);
    }
    Point position = e.GetPosition((IInputElement) this.lowerLabelBar);
    if (this.lowerLabelBounds == null || this.lowerLabelBounds.Count <= 1)
      return;
    for (int index = 0; index < this.lowerLabelBounds.Count; ++index)
    {
      if (position.X > this.lowerLabelBounds[index] && index + 1 == this.lowerLabelBounds.Count)
      {
        this.LabelSelection(this.lowerLabelBounds[index], 1.0);
      }
      else
      {
        if (position.X > this.lowerLabelBounds[index] && position.X < this.lowerLabelBounds[index + 1])
        {
          this.LabelSelection(this.lowerLabelBounds[index], this.lowerLabelBounds[index + 1]);
          break;
        }
        if (position.X < this.lowerLabelBounds[index])
          this.LabelSelection(0.0, this.lowerLabelBounds[0]);
      }
    }
  }

  private void LabelSelection(double startleft, double endright)
  {
    double num = 1.0 * (endright / this.Navigator.TrackSize);
    this.Navigator.IsValueChangedTrigger = num == this.Navigator.RangeEnd;
    this.Navigator.RangeStart = 1.0 * (startleft / this.Navigator.TrackSize) == this.Navigator.Maximum ? 0.999 : 1.0 * (startleft / this.Navigator.TrackSize);
    this.Navigator.IsValueChangedTrigger = true;
    this.Navigator.RangeEnd = this.Navigator.RangeStart > num ? this.Navigator.Maximum : num;
    this.SetSelectedDataStyle();
  }

  private void UpperLabelBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (this.EnableDeferredUpdate)
    {
      this.timer = new DispatcherTimer();
      this.timer.Tick += new EventHandler(this.OnTimeout);
    }
    Point position = e.GetPosition((IInputElement) this.upperLabelBar);
    if (this.upperLabelBounds == null || this.upperLabelBounds.Count <= 1)
      return;
    for (int index = 0; index < this.upperLabelBounds.Count; ++index)
    {
      if (position.X > this.upperLabelBounds[index] && index + 1 == this.upperLabelBounds.Count)
      {
        this.LabelSelection(this.upperLabelBounds[index], 1.0);
      }
      else
      {
        if (position.X > this.upperLabelBounds[index] && position.X < this.upperLabelBounds[index + 1])
        {
          this.LabelSelection(this.upperLabelBounds[index], this.upperLabelBounds[index + 1]);
          break;
        }
        if (position.X < this.upperLabelBounds[index])
          this.LabelSelection(0.0, this.upperLabelBounds[0]);
      }
    }
  }

  private void LowerLabelBar_MouseLeave(object sender, MouseEventArgs e)
  {
    if (this.hover == null)
      return;
    (this.hover.Children[0] as Rectangle).Fill = (Brush) new SolidColorBrush(Colors.Transparent);
  }

  private void LowerLabelBar_MouseMove(object sender, MouseEventArgs e)
  {
    Point position = e.GetPosition((IInputElement) this.lowerLabelBar);
    if (this.lowerLabelBounds == null)
      return;
    for (int index = 0; index < this.lowerLabelBounds.Count; ++index)
    {
      if (index + 1 < this.lowerLabelBounds.Count && position.X > this.lowerLabelBounds[index] && position.X < this.lowerLabelBounds[index + 1])
      {
        Canvas.SetLeft((UIElement) (this.hover.Children[0] as Rectangle), this.lowerLabelBounds[index]);
        (this.hover.Children[0] as Rectangle).Height = this.Navigator.ActualHeight;
        (this.hover.Children[0] as Rectangle).Width = this.lowerLabelBounds[index + 1] - this.lowerLabelBounds[index];
        (this.hover.Children[0] as Rectangle).Fill = (Brush) new SolidColorBrush(Color.FromArgb((byte) 126, (byte) 0, (byte) 0, (byte) 0));
        this.hover.Opacity = 0.5;
        break;
      }
      if (position.X < this.lowerLabelBounds[index])
      {
        Canvas.SetLeft((UIElement) (this.hover.Children[0] as Rectangle), 0.0);
        (this.hover.Children[0] as Rectangle).Height = this.Navigator.ActualHeight;
        (this.hover.Children[0] as Rectangle).Width = this.lowerLabelBounds[0];
        (this.hover.Children[0] as Rectangle).Fill = (Brush) new SolidColorBrush(Color.FromArgb((byte) 126, (byte) 0, (byte) 0, (byte) 0));
        this.hover.Opacity = 0.5;
      }
    }
  }

  private void UpperLabelBar_MouseLeave(object sender, MouseEventArgs e)
  {
    if (this.hover == null)
      return;
    (this.hover.Children[0] as Rectangle).Fill = (Brush) new SolidColorBrush(Colors.Transparent);
  }

  private void UpperLabelBar_MouseMove(object sender, MouseEventArgs e)
  {
    Point position = e.GetPosition((IInputElement) this.upperLabelBar);
    if (this.upperLabelBounds == null)
      return;
    for (int index = 0; index < this.upperLabelBounds.Count; ++index)
    {
      if (index + 1 < this.upperLabelBounds.Count && position.X > this.upperLabelBounds[index] && position.X < this.upperLabelBounds[index + 1])
      {
        Canvas.SetLeft((UIElement) (this.hover.Children[0] as Rectangle), this.upperLabelBounds[index]);
        (this.hover.Children[0] as Rectangle).Height = this.Navigator.ActualHeight;
        (this.hover.Children[0] as Rectangle).Width = this.upperLabelBounds[index + 1] - this.upperLabelBounds[index];
        (this.hover.Children[0] as Rectangle).Fill = (Brush) new SolidColorBrush(Color.FromArgb((byte) 126, (byte) 0, (byte) 0, (byte) 0));
        this.hover.Opacity = 0.5;
        break;
      }
      if (position.X < this.upperLabelBounds[index])
      {
        Canvas.SetLeft((UIElement) (this.hover.Children[0] as Rectangle), 0.0);
        (this.hover.Children[0] as Rectangle).Height = this.Navigator.ActualHeight;
        (this.hover.Children[0] as Rectangle).Width = this.upperLabelBounds[0];
        (this.hover.Children[0] as Rectangle).Fill = (Brush) new SolidColorBrush(Color.FromArgb((byte) 126, (byte) 0, (byte) 0, (byte) 0));
        this.hover.Opacity = 0.5;
      }
    }
  }

  private void InsertLabels(string dockposition)
  {
    int index = 0;
    if (dockposition == "Lower" && this.lowerLabelBar != null)
    {
      double num1 = this.Navigator.TrackSize / this.totalNoofDays;
      double a = this.daysvalue[index] * num1;
      double num2 = this.LowerLevelBarStyle.LabelHorizontalAlignment == HorizontalAlignment.Center || this.LowerLevelBarStyle.LabelHorizontalAlignment == HorizontalAlignment.Stretch ? a / 2.0 - this.txtblockwidth / 2.0 : (this.LowerLevelBarStyle.LabelHorizontalAlignment != HorizontalAlignment.Left ? a - this.txtblockwidth : 0.0);
      this.labelElementBounds = new Rect[this.lowerLabelRecycler.Count];
      this.lowerLabelBounds = new ObservableCollection<double>();
      foreach (TextBlock element in this.lowerLabelRecycler)
      {
        element.Visibility = Visibility.Visible;
        if (num2 < 0.0)
          element.Visibility = Visibility.Collapsed;
        Canvas.SetLeft((UIElement) element, num2);
        Line line1 = this.lowerTickLineRecycler[index];
        line1.X1 = Math.Round(a);
        line1.X2 = Math.Round(a);
        line1.Y1 = 0.0;
        line1.Y2 = this.lowerBorder.DesiredSize.Height;
        line1.Style = this.LowerBarTickLineStyle;
        if (this.ShowGridLines)
        {
          Line line2 = this.lowerGridLineRecycler[index];
          line2.X1 = Math.Round(a);
          line2.X2 = Math.Round(a);
          line2.Y1 = 0.0;
          line2.Y2 = this.Navigator.ActualHeight;
          line2.Style = this.LowerBarGridLineStyle;
        }
        else
          this.lowerGridLineRecycler[index].ClearUIValues();
        this.lowerLabelBounds.Add(a);
        this.labelElementBounds[index] = new Rect(num2, 0.0, element.DesiredSize.Width, 17.0);
        if (index + 1 < this.daysvalue.Count)
          a = this.daysvalue[index + 1] * num1;
        num2 = this.LowerLevelBarStyle.LabelHorizontalAlignment == HorizontalAlignment.Center || this.LowerLevelBarStyle.LabelHorizontalAlignment == HorizontalAlignment.Stretch ? (a - this.daysvalue[index] * num1) / 2.0 - this.txtblockwidth / 2.0 + this.daysvalue[index] * num1 : (this.LowerLevelBarStyle.LabelHorizontalAlignment != HorizontalAlignment.Left ? a - this.txtblockwidth : this.daysvalue[index] * num1);
        if (index != 0 && this.daysvalue[index] * num1 - this.daysvalue[index - 1] * num1 < this.txtblockwidth)
          element.Visibility = Visibility.Collapsed;
        ++index;
      }
    }
    else
    {
      this.upperLabelBounds = new ObservableCollection<double>();
      double num3 = 0.0;
      if (this.XValues != null && (this.XValues as IList<DateTime>).Count > 0)
        num3 = (this.XValues as IList<DateTime>).Count != 1 ? this.Navigator.TrackSize / (this.maximumDateTimeValue.ToOADate() - this.minimumDateTimeValue.ToOADate()) : this.Navigator.TrackSize / this.maximumDateTimeValue.ToOADate();
      else if (Convert.ToString(this.Minimum) != "0" && Convert.ToString(this.Maximum) != "1")
        num3 = this.Navigator.TrackSize / (Convert.ToDateTime(this.Maximum, (IFormatProvider) CultureInfo.InvariantCulture).ToOADate() - Convert.ToDateTime(this.Minimum, (IFormatProvider) CultureInfo.InvariantCulture).ToOADate());
      double a = this.daysvalue[index] * num3;
      double num4 = this.HigherLevelBarStyle.LabelHorizontalAlignment == HorizontalAlignment.Center || this.HigherLevelBarStyle.LabelHorizontalAlignment == HorizontalAlignment.Stretch ? a / 2.0 - this.txtblockwidth / 2.0 : (this.HigherLevelBarStyle.LabelHorizontalAlignment != HorizontalAlignment.Left ? a - this.txtblockwidth : 0.0);
      this.labelElementBounds = new Rect[this.upperLabelRecycler.Count];
      foreach (TextBlock element in this.upperLabelRecycler)
      {
        element.Visibility = Visibility.Visible;
        if (num4 < 0.0)
          element.Visibility = Visibility.Collapsed;
        Canvas.SetLeft((UIElement) element, num4);
        Line line3 = this.upperTickLineRecycler[index];
        line3.X1 = a;
        line3.X2 = a;
        line3.Y1 = 0.0;
        line3.Y2 = this.uppperBorder.DesiredSize.Height;
        line3.Style = this.HigherBarTickLineStyle;
        Line line4 = this.upperGridLineRecycler[index];
        line4.X1 = Math.Round(a);
        line4.X2 = Math.Round(a);
        line4.Y1 = 0.0;
        line4.Y2 = this.Navigator.ActualHeight;
        if (this.HigherBarGridLineStyle != null)
          line4.Style = this.HigherBarGridLineStyle;
        else
          line4.ClearValue(FrameworkElement.StyleProperty);
        this.labelElementBounds[index] = new Rect(num4, 0.0, element.DesiredSize.Width, 17.0);
        this.upperLabelBounds.Add(a);
        if (index + 1 < this.daysvalue.Count)
          a = this.daysvalue[index + 1] * num3;
        num4 = this.HigherLevelBarStyle.LabelHorizontalAlignment == HorizontalAlignment.Center || this.HigherLevelBarStyle.LabelHorizontalAlignment == HorizontalAlignment.Stretch ? (a - this.daysvalue[index] * num3) / 2.0 - this.txtblockwidth / 2.0 + this.daysvalue[index] * num3 : (this.HigherLevelBarStyle.LabelHorizontalAlignment != HorizontalAlignment.Left ? a - this.txtblockwidth - line3.StrokeThickness : this.daysvalue[index] * num3 + line3.StrokeThickness);
        if (index != 0 && this.daysvalue[index] * num3 - this.daysvalue[index - 1] * num3 < this.txtblockwidth)
          element.Visibility = Visibility.Collapsed;
        ++index;
      }
    }
    this.CalculateSelectedData();
  }

  private void SetHourInterval(
    int hourinterval,
    string dockposition,
    ObservableCollection<string> formatter)
  {
    string empty1 = string.Empty;
    if (formatter != null && formatter.Count == 0)
      this.isFormatterEmpty = true;
    if (formatter != null && !this.isFormatterEmpty)
    {
      for (int index = 0; index < formatter.Count; ++index)
      {
        if (this.AddLabels(formatter[index].ToString(), dockposition, "Hour"))
        {
          empty1 = formatter[index].ToString();
          break;
        }
      }
    }
    this.daysvalue.Clear();
    this.txtblockwidth = 0.0;
    if (dockposition == "Lower")
      this.lowerBarLabels.Clear();
    else
      this.upperBarLabels.Clear();
    this.labelElementBounds = (Rect[]) null;
    string empty2 = string.Empty;
    DateTime currentDate = this.minimumDateTimeValue;
    switch (hourinterval)
    {
      case 0:
        while (currentDate <= this.maximumDateTimeValue)
        {
          if (formatter == null || this.isFormatterEmpty)
            empty2 = currentDate.ToString("hh tt", (IFormatProvider) CultureInfo.CurrentCulture);
          else if (!string.IsNullOrEmpty(empty1))
            empty2 = currentDate.ToString(empty1, (IFormatProvider) CultureInfo.CurrentCulture);
          if (dockposition == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) empty2
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) empty2
            });
          currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, 0, 0).AddHours(1.0);
          this.AddDaysValues(currentDate);
        }
        if (this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          break;
        }
        if (this.navigatorIntervals.Contains("Hour"))
        {
          this.SetHourInterval(1, dockposition, this.formatters[this.navigatorIntervals.IndexOf("Hour")]);
          break;
        }
        this.SetHourInterval(1, dockposition, (ObservableCollection<string>) null);
        break;
      case 1:
        while (currentDate <= this.maximumDateTimeValue)
        {
          if (formatter == null || this.isFormatterEmpty)
            empty2 = currentDate.ToString("hh tt", (IFormatProvider) CultureInfo.CurrentCulture);
          else if (!string.IsNullOrEmpty(empty1))
            empty2 = currentDate.ToString(empty1, (IFormatProvider) CultureInfo.CurrentCulture);
          if (dockposition == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) empty2
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) empty2
            });
          currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, 0, 0).AddHours(1.0);
          this.AddDaysValues(currentDate);
        }
        if (this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          break;
        }
        if (this.navigatorIntervals.Contains("Hour"))
        {
          this.SetHourInterval(2, dockposition, this.formatters[this.navigatorIntervals.IndexOf("Hour")]);
          break;
        }
        this.SetHourInterval(2, dockposition, (ObservableCollection<string>) null);
        break;
      case 2:
        while (currentDate <= this.maximumDateTimeValue)
        {
          if (formatter == null || this.isFormatterEmpty)
            empty2 = currentDate.ToString("hh tt", (IFormatProvider) CultureInfo.CurrentCulture);
          else if (!string.IsNullOrEmpty(empty1))
            empty2 = currentDate.ToString(empty1, (IFormatProvider) CultureInfo.CurrentCulture);
          if (dockposition == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) empty2
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) empty2
            });
          currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, 0, 0).AddHours(2.0);
          this.AddDaysValues(currentDate);
        }
        if (this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          break;
        }
        if (this.navigatorIntervals.Contains("Hour"))
        {
          this.SetHourInterval(3, dockposition, this.formatters[this.navigatorIntervals.IndexOf("Hour")]);
          break;
        }
        this.SetHourInterval(3, dockposition, (ObservableCollection<string>) null);
        break;
      case 3:
        while (currentDate <= this.maximumDateTimeValue)
        {
          if (formatter == null || this.isFormatterEmpty)
            empty2 = currentDate.ToString("ht", (IFormatProvider) CultureInfo.CurrentCulture);
          else if (!string.IsNullOrEmpty(empty1))
            empty2 = currentDate.ToString(empty1, (IFormatProvider) CultureInfo.CurrentCulture);
          if (dockposition == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) empty2
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) empty2
            });
          currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, 0, 0).AddHours(4.0);
          this.AddDaysValues(currentDate);
        }
        if (this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          break;
        }
        this.ClearLabels(dockposition);
        break;
    }
    if (!this.isFormatterEmpty)
      return;
    this.isFormatterEmpty = false;
  }

  private void SetDayInterval(
    int dayinterval,
    string dockposition,
    ObservableCollection<string> formatter)
  {
    string empty1 = string.Empty;
    if (formatter != null && formatter.Count == 0)
      this.isFormatterEmpty = true;
    if (formatter != null && !this.isFormatterEmpty)
    {
      for (int index = 0; index < formatter.Count; ++index)
      {
        if (this.AddLabels(formatter[index].ToString(), dockposition, "Day"))
        {
          empty1 = formatter[index].ToString();
          break;
        }
      }
    }
    this.daysvalue.Clear();
    this.txtblockwidth = 0.0;
    if (dockposition == "Lower")
      this.lowerBarLabels.Clear();
    else
      this.upperBarLabels.Clear();
    string empty2 = string.Empty;
    this.labelElementBounds = (Rect[]) null;
    DateTime currentDate = this.minimumDateTimeValue;
    switch (dayinterval)
    {
      case 0:
        while (currentDate <= this.maximumDateTimeValue)
        {
          if (formatter == null || this.isFormatterEmpty)
            empty2 = currentDate.ToString("dddd, MMMM d, yyyy", (IFormatProvider) CultureInfo.CurrentCulture);
          else if (!string.IsNullOrEmpty(empty1))
            empty2 = currentDate.ToString(empty1, (IFormatProvider) CultureInfo.CurrentCulture);
          if (dockposition == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) empty2
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) empty2
            });
          currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day).AddDays(1.0);
          this.AddDaysValues(currentDate);
        }
        if (dockposition == "Lower" && this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          if (this.Navigator.TrackSize - this.txtblockwidth * (double) this.lowerLabelBar.Children.Count > this.txtblockwidth / 2.0)
          {
            if (this.navigatorIntervals.Count == 0 || this.navigatorIntervals.Contains("Hour"))
              this.SetDayInterval(0, "Upper", (ObservableCollection<string>) null);
            if (this.navigatorIntervals.Count == 0 || this.navigatorIntervals.Contains("Hour"))
            {
              if (this.navigatorIntervals.Contains("Hour"))
              {
                this.SetHourInterval(0, "Lower", this.formatters[this.navigatorIntervals.IndexOf("Hour")]);
                break;
              }
              this.SetHourInterval(0, "Lower", (ObservableCollection<string>) null);
              break;
            }
            break;
          }
          break;
        }
        if (dockposition == "Upper" && this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          break;
        }
        if (this.navigatorIntervals.Contains("Day"))
        {
          this.SetDayInterval(1, dockposition, this.formatters[this.navigatorIntervals.IndexOf("Day")]);
          break;
        }
        this.SetDayInterval(1, dockposition, (ObservableCollection<string>) null);
        break;
      case 1:
        while (currentDate <= this.maximumDateTimeValue)
        {
          if (formatter == null || this.isFormatterEmpty)
            empty2 = currentDate.ToString("ddd, MMM d, yyyy", (IFormatProvider) CultureInfo.CurrentCulture);
          else if (!string.IsNullOrEmpty(empty1))
            empty2 = currentDate.ToString(empty1, (IFormatProvider) CultureInfo.CurrentCulture);
          if (dockposition == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) empty2
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) empty2
            });
          currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day).AddDays(1.0);
          this.AddDaysValues(currentDate);
        }
        if (dockposition == "Lower" && this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          break;
        }
        if (this.navigatorIntervals.Contains("Day"))
        {
          this.SetDayInterval(2, dockposition, this.formatters[this.navigatorIntervals.IndexOf("Day")]);
          break;
        }
        this.SetDayInterval(2, dockposition, (ObservableCollection<string>) null);
        break;
      case 2:
        while (currentDate <= this.maximumDateTimeValue)
        {
          if (formatter == null || this.isFormatterEmpty)
            empty2 = currentDate.ToString("dddd, d", (IFormatProvider) CultureInfo.CurrentCulture);
          else if (!string.IsNullOrEmpty(empty1))
            empty2 = currentDate.ToString(empty1, (IFormatProvider) CultureInfo.CurrentCulture);
          if (dockposition == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) empty2
            });
          currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day).AddDays(1.0);
          this.AddDaysValues(currentDate);
        }
        if (dockposition == "Lower" && this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          break;
        }
        if (this.navigatorIntervals.Contains("Day"))
        {
          this.SetDayInterval(3, dockposition, this.formatters[this.navigatorIntervals.IndexOf("Day")]);
          break;
        }
        this.SetDayInterval(3, dockposition, (ObservableCollection<string>) null);
        break;
      case 3:
        while (currentDate <= this.maximumDateTimeValue)
        {
          if (formatter == null || this.isFormatterEmpty)
            empty2 = currentDate.Day.ToString();
          else if (!string.IsNullOrEmpty(empty1))
            empty2 = currentDate.ToString(empty1, (IFormatProvider) CultureInfo.CurrentCulture);
          if (dockposition == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) empty2
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) empty2
            });
          currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day).AddDays(1.0);
          this.AddDaysValues(currentDate);
        }
        if (this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          break;
        }
        if (this.navigatorIntervals.Count == 0 || this.navigatorIntervals.Contains("Week"))
        {
          if (this.navigatorIntervals.Contains("Week"))
          {
            this.SetWeekInterval(0, dockposition, this.formatters[this.navigatorIntervals.IndexOf("Week")]);
            break;
          }
          this.SetWeekInterval(0, dockposition, (ObservableCollection<string>) null);
          break;
        }
        this.ClearLabels(dockposition);
        break;
    }
    if (!this.isFormatterEmpty)
      return;
    this.isFormatterEmpty = false;
  }

  private bool AddLabels(string format, string dockposition, string interval)
  {
    DateTime currentDate = this.minimumDateTimeValue;
    string empty = string.Empty;
    while (currentDate <= this.maximumDateTimeValue)
    {
      string str = currentDate.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture);
      if (dockposition == "Lower")
        this.lowerBarLabels.Add(new ChartAxisLabel()
        {
          LabelContent = (object) str
        });
      else
        this.upperBarLabels.Add(new ChartAxisLabel()
        {
          LabelContent = (object) str
        });
      switch (interval)
      {
        case "Day":
          currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day).AddDays(1.0);
          break;
        case "Year":
          if ((currentDate - this.minimumDateTimeValue).TotalDays != 0.0)
            this.daysvalue.Add((currentDate - this.minimumDateTimeValue).TotalDays);
          currentDate = new DateTime(currentDate.Year + 1, 1, 1);
          break;
        case "Quarter":
          if (currentDate.Month >= 1 && currentDate.Month <= 3)
          {
            currentDate = new DateTime(currentDate.Year, 4, 1);
            break;
          }
          if (currentDate.Month >= 4 && currentDate.Month <= 6)
          {
            currentDate = new DateTime(currentDate.Year, 7, 1);
            break;
          }
          if (currentDate.Month >= 7 && currentDate.Month <= 9)
          {
            currentDate = new DateTime(currentDate.Year, 10, 1);
            break;
          }
          if (currentDate.Month >= 10 && currentDate.Month <= 12)
          {
            currentDate = new DateTime(currentDate.Year + 1, 1, 1);
            break;
          }
          break;
        case "Month":
          currentDate = !(currentDate == this.minimumDateTimeValue) ? currentDate.AddMonths(1) : (currentDate.Month != 12 ? new DateTime(currentDate.Year, currentDate.Month + 1, 1) : new DateTime(currentDate.Year + 1, 1, 1));
          break;
        case "Week":
          if (currentDate.DayOfWeek != DayOfWeek.Monday)
          {
            while (currentDate.DayOfWeek != DayOfWeek.Monday)
              currentDate = currentDate.AddDays(1.0);
            currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
            break;
          }
          currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
          currentDate = currentDate.AddDays(7.0);
          break;
        default:
          currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, 0, 0).AddHours(1.0);
          break;
      }
      if (interval == "Year")
        this.daysvalue.Add((this.maximumDateTimeValue - this.minimumDateTimeValue).TotalDays);
      else
        this.AddDaysValues(currentDate);
    }
    if (this.GenerateLabelContainers(dockposition))
    {
      this.ClearLabels(dockposition);
      return true;
    }
    this.ClearLabels(dockposition);
    return false;
  }

  private void SetWeekInterval(
    int weekinterval,
    string dockposition,
    ObservableCollection<string> formatter)
  {
    string empty1 = string.Empty;
    if (formatter != null)
    {
      for (int index = 0; index < formatter.Count; ++index)
      {
        if (this.AddLabels(formatter[index].ToString(), dockposition, "Week"))
        {
          empty1 = formatter[index].ToString();
          break;
        }
      }
    }
    this.txtblockwidth = 0.0;
    this.daysvalue.Clear();
    if (dockposition == "Lower")
      this.lowerBarLabels.Clear();
    else
      this.upperBarLabels.Clear();
    this.labelElementBounds = (Rect[]) null;
    DateTime dateTime = this.minimumDateTimeValue;
    string empty2 = string.Empty;
    while (dateTime.DayOfWeek != DayOfWeek.Monday)
      dateTime = dateTime.AddDays(-1.0);
    switch (weekinterval)
    {
      case 0:
        while (dateTime <= this.maximumDateTimeValue)
        {
          string str = string.IsNullOrEmpty(empty1) ? ChartLocalizationResourceAccessor.Instance.GetString("Week") + (object) SfDateTimeRangeNavigator.GetWeekNumber(dateTime) + dateTime.ToString(" MMMM, yyyy", (IFormatProvider) CultureInfo.CurrentCulture) : dateTime.ToString(empty1, (IFormatProvider) CultureInfo.CurrentCulture);
          if (dockposition == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          if (dateTime.DayOfWeek != DayOfWeek.Monday)
          {
            while (dateTime.DayOfWeek != DayOfWeek.Monday)
              dateTime = dateTime.AddDays(1.0);
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
          }
          else
          {
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            dateTime = dateTime.AddDays(7.0);
          }
          this.AddDaysValues(dateTime);
        }
        if (dockposition == "Lower" && this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          if (this.Navigator.TrackSize - this.txtblockwidth * (double) this.lowerLabelBar.Children.Count <= this.txtblockwidth / 2.0)
            break;
          if (this.navigatorIntervals.Count == 0 || this.navigatorIntervals.Contains("Day"))
            this.SetWeekInterval(0, "Upper", (ObservableCollection<string>) null);
          if (this.navigatorIntervals.Contains("Day"))
          {
            this.SetDayInterval(0, "Lower", this.formatters[this.navigatorIntervals.IndexOf("Day")]);
            break;
          }
          if (this.navigatorIntervals.Contains("Hour"))
          {
            this.SetHourInterval(0, "Lower", this.formatters[this.navigatorIntervals.IndexOf("Hour")]);
            break;
          }
          if (this.navigatorIntervals.Count != 0)
            break;
          this.SetDayInterval(0, "Lower", (ObservableCollection<string>) null);
          break;
        }
        if (dockposition == "Upper" && this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          break;
        }
        if (this.navigatorIntervals.Contains("Week"))
        {
          this.SetWeekInterval(1, dockposition, this.formatters[this.navigatorIntervals.IndexOf("Week")]);
          break;
        }
        this.SetWeekInterval(1, dockposition, (ObservableCollection<string>) null);
        break;
      case 1:
        while (dateTime <= this.maximumDateTimeValue)
        {
          string str = string.IsNullOrEmpty(empty1) ? ChartLocalizationResourceAccessor.Instance.GetString("Week") + (object) SfDateTimeRangeNavigator.GetWeekNumber(dateTime) + dateTime.ToString(" MMMM, yyyy", (IFormatProvider) CultureInfo.CurrentCulture) : dateTime.ToString(empty1, (IFormatProvider) CultureInfo.CurrentCulture);
          if (dockposition == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          if (dateTime.DayOfWeek != DayOfWeek.Monday)
          {
            while (dateTime.DayOfWeek != DayOfWeek.Monday)
              dateTime = dateTime.AddDays(1.0);
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
          }
          else
          {
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            dateTime = dateTime.AddDays(7.0);
          }
          this.AddDaysValues(dateTime);
        }
        if (this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          break;
        }
        if (this.navigatorIntervals.Contains("Week"))
        {
          this.SetWeekInterval(2, dockposition, this.formatters[this.navigatorIntervals.IndexOf("Week")]);
          break;
        }
        this.SetWeekInterval(2, dockposition, (ObservableCollection<string>) null);
        break;
      case 2:
        while (dateTime <= this.maximumDateTimeValue)
        {
          string str = string.IsNullOrEmpty(empty1) ? ChartLocalizationResourceAccessor.Instance.GetString("Week") + (object) SfDateTimeRangeNavigator.GetWeekNumber(dateTime) : dateTime.ToString(empty1, (IFormatProvider) CultureInfo.CurrentCulture);
          if (dockposition == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          if (dateTime.DayOfWeek != DayOfWeek.Monday)
          {
            while (dateTime.DayOfWeek != DayOfWeek.Monday)
              dateTime = dateTime.AddDays(1.0);
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
          }
          else
          {
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            dateTime = dateTime.AddDays(7.0);
          }
          this.AddDaysValues(dateTime);
        }
        if (this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          break;
        }
        if (this.navigatorIntervals.Contains("Week"))
        {
          this.SetWeekInterval(3, dockposition, this.formatters[this.navigatorIntervals.IndexOf("Week")]);
          break;
        }
        this.SetWeekInterval(3, dockposition, (ObservableCollection<string>) null);
        break;
      case 3:
        while (dateTime <= this.maximumDateTimeValue)
        {
          string str = string.IsNullOrEmpty(empty1) ? ChartLocalizationResourceAccessor.Instance.GetString("W") + (object) SfDateTimeRangeNavigator.GetWeekNumber(dateTime) : dateTime.ToString(empty1, (IFormatProvider) CultureInfo.CurrentCulture);
          if (dockposition == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          if (dateTime.DayOfWeek != DayOfWeek.Monday)
          {
            while (dateTime.DayOfWeek != DayOfWeek.Monday)
              dateTime = dateTime.AddDays(1.0);
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
          }
          else
          {
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            dateTime = dateTime.AddDays(7.0);
          }
          this.AddDaysValues(dateTime);
        }
        if (this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          break;
        }
        if (this.navigatorIntervals.Count == 0 || this.navigatorIntervals.Contains("Month"))
        {
          if (this.navigatorIntervals.Contains("Month"))
          {
            this.SetMonthInterval(0, dockposition, this.formatters[this.navigatorIntervals.IndexOf("Month")]);
            break;
          }
          this.SetMonthInterval(0, dockposition, (ObservableCollection<string>) null);
          break;
        }
        this.ClearLabels(dockposition);
        break;
    }
  }

  private void SetMonthInterval(
    int monthinterval,
    string dockposition,
    ObservableCollection<string> formatter)
  {
    string empty = string.Empty;
    if (formatter != null && formatter.Count == 0)
      this.isFormatterEmpty = true;
    if (formatter != null && !this.isFormatterEmpty)
    {
      for (int index = 0; index < formatter.Count; ++index)
      {
        if (this.AddLabels(formatter[index].ToString(), dockposition, "Month"))
        {
          empty = formatter[index].ToString();
          break;
        }
      }
    }
    this.txtblockwidth = 0.0;
    this.daysvalue.Clear();
    if (dockposition == "Lower")
      this.lowerBarLabels.Clear();
    else
      this.upperBarLabels.Clear();
    this.labelElementBounds = (Rect[]) null;
    DateTime currentDate = this.minimumDateTimeValue;
    string str = string.Empty;
    switch (monthinterval)
    {
      case 0:
        while (currentDate <= this.maximumDateTimeValue)
        {
          if (formatter == null || this.isFormatterEmpty)
            str = currentDate.ToString("MMMM, yyyy", (IFormatProvider) CultureInfo.CurrentCulture);
          else if (!string.IsNullOrEmpty(empty))
            str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
          if (dockposition == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          currentDate = !(currentDate == this.minimumDateTimeValue) ? currentDate.AddMonths(1) : (currentDate.Month != 12 ? new DateTime(currentDate.Year, currentDate.Month + 1, 1) : new DateTime(currentDate.Year + 1, 1, 1));
          this.AddDaysValues(currentDate);
        }
        if (dockposition == "Lower" && this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          if (this.Navigator.TrackSize - this.txtblockwidth * (double) this.lowerLabelBar.Children.Count > this.txtblockwidth * 30.0)
          {
            if (this.navigatorIntervals.Count == 0 || this.navigatorIntervals.Contains("Week"))
            {
              if (this.navigatorIntervals.Contains("Month"))
                this.SetMonthInterval(0, "Upper", this.formatters[this.navigatorIntervals.IndexOf("Month")]);
              else
                this.SetMonthInterval(0, "Upper", (ObservableCollection<string>) null);
            }
            if (this.navigatorIntervals.Contains("Week"))
            {
              this.SetWeekInterval(0, "Lower", this.formatters[this.navigatorIntervals.IndexOf("Week")]);
              break;
            }
            if (this.navigatorIntervals.Contains("Day"))
            {
              this.SetDayInterval(0, "Lower", this.formatters[this.navigatorIntervals.IndexOf("Day")]);
              break;
            }
            if (this.navigatorIntervals.Count == 0)
            {
              this.SetWeekInterval(0, "Lower", (ObservableCollection<string>) null);
              break;
            }
            break;
          }
          break;
        }
        if (dockposition == "Upper" && this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          break;
        }
        if (this.navigatorIntervals.Contains("Month"))
        {
          this.SetMonthInterval(1, dockposition, this.formatters[this.navigatorIntervals.IndexOf("Month")]);
          break;
        }
        this.SetMonthInterval(1, dockposition, (ObservableCollection<string>) null);
        break;
      case 1:
        while (currentDate <= this.maximumDateTimeValue)
        {
          if (formatter == null || this.isFormatterEmpty)
            str = currentDate.ToString("MMMM", (IFormatProvider) CultureInfo.CurrentCulture);
          else if (!string.IsNullOrEmpty(empty))
            str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
          if (dockposition == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          currentDate = !(currentDate == this.minimumDateTimeValue) ? currentDate.AddMonths(1) : (currentDate.Month != 12 ? new DateTime(currentDate.Year, currentDate.Month + 1, 1) : new DateTime(currentDate.Year + 1, 1, 1));
          this.AddDaysValues(currentDate);
        }
        if (this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          break;
        }
        if (this.navigatorIntervals.Contains("Month"))
        {
          this.SetMonthInterval(2, dockposition, this.formatters[this.navigatorIntervals.IndexOf("Month")]);
          break;
        }
        this.SetMonthInterval(2, dockposition, (ObservableCollection<string>) null);
        break;
      case 2:
        while (currentDate <= this.maximumDateTimeValue)
        {
          if (formatter == null || this.isFormatterEmpty)
            str = currentDate.ToString("MMM", (IFormatProvider) CultureInfo.CurrentCulture);
          else if (!string.IsNullOrEmpty(empty))
            str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
          if (dockposition == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          currentDate = !(currentDate == this.minimumDateTimeValue) ? currentDate.AddMonths(1) : (currentDate.Month != 12 ? new DateTime(currentDate.Year, currentDate.Month + 1, 1) : new DateTime(currentDate.Year + 1, 1, 1));
          this.AddDaysValues(currentDate);
        }
        if (this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          break;
        }
        if (this.navigatorIntervals.Contains("Month"))
        {
          this.SetMonthInterval(3, dockposition, this.formatters[this.navigatorIntervals.IndexOf("Month")]);
          break;
        }
        this.SetMonthInterval(3, dockposition, (ObservableCollection<string>) null);
        break;
      case 3:
        while (currentDate <= this.maximumDateTimeValue)
        {
          if (formatter == null || this.isFormatterEmpty)
            str = currentDate.ToString("MMM", (IFormatProvider) CultureInfo.CurrentCulture).Substring(0, 1);
          else if (!string.IsNullOrEmpty(empty))
            str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
          if (dockposition == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          currentDate = !(currentDate == this.minimumDateTimeValue) ? currentDate.AddMonths(1) : (currentDate.Month != 12 ? new DateTime(currentDate.Year, currentDate.Month + 1, 1) : new DateTime(currentDate.Year + 1, 1, 1));
          this.AddDaysValues(currentDate);
        }
        if (this.GenerateLabelContainers(dockposition))
        {
          this.InsertLabels(dockposition);
          break;
        }
        if (this.navigatorIntervals.Count == 0 || this.navigatorIntervals.Contains("Quarter"))
        {
          if (this.navigatorIntervals.Contains("Quarter"))
          {
            this.SetQuarterInterval(0, dockposition, this.formatters[this.navigatorIntervals.IndexOf("Quarter")]);
            break;
          }
          this.SetQuarterInterval(0, dockposition, (ObservableCollection<string>) null);
          break;
        }
        this.ClearLabels(dockposition);
        break;
    }
    if (!this.isFormatterEmpty)
      return;
    this.isFormatterEmpty = false;
  }

  private void ClearLabels(string dockposition)
  {
    if (dockposition == "Lower")
    {
      this.lowerBarLabels.Clear();
      if (this.lowerLabelBounds != null)
        this.lowerLabelBounds.Clear();
    }
    else
    {
      this.upperBarLabels.Clear();
      if (this.upperLabelBounds != null)
        this.upperLabelBounds.Clear();
    }
    this.GenerateLabelContainers(dockposition);
  }

  private void SetQuarterInterval(
    int quarterinterval,
    string dockpostion,
    ObservableCollection<string> formatter)
  {
    string empty = string.Empty;
    if (formatter != null && formatter.Count == 0)
      this.isFormatterEmpty = true;
    if (formatter != null && !this.isFormatterEmpty)
    {
      for (int index = 0; index < formatter.Count; ++index)
      {
        if (this.AddLabels(formatter[index].ToString(), dockpostion, "Quarter"))
        {
          empty = formatter[index].ToString();
          break;
        }
      }
    }
    this.txtblockwidth = 0.0;
    this.daysvalue.Clear();
    if (dockpostion == "Lower")
      this.lowerBarLabels.Clear();
    else
      this.upperBarLabels.Clear();
    this.labelElementBounds = (Rect[]) null;
    DateTime currentDate = this.minimumDateTimeValue;
    string str = string.Empty;
    switch (quarterinterval)
    {
      case 0:
        while (currentDate <= this.maximumDateTimeValue)
        {
          if (currentDate.Month >= 1 && currentDate.Month <= 3)
          {
            if (formatter == null || this.isFormatterEmpty)
              str = $"{ChartLocalizationResourceAccessor.Instance.GetString("Quarter")} 1, {currentDate.ToString("yyyy", (IFormatProvider) CultureInfo.CurrentCulture)}";
            else if (!string.IsNullOrEmpty(empty))
              str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
            currentDate = new DateTime(currentDate.Year, 4, 1);
          }
          else if (currentDate.Month >= 4 && currentDate.Month <= 6)
          {
            if (formatter == null || this.isFormatterEmpty)
              str = $"{ChartLocalizationResourceAccessor.Instance.GetString("Quarter")} 2, {currentDate.ToString("yyyy", (IFormatProvider) CultureInfo.CurrentCulture)}";
            else if (!string.IsNullOrEmpty(empty))
              str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
            currentDate = new DateTime(currentDate.Year, 7, 1);
          }
          else if (currentDate.Month >= 7 && currentDate.Month <= 9)
          {
            if (formatter == null || this.isFormatterEmpty)
              str = $"{ChartLocalizationResourceAccessor.Instance.GetString("Quarter")} 3, {currentDate.ToString("yyyy", (IFormatProvider) CultureInfo.CurrentCulture)}";
            else if (!string.IsNullOrEmpty(empty))
              str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
            currentDate = new DateTime(currentDate.Year, 10, 1);
          }
          else if (currentDate.Month >= 10 && currentDate.Month <= 12)
          {
            if (formatter == null || this.isFormatterEmpty)
              str = $"{ChartLocalizationResourceAccessor.Instance.GetString("Quarter")} 4, {currentDate.ToString("yyyy", (IFormatProvider) CultureInfo.CurrentCulture)}";
            else if (!string.IsNullOrEmpty(empty))
              str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
            currentDate = new DateTime(currentDate.Year + 1, 1, 1);
          }
          if (dockpostion == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          this.AddDaysValues(currentDate);
        }
        if (dockpostion == "Lower" && this.GenerateLabelContainers(dockpostion))
        {
          this.InsertLabels(dockpostion);
          if (this.Navigator.TrackSize - this.txtblockwidth * (double) this.lowerLabelBar.Children.Count > this.txtblockwidth / 2.0)
          {
            if (this.navigatorIntervals.Count == 0 || this.navigatorIntervals.Contains("Month"))
              this.SetQuarterInterval(0, "Upper", (ObservableCollection<string>) null);
            if (this.navigatorIntervals.Contains("Month"))
            {
              this.SetMonthInterval(0, "Lower", this.formatters[this.navigatorIntervals.IndexOf("Month")]);
              break;
            }
            if (this.navigatorIntervals.Contains("Week"))
            {
              this.SetWeekInterval(0, "Lower", this.formatters[this.navigatorIntervals.IndexOf("Week")]);
              break;
            }
            if (this.navigatorIntervals.Contains("Day"))
            {
              this.SetDayInterval(0, "Lower", this.formatters[this.navigatorIntervals.IndexOf("Day")]);
              break;
            }
            if (this.navigatorIntervals.Count == 0)
            {
              this.SetMonthInterval(0, "Lower", (ObservableCollection<string>) null);
              break;
            }
            break;
          }
          break;
        }
        if (dockpostion == "Upper" && this.GenerateLabelContainers(dockpostion))
        {
          this.InsertLabels(dockpostion);
          break;
        }
        if (this.navigatorIntervals.Contains("Quarter"))
        {
          this.SetQuarterInterval(1, dockpostion, this.formatters[this.navigatorIntervals.IndexOf("Quarter")]);
          break;
        }
        this.SetQuarterInterval(1, dockpostion, (ObservableCollection<string>) null);
        break;
      case 1:
        while (currentDate <= this.maximumDateTimeValue)
        {
          if (currentDate.Month >= 1 && currentDate.Month <= 3)
          {
            if (formatter == null || this.isFormatterEmpty)
              str = $"{ChartLocalizationResourceAccessor.Instance.GetString("Quarter")} 1, {currentDate.ToString("yy", (IFormatProvider) CultureInfo.CurrentCulture)}";
            else if (!string.IsNullOrEmpty(empty))
              str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
            currentDate = new DateTime(currentDate.Year, 4, 1);
          }
          else if (currentDate.Month >= 4 && currentDate.Month <= 6)
          {
            if (formatter == null || this.isFormatterEmpty)
              str = $"{ChartLocalizationResourceAccessor.Instance.GetString("Quarter")} 2, {currentDate.ToString("yy", (IFormatProvider) CultureInfo.CurrentCulture)}";
            else if (!string.IsNullOrEmpty(empty))
              str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
            currentDate = new DateTime(currentDate.Year, 7, 1);
          }
          else if (currentDate.Month >= 7 && currentDate.Month <= 9)
          {
            if (formatter == null || this.isFormatterEmpty)
              str = $"{ChartLocalizationResourceAccessor.Instance.GetString("Quarter")} 3, {currentDate.ToString("yy", (IFormatProvider) CultureInfo.CurrentCulture)}";
            else if (!string.IsNullOrEmpty(empty))
              str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
            currentDate = new DateTime(currentDate.Year, 10, 1);
          }
          else if (currentDate.Month >= 10 && currentDate.Month <= 12)
          {
            if (formatter == null || this.isFormatterEmpty)
              str = $"{ChartLocalizationResourceAccessor.Instance.GetString("Quarter")} 4, {currentDate.ToString("yy", (IFormatProvider) CultureInfo.CurrentCulture)}";
            else if (!string.IsNullOrEmpty(empty))
              str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
            currentDate = new DateTime(currentDate.Year + 1, 1, 1);
          }
          if (dockpostion == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          this.AddDaysValues(currentDate);
        }
        if (this.GenerateLabelContainers(dockpostion))
        {
          this.InsertLabels(dockpostion);
          break;
        }
        if (this.navigatorIntervals.Contains("Quarter"))
        {
          this.SetQuarterInterval(2, dockpostion, this.formatters[this.navigatorIntervals.IndexOf("Quarter")]);
          break;
        }
        this.SetQuarterInterval(2, dockpostion, (ObservableCollection<string>) null);
        break;
      case 2:
        while (currentDate <= this.maximumDateTimeValue)
        {
          if (currentDate.Month >= 1 && currentDate.Month <= 3)
          {
            if (formatter == null || this.isFormatterEmpty)
              str = $"{ChartLocalizationResourceAccessor.Instance.GetString("Q")}1, {currentDate.ToString("yyyy", (IFormatProvider) CultureInfo.CurrentCulture)}";
            else if (!string.IsNullOrEmpty(empty))
              str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
            currentDate = new DateTime(currentDate.Year, 4, 1);
          }
          else if (currentDate.Month >= 4 && currentDate.Month <= 6)
          {
            if (formatter == null || this.isFormatterEmpty)
              str = $"{ChartLocalizationResourceAccessor.Instance.GetString("Q")}2, {currentDate.ToString("yyyy", (IFormatProvider) CultureInfo.CurrentCulture)}";
            else if (!string.IsNullOrEmpty(empty))
              str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
            currentDate = new DateTime(currentDate.Year, 7, 1);
          }
          else if (currentDate.Month >= 7 && currentDate.Month <= 9)
          {
            if (formatter == null || this.isFormatterEmpty)
              str = $"{ChartLocalizationResourceAccessor.Instance.GetString("Q")}3, {currentDate.ToString("yyyy", (IFormatProvider) CultureInfo.CurrentCulture)}";
            else if (!string.IsNullOrEmpty(empty))
              str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
            currentDate = new DateTime(currentDate.Year, 10, 1);
          }
          else if (currentDate.Month >= 10 && currentDate.Month <= 12)
          {
            str = formatter == null || this.isFormatterEmpty ? $"{ChartLocalizationResourceAccessor.Instance.GetString("Q")}4, {currentDate.ToString("yyyy", (IFormatProvider) CultureInfo.CurrentCulture)}" : currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
            currentDate = new DateTime(currentDate.Year + 1, 1, 1);
          }
          if (dockpostion == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          this.AddDaysValues(currentDate);
        }
        if (this.GenerateLabelContainers(dockpostion))
        {
          this.InsertLabels(dockpostion);
          break;
        }
        if (this.navigatorIntervals.Contains("Quarter"))
        {
          this.SetQuarterInterval(3, dockpostion, this.formatters[this.navigatorIntervals.IndexOf("Quarter")]);
          break;
        }
        this.SetQuarterInterval(3, dockpostion, (ObservableCollection<string>) null);
        break;
      case 3:
        while (currentDate <= this.maximumDateTimeValue)
        {
          if (currentDate.Month >= 1 && currentDate.Month <= 3)
          {
            if (formatter == null || this.isFormatterEmpty)
              str = ChartLocalizationResourceAccessor.Instance.GetString("Q") + "1";
            else if (!string.IsNullOrEmpty(empty))
              str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
            currentDate = new DateTime(currentDate.Year, 4, 1);
          }
          else if (currentDate.Month >= 4 && currentDate.Month <= 6)
          {
            if (formatter == null || this.isFormatterEmpty)
              str = ChartLocalizationResourceAccessor.Instance.GetString("Q") + "2";
            else if (!string.IsNullOrEmpty(empty))
              str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
            currentDate = new DateTime(currentDate.Year, 7, 1);
          }
          else if (currentDate.Month >= 7 && currentDate.Month <= 9)
          {
            if (formatter == null || this.isFormatterEmpty)
              str = ChartLocalizationResourceAccessor.Instance.GetString("Q") + "3";
            else if (!string.IsNullOrEmpty(empty))
              str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
            currentDate = new DateTime(currentDate.Year, 10, 1);
          }
          else if (currentDate.Month >= 10 && currentDate.Month <= 12)
          {
            if (formatter == null || this.isFormatterEmpty)
              str = ChartLocalizationResourceAccessor.Instance.GetString("Q") + "4";
            else if (!string.IsNullOrEmpty(empty))
              str = currentDate.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture);
            currentDate = new DateTime(currentDate.Year + 1, 1, 1);
          }
          if (dockpostion == "Lower")
            this.lowerBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          else
            this.upperBarLabels.Add(new ChartAxisLabel()
            {
              LabelContent = (object) str
            });
          this.AddDaysValues(currentDate);
        }
        if (this.GenerateLabelContainers(dockpostion))
        {
          this.InsertLabels(dockpostion);
          break;
        }
        if (this.navigatorIntervals.Count == 0 || this.navigatorIntervals.Contains("Year"))
        {
          this.ClearLabels("Upper");
          if (this.navigatorIntervals.Contains("Year"))
          {
            this.SetYearInterval(0, "Lower", this.formatters[this.navigatorIntervals.IndexOf("Year")]);
            break;
          }
          this.SetYearInterval(0, "Lower", (ObservableCollection<string>) null);
          break;
        }
        this.ClearLabels(dockpostion);
        break;
    }
    if (!this.isFormatterEmpty)
      return;
    this.isFormatterEmpty = false;
  }

  private void SetYearInterval(
    int yearInterval,
    string dockposition,
    ObservableCollection<string> formatter)
  {
    if (this.isMinMaxSet)
    {
      bool flag = false;
      string empty = string.Empty;
      if (formatter != null && formatter.Count == 0)
        this.isFormatterEmpty = true;
      if (formatter != null && !this.isFormatterEmpty)
      {
        for (int index = 0; index < formatter.Count; ++index)
        {
          if (this.AddLabels(formatter[index].ToString(), dockposition, "Year"))
          {
            empty = formatter[index].ToString();
            break;
          }
        }
      }
      if (dockposition == "Upper")
        this.upperBarLabels.Clear();
      else
        this.lowerBarLabels.Clear();
      this.daysvalue.Clear();
      DateTime dateTime = this.minimumDateTimeValue;
      switch (yearInterval)
      {
        case 0:
          for (; dateTime <= this.maximumDateTimeValue; dateTime = new DateTime(dateTime.Year + 1, 1, 1))
          {
            if (dockposition == "Upper")
            {
              if (formatter == null || this.isFormatterEmpty)
                this.upperBarLabels.Add(new ChartAxisLabel()
                {
                  LabelContent = (object) dateTime.ToString("yyyy", (IFormatProvider) CultureInfo.CurrentCulture)
                });
              else if (!string.IsNullOrEmpty(empty))
                this.upperBarLabels.Add(new ChartAxisLabel()
                {
                  LabelContent = (object) dateTime.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture)
                });
            }
            else if (formatter == null || this.isFormatterEmpty)
              this.lowerBarLabels.Add(new ChartAxisLabel()
              {
                LabelContent = (object) dateTime.ToString("yyyy", (IFormatProvider) CultureInfo.CurrentCulture)
              });
            else if (!string.IsNullOrEmpty(empty))
              this.lowerBarLabels.Add(new ChartAxisLabel()
              {
                LabelContent = (object) dateTime.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture)
              });
            if ((dateTime - this.minimumDateTimeValue).TotalDays != 0.0)
              this.daysvalue.Add((dateTime - this.minimumDateTimeValue).TotalDays);
          }
          break;
        case 1:
          while (dateTime <= this.maximumDateTimeValue)
          {
            if (dockposition == "Upper")
            {
              if (formatter == null || this.isFormatterEmpty)
                this.upperBarLabels.Add(new ChartAxisLabel()
                {
                  LabelContent = (object) dateTime.ToString("yy", (IFormatProvider) CultureInfo.CurrentCulture)
                });
              else if (!string.IsNullOrEmpty(empty))
                this.upperBarLabels.Add(new ChartAxisLabel()
                {
                  LabelContent = (object) dateTime.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture)
                });
            }
            else if (formatter == null || this.isFormatterEmpty)
              this.lowerBarLabels.Add(new ChartAxisLabel()
              {
                LabelContent = (object) dateTime.ToString("yy", (IFormatProvider) CultureInfo.CurrentCulture)
              });
            else if (!string.IsNullOrEmpty(empty))
              this.lowerBarLabels.Add(new ChartAxisLabel()
              {
                LabelContent = (object) dateTime.ToString(empty, (IFormatProvider) CultureInfo.CurrentCulture)
              });
            if ((dateTime - this.minimumDateTimeValue).TotalDays != 0.0)
              this.daysvalue.Add((dateTime - this.minimumDateTimeValue).TotalDays);
            dateTime = new DateTime(dateTime.Year + 1, 1, 1);
            flag = true;
          }
          break;
      }
      this.daysvalue.Add((this.maximumDateTimeValue - this.minimumDateTimeValue).TotalDays);
      if (this.GenerateLabelContainers(dockposition))
        this.InsertLabels(dockposition);
      else if (!flag)
      {
        if (this.navigatorIntervals.Contains("Year"))
          this.SetYearInterval(1, dockposition, this.formatters[this.navigatorIntervals.IndexOf("Year")]);
        else
          this.SetYearInterval(1, dockposition, (ObservableCollection<string>) null);
      }
      else
        this.ClearLabels(dockposition);
    }
    if (this.isFormatterEmpty)
      this.isFormatterEmpty = false;
    if (this.navigatorIntervals.Contains("Quarter") && this.dockPosition != "Lower")
      this.SetQuarterInterval(0, this.dockPosition, this.formatters[this.navigatorIntervals.IndexOf("Quarter")]);
    else if (this.navigatorIntervals.Contains("Month") && this.dockPosition != "Lower")
      this.SetMonthInterval(0, this.dockPosition, this.formatters[this.navigatorIntervals.IndexOf("Month")]);
    else if (this.navigatorIntervals.Contains("Week") && this.dockPosition != "Lower")
      this.SetWeekInterval(0, this.dockPosition, this.formatters[this.navigatorIntervals.IndexOf("Week")]);
    else if (this.navigatorIntervals.Contains("Day") && this.dockPosition != "Lower")
    {
      this.SetDayInterval(0, this.dockPosition, this.formatters[this.navigatorIntervals.IndexOf("Day")]);
    }
    else
    {
      if (!this.navigatorIntervals.Contains("Hour") || !(this.dockPosition != "Lower"))
        return;
      this.SetHourInterval(0, this.dockPosition, this.formatters[this.navigatorIntervals.IndexOf("Hour")]);
    }
  }

  private bool GenerateLabelContainers(string postion)
  {
    this.txtblockwidth = 0.0;
    int index1 = 0;
    ObservableCollection<ChartAxisLabel> observableCollection1 = new ObservableCollection<ChartAxisLabel>();
    UIElementsRecycler<TextBlock> elementsRecycler;
    ObservableCollection<ChartAxisLabel> observableCollection2;
    Panel panel;
    if (postion == "Upper")
    {
      elementsRecycler = this.upperLabelRecycler;
      observableCollection2 = this.upperBarLabels;
      panel = this.upperLabelBar;
      this.upperTickLineRecycler.GenerateElements(this.upperBarLabels.Count);
      this.upperGridLineRecycler.GenerateElements(this.upperBarLabels.Count);
    }
    else
    {
      elementsRecycler = this.lowerLabelRecycler;
      observableCollection2 = this.lowerBarLabels;
      panel = this.lowerLabelBar;
      this.lowerTickLineRecycler.GenerateElements(this.lowerBarLabels.Count);
      this.lowerGridLineRecycler.GenerateElements(this.lowerBarLabels.Count);
    }
    elementsRecycler.GenerateElements(observableCollection2.Count);
    int count = observableCollection2.Count;
    if (postion == "Upper")
    {
      HigherBarLabelsCreatedEventArgs e = new HigherBarLabelsCreatedEventArgs();
      foreach (ChartAxisLabel chartAxisLabel in (Collection<ChartAxisLabel>) observableCollection2)
        e.HigherBarLabels.Add(new RangeNavigatorLabel()
        {
          Content = chartAxisLabel.LabelContent
        });
      if (this.HigherBarLabelsCreated != null)
        this.HigherBarLabelsCreated((object) this, e);
      for (int index2 = 0; index2 < count; ++index2)
        observableCollection2[index2].LabelContent = e.HigherBarLabels[index2].Content;
    }
    else
    {
      LowerBarLabelsCreatedEventArgs e = new LowerBarLabelsCreatedEventArgs();
      foreach (ChartAxisLabel chartAxisLabel in (Collection<ChartAxisLabel>) observableCollection2)
        e.LowerBarLabels.Add(new RangeNavigatorLabel()
        {
          Content = chartAxisLabel.LabelContent
        });
      if (this.LowerBarLabelsCreated != null)
        this.LowerBarLabelsCreated((object) this, e);
      for (int index3 = 0; index3 < count; ++index3)
        observableCollection2[index3].LabelContent = e.LowerBarLabels[index3].Content;
    }
    foreach (ChartAxisLabel chartAxisLabel in (Collection<ChartAxisLabel>) observableCollection2)
    {
      TextBlock textBlock = elementsRecycler[index1];
      textBlock.Text = chartAxisLabel.LabelContent.ToString();
      textBlock.Visibility = Visibility.Visible;
      textBlock.HorizontalAlignment = HorizontalAlignment.Center;
      ++index1;
      textBlock.Measure(this.Navigator.DesiredSize);
      this.txtblockwidth = Math.Max(textBlock.DesiredSize.Width, this.txtblockwidth);
    }
    return this.txtblockwidth * (double) panel.Children.Count < this.Navigator.TrackSize;
  }

  private void OnInternalValueChanged()
  {
    if (!this.isUpdate)
      this.CalculateSelectedData();
    this.OnValueChanged();
  }

  private void ResetTimer()
  {
    if (this.timer != null)
    {
      this.timer.Stop();
      this.timer.Interval = TimeSpan.FromSeconds(this.DeferredUpdateDelay);
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
    if (this.EnableDeferredUpdate)
      this.OnInternalValueChanged();
    if (this.timer != null)
    {
      this.timer.Stop();
      this.timer.Tick -= new EventHandler(this.OnTimeout);
    }
    this.timer = (DispatcherTimer) null;
  }

  private void GenerateDataTablePoints()
  {
    this.DataEnd = 0.0;
    IEnumerator enumerator = (this.ItemsSource as DataTable).Rows.GetEnumerator();
    if (!enumerator.MoveNext())
      return;
    this.XValues = (IEnumerable) new List<DateTime>();
    IList<DateTime> xvalues = (IList<DateTime>) (this.XValues as List<DateTime>);
    do
    {
      object obj = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
      xvalues.Add((DateTime) obj);
      ++this.DataEnd;
    }
    while (enumerator.MoveNext());
  }

  private void ClearLabels()
  {
    this.minimumDateTimeValue = DateTime.MinValue;
    this.maximumDateTimeValue = DateTime.MinValue;
    this.ClearNavigatorLabels();
    if (this.upperLabelBar == null || this.lowerLabelBar == null)
      return;
    this.GenerateLabelContainers("Upper");
    this.GenerateLabelContainers("Lower");
    this.UpdateTooltip();
  }

  private void ClearNavigatorLabels()
  {
    if (this.upperBarLabels != null)
      this.upperBarLabels.Clear();
    if (this.lowerBarLabels != null)
      this.lowerBarLabels.Clear();
    if (this.lowerLabelBounds != null)
      this.lowerLabelBounds.Clear();
    if (this.upperLabelBounds == null)
      return;
    this.upperLabelBounds.Clear();
  }
}
