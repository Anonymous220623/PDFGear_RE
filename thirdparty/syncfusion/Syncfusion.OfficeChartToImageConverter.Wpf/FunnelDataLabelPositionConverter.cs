// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.FunnelDataLabelPositionConverter
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using Syncfusion.UI.Xaml.Charts;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal class FunnelDataLabelPositionConverter : IValueConverter
{
  internal LabelConvertor LabelConverterObject;

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    ChartAdornment chartAdornment = value as ChartAdornment;
    double high = (double) (chartAdornment.Item as ChartPoint).High;
    double low = (double) (chartAdornment.Item as ChartPoint).Low;
    double num = (high + low) / 2.0;
    RangeColumnSeries series = chartAdornment.Series as RangeColumnSeries;
    double point1 = series.Area.ValueToPoint(series.Area.Axes[0], num);
    double point2 = series.Area.ValueToPoint(series.Area.Axes[0], high);
    string str = this.LabelConverterObject.Convert(value, targetType, parameter, culture).ToString();
    this.LabelConverterObject.Index = this.LabelConverterObject.Index != 0 ? this.LabelConverterObject.Index - 1 : this.LabelConverterObject.CategoryNames.Length - 1;
    if (str == string.Empty)
      return (object) new Thickness(series.Area.ActualWidth, 0.0, 0.0, 0.0);
    TextBlock textBlock = new TextBlock() { Text = str };
    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    return (object) new Thickness(-(point2 - point1) - textBlock.DesiredSize.Width / 3.0, 0.0, -(-(point2 - point1) + textBlock.DesiredSize.Width / 3.0), 0.0);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value;
  }
}
