// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.RangeAxisBaseHelper
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal static class RangeAxisBaseHelper
{
  internal static void AddSmallTicksPoint(
    ChartAxis axis,
    double position,
    double interval,
    int smallTicksPerInterval)
  {
    int num1 = smallTicksPerInterval + 1;
    double num2 = interval / (double) num1;
    double num3 = position + interval;
    double num4 = position;
    double end = axis.VisibleRange.End;
    double start = axis.VisibleRange.Start;
    DoubleRange visibleRange = axis.VisibleRange;
    if (axis is NumericalAxis numericalAxis && numericalAxis.IsSecondaryAxis && numericalAxis.BreakRangesInfo.Count > 0)
    {
      List<DoubleRange> axisRanges = numericalAxis.AxisRanges;
      for (int index = 0; index < axisRanges.Count; ++index)
      {
        if (axisRanges[index].Inside(position))
        {
          end = axisRanges[index].End;
          visibleRange = axisRanges[index];
          break;
        }
      }
    }
    int num5 = 0;
    if (axis.m_smalltickPoints.Count == 0 && num4 > start)
    {
      for (double num6 = position; num6 > start && num6 < end && num5 < num1; num6 -= num2)
      {
        if (num6 != position)
          axis.m_smalltickPoints.Add(num6);
        ++num5;
      }
    }
    int num7 = 0;
    while (num4 < num3 && num4 < end && num7 < num1)
    {
      if (num4 != position && visibleRange.Inside(num4))
        axis.m_smalltickPoints.Add(num4);
      ++num7;
      num4 += num2;
      if (Math.Round(num4 * 100000000.0) / 100000000.0 >= num3)
        num4 = num3;
    }
  }

  internal static void GenerateVisibleLabels(ChartAxis axis, double smallTicksPerInterval)
  {
    double visibleInterval = axis.VisibleInterval;
    for (double num = axis.VisibleRange.Start - axis.VisibleRange.Start % visibleInterval; num <= axis.VisibleRange.End; num += visibleInterval)
    {
      if (axis.VisibleRange.Inside(num))
        axis.VisibleLabels.Add(new ChartAxisLabel(num, axis.GetActualLabelContent(num), num));
      if (axis.smallTicksRequired)
        axis.AddSmallTicksPoint(num);
    }
  }
}
