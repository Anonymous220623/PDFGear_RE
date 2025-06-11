// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Converters.StringStrokeConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Utils.Converters;

public class StringStrokeConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
      return (object) Brushes.Transparent;
    string str = value.ToString();
    try
    {
      if (str.StartsWith("#"))
      {
        if (str.Length == 9 && str.StartsWith("#00"))
          return parameter.ToString().Equals("Stroke") ? (object) Brushes.Black : (object) Brushes.Red;
        if (str.Equals("#FFFFFFFF", StringComparison.OrdinalIgnoreCase))
          return parameter.ToString().Equals("Stroke") ? (object) Brushes.Black : (object) Brushes.Transparent;
      }
      return (object) Brushes.Transparent;
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
