// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.RectangleCircularConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Tools.Converter;

public class RectangleCircularConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values.Length != 2 || !(values[0] is double val1) || !(values[1] is double val2))
      return DependencyProperty.UnsetValue;
    return val1 < double.Epsilon || val2 < double.Epsilon ? (object) 0.0 : (object) (Math.Min(val1, val2) / 2.0);
  }

  public object[] ConvertBack(
    object value,
    Type[] targetTypes,
    object parameter,
    CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}
