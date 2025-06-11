// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.LegendMarginConverter
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class LegendMarginConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (Visibility) value == Visibility.Visible ? (object) new Thickness().GetThickness(-20.0, 0.0, 0.0, 0.0) : (object) new Thickness().GetThickness(0.0, 0.0, 0.0, 0.0);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value;
  }
}
