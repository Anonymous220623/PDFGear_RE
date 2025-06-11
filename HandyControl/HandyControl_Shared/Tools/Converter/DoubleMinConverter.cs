// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.DoubleMinConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Extension;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Tools.Converter;

public class DoubleMinConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (!(value is double num1))
      return (object) 0.0;
    if (!(parameter is string input))
      return (object) (num1 < 0.0 ? 0.0 : num1);
    double num2 = input.Value<double>();
    return (object) (num1 < num2 ? num2 : num1);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}
