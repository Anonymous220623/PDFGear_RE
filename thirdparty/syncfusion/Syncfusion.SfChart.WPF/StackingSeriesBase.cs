// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.StackingSeriesBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class StackingSeriesBase : XyDataSeries
{
  public static readonly DependencyProperty GroupingLabelProperty = DependencyProperty.Register(nameof (GroupingLabel), typeof (string), typeof (StackingSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(StackingSeriesBase.OnGroupingLabelChanged)));
  internal bool stackValueCalculated;

  public string GroupingLabel
  {
    get => (string) this.GetValue(StackingSeriesBase.GroupingLabelProperty);
    set => this.SetValue(StackingSeriesBase.GroupingLabelProperty, (object) value);
  }

  protected internal IList<double> YRangeStartValues { get; set; }

  protected internal IList<double> YRangeEndValues { get; set; }

  public StackingValues GetCumulativeStackValues(ChartSeriesBase series)
  {
    if (this.Area.StackedValues == null || !this.Area.StackedValues.Keys.Contains<object>((object) series))
      this.CalculateStackingValues();
    return this.Area.StackedValues.Keys.Contains<object>((object) series) ? this.Area.StackedValues[(object) series] : (StackingValues) null;
  }

  internal List<string> GetDistinctXValues()
  {
    List<string> source = new List<string>();
    if (this.Area != null)
    {
      foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) this.Area.VisibleSeries)
      {
        foreach (string xvalue in (IEnumerable<string>) (chartSeriesBase.XValues as List<string>))
          source.Add(xvalue);
      }
    }
    return source.Distinct<string>().ToList<string>();
  }

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

  internal override void ReValidateYValues(List<int>[] emptyPointIndex)
  {
    base.ReValidateYValues(emptyPointIndex);
    this.CalculateStackingValues();
  }

  internal override void GeneratePixels()
  {
    if (!this.IsActualTransposed)
      this.GenerateStackingColumnPixels();
    else
      this.GenerateStackingBarPixels();
  }

  internal override void OnDataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    this.ResetStackedValues();
    base.OnDataCollectionChanged(sender, e);
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    this.ResetStackedValues();
    base.OnDataSourceChanged(oldValue, newValue);
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.ResetStackedValues();
    base.OnBindingPathChanged(args);
  }

  internal void GenerateStackingColumnPixels()
  {
    WriteableBitmap fastRenderSurface = this.Area.fastRenderSurface;
    ChartTransform.ChartCartesianTransformer transformer = this.CreateTransformer(new Size(this.Area.SeriesClipRect.Width, this.Area.SeriesClipRect.Height), true) as ChartTransform.ChartCartesianTransformer;
    bool isInversed1 = transformer.XAxis.IsInversed;
    bool isInversed2 = transformer.YAxis.IsInversed;
    int index = this.dataPoint.Index;
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    List<double> stackingSeriesXvalues = StackingSeriesBase.GetStackingSeriesXValues((XyDataSeries) this);
    double num1;
    double num2;
    double yrangeEndValue;
    double yrangeStartValue;
    if (!this.IsIndexed)
    {
      num1 = stackingSeriesXvalues[index] + sideBySideInfo.Start;
      num2 = stackingSeriesXvalues[index] + sideBySideInfo.End;
      yrangeEndValue = this.YRangeEndValues[index];
      yrangeStartValue = this.YRangeStartValues[index];
    }
    else
    {
      num1 = (double) index + sideBySideInfo.Start;
      num2 = (double) index + sideBySideInfo.End;
      yrangeEndValue = this.YRangeEndValues[index];
      yrangeStartValue = this.YRangeStartValues[index];
    }
    double x1 = isInversed1 ? num2 : num1;
    double x2 = isInversed1 ? num1 : num2;
    double y1 = isInversed2 ? yrangeEndValue : yrangeStartValue;
    double y2 = isInversed2 ? yrangeStartValue : yrangeEndValue;
    Point visible1 = transformer.TransformToVisible(x1, y2);
    Point visible2 = transformer.TransformToVisible(x2, y1);
    float x3 = (float) visible1.X;
    float x4 = (float) visible2.X;
    float y3 = (float) visible1.Y;
    float y4 = (float) visible2.Y;
    this.selectedSegmentPixels.Clear();
    float num3 = x3;
    float num4 = x4;
    float num5 = yrangeEndValue > 0.0 ? y3 : y4;
    float num6 = yrangeEndValue > 0.0 ? y4 : y3;
    if (this is ISegmentSpacing segmentSpacing)
    {
      double segmentSpacing1 = segmentSpacing.SegmentSpacing;
      if (segmentSpacing1 > 0.0 && segmentSpacing1 <= 1.0)
      {
        double segmentSpacing2 = segmentSpacing.CalculateSegmentSpacing(segmentSpacing1, (double) num4, (double) num3);
        double segmentSpacing3 = segmentSpacing.CalculateSegmentSpacing(segmentSpacing1, (double) num3, (double) num4);
        num3 = (float) segmentSpacing2;
        num4 = (float) segmentSpacing3;
      }
    }
    if ((double) num5 < (double) num6)
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) num3, (int) num5, (int) num4, (int) num6, this.selectedSegmentPixels);
    else
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) num3, (int) num6, (int) num4, (int) num5, this.selectedSegmentPixels);
  }

  internal void GenerateStackingBarPixels()
  {
    WriteableBitmap fastRenderSurface = this.Area.fastRenderSurface;
    ChartTransform.ChartCartesianTransformer transformer = this.CreateTransformer(new Size(this.Area.SeriesClipRect.Width, this.Area.SeriesClipRect.Height), true) as ChartTransform.ChartCartesianTransformer;
    bool flag = transformer.XAxis.IsLogarithmic || transformer.YAxis.IsLogarithmic;
    bool isInversed1 = transformer.XAxis.IsInversed;
    bool isInversed2 = transformer.YAxis.IsInversed;
    int index = this.dataPoint.Index;
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    List<double> stackingSeriesXvalues = StackingSeriesBase.GetStackingSeriesXValues((XyDataSeries) this);
    double a1;
    double a2;
    double yrangeEndValue;
    double yrangeStartValue;
    if (!this.IsIndexed)
    {
      a1 = stackingSeriesXvalues[index] + sideBySideInfo.Start;
      a2 = stackingSeriesXvalues[index] + sideBySideInfo.End;
      yrangeEndValue = this.YRangeEndValues[index];
      yrangeStartValue = this.YRangeStartValues[index];
    }
    else
    {
      a1 = (double) index + sideBySideInfo.Start;
      a2 = (double) index + sideBySideInfo.End;
      yrangeEndValue = this.YRangeEndValues[index];
      yrangeStartValue = this.YRangeStartValues[index];
    }
    double num1 = transformer.XAxis.VisibleRange.Start;
    double num2 = transformer.XAxis.VisibleRange.End;
    double num3 = transformer.YAxis.VisibleRange.Start;
    double num4 = transformer.YAxis.VisibleRange.End;
    double height1 = transformer.XAxis.RenderedRect.Height;
    double width1 = transformer.YAxis.RenderedRect.Width;
    double num5 = this.Area.SeriesClipRect.Right - transformer.YAxis.RenderedRect.Right;
    double num6 = this.Area.SeriesClipRect.Bottom - transformer.XAxis.RenderedRect.Bottom;
    Size size = new Size(height1, width1);
    if (isInversed1)
    {
      double num7 = num1;
      num1 = num2;
      num2 = num7;
    }
    if (isInversed2)
    {
      double num8 = num3;
      num3 = num4;
      num4 = num8;
    }
    float num9;
    float num10;
    float num11;
    float num12;
    if (!flag)
    {
      double num13 = isInversed1 ? (a2 < num2 ? num2 : a2) : (a1 < num1 ? num1 : a1);
      double num14 = isInversed1 ? (a1 > num1 ? num1 : a1) : (a2 > num2 ? num2 : a2);
      double num15 = isInversed2 ? (yrangeStartValue > num3 ? num3 : (yrangeStartValue < num4 ? num4 : yrangeStartValue)) : (yrangeEndValue > num4 ? num4 : (yrangeEndValue < num3 ? num3 : yrangeEndValue));
      double num16 = isInversed2 ? (yrangeEndValue < num4 ? num4 : (yrangeEndValue > num3 ? num3 : yrangeEndValue)) : (yrangeStartValue < num3 ? num3 : (yrangeStartValue > num4 ? num4 : yrangeStartValue));
      num9 = (float) (num6 + size.Width * transformer.XAxis.ValueToCoefficientCalc(num13));
      num10 = (float) (num6 + size.Width * transformer.XAxis.ValueToCoefficientCalc(num14));
      num11 = (float) (num5 + size.Height * (1.0 - transformer.YAxis.ValueToCoefficientCalc(num15)));
      num12 = (float) (num5 + size.Height * (1.0 - transformer.YAxis.ValueToCoefficientCalc(num16)));
    }
    else
    {
      double newBase1 = transformer.XAxis.IsLogarithmic ? (transformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
      double newBase2 = transformer.YAxis.IsLogarithmic ? (transformer.YAxis as LogarithmicAxis).LogarithmicBase : 1.0;
      double num17 = newBase1 == 1.0 ? a1 : Math.Log(a1, newBase1);
      double num18 = newBase1 == 1.0 ? a2 : Math.Log(a2, newBase1);
      double num19 = newBase2 == 1.0 ? yrangeEndValue : Math.Log(yrangeEndValue, newBase2);
      double num20 = newBase2 == 1.0 ? yrangeStartValue : Math.Log(yrangeStartValue, newBase2);
      double num21 = isInversed1 ? (num18 < num2 ? num2 : num18) : (num17 < num1 ? num1 : num17);
      double num22 = isInversed1 ? (num17 > num1 ? num1 : num17) : (num18 > num2 ? num2 : num18);
      double num23 = isInversed2 ? (num20 > num3 ? num3 : (num20 < num4 ? num4 : num20)) : (num19 > num4 ? num4 : (num19 < num3 ? num3 : num19));
      double num24 = isInversed2 ? (num19 < num4 ? num4 : (num19 > num3 ? num3 : num19)) : (num20 < num3 ? num3 : (num20 > num4 ? num4 : num20));
      num9 = (float) (num6 + size.Width * transformer.XAxis.ValueToCoefficientCalc(num21));
      num10 = (float) (num6 + size.Width * transformer.XAxis.ValueToCoefficientCalc(num22));
      num11 = (float) (num5 + size.Height * (1.0 - transformer.YAxis.ValueToCoefficientCalc(num23)));
      num12 = (float) (num5 + size.Height * (1.0 - transformer.YAxis.ValueToCoefficientCalc(num24)));
    }
    Rect seriesClipRect = this.Area.SeriesClipRect;
    double width2 = (double) (int) seriesClipRect.Width;
    seriesClipRect = this.Area.SeriesClipRect;
    double height2 = (double) (int) seriesClipRect.Height;
    float num25 = num9;
    float num26 = num10;
    float num27 = yrangeEndValue > 0.0 ? num11 : num12;
    float num28 = yrangeEndValue > 0.0 ? num12 : num11;
    if (this is ISegmentSpacing segmentSpacing)
    {
      double segmentSpacing1 = segmentSpacing.SegmentSpacing;
      if (segmentSpacing1 > 0.0 && segmentSpacing1 <= 1.0)
      {
        double segmentSpacing2 = segmentSpacing.CalculateSegmentSpacing(segmentSpacing1, (double) num25, (double) num26);
        double segmentSpacing3 = segmentSpacing.CalculateSegmentSpacing(segmentSpacing1, (double) num26, (double) num25);
        num26 = (float) segmentSpacing2;
        num25 = (float) segmentSpacing3;
      }
    }
    float num29 = num26 - num25;
    this.selectedSegmentPixels.Clear();
    if ((double) num27 < (double) num28)
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) (width2 - (double) num28), (int) (height2 - (double) num25 - (double) num29), (int) (width2 - (double) num27), (int) (height2 - (double) num25), this.selectedSegmentPixels);
    else
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) (width2 - (double) num27), (int) (height2 - (double) num25 - (double) num29), (int) (width2 - (double) num28), (int) (height2 - (double) num25), this.selectedSegmentPixels);
  }

  protected double GetStackedYValue(int index)
  {
    return index >= this.YRangeEndValues.Count ? double.NaN : this.YRangeEndValues[index];
  }

  private static void OnGroupingLabelChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is StackingSeriesBase stackingSeriesBase) || stackingSeriesBase.Area == null || stackingSeriesBase.ActualArea == null)
      return;
    stackingSeriesBase.ActualArea.SBSInfoCalculated = false;
    stackingSeriesBase.Area.ScheduleUpdate();
  }

  private void CalculateStackingValues()
  {
    this.Area.StackedValues = new Dictionary<object, StackingValues>();
    IEnumerable<ChartSeriesBase> chartSeriesBases = this.Area.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series is StackingSeriesBase && series.ActualYAxis != null && series.ActualXAxis != null));
    foreach (ChartSeriesBase chartSeriesBase1 in chartSeriesBases)
    {
      foreach (var data in chartSeriesBase1.ActualYAxis.RegisteredSeries.Intersect<ISupportAxes>((IEnumerable<ISupportAxes>) chartSeriesBase1.ActualXAxis.RegisteredSeries).ToList<ISupportAxes>().Where<ISupportAxes>((Func<ISupportAxes, bool>) (seriesGroup => seriesGroup is StackingSeriesBase)).GroupBy<ISupportAxes, string>((Func<ISupportAxes, string>) (seriesGroup => (seriesGroup as StackingSeriesBase).GroupingLabel)).Select(groups => new
      {
        GroupingPath = groups.Key,
        Series = groups
      }))
      {
        int num1 = 0;
        List<double> doubleList1 = new List<double>();
        List<double> doubleList2 = new List<double>();
        List<double> doubleList3 = new List<double>();
        bool reCalculation = true;
        foreach (ChartSeriesBase chartSeriesBase2 in (IEnumerable<ISupportAxes>) data.Series)
        {
          ChartSeriesBase chartSeries = chartSeriesBase2;
          if (chartSeries is StackingSeriesBase && chartSeries.IsSeriesVisible)
          {
            if (!((StackingSeriesBase) chartSeries).stackValueCalculated)
            {
              StackingValues stackingValues = new StackingValues();
              stackingValues.StartValues = (IList<double>) new List<double>();
              stackingValues.EndValues = (IList<double>) new List<double>();
              IList<double> doubleList4 = ((XyDataSeries) chartSeries).YValues;
              IList<string> xvalues = (IList<string>) (chartSeries.XValues as List<string>);
              List<double> stackingSeriesXvalues = StackingSeriesBase.GetStackingSeriesXValues((XyDataSeries) chartSeries);
              if (chartSeries.ActualXAxis is CategoryAxis && !(chartSeries.ActualXAxis as CategoryAxis).IsIndexed)
              {
                if (chartSeries is StackingLine100Series && chartSeries is StackingLineSeries)
                {
                  chartSeries.GroupedActualData.Clear();
                  List<double> doubleList5 = new List<double>();
                  for (int key = 0; key < chartSeries.DistinctValuesIndexes.Count; ++key)
                  {
                    if (chartSeries.DistinctValuesIndexes[(double) key].Count > 0)
                    {
                      List<List<double>> list = chartSeries.DistinctValuesIndexes[(double) key].Where<int>((Func<int, bool>) (index => chartSeries.GroupedSeriesYValues[0].Count > index)).Select<int, List<double>>((Func<int, List<double>>) (index => new List<double>()
                      {
                        chartSeries.GroupedSeriesYValues[0][index],
                        (double) index
                      })).OrderByDescending<List<double>, double>((Func<List<double>, double>) (val => val[0])).ToList<List<double>>();
                      for (int index = 0; index < list.Count; ++index)
                      {
                        double num2 = list[index][0];
                        doubleList5.Add(num2);
                        chartSeries.DistinctValuesIndexes[(double) key][index] = (int) list[index][1];
                        chartSeries.GroupedActualData.Add(chartSeries.ActualData[(int) list[index][1]]);
                      }
                    }
                  }
                  doubleList4 = (IList<double>) doubleList5;
                }
                else
                {
                  doubleList4 = chartSeries.GroupedSeriesYValues[0];
                  chartSeries.GroupedActualData.AddRange((IEnumerable<object>) chartSeries.ActualData);
                }
              }
              double num3 = this.ActualXAxis != null ? this.ActualXAxis.Origin : 0.0;
              int index1 = 0;
              if (this.ActualXAxis != null && this.ActualXAxis.Origin == 0.0 && this.ActualYAxis is LogarithmicAxis && (this.ActualYAxis as LogarithmicAxis).Minimum.HasValue)
                num3 = (this.ActualYAxis as LogarithmicAxis).Minimum.Value;
              foreach (double num4 in (IEnumerable<double>) doubleList4)
              {
                double d1 = 0.0;
                double d2 = num4;
                if (doubleList1.Count <= index1)
                  doubleList1.Add(0.0);
                if (doubleList3.Count <= index1)
                  doubleList3.Add(0.0);
                if (doubleList2.Count <= index1)
                  doubleList2.Add(0.0);
                if (stackingValues.StartValues.Count <= index1)
                {
                  stackingValues.StartValues.Add(0.0);
                  stackingValues.EndValues.Add(0.0);
                }
                bool flag = false;
                int index2 = 0;
                for (int index3 = 0; index3 < doubleList2.Count; ++index3)
                {
                  if (stackingSeriesXvalues.Count > index1 && doubleList2[index3] == stackingSeriesXvalues[index1])
                  {
                    index2 = index3;
                    flag = true;
                    break;
                  }
                }
                string str = "Stacking";
                string xItem = xvalues != null ? xvalues[index1] : string.Empty;
                if (flag)
                {
                  if (d2 >= 0.0)
                  {
                    d1 = doubleList1[index2];
                    if (chartSeries.GetType().Name.Contains(str) && chartSeries.GetType().Name.Contains("100Series"))
                      d2 = this.Area.GetPercentage(data.Series as IList<ISupportAxes>, xItem, d2, index1, reCalculation);
                    if (!double.IsNaN(doubleList1[index2]))
                    {
                      List<double> doubleList6;
                      int index4;
                      (doubleList6 = doubleList1)[index4 = index2] = doubleList6[index4] + d2;
                    }
                    else
                      doubleList1[index2] = d2;
                  }
                  else
                  {
                    d1 = doubleList3[index2];
                    if (chartSeries.GetType().Name.Contains(str) && chartSeries.GetType().Name.Contains("100Series"))
                      d2 = this.Area.GetPercentage(data.Series as IList<ISupportAxes>, xItem, d2, index1, reCalculation);
                    if (!double.IsNaN(doubleList1[index2]))
                    {
                      if (!double.IsNaN(d2))
                      {
                        List<double> doubleList7;
                        int index5;
                        (doubleList7 = doubleList3)[index5 = index2] = doubleList7[index5] + d2;
                      }
                    }
                    else
                      doubleList3[index2] = d2;
                  }
                }
                else
                {
                  if (d2 >= 0.0)
                  {
                    if (chartSeries.GetType().Name.Contains(str) && chartSeries.GetType().Name.Contains("100Series"))
                      d2 = this.Area.GetPercentage(data.Series as IList<ISupportAxes>, xItem, d2, index1, reCalculation);
                    doubleList1.Add(d2);
                    doubleList3.Add(0.0);
                  }
                  else
                  {
                    if (chartSeries.GetType().Name.Contains(str) && chartSeries.GetType().Name.Contains("100Series"))
                      d2 = this.Area.GetPercentage(data.Series as IList<ISupportAxes>, xItem, d2, index1, reCalculation);
                    doubleList1.Add(double.IsNaN(d2) ? d2 : 0.0);
                    doubleList3.Add(d2);
                  }
                  if (stackingSeriesXvalues.Count > index1)
                    doubleList2.Add(stackingSeriesXvalues[index1]);
                }
                stackingValues.StartValues[index1] = num1 == 0 || d1 < num3 ? num3 : d1;
                stackingValues.EndValues[index1] = d2 + (double.IsNaN(d1) ? num3 : d1);
                if (stackingValues.EndValues[index1] < d1)
                  stackingValues.StartValues[index1] = d1 + num3;
                if (chartSeries.GetType().Name.Contains(str) && chartSeries.GetType().Name.Contains("100Series"))
                {
                  if (stackingValues.EndValues[index1] > 100.0)
                    stackingValues.EndValues[index1] = 100.0;
                  if (stackingValues.StartValues[index1] > 100.0)
                    stackingValues.StartValues[index1] = 100.0;
                }
                ++index1;
              }
              ++num1;
              if (stackingValues.StartValues.Count > 0 && stackingValues.EndValues.Count > 0)
              {
                this.Area.StackedValues.Add((object) chartSeries, stackingValues);
                ((StackingSeriesBase) chartSeries).stackValueCalculated = true;
              }
              reCalculation = false;
            }
            else
              break;
          }
        }
      }
    }
    foreach (ChartSeriesBase chartSeriesBase in chartSeriesBases)
    {
      if (chartSeriesBase is StackingSeriesBase stackingSeriesBase)
        stackingSeriesBase.stackValueCalculated = false;
    }
  }

  private void ResetStackedValues()
  {
    if (this.Area == null)
      return;
    this.Area.StackedValues = (Dictionary<object, StackingValues>) null;
  }

  private static List<double> GetStackingSeriesXValues(XyDataSeries chartseries)
  {
    return !(chartseries.ActualXAxis is CategoryAxis) || (chartseries.ActualXAxis as CategoryAxis).IsIndexed ? chartseries.GetXValues() : chartseries.GroupedXValuesIndexes;
  }

  internal override List<object> GetDataPoints(
    double startX,
    double endX,
    double startY,
    double endY,
    int minimum,
    int maximum,
    List<double> xValues,
    bool validateYValues)
  {
    List<object> dataPoints = new List<object>();
    if (xValues.Count != this.YRangeEndValues.Count)
      return (List<object>) null;
    for (int index = minimum; index <= maximum; ++index)
    {
      double xValue = xValues[index];
      if (validateYValues || startX <= xValue && xValue <= endX)
      {
        double yrangeEndValue = this.YRangeEndValues[index];
        if (startY <= yrangeEndValue && yrangeEndValue <= endY)
          dataPoints.Add(this.ActualData[index]);
      }
    }
    return dataPoints;
  }
}
