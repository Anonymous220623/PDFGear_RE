// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.NumericalAxisHelper
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal static class NumericalAxisHelper
{
  internal static void GenerateVisibleLabels(
    ChartAxisBase2D axis,
    object minimum,
    object maximum,
    object actualInterval,
    double smallTicksPerInterval)
  {
    DoubleRange visibleRange = axis.VisibleRange;
    double visibleInterval = axis.VisibleInterval;
    double num1 = minimum != null && maximum != null && actualInterval != null && !axis.IsZoomed || axis.DesiredIntervalsCount.HasValue || axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible || axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible && !axis.IsZoomed ? visibleRange.Start : visibleRange.Start - visibleRange.Start % visibleInterval;
    for (double num2 = double.NaN; num1 <= visibleRange.End && num1 != num2; num1 += visibleInterval)
    {
      if (visibleRange.Inside(num1))
        axis.VisibleLabels.Add(new ChartAxisLabel(num1, axis.GetActualLabelContent(num1), num1));
      if (axis.smallTicksRequired)
        axis.AddSmallTicksPoint(num1);
      num2 = num1;
    }
    if ((maximum == null || !visibleRange.End.Equals(maximum)) && axis.EdgeLabelsVisibilityMode != EdgeLabelsVisibilityMode.AlwaysVisible && (axis.EdgeLabelsVisibilityMode != EdgeLabelsVisibilityMode.Visible || axis.IsZoomed) || visibleRange.End.Equals(num1 - visibleInterval))
      return;
    axis.VisibleLabels.Add(new ChartAxisLabel(visibleRange.End, axis.GetActualLabelContent(visibleRange.End), visibleRange.End));
  }

  internal static void GenerateVisibleLabels3D(
    ChartAxis axis,
    object minimum,
    object maximum,
    object actualInterval,
    double smallTicksPerInterval)
  {
    DoubleRange visibleRange = axis.VisibleRange;
    double visibleInterval = axis.VisibleInterval;
    double num;
    for (num = minimum != null && maximum != null && actualInterval != null || axis.DesiredIntervalsCount.HasValue || axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible || axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible ? visibleRange.Start : visibleRange.Start - visibleRange.Start % visibleInterval; num <= visibleRange.End; num += visibleInterval)
    {
      if (visibleRange.Inside(num))
        axis.VisibleLabels.Add(new ChartAxisLabel(num, axis.GetActualLabelContent(num), num));
      if (axis.smallTicksRequired)
        axis.AddSmallTicksPoint(num);
    }
    if ((maximum == null || !visibleRange.End.Equals(maximum)) && axis.EdgeLabelsVisibilityMode != EdgeLabelsVisibilityMode.AlwaysVisible && axis.EdgeLabelsVisibilityMode != EdgeLabelsVisibilityMode.Visible || visibleRange.End.Equals(num - visibleInterval))
      return;
    axis.VisibleLabels.Add(new ChartAxisLabel(visibleRange.End, axis.GetActualLabelContent(visibleRange.End), visibleRange.End));
  }

  internal static void OnMinMaxChanged(ChartAxis axis, object maximum, object minimum)
  {
    if (minimum != null || maximum != null)
    {
      double start = minimum == null ? double.NegativeInfinity : ((double?) minimum).Value;
      double end = maximum == null ? double.PositiveInfinity : ((double?) maximum).Value;
      axis.ActualRange = new DoubleRange(start, end);
    }
    else
      axis.ActualRange = DoubleRange.Empty;
    if (axis.Area == null)
      return;
    axis.Area.ScheduleUpdate();
  }

  internal static DoubleRange ApplyRangePadding(
    ChartAxis axis,
    DoubleRange range,
    double interval,
    NumericalPadding rangePadding)
  {
    double start1 = range.Start;
    double end1 = range.End;
    if (rangePadding == NumericalPadding.Normal)
    {
      double num1 = start1;
      double start2;
      if (start1 < 0.0)
      {
        num1 = 0.0;
        start2 = start1 + start1 / 20.0;
        double num2 = interval + start2 % interval;
        if (0.365 * interval >= num2)
          start2 -= interval;
        if (start2 % interval < 0.0)
          start2 = start2 - interval - start2 % interval;
      }
      else
      {
        start2 = start1 < 5.0 / 6.0 * end1 ? 0.0 : start1 - (end1 - start1) / 2.0;
        if (start2 % interval > 0.0)
          start2 -= start2 % interval;
      }
      double end2 = end1 + (end1 - num1) / 20.0;
      double num3 = interval - end2 % interval;
      if (0.365 * interval >= num3)
        end2 += interval;
      if (end2 % interval > 0.0)
        end2 = end2 + interval - end2 % interval;
      range = new DoubleRange(start2, end2);
      if (start2 == 0.0)
      {
        axis.ActualInterval = axis.CalculateActualInterval(range, axis.AvailableSize);
        return new DoubleRange(0.0, Math.Ceiling(end2 / axis.ActualInterval) * axis.ActualInterval);
      }
    }
    else if (rangePadding != NumericalPadding.Auto && rangePadding != NumericalPadding.None && rangePadding != NumericalPadding.Normal)
    {
      double start3 = Math.Floor(start1 / interval) * interval;
      double end3 = Math.Ceiling(end1 / interval) * interval;
      double start4 = start3 - interval;
      double end4 = end3 + interval;
      switch (rangePadding)
      {
        case NumericalPadding.Round:
          return new DoubleRange(start3, end3);
        case NumericalPadding.RoundStart:
          return new DoubleRange(start3, end1);
        case NumericalPadding.RoundEnd:
          return new DoubleRange(start1, end3);
        case NumericalPadding.PrependInterval:
          return new DoubleRange(start4, end1);
        case NumericalPadding.AppendInterval:
          return new DoubleRange(start1, end4);
        default:
          return new DoubleRange(start4, end4);
      }
    }
    return range;
  }

  internal static void GenerateScaleBreakVisibleLabels(
    NumericalAxis axis,
    object actualInterval,
    double smallTicksPerInterval)
  {
    List<DoubleRange> axisRanges = axis?.AxisRanges;
    if (axisRanges == null || axisRanges.Count == 0)
    {
      NumericalAxisHelper.GenerateVisibleLabels((ChartAxisBase2D) axis, (object) axis.Minimum, (object) axis.Maximum, (object) axis.Interval, (double) axis.SmallTicksPerInterval);
    }
    else
    {
      for (int index = 0; index < axisRanges.Count; ++index)
      {
        double start = axisRanges[index].Start;
        double actualInterval1 = axis.CalculateActualInterval(axisRanges[index], axis.Orientation.Equals((object) Orientation.Vertical) ? new Size(axis.AvailableSize.Width, axis.AxisLength[index]) : new Size(axis.AxisLength[index], axis.AvailableSize.Height));
        if (axisRanges[index].Inside(start))
        {
          for (; start <= axisRanges[index].End; start += actualInterval1)
          {
            axis.VisibleLabels.Add(new ChartAxisLabel(start, axis.GetActualLabelContent(start), start));
            if (axisRanges[index].Delta != 0.0)
            {
              if (axis.smallTicksRequired)
                axis.AddSmallTicksPoint(start, actualInterval1);
            }
            else
              break;
          }
          if (!axisRanges[index].End.Equals(start - actualInterval1))
          {
            int count = axis.VisibleLabels.Count;
            ChartAxisLabel chartAxisLabel = new ChartAxisLabel();
            chartAxisLabel.Position = axisRanges[index].End;
            chartAxisLabel.LabelContent = axis.GetActualLabelContent(axisRanges[index].End);
            if (Convert.ToDouble(axis.VisibleLabels[count - 1].GetContent()) != axisRanges[index].End)
              axis.VisibleLabels.Add(chartAxisLabel);
          }
        }
      }
    }
  }

  internal static void CalculateVisibleRange(
    ChartAxisBase2D axis,
    Size avalableSize,
    object interval)
  {
    if (axis.ZoomFactor >= 1.0 && axis.ZoomPosition <= 0.0)
      return;
    if (interval == null || ((double?) interval).Value == 0.0)
    {
      axis.VisibleInterval = axis.CalculateNiceInterval(axis.VisibleRange, avalableSize);
    }
    else
    {
      if (interval == null)
        return;
      double num = ((double?) interval).Value;
      axis.VisibleInterval = axis.EnableAutoIntervalOnZooming ? axis.CalculateNiceInterval(axis.VisibleRange, avalableSize) : num;
    }
  }
}
