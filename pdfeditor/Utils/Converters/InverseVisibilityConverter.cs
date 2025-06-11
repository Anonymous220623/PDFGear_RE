// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Converters.InverseVisibilityConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace pdfeditor.Utils.Converters;

internal class InverseVisibilityConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
      return (object) Visibility.Visible;
    if ((Visibility) value != Visibility.Visible)
      return (object) Visibility.Visible;
    return parameter != null && parameter.GetType() == typeof (Visibility) ? parameter : (object) Visibility.Collapsed;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) null;
  }
}
