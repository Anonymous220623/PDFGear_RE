// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelChartToImageConverter.ColorConverter
// Assembly: Syncfusion.ExcelChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 8A92A829-7139-4C93-8632-144655877EB3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelChartToImageConverter.Wpf.dll

using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.ExcelChartToImageConverter;

internal class ColorConverter : IValueConverter
{
  internal Dictionary<char, Brush> Brushes;
  internal Dictionary<int, Brush> LabelBrushes;
  internal Brush DefaultBrush;
  private int m_index = -1;

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    ChartSegment chartSegment = value as ChartSegment;
    ChartAdornment chartAdornment = value as ChartAdornment;
    bool flag1 = parameter is bool flag2 && flag2;
    if (chartSegment != null && flag1)
    {
      ChartPoint chartPoint = chartSegment.Item as ChartPoint;
      int key = chartAdornment == null ? (chartSegment.Series.ItemsSource as ObservableCollection<ChartPoint>).IndexOf(chartPoint) : (chartAdornment.Series.ItemsSource as ObservableCollection<ChartPoint>).IndexOf(chartPoint);
      if (this.LabelBrushes.ContainsKey(key))
        return (object) this.LabelBrushes[key];
      return this.LabelBrushes.ContainsKey(-2) && chartPoint.Value < 0.0 ? (object) this.LabelBrushes[-2] : (object) this.LabelBrushes[-1];
    }
    ++this.m_index;
    if (this.Brushes != null)
    {
      double num = 0.0;
      if (value is ChartAxisLabel)
        num = System.Convert.ToDouble((value as ChartAxisLabel).Position);
      else if (value != null)
      {
        double result = 0.0;
        if (chartAdornment != null)
          num = (chartAdornment.Item as ChartPoint).Value;
        else if (value is string)
        {
          if (double.TryParse(value.ToString(), out result))
            num = result;
        }
        else if (value is IConvertible)
          num = System.Convert.ToDouble(value);
      }
      if (num < 0.0 && this.Brushes.ContainsKey('-'))
        return (object) this.Brushes['-'];
      if (num == 0.0 && this.Brushes.ContainsKey('0'))
        return (object) this.Brushes['0'];
      if (num > 0.0 && this.Brushes.ContainsKey('+'))
        return (object) this.Brushes['+'];
      return this.LabelBrushes != null && this.LabelBrushes.ContainsKey(this.m_index) ? (object) this.LabelBrushes[this.m_index] : (object) this.Brushes['d'];
    }
    return this.LabelBrushes.ContainsKey(this.m_index) ? (object) this.LabelBrushes[this.m_index] : (object) this.DefaultBrush;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value;
  }
}
