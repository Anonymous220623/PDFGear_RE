// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.StackingSeriesBase3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class StackingSeriesBase3D : XyzDataSeries3D
{
  public static readonly DependencyProperty GroupingLabelProperty = DependencyProperty.Register(nameof (GroupingLabel), typeof (string), typeof (StackingSeriesBase3D), new PropertyMetadata((object) null, new PropertyChangedCallback(StackingSeriesBase3D.OnGroupingLabelChanged)));

  public string GroupingLabel
  {
    get => (string) this.GetValue(StackingSeriesBase3D.GroupingLabelProperty);
    set => this.SetValue(StackingSeriesBase3D.GroupingLabelProperty, (object) value);
  }

  internal bool StackValueCalculated { get; set; }

  protected internal IList<double> YRangeStartValues { get; set; }

  protected internal IList<double> YRangeEndValues { get; set; }

  public override void FindNearestChartPoint(
    Point point,
    out double x,
    out double y,
    out double stackedYValue)
  {
    base.FindNearestChartPoint(point, out x, out y, out stackedYValue);
    if (double.IsNaN(x) || double.IsNaN(y))
      return;
    if (this.ActualXValues is IList<double> && !this.IsIndexed)
    {
      IList<double> actualXvalues = this.ActualXValues as IList<double>;
      stackedYValue = this.GetStackedYValue(actualXvalues.IndexOf(x));
    }
    else
      stackedYValue = this.GetStackedYValue((int) x);
  }

  public StackingValues GetCumulativeStackValues(ChartSeriesBase series)
  {
    if (this.Area.StackedValues == null || !this.Area.StackedValues.Keys.Contains<object>((object) series))
      this.CalculateStackingValues();
    return this.Area.StackedValues == null || !this.Area.StackedValues.Keys.Contains<object>((object) series) ? (StackingValues) null : this.Area.StackedValues[(object) series];
  }

  protected double GetStackedYValue(int index) => this.YRangeEndValues[index];

  protected override DependencyObject CloneSeries(DependencyObject obj) => base.CloneSeries(obj);

  private static void OnGroupingLabelChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is StackingSeriesBase3D stackingSeriesBase3D) || stackingSeriesBase3D.Area == null || stackingSeriesBase3D.ActualArea == null)
      return;
    stackingSeriesBase3D.ActualArea.SBSInfoCalculated = false;
    stackingSeriesBase3D.Area.ScheduleUpdate();
  }

  private void CalculateStackingValues()
  {
    this.Area.StackedValues = new Dictionary<object, StackingValues>();
    IEnumerable<ChartSeriesBase> source1 = this.Area.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series is StackingSeriesBase3D));
    if (!(source1 is ChartSeriesBase[] chartSeriesBaseArray))
      chartSeriesBaseArray = source1.ToArray<ChartSeriesBase>();
    ChartSeriesBase[] source2 = chartSeriesBaseArray;
    int num1 = 0;
    foreach (ChartSeriesBase chartSeriesBase1 in source2)
    {
      foreach (var data in chartSeriesBase1.ActualYAxis.RegisteredSeries.Where<ISupportAxes>((Func<ISupportAxes, bool>) (seriesGroup => seriesGroup is StackingSeriesBase3D && (seriesGroup as StackingSeriesBase3D).IsSeriesVisible)).GroupBy<ISupportAxes, string>((Func<ISupportAxes, string>) (seriesGroup => (seriesGroup as StackingSeriesBase3D).GroupingLabel)).Select(groups => new
      {
        GroupingPath = groups.Key,
        Series = groups
      }))
      {
        bool isReCalculation = true;
        List<double> doubleList1 = new List<double>();
        List<double> doubleList2 = new List<double>();
        bool flag = true;
        foreach (ChartSeriesBase chartSeriesBase2 in (IEnumerable<ISupportAxes>) data.Series)
        {
          ChartSeriesBase chartSeries = chartSeriesBase2;
          if (chartSeries is StackingSeriesBase3D && chartSeries.IsSeriesVisible)
          {
            if (!((StackingSeriesBase3D) chartSeries).StackValueCalculated)
            {
              StackingValues stackingValues = new StackingValues()
              {
                StartValues = (IList<double>) new List<double>(),
                EndValues = (IList<double>) new List<double>()
              };
              IList<double> doubleList3 = ((XyDataSeries3D) chartSeries).YValues;
              if (chartSeries.ActualXAxis is CategoryAxis3D && !(chartSeries.ActualXAxis as CategoryAxis3D).IsIndexed)
              {
                if (!(chartSeries is StackingColumn100Series3D) && !(chartSeries is StackingBar100Series3D))
                {
                  chartSeries.GroupedActualData.Clear();
                  List<double> doubleList4 = new List<double>();
                  for (int key = 0; key < chartSeries.DistinctValuesIndexes.Count; ++key)
                  {
                    List<List<double>> list = chartSeries.DistinctValuesIndexes[(double) key].Select<int, List<double>>((Func<int, List<double>>) (index => new List<double>()
                    {
                      chartSeries.GroupedSeriesYValues[0][index],
                      (double) index
                    })).ToList<List<double>>();
                    for (int index = 0; index < chartSeries.DistinctValuesIndexes[(double) key].Count; ++index)
                    {
                      double num2 = list[index][0];
                      doubleList4.Add(num2);
                      chartSeries.GroupedActualData.Add(this.ActualData[(int) list[index][1]]);
                    }
                  }
                  doubleList3 = (IList<double>) doubleList4;
                }
                else
                {
                  doubleList3 = chartSeries.GroupedSeriesYValues[0];
                  chartSeries.GroupedActualData.AddRange((IEnumerable<object>) chartSeries.ActualData);
                }
              }
              double num3 = this.ActualXAxis != null ? this.ActualXAxis.Origin : 0.0;
              int index1 = 0;
              if (this.ActualXAxis != null && this.ActualXAxis.Origin == 0.0 && this.ActualYAxis is LogarithmicAxis && (this.ActualYAxis as LogarithmicAxis).Minimum.HasValue)
                num3 = (this.ActualYAxis as LogarithmicAxis).Minimum.Value;
              foreach (double num4 in (IEnumerable<double>) doubleList3)
              {
                double num5 = num4;
                if (doubleList1.Count <= index1)
                  doubleList1.Add(0.0);
                if (doubleList2.Count <= index1)
                  doubleList2.Add(0.0);
                if (stackingValues.StartValues.Count <= index1)
                {
                  stackingValues.StartValues.Add(0.0);
                  stackingValues.EndValues.Add(0.0);
                }
                double d;
                if (num5 >= 0.0)
                {
                  d = doubleList1[index1];
                  if (chartSeries.GetType().Name.Contains("100Series"))
                    num5 = this.Area.GetPercentByIndex((data.Series as IList<ISupportAxes>).OfType<StackingSeriesBase3D>().ToList<StackingSeriesBase3D>(), index1, num5, isReCalculation);
                  List<double> doubleList5;
                  int index2;
                  (doubleList5 = doubleList1)[index2 = index1] = doubleList5[index2] + num5;
                }
                else
                {
                  d = doubleList2[index1];
                  if (chartSeries.GetType().Name.Contains("100Series"))
                    num5 = this.Area.GetPercentByIndex((data.Series as IList<ISupportAxes>).OfType<StackingSeriesBase3D>().ToList<StackingSeriesBase3D>(), index1, num5, isReCalculation);
                  List<double> doubleList6;
                  int index3;
                  (doubleList6 = doubleList2)[index3 = index1] = doubleList6[index3] + num5;
                }
                stackingValues.StartValues[index1] = flag ? num3 : d;
                stackingValues.EndValues[index1] = num5 + (double.IsNaN(d) ? num3 : d);
                ++index1;
              }
              flag = false;
              ++num1;
              this.Area.StackedValues.Add((object) chartSeries, stackingValues);
              ((StackingSeriesBase3D) chartSeries).StackValueCalculated = true;
              isReCalculation = false;
            }
            else
              break;
          }
        }
      }
    }
    foreach (StackingSeriesBase3D stackingSeriesBase3D in source2.OfType<StackingSeriesBase3D>())
      stackingSeriesBase3D.StackValueCalculated = false;
  }
}
