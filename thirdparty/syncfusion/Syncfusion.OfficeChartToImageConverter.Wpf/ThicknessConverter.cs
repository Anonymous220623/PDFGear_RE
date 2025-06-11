// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.ThicknessConverter
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal class ThicknessConverter : IValueConverter
{
  internal Dictionary<int, Thickness> LabelThicknesses;
  internal Thickness DefaultThickness;
  internal Dictionary<int, double> BorderThicknesses;
  internal bool IsSeries;
  private int m_index = -1;

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (this.IsSeries)
    {
      if (this.BorderThicknesses != null && this.BorderThicknesses.Count > 0)
      {
        ChartSegment chartSegment = value as ChartSegment;
        ChartAdornment chartAdornment = value as ChartAdornment;
        ChartPoint chartPoint = chartSegment.Item as ChartPoint;
        int key = chartAdornment == null ? (chartSegment.Series.ItemsSource as ObservableCollection<ChartPoint>).IndexOf(chartPoint) : (chartAdornment.Series.ItemsSource as ObservableCollection<ChartPoint>).IndexOf(chartPoint);
        if (this.BorderThicknesses.ContainsKey(key))
          return (object) this.BorderThicknesses[key];
      }
      return (object) this.DefaultThickness.Left;
    }
    ++this.m_index;
    return (object) (this.LabelThicknesses.ContainsKey(this.m_index) ? this.LabelThicknesses[this.m_index] : this.DefaultThickness);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value;
  }
}
