// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.String2VisibilityConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Tools.Converter;

public class String2VisibilityConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) (Visibility) (string.IsNullOrEmpty((string) value) ? 2 : 0);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) (bool) (value == null ? 0 : ((Visibility) value == Visibility.Collapsed ? 1 : 0));
  }
}
