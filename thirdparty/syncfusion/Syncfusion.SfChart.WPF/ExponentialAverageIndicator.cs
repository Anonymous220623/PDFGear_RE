// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ExponentialAverageIndicator
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

public class ExponentialAverageIndicator : FinancialTechnicalIndicator
{
  private IList<double> CloseValues = (IList<double>) new List<double>();
  private List<double> xValues;
  private List<double> xPoints = new List<double>();
  private List<double> yPoints = new List<double>();
  private TechnicalIndicatorSegment fastLineSegment;
  public static readonly DependencyProperty PeriodProperty = DependencyProperty.Register(nameof (Period), typeof (int), typeof (ExponentialAverageIndicator), new PropertyMetadata((object) 2, new PropertyChangedCallback(ExponentialAverageIndicator.OnPeriodChanged)));
  public static readonly DependencyProperty SignalLineColorProperty = DependencyProperty.Register(nameof (SignalLineColor), typeof (Brush), typeof (ExponentialAverageIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Green), new PropertyChangedCallback(ExponentialAverageIndicator.OnColorChanged)));

  public int Period
  {
    get => (int) this.GetValue(ExponentialAverageIndicator.PeriodProperty);
    set => this.SetValue(ExponentialAverageIndicator.PeriodProperty, (object) value);
  }

  public Brush SignalLineColor
  {
    get => (Brush) this.GetValue(ExponentialAverageIndicator.SignalLineColorProperty);
    set => this.SetValue(ExponentialAverageIndicator.SignalLineColorProperty, (object) value);
  }

  private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ExponentialAverageIndicator).UpdateArea();
  }

  private static void OnPeriodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ExponentialAverageIndicator).UpdateArea();
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnDataSourceChanged(oldValue, newValue);
    this.fastLineSegment = (TechnicalIndicatorSegment) null;
    this.CloseValues.Clear();
    this.GeneratePoints(new string[1]{ this.Close }, this.CloseValues);
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.fastLineSegment = (TechnicalIndicatorSegment) null;
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
    this.xValues = this.GetXValues();
    if (this.Period < 1)
      return;
    this.Period = this.Period < this.xValues.Count ? this.Period : this.xValues.Count - 1;
    this.CalculateExponential(this.Period, this.xValues, this.xPoints, this.yPoints);
    if (this.fastLineSegment == null)
    {
      this.fastLineSegment = new TechnicalIndicatorSegment(this.xValues, this.yPoints, this.SignalLineColor, (ChartSeriesBase) this, this.Period);
      this.fastLineSegment.SetValues(this.xValues, this.yPoints, this.SignalLineColor, (ChartSeriesBase) this, this.Period);
      this.Segments.Add((ChartSegment) this.fastLineSegment);
    }
    else
    {
      if (this.ActualXValues == null)
        return;
      this.fastLineSegment.SetData((IList<double>) this.xPoints, (IList<double>) this.yPoints, this.SignalLineColor, this.Period);
      this.fastLineSegment.SetRange();
    }
  }

  private void CalculateExponential(
    int len,
    List<double> xValues,
    List<double> xPoints,
    List<double> yPoints)
  {
    double d = double.NaN;
    double num1 = 2.0 / (1.0 + (double) len);
    double num2 = 1.0 - num1;
    xPoints.Clear();
    yPoints.Clear();
    for (int index = 0; index < this.DataCount; ++index)
    {
      d = !double.IsNaN(d) ? num1 * this.CloseValues[index] + num2 * d : this.CloseValues[index];
      yPoints.Add(d);
      xPoints.Add(xValues[index]);
    }
  }

  public override void UpdateSegments(int index, NotifyCollectionChangedAction action)
  {
    this.Area.ScheduleUpdate();
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    obj = (DependencyObject) new ExponentialAverageIndicator()
    {
      Period = this.Period,
      SignalLineColor = this.SignalLineColor
    };
    return base.CloneSeries(obj);
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) null;
}
