// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SegmentSelectionBrushConverter
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SegmentSelectionBrushConverter : IValueConverter
{
  private ChartSeriesBase series;

  public SegmentSelectionBrushConverter(ChartSeriesBase series) => this.series = series;

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value != null)
      return value;
    if (this.series != null)
    {
      if (this.series.Interior != null)
        return (object) this.series.Interior;
      if (this.series.Palette != ChartColorPalette.None)
      {
        int colorIndex = int.Parse(parameter.ToString());
        if (colorIndex != -1 && this.series.ColorModel != null)
          return (object) this.series.ColorModel.GetBrush(colorIndex);
      }
      else if (this.series.ActualArea != null && this.series.ActualArea.Palette != ChartColorPalette.None && this.series.ActualArea.ColorModel != null)
      {
        int seriesIndex = this.series.ActualArea.GetSeriesIndex(this.series);
        if (seriesIndex >= 0)
          return (object) this.series.ActualArea.ColorModel.GetBrush(seriesIndex);
        if (this.series.ActualArea is SfChart)
          return (object) this.series.ActualArea.ColorModel.GetBrush((this.series.ActualArea as SfChart).TechnicalIndicators.IndexOf(this.series as ChartSeries));
      }
    }
    return (object) null;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value;
  }
}
