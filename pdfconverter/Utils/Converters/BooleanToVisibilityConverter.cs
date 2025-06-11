// Decompiled with JetBrains decompiler
// Type: pdfconverter.Utils.Converters.BooleanToVisibilityConverter
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace pdfconverter.Utils.Converters;

public class BooleanToVisibilityConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) (Visibility) ((bool) value ? 0 : 2);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) ((Visibility) value == Visibility.Visible);
  }
}
