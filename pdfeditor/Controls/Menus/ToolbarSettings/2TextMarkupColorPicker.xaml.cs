// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarSettings.ColorIndicatorConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Menus.ToolbarSettings;

public class ColorIndicatorConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (!(value is Color color))
      return (object) null;
    return ((double) color.R * 0.2125999927520752 + (double) color.G * 0.71520000696182251 + (double) color.B * 0.0722000002861023) / (double) byte.MaxValue < 0.65 ? (object) new SolidColorBrush(Colors.White) : (object) new SolidColorBrush(Colors.Black);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
