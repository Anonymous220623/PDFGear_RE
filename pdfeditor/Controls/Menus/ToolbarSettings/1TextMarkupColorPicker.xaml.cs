// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarSettings.StringColorConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Menus.ToolbarSettings;

public class StringColorConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value is string str)
    {
      try
      {
        return (object) (Color) ColorConverter.ConvertFromString(str);
      }
      catch
      {
      }
    }
    return (object) Colors.Black;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
