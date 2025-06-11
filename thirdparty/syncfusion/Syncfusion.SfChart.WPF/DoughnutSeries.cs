// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.DoughnutSeries
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
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class DoughnutSeries : CircularSeriesBase, ISegmentSelectable, INotifyPropertyChanged
{
  public static readonly DependencyProperty TrackBorderWidthProperty = DependencyProperty.Register(nameof (TrackBorderWidth), typeof (double), typeof (DoughnutSeries), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty TrackBorderColorProperty = DependencyProperty.Register(nameof (TrackBorderColor), typeof (Brush), typeof (DoughnutSeries), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsStackedDoughnutProperty = DependencyProperty.Register(nameof (IsStackedDoughnut), typeof (bool), typeof (DoughnutSeries), new PropertyMetadata((object) false, new PropertyChangedCallback(DoughnutSeries.OnDoughnutSeriesPropertyChanged)));
  public static readonly DependencyProperty MaximumValueProperty = DependencyProperty.Register(nameof (MaximumValue), typeof (double?), typeof (DoughnutSeries), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(DoughnutSeries.OnDoughnutSeriesPropertyChanged)));
  public static readonly DependencyProperty TrackColorProperty = DependencyProperty.Register(nameof (TrackColor), typeof (Brush), typeof (DoughnutSeries), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb((byte) 51, (byte) 128 /*0x80*/, (byte) 128 /*0x80*/, (byte) 128 /*0x80*/)), new PropertyChangedCallback(DoughnutSeries.OnTrackColorPropertyChanged)));
  public static readonly DependencyProperty CapStyleProperty = DependencyProperty.Register(nameof (CapStyle), typeof (DoughnutCapStyle), typeof (DoughnutSeries), new PropertyMetadata((object) DoughnutCapStyle.BothFlat, new PropertyChangedCallback(DoughnutSeries.OnDoughnutSeriesPropertyChanged)));
  public static readonly DependencyProperty SegmentSpacingProperty = DependencyProperty.Register(nameof (SegmentSpacing), typeof (double), typeof (DoughnutSeries), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(DoughnutSeries.OnDoughnutSeriesPropertyChanged)));
  public static readonly DependencyProperty DoughnutCoefficientProperty = DependencyProperty.Register(nameof (DoughnutCoefficient), typeof (double), typeof (DoughnutSeries), new PropertyMetadata((object) 0.8, new PropertyChangedCallback(DoughnutSeries.OnDoughnutCoefficientChanged)));
  public static readonly DependencyProperty DoughnutSizeProperty = DependencyProperty.Register(nameof (DoughnutSize), typeof (double), typeof (DoughnutSeries), new PropertyMetadata((object) 0.8, new PropertyChangedCallback(DoughnutSeries.OnDoughnutSizeChanged)));
  public static readonly DependencyProperty DoughnutHoleSizeProperty = DependencyProperty.RegisterAttached("DoughnutHoleSize", typeof (double), typeof (DoughnutSeries), new PropertyMetadata((object) 0.4, new PropertyChangedCallback(DoughnutSeries.OnDoughnutHoleSizeChanged)));
  public static readonly DependencyProperty CenterViewProperty = DependencyProperty.Register(nameof (CenterView), typeof (ContentControl), typeof (DoughnutSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(DoughnutSeries.OnCenterViewPropertyChanged)));
  internal double InternalDoughnutCoefficient = 0.8;
  internal double InternalDoughnutSize = 0.8;
  private double innerRadius;
  private double ARCLENGTH;
  private Storyboard sb;
  private int animateCount;

  public DoughnutSeries()
  {
    this.DefaultStyleKey = (object) typeof (DoughnutSeries);
    this.InnerRadius = double.NaN;
  }

  public double TrackBorderWidth
  {
    get => (double) this.GetValue(DoughnutSeries.TrackBorderWidthProperty);
    set => this.SetValue(DoughnutSeries.TrackBorderWidthProperty, (object) value);
  }

  public Brush TrackBorderColor
  {
    get => (Brush) this.GetValue(DoughnutSeries.TrackBorderColorProperty);
    set => this.SetValue(DoughnutSeries.TrackBorderColorProperty, (object) value);
  }

  public bool IsStackedDoughnut
  {
    get => (bool) this.GetValue(DoughnutSeries.IsStackedDoughnutProperty);
    set => this.SetValue(DoughnutSeries.IsStackedDoughnutProperty, (object) value);
  }

  public double MaximumValue
  {
    get => (double) this.GetValue(DoughnutSeries.MaximumValueProperty);
    set => this.SetValue(DoughnutSeries.MaximumValueProperty, (object) value);
  }

  public Brush TrackColor
  {
    get => (Brush) this.GetValue(DoughnutSeries.TrackColorProperty);
    set => this.SetValue(DoughnutSeries.TrackColorProperty, (object) value);
  }

  public DoughnutCapStyle CapStyle
  {
    get => (DoughnutCapStyle) this.GetValue(DoughnutSeries.CapStyleProperty);
    set => this.SetValue(DoughnutSeries.CapStyleProperty, (object) value);
  }

  public double SegmentSpacing
  {
    get => (double) this.GetValue(DoughnutSeries.SegmentSpacingProperty);
    set => this.SetValue(DoughnutSeries.SegmentSpacingProperty, (object) value);
  }

  public double DoughnutCoefficient
  {
    get => (double) this.GetValue(DoughnutSeries.DoughnutCoefficientProperty);
    set => this.SetValue(DoughnutSeries.DoughnutCoefficientProperty, (object) value);
  }

  public double DoughnutSize
  {
    get => (double) this.GetValue(DoughnutSeries.DoughnutSizeProperty);
    set => this.SetValue(DoughnutSeries.DoughnutSizeProperty, (object) value);
  }

  public ContentControl CenterView
  {
    get => (ContentControl) this.GetValue(DoughnutSeries.CenterViewProperty);
    set => this.SetValue(DoughnutSeries.CenterViewProperty, (object) value);
  }

  public double InnerRadius
  {
    get => this.innerRadius;
    internal set
    {
      this.innerRadius = value;
      this.OnPropertyChanged(nameof (InnerRadius));
    }
  }

  internal double SegmentGapAngle { get; set; }

  public static double GetDoughnutHoleSize(DependencyObject obj)
  {
    return (double) obj.GetValue(DoughnutSeries.DoughnutHoleSizeProperty);
  }

  public static void SetDoughnutHoleSize(DependencyObject obj, double value)
  {
    obj.SetValue(DoughnutSeries.DoughnutHoleSizeProperty, (object) value);
  }

  public override void CreateSegments()
  {
    List<object> actualData = this.ActualData;
    IList<double> source;
    List<double> xValues;
    if (double.IsNaN(this.GroupTo))
    {
      source = this.ToggledLegendIndex.Count <= 0 ? this.YValues : (IList<double>) this.GetYValues();
      xValues = this.GetXValues();
    }
    else
    {
      if (this.Adornments != null)
        this.Adornments.Clear();
      if (this.Segments != null)
        this.Segments.Clear();
      double sumOfYValues = this.YValues.Select<double, double>((Func<double, double>) (val => val <= 0.0 ? Math.Abs(double.IsNaN(val) ? 0.0 : val) : val)).Sum();
      double xIndexValues = 0.0;
      xValues = this.YValues.Where<double>((Func<double, bool>) (val => this.GetGroupModeValue(val, sumOfYValues) > this.GroupTo)).Select<double, double>((Func<double, double>) (val => xIndexValues++)).ToList<double>();
      if (this.YValues.Count > xValues.Count)
        xValues.Add(xIndexValues);
      Tuple<List<double>, List<object>> groupToYvalues = this.GetGroupToYValues();
      actualData = groupToYvalues.Item2;
      source = this.ToggledLegendIndex.Count <= 0 ? (IList<double>) groupToYvalues.Item1 : (IList<double>) this.GetToggleYValues(groupToYvalues.Item1);
    }
    double radianConverter1 = this.DegreeToRadianConverter(this.EndAngle);
    double radianConverter2 = this.DegreeToRadianConverter(this.StartAngle);
    if (radianConverter2 == radianConverter1)
      this.Segments.Clear();
    this.ARCLENGTH = radianConverter1 - radianConverter2;
    if (Math.Abs(Math.Round(this.ARCLENGTH, 2)) > 2.0 * Math.PI)
      this.ARCLENGTH %= 2.0 * Math.PI;
    this.ClearUnUsedAdornments(this.DataCount);
    this.ClearUnUsedSegments(this.DataCount);
    int explodeIndex = this.ExplodeIndex;
    bool explodeAll = this.ExplodeAll;
    if (xValues == null)
      return;
    double num1 = source.Select<double, double>((Func<double, double>) (val => val <= 0.0 ? Math.Abs(double.IsNaN(val) ? 0.0 : val) : val)).Sum();
    double d = double.IsNaN(this.MaximumValue) ? num1 : this.MaximumValue;
    bool flag1 = !double.IsNaN(d) && this.IsStackedDoughnut && this.GetDoughnutSeriesCount() == 1;
    bool flag2 = false;
    int num2 = 0;
    int visibleSegmentCount = 0;
    for (int index = 0; index < xValues.Count; ++index)
    {
      flag2 = false;
      bool flag3;
      double num3;
      if (flag1)
      {
        flag3 = source[index] >= d;
        num3 = num1 == 0.0 ? 0.0 : Math.Abs(double.IsNaN(source[index]) ? 0.0 : (flag3 ? d : source[index])) * (this.ARCLENGTH / d);
      }
      else
      {
        flag3 = source[index] >= d;
        num3 = num1 == 0.0 ? 0.0 : Math.Abs(double.IsNaN(source[index]) ? 0.0 : source[index]) * (this.ARCLENGTH / num1);
      }
      if (index < this.Segments.Count)
      {
        DoughnutSegment segment = this.Segments[index] as DoughnutSegment;
        segment.SetData(radianConverter2, radianConverter2 + num3, this);
        segment.XData = xValues[index];
        segment.YData = !double.IsNaN(this.GroupTo) ? Math.Abs(source[index]) : Math.Abs(this.YValues[index]);
        segment.AngleOfSlice = (2.0 * radianConverter2 + num3) / 2.0;
        segment.IsExploded = explodeAll || explodeIndex == index;
        segment.Item = actualData[index];
        segment.IsEndValueExceed = flag3;
        segment.DoughnutSegmentIndex = num2;
        if (this.SegmentColorPath != null && !this.Segments[index].IsEmptySegmentInterior && this.ColorValues.Count > 0 && !this.Segments[index].IsSelectedSegment)
          this.Segments[index].Interior = this.Interior != null ? this.Interior : this.ColorValues[index];
        if (this.ToggledLegendIndex.Contains(index))
          this.Segments[index].IsSegmentVisible = false;
        else
          this.Segments[index].IsSegmentVisible = true;
        segment.UpdateTrackInterior(index);
      }
      else if (this.CreateSegment() is DoughnutSegment segment1)
      {
        segment1.XData = xValues[index];
        segment1.YData = !double.IsNaN(this.GroupTo) ? Math.Abs(source[index]) : Math.Abs(this.YValues[index]);
        segment1.IsExploded = explodeAll || explodeIndex == index;
        segment1.Item = actualData[index];
        segment1.IsEndValueExceed = flag3;
        segment1.DoughnutSegmentIndex = num2;
        segment1.SetData(radianConverter2, radianConverter2 + num3, this);
        segment1.AngleOfSlice = (2.0 * radianConverter2 + num3) / 2.0;
        if (this.ToggledLegendIndex.Contains(index))
          segment1.IsSegmentVisible = false;
        else
          segment1.IsSegmentVisible = true;
        this.Segments.Add((ChartSegment) segment1);
        segment1.UpdateTrackInterior(index);
      }
      if (this.AdornmentsInfo != null)
        this.AddDoughnutAdornments(xValues[index], source[index], radianConverter2, radianConverter2 + num3, index);
      if (!double.IsNaN(source[index]))
      {
        ++num2;
        if (source[index] != 0.0)
          ++visibleSegmentCount;
      }
      if (!this.IsStackedDoughnut)
        radianConverter2 += num3;
    }
    if (this.ShowEmptyPoints)
      this.UpdateEmptyPointSegments(xValues, false);
    this.UpdateSegmentGapAngle(visibleSegmentCount);
  }

  internal void ManipulateAdditionalVisual(UIElement element, NotifyCollectionChangedAction action)
  {
    if (!(element is FrameworkElement frameworkElement))
      return;
    switch (action)
    {
      case NotifyCollectionChangedAction.Add:
        if (!(frameworkElement.Tag is DoughnutSegment tag1) || this.SeriesPanel.Children.Contains((UIElement) tag1.CircularDoughnutPath))
          break;
        this.SeriesPanel.Children.Add((UIElement) tag1.CircularDoughnutPath);
        break;
      case NotifyCollectionChangedAction.Remove:
        if (!(frameworkElement.Tag is DoughnutSegment tag2) || !this.SeriesPanel.Children.Contains((UIElement) tag2.CircularDoughnutPath))
          break;
        this.SeriesPanel.Children.Remove((UIElement) tag2.CircularDoughnutPath);
        break;
      case NotifyCollectionChangedAction.Replace:
        if (!(frameworkElement.Tag is DoughnutSegment tag3) || this.SeriesPanel.Children.Contains((UIElement) tag3.CircularDoughnutPath))
          break;
        this.SeriesPanel.Children.Add((UIElement) tag3.CircularDoughnutPath);
        break;
    }
  }

  internal void RemoveCenterView(ContentControl centerView)
  {
    if (centerView == null || centerView.Parent == null)
      return;
    centerView.SizeChanged -= new SizeChangedEventHandler(this.CenterViewSizeChanged);
    centerView.Content = (object) null;
    if (!(centerView.Parent is Canvas parent))
      return;
    parent.Children.Remove((UIElement) centerView);
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    DoughnutSegment toolTipTag = this.ToolTipTag as DoughnutSegment;
    Point dataPointPosition = new Point();
    double radius = this.Radius;
    double angleOfSlice = toolTipTag.AngleOfSlice;
    double num1;
    if (!this.IsStackedDoughnut)
    {
      double num2 = this.InnerRadius + (this.Radius - this.InnerRadius) / 2.0;
      num1 = toolTipTag.IsExploded ? this.ExplodeRadius + num2 : num2;
    }
    else
    {
      if (this.Area == null || !this.Area.RootPanelDesiredSize.HasValue)
        return dataPointPosition;
      Size size = this.Area.RootPanelDesiredSize.Value;
      double height = this.Area.RootPanelDesiredSize.Value.Height;
      double width = this.Area.RootPanelDesiredSize.Value.Width;
      int count = this.Segments.Count;
      double num3 = this.InternalDoughnutSize * Math.Min(width, height) / 2.0;
      double num4 = (num3 - num3 * this.ActualArea.InternalDoughnutHoleSize) / (double) count * this.InternalDoughnutCoefficient;
      num1 = num3 - num4 * (double) (count - (toolTipTag.DoughnutSegmentIndex + 1)) - num4 / 2.0;
    }
    dataPointPosition.X = this.Center.X + Math.Cos(angleOfSlice) * num1;
    dataPointPosition.Y = this.Center.Y + Math.Sin(angleOfSlice) * num1;
    return dataPointPosition;
  }

  internal override void UpdateEmptyPointSegments(List<double> xValues, bool isSidebySideSeries)
  {
    if (this.EmptyPointIndexes == null)
      return;
    foreach (int index in this.EmptyPointIndexes[0])
    {
      DoughnutSegment segment = this.Segments[index] as DoughnutSegment;
      bool isExploded = segment.IsExploded;
      this.Segments[index].IsEmptySegmentInterior = true;
      (this.Segments[index] as DoughnutSegment).AngleOfSlice = segment.AngleOfSlice;
      (this.Segments[index] as DoughnutSegment).IsExploded = isExploded;
      if (this.Adornments.Count > 0)
        this.Adornments[index].IsEmptySegmentInterior = true;
    }
  }

  internal override bool GetAnimationIsActive()
  {
    return this.sb != null && this.sb.GetCurrentState() == ClockState.Active;
  }

  internal override void Animate()
  {
    double radianConverter = this.DegreeToRadianConverter(this.StartAngle);
    if (this.Segments.Count <= 0)
      return;
    if (this.sb != null && this.animateCount <= this.Segments.Count)
      this.sb = new Storyboard();
    else if (this.sb != null)
    {
      this.sb.Stop();
      if (!this.canAnimate)
      {
        foreach (DoughnutSegment segment in (Collection<ChartSegment>) this.Segments)
        {
          if (segment.EndAngle - segment.StartAngle != 0.0)
          {
            segment.ActualStartAngle = segment.StartAngle;
            segment.ActualEndAngle = segment.EndAngle;
          }
        }
        this.ResetAdornmentAnimationState();
        return;
      }
    }
    else
      this.sb = new Storyboard();
    this.AnimateAdornments(this.sb);
    foreach (DoughnutSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      ++this.animateCount;
      double startAngle = segment.StartAngle;
      double endAngle = segment.EndAngle;
      if (endAngle - startAngle != 0.0)
      {
        DoubleAnimationUsingKeyFrames element1 = new DoubleAnimationUsingKeyFrames();
        SplineDoubleKeyFrame splineDoubleKeyFrame1 = new SplineDoubleKeyFrame();
        splineDoubleKeyFrame1.Value = radianConverter;
        SplineDoubleKeyFrame keyFrame1 = splineDoubleKeyFrame1;
        keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
        element1.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
        SplineDoubleKeyFrame splineDoubleKeyFrame2 = new SplineDoubleKeyFrame();
        splineDoubleKeyFrame2.Value = startAngle;
        SplineDoubleKeyFrame keyFrame2 = splineDoubleKeyFrame2;
        keyFrame2.KeyTime = keyFrame2.KeyTime.GetKeyTime(this.AnimationDuration);
        keyFrame2.KeySpline = new KeySpline()
        {
          ControlPoint1 = new Point(0.64, 0.84),
          ControlPoint2 = new Point(0.67, 0.95)
        };
        element1.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
        Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath((object) DoughnutSegment.ActualStartAngleProperty));
        Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) segment);
        this.sb.Children.Add((Timeline) element1);
        DoubleAnimationUsingKeyFrames element2 = new DoubleAnimationUsingKeyFrames();
        SplineDoubleKeyFrame splineDoubleKeyFrame3 = new SplineDoubleKeyFrame();
        splineDoubleKeyFrame3.Value = radianConverter;
        SplineDoubleKeyFrame keyFrame3 = splineDoubleKeyFrame3;
        keyFrame3.KeyTime = keyFrame3.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
        element2.KeyFrames.Add((DoubleKeyFrame) keyFrame3);
        SplineDoubleKeyFrame splineDoubleKeyFrame4 = new SplineDoubleKeyFrame();
        splineDoubleKeyFrame4.Value = endAngle;
        SplineDoubleKeyFrame keyFrame4 = splineDoubleKeyFrame4;
        keyFrame4.KeyTime = keyFrame4.KeyTime.GetKeyTime(this.AnimationDuration);
        KeySpline keySpline = new KeySpline()
        {
          ControlPoint1 = new Point(0.64, 0.84),
          ControlPoint2 = new Point(0.67, 0.95)
        };
        keyFrame4.KeySpline = keySpline;
        element2.KeyFrames.Add((DoubleKeyFrame) keyFrame4);
        Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath((object) DoughnutSegment.ActualEndAngleProperty));
        Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) segment);
        this.sb.Children.Add((Timeline) element2);
      }
    }
    this.sb.Begin();
  }

  internal override void Dispose()
  {
    this.RemoveCenterView(this.CenterView);
    if (this.sb != null)
    {
      this.sb.Stop();
      this.sb.Children.Clear();
      this.sb = (Storyboard) null;
    }
    base.Dispose();
  }

  internal void AddCenterView(ContentControl centerView)
  {
    if (centerView == null || this.SeriesPanel == null || centerView.Parent != null)
      return;
    this.SeriesPanel.Children.Add((UIElement) centerView);
    this.CenterView.SizeChanged += new SizeChangedEventHandler(this.CenterViewSizeChanged);
  }

  private void CenterViewSizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.PositioningCenterView();
  }

  internal void PositioningCenterView()
  {
    if (this.CenterView == null)
      return;
    double num1 = this.Center.X - this.CenterView.ActualWidth / 2.0;
    double num2 = this.Center.Y - this.CenterView.ActualHeight / 2.0;
    this.CenterView.SetValue(Canvas.LeftProperty, (object) num1);
    this.CenterView.SetValue(Canvas.TopProperty, (object) num2);
  }

  internal int GetDoughnutSeriesCount()
  {
    return this.Area.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series is DoughnutSeries)).ToList<ChartSeriesBase>().Count;
  }

  protected internal override IChartTransformer CreateTransformer(Size size, bool create)
  {
    if (create || this.ChartTransformer == null)
      this.ChartTransformer = ChartTransform.CreateSimple(size);
    return this.ChartTransformer;
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new DoughnutSegment();

  protected override ChartAdornment CreateAdornment(
    AdornmentSeries series,
    double xVal,
    double yVal,
    double angle,
    double radius)
  {
    ChartPieAdornment adornment = new ChartPieAdornment(xVal, yVal, angle, radius, series);
    adornment.SetValues(xVal, yVal, angle, radius, series);
    return (ChartAdornment) adornment;
  }

  protected override void SetExplodeIndex(int i)
  {
    if (this.Segments.Count <= 0)
      return;
    foreach (DoughnutSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      int num = !double.IsNaN(this.GroupTo) ? this.Segments.IndexOf((ChartSegment) segment) : this.ActualData.IndexOf(segment.Item);
      if (i == num)
      {
        segment.IsExploded = !segment.IsExploded;
        this.UpdateSegments(i, NotifyCollectionChangedAction.Remove);
      }
      else if (i == -1)
      {
        segment.IsExploded = false;
        this.UpdateSegments(i, NotifyCollectionChangedAction.Remove);
      }
    }
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    if (oldValue != null)
      this.animateCount = 0;
    base.OnDataSourceChanged(oldValue, newValue);
  }

  protected override void SetExplodeRadius()
  {
    if (this.Segments.Count <= 0)
      return;
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
      this.UpdateSegments(this.Segments.IndexOf(segment), NotifyCollectionChangedAction.Replace);
  }

  protected override void SetExplodeAll()
  {
    if (this.Segments.Count <= 0)
      return;
    foreach (DoughnutSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      int index = this.Segments.IndexOf((ChartSegment) segment);
      segment.IsExploded = true;
      this.UpdateSegments(index, NotifyCollectionChangedAction.Replace);
    }
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new DoughnutSeries()
    {
      DoughnutCoefficient = this.DoughnutCoefficient
    });
  }

  private static void OnTrackColorPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is DoughnutSeries doughnutSeries) || doughnutSeries.Segments == null)
      return;
    for (int index = 0; index < doughnutSeries.Segments.Count; ++index)
      (doughnutSeries.Segments[index] as DoughnutSegment).UpdateTrackInterior(index);
  }

  private static void OnDoughnutSeriesPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is DoughnutSeries doughnutSeries))
      return;
    doughnutSeries.UpdateArea();
  }

  private static void OnDoughnutCoefficientChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    DoughnutSeries doughnutSeries = d as DoughnutSeries;
    doughnutSeries.InternalDoughnutCoefficient = ChartMath.MinMax((double) e.NewValue, 0.0, 1.0);
    doughnutSeries.UpdateArea();
  }

  private static void OnDoughnutSizeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    DoughnutSeries doughnutSeries = d as DoughnutSeries;
    doughnutSeries.InternalDoughnutSize = ChartMath.MinMax((double) e.NewValue, 0.0, 1.0);
    doughnutSeries.UpdateArea();
  }

  private static void OnDoughnutHoleSizeChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(sender is SfChart sfChart))
      return;
    sfChart.InternalDoughnutHoleSize = ChartMath.MinMax((double) e.NewValue, 0.0, 1.0);
    sfChart.ScheduleUpdate();
  }

  private static void OnCenterViewPropertyChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs e)
  {
    DoughnutSeries doughnutSeries = sender as DoughnutSeries;
    doughnutSeries.RemoveCenterView(e.OldValue as ContentControl);
    doughnutSeries.AddCenterView(e.NewValue as ContentControl);
  }

  private void UpdateSegmentGapAngle(int visibleSegmentCount)
  {
    if (this.IsStackedDoughnut || this.GetDoughnutSeriesCount() != 1)
      return;
    this.SegmentGapAngle = (visibleSegmentCount == 1 ? 0.0 : this.SegmentSpacing) * Math.Abs(this.ARCLENGTH) / (double) (this.Segments.Count * 2);
  }

  private void AddDoughnutAdornments(
    double x,
    double y,
    double startAngle,
    double endAngle,
    int index)
  {
    double xPos = (startAngle + endAngle) / 2.0;
    if (this.Area == null || !this.Area.RootPanelDesiredSize.HasValue)
      return;
    Size size = this.Area.RootPanelDesiredSize.Value;
    double height = this.Area.RootPanelDesiredSize.Value.Height;
    double width = this.Area.RootPanelDesiredSize.Value.Width;
    double yPos;
    if (this.IsStackedDoughnut)
    {
      int dataCount = this.DataCount;
      double num1 = this.InternalDoughnutSize * Math.Min(width, height) / 2.0;
      double num2 = (num1 - num1 * this.ActualArea.InternalDoughnutHoleSize) / (double) dataCount * this.InternalDoughnutCoefficient;
      yPos = num1 - num2 * (double) (dataCount - (index + 1));
    }
    else
    {
      List<ChartSeriesBase> list = this.Area.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series is DoughnutSeries)).ToList<ChartSeriesBase>();
      int num3 = list.Count<ChartSeriesBase>();
      double num4 = (double) list.IndexOf((ChartSeriesBase) this);
      double num5 = this.InternalDoughnutSize * Math.Min(width, height) / 2.0;
      double num6 = (num5 - num5 * this.Area.InternalDoughnutHoleSize) / (double) num3;
      yPos = num5 - num6 * ((double) num3 - (num4 + 1.0));
    }
    if (index < this.Adornments.Count)
      this.Adornments[index].SetData(x, y, xPos, yPos);
    else
      this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, x, y, xPos, yPos));
    this.Adornments[index].Item = !double.IsNaN(this.GroupTo) ? this.Segments[index].Item : this.ActualData[index];
  }
}
