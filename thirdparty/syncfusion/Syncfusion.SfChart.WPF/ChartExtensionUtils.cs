// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartExtensionUtils
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public static class ChartExtensionUtils
{
  private static DateTime BaseDate = new DateTime(1899, 12, 30);

  internal static DateTime ValidateNonWorkingDate(
    this DateTime date,
    string days,
    bool isToBack,
    int nonWokingDays)
  {
    int num1 = 0;
    bool flag1 = false;
    bool flag2;
    do
    {
      flag2 = false;
      if (!days.Contains(date.DayOfWeek.ToString()))
      {
        int num2 = num1 + 1;
        flag1 = true;
        date = date.AddDays(isToBack ? (double) -nonWokingDays : (double) nonWokingDays);
        break;
      }
    }
    while (flag2);
    return date;
  }

  internal static DateTime ValidateNonWorkingHours(
    this DateTime date,
    double startTime,
    double endTime,
    bool isToBack)
  {
    double totalHours = date.TimeOfDay.TotalHours;
    if (isToBack)
    {
      if (totalHours < startTime)
      {
        date = date.AddDays(-1.0);
        date = new DateTime(date.Year, date.Month, date.Day, (int) (endTime - (startTime - totalHours)), date.Minute, date.Second);
      }
      else if (totalHours > endTime)
        date = date.AddHours(-(totalHours - endTime));
    }
    else if (totalHours < startTime)
      date = date.AddHours(startTime - totalHours);
    else if (totalHours > endTime)
      date = new DateTime(date.Year, date.Month, date.Day).AddHours(24.0).AddHours(startTime);
    return date;
  }

  internal static List<Vector3D> Get3DVector(this List<Point> points, double depth)
  {
    List<Vector3D> vector3DList = new List<Vector3D>();
    foreach (Point point in points)
      vector3DList.Add(new Vector3D(point, depth));
    return vector3DList;
  }

  public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
    this IEnumerable<TSource> source,
    Func<TSource, TKey> keySelector)
  {
    HashSet<TKey> seenKeys = new HashSet<TKey>();
    return source == null ? (IEnumerable<TSource>) null : source.Where<TSource>((Func<TSource, bool>) (element => seenKeys.Add(keySelector(element))));
  }

  public static double ToOADate(this DateTime time)
  {
    return time.Subtract(ChartExtensionUtils.BaseDate).TotalDays;
  }

  public static DateTime FromOADate(this double value)
  {
    return ChartExtensionUtils.BaseDate.AddDays(value);
  }

  public static DoubleRange Sum(this IEnumerable<DoubleRange> ranges)
  {
    DoubleRange empty = DoubleRange.Empty;
    IEnumerator<DoubleRange> enumerator = ranges.GetEnumerator();
    while (enumerator.MoveNext())
      empty += enumerator.Current;
    return empty;
  }

  public static ChartSeriesBase Max(
    this IEnumerable<ChartSeriesBase> source,
    Func<ChartSeriesBase, double> selector)
  {
    if (!(source is ChartSeriesBase[] chartSeriesBaseArray))
      chartSeriesBaseArray = source.ToArray<ChartSeriesBase>();
    ChartSeriesBase[] source1 = chartSeriesBaseArray;
    if (!((IEnumerable<ChartSeriesBase>) source1).Any<ChartSeriesBase>())
      return (ChartSeriesBase) null;
    ChartSeriesBase chartSeriesBase = source1[0];
    double num1 = selector(chartSeriesBase);
    for (int index = 0; index < ((IEnumerable<ChartSeriesBase>) source1).Count<ChartSeriesBase>(); ++index)
    {
      double num2 = selector(source1[index]);
      if (num2 > num1)
      {
        num1 = num2;
        chartSeriesBase = source1[index];
      }
    }
    return chartSeriesBase;
  }

  internal static int GetAdornmentIndex(object source)
  {
    FrameworkElement reference = source as FrameworkElement;
    int adornmentIndex = -1;
    if (reference.DataContext is ChartAdornmentContainer dataContext)
    {
      ChartAdornment adornment = dataContext.Adornment;
      ChartSeriesBase series = adornment?.Series;
      if (adornment != null && series != null && series.Adornments != null && series.Adornments.Count > 0)
        adornmentIndex = series.Adornments.IndexOf(adornment);
    }
    else
    {
      while (!(reference is ChartAdornmentContainer) && reference != null)
      {
        reference = VisualTreeHelper.GetParent((DependencyObject) reference) as FrameworkElement;
        if (reference is ContentControl && (reference as ContentControl).Tag is int)
          return (int) (reference as ContentControl).Tag;
        if (reference is ChartAdornmentContainer)
        {
          ChartAdornment adornment = (reference as ChartAdornmentContainer).Adornment;
          ChartSeriesBase series = adornment?.Series;
          if (adornment != null && series != null && series.Adornments != null && series.Adornments.Count > 0)
            adornmentIndex = series.Adornments.IndexOf(adornment);
        }
      }
    }
    return adornmentIndex;
  }

  public static void SetStrokeDashArray(UIElementsRecycler<Line> lineRecycler)
  {
    if (lineRecycler.Count <= 0)
      return;
    DoubleCollection strokeDashArray = lineRecycler[0].StrokeDashArray;
    if (strokeDashArray == null || strokeDashArray.Count <= 0)
      return;
    foreach (Line line in lineRecycler)
    {
      DoubleCollection doubleCollection = new DoubleCollection();
      foreach (double num in strokeDashArray)
        doubleCollection.Add(num);
      line.StrokeDashArray = doubleCollection;
    }
  }

  internal static bool IsDraggable(ChartSeriesBase chartSeries)
  {
    return chartSeries is RangeSegmentDraggingBase && (chartSeries as RangeSegmentDraggingBase).EnableSegmentDragging || chartSeries is XySegmentDraggingBase && (chartSeries as XySegmentDraggingBase).EnableSegmentDragging || chartSeries is XySeriesDraggingBase && (chartSeries as XySeriesDraggingBase).EnableSeriesDragging;
  }

  internal static int BinarySearch(List<double> xValues, double touchValue, int min, int max)
  {
    int num1 = 0;
    double num2 = double.MaxValue;
    while (min <= max)
    {
      int index = (min + max) / 2;
      double xValue = xValues[index];
      double num3 = Math.Abs(touchValue - xValue);
      if (num3 < num2)
      {
        num2 = num3;
        num1 = index;
      }
      if (touchValue == xValue)
        return index;
      if (touchValue < xValues[index])
        max = index - 1;
      else
        min = index + 1;
    }
    return num1;
  }

  internal static Brush GetInterior(ChartSeriesBase series, int segmentIndex)
  {
    ChartSeriesBase series1 = series;
    if (series1 != null)
    {
      if (series1.Interior != null)
        return series1.Interior;
      if (series1.SegmentColorPath != null && series1.ColorValues.Count > 0)
      {
        if (segmentIndex != -1 && segmentIndex < series1.ActualData.Count)
        {
          if (series1.ColorValues[segmentIndex] != null)
            return series1.ColorValues[segmentIndex];
          if (series1.Palette != ChartColorPalette.None && series1.ColorValues[segmentIndex] == null)
          {
            series1.ColorValues[segmentIndex] = series1.ColorModel.GetBrush(segmentIndex);
            return series1.ColorModel.GetBrush(segmentIndex);
          }
          int seriesIndex = series1.ActualArea.GetSeriesIndex(series1);
          series1.ColorValues[segmentIndex] = series1.ActualArea.ColorModel.GetBrush(seriesIndex);
          return series1.ActualArea.ColorModel.GetBrush(seriesIndex);
        }
      }
      else if (series1.Palette != ChartColorPalette.None)
      {
        if (segmentIndex != -1 && series1.ColorModel != null)
          return series1.ColorModel.GetBrush(segmentIndex);
      }
      else if (series1.ActualArea != null && series1.ActualArea.Palette != ChartColorPalette.None && series1.ActualArea.ColorModel != null)
      {
        int seriesIndex = series1.ActualArea.GetSeriesIndex(series1);
        SfChart actualArea = series1.ActualArea as SfChart;
        if (seriesIndex >= 0)
          return series1.ActualArea.ColorModel.GetBrush(seriesIndex);
        if (actualArea != null && actualArea.TechnicalIndicators != null && actualArea.TechnicalIndicators.Count > 0)
        {
          int colorIndex = actualArea.TechnicalIndicators.IndexOf(series1 as ChartSeries);
          return series1.ActualArea.ColorModel.GetBrush(colorIndex);
        }
      }
    }
    return (Brush) new SolidColorBrush(Colors.Transparent);
  }

  internal static Rect GetAxisArrangeRect(
    Point mousePoint,
    ChartAxis axis,
    out bool isPointInsideRect)
  {
    Rect axisArrangeRect = new Rect();
    double left = axis.ArrangeRect.Left;
    double top = axis.ArrangeRect.Top;
    foreach (ChartAxis associatedAx in axis.AssociatedAxes)
    {
      if (axis.Orientation == Orientation.Horizontal)
      {
        top = associatedAx.ArrangeRect.Top;
        axisArrangeRect = new Rect(left, top, axis.ArrangeRect.Width, associatedAx.ArrangeRect.Height);
      }
      else
      {
        left = associatedAx.ArrangeRect.Left;
        axisArrangeRect = new Rect(left, top, associatedAx.ArrangeRect.Width, axis.ArrangeRect.Height);
      }
      if (axisArrangeRect.Contains(mousePoint))
      {
        isPointInsideRect = true;
        return axisArrangeRect;
      }
    }
    isPointInsideRect = false;
    return axisArrangeRect;
  }
}
