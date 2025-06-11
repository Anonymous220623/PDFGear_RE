// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.ThicknessSplitConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Tools.Converter;

public class ThicknessSplitConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (!(value is Thickness thickness1) || !(parameter is string str))
      return value;
    char[] chArray = new char[1]{ ',' };
    string[] strArray = str.Split(chArray);
    if (strArray.Length != 4)
      return (object) thickness1;
    Thickness thickness2 = new Thickness(thickness1.Left, thickness1.Top, thickness1.Right, thickness1.Bottom);
    double result1;
    if (double.TryParse(strArray[0], out result1))
      thickness2.Left = result1 * thickness1.Left;
    double result2;
    if (double.TryParse(strArray[1], out result2))
      thickness2.Top = result2 * thickness1.Top;
    double result3;
    if (double.TryParse(strArray[2], out result3))
      thickness2.Right = result3 * thickness1.Right;
    double result4;
    if (double.TryParse(strArray[3], out result4))
      thickness2.Bottom = result4 * thickness1.Bottom;
    return (object) thickness2;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}
