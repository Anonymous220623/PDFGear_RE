// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.MACDTechnicalIndicator
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class MACDTechnicalIndicator : FinancialTechnicalIndicator
{
  private IList<double> CloseValues = (IList<double>) new List<double>();
  private List<double> xValues;
  private List<double> FastEMA = new List<double>();
  private List<double> SlowEMA = new List<double>();
  private List<double> MACDPoints = new List<double>();
  private List<double> SignalPoints = new List<double>();
  private List<double> xPoints = new List<double>();
  private List<double> HistogramYPoints = new List<double>();
  private List<double> CenterYPoints = new List<double>();
  private IList<double> x1Values;
  private IList<double> x2Values;
  private IList<double> y1Values;
  private IList<double> y2Values;
  private TechnicalIndicatorSegment MACDLineSegment;
  private TechnicalIndicatorSegment SignalLineSegment;
  private TechnicalIndicatorSegment CenterlLineSegment;
  private FastColumnBitmapSegment HistogramSegment;
  public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof (Type), typeof (MACDType), typeof (MACDTechnicalIndicator), new PropertyMetadata((object) MACDType.Line, new PropertyChangedCallback(MACDTechnicalIndicator.OnValueChanged)));
  public static readonly DependencyProperty ShortPeriodProperty = DependencyProperty.Register(nameof (ShortPeriod), typeof (int), typeof (MACDTechnicalIndicator), new PropertyMetadata((object) 12, new PropertyChangedCallback(MACDTechnicalIndicator.OnValueChanged)));
  public static readonly DependencyProperty LongPeriodProperty = DependencyProperty.Register(nameof (LongPeriod), typeof (int), typeof (MACDTechnicalIndicator), new PropertyMetadata((object) 26, new PropertyChangedCallback(MACDTechnicalIndicator.OnValueChanged)));
  public static readonly DependencyProperty PeriodProperty = DependencyProperty.Register(nameof (Period), typeof (int), typeof (MACDTechnicalIndicator), new PropertyMetadata((object) 9, new PropertyChangedCallback(MACDTechnicalIndicator.OnValueChanged)));
  public static readonly DependencyProperty ConvergenceLineColorProperty = DependencyProperty.Register(nameof (ConvergenceLineColor), typeof (Brush), typeof (MACDTechnicalIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Red), new PropertyChangedCallback(MACDTechnicalIndicator.OnColorChanged)));
  public static readonly DependencyProperty DivergenceLineColorProperty = DependencyProperty.Register(nameof (DivergenceLineColor), typeof (Brush), typeof (MACDTechnicalIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Red), new PropertyChangedCallback(MACDTechnicalIndicator.OnColorChanged)));
  public static readonly DependencyProperty HistogramColorProperty = DependencyProperty.Register(nameof (HistogramColor), typeof (Brush), typeof (MACDTechnicalIndicator), new PropertyMetadata((object) null, new PropertyChangedCallback(MACDTechnicalIndicator.OnColorChanged)));
  public static readonly DependencyProperty SignalLineColorProperty = DependencyProperty.Register(nameof (SignalLineColor), typeof (Brush), typeof (MACDTechnicalIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Blue), new PropertyChangedCallback(MACDTechnicalIndicator.OnColorChanged)));

  protected internal override bool IsSideBySide
  {
    get => this.Type == MACDType.Histogram || this.Type == MACDType.Both;
  }

  public MACDType Type
  {
    get => (MACDType) this.GetValue(MACDTechnicalIndicator.TypeProperty);
    set => this.SetValue(MACDTechnicalIndicator.TypeProperty, (object) value);
  }

  public int ShortPeriod
  {
    get => (int) this.GetValue(MACDTechnicalIndicator.ShortPeriodProperty);
    set => this.SetValue(MACDTechnicalIndicator.ShortPeriodProperty, (object) value);
  }

  public int LongPeriod
  {
    get => (int) this.GetValue(MACDTechnicalIndicator.LongPeriodProperty);
    set => this.SetValue(MACDTechnicalIndicator.LongPeriodProperty, (object) value);
  }

  public int Period
  {
    get => (int) this.GetValue(MACDTechnicalIndicator.PeriodProperty);
    set => this.SetValue(MACDTechnicalIndicator.PeriodProperty, (object) value);
  }

  public Brush ConvergenceLineColor
  {
    get => (Brush) this.GetValue(MACDTechnicalIndicator.ConvergenceLineColorProperty);
    set => this.SetValue(MACDTechnicalIndicator.ConvergenceLineColorProperty, (object) value);
  }

  public Brush DivergenceLineColor
  {
    get => (Brush) this.GetValue(MACDTechnicalIndicator.DivergenceLineColorProperty);
    set => this.SetValue(MACDTechnicalIndicator.DivergenceLineColorProperty, (object) value);
  }

  public Brush HistogramColor
  {
    get => (Brush) this.GetValue(MACDTechnicalIndicator.HistogramColorProperty);
    set => this.SetValue(MACDTechnicalIndicator.HistogramColorProperty, (object) value);
  }

  public Brush SignalLineColor
  {
    get => (Brush) this.GetValue(MACDTechnicalIndicator.SignalLineColorProperty);
    set => this.SetValue(MACDTechnicalIndicator.SignalLineColorProperty, (object) value);
  }

  internal override void SetIndicatorInfo(
    ChartPointInfo info,
    List<double> yValue,
    bool seriesPalette)
  {
    if (this.Type == MACDType.Both && yValue.Count > 0)
    {
      info.UpperLine = double.IsNaN(yValue[0]) ? "null" : Math.Round(yValue[0], 2).ToString();
      info.LowerLine = double.IsNaN(yValue[1]) ? "null" : Math.Round(yValue[1], 2).ToString();
      info.SignalLine = double.IsNaN(yValue[2]) ? "null" : Math.Round(yValue[2], 2).ToString();
    }
    if (this.Type == MACDType.Line && yValue.Count > 0)
    {
      info.LowerLine = double.IsNaN(yValue[0]) ? "null" : Math.Round(yValue[0], 2).ToString();
      info.SignalLine = double.IsNaN(yValue[1]) ? "null" : Math.Round(yValue[1], 2).ToString();
    }
    if (this.Type != MACDType.Histogram || yValue.Count <= 0)
      return;
    info.UpperLine = double.IsNaN(yValue[0]) ? "null" : Math.Round(yValue[0], 2).ToString();
  }

  private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as MACDTechnicalIndicator).UpdateArea();
  }

  private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as MACDTechnicalIndicator).UpdateArea();
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnDataSourceChanged(oldValue, newValue);
    this.MACDLineSegment = (TechnicalIndicatorSegment) null;
    this.CenterlLineSegment = (TechnicalIndicatorSegment) null;
    this.HistogramSegment = (FastColumnBitmapSegment) null;
    this.CloseValues.Clear();
    this.GeneratePoints(new string[1]{ this.Close }, this.CloseValues);
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.CloseValues.Clear();
    base.OnBindingPathChanged(args);
  }

  protected internal override void SetSeriesItemSource(ChartSeriesBase series)
  {
    if (series.ActualSeriesYValues.Length <= 0)
      return;
    this.ActualXValues = this.Clone(series.ActualXValues);
    this.CloseValues = (IList<double>) ChartSeriesBase.Clone(series.ActualSeriesYValues[0]);
    this.Area.ScheduleUpdate();
  }

  protected internal override void GeneratePoints()
  {
    this.GeneratePoints(new string[1]{ this.Close }, this.CloseValues);
  }

  public override void CreateSegments()
  {
    if (this.Period + this.LongPeriod >= this.DataCount || this.Period < 1 || this.ShortPeriod >= this.DataCount)
    {
      this.Segments.Clear();
    }
    else
    {
      this.xValues = this.GetXValues();
      if (this.Period < 1)
        return;
      this.AddMACDPoints();
      if (!this.Segments.Contains((ChartSegment) this.MACDLineSegment) || !this.Segments.Contains((ChartSegment) this.SignalLineSegment) || !this.Segments.Contains((ChartSegment) this.CenterlLineSegment) || !this.Segments.Contains((ChartSegment) this.HistogramSegment) || this.Segments.Count == 0)
      {
        this.Segments.Clear();
        if (this.Type == MACDType.Histogram || this.Type == MACDType.Both)
        {
          this.CalculateHistogram();
          this.HistogramSegment = new FastColumnBitmapSegment(this.x1Values, this.y1Values, this.x2Values, this.y2Values, (ChartSeries) this);
          this.HistogramSegment.SetData(this.x1Values, this.y1Values, this.x2Values, this.y2Values);
          this.Interior = this.HistogramColor;
          this.Segments.Add((ChartSegment) this.HistogramSegment);
        }
        if (this.Type != MACDType.Line && this.Type != MACDType.Both)
          return;
        this.MACDLineSegment = new TechnicalIndicatorSegment(this.xPoints, this.MACDPoints, this.ConvergenceLineColor, (ChartSeriesBase) this, this.LongPeriod);
        this.MACDLineSegment.SetValues(this.xPoints, this.MACDPoints, this.ConvergenceLineColor, (ChartSeriesBase) this, this.LongPeriod);
        this.Segments.Add((ChartSegment) this.MACDLineSegment);
        this.SignalLineSegment = new TechnicalIndicatorSegment(this.xPoints, this.SignalPoints, this.DivergenceLineColor, (ChartSeriesBase) this, this.LongPeriod + this.Period - 1);
        this.SignalLineSegment.SetValues(this.xPoints, this.SignalPoints, this.DivergenceLineColor, (ChartSeriesBase) this, this.LongPeriod + this.Period - 1);
        this.Segments.Add((ChartSegment) this.SignalLineSegment);
        this.CenterlLineSegment = new TechnicalIndicatorSegment(this.xPoints, this.CenterYPoints, this.SignalLineColor, (ChartSeriesBase) this);
        this.CenterlLineSegment.SetValues(this.xPoints, this.CenterYPoints, this.SignalLineColor, (ChartSeriesBase) this);
        this.Segments.Add((ChartSegment) this.CenterlLineSegment);
      }
      else if (this.Type == MACDType.Both)
      {
        this.HistogramSegment.SetData(this.x1Values, this.y1Values, this.x2Values, this.y2Values);
        this.MACDLineSegment.SetData((IList<double>) this.xPoints, (IList<double>) this.MACDPoints, this.ConvergenceLineColor);
        this.MACDLineSegment.SetRange();
        this.SignalLineSegment.SetData((IList<double>) this.xPoints, (IList<double>) this.SignalPoints, this.DivergenceLineColor);
        this.SignalLineSegment.SetRange();
        this.CenterlLineSegment.SetData((IList<double>) this.xPoints, (IList<double>) this.CenterYPoints, this.SignalLineColor);
        this.CenterlLineSegment.SetRange();
      }
      else if (this.Type == MACDType.Line)
      {
        this.Segments.Remove((ChartSegment) this.HistogramSegment);
        this.HistogramSegment = (FastColumnBitmapSegment) null;
        this.MACDLineSegment.SetData((IList<double>) this.xPoints, (IList<double>) this.MACDPoints, this.ConvergenceLineColor);
        this.MACDLineSegment.SetRange();
        this.SignalLineSegment.SetData((IList<double>) this.xPoints, (IList<double>) this.SignalPoints, this.DivergenceLineColor);
        this.SignalLineSegment.SetRange();
        this.CenterlLineSegment.SetData((IList<double>) this.xPoints, (IList<double>) this.CenterYPoints, this.SignalLineColor);
        this.CenterlLineSegment.SetRange();
      }
      else
      {
        if (this.Type != MACDType.Histogram)
          return;
        this.Segments.Remove((ChartSegment) this.MACDLineSegment);
        this.Segments.Remove((ChartSegment) this.SignalLineSegment);
        this.Segments.Remove((ChartSegment) this.CenterlLineSegment);
        this.MACDLineSegment = (TechnicalIndicatorSegment) null;
        this.SignalLineSegment = (TechnicalIndicatorSegment) null;
        this.CenterlLineSegment = (TechnicalIndicatorSegment) null;
        this.HistogramSegment.SetData(this.x1Values, this.y1Values, this.x2Values, this.y2Values);
      }
    }
  }

  public override void UpdateSegments(int index, NotifyCollectionChangedAction action)
  {
    this.Area.ScheduleUpdate();
  }

  public void AddMACDPoints()
  {
    this.MACDPoints.Clear();
    this.SignalPoints.Clear();
    this.CenterYPoints.Clear();
    this.xPoints.Clear();
    this.FastEMA = this.CalculateEMA(this.ShortPeriod);
    this.SlowEMA = this.CalculateEMA(this.LongPeriod);
    for (int index = 0; index < this.DataCount; ++index)
    {
      this.xPoints.Add(this.xValues[index]);
      this.CenterYPoints.Add(0.0);
      this.MACDPoints.Add(double.IsNaN(this.FastEMA[index] - this.SlowEMA[index]) ? 0.0 : this.FastEMA[index] - this.SlowEMA[index]);
    }
    double num1 = 2.0 / ((double) this.Period + 1.0);
    double num2 = 0.0;
    double num3 = 0.0;
    int index1 = this.LongPeriod - 1;
    do
    {
      num2 += this.MACDPoints[index1];
      ++index1;
      ++num3;
    }
    while ((double) this.Period != num3);
    double num4 = num2 / (double) this.Period;
    for (int index2 = 0; index2 < this.LongPeriod + this.Period - 2; ++index2)
      this.SignalPoints.Add(0.0);
    this.SignalPoints.Add(num4);
    for (int index3 = this.LongPeriod + this.Period - 1; index3 < this.DataCount; ++index3)
      this.SignalPoints.Add(this.MACDPoints[index3] * num1 + this.SignalPoints[index3 - 1] * (1.0 - num1));
  }

  private List<double> CalculateEMA(int length)
  {
    double num1 = 2.0 / ((double) length + 1.0);
    double num2 = 0.0;
    List<double> ema = new List<double>();
    for (int index = 0; index < length; ++index)
    {
      ema.Add(double.NaN);
      num2 += this.CloseValues[index];
    }
    double num3 = num2 / (double) length;
    ema[length - 1] = num3;
    for (int index = length; index < this.DataCount; ++index)
      ema.Add(this.CloseValues[index] * num1 + ema[index - 1] * (1.0 - num1));
    return ema;
  }

  private void CalculateHistogram()
  {
    this.x1Values = (IList<double>) new List<double>();
    this.x2Values = (IList<double>) new List<double>();
    this.y1Values = (IList<double>) new List<double>();
    this.y2Values = (IList<double>) new List<double>();
    this.Area.SBSInfoCalculated = false;
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    for (int index = 0; index < this.DataCount; ++index)
    {
      this.HistogramYPoints.Add(this.MACDPoints[index] - this.SignalPoints[index]);
      if (!this.IsIndexed)
      {
        this.x1Values.Add(this.xValues[index] + sideBySideInfo.Start);
        this.x2Values.Add(this.xValues[index] + sideBySideInfo.End);
        this.y1Values.Add(this.HistogramYPoints[index]);
        this.y2Values.Add(0.0);
      }
      else
      {
        this.x1Values.Add((double) index + sideBySideInfo.Start);
        this.x2Values.Add((double) index + sideBySideInfo.End);
        this.y1Values.Add(this.HistogramYPoints[index]);
        this.y2Values.Add(0.0);
      }
    }
  }

  private static void OnAverageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as MACDTechnicalIndicator).Invalidate();
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    obj = (DependencyObject) new MACDTechnicalIndicator()
    {
      Period = this.Period,
      Type = this.Type,
      ShortPeriod = this.ShortPeriod,
      LongPeriod = this.LongPeriod,
      SignalLineColor = this.SignalLineColor,
      ConvergenceLineColor = this.ConvergenceLineColor,
      DivergenceLineColor = this.DivergenceLineColor
    };
    return base.CloneSeries(obj);
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) null;
}
