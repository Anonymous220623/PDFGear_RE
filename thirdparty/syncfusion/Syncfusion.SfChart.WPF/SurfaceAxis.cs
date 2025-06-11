// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SurfaceAxis
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SurfaceAxis : Control
{
  private Panel labelsPanel;
  private Panel elementsPanel;
  private SurfaceAxisPanel axisPanel;
  private ObservableCollection<ChartAxisLabel> m_VisibleLabels;
  private DoubleRange m_actualRange = DoubleRange.Empty;
  private Size availableSize;
  internal ILayoutCalculator axisLabelsPanel;
  internal ILayoutCalculator axisElementsPanel;
  internal bool smallTicksRequired;
  internal List<double> m_smalltickPoints = new List<double>();
  protected internal double MaxPixelsCount = 100.0;
  internal static readonly int[] c_intervalDivs = new int[4]
  {
    10,
    5,
    2,
    1
  };
  internal bool OpposedPosition;
  internal double LeftOffset;
  internal double RightOffset;
  internal static readonly DependencyProperty IsInversedProperty = DependencyProperty.Register(nameof (IsInversed), typeof (bool), typeof (SurfaceAxis), new PropertyMetadata((object) false, new PropertyChangedCallback(SurfaceAxis.OnInversedChanged)));
  public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (SurfaceAxis), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (SurfaceAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(SurfaceAxis.OnPropertyChanged)));
  public static readonly DependencyProperty LabelFormatProperty = DependencyProperty.Register(nameof (LabelFormat), typeof (string), typeof (SurfaceAxis), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(SurfaceAxis.OnPropertyChanged)));
  public static readonly DependencyProperty LabelTemplateProperty = DependencyProperty.Register(nameof (LabelTemplate), typeof (DataTemplate), typeof (SurfaceAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(SurfaceAxis.OnPropertyChanged)));
  public static readonly DependencyProperty SmallTicksPerIntervalProperty = DependencyProperty.Register(nameof (SmallTicksPerInterval), typeof (int), typeof (SurfaceAxis), new PropertyMetadata((object) 0, new PropertyChangedCallback(SurfaceAxis.OnSmallTicksPerIntervalPropertyChanged)));
  public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (double?), typeof (SurfaceAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(SurfaceAxis.OnMinimumChanged)));
  public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (double?), typeof (SurfaceAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(SurfaceAxis.OnMaximumChanged)));
  public static readonly DependencyProperty AxisLineStyleProperty = DependencyProperty.Register(nameof (AxisLineStyle), typeof (Style), typeof (SurfaceAxis), (PropertyMetadata) null);
  internal static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (SurfaceAxis), new PropertyMetadata((object) Orientation.Horizontal));
  public static readonly DependencyProperty TickLineSizeProperty = DependencyProperty.Register(nameof (TickLineSize), typeof (double), typeof (SurfaceAxis), new PropertyMetadata((object) 6.0, new PropertyChangedCallback(SurfaceAxis.OnPropertyChanged)));
  public static readonly DependencyProperty MajorTickLineStyleProperty = DependencyProperty.Register(nameof (MajorTickLineStyle), typeof (Style), typeof (SurfaceAxis), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MinorTickLineStyleProperty = DependencyProperty.Register(nameof (MinorTickLineStyle), typeof (Style), typeof (SurfaceAxis), (PropertyMetadata) null);
  public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof (Interval), typeof (double?), typeof (SurfaceAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(SurfaceAxis.OnPropertyChanged)));
  public static readonly DependencyProperty ShowGridLinesProperty = DependencyProperty.Register(nameof (ShowGridLines), typeof (bool), typeof (SurfaceAxis), new PropertyMetadata((object) true, new PropertyChangedCallback(SurfaceAxis.OnPropertyChanged)));
  public static readonly DependencyProperty GridLineStrokeProperty = DependencyProperty.Register(nameof (GridLineStroke), typeof (Brush), typeof (SurfaceAxis), new PropertyMetadata((object) new SolidColorBrush(Colors.Black), new PropertyChangedCallback(SurfaceAxis.OnPropertyChanged)));
  public static readonly DependencyProperty GridLineThicknessProperty = DependencyProperty.Register(nameof (GridLineThickness), typeof (double), typeof (SurfaceAxis), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(SurfaceAxis.OnPropertyChanged)));
  public static readonly DependencyProperty EdgeLabelsDrawingModeProperty = DependencyProperty.Register(nameof (EdgeLabelsDrawingMode), typeof (EdgeLabelsDrawingMode), typeof (SurfaceAxis), new PropertyMetadata((object) EdgeLabelsDrawingMode.Center, new PropertyChangedCallback(SurfaceAxis.OnPropertyChanged)));
  public static readonly DependencyProperty RangePaddingProperty = DependencyProperty.Register(nameof (RangePadding), typeof (NumericalPadding), typeof (SurfaceAxis), new PropertyMetadata((object) NumericalPadding.Round, new PropertyChangedCallback(SurfaceAxis.OnPropertyChanged)));

  public SurfaceAxis()
  {
    this.DefaultStyleKey = (object) typeof (SurfaceAxis);
    this.m_VisibleLabels = new ObservableCollection<ChartAxisLabel>();
  }

  internal Size AxisDesiredSize { get; set; }

  internal double ActualInterval { get; set; }

  internal SfSurfaceChart Area { get; set; }

  internal bool IsInversed
  {
    get => (bool) this.GetValue(SurfaceAxis.IsInversedProperty);
    set => this.SetValue(SurfaceAxis.IsInversedProperty, (object) value);
  }

  private static void OnInversedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    SurfaceAxis axis = d as SurfaceAxis;
    SfSurfaceChart area = axis.Area;
    area?.RenderAxis(area.RootPanelDesiredSize.Value, axis);
  }

  internal DoubleRange ActualRange
  {
    get => this.m_actualRange;
    set => this.m_actualRange = value;
  }

  public object Header
  {
    get => this.GetValue(SurfaceAxis.HeaderProperty);
    set => this.SetValue(SurfaceAxis.HeaderProperty, value);
  }

  public DataTemplate HeaderTemplate
  {
    get => (DataTemplate) this.GetValue(SurfaceAxis.HeaderTemplateProperty);
    set => this.SetValue(SurfaceAxis.HeaderTemplateProperty, (object) value);
  }

  public string LabelFormat
  {
    get => (string) this.GetValue(SurfaceAxis.LabelFormatProperty);
    set => this.SetValue(SurfaceAxis.LabelFormatProperty, (object) value);
  }

  public DataTemplate LabelTemplate
  {
    get => (DataTemplate) this.GetValue(SurfaceAxis.LabelTemplateProperty);
    set => this.SetValue(SurfaceAxis.LabelTemplateProperty, (object) value);
  }

  public int SmallTicksPerInterval
  {
    get => (int) this.GetValue(SurfaceAxis.SmallTicksPerIntervalProperty);
    set => this.SetValue(SurfaceAxis.SmallTicksPerIntervalProperty, (object) value);
  }

  private static void OnSmallTicksPerIntervalPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SurfaceAxis surfaceAxis))
      return;
    surfaceAxis.smallTicksRequired = (int) e.NewValue > 0 || surfaceAxis.smallTicksRequired;
    if (surfaceAxis.Area == null)
      return;
    surfaceAxis.Area.ScheduleUpdate();
  }

  internal List<double> SmallTickPoints => this.m_smalltickPoints;

  public double? Minimum
  {
    get => (double?) this.GetValue(SurfaceAxis.MinimumProperty);
    set => this.SetValue(SurfaceAxis.MinimumProperty, (object) value);
  }

  private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as SurfaceAxis).OnMinMaxChanged();
  }

  public double? Maximum
  {
    get => (double?) this.GetValue(SurfaceAxis.MaximumProperty);
    set => this.SetValue(SurfaceAxis.MaximumProperty, (object) value);
  }

  private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as SurfaceAxis).OnMinMaxChanged();
  }

  private void OnMinMaxChanged()
  {
    if (this.Minimum.HasValue || this.Maximum.HasValue)
      this.ActualRange = new DoubleRange(!this.Minimum.HasValue ? double.NegativeInfinity : this.Minimum.Value, !this.Maximum.HasValue ? double.PositiveInfinity : this.Maximum.Value);
    if (this.Area == null)
      return;
    this.Area.IsPointsGenerated = false;
    this.Area.ScheduleUpdate();
  }

  public ObservableCollection<ChartAxisLabel> VisibleLabels => this.m_VisibleLabels;

  public Style AxisLineStyle
  {
    get => (Style) this.GetValue(SurfaceAxis.AxisLineStyleProperty);
    set => this.SetValue(SurfaceAxis.AxisLineStyleProperty, (object) value);
  }

  internal Orientation Orientation
  {
    get => (Orientation) this.GetValue(SurfaceAxis.OrientationProperty);
    set => this.SetValue(SurfaceAxis.OrientationProperty, (object) value);
  }

  public double TickLineSize
  {
    get => (double) this.GetValue(SurfaceAxis.TickLineSizeProperty);
    set => this.SetValue(SurfaceAxis.TickLineSizeProperty, (object) value);
  }

  public Style MajorTickLineStyle
  {
    get => (Style) this.GetValue(SurfaceAxis.MajorTickLineStyleProperty);
    set => this.SetValue(SurfaceAxis.MajorTickLineStyleProperty, (object) value);
  }

  public Style MinorTickLineStyle
  {
    get => (Style) this.GetValue(SurfaceAxis.MinorTickLineStyleProperty);
    set => this.SetValue(SurfaceAxis.MinorTickLineStyleProperty, (object) value);
  }

  public double? Interval
  {
    get => (double?) this.GetValue(SurfaceAxis.IntervalProperty);
    set => this.SetValue(SurfaceAxis.IntervalProperty, (object) value);
  }

  public bool ShowGridLines
  {
    get => (bool) this.GetValue(SurfaceAxis.ShowGridLinesProperty);
    set => this.SetValue(SurfaceAxis.ShowGridLinesProperty, (object) value);
  }

  private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SurfaceAxis surfaceAxis) || surfaceAxis.Area == null)
      return;
    surfaceAxis.Area.ScheduleUpdate();
  }

  public Brush GridLineStroke
  {
    get => (Brush) this.GetValue(SurfaceAxis.GridLineStrokeProperty);
    set => this.SetValue(SurfaceAxis.GridLineStrokeProperty, (object) value);
  }

  public double GridLineThickness
  {
    get => (double) this.GetValue(SurfaceAxis.GridLineThicknessProperty);
    set => this.SetValue(SurfaceAxis.GridLineThicknessProperty, (object) value);
  }

  public EdgeLabelsDrawingMode EdgeLabelsDrawingMode
  {
    get => (EdgeLabelsDrawingMode) this.GetValue(SurfaceAxis.EdgeLabelsDrawingModeProperty);
    set => this.SetValue(SurfaceAxis.EdgeLabelsDrawingModeProperty, (object) value);
  }

  public NumericalPadding RangePadding
  {
    get => (NumericalPadding) this.GetValue(SurfaceAxis.RangePaddingProperty);
    set => this.SetValue(SurfaceAxis.RangePaddingProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    this.labelsPanel = this.GetTemplateChild("axisLabelsPanel") as Panel;
    this.elementsPanel = this.GetTemplateChild("axisElementPanel") as Panel;
    this.axisPanel = this.GetTemplateChild("axisPanel") as SurfaceAxisPanel;
    if (this.axisPanel != null)
      this.axisPanel.Axis = this;
    this.UpdatePanels();
  }

  internal virtual void ComputeDesiredSize(Size size)
  {
    this.availableSize = size;
    this.CalculateRangeAndInterval(size);
    if (this.Visibility == Visibility.Collapsed)
      return;
    this.ApplyTemplate();
    if (this.axisPanel == null)
      return;
    this.UpdatePanels();
    this.UpdateLabels();
    this.AxisDesiredSize = this.axisPanel.ComputeSize(size);
  }

  private void CalculateRangeAndInterval(Size availableSize)
  {
    DoubleRange actualRange = this.ActualRange;
    DoubleRange range = this.CalculateActualRange();
    if (range.IsEmpty)
      range = new DoubleRange(0.0, 1.0);
    if (range.Start == range.End)
      range = new DoubleRange(range.Start, range.End + 1.0);
    this.ActualInterval = this.CalculateActualInterval(range, availableSize);
    this.ActualRange = this.ApplyRangePadding(range, this.ActualInterval, this.RangePadding);
    this.Area.RangeChanged = actualRange != this.ActualRange || this.Area.RangeChanged;
  }

  private DoubleRange CalculateActualRange()
  {
    if (this.ActualRange.IsEmpty)
      return this.CalculateDataRange();
    if (this.Minimum.HasValue && this.Maximum.HasValue)
      return this.ActualRange;
    DoubleRange dataRange = this.CalculateDataRange();
    if (this.Minimum.HasValue)
      return new DoubleRange(this.ActualRange.Start, double.IsInfinity(this.ActualRange.End) ? this.ActualRange.Start + 1.0 : dataRange.End);
    return this.Maximum.HasValue ? new DoubleRange(double.IsInfinity(this.ActualRange.Start) ? this.ActualRange.End - 1.0 : dataRange.Start, this.ActualRange.End) : dataRange;
  }

  internal DoubleRange CalculateDataRange()
  {
    if (this.Area.InterernalXAxis == this)
      return this.Area.XRange;
    if (this.Area.InterernalYAxis == this)
      return this.Area.YRange;
    return this.Area.InterernalZAxis == this ? this.Area.ZRange : DoubleRange.Empty;
  }

  internal double CalculateActualInterval(DoubleRange range, Size availableSize)
  {
    return !this.Interval.HasValue ? this.CalculateNiceInterval(range, availableSize) : this.Interval.Value;
  }

  protected virtual DoubleRange ApplyRangePadding(
    DoubleRange range,
    double interval,
    NumericalPadding rangePadding)
  {
    if (!this.Minimum.HasValue && !this.Maximum.HasValue)
      return this.CalculateRangePadding(ref range, interval, rangePadding);
    if (this.Minimum.HasValue && this.Maximum.HasValue)
      return range;
    DoubleRange rangePadding1 = this.CalculateRangePadding(ref range, interval, rangePadding);
    return !this.Minimum.HasValue ? new DoubleRange(rangePadding1.Start, range.End) : new DoubleRange(range.Start, rangePadding1.End);
  }

  private DoubleRange CalculateRangePadding(
    ref DoubleRange range,
    double interval,
    NumericalPadding rangePadding)
  {
    switch (rangePadding)
    {
      case NumericalPadding.Round:
      case NumericalPadding.Additional:
        double start1 = Math.Floor(range.Start / interval) * interval;
        double end1 = Math.Ceiling(range.End / interval) * interval;
        if (rangePadding == NumericalPadding.Additional)
        {
          start1 -= interval;
          end1 += interval;
        }
        return new DoubleRange(start1, end1);
      case NumericalPadding.Normal:
        double num1 = range.Start;
        double start2;
        if (range.Start < 0.0)
        {
          num1 = 0.0;
          start2 = range.Start + range.Start / 20.0;
          double num2 = interval + start2 % interval;
          if (0.365 * interval >= num2)
            start2 -= interval;
          if (start2 % interval < 0.0)
            start2 = start2 - interval - start2 % interval;
        }
        else
        {
          start2 = range.Start < 5.0 / 6.0 * range.End ? 0.0 : range.Start - (range.End - range.Start) / 2.0;
          if (start2 % interval > 0.0)
            start2 -= start2 % interval;
        }
        double end2 = range.End + (range.End - num1) / 20.0;
        double num3 = interval - end2 % interval;
        if (0.365 * interval >= num3)
          end2 += interval;
        if (end2 % interval > 0.0)
          end2 = end2 + interval - end2 % interval;
        range = new DoubleRange(start2, end2);
        if (start2 == 0.0)
        {
          this.ActualInterval = this.CalculateActualInterval(range, this.availableSize);
          return new DoubleRange(0.0, Math.Ceiling(end2 / this.ActualInterval) * this.ActualInterval);
        }
        break;
    }
    return range;
  }

  protected internal virtual double CalculateNiceInterval(
    DoubleRange actualRange,
    Size availableSize)
  {
    double delta = actualRange.Delta;
    double desiredIntervalsCount = this.GetActualDesiredIntervalsCount(availableSize);
    double d = delta / desiredIntervalsCount;
    double num1 = Math.Pow(10.0, Math.Floor(Math.Log10(d)));
    foreach (int cIntervalDiv in SurfaceAxis.c_intervalDivs)
    {
      double num2 = num1 * (double) cIntervalDiv;
      if (desiredIntervalsCount >= delta / num2)
        d = num2;
      else
        break;
    }
    return d;
  }

  protected internal double GetActualDesiredIntervalsCount(Size availableSize)
  {
    return Math.Max((this.Orientation == Orientation.Horizontal ? availableSize.Width : availableSize.Height) * ((this.Orientation == Orientation.Horizontal ? 0.54 : 1.0) * 3.0) / this.MaxPixelsCount, 1.0);
  }

  private void UpdatePanels()
  {
    if (this.axisPanel == null || this.axisLabelsPanel != null)
      return;
    this.axisPanel.LayoutCalc.Clear();
    this.axisLabelsPanel = (ILayoutCalculator) new SurfaceAxisLabelsPanel(this.labelsPanel)
    {
      Axis = this
    };
    this.axisElementsPanel = (ILayoutCalculator) new SurfaceAxisElementPanel(this.elementsPanel)
    {
      Axis = this
    };
    this.axisPanel.LayoutCalc.Add(this.axisLabelsPanel);
    this.axisPanel.LayoutCalc.Add(this.axisElementsPanel);
  }

  private void UpdateLabels()
  {
    if (this.ActualRange.Delta <= 0.0)
      return;
    this.VisibleLabels.Clear();
    this.m_smalltickPoints.Clear();
    this.GenerateVisibleLabels();
    if (this.axisLabelsPanel == null)
      return;
    if (this.axisElementsPanel != null)
      this.axisElementsPanel.UpdateElements();
    this.axisLabelsPanel.UpdateElements();
  }

  internal void GenerateVisibleLabels()
  {
    double actualInterval = this.ActualInterval;
    double start;
    for (start = this.ActualRange.Start; start <= this.ActualRange.End; start += actualInterval)
    {
      if (this.ActualRange.Inside(start))
        this.VisibleLabels.Add(new ChartAxisLabel(start, this.GetLabelContent(start), start));
      if (this.smallTicksRequired)
        this.AddSmallTicksPoint(start, this.ActualInterval, (double) this.SmallTicksPerInterval);
    }
    if (!this.Minimum.HasValue || !this.ActualRange.End.Equals((object) this.Maximum) || this.ActualRange.End.Equals(start - actualInterval))
      return;
    this.VisibleLabels.Add(new ChartAxisLabel(this.ActualRange.End, this.GetLabelContent(this.ActualRange.End), this.ActualRange.End));
  }

  internal object GetLabelContent(double position)
  {
    if (this == this.Area.XAxis)
    {
      if (this.Area.XValues is List<string> xvalues && position * (double) this.Area.ColumnSize <= (double) xvalues.Count && position >= 0.0)
        return !string.IsNullOrEmpty(this.LabelFormat) ? (object) string.Format(this.LabelFormat, (object) xvalues[(int) (position * (double) this.Area.ColumnSize)]) : (object) xvalues[position * (double) this.Area.ColumnSize == (double) xvalues.Count ? (int) (double) (xvalues.Count - 1) : (int) (position * (double) this.Area.ColumnSize)];
    }
    else if (this == this.Area.ZAxis && this.Area.ZValues is List<string> zvalues && position * (double) this.Area.RowSize <= (double) zvalues.Count && position >= 0.0)
      return !string.IsNullOrEmpty(this.LabelFormat) ? (object) string.Format(this.LabelFormat, (object) zvalues[(int) (position * (double) this.Area.RowSize)]) : (object) zvalues[position * (double) this.Area.RowSize == (double) zvalues.Count ? (int) (double) (zvalues.Count - 1) : (int) (position * (double) this.Area.RowSize)];
    return (object) Math.Round(position, 6).ToString(this.LabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
  }

  internal void AddSmallTicksPoint(double position, double interval, double smallTicksPerInterval)
  {
    double num1 = interval / (smallTicksPerInterval + 1.0);
    double num2 = position + num1;
    double end = this.ActualRange.End;
    for (position += interval; num2 < position && num2 <= end; num2 += num1)
    {
      if (this.ActualRange.Inside(num2))
        this.m_smalltickPoints.Add(num2);
    }
  }

  internal void ArrangeAxisPanel(Size size)
  {
    if (this.axisPanel == null)
      return;
    this.axisPanel.ArrangeElements(new Size(size.Width, size.Height));
  }

  internal double ValueToCoefficient(double value)
  {
    double start = this.ActualRange.Start;
    double delta = this.ActualRange.Delta;
    double num = (value - start) / delta;
    return !this.IsInversed ? num : 1.0 - num;
  }

  internal double ValueTo3DCoefficient(double value, bool isReverse)
  {
    double start = this.ActualRange.Start;
    double delta = this.ActualRange.Delta;
    double num = this == this.Area.InterernalXAxis ? 2.5 * (value - start) / delta - 1.25 : (this == this.Area.InterernalZAxis ? (2.0 * (value - start) / delta - 1.0) * -1.0 : 2.0 * (value - start) / delta - 1.0);
    return !isReverse ? num : num * -1.0;
  }
}
