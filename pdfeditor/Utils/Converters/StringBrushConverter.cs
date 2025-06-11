// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Converters.StringBrushConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Utils.Converters;

public class StringBrushConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
      return (object) Brushes.Transparent;
    string str = value.ToString();
    try
    {
      return str.StartsWith("#") ? (object) new SolidColorBrush((Color) ColorConverter.ConvertFromString(str)) : (object) Brushes.Transparent;
    }
    catch (Exception ex)
    {
      return (object) Brushes.Transparent;
    }
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
