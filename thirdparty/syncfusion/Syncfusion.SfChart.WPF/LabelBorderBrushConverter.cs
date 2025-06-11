// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.LabelBorderBrushConverter
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class LabelBorderBrushConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (!(value is ChartAdornment chartAdornment))
      return value;
    ChartSeriesBase series = chartAdornment.Series;
    ChartBase actualArea = series.ActualArea;
    ISegmentSelectable segmentSelectable = series as ISegmentSelectable;
    if (actualArea.SelectedSeriesCollection.Contains(series) && actualArea.GetSeriesSelectionBrush(series) != null && series is ChartSeries && series.adornmentInfo.HighlightOnSelection && actualArea.GetEnableSeriesSelection())
      return (object) actualArea.GetSeriesSelectionBrush(series);
    if (series.SelectedSegmentsIndexes.Contains(series.ActualData.IndexOf(chartAdornment.Item)) && segmentSelectable != null && series.adornmentInfo.HighlightOnSelection && segmentSelectable.SegmentSelectionBrush != null)
      return (object) segmentSelectable.SegmentSelectionBrush;
    return chartAdornment.Series.adornmentInfo.IsAdornmentLabelCreatedEventHooked && chartAdornment.CustomAdornmentLabel != null && chartAdornment.CustomAdornmentLabel.Background != null ? (object) chartAdornment.CustomAdornmentLabel.BorderBrush : (object) chartAdornment.BorderBrush;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value;
  }
}
