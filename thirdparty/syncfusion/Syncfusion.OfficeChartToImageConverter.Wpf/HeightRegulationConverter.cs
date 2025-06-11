// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.HeightRegulationConverter
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using Syncfusion.UI.Xaml.Charts;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal class HeightRegulationConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    object obj = typeof (ChartAxisLabel).GetProperty("ChartAxis", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetProperty).GetValue(value, (object[]) null);
    return (object) (((FrameworkElement) typeof (ChartAxis).GetProperty("Area", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetProperty).GetValue(obj, (object[]) null)).Height / 6.666667);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value;
  }
}
