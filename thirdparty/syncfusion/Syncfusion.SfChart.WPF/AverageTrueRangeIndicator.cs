// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.AverageTrueRangeIndicator
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

public class AverageTrueRangeIndicator : FinancialTechnicalIndicator
{
  private IList<double> _closeValues = (IList<double>) new List<double>();
  private IList<double> _highValues = (IList<double>) new List<double>();
  private IList<double> _lowValues = (IList<double>) new List<double>();
  private IList<double> range = (IList<double>) new List<double>();
  private IList<double> trueRange = (IList<double>) new List<double>();
  private IList<double> HCp = (IList<double>) new List<double>();
  private IList<double> LCp = (IList<double>) new List<double>();
  private List<double> _xValues;
  private List<double> xPoints = new List<double>();
  private List<double> yPoints = new List<double>();
  private TechnicalIndicatorSegment _fastLineSegment;
  public static readonly DependencyProperty PeriodProperty = DependencyProperty.Register(nameof (Period), typeof (int), typeof (AverageTrueRangeIndicator), new PropertyMetadata((object) 14, new PropertyChangedCallback(AverageTrueRangeIndicator.OnMovingAverageChanged)));
  public static readonly DependencyProperty SignalLineColorProperty = DependencyProperty.Register(nameof (SignalLineColor), typeof (Brush), typeof (AverageTrueRangeIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Green), new PropertyChangedCallback(AverageTrueRangeIndicator.OnColorChanged)));

  internal override bool IsMultipleYPathRequired => true;

  public int Period
  {
    get => (int) this.GetValue(AverageTrueRangeIndicator.PeriodProperty);
    set => this.SetValue(AverageTrueRangeIndicator.PeriodProperty, (object) value);
  }

  public Brush SignalLineColor
  {
    get => (Brush) this.GetValue(AverageTrueRangeIndicator.SignalLineColorProperty);
    set => this.SetValue(AverageTrueRangeIndicator.SignalLineColorProperty, (object) value);
  }

  private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is AverageTrueRangeIndicator trueRangeIndicator))
      return;
    trueRangeIndicator.UpdateArea();
  }

  private static void OnMovingAverageChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is AverageTrueRangeIndicator trueRangeIndicator))
      return;
    trueRangeIndicator.UpdateArea();
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnDataSourceChanged(oldValue, newValue);
    this._fastLineSegment = (TechnicalIndicatorSegment) null;
    this._highValues.Clear();
    this._lowValues.Clear();
    this._closeValues.Clear();
    this.GeneratePoints(new string[3]
    {
      this.High,
      this.Low,
      this.Close
    }, this._highValues, this._lowValues, this._closeValues);
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this._fastLineSegment = (TechnicalIndicatorSegment) null;
    this._highValues.Clear();
    this._lowValues.Clear();
    this._closeValues.Clear();
    base.OnBindingPathChanged(args);
  }

  protected internal override void SetSeriesItemSource(ChartSeriesBase series)
  {
    if (series.ActualSeriesYValues.Length <= 3)
      return;
    this.ActualXValues = this.Clone(series.ActualXValues);
    this._highValues = (IList<double>) ChartSeriesBase.Clone(series.ActualSeriesYValues[0]);
    this._lowValues = (IList<double>) ChartSeriesBase.Clone(series.ActualSeriesYValues[1]);
    this._closeValues = (IList<double>) ChartSeriesBase.Clone(series.ActualSeriesYValues[2]);
    this.Area.ScheduleUpdate();
  }

  protected internal override void GeneratePoints()
  {
    this.GeneratePoints(new string[3]
    {
      this.High,
      this.Low,
      this.Close
    }, this._highValues, this._lowValues, this._closeValues);
  }

  public override void CreateSegments()
  {
    this._xValues = this.GetXValues();
    if (this.Period < 1 || this.Period >= this.DataCount)
    {
      this.Segments.Clear();
    }
    else
    {
      this.ComputeAverageTrueRange(this.Period);
      if (this._fastLineSegment == null || this.Segments.Count == 0)
      {
        this._fastLineSegment = new TechnicalIndicatorSegment(this.xPoints, this.yPoints, this.SignalLineColor, (ChartSeriesBase) this, this.Period);
        this._fastLineSegment.SetValues(this.xPoints, this.yPoints, this.SignalLineColor, (ChartSeriesBase) this, this.Period);
        this.Segments.Add((ChartSegment) this._fastLineSegment);
      }
      else
      {
        this._fastLineSegment.SetData((IList<double>) this.xPoints, (IList<double>) this.yPoints, this.SignalLineColor, this.Period);
        this._fastLineSegment.SetRange();
      }
    }
  }

  private void ComputeAverageTrueRange(int len)
  {
    this.xPoints.Clear();
    this.yPoints.Clear();
    double num = 0.0;
    this.range.Add(this._highValues[0] - this._lowValues[0]);
    this.HCp.Add(double.NaN);
    this.LCp.Add(double.NaN);
    this.trueRange.Add(this.range[0]);
    for (int index = 1; index < this.DataCount; ++index)
    {
      this.range.Add(this._highValues[index] - this._lowValues[index]);
      this.HCp.Add(Math.Abs(this._highValues[index] - this._closeValues[index - 1]));
      this.LCp.Add(Math.Abs(this._lowValues[index] - this._closeValues[index - 1]));
      this.trueRange.Add(Math.Max(this.range[index], Math.Max(this.HCp[index], this.LCp[index])));
    }
    for (int index = 0; index < len; ++index)
    {
      this.xPoints.Add(this._xValues[index]);
      this.yPoints.Add(0.0);
      num += this.trueRange[index];
    }
    this.yPoints[len - 1] = num / (double) len;
    for (int index = len; index < this.DataCount; ++index)
    {
      this.xPoints.Add(this._xValues[index]);
      this.yPoints.Add((this.yPoints[index - 1] * (double) (len - 1) + this.trueRange[index]) / (double) len);
    }
  }

  public override void UpdateSegments(int index, NotifyCollectionChangedAction action)
  {
    this.Area.ScheduleUpdate();
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    obj = (DependencyObject) new AverageTrueRangeIndicator()
    {
      SignalLineColor = this.SignalLineColor,
      Period = this.Period
    };
    return base.CloneSeries(obj);
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) null;
}
