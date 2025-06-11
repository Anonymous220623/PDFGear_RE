// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Converters.SerarchDelVisibitlityConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace pdfeditor.Utils.Converters;

internal class SerarchDelVisibitlityConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value == null ? (object) Visibility.Collapsed : (object) (Visibility) (((string) value).Trim().Length > 0 ? 0 : 2);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
