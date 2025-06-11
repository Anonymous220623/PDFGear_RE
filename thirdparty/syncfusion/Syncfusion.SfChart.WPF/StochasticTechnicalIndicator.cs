// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.StochasticTechnicalIndicator
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class StochasticTechnicalIndicator : FinancialTechnicalIndicator
{
  private IList<double> HighValues = (IList<double>) new List<double>();
  private IList<double> LowValues = (IList<double>) new List<double>();
  private IList<double> CloseValues = (IList<double>) new List<double>();
  private List<double> xValues;
  private List<double> upperXPoints = new List<double>();
  private List<double> upperYPoints = new List<double>();
  private List<double> lowerXPoints = new List<double>();
  private List<double> lowerYPoints = new List<double>();
  private List<double> periodXPoints = new List<double>();
  private List<double> periodYPoints = new List<double>();
  private List<double> signalXPoints = new List<double>();
  private List<double> signalYPoints = new List<double>();
  private List<double> percentK = new List<double>();
  private TechnicalIndicatorSegment upperLineSegment;
  private TechnicalIndicatorSegment lowerLineSegment;
  private TechnicalIndicatorSegment periodLineSegment;
  private TechnicalIndicatorSegment signalLineSegment;
  public static readonly DependencyProperty PeriodProperty = DependencyProperty.Register(nameof (Period), typeof (int), typeof (StochasticTechnicalIndicator), new PropertyMetadata((object) 14, new PropertyChangedCallback(StochasticTechnicalIndicator.OnPeriodChanged)));
  public static readonly DependencyProperty KPeriodProperty = DependencyProperty.Register(nameof (KPeriod), typeof (int), typeof (StochasticTechnicalIndicator), new PropertyMetadata((object) 5, new PropertyChangedCallback(StochasticTechnicalIndicator.OnPeriodChanged)));
  public static readonly DependencyProperty DPeriodProperty = DependencyProperty.Register(nameof (DPeriod), typeof (int), typeof (StochasticTechnicalIndicator), new PropertyMetadata((object) 3, new PropertyChangedCallback(StochasticTechnicalIndicator.OnPeriodChanged)));
  public static readonly DependencyProperty PeriodLineColorProperty = DependencyProperty.Register(nameof (PeriodLineColor), typeof (Brush), typeof (StochasticTechnicalIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Green), new PropertyChangedCallback(StochasticTechnicalIndicator.OnColorChanged)));
  public static readonly DependencyProperty UpperLineColorProperty = DependencyProperty.Register(nameof (UpperLineColor), typeof (Brush), typeof (StochasticTechnicalIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Red), new PropertyChangedCallback(StochasticTechnicalIndicator.OnColorChanged)));
  public static readonly DependencyProperty LowerLineColorProperty = DependencyProperty.Register(nameof (LowerLineColor), typeof (Brush), typeof (StochasticTechnicalIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Red), new PropertyChangedCallback(StochasticTechnicalIndicator.OnColorChanged)));
  public static readonly DependencyProperty SignalLineColorProperty = DependencyProperty.Register(nameof (SignalLineColor), typeof (Brush), typeof (StochasticTechnicalIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Blue), new PropertyChangedCallback(StochasticTechnicalIndicator.OnColorChanged)));

  internal override bool IsMultipleYPathRequired => true;

  public int Period
  {
    get => (int) this.GetValue(StochasticTechnicalIndicator.PeriodProperty);
    set => this.SetValue(StochasticTechnicalIndicator.PeriodProperty, (object) value);
  }

  public int KPeriod
  {
    get => (int) this.GetValue(StochasticTechnicalIndicator.KPeriodProperty);
    set => this.SetValue(StochasticTechnicalIndicator.KPeriodProperty, (object) value);
  }

  public int DPeriod
  {
    get => (int) this.GetValue(StochasticTechnicalIndicator.DPeriodProperty);
    set => this.SetValue(StochasticTechnicalIndicator.DPeriodProperty, (object) value);
  }

  public Brush PeriodLineColor
  {
    get => (Brush) this.GetValue(StochasticTechnicalIndicator.PeriodLineColorProperty);
    set => this.SetValue(StochasticTechnicalIndicator.PeriodLineColorProperty, (object) value);
  }

  public Brush UpperLineColor
  {
    get => (Brush) this.GetValue(StochasticTechnicalIndicator.UpperLineColorProperty);
    set => this.SetValue(StochasticTechnicalIndicator.UpperLineColorProperty, (object) value);
  }

  public Brush LowerLineColor
  {
    get => (Brush) this.GetValue(StochasticTechnicalIndicator.LowerLineColorProperty);
    set => this.SetValue(StochasticTechnicalIndicator.LowerLineColorProperty, (object) value);
  }

  public Brush SignalLineColor
  {
    get => (Brush) this.GetValue(StochasticTechnicalIndicator.SignalLineColorProperty);
    set => this.SetValue(StochasticTechnicalIndicator.SignalLineColorProperty, (object) value);
  }

  internal override void SetIndicatorInfo(
    ChartPointInfo info,
    List<double> yValue,
    bool seriesPalette)
  {
    if (yValue.Count <= 0)
      return;
    info.UpperLine = double.IsNaN(yValue[2]) ? "null" : Math.Round(yValue[2], 2).ToString();
    info.SignalLine = double.IsNaN(yValue[3]) ? "null" : Math.Round(yValue[3], 2).ToString();
  }

  private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as StochasticTechnicalIndicator).UpdateArea();
  }

  private static void OnPeriodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is StochasticTechnicalIndicator technicalIndicator))
      return;
    technicalIndicator.UpdateArea();
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnDataSourceChanged(oldValue, newValue);
    this.upperLineSegment = (TechnicalIndicatorSegment) null;
    this.lowerLineSegment = (TechnicalIndicatorSegment) null;
    this.signalLineSegment = (TechnicalIndicatorSegment) null;
    this.periodLineSegment = (TechnicalIndicatorSegment) null;
    this.HighValues.Clear();
    this.LowValues.Clear();
    this.CloseValues.Clear();
    this.GeneratePoints(new string[3]
    {
      this.High,
      this.Low,
      this.Close
    }, this.HighValues, this.LowValues, this.CloseValues);
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.HighValues.Clear();
    this.LowValues.Clear();
    this.CloseValues.Clear();
    base.OnBindingPathChanged(args);
  }

  protected internal override void SetSeriesItemSource(ChartSeriesBase series)
  {
    if (series.ActualSeriesYValues.Length <= 2)
      return;
    this.ActualXValues = this.Clone(series.ActualXValues);
    this.HighValues = (IList<double>) ChartSeriesBase.Clone(series.ActualSeriesYValues[0]);
    this.LowValues = (IList<double>) ChartSeriesBase.Clone(series.ActualSeriesYValues[1]);
    this.CloseValues = (IList<double>) ChartSeriesBase.Clone(series.ActualSeriesYValues[2]);
    this.Area.ScheduleUpdate();
  }

  protected internal override void GeneratePoints()
  {
    this.GeneratePoints(new string[3]
    {
      this.High,
      this.Low,
      this.Close
    }, this.HighValues, this.LowValues, this.CloseValues);
  }

  public override void CreateSegments()
  {
    this.xValues = this.GetXValues();
    if (this.Period < 1 || this.DPeriod < 1 || this.KPeriod < 1)
      return;
    this.Period = this.Period < this.xValues.Count ? this.Period : this.xValues.Count - 1;
    this.BasePoints(this.Period, this.percentK);
    this.AddPoints(this.Period, this.KPeriod);
    this.AddSignalPoints(this.DPeriod);
    if (this.upperLineSegment == null || this.lowerLineSegment == null || this.periodLineSegment == null)
    {
      this.Segments.Clear();
      this.upperLineSegment = new TechnicalIndicatorSegment(this.upperXPoints, this.upperYPoints, this.UpperLineColor, (ChartSeriesBase) this);
      this.upperLineSegment.SetValues(this.upperXPoints, this.upperYPoints, this.UpperLineColor, (ChartSeriesBase) this);
      this.Segments.Add((ChartSegment) this.upperLineSegment);
      this.lowerLineSegment = new TechnicalIndicatorSegment(this.lowerXPoints, this.lowerYPoints, this.LowerLineColor, (ChartSeriesBase) this);
      this.lowerLineSegment.SetValues(this.lowerXPoints, this.lowerYPoints, this.LowerLineColor, (ChartSeriesBase) this);
      this.Segments.Add((ChartSegment) this.lowerLineSegment);
      this.periodLineSegment = new TechnicalIndicatorSegment(this.periodXPoints, this.periodYPoints, this.PeriodLineColor, (ChartSeriesBase) this, this.Period + this.KPeriod - 1);
      this.periodLineSegment.SetValues(this.periodXPoints, this.periodYPoints, this.PeriodLineColor, (ChartSeriesBase) this, this.Period + this.KPeriod - 1);
      this.Segments.Add((ChartSegment) this.periodLineSegment);
      this.signalLineSegment = new TechnicalIndicatorSegment(this.signalXPoints, this.signalYPoints, this.SignalLineColor, (ChartSeriesBase) this, this.Period + this.KPeriod + this.DPeriod - 2);
      this.signalLineSegment.SetValues(this.signalXPoints, this.signalYPoints, this.SignalLineColor, (ChartSeriesBase) this, this.Period + this.KPeriod + this.DPeriod - 2);
      this.Segments.Add((ChartSegment) this.signalLineSegment);
    }
    else
    {
      this.upperLineSegment.SetData((IList<double>) this.upperXPoints, (IList<double>) this.upperYPoints, this.UpperLineColor);
      this.upperLineSegment.SetRange();
      this.lowerLineSegment.SetData((IList<double>) this.lowerXPoints, (IList<double>) this.lowerYPoints, this.LowerLineColor);
      this.lowerLineSegment.SetRange();
      this.periodLineSegment.SetData((IList<double>) this.periodXPoints, (IList<double>) this.periodYPoints, this.PeriodLineColor, this.Period + this.KPeriod - 1);
      this.periodLineSegment.SetRange();
      this.signalLineSegment.SetData((IList<double>) this.signalXPoints, (IList<double>) this.signalYPoints, this.SignalLineColor, this.Period + this.KPeriod + this.DPeriod - 2);
      this.signalLineSegment.SetRange();
    }
  }

  public override void UpdateSegments(int index, NotifyCollectionChangedAction action)
  {
    this.Area.ScheduleUpdate();
  }

  private void BasePoints(int period, List<double> ypoints)
  {
    ypoints.Clear();
    List<double> doubleList1 = new List<double>();
    List<double> doubleList2 = new List<double>();
    for (int index = 0; index < period - 1; ++index)
    {
      doubleList2.Add(0.0);
      ypoints.Add(0.0);
      doubleList1.Add(0.0);
    }
    for (int index1 = period - 1; index1 < this.DataCount; ++index1)
    {
      double val1_1 = double.MaxValue;
      double val1_2 = double.MinValue;
      for (int index2 = 0; index2 < period; ++index2)
      {
        val1_1 = Math.Min(val1_1, this.LowValues[index1 - index2]);
        val1_2 = Math.Max(val1_2, this.HighValues[index1 - index2]);
      }
      doubleList2.Add(val1_2);
      doubleList1.Add(val1_1);
    }
    for (int index = period - 1; index < this.DataCount; ++index)
    {
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = num1 + (this.CloseValues[index] - doubleList1[index]);
      double num4 = num2 + (doubleList2[index] - doubleList1[index]);
      ypoints.Add(num3 / num4 * 100.0);
    }
  }

  private void AddPoints(int period, int k_period)
  {
    if (this.DataCount <= period + k_period)
      return;
    this.AddPeriodPoints(k_period);
    List<double> list = this.xValues.Select<double, double>((Func<double, double>) (val => val)).ToList<double>();
    this.upperXPoints.Clear();
    this.lowerXPoints.Clear();
    this.upperXPoints.AddRange((IEnumerable<double>) list);
    this.lowerXPoints.AddRange((IEnumerable<double>) list);
    this.periodXPoints = list;
    this.upperYPoints.Clear();
    this.lowerYPoints.Clear();
    for (int index = 0; index < this.DataCount; ++index)
    {
      this.upperYPoints.Add(80.0);
      this.lowerYPoints.Add(20.0);
    }
  }

  private void AddSignalPoints(int d_period)
  {
    if (this.periodYPoints.Count < this.Period + this.KPeriod + d_period)
      return;
    this.signalXPoints = this.xValues;
    this.signalYPoints.Clear();
    for (int index = 0; index < d_period - 1; ++index)
      this.signalYPoints.Add(0.0);
    foreach (double num in Enumerable.Range(0, this.periodYPoints.Count - (d_period - 1)).Select<int, double>((Func<int, double>) (n => this.periodYPoints.Skip<double>(n).Take<double>(d_period).Average())).ToList<double>())
      this.signalYPoints.Add(num);
  }

  private void AddPeriodPoints(int k_period)
  {
    this.periodYPoints.Clear();
    for (int index = 0; index < k_period - 1; ++index)
      this.periodYPoints.Add(0.0);
    foreach (double num in Enumerable.Range(0, this.percentK.Count - (k_period - 1)).Select<int, double>((Func<int, double>) (n => this.percentK.Skip<double>(n).Take<double>(k_period).Average())).ToList<double>())
      this.periodYPoints.Add(num);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    obj = (DependencyObject) new StochasticTechnicalIndicator()
    {
      SignalLineColor = this.SignalLineColor,
      LowerLineColor = this.LowerLineColor,
      UpperLineColor = this.UpperLineColor,
      Period = this.Period,
      KPeriod = this.KPeriod,
      DPeriod = this.DPeriod
    };
    return base.CloneSeries(obj);
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) null;
}
