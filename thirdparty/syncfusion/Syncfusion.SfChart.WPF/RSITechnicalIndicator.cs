// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.RSITechnicalIndicator
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

public class RSITechnicalIndicator : FinancialTechnicalIndicator
{
  private IList<double> CloseValues = (IList<double>) new List<double>();
  private List<double> xValues;
  private List<double> xPoints = new List<double>();
  private List<double> yPoints = new List<double>();
  private List<double> upperXPoints = new List<double>();
  private List<double> upperYPoints = new List<double>();
  private List<double> lowerXPoints = new List<double>();
  private List<double> lowerYPoints = new List<double>();
  private TechnicalIndicatorSegment upperLineSegment;
  private TechnicalIndicatorSegment lowerLineSegment;
  private TechnicalIndicatorSegment signalLineSegment;
  public static readonly DependencyProperty PeriodProperty = DependencyProperty.Register(nameof (Period), typeof (int), typeof (RSITechnicalIndicator), new PropertyMetadata((object) 14, new PropertyChangedCallback(RSITechnicalIndicator.OnMovingAverageChanged)));
  public static readonly DependencyProperty UpperLineColorProperty = DependencyProperty.Register(nameof (UpperLineColor), typeof (Brush), typeof (RSITechnicalIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Red), new PropertyChangedCallback(RSITechnicalIndicator.OnColorChanged)));
  public static readonly DependencyProperty LowerLineColorProperty = DependencyProperty.Register(nameof (LowerLineColor), typeof (Brush), typeof (RSITechnicalIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Red), new PropertyChangedCallback(RSITechnicalIndicator.OnColorChanged)));
  public static readonly DependencyProperty SignalLineColorProperty = DependencyProperty.Register(nameof (SignalLineColor), typeof (Brush), typeof (RSITechnicalIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Blue), new PropertyChangedCallback(RSITechnicalIndicator.OnColorChanged)));

  public int Period
  {
    get => (int) this.GetValue(RSITechnicalIndicator.PeriodProperty);
    set => this.SetValue(RSITechnicalIndicator.PeriodProperty, (object) value);
  }

  public Brush UpperLineColor
  {
    get => (Brush) this.GetValue(RSITechnicalIndicator.UpperLineColorProperty);
    set => this.SetValue(RSITechnicalIndicator.UpperLineColorProperty, (object) value);
  }

  public Brush LowerLineColor
  {
    get => (Brush) this.GetValue(RSITechnicalIndicator.LowerLineColorProperty);
    set => this.SetValue(RSITechnicalIndicator.LowerLineColorProperty, (object) value);
  }

  public Brush SignalLineColor
  {
    get => (Brush) this.GetValue(RSITechnicalIndicator.SignalLineColorProperty);
    set => this.SetValue(RSITechnicalIndicator.SignalLineColorProperty, (object) value);
  }

  internal override void SetIndicatorInfo(
    ChartPointInfo info,
    List<double> yValue,
    bool seriesPalette)
  {
    if (yValue.Count <= 0)
      return;
    info.SignalLine = double.IsNaN(yValue[2]) ? "null" : Math.Round(yValue[2], 2).ToString();
  }

  private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is RSITechnicalIndicator technicalIndicator))
      return;
    technicalIndicator.UpdateArea();
  }

  private static void OnMovingAverageChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is RSITechnicalIndicator technicalIndicator))
      return;
    technicalIndicator.UpdateArea();
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnDataSourceChanged(oldValue, newValue);
    this.upperLineSegment = (TechnicalIndicatorSegment) null;
    this.lowerLineSegment = (TechnicalIndicatorSegment) null;
    this.signalLineSegment = (TechnicalIndicatorSegment) null;
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
    if (this.DataCount <= this.Period || this.Period < 1)
    {
      this.Segments.Clear();
    }
    else
    {
      this.xValues = this.GetXValues();
      this.ComputeRSI(this.Period);
      List<double> list = this.xValues.Select<double, double>((Func<double, double>) (val => val)).ToList<double>();
      this.upperXPoints.Clear();
      this.lowerXPoints.Clear();
      this.upperXPoints.AddRange((IEnumerable<double>) list);
      this.lowerXPoints.AddRange((IEnumerable<double>) list);
      this.upperYPoints.Clear();
      this.lowerYPoints.Clear();
      for (int index = 0; index < this.DataCount; ++index)
      {
        this.upperYPoints.Add(70.0);
        this.lowerYPoints.Add(30.0);
      }
      if (this.upperLineSegment == null || this.lowerLineSegment == null || this.signalLineSegment == null || this.Segments.Count == 0)
      {
        this.Segments.Clear();
        this.upperLineSegment = new TechnicalIndicatorSegment(this.upperXPoints, this.upperYPoints, this.UpperLineColor, (ChartSeriesBase) this);
        this.upperLineSegment.SetValues(this.upperXPoints, this.upperYPoints, this.UpperLineColor, (ChartSeriesBase) this);
        this.Segments.Add((ChartSegment) this.upperLineSegment);
        this.lowerLineSegment = new TechnicalIndicatorSegment(this.lowerXPoints, this.lowerYPoints, this.LowerLineColor, (ChartSeriesBase) this);
        this.lowerLineSegment.SetValues(this.lowerXPoints, this.lowerYPoints, this.LowerLineColor, (ChartSeriesBase) this);
        this.Segments.Add((ChartSegment) this.lowerLineSegment);
        this.signalLineSegment = new TechnicalIndicatorSegment(this.xPoints, this.yPoints, this.SignalLineColor, (ChartSeriesBase) this, this.Period + 1);
        this.signalLineSegment.SetValues(this.xPoints, this.yPoints, this.SignalLineColor, (ChartSeriesBase) this, this.Period + 1);
        this.Segments.Add((ChartSegment) this.signalLineSegment);
      }
      else
      {
        this.upperLineSegment.SetData((IList<double>) this.upperXPoints, (IList<double>) this.upperYPoints, this.UpperLineColor);
        this.upperLineSegment.SetRange();
        this.lowerLineSegment.SetData((IList<double>) this.lowerXPoints, (IList<double>) this.lowerYPoints, this.LowerLineColor);
        this.lowerLineSegment.SetRange();
        this.signalLineSegment.SetData((IList<double>) this.xPoints, (IList<double>) this.yPoints, this.SignalLineColor, this.Period + 1);
        this.signalLineSegment.SetRange();
      }
    }
  }

  public override void UpdateSegments(int index, NotifyCollectionChangedAction action)
  {
    this.Area.ScheduleUpdate();
  }

  private void ComputeRSI(int period)
  {
    this.xPoints.Clear();
    this.yPoints.Clear();
    double num1 = 0.0;
    double num2 = 0.0;
    double num3 = this.CloseValues[0];
    for (int index = 1; index <= period; ++index)
    {
      double closeValue = this.CloseValues[index];
      if (closeValue > num3)
        num1 += closeValue - num3;
      else if (closeValue < num3)
        num2 += num3 - closeValue;
      num3 = closeValue;
      this.yPoints.Add(0.0);
    }
    double num4 = num1 / (double) period;
    double num5 = num2 / (double) period;
    this.yPoints.Add(100.0 - 100.0 / (1.0 + num4 / num5));
    for (int index = period + 1; index < this.DataCount; ++index)
    {
      double closeValue = this.CloseValues[index];
      if (closeValue > num3)
      {
        num4 = (num4 * (double) (period - 1) + (closeValue - num3)) / (double) period;
        num5 = num5 * (double) (period - 1) / (double) period;
      }
      else if (closeValue < num3)
      {
        num5 = (num5 * (double) (period - 1) + (num3 - closeValue)) / (double) period;
        num4 = num4 * (double) (period - 1) / (double) period;
      }
      num3 = closeValue;
      this.yPoints.Add(100.0 - 100.0 / (1.0 + num4 / num5));
    }
    this.xPoints = this.xValues.Select<double, double>((Func<double, double>) (val => val)).ToList<double>();
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    obj = (DependencyObject) new RSITechnicalIndicator()
    {
      LowerLineColor = this.LowerLineColor,
      UpperLineColor = this.UpperLineColor,
      SignalLineColor = this.SignalLineColor,
      Period = this.Period
    };
    return base.CloneSeries(obj);
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) null;
}
