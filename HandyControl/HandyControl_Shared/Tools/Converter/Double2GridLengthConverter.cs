// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.Double2GridLengthConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Tools.Converter;

public class Double2GridLengthConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (!(value is double num))
      return (object) new GridLength(0.0);
    return double.IsNaN(num) || double.IsInfinity(num) ? (object) new GridLength(1.0, GridUnitType.Star) : (object) new GridLength(num);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}
