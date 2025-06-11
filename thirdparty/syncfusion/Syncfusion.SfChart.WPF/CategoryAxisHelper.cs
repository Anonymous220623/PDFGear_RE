// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.CategoryAxisHelper
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal class CategoryAxisHelper
{
  internal static DoubleRange ApplyRangePadding(
    ChartAxis axis,
    DoubleRange range,
    double interval,
    LabelPlacement labelPlacement)
  {
    return !(axis.Area.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series.ActualXAxis == axis)).Max((Func<ChartSeriesBase, double>) (filteredSeries => (double) filteredSeries.DataCount)) is PolarRadarSeriesBase) && labelPlacement == LabelPlacement.BetweenTicks ? new DoubleRange(-0.5, (double) (int) range.End + 0.5) : range;
  }

  internal static object GetLabelContent(ChartAxis axis, double position)
  {
    ChartSeriesBase actualSeries = axis.Area.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series.ActualXAxis == axis)).Max((Func<ChartSeriesBase, double>) (filteredSeries => (double) filteredSeries.DataCount));
    if (actualSeries == null)
      return (object) position;
    return axis.CustomLabels.Count > 0 || axis.GetLabelSource() != null ? axis.GetCustomLabelContent(position) ?? CategoryAxisHelper.GetLabelContent(axis, (int) Math.Round(position), actualSeries) ?? (object) string.Empty : CategoryAxisHelper.GetLabelContent(axis, (int) Math.Round(position), actualSeries) ?? (object) string.Empty;
  }

  internal static object GetLabelContent(ChartAxis axis, int pos, ChartSeriesBase actualSeries)
  {
    int num1;
    switch (actualSeries)
    {
      case WaterfallSeries _:
      case HistogramSeries _:
      case ErrorBarSeries _:
      case PolarRadarSeriesBase _:
        num1 = 1;
        break;
      default:
        num1 = axis is ChartAxisBase2D ? ((axis as CategoryAxis).IsIndexed ? 1 : 0) : ((axis as CategoryAxis3D).IsIndexed ? 1 : 0);
        break;
    }
    bool flag1 = num1 != 0;
    if (actualSeries == null)
      return (object) pos;
    object obj = (object) string.Empty;
    string labelContent = string.Empty;
    int num2 = 0;
    foreach (ChartSeriesBase chartSeriesBase in (Collection<ISupportAxes>) axis.RegisteredSeries)
    {
      XyzDataSeries3D xyzDataSeries3D = actualSeries as XyzDataSeries3D;
      bool flag2 = axis is ChartAxisBase3D chartAxisBase3D && chartAxisBase3D.IsZAxis;
      object empty = (object) string.Empty;
      IEnumerable enumerable;
      ChartValueType chartValueType;
      if (flag2)
      {
        enumerable = xyzDataSeries3D.ActualZValues;
        chartValueType = xyzDataSeries3D.ZAxisValueType;
      }
      else
      {
        enumerable = chartSeriesBase.ActualXValues;
        chartValueType = chartSeriesBase.XAxisValueType;
      }
      if (enumerable is List<double> doubleList && pos < doubleList.Count && pos >= 0)
      {
        switch (chartValueType)
        {
          case ChartValueType.Double:
          case ChartValueType.Logarithmic:
            empty = (object) doubleList[pos].ToString(axis.LabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
            break;
          case ChartValueType.DateTime:
            empty = (object) doubleList[pos].FromOADate().ToString(axis.LabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
            break;
          case ChartValueType.TimeSpan:
            empty = (object) TimeSpan.FromMilliseconds(doubleList[pos]).ToString(axis.LabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
            break;
        }
      }
      else
      {
        List<string> stringList1 = new List<string>();
        List<string> stringList2 = !(chartSeriesBase is PolarRadarSeriesBase) ? (!flag1 ? actualSeries.GroupedXValues : enumerable as List<string>) : actualSeries.XValues as List<string>;
        if (stringList2 != null && pos < stringList2.Count && pos >= 0)
        {
          if (!string.IsNullOrEmpty(axis.LabelFormat))
            obj = (object) string.Format(axis.LabelFormat, (object) stringList2[pos]);
          empty = (object) stringList2[pos];
        }
      }
      if (!string.IsNullOrEmpty(empty.ToString()) && !labelContent.Contains(empty.ToString()))
        labelContent = num2 <= 0 || string.IsNullOrEmpty(labelContent) ? empty.ToString() : $"{labelContent}, {empty}";
      if (flag2 || !flag1)
        return (object) labelContent;
      switch (chartSeriesBase)
      {
        case WaterfallSeries _:
        case HistogramSeries _:
        case PolarRadarSeriesBase _:
        case ErrorBarSeries _:
          return (object) labelContent;
        default:
          ++num2;
          continue;
      }
    }
    return (object) labelContent;
  }

  internal static void GenerateVisibleLabels(ChartAxis axis, LabelPlacement labelPlacement)
  {
    if (axis is ChartAxisBase3D && (axis as ChartAxisBase3D).IsManhattanAxis)
    {
      for (int index = 0; index < axis.RegisteredSeries.Count; ++index)
      {
        string label = (axis.RegisteredSeries[index] as CartesianSeries3D).Label;
        axis.VisibleLabels.Add(new ChartAxisLabel((double) index, string.IsNullOrEmpty(label) ? (object) ("Series " + (object) (index + 1)) : (object) label, (double) index));
      }
    }
    else
    {
      ChartSeriesBase actualSeries = axis.Area.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series.ActualXAxis == axis || (series is XyzDataSeries3D ? (series as XyzDataSeries3D).ActualZAxis : (ChartAxis) null) == axis)).Max((Func<ChartSeriesBase, double>) (filteredSeries => (double) filteredSeries.DataCount)) ?? (axis.Area is SfChart ? ((IEnumerable<ChartSeriesBase>) (axis.Area as SfChart).TechnicalIndicators.Where<ChartSeries>((Func<ChartSeries, bool>) (series => series.ActualXAxis == axis))).Max((Func<ChartSeriesBase, double>) (filteredSeries => (double) filteredSeries.DataCount)) : (ChartSeriesBase) null);
      if (actualSeries == null)
        return;
      DoubleRange visibleRange = axis.VisibleRange;
      double actualInterval = axis.ActualInterval;
      double visibleInterval = axis.VisibleInterval;
      double a = visibleRange.Start - visibleRange.Start % actualInterval;
      bool flag = actualSeries is PolarRadarSeriesBase;
      int num1 = actualSeries is WaterfallSeries || actualSeries is HistogramSeries || actualSeries is ErrorBarSeries || flag || (axis is ChartAxisBase2D ? (axis as CategoryAxis).IsIndexed : (axis as CategoryAxis3D).IsIndexed) ? actualSeries.DataCount : actualSeries.DistinctValuesIndexes.Count;
      for (; a <= visibleRange.End; a += visibleInterval)
      {
        int num2 = (int) Math.Round(a);
        if (visibleRange.Inside((double) num2) && num2 < num1 && num2 > -1)
        {
          object labelContent = CategoryAxisHelper.GetLabelContent(axis, num2, actualSeries);
          axis.VisibleLabels.Add(new ChartAxisLabel((double) num2, labelContent, (double) num2));
        }
      }
      double position = visibleRange.Start - visibleRange.Start % actualInterval;
      if (flag || labelPlacement == LabelPlacement.OnTicks)
        return;
      for (; position <= visibleRange.End; ++position)
      {
        if (position == 0.0 && axis.VisibleRange.Inside(-0.5))
          axis.m_smalltickPoints.Add(-0.5);
        CategoryAxisHelper.AddBetweenTicks(axis, position, 1.0);
      }
    }
  }

  internal static void AddBetweenTicks(ChartAxis axis, double position, double interval)
  {
    double num1 = interval / 2.0;
    double num2 = position + num1;
    double end = axis.VisibleRange.End;
    for (++position; num2 < position && num2 <= end; num2 += num1)
    {
      if (axis.VisibleRange.Inside(num2))
        axis.m_smalltickPoints.Add(num2);
    }
  }

  internal static double CalculateActualInterval(
    ChartAxis axis,
    DoubleRange range,
    Size availableSize,
    object interval)
  {
    return interval == null ? Math.Max(1.0, Math.Floor(range.Delta / axis.GetActualDesiredIntervalsCount(availableSize))) : ((double?) interval).Value;
  }

  internal static void GroupData(ChartVisibleSeriesCollection visibleSeries)
  {
    List<string> source1 = new List<string>();
    foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) visibleSeries)
    {
      switch (chartSeriesBase)
      {
        case WaterfallSeries _:
          return;
        case ErrorBarSeries _:
          return;
        case HistogramSeries _:
          return;
        case PolarRadarSeriesBase _:
          return;
        default:
          if (chartSeriesBase.ActualXValues is List<string>)
          {
            source1.AddRange((IEnumerable<string>) (chartSeriesBase.ActualXValues as List<string>));
            continue;
          }
          source1.AddRange((chartSeriesBase.ActualXValues as List<double>).Select<double, string>((Func<double, string>) (val => val.ToString())));
          continue;
      }
    }
    List<string> distinctXValues = source1.Distinct<string>().ToList<string>();
    foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) visibleSeries)
    {
      chartSeriesBase.GroupedXValuesIndexes = !(chartSeriesBase.ActualXValues is List<string>) ? (chartSeriesBase.ActualXValues as List<double>).Select<double, double>((Func<double, double>) (val => (double) distinctXValues.IndexOf(val.ToString()))).ToList<double>() : ((IEnumerable<string>) chartSeriesBase.ActualXValues).Select<string, double>((Func<string, double>) (val => (double) distinctXValues.IndexOf(val))).ToList<double>();
      chartSeriesBase.GroupedXValues = distinctXValues;
    }
    foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) visibleSeries)
    {
      ChartSeriesBase series = chartSeriesBase;
      series.DistinctValuesIndexes.Clear();
      Dictionary<int, List<double>>[] dictionaryArray = new Dictionary<int, List<double>>[series.ActualSeriesYValues.Length];
      List<double>[] yValues = new List<double>[series.ActualSeriesYValues.Length];
      series.GroupedSeriesYValues = new IList<double>[series.ActualSeriesYValues.Length];
      bool isRangeColumnSingleValue = series is RangeColumnSeries && !series.IsMultipleYPathRequired;
      for (int index = 0; index < series.ActualSeriesYValues.Length; ++index)
      {
        series.GroupedSeriesYValues[index] = (IList<double>) new List<double>();
        dictionaryArray[index] = new Dictionary<int, List<double>>();
        ((List<double>) series.GroupedSeriesYValues[index]).AddRange((IEnumerable<double>) series.ActualSeriesYValues[index]);
        if (isRangeColumnSingleValue)
          break;
      }
      List<string> source2 = series.ActualXValues is List<string> ? series.ActualXValues as List<string> : (series.ActualXValues as List<double>).Select<double, string>((Func<double, string>) (val => val.ToString())).ToList<string>();
      for (int i = 0; i < distinctXValues.Count; ++i)
      {
        int count = 0;
        List<int> indexes = new List<int>();
        for (int index = 0; index < series.ActualSeriesYValues.Length; ++index)
        {
          yValues[index] = new List<double>();
          if (isRangeColumnSingleValue)
            break;
        }
        source2.Select(xValues => new
        {
          xValues = xValues,
          index = count++
        }).Where(_param1 => distinctXValues[i] == _param1.xValues).Select(_param0 => _param0.index).Select<int, int>((Func<int, int>) (t1 =>
        {
          for (int index = 0; index < series.ActualSeriesYValues.Length; ++index)
          {
            yValues[index].Add(series.ActualSeriesYValues[index][count - 1]);
            if (index == 0)
              indexes.Add(count - 1);
            if (isRangeColumnSingleValue)
              break;
          }
          return 0;
        })).ToList<int>();
        for (int index = 0; index < series.ActualSeriesYValues.Length; ++index)
        {
          dictionaryArray[index].Add(i, yValues[index]);
          if (isRangeColumnSingleValue)
            break;
        }
        series.DistinctValuesIndexes.Add((double) i, indexes);
      }
      AggregateFunctions aggregateFunctions = series.ActualXAxis is CategoryAxis ? (series.ActualXAxis as CategoryAxis).AggregateFunctions : (series.ActualXAxis is CategoryAxis3D ? (series.ActualXAxis as CategoryAxis3D).AggregateFunctions : AggregateFunctions.None);
      if (aggregateFunctions != AggregateFunctions.None)
      {
        series.DistinctValuesIndexes.Clear();
        series.GroupedXValuesIndexes.Clear();
        for (int index = 0; index < series.ActualSeriesYValues.Length; ++index)
        {
          series.GroupedSeriesYValues[index].Clear();
          if (isRangeColumnSingleValue)
            break;
        }
        for (int key = 0; key < distinctXValues.Count; ++key)
        {
          series.GroupedXValuesIndexes.Add((double) key);
          for (int index = 0; index < series.ActualSeriesYValues.Length; ++index)
          {
            if (dictionaryArray[index][key].Count > 0)
            {
              switch (aggregateFunctions)
              {
                case AggregateFunctions.Average:
                  series.GroupedSeriesYValues[index].Add(dictionaryArray[index][key].Average());
                  break;
                case AggregateFunctions.Count:
                  series.GroupedSeriesYValues[index].Add((double) dictionaryArray[index][key].Count<double>());
                  break;
                case AggregateFunctions.Max:
                  series.GroupedSeriesYValues[index].Add(dictionaryArray[index][key].Max());
                  break;
                case AggregateFunctions.Min:
                  series.GroupedSeriesYValues[index].Add(dictionaryArray[index][key].Min());
                  break;
                case AggregateFunctions.Sum:
                  series.GroupedSeriesYValues[index].Add(dictionaryArray[index][key].Sum());
                  break;
              }
            }
            if (isRangeColumnSingleValue)
              break;
          }
          List<int> intList = new List<int>() { key };
          series.DistinctValuesIndexes.Add((double) key, intList);
        }
      }
    }
  }
}
