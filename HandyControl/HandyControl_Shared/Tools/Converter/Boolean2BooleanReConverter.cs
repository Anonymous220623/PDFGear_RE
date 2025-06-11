// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.Boolean2BooleanReConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Tools.Converter;

public class Boolean2BooleanReConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value is bool flag ? (object) !flag : value;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value is bool flag ? (object) !flag : value;
  }
}
