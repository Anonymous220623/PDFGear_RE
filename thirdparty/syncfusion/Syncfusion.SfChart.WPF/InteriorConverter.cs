// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.InteriorConverter
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class InteriorConverter : IValueConverter
{
  private ChartSeriesBase series;

  public InteriorConverter(ChartSeriesBase series) => this.series = series;

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) ChartExtensionUtils.GetInterior(this.series, int.Parse(parameter.ToString()));
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value;
  }
}
