// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.Color2ChannelAConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Tools.Converter;

public class Color2ChannelAConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value is SolidColorBrush solidColorBrush ? (object) solidColorBrush.Color.A : (object) 0;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (!(value is string str))
      return parameter;
    try
    {
      if (ColorConverter.ConvertFromString(str) is Color color)
        return (object) new SolidColorBrush(color);
    }
    catch
    {
      return parameter;
    }
    return parameter;
  }
}
