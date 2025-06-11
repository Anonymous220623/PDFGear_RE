// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.MomentumTechnicalIndicator
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

public class MomentumTechnicalIndicator : FinancialTechnicalIndicator
{
  private IList<double> closeValues = (IList<double>) new List<double>();
  private List<double> xValues;
  private List<double> momentumXPoints = new List<double>();
  private List<double> momentumYPoints = new List<double>();
  private List<double> centerXPoints = new List<double>();
  private List<double> centerYPoints = new List<double>();
  private TechnicalIndicatorSegment momentumLineSegment;
  private TechnicalIndicatorSegment centerlLineSegment;
  public static readonly DependencyProperty PeriodProperty = DependencyProperty.Register(nameof (Period), typeof (int), typeof (MomentumTechnicalIndicator), new PropertyMetadata((object) 14, new PropertyChangedCallback(MomentumTechnicalIndicator.OnMovingAverageChanged)));
  public static readonly DependencyProperty MomentumLineColorProperty = DependencyProperty.Register(nameof (MomentumLineColor), typeof (Brush), typeof (MomentumTechnicalIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Red), new PropertyChangedCallback(MomentumTechnicalIndicator.OnColorChanged)));
  public static readonly DependencyProperty CenterLineColorProperty = DependencyProperty.Register(nameof (CenterLineColor), typeof (Brush), typeof (MomentumTechnicalIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Green), new PropertyChangedCallback(MomentumTechnicalIndicator.OnColorChanged)));

  public int Period
  {
    get => (int) this.GetValue(MomentumTechnicalIndicator.PeriodProperty);
    set => this.SetValue(MomentumTechnicalIndicator.PeriodProperty, (object) value);
  }

  public Brush MomentumLineColor
  {
    get => (Brush) this.GetValue(MomentumTechnicalIndicator.MomentumLineColorProperty);
    set => this.SetValue(MomentumTechnicalIndicator.MomentumLineColorProperty, (object) value);
  }

  public Brush CenterLineColor
  {
    get => (Brush) this.GetValue(MomentumTechnicalIndicator.CenterLineColorProperty);
    set => this.SetValue(MomentumTechnicalIndicator.CenterLineColorProperty, (object) value);
  }

  internal override void SetIndicatorInfo(
    ChartPointInfo info,
    List<double> yValue,
    bool seriesPalette)
  {
    if (yValue.Count <= 0)
      return;
    info.SignalLine = double.IsNaN(yValue[0]) ? "null" : Math.Round(yValue[0], 2).ToString();
  }

  private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is MomentumTechnicalIndicator technicalIndicator))
      return;
    technicalIndicator.UpdateArea();
  }

  private static void OnMovingAverageChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as MomentumTechnicalIndicator).UpdateArea();
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnDataSourceChanged(oldValue, newValue);
    this.momentumLineSegment = (TechnicalIndicatorSegment) null;
    this.centerlLineSegment = (TechnicalIndicatorSegment) null;
    this.closeValues.Clear();
    this.GeneratePoints(new string[1]{ this.Close }, this.closeValues);
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.closeValues.Clear();
    base.OnBindingPathChanged(args);
  }

  protected internal override void SetSeriesItemSource(ChartSeriesBase series)
  {
    if (series.ActualSeriesYValues.Length <= 0)
      return;
    this.ActualXValues = this.Clone(series.ActualXValues);
    this.closeValues = (IList<double>) ChartSeriesBase.Clone(series.ActualSeriesYValues[0]);
    this.Area.ScheduleUpdate();
  }

  protected internal override void GeneratePoints()
  {
    this.GeneratePoints(new string[1]{ this.Close }, this.closeValues);
  }

  public override void CreateSegments()
  {
    this.xValues = this.GetXValues();
    if (this.Period < 1)
      return;
    if (this.Period < this.DataCount)
    {
      this.ComputeMomentum(this.Period);
      this.centerXPoints.Clear();
      this.centerYPoints.Clear();
      for (int index = 0; index < this.DataCount; ++index)
      {
        this.centerXPoints.Add(this.xValues[index]);
        this.centerYPoints.Add(100.0);
      }
      if (this.momentumLineSegment == null || this.centerlLineSegment == null)
      {
        this.Segments.Clear();
        this.momentumLineSegment = new TechnicalIndicatorSegment(this.momentumXPoints, this.momentumYPoints, this.MomentumLineColor, (ChartSeriesBase) this);
        this.momentumLineSegment.SetValues(this.momentumXPoints, this.momentumYPoints, this.MomentumLineColor, (ChartSeriesBase) this);
        this.Segments.Add((ChartSegment) this.momentumLineSegment);
        this.centerlLineSegment = new TechnicalIndicatorSegment(this.centerXPoints, this.centerYPoints, this.CenterLineColor, (ChartSeriesBase) this);
        this.centerlLineSegment.SetValues(this.centerXPoints, this.centerYPoints, this.CenterLineColor, (ChartSeriesBase) this);
        this.Segments.Add((ChartSegment) this.centerlLineSegment);
      }
      else
      {
        this.momentumLineSegment.SetData((IList<double>) this.momentumXPoints, (IList<double>) this.momentumYPoints, this.MomentumLineColor);
        this.momentumLineSegment.SetRange();
        this.centerlLineSegment.SetData((IList<double>) this.centerXPoints, (IList<double>) this.centerYPoints, this.CenterLineColor);
        this.centerlLineSegment.SetRange();
      }
    }
    if (this.momentumLineSegment == null || this.centerlLineSegment == null || this.DataCount <= 0)
      return;
    this.momentumLineSegment.SetRange();
    this.centerlLineSegment.SetRange();
  }

  public override void UpdateSegments(int index, NotifyCollectionChangedAction action)
  {
    base.UpdateSegments(index, action);
    this.Area.ScheduleUpdate();
  }

  private void ComputeMomentum(int len)
  {
    this.momentumXPoints.Clear();
    this.momentumYPoints.Clear();
    for (int index = 0; index < this.DataCount; ++index)
    {
      if (index >= len)
      {
        this.momentumXPoints.Add(this.xValues[index]);
        this.momentumYPoints.Add(this.closeValues[index] / this.closeValues[index - len] * 100.0);
      }
    }
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    obj = (DependencyObject) new MomentumTechnicalIndicator()
    {
      Period = this.Period,
      MomentumLineColor = this.MomentumLineColor,
      CenterLineColor = this.CenterLineColor
    };
    return base.CloneSeries(obj);
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) null;
}
