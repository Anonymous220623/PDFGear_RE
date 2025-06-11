// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.TriangularAverageIndicator
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

public class TriangularAverageIndicator : FinancialTechnicalIndicator
{
  private IList<double> YValues = (IList<double>) new List<double>();
  private List<double> xValues;
  private List<double> xPoints = new List<double>();
  private List<double> yPoints = new List<double>();
  private TechnicalIndicatorSegment fastLineSegment;
  public static readonly DependencyProperty PeriodProperty = DependencyProperty.Register(nameof (Period), typeof (int), typeof (TriangularAverageIndicator), new PropertyMetadata((object) 2, new PropertyChangedCallback(TriangularAverageIndicator.OnMovingAverageChanged)));
  public static readonly DependencyProperty SignalLineColorProperty = DependencyProperty.Register(nameof (SignalLineColor), typeof (Brush), typeof (TriangularAverageIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Green), new PropertyChangedCallback(TriangularAverageIndicator.OnColorChanged)));

  public int Period
  {
    get => (int) this.GetValue(TriangularAverageIndicator.PeriodProperty);
    set => this.SetValue(TriangularAverageIndicator.PeriodProperty, (object) value);
  }

  public Brush SignalLineColor
  {
    get => (Brush) this.GetValue(TriangularAverageIndicator.SignalLineColorProperty);
    set => this.SetValue(TriangularAverageIndicator.SignalLineColorProperty, (object) value);
  }

  private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as TriangularAverageIndicator).UpdateArea();
  }

  private static void OnMovingAverageChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as TriangularAverageIndicator).UpdateArea();
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
    double period = (double) this.Period;
    if (period < (double) this.DataCount)
      this.AddTriangularPoints((int) period);
    if (this.fastLineSegment == null)
    {
      this.fastLineSegment = new TechnicalIndicatorSegment(this.xPoints, this.yPoints, this.SignalLineColor, (ChartSeriesBase) this);
      this.fastLineSegment.SetValues(this.xPoints, this.yPoints, this.SignalLineColor, (ChartSeriesBase) this, this.Period);
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

  public override void UpdateSegments(int index, NotifyCollectionChangedAction action)
  {
    this.Area.ScheduleUpdate();
  }

  private void AddTriangularPoints(int avg)
  {
    FinancialTechnicalIndicator.ComputeMovingAverage(avg, this.xValues, this.YValues, this.xPoints, this.yPoints);
    List<double> yValues = new List<double>((IEnumerable<double>) this.yPoints);
    for (int index1 = 0; index1 < avg - 1; ++index1)
    {
      double num1 = 0.0;
      for (int index2 = 0; index2 < index1 + 1; ++index2)
        num1 += this.YValues[index2];
      double num2 = num1 / (double) (index1 + 1);
      yValues[index1] = num2;
    }
    FinancialTechnicalIndicator.ComputeMovingAverage(avg, this.xValues, (IList<double>) yValues, this.xPoints, this.yPoints);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    obj = (DependencyObject) new TriangularAverageIndicator()
    {
      SignalLineColor = this.SignalLineColor,
      Period = this.Period
    };
    return base.CloneSeries(obj);
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) null;
}
