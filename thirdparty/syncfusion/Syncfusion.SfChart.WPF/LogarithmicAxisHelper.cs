// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.LogarithmicAxisHelper
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal static class LogarithmicAxisHelper
{
  internal static double CalculateNiceInterval(
    ChartAxis axis,
    DoubleRange actualRange,
    Size availableSize)
  {
    double delta = actualRange.Delta;
    double desiredIntervalsCount = axis.GetActualDesiredIntervalsCount(availableSize);
    double d = delta;
    double num1 = Math.Pow(10.0, Math.Floor(Math.Log10(d)));
    foreach (int cIntervalDiv in ChartAxis.c_intervalDivs)
    {
      double num2 = num1 * (double) cIntervalDiv;
      if (desiredIntervalsCount >= delta / num2)
        d = num2;
      else
        break;
    }
    return d < 1.0 ? 1.0 : d;
  }

  internal static void GenerateVisibleLabels(
    ChartAxisBase2D axis,
    object minimum,
    object maximum,
    object actualInterval,
    double logBase)
  {
    double visibleInterval = axis.VisibleInterval;
    LogarithmicAxis logarithmicAxis = axis as LogarithmicAxis;
    double num;
    for (num = minimum != null && maximum != null && actualInterval != null || axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible || axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible && !axis.IsZoomed ? axis.VisibleRange.Start : axis.VisibleRange.Start - axis.VisibleRange.Start % axis.ActualInterval; num <= axis.VisibleRange.End; num += visibleInterval)
    {
      if (axis.VisibleRange.Inside(num))
        axis.VisibleLabels.Add(new ChartAxisLabel(num, logarithmicAxis.GetActualLabelContent(Math.Pow(logBase, num)), num));
      if (axis.smallTicksRequired)
        axis.AddSmallTicksPoint(num, logarithmicAxis.LogarithmicBase);
    }
    if ((maximum == null || !axis.VisibleRange.End.Equals(maximum)) && axis.EdgeLabelsVisibilityMode != EdgeLabelsVisibilityMode.AlwaysVisible && (axis.EdgeLabelsVisibilityMode != EdgeLabelsVisibilityMode.Visible || axis.IsZoomed) || axis.VisibleRange.End.Equals(num - visibleInterval))
      return;
    axis.VisibleLabels.Add(new ChartAxisLabel(axis.VisibleRange.End, logarithmicAxis.GetActualLabelContent(Math.Pow(logBase, axis.VisibleRange.End)), axis.VisibleRange.End));
  }

  internal static void GenerateVisibleLabels3D(
    ChartAxis axis,
    object minimum,
    object maximum,
    object actualInterval,
    double logBase)
  {
    double visibleInterval = axis.VisibleInterval;
    double num;
    for (num = minimum != null && maximum != null && actualInterval != null || axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible || axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible ? axis.VisibleRange.Start : axis.VisibleRange.Start - axis.VisibleRange.Start % axis.ActualInterval; num <= axis.VisibleRange.End; num += visibleInterval)
    {
      if (axis.VisibleRange.Inside(num))
        axis.VisibleLabels.Add(new ChartAxisLabel(num, axis.GetLabelContent(Math.Pow(logBase, num)), num));
      if (axis.smallTicksRequired)
        axis.AddSmallTicksPoint(num, (axis as LogarithmicAxis3D).LogarithmicBase);
    }
    if ((maximum == null || !axis.VisibleRange.End.Equals(maximum)) && axis.EdgeLabelsVisibilityMode != EdgeLabelsVisibilityMode.AlwaysVisible && axis.EdgeLabelsVisibilityMode != EdgeLabelsVisibilityMode.Visible || axis.VisibleRange.End.Equals(num - visibleInterval))
      return;
    axis.VisibleLabels.Add(new ChartAxisLabel(axis.VisibleRange.End, axis.GetLabelContent(Math.Pow(logBase, axis.VisibleRange.End)), axis.VisibleRange.End));
  }

  internal static void AddSmallTicksPoint(
    ChartAxis axis,
    double position,
    double logarithmicBase,
    double smallTicksPerInterval)
  {
    double num1 = Math.Pow(logarithmicBase, position - axis.VisibleInterval);
    double num2 = Math.Pow(logarithmicBase, position);
    double num3 = (num2 - num1) / (smallTicksPerInterval + 1.0);
    double a = num1 + num3;
    double num4 = Math.Log(a, logarithmicBase);
    while (a < num2)
    {
      if (axis.VisibleRange.Inside(num4))
        axis.m_smalltickPoints.Add(num4);
      a += num3;
      num4 = Math.Log(a, logarithmicBase);
    }
  }

  internal static void OnMinMaxChanged(
    ChartAxis axis,
    object minimum,
    object maximum,
    double logarithmicBase)
  {
    if (minimum != null || maximum != null)
    {
      double num1;
      if (minimum != null)
      {
        double? nullable = (double?) minimum;
        num1 = ((nullable.GetValueOrDefault() <= 0.0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0 ? (double?) minimum : new double?(1.0)).Value;
      }
      else
        num1 = double.NegativeInfinity;
      double num2 = num1;
      double num3;
      if (maximum != null)
      {
        double? nullable = (double?) maximum;
        num3 = ((nullable.GetValueOrDefault() <= 0.0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0 ? (double?) maximum : new double?(1.0)).Value;
      }
      else
        num3 = double.PositiveInfinity;
      double num4 = num3;
      axis.ActualRange = logarithmicBase == 10.0 ? new DoubleRange(Math.Log10(num2), Math.Log10(num4)) : new DoubleRange(Math.Log(num2, logarithmicBase), Math.Log(num4, logarithmicBase));
    }
    if (axis.Area == null)
      return;
    axis.Area.ScheduleUpdate();
  }

  internal static DoubleRange CalculateActualRange(
    ChartAxis axis,
    DoubleRange range,
    double logarithmicBase)
  {
    if (range.IsEmpty)
      return range;
    double d1 = Math.Log(range.Start, logarithmicBase);
    double x1 = double.IsInfinity(d1) || double.IsNaN(d1) ? range.Start : d1;
    double d2 = Math.Log(range.End, logarithmicBase);
    double x2 = double.IsInfinity(d2) || double.IsNaN(d2) ? logarithmicBase : d2;
    range = new DoubleRange(ChartMath.Round(x1, 1.0, false), ChartMath.Round(x2, 1.0, true));
    return range;
  }

  internal static void CalculateVisibleRange(
    ChartAxisBase2D axis,
    Size avalableSize,
    object interval)
  {
    if (axis.ZoomFactor >= 1.0 && axis.ZoomPosition <= 0.0)
      return;
    if (interval != null)
      axis.VisibleInterval = axis.EnableAutoIntervalOnZooming ? axis.CalculateNiceInterval(axis.VisibleRange, avalableSize) : axis.ActualInterval;
    else
      axis.VisibleInterval = axis.CalculateNiceInterval(axis.VisibleRange, avalableSize);
  }
}
