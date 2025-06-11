// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.Int2StringConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Tools.Converter;

public class Int2StringConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    int result;
    if (value == null || !int.TryParse(value.ToString(), out result))
      return value;
    if (!(parameter is string str))
      return (object) result;
    char[] chArray = new char[1]{ ';' };
    string[] strArray = str.Split(chArray);
    return strArray.Length > result ? (object) strArray[result] : (object) result;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}
