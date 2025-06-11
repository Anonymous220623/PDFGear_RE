// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ColorPickers.ColorPickerColorBrushConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.ColorPickers;

internal class ColorPickerColorBrushConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    switch (value)
    {
      case Color color:
        return (object) new SolidColorBrush(color);
      case SolidColorBrush solidColorBrush:
        return (object) solidColorBrush;
      default:
        return (object) null;
    }
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    switch (value)
    {
      case SolidColorBrush solidColorBrush:
        return (object) solidColorBrush.Color;
      case Color color:
        return (object) color;
      default:
        return (object) Colors.Black;
    }
  }
}
