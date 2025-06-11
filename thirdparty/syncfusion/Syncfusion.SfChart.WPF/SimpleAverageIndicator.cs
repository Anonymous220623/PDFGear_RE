// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SimpleAverageIndicator
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SimpleAverageIndicator : FinancialTechnicalIndicator
{
  private IList<double> YValues = (IList<double>) new List<double>();
  private List<double> xValues;
  private List<double> xPoints = new List<double>();
  private List<double> yPoints = new List<double>();
  private TechnicalIndicatorSegment fastLineSegment;
  public static readonly DependencyProperty PeriodProperty = DependencyProperty.Register(nameof (Period), typeof (int), typeof (SimpleAverageIndicator), new PropertyMetadata((object) 2, new PropertyChangedCallback(SimpleAverageIndicator.OnMovingAverageChanged)));
  public static readonly DependencyProperty SignalLineColorProperty = DependencyProperty.Register(nameof (SignalLineColor), typeof (Brush), typeof (SimpleAverageIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Blue), new PropertyChangedCallback(SimpleAverageIndicator.OnColorChanged)));

  public int Period
  {
    get => (int) this.GetValue(SimpleAverageIndicator.PeriodProperty);
    set => this.SetValue(SimpleAverageIndicator.PeriodProperty, (object) value);
  }

  public Brush SignalLineColor
  {
    get => (Brush) this.GetValue(SimpleAverageIndicator.SignalLineColorProperty);
    set => this.SetValue(SimpleAverageIndicator.SignalLineColorProperty, (object) value);
  }

  private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SimpleAverageIndicator averageIndicator))
      return;
    averageIndicator.UpdateArea();
  }

  private static void OnMovingAverageChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as SimpleAverageIndicator).UpdateArea();
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnDataSourceChanged(oldValue, newValue);
    this.fastLineSegment = (TechnicalIndicatorSegment) null;
    this.YValues.Clear();
    this.GeneratePoints(new string[1]{ this.Close }, this.YValues);
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.fastLineSegment = (TechnicalIndicatorSegment) null;
    this.YValues.Clear();
    base.OnBindingPathChanged(args);
  }

  protected internal override void SetSeriesItemSource(ChartSeriesBase series)
  {
    if (series.ActualSeriesYValues.Length <= 0)
      return;
    this.ActualXValues = this.Clone(series.ActualXValues);
    this.YValues = (IList<double>) ChartSeriesBase.Clone(series.ActualSeriesYValues[0]);
    this.Area.ScheduleUpdate();
  }

  protected internal override void GeneratePoints()
  {
    this.GeneratePoints(new string[1]{ this.Close }, this.YValues);
  }

  public override void CreateSegments()
  {
    this.xValues = this.GetXValues();
    if (this.Period < 1)
      return;
    this.Period = this.Period < this.xValues.Count ? this.Period : this.xValues.Count - 1;
    FinancialTechnicalIndicator.ComputeMovingAverage(this.Period, this.xValues, this.YValues, this.xPoints, this.yPoints);
    if (this.fastLineSegment == null)
    {
      TechnicalIndicatorSegment indicatorSegment = new TechnicalIndicatorSegment(this.xValues, this.yPoints, this.SignalLineColor, (ChartSeriesBase) this, this.Period);
      indicatorSegment.SetValues(this.xValues, this.yPoints, this.SignalLineColor, (ChartSeriesBase) this, this.Period);
      this.fastLineSegment = indicatorSegment;
      this.Segments.Add((ChartSegment) indicatorSegment);
    }
    else
    {
      if (this.ActualXValues == null)
        return;
      this.fastLineSegment.SetData((IList<double>) this.xValues, (IList<double>) this.yPoints, this.SignalLineColor, this.Period);
      this.fastLineSegment.SetRange();
    }
  }

  public override void UpdateSegments(int index, NotifyCollectionChangedAction action)
  {
    this.Area.ScheduleUpdate();
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    obj = (DependencyObject) new SimpleAverageIndicator()
    {
      SignalLineColor = this.SignalLineColor,
      Period = this.Period
    };
    return base.CloneSeries(obj);
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) null;
}
