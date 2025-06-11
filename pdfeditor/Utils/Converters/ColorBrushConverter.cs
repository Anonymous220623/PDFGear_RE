// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Converters.ColorBrushConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Utils.Converters;

public class ColorBrushConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    switch (value)
    {
      case Brush brush:
        return (object) brush;
      case string str:
        return ColorConverter.ConvertFromString(str);
      case Color color:
        return (object) new SolidColorBrush(color);
      default:
        return (object) null;
    }
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
