// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.Number2PercentageConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Expression.Drawing;
using HandyControl.Tools.Extension;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Tools.Converter;

public class Number2PercentageConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values == null || values.Length != 2)
      return (object) 0.0;
    object obj1 = values[0];
    object obj2 = values[1];
    if (obj1 == null || obj2 == null)
      return (object) 0.0;
    string input1 = values[0].ToString();
    string input2 = values[1].ToString();
    double num1 = input1.Value<double>();
    double num2 = input2.Value<double>();
    return MathHelper.IsVerySmall(num2) ? (object) 100.0 : (object) (num1 / num2 * 100.0);
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
