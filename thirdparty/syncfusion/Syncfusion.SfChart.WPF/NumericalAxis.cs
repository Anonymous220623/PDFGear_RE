// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.NumericalAxis
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

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class NumericalAxis : RangeAxisBase
{
  public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof (Interval), typeof (double?), typeof (NumericalAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(NumericalAxis.OnIntervalChanged)));
  public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (double?), typeof (NumericalAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(NumericalAxis.OnMinimumChanged)));
  public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (double?), typeof (NumericalAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(NumericalAxis.OnMaximumChanged)));
  public static readonly DependencyProperty RangePaddingProperty = DependencyProperty.Register(nameof (RangePadding), typeof (NumericalPadding), typeof (NumericalAxis), new PropertyMetadata((object) NumericalPadding.Auto, new PropertyChangedCallback(NumericalAxis.OnPropertyChanged)));
  public static readonly DependencyProperty StartRangeFromZeroProperty = DependencyProperty.Register(nameof (StartRangeFromZero), typeof (bool), typeof (NumericalAxis), new PropertyMetadata((object) false, new PropertyChangedCallback(NumericalAxis.OnPropertyChanged)));
  public static readonly DependencyProperty AxisScaleBreaksProperty = DependencyProperty.Register(nameof (AxisScaleBreaks), typeof (ChartAxisScaleBreaks), typeof (NumericalAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(NumericalAxis.OnScaleBreakChanged)));
  public static readonly DependencyProperty BreakPositionProperty = DependencyProperty.Register(nameof (BreakPosition), typeof (ScaleBreakPosition), typeof (NumericalAxis), new PropertyMetadata((object) ScaleBreakPosition.DataCount, new PropertyChangedCallback(NumericalAxis.OnBreakPositionChanged)));
  internal double DataRangeDelta;
  internal List<FrameworkElement> BreakShapes;
  internal List<DoubleRange> BreakRanges;
  internal List<DoubleRange> AxisRanges;
  internal Dictionary<DoubleRange, ChartAxisScaleBreak> BreakRangesInfo;
  internal List<double> AxisLength;
  private List<DoubleRange> dataRanges;
  private List<double> dataPoints;

  public NumericalAxis()
  {
    this.Breaks = new ChartAxisScaleBreak();
    this.AxisScaleBreaks = new ChartAxisScaleBreaks();
    this.BreakShapes = new List<FrameworkElement>();
    this.BreakRangesInfo = new Dictionary<DoubleRange, ChartAxisScaleBreak>();
  }

  public double? Interval
  {
    get => (double?) this.GetValue(NumericalAxis.IntervalProperty);
    set => this.SetValue(NumericalAxis.IntervalProperty, (object) value);
  }

  public double? Minimum
  {
    get => (double?) this.GetValue(NumericalAxis.MinimumProperty);
    set => this.SetValue(NumericalAxis.MinimumProperty, (object) value);
  }

  public double? Maximum
  {
    get => (double?) this.GetValue(NumericalAxis.MaximumProperty);
    set => this.SetValue(NumericalAxis.MaximumProperty, (object) value);
  }

  public NumericalPadding RangePadding
  {
    get => (NumericalPadding) this.GetValue(NumericalAxis.RangePaddingProperty);
    set => this.SetValue(NumericalAxis.RangePaddingProperty, (object) value);
  }

  public bool StartRangeFromZero
  {
    get => (bool) this.GetValue(NumericalAxis.StartRangeFromZeroProperty);
    set => this.SetValue(NumericalAxis.StartRangeFromZeroProperty, (object) value);
  }

  public ChartAxisScaleBreaks AxisScaleBreaks
  {
    get => (ChartAxisScaleBreaks) this.GetValue(NumericalAxis.AxisScaleBreaksProperty);
    set => this.SetValue(NumericalAxis.AxisScaleBreaksProperty, (object) value);
  }

  public ScaleBreakPosition BreakPosition
  {
    get => (ScaleBreakPosition) this.GetValue(NumericalAxis.BreakPositionProperty);
    set => this.SetValue(NumericalAxis.BreakPositionProperty, (object) value);
  }

  internal NumericalPadding ActualRangePadding
  {
    get
    {
      SfChart area = this.Area as SfChart;
      return this.RangePadding == NumericalPadding.Auto && area != null && area.Series != null && area.Series.Count > 0 && (this.Orientation == Orientation.Vertical && !area.Series[0].IsActualTransposed || this.Orientation == Orientation.Horizontal && area.Series[0].IsActualTransposed) ? NumericalPadding.Round : (NumericalPadding) this.GetValue(NumericalAxis.RangePaddingProperty);
    }
  }

  internal ChartAxisScaleBreak Breaks { get; set; }

  public override double CoefficientToValue(double value)
  {
    value = this.IsInversed ? 1.0 - value : value;
    return this.AxisRanges == null || this.AxisRanges.Count <= 0 ? this.VisibleRange.Start + this.VisibleRange.Delta * value : this.CalculateCoefficientToValue(value);
  }

  public override double ValueToCoefficient(double value)
  {
    double start = this.VisibleRange.Start;
    double delta = this.VisibleRange.Delta;
    double num;
    if (this.AxisRanges != null && this.AxisRanges.Count > 0)
    {
      num = this.CalculateValueToCoefficient(value);
    }
    else
    {
      if (delta == 0.0)
        return -1.0;
      num = (value - start) / delta;
    }
    return !this.IsActualInversed ? num : 1.0 - num;
  }

  public override double ValueToPolarCoefficient(double value)
  {
    double start = this.VisibleRange.Start;
    double delta = this.VisibleRange.Delta;
    return this.ValueBasedOnAngle((value - start) / delta);
  }

  public override double PolarCoefficientToValue(double value)
  {
    value = this.ValueBasedOnAngle(value);
    return this.VisibleRange.Start + this.VisibleRange.Delta * value;
  }

  internal override void Dispose()
  {
    if (this.AxisScaleBreaks != null)
    {
      this.AxisScaleBreaks.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnScaleBreakCollectionChanged);
      this.AxisScaleBreaks.Clear();
      this.AxisScaleBreaks = (ChartAxisScaleBreaks) null;
    }
    base.Dispose();
  }

  internal bool BreakExistence()
  {
    return this.AxisScaleBreaks.Count > 0 && this.BreakRanges != null && this.BreakRanges.Count > 0;
  }

  internal void OnScaleBreakChanged(ChartAxisScaleBreaks newValue, ChartAxisScaleBreaks oldValue)
  {
    if (newValue != null)
    {
      newValue.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnScaleBreakCollectionChanged);
      foreach (ChartAxisScaleBreak element in (Collection<ChartAxisScaleBreak>) newValue)
      {
        element.PropertyChanged += new PropertyChangedEventHandler(this.Scalebreak_PropertyChanged);
        if (this.axisPanel != null && !this.axisPanel.Children.Contains((UIElement) element))
          this.axisPanel.Children.Add((UIElement) element);
      }
    }
    if (oldValue != null)
    {
      oldValue.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnScaleBreakCollectionChanged);
      foreach (ChartAxisScaleBreak element in (Collection<ChartAxisScaleBreak>) oldValue)
      {
        element.PropertyChanged -= new PropertyChangedEventHandler(this.Scalebreak_PropertyChanged);
        if (this.axisPanel != null && this.axisPanel.Children.Contains((UIElement) element))
          this.axisPanel.Children.Remove((UIElement) element);
      }
    }
    this.UpdateArea();
  }

  internal void DrawScaleBreakLines()
  {
    if (this.Area == null || this.Area.AreaType != ChartAreaType.CartesianAxes)
      this.ClearBreakElements();
    else
      ChartAxisScaleBreak.DrawPath(this);
  }

  internal void ClearBreakElements()
  {
    if (this.Area == null)
      return;
    if (this.AxisRanges != null)
      this.AxisRanges.Clear();
    if (this.BreakRanges != null)
      this.BreakRanges.Clear();
    if (this.dataRanges != null)
      this.dataRanges.Clear();
    this.BreakRangesInfo.Clear();
    if (this.Area.AdorningCanvas == null)
      return;
    foreach (UIElement breakShape in this.BreakShapes)
      this.Area.AdorningCanvas.Children.Remove(breakShape);
    this.BreakShapes.Clear();
  }

  internal void CalculateScaleBreaksRanges(ObservableCollection<ISupportAxes> series)
  {
    if (this.Area == null || this.AxisScaleBreaks.Count == 0)
      return;
    this.DataRangeDelta = 0.0;
    this.BreakRanges = new List<DoubleRange>();
    this.BreakRangesInfo = new Dictionary<DoubleRange, ChartAxisScaleBreak>();
    this.AxisRanges = new List<DoubleRange>();
    this.dataRanges = new List<DoubleRange>();
    for (int index = 0; index < this.AxisScaleBreaks.Count; ++index)
    {
      if (!double.IsNaN(this.AxisScaleBreaks[index].Start) && !double.IsNaN(this.AxisScaleBreaks[index].End))
      {
        DoubleRange doubleRange = new DoubleRange(this.AxisScaleBreaks[index].Start, this.AxisScaleBreaks[index].End);
        if (this.VisibleRange.Inside(doubleRange) && doubleRange.Delta > 0.0)
          this.ComputeBreakRange(doubleRange, index);
      }
    }
    this.AddDataRange();
    foreach (DoubleRange dataRange in this.dataRanges)
      this.DataRangeDelta += dataRange.Delta;
    this.ComputeAxisHeight(this.dataRanges, this.DataRangeDelta);
  }

  internal void ComputeScaleBreaks()
  {
    if (this.Area != null)
    {
      if (this.Area.AreaType != ChartAreaType.CartesianAxes || this.ZoomFactor != 1.0 || this.ZoomPosition != 0.0 || this.IsDefaultRange)
      {
        this.GenerateDefaultLabel();
      }
      else
      {
        ObservableCollection<ISupportAxes> registeredSeries = this.RegisteredSeries;
        if (registeredSeries.Count == 0 || registeredSeries.Any<ISupportAxes>((Func<ISupportAxes, bool>) (series => !(series is CartesianSeries) || series is StackingSeriesBase)))
        {
          this.GenerateDefaultLabel();
        }
        else
        {
          this.GetSeriesYValues(registeredSeries);
          this.CalculateScaleBreaksRanges(registeredSeries);
          NumericalAxisHelper.GenerateScaleBreakVisibleLabels(this, (object) this.Interval, (double) this.SmallTicksPerInterval);
        }
      }
    }
    else
    {
      this.ClearBreakElements();
      this.GenerateDefaultLabel();
    }
  }

  protected internal override double CalculateActualInterval(DoubleRange range, Size availableSize)
  {
    return !this.Interval.HasValue || Convert.ToDouble((object) this.Interval) == 0.0 || this.AxisRanges != null && this.AxisRanges.Count > 0 ? this.CalculateNiceInterval(range, availableSize) : this.Interval.Value;
  }

  protected internal override void CalculateVisibleRange(Size avalableSize)
  {
    base.CalculateVisibleRange(avalableSize);
    NumericalAxisHelper.CalculateVisibleRange((ChartAxisBase2D) this, avalableSize, (object) this.Interval);
  }

  protected virtual void OnMaximumChanged(DependencyPropertyChangedEventArgs args)
  {
    this.OnMinMaxChanged();
  }

  protected virtual void OnMinimumChanged(DependencyPropertyChangedEventArgs args)
  {
    this.OnMinMaxChanged();
  }

  protected virtual void OnIntervalChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  protected override void GenerateVisibleLabels()
  {
    this.SetRangeForAxisStyle();
    this.ClearBreakElements();
    if (this.AxisScaleBreaks.Count > 0 && this.IsSecondaryAxis && this.CustomLabels.Count == 0 && this.GetLabelSource() == null)
      this.ComputeScaleBreaks();
    else
      NumericalAxisHelper.GenerateVisibleLabels((ChartAxisBase2D) this, (object) this.Minimum, (object) this.Maximum, (object) this.Interval, (double) this.SmallTicksPerInterval);
  }

  protected override DoubleRange CalculateActualRange()
  {
    if (this.ActualRange.IsEmpty)
    {
      DoubleRange actualRange = base.CalculateActualRange();
      if (this.IncludeAnnotationRange)
      {
        foreach (Annotation annotation in (Collection<Annotation>) (this.Area as SfChart).Annotations)
        {
          if (this.Orientation == Orientation.Vertical && annotation.CoordinateUnit == CoordinateUnit.Axis && annotation.YAxis == this)
          {
            DoubleRange doubleRange1 = actualRange;
            double start = Annotation.ConvertData(annotation.Y1, (ChartAxis) this);
            double end;
            switch (annotation)
            {
              case TextAnnotation _:
                end = Annotation.ConvertData(annotation.Y1, (ChartAxis) this);
                break;
              case ImageAnnotation _:
                end = Annotation.ConvertData((annotation as ImageAnnotation).Y2, (ChartAxis) this);
                break;
              default:
                end = Annotation.ConvertData((annotation as ShapeAnnotation).Y2, (ChartAxis) this);
                break;
            }
            DoubleRange doubleRange2 = new DoubleRange(start, end);
            actualRange = doubleRange1 + doubleRange2;
          }
          else if (this.Orientation == Orientation.Horizontal && annotation.CoordinateUnit == CoordinateUnit.Axis && annotation.XAxis == this)
          {
            DoubleRange doubleRange3 = actualRange;
            double start = Annotation.ConvertData(annotation.X1, (ChartAxis) this);
            double end;
            switch (annotation)
            {
              case TextAnnotation _:
                end = Annotation.ConvertData(annotation.X1, (ChartAxis) this);
                break;
              case ImageAnnotation _:
                end = Annotation.ConvertData((annotation as ImageAnnotation).X2, (ChartAxis) this);
                break;
              default:
                end = Annotation.ConvertData((annotation as ShapeAnnotation).X2, (ChartAxis) this);
                break;
            }
            DoubleRange doubleRange4 = new DoubleRange(start, end);
            actualRange = doubleRange3 + doubleRange4;
          }
        }
      }
      return actualRange;
    }
    if (this.Minimum.HasValue && this.Maximum.HasValue)
      return this.ActualRange;
    DoubleRange actualRange1 = base.CalculateActualRange();
    if (this.StartRangeFromZero && actualRange1.Start > 0.0)
      return new DoubleRange(0.0, actualRange1.End);
    if (this.Minimum.HasValue)
    {
      DoubleRange doubleRange = new DoubleRange(this.ActualRange.Start, double.IsNaN(actualRange1.End) ? this.ActualRange.Start + 1.0 : actualRange1.End);
      double start = Convert.ToDouble((object) this.Minimum);
      return doubleRange.Start != start ? new DoubleRange(start, this.ActualRange.Start + 1.0) : doubleRange;
    }
    if (this.Maximum.HasValue)
    {
      DoubleRange doubleRange = new DoubleRange(double.IsNaN(actualRange1.Start) ? this.ActualRange.End - 1.0 : actualRange1.Start, this.ActualRange.End);
      double end = Convert.ToDouble((object) this.Maximum);
      return doubleRange.End != end ? new DoubleRange(this.ActualRange.End - 1.0, end) : doubleRange;
    }
    if (this.IncludeAnnotationRange)
    {
      foreach (Annotation annotation in (Collection<Annotation>) (this.Area as SfChart).Annotations)
      {
        if (this.Orientation == Orientation.Vertical && annotation.CoordinateUnit == CoordinateUnit.Axis && annotation.YAxis == this)
        {
          DoubleRange doubleRange5 = actualRange1;
          double start = Annotation.ConvertData(annotation.Y1, (ChartAxis) this);
          double end;
          switch (annotation)
          {
            case TextAnnotation _:
              end = Annotation.ConvertData(annotation.Y1, (ChartAxis) this);
              break;
            case ImageAnnotation _:
              end = Annotation.ConvertData((annotation as ImageAnnotation).Y2, (ChartAxis) this);
              break;
            default:
              end = Annotation.ConvertData((annotation as ShapeAnnotation).Y2, (ChartAxis) this);
              break;
          }
          DoubleRange doubleRange6 = new DoubleRange(start, end);
          actualRange1 = doubleRange5 + doubleRange6;
        }
        else if (this.Orientation == Orientation.Horizontal && annotation.CoordinateUnit == CoordinateUnit.Axis && annotation.XAxis == this)
        {
          DoubleRange doubleRange7 = actualRange1;
          double start = Annotation.ConvertData(annotation.X1, (ChartAxis) this);
          double end;
          switch (annotation)
          {
            case TextAnnotation _:
              end = Annotation.ConvertData(annotation.X1, (ChartAxis) this);
              break;
            case ImageAnnotation _:
              end = Annotation.ConvertData((annotation as ImageAnnotation).X2, (ChartAxis) this);
              break;
            default:
              end = Annotation.ConvertData((annotation as ShapeAnnotation).X2, (ChartAxis) this);
              break;
          }
          DoubleRange doubleRange8 = new DoubleRange(start, end);
          actualRange1 = doubleRange7 + doubleRange8;
        }
      }
    }
    return actualRange1;
  }

  protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
  {
    if (!this.Minimum.HasValue && !this.Maximum.HasValue)
      return NumericalAxisHelper.ApplyRangePadding((ChartAxis) this, base.ApplyRangePadding(range, interval), interval, this.ActualRangePadding);
    if (this.Minimum.HasValue && this.Maximum.HasValue)
      return range;
    DoubleRange doubleRange = NumericalAxisHelper.ApplyRangePadding((ChartAxis) this, base.ApplyRangePadding(range, interval), interval, this.ActualRangePadding);
    return !this.Minimum.HasValue ? new DoubleRange(doubleRange.Start, range.End) : new DoubleRange(range.Start, doubleRange.End);
  }

  protected override DependencyObject CloneAxis(DependencyObject obj)
  {
    NumericalAxis numericalAxis = new NumericalAxis();
    numericalAxis.Minimum = this.Minimum;
    numericalAxis.Maximum = this.Maximum;
    numericalAxis.StartRangeFromZero = this.StartRangeFromZero;
    numericalAxis.Interval = this.Interval;
    numericalAxis.RangePadding = this.RangePadding;
    foreach (ChartAxisScaleBreak axisScaleBreak in (Collection<ChartAxisScaleBreak>) this.AxisScaleBreaks)
      numericalAxis.AxisScaleBreaks.Add((ChartAxisScaleBreak) axisScaleBreak.Clone());
    obj = (DependencyObject) numericalAxis;
    return base.CloneAxis(obj);
  }

  private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as NumericalAxis).OnPropertyChanged();
  }

  private static void OnScaleBreakChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as NumericalAxis).OnScaleBreakChanged(e.NewValue as ChartAxisScaleBreaks, e.OldValue as ChartAxisScaleBreaks);
  }

  private static void OnBreakPositionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    NumericalAxis numericalAxis = d as NumericalAxis;
    if (numericalAxis.Area == null)
      return;
    numericalAxis.Area.ScheduleUpdate();
  }

  private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as NumericalAxis).OnMinimumChanged(e);
  }

  private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as NumericalAxis).OnMaximumChanged(e);
  }

  private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as NumericalAxis).OnIntervalChanged(e);
  }

  private void Scalebreak_PropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    this.UpdateArea();
  }

  private void OnScaleBreakCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == NotifyCollectionChangedAction.Add)
    {
      foreach (ChartAxisScaleBreak newItem in (IEnumerable) e.NewItems)
      {
        newItem.PropertyChanged += new PropertyChangedEventHandler(this.Scalebreak_PropertyChanged);
        if (this.axisPanel != null && !this.axisPanel.Children.Contains((UIElement) newItem))
          this.axisPanel.Children.Add((UIElement) newItem);
      }
      if (this.Area == null)
        return;
      this.UpdateArea();
    }
    else if (e.Action == NotifyCollectionChangedAction.Remove)
    {
      foreach (ChartAxisScaleBreak oldItem in (IEnumerable) e.OldItems)
      {
        oldItem.PropertyChanged -= new PropertyChangedEventHandler(this.Scalebreak_PropertyChanged);
        if (this.axisPanel != null && this.axisPanel.Children.Contains((UIElement) oldItem))
          this.axisPanel.Children.Remove((UIElement) oldItem);
      }
      this.UpdateArea();
    }
    else if (e.Action == NotifyCollectionChangedAction.Reset)
    {
      if (this.Area.AdorningCanvas == null)
        return;
      UIElement[] uiElementArray = new UIElement[this.axisPanel.Children.Count];
      this.axisPanel.Children.CopyTo(uiElementArray, 0);
      foreach (ChartAxisScaleBreak element in ((IEnumerable<UIElement>) uiElementArray).Where<UIElement>((Func<UIElement, bool>) (it => it is ChartAxisScaleBreak)))
      {
        if (this.axisPanel.Children.Contains((UIElement) element))
        {
          element.PropertyChanged -= new PropertyChangedEventHandler(this.Scalebreak_PropertyChanged);
          this.axisPanel.Children.Remove((UIElement) element);
        }
      }
      this.UpdateArea();
    }
    else
    {
      if (e.Action != NotifyCollectionChangedAction.Replace)
        return;
      if (e.OldItems[0] is ChartAxisScaleBreak oldItem && this.axisPanel != null && this.axisPanel.Children.Contains((UIElement) oldItem))
      {
        oldItem.PropertyChanged -= new PropertyChangedEventHandler(this.Scalebreak_PropertyChanged);
        this.axisPanel.Children.Remove((UIElement) oldItem);
      }
      if (e.NewItems[0] is ChartAxisScaleBreak newItem && this.axisPanel != null && !this.axisPanel.Children.Contains((UIElement) newItem))
      {
        newItem.PropertyChanged += new PropertyChangedEventHandler(this.Scalebreak_PropertyChanged);
        this.axisPanel.Children.Add((UIElement) newItem);
      }
      this.UpdateArea();
    }
  }

  private void UpdateArea()
  {
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  private void OnMinMaxChanged()
  {
    NumericalAxisHelper.OnMinMaxChanged((ChartAxis) this, (object) this.Maximum, (object) this.Minimum);
  }

  private void ComputeBreakRange(DoubleRange currentRange, int index)
  {
    bool flag = true;
    int index1 = 0;
    while (index1 < this.BreakRanges.Count)
    {
      DoubleRange breakRange = this.BreakRanges[index1];
      if (currentRange.Inside(breakRange))
      {
        this.BreakRangesInfo.Remove(this.BreakRanges[index1]);
        this.BreakRanges.RemoveAt(index1);
      }
      else
      {
        if (breakRange.Inside(currentRange))
        {
          flag = false;
          break;
        }
        if (breakRange.Intersects(currentRange))
        {
          currentRange += breakRange;
          this.BreakRangesInfo.Remove(this.BreakRanges[index1]);
          this.BreakRanges.RemoveAt(index1);
        }
        else
          ++index1;
      }
    }
    if (!flag || currentRange.Delta == 0.0)
      return;
    this.BreakRanges.Add(currentRange);
    this.BreakRangesInfo.Add(currentRange, this.AxisScaleBreaks[index]);
  }

  private double CalculateCoefficientToValue(double value)
  {
    double coefficientToValue = double.NaN;
    double num1 = 0.0;
    double num2 = 5.0;
    double num3 = this.Orientation == Orientation.Horizontal ? this.AvailableSize.Width : this.AvailableSize.Height;
    for (int index = 0; index < this.AxisRanges.Count; ++index)
    {
      DoubleRange doubleRange = DoubleRange.Empty;
      if (this.BreakRanges != null && index < this.BreakRanges.Count)
      {
        num2 = this.BreakRangesInfo[this.BreakRanges[index]].BreakSpacing;
        doubleRange = new DoubleRange(this.ValueToCoefficient(this.BreakRanges[index].Start), this.ValueToCoefficient(this.BreakRanges[index].End));
      }
      if (new DoubleRange(this.ValueToCoefficient(this.AxisRanges[index].Start), this.ValueToCoefficient(this.AxisRanges[index].End)).Inside(value))
      {
        coefficientToValue = this.AxisRanges[index].Start + this.AxisRanges[index].Delta * ((value - num1) / (this.AxisLength[index] / num3));
        break;
      }
      if (value > 1.0)
      {
        coefficientToValue = this.AxisRanges[this.AxisRanges.Count - 1].Start + this.AxisRanges[this.AxisRanges.Count - 1].Delta * ((value - num1) / (this.AxisLength.Last<double>() / num3));
        break;
      }
      if (value < 0.0)
      {
        coefficientToValue = this.AxisRanges[0].Start + this.AxisRanges[0].Delta * ((value - num1) / (this.AxisLength.First<double>() / num3));
        break;
      }
      if (index < this.BreakRanges.Count && value > doubleRange.Start && value < doubleRange.End)
      {
        coefficientToValue = this.BreakRanges[index].Start + this.BreakRanges[index].Delta * ((value - num1) / this.BreakRangesInfo[this.BreakRanges[index]].BreakSpacing / num3);
        break;
      }
      num1 += (this.AxisLength[index] + num2) / num3;
    }
    return coefficientToValue;
  }

  internal double CalculateValueToCoefficient(double value)
  {
    double valueToCoefficient = 0.0;
    double num1 = 5.0;
    DoubleRange empty = DoubleRange.Empty;
    double num2 = this.Orientation == Orientation.Horizontal ? this.AvailableSize.Width : this.AvailableSize.Height;
    foreach (DoubleRange axisRange in this.AxisRanges)
      empty += axisRange;
    for (int index = 0; index < this.AxisRanges.Count; ++index)
    {
      if (this.BreakRanges != null && index < this.BreakRanges.Count)
        num1 = this.BreakRangesInfo[this.BreakRanges[index]].BreakSpacing;
      if (empty.Inside(value))
      {
        if (this.AxisRanges[index].Inside(value))
        {
          double start = this.AxisRanges[index].Start;
          double delta = this.AxisRanges[index].Delta;
          valueToCoefficient += this.AxisLength[index] / num2 * (value - start) / delta;
          break;
        }
        if (this.BreakRanges != null && index < this.BreakRanges.Count && value > this.BreakRanges[index].Start && value < this.BreakRanges[index].End)
        {
          valueToCoefficient = valueToCoefficient + this.AxisLength[index] / num2 + (value - this.BreakRanges[index].Start) / this.BreakRanges[index].Delta * (num1 / num2);
          break;
        }
        valueToCoefficient += (this.AxisLength[index] + num1) / num2;
      }
      else
        valueToCoefficient = (value - empty.Start) / empty.Delta;
    }
    return valueToCoefficient;
  }

  private void GenerateDefaultLabel()
  {
    this.VisibleLabels.Clear();
    this.SmallTickPoints.Clear();
    NumericalAxisHelper.GenerateVisibleLabels((ChartAxisBase2D) this, (object) this.Minimum, (object) this.Maximum, (object) this.Interval, (double) this.SmallTicksPerInterval);
  }

  private void GetSeriesYValues(
    ObservableCollection<ISupportAxes> registeredSeries)
  {
    foreach (ISupportAxes supportAxes in (Collection<ISupportAxes>) registeredSeries)
    {
      this.dataPoints = new List<double>();
      if (supportAxes.ActualYAxis == this)
      {
        switch (supportAxes)
        {
          case BoxAndWhiskerSeries _:
            List<IList<double>> ycollection = (supportAxes as BoxAndWhiskerSeries).YCollection;
            if (ycollection != null)
            {
              for (int index = 0; index < ycollection.Count<IList<double>>(); ++index)
              {
                foreach (double num in (IEnumerable<double>) ycollection[index])
                  this.dataPoints.Add(num);
              }
              continue;
            }
            continue;
          case WaterfallSeries _:
          case ErrorBarSeries _:
            using (IEnumerator<ChartSegment> enumerator = (!(supportAxes is WaterfallSeries) ? (Collection<ChartSegment>) (supportAxes as ErrorBarSeries).Segments : (Collection<ChartSegment>) (supportAxes as WaterfallSeries).Segments).GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                ChartSegment current = enumerator.Current;
                if (!this.dataPoints.Contains(current.YRange.Start))
                  this.dataPoints.Add(current.YRange.Start);
                if (!this.dataPoints.Contains(current.YRange.End))
                  this.dataPoints.Add(current.YRange.End);
              }
              continue;
            }
          case CartesianSeries cartesianSeries:
            if (cartesianSeries.ActualSeriesYValues != null)
            {
              for (int index = 0; index < ((IEnumerable<IList<double>>) cartesianSeries.ActualSeriesYValues).Count<IList<double>>(); ++index)
              {
                foreach (double num in (IEnumerable<double>) cartesianSeries.ActualSeriesYValues[index])
                  this.dataPoints.Add(num);
              }
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
  }

  private void ComputeAxisHeight(List<DoubleRange> axisSegment, double delta)
  {
    if (axisSegment != null && axisSegment.Count == 0)
      return;
    this.AxisLength = new List<double>();
    double num1 = 0.0;
    double num2 = 5.0;
    double num3 = 50.0;
    double num4 = this.Orientation == Orientation.Horizontal ? this.AvailableSize.Width : this.AvailableSize.Height;
    foreach (DoubleRange doubleRange in axisSegment)
    {
      foreach (double dataPoint in this.dataPoints)
      {
        if (doubleRange.Inside(dataPoint))
          ++num1;
      }
    }
    for (int i = 0; i < axisSegment.Count; ++i)
    {
      if (i < this.BreakRanges.Count)
      {
        num2 = this.BreakRangesInfo[this.BreakRanges[i]].BreakSpacing;
        num3 = this.BreakRangesInfo[this.BreakRanges[i]].BreakPercent;
        if (num3 < 0.0 || num3 > 100.0)
          num3 = 50.0;
      }
      int num5 = this.dataPoints.Count<double>((Func<double, bool>) (item => axisSegment[i].Inside(item)));
      double num6;
      switch (this.BreakPosition)
      {
        case ScaleBreakPosition.Scale:
          num6 = num4 * axisSegment[i].Delta / delta;
          break;
        case ScaleBreakPosition.Percent:
          num6 = this.BreakRanges.Count <= i ? num4 : num4 * num3 / 100.0;
          num4 -= num6;
          break;
        default:
          num6 = axisSegment.Count == 1 ? num4 : (0.5 / (double) axisSegment.Count + (double) num5 / num1 * 0.5) * num4;
          break;
      }
      if (this.BreakRanges.Count > 0)
      {
        if (i == 0 || i == axisSegment.Count - 1)
          num6 -= num2 / 2.0;
        else
          num6 -= this.BreakRangesInfo[this.BreakRanges[i - 1]].BreakSpacing / 2.0 + this.BreakRangesInfo[this.BreakRanges[i]].BreakSpacing / 2.0;
      }
      double num7 = num6 < 0.0 ? 0.0 : num6;
      if (this.AxisLength != null)
        this.AxisLength.Add(num7);
    }
    this.AxisRanges = axisSegment;
  }

  private void AddDataRange()
  {
    this.dataRanges = new List<DoubleRange>();
    List<DoubleRange> list = this.BreakRanges.OrderBy<DoubleRange, double>((Func<DoubleRange, double>) (i => i.Start)).ToList<DoubleRange>();
    this.BreakRanges.Clear();
    this.BreakRanges = list.ToList<DoubleRange>();
    for (int index = 0; index < list.Count; ++index)
    {
      if (index == 0)
      {
        if (list[index].Start > this.VisibleRange.Start)
          this.dataRanges.Add(new DoubleRange(this.VisibleRange.Start, list[index].Start));
        else
          this.BreakRanges.RemoveAt(index);
      }
      else
        this.dataRanges.Add(new DoubleRange(list[index - 1].End, list[index].Start));
    }
    if (list.Count <= 0)
      return;
    if (list[list.Count - 1].End < this.VisibleRange.End)
      this.dataRanges.Add(new DoubleRange(list[list.Count - 1].End, this.VisibleRange.End));
    else
      this.BreakRanges.Remove(list[list.Count - 1]);
  }
}
