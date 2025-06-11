// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.LabelBackgroundConverter
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class LabelBackgroundConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (!(value is ChartAdornment adornment))
      return value;
    if (adornment.CanHideLabel)
      return (object) new SolidColorBrush(Colors.Transparent);
    if (adornment.Series.ActualArea.SelectedSeriesCollection.Contains(adornment.Series) && adornment.Series.ActualArea.GetSeriesSelectionBrush(adornment.Series) != null && adornment.Series.adornmentInfo.HighlightOnSelection && adornment.Series is ChartSeries && adornment.Series.ActualArea.GetEnableSeriesSelection() && (adornment.Series.adornmentInfo.UseSeriesPalette || adornment.Background != null))
      return (object) adornment.Series.ActualArea.GetSeriesSelectionBrush(adornment.Series);
    if (LabelBackgroundConverter.IsAdornmentSelected(adornment))
      return (object) (adornment.Series as ISegmentSelectable).SegmentSelectionBrush;
    if (adornment.Series.adornmentInfo.IsAdornmentLabelCreatedEventHooked && adornment.CustomAdornmentLabel != null && adornment.CustomAdornmentLabel.Background != null)
      return (object) adornment.CustomAdornmentLabel.Background;
    return adornment.Series.adornmentInfo.UseSeriesPalette && adornment.Background == null ? (object) adornment.Interior : (object) adornment.Background;
  }

  private static bool IsAdornmentSelected(ChartAdornment adornment)
  {
    if (!adornment.Series.SelectedSegmentsIndexes.Contains(adornment.Series.ActualData.IndexOf(adornment.Item)) || !(adornment.Series is ISegmentSelectable) || !adornment.Series.adornmentInfo.HighlightOnSelection || (adornment.Series as ISegmentSelectable).SegmentSelectionBrush == null)
      return false;
    return adornment.Series.adornmentInfo.UseSeriesPalette || adornment.Background != null;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value;
  }
}
