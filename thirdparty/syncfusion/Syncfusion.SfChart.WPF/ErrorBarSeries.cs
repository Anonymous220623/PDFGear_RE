// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ErrorBarSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ErrorBarSeries : XyDataSeries
{
  public static readonly DependencyProperty HorizontalErrorPathProperty = DependencyProperty.Register(nameof (HorizontalErrorPath), typeof (string), typeof (ErrorBarSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(ErrorBarSeries.OnYPathChanged)));
  public static readonly DependencyProperty VerticalErrorPathProperty = DependencyProperty.Register(nameof (VerticalErrorPath), typeof (string), typeof (ErrorBarSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(ErrorBarSeries.OnYPathChanged)));
  public static readonly DependencyProperty HorizontalLineStyleProperty = DependencyProperty.Register(nameof (HorizontalLineStyle), typeof (LineStyle), typeof (ErrorBarSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(ErrorBarSeries.OnHorizontalPropertyChanged)));
  public static readonly DependencyProperty VerticalLineStyleProperty = DependencyProperty.Register(nameof (VerticalLineStyle), typeof (LineStyle), typeof (ErrorBarSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(ErrorBarSeries.OnVerticalPropertyChanged)));
  public static readonly DependencyProperty HorizontalCapLineStyleProperty = DependencyProperty.Register(nameof (HorizontalCapLineStyle), typeof (CapLineStyle), typeof (ErrorBarSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(ErrorBarSeries.OnHorizontalCapPropertyChanged)));
  public static readonly DependencyProperty VerticalCapLineStyleProperty = DependencyProperty.Register(nameof (VerticalCapLineStyle), typeof (CapLineStyle), typeof (ErrorBarSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(ErrorBarSeries.OnVerticalCapPropertyChanged)));
  public static readonly DependencyProperty HorizontalErrorValueProperty = DependencyProperty.Register(nameof (HorizontalErrorValue), typeof (double), typeof (ErrorBarSeries), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ErrorBarSeries.OnHorizontalErrorValuePropertyChanged)));
  public static readonly DependencyProperty VerticalErrorValueProperty = DependencyProperty.Register(nameof (VerticalErrorValue), typeof (double), typeof (ErrorBarSeries), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ErrorBarSeries.OnVerticalErrorValuePropertyChanged)));
  public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof (Mode), typeof (ErrorBarMode), typeof (ErrorBarSeries), new PropertyMetadata((object) ErrorBarMode.Both, new PropertyChangedCallback(ErrorBarSeries.OnPropertyChanged)));
  public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof (Type), typeof (ErrorBarType), typeof (ErrorBarSeries), new PropertyMetadata((object) ErrorBarType.Fixed, new PropertyChangedCallback(ErrorBarSeries.OnPropertyChanged)));
  public static readonly DependencyProperty HorizontalDirectionProperty = DependencyProperty.Register(nameof (HorizontalDirection), typeof (ErrorBarDirection), typeof (ErrorBarSeries), new PropertyMetadata((object) ErrorBarDirection.Both, new PropertyChangedCallback(ErrorBarSeries.OnPropertyChanged)));
  public static readonly DependencyProperty VerticalDirectionProperty = DependencyProperty.Register(nameof (VerticalDirection), typeof (ErrorBarDirection), typeof (ErrorBarSeries), new PropertyMetadata((object) ErrorBarDirection.Both, new PropertyChangedCallback(ErrorBarSeries.OnPropertyChanged)));
  private double _horizontalErrorValue;

  public ErrorBarSeries()
  {
    this.DefaultStyleKey = (object) typeof (ErrorBarSeries);
    this.HorizontalCustomValues = (IList<double>) new List<double>();
    this.VerticalCustomValues = (IList<double>) new List<double>();
  }

  public string HorizontalErrorPath
  {
    get => (string) this.GetValue(ErrorBarSeries.HorizontalErrorPathProperty);
    set => this.SetValue(ErrorBarSeries.HorizontalErrorPathProperty, (object) value);
  }

  public string VerticalErrorPath
  {
    get => (string) this.GetValue(ErrorBarSeries.VerticalErrorPathProperty);
    set => this.SetValue(ErrorBarSeries.VerticalErrorPathProperty, (object) value);
  }

  public LineStyle HorizontalLineStyle
  {
    get => (LineStyle) this.GetValue(ErrorBarSeries.HorizontalLineStyleProperty);
    set => this.SetValue(ErrorBarSeries.HorizontalLineStyleProperty, (object) value);
  }

  public LineStyle VerticalLineStyle
  {
    get => (LineStyle) this.GetValue(ErrorBarSeries.VerticalLineStyleProperty);
    set => this.SetValue(ErrorBarSeries.VerticalLineStyleProperty, (object) value);
  }

  public CapLineStyle HorizontalCapLineStyle
  {
    get => (CapLineStyle) this.GetValue(ErrorBarSeries.HorizontalCapLineStyleProperty);
    set => this.SetValue(ErrorBarSeries.HorizontalCapLineStyleProperty, (object) value);
  }

  public CapLineStyle VerticalCapLineStyle
  {
    get => (CapLineStyle) this.GetValue(ErrorBarSeries.VerticalCapLineStyleProperty);
    set => this.SetValue(ErrorBarSeries.VerticalCapLineStyleProperty, (object) value);
  }

  public double HorizontalErrorValue
  {
    get => (double) this.GetValue(ErrorBarSeries.HorizontalErrorValueProperty);
    set => this.SetValue(ErrorBarSeries.HorizontalErrorValueProperty, (object) value);
  }

  public double VerticalErrorValue
  {
    get => (double) this.GetValue(ErrorBarSeries.VerticalErrorValueProperty);
    set => this.SetValue(ErrorBarSeries.VerticalErrorValueProperty, (object) value);
  }

  public ErrorBarMode Mode
  {
    get => (ErrorBarMode) this.GetValue(ErrorBarSeries.ModeProperty);
    set => this.SetValue(ErrorBarSeries.ModeProperty, (object) value);
  }

  public ErrorBarType Type
  {
    get => (ErrorBarType) this.GetValue(ErrorBarSeries.TypeProperty);
    set => this.SetValue(ErrorBarSeries.TypeProperty, (object) value);
  }

  public ErrorBarDirection HorizontalDirection
  {
    get => (ErrorBarDirection) this.GetValue(ErrorBarSeries.HorizontalDirectionProperty);
    set => this.SetValue(ErrorBarSeries.HorizontalDirectionProperty, (object) value);
  }

  public ErrorBarDirection VerticalDirection
  {
    get => (ErrorBarDirection) this.GetValue(ErrorBarSeries.VerticalDirectionProperty);
    set => this.SetValue(ErrorBarSeries.VerticalDirectionProperty, (object) value);
  }

  internal override bool IsMultipleYPathRequired => true;

  internal string HorizontalErrorMemberPath { get; set; }

  internal string VerticalErrorMemberPath { get; set; }

  protected internal IList<double> HorizontalCustomValues { get; set; }

  protected internal IList<double> VerticalCustomValues { get; set; }

  public override void CreateSegments()
  {
    List<double> xvalues = this.GetXValues();
    if (xvalues == null)
      return;
    double[] sdErrorValue1 = this.GetSdErrorValue((IList<double>) xvalues);
    double[] sdErrorValue2 = this.GetSdErrorValue(this.YValues);
    double errorvalue;
    if (this.Type == ErrorBarType.StandardErrors || this.Type == ErrorBarType.StandardDeviation)
    {
      this._horizontalErrorValue = sdErrorValue1[1];
      errorvalue = sdErrorValue2[1];
    }
    else
    {
      this._horizontalErrorValue = this.HorizontalErrorValue;
      errorvalue = this.VerticalErrorValue;
    }
    if (this.Segments.Count > this.DataCount)
      this.ClearUnUsedSegments(this.DataCount);
    if (this.ActualXAxis is DateTimeAxis actualXaxis && actualXaxis.IntervalType == DateTimeIntervalType.Auto)
      actualXaxis.ActualRangeChanged += new EventHandler<ActualRangeChangedEventArgs>(this.ErrorBarSeries_ActualRangeChanged);
    else if (actualXaxis != null && actualXaxis.IntervalType != DateTimeIntervalType.Auto)
      actualXaxis.ActualRangeChanged -= new EventHandler<ActualRangeChangedEventArgs>(this.ErrorBarSeries_ActualRangeChanged);
    for (int index = 0; index < this.DataCount; ++index)
    {
      switch (this.Type)
      {
        case ErrorBarType.Percentage:
          if (this.ActualXAxis is DateTimeAxis || this.ActualXAxis is CategoryAxis || this.ActualXAxis is DateTimeCategoryAxis)
          {
            this._horizontalErrorValue = ErrorBarSeries.GetPercentageErrorBarValue((double) (index + 1), this.HorizontalErrorValue);
            errorvalue = ErrorBarSeries.GetPercentageErrorBarValue(this.YValues[index], this.VerticalErrorValue);
            break;
          }
          this._horizontalErrorValue = ErrorBarSeries.GetPercentageErrorBarValue(xvalues[index], this.HorizontalErrorValue);
          errorvalue = ErrorBarSeries.GetPercentageErrorBarValue(this.YValues[index], this.VerticalErrorValue);
          break;
        case ErrorBarType.Custom:
          this._horizontalErrorValue = this.HorizontalCustomValues.Count > 0 ? this.HorizontalCustomValues[index] : 0.0;
          errorvalue = this.VerticalCustomValues.Count > 0 ? this.VerticalCustomValues[index] : 0.0;
          break;
      }
      ChartPoint point1;
      ChartPoint point2;
      ChartPoint point3;
      ChartPoint point4;
      if (this.Type == ErrorBarType.StandardDeviation)
      {
        if (this.HorizontalDirection == ErrorBarDirection.Plus)
        {
          point1 = new ChartPoint(sdErrorValue1[0], this.YValues[index]);
          point2 = new ChartPoint(this.GetPlusValue(sdErrorValue1[0], this._horizontalErrorValue, true), this.YValues[index]);
        }
        else if (this.HorizontalDirection == ErrorBarDirection.Minus)
        {
          point1 = new ChartPoint(this.GetMinusValue(sdErrorValue1[0], this._horizontalErrorValue, true), this.YValues[index]);
          point2 = new ChartPoint(sdErrorValue1[0], this.YValues[index]);
        }
        else
        {
          point1 = new ChartPoint(this.GetMinusValue(sdErrorValue1[0], this._horizontalErrorValue, true), this.YValues[index]);
          point2 = new ChartPoint(this.GetPlusValue(sdErrorValue1[0], this._horizontalErrorValue, true), this.YValues[index]);
        }
        if (this.VerticalDirection == ErrorBarDirection.Plus)
        {
          point3 = new ChartPoint(xvalues[index], sdErrorValue2[0]);
          point4 = new ChartPoint(xvalues[index], this.GetPlusValue(sdErrorValue2[0], errorvalue, false));
        }
        else if (this.VerticalDirection == ErrorBarDirection.Minus)
        {
          point3 = new ChartPoint(xvalues[index], this.GetMinusValue(sdErrorValue2[0], errorvalue, false));
          point4 = new ChartPoint(xvalues[index], sdErrorValue2[0]);
        }
        else
        {
          point3 = new ChartPoint(xvalues[index], this.GetMinusValue(sdErrorValue2[0], errorvalue, false));
          point4 = new ChartPoint(xvalues[index], this.GetPlusValue(sdErrorValue2[0], errorvalue, false));
        }
      }
      else
      {
        if (this.HorizontalDirection == ErrorBarDirection.Plus)
        {
          point1 = new ChartPoint(xvalues[index], this.YValues[index]);
          point2 = new ChartPoint(this.GetPlusValue(xvalues[index], this._horizontalErrorValue, true), this.YValues[index]);
        }
        else if (this.HorizontalDirection == ErrorBarDirection.Minus)
        {
          point1 = new ChartPoint(this.GetMinusValue(xvalues[index], this._horizontalErrorValue, true), this.YValues[index]);
          point2 = new ChartPoint(xvalues[index], this.YValues[index]);
        }
        else
        {
          point1 = new ChartPoint(this.GetMinusValue(xvalues[index], this._horizontalErrorValue, true), this.YValues[index]);
          point2 = new ChartPoint(this.GetPlusValue(xvalues[index], this._horizontalErrorValue, true), this.YValues[index]);
        }
        if (this.VerticalDirection == ErrorBarDirection.Plus)
        {
          point3 = new ChartPoint(xvalues[index], this.YValues[index]);
          point4 = new ChartPoint(xvalues[index], this.GetPlusValue(this.YValues[index], errorvalue, false));
        }
        else if (this.VerticalDirection == ErrorBarDirection.Minus)
        {
          point3 = new ChartPoint(xvalues[index], this.GetMinusValue(this.YValues[index], errorvalue, false));
          point4 = new ChartPoint(xvalues[index], this.YValues[index]);
        }
        else
        {
          point3 = new ChartPoint(xvalues[index], this.GetMinusValue(this.YValues[index], errorvalue, false));
          point4 = new ChartPoint(xvalues[index], this.GetPlusValue(this.YValues[index], errorvalue, false));
        }
      }
      if (index < this.Segments.Count)
        this.Segments[index].SetData(point1, point2, point3, point4);
      else if (this.CreateSegment() is ErrorBarSegment segment)
      {
        segment.Series = (ChartSeriesBase) this;
        segment.Item = this.ActualData[index];
        segment.SetData(point1, point2, point3, point4);
        this.Segments.Add((ChartSegment) segment);
      }
    }
  }

  protected internal override void GeneratePoints()
  {
    this.HorizontalCustomValues.Clear();
    this.VerticalCustomValues.Clear();
    this.YValues.Clear();
    if (this.YBindingPath != null && this.HorizontalErrorPath != null && this.VerticalErrorPath != null)
      this.GeneratePoints(new string[3]
      {
        this.YBindingPath,
        this.HorizontalErrorPath,
        this.VerticalErrorPath
      }, this.YValues, this.HorizontalCustomValues, this.VerticalCustomValues);
    else if (this.YBindingPath != null && this.HorizontalErrorPath != null)
      this.GeneratePoints(new string[2]
      {
        this.YBindingPath,
        this.HorizontalErrorPath
      }, this.YValues, this.HorizontalCustomValues);
    else if (this.YBindingPath != null && this.VerticalErrorPath != null)
    {
      this.GeneratePoints(new string[2]
      {
        this.YBindingPath,
        this.VerticalErrorPath
      }, this.YValues, this.VerticalCustomValues);
    }
    else
    {
      if (this.YBindingPath == null)
        return;
      this.GeneratePoints(new string[1]{ this.YBindingPath }, this.YValues);
    }
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new ErrorBarSegment();

  protected override void OnXAxisChanged(ChartAxis oldAxis, ChartAxis newAxis)
  {
    base.OnXAxisChanged(oldAxis, newAxis);
    if (newAxis is DateTimeAxis)
      newAxis.ActualRangeChanged += new EventHandler<ActualRangeChangedEventArgs>(this.ErrorBarSeries_ActualRangeChanged);
    if (!(oldAxis is DateTimeAxis))
      return;
    oldAxis.ActualRangeChanged -= new EventHandler<ActualRangeChangedEventArgs>(this.ErrorBarSeries_ActualRangeChanged);
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    this.HorizontalCustomValues.Clear();
    this.VerticalCustomValues.Clear();
    this.YValues.Clear();
    if (this.YBindingPath != null && this.HorizontalErrorPath != null && this.VerticalErrorPath != null)
      this.GeneratePoints(new string[3]
      {
        this.YBindingPath,
        this.HorizontalErrorPath,
        this.VerticalErrorPath
      }, this.YValues, this.HorizontalCustomValues, this.VerticalCustomValues);
    else if (this.YBindingPath != null && this.HorizontalErrorPath != null)
      this.GeneratePoints(new string[2]
      {
        this.YBindingPath,
        this.HorizontalErrorPath
      }, this.YValues, this.HorizontalCustomValues);
    else if (this.YBindingPath != null && this.VerticalErrorPath != null)
      this.GeneratePoints(new string[2]
      {
        this.YBindingPath,
        this.VerticalErrorPath
      }, this.YValues, this.VerticalCustomValues);
    else if (this.YBindingPath != null)
      this.GeneratePoints(new string[1]{ this.YBindingPath }, this.YValues);
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.HorizontalCustomValues.Clear();
    this.VerticalCustomValues.Clear();
    this.YValues.Clear();
    base.OnBindingPathChanged(args);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    if (obj is ErrorBarSeries errorBarSeries)
    {
      errorBarSeries.HorizontalErrorPath = this.HorizontalErrorPath;
      errorBarSeries.VerticalErrorPath = this.VerticalErrorPath;
      errorBarSeries.Type = this.Type;
      errorBarSeries.Mode = this.Mode;
      errorBarSeries.HorizontalErrorValue = this.HorizontalErrorValue;
      errorBarSeries.VerticalErrorValue = this.VerticalErrorValue;
      errorBarSeries.HorizontalLineStyle = this.HorizontalLineStyle;
      errorBarSeries.VerticalLineStyle = this.VerticalLineStyle;
      errorBarSeries.HorizontalCapLineStyle = this.HorizontalCapLineStyle;
      errorBarSeries.VerticalCapLineStyle = this.VerticalCapLineStyle;
    }
    return base.CloneSeries(obj);
  }

  private static double GetPercentageErrorBarValue(double value, double errorValue)
  {
    return value * (errorValue / 100.0);
  }

  private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ErrorBarSeries errorBarSeries) || e.NewValue == null)
      return;
    errorBarSeries.OnBindingPathChanged(e);
  }

  private static void OnHorizontalErrorValuePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ErrorBarSeries errorBarSeries) || errorBarSeries.Area == null || errorBarSeries.Type != ErrorBarType.Fixed && errorBarSeries.Type != ErrorBarType.Percentage || errorBarSeries.Mode == ErrorBarMode.Vertical)
      return;
    errorBarSeries.Area.ScheduleUpdate();
  }

  private static void OnVerticalErrorValuePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ErrorBarSeries errorBarSeries) || errorBarSeries.Area == null || errorBarSeries.Type != ErrorBarType.Fixed && errorBarSeries.Type != ErrorBarType.Percentage || errorBarSeries.Mode == ErrorBarMode.Horizontal)
      return;
    errorBarSeries.Area.ScheduleUpdate();
  }

  private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ErrorBarSeries errorBarSeries) || errorBarSeries.Area == null)
      return;
    errorBarSeries.Area.ScheduleUpdate();
  }

  private static void OnHorizontalPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ErrorBarSeries errorBarSeries) || errorBarSeries.HorizontalLineStyle == null)
      return;
    errorBarSeries.HorizontalLineStyle.Series = (ChartSeriesBase) errorBarSeries;
    foreach (ErrorBarSegment segment in (Collection<ChartSegment>) errorBarSeries.Segments)
      segment.UpdateVisualBinding();
    if (errorBarSeries.Area == null)
      return;
    errorBarSeries.Area.ScheduleUpdate();
  }

  private static void OnHorizontalCapPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ErrorBarSeries errorBarSeries) || errorBarSeries.HorizontalCapLineStyle == null)
      return;
    errorBarSeries.HorizontalCapLineStyle.Series = (ChartSeriesBase) errorBarSeries;
    foreach (ErrorBarSegment segment in (Collection<ChartSegment>) errorBarSeries.Segments)
      segment.UpdateVisualBinding();
    if (errorBarSeries.Area == null)
      return;
    errorBarSeries.Area.ScheduleUpdate();
  }

  private static void OnVerticalPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ErrorBarSeries errorBarSeries) || errorBarSeries.VerticalLineStyle == null)
      return;
    errorBarSeries.VerticalLineStyle.Series = (ChartSeriesBase) errorBarSeries;
    foreach (ErrorBarSegment segment in (Collection<ChartSegment>) errorBarSeries.Segments)
      segment.UpdateVisualBinding();
    if (errorBarSeries.Area == null)
      return;
    errorBarSeries.Area.ScheduleUpdate();
  }

  private static void OnVerticalCapPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ErrorBarSeries errorBarSeries) || errorBarSeries.VerticalCapLineStyle == null)
      return;
    errorBarSeries.VerticalCapLineStyle.Series = (ChartSeriesBase) errorBarSeries;
    foreach (ErrorBarSegment segment in (Collection<ChartSegment>) errorBarSeries.Segments)
      segment.UpdateVisualBinding();
    if (errorBarSeries.Area == null)
      return;
    errorBarSeries.Area.ScheduleUpdate();
  }

  private void ErrorBarSeries_ActualRangeChanged(object sender, ActualRangeChangedEventArgs e)
  {
    if (!(sender is DateTimeAxis dateTimeAxis) || dateTimeAxis.IntervalType != DateTimeIntervalType.Auto)
      return;
    double num1 = ((DateTime) e.ActualMinimum).ToOADate();
    double num2 = ((DateTime) e.ActualMaximum).ToOADate();
    for (int index = 0; index < this.Segments.Count; ++index)
    {
      ErrorBarSegment segment = this.Segments[index] as ErrorBarSegment;
      Point point = this.Type != ErrorBarType.Custom ? segment.DateTimeIntervalCalculation(this._horizontalErrorValue, dateTimeAxis.ActualIntervalType) : segment.DateTimeIntervalCalculation(this.HorizontalCustomValues.Count > 0 ? this.HorizontalCustomValues[index] : 0.0, dateTimeAxis.ActualIntervalType);
      if (num1 > point.X)
        num1 = point.X;
      if (num2 < point.Y)
        num2 = point.Y;
    }
    DateTime actualMinimum = (DateTime) e.ActualMinimum;
    if (num1 < actualMinimum.ToOADate())
      e.ActualMinimum = (object) Convert.ToDouble(num1).FromOADate();
    DateTime actualMaximum = (DateTime) e.ActualMaximum;
    if (num2 <= actualMaximum.ToOADate())
      return;
    e.ActualMaximum = (object) Convert.ToDouble(num2).FromOADate();
  }

  private double[] GetSdErrorValue(IList<double> values)
  {
    double num1 = values.Sum() / (double) values.Count;
    List<double> doubleList = new List<double>();
    List<double> source = new List<double>();
    for (int index = 0; index < values.Count; ++index)
    {
      doubleList.Add(values[index] - num1);
      source.Add(doubleList[index] * doubleList[index]);
    }
    double num2 = source.Sum<double>((Func<double, double>) (x => x));
    double[] sdErrorValue = new double[2];
    double num3 = Math.Sqrt(num2 / (double) (values.Count - 1));
    double num4 = num3 / Math.Sqrt((double) this.DataCount);
    sdErrorValue[0] = num1;
    sdErrorValue[1] = this.Type == ErrorBarType.StandardDeviation ? num3 : num4;
    return sdErrorValue;
  }

  private double GetPlusValue(double value, double errorvalue, bool axischeck)
  {
    if (!(this.ActualXAxis is DateTimeAxis) || !axischeck)
      return value + errorvalue;
    DateTimeAxis actualXaxis = this.ActualXAxis as DateTimeAxis;
    return DateTimeAxisHelper.IncreaseInterval(Convert.ToDouble(value).FromOADate(), errorvalue, actualXaxis.IntervalType).ToOADate();
  }

  private double GetMinusValue(double value, double errorvalue, bool axischeck)
  {
    if (!(this.ActualXAxis is DateTimeAxis) || !axischeck)
      return value - errorvalue;
    DateTimeAxis actualXaxis = this.ActualXAxis as DateTimeAxis;
    return DateTimeAxisHelper.IncreaseInterval(Convert.ToDouble(value).FromOADate(), -errorvalue, actualXaxis.IntervalType).ToOADate();
  }
}
