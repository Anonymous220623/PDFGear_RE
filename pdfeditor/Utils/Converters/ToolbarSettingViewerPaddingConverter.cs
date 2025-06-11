// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Converters.ToolbarSettingViewerPaddingConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace pdfeditor.Utils.Converters;

public class ToolbarSettingViewerPaddingConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value == null ? (object) new Thickness(10.0) : (object) new Thickness(10.0, 50.0, 10.0, 10.0);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
