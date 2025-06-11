// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.TimeSpanAxisHelper
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal static class TimeSpanAxisHelper
{
  internal static void GenerateVisibleLabels(
    ChartAxisBase2D axis,
    object minimum,
    object maximum,
    object actualInterval)
  {
    double visibleInterval = axis.VisibleInterval;
    DoubleRange visibleRange = axis.VisibleRange;
    TimeSpanAxis timeSpanAxis = axis as TimeSpanAxis;
    double num1 = minimum != null && maximum != null && actualInterval != null || axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible || axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible && !axis.IsZoomed ? visibleRange.Start : visibleRange.Start - visibleRange.Start % visibleInterval;
    for (double num2 = double.NaN; num1 <= axis.VisibleRange.End && num1 != num2; num1 += visibleInterval)
    {
      if (axis.VisibleRange.Inside(num1))
        axis.VisibleLabels.Add(new ChartAxisLabel(num1, timeSpanAxis.GetActualLabelContent(num1), num1));
      if (axis.smallTicksRequired)
        axis.AddSmallTicksPoint(num1);
      num2 = num1;
    }
    if ((maximum == null || !visibleRange.End.Equals(((TimeSpan) maximum).TotalMilliseconds)) && axis.EdgeLabelsVisibilityMode != EdgeLabelsVisibilityMode.AlwaysVisible && (axis.EdgeLabelsVisibilityMode != EdgeLabelsVisibilityMode.Visible || axis.IsZoomed) || visibleRange.End.Equals(num1 - visibleInterval))
      return;
    axis.VisibleLabels.Add(new ChartAxisLabel(visibleRange.End, timeSpanAxis.GetActualLabelContent(visibleRange.End), visibleRange.End));
  }

  internal static void GenerateVisibleLabels3D(
    ChartAxis axis,
    object minimum,
    object maximum,
    object actualInterval)
  {
    double visibleInterval = axis.VisibleInterval;
    DoubleRange visibleRange = axis.VisibleRange;
    double num;
    for (num = minimum != null && maximum != null && actualInterval != null || axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible || axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible ? visibleRange.Start : visibleRange.Start - visibleRange.Start % visibleInterval; num <= axis.VisibleRange.End; num += visibleInterval)
    {
      if (axis.VisibleRange.Inside(num))
        axis.VisibleLabels.Add(new ChartAxisLabel(num, axis.GetLabelContent(num), num));
      if (axis.smallTicksRequired)
        axis.AddSmallTicksPoint(num);
    }
    if ((maximum == null || !visibleRange.End.Equals(((TimeSpan) maximum).TotalMilliseconds)) && axis.EdgeLabelsVisibilityMode != EdgeLabelsVisibilityMode.AlwaysVisible && axis.EdgeLabelsVisibilityMode != EdgeLabelsVisibilityMode.Visible || visibleRange.End.Equals(num - visibleInterval))
      return;
    axis.VisibleLabels.Add(new ChartAxisLabel(visibleRange.End, axis.GetLabelContent(visibleRange.End), visibleRange.End));
  }

  internal static void CalculateVisibleRange(
    ChartAxisBase2D axis,
    object interval,
    Size avalableSize)
  {
    if (axis.ZoomFactor >= 1.0 && axis.ZoomPosition <= 0.0)
      return;
    if (interval != null)
      axis.VisibleInterval = axis.EnableAutoIntervalOnZooming ? axis.CalculateNiceInterval(axis.VisibleRange, avalableSize) : axis.ActualInterval;
    else
      axis.VisibleInterval = axis.CalculateNiceInterval(axis.VisibleRange, avalableSize);
  }
}
